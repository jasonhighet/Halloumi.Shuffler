namespace Halloumi.Shuffler.Controls
{
    partial class SkipBack
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
            this.btnRemoveLastSend = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.panel1 = new Halloumi.Common.Windows.Controls.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSkipBack = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClearSends = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblTitle = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.hdrTrackFX = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.btnSaveLastSend = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.panel3 = new Halloumi.Common.Windows.Controls.Panel();
            this.panel5 = new Halloumi.Common.Windows.Controls.Panel();
            this.chkEnableTrackFXAutomation = new Halloumi.Common.Windows.Controls.CheckBox();
            this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox1 = new Halloumi.Common.Windows.Controls.CheckBox();
            this.label1 = new Halloumi.Common.Windows.Controls.Label();
            this.rdbBeats32 = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rdbBeats16 = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rdbBeats8 = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rdbBeats4 = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hdrTrackFX)).BeginInit();
            this.hdrTrackFX.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.flowLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRemoveLastSend
            // 
            this.btnRemoveLastSend.Location = new System.Drawing.Point(366, 4);
            this.btnRemoveLastSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemoveLastSend.Name = "btnRemoveLastSend";
            this.btnRemoveLastSend.Size = new System.Drawing.Size(83, 31);
            this.btnRemoveLastSend.TabIndex = 38;
            this.btnRemoveLastSend.Values.Text = "Remove";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(1, 79);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panel1.Size = new System.Drawing.Size(564, 50);
            this.panel1.TabIndex = 85;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.btnSkipBack);
            this.flowLayoutPanel1.Controls.Add(this.checkBox1);
            this.flowLayoutPanel1.Controls.Add(this.rdbBeats32);
            this.flowLayoutPanel1.Controls.Add(this.rdbBeats16);
            this.flowLayoutPanel1.Controls.Add(this.rdbBeats8);
            this.flowLayoutPanel1.Controls.Add(this.rdbBeats4);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(7, 6);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(550, 38);
            this.flowLayoutPanel1.TabIndex = 16;
            // 
            // btnSkipBack
            // 
            this.btnSkipBack.Location = new System.Drawing.Point(372, 4);
            this.btnSkipBack.Margin = new System.Windows.Forms.Padding(4);
            this.btnSkipBack.Name = "btnSkipBack";
            this.btnSkipBack.Size = new System.Drawing.Size(174, 31);
            this.btnSkipBack.TabIndex = 37;
            this.btnSkipBack.Values.Text = "Skip Back";
            // 
            // btnClearSends
            // 
            this.btnClearSends.Location = new System.Drawing.Point(457, 4);
            this.btnClearSends.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearSends.Name = "btnClearSends";
            this.btnClearSends.Size = new System.Drawing.Size(83, 31);
            this.btnClearSends.TabIndex = 37;
            this.btnClearSends.Values.Text = "Clear";
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(76, 27);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Values.Text = "Skip Back";
            // 
            // hdrTrackFX
            // 
            this.hdrTrackFX.Controls.Add(this.lblTitle);
            this.hdrTrackFX.Dock = System.Windows.Forms.DockStyle.Top;
            this.hdrTrackFX.Location = new System.Drawing.Point(1, 1);
            this.hdrTrackFX.Margin = new System.Windows.Forms.Padding(4);
            this.hdrTrackFX.Name = "hdrTrackFX";
            this.hdrTrackFX.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderColumnCustom1;
            this.hdrTrackFX.Size = new System.Drawing.Size(564, 27);
            this.hdrTrackFX.TabIndex = 1;
            // 
            // kryptonBorderEdge1
            // 
            this.kryptonBorderEdge1.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(1, 28);
            this.kryptonBorderEdge1.Margin = new System.Windows.Forms.Padding(4);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(564, 1);
            this.kryptonBorderEdge1.Text = "kryptonBorderEdge1";
            // 
            // btnSaveLastSend
            // 
            this.btnSaveLastSend.Location = new System.Drawing.Point(275, 4);
            this.btnSaveLastSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveLastSend.Name = "btnSaveLastSend";
            this.btnSaveLastSend.Size = new System.Drawing.Size(83, 31);
            this.btnSaveLastSend.TabIndex = 36;
            this.btnSaveLastSend.Values.Text = "Save";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.kryptonBorderEdge1);
            this.panel3.Controls.Add(this.hdrTrackFX);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(1);
            this.panel3.Size = new System.Drawing.Size(566, 132);
            this.panel3.TabIndex = 77;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Controls.Add(this.chkEnableTrackFXAutomation);
            this.panel5.Controls.Add(this.flowLayoutPanel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(1, 29);
            this.panel5.Margin = new System.Windows.Forms.Padding(4);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(13, 6, 7, 6);
            this.panel5.Size = new System.Drawing.Size(564, 50);
            this.panel5.TabIndex = 82;
            // 
            // chkEnableTrackFXAutomation
            // 
            this.chkEnableTrackFXAutomation.AutoSize = false;
            this.chkEnableTrackFXAutomation.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkEnableTrackFXAutomation.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            this.chkEnableTrackFXAutomation.Location = new System.Drawing.Point(13, 6);
            this.chkEnableTrackFXAutomation.Margin = new System.Windows.Forms.Padding(4);
            this.chkEnableTrackFXAutomation.Name = "chkEnableTrackFXAutomation";
            this.chkEnableTrackFXAutomation.Size = new System.Drawing.Size(169, 38);
            this.chkEnableTrackFXAutomation.TabIndex = 41;
            this.chkEnableTrackFXAutomation.Values.Text = "Enable Automation";
            // 
            // flowLayoutPanel6
            // 
            this.flowLayoutPanel6.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel6.Controls.Add(this.btnClearSends);
            this.flowLayoutPanel6.Controls.Add(this.btnRemoveLastSend);
            this.flowLayoutPanel6.Controls.Add(this.btnSaveLastSend);
            this.flowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel6.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel6.Location = new System.Drawing.Point(13, 6);
            this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            this.flowLayoutPanel6.Size = new System.Drawing.Size(544, 38);
            this.flowLayoutPanel6.TabIndex = 16;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = false;
            this.checkBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBox1.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel;
            this.checkBox1.Location = new System.Drawing.Point(288, 4);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(20, 4, 4, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(76, 31);
            this.checkBox1.TabIndex = 42;
            this.checkBox1.Values.Text = "Return";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(29, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label1.Size = new System.Drawing.Size(55, 28);
            this.label1.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label1.TabIndex = 66;
            this.label1.Text = "Beats:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rdbBeats32
            // 
            this.rdbBeats32.Location = new System.Drawing.Point(224, 8);
            this.rdbBeats32.Margin = new System.Windows.Forms.Padding(4, 8, 4, 4);
            this.rdbBeats32.Name = "rdbBeats32";
            this.rdbBeats32.Size = new System.Drawing.Size(40, 24);
            this.rdbBeats32.TabIndex = 67;
            this.rdbBeats32.Tag = "0.5";
            this.rdbBeats32.Values.Text = "32";
            // 
            // rdbBeats16
            // 
            this.rdbBeats16.Checked = true;
            this.rdbBeats16.Location = new System.Drawing.Point(176, 8);
            this.rdbBeats16.Margin = new System.Windows.Forms.Padding(4, 8, 4, 4);
            this.rdbBeats16.Name = "rdbBeats16";
            this.rdbBeats16.Size = new System.Drawing.Size(40, 24);
            this.rdbBeats16.TabIndex = 68;
            this.rdbBeats16.Tag = "0.25";
            this.rdbBeats16.Values.Text = "16";
            // 
            // rdbBeats8
            // 
            this.rdbBeats8.Location = new System.Drawing.Point(136, 8);
            this.rdbBeats8.Margin = new System.Windows.Forms.Padding(4, 8, 4, 4);
            this.rdbBeats8.Name = "rdbBeats8";
            this.rdbBeats8.Size = new System.Drawing.Size(32, 24);
            this.rdbBeats8.TabIndex = 69;
            this.rdbBeats8.Tag = "0.125";
            this.rdbBeats8.Values.Text = "8";
            // 
            // rdbBeats4
            // 
            this.rdbBeats4.Location = new System.Drawing.Point(96, 8);
            this.rdbBeats4.Margin = new System.Windows.Forms.Padding(4, 8, 4, 4);
            this.rdbBeats4.Name = "rdbBeats4";
            this.rdbBeats4.Size = new System.Drawing.Size(32, 24);
            this.rdbBeats4.TabIndex = 70;
            this.rdbBeats4.Tag = "0.0625";
            this.rdbBeats4.Values.Text = "4";
            // 
            // SkipBack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Name = "SkipBack";
            this.Size = new System.Drawing.Size(566, 132);
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hdrTrackFX)).EndInit();
            this.hdrTrackFX.ResumeLayout(false);
            this.hdrTrackFX.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.flowLayoutPanel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRemoveLastSend;
        private Halloumi.Common.Windows.Controls.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSkipBack;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClearSends;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTitle;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel hdrTrackFX;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSaveLastSend;
        private Halloumi.Common.Windows.Controls.Panel panel3;
        private Halloumi.Common.Windows.Controls.Panel panel5;
        private Halloumi.Common.Windows.Controls.CheckBox chkEnableTrackFXAutomation;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
        private Halloumi.Common.Windows.Controls.CheckBox checkBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rdbBeats32;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rdbBeats16;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rdbBeats8;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rdbBeats4;
        private Halloumi.Common.Windows.Controls.Label label1;
    }
}
