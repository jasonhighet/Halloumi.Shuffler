namespace Halloumi.Shuffler.Controls
{
    partial class TrackWave
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblViewDetails = new Halloumi.Common.Windows.Controls.Label();
            this.flpPlaybackButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPlay = new Halloumi.Common.Windows.Controls.Button();
            this.btnStop = new Halloumi.Common.Windows.Controls.Button();
            this.btnEdit = new Halloumi.Common.Windows.Controls.Button();
            this.flpZoomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnZoomFull = new Halloumi.Common.Windows.Controls.Button();
            this.btnZoomOut = new Halloumi.Common.Windows.Controls.Button();
            this.btnZoomIn = new Halloumi.Common.Windows.Controls.Button();
            this.scrollBar = new System.Windows.Forms.HScrollBar();
            this.picWaveForm = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuSetPreFadeInStart = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetFadeInStart = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetFadeInEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetFadeOutStart = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetFadeOutEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetSkipStart = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetSkipEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetSampleStart = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetSampleEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetSampleOffset = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.flpPlaybackButtons.SuspendLayout();
            this.flpZoomButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWaveForm)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblViewDetails);
            this.panel1.Controls.Add(this.flpPlaybackButtons);
            this.panel1.Controls.Add(this.flpZoomButtons);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 175);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1048, 47);
            this.panel1.TabIndex = 18;
            // 
            // lblViewDetails
            // 
            this.lblViewDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblViewDetails.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblViewDetails.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblViewDetails.Location = new System.Drawing.Point(317, 0);
            this.lblViewDetails.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblViewDetails.Name = "lblViewDetails";
            this.lblViewDetails.Size = new System.Drawing.Size(399, 47);
            this.lblViewDetails.TabIndex = 27;
            this.lblViewDetails.Text = "View: 1:00 to 2:05 (1:05)      Cursor: 1:00";
            this.lblViewDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flpPlaybackButtons
            // 
            this.flpPlaybackButtons.Controls.Add(this.btnPlay);
            this.flpPlaybackButtons.Controls.Add(this.btnStop);
            this.flpPlaybackButtons.Controls.Add(this.btnEdit);
            this.flpPlaybackButtons.Dock = System.Windows.Forms.DockStyle.Left;
            this.flpPlaybackButtons.Location = new System.Drawing.Point(0, 0);
            this.flpPlaybackButtons.Margin = new System.Windows.Forms.Padding(4);
            this.flpPlaybackButtons.Name = "flpPlaybackButtons";
            this.flpPlaybackButtons.Size = new System.Drawing.Size(317, 47);
            this.flpPlaybackButtons.TabIndex = 26;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(5, 5);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(5);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(93, 38);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "Play";
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(108, 5);
            this.btnStop.Margin = new System.Windows.Forms.Padding(5);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(93, 38);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(211, 5);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(5);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(93, 38);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // flpZoomButtons
            // 
            this.flpZoomButtons.Controls.Add(this.btnZoomFull);
            this.flpZoomButtons.Controls.Add(this.btnZoomOut);
            this.flpZoomButtons.Controls.Add(this.btnZoomIn);
            this.flpZoomButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flpZoomButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpZoomButtons.Location = new System.Drawing.Point(716, 0);
            this.flpZoomButtons.Margin = new System.Windows.Forms.Padding(4);
            this.flpZoomButtons.Name = "flpZoomButtons";
            this.flpZoomButtons.Size = new System.Drawing.Size(332, 47);
            this.flpZoomButtons.TabIndex = 25;
            // 
            // btnZoomFull
            // 
            this.btnZoomFull.Location = new System.Drawing.Point(234, 5);
            this.btnZoomFull.Margin = new System.Windows.Forms.Padding(5);
            this.btnZoomFull.Name = "btnZoomFull";
            this.btnZoomFull.Size = new System.Drawing.Size(93, 38);
            this.btnZoomFull.TabIndex = 2;
            this.btnZoomFull.Text = "Zoom Full";
            this.btnZoomFull.Click += new System.EventHandler(this.btnZoomFull_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Location = new System.Drawing.Point(131, 5);
            this.btnZoomOut.Margin = new System.Windows.Forms.Padding(5);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(93, 38);
            this.btnZoomOut.TabIndex = 1;
            this.btnZoomOut.Text = "Zoom Out";
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Location = new System.Drawing.Point(28, 5);
            this.btnZoomIn.Margin = new System.Windows.Forms.Padding(5);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(93, 38);
            this.btnZoomIn.TabIndex = 0;
            this.btnZoomIn.Text = "Zoom In";
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // scrollBar
            // 
            this.scrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.scrollBar.Location = new System.Drawing.Point(0, 158);
            this.scrollBar.Name = "scrollBar";
            this.scrollBar.Size = new System.Drawing.Size(1048, 17);
            this.scrollBar.TabIndex = 22;
            this.scrollBar.ValueChanged += new System.EventHandler(this.scrollBar_ValueChanged);
            // 
            // picWaveForm
            // 
            this.picWaveForm.BackColor = System.Drawing.Color.WhiteSmoke;
            this.picWaveForm.ContextMenuStrip = this.contextMenuStrip;
            this.picWaveForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picWaveForm.ErrorImage = null;
            this.picWaveForm.InitialImage = null;
            this.picWaveForm.Location = new System.Drawing.Point(0, 0);
            this.picWaveForm.Margin = new System.Windows.Forms.Padding(4);
            this.picWaveForm.Name = "picWaveForm";
            this.picWaveForm.Size = new System.Drawing.Size(1048, 158);
            this.picWaveForm.TabIndex = 23;
            this.picWaveForm.TabStop = false;
            this.picWaveForm.DoubleClick += new System.EventHandler(this.picWaveForm_DoubleClick);
            this.picWaveForm.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picWaveForm_MouseUp);
            this.picWaveForm.Resize += new System.EventHandler(this.picWaveForm_Resize);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSetPreFadeInStart,
            this.mnuSetFadeInStart,
            this.mnuSetFadeInEnd,
            this.mnuSetFadeOutStart,
            this.mnuSetFadeOutEnd,
            this.mnuSetSkipStart,
            this.mnuSetSkipEnd,
            this.mnuSetSampleStart,
            this.mnuSetSampleEnd,
            this.mnuSetSampleOffset});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(226, 264);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // mnuSetPreFadeInStart
            // 
            this.mnuSetPreFadeInStart.Name = "mnuSetPreFadeInStart";
            this.mnuSetPreFadeInStart.Size = new System.Drawing.Size(225, 26);
            this.mnuSetPreFadeInStart.Text = "Set Pre-Fade-In Start";
            this.mnuSetPreFadeInStart.Click += new System.EventHandler(this.mnuSetPreFadeInStart_Click);
            // 
            // mnuSetFadeInStart
            // 
            this.mnuSetFadeInStart.Name = "mnuSetFadeInStart";
            this.mnuSetFadeInStart.Size = new System.Drawing.Size(225, 26);
            this.mnuSetFadeInStart.Text = "Set Fade-In Start";
            this.mnuSetFadeInStart.Click += new System.EventHandler(this.mnuSetFadeInStart_Click);
            // 
            // mnuSetFadeInEnd
            // 
            this.mnuSetFadeInEnd.Name = "mnuSetFadeInEnd";
            this.mnuSetFadeInEnd.Size = new System.Drawing.Size(225, 26);
            this.mnuSetFadeInEnd.Text = "Set Fade-In End";
            this.mnuSetFadeInEnd.Click += new System.EventHandler(this.mnuSetFadeInEnd_Click);
            // 
            // mnuSetFadeOutStart
            // 
            this.mnuSetFadeOutStart.Name = "mnuSetFadeOutStart";
            this.mnuSetFadeOutStart.Size = new System.Drawing.Size(225, 26);
            this.mnuSetFadeOutStart.Text = "Set Fade-Out Start";
            this.mnuSetFadeOutStart.Click += new System.EventHandler(this.mnuSetFadeOutStart_Click);
            // 
            // mnuSetFadeOutEnd
            // 
            this.mnuSetFadeOutEnd.Name = "mnuSetFadeOutEnd";
            this.mnuSetFadeOutEnd.Size = new System.Drawing.Size(225, 26);
            this.mnuSetFadeOutEnd.Text = "Set Fade-Out End";
            this.mnuSetFadeOutEnd.Click += new System.EventHandler(this.mnuSetFadeOutEnd_Click);
            // 
            // mnuSetSkipStart
            // 
            this.mnuSetSkipStart.Name = "mnuSetSkipStart";
            this.mnuSetSkipStart.Size = new System.Drawing.Size(225, 26);
            this.mnuSetSkipStart.Text = "Set Skip Section Start";
            this.mnuSetSkipStart.Click += new System.EventHandler(this.mnuSetSkipStart_Click);
            // 
            // mnuSetSkipEnd
            // 
            this.mnuSetSkipEnd.Name = "mnuSetSkipEnd";
            this.mnuSetSkipEnd.Size = new System.Drawing.Size(225, 26);
            this.mnuSetSkipEnd.Text = "Set Skip Section End";
            this.mnuSetSkipEnd.Click += new System.EventHandler(this.mnuSetSkipEnd_Click);
            // 
            // mnuSetSampleStart
            // 
            this.mnuSetSampleStart.Name = "mnuSetSampleStart";
            this.mnuSetSampleStart.Size = new System.Drawing.Size(225, 26);
            this.mnuSetSampleStart.Text = "Set Sample Start";
            this.mnuSetSampleStart.Click += new System.EventHandler(this.mnuSetSampleStart_Click);
            // 
            // mnuSetSampleEnd
            // 
            this.mnuSetSampleEnd.Name = "mnuSetSampleEnd";
            this.mnuSetSampleEnd.Size = new System.Drawing.Size(225, 26);
            this.mnuSetSampleEnd.Text = "Set Sample End";
            this.mnuSetSampleEnd.Click += new System.EventHandler(this.mnuSetSampleEnd_Click);
            // 
            // mnuSetSampleOffset
            // 
            this.mnuSetSampleOffset.Name = "mnuSetSampleOffset";
            this.mnuSetSampleOffset.Size = new System.Drawing.Size(225, 26);
            this.mnuSetSampleOffset.Text = "Set Sample Offset";
            this.mnuSetSampleOffset.Click += new System.EventHandler(this.mnuSetSampleOffset_Click);
            // 
            // TrackWave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picWaveForm);
            this.Controls.Add(this.scrollBar);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TrackWave";
            this.Size = new System.Drawing.Size(1048, 222);
            this.panel1.ResumeLayout(false);
            this.flpPlaybackButtons.ResumeLayout(false);
            this.flpZoomButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWaveForm)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.HScrollBar scrollBar;
        private System.Windows.Forms.PictureBox picWaveForm;
        private System.Windows.Forms.FlowLayoutPanel flpZoomButtons;
        private Halloumi.Common.Windows.Controls.Button btnZoomFull;
        private Halloumi.Common.Windows.Controls.Button btnZoomOut;
        private Halloumi.Common.Windows.Controls.Button btnZoomIn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuSetPreFadeInStart;
        private System.Windows.Forms.ToolStripMenuItem mnuSetFadeInStart;
        private System.Windows.Forms.ToolStripMenuItem mnuSetFadeInEnd;
        private System.Windows.Forms.ToolStripMenuItem mnuSetFadeOutStart;
        private System.Windows.Forms.ToolStripMenuItem mnuSetFadeOutEnd;
        private System.Windows.Forms.FlowLayoutPanel flpPlaybackButtons;
        private Halloumi.Common.Windows.Controls.Button btnPlay;
        private Halloumi.Common.Windows.Controls.Button btnStop;
        private Halloumi.Common.Windows.Controls.Button btnEdit;
        private Halloumi.Common.Windows.Controls.Label lblViewDetails;
        private System.Windows.Forms.ToolStripMenuItem mnuSetSkipStart;
        private System.Windows.Forms.ToolStripMenuItem mnuSetSkipEnd;
        private System.Windows.Forms.ToolStripMenuItem mnuSetSampleStart;
        private System.Windows.Forms.ToolStripMenuItem mnuSetSampleEnd;
        private System.Windows.Forms.ToolStripMenuItem mnuSetSampleOffset;

    }
}