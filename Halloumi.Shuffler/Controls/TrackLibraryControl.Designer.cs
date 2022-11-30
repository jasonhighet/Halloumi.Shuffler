using System;

namespace Halloumi.Shuffler.Controls
{
    partial class TrackLibraryControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mnuGenre = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRenameGenre = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuArtist = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRenameArtist = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAlbum = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRenameAlbum = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdateAlbumArtist = new System.Windows.Forms.ToolStripMenuItem();
            this.imlAlbumArt = new System.Windows.Forms.ImageList(this.components);
            this.mnuTrack = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuPlay = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQueue = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAddTrackToCollection = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRemoveTrackFromCollection = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuOpenFileLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuUpdateTrackDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdateArtist = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdateAlbum = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdateGenre = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdateShufflerDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRemoveShufflerDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReloadMetadata = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCalculateKey = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRank = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.sepOpenFileLocation = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAddToSampler = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditSamples = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExportMixSectionsAsSamples = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.pnlBackground2 = new Halloumi.Common.Windows.Controls.Panel();
            this.splLibraryMixable = new System.Windows.Forms.SplitContainer();
            this.pnlLibraryDetails = new Halloumi.Common.Windows.Controls.Panel();
            this.splLibrary = new ComponentFactory.Krypton.Toolkit.KryptonSplitContainer();
            this.splLeftRight = new ComponentFactory.Krypton.Toolkit.KryptonSplitContainer();
            this.splLeftMiddle = new ComponentFactory.Krypton.Toolkit.KryptonSplitContainer();
            this.pnlGenre = new Halloumi.Common.Windows.Controls.Panel();
            this.grdGenre = new Halloumi.Shuffler.Controls.DataGridView();
            this.colGenre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlArtist = new Halloumi.Common.Windows.Controls.Panel();
            this.grdArtist = new Halloumi.Shuffler.Controls.DataGridView();
            this.colArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlAlbum = new Halloumi.Common.Windows.Controls.Panel();
            this.lstAlbum = new Halloumi.Common.Windows.Controls.ListView();
            this.colAlbumName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAlbumAlbumArtist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.linAlbumHeader = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.pnlAlbumHeader = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.lblAlbumHeader = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.pnlTrack = new Halloumi.Common.Windows.Controls.Panel();
            this.grdTracks = new Halloumi.Shuffler.Controls.DataGridView();
            this.colTrackDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrackAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrackGenre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrackLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrackStartBPM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrackEndBPM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrackNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnrankedCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrackRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrackKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBitrate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trackDetails = new Halloumi.Shuffler.Controls.TrackDetails();
            this.pnlDivider = new Halloumi.Common.Windows.Controls.Panel();
            this.flpToolbarRight = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFilter = new Halloumi.Common.Windows.Controls.Label();
            this.txtSearch = new Halloumi.Common.Windows.Controls.TextBox();
            this.label5 = new Halloumi.Common.Windows.Controls.Label();
            this.txtMinBPM = new Halloumi.Common.Windows.Controls.TextBox();
            this.label4 = new Halloumi.Common.Windows.Controls.Label();
            this.txtMaxBPM = new Halloumi.Common.Windows.Controls.TextBox();
            this.label1 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbCollection = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label7 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbExcludedCollection = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label6 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbTrackRankFilter = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label2 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbQueued = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label3 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbShufflerFilter = new Halloumi.Common.Windows.Controls.ComboBox();
            this.mixableTracks = new Halloumi.Shuffler.Controls.MixableTracks();
            this.mnuGenre.SuspendLayout();
            this.mnuArtist.SuspendLayout();
            this.mnuAlbum.SuspendLayout();
            this.mnuTrack.SuspendLayout();
            this.pnlBackground2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splLibraryMixable)).BeginInit();
            this.splLibraryMixable.Panel1.SuspendLayout();
            this.splLibraryMixable.Panel2.SuspendLayout();
            this.splLibraryMixable.SuspendLayout();
            this.pnlLibraryDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splLibrary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splLibrary.Panel1)).BeginInit();
            this.splLibrary.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splLibrary.Panel2)).BeginInit();
            this.splLibrary.Panel2.SuspendLayout();
            this.splLibrary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splLeftRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splLeftRight.Panel1)).BeginInit();
            this.splLeftRight.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splLeftRight.Panel2)).BeginInit();
            this.splLeftRight.Panel2.SuspendLayout();
            this.splLeftRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splLeftMiddle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splLeftMiddle.Panel1)).BeginInit();
            this.splLeftMiddle.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splLeftMiddle.Panel2)).BeginInit();
            this.splLeftMiddle.Panel2.SuspendLayout();
            this.splLeftMiddle.SuspendLayout();
            this.pnlGenre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdGenre)).BeginInit();
            this.pnlArtist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdArtist)).BeginInit();
            this.pnlAlbum.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAlbumHeader)).BeginInit();
            this.pnlAlbumHeader.SuspendLayout();
            this.pnlTrack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTracks)).BeginInit();
            this.pnlDivider.SuspendLayout();
            this.flpToolbarRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCollection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExcludedCollection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrackRankFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbQueued)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbShufflerFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // mnuGenre
            // 
            this.mnuGenre.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.mnuGenre.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuGenre.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRenameGenre});
            this.mnuGenre.Name = "mnuGenre";
            this.mnuGenre.Size = new System.Drawing.Size(238, 40);
            // 
            // mnuRenameGenre
            // 
            this.mnuRenameGenre.Name = "mnuRenameGenre";
            this.mnuRenameGenre.Size = new System.Drawing.Size(237, 36);
            this.mnuRenameGenre.Text = "&Rename Genre...";
            // 
            // mnuArtist
            // 
            this.mnuArtist.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.mnuArtist.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuArtist.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRenameArtist});
            this.mnuArtist.Name = "mnuGenre";
            this.mnuArtist.Size = new System.Drawing.Size(232, 40);
            // 
            // mnuRenameArtist
            // 
            this.mnuRenameArtist.Name = "mnuRenameArtist";
            this.mnuRenameArtist.Size = new System.Drawing.Size(231, 36);
            this.mnuRenameArtist.Text = "&Rename Artist...";
            // 
            // mnuAlbum
            // 
            this.mnuAlbum.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.mnuAlbum.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuAlbum.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRenameAlbum,
            this.mnuUpdateAlbumArtist});
            this.mnuAlbum.Name = "mnuGenre";
            this.mnuAlbum.Size = new System.Drawing.Size(291, 76);
            // 
            // mnuRenameAlbum
            // 
            this.mnuRenameAlbum.Name = "mnuRenameAlbum";
            this.mnuRenameAlbum.Size = new System.Drawing.Size(290, 36);
            this.mnuRenameAlbum.Text = "&Rename Album...";
            // 
            // mnuUpdateAlbumArtist
            // 
            this.mnuUpdateAlbumArtist.Name = "mnuUpdateAlbumArtist";
            this.mnuUpdateAlbumArtist.Size = new System.Drawing.Size(290, 36);
            this.mnuUpdateAlbumArtist.Text = "Update Album A&rtist...";
            // 
            // imlAlbumArt
            // 
            this.imlAlbumArt.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imlAlbumArt.ImageSize = new System.Drawing.Size(75, 75);
            this.imlAlbumArt.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // mnuTrack
            // 
            this.mnuTrack.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.mnuTrack.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuTrack.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPlay,
            this.mnuQueue,
            this.toolStripSeparator2,
            this.mnuAddTrackToCollection,
            this.mnuRemoveTrackFromCollection,
            this.toolStripSeparator1,
            this.mnuOpenFileLocation,
            this.toolStripSeparator3,
            this.mnuUpdateTrackDetails,
            this.mnuUpdateArtist,
            this.mnuUpdateAlbum,
            this.mnuUpdateGenre,
            this.mnuUpdateShufflerDetails,
            this.mnuRemoveShufflerDetails,
            this.mnuReloadMetadata,
            this.mnuCalculateKey,
            this.toolStripSeparator4,
            this.mnuRank,
            this.sepOpenFileLocation,
            this.mnuAddToSampler,
            this.mnuEditSamples,
            this.mnuExportMixSectionsAsSamples});
            this.mnuTrack.Name = "mnuTrack";
            this.mnuTrack.Size = new System.Drawing.Size(381, 646);
            // 
            // mnuPlay
            // 
            this.mnuPlay.Name = "mnuPlay";
            this.mnuPlay.Size = new System.Drawing.Size(380, 36);
            this.mnuPlay.Text = "&Play";
            // 
            // mnuQueue
            // 
            this.mnuQueue.Name = "mnuQueue";
            this.mnuQueue.Size = new System.Drawing.Size(380, 36);
            this.mnuQueue.Text = "&Queue";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(377, 6);
            // 
            // mnuAddTrackToCollection
            // 
            this.mnuAddTrackToCollection.Name = "mnuAddTrackToCollection";
            this.mnuAddTrackToCollection.Size = new System.Drawing.Size(380, 36);
            this.mnuAddTrackToCollection.Text = "A&dd To Collection";
            // 
            // mnuRemoveTrackFromCollection
            // 
            this.mnuRemoveTrackFromCollection.Name = "mnuRemoveTrackFromCollection";
            this.mnuRemoveTrackFromCollection.Size = new System.Drawing.Size(380, 36);
            this.mnuRemoveTrackFromCollection.Text = "&Remove From Collection";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(377, 6);
            // 
            // mnuOpenFileLocation
            // 
            this.mnuOpenFileLocation.Name = "mnuOpenFileLocation";
            this.mnuOpenFileLocation.Size = new System.Drawing.Size(380, 36);
            this.mnuOpenFileLocation.Text = "Open File &Location";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(377, 6);
            // 
            // mnuUpdateTrackDetails
            // 
            this.mnuUpdateTrackDetails.Name = "mnuUpdateTrackDetails";
            this.mnuUpdateTrackDetails.Size = new System.Drawing.Size(380, 36);
            this.mnuUpdateTrackDetails.Text = "Update &Track Details";
            // 
            // mnuUpdateArtist
            // 
            this.mnuUpdateArtist.Name = "mnuUpdateArtist";
            this.mnuUpdateArtist.Size = new System.Drawing.Size(380, 36);
            this.mnuUpdateArtist.Text = "Update A&rtist...";
            // 
            // mnuUpdateAlbum
            // 
            this.mnuUpdateAlbum.Name = "mnuUpdateAlbum";
            this.mnuUpdateAlbum.Size = new System.Drawing.Size(380, 36);
            this.mnuUpdateAlbum.Text = "Update &Album...";
            // 
            // mnuUpdateGenre
            // 
            this.mnuUpdateGenre.Name = "mnuUpdateGenre";
            this.mnuUpdateGenre.Size = new System.Drawing.Size(380, 36);
            this.mnuUpdateGenre.Text = "Update &Genre...";
            // 
            // mnuUpdateShufflerDetails
            // 
            this.mnuUpdateShufflerDetails.Name = "mnuUpdateShufflerDetails";
            this.mnuUpdateShufflerDetails.Size = new System.Drawing.Size(380, 36);
            this.mnuUpdateShufflerDetails.Text = "Update &Shuffler Details...";
            // 
            // mnuRemoveShufflerDetails
            // 
            this.mnuRemoveShufflerDetails.Name = "mnuRemoveShufflerDetails";
            this.mnuRemoveShufflerDetails.Size = new System.Drawing.Size(380, 36);
            this.mnuRemoveShufflerDetails.Text = "&Remove Shuffer Details";
            // 
            // mnuReloadMetadata
            // 
            this.mnuReloadMetadata.Name = "mnuReloadMetadata";
            this.mnuReloadMetadata.Size = new System.Drawing.Size(380, 36);
            this.mnuReloadMetadata.Text = "Reload &Metadata";
            this.mnuReloadMetadata.Click += new System.EventHandler(this.mnuReloadMetadata_Click);
            // 
            // mnuCalculateKey
            // 
            this.mnuCalculateKey.Name = "mnuCalculateKey";
            this.mnuCalculateKey.Size = new System.Drawing.Size(380, 36);
            this.mnuCalculateKey.Text = "Calculate &Key";
            this.mnuCalculateKey.Click += new System.EventHandler(this.mnuCalculateKey_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(377, 6);
            // 
            // mnuRank
            // 
            this.mnuRank.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.mnuRank.Name = "mnuRank";
            this.mnuRank.Size = new System.Drawing.Size(380, 36);
            this.mnuRank.Text = "Track Rating";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(315, 40);
            this.toolStripMenuItem1.Text = "toolStripMenuItem1";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.mnuRank_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(315, 40);
            this.toolStripMenuItem2.Text = "toolStripMenuItem2";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.mnuRank_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(315, 40);
            this.toolStripMenuItem3.Text = "toolStripMenuItem3";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.mnuRank_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(315, 40);
            this.toolStripMenuItem4.Text = "toolStripMenuItem4";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.mnuRank_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(315, 40);
            this.toolStripMenuItem5.Text = "toolStripMenuItem5";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.mnuRank_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(315, 40);
            this.toolStripMenuItem6.Text = "toolStripMenuItem6";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.mnuRank_Click);
            // 
            // sepOpenFileLocation
            // 
            this.sepOpenFileLocation.Name = "sepOpenFileLocation";
            this.sepOpenFileLocation.Size = new System.Drawing.Size(377, 6);
            // 
            // mnuAddToSampler
            // 
            this.mnuAddToSampler.Name = "mnuAddToSampler";
            this.mnuAddToSampler.Size = new System.Drawing.Size(380, 36);
            // 
            // mnuEditSamples
            // 
            this.mnuEditSamples.Name = "mnuEditSamples";
            this.mnuEditSamples.Size = new System.Drawing.Size(380, 36);
            this.mnuEditSamples.Text = "Edit Samples";
            this.mnuEditSamples.Click += new System.EventHandler(this.mnuEditSamples_Click);
            // 
            // mnuExportMixSectionsAsSamples
            // 
            this.mnuExportMixSectionsAsSamples.Name = "mnuExportMixSectionsAsSamples";
            this.mnuExportMixSectionsAsSamples.Size = new System.Drawing.Size(380, 36);
            this.mnuExportMixSectionsAsSamples.Text = "Export Mi&x Sections As Samples";
            this.mnuExportMixSectionsAsSamples.Click += new System.EventHandler(this.mnuExportMixSectionsAsSamples_Click);
            // 
            // pnlBackground2
            // 
            this.pnlBackground2.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBackground2.Controls.Add(this.splLibraryMixable);
            this.pnlBackground2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground2.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground2.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBackground2.Name = "pnlBackground2";
            this.pnlBackground2.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.pnlBackground2.Size = new System.Drawing.Size(1866, 1090);
            this.pnlBackground2.TabIndex = 12;
            // 
            // splLibraryMixable
            // 
            this.splLibraryMixable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splLibraryMixable.Location = new System.Drawing.Point(7, 8);
            this.splLibraryMixable.Margin = new System.Windows.Forms.Padding(4);
            this.splLibraryMixable.Name = "splLibraryMixable";
            this.splLibraryMixable.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splLibraryMixable.Panel1
            // 
            this.splLibraryMixable.Panel1.Controls.Add(this.pnlLibraryDetails);
            // 
            // splLibraryMixable.Panel2
            // 
            this.splLibraryMixable.Panel2.Controls.Add(this.mixableTracks);
            this.splLibraryMixable.Size = new System.Drawing.Size(1852, 1074);
            this.splLibraryMixable.SplitterDistance = 823;
            this.splLibraryMixable.SplitterWidth = 6;
            this.splLibraryMixable.TabIndex = 0;
            // 
            // pnlLibraryDetails
            // 
            this.pnlLibraryDetails.BackColor = System.Drawing.SystemColors.Control;
            this.pnlLibraryDetails.Controls.Add(this.splLibrary);
            this.pnlLibraryDetails.Controls.Add(this.trackDetails);
            this.pnlLibraryDetails.Controls.Add(this.pnlDivider);
            this.pnlLibraryDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLibraryDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlLibraryDetails.Margin = new System.Windows.Forms.Padding(6);
            this.pnlLibraryDetails.Name = "pnlLibraryDetails";
            this.pnlLibraryDetails.Size = new System.Drawing.Size(1852, 823);
            this.pnlLibraryDetails.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlLibraryDetails.TabIndex = 11;
            // 
            // splLibrary
            // 
            this.splLibrary.Cursor = System.Windows.Forms.Cursors.Default;
            this.splLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splLibrary.Location = new System.Drawing.Point(0, 56);
            this.splLibrary.Margin = new System.Windows.Forms.Padding(6);
            this.splLibrary.Name = "splLibrary";
            this.splLibrary.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splLibrary.Panel1
            // 
            this.splLibrary.Panel1.Controls.Add(this.splLeftRight);
            this.splLibrary.Panel1.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            // 
            // splLibrary.Panel2
            // 
            this.splLibrary.Panel2.Controls.Add(this.pnlTrack);
            this.splLibrary.Panel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.splLibrary.Size = new System.Drawing.Size(1852, 657);
            this.splLibrary.SplitterDistance = 280;
            this.splLibrary.TabIndex = 55;
            // 
            // splLeftRight
            // 
            this.splLeftRight.Cursor = System.Windows.Forms.Cursors.Default;
            this.splLeftRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splLeftRight.Location = new System.Drawing.Point(0, 9);
            this.splLeftRight.Margin = new System.Windows.Forms.Padding(6);
            this.splLeftRight.Name = "splLeftRight";
            // 
            // splLeftRight.Panel1
            // 
            this.splLeftRight.Panel1.Controls.Add(this.splLeftMiddle);
            // 
            // splLeftRight.Panel2
            // 
            this.splLeftRight.Panel2.Controls.Add(this.pnlAlbum);
            this.splLeftRight.Size = new System.Drawing.Size(1852, 271);
            this.splLeftRight.SplitterDistance = 714;
            this.splLeftRight.TabIndex = 0;
            // 
            // splLeftMiddle
            // 
            this.splLeftMiddle.Cursor = System.Windows.Forms.Cursors.Default;
            this.splLeftMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splLeftMiddle.Location = new System.Drawing.Point(0, 0);
            this.splLeftMiddle.Margin = new System.Windows.Forms.Padding(6);
            this.splLeftMiddle.Name = "splLeftMiddle";
            // 
            // splLeftMiddle.Panel1
            // 
            this.splLeftMiddle.Panel1.Controls.Add(this.pnlGenre);
            // 
            // splLeftMiddle.Panel2
            // 
            this.splLeftMiddle.Panel2.Controls.Add(this.pnlArtist);
            this.splLeftMiddle.Size = new System.Drawing.Size(714, 271);
            this.splLeftMiddle.SplitterDistance = 330;
            this.splLeftMiddle.TabIndex = 0;
            // 
            // pnlGenre
            // 
            this.pnlGenre.BackColor = System.Drawing.SystemColors.Control;
            this.pnlGenre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGenre.Controls.Add(this.grdGenre);
            this.pnlGenre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGenre.Location = new System.Drawing.Point(0, 0);
            this.pnlGenre.Margin = new System.Windows.Forms.Padding(6);
            this.pnlGenre.Name = "pnlGenre";
            this.pnlGenre.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.pnlGenre.Size = new System.Drawing.Size(330, 271);
            this.pnlGenre.TabIndex = 2;
            // 
            // grdGenre
            // 
            this.grdGenre.AllowUserToAddRows = false;
            this.grdGenre.AllowUserToDeleteRows = false;
            this.grdGenre.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grdGenre.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdGenre.ColumnHeadersHeight = 26;
            this.grdGenre.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGenre});
            this.grdGenre.ContextMenuStrip = this.mnuGenre;
            this.grdGenre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdGenre.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            this.grdGenre.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellSheet;
            this.grdGenre.HideOuterBorders = true;
            this.grdGenre.Location = new System.Drawing.Point(1, 2);
            this.grdGenre.Margin = new System.Windows.Forms.Padding(6);
            this.grdGenre.MergeColor = System.Drawing.Color.Gainsboro;
            this.grdGenre.Name = "grdGenre";
            this.grdGenre.ReadOnly = true;
            this.grdGenre.RowHeadersVisible = false;
            this.grdGenre.RowHeadersWidth = 72;
            this.grdGenre.RowTemplate.Height = 44;
            this.grdGenre.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdGenre.Size = new System.Drawing.Size(328, 267);
            this.grdGenre.SortColumnIndex = 0;
            this.grdGenre.SortOrder = System.Windows.Forms.SortOrder.None;
            this.grdGenre.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellSheet;
            this.grdGenre.StateCommon.DataCell.Border.Color1 = System.Drawing.Color.White;
            this.grdGenre.StateCommon.DataCell.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grdGenre.StateCommon.DataCell.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.grdGenre.TabIndex = 0;
            // 
            // colGenre
            // 
            this.colGenre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGenre.DataPropertyName = "Name";
            this.colGenre.HeaderText = "Genre";
            this.colGenre.MinimumWidth = 9;
            this.colGenre.Name = "colGenre";
            this.colGenre.ReadOnly = true;
            // 
            // pnlArtist
            // 
            this.pnlArtist.BackColor = System.Drawing.SystemColors.Control;
            this.pnlArtist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlArtist.Controls.Add(this.grdArtist);
            this.pnlArtist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlArtist.Location = new System.Drawing.Point(0, 0);
            this.pnlArtist.Margin = new System.Windows.Forms.Padding(6);
            this.pnlArtist.Name = "pnlArtist";
            this.pnlArtist.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.pnlArtist.Size = new System.Drawing.Size(379, 271);
            this.pnlArtist.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlArtist.TabIndex = 3;
            // 
            // grdArtist
            // 
            this.grdArtist.AllowUserToAddRows = false;
            this.grdArtist.AllowUserToDeleteRows = false;
            this.grdArtist.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grdArtist.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.grdArtist.ColumnHeadersHeight = 26;
            this.grdArtist.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colArtist});
            this.grdArtist.ContextMenuStrip = this.mnuArtist;
            this.grdArtist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdArtist.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            this.grdArtist.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellCustom1;
            this.grdArtist.HideOuterBorders = true;
            this.grdArtist.Location = new System.Drawing.Point(1, 2);
            this.grdArtist.Margin = new System.Windows.Forms.Padding(6);
            this.grdArtist.MergeColor = System.Drawing.Color.Gainsboro;
            this.grdArtist.Name = "grdArtist";
            this.grdArtist.ReadOnly = true;
            this.grdArtist.RowHeadersVisible = false;
            this.grdArtist.RowHeadersWidth = 72;
            this.grdArtist.RowTemplate.Height = 44;
            this.grdArtist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdArtist.Size = new System.Drawing.Size(377, 267);
            this.grdArtist.SortColumnIndex = 0;
            this.grdArtist.SortOrder = System.Windows.Forms.SortOrder.None;
            this.grdArtist.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellCustom1;
            this.grdArtist.StateCommon.DataCell.Border.Color1 = System.Drawing.Color.White;
            this.grdArtist.StateCommon.DataCell.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grdArtist.StateCommon.DataCell.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.grdArtist.TabIndex = 0;
            // 
            // colArtist
            // 
            this.colArtist.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colArtist.DataPropertyName = "Name";
            this.colArtist.HeaderText = "Artist";
            this.colArtist.MinimumWidth = 9;
            this.colArtist.Name = "colArtist";
            this.colArtist.ReadOnly = true;
            // 
            // pnlAlbum
            // 
            this.pnlAlbum.BackColor = System.Drawing.SystemColors.Control;
            this.pnlAlbum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAlbum.Controls.Add(this.lstAlbum);
            this.pnlAlbum.Controls.Add(this.linAlbumHeader);
            this.pnlAlbum.Controls.Add(this.pnlAlbumHeader);
            this.pnlAlbum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAlbum.Location = new System.Drawing.Point(0, 0);
            this.pnlAlbum.Margin = new System.Windows.Forms.Padding(6);
            this.pnlAlbum.Name = "pnlAlbum";
            this.pnlAlbum.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.pnlAlbum.Size = new System.Drawing.Size(1133, 271);
            this.pnlAlbum.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlAlbum.TabIndex = 4;
            // 
            // lstAlbum
            // 
            this.lstAlbum.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstAlbum.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAlbumName,
            this.colAlbumAlbumArtist});
            this.lstAlbum.ContextMenuStrip = this.mnuAlbum;
            this.lstAlbum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAlbum.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lstAlbum.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lstAlbum.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstAlbum.HideSelection = false;
            this.lstAlbum.LargeImageList = this.imlAlbumArt;
            this.lstAlbum.Location = new System.Drawing.Point(1, 43);
            this.lstAlbum.Margin = new System.Windows.Forms.Padding(6);
            this.lstAlbum.Name = "lstAlbum";
            this.lstAlbum.Size = new System.Drawing.Size(1131, 226);
            this.lstAlbum.TabIndex = 3;
            this.lstAlbum.UseCompatibleStateImageBehavior = false;
            // 
            // colAlbumName
            // 
            this.colAlbumName.DisplayIndex = 1;
            this.colAlbumName.Width = 679;
            // 
            // colAlbumAlbumArtist
            // 
            this.colAlbumAlbumArtist.DisplayIndex = 0;
            this.colAlbumAlbumArtist.Width = 0;
            // 
            // linAlbumHeader
            // 
            this.linAlbumHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.linAlbumHeader.Location = new System.Drawing.Point(1, 42);
            this.linAlbumHeader.Margin = new System.Windows.Forms.Padding(6);
            this.linAlbumHeader.Name = "linAlbumHeader";
            this.linAlbumHeader.Size = new System.Drawing.Size(1131, 1);
            this.linAlbumHeader.Text = "kryptonBorderEdge1";
            // 
            // pnlAlbumHeader
            // 
            this.pnlAlbumHeader.Controls.Add(this.lblAlbumHeader);
            this.pnlAlbumHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAlbumHeader.Location = new System.Drawing.Point(1, 2);
            this.pnlAlbumHeader.Margin = new System.Windows.Forms.Padding(6);
            this.pnlAlbumHeader.Name = "pnlAlbumHeader";
            this.pnlAlbumHeader.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderColumnCustom1;
            this.pnlAlbumHeader.Size = new System.Drawing.Size(1131, 40);
            this.pnlAlbumHeader.TabIndex = 1;
            // 
            // lblAlbumHeader
            // 
            this.lblAlbumHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblAlbumHeader.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            this.lblAlbumHeader.Location = new System.Drawing.Point(0, 0);
            this.lblAlbumHeader.Margin = new System.Windows.Forms.Padding(6);
            this.lblAlbumHeader.Name = "lblAlbumHeader";
            this.lblAlbumHeader.Size = new System.Drawing.Size(76, 40);
            this.lblAlbumHeader.TabIndex = 0;
            this.lblAlbumHeader.Values.Text = "Album";
            // 
            // pnlTrack
            // 
            this.pnlTrack.BackColor = System.Drawing.SystemColors.Control;
            this.pnlTrack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTrack.Controls.Add(this.grdTracks);
            this.pnlTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTrack.Location = new System.Drawing.Point(0, 0);
            this.pnlTrack.Margin = new System.Windows.Forms.Padding(6);
            this.pnlTrack.Name = "pnlTrack";
            this.pnlTrack.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.pnlTrack.Size = new System.Drawing.Size(1852, 364);
            this.pnlTrack.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlTrack.TabIndex = 5;
            // 
            // grdTracks
            // 
            this.grdTracks.AllowUserToAddRows = false;
            this.grdTracks.AllowUserToDeleteRows = false;
            this.grdTracks.AllowUserToResizeColumns = false;
            this.grdTracks.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grdTracks.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.grdTracks.ColumnHeadersHeight = 26;
            this.grdTracks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTrackDescription,
            this.colTrackAlbum,
            this.colTrackGenre,
            this.colTrackLength,
            this.colTrackStartBPM,
            this.colTrackEndBPM,
            this.colTrackNumber,
            this.colInCount,
            this.colOutCount,
            this.colUnrankedCount,
            this.colTrackRank,
            this.colTrackKey,
            this.colBitrate});
            this.grdTracks.ContextMenuStrip = this.mnuTrack;
            this.grdTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTracks.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            this.grdTracks.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellList;
            this.grdTracks.HideOuterBorders = true;
            this.grdTracks.Location = new System.Drawing.Point(1, 2);
            this.grdTracks.Margin = new System.Windows.Forms.Padding(6);
            this.grdTracks.MergeColor = System.Drawing.Color.Gainsboro;
            this.grdTracks.Name = "grdTracks";
            this.grdTracks.ReadOnly = true;
            this.grdTracks.RowHeadersVisible = false;
            this.grdTracks.RowHeadersWidth = 72;
            this.grdTracks.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdTracks.RowTemplate.Height = 44;
            this.grdTracks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdTracks.Size = new System.Drawing.Size(1850, 360);
            this.grdTracks.SortColumnIndex = 0;
            this.grdTracks.SortOrder = System.Windows.Forms.SortOrder.None;
            this.grdTracks.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellList;
            this.grdTracks.StateCommon.DataCell.Border.Color1 = System.Drawing.Color.White;
            this.grdTracks.StateCommon.DataCell.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grdTracks.StateCommon.DataCell.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.grdTracks.TabIndex = 53;
            // 
            // colTrackDescription
            // 
            this.colTrackDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTrackDescription.DataPropertyName = "Description";
            this.colTrackDescription.HeaderText = "Description";
            this.colTrackDescription.MinimumWidth = 9;
            this.colTrackDescription.Name = "colTrackDescription";
            this.colTrackDescription.ReadOnly = true;
            this.colTrackDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colTrackAlbum
            // 
            this.colTrackAlbum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTrackAlbum.DataPropertyName = "Album";
            this.colTrackAlbum.FillWeight = 50F;
            this.colTrackAlbum.HeaderText = "Album";
            this.colTrackAlbum.MinimumWidth = 9;
            this.colTrackAlbum.Name = "colTrackAlbum";
            this.colTrackAlbum.ReadOnly = true;
            this.colTrackAlbum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colTrackGenre
            // 
            this.colTrackGenre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTrackGenre.DataPropertyName = "Genre";
            this.colTrackGenre.HeaderText = "Genre";
            this.colTrackGenre.MinimumWidth = 9;
            this.colTrackGenre.Name = "colTrackGenre";
            this.colTrackGenre.ReadOnly = true;
            this.colTrackGenre.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTrackGenre.Width = 113;
            // 
            // colTrackLength
            // 
            this.colTrackLength.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTrackLength.DataPropertyName = "LengthFormatted";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colTrackLength.DefaultCellStyle = dataGridViewCellStyle4;
            this.colTrackLength.HeaderText = "Length";
            this.colTrackLength.MinimumWidth = 9;
            this.colTrackLength.Name = "colTrackLength";
            this.colTrackLength.ReadOnly = true;
            this.colTrackLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTrackLength.Width = 122;
            // 
            // colTrackStartBPM
            // 
            this.colTrackStartBPM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTrackStartBPM.DataPropertyName = "StartBPM";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.colTrackStartBPM.DefaultCellStyle = dataGridViewCellStyle5;
            this.colTrackStartBPM.HeaderText = "Start BPM";
            this.colTrackStartBPM.MinimumWidth = 9;
            this.colTrackStartBPM.Name = "colTrackStartBPM";
            this.colTrackStartBPM.ReadOnly = true;
            this.colTrackStartBPM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTrackStartBPM.Width = 149;
            // 
            // colTrackEndBPM
            // 
            this.colTrackEndBPM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTrackEndBPM.DataPropertyName = "EndBPM";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = null;
            this.colTrackEndBPM.DefaultCellStyle = dataGridViewCellStyle6;
            this.colTrackEndBPM.HeaderText = "End BPM";
            this.colTrackEndBPM.MinimumWidth = 9;
            this.colTrackEndBPM.Name = "colTrackEndBPM";
            this.colTrackEndBPM.ReadOnly = true;
            this.colTrackEndBPM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTrackEndBPM.Width = 142;
            // 
            // colTrackNumber
            // 
            this.colTrackNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTrackNumber.DataPropertyName = "TrackNumberFormatted";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colTrackNumber.DefaultCellStyle = dataGridViewCellStyle7;
            this.colTrackNumber.HeaderText = "#";
            this.colTrackNumber.MinimumWidth = 9;
            this.colTrackNumber.Name = "colTrackNumber";
            this.colTrackNumber.ReadOnly = true;
            this.colTrackNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTrackNumber.Width = 70;
            // 
            // colInCount
            // 
            this.colInCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colInCount.DataPropertyName = "InCount";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colInCount.DefaultCellStyle = dataGridViewCellStyle8;
            this.colInCount.HeaderText = "In";
            this.colInCount.MinimumWidth = 9;
            this.colInCount.Name = "colInCount";
            this.colInCount.ReadOnly = true;
            this.colInCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colInCount.Width = 76;
            // 
            // colOutCount
            // 
            this.colOutCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colOutCount.DataPropertyName = "OutCount";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colOutCount.DefaultCellStyle = dataGridViewCellStyle9;
            this.colOutCount.HeaderText = "Out";
            this.colOutCount.MinimumWidth = 9;
            this.colOutCount.Name = "colOutCount";
            this.colOutCount.ReadOnly = true;
            this.colOutCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colOutCount.Width = 93;
            // 
            // colUnrankedCount
            // 
            this.colUnrankedCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colUnrankedCount.DataPropertyName = "UnrankedCount";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colUnrankedCount.DefaultCellStyle = dataGridViewCellStyle10;
            this.colUnrankedCount.HeaderText = "No";
            this.colUnrankedCount.MinimumWidth = 9;
            this.colUnrankedCount.Name = "colUnrankedCount";
            this.colUnrankedCount.ReadOnly = true;
            this.colUnrankedCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colUnrankedCount.Width = 86;
            // 
            // colTrackRank
            // 
            this.colTrackRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTrackRank.DataPropertyName = "RankDescription";
            this.colTrackRank.HeaderText = "Rank";
            this.colTrackRank.MinimumWidth = 9;
            this.colTrackRank.Name = "colTrackRank";
            this.colTrackRank.ReadOnly = true;
            this.colTrackRank.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTrackRank.Width = 104;
            // 
            // colTrackKey
            // 
            this.colTrackKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTrackKey.DataPropertyName = "Key";
            this.colTrackKey.HeaderText = "Key";
            this.colTrackKey.MinimumWidth = 9;
            this.colTrackKey.Name = "colTrackKey";
            this.colTrackKey.ReadOnly = true;
            this.colTrackKey.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTrackKey.Width = 91;
            // 
            // colBitrate
            // 
            this.colBitrate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colBitrate.DataPropertyName = "Bitrate";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colBitrate.DefaultCellStyle = dataGridViewCellStyle11;
            this.colBitrate.HeaderText = "KPS";
            this.colBitrate.MinimumWidth = 9;
            this.colBitrate.Name = "colBitrate";
            this.colBitrate.ReadOnly = true;
            this.colBitrate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colBitrate.Width = 93;
            // 
            // trackDetails
            // 
            this.trackDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.trackDetails.Location = new System.Drawing.Point(0, 713);
            this.trackDetails.Margin = new System.Windows.Forms.Padding(6);
            this.trackDetails.Name = "trackDetails";
            this.trackDetails.Size = new System.Drawing.Size(1852, 110);
            this.trackDetails.TabIndex = 54;
            // 
            // pnlDivider
            // 
            this.pnlDivider.BackColor = System.Drawing.SystemColors.Control;
            this.pnlDivider.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDivider.Controls.Add(this.flpToolbarRight);
            this.pnlDivider.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDivider.Location = new System.Drawing.Point(0, 0);
            this.pnlDivider.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.Padding = new System.Windows.Forms.Padding(7, 4, 7, 8);
            this.pnlDivider.Size = new System.Drawing.Size(1852, 56);
            this.pnlDivider.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlDivider.TabIndex = 52;
            // 
            // flpToolbarRight
            // 
            this.flpToolbarRight.BackColor = System.Drawing.Color.Transparent;
            this.flpToolbarRight.Controls.Add(this.lblFilter);
            this.flpToolbarRight.Controls.Add(this.txtSearch);
            this.flpToolbarRight.Controls.Add(this.label5);
            this.flpToolbarRight.Controls.Add(this.txtMinBPM);
            this.flpToolbarRight.Controls.Add(this.label4);
            this.flpToolbarRight.Controls.Add(this.txtMaxBPM);
            this.flpToolbarRight.Controls.Add(this.label1);
            this.flpToolbarRight.Controls.Add(this.cmbCollection);
            this.flpToolbarRight.Controls.Add(this.label7);
            this.flpToolbarRight.Controls.Add(this.cmbExcludedCollection);
            this.flpToolbarRight.Controls.Add(this.label6);
            this.flpToolbarRight.Controls.Add(this.cmbTrackRankFilter);
            this.flpToolbarRight.Controls.Add(this.label2);
            this.flpToolbarRight.Controls.Add(this.cmbQueued);
            this.flpToolbarRight.Controls.Add(this.label3);
            this.flpToolbarRight.Controls.Add(this.cmbShufflerFilter);
            this.flpToolbarRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpToolbarRight.Location = new System.Drawing.Point(7, 4);
            this.flpToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flpToolbarRight.Name = "flpToolbarRight";
            this.flpToolbarRight.Size = new System.Drawing.Size(1838, 44);
            this.flpToolbarRight.TabIndex = 0;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblFilter.Location = new System.Drawing.Point(0, 0);
            this.lblFilter.Margin = new System.Windows.Forms.Padding(0);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.lblFilter.Size = new System.Drawing.Size(80, 39);
            this.lblFilter.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblFilter.TabIndex = 5;
            this.lblFilter.Text = "Search:";
            this.lblFilter.Click += new System.EventHandler(this.lblFilter_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.ErrorProvider = null;
            this.txtSearch.Location = new System.Drawing.Point(86, 6);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(6);
            this.txtSearch.MaximumValue = 2147483647D;
            this.txtSearch.MinimumValue = -2147483648D;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(160, 35);
            this.txtSearch.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(252, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.label5.Size = new System.Drawing.Size(54, 39);
            this.label5.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label5.TabIndex = 13;
            this.label5.Text = "Min:";
            // 
            // txtMinBPM
            // 
            this.txtMinBPM.EntryType = Halloumi.Common.Windows.Controls.TextBox.TextEntryType.Integer;
            this.txtMinBPM.ErrorProvider = null;
            this.txtMinBPM.Location = new System.Drawing.Point(312, 6);
            this.txtMinBPM.Margin = new System.Windows.Forms.Padding(6);
            this.txtMinBPM.MaximumValue = 2147483647D;
            this.txtMinBPM.MinimumValue = -2147483648D;
            this.txtMinBPM.Name = "txtMinBPM";
            this.txtMinBPM.Size = new System.Drawing.Size(48, 35);
            this.txtMinBPM.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(366, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.label4.Size = new System.Drawing.Size(58, 39);
            this.label4.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label4.TabIndex = 11;
            this.label4.Text = "Max:";
            // 
            // txtMaxBPM
            // 
            this.txtMaxBPM.EntryType = Halloumi.Common.Windows.Controls.TextBox.TextEntryType.Integer;
            this.txtMaxBPM.ErrorProvider = null;
            this.txtMaxBPM.Location = new System.Drawing.Point(430, 6);
            this.txtMaxBPM.Margin = new System.Windows.Forms.Padding(6);
            this.txtMaxBPM.MaximumValue = 2147483647D;
            this.txtMaxBPM.MinimumValue = -2147483648D;
            this.txtMaxBPM.Name = "txtMaxBPM";
            this.txtMaxBPM.Size = new System.Drawing.Size(48, 35);
            this.txtMaxBPM.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(484, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.label1.Size = new System.Drawing.Size(110, 39);
            this.label1.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label1.TabIndex = 4;
            this.label1.Text = "Collection:";
            // 
            // cmbCollection
            // 
            this.cmbCollection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCollection.DropDownWidth = 121;
            this.cmbCollection.ErrorProvider = null;
            this.cmbCollection.Location = new System.Drawing.Point(600, 6);
            this.cmbCollection.Margin = new System.Windows.Forms.Padding(6);
            this.cmbCollection.Name = "cmbCollection";
            this.cmbCollection.Size = new System.Drawing.Size(188, 33);
            this.cmbCollection.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(794, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.label7.Size = new System.Drawing.Size(89, 39);
            this.label7.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label7.TabIndex = 17;
            this.label7.Text = "Exclude:";
            // 
            // cmbExcludedCollection
            // 
            this.cmbExcludedCollection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExcludedCollection.DropDownWidth = 121;
            this.cmbExcludedCollection.ErrorProvider = null;
            this.cmbExcludedCollection.Location = new System.Drawing.Point(889, 6);
            this.cmbExcludedCollection.Margin = new System.Windows.Forms.Padding(6);
            this.cmbExcludedCollection.Name = "cmbExcludedCollection";
            this.cmbExcludedCollection.Size = new System.Drawing.Size(188, 33);
            this.cmbExcludedCollection.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(1083, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.label6.Size = new System.Drawing.Size(64, 39);
            this.label6.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label6.TabIndex = 14;
            this.label6.Text = "Rank:";
            // 
            // cmbTrackRankFilter
            // 
            this.cmbTrackRankFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTrackRankFilter.DropDownWidth = 121;
            this.cmbTrackRankFilter.ErrorProvider = null;
            this.cmbTrackRankFilter.Items.AddRange(new object[] {
            "",
            "Good+",
            "Bearable+",
            "Unranked",
            "Forbidden"});
            this.cmbTrackRankFilter.Location = new System.Drawing.Point(1153, 6);
            this.cmbTrackRankFilter.Margin = new System.Windows.Forms.Padding(6);
            this.cmbTrackRankFilter.Name = "cmbTrackRankFilter";
            this.cmbTrackRankFilter.Size = new System.Drawing.Size(138, 33);
            this.cmbTrackRankFilter.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(1297, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.label2.Size = new System.Drawing.Size(92, 39);
            this.label2.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label2.TabIndex = 7;
            this.label2.Text = "Queued:";
            // 
            // cmbQueued
            // 
            this.cmbQueued.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQueued.DropDownWidth = 121;
            this.cmbQueued.ErrorProvider = null;
            this.cmbQueued.Items.AddRange(new object[] {
            "",
            "Yes",
            "No"});
            this.cmbQueued.Location = new System.Drawing.Point(1395, 6);
            this.cmbQueued.Margin = new System.Windows.Forms.Padding(6);
            this.cmbQueued.Name = "cmbQueued";
            this.cmbQueued.Size = new System.Drawing.Size(72, 33);
            this.cmbQueued.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(1473, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.label3.Size = new System.Drawing.Size(90, 39);
            this.label3.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label3.TabIndex = 9;
            this.label3.Text = "Shuffler:";
            // 
            // cmbShufflerFilter
            // 
            this.cmbShufflerFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShufflerFilter.DropDownWidth = 121;
            this.cmbShufflerFilter.ErrorProvider = null;
            this.cmbShufflerFilter.Items.AddRange(new object[] {
            "",
            "Yes",
            "No"});
            this.cmbShufflerFilter.Location = new System.Drawing.Point(1569, 6);
            this.cmbShufflerFilter.Margin = new System.Windows.Forms.Padding(6);
            this.cmbShufflerFilter.Name = "cmbShufflerFilter";
            this.cmbShufflerFilter.Size = new System.Drawing.Size(72, 33);
            this.cmbShufflerFilter.TabIndex = 6;
            // 
            // mixableTracks
            // 
            this.mixableTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mixableTracks.Location = new System.Drawing.Point(0, 0);
            this.mixableTracks.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.mixableTracks.Name = "mixableTracks";
            this.mixableTracks.Size = new System.Drawing.Size(1852, 245);
            this.mixableTracks.TabIndex = 0;
            // 
            // TrackLibraryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlBackground2);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "TrackLibraryControl";
            this.Size = new System.Drawing.Size(1866, 1090);
            this.mnuGenre.ResumeLayout(false);
            this.mnuArtist.ResumeLayout(false);
            this.mnuAlbum.ResumeLayout(false);
            this.mnuTrack.ResumeLayout(false);
            this.pnlBackground2.ResumeLayout(false);
            this.splLibraryMixable.Panel1.ResumeLayout(false);
            this.splLibraryMixable.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splLibraryMixable)).EndInit();
            this.splLibraryMixable.ResumeLayout(false);
            this.pnlLibraryDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splLibrary.Panel1)).EndInit();
            this.splLibrary.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splLibrary.Panel2)).EndInit();
            this.splLibrary.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splLibrary)).EndInit();
            this.splLibrary.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splLeftRight.Panel1)).EndInit();
            this.splLeftRight.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splLeftRight.Panel2)).EndInit();
            this.splLeftRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splLeftRight)).EndInit();
            this.splLeftRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splLeftMiddle.Panel1)).EndInit();
            this.splLeftMiddle.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splLeftMiddle.Panel2)).EndInit();
            this.splLeftMiddle.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splLeftMiddle)).EndInit();
            this.splLeftMiddle.ResumeLayout(false);
            this.pnlGenre.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdGenre)).EndInit();
            this.pnlArtist.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdArtist)).EndInit();
            this.pnlAlbum.ResumeLayout(false);
            this.pnlAlbum.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAlbumHeader)).EndInit();
            this.pnlAlbumHeader.ResumeLayout(false);
            this.pnlAlbumHeader.PerformLayout();
            this.pnlTrack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTracks)).EndInit();
            this.pnlDivider.ResumeLayout(false);
            this.flpToolbarRight.ResumeLayout(false);
            this.flpToolbarRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCollection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExcludedCollection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrackRankFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbQueued)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbShufflerFilter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip mnuGenre;
        private System.Windows.Forms.ToolStripMenuItem mnuRenameGenre;
        private System.Windows.Forms.ContextMenuStrip mnuArtist;
        private System.Windows.Forms.ToolStripMenuItem mnuRenameArtist;
        private System.Windows.Forms.ContextMenuStrip mnuAlbum;
        private System.Windows.Forms.ToolStripMenuItem mnuRenameAlbum;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateAlbumArtist;
        private System.Windows.Forms.ImageList imlAlbumArt;
        private System.Windows.Forms.ContextMenuStrip mnuTrack;
        private System.Windows.Forms.ToolStripMenuItem mnuPlay;
        private System.Windows.Forms.ToolStripMenuItem mnuQueue;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuAddTrackToCollection;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveTrackFromCollection;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenFileLocation;
        private System.Windows.Forms.ToolStripSeparator sepOpenFileLocation;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateTrackDetails;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateArtist;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateAlbum;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateGenre;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateShufflerDetails;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveShufflerDetails;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mnuAddToSampler;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mnuRank;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private Halloumi.Common.Windows.Controls.Panel pnlBackground2;
        private System.Windows.Forms.SplitContainer splLibraryMixable;
        private MixableTracks mixableTracks;
        private Halloumi.Common.Windows.Controls.Panel pnlLibraryDetails;
        private ComponentFactory.Krypton.Toolkit.KryptonSplitContainer splLibrary;
        private ComponentFactory.Krypton.Toolkit.KryptonSplitContainer splLeftRight;
        private ComponentFactory.Krypton.Toolkit.KryptonSplitContainer splLeftMiddle;
        private Halloumi.Common.Windows.Controls.Panel pnlGenre;
        private DataGridView grdGenre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenre;
        private Halloumi.Common.Windows.Controls.Panel pnlArtist;
        private DataGridView grdArtist;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArtist;
        private Halloumi.Common.Windows.Controls.Panel pnlAlbum;
        private Halloumi.Common.Windows.Controls.ListView lstAlbum;
        private System.Windows.Forms.ColumnHeader colAlbumName;
        private System.Windows.Forms.ColumnHeader colAlbumAlbumArtist;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge linAlbumHeader;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel pnlAlbumHeader;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAlbumHeader;
        private Halloumi.Common.Windows.Controls.Panel pnlTrack;
        private DataGridView grdTracks;
        private TrackDetails trackDetails;
        private Halloumi.Common.Windows.Controls.Panel pnlDivider;
        private System.Windows.Forms.FlowLayoutPanel flpToolbarRight;
        private Halloumi.Common.Windows.Controls.Label lblFilter;
        private Halloumi.Common.Windows.Controls.TextBox txtSearch;
        private Halloumi.Common.Windows.Controls.Label label5;
        private Halloumi.Common.Windows.Controls.TextBox txtMinBPM;
        private Halloumi.Common.Windows.Controls.Label label4;
        private Halloumi.Common.Windows.Controls.TextBox txtMaxBPM;
        private Halloumi.Common.Windows.Controls.Label label1;
        private Halloumi.Common.Windows.Controls.ComboBox cmbCollection;
        private Halloumi.Common.Windows.Controls.Label label7;
        private Halloumi.Common.Windows.Controls.ComboBox cmbExcludedCollection;
        private Halloumi.Common.Windows.Controls.Label label6;
        private Halloumi.Common.Windows.Controls.ComboBox cmbTrackRankFilter;
        private Halloumi.Common.Windows.Controls.Label label2;
        private Halloumi.Common.Windows.Controls.ComboBox cmbQueued;
        private Halloumi.Common.Windows.Controls.Label label3;
        private Halloumi.Common.Windows.Controls.ComboBox cmbShufflerFilter;
        private System.Windows.Forms.ToolStripMenuItem mnuCalculateKey;
        private System.Windows.Forms.ToolStripMenuItem mnuReloadMetadata;
        private System.Windows.Forms.ToolStripMenuItem mnuEditSamples;
        private System.Windows.Forms.ToolStripMenuItem mnuExportMixSectionsAsSamples;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackAlbum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackGenre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackStartBPM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackEndBPM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnrankedCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBitrate;
    }
}
