namespace Halloumi.Shuffler.Controls
{
    partial class PlaylistControl
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
            var dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            var dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            var dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mnuTrack = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuPlay = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAddTrackToPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRemoveTrackFromPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuOpenFileLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuUpdateTrackDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdateShufflerDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRemoveShufflerDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuTrackRank = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMixRank = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPreviewMix = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlContentBackground = new Halloumi.Common.Windows.Controls.Panel();
            this.splTopBottom = new ComponentFactory.Krypton.Toolkit.KryptonSplitContainer();
            this.pnlTop = new Halloumi.Common.Windows.Controls.Panel();
            this.pnlGrid = new Halloumi.Common.Windows.Controls.Panel();
            this.grdPlaylist = new Halloumi.Shuffler.Controls.DataGridView();
            this.colTrack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndBPM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrackRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrackKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKeyDiff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.linLine = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.pnlButtons = new Halloumi.Common.Windows.Controls.Panel();
            this.flpLeft = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCount = new Halloumi.Common.Windows.Controls.Label();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRemove = new Halloumi.Common.Windows.Controls.Button();
            this.btnClear = new Halloumi.Common.Windows.Controls.Button();
            this.btnSave = new Halloumi.Common.Windows.Controls.Button();
            this.btnOpen = new Halloumi.Common.Windows.Controls.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.trackDetails = new Halloumi.Shuffler.Controls.TrackDetails();
            this.mixableTracks = new Halloumi.Shuffler.Controls.MixableTracks();
            this.mnuTrack.SuspendLayout();
            this.pnlContentBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splTopBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splTopBottom.Panel1)).BeginInit();
            this.splTopBottom.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splTopBottom.Panel2)).BeginInit();
            this.splTopBottom.Panel2.SuspendLayout();
            this.splTopBottom.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPlaylist)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.flpLeft.SuspendLayout();
            this.flpButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuTrack
            // 
            this.mnuTrack.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.mnuTrack.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuTrack.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPlay,
            this.mnuSep1,
            this.mnuAddTrackToPlaylist,
            this.mnuRemoveTrackFromPlaylist,
            this.mnuSep2,
            this.mnuOpenFileLocation,
            this.mnuSep3,
            this.mnuUpdateTrackDetails,
            this.mnuUpdateShufflerDetails,
            this.mnuRemoveShufflerDetails,
            this.mnuSep4,
            this.mnuTrackRank,
            this.mnuMixRank,
            this.mnuPreviewMix});
            this.mnuTrack.Name = "mnuTrack";
            this.mnuTrack.Size = new System.Drawing.Size(242, 268);
            // 
            // mnuPlay
            // 
            this.mnuPlay.Name = "mnuPlay";
            this.mnuPlay.Size = new System.Drawing.Size(241, 24);
            this.mnuPlay.Text = "&Play";
            // 
            // mnuSep1
            // 
            this.mnuSep1.Name = "mnuSep1";
            this.mnuSep1.Size = new System.Drawing.Size(238, 6);
            // 
            // mnuAddTrackToPlaylist
            // 
            this.mnuAddTrackToPlaylist.Name = "mnuAddTrackToPlaylist";
            this.mnuAddTrackToPlaylist.Size = new System.Drawing.Size(241, 24);
            this.mnuAddTrackToPlaylist.Text = "A&dd To Playlist";
            // 
            // mnuRemoveTrackFromPlaylist
            // 
            this.mnuRemoveTrackFromPlaylist.Name = "mnuRemoveTrackFromPlaylist";
            this.mnuRemoveTrackFromPlaylist.Size = new System.Drawing.Size(241, 24);
            this.mnuRemoveTrackFromPlaylist.Text = "&Remove From Playlist";
            // 
            // mnuSep2
            // 
            this.mnuSep2.Name = "mnuSep2";
            this.mnuSep2.Size = new System.Drawing.Size(238, 6);
            // 
            // mnuOpenFileLocation
            // 
            this.mnuOpenFileLocation.Name = "mnuOpenFileLocation";
            this.mnuOpenFileLocation.Size = new System.Drawing.Size(241, 24);
            this.mnuOpenFileLocation.Text = "Open File &Location";
            // 
            // mnuSep3
            // 
            this.mnuSep3.Name = "mnuSep3";
            this.mnuSep3.Size = new System.Drawing.Size(238, 6);
            // 
            // mnuUpdateTrackDetails
            // 
            this.mnuUpdateTrackDetails.Name = "mnuUpdateTrackDetails";
            this.mnuUpdateTrackDetails.Size = new System.Drawing.Size(241, 24);
            this.mnuUpdateTrackDetails.Text = "Update &Track Details";
            // 
            // mnuUpdateShufflerDetails
            // 
            this.mnuUpdateShufflerDetails.Name = "mnuUpdateShufflerDetails";
            this.mnuUpdateShufflerDetails.Size = new System.Drawing.Size(241, 24);
            this.mnuUpdateShufflerDetails.Text = "Update &Shuffler Details...";
            // 
            // mnuRemoveShufflerDetails
            // 
            this.mnuRemoveShufflerDetails.Name = "mnuRemoveShufflerDetails";
            this.mnuRemoveShufflerDetails.Size = new System.Drawing.Size(241, 24);
            this.mnuRemoveShufflerDetails.Text = "&Remove Shuffler Details";
            this.mnuRemoveShufflerDetails.Click += new System.EventHandler(this.mnuRemoveShufflerDetails_Click);
            // 
            // mnuSep4
            // 
            this.mnuSep4.Name = "mnuSep4";
            this.mnuSep4.Size = new System.Drawing.Size(238, 6);
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
            this.mnuTrackRank.Size = new System.Drawing.Size(241, 24);
            this.mnuTrackRank.Text = "Trac&k Rating";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(211, 24);
            this.toolStripMenuItem1.Text = "toolStripMenuItem1";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.mnuTrackRank_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(211, 24);
            this.toolStripMenuItem2.Text = "toolStripMenuItem2";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.mnuTrackRank_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(211, 24);
            this.toolStripMenuItem3.Text = "toolStripMenuItem3";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.mnuTrackRank_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(211, 24);
            this.toolStripMenuItem4.Text = "toolStripMenuItem4";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.mnuTrackRank_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(211, 24);
            this.toolStripMenuItem5.Text = "toolStripMenuItem5";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.mnuTrackRank_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(211, 24);
            this.toolStripMenuItem6.Text = "toolStripMenuItem6";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.mnuTrackRank_Click);
            // 
            // mnuMixRank
            // 
            this.mnuMixRank.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9,
            this.toolStripMenuItem10,
            this.toolStripMenuItem11,
            this.toolStripMenuItem12});
            this.mnuMixRank.Name = "mnuMixRank";
            this.mnuMixRank.Size = new System.Drawing.Size(241, 24);
            this.mnuMixRank.Text = "Mi&x Rating";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(219, 24);
            this.toolStripMenuItem7.Text = "toolStripMenuItem7";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.mnuMixRank_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(219, 24);
            this.toolStripMenuItem8.Text = "toolStripMenuItem8";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.mnuMixRank_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(219, 24);
            this.toolStripMenuItem9.Text = "toolStripMenuItem9";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.mnuMixRank_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(219, 24);
            this.toolStripMenuItem10.Text = "toolStripMenuItem10";
            this.toolStripMenuItem10.Click += new System.EventHandler(this.mnuMixRank_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(219, 24);
            this.toolStripMenuItem11.Text = "toolStripMenuItem11";
            this.toolStripMenuItem11.Click += new System.EventHandler(this.mnuMixRank_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(219, 24);
            this.toolStripMenuItem12.Text = "toolStripMenuItem12";
            this.toolStripMenuItem12.Click += new System.EventHandler(this.mnuMixRank_Click);
            // 
            // mnuPreviewMix
            // 
            this.mnuPreviewMix.Name = "mnuPreviewMix";
            this.mnuPreviewMix.Size = new System.Drawing.Size(241, 24);
            this.mnuPreviewMix.Text = "Pre&view Mix";
            // 
            // pnlContentBackground
            // 
            this.pnlContentBackground.BackColor = System.Drawing.SystemColors.Control;
            this.pnlContentBackground.Controls.Add(this.splTopBottom);
            this.pnlContentBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContentBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlContentBackground.Margin = new System.Windows.Forms.Padding(4);
            this.pnlContentBackground.Name = "pnlContentBackground";
            this.pnlContentBackground.Padding = new System.Windows.Forms.Padding(5);
            this.pnlContentBackground.Size = new System.Drawing.Size(881, 455);
            this.pnlContentBackground.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlContentBackground.TabIndex = 12;
            // 
            // splTopBottom
            // 
            this.splTopBottom.Cursor = System.Windows.Forms.Cursors.Default;
            this.splTopBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splTopBottom.Location = new System.Drawing.Point(5, 5);
            this.splTopBottom.Name = "splTopBottom";
            this.splTopBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splTopBottom.Panel1
            // 
            this.splTopBottom.Panel1.Controls.Add(this.pnlTop);
            // 
            // splTopBottom.Panel2
            // 
            this.splTopBottom.Panel2.Controls.Add(this.mixableTracks);
            this.splTopBottom.Size = new System.Drawing.Size(871, 445);
            this.splTopBottom.SplitterDistance = 323;
            this.splTopBottom.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.SystemColors.Control;
            this.pnlTop.Controls.Add(this.pnlGrid);
            this.pnlTop.Controls.Add(this.panel1);
            this.pnlTop.Controls.Add(this.trackDetails);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(871, 323);
            this.pnlTop.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlTop.TabIndex = 11;
            // 
            // pnlGrid
            // 
            this.pnlGrid.BackColor = System.Drawing.SystemColors.Control;
            this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGrid.Controls.Add(this.grdPlaylist);
            this.pnlGrid.Controls.Add(this.linLine);
            this.pnlGrid.Controls.Add(this.pnlButtons);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 0);
            this.pnlGrid.Margin = new System.Windows.Forms.Padding(0);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(1);
            this.pnlGrid.Size = new System.Drawing.Size(871, 245);
            this.pnlGrid.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlGrid.TabIndex = 15;
            // 
            // grdPlaylist
            // 
            this.grdPlaylist.AllowDrop = true;
            this.grdPlaylist.AllowUserToAddRows = false;
            this.grdPlaylist.AllowUserToDeleteRows = false;
            this.grdPlaylist.AllowUserToResizeColumns = false;
            this.grdPlaylist.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grdPlaylist.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdPlaylist.ColumnHeadersHeight = 26;
            this.grdPlaylist.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTrack,
            this.colLength,
            this.colEndBPM,
            this.colMix,
            this.colTrackRank,
            this.colTrackKey,
            this.colKeyDiff});
            this.grdPlaylist.ContextMenuStrip = this.mnuTrack;
            this.grdPlaylist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdPlaylist.HideOuterBorders = true;
            this.grdPlaylist.Location = new System.Drawing.Point(1, 1);
            this.grdPlaylist.Margin = new System.Windows.Forms.Padding(4);
            this.grdPlaylist.MergeColor = System.Drawing.Color.Gainsboro;
            this.grdPlaylist.Name = "grdPlaylist";
            this.grdPlaylist.ReadOnly = true;
            this.grdPlaylist.RowHeadersVisible = false;
            this.grdPlaylist.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdPlaylist.RowTemplate.Height = 24;
            this.grdPlaylist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdPlaylist.Size = new System.Drawing.Size(869, 189);
            this.grdPlaylist.SortColumnIndex = -1;
            this.grdPlaylist.SortOrder = System.Windows.Forms.SortOrder.None;
            this.grdPlaylist.StateCommon.Background.Color1 = System.Drawing.Color.White;
            this.grdPlaylist.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.grdPlaylist.StateCommon.DataCell.Border.Color1 = System.Drawing.Color.White;
            this.grdPlaylist.StateCommon.DataCell.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grdPlaylist.TabIndex = 49;
            this.grdPlaylist.VirtualMode = true;
            // 
            // colTrack
            // 
            this.colTrack.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTrack.DataPropertyName = "Description";
            this.colTrack.HeaderText = "Track";
            this.colTrack.Name = "colTrack";
            this.colTrack.ReadOnly = true;
            // 
            // colLength
            // 
            this.colLength.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLength.DataPropertyName = "LengthFormatted";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colLength.DefaultCellStyle = dataGridViewCellStyle2;
            this.colLength.HeaderText = "Length";
            this.colLength.Name = "colLength";
            this.colLength.ReadOnly = true;
            this.colLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colLength.Width = 62;
            // 
            // colEndBPM
            // 
            this.colEndBPM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colEndBPM.DataPropertyName = "BPM";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.colEndBPM.DefaultCellStyle = dataGridViewCellStyle3;
            this.colEndBPM.HeaderText = "BPM";
            this.colEndBPM.Name = "colEndBPM";
            this.colEndBPM.ReadOnly = true;
            this.colEndBPM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEndBPM.Width = 47;
            // 
            // colMix
            // 
            this.colMix.DataPropertyName = "MixRankDescription";
            this.colMix.HeaderText = "Mix";
            this.colMix.Name = "colMix";
            this.colMix.ReadOnly = true;
            // 
            // colTrackRank
            // 
            this.colTrackRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTrackRank.DataPropertyName = "TrackRankDescription";
            this.colTrackRank.HeaderText = "Rank";
            this.colTrackRank.Name = "colTrackRank";
            this.colTrackRank.ReadOnly = true;
            this.colTrackRank.Width = 68;
            // 
            // colTrackKey
            // 
            this.colTrackKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTrackKey.DataPropertyName = "Key";
            this.colTrackKey.HeaderText = "Key";
            this.colTrackKey.Name = "colTrackKey";
            this.colTrackKey.ReadOnly = true;
            this.colTrackKey.Width = 60;
            // 
            // colKeyDiff
            // 
            this.colKeyDiff.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colKeyDiff.DataPropertyName = "KeyRank";
            this.colKeyDiff.HeaderText = "Key Rank";
            this.colKeyDiff.Name = "colKeyDiff";
            this.colKeyDiff.ReadOnly = true;
            this.colKeyDiff.Width = 96;
            // 
            // linLine
            // 
            this.linLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.linLine.Location = new System.Drawing.Point(1, 190);
            this.linLine.Margin = new System.Windows.Forms.Padding(4);
            this.linLine.Name = "linLine";
            this.linLine.Size = new System.Drawing.Size(869, 2);
            this.linLine.TabIndex = 48;
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.flpLeft);
            this.pnlButtons.Controls.Add(this.flpButtons);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(1, 192);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(4);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(869, 52);
            this.pnlButtons.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlButtons.TabIndex = 47;
            // 
            // flpLeft
            // 
            this.flpLeft.BackColor = System.Drawing.Color.Transparent;
            this.flpLeft.Controls.Add(this.lblCount);
            this.flpLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flpLeft.Location = new System.Drawing.Point(0, 0);
            this.flpLeft.Margin = new System.Windows.Forms.Padding(4);
            this.flpLeft.Name = "flpLeft";
            this.flpLeft.Padding = new System.Windows.Forms.Padding(3, 2, 7, 2);
            this.flpLeft.Size = new System.Drawing.Size(132, 52);
            this.flpLeft.TabIndex = 18;
            // 
            // lblCount
            // 
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCount.Location = new System.Drawing.Point(7, 2);
            this.lblCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.lblCount.Size = new System.Drawing.Size(107, 33);
            this.lblCount.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblCount.TabIndex = 7;
            this.lblCount.Text = "0 tracks";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flpButtons
            // 
            this.flpButtons.BackColor = System.Drawing.Color.Transparent;
            this.flpButtons.Controls.Add(this.btnRemove);
            this.flpButtons.Controls.Add(this.btnClear);
            this.flpButtons.Controls.Add(this.btnSave);
            this.flpButtons.Controls.Add(this.btnOpen);
            this.flpButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpButtons.Location = new System.Drawing.Point(489, 0);
            this.flpButtons.Margin = new System.Windows.Forms.Padding(4);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Padding = new System.Windows.Forms.Padding(3, 2, 7, 2);
            this.flpButtons.Size = new System.Drawing.Size(380, 52);
            this.flpButtons.TabIndex = 16;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(284, 7);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(5);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(81, 38);
            this.btnRemove.TabIndex = 10;
            this.btnRemove.Text = "Remove";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(193, 7);
            this.btnClear.Margin = new System.Windows.Forms.Padding(5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(81, 38);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(102, 7);
            this.btnSave.Margin = new System.Windows.Forms.Padding(5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(81, 38);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(11, 7);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(81, 38);
            this.btnOpen.TabIndex = 13;
            this.btnOpen.Text = "Open";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 245);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(871, 5);
            this.panel1.TabIndex = 14;
            // 
            // trackDetails
            // 
            this.trackDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.trackDetails.Location = new System.Drawing.Point(0, 250);
            this.trackDetails.Name = "trackDetails";
            this.trackDetails.Size = new System.Drawing.Size(871, 73);
            this.trackDetails.TabIndex = 12;
            // 
            // mixableTracks
            // 
            this.mixableTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mixableTracks.Location = new System.Drawing.Point(0, 0);
            this.mixableTracks.Margin = new System.Windows.Forms.Padding(4);
            this.mixableTracks.Name = "mixableTracks";
            this.mixableTracks.PlaylistControl = null;
            this.mixableTracks.Size = new System.Drawing.Size(871, 117);
            this.mixableTracks.TabIndex = 0;
            // 
            // PlaylistControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlContentBackground);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PlaylistControl";
            this.Size = new System.Drawing.Size(881, 455);
            this.mnuTrack.ResumeLayout(false);
            this.pnlContentBackground.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splTopBottom.Panel1)).EndInit();
            this.splTopBottom.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splTopBottom.Panel2)).EndInit();
            this.splTopBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splTopBottom)).EndInit();
            this.splTopBottom.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPlaylist)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.flpLeft.ResumeLayout(false);
            this.flpButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip mnuTrack;
        private System.Windows.Forms.ToolStripMenuItem mnuPlay;
        private System.Windows.Forms.ToolStripSeparator mnuSep1;
        private System.Windows.Forms.ToolStripMenuItem mnuAddTrackToPlaylist;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveTrackFromPlaylist;
        private System.Windows.Forms.ToolStripSeparator mnuSep2;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenFileLocation;
        private System.Windows.Forms.ToolStripSeparator mnuSep4;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateTrackDetails;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateShufflerDetails;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveShufflerDetails;
        private System.Windows.Forms.ToolStripSeparator mnuSep3;
        private System.Windows.Forms.ToolStripMenuItem mnuTrackRank;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem mnuMixRank;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem mnuPreviewMix;
        private Halloumi.Common.Windows.Controls.Panel pnlContentBackground;
        private ComponentFactory.Krypton.Toolkit.KryptonSplitContainer splTopBottom;
        private Halloumi.Common.Windows.Controls.Panel pnlTop;
        private Halloumi.Common.Windows.Controls.Panel pnlGrid;
        private DataGridView grdPlaylist;
        private Halloumi.Common.Windows.Controls.BeveledLine linLine;
        private Halloumi.Common.Windows.Controls.Panel pnlButtons;
        private System.Windows.Forms.FlowLayoutPanel flpLeft;
        private Halloumi.Common.Windows.Controls.Label lblCount;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private Halloumi.Common.Windows.Controls.Button btnRemove;
        private Halloumi.Common.Windows.Controls.Button btnClear;
        private Halloumi.Common.Windows.Controls.Button btnSave;
        private Halloumi.Common.Windows.Controls.Button btnOpen;
        private System.Windows.Forms.Panel panel1;
        private TrackDetails trackDetails;
        private MixableTracks mixableTracks;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrack;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndBPM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMix;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKeyDiff;
    }
}
