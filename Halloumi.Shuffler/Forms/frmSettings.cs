using System;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmSettings : BaseForm
    {
        public FrmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            var settings = Settings.Default;
            txtLibraryFolder.Text = settings.LibraryFolder;
            txtShufflerFolder.Text = settings.ShufflerFolder;
            txtWinampPluginFolder.Text = settings.WaPluginsFolder;
            txtVSTPluginFolder.Text = settings.VstPluginsFolder;
            txtAnalogXScratchFolder.Text = settings.AnalogXScratchFolder;
            txtKeyFinderFolder.Text = settings.KeyFinderFolder;
            txtLoopFolder.Text = settings.LoopLibraryFolder;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var settings = Settings.Default;
            settings.LibraryFolder = txtLibraryFolder.Text;
            settings.ShufflerFolder = txtShufflerFolder.Text;
            settings.VstPluginsFolder = txtVSTPluginFolder.Text;
            settings.WaPluginsFolder = txtWinampPluginFolder.Text;
            settings.AnalogXScratchFolder = txtAnalogXScratchFolder.Text;
            settings.KeyFinderFolder = txtKeyFinderFolder.Text;
            settings.LoopLibraryFolder = txtLoopFolder.Text;


            settings.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}