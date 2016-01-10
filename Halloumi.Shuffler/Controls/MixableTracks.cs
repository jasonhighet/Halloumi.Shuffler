using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Shuffler.Engine;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Controls
{
    public partial class MixableTracks : UserControl
    {
        public EventHandler QueueTrack;

        #region Private Variables

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        public MixLibrary MixLibrary { get; set; }

        public PlaylistControl PlaylistControl { get; set; }

        private Track _parentTrack;

        public enum View
        {
            FromTracks,
            ToTracks
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MixableTracks class.
        /// </summary>
        public MixableTracks()
        {
            InitializeComponent();
            grdMixableTracks.SortOrderChanged += new EventHandler(grdMixableTracks_SortOrderChanged);
            grdMixableTracks.CellContentDoubleClick += new DataGridViewCellEventHandler(grdMixableTracks_CellContentDoubleClick);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            LoadSettings();
        }

        public void DisplayMixableTracks(Track parentTrack)
        {
            _parentTrack = parentTrack;

            // BindData();
            var bindData = new BindDataHandler(BindData);
            this.BeginInvoke(bindData);
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            _bindDataAllowed = false;

            var settings = Halloumi.Shuffler.Forms.Settings.Default;
            cmbRank.SelectedIndex = settings.MixableRankFilterIndex;
            cmbKeyRank.SelectedIndex = settings.MixableKeyRankFilterIndex;
            cmbView.SelectedIndex = settings.MixableViewIndex;
            chkExcludeQueued.Checked = settings.MixableTracksExcludeQueued;

            _bindDataAllowed = true;
        }

        /// <summary>
        /// Handles the CellContentDoubleClick event of the grdMixableTracks control.
        /// </summary>
        private void grdMixableTracks_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var track = GetSelectedTrack();
            if (track != null && this.PlaylistControl != null)
                this.PlaylistControl.QueueTrack(track);
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            if (!_bindDataAllowed) return;

            var view = cmbView.ParseEnum<View>();

            var rankFilter = cmbRank.GetTextThreadSafe();
            List<int> ranks = null;
            if (rankFilter == "Good+") ranks = new List<int> { 5, 4, 3 };
            else if (rankFilter == "Bearable+") ranks = new List<int> { 5, 4, 3, 2 };
            else if (rankFilter == "Unranked") ranks = new List<int> { 1 };
            else if (rankFilter == "Forbidden") ranks = new List<int> { 0 };
            else ranks = new List<int> { 5, 4, 3, 2, 1, 0 };

            var keyRankFilter = cmbKeyRank.GetTextThreadSafe();
            var minimumKeyRank = -1;
            if (keyRankFilter == "Very Good+") minimumKeyRank = 4;
            else if (keyRankFilter == "Good+") minimumKeyRank = 3;
            else if (keyRankFilter == "Bearable+") minimumKeyRank = 2;
            else if (keyRankFilter == "Not Good") minimumKeyRank = 0;

            var tracks = (view == View.FromTracks)
                ? this.MixLibrary.GetMixableFromTracks(_parentTrack, ranks)
                : this.MixLibrary.GetMixableToTracks(_parentTrack, ranks);

            var playListTracks = new List<Track>();
            if (this.PlaylistControl != null)
            {
                playListTracks = this.PlaylistControl.GetTracks();
            }

            if (minimumKeyRank == 0)
            {
                tracks = tracks
                    .Where(t => BE.KeyHelper.GetKeyMixRank(_parentTrack.Key, t.Key) <= 1)
                    .ToList();
            }
            else if (minimumKeyRank != -1)
            {
                tracks = tracks
                    .Where(t => BE.KeyHelper.GetKeyMixRank(_parentTrack.Key, t.Key) >= minimumKeyRank)
                    .ToList();
            }

            var mixableTracks = new List<MixableTrackModel>();
            foreach (var track in tracks)
            {
                if (mixableTracks.Exists(mt => mt.Description == track.Description)) continue;
                if (chkExcludeQueued.Checked && playListTracks.Exists(mt => mt.Description == track.Description)) continue;

                var mixableTrack = new MixableTrackModel
                {
                    Track = track,
                    Description = track.Description,
                    BPM = track.BPM,
                    Diff = BE.BassHelper.GetAbsoluteBPMPercentChange(_parentTrack.EndBPM, track.StartBPM),
                    MixRank = (view == View.FromTracks)
                        ? this.MixLibrary.GetExtendedMixLevel(track, _parentTrack)
                        : this.MixLibrary.GetExtendedMixLevel(_parentTrack, track),

                    Rank = track.Rank,
                    RankDescription = this.MixLibrary.GetRankDescription(track.Rank),
                    Key = BE.KeyHelper.GetDisplayKey(track.Key),
                    KeyDiff = BE.KeyHelper.GetKeyDifference(_parentTrack.Key, track.Key),
                    KeyRankDescription = BE.KeyHelper.GetKeyMixRankDescription(_parentTrack.Key, track.Key)
                };

                mixableTrack.MixRankDescription = this.MixLibrary.GetRankDescription(Convert.ToInt32(Math.Floor(mixableTrack.MixRank)));
                var hasExtendedMix = this.MixLibrary.HasExtendedMix(_parentTrack, track);
                if (hasExtendedMix) mixableTrack.MixRankDescription += "*";

                mixableTracks.Add(mixableTrack);
            }

            if (grdMixableTracks.SortedColumn != null)
            {
                var sortField = grdMixableTracks.SortedColumn.DataPropertyName;
                if (sortField == "Description") mixableTracks = mixableTracks.OrderBy(t => t.Description).ToList();
                if (sortField == "BPM") mixableTracks = mixableTracks.OrderBy(t => t.BPM).ToList();
                if (sortField == "Diff") mixableTracks = mixableTracks.OrderBy(t => t.Diff).ToList();
                if (sortField == "Mix") mixableTracks = mixableTracks.OrderBy(t => t.MixRank).ThenByDescending(t => t.Diff).ToList();
                if (sortField == "RankDescription") mixableTracks = mixableTracks.OrderBy(t => t.Rank).ThenByDescending(t => t.Diff).ToList();
                if (sortField == "Key") mixableTracks = mixableTracks.OrderBy(t => t.Key).ToList();
                if (sortField == "KeyRankDescription") mixableTracks = mixableTracks.OrderByDescending(t => t.KeyDiff).ToList();

                if (grdMixableTracks.SortOrder == SortOrder.Descending) mixableTracks.Reverse();
            }
            else
            {
                mixableTracks = mixableTracks
                    .OrderByDescending(t => t.MixRank)
                    .ThenBy(t => t.KeyDiff)
                    .ThenBy(t => t.Diff)
                    .ThenByDescending(t => t.Rank)
                    .ThenBy(t => t.Description)
                    .ToList();
            }

            grdMixableTracks.SaveSelectedRows();
            grdMixableTracks.DataSource = mixableTracks;
            grdMixableTracks.RestoreSelectedRows();

            lblCount.Text = string.Format("{0} tracks", mixableTracks.Count);
        }

        private bool _bindDataAllowed = true;

        /// <summary>
        /// Gets the selected track.
        /// </summary>
        /// <returns>The selected track</returns>
        public Track GetSelectedTrack()
        {
            if (grdMixableTracks.SelectedRows.Count == 0) return null;
            return (grdMixableTracks.SelectedRows[0].DataBoundItem as MixableTrackModel).Track;
        }

        /// <summary>
        /// Handles the SortOrderChanged event of the grdTracks control.
        /// </summary>
        private void grdMixableTracks_SortOrderChanged(object sender, EventArgs e)
        {
            var bindData = new BindDataHandler(BindData);
            this.BeginInvoke(bindData);
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private delegate void BindDataHandler();

        /// <summary>
        /// Represents a row in the grid
        /// </summary>
        private class MixableTrackModel
        {
            public string Description { get; set; }

            public decimal BPM { get; set; }

            public int Rank { get; set; }

            public string RankDescription { get; set; }

            public double MixRank { get; set; }

            public string MixRankDescription { get; set; }

            public decimal Diff { get; set; }

            public Track Track { get; set; }

            public string Key { get; set; }

            public string KeyRankDescription { get; set; }

            public int KeyDiff { get; set; }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbFilter control.
        /// </summary>
        private void cmbRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            Halloumi.Shuffler.Forms.Settings.Default.MixableRankFilterIndex = cmbRank.SelectedIndex;
            BindData();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkExcludeQueued control.
        /// </summary>
        private void chkExcludeQueued_CheckedChanged(object sender, EventArgs e)
        {
            Halloumi.Shuffler.Forms.Settings.Default.MixableTracksExcludeQueued = chkExcludeQueued.Checked;
            BindData();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbKeyRank control.
        /// </summary>
        private void cmbKeyRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            Halloumi.Shuffler.Forms.Settings.Default.MixableKeyRankFilterIndex = cmbKeyRank.SelectedIndex;
            BindData();
        }

        private void cmbView_SelectedIndexChanged(object sender, EventArgs e)
        {
            Halloumi.Shuffler.Forms.Settings.Default.MixableViewIndex = cmbView.SelectedIndex;
            BindData();
        }
    }
}