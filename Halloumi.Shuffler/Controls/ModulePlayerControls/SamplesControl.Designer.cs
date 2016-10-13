namespace Halloumi.Shuffler.Controls.ModulePlayerControls
{
    partial class SamplesControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel3 = new Halloumi.Common.Windows.Controls.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPlay = new Halloumi.Common.Windows.Controls.Button();
            this.btnStop = new Halloumi.Common.Windows.Controls.Button();
            this.btnEdit = new Halloumi.Common.Windows.Controls.Button();
            this.btnImport = new Halloumi.Common.Windows.Controls.Button();
            this.btnExport = new Halloumi.Common.Windows.Controls.Button();
            this.panel1 = new Halloumi.Common.Windows.Controls.Panel();
            this.panel2 = new Halloumi.Common.Windows.Controls.Panel();
            this.grdSamples = new Halloumi.Shuffler.Controls.DataGridView();
            this.colSampleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBPM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSamples)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.flowLayoutPanel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(515, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(118, 362);
            this.panel3.Style = Halloumi.Common.Windows.Controls.PanelStyle.Background;
            this.panel3.TabIndex = 4;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnPlay);
            this.flowLayoutPanel1.Controls.Add(this.btnStop);
            this.flowLayoutPanel1.Controls.Add(this.btnEdit);
            this.flowLayoutPanel1.Controls.Add(this.btnImport);
            this.flowLayoutPanel1.Controls.Add(this.btnExport);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(118, 362);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(9, 9);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(100, 31);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "Play";
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(9, 48);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 31);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(9, 87);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 31);
            this.btnEdit.TabIndex = 0;
            this.btnEdit.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(9, 126);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(100, 31);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Import";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(9, 165);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 31);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(515, 362);
            this.panel1.Style = Halloumi.Common.Windows.Controls.PanelStyle.Background;
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.grdSamples);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(5, 5);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(1);
            this.panel2.Size = new System.Drawing.Size(505, 352);
            this.panel2.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.panel2.TabIndex = 6;
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
            this.colSampleName,
            this.colBPM});
            this.grdSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSamples.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            this.grdSamples.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellList;
            this.grdSamples.Location = new System.Drawing.Point(1, 1);
            this.grdSamples.MergeColor = System.Drawing.Color.Gainsboro;
            this.grdSamples.Name = "grdSamples";
            this.grdSamples.ReadOnly = true;
            this.grdSamples.RowHeadersVisible = false;
            this.grdSamples.RowTemplate.Height = 24;
            this.grdSamples.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdSamples.Size = new System.Drawing.Size(503, 350);
            this.grdSamples.SortColumnIndex = -1;
            this.grdSamples.SortOrder = System.Windows.Forms.SortOrder.None;
            this.grdSamples.TabIndex = 1;
            this.grdSamples.VirtualMode = true;
            // 
            // colSampleName
            // 
            this.colSampleName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSampleName.DataPropertyName = "Description";
            this.colSampleName.HeaderText = "Sample";
            this.colSampleName.Name = "colSampleName";
            this.colSampleName.ReadOnly = true;
            // 
            // colBPM
            // 
            this.colBPM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colBPM.DataPropertyName = "BPM";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colBPM.DefaultCellStyle = dataGridViewCellStyle2;
            this.colBPM.HeaderText = "BPM";
            this.colBPM.Name = "colBPM";
            this.colBPM.ReadOnly = true;
            this.colBPM.Width = 72;
            // 
            // SamplesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Name = "SamplesControl";
            this.Size = new System.Drawing.Size(633, 362);
            this.panel3.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSamples)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Windows.Controls.Panel panel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Common.Windows.Controls.Button btnEdit;
        private Common.Windows.Controls.Button btnPlay;
        private Common.Windows.Controls.Button btnStop;
        private Common.Windows.Controls.Button btnExport;
        private Common.Windows.Controls.Button btnImport;
        private Common.Windows.Controls.Panel panel1;
        private Common.Windows.Controls.Panel panel2;
        private DataGridView grdSamples;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSampleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBPM;
    }
}
