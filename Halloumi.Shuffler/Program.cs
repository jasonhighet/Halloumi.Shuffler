using System;
using System.IO;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.Forms;
using Halloumi.Common.Windows.Helpers;

namespace Halloumi.Shuffler
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            DebugHelper.DebugMode = false;
            WindowsApplicationHelper.InitialiseWindowsApplication();

            CommonFunctions.SetDefaultSettings();
            var settings = Settings.Default;
            if (InvalidSettingsFolders(settings))
            {
                Application.Run(new FrmSettings());
            }
            if (!InvalidSettingsFolders(settings))
            {
                var application = new ShufflerApplication();


#if DEBUG
                Application.Run(new FrmMain(application));
#else
                try
                {
                    Application.Run(new FrmMain(application));
                }
                catch (Exception exception)
                {
                    CommonFunctions.LogException(exception);
                }
#endif
            }
        }

        private static bool InvalidSettingsFolders(Settings settings)
        {
            return settings.LibraryFolder == "" || !Directory.Exists(settings.LibraryFolder)
                   || settings.ShufflerFolder == "" || !Directory.Exists(settings.ShufflerFolder);
        }
    }
}