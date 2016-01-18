using System.ComponentModel;
using System.Windows.Forms;
using Halloumi.BassEngine.Helpers;
using Halloumi.Shuffler.Engine;
using Halloumi.Shuffler.Engine.Models;
using BE = Halloumi.BassEngine;

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
                lblCurrentTrackDescription.Text = track.Description.Replace("&", "&&");

                var details = track.Album + " - " + track.Genre + " ";
                details += " - " + track.LengthFormatted;
                if (track.Bpm != 0) details += " - " + track.Bpm.ToString("0.00") + " BPM";
                if (track.Key != "") details += " - " + KeyHelper.GetDisplayKey(track.Key);

                lblCurrentTrackDetails.Text = details;

                picCover.Image = Library.GetAlbumCover(new Album(track.Album));

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