using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmUpdateTrackDetails : BaseForm
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the frmUpdateTrackDetails class.
        /// </summary>
        public FrmUpdateTrackDetails()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the frmUpdateTitle control.
        /// </summary>
        private void frmUpdateTitle_Load(object sender, EventArgs e)
        {
            BindData();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Binds the data for the form to the controls
        /// </summary>
        private void BindData()
        {
            cmbArtist.DataSource = Library.GetAllArtists();
            cmbArtist.DisplayMember = "Name";
            cmbArtist.ValueMember = "Name";

            cmbAlbumArtist.DataSource = Library.GetAllAlbumArtists();
            cmbAlbumArtist.DisplayMember = "Name";
            cmbAlbumArtist.ValueMember = "Name";

            cmbGenre.DataSource = Library.GetAllGenres();
            cmbGenre.DisplayMember = "Name";
            cmbGenre.ValueMember = "Name";

            cmbAlbum.DataSource = Library.GetAllAlbums();
            cmbAlbum.DisplayMember = "Name";
            cmbAlbum.ValueMember = "Name";

            var trackNumbers = new List<string>();
            trackNumbers.Add("");
            for (var i = 0; i <= 80; i++) trackNumbers.Add(i.ToString());
            cmbTrackNumber.DataSource = trackNumbers;

            txtTitle.Text = Track.Title;
            cmbAlbum.Text = Track.Album;
            cmbAlbumArtist.Text = Track.AlbumArtist;
            cmbGenre.Text = Track.Genre;
            cmbArtist.Text = Track.Artist;
            cmbTrackNumber.Text = Track.TrackNumber.ToString();

            txtFile.Text = Track.Filename;

            Text = "Update Title";
        }

        /// <summary>
        /// Updates the data for the screen
        /// </summary>
        private void UpdateData()
        {
            if (!ValidateData()) return;

            Cursor = Cursors.Hand;
            Application.DoEvents();

            var saved = Library.UpdateTrackDetails(Track,
                cmbArtist.Text,
                txtTitle.Text,
                cmbAlbum.Text,
                cmbAlbumArtist.Text,
                cmbGenre.Text,
                ConversionHelper.ToInt(cmbTrackNumber.Text),
                chkUpdateAuxillaryFiles.Checked);

            if (!saved)
            {
                MessageBoxHelper.ShowError("Could not update file. The file may be in use by another process.");
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
            Close();
        }

        /// <summary>
        /// Validates the data.
        /// </summary>
        /// <returns>True if valid</returns>
        private bool ValidateData()
        {
            return txtTitle.IsValid()
                && cmbAlbum.IsValid()
                && cmbAlbumArtist.IsValid()
                && cmbArtist.IsValid()
                && cmbGenre.IsValid()
                && cmbTrackNumber.IsValid();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        public Library Library { get; set; }

        /// <summary>
        /// Gets or sets the track.
        /// </summary>
        public Track Track { get; set; }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the btnOK control.
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        #endregion
    }
}
