namespace Halloumi.Shuffler.Controls
{
    partial class SamplePlayer
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
            this.pnlBackground = new Halloumi.Common.Windows.Controls.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPlay = new Halloumi.Common.Windows.Controls.Button();
            this.btnScratch = new Halloumi.Common.Windows.Controls.Button();
            this.lblSampleDescription = new Halloumi.Common.Windows.Controls.Label();
            this.pnlBackground.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.BackColor = System.Drawing.Color.Transparent;
            this.pnlBackground.Controls.Add(this.panel1);
            this.pnlBackground.Controls.Add(this.lblSampleDescription);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(7, 2);
            this.pnlBackground.Margin = new System.Windows.Forms.Padding(4, 4, 0, 4);
            this.pnlBackground.MinimumSize = new System.Drawing.Size(599, 40);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(599, 40);
            this.pnlBackground.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnPlay);
            this.panel1.Controls.Add(this.btnScratch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(375, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(224, 40);
            this.panel1.TabIndex = 5;
            // 
            // btnPlay
            // 
            this.btnPlay.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPlay.Location = new System.Drawing.Point(69, 0);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Padding = new System.Windows.Forms.Padding(4);
            this.btnPlay.Size = new System.Drawing.Size(69, 40);
            this.btnPlay.TabIndex = 5;
            this.btnPlay.Text = "&Play";
            this.btnPlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPlay_MouseDown);
            this.btnPlay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPlay_MouseUp);
            // 
            // btnScratch
            // 
            this.btnScratch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnScratch.Location = new System.Drawing.Point(138, 0);
            this.btnScratch.Margin = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.btnScratch.Name = "btnScratch";
            this.btnScratch.Padding = new System.Windows.Forms.Padding(4);
            this.btnScratch.Size = new System.Drawing.Size(86, 40);
            this.btnScratch.TabIndex = 4;
            this.btnScratch.Text = "Scratch";
            this.btnScratch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnScratch_MouseDown);
            // 
            // lblSampleDescription
            // 
            this.lblSampleDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblSampleDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSampleDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSampleDescription.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblSampleDescription.Location = new System.Drawing.Point(0, 0);
            this.lblSampleDescription.Margin = new System.Windows.Forms.Padding(0);
            this.lblSampleDescription.Name = "lblSampleDescription";
            this.lblSampleDescription.Padding = new System.Windows.Forms.Padding(0, 8, 0, 5);
            this.lblSampleDescription.Size = new System.Drawing.Size(599, 40);
            this.lblSampleDescription.TabIndex = 4;
            this.lblSampleDescription.Text = "Sample Name";
            // 
            // SamplePlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlBackground);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SamplePlayer";
            this.Padding = new System.Windows.Forms.Padding(7, 2, 2, 2);
            this.Size = new System.Drawing.Size(608, 43);
            this.pnlBackground.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlBackground;
        private Halloumi.Common.Windows.Controls.Label lblSampleDescription;
        private System.Windows.Forms.Panel panel1;
        private Common.Windows.Controls.Button btnPlay;
        private Common.Windows.Controls.Button btnScratch;
    }
}
