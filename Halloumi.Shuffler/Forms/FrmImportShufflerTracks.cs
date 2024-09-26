using System;
using System.IO;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioLibrary;

namespace Halloumi.Shuffler.Forms
{
    public partial class frmImportShufflerTracks : BaseForm
    {
        public frmImportShufflerTracks()
        {
            InitializeComponent();
        }

        public Library Library { get; set; }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtImportFolder.Text == Library.LibraryFolder)
                return;

            if (!Directory.Exists(txtImportFolder.Text))
                return;


            Library.ImportExternalShufflerTracks(txtImportFolder.Text);

            Close();
        }
    }
}