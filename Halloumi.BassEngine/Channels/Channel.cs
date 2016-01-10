using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Halloumi.Common.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.AddOn.Vst;
using Un4seen.Bass.AddOn.WaDsp;

namespace Halloumi.BassEngine.Channels
{
    public abstract class Channel
    {
        public IBMPProvider BpmProvider { get; private set; }

        internal int InternalChannel { get; set; }

        public Channel(IBMPProvider bpmProvider)
        {
            BpmProvider = bpmProvider;
        }

        public void AddInputChannel(MixerChannel inputChannel)
        {
            if (inputChannel.OutputType == MixerChannelOutputType.SingleOutput)
            {
                BassHelper.AddChannelToDecoderMixer(this.InternalChannel, inputChannel.InternalChannel);
            }
            else if (inputChannel.OutputType == MixerChannelOutputType.MultipleOutputs)
            {
                var splitOutputChannel = BassHelper.SplitDecoderMixer(inputChannel.InternalChannel);
                BassHelper.AddChannelToDecoderMixer(this.InternalChannel, splitOutputChannel);
            }
        }

        /// <summary>
        /// Sets the volume.
        /// </summary>
        /// <param name="volume">A value between 0 and 100</param>
        public void SetVolume(decimal volume)
        {
            BassHelper.SetVolume(this.InternalChannel, volume);
        }

        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <returns>A value between 0 and 100</returns>
        public decimal GetVolume()
        {
            return BassHelper.GetVolume(this.InternalChannel);
        }

        /// <summary>
        /// Gets the current volume levels.
        /// </summary>
        /// <returns>A VolumeLevels object containing the left and right volume levels (0 - 32768)</returns>
        public VolumeLevels GetVolumeLevels()
        {
            var levels = new VolumeLevels();
            int level = Bass.BASS_ChannelGetLevel(this.InternalChannel);
            levels.Left = Utils.LowWord32(level);
            levels.Right = Utils.HighWord32(level);
            return levels;
        }

        public VSTPlugin LoadVSTPlugin1(string location)
        {
            if (this.VSTPlugin1 == null || this.VSTPlugin1.Location != location)
            {
                ClearVSTPlugin1();
                this.VSTPlugin1 = LoadVSTPlugin(location, 1);
                SetPluginBPM(this.VSTPlugin1);
            }
            return this.VSTPlugin1;
        }

        public VSTPlugin LoadVSTPlugin2(string location)
        {
            if (this.VSTPlugin2 == null || this.VSTPlugin2.Location != location)
            {
                ClearVSTPlugin2();
                this.VSTPlugin2 = LoadVSTPlugin(location, 2);
                SetPluginBPM(this.VSTPlugin2);
            }
            return this.VSTPlugin1;
        }

        public void ClearVSTPlugin1()
        {
            RemoveVSTPlugin(this.VSTPlugin1);
            this.VSTPlugin1 = null;
        }

        public void ClearVSTPlugin2()
        {
            RemoveVSTPlugin(this.VSTPlugin2);
            this.VSTPlugin2 = null;
        }

        public VSTPlugin VSTPlugin1
        {
            get;
            internal set;
        }

        public VSTPlugin VSTPlugin2
        {
            get;
            internal set;
        }

        private void RemoveVSTPlugin(VSTPlugin plugin)
        {
            if (plugin == null) return;
            DebugHelper.WriteLine("Unload plugin" + plugin.Name);
            if (plugin.Form != null)
            {
                if (!plugin.Form.IsDisposed)
                {
                    plugin.Form.CanClose = true;
                    plugin.Form.Close();
                    plugin.Form.Dispose();
                    plugin.Form = null;
                }
            }

            BassVst.BASS_VST_ChannelRemoveDSP(this.InternalChannel, plugin.ID);
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        private VSTPlugin LoadVSTPlugin(string location, int priority)
        {
            if (location == "") return null;

            //if (!location.Contains("\\")) location = Path.Combine(this.VSTPluginsFolder + "", location);
            //if (!location.EndsWith(".dll")) location += ".dll";

            if (!File.Exists(location)) return null;

            //BassMix.BASS_Mixer_ChannelPause(this.InternalChannel);

            VSTPlugin plugin = new VSTPlugin();
            plugin.ID = BassVst.BASS_VST_ChannelSetDSP(this.InternalChannel, location, BASSVSTDsp.BASS_VST_DEFAULT, priority);
            if (plugin.ID == 0) throw new Exception("Cannot load plugin " + Path.GetFileNameWithoutExtension(location));

            BASS_VST_INFO info = BassVst.BASS_VST_GetInfo(plugin.ID);
            if (info != null)
            {
                plugin.Name = info.effectName;
                plugin.EditorWidth = info.editorWidth;
                plugin.EditorHeight = info.editorHeight;
            }
            else
            {
                plugin.Name = StringHelper.TitleCase(Path.GetFileNameWithoutExtension(location).Replace("_", " "));
            }

            if (plugin.EditorWidth == 0) plugin.EditorWidth = 400;
            if (plugin.EditorHeight == 0) plugin.EditorHeight = 600;

            plugin.Location = location;

            SetPluginBPM(plugin);
            //BassMix.BASS_Mixer_ChannelPlay(this.InternalChannel);

            return plugin;
        }

        private decimal _delayNotes = 0.25M;

        public decimal DelayNotes
        {
            get { return _delayNotes; }
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _delayNotes = value;
                    SetPluginBPM();
                }
            }
        }

        /// <summary>
        /// Sets the delay by BPM.
        /// </summary>
        public void SetPluginBPM()
        {
            lock (this)
            {
                SetPluginBPM(this.VSTPlugin1);
                SetPluginBPM(this.VSTPlugin2);
            }
        }

        private void SetPluginBPM(VSTPlugin plugin)
        {
            if (plugin == null)
                return;
            if (this.BpmProvider == null)
                return;

            if (!plugin.Name.ToLower().Contains("delay"))
                return;

            var parameterCount = BassVst.BASS_VST_GetParamCount(plugin.ID);
            for (int i = 0; i < parameterCount; i++)
            {
                var info = BassVst.BASS_VST_GetParamInfo(plugin.ID, i);
                if (info.name.ToLower() != "time") continue;
                
                var bpm = this.BpmProvider.GetCurrentBPM();
                var quarterNoteDelayLength = BassHelper.GetDefaultDelayLength(bpm);

                if (_delayNotes == 0)
                {
                    BassVst.BASS_VST_SetBypass(plugin.ID, true);
                }
                else
                {
                    BassVst.BASS_VST_SetBypass(plugin.ID, false);
                    var tapeDelayValue = GetTapeDelayValue(quarterNoteDelayLength * ((double)_delayNotes / 0.25));
                    BassVst.BASS_VST_SetParam(plugin.ID, i, tapeDelayValue);
                }
                
            }
        }

        /// <summary>
        /// Gets the tape delay value.
        /// </summary>
        /// <param name="delayLength">Length of the delay.</param>
        /// <returns>The tape delay value</returns>
        private float GetTapeDelayValue(double delayLength)
        {
            var minMS = 60;
            var maxMS = 1500;
            if (delayLength < minMS) delayLength = minMS;
            if (delayLength > maxMS) delayLength = maxMS;

            return (float)((delayLength - minMS) / (maxMS - minMS));
        }

        /// <summary>
        /// Set to true once the winamp dsp engine has been intitalised
        /// </summary>
        private static bool _waDSPLoaded = false;

        public WAPlugin WAPlugin { get; private set; }

        /// <summary>
        /// Loads a winamp DSP plugin and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the winamp dsp dll</param>
        public WAPlugin LoadWAPlugin(string location)
        {
            if (location == "") return null;

            //if (!location.Contains("\\")) location = Path.Combine(this.WAPluginsFolder, location);
            //if (!location.EndsWith(".dll")) location += ".dll";

            if (this.WAPlugin != null && this.WAPlugin.Location == location) return this.WAPlugin;

            if (!File.Exists(location)) return null;

            BassMix.BASS_Mixer_ChannelPause(this.InternalChannel);

            if (!_waDSPLoaded) StartWADSPEngine();

            DebugHelper.WriteLine("Load WAPlugin " + location);

            WAPlugin plugin = new WAPlugin();

            plugin.ID = BassWaDsp.BASS_WADSP_Load(location, 10, 10, 300, 300, null);
            plugin.Module = 0;
            plugin.Name = BassWaDsp.BASS_WADSP_GetName(plugin.ID);
            plugin.Location = location;
            BassWaDsp.BASS_WADSP_Start(plugin.ID, plugin.Module, this.InternalChannel);
            BassWaDsp.BASS_WADSP_ChannelSetDSP(plugin.ID, this.InternalChannel, 1);

            this.WAPlugin = plugin;

            BassMix.BASS_Mixer_ChannelPlay(this.InternalChannel);

            return plugin;
        }

        public void ClearWAPlugin()
        {
            if (this.WAPlugin == null)
                return;

            DebugHelper.WriteLine("Unload plugin" + this.WAPlugin.Name);
            try
            {
                BassWaDsp.BASS_WADSP_Stop(this.WAPlugin.ID);
            }
            catch { }

            try
            {
                BassWaDsp.BASS_WADSP_ChannelRemoveDSP(this.WAPlugin.ID);
            }
            catch
            { }

            try
            {
                BassWaDsp.BASS_WADSP_FreeDSP(this.WAPlugin.ID);
            }
            catch
            { }

            this.WAPlugin = null;
        }

        /// <summary>
        /// Intialises the winamp dsp engine
        /// </summary>
        private void StartWADSPEngine()
        {
            DebugHelper.WriteLine("StartWADSPEngine");
            //BassWaDsp.BASS_WADSP_Init(_windowHandle);
            BassWaDsp.BASS_WADSP_Init(IntPtr.Zero);
            _waDSPLoaded = true;
        }
    }

    public interface IBMPProvider
    {
        decimal GetCurrentBPM();
    }
}