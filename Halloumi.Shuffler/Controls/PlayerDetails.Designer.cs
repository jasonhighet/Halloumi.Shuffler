namespace Halloumi.Shuffler.Controls
{
    partial class PlayerDetails
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
            this.components = new System.ComponentModel.Container();
            this.pnlBackground = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.pnlMiddle = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCurrentTrackDetails = new Halloumi.Common.Windows.Controls.Label();
            this.lblCurrentTrackDescription = new Halloumi.Common.Windows.Controls.Label();
            this.pnlSlider = new System.Windows.Forms.Panel();
            this.lblTimeRemaining = new Halloumi.Common.Windows.Controls.Label();
            this.lblTimeElapsed = new Halloumi.Common.Windows.Controls.Label();
            this.picCover = new System.Windows.Forms.PictureBox();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlVolume = new System.Windows.Forms.Panel();
            this.lblVolume = new Halloumi.Common.Windows.Controls.Label();
            this.lblVolumCaption = new Halloumi.Common.Windows.Controls.Label();
            this.picVisuals = new System.Windows.Forms.PictureBox();
            this.pnlLeft = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.beveledLine2 = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.flpTabButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnLibrary = new ComponentFactory.Krypton.Toolkit.KryptonCheckButton();
            this.btnPlaylist = new ComponentFactory.Krypton.Toolkit.KryptonCheckButton();
            this.btnMixer = new ComponentFactory.Krypton.Toolkit.KryptonCheckButton();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPrevious = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnReplayMix = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPlay = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPause = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSkipToEnd = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnNext = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.tabButtons = new ComponentFactory.Krypton.Toolkit.KryptonCheckSet(this.components);
            this.beveledLine1 = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.slider = new Halloumi.Shuffler.Controls.Slider();
            this.sldVolume = new Halloumi.Shuffler.Controls.Slider();
            this.volumeLevels = new Halloumi.Shuffler.Controls.VolumeLevels();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBackground)).BeginInit();
            this.pnlBackground.SuspendLayout();
            this.pnlMiddle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlSlider.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCover)).BeginInit();
            this.pnlRight.SuspendLayout();
            this.pnlVolume.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picVisuals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlLeft)).BeginInit();
            this.pnlLeft.SuspendLayout();
            this.flpTabButtons.SuspendLayout();
            this.flpButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabButtons)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.pnlMiddle);
            this.pnlBackground.Controls.Add(this.pnlRight);
            this.pnlBackground.Controls.Add(this.pnlLeft);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.PanelAlternate;
            this.pnlBackground.Size = new System.Drawing.Size(1255, 110);
            this.pnlBackground.TabIndex = 4;
            // 
            // pnlMiddle
            // 
            this.pnlMiddle.BackColor = System.Drawing.Color.Transparent;
            this.pnlMiddle.Controls.Add(this.panel1);
            this.pnlMiddle.Controls.Add(this.pnlSlider);
            this.pnlMiddle.Controls.Add(this.picCover);
            this.pnlMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMiddle.Location = new System.Drawing.Point(284, 0);
            this.pnlMiddle.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMiddle.Name = "pnlMiddle";
            this.pnlMiddle.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.pnlMiddle.Size = new System.Drawing.Size(691, 110);
            this.pnlMiddle.TabIndex = 27;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.lblCurrentTrackDetails);
            this.panel1.Controls.Add(this.lblCurrentTrackDescription);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(106, 5);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.panel1.Size = new System.Drawing.Size(580, 75);
            this.panel1.TabIndex = 25;
            // 
            // lblCurrentTrackDetails
            // 
            this.lblCurrentTrackDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCurrentTrackDetails.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentTrackDetails.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCurrentTrackDetails.Location = new System.Drawing.Point(7, 25);
            this.lblCurrentTrackDetails.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentTrackDetails.Name = "lblCurrentTrackDetails";
            this.lblCurrentTrackDetails.Padding = new System.Windows.Forms.Padding(0, 0, 13, 0);
            this.lblCurrentTrackDetails.Size = new System.Drawing.Size(573, 28);
            this.lblCurrentTrackDetails.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblCurrentTrackDetails.TabIndex = 1;
            this.lblCurrentTrackDetails.Text = "Blue Brazil - Blue Note In A Latin Groove Vol. 3 - Latin - 2:02 - 102BPM";
            this.lblCurrentTrackDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCurrentTrackDescription
            // 
            this.lblCurrentTrackDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCurrentTrackDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentTrackDescription.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCurrentTrackDescription.Location = new System.Drawing.Point(7, 0);
            this.lblCurrentTrackDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentTrackDescription.Name = "lblCurrentTrackDescription";
            this.lblCurrentTrackDescription.Padding = new System.Windows.Forms.Padding(0, 0, 13, 0);
            this.lblCurrentTrackDescription.Size = new System.Drawing.Size(573, 25);
            this.lblCurrentTrackDescription.Style = Halloumi.Common.Windows.Controls.LabelStyle.Heading;
            this.lblCurrentTrackDescription.TabIndex = 0;
            this.lblCurrentTrackDescription.Text = "Elza Soares &&  Roberto Ribeiro - O Que Vem De Baixo Nao Me Atinge";
            this.lblCurrentTrackDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSlider
            // 
            this.pnlSlider.BackColor = System.Drawing.Color.Transparent;
            this.pnlSlider.Controls.Add(this.slider);
            this.pnlSlider.Controls.Add(this.lblTimeRemaining);
            this.pnlSlider.Controls.Add(this.lblTimeElapsed);
            this.pnlSlider.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSlider.Location = new System.Drawing.Point(106, 80);
            this.pnlSlider.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlSlider.Name = "pnlSlider";
            this.pnlSlider.Size = new System.Drawing.Size(580, 25);
            this.pnlSlider.TabIndex = 17;
            // 
            // lblTimeRemaining
            // 
            this.lblTimeRemaining.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTimeRemaining.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeRemaining.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTimeRemaining.Location = new System.Drawing.Point(504, 0);
            this.lblTimeRemaining.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new System.Drawing.Size(76, 25);
            this.lblTimeRemaining.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblTimeRemaining.TabIndex = 1;
            this.lblTimeRemaining.Text = "00:00";
            this.lblTimeRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTimeElapsed
            // 
            this.lblTimeElapsed.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTimeElapsed.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeElapsed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTimeElapsed.Location = new System.Drawing.Point(0, 0);
            this.lblTimeElapsed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTimeElapsed.Name = "lblTimeElapsed";
            this.lblTimeElapsed.Size = new System.Drawing.Size(84, 25);
            this.lblTimeElapsed.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblTimeElapsed.TabIndex = 0;
            this.lblTimeElapsed.Text = "00:00";
            this.lblTimeElapsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picCover
            // 
            this.picCover.BackColor = System.Drawing.Color.White;
            this.picCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCover.Dock = System.Windows.Forms.DockStyle.Left;
            this.picCover.Location = new System.Drawing.Point(0, 5);
            this.picCover.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picCover.Name = "picCover";
            this.picCover.Size = new System.Drawing.Size(106, 100);
            this.picCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCover.TabIndex = 16;
            this.picCover.TabStop = false;
            this.picCover.Visible = false;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.Transparent;
            this.pnlRight.Controls.Add(this.pnlVolume);
            this.pnlRight.Controls.Add(this.picVisuals);
            this.pnlRight.Controls.Add(this.volumeLevels);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(975, 0);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.pnlRight.Size = new System.Drawing.Size(280, 110);
            this.pnlRight.TabIndex = 26;
            // 
            // pnlVolume
            // 
            this.pnlVolume.BackColor = System.Drawing.Color.Transparent;
            this.pnlVolume.Controls.Add(this.sldVolume);
            this.pnlVolume.Controls.Add(this.lblVolume);
            this.pnlVolume.Controls.Add(this.lblVolumCaption);
            this.pnlVolume.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlVolume.Location = new System.Drawing.Point(5, 80);
            this.pnlVolume.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlVolume.Name = "pnlVolume";
            this.pnlVolume.Size = new System.Drawing.Size(270, 25);
            this.pnlVolume.TabIndex = 26;
            // 
            // lblVolume
            // 
            this.lblVolume.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblVolume.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolume.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblVolume.Location = new System.Drawing.Point(233, 0);
            this.lblVolume.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(37, 25);
            this.lblVolume.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblVolume.TabIndex = 1;
            this.lblVolume.Text = "100";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVolumCaption
            // 
            this.lblVolumCaption.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblVolumCaption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolumCaption.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblVolumCaption.Location = new System.Drawing.Point(0, 0);
            this.lblVolumCaption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVolumCaption.Name = "lblVolumCaption";
            this.lblVolumCaption.Size = new System.Drawing.Size(40, 25);
            this.lblVolumCaption.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblVolumCaption.TabIndex = 0;
            this.lblVolumCaption.Text = "Vol:";
            this.lblVolumCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picVisuals
            // 
            this.picVisuals.BackColor = System.Drawing.Color.Transparent;
            this.picVisuals.Location = new System.Drawing.Point(5, 5);
            this.picVisuals.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picVisuals.Name = "picVisuals";
            this.picVisuals.Size = new System.Drawing.Size(269, 66);
            this.picVisuals.TabIndex = 28;
            this.picVisuals.TabStop = false;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.beveledLine2);
            this.pnlLeft.Controls.Add(this.flpTabButtons);
            this.pnlLeft.Controls.Add(this.flpButtons);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.pnlLeft.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.PanelAlternate;
            this.pnlLeft.Size = new System.Drawing.Size(284, 110);
            this.pnlLeft.TabIndex = 20;
            // 
            // beveledLine2
            // 
            this.beveledLine2.Dock = System.Windows.Forms.DockStyle.Top;
            this.beveledLine2.Location = new System.Drawing.Point(5, 58);
            this.beveledLine2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.beveledLine2.Name = "beveledLine2";
            this.beveledLine2.Size = new System.Drawing.Size(274, 2);
            this.beveledLine2.TabIndex = 23;
            // 
            // flpTabButtons
            // 
            this.flpTabButtons.BackColor = System.Drawing.Color.Transparent;
            this.flpTabButtons.Controls.Add(this.btnLibrary);
            this.flpTabButtons.Controls.Add(this.btnPlaylist);
            this.flpTabButtons.Controls.Add(this.btnMixer);
            this.flpTabButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpTabButtons.Location = new System.Drawing.Point(5, 67);
            this.flpTabButtons.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.flpTabButtons.Name = "flpTabButtons";
            this.flpTabButtons.Size = new System.Drawing.Size(274, 38);
            this.flpTabButtons.TabIndex = 22;
            // 
            // btnLibrary
            // 
            this.btnLibrary.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Cluster;
            this.btnLibrary.Checked = true;
            this.btnLibrary.Location = new System.Drawing.Point(0, 0);
            this.btnLibrary.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnLibrary.Name = "btnLibrary";
            this.btnLibrary.Size = new System.Drawing.Size(87, 38);
            this.btnLibrary.TabIndex = 16;
            this.btnLibrary.Values.Text = "Library";
            // 
            // btnPlaylist
            // 
            this.btnPlaylist.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Cluster;
            this.btnPlaylist.Location = new System.Drawing.Point(90, 0);
            this.btnPlaylist.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnPlaylist.Name = "btnPlaylist";
            this.btnPlaylist.Size = new System.Drawing.Size(87, 38);
            this.btnPlaylist.TabIndex = 15;
            this.btnPlaylist.Values.Text = "Playlist";
            // 
            // btnMixer
            // 
            this.btnMixer.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Cluster;
            this.btnMixer.Location = new System.Drawing.Point(180, 0);
            this.btnMixer.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnMixer.Name = "btnMixer";
            this.btnMixer.Size = new System.Drawing.Size(87, 38);
            this.btnMixer.TabIndex = 17;
            this.btnMixer.Values.Text = "Mixer";
            // 
            // flpButtons
            // 
            this.flpButtons.BackColor = System.Drawing.Color.Transparent;
            this.flpButtons.Controls.Add(this.btnPrevious);
            this.flpButtons.Controls.Add(this.btnReplayMix);
            this.flpButtons.Controls.Add(this.btnPlay);
            this.flpButtons.Controls.Add(this.btnPause);
            this.flpButtons.Controls.Add(this.btnSkipToEnd);
            this.flpButtons.Controls.Add(this.btnNext);
            this.flpButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpButtons.Location = new System.Drawing.Point(5, 5);
            this.flpButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Size = new System.Drawing.Size(274, 53);
            this.flpButtons.TabIndex = 21;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(0, 0);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(51, 47);
            this.btnPrevious.TabIndex = 9;
            this.btnPrevious.Values.Image = global::Halloumi.Shuffler.Properties.Resources.player_restart;
            this.btnPrevious.Values.Text = "";
            // 
            // btnReplayMix
            // 
            this.btnReplayMix.Location = new System.Drawing.Point(54, 0);
            this.btnReplayMix.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnReplayMix.Name = "btnReplayMix";
            this.btnReplayMix.Size = new System.Drawing.Size(51, 47);
            this.btnReplayMix.TabIndex = 14;
            this.btnReplayMix.Values.Image = global::Halloumi.Shuffler.Properties.Resources.player_rew;
            this.btnReplayMix.Values.Text = "";
            this.btnReplayMix.Click += new System.EventHandler(this.btnReplayMix_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(108, 0);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(51, 47);
            this.btnPlay.TabIndex = 10;
            this.btnPlay.Values.Image = global::Halloumi.Shuffler.Properties.Resources.player_play;
            this.btnPlay.Values.Text = "";
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(162, 0);
            this.btnPause.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(51, 47);
            this.btnPause.TabIndex = 13;
            this.btnPause.Values.Image = global::Halloumi.Shuffler.Properties.Resources.player_pause;
            this.btnPause.Values.Text = "";
            this.btnPause.Visible = false;
            // 
            // btnSkipToEnd
            // 
            this.btnSkipToEnd.Location = new System.Drawing.Point(216, 0);
            this.btnSkipToEnd.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnSkipToEnd.Name = "btnSkipToEnd";
            this.btnSkipToEnd.Size = new System.Drawing.Size(51, 47);
            this.btnSkipToEnd.TabIndex = 11;
            this.btnSkipToEnd.Values.Image = global::Halloumi.Shuffler.Properties.Resources.player_fwd;
            this.btnSkipToEnd.Values.Text = "";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(0, 47);
            this.btnNext.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(51, 47);
            this.btnNext.TabIndex = 12;
            this.btnNext.Values.Image = global::Halloumi.Shuffler.Properties.Resources.player_end;
            this.btnNext.Values.Text = "";
            // 
            // tabButtons
            // 
            this.tabButtons.CheckButtons.Add(this.btnPlaylist);
            this.tabButtons.CheckButtons.Add(this.btnLibrary);
            this.tabButtons.CheckButtons.Add(this.btnMixer);
            this.tabButtons.CheckedButton = this.btnLibrary;
            this.tabButtons.CheckedButtonChanged += new System.EventHandler(this.tabButtons_CheckedButtonChanged);
            // 
            // beveledLine1
            // 
            this.beveledLine1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.beveledLine1.Location = new System.Drawing.Point(0, 110);
            this.beveledLine1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.beveledLine1.Name = "beveledLine1";
            this.beveledLine1.Size = new System.Drawing.Size(1255, 2);
            this.beveledLine1.TabIndex = 2;
            // 
            // slider
            // 
            this.slider.Animated = false;
            this.slider.AnimationSize = 0.2F;
            this.slider.AnimationSpeed = MediaSlider.MediaSlider.AnimateSpeed.Normal;
            this.slider.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.slider.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.slider.BackColor = System.Drawing.SystemColors.Control;
            this.slider.BackgroundImage = null;
            this.slider.BackGroundImage = null;
            this.slider.ButtonAccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.slider.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(158)))), ((int)(((byte)(191)))));
            this.slider.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.slider.ButtonCornerRadius = ((uint)(2u));
            this.slider.ButtonSize = new System.Drawing.Size(24, 12);
            this.slider.ButtonStyle = MediaSlider.MediaSlider.ButtonType.GlassInline;
            this.slider.ContextMenuStrip = null;
            this.slider.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.slider.LargeChange = 2;
            this.slider.Location = new System.Drawing.Point(84, 3);
            this.slider.Margin = new System.Windows.Forms.Padding(0);
            this.slider.Maximum = 10;
            this.slider.Minimum = 0;
            this.slider.Name = "slider";
            this.slider.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.slider.ResizeRedraw = true;
            this.slider.ShowButtonOnHover = false;
            this.slider.Size = new System.Drawing.Size(420, 22);
            this.slider.SliderFlyOut = MediaSlider.MediaSlider.FlyOutStyle.None;
            this.slider.SmallChange = 1;
            this.slider.SmoothScrolling = false;
            this.slider.TabIndex = 2;
            this.slider.TickColor = System.Drawing.Color.DarkOliveGreen;
            this.slider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.slider.TickType = MediaSlider.MediaSlider.TickMode.Standard;
            this.slider.TrackBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(158)))), ((int)(((byte)(191)))));
            this.slider.TrackDepth = 6;
            this.slider.TrackFillColor = System.Drawing.Color.Transparent;
            this.slider.TrackProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(228)))), ((int)(((byte)(138)))));
            this.slider.TrackShadow = false;
            this.slider.TrackShadowColor = System.Drawing.Color.DarkGray;
            this.slider.TrackStyle = MediaSlider.MediaSlider.TrackType.Progress;
            this.slider.Value = 0;
            this.slider.WheelScrollTicks = 3;
            // 
            // sldVolume
            // 
            this.sldVolume.Animated = false;
            this.sldVolume.AnimationSize = 0.2F;
            this.sldVolume.AnimationSpeed = MediaSlider.MediaSlider.AnimateSpeed.Normal;
            this.sldVolume.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.sldVolume.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.sldVolume.BackColor = System.Drawing.SystemColors.Control;
            this.sldVolume.BackgroundImage = null;
            this.sldVolume.BackGroundImage = null;
            this.sldVolume.ButtonAccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.sldVolume.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(158)))), ((int)(((byte)(191)))));
            this.sldVolume.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.sldVolume.ButtonCornerRadius = ((uint)(2u));
            this.sldVolume.ButtonSize = new System.Drawing.Size(24, 12);
            this.sldVolume.ButtonStyle = MediaSlider.MediaSlider.ButtonType.GlassInline;
            this.sldVolume.ContextMenuStrip = null;
            this.sldVolume.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sldVolume.LargeChange = 2;
            this.sldVolume.Location = new System.Drawing.Point(40, 3);
            this.sldVolume.Margin = new System.Windows.Forms.Padding(0);
            this.sldVolume.Maximum = 10;
            this.sldVolume.Minimum = 0;
            this.sldVolume.Name = "sldVolume";
            this.sldVolume.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sldVolume.ResizeRedraw = true;
            this.sldVolume.ShowButtonOnHover = false;
            this.sldVolume.Size = new System.Drawing.Size(193, 22);
            this.sldVolume.SliderFlyOut = MediaSlider.MediaSlider.FlyOutStyle.None;
            this.sldVolume.SmallChange = 1;
            this.sldVolume.SmoothScrolling = false;
            this.sldVolume.TabIndex = 2;
            this.sldVolume.TickColor = System.Drawing.Color.DarkOliveGreen;
            this.sldVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.sldVolume.TickType = MediaSlider.MediaSlider.TickMode.Standard;
            this.sldVolume.TrackBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(158)))), ((int)(((byte)(191)))));
            this.sldVolume.TrackDepth = 6;
            this.sldVolume.TrackFillColor = System.Drawing.Color.Transparent;
            this.sldVolume.TrackProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(228)))), ((int)(((byte)(138)))));
            this.sldVolume.TrackShadow = false;
            this.sldVolume.TrackShadowColor = System.Drawing.Color.DarkGray;
            this.sldVolume.TrackStyle = MediaSlider.MediaSlider.TrackType.Progress;
            this.sldVolume.Value = 0;
            this.sldVolume.WheelScrollTicks = 3;
            // 
            // volumeLevels
            // 
            this.volumeLevels.BackColor = System.Drawing.Color.Transparent;
            this.volumeLevels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.volumeLevels.Location = new System.Drawing.Point(5, 5);
            this.volumeLevels.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.volumeLevels.Name = "volumeLevels";
            this.volumeLevels.Padding = new System.Windows.Forms.Padding(0, 5, 0, 10);
            this.volumeLevels.Size = new System.Drawing.Size(270, 100);
            this.volumeLevels.TabIndex = 29;
            this.volumeLevels.Visible = false;
            // 
            // PlayerDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlBackground);
            this.Controls.Add(this.beveledLine1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PlayerDetails";
            this.Size = new System.Drawing.Size(1255, 112);
            ((System.ComponentModel.ISupportInitialize)(this.pnlBackground)).EndInit();
            this.pnlBackground.ResumeLayout(false);
            this.pnlMiddle.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlSlider.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCover)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.pnlVolume.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picVisuals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlLeft)).EndInit();
            this.pnlLeft.ResumeLayout(false);
            this.flpTabButtons.ResumeLayout(false);
            this.flpButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabButtons)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.BeveledLine beveledLine1;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel pnlBackground;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel pnlLeft;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPlay;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPause;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSkipToEnd;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnNext;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrevious;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnReplayMix;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlMiddle;
        private System.Windows.Forms.Panel panel1;
        private Halloumi.Common.Windows.Controls.Label lblCurrentTrackDetails;
        private Halloumi.Common.Windows.Controls.Label lblCurrentTrackDescription;
        private System.Windows.Forms.Panel pnlSlider;
        private Slider slider;
        private Halloumi.Common.Windows.Controls.Label lblTimeRemaining;
        private Halloumi.Common.Windows.Controls.Label lblTimeElapsed;
        private System.Windows.Forms.PictureBox picCover;
        private System.Windows.Forms.Panel pnlVolume;
        private Slider sldVolume;
        private Halloumi.Common.Windows.Controls.Label lblVolume;
        private Halloumi.Common.Windows.Controls.Label lblVolumCaption;
        private Halloumi.Common.Windows.Controls.BeveledLine beveledLine2;
        private System.Windows.Forms.FlowLayoutPanel flpTabButtons;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckButton btnPlaylist;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckSet tabButtons;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckButton btnLibrary;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckButton btnMixer;
        private VolumeLevels volumeLevels;
        private System.Windows.Forms.PictureBox picVisuals;

    }
}
