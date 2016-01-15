using System;
using System.Linq;
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

            _plugin = plugin;
            _bassPlayer = bassPlayer;

            CanClose = false;
            FormClosing += new FormClosingEventHandler(VSTPluginConfigForm_FormClosing);

            MinimizeBox = true;
            ShowInTaskbar = true;
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            TopMost = true;

            var borderSize = SystemInformation.BorderSize;
            Width = plugin.EditorWidth + (borderSize.Width * 2) + 15;
            Height = plugin.EditorHeight + _menuStrip.Height + (borderSize.Height * 2) + 35;

            var icon = ApplicationHelper.GetIcon();
            if (icon != null) Icon = icon;
            
            Text = plugin.Name;


            BassVst.BASS_VST_EmbedEditor(plugin.Id, EditorPanelHandle);
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
                    menuItem.Click += new EventHandler(mnuPreset_Click);
                    _mnuPresets.DropDownItems.Add(menuItem);
                }
                
                i++;
            }
        }

        private void mnuPreset_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripDropDownItem)sender;
            var index = int.Parse(menuItem.Tag.ToString());

            _bassPlayer.SetVstPluginPreset(_plugin, index);
        }


        private void VSTPluginConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CanClose)
            {
                e.Cancel = true;
                Visible = false;
            }
            else
            {
                _bassPlayer = null;
                _plugin = null;
            }
        }

        public bool CanClose
        {
            get;
            set;
        }

        public IntPtr EditorPanelHandle
        {
            get { return _pnlEditor.Handle; }
        }

        private void InitializeComponent()
        {
            _menuStrip = new MenuStrip();
            _mnuPresets = new ToolStripMenuItem();
            _pnlEditor = new Panel();
            _menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            _menuStrip.Items.AddRange(new ToolStripItem[] {
            _mnuPresets});
            _menuStrip.Location = new System.Drawing.Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Size = new System.Drawing.Size(284, 24);
            _menuStrip.TabIndex = 0;
            _menuStrip.Text = "menuStrip1";
            // 
            // mnuPresets
            // 
            _mnuPresets.Name = "_mnuPresets";
            _mnuPresets.Size = new System.Drawing.Size(56, 20);
            _mnuPresets.Text = "&Presets";
            // 
            // pnlEditor
            // 
            _pnlEditor.Dock = DockStyle.Fill;
            _pnlEditor.Location = new System.Drawing.Point(0, 24);
            _pnlEditor.Name = "_pnlEditor";
            _pnlEditor.Size = new System.Drawing.Size(284, 240);
            _pnlEditor.TabIndex = 1;
            // 
            // VSTPluginConfigForm
            // 
            ClientSize = new System.Drawing.Size(284, 264);
            Controls.Add(_pnlEditor);
            Controls.Add(_menuStrip);
            MainMenuStrip = _menuStrip;
            Name = "VstPluginConfigForm";
            _menuStrip.ResumeLayout(false);
            _menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }
    }
}
