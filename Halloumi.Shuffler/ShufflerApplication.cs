﻿using System;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Midi;
using Halloumi.Shuffler.AudioEngine.Plugins;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Helpers;
using Halloumi.Shuffler.Forms;

namespace Halloumi.Shuffler
{
    public class ShufflerApplication
    {
        private frmPluginSettings _pluginSettingsForm;

        private bool _useConservativeFadeOut;

        public ShufflerApplication()
        {
            BassPlayer = new BassPlayer();
            Library = new Library(BassPlayer);

            CollectionHelper.Library = Library;

            LoadSettings();

            MixLibrary = new MixLibrary(Library.ShufflerFolder);
            SampleLibrary = new SampleLibrary(BassPlayer, Library);

            MidiManager = new MidiManager();
            MidiMapper = new BassPlayerMidiMapper(BassPlayer, MidiManager);

            LoadFromDatabase();


            BassPlayer.OnTrackChange += BassPlayer_OnTrackChange;
            BassPlayer.OnTrackQueued += BassPlayer_OnTrackQueued;
        }

        public BaseMinimizeToTrayForm BaseForm { get; set; }

        public bool UseConservativeFadeOut
        {
            get => _useConservativeFadeOut;
            set
            {
                _useConservativeFadeOut = value;
                SetConservativeFadeOutSettings();
            }
        }

        public BassPlayerMidiMapper MidiMapper { get; }

        public MixLibrary MixLibrary { get; }

        public Library Library { get; }

        public SampleLibrary SampleLibrary { get; }

        public BassPlayer BassPlayer { get; }

        public MidiManager MidiManager { get; }

        private void LoadFromDatabase()
        {
            Library.LoadFromDatabase();
            SampleLibrary.LoadFromCache();

            MixLibrary.AvailableTracks = Library.GetTracks();
            MixLibrary.LoadFromDatabase();

            ExtenedAttributesHelper.ShufflerFolder = Library.ShufflerFolder;
            ExtenedAttributesHelper.LoadFromDatabase();
            Library.LoadAllExtendedAttributes();

            AutomationAttributesHelper.ShufflerFolder = Library.ShufflerFolder;
            AutomationAttributesHelper.LoadFromDatabase();

            CollectionHelper.LoadFromDatabase();
        }

        public void Unload()
        {
            SaveSettings();
            Library.SaveToDatabase();
            BassPlayer.Dispose();
            MidiManager.Dispose();
        }

        /// <summary>
        ///     Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            var settings = Settings.Default;
            Library.LibraryFolder = settings.LibraryFolder;
            ExtenedAttributesHelper.ShufflerFolder = settings.ShufflerFolder;
            PluginHelper.WaPluginsFolder = settings.WaPluginsFolder;
            PluginHelper.VstPluginsFolder = settings.VstPluginsFolder;
            BassPlayer.TrackFxAutomationEnabled = settings.EnableTrackFxAutomation;
            BassPlayer.SampleAutomationEnabled = settings.EnableSampleAutomation;
            KeyHelper.SetApplicationFolder(settings.KeyFinderFolder);


            if (settings.WaPlugin != "")
                try
                {
                    BassPlayer.LoadWaPlugin(settings.WaPlugin);
                }
                catch
                {
                    // ignored
                }

            if (settings.MainMixerVstPlugin != "")
                try
                {
                    BassPlayer.LoadMainVstPlugin(settings.MainMixerVstPlugin, 0);
                }
                catch
                {
                    // ignored
                }

            if (settings.MainMixerVstPlugin2 != "")
                try
                {
                    BassPlayer.LoadMainVstPlugin(settings.MainMixerVstPlugin2, 1);
                }
                catch
                {
                    // ignored
                }

            if (settings.MainMixerVstPluginParameters != "" && BassPlayer.MainVstPlugin != null)
                try
                {
                    PluginHelper.SetVstPluginParameters(BassPlayer.MainVstPlugin,
                        settings.MainMixerVstPluginParameters);
                }
                catch
                {
                    // ignored
                }

            if (settings.MainMixerVstPlugin2Parameters != "" && BassPlayer.MainVstPlugin2 != null)
                try
                {
                    PluginHelper.SetVstPluginParameters(BassPlayer.MainVstPlugin2,
                        settings.MainMixerVstPlugin2Parameters);
                }
                catch
                {
                    // ignored
                }

            if (settings.SamplerVstPlugin != "")
                try
                {
                    BassPlayer.LoadSamplerVstPlugin(settings.SamplerVstPlugin, 0);
                }
                catch
                {
                    // ignored
                }

            if (settings.SamplerVstPluginParameters != "" && BassPlayer.SamplerVstPlugin != null)
                try
                {
                    PluginHelper.SetVstPluginParameters(BassPlayer.SamplerVstPlugin,
                        settings.SamplerVstPluginParameters);
                }
                catch
                {
                    // ignored
                }

            if (settings.SamplerVstPlugin2 != "")
                try
                {
                    BassPlayer.LoadSamplerVstPlugin(settings.SamplerVstPlugin2, 1);
                }
                catch
                {
                    // ignored
                }

            if (settings.SamplerVstPlugin2Parameters != "" && BassPlayer.SamplerVstPlugin2 != null)
                try
                {
                    PluginHelper.SetVstPluginParameters(BassPlayer.SamplerVstPlugin2,
                        settings.SamplerVstPlugin2Parameters);
                }
                catch
                {
                    // ignored
                }

            if (settings.TrackVstPlugin != "")
                try
                {
                    BassPlayer.LoadTracksVstPlugin(settings.TrackVstPlugin, 0);
                }
                catch
                {
                    // ignored
                }

            if (settings.TrackVstPluginParameters != "" && BassPlayer.TrackVstPlugin != null)
                try
                {
                    PluginHelper.SetVstPluginParameters(BassPlayer.TrackVstPlugin, settings.TrackVstPluginParameters);
                }
                catch
                {
                    // ignored
                }

            if (settings.TrackFxvstPlugin != "")
                try
                {
                    BassPlayer.LoadTrackSendFxvstPlugin(settings.TrackFxvstPlugin, 0);
                }
                catch
                {
                    // ignored
                }

            if (settings.TrackFxvstPluginParameters != "" && BassPlayer.TrackSendFxVstPlugin != null)
                try
                {
                    PluginHelper.SetVstPluginParameters(BassPlayer.TrackSendFxVstPlugin,
                        settings.TrackFxvstPluginParameters);
                }
                catch
                {
                    // ignored
                }

            if (settings.TrackFxvstPlugin2 != "")
                try
                {
                    BassPlayer.LoadTrackSendFxvstPlugin(settings.TrackFxvstPlugin2, 1);
                }
                catch
                {
                    // ignored
                }

            if (settings.TrackFxvstPlugin2Parameters != "" && BassPlayer.TrackSendFxVstPlugin2 != null)
                try
                {
                    PluginHelper.SetVstPluginParameters(BassPlayer.TrackSendFxVstPlugin2,
                        settings.TrackFxvstPlugin2Parameters);
                }
                catch
                {
                    // ignored
                }

            BassPlayer.LimitSongLength = settings.LimitSongLength;
            BassPlayer.SetMonitorVolume(settings.MonitorVolume);

            UseConservativeFadeOut = settings.LimitSongLength;
        }

        public void SaveSettings()
        {
            var settings = Settings.Default;

            var winampPlugin = "";
            if (BassPlayer.WaPlugin != null) winampPlugin = BassPlayer.WaPlugin.Location;
            settings.WaPlugin = winampPlugin;

            var mainVstPlugin = "";
            if (BassPlayer.MainVstPlugin != null) mainVstPlugin = BassPlayer.MainVstPlugin.Location;
            settings.MainMixerVstPlugin = mainVstPlugin;

            var mainVstPlugin2 = "";
            if (BassPlayer.MainVstPlugin2 != null) mainVstPlugin2 = BassPlayer.MainVstPlugin2.Location;
            settings.MainMixerVstPlugin2 = mainVstPlugin2;

            var mainVstPluginParameters = "";
            if (BassPlayer.MainVstPlugin != null)
                mainVstPluginParameters = PluginHelper.GetVstPluginParameters(BassPlayer.MainVstPlugin);
            settings.MainMixerVstPluginParameters = mainVstPluginParameters;

            var mainVstPluginParameters2 = "";
            if (BassPlayer.MainVstPlugin2 != null)
                mainVstPluginParameters2 = PluginHelper.GetVstPluginParameters(BassPlayer.MainVstPlugin2);
            settings.MainMixerVstPlugin2Parameters = mainVstPluginParameters2;

            var samplerVstPlugin = "";
            if (BassPlayer.SamplerVstPlugin != null) samplerVstPlugin = BassPlayer.SamplerVstPlugin.Location;
            settings.SamplerVstPlugin = samplerVstPlugin;

            var samplerVstPluginParameters = "";
            if (BassPlayer.SamplerVstPlugin != null)
                samplerVstPluginParameters = PluginHelper.GetVstPluginParameters(BassPlayer.SamplerVstPlugin);
            settings.SamplerVstPluginParameters = samplerVstPluginParameters;

            var samplerVstPlugin2 = "";
            if (BassPlayer.SamplerVstPlugin2 != null) samplerVstPlugin2 = BassPlayer.SamplerVstPlugin2.Location;
            settings.SamplerVstPlugin2 = samplerVstPlugin2;

            var samplerVstPluginParameters2 = "";
            if (BassPlayer.SamplerVstPlugin2 != null)
                samplerVstPluginParameters2 = PluginHelper.GetVstPluginParameters(BassPlayer.SamplerVstPlugin2);
            settings.SamplerVstPlugin2Parameters = samplerVstPluginParameters2;

            var trackVstPlugin = "";
            if (BassPlayer.TrackVstPlugin != null) trackVstPlugin = BassPlayer.TrackVstPlugin.Location;
            settings.TrackVstPlugin = trackVstPlugin;

            var trackVstPluginParameters = "";
            if (BassPlayer.TrackVstPlugin != null)
                trackVstPluginParameters = PluginHelper.GetVstPluginParameters(BassPlayer.TrackVstPlugin);
            settings.TrackVstPluginParameters = trackVstPluginParameters;

            var trackFxVstPlugin = "";
            if (BassPlayer.TrackSendFxVstPlugin != null) trackFxVstPlugin = BassPlayer.TrackSendFxVstPlugin.Location;
            settings.TrackFxvstPlugin = trackFxVstPlugin;

            var trackFxVstPluginParameters = "";
            if (BassPlayer.TrackSendFxVstPlugin != null)
                trackFxVstPluginParameters = PluginHelper.GetVstPluginParameters(BassPlayer.TrackSendFxVstPlugin);
            settings.TrackFxvstPluginParameters = trackFxVstPluginParameters;

            var trackFxVstPlugin2 = "";
            if (BassPlayer.TrackSendFxVstPlugin2 != null) trackFxVstPlugin2 = BassPlayer.TrackSendFxVstPlugin2.Location;
            settings.TrackFxvstPlugin2 = trackFxVstPlugin2;

            var trackFxVstPluginParameters2 = "";
            if (BassPlayer.TrackSendFxVstPlugin2 != null)
                trackFxVstPluginParameters2 = PluginHelper.GetVstPluginParameters(BassPlayer.TrackSendFxVstPlugin2);
            settings.TrackFxvstPlugin2Parameters = trackFxVstPluginParameters2;

            settings.LimitSongLength = UseConservativeFadeOut;
            settings.Volume = BassPlayer.GetMixerVolume();

            settings.SamplerDelayNotes = BassPlayer.SamplerDelayNotes;
            settings.SamplerVolume = Convert.ToInt32(BassPlayer.GetSamplerMixerVolume());

            settings.TrackFxDelayNotes = BassPlayer.TrackSendFxDelayNotes;
            settings.TrackFxVolume = Convert.ToInt32(BassPlayer.GetTrackSendFxVolume());

            settings.SamplerOutput = BassPlayer.SamplerOutput;
            settings.TrackOutput = BassPlayer.TrackOutput;
            settings.MonitorVolume = Convert.ToInt32(BassPlayer.GetMonitorVolume());
            settings.RawLoopOutput = BassPlayer.RawLoopOutput;

            settings.EnableTrackFxAutomation = BassPlayer.TrackFxAutomationEnabled;
            settings.EnableSampleAutomation = BassPlayer.SampleAutomationEnabled;

            settings.Save();
        }

        public void ShowPlugin(VstPlugin plugin)
        {
            if (plugin == null)
                ShowPluginsForm();
            else
                PluginHelper.ShowVstPluginConfig(plugin);
        }

        public void ShowPlugin(WaPlugin plugin)
        {
            if (plugin == null)
                ShowPluginsForm();
            else
                PluginHelper.ShowWaPluginConfig(plugin);
        }

        /// <summary>
        ///     Shows the plug-in form.
        /// </summary>
        public void ShowPluginsForm()
        {
            if (_pluginSettingsForm == null || _pluginSettingsForm.IsDisposed)
                _pluginSettingsForm = new frmPluginSettings(BassPlayer);
            if (!_pluginSettingsForm.Visible)
                WindowHelper.ShowDialog(BaseForm, _pluginSettingsForm);
        }

        private void SetConservativeFadeOutSettings()
        {
            if (!_useConservativeFadeOut) return;
            if (BassPlayer.CurrentTrack == null || BassPlayer.NextTrack == null) return;

            var track1 = Library.GetTrackByFilename(BassPlayer.CurrentTrack.Filename);
            var track2 = Library.GetTrackByFilename(BassPlayer.NextTrack.Filename);
            var mixRank = MixLibrary.GetMixLevel(track1, track2);
            var hasExtendedMix = MixLibrary.HasExtendedMix(track1, track2);

            if (mixRank <= 2 && !hasExtendedMix)
                BassPlayer.SetConservativeFadeOutSettings();
        }

        /// <summary>
        ///     Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange(object sender, EventArgs e)
        {
            SetIconText();
        }

        /// <summary>
        ///     Handles the OnTrackQueued event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackQueued(object sender, EventArgs e)
        {
            SetConservativeFadeOutSettings();
        }


        /// <summary>
        ///     Sets the icon text.
        /// </summary>
        private void SetIconText()
        {
            if (BaseForm == null) return;

            if (BassPlayer.CurrentTrack != null)
            {
                var text = BassPlayer.CurrentTrack.Description.Replace("&", "&&");
                if (text.Length > 63) text = text.Substring(0, 63);
                BaseForm.NotifyIcon.Text = text;
            }
            else
            {
                BaseForm.NotifyIcon.Text = "";
            }
        }
    }
}