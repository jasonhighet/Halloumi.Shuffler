namespace Halloumi.Shuffler.Forms
{
    partial class FrmSettings
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
            Halloumi.Common.Windows.Controls.FolderSelectButton fsbWinampPluginFolder;
            Halloumi.Common.Windows.Controls.FolderSelectButton fsbAnalogXScratchFolder;
            Halloumi.Common.Windows.Controls.FolderSelectButton fsbKeyFinderFolder;
            this.txtWinampPluginFolder = new Halloumi.Common.Windows.Controls.TextBox();
            this.txtAnalogXScratchFolder = new Halloumi.Common.Windows.Controls.TextBox();
            this.txtKeyFinderFolder = new Halloumi.Common.Windows.Controls.TextBox();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new Halloumi.Common.Windows.Controls.Button();
            this.btnOK = new Halloumi.Common.Windows.Controls.Button();
            this.linLine = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.pnlButtons = new Halloumi.Common.Windows.Controls.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new Halloumi.Common.Windows.Controls.Label();
            this.kryptonComboBox1 = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.kryptonComboBox2 = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.pnlMain = new Halloumi.Common.Windows.Controls.Panel();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new Halloumi.Common.Windows.Controls.Label();
            this.label6 = new Halloumi.Common.Windows.Controls.Label();
            this.fsbVSTPluginFolder = new Halloumi.Common.Windows.Controls.FolderSelectButton();
            this.txtVSTPluginFolder = new Halloumi.Common.Windows.Controls.TextBox();
            this.fsbShufflerFolder = new Halloumi.Common.Windows.Controls.FolderSelectButton();
            this.txtShufflerFolder = new Halloumi.Common.Windows.Controls.TextBox();
            this.label3 = new Halloumi.Common.Windows.Controls.Label();
            this.title1 = new Halloumi.Common.Windows.Controls.Label();
            this.label4 = new Halloumi.Common.Windows.Controls.Label();
            this.label5 = new Halloumi.Common.Windows.Controls.Label();
            this.txtLibraryFolder = new Halloumi.Common.Windows.Controls.TextBox();
            this.fsbLibraryFolder = new Halloumi.Common.Windows.Controls.FolderSelectButton();
            this.kryptonManager1 = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            fsbWinampPluginFolder = new Halloumi.Common.Windows.Controls.FolderSelectButton();
            fsbAnalogXScratchFolder = new Halloumi.Common.Windows.Controls.FolderSelectButton();
            fsbKeyFinderFolder = new Halloumi.Common.Windows.Controls.FolderSelectButton();
            this.flpButtons.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox2)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.tblMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // fsbWinampPluginFolder
            // 
            fsbWinampPluginFolder.AssociatedControl = this.txtWinampPluginFolder;
            fsbWinampPluginFolder.Location = new System.Drawing.Point(612, 148);
            fsbWinampPluginFolder.Margin = new System.Windows.Forms.Padding(4);
            fsbWinampPluginFolder.Name = "fsbWinampPluginFolder";
            fsbWinampPluginFolder.Size = new System.Drawing.Size(36, 30);
            fsbWinampPluginFolder.TabIndex = 30;
            fsbWinampPluginFolder.Values.Text = "...";
            // 
            // txtWinampPluginFolder
            // 
            this.txtWinampPluginFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtWinampPluginFolder.ErrorMessage = "Please enter a WinAmp plugins folder";
            this.txtWinampPluginFolder.ErrorProvider = null;
            this.txtWinampPluginFolder.IsRequired = true;
            this.txtWinampPluginFolder.Location = new System.Drawing.Point(195, 148);
            this.txtWinampPluginFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtWinampPluginFolder.MaximumValue = 2147483647D;
            this.txtWinampPluginFolder.MinimumValue = -2147483648D;
            this.txtWinampPluginFolder.Name = "txtWinampPluginFolder";
            this.txtWinampPluginFolder.ReadOnly = true;
            this.txtWinampPluginFolder.Size = new System.Drawing.Size(409, 27);
            this.txtWinampPluginFolder.TabIndex = 25;
            this.txtWinampPluginFolder.TabStop = false;
            // 
            // fsbAnalogXScratchFolder
            // 
            fsbAnalogXScratchFolder.AssociatedControl = this.txtAnalogXScratchFolder;
            fsbAnalogXScratchFolder.Location = new System.Drawing.Point(612, 196);
            fsbAnalogXScratchFolder.Margin = new System.Windows.Forms.Padding(4);
            fsbAnalogXScratchFolder.Name = "fsbAnalogXScratchFolder";
            fsbAnalogXScratchFolder.Size = new System.Drawing.Size(36, 30);
            fsbAnalogXScratchFolder.TabIndex = 33;
            fsbAnalogXScratchFolder.Values.Text = "...";
            // 
            // txtAnalogXScratchFolder
            // 
            this.txtAnalogXScratchFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtAnalogXScratchFolder.ErrorMessage = "Please enter an AnalogX Scratch folder";
            this.txtAnalogXScratchFolder.ErrorProvider = null;
            this.txtAnalogXScratchFolder.IsRequired = true;
            this.txtAnalogXScratchFolder.Location = new System.Drawing.Point(195, 196);
            this.txtAnalogXScratchFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtAnalogXScratchFolder.MaximumValue = 2147483647D;
            this.txtAnalogXScratchFolder.MinimumValue = -2147483648D;
            this.txtAnalogXScratchFolder.Name = "txtAnalogXScratchFolder";
            this.txtAnalogXScratchFolder.ReadOnly = true;
            this.txtAnalogXScratchFolder.Size = new System.Drawing.Size(409, 27);
            this.txtAnalogXScratchFolder.TabIndex = 32;
            this.txtAnalogXScratchFolder.TabStop = false;
            // 
            // fsbKeyFinderFolder
            // 
            fsbKeyFinderFolder.AssociatedControl = this.txtKeyFinderFolder;
            fsbKeyFinderFolder.Location = new System.Drawing.Point(612, 244);
            fsbKeyFinderFolder.Margin = new System.Windows.Forms.Padding(4);
            fsbKeyFinderFolder.Name = "fsbKeyFinderFolder";
            fsbKeyFinderFolder.Size = new System.Drawing.Size(36, 30);
            fsbKeyFinderFolder.TabIndex = 34;
            fsbKeyFinderFolder.Values.Text = "...";
            // 
            // txtKeyFinderFolder
            // 
            this.txtKeyFinderFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtKeyFinderFolder.ErrorMessage = "Please enter a Keyfinder folder";
            this.txtKeyFinderFolder.ErrorProvider = null;
            this.txtKeyFinderFolder.IsRequired = true;
            this.txtKeyFinderFolder.Location = new System.Drawing.Point(195, 244);
            this.txtKeyFinderFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtKeyFinderFolder.MaximumValue = 2147483647D;
            this.txtKeyFinderFolder.MinimumValue = -2147483648D;
            this.txtKeyFinderFolder.Name = "txtKeyFinderFolder";
            this.txtKeyFinderFolder.ReadOnly = true;
            this.txtKeyFinderFolder.Size = new System.Drawing.Size(409, 27);
            this.txtKeyFinderFolder.TabIndex = 35;
            this.txtKeyFinderFolder.TabStop = false;
            // 
            // flpButtons
            // 
            this.flpButtons.BackColor = System.Drawing.Color.Transparent;
            this.flpButtons.Controls.Add(this.btnCancel);
            this.flpButtons.Controls.Add(this.btnOK);
            this.flpButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpButtons.Location = new System.Drawing.Point(0, 1);
            this.flpButtons.Margin = new System.Windows.Forms.Padding(4);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Padding = new System.Windows.Forms.Padding(7, 2, 7, 6);
            this.flpButtons.Size = new System.Drawing.Size(695, 52);
            this.flpButtons.TabIndex = 16;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(560, 7);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 38);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(434, 7);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(116, 38);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // linLine
            // 
            this.linLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.linLine.Location = new System.Drawing.Point(0, 313);
            this.linLine.Margin = new System.Windows.Forms.Padding(4);
            this.linLine.Name = "linLine";
            this.linLine.Size = new System.Drawing.Size(695, 2);
            this.linLine.TabIndex = 34;
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.flpButtons);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 315);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(4);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(695, 53);
            this.pnlButtons.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlButtons.TabIndex = 32;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.00504F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.99496F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.kryptonComboBox1, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 40);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label2.Size = new System.Drawing.Size(39, 27);
            this.label2.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label2.TabIndex = 12;
            this.label2.Text = "VST Plugin:";
            // 
            // kryptonComboBox1
            // 
            this.kryptonComboBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kryptonComboBox1.DropDownWidth = 257;
            this.kryptonComboBox1.Location = new System.Drawing.Point(48, 23);
            this.kryptonComboBox1.Name = "kryptonComboBox1";
            this.kryptonComboBox1.Size = new System.Drawing.Size(82, 25);
            this.kryptonComboBox1.TabIndex = 13;
            // 
            // kryptonComboBox2
            // 
            this.kryptonComboBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kryptonComboBox2.DropDownWidth = 257;
            this.kryptonComboBox2.Location = new System.Drawing.Point(27, 21);
            this.kryptonComboBox2.Name = "kryptonComboBox2";
            this.kryptonComboBox2.Size = new System.Drawing.Size(41, 25);
            this.kryptonComboBox2.TabIndex = 14;
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
            this.pnlMain.Size = new System.Drawing.Size(695, 313);
            this.pnlMain.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlMain.TabIndex = 35;
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 3;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.50985F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.49015F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tblMain.Controls.Add(this.label7, 0, 5);
            this.tblMain.Controls.Add(this.txtKeyFinderFolder, 0, 5);
            this.tblMain.Controls.Add(fsbKeyFinderFolder, 2, 5);
            this.tblMain.Controls.Add(this.txtAnalogXScratchFolder, 0, 4);
            this.tblMain.Controls.Add(this.label6, 0, 4);
            this.tblMain.Controls.Add(fsbWinampPluginFolder, 2, 3);
            this.tblMain.Controls.Add(this.fsbVSTPluginFolder, 2, 2);
            this.tblMain.Controls.Add(this.fsbShufflerFolder, 2, 1);
            this.tblMain.Controls.Add(this.label3, 0, 3);
            this.tblMain.Controls.Add(this.title1, 0, 1);
            this.tblMain.Controls.Add(this.label4, 0, 0);
            this.tblMain.Controls.Add(this.label5, 0, 2);
            this.tblMain.Controls.Add(this.txtLibraryFolder, 1, 0);
            this.tblMain.Controls.Add(this.txtShufflerFolder, 1, 1);
            this.tblMain.Controls.Add(this.txtVSTPluginFolder, 1, 2);
            this.tblMain.Controls.Add(this.txtWinampPluginFolder, 1, 3);
            this.tblMain.Controls.Add(fsbAnalogXScratchFolder, 2, 4);
            this.tblMain.Controls.Add(this.fsbLibraryFolder, 2, 0);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(13, 12);
            this.tblMain.Margin = new System.Windows.Forms.Padding(4);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 6;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28587F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28587F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28445F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28445F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28816F));
            this.tblMain.Size = new System.Drawing.Size(669, 289);
            this.tblMain.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(4, 240);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label7.Size = new System.Drawing.Size(183, 33);
            this.label7.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label7.TabIndex = 36;
            this.label7.Text = "Key Finder Folder:";
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(4, 192);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label6.Size = new System.Drawing.Size(183, 33);
            this.label6.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label6.TabIndex = 31;
            this.label6.Text = "AnlogX Scratch Folder:";
            // 
            // fsbVSTPluginFolder
            // 
            this.fsbVSTPluginFolder.AssociatedControl = this.txtVSTPluginFolder;
            this.fsbVSTPluginFolder.Location = new System.Drawing.Point(612, 100);
            this.fsbVSTPluginFolder.Margin = new System.Windows.Forms.Padding(4);
            this.fsbVSTPluginFolder.Name = "fsbVSTPluginFolder";
            this.fsbVSTPluginFolder.Size = new System.Drawing.Size(36, 30);
            this.fsbVSTPluginFolder.TabIndex = 29;
            this.fsbVSTPluginFolder.Values.Text = "...";
            // 
            // txtVSTPluginFolder
            // 
            this.txtVSTPluginFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtVSTPluginFolder.ErrorMessage = "Please enter a VST Plugins folder";
            this.txtVSTPluginFolder.ErrorProvider = null;
            this.txtVSTPluginFolder.IsRequired = true;
            this.txtVSTPluginFolder.Location = new System.Drawing.Point(195, 100);
            this.txtVSTPluginFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtVSTPluginFolder.MaximumValue = 2147483647D;
            this.txtVSTPluginFolder.MinimumValue = -2147483648D;
            this.txtVSTPluginFolder.Name = "txtVSTPluginFolder";
            this.txtVSTPluginFolder.ReadOnly = true;
            this.txtVSTPluginFolder.Size = new System.Drawing.Size(409, 27);
            this.txtVSTPluginFolder.TabIndex = 24;
            this.txtVSTPluginFolder.TabStop = false;
            // 
            // fsbShufflerFolder
            // 
            this.fsbShufflerFolder.AssociatedControl = this.txtShufflerFolder;
            this.fsbShufflerFolder.Location = new System.Drawing.Point(612, 52);
            this.fsbShufflerFolder.Margin = new System.Windows.Forms.Padding(4);
            this.fsbShufflerFolder.Name = "fsbShufflerFolder";
            this.fsbShufflerFolder.Size = new System.Drawing.Size(36, 30);
            this.fsbShufflerFolder.TabIndex = 27;
            this.fsbShufflerFolder.Values.Text = "...";
            // 
            // txtShufflerFolder
            // 
            this.txtShufflerFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtShufflerFolder.ErrorMessage = "Please enter a shuffler folder";
            this.txtShufflerFolder.ErrorProvider = null;
            this.txtShufflerFolder.IsRequired = true;
            this.txtShufflerFolder.Location = new System.Drawing.Point(195, 52);
            this.txtShufflerFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtShufflerFolder.MaximumValue = 2147483647D;
            this.txtShufflerFolder.MinimumValue = -2147483648D;
            this.txtShufflerFolder.Name = "txtShufflerFolder";
            this.txtShufflerFolder.ReadOnly = true;
            this.txtShufflerFolder.Size = new System.Drawing.Size(409, 27);
            this.txtShufflerFolder.TabIndex = 22;
            this.txtShufflerFolder.TabStop = false;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(4, 144);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label3.Size = new System.Drawing.Size(183, 33);
            this.label3.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label3.TabIndex = 18;
            this.label3.Text = "WinAmp Plugins Folder:";
            // 
            // title1
            // 
            this.title1.Dock = System.Windows.Forms.DockStyle.Top;
            this.title1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title1.ForeColor = System.Drawing.Color.White;
            this.title1.Location = new System.Drawing.Point(4, 48);
            this.title1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.title1.Name = "title1";
            this.title1.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.title1.Size = new System.Drawing.Size(183, 33);
            this.title1.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.title1.TabIndex = 11;
            this.title1.Text = "Shuffler Folder:";
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(4, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label4.Size = new System.Drawing.Size(183, 33);
            this.label4.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label4.TabIndex = 19;
            this.label4.Text = "Library Folder:";
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(4, 96);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label5.Size = new System.Drawing.Size(183, 33);
            this.label5.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label5.TabIndex = 20;
            this.label5.Text = "VST Plugins Folder:";
            // 
            // txtLibraryFolder
            // 
            this.txtLibraryFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtLibraryFolder.ErrorMessage = "Please enter a library folder";
            this.txtLibraryFolder.ErrorProvider = null;
            this.txtLibraryFolder.IsRequired = true;
            this.txtLibraryFolder.Location = new System.Drawing.Point(195, 4);
            this.txtLibraryFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtLibraryFolder.MaximumValue = 2147483647D;
            this.txtLibraryFolder.MinimumValue = -2147483648D;
            this.txtLibraryFolder.Name = "txtLibraryFolder";
            this.txtLibraryFolder.ReadOnly = true;
            this.txtLibraryFolder.Size = new System.Drawing.Size(409, 27);
            this.txtLibraryFolder.TabIndex = 21;
            this.txtLibraryFolder.TabStop = false;
            // 
            // fsbLibraryFolder
            // 
            this.fsbLibraryFolder.AssociatedControl = this.txtLibraryFolder;
            this.fsbLibraryFolder.Location = new System.Drawing.Point(612, 4);
            this.fsbLibraryFolder.Margin = new System.Windows.Forms.Padding(4);
            this.fsbLibraryFolder.Name = "fsbLibraryFolder";
            this.fsbLibraryFolder.Size = new System.Drawing.Size(36, 30);
            this.fsbLibraryFolder.TabIndex = 26;
            this.fsbLibraryFolder.Values.Text = "...";
            // 
            // kryptonManager1
            // 
            this.kryptonManager1.GlobalAllowFormChrome = false;
            this.kryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2010Silver;
            // 
            // FrmSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AllowFormChrome = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(695, 368);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.linLine);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Halloumi : Shuffler : Settings";
            this.UseApplicationIcon = true;
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.flpButtons.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox2)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            this.tblMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private Halloumi.Common.Windows.Controls.Button btnOK;
        private Halloumi.Common.Windows.Controls.BeveledLine linLine;
        private Halloumi.Common.Windows.Controls.Panel pnlButtons;
        private Halloumi.Common.Windows.Controls.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Halloumi.Common.Windows.Controls.Label label2;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kryptonComboBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kryptonComboBox2;
        private Halloumi.Common.Windows.Controls.Panel pnlMain;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private Halloumi.Common.Windows.Controls.Label label3;
        private Halloumi.Common.Windows.Controls.Label title1;
        private Halloumi.Common.Windows.Controls.Label label4;
        private Halloumi.Common.Windows.Controls.Label label5;
        private Halloumi.Common.Windows.Controls.TextBox txtLibraryFolder;
        private Halloumi.Common.Windows.Controls.TextBox txtShufflerFolder;
        private Halloumi.Common.Windows.Controls.TextBox txtVSTPluginFolder;
        private Halloumi.Common.Windows.Controls.TextBox txtWinampPluginFolder;
        private Halloumi.Common.Windows.Controls.FolderSelectButton fsbVSTPluginFolder;
        private Halloumi.Common.Windows.Controls.FolderSelectButton fsbShufflerFolder;
        private Halloumi.Common.Windows.Controls.FolderSelectButton fsbLibraryFolder;
        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager1;
        private Halloumi.Common.Windows.Controls.TextBox txtAnalogXScratchFolder;
        private Halloumi.Common.Windows.Controls.Label label6;
        private Halloumi.Common.Windows.Controls.Label label7;
        private Halloumi.Common.Windows.Controls.TextBox txtKeyFinderFolder;
    }
}