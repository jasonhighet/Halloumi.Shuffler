using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Helpers;
using Un4seen.Bass.AddOn.Vst;

namespace Halloumi.Shuffler.AudioEngine.Plugins
{
    public class VstPluginConfigForm : Form
    {
        private MenuStrip _menuStrip;
        private ToolStripMenuItem _mnuParameters;
        private ToolStripMenuItem _mnuPresets;
        private ToolStripMenuItem _mnuVst;
        private VstPlugin _plugin;
        private Panel _pnlEditor;

        public VstPluginConfigForm(VstPlugin plugin)
        {
            InitializeComponent();
            Initialize(plugin);
        }

        public bool CanClose { get; set; }

        public IntPtr EditorPanelHandle => _pnlEditor.Handle;

        private void Initialize(VstPlugin plugin)
        {
            _plugin = plugin;
            
            CanClose = false;
            FormClosing += VSTPluginConfigForm_FormClosing;

            MinimizeBox = true;
            ShowInTaskbar = true;
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            TopMost = true;

            var borderSize = SystemInformation.BorderSize;
            Width = plugin.EditorWidth + (borderSize.Width*2) + 15;
            Height = plugin.EditorHeight + _menuStrip.Height + (borderSize.Height*2) + 35;

            var icon = ApplicationHelper.GetIcon();
            if (icon != null) Icon = icon;

            Text = plugin.Name;


            BassVst.BASS_VST_EmbedEditor(plugin.Id, EditorPanelHandle);
            plugin.Form = this;

            LoadPresets(plugin);
        }

        private void LoadPresets(VstPlugin plugin)
        {
            var presets = PluginHelper.GetPluginPresets(plugin);

            foreach (var preset in presets)
            {
                var existingMenuItems = _mnuPresets.DropDownItems.Cast<ToolStripItem>().Select(t => t.Text).ToList();

                if (!existingMenuItems.Contains(preset.Name))
                {
                    var menuItem = new ToolStripMenuItem
                    {
                        Text = preset.Name,
                        Tag = preset.Id
                    };
                    menuItem.Click += mnuPreset_Click;
                    _mnuPresets.DropDownItems.Add(menuItem);
                }
            }
        }

        private void mnuPreset_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripDropDownItem) sender;
            var presetId = int.Parse(menuItem.Tag.ToString());

            PluginHelper.SetVstPluginPreset(_plugin, presetId);
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
                _plugin = null;
            }
        }

        private void InitializeComponent()
        {
            _menuStrip = new MenuStrip();
            _mnuPresets = new ToolStripMenuItem();
            _pnlEditor = new Panel();
            _mnuVst = new ToolStripMenuItem();
            _mnuParameters = new ToolStripMenuItem();
            _menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _menuStrip
            // 
            _menuStrip.ImageScalingSize = new Size(20, 20);
            _menuStrip.Items.AddRange(new ToolStripItem[]
            {
                _mnuPresets,
                _mnuVst
            });
            _menuStrip.Location = new Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Size = new Size(284, 28);
            _menuStrip.TabIndex = 0;
            _menuStrip.Text = @"menuStrip1";
            // 
            // _mnuPresets
            // 
            _mnuPresets.Name = "_mnuPresets";
            _mnuPresets.Size = new Size(67, 24);
            _mnuPresets.Text = @"&Presets";
            // 
            // _pnlEditor
            // 
            _pnlEditor.Dock = DockStyle.Fill;
            _pnlEditor.Location = new Point(0, 28);
            _pnlEditor.Name = "_pnlEditor";
            _pnlEditor.Size = new Size(284, 236);
            _pnlEditor.TabIndex = 1;
            // 
            // _mnuVst
            // 
            _mnuVst.DropDownItems.AddRange(new ToolStripItem[]
            {
                _mnuParameters
            });
            _mnuVst.Name = "_mnuVst";
            _mnuVst.Size = new Size(46, 24);
            _mnuVst.Text = @"&VST";
            // 
            // _mnuParameters
            // 
            _mnuParameters.Name = "_mnuParameters";
            _mnuParameters.Size = new Size(181, 26);
            _mnuParameters.Text = @"&Parameters";
            _mnuParameters.Click += mnuParameters_Click;
            // 
            // VstPluginConfigForm
            // 
            ClientSize = new Size(284, 264);
            Controls.Add(_pnlEditor);
            Controls.Add(_menuStrip);
            MainMenuStrip = _menuStrip;
            Name = "VstPluginConfigForm";
            _menuStrip.ResumeLayout(false);
            _menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void mnuParameters_Click(object sender, EventArgs e)
        {
            var builder = new StringBuilder();

            builder.AppendLine(_plugin.Name);

            foreach (var parameter in _plugin.Parameters)
            {
                var info = BassVst.BASS_VST_GetParamInfo(_plugin.Id, parameter.Id);
                var value = BassVst.BASS_VST_GetParam(_plugin.Id, parameter.Id);

                if (info == null) continue;

                var line = $"{info.name}  {info.display}  {value}  {info.unit}";
                builder.AppendLine(line);
            }

            MessageBoxHelper.Show(builder.ToString());
        }
    }
}