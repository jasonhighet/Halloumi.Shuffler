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
            pnlMain = new Halloumi.Common.Windows.Controls.Panel();
            tblMain = new System.Windows.Forms.TableLayoutPanel();
            panel4 = new Halloumi.Common.Windows.Controls.Panel();
            flpTrackFX = new System.Windows.Forms.FlowLayoutPanel();
            chkShowTrackFX = new Halloumi.Common.Windows.Controls.CheckBox();
            cmbTrackFX = new Halloumi.Common.Windows.Controls.ComboBox();
            flpRight = new System.Windows.Forms.FlowLayoutPanel();
            label12 = new Halloumi.Common.Windows.Controls.Label();
            rdbDelay1 = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            rdbDelay2 = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            rdbDelay3 = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            rdbDelay4 = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            btnUpdateTrackFX = new Halloumi.Common.Windows.Controls.Button();
            btnDeleteTrackFX = new Halloumi.Common.Windows.Controls.Button();
            btnAddTrackFX = new Halloumi.Common.Windows.Controls.Button();
            btnClearTrackFX = new Halloumi.Common.Windows.Controls.Button();
            btnTrackFXZoom = new Halloumi.Common.Windows.Controls.Button();
            kryptonHeader3 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            panel3 = new Halloumi.Common.Windows.Controls.Panel();
            tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            label5 = new Halloumi.Common.Windows.Controls.Label();
            btnAddSample = new Halloumi.Common.Windows.Controls.Button();
            label3 = new Halloumi.Common.Windows.Controls.Label();
            label4 = new Halloumi.Common.Windows.Controls.Label();
            cmbSampleLength = new Halloumi.Shuffler.Controls.SecondsComboBox();
            txtSampleStartPosition = new Halloumi.Common.Windows.Controls.SecondsTextBox();
            chkLoopSample = new Halloumi.Common.Windows.Controls.CheckBox();
            btnSampleUpdate = new Halloumi.Common.Windows.Controls.Button();
            lblSampleBPM = new Halloumi.Common.Windows.Controls.Label();
            btnZoomSample = new Halloumi.Common.Windows.Controls.Button();
            lstSamples = new Halloumi.Common.Windows.Controls.ListView();
            colSample = new System.Windows.Forms.ColumnHeader();
            btnRemoveSample = new Halloumi.Common.Windows.Controls.Button();
            btnRenameSample = new Halloumi.Common.Windows.Controls.Button();
            kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            pnlFadeOut = new Halloumi.Common.Windows.Controls.Panel();
            tblFadeOut = new System.Windows.Forms.TableLayoutPanel();
            btnCalcEndBPM = new Halloumi.Common.Windows.Controls.Button();
            label19 = new Halloumi.Common.Windows.Controls.Label();
            cmbFadeOutLoopCount = new Halloumi.Common.Windows.Controls.ComboBox();
            lblFadeOutLoopCount = new Halloumi.Common.Windows.Controls.Label();
            lblFadeOutStartPosition = new Halloumi.Common.Windows.Controls.Label();
            lblFadeOutLength = new Halloumi.Common.Windows.Controls.Label();
            txtFadeOutStartPosition = new Halloumi.Common.Windows.Controls.SecondsTextBox();
            lblPowerDown = new Halloumi.Common.Windows.Controls.Label();
            chkPowerDown = new Halloumi.Common.Windows.Controls.CheckBox();
            btnFadeOutUpdate = new Halloumi.Common.Windows.Controls.Button();
            btnZoomFadeOut = new Halloumi.Common.Windows.Controls.Button();
            label15 = new Halloumi.Common.Windows.Controls.Label();
            chkUseSkipSection = new Halloumi.Common.Windows.Controls.CheckBox();
            label18 = new Halloumi.Common.Windows.Controls.Label();
            txtSkipStart = new Halloumi.Common.Windows.Controls.SecondsTextBox();
            btnSkipUpdate = new Halloumi.Common.Windows.Controls.Button();
            cmbSkipLength = new Halloumi.Shuffler.Controls.SecondsComboBox();
            btnSkipZoom = new Halloumi.Common.Windows.Controls.Button();
            panel5 = new System.Windows.Forms.Panel();
            btnCopyRight = new Halloumi.Common.Windows.Controls.Button();
            cmbCustomFadeOutLength = new Halloumi.Shuffler.Controls.SecondsComboBox();
            lblEndBPM = new Halloumi.Common.Windows.Controls.Label();
            hdrFadeOut = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            pnlFadeIn = new Halloumi.Common.Windows.Controls.Panel();
            tblFadeIn = new System.Windows.Forms.TableLayoutPanel();
            btnCalcStartBPM = new Halloumi.Common.Windows.Controls.Button();
            btnPreFadeInUpdate = new Halloumi.Common.Windows.Controls.Button();
            lblFadeInLoopCount = new Halloumi.Common.Windows.Controls.Label();
            lblPreFadeIn = new Halloumi.Common.Windows.Controls.Label();
            lblFadeInPosition = new Halloumi.Common.Windows.Controls.Label();
            txtFadeInPosition = new Halloumi.Common.Windows.Controls.SecondsTextBox();
            txtPreFadeInStartPosition = new Halloumi.Common.Windows.Controls.SecondsTextBox();
            lblPreFadeInStartPosition = new Halloumi.Common.Windows.Controls.Label();
            lblPreFadeInStartVolume = new Halloumi.Common.Windows.Controls.Label();
            lblFadeInLength = new Halloumi.Common.Windows.Controls.Label();
            chkUsePreFadeIn = new Halloumi.Common.Windows.Controls.CheckBox();
            cmbFadeInLoopCount = new Halloumi.Common.Windows.Controls.ComboBox();
            cmbPreFadeInStartVolume = new Halloumi.Common.Windows.Controls.ComboBox();
            btnFadeInUpdate = new Halloumi.Common.Windows.Controls.Button();
            lblStartBPM = new Halloumi.Common.Windows.Controls.Label();
            btnZoomFadeIn = new Halloumi.Common.Windows.Controls.Button();
            btnZoomPreFade = new Halloumi.Common.Windows.Controls.Button();
            panel2 = new System.Windows.Forms.Panel();
            btnCopyLeft = new Halloumi.Common.Windows.Controls.Button();
            cmbCustomFadeInLength = new Halloumi.Shuffler.Controls.SecondsComboBox();
            hdrFadeIn = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            trackWave = new Halloumi.Shuffler.Controls.TrackWave();
            pnlTrackRank = new System.Windows.Forms.FlowLayoutPanel();
            lblTrackRank = new Halloumi.Common.Windows.Controls.Label();
            cmbTrackRank = new Halloumi.Common.Windows.Controls.ComboBox();
            linLine = new Halloumi.Common.Windows.Controls.BeveledLine();
            flpButtonsRight = new System.Windows.Forms.FlowLayoutPanel();
            btnCancel = new Halloumi.Common.Windows.Controls.Button();
            btnOK = new Halloumi.Common.Windows.Controls.Button();
            pnlButtons = new Halloumi.Common.Windows.Controls.Panel();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            flpButtonsLeft = new System.Windows.Forms.FlowLayoutPanel();
            label6 = new Halloumi.Common.Windows.Controls.Label();
            cmbOutput = new Halloumi.Common.Windows.Controls.ComboBox();
            panel1 = new Halloumi.Common.Windows.Controls.Panel();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            comboBox1 = new Halloumi.Common.Windows.Controls.ComboBox();
            label1 = new Halloumi.Common.Windows.Controls.Label();
            label2 = new Halloumi.Common.Windows.Controls.Label();
            pnlMain.SuspendLayout();
            tblMain.SuspendLayout();
            panel4.SuspendLayout();
            flpTrackFX.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cmbTrackFX).BeginInit();
            flpRight.SuspendLayout();
            panel3.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cmbSampleLength).BeginInit();
            pnlFadeOut.SuspendLayout();
            tblFadeOut.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cmbFadeOutLoopCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cmbSkipLength).BeginInit();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cmbCustomFadeOutLength).BeginInit();
            pnlFadeIn.SuspendLayout();
            tblFadeIn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cmbFadeInLoopCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cmbPreFadeInStartVolume).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cmbCustomFadeInLength).BeginInit();
            pnlTrackRank.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cmbTrackRank).BeginInit();
            flpButtonsRight.SuspendLayout();
            pnlButtons.SuspendLayout();
            flpButtonsLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cmbOutput).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)comboBox1).BeginInit();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.BackColor = System.Drawing.SystemColors.Control;
            pnlMain.Controls.Add(tblMain);
            pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlMain.Location = new System.Drawing.Point(0, 0);
            pnlMain.Margin = new System.Windows.Forms.Padding(6);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            pnlMain.Size = new System.Drawing.Size(2668, 948);
            pnlMain.Style = Common.Windows.Controls.PanelStyle.Background;
            pnlMain.TabIndex = 0;
            // 
            // tblMain
            // 
            tblMain.ColumnCount = 3;
            tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3333244F));
            tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3333359F));
            tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3333359F));
            tblMain.Controls.Add(panel4, 0, 2);
            tblMain.Controls.Add(panel3, 2, 1);
            tblMain.Controls.Add(pnlFadeOut, 1, 1);
            tblMain.Controls.Add(pnlFadeIn, 0, 1);
            tblMain.Controls.Add(trackWave, 0, 0);
            tblMain.Controls.Add(pnlTrackRank, 0, 3);
            tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tblMain.Location = new System.Drawing.Point(10, 9);
            tblMain.Margin = new System.Windows.Forms.Padding(6);
            tblMain.Name = "tblMain";
            tblMain.RowCount = 4;
            tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 513F));
            tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 167F));
            tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            tblMain.Size = new System.Drawing.Size(2648, 930);
            tblMain.TabIndex = 45;
            // 
            // panel4
            // 
            panel4.BackColor = System.Drawing.SystemColors.Control;
            panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tblMain.SetColumnSpan(panel4, 3);
            panel4.Controls.Add(flpTrackFX);
            panel4.Controls.Add(kryptonHeader3);
            panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            panel4.Location = new System.Drawing.Point(6, 700);
            panel4.Margin = new System.Windows.Forms.Padding(6);
            panel4.Name = "panel4";
            panel4.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            panel4.Size = new System.Drawing.Size(2636, 155);
            panel4.Style = Common.Windows.Controls.PanelStyle.Content;
            panel4.TabIndex = 49;
            // 
            // flpTrackFX
            // 
            flpTrackFX.Controls.Add(chkShowTrackFX);
            flpTrackFX.Controls.Add(cmbTrackFX);
            flpTrackFX.Controls.Add(flpRight);
            flpTrackFX.Controls.Add(btnUpdateTrackFX);
            flpTrackFX.Controls.Add(btnDeleteTrackFX);
            flpTrackFX.Controls.Add(btnAddTrackFX);
            flpTrackFX.Controls.Add(btnClearTrackFX);
            flpTrackFX.Controls.Add(btnTrackFXZoom);
            flpTrackFX.Dock = System.Windows.Forms.DockStyle.Fill;
            flpTrackFX.Location = new System.Drawing.Point(1, 36);
            flpTrackFX.Margin = new System.Windows.Forms.Padding(6);
            flpTrackFX.Name = "flpTrackFX";
            flpTrackFX.Padding = new System.Windows.Forms.Padding(10, 4, 10, 9);
            flpTrackFX.Size = new System.Drawing.Size(2634, 117);
            flpTrackFX.TabIndex = 2;
            // 
            // chkShowTrackFX
            // 
            chkShowTrackFX.AutoSize = false;
            chkShowTrackFX.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.BoldPanel;
            chkShowTrackFX.Location = new System.Drawing.Point(16, 10);
            chkShowTrackFX.Margin = new System.Windows.Forms.Padding(6);
            chkShowTrackFX.Name = "chkShowTrackFX";
            chkShowTrackFX.Size = new System.Drawing.Size(232, 46);
            chkShowTrackFX.TabIndex = 41;
            chkShowTrackFX.Values.Text = "Show Track FX";
            chkShowTrackFX.CheckedChanged += chkShowTrackFX_CheckedChanged;
            // 
            // cmbTrackFX
            // 
            cmbTrackFX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbTrackFX.DropDownWidth = 72;
            cmbTrackFX.ErrorProvider = null;
            cmbTrackFX.Items.AddRange(new object[] { "Speakers", "Monitor", "Both" });
            cmbTrackFX.Location = new System.Drawing.Point(260, 10);
            cmbTrackFX.Margin = new System.Windows.Forms.Padding(6);
            cmbTrackFX.Name = "cmbTrackFX";
            cmbTrackFX.Size = new System.Drawing.Size(199, 33);
            cmbTrackFX.TabIndex = 47;
            cmbTrackFX.SelectedIndexChanged += cmbTrackFX_SelectedIndexChanged;
            // 
            // flpRight
            // 
            flpRight.BackColor = System.Drawing.Color.Transparent;
            flpRight.Controls.Add(label12);
            flpRight.Controls.Add(rdbDelay1);
            flpRight.Controls.Add(rdbDelay2);
            flpRight.Controls.Add(rdbDelay3);
            flpRight.Controls.Add(rdbDelay4);
            flpRight.Location = new System.Drawing.Point(471, 10);
            flpRight.Margin = new System.Windows.Forms.Padding(6);
            flpRight.Name = "flpRight";
            flpRight.Size = new System.Drawing.Size(455, 52);
            flpRight.TabIndex = 45;
            // 
            // label12
            // 
            label12.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label12.ForeColor = System.Drawing.Color.White;
            label12.Location = new System.Drawing.Point(6, 0);
            label12.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(77, 42);
            label12.Style = Common.Windows.Controls.LabelStyle.Caption;
            label12.TabIndex = 57;
            label12.Text = "Delay:";
            label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rdbDelay1
            // 
            rdbDelay1.Location = new System.Drawing.Point(95, 6);
            rdbDelay1.Margin = new System.Windows.Forms.Padding(6);
            rdbDelay1.Name = "rdbDelay1";
            rdbDelay1.Size = new System.Drawing.Size(57, 33);
            rdbDelay1.TabIndex = 62;
            rdbDelay1.Tag = "0.5";
            rdbDelay1.Values.Text = "1/2";
            // 
            // rdbDelay2
            // 
            rdbDelay2.Checked = true;
            rdbDelay2.Location = new System.Drawing.Point(164, 6);
            rdbDelay2.Margin = new System.Windows.Forms.Padding(6);
            rdbDelay2.Name = "rdbDelay2";
            rdbDelay2.Size = new System.Drawing.Size(57, 33);
            rdbDelay2.TabIndex = 63;
            rdbDelay2.Tag = "0.25";
            rdbDelay2.Values.Text = "1/4";
            // 
            // rdbDelay3
            // 
            rdbDelay3.Location = new System.Drawing.Point(233, 6);
            rdbDelay3.Margin = new System.Windows.Forms.Padding(6);
            rdbDelay3.Name = "rdbDelay3";
            rdbDelay3.Size = new System.Drawing.Size(57, 33);
            rdbDelay3.TabIndex = 64;
            rdbDelay3.Tag = "0.125";
            rdbDelay3.Values.Text = "1/8";
            // 
            // rdbDelay4
            // 
            rdbDelay4.Location = new System.Drawing.Point(302, 6);
            rdbDelay4.Margin = new System.Windows.Forms.Padding(6);
            rdbDelay4.Name = "rdbDelay4";
            rdbDelay4.Size = new System.Drawing.Size(69, 33);
            rdbDelay4.TabIndex = 65;
            rdbDelay4.Tag = "0.0625";
            rdbDelay4.Values.Text = "1/16";
            // 
            // btnUpdateTrackFX
            // 
            btnUpdateTrackFX.Location = new System.Drawing.Point(939, 12);
            btnUpdateTrackFX.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            btnUpdateTrackFX.Name = "btnUpdateTrackFX";
            btnUpdateTrackFX.Size = new System.Drawing.Size(122, 57);
            btnUpdateTrackFX.TabIndex = 43;
            btnUpdateTrackFX.Text = "Update";
            btnUpdateTrackFX.Click += btnUpdateTrackFX_Click;
            // 
            // btnDeleteTrackFX
            // 
            btnDeleteTrackFX.Location = new System.Drawing.Point(1075, 12);
            btnDeleteTrackFX.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            btnDeleteTrackFX.Name = "btnDeleteTrackFX";
            btnDeleteTrackFX.Size = new System.Drawing.Size(122, 57);
            btnDeleteTrackFX.TabIndex = 42;
            btnDeleteTrackFX.Text = "Delete";
            btnDeleteTrackFX.Click += btnDeleteTrackFX_Click;
            // 
            // btnAddTrackFX
            // 
            btnAddTrackFX.Location = new System.Drawing.Point(1211, 12);
            btnAddTrackFX.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            btnAddTrackFX.Name = "btnAddTrackFX";
            btnAddTrackFX.Size = new System.Drawing.Size(122, 57);
            btnAddTrackFX.TabIndex = 44;
            btnAddTrackFX.Text = "Add";
            btnAddTrackFX.Click += btnAddTrackFX_Click;
            // 
            // btnClearTrackFX
            // 
            btnClearTrackFX.Location = new System.Drawing.Point(1347, 12);
            btnClearTrackFX.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            btnClearTrackFX.Name = "btnClearTrackFX";
            btnClearTrackFX.Size = new System.Drawing.Size(122, 57);
            btnClearTrackFX.TabIndex = 46;
            btnClearTrackFX.Text = "Clear";
            btnClearTrackFX.Click += btnClearTrackFX_Click;
            // 
            // btnTrackFXZoom
            // 
            btnTrackFXZoom.Location = new System.Drawing.Point(1483, 12);
            btnTrackFXZoom.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            btnTrackFXZoom.Name = "btnTrackFXZoom";
            btnTrackFXZoom.Size = new System.Drawing.Size(122, 57);
            btnTrackFXZoom.TabIndex = 48;
            btnTrackFXZoom.Text = "Clear";
            btnTrackFXZoom.Click += btnTrackFXZoom_Click;
            // 
            // kryptonHeader3
            // 
            kryptonHeader3.Dock = System.Windows.Forms.DockStyle.Top;
            kryptonHeader3.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            kryptonHeader3.Location = new System.Drawing.Point(1, 2);
            kryptonHeader3.Margin = new System.Windows.Forms.Padding(6);
            kryptonHeader3.Name = "kryptonHeader3";
            kryptonHeader3.Size = new System.Drawing.Size(2634, 34);
            kryptonHeader3.StateCommon.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            kryptonHeader3.TabIndex = 1;
            kryptonHeader3.Values.Description = "";
            kryptonHeader3.Values.Heading = "Track FX Automation";
            kryptonHeader3.Values.Image = null;
            // 
            // panel3
            // 
            panel3.BackColor = System.Drawing.SystemColors.Control;
            panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel3.Controls.Add(tableLayoutPanel3);
            panel3.Controls.Add(kryptonHeader2);
            panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            panel3.Location = new System.Drawing.Point(1770, 187);
            panel3.Margin = new System.Windows.Forms.Padding(6);
            panel3.Name = "panel3";
            panel3.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            panel3.Size = new System.Drawing.Size(872, 501);
            panel3.Style = Common.Windows.Controls.PanelStyle.Content;
            panel3.TabIndex = 47;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.67606F));
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.85915F));
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.46479F));
            tableLayoutPanel3.Controls.Add(label5, 0, 6);
            tableLayoutPanel3.Controls.Add(btnAddSample, 2, 0);
            tableLayoutPanel3.Controls.Add(label3, 0, 4);
            tableLayoutPanel3.Controls.Add(label4, 0, 5);
            tableLayoutPanel3.Controls.Add(cmbSampleLength, 1, 5);
            tableLayoutPanel3.Controls.Add(txtSampleStartPosition, 1, 4);
            tableLayoutPanel3.Controls.Add(chkLoopSample, 1, 6);
            tableLayoutPanel3.Controls.Add(btnSampleUpdate, 2, 4);
            tableLayoutPanel3.Controls.Add(lblSampleBPM, 2, 6);
            tableLayoutPanel3.Controls.Add(btnZoomSample, 2, 5);
            tableLayoutPanel3.Controls.Add(lstSamples, 0, 0);
            tableLayoutPanel3.Controls.Add(btnRemoveSample, 2, 1);
            tableLayoutPanel3.Controls.Add(btnRenameSample, 2, 2);
            tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel3.Location = new System.Drawing.Point(1, 36);
            tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(6);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            tableLayoutPanel3.RowCount = 7;
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66556F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66556F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66889F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66889F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66555F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66555F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            tableLayoutPanel3.Size = new System.Drawing.Size(870, 463);
            tableLayoutPanel3.TabIndex = 11;
            // 
            // label5
            // 
            label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label5.ForeColor = System.Drawing.Color.White;
            label5.Location = new System.Drawing.Point(16, 379);
            label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label5.Name = "label5";
            label5.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            label5.Size = new System.Drawing.Size(186, 44);
            label5.Style = Common.Windows.Controls.LabelStyle.Caption;
            label5.TabIndex = 54;
            label5.Text = "Looped:";
            // 
            // btnAddSample
            // 
            btnAddSample.Dock = System.Windows.Forms.DockStyle.Top;
            btnAddSample.Location = new System.Drawing.Point(632, 9);
            btnAddSample.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnAddSample.Name = "btnAddSample";
            btnAddSample.Size = new System.Drawing.Size(222, 44);
            btnAddSample.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnAddSample.TabIndex = 50;
            btnAddSample.Text = "Add Sample";
            btnAddSample.Click += btnAddSample_Click;
            // 
            // label3
            // 
            label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label3.ForeColor = System.Drawing.Color.White;
            label3.Location = new System.Drawing.Point(16, 237);
            label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label3.Name = "label3";
            label3.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            label3.Size = new System.Drawing.Size(186, 44);
            label3.Style = Common.Windows.Controls.LabelStyle.Caption;
            label3.TabIndex = 52;
            label3.Text = "Start:";
            // 
            // label4
            // 
            label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label4.ForeColor = System.Drawing.Color.White;
            label4.Location = new System.Drawing.Point(16, 308);
            label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label4.Name = "label4";
            label4.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            label4.Size = new System.Drawing.Size(186, 44);
            label4.Style = Common.Windows.Controls.LabelStyle.Caption;
            label4.TabIndex = 53;
            label4.Text = "Length:";
            // 
            // cmbSampleLength
            // 
            cmbSampleLength.DropDownWidth = 79;
            cmbSampleLength.Location = new System.Drawing.Point(336, 314);
            cmbSampleLength.Margin = new System.Windows.Forms.Padding(6);
            cmbSampleLength.Name = "cmbSampleLength";
            cmbSampleLength.Seconds = 0D;
            cmbSampleLength.Size = new System.Drawing.Size(170, 33);
            cmbSampleLength.TabIndex = 55;
            cmbSampleLength.Text = "00:00:0000";
            // 
            // txtSampleStartPosition
            // 
            txtSampleStartPosition.Location = new System.Drawing.Point(336, 243);
            txtSampleStartPosition.Margin = new System.Windows.Forms.Padding(6);
            txtSampleStartPosition.Name = "txtSampleStartPosition";
            txtSampleStartPosition.Seconds = 0D;
            txtSampleStartPosition.Size = new System.Drawing.Size(170, 37);
            txtSampleStartPosition.TabIndex = 56;
            txtSampleStartPosition.Text = "00:00.0000";
            // 
            // chkLoopSample
            // 
            chkLoopSample.CheckedValue = "0.5";
            chkLoopSample.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            chkLoopSample.Location = new System.Drawing.Point(336, 385);
            chkLoopSample.Margin = new System.Windows.Forms.Padding(6);
            chkLoopSample.Name = "chkLoopSample";
            chkLoopSample.Size = new System.Drawing.Size(99, 33);
            chkLoopSample.TabIndex = 57;
            chkLoopSample.UncheckedValue = "1";
            chkLoopSample.Value = "1";
            chkLoopSample.Values.Text = "Looped";
            // 
            // btnSampleUpdate
            // 
            btnSampleUpdate.Dock = System.Windows.Forms.DockStyle.Top;
            btnSampleUpdate.Location = new System.Drawing.Point(632, 237);
            btnSampleUpdate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnSampleUpdate.Name = "btnSampleUpdate";
            btnSampleUpdate.Size = new System.Drawing.Size(222, 44);
            btnSampleUpdate.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnSampleUpdate.TabIndex = 58;
            btnSampleUpdate.Text = "Update";
            btnSampleUpdate.Click += btnSampleUpdate_Click;
            // 
            // lblSampleBPM
            // 
            lblSampleBPM.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblSampleBPM.ForeColor = System.Drawing.Color.White;
            lblSampleBPM.Location = new System.Drawing.Point(632, 379);
            lblSampleBPM.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblSampleBPM.Name = "lblSampleBPM";
            lblSampleBPM.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            lblSampleBPM.Size = new System.Drawing.Size(133, 44);
            lblSampleBPM.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblSampleBPM.TabIndex = 59;
            lblSampleBPM.Text = "100BPM";
            // 
            // btnZoomSample
            // 
            btnZoomSample.Dock = System.Windows.Forms.DockStyle.Top;
            btnZoomSample.Location = new System.Drawing.Point(632, 308);
            btnZoomSample.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnZoomSample.Name = "btnZoomSample";
            btnZoomSample.Size = new System.Drawing.Size(222, 44);
            btnZoomSample.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnZoomSample.TabIndex = 60;
            btnZoomSample.Text = "Zoom";
            btnZoomSample.Click += btnZoomSample_Click;
            // 
            // lstSamples
            // 
            lstSamples.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { colSample });
            tableLayoutPanel3.SetColumnSpan(lstSamples, 2);
            lstSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            lstSamples.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lstSamples.FullRowSelect = true;
            lstSamples.GridLines = true;
            lstSamples.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            lstSamples.HideSelection = false;
            lstSamples.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem5, listViewItem6, listViewItem7, listViewItem8 });
            lstSamples.Location = new System.Drawing.Point(16, 15);
            lstSamples.Margin = new System.Windows.Forms.Padding(6);
            lstSamples.MultiSelect = false;
            lstSamples.Name = "lstSamples";
            tableLayoutPanel3.SetRowSpan(lstSamples, 3);
            lstSamples.Size = new System.Drawing.Size(604, 201);
            lstSamples.Sorting = System.Windows.Forms.SortOrder.Ascending;
            lstSamples.TabIndex = 61;
            lstSamples.UseCompatibleStateImageBehavior = false;
            lstSamples.View = System.Windows.Forms.View.Details;
            lstSamples.SelectedIndexChanged += lstSamples_SelectedIndexChanged;
            // 
            // colSample
            // 
            colSample.Text = "Samples";
            colSample.Width = 252;
            // 
            // btnRemoveSample
            // 
            btnRemoveSample.Location = new System.Drawing.Point(632, 80);
            btnRemoveSample.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnRemoveSample.Name = "btnRemoveSample";
            btnRemoveSample.Size = new System.Drawing.Size(183, 44);
            btnRemoveSample.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnRemoveSample.TabIndex = 51;
            btnRemoveSample.Text = "Remove Sample";
            btnRemoveSample.Click += btnRemoveSample_Click;
            // 
            // btnRenameSample
            // 
            btnRenameSample.Location = new System.Drawing.Point(632, 151);
            btnRenameSample.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnRenameSample.Name = "btnRenameSample";
            btnRenameSample.Size = new System.Drawing.Size(183, 44);
            btnRenameSample.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnRenameSample.TabIndex = 62;
            btnRenameSample.Text = "Rename Sample";
            btnRenameSample.Click += btnRenameSample_Click;
            // 
            // kryptonHeader2
            // 
            kryptonHeader2.Dock = System.Windows.Forms.DockStyle.Top;
            kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            kryptonHeader2.Location = new System.Drawing.Point(1, 2);
            kryptonHeader2.Margin = new System.Windows.Forms.Padding(6);
            kryptonHeader2.Name = "kryptonHeader2";
            kryptonHeader2.Size = new System.Drawing.Size(870, 34);
            kryptonHeader2.StateCommon.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            kryptonHeader2.TabIndex = 0;
            kryptonHeader2.Values.Description = "";
            kryptonHeader2.Values.Heading = "Samples";
            kryptonHeader2.Values.Image = null;
            // 
            // pnlFadeOut
            // 
            pnlFadeOut.BackColor = System.Drawing.SystemColors.Control;
            pnlFadeOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlFadeOut.Controls.Add(tblFadeOut);
            pnlFadeOut.Controls.Add(hdrFadeOut);
            pnlFadeOut.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlFadeOut.Location = new System.Drawing.Point(888, 187);
            pnlFadeOut.Margin = new System.Windows.Forms.Padding(6);
            pnlFadeOut.Name = "pnlFadeOut";
            pnlFadeOut.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            pnlFadeOut.Size = new System.Drawing.Size(870, 501);
            pnlFadeOut.Style = Common.Windows.Controls.PanelStyle.Content;
            pnlFadeOut.TabIndex = 44;
            // 
            // tblFadeOut
            // 
            tblFadeOut.ColumnCount = 3;
            tblFadeOut.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.21127F));
            tblFadeOut.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.30127F));
            tblFadeOut.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.62093F));
            tblFadeOut.Controls.Add(btnCalcEndBPM, 2, 3);
            tblFadeOut.Controls.Add(label19, 0, 6);
            tblFadeOut.Controls.Add(cmbFadeOutLoopCount, 1, 2);
            tblFadeOut.Controls.Add(lblFadeOutLoopCount, 0, 2);
            tblFadeOut.Controls.Add(lblFadeOutStartPosition, 0, 0);
            tblFadeOut.Controls.Add(lblFadeOutLength, 0, 1);
            tblFadeOut.Controls.Add(txtFadeOutStartPosition, 1, 0);
            tblFadeOut.Controls.Add(lblPowerDown, 0, 3);
            tblFadeOut.Controls.Add(chkPowerDown, 1, 3);
            tblFadeOut.Controls.Add(btnFadeOutUpdate, 2, 0);
            tblFadeOut.Controls.Add(btnZoomFadeOut, 2, 1);
            tblFadeOut.Controls.Add(label15, 0, 4);
            tblFadeOut.Controls.Add(chkUseSkipSection, 1, 4);
            tblFadeOut.Controls.Add(label18, 0, 5);
            tblFadeOut.Controls.Add(txtSkipStart, 1, 5);
            tblFadeOut.Controls.Add(btnSkipUpdate, 2, 5);
            tblFadeOut.Controls.Add(cmbSkipLength, 1, 6);
            tblFadeOut.Controls.Add(btnSkipZoom, 2, 6);
            tblFadeOut.Controls.Add(panel5, 1, 1);
            tblFadeOut.Controls.Add(lblEndBPM, 2, 2);
            tblFadeOut.Dock = System.Windows.Forms.DockStyle.Fill;
            tblFadeOut.Location = new System.Drawing.Point(1, 36);
            tblFadeOut.Margin = new System.Windows.Forms.Padding(6);
            tblFadeOut.Name = "tblFadeOut";
            tblFadeOut.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            tblFadeOut.RowCount = 7;
            tblFadeOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tblFadeOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tblFadeOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tblFadeOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tblFadeOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tblFadeOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tblFadeOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            tblFadeOut.Size = new System.Drawing.Size(868, 463);
            tblFadeOut.TabIndex = 11;
            // 
            // btnCalcEndBPM
            // 
            btnCalcEndBPM.Dock = System.Windows.Forms.DockStyle.Top;
            btnCalcEndBPM.Location = new System.Drawing.Point(663, 204);
            btnCalcEndBPM.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnCalcEndBPM.Name = "btnCalcEndBPM";
            btnCalcEndBPM.Size = new System.Drawing.Size(189, 50);
            btnCalcEndBPM.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnCalcEndBPM.TabIndex = 62;
            btnCalcEndBPM.Text = "Calculate";
            btnCalcEndBPM.Click += btnCalcEndBPM_Click;
            // 
            // label19
            // 
            label19.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label19.ForeColor = System.Drawing.Color.White;
            label19.Location = new System.Drawing.Point(16, 399);
            label19.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label19.Name = "label19";
            label19.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            label19.Size = new System.Drawing.Size(161, 44);
            label19.Style = Common.Windows.Controls.LabelStyle.Caption;
            label19.TabIndex = 58;
            label19.Text = "  Length:";
            // 
            // cmbFadeOutLoopCount
            // 
            cmbFadeOutLoopCount.DropDownWidth = 79;
            cmbFadeOutLoopCount.EntryType = Common.Windows.Controls.ComboBox.TextEntryType.Integer;
            cmbFadeOutLoopCount.ErrorProvider = null;
            cmbFadeOutLoopCount.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "10", "12", "14", "16", "32", "64" });
            cmbFadeOutLoopCount.Location = new System.Drawing.Point(314, 145);
            cmbFadeOutLoopCount.Margin = new System.Windows.Forms.Padding(6);
            cmbFadeOutLoopCount.MaximumValue = 0;
            cmbFadeOutLoopCount.MaxLength = 3;
            cmbFadeOutLoopCount.MinimumValue = 100;
            cmbFadeOutLoopCount.Name = "cmbFadeOutLoopCount";
            cmbFadeOutLoopCount.Size = new System.Drawing.Size(183, 33);
            cmbFadeOutLoopCount.StateCommon.ComboBox.Back.Color1 = System.Drawing.Color.LemonChiffon;
            cmbFadeOutLoopCount.TabIndex = 44;
            cmbFadeOutLoopCount.Text = "0";
            cmbFadeOutLoopCount.SelectedIndexChanged += cmbFadeOutLoopCount_SelectedIndexChanged;
            // 
            // lblFadeOutLoopCount
            // 
            lblFadeOutLoopCount.Dock = System.Windows.Forms.DockStyle.Top;
            lblFadeOutLoopCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblFadeOutLoopCount.ForeColor = System.Drawing.Color.White;
            lblFadeOutLoopCount.Location = new System.Drawing.Point(16, 139);
            lblFadeOutLoopCount.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblFadeOutLoopCount.Name = "lblFadeOutLoopCount";
            lblFadeOutLoopCount.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            lblFadeOutLoopCount.Size = new System.Drawing.Size(286, 44);
            lblFadeOutLoopCount.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblFadeOutLoopCount.TabIndex = 43;
            lblFadeOutLoopCount.Text = "Loop Count:";
            // 
            // lblFadeOutStartPosition
            // 
            lblFadeOutStartPosition.Dock = System.Windows.Forms.DockStyle.Top;
            lblFadeOutStartPosition.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblFadeOutStartPosition.ForeColor = System.Drawing.Color.White;
            lblFadeOutStartPosition.Location = new System.Drawing.Point(16, 9);
            lblFadeOutStartPosition.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblFadeOutStartPosition.Name = "lblFadeOutStartPosition";
            lblFadeOutStartPosition.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            lblFadeOutStartPosition.Size = new System.Drawing.Size(286, 44);
            lblFadeOutStartPosition.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblFadeOutStartPosition.TabIndex = 0;
            lblFadeOutStartPosition.Text = "Fade-Out Start:";
            // 
            // lblFadeOutLength
            // 
            lblFadeOutLength.Dock = System.Windows.Forms.DockStyle.Top;
            lblFadeOutLength.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblFadeOutLength.ForeColor = System.Drawing.Color.White;
            lblFadeOutLength.Location = new System.Drawing.Point(16, 74);
            lblFadeOutLength.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblFadeOutLength.Name = "lblFadeOutLength";
            lblFadeOutLength.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            lblFadeOutLength.Size = new System.Drawing.Size(286, 44);
            lblFadeOutLength.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblFadeOutLength.TabIndex = 21;
            lblFadeOutLength.Text = "Fade Length:";
            // 
            // txtFadeOutStartPosition
            // 
            txtFadeOutStartPosition.Location = new System.Drawing.Point(314, 15);
            txtFadeOutStartPosition.Margin = new System.Windows.Forms.Padding(6);
            txtFadeOutStartPosition.Name = "txtFadeOutStartPosition";
            txtFadeOutStartPosition.Seconds = 0D;
            txtFadeOutStartPosition.Size = new System.Drawing.Size(183, 37);
            txtFadeOutStartPosition.TabIndex = 0;
            txtFadeOutStartPosition.Text = "00:00.0000";
            // 
            // lblPowerDown
            // 
            lblPowerDown.Dock = System.Windows.Forms.DockStyle.Top;
            lblPowerDown.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblPowerDown.ForeColor = System.Drawing.Color.White;
            lblPowerDown.Location = new System.Drawing.Point(16, 204);
            lblPowerDown.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblPowerDown.Name = "lblPowerDown";
            lblPowerDown.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            lblPowerDown.Size = new System.Drawing.Size(286, 44);
            lblPowerDown.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblPowerDown.TabIndex = 45;
            lblPowerDown.Text = "Power Down:";
            // 
            // chkPowerDown
            // 
            chkPowerDown.CheckedValue = "0.5";
            chkPowerDown.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            chkPowerDown.Location = new System.Drawing.Point(314, 210);
            chkPowerDown.Margin = new System.Windows.Forms.Padding(6);
            chkPowerDown.Name = "chkPowerDown";
            chkPowerDown.Size = new System.Drawing.Size(145, 33);
            chkPowerDown.TabIndex = 46;
            chkPowerDown.UncheckedValue = "1";
            chkPowerDown.Value = "1";
            chkPowerDown.Values.Text = "Power down";
            // 
            // btnFadeOutUpdate
            // 
            btnFadeOutUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            btnFadeOutUpdate.Location = new System.Drawing.Point(663, 9);
            btnFadeOutUpdate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnFadeOutUpdate.Name = "btnFadeOutUpdate";
            btnFadeOutUpdate.Size = new System.Drawing.Size(189, 59);
            btnFadeOutUpdate.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnFadeOutUpdate.TabIndex = 50;
            btnFadeOutUpdate.Text = "Update";
            btnFadeOutUpdate.Click += btnFadeOutUpdate_Click;
            // 
            // btnZoomFadeOut
            // 
            btnZoomFadeOut.Dock = System.Windows.Forms.DockStyle.Top;
            btnZoomFadeOut.Location = new System.Drawing.Point(663, 74);
            btnZoomFadeOut.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnZoomFadeOut.Name = "btnZoomFadeOut";
            btnZoomFadeOut.Size = new System.Drawing.Size(189, 56);
            btnZoomFadeOut.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnZoomFadeOut.TabIndex = 52;
            btnZoomFadeOut.Text = "Zoom";
            btnZoomFadeOut.Click += btnZoomFadeOut_Click;
            // 
            // label15
            // 
            label15.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label15.ForeColor = System.Drawing.Color.White;
            label15.Location = new System.Drawing.Point(16, 269);
            label15.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label15.Name = "label15";
            label15.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            label15.Size = new System.Drawing.Size(161, 44);
            label15.Style = Common.Windows.Controls.LabelStyle.Caption;
            label15.TabIndex = 53;
            label15.Text = "Skip Section:";
            // 
            // chkUseSkipSection
            // 
            chkUseSkipSection.CheckedValue = "0.5";
            tblFadeOut.SetColumnSpan(chkUseSkipSection, 2);
            chkUseSkipSection.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            chkUseSkipSection.Location = new System.Drawing.Point(314, 275);
            chkUseSkipSection.Margin = new System.Windows.Forms.Padding(6);
            chkUseSkipSection.Name = "chkUseSkipSection";
            chkUseSkipSection.Size = new System.Drawing.Size(180, 33);
            chkUseSkipSection.TabIndex = 54;
            chkUseSkipSection.UncheckedValue = "1";
            chkUseSkipSection.Value = "1";
            chkUseSkipSection.Values.Text = "Use skip section";
            chkUseSkipSection.CheckedChanged += chkUseSkipSection_CheckedChanged;
            // 
            // label18
            // 
            label18.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label18.ForeColor = System.Drawing.Color.White;
            label18.Location = new System.Drawing.Point(16, 334);
            label18.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label18.Name = "label18";
            label18.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            label18.Size = new System.Drawing.Size(161, 44);
            label18.Style = Common.Windows.Controls.LabelStyle.Caption;
            label18.TabIndex = 55;
            label18.Text = "  Start:";
            // 
            // txtSkipStart
            // 
            txtSkipStart.Location = new System.Drawing.Point(314, 340);
            txtSkipStart.Margin = new System.Windows.Forms.Padding(6);
            txtSkipStart.Name = "txtSkipStart";
            txtSkipStart.Seconds = 0D;
            txtSkipStart.Size = new System.Drawing.Size(183, 37);
            txtSkipStart.TabIndex = 56;
            txtSkipStart.Text = "00:00.0000";
            txtSkipStart.TextChanged += txtSkipStart_TextChanged;
            // 
            // btnSkipUpdate
            // 
            btnSkipUpdate.Dock = System.Windows.Forms.DockStyle.Top;
            btnSkipUpdate.Location = new System.Drawing.Point(663, 334);
            btnSkipUpdate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnSkipUpdate.Name = "btnSkipUpdate";
            btnSkipUpdate.Size = new System.Drawing.Size(189, 39);
            btnSkipUpdate.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnSkipUpdate.TabIndex = 57;
            btnSkipUpdate.Text = "Update";
            btnSkipUpdate.Click += btnSkipUpdate_Click;
            // 
            // cmbSkipLength
            // 
            cmbSkipLength.DropDownWidth = 79;
            cmbSkipLength.Location = new System.Drawing.Point(314, 405);
            cmbSkipLength.Margin = new System.Windows.Forms.Padding(6);
            cmbSkipLength.Name = "cmbSkipLength";
            cmbSkipLength.Seconds = 0D;
            cmbSkipLength.Size = new System.Drawing.Size(183, 33);
            cmbSkipLength.TabIndex = 59;
            cmbSkipLength.Text = "00:00.0000";
            cmbSkipLength.SelectedIndexChanged += cmbSkipLength_SelectedIndexChanged;
            cmbSkipLength.TextChanged += cmbSkipLength_TextChanged;
            // 
            // btnSkipZoom
            // 
            btnSkipZoom.AutoScroll = true;
            btnSkipZoom.Dock = System.Windows.Forms.DockStyle.Top;
            btnSkipZoom.Location = new System.Drawing.Point(663, 399);
            btnSkipZoom.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnSkipZoom.Name = "btnSkipZoom";
            btnSkipZoom.Size = new System.Drawing.Size(189, 46);
            btnSkipZoom.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnSkipZoom.TabIndex = 60;
            btnSkipZoom.Text = "Zoom";
            btnSkipZoom.Click += btnSkipZoom_Click;
            // 
            // panel5
            // 
            panel5.Controls.Add(btnCopyRight);
            panel5.Controls.Add(cmbCustomFadeOutLength);
            panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            panel5.Location = new System.Drawing.Point(308, 74);
            panel5.Margin = new System.Windows.Forms.Padding(0);
            panel5.Name = "panel5";
            panel5.Size = new System.Drawing.Size(349, 65);
            panel5.TabIndex = 61;
            // 
            // btnCopyRight
            // 
            btnCopyRight.Location = new System.Drawing.Point(199, 4);
            btnCopyRight.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnCopyRight.Name = "btnCopyRight";
            btnCopyRight.Size = new System.Drawing.Size(66, 50);
            btnCopyRight.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnCopyRight.TabIndex = 54;
            btnCopyRight.Text = "<";
            btnCopyRight.Click += btnCopyRight_Click;
            // 
            // cmbCustomFadeOutLength
            // 
            cmbCustomFadeOutLength.DropDownWidth = 79;
            cmbCustomFadeOutLength.Location = new System.Drawing.Point(6, 0);
            cmbCustomFadeOutLength.Margin = new System.Windows.Forms.Padding(6);
            cmbCustomFadeOutLength.Name = "cmbCustomFadeOutLength";
            cmbCustomFadeOutLength.Seconds = 0D;
            cmbCustomFadeOutLength.Size = new System.Drawing.Size(183, 33);
            cmbCustomFadeOutLength.TabIndex = 1;
            cmbCustomFadeOutLength.Text = "00:00:0000";
            cmbCustomFadeOutLength.SelectedIndexChanged += cmbCustomFadeOutLength_SelectedIndexChanged;
            cmbCustomFadeOutLength.TextChanged += cmbCustomFadeOutLength_TextChanged;
            cmbCustomFadeOutLength.Leave += cmbCustomFadeOutLength_Leave;
            // 
            // lblEndBPM
            // 
            lblEndBPM.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblEndBPM.ForeColor = System.Drawing.Color.White;
            lblEndBPM.Location = new System.Drawing.Point(663, 139);
            lblEndBPM.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblEndBPM.Name = "lblEndBPM";
            lblEndBPM.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            lblEndBPM.Size = new System.Drawing.Size(133, 44);
            lblEndBPM.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblEndBPM.TabIndex = 49;
            lblEndBPM.Text = "100BPM";
            // 
            // hdrFadeOut
            // 
            hdrFadeOut.Dock = System.Windows.Forms.DockStyle.Top;
            hdrFadeOut.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            hdrFadeOut.Location = new System.Drawing.Point(1, 2);
            hdrFadeOut.Margin = new System.Windows.Forms.Padding(6);
            hdrFadeOut.Name = "hdrFadeOut";
            hdrFadeOut.Size = new System.Drawing.Size(868, 34);
            hdrFadeOut.StateCommon.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            hdrFadeOut.TabIndex = 0;
            hdrFadeOut.Values.Description = "";
            hdrFadeOut.Values.Heading = "Fade Out";
            hdrFadeOut.Values.Image = null;
            // 
            // pnlFadeIn
            // 
            pnlFadeIn.BackColor = System.Drawing.SystemColors.Control;
            pnlFadeIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlFadeIn.Controls.Add(tblFadeIn);
            pnlFadeIn.Controls.Add(hdrFadeIn);
            pnlFadeIn.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlFadeIn.Location = new System.Drawing.Point(6, 187);
            pnlFadeIn.Margin = new System.Windows.Forms.Padding(6);
            pnlFadeIn.Name = "pnlFadeIn";
            pnlFadeIn.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            pnlFadeIn.Size = new System.Drawing.Size(870, 501);
            pnlFadeIn.Style = Common.Windows.Controls.PanelStyle.Content;
            pnlFadeIn.TabIndex = 1;
            // 
            // tblFadeIn
            // 
            tblFadeIn.ColumnCount = 3;
            tblFadeIn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tblFadeIn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.28147F));
            tblFadeIn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.47949F));
            tblFadeIn.Controls.Add(btnCalcStartBPM, 2, 3);
            tblFadeIn.Controls.Add(btnPreFadeInUpdate, 2, 4);
            tblFadeIn.Controls.Add(lblFadeInLoopCount, 0, 2);
            tblFadeIn.Controls.Add(lblPreFadeIn, 0, 3);
            tblFadeIn.Controls.Add(lblFadeInPosition, 0, 0);
            tblFadeIn.Controls.Add(txtFadeInPosition, 1, 0);
            tblFadeIn.Controls.Add(txtPreFadeInStartPosition, 1, 4);
            tblFadeIn.Controls.Add(lblPreFadeInStartPosition, 0, 4);
            tblFadeIn.Controls.Add(lblPreFadeInStartVolume, 0, 5);
            tblFadeIn.Controls.Add(lblFadeInLength, 0, 1);
            tblFadeIn.Controls.Add(chkUsePreFadeIn, 1, 3);
            tblFadeIn.Controls.Add(cmbFadeInLoopCount, 1, 2);
            tblFadeIn.Controls.Add(cmbPreFadeInStartVolume, 1, 5);
            tblFadeIn.Controls.Add(btnFadeInUpdate, 2, 0);
            tblFadeIn.Controls.Add(lblStartBPM, 2, 2);
            tblFadeIn.Controls.Add(btnZoomFadeIn, 2, 1);
            tblFadeIn.Controls.Add(btnZoomPreFade, 2, 5);
            tblFadeIn.Controls.Add(panel2, 1, 1);
            tblFadeIn.Dock = System.Windows.Forms.DockStyle.Fill;
            tblFadeIn.Location = new System.Drawing.Point(1, 36);
            tblFadeIn.Margin = new System.Windows.Forms.Padding(6);
            tblFadeIn.Name = "tblFadeIn";
            tblFadeIn.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            tblFadeIn.RowCount = 6;
            tblFadeIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tblFadeIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tblFadeIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tblFadeIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tblFadeIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tblFadeIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tblFadeIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            tblFadeIn.Size = new System.Drawing.Size(868, 463);
            tblFadeIn.TabIndex = 11;
            // 
            // btnCalcStartBPM
            // 
            btnCalcStartBPM.Dock = System.Windows.Forms.DockStyle.Top;
            btnCalcStartBPM.Location = new System.Drawing.Point(664, 231);
            btnCalcStartBPM.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnCalcStartBPM.Name = "btnCalcStartBPM";
            btnCalcStartBPM.Size = new System.Drawing.Size(188, 50);
            btnCalcStartBPM.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnCalcStartBPM.TabIndex = 56;
            btnCalcStartBPM.Text = "Calculate";
            btnCalcStartBPM.Click += btnCalcStartBPM_Click;
            // 
            // btnPreFadeInUpdate
            // 
            btnPreFadeInUpdate.Dock = System.Windows.Forms.DockStyle.Top;
            btnPreFadeInUpdate.Location = new System.Drawing.Point(664, 305);
            btnPreFadeInUpdate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnPreFadeInUpdate.Name = "btnPreFadeInUpdate";
            btnPreFadeInUpdate.Size = new System.Drawing.Size(188, 48);
            btnPreFadeInUpdate.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnPreFadeInUpdate.TabIndex = 54;
            btnPreFadeInUpdate.Text = "Update";
            btnPreFadeInUpdate.Click += btnPreFadeInUpdate_Click;
            // 
            // lblFadeInLoopCount
            // 
            lblFadeInLoopCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblFadeInLoopCount.ForeColor = System.Drawing.Color.White;
            lblFadeInLoopCount.Location = new System.Drawing.Point(16, 157);
            lblFadeInLoopCount.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblFadeInLoopCount.Name = "lblFadeInLoopCount";
            lblFadeInLoopCount.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            lblFadeInLoopCount.Size = new System.Drawing.Size(161, 44);
            lblFadeInLoopCount.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblFadeInLoopCount.TabIndex = 43;
            lblFadeInLoopCount.Text = "Loop Count:";
            // 
            // lblPreFadeIn
            // 
            lblPreFadeIn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblPreFadeIn.ForeColor = System.Drawing.Color.White;
            lblPreFadeIn.Location = new System.Drawing.Point(16, 231);
            lblPreFadeIn.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblPreFadeIn.Name = "lblPreFadeIn";
            lblPreFadeIn.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            lblPreFadeIn.Size = new System.Drawing.Size(161, 44);
            lblPreFadeIn.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblPreFadeIn.TabIndex = 42;
            lblPreFadeIn.Text = "Pre-Fade-In:";
            // 
            // lblFadeInPosition
            // 
            lblFadeInPosition.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblFadeInPosition.ForeColor = System.Drawing.Color.White;
            lblFadeInPosition.Location = new System.Drawing.Point(16, 9);
            lblFadeInPosition.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblFadeInPosition.Name = "lblFadeInPosition";
            lblFadeInPosition.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            lblFadeInPosition.Size = new System.Drawing.Size(161, 44);
            lblFadeInPosition.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblFadeInPosition.TabIndex = 16;
            lblFadeInPosition.Text = "Fade-In Start:";
            // 
            // txtFadeInPosition
            // 
            txtFadeInPosition.Location = new System.Drawing.Point(298, 15);
            txtFadeInPosition.Margin = new System.Windows.Forms.Padding(6);
            txtFadeInPosition.Name = "txtFadeInPosition";
            txtFadeInPosition.Seconds = 0D;
            txtFadeInPosition.Size = new System.Drawing.Size(199, 37);
            txtFadeInPosition.TabIndex = 0;
            txtFadeInPosition.Text = "00:00.0000";
            // 
            // txtPreFadeInStartPosition
            // 
            txtPreFadeInStartPosition.Location = new System.Drawing.Point(298, 311);
            txtPreFadeInStartPosition.Margin = new System.Windows.Forms.Padding(6);
            txtPreFadeInStartPosition.Name = "txtPreFadeInStartPosition";
            txtPreFadeInStartPosition.Seconds = 0D;
            txtPreFadeInStartPosition.Size = new System.Drawing.Size(199, 37);
            txtPreFadeInStartPosition.TabIndex = 6;
            txtPreFadeInStartPosition.Text = "00:00.0000";
            // 
            // lblPreFadeInStartPosition
            // 
            lblPreFadeInStartPosition.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblPreFadeInStartPosition.ForeColor = System.Drawing.Color.White;
            lblPreFadeInStartPosition.Location = new System.Drawing.Point(16, 305);
            lblPreFadeInStartPosition.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblPreFadeInStartPosition.Name = "lblPreFadeInStartPosition";
            lblPreFadeInStartPosition.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            lblPreFadeInStartPosition.Size = new System.Drawing.Size(161, 44);
            lblPreFadeInStartPosition.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblPreFadeInStartPosition.TabIndex = 39;
            lblPreFadeInStartPosition.Text = "  Start:";
            // 
            // lblPreFadeInStartVolume
            // 
            lblPreFadeInStartVolume.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblPreFadeInStartVolume.ForeColor = System.Drawing.Color.White;
            lblPreFadeInStartVolume.Location = new System.Drawing.Point(16, 379);
            lblPreFadeInStartVolume.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblPreFadeInStartVolume.Name = "lblPreFadeInStartVolume";
            lblPreFadeInStartVolume.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            lblPreFadeInStartVolume.Size = new System.Drawing.Size(161, 48);
            lblPreFadeInStartVolume.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblPreFadeInStartVolume.TabIndex = 37;
            lblPreFadeInStartVolume.Text = "  Vol:";
            // 
            // lblFadeInLength
            // 
            lblFadeInLength.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblFadeInLength.ForeColor = System.Drawing.Color.White;
            lblFadeInLength.Location = new System.Drawing.Point(16, 83);
            lblFadeInLength.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblFadeInLength.Name = "lblFadeInLength";
            lblFadeInLength.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            lblFadeInLength.Size = new System.Drawing.Size(161, 44);
            lblFadeInLength.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblFadeInLength.TabIndex = 21;
            lblFadeInLength.Text = "Fade Length:";
            // 
            // chkUsePreFadeIn
            // 
            chkUsePreFadeIn.CheckedValue = "0.5";
            chkUsePreFadeIn.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            chkUsePreFadeIn.Location = new System.Drawing.Point(298, 237);
            chkUsePreFadeIn.Margin = new System.Windows.Forms.Padding(6);
            chkUsePreFadeIn.Name = "chkUsePreFadeIn";
            chkUsePreFadeIn.Size = new System.Drawing.Size(176, 33);
            chkUsePreFadeIn.TabIndex = 5;
            chkUsePreFadeIn.UncheckedValue = "1";
            chkUsePreFadeIn.Value = "1";
            chkUsePreFadeIn.Values.Text = "Use pre-fade-in";
            chkUsePreFadeIn.CheckedChanged += chkUsePreFadeIn_CheckedChanged;
            // 
            // cmbFadeInLoopCount
            // 
            cmbFadeInLoopCount.DropDownWidth = 79;
            cmbFadeInLoopCount.EntryType = Common.Windows.Controls.ComboBox.TextEntryType.Integer;
            cmbFadeInLoopCount.ErrorProvider = null;
            cmbFadeInLoopCount.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "10", "12", "14", "16", "32", "64" });
            cmbFadeInLoopCount.Location = new System.Drawing.Point(298, 163);
            cmbFadeInLoopCount.Margin = new System.Windows.Forms.Padding(6);
            cmbFadeInLoopCount.MaximumValue = 0;
            cmbFadeInLoopCount.MaxLength = 3;
            cmbFadeInLoopCount.MinimumValue = 100;
            cmbFadeInLoopCount.Name = "cmbFadeInLoopCount";
            cmbFadeInLoopCount.Size = new System.Drawing.Size(199, 33);
            cmbFadeInLoopCount.StateCommon.ComboBox.Back.Color1 = System.Drawing.Color.LemonChiffon;
            cmbFadeInLoopCount.TabIndex = 44;
            cmbFadeInLoopCount.Text = "0";
            cmbFadeInLoopCount.SelectedIndexChanged += cmbFadeInLoopCount_SelectedIndexChanged;
            // 
            // cmbPreFadeInStartVolume
            // 
            cmbPreFadeInStartVolume.DropDownWidth = 79;
            cmbPreFadeInStartVolume.EntryType = Common.Windows.Controls.ComboBox.TextEntryType.Integer;
            cmbPreFadeInStartVolume.ErrorProvider = null;
            cmbPreFadeInStartVolume.Location = new System.Drawing.Point(298, 385);
            cmbPreFadeInStartVolume.Margin = new System.Windows.Forms.Padding(6);
            cmbPreFadeInStartVolume.MaximumValue = 0;
            cmbPreFadeInStartVolume.MaxLength = 3;
            cmbPreFadeInStartVolume.MinimumValue = 100;
            cmbPreFadeInStartVolume.Name = "cmbPreFadeInStartVolume";
            cmbPreFadeInStartVolume.Size = new System.Drawing.Size(199, 33);
            cmbPreFadeInStartVolume.TabIndex = 7;
            // 
            // btnFadeInUpdate
            // 
            btnFadeInUpdate.Location = new System.Drawing.Point(664, 9);
            btnFadeInUpdate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnFadeInUpdate.Name = "btnFadeInUpdate";
            btnFadeInUpdate.Size = new System.Drawing.Size(186, 54);
            btnFadeInUpdate.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnFadeInUpdate.TabIndex = 46;
            btnFadeInUpdate.Text = "Update";
            btnFadeInUpdate.Click += btnFadeInUpdate_Click;
            // 
            // lblStartBPM
            // 
            lblStartBPM.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblStartBPM.ForeColor = System.Drawing.Color.White;
            lblStartBPM.Location = new System.Drawing.Point(664, 157);
            lblStartBPM.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblStartBPM.Name = "lblStartBPM";
            lblStartBPM.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            lblStartBPM.Size = new System.Drawing.Size(128, 44);
            lblStartBPM.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblStartBPM.TabIndex = 45;
            lblStartBPM.Text = "100BPM";
            // 
            // btnZoomFadeIn
            // 
            btnZoomFadeIn.Dock = System.Windows.Forms.DockStyle.Top;
            btnZoomFadeIn.Location = new System.Drawing.Point(664, 83);
            btnZoomFadeIn.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnZoomFadeIn.Name = "btnZoomFadeIn";
            btnZoomFadeIn.Size = new System.Drawing.Size(188, 50);
            btnZoomFadeIn.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnZoomFadeIn.TabIndex = 52;
            btnZoomFadeIn.Text = "Zoom";
            btnZoomFadeIn.Click += btnZoomFadeIn_Click;
            // 
            // btnZoomPreFade
            // 
            btnZoomPreFade.Dock = System.Windows.Forms.DockStyle.Top;
            btnZoomPreFade.Location = new System.Drawing.Point(664, 379);
            btnZoomPreFade.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnZoomPreFade.Name = "btnZoomPreFade";
            btnZoomPreFade.Size = new System.Drawing.Size(188, 52);
            btnZoomPreFade.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnZoomPreFade.TabIndex = 53;
            btnZoomPreFade.Text = "Zoom";
            btnZoomPreFade.Click += btnZoomPreFade_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnCopyLeft);
            panel2.Controls.Add(cmbCustomFadeInLength);
            panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(292, 83);
            panel2.Margin = new System.Windows.Forms.Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(366, 74);
            panel2.TabIndex = 55;
            // 
            // btnCopyLeft
            // 
            btnCopyLeft.Location = new System.Drawing.Point(210, 6);
            btnCopyLeft.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            btnCopyLeft.Name = "btnCopyLeft";
            btnCopyLeft.Size = new System.Drawing.Size(66, 50);
            btnCopyLeft.Style = Common.Windows.Controls.ButtonStyle.Secondary;
            btnCopyLeft.TabIndex = 53;
            btnCopyLeft.Text = ">";
            btnCopyLeft.Click += btnCopyLeft_Click;
            // 
            // cmbCustomFadeInLength
            // 
            cmbCustomFadeInLength.DropDownWidth = 79;
            cmbCustomFadeInLength.Location = new System.Drawing.Point(6, 6);
            cmbCustomFadeInLength.Margin = new System.Windows.Forms.Padding(0);
            cmbCustomFadeInLength.Name = "cmbCustomFadeInLength";
            cmbCustomFadeInLength.Seconds = 0D;
            cmbCustomFadeInLength.Size = new System.Drawing.Size(199, 33);
            cmbCustomFadeInLength.TabIndex = 1;
            cmbCustomFadeInLength.Text = "00:00.0000";
            cmbCustomFadeInLength.SelectedIndexChanged += cmbCustomFadeInLength_SelectedIndexChanged;
            cmbCustomFadeInLength.TextChanged += cmbCustomFadeInLength_TextChanged;
            cmbCustomFadeInLength.Leave += cmbCustomFadeInLength_Leave;
            // 
            // hdrFadeIn
            // 
            hdrFadeIn.Dock = System.Windows.Forms.DockStyle.Top;
            hdrFadeIn.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            hdrFadeIn.Location = new System.Drawing.Point(1, 2);
            hdrFadeIn.Margin = new System.Windows.Forms.Padding(6);
            hdrFadeIn.Name = "hdrFadeIn";
            hdrFadeIn.Size = new System.Drawing.Size(868, 34);
            hdrFadeIn.StateCommon.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            hdrFadeIn.TabIndex = 0;
            hdrFadeIn.Values.Description = "";
            hdrFadeIn.Values.Heading = "Fade In";
            hdrFadeIn.Values.Image = null;
            // 
            // trackWave
            // 
            tblMain.SetColumnSpan(trackWave, 3);
            trackWave.Dock = System.Windows.Forms.DockStyle.Fill;
            trackWave.Location = new System.Drawing.Point(7, 8);
            trackWave.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            trackWave.Name = "trackWave";
            trackWave.Size = new System.Drawing.Size(2634, 165);
            trackWave.TabIndex = 45;
            // 
            // pnlTrackRank
            // 
            tblMain.SetColumnSpan(pnlTrackRank, 3);
            pnlTrackRank.Controls.Add(lblTrackRank);
            pnlTrackRank.Controls.Add(cmbTrackRank);
            pnlTrackRank.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlTrackRank.Location = new System.Drawing.Point(4, 865);
            pnlTrackRank.Margin = new System.Windows.Forms.Padding(4);
            pnlTrackRank.Name = "pnlTrackRank";
            pnlTrackRank.Padding = new System.Windows.Forms.Padding(15, 0, 6, 0);
            pnlTrackRank.Size = new System.Drawing.Size(2640, 61);
            pnlTrackRank.TabIndex = 50;
            pnlTrackRank.WrapContents = false;
            // 
            // lblTrackRank
            // 
            lblTrackRank.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblTrackRank.ForeColor = System.Drawing.Color.White;
            lblTrackRank.Location = new System.Drawing.Point(15, 10);
            lblTrackRank.Margin = new System.Windows.Forms.Padding(0, 10, 11, 0);
            lblTrackRank.Name = "lblTrackRank";
            lblTrackRank.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            lblTrackRank.Size = new System.Drawing.Size(245, 38);
            lblTrackRank.Style = Common.Windows.Controls.LabelStyle.Caption;
            lblTrackRank.TabIndex = 0;
            lblTrackRank.Text = "Track Rank:";
            // 
            // cmbTrackRank
            // 
            cmbTrackRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbTrackRank.DropDownWidth = 220;
            cmbTrackRank.ErrorProvider = null;
            cmbTrackRank.Location = new System.Drawing.Point(271, 6);
            cmbTrackRank.Margin = new System.Windows.Forms.Padding(0, 6, 6, 6);
            cmbTrackRank.Name = "cmbTrackRank";
            cmbTrackRank.Size = new System.Drawing.Size(220, 33);
            cmbTrackRank.TabIndex = 1;
            // 
            // linLine
            // 
            linLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            linLine.Location = new System.Drawing.Point(0, 948);
            linLine.Margin = new System.Windows.Forms.Padding(6);
            linLine.Name = "linLine";
            linLine.Size = new System.Drawing.Size(2668, 2);
            linLine.TabIndex = 0;
            // 
            // flpButtonsRight
            // 
            flpButtonsRight.BackColor = System.Drawing.Color.Transparent;
            flpButtonsRight.Controls.Add(btnCancel);
            flpButtonsRight.Controls.Add(btnOK);
            flpButtonsRight.Dock = System.Windows.Forms.DockStyle.Right;
            flpButtonsRight.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flpButtonsRight.Location = new System.Drawing.Point(2074, 2);
            flpButtonsRight.Margin = new System.Windows.Forms.Padding(6);
            flpButtonsRight.Name = "flpButtonsRight";
            flpButtonsRight.Padding = new System.Windows.Forms.Padding(10, 9, 0, 9);
            flpButtonsRight.Size = new System.Drawing.Size(593, 76);
            flpButtonsRight.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(411, 17);
            btnCancel.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(165, 46);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnOK
            // 
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(232, 17);
            btnOK.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(165, 46);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.Click += btnOK_Click;
            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            pnlButtons.Controls.Add(flowLayoutPanel1);
            pnlButtons.Controls.Add(flpButtonsLeft);
            pnlButtons.Controls.Add(flpButtonsRight);
            pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlButtons.Location = new System.Drawing.Point(0, 950);
            pnlButtons.Margin = new System.Windows.Forms.Padding(6);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            pnlButtons.Size = new System.Drawing.Size(2668, 80);
            pnlButtons.Style = Common.Windows.Controls.PanelStyle.ButtonStrip;
            pnlButtons.TabIndex = 24;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            flowLayoutPanel1.Location = new System.Drawing.Point(407, 2);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10, 9, 0, 9);
            flowLayoutPanel1.Size = new System.Drawing.Size(395, 76);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // flpButtonsLeft
            // 
            flpButtonsLeft.Controls.Add(label6);
            flpButtonsLeft.Controls.Add(cmbOutput);
            flpButtonsLeft.Dock = System.Windows.Forms.DockStyle.Left;
            flpButtonsLeft.Location = new System.Drawing.Point(1, 2);
            flpButtonsLeft.Margin = new System.Windows.Forms.Padding(6);
            flpButtonsLeft.Name = "flpButtonsLeft";
            flpButtonsLeft.Padding = new System.Windows.Forms.Padding(18);
            flpButtonsLeft.Size = new System.Drawing.Size(406, 76);
            flpButtonsLeft.TabIndex = 1;
            // 
            // label6
            // 
            label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label6.ForeColor = System.Drawing.SystemColors.ControlText;
            label6.Location = new System.Drawing.Point(24, 18);
            label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label6.Name = "label6";
            label6.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            label6.Size = new System.Drawing.Size(98, 50);
            label6.Style = Common.Windows.Controls.LabelStyle.Caption;
            label6.TabIndex = 7;
            label6.Text = "Output:";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbOutput
            // 
            cmbOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbOutput.DropDownWidth = 72;
            cmbOutput.ErrorProvider = null;
            cmbOutput.Items.AddRange(new object[] { "Speakers", "Monitor", "Both" });
            cmbOutput.Location = new System.Drawing.Point(134, 24);
            cmbOutput.Margin = new System.Windows.Forms.Padding(6);
            cmbOutput.Name = "cmbOutput";
            cmbOutput.Size = new System.Drawing.Size(132, 33);
            cmbOutput.TabIndex = 8;
            cmbOutput.SelectedIndexChanged += cmbOutput_SelectedIndexChanged;
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.SystemColors.Control;
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(200, 100);
            panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.08772F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.19298F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.36842F));
            tableLayoutPanel1.Controls.Add(comboBox1, 1, 2);
            tableLayoutPanel1.Controls.Add(label1, 0, 2);
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // comboBox1
            // 
            comboBox1.DropDownWidth = 79;
            comboBox1.EntryType = Common.Windows.Controls.ComboBox.TextEntryType.Integer;
            comboBox1.ErrorProvider = null;
            comboBox1.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "10", "12", "14", "16", "32", "64" });
            comboBox1.Location = new System.Drawing.Point(73, 43);
            comboBox1.MaximumValue = 0;
            comboBox1.MaxLength = 3;
            comboBox1.MinimumValue = 100;
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(68, 33);
            comboBox1.StateCommon.ComboBox.Back.Color1 = System.Drawing.Color.LemonChiffon;
            comboBox1.TabIndex = 44;
            comboBox1.Text = "0";
            // 
            // label1
            // 
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(3, 40);
            label1.Name = "label1";
            label1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            label1.Size = new System.Drawing.Size(64, 24);
            label1.Style = Common.Windows.Controls.LabelStyle.Caption;
            label1.TabIndex = 43;
            label1.Text = "Loop Count:";
            // 
            // label2
            // 
            label2.Dock = System.Windows.Forms.DockStyle.Top;
            label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.ForeColor = System.Drawing.Color.White;
            label2.Location = new System.Drawing.Point(8, 5);
            label2.Name = "label2";
            label2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            label2.Size = new System.Drawing.Size(94, 24);
            label2.Style = Common.Windows.Controls.LabelStyle.Caption;
            label2.TabIndex = 0;
            label2.Text = "Fade-Out Start:";
            // 
            // FrmShufflerDetails
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(24, 32, 48);
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(2668, 1030);
            Controls.Add(pnlMain);
            Controls.Add(linLine);
            Controls.Add(pnlButtons);
            Margin = new System.Windows.Forms.Padding(6);
            Name = "FrmShufflerDetails";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Halloumi : Shuffler : Track";
            UseApplicationIcon = true;
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            FormClosed += frmTrack_FormClosed;
            Load += frmShufflerDetails_Load;
            pnlMain.ResumeLayout(false);
            tblMain.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            flpTrackFX.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)cmbTrackFX).EndInit();
            flpRight.ResumeLayout(false);
            flpRight.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cmbSampleLength).EndInit();
            pnlFadeOut.ResumeLayout(false);
            pnlFadeOut.PerformLayout();
            tblFadeOut.ResumeLayout(false);
            tblFadeOut.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cmbFadeOutLoopCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)cmbSkipLength).EndInit();
            panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)cmbCustomFadeOutLength).EndInit();
            pnlFadeIn.ResumeLayout(false);
            pnlFadeIn.PerformLayout();
            tblFadeIn.ResumeLayout(false);
            tblFadeIn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cmbFadeInLoopCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)cmbPreFadeInStartVolume).EndInit();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)cmbCustomFadeInLength).EndInit();
            pnlTrackRank.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)cmbTrackRank).EndInit();
            flpButtonsRight.ResumeLayout(false);
            pnlButtons.ResumeLayout(false);
            flpButtonsLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)cmbOutput).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)comboBox1).EndInit();
            ResumeLayout(false);

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
        private System.Windows.Forms.FlowLayoutPanel pnlTrackRank;
        private Halloumi.Common.Windows.Controls.Label lblTrackRank;
        private Halloumi.Common.Windows.Controls.ComboBox cmbTrackRank;
    }
}