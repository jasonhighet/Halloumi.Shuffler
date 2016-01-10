namespace Halloumi.Shuffler.Forms
{
    partial class frmMonitorSettings
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
            this.pnlMain = new Halloumi.Common.Windows.Controls.Panel();
            this.pnlVolume = new System.Windows.Forms.Panel();
            this.pnlVolumeInner = new System.Windows.Forms.Panel();
            this.pnlFader = new Halloumi.Common.Windows.Controls.Panel();
            this.sldVolume = new Halloumi.Shuffler.Controls.Slider();
            this.lblVolume = new Halloumi.Common.Windows.Controls.Label();
            this.lblVolumCaption = new Halloumi.Common.Windows.Controls.Label();
            this.beveledLine = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.btnClose = new Halloumi.Common.Windows.Controls.Button();
            this.flpButtonsRight = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlButtons = new Halloumi.Common.Windows.Controls.Panel();
            this.pnlMain.SuspendLayout();
            this.pnlVolume.SuspendLayout();
            this.pnlVolumeInner.SuspendLayout();
            this.pnlFader.SuspendLayout();
            this.flpButtonsRight.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Controls.Add(this.pnlVolume);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(10);
            this.pnlMain.Size = new System.Drawing.Size(342, 45);
            this.pnlMain.TabIndex = 32;
            // 
            // pnlVolume
            // 
            this.pnlVolume.BackColor = System.Drawing.Color.Transparent;
            this.pnlVolume.Controls.Add(this.pnlVolumeInner);
            this.pnlVolume.Controls.Add(this.lblVolume);
            this.pnlVolume.Controls.Add(this.lblVolumCaption);
            this.pnlVolume.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlVolume.Location = new System.Drawing.Point(10, 10);
            this.pnlVolume.Name = "pnlVolume";
            this.pnlVolume.Size = new System.Drawing.Size(322, 27);
            this.pnlVolume.TabIndex = 34;
            // 
            // pnlVolumeInner
            // 
            this.pnlVolumeInner.Controls.Add(this.pnlFader);
            this.pnlVolumeInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVolumeInner.Location = new System.Drawing.Point(57, 0);
            this.pnlVolumeInner.Margin = new System.Windows.Forms.Padding(0);
            this.pnlVolumeInner.Name = "pnlVolumeInner";
            this.pnlVolumeInner.Size = new System.Drawing.Size(237, 27);
            this.pnlVolumeInner.TabIndex = 8;
            // 
            // pnlFader
            // 
            this.pnlFader.BackColor = System.Drawing.Color.White;
            this.pnlFader.Controls.Add(this.sldVolume);
            this.pnlFader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFader.Location = new System.Drawing.Point(0, 0);
            this.pnlFader.Name = "pnlFader";
            this.pnlFader.Padding = new System.Windows.Forms.Padding(5);
            this.pnlFader.Size = new System.Drawing.Size(237, 22);
            this.pnlFader.TabIndex = 67;
            // 
            // sldVolume
            // 
            this.sldVolume.Animated = false;
            this.sldVolume.AnimationSize = 0.2F;
            this.sldVolume.AnimationSpeed = MediaSlider.MediaSlider.AnimateSpeed.Normal;
            this.sldVolume.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.sldVolume.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.sldVolume.BackColor = System.Drawing.Color.White;
            this.sldVolume.BackgroundImage = null;
            this.sldVolume.BackGroundImage = null;
            this.sldVolume.ButtonAccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.sldVolume.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(169)))), ((int)(((byte)(179)))));
            this.sldVolume.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.sldVolume.ButtonCornerRadius = ((uint)(2u));
            this.sldVolume.ButtonSize = new System.Drawing.Size(24, 12);
            this.sldVolume.ButtonStyle = MediaSlider.MediaSlider.ButtonType.GlassInline;
            this.sldVolume.ContextMenuStrip = null;
            this.sldVolume.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sldVolume.LargeChange = 2;
            this.sldVolume.Location = new System.Drawing.Point(5, 5);
            this.sldVolume.Margin = new System.Windows.Forms.Padding(0);
            this.sldVolume.Maximum = 100;
            this.sldVolume.Minimum = 0;
            this.sldVolume.Name = "sldVolume";
            this.sldVolume.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sldVolume.ResizeRedraw = true;
            this.sldVolume.ShowButtonOnHover = false;
            this.sldVolume.Size = new System.Drawing.Size(227, 12);
            this.sldVolume.SliderFlyOut = MediaSlider.MediaSlider.FlyOutStyle.None;
            this.sldVolume.SmallChange = 1;
            this.sldVolume.SmoothScrolling = false;
            this.sldVolume.TabIndex = 9;
            this.sldVolume.TickColor = System.Drawing.Color.DarkOliveGreen;
            this.sldVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.sldVolume.TickType = MediaSlider.MediaSlider.TickMode.Standard;
            this.sldVolume.TrackBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(169)))), ((int)(((byte)(179)))));
            this.sldVolume.TrackDepth = 6;
            this.sldVolume.TrackFillColor = System.Drawing.Color.Transparent;
            this.sldVolume.TrackProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(228)))), ((int)(((byte)(138)))));
            this.sldVolume.TrackShadow = false;
            this.sldVolume.TrackShadowColor = System.Drawing.Color.DarkGray;
            this.sldVolume.TrackStyle = MediaSlider.MediaSlider.TrackType.Progress;
            this.sldVolume.Value = 0;
            // 
            // lblVolume
            // 
            this.lblVolume.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblVolume.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolume.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblVolume.Location = new System.Drawing.Point(294, 0);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(28, 27);
            this.lblVolume.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblVolume.TabIndex = 6;
            this.lblVolume.Text = "100";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVolumCaption
            // 
            this.lblVolumCaption.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblVolumCaption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolumCaption.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblVolumCaption.Location = new System.Drawing.Point(0, 0);
            this.lblVolumCaption.Name = "lblVolumCaption";
            this.lblVolumCaption.Size = new System.Drawing.Size(57, 27);
            this.lblVolumCaption.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblVolumCaption.TabIndex = 4;
            this.lblVolumCaption.Text = "Volume:";
            this.lblVolumCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // beveledLine
            // 
            this.beveledLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.beveledLine.Location = new System.Drawing.Point(0, 45);
            this.beveledLine.Name = "beveledLine";
            this.beveledLine.Size = new System.Drawing.Size(342, 2);
            this.beveledLine.TabIndex = 31;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(259, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 25);
            this.btnClose.TabIndex = 31;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // flpButtonsRight
            // 
            this.flpButtonsRight.BackColor = System.Drawing.Color.Transparent;
            this.flpButtonsRight.Controls.Add(this.btnClose);
            this.flpButtonsRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpButtonsRight.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpButtonsRight.Location = new System.Drawing.Point(0, 0);
            this.flpButtonsRight.Name = "flpButtonsRight";
            this.flpButtonsRight.Padding = new System.Windows.Forms.Padding(5);
            this.flpButtonsRight.Size = new System.Drawing.Size(342, 43);
            this.flpButtonsRight.TabIndex = 0;
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.flpButtonsRight);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 47);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(342, 43);
            this.pnlButtons.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlButtons.TabIndex = 30;
            // 
            // frmMonitorSettings
            // 
            this.AcceptButton = this.btnClose;
            this.AllowFormChrome = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(342, 90);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.beveledLine);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMonitorSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Halloumi : Shuffler : Monitor Settings";
            this.UseApplicationIcon = true;
            this.Load += new System.EventHandler(this.frmMonitorSettings_Load);
            this.pnlMain.ResumeLayout(false);
            this.pnlVolume.ResumeLayout(false);
            this.pnlVolumeInner.ResumeLayout(false);
            this.pnlFader.ResumeLayout(false);
            this.flpButtonsRight.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlMain;
        private Halloumi.Common.Windows.Controls.BeveledLine beveledLine;
        private Halloumi.Common.Windows.Controls.Button btnClose;
        private System.Windows.Forms.FlowLayoutPanel flpButtonsRight;
        private Halloumi.Common.Windows.Controls.Panel pnlButtons;
        private System.Windows.Forms.Panel pnlVolume;
        private System.Windows.Forms.Panel pnlVolumeInner;
        private Halloumi.Common.Windows.Controls.Panel pnlFader;
        private Halloumi.Shuffler.Controls.Slider sldVolume;
        private Halloumi.Common.Windows.Controls.Label lblVolume;
        private Halloumi.Common.Windows.Controls.Label lblVolumCaption;
    }
}