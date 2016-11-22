using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Windows.Controllers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Midi;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.AudioEngine.Plugins;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.Controls;
using Halloumi.Shuffler.Forms.TrackPlayerExtensions;
using AE = Halloumi.Shuffler.AudioEngine;
using Track = Halloumi.Shuffler.AudioLibrary.Models.Track;

namespace Halloumi.Shuffler.Forms
{
    /// <summary>
    /// </summary>
    public partial class FrmMain : BaseMinimizeToTrayForm
    {
        private FrmGeneratePlaylist _autoGenerateSettings;

        private bool _bassPlayerOnTrackChange;
        private FrmFadeNow _frmFadeNow;
        private FrmSampleLibrary _frmSampleLibrary;
        private FrmModuleEditor _frmModuleEditor;

        private FrmGeneratePlaylist _generatePlaylist;

        private MidiManager _midiManager;

        private FrmMonitorSettings _monitorSettings;

        private FrmPlugin _pluginForm;


        /// <summary>
        ///     Initializes a new instance of the frmMain class.
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();

            mnuPlugins.Click += mnuPlugins_Click;
            mnuAbout.Click += mnuAbout_Click;
            mnuSettings.Click += mnuSettings_Click;
            mnuExit.Click += mnuExit_Click;
            mnuExportPlaylistTracks.Click += mnuExportPlaylistTracks_Click;
            mnuConservativeFadeOut.Click += mnuConservativeFadeOut_Click;

            mnuWinampDSPConfig.Click += mnuWinampDSPConfig_Click;
            mnuVSTPluginConfig.Click += mnuVSTPluginConfig_Click;
            mnuSamplerVSTPluginConfig.Click += mnuSamplerVSTPluginConfig_Click;
            mnuTrackVSTPluginConfig.Click += mnuTrackVSTPluginConfig_Click;
            mnuSamplerVSTPluginConfig2.Click += mnuSamplerVSTPluginConfig2_Click;
            mnuTrackFXVSTPluginConfig.Click += mnuTrackFXVSTPluginConfig_Click;
            mnuTrackFXVSTPluginConfig2.Click += mnuTrackFXVSTPluginConfig2_Click;

            mnuViewLibrary.Click += mnuViewLibrary_Click;
            mnuViewMixer.Click += mnuViewMixer_Click;
            mnuViewPlaylist.Click += mnuViewPlaylist_Click;

            notificationContextMenu.Opening += notificationContextMenu_Opening;
            mnuPlayPause.Click += mnuPlayPause_Click;
            mnuPause.Click += mnuPause_Click;
            mnuNext.Click += mnuNext_Click;
            mnuPrevious.Click += mnuPrevious_Click;
            mnuSkipToEnd.Click += mnuSkipToEnd_Click;
            NotifyIcon.ContextMenuStrip = notificationContextMenu;

            BassPlayer = new AE.BassPlayer(Handle);
            Library = new Library(BassPlayer);

            BassPlayer.OnTrackChange += BassPlayer_OnTrackChange;
            BassPlayer.OnTrackQueued += BassPlayer_OnTrackQueued;

            LoadSettings();

            MixLibrary = new MixLibrary(Library.ShufflerFolder);

            SampleLibrary = new SampleLibrary(BassPlayer, Library);

            playlistControl.Library = Library;
            playlistControl.MixLibrary = MixLibrary;
            playlistControl.BassPlayer = BassPlayer;
            playlistControl.ToolStripLabel = lblPlaylistStatus;

            playerDetails.Library = Library;
            playerDetails.BassPlayer = BassPlayer;
            playerDetails.PlaylistControl = playlistControl;
            playerDetails.SetSelectedView(PlayerDetails.SelectedView.Playlist);
            playerDetails.SelectedViewChanged += playerDetails_SelectedViewChanged;

            playerDetails.MixLibrary = MixLibrary;
            playlistControl.MixLibrary = MixLibrary;

            trackLibraryControl.Library = Library;
            trackLibraryControl.BassPlayer = BassPlayer;
            trackLibraryControl.PlaylistControl = playlistControl;
            trackLibraryControl.MixLibrary = MixLibrary;
            trackLibraryControl.ToolStripLabel = lblLibraryStatus;
            trackLibraryControl.SamplerControl = mixerControl.SamplerControl;
            trackLibraryControl.SampleLibrary = SampleLibrary;

            mixerControl.Library = Library;
            mixerControl.BassPlayer = BassPlayer;
            mixerControl.PlaylistControl = playlistControl;

            Library.LoadFromDatabase();

            if (Library.GetTracks().Count > 0)
                Library.LoadPlaylists();

            trackLibraryControl.Initalize();

            SampleLibrary.LoadFromCache();

            mixerControl.Initialize();
            playerDetails.Initialize();
            playlistControl.Initalize(trackLibraryControl);

            shufflerController.PlaylistControl = playlistControl;
            shufflerController.LibraryControl = trackLibraryControl;
            shufflerController.BassPlayer = BassPlayer;
            shufflerController.Initalize();

            MixLibrary.AvailableTracks = Library.GetTracks();
            MixLibrary.LoadAllMixDetails();
            //var devices = AE.BassHelper.GetWaveOutDevices();

            SetView(PlayerDetails.SelectedView.Library);
        }

        public BassPlayerMidiMapper MidiMapper { get; private set; }

        private MixLibrary MixLibrary { get; }

        private Library Library { get; }

        private SampleLibrary SampleLibrary { get; }

        private AE.BassPlayer BassPlayer { get; }

        /// <summary>
        ///     Handles the LoadDocument event of the fileMenuController control.
        /// </summary>
        private void fileMenuController_LoadDocument(object sender, FileMenuControllerEventArgs e)
        {
            playlistControl.OpenPlaylist(e.FileName);
        }

        /// <summary>
        ///     Handles the SaveDocument event of the fileMenuController control.
        /// </summary>
        private void fileMenuController_SaveDocument(object sender, FileMenuControllerEventArgs e)
        {
            playlistControl.SavePlaylist(e.FileName);
        }

        private void mnuTrackRank_Click(object sender, EventArgs e)
        {
            var toolStripDropDownItem = sender as ToolStripDropDownItem;
            if (toolStripDropDownItem == null) return;

            var mixRankDescription = toolStripDropDownItem.Text;
            var mixRank = MixLibrary.GetRankFromDescription(mixRankDescription);

            var track = playlistControl.GetCurrentTrack();
            track.Rank = (int) mixRank;
            Library.SaveRank(track);
        }

        private void mnuViewVisuals_Click(object sender, EventArgs e)
        {
            mnuViewVisuals.Checked = !mnuViewVisuals.Checked;
            playerDetails.VisualsShown = mnuViewVisuals.Checked;
        }

        private void mnuLibrary_DropDownOpening(object sender, EventArgs e)
        {
            mnuUpdateLibrary.Enabled = !trackLibraryControl.IsLibraryUpdating();
            mnuCancelLibraryUpdate.Enabled = trackLibraryControl.IsLibraryUpdating();
        }

        private void mnuUpdateLibrary_Click(object sender, EventArgs e)
        {
            trackLibraryControl.ImportLibrary();
        }

        private void mnuCancelLibraryUpdate_Click(object sender, EventArgs e)
        {
            trackLibraryControl.CancelLibraryImport();
        }

        private void mnuUpdateLibraryOnStartup_Click(object sender, EventArgs e)
        {
            mnuUpdateLibraryOnStartup.Checked = !mnuUpdateLibraryOnStartup.Checked;
        }

        private void mnuReplayMix_Click(object sender, EventArgs e)
        {
            playlistControl.ReplayMix();
        }

        private void mnuViewAlbumArt_Click(object sender, EventArgs e)
        {
            mnuViewAlbumArt.Checked = !mnuViewAlbumArt.Checked;
            playerDetails.AlbumArtShown = mnuViewAlbumArt.Checked;
        }

        private void mnuCleanLibrary_Click(object sender, EventArgs e)
        {
            Library.CleanLibrary();
        }

        private void mnuSyncShufflerFiles_Click(object sender, EventArgs e)
        {
            var exportShufflerFiles = new FrmImportShufflerFiles
            {
                Library = Library,
                MixLibrary = MixLibrary
            };
            exportShufflerFiles.ShowDialog();
        }

        private void mnuUpdateDuplicateTracks_Click(object sender, EventArgs e)
        {
            var similarTracks = new FrmUpdateSimilarTracks
            {
                BassPlayer = BassPlayer,
                Library = Library,
                Tracks = trackLibraryControl.GetDisplayedTracks().ToList()
            };
            similarTracks.ShowDialog();
        }

        private void mnuAutoGenerateSettings_Click(object sender, EventArgs e)
        {
            if (_autoGenerateSettings == null || _autoGenerateSettings.IsDisposed)
            {
                _autoGenerateSettings = new FrmGeneratePlaylist
                {
                    LibraryControl = trackLibraryControl,
                    PlaylistControl = playlistControl
                };
                _autoGenerateSettings.SetScreenMode(FrmGeneratePlaylist.ScreenMode.AutoGenerateSettings);
            }
            if (!_autoGenerateSettings.Visible)
            {
                WindowHelper.ShowDialog(this, _autoGenerateSettings);
            }
        }

        private void mnuAutoshuffle_Click(object sender, EventArgs e)
        {
            shufflerController.AutoGenerateEnabled = mnuAutoshuffle.Checked;
        }

        private void mnuAutoGenerateNow_Click(object sender, EventArgs e)
        {
            shufflerController.AutoGenerateNow();
        }

        private void mnuExportLibraryTracks_Click(object sender, EventArgs e)
        {
            var tracks = trackLibraryControl.GetDisplayedTracks();
            const string playlistName = "";
            ExportTracks(tracks, playlistName);
        }

        private void mnuSampleLibrary_Click(object sender, EventArgs e)
        {
            if (_frmSampleLibrary == null || _frmSampleLibrary.IsDisposed)
            {
                _frmSampleLibrary = new FrmSampleLibrary();
                _frmSampleLibrary.Initialize(BassPlayer, SampleLibrary);
            }

            if (!_frmSampleLibrary.Visible)
            {
                WindowHelper.ShowDialog(this, _frmSampleLibrary);
            }
        }

        private void mnuFadeNow_Click(object sender, EventArgs e)
        {
            if (_frmFadeNow == null || _frmFadeNow.IsDisposed)
            {
                _frmFadeNow = new FrmFadeNow
                {
                    BassPlayer = BassPlayer,
                    Library = Library,
                    PlaylistControl = playlistControl
                };
            }
            if (!_frmFadeNow.Visible)
            {
                WindowHelper.ShowDialog(this, _frmFadeNow);
            }
        }

        private void playerDetails_SelectedViewChanged(object sender, EventArgs e)
        {
            SetView(playerDetails.GetSelectedView());
        }

        private void SetConservativeFadeOutSettings()
        {
            if (mnuConservativeFadeOut.Checked
                && BassPlayer.CurrentTrack != null && BassPlayer.NextTrack != null)
            {
                var track1 = Library.GetTrackByFilename(BassPlayer.CurrentTrack.Filename);
                var track2 = Library.GetTrackByFilename(BassPlayer.NextTrack.Filename);
                var mixRank = MixLibrary.GetMixLevel(track1, track2);
                var hasExtendedMix = MixLibrary.HasExtendedMix(track1, track2);

                if (mixRank <= 2 && !hasExtendedMix)
                    BassPlayer.SetConservativeFadeOutSettings();
            }
        }


        /// <summary>
        ///     Shows the plugin form.
        /// </summary>
        private void ShowPluginForm()
        {
            if (_pluginForm == null || _pluginForm.IsDisposed)
            {
                _pluginForm = new FrmPlugin(BassPlayer);
            }
            if (!_pluginForm.Visible)
            {
                WindowHelper.ShowDialog(this, _pluginForm);
            }
        }

        /// <summary>
        ///     Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            var settings = Settings.Default;
            Library.LibraryFolder = settings.LibraryFolder;
            Library.PlaylistFolder = settings.PlaylistFolder;
            ExtenedAttributesHelper.ExtendedAttributeFolder = settings.ShufflerFolder;
            PluginHelper.WaPluginsFolder = settings.WaPluginsFolder;
            PluginHelper.VstPluginsFolder = settings.VstPluginsFolder;
            formStateController.FormStateSettings = settings.FormStateSettings;
            BassPlayer.TrackFxAutomationEnabled = settings.EnableTrackFxAutomation;
            BassPlayer.SampleAutomationEnabled = settings.EnableSampleAutomation;
            KeyHelper.SetApplicationFolder(settings.KeyFinderFolder);
        }

        /// <summary>
        ///     Loads the UI settings.
        /// </summary>
        private void LoadUiSettings()
        {
            var settings = Settings.Default;
            //if (settings.LeftRightSplit != 0)
            //{
            //    try { splLeftRight.SplitterDistance = settings.LeftRightSplit; }
            //    catch { }
            //}

            //if (settings.TrackSplit != 0)
            //{
            //    try { splTrack.SplitterDistance = settings.TrackSplit; }
            //    catch { }
            //}

            if (settings.WaPlugin != "")
            {
                try
                {
                    BassPlayer.LoadWaPlugin(settings.WaPlugin);
                }
                catch
                {
                    // ignored
                }
            }

            if (settings.MainMixerVstPlugin != "")
            {
                try
                {
                    BassPlayer.LoadMainVstPlugin(settings.MainMixerVstPlugin, 0);
                }
                catch
                {
                    // ignored
                }
            }

            if (settings.MainMixerVstPlugin2 != "")
            {
                try
                {
                    BassPlayer.LoadMainVstPlugin(settings.MainMixerVstPlugin2, 1);
                }
                catch
                {
                    // ignored
                }
            }

            if (settings.MainMixerVstPluginParameters != "" && BassPlayer.MainVstPlugin != null)
            {
                try
                {
                    PluginHelper.SetVstPluginParameters(BassPlayer.MainVstPlugin, settings.MainMixerVstPluginParameters);
                }
                catch
                {
                    // ignored
                }
            }

            if (settings.MainMixerVstPlugin2Parameters != "" && BassPlayer.MainVstPlugin2 != null)
            {
                try
                {
                    PluginHelper.SetVstPluginParameters(BassPlayer.MainVstPlugin2,
                        settings.MainMixerVstPlugin2Parameters);
                }
                catch
                {
                    // ignored
                }
            }

            if (settings.SamplerVstPlugin != "")
            {
                try
                {
                    BassPlayer.LoadSamplerVstPlugin(settings.SamplerVstPlugin, 0);
                }
                catch
                {
                    // ignored
                }
            }

            if (settings.SamplerVstPluginParameters != "" && BassPlayer.SamplerVstPlugin != null)
            {
                try
                {
                    PluginHelper.SetVstPluginParameters(BassPlayer.SamplerVstPlugin, settings.SamplerVstPluginParameters);
                }
                catch
                {
                    // ignored
                }
            }

            if (settings.SamplerVstPlugin2 != "")
            {
                try
                {
                    BassPlayer.LoadSamplerVstPlugin(settings.SamplerVstPlugin2, 1);
                }
                catch
                {
                    // ignored
                }
            }

            if (settings.SamplerVstPlugin2Parameters != "" && BassPlayer.SamplerVstPlugin2 != null)
            {
                try
                {
                    PluginHelper.SetVstPluginParameters(BassPlayer.SamplerVstPlugin2,
                        settings.SamplerVstPlugin2Parameters);
                }
                catch
                {
                    // ignored
                }
            }

            if (settings.TrackVstPlugin != "")
            {
                try
                {
                    BassPlayer.LoadTracksVstPlugin(settings.TrackVstPlugin, 0);
                }
                catch
                {
                    // ignored
                }
            }

            if (settings.TrackVstPluginParameters != "" && BassPlayer.TrackVstPlugin != null)
            {
                try
                {
                    PluginHelper.SetVstPluginParameters(BassPlayer.TrackVstPlugin, settings.TrackVstPluginParameters);
                }
                catch
                {
                    // ignored
                }
            }

            if (settings.TrackFxvstPlugin != "")
            {
                try
                {
                    BassPlayer.LoadTrackSendFxvstPlugin(settings.TrackFxvstPlugin, 0);
                }
                catch
                {
                    // ignored
                }
            }

            if (settings.TrackFxvstPluginParameters != "" && BassPlayer.TrackSendFxvstPlugin != null)
            {
                try
                {
                    PluginHelper.SetVstPluginParameters(BassPlayer.TrackSendFxvstPlugin,
                        settings.TrackFxvstPluginParameters);
                }
                catch
                {
                    // ignored
                }
            }

            if (settings.TrackFxvstPlugin2 != "")
            {
                try
                {
                    BassPlayer.LoadTrackSendFxvstPlugin(settings.TrackFxvstPlugin2, 1);
                }
                catch
                {
                    // ignored
                }
            }

            if (settings.TrackFxvstPlugin2Parameters != "" && BassPlayer.TrackSendFxvstPlugin2 != null)
            {
                try
                {
                    PluginHelper.SetVstPluginParameters(BassPlayer.TrackSendFxvstPlugin2,
                        settings.TrackFxvstPlugin2Parameters);
                }
                catch
                {
                    // ignored
                }
            }

            MinimizeToTrayEnabled = (settings.MinimizeToTray);
            mnuMinimizeToTray.Checked = (settings.MinimizeToTray);

            mnuShowMixableTracks.Checked = (settings.ShowMixableTracks);
            mnuShowTrackDetails.Checked = (settings.ShowTrackDetails);

            playlistControl.ShowMixableTracks = mnuShowMixableTracks.Checked;
            trackLibraryControl.ShowMixableTracks = mnuShowMixableTracks.Checked;

            BassPlayer.LimitSongLength = settings.LimitSongLength;
            mnuConservativeFadeOut.Checked = settings.LimitSongLength;
            BassPlayer.LimitSongLength = mnuConservativeFadeOut.Checked;

            BassPlayer.SetMonitorVolume(settings.MonitorVolume);

            playerDetails.VisualsShown = settings.VisualsShown;
            mnuViewVisuals.Checked = settings.VisualsShown;

            playerDetails.AlbumArtShown = settings.AlbumArtShown;
            mnuViewAlbumArt.Checked = settings.AlbumArtShown;

            mnuSkipAfterMix.Checked = settings.SkipAfterMix;

            mixerControl.LoadSettings();

            trackLibraryControl.LoadUiSettings();

            //this.shufflerControl.CurrentShufflerMode = settings.ShufflerMode;

            mnuUpdateLibraryOnStartup.Checked = settings.UpdateLibraryOnStartup;

            fileMenuController.RecentFiles = settings.RecentFiles;


            _midiManager = new MidiManager();
            MidiMapper = new BassPlayerMidiMapper(BassPlayer, _midiManager);
        }

        /// <summary>
        ///     Saves the settings.
        /// </summary>
        private void SaveSettings()
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
            if (BassPlayer.TrackSendFxvstPlugin != null) trackFxVstPlugin = BassPlayer.TrackSendFxvstPlugin.Location;
            settings.TrackFxvstPlugin = trackFxVstPlugin;

            var trackFxVstPluginParameters = "";
            if (BassPlayer.TrackSendFxvstPlugin != null)
                trackFxVstPluginParameters = PluginHelper.GetVstPluginParameters(BassPlayer.TrackSendFxvstPlugin);
            settings.TrackFxvstPluginParameters = trackFxVstPluginParameters;

            var trackFxVstPlugin2 = "";
            if (BassPlayer.TrackSendFxvstPlugin2 != null) trackFxVstPlugin2 = BassPlayer.TrackSendFxvstPlugin2.Location;
            settings.TrackFxvstPlugin2 = trackFxVstPlugin2;

            var trackFxVstPluginParameters2 = "";
            if (BassPlayer.TrackSendFxvstPlugin2 != null)
                trackFxVstPluginParameters2 = PluginHelper.GetVstPluginParameters(BassPlayer.TrackSendFxvstPlugin2);
            settings.TrackFxvstPlugin2Parameters = trackFxVstPluginParameters2;

            settings.SkipAfterMix = mnuSkipAfterMix.Checked;

            settings.MinimizeToTray = MinimizeToTrayEnabled;
            settings.LimitSongLength = BassPlayer.LimitSongLength;
            settings.FormStateSettings = formStateController.FormStateSettings;
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

            settings.VisualsShown = playerDetails.VisualsShown;
            settings.AlbumArtShown = playerDetails.AlbumArtShown;
            settings.ShowMixableTracks = mnuShowMixableTracks.Checked;
            settings.ShowTrackDetails = mnuShowTrackDetails.Checked;

            settings.UpdateLibraryOnStartup = mnuUpdateLibraryOnStartup.Checked;

            settings.RecentFiles = fileMenuController.RecentFiles;

            settings.Save();

            trackLibraryControl.SaveSettings();
        }

        /// <summary>
        ///     Sets the icon text.
        /// </summary>
        private void SetIconText()
        {
            if (BassPlayer.CurrentTrack != null)
            {
                var text = BassPlayer.CurrentTrack.Description.Replace("&", "&&");
                if (text.Length > 63) text = text.Substring(0, 63);
                NotifyIcon.Text = text;
            }
            else NotifyIcon.Text = "";
        }


        /// <summary>
        ///     Handles the FormClosed event of the frmMain control.
        /// </summary>
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            mixerControl.Unload();
            Library.SaveToDatabase();
            BassPlayer.Dispose();
            _midiManager.Dispose();
        }

        /// <summary>
        ///     Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange(object sender, EventArgs e)
        {
            //if (!this.IsHandleCreated) return;
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(BassPlayer_OnTrackChange));
            }
            else BassPlayer_OnTrackChange();
        }

        /// <summary>
        ///     Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange()
        {
            if (_bassPlayerOnTrackChange) return;
            _bassPlayerOnTrackChange = true;

            SetIconText();

            _bassPlayerOnTrackChange = false;
        }

        /// <summary>
        ///     Handles the Click event of the mnuVSTPluginConfig control.
        /// </summary>
        private void mnuVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.MainVstPlugin == null)
                ShowPluginForm();
            else
                PluginHelper.ShowVstPluginConfig(BassPlayer.MainVstPlugin);
        }

        /// <summary>
        ///     Handles the Click event of the mnuWinampDSPConfig control.
        /// </summary>
        private void mnuWinampDSPConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.WaPlugin == null)
                ShowPluginForm();
            else
                PluginHelper.ShowWaPluginConfig(BassPlayer.WaPlugin);
        }

        /// <summary>
        ///     Handles the Click event of the mnuSamplerVSTPluginConfig control.
        /// </summary>
        private void mnuSamplerVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.SamplerVstPlugin == null)
                ShowPluginForm();
            else
                PluginHelper.ShowVstPluginConfig(BassPlayer.SamplerVstPlugin);
        }

        /// <summary>
        ///     Handles the Click event of the mnuTrackVSTPluginConfig control.
        /// </summary>
        private void mnuTrackVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.TrackVstPlugin == null)
                ShowPluginForm();
            else
                PluginHelper.ShowVstPluginConfig(BassPlayer.TrackVstPlugin);
        }

        /// <summary>
        ///     Handles the Click event of the mnuTrackFXVSTPluginConfig control.
        /// </summary>
        private void mnuTrackFXVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.TrackSendFxvstPlugin == null)
                ShowPluginForm();
            else
                PluginHelper.ShowVstPluginConfig(BassPlayer.TrackSendFxvstPlugin);
        }

        /// <summary>
        ///     Handles the Click event of the mnuTrackFXVSTPluginConfig control.
        /// </summary>
        private void mnuTrackFXVSTPluginConfig2_Click(object sender, EventArgs e)
        {
            if (BassPlayer.TrackSendFxvstPlugin2 == null)
                ShowPluginForm();
            else
                PluginHelper.ShowVstPluginConfig(BassPlayer.TrackSendFxvstPlugin2);
        }

        /// <summary>
        ///     Handles the Click event of the mnuSamplerVSTPluginConfig2 control.
        /// </summary>
        private void mnuSamplerVSTPluginConfig2_Click(object sender, EventArgs e)
        {
            if (BassPlayer.SamplerVstPlugin2 == null)
                ShowPluginForm();
            else
                PluginHelper.ShowVstPluginConfig(BassPlayer.SamplerVstPlugin2);
        }

        /// <summary>
        ///     Handles the Click event of the mnuPlugins control.
        /// </summary>
        private void mnuPlugins_Click(object sender, EventArgs e)
        {
            ShowPluginForm();
        }

        /// <summary>
        ///     Handles the Click event of the mnuViewMixer control.
        /// </summary>
        private void mnuViewMixer_Click(object sender, EventArgs e)
        {
            playerDetails.SetSelectedView(PlayerDetails.SelectedView.Mixer);
        }

        /// <summary>
        ///     Handles the Click event of the mnuViewLibrary control.
        /// </summary>
        private void mnuViewLibrary_Click(object sender, EventArgs e)
        {
            playerDetails.SetSelectedView(PlayerDetails.SelectedView.Library);
        }

        /// <summary>
        ///     Handles the Click event of the mnuViewPlaylist control.
        /// </summary>
        private void mnuViewPlaylist_Click(object sender, EventArgs e)
        {
            playerDetails.SetSelectedView(PlayerDetails.SelectedView.Playlist);
        }

        private void SetView(PlayerDetails.SelectedView view)
        {
            trackLibraryControl.Visible = (view == PlayerDetails.SelectedView.Library);
            mnuViewLibrary.Checked = (view == PlayerDetails.SelectedView.Library);

            mixerControl.Visible = (view == PlayerDetails.SelectedView.Mixer);
            mnuViewMixer.Checked = (view == PlayerDetails.SelectedView.Mixer);

            playlistControl.Visible = (view == PlayerDetails.SelectedView.Playlist);
            mnuViewPlaylist.Checked = (view == PlayerDetails.SelectedView.Playlist);

            playerDetails.SetSelectedView(view);
        }

        /// <summary>
        ///     Handles the Click event of the mnuLimitSongLength control.
        /// </summary>
        private void mnuConservativeFadeOut_Click(object sender, EventArgs e)
        {
            mnuConservativeFadeOut.Checked = !mnuConservativeFadeOut.Checked;
            BassPlayer.LimitSongLength = mnuConservativeFadeOut.Checked;
            SetConservativeFadeOutSettings();
        }

        /// <summary>
        ///     Handles the OnTrackQueued event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackQueued(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate { SetConservativeFadeOutSettings(); }));
            }
            else
            {
                SetConservativeFadeOutSettings();
            }
        }

        /// <summary>
        ///     Handles the FormClosing event of the frmMain control.
        /// </summary>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        ///     Handles the Click event of the mnuSettings control.
        /// </summary>
        public void mnuSettings_Click(object sender, EventArgs e)
        {
            var settings = new FrmSettings();
            settings.ShowDialog();
        }

        /// <summary>
        ///     Handles the Click event of the mnuAbout control.
        /// </summary>
        private void mnuAbout_Click(object sender, EventArgs e)
        {
            aboutDialog.Show();
        }

        /// <summary>
        ///     Handles the Click event of the mnuExit control.
        /// </summary>
        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
            NotifyIcon.Visible = false;
        }

        /// <summary>
        ///     Handles the Load event of the frmMain control.
        /// </summary>
        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadUiSettings();

            if (mnuUpdateLibraryOnStartup.Checked) trackLibraryControl.ImportLibrary();
        }

        /// <summary>
        ///     Handles the Resize event of the frmMain control.
        /// </summary>
        private void frmMain_Resize(object sender, EventArgs e)
        {
        }

        ///// <summary>
        /////     Handles the Click event of the mnuMinimizeToTray control.
        ///// </summary>
        //private void mnuMinimizeToTray_Click(object sender, EventArgs e)
        //{
        //    MinimizeToTrayEnabled = !MinimizeToTrayEnabled;
        //    mnuMinimizeToTray.Checked = MinimizeToTrayEnabled;
        //    SaveSettings();
        //}

        /// <summary>
        ///     Handles the Click event of the mnuMinimizeToTray control.
        /// </summary>
        private void mnuShowMixableTracks_Click(object sender, EventArgs e)
        {
            mnuShowMixableTracks.Checked = !mnuShowMixableTracks.Checked;
            trackLibraryControl.ShowMixableTracks = mnuShowMixableTracks.Checked;
            playlistControl.ShowMixableTracks = mnuShowMixableTracks.Checked;

            SaveSettings();
        }

        /// <summary>
        ///     Handles the Click event of the mnuRank control.
        /// </summary>
        private void mnuRank_Click(object sender, EventArgs e)
        {
            var toolStripDropDownItem = sender as ToolStripDropDownItem;
            if (toolStripDropDownItem != null)
            {
                var mixRankDescription = toolStripDropDownItem.Text;
                var mixRank = MixLibrary.GetRankFromDescription(mixRankDescription);
                playerDetails.SetCurrentMixRank((int) mixRank);
            }
            playerDetails.DisplayCurrentTrackDetails();

            if (mnuSkipAfterMix.Checked) BassPlayer.SkipToEnd();
        }

        /// <summary>
        ///     Handles the Click event of the mnuSkipToEnd control.
        /// </summary>
        private void mnuSkipToEnd_Click(object sender, EventArgs e)
        {
            BassPlayer.SkipToEnd();
        }

        /// <summary>
        ///     Handles the Click event of the mnuPrevious control.
        /// </summary>
        private void mnuPrevious_Click(object sender, EventArgs e)
        {
            playlistControl.PlayPreviousTrack();
        }

        /// <summary>
        ///     Handles the Click event of the mnuNext control.
        /// </summary>
        private void mnuNext_Click(object sender, EventArgs e)
        {
            playlistControl.PlayNextTrack();
        }

        /// <summary>
        ///     Handles the Click event of the mnuPlayPause control.
        /// </summary>
        private void mnuPlayPause_Click(object sender, EventArgs e)
        {
            BassPlayer.Play();
        }

        /// <summary>
        ///     Handles the Click event of the mnuPlayPause control.
        /// </summary>
        private void mnuPause_Click(object sender, EventArgs e)
        {
            BassPlayer.Pause();
        }

        /// <summary>
        ///     Handles the Click event of the mnuExportPlaylist control.
        /// </summary>
        private void mnuExportPlaylistTracks_Click(object sender, EventArgs e)
        {
            var tracks = playlistControl.GetTracks();

            var playlistName = (!string.IsNullOrEmpty(playlistControl.CurrentPlaylistFile))
                ? Path.GetFileNameWithoutExtension(playlistControl.CurrentPlaylistFile)
                : trackLibraryControl.PlaylistFilter;

            ExportTracks(tracks, playlistName);
        }

        private void ExportTracks(List<Track> tracks, string playlistName)
        {
            var exportPlaylist = new FrmExportPlaylist
            {
                Library = Library,
                Tracks = tracks,
                PlaylistName = playlistName
            };
            exportPlaylist.ShowDialog();
        }

        /// <summary>
        ///     Handles the Opening event of the notificationContextMenu control.
        /// </summary>
        private void notificationContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (BassPlayer.PlayState == PlayState.Playing)
            {
                mnuPlayPause.Visible = false;
                mnuPause.Visible = true;
            }
            else
            {
                mnuPlayPause.Visible = true;
                mnuPause.Visible = false;
            }
            BindMixRankMenu();
            BindTrackRankMenu();
        }

        /// <summary>
        ///     Binds the mix rank menu.
        /// </summary>
        private void BindMixRankMenu()
        {
            var currentMixRank = playerDetails.GetCurrentMixRank();
            for (var i = 0; i < 6; i++)
            {
                mnuRank.DropDownItems[i].Text = MixLibrary.GetRankDescription(5 - i);
                ((ToolStripMenuItem) mnuRank.DropDownItems[i]).Checked = ((5 - i) == currentMixRank);
            }
        }

        /// <summary>
        ///     Binds the track rank menu.
        /// </summary>
        private void BindTrackRankMenu()
        {
            var currentMixRank = -1;
            if (playlistControl.GetCurrentTrack() != null)
            {
                currentMixRank = playlistControl.GetCurrentTrack().Rank;
            }
            for (var i = 0; i < 6; i++)
            {
                mnuTrackRank.DropDownItems[i].Text = MixLibrary.GetRankDescription(5 - i);
                ((ToolStripMenuItem) mnuTrackRank.DropDownItems[i]).Checked = ((5 - i) == currentMixRank);
            }
        }

        private void mnuGeneratePlaylist_Click(object sender, EventArgs e)
        {
            if (_generatePlaylist == null || _generatePlaylist.IsDisposed)
            {
                _generatePlaylist = new FrmGeneratePlaylist
                {
                    LibraryControl = trackLibraryControl,
                    PlaylistControl = playlistControl
                };
                _generatePlaylist.SetScreenMode(FrmGeneratePlaylist.ScreenMode.GeneratePlaylist);
            }
            if (!_generatePlaylist.Visible)
            {
                WindowHelper.ShowDialog(this, _generatePlaylist);
            }
        }

        private void mnuMonitorSettings_Click(object sender, EventArgs e)
        {
            if (_monitorSettings == null || _monitorSettings.IsDisposed)
            {
                _monitorSettings = new FrmMonitorSettings {BassPlayer = BassPlayer};
            }
            if (!_monitorSettings.Visible)
            {
                WindowHelper.ShowDialog(this, _monitorSettings);
            }
        }

        private void mnuExportShufflerTracks_Click(object sender, EventArgs e)
        {
            var exportPlaylist = new FrmExportShufflerTracks
            {
                Library = Library,
                SampleLibrary = SampleLibrary
            };
            exportPlaylist.ShowDialog();
        }

        private void mnuModuleEditor_Click(object sender, EventArgs e)
        {
            if (_frmModuleEditor == null || _frmModuleEditor.IsDisposed)
            {
                _frmModuleEditor = new FrmModuleEditor();
                _frmModuleEditor.Initialize(BassPlayer, SampleLibrary, Library);
            }

            if (!_frmModuleEditor.Visible)
            {
                WindowHelper.ShowDialog(this, _frmModuleEditor);
            }
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            trackLibraryControl.InitialBind();
        }

        private void mnuShowTrackDetails_Click(object sender, EventArgs e)
        {
            mnuShowTrackDetails.Checked = !mnuShowTrackDetails.Checked;
            trackLibraryControl.ShowTrackDetails = mnuShowTrackDetails.Checked;
            playlistControl.ShowTrackDetails = mnuShowTrackDetails.Checked;

            SaveSettings();
        }
    }
}