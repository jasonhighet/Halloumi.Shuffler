namespace Halloumi.Shuffler.Forms
{
    partial class FrmGeneratePlaylist
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
            this.components = new System.ComponentModel.Container();
            this.pnlButtons = new Halloumi.Common.Windows.Controls.Panel();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new Halloumi.Common.Windows.Controls.Button();
            this.btnStop = new Halloumi.Common.Windows.Controls.Button();
            this.btnOK = new Halloumi.Common.Windows.Controls.Button();
            this.btnStart = new Halloumi.Common.Windows.Controls.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.beveledLine = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.pnlMain = new Halloumi.Common.Windows.Controls.Panel();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new Halloumi.Common.Windows.Controls.Label();
            this.label1 = new Halloumi.Common.Windows.Controls.Label();
            this.label4 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbDirection = new Halloumi.Common.Windows.Controls.ComboBox();
            this.cmbAllowBearable = new Halloumi.Common.Windows.Controls.ComboBox();
            this.btnExcludeTracks = new Halloumi.Common.Windows.Controls.FileSelectButton();
            this.txtExcludeTracks = new Halloumi.Common.Windows.Controls.TextBox();
            this.chkExlcudeMixesOnly = new Halloumi.Common.Windows.Controls.CheckBox();
            this.label7 = new Halloumi.Common.Windows.Controls.Label();
            this.label3 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbMode = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label8 = new Halloumi.Common.Windows.Controls.Label();
            this.chkRestrictGenreClumping = new Halloumi.Common.Windows.Controls.CheckBox();
            this.label2 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbApproxLength = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label6 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbExtendedMixes = new Halloumi.Common.Windows.Controls.ComboBox();
            this.chkRestrictArtistClumping = new Halloumi.Common.Windows.Controls.CheckBox();
            this.label9 = new Halloumi.Common.Windows.Controls.Label();
            this.chkDisplayedTracksOnly = new Halloumi.Common.Windows.Controls.CheckBox();
            this.label10 = new Halloumi.Common.Windows.Controls.Label();
            this.chkRestrictTitleClumping = new Halloumi.Common.Windows.Controls.CheckBox();
            this.label11 = new Halloumi.Common.Windows.Controls.Label();
            this.label13 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbTracksToGenerate = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label12 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbKeyMixing = new Halloumi.Common.Windows.Controls.ComboBox();
            this.cmbContinueMix = new Halloumi.Common.Windows.Controls.ComboBox();
            this.lblStatus = new Halloumi.Common.Windows.Controls.Label();
            this.pnlButtons.SuspendLayout();
            this.flpButtons.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.tblMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDirection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAllowBearable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbApproxLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExtendedMixes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTracksToGenerate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbKeyMixing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbContinueMix)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.flpButtons);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 491);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(4);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(561, 53);
            this.pnlButtons.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlButtons.TabIndex = 38;
            // 
            // flpButtons
            // 
            this.flpButtons.BackColor = System.Drawing.Color.Transparent;
            this.flpButtons.Controls.Add(this.btnCancel);
            this.flpButtons.Controls.Add(this.btnStop);
            this.flpButtons.Controls.Add(this.btnOK);
            this.flpButtons.Controls.Add(this.btnStart);
            this.flpButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpButtons.Location = new System.Drawing.Point(0, -2);
            this.flpButtons.Margin = new System.Windows.Forms.Padding(4);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Padding = new System.Windows.Forms.Padding(7, 2, 7, 6);
            this.flpButtons.Size = new System.Drawing.Size(561, 55);
            this.flpButtons.TabIndex = 16;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(426, 7);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 38);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(300, 7);
            this.btnStop.Margin = new System.Windows.Forms.Padding(5);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(116, 38);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(174, 7);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(116, 38);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(48, 7);
            this.btnStart.Margin = new System.Windows.Forms.Padding(5);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(116, 38);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // timer
            // 
            this.timer.Interval = 500;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // beveledLine
            // 
            this.beveledLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.beveledLine.Location = new System.Drawing.Point(0, 489);
            this.beveledLine.Margin = new System.Windows.Forms.Padding(4);
            this.beveledLine.Name = "beveledLine";
            this.beveledLine.Size = new System.Drawing.Size(561, 2);
            this.beveledLine.TabIndex = 41;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMain.Controls.Add(this.tblMain);
            this.pnlMain.Controls.Add(this.lblStatus);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(4);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlMain.Size = new System.Drawing.Size(561, 489);
            this.pnlMain.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlMain.TabIndex = 42;
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 4;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 198F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tblMain.Controls.Add(this.label5, 0, 12);
            this.tblMain.Controls.Add(this.label1, 0, 4);
            this.tblMain.Controls.Add(this.label4, 0, 3);
            this.tblMain.Controls.Add(this.cmbDirection, 1, 3);
            this.tblMain.Controls.Add(this.cmbAllowBearable, 1, 4);
            this.tblMain.Controls.Add(this.btnExcludeTracks, 3, 12);
            this.tblMain.Controls.Add(this.txtExcludeTracks, 1, 12);
            this.tblMain.Controls.Add(this.chkExlcudeMixesOnly, 1, 13);
            this.tblMain.Controls.Add(this.label7, 0, 9);
            this.tblMain.Controls.Add(this.label3, 0, 1);
            this.tblMain.Controls.Add(this.cmbMode, 1, 1);
            this.tblMain.Controls.Add(this.label8, 0, 8);
            this.tblMain.Controls.Add(this.chkRestrictGenreClumping, 1, 8);
            this.tblMain.Controls.Add(this.label2, 0, 2);
            this.tblMain.Controls.Add(this.cmbApproxLength, 1, 2);
            this.tblMain.Controls.Add(this.label6, 0, 5);
            this.tblMain.Controls.Add(this.cmbExtendedMixes, 1, 5);
            this.tblMain.Controls.Add(this.chkRestrictArtistClumping, 1, 9);
            this.tblMain.Controls.Add(this.label9, 0, 11);
            this.tblMain.Controls.Add(this.chkDisplayedTracksOnly, 1, 11);
            this.tblMain.Controls.Add(this.label10, 0, 10);
            this.tblMain.Controls.Add(this.chkRestrictTitleClumping, 1, 10);
            this.tblMain.Controls.Add(this.label11, 0, 7);
            this.tblMain.Controls.Add(this.label13, 0, 0);
            this.tblMain.Controls.Add(this.cmbTracksToGenerate, 1, 0);
            this.tblMain.Controls.Add(this.label12, 0, 6);
            this.tblMain.Controls.Add(this.cmbKeyMixing, 1, 6);
            this.tblMain.Controls.Add(this.cmbContinueMix, 1, 7);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(13, 12);
            this.tblMain.Margin = new System.Windows.Forms.Padding(4);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 14;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.140437F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143522F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142806F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143052F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143052F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143052F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.14003F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142641F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143052F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.145394F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142707F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142737F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143052F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.144472F));
            this.tblMain.Size = new System.Drawing.Size(535, 431);
            this.tblMain.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(4, 360);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label5.Size = new System.Drawing.Size(190, 30);
            this.label5.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label5.TabIndex = 28;
            this.label5.Text = "Exclude Tracks:";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 120);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label1.Size = new System.Drawing.Size(167, 30);
            this.label1.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label1.TabIndex = 21;
            this.label1.Text = "Allow Bearables:";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(4, 90);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label4.Size = new System.Drawing.Size(167, 30);
            this.label4.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label4.TabIndex = 20;
            this.label4.Text = "Direction:";
            // 
            // cmbDirection
            // 
            this.cmbDirection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbDirection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDirection.DropBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.InputControlCustom1;
            this.cmbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDirection.DropDownWidth = 264;
            this.cmbDirection.ErrorProvider = null;
            this.cmbDirection.IsRequired = true;
            this.cmbDirection.Location = new System.Drawing.Point(202, 94);
            this.cmbDirection.Margin = new System.Windows.Forms.Padding(4);
            this.cmbDirection.Name = "cmbDirection";
            this.cmbDirection.Size = new System.Drawing.Size(195, 25);
            this.cmbDirection.TabIndex = 24;
            // 
            // cmbAllowBearable
            // 
            this.cmbAllowBearable.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbAllowBearable.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbAllowBearable.DropBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.InputControlCustom1;
            this.cmbAllowBearable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAllowBearable.DropDownWidth = 264;
            this.cmbAllowBearable.ErrorProvider = null;
            this.cmbAllowBearable.IsRequired = true;
            this.cmbAllowBearable.Location = new System.Drawing.Point(202, 124);
            this.cmbAllowBearable.Margin = new System.Windows.Forms.Padding(4);
            this.cmbAllowBearable.Name = "cmbAllowBearable";
            this.cmbAllowBearable.Size = new System.Drawing.Size(195, 25);
            this.cmbAllowBearable.TabIndex = 23;
            // 
            // btnExcludeTracks
            // 
            this.btnExcludeTracks.AssociatedControl = this.txtExcludeTracks;
            this.btnExcludeTracks.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnExcludeTracks.Filter = "Playlist files (*.m3u)|*.m3u";
            this.btnExcludeTracks.Location = new System.Drawing.Point(486, 364);
            this.btnExcludeTracks.Margin = new System.Windows.Forms.Padding(4);
            this.btnExcludeTracks.Name = "btnExcludeTracks";
            this.btnExcludeTracks.Size = new System.Drawing.Size(45, 21);
            this.btnExcludeTracks.TabIndex = 29;
            this.btnExcludeTracks.Title = "Open playlist of excluded tracks";
            this.btnExcludeTracks.Values.Text = "...";
            // 
            // txtExcludeTracks
            // 
            this.tblMain.SetColumnSpan(this.txtExcludeTracks, 2);
            this.txtExcludeTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExcludeTracks.ErrorProvider = null;
            this.txtExcludeTracks.Location = new System.Drawing.Point(202, 364);
            this.txtExcludeTracks.Margin = new System.Windows.Forms.Padding(4);
            this.txtExcludeTracks.MaximumValue = 2147483647D;
            this.txtExcludeTracks.MinimumValue = -2147483648D;
            this.txtExcludeTracks.Name = "txtExcludeTracks";
            this.txtExcludeTracks.Size = new System.Drawing.Size(276, 27);
            this.txtExcludeTracks.TabIndex = 30;
            // 
            // chkExlcudeMixesOnly
            // 
            this.chkExlcudeMixesOnly.Location = new System.Drawing.Point(201, 392);
            this.chkExlcudeMixesOnly.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkExlcudeMixesOnly.Name = "chkExlcudeMixesOnly";
            this.chkExlcudeMixesOnly.Size = new System.Drawing.Size(156, 24);
            this.chkExlcudeMixesOnly.TabIndex = 33;
            this.chkExlcudeMixesOnly.Values.Text = "Exclude Mixes Only";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(4, 270);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label7.Size = new System.Drawing.Size(167, 30);
            this.label7.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label7.TabIndex = 34;
            this.label7.Text = "Artist Clumping:";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(4, 30);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label3.Size = new System.Drawing.Size(167, 30);
            this.label3.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label3.TabIndex = 26;
            this.label3.Text = "Mode:";
            // 
            // cmbMode
            // 
            this.cmbMode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbMode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbMode.DropBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.InputControlCustom1;
            this.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMode.DropDownWidth = 264;
            this.cmbMode.ErrorProvider = null;
            this.cmbMode.IsRequired = true;
            this.cmbMode.Location = new System.Drawing.Point(202, 34);
            this.cmbMode.Margin = new System.Windows.Forms.Padding(4);
            this.cmbMode.Name = "cmbMode";
            this.cmbMode.Size = new System.Drawing.Size(195, 25);
            this.cmbMode.TabIndex = 27;
            this.cmbMode.SelectedIndexChanged += new System.EventHandler(this.cmbMode_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(4, 240);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label8.Size = new System.Drawing.Size(167, 30);
            this.label8.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label8.TabIndex = 36;
            this.label8.Text = "Genre Clumping:";
            // 
            // chkRestrictGenreClumping
            // 
            this.chkRestrictGenreClumping.Location = new System.Drawing.Point(201, 242);
            this.chkRestrictGenreClumping.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkRestrictGenreClumping.Name = "chkRestrictGenreClumping";
            this.chkRestrictGenreClumping.Size = new System.Drawing.Size(190, 24);
            this.chkRestrictGenreClumping.TabIndex = 37;
            this.chkRestrictGenreClumping.Values.Text = "Restrict Genre Clumping";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label2.Size = new System.Drawing.Size(167, 30);
            this.label2.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label2.TabIndex = 22;
            this.label2.Text = "Approximate Length:";
            // 
            // cmbApproxLength
            // 
            this.cmbApproxLength.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbApproxLength.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbApproxLength.DropBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.InputControlCustom1;
            this.cmbApproxLength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbApproxLength.DropDownWidth = 264;
            this.cmbApproxLength.ErrorProvider = null;
            this.cmbApproxLength.IsRequired = true;
            this.cmbApproxLength.Location = new System.Drawing.Point(202, 64);
            this.cmbApproxLength.Margin = new System.Windows.Forms.Padding(4);
            this.cmbApproxLength.Name = "cmbApproxLength";
            this.cmbApproxLength.Size = new System.Drawing.Size(195, 25);
            this.cmbApproxLength.TabIndex = 25;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(4, 150);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label6.Size = new System.Drawing.Size(167, 30);
            this.label6.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label6.TabIndex = 31;
            this.label6.Text = "Use Extended Mixes:";
            // 
            // cmbExtendedMixes
            // 
            this.cmbExtendedMixes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbExtendedMixes.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbExtendedMixes.DropBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.InputControlCustom1;
            this.cmbExtendedMixes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExtendedMixes.DropDownWidth = 264;
            this.cmbExtendedMixes.ErrorProvider = null;
            this.cmbExtendedMixes.IsRequired = true;
            this.cmbExtendedMixes.Items.AddRange(new object[] {
            "Any",
            "Always",
            "Never"});
            this.cmbExtendedMixes.Location = new System.Drawing.Point(202, 154);
            this.cmbExtendedMixes.Margin = new System.Windows.Forms.Padding(4);
            this.cmbExtendedMixes.Name = "cmbExtendedMixes";
            this.cmbExtendedMixes.Size = new System.Drawing.Size(195, 25);
            this.cmbExtendedMixes.TabIndex = 32;
            // 
            // chkRestrictArtistClumping
            // 
            this.chkRestrictArtistClumping.Location = new System.Drawing.Point(201, 272);
            this.chkRestrictArtistClumping.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkRestrictArtistClumping.Name = "chkRestrictArtistClumping";
            this.chkRestrictArtistClumping.Size = new System.Drawing.Size(186, 24);
            this.chkRestrictArtistClumping.TabIndex = 35;
            this.chkRestrictArtistClumping.Values.Text = "Restrict Artist Clumping";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(4, 330);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label9.Size = new System.Drawing.Size(167, 30);
            this.label9.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label9.TabIndex = 38;
            this.label9.Text = "Displayed Tracks:";
            // 
            // chkDisplayedTracksOnly
            // 
            this.chkDisplayedTracksOnly.Location = new System.Drawing.Point(201, 332);
            this.chkDisplayedTracksOnly.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkDisplayedTracksOnly.Name = "chkDisplayedTracksOnly";
            this.chkDisplayedTracksOnly.Size = new System.Drawing.Size(196, 24);
            this.chkDisplayedTracksOnly.TabIndex = 39;
            this.chkDisplayedTracksOnly.Values.Text = "Use displayed tracks only";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(4, 300);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label10.Size = new System.Drawing.Size(167, 30);
            this.label10.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label10.TabIndex = 40;
            this.label10.Text = "Title Clumping:";
            // 
            // chkRestrictTitleClumping
            // 
            this.chkRestrictTitleClumping.Location = new System.Drawing.Point(201, 302);
            this.chkRestrictTitleClumping.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkRestrictTitleClumping.Name = "chkRestrictTitleClumping";
            this.chkRestrictTitleClumping.Size = new System.Drawing.Size(178, 24);
            this.chkRestrictTitleClumping.TabIndex = 41;
            this.chkRestrictTitleClumping.Values.Text = "Restrict Title Clumping";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(4, 210);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label11.Size = new System.Drawing.Size(167, 30);
            this.label11.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label11.TabIndex = 42;
            this.label11.Text = "Continue Mix";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(4, 0);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label13.Size = new System.Drawing.Size(167, 30);
            this.label13.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label13.TabIndex = 45;
            this.label13.Text = "Tracks To Generate:";
            // 
            // cmbTracksToGenerate
            // 
            this.cmbTracksToGenerate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbTracksToGenerate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbTracksToGenerate.DropBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.InputControlCustom1;
            this.cmbTracksToGenerate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTracksToGenerate.DropDownWidth = 264;
            this.cmbTracksToGenerate.ErrorProvider = null;
            this.cmbTracksToGenerate.IsRequired = true;
            this.cmbTracksToGenerate.Location = new System.Drawing.Point(202, 4);
            this.cmbTracksToGenerate.Margin = new System.Windows.Forms.Padding(4);
            this.cmbTracksToGenerate.Name = "cmbTracksToGenerate";
            this.cmbTracksToGenerate.Size = new System.Drawing.Size(195, 25);
            this.cmbTracksToGenerate.TabIndex = 47;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(4, 180);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label12.Size = new System.Drawing.Size(167, 30);
            this.label12.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label12.TabIndex = 48;
            this.label12.Text = "Key Mixing:";
            // 
            // cmbKeyMixing
            // 
            this.cmbKeyMixing.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbKeyMixing.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbKeyMixing.DropBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.InputControlCustom1;
            this.cmbKeyMixing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyMixing.DropDownWidth = 264;
            this.cmbKeyMixing.ErrorProvider = null;
            this.cmbKeyMixing.IsRequired = true;
            this.cmbKeyMixing.Location = new System.Drawing.Point(202, 184);
            this.cmbKeyMixing.Margin = new System.Windows.Forms.Padding(4);
            this.cmbKeyMixing.Name = "cmbKeyMixing";
            this.cmbKeyMixing.Size = new System.Drawing.Size(195, 25);
            this.cmbKeyMixing.TabIndex = 49;
            // 
            // cmbContinueMix
            // 
            this.cmbContinueMix.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbContinueMix.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbContinueMix.DropBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.InputControlCustom1;
            this.cmbContinueMix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbContinueMix.DropDownWidth = 264;
            this.cmbContinueMix.ErrorProvider = null;
            this.cmbContinueMix.IsRequired = true;
            this.cmbContinueMix.Location = new System.Drawing.Point(202, 214);
            this.cmbContinueMix.Margin = new System.Windows.Forms.Padding(4);
            this.cmbContinueMix.Name = "cmbContinueMix";
            this.cmbContinueMix.Size = new System.Drawing.Size(195, 25);
            this.cmbContinueMix.TabIndex = 50;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblStatus.Location = new System.Drawing.Point(13, 443);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(535, 34);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Generating playlist...";
            // 
            // frmGeneratePlaylist
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(561, 544);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.beveledLine);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmGeneratePlaylist";
            this.Text = "Halloumi : Shuffler : Generate Playlist";
            this.UseApplicationIcon = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGeneratePlaylist_FormClosing);
            this.pnlButtons.ResumeLayout(false);
            this.flpButtons.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            this.tblMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDirection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAllowBearable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbApproxLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExtendedMixes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTracksToGenerate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbKeyMixing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbContinueMix)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlButtons;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private Halloumi.Common.Windows.Controls.Button btnCancel;
        private Halloumi.Common.Windows.Controls.Button btnStop;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Timer timer;
        private Halloumi.Common.Windows.Controls.BeveledLine beveledLine;
        private Halloumi.Common.Windows.Controls.Panel pnlMain;
        private Halloumi.Common.Windows.Controls.Label lblStatus;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private Halloumi.Common.Windows.Controls.Label label2;
        private Halloumi.Common.Windows.Controls.Label label1;
        private Halloumi.Common.Windows.Controls.Label label4;
        private Halloumi.Common.Windows.Controls.ComboBox cmbDirection;
        private Halloumi.Common.Windows.Controls.ComboBox cmbAllowBearable;
        private Halloumi.Common.Windows.Controls.ComboBox cmbApproxLength;
        private Halloumi.Common.Windows.Controls.Button btnStart;
        private Halloumi.Common.Windows.Controls.ComboBox cmbMode;
        private Halloumi.Common.Windows.Controls.Label label3;
        private Halloumi.Common.Windows.Controls.Label label5;
        private Halloumi.Common.Windows.Controls.FileSelectButton btnExcludeTracks;
        private Halloumi.Common.Windows.Controls.TextBox txtExcludeTracks;
        private Common.Windows.Controls.ComboBox cmbExtendedMixes;
        private Common.Windows.Controls.Label label6;
        private Common.Windows.Controls.CheckBox chkExlcudeMixesOnly;
        private Halloumi.Common.Windows.Controls.CheckBox chkRestrictArtistClumping;
        private Halloumi.Common.Windows.Controls.Label label7;
        private Halloumi.Common.Windows.Controls.Label label8;
        private Halloumi.Common.Windows.Controls.CheckBox chkRestrictGenreClumping;
        private Halloumi.Common.Windows.Controls.Label label9;
        private Halloumi.Common.Windows.Controls.CheckBox chkDisplayedTracksOnly;
        private Halloumi.Common.Windows.Controls.Label label10;
        private Halloumi.Common.Windows.Controls.CheckBox chkRestrictTitleClumping;
        private Common.Windows.Controls.Label label11;
        private Halloumi.Common.Windows.Controls.Label label13;
        private Halloumi.Common.Windows.Controls.ComboBox cmbTracksToGenerate;
        private Halloumi.Common.Windows.Controls.Button btnOK;
        private Halloumi.Common.Windows.Controls.Label label12;
        private Halloumi.Common.Windows.Controls.ComboBox cmbKeyMixing;
        private Halloumi.Common.Windows.Controls.ComboBox cmbContinueMix;
    }
}