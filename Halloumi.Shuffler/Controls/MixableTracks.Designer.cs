namespace Halloumi.Shuffler.Controls
{
    partial class MixableTracks
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
            var dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            var dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            var dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlContentBackground = new Halloumi.Common.Windows.Controls.Panel();
            this.pnlGridBorder = new Halloumi.Common.Windows.Controls.Panel();
            this.linLine = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.pnlButtons = new Halloumi.Common.Windows.Controls.Panel();
            this.lblCount = new Halloumi.Common.Windows.Controls.Label();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.cmbView = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label2 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbKeyRank = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label1 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbRank = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label3 = new Halloumi.Common.Windows.Controls.Label();
            this.chkExcludeQueued = new Halloumi.Common.Windows.Controls.CheckBox();
            this.grdMixableTracks = new Halloumi.Shuffler.Controls.DataGridView();
            this.colTrackDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBPM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMixRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKeyDiff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlContentBackground.SuspendLayout();
            this.pnlGridBorder.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.flpButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbKeyRank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMixableTracks)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlContentBackground
            // 
            this.pnlContentBackground.BackColor = System.Drawing.SystemColors.Control;
            this.pnlContentBackground.Controls.Add(this.pnlGridBorder);
            this.pnlContentBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContentBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlContentBackground.Margin = new System.Windows.Forms.Padding(4);
            this.pnlContentBackground.Name = "pnlContentBackground";
            this.pnlContentBackground.Size = new System.Drawing.Size(881, 222);
            this.pnlContentBackground.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlContentBackground.TabIndex = 12;
            // 
            // pnlGridBorder
            // 
            this.pnlGridBorder.BackColor = System.Drawing.SystemColors.Control;
            this.pnlGridBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGridBorder.Controls.Add(this.grdMixableTracks);
            this.pnlGridBorder.Controls.Add(this.linLine);
            this.pnlGridBorder.Controls.Add(this.pnlButtons);
            this.pnlGridBorder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridBorder.Location = new System.Drawing.Point(0, 0);
            this.pnlGridBorder.Margin = new System.Windows.Forms.Padding(4);
            this.pnlGridBorder.Name = "pnlGridBorder";
            this.pnlGridBorder.Padding = new System.Windows.Forms.Padding(1);
            this.pnlGridBorder.Size = new System.Drawing.Size(881, 222);
            this.pnlGridBorder.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlGridBorder.TabIndex = 11;
            // 
            // linLine
            // 
            this.linLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.linLine.Location = new System.Drawing.Point(1, 182);
            this.linLine.Margin = new System.Windows.Forms.Padding(4);
            this.linLine.Name = "linLine";
            this.linLine.Size = new System.Drawing.Size(879, 2);
            this.linLine.TabIndex = 50;
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.lblCount);
            this.pnlButtons.Controls.Add(this.flpButtons);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(1, 184);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(4);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(879, 37);
            this.pnlButtons.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlButtons.TabIndex = 49;
            // 
            // lblCount
            // 
            this.lblCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCount.Location = new System.Drawing.Point(0, 0);
            this.lblCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.lblCount.Size = new System.Drawing.Size(89, 37);
            this.lblCount.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblCount.TabIndex = 17;
            this.lblCount.Text = "0 tracks";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flpButtons
            // 
            this.flpButtons.BackColor = System.Drawing.Color.Transparent;
            this.flpButtons.Controls.Add(this.cmbView);
            this.flpButtons.Controls.Add(this.label2);
            this.flpButtons.Controls.Add(this.cmbKeyRank);
            this.flpButtons.Controls.Add(this.label1);
            this.flpButtons.Controls.Add(this.cmbRank);
            this.flpButtons.Controls.Add(this.label3);
            this.flpButtons.Controls.Add(this.chkExcludeQueued);
            this.flpButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpButtons.Location = new System.Drawing.Point(97, 0);
            this.flpButtons.Margin = new System.Windows.Forms.Padding(4);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Padding = new System.Windows.Forms.Padding(3, 2, 7, 2);
            this.flpButtons.Size = new System.Drawing.Size(782, 37);
            this.flpButtons.TabIndex = 16;
            // 
            // cmbView
            // 
            this.cmbView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbView.DropDownWidth = 72;
            this.cmbView.ErrorProvider = null;
            this.cmbView.Items.AddRange(new object[] {
            "To Tracks",
            "From Tracks"});
            this.cmbView.Location = new System.Drawing.Point(649, 6);
            this.cmbView.Margin = new System.Windows.Forms.Padding(4);
            this.cmbView.Name = "cmbView";
            this.cmbView.Size = new System.Drawing.Size(119, 25);
            this.cmbView.TabIndex = 13;
            this.cmbView.SelectedIndexChanged += new System.EventHandler(this.cmbView_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(587, 2);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.label2.Size = new System.Drawing.Size(54, 33);
            this.label2.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label2.TabIndex = 12;
            this.label2.Text = "View:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbKeyRank
            // 
            this.cmbKeyRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyRank.DropDownWidth = 72;
            this.cmbKeyRank.ErrorProvider = null;
            this.cmbKeyRank.Items.AddRange(new object[] {
            "All",
            "Very Good+",
            "Good+",
            "Bearable+",
            "Not Good"});
            this.cmbKeyRank.Location = new System.Drawing.Point(478, 6);
            this.cmbKeyRank.Margin = new System.Windows.Forms.Padding(4);
            this.cmbKeyRank.Name = "cmbKeyRank";
            this.cmbKeyRank.Size = new System.Drawing.Size(101, 25);
            this.cmbKeyRank.TabIndex = 11;
            this.cmbKeyRank.SelectedIndexChanged += new System.EventHandler(this.cmbKeyRank_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(388, 2);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(82, 33);
            this.label1.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label1.TabIndex = 10;
            this.label1.Text = "Key Rank:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbRank
            // 
            this.cmbRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRank.DropDownWidth = 72;
            this.cmbRank.ErrorProvider = null;
            this.cmbRank.Items.AddRange(new object[] {
            "All",
            "Good+",
            "Bearable+",
            "Unranked",
            "Forbidden"});
            this.cmbRank.Location = new System.Drawing.Point(279, 6);
            this.cmbRank.Margin = new System.Windows.Forms.Padding(4);
            this.cmbRank.Name = "cmbRank";
            this.cmbRank.Size = new System.Drawing.Size(101, 25);
            this.cmbRank.TabIndex = 8;
            this.cmbRank.SelectedIndexChanged += new System.EventHandler(this.cmbRank_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(214, 2);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.label3.Size = new System.Drawing.Size(57, 33);
            this.label3.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label3.TabIndex = 7;
            this.label3.Text = "Rank:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkExcludeQueued
            // 
            this.chkExcludeQueued.Location = new System.Drawing.Point(92, 7);
            this.chkExcludeQueued.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.chkExcludeQueued.Name = "chkExcludeQueued";
            this.chkExcludeQueued.Size = new System.Drawing.Size(115, 24);
            this.chkExcludeQueued.TabIndex = 9;
            this.chkExcludeQueued.Values.Text = "Hide Queued";
            this.chkExcludeQueued.CheckedChanged += new System.EventHandler(this.chkExcludeQueued_CheckedChanged);
            // 
            // grdMixableTracks
            // 
            this.grdMixableTracks.AllowUserToAddRows = false;
            this.grdMixableTracks.AllowUserToDeleteRows = false;
            this.grdMixableTracks.AllowUserToResizeColumns = false;
            this.grdMixableTracks.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grdMixableTracks.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdMixableTracks.ColumnHeadersHeight = 26;
            this.grdMixableTracks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTrackDescription,
            this.colBPM,
            this.colDiff,
            this.colMixRank,
            this.colRank,
            this.colKey,
            this.colKeyDiff});
            this.grdMixableTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMixableTracks.HideOuterBorders = true;
            this.grdMixableTracks.Location = new System.Drawing.Point(1, 1);
            this.grdMixableTracks.Margin = new System.Windows.Forms.Padding(4);
            this.grdMixableTracks.MergeColor = System.Drawing.Color.Gainsboro;
            this.grdMixableTracks.Name = "grdMixableTracks";
            this.grdMixableTracks.ReadOnly = true;
            this.grdMixableTracks.RowHeadersVisible = false;
            this.grdMixableTracks.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grdMixableTracks.RowTemplate.Height = 24;
            this.grdMixableTracks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdMixableTracks.Size = new System.Drawing.Size(879, 181);
            this.grdMixableTracks.SortColumnIndex = -1;
            this.grdMixableTracks.SortOrder = System.Windows.Forms.SortOrder.None;
            this.grdMixableTracks.StateCommon.Background.Color1 = System.Drawing.Color.White;
            this.grdMixableTracks.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.grdMixableTracks.StateCommon.DataCell.Border.Color1 = System.Drawing.Color.White;
            this.grdMixableTracks.StateCommon.DataCell.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grdMixableTracks.TabIndex = 51;
            // 
            // colTrackDescription
            // 
            this.colTrackDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTrackDescription.DataPropertyName = "Description";
            this.colTrackDescription.HeaderText = "Track";
            this.colTrackDescription.Name = "colTrackDescription";
            this.colTrackDescription.ReadOnly = true;
            this.colTrackDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colBPM
            // 
            this.colBPM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colBPM.DataPropertyName = "BPM";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.colBPM.DefaultCellStyle = dataGridViewCellStyle2;
            this.colBPM.HeaderText = "BPM";
            this.colBPM.Name = "colBPM";
            this.colBPM.ReadOnly = true;
            this.colBPM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colBPM.Width = 66;
            // 
            // colDiff
            // 
            this.colDiff.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colDiff.DataPropertyName = "Diff";
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.colDiff.DefaultCellStyle = dataGridViewCellStyle3;
            this.colDiff.HeaderText = "Diff";
            this.colDiff.Name = "colDiff";
            this.colDiff.ReadOnly = true;
            this.colDiff.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colDiff.Visible = false;
            this.colDiff.Width = 61;
            // 
            // colMixRank
            // 
            this.colMixRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMixRank.DataPropertyName = "MixRankDescription";
            this.colMixRank.HeaderText = "Mix";
            this.colMixRank.Name = "colMixRank";
            this.colMixRank.ReadOnly = true;
            this.colMixRank.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colMixRank.Width = 60;
            // 
            // colRank
            // 
            this.colRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colRank.DataPropertyName = "RankDescription";
            this.colRank.HeaderText = "Rank";
            this.colRank.Name = "colRank";
            this.colRank.ReadOnly = true;
            this.colRank.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colRank.Width = 68;
            // 
            // colKey
            // 
            this.colKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colKey.DataPropertyName = "Key";
            this.colKey.HeaderText = "Key";
            this.colKey.Name = "colKey";
            this.colKey.ReadOnly = true;
            this.colKey.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colKey.Width = 60;
            // 
            // colKeyDiff
            // 
            this.colKeyDiff.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colKeyDiff.DataPropertyName = "KeyRankDescription";
            this.colKeyDiff.HeaderText = "Key Rank";
            this.colKeyDiff.Name = "colKeyDiff";
            this.colKeyDiff.ReadOnly = true;
            this.colKeyDiff.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colKeyDiff.Width = 96;
            // 
            // MixableTracks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlContentBackground);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MixableTracks";
            this.Size = new System.Drawing.Size(881, 222);
            this.pnlContentBackground.ResumeLayout(false);
            this.pnlGridBorder.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.flpButtons.ResumeLayout(false);
            this.flpButtons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbKeyRank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMixableTracks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlContentBackground;
        private Halloumi.Common.Windows.Controls.Panel pnlGridBorder;
        private DataGridView grdMixableTracks;
        private Halloumi.Common.Windows.Controls.BeveledLine linLine;
        private Halloumi.Common.Windows.Controls.Panel pnlButtons;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private Halloumi.Common.Windows.Controls.ComboBox cmbRank;
        private Halloumi.Common.Windows.Controls.Label label3;
        private Halloumi.Common.Windows.Controls.CheckBox chkExcludeQueued;
        private Halloumi.Common.Windows.Controls.Label lblCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBPM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiff;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMixRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKeyDiff;
        private Halloumi.Common.Windows.Controls.ComboBox cmbKeyRank;
        private Halloumi.Common.Windows.Controls.Label label1;
        private Halloumi.Common.Windows.Controls.ComboBox cmbView;
        private Halloumi.Common.Windows.Controls.Label label2;


    }
}
