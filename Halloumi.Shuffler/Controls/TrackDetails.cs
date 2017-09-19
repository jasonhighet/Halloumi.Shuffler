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
        /// <summary>
        ///     Gets or sets the current track description.
        /// </summary>
        private string _currentFilename;

        public TrackDetails()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Library Library { get; set; }

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

                picCover.Image = Library.GetAlbumCover(track.Album);

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