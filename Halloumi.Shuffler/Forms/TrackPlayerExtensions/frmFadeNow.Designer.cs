namespace Halloumi.Shuffler.Forms.TrackPlayerExtensions
{
    partial class frmFadeNow
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
            this.panel3 = new Halloumi.Common.Windows.Controls.Panel();
            this.panel1 = new Halloumi.Common.Windows.Controls.Panel();
            this.btnFadeNow = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbFadeType = new Halloumi.Common.Windows.Controls.ComboBox();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFadeType)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(1);
            this.panel3.Size = new System.Drawing.Size(373, 50);
            this.panel3.TabIndex = 77;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panel1.Size = new System.Drawing.Size(371, 51);
            this.panel1.TabIndex = 85;
            // 
            // btnFadeNow
            // 
            this.btnFadeNow.Location = new System.Drawing.Point(245, 4);
            this.btnFadeNow.Margin = new System.Windows.Forms.Padding(4);
            this.btnFadeNow.Name = "btnFadeNow";
            this.btnFadeNow.Size = new System.Drawing.Size(114, 31);
            this.btnFadeNow.TabIndex = 38;
            this.btnFadeNow.Values.Text = "Fade Now";
            this.btnFadeNow.Click += new System.EventHandler(this.btnFadeNow_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.cmbFadeType);
            this.flowLayoutPanel1.Controls.Add(this.btnFadeNow);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(7, 6);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(368, 39);
            this.flowLayoutPanel1.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(4, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 31);
            this.label2.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label2.TabIndex = 39;
            this.label2.Text = "Fade Type:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbFadeType
            // 
            this.cmbFadeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFadeType.DropDownWidth = 163;
            this.cmbFadeType.ErrorProvider = null;
            this.cmbFadeType.Items.AddRange(new object[] {
            "Skip To End",
            "Power Down",
            "Cut",
            "Quick Fade"});
            this.cmbFadeType.Location = new System.Drawing.Point(103, 3);
            this.cmbFadeType.Name = "cmbFadeType";
            this.cmbFadeType.Size = new System.Drawing.Size(135, 25);
            this.cmbFadeType.TabIndex = 38;
            // 
            // frmFadeNow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 50);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmFadeNow";
            this.Text = "Fade Now";
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbFadeType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel panel3;
        private Halloumi.Common.Windows.Controls.Panel panel1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnFadeNow;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Halloumi.Common.Windows.Controls.Label label2;
        private Halloumi.Common.Windows.Controls.ComboBox cmbFadeType;
    }
}