﻿namespace Halloumi.Shuffler.Controls
{
    partial class SampleLibraryControl
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
            this.pnlBackground2 = new Halloumi.Common.Windows.Controls.Panel();
            this.pnlLibraryDetails = new Halloumi.Common.Windows.Controls.Panel();
            this.pnlTrack = new Halloumi.Common.Windows.Controls.Panel();
            this.grdSamples = new Halloumi.Shuffler.Controls.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnEditSample = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditTags = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCalculateKey = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopySample = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlSampleDetails = new Halloumi.Common.Windows.Controls.Panel();
            this.pnlDivider = new Halloumi.Common.Windows.Controls.Panel();
            this.flpToolbarRight = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFilter = new Halloumi.Common.Windows.Controls.Label();
            this.txtSearch = new Halloumi.Common.Windows.Controls.TextBox();
            this.label5 = new Halloumi.Common.Windows.Controls.Label();
            this.txtMinBPM = new Halloumi.Common.Windows.Controls.TextBox();
            this.label4 = new Halloumi.Common.Windows.Controls.Label();
            this.txtMaxBPM = new Halloumi.Common.Windows.Controls.TextBox();
            this.label3 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbKey = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label1 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbLoopType = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label2 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbTag = new Halloumi.Common.Windows.Controls.ComboBox();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBPM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlBackground2.SuspendLayout();
            this.pnlLibraryDetails.SuspendLayout();
            this.pnlTrack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSamples)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.pnlDivider.SuspendLayout();
            this.flpToolbarRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbKey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLoopType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTag)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBackground2
            // 
            this.pnlBackground2.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBackground2.Controls.Add(this.pnlLibraryDetails);
            this.pnlBackground2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground2.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground2.Name = "pnlBackground2";
            this.pnlBackground2.Padding = new System.Windows.Forms.Padding(5);
            this.pnlBackground2.Size = new System.Drawing.Size(1139, 612);
            this.pnlBackground2.TabIndex = 13;
            // 
            // pnlLibraryDetails
            // 
            this.pnlLibraryDetails.BackColor = System.Drawing.SystemColors.Control;
            this.pnlLibraryDetails.Controls.Add(this.pnlTrack);
            this.pnlLibraryDetails.Controls.Add(this.pnlSampleDetails);
            this.pnlLibraryDetails.Controls.Add(this.pnlDivider);
            this.pnlLibraryDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLibraryDetails.Location = new System.Drawing.Point(5, 5);
            this.pnlLibraryDetails.Margin = new System.Windows.Forms.Padding(4);
            this.pnlLibraryDetails.Name = "pnlLibraryDetails";
            this.pnlLibraryDetails.Size = new System.Drawing.Size(1129, 602);
            this.pnlLibraryDetails.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlLibraryDetails.TabIndex = 12;
            // 
            // pnlTrack
            // 
            this.pnlTrack.BackColor = System.Drawing.SystemColors.Control;
            this.pnlTrack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTrack.Controls.Add(this.grdSamples);
            this.pnlTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTrack.Location = new System.Drawing.Point(0, 37);
            this.pnlTrack.Margin = new System.Windows.Forms.Padding(4);
            this.pnlTrack.Name = "pnlTrack";
            this.pnlTrack.Padding = new System.Windows.Forms.Padding(1);
            this.pnlTrack.Size = new System.Drawing.Size(1129, 488);
            this.pnlTrack.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlTrack.TabIndex = 57;
            // 
            // grdSamples
            // 
            this.grdSamples.AllowUserToAddRows = false;
            this.grdSamples.AllowUserToDeleteRows = false;
            this.grdSamples.AllowUserToResizeColumns = false;
            this.grdSamples.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grdSamples.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdSamples.ColumnHeadersHeight = 26;
            this.grdSamples.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            this.grdSamples.ContextMenuStrip = this.contextMenuStrip;
            this.grdSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSamples.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            this.grdSamples.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellList;
            this.grdSamples.HideOuterBorders = true;
            this.grdSamples.Location = new System.Drawing.Point(1, 1);
            this.grdSamples.Margin = new System.Windows.Forms.Padding(4);
            this.grdSamples.MergeColor = System.Drawing.Color.Gainsboro;
            this.grdSamples.Name = "grdSamples";
            this.grdSamples.ReadOnly = true;
            this.grdSamples.RowHeadersVisible = false;
            this.grdSamples.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdSamples.RowTemplate.Height = 24;
            this.grdSamples.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdSamples.Size = new System.Drawing.Size(1127, 486);
            this.grdSamples.SortColumnIndex = 0;
            this.grdSamples.SortOrder = System.Windows.Forms.SortOrder.None;
            this.grdSamples.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellList;
            this.grdSamples.StateCommon.DataCell.Border.Color1 = System.Drawing.Color.White;
            this.grdSamples.StateCommon.DataCell.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grdSamples.TabIndex = 53;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Description";
            this.dataGridViewTextBoxColumn1.HeaderText = "Description";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "BPM";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn2.HeaderText = "BPM";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dataGridViewTextBoxColumn2.Width = 66;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Key";
            this.dataGridViewTextBoxColumn3.HeaderText = "Key";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dataGridViewTextBoxColumn3.Width = 60;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "LengthFormatted";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn4.HeaderText = "Length";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dataGridViewTextBoxColumn4.Width = 81;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Tags";
            this.dataGridViewTextBoxColumn5.HeaderText = "Tags";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dataGridViewTextBoxColumn5.Width = 67;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnEditSample,
            this.mnuEditTags,
            this.mnuCalculateKey,
            this.mnuCopySample});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(183, 128);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // mnEditSample
            // 
            this.mnEditSample.Name = "mnEditSample";
            this.mnEditSample.Size = new System.Drawing.Size(182, 24);
            this.mnEditSample.Text = "Edit &Sample";
            this.mnEditSample.Click += new System.EventHandler(this.mnEditSample_Click);
            // 
            // mnuEditTags
            // 
            this.mnuEditTags.Name = "mnuEditTags";
            this.mnuEditTags.Size = new System.Drawing.Size(182, 24);
            this.mnuEditTags.Text = "Edit &Tags";
            this.mnuEditTags.Click += new System.EventHandler(this.mnuEditTags_Click);
            // 
            // mnuCalculateKey
            // 
            this.mnuCalculateKey.Name = "mnuCalculateKey";
            this.mnuCalculateKey.Size = new System.Drawing.Size(182, 24);
            this.mnuCalculateKey.Text = "Calculate &Key";
            this.mnuCalculateKey.Click += new System.EventHandler(this.mnuCalculateKey_Click);
            // 
            // mnuCopySample
            // 
            this.mnuCopySample.Name = "mnuCopySample";
            this.mnuCopySample.Size = new System.Drawing.Size(182, 24);
            this.mnuCopySample.Text = "&Copy Sample(s)";
            this.mnuCopySample.Click += new System.EventHandler(this.mnuCopySample_Click);
            // 
            // pnlSampleDetails
            // 
            this.pnlSampleDetails.BackColor = System.Drawing.Color.Transparent;
            this.pnlSampleDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSampleDetails.Location = new System.Drawing.Point(0, 525);
            this.pnlSampleDetails.Name = "pnlSampleDetails";
            this.pnlSampleDetails.Size = new System.Drawing.Size(1129, 77);
            this.pnlSampleDetails.TabIndex = 56;
            // 
            // pnlDivider
            // 
            this.pnlDivider.BackColor = System.Drawing.SystemColors.Control;
            this.pnlDivider.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDivider.Controls.Add(this.flpToolbarRight);
            this.pnlDivider.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDivider.Location = new System.Drawing.Point(0, 0);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.Padding = new System.Windows.Forms.Padding(5, 3, 5, 5);
            this.pnlDivider.Size = new System.Drawing.Size(1129, 37);
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
            this.flpToolbarRight.Controls.Add(this.label3);
            this.flpToolbarRight.Controls.Add(this.cmbKey);
            this.flpToolbarRight.Controls.Add(this.label1);
            this.flpToolbarRight.Controls.Add(this.cmbLoopType);
            this.flpToolbarRight.Controls.Add(this.label2);
            this.flpToolbarRight.Controls.Add(this.cmbTag);
            this.flpToolbarRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpToolbarRight.Location = new System.Drawing.Point(5, 3);
            this.flpToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            this.flpToolbarRight.Name = "flpToolbarRight";
            this.flpToolbarRight.Size = new System.Drawing.Size(1119, 29);
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
            this.lblFilter.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblFilter.Size = new System.Drawing.Size(56, 26);
            this.lblFilter.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblFilter.TabIndex = 5;
            this.lblFilter.Text = "Search:";
            // 
            // txtSearch
            // 
            this.txtSearch.ErrorProvider = null;
            this.txtSearch.Location = new System.Drawing.Point(60, 4);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtSearch.MaximumValue = 2147483647D;
            this.txtSearch.MinimumValue = -2147483648D;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(279, 24);
            this.txtSearch.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(343, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label5.Size = new System.Drawing.Size(37, 26);
            this.label5.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label5.TabIndex = 13;
            this.label5.Text = "Min:";
            // 
            // txtMinBPM
            // 
            this.txtMinBPM.EntryType = Halloumi.Common.Windows.Controls.TextBox.TextEntryType.Integer;
            this.txtMinBPM.ErrorProvider = null;
            this.txtMinBPM.Location = new System.Drawing.Point(384, 4);
            this.txtMinBPM.Margin = new System.Windows.Forms.Padding(4);
            this.txtMinBPM.MaximumValue = 2147483647D;
            this.txtMinBPM.MinimumValue = -2147483648D;
            this.txtMinBPM.Name = "txtMinBPM";
            this.txtMinBPM.Size = new System.Drawing.Size(35, 24);
            this.txtMinBPM.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(423, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label4.Size = new System.Drawing.Size(40, 26);
            this.label4.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label4.TabIndex = 11;
            this.label4.Text = "Max:";
            // 
            // txtMaxBPM
            // 
            this.txtMaxBPM.EntryType = Halloumi.Common.Windows.Controls.TextBox.TextEntryType.Integer;
            this.txtMaxBPM.ErrorProvider = null;
            this.txtMaxBPM.Location = new System.Drawing.Point(467, 4);
            this.txtMaxBPM.Margin = new System.Windows.Forms.Padding(4);
            this.txtMaxBPM.MaximumValue = 2147483647D;
            this.txtMaxBPM.MinimumValue = -2147483648D;
            this.txtMaxBPM.Name = "txtMaxBPM";
            this.txtMaxBPM.Size = new System.Drawing.Size(35, 24);
            this.txtMaxBPM.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(506, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label3.Size = new System.Drawing.Size(36, 26);
            this.label3.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label3.TabIndex = 9;
            this.label3.Text = "Key:";
            // 
            // cmbKey
            // 
            this.cmbKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKey.DropDownWidth = 121;
            this.cmbKey.ErrorProvider = null;
            this.cmbKey.Items.AddRange(new object[] {
            "",
            "Yes",
            "No"});
            this.cmbKey.Location = new System.Drawing.Point(546, 4);
            this.cmbKey.Margin = new System.Windows.Forms.Padding(4);
            this.cmbKey.Name = "cmbKey";
            this.cmbKey.Size = new System.Drawing.Size(81, 25);
            this.cmbKey.TabIndex = 6;
            this.cmbKey.SelectedIndexChanged += new System.EventHandler(this.cmbKey_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(631, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label1.Size = new System.Drawing.Size(82, 26);
            this.label1.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label1.TabIndex = 15;
            this.label1.Text = "Loop Type:";
            // 
            // cmbLoopType
            // 
            this.cmbLoopType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoopType.DropDownWidth = 121;
            this.cmbLoopType.ErrorProvider = null;
            this.cmbLoopType.Items.AddRange(new object[] {
            "",
            "Yes",
            "No"});
            this.cmbLoopType.Location = new System.Drawing.Point(717, 4);
            this.cmbLoopType.Margin = new System.Windows.Forms.Padding(4);
            this.cmbLoopType.Name = "cmbLoopType";
            this.cmbLoopType.Size = new System.Drawing.Size(160, 25);
            this.cmbLoopType.TabIndex = 14;
            this.cmbLoopType.SelectedIndexChanged += new System.EventHandler(this.cmbLoopType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(881, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label2.Size = new System.Drawing.Size(37, 26);
            this.label2.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label2.TabIndex = 17;
            this.label2.Text = "Tag:";
            // 
            // cmbTag
            // 
            this.cmbTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTag.DropDownWidth = 121;
            this.cmbTag.ErrorProvider = null;
            this.cmbTag.Items.AddRange(new object[] {
            "",
            "Yes",
            "No"});
            this.cmbTag.Location = new System.Drawing.Point(922, 4);
            this.cmbTag.Margin = new System.Windows.Forms.Padding(4);
            this.cmbTag.Name = "cmbTag";
            this.cmbTag.Size = new System.Drawing.Size(147, 25);
            this.cmbTag.TabIndex = 16;
            // 
            // colDescription
            // 
            this.colDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDescription.DataPropertyName = "Description";
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            this.colDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colBPM
            // 
            this.colBPM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colBPM.DataPropertyName = "BPM";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.colBPM.DefaultCellStyle = dataGridViewCellStyle4;
            this.colBPM.HeaderText = "BPM";
            this.colBPM.Name = "colBPM";
            this.colBPM.ReadOnly = true;
            this.colBPM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colKey
            // 
            this.colKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colKey.DataPropertyName = "Key";
            this.colKey.HeaderText = "Key";
            this.colKey.Name = "colKey";
            this.colKey.ReadOnly = true;
            this.colKey.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colLength
            // 
            this.colLength.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLength.DataPropertyName = "LengthFormatted";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colLength.DefaultCellStyle = dataGridViewCellStyle5;
            this.colLength.HeaderText = "Length";
            this.colLength.Name = "colLength";
            this.colLength.ReadOnly = true;
            this.colLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colTags
            // 
            this.colTags.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTags.DataPropertyName = "Tags";
            this.colTags.HeaderText = "Tags";
            this.colTags.Name = "colTags";
            this.colTags.ReadOnly = true;
            this.colTags.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // SampleLibraryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlBackground2);
            this.Name = "SampleLibraryControl";
            this.Size = new System.Drawing.Size(1139, 612);
            this.pnlBackground2.ResumeLayout(false);
            this.pnlLibraryDetails.ResumeLayout(false);
            this.pnlTrack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSamples)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.pnlDivider.ResumeLayout(false);
            this.flpToolbarRight.ResumeLayout(false);
            this.flpToolbarRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbKey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLoopType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTag)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlBackground2;
        private Halloumi.Common.Windows.Controls.Panel pnlLibraryDetails;
        private Halloumi.Common.Windows.Controls.Panel pnlDivider;
        private System.Windows.Forms.FlowLayoutPanel flpToolbarRight;
        private Halloumi.Common.Windows.Controls.Label lblFilter;
        private Halloumi.Common.Windows.Controls.TextBox txtSearch;
        private Halloumi.Common.Windows.Controls.Label label5;
        private Halloumi.Common.Windows.Controls.TextBox txtMinBPM;
        private Halloumi.Common.Windows.Controls.Label label4;
        private Halloumi.Common.Windows.Controls.TextBox txtMaxBPM;
        private Halloumi.Common.Windows.Controls.Label label3;
        private Halloumi.Common.Windows.Controls.ComboBox cmbKey;
        private Halloumi.Common.Windows.Controls.Panel pnlTrack;
        private DataGridView grdSamples;
        private Halloumi.Common.Windows.Controls.Panel pnlSampleDetails;
        private Halloumi.Common.Windows.Controls.Label label1;
        private Halloumi.Common.Windows.Controls.ComboBox cmbLoopType;
        private Halloumi.Common.Windows.Controls.Label label2;
        private Halloumi.Common.Windows.Controls.ComboBox cmbTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBPM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTags;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnEditSample;
        private System.Windows.Forms.ToolStripMenuItem mnuEditTags;
        private System.Windows.Forms.ToolStripMenuItem mnuCalculateKey;
        private System.Windows.Forms.ToolStripMenuItem mnuCopySample;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    }
}
