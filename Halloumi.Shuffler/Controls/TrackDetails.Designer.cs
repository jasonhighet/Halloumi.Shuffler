namespace Halloumi.Shuffler.Controls
{
    partial class TrackDetails
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
            pnlBorder = new Halloumi.Common.Windows.Controls.Panel();
            pnlBackground = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            pnlRight = new System.Windows.Forms.Panel();
            lblCurrentTrackDetails = new Halloumi.Common.Windows.Controls.Label();
            lblCurrentTrackDescription = new Halloumi.Common.Windows.Controls.Label();
            pnlMixInfo = new System.Windows.Forms.Panel();
            pnlMixCounts = new System.Windows.Forms.Panel();
            lblMixOut = new Halloumi.Common.Windows.Controls.Label();
            lblMixIn = new Halloumi.Common.Windows.Controls.Label();
            btnToggleMixable = new System.Windows.Forms.Button();
            picCover = new System.Windows.Forms.PictureBox();
            linMenuBorder = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            pnlBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlBackground).BeginInit();
            pnlBackground.SuspendLayout();
            pnlRight.SuspendLayout();
            pnlMixInfo.SuspendLayout();
            pnlMixCounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picCover).BeginInit();
            SuspendLayout();
            // 
            // pnlBorder
            // 
            pnlBorder.BackColor = System.Drawing.SystemColors.Control;
            pnlBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlBorder.Controls.Add(pnlBackground);
            pnlBorder.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBorder.Location = new System.Drawing.Point(0, 0);
            pnlBorder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnlBorder.Name = "pnlBorder";
            pnlBorder.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            pnlBorder.Size = new System.Drawing.Size(1204, 110);
            pnlBorder.TabIndex = 6;
            // 
            // pnlBackground
            // 
            pnlBackground.Controls.Add(pnlRight);
            pnlBackground.Controls.Add(linMenuBorder);
            pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBackground.Location = new System.Drawing.Point(1, 2);
            pnlBackground.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            pnlBackground.Name = "pnlBackground";
            pnlBackground.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.PanelAlternate;
            pnlBackground.Size = new System.Drawing.Size(1202, 106);
            pnlBackground.TabIndex = 6;
            // 
            // pnlRight
            // 
            pnlRight.BackColor = System.Drawing.Color.Transparent;
            pnlRight.Controls.Add(lblCurrentTrackDetails);
            pnlRight.Controls.Add(lblCurrentTrackDescription);
            pnlRight.Controls.Add(pnlMixInfo);
            pnlRight.Controls.Add(picCover);
            pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlRight.Location = new System.Drawing.Point(1, 0);
            pnlRight.Margin = new System.Windows.Forms.Padding(0);
            pnlRight.Name = "pnlRight";
            pnlRight.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            pnlRight.Size = new System.Drawing.Size(1201, 106);
            pnlRight.TabIndex = 24;
            // 
            // lblCurrentTrackDetails
            // 
            lblCurrentTrackDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            lblCurrentTrackDetails.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblCurrentTrackDetails.ForeColor = System.Drawing.SystemColors.ControlText;
            lblCurrentTrackDetails.Location = new System.Drawing.Point(93, 55);
            lblCurrentTrackDetails.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblCurrentTrackDetails.Name = "lblCurrentTrackDetails";
            lblCurrentTrackDetails.Padding = new System.Windows.Forms.Padding(14, 0, 18, 0);
            lblCurrentTrackDetails.Size = new System.Drawing.Size(767, 42);
            lblCurrentTrackDetails.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblCurrentTrackDetails.TabIndex = 18;
            lblCurrentTrackDetails.Text = "Blue Brazil - Blue Note In A Latin Groove Vol. 3 - Latin - 2:02 - 102BPM";
            lblCurrentTrackDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCurrentTrackDescription
            // 
            lblCurrentTrackDescription.Dock = System.Windows.Forms.DockStyle.Top;
            lblCurrentTrackDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblCurrentTrackDescription.ForeColor = System.Drawing.SystemColors.ControlText;
            lblCurrentTrackDescription.Location = new System.Drawing.Point(93, 9);
            lblCurrentTrackDescription.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblCurrentTrackDescription.Name = "lblCurrentTrackDescription";
            lblCurrentTrackDescription.Padding = new System.Windows.Forms.Padding(14, 0, 18, 0);
            lblCurrentTrackDescription.Size = new System.Drawing.Size(767, 38);
            lblCurrentTrackDescription.Style = Common.Windows.Controls.LabelStyle.Heading;
            lblCurrentTrackDescription.TabIndex = 17;
            lblCurrentTrackDescription.Text = "Elza Soares &&  Roberto Ribeiro - O Que Vem De Baixo Nao Me Atinge";
            lblCurrentTrackDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlMixInfo
            // 
            pnlMixInfo.BackColor = System.Drawing.Color.Transparent;
            pnlMixInfo.Controls.Add(pnlMixCounts);
            pnlMixInfo.Controls.Add(btnToggleMixable);
            pnlMixInfo.Dock = System.Windows.Forms.DockStyle.Right;
            pnlMixInfo.Location = new System.Drawing.Point(860, 9);
            pnlMixInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnlMixInfo.Name = "pnlMixInfo";
            pnlMixInfo.Size = new System.Drawing.Size(331, 88);
            pnlMixInfo.TabIndex = 25;
            // 
            // pnlMixCounts
            // 
            pnlMixCounts.BackColor = System.Drawing.Color.Transparent;
            pnlMixCounts.Controls.Add(lblMixOut);
            pnlMixCounts.Controls.Add(lblMixIn);
            pnlMixCounts.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlMixCounts.Location = new System.Drawing.Point(0, 0);
            pnlMixCounts.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnlMixCounts.Name = "pnlMixCounts";
            pnlMixCounts.Size = new System.Drawing.Size(301, 88);
            pnlMixCounts.TabIndex = 1;
            // 
            // lblMixOut
            // 
            lblMixOut.Dock = System.Windows.Forms.DockStyle.Bottom;
            lblMixOut.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblMixOut.ForeColor = System.Drawing.SystemColors.ControlText;
            lblMixOut.Location = new System.Drawing.Point(0, 46);
            lblMixOut.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMixOut.Name = "lblMixOut";
            lblMixOut.Size = new System.Drawing.Size(301, 42);
            lblMixOut.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblMixOut.TabIndex = 1;
            lblMixOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMixIn
            // 
            lblMixIn.Dock = System.Windows.Forms.DockStyle.Top;
            lblMixIn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblMixIn.ForeColor = System.Drawing.SystemColors.ControlText;
            lblMixIn.Location = new System.Drawing.Point(0, 0);
            lblMixIn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMixIn.Name = "lblMixIn";
            lblMixIn.Size = new System.Drawing.Size(301, 38);
            lblMixIn.Style = Common.Windows.Controls.LabelStyle.Heading;
            lblMixIn.TabIndex = 0;
            lblMixIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnToggleMixable
            // 
            btnToggleMixable.BackColor = System.Drawing.Color.Transparent;
            btnToggleMixable.Dock = System.Windows.Forms.DockStyle.Right;
            btnToggleMixable.FlatAppearance.BorderSize = 0;
            btnToggleMixable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnToggleMixable.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnToggleMixable.Location = new System.Drawing.Point(301, 0);
            btnToggleMixable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnToggleMixable.Name = "btnToggleMixable";
            btnToggleMixable.Size = new System.Drawing.Size(30, 88);
            btnToggleMixable.TabIndex = 0;
            btnToggleMixable.Text = "▼";
            btnToggleMixable.UseVisualStyleBackColor = false;
            btnToggleMixable.Click += btnToggleMixable_Click;
            // 
            // picCover
            // 
            picCover.BackColor = System.Drawing.Color.White;
            picCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            picCover.Dock = System.Windows.Forms.DockStyle.Left;
            picCover.Location = new System.Drawing.Point(10, 9);
            picCover.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            picCover.Name = "picCover";
            picCover.Size = new System.Drawing.Size(83, 88);
            picCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            picCover.TabIndex = 16;
            picCover.TabStop = false;
            // 
            // linMenuBorder
            // 
            linMenuBorder.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ButtonStandalone;
            linMenuBorder.Dock = System.Windows.Forms.DockStyle.Left;
            linMenuBorder.Location = new System.Drawing.Point(0, 0);
            linMenuBorder.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            linMenuBorder.Name = "linMenuBorder";
            linMenuBorder.Orientation = System.Windows.Forms.Orientation.Vertical;
            linMenuBorder.Size = new System.Drawing.Size(1, 106);
            linMenuBorder.Text = "kryptonBorderEdge4";
            // 
            // TrackDetails
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlBorder);
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Name = "TrackDetails";
            Size = new System.Drawing.Size(1204, 110);
            pnlBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pnlBackground).EndInit();
            pnlBackground.ResumeLayout(false);
            pnlBackground.PerformLayout();
            pnlRight.ResumeLayout(false);
            pnlMixInfo.ResumeLayout(false);
            pnlMixCounts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picCover).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlBorder;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel pnlBackground;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlMixInfo;
        private System.Windows.Forms.Button btnToggleMixable;
        private System.Windows.Forms.Panel pnlMixCounts;
        private Halloumi.Common.Windows.Controls.Label lblMixIn;
        private Halloumi.Common.Windows.Controls.Label lblMixOut;
        private Halloumi.Common.Windows.Controls.Label lblCurrentTrackDetails;
        private Halloumi.Common.Windows.Controls.Label lblCurrentTrackDescription;
        private System.Windows.Forms.PictureBox picCover;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge linMenuBorder;

    }
}
