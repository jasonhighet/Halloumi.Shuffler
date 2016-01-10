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
    public partial class frmPlugin : BaseForm
    {
        private BE.BassPlayer BassPlayer { get; set; }

        public frmPlugin(BE.BassPlayer bassPlayer)
        {
            InitializeComponent();
            this.BassPlayer = bassPlayer;
            BindData();
        }

        private void cmbVSTPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbVSTPlugins.SelectedIndex == -1) return;
            LoadMainVSTPlugin(cmbVSTPlugins.SelectedValue.ToString());
        }

        private void cmbWAPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbWAPlugins.SelectedIndex == -1) return;
            LoadWAPlugin(cmbWAPlugins.SelectedValue.ToString());
        }

        private void cmbSampleVSTPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbSamplerVSTPlugins.SelectedIndex == -1) return;
            LoadSamplerVSTPlugin(cmbSamplerVSTPlugins.SelectedValue.ToString());
        }

        private void cmbTrackVSTPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbTrackVSTPlugins.SelectedIndex == -1) return;
            LoadTrackVSTPlugin(cmbTrackVSTPlugins.SelectedValue.ToString());
        }

        private void cmbSamplerVSTPlugins2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbSamplerVSTPlugins2.SelectedIndex == -1) return;
            LoadSamplerVSTPlugin2(cmbSamplerVSTPlugins2.SelectedValue.ToString());
        }

        private void cmbTrackFXVSTPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbTrackFXVSTPlugins.SelectedIndex == -1) return;
            LoadTrackFXVSTPlugin(cmbTrackFXVSTPlugins.SelectedValue.ToString());
        }

        private void btnWAPluginConfig_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.WAPlugin == null) return;
            this.BassPlayer.ShowWAPluginConfig(this.BassPlayer.WAPlugin);
        }

        private void btnVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.MainVSTPlugin == null) return;
            this.BassPlayer.ShowVSTPluginConfig(this.BassPlayer.MainVSTPlugin);
        }

        private class VSTPluginModel
        {
            public string Location { get; set; }

            public string Name { get; set; }
        }

        public class WAPluginModel
        {
            public string Location { get; set; }

            public string Name { get; set; }
        }

        public void LoadWAPlugin(string location)
        {
            try
            {
                if (location == this.CurrentWAPluginLocation) return;
                this.BassPlayer.UnloadAllWAPlugins();
                if (location != "") this.BassPlayer.LoadWAPlugin(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        public void LoadMainVSTPlugin(string location)
        {
            try
            {
                if (location == this.MainVSTPluginLocation) return;
                this.BassPlayer.ClearMainVSTPlugin();
                if (location != "") this.BassPlayer.LoadMainVSTPlugin(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        public void LoadMainMixerVSTPlugin2(string location)
        {
            try
            {
                if (location == this.MainVSTPluginLocation2) return;
                this.BassPlayer.ClearMainVSTPlugin2();
                if (location != "") this.BassPlayer.LoadMainVSTPlugin2(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        public void LoadSamplerVSTPlugin(string location)
        {
            try
            {
                if (location == this.CurrentSamplerVSTPluginLocation) return;
                this.BassPlayer.ClearSamplerVSTPlugin();
                if (location != "") this.BassPlayer.LoadSamplerVSTPlugin(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        public void LoadSamplerVSTPlugin2(string location)
        {
            try
            {
                if (location == this.CurrentSamplerVSTPluginLocation2) return;
                this.BassPlayer.ClearSamplerVSTPlugin2();
                if (location != "") this.BassPlayer.LoadSamplerVSTPlugin2(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        public void LoadTrackVSTPlugin(string location)
        {
            try
            {
                if (location == this.CurrentTrackVSTPluginLocation) return;
                this.BassPlayer.ClearTracksVSTPlugin();
                if (location != "") this.BassPlayer.LoadTracksVSTPlugin(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        public void LoadTrackFXVSTPlugin(string location)
        {
            try
            {
                if (location == this.CurrentTrackFXVSTPluginLocation) return;
                this.BassPlayer.ClearTrackSendFXVSTPlugin();
                if (location != "") this.BassPlayer.LoadTrackSendFXVSTPlugin(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        public void LoadTrackFXVSTPlugin2(string location)
        {
            try
            {
                if (location == this.CurrentTrackFXVSTPluginLocation2) return;
                this.BassPlayer.ClearTrackSendFXVSTPlugin2();
                if (location != "") this.BassPlayer.LoadTrackSendFXVSTPlugin2(location);
            }
            catch (Exception e)
            {
                base.HandleException(e);
            }
        }

        private void BindData()
        {
            _binding = true;

            BindWAPlugins();
            BindMainVSTPlugins();
            BindMainVSTPlugins2();
            BindSamplerVSTPlugins();
            BindSamplerVSTPlugins2();
            BindTrackVSTPlugins();
            BindTrackFXVSTPlugins();
            BindTrackFXVSTPlugins2();

            _binding = false;
        }

        private bool _binding = false;

        public void BindWAPlugins()
        {
            var plugins = new List<WAPluginModel>();
            plugins.Add(new WAPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in this.BassPlayer.FindWAPlugins())
            {
                var model = new WAPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbWAPlugins.Items.Clear();
            cmbWAPlugins.DisplayMember = "Name";
            cmbWAPlugins.ValueMember = "Location";
            cmbWAPlugins.DataSource = plugins;

            cmbWAPlugins.SelectedIndex = 0;
            for (int i = 0; i < cmbWAPlugins.Items.Count; i++)
            {
                var plugin = cmbWAPlugins.Items[i] as WAPluginModel;
                if (plugin.Location == this.CurrentWAPluginLocation)
                {
                    cmbWAPlugins.SelectedIndex = i;
                    break;
                }
            }
        }

        public void BindMainVSTPlugins()
        {
            var plugins = new List<VSTPluginModel>();
            plugins.Add(new VSTPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in this.BassPlayer.FindVSTPlugins())
            {
                var model = new VSTPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbVSTPlugins.Items.Clear();
            cmbVSTPlugins.DisplayMember = "Name";
            cmbVSTPlugins.ValueMember = "Location";
            cmbVSTPlugins.DataSource = plugins;

            cmbVSTPlugins.SelectedIndex = 0;
            for (int i = 0; i < cmbVSTPlugins.Items.Count; i++)
            {
                var plugin = cmbVSTPlugins.Items[i] as VSTPluginModel;
                if (plugin.Location == this.MainVSTPluginLocation)
                {
                    cmbVSTPlugins.SelectedIndex = i;
                    break;
                }
            }
        }

        public void BindMainVSTPlugins2()
        {
            var plugins = new List<VSTPluginModel>();
            plugins.Add(new VSTPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in this.BassPlayer.FindVSTPlugins())
            {
                var model = new VSTPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbMainMixerPlugins2.Items.Clear();
            cmbMainMixerPlugins2.DisplayMember = "Name";
            cmbMainMixerPlugins2.ValueMember = "Location";
            cmbMainMixerPlugins2.DataSource = plugins;

            cmbMainMixerPlugins2.SelectedIndex = 0;
            for (int i = 0; i < cmbMainMixerPlugins2.Items.Count; i++)
            {
                var plugin = cmbMainMixerPlugins2.Items[i] as VSTPluginModel;
                if (plugin.Location == this.MainVSTPluginLocation2)
                {
                    cmbMainMixerPlugins2.SelectedIndex = i;
                    break;
                }
            }
        }

        public void BindSamplerVSTPlugins()
        {
            var plugins = new List<VSTPluginModel>();
            plugins.Add(new VSTPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in this.BassPlayer.FindVSTPlugins())
            {
                var model = new VSTPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbSamplerVSTPlugins.Items.Clear();
            cmbSamplerVSTPlugins.DisplayMember = "Name";
            cmbSamplerVSTPlugins.ValueMember = "Location";
            cmbSamplerVSTPlugins.DataSource = plugins;

            cmbSamplerVSTPlugins.SelectedIndex = 0;
            for (int i = 0; i < cmbSamplerVSTPlugins.Items.Count; i++)
            {
                var plugin = cmbSamplerVSTPlugins.Items[i] as VSTPluginModel;
                if (plugin.Location == this.CurrentSamplerVSTPluginLocation)
                {
                    cmbSamplerVSTPlugins.SelectedIndex = i;
                    break;
                }
            }
        }

        public void BindSamplerVSTPlugins2()
        {
            var plugins = new List<VSTPluginModel>();
            plugins.Add(new VSTPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in this.BassPlayer.FindVSTPlugins())
            {
                var model = new VSTPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbSamplerVSTPlugins2.Items.Clear();
            cmbSamplerVSTPlugins2.DisplayMember = "Name";
            cmbSamplerVSTPlugins2.ValueMember = "Location";
            cmbSamplerVSTPlugins2.DataSource = plugins;

            cmbSamplerVSTPlugins2.SelectedIndex = 0;
            for (int i = 0; i < cmbSamplerVSTPlugins2.Items.Count; i++)
            {
                var plugin = cmbSamplerVSTPlugins2.Items[i] as VSTPluginModel;
                if (plugin.Location == this.CurrentSamplerVSTPluginLocation2)
                {
                    cmbSamplerVSTPlugins2.SelectedIndex = i;
                    break;
                }
            }
        }

        public void BindTrackVSTPlugins()
        {
            var plugins = new List<VSTPluginModel>();
            plugins.Add(new VSTPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in this.BassPlayer.FindVSTPlugins())
            {
                var model = new VSTPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbTrackVSTPlugins.Items.Clear();
            cmbTrackVSTPlugins.DisplayMember = "Name";
            cmbTrackVSTPlugins.ValueMember = "Location";
            cmbTrackVSTPlugins.DataSource = plugins;

            cmbTrackVSTPlugins.SelectedIndex = 0;
            for (int i = 0; i < cmbTrackVSTPlugins.Items.Count; i++)
            {
                var plugin = cmbTrackVSTPlugins.Items[i] as VSTPluginModel;
                if (plugin.Location == this.CurrentTrackVSTPluginLocation)
                {
                    cmbTrackVSTPlugins.SelectedIndex = i;
                    break;
                }
            }
        }

        public void BindTrackFXVSTPlugins()
        {
            var plugins = new List<VSTPluginModel>();
            plugins.Add(new VSTPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in this.BassPlayer.FindVSTPlugins())
            {
                var model = new VSTPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbTrackFXVSTPlugins.Items.Clear();
            cmbTrackFXVSTPlugins.DisplayMember = "Name";
            cmbTrackFXVSTPlugins.ValueMember = "Location";
            cmbTrackFXVSTPlugins.DataSource = plugins;

            cmbTrackFXVSTPlugins.SelectedIndex = 0;
            for (int i = 0; i < cmbTrackFXVSTPlugins.Items.Count; i++)
            {
                var plugin = cmbTrackFXVSTPlugins.Items[i] as VSTPluginModel;
                if (plugin.Location == this.CurrentTrackFXVSTPluginLocation)
                {
                    cmbTrackFXVSTPlugins.SelectedIndex = i;
                    break;
                }
            }
        }

        public void BindTrackFXVSTPlugins2()
        {
            var plugins = new List<VSTPluginModel>();
            plugins.Add(new VSTPluginModel()
            {
                Location = "",
                Name = "(None)"
            });

            foreach (var plugin in this.BassPlayer.FindVSTPlugins())
            {
                var model = new VSTPluginModel();
                model.Location = plugin.Location;
                model.Name = plugin.Name;
                plugins.Add(model);
            }

            cmbTrackFXVSTPlugins2.Items.Clear();
            cmbTrackFXVSTPlugins2.DisplayMember = "Name";
            cmbTrackFXVSTPlugins2.ValueMember = "Location";
            cmbTrackFXVSTPlugins2.DataSource = plugins;

            cmbTrackFXVSTPlugins2.SelectedIndex = 0;
            for (int i = 0; i < cmbTrackFXVSTPlugins2.Items.Count; i++)
            {
                var plugin = cmbTrackFXVSTPlugins2.Items[i] as VSTPluginModel;
                if (plugin.Location == this.CurrentTrackFXVSTPluginLocation2)
                {
                    cmbTrackFXVSTPlugins2.SelectedIndex = i;
                    break;
                }
            }
        }

        public string MainVSTPluginLocation
        {
            get
            {
                if (this.BassPlayer.MainVSTPlugin == null) return "";
                return this.BassPlayer.MainVSTPlugin.Location;
            }
        }

        public string MainVSTPluginLocation2
        {
            get
            {
                if (this.BassPlayer.MainVSTPlugin2 == null) return "";
                return this.BassPlayer.MainVSTPlugin2.Location;
            }
        }

        public string CurrentSamplerVSTPluginLocation
        {
            get
            {
                if (this.BassPlayer.SamplerVSTPlugin == null) return "";
                return this.BassPlayer.SamplerVSTPlugin.Location;
            }
        }

        public string CurrentSamplerVSTPluginLocation2
        {
            get
            {
                if (this.BassPlayer.SamplerVSTPlugin2 == null) return "";
                return this.BassPlayer.SamplerVSTPlugin2.Location;
            }
        }

        public string CurrentTrackVSTPluginLocation
        {
            get
            {
                if (this.BassPlayer.TrackVSTPlugin == null) return "";
                return this.BassPlayer.TrackVSTPlugin.Location;
            }
        }

        public string CurrentTrackFXVSTPluginLocation
        {
            get
            {
                if (this.BassPlayer.TrackSendFXVSTPlugin == null) return "";
                return this.BassPlayer.TrackSendFXVSTPlugin.Location;
            }
        }

        public string CurrentTrackFXVSTPluginLocation2
        {
            get
            {
                if (this.BassPlayer.TrackSendFXVSTPlugin2 == null) return "";
                return this.BassPlayer.TrackSendFXVSTPlugin2.Location;
            }
        }

        public string CurrentWAPluginLocation
        {
            get
            {
                if (this.BassPlayer.WAPlugin == null) return "";
                return this.BassPlayer.WAPlugin.Location;
            }
        }

        private void btnSamplerVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.SamplerVSTPlugin == null) return;
            this.BassPlayer.ShowVSTPluginConfig(this.BassPlayer.SamplerVSTPlugin);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var settings = Settings.Default;

            var winampPlugin = "";
            if (this.BassPlayer.WAPlugin != null) winampPlugin = this.BassPlayer.WAPlugin.Location;
            settings.WAPlugin = winampPlugin;

            var mainVstPlugin = "";
            if (this.BassPlayer.MainVSTPlugin != null) mainVstPlugin = this.BassPlayer.MainVSTPlugin.Location;
            settings.MainMixerVSTPlugin = mainVstPlugin;

            var mainVstPluginParameters = "";
            if (this.BassPlayer.MainVSTPlugin != null) mainVstPluginParameters = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.MainVSTPlugin);
            settings.MainMixerVSTPluginParameters = mainVstPluginParameters;

            var mainVstPlugin2 = "";
            if (this.BassPlayer.MainVSTPlugin2 != null) mainVstPlugin2 = this.BassPlayer.MainVSTPlugin2.Location;
            settings.MainMixerVSTPlugin2 = mainVstPlugin2;

            var mainVstPluginParameters2 = "";
            if (this.BassPlayer.MainVSTPlugin2 != null) mainVstPluginParameters2 = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.MainVSTPlugin2);
            settings.MainMixerVSTPlugin2Parameters = mainVstPluginParameters2;

            var samplerVstPlugin = "";
            if (this.BassPlayer.SamplerVSTPlugin != null) samplerVstPlugin = this.BassPlayer.SamplerVSTPlugin.Location;
            settings.SamplerVSTPlugin = samplerVstPlugin;

            var samplerVstPluginParameters = "";
            if (this.BassPlayer.SamplerVSTPlugin != null) samplerVstPluginParameters = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.SamplerVSTPlugin);
            settings.SamplerVSTPluginParameters = samplerVstPluginParameters;

            var samplerVstPlugin2 = "";
            if (this.BassPlayer.SamplerVSTPlugin2 != null) samplerVstPlugin2 = this.BassPlayer.SamplerVSTPlugin2.Location;
            settings.SamplerVSTPlugin2 = samplerVstPlugin2;

            var samplerVstPluginParameters2 = "";
            if (this.BassPlayer.SamplerVSTPlugin2 != null) samplerVstPluginParameters2 = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.SamplerVSTPlugin2);
            settings.SamplerVSTPlugin2Parameters = samplerVstPluginParameters2;

            var trackVstPlugin = "";
            if (this.BassPlayer.TrackVSTPlugin != null) trackVstPlugin = this.BassPlayer.TrackVSTPlugin.Location;
            settings.TrackVSTPlugin = trackVstPlugin;

            var trackVstPluginParameters = "";
            if (this.BassPlayer.TrackVSTPlugin != null) trackVstPluginParameters = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.TrackVSTPlugin);
            settings.TrackVSTPluginParameters = trackVstPluginParameters;

            var trackFXVstPlugin = "";
            if (this.BassPlayer.TrackSendFXVSTPlugin != null) trackFXVstPlugin = this.BassPlayer.TrackSendFXVSTPlugin.Location;
            settings.TrackFXVSTPlugin = trackFXVstPlugin;

            var trackFXVstPluginParameters = "";
            if (this.BassPlayer.TrackSendFXVSTPlugin != null) trackFXVstPluginParameters = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.TrackSendFXVSTPlugin);
            settings.TrackFXVSTPluginParameters = trackFXVstPluginParameters;

            var trackFXVstPlugin2 = "";
            if (this.BassPlayer.TrackSendFXVSTPlugin2 != null) trackFXVstPlugin2 = this.BassPlayer.TrackSendFXVSTPlugin2.Location;
            settings.TrackFXVSTPlugin2 = trackFXVstPlugin2;

            var trackFXVstPluginParameters2 = "";
            if (this.BassPlayer.TrackSendFXVSTPlugin2 != null) trackFXVstPluginParameters2 = this.BassPlayer.GetVstPluginParameters(this.BassPlayer.TrackSendFXVSTPlugin2);
            settings.TrackFXVSTPlugin2Parameters = trackFXVstPluginParameters2;

            settings.Save();
            this.Close();
        }

        private void btnTrackVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.TrackVSTPlugin == null) return;
            this.BassPlayer.ShowVSTPluginConfig(this.BassPlayer.TrackVSTPlugin);
        }

        private void btnTrackFXVSTPluginConfig_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.TrackSendFXVSTPlugin == null) return;
            this.BassPlayer.ShowVSTPluginConfig(this.BassPlayer.TrackSendFXVSTPlugin);
        }

        private void btnSamplerVSTPlugin2Config_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.SamplerVSTPlugin2 == null) return;
            this.BassPlayer.ShowVSTPluginConfig(this.BassPlayer.SamplerVSTPlugin2);
        }

        private void cmbTrackFXVSTPlugins2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbTrackFXVSTPlugins2.SelectedIndex == -1) return;
            LoadTrackFXVSTPlugin2(cmbTrackFXVSTPlugins2.SelectedValue.ToString());
        }

        private void btnTrackFXVSTPluginConfig2_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.TrackSendFXVSTPlugin2 == null) return;
            this.BassPlayer.ShowVSTPluginConfig(this.BassPlayer.TrackSendFXVSTPlugin2);
        }

        private void btnMainMixerPluginConfig2_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.MainVSTPlugin2 == null) return;
            this.BassPlayer.ShowVSTPluginConfig(this.BassPlayer.MainVSTPlugin2);
        }

        private void cmbMainMixerPlugins2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            if (cmbMainMixerPlugins2.SelectedIndex == -1) return;
            LoadMainMixerVSTPlugin2(cmbMainMixerPlugins2.SelectedValue.ToString());
        }
    }
}