namespace Halloumi.Shuffler.Forms
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.kryptonManager = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.pnlTop = new System.Windows.Forms.Panel();
            this.playerDetails = new Halloumi.Shuffler.Controls.PlayerDetails();
            this.pnlMenuBar = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.linMenuBorder = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBestMix = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAutoshuffle = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAutoshuffleSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAutoGenerateNow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExportPlaylistTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLibrary = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdateLibrary = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancelLibraryUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCleanLibrary = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuUpdateLibraryOnStartup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuUpdateDuplicateTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExportLibraryTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExportShufflerTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuImportCollection = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteCollection = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuImportTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewLibrary = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewMixer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuShowPlayer = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowTrackDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowMixableTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewVisuals = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewAlbumArt = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuMinimizeToTray = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTracker = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSampleLibrary = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuModuleEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMonitorSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuPlugins = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWinampDSPConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVSTPluginConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrackVSTPluginConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrackFXVSTPluginConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrackFXVSTPluginConfig2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSamplerVSTPluginConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSamplerVSTPluginConfig2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuConservativeFadeOut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.formStateController = new Halloumi.Common.Windows.Contollers.FormStateController(this.components);
            this.notificationContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuPlayPause = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPause = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNext = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPrevious = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSkipToEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReplayMix = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuTrackRank = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRank = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRank0 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRank1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRank2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRank3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRank4 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRank5 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSkipAfterMix = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit2 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblPlayerStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLibraryStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPlaylistStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.fileMenuController = new Halloumi.Common.Windows.Controllers.FileMenuController(this.components);
            this.aboutDialog = new Halloumi.Common.Windows.Controls.AboutDialog(this.components);
            this.pnlMain = new Halloumi.Common.Windows.Controls.Panel();
            this.trackLibraryControl = new Halloumi.Shuffler.Controls.TrackLibraryControl();
            this.playlistControl = new Halloumi.Shuffler.Controls.PlaylistControl();
            this.mixerControl = new Halloumi.Shuffler.Controls.MixerControl();
            this.shufflerController = new Halloumi.Shuffler.Controls.ShufflerController(this.components);
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuResetMidi = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMenuBar)).BeginInit();
            this.pnlMenuBar.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.notificationContextMenu.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonManager
            // 
            this.kryptonManager.GlobalAllowFormChrome = false;
            this.kryptonManager.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2010Silver;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.playerDetails);
            this.pnlTop.Controls.Add(this.pnlMenuBar);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1253, 149);
            this.pnlTop.TabIndex = 30;
            // 
            // playerDetails
            // 
            this.playerDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.playerDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playerDetails.Location = new System.Drawing.Point(0, 37);
            this.playerDetails.Margin = new System.Windows.Forms.Padding(5);
            this.playerDetails.Name = "playerDetails";
            this.playerDetails.Size = new System.Drawing.Size(1253, 112);
            this.playerDetails.TabIndex = 34;
            // 
            // pnlMenuBar
            // 
            this.pnlMenuBar.Controls.Add(this.linMenuBorder);
            this.pnlMenuBar.Controls.Add(this.menuStrip);
            this.pnlMenuBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMenuBar.Location = new System.Drawing.Point(0, 0);
            this.pnlMenuBar.Margin = new System.Windows.Forms.Padding(4);
            this.pnlMenuBar.Name = "pnlMenuBar";
            this.pnlMenuBar.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderRowList;
            this.pnlMenuBar.Size = new System.Drawing.Size(1253, 37);
            this.pnlMenuBar.TabIndex = 29;
            // 
            // linMenuBorder
            // 
            this.linMenuBorder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.linMenuBorder.Location = new System.Drawing.Point(0, 36);
            this.linMenuBorder.Margin = new System.Windows.Forms.Padding(4);
            this.linMenuBorder.Name = "linMenuBorder";
            this.linMenuBorder.Size = new System.Drawing.Size(1253, 1);
            this.linMenuBorder.Text = "kryptonBorderEdge4";
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuLibrary,
            this.mnuView,
            this.mnuTracker,
            this.mnuOptions,
            this.mnuHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1253, 37);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBestMix,
            this.toolStripSeparator9,
            this.mnuAutoshuffle,
            this.mnuAutoshuffleSettings,
            this.mnuAutoGenerateNow,
            this.toolStripSeparator11,
            this.mnuExportPlaylistTracks,
            this.toolStripSeparator13,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(69, 33);
            this.mnuFile.Text = "&Playlist";
            this.mnuFile.DropDownOpening += new System.EventHandler(this.mnuFile_DropDownOpening);
            // 
            // mnuBestMix
            // 
            this.mnuBestMix.Name = "mnuBestMix";
            this.mnuBestMix.Size = new System.Drawing.Size(256, 26);
            this.mnuBestMix.Text = "&Generate Playlist...";
            this.mnuBestMix.Click += new System.EventHandler(this.mnuGeneratePlaylist_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(253, 6);
            // 
            // mnuAutoshuffle
            // 
            this.mnuAutoshuffle.CheckOnClick = true;
            this.mnuAutoshuffle.Name = "mnuAutoshuffle";
            this.mnuAutoshuffle.Size = new System.Drawing.Size(256, 26);
            this.mnuAutoshuffle.Text = "&Auto-Generate Enabled";
            this.mnuAutoshuffle.Click += new System.EventHandler(this.mnuAutoshuffle_Click);
            // 
            // mnuAutoshuffleSettings
            // 
            this.mnuAutoshuffleSettings.Name = "mnuAutoshuffleSettings";
            this.mnuAutoshuffleSettings.Size = new System.Drawing.Size(256, 26);
            this.mnuAutoshuffleSettings.Text = "&Auto-Generate &Settings...";
            this.mnuAutoshuffleSettings.Click += new System.EventHandler(this.mnuAutoGenerateSettings_Click);
            // 
            // mnuAutoGenerateNow
            // 
            this.mnuAutoGenerateNow.Name = "mnuAutoGenerateNow";
            this.mnuAutoGenerateNow.Size = new System.Drawing.Size(256, 26);
            this.mnuAutoGenerateNow.Text = "Auto-Generate &Now";
            this.mnuAutoGenerateNow.Click += new System.EventHandler(this.mnuAutoGenerateNow_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(253, 6);
            // 
            // mnuExportPlaylistTracks
            // 
            this.mnuExportPlaylistTracks.Name = "mnuExportPlaylistTracks";
            this.mnuExportPlaylistTracks.Size = new System.Drawing.Size(256, 26);
            this.mnuExportPlaylistTracks.Text = "&Export Tracks...";
            this.mnuExportPlaylistTracks.Click += new System.EventHandler(this.mnuExportPlaylistTracks_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(253, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Image = ((System.Drawing.Image)(resources.GetObject("mnuExit.Image")));
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.mnuExit.Size = new System.Drawing.Size(256, 26);
            this.mnuExit.Text = "E&xit";
            // 
            // mnuLibrary
            // 
            this.mnuLibrary.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuUpdateLibrary,
            this.mnuCancelLibraryUpdate,
            this.mnuCleanLibrary,
            this.toolStripSeparator8,
            this.mnuUpdateLibraryOnStartup,
            this.toolStripSeparator1,
            this.mnuUpdateDuplicateTracks,
            this.toolStripSeparator10,
            this.mnuExportLibraryTracks,
            this.mnuExportShufflerTracks,
            this.toolStripSeparator14,
            this.mnuImportCollection,
            this.mnuDeleteCollection,
            this.toolStripSeparator7,
            this.mnuImportTracks});
            this.mnuLibrary.Name = "mnuLibrary";
            this.mnuLibrary.Size = new System.Drawing.Size(68, 33);
            this.mnuLibrary.Text = "&Library";
            this.mnuLibrary.DropDownOpening += new System.EventHandler(this.mnuLibrary_DropDownOpening);
            // 
            // mnuUpdateLibrary
            // 
            this.mnuUpdateLibrary.Name = "mnuUpdateLibrary";
            this.mnuUpdateLibrary.Size = new System.Drawing.Size(271, 26);
            this.mnuUpdateLibrary.Text = "&Update Library";
            this.mnuUpdateLibrary.Click += new System.EventHandler(this.mnuUpdateLibrary_Click);
            // 
            // mnuCancelLibraryUpdate
            // 
            this.mnuCancelLibraryUpdate.Name = "mnuCancelLibraryUpdate";
            this.mnuCancelLibraryUpdate.Size = new System.Drawing.Size(271, 26);
            this.mnuCancelLibraryUpdate.Text = "&Cancel Update";
            this.mnuCancelLibraryUpdate.Click += new System.EventHandler(this.mnuCancelLibraryUpdate_Click);
            // 
            // mnuCleanLibrary
            // 
            this.mnuCleanLibrary.Name = "mnuCleanLibrary";
            this.mnuCleanLibrary.Size = new System.Drawing.Size(271, 26);
            this.mnuCleanLibrary.Text = "Clean Library";
            this.mnuCleanLibrary.Click += new System.EventHandler(this.mnuCleanLibrary_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(268, 6);
            // 
            // mnuUpdateLibraryOnStartup
            // 
            this.mnuUpdateLibraryOnStartup.Name = "mnuUpdateLibraryOnStartup";
            this.mnuUpdateLibraryOnStartup.Size = new System.Drawing.Size(271, 26);
            this.mnuUpdateLibraryOnStartup.Text = "Update Library On &Start Up";
            this.mnuUpdateLibraryOnStartup.Click += new System.EventHandler(this.mnuUpdateLibraryOnStartup_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(268, 6);
            // 
            // mnuUpdateDuplicateTracks
            // 
            this.mnuUpdateDuplicateTracks.Name = "mnuUpdateDuplicateTracks";
            this.mnuUpdateDuplicateTracks.Size = new System.Drawing.Size(271, 26);
            this.mnuUpdateDuplicateTracks.Text = "Find &Duplicate Tracks...";
            this.mnuUpdateDuplicateTracks.Click += new System.EventHandler(this.mnuUpdateDuplicateTracks_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(268, 6);
            // 
            // mnuExportLibraryTracks
            // 
            this.mnuExportLibraryTracks.Name = "mnuExportLibraryTracks";
            this.mnuExportLibraryTracks.Size = new System.Drawing.Size(271, 26);
            this.mnuExportLibraryTracks.Text = "Export Displayed Tracks...";
            this.mnuExportLibraryTracks.Click += new System.EventHandler(this.mnuExportLibraryTracks_Click);
            // 
            // mnuExportShufflerTracks
            // 
            this.mnuExportShufflerTracks.Name = "mnuExportShufflerTracks";
            this.mnuExportShufflerTracks.Size = new System.Drawing.Size(271, 26);
            this.mnuExportShufflerTracks.Text = "Export Shuffler Tracks...";
            this.mnuExportShufflerTracks.Click += new System.EventHandler(this.mnuExportShufflerTracks_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(268, 6);
            // 
            // mnuImportCollection
            // 
            this.mnuImportCollection.Name = "mnuImportCollection";
            this.mnuImportCollection.Size = new System.Drawing.Size(271, 26);
            this.mnuImportCollection.Text = "Import Collection...";
            this.mnuImportCollection.Click += new System.EventHandler(this.mnuImportCollection_Click);
            // 
            // mnuDeleteCollection
            // 
            this.mnuDeleteCollection.Name = "mnuDeleteCollection";
            this.mnuDeleteCollection.Size = new System.Drawing.Size(271, 26);
            this.mnuDeleteCollection.Text = "Delete Collection";
            this.mnuDeleteCollection.Click += new System.EventHandler(this.mnuDeleteCollection_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(268, 6);
            // 
            // mnuImportTracks
            // 
            this.mnuImportTracks.Name = "mnuImportTracks";
            this.mnuImportTracks.Size = new System.Drawing.Size(271, 26);
            this.mnuImportTracks.Text = "Import Tracks...";
            this.mnuImportTracks.Click += new System.EventHandler(this.mnuImportTracks_Click);
            // 
            // mnuView
            // 
            this.mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewPlaylist,
            this.mnuViewLibrary,
            this.mnuViewMixer,
            this.toolStripSeparator12,
            this.mnuShowPlayer,
            this.mnuShowTrackDetails,
            this.mnuShowMixableTracks,
            this.mnuViewVisuals,
            this.mnuViewAlbumArt,
            this.toolStripSeparator6,
            this.mnuMinimizeToTray});
            this.mnuView.Name = "mnuView";
            this.mnuView.Size = new System.Drawing.Size(55, 33);
            this.mnuView.Text = "&View";
            // 
            // mnuViewPlaylist
            // 
            this.mnuViewPlaylist.Checked = true;
            this.mnuViewPlaylist.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuViewPlaylist.Name = "mnuViewPlaylist";
            this.mnuViewPlaylist.Size = new System.Drawing.Size(204, 26);
            this.mnuViewPlaylist.Text = "&Playlist";
            // 
            // mnuViewLibrary
            // 
            this.mnuViewLibrary.Name = "mnuViewLibrary";
            this.mnuViewLibrary.Size = new System.Drawing.Size(204, 26);
            this.mnuViewLibrary.Text = "Library";
            // 
            // mnuViewMixer
            // 
            this.mnuViewMixer.Name = "mnuViewMixer";
            this.mnuViewMixer.Size = new System.Drawing.Size(204, 26);
            this.mnuViewMixer.Text = "Mixer";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(201, 6);
            // 
            // mnuShowPlayer
            // 
            this.mnuShowPlayer.Name = "mnuShowPlayer";
            this.mnuShowPlayer.Size = new System.Drawing.Size(204, 26);
            this.mnuShowPlayer.Text = "Player Details";
            this.mnuShowPlayer.Click += new System.EventHandler(this.mnuShowPlayer_Click);
            // 
            // mnuShowTrackDetails
            // 
            this.mnuShowTrackDetails.Name = "mnuShowTrackDetails";
            this.mnuShowTrackDetails.Size = new System.Drawing.Size(204, 26);
            this.mnuShowTrackDetails.Text = "Track &Details";
            this.mnuShowTrackDetails.Click += new System.EventHandler(this.mnuShowTrackDetails_Click);
            // 
            // mnuShowMixableTracks
            // 
            this.mnuShowMixableTracks.Name = "mnuShowMixableTracks";
            this.mnuShowMixableTracks.Size = new System.Drawing.Size(204, 26);
            this.mnuShowMixableTracks.Text = "&Mixable Tracks";
            this.mnuShowMixableTracks.Click += new System.EventHandler(this.mnuShowMixableTracks_Click);
            // 
            // mnuViewVisuals
            // 
            this.mnuViewVisuals.Name = "mnuViewVisuals";
            this.mnuViewVisuals.Size = new System.Drawing.Size(204, 26);
            this.mnuViewVisuals.Text = "&Visuals";
            this.mnuViewVisuals.Click += new System.EventHandler(this.mnuViewVisuals_Click);
            // 
            // mnuViewAlbumArt
            // 
            this.mnuViewAlbumArt.Name = "mnuViewAlbumArt";
            this.mnuViewAlbumArt.Size = new System.Drawing.Size(204, 26);
            this.mnuViewAlbumArt.Text = "&Album Art";
            this.mnuViewAlbumArt.Click += new System.EventHandler(this.mnuViewAlbumArt_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(201, 6);
            // 
            // mnuMinimizeToTray
            // 
            this.mnuMinimizeToTray.Name = "mnuMinimizeToTray";
            this.mnuMinimizeToTray.Size = new System.Drawing.Size(204, 26);
            this.mnuMinimizeToTray.Text = "Minimi&ze To Tray";
            // 
            // mnuTracker
            // 
            this.mnuTracker.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSampleLibrary,
            this.mnuModuleEditor});
            this.mnuTracker.Name = "mnuTracker";
            this.mnuTracker.Size = new System.Drawing.Size(78, 33);
            this.mnuTracker.Text = "&Sampler";
            // 
            // mnuSampleLibrary
            // 
            this.mnuSampleLibrary.Name = "mnuSampleLibrary";
            this.mnuSampleLibrary.Size = new System.Drawing.Size(191, 26);
            this.mnuSampleLibrary.Text = "Sample Library";
            this.mnuSampleLibrary.Click += new System.EventHandler(this.mnuSampleLibrary_Click);
            // 
            // mnuModuleEditor
            // 
            this.mnuModuleEditor.Name = "mnuModuleEditor";
            this.mnuModuleEditor.Size = new System.Drawing.Size(191, 26);
            this.mnuModuleEditor.Text = "Module Editor";
            this.mnuModuleEditor.Click += new System.EventHandler(this.mnuModuleEditor_Click);
            // 
            // mnuOptions
            // 
            this.mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSettings,
            this.mnuMonitorSettings,
            this.toolStripSeparator4,
            this.mnuPlugins,
            this.mnuWinampDSPConfig,
            this.mnuVSTPluginConfig,
            this.mnuTrackVSTPluginConfig,
            this.mnuTrackFXVSTPluginConfig,
            this.mnuTrackFXVSTPluginConfig2,
            this.mnuSamplerVSTPluginConfig,
            this.mnuSamplerVSTPluginConfig2,
            this.toolStripSeparator5,
            this.mnuConservativeFadeOut,
            this.toolStripSeparator15,
            this.mnuResetMidi});
            this.mnuOptions.Name = "mnuOptions";
            this.mnuOptions.Size = new System.Drawing.Size(75, 33);
            this.mnuOptions.Text = "&Options";
            // 
            // mnuSettings
            // 
            this.mnuSettings.Name = "mnuSettings";
            this.mnuSettings.Size = new System.Drawing.Size(310, 26);
            this.mnuSettings.Text = "&Settings...";
            // 
            // mnuMonitorSettings
            // 
            this.mnuMonitorSettings.Name = "mnuMonitorSettings";
            this.mnuMonitorSettings.Size = new System.Drawing.Size(310, 26);
            this.mnuMonitorSettings.Text = "&Monitor Settings";
            this.mnuMonitorSettings.Click += new System.EventHandler(this.mnuMonitorSettings_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(307, 6);
            // 
            // mnuPlugins
            // 
            this.mnuPlugins.Name = "mnuPlugins";
            this.mnuPlugins.Size = new System.Drawing.Size(310, 26);
            this.mnuPlugins.Text = "&Effects Settings...";
            // 
            // mnuWinampDSPConfig
            // 
            this.mnuWinampDSPConfig.Name = "mnuWinampDSPConfig";
            this.mnuWinampDSPConfig.Size = new System.Drawing.Size(310, 26);
            this.mnuWinampDSPConfig.Text = "Main Mixer &DSP Effect Settings...";
            // 
            // mnuVSTPluginConfig
            // 
            this.mnuVSTPluginConfig.Name = "mnuVSTPluginConfig";
            this.mnuVSTPluginConfig.Size = new System.Drawing.Size(310, 26);
            this.mnuVSTPluginConfig.Text = "Main Mixer &VST Effect Settings...";
            // 
            // mnuTrackVSTPluginConfig
            // 
            this.mnuTrackVSTPluginConfig.Name = "mnuTrackVSTPluginConfig";
            this.mnuTrackVSTPluginConfig.Size = new System.Drawing.Size(310, 26);
            this.mnuTrackVSTPluginConfig.Text = "&Track Mixer VST Effect Settings...";
            // 
            // mnuTrackFXVSTPluginConfig
            // 
            this.mnuTrackFXVSTPluginConfig.Name = "mnuTrackFXVSTPluginConfig";
            this.mnuTrackFXVSTPluginConfig.Size = new System.Drawing.Size(310, 26);
            this.mnuTrackFXVSTPluginConfig.Text = "Track &FX VST Effect Settings...";
            // 
            // mnuTrackFXVSTPluginConfig2
            // 
            this.mnuTrackFXVSTPluginConfig2.Name = "mnuTrackFXVSTPluginConfig2";
            this.mnuTrackFXVSTPluginConfig2.Size = new System.Drawing.Size(310, 26);
            this.mnuTrackFXVSTPluginConfig2.Text = "Track FX VST Effect #2 Settings...";
            // 
            // mnuSamplerVSTPluginConfig
            // 
            this.mnuSamplerVSTPluginConfig.Name = "mnuSamplerVSTPluginConfig";
            this.mnuSamplerVSTPluginConfig.Size = new System.Drawing.Size(310, 26);
            this.mnuSamplerVSTPluginConfig.Text = "&Sampler VST Effect Settings...";
            // 
            // mnuSamplerVSTPluginConfig2
            // 
            this.mnuSamplerVSTPluginConfig2.Name = "mnuSamplerVSTPluginConfig2";
            this.mnuSamplerVSTPluginConfig2.Size = new System.Drawing.Size(310, 26);
            this.mnuSamplerVSTPluginConfig2.Text = "&Sampler VST Effect #&2 Settings...";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(307, 6);
            // 
            // mnuConservativeFadeOut
            // 
            this.mnuConservativeFadeOut.Name = "mnuConservativeFadeOut";
            this.mnuConservativeFadeOut.Size = new System.Drawing.Size(310, 26);
            this.mnuConservativeFadeOut.Text = "&Conservative Fade On Poor Mixes";
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(55, 33);
            this.mnuHelp.Text = "&Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(142, 26);
            this.mnuAbout.Text = "&About...";
            // 
            // formStateController
            // 
            this.formStateController.Form = this;
            this.formStateController.FormStateSettings = "";
            // 
            // notificationContextMenu
            // 
            this.notificationContextMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.notificationContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.notificationContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPlayPause,
            this.mnuPause,
            this.mnuNext,
            this.mnuPrevious,
            this.mnuSkipToEnd,
            this.mnuReplayMix,
            this.toolStripSeparator3,
            this.mnuTrackRank,
            this.mnuRank,
            this.mnuSkipAfterMix,
            this.toolStripSeparator2,
            this.mnuExit2});
            this.notificationContextMenu.Name = "notificationContextMenu";
            this.notificationContextMenu.Size = new System.Drawing.Size(223, 276);
            // 
            // mnuPlayPause
            // 
            this.mnuPlayPause.Image = global::Halloumi.Shuffler.Properties.Resources.player_play_small;
            this.mnuPlayPause.Name = "mnuPlayPause";
            this.mnuPlayPause.Size = new System.Drawing.Size(222, 26);
            this.mnuPlayPause.Text = "&Play";
            // 
            // mnuPause
            // 
            this.mnuPause.Image = global::Halloumi.Shuffler.Properties.Resources.player_pause_small;
            this.mnuPause.Name = "mnuPause";
            this.mnuPause.Size = new System.Drawing.Size(222, 26);
            this.mnuPause.Text = "&Pause";
            // 
            // mnuNext
            // 
            this.mnuNext.Image = global::Halloumi.Shuffler.Properties.Resources.player_end_small;
            this.mnuNext.Name = "mnuNext";
            this.mnuNext.Size = new System.Drawing.Size(222, 26);
            this.mnuNext.Text = "&Next";
            // 
            // mnuPrevious
            // 
            this.mnuPrevious.Image = ((System.Drawing.Image)(resources.GetObject("mnuPrevious.Image")));
            this.mnuPrevious.Name = "mnuPrevious";
            this.mnuPrevious.Size = new System.Drawing.Size(222, 26);
            this.mnuPrevious.Text = "Pre&vious";
            // 
            // mnuSkipToEnd
            // 
            this.mnuSkipToEnd.Image = global::Halloumi.Shuffler.Properties.Resources.player_fwd_small;
            this.mnuSkipToEnd.Name = "mnuSkipToEnd";
            this.mnuSkipToEnd.Size = new System.Drawing.Size(222, 26);
            this.mnuSkipToEnd.Text = "&Skip To End";
            // 
            // mnuReplayMix
            // 
            this.mnuReplayMix.Image = global::Halloumi.Shuffler.Properties.Resources.player_rew1;
            this.mnuReplayMix.Name = "mnuReplayMix";
            this.mnuReplayMix.Size = new System.Drawing.Size(222, 26);
            this.mnuReplayMix.Text = "&Replay Mix";
            this.mnuReplayMix.Click += new System.EventHandler(this.mnuReplayMix_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(219, 6);
            // 
            // mnuTrackRank
            // 
            this.mnuTrackRank.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.mnuTrackRank.Name = "mnuTrackRank";
            this.mnuTrackRank.Size = new System.Drawing.Size(222, 26);
            this.mnuTrackRank.Text = "&Track Rating";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(225, 26);
            this.toolStripMenuItem1.Text = "toolStripMenuItem1";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.mnuTrackRank_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(225, 26);
            this.toolStripMenuItem2.Text = "toolStripMenuItem2";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.mnuTrackRank_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(225, 26);
            this.toolStripMenuItem3.Text = "toolStripMenuItem3";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.mnuTrackRank_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(225, 26);
            this.toolStripMenuItem4.Text = "toolStripMenuItem4";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.mnuTrackRank_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(225, 26);
            this.toolStripMenuItem5.Text = "toolStripMenuItem5";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.mnuTrackRank_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(225, 26);
            this.toolStripMenuItem6.Text = "toolStripMenuItem6";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.mnuTrackRank_Click);
            // 
            // mnuRank
            // 
            this.mnuRank.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRank0,
            this.mnuRank1,
            this.mnuRank2,
            this.mnuRank3,
            this.mnuRank4,
            this.mnuRank5});
            this.mnuRank.Name = "mnuRank";
            this.mnuRank.Size = new System.Drawing.Size(222, 26);
            this.mnuRank.Text = "&Mix Rating";
            // 
            // mnuRank0
            // 
            this.mnuRank0.Name = "mnuRank0";
            this.mnuRank0.Size = new System.Drawing.Size(225, 26);
            this.mnuRank0.Text = "toolStripMenuItem1";
            this.mnuRank0.Click += new System.EventHandler(this.mnuRank_Click);
            // 
            // mnuRank1
            // 
            this.mnuRank1.Name = "mnuRank1";
            this.mnuRank1.Size = new System.Drawing.Size(225, 26);
            this.mnuRank1.Text = "toolStripMenuItem2";
            this.mnuRank1.Click += new System.EventHandler(this.mnuRank_Click);
            // 
            // mnuRank2
            // 
            this.mnuRank2.Name = "mnuRank2";
            this.mnuRank2.Size = new System.Drawing.Size(225, 26);
            this.mnuRank2.Text = "toolStripMenuItem3";
            this.mnuRank2.Click += new System.EventHandler(this.mnuRank_Click);
            // 
            // mnuRank3
            // 
            this.mnuRank3.Name = "mnuRank3";
            this.mnuRank3.Size = new System.Drawing.Size(225, 26);
            this.mnuRank3.Text = "toolStripMenuItem4";
            this.mnuRank3.Click += new System.EventHandler(this.mnuRank_Click);
            // 
            // mnuRank4
            // 
            this.mnuRank4.Name = "mnuRank4";
            this.mnuRank4.Size = new System.Drawing.Size(225, 26);
            this.mnuRank4.Text = "toolStripMenuItem5";
            this.mnuRank4.Click += new System.EventHandler(this.mnuRank_Click);
            // 
            // mnuRank5
            // 
            this.mnuRank5.Name = "mnuRank5";
            this.mnuRank5.Size = new System.Drawing.Size(225, 26);
            this.mnuRank5.Text = "toolStripMenuItem1";
            this.mnuRank5.Click += new System.EventHandler(this.mnuRank_Click);
            // 
            // mnuSkipAfterMix
            // 
            this.mnuSkipAfterMix.CheckOnClick = true;
            this.mnuSkipAfterMix.Name = "mnuSkipAfterMix";
            this.mnuSkipAfterMix.Size = new System.Drawing.Size(222, 26);
            this.mnuSkipAfterMix.Text = "S&kip After Rating Mix";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(219, 6);
            // 
            // mnuExit2
            // 
            this.mnuExit2.Image = global::Halloumi.Shuffler.Properties.Resources.exit;
            this.mnuExit2.Name = "mnuExit2";
            this.mnuExit2.Size = new System.Drawing.Size(222, 26);
            this.mnuExit2.Text = "E&xit";
            this.mnuExit2.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblPlayerStatus,
            this.lblLibraryStatus,
            this.lblPlaylistStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 773);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip.Size = new System.Drawing.Size(1253, 26);
            this.statusStrip.TabIndex = 33;
            // 
            // lblPlayerStatus
            // 
            this.lblPlayerStatus.Name = "lblPlayerStatus";
            this.lblPlayerStatus.Size = new System.Drawing.Size(942, 20);
            this.lblPlayerStatus.Spring = true;
            this.lblPlayerStatus.Text = "0:00 remaining";
            this.lblPlayerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLibraryStatus
            // 
            this.lblLibraryStatus.Name = "lblLibraryStatus";
            this.lblLibraryStatus.Size = new System.Drawing.Size(124, 20);
            this.lblLibraryStatus.Text = "0 tracks in library.";
            this.lblLibraryStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPlaylistStatus
            // 
            this.lblPlaylistStatus.Name = "lblPlaylistStatus";
            this.lblPlaylistStatus.Size = new System.Drawing.Size(167, 20);
            this.lblPlaylistStatus.Text = "0 tracks in playlist (0:00)";
            this.lblPlaylistStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // fileMenuController
            // 
            this.fileMenuController.CanCreateNew = false;
            this.fileMenuController.CanReload = false;
            this.fileMenuController.FileFilter = "Playlist Files (*.m3u)|*.m3u";
            this.fileMenuController.FileMenu = this.mnuFile;
            this.fileMenuController.ForceSaveAs = true;
            this.fileMenuController.SaveDocument += new Halloumi.Common.Windows.Controllers.FileMenuControllerEventHandler(this.fileMenuController_SaveDocument);
            this.fileMenuController.LoadDocument += new Halloumi.Common.Windows.Controllers.FileMenuControllerEventHandler(this.fileMenuController_LoadDocument);
            // 
            // aboutDialog
            // 
            this.aboutDialog.Image = global::Halloumi.Shuffler.Properties.Resources.logo;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMain.Controls.Add(this.trackLibraryControl);
            this.pnlMain.Controls.Add(this.playlistControl);
            this.pnlMain.Controls.Add(this.mixerControl);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 149);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1253, 624);
            this.pnlMain.TabIndex = 36;
            // 
            // trackLibraryControl
            // 
            this.trackLibraryControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackLibraryControl.Location = new System.Drawing.Point(0, 0);
            this.trackLibraryControl.Margin = new System.Windows.Forms.Padding(5);
            this.trackLibraryControl.Name = "trackLibraryControl";
            this.trackLibraryControl.Size = new System.Drawing.Size(1253, 624);
            this.trackLibraryControl.TabIndex = 2;
            // 
            // playlistControl
            // 
            this.playlistControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playlistControl.Location = new System.Drawing.Point(0, 0);
            this.playlistControl.Margin = new System.Windows.Forms.Padding(5);
            this.playlistControl.Name = "playlistControl";
            this.playlistControl.Size = new System.Drawing.Size(1253, 624);
            this.playlistControl.TabIndex = 3;
            // 
            // mixerControl
            // 
            this.mixerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mixerControl.Location = new System.Drawing.Point(0, 0);
            this.mixerControl.Margin = new System.Windows.Forms.Padding(0);
            this.mixerControl.Name = "mixerControl";
            this.mixerControl.Size = new System.Drawing.Size(1253, 624);
            this.mixerControl.TabIndex = 1;
            this.mixerControl.Visible = false;
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(307, 6);
            // 
            // mnuResetMidi
            // 
            this.mnuResetMidi.Name = "mnuResetMidi";
            this.mnuResetMidi.Size = new System.Drawing.Size(310, 26);
            this.mnuResetMidi.Text = "&Reset Midi";
            this.mnuResetMidi.Click += new System.EventHandler(this.MnuResetMidi_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1253, 799);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pnlTop);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmMain";
            this.Text = "Halloumi : Shuffler";
            this.UseApplicationIcon = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlMenuBar)).EndInit();
            this.pnlMenuBar.ResumeLayout(false);
            this.pnlMenuBar.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.notificationContextMenu.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager;
        private System.Windows.Forms.Panel pnlTop;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel pnlMenuBar;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge linMenuBorder;
        private Halloumi.Shuffler.Controls.PlayerDetails playerDetails;
        private Halloumi.Common.Windows.Controls.AboutDialog aboutDialog;
        private Halloumi.Common.Windows.Contollers.FormStateController formStateController;
        private System.Windows.Forms.ContextMenuStrip notificationContextMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuPlayPause;
        private System.Windows.Forms.ToolStripMenuItem mnuNext;
        private System.Windows.Forms.ToolStripMenuItem mnuPrevious;
        private System.Windows.Forms.ToolStripMenuItem mnuSkipToEnd;
        private System.Windows.Forms.ToolStripMenuItem mnuRank;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuExit2;
        private System.Windows.Forms.ToolStripMenuItem mnuPause;
        private System.Windows.Forms.ToolStripMenuItem mnuRank0;
        private System.Windows.Forms.ToolStripMenuItem mnuRank1;
        private System.Windows.Forms.ToolStripMenuItem mnuRank2;
        private System.Windows.Forms.ToolStripMenuItem mnuRank3;
        private System.Windows.Forms.ToolStripMenuItem mnuRank4;
        private System.Windows.Forms.ToolStripMenuItem mnuRank5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblLibraryStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblPlaylistStatus;
        private Halloumi.Shuffler.Controls.MixerControl mixerControl;
        private Halloumi.Shuffler.Controls.TrackLibraryControl trackLibraryControl;
        private Halloumi.Common.Windows.Controllers.FileMenuController fileMenuController;
        private System.Windows.Forms.ToolStripMenuItem mnuTrackRank;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem mnuSkipAfterMix;
        private System.Windows.Forms.ToolStripMenuItem mnuReplayMix;
        private Halloumi.Common.Windows.Controls.Panel pnlMain;
        private Halloumi.Shuffler.Controls.PlaylistControl playlistControl;
        private Halloumi.Shuffler.Controls.ShufflerController shufflerController;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuBestMix;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem mnuAutoshuffle;
        private System.Windows.Forms.ToolStripMenuItem mnuAutoshuffleSettings;
        private System.Windows.Forms.ToolStripMenuItem mnuAutoGenerateNow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem mnuExportPlaylistTracks;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuLibrary;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateLibrary;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelLibraryUpdate;
        private System.Windows.Forms.ToolStripMenuItem mnuCleanLibrary;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateLibraryOnStartup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateDuplicateTracks;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem mnuExportLibraryTracks;
        private System.Windows.Forms.ToolStripMenuItem mnuView;
        private System.Windows.Forms.ToolStripMenuItem mnuViewPlaylist;
        private System.Windows.Forms.ToolStripMenuItem mnuViewLibrary;
        private System.Windows.Forms.ToolStripMenuItem mnuViewMixer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem mnuShowMixableTracks;
        private System.Windows.Forms.ToolStripMenuItem mnuViewVisuals;
        private System.Windows.Forms.ToolStripMenuItem mnuViewAlbumArt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem mnuMinimizeToTray;
        private System.Windows.Forms.ToolStripMenuItem mnuOptions;
        private System.Windows.Forms.ToolStripMenuItem mnuSettings;
        private System.Windows.Forms.ToolStripMenuItem mnuMonitorSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mnuPlugins;
        private System.Windows.Forms.ToolStripMenuItem mnuWinampDSPConfig;
        private System.Windows.Forms.ToolStripMenuItem mnuVSTPluginConfig;
        private System.Windows.Forms.ToolStripMenuItem mnuTrackVSTPluginConfig;
        private System.Windows.Forms.ToolStripMenuItem mnuTrackFXVSTPluginConfig;
        private System.Windows.Forms.ToolStripMenuItem mnuTrackFXVSTPluginConfig2;
        private System.Windows.Forms.ToolStripMenuItem mnuSamplerVSTPluginConfig;
        private System.Windows.Forms.ToolStripMenuItem mnuSamplerVSTPluginConfig2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mnuConservativeFadeOut;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ToolStripMenuItem mnuTracker;
        private System.Windows.Forms.ToolStripMenuItem mnuSampleLibrary;
        private System.Windows.Forms.ToolStripMenuItem mnuExportShufflerTracks;
        private System.Windows.Forms.ToolStripMenuItem mnuModuleEditor;
        private System.Windows.Forms.ToolStripMenuItem mnuShowTrackDetails;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem mnuImportCollection;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteCollection;
        private System.Windows.Forms.ToolStripMenuItem mnuShowPlayer;
        private System.Windows.Forms.ToolStripStatusLabel lblPlayerStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem mnuImportTracks;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem mnuResetMidi;
    }
}