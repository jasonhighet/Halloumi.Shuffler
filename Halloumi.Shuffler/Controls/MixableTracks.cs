using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.Controls
{
    public partial class MixableTracks : UserControl
    {
        public enum View
        {
            FromTracks,
            ToTracks
        }

        private bool _bindDataAllowed = true;
        private TrackLibraryControl _libraryControl;
        private Track _parentTrack;

        /// <summary>
        ///     Initializes a new instance of the MixableTracks class.
        /// </summary>
        public MixableTracks()
        {
            InitializeComponent();
            grdMixableTracks.SortOrderChanged += grdMixableTracks_SortOrderChanged;
            grdMixableTracks.CellContentDoubleClick += grdMixableTracks_CellContentDoubleClick;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PlaylistControl PlaylistControl { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ShufflerApplication ShufflerApplication { get; set; }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public void Initialize(ShufflerApplication application, TrackLibraryControl libraryControl)
        {
            ShufflerApplication = application;
            _libraryControl = libraryControl;
            LoadSettings();
        }

        public void DisplayMixableTracks(Track parentTrack)
        {
            _parentTrack = parentTrack;

            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(BindData));
            else BindData();
        }

        /// <summary>
        ///     Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            _bindDataAllowed = false;

            var s = ShufflerApplication.LoadMixableTracksSettings();
            cmbRank.SelectedIndex    = s.RankFilterIndex;
            cmbKeyRank.SelectedIndex = s.KeyRankFilterIndex;
            cmbView.SelectedIndex    = s.ViewIndex;
            chkExcludeQueued.Checked = s.ExcludeQueued;

            _bindDataAllowed = true;
        }

        private void SaveCurrentSettings()
        {
            ShufflerApplication.SaveMixableTracksSettings(new MixableTracksDisplaySettings
            {
                RankFilterIndex    = cmbRank.SelectedIndex,
                KeyRankFilterIndex = cmbKeyRank.SelectedIndex,
                ViewIndex          = cmbView.SelectedIndex,
                ExcludeQueued      = chkExcludeQueued.Checked
            });
        }

        /// <summary>
        ///     Handles the CellContentDoubleClick event of the grdMixableTracks control.
        /// </summary>
        private void grdMixableTracks_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            QueueTrack();
        }

        private void QueueTrack()
        {
            var track = GetSelectedTrack();
            if (track != null)
                PlaylistControl?.QueueTrack(track);
        }

        private void InsertTrack()
        {
            var track = GetSelectedTrack();
            if (track == null) return;

            var view = cmbView.ParseEnum<View>();
            if (view == View.FromTracks)
                PlaylistControl?.InsertTrackBefore(track);
            else
                PlaylistControl?.InsertTrackAfter(track);
        }

        /// <summary>
        ///     Builds a <see cref="MixableTrackFilter"/> from the current UI state.
        /// </summary>
        private MixableTrackFilter BuildMixableTrackFilter()
        {
            // Map rank combo to list of allowed rank levels
            var rankText = cmbRank.GetTextThreadSafe();
            List<int> rankLevels;
            switch (rankText)
            {
                case "Good+":     rankLevels = new List<int> { 5, 4, 3 };          break;
                case "Bearable+": rankLevels = new List<int> { 5, 4, 3, 2 };       break;
                case "Unranked":  rankLevels = new List<int> { 1 };                 break;
                case "Forbidden": rankLevels = new List<int> { 0 };                 break;
                default:          rankLevels = new List<int> { 5, 4, 3, 2, 1, 0 }; break;
            }

            // Map key rank combo to minimum key rank threshold
            var keyRankText = cmbKeyRank.GetTextThreadSafe();
            int minKeyRank;
            switch (keyRankText)
            {
                case "Very Good+": minKeyRank = 4;  break;
                case "Good+":      minKeyRank = 3;  break;
                case "Bearable+":  minKeyRank = 2;  break;
                case "Not Good":   minKeyRank = 0;  break;
                default:           minKeyRank = -1; break;
            }

            var view = cmbView.ParseEnum<View>();

            return new MixableTrackFilter
            {
                MixRankLevels = rankLevels,
                MinKeyRank    = minKeyRank,
                Direction     = view == View.FromTracks
                                ? MixableTrackFilter.TrackDirection.From
                                : MixableTrackFilter.TrackDirection.To,
                ExcludeQueued = chkExcludeQueued.Checked
            };
        }

        /// <summary>
        ///     Binds the data.
        /// </summary>
        private void BindData()
        {
            if (!_bindDataAllowed) return;

            var filter = BuildMixableTrackFilter();
            var playlist = PlaylistControl?.GetTracks();
            var candidates = _libraryControl?.DisplayedTracks ?? new List<Track>();

            var results = ShufflerApplication != null && _parentTrack != null
                ? ShufflerApplication.GetMixableTracks(_parentTrack, filter, candidates, playlist)
                : new List<MixableTrackResult>();

            // Column-sort override — UI concern, stays here
            if (grdMixableTracks.SortedColumn != null)
            {
                var col = grdMixableTracks.SortedColumn.DataPropertyName;
                if (col == "Description")        results = results.OrderBy(t => t.Description).ToList();
                if (col == "Bpm")                results = results.OrderBy(t => t.Bpm).ToList();
                if (col == "Diff")               results = results.OrderBy(t => t.Diff).ToList();
                if (col == "MixRankDescription") results = results.OrderBy(t => t.MixRank).ThenByDescending(t => t.Diff).ToList();
                if (col == "RankDescription")    results = results.OrderBy(t => t.Rank).ThenByDescending(t => t.Diff).ToList();
                if (col == "Key")                results = results.OrderBy(t => t.Key).ToList();
                if (col == "KeyRankDescription") results = results.OrderByDescending(t => t.KeyDiff).ToList();

                if (grdMixableTracks.SortOrder == SortOrder.Descending) results.Reverse();
            }

            grdMixableTracks.SaveSelectedRows();
            grdMixableTracks.DataSource = results;
            grdMixableTracks.RestoreSelectedRows();

            lblCount.Text = $"{results.Count} tracks";
        }

        /// <summary>
        ///     Gets the selected track.
        /// </summary>
        public Track GetSelectedTrack()
        {
            if (grdMixableTracks.SelectedRows.Count == 0) return null;
            var result = grdMixableTracks.SelectedRows[0].DataBoundItem as MixableTrackResult;
            return result?.Track;
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
            SaveCurrentSettings();
            BindData();
        }

        /// <summary>
        ///     Handles the CheckedChanged event of the chkExcludeQueued control.
        /// </summary>
        private void chkExcludeQueued_CheckedChanged(object sender, EventArgs e)
        {
            SaveCurrentSettings();
            BindData();
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the cmbKeyRank control.
        /// </summary>
        private void cmbKeyRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveCurrentSettings();
            BindData();
        }

        private void cmbView_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveCurrentSettings();
            BindData();
        }

        private delegate void BindDataHandler();

        private void mnuQueueTrack_Click(object sender, EventArgs e)
        {
            QueueTrack();
        }

        private void mnuInsertTrack_Click(object sender, EventArgs e)
        {
            InsertTrack();
        }
    }
}