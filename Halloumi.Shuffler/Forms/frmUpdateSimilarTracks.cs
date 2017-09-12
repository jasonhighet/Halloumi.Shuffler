using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;
using AE = Halloumi.Shuffler.AudioEngine;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmUpdateSimilarTracks : BaseForm
    {
        public BassPlayer BassPlayer { get; set; }

        public Library Library { get; set; }

        public List<Track> Tracks { get; set; }

        public List<Track> AllTracks { get; set; }

        public List<Track> SimilarTracks { get; set; }

        public FrmUpdateSimilarTracks()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates the details of the selected track
        /// </summary>
        private void UpdateTrackDetails()
        {
            if (GetSelectedTrack() == null) return;
            var form = new FrmUpdateTrackDetails();
            form.Library = Library;
            form.Track = GetSelectedTrack();
            var result = form.ShowDialog();
            if (result == DialogResult.OK) BindData();
        }

        /// <summary>
        /// Updates the shuffler details.
        /// </summary>
        private void UpdateShufflerDetails()
        {
            if (GetSelectedTrack() == null) return;

            var form = new FrmShufflerDetails();
            form.BassPlayer = BassPlayer;
            form.Filename = GetSelectedTrack().Filename;

            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                Library.LoadTrack(GetSelectedTrack().Filename);
                BassPlayer.ReloadTrack(GetSelectedTrack().Filename);
                BindData();
            }
        }

        /// <summary>
        /// Gets the selected track.
        /// </summary>
        /// <returns>The selected track</returns>
        public Track GetSelectedTrack()
        {
            if (grdTracks.SelectedRows.Count == 0) return null;
            return grdTracks.SelectedRows[0].DataBoundItem as Track;
        }

        /// <summary>
        /// Gets the selected tracks.
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

        private class TrackDetail
        {
            public string Title { get; set; }

            public decimal Length { get; set; }

            public string Filename { get; set; }

            public bool IsShuffler { get; set; }

            public TrackDetail(Track track)
            {
                Title = track.Title;
                Length = track.FullLength;
                Filename = track.Filename;
                IsShuffler = track.IsShufflerTrack;
            }

            public override string ToString()
            {
                return Title + " " + Length.ToString();
            }
        }

        private void frmSimilarTracks_Load(object sender, EventArgs e)
        {
            SimilarTracks = GetSimilarTracks();
            BindData();
        }

        private void BindData()
        {
            grdTracks.DataSource = SimilarTracks;
        }

        private List<Track> GetSimilarTracks()
        {
            var similarTracks = new List<Track>();
            var allTracks = Library.GetTracks();

            var artists = Tracks
                .Select(t => t.Artist)
                .Distinct()
                .ToList();

            foreach (var artist in artists)
            {
                var trackTitles = Tracks
                    .Where(t => t.Artist == artist)
                    .Select(t => t.Title)
                    .Distinct()
                    .ToList();

                foreach (var trackTitle in trackTitles)
                {
                    var track = Tracks
                        .Where(t => t.Artist == artist && t.Title == trackTitle)
                        .OrderBy(t => t.Filename)
                        .FirstOrDefault();

                    if (track == null) continue;

                    var trackDuplicates = allTracks
                     .Where(t => t.Artist == track.Artist
                         && t.Title == track.Title
                         && t.Filename != track.Filename
                         && t.Length != track.Length)
                     .ToList();

                    if (trackDuplicates.Count > 0)
                    {
                        similarTracks.Add(track);
                        similarTracks.AddRange(trackDuplicates);
                    }
                }
            }

            similarTracks = similarTracks
                .Distinct()
                .OrderBy(t => t.Artist)
                .ThenBy(t => t.Title)
                .ThenBy(t => t.Filename)
                .ToList();

            return similarTracks;
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

            var updateTrackAudio = new FrmUpdateTrackAudio();
            updateTrackAudio.Library = Library;
            updateTrackAudio.DestinationTracks = GetSelectedTracks();
            updateTrackAudio.SourceTracks = GetSimilarTracks(GetSelectedTrack());
            updateTrackAudio.ShowDialog();
            BindData();
        }

        /// <summary>
        /// Handles the Click event of the mnuUpdateTrackTitle control.
        /// </summary>
        private void mnuUpdateTrackTitle_Click(object sender, EventArgs e)
        {
            if (GetSelectedTracks().Count == 0) return;

            var form = new FrmUpdateTrackTitle();
            form.Library = Library;
            form.Tracks = GetSelectedTracks();
            form.ShowDialog();
            BindData();
        }

        /// <summary>
        /// Gets similar tracks to a track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of similar tracks</returns>
        private List<Track> GetSimilarTracks(Track track)
        {
            return SimilarTracks.Where(t => t.Artist == track.Artist && StringHelper.FuzzyCompare(track.Title, t.Title)).ToList();
        }
    }
}