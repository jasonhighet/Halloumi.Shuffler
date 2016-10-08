using System;
using Halloumi.Shuffler.AudioEngine;
using Halloumi.Shuffler.AudioLibrary;

namespace Halloumi.Shuffler.Forms
{
    partial class FrmModuleEditor
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
            this.fileMenuController = new Halloumi.Common.Windows.Controllers.FileMenuController(this.components);
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.kryptonCheckSet = new ComponentFactory.Krypton.Toolkit.KryptonCheckSet(this.components);
            this.beveledLine1 = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.pnlHeader = new Halloumi.Common.Windows.Controls.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSamples = new ComponentFactory.Krypton.Toolkit.KryptonCheckButton();
            this.btnSong = new ComponentFactory.Krypton.Toolkit.KryptonCheckButton();
            this.btnSequences = new ComponentFactory.Krypton.Toolkit.KryptonCheckButton();
            this.panel1 = new Halloumi.Common.Windows.Controls.Panel();
            this.samplesControl = new Halloumi.Shuffler.Controls.ModulePlayerControls.SamplesControl();
            this.songControl = new Halloumi.Shuffler.Controls.ModulePlayerControls.SongControl();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonCheckSet)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileMenuController
            // 
            this.fileMenuController.FileFilter = "Shuffler Modules (*.json)|*.json";
            this.fileMenuController.FileMenu = this.mnuFile;
            this.fileMenuController.SaveDocument += new Halloumi.Common.Windows.Controllers.FileMenuControllerEventHandler(this.fileMenuController_SaveDocument);
            this.fileMenuController.LoadDocument += new Halloumi.Common.Windows.Controllers.FileMenuControllerEventHandler(this.fileMenuController_LoadDocument);
            this.fileMenuController.NewDocument += new Halloumi.Common.Windows.Controllers.FileMenuControllerEventHandler(this.fileMenuController_NewDocument);
            // 
            // mnuFile
            // 
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(44, 24);
            this.mnuFile.Text = "&File";
            // 
            // menuStrip
            // 
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(833, 28);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // kryptonCheckSet
            // 
            this.kryptonCheckSet.CheckButtons.Add(this.btnSamples);
            this.kryptonCheckSet.CheckButtons.Add(this.btnSong);
            this.kryptonCheckSet.CheckButtons.Add(this.btnSequences);
            this.kryptonCheckSet.CheckedButton = this.btnSamples;
            // 
            // beveledLine1
            // 
            this.beveledLine1.Dock = System.Windows.Forms.DockStyle.Top;
            this.beveledLine1.Location = new System.Drawing.Point(0, 28);
            this.beveledLine1.Name = "beveledLine1";
            this.beveledLine1.Size = new System.Drawing.Size(833, 2);
            this.beveledLine1.TabIndex = 4;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHeader.Controls.Add(this.flowLayoutPanel1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 30);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(833, 44);
            this.pnlHeader.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlHeader.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnSamples);
            this.flowLayoutPanel1.Controls.Add(this.btnSong);
            this.flowLayoutPanel1.Controls.Add(this.btnSequences);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(833, 44);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnSamples
            // 
            this.btnSamples.Checked = true;
            this.btnSamples.Location = new System.Drawing.Point(3, 3);
            this.btnSamples.Name = "btnSamples";
            this.btnSamples.Size = new System.Drawing.Size(90, 35);
            this.btnSamples.TabIndex = 0;
            this.btnSamples.Values.Text = "Samples";
            this.btnSamples.Click += new System.EventHandler(this.btnSamples_Click);
            // 
            // btnSong
            // 
            this.btnSong.Location = new System.Drawing.Point(99, 3);
            this.btnSong.Name = "btnSong";
            this.btnSong.Size = new System.Drawing.Size(90, 35);
            this.btnSong.TabIndex = 1;
            this.btnSong.Values.Text = "Song";
            this.btnSong.Click += new System.EventHandler(this.btnSong_Click);
            // 
            // btnSequences
            // 
            this.btnSequences.Location = new System.Drawing.Point(195, 3);
            this.btnSequences.Name = "btnSequences";
            this.btnSequences.Size = new System.Drawing.Size(90, 35);
            this.btnSequences.TabIndex = 2;
            this.btnSequences.Values.Text = "Sequences";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.songControl);
            this.panel1.Controls.Add(this.samplesControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 74);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(833, 492);
            this.panel1.Style = Halloumi.Common.Windows.Controls.PanelStyle.Background;
            this.panel1.TabIndex = 7;
            // 
            // samplesControl
            // 
            this.samplesControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.samplesControl.Location = new System.Drawing.Point(3, 3);
            this.samplesControl.Name = "samplesControl";
            this.samplesControl.Size = new System.Drawing.Size(827, 486);
            this.samplesControl.TabIndex = 7;
            // 
            // songControl
            // 
            this.songControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songControl.Location = new System.Drawing.Point(3, 3);
            this.songControl.Name = "songControl";
            this.songControl.Size = new System.Drawing.Size(827, 486);
            this.songControl.TabIndex = 8;
            // 
            // FrmModuleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 566);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.beveledLine1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FrmModuleEditor";
            this.Text = "frmModulePlayer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmModuleEditor_FormClosed);
            this.Load += new System.EventHandler(this.FrmModuleEditor_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonCheckSet)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Common.Windows.Controllers.FileMenuController fileMenuController;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckSet kryptonCheckSet;
        private Common.Windows.Controls.BeveledLine beveledLine1;
        private Common.Windows.Controls.Panel pnlHeader;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckButton btnSamples;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckButton btnSong;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckButton btnSequences;
        private Common.Windows.Controls.Panel panel1;
        private Controls.ModulePlayerControls.SongControl songControl;
        private Controls.ModulePlayerControls.SamplesControl samplesControl;
    }
}