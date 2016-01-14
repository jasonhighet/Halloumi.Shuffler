using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Collections;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.Controls;
using Halloumi.Shuffler.Engine;
using Halloumi.Shuffler.Forms.TrackPlayerExtensions;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Forms
{
    /// <summary>
    ///
    /// </summary>
    public partial class FrmMain : BaseMinimizeToTrayForm
    {
        #region Private Variables

        private MixLibrary MixLibrary { get; set; }

        private Library Library { get; set; }

        private SampleLibrary SampleLibrary { get; set; }

        private BE.BassPlayer BassPlayer { get; set; }

        private string SearchFilter { get; set; }

        private string PlaylistFilter { get; set; }

        private Library.ShufflerFilter ShufflerFilter { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the frmMain class.
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();

            mnuPlugins.Click += new EventHandler(mnuPlugins_Click);
            mnuAbout.Click += new EventHandler(mnuAbout_Click);
            mnuSettings.Click += new EventHandler(mnuSettings_Click);
            mnuExit.Click += new EventHandler(mnuExit_Click);
            mnuExportPlaylistTracks.Click += new EventHandler(mnuExportPlaylistTracks_Click);
            mnuConservativeFadeOut.Click += new EventHandler(mnuConservativeFadeOut_Click);

            mnuWinampDSPConfig.Click += new EventHandler(mnuWinampDSPConfig_Click);
            mnuVSTPluginConfig.Click += new EventHandler(mnuVSTPluginConfig_Click);
            mnuSamplerVSTPluginConfig.Click += new EventHandler(mnuSamplerVSTPluginConfig_Click);
            mnuTrackVSTPluginConfig.Click += new EventHandler(mnuTrackVSTPluginConfig_Click);
            mnuSamplerVSTPluginConfig2.Click += new EventHandler(mnuSamplerVSTPluginConfig2_Click);
            mnuTrackFXVSTPluginConfig.Click += new EventHandler(mnuTrackFXVSTPluginConfig_Click);
            mnuTrackFXVSTPluginConfig2.Click += new EventHandler(mnuTrackFXVSTPluginConfig2_Click);

            mnuViewLibrary.Click += new EventHandler(mnuViewLibrary_Click);
            mnuViewMixer.Click += new EventHandler(mnuViewMixer_Click);
            mnuViewPlaylist.Click += new EventHandler(mnuViewPlaylist_Click);

            notificationContextMenu.Opening += new CancelEventHandler(notificationContextMenu_Opening);
            mnuPlayPause.Click += new EventHandler(mnuPlayPause_Click);
            mnuPause.Click += new EventHandler(mnuPause_Click);
            mnuNext.Click += new EventHandler(mnuNext_Click);
            mnuPrevious.Click += new EventHandler(mnuPrevious_Click);
            mnuSkipToEnd.Click += new EventHandler(mnuSkipToEnd_Click);
            this.NotifyIcon.ContextMenuStrip = notificationContextMenu;

            this.PlaylistFilter = "";
            this.SearchFilter = "";
            this.ShufflerFilter = Library.ShufflerFilter.None;

            this.BassPlayer = new BE.BassPlayer(this.Handle);
            this.Library = new Library(this.BassPlayer);

            this.BassPlayer.OnTrackChange += new EventHandler(BassPlayer_OnTrackChange);
            this.BassPlayer.OnTrackQueued += new EventHandler(BassPlayer_OnTrackQueued);

            LoadSettings();

            this.MixLibrary = new MixLibrary(this.Library.ShufflerFolder);

            this.SampleLibrary = new SampleLibrary(this.BassPlayer, this.Library);

            playlistControl.Library = this.Library;
            playlistControl.MixLibrary = this.MixLibrary;
            playlistControl.BassPlayer = this.BassPlayer;
            playlistControl.ToolStripLabel = lblPlaylistStatus;

            playerDetails.Library = this.Library;
            playerDetails.BassPlayer = this.BassPlayer;
            playerDetails.PlaylistControl = this.playlistControl;
            playerDetails.SetSelectedView(PlayerDetails.SelectedView.Playlist);
            playerDetails.SelectedViewChanged += new EventHandler(playerDetails_SelectedViewChanged);

            playerDetails.MixLibrary = this.MixLibrary;
            playlistControl.MixLibrary = this.MixLibrary;

            trackLibraryControl.Library = this.Library;
            trackLibraryControl.BassPlayer = this.BassPlayer;
            trackLibraryControl.PlaylistControl = this.playlistControl;
            trackLibraryControl.MixLibrary = this.MixLibrary;
            trackLibraryControl.ToolStripLabel = lblLibraryStatus;
            trackLibraryControl.SamplerControl = this.mixerControl.SamplerControl;
            trackLibraryControl.SampleLibrary = this.SampleLibrary;

            mixerControl.Library = this.Library;
            mixerControl.BassPlayer = this.BassPlayer;
            mixerControl.PlaylistControl = this.playlistControl;

            this.Library.LoadFromCache();
            this.Library.LoadPlaylists();
            trackLibraryControl.Initalize();

            this.SampleLibrary.LoadFromCache();

            mixerControl.Initialize();
            playerDetails.Initialize();
            playlistControl.Initalize();

            shufflerController.PlaylistControl = this.playlistControl;
            shufflerController.LibraryControl = this.trackLibraryControl;
            shufflerController.BassPlayer = this.BassPlayer;
            shufflerController.Initalize();

            this.MixLibrary.AvailableTracks = this.Library.GetTracks();
            this.MixLibrary.LoadAllMixDetails();
            //var devices = BE.BassHelper.GetWaveOutDevices();

            SetView(PlayerDetails.SelectedView.Library);
        }

        private void playerDetails_SelectedViewChanged(object sender, EventArgs e)
        {
            SetView(playerDetails.GetSelectedView());
        }

        private void SetConservativeFadeOutSettings()
        {
            if (mnuConservativeFadeOut.Checked
                && this.BassPlayer.CurrentTrack != null && this.BassPlayer.NextTrack != null)
            {
                var track1 = this.Library.GetTrackByFilename(this.BassPlayer.CurrentTrack.Filename);
                var track2 = this.Library.GetTrackByFilename(this.BassPlayer.NextTrack.Filename);
                var mixRank = this.MixLibrary.GetMixLevel(track1, track2);
                var hasExtendedMix = this.MixLibrary.HasExtendedMix(track1, track2);

                if (mixRank <= 2 && !hasExtendedMix)
                    this.BassPlayer.SetConservativeFadeOutSettings();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Shows the plugin form.
        /// </summary>
        private void ShowPluginForm()
        {
            if (_pluginForm == null || _pluginForm.IsDisposed)
            {
                _pluginForm = new FrmPlugin(this.BassPlayer);
            }
            if (!_pluginForm.Visible)
            {
                WindowHelper.ShowDialog(this, _pluginForm);
            }
        }

        private FrmPlugin _pluginForm = null;

        /// <summary>
        /// Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            var settings = Settings.Default;
            this.Library.LibraryFolder = settings.LibraryFolder;
            this.Library.PlaylistFolder = settings.PlaylistFolder;
            this.Library.ShufflerFolder = settings.ShufflerFolder;
            this.BassPlayer.ExtendedAttributeFolder = settings.ShufflerFolder;
            this.BassPlayer.WaPluginsFolder = settings.WaPluginsFolder;
            this.BassPlayer.VstPluginsFolder = settings.VstPluginsFolder;
            formStateController.FormStateSettings = settings.FormStateSettings;
            this.BassPlayer.TrackFxAutomationEnabled = settings.EnableTrackFxAutomation;
            this.BassPlayer.SampleAutomationEnabled = settings.EnableSampleAutomation;
            BE.KeyHelper.SetApplicationFolder(settings.KeyFinderFolder);
        }

        /// <summary>
        /// Loads the UI settings.
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
                try { this.BassPlayer.LoadWaPlugin(settings.WaPlugin); }
                catch { }
            }

            if (settings.MainMixerVstPlugin != "")
            {
                try { this.BassPlayer.LoadMainVstPlugin(settings.MainMixerVstPlugin); }
                catch { }
            }

            if (settings.MainMixerVstPlugin2 != "")
            {
                try { this.BassPlayer.LoadMainVstPlugin2(settings.MainMixerVstPlugin2); }
                catch { }
            }

            if (settings.MainMixerVstPluginParameters != "" && this.BassPlayer.MainVstPlugin != null)
            {
                try { this.BassPlayer.SetVstPluginParameters(this.BassPlayer.MainVstPlugin, settings.MainMixerVstPluginParameters); }
                catch { }
            }

            if (settings.MainMixerVstPlugin2Parameters != "" && this.BassPlayer.MainVstPlugin2 != null)
            {
                try { this.BassPlayer.SetVstPluginParameters(this.BassPlayer.MainVstPlugin2, settings.MainMixerVstPlugin2Parameters); }
                catch { }
            }

            if (settings.SamplerVstPlugin != "")
            {
                try { this.BassPlayer.LoadSamplerVstPlugin(settings.SamplerVstPlugin); }
                catch { }
            }

            if (settings.SamplerVstPluginParameters != "" && this.BassPlayer.SamplerVstPlugin != null)
            {
                try { this.BassPlayer.SetVstPluginParameters(this.BassPlayer.SamplerVstPlugin, settings.SamplerVstPluginParameters); }
                catch { }
            }

            if (settings.SamplerVstPlugin2 != "")
            {
                try { this.BassPlayer.LoadSamplerVstPlugin2(settings.SamplerVstPlugin2); }
                catch { }
            }

            if (settings.SamplerVstPlugin2Parameters != "" && this.BassPlayer.SamplerVstPlugin2 != null)
            {
                try { this.BassPlayer.SetVstPluginParameters(this.BassPlayer.SamplerVstPlugin2, settings.SamplerVstPlugin2Parameters); }
                catch { }
            }

            if (settings.TrackVstPlugin != "")
            {
                try { this.BassPlayer.LoadTracksVstPlugin(settings.TrackVstPlugin); }
                catch { }
            }

            if (settings.TrackVstPluginParameters != "" && this.BassPlayer.TrackVstPlugin != null)
            {
                try { this.BassPlayer.SetVstPluginParameters(this.BassPlayer.TrackVstPlugin, settings.TrackVstPluginParameters); }
                catch { }
            }

            if (settings.TrackFxvstPlugin != "")
            {
                try { this.BassPlayer.LoadTrackSendFxvstPlugin(settings.TrackFxvstPlugin); }
                catch { }
            }

            if (settings.TrackFxvstPluginParameters != "" && this.BassPlayer.TrackSendFxvstPlugin != null)
            {
                try { this.BassPlayer.SetVstPluginParameters(this.BassPlayer.TrackSendFxvstPlugin, settings.TrackFxvstPluginParameters); }
                catch { }
            }

            if (settings.TrackFxvstPlugin2 != "")
            {
                try { this.BassPlayer.LoadTrackSendFxvstPlugin2(settings.TrackFxvstPlugin2); }
                catch { }
            }

            if (settings.TrackFxvstPlugin2Parameters != "" && this.BassPlayer.TrackSendFxvstPlugin2 != null)
            {
                try { this.BassPlayer.SetVstPluginParameters(this.BassPlayer.TrackSendFxvstPlugin2, settings.TrackFxvstPlugin2Parameters); }
                catch { }
            }

            this.MinimizeToTrayEnabled = (settings.MinimizeToTray);
            mnuMinimizeToTray.Checked = (settings.MinimizeToTray);

            mnuShowMixableTracks.Checked = (settings.ShowMixableTracks);
            playlistControl.ShowMixableTracks = mnuShowMixableTracks.Checked;
            trackLibraryControl.ShowMixableTracks = mnuShowMixableTracks.Checked;

            this.BassPlayer.LimitSongLength = settings.LimitSongLength;
            mnuConservativeFadeOut.Checked = settings.LimitSongLength;
            this.BassPlayer.LimitSongLength = mnuConservativeFadeOut.Checked;

            this.BassPlayer.SetMonitorVolume(settings.MonitorVolume);

            playerDetails.VisualsShown = settings.VisualsShown;
            mnuViewVisuals.Checked = settings.VisualsShown;

            playerDetails.AlbumArtShown = settings.AlbumArtShown;
            mnuViewAlbumArt.Checked = settings.AlbumArtShown;

            mnuSkipAfterMix.Checked = settings.SkipAfterMix;

            this.mixerControl.LoadSettings();

            this.trackLibraryControl.LoadUiSettings();

            //this.shufflerControl.CurrentShufflerMode = settings.ShufflerMode;

            mnuUpdateLibraryOnStartup.Checked = settings.UpdateLibraryOnStartup;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            var settings = Settings.Default;

            var winampPlugin = "";
            if (this.BassPlayer.WaPlugin != null) winampPlugin = this.BassPlayer.WaPlugin.Location;
            settings.WaPlugin = winampPlugin;

            var mainVstPlugin = "";
            if (this.BassPlayer.MainVstPlugin != null) mainVstPlugin = this.BassPlayer.MainVstPlugin.Location;
            settings.MainMixerVstPlugin = mainVstPlugin;

            var mainVstPlugin2 = "";
            if (this.BassPlayer.MainVstPlugin2 != null) mainVstPlugin2 = this.BassPlayer.MainVstPlugin2.Location;
            settings.MainMixerVstPlugin2 = mainVstPlugin2;

            var mainVstPluginParameters = "";
            if (this.BassPlayer.MainVstPlugin != null) mainVstPluginParameters = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.MainVstPlugin);
            settings.MainMixerVstPluginParameters = mainVstPluginParameters;

            var mainVstPluginParameters2 = "";
            if (this.BassPlayer.MainVstPlugin2 != null) mainVstPluginParameters2 = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.MainVstPlugin2);
            settings.MainMixerVstPlugin2Parameters = mainVstPluginParameters2;

            var samplerVstPlugin = "";
            if (this.BassPlayer.SamplerVstPlugin != null) samplerVstPlugin = this.BassPlayer.SamplerVstPlugin.Location;
            settings.SamplerVstPlugin = samplerVstPlugin;

            var samplerVstPluginParameters = "";
            if (this.BassPlayer.SamplerVstPlugin != null) samplerVstPluginParameters = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.SamplerVstPlugin);
            settings.SamplerVstPluginParameters = samplerVstPluginParameters;

            var samplerVstPlugin2 = "";
            if (this.BassPlayer.SamplerVstPlugin2 != null) samplerVstPlugin2 = this.BassPlayer.SamplerVstPlugin2.Location;
            settings.SamplerVstPlugin2 = samplerVstPlugin2;

            var samplerVstPluginParameters2 = "";
            if (this.BassPlayer.SamplerVstPlugin2 != null) samplerVstPluginParameters2 = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.SamplerVstPlugin2);
            settings.SamplerVstPlugin2Parameters = samplerVstPluginParameters2;

            var trackVstPlugin = "";
            if (this.BassPlayer.TrackVstPlugin != null) trackVstPlugin = this.BassPlayer.TrackVstPlugin.Location;
            settings.TrackVstPlugin = trackVstPlugin;

            var trackVstPluginParameters = "";
            if (this.BassPlayer.TrackVstPlugin != null) trackVstPluginParameters = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.TrackVstPlugin);
            settings.TrackVstPluginParameters = trackVstPluginParameters;

            var trackFxVstPlugin = "";
            if (this.BassPlayer.TrackSendFxvstPlugin != null) trackFxVstPlugin = this.BassPlayer.TrackSendFxvstPlugin.Location;
            settings.TrackFxvstPlugin = trackFxVstPlugin;

            var trackFxVstPluginParameters = "";
            if (this.BassPlayer.TrackSendFxvstPlugin != null) trackFxVstPluginParameters = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.TrackSendFxvstPlugin);
            settings.TrackFxvstPluginParameters = trackFxVstPluginParameters;

            var trackFxVstPlugin2 = "";
            if (this.BassPlayer.TrackSendFxvstPlugin2 != null) trackFxVstPlugin2 = this.BassPlayer.TrackSendFxvstPlugin2.Location;
            settings.TrackFxvstPlugin2 = trackFxVstPlugin2;

            var trackFxVstPluginParameters2 = "";
            if (this.BassPlayer.TrackSendFxvstPlugin2 != null) trackFxVstPluginParameters2 = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.TrackSendFxvstPlugin2);
            settings.TrackFxvstPlugin2Parameters = trackFxVstPluginParameters2;

            settings.SkipAfterMix = mnuSkipAfterMix.Checked;

            settings.MinimizeToTray = this.MinimizeToTrayEnabled;
            settings.LimitSongLength = this.BassPlayer.LimitSongLength;
            settings.FormStateSettings = formStateController.FormStateSettings;
            //settings.LeftRightSplit = splLeftRight.SplitterDistance;
            //settings.TrackSplit = splTrack.SplitterDistance;
            settings.Volume = this.BassPlayer.GetMixerVolume();

            //var effect1 = this.BassPlayer.SamplerVSTPlugin;
            //if (effect1 != null)
            //{
            //    settings.BypassSamplerEffect1 = this.BassPlayer.GetVSTPluginBypass(effect1);
            //}
            //else settings.BypassSamplerEffect1 = false;

            //var effect2 = this.BassPlayer.SamplerVSTPlugin2;
            //if (effect2 != null)
            //{
            //    settings.BypassSamplerEffect2 = this.BassPlayer.GetVSTPluginBypass(effect2);
            //}
            //else settings.BypassSamplerEffect2 = false;

            settings.SamplerDelayNotes = this.BassPlayer.SamplerDelayNotes;
            settings.SamplerVolume = Convert.ToInt32(this.BassPlayer.GetSamplerMixerVolume());

            //effect1 = this.BassPlayer.TrackSendFXVSTPlugin;
            //if (effect1 != null)
            //{
            //    settings.BypassTrackFXEffect1 = this.BassPlayer.GetVSTPluginBypass(effect1);
            //}
            //else settings.BypassTrackFXEffect1 = false;

            //effect2 = this.BassPlayer.TrackSendFXVSTPlugin2;
            //if (effect2 != null)
            //{
            //    settings.BypassTrackFXEffect2 = this.BassPlayer.GetVSTPluginBypass(effect2);
            //}
            //else settings.BypassTrackFXEffect2 = false;

            settings.TrackFxDelayNotes = this.BassPlayer.TrackSendFxDelayNotes;
            settings.TrackFxVolume = Convert.ToInt32(this.BassPlayer.GetTrackSendFxVolume());

            settings.SamplerOutput = this.BassPlayer.SamplerOutput;
            settings.TrackOutput = this.BassPlayer.TrackOutput;
            settings.MonitorVolume = Convert.ToInt32(this.BassPlayer.GetMonitorVolume());
            settings.RawLoopOutput = this.BassPlayer.RawLoopOutput;

            settings.EnableTrackFxAutomation = this.BassPlayer.TrackFxAutomationEnabled;
            settings.EnableSampleAutomation = this.BassPlayer.SampleAutomationEnabled;

            settings.VisualsShown = playerDetails.VisualsShown;
            settings.AlbumArtShown = playerDetails.AlbumArtShown;
            settings.ShowMixableTracks = mnuShowMixableTracks.Checked;

            settings.UpdateLibraryOnStartup = mnuUpdateLibraryOnStartup.Checked;

            settings.Save();

            this.trackLibraryControl.SaveSettings();
        }

        /// <summary>
        /// Sets the icon text.
        /// </summary>
        private void SetIconText()
        {
            if (this.BassPlayer.CurrentTrack != null)
            {
                var text = this.BassPlayer.CurrentTrack.Description.Replace("&", "&&");
                if (text.Length > 63) text = text.Substring(0, 63);
                this.NotifyIcon.Text = text;
            }
            else this.NotifyIcon.Text = "";
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the FormClosed event of the frmMain control.
        /// </summary>
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            mixerControl.Unload();
            this.Library.SaveCache();
            this.BassPlayer.Dispose();
        }

        /// <summary>
        /// Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange(object sender, EventArgs e)
        {
            //if (!this.IsHandleCreated) return;
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    BassPlayer_OnTrackChange();
                }));
            }
            else BassPlayer_OnTrackChange();
        }

        /// <summary>
        /// Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange()
        {
            if (_bassPlayerOnTrackChange) return;
            _bassPlayerOnTrackChange = true;

            SetIconText();

            _bassPlayerOnTrackChange = false;
        }

        private bool _bassPlayerOnTrackChange = false;

        /// <summary>
        /// Handles the Click event of the mnuVSTPluginConfig control.
        /// </summary>
        private void mnuVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.MainVstPlugin == null)
                ShowPluginForm();
            else
                this.BassPlayer.ShowVstPluginConfig(this.BassPlayer.MainVstPlugin);
        }

        /// <summary>
        /// Handles the Click event of the mnuWinampDSPConfig control.
        /// </summary>
        private void mnuWinampDSPConfig_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.WaPlugin == null)
                ShowPluginForm();
            else
                this.BassPlayer.ShowWaPluginConfig(this.BassPlayer.WaPlugin);
        }

        /// <summary>
        /// Handles the Click event of the mnuSamplerVSTPluginConfig control.
        /// </summary>
        private void mnuSamplerVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.SamplerVstPlugin == null)
                ShowPluginForm();
            else
                this.BassPlayer.ShowVstPluginConfig(this.BassPlayer.SamplerVstPlugin);
        }

        /// <summary>
        /// Handles the Click event of the mnuTrackVSTPluginConfig control.
        /// </summary>
        private void mnuTrackVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.TrackVstPlugin == null)
                ShowPluginForm();
            else
                this.BassPlayer.ShowVstPluginConfig(this.BassPlayer.TrackVstPlugin);
        }

        /// <summary>
        /// Handles the Click event of the mnuTrackFXVSTPluginConfig control.
        /// </summary>
        private void mnuTrackFXVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.TrackSendFxvstPlugin == null)
                ShowPluginForm();
            else
                this.BassPlayer.ShowVstPluginConfig(this.BassPlayer.TrackSendFxvstPlugin);
        }

        /// <summary>
        /// Handles the Click event of the mnuTrackFXVSTPluginConfig control.
        /// </summary>
        private void mnuTrackFXVSTPluginConfig2_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.TrackSendFxvstPlugin2 == null)
                ShowPluginForm();
            else
                this.BassPlayer.ShowVstPluginConfig(this.BassPlayer.TrackSendFxvstPlugin2);
        }

        /// <summary>
        /// Handles the Click event of the mnuSamplerVSTPluginConfig2 control.
        /// </summary>
        private void mnuSamplerVSTPluginConfig2_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.SamplerVstPlugin2 == null)
                ShowPluginForm();
            else
                this.BassPlayer.ShowVstPluginConfig(this.BassPlayer.SamplerVstPlugin2);
        }

        /// <summary>
        /// Handles the Click event of the mnuPlugins control.
        /// </summary>
        private void mnuPlugins_Click(object sender, EventArgs e)
        {
            ShowPluginForm();
        }

        /// <summary>
        /// Handles the Click event of the mnuViewMixer control.
        /// </summary>
        private void mnuViewMixer_Click(object sender, EventArgs e)
        {
            playerDetails.SetSelectedView(PlayerDetails.SelectedView.Mixer);
        }

        /// <summary>
        /// Handles the Click event of the mnuViewLibrary control.
        /// </summary>
        private void mnuViewLibrary_Click(object sender, EventArgs e)
        {
            playerDetails.SetSelectedView(PlayerDetails.SelectedView.Library);
        }

        /// <summary>
        /// Handles the Click event of the mnuViewPlaylist control.
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
        /// Handles the Click event of the mnuLimitSongLength control.
        /// </summary>
        private void mnuConservativeFadeOut_Click(object sender, EventArgs e)
        {
            mnuConservativeFadeOut.Checked = !mnuConservativeFadeOut.Checked;
            this.BassPlayer.LimitSongLength = mnuConservativeFadeOut.Checked;
            SetConservativeFadeOutSettings();
        }

        /// <summary>
        /// Handles the OnTrackQueued event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackQueued(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    SetConservativeFadeOutSettings();
                }));
            }
            else
            {
                SetConservativeFadeOutSettings();
            }
        }

        /// <summary>
        /// Handles the FormClosing event of the frmMain control.
        /// </summary>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        /// Handles the Click event of the mnuSettings control.
        /// </summary>
        public void mnuSettings_Click(object sender, EventArgs e)
        {
            var settings = new FrmSettings();
            settings.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the mnuAbout control.
        /// </summary>
        private void mnuAbout_Click(object sender, EventArgs e)
        {
            aboutDialog.Show();
        }

        /// <summary>
        /// Handles the Click event of the mnuExit control.
        /// </summary>
        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.NotifyIcon.Visible = false;
        }

        /// <summary>
        /// Handles the Load event of the frmMain control.
        /// </summary>
        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadUiSettings();

            if (mnuUpdateLibraryOnStartup.Checked) this.trackLibraryControl.ImportLibrary();
        }

        /// <summary>
        /// Handles the Resize event of the frmMain control.
        /// </summary>
        private void frmMain_Resize(object sender, EventArgs e)
        { }

        /// <summary>
        /// Handles the Click event of the mnuMinimizeToTray control.
        /// </summary>
        private void mnuMinimizeToTray_Click(object sender, EventArgs e)
        {
            this.MinimizeToTrayEnabled = !this.MinimizeToTrayEnabled;
            mnuMinimizeToTray.Checked = this.MinimizeToTrayEnabled;
            SaveSettings();
        }

        /// <summary>
        /// Handles the Click event of the mnuMinimizeToTray control.
        /// </summary>
        private void mnuShowMixableTracks_Click(object sender, EventArgs e)
        {
            mnuShowMixableTracks.Checked = !mnuShowMixableTracks.Checked;
            trackLibraryControl.ShowMixableTracks = mnuShowMixableTracks.Checked;
            playlistControl.ShowMixableTracks = mnuShowMixableTracks.Checked;

            SaveSettings();
        }

        /// <summary>
        /// Handles the Click event of the mnuRank control.
        /// </summary>
        private void mnuRank_Click(object sender, EventArgs e)
        {
            var mixRankDescription = (sender as ToolStripDropDownItem).Text;
            var mixRank = this.MixLibrary.GetRankFromDescription(mixRankDescription);
            playerDetails.SetCurrentMixRank(mixRank);
            playerDetails.DisplayCurrentTrackDetails();

            if (mnuSkipAfterMix.Checked) this.BassPlayer.SkipToEnd();
        }

        /// <summary>
        /// Handles the Click event of the mnuSkipToEnd control.
        /// </summary>
        private void mnuSkipToEnd_Click(object sender, EventArgs e)
        {
            this.BassPlayer.SkipToEnd();
        }

        /// <summary>
        /// Handles the Click event of the mnuPrevious control.
        /// </summary>
        private void mnuPrevious_Click(object sender, EventArgs e)
        {
            playlistControl.PlayPreviousTrack();
        }

        /// <summary>
        /// Handles the Click event of the mnuNext control.
        /// </summary>
        private void mnuNext_Click(object sender, EventArgs e)
        {
            playlistControl.PlayNextTrack();
        }

        /// <summary>
        /// Handles the Click event of the mnuPlayPause control.
        /// </summary>
        private void mnuPlayPause_Click(object sender, EventArgs e)
        {
            this.BassPlayer.Play();
        }

        /// <summary>
        /// Handles the Click event of the mnuPlayPause control.
        /// </summary>
        private void mnuPause_Click(object sender, EventArgs e)
        {
            this.BassPlayer.Pause();
        }

        /// <summary>
        /// Handles the Click event of the mnuExportPlaylist control.
        /// </summary>
        private void mnuExportPlaylistTracks_Click(object sender, EventArgs e)
        {
            var tracks = this.playlistControl.GetTracks();

            var playlistName = (!string.IsNullOrEmpty(this.playlistControl.CurrentPlaylistFile))
                ? Path.GetFileNameWithoutExtension(this.playlistControl.CurrentPlaylistFile)
                : this.trackLibraryControl.PlaylistFilter;

            ExportTracks(tracks, playlistName);
        }

        private void ExportTracks(List<Track> tracks, string playlistName)
        {
            var exportPlaylist = new FrmExportPlaylist();
            exportPlaylist.Library = this.Library;
            exportPlaylist.Tracks = tracks;
            exportPlaylist.PlaylistName = playlistName;
            exportPlaylist.ShowDialog();
        }

        /// <summary>
        /// Handles the Opening event of the notificationContextMenu control.
        /// </summary>
        private void notificationContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (this.BassPlayer.PlayState == BE.PlayState.Playing)
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
        /// Binds the mix rank menu.
        /// </summary>
        private void BindMixRankMenu()
        {
            var currentMixRank = playerDetails.GetCurrentMixRank();
            for (var i = 0; i < 6; i++)
            {
                mnuRank.DropDownItems[i].Text = this.MixLibrary.GetRankDescription(5 - i);
                ((ToolStripMenuItem)mnuRank.DropDownItems[i]).Checked = ((5 - i) == currentMixRank);
            }
        }

        /// <summary>
        /// Binds the track rank menu.
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
                mnuTrackRank.DropDownItems[i].Text = this.MixLibrary.GetRankDescription(5 - i);
                ((ToolStripMenuItem)mnuTrackRank.DropDownItems[i]).Checked = ((5 - i) == currentMixRank);
            }
        }

        private void mnuGeneratePlaylist_Click(object sender, EventArgs e)
        {
            if (_generatePlaylist == null || _generatePlaylist.IsDisposed)
            {
                _generatePlaylist = new FrmGeneratePlaylist
                {
                    LibraryControl = this.trackLibraryControl,
                    PlaylistControl = this.playlistControl
                };
                _generatePlaylist.SetScreenMode(FrmGeneratePlaylist.ScreenMode.GeneratePlaylist);
            }
            if (!_generatePlaylist.Visible)
            {
                WindowHelper.ShowDialog(this, _generatePlaylist);
            }
        }

        private FrmGeneratePlaylist _generatePlaylist = null;
        private FrmGeneratePlaylist _autoGenerateSettings = null;
        private FrmSampleLibrary _frmSampleLibrary = null;
        private FrmFadeNow _frmFadeNow = null;

        private void mnuMonitorSettings_Click(object sender, EventArgs e)
        {
            if (_monitorSettings == null || _monitorSettings.IsDisposed)
            {
                _monitorSettings = new FrmMonitorSettings();
                _monitorSettings.BassPlayer = this.BassPlayer;
            }
            if (!_monitorSettings.Visible)
            {
                WindowHelper.ShowDialog(this, _monitorSettings);
            }
        }

        private FrmMonitorSettings _monitorSettings = null;

        #endregion

        /// <summary>
        /// Handles the LoadDocument event of the fileMenuController control.
        /// </summary>
        private void fileMenuController_LoadDocument(object sender, Halloumi.Common.Windows.Controllers.FileMenuControllerEventArgs e)
        {
            playlistControl.OpenPlaylist(e.FileName);
        }

        /// <summary>
        /// Handles the SaveDocument event of the fileMenuController control.
        /// </summary>
        private void fileMenuController_SaveDocument(object sender, Halloumi.Common.Windows.Controllers.FileMenuControllerEventArgs e)
        {
            playlistControl.SavePlaylist(e.FileName);
        }

        private void mnuTrackRank_Click(object sender, EventArgs e)
        {
            var mixRankDescription = (sender as ToolStripDropDownItem).Text;
            var mixRank = this.MixLibrary.GetRankFromDescription(mixRankDescription);

            var track = playlistControl.GetCurrentTrack();
            track.Rank = mixRank;
            this.Library.SaveRank(track);
        }

        private void mnuViewVisuals_Click(object sender, EventArgs e)
        {
            mnuViewVisuals.Checked = !mnuViewVisuals.Checked;
            this.playerDetails.VisualsShown = mnuViewVisuals.Checked;
        }

        private void mnuLibrary_DropDownOpening(object sender, EventArgs e)
        {
            mnuUpdateLibrary.Enabled = !this.trackLibraryControl.IsLibraryUpdating();
            mnuCancelLibraryUpdate.Enabled = this.trackLibraryControl.IsLibraryUpdating();
        }

        private void mnuUpdateLibrary_Click(object sender, EventArgs e)
        {
            this.trackLibraryControl.ImportLibrary();
        }

        private void mnuCancelLibraryUpdate_Click(object sender, EventArgs e)
        {
            this.trackLibraryControl.CancelLibraryImport();
        }

        private void mnuUpdateLibraryOnStartup_Click(object sender, EventArgs e)
        {
            mnuUpdateLibraryOnStartup.Checked = !mnuUpdateLibraryOnStartup.Checked;
        }

        private void mnuReplayMix_Click(object sender, EventArgs e)
        {
            this.playlistControl.ReplayMix();
        }

        private void mnuViewAlbumArt_Click(object sender, EventArgs e)
        {
            mnuViewAlbumArt.Checked = !mnuViewAlbumArt.Checked;
            this.playerDetails.AlbumArtShown = mnuViewAlbumArt.Checked;
        }

        private void mnuCleanLibrary_Click(object sender, EventArgs e)
        {
            this.Library.CleanLibrary();
        }

        private void mnuSyncShufflerFiles_Click(object sender, EventArgs e)
        {
            var exportShufflerFiles = new FrmImportShufflerFiles();
            exportShufflerFiles.Library = this.Library;
            exportShufflerFiles.MixLibrary = this.MixLibrary;
            exportShufflerFiles.ShowDialog();
        }

        private void mnuUpdateDuplicateTracks_Click(object sender, EventArgs e)
        {
            var similarTracks = new FrmUpdateSimilarTracks();
            similarTracks.BassPlayer = this.BassPlayer;
            similarTracks.Library = this.Library;
            similarTracks.Tracks = this.trackLibraryControl.GetDisplayedTracks().ToList();
            similarTracks.ShowDialog();
        }

        private void mnuAutoGenerateSettings_Click(object sender, EventArgs e)
        {
            if (_autoGenerateSettings == null || _autoGenerateSettings.IsDisposed)
            {
                _autoGenerateSettings = new FrmGeneratePlaylist
                {
                    LibraryControl = this.trackLibraryControl,
                    PlaylistControl = this.playlistControl
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
            this.shufflerController.AutoGenerateEnabled = mnuAutoshuffle.Checked;
        }

        private void mnuAutoGenerateNow_Click(object sender, EventArgs e)
        {
            this.shufflerController.AutoGenerateNow();
        }

        private void mnuExportLibraryTracks_Click(object sender, EventArgs e)
        {
            var tracks = this.trackLibraryControl.GetDisplayedTracks();
            var playlistName = "";
            ExportTracks(tracks, playlistName);
        }

        private void mnuSampleLibrary_Click(object sender, EventArgs e)
        {
            if (_frmSampleLibrary == null || _frmSampleLibrary.IsDisposed)
            {
                _frmSampleLibrary = new FrmSampleLibrary();
                _frmSampleLibrary.Initialize(this.BassPlayer, this.SampleLibrary);
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
                    BassPlayer = this.BassPlayer,
                    Library = this.Library,
                    PlaylistControl = this.playlistControl,
                };
            }
            if (!_frmFadeNow.Visible)
            {
                WindowHelper.ShowDialog(this, _frmFadeNow);
            }
        }
    }
}