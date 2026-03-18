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
            pnlBackground = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            pnlMiddle = new System.Windows.Forms.Panel();
            panel1 = new System.Windows.Forms.Panel();
            lblCurrentTrackDetails = new Halloumi.Common.Windows.Controls.Label();
            lblCurrentTrackDescription = new Halloumi.Common.Windows.Controls.Label();
            pnlSlider = new System.Windows.Forms.Panel();
            slider = new Slider();
            lblTimeRemaining = new Halloumi.Common.Windows.Controls.Label();
            lblTimeElapsed = new Halloumi.Common.Windows.Controls.Label();
            picCover = new System.Windows.Forms.PictureBox();
            pnlRight = new System.Windows.Forms.Panel();
            pnlVolume = new System.Windows.Forms.Panel();
            sldVolume = new Slider();
            lblVolume = new Halloumi.Common.Windows.Controls.Label();
            lblVolumCaption = new Halloumi.Common.Windows.Controls.Label();
            picVisuals = new System.Windows.Forms.PictureBox();
            volumeLevels = new VolumeLevels();
            pnlLeft = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            flpRankButtons = new System.Windows.Forms.FlowLayoutPanel();
            btnRankExcellent = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            btnRankVeryGood = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            btnRankGood = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            btnRankBearable = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            btnRankForbidden = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            btnPrevious = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            btnReplayMix = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            btnPlay = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            btnPause = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            btnSkipToEnd = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            btnNext = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            pnlTrackInfo = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            lblNextTrack = new Halloumi.Common.Windows.Controls.Label();
            lblPrevTrack = new Halloumi.Common.Windows.Controls.Label();
            beveledLine1 = new Halloumi.Common.Windows.Controls.BeveledLine();
            pnlPlayerMixCounts = new System.Windows.Forms.Panel();
            lblPlayerMixOut = new Halloumi.Common.Windows.Controls.Label();
            lblPlayerMixIn = new Halloumi.Common.Windows.Controls.Label();
            ((System.ComponentModel.ISupportInitialize)pnlBackground).BeginInit();
            pnlBackground.SuspendLayout();
            pnlMiddle.SuspendLayout();
            panel1.SuspendLayout();
            pnlSlider.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picCover).BeginInit();
            pnlRight.SuspendLayout();
            pnlVolume.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picVisuals).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pnlLeft).BeginInit();
            pnlLeft.SuspendLayout();
            flpRankButtons.SuspendLayout();
            flpButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlTrackInfo).BeginInit();
            pnlTrackInfo.SuspendLayout();
            pnlPlayerMixCounts.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBackground
            // 
            pnlBackground.Controls.Add(pnlMiddle);
            pnlBackground.Controls.Add(pnlRight);
            pnlBackground.Controls.Add(pnlLeft);
            pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBackground.Location = new System.Drawing.Point(0, 0);
            pnlBackground.Margin = new System.Windows.Forms.Padding(6);
            pnlBackground.Name = "pnlBackground";
            pnlBackground.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.PanelAlternate;
            pnlBackground.Size = new System.Drawing.Size(2043, 166);
            pnlBackground.TabIndex = 4;
            // 
            // pnlMiddle
            // 
            pnlMiddle.BackColor = System.Drawing.Color.Transparent;
            pnlMiddle.Controls.Add(pnlPlayerMixCounts);
            pnlMiddle.Controls.Add(panel1);
            pnlMiddle.Controls.Add(pnlSlider);
            pnlMiddle.Controls.Add(picCover);
            pnlMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlMiddle.Location = new System.Drawing.Point(390, 0);
            pnlMiddle.Margin = new System.Windows.Forms.Padding(0);
            pnlMiddle.Name = "pnlMiddle";
            pnlMiddle.Padding = new System.Windows.Forms.Padding(0, 8, 7, 8);
            pnlMiddle.Size = new System.Drawing.Size(1268, 166);
            pnlMiddle.TabIndex = 27;
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.Transparent;
            panel1.Controls.Add(lblCurrentTrackDetails);
            panel1.Controls.Add(lblCurrentTrackDescription);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(145, 8);
            panel1.Margin = new System.Windows.Forms.Padding(6);
            panel1.Name = "panel1";
            panel1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            panel1.Size = new System.Drawing.Size(1116, 112);
            panel1.TabIndex = 25;
            // 
            // lblCurrentTrackDetails
            // 
            lblCurrentTrackDetails.Dock = System.Windows.Forms.DockStyle.Top;
            lblCurrentTrackDetails.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblCurrentTrackDetails.ForeColor = System.Drawing.SystemColors.ControlText;
            lblCurrentTrackDetails.Location = new System.Drawing.Point(10, 38);
            lblCurrentTrackDetails.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblCurrentTrackDetails.Name = "lblCurrentTrackDetails";
            lblCurrentTrackDetails.Padding = new System.Windows.Forms.Padding(0, 0, 18, 0);
            lblCurrentTrackDetails.Size = new System.Drawing.Size(1106, 42);
            lblCurrentTrackDetails.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblCurrentTrackDetails.TabIndex = 1;
            lblCurrentTrackDetails.Text = "Blue Brazil - Blue Note In A Latin Groove Vol. 3 - Latin - 2:02 - 102BPM";
            lblCurrentTrackDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCurrentTrackDescription
            // 
            lblCurrentTrackDescription.Dock = System.Windows.Forms.DockStyle.Top;
            lblCurrentTrackDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblCurrentTrackDescription.ForeColor = System.Drawing.SystemColors.ControlText;
            lblCurrentTrackDescription.Location = new System.Drawing.Point(10, 0);
            lblCurrentTrackDescription.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblCurrentTrackDescription.Name = "lblCurrentTrackDescription";
            lblCurrentTrackDescription.Padding = new System.Windows.Forms.Padding(0, 0, 18, 0);
            lblCurrentTrackDescription.Size = new System.Drawing.Size(1106, 38);
            lblCurrentTrackDescription.Style = Common.Windows.Controls.LabelStyle.Heading;
            lblCurrentTrackDescription.TabIndex = 0;
            lblCurrentTrackDescription.Text = "Elza Soares &&  Roberto Ribeiro - O Que Vem De Baixo Nao Me Atinge";
            lblCurrentTrackDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSlider
            // 
            pnlSlider.BackColor = System.Drawing.Color.Transparent;
            pnlSlider.Controls.Add(slider);
            pnlSlider.Controls.Add(lblTimeRemaining);
            pnlSlider.Controls.Add(lblTimeElapsed);
            pnlSlider.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlSlider.Location = new System.Drawing.Point(145, 120);
            pnlSlider.Margin = new System.Windows.Forms.Padding(6);
            pnlSlider.Name = "pnlSlider";
            pnlSlider.Size = new System.Drawing.Size(1116, 38);
            pnlSlider.TabIndex = 17;
            // 
            // slider
            // 
            slider.Animated = false;
            slider.AnimationSize = 0.2F;
            slider.AnimationSpeed = MediaSlider.MediaSlider.AnimateSpeed.Normal;
            slider.AutoScrollMargin = new System.Drawing.Size(0, 0);
            slider.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            slider.BackColor = System.Drawing.SystemColors.Control;
            slider.BackgroundImage = null;
            slider.BackGroundImage = null;
            slider.ButtonAccentColor = System.Drawing.Color.FromArgb(128, 64, 64, 64);
            slider.ButtonBorderColor = System.Drawing.Color.FromArgb(133, 158, 191);
            slider.ButtonColor = System.Drawing.Color.FromArgb(160, 0, 0, 0);
            slider.ButtonCornerRadius = 2U;
            slider.ButtonSize = new System.Drawing.Size(24, 12);
            slider.ButtonStyle = MediaSlider.MediaSlider.ButtonType.GlassInline;
            slider.Dock = System.Windows.Forms.DockStyle.Bottom;
            slider.LargeChange = 2;
            slider.Location = new System.Drawing.Point(116, 5);
            slider.Margin = new System.Windows.Forms.Padding(0);
            slider.Maximum = 10;
            slider.Minimum = 0;
            slider.Name = "slider";
            slider.Orientation = System.Windows.Forms.Orientation.Horizontal;
            slider.ResizeRedraw = true;
            slider.ShowButtonOnHover = false;
            slider.Size = new System.Drawing.Size(896, 33);
            slider.SliderFlyOut = MediaSlider.MediaSlider.FlyOutStyle.None;
            slider.SmallChange = 1;
            slider.SmoothScrolling = false;
            slider.TabIndex = 2;
            slider.TickColor = System.Drawing.Color.DarkOliveGreen;
            slider.TickStyle = System.Windows.Forms.TickStyle.None;
            slider.TickType = MediaSlider.MediaSlider.TickMode.Standard;
            slider.TrackBorderColor = System.Drawing.Color.FromArgb(133, 158, 191);
            slider.TrackDepth = 6;
            slider.TrackFillColor = System.Drawing.Color.Transparent;
            slider.TrackProgressColor = System.Drawing.Color.FromArgb(255, 228, 138);
            slider.TrackShadow = false;
            slider.TrackShadowColor = System.Drawing.Color.DarkGray;
            slider.TrackStyle = MediaSlider.MediaSlider.TrackType.Progress;
            slider.Value = 0;
            slider.WheelScrollTicks = 3;
            // 
            // lblTimeRemaining
            // 
            lblTimeRemaining.Dock = System.Windows.Forms.DockStyle.Right;
            lblTimeRemaining.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblTimeRemaining.ForeColor = System.Drawing.SystemColors.ControlText;
            lblTimeRemaining.Location = new System.Drawing.Point(1012, 0);
            lblTimeRemaining.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblTimeRemaining.Name = "lblTimeRemaining";
            lblTimeRemaining.Size = new System.Drawing.Size(104, 38);
            lblTimeRemaining.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblTimeRemaining.TabIndex = 1;
            lblTimeRemaining.Text = "00:00";
            lblTimeRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTimeElapsed
            // 
            lblTimeElapsed.Dock = System.Windows.Forms.DockStyle.Left;
            lblTimeElapsed.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblTimeElapsed.ForeColor = System.Drawing.SystemColors.ControlText;
            lblTimeElapsed.Location = new System.Drawing.Point(0, 0);
            lblTimeElapsed.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblTimeElapsed.Name = "lblTimeElapsed";
            lblTimeElapsed.Size = new System.Drawing.Size(116, 38);
            lblTimeElapsed.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblTimeElapsed.TabIndex = 0;
            lblTimeElapsed.Text = "00:00";
            lblTimeElapsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picCover
            // 
            picCover.BackColor = System.Drawing.Color.White;
            picCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            picCover.Dock = System.Windows.Forms.DockStyle.Left;
            picCover.Location = new System.Drawing.Point(0, 8);
            picCover.Margin = new System.Windows.Forms.Padding(6);
            picCover.Name = "picCover";
            picCover.Size = new System.Drawing.Size(145, 150);
            picCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            picCover.TabIndex = 16;
            picCover.TabStop = false;
            picCover.Visible = false;
            // 
            // pnlRight
            // 
            pnlRight.BackColor = System.Drawing.Color.Transparent;
            pnlRight.Controls.Add(pnlVolume);
            pnlRight.Controls.Add(picVisuals);
            pnlRight.Controls.Add(volumeLevels);
            pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            pnlRight.Location = new System.Drawing.Point(1658, 0);
            pnlRight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pnlRight.Name = "pnlRight";
            pnlRight.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            pnlRight.Size = new System.Drawing.Size(385, 166);
            pnlRight.TabIndex = 26;
            // 
            // pnlVolume
            // 
            pnlVolume.BackColor = System.Drawing.Color.Transparent;
            pnlVolume.Controls.Add(sldVolume);
            pnlVolume.Controls.Add(lblVolume);
            pnlVolume.Controls.Add(lblVolumCaption);
            pnlVolume.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlVolume.Location = new System.Drawing.Point(7, 120);
            pnlVolume.Margin = new System.Windows.Forms.Padding(6);
            pnlVolume.Name = "pnlVolume";
            pnlVolume.Size = new System.Drawing.Size(371, 38);
            pnlVolume.TabIndex = 26;
            // 
            // sldVolume
            // 
            sldVolume.Animated = false;
            sldVolume.AnimationSize = 0.2F;
            sldVolume.AnimationSpeed = MediaSlider.MediaSlider.AnimateSpeed.Normal;
            sldVolume.AutoScrollMargin = new System.Drawing.Size(0, 0);
            sldVolume.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            sldVolume.BackColor = System.Drawing.SystemColors.Control;
            sldVolume.BackgroundImage = null;
            sldVolume.BackGroundImage = null;
            sldVolume.ButtonAccentColor = System.Drawing.Color.FromArgb(128, 64, 64, 64);
            sldVolume.ButtonBorderColor = System.Drawing.Color.FromArgb(133, 158, 191);
            sldVolume.ButtonColor = System.Drawing.Color.FromArgb(160, 0, 0, 0);
            sldVolume.ButtonCornerRadius = 2U;
            sldVolume.ButtonSize = new System.Drawing.Size(24, 12);
            sldVolume.ButtonStyle = MediaSlider.MediaSlider.ButtonType.GlassInline;
            sldVolume.Dock = System.Windows.Forms.DockStyle.Bottom;
            sldVolume.LargeChange = 2;
            sldVolume.Location = new System.Drawing.Point(55, 5);
            sldVolume.Margin = new System.Windows.Forms.Padding(0);
            sldVolume.Maximum = 10;
            sldVolume.Minimum = 0;
            sldVolume.Name = "sldVolume";
            sldVolume.Orientation = System.Windows.Forms.Orientation.Horizontal;
            sldVolume.ResizeRedraw = true;
            sldVolume.ShowButtonOnHover = false;
            sldVolume.Size = new System.Drawing.Size(265, 33);
            sldVolume.SliderFlyOut = MediaSlider.MediaSlider.FlyOutStyle.None;
            sldVolume.SmallChange = 1;
            sldVolume.SmoothScrolling = false;
            sldVolume.TabIndex = 2;
            sldVolume.TickColor = System.Drawing.Color.DarkOliveGreen;
            sldVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            sldVolume.TickType = MediaSlider.MediaSlider.TickMode.Standard;
            sldVolume.TrackBorderColor = System.Drawing.Color.FromArgb(133, 158, 191);
            sldVolume.TrackDepth = 6;
            sldVolume.TrackFillColor = System.Drawing.Color.Transparent;
            sldVolume.TrackProgressColor = System.Drawing.Color.FromArgb(255, 228, 138);
            sldVolume.TrackShadow = false;
            sldVolume.TrackShadowColor = System.Drawing.Color.DarkGray;
            sldVolume.TrackStyle = MediaSlider.MediaSlider.TrackType.Progress;
            sldVolume.Value = 0;
            sldVolume.WheelScrollTicks = 3;
            // 
            // lblVolume
            // 
            lblVolume.Dock = System.Windows.Forms.DockStyle.Right;
            lblVolume.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblVolume.ForeColor = System.Drawing.SystemColors.ControlText;
            lblVolume.Location = new System.Drawing.Point(320, 0);
            lblVolume.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblVolume.Name = "lblVolume";
            lblVolume.Size = new System.Drawing.Size(51, 38);
            lblVolume.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblVolume.TabIndex = 1;
            lblVolume.Text = "100";
            lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVolumCaption
            // 
            lblVolumCaption.Dock = System.Windows.Forms.DockStyle.Left;
            lblVolumCaption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblVolumCaption.ForeColor = System.Drawing.SystemColors.ControlText;
            lblVolumCaption.Location = new System.Drawing.Point(0, 0);
            lblVolumCaption.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblVolumCaption.Name = "lblVolumCaption";
            lblVolumCaption.Size = new System.Drawing.Size(55, 38);
            lblVolumCaption.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblVolumCaption.TabIndex = 0;
            lblVolumCaption.Text = "Vol:";
            lblVolumCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picVisuals
            // 
            picVisuals.BackColor = System.Drawing.Color.Transparent;
            picVisuals.Location = new System.Drawing.Point(7, 8);
            picVisuals.Margin = new System.Windows.Forms.Padding(6);
            picVisuals.Name = "picVisuals";
            picVisuals.Size = new System.Drawing.Size(370, 99);
            picVisuals.TabIndex = 28;
            picVisuals.TabStop = false;
            // 
            // volumeLevels
            // 
            volumeLevels.BackColor = System.Drawing.Color.Transparent;
            volumeLevels.Dock = System.Windows.Forms.DockStyle.Fill;
            volumeLevels.Location = new System.Drawing.Point(7, 8);
            volumeLevels.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            volumeLevels.Name = "volumeLevels";
            volumeLevels.Padding = new System.Windows.Forms.Padding(0, 8, 0, 15);
            volumeLevels.Size = new System.Drawing.Size(371, 150);
            volumeLevels.TabIndex = 29;
            volumeLevels.Visible = false;
            // 
            // pnlLeft
            // 
            pnlLeft.Controls.Add(flpRankButtons);
            pnlLeft.Controls.Add(flpButtons);
            pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            pnlLeft.Location = new System.Drawing.Point(0, 0);
            pnlLeft.Margin = new System.Windows.Forms.Padding(6);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            pnlLeft.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.PanelAlternate;
            pnlLeft.Size = new System.Drawing.Size(390, 166);
            pnlLeft.TabIndex = 20;
            // 
            // flpRankButtons
            // 
            flpRankButtons.BackColor = System.Drawing.Color.Transparent;
            flpRankButtons.Controls.Add(btnRankExcellent);
            flpRankButtons.Controls.Add(btnRankVeryGood);
            flpRankButtons.Controls.Add(btnRankGood);
            flpRankButtons.Controls.Add(btnRankBearable);
            flpRankButtons.Controls.Add(btnRankForbidden);
            flpRankButtons.Dock = System.Windows.Forms.DockStyle.Top;
            flpRankButtons.Location = new System.Drawing.Point(7, 88);
            flpRankButtons.Margin = new System.Windows.Forms.Padding(0);
            flpRankButtons.Name = "flpRankButtons";
            flpRankButtons.Size = new System.Drawing.Size(376, 70);
            flpRankButtons.TabIndex = 22;
            // 
            // btnRankExcellent
            // 
            btnRankExcellent.Location = new System.Drawing.Point(0, 0);
            btnRankExcellent.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btnRankExcellent.Name = "btnRankExcellent";
            btnRankExcellent.Size = new System.Drawing.Size(70, 70);
            btnRankExcellent.TabIndex = 15;
            btnRankExcellent.Values.Text = "EX";
            // 
            // btnRankVeryGood
            // 
            btnRankVeryGood.Location = new System.Drawing.Point(74, 0);
            btnRankVeryGood.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btnRankVeryGood.Name = "btnRankVeryGood";
            btnRankVeryGood.Size = new System.Drawing.Size(70, 70);
            btnRankVeryGood.TabIndex = 16;
            btnRankVeryGood.Values.Text = "VG";
            // 
            // btnRankGood
            // 
            btnRankGood.Location = new System.Drawing.Point(148, 0);
            btnRankGood.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btnRankGood.Name = "btnRankGood";
            btnRankGood.Size = new System.Drawing.Size(70, 70);
            btnRankGood.TabIndex = 17;
            btnRankGood.Values.Text = "GD";
            // 
            // btnRankBearable
            // 
            btnRankBearable.Location = new System.Drawing.Point(222, 0);
            btnRankBearable.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btnRankBearable.Name = "btnRankBearable";
            btnRankBearable.Size = new System.Drawing.Size(70, 70);
            btnRankBearable.TabIndex = 18;
            btnRankBearable.Values.Text = "OK";
            // 
            // btnRankForbidden
            // 
            btnRankForbidden.Location = new System.Drawing.Point(296, 0);
            btnRankForbidden.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btnRankForbidden.Name = "btnRankForbidden";
            btnRankForbidden.Size = new System.Drawing.Size(70, 70);
            btnRankForbidden.TabIndex = 20;
            btnRankForbidden.Values.Text = "NO";
            // 
            // flpButtons
            // 
            flpButtons.BackColor = System.Drawing.Color.Transparent;
            flpButtons.Controls.Add(btnPrevious);
            flpButtons.Controls.Add(btnReplayMix);
            flpButtons.Controls.Add(btnPlay);
            flpButtons.Controls.Add(btnPause);
            flpButtons.Controls.Add(btnSkipToEnd);
            flpButtons.Controls.Add(btnNext);
            flpButtons.Dock = System.Windows.Forms.DockStyle.Top;
            flpButtons.Location = new System.Drawing.Point(7, 8);
            flpButtons.Margin = new System.Windows.Forms.Padding(6);
            flpButtons.Name = "flpButtons";
            flpButtons.Size = new System.Drawing.Size(376, 80);
            flpButtons.TabIndex = 21;
            // 
            // btnPrevious
            // 
            btnPrevious.Location = new System.Drawing.Point(0, 0);
            btnPrevious.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new System.Drawing.Size(70, 70);
            btnPrevious.TabIndex = 9;
            btnPrevious.Values.Image = Properties.Resources.player_restart;
            btnPrevious.Values.Text = "";
            // 
            // btnReplayMix
            // 
            btnReplayMix.Location = new System.Drawing.Point(74, 0);
            btnReplayMix.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btnReplayMix.Name = "btnReplayMix";
            btnReplayMix.Size = new System.Drawing.Size(70, 70);
            btnReplayMix.TabIndex = 14;
            btnReplayMix.Values.Image = Properties.Resources.player_rew;
            btnReplayMix.Values.Text = "";
            btnReplayMix.Click += btnReplayMix_Click;
            // 
            // btnPlay
            // 
            btnPlay.Location = new System.Drawing.Point(148, 0);
            btnPlay.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new System.Drawing.Size(70, 70);
            btnPlay.TabIndex = 10;
            btnPlay.Values.Image = Properties.Resources.player_play;
            btnPlay.Values.Text = "";
            // 
            // btnPause
            // 
            btnPause.Location = new System.Drawing.Point(222, 0);
            btnPause.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btnPause.Name = "btnPause";
            btnPause.Size = new System.Drawing.Size(70, 70);
            btnPause.TabIndex = 13;
            btnPause.Values.Image = Properties.Resources.player_pause;
            btnPause.Values.Text = "";
            btnPause.Visible = false;
            // 
            // btnSkipToEnd
            // 
            btnSkipToEnd.Location = new System.Drawing.Point(296, 0);
            btnSkipToEnd.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btnSkipToEnd.Name = "btnSkipToEnd";
            btnSkipToEnd.Size = new System.Drawing.Size(70, 70);
            btnSkipToEnd.TabIndex = 11;
            btnSkipToEnd.Values.Image = Properties.Resources.player_fwd;
            btnSkipToEnd.Values.Text = "";
            // 
            // btnNext
            // 
            btnNext.Location = new System.Drawing.Point(0, 70);
            btnNext.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btnNext.Name = "btnNext";
            btnNext.Size = new System.Drawing.Size(70, 70);
            btnNext.TabIndex = 12;
            btnNext.Values.Image = Properties.Resources.player_end;
            btnNext.Values.Text = "";
            // 
            // pnlTrackInfo
            // 
            pnlTrackInfo.Controls.Add(lblNextTrack);
            pnlTrackInfo.Controls.Add(lblPrevTrack);
            pnlTrackInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlTrackInfo.Location = new System.Drawing.Point(0, 166);
            pnlTrackInfo.Margin = new System.Windows.Forms.Padding(0);
            pnlTrackInfo.Name = "pnlTrackInfo";
            pnlTrackInfo.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.PanelAlternate;
            pnlTrackInfo.Size = new System.Drawing.Size(2043, 39);
            pnlTrackInfo.TabIndex = 5;
            // 
            // lblNextTrack
            // 
            lblNextTrack.BackColor = System.Drawing.Color.Transparent;
            lblNextTrack.Dock = System.Windows.Forms.DockStyle.Right;
            lblNextTrack.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblNextTrack.ForeColor = System.Drawing.Color.Silver;
            lblNextTrack.Location = new System.Drawing.Point(1181, 0);
            lblNextTrack.Margin = new System.Windows.Forms.Padding(0);
            lblNextTrack.Name = "lblNextTrack";
            lblNextTrack.Padding = new System.Windows.Forms.Padding(0, 0, 11, 0);
            lblNextTrack.Size = new System.Drawing.Size(862, 39);
            lblNextTrack.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblNextTrack.TabIndex = 1;
            lblNextTrack.Text = "Next Track:";
            lblNextTrack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPrevTrack
            // 
            lblPrevTrack.BackColor = System.Drawing.Color.Transparent;
            lblPrevTrack.Dock = System.Windows.Forms.DockStyle.Left;
            lblPrevTrack.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblPrevTrack.ForeColor = System.Drawing.Color.Silver;
            lblPrevTrack.Location = new System.Drawing.Point(0, 0);
            lblPrevTrack.Margin = new System.Windows.Forms.Padding(0);
            lblPrevTrack.Name = "lblPrevTrack";
            lblPrevTrack.Padding = new System.Windows.Forms.Padding(11, 0, 0, 0);
            lblPrevTrack.Size = new System.Drawing.Size(862, 39);
            lblPrevTrack.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblPrevTrack.TabIndex = 0;
            lblPrevTrack.Text = "Previous Track:";
            lblPrevTrack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // beveledLine1
            // 
            beveledLine1.Dock = System.Windows.Forms.DockStyle.Bottom;
            beveledLine1.Location = new System.Drawing.Point(0, 205);
            beveledLine1.Margin = new System.Windows.Forms.Padding(6);
            beveledLine1.Name = "beveledLine1";
            beveledLine1.Size = new System.Drawing.Size(2043, 2);
            beveledLine1.TabIndex = 2;
            // 
            // pnlPlayerMixCounts
            // 
            pnlPlayerMixCounts.BackColor = System.Drawing.Color.Transparent;
            pnlPlayerMixCounts.Controls.Add(lblPlayerMixOut);
            pnlPlayerMixCounts.Controls.Add(lblPlayerMixIn);
            pnlPlayerMixCounts.Dock = System.Windows.Forms.DockStyle.Right;
            pnlPlayerMixCounts.Location = new System.Drawing.Point(1082, 8);
            pnlPlayerMixCounts.Name = "pnlPlayerMixCounts";
            pnlPlayerMixCounts.Padding = new System.Windows.Forms.Padding(0, 0, 8, 8);
            pnlPlayerMixCounts.Size = new System.Drawing.Size(179, 112);
            pnlPlayerMixCounts.TabIndex = 31;
            // 
            // lblPlayerMixOut
            // 
            lblPlayerMixOut.Dock = System.Windows.Forms.DockStyle.Top;
            lblPlayerMixOut.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblPlayerMixOut.ForeColor = System.Drawing.SystemColors.ControlText;
            lblPlayerMixOut.Location = new System.Drawing.Point(0, 38);
            lblPlayerMixOut.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
            lblPlayerMixOut.Name = "lblPlayerMixOut";
            lblPlayerMixOut.Size = new System.Drawing.Size(171, 42);
            lblPlayerMixOut.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblPlayerMixOut.TabIndex = 1;
            lblPlayerMixOut.Text = "0E 0VG 0G";
            lblPlayerMixOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPlayerMixIn
            // 
            lblPlayerMixIn.Dock = System.Windows.Forms.DockStyle.Top;
            lblPlayerMixIn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblPlayerMixIn.ForeColor = System.Drawing.SystemColors.ControlText;
            lblPlayerMixIn.Location = new System.Drawing.Point(0, 0);
            lblPlayerMixIn.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
            lblPlayerMixIn.Name = "lblPlayerMixIn";
            lblPlayerMixIn.Size = new System.Drawing.Size(171, 38);
            lblPlayerMixIn.Style = Common.Windows.Controls.LabelStyle.Heading;
            lblPlayerMixIn.TabIndex = 0;
            lblPlayerMixIn.Text = "0E 0VG 0G";
            lblPlayerMixIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PlayerDetails
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlBackground);
            Controls.Add(pnlTrackInfo);
            Controls.Add(beveledLine1);
            Margin = new System.Windows.Forms.Padding(6);
            Name = "PlayerDetails";
            Size = new System.Drawing.Size(2043, 207);
            ((System.ComponentModel.ISupportInitialize)pnlBackground).EndInit();
            pnlBackground.ResumeLayout(false);
            pnlMiddle.ResumeLayout(false);
            panel1.ResumeLayout(false);
            pnlSlider.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picCover).EndInit();
            pnlRight.ResumeLayout(false);
            pnlVolume.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picVisuals).EndInit();
            ((System.ComponentModel.ISupportInitialize)pnlLeft).EndInit();
            pnlLeft.ResumeLayout(false);
            flpRankButtons.ResumeLayout(false);
            flpButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pnlTrackInfo).EndInit();
            pnlTrackInfo.ResumeLayout(false);
            pnlPlayerMixCounts.ResumeLayout(false);
            ResumeLayout(false);

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
        private VolumeLevels volumeLevels;
        private System.Windows.Forms.PictureBox picVisuals;
        private System.Windows.Forms.FlowLayoutPanel flpRankButtons;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRankExcellent;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRankVeryGood;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRankGood;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRankBearable;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRankForbidden;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel pnlTrackInfo;
        private Halloumi.Common.Windows.Controls.Label lblPrevTrack;
        private Halloumi.Common.Windows.Controls.Label lblNextTrack;
        private System.Windows.Forms.Panel pnlPlayerMixCounts;
        private Common.Windows.Controls.Label lblPlayerMixOut;
        private Common.Windows.Controls.Label lblPlayerMixIn;
    }
}
