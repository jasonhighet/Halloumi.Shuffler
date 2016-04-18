using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;
using Halloumi.Shuffler.Forms;

namespace Halloumi.Shuffler.Controls
{
    public partial class MixableTracks : UserControl
    {
        public enum View
        {
            FromTracks,
            ToTracks
        }

        private TrackLibraryControl _libraryControl;
        private bool _bindDataAllowed = true;
        private MixLibrary _mixLibrary;

        private Track _parentTrack;
        public EventHandler QueueTrack;

        /// <summary>
        ///     Initializes a new instance of the MixableTracks class.
        /// </summary>
        public MixableTracks()
        {
            InitializeComponent();
            grdMixableTracks.SortOrderChanged += grdMixableTracks_SortOrderChanged;
            grdMixableTracks.CellContentDoubleClick += grdMixableTracks_CellContentDoubleClick;
        }


        public PlaylistControl PlaylistControl { get; set; }

        private List<Track> GetMixableFromTracks(Track track, List<int> mixLevels)
        {
            var mixableTracks = _mixLibrary
                .GetMixableFromTracks(track, mixLevels)
                .Select(x => x.Description)
                .ToList();

            return _libraryControl
                .GetAvailableTracks()
                .Where(x => mixableTracks.Contains(x.Description))
                .ToList();
        }

        private List<Track> GetMixableToTracks(Track track, List<int> mixLevels)
        {
            var mixableTracks = _mixLibrary
                .GetMixableToTracks(track, mixLevels)
                .Select(x => x.Description)
                .ToList();

            return _libraryControl
                .GetAvailableTracks()
                .Where(x => mixableTracks.Contains(x.Description))
                .ToList();
        }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public void Initialize(MixLibrary mixLibrary, TrackLibraryControl libraryControl)
        {
            _mixLibrary = mixLibrary;
            LoadSettings();
            _libraryControl = libraryControl;
        }

        public void DisplayMixableTracks(Track parentTrack)
        {
            _parentTrack = parentTrack;

            // BindData();
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(BindData));
            }
            else BindData();
        }


        /// <summary>
        ///     Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            _bindDataAllowed = false;

            var settings = Settings.Default;
            cmbRank.SelectedIndex = settings.MixableRankFilterIndex;
            cmbKeyRank.SelectedIndex = settings.MixableKeyRankFilterIndex;
            cmbView.SelectedIndex = settings.MixableViewIndex;
            chkExcludeQueued.Checked = settings.MixableTracksExcludeQueued;

            _bindDataAllowed = true;
        }

        /// <summary>
        ///     Handles the CellContentDoubleClick event of the grdMixableTracks control.
        /// </summary>
        private void grdMixableTracks_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var track = GetSelectedTrack();
            if (track != null)
                PlaylistControl?.QueueTrack(track);
        }

        /// <summary>
        ///     Binds the data.
        /// </summary>
        private void BindData()
        {
            if (!_bindDataAllowed) return;

            var view = cmbView.ParseEnum<View>();

            var rankFilter = cmbRank.GetTextThreadSafe();
            List<int> ranks;

            switch (rankFilter)
            {
                case "Good+":
                    ranks = new List<int> {5, 4, 3};
                    break;
                case "Bearable+":
                    ranks = new List<int> {5, 4, 3, 2};
                    break;
                case "Unranked":
                    ranks = new List<int> {1};
                    break;
                case "Forbidden":
                    ranks = new List<int> {0};
                    break;
                default:
                    ranks = new List<int> {5, 4, 3, 2, 1, 0};
                    break;
            }

            var keyRankFilter = cmbKeyRank.GetTextThreadSafe();
            int minimumKeyRank;

            switch (keyRankFilter)
            {
                case "Very Good+":
                    minimumKeyRank = 4;
                    break;
                case "Good+":
                    minimumKeyRank = 3;
                    break;
                case "Bearable+":
                    minimumKeyRank = 2;
                    break;
                case "Not Good":
                    minimumKeyRank = 0;
                    break;
                default:
                    minimumKeyRank = -1;
                    break;
            }

            var tracks = (view == View.FromTracks)
                ? GetMixableFromTracks(_parentTrack, ranks)
                : GetMixableToTracks(_parentTrack, ranks);

            var playListTracks = new List<Track>();
            if (PlaylistControl != null)
            {
                playListTracks = PlaylistControl.GetTracks();
            }

            if (minimumKeyRank == 0)
            {
                tracks = tracks
                    .Where(t => KeyHelper.GetKeyMixRank(_parentTrack.Key, t.Key) <= 1)
                    .ToList();
            }
            else if (minimumKeyRank != -1)
            {
                tracks = tracks
                    .Where(t => KeyHelper.GetKeyMixRank(_parentTrack.Key, t.Key) >= minimumKeyRank)
                    .ToList();
            }

            var mixableTracks = new List<MixableTrackModel>();
            foreach (var track in tracks)
            {
                if (mixableTracks.Exists(mt => mt.Description == track.Description)) continue;
                if (chkExcludeQueued.Checked && playListTracks.Exists(mt => mt.Description == track.Description))
                    continue;

                var mixableTrack = new MixableTrackModel
                {
                    Track = track,
                    Description = track.Description,
                    Bpm = track.Bpm,
                    Diff = BpmHelper.GetAbsoluteBpmPercentChange(_parentTrack.EndBpm, track.StartBpm),
                    MixRank = (view == View.FromTracks)
                        ? _mixLibrary.GetExtendedMixLevel(track, _parentTrack)
                        : _mixLibrary.GetExtendedMixLevel(_parentTrack, track),

                    MixRankDescription = (view == View.FromTracks)
                        ? _mixLibrary.GetExtendedMixDescription(track, _parentTrack)
                        : _mixLibrary.GetExtendedMixDescription(_parentTrack, track),

                    Rank = track.Rank,
                    RankDescription = track.RankDescription,
                    Key = KeyHelper.GetDisplayKey(track.Key),

                    KeyDiff = KeyHelper.GetKeyDifference(_parentTrack.Key, track.Key),
                    KeyRankDescription = KeyHelper.GetKeyMixRankDescription(track.Key, _parentTrack.Key)
                     
                };

                mixableTrack.MixRankDescription =
                    _mixLibrary.GetRankDescription(Convert.ToInt32(Math.Floor(mixableTrack.MixRank)));
                var hasExtendedMix = _mixLibrary.HasExtendedMix(_parentTrack, track);
                if (hasExtendedMix) mixableTrack.MixRankDescription += "*";

                mixableTracks.Add(mixableTrack);
            }

            if (grdMixableTracks.SortedColumn != null)
            {
                var sortField = grdMixableTracks.SortedColumn.DataPropertyName;
                if (sortField == "Description") mixableTracks = mixableTracks.OrderBy(t => t.Description).ToList();
                if (sortField == "BPM") mixableTracks = mixableTracks.OrderBy(t => t.Bpm).ToList();
                if (sortField == "Diff") mixableTracks = mixableTracks.OrderBy(t => t.Diff).ToList();
                if (sortField == "MixRankDescription")
                    mixableTracks = mixableTracks.OrderBy(t => t.MixRank).ThenByDescending(t => t.Diff).ToList();
                if (sortField == "RankDescription")
                    mixableTracks = mixableTracks.OrderBy(t => t.Rank).ThenByDescending(t => t.Diff).ToList();
                if (sortField == "Key") mixableTracks = mixableTracks.OrderBy(t => t.Key).ToList();
                if (sortField == "KeyRankDescription")
                    mixableTracks = mixableTracks.OrderByDescending(t => t.KeyDiff).ToList();

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

            lblCount.Text = $"{mixableTracks.Count} tracks";
        }

        /// <summary>
        ///     Gets the selected track.
        /// </summary>
        /// <returns>The selected track</returns>
        public Track GetSelectedTrack()
        {
            if (grdMixableTracks.SelectedRows.Count == 0) return null;

            var model = grdMixableTracks.SelectedRows[0].DataBoundItem as MixableTrackModel;

            return model?.Track;
        }

        /// <summary>
        ///     Handles the SortOrderChanged event of the grdTracks control.
        /// </summary>
        private void grdMixableTracks_SortOrderChanged(object sender, EventArgs e)
        {
            var bindData = new BindDataHandler(BindData);
            BeginInvoke(bindData);
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the cmbFilter control.
        /// </summary>
        private void cmbRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.MixableRankFilterIndex = cmbRank.SelectedIndex;
            BindData();
        }

        /// <summary>
        ///     Handles the CheckedChanged event of the chkExcludeQueued control.
        /// </summary>
        private void chkExcludeQueued_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.MixableTracksExcludeQueued = chkExcludeQueued.Checked;
            BindData();
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the cmbKeyRank control.
        /// </summary>
        private void cmbKeyRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.MixableKeyRankFilterIndex = cmbKeyRank.SelectedIndex;
            BindData();
        }

        private void cmbView_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.MixableViewIndex = cmbView.SelectedIndex;
            BindData();
        }

        /// <summary>
        ///     Binds the data.
        /// </summary>
        private delegate void BindDataHandler();

        /// <summary>
        ///     Represents a row in the grid
        /// </summary>
        private class MixableTrackModel
        {
            public string Description { get; set; }

            public decimal Bpm { get; set; }

            public int Rank { get; set; }

            public double MixRank { get; set; }

            public string MixRankDescription { get; set; }

            public string KeyRankDescription { get; set; }

            public string RankDescription { get; set; }

            public decimal Diff { get; set; }

            public Track Track { get; set; }

            public string Key { get; set; }

            public int KeyDiff { get; set; }
        }
    }
}