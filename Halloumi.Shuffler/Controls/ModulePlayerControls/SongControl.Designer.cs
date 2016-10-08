using System;

namespace Halloumi.Shuffler.Controls.ModulePlayerControls
{
    partial class SongControl
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
            this.panel1 = new Halloumi.Common.Windows.Controls.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbBPM = new Halloumi.Common.Windows.Controls.ComboBox();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBPM)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(740, 413);
            this.panel1.Style = Halloumi.Common.Windows.Controls.PanelStyle.Background;
            this.panel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.cmbBPM);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(740, 32);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.label1.Size = new System.Drawing.Size(41, 24);
            this.label1.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label1.TabIndex = 0;
            this.label1.Text = "BPM:";
            // 
            // cmbBPM
            // 
            this.cmbBPM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBPM.DropDownWidth = 121;
            this.cmbBPM.ErrorProvider = null;
            this.cmbBPM.Items.AddRange(new object[] {
            "",
            "Yes",
            "No"});
            this.cmbBPM.Location = new System.Drawing.Point(51, 4);
            this.cmbBPM.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBPM.Name = "cmbBPM";
            this.cmbBPM.Size = new System.Drawing.Size(81, 25);
            this.cmbBPM.TabIndex = 7;
            this.cmbBPM.SelectedIndexChanged += new System.EventHandler(this.cmbBPM_SelectedIndexChanged);
            // 
            // SongControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "SongControl";
            this.Size = new System.Drawing.Size(740, 413);
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBPM)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Windows.Controls.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Common.Windows.Controls.Label label1;
        private Common.Windows.Controls.ComboBox cmbBPM;
    }
}
