using System;
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
            if (settings.LibraryFolder == "")
            {
                Application.Run(new FrmSettings());
            }
            if (settings.LibraryFolder != "")
            {
#if DEBUG
                Application.Run(new FrmMain());
#else
                try
                {
                    Application.Run(new FrmMain());
                }
                catch (Exception exception)
                {
                    CommonFunctions.LogException(exception);
                }
#endif
            }
        }
    }
}