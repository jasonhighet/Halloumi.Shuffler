using System;
using System.Collections.Generic;
using Halloumi.Common.Windows.Forms;
using AE = Halloumi.Shuffler.AudioEngine;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmPlugin : BaseForm
    {
        private AE.BassPlayer BassPlayer { get; set; }

        public FrmPlugin(AE.BassPlayer bassPlayer)
        {
            InitializeComponent();
            BassPlayer = bassPlayer;
            BindData();
        }

        private void cmbVSTPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbVSTPlugins.SelectedIndex == -1) return;
            LoadMainVstPlugin(cmbVSTPlugins.SelectedValue.ToString());
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
            LoadTrackFxvstPlugin(cmbTrackFXVSTPlugins.SelectedValue.ToString());
        }

        private void btnWAPluginConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.WaPlugin == null) return;
            BassPlayer.ShowWaPluginConfig(BassPlayer.WaPlugin);
        }

        private void btnVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.MainVstPlugin == null) return;
            BassPlayer.ShowVstPluginConfig(BassPlayer.MainVstPlugin);
        }

        private class VstPluginModel
        {
            public string Location { get; set; }

            public string Name { get; set; }
        }

        public class WaPluginModel
        {
            public string Location { get; set; }

            public string Name { get; set; }
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

        public void LoadMainVstPlugin(string location)
        {
            try
            {
                if (location == MainVstPluginLocation) return;
                BassPlayer.ClearMainVstPlugin();
                if (location != "") BassPlayer.LoadMainVstPlugin(location);
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
                BassPlayer.ClearMainVstPlugin2();
                if (location != "") BassPlayer.LoadMainVstPlugin2(location);
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
                BassPlayer.ClearSamplerVstPlugin();
                if (location != "") BassPlayer.LoadSamplerVstPlugin(location);
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
                BassPlayer.ClearSamplerVstPlugin2();
                if (location != "") BassPlayer.LoadSamplerVstPlugin2(location);
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
                BassPlayer.ClearTracksVstPlugin();
                if (location != "") BassPlayer.LoadTracksVstPlugin(location);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public void LoadTrackFxvstPlugin(string location)
        {
            try
            {
                if (location == CurrentTrackFxvstPluginLocation) return;
                BassPlayer.ClearTrackSendFxvstPlugin();
                if (location != "") BassPlayer.LoadTrackSendFxvstPlugin(location);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public void LoadTrackFxvstPlugin2(string location)
        {
            try
            {
                if (location == CurrentTrackFxvstPluginLocation2) return;
                BassPlayer.ClearTrackSendFxvstPlugin2();
                if (location != "") BassPlayer.LoadTrackSendFxvstPlugin2(location);
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

        private bool _binding = false;

        public void BindWaPlugins()
        {
            var plugins = new List<WaPluginModel>();
            plugins.Add(new WaPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in BassPlayer.FindWaPlugins())
            {
                var model = new WaPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbWAPlugins.Items.Clear();
            cmbWAPlugins.DisplayMember = "Name";
            cmbWAPlugins.ValueMember = "Location";
            cmbWAPlugins.DataSource = plugins;

            cmbWAPlugins.SelectedIndex = 0;
            for (var i = 0; i < cmbWAPlugins.Items.Count; i++)
            {
                var plugin = cmbWAPlugins.Items[i] as WaPluginModel;
                if (plugin.Location == CurrentWaPluginLocation)
                {
                    cmbWAPlugins.SelectedIndex = i;
                    break;
                }
            }
        }

        public void BindMainVstPlugins()
        {
            var plugins = new List<VstPluginModel>();
            plugins.Add(new VstPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in BassPlayer.FindVstPlugins())
            {
                var model = new VstPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbVSTPlugins.Items.Clear();
            cmbVSTPlugins.DisplayMember = "Name";
            cmbVSTPlugins.ValueMember = "Location";
            cmbVSTPlugins.DataSource = plugins;

            cmbVSTPlugins.SelectedIndex = 0;
            for (var i = 0; i < cmbVSTPlugins.Items.Count; i++)
            {
                var plugin = cmbVSTPlugins.Items[i] as VstPluginModel;
                if (plugin.Location == MainVstPluginLocation)
                {
                    cmbVSTPlugins.SelectedIndex = i;
                    break;
                }
            }
        }

        public void BindMainVstPlugins2()
        {
            var plugins = new List<VstPluginModel>();
            plugins.Add(new VstPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in BassPlayer.FindVstPlugins())
            {
                var model = new VstPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbMainMixerPlugins2.Items.Clear();
            cmbMainMixerPlugins2.DisplayMember = "Name";
            cmbMainMixerPlugins2.ValueMember = "Location";
            cmbMainMixerPlugins2.DataSource = plugins;

            cmbMainMixerPlugins2.SelectedIndex = 0;
            for (var i = 0; i < cmbMainMixerPlugins2.Items.Count; i++)
            {
                var plugin = cmbMainMixerPlugins2.Items[i] as VstPluginModel;
                if (plugin.Location == MainVstPluginLocation2)
                {
                    cmbMainMixerPlugins2.SelectedIndex = i;
                    break;
                }
            }
        }

        public void BindSamplerVstPlugins()
        {
            var plugins = new List<VstPluginModel>();
            plugins.Add(new VstPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in BassPlayer.FindVstPlugins())
            {
                var model = new VstPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbSamplerVSTPlugins.Items.Clear();
            cmbSamplerVSTPlugins.DisplayMember = "Name";
            cmbSamplerVSTPlugins.ValueMember = "Location";
            cmbSamplerVSTPlugins.DataSource = plugins;

            cmbSamplerVSTPlugins.SelectedIndex = 0;
            for (var i = 0; i < cmbSamplerVSTPlugins.Items.Count; i++)
            {
                var plugin = cmbSamplerVSTPlugins.Items[i] as VstPluginModel;
                if (plugin.Location == CurrentSamplerVstPluginLocation)
                {
                    cmbSamplerVSTPlugins.SelectedIndex = i;
                    break;
                }
            }
        }

        public void BindSamplerVstPlugins2()
        {
            var plugins = new List<VstPluginModel>();
            plugins.Add(new VstPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in BassPlayer.FindVstPlugins())
            {
                var model = new VstPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbSamplerVSTPlugins2.Items.Clear();
            cmbSamplerVSTPlugins2.DisplayMember = "Name";
            cmbSamplerVSTPlugins2.ValueMember = "Location";
            cmbSamplerVSTPlugins2.DataSource = plugins;

            cmbSamplerVSTPlugins2.SelectedIndex = 0;
            for (var i = 0; i < cmbSamplerVSTPlugins2.Items.Count; i++)
            {
                var plugin = cmbSamplerVSTPlugins2.Items[i] as VstPluginModel;
                if (plugin.Location == CurrentSamplerVstPluginLocation2)
                {
                    cmbSamplerVSTPlugins2.SelectedIndex = i;
                    break;
                }
            }
        }

        public void BindTrackVstPlugins()
        {
            var plugins = new List<VstPluginModel>();
            plugins.Add(new VstPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in BassPlayer.FindVstPlugins())
            {
                var model = new VstPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbTrackVSTPlugins.Items.Clear();
            cmbTrackVSTPlugins.DisplayMember = "Name";
            cmbTrackVSTPlugins.ValueMember = "Location";
            cmbTrackVSTPlugins.DataSource = plugins;

            cmbTrackVSTPlugins.SelectedIndex = 0;
            for (var i = 0; i < cmbTrackVSTPlugins.Items.Count; i++)
            {
                var plugin = cmbTrackVSTPlugins.Items[i] as VstPluginModel;
                if (plugin.Location == CurrentTrackVstPluginLocation)
                {
                    cmbTrackVSTPlugins.SelectedIndex = i;
                    break;
                }
            }
        }

        public void BindTrackFxvstPlugins()
        {
            var plugins = new List<VstPluginModel>();
            plugins.Add(new VstPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in BassPlayer.FindVstPlugins())
            {
                var model = new VstPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbTrackFXVSTPlugins.Items.Clear();
            cmbTrackFXVSTPlugins.DisplayMember = "Name";
            cmbTrackFXVSTPlugins.ValueMember = "Location";
            cmbTrackFXVSTPlugins.DataSource = plugins;

            cmbTrackFXVSTPlugins.SelectedIndex = 0;
            for (var i = 0; i < cmbTrackFXVSTPlugins.Items.Count; i++)
            {
                var plugin = cmbTrackFXVSTPlugins.Items[i] as VstPluginModel;
                if (plugin.Location == CurrentTrackFxvstPluginLocation)
                {
                    cmbTrackFXVSTPlugins.SelectedIndex = i;
                    break;
                }
            }
        }

        public void BindTrackFxvstPlugins2()
        {
            var plugins = new List<VstPluginModel>();
            plugins.Add(new VstPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in BassPlayer.FindVstPlugins())
            {
                var model = new VstPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbTrackFXVSTPlugins2.Items.Clear();
            cmbTrackFXVSTPlugins2.DisplayMember = "Name";
            cmbTrackFXVSTPlugins2.ValueMember = "Location";
            cmbTrackFXVSTPlugins2.DataSource = plugins;

            cmbTrackFXVSTPlugins2.SelectedIndex = 0;
            for (var i = 0; i < cmbTrackFXVSTPlugins2.Items.Count; i++)
            {
                var plugin = cmbTrackFXVSTPlugins2.Items[i] as VstPluginModel;
                if (plugin.Location == CurrentTrackFxvstPluginLocation2)
                {
                    cmbTrackFXVSTPlugins2.SelectedIndex = i;
                    break;
                }
            }
        }

        public string MainVstPluginLocation
        {
            get
            {
                if (BassPlayer.MainVstPlugin == null) return "";
                return BassPlayer.MainVstPlugin.Location;
            }
        }

        public string MainVstPluginLocation2
        {
            get
            {
                if (BassPlayer.MainVstPlugin2 == null) return "";
                return BassPlayer.MainVstPlugin2.Location;
            }
        }

        public string CurrentSamplerVstPluginLocation
        {
            get
            {
                if (BassPlayer.SamplerVstPlugin == null) return "";
                return BassPlayer.SamplerVstPlugin.Location;
            }
        }

        public string CurrentSamplerVstPluginLocation2
        {
            get
            {
                if (BassPlayer.SamplerVstPlugin2 == null) return "";
                return BassPlayer.SamplerVstPlugin2.Location;
            }
        }

        public string CurrentTrackVstPluginLocation
        {
            get
            {
                if (BassPlayer.TrackVstPlugin == null) return "";
                return BassPlayer.TrackVstPlugin.Location;
            }
        }

        public string CurrentTrackFxvstPluginLocation
        {
            get
            {
                if (BassPlayer.TrackSendFxvstPlugin == null) return "";
                return BassPlayer.TrackSendFxvstPlugin.Location;
            }
        }

        public string CurrentTrackFxvstPluginLocation2
        {
            get
            {
                if (BassPlayer.TrackSendFxvstPlugin2 == null) return "";
                return BassPlayer.TrackSendFxvstPlugin2.Location;
            }
        }

        public string CurrentWaPluginLocation
        {
            get
            {
                if (BassPlayer.WaPlugin == null) return "";
                return BassPlayer.WaPlugin.Location;
            }
        }

        private void btnSamplerVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.SamplerVstPlugin == null) return;
            BassPlayer.ShowVstPluginConfig(BassPlayer.SamplerVstPlugin);
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
            if (BassPlayer.MainVstPlugin != null) mainVstPluginParameters = BassPlayer.GetVstPluginParameters(BassPlayer.MainVstPlugin);
            settings.MainMixerVstPluginParameters = mainVstPluginParameters;

            var mainVstPlugin2 = "";
            if (BassPlayer.MainVstPlugin2 != null) mainVstPlugin2 = BassPlayer.MainVstPlugin2.Location;
            settings.MainMixerVstPlugin2 = mainVstPlugin2;

            var mainVstPluginParameters2 = "";
            if (BassPlayer.MainVstPlugin2 != null) mainVstPluginParameters2 = BassPlayer.GetVstPluginParameters(BassPlayer.MainVstPlugin2);
            settings.MainMixerVstPlugin2Parameters = mainVstPluginParameters2;

            var samplerVstPlugin = "";
            if (BassPlayer.SamplerVstPlugin != null) samplerVstPlugin = BassPlayer.SamplerVstPlugin.Location;
            settings.SamplerVstPlugin = samplerVstPlugin;

            var samplerVstPluginParameters = "";
            if (BassPlayer.SamplerVstPlugin != null) samplerVstPluginParameters = BassPlayer.GetVstPluginParameters(BassPlayer.SamplerVstPlugin);
            settings.SamplerVstPluginParameters = samplerVstPluginParameters;

            var samplerVstPlugin2 = "";
            if (BassPlayer.SamplerVstPlugin2 != null) samplerVstPlugin2 = BassPlayer.SamplerVstPlugin2.Location;
            settings.SamplerVstPlugin2 = samplerVstPlugin2;

            var samplerVstPluginParameters2 = "";
            if (BassPlayer.SamplerVstPlugin2 != null) samplerVstPluginParameters2 = BassPlayer.GetVstPluginParameters(BassPlayer.SamplerVstPlugin2);
            settings.SamplerVstPlugin2Parameters = samplerVstPluginParameters2;

            var trackVstPlugin = "";
            if (BassPlayer.TrackVstPlugin != null) trackVstPlugin = BassPlayer.TrackVstPlugin.Location;
            settings.TrackVstPlugin = trackVstPlugin;

            var trackVstPluginParameters = "";
            if (BassPlayer.TrackVstPlugin != null) trackVstPluginParameters = BassPlayer.GetVstPluginParameters(BassPlayer.TrackVstPlugin);
            settings.TrackVstPluginParameters = trackVstPluginParameters;

            var trackFxVstPlugin = "";
            if (BassPlayer.TrackSendFxvstPlugin != null) trackFxVstPlugin = BassPlayer.TrackSendFxvstPlugin.Location;
            settings.TrackFxvstPlugin = trackFxVstPlugin;

            var trackFxVstPluginParameters = "";
            if (BassPlayer.TrackSendFxvstPlugin != null) trackFxVstPluginParameters = BassPlayer.GetVstPluginParameters(BassPlayer.TrackSendFxvstPlugin);
            settings.TrackFxvstPluginParameters = trackFxVstPluginParameters;

            var trackFxVstPlugin2 = "";
            if (BassPlayer.TrackSendFxvstPlugin2 != null) trackFxVstPlugin2 = BassPlayer.TrackSendFxvstPlugin2.Location;
            settings.TrackFxvstPlugin2 = trackFxVstPlugin2;

            var trackFxVstPluginParameters2 = "";
            if (BassPlayer.TrackSendFxvstPlugin2 != null) trackFxVstPluginParameters2 = BassPlayer.GetVstPluginParameters(BassPlayer.TrackSendFxvstPlugin2);
            settings.TrackFxvstPlugin2Parameters = trackFxVstPluginParameters2;

            settings.Save();
            Close();
        }

        private void btnTrackVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.TrackVstPlugin == null) return;
            BassPlayer.ShowVstPluginConfig(BassPlayer.TrackVstPlugin);
        }

        private void btnTrackFXVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (BassPlayer.TrackSendFxvstPlugin == null) return;
            BassPlayer.ShowVstPluginConfig(BassPlayer.TrackSendFxvstPlugin);
        }

        private void btnSamplerVSTPlugin2Config_Click(object sender, EventArgs e)
        {
            if (BassPlayer.SamplerVstPlugin2 == null) return;
            BassPlayer.ShowVstPluginConfig(BassPlayer.SamplerVstPlugin2);
        }

        private void cmbTrackFXVSTPlugins2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbTrackFXVSTPlugins2.SelectedIndex == -1) return;
            LoadTrackFxvstPlugin2(cmbTrackFXVSTPlugins2.SelectedValue.ToString());
        }

        private void btnTrackFXVSTPluginConfig2_Click(object sender, EventArgs e)
        {
            if (BassPlayer.TrackSendFxvstPlugin2 == null) return;
            BassPlayer.ShowVstPluginConfig(BassPlayer.TrackSendFxvstPlugin2);
        }

        private void btnMainMixerPluginConfig2_Click(object sender, EventArgs e)
        {
            if (BassPlayer.MainVstPlugin2 == null) return;
            BassPlayer.ShowVstPluginConfig(BassPlayer.MainVstPlugin2);
        }

        private void cmbMainMixerPlugins2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbMainMixerPlugins2.SelectedIndex == -1) return;
            LoadMainMixerVstPlugin2(cmbMainMixerPlugins2.SelectedValue.ToString());
        }
    }
}