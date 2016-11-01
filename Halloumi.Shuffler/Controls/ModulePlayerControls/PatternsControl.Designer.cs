﻿using System;

namespace Halloumi.Shuffler.Controls.ModulePlayerControls
{
    partial class PatternsControl
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
            this.panel3 = new Halloumi.Common.Windows.Controls.Panel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPlay = new Halloumi.Common.Windows.Controls.Button();
            this.btnPlayChannel = new Halloumi.Common.Windows.Controls.Button();
            this.btnStop = new Halloumi.Common.Windows.Controls.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listBuilder = new Halloumi.Shuffler.Controls.ListBuilder();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbChannel = new Halloumi.Common.Windows.Controls.ComboBox();
            this.btnAddChannel = new Halloumi.Common.Windows.Controls.Button();
            this.btnDeleteChannel = new Halloumi.Common.Windows.Controls.Button();
            this.panel1 = new Halloumi.Common.Windows.Controls.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new Halloumi.Common.Windows.Controls.Label();
            this.cmbPattern = new Halloumi.Common.Windows.Controls.ComboBox();
            this.btnAddPattern = new Halloumi.Common.Windows.Controls.Button();
            this.btnDeletePattern = new Halloumi.Common.Windows.Controls.Button();
            this.btnPlugins = new Halloumi.Common.Windows.Controls.Button();
            this.panel3.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbChannel)).BeginInit();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPattern)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.flowLayoutPanel3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(775, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(109, 413);
            this.panel3.Style = Halloumi.Common.Windows.Controls.PanelStyle.Background;
            this.panel3.TabIndex = 5;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.btnPlay);
            this.flowLayoutPanel3.Controls.Add(this.btnPlayChannel);
            this.flowLayoutPanel3.Controls.Add(this.btnStop);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.flowLayoutPanel3.Size = new System.Drawing.Size(109, 413);
            this.flowLayoutPanel3.TabIndex = 0;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(4, 9);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(100, 31);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "Play";
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnPlayChannel
            // 
            this.btnPlayChannel.Location = new System.Drawing.Point(4, 48);
            this.btnPlayChannel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPlayChannel.Name = "btnPlayChannel";
            this.btnPlayChannel.Size = new System.Drawing.Size(100, 31);
            this.btnPlayChannel.TabIndex = 3;
            this.btnPlayChannel.Text = "Play Channel";
            this.btnPlayChannel.Click += new System.EventHandler(this.btnPlayChannel_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(4, 87);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 31);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listBuilder);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 79);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(775, 334);
            this.panel2.TabIndex = 2;
            // 
            // listBuilder
            // 
            this.listBuilder.AllowMultipleAvailableItems = false;
            this.listBuilder.BackColor = System.Drawing.Color.Transparent;
            this.listBuilder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBuilder.Location = new System.Drawing.Point(5, 5);
            this.listBuilder.Name = "listBuilder";
            this.listBuilder.PropertiesButtonVisible = false;
            this.listBuilder.Size = new System.Drawing.Size(765, 324);
            this.listBuilder.TabIndex = 0;
            this.listBuilder.SelectedItemsChanged += new System.EventHandler(this.listBuilder_OnDestinationListChanged);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Controls.Add(this.cmbChannel);
            this.flowLayoutPanel2.Controls.Add(this.btnAddChannel);
            this.flowLayoutPanel2.Controls.Add(this.btnDeleteChannel);
            this.flowLayoutPanel2.Controls.Add(this.btnPlugins);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 39);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(775, 40);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.label2.Size = new System.Drawing.Size(71, 24);
            this.label2.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label2.TabIndex = 0;
            this.label2.Text = "Channel:";
            // 
            // cmbChannel
            // 
            this.cmbChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChannel.DropDownWidth = 121;
            this.cmbChannel.ErrorProvider = null;
            this.cmbChannel.Items.AddRange(new object[] {
            "",
            "Yes",
            "No"});
            this.cmbChannel.Location = new System.Drawing.Point(81, 4);
            this.cmbChannel.Margin = new System.Windows.Forms.Padding(4);
            this.cmbChannel.Name = "cmbChannel";
            this.cmbChannel.Size = new System.Drawing.Size(327, 25);
            this.cmbChannel.TabIndex = 7;
            this.cmbChannel.SelectedIndexChanged += new System.EventHandler(this.cmbChannel_SelectedIndexChanged);
            // 
            // btnAddChannel
            // 
            this.btnAddChannel.Location = new System.Drawing.Point(416, 4);
            this.btnAddChannel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddChannel.Name = "btnAddChannel";
            this.btnAddChannel.Size = new System.Drawing.Size(100, 31);
            this.btnAddChannel.TabIndex = 8;
            this.btnAddChannel.Text = "Add";
            this.btnAddChannel.Click += new System.EventHandler(this.btnAddChannel_Click);
            // 
            // btnDeleteChannel
            // 
            this.btnDeleteChannel.Location = new System.Drawing.Point(524, 4);
            this.btnDeleteChannel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDeleteChannel.Name = "btnDeleteChannel";
            this.btnDeleteChannel.Size = new System.Drawing.Size(100, 31);
            this.btnDeleteChannel.TabIndex = 9;
            this.btnDeleteChannel.Text = "Delete";
            this.btnDeleteChannel.Click += new System.EventHandler(this.btnDeleteChannel_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.flowLayoutPanel2);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(775, 413);
            this.panel1.Style = Halloumi.Common.Windows.Controls.PanelStyle.Background;
            this.panel1.TabIndex = 6;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.cmbPattern);
            this.flowLayoutPanel1.Controls.Add(this.btnAddPattern);
            this.flowLayoutPanel1.Controls.Add(this.btnDeletePattern);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(775, 39);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.label1.Size = new System.Drawing.Size(71, 24);
            this.label1.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.label1.TabIndex = 0;
            this.label1.Text = "Pattern:";
            // 
            // cmbPattern
            // 
            this.cmbPattern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPattern.DropDownWidth = 121;
            this.cmbPattern.ErrorProvider = null;
            this.cmbPattern.Items.AddRange(new object[] {
            "",
            "Yes",
            "No"});
            this.cmbPattern.Location = new System.Drawing.Point(81, 4);
            this.cmbPattern.Margin = new System.Windows.Forms.Padding(4);
            this.cmbPattern.Name = "cmbPattern";
            this.cmbPattern.Size = new System.Drawing.Size(327, 25);
            this.cmbPattern.TabIndex = 7;
            this.cmbPattern.SelectedIndexChanged += new System.EventHandler(this.cmbPattern_SelectedIndexChanged);
            // 
            // btnAddPattern
            // 
            this.btnAddPattern.Location = new System.Drawing.Point(416, 4);
            this.btnAddPattern.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddPattern.Name = "btnAddPattern";
            this.btnAddPattern.Size = new System.Drawing.Size(100, 31);
            this.btnAddPattern.TabIndex = 8;
            this.btnAddPattern.Text = "Add";
            this.btnAddPattern.Click += new System.EventHandler(this.btnAddPattern_Click);
            // 
            // btnDeletePattern
            // 
            this.btnDeletePattern.Location = new System.Drawing.Point(524, 4);
            this.btnDeletePattern.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDeletePattern.Name = "btnDeletePattern";
            this.btnDeletePattern.Size = new System.Drawing.Size(100, 31);
            this.btnDeletePattern.TabIndex = 9;
            this.btnDeletePattern.Text = "Delete";
            this.btnDeletePattern.Click += new System.EventHandler(this.btnDeletePattern_Click);
            // 
            // btnPlugins
            // 
            this.btnPlugins.Location = new System.Drawing.Point(632, 4);
            this.btnPlugins.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPlugins.Name = "btnPlugins";
            this.btnPlugins.Size = new System.Drawing.Size(100, 31);
            this.btnPlugins.TabIndex = 10;
            this.btnPlugins.Text = "Plugins";
            this.btnPlugins.Click += new System.EventHandler(this.btnPlugins_Click);
            // 
            // PatternsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Name = "PatternsControl";
            this.Size = new System.Drawing.Size(884, 413);
            this.panel3.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbChannel)).EndInit();
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbPattern)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Windows.Controls.Panel panel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private Common.Windows.Controls.Button btnPlay;
        private Common.Windows.Controls.Button btnStop;
        private System.Windows.Forms.Panel panel2;
        private ListBuilder listBuilder;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private Common.Windows.Controls.Label label2;
        private Common.Windows.Controls.ComboBox cmbChannel;
        private Common.Windows.Controls.Button btnAddChannel;
        private Common.Windows.Controls.Button btnDeleteChannel;
        private Common.Windows.Controls.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Common.Windows.Controls.Label label1;
        private Common.Windows.Controls.ComboBox cmbPattern;
        private Common.Windows.Controls.Button btnAddPattern;
        private Common.Windows.Controls.Button btnDeletePattern;
        private Common.Windows.Controls.Button btnPlayChannel;
        private Common.Windows.Controls.Button btnPlugins;
    }
}
