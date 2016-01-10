using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.Engine;

namespace Halloumi.Shuffler.Forms
{
    public partial class frmUpdateTrackDetails : BaseForm
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the frmUpdateTrackDetails class.
        /// </summary>
        public frmUpdateTrackDetails()
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
            cmbArtist.DataSource = this.Library.GetAllArtists();
            cmbArtist.DisplayMember = "Name";
            cmbArtist.ValueMember = "Name";

            cmbAlbumArtist.DataSource = this.Library.GetAllAlbumArtists();
            cmbAlbumArtist.DisplayMember = "Name";
            cmbAlbumArtist.ValueMember = "Name";

            cmbGenre.DataSource = this.Library.GetAllGenres();
            cmbGenre.DisplayMember = "Name";
            cmbGenre.ValueMember = "Name";

            cmbAlbum.DataSource = this.Library.GetAllAlbums();
            cmbAlbum.DisplayMember = "Name";
            cmbAlbum.ValueMember = "Name";

            var trackNumbers = new List<string>();
            trackNumbers.Add("");
            for (int i = 0; i <= 80; i++) trackNumbers.Add(i.ToString());
            cmbTrackNumber.DataSource = trackNumbers;

            txtTitle.Text = this.Track.Title;
            cmbAlbum.Text = this.Track.Album;
            cmbAlbumArtist.Text = this.Track.AlbumArtist;
            cmbGenre.Text = this.Track.Genre;
            cmbArtist.Text = this.Track.Artist;
            cmbTrackNumber.Text = this.Track.TrackNumber.ToString();

            txtFile.Text = this.Track.Filename;

            this.Text = "Update Title";
        }

        /// <summary>
        /// Updates the data for the screen
        /// </summary>
        private void UpdateData()
        {
            if (!this.ValidateData()) return;

            this.Cursor = Cursors.Hand;
            Application.DoEvents();

            var saved = this.Library.UpdateTrackDetails(this.Track,
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
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
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
