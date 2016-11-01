using Halloumi.Shuffler.Controls;

namespace Halloumi.Shuffler.Forms
{
    partial class FrmVstChain
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
            this.listBuilder = new Halloumi.Shuffler.Controls.ListBuilder();
            this.SuspendLayout();
            // 
            // listBuilder
            // 
            this.listBuilder.AllowMultipleAvailableItems = false;
            this.listBuilder.BackColor = System.Drawing.Color.Transparent;
            this.listBuilder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBuilder.Location = new System.Drawing.Point(0, 0);
            this.listBuilder.Name = "listBuilder";
            this.listBuilder.PropertiesButtonVisible = false;
            this.listBuilder.Size = new System.Drawing.Size(485, 384);
            this.listBuilder.TabIndex = 0;
            this.listBuilder.SelectedItemsChanged += new System.EventHandler(this.listBuilder_SelectedItemsChanged);
            this.listBuilder.PropertiesClicked += new System.EventHandler(this.listBuilder_PropertiesClicked);
            // 
            // FrmVstChain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 384);
            this.Controls.Add(this.listBuilder);
            this.Name = "FrmVstChain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Falafel : Shuffler : VST Chain";
            this.ResumeLayout(false);

        }

        #endregion

        private ListBuilder listBuilder;
    }
}