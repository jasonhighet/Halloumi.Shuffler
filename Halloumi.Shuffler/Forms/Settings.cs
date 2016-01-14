﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.Engine;

namespace Halloumi.Shuffler.Forms
{
    public class Settings
    {
        private static Settings _default = null;

        public static Settings Default
        {
            get
            {
                if (_default == null) _default = LoadSetttings();
                return _default;
            }
        }

        private static Settings LoadSetttings()
        {
            var settings = new Settings();
            try
            {
                var filename = Path.Combine(ApplicationHelper.GetUserDataPath(), "Halloumi.Shuffler.Settings.xml");
                if (File.Exists(filename))
                {
                    settings = SerializationHelper<Settings>.FromXmlFile(filename);
                }
            }
            catch
            { }

            return settings;
        }

        public void Save()
        {
            var filename = Path.Combine(ApplicationHelper.GetUserDataPath(), "Halloumi.Shuffler.Settings.xml");
            SerializationHelper<Settings>.ToXmlFile(this, filename);
        }

        public Settings()
        {
            LibraryFolder = "";
            PlaylistFolder = "";
            ShufflerFolder = "";
            WaPluginsFolder = "";
            VstPluginsFolder = "";
            WaPlugin = "";
            LeftRightSplit = 0;
            FormStateSettings = "";
            TrackSplit = 0;
            MinimizeToTray = true;
            LimitSongLength = true;
            ExportPlaylistFolder = "";
            Volume = 100;
            ShowMixableTracks = false;
            AnalogXScratchFolder = "";
            KeyFinderFolder = "";
            MainMixerVstPlugin = "";
            MainMixerVstPlugin2 = "";
            SamplerVstPlugin = "";
            TrackVstPlugin = "";
            TrackFxvstPlugin = "";
            SamplerVstPlugin2 = "";
            TrackFxvstPlugin2 = "";
            MainMixerVstPluginParameters = "";
            MainMixerVstPlugin2Parameters = "";
            SamplerVstPluginParameters = "";
            TrackVstPluginParameters = "";
            TrackFxvstPluginParameters = "";
            SamplerVstPlugin2Parameters = "";
            TrackFxvstPlugin2Parameters = "";
            CutSamplerBass = true;
            BypassSamplerEffect1 = false;
            BypassSamplerEffect2 = false;
            SamplerVolume = 50;
            SamplerDelayNotes = 0.25M;
            BypassTrackFxEffect1 = false;
            BypassTrackFxEffect2 = false;
            TrackFxVolume = 50;
            TrackFxDelayNotes = 0.25M;
            SamplerOutput = BassEngine.Channels.SoundOutput.Speakers;
            TrackOutput = BassEngine.Channels.SoundOutput.Speakers;
            MonitorVolume = 5;
            MixableRankFilterIndex = 0;
            MixableKeyRankFilterIndex = 0;
            MixableViewIndex = 0;
            RawLoopOutput = BassEngine.Channels.SoundOutput.Speakers;
            EnableTrackFxAutomation = true;
            EnableSampleAutomation = true;
            ShufflerMode = TrackSelector.MixStrategy.None;
            UpdateLibraryOnStartup = true;
            AlbumArtShown = true;
            ImportShufflerFilesFolder = "";
        }

        public string LibraryFolder { get; set; }

        public string PlaylistFolder { get; set; }

        public string ShufflerFolder { get; set; }

        public string WaPluginsFolder { get; set; }

        public string VstPluginsFolder { get; set; }

        public string MainMixerVstPlugin { get; set; }

        public string MainMixerVstPlugin2 { get; set; }

        public string WaPlugin { get; set; }

        public int LeftRightSplit { get; set; }

        public string FormStateSettings { get; set; }

        public int TrackSplit { get; set; }

        public bool MinimizeToTray { get; set; }

        public bool LimitSongLength { get; set; }

        public string ExportPlaylistFolder { get; set; }

        public decimal Volume { get; set; }

        public bool ShowMixableTracks { get; set; }

        public string AnalogXScratchFolder { get; set; }

        public string KeyFinderFolder { get; set; }

        public string SamplerVstPlugin { get; set; }

        public string TrackVstPlugin { get; set; }

        public string TrackFxvstPlugin { get; set; }

        public string SamplerVstPlugin2 { get; set; }

        public string TrackFxvstPlugin2 { get; set; }

        public string MainMixerVstPluginParameters { get; set; }

        public string MainMixerVstPlugin2Parameters { get; set; }

        public string SamplerVstPluginParameters { get; set; }

        public string TrackVstPluginParameters { get; set; }

        public string TrackFxvstPluginParameters { get; set; }

        public string SamplerVstPlugin2Parameters { get; set; }

        public string TrackFxvstPlugin2Parameters { get; set; }

        public bool CutSamplerBass { get; set; }

        public bool BypassSamplerEffect1 { get; set; }

        public bool BypassSamplerEffect2 { get; set; }

        public int SamplerVolume { get; set; }

        public decimal SamplerDelayNotes { get; set; }

        public bool BypassTrackFxEffect1 { get; set; }

        public bool BypassTrackFxEffect2 { get; set; }

        public int TrackFxVolume { get; set; }

        public decimal TrackFxDelayNotes { get; set; }

        public BassEngine.Channels.SoundOutput SamplerOutput { get; set; }

        public BassEngine.Channels.SoundOutput TrackOutput { get; set; }

        public int MonitorVolume { get; set; }

        public int MixableRankFilterIndex { get; set; }

        public int MixableViewIndex { get; set; }

        public int MixableKeyRankFilterIndex { get; set; }

        public bool MixableTracksExcludeQueued { get; set; }

        public BassEngine.Channels.SoundOutput RawLoopOutput { get; set; }

        public bool EnableTrackFxAutomation { get; set; }

        public bool EnableSampleAutomation { get; set; }

        public bool VisualsShown { get; set; }

        public bool AlbumArtShown { get; set; }

        public bool SkipAfterMix { get; set; }

        public TrackSelector.MixStrategy ShufflerMode { get; set; }

        public bool UpdateLibraryOnStartup { get; set; }

        public string ImportShufflerFilesFolder { get; set; }
    }
}