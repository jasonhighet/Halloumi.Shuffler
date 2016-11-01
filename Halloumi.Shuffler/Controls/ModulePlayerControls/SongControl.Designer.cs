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
            this.panel3 = new Halloumi.Common.Windows.Controls.Panel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStop = new Halloumi.Common.Windows.Controls.Button();
            this.btnPlay = new Halloumi.Common.Windows.Controls.Button();
            this.panel1 = new Halloumi.Common.Windows.Controls.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbBPM = new Halloumi.Common.Windows.Controls.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listBuilder = new ListBuilder();
            this.btnLoop = new Halloumi.Common.Windows.Controls.Button();
            this.panel3.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBPM)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.flowLayoutPanel3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(631, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(109, 413);
            this.panel3.Style = Halloumi.Common.Windows.Controls.PanelStyle.Background;
            this.panel3.TabIndex = 6;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.btnPlay);
            this.flowLayoutPanel3.Controls.Add(this.btnLoop);
            this.flowLayoutPanel3.Controls.Add(this.btnStop);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.flowLayoutPanel3.Size = new System.Drawing.Size(109, 413);
            this.flowLayoutPanel3.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(4, 87);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 31);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(4, 9);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(100, 31);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "Play";
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Controls.Add(this.panel3);
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
            this.flowLayoutPanel1.Size = new System.Drawing.Size(631, 32);
            this.flowLayoutPanel1.TabIndex = 7;
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
            // panel2
            // 
            this.panel2.Controls.Add(this.listBuilder);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 32);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(631, 381);
            this.panel2.TabIndex = 8;
            // 
            // listBuilder
            // 
            this.listBuilder.AllowMultipleAvailableItems = false;
            this.listBuilder.BackColor = System.Drawing.Color.Transparent;
            this.listBuilder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBuilder.Location = new System.Drawing.Point(5, 5);
            this.listBuilder.Name = "listBuilder";
            this.listBuilder.Size = new System.Drawing.Size(621, 371);
            this.listBuilder.TabIndex = 0;
            this.listBuilder.SelectedItemsChanged += new System.EventHandler(this.listBuilder_OnDestinationListChanged);
            // 
            // btnLoop
            // 
            this.btnLoop.Location = new System.Drawing.Point(4, 48);
            this.btnLoop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoop.Name = "btnLoop";
            this.btnLoop.Size = new System.Drawing.Size(100, 31);
            this.btnLoop.TabIndex = 3;
            this.btnLoop.Text = "Loop";
            this.btnLoop.Click += new System.EventHandler(this.btnLoop_Click);
            // 
            // SongControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "SongControl";
            this.Size = new System.Drawing.Size(740, 413);
            this.panel3.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBPM)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Windows.Controls.Panel panel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private Common.Windows.Controls.Button btnPlay;
        private Common.Windows.Controls.Button btnStop;
        private Common.Windows.Controls.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private ListBuilder listBuilder;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Common.Windows.Controls.Label label1;
        private Common.Windows.Controls.ComboBox cmbBPM;
        private Common.Windows.Controls.Button btnLoop;
    }
}
