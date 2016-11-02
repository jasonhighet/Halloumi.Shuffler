using System;
using System.Collections.Generic;
using System.Linq;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Plugins;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmVstChain : BaseForm
    {
        private Channel _channel;
        private List<PluginModel> _plugins;

        public FrmVstChain()
        {
            InitializeComponent();
        }

        public void Initialise(Channel channel)
        {
            _channel = channel;
            _plugins = PluginHelper.FindVstPlugins().Select(plugin => new PluginModel
            {
                Location = plugin.Location,
                Name = plugin.Name
            }).ToList();

            listBuilder.PropertiesButtonVisible = true;
            BindData();
        }

        public void BindData()
        {
            listBuilder.SetAvailableItems(_plugins.Select(x => x.Name).ToList());

            var selectedPlugins = _channel.VstPlugins.Where(x=> x!= null).Select(x => x.Name).ToList();
            listBuilder.SetSelectedItems(selectedPlugins);
        }

        private void listBuilder_SelectedItemsChanged(object sender, EventArgs e)
        {
            var selectedPlugins = GetSelectedPluginModels();

            _channel.ClearVstPlugins();
            foreach (var plugin in selectedPlugins)
            {
                _channel.LoadVstPlugin(plugin.Location, selectedPlugins.IndexOf(plugin));
            }
        }

        private List<PluginModel> GetSelectedPluginModels()
        {
            var selectedPlugins = listBuilder
                .GetSelectedItems()
                .Select(GetPluginModelByName)
                .Where(x => x != null)
                .ToList();
            return selectedPlugins;
        }

        private List<PluginModel> GetSelectedSelectedPluginModels()
        {
            var selectedPlugins = listBuilder
                .GetSelectedSelectedItems()
                .Select(GetPluginModelByName)
                .Where(x => x != null)
                .ToList();
            return selectedPlugins;
        }


        private PluginModel GetPluginModelByName(string selectedItem)
        {
            return _plugins.FirstOrDefault(plugin => plugin.Name == selectedItem);
        }

        private class PluginModel
        {
            public string Location { get; set; }
            public string Name { get; set; }
        }

        private void listBuilder_PropertiesClicked(object sender, EventArgs e)
        {
            var pluginModel = GetSelectedSelectedPluginModels().FirstOrDefault();
            if(pluginModel == null) 
                return;

            var plugin = _channel.VstPlugins.FirstOrDefault(x => string.Equals(x.Name, pluginModel.Name, StringComparison.CurrentCultureIgnoreCase));

            PluginHelper.ShowVstPluginConfig(plugin);
        }
    }
}