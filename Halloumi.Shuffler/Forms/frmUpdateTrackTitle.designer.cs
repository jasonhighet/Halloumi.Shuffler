namespace Halloumi.Shuffler.Forms
{
    partial class FrmUpdateTrackTitle
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
            this.cmbTitle = new Halloumi.Common.Windows.Controls.ComboBox();
            this.flpButtonsRight = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new Halloumi.Common.Windows.Controls.Button();
            this.btnOK = new Halloumi.Common.Windows.Controls.Button();
            this.beveledLine = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.pnlButtons = new Halloumi.Common.Windows.Controls.Panel();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTitle)).BeginInit();
            this.flpButtonsRight.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMain.Controls.Add(this.chkUpdateAuxillaryFiles);
            this.pnlMain.Controls.Add(this.cmbTitle);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlMain.Size = new System.Drawing.Size(591, 85);
            this.pnlMain.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlMain.TabIndex = 29;
            // 
            // chkUpdateAuxillaryFiles
            // 
            this.chkUpdateAuxillaryFiles.AutoSize = false;
            this.chkUpdateAuxillaryFiles.Checked = true;
            this.chkUpdateAuxillaryFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUpdateAuxillaryFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkUpdateAuxillaryFiles.Location = new System.Drawing.Point(13, 37);
            this.chkUpdateAuxillaryFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkUpdateAuxillaryFiles.Name = "chkUpdateAuxillaryFiles";
            this.chkUpdateAuxillaryFiles.Size = new System.Drawing.Size(565, 49);
            this.chkUpdateAuxillaryFiles.TabIndex = 45;
            this.chkUpdateAuxillaryFiles.Value = "True";
            this.chkUpdateAuxillaryFiles.Values.Text = "Rename in shuffler files, playlists etc.";
            // 
            // cmbTitle
            // 
            this.cmbTitle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbTitle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTitle.DropDownWidth = 264;
            this.cmbTitle.ErrorMessage = "Please select a source track";
            this.cmbTitle.ErrorProvider = null;
            this.cmbTitle.IsRequired = true;
            this.cmbTitle.Location = new System.Drawing.Point(13, 12);
            this.cmbTitle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbTitle.Name = "cmbTitle";
            this.cmbTitle.Size = new System.Drawing.Size(565, 25);
            this.cmbTitle.TabIndex = 0;
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
            this.flpButtonsRight.Size = new System.Drawing.Size(591, 53);
            this.flpButtonsRight.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(471, 7);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 38);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "&Cancel";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(360, 7);
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
            this.beveledLine.Location = new System.Drawing.Point(0, 85);
            this.beveledLine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.beveledLine.Name = "beveledLine";
            this.beveledLine.Size = new System.Drawing.Size(591, 2);
            this.beveledLine.TabIndex = 28;
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.flpButtonsRight);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 87);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(591, 53);
            this.pnlButtons.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlButtons.TabIndex = 27;
            // 
            // frmUpdateTrackTitle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 140);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.beveledLine);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmUpdateTrackTitle";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Title";
            this.Load += new System.EventHandler(this.frmUpdateTrackTitle_Load);
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbTitle)).EndInit();
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
        private Halloumi.Common.Windows.Controls.ComboBox cmbTitle;
        private Halloumi.Common.Windows.Controls.CheckBox chkUpdateAuxillaryFiles;
    }
}