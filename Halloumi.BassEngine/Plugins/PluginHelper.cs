using Halloumi.Common.Helpers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Un4seen.Bass.AddOn.Vst;
using Un4seen.Bass.AddOn.WaDsp;

namespace Halloumi.Shuffler.AudioEngine.Plugins
{
    public static class PluginHelper
    {
        /// <summary>
        ///     Gets or sets the WA plug-ins folder.
        /// </summary>
        public static string WaPluginsFolder { get; set; }

        /// <summary>
        ///     Gets or sets the VST plug-ins folder.
        /// </summary>
        public static string VstPluginsFolder { get; set; }

        static PluginHelper()
        {
            WaPluginsFolder = @"C:\Program Files (x86)\Winamp\Plugins\";
            VstPluginsFolder = @"C:\Program Files (x86)\Steinberg\VstPlugins\";
        }

        /// <summary>
        ///     Returns a list of all available (unloaded) VST plug-ins
        /// </summary>
        /// <returns>A list of all available (unloaded) VST plug-ins</returns>
        public static List<VstPlugin> FindVstPlugins()
        {
            if (!Directory.Exists(VstPluginsFolder)) return new List<VstPlugin>();

            return Directory
                .GetFiles(VstPluginsFolder, "*.dll", SearchOption.AllDirectories)
                .Select(filename => new VstPlugin
                {
                    Location = filename,
                    Name = GetPluginNameFromFileName(filename)
                }).ToList();
        }

        /// <summary>
        ///     Shows the WinAmp DSP plug-in configuration screen
        /// </summary>
        /// <param name="plugin">The WinAmp DSP plug-in.</param>
        public static void ShowWaPluginConfig(WaPlugin plugin)
        {
            if (plugin == null) return;

            BassWaDsp.BASS_WADSP_Config(plugin.Id);
        }


        /// <summary>
        ///     Shows the VST plug-in configuration screen.
        /// </summary>
        /// <param name="plugin">The plug-in.</param>
        public static void ShowVstPluginConfig(VstPlugin plugin)
        {
            if (plugin == null) return;

            if (plugin.Form != null && !plugin.Form.IsDisposed)
            {
                plugin.Form.Show();
                plugin.Form.BringToFront();
                return;
            }

            plugin.Form = null;

            var containerForm = new VstPluginConfigForm(plugin);
            containerForm.Show();
        }

        /// <summary>
        ///     Returns a list of all available (unloaded) WinAmp DSP plug-ins
        /// </summary>
        /// <returns>A list of all available (unloaded) WinAmp DSP plug-ins</returns>
        public static List<WaPlugin> FindWaPlugins()
        {
            var plugins = new List<WaPlugin>();
            if (WaPluginsFolder == "") return plugins;

            plugins = Directory.GetFiles(WaPluginsFolder, "dsp_*.dll", SearchOption.AllDirectories)
                .Select(filename => new WaPlugin
                {
                    Location = filename,
                    Name = GetPluginNameFromFileName(filename)
                }).ToList();

            return plugins;
        }

        private static string GetPluginNameFromFileName(string pluginLocation)
        {
            return StringHelper.TitleCase((Path.GetFileNameWithoutExtension(pluginLocation) + "")
                .Replace("dsp_", "")
                .Replace("_", " ")
                .ToLower());
        }

        /// <summary>
        ///     Gets the VST plug-in settings.
        /// </summary>
        /// <param name="plugin">The plug-in.</param>
        /// <returns>The settings as a key=value comma delimited list</returns>
        public static string GetVstPluginParameters(VstPlugin plugin)
        {
            var values = new List<string>();
            var parameterCount = BassVst.BASS_VST_GetParamCount(plugin.Id);
            for (var i = 0; i < parameterCount; i++)
            {
                var value = BassVst.BASS_VST_GetParam(plugin.Id, i);
                values.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            return string.Join(",", values.ToArray());
        }

        /// <summary>
        ///     Sets the VST plug-in settings.
        /// </summary>
        /// <param name="plugin">The plug-in.</param>
        /// <param name="parameters">The parameters.</param>
        public static void SetVstPluginParameters(VstPlugin plugin, string parameters)
        {
            if (parameters.Trim() == "") return;
            var values = parameters.Split(',').ToList();
            var parameterCount = BassVst.BASS_VST_GetParamCount(plugin.Id);
            for (var i = 0; i < parameterCount; i++)
            {
                try
                {
                    if (i >= values.Count) continue;
                    var value = float.Parse(values[i]);
                    BassVst.BASS_VST_SetParam(plugin.Id, i, value);
                }
                catch
                {
                    // ignored
                }
            }
        }

        public static void SetVstPluginPreset(VstPlugin plugin, int presetId)
        {
            BassVst.BASS_VST_SetProgram(plugin.Id, presetId);
        }

        public static List<VstPluginPreset> GetPluginPresets(VstPlugin plugin)
        {
            var names = BassVst.BASS_VST_GetProgramNames(plugin.Id).ToList();
            return names.Select(name => new VstPluginPreset()
            {
                Id = names.IndexOf(name),
                Name = name.Trim()
            })
            .Where(x => x.Name != "")
            .ToList();
        }

        public class VstPluginPreset
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

    }
}
