using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Plugins;
using Halloumi.Common.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.AddOn.Vst;
using Un4seen.Bass.AddOn.WaDsp;

namespace Halloumi.Shuffler.AudioEngine.Channels
{
    public abstract class Channel
    {
        /// <summary>
        ///     Set to true once the WinAmp DSP engine has been initialized
        /// </summary>
        private static bool _waDspLoaded;

        private decimal _syncNotes = 0.25M;

        protected Channel(IBmpProvider bpmProvider = null)
        {
            BpmProvider = bpmProvider;
            VstPlugins = new List<VstPlugin>();
        }

        public IBmpProvider BpmProvider { get; }

        public int ChannelId { get; internal set; }

        public List<VstPlugin> VstPlugins { get; internal set; }


        public VstPlugin GetVstPlugin(int index)
        {
            return index >= VstPlugins.Count() ? null : VstPlugins[index];
        }

        public decimal SyncNotes
        {
            get { return _syncNotes; }
            set
            {
                if (value < 0 || value > 1) return;

                _syncNotes = value;

                foreach (var plugin in VstPlugins)
                {
                    SetPluginSyncNotes(plugin, _syncNotes);
                }

                SetPluginBpm();
            }
        }

        public WaPlugin WaPlugin { get; private set; }

        private static void SetPluginSyncNotes(VstPlugin plugin, decimal syncNotes)
        {
            if (plugin == null)
                return;

            foreach (var parameter in plugin.Parameters.Where(x => x.SyncToBpm && x.VariableSyncNotes))
            {
                parameter.SyncNotes = syncNotes;
            }
        }

        public void AddInputChannel(MixerChannel inputChannel)
        {
            switch (inputChannel.OutputType)
            {
                case MixerChannelOutputType.SingleOutput:
                    ChannelHelper.AddChannelToDecoderMixer(ChannelId, inputChannel.ChannelId);
                    break;
                case MixerChannelOutputType.MultipleOutputs:
                    var splitOutputChannel = ChannelHelper.SplitDecoderMixer(inputChannel.ChannelId);
                    ChannelHelper.AddChannelToDecoderMixer(ChannelId, splitOutputChannel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RemoveInputChannel(MixerChannel inputChannel)
        {
            switch (inputChannel.OutputType)
            {
                case MixerChannelOutputType.SingleOutput:
                    ChannelHelper.AddChannelToDecoderMixer(ChannelId, inputChannel.ChannelId);
                    break;
                case MixerChannelOutputType.MultipleOutputs:
                    var splitOutputChannel = ChannelHelper.SplitDecoderMixer(inputChannel.ChannelId);
                    ChannelHelper.AddChannelToDecoderMixer(ChannelId, splitOutputChannel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        /// <summary>
        ///     Sets the volume.
        /// </summary>
        /// <param name="volume">A value between 0 and 100</param>
        public void SetVolume(decimal volume)
        {
            AudioStreamHelper.SetVolume(ChannelId, volume);
        }

        /// <summary>
        ///     Gets the volume.
        /// </summary>
        /// <returns>A value between 0 and 100</returns>
        public decimal GetVolume()
        {
            return AudioStreamHelper.GetVolume(ChannelId);
        }

        /// <summary>
        ///     Gets the current volume levels.
        /// </summary>
        /// <returns>A VolumeLevels object containing the left and right volume levels (0 - 32768)</returns>
        public VolumeLevels GetVolumeLevels()
        {
            var levels = new VolumeLevels();
            var level = Bass.BASS_ChannelGetLevel(ChannelId);
            levels.Left = Utils.LowWord32(level);
            levels.Right = Utils.HighWord32(level);
            return levels;
        }

        public void LoadVstSettings(string name)
        {
            var filename = GetChannelVstSettingsFilename(name);
            if(!File.Exists(filename)) return;

            var vstSettings = SerializationHelper<VstSettings>.FromXmlFile(filename);

            ApplyVstSettings(vstSettings);
        }

        public void ApplyVstSettings(VstSettings vstSettings)
        {
            foreach (var plugin in VstPlugins)
            {
                ClearVstPlugin(VstPlugins.IndexOf(plugin));
            }

            foreach (var settings in vstSettings.Plugins)
            {
                var index = vstSettings.Plugins.IndexOf(settings);
                var plugin = LoadVstPlugin(settings.Key, index);
                PluginHelper.SetVstPluginParameters(plugin, settings.Value);
            }
        }

        private static string GetChannelVstSettingsFilename(string name)
        {
            return Path.Combine(ApplicationHelper.GetUserDataPath(), FileSystemHelper.StripInvalidFileNameChars(name) + ".VstSettings.xml");
        }

        public void SaveVstSettings(string name)
        {
            var vstSettings = GetVstSettings();

            var filename = GetChannelVstSettingsFilename(name);
            SerializationHelper<VstSettings>.ToXmlFile(vstSettings, filename);
        }

        public VstSettings GetVstSettings()
        {
            var vstSettings = new VstSettings();
            foreach (var plugin in VstPlugins)
            {
                var settings = new KeyValuePair<string, string>(plugin.Location,
                    PluginHelper.GetVstPluginParameters(plugin));
                vstSettings.Plugins.Add(settings);
            }
            return vstSettings;
        }

        public class VstSettings
        {
            public VstSettings()
            {
                Plugins = new List<KeyValuePair<string, string>>();
            }
            public List<KeyValuePair<string, string>> Plugins { get; set; }
        }

        public VstPlugin LoadVstPlugin(string location, int index)
        {
            if (VstPlugins.Count() > index
                && VstPlugins[index] != null
                && VstPlugins[index].Location == location)
                return VstPlugins[index];

            ClearVstPlugin(index);

            for (var i = VstPlugins.Count(); i < index + 1; i++)
            {
                VstPlugins.Add(null);
            }

            VstPlugins[index] = LoadAndApplyVstPlugin(location, index + 1);

            SetPluginBpm(VstPlugins[index]);
            return VstPlugins[index];
        }

        public void ClearVstPlugins()
        {
            for (var i = 0; i < VstPlugins.Count; i++)
            {
                ClearVstPlugin(i);
            }
        }


        public void ClearVstPlugin(int index)
        {
            if (index >= VstPlugins.Count() || VstPlugins[index] == null) return;

            RemoveVstPlugin(VstPlugins[index]);
            VstPlugins[index] = null;
        }


        private void RemoveVstPlugin(VstPlugin plugin)
        {
            if (plugin == null) return;
            // DebugHelper.WriteLine("Unload plug-in" + plugin.Name);
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

            BassVst.BASS_VST_ChannelRemoveDSP(ChannelId, plugin.Id);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        /// <param name="priority">The priority.</param>
        /// <returns>The VST plug-in</returns>
        private VstPlugin LoadAndApplyVstPlugin(string location, int priority)
        {
            if (location == "") return null;

            if (!File.Exists(location)) return null;

            var plugin = new VstPlugin
            {
                Id = BassVst.BASS_VST_ChannelSetDSP(ChannelId, location, BASSVSTDsp.BASS_VST_DEFAULT, priority)
            };

            if (plugin.Id == 0)
                throw new Exception("Cannot load plug-in " + Path.GetFileNameWithoutExtension(location));

            var info = BassVst.BASS_VST_GetInfo(plugin.Id);
            if (info != null)
            {
                plugin.Name = info.effectName;
                plugin.EditorWidth = info.editorWidth;
                plugin.EditorHeight = info.editorHeight;
            }
            else
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(location);
                if (fileNameWithoutExtension != null)
                    plugin.Name = StringHelper.TitleCase(fileNameWithoutExtension.Replace("_", " "));
            }

            if (plugin.EditorWidth == 0) plugin.EditorWidth = 400;
            if (plugin.EditorHeight == 0) plugin.EditorHeight = 600;

            plugin.Location = location;
            LoadPluginParameters(plugin);

            SetPluginBpm(plugin);

            return plugin;
        }

        private static void LoadPluginParameters(VstPlugin plugin)
        {
            plugin.Parameters = new List<VstPlugin.VstPluginParameter>();

            var parameterCount = BassVst.BASS_VST_GetParamCount(plugin.Id);
            for (var i = 0; i < parameterCount; i++)
            {
                var parameterInfo = BassVst.BASS_VST_GetParamInfo(plugin.Id, i);

                var name = parameterInfo.name.Trim();
                if (string.IsNullOrWhiteSpace(name) || name.ToLower().StartsWith("unused"))
                    continue;

                var parameter = new VstPlugin.VstPluginParameter
                {
                    Id = i,
                    Name = parameterInfo.name
                };

                LoadPresetParameterValues(plugin, parameter);

                plugin.Parameters.Add(parameter);
            }
        }

        private static void LoadPresetParameterValues(VstPlugin plugin, VstPlugin.VstPluginParameter parameter)
        {
            var presetParameters = new[]
            {
                new
                {
                    PluginName = "Tape Delay",
                    ParameterName = "Time",
                    SyncToBpm = true,
                    MinSyncMilliSeconds = 60M,
                    MaxSyncMilliSeconds = 1500M,
                    SyncUsingLogScale = false,
                    VariableSyncNotes = true,
                    DefaultSyncNotes = (1M/4M)
                },
                new
                {
                    PluginName = "Classic Delay",
                    ParameterName = "Time",
                    SyncToBpm = true,
                    MinSyncMilliSeconds = 50M,
                    MaxSyncMilliSeconds = 5000M,
                    SyncUsingLogScale = true,
                    VariableSyncNotes = true,
                    DefaultSyncNotes = (1M/4M)
                }
            }.ToList();


            var presetParameter = presetParameters
                .FirstOrDefault(p => string.Equals(plugin.Name, p.PluginName, StringComparison.CurrentCultureIgnoreCase)
                                     &&
                                     string.Equals(parameter.Name, p.ParameterName,
                                         StringComparison.CurrentCultureIgnoreCase));

            if (presetParameter == null) return;

            parameter.SyncToBpm = presetParameter.SyncToBpm;
            parameter.MinSyncMilliSeconds = presetParameter.MinSyncMilliSeconds;
            parameter.MaxSyncMilliSeconds = presetParameter.MaxSyncMilliSeconds;
            parameter.SyncUsingLogScale = presetParameter.SyncUsingLogScale;
            parameter.VariableSyncNotes = presetParameter.VariableSyncNotes;
            parameter.SyncNotes = presetParameter.DefaultSyncNotes;
        }

        /// <summary>
        ///     Sets the delay by BPM.
        /// </summary>
        public void SetPluginBpm()
        {
            lock (this)
            {
                foreach (var plugin in VstPlugins)
                {
                    SetPluginBpm(plugin);
                }
            }
        }

        private void SetPluginBpm(VstPlugin plugin)
        {
            if (plugin == null)
                return;
            if (BpmProvider == null)
                return;

            if (!plugin.Parameters.Any(x => x.SyncToBpm))
                return;

            var bpm = BpmProvider.GetCurrentBpm();
            var quarterNoteLength = BpmHelper.GetDefaultDelayLength(bpm);
            var fullNoteLength = quarterNoteLength*4;

            var syncParameters = plugin.Parameters.Where(p => p.SyncToBpm).ToList();

            var mutePlugin = syncParameters.Any(p => p.SyncNotes == 0);
            BassVst.BASS_VST_SetBypass(plugin.Id, mutePlugin);

            if (mutePlugin)
                return;

            foreach (var parameter in syncParameters)
            {
                var syncLength = fullNoteLength*(double) parameter.SyncNotes;
                var vstDelayValue = GetVstSyncValue(syncLength, parameter);
                BassVst.BASS_VST_SetParam(plugin.Id, parameter.Id, vstDelayValue);
            }
        }

        /// <summary>
        ///     Converts a sync length value to a VST specific percentage (based on the parameter max/min milliseconds)
        /// </summary>
        /// <param name="syncLength">Length of the delay in milliseconds.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        ///     The VST specific percentage
        /// </returns>
        private static float GetVstSyncValue(double syncLength, VstPlugin.VstPluginParameter parameter)
        {
            var minMs = (double) parameter.MinSyncMilliSeconds;
            var maxMs = (double) parameter.MaxSyncMilliSeconds;

            if (syncLength < minMs) syncLength = minMs;
            if (syncLength > maxMs) syncLength = maxMs;

            return parameter.SyncUsingLogScale
                ? (float) (Math.Log10(syncLength/minMs)/Math.Log10(maxMs/minMs))
                : (float) ((syncLength - minMs)/(maxMs - minMs));
        }


        /// <summary>
        ///     Loads a WinAmp DSP plug-in and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the WinAmp DSP DLL</param>
        public WaPlugin LoadWaPlugin(string location)
        {
            if (location == "") return null;

            if (WaPlugin != null && WaPlugin.Location == location) return WaPlugin;

            if (!File.Exists(location)) return null;

            BassMix.BASS_Mixer_ChannelPause(ChannelId);

            if (!_waDspLoaded) StartWaDspEngine();

            // DebugHelper.WriteLine("Load WAPlugin " + location);

            var plugin = new WaPlugin
            {
                Id = BassWaDsp.BASS_WADSP_Load(location, 10, 10, 300, 300, null),
                Module = 0
            };

            plugin.Name = BassWaDsp.BASS_WADSP_GetName(plugin.Id);
            plugin.Location = location;
            BassWaDsp.BASS_WADSP_Start(plugin.Id, plugin.Module, ChannelId);
            BassWaDsp.BASS_WADSP_ChannelSetDSP(plugin.Id, ChannelId, 1);

            WaPlugin = plugin;

            BassMix.BASS_Mixer_ChannelPlay(ChannelId);

            return plugin;
        }

        public void ClearWaPlugin()
        {
            if (WaPlugin == null)
                return;

            // DebugHelper.WriteLine("Unload plug-in" + WaPlugin.Name);
            try
            {
                BassWaDsp.BASS_WADSP_Stop(WaPlugin.Id);
                BassWaDsp.BASS_WADSP_ChannelRemoveDSP(WaPlugin.Id);
                BassWaDsp.BASS_WADSP_FreeDSP(WaPlugin.Id);
            }
            catch
            {
                // ignored
            }

            WaPlugin = null;
        }

        /// <summary>
        ///     Initialises the WinAmp DSP engine
        /// </summary>
        private static void StartWaDspEngine()
        {
            // DebugHelper.WriteLine("StartWADSPEngine");
            BassWaDsp.BASS_WADSP_Init(IntPtr.Zero);
            _waDspLoaded = true;
        }
    }

    public interface IBmpProvider
    {
        decimal GetCurrentBpm();
    }
}