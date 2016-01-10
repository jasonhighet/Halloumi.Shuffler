using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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
            WindowsApplicationHelper.InitialiseWindowsApplication();

            CommonFunctions.SetDefaultSettings();
            var settings = Settings.Default;
            if (settings.LibraryFolder == "")
            {
                Application.Run(new frmSettings());
            }
            if (settings.LibraryFolder != "")
            {
#if DEBUG
                 Application.Run(new frmMain());
#else
                try
                {
                    Application.Run(new frmMain());
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