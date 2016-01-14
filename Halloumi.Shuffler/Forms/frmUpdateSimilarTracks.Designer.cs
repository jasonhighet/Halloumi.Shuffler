namespace Halloumi.Shuffler.Forms
{
    partial class FrmUpdateSimilarTracks
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
            var dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            var dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            var dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTrack = new Halloumi.Common.Windows.Controls.Panel();
            this.grdTracks = new Halloumi.Shuffler.Controls.DataGridView();
            this.colTrackDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrackAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrackLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrackNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mnuTrack = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuUpdateTrackTitle = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdateTrackDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdateShufflerDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuUpdateAudioData = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlTrack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTracks)).BeginInit();
            this.mnuTrack.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTrack
            // 
            this.pnlTrack.BackColor = System.Drawing.SystemColors.Control;
            this.pnlTrack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTrack.Controls.Add(this.grdTracks);
            this.pnlTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTrack.Location = new System.Drawing.Point(0, 0);
            this.pnlTrack.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlTrack.Name = "pnlTrack";
            this.pnlTrack.Padding = new System.Windows.Forms.Padding(1);
            this.pnlTrack.Size = new System.Drawing.Size(1079, 598);
            this.pnlTrack.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlTrack.TabIndex = 6;
            // 
            // grdTracks
            // 
            this.grdTracks.AllowUserToAddRows = false;
            this.grdTracks.AllowUserToDeleteRows = false;
            this.grdTracks.AllowUserToResizeColumns = false;
            this.grdTracks.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grdTracks.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdTracks.ColumnHeadersHeight = 26;
            this.grdTracks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTrackDescription,
            this.colTrackAlbum,
            this.colTrackLength,
            this.colTrackNumber});
            this.grdTracks.ContextMenuStrip = this.mnuTrack;
            this.grdTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTracks.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            this.grdTracks.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellList;
            this.grdTracks.HideOuterBorders = true;
            this.grdTracks.Location = new System.Drawing.Point(1, 1);
            this.grdTracks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grdTracks.MergeColor = System.Drawing.Color.Gainsboro;
            this.grdTracks.Name = "grdTracks";
            this.grdTracks.ReadOnly = true;
            this.grdTracks.RowHeadersVisible = false;
            this.grdTracks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdTracks.Size = new System.Drawing.Size(1077, 596);
            this.grdTracks.SortColumnIndex = 0;
            this.grdTracks.SortOrder = System.Windows.Forms.SortOrder.None;
            this.grdTracks.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellList;
            this.grdTracks.StateCommon.DataCell.Border.Color1 = System.Drawing.Color.White;
            this.grdTracks.StateCommon.DataCell.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grdTracks.TabIndex = 4;
            // 
            // colTrackDescription
            // 
            this.colTrackDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTrackDescription.DataPropertyName = "Description";
            this.colTrackDescription.HeaderText = "Description";
            this.colTrackDescription.Name = "colTrackDescription";
            this.colTrackDescription.ReadOnly = true;
            this.colTrackDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colTrackAlbum
            // 
            this.colTrackAlbum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTrackAlbum.DataPropertyName = "Album";
            this.colTrackAlbum.HeaderText = "Album";
            this.colTrackAlbum.Name = "colTrackAlbum";
            this.colTrackAlbum.ReadOnly = true;
            this.colTrackAlbum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colTrackLength
            // 
            this.colTrackLength.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTrackLength.DataPropertyName = "FullLengthFormatted";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colTrackLength.DefaultCellStyle = dataGridViewCellStyle2;
            this.colTrackLength.HeaderText = "Length";
            this.colTrackLength.Name = "colTrackLength";
            this.colTrackLength.ReadOnly = true;
            this.colTrackLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTrackLength.Width = 87;
            // 
            // colTrackNumber
            // 
            this.colTrackNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTrackNumber.DataPropertyName = "TrackNumberFormatted";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colTrackNumber.DefaultCellStyle = dataGridViewCellStyle3;
            this.colTrackNumber.HeaderText = "#";
            this.colTrackNumber.Name = "colTrackNumber";
            this.colTrackNumber.ReadOnly = true;
            this.colTrackNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTrackNumber.Width = 51;
            // 
            // mnuTrack
            // 
            this.mnuTrack.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.mnuTrack.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuTrack.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuUpdateTrackTitle,
            this.mnuUpdateTrackDetails,
            this.mnuUpdateShufflerDetails,
            this.toolStripSeparator2,
            this.mnuUpdateAudioData});
            this.mnuTrack.Name = "mnuTrack";
            this.mnuTrack.Size = new System.Drawing.Size(242, 106);
            // 
            // mnuUpdateTrackTitle
            // 
            this.mnuUpdateTrackTitle.Name = "mnuUpdateTrackTitle";
            this.mnuUpdateTrackTitle.Size = new System.Drawing.Size(241, 24);
            this.mnuUpdateTrackTitle.Text = "Update Track &Title";
            this.mnuUpdateTrackTitle.Click += new System.EventHandler(this.mnuUpdateTrackTitle_Click);
            // 
            // mnuUpdateTrackDetails
            // 
            this.mnuUpdateTrackDetails.Name = "mnuUpdateTrackDetails";
            this.mnuUpdateTrackDetails.Size = new System.Drawing.Size(241, 24);
            this.mnuUpdateTrackDetails.Text = "Update Track &Details";
            this.mnuUpdateTrackDetails.Click += new System.EventHandler(this.mnuUpdateTrackDetails_Click);
            // 
            // mnuUpdateShufflerDetails
            // 
            this.mnuUpdateShufflerDetails.Name = "mnuUpdateShufflerDetails";
            this.mnuUpdateShufflerDetails.Size = new System.Drawing.Size(241, 24);
            this.mnuUpdateShufflerDetails.Text = "Update &Shuffler Details...";
            this.mnuUpdateShufflerDetails.Click += new System.EventHandler(this.mnuUpdateShufflerDetails_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(238, 6);
            // 
            // mnuUpdateAudioData
            // 
            this.mnuUpdateAudioData.Name = "mnuUpdateAudioData";
            this.mnuUpdateAudioData.Size = new System.Drawing.Size(241, 24);
            this.mnuUpdateAudioData.Text = "Update &Audio Data";
            this.mnuUpdateAudioData.Click += new System.EventHandler(this.mnuUpdateAudioData_Click);
            // 
            // frmUpdateSimilarTracks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1079, 598);
            this.Controls.Add(this.pnlTrack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmUpdateSimilarTracks";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Update Similar Tracks";
            this.UseApplicationIcon = true;
            this.Load += new System.EventHandler(this.frmSimilarTracks_Load);
            this.pnlTrack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTracks)).EndInit();
            this.mnuTrack.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlTrack;
        private Halloumi.Shuffler.Controls.DataGridView grdTracks;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackAlbum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackNumber;
        private System.Windows.Forms.ContextMenuStrip mnuTrack;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateTrackDetails;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateShufflerDetails;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateAudioData;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateTrackTitle;
    }
}