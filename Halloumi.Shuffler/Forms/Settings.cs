using System;
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
            this.LibraryFolder = "";
            this.PlaylistFolder = "";
            this.ShufflerFolder = "";
            this.WAPluginsFolder = "";
            this.VSTPluginsFolder = "";
            this.WAPlugin = "";
            this.LeftRightSplit = 0;
            this.FormStateSettings = "";
            this.TrackSplit = 0;
            this.MinimizeToTray = true;
            this.LimitSongLength = true;
            this.ExportPlaylistFolder = "";
            this.Volume = 100;
            this.ShowMixableTracks = false;
            this.AnalogXScratchFolder = "";
            this.KeyFinderFolder = "";
            this.MainMixerVSTPlugin = "";
            this.MainMixerVSTPlugin2 = "";
            this.SamplerVSTPlugin = "";
            this.TrackVSTPlugin = "";
            this.TrackFXVSTPlugin = "";
            this.SamplerVSTPlugin2 = "";
            this.TrackFXVSTPlugin2 = "";
            this.MainMixerVSTPluginParameters = "";
            this.MainMixerVSTPlugin2Parameters = "";
            this.SamplerVSTPluginParameters = "";
            this.TrackVSTPluginParameters = "";
            this.TrackFXVSTPluginParameters = "";
            this.SamplerVSTPlugin2Parameters = "";
            this.TrackFXVSTPlugin2Parameters = "";
            this.CutSamplerBass = true;
            this.BypassSamplerEffect1 = false;
            this.BypassSamplerEffect2 = false;
            this.SamplerVolume = 50;
            this.SamplerDelayNotes = 0.25M;
            this.BypassTrackFXEffect1 = false;
            this.BypassTrackFXEffect2 = false;
            this.TrackFXVolume = 50;
            this.TrackFXDelayNotes = 0.25M;
            this.SamplerOutput = Halloumi.BassEngine.Channels.SoundOutput.Speakers;
            this.TrackOutput = Halloumi.BassEngine.Channels.SoundOutput.Speakers;
            this.MonitorVolume = 5;
            this.MixableRankFilterIndex = 0;
            this.MixableKeyRankFilterIndex = 0;
            this.MixableViewIndex = 0;
            this.RawLoopOutput = Halloumi.BassEngine.Channels.SoundOutput.Speakers;
            this.EnableTrackFXAutomation = true;
            this.EnableSampleAutomation = true;
            this.ShufflerMode = TrackSelector.MixStrategy.None;
            this.UpdateLibraryOnStartup = true;
            this.AlbumArtShown = true;
            this.ImportShufflerFilesFolder = "";
        }

        public string LibraryFolder { get; set; }

        public string PlaylistFolder { get; set; }

        public string ShufflerFolder { get; set; }

        public string WAPluginsFolder { get; set; }

        public string VSTPluginsFolder { get; set; }

        public string MainMixerVSTPlugin { get; set; }

        public string MainMixerVSTPlugin2 { get; set; }

        public string WAPlugin { get; set; }

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

        public string SamplerVSTPlugin { get; set; }

        public string TrackVSTPlugin { get; set; }

        public string TrackFXVSTPlugin { get; set; }

        public string SamplerVSTPlugin2 { get; set; }

        public string TrackFXVSTPlugin2 { get; set; }

        public string MainMixerVSTPluginParameters { get; set; }

        public string MainMixerVSTPlugin2Parameters { get; set; }

        public string SamplerVSTPluginParameters { get; set; }

        public string TrackVSTPluginParameters { get; set; }

        public string TrackFXVSTPluginParameters { get; set; }

        public string SamplerVSTPlugin2Parameters { get; set; }

        public string TrackFXVSTPlugin2Parameters { get; set; }

        public bool CutSamplerBass { get; set; }

        public bool BypassSamplerEffect1 { get; set; }

        public bool BypassSamplerEffect2 { get; set; }

        public int SamplerVolume { get; set; }

        public decimal SamplerDelayNotes { get; set; }

        public bool BypassTrackFXEffect1 { get; set; }

        public bool BypassTrackFXEffect2 { get; set; }

        public int TrackFXVolume { get; set; }

        public decimal TrackFXDelayNotes { get; set; }

        public BassEngine.Channels.SoundOutput SamplerOutput { get; set; }

        public BassEngine.Channels.SoundOutput TrackOutput { get; set; }

        public int MonitorVolume { get; set; }

        public int MixableRankFilterIndex { get; set; }

        public int MixableViewIndex { get; set; }

        public int MixableKeyRankFilterIndex { get; set; }

        public bool MixableTracksExcludeQueued { get; set; }

        public BassEngine.Channels.SoundOutput RawLoopOutput { get; set; }

        public bool EnableTrackFXAutomation { get; set; }

        public bool EnableSampleAutomation { get; set; }

        public bool VisualsShown { get; set; }

        public bool AlbumArtShown { get; set; }

        public bool SkipAfterMix { get; set; }

        public TrackSelector.MixStrategy ShufflerMode { get; set; }

        public bool UpdateLibraryOnStartup { get; set; }

        public string ImportShufflerFilesFolder { get; set; }
    }
}