namespace Halloumi.Shuffler.Forms
{
    partial class frmPluginSettings
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
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOK = new Halloumi.Common.Windows.Controls.Button();
            this.pnlButtons = new Halloumi.Common.Windows.Controls.Panel();
            this.linLine = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbMainMixerPlugins2 = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.btnMainMixerPluginConfig2 = new Halloumi.Common.Windows.Controls.Button();
            this.label6 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbTrackFXVSTPlugins2 = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.btnTrackFXVSTPluginConfig2 = new Halloumi.Common.Windows.Controls.Button();
            this.label5 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbTrackFXVSTPlugins = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.btnTrackFXVSTPluginConfig = new Halloumi.Common.Windows.Controls.Button();
            this.cmbSamplerVSTPlugins = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.label2 = new Halloumi.Common.Windows.Controls.Label();
            this.label1 = new Halloumi.Common.Windows.Controls.Label();
            this.title1 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbWAPlugins = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.cmbVSTPlugins = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.btnWAPluginConfig = new Halloumi.Common.Windows.Controls.Button();
            this.btnVSTPluginConfig = new Halloumi.Common.Windows.Controls.Button();
            this.cmbTrackVSTPlugins = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.btnTrackVSTPluginConfig = new Halloumi.Common.Windows.Controls.Button();
            this.btnSamplerVSTPluginConfig = new Halloumi.Common.Windows.Controls.Button();
            this.label3 = new Halloumi.Common.Windows.Controls.Label();
            this.btnSamplerVSTPlugin2Config = new Halloumi.Common.Windows.Controls.Button();
            this.cmbSamplerVSTPlugins2 = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.label4 = new Halloumi.Common.Windows.Controls.Label();
            this.pnlMain = new Halloumi.Common.Windows.Controls.Panel();
            this.btnExport = new Halloumi.Common.Windows.Controls.Button();
            this.btnImport = new Halloumi.Common.Windows.Controls.Button();
            this.flpButtons.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tblMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMainMixerPlugins2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrackFXVSTPlugins2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrackFXVSTPlugins)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSamplerVSTPlugins)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbWAPlugins)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVSTPlugins)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrackVSTPlugins)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSamplerVSTPlugins2)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpButtons
            // 
            this.flpButtons.BackColor = System.Drawing.Color.Transparent;
            this.flpButtons.Controls.Add(this.btnImport);
            this.flpButtons.Controls.Add(this.btnExport);
            this.flpButtons.Controls.Add(this.btnOK);
            this.flpButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpButtons.Location = new System.Drawing.Point(0, 1);
            this.flpButtons.Margin = new System.Windows.Forms.Padding(4);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.flpButtons.Size = new System.Drawing.Size(702, 52);
            this.flpButtons.TabIndex = 16;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(315, 11);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(116, 31);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "Close";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.flpButtons);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 384);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(4);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(702, 53);
            this.pnlButtons.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlButtons.TabIndex = 27;
            // 
            // linLine
            // 
            this.linLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.linLine.Location = new System.Drawing.Point(0, 382);
            this.linLine.Margin = new System.Windows.Forms.Padding(4);
            this.linLine.Name = "linLine";
            this.linLine.Size = new System.Drawing.Size(702, 2);
            this.linLine.TabIndex = 31;
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 3;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.8396F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.1604F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.tblMain.Controls.Add(this.label7, 0, 2);
            this.tblMain.Controls.Add(this.cmbMainMixerPlugins2, 1, 2);
            this.tblMain.Controls.Add(this.btnMainMixerPluginConfig2, 2, 2);
            this.tblMain.Controls.Add(this.label6, 0, 5);
            this.tblMain.Controls.Add(this.cmbTrackFXVSTPlugins2, 1, 5);
            this.tblMain.Controls.Add(this.btnTrackFXVSTPluginConfig2, 2, 5);
            this.tblMain.Controls.Add(this.label5, 0, 4);
            this.tblMain.Controls.Add(this.cmbTrackFXVSTPlugins, 1, 4);
            this.tblMain.Controls.Add(this.btnTrackFXVSTPluginConfig, 2, 4);
            this.tblMain.Controls.Add(this.cmbSamplerVSTPlugins, 1, 6);
            this.tblMain.Controls.Add(this.label2, 0, 6);
            this.tblMain.Controls.Add(this.label1, 0, 1);
            this.tblMain.Controls.Add(this.title1, 0, 0);
            this.tblMain.Controls.Add(this.cmbWAPlugins, 1, 0);
            this.tblMain.Controls.Add(this.cmbVSTPlugins, 1, 1);
            this.tblMain.Controls.Add(this.btnWAPluginConfig, 2, 0);
            this.tblMain.Controls.Add(this.btnVSTPluginConfig, 2, 1);
            this.tblMain.Controls.Add(this.cmbTrackVSTPlugins, 1, 3);
            this.tblMain.Controls.Add(this.btnTrackVSTPluginConfig, 2, 3);
            this.tblMain.Controls.Add(this.btnSamplerVSTPluginConfig, 2, 6);
            this.tblMain.Controls.Add(this.label3, 0, 3);
            this.tblMain.Controls.Add(this.btnSamplerVSTPlugin2Config, 2, 7);
            this.tblMain.Controls.Add(this.cmbSamplerVSTPlugins2, 1, 7);
            this.tblMain.Controls.Add(this.label4, 0, 7);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(13, 12);
            this.tblMain.Margin = new System.Windows.Forms.Padding(4);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 8;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.4999F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.4999F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.50328F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49865F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49918F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.50167F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49824F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49918F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblMain.Size = new System.Drawing.Size(676, 360);
            this.tblMain.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(4, 88);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label7.Size = new System.Drawing.Size(197, 33);
            this.label7.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label7.TabIndex = 35;
            this.label7.Text = "Main Mixer VST Effect #2:";
            // 
            // cmbMainMixerPlugins2
            // 
            this.cmbMainMixerPlugins2.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbMainMixerPlugins2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMainMixerPlugins2.DropDownWidth = 257;
            this.cmbMainMixerPlugins2.Location = new System.Drawing.Point(209, 92);
            this.cmbMainMixerPlugins2.Margin = new System.Windows.Forms.Padding(4);
            this.cmbMainMixerPlugins2.Name = "cmbMainMixerPlugins2";
            this.cmbMainMixerPlugins2.Size = new System.Drawing.Size(360, 25);
            this.cmbMainMixerPlugins2.TabIndex = 34;
            this.cmbMainMixerPlugins2.SelectedIndexChanged += new System.EventHandler(this.cmbMainMixerPlugins2_SelectedIndexChanged);
            // 
            // btnMainMixerPluginConfig2
            // 
            this.btnMainMixerPluginConfig2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMainMixerPluginConfig2.Location = new System.Drawing.Point(578, 94);
            this.btnMainMixerPluginConfig2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnMainMixerPluginConfig2.Name = "btnMainMixerPluginConfig2";
            this.btnMainMixerPluginConfig2.Size = new System.Drawing.Size(73, 31);
            this.btnMainMixerPluginConfig2.TabIndex = 33;
            this.btnMainMixerPluginConfig2.Text = "Config";
            this.btnMainMixerPluginConfig2.Click += new System.EventHandler(this.btnMainMixerPluginConfig2_Click);
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(4, 221);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label6.Size = new System.Drawing.Size(197, 33);
            this.label6.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label6.TabIndex = 32;
            this.label6.Text = "Track FX VST Effect #2:";
            // 
            // cmbTrackFXVSTPlugins2
            // 
            this.cmbTrackFXVSTPlugins2.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbTrackFXVSTPlugins2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTrackFXVSTPlugins2.DropDownWidth = 257;
            this.cmbTrackFXVSTPlugins2.Location = new System.Drawing.Point(209, 225);
            this.cmbTrackFXVSTPlugins2.Margin = new System.Windows.Forms.Padding(4);
            this.cmbTrackFXVSTPlugins2.Name = "cmbTrackFXVSTPlugins2";
            this.cmbTrackFXVSTPlugins2.Size = new System.Drawing.Size(360, 25);
            this.cmbTrackFXVSTPlugins2.TabIndex = 31;
            this.cmbTrackFXVSTPlugins2.SelectedIndexChanged += new System.EventHandler(this.cmbTrackFXVSTPlugins2_SelectedIndexChanged);
            // 
            // btnTrackFXVSTPluginConfig2
            // 
            this.btnTrackFXVSTPluginConfig2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrackFXVSTPluginConfig2.Location = new System.Drawing.Point(578, 227);
            this.btnTrackFXVSTPluginConfig2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnTrackFXVSTPluginConfig2.Name = "btnTrackFXVSTPluginConfig2";
            this.btnTrackFXVSTPluginConfig2.Size = new System.Drawing.Size(73, 31);
            this.btnTrackFXVSTPluginConfig2.TabIndex = 30;
            this.btnTrackFXVSTPluginConfig2.Text = "Config";
            this.btnTrackFXVSTPluginConfig2.Click += new System.EventHandler(this.btnTrackFXVSTPluginConfig2_Click);
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(4, 177);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label5.Size = new System.Drawing.Size(197, 33);
            this.label5.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label5.TabIndex = 29;
            this.label5.Text = "Track FX VST Effect:";
            // 
            // cmbTrackFXVSTPlugins
            // 
            this.cmbTrackFXVSTPlugins.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbTrackFXVSTPlugins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTrackFXVSTPlugins.DropDownWidth = 257;
            this.cmbTrackFXVSTPlugins.Location = new System.Drawing.Point(209, 181);
            this.cmbTrackFXVSTPlugins.Margin = new System.Windows.Forms.Padding(4);
            this.cmbTrackFXVSTPlugins.Name = "cmbTrackFXVSTPlugins";
            this.cmbTrackFXVSTPlugins.Size = new System.Drawing.Size(360, 25);
            this.cmbTrackFXVSTPlugins.TabIndex = 28;
            this.cmbTrackFXVSTPlugins.SelectedIndexChanged += new System.EventHandler(this.cmbTrackFXVSTPlugins_SelectedIndexChanged);
            // 
            // btnTrackFXVSTPluginConfig
            // 
            this.btnTrackFXVSTPluginConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrackFXVSTPluginConfig.Location = new System.Drawing.Point(578, 183);
            this.btnTrackFXVSTPluginConfig.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnTrackFXVSTPluginConfig.Name = "btnTrackFXVSTPluginConfig";
            this.btnTrackFXVSTPluginConfig.Size = new System.Drawing.Size(73, 31);
            this.btnTrackFXVSTPluginConfig.TabIndex = 27;
            this.btnTrackFXVSTPluginConfig.Text = "Config";
            this.btnTrackFXVSTPluginConfig.Click += new System.EventHandler(this.btnTrackFXVSTPluginConfig_Click);
            // 
            // cmbSamplerVSTPlugins
            // 
            this.cmbSamplerVSTPlugins.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbSamplerVSTPlugins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSamplerVSTPlugins.DropDownWidth = 257;
            this.cmbSamplerVSTPlugins.Location = new System.Drawing.Point(209, 270);
            this.cmbSamplerVSTPlugins.Margin = new System.Windows.Forms.Padding(4);
            this.cmbSamplerVSTPlugins.Name = "cmbSamplerVSTPlugins";
            this.cmbSamplerVSTPlugins.Size = new System.Drawing.Size(360, 25);
            this.cmbSamplerVSTPlugins.TabIndex = 19;
            this.cmbSamplerVSTPlugins.SelectedIndexChanged += new System.EventHandler(this.cmbSampleVSTPlugins_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 266);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label2.Size = new System.Drawing.Size(197, 33);
            this.label2.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label2.TabIndex = 18;
            this.label2.Text = "Sampler VST Effect:";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 44);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label1.Size = new System.Drawing.Size(197, 33);
            this.label1.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label1.TabIndex = 12;
            this.label1.Text = "Main Mixer VST Effect:";
            // 
            // title1
            // 
            this.title1.Dock = System.Windows.Forms.DockStyle.Top;
            this.title1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title1.ForeColor = System.Drawing.Color.White;
            this.title1.Location = new System.Drawing.Point(4, 0);
            this.title1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.title1.Name = "title1";
            this.title1.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.title1.Size = new System.Drawing.Size(197, 33);
            this.title1.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.title1.TabIndex = 11;
            this.title1.Text = "Main Mixer DSP Effect:";
            // 
            // cmbWAPlugins
            // 
            this.cmbWAPlugins.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbWAPlugins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWAPlugins.DropDownWidth = 257;
            this.cmbWAPlugins.Location = new System.Drawing.Point(209, 4);
            this.cmbWAPlugins.Margin = new System.Windows.Forms.Padding(4);
            this.cmbWAPlugins.Name = "cmbWAPlugins";
            this.cmbWAPlugins.Size = new System.Drawing.Size(360, 25);
            this.cmbWAPlugins.TabIndex = 13;
            this.cmbWAPlugins.SelectedIndexChanged += new System.EventHandler(this.cmbWAPlugins_SelectedIndexChanged);
            // 
            // cmbVSTPlugins
            // 
            this.cmbVSTPlugins.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbVSTPlugins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVSTPlugins.DropDownWidth = 257;
            this.cmbVSTPlugins.Location = new System.Drawing.Point(209, 48);
            this.cmbVSTPlugins.Margin = new System.Windows.Forms.Padding(4);
            this.cmbVSTPlugins.Name = "cmbVSTPlugins";
            this.cmbVSTPlugins.Size = new System.Drawing.Size(360, 25);
            this.cmbVSTPlugins.TabIndex = 14;
            this.cmbVSTPlugins.SelectedIndexChanged += new System.EventHandler(this.cmbVSTPlugins_SelectedIndexChanged);
            // 
            // btnWAPluginConfig
            // 
            this.btnWAPluginConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWAPluginConfig.Location = new System.Drawing.Point(578, 6);
            this.btnWAPluginConfig.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnWAPluginConfig.Name = "btnWAPluginConfig";
            this.btnWAPluginConfig.Size = new System.Drawing.Size(73, 31);
            this.btnWAPluginConfig.TabIndex = 16;
            this.btnWAPluginConfig.Text = "Config";
            this.btnWAPluginConfig.Click += new System.EventHandler(this.btnWAPluginConfig_Click);
            // 
            // btnVSTPluginConfig
            // 
            this.btnVSTPluginConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVSTPluginConfig.Location = new System.Drawing.Point(578, 50);
            this.btnVSTPluginConfig.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnVSTPluginConfig.Name = "btnVSTPluginConfig";
            this.btnVSTPluginConfig.Size = new System.Drawing.Size(73, 31);
            this.btnVSTPluginConfig.TabIndex = 17;
            this.btnVSTPluginConfig.Text = "Config";
            this.btnVSTPluginConfig.Click += new System.EventHandler(this.btnVSTPluginConfig_Click);
            // 
            // cmbTrackVSTPlugins
            // 
            this.cmbTrackVSTPlugins.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbTrackVSTPlugins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTrackVSTPlugins.DropDownWidth = 257;
            this.cmbTrackVSTPlugins.Location = new System.Drawing.Point(209, 137);
            this.cmbTrackVSTPlugins.Margin = new System.Windows.Forms.Padding(4);
            this.cmbTrackVSTPlugins.Name = "cmbTrackVSTPlugins";
            this.cmbTrackVSTPlugins.Size = new System.Drawing.Size(360, 25);
            this.cmbTrackVSTPlugins.TabIndex = 22;
            this.cmbTrackVSTPlugins.SelectedIndexChanged += new System.EventHandler(this.cmbTrackVSTPlugins_SelectedIndexChanged);
            // 
            // btnTrackVSTPluginConfig
            // 
            this.btnTrackVSTPluginConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrackVSTPluginConfig.Location = new System.Drawing.Point(578, 139);
            this.btnTrackVSTPluginConfig.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnTrackVSTPluginConfig.Name = "btnTrackVSTPluginConfig";
            this.btnTrackVSTPluginConfig.Size = new System.Drawing.Size(73, 31);
            this.btnTrackVSTPluginConfig.TabIndex = 21;
            this.btnTrackVSTPluginConfig.Text = "Config";
            this.btnTrackVSTPluginConfig.Click += new System.EventHandler(this.btnTrackVSTPluginConfig_Click);
            // 
            // btnSamplerVSTPluginConfig
            // 
            this.btnSamplerVSTPluginConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSamplerVSTPluginConfig.Location = new System.Drawing.Point(578, 272);
            this.btnSamplerVSTPluginConfig.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnSamplerVSTPluginConfig.Name = "btnSamplerVSTPluginConfig";
            this.btnSamplerVSTPluginConfig.Size = new System.Drawing.Size(73, 31);
            this.btnSamplerVSTPluginConfig.TabIndex = 20;
            this.btnSamplerVSTPluginConfig.Text = "Config";
            this.btnSamplerVSTPluginConfig.Click += new System.EventHandler(this.btnSamplerVSTPluginConfig_Click);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(4, 133);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label3.Size = new System.Drawing.Size(197, 33);
            this.label3.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label3.TabIndex = 23;
            this.label3.Text = "Track Mixer VST Effect:";
            // 
            // btnSamplerVSTPlugin2Config
            // 
            this.btnSamplerVSTPlugin2Config.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSamplerVSTPlugin2Config.Location = new System.Drawing.Point(578, 316);
            this.btnSamplerVSTPlugin2Config.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnSamplerVSTPlugin2Config.Name = "btnSamplerVSTPlugin2Config";
            this.btnSamplerVSTPlugin2Config.Size = new System.Drawing.Size(73, 31);
            this.btnSamplerVSTPlugin2Config.TabIndex = 24;
            this.btnSamplerVSTPlugin2Config.Text = "Config";
            this.btnSamplerVSTPlugin2Config.Click += new System.EventHandler(this.btnSamplerVSTPlugin2Config_Click);
            // 
            // cmbSamplerVSTPlugins2
            // 
            this.cmbSamplerVSTPlugins2.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbSamplerVSTPlugins2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSamplerVSTPlugins2.DropDownWidth = 257;
            this.cmbSamplerVSTPlugins2.Location = new System.Drawing.Point(209, 314);
            this.cmbSamplerVSTPlugins2.Margin = new System.Windows.Forms.Padding(4);
            this.cmbSamplerVSTPlugins2.Name = "cmbSamplerVSTPlugins2";
            this.cmbSamplerVSTPlugins2.Size = new System.Drawing.Size(360, 25);
            this.cmbSamplerVSTPlugins2.TabIndex = 25;
            this.cmbSamplerVSTPlugins2.SelectedIndexChanged += new System.EventHandler(this.cmbSamplerVSTPlugins2_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(4, 310);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label4.Size = new System.Drawing.Size(197, 33);
            this.label4.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label4.TabIndex = 26;
            this.label4.Text = "Sampler VST Effect #2:";
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMain.Controls.Add(this.tblMain);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(4);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlMain.Size = new System.Drawing.Size(702, 384);
            this.pnlMain.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlMain.TabIndex = 30;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(441, 11);
            this.btnExport.Margin = new System.Windows.Forms.Padding(5);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(116, 31);
            this.btnExport.TabIndex = 9;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(567, 11);
            this.btnImport.Margin = new System.Windows.Forms.Padding(5);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(116, 31);
            this.btnImport.TabIndex = 10;
            this.btnImport.Text = "Import";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // FrmPlugin
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(702, 437);
            this.Controls.Add(this.linLine);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPluginSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Halloumi : Shuffler : Effects";
            this.UseApplicationIcon = true;
            this.flpButtons.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbMainMixerPlugins2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrackFXVSTPlugins2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrackFXVSTPlugins)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSamplerVSTPlugins)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbWAPlugins)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVSTPlugins)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrackVSTPlugins)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSamplerVSTPlugins2)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private Halloumi.Common.Windows.Controls.Button btnOK;
        private Halloumi.Common.Windows.Controls.Panel pnlButtons;
        private Halloumi.Common.Windows.Controls.BeveledLine linLine;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private Halloumi.Common.Windows.Controls.Panel pnlMain;
        private Halloumi.Common.Windows.Controls.Label label1;
        private Halloumi.Common.Windows.Controls.Label title1;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbVSTPlugins;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbWAPlugins;
        private Halloumi.Common.Windows.Controls.Button btnWAPluginConfig;
        private Halloumi.Common.Windows.Controls.Button btnVSTPluginConfig;
        private Halloumi.Common.Windows.Controls.Label label2;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbSamplerVSTPlugins;
        private Halloumi.Common.Windows.Controls.Button btnSamplerVSTPluginConfig;
        private Halloumi.Common.Windows.Controls.Label label3;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbTrackVSTPlugins;
        private Halloumi.Common.Windows.Controls.Button btnTrackVSTPluginConfig;
        private Halloumi.Common.Windows.Controls.Label label5;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbTrackFXVSTPlugins;
        private Halloumi.Common.Windows.Controls.Button btnTrackFXVSTPluginConfig;
        private Halloumi.Common.Windows.Controls.Button btnSamplerVSTPlugin2Config;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbSamplerVSTPlugins2;
        private Halloumi.Common.Windows.Controls.Label label4;
        private Halloumi.Common.Windows.Controls.Label label6;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbTrackFXVSTPlugins2;
        private Halloumi.Common.Windows.Controls.Button btnTrackFXVSTPluginConfig2;
        private Halloumi.Common.Windows.Controls.Label label7;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbMainMixerPlugins2;
        private Halloumi.Common.Windows.Controls.Button btnMainMixerPluginConfig2;
        private Common.Windows.Controls.Button btnImport;
        private Common.Windows.Controls.Button btnExport;
    }
}