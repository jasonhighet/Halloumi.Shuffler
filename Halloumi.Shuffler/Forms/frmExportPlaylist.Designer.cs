namespace Halloumi.Shuffler.Forms
{
    partial class frmExportPlaylist
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
            this.txtAlbumImage = new Halloumi.Common.Windows.Controls.TextBox();
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
            this.label3 = new Halloumi.Common.Windows.Controls.Label();
            this.label1 = new Halloumi.Common.Windows.Controls.Label();
            this.title1 = new Halloumi.Common.Windows.Controls.Label();
            this.label4 = new Halloumi.Common.Windows.Controls.Label();
            this.label5 = new Halloumi.Common.Windows.Controls.Label();
            this.txtOutputFolder = new Halloumi.Common.Windows.Controls.TextBox();
            this.fsbLibraryFolder = new Halloumi.Common.Windows.Controls.FolderSelectButton();
            this.chkTrackNumbers = new Halloumi.Common.Windows.Controls.CheckBox();
            this.cmbAlbumArtist = new Halloumi.Common.Windows.Controls.ComboBox();
            this.flsSelectImage = new Halloumi.Common.Windows.Controls.FileSelectButton();
            this.txtAlbumName = new Halloumi.Common.Windows.Controls.TextBox();
            this.chkCreateSubfolder = new Halloumi.Common.Windows.Controls.CheckBox();
            this.progressDialog = new Halloumi.Common.Windows.Components.ProgressDialog(this.components);
            this.flpButtons.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox2)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.tblMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAlbumArtist)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAlbumImage
            // 
            this.txtAlbumImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtAlbumImage.ErrorProvider = null;
            this.txtAlbumImage.Location = new System.Drawing.Point(133, 195);
            this.txtAlbumImage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtAlbumImage.MaximumValue = 2147483647D;
            this.txtAlbumImage.MinimumValue = -2147483648D;
            this.txtAlbumImage.Name = "txtAlbumImage";
            this.txtAlbumImage.ReadOnly = true;
            this.txtAlbumImage.Size = new System.Drawing.Size(461, 27);
            this.txtAlbumImage.TabIndex = 25;
            this.txtAlbumImage.TabStop = false;
            this.txtAlbumImage.TextChanged += new System.EventHandler(this.txtAlbumImage_TextChanged);
            // 
            // flpButtons
            // 
            this.flpButtons.BackColor = System.Drawing.Color.Transparent;
            this.flpButtons.Controls.Add(this.btnCancel);
            this.flpButtons.Controls.Add(this.btnOK);
            this.flpButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpButtons.Location = new System.Drawing.Point(0, 1);
            this.flpButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Padding = new System.Windows.Forms.Padding(7, 2, 7, 6);
            this.flpButtons.Size = new System.Drawing.Size(680, 52);
            this.flpButtons.TabIndex = 16;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(545, 7);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 38);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(419, 7);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(116, 38);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // linLine
            // 
            this.linLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.linLine.Location = new System.Drawing.Point(0, 258);
            this.linLine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.linLine.Name = "linLine";
            this.linLine.Size = new System.Drawing.Size(680, 2);
            this.linLine.TabIndex = 34;
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.flpButtons);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 260);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.pnlMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlMain.Size = new System.Drawing.Size(680, 258);
            this.pnlMain.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlMain.TabIndex = 35;
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 3;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.63355F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78.36645F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tblMain.Controls.Add(this.label3, 0, 5);
            this.tblMain.Controls.Add(this.label1, 0, 3);
            this.tblMain.Controls.Add(this.title1, 0, 2);
            this.tblMain.Controls.Add(this.label4, 0, 0);
            this.tblMain.Controls.Add(this.label5, 0, 4);
            this.tblMain.Controls.Add(this.txtOutputFolder, 1, 0);
            this.tblMain.Controls.Add(this.txtAlbumImage, 1, 5);
            this.tblMain.Controls.Add(this.fsbLibraryFolder, 2, 0);
            this.tblMain.Controls.Add(this.chkTrackNumbers, 1, 4);
            this.tblMain.Controls.Add(this.cmbAlbumArtist, 1, 3);
            this.tblMain.Controls.Add(this.flsSelectImage, 2, 5);
            this.tblMain.Controls.Add(this.txtAlbumName, 1, 2);
            this.tblMain.Controls.Add(this.chkCreateSubfolder, 1, 1);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(13, 12);
            this.tblMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 6;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00125F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00125F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99875F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99875F));
            this.tblMain.Size = new System.Drawing.Size(654, 234);
            this.tblMain.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(4, 191);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label3.Size = new System.Drawing.Size(121, 33);
            this.label3.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label3.TabIndex = 18;
            this.label3.Text = "Image:";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 111);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label1.Size = new System.Drawing.Size(121, 33);
            this.label1.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label1.TabIndex = 12;
            this.label1.Text = "Album Artist:";
            // 
            // title1
            // 
            this.title1.Dock = System.Windows.Forms.DockStyle.Top;
            this.title1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title1.ForeColor = System.Drawing.Color.White;
            this.title1.Location = new System.Drawing.Point(4, 71);
            this.title1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.title1.Name = "title1";
            this.title1.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.title1.Size = new System.Drawing.Size(121, 33);
            this.title1.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.title1.TabIndex = 11;
            this.title1.Text = "Album Name:";
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
            this.label4.Size = new System.Drawing.Size(121, 33);
            this.label4.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label4.TabIndex = 19;
            this.label4.Text = "Output Folder:";
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(4, 151);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label5.Size = new System.Drawing.Size(121, 33);
            this.label5.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label5.TabIndex = 20;
            this.label5.Text = "Options:";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtOutputFolder.ErrorMessage = "Please enter a library folder";
            this.txtOutputFolder.ErrorProvider = null;
            this.txtOutputFolder.IsRequired = true;
            this.txtOutputFolder.Location = new System.Drawing.Point(133, 4);
            this.txtOutputFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtOutputFolder.MaximumValue = 2147483647D;
            this.txtOutputFolder.MinimumValue = -2147483648D;
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.ReadOnly = true;
            this.txtOutputFolder.Size = new System.Drawing.Size(461, 27);
            this.txtOutputFolder.TabIndex = 21;
            this.txtOutputFolder.TabStop = false;
            // 
            // fsbLibraryFolder
            // 
            this.fsbLibraryFolder.AssociatedControl = this.txtOutputFolder;
            this.fsbLibraryFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.fsbLibraryFolder.Location = new System.Drawing.Point(602, 4);
            this.fsbLibraryFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fsbLibraryFolder.Name = "fsbLibraryFolder";
            this.fsbLibraryFolder.Size = new System.Drawing.Size(48, 29);
            this.fsbLibraryFolder.TabIndex = 26;
            this.fsbLibraryFolder.Values.Text = "...";
            // 
            // chkTrackNumbers
            // 
            this.chkTrackNumbers.Checked = true;
            this.chkTrackNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrackNumbers.Location = new System.Drawing.Point(133, 155);
            this.chkTrackNumbers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkTrackNumbers.Name = "chkTrackNumbers";
            this.chkTrackNumbers.Size = new System.Drawing.Size(275, 24);
            this.chkTrackNumbers.TabIndex = 31;
            this.chkTrackNumbers.Value = "True";
            this.chkTrackNumbers.Values.Text = "Use playlist position as track number";
            // 
            // cmbAlbumArtist
            // 
            this.tblMain.SetColumnSpan(this.cmbAlbumArtist, 2);
            this.cmbAlbumArtist.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbAlbumArtist.DropDownWidth = 386;
            this.cmbAlbumArtist.ErrorMessage = "Please enter an album artist";
            this.cmbAlbumArtist.ErrorProvider = null;
            this.cmbAlbumArtist.IsRequired = true;
            this.cmbAlbumArtist.Location = new System.Drawing.Point(133, 115);
            this.cmbAlbumArtist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbAlbumArtist.Name = "cmbAlbumArtist";
            this.cmbAlbumArtist.Size = new System.Drawing.Size(517, 25);
            this.cmbAlbumArtist.TabIndex = 33;
            // 
            // flsSelectImage
            // 
            this.flsSelectImage.AssociatedControl = this.txtAlbumImage;
            this.flsSelectImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.flsSelectImage.Filter = "Images (*.jpg)|*.jpg";
            this.flsSelectImage.Location = new System.Drawing.Point(602, 195);
            this.flsSelectImage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flsSelectImage.Name = "flsSelectImage";
            this.flsSelectImage.Size = new System.Drawing.Size(48, 31);
            this.flsSelectImage.TabIndex = 34;
            this.flsSelectImage.Title = "Select album image";
            this.flsSelectImage.Values.Text = "...";
            // 
            // txtAlbumName
            // 
            this.tblMain.SetColumnSpan(this.txtAlbumName, 2);
            this.txtAlbumName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtAlbumName.ErrorMessage = "Please enter an album name";
            this.txtAlbumName.ErrorProvider = null;
            this.txtAlbumName.IsRequired = true;
            this.txtAlbumName.Location = new System.Drawing.Point(133, 75);
            this.txtAlbumName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtAlbumName.MaximumValue = 2147483647D;
            this.txtAlbumName.MinimumValue = -2147483648D;
            this.txtAlbumName.Name = "txtAlbumName";
            this.txtAlbumName.Size = new System.Drawing.Size(517, 27);
            this.txtAlbumName.TabIndex = 35;
            // 
            // chkCreateSubfolder
            // 
            this.chkCreateSubfolder.Checked = true;
            this.chkCreateSubfolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreateSubfolder.Location = new System.Drawing.Point(133, 41);
            this.chkCreateSubfolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkCreateSubfolder.Name = "chkCreateSubfolder";
            this.chkCreateSubfolder.Size = new System.Drawing.Size(300, 24);
            this.chkCreateSubfolder.TabIndex = 36;
            this.chkCreateSubfolder.Value = "True";
            this.chkCreateSubfolder.Values.Text = "Create sub-folder based on album name";
            // 
            // progressDialog
            // 
            this.progressDialog.PerformProcessing += new System.EventHandler(this.progressDialog_PerformProcessing);
            this.progressDialog.ProcessingCompleted += new System.EventHandler(this.progressDialog_ProcessingCompleted);
            // 
            // frmExportPlaylist
            // 
            this.AcceptButton = this.btnOK;
            this.AllowFormChrome = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(680, 313);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.linLine);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExportPlaylist";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Halloumi : Shuffler : Playlist Export";
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
            ((System.ComponentModel.ISupportInitialize)(this.cmbAlbumArtist)).EndInit();
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
        private Halloumi.Common.Windows.Controls.Label label1;
        private Halloumi.Common.Windows.Controls.Label title1;
        private Halloumi.Common.Windows.Controls.Label label4;
        private Halloumi.Common.Windows.Controls.Label label5;
        private Halloumi.Common.Windows.Controls.TextBox txtOutputFolder;
        private Halloumi.Common.Windows.Controls.TextBox txtAlbumImage;
        private Halloumi.Common.Windows.Controls.FolderSelectButton fsbLibraryFolder;
        private Halloumi.Common.Windows.Controls.CheckBox chkTrackNumbers;
        private Halloumi.Common.Windows.Controls.ComboBox cmbAlbumArtist;
        private Halloumi.Common.Windows.Controls.FileSelectButton flsSelectImage;
        private Halloumi.Common.Windows.Components.ProgressDialog progressDialog;
        private Halloumi.Common.Windows.Controls.TextBox txtAlbumName;
        private Halloumi.Common.Windows.Controls.CheckBox chkCreateSubfolder;
    }
}