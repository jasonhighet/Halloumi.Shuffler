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
            components = new System.ComponentModel.Container();
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
            mnuGenre = new System.Windows.Forms.ContextMenuStrip(components);
            mnuRenameGenre = new System.Windows.Forms.ToolStripMenuItem();
            mnuArtist = new System.Windows.Forms.ContextMenuStrip(components);
            mnuRenameArtist = new System.Windows.Forms.ToolStripMenuItem();
            mnuAlbum = new System.Windows.Forms.ContextMenuStrip(components);
            mnuRenameAlbum = new System.Windows.Forms.ToolStripMenuItem();
            mnuUpdateAlbumArtist = new System.Windows.Forms.ToolStripMenuItem();
            imlAlbumArt = new System.Windows.Forms.ImageList(components);
            mnuTrack = new System.Windows.Forms.ContextMenuStrip(components);
            mnuPlay = new System.Windows.Forms.ToolStripMenuItem();
            mnuQueue = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            mnuAddTrackToCollection = new System.Windows.Forms.ToolStripMenuItem();
            mnuRemoveTrackFromCollection = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            mnuOpenFileLocation = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            mnuUpdateTrackDetails = new System.Windows.Forms.ToolStripMenuItem();
            mnuUpdateArtist = new System.Windows.Forms.ToolStripMenuItem();
            mnuUpdateAlbum = new System.Windows.Forms.ToolStripMenuItem();
            mnuUpdateGenre = new System.Windows.Forms.ToolStripMenuItem();
            mnuUpdateAudio = new System.Windows.Forms.ToolStripMenuItem();
            mnuUpdateShufflerDetails = new System.Windows.Forms.ToolStripMenuItem();
            mnuRemoveShufflerDetails = new System.Windows.Forms.ToolStripMenuItem();
            mnuReloadMetadata = new System.Windows.Forms.ToolStripMenuItem();
            mnuCalculateKey = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            mnuRank = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            sepOpenFileLocation = new System.Windows.Forms.ToolStripSeparator();
            mnuAddToSampler = new System.Windows.Forms.ToolStripMenuItem();
            mnuEditSamples = new System.Windows.Forms.ToolStripMenuItem();
            mnuExportMixSectionsAsSamples = new System.Windows.Forms.ToolStripMenuItem();
            backgroundWorker = new System.ComponentModel.BackgroundWorker();
            pnlBackground2 = new Halloumi.Common.Windows.Controls.Panel();
            splLibraryMixable = new System.Windows.Forms.SplitContainer();
            pnlLibraryDetails = new Halloumi.Common.Windows.Controls.Panel();
            splLibrary = new ComponentFactory.Krypton.Toolkit.KryptonSplitContainer();
            splLeftRight = new ComponentFactory.Krypton.Toolkit.KryptonSplitContainer();
            splLeftMiddle = new ComponentFactory.Krypton.Toolkit.KryptonSplitContainer();
            pnlGenre = new Halloumi.Common.Windows.Controls.Panel();
            grdGenre = new DataGridView();
            colGenre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            pnlArtist = new Halloumi.Common.Windows.Controls.Panel();
            grdArtist = new DataGridView();
            colArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            pnlAlbum = new Halloumi.Common.Windows.Controls.Panel();
            lstAlbum = new Halloumi.Common.Windows.Controls.ListView();
            colAlbumName = new System.Windows.Forms.ColumnHeader();
            colAlbumAlbumArtist = new System.Windows.Forms.ColumnHeader();
            linAlbumHeader = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            pnlAlbumHeader = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            lblAlbumHeader = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            pnlTrack = new Halloumi.Common.Windows.Controls.Panel();
            grdTracks = new DataGridView();
            colTrackDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTrackAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTrackGenre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTrackLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTrackStartBPM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTrackEndBPM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTrackNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colInCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colOutCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colUnrankedCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTrackRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTrackKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBitrate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            trackDetails = new TrackDetails();
            pnlDivider = new Halloumi.Common.Windows.Controls.Panel();
            flpToolbarRight = new System.Windows.Forms.FlowLayoutPanel();
            lblFilter = new Halloumi.Common.Windows.Controls.Label();
            txtSearch = new Halloumi.Common.Windows.Controls.TextBox();
            label5 = new Halloumi.Common.Windows.Controls.Label();
            txtMinBPM = new Halloumi.Common.Windows.Controls.TextBox();
            label4 = new Halloumi.Common.Windows.Controls.Label();
            txtMaxBPM = new Halloumi.Common.Windows.Controls.TextBox();
            label1 = new Halloumi.Common.Windows.Controls.Label();
            cmbCollection = new Halloumi.Common.Windows.Controls.ComboBox();
            label7 = new Halloumi.Common.Windows.Controls.Label();
            cmbExcludedCollection = new Halloumi.Common.Windows.Controls.ComboBox();
            label6 = new Halloumi.Common.Windows.Controls.Label();
            cmbTrackRankFilter = new Halloumi.Common.Windows.Controls.ComboBox();
            label2 = new Halloumi.Common.Windows.Controls.Label();
            cmbQueued = new Halloumi.Common.Windows.Controls.ComboBox();
            label3 = new Halloumi.Common.Windows.Controls.Label();
            cmbShufflerFilter = new Halloumi.Common.Windows.Controls.ComboBox();
            mixableTracks = new MixableTracks();
            btnUnshuffledRoulette = new Halloumi.Common.Windows.Controls.Button();
            mnuGenre.SuspendLayout();
            mnuArtist.SuspendLayout();
            mnuAlbum.SuspendLayout();
            mnuTrack.SuspendLayout();
            pnlBackground2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splLibraryMixable).BeginInit();
            splLibraryMixable.Panel1.SuspendLayout();
            splLibraryMixable.Panel2.SuspendLayout();
            splLibraryMixable.SuspendLayout();
            pnlLibraryDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splLibrary).BeginInit();
            (splLibrary.Panel1).BeginInit();
            splLibrary.Panel1.SuspendLayout();
            (splLibrary.Panel2).BeginInit();
            splLibrary.Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splLeftRight).BeginInit();
            (splLeftRight.Panel1).BeginInit();
            splLeftRight.Panel1.SuspendLayout();
            (splLeftRight.Panel2).BeginInit();
            splLeftRight.Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splLeftMiddle).BeginInit();
            (splLeftMiddle.Panel1).BeginInit();
            splLeftMiddle.Panel1.SuspendLayout();
            (splLeftMiddle.Panel2).BeginInit();
            splLeftMiddle.Panel2.SuspendLayout();
            pnlGenre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdGenre).BeginInit();
            pnlArtist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdArtist).BeginInit();
            pnlAlbum.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlAlbumHeader).BeginInit();
            pnlAlbumHeader.SuspendLayout();
            pnlTrack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdTracks).BeginInit();
            pnlDivider.SuspendLayout();
            flpToolbarRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cmbCollection).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cmbExcludedCollection).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cmbTrackRankFilter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cmbQueued).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cmbShufflerFilter).BeginInit();
            SuspendLayout();
            // 
            // mnuGenre
            // 
            mnuGenre.Font = new System.Drawing.Font("Segoe UI", 9F);
            mnuGenre.ImageScalingSize = new System.Drawing.Size(20, 20);
            mnuGenre.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuRenameGenre });
            mnuGenre.Name = "mnuGenre";
            mnuGenre.Size = new System.Drawing.Size(238, 38);
            // 
            // mnuRenameGenre
            // 
            mnuRenameGenre.Name = "mnuRenameGenre";
            mnuRenameGenre.Size = new System.Drawing.Size(237, 34);
            mnuRenameGenre.Text = "&Rename Genre...";
            // 
            // mnuArtist
            // 
            mnuArtist.Font = new System.Drawing.Font("Segoe UI", 9F);
            mnuArtist.ImageScalingSize = new System.Drawing.Size(20, 20);
            mnuArtist.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuRenameArtist });
            mnuArtist.Name = "mnuGenre";
            mnuArtist.Size = new System.Drawing.Size(232, 38);
            // 
            // mnuRenameArtist
            // 
            mnuRenameArtist.Name = "mnuRenameArtist";
            mnuRenameArtist.Size = new System.Drawing.Size(231, 34);
            mnuRenameArtist.Text = "&Rename Artist...";
            // 
            // mnuAlbum
            // 
            mnuAlbum.Font = new System.Drawing.Font("Segoe UI", 9F);
            mnuAlbum.ImageScalingSize = new System.Drawing.Size(20, 20);
            mnuAlbum.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuRenameAlbum, mnuUpdateAlbumArtist });
            mnuAlbum.Name = "mnuGenre";
            mnuAlbum.Size = new System.Drawing.Size(291, 72);
            // 
            // mnuRenameAlbum
            // 
            mnuRenameAlbum.Name = "mnuRenameAlbum";
            mnuRenameAlbum.Size = new System.Drawing.Size(290, 34);
            mnuRenameAlbum.Text = "&Rename Album...";
            // 
            // mnuUpdateAlbumArtist
            // 
            mnuUpdateAlbumArtist.Name = "mnuUpdateAlbumArtist";
            mnuUpdateAlbumArtist.Size = new System.Drawing.Size(290, 34);
            mnuUpdateAlbumArtist.Text = "Update Album A&rtist...";
            // 
            // imlAlbumArt
            // 
            imlAlbumArt.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            imlAlbumArt.ImageSize = new System.Drawing.Size(75, 75);
            imlAlbumArt.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // mnuTrack
            // 
            mnuTrack.Font = new System.Drawing.Font("Segoe UI", 9F);
            mnuTrack.ImageScalingSize = new System.Drawing.Size(20, 20);
            mnuTrack.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuPlay, mnuQueue, toolStripSeparator2, mnuAddTrackToCollection, mnuRemoveTrackFromCollection, toolStripSeparator1, mnuOpenFileLocation, toolStripSeparator3, mnuUpdateTrackDetails, mnuUpdateArtist, mnuUpdateAlbum, mnuUpdateGenre, mnuUpdateAudio, mnuUpdateShufflerDetails, mnuRemoveShufflerDetails, mnuReloadMetadata, mnuCalculateKey, toolStripSeparator4, mnuRank, sepOpenFileLocation, mnuAddToSampler, mnuEditSamples, mnuExportMixSectionsAsSamples });
            mnuTrack.Name = "mnuTrack";
            mnuTrack.Size = new System.Drawing.Size(381, 646);
            // 
            // mnuPlay
            // 
            mnuPlay.Name = "mnuPlay";
            mnuPlay.Size = new System.Drawing.Size(380, 34);
            mnuPlay.Text = "&Play";
            // 
            // mnuQueue
            // 
            mnuQueue.Name = "mnuQueue";
            mnuQueue.Size = new System.Drawing.Size(380, 34);
            mnuQueue.Text = "&Queue";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(377, 6);
            // 
            // mnuAddTrackToCollection
            // 
            mnuAddTrackToCollection.Name = "mnuAddTrackToCollection";
            mnuAddTrackToCollection.Size = new System.Drawing.Size(380, 34);
            mnuAddTrackToCollection.Text = "A&dd To Collection";
            // 
            // mnuRemoveTrackFromCollection
            // 
            mnuRemoveTrackFromCollection.Name = "mnuRemoveTrackFromCollection";
            mnuRemoveTrackFromCollection.Size = new System.Drawing.Size(380, 34);
            mnuRemoveTrackFromCollection.Text = "&Remove From Collection";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(377, 6);
            // 
            // mnuOpenFileLocation
            // 
            mnuOpenFileLocation.Name = "mnuOpenFileLocation";
            mnuOpenFileLocation.Size = new System.Drawing.Size(380, 34);
            mnuOpenFileLocation.Text = "Open File &Location";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(377, 6);
            // 
            // mnuUpdateTrackDetails
            // 
            mnuUpdateTrackDetails.Name = "mnuUpdateTrackDetails";
            mnuUpdateTrackDetails.Size = new System.Drawing.Size(380, 34);
            mnuUpdateTrackDetails.Text = "Update &Track Details";
            // 
            // mnuUpdateArtist
            // 
            mnuUpdateArtist.Name = "mnuUpdateArtist";
            mnuUpdateArtist.Size = new System.Drawing.Size(380, 34);
            mnuUpdateArtist.Text = "Update A&rtist...";
            // 
            // mnuUpdateAlbum
            // 
            mnuUpdateAlbum.Name = "mnuUpdateAlbum";
            mnuUpdateAlbum.Size = new System.Drawing.Size(380, 34);
            mnuUpdateAlbum.Text = "Update &Album...";
            // 
            // mnuUpdateGenre
            // 
            mnuUpdateGenre.Name = "mnuUpdateGenre";
            mnuUpdateGenre.Size = new System.Drawing.Size(380, 34);
            mnuUpdateGenre.Text = "Update &Genre...";
            // 
            // mnuUpdateAudio
            // 
            mnuUpdateAudio.Name = "mnuUpdateAudio";
            mnuUpdateAudio.Size = new System.Drawing.Size(380, 34);
            mnuUpdateAudio.Text = "Update Audio";
            // 
            // mnuUpdateShufflerDetails
            // 
            mnuUpdateShufflerDetails.Name = "mnuUpdateShufflerDetails";
            mnuUpdateShufflerDetails.Size = new System.Drawing.Size(380, 34);
            mnuUpdateShufflerDetails.Text = "Update &Shuffler Details...";
            // 
            // mnuRemoveShufflerDetails
            // 
            mnuRemoveShufflerDetails.Name = "mnuRemoveShufflerDetails";
            mnuRemoveShufflerDetails.Size = new System.Drawing.Size(380, 34);
            mnuRemoveShufflerDetails.Text = "&Remove Shuffer Details";
            // 
            // mnuReloadMetadata
            // 
            mnuReloadMetadata.Name = "mnuReloadMetadata";
            mnuReloadMetadata.Size = new System.Drawing.Size(380, 34);
            mnuReloadMetadata.Text = "Reload &Metadata";
            mnuReloadMetadata.Click += mnuReloadMetadata_Click;
            // 
            // mnuCalculateKey
            // 
            mnuCalculateKey.Name = "mnuCalculateKey";
            mnuCalculateKey.Size = new System.Drawing.Size(380, 34);
            mnuCalculateKey.Text = "Calculate &Key";
            mnuCalculateKey.Click += mnuCalculateKey_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(377, 6);
            // 
            // mnuRank
            // 
            mnuRank.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4, toolStripMenuItem5, toolStripMenuItem6 });
            mnuRank.Name = "mnuRank";
            mnuRank.Size = new System.Drawing.Size(380, 34);
            mnuRank.Text = "Track Rating";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(270, 34);
            toolStripMenuItem1.Text = "toolStripMenuItem1";
            toolStripMenuItem1.Click += mnuRank_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(270, 34);
            toolStripMenuItem2.Text = "toolStripMenuItem2";
            toolStripMenuItem2.Click += mnuRank_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(270, 34);
            toolStripMenuItem3.Text = "toolStripMenuItem3";
            toolStripMenuItem3.Click += mnuRank_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new System.Drawing.Size(270, 34);
            toolStripMenuItem4.Text = "toolStripMenuItem4";
            toolStripMenuItem4.Click += mnuRank_Click;
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new System.Drawing.Size(270, 34);
            toolStripMenuItem5.Text = "toolStripMenuItem5";
            toolStripMenuItem5.Click += mnuRank_Click;
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new System.Drawing.Size(270, 34);
            toolStripMenuItem6.Text = "toolStripMenuItem6";
            toolStripMenuItem6.Click += mnuRank_Click;
            // 
            // sepOpenFileLocation
            // 
            sepOpenFileLocation.Name = "sepOpenFileLocation";
            sepOpenFileLocation.Size = new System.Drawing.Size(377, 6);
            // 
            // mnuAddToSampler
            // 
            mnuAddToSampler.Name = "mnuAddToSampler";
            mnuAddToSampler.Size = new System.Drawing.Size(380, 34);
            // 
            // mnuEditSamples
            // 
            mnuEditSamples.Name = "mnuEditSamples";
            mnuEditSamples.Size = new System.Drawing.Size(380, 34);
            mnuEditSamples.Text = "Edit Samples";
            mnuEditSamples.Click += mnuEditSamples_Click;
            // 
            // mnuExportMixSectionsAsSamples
            // 
            mnuExportMixSectionsAsSamples.Name = "mnuExportMixSectionsAsSamples";
            mnuExportMixSectionsAsSamples.Size = new System.Drawing.Size(380, 34);
            mnuExportMixSectionsAsSamples.Text = "Export Mi&x Sections As Samples";
            mnuExportMixSectionsAsSamples.Click += mnuExportMixSectionsAsSamples_Click;
            // 
            // pnlBackground2
            // 
            pnlBackground2.BackColor = System.Drawing.SystemColors.Control;
            pnlBackground2.Controls.Add(splLibraryMixable);
            pnlBackground2.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBackground2.Location = new System.Drawing.Point(0, 0);
            pnlBackground2.Margin = new System.Windows.Forms.Padding(4);
            pnlBackground2.Name = "pnlBackground2";
            pnlBackground2.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            pnlBackground2.Size = new System.Drawing.Size(2199, 1090);
            pnlBackground2.TabIndex = 12;
            // 
            // splLibraryMixable
            // 
            splLibraryMixable.Dock = System.Windows.Forms.DockStyle.Fill;
            splLibraryMixable.Location = new System.Drawing.Point(7, 8);
            splLibraryMixable.Margin = new System.Windows.Forms.Padding(4);
            splLibraryMixable.Name = "splLibraryMixable";
            splLibraryMixable.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splLibraryMixable.Panel1
            // 
            splLibraryMixable.Panel1.Controls.Add(pnlLibraryDetails);
            // 
            // splLibraryMixable.Panel2
            // 
            splLibraryMixable.Panel2.Controls.Add(mixableTracks);
            splLibraryMixable.Size = new System.Drawing.Size(2185, 1074);
            splLibraryMixable.SplitterDistance = 822;
            splLibraryMixable.SplitterWidth = 6;
            splLibraryMixable.TabIndex = 0;
            // 
            // pnlLibraryDetails
            // 
            pnlLibraryDetails.BackColor = System.Drawing.SystemColors.Control;
            pnlLibraryDetails.Controls.Add(splLibrary);
            pnlLibraryDetails.Controls.Add(trackDetails);
            pnlLibraryDetails.Controls.Add(pnlDivider);
            pnlLibraryDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlLibraryDetails.Location = new System.Drawing.Point(0, 0);
            pnlLibraryDetails.Margin = new System.Windows.Forms.Padding(6);
            pnlLibraryDetails.Name = "pnlLibraryDetails";
            pnlLibraryDetails.Size = new System.Drawing.Size(2185, 822);
            pnlLibraryDetails.Style = Common.Windows.Controls.PanelStyle.Content;
            pnlLibraryDetails.TabIndex = 11;
            // 
            // splLibrary
            // 
            splLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            splLibrary.Location = new System.Drawing.Point(0, 62);
            splLibrary.Margin = new System.Windows.Forms.Padding(6);
            splLibrary.Name = "splLibrary";
            splLibrary.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // 
            // 
            splLibrary.Panel1.Controls.Add(splLeftRight);
            splLibrary.Panel1.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            // 
            // 
            // 
            splLibrary.Panel2.Controls.Add(pnlTrack);
            splLibrary.Panel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            splLibrary.Size = new System.Drawing.Size(2185, 650);
            splLibrary.SplitterDistance = 275;
            splLibrary.TabIndex = 55;
            // 
            // splLeftRight
            // 
            splLeftRight.Dock = System.Windows.Forms.DockStyle.Fill;
            splLeftRight.Location = new System.Drawing.Point(0, 9);
            splLeftRight.Margin = new System.Windows.Forms.Padding(6);
            splLeftRight.Name = "splLeftRight";
            // 
            // 
            // 
            splLeftRight.Panel1.Controls.Add(splLeftMiddle);
            // 
            // 
            // 
            splLeftRight.Panel2.Controls.Add(pnlAlbum);
            splLeftRight.Size = new System.Drawing.Size(2185, 266);
            splLeftRight.SplitterDistance = 841;
            splLeftRight.TabIndex = 0;
            // 
            // splLeftMiddle
            // 
            splLeftMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            splLeftMiddle.Location = new System.Drawing.Point(0, 0);
            splLeftMiddle.Margin = new System.Windows.Forms.Padding(6);
            splLeftMiddle.Name = "splLeftMiddle";
            // 
            // 
            // 
            splLeftMiddle.Panel1.Controls.Add(pnlGenre);
            // 
            // 
            // 
            splLeftMiddle.Panel2.Controls.Add(pnlArtist);
            splLeftMiddle.Size = new System.Drawing.Size(841, 266);
            splLeftMiddle.SplitterDistance = 386;
            splLeftMiddle.TabIndex = 0;
            // 
            // pnlGenre
            // 
            pnlGenre.BackColor = System.Drawing.SystemColors.Control;
            pnlGenre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlGenre.Controls.Add(grdGenre);
            pnlGenre.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlGenre.Location = new System.Drawing.Point(0, 0);
            pnlGenre.Margin = new System.Windows.Forms.Padding(6);
            pnlGenre.Name = "pnlGenre";
            pnlGenre.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            pnlGenre.Size = new System.Drawing.Size(386, 266);
            pnlGenre.TabIndex = 2;
            // 
            // grdGenre
            // 
            grdGenre.AllowUserToAddRows = false;
            grdGenre.AllowUserToDeleteRows = false;
            grdGenre.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            grdGenre.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            grdGenre.ColumnHeadersHeight = 44;
            grdGenre.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colGenre });
            grdGenre.ContextMenuStrip = mnuGenre;
            grdGenre.Dock = System.Windows.Forms.DockStyle.Fill;
            grdGenre.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            grdGenre.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellSheet;
            grdGenre.HideOuterBorders = true;
            grdGenre.Location = new System.Drawing.Point(1, 2);
            grdGenre.Margin = new System.Windows.Forms.Padding(6);
            grdGenre.MergeColor = System.Drawing.Color.Gainsboro;
            grdGenre.Name = "grdGenre";
            grdGenre.ReadOnly = true;
            grdGenre.RowHeadersVisible = false;
            grdGenre.RowHeadersWidth = 72;
            grdGenre.RowTemplate.Height = 44;
            grdGenre.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            grdGenre.Size = new System.Drawing.Size(384, 262);
            grdGenre.SortColumnIndex = 0;
            grdGenre.SortOrder = System.Windows.Forms.SortOrder.None;
            grdGenre.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellSheet;
            grdGenre.StateCommon.DataCell.Border.Color1 = System.Drawing.Color.White;
            grdGenre.StateCommon.DataCell.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right;
            grdGenre.TabIndex = 0;
            // 
            // colGenre
            // 
            colGenre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colGenre.DataPropertyName = "Name";
            colGenre.HeaderText = "Genre";
            colGenre.MinimumWidth = 9;
            colGenre.Name = "colGenre";
            colGenre.ReadOnly = true;
            // 
            // pnlArtist
            // 
            pnlArtist.BackColor = System.Drawing.SystemColors.Control;
            pnlArtist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlArtist.Controls.Add(grdArtist);
            pnlArtist.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlArtist.Location = new System.Drawing.Point(0, 0);
            pnlArtist.Margin = new System.Windows.Forms.Padding(6);
            pnlArtist.Name = "pnlArtist";
            pnlArtist.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            pnlArtist.Size = new System.Drawing.Size(450, 266);
            pnlArtist.Style = Common.Windows.Controls.PanelStyle.Content;
            pnlArtist.TabIndex = 3;
            // 
            // grdArtist
            // 
            grdArtist.AllowUserToAddRows = false;
            grdArtist.AllowUserToDeleteRows = false;
            grdArtist.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.WhiteSmoke;
            grdArtist.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            grdArtist.ColumnHeadersHeight = 44;
            grdArtist.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colArtist });
            grdArtist.ContextMenuStrip = mnuArtist;
            grdArtist.Dock = System.Windows.Forms.DockStyle.Fill;
            grdArtist.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            grdArtist.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellCustom1;
            grdArtist.HideOuterBorders = true;
            grdArtist.Location = new System.Drawing.Point(1, 2);
            grdArtist.Margin = new System.Windows.Forms.Padding(6);
            grdArtist.MergeColor = System.Drawing.Color.Gainsboro;
            grdArtist.Name = "grdArtist";
            grdArtist.ReadOnly = true;
            grdArtist.RowHeadersVisible = false;
            grdArtist.RowHeadersWidth = 72;
            grdArtist.RowTemplate.Height = 44;
            grdArtist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            grdArtist.Size = new System.Drawing.Size(448, 262);
            grdArtist.SortColumnIndex = 0;
            grdArtist.SortOrder = System.Windows.Forms.SortOrder.None;
            grdArtist.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellCustom1;
            grdArtist.StateCommon.DataCell.Border.Color1 = System.Drawing.Color.White;
            grdArtist.StateCommon.DataCell.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right;
            grdArtist.TabIndex = 0;
            // 
            // colArtist
            // 
            colArtist.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colArtist.DataPropertyName = "Name";
            colArtist.HeaderText = "Artist";
            colArtist.MinimumWidth = 9;
            colArtist.Name = "colArtist";
            colArtist.ReadOnly = true;
            // 
            // pnlAlbum
            // 
            pnlAlbum.BackColor = System.Drawing.SystemColors.Control;
            pnlAlbum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlAlbum.Controls.Add(lstAlbum);
            pnlAlbum.Controls.Add(linAlbumHeader);
            pnlAlbum.Controls.Add(pnlAlbumHeader);
            pnlAlbum.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlAlbum.Location = new System.Drawing.Point(0, 0);
            pnlAlbum.Margin = new System.Windows.Forms.Padding(6);
            pnlAlbum.Name = "pnlAlbum";
            pnlAlbum.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            pnlAlbum.Size = new System.Drawing.Size(1339, 266);
            pnlAlbum.Style = Common.Windows.Controls.PanelStyle.Content;
            pnlAlbum.TabIndex = 4;
            // 
            // lstAlbum
            // 
            lstAlbum.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lstAlbum.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { colAlbumName, colAlbumAlbumArtist });
            lstAlbum.ContextMenuStrip = mnuAlbum;
            lstAlbum.Dock = System.Windows.Forms.DockStyle.Fill;
            lstAlbum.Font = new System.Drawing.Font("Segoe UI", 9F);
            lstAlbum.ForeColor = System.Drawing.SystemColors.WindowText;
            lstAlbum.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            lstAlbum.HideSelection = false;
            lstAlbum.LargeImageList = imlAlbumArt;
            lstAlbum.Location = new System.Drawing.Point(1, 43);
            lstAlbum.Margin = new System.Windows.Forms.Padding(6);
            lstAlbum.Name = "lstAlbum";
            lstAlbum.Size = new System.Drawing.Size(1337, 221);
            lstAlbum.TabIndex = 3;
            lstAlbum.UseCompatibleStateImageBehavior = false;
            // 
            // colAlbumName
            // 
            colAlbumName.DisplayIndex = 1;
            colAlbumName.Width = 679;
            // 
            // colAlbumAlbumArtist
            // 
            colAlbumAlbumArtist.DisplayIndex = 0;
            colAlbumAlbumArtist.Width = 0;
            // 
            // linAlbumHeader
            // 
            linAlbumHeader.Dock = System.Windows.Forms.DockStyle.Top;
            linAlbumHeader.Location = new System.Drawing.Point(1, 42);
            linAlbumHeader.Margin = new System.Windows.Forms.Padding(6);
            linAlbumHeader.Name = "linAlbumHeader";
            linAlbumHeader.Size = new System.Drawing.Size(1337, 1);
            linAlbumHeader.Text = "kryptonBorderEdge1";
            // 
            // pnlAlbumHeader
            // 
            pnlAlbumHeader.Controls.Add(lblAlbumHeader);
            pnlAlbumHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlAlbumHeader.Location = new System.Drawing.Point(1, 2);
            pnlAlbumHeader.Margin = new System.Windows.Forms.Padding(6);
            pnlAlbumHeader.Name = "pnlAlbumHeader";
            pnlAlbumHeader.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderColumnCustom1;
            pnlAlbumHeader.Size = new System.Drawing.Size(1337, 40);
            pnlAlbumHeader.TabIndex = 1;
            // 
            // lblAlbumHeader
            // 
            lblAlbumHeader.Dock = System.Windows.Forms.DockStyle.Left;
            lblAlbumHeader.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            lblAlbumHeader.Location = new System.Drawing.Point(0, 0);
            lblAlbumHeader.Margin = new System.Windows.Forms.Padding(6);
            lblAlbumHeader.Name = "lblAlbumHeader";
            lblAlbumHeader.Size = new System.Drawing.Size(76, 40);
            lblAlbumHeader.TabIndex = 0;
            lblAlbumHeader.Values.Text = "Album";
            // 
            // pnlTrack
            // 
            pnlTrack.BackColor = System.Drawing.SystemColors.Control;
            pnlTrack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlTrack.Controls.Add(grdTracks);
            pnlTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlTrack.Location = new System.Drawing.Point(0, 0);
            pnlTrack.Margin = new System.Windows.Forms.Padding(6);
            pnlTrack.Name = "pnlTrack";
            pnlTrack.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            pnlTrack.Size = new System.Drawing.Size(2185, 362);
            pnlTrack.Style = Common.Windows.Controls.PanelStyle.Content;
            pnlTrack.TabIndex = 5;
            // 
            // grdTracks
            // 
            grdTracks.AllowUserToAddRows = false;
            grdTracks.AllowUserToDeleteRows = false;
            grdTracks.AllowUserToResizeColumns = false;
            grdTracks.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.WhiteSmoke;
            grdTracks.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            grdTracks.ColumnHeadersHeight = 44;
            grdTracks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colTrackDescription, colTrackAlbum, colTrackGenre, colTrackLength, colTrackStartBPM, colTrackEndBPM, colTrackNumber, colInCount, colOutCount, colUnrankedCount, colTrackRank, colTrackKey, colBitrate });
            grdTracks.ContextMenuStrip = mnuTrack;
            grdTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            grdTracks.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            grdTracks.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellList;
            grdTracks.HideOuterBorders = true;
            grdTracks.Location = new System.Drawing.Point(1, 2);
            grdTracks.Margin = new System.Windows.Forms.Padding(6);
            grdTracks.MergeColor = System.Drawing.Color.Gainsboro;
            grdTracks.Name = "grdTracks";
            grdTracks.ReadOnly = true;
            grdTracks.RowHeadersVisible = false;
            grdTracks.RowHeadersWidth = 72;
            grdTracks.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            grdTracks.RowTemplate.Height = 44;
            grdTracks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            grdTracks.Size = new System.Drawing.Size(2183, 358);
            grdTracks.SortColumnIndex = 0;
            grdTracks.SortOrder = System.Windows.Forms.SortOrder.None;
            grdTracks.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellList;
            grdTracks.StateCommon.DataCell.Border.Color1 = System.Drawing.Color.White;
            grdTracks.StateCommon.DataCell.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right;
            grdTracks.TabIndex = 53;
            // 
            // colTrackDescription
            // 
            colTrackDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colTrackDescription.DataPropertyName = "Description";
            colTrackDescription.HeaderText = "Description";
            colTrackDescription.MinimumWidth = 9;
            colTrackDescription.Name = "colTrackDescription";
            colTrackDescription.ReadOnly = true;
            colTrackDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colTrackAlbum
            // 
            colTrackAlbum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colTrackAlbum.DataPropertyName = "Album";
            colTrackAlbum.FillWeight = 50F;
            colTrackAlbum.HeaderText = "Album";
            colTrackAlbum.MinimumWidth = 9;
            colTrackAlbum.Name = "colTrackAlbum";
            colTrackAlbum.ReadOnly = true;
            colTrackAlbum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colTrackGenre
            // 
            colTrackGenre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colTrackGenre.DataPropertyName = "Genre";
            colTrackGenre.HeaderText = "Genre";
            colTrackGenre.MinimumWidth = 9;
            colTrackGenre.Name = "colTrackGenre";
            colTrackGenre.ReadOnly = true;
            colTrackGenre.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            colTrackGenre.Width = 97;
            // 
            // colTrackLength
            // 
            colTrackLength.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colTrackLength.DataPropertyName = "LengthFormatted";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            colTrackLength.DefaultCellStyle = dataGridViewCellStyle4;
            colTrackLength.HeaderText = "Length";
            colTrackLength.MinimumWidth = 9;
            colTrackLength.Name = "colTrackLength";
            colTrackLength.ReadOnly = true;
            colTrackLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            colTrackLength.Width = 106;
            // 
            // colTrackStartBPM
            // 
            colTrackStartBPM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colTrackStartBPM.DataPropertyName = "StartBPM";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            colTrackStartBPM.DefaultCellStyle = dataGridViewCellStyle5;
            colTrackStartBPM.HeaderText = "Start BPM";
            colTrackStartBPM.MinimumWidth = 9;
            colTrackStartBPM.Name = "colTrackStartBPM";
            colTrackStartBPM.ReadOnly = true;
            colTrackStartBPM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            colTrackStartBPM.Width = 133;
            // 
            // colTrackEndBPM
            // 
            colTrackEndBPM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colTrackEndBPM.DataPropertyName = "EndBPM";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = null;
            colTrackEndBPM.DefaultCellStyle = dataGridViewCellStyle6;
            colTrackEndBPM.HeaderText = "End BPM";
            colTrackEndBPM.MinimumWidth = 9;
            colTrackEndBPM.Name = "colTrackEndBPM";
            colTrackEndBPM.ReadOnly = true;
            colTrackEndBPM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            colTrackEndBPM.Width = 126;
            // 
            // colTrackNumber
            // 
            colTrackNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colTrackNumber.DataPropertyName = "TrackNumberFormatted";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            colTrackNumber.DefaultCellStyle = dataGridViewCellStyle7;
            colTrackNumber.HeaderText = "#";
            colTrackNumber.MinimumWidth = 9;
            colTrackNumber.Name = "colTrackNumber";
            colTrackNumber.ReadOnly = true;
            colTrackNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            colTrackNumber.Width = 54;
            // 
            // colInCount
            // 
            colInCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colInCount.DataPropertyName = "InCount";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            colInCount.DefaultCellStyle = dataGridViewCellStyle8;
            colInCount.HeaderText = "In";
            colInCount.MinimumWidth = 9;
            colInCount.Name = "colInCount";
            colInCount.ReadOnly = true;
            colInCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            colInCount.Width = 60;
            // 
            // colOutCount
            // 
            colOutCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colOutCount.DataPropertyName = "OutCount";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            colOutCount.DefaultCellStyle = dataGridViewCellStyle9;
            colOutCount.HeaderText = "Out";
            colOutCount.MinimumWidth = 9;
            colOutCount.Name = "colOutCount";
            colOutCount.ReadOnly = true;
            colOutCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            colOutCount.Width = 77;
            // 
            // colUnrankedCount
            // 
            colUnrankedCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colUnrankedCount.DataPropertyName = "UnrankedCount";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            colUnrankedCount.DefaultCellStyle = dataGridViewCellStyle10;
            colUnrankedCount.HeaderText = "No";
            colUnrankedCount.MinimumWidth = 9;
            colUnrankedCount.Name = "colUnrankedCount";
            colUnrankedCount.ReadOnly = true;
            colUnrankedCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            colUnrankedCount.Width = 70;
            // 
            // colTrackRank
            // 
            colTrackRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colTrackRank.DataPropertyName = "RankDescription";
            colTrackRank.HeaderText = "Rank";
            colTrackRank.MinimumWidth = 9;
            colTrackRank.Name = "colTrackRank";
            colTrackRank.ReadOnly = true;
            colTrackRank.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            colTrackRank.Width = 88;
            // 
            // colTrackKey
            // 
            colTrackKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colTrackKey.DataPropertyName = "Key";
            colTrackKey.HeaderText = "Key";
            colTrackKey.MinimumWidth = 9;
            colTrackKey.Name = "colTrackKey";
            colTrackKey.ReadOnly = true;
            colTrackKey.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            colTrackKey.Width = 75;
            // 
            // colBitrate
            // 
            colBitrate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colBitrate.DataPropertyName = "Bitrate";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            colBitrate.DefaultCellStyle = dataGridViewCellStyle11;
            colBitrate.HeaderText = "KPS";
            colBitrate.MinimumWidth = 9;
            colBitrate.Name = "colBitrate";
            colBitrate.ReadOnly = true;
            colBitrate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            colBitrate.Width = 77;
            // 
            // trackDetails
            // 
            trackDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            trackDetails.Location = new System.Drawing.Point(0, 712);
            trackDetails.Margin = new System.Windows.Forms.Padding(6);
            trackDetails.Name = "trackDetails";
            trackDetails.Size = new System.Drawing.Size(2185, 110);
            trackDetails.TabIndex = 54;
            // 
            // pnlDivider
            // 
            pnlDivider.BackColor = System.Drawing.SystemColors.Control;
            pnlDivider.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlDivider.Controls.Add(btnUnshuffledRoulette);
            pnlDivider.Controls.Add(flpToolbarRight);
            pnlDivider.Dock = System.Windows.Forms.DockStyle.Top;
            pnlDivider.Location = new System.Drawing.Point(0, 0);
            pnlDivider.Margin = new System.Windows.Forms.Padding(4);
            pnlDivider.Name = "pnlDivider";
            pnlDivider.Padding = new System.Windows.Forms.Padding(7, 4, 7, 8);
            pnlDivider.Size = new System.Drawing.Size(2185, 62);
            pnlDivider.Style = Common.Windows.Controls.PanelStyle.ButtonStrip;
            pnlDivider.TabIndex = 52;
            // 
            // flpToolbarRight
            // 
            flpToolbarRight.BackColor = System.Drawing.Color.Transparent;
            flpToolbarRight.Controls.Add(lblFilter);
            flpToolbarRight.Controls.Add(txtSearch);
            flpToolbarRight.Controls.Add(label5);
            flpToolbarRight.Controls.Add(txtMinBPM);
            flpToolbarRight.Controls.Add(label4);
            flpToolbarRight.Controls.Add(txtMaxBPM);
            flpToolbarRight.Controls.Add(label1);
            flpToolbarRight.Controls.Add(cmbCollection);
            flpToolbarRight.Controls.Add(label7);
            flpToolbarRight.Controls.Add(cmbExcludedCollection);
            flpToolbarRight.Controls.Add(label6);
            flpToolbarRight.Controls.Add(cmbTrackRankFilter);
            flpToolbarRight.Controls.Add(label2);
            flpToolbarRight.Controls.Add(cmbQueued);
            flpToolbarRight.Controls.Add(label3);
            flpToolbarRight.Controls.Add(cmbShufflerFilter);
            flpToolbarRight.Dock = System.Windows.Forms.DockStyle.Fill;
            flpToolbarRight.Location = new System.Drawing.Point(7, 4);
            flpToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            flpToolbarRight.Name = "flpToolbarRight";
            flpToolbarRight.Size = new System.Drawing.Size(2171, 50);
            flpToolbarRight.TabIndex = 0;
            // 
            // lblFilter
            // 
            lblFilter.AutoSize = true;
            lblFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblFilter.ForeColor = System.Drawing.SystemColors.ControlText;
            lblFilter.Location = new System.Drawing.Point(0, 0);
            lblFilter.Margin = new System.Windows.Forms.Padding(0);
            lblFilter.Name = "lblFilter";
            lblFilter.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            lblFilter.Size = new System.Drawing.Size(80, 39);
            lblFilter.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblFilter.TabIndex = 5;
            lblFilter.Text = "Search:";
            lblFilter.Click += lblFilter_Click;
            // 
            // txtSearch
            // 
            txtSearch.ErrorProvider = null;
            txtSearch.Location = new System.Drawing.Point(86, 6);
            txtSearch.Margin = new System.Windows.Forms.Padding(6);
            txtSearch.MaximumValue = 2147483647D;
            txtSearch.MinimumValue = -2147483648D;
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(160, 35);
            txtSearch.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label5.ForeColor = System.Drawing.SystemColors.ControlText;
            label5.Location = new System.Drawing.Point(252, 0);
            label5.Margin = new System.Windows.Forms.Padding(0);
            label5.Name = "label5";
            label5.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            label5.Size = new System.Drawing.Size(54, 39);
            label5.Style = Common.Windows.Controls.LabelStyle.Caption;
            label5.TabIndex = 13;
            label5.Text = "Min:";
            // 
            // txtMinBPM
            // 
            txtMinBPM.EntryType = Common.Windows.Controls.TextBox.TextEntryType.Integer;
            txtMinBPM.ErrorProvider = null;
            txtMinBPM.Location = new System.Drawing.Point(312, 6);
            txtMinBPM.Margin = new System.Windows.Forms.Padding(6);
            txtMinBPM.MaximumValue = 2147483647D;
            txtMinBPM.MinimumValue = -2147483648D;
            txtMinBPM.Name = "txtMinBPM";
            txtMinBPM.Size = new System.Drawing.Size(48, 35);
            txtMinBPM.TabIndex = 12;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label4.ForeColor = System.Drawing.SystemColors.ControlText;
            label4.Location = new System.Drawing.Point(366, 0);
            label4.Margin = new System.Windows.Forms.Padding(0);
            label4.Name = "label4";
            label4.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            label4.Size = new System.Drawing.Size(58, 39);
            label4.Style = Common.Windows.Controls.LabelStyle.Caption;
            label4.TabIndex = 11;
            label4.Text = "Max:";
            // 
            // txtMaxBPM
            // 
            txtMaxBPM.EntryType = Common.Windows.Controls.TextBox.TextEntryType.Integer;
            txtMaxBPM.ErrorProvider = null;
            txtMaxBPM.Location = new System.Drawing.Point(430, 6);
            txtMaxBPM.Margin = new System.Windows.Forms.Padding(6);
            txtMaxBPM.MaximumValue = 2147483647D;
            txtMaxBPM.MinimumValue = -2147483648D;
            txtMaxBPM.Name = "txtMaxBPM";
            txtMaxBPM.Size = new System.Drawing.Size(48, 35);
            txtMaxBPM.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label1.ForeColor = System.Drawing.SystemColors.ControlText;
            label1.Location = new System.Drawing.Point(484, 0);
            label1.Margin = new System.Windows.Forms.Padding(0);
            label1.Name = "label1";
            label1.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            label1.Size = new System.Drawing.Size(110, 39);
            label1.Style = Common.Windows.Controls.LabelStyle.Caption;
            label1.TabIndex = 4;
            label1.Text = "Collection:";
            // 
            // cmbCollection
            // 
            cmbCollection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbCollection.DropDownWidth = 121;
            cmbCollection.ErrorProvider = null;
            cmbCollection.Location = new System.Drawing.Point(600, 6);
            cmbCollection.Margin = new System.Windows.Forms.Padding(6);
            cmbCollection.Name = "cmbCollection";
            cmbCollection.Size = new System.Drawing.Size(188, 33);
            cmbCollection.TabIndex = 3;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label7.ForeColor = System.Drawing.SystemColors.ControlText;
            label7.Location = new System.Drawing.Point(794, 0);
            label7.Margin = new System.Windows.Forms.Padding(0);
            label7.Name = "label7";
            label7.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            label7.Size = new System.Drawing.Size(89, 39);
            label7.Style = Common.Windows.Controls.LabelStyle.Caption;
            label7.TabIndex = 17;
            label7.Text = "Exclude:";
            // 
            // cmbExcludedCollection
            // 
            cmbExcludedCollection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbExcludedCollection.DropDownWidth = 121;
            cmbExcludedCollection.ErrorProvider = null;
            cmbExcludedCollection.Location = new System.Drawing.Point(889, 6);
            cmbExcludedCollection.Margin = new System.Windows.Forms.Padding(6);
            cmbExcludedCollection.Name = "cmbExcludedCollection";
            cmbExcludedCollection.Size = new System.Drawing.Size(188, 33);
            cmbExcludedCollection.TabIndex = 16;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label6.ForeColor = System.Drawing.SystemColors.ControlText;
            label6.Location = new System.Drawing.Point(1083, 0);
            label6.Margin = new System.Windows.Forms.Padding(0);
            label6.Name = "label6";
            label6.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            label6.Size = new System.Drawing.Size(64, 39);
            label6.Style = Common.Windows.Controls.LabelStyle.Caption;
            label6.TabIndex = 14;
            label6.Text = "Rank:";
            // 
            // cmbTrackRankFilter
            // 
            cmbTrackRankFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbTrackRankFilter.DropDownWidth = 121;
            cmbTrackRankFilter.ErrorProvider = null;
            cmbTrackRankFilter.Items.AddRange(new object[] { "", "Good+", "Bearable+", "Unranked", "Forbidden" });
            cmbTrackRankFilter.Location = new System.Drawing.Point(1153, 6);
            cmbTrackRankFilter.Margin = new System.Windows.Forms.Padding(6);
            cmbTrackRankFilter.Name = "cmbTrackRankFilter";
            cmbTrackRankFilter.Size = new System.Drawing.Size(138, 33);
            cmbTrackRankFilter.TabIndex = 15;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label2.ForeColor = System.Drawing.SystemColors.ControlText;
            label2.Location = new System.Drawing.Point(1297, 0);
            label2.Margin = new System.Windows.Forms.Padding(0);
            label2.Name = "label2";
            label2.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            label2.Size = new System.Drawing.Size(92, 39);
            label2.Style = Common.Windows.Controls.LabelStyle.Caption;
            label2.TabIndex = 7;
            label2.Text = "Queued:";
            // 
            // cmbQueued
            // 
            cmbQueued.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbQueued.DropDownWidth = 121;
            cmbQueued.ErrorProvider = null;
            cmbQueued.Items.AddRange(new object[] { "", "Yes", "No" });
            cmbQueued.Location = new System.Drawing.Point(1395, 6);
            cmbQueued.Margin = new System.Windows.Forms.Padding(6);
            cmbQueued.Name = "cmbQueued";
            cmbQueued.Size = new System.Drawing.Size(72, 33);
            cmbQueued.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label3.ForeColor = System.Drawing.SystemColors.ControlText;
            label3.Location = new System.Drawing.Point(1473, 0);
            label3.Margin = new System.Windows.Forms.Padding(0);
            label3.Name = "label3";
            label3.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            label3.Size = new System.Drawing.Size(90, 39);
            label3.Style = Common.Windows.Controls.LabelStyle.Caption;
            label3.TabIndex = 9;
            label3.Text = "Shuffler:";
            // 
            // cmbShufflerFilter
            // 
            cmbShufflerFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbShufflerFilter.DropDownWidth = 121;
            cmbShufflerFilter.ErrorProvider = null;
            cmbShufflerFilter.Items.AddRange(new object[] { "", "Yes", "No" });
            cmbShufflerFilter.Location = new System.Drawing.Point(1569, 6);
            cmbShufflerFilter.Margin = new System.Windows.Forms.Padding(6);
            cmbShufflerFilter.Name = "cmbShufflerFilter";
            cmbShufflerFilter.Size = new System.Drawing.Size(72, 33);
            cmbShufflerFilter.TabIndex = 6;
            // 
            // mixableTracks
            // 
            mixableTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            mixableTracks.Location = new System.Drawing.Point(0, 0);
            mixableTracks.Margin = new System.Windows.Forms.Padding(6);
            mixableTracks.Name = "mixableTracks";
            mixableTracks.Size = new System.Drawing.Size(2185, 246);
            mixableTracks.TabIndex = 0;
            // 
            // btnUnshuffledRoulette
            // 
            btnUnshuffledRoulette.Dock = System.Windows.Forms.DockStyle.Right;
            btnUnshuffledRoulette.Location = new System.Drawing.Point(1930, 4);
            btnUnshuffledRoulette.Margin = new System.Windows.Forms.Padding(4);
            btnUnshuffledRoulette.Name = "btnUnshuffledRoulette";
            btnUnshuffledRoulette.Size = new System.Drawing.Size(248, 50);
            btnUnshuffledRoulette.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnUnshuffledRoulette.TabIndex = 56;
            btnUnshuffledRoulette.Text = "Unshuffled Roulette";
            // 
            // TrackLibraryControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlBackground2);
            Margin = new System.Windows.Forms.Padding(6);
            Name = "TrackLibraryControl";
            Size = new System.Drawing.Size(2199, 1090);
            mnuGenre.ResumeLayout(false);
            mnuArtist.ResumeLayout(false);
            mnuAlbum.ResumeLayout(false);
            mnuTrack.ResumeLayout(false);
            pnlBackground2.ResumeLayout(false);
            splLibraryMixable.Panel1.ResumeLayout(false);
            splLibraryMixable.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splLibraryMixable).EndInit();
            splLibraryMixable.ResumeLayout(false);
            pnlLibraryDetails.ResumeLayout(false);
            (splLibrary.Panel1).EndInit();
            splLibrary.Panel1.ResumeLayout(false);
            (splLibrary.Panel2).EndInit();
            splLibrary.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splLibrary).EndInit();
            (splLeftRight.Panel1).EndInit();
            splLeftRight.Panel1.ResumeLayout(false);
            (splLeftRight.Panel2).EndInit();
            splLeftRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splLeftRight).EndInit();
            (splLeftMiddle.Panel1).EndInit();
            splLeftMiddle.Panel1.ResumeLayout(false);
            (splLeftMiddle.Panel2).EndInit();
            splLeftMiddle.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splLeftMiddle).EndInit();
            pnlGenre.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grdGenre).EndInit();
            pnlArtist.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grdArtist).EndInit();
            pnlAlbum.ResumeLayout(false);
            pnlAlbum.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlAlbumHeader).EndInit();
            pnlAlbumHeader.ResumeLayout(false);
            pnlAlbumHeader.PerformLayout();
            pnlTrack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grdTracks).EndInit();
            pnlDivider.ResumeLayout(false);
            flpToolbarRight.ResumeLayout(false);
            flpToolbarRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cmbCollection).EndInit();
            ((System.ComponentModel.ISupportInitialize)cmbExcludedCollection).EndInit();
            ((System.ComponentModel.ISupportInitialize)cmbTrackRankFilter).EndInit();
            ((System.ComponentModel.ISupportInitialize)cmbQueued).EndInit();
            ((System.ComponentModel.ISupportInitialize)cmbShufflerFilter).EndInit();
            ResumeLayout(false);

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
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateAudio;
        private Common.Windows.Controls.Button btnUnshuffledRoulette;
    }
}
