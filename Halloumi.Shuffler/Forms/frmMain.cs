using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Windows.Controllers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.Controls;
using AE = Halloumi.Shuffler.AudioEngine;
using Track = Halloumi.Shuffler.AudioLibrary.Models.Track;

namespace Halloumi.Shuffler.Forms
{
    /// <summary>
    /// </summary>
    public partial class FrmMain : BaseMinimizeToTrayForm
    {
        private readonly ShufflerApplication _application;
        private FrmGeneratePlaylist _autoGenerateSettings;
        private FrmModuleEditor _frmModuleEditor;
        private FrmImportShufflerTracks _frmImportShufflerTracks;


        private FrmSampleLibrary _frmSampleLibrary;

        private FrmGeneratePlaylist _generatePlaylist;
        private FrmMonitorSettings _monitorSettings;


        /// <summary>
        ///     Initializes a new instance of the frmMain class.
        /// </summary>
        public FrmMain(ShufflerApplication application)
        {
            InitializeComponent();

            _application = application;
            _application.BaseForm = this;

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


            var settings = Settings.Default;
            formStateController.FormStateSettings = settings.FormStateSettings;

            playlistControl.Library = application.Library;
            playlistControl.MixLibrary = application.MixLibrary;
            playlistControl.BassPlayer = application.BassPlayer;
            playlistControl.ToolStripLabel = lblPlaylistStatus;

            playerDetails.Library = application.Library;
            playerDetails.BassPlayer = application.BassPlayer;
            playerDetails.PlaylistControl = playlistControl;
            playerDetails.SetSelectedView(PlayerDetails.SelectedView.Playlist);
            playerDetails.SelectedViewChanged += playerDetails_SelectedViewChanged;
            playerDetails.ToolStripLabel = lblPlayerStatus;

            playerDetails.MixLibrary = application.MixLibrary;
            playlistControl.MixLibrary = application.MixLibrary;

            trackLibraryControl.Library = application.Library;
            trackLibraryControl.BassPlayer = application.BassPlayer;
            trackLibraryControl.PlaylistControl = playlistControl;
            trackLibraryControl.MixLibrary = application.MixLibrary;
            trackLibraryControl.ToolStripLabel = lblLibraryStatus;
            trackLibraryControl.SamplerControl = mixerControl.SamplerControl;
            trackLibraryControl.SampleLibrary = application.SampleLibrary;

            mixerControl.Library = application.Library;
            mixerControl.BassPlayer = application.BassPlayer;
            mixerControl.PlaylistControl = playlistControl;

            trackLibraryControl.Initalize();

            mixerControl.Initialize();
            playerDetails.Initialize();
            playlistControl.Initalize(trackLibraryControl);

            shufflerController.PlaylistControl = playlistControl;
            shufflerController.LibraryControl = trackLibraryControl;
            shufflerController.BassPlayer = application.BassPlayer;
            shufflerController.Initalize();

            SetView(PlayerDetails.SelectedView.Library);

            var newMenu = new ToolStripMenuItem(mnuPlayPause.Text, mnuPlayPause.Image);
            newMenu.Click += mnuPlayPause_Click;
            mnuFile.DropDownItems.Insert(0, newMenu);

            newMenu = new ToolStripMenuItem(mnuPause.Text, mnuPause.Image);
            newMenu.Click += mnuPause_Click;
            mnuFile.DropDownItems.Insert(1, newMenu);

            newMenu = new ToolStripMenuItem(mnuNext.Text, mnuNext.Image);
            newMenu.Click += mnuNext_Click;
            mnuFile.DropDownItems.Insert(2, newMenu);

            newMenu = new ToolStripMenuItem(mnuPrevious.Text, mnuPrevious.Image);
            newMenu.Click += mnuPrevious_Click;
            mnuFile.DropDownItems.Insert(3, newMenu);

            newMenu = new ToolStripMenuItem(mnuSkipToEnd.Text, mnuSkipToEnd.Image);
            newMenu.Click += mnuSkipToEnd_Click;
            mnuFile.DropDownItems.Insert(4, newMenu);

            newMenu = new ToolStripMenuItem(mnuReplayMix.Text, mnuReplayMix.Image);
            newMenu.Click += mnuReplayMix_Click;
            mnuFile.DropDownItems.Insert(5, newMenu);

            mnuFile.DropDownItems.Insert(6, new ToolStripSeparator());
        }


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
            var mixRank = _application.MixLibrary.GetRankFromDescription(mixRankDescription);

            var track = playlistControl.GetCurrentTrack();
            if (track == null)
                return;

            var tracks = new List<Track> { track };
            _application.Library.SetRank(tracks, (int)mixRank);
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
            _application.Library.CleanLibrary();
        }

        private void mnuUpdateDuplicateTracks_Click(object sender, EventArgs e)
        {
            var similarTracks = new FrmUpdateSimilarTracks
            {
                BassPlayer = _application.BassPlayer,
                Library = _application.Library,
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
                WindowHelper.ShowDialog(this, _autoGenerateSettings);
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
            var tracks = trackLibraryControl.DisplayedTracks;
            const string playlistName = "";
            ExportTracks(tracks, playlistName);
        }

        private void mnuSampleLibrary_Click(object sender, EventArgs e)
        {
            if (_frmSampleLibrary == null || _frmSampleLibrary.IsDisposed)
            {
                _frmSampleLibrary = new FrmSampleLibrary();
                _frmSampleLibrary.Initialize(_application.BassPlayer, _application.SampleLibrary);
            }

            if (!_frmSampleLibrary.Visible)
                WindowHelper.ShowDialog(this, _frmSampleLibrary);
        }

        private void playerDetails_SelectedViewChanged(object sender, EventArgs e)
        {
            SetView(playerDetails.GetSelectedView());
        }


        /// <summary>
        ///     Loads the UI settings.
        /// </summary>
        private void LoadUiSettings()
        {
            var settings = Settings.Default;

            MinimizeToTrayEnabled = settings.MinimizeToTray;
            mnuMinimizeToTray.Checked = settings.MinimizeToTray;
            mnuShowMixableTracks.Checked = settings.ShowMixableTracks;
            mnuShowTrackDetails.Checked = settings.ShowTrackDetails;
            trackLibraryControl.ShowTrackDetails = mnuShowTrackDetails.Checked;
            playlistControl.ShowTrackDetails = mnuShowTrackDetails.Checked;
            playlistControl.ShowMixableTracks = mnuShowMixableTracks.Checked;
            trackLibraryControl.ShowMixableTracks = mnuShowMixableTracks.Checked;
            mnuConservativeFadeOut.Checked = settings.LimitSongLength;
            playerDetails.VisualsShown = settings.VisualsShown;
            mnuViewVisuals.Checked = settings.VisualsShown;
            playerDetails.AlbumArtShown = settings.AlbumArtShown;
            mnuViewAlbumArt.Checked = settings.AlbumArtShown;
            mnuSkipAfterMix.Checked = settings.SkipAfterMix;
            mnuShowPlayer.Checked = settings.ShowPlayer;
            playerDetails.Visible = mnuShowPlayer.Checked;
            pnlTop.AutoSize = !mnuShowPlayer.Checked;

            mixerControl.LoadSettings();
            trackLibraryControl.LoadUiSettings();

            mnuUpdateLibraryOnStartup.Checked = settings.UpdateLibraryOnStartup;

            fileMenuController.RecentFiles = settings.RecentFiles;

            playlistControl.LoadWorkingPlaylist();
        }

        /// <summary>
        ///     Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            var settings = Settings.Default;
            settings.SkipAfterMix = mnuSkipAfterMix.Checked;
            settings.MinimizeToTray = MinimizeToTrayEnabled;
            settings.FormStateSettings = formStateController.FormStateSettings;
            settings.VisualsShown = playerDetails.VisualsShown;
            settings.AlbumArtShown = playerDetails.AlbumArtShown;
            settings.ShowMixableTracks = mnuShowMixableTracks.Checked;
            settings.ShowTrackDetails = mnuShowTrackDetails.Checked;
            settings.UpdateLibraryOnStartup = mnuUpdateLibraryOnStartup.Checked;
            settings.RecentFiles = fileMenuController.RecentFiles;
            settings.ShowPlayer = mnuShowPlayer.Checked;
            settings.Save();

            trackLibraryControl.SaveSettings();
        }


        /// <summary>
        ///     Handles the FormClosed event of the frmMain control.
        /// </summary>
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            mixerControl.Unload();
            _application.Unload();
        }


        /// <summary>
        ///     Handles the Click event of the mnuVSTPluginConfig control.
        /// </summary>
        private void mnuVSTPluginConfig_Click(object sender, EventArgs e)
        {
            _application.ShowPlugin(_application.BassPlayer.MainVstPlugin);
        }

        /// <summary>
        ///     Handles the Click event of the mnuWinampDSPConfig control.
        /// </summary>
        private void mnuWinampDSPConfig_Click(object sender, EventArgs e)
        {
            _application.ShowPlugin(_application.BassPlayer.WaPlugin);
        }

        /// <summary>
        ///     Handles the Click event of the mnuSamplerVSTPluginConfig control.
        /// </summary>
        private void mnuSamplerVSTPluginConfig_Click(object sender, EventArgs e)
        {
            _application.ShowPlugin(_application.BassPlayer.SamplerVstPlugin);
        }

        /// <summary>
        ///     Handles the Click event of the mnuTrackVSTPluginConfig control.
        /// </summary>
        private void mnuTrackVSTPluginConfig_Click(object sender, EventArgs e)
        {
            _application.ShowPlugin(_application.BassPlayer.TrackVstPlugin);
        }

        /// <summary>
        ///     Handles the Click event of the mnuTrackFXVSTPluginConfig control.
        /// </summary>
        private void mnuTrackFXVSTPluginConfig_Click(object sender, EventArgs e)
        {
            _application.ShowPlugin(_application.BassPlayer.TrackSendFxVstPlugin);
        }

        /// <summary>
        ///     Handles the Click event of the mnuTrackFXVSTPluginConfig control.
        /// </summary>
        private void mnuTrackFXVSTPluginConfig2_Click(object sender, EventArgs e)
        {
            _application.ShowPlugin(_application.BassPlayer.TrackSendFxVstPlugin2);
        }

        /// <summary>
        ///     Handles the Click event of the mnuSamplerVSTPluginConfig2 control.
        /// </summary>
        private void mnuSamplerVSTPluginConfig2_Click(object sender, EventArgs e)
        {
            _application.ShowPlugin(_application.BassPlayer.SamplerVstPlugin2);
        }

        /// <summary>
        ///     Handles the Click event of the mnuPlugins control.
        /// </summary>
        private void mnuPlugins_Click(object sender, EventArgs e)
        {
            _application.ShowPluginsForm();
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
            trackLibraryControl.Visible = view == PlayerDetails.SelectedView.Library;
            mnuViewLibrary.Checked = view == PlayerDetails.SelectedView.Library;

            mixerControl.Visible = view == PlayerDetails.SelectedView.Mixer;
            mnuViewMixer.Checked = view == PlayerDetails.SelectedView.Mixer;

            playlistControl.Visible = view == PlayerDetails.SelectedView.Playlist;
            mnuViewPlaylist.Checked = view == PlayerDetails.SelectedView.Playlist;

            playerDetails.SetSelectedView(view);
        }

        /// <summary>
        ///     Handles the Click event of the mnuLimitSongLength control.
        /// </summary>
        private void mnuConservativeFadeOut_Click(object sender, EventArgs e)
        {
            mnuConservativeFadeOut.Checked = !mnuConservativeFadeOut.Checked;
            _application.BassPlayer.LimitSongLength = mnuConservativeFadeOut.Checked;
            _application.UseConservativeFadeOut = mnuConservativeFadeOut.Checked;
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
                var mixRank = _application.MixLibrary.GetRankFromDescription(mixRankDescription);
                playerDetails.SetCurrentMixRank((int)mixRank);
            }
            playerDetails.DisplayCurrentTrackDetails();

            if (mnuSkipAfterMix.Checked) _application.BassPlayer.SkipToFadeOut();
        }

        /// <summary>
        ///     Handles the Click event of the mnuSkipToEnd control.
        /// </summary>
        private void mnuSkipToEnd_Click(object sender, EventArgs e)
        {
            _application.BassPlayer.SkipToFadeOut();
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
            _application.BassPlayer.Play();
        }

        /// <summary>
        ///     Handles the Click event of the mnuPlayPause control.
        /// </summary>
        private void mnuPause_Click(object sender, EventArgs e)
        {
            _application.BassPlayer.Pause();
        }

        /// <summary>
        ///     Handles the Click event of the mnuExportPlaylist control.
        /// </summary>
        private void mnuExportPlaylistTracks_Click(object sender, EventArgs e)
        {
            var tracks = playlistControl.GetTracks();

            var playlistName = !string.IsNullOrEmpty(playlistControl.CurrentPlaylistFile)
                ? Path.GetFileNameWithoutExtension(playlistControl.CurrentPlaylistFile)
                : trackLibraryControl.CollectionFilter;

            ExportTracks(tracks, playlistName);
        }

        private void ExportTracks(List<Track> tracks, string playlistName)
        {
            var exportPlaylist = new FrmExportPlaylist
            {
                Library = _application.Library,
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
            if (_application.BassPlayer.PlayState == PlayState.Playing)
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
                mnuRank.DropDownItems[i].Text = _application.MixLibrary.GetRankDescription(5 - i);
                ((ToolStripMenuItem)mnuRank.DropDownItems[i]).Checked = 5 - i == currentMixRank;
            }
        }

        /// <summary>
        ///     Binds the track rank menu.
        /// </summary>
        private void BindTrackRankMenu()
        {
            var currentMixRank = -1;
            if (playlistControl.GetCurrentTrack() != null)
                currentMixRank = playlistControl.GetCurrentTrack().Rank;
            for (var i = 0; i < 6; i++)
            {
                mnuTrackRank.DropDownItems[i].Text = _application.MixLibrary.GetRankDescription(5 - i);
                ((ToolStripMenuItem)mnuTrackRank.DropDownItems[i]).Checked = 5 - i == currentMixRank;
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
                WindowHelper.ShowDialog(this, _generatePlaylist);
        }

        private void mnuMonitorSettings_Click(object sender, EventArgs e)
        {
            if (_monitorSettings == null || _monitorSettings.IsDisposed)
                _monitorSettings = new FrmMonitorSettings { BassPlayer = _application.BassPlayer };
            if (!_monitorSettings.Visible)
                WindowHelper.ShowDialog(this, _monitorSettings);
        }

        private void mnuExportShufflerTracks_Click(object sender, EventArgs e)
        {
            var exportPlaylist = new FrmExportShufflerTracks
            {
                Library = _application.Library,
                SampleLibrary = _application.SampleLibrary
            };
            exportPlaylist.ShowDialog();
        }

        private void mnuModuleEditor_Click(object sender, EventArgs e)
        {
            if (_frmModuleEditor == null || _frmModuleEditor.IsDisposed)
            {
                _frmModuleEditor = new FrmModuleEditor();
                _frmModuleEditor.Initialize(_application.BassPlayer, _application.SampleLibrary, _application.Library);
            }

            if (!_frmModuleEditor.Visible)
                WindowHelper.ShowDialog(this, _frmModuleEditor);
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

        private void mnuImportCollection_Click(object sender, EventArgs e)
        {
            trackLibraryControl.ImportCollection();
        }

        private void mnuDeleteCollection_Click(object sender, EventArgs e)
        {
            trackLibraryControl.DeleteCollection();
        }

        private void mnuShowPlayer_Click(object sender, EventArgs e)
        {
            mnuShowPlayer.Checked = !mnuShowPlayer.Checked;
            playerDetails.Visible = mnuShowPlayer.Checked;
            pnlTop.AutoSize = !mnuShowPlayer.Checked;

            SaveSettings();

        }

        private void mnuFile_DropDownOpening(object sender, EventArgs e)
        {
            if (_application.BassPlayer.PlayState == PlayState.Playing)
            {

                mnuFile.DropDownItems[0].Visible = false;
                mnuFile.DropDownItems[1].Visible = true;
            }
            else
            {
                mnuFile.DropDownItems[0].Visible = true;
                mnuFile.DropDownItems[1].Visible = false;
            }
        }

        private void mnuImportTracks_Click(object sender, EventArgs e)
        {
            if (_frmImportShufflerTracks == null || _frmImportShufflerTracks.IsDisposed)
            {
                _frmImportShufflerTracks = new FrmImportShufflerTracks {Library = _application.Library};
            }

            if (!_frmImportShufflerTracks.Visible)
                WindowHelper.ShowDialog(this, _frmImportShufflerTracks);
        }

        private void MnuResetMidi_Click(object sender, EventArgs e)
        {
            _application.ResetMidi();
        }
    }
}