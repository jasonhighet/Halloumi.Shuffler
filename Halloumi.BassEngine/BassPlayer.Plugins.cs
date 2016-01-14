using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Halloumi.BassEngine.Plugins;
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
        private List<WaPlugin> _waPlugins = new List<WaPlugin>();

        #endregion

        #region Properties

        /// <summary>
        /// Returns a collection of all loaded Winamp Plugins
        /// </summary>
        public WaPlugin WaPlugin
        {
            get { return this._speakerOutput.WaPlugin; }
        }

        /// <summary>
        /// Returns a collection of all loaded VST Plugins
        /// </summary>
        public VstPlugin MainVstPlugin
        {
            get { return this._speakerOutput.VstPlugin1; }
        }

        /// <summary>
        /// Returns a collection of all loaded VST Plugins
        /// </summary>
        public VstPlugin MainVstPlugin2
        {
            get { return this._speakerOutput.VstPlugin2; }
        }

        /// <summary>
        /// Returns a collection of all loaded trackFX VST Plugins
        /// </summary>
        public VstPlugin SamplerVstPlugin
        {
            get { return this._samplerMixer.VstPlugin1; }
        }

        /// <summary>
        /// Returns a collection of all loaded trackFX VST Plugins
        /// </summary>
        public VstPlugin SamplerVstPlugin2
        {
            get { return this._samplerMixer.VstPlugin2; }
        }

        /// <summary>
        /// Returns a collection of all loaded mixer VST Plugins
        /// </summary>
        public VstPlugin TrackVstPlugin
        {
            get { return _trackMixer.VstPlugin1; }
        }

        /// <summary>
        /// Returns a collection of all loaded mixer VST Plugins
        /// </summary>
        public VstPlugin TrackSendFxvstPlugin
        {
            get { return _trackSendFxMixer.VstPlugin1; }
        }

        /// <summary>
        /// Returns a collection of all loaded mixer VST Plugins
        /// </summary>
        public VstPlugin TrackSendFxvstPlugin2
        {
            get { return _trackSendFxMixer.VstPlugin2; }
        }

        /// <summary>
        /// Gets or sets the WA plugins folder.
        /// </summary>
        public string WaPluginsFolder { get; set; }

        /// <summary>
        /// Gets or sets the VST plugins folder.
        /// </summary>
        public string VstPluginsFolder { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads a winamp DSP plugin and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the winamp dsp dll</param>
        public WaPlugin LoadWaPlugin(string location)
        {
            //if (location == "") return null;

            //if (!location.Contains("\\")) location = Path.Combine(this.WAPluginsFolder, location);
            //if (!location.EndsWith(".dll")) location += ".dll";

            //if (!File.Exists(location)) return null;

            var playing = (this.PlayState == PlayState.Playing);
            if (playing) this.Pause();

            var plugin = _speakerOutput.LoadWaPlugin(location);

            if (playing) this.Play();

            return plugin;
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        public VstPlugin LoadMainVstPlugin(string location)
        {
            return this._speakerOutput.LoadVstPlugin1(location);
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        public VstPlugin LoadMainVstPlugin2(string location)
        {
            return this._speakerOutput.LoadVstPlugin2(location);
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        public VstPlugin LoadSamplerVstPlugin(string location)
        {
            return this._samplerMixer.LoadVstPlugin1(location);
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        public VstPlugin LoadSamplerVstPlugin2(string location)
        {
            return this._samplerMixer.LoadVstPlugin2(location);
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        public VstPlugin LoadTracksVstPlugin(string location)
        {
            return _trackMixer.LoadVstPlugin1(location);
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        public VstPlugin LoadTrackSendFxvstPlugin(string location)
        {
            return _trackSendFxMixer.LoadVstPlugin1(location);
        }

        /// <summary>
        /// Loads a VST plugin and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the vst dll</param>
        public VstPlugin LoadTrackSendFxvstPlugin2(string location)
        {
            return _trackSendFxMixer.LoadVstPlugin2(location);
        }

        /// <summary>
        /// Mutes the track FX.
        /// </summary>
        public void StopTrackFxSend()
        {
            DebugHelper.WriteLine("Start StopTrackFXSend");

            this._trackSendMixer.SetVolume(0);

            if (this.CurrentTrack != null
                && this.LastTrackFxTrigger != null
                && this.LastTrackFxTriggerTrack != null)
            {
                DebugHelper.WriteLine("Calculate TrackFXSend Length");

                var position = BassHelper.GetTrackPosition(this.LastTrackFxTriggerTrack);
                var positionSeconds = this.LastTrackFxTriggerTrack.SamplesToSeconds(position);
                var length = positionSeconds - this.LastTrackFxTrigger.Start;

                if (length <= 0 || position >= this.LastTrackFxTriggerTrack.FadeOutStart)
                {
                    length = LastTrackFxTriggerTrack.SamplesToSeconds(this.LastTrackFxTriggerTrack.FadeOutStart) - this.LastTrackFxTrigger.Start;
                }

                this.LastTrackFxTrigger.Length = length;
            }

            DebugHelper.WriteLine("End StopTrackFXSend");
        }

        /// <summary>
        /// Unmutes the track FX.
        /// </summary>
        public void StartTrackFxSend()
        {
            _trackSendMixer.SetVolume(50M);
            if (this.CurrentTrack == null) return;

            this.LastTrackFxTriggerTrack = this.CurrentTrack;

            var position = BassHelper.GetTrackPosition(this.LastTrackFxTriggerTrack);
            this.LastTrackFxTrigger = new TrackFxTrigger();
            this.LastTrackFxTrigger.Start = this.LastTrackFxTriggerTrack.SamplesToSeconds(position);
            this.LastTrackFxTrigger.DelayNotes = this.TrackSendFxDelayNotes;

            _trackSendMixer.SetPluginBpm();
        }

        /// <summary>
        /// Determines whether audio is being sent to the track FX.
        /// </summary>
        /// <returns>True if audio is being sent to the track FX; otherwise, false.</returns>
        public bool IsTrackFxSending()
        {
            return (this._trackSendMixer.GetVolume() != 0M);
        }

        /// <summary>
        /// Gets the sample mixer volume.
        /// </summary>
        /// <returns>A value between 0 and 100</returns>
        public decimal GetTrackSendFxVolume()
        {
            return _trackSendFxMixer.GetVolume();
        }

        /// <summary>
        /// Sets the sample mixer volume.
        /// </summary>
        /// <param name="volume">The volume as a value between 0 and 100.</param>
        public void SetTrackSendFxMixerVolume(decimal volume)
        {
            _trackSendFxMixer.SetVolume(volume);
        }

        /// <summary>
        /// Returns a list of all available (unloaded) VST plugins
        /// </summary>
        /// <returns>A list of all available (unloaded) VST plugins</returns>
        public List<VstPlugin> FindVstPlugins()
        {
            var plugins = new List<VstPlugin>();
            if (Directory.Exists(this.VstPluginsFolder))
            {
                foreach (var pluginLocation in Directory.GetFiles(this.VstPluginsFolder, "*.dll", SearchOption.AllDirectories))
                {
                    var plugin = new VstPlugin();
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
        public void ClearMainVstPlugin()
        {
            this._speakerOutput.ClearVstPlugin1();
        }

        /// <summary>
        /// Clears the main VST plugin.
        /// </summary>
        public void ClearMainVstPlugin2()
        {
            this._speakerOutput.ClearVstPlugin2();
        }

        /// <summary>
        /// Clears the trackFX VST plugin.
        /// </summary>
        public void ClearSamplerVstPlugin()
        {
            this._samplerMixer.ClearVstPlugin1();
        }

        /// <summary>
        /// Clears the sampler VST plugin 2.
        /// </summary>
        public void ClearSamplerVstPlugin2()
        {
            this._samplerMixer.ClearVstPlugin2();
        }

        /// <summary>
        /// Clears the tracks VST plugin.
        /// </summary>
        public void ClearTracksVstPlugin()
        {
            this._trackMixer.ClearVstPlugin1();
        }

        /// <summary>
        /// Clears the tracks VST plugin.
        /// </summary>
        public void ClearTrackSendFxvstPlugin()
        {
            this._trackSendFxMixer.ClearVstPlugin1();
        }

        /// <summary>
        /// Clears the trackFX VST plugin 2.
        /// </summary>
        public void ClearTrackSendFxvstPlugin2()
        {
            this._trackSendFxMixer.ClearVstPlugin2();
        }

        /// <summary>
        /// Unloads all loaded VST plugins.
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
        /// Unloads all loaded WinAmp plugins.
        /// </summary>
        public void UnloadAllWaPlugins()
        {
            this._speakerOutput.ClearWaPlugin();
        }

        /// <summary>
        /// Shows the WinAmp DSP plugin config screen
        /// </summary>
        /// <param name="plugin">The WinAmp DSP plugin.</param>
        public void ShowWaPluginConfig(WaPlugin plugin)
        {
            if (plugin == null) return;

            BassWaDsp.BASS_WADSP_Config(plugin.Id);
        }

        /// <summary>
        /// Shows the VST plugin config screen.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
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
        /// Returns a list of all available (unloaded) WinAmp DSP plugins
        /// </summary>
        /// <returns>A list of all available (unloaded) WinAmp DSP plugins</returns>
        public List<WaPlugin> FindWaPlugins()
        {
            var plugins = new List<WaPlugin>();
            if (this.WaPluginsFolder == "") return plugins;

            foreach (var pluginLocation in Directory.GetFiles(this.WaPluginsFolder, "dsp_*.dll", SearchOption.AllDirectories))
            {
                var plugin = new WaPlugin();
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
        public string GetVstPluginParameters(VstPlugin plugin)
        {
            var values = new List<string>();
            var parameterCount = BassVst.BASS_VST_GetParamCount(plugin.Id);
            for (var i = 0; i < parameterCount; i++)
            {
                var value = BassVst.BASS_VST_GetParam(plugin.Id, i);
                values.Add(value.ToString());
            }
            return string.Join(",", values.ToArray());
        }

        /// <summary>
        /// Sets the VST plugin settings.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="settings">The settings as a key=value comma delimeted list.</param>
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
                catch { }
            }

            SetDelayByBpm();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the delay by BPM.
        /// </summary>
        private void SetDelayByBpm()
        {
            this._trackSendFxMixer.SetPluginBpm();
            this._samplerMixer.SetPluginBpm();
        }

        public decimal GetCurrentBpm()
        {
            var bpm = 100M;
            if (this.CurrentTrack != null)
            {
                bpm = this.CurrentTrack.Bpm;

                //if (!this.IsLocked())
                {
                    var trackPosition = GetPositionNoLock();

                    var position = trackPosition.Positition;
                    if (position < 0) position = 0;

                    if (position < this.CurrentTrack.FullStartLoopLength)
                    {
                        bpm = this.CurrentTrack.StartBpm;
                    }
                    else
                    {
                        var range = this.CurrentTrack.EndBpm - this.CurrentTrack.StartBpm;
                        var percentComplete = (decimal)((double)position / (double)trackPosition.Length);
                        bpm = this.CurrentTrack.StartBpm + (range * percentComplete);
                    }
                }
            }

            bpm = BassHelper.NormaliseBpm(bpm);
            return bpm;
        }

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

        public void SetVstPluginPreset(VstPlugin plugin, int presetIndex)
        {
            BassVst.BASS_VST_SetProgram(plugin.Id, presetIndex);
            SetDelayByBpm();
        }

        #endregion
    }
}