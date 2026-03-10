using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmExportPlaylist : BaseForm
    {
        public FrmExportPlaylist()
        {
            InitializeComponent();
        }


        public ShufflerApplication Application { get; set; }
        public List<Track> Tracks { get; set; }
        public string PlaylistName { get; set; }

        private Image AlbumImage { get; set; }


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
            var albumArtists = Tracks.Select(t => t.Artist)
                .Union(Tracks.Select(t => t.AlbumArtist))
                .Distinct()
                .Where(aa => aa != "Various")
                .OrderBy(aa => Tracks.Count(t => t.Artist == aa) + Tracks.Count(t => t.AlbumArtist == aa))
                .ToList();

            if (albumArtists.Count > 1) albumArtists.Insert(0, "Various");

            cmbAlbumArtist.DataSource = albumArtists;

            txtAlbumName.Text = PlaylistName;
            if (txtAlbumName.Text == "" && albumArtists.Count == 1) txtAlbumName.Text = albumArtists[0];

            btnOK.Enabled = Tracks.Count > 0;


            txtOutputFolder.Text = Application.GetExportPlaylistFolder();
        }

        /// <summary>
        ///     Validates the data.
        /// </summary>
        /// <returns>True if valid</returns>
        private bool ValidateData()
        {
            txtOutputFolder.IsValid();
            txtAlbumName.IsValid();
            cmbAlbumArtist.IsValid();

            return txtOutputFolder.IsValid()
                   && txtAlbumName.IsValid()
                   && cmbAlbumArtist.IsValid();
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;

            progressDialog.Maximum = Tracks.Count;
            progressDialog.Title = "Exporting Playlist";
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
            var albumArtist = "";
            Invoke((MethodInvoker) delegate { albumArtist = cmbAlbumArtist.Text; });

            var destinationFolder = txtOutputFolder.Text;

            if (chkCreateSubfolder.Checked)
            {
                destinationFolder = Path.Combine(destinationFolder,
                    FileSystemHelper.StripInvalidFileNameChars(txtAlbumName.Text));
                if (!Directory.Exists(destinationFolder)) Directory.CreateDirectory(destinationFolder);
            }


            try
            {
                FileSystemHelper.DeleteFiles(destinationFolder, "*.mp3;*.jpg", false);
            }
            catch
            {
                // ignored
            }

            foreach (var track in Tracks.TakeWhile(track => !progressDialog.Cancelled))
            {
                progressDialog.Text = "Exporting " + track.Description;
                progressDialog.Details += "Copying track " + track.Description + "...";

                var destination = Path.Combine(destinationFolder, Path.GetFileName(track.Filename));
                try
                {
                    FileSystemHelper.Copy(track.Filename, destination);
                    if (progressDialog.Cancelled) break;


                    var destinationTrack = Application.LoadNonLibraryTrack(destination);

                    destinationTrack.AlbumArtist = albumArtist;
                    destinationTrack.Album = txtAlbumName.Text;
                    if (chkTrackNumbers.Checked)
                        destinationTrack.TrackNumber = Tracks.IndexOf(track) + 1;
                    else
                        destinationTrack.TrackNumber = 0;

                    Application.SaveNonLibraryTrack(destinationTrack);

                    if (AlbumImage != null)
                        Application.SetTrackAlbumCover(destinationTrack, AlbumImage);


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

            if (AlbumImage != null)
                ImageHelper.SaveJpg(Path.Combine(destinationFolder, "folder.jpg"), AlbumImage);

            progressDialog.Text = "Export completed.";

            Application.SetExportPlaylistFolder(txtOutputFolder.Text);
        }

        private void progressDialog_ProcessingCompleted(object sender, EventArgs e)
        {
        }

        private void txtAlbumImage_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var oldImage = AlbumImage;
                oldImage?.Dispose();

                var newImage = Image.FromFile(txtAlbumImage.Text);
                var isSmall = newImage.Width <= 750/2;

                var resizedImage = ImageHelper.ScaleAndCropImageToFit(newImage, new Size(750, 750));
                newImage.Dispose();

                if (isSmall)
                {
                    oldImage = resizedImage;
                    resizedImage = ImageHelper.MedianFilter(resizedImage, 2);
                    oldImage.Dispose();
                }

                AlbumImage = resizedImage;
            }
            catch
            {
                txtAlbumImage.Text = "";
                AlbumImage = null;
            }
        }
    }
}