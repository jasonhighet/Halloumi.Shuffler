using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioLibrary;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmImportShufflerFiles : BaseForm
    {
        public FrmImportShufflerFiles()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the frmImportExportShufflerFiles control.
        /// </summary>
        private void frmImportExportShufflerFiles_Load(object sender, EventArgs e)
        {
            var settings = Settings.Default;
            var folder = settings.ImportShufflerFilesFolder;
            if (folder == "")
                folder = @"F:\Music\Shuffler\";

            if (!Directory.Exists(folder)) folder = @"";
            txtOutputFolder.Text = folder;
        }

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Library Library { get; set; }

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MixLibrary MixLibrary { get; set; }

        /// <summary>
        /// Handles the Click event of the btnOK control.
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            btnOK.Enabled = false;
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var folder = txtOutputFolder.Text;
            MixLibrary.ImportFromFolder(folder, chkDeleteAfterImport.Checked);
            Library.ImportShufflerDetails(folder, chkDeleteAfterImport.Checked);
            Library.LinkedSampleLibrary.ImportDetails(folder, chkDeleteAfterImport.Checked);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var settings = Settings.Default;
            settings.ImportShufflerFilesFolder = txtOutputFolder.Text;
            settings.Save();

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}