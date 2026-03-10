using System;
using System.IO;
using Halloumi.Common.Windows.Forms;

namespace Halloumi.Shuffler.Forms
{
    public partial class frmImportShufflerTracks : BaseForm
    {
        public frmImportShufflerTracks()
        {
            InitializeComponent();
        }

        public ShufflerApplication Application { get; set; }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtImportFolder.Text == Application.GetLibraryFolder())
                return;

            if (!Directory.Exists(txtImportFolder.Text))
                return;


            Application.ImportExternalShufflerTracks(txtImportFolder.Text);

            Close();
        }
    }
}