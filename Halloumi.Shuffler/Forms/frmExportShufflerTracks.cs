using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioLibrary.Models;
using Halloumi.Shuffler;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmExportShufflerTracks : BaseForm
    {
        public FrmExportShufflerTracks()
        {
            InitializeComponent();
        }

        private List<Track> _tracks;


        public ShufflerApplication Application { get; set; }

        /// <summary>
        ///     Handles the Load event of the frmSettings control.
        /// </summary>
        private void frmSettings_Load(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        ///     Binds the data.
        /// </summary>
        private void BindData()
        {
            _tracks = Application.GetAllShufflerTracks();
            var sampleTracks = Application.GetAllSampleTracks().Where(track => _tracks.All(x => x.Filename != track.Filename));
            _tracks.AddRange(sampleTracks);

            btnOK.Enabled = _tracks.Count > 0;
            txtOutputFolder.Text = Application.GetExportPlaylistFolder();
        }

        /// <summary>
        ///     Validates the data.
        /// </summary>
        /// <returns>True if valid</returns>
        private bool ValidateData()
        {
            txtOutputFolder.IsValid();

            return txtOutputFolder.IsValid();
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;

            progressDialog.Maximum = _tracks.Count;
            progressDialog.Title = "Exporting Shuffler Files";
            progressDialog.CloseOnComplete = false;
            progressDialog.CloseOnCancel = true;
            progressDialog.Start();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        private void progressDialog_PerformProcessing(object sender, EventArgs e)
        {
            Application.ExportShufflerTracks(
                _tracks,
                txtOutputFolder.Text,
                chkCreateSubfolder.Checked,
                () => progressDialog.Cancelled,
                track =>
                {
                    progressDialog.Text = "Exporting " + track.Description;
                    progressDialog.Details += "Copying track " + track.Description + "...";
                    progressDialog.Value++;
                },
                (track, folder, message) =>
                {
                    progressDialog.Details += string.Format("{0}ERROR: Could not copy file '{1}' to '{2}'{0}{3}{0}",
                        Environment.NewLine,
                        track.Description,
                        folder,
                        message);
                });

            progressDialog.Text = "Export completed.";
        }

        private void progressDialog_ProcessingCompleted(object sender, EventArgs e)
        {
        }

    }
}