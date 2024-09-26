namespace Halloumi.Shuffler.Forms
{
    partial class frmImportShufflerTracks
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
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOK = new Halloumi.Common.Windows.Controls.Button();
            this.linLine = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.pnlButtons = new Halloumi.Common.Windows.Controls.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new Halloumi.Common.Windows.Controls.Label();
            this.kryptonComboBox1 = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.kryptonComboBox2 = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.pnlMain = new Halloumi.Common.Windows.Controls.Panel();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new Halloumi.Common.Windows.Controls.Label();
            this.txtImportFolder = new Halloumi.Common.Windows.Controls.TextBox();
            this.fsbLibraryFolder = new Halloumi.Common.Windows.Controls.FolderSelectButton();
            this.progressDialog = new Halloumi.Common.Windows.Components.ProgressDialog(this.components);
            this.flpButtons.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox2)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.tblMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpButtons
            // 
            this.flpButtons.BackColor = System.Drawing.Color.Transparent;
            this.flpButtons.Controls.Add(this.btnOK);
            this.flpButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpButtons.Location = new System.Drawing.Point(0, 1);
            this.flpButtons.Margin = new System.Windows.Forms.Padding(4);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Padding = new System.Windows.Forms.Padding(7, 2, 7, 6);
            this.flpButtons.Size = new System.Drawing.Size(680, 52);
            this.flpButtons.TabIndex = 16;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(545, 7);
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
            this.linLine.Location = new System.Drawing.Point(0, 100);
            this.linLine.Margin = new System.Windows.Forms.Padding(4);
            this.linLine.Name = "linLine";
            this.linLine.Size = new System.Drawing.Size(680, 2);
            this.linLine.TabIndex = 34;
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.flpButtons);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 102);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(4);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(680, 53);
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
            this.pnlMain.Size = new System.Drawing.Size(680, 100);
            this.pnlMain.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlMain.TabIndex = 35;
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 3;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.63355F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78.36645F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tblMain.Controls.Add(this.label4, 0, 0);
            this.tblMain.Controls.Add(this.txtImportFolder, 1, 0);
            this.tblMain.Controls.Add(this.fsbLibraryFolder, 2, 0);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(13, 12);
            this.tblMain.Margin = new System.Windows.Forms.Padding(4);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tblMain.Size = new System.Drawing.Size(654, 76);
            this.tblMain.TabIndex = 9;
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
            this.label4.Size = new System.Drawing.Size(120, 33);
            this.label4.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label4.TabIndex = 19;
            this.label4.Text = "Import Folder:";
            // 
            // txtImportFolder
            // 
            this.txtImportFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtImportFolder.ErrorMessage = "Please enter a library folder";
            this.txtImportFolder.ErrorProvider = null;
            this.txtImportFolder.IsRequired = true;
            this.txtImportFolder.Location = new System.Drawing.Point(132, 4);
            this.txtImportFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtImportFolder.MaximumValue = 2147483647D;
            this.txtImportFolder.MinimumValue = -2147483648D;
            this.txtImportFolder.Name = "txtImportFolder";
            this.txtImportFolder.ReadOnly = true;
            this.txtImportFolder.Size = new System.Drawing.Size(458, 27);
            this.txtImportFolder.TabIndex = 21;
            this.txtImportFolder.TabStop = false;
            // 
            // fsbLibraryFolder
            // 
            this.fsbLibraryFolder.AssociatedControl = this.txtImportFolder;
            this.fsbLibraryFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.fsbLibraryFolder.Location = new System.Drawing.Point(598, 4);
            this.fsbLibraryFolder.Margin = new System.Windows.Forms.Padding(4);
            this.fsbLibraryFolder.Name = "fsbLibraryFolder";
            this.fsbLibraryFolder.Size = new System.Drawing.Size(52, 29);
            this.fsbLibraryFolder.TabIndex = 26;
            this.fsbLibraryFolder.Values.Text = "...";
            // 
            // FrmImportShufflerTracks
            // 
            this.AcceptButton = this.btnOK;
            this.AllowFormChrome = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 155);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.linLine);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmImportShufflerTracks";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Halloumi : Shuffler : Import Tracks";
            this.UseApplicationIcon = true;
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Halloumi.Common.Windows.Controls.Label label2;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kryptonComboBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kryptonComboBox2;
        private Halloumi.Common.Windows.Controls.Panel pnlMain;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private Halloumi.Common.Windows.Controls.Label label4;
        private Halloumi.Common.Windows.Controls.TextBox txtImportFolder;
        private Halloumi.Common.Windows.Controls.FolderSelectButton fsbLibraryFolder;
        private Halloumi.Common.Windows.Components.ProgressDialog progressDialog;
    }
}