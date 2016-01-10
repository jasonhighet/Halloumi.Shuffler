using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Un4seen.Bass.AddOn.Vst;
using Halloumi.Common.Helpers;

namespace Halloumi.BassEngine
{
    public class VSTPluginConfigForm : Form
    {
        private ToolStripMenuItem mnuPresets;
        private Panel pnlEditor;
        private MenuStrip menuStrip;
        private VSTPlugin Plugin;
        private BassPlayer BassPlayer;
    
        public VSTPluginConfigForm(VSTPlugin plugin, BassPlayer bassPlayer) : base()
        {
            InitializeComponent();

            this.Plugin = plugin;
            this.BassPlayer = bassPlayer;

            this.CanClose = false;
            this.FormClosing += new FormClosingEventHandler(VSTPluginConfigForm_FormClosing);

            this.MinimizeBox = true;
            this.ShowInTaskbar = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.TopMost = true;

            var borderSize = SystemInformation.BorderSize;
            this.Width = plugin.EditorWidth + (borderSize.Width * 2) + 15;
            this.Height = plugin.EditorHeight + menuStrip.Height + (borderSize.Height * 2) + 35;

            var icon = ApplicationHelper.GetIcon();
            if (icon != null) base.Icon = icon;
            
            this.Text = plugin.Name;


            BassVst.BASS_VST_EmbedEditor(plugin.ID, this.EditorPanelHandle);
            plugin.Form = this;

            var presetNames = BassVst.BASS_VST_GetProgramNames(plugin.ID).ToList();

            int i = 0;
            foreach(var presetName in presetNames)
            {
                var existingMenuItems = mnuPresets.DropDownItems.Cast<ToolStripItem>().Select(t => t.Text).ToList();
                
                if (presetName != null && presetName.Trim() != "" && !existingMenuItems.Contains(presetName.Trim()))
                {
                    var menuItem = new ToolStripMenuItem();
                    menuItem.Text = presetName.Trim();
                    menuItem.Tag = i.ToString();
                    menuItem.Click += new System.EventHandler(mnuPreset_Click);
                    mnuPresets.DropDownItems.Add(menuItem);
                }
                
                i++;
            }
        }

        private void mnuPreset_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripDropDownItem)sender;
            var index = int.Parse(menuItem.Tag.ToString());

            this.BassPlayer.SetVSTPluginPreset(this.Plugin, index);
        }


        private void VSTPluginConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.CanClose)
            {
                e.Cancel = true;
                this.Visible = false;
            }
            else
            {
                this.BassPlayer = null;
                this.Plugin = null;
            }
        }

        public bool CanClose
        {
            get;
            set;
        }

        public IntPtr EditorPanelHandle
        {
            get { return this.pnlEditor.Handle; }
        }

        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.mnuPresets = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlEditor = new System.Windows.Forms.Panel();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPresets});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(284, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // mnuPresets
            // 
            this.mnuPresets.Name = "mnuPresets";
            this.mnuPresets.Size = new System.Drawing.Size(56, 20);
            this.mnuPresets.Text = "&Presets";
            // 
            // pnlEditor
            // 
            this.pnlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEditor.Location = new System.Drawing.Point(0, 24);
            this.pnlEditor.Name = "pnlEditor";
            this.pnlEditor.Size = new System.Drawing.Size(284, 240);
            this.pnlEditor.TabIndex = 1;
            // 
            // VSTPluginConfigForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.pnlEditor);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "VSTPluginConfigForm";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
