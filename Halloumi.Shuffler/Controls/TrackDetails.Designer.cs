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
            this.pnlBorder = new Halloumi.Common.Windows.Controls.Panel();
            this.pnlBackground = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.lblCurrentTrackDetails = new Halloumi.Common.Windows.Controls.Label();
            this.lblCurrentTrackDescription = new Halloumi.Common.Windows.Controls.Label();
            this.picCover = new System.Windows.Forms.PictureBox();
            this.linMenuBorder = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.pnlBorder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBackground)).BeginInit();
            this.pnlBackground.SuspendLayout();
            this.pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCover)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBorder
            // 
            this.pnlBorder.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBorder.Controls.Add(this.pnlBackground);
            this.pnlBorder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBorder.Location = new System.Drawing.Point(0, 0);
            this.pnlBorder.Name = "pnlBorder";
            this.pnlBorder.Padding = new System.Windows.Forms.Padding(1);
            this.pnlBorder.Size = new System.Drawing.Size(876, 73);
            this.pnlBorder.TabIndex = 6;
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.pnlRight);
            this.pnlBackground.Controls.Add(this.linMenuBorder);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(1, 1);
            this.pnlBackground.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.PanelAlternate;
            this.pnlBackground.Size = new System.Drawing.Size(874, 71);
            this.pnlBackground.TabIndex = 6;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.Transparent;
            this.pnlRight.Controls.Add(this.lblCurrentTrackDetails);
            this.pnlRight.Controls.Add(this.lblCurrentTrackDescription);
            this.pnlRight.Controls.Add(this.picCover);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(1, 0);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.pnlRight.Size = new System.Drawing.Size(873, 71);
            this.pnlRight.TabIndex = 24;
            // 
            // lblCurrentTrackDetails
            // 
            this.lblCurrentTrackDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCurrentTrackDetails.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentTrackDetails.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCurrentTrackDetails.Location = new System.Drawing.Point(68, 37);
            this.lblCurrentTrackDetails.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentTrackDetails.Name = "lblCurrentTrackDetails";
            this.lblCurrentTrackDetails.Padding = new System.Windows.Forms.Padding(10, 0, 13, 0);
            this.lblCurrentTrackDetails.Size = new System.Drawing.Size(798, 28);
            this.lblCurrentTrackDetails.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblCurrentTrackDetails.TabIndex = 18;
            this.lblCurrentTrackDetails.Text = "Blue Brazil - Blue Note In A Latin Groove Vol. 3 - Latin - 2:02 - 102BPM";
            this.lblCurrentTrackDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCurrentTrackDescription
            // 
            this.lblCurrentTrackDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCurrentTrackDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentTrackDescription.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCurrentTrackDescription.Location = new System.Drawing.Point(68, 6);
            this.lblCurrentTrackDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentTrackDescription.Name = "lblCurrentTrackDescription";
            this.lblCurrentTrackDescription.Padding = new System.Windows.Forms.Padding(10, 0, 13, 0);
            this.lblCurrentTrackDescription.Size = new System.Drawing.Size(798, 25);
            this.lblCurrentTrackDescription.Style = Halloumi.Common.Windows.Controls.LabelStyle.Heading;
            this.lblCurrentTrackDescription.TabIndex = 17;
            this.lblCurrentTrackDescription.Text = "Elza Soares &&  Roberto Ribeiro - O Que Vem De Baixo Nao Me Atinge";
            this.lblCurrentTrackDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picCover
            // 
            this.picCover.BackColor = System.Drawing.Color.White;
            this.picCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCover.Dock = System.Windows.Forms.DockStyle.Left;
            this.picCover.Location = new System.Drawing.Point(7, 6);
            this.picCover.Margin = new System.Windows.Forms.Padding(4);
            this.picCover.Name = "picCover";
            this.picCover.Size = new System.Drawing.Size(61, 59);
            this.picCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCover.TabIndex = 16;
            this.picCover.TabStop = false;
            // 
            // linMenuBorder
            // 
            this.linMenuBorder.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ButtonStandalone;
            this.linMenuBorder.Dock = System.Windows.Forms.DockStyle.Left;
            this.linMenuBorder.Location = new System.Drawing.Point(0, 0);
            this.linMenuBorder.Margin = new System.Windows.Forms.Padding(4);
            this.linMenuBorder.Name = "linMenuBorder";
            this.linMenuBorder.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.linMenuBorder.Size = new System.Drawing.Size(1, 71);
            this.linMenuBorder.Text = "kryptonBorderEdge4";
            // 
            // TrackDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlBorder);
            this.Name = "TrackDetails";
            this.Size = new System.Drawing.Size(876, 73);
            this.pnlBorder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlBackground)).EndInit();
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCover)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlBorder;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel pnlBackground;
        private System.Windows.Forms.Panel pnlRight;
        private Halloumi.Common.Windows.Controls.Label lblCurrentTrackDetails;
        private Halloumi.Common.Windows.Controls.Label lblCurrentTrackDescription;
        private System.Windows.Forms.PictureBox picCover;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge linMenuBorder;

    }
}
