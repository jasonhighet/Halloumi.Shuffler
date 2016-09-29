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
            this.samplesControl = new Halloumi.Shuffler.Controls.ModulePlayerControls.SamplesControl();
            this.SuspendLayout();
            // 
            // samplesControl1
            // 
            this.samplesControl.BassPlayer = null;
            this.samplesControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.samplesControl.Location = new System.Drawing.Point(0, 0);
            this.samplesControl.ModulePlayer = null;
            this.samplesControl.Name = "samplesControl1";
            this.samplesControl.SampleLibrary = null;
            this.samplesControl.Size = new System.Drawing.Size(833, 566);
            this.samplesControl.TabIndex = 0;
            // 
            // frmModuleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 566);
            this.Controls.Add(this.samplesControl);
            this.Name = "FrmModuleEditor";
            this.Text = "frmModulePlayer";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ModulePlayerControls.SamplesControl samplesControl;

    }
}