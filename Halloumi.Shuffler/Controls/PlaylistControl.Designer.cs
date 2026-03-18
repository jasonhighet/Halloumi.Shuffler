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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            mnuTrack = new System.Windows.Forms.ContextMenuStrip(components);
            mnuPlay = new System.Windows.Forms.ToolStripMenuItem();
            mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
            mnuAddTrackToCollection = new System.Windows.Forms.ToolStripMenuItem();
            mnuRemoveTrackFromCollection = new System.Windows.Forms.ToolStripMenuItem();
            mnuSep2 = new System.Windows.Forms.ToolStripSeparator();
            mnuOpenFileLocation = new System.Windows.Forms.ToolStripMenuItem();
            mnuSep3 = new System.Windows.Forms.ToolStripSeparator();
            mnuUpdateTrackDetails = new System.Windows.Forms.ToolStripMenuItem();
            mnuUpdateShufflerDetails = new System.Windows.Forms.ToolStripMenuItem();
            mnuRemoveShufflerDetails = new System.Windows.Forms.ToolStripMenuItem();
            mnuSep4 = new System.Windows.Forms.ToolStripSeparator();
            mnuTrackRank = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            mnuMixRank = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            mnuPreviewMix = new System.Windows.Forms.ToolStripMenuItem();
            pnlContentBackground = new Halloumi.Common.Windows.Controls.Panel();
            splTopBottom = new ComponentFactory.Krypton.Toolkit.KryptonSplitContainer();
            pnlTop = new Halloumi.Common.Windows.Controls.Panel();
            pnlGrid = new Halloumi.Common.Windows.Controls.Panel();
            grdPlaylist = new DataGridView();
            colTrack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEndBPM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colMix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTrackRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTrackKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colKeyDiff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            linLine = new Halloumi.Common.Windows.Controls.BeveledLine();
            pnlButtons = new Halloumi.Common.Windows.Controls.Panel();
            flpLeft = new System.Windows.Forms.FlowLayoutPanel();
            lblCount = new Halloumi.Common.Windows.Controls.Label();
            flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            btnRemove = new Halloumi.Common.Windows.Controls.Button();
            btnClear = new Halloumi.Common.Windows.Controls.Button();
            btnGenerate = new Halloumi.Common.Windows.Controls.Button();
            btnGenerateNow = new Halloumi.Common.Windows.Controls.Button();
            btnLeastMixedRoulette = new Halloumi.Common.Windows.Controls.Button();
            panel1 = new System.Windows.Forms.Panel();
            trackDetails = new TrackDetails();
            mixableTracks = new MixableTracks();
            mnuTrack.SuspendLayout();
            pnlContentBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splTopBottom).BeginInit();
            (splTopBottom.Panel1).BeginInit();
            splTopBottom.Panel1.SuspendLayout();
            (splTopBottom.Panel2).BeginInit();
            splTopBottom.Panel2.SuspendLayout();
            pnlTop.SuspendLayout();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdPlaylist).BeginInit();
            pnlButtons.SuspendLayout();
            flpLeft.SuspendLayout();
            flpButtons.SuspendLayout();
            SuspendLayout();
            // 
            // mnuTrack
            // 
            mnuTrack.Font = new System.Drawing.Font("Segoe UI", 9F);
            mnuTrack.ImageScalingSize = new System.Drawing.Size(20, 20);
            mnuTrack.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuPlay, mnuSep1, mnuAddTrackToCollection, mnuRemoveTrackFromCollection, mnuSep2, mnuOpenFileLocation, mnuSep3, mnuUpdateTrackDetails, mnuUpdateShufflerDetails, mnuRemoveShufflerDetails, mnuSep4, mnuTrackRank, mnuMixRank, mnuPreviewMix });
            mnuTrack.Name = "mnuTrack";
            mnuTrack.Size = new System.Drawing.Size(316, 368);
            // 
            // mnuPlay
            // 
            mnuPlay.Name = "mnuPlay";
            mnuPlay.Size = new System.Drawing.Size(315, 34);
            mnuPlay.Text = "&Play";
            // 
            // mnuSep1
            // 
            mnuSep1.Name = "mnuSep1";
            mnuSep1.Size = new System.Drawing.Size(312, 6);
            // 
            // mnuAddTrackToCollection
            // 
            mnuAddTrackToCollection.Name = "mnuAddTrackToCollection";
            mnuAddTrackToCollection.Size = new System.Drawing.Size(315, 34);
            mnuAddTrackToCollection.Text = "A&dd To Collection";
            // 
            // mnuRemoveTrackFromCollection
            // 
            mnuRemoveTrackFromCollection.Name = "mnuRemoveTrackFromCollection";
            mnuRemoveTrackFromCollection.Size = new System.Drawing.Size(315, 34);
            mnuRemoveTrackFromCollection.Text = "&Remove From Collection";
            // 
            // mnuSep2
            // 
            mnuSep2.Name = "mnuSep2";
            mnuSep2.Size = new System.Drawing.Size(312, 6);
            // 
            // mnuOpenFileLocation
            // 
            mnuOpenFileLocation.Name = "mnuOpenFileLocation";
            mnuOpenFileLocation.Size = new System.Drawing.Size(315, 34);
            mnuOpenFileLocation.Text = "Open File &Location";
            // 
            // mnuSep3
            // 
            mnuSep3.Name = "mnuSep3";
            mnuSep3.Size = new System.Drawing.Size(312, 6);
            // 
            // mnuUpdateTrackDetails
            // 
            mnuUpdateTrackDetails.Name = "mnuUpdateTrackDetails";
            mnuUpdateTrackDetails.Size = new System.Drawing.Size(315, 34);
            mnuUpdateTrackDetails.Text = "Update &Track Details";
            // 
            // mnuUpdateShufflerDetails
            // 
            mnuUpdateShufflerDetails.Name = "mnuUpdateShufflerDetails";
            mnuUpdateShufflerDetails.Size = new System.Drawing.Size(315, 34);
            mnuUpdateShufflerDetails.Text = "Update &Shuffler Details...";
            // 
            // mnuRemoveShufflerDetails
            // 
            mnuRemoveShufflerDetails.Name = "mnuRemoveShufflerDetails";
            mnuRemoveShufflerDetails.Size = new System.Drawing.Size(315, 34);
            mnuRemoveShufflerDetails.Text = "&Remove Shuffler Details";
            mnuRemoveShufflerDetails.Click += mnuRemoveShufflerDetails_Click;
            // 
            // mnuSep4
            // 
            mnuSep4.Name = "mnuSep4";
            mnuSep4.Size = new System.Drawing.Size(312, 6);
            // 
            // mnuTrackRank
            // 
            mnuTrackRank.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4, toolStripMenuItem5, toolStripMenuItem6 });
            mnuTrackRank.Name = "mnuTrackRank";
            mnuTrackRank.Size = new System.Drawing.Size(315, 34);
            mnuTrackRank.Text = "Trac&k Rating";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(270, 34);
            toolStripMenuItem1.Text = "toolStripMenuItem1";
            toolStripMenuItem1.Click += mnuTrackRank_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(270, 34);
            toolStripMenuItem2.Text = "toolStripMenuItem2";
            toolStripMenuItem2.Click += mnuTrackRank_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(270, 34);
            toolStripMenuItem3.Text = "toolStripMenuItem3";
            toolStripMenuItem3.Click += mnuTrackRank_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new System.Drawing.Size(270, 34);
            toolStripMenuItem4.Text = "toolStripMenuItem4";
            toolStripMenuItem4.Click += mnuTrackRank_Click;
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new System.Drawing.Size(270, 34);
            toolStripMenuItem5.Text = "toolStripMenuItem5";
            toolStripMenuItem5.Click += mnuTrackRank_Click;
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new System.Drawing.Size(270, 34);
            toolStripMenuItem6.Text = "toolStripMenuItem6";
            toolStripMenuItem6.Click += mnuTrackRank_Click;
            // 
            // mnuMixRank
            // 
            mnuMixRank.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem7, toolStripMenuItem8, toolStripMenuItem9, toolStripMenuItem10, toolStripMenuItem11, toolStripMenuItem12 });
            mnuMixRank.Name = "mnuMixRank";
            mnuMixRank.Size = new System.Drawing.Size(315, 34);
            mnuMixRank.Text = "Mi&x Rating";
            // 
            // toolStripMenuItem7
            // 
            toolStripMenuItem7.Name = "toolStripMenuItem7";
            toolStripMenuItem7.Size = new System.Drawing.Size(281, 34);
            toolStripMenuItem7.Text = "toolStripMenuItem7";
            toolStripMenuItem7.Click += mnuMixRank_Click;
            // 
            // toolStripMenuItem8
            // 
            toolStripMenuItem8.Name = "toolStripMenuItem8";
            toolStripMenuItem8.Size = new System.Drawing.Size(281, 34);
            toolStripMenuItem8.Text = "toolStripMenuItem8";
            toolStripMenuItem8.Click += mnuMixRank_Click;
            // 
            // toolStripMenuItem9
            // 
            toolStripMenuItem9.Name = "toolStripMenuItem9";
            toolStripMenuItem9.Size = new System.Drawing.Size(281, 34);
            toolStripMenuItem9.Text = "toolStripMenuItem9";
            toolStripMenuItem9.Click += mnuMixRank_Click;
            // 
            // toolStripMenuItem10
            // 
            toolStripMenuItem10.Name = "toolStripMenuItem10";
            toolStripMenuItem10.Size = new System.Drawing.Size(281, 34);
            toolStripMenuItem10.Text = "toolStripMenuItem10";
            toolStripMenuItem10.Click += mnuMixRank_Click;
            // 
            // toolStripMenuItem11
            // 
            toolStripMenuItem11.Name = "toolStripMenuItem11";
            toolStripMenuItem11.Size = new System.Drawing.Size(281, 34);
            toolStripMenuItem11.Text = "toolStripMenuItem11";
            toolStripMenuItem11.Click += mnuMixRank_Click;
            // 
            // toolStripMenuItem12
            // 
            toolStripMenuItem12.Name = "toolStripMenuItem12";
            toolStripMenuItem12.Size = new System.Drawing.Size(281, 34);
            toolStripMenuItem12.Text = "toolStripMenuItem12";
            toolStripMenuItem12.Click += mnuMixRank_Click;
            // 
            // mnuPreviewMix
            // 
            mnuPreviewMix.Name = "mnuPreviewMix";
            mnuPreviewMix.Size = new System.Drawing.Size(315, 34);
            mnuPreviewMix.Text = "Pre&view Mix";
            // 
            // pnlContentBackground
            // 
            pnlContentBackground.BackColor = System.Drawing.SystemColors.Control;
            pnlContentBackground.Controls.Add(splTopBottom);
            pnlContentBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlContentBackground.Location = new System.Drawing.Point(0, 0);
            pnlContentBackground.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            pnlContentBackground.Name = "pnlContentBackground";
            pnlContentBackground.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            pnlContentBackground.Size = new System.Drawing.Size(1621, 744);
            pnlContentBackground.Style = Common.Windows.Controls.PanelStyle.Content;
            pnlContentBackground.TabIndex = 12;
            // 
            // splTopBottom
            // 
            splTopBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            splTopBottom.Location = new System.Drawing.Point(7, 7);
            splTopBottom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            splTopBottom.Name = "splTopBottom";
            splTopBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // 
            // 
            splTopBottom.Panel1.Controls.Add(pnlTop);
            // 
            // 
            // 
            splTopBottom.Panel2.Controls.Add(mixableTracks);
            splTopBottom.Size = new System.Drawing.Size(1607, 730);
            splTopBottom.SplitterDistance = 526;
            splTopBottom.TabIndex = 0;
            // 
            // pnlTop
            // 
            pnlTop.BackColor = System.Drawing.SystemColors.Control;
            pnlTop.Controls.Add(pnlGrid);
            pnlTop.Controls.Add(panel1);
            pnlTop.Controls.Add(trackDetails);
            pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlTop.Location = new System.Drawing.Point(0, 0);
            pnlTop.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new System.Drawing.Size(1607, 526);
            pnlTop.Style = Common.Windows.Controls.PanelStyle.Content;
            pnlTop.TabIndex = 11;
            // 
            // pnlGrid
            // 
            pnlGrid.BackColor = System.Drawing.SystemColors.Control;
            pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlGrid.Controls.Add(grdPlaylist);
            pnlGrid.Controls.Add(linLine);
            pnlGrid.Controls.Add(pnlButtons);
            pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlGrid.Location = new System.Drawing.Point(0, 0);
            pnlGrid.Margin = new System.Windows.Forms.Padding(0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            pnlGrid.Size = new System.Drawing.Size(1607, 408);
            pnlGrid.Style = Common.Windows.Controls.PanelStyle.ButtonStrip;
            pnlGrid.TabIndex = 15;
            // 
            // grdPlaylist
            // 
            grdPlaylist.AllowDrop = true;
            grdPlaylist.AllowUserToAddRows = false;
            grdPlaylist.AllowUserToDeleteRows = false;
            grdPlaylist.AllowUserToResizeColumns = false;
            grdPlaylist.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            grdPlaylist.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            grdPlaylist.ColumnHeadersHeight = 44;
            grdPlaylist.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colTrack, colLength, colEndBPM, colMix, colTrackRank, colTrackKey, colKeyDiff });
            grdPlaylist.ContextMenuStrip = mnuTrack;
            grdPlaylist.Dock = System.Windows.Forms.DockStyle.Fill;
            grdPlaylist.HideOuterBorders = true;
            grdPlaylist.Location = new System.Drawing.Point(2, 2);
            grdPlaylist.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            grdPlaylist.MergeColor = System.Drawing.Color.Gainsboro;
            grdPlaylist.Name = "grdPlaylist";
            grdPlaylist.ReadOnly = true;
            grdPlaylist.RowHeadersVisible = false;
            grdPlaylist.RowHeadersWidth = 72;
            grdPlaylist.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            grdPlaylist.RowTemplate.Height = 44;
            grdPlaylist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            grdPlaylist.Size = new System.Drawing.Size(1603, 324);
            grdPlaylist.SortColumnIndex = -1;
            grdPlaylist.SortOrder = System.Windows.Forms.SortOrder.None;
            grdPlaylist.StateCommon.Background.Color1 = System.Drawing.Color.White;
            grdPlaylist.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            grdPlaylist.StateCommon.DataCell.Border.Color1 = System.Drawing.Color.White;
            grdPlaylist.StateCommon.DataCell.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right;
            grdPlaylist.TabIndex = 49;
            grdPlaylist.VirtualMode = true;
            // 
            // colTrack
            // 
            colTrack.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colTrack.DataPropertyName = "Description";
            colTrack.HeaderText = "Track";
            colTrack.MinimumWidth = 9;
            colTrack.Name = "colTrack";
            colTrack.ReadOnly = true;
            // 
            // colLength
            // 
            colLength.DataPropertyName = "LengthFormatted";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            colLength.DefaultCellStyle = dataGridViewCellStyle2;
            colLength.HeaderText = "Length";
            colLength.MinimumWidth = 90;
            colLength.Name = "colLength";
            colLength.ReadOnly = true;
            colLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            colLength.Width = 90;
            // 
            // colEndBPM
            // 
            colEndBPM.DataPropertyName = "BPM";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            colEndBPM.DefaultCellStyle = dataGridViewCellStyle3;
            colEndBPM.HeaderText = "BPM";
            colEndBPM.MinimumWidth = 90;
            colEndBPM.Name = "colEndBPM";
            colEndBPM.ReadOnly = true;
            colEndBPM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            colEndBPM.Width = 90;
            // 
            // colMix
            // 
            colMix.DataPropertyName = "MixRankDescription";
            colMix.HeaderText = "Mix";
            colMix.MinimumWidth = 9;
            colMix.Name = "colMix";
            colMix.ReadOnly = true;
            colMix.Width = 175;
            // 
            // colTrackRank
            // 
            colTrackRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colTrackRank.DataPropertyName = "TrackRankDescription";
            colTrackRank.HeaderText = "Rank";
            colTrackRank.MinimumWidth = 9;
            colTrackRank.Name = "colTrackRank";
            colTrackRank.ReadOnly = true;
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
            colTrackKey.Width = 75;
            // 
            // colKeyDiff
            // 
            colKeyDiff.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colKeyDiff.DataPropertyName = "KeyRank";
            colKeyDiff.HeaderText = "Key Rank";
            colKeyDiff.MinimumWidth = 9;
            colKeyDiff.Name = "colKeyDiff";
            colKeyDiff.ReadOnly = true;
            colKeyDiff.Width = 127;
            // 
            // linLine
            // 
            linLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            linLine.Location = new System.Drawing.Point(2, 326);
            linLine.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            linLine.Name = "linLine";
            linLine.Size = new System.Drawing.Size(1603, 2);
            linLine.TabIndex = 48;
            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            pnlButtons.Controls.Add(flpLeft);
            pnlButtons.Controls.Add(flpButtons);
            pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlButtons.Location = new System.Drawing.Point(2, 328);
            pnlButtons.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new System.Drawing.Size(1603, 78);
            pnlButtons.Style = Common.Windows.Controls.PanelStyle.ButtonStrip;
            pnlButtons.TabIndex = 47;
            // 
            // flpLeft
            // 
            flpLeft.BackColor = System.Drawing.Color.Transparent;
            flpLeft.Controls.Add(lblCount);
            flpLeft.Dock = System.Windows.Forms.DockStyle.Left;
            flpLeft.Location = new System.Drawing.Point(0, 0);
            flpLeft.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            flpLeft.Name = "flpLeft";
            flpLeft.Padding = new System.Windows.Forms.Padding(4, 4, 9, 4);
            flpLeft.Size = new System.Drawing.Size(182, 78);
            flpLeft.TabIndex = 18;
            // 
            // lblCount
            // 
            lblCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblCount.ForeColor = System.Drawing.SystemColors.ControlText;
            lblCount.Location = new System.Drawing.Point(10, 4);
            lblCount.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblCount.Name = "lblCount";
            lblCount.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            lblCount.Size = new System.Drawing.Size(147, 50);
            lblCount.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblCount.TabIndex = 7;
            lblCount.Text = "0 tracks";
            lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flpButtons
            // 
            flpButtons.BackColor = System.Drawing.Color.Transparent;
            flpButtons.Controls.Add(btnRemove);
            flpButtons.Controls.Add(btnClear);
            flpButtons.Controls.Add(btnGenerate);
            flpButtons.Controls.Add(btnGenerateNow);
            flpButtons.Controls.Add(btnLeastMixedRoulette);
            flpButtons.Dock = System.Windows.Forms.DockStyle.Right;
            flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flpButtons.Location = new System.Drawing.Point(722, 0);
            flpButtons.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            flpButtons.Name = "flpButtons";
            flpButtons.Padding = new System.Windows.Forms.Padding(4, 4, 9, 4);
            flpButtons.Size = new System.Drawing.Size(881, 78);
            flpButtons.TabIndex = 16;
            // 
            // btnRemove
            // 
            btnRemove.Location = new System.Drawing.Point(749, 11);
            btnRemove.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new System.Drawing.Size(112, 57);
            btnRemove.TabIndex = 10;
            btnRemove.Text = "Remove";
            // 
            // btnClear
            // 
            btnClear.Location = new System.Drawing.Point(623, 11);
            btnClear.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(112, 57);
            btnClear.TabIndex = 11;
            btnClear.Text = "Clear";
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new System.Drawing.Point(471, 11);
            btnGenerate.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new System.Drawing.Size(138, 57);
            btnGenerate.TabIndex = 12;
            btnGenerate.Text = "Generate";
            // 
            // btnGenerateNow
            // 
            btnGenerateNow.Location = new System.Drawing.Point(274, 11);
            btnGenerateNow.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            btnGenerateNow.Name = "btnGenerateNow";
            btnGenerateNow.Size = new System.Drawing.Size(183, 57);
            btnGenerateNow.TabIndex = 13;
            btnGenerateNow.Text = "Generate Now";
            // 
            // btnLeastMixedRoulette
            // 
            btnLeastMixedRoulette.Location = new System.Drawing.Point(22, 11);
            btnLeastMixedRoulette.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            btnLeastMixedRoulette.Name = "btnLeastMixedRoulette";
            btnLeastMixedRoulette.Size = new System.Drawing.Size(238, 57);
            btnLeastMixedRoulette.TabIndex = 14;
            btnLeastMixedRoulette.Text = "Least Mixed Roulette";
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.Transparent;
            panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel1.Location = new System.Drawing.Point(0, 408);
            panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1607, 7);
            panel1.TabIndex = 14;
            // 
            // trackDetails
            // 
            trackDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            trackDetails.Location = new System.Drawing.Point(0, 415);
            trackDetails.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            trackDetails.Name = "trackDetails";
            trackDetails.Size = new System.Drawing.Size(1607, 111);
            trackDetails.TabIndex = 12;
            // 
            // mixableTracks
            // 
            mixableTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            mixableTracks.Location = new System.Drawing.Point(0, 0);
            mixableTracks.Margin = new System.Windows.Forms.Padding(11, 11, 11, 11);
            mixableTracks.Name = "mixableTracks";
            mixableTracks.Size = new System.Drawing.Size(1607, 199);
            mixableTracks.TabIndex = 0;
            // 
            // PlaylistControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlContentBackground);
            Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            Name = "PlaylistControl";
            Size = new System.Drawing.Size(1621, 744);
            mnuTrack.ResumeLayout(false);
            pnlContentBackground.ResumeLayout(false);
            (splTopBottom.Panel1).EndInit();
            splTopBottom.Panel1.ResumeLayout(false);
            (splTopBottom.Panel2).EndInit();
            splTopBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splTopBottom).EndInit();
            pnlTop.ResumeLayout(false);
            pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grdPlaylist).EndInit();
            pnlButtons.ResumeLayout(false);
            flpLeft.ResumeLayout(false);
            flpButtons.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip mnuTrack;
        private System.Windows.Forms.ToolStripMenuItem mnuPlay;
        private System.Windows.Forms.ToolStripSeparator mnuSep1;
        private System.Windows.Forms.ToolStripMenuItem mnuAddTrackToCollection;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveTrackFromCollection;
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
        private Halloumi.Common.Windows.Controls.Button btnGenerate;
        private Halloumi.Common.Windows.Controls.Button btnGenerateNow;
        private Halloumi.Common.Windows.Controls.Button btnLeastMixedRoulette;
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
