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
            this.panel3 = new Halloumi.Common.Windows.Controls.Panel();
            this.panel2 = new Halloumi.Common.Windows.Controls.Panel();
            this.grdSamples = new Halloumi.Common.Windows.Controls.DataGridView();
            this.colSampleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new Halloumi.Common.Windows.Controls.Button();
            this.button2 = new Halloumi.Common.Windows.Controls.Button();
            this.button4 = new Halloumi.Common.Windows.Controls.Button();
            this.button3 = new Halloumi.Common.Windows.Controls.Button();
            this.button5 = new Halloumi.Common.Windows.Controls.Button();
            this.button6 = new Halloumi.Common.Windows.Controls.Button();
            this.button7 = new Halloumi.Common.Windows.Controls.Button();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSamples)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.flowLayoutPanel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(524, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(109, 362);
            this.panel3.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.panel3.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.grdSamples);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(1);
            this.panel2.Size = new System.Drawing.Size(524, 362);
            this.panel2.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.panel2.TabIndex = 5;
            // 
            // grdSamples
            // 
            this.grdSamples.AllowUserToAddRows = false;
            this.grdSamples.AllowUserToDeleteRows = false;
            this.grdSamples.AllowUserToResizeRows = false;
            this.grdSamples.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSampleName});
            this.grdSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSamples.Location = new System.Drawing.Point(1, 1);
            this.grdSamples.MergeColor = System.Drawing.Color.Gainsboro;
            this.grdSamples.Name = "grdSamples";
            this.grdSamples.ReadOnly = true;
            this.grdSamples.RowHeadersVisible = false;
            this.grdSamples.RowTemplate.Height = 24;
            this.grdSamples.Size = new System.Drawing.Size(522, 360);
            this.grdSamples.TabIndex = 1;
            // 
            // colSampleName
            // 
            this.colSampleName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSampleName.HeaderText = "Sample";
            this.colSampleName.Name = "colSampleName";
            this.colSampleName.ReadOnly = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.button4);
            this.flowLayoutPanel1.Controls.Add(this.button3);
            this.flowLayoutPanel1.Controls.Add(this.button5);
            this.flowLayoutPanel1.Controls.Add(this.button6);
            this.flowLayoutPanel1.Controls.Add(this.button7);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(109, 362);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(4, 4);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 31);
            this.button1.TabIndex = 0;
            this.button1.Text = "Edit";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(4, 43);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 31);
            this.button2.TabIndex = 1;
            this.button2.Text = "Play";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(4, 82);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 31);
            this.button4.TabIndex = 2;
            this.button4.Text = "Stop";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(4, 121);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 31);
            this.button3.TabIndex = 3;
            this.button3.Text = "Export";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(4, 160);
            this.button5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 31);
            this.button5.TabIndex = 4;
            this.button5.Text = "Add";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(4, 199);
            this.button6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(100, 31);
            this.button6.TabIndex = 5;
            this.button6.Text = "Remove";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(4, 238);
            this.button7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(100, 31);
            this.button7.TabIndex = 6;
            this.button7.Text = "Import";
            // 
            // SamplesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Name = "SamplesControl";
            this.Size = new System.Drawing.Size(633, 362);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSamples)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Windows.Controls.Panel panel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Common.Windows.Controls.Button button1;
        private Common.Windows.Controls.Button button2;
        private Common.Windows.Controls.Button button4;
        private Common.Windows.Controls.Button button3;
        private Common.Windows.Controls.Button button5;
        private Common.Windows.Controls.Button button6;
        private Common.Windows.Controls.Button button7;
        private Common.Windows.Controls.Panel panel2;
        private Common.Windows.Controls.DataGridView grdSamples;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSampleName;
    }
}
