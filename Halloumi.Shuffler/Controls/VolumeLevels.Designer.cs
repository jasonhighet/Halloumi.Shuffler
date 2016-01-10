namespace Halloumi.Shuffler.Controls
{
    partial class VolumeLevels
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
            this.volLeft = new Halloumi.Shuffler.Controls.VolumeLevel();
            this.volRight = new Halloumi.Shuffler.Controls.VolumeLevel();
            this.SuspendLayout();
            // 
            // volLeft
            // 
            this.volLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.volLeft.Dock = System.Windows.Forms.DockStyle.Top;
            this.volLeft.Location = new System.Drawing.Point(0, 0);
            this.volLeft.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.volLeft.Max = 100;
            this.volLeft.Min = 0;
            this.volLeft.Name = "volLeft";
            this.volLeft.Size = new System.Drawing.Size(165, 22);
            this.volLeft.TabIndex = 0;
            this.volLeft.Value = 50;
            // 
            // volRight
            // 
            this.volRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.volRight.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.volRight.Location = new System.Drawing.Point(0, 32);
            this.volRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.volRight.Max = 100;
            this.volRight.Min = 0;
            this.volRight.Name = "volRight";
            this.volRight.Size = new System.Drawing.Size(165, 22);
            this.volRight.TabIndex = 1;
            this.volRight.Value = 50;
            // 
            // VolumeLevels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.volRight);
            this.Controls.Add(this.volLeft);
            this.Name = "VolumeLevels";
            this.Size = new System.Drawing.Size(165, 54);
            this.ResumeLayout(false);

        }

        #endregion

        private VolumeLevel volLeft;
        private VolumeLevel volRight;
    }
}
