namespace Halloumi.Shuffler.Controls
{
    partial class VolumeLevel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlBackground = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.pnlLevel = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBackground)).BeginInit();
            this.pnlBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBackground.Controls.Add(this.pnlLevel);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(198, 18);
            this.pnlBackground.TabIndex = 1;
            // 
            // pnlLevel
            // 
            this.pnlLevel.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLevel.Location = new System.Drawing.Point(0, 0);
            this.pnlLevel.Name = "pnlLevel";
            this.pnlLevel.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.HeaderDockActive;
            this.pnlLevel.Size = new System.Drawing.Size(100, 16);
            this.pnlLevel.TabIndex = 2;
            // 
            // VolumeLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pnlBackground);
            this.Name = "VolumeLevel";
            this.Size = new System.Drawing.Size(198, 18);
            ((System.ComponentModel.ISupportInitialize)(this.pnlBackground)).EndInit();
            this.pnlBackground.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlLevel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPanel pnlBackground;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel pnlLevel;
    }
}
