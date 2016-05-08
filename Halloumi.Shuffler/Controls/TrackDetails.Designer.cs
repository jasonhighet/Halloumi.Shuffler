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
            this.pnlOuterBackground = new Halloumi.Common.Windows.Controls.Panel();
            this.pnlBorderBackground = new Halloumi.Common.Windows.Controls.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblKey = new Halloumi.Common.Windows.Controls.Label();
            this.lblBpm = new Halloumi.Common.Windows.Controls.Label();
            this.lblTime = new Halloumi.Common.Windows.Controls.Label();
            this.lblGenre = new Halloumi.Common.Windows.Controls.Label();
            this.lblAlbum = new Halloumi.Common.Windows.Controls.Label();
            this.lblTrack = new Halloumi.Common.Windows.Controls.Label();
            this.lblArtist = new Halloumi.Common.Windows.Controls.Label();
            this.picCover = new System.Windows.Forms.PictureBox();
            this.pnlOuterBackground.SuspendLayout();
            this.pnlBorderBackground.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCover)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlOuterBackground
            // 
            this.pnlOuterBackground.BackColor = System.Drawing.SystemColors.Control;
            this.pnlOuterBackground.Controls.Add(this.pnlBorderBackground);
            this.pnlOuterBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOuterBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlOuterBackground.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlOuterBackground.Name = "pnlOuterBackground";
            this.pnlOuterBackground.Padding = new System.Windows.Forms.Padding(5, 5, 0, 0);
            this.pnlOuterBackground.Size = new System.Drawing.Size(280, 660);
            this.pnlOuterBackground.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlOuterBackground.TabIndex = 8;
            // 
            // pnlBorderBackground
            // 
            this.pnlBorderBackground.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBorderBackground.Controls.Add(this.panel1);
            this.pnlBorderBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBorderBackground.Location = new System.Drawing.Point(5, 5);
            this.pnlBorderBackground.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlBorderBackground.Name = "pnlBorderBackground";
            this.pnlBorderBackground.Size = new System.Drawing.Size(275, 655);
            this.pnlBorderBackground.Style = Halloumi.Common.Windows.Controls.PanelStyle.Background;
            this.pnlBorderBackground.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblKey);
            this.panel1.Controls.Add(this.lblBpm);
            this.panel1.Controls.Add(this.lblTime);
            this.panel1.Controls.Add(this.lblGenre);
            this.panel1.Controls.Add(this.lblAlbum);
            this.panel1.Controls.Add(this.lblTrack);
            this.panel1.Controls.Add(this.lblArtist);
            this.panel1.Controls.Add(this.picCover);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(275, 655);
            this.panel1.TabIndex = 0;
            // 
            // lblKey
            // 
            this.lblKey.AutoEllipsis = true;
            this.lblKey.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblKey.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKey.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblKey.Location = new System.Drawing.Point(0, 495);
            this.lblKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKey.Name = "lblKey";
            this.lblKey.Padding = new System.Windows.Forms.Padding(7, 6, 7, 4);
            this.lblKey.Size = new System.Drawing.Size(273, 37);
            this.lblKey.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblKey.TabIndex = 48;
            this.lblKey.Text = "C# Minor";
            this.lblKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBpm
            // 
            this.lblBpm.AutoEllipsis = true;
            this.lblBpm.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBpm.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBpm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblBpm.Location = new System.Drawing.Point(0, 458);
            this.lblBpm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBpm.Name = "lblBpm";
            this.lblBpm.Padding = new System.Windows.Forms.Padding(7, 6, 7, 4);
            this.lblBpm.Size = new System.Drawing.Size(273, 37);
            this.lblBpm.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblBpm.TabIndex = 47;
            this.lblBpm.Text = "102BPM";
            this.lblBpm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTime
            // 
            this.lblTime.AutoEllipsis = true;
            this.lblTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTime.Location = new System.Drawing.Point(0, 421);
            this.lblTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Padding = new System.Windows.Forms.Padding(7, 6, 7, 4);
            this.lblTime.Size = new System.Drawing.Size(273, 37);
            this.lblTime.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblTime.TabIndex = 46;
            this.lblTime.Text = "2:02";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGenre
            // 
            this.lblGenre.AutoEllipsis = true;
            this.lblGenre.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGenre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGenre.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblGenre.Location = new System.Drawing.Point(0, 384);
            this.lblGenre.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Padding = new System.Windows.Forms.Padding(7, 6, 7, 4);
            this.lblGenre.Size = new System.Drawing.Size(273, 37);
            this.lblGenre.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblGenre.TabIndex = 45;
            this.lblGenre.Text = "Latin";
            this.lblGenre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAlbum
            // 
            this.lblAlbum.AutoEllipsis = true;
            this.lblAlbum.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAlbum.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlbum.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblAlbum.Location = new System.Drawing.Point(0, 347);
            this.lblAlbum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAlbum.Name = "lblAlbum";
            this.lblAlbum.Padding = new System.Windows.Forms.Padding(7, 6, 7, 4);
            this.lblAlbum.Size = new System.Drawing.Size(273, 37);
            this.lblAlbum.Style = Halloumi.Common.Windows.Controls.LabelStyle.Heading;
            this.lblAlbum.TabIndex = 44;
            this.lblAlbum.Text = "Blue Brazil - Blue Note In A Latin Groove Vol. 3";
            this.lblAlbum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTrack
            // 
            this.lblTrack.AutoEllipsis = true;
            this.lblTrack.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTrack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrack.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTrack.Location = new System.Drawing.Point(0, 310);
            this.lblTrack.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTrack.Name = "lblTrack";
            this.lblTrack.Padding = new System.Windows.Forms.Padding(7, 6, 7, 4);
            this.lblTrack.Size = new System.Drawing.Size(273, 37);
            this.lblTrack.Style = Halloumi.Common.Windows.Controls.LabelStyle.Heading;
            this.lblTrack.TabIndex = 43;
            this.lblTrack.Text = "O Que Vem De Baixo Nao Me Atinge";
            this.lblTrack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblArtist
            // 
            this.lblArtist.AutoEllipsis = true;
            this.lblArtist.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblArtist.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArtist.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblArtist.Location = new System.Drawing.Point(0, 273);
            this.lblArtist.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblArtist.Name = "lblArtist";
            this.lblArtist.Padding = new System.Windows.Forms.Padding(7, 6, 7, 4);
            this.lblArtist.Size = new System.Drawing.Size(273, 37);
            this.lblArtist.Style = Halloumi.Common.Windows.Controls.LabelStyle.Heading;
            this.lblArtist.TabIndex = 42;
            this.lblArtist.Text = "Elza Soares &&  Roberto Ribeiro";
            this.lblArtist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picCover
            // 
            this.picCover.BackColor = System.Drawing.Color.White;
            this.picCover.Dock = System.Windows.Forms.DockStyle.Top;
            this.picCover.Location = new System.Drawing.Point(0, 0);
            this.picCover.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picCover.Name = "picCover";
            this.picCover.Size = new System.Drawing.Size(273, 273);
            this.picCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCover.TabIndex = 41;
            this.picCover.TabStop = false;
            // 
            // TrackDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlOuterBackground);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "TrackDetails";
            this.Size = new System.Drawing.Size(280, 660);
            this.pnlOuterBackground.ResumeLayout(false);
            this.pnlBorderBackground.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCover)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Windows.Controls.Panel pnlOuterBackground;
        private Common.Windows.Controls.Panel pnlBorderBackground;
        private System.Windows.Forms.Panel panel1;
        private Common.Windows.Controls.Label lblKey;
        private Common.Windows.Controls.Label lblBpm;
        private Common.Windows.Controls.Label lblTime;
        private Common.Windows.Controls.Label lblGenre;
        private Common.Windows.Controls.Label lblAlbum;
        private Common.Windows.Controls.Label lblTrack;
        private Common.Windows.Controls.Label lblArtist;
        private System.Windows.Forms.PictureBox picCover;
    }
}
