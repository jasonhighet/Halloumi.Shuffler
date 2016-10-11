namespace Halloumi.Shuffler.TestHarness
{
    partial class Form1
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
            this.listBuilder1 = new Halloumi.Shuffler.TestHarness.ListBuilder();
            this.SuspendLayout();
            // 
            // listBuilder1
            // 
            this.listBuilder1.AllowMultipleSourceItemsInDestination = false;
            this.listBuilder1.BackColor = System.Drawing.Color.Transparent;
            this.listBuilder1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBuilder1.Location = new System.Drawing.Point(0, 0);
            this.listBuilder1.Name = "listBuilder1";
            this.listBuilder1.Size = new System.Drawing.Size(934, 402);
            this.listBuilder1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 402);
            this.Controls.Add(this.listBuilder1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ListBuilder listBuilder1;
    }
}

