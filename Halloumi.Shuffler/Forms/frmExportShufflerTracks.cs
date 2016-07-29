using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmExportShufflerTracks : BaseForm
    {
        public FrmExportShufflerTracks()
        {
            InitializeComponent();
        }

        private List<Track> _tracks;


        public Library Library { get; set; }

        public SampleLibrary SampleLibrary { get; set; }

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
            _tracks = Library.GetTracks(shufflerFilter: Library.ShufflerFilter.ShuflerTracks);
            var sampleTracks = SampleLibrary.GetAllTracks().Where(track => _tracks.All(x => x.Filename != track.Filename));
            _tracks.AddRange(sampleTracks);

            btnOK.Enabled = _tracks.Count > 0;
            var settings = Settings.Default;
            txtOutputFolder.Text = settings.ExportPlaylistFolder;
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

            var destinationFolder = txtOutputFolder.Text;

            if (chkCreateSubfolder.Checked)
            {
                destinationFolder = Path.Combine(destinationFolder,
                    FileSystemHelper.StripInvalidFileNameChars("Library"));

                if (!Directory.Exists(destinationFolder)) Directory.CreateDirectory(destinationFolder);
            }

            try
            {
                FileSystemHelper.DeleteFiles(destinationFolder, "*.mp3;*.jpg", true);
            }
            catch
            {
                // ignored
            }

            foreach (var track in _tracks.TakeWhile(track => !progressDialog.Cancelled))
            {
                progressDialog.Text = "Exporting " + track.Description;
                progressDialog.Details += "Copying track " + track.Description + "...";

                var destination = track.Filename.Replace(Library.LibraryFolder, destinationFolder);
                try
                {


                    FileSystemHelper.Copy(track.Filename, destination);
                    if (progressDialog.Cancelled) break;

                    progressDialog.Details += "Done" + Environment.NewLine;
                }
                catch (Exception exception)
                {
                    var message = string.Format("{0}ERROR: Could not copy file '{1}' to '{2}'{0}{3}{0}",
                        Environment.NewLine,
                        track.Description,
                        destinationFolder,
                        exception.Message);

                    progressDialog.Details += message;

                    if (File.Exists(destination))
                    {
                        try
                        {
                            File.Delete(destination);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
                progressDialog.Value++;
            }


            progressDialog.Text = "Export completed.";

            var settings = Settings.Default;
            settings.ExportPlaylistFolder = txtOutputFolder.Text;
            settings.Save();
        }

        private void progressDialog_ProcessingCompleted(object sender, EventArgs e)
        {
        }

    }
}