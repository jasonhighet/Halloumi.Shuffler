using System.ComponentModel;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;
using AE = Halloumi.Shuffler.AudioEngine;

namespace Halloumi.Shuffler.Controls
{
    public partial class TrackDetails : UserControl
    {
        /// <summary>
        ///     Gets or sets the current track description.
        /// </summary>
        private string _currentFilename;

        public ShufflerApplication ShufflerApplication { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AlbumArtShown
        {
            get => picCover.Visible;
            set => picCover.Visible = value;
        }

        public TrackDetails()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Displays the current track details.
        /// </summary>
        public void DisplayTrackDetails(Track track)
        {
            if (track != null && track.Filename == _currentFilename) return;

            if (track != null)
            {
                lblCurrentTrackDescription.Text = track.Description.Replace("&", "&&");

                var details = $"{track.Album} - {track.Genre} - {track.LengthFormatted}";

                if (track.Bpm != 0)
                    details += $" - {track.Bpm:0.00} BPM";

                if (track.Key != "")
                    details += $" - {KeyHelper.GetDisplayKey(track.Key)}";

                details += $" - {track.Bitrate:00} KPS";

                lblCurrentTrackDetails.Text = details;

                if (ShufflerApplication != null)
                    picCover.Image = ShufflerApplication.GetAlbumCover(track.Album);

                _currentFilename = track.Filename;
            }
            else
            {
                lblCurrentTrackDescription.Text = "";
                lblCurrentTrackDetails.Text = "";
                picCover.Image = null;

                _currentFilename = "";
            }
        }
    }
}