using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Halloumi.BassEngine.Plugins;
using Un4seen.Bass.AddOn.Vst;
using Halloumi.Common.Helpers;

namespace Halloumi.BassEngine
{
    public class VstPluginConfigForm : Form
    {
        private ToolStripMenuItem _mnuPresets;
        private Panel _pnlEditor;
        private MenuStrip _menuStrip;
        private VstPlugin _plugin;
        private BassPlayer _bassPlayer;
    
        public VstPluginConfigForm(VstPlugin plugin, BassPlayer bassPlayer) : base()
        {
            InitializeComponent();

            this._plugin = plugin;
            this._bassPlayer = bassPlayer;

            this.CanClose = false;
            this.FormClosing += new FormClosingEventHandler(VSTPluginConfigForm_FormClosing);

            this.MinimizeBox = true;
            this.ShowInTaskbar = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.TopMost = true;

            var borderSize = SystemInformation.BorderSize;
            this.Width = plugin.EditorWidth + (borderSize.Width * 2) + 15;
            this.Height = plugin.EditorHeight + _menuStrip.Height + (borderSize.Height * 2) + 35;

            var icon = ApplicationHelper.GetIcon();
            if (icon != null) base.Icon = icon;
            
            this.Text = plugin.Name;


            BassVst.BASS_VST_EmbedEditor(plugin.Id, this.EditorPanelHandle);
            plugin.Form = this;

            var presetNames = BassVst.BASS_VST_GetProgramNames(plugin.Id).ToList();

            var i = 0;
            foreach(var presetName in presetNames)
            {
                var existingMenuItems = _mnuPresets.DropDownItems.Cast<ToolStripItem>().Select(t => t.Text).ToList();
                
                if (presetName != null && presetName.Trim() != "" && !existingMenuItems.Contains(presetName.Trim()))
                {
                    var menuItem = new ToolStripMenuItem();
                    menuItem.Text = presetName.Trim();
                    menuItem.Tag = i.ToString();
                    menuItem.Click += new System.EventHandler(mnuPreset_Click);
                    _mnuPresets.DropDownItems.Add(menuItem);
                }
                
                i++;
            }
        }

        private void mnuPreset_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripDropDownItem)sender;
            var index = int.Parse(menuItem.Tag.ToString());

            this._bassPlayer.SetVstPluginPreset(this._plugin, index);
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
                this._bassPlayer = null;
                this._plugin = null;
            }
        }

        public bool CanClose
        {
            get;
            set;
        }

        public IntPtr EditorPanelHandle
        {
            get { return this._pnlEditor.Handle; }
        }

        private void InitializeComponent()
        {
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this._mnuPresets = new System.Windows.Forms.ToolStripMenuItem();
            this._pnlEditor = new System.Windows.Forms.Panel();
            this._menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mnuPresets});
            this._menuStrip.Location = new System.Drawing.Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Size = new System.Drawing.Size(284, 24);
            this._menuStrip.TabIndex = 0;
            this._menuStrip.Text = "menuStrip1";
            // 
            // mnuPresets
            // 
            this._mnuPresets.Name = "_mnuPresets";
            this._mnuPresets.Size = new System.Drawing.Size(56, 20);
            this._mnuPresets.Text = "&Presets";
            // 
            // pnlEditor
            // 
            this._pnlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlEditor.Location = new System.Drawing.Point(0, 24);
            this._pnlEditor.Name = "_pnlEditor";
            this._pnlEditor.Size = new System.Drawing.Size(284, 240);
            this._pnlEditor.TabIndex = 1;
            // 
            // VSTPluginConfigForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this._pnlEditor);
            this.Controls.Add(this._menuStrip);
            this.MainMenuStrip = this._menuStrip;
            this.Name = "VstPluginConfigForm";
            this._menuStrip.ResumeLayout(false);
            this._menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
