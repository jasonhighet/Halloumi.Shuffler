using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmPlugin : BaseForm
    {
        private BE.BassPlayer BassPlayer { get; set; }

        public FrmPlugin(BE.BassPlayer bassPlayer)
        {
            InitializeComponent();
            this.BassPlayer = bassPlayer;
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
            if (this.BassPlayer.WaPlugin == null) return;
            this.BassPlayer.ShowWaPluginConfig(this.BassPlayer.WaPlugin);
        }

        private void btnVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.MainVstPlugin == null) return;
            this.BassPlayer.ShowVstPluginConfig(this.BassPlayer.MainVstPlugin);
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
                if (location == this.CurrentWaPluginLocation) return;
                this.BassPlayer.UnloadAllWaPlugins();
                if (location != "") this.BassPlayer.LoadWaPlugin(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        public void LoadMainVstPlugin(string location)
        {
            try
            {
                if (location == this.MainVstPluginLocation) return;
                this.BassPlayer.ClearMainVstPlugin();
                if (location != "") this.BassPlayer.LoadMainVstPlugin(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        public void LoadMainMixerVstPlugin2(string location)
        {
            try
            {
                if (location == this.MainVstPluginLocation2) return;
                this.BassPlayer.ClearMainVstPlugin2();
                if (location != "") this.BassPlayer.LoadMainVstPlugin2(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        public void LoadSamplerVstPlugin(string location)
        {
            try
            {
                if (location == this.CurrentSamplerVstPluginLocation) return;
                this.BassPlayer.ClearSamplerVstPlugin();
                if (location != "") this.BassPlayer.LoadSamplerVstPlugin(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        public void LoadSamplerVstPlugin2(string location)
        {
            try
            {
                if (location == this.CurrentSamplerVstPluginLocation2) return;
                this.BassPlayer.ClearSamplerVstPlugin2();
                if (location != "") this.BassPlayer.LoadSamplerVstPlugin2(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        public void LoadTrackVstPlugin(string location)
        {
            try
            {
                if (location == this.CurrentTrackVstPluginLocation) return;
                this.BassPlayer.ClearTracksVstPlugin();
                if (location != "") this.BassPlayer.LoadTracksVstPlugin(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        public void LoadTrackFxvstPlugin(string location)
        {
            try
            {
                if (location == this.CurrentTrackFxvstPluginLocation) return;
                this.BassPlayer.ClearTrackSendFxvstPlugin();
                if (location != "") this.BassPlayer.LoadTrackSendFxvstPlugin(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        public void LoadTrackFxvstPlugin2(string location)
        {
            try
            {
                if (location == this.CurrentTrackFxvstPluginLocation2) return;
                this.BassPlayer.ClearTrackSendFxvstPlugin2();
                if (location != "") this.BassPlayer.LoadTrackSendFxvstPlugin2(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
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

            foreach (var plugin in this.BassPlayer.FindWaPlugins())
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
                if (plugin.Location == this.CurrentWaPluginLocation)
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

            foreach (var plugin in this.BassPlayer.FindVstPlugins())
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
                if (plugin.Location == this.MainVstPluginLocation)
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

            foreach (var plugin in this.BassPlayer.FindVstPlugins())
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
                if (plugin.Location == this.MainVstPluginLocation2)
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

            foreach (var plugin in this.BassPlayer.FindVstPlugins())
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
                if (plugin.Location == this.CurrentSamplerVstPluginLocation)
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

            foreach (var plugin in this.BassPlayer.FindVstPlugins())
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
                if (plugin.Location == this.CurrentSamplerVstPluginLocation2)
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

            foreach (var plugin in this.BassPlayer.FindVstPlugins())
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
                if (plugin.Location == this.CurrentTrackVstPluginLocation)
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

            foreach (var plugin in this.BassPlayer.FindVstPlugins())
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
                if (plugin.Location == this.CurrentTrackFxvstPluginLocation)
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

            foreach (var plugin in this.BassPlayer.FindVstPlugins())
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
                if (plugin.Location == this.CurrentTrackFxvstPluginLocation2)
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
                if (this.BassPlayer.MainVstPlugin == null) return "";
                return this.BassPlayer.MainVstPlugin.Location;
            }
        }

        public string MainVstPluginLocation2
        {
            get
            {
                if (this.BassPlayer.MainVstPlugin2 == null) return "";
                return this.BassPlayer.MainVstPlugin2.Location;
            }
        }

        public string CurrentSamplerVstPluginLocation
        {
            get
            {
                if (this.BassPlayer.SamplerVstPlugin == null) return "";
                return this.BassPlayer.SamplerVstPlugin.Location;
            }
        }

        public string CurrentSamplerVstPluginLocation2
        {
            get
            {
                if (this.BassPlayer.SamplerVstPlugin2 == null) return "";
                return this.BassPlayer.SamplerVstPlugin2.Location;
            }
        }

        public string CurrentTrackVstPluginLocation
        {
            get
            {
                if (this.BassPlayer.TrackVstPlugin == null) return "";
                return this.BassPlayer.TrackVstPlugin.Location;
            }
        }

        public string CurrentTrackFxvstPluginLocation
        {
            get
            {
                if (this.BassPlayer.TrackSendFxvstPlugin == null) return "";
                return this.BassPlayer.TrackSendFxvstPlugin.Location;
            }
        }

        public string CurrentTrackFxvstPluginLocation2
        {
            get
            {
                if (this.BassPlayer.TrackSendFxvstPlugin2 == null) return "";
                return this.BassPlayer.TrackSendFxvstPlugin2.Location;
            }
        }

        public string CurrentWaPluginLocation
        {
            get
            {
                if (this.BassPlayer.WaPlugin == null) return "";
                return this.BassPlayer.WaPlugin.Location;
            }
        }

        private void btnSamplerVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.SamplerVstPlugin == null) return;
            this.BassPlayer.ShowVstPluginConfig(this.BassPlayer.SamplerVstPlugin);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var settings = Settings.Default;

            var winampPlugin = "";
            if (this.BassPlayer.WaPlugin != null) winampPlugin = this.BassPlayer.WaPlugin.Location;
            settings.WaPlugin = winampPlugin;

            var mainVstPlugin = "";
            if (this.BassPlayer.MainVstPlugin != null) mainVstPlugin = this.BassPlayer.MainVstPlugin.Location;
            settings.MainMixerVstPlugin = mainVstPlugin;

            var mainVstPluginParameters = "";
            if (this.BassPlayer.MainVstPlugin != null) mainVstPluginParameters = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.MainVstPlugin);
            settings.MainMixerVstPluginParameters = mainVstPluginParameters;

            var mainVstPlugin2 = "";
            if (this.BassPlayer.MainVstPlugin2 != null) mainVstPlugin2 = this.BassPlayer.MainVstPlugin2.Location;
            settings.MainMixerVstPlugin2 = mainVstPlugin2;

            var mainVstPluginParameters2 = "";
            if (this.BassPlayer.MainVstPlugin2 != null) mainVstPluginParameters2 = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.MainVstPlugin2);
            settings.MainMixerVstPlugin2Parameters = mainVstPluginParameters2;

            var samplerVstPlugin = "";
            if (this.BassPlayer.SamplerVstPlugin != null) samplerVstPlugin = this.BassPlayer.SamplerVstPlugin.Location;
            settings.SamplerVstPlugin = samplerVstPlugin;

            var samplerVstPluginParameters = "";
            if (this.BassPlayer.SamplerVstPlugin != null) samplerVstPluginParameters = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.SamplerVstPlugin);
            settings.SamplerVstPluginParameters = samplerVstPluginParameters;

            var samplerVstPlugin2 = "";
            if (this.BassPlayer.SamplerVstPlugin2 != null) samplerVstPlugin2 = this.BassPlayer.SamplerVstPlugin2.Location;
            settings.SamplerVstPlugin2 = samplerVstPlugin2;

            var samplerVstPluginParameters2 = "";
            if (this.BassPlayer.SamplerVstPlugin2 != null) samplerVstPluginParameters2 = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.SamplerVstPlugin2);
            settings.SamplerVstPlugin2Parameters = samplerVstPluginParameters2;

            var trackVstPlugin = "";
            if (this.BassPlayer.TrackVstPlugin != null) trackVstPlugin = this.BassPlayer.TrackVstPlugin.Location;
            settings.TrackVstPlugin = trackVstPlugin;

            var trackVstPluginParameters = "";
            if (this.BassPlayer.TrackVstPlugin != null) trackVstPluginParameters = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.TrackVstPlugin);
            settings.TrackVstPluginParameters = trackVstPluginParameters;

            var trackFxVstPlugin = "";
            if (this.BassPlayer.TrackSendFxvstPlugin != null) trackFxVstPlugin = this.BassPlayer.TrackSendFxvstPlugin.Location;
            settings.TrackFxvstPlugin = trackFxVstPlugin;

            var trackFxVstPluginParameters = "";
            if (this.BassPlayer.TrackSendFxvstPlugin != null) trackFxVstPluginParameters = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.TrackSendFxvstPlugin);
            settings.TrackFxvstPluginParameters = trackFxVstPluginParameters;

            var trackFxVstPlugin2 = "";
            if (this.BassPlayer.TrackSendFxvstPlugin2 != null) trackFxVstPlugin2 = this.BassPlayer.TrackSendFxvstPlugin2.Location;
            settings.TrackFxvstPlugin2 = trackFxVstPlugin2;

            var trackFxVstPluginParameters2 = "";
            if (this.BassPlayer.TrackSendFxvstPlugin2 != null) trackFxVstPluginParameters2 = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.TrackSendFxvstPlugin2);
            settings.TrackFxvstPlugin2Parameters = trackFxVstPluginParameters2;

            settings.Save();
            this.Close();
        }

        private void btnTrackVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.TrackVstPlugin == null) return;
            this.BassPlayer.ShowVstPluginConfig(this.BassPlayer.TrackVstPlugin);
        }

        private void btnTrackFXVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.TrackSendFxvstPlugin == null) return;
            this.BassPlayer.ShowVstPluginConfig(this.BassPlayer.TrackSendFxvstPlugin);
        }

        private void btnSamplerVSTPlugin2Config_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.SamplerVstPlugin2 == null) return;
            this.BassPlayer.ShowVstPluginConfig(this.BassPlayer.SamplerVstPlugin2);
        }

        private void cmbTrackFXVSTPlugins2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbTrackFXVSTPlugins2.SelectedIndex == -1) return;
            LoadTrackFxvstPlugin2(cmbTrackFXVSTPlugins2.SelectedValue.ToString());
        }

        private void btnTrackFXVSTPluginConfig2_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.TrackSendFxvstPlugin2 == null) return;
            this.BassPlayer.ShowVstPluginConfig(this.BassPlayer.TrackSendFxvstPlugin2);
        }

        private void btnMainMixerPluginConfig2_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.MainVstPlugin2 == null) return;
            this.BassPlayer.ShowVstPluginConfig(this.BassPlayer.MainVstPlugin2);
        }

        private void cmbMainMixerPlugins2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbMainMixerPlugins2.SelectedIndex == -1) return;
            LoadMainMixerVstPlugin2(cmbMainMixerPlugins2.SelectedValue.ToString());
        }
    }
}