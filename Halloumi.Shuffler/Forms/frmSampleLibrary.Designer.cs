namespace Halloumi.Shuffler.Forms
{
    partial class frmSampleLibrary
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
            this.sampleLibraryControl = new Halloumi.Shuffler.Controls.SampleLibraryControl();
            this.SuspendLayout();
            // 
            // sampleLibraryControl
            // 
            this.sampleLibraryControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sampleLibraryControl.Location = new System.Drawing.Point(0, 0);
            this.sampleLibraryControl.Name = "sampleLibraryControl";
            this.sampleLibraryControl.Size = new System.Drawing.Size(1201, 566);
            this.sampleLibraryControl.TabIndex = 0;
            // 
            // frmSampleLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1201, 566);
            this.Controls.Add(this.sampleLibraryControl);
            this.Name = "frmSampleLibrary";
            this.Text = "frmSampleLibrary";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSampleLibrary_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Shuffler.Controls.SampleLibraryControl sampleLibraryControl;
    }
}