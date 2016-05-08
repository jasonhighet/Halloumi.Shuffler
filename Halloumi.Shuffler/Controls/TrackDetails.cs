using System.ComponentModel;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;
using AE = Halloumi.Shuffler.AudioEngine;

namespace Halloumi.Shuffler.Controls
{
    public partial class TrackDetails : UserControl
    {
        public TrackDetails()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the current track description.
        /// </summary>
        private string _currentFilename;

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Library Library { get; set; }

        /// <summary>
        /// Displays the current track details.
        /// </summary>
        public void DisplayTrackDetails(Track track)
        {
            if (track != null && track.Filename == _currentFilename) return;

            if (track != null)
            {
                lblArtist.Text = track.Artist.Replace("&", "&&");
                lblTime.Text = track.LengthFormatted;
                lblAlbum.Text = track.Album.Replace("&", "&&");
                lblGenre.Text = track.Genre.Replace("&", "&&");
                lblTrack.Text = track.Title.Replace("&", "&&");
                lblBpm.Text = track.Bpm.ToString("0.00") + " BPM";
                lblKey.Text= KeyHelper.GetDisplayKey(track.Key);

                picCover.Image = Library.GetAlbumCover(new Album(track.Album));

                _currentFilename = track.Filename;
            }
            else
            {
                lblArtist.Text = "";
                lblTime.Text = "";
                lblAlbum.Text = "";
                lblGenre.Text = "";
                lblTrack.Text = "";
                lblBpm.Text = "";
                lblKey.Text = "";

                picCover.Image = null;

                _currentFilename = "";
            }
        }
    }
}