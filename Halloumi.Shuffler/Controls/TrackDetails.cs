using System;
using System.ComponentModel;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.Controls
{
    public partial class TrackDetails : UserControl
    {
        private string _currentFilename;
        private Track _currentTrack;
        private ShufflerApplication _shufflerApplication;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ShufflerApplication ShufflerApplication
        {
            get => _shufflerApplication;
            set
            {
                if (_shufflerApplication != null)
                    _shufflerApplication.OnMixRankChanged -= Application_OnMixRankChanged;
                _shufflerApplication = value;
                if (_shufflerApplication != null)
                    _shufflerApplication.OnMixRankChanged += Application_OnMixRankChanged;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AlbumArtShown
        {
            get => picCover.Visible;
            set => picCover.Visible = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool MixableTracksShown
        {
            get => btnToggleMixable.Text == "\u25B2";
            set => btnToggleMixable.Text = value ? "\u25B2" : "\u25BC";
        }

        public event EventHandler MixableTracksToggleRequested;

        public TrackDetails()
        {
            InitializeComponent();
        }

        public void DisplayTrackDetails(Track track)
        {
            if (track != null && track.Filename == _currentFilename) return;

            _currentTrack = track;

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

                if (_shufflerApplication != null)
                    picCover.Image = _shufflerApplication.GetAlbumCover(track.Album);

                _currentFilename = track.Filename;
            }
            else
            {
                lblCurrentTrackDescription.Text = "";
                lblCurrentTrackDetails.Text = "";
                picCover.Image = null;
                _currentFilename = "";
            }

            RefreshMixCounts();
        }

        private void RefreshMixCounts()
        {
            if (_shufflerApplication == null || _currentTrack == null)
            {
                lblMixIn.Text = "In:  0E 0VG 0G 0TOT";
                lblMixOut.Text = "Out: 0E 0VG 0G 0TOT";
                return;
            }

            var inEx = _shufflerApplication.GetMixInCount(_currentTrack, 5);
            var inVg = _shufflerApplication.GetMixInCount(_currentTrack, 4) - inEx;
            var inGd = _shufflerApplication.GetMixInCount(_currentTrack, 3) - inEx - inVg;

            var outEx = _shufflerApplication.GetMixOutCount(_currentTrack, 5);
            var outVg = _shufflerApplication.GetMixOutCount(_currentTrack, 4) - outEx;
            var outGd = _shufflerApplication.GetMixOutCount(_currentTrack, 3) - outEx - outVg;

            lblMixIn.Text = $"In:  {inEx}E {inVg}VG {inGd}G {inEx + inVg + inGd}TOT";
            lblMixOut.Text = $"Out: {outEx}E {outVg}VG {outGd}G {outEx + outVg + outGd}TOT";
        }

        private void Application_OnMixRankChanged(object sender, MixRankChangedEventArgs e)
        {
            if (_currentTrack == null) return;
            if (e.FromTrack?.Description == _currentTrack.Description ||
                e.ToTrack?.Description == _currentTrack.Description)
                RefreshMixCounts();
        }

        private void btnToggleMixable_Click(object sender, EventArgs e)
        {
            MixableTracksToggleRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
