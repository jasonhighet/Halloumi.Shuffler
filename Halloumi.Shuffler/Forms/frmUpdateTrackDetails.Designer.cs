namespace Halloumi.Shuffler.Forms
{
    partial class FrmUpdateTrackDetails
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
            this.pnlMain = new Halloumi.Common.Windows.Controls.Panel();
            this.chkUpdateAuxillaryFiles = new Halloumi.Common.Windows.Controls.CheckBox();
            this.txtFile = new Halloumi.Common.Windows.Controls.TextBox();
            this.label7 = new Halloumi.Common.Windows.Controls.Label();
            this.panel3 = new Halloumi.Common.Windows.Controls.Panel();
            this.panel2 = new Halloumi.Common.Windows.Controls.Panel();
            this.cmbGenre = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label6 = new Halloumi.Common.Windows.Controls.Label();
            this.panel1 = new Halloumi.Common.Windows.Controls.Panel();
            this.cmbTrackNumber = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label1 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbAlbumArtist = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label5 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbAlbum = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label4 = new Halloumi.Common.Windows.Controls.Label();
            this.txtTitle = new Halloumi.Common.Windows.Controls.TextBox();
            this.label3 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbArtist = new Halloumi.Common.Windows.Controls.ComboBox();
            this.label2 = new Halloumi.Common.Windows.Controls.Label();
            this.flpButtonsRight = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new Halloumi.Common.Windows.Controls.Button();
            this.btnOK = new Halloumi.Common.Windows.Controls.Button();
            this.beveledLine = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.pnlButtons = new Halloumi.Common.Windows.Controls.Panel();
            this.pnlMain.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGenre)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrackNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAlbumArtist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAlbum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbArtist)).BeginInit();
            this.flpButtonsRight.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMain.Controls.Add(this.chkUpdateAuxillaryFiles);
            this.pnlMain.Controls.Add(this.txtFile);
            this.pnlMain.Controls.Add(this.label7);
            this.pnlMain.Controls.Add(this.panel3);
            this.pnlMain.Controls.Add(this.cmbAlbumArtist);
            this.pnlMain.Controls.Add(this.label5);
            this.pnlMain.Controls.Add(this.cmbAlbum);
            this.pnlMain.Controls.Add(this.label4);
            this.pnlMain.Controls.Add(this.txtTitle);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.cmbArtist);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(13, 0, 13, 12);
            this.pnlMain.Size = new System.Drawing.Size(531, 381);
            this.pnlMain.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlMain.TabIndex = 29;
            // 
            // chkUpdateAuxillaryFiles
            // 
            this.chkUpdateAuxillaryFiles.AutoSize = false;
            this.chkUpdateAuxillaryFiles.Checked = true;
            this.chkUpdateAuxillaryFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUpdateAuxillaryFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkUpdateAuxillaryFiles.Location = new System.Drawing.Point(13, 347);
            this.chkUpdateAuxillaryFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkUpdateAuxillaryFiles.Name = "chkUpdateAuxillaryFiles";
            this.chkUpdateAuxillaryFiles.Size = new System.Drawing.Size(505, 49);
            this.chkUpdateAuxillaryFiles.TabIndex = 44;
            this.chkUpdateAuxillaryFiles.Value = "True";
            this.chkUpdateAuxillaryFiles.Values.Text = "Rename in shuffler files, playlists etc.";
            // 
            // txtFile
            // 
            this.txtFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtFile.ErrorMessage = "Please enter a title.";
            this.txtFile.ErrorProvider = null;
            this.txtFile.Location = new System.Drawing.Point(13, 320);
            this.txtFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFile.MaximumValue = 2147483647D;
            this.txtFile.MinimumValue = -2147483648D;
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.Size = new System.Drawing.Size(505, 27);
            this.txtFile.TabIndex = 42;
            this.txtFile.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(13, 288);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(0, 12, 0, 1);
            this.label7.Size = new System.Drawing.Size(36, 32);
            this.label7.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label7.TabIndex = 41;
            this.label7.Text = "File:";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(13, 230);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(505, 58);
            this.panel3.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.panel3.TabIndex = 40;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.cmbGenre);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(112, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.panel2.Size = new System.Drawing.Size(393, 58);
            this.panel2.TabIndex = 40;
            // 
            // cmbGenre
            // 
            this.cmbGenre.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbGenre.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbGenre.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbGenre.DropDownWidth = 264;
            this.cmbGenre.ErrorMessage = "Please enter a genre.";
            this.cmbGenre.ErrorProvider = null;
            this.cmbGenre.IsRequired = true;
            this.cmbGenre.Location = new System.Drawing.Point(13, 32);
            this.cmbGenre.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbGenre.MaximumValue = 100;
            this.cmbGenre.MinimumValue = 0;
            this.cmbGenre.Name = "cmbGenre";
            this.cmbGenre.Size = new System.Drawing.Size(380, 25);
            this.cmbGenre.TabIndex = 40;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(13, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(0, 12, 0, 1);
            this.label6.Size = new System.Drawing.Size(53, 32);
            this.label6.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label6.TabIndex = 39;
            this.label6.Text = "Genre:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.cmbTrackNumber);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(112, 58);
            this.panel1.TabIndex = 3;
            // 
            // cmbTrackNumber
            // 
            this.cmbTrackNumber.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbTrackNumber.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbTrackNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbTrackNumber.DropDownWidth = 264;
            this.cmbTrackNumber.EntryType = Halloumi.Common.Windows.Controls.ComboBox.TextEntryType.Integer;
            this.cmbTrackNumber.ErrorProvider = null;
            this.cmbTrackNumber.Location = new System.Drawing.Point(0, 32);
            this.cmbTrackNumber.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbTrackNumber.MaximumValue = 100;
            this.cmbTrackNumber.MinimumValue = 0;
            this.cmbTrackNumber.Name = "cmbTrackNumber";
            this.cmbTrackNumber.Size = new System.Drawing.Size(112, 25);
            this.cmbTrackNumber.TabIndex = 3;
            this.cmbTrackNumber.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 12, 0, 1);
            this.label1.Size = new System.Drawing.Size(109, 32);
            this.label1.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label1.TabIndex = 2;
            this.label1.Text = "Track Number:";
            // 
            // cmbAlbumArtist
            // 
            this.cmbAlbumArtist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbAlbumArtist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbAlbumArtist.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbAlbumArtist.DropDownWidth = 264;
            this.cmbAlbumArtist.ErrorMessage = "Please enter an album artist.";
            this.cmbAlbumArtist.ErrorProvider = null;
            this.cmbAlbumArtist.IsRequired = true;
            this.cmbAlbumArtist.Location = new System.Drawing.Point(13, 205);
            this.cmbAlbumArtist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbAlbumArtist.Name = "cmbAlbumArtist";
            this.cmbAlbumArtist.Size = new System.Drawing.Size(505, 25);
            this.cmbAlbumArtist.TabIndex = 37;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(13, 173);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 12, 0, 1);
            this.label5.Size = new System.Drawing.Size(97, 32);
            this.label5.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label5.TabIndex = 36;
            this.label5.Text = "Album Artist:";
            // 
            // cmbAlbum
            // 
            this.cmbAlbum.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbAlbum.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbAlbum.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbAlbum.DropDownWidth = 264;
            this.cmbAlbum.ErrorMessage = "Please enter an album.";
            this.cmbAlbum.ErrorProvider = null;
            this.cmbAlbum.IsRequired = true;
            this.cmbAlbum.Location = new System.Drawing.Point(13, 148);
            this.cmbAlbum.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbAlbum.Name = "cmbAlbum";
            this.cmbAlbum.Size = new System.Drawing.Size(505, 25);
            this.cmbAlbum.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(13, 116);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 12, 0, 1);
            this.label4.Size = new System.Drawing.Size(57, 32);
            this.label4.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label4.TabIndex = 34;
            this.label4.Text = "Album:";
            // 
            // txtTitle
            // 
            this.txtTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtTitle.ErrorMessage = "Please enter a title.";
            this.txtTitle.ErrorProvider = null;
            this.txtTitle.IsRequired = true;
            this.txtTitle.Location = new System.Drawing.Point(13, 89);
            this.txtTitle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTitle.MaximumValue = 2147483647D;
            this.txtTitle.MinimumValue = -2147483648D;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(505, 27);
            this.txtTitle.TabIndex = 33;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(13, 57);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 12, 0, 1);
            this.label3.Size = new System.Drawing.Size(42, 32);
            this.label3.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label3.TabIndex = 5;
            this.label3.Text = "Title:";
            // 
            // cmbArtist
            // 
            this.cmbArtist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbArtist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbArtist.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbArtist.DropDownWidth = 264;
            this.cmbArtist.ErrorMessage = "Please enter an artist.";
            this.cmbArtist.ErrorProvider = null;
            this.cmbArtist.IsRequired = true;
            this.cmbArtist.Location = new System.Drawing.Point(13, 32);
            this.cmbArtist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbArtist.Name = "cmbArtist";
            this.cmbArtist.Size = new System.Drawing.Size(505, 25);
            this.cmbArtist.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(13, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 12, 0, 1);
            this.label2.Size = new System.Drawing.Size(49, 32);
            this.label2.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label2.TabIndex = 3;
            this.label2.Text = "Artist:";
            // 
            // flpButtonsRight
            // 
            this.flpButtonsRight.BackColor = System.Drawing.Color.Transparent;
            this.flpButtonsRight.Controls.Add(this.btnCancel);
            this.flpButtonsRight.Controls.Add(this.btnOK);
            this.flpButtonsRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpButtonsRight.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpButtonsRight.Location = new System.Drawing.Point(0, 0);
            this.flpButtonsRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flpButtonsRight.Name = "flpButtonsRight";
            this.flpButtonsRight.Padding = new System.Windows.Forms.Padding(7, 2, 7, 6);
            this.flpButtonsRight.Size = new System.Drawing.Size(531, 53);
            this.flpButtonsRight.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(411, 7);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 38);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "&Cancel";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(300, 7);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(101, 38);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // beveledLine
            // 
            this.beveledLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.beveledLine.Location = new System.Drawing.Point(0, 381);
            this.beveledLine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.beveledLine.Name = "beveledLine";
            this.beveledLine.Size = new System.Drawing.Size(531, 2);
            this.beveledLine.TabIndex = 28;
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.flpButtonsRight);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 383);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(531, 53);
            this.pnlButtons.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlButtons.TabIndex = 27;
            // 
            // frmUpdateTrackDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 436);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.beveledLine);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmUpdateTrackDetails";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Update Track Details";
            this.Load += new System.EventHandler(this.frmUpdateTitle_Load);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGenre)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrackNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAlbumArtist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAlbum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbArtist)).EndInit();
            this.flpButtonsRight.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlMain;
        private System.Windows.Forms.FlowLayoutPanel flpButtonsRight;
        private Halloumi.Common.Windows.Controls.Button btnCancel;
        private Halloumi.Common.Windows.Controls.Button btnOK;
        private Halloumi.Common.Windows.Controls.BeveledLine beveledLine;
        private Halloumi.Common.Windows.Controls.Panel pnlButtons;
        private Halloumi.Common.Windows.Controls.Label label2;
        private Halloumi.Common.Windows.Controls.Label label4;
        private Halloumi.Common.Windows.Controls.TextBox txtTitle;
        private Halloumi.Common.Windows.Controls.Label label3;
        private Halloumi.Common.Windows.Controls.ComboBox cmbArtist;
        private Halloumi.Common.Windows.Controls.ComboBox cmbAlbumArtist;
        private Halloumi.Common.Windows.Controls.Label label5;
        private Halloumi.Common.Windows.Controls.ComboBox cmbAlbum;
        private Halloumi.Common.Windows.Controls.Panel panel3;
        private Halloumi.Common.Windows.Controls.Panel panel1;
        private Halloumi.Common.Windows.Controls.Label label1;
        private Halloumi.Common.Windows.Controls.Panel panel2;
        private Halloumi.Common.Windows.Controls.ComboBox cmbGenre;
        private Halloumi.Common.Windows.Controls.Label label6;
        private Halloumi.Common.Windows.Controls.ComboBox cmbTrackNumber;
        private Halloumi.Common.Windows.Controls.TextBox txtFile;
        private Halloumi.Common.Windows.Controls.Label label7;
        private Halloumi.Common.Windows.Controls.CheckBox chkUpdateAuxillaryFiles;
    }
}