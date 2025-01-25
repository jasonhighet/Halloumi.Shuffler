namespace Halloumi.Shuffler.Forms 
{
    partial class FrmShufflerDetails
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
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("Sample #1");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("Sample #2");
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("Sample #3");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("Sample #4");
            this.pnlMain = new Halloumi.Common.Windows.Controls.Panel();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new Halloumi.Common.Windows.Controls.Panel();
            this.flpTrackFX = new System.Windows.Forms.FlowLayoutPanel();
            this.chkShowTrackFX = new Halloumi.Common.Windows.Controls.CheckBox();
            this.cmbTrackFX = new Halloumi.Common.Windows.Controls.ComboBox();
            this.flpRight = new System.Windows.Forms.FlowLayoutPanel();
            this.label12 = new Halloumi.Common.Windows.Controls.Label();
            this.rdbDelay1 = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rdbDelay2 = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rdbDelay3 = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rdbDelay4 = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.btnUpdateTrackFX = new Halloumi.Common.Windows.Controls.Button();
            this.btnDeleteTrackFX = new Halloumi.Common.Windows.Controls.Button();
            this.btnAddTrackFX = new Halloumi.Common.Windows.Controls.Button();
            this.btnClearTrackFX = new Halloumi.Common.Windows.Controls.Button();
            this.btnTrackFXZoom = new Halloumi.Common.Windows.Controls.Button();
            this.kryptonHeader3 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.panel3 = new Halloumi.Common.Windows.Controls.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new Halloumi.Common.Windows.Controls.Label();
            this.btnAddSample = new Halloumi.Common.Windows.Controls.Button();
            this.label3 = new Halloumi.Common.Windows.Controls.Label();
            this.label4 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbSampleLength = new Halloumi.Shuffler.Controls.SecondsComboBox();
            this.txtSampleStartPosition = new Halloumi.Common.Windows.Controls.SecondsTextBox();
            this.chkLoopSample = new Halloumi.Common.Windows.Controls.CheckBox();
            this.btnSampleUpdate = new Halloumi.Common.Windows.Controls.Button();
            this.lblSampleBPM = new Halloumi.Common.Windows.Controls.Label();
            this.btnZoomSample = new Halloumi.Common.Windows.Controls.Button();
            this.lstSamples = new Halloumi.Common.Windows.Controls.ListView();
            this.colSample = new System.Windows.Forms.ColumnHeader();
            this.btnRemoveSample = new Halloumi.Common.Windows.Controls.Button();
            this.btnRenameSample = new Halloumi.Common.Windows.Controls.Button();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.pnlFadeOut = new Halloumi.Common.Windows.Controls.Panel();
            this.tblFadeOut = new System.Windows.Forms.TableLayoutPanel();
            this.label19 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbFadeOutLoopCount = new Halloumi.Common.Windows.Controls.ComboBox();
            this.lblFadeOutLoopCount = new Halloumi.Common.Windows.Controls.Label();
            this.lblFadeOutStartPosition = new Halloumi.Common.Windows.Controls.Label();
            this.lblFadeOutLength = new Halloumi.Common.Windows.Controls.Label();
            this.txtFadeOutStartPosition = new Halloumi.Common.Windows.Controls.SecondsTextBox();
            this.lblPowerDown = new Halloumi.Common.Windows.Controls.Label();
            this.chkPowerDown = new Halloumi.Common.Windows.Controls.CheckBox();
            this.btnFadeOutUpdate = new Halloumi.Common.Windows.Controls.Button();
            this.lblEndBPM = new Halloumi.Common.Windows.Controls.Label();
            this.btnZoomFadeOut = new Halloumi.Common.Windows.Controls.Button();
            this.label15 = new Halloumi.Common.Windows.Controls.Label();
            this.chkUseSkipSection = new Halloumi.Common.Windows.Controls.CheckBox();
            this.label18 = new Halloumi.Common.Windows.Controls.Label();
            this.txtSkipStart = new Halloumi.Common.Windows.Controls.SecondsTextBox();
            this.btnSkipUpdate = new Halloumi.Common.Windows.Controls.Button();
            this.cmbSkipLength = new Halloumi.Shuffler.Controls.SecondsComboBox();
            this.btnSkipZoom = new Halloumi.Common.Windows.Controls.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnCopyRight = new Halloumi.Common.Windows.Controls.Button();
            this.cmbCustomFadeOutLength = new Halloumi.Shuffler.Controls.SecondsComboBox();
            this.hdrFadeOut = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.pnlFadeIn = new Halloumi.Common.Windows.Controls.Panel();
            this.tblFadeIn = new System.Windows.Forms.TableLayoutPanel();
            this.btnPreFadeInUpdate = new Halloumi.Common.Windows.Controls.Button();
            this.lblFadeInLoopCount = new Halloumi.Common.Windows.Controls.Label();
            this.lblPreFadeIn = new Halloumi.Common.Windows.Controls.Label();
            this.lblFadeInPosition = new Halloumi.Common.Windows.Controls.Label();
            this.txtFadeInPosition = new Halloumi.Common.Windows.Controls.SecondsTextBox();
            this.txtPreFadeInStartPosition = new Halloumi.Common.Windows.Controls.SecondsTextBox();
            this.lblPreFadeInStartPosition = new Halloumi.Common.Windows.Controls.Label();
            this.lblPreFadeInStartVolume = new Halloumi.Common.Windows.Controls.Label();
            this.lblFadeInLength = new Halloumi.Common.Windows.Controls.Label();
            this.chkUsePreFadeIn = new Halloumi.Common.Windows.Controls.CheckBox();
            this.cmbFadeInLoopCount = new Halloumi.Common.Windows.Controls.ComboBox();
            this.cmbPreFadeInStartVolume = new Halloumi.Common.Windows.Controls.ComboBox();
            this.btnFadeInUpdate = new Halloumi.Common.Windows.Controls.Button();
            this.lblStartBPM = new Halloumi.Common.Windows.Controls.Label();
            this.btnZoomFadeIn = new Halloumi.Common.Windows.Controls.Button();
            this.btnZoomPreFade = new Halloumi.Common.Windows.Controls.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCopyLeft = new Halloumi.Common.Windows.Controls.Button();
            this.cmbCustomFadeInLength = new Halloumi.Shuffler.Controls.SecondsComboBox();
            this.hdrFadeIn = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.trackWave = new Halloumi.Shuffler.Controls.TrackWave();
            this.linLine = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.flpButtonsRight = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new Halloumi.Common.Windows.Controls.Button();
            this.btnOK = new Halloumi.Common.Windows.Controls.Button();
            this.pnlButtons = new Halloumi.Common.Windows.Controls.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flpButtonsLeft = new System.Windows.Forms.FlowLayoutPanel();
            this.label6 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbOutput = new Halloumi.Common.Windows.Controls.ComboBox();
            this.panel1 = new Halloumi.Common.Windows.Controls.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox1 = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label1 = new Halloumi.Common.Windows.Controls.Label();
            this.label2 = new Halloumi.Common.Windows.Controls.Label();
            this.btnCalcStartBPM = new Halloumi.Common.Windows.Controls.Button();
            this.btnCalcEndBPM = new Halloumi.Common.Windows.Controls.Button();
            this.pnlMain.SuspendLayout();
            this.tblMain.SuspendLayout();
            this.panel4.SuspendLayout();
            this.flpTrackFX.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrackFX)).BeginInit();
            this.flpRight.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSampleLength)).BeginInit();
            this.pnlFadeOut.SuspendLayout();
            this.tblFadeOut.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFadeOutLoopCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSkipLength)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomFadeOutLength)).BeginInit();
            this.pnlFadeIn.SuspendLayout();
            this.tblFadeIn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFadeInLoopCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPreFadeInStartVolume)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomFadeInLength)).BeginInit();
            this.flpButtonsRight.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.flpButtonsLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOutput)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMain.Controls.Add(this.tblMain);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.pnlMain.Size = new System.Drawing.Size(1940, 632);
            this.pnlMain.Style = Halloumi.Common.Windows.Controls.PanelStyle.Background;
            this.pnlMain.TabIndex = 0;
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 3;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tblMain.Controls.Add(this.panel4, 0, 2);
            this.tblMain.Controls.Add(this.panel3, 2, 1);
            this.tblMain.Controls.Add(this.pnlFadeOut, 1, 1);
            this.tblMain.Controls.Add(this.pnlFadeIn, 0, 1);
            this.tblMain.Controls.Add(this.trackWave, 0, 0);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(7, 6);
            this.tblMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 3;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 342F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 91F));
            this.tblMain.Size = new System.Drawing.Size(1926, 620);
            this.tblMain.TabIndex = 45;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Control;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblMain.SetColumnSpan(this.panel4, 3);
            this.panel4.Controls.Add(this.flpTrackFX);
            this.panel4.Controls.Add(this.kryptonHeader3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(4, 533);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.panel4.Size = new System.Drawing.Size(1918, 83);
            this.panel4.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.panel4.TabIndex = 49;
            // 
            // flpTrackFX
            // 
            this.flpTrackFX.Controls.Add(this.chkShowTrackFX);
            this.flpTrackFX.Controls.Add(this.cmbTrackFX);
            this.flpTrackFX.Controls.Add(this.flpRight);
            this.flpTrackFX.Controls.Add(this.btnUpdateTrackFX);
            this.flpTrackFX.Controls.Add(this.btnDeleteTrackFX);
            this.flpTrackFX.Controls.Add(this.btnAddTrackFX);
            this.flpTrackFX.Controls.Add(this.btnClearTrackFX);
            this.flpTrackFX.Controls.Add(this.btnTrackFXZoom);
            this.flpTrackFX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTrackFX.Location = new System.Drawing.Point(1, 26);
            this.flpTrackFX.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flpTrackFX.Name = "flpTrackFX";
            this.flpTrackFX.Padding = new System.Windows.Forms.Padding(7, 3, 7, 6);
            this.flpTrackFX.Size = new System.Drawing.Size(1916, 56);
            this.flpTrackFX.TabIndex = 2;
            // 
            // chkShowTrackFX
            // 
            this.chkShowTrackFX.AutoSize = false;
            this.chkShowTrackFX.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.BoldPanel;
            this.chkShowTrackFX.Location = new System.Drawing.Point(11, 7);
            this.chkShowTrackFX.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkShowTrackFX.Name = "chkShowTrackFX";
            this.chkShowTrackFX.Size = new System.Drawing.Size(169, 31);
            this.chkShowTrackFX.TabIndex = 41;
            this.chkShowTrackFX.Values.Text = "Show Track FX";
            this.chkShowTrackFX.CheckedChanged += new System.EventHandler(this.chkShowTrackFX_CheckedChanged);
            // 
            // cmbTrackFX
            // 
            this.cmbTrackFX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTrackFX.DropDownWidth = 72;
            this.cmbTrackFX.ErrorProvider = null;
            this.cmbTrackFX.Items.AddRange(new object[] {
            "Speakers",
            "Monitor",
            "Both"});
            this.cmbTrackFX.Location = new System.Drawing.Point(188, 7);
            this.cmbTrackFX.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbTrackFX.Name = "cmbTrackFX";
            this.cmbTrackFX.Size = new System.Drawing.Size(145, 25);
            this.cmbTrackFX.TabIndex = 47;
            this.cmbTrackFX.SelectedIndexChanged += new System.EventHandler(this.cmbTrackFX_SelectedIndexChanged);
            // 
            // flpRight
            // 
            this.flpRight.BackColor = System.Drawing.Color.Transparent;
            this.flpRight.Controls.Add(this.label12);
            this.flpRight.Controls.Add(this.rdbDelay1);
            this.flpRight.Controls.Add(this.rdbDelay2);
            this.flpRight.Controls.Add(this.rdbDelay3);
            this.flpRight.Controls.Add(this.rdbDelay4);
            this.flpRight.Location = new System.Drawing.Point(341, 7);
            this.flpRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flpRight.Name = "flpRight";
            this.flpRight.Size = new System.Drawing.Size(331, 35);
            this.flpRight.TabIndex = 45;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(4, 0);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 28);
            this.label12.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label12.TabIndex = 57;
            this.label12.Text = "Delay:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rdbDelay1
            // 
            this.rdbDelay1.Location = new System.Drawing.Point(68, 4);
            this.rdbDelay1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdbDelay1.Name = "rdbDelay1";
            this.rdbDelay1.Size = new System.Drawing.Size(46, 24);
            this.rdbDelay1.TabIndex = 62;
            this.rdbDelay1.Tag = "0.5";
            this.rdbDelay1.Values.Text = "1/2";
            // 
            // rdbDelay2
            // 
            this.rdbDelay2.Checked = true;
            this.rdbDelay2.Location = new System.Drawing.Point(122, 4);
            this.rdbDelay2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdbDelay2.Name = "rdbDelay2";
            this.rdbDelay2.Size = new System.Drawing.Size(46, 24);
            this.rdbDelay2.TabIndex = 63;
            this.rdbDelay2.Tag = "0.25";
            this.rdbDelay2.Values.Text = "1/4";
            // 
            // rdbDelay3
            // 
            this.rdbDelay3.Location = new System.Drawing.Point(176, 4);
            this.rdbDelay3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdbDelay3.Name = "rdbDelay3";
            this.rdbDelay3.Size = new System.Drawing.Size(46, 24);
            this.rdbDelay3.TabIndex = 64;
            this.rdbDelay3.Tag = "0.125";
            this.rdbDelay3.Values.Text = "1/8";
            // 
            // rdbDelay4
            // 
            this.rdbDelay4.Location = new System.Drawing.Point(230, 4);
            this.rdbDelay4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdbDelay4.Name = "rdbDelay4";
            this.rdbDelay4.Size = new System.Drawing.Size(55, 24);
            this.rdbDelay4.TabIndex = 65;
            this.rdbDelay4.Tag = "0.0625";
            this.rdbDelay4.Values.Text = "1/16";
            // 
            // btnUpdateTrackFX
            // 
            this.btnUpdateTrackFX.Location = new System.Drawing.Point(681, 8);
            this.btnUpdateTrackFX.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnUpdateTrackFX.Name = "btnUpdateTrackFX";
            this.btnUpdateTrackFX.Size = new System.Drawing.Size(89, 38);
            this.btnUpdateTrackFX.TabIndex = 43;
            this.btnUpdateTrackFX.Text = "Update";
            this.btnUpdateTrackFX.Click += new System.EventHandler(this.btnUpdateTrackFX_Click);
            // 
            // btnDeleteTrackFX
            // 
            this.btnDeleteTrackFX.Location = new System.Drawing.Point(780, 8);
            this.btnDeleteTrackFX.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnDeleteTrackFX.Name = "btnDeleteTrackFX";
            this.btnDeleteTrackFX.Size = new System.Drawing.Size(89, 38);
            this.btnDeleteTrackFX.TabIndex = 42;
            this.btnDeleteTrackFX.Text = "Delete";
            this.btnDeleteTrackFX.Click += new System.EventHandler(this.btnDeleteTrackFX_Click);
            // 
            // btnAddTrackFX
            // 
            this.btnAddTrackFX.Location = new System.Drawing.Point(879, 8);
            this.btnAddTrackFX.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnAddTrackFX.Name = "btnAddTrackFX";
            this.btnAddTrackFX.Size = new System.Drawing.Size(89, 38);
            this.btnAddTrackFX.TabIndex = 44;
            this.btnAddTrackFX.Text = "Add";
            this.btnAddTrackFX.Click += new System.EventHandler(this.btnAddTrackFX_Click);
            // 
            // btnClearTrackFX
            // 
            this.btnClearTrackFX.Location = new System.Drawing.Point(978, 8);
            this.btnClearTrackFX.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnClearTrackFX.Name = "btnClearTrackFX";
            this.btnClearTrackFX.Size = new System.Drawing.Size(89, 38);
            this.btnClearTrackFX.TabIndex = 46;
            this.btnClearTrackFX.Text = "Clear";
            this.btnClearTrackFX.Click += new System.EventHandler(this.btnClearTrackFX_Click);
            // 
            // btnTrackFXZoom
            // 
            this.btnTrackFXZoom.Location = new System.Drawing.Point(1077, 8);
            this.btnTrackFXZoom.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnTrackFXZoom.Name = "btnTrackFXZoom";
            this.btnTrackFXZoom.Size = new System.Drawing.Size(89, 38);
            this.btnTrackFXZoom.TabIndex = 48;
            this.btnTrackFXZoom.Text = "Clear";
            this.btnTrackFXZoom.Click += new System.EventHandler(this.btnTrackFXZoom_Click);
            // 
            // kryptonHeader3
            // 
            this.kryptonHeader3.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader3.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader3.Location = new System.Drawing.Point(1, 1);
            this.kryptonHeader3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.kryptonHeader3.Name = "kryptonHeader3";
            this.kryptonHeader3.Size = new System.Drawing.Size(1916, 25);
            this.kryptonHeader3.StateCommon.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            this.kryptonHeader3.TabIndex = 1;
            this.kryptonHeader3.Values.Description = "";
            this.kryptonHeader3.Values.Heading = "Track FX Automation";
            this.kryptonHeader3.Values.Image = null;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.tableLayoutPanel3);
            this.panel3.Controls.Add(this.kryptonHeader2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(1287, 191);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.panel3.Size = new System.Drawing.Size(635, 334);
            this.panel3.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.panel3.TabIndex = 47;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.67606F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.85915F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.46479F));
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.btnAddSample, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.cmbSampleLength, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.txtSampleStartPosition, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.chkLoopSample, 1, 6);
            this.tableLayoutPanel3.Controls.Add(this.btnSampleUpdate, 2, 4);
            this.tableLayoutPanel3.Controls.Add(this.lblSampleBPM, 2, 6);
            this.tableLayoutPanel3.Controls.Add(this.btnZoomSample, 2, 5);
            this.tableLayoutPanel3.Controls.Add(this.lstSamples, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnRemoveSample, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnRenameSample, 2, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(1, 26);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tableLayoutPanel3.RowCount = 7;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66556F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66556F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66889F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66889F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66555F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66555F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(633, 307);
            this.tableLayoutPanel3.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(11, 251);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label5.Size = new System.Drawing.Size(135, 29);
            this.label5.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label5.TabIndex = 54;
            this.label5.Text = "Looped:";
            // 
            // btnAddSample
            // 
            this.btnAddSample.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddSample.Location = new System.Drawing.Point(459, 6);
            this.btnAddSample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnAddSample.Name = "btnAddSample";
            this.btnAddSample.Size = new System.Drawing.Size(163, 29);
            this.btnAddSample.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnAddSample.TabIndex = 50;
            this.btnAddSample.Text = "Add Sample";
            this.btnAddSample.Click += new System.EventHandler(this.btnAddSample_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(11, 157);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label3.Size = new System.Drawing.Size(135, 29);
            this.label3.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label3.TabIndex = 52;
            this.label3.Text = "Start:";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(11, 204);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label4.Size = new System.Drawing.Size(135, 29);
            this.label4.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label4.TabIndex = 53;
            this.label4.Text = "Length:";
            // 
            // cmbSampleLength
            // 
            this.cmbSampleLength.DropDownWidth = 79;
            this.cmbSampleLength.Location = new System.Drawing.Point(244, 208);
            this.cmbSampleLength.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbSampleLength.Name = "cmbSampleLength";
            this.cmbSampleLength.Seconds = 0D;
            this.cmbSampleLength.Size = new System.Drawing.Size(124, 25);
            this.cmbSampleLength.TabIndex = 55;
            this.cmbSampleLength.Text = "00:00:0000";
            // 
            // txtSampleStartPosition
            // 
            this.txtSampleStartPosition.Location = new System.Drawing.Point(244, 161);
            this.txtSampleStartPosition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSampleStartPosition.Name = "txtSampleStartPosition";
            this.txtSampleStartPosition.Seconds = 0D;
            this.txtSampleStartPosition.Size = new System.Drawing.Size(124, 27);
            this.txtSampleStartPosition.TabIndex = 56;
            this.txtSampleStartPosition.Text = "00:00.0000";
            // 
            // chkLoopSample
            // 
            this.chkLoopSample.CheckedValue = "0.5";
            this.chkLoopSample.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            this.chkLoopSample.Location = new System.Drawing.Point(244, 255);
            this.chkLoopSample.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkLoopSample.Name = "chkLoopSample";
            this.chkLoopSample.Size = new System.Drawing.Size(76, 24);
            this.chkLoopSample.TabIndex = 57;
            this.chkLoopSample.UncheckedValue = "1";
            this.chkLoopSample.Value = "1";
            this.chkLoopSample.Values.Text = "Looped";
            // 
            // btnSampleUpdate
            // 
            this.btnSampleUpdate.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSampleUpdate.Location = new System.Drawing.Point(459, 157);
            this.btnSampleUpdate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnSampleUpdate.Name = "btnSampleUpdate";
            this.btnSampleUpdate.Size = new System.Drawing.Size(163, 29);
            this.btnSampleUpdate.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnSampleUpdate.TabIndex = 58;
            this.btnSampleUpdate.Text = "Update";
            this.btnSampleUpdate.Click += new System.EventHandler(this.btnSampleUpdate_Click);
            // 
            // lblSampleBPM
            // 
            this.lblSampleBPM.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSampleBPM.ForeColor = System.Drawing.Color.White;
            this.lblSampleBPM.Location = new System.Drawing.Point(459, 251);
            this.lblSampleBPM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSampleBPM.Name = "lblSampleBPM";
            this.lblSampleBPM.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblSampleBPM.Size = new System.Drawing.Size(97, 29);
            this.lblSampleBPM.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblSampleBPM.TabIndex = 59;
            this.lblSampleBPM.Text = "100BPM";
            // 
            // btnZoomSample
            // 
            this.btnZoomSample.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnZoomSample.Location = new System.Drawing.Point(459, 204);
            this.btnZoomSample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnZoomSample.Name = "btnZoomSample";
            this.btnZoomSample.Size = new System.Drawing.Size(163, 29);
            this.btnZoomSample.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnZoomSample.TabIndex = 60;
            this.btnZoomSample.Text = "Zoom";
            this.btnZoomSample.Click += new System.EventHandler(this.btnZoomSample_Click);
            // 
            // lstSamples
            // 
            this.lstSamples.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSample});
            this.tableLayoutPanel3.SetColumnSpan(this.lstSamples, 2);
            this.lstSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSamples.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSamples.FullRowSelect = true;
            this.lstSamples.GridLines = true;
            this.lstSamples.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstSamples.HideSelection = false;
            this.lstSamples.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8});
            this.lstSamples.Location = new System.Drawing.Point(11, 10);
            this.lstSamples.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstSamples.MultiSelect = false;
            this.lstSamples.Name = "lstSamples";
            this.tableLayoutPanel3.SetRowSpan(this.lstSamples, 3);
            this.lstSamples.Size = new System.Drawing.Size(440, 133);
            this.lstSamples.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstSamples.TabIndex = 61;
            this.lstSamples.UseCompatibleStateImageBehavior = false;
            this.lstSamples.View = System.Windows.Forms.View.Details;
            this.lstSamples.SelectedIndexChanged += new System.EventHandler(this.lstSamples_SelectedIndexChanged);
            // 
            // colSample
            // 
            this.colSample.Text = "Samples";
            this.colSample.Width = 252;
            // 
            // btnRemoveSample
            // 
            this.btnRemoveSample.Location = new System.Drawing.Point(459, 53);
            this.btnRemoveSample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnRemoveSample.Name = "btnRemoveSample";
            this.btnRemoveSample.Size = new System.Drawing.Size(133, 29);
            this.btnRemoveSample.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnRemoveSample.TabIndex = 51;
            this.btnRemoveSample.Text = "Remove Sample";
            this.btnRemoveSample.Click += new System.EventHandler(this.btnRemoveSample_Click);
            // 
            // btnRenameSample
            // 
            this.btnRenameSample.Location = new System.Drawing.Point(459, 100);
            this.btnRenameSample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnRenameSample.Name = "btnRenameSample";
            this.btnRenameSample.Size = new System.Drawing.Size(133, 29);
            this.btnRenameSample.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnRenameSample.TabIndex = 62;
            this.btnRenameSample.Text = "Rename Sample";
            this.btnRenameSample.Click += new System.EventHandler(this.btnRenameSample_Click);
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader2.Location = new System.Drawing.Point(1, 1);
            this.kryptonHeader2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.Size = new System.Drawing.Size(633, 25);
            this.kryptonHeader2.StateCommon.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            this.kryptonHeader2.TabIndex = 0;
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Samples";
            this.kryptonHeader2.Values.Image = null;
            // 
            // pnlFadeOut
            // 
            this.pnlFadeOut.BackColor = System.Drawing.SystemColors.Control;
            this.pnlFadeOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFadeOut.Controls.Add(this.tblFadeOut);
            this.pnlFadeOut.Controls.Add(this.hdrFadeOut);
            this.pnlFadeOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFadeOut.Location = new System.Drawing.Point(645, 191);
            this.pnlFadeOut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlFadeOut.Name = "pnlFadeOut";
            this.pnlFadeOut.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.pnlFadeOut.Size = new System.Drawing.Size(634, 334);
            this.pnlFadeOut.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlFadeOut.TabIndex = 44;
            // 
            // tblFadeOut
            // 
            this.tblFadeOut.ColumnCount = 3;
            this.tblFadeOut.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.21127F));
            this.tblFadeOut.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.30127F));
            this.tblFadeOut.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.62093F));
            this.tblFadeOut.Controls.Add(this.btnCalcEndBPM, 2, 3);
            this.tblFadeOut.Controls.Add(this.label19, 0, 6);
            this.tblFadeOut.Controls.Add(this.cmbFadeOutLoopCount, 1, 2);
            this.tblFadeOut.Controls.Add(this.lblFadeOutLoopCount, 0, 2);
            this.tblFadeOut.Controls.Add(this.lblFadeOutStartPosition, 0, 0);
            this.tblFadeOut.Controls.Add(this.lblFadeOutLength, 0, 1);
            this.tblFadeOut.Controls.Add(this.txtFadeOutStartPosition, 1, 0);
            this.tblFadeOut.Controls.Add(this.lblPowerDown, 0, 3);
            this.tblFadeOut.Controls.Add(this.chkPowerDown, 1, 3);
            this.tblFadeOut.Controls.Add(this.btnFadeOutUpdate, 2, 0);
            this.tblFadeOut.Controls.Add(this.btnZoomFadeOut, 2, 1);
            this.tblFadeOut.Controls.Add(this.label15, 0, 4);
            this.tblFadeOut.Controls.Add(this.chkUseSkipSection, 1, 4);
            this.tblFadeOut.Controls.Add(this.label18, 0, 5);
            this.tblFadeOut.Controls.Add(this.txtSkipStart, 1, 5);
            this.tblFadeOut.Controls.Add(this.btnSkipUpdate, 2, 5);
            this.tblFadeOut.Controls.Add(this.cmbSkipLength, 1, 6);
            this.tblFadeOut.Controls.Add(this.btnSkipZoom, 2, 6);
            this.tblFadeOut.Controls.Add(this.panel5, 1, 1);
            this.tblFadeOut.Controls.Add(this.lblEndBPM, 2, 2);
            this.tblFadeOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblFadeOut.Location = new System.Drawing.Point(1, 26);
            this.tblFadeOut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tblFadeOut.Name = "tblFadeOut";
            this.tblFadeOut.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tblFadeOut.RowCount = 7;
            this.tblFadeOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblFadeOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblFadeOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblFadeOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblFadeOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblFadeOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblFadeOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tblFadeOut.Size = new System.Drawing.Size(632, 307);
            this.tblFadeOut.TabIndex = 11;
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(11, 264);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label19.Size = new System.Drawing.Size(117, 29);
            this.label19.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label19.TabIndex = 58;
            this.label19.Text = "  Length:";
            // 
            // cmbFadeOutLoopCount
            // 
            this.cmbFadeOutLoopCount.DropDownWidth = 79;
            this.cmbFadeOutLoopCount.EntryType = Halloumi.Common.Windows.Controls.ComboBox.TextEntryType.Integer;
            this.cmbFadeOutLoopCount.ErrorProvider = null;
            this.cmbFadeOutLoopCount.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "10",
            "12",
            "14",
            "16",
            "32",
            "64"});
            this.cmbFadeOutLoopCount.Location = new System.Drawing.Point(228, 96);
            this.cmbFadeOutLoopCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbFadeOutLoopCount.MaximumValue = 0;
            this.cmbFadeOutLoopCount.MaxLength = 3;
            this.cmbFadeOutLoopCount.MinimumValue = 100;
            this.cmbFadeOutLoopCount.Name = "cmbFadeOutLoopCount";
            this.cmbFadeOutLoopCount.Size = new System.Drawing.Size(133, 25);
            this.cmbFadeOutLoopCount.StateCommon.ComboBox.Back.Color1 = System.Drawing.Color.LemonChiffon;
            this.cmbFadeOutLoopCount.TabIndex = 44;
            this.cmbFadeOutLoopCount.Text = "0";
            this.cmbFadeOutLoopCount.SelectedIndexChanged += new System.EventHandler(this.cmbFadeOutLoopCount_SelectedIndexChanged);
            // 
            // lblFadeOutLoopCount
            // 
            this.lblFadeOutLoopCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFadeOutLoopCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFadeOutLoopCount.ForeColor = System.Drawing.Color.White;
            this.lblFadeOutLoopCount.Location = new System.Drawing.Point(11, 92);
            this.lblFadeOutLoopCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFadeOutLoopCount.Name = "lblFadeOutLoopCount";
            this.lblFadeOutLoopCount.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblFadeOutLoopCount.Size = new System.Drawing.Size(209, 29);
            this.lblFadeOutLoopCount.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblFadeOutLoopCount.TabIndex = 43;
            this.lblFadeOutLoopCount.Text = "Loop Count:";
            // 
            // lblFadeOutStartPosition
            // 
            this.lblFadeOutStartPosition.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFadeOutStartPosition.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFadeOutStartPosition.ForeColor = System.Drawing.Color.White;
            this.lblFadeOutStartPosition.Location = new System.Drawing.Point(11, 6);
            this.lblFadeOutStartPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFadeOutStartPosition.Name = "lblFadeOutStartPosition";
            this.lblFadeOutStartPosition.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblFadeOutStartPosition.Size = new System.Drawing.Size(209, 29);
            this.lblFadeOutStartPosition.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblFadeOutStartPosition.TabIndex = 0;
            this.lblFadeOutStartPosition.Text = "Fade-Out Start:";
            // 
            // lblFadeOutLength
            // 
            this.lblFadeOutLength.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFadeOutLength.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFadeOutLength.ForeColor = System.Drawing.Color.White;
            this.lblFadeOutLength.Location = new System.Drawing.Point(11, 49);
            this.lblFadeOutLength.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFadeOutLength.Name = "lblFadeOutLength";
            this.lblFadeOutLength.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblFadeOutLength.Size = new System.Drawing.Size(209, 29);
            this.lblFadeOutLength.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblFadeOutLength.TabIndex = 21;
            this.lblFadeOutLength.Text = "Fade Length:";
            // 
            // txtFadeOutStartPosition
            // 
            this.txtFadeOutStartPosition.Location = new System.Drawing.Point(228, 10);
            this.txtFadeOutStartPosition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFadeOutStartPosition.Name = "txtFadeOutStartPosition";
            this.txtFadeOutStartPosition.Seconds = 0D;
            this.txtFadeOutStartPosition.Size = new System.Drawing.Size(133, 27);
            this.txtFadeOutStartPosition.TabIndex = 0;
            this.txtFadeOutStartPosition.Text = "00:00.0000";
            // 
            // lblPowerDown
            // 
            this.lblPowerDown.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPowerDown.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPowerDown.ForeColor = System.Drawing.Color.White;
            this.lblPowerDown.Location = new System.Drawing.Point(11, 135);
            this.lblPowerDown.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPowerDown.Name = "lblPowerDown";
            this.lblPowerDown.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblPowerDown.Size = new System.Drawing.Size(209, 29);
            this.lblPowerDown.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblPowerDown.TabIndex = 45;
            this.lblPowerDown.Text = "Power Down:";
            // 
            // chkPowerDown
            // 
            this.chkPowerDown.CheckedValue = "0.5";
            this.chkPowerDown.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            this.chkPowerDown.Location = new System.Drawing.Point(228, 139);
            this.chkPowerDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkPowerDown.Name = "chkPowerDown";
            this.chkPowerDown.Size = new System.Drawing.Size(109, 24);
            this.chkPowerDown.TabIndex = 46;
            this.chkPowerDown.UncheckedValue = "1";
            this.chkPowerDown.Value = "1";
            this.chkPowerDown.Values.Text = "Power down";
            // 
            // btnFadeOutUpdate
            // 
            this.btnFadeOutUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFadeOutUpdate.Location = new System.Drawing.Point(482, 6);
            this.btnFadeOutUpdate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnFadeOutUpdate.Name = "btnFadeOutUpdate";
            this.btnFadeOutUpdate.Size = new System.Drawing.Size(139, 39);
            this.btnFadeOutUpdate.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnFadeOutUpdate.TabIndex = 50;
            this.btnFadeOutUpdate.Text = "Update";
            this.btnFadeOutUpdate.Click += new System.EventHandler(this.btnFadeOutUpdate_Click);
            // 
            // lblEndBPM
            // 
            this.lblEndBPM.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndBPM.ForeColor = System.Drawing.Color.White;
            this.lblEndBPM.Location = new System.Drawing.Point(482, 92);
            this.lblEndBPM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEndBPM.Name = "lblEndBPM";
            this.lblEndBPM.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblEndBPM.Size = new System.Drawing.Size(97, 29);
            this.lblEndBPM.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblEndBPM.TabIndex = 49;
            this.lblEndBPM.Text = "100BPM";
            // 
            // btnZoomFadeOut
            // 
            this.btnZoomFadeOut.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnZoomFadeOut.Location = new System.Drawing.Point(482, 49);
            this.btnZoomFadeOut.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnZoomFadeOut.Name = "btnZoomFadeOut";
            this.btnZoomFadeOut.Size = new System.Drawing.Size(139, 37);
            this.btnZoomFadeOut.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnZoomFadeOut.TabIndex = 52;
            this.btnZoomFadeOut.Text = "Zoom";
            this.btnZoomFadeOut.Click += new System.EventHandler(this.btnZoomFadeOut_Click);
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(11, 178);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label15.Size = new System.Drawing.Size(117, 29);
            this.label15.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label15.TabIndex = 53;
            this.label15.Text = "Skip Section:";
            // 
            // chkUseSkipSection
            // 
            this.chkUseSkipSection.CheckedValue = "0.5";
            this.tblFadeOut.SetColumnSpan(this.chkUseSkipSection, 2);
            this.chkUseSkipSection.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            this.chkUseSkipSection.Location = new System.Drawing.Point(228, 182);
            this.chkUseSkipSection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkUseSkipSection.Name = "chkUseSkipSection";
            this.chkUseSkipSection.Size = new System.Drawing.Size(134, 24);
            this.chkUseSkipSection.TabIndex = 54;
            this.chkUseSkipSection.UncheckedValue = "1";
            this.chkUseSkipSection.Value = "1";
            this.chkUseSkipSection.Values.Text = "Use skip section";
            this.chkUseSkipSection.CheckedChanged += new System.EventHandler(this.chkUseSkipSection_CheckedChanged);
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(11, 221);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label18.Size = new System.Drawing.Size(117, 29);
            this.label18.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label18.TabIndex = 55;
            this.label18.Text = "  Start:";
            // 
            // txtSkipStart
            // 
            this.txtSkipStart.Location = new System.Drawing.Point(228, 225);
            this.txtSkipStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSkipStart.Name = "txtSkipStart";
            this.txtSkipStart.Seconds = 0D;
            this.txtSkipStart.Size = new System.Drawing.Size(133, 27);
            this.txtSkipStart.TabIndex = 56;
            this.txtSkipStart.Text = "00:00.0000";
            this.txtSkipStart.TextChanged += new System.EventHandler(this.txtSkipStart_TextChanged);
            // 
            // btnSkipUpdate
            // 
            this.btnSkipUpdate.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSkipUpdate.Location = new System.Drawing.Point(482, 221);
            this.btnSkipUpdate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnSkipUpdate.Name = "btnSkipUpdate";
            this.btnSkipUpdate.Size = new System.Drawing.Size(139, 26);
            this.btnSkipUpdate.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnSkipUpdate.TabIndex = 57;
            this.btnSkipUpdate.Text = "Update";
            this.btnSkipUpdate.Click += new System.EventHandler(this.btnSkipUpdate_Click);
            // 
            // cmbSkipLength
            // 
            this.cmbSkipLength.DropDownWidth = 79;
            this.cmbSkipLength.Location = new System.Drawing.Point(228, 268);
            this.cmbSkipLength.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbSkipLength.Name = "cmbSkipLength";
            this.cmbSkipLength.Seconds = 0D;
            this.cmbSkipLength.Size = new System.Drawing.Size(133, 25);
            this.cmbSkipLength.TabIndex = 59;
            this.cmbSkipLength.Text = "00:00.0000";
            this.cmbSkipLength.SelectedIndexChanged += new System.EventHandler(this.cmbSkipLength_SelectedIndexChanged);
            this.cmbSkipLength.TextChanged += new System.EventHandler(this.cmbSkipLength_TextChanged);
            // 
            // btnSkipZoom
            // 
            this.btnSkipZoom.AutoScroll = true;
            this.btnSkipZoom.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSkipZoom.Location = new System.Drawing.Point(482, 264);
            this.btnSkipZoom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnSkipZoom.Name = "btnSkipZoom";
            this.btnSkipZoom.Size = new System.Drawing.Size(139, 31);
            this.btnSkipZoom.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnSkipZoom.TabIndex = 60;
            this.btnSkipZoom.Text = "Zoom";
            this.btnSkipZoom.Click += new System.EventHandler(this.btnSkipZoom_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnCopyRight);
            this.panel5.Controls.Add(this.cmbCustomFadeOutLength);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(224, 49);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(254, 43);
            this.panel5.TabIndex = 61;
            // 
            // btnCopyRight
            // 
            this.btnCopyRight.Location = new System.Drawing.Point(145, 3);
            this.btnCopyRight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnCopyRight.Name = "btnCopyRight";
            this.btnCopyRight.Size = new System.Drawing.Size(48, 33);
            this.btnCopyRight.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnCopyRight.TabIndex = 54;
            this.btnCopyRight.Text = "<";
            this.btnCopyRight.Click += new System.EventHandler(this.btnCopyRight_Click);
            // 
            // cmbCustomFadeOutLength
            // 
            this.cmbCustomFadeOutLength.DropDownWidth = 79;
            this.cmbCustomFadeOutLength.Location = new System.Drawing.Point(4, 0);
            this.cmbCustomFadeOutLength.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbCustomFadeOutLength.Name = "cmbCustomFadeOutLength";
            this.cmbCustomFadeOutLength.Seconds = 0D;
            this.cmbCustomFadeOutLength.Size = new System.Drawing.Size(133, 25);
            this.cmbCustomFadeOutLength.TabIndex = 1;
            this.cmbCustomFadeOutLength.Text = "00:00:0000";
            this.cmbCustomFadeOutLength.SelectedIndexChanged += new System.EventHandler(this.cmbCustomFadeOutLength_SelectedIndexChanged);
            this.cmbCustomFadeOutLength.TextChanged += new System.EventHandler(this.cmbCustomFadeOutLength_TextChanged);
            this.cmbCustomFadeOutLength.Leave += new System.EventHandler(this.cmbCustomFadeOutLength_Leave);
            // 
            // hdrFadeOut
            // 
            this.hdrFadeOut.Dock = System.Windows.Forms.DockStyle.Top;
            this.hdrFadeOut.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.hdrFadeOut.Location = new System.Drawing.Point(1, 1);
            this.hdrFadeOut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.hdrFadeOut.Name = "hdrFadeOut";
            this.hdrFadeOut.Size = new System.Drawing.Size(632, 25);
            this.hdrFadeOut.StateCommon.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            this.hdrFadeOut.TabIndex = 0;
            this.hdrFadeOut.Values.Description = "";
            this.hdrFadeOut.Values.Heading = "Fade Out";
            this.hdrFadeOut.Values.Image = null;
            // 
            // pnlFadeIn
            // 
            this.pnlFadeIn.BackColor = System.Drawing.SystemColors.Control;
            this.pnlFadeIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFadeIn.Controls.Add(this.tblFadeIn);
            this.pnlFadeIn.Controls.Add(this.hdrFadeIn);
            this.pnlFadeIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFadeIn.Location = new System.Drawing.Point(4, 191);
            this.pnlFadeIn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlFadeIn.Name = "pnlFadeIn";
            this.pnlFadeIn.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.pnlFadeIn.Size = new System.Drawing.Size(633, 334);
            this.pnlFadeIn.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlFadeIn.TabIndex = 1;
            // 
            // tblFadeIn
            // 
            this.tblFadeIn.ColumnCount = 3;
            this.tblFadeIn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblFadeIn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.28147F));
            this.tblFadeIn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.47949F));
            this.tblFadeIn.Controls.Add(this.btnCalcStartBPM, 2, 3);
            this.tblFadeIn.Controls.Add(this.btnPreFadeInUpdate, 2, 4);
            this.tblFadeIn.Controls.Add(this.lblFadeInLoopCount, 0, 2);
            this.tblFadeIn.Controls.Add(this.lblPreFadeIn, 0, 3);
            this.tblFadeIn.Controls.Add(this.lblFadeInPosition, 0, 0);
            this.tblFadeIn.Controls.Add(this.txtFadeInPosition, 1, 0);
            this.tblFadeIn.Controls.Add(this.txtPreFadeInStartPosition, 1, 4);
            this.tblFadeIn.Controls.Add(this.lblPreFadeInStartPosition, 0, 4);
            this.tblFadeIn.Controls.Add(this.lblPreFadeInStartVolume, 0, 5);
            this.tblFadeIn.Controls.Add(this.lblFadeInLength, 0, 1);
            this.tblFadeIn.Controls.Add(this.chkUsePreFadeIn, 1, 3);
            this.tblFadeIn.Controls.Add(this.cmbFadeInLoopCount, 1, 2);
            this.tblFadeIn.Controls.Add(this.cmbPreFadeInStartVolume, 1, 5);
            this.tblFadeIn.Controls.Add(this.btnFadeInUpdate, 2, 0);
            this.tblFadeIn.Controls.Add(this.lblStartBPM, 2, 2);
            this.tblFadeIn.Controls.Add(this.btnZoomFadeIn, 2, 1);
            this.tblFadeIn.Controls.Add(this.btnZoomPreFade, 2, 5);
            this.tblFadeIn.Controls.Add(this.panel2, 1, 1);
            this.tblFadeIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblFadeIn.Location = new System.Drawing.Point(1, 26);
            this.tblFadeIn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tblFadeIn.Name = "tblFadeIn";
            this.tblFadeIn.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tblFadeIn.RowCount = 6;
            this.tblFadeIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblFadeIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblFadeIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblFadeIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblFadeIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblFadeIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblFadeIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tblFadeIn.Size = new System.Drawing.Size(631, 307);
            this.tblFadeIn.TabIndex = 11;
            // 
            // btnPreFadeInUpdate
            // 
            this.btnPreFadeInUpdate.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPreFadeInUpdate.Location = new System.Drawing.Point(482, 202);
            this.btnPreFadeInUpdate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnPreFadeInUpdate.Name = "btnPreFadeInUpdate";
            this.btnPreFadeInUpdate.Size = new System.Drawing.Size(138, 32);
            this.btnPreFadeInUpdate.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnPreFadeInUpdate.TabIndex = 54;
            this.btnPreFadeInUpdate.Text = "Update";
            this.btnPreFadeInUpdate.Click += new System.EventHandler(this.btnPreFadeInUpdate_Click);
            // 
            // lblFadeInLoopCount
            // 
            this.lblFadeInLoopCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFadeInLoopCount.ForeColor = System.Drawing.Color.White;
            this.lblFadeInLoopCount.Location = new System.Drawing.Point(11, 104);
            this.lblFadeInLoopCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFadeInLoopCount.Name = "lblFadeInLoopCount";
            this.lblFadeInLoopCount.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblFadeInLoopCount.Size = new System.Drawing.Size(117, 29);
            this.lblFadeInLoopCount.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblFadeInLoopCount.TabIndex = 43;
            this.lblFadeInLoopCount.Text = "Loop Count:";
            // 
            // lblPreFadeIn
            // 
            this.lblPreFadeIn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreFadeIn.ForeColor = System.Drawing.Color.White;
            this.lblPreFadeIn.Location = new System.Drawing.Point(11, 153);
            this.lblPreFadeIn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPreFadeIn.Name = "lblPreFadeIn";
            this.lblPreFadeIn.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblPreFadeIn.Size = new System.Drawing.Size(117, 29);
            this.lblPreFadeIn.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblPreFadeIn.TabIndex = 42;
            this.lblPreFadeIn.Text = "Pre-Fade-In:";
            // 
            // lblFadeInPosition
            // 
            this.lblFadeInPosition.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFadeInPosition.ForeColor = System.Drawing.Color.White;
            this.lblFadeInPosition.Location = new System.Drawing.Point(11, 6);
            this.lblFadeInPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFadeInPosition.Name = "lblFadeInPosition";
            this.lblFadeInPosition.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblFadeInPosition.Size = new System.Drawing.Size(117, 29);
            this.lblFadeInPosition.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblFadeInPosition.TabIndex = 16;
            this.lblFadeInPosition.Text = "Fade-In Start:";
            // 
            // txtFadeInPosition
            // 
            this.txtFadeInPosition.Location = new System.Drawing.Point(216, 10);
            this.txtFadeInPosition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFadeInPosition.Name = "txtFadeInPosition";
            this.txtFadeInPosition.Seconds = 0D;
            this.txtFadeInPosition.Size = new System.Drawing.Size(145, 27);
            this.txtFadeInPosition.TabIndex = 0;
            this.txtFadeInPosition.Text = "00:00.0000";
            // 
            // txtPreFadeInStartPosition
            // 
            this.txtPreFadeInStartPosition.Location = new System.Drawing.Point(216, 206);
            this.txtPreFadeInStartPosition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPreFadeInStartPosition.Name = "txtPreFadeInStartPosition";
            this.txtPreFadeInStartPosition.Seconds = 0D;
            this.txtPreFadeInStartPosition.Size = new System.Drawing.Size(145, 27);
            this.txtPreFadeInStartPosition.TabIndex = 6;
            this.txtPreFadeInStartPosition.Text = "00:00.0000";
            // 
            // lblPreFadeInStartPosition
            // 
            this.lblPreFadeInStartPosition.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreFadeInStartPosition.ForeColor = System.Drawing.Color.White;
            this.lblPreFadeInStartPosition.Location = new System.Drawing.Point(11, 202);
            this.lblPreFadeInStartPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPreFadeInStartPosition.Name = "lblPreFadeInStartPosition";
            this.lblPreFadeInStartPosition.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblPreFadeInStartPosition.Size = new System.Drawing.Size(117, 29);
            this.lblPreFadeInStartPosition.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblPreFadeInStartPosition.TabIndex = 39;
            this.lblPreFadeInStartPosition.Text = "  Start:";
            // 
            // lblPreFadeInStartVolume
            // 
            this.lblPreFadeInStartVolume.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreFadeInStartVolume.ForeColor = System.Drawing.Color.White;
            this.lblPreFadeInStartVolume.Location = new System.Drawing.Point(11, 251);
            this.lblPreFadeInStartVolume.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPreFadeInStartVolume.Name = "lblPreFadeInStartVolume";
            this.lblPreFadeInStartVolume.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblPreFadeInStartVolume.Size = new System.Drawing.Size(117, 32);
            this.lblPreFadeInStartVolume.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblPreFadeInStartVolume.TabIndex = 37;
            this.lblPreFadeInStartVolume.Text = "  Vol:";
            // 
            // lblFadeInLength
            // 
            this.lblFadeInLength.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFadeInLength.ForeColor = System.Drawing.Color.White;
            this.lblFadeInLength.Location = new System.Drawing.Point(11, 55);
            this.lblFadeInLength.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFadeInLength.Name = "lblFadeInLength";
            this.lblFadeInLength.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblFadeInLength.Size = new System.Drawing.Size(117, 29);
            this.lblFadeInLength.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblFadeInLength.TabIndex = 21;
            this.lblFadeInLength.Text = "Fade Length:";
            // 
            // chkUsePreFadeIn
            // 
            this.chkUsePreFadeIn.CheckedValue = "0.5";
            this.chkUsePreFadeIn.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            this.chkUsePreFadeIn.Location = new System.Drawing.Point(216, 157);
            this.chkUsePreFadeIn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkUsePreFadeIn.Name = "chkUsePreFadeIn";
            this.chkUsePreFadeIn.Size = new System.Drawing.Size(131, 24);
            this.chkUsePreFadeIn.TabIndex = 5;
            this.chkUsePreFadeIn.UncheckedValue = "1";
            this.chkUsePreFadeIn.Value = "1";
            this.chkUsePreFadeIn.Values.Text = "Use pre-fade-in";
            this.chkUsePreFadeIn.CheckedChanged += new System.EventHandler(this.chkUsePreFadeIn_CheckedChanged);
            // 
            // cmbFadeInLoopCount
            // 
            this.cmbFadeInLoopCount.DropDownWidth = 79;
            this.cmbFadeInLoopCount.EntryType = Halloumi.Common.Windows.Controls.ComboBox.TextEntryType.Integer;
            this.cmbFadeInLoopCount.ErrorProvider = null;
            this.cmbFadeInLoopCount.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "10",
            "12",
            "14",
            "16",
            "32",
            "64"});
            this.cmbFadeInLoopCount.Location = new System.Drawing.Point(216, 108);
            this.cmbFadeInLoopCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbFadeInLoopCount.MaximumValue = 0;
            this.cmbFadeInLoopCount.MaxLength = 3;
            this.cmbFadeInLoopCount.MinimumValue = 100;
            this.cmbFadeInLoopCount.Name = "cmbFadeInLoopCount";
            this.cmbFadeInLoopCount.Size = new System.Drawing.Size(145, 25);
            this.cmbFadeInLoopCount.StateCommon.ComboBox.Back.Color1 = System.Drawing.Color.LemonChiffon;
            this.cmbFadeInLoopCount.TabIndex = 44;
            this.cmbFadeInLoopCount.Text = "0";
            this.cmbFadeInLoopCount.SelectedIndexChanged += new System.EventHandler(this.cmbFadeInLoopCount_SelectedIndexChanged);
            // 
            // cmbPreFadeInStartVolume
            // 
            this.cmbPreFadeInStartVolume.DropDownWidth = 79;
            this.cmbPreFadeInStartVolume.EntryType = Halloumi.Common.Windows.Controls.ComboBox.TextEntryType.Integer;
            this.cmbPreFadeInStartVolume.ErrorProvider = null;
            this.cmbPreFadeInStartVolume.Location = new System.Drawing.Point(216, 255);
            this.cmbPreFadeInStartVolume.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbPreFadeInStartVolume.MaximumValue = 0;
            this.cmbPreFadeInStartVolume.MaxLength = 3;
            this.cmbPreFadeInStartVolume.MinimumValue = 100;
            this.cmbPreFadeInStartVolume.Name = "cmbPreFadeInStartVolume";
            this.cmbPreFadeInStartVolume.Size = new System.Drawing.Size(145, 25);
            this.cmbPreFadeInStartVolume.TabIndex = 7;
            // 
            // btnFadeInUpdate
            // 
            this.btnFadeInUpdate.Location = new System.Drawing.Point(482, 6);
            this.btnFadeInUpdate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnFadeInUpdate.Name = "btnFadeInUpdate";
            this.btnFadeInUpdate.Size = new System.Drawing.Size(135, 36);
            this.btnFadeInUpdate.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnFadeInUpdate.TabIndex = 46;
            this.btnFadeInUpdate.Text = "Update";
            this.btnFadeInUpdate.Click += new System.EventHandler(this.btnFadeInUpdate_Click);
            // 
            // lblStartBPM
            // 
            this.lblStartBPM.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartBPM.ForeColor = System.Drawing.Color.White;
            this.lblStartBPM.Location = new System.Drawing.Point(482, 104);
            this.lblStartBPM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStartBPM.Name = "lblStartBPM";
            this.lblStartBPM.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblStartBPM.Size = new System.Drawing.Size(93, 29);
            this.lblStartBPM.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblStartBPM.TabIndex = 45;
            this.lblStartBPM.Text = "100BPM";
            // 
            // btnZoomFadeIn
            // 
            this.btnZoomFadeIn.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnZoomFadeIn.Location = new System.Drawing.Point(482, 55);
            this.btnZoomFadeIn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnZoomFadeIn.Name = "btnZoomFadeIn";
            this.btnZoomFadeIn.Size = new System.Drawing.Size(138, 33);
            this.btnZoomFadeIn.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnZoomFadeIn.TabIndex = 52;
            this.btnZoomFadeIn.Text = "Zoom";
            this.btnZoomFadeIn.Click += new System.EventHandler(this.btnZoomFadeIn_Click);
            // 
            // btnZoomPreFade
            // 
            this.btnZoomPreFade.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnZoomPreFade.Location = new System.Drawing.Point(482, 251);
            this.btnZoomPreFade.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnZoomPreFade.Name = "btnZoomPreFade";
            this.btnZoomPreFade.Size = new System.Drawing.Size(138, 35);
            this.btnZoomPreFade.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnZoomPreFade.TabIndex = 53;
            this.btnZoomPreFade.Text = "Zoom";
            this.btnZoomPreFade.Click += new System.EventHandler(this.btnZoomPreFade_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCopyLeft);
            this.panel2.Controls.Add(this.cmbCustomFadeInLength);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(212, 55);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(266, 49);
            this.panel2.TabIndex = 55;
            // 
            // btnCopyLeft
            // 
            this.btnCopyLeft.Location = new System.Drawing.Point(153, 4);
            this.btnCopyLeft.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnCopyLeft.Name = "btnCopyLeft";
            this.btnCopyLeft.Size = new System.Drawing.Size(48, 33);
            this.btnCopyLeft.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnCopyLeft.TabIndex = 53;
            this.btnCopyLeft.Text = ">";
            this.btnCopyLeft.Click += new System.EventHandler(this.btnCopyLeft_Click);
            // 
            // cmbCustomFadeInLength
            // 
            this.cmbCustomFadeInLength.DropDownWidth = 79;
            this.cmbCustomFadeInLength.Location = new System.Drawing.Point(4, 4);
            this.cmbCustomFadeInLength.Margin = new System.Windows.Forms.Padding(0);
            this.cmbCustomFadeInLength.Name = "cmbCustomFadeInLength";
            this.cmbCustomFadeInLength.Seconds = 0D;
            this.cmbCustomFadeInLength.Size = new System.Drawing.Size(145, 25);
            this.cmbCustomFadeInLength.TabIndex = 1;
            this.cmbCustomFadeInLength.Text = "00:00.0000";
            this.cmbCustomFadeInLength.SelectedIndexChanged += new System.EventHandler(this.cmbCustomFadeInLength_SelectedIndexChanged);
            this.cmbCustomFadeInLength.TextChanged += new System.EventHandler(this.cmbCustomFadeInLength_TextChanged);
            this.cmbCustomFadeInLength.Leave += new System.EventHandler(this.cmbCustomFadeInLength_Leave);
            // 
            // hdrFadeIn
            // 
            this.hdrFadeIn.Dock = System.Windows.Forms.DockStyle.Top;
            this.hdrFadeIn.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.hdrFadeIn.Location = new System.Drawing.Point(1, 1);
            this.hdrFadeIn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.hdrFadeIn.Name = "hdrFadeIn";
            this.hdrFadeIn.Size = new System.Drawing.Size(631, 25);
            this.hdrFadeIn.StateCommon.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            this.hdrFadeIn.TabIndex = 0;
            this.hdrFadeIn.Values.Description = "";
            this.hdrFadeIn.Values.Heading = "Fade In";
            this.hdrFadeIn.Values.Image = null;
            // 
            // trackWave
            // 
            this.tblMain.SetColumnSpan(this.trackWave, 3);
            this.trackWave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackWave.Location = new System.Drawing.Point(5, 5);
            this.trackWave.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.trackWave.Name = "trackWave";
            this.trackWave.Size = new System.Drawing.Size(1916, 177);
            this.trackWave.TabIndex = 45;
            // 
            // linLine
            // 
            this.linLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.linLine.Location = new System.Drawing.Point(0, 632);
            this.linLine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.linLine.Name = "linLine";
            this.linLine.Size = new System.Drawing.Size(1940, 2);
            this.linLine.TabIndex = 0;
            // 
            // flpButtonsRight
            // 
            this.flpButtonsRight.BackColor = System.Drawing.Color.Transparent;
            this.flpButtonsRight.Controls.Add(this.btnCancel);
            this.flpButtonsRight.Controls.Add(this.btnOK);
            this.flpButtonsRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flpButtonsRight.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpButtonsRight.Location = new System.Drawing.Point(1508, 1);
            this.flpButtonsRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flpButtonsRight.Name = "flpButtonsRight";
            this.flpButtonsRight.Padding = new System.Windows.Forms.Padding(7, 6, 0, 6);
            this.flpButtonsRight.Size = new System.Drawing.Size(431, 51);
            this.flpButtonsRight.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(299, 11);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 31);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(169, 11);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(120, 31);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.flowLayoutPanel1);
            this.pnlButtons.Controls.Add(this.flpButtonsLeft);
            this.pnlButtons.Controls.Add(this.flpButtonsRight);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 634);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.pnlButtons.Size = new System.Drawing.Size(1940, 53);
            this.pnlButtons.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlButtons.TabIndex = 24;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(296, 1);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(7, 6, 0, 6);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(287, 51);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // flpButtonsLeft
            // 
            this.flpButtonsLeft.Controls.Add(this.label6);
            this.flpButtonsLeft.Controls.Add(this.cmbOutput);
            this.flpButtonsLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flpButtonsLeft.Location = new System.Drawing.Point(1, 1);
            this.flpButtonsLeft.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flpButtonsLeft.Name = "flpButtonsLeft";
            this.flpButtonsLeft.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.flpButtonsLeft.Size = new System.Drawing.Size(295, 51);
            this.flpButtonsLeft.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(17, 12);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.label6.Size = new System.Drawing.Size(71, 33);
            this.label6.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label6.TabIndex = 7;
            this.label6.Text = "Output:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbOutput
            // 
            this.cmbOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOutput.DropDownWidth = 72;
            this.cmbOutput.ErrorProvider = null;
            this.cmbOutput.Items.AddRange(new object[] {
            "Speakers",
            "Monitor",
            "Both"});
            this.cmbOutput.Location = new System.Drawing.Point(96, 16);
            this.cmbOutput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbOutput.Name = "cmbOutput";
            this.cmbOutput.Size = new System.Drawing.Size(96, 25);
            this.cmbOutput.TabIndex = 8;
            this.cmbOutput.SelectedIndexChanged += new System.EventHandler(this.cmbOutput_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.08772F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.19298F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.36842F));
            this.tableLayoutPanel1.Controls.Add(this.comboBox1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownWidth = 79;
            this.comboBox1.EntryType = Halloumi.Common.Windows.Controls.ComboBox.TextEntryType.Integer;
            this.comboBox1.ErrorProvider = null;
            this.comboBox1.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "10",
            "12",
            "14",
            "16",
            "32",
            "64"});
            this.comboBox1.Location = new System.Drawing.Point(73, 43);
            this.comboBox1.MaximumValue = 0;
            this.comboBox1.MaxLength = 3;
            this.comboBox1.MinimumValue = 100;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(68, 25);
            this.comboBox1.StateCommon.ComboBox.Back.Color1 = System.Drawing.Color.LemonChiffon;
            this.comboBox1.TabIndex = 44;
            this.comboBox1.Text = "0";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 40);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.label1.Size = new System.Drawing.Size(64, 24);
            this.label1.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label1.TabIndex = 43;
            this.label1.Text = "Loop Count:";
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 5);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.label2.Size = new System.Drawing.Size(94, 24);
            this.label2.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label2.TabIndex = 0;
            this.label2.Text = "Fade-Out Start:";
            // 
            // btnCalcStartBPM
            // 
            this.btnCalcStartBPM.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCalcStartBPM.Location = new System.Drawing.Point(482, 153);
            this.btnCalcStartBPM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnCalcStartBPM.Name = "btnCalcStartBPM";
            this.btnCalcStartBPM.Size = new System.Drawing.Size(138, 33);
            this.btnCalcStartBPM.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnCalcStartBPM.TabIndex = 56;
            this.btnCalcStartBPM.Text = "Calculate";
            this.btnCalcStartBPM.Click += new System.EventHandler(this.btnCalcStartBPM_Click);
            // 
            // btnCalcEndBPM
            // 
            this.btnCalcEndBPM.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCalcEndBPM.Location = new System.Drawing.Point(482, 135);
            this.btnCalcEndBPM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnCalcEndBPM.Name = "btnCalcEndBPM";
            this.btnCalcEndBPM.Size = new System.Drawing.Size(139, 33);
            this.btnCalcEndBPM.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnCalcEndBPM.TabIndex = 62;
            this.btnCalcEndBPM.Text = "Calculate";
            this.btnCalcEndBPM.Click += new System.EventHandler(this.btnCalcEndBPM_Click);
            // 
            // FrmShufflerDetails
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(32)))), ((int)(((byte)(48)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1940, 687);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.linLine);
            this.Controls.Add(this.pnlButtons);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmShufflerDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Halloumi : Shuffler : Track";
            this.UseApplicationIcon = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTrack_FormClosed);
            this.Load += new System.EventHandler(this.frmShufflerDetails_Load);
            this.pnlMain.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.flpTrackFX.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrackFX)).EndInit();
            this.flpRight.ResumeLayout(false);
            this.flpRight.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSampleLength)).EndInit();
            this.pnlFadeOut.ResumeLayout(false);
            this.pnlFadeOut.PerformLayout();
            this.tblFadeOut.ResumeLayout(false);
            this.tblFadeOut.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFadeOutLoopCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSkipLength)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomFadeOutLength)).EndInit();
            this.pnlFadeIn.ResumeLayout(false);
            this.pnlFadeIn.PerformLayout();
            this.tblFadeIn.ResumeLayout(false);
            this.tblFadeIn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFadeInLoopCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPreFadeInStartVolume)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomFadeInLength)).EndInit();
            this.flpButtonsRight.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.flpButtonsLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbOutput)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlMain;
        private Halloumi.Common.Windows.Controls.BeveledLine linLine;
        private System.Windows.Forms.FlowLayoutPanel flpButtonsRight;
        private Halloumi.Common.Windows.Controls.Button btnCancel;
        private Halloumi.Common.Windows.Controls.Button btnOK;
        private Halloumi.Common.Windows.Controls.Panel pnlButtons;
        private Halloumi.Common.Windows.Controls.SecondsTextBox txtFadeInPosition;
        private Halloumi.Common.Windows.Controls.Label lblFadeInPosition;
        private Halloumi.Common.Windows.Controls.Label lblFadeInLength;
        private Halloumi.Common.Windows.Controls.SecondsTextBox txtFadeOutStartPosition;
        private Halloumi.Common.Windows.Controls.ComboBox cmbPreFadeInStartVolume;
        private Halloumi.Common.Windows.Controls.Label lblPreFadeInStartVolume;
        private Halloumi.Common.Windows.Controls.CheckBox chkUsePreFadeIn;
        private Halloumi.Common.Windows.Controls.Panel pnlFadeIn;
        private System.Windows.Forms.TableLayoutPanel tblFadeIn;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader hdrFadeIn;
        private Halloumi.Common.Windows.Controls.Panel pnlFadeOut;
        private System.Windows.Forms.TableLayoutPanel tblFadeOut;
        private Halloumi.Common.Windows.Controls.Label lblFadeOutStartPosition;
        private Halloumi.Common.Windows.Controls.Label lblFadeOutLength;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader hdrFadeOut;
        private Halloumi.Common.Windows.Controls.Label lblPreFadeIn;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private Halloumi.Shuffler.Controls.SecondsComboBox cmbCustomFadeInLength;
        private Halloumi.Shuffler.Controls.SecondsComboBox cmbCustomFadeOutLength;
        private Halloumi.Common.Windows.Controls.ComboBox cmbFadeInLoopCount;
        private Halloumi.Common.Windows.Controls.Label lblFadeInLoopCount;
        private Halloumi.Common.Windows.Controls.SecondsTextBox txtPreFadeInStartPosition;
        private Halloumi.Common.Windows.Controls.Label lblPreFadeInStartPosition;
        private System.Windows.Forms.FlowLayoutPanel flpButtonsLeft;
        private Halloumi.Common.Windows.Controls.CheckBox chkPowerDown;
        private Halloumi.Shuffler.Controls.TrackWave trackWave;
        private Halloumi.Common.Windows.Controls.Label lblEndBPM;
        private Halloumi.Common.Windows.Controls.Button btnFadeOutUpdate;
        private Halloumi.Common.Windows.Controls.Label lblStartBPM;
        private Halloumi.Common.Windows.Controls.Button btnFadeInUpdate;
        private Halloumi.Common.Windows.Controls.ComboBox cmbFadeOutLoopCount;
        private Halloumi.Common.Windows.Controls.Label lblFadeOutLoopCount;
        private Halloumi.Common.Windows.Controls.Label lblPowerDown;
        private Halloumi.Common.Windows.Controls.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Halloumi.Common.Windows.Controls.ComboBox comboBox1;
        private Halloumi.Common.Windows.Controls.Label label1;
        private Halloumi.Common.Windows.Controls.Label label2;
        private Halloumi.Common.Windows.Controls.Panel panel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Halloumi.Common.Windows.Controls.Button btnAddSample;
        private Halloumi.Common.Windows.Controls.Button btnRemoveSample;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private Halloumi.Common.Windows.Controls.Label label5;
        private Halloumi.Common.Windows.Controls.Label label3;
        private Halloumi.Common.Windows.Controls.Label label4;
        private Halloumi.Shuffler.Controls.SecondsComboBox cmbSampleLength;
        private Halloumi.Common.Windows.Controls.SecondsTextBox txtSampleStartPosition;
        private Halloumi.Common.Windows.Controls.CheckBox chkLoopSample;
        private Halloumi.Common.Windows.Controls.Button btnSampleUpdate;
        private Halloumi.Common.Windows.Controls.Label lblSampleBPM;
        private Halloumi.Common.Windows.Controls.Button btnZoomSample;
        private Halloumi.Common.Windows.Controls.Button btnZoomFadeOut;
        private Halloumi.Common.Windows.Controls.Button btnZoomFadeIn;
        private Halloumi.Common.Windows.Controls.Button btnZoomPreFade;
        private Halloumi.Common.Windows.Controls.Label label6;
        private Halloumi.Common.Windows.Controls.ComboBox cmbOutput;
        private Halloumi.Common.Windows.Controls.Button btnPreFadeInUpdate;
        private Halloumi.Common.Windows.Controls.Panel panel4;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader3;
        private System.Windows.Forms.FlowLayoutPanel flpTrackFX;
        private Halloumi.Common.Windows.Controls.CheckBox chkShowTrackFX;
        private Halloumi.Common.Windows.Controls.Button btnAddTrackFX;
        private Halloumi.Common.Windows.Controls.Button btnDeleteTrackFX;
        private Halloumi.Common.Windows.Controls.Button btnUpdateTrackFX;
        private System.Windows.Forms.FlowLayoutPanel flpRight;
        private Halloumi.Common.Windows.Controls.Label label12;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rdbDelay1;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rdbDelay2;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rdbDelay3;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rdbDelay4;
        private Halloumi.Common.Windows.Controls.Button btnClearTrackFX;
        private Halloumi.Common.Windows.Controls.ComboBox cmbTrackFX;
        private Halloumi.Common.Windows.Controls.Button btnTrackFXZoom;
        private Halloumi.Common.Windows.Controls.Label label19;
        private Halloumi.Common.Windows.Controls.Label label15;
        private Halloumi.Common.Windows.Controls.CheckBox chkUseSkipSection;
        private Halloumi.Common.Windows.Controls.Label label18;
        private Halloumi.Common.Windows.Controls.SecondsTextBox txtSkipStart;
        private Halloumi.Common.Windows.Controls.Button btnSkipUpdate;
        private Halloumi.Shuffler.Controls.SecondsComboBox cmbSkipLength;
        private Halloumi.Common.Windows.Controls.Button btnSkipZoom;
        private Halloumi.Common.Windows.Controls.ListView lstSamples;
        private System.Windows.Forms.ColumnHeader colSample;
        private Halloumi.Common.Windows.Controls.Button btnRenameSample;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel5;
        private Common.Windows.Controls.Button btnCopyRight;
        private System.Windows.Forms.Panel panel2;
        private Common.Windows.Controls.Button btnCopyLeft;
        private Common.Windows.Controls.Button btnCalcStartBPM;
        private Common.Windows.Controls.Button btnCalcEndBPM;
    }
}