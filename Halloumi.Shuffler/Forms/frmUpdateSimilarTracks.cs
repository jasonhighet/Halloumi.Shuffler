using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmUpdateSimilarTracks : BaseForm
    {
        public FrmUpdateSimilarTracks()
        {
            InitializeComponent();
        }

        public BassPlayer BassPlayer { get; set; }

        public Library Library { get; set; }

        public List<Track> SimilarTracks { get; set; }

        /// <summary>
        ///     Updates the details of the selected track
        /// </summary>
        private void UpdateTrackDetails()
        {
            var selectedTrack = GetSelectedTrack();
            if (selectedTrack == null) return;

            var form = new FrmUpdateTrackDetails
            {
                Library = Library,
                Track = selectedTrack
            };
            var result = form.ShowDialog();
            if (result == DialogResult.OK) BindData();
        }

        /// <summary>
        ///     Updates the shuffler details.
        /// </summary>
        private void UpdateShufflerDetails()
        {
            var selectedTrack = GetSelectedTrack();
            if (selectedTrack == null) return;

            var form = new FrmShufflerDetails
            {
                BassPlayer = BassPlayer,
                Filename = selectedTrack.Filename
            };

            var result = form.ShowDialog();
            if (result != DialogResult.OK) return;
            Library.LoadTrack(selectedTrack.Filename);
            BassPlayer.ReloadTrack(selectedTrack.Filename);
            BindData();
        }

        /// <summary>
        ///     Gets the selected track.
        /// </summary>
        /// <returns>The selected track</returns>
        public Track GetSelectedTrack()
        {
            if (grdTracks.SelectedRows.Count == 0) return null;
            return grdTracks.SelectedRows[0].DataBoundItem as Track;
        }

        /// <summary>
        ///     Gets the selected tracks.
        /// </summary>
        /// <returns>The selected tracks</returns>
        private List<Track> GetSelectedTracks()
        {
            var tracks = new List<Track>();
            for (var i = 0; i < grdTracks.Rows.Count; i++)
            {
                var row = grdTracks.Rows[i];
                if (row.Selected) tracks.Add(row.DataBoundItem as Track);
            }
            return tracks;
        }

        private void frmSimilarTracks_Load(object sender, EventArgs e)
        {
            SimilarTracks = Library.GetDuplicateButDifferentShufflerTracks();
            BindData();
        }

        private void BindData()
        {
            grdTracks.DataSource = SimilarTracks;
        }

        private void mnuUpdateTrackDetails_Click(object sender, EventArgs e)
        {
            UpdateTrackDetails();
        }

        private void mnuUpdateShufflerDetails_Click(object sender, EventArgs e)
        {
            UpdateShufflerDetails();
        }

        private void mnuUpdateAudioData_Click(object sender, EventArgs e)
        {
            if (GetSelectedTracks().Count == 0) return;

            var updateTrackAudio = new FrmUpdateTrackAudio
            {
                Library = Library,
                DestinationTracks = GetSelectedTracks(),
                SourceTracks = GetSimilarTracks(GetSelectedTrack())
            };
            updateTrackAudio.ShowDialog();
            BindData();
        }

        /// <summary>
        ///     Handles the Click event of the mnuUpdateTrackTitle control.
        /// </summary>
        private void mnuUpdateTrackTitle_Click(object sender, EventArgs e)
        {
            if (GetSelectedTracks().Count == 0) return;

            var form = new FrmUpdateTrackTitle
            {
                Library = Library,
                Tracks = GetSelectedTracks()
            };
            form.ShowDialog();
            BindData();
        }

        /// <summary>
        ///     Gets similar tracks to a track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of similar tracks</returns>
        private List<Track> GetSimilarTracks(Track track)
        {
            return SimilarTracks.Where(t => t.Artist == track.Artist && StringHelper.FuzzyCompare(track.Title, t.Title))
                .ToList();
        }
    }
}