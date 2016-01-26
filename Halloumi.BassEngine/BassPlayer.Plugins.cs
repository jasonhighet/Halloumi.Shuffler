using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Halloumi.BassEngine.Helpers;
using Halloumi.BassEngine.Models;
using Halloumi.BassEngine.Plugins;
using Halloumi.Common.Helpers;
using Un4seen.Bass.AddOn.Vst;
using Un4seen.Bass.AddOn.WaDsp;

namespace Halloumi.BassEngine
{
    public partial class BassPlayer
    {
        /// <summary>
        ///     Returns a collection of all loaded WinAmp plug-ins
        /// </summary>
        public WaPlugin WaPlugin => _speakerOutput.WaPlugin;

        /// <summary>
        ///     Returns a collection of all loaded VST plug-ins
        /// </summary>
        public VstPlugin MainVstPlugin => _speakerOutput.VstPlugin1;

        /// <summary>
        ///     Returns a collection of all loaded VST plug-ins
        /// </summary>
        public VstPlugin MainVstPlugin2 => _speakerOutput.VstPlugin2;

        /// <summary>
        ///     Returns a collection of all loaded trackFX VST plug-ins
        /// </summary>
        public VstPlugin SamplerVstPlugin => _samplerMixer.VstPlugin1;

        /// <summary>
        ///     Returns a collection of all loaded trackFX VST plug-ins
        /// </summary>
        public VstPlugin SamplerVstPlugin2 => _samplerMixer.VstPlugin2;

        /// <summary>
        ///     Returns a collection of all loaded mixer VST plug-ins
        /// </summary>
        public VstPlugin TrackVstPlugin => _trackMixer.VstPlugin1;

        /// <summary>
        ///     Returns a collection of all loaded mixer VST plug-ins
        /// </summary>
        public VstPlugin TrackSendFxvstPlugin => _trackSendFxMixer.VstPlugin1;

        /// <summary>
        ///     Returns a collection of all loaded mixer VST plug-ins
        /// </summary>
        public VstPlugin TrackSendFxvstPlugin2 => _trackSendFxMixer.VstPlugin2;

        /// <summary>
        ///     Gets or sets the WA plug-ins folder.
        /// </summary>
        public string WaPluginsFolder { get; set; }

        /// <summary>
        ///     Gets or sets the VST plug-ins folder.
        /// </summary>
        public string VstPluginsFolder { get; set; }

        public decimal TrackSendFxDelayNotes
        {
            get { return _trackSendFxMixer.DelayNotes; }
            set { _trackSendFxMixer.DelayNotes = value; }
        }

        public decimal SamplerDelayNotes
        {
            get { return _samplerMixer.DelayNotes; }
            set { _samplerMixer.DelayNotes = value; }
        }

        /// <summary>
        ///     Gets the current BPM.
        /// </summary>
        /// <returns>The current BPM</returns>
        public decimal GetCurrentBpm()
        {
            var bpm = 100M;
            if (CurrentTrack != null)
            {
                var trackPosition = GetPositionNoLock();
                var position = trackPosition.Positition;
                if (position < 0) position = 0;

                if (position < CurrentTrack.FullStartLoopLength)
                {
                    bpm = CurrentTrack.StartBpm;
                }
                else
                {
                    var range = CurrentTrack.EndBpm - CurrentTrack.StartBpm;
                    var percentComplete = (decimal) (position/(double) trackPosition.Length);
                    bpm = CurrentTrack.StartBpm + (range*percentComplete);
                }
            }

            bpm = BpmHelper.NormaliseBpm(bpm);
            return bpm;
        }


        /// <summary>
        ///     Loads a WinAmp DSP plug-in and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the WinAmp DSP DLL</param>
        public WaPlugin LoadWaPlugin(string location)
        {
            var playing = (PlayState == PlayState.Playing);
            if (playing) Pause();

            var plugin = _speakerOutput.LoadWaPlugin(location);

            if (playing) Play();

            return plugin;
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        public VstPlugin LoadMainVstPlugin(string location)
        {
            return _speakerOutput.LoadVstPlugin1(location);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        public VstPlugin LoadMainVstPlugin2(string location)
        {
            return _speakerOutput.LoadVstPlugin2(location);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        public VstPlugin LoadSamplerVstPlugin(string location)
        {
            return _samplerMixer.LoadVstPlugin1(location);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        public VstPlugin LoadSamplerVstPlugin2(string location)
        {
            return _samplerMixer.LoadVstPlugin2(location);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        public VstPlugin LoadTracksVstPlugin(string location)
        {
            return _trackMixer.LoadVstPlugin1(location);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        public VstPlugin LoadTrackSendFxvstPlugin(string location)
        {
            return _trackSendFxMixer.LoadVstPlugin1(location);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        public VstPlugin LoadTrackSendFxvstPlugin2(string location)
        {
            return _trackSendFxMixer.LoadVstPlugin2(location);
        }

        /// <summary>
        ///     Mutes the track FX.
        /// </summary>
        public void StopTrackFxSend()
        {
            if (CurrentTrack == null || LastTrackFxTrigger == null || LastTrackFxTriggerTrack == null)
                return;

            DebugHelper.WriteLine("Start StopTrackFXSend");

            _trackSendMixer.SetVolume(0);

            DebugHelper.WriteLine("Calculate TrackFXSend Length");

            var position = AudioStreamHelper.GetPosition(LastTrackFxTriggerTrack);
            var positionSeconds = LastTrackFxTriggerTrack.SamplesToSeconds(position);
            var length = positionSeconds - LastTrackFxTrigger.Start;

            if (length <= 0 || position >= LastTrackFxTriggerTrack.FadeOutStart)
            {
                length = LastTrackFxTriggerTrack.SamplesToSeconds(LastTrackFxTriggerTrack.FadeOutStart) -
                         LastTrackFxTrigger.Start;
            }

            LastTrackFxTrigger.Length = length;

            DebugHelper.WriteLine("End StopTrackFXSend");
        }

        /// <summary>
        ///     Silences the track FX.
        /// </summary>
        public void StartTrackFxSend()
        {
            _trackSendMixer.SetVolume(50M);
            if (CurrentTrack == null) return;

            LastTrackFxTriggerTrack = CurrentTrack;

            var position = AudioStreamHelper.GetPosition(LastTrackFxTriggerTrack);
            LastTrackFxTrigger = new TrackFXTrigger
            {
                Start = LastTrackFxTriggerTrack.SamplesToSeconds(position),
                DelayNotes = TrackSendFxDelayNotes
            };

            _trackSendMixer.SetPluginBpm();
        }

        /// <summary>
        ///     Determines whether audio is being sent to the track FX.
        /// </summary>
        /// <returns>True if audio is being sent to the track FX; otherwise, false.</returns>
        public bool IsTrackFxSending()
        {
            return (_trackSendMixer.GetVolume() != 0M);
        }

        /// <summary>
        ///     Gets the sample mixer volume.
        /// </summary>
        /// <returns>A value between 0 and 100</returns>
        public decimal GetTrackSendFxVolume()
        {
            return _trackSendFxMixer.GetVolume();
        }

        /// <summary>
        ///     Sets the sample mixer volume.
        /// </summary>
        /// <param name="volume">The volume as a value between 0 and 100.</param>
        public void SetTrackSendFxMixerVolume(decimal volume)
        {
            _trackSendFxMixer.SetVolume(volume);
        }

        /// <summary>
        ///     Returns a list of all available (unloaded) VST plug-ins
        /// </summary>
        /// <returns>A list of all available (unloaded) VST plug-ins</returns>
        public List<VstPlugin> FindVstPlugins()
        {
            if (!Directory.Exists(VstPluginsFolder)) return new List<VstPlugin>();

            return Directory
                .GetFiles(VstPluginsFolder, "*.dll", SearchOption.AllDirectories)
                .Select(filename => new VstPlugin
                {
                    Location = filename,
                    Name = GetPluginNameFromFileName(filename)
                }).ToList();
        }

        /// <summary>
        ///     Clears the main VST plug-in.
        /// </summary>
        public void ClearMainVstPlugin()
        {
            _speakerOutput.ClearVstPlugin1();
        }

        /// <summary>
        ///     Clears the main VST plug-in.
        /// </summary>
        public void ClearMainVstPlugin2()
        {
            _speakerOutput.ClearVstPlugin2();
        }

        /// <summary>
        ///     Clears the trackFX VST plug-in.
        /// </summary>
        public void ClearSamplerVstPlugin()
        {
            _samplerMixer.ClearVstPlugin1();
        }

        /// <summary>
        ///     Clears the sampler VST plug-in 2.
        /// </summary>
        public void ClearSamplerVstPlugin2()
        {
            _samplerMixer.ClearVstPlugin2();
        }

        /// <summary>
        ///     Clears the tracks VST plug-in.
        /// </summary>
        public void ClearTracksVstPlugin()
        {
            _trackMixer.ClearVstPlugin1();
        }

        /// <summary>
        ///     Clears the tracks VST plug-in.
        /// </summary>
        public void ClearTrackSendFxvstPlugin()
        {
            _trackSendFxMixer.ClearVstPlugin1();
        }

        /// <summary>
        ///     Clears the trackFX VST plug-in 2.
        /// </summary>
        public void ClearTrackSendFxvstPlugin2()
        {
            _trackSendFxMixer.ClearVstPlugin2();
        }

        /// <summary>
        ///     Unloads all loaded VST plug-ins.
        /// </summary>
        public void UnloadAllVstPlugins()
        {
            ClearMainVstPlugin();
            ClearMainVstPlugin2();
            ClearSamplerVstPlugin();
            ClearSamplerVstPlugin2();
            ClearTracksVstPlugin();
            ClearTrackSendFxvstPlugin();
            ClearTrackSendFxvstPlugin2();
        }

        /// <summary>
        ///     Unloads all loaded WinAmp plug-ins.
        /// </summary>
        public void UnloadAllWaPlugins()
        {
            _speakerOutput.ClearWaPlugin();
        }

        /// <summary>
        ///     Shows the WinAmp DSP plug-in configuration screen
        /// </summary>
        /// <param name="plugin">The WinAmp DSP plug-in.</param>
        public void ShowWaPluginConfig(WaPlugin plugin)
        {
            if (plugin == null) return;

            BassWaDsp.BASS_WADSP_Config(plugin.Id);
        }

        /// <summary>
        ///     Shows the VST plug-in configuration screen.
        /// </summary>
        /// <param name="plugin">The plug-in.</param>
        public void ShowVstPluginConfig(VstPlugin plugin)
        {
            if (plugin == null) return;

            SetDelayByBpm();

            if (plugin.Form != null && !plugin.Form.IsDisposed)
            {
                plugin.Form.Show();
                plugin.Form.BringToFront();
                return;
            }

            plugin.Form = null;

            var containerForm = new VstPluginConfigForm(plugin, this);
            containerForm.Show();
        }

        /// <summary>
        ///     Returns a list of all available (unloaded) WinAmp DSP plug-ins
        /// </summary>
        /// <returns>A list of all available (unloaded) WinAmp DSP plug-ins</returns>
        public List<WaPlugin> FindWaPlugins()
        {
            var plugins = new List<WaPlugin>();
            if (WaPluginsFolder == "") return plugins;

            plugins = Directory.GetFiles(WaPluginsFolder, "dsp_*.dll", SearchOption.AllDirectories)
                .Select(filename => new WaPlugin
                {
                    Location = filename,
                    Name = GetPluginNameFromFileName(filename)
                }).ToList();

            return plugins;
        }

        private static string GetPluginNameFromFileName(string pluginLocation)
        {
            return StringHelper.TitleCase((Path.GetFileNameWithoutExtension(pluginLocation) + "")
                .Replace("dsp_", "")
                .Replace("_", " ")
                .ToLower());
        }

        /// <summary>
        ///     Gets the VST plug-in settings.
        /// </summary>
        /// <param name="plugin">The plug-in.</param>
        /// <returns>The settings as a key=value comma delimited list</returns>
        public string GetVstPluginParameters(VstPlugin plugin)
        {
            var values = new List<string>();
            var parameterCount = BassVst.BASS_VST_GetParamCount(plugin.Id);
            for (var i = 0; i < parameterCount; i++)
            {
                var value = BassVst.BASS_VST_GetParam(plugin.Id, i);
                values.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            return string.Join(",", values.ToArray());
        }

        /// <summary>
        ///     Sets the VST plug-in settings.
        /// </summary>
        /// <param name="plugin">The plug-in.</param>
        /// <param name="parameters">The parameters.</param>
        public void SetVstPluginParameters(VstPlugin plugin, string parameters)
        {
            if (parameters.Trim() == "") return;
            var values = parameters.Split(',').ToList();
            var parameterCount = BassVst.BASS_VST_GetParamCount(plugin.Id);
            for (var i = 0; i < parameterCount; i++)
            {
                try
                {
                    if (i >= values.Count) continue;
                    var value = float.Parse(values[i]);
                    BassVst.BASS_VST_SetParam(plugin.Id, i, value);
                }
                catch
                {
                    // ignored
                }
            }

            SetDelayByBpm();
        }


        /// <summary>
        ///     Sets the delay by BPM.
        /// </summary>
        private void SetDelayByBpm()
        {
            _trackSendFxMixer.SetPluginBpm();
            _samplerMixer.SetPluginBpm();
        }

        public void SetVstPluginPreset(VstPlugin plugin, int presetIndex)
        {
            BassVst.BASS_VST_SetProgram(plugin.Id, presetIndex);
            SetDelayByBpm();
        }
    }
}