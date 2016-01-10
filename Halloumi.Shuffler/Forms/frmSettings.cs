using System;
using Halloumi.Common.Windows.Forms;

namespace Halloumi.Shuffler.Forms
{
    public partial class frmSettings : BaseForm
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Validates the data.
        /// </summary>
        /// <returns>True if valid</returns>
        private bool ValidateData()
        {
            return txtLibraryFolder.IsValid()
                && txtPlaylistFolder.IsValid()
                && txtShufflerFolder.IsValid()
                && txtWinampPluginFolder.IsValid()
                && txtVSTPluginFolder.IsValid()
                && txtAnalogXScratchFolder.IsValid()
                && txtKeyFinderFolder.IsValid();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            var settings = Settings.Default;
            txtLibraryFolder.Text = settings.LibraryFolder;
            txtPlaylistFolder.Text = settings.PlaylistFolder;
            txtShufflerFolder.Text = settings.ShufflerFolder;
            txtWinampPluginFolder.Text = settings.WAPluginsFolder;
            txtVSTPluginFolder.Text = settings.VSTPluginsFolder;
            txtAnalogXScratchFolder.Text = settings.AnalogXScratchFolder;
            txtKeyFinderFolder.Text = settings.KeyFinderFolder;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           // if (!this.ValidateData()) return;

            var settings = Settings.Default;
            settings.LibraryFolder = txtLibraryFolder.Text;
            settings.PlaylistFolder = txtPlaylistFolder.Text;
            settings.ShufflerFolder = txtShufflerFolder.Text;
            settings.VSTPluginsFolder = txtVSTPluginFolder.Text;
            settings.WAPluginsFolder = txtWinampPluginFolder.Text;
            settings.AnalogXScratchFolder = txtAnalogXScratchFolder.Text;
            settings.KeyFinderFolder = txtKeyFinderFolder.Text;
            settings.Save();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}