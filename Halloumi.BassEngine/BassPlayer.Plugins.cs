using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Halloumi.Common.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.AddOn.Vst;
using Un4seen.Bass.AddOn.WaDsp;

namespace Halloumi.BassEngine
{
    public partial class BassPlayer
    {
        #region Private Variables

        /// <summary>
        /// A collection of all loaded winamp plugins
        /// </summary>
        private List<WAPlugin> _waPlugins = new List<WAPlugin>();

        #endregion

        #region Properties

        /// <summary>
        /// Returns a collection of all loaded Winamp Plugins
        /// </summary>
        public WAPlugin WAPlugin
        {
            get { return this.SpeakerOutput.WAPlugin; }
        }

        /// <summary>
        /// Returns a collection of all loaded VST Plugins
        /// </summary>
        public VSTPlugin MainVSTPlugin
        {
            get { return this.SpeakerOutput.VSTPlugin1; }
        }

        /// <summary>
        /// Returns a collection of all loaded VST Plugins
        /// </summary>
        public VSTPlugin MainVSTPlugin2
        {
            get { return this.SpeakerOutput.VSTPlugin2; }
        }

        /// <summary>
        /// Returns a collection of all loaded trackFX VST Plugins
        /// </summary>
        public VSTPlugin SamplerVSTPlugin
        {
            get { return this.SamplerMixer.VSTPlugin1; }
        }

        /// <summary>
        /// Returns a collection of all loaded trackFX VST Plugins
        /// </summary>
        public VSTPlugin SamplerVSTPlugin2
        {
            get { return this.SamplerMixer.VSTPlugin2; }
        }

        /// <summary>
        /// Returns a collection of all loaded mixer VST Plugins
        /// </summary>
        public VSTPlugin TrackVSTPlugin
        {
            get { return TrackMixer.VSTPlugin1; }
        }

        /// <summary>
        /// Returns a collection of all loaded mixer VST Plugins
        /// </summary>
        public VSTPlugin TrackSendFXVSTPlugin
        {
            get { return TrackSendFXMixer.VSTPlugin1; }
        }

        /// <summary>
        /// Returns a collection of all loaded mixer VST Plugins
        /// </summary>
        public VSTPlugin TrackSendFXVSTPlugin2
        {
            get { return TrackSendFXMixer.VSTPlugin2; }
        }

        /// <summary>
        /// Gets or sets the WA plugins folder.
        /// </summary>
        public string WAPluginsFolder { get; set; }

        /// <summary>
        /// Gets or sets the VST plugins folder.
        /// </summary>
        public string VSTPluginsFolder { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads a winamp DSP plugin and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the winamp dsp dll</param>
        public WAPlugin LoadWAPlugin(string location)
        {
            //if (location == "") return null;

            //if (!location.Contains("\\")) location = Path.Combine(this.WAPluginsFolder, location);
            //if (!location.EndsWith(".dll")) location += ".dll";

            //if (!File.Exists(location)) return null;

            var playing = (this.PlayState == PlayState.Playing);
            if (playing) this.Pause();

            var plugin = SpeakerOutput.LoadWAPlugin(location);

            if (playing) this.Play();

            return plugin;
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        public VSTPlugin LoadMainVSTPlugin(string location)
        {
            return this.SpeakerOutput.LoadVSTPlugin1(location);
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        public VSTPlugin LoadMainVSTPlugin2(string location)
        {
            return this.SpeakerOutput.LoadVSTPlugin2(location);
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        public VSTPlugin LoadSamplerVSTPlugin(string location)
        {
            return this.SamplerMixer.LoadVSTPlugin1(location);
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        public VSTPlugin LoadSamplerVSTPlugin2(string location)
        {
            return this.SamplerMixer.LoadVSTPlugin2(location);
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        public VSTPlugin LoadTracksVSTPlugin(string location)
        {
            return TrackMixer.LoadVSTPlugin1(location);
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        public VSTPlugin LoadTrackSendFXVSTPlugin(string location)
        {
            return TrackSendFXMixer.LoadVSTPlugin1(location);
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        public VSTPlugin LoadTrackSendFXVSTPlugin2(string location)
        {
            return TrackSendFXMixer.LoadVSTPlugin2(location);
        }

        /// <summary>
        /// Mutes the track FX.
        /// </summary>
        public void StopTrackFXSend()
        {
            DebugHelper.WriteLine("Start StopTrackFXSend");

            this.TrackSendMixer.SetVolume(0);

            if (this.CurrentTrack != null
                && this.LastTrackFXTrigger != null
                && this.LastTrackFXTriggerTrack != null)
            {
                DebugHelper.WriteLine("Calculate TrackFXSend Length");

                var position = BassHelper.GetTrackPosition(this.LastTrackFXTriggerTrack);
                var positionSeconds = this.LastTrackFXTriggerTrack.SamplesToSeconds(position);
                var length = positionSeconds - this.LastTrackFXTrigger.Start;

                if (length <= 0 || position >= this.LastTrackFXTriggerTrack.FadeOutStart)
                {
                    length = LastTrackFXTriggerTrack.SamplesToSeconds(this.LastTrackFXTriggerTrack.FadeOutStart) - this.LastTrackFXTrigger.Start;
                }

                this.LastTrackFXTrigger.Length = length;
            }

            DebugHelper.WriteLine("End StopTrackFXSend");
        }

        /// <summary>
        /// Unmutes the track FX.
        /// </summary>
        public void StartTrackFXSend()
        {
            TrackSendMixer.SetVolume(50M);
            if (this.CurrentTrack == null) return;

            this.LastTrackFXTriggerTrack = this.CurrentTrack;

            var position = BassHelper.GetTrackPosition(this.LastTrackFXTriggerTrack);
            this.LastTrackFXTrigger = new TrackFXTrigger();
            this.LastTrackFXTrigger.Start = this.LastTrackFXTriggerTrack.SamplesToSeconds(position);
            this.LastTrackFXTrigger.DelayNotes = this.TrackSendFXDelayNotes;

            TrackSendMixer.SetPluginBPM();
        }

        /// <summary>
        /// Determines whether audio is being sent to the track FX.
        /// </summary>
        /// <returns>True if audio is being sent to the track FX; otherwise, false.</returns>
        public bool IsTrackFXSending()
        {
            return (this.TrackSendMixer.GetVolume() != 0M);
        }

        /// <summary>
        /// Gets the sample mixer volume.
        /// </summary>
        /// <returns>A value between 0 and 100</returns>
        public decimal GetTrackSendFXVolume()
        {
            return TrackSendFXMixer.GetVolume();
        }

        /// <summary>
        /// Sets the sample mixer volume.
        /// </summary>
        /// <param name="volume">The volume as a value between 0 and 100.</param>
        public void SetTrackSendFXMixerVolume(decimal volume)
        {
            TrackSendFXMixer.SetVolume(volume);
        }

        /// <summary>
        /// Returns a list of all available (unloaded) VST plugins
        /// </summary>
        /// <returns>A list of all available (unloaded) VST plugins</returns>
        public List<VSTPlugin> FindVSTPlugins()
        {
            List<VSTPlugin> plugins = new List<VSTPlugin>();
            if (Directory.Exists(this.VSTPluginsFolder))
            {
                foreach (string pluginLocation in Directory.GetFiles(this.VSTPluginsFolder, "*.dll", SearchOption.AllDirectories))
                {
                    VSTPlugin plugin = new VSTPlugin();
                    plugin.Location = pluginLocation;
                    plugin.Name = Path.GetFileNameWithoutExtension(pluginLocation);
                    plugin.Name = plugin.Name.Replace("_", " ");
                    plugin.Name = StringHelper.TitleCase(plugin.Name);
                    plugins.Add(plugin);
                }
            }
            return plugins;
        }

        /// <summary>
        /// Clears the main VST plugin.
        /// </summary>
        public void ClearMainVSTPlugin()
        {
            this.SpeakerOutput.ClearVSTPlugin1();
        }

        /// <summary>
        /// Clears the main VST plugin.
        /// </summary>
        public void ClearMainVSTPlugin2()
        {
            this.SpeakerOutput.ClearVSTPlugin2();
        }

        /// <summary>
        /// Clears the trackFX VST plugin.
        /// </summary>
        public void ClearSamplerVSTPlugin()
        {
            this.SamplerMixer.ClearVSTPlugin1();
        }

        /// <summary>
        /// Clears the sampler VST plugin 2.
        /// </summary>
        public void ClearSamplerVSTPlugin2()
        {
            this.SamplerMixer.ClearVSTPlugin2();
        }

        /// <summary>
        /// Clears the tracks VST plugin.
        /// </summary>
        public void ClearTracksVSTPlugin()
        {
            this.TrackMixer.ClearVSTPlugin1();
        }

        /// <summary>
        /// Clears the tracks VST plugin.
        /// </summary>
        public void ClearTrackSendFXVSTPlugin()
        {
            this.TrackSendFXMixer.ClearVSTPlugin1();
        }

        /// <summary>
        /// Clears the trackFX VST plugin 2.
        /// </summary>
        public void ClearTrackSendFXVSTPlugin2()
        {
            this.TrackSendFXMixer.ClearVSTPlugin2();
        }

        /// <summary>
        /// Unloads all loaded VST plugins.
        /// </summary>
        public void UnloadAllVSTPlugins()
        {
            ClearMainVSTPlugin();
            ClearMainVSTPlugin2();
            ClearSamplerVSTPlugin();
            ClearSamplerVSTPlugin2();
            ClearTracksVSTPlugin();
            ClearTrackSendFXVSTPlugin();
            ClearTrackSendFXVSTPlugin2();
        }

        /// <summary>
        /// Unloads all loaded WinAmp plugins.
        /// </summary>
        public void UnloadAllWAPlugins()
        {
            this.SpeakerOutput.ClearWAPlugin();
        }

        /// <summary>
        /// Shows the WinAmp DSP plugin config screen
        /// </summary>
        /// <param name="plugin">The WinAmp DSP plugin.</param>
        public void ShowWAPluginConfig(WAPlugin plugin)
        {
            if (plugin == null) return;

            BassWaDsp.BASS_WADSP_Config(plugin.ID);
        }

        /// <summary>
        /// Shows the VST plugin config screen.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        public void ShowVSTPluginConfig(VSTPlugin plugin)
        {
            if (plugin == null) return;

            SetDelayByBPM();

            if (plugin.Form != null && !plugin.Form.IsDisposed)
            {
                plugin.Form.Show();
                plugin.Form.BringToFront();
                return;
            }

            plugin.Form = null;

            VSTPluginConfigForm containerForm = new VSTPluginConfigForm(plugin, this);
            containerForm.Show();
        }

        /// <summary>
        /// Returns a list of all available (unloaded) WinAmp DSP plugins
        /// </summary>
        /// <returns>A list of all available (unloaded) WinAmp DSP plugins</returns>
        public List<WAPlugin> FindWAPlugins()
        {
            List<WAPlugin> plugins = new List<WAPlugin>();
            if (this.WAPluginsFolder == "") return plugins;

            foreach (string pluginLocation in Directory.GetFiles(this.WAPluginsFolder, "dsp_*.dll", SearchOption.AllDirectories))
            {
                WAPlugin plugin = new WAPlugin();
                plugin.Location = pluginLocation;
                plugin.Name = Path.GetFileNameWithoutExtension(pluginLocation);
                plugin.Name = plugin.Name.Replace("dsp_", "");
                plugin.Name = plugin.Name.Replace("_", " ");
                plugin.Name = StringHelper.TitleCase(plugin.Name.ToLower());
                plugins.Add(plugin);
            }

            return plugins;
        }

        /// <summary>
        /// Gets the VST plugin settings.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <returns>The settings as a key=value comma delimeted list</returns>
        public string GetVstPluginParameters(VSTPlugin plugin)
        {
            var values = new List<string>();
            var parameterCount = BassVst.BASS_VST_GetParamCount(plugin.ID);
            for (int i = 0; i < parameterCount; i++)
            {
                var value = BassVst.BASS_VST_GetParam(plugin.ID, i);
                values.Add(value.ToString());
            }
            return string.Join(",", values.ToArray());
        }

        /// <summary>
        /// Sets the VST plugin settings.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="settings">The settings as a key=value comma delimeted list.</param>
        public void SetVstPluginParameters(VSTPlugin plugin, string parameters)
        {
            if (parameters.Trim() == "") return;
            var values = parameters.Split(',').ToList();
            var parameterCount = BassVst.BASS_VST_GetParamCount(plugin.ID);
            for (int i = 0; i < parameterCount; i++)
            {
                try
                {
                    if (i >= values.Count) continue;
                    var value = float.Parse(values[i]);
                    BassVst.BASS_VST_SetParam(plugin.ID, i, value);
                }
                catch { }
            }

            SetDelayByBPM();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the delay by BPM.
        /// </summary>
        private void SetDelayByBPM()
        {
            this.TrackSendFXMixer.SetPluginBPM();
            this.SamplerMixer.SetPluginBPM();
        }

        public decimal GetCurrentBPM()
        {
            var bpm = 100M;
            if (this.CurrentTrack != null)
            {
                bpm = this.CurrentTrack.BPM;

                //if (!this.IsLocked())
                {
                    var trackPosition = GetPositionNoLock();

                    var position = trackPosition.Positition;
                    if (position < 0) position = 0;

                    if (position < this.CurrentTrack.FullStartLoopLength)
                    {
                        bpm = this.CurrentTrack.StartBPM;
                    }
                    else
                    {
                        var range = this.CurrentTrack.EndBPM - this.CurrentTrack.StartBPM;
                        var percentComplete = (decimal)((double)position / (double)trackPosition.Length);
                        bpm = this.CurrentTrack.StartBPM + (range * percentComplete);
                    }
                }
            }

            bpm = BassHelper.NormaliseBPM(bpm);
            return bpm;
        }

        public decimal TrackSendFXDelayNotes
        {
            get { return TrackSendFXMixer.DelayNotes; }
            set { TrackSendFXMixer.DelayNotes = value; }
        }

        public decimal SamplerDelayNotes
        {
            get { return SamplerMixer.DelayNotes; }
            set { SamplerMixer.DelayNotes = value; }
        }

        public void SetVSTPluginPreset(VSTPlugin plugin, int presetIndex)
        {
            BassVst.BASS_VST_SetProgram(plugin.ID, presetIndex);
            SetDelayByBPM();
        }

        #endregion
    }
}