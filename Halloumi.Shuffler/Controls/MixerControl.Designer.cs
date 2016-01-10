namespace Halloumi.Shuffler.Controls
{
    partial class MixerControl
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
            this.pnlBackground = new Halloumi.Common.Windows.Controls.Panel();
            this.spllLeftRight = new ComponentFactory.Krypton.Toolkit.KryptonSplitContainer();
            this.trackMixerControl = new Halloumi.Shuffler.Controls.TrackMixerControl();
            this.samplerControl = new Halloumi.Shuffler.Controls.SamplerControl();
            this.pnlBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spllLeftRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spllLeftRight.Panel1)).BeginInit();
            this.spllLeftRight.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spllLeftRight.Panel2)).BeginInit();
            this.spllLeftRight.Panel2.SuspendLayout();
            this.spllLeftRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBackground.Controls.Add(this.spllLeftRight);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Padding = new System.Windows.Forms.Padding(6);
            this.pnlBackground.Size = new System.Drawing.Size(1287, 676);
            this.pnlBackground.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlBackground.TabIndex = 11;
            // 
            // spllLeftRight
            // 
            this.spllLeftRight.Cursor = System.Windows.Forms.Cursors.Default;
            this.spllLeftRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spllLeftRight.Location = new System.Drawing.Point(6, 6);
            this.spllLeftRight.Margin = new System.Windows.Forms.Padding(4);
            this.spllLeftRight.Name = "spllLeftRight";
            // 
            // spllLeftRight.Panel1
            // 
            this.spllLeftRight.Panel1.Controls.Add(this.trackMixerControl);
            // 
            // spllLeftRight.Panel2
            // 
            this.spllLeftRight.Panel2.Controls.Add(this.samplerControl);
            this.spllLeftRight.Size = new System.Drawing.Size(1275, 664);
            this.spllLeftRight.SplitterDistance = 622;
            this.spllLeftRight.TabIndex = 5;
            // 
            // trackMixerControl
            // 
            this.trackMixerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackMixerControl.Location = new System.Drawing.Point(0, 0);
            this.trackMixerControl.Margin = new System.Windows.Forms.Padding(5);
            this.trackMixerControl.Name = "trackMixerControl";
            this.trackMixerControl.Size = new System.Drawing.Size(622, 664);
            this.trackMixerControl.TabIndex = 0;
            // 
            // samplerControl
            // 
            this.samplerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.samplerControl.Location = new System.Drawing.Point(0, 0);
            this.samplerControl.Margin = new System.Windows.Forms.Padding(5);
            this.samplerControl.Name = "samplerControl";
            this.samplerControl.Size = new System.Drawing.Size(648, 664);
            this.samplerControl.TabIndex = 0;
            // 
            // MixerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlBackground);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MixerControl";
            this.Size = new System.Drawing.Size(1287, 676);
            this.pnlBackground.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spllLeftRight.Panel1)).EndInit();
            this.spllLeftRight.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spllLeftRight.Panel2)).EndInit();
            this.spllLeftRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spllLeftRight)).EndInit();
            this.spllLeftRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlBackground;
        private ComponentFactory.Krypton.Toolkit.KryptonSplitContainer spllLeftRight;
        private TrackMixerControl trackMixerControl;
        private SamplerControl samplerControl;


    }
}
