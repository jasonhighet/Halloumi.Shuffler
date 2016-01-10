using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.Engine;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Forms
{
    public partial class frmImportShufflerFiles : BaseForm
    {
        public frmImportShufflerFiles()
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
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var folder = txtOutputFolder.Text;
            this.MixLibrary.ImportFromFolder(folder, chkDeleteAfterImport.Checked);
            this.Library.ImportShufflerDetails(folder, chkDeleteAfterImport.Checked);
            this.Library.LinkedSampleLibrary.ImportDetails(folder, chkDeleteAfterImport.Checked);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var settings = Settings.Default;
            settings.ImportShufflerFilesFolder = txtOutputFolder.Text;
            settings.Save();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}