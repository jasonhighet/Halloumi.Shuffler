namespace Halloumi.Shuffler.Forms 
{
    partial class FrmEditTrackSamples
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
            var listViewItem1 = new System.Windows.Forms.ListViewItem("Sample #1");
            var listViewItem2 = new System.Windows.Forms.ListViewItem("Sample #2");
            var listViewItem3 = new System.Windows.Forms.ListViewItem("Sample #3");
            var listViewItem4 = new System.Windows.Forms.ListViewItem("Sample #4");
            this.pnlMain = new Halloumi.Common.Windows.Controls.Panel();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new Halloumi.Common.Windows.Controls.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbLoopMode = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label8 = new Halloumi.Common.Windows.Controls.Label();
            this.lblSampleBPM = new Halloumi.Common.Windows.Controls.Label();
            this.btnSampleUpdate = new Halloumi.Common.Windows.Controls.Button();
            this.label3 = new Halloumi.Common.Windows.Controls.Label();
            this.btnZoomSample = new Halloumi.Common.Windows.Controls.Button();
            this.txtSampleStartPosition = new Halloumi.Common.Windows.Controls.SecondsTextBox();
            this.label7 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbSampleLength = new Halloumi.Shuffler.Controls.SecondsComboBox();
            this.label4 = new Halloumi.Common.Windows.Controls.Label();
            this.txtSampleOffsetPosition = new Halloumi.Common.Windows.Controls.SecondsTextBox();
            this.label14 = new Halloumi.Common.Windows.Controls.Label();
            this.chkPrimaryLoop = new Halloumi.Common.Windows.Controls.CheckBox();
            this.label5 = new Halloumi.Common.Windows.Controls.Label();
            this.label15 = new Halloumi.Common.Windows.Controls.Label();
            this.chkAtonal = new Halloumi.Common.Windows.Controls.CheckBox();
            this.txtTags = new Halloumi.Common.Windows.Controls.TextBox();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.pnlSamples = new Halloumi.Common.Windows.Controls.Panel();
            this.tblSamples = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddSample = new Halloumi.Common.Windows.Controls.Button();
            this.lstSamples = new Halloumi.Common.Windows.Controls.ListView();
            this.colSample = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRemoveSample = new Halloumi.Common.Windows.Controls.Button();
            this.btnRenameSample = new Halloumi.Common.Windows.Controls.Button();
            this.btnImportSamplesFromMix = new Halloumi.Common.Windows.Controls.Button();
            this.hdrSamples = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.pnlTrackDetails = new Halloumi.Common.Windows.Controls.Panel();
            this.tblTrackDetails = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new Halloumi.Common.Windows.Controls.Label();
            this.label10 = new Halloumi.Common.Windows.Controls.Label();
            this.label11 = new Halloumi.Common.Windows.Controls.Label();
            this.label12 = new Halloumi.Common.Windows.Controls.Label();
            this.label13 = new Halloumi.Common.Windows.Controls.Label();
            this.lblTrackTitle = new Halloumi.Common.Windows.Controls.Label();
            this.lblTrackArtist = new Halloumi.Common.Windows.Controls.Label();
            this.lblTrackGenre = new Halloumi.Common.Windows.Controls.Label();
            this.lblTrackBPM = new Halloumi.Common.Windows.Controls.Label();
            this.lblTrackKey = new Halloumi.Common.Windows.Controls.Label();
            this.hdrTrackDetails = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
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
            this.label1 = new Halloumi.Common.Windows.Controls.Label();
            this.label2 = new Halloumi.Common.Windows.Controls.Label();
            this.pnlMain.SuspendLayout();
            this.tblMain.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLoopMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSampleLength)).BeginInit();
            this.pnlSamples.SuspendLayout();
            this.tblSamples.SuspendLayout();
            this.pnlTrackDetails.SuspendLayout();
            this.tblTrackDetails.SuspendLayout();
            this.flpButtonsRight.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.flpButtonsLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOutput)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.pnlMain.Size = new System.Drawing.Size(1633, 622);
            this.pnlMain.Style = Halloumi.Common.Windows.Controls.PanelStyle.Background;
            this.pnlMain.TabIndex = 0;
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 3;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblMain.Controls.Add(this.panel2, 1, 1);
            this.tblMain.Controls.Add(this.pnlSamples, 0, 1);
            this.tblMain.Controls.Add(this.pnlTrackDetails, 2, 1);
            this.tblMain.Controls.Add(this.trackWave, 0, 0);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(7, 6);
            this.tblMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 316F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 274F));
            this.tblMain.Size = new System.Drawing.Size(1619, 610);
            this.tblMain.TabIndex = 45;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.tableLayoutPanel2);
            this.panel2.Controls.Add(this.kryptonHeader1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(543, 320);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(1);
            this.panel2.Size = new System.Drawing.Size(531, 286);
            this.panel2.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.panel2.TabIndex = 48;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.Controls.Add(this.cmbLoopMode, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.lblSampleBPM, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnSampleUpdate, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnZoomSample, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtSampleStartPosition, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmbSampleLength, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtSampleOffsetPosition, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label14, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.chkPrimaryLoop, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label15, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.chkAtonal, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtTags, 1, 6);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 26);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28483F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28483F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28483F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28769F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28483F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28817F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28483F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(529, 259);
            this.tableLayoutPanel2.TabIndex = 11;
            // 
            // cmbLoopMode
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.cmbLoopMode, 2);
            this.cmbLoopMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoopMode.DropDownWidth = 72;
            this.cmbLoopMode.ErrorProvider = null;
            this.cmbLoopMode.Items.AddRange(new object[] {
            "Full Loop",
            "Partial Loop (Anchor Start)",
            "Partial Loop (Anchor End)"});
            this.cmbLoopMode.Location = new System.Drawing.Point(182, 185);
            this.cmbLoopMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbLoopMode.Name = "cmbLoopMode";
            this.cmbLoopMode.Size = new System.Drawing.Size(243, 25);
            this.cmbLoopMode.TabIndex = 71;
            this.cmbLoopMode.SelectedIndexChanged += new System.EventHandler(this.cmbLoopMode_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(11, 216);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label8.Size = new System.Drawing.Size(135, 30);
            this.label8.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label8.TabIndex = 65;
            this.label8.Text = "Tags:";
            // 
            // lblSampleBPM
            // 
            this.lblSampleBPM.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSampleBPM.ForeColor = System.Drawing.Color.White;
            this.lblSampleBPM.Location = new System.Drawing.Point(353, 76);
            this.lblSampleBPM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSampleBPM.Name = "lblSampleBPM";
            this.lblSampleBPM.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblSampleBPM.Size = new System.Drawing.Size(97, 30);
            this.lblSampleBPM.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblSampleBPM.TabIndex = 59;
            this.lblSampleBPM.Text = "100BPM";
            // 
            // btnSampleUpdate
            // 
            this.btnSampleUpdate.Location = new System.Drawing.Point(353, 6);
            this.btnSampleUpdate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnSampleUpdate.Name = "btnSampleUpdate";
            this.btnSampleUpdate.Size = new System.Drawing.Size(135, 31);
            this.btnSampleUpdate.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnSampleUpdate.TabIndex = 58;
            this.btnSampleUpdate.Text = "Update";
            this.btnSampleUpdate.Click += new System.EventHandler(this.btnSampleUpdate_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(11, 6);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label3.Size = new System.Drawing.Size(135, 30);
            this.label3.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label3.TabIndex = 52;
            this.label3.Text = "Start:";
            // 
            // btnZoomSample
            // 
            this.btnZoomSample.Location = new System.Drawing.Point(353, 41);
            this.btnZoomSample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnZoomSample.Name = "btnZoomSample";
            this.btnZoomSample.Size = new System.Drawing.Size(135, 31);
            this.btnZoomSample.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnZoomSample.TabIndex = 60;
            this.btnZoomSample.Text = "Zoom";
            this.btnZoomSample.Click += new System.EventHandler(this.btnZoomSample_Click);
            // 
            // txtSampleStartPosition
            // 
            this.txtSampleStartPosition.Location = new System.Drawing.Point(182, 10);
            this.txtSampleStartPosition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSampleStartPosition.Name = "txtSampleStartPosition";
            this.txtSampleStartPosition.Seconds = 0D;
            this.txtSampleStartPosition.Size = new System.Drawing.Size(124, 27);
            this.txtSampleStartPosition.TabIndex = 56;
            this.txtSampleStartPosition.Text = "00:00.0000";
            this.txtSampleStartPosition.TextChanged += new System.EventHandler(this.txtSampleStartPosition_TextChanged);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(11, 41);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label7.Size = new System.Drawing.Size(135, 30);
            this.label7.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label7.TabIndex = 63;
            this.label7.Text = "Offset:";
            // 
            // cmbSampleLength
            // 
            this.cmbSampleLength.DropDownWidth = 79;
            this.cmbSampleLength.Location = new System.Drawing.Point(182, 80);
            this.cmbSampleLength.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbSampleLength.Name = "cmbSampleLength";
            this.cmbSampleLength.Seconds = 0D;
            this.cmbSampleLength.Size = new System.Drawing.Size(124, 25);
            this.cmbSampleLength.TabIndex = 55;
            this.cmbSampleLength.Text = "00:00:0000";
            this.cmbSampleLength.SelectedIndexChanged += new System.EventHandler(this.cmbSampleLength_SelectedIndexChanged);
            this.cmbSampleLength.TextUpdate += new System.EventHandler(this.cmbSampleLength_TextUpdate);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(11, 76);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label4.Size = new System.Drawing.Size(135, 30);
            this.label4.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label4.TabIndex = 53;
            this.label4.Text = "Length:";
            // 
            // txtSampleOffsetPosition
            // 
            this.txtSampleOffsetPosition.Location = new System.Drawing.Point(182, 45);
            this.txtSampleOffsetPosition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSampleOffsetPosition.Name = "txtSampleOffsetPosition";
            this.txtSampleOffsetPosition.Seconds = 0D;
            this.txtSampleOffsetPosition.Size = new System.Drawing.Size(124, 27);
            this.txtSampleOffsetPosition.TabIndex = 64;
            this.txtSampleOffsetPosition.Text = "00:00.0000";
            this.txtSampleOffsetPosition.TextChanged += new System.EventHandler(this.txtSampleOffsetPosition_TextChanged);
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(11, 111);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label14.Size = new System.Drawing.Size(135, 30);
            this.label14.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label14.TabIndex = 67;
            this.label14.Text = "Primary Loop:";
            // 
            // chkPrimaryLoop
            // 
            this.chkPrimaryLoop.CheckedValue = "0.5";
            this.chkPrimaryLoop.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            this.chkPrimaryLoop.Location = new System.Drawing.Point(182, 115);
            this.chkPrimaryLoop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkPrimaryLoop.Name = "chkPrimaryLoop";
            this.chkPrimaryLoop.Size = new System.Drawing.Size(115, 24);
            this.chkPrimaryLoop.TabIndex = 68;
            this.chkPrimaryLoop.UncheckedValue = "1";
            this.chkPrimaryLoop.Value = "1";
            this.chkPrimaryLoop.Values.Text = "Primary Loop";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(11, 181);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label5.Size = new System.Drawing.Size(135, 30);
            this.label5.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label5.TabIndex = 54;
            this.label5.Text = "Loop:";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(11, 146);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label15.Size = new System.Drawing.Size(135, 30);
            this.label15.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label15.TabIndex = 69;
            this.label15.Text = "AtonalOnly:";
            // 
            // chkAtonal
            // 
            this.chkAtonal.CheckedValue = "0.5";
            this.chkAtonal.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            this.chkAtonal.Location = new System.Drawing.Point(182, 150);
            this.chkAtonal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkAtonal.Name = "chkAtonal";
            this.chkAtonal.Size = new System.Drawing.Size(69, 24);
            this.chkAtonal.TabIndex = 66;
            this.chkAtonal.UncheckedValue = "1";
            this.chkAtonal.Value = "1";
            this.chkAtonal.Values.Text = "AtonalOnly";
            // 
            // txtTags
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.txtTags, 2);
            this.txtTags.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtTags.ErrorProvider = null;
            this.txtTags.Location = new System.Drawing.Point(181, 218);
            this.txtTags.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTags.MaximumValue = 2147483647D;
            this.txtTags.MinimumValue = -2147483648D;
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(338, 27);
            this.txtTags.TabIndex = 70;
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(1, 1);
            this.kryptonHeader1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.Size = new System.Drawing.Size(529, 25);
            this.kryptonHeader1.StateCommon.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            this.kryptonHeader1.TabIndex = 0;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Sample Details";
            this.kryptonHeader1.Values.Image = null;
            // 
            // pnlSamples
            // 
            this.pnlSamples.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSamples.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSamples.Controls.Add(this.tblSamples);
            this.pnlSamples.Controls.Add(this.hdrSamples);
            this.pnlSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSamples.Location = new System.Drawing.Point(4, 320);
            this.pnlSamples.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlSamples.Name = "pnlSamples";
            this.pnlSamples.Padding = new System.Windows.Forms.Padding(1);
            this.pnlSamples.Size = new System.Drawing.Size(531, 286);
            this.pnlSamples.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlSamples.TabIndex = 47;
            // 
            // tblSamples
            // 
            this.tblSamples.ColumnCount = 3;
            this.tblSamples.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.67606F));
            this.tblSamples.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.85915F));
            this.tblSamples.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.46479F));
            this.tblSamples.Controls.Add(this.btnAddSample, 2, 0);
            this.tblSamples.Controls.Add(this.lstSamples, 0, 0);
            this.tblSamples.Controls.Add(this.btnRemoveSample, 2, 1);
            this.tblSamples.Controls.Add(this.btnRenameSample, 2, 2);
            this.tblSamples.Controls.Add(this.btnImportSamplesFromMix, 2, 3);
            this.tblSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblSamples.Location = new System.Drawing.Point(1, 26);
            this.tblSamples.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tblSamples.Name = "tblSamples";
            this.tblSamples.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tblSamples.RowCount = 5;
            this.tblSamples.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49855F));
            this.tblSamples.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49855F));
            this.tblSamples.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.50104F));
            this.tblSamples.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.50187F));
            this.tblSamples.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49854F));
            this.tblSamples.Size = new System.Drawing.Size(529, 259);
            this.tblSamples.TabIndex = 11;
            // 
            // btnAddSample
            // 
            this.btnAddSample.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddSample.Location = new System.Drawing.Point(384, 6);
            this.btnAddSample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnAddSample.Name = "btnAddSample";
            this.btnAddSample.Size = new System.Drawing.Size(134, 38);
            this.btnAddSample.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnAddSample.TabIndex = 50;
            this.btnAddSample.Text = "Add Sample";
            this.btnAddSample.Click += new System.EventHandler(this.btnAddSample_Click);
            // 
            // lstSamples
            // 
            this.lstSamples.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSample});
            this.tblSamples.SetColumnSpan(this.lstSamples, 2);
            this.lstSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSamples.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSamples.FullRowSelect = true;
            this.lstSamples.GridLines = true;
            this.lstSamples.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstSamples.HideSelection = false;
            this.lstSamples.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4});
            this.lstSamples.Location = new System.Drawing.Point(11, 10);
            this.lstSamples.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstSamples.MultiSelect = false;
            this.lstSamples.Name = "lstSamples";
            this.tblSamples.SetRowSpan(this.lstSamples, 5);
            this.lstSamples.Size = new System.Drawing.Size(365, 239);
            this.lstSamples.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstSamples.TabIndex = 61;
            this.lstSamples.UseCompatibleStateImageBehavior = false;
            this.lstSamples.View = System.Windows.Forms.View.Details;
            this.lstSamples.SelectedIndexChanged += new System.EventHandler(this.lstSamples_SelectedIndexChanged);
            // 
            // colSample
            // 
            this.colSample.Text = "Samples";
            this.colSample.Width = 400;
            // 
            // btnRemoveSample
            // 
            this.btnRemoveSample.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRemoveSample.Location = new System.Drawing.Point(384, 55);
            this.btnRemoveSample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnRemoveSample.Name = "btnRemoveSample";
            this.btnRemoveSample.Size = new System.Drawing.Size(134, 38);
            this.btnRemoveSample.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnRemoveSample.TabIndex = 51;
            this.btnRemoveSample.Text = "Remove Sample";
            this.btnRemoveSample.Click += new System.EventHandler(this.btnRemoveSample_Click);
            // 
            // btnRenameSample
            // 
            this.btnRenameSample.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRenameSample.Location = new System.Drawing.Point(384, 104);
            this.btnRenameSample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnRenameSample.Name = "btnRenameSample";
            this.btnRenameSample.Size = new System.Drawing.Size(134, 38);
            this.btnRenameSample.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnRenameSample.TabIndex = 62;
            this.btnRenameSample.Text = "Rename Sample";
            this.btnRenameSample.Click += new System.EventHandler(this.btnRenameSample_Click);
            // 
            // btnImportSamplesFromMix
            // 
            this.btnImportSamplesFromMix.Location = new System.Drawing.Point(384, 153);
            this.btnImportSamplesFromMix.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.btnImportSamplesFromMix.Name = "btnImportSamplesFromMix";
            this.btnImportSamplesFromMix.Size = new System.Drawing.Size(133, 38);
            this.btnImportSamplesFromMix.Style = Halloumi.Common.Windows.Controls.ButtonStyle.Secondary;
            this.btnImportSamplesFromMix.TabIndex = 63;
            this.btnImportSamplesFromMix.Text = "Import From Mix";
            this.btnImportSamplesFromMix.Click += new System.EventHandler(this.btnImportSamplesFromMix_Click);
            // 
            // hdrSamples
            // 
            this.hdrSamples.Dock = System.Windows.Forms.DockStyle.Top;
            this.hdrSamples.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.hdrSamples.Location = new System.Drawing.Point(1, 1);
            this.hdrSamples.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.hdrSamples.Name = "hdrSamples";
            this.hdrSamples.Size = new System.Drawing.Size(529, 25);
            this.hdrSamples.StateCommon.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            this.hdrSamples.TabIndex = 0;
            this.hdrSamples.Values.Description = "";
            this.hdrSamples.Values.Heading = "Samples";
            this.hdrSamples.Values.Image = null;
            // 
            // pnlTrackDetails
            // 
            this.pnlTrackDetails.BackColor = System.Drawing.SystemColors.Control;
            this.pnlTrackDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTrackDetails.Controls.Add(this.tblTrackDetails);
            this.pnlTrackDetails.Controls.Add(this.hdrTrackDetails);
            this.pnlTrackDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTrackDetails.Location = new System.Drawing.Point(1082, 320);
            this.pnlTrackDetails.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlTrackDetails.Name = "pnlTrackDetails";
            this.pnlTrackDetails.Padding = new System.Windows.Forms.Padding(1);
            this.pnlTrackDetails.Size = new System.Drawing.Size(533, 286);
            this.pnlTrackDetails.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlTrackDetails.TabIndex = 44;
            // 
            // tblTrackDetails
            // 
            this.tblTrackDetails.ColumnCount = 2;
            this.tblTrackDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.43327F));
            this.tblTrackDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.56673F));
            this.tblTrackDetails.Controls.Add(this.label9, 0, 0);
            this.tblTrackDetails.Controls.Add(this.label10, 0, 1);
            this.tblTrackDetails.Controls.Add(this.label11, 0, 2);
            this.tblTrackDetails.Controls.Add(this.label12, 0, 3);
            this.tblTrackDetails.Controls.Add(this.label13, 0, 4);
            this.tblTrackDetails.Controls.Add(this.lblTrackTitle, 1, 0);
            this.tblTrackDetails.Controls.Add(this.lblTrackArtist, 1, 1);
            this.tblTrackDetails.Controls.Add(this.lblTrackGenre, 1, 2);
            this.tblTrackDetails.Controls.Add(this.lblTrackBPM, 1, 3);
            this.tblTrackDetails.Controls.Add(this.lblTrackKey, 1, 4);
            this.tblTrackDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblTrackDetails.Location = new System.Drawing.Point(1, 26);
            this.tblTrackDetails.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tblTrackDetails.Name = "tblTrackDetails";
            this.tblTrackDetails.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tblTrackDetails.RowCount = 5;
            this.tblTrackDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblTrackDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblTrackDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblTrackDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblTrackDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblTrackDetails.Size = new System.Drawing.Size(531, 259);
            this.tblTrackDetails.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(11, 6);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label9.Size = new System.Drawing.Size(135, 30);
            this.label9.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label9.TabIndex = 53;
            this.label9.Text = "Title:";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(11, 55);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label10.Size = new System.Drawing.Size(135, 30);
            this.label10.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label10.TabIndex = 54;
            this.label10.Text = "Artist:";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(11, 104);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label11.Size = new System.Drawing.Size(135, 30);
            this.label11.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label11.TabIndex = 55;
            this.label11.Text = "Genre:";
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(11, 153);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label12.Size = new System.Drawing.Size(135, 30);
            this.label12.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label12.TabIndex = 56;
            this.label12.Text = "BPM:";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(11, 202);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label13.Size = new System.Drawing.Size(135, 30);
            this.label13.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label13.TabIndex = 57;
            this.label13.Text = "Key:";
            // 
            // lblTrackTitle
            // 
            this.lblTrackTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTrackTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTrackTitle.ForeColor = System.Drawing.Color.White;
            this.lblTrackTitle.Location = new System.Drawing.Point(158, 6);
            this.lblTrackTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTrackTitle.Name = "lblTrackTitle";
            this.lblTrackTitle.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblTrackTitle.Size = new System.Drawing.Size(362, 30);
            this.lblTrackTitle.Style = Halloumi.Common.Windows.Controls.LabelStyle.Text;
            this.lblTrackTitle.TabIndex = 58;
            this.lblTrackTitle.Text = "Title:";
            // 
            // lblTrackArtist
            // 
            this.lblTrackArtist.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTrackArtist.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTrackArtist.ForeColor = System.Drawing.Color.White;
            this.lblTrackArtist.Location = new System.Drawing.Point(158, 55);
            this.lblTrackArtist.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTrackArtist.Name = "lblTrackArtist";
            this.lblTrackArtist.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblTrackArtist.Size = new System.Drawing.Size(362, 30);
            this.lblTrackArtist.Style = Halloumi.Common.Windows.Controls.LabelStyle.Text;
            this.lblTrackArtist.TabIndex = 59;
            this.lblTrackArtist.Text = "Title:";
            // 
            // lblTrackGenre
            // 
            this.lblTrackGenre.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTrackGenre.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTrackGenre.ForeColor = System.Drawing.Color.White;
            this.lblTrackGenre.Location = new System.Drawing.Point(158, 104);
            this.lblTrackGenre.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTrackGenre.Name = "lblTrackGenre";
            this.lblTrackGenre.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblTrackGenre.Size = new System.Drawing.Size(362, 30);
            this.lblTrackGenre.Style = Halloumi.Common.Windows.Controls.LabelStyle.Text;
            this.lblTrackGenre.TabIndex = 60;
            this.lblTrackGenre.Text = "Title:";
            // 
            // lblTrackBPM
            // 
            this.lblTrackBPM.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTrackBPM.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTrackBPM.ForeColor = System.Drawing.Color.White;
            this.lblTrackBPM.Location = new System.Drawing.Point(158, 153);
            this.lblTrackBPM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTrackBPM.Name = "lblTrackBPM";
            this.lblTrackBPM.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblTrackBPM.Size = new System.Drawing.Size(362, 30);
            this.lblTrackBPM.Style = Halloumi.Common.Windows.Controls.LabelStyle.Text;
            this.lblTrackBPM.TabIndex = 61;
            this.lblTrackBPM.Text = "Title:";
            // 
            // lblTrackKey
            // 
            this.lblTrackKey.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTrackKey.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTrackKey.ForeColor = System.Drawing.Color.White;
            this.lblTrackKey.Location = new System.Drawing.Point(158, 202);
            this.lblTrackKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTrackKey.Name = "lblTrackKey";
            this.lblTrackKey.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblTrackKey.Size = new System.Drawing.Size(362, 30);
            this.lblTrackKey.Style = Halloumi.Common.Windows.Controls.LabelStyle.Text;
            this.lblTrackKey.TabIndex = 62;
            this.lblTrackKey.Text = "Title:";
            // 
            // hdrTrackDetails
            // 
            this.hdrTrackDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.hdrTrackDetails.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.hdrTrackDetails.Location = new System.Drawing.Point(1, 1);
            this.hdrTrackDetails.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.hdrTrackDetails.Name = "hdrTrackDetails";
            this.hdrTrackDetails.Size = new System.Drawing.Size(531, 25);
            this.hdrTrackDetails.StateCommon.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            this.hdrTrackDetails.TabIndex = 0;
            this.hdrTrackDetails.Values.Description = "";
            this.hdrTrackDetails.Values.Heading = "Track Details";
            this.hdrTrackDetails.Values.Image = null;
            // 
            // trackWave
            // 
            this.trackWave.BassPlayer = null;
            this.tblMain.SetColumnSpan(this.trackWave, 3);
            this.trackWave.CurrentSample = null;
            this.trackWave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackWave.Filename = null;
            this.trackWave.Location = new System.Drawing.Point(5, 5);
            this.trackWave.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.trackWave.Mode = Halloumi.Shuffler.Controls.TrackWave.TrackWaveMode.Shuffler;
            this.trackWave.Name = "trackWave";
            this.trackWave.Samples = null;
            this.trackWave.ShowTrackFx = false;
            this.trackWave.Size = new System.Drawing.Size(1609, 306);
            this.trackWave.TabIndex = 45;
            this.trackWave.TrackSamples = null;
            // 
            // linLine
            // 
            this.linLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.linLine.Location = new System.Drawing.Point(0, 622);
            this.linLine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.linLine.Name = "linLine";
            this.linLine.Size = new System.Drawing.Size(1633, 2);
            this.linLine.TabIndex = 0;
            // 
            // flpButtonsRight
            // 
            this.flpButtonsRight.BackColor = System.Drawing.Color.Transparent;
            this.flpButtonsRight.Controls.Add(this.btnCancel);
            this.flpButtonsRight.Controls.Add(this.btnOK);
            this.flpButtonsRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.flpButtonsRight.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpButtonsRight.Location = new System.Drawing.Point(1345, 1);
            this.flpButtonsRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flpButtonsRight.Name = "flpButtonsRight";
            this.flpButtonsRight.Padding = new System.Windows.Forms.Padding(7, 1, 0, 6);
            this.flpButtonsRight.Size = new System.Drawing.Size(287, 51);
            this.flpButtonsRight.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(155, 6);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 38);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(25, 6);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(120, 38);
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
            this.pnlButtons.Location = new System.Drawing.Point(0, 624);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(1);
            this.pnlButtons.Size = new System.Drawing.Size(1633, 53);
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
            this.flpButtonsLeft.Padding = new System.Windows.Forms.Padding(13, 8, 13, 12);
            this.flpButtonsLeft.Size = new System.Drawing.Size(295, 51);
            this.flpButtonsLeft.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(17, 8);
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
            this.cmbOutput.Location = new System.Drawing.Point(96, 12);
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
            // frmEditTrackSamples
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(32)))), ((int)(((byte)(48)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1633, 677);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.linLine);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmEditTrackSamples";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Halloumi : Shuffler : Samples";
            this.UseApplicationIcon = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSamples_FormClosed);
            this.Load += new System.EventHandler(this.frmSamples_Load);
            this.pnlMain.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLoopMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSampleLength)).EndInit();
            this.pnlSamples.ResumeLayout(false);
            this.pnlSamples.PerformLayout();
            this.tblSamples.ResumeLayout(false);
            this.pnlTrackDetails.ResumeLayout(false);
            this.pnlTrackDetails.PerformLayout();
            this.tblTrackDetails.ResumeLayout(false);
            this.flpButtonsRight.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.flpButtonsLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbOutput)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlMain;
        private Halloumi.Common.Windows.Controls.BeveledLine linLine;
        private System.Windows.Forms.FlowLayoutPanel flpButtonsRight;
        private Halloumi.Common.Windows.Controls.Button btnCancel;
        private Halloumi.Common.Windows.Controls.Button btnOK;
        private Halloumi.Common.Windows.Controls.Panel pnlButtons;
        private Halloumi.Common.Windows.Controls.Panel pnlTrackDetails;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader hdrTrackDetails;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.FlowLayoutPanel flpButtonsLeft;
        private Halloumi.Shuffler.Controls.TrackWave trackWave;
        private Halloumi.Common.Windows.Controls.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Halloumi.Common.Windows.Controls.Label label1;
        private Halloumi.Common.Windows.Controls.Label label2;
        private Halloumi.Common.Windows.Controls.Panel pnlSamples;
        private System.Windows.Forms.TableLayoutPanel tblSamples;
        private Halloumi.Common.Windows.Controls.Button btnAddSample;
        private Halloumi.Common.Windows.Controls.Button btnRemoveSample;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader hdrSamples;
        private Halloumi.Common.Windows.Controls.Label label5;
        private Halloumi.Common.Windows.Controls.Label label3;
        private Halloumi.Common.Windows.Controls.Label label4;
        private Halloumi.Shuffler.Controls.SecondsComboBox cmbSampleLength;
        private Halloumi.Common.Windows.Controls.SecondsTextBox txtSampleStartPosition;
        private Halloumi.Common.Windows.Controls.Button btnSampleUpdate;
        private Halloumi.Common.Windows.Controls.Label lblSampleBPM;
        private Halloumi.Common.Windows.Controls.Button btnZoomSample;
        private Halloumi.Common.Windows.Controls.Label label6;
        private Halloumi.Common.Windows.Controls.ComboBox cmbOutput;
        private Halloumi.Common.Windows.Controls.ListView lstSamples;
        private System.Windows.Forms.ColumnHeader colSample;
        private Halloumi.Common.Windows.Controls.Button btnRenameSample;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tblTrackDetails;
        private Halloumi.Common.Windows.Controls.Label label8;
        private Halloumi.Common.Windows.Controls.Label label7;
        private Halloumi.Common.Windows.Controls.SecondsTextBox txtSampleOffsetPosition;
        private Halloumi.Common.Windows.Controls.CheckBox chkAtonal;
        private Halloumi.Common.Windows.Controls.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private Halloumi.Common.Windows.Controls.Label label9;
        private Halloumi.Common.Windows.Controls.Label label10;
        private Halloumi.Common.Windows.Controls.Label label11;
        private Halloumi.Common.Windows.Controls.Label label12;
        private Halloumi.Common.Windows.Controls.Label label13;
        private Halloumi.Common.Windows.Controls.Label lblTrackTitle;
        private Halloumi.Common.Windows.Controls.Label lblTrackArtist;
        private Halloumi.Common.Windows.Controls.Label lblTrackGenre;
        private Halloumi.Common.Windows.Controls.Label lblTrackBPM;
        private Halloumi.Common.Windows.Controls.Label lblTrackKey;
        private Halloumi.Common.Windows.Controls.Label label14;
        private Halloumi.Common.Windows.Controls.CheckBox chkPrimaryLoop;
        private Halloumi.Common.Windows.Controls.Label label15;
        private Halloumi.Common.Windows.Controls.TextBox txtTags;
        private Halloumi.Common.Windows.Controls.ComboBox cmbLoopMode;
        private Halloumi.Common.Windows.Controls.Button btnImportSamplesFromMix;
    }
}