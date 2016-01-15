using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Common.Helpers;

namespace Halloumi.Shuffler.Forms
{
    public static class CommonFunctions
    {
        /// <summary>
        /// Gets a list of default track numbers.
        /// </summary>
        /// <returns>A list of default track numbers.</returns>
        public static List<string> GetDefaultTrackNumbers()
        {
            var trackNumbers = new List<string>();
            trackNumbers.Add("");
            for (var i = 1; i <= 80; i++) trackNumbers.Add(i.ToString());
            return trackNumbers;
        }

        /// <summary>
        /// Sets the default settings.
        /// </summary>
        public static void SetDefaultSettings()
        {
            var settings = Settings.Default;

            if (Environment.MachineName == "JASON-LAPTOP")
            {
                if (settings.LibraryFolder == "") settings.LibraryFolder = @"E:\Music\Library";
                if (settings.PlaylistFolder == "") settings.PlaylistFolder = @"E:\Music\Playlists";
                if (settings.ShufflerFolder == "") settings.ShufflerFolder = @"E:\Music\Shuffler";
                if (settings.WaPluginsFolder == "") settings.WaPluginsFolder = @"C:\Program Files\Winamp\Plugins";
                if (settings.VstPluginsFolder == "") settings.VstPluginsFolder = @"C:\Program Files\Steinberg\VstPlugins";
                if (settings.AnalogXScratchFolder == "") settings.AnalogXScratchFolder = @"C:\Program Files\AnalogX\Scratch";
                if (settings.KeyFinderFolder == "") settings.KeyFinderFolder = @"C:\Program Files\KeyFinderFolder";
                settings.Save();
            }
            else
            {
                if (settings.LibraryFolder == "") settings.LibraryFolder = FindDefaultFolder(@"Music\Library");
                if (settings.PlaylistFolder == "") settings.PlaylistFolder = FindDefaultFolder(@"Music\Playlists");
                if (settings.ShufflerFolder == "") settings.ShufflerFolder = FindDefaultFolder(@"Music\Shuffler");
                if (settings.WaPluginsFolder == "") settings.WaPluginsFolder = FindDefaultFolder(@"Program Files\Winamp\Plugins");
                if (settings.VstPluginsFolder == "") settings.VstPluginsFolder = FindDefaultFolder(@"Program Files\Steinberg\VstPlugins");
                if (settings.AnalogXScratchFolder == "") settings.AnalogXScratchFolder = FindDefaultFolder(@"Program Files\AnalogX\Scratch");
                if (settings.KeyFinderFolder == "") FindDefaultFolder(@"Program Files\KeyFinderFolder");
                settings.Save();
            }
        }

        private static string FindDefaultFolder(string subfolder)
        {
            var drives = new string[] { "C", "D", "E", "F" }.ToList();
            foreach (var drive in drives)
            {
                var path = drive + @":\" + subfolder;
                if (Directory.Exists(path)) return path;
            }

            return "";
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void LogException(Exception exception)
        {
            var text = exception + Environment.NewLine + exception.StackTrace;
            try
            {
                var logFile = string.Format("ErrorLog_{0}.log", DateTime.Now.ToString("yyyyMMddhhmmss"));
                logFile = Path.Combine(Settings.Default.ShufflerFolder, logFile);
                File.WriteAllText(logFile, text);
                DebugHelper.WriteLine(text);
            }
            catch (Exception)
            {
                DebugHelper.WriteLine(text);
            }
        }
    }
}