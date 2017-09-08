using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine.Plugins;
using AE = Halloumi.Shuffler.AudioEngine;

namespace Halloumi.Shuffler.Forms
{
    // ReSharper disable once InconsistentNaming
    public partial class frmPluginSettings : BaseForm
    {
        private bool _binding;

        public frmPluginSettings(AE.BassPlayer bassPlayer)
        {
            InitializeComponent();
            BassPlayer = bassPlayer;
            BindData();
        }

        private AE.BassPlayer BassPlayer { get; }

        public string MainVstPluginLocation => BassPlayer.MainVstPlugin == null
            ? ""
            : BassPlayer.MainVstPlugin.Location;

        public string MainVstPluginLocation2 => BassPlayer.MainVstPlugin2 == null
            ? ""
            : BassPlayer.MainVstPlugin2.Location;

        public string CurrentSamplerVstPluginLocation => BassPlayer.SamplerVstPlugin == null
            ? ""
            : BassPlayer.SamplerVstPlugin.Location;

        public string CurrentSamplerVstPluginLocation2 => BassPlayer.SamplerVstPlugin2 == null
            ? ""
            : BassPlayer.SamplerVstPlugin2.Location;

        public string CurrentTrackVstPluginLocation => BassPlayer.TrackVstPlugin == null
            ? ""
            : BassPlayer.TrackVstPlugin.Location;

        public string CurrentTrackFxvstPluginLocation => BassPlayer.TrackSendFxVstPlugin == null
            ? ""
            : BassPlayer.TrackSendFxVstPlugin.Location;

        public string CurrentTrackFxvstPluginLocation2 => BassPlayer.TrackSendFxVstPlugin2 == null
            ? ""
            : BassPlayer.TrackSendFxVstPlugin2.Location;

        public string CurrentWaPluginLocation => BassPlayer.WaPlugin == null ? "" : BassPlayer.WaPlugin.Location;

        private void cmbVSTPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbVSTPlugins.SelectedIndex == -1) return;
            LoadMainMixerVstPlugin(cmbVSTPlugins.SelectedValue.ToString());
        }

        private void cmbWAPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbWAPlugins.SelectedIndex == -1) return;
            LoadWaPlugin(cmbWAPlugins.SelectedValue.ToString());
        }

        private void cmbSampleVSTPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbSamplerVSTPlugins.SelectedIndex == -1) return;
            LoadSamplerVstPlugin(cmbSamplerVSTPlugins.SelectedValue.ToString());
        }

        private void cmbTrackVSTPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbTrackVSTPlugins.SelectedIndex == -1) return;
            LoadTrackVstPlugin(cmbTrackVSTPlugins.SelectedValue.ToString());
        }

        private void cmbSamplerVSTPlugins2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbSamplerVSTPlugins2.SelectedIndex == -1) return;
            LoadSamplerVstPlugin2(cmbSamplerVSTPlugins2.SelectedValue.ToString());
        }

        private void cmbTrackFXVSTPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbTrackFXVSTPlugins.SelectedIndex == -1) return;
            LoadTrackFxVstPlugin(cmbTrackFXVSTPlugins.SelectedValue.ToString());
        }

        private void btnWAPluginConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.WaPlugin == null) return;
            PluginHelper.ShowWaPluginConfig(BassPlayer.WaPlugin);
        }

        private void btnVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.MainVstPlugin == null) return;
            PluginHelper.ShowVstPluginConfig(BassPlayer.MainVstPlugin);
        }

        public void LoadWaPlugin(string location)
        {
            try
            {
                if (location == CurrentWaPluginLocation) return;
                BassPlayer.UnloadAllWaPlugins();
                if (location != "") BassPlayer.LoadWaPlugin(location);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public void LoadMainMixerVstPlugin(string location)
        {
            try
            {
                if (location == MainVstPluginLocation) return;
                BassPlayer.ClearMainVstPlugin(0);
                if (location != "") BassPlayer.LoadMainVstPlugin(location, 0);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public void LoadMainMixerVstPlugin2(string location)
        {
            try
            {
                if (location == MainVstPluginLocation2) return;
                BassPlayer.ClearMainVstPlugin(1);
                if (location != "") BassPlayer.LoadMainVstPlugin(location, 1);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public void LoadSamplerVstPlugin(string location)
        {
            try
            {
                if (location == CurrentSamplerVstPluginLocation) return;
                BassPlayer.ClearSamplerVstPlugin(0);
                if (location != "") BassPlayer.LoadSamplerVstPlugin(location, 0);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public void LoadSamplerVstPlugin2(string location)
        {
            try
            {
                if (location == CurrentSamplerVstPluginLocation2) return;
                BassPlayer.ClearSamplerVstPlugin(1);
                if (location != "") BassPlayer.LoadSamplerVstPlugin(location, 1);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public void LoadTrackVstPlugin(string location)
        {
            try
            {
                if (location == CurrentTrackVstPluginLocation) return;
                BassPlayer.ClearTracksVstPlugin(0);
                if (location != "") BassPlayer.LoadTracksVstPlugin(location, 0);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public void LoadTrackFxVstPlugin(string location)
        {
            try
            {
                if (location == CurrentTrackFxvstPluginLocation) return;
                BassPlayer.ClearTrackSendFxvstPlugin(0);
                if (location != "") BassPlayer.LoadTrackSendFxvstPlugin(location, 0);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public void LoadTrackFxVstPlugin2(string location)
        {
            try
            {
                if (location == CurrentTrackFxvstPluginLocation2) return;
                BassPlayer.ClearTrackSendFxvstPlugin(1);
                if (location != "") BassPlayer.LoadTrackSendFxvstPlugin(location, 1);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        private void BindData()
        {
            _binding = true;

            BindWaPlugins();
            BindMainVstPlugins();
            BindMainVstPlugins2();
            BindSamplerVstPlugins();
            BindSamplerVstPlugins2();
            BindTrackVstPlugins();
            BindTrackFxvstPlugins();
            BindTrackFxvstPlugins2();

            _binding = false;
        }

        public void BindWaPlugins()
        {
            var plugins = new List<WaPluginModel>
            {
                new WaPluginModel
                {
                    Location = "",
                    Name = "(None)"
                }
            };
            plugins.AddRange(PluginHelper.FindWaPlugins()
                .Select(plugin => new WaPluginModel
                {
                    Location = plugin.Location,
                    Name = plugin.Name
                }));

            cmbWAPlugins.DataSource = null;
            cmbWAPlugins.Items.Clear();
            cmbWAPlugins.DisplayMember = "Name";
            cmbWAPlugins.ValueMember = "Location";
            cmbWAPlugins.DataSource = plugins;

            cmbWAPlugins.SelectedIndex = 0;
            for (var i = 0; i < cmbWAPlugins.Items.Count; i++)
            {
                var plugin = cmbWAPlugins.Items[i] as WaPluginModel;
                if (plugin != null && plugin.Location != CurrentWaPluginLocation) continue;
                cmbWAPlugins.SelectedIndex = i;
                break;
            }
        }

        public void BindMainVstPlugins()
        {
            var plugins = new List<VstPluginModel>
            {
                new VstPluginModel
                {
                    Location = "",
                    Name = "(None)"
                }
            };
            plugins.AddRange(PluginHelper.FindVstPlugins()
                .Select(plugin => new VstPluginModel
                {
                    Location = plugin.Location,
                    Name = plugin.Name
                }));

            cmbVSTPlugins.DataSource = null;
            cmbVSTPlugins.Items.Clear();
            cmbVSTPlugins.DisplayMember = "Name";
            cmbVSTPlugins.ValueMember = "Location";
            cmbVSTPlugins.DataSource = plugins;

            cmbVSTPlugins.SelectedIndex = 0;
            for (var i = 0; i < cmbVSTPlugins.Items.Count; i++)
            {
                var plugin = cmbVSTPlugins.Items[i] as VstPluginModel;
                if (plugin == null || plugin.Location != MainVstPluginLocation) continue;
                cmbVSTPlugins.SelectedIndex = i;
                break;
            }
        }

        public void BindMainVstPlugins2()
        {
            var plugins = new List<VstPluginModel>
            {
                new VstPluginModel
                {
                    Location = "",
                    Name = "(None)"
                }
            };
            plugins.AddRange(PluginHelper.FindVstPlugins()
                .Select(plugin => new VstPluginModel
                {
                    Location = plugin.Location,
                    Name = plugin.Name
                }));

            cmbMainMixerPlugins2.DataSource = null;
            cmbMainMixerPlugins2.Items.Clear();
            cmbMainMixerPlugins2.DisplayMember = "Name";
            cmbMainMixerPlugins2.ValueMember = "Location";
            cmbMainMixerPlugins2.DataSource = plugins;

            cmbMainMixerPlugins2.SelectedIndex = 0;
            for (var i = 0; i < cmbMainMixerPlugins2.Items.Count; i++)
            {
                var plugin = cmbMainMixerPlugins2.Items[i] as VstPluginModel;
                if (plugin == null || plugin.Location != MainVstPluginLocation2) continue;
                cmbMainMixerPlugins2.SelectedIndex = i;
                break;
            }
        }

        public void BindSamplerVstPlugins()
        {
            var plugins = new List<VstPluginModel>
            {
                new VstPluginModel
                {
                    Location = "",
                    Name = "(None)"
                }
            };

            foreach (var plugin in PluginHelper.FindVstPlugins())
            {
                var model = new VstPluginModel
                {
                    Location = plugin.Location,
                    Name = plugin.Name
                };
                plugins.Add(model);
            }

            cmbSamplerVSTPlugins.DataSource = null;
            cmbSamplerVSTPlugins.Items.Clear();
            cmbSamplerVSTPlugins.DisplayMember = "Name";
            cmbSamplerVSTPlugins.ValueMember = "Location";
            cmbSamplerVSTPlugins.DataSource = plugins;

            cmbSamplerVSTPlugins.SelectedIndex = 0;
            for (var i = 0; i < cmbSamplerVSTPlugins.Items.Count; i++)
            {
                var plugin = cmbSamplerVSTPlugins.Items[i] as VstPluginModel;
                if (plugin == null || plugin.Location != CurrentSamplerVstPluginLocation) continue;
                cmbSamplerVSTPlugins.SelectedIndex = i;
                break;
            }
        }

        public void BindSamplerVstPlugins2()
        {
            var plugins = new List<VstPluginModel>
            {
                new VstPluginModel
                {
                    Location = "",
                    Name = "(None)"
                }
            };
            plugins.AddRange(PluginHelper.FindVstPlugins()
                .Select(plugin => new VstPluginModel
                {
                    Location = plugin.Location,
                    Name = plugin.Name
                }));

            cmbSamplerVSTPlugins2.DataSource = null;
            cmbSamplerVSTPlugins2.Items.Clear();
            cmbSamplerVSTPlugins2.DisplayMember = "Name";
            cmbSamplerVSTPlugins2.ValueMember = "Location";
            cmbSamplerVSTPlugins2.DataSource = plugins;

            cmbSamplerVSTPlugins2.SelectedIndex = 0;
            for (var i = 0; i < cmbSamplerVSTPlugins2.Items.Count; i++)
            {
                var plugin = cmbSamplerVSTPlugins2.Items[i] as VstPluginModel;
                if (plugin == null || plugin.Location != CurrentSamplerVstPluginLocation2) continue;
                cmbSamplerVSTPlugins2.SelectedIndex = i;
                break;
            }
        }

        public void BindTrackVstPlugins()
        {
            var plugins = new List<VstPluginModel>
            {
                new VstPluginModel
                {
                    Location = "",
                    Name = "(None)"
                }
            };
            plugins.AddRange(PluginHelper.FindVstPlugins()
                .Select(plugin => new VstPluginModel
                {
                    Location = plugin.Location,
                    Name = plugin.Name
                }));

            cmbTrackVSTPlugins.DataSource = null;
            cmbTrackVSTPlugins.Items.Clear();
            cmbTrackVSTPlugins.DisplayMember = "Name";
            cmbTrackVSTPlugins.ValueMember = "Location";
            cmbTrackVSTPlugins.DataSource = plugins;

            cmbTrackVSTPlugins.SelectedIndex = 0;
            for (var i = 0; i < cmbTrackVSTPlugins.Items.Count; i++)
            {
                var plugin = cmbTrackVSTPlugins.Items[i] as VstPluginModel;
                if (plugin != null && plugin.Location != CurrentTrackVstPluginLocation) continue;
                cmbTrackVSTPlugins.SelectedIndex = i;
                break;
            }
        }

        public void BindTrackFxvstPlugins()
        {
            var plugins = new List<VstPluginModel>
            {
                new VstPluginModel
                {
                    Location = "",
                    Name = "(None)"
                }
            };
            plugins.AddRange(PluginHelper.FindVstPlugins()
                .Select(plugin => new VstPluginModel
                {
                    Location = plugin.Location,
                    Name = plugin.Name
                }));

            cmbTrackFXVSTPlugins.DataSource = null;
            cmbTrackFXVSTPlugins.Items.Clear();
            cmbTrackFXVSTPlugins.DisplayMember = "Name";
            cmbTrackFXVSTPlugins.ValueMember = "Location";
            cmbTrackFXVSTPlugins.DataSource = plugins;

            cmbTrackFXVSTPlugins.SelectedIndex = 0;
            for (var i = 0; i < cmbTrackFXVSTPlugins.Items.Count; i++)
            {
                var plugin = cmbTrackFXVSTPlugins.Items[i] as VstPluginModel;
                if (plugin != null && plugin.Location != CurrentTrackFxvstPluginLocation) continue;
                cmbTrackFXVSTPlugins.SelectedIndex = i;
                break;
            }
        }

        public void BindTrackFxvstPlugins2()
        {
            var plugins = new List<VstPluginModel>
            {
                new VstPluginModel
                {
                    Location = "",
                    Name = "(None)"
                }
            };
            plugins.AddRange(PluginHelper.FindVstPlugins()
                .Select(plugin => new VstPluginModel
                {
                    Location = plugin.Location,
                    Name = plugin.Name
                }));

            cmbTrackFXVSTPlugins2.DataSource = null;
            cmbTrackFXVSTPlugins2.Items.Clear();
            cmbTrackFXVSTPlugins2.DisplayMember = "Name";
            cmbTrackFXVSTPlugins2.ValueMember = "Location";
            cmbTrackFXVSTPlugins2.DataSource = plugins;

            cmbTrackFXVSTPlugins2.SelectedIndex = 0;
            for (var i = 0; i < cmbTrackFXVSTPlugins2.Items.Count; i++)
            {
                var plugin = cmbTrackFXVSTPlugins2.Items[i] as VstPluginModel;
                if (plugin != null && plugin.Location != CurrentTrackFxvstPluginLocation2) continue;
                cmbTrackFXVSTPlugins2.SelectedIndex = i;
                break;
            }
        }

        private void btnSamplerVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.SamplerVstPlugin == null) return;
            PluginHelper.ShowVstPluginConfig(BassPlayer.SamplerVstPlugin);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var settings = Settings.Default;

            var winampPlugin = "";
            if (BassPlayer.WaPlugin != null) winampPlugin = BassPlayer.WaPlugin.Location;
            settings.WaPlugin = winampPlugin;

            var mainVstPlugin = "";
            if (BassPlayer.MainVstPlugin != null) mainVstPlugin = BassPlayer.MainVstPlugin.Location;
            settings.MainMixerVstPlugin = mainVstPlugin;

            var mainVstPluginParameters = "";
            if (BassPlayer.MainVstPlugin != null)
                mainVstPluginParameters = PluginHelper.GetVstPluginParameters(BassPlayer.MainVstPlugin);
            settings.MainMixerVstPluginParameters = mainVstPluginParameters;

            var mainVstPlugin2 = "";
            if (BassPlayer.MainVstPlugin2 != null) mainVstPlugin2 = BassPlayer.MainVstPlugin2.Location;
            settings.MainMixerVstPlugin2 = mainVstPlugin2;

            var mainVstPluginParameters2 = "";
            if (BassPlayer.MainVstPlugin2 != null)
                mainVstPluginParameters2 = PluginHelper.GetVstPluginParameters(BassPlayer.MainVstPlugin2);
            settings.MainMixerVstPlugin2Parameters = mainVstPluginParameters2;

            var samplerVstPlugin = "";
            if (BassPlayer.SamplerVstPlugin != null) samplerVstPlugin = BassPlayer.SamplerVstPlugin.Location;
            settings.SamplerVstPlugin = samplerVstPlugin;

            var samplerVstPluginParameters = "";
            if (BassPlayer.SamplerVstPlugin != null)
                samplerVstPluginParameters = PluginHelper.GetVstPluginParameters(BassPlayer.SamplerVstPlugin);
            settings.SamplerVstPluginParameters = samplerVstPluginParameters;

            var samplerVstPlugin2 = "";
            if (BassPlayer.SamplerVstPlugin2 != null) samplerVstPlugin2 = BassPlayer.SamplerVstPlugin2.Location;
            settings.SamplerVstPlugin2 = samplerVstPlugin2;

            var samplerVstPluginParameters2 = "";
            if (BassPlayer.SamplerVstPlugin2 != null)
                samplerVstPluginParameters2 = PluginHelper.GetVstPluginParameters(BassPlayer.SamplerVstPlugin2);
            settings.SamplerVstPlugin2Parameters = samplerVstPluginParameters2;

            var trackVstPlugin = "";
            if (BassPlayer.TrackVstPlugin != null) trackVstPlugin = BassPlayer.TrackVstPlugin.Location;
            settings.TrackVstPlugin = trackVstPlugin;

            var trackVstPluginParameters = "";
            if (BassPlayer.TrackVstPlugin != null)
                trackVstPluginParameters = PluginHelper.GetVstPluginParameters(BassPlayer.TrackVstPlugin);
            settings.TrackVstPluginParameters = trackVstPluginParameters;

            var trackFxVstPlugin = "";
            if (BassPlayer.TrackSendFxVstPlugin != null) trackFxVstPlugin = BassPlayer.TrackSendFxVstPlugin.Location;
            settings.TrackFxvstPlugin = trackFxVstPlugin;

            var trackFxVstPluginParameters = "";
            if (BassPlayer.TrackSendFxVstPlugin != null)
                trackFxVstPluginParameters = PluginHelper.GetVstPluginParameters(BassPlayer.TrackSendFxVstPlugin);
            settings.TrackFxvstPluginParameters = trackFxVstPluginParameters;

            var trackFxVstPlugin2 = "";
            if (BassPlayer.TrackSendFxVstPlugin2 != null) trackFxVstPlugin2 = BassPlayer.TrackSendFxVstPlugin2.Location;
            settings.TrackFxvstPlugin2 = trackFxVstPlugin2;

            var trackFxVstPluginParameters2 = "";
            if (BassPlayer.TrackSendFxVstPlugin2 != null)
                trackFxVstPluginParameters2 = PluginHelper.GetVstPluginParameters(BassPlayer.TrackSendFxVstPlugin2);
            settings.TrackFxvstPlugin2Parameters = trackFxVstPluginParameters2;

            settings.Save();
            Close();
        }

        private void btnTrackVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.TrackVstPlugin == null) return;
            PluginHelper.ShowVstPluginConfig(BassPlayer.TrackVstPlugin);
        }

        private void btnTrackFXVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.TrackSendFxVstPlugin == null) return;
            PluginHelper.ShowVstPluginConfig(BassPlayer.TrackSendFxVstPlugin);
        }

        private void btnSamplerVSTPlugin2Config_Click(object sender, EventArgs e)
        {
            if (BassPlayer.SamplerVstPlugin2 == null) return;
            PluginHelper.ShowVstPluginConfig(BassPlayer.SamplerVstPlugin2);
        }

        private void cmbTrackFXVSTPlugins2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbTrackFXVSTPlugins2.SelectedIndex == -1) return;
            LoadTrackFxVstPlugin2(cmbTrackFXVSTPlugins2.SelectedValue.ToString());
        }

        private void btnTrackFXVSTPluginConfig2_Click(object sender, EventArgs e)
        {
            if (BassPlayer.TrackSendFxVstPlugin2 == null) return;
            PluginHelper.ShowVstPluginConfig(BassPlayer.TrackSendFxVstPlugin2);
        }

        private void btnMainMixerPluginConfig2_Click(object sender, EventArgs e)
        {
            if (BassPlayer.MainVstPlugin2 == null) return;
            PluginHelper.ShowVstPluginConfig(BassPlayer.MainVstPlugin2);
        }

        private void cmbMainMixerPlugins2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbMainMixerPlugins2.SelectedIndex == -1) return;
            LoadMainMixerVstPlugin2(cmbMainMixerPlugins2.SelectedValue.ToString());
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var settings = new PluginSettings();

            var winampPlugin = "";
            if (BassPlayer.WaPlugin != null) winampPlugin = BassPlayer.WaPlugin.Location;
            settings.WaPlugin = winampPlugin;

            var mainVstPlugin = "";
            if (BassPlayer.MainVstPlugin != null) mainVstPlugin = BassPlayer.MainVstPlugin.Location;
            settings.MainMixerVstPlugin = mainVstPlugin;

            var mainVstPluginParameters = "";
            if (BassPlayer.MainVstPlugin != null)
                mainVstPluginParameters = PluginHelper.GetVstPluginParameters(BassPlayer.MainVstPlugin);
            settings.MainMixerVstPluginParameters = mainVstPluginParameters;

            var mainVstPlugin2 = "";
            if (BassPlayer.MainVstPlugin2 != null) mainVstPlugin2 = BassPlayer.MainVstPlugin2.Location;
            settings.MainMixerVstPlugin2 = mainVstPlugin2;

            var mainVstPluginParameters2 = "";
            if (BassPlayer.MainVstPlugin2 != null)
                mainVstPluginParameters2 = PluginHelper.GetVstPluginParameters(BassPlayer.MainVstPlugin2);
            settings.MainMixerVstPlugin2Parameters = mainVstPluginParameters2;

            var samplerVstPlugin = "";
            if (BassPlayer.SamplerVstPlugin != null) samplerVstPlugin = BassPlayer.SamplerVstPlugin.Location;
            settings.SamplerVstPlugin = samplerVstPlugin;

            var samplerVstPluginParameters = "";
            if (BassPlayer.SamplerVstPlugin != null)
                samplerVstPluginParameters = PluginHelper.GetVstPluginParameters(BassPlayer.SamplerVstPlugin);
            settings.SamplerVstPluginParameters = samplerVstPluginParameters;

            var samplerVstPlugin2 = "";
            if (BassPlayer.SamplerVstPlugin2 != null) samplerVstPlugin2 = BassPlayer.SamplerVstPlugin2.Location;
            settings.SamplerVstPlugin2 = samplerVstPlugin2;

            var samplerVstPluginParameters2 = "";
            if (BassPlayer.SamplerVstPlugin2 != null)
                samplerVstPluginParameters2 = PluginHelper.GetVstPluginParameters(BassPlayer.SamplerVstPlugin2);
            settings.SamplerVstPlugin2Parameters = samplerVstPluginParameters2;

            var trackVstPlugin = "";
            if (BassPlayer.TrackVstPlugin != null) trackVstPlugin = BassPlayer.TrackVstPlugin.Location;
            settings.TrackVstPlugin = trackVstPlugin;

            var trackVstPluginParameters = "";
            if (BassPlayer.TrackVstPlugin != null)
                trackVstPluginParameters = PluginHelper.GetVstPluginParameters(BassPlayer.TrackVstPlugin);
            settings.TrackVstPluginParameters = trackVstPluginParameters;

            var trackFxVstPlugin = "";
            if (BassPlayer.TrackSendFxVstPlugin != null) trackFxVstPlugin = BassPlayer.TrackSendFxVstPlugin.Location;
            settings.TrackFxVstPlugin = trackFxVstPlugin;

            var trackFxVstPluginParameters = "";
            if (BassPlayer.TrackSendFxVstPlugin != null)
                trackFxVstPluginParameters = PluginHelper.GetVstPluginParameters(BassPlayer.TrackSendFxVstPlugin);
            settings.TrackFxvstPluginParameters = trackFxVstPluginParameters;

            var trackFxVstPlugin2 = "";
            if (BassPlayer.TrackSendFxVstPlugin2 != null) trackFxVstPlugin2 = BassPlayer.TrackSendFxVstPlugin2.Location;
            settings.TrackFxVstPlugin2 = trackFxVstPlugin2;

            var trackFxVstPluginParameters2 = "";
            if (BassPlayer.TrackSendFxVstPlugin2 != null)
                trackFxVstPluginParameters2 = PluginHelper.GetVstPluginParameters(BassPlayer.TrackSendFxVstPlugin2);
            settings.TrackFxvstPlugin2Parameters = trackFxVstPluginParameters2;

            var filename = FileDialogHelper.SaveAs("Plugin Settings (*.PluginSettings.xml)|*.PluginSettings.xml", "");
            if (filename != "")
                SerializationHelper<PluginSettings>.ToXmlFile(settings, filename);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var filename = FileDialogHelper.OpenSingle("Plugin Settings (*.PluginSettings.xml)|*.PluginSettings.xml");
            if (!File.Exists(filename))
                return;

            var settings = SerializationHelper<PluginSettings>.FromXmlFile(filename);

            var waPlugin = PluginHelper.FindWaPluginByLocation(settings.WaPlugin);
            if (waPlugin != null)
                LoadWaPlugin(waPlugin.Location);

            var mainMixerVstPlugin = PluginHelper.FindVstPluginByLocation(settings.MainMixerVstPlugin);
            if (mainMixerVstPlugin != null)
            {
                LoadMainMixerVstPlugin(mainMixerVstPlugin.Location);
                PluginHelper.SetVstPluginParameters(BassPlayer.MainVstPlugin, settings.MainMixerVstPlugin);
            }

            var mainMixerVstPlugin2 = PluginHelper.FindVstPluginByLocation(settings.MainMixerVstPlugin2);
            if (mainMixerVstPlugin2 != null)
            {
                LoadMainMixerVstPlugin2(mainMixerVstPlugin2.Location);
                PluginHelper.SetVstPluginParameters(BassPlayer.MainVstPlugin2, settings.MainMixerVstPlugin2);
            }

            var samplerVstPlugin = PluginHelper.FindVstPluginByLocation(settings.SamplerVstPlugin);
            if (samplerVstPlugin != null)
            {
                LoadSamplerVstPlugin(samplerVstPlugin.Location);
                PluginHelper.SetVstPluginParameters(BassPlayer.SamplerVstPlugin, settings.SamplerVstPlugin);
            }

            var samplerVstPlugin2 = PluginHelper.FindVstPluginByLocation(settings.SamplerVstPlugin2);
            if (samplerVstPlugin2 != null)
            {
                LoadSamplerVstPlugin2(samplerVstPlugin2.Location);
                PluginHelper.SetVstPluginParameters(BassPlayer.SamplerVstPlugin2, settings.SamplerVstPlugin2);
            }

            var trackVstPlugin = PluginHelper.FindVstPluginByLocation(settings.TrackVstPlugin);
            if (trackVstPlugin != null)
            {
                LoadTrackVstPlugin(trackVstPlugin.Location);
                PluginHelper.SetVstPluginParameters(BassPlayer.TrackVstPlugin, settings.TrackVstPlugin);
            }

            var trackFxVstPlugin = PluginHelper.FindVstPluginByLocation(settings.TrackFxVstPlugin);
            if (trackFxVstPlugin != null)
            {
                LoadTrackFxVstPlugin(trackFxVstPlugin.Location);
                PluginHelper.SetVstPluginParameters(BassPlayer.TrackSendFxVstPlugin, settings.TrackFxVstPlugin);
            }


            var trackFxVstPlugin2 = PluginHelper.FindVstPluginByLocation(settings.TrackFxVstPlugin2);
            if (trackFxVstPlugin2 != null)
            {
                LoadTrackFxVstPlugin2(trackFxVstPlugin2.Location);
                PluginHelper.SetVstPluginParameters(BassPlayer.TrackSendFxVstPlugin2, settings.TrackFxVstPlugin2);
            }


            BindData();
        }

        private class VstPluginModel
        {
            public string Location { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Name { get; set; }
        }

        public class WaPluginModel
        {
            public string Location { get; set; }

            public string Name { get; set; }
        }
    }
}