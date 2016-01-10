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
            this.lblSampleDescription = new Halloumi.Common.Windows.Controls.Label();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnLink = new Halloumi.Common.Windows.Controls.Button();
            this.btnScratch = new Halloumi.Common.Windows.Controls.Button();
            this.btnPlay = new Halloumi.Common.Windows.Controls.Button();
            this.pnlBackground.SuspendLayout();
            this.flpButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.BackColor = System.Drawing.Color.Transparent;
            this.pnlBackground.Controls.Add(this.lblSampleDescription);
            this.pnlBackground.Controls.Add(this.flpButtons);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(7, 2);
            this.pnlBackground.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(986, 48);
            this.pnlBackground.TabIndex = 0;
            // 
            // lblSampleDescription
            // 
            this.lblSampleDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblSampleDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSampleDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSampleDescription.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblSampleDescription.Location = new System.Drawing.Point(0, 0);
            this.lblSampleDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSampleDescription.Name = "lblSampleDescription";
            this.lblSampleDescription.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.lblSampleDescription.Size = new System.Drawing.Size(757, 48);
            this.lblSampleDescription.TabIndex = 4;
            this.lblSampleDescription.Text = "Sample Name";
            // 
            // flpButtons
            // 
            this.flpButtons.BackColor = System.Drawing.Color.Transparent;
            this.flpButtons.Controls.Add(this.btnLink);
            this.flpButtons.Controls.Add(this.btnScratch);
            this.flpButtons.Controls.Add(this.btnPlay);
            this.flpButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flpButtons.Location = new System.Drawing.Point(757, 0);
            this.flpButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flpButtons.Size = new System.Drawing.Size(229, 48);
            this.flpButtons.TabIndex = 3;
            // 
            // btnLink
            // 
            this.btnLink.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLink.Location = new System.Drawing.Point(157, 5);
            this.btnLink.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(67, 38);
            this.btnLink.TabIndex = 4;
            this.btnLink.Text = "&Unlink";
            this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
            // 
            // btnScratch
            // 
            this.btnScratch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnScratch.Location = new System.Drawing.Point(74, 5);
            this.btnScratch.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnScratch.Name = "btnScratch";
            this.btnScratch.Size = new System.Drawing.Size(73, 38);
            this.btnScratch.TabIndex = 0;
            this.btnScratch.Text = "Scratch";
            this.btnScratch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnScratch_MouseDown);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(8, 5);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(56, 38);
            this.btnPlay.TabIndex = 3;
            this.btnPlay.Text = "&Play";
            this.btnPlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPlay_MouseDown);
            this.btnPlay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPlay_MouseUp);
            // 
            // SamplePlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlBackground);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "SamplePlayer";
            this.Padding = new System.Windows.Forms.Padding(7, 2, 7, 2);
            this.Size = new System.Drawing.Size(1000, 52);
            this.pnlBackground.ResumeLayout(false);
            this.flpButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlBackground;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private Halloumi.Common.Windows.Controls.Button btnScratch;
        private Halloumi.Common.Windows.Controls.Button btnPlay;
        private Halloumi.Common.Windows.Controls.Label lblSampleDescription;
        private Halloumi.Common.Windows.Controls.Button btnLink;

    }
}
