using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Halloumi.BassEngine.Helpers;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.Engine;
using Halloumi.Shuffler.Engine.Models;
using Halloumi.Shuffler.Forms;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Controls
{
    /// <summary>
    ///
    /// </summary>
    public partial class TrackLibraryControl : UserControl
    {
        #region Private Variables

        private bool _binding = false;

        private string SearchFilter { get; set; }

        public string PlaylistFilter { get; internal set; }

        public string ExcludedPlaylistFilter { get; internal set; }

        private Library.ShufflerFilter ShufflerFilter { get; set; }

        private Library.TrackRankFilter TrackRankFilter { get; set; }

        private int MinBpm { get; set; }

        private int MaxBpm { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SamplerControl SamplerControl { get; set; }

        #endregion

        public EventHandler SelectedTracksChanged;
        public EventHandler DisplayedTracksChanging;

        private class TrackModel
        {
            public string Filename { get; set; }

            public string Description { get; set; }

            public string Album { get; set; }

            public string Genre { get; set; }

            public string LengthFormatted { get; set; }

            public decimal Length { get; set; }

            public decimal StartBpm { get; set; }

            public decimal EndBpm { get; set; }

            public decimal Bpm { get; set; }

            public int InCount { get; set; }

            public int OutCount { get; set; }

            public int UnrankedCount { get; set; }

            public string TrackNumberFormatted { get; set; }

            public Track Track { get; set; }

            public string RankDescription { get; set; }

            public string Key { get; set; }

            public TrackModel(Track track)
            {
                Filename = track.Filename;
                Description = track.Description;
                Album = track.Album;
                Genre = track.Genre;
                LengthFormatted = track.LengthFormatted;
                Length = track.Length;
                Bpm = track.Bpm;
                EndBpm = track.EndBpm;
                StartBpm = track.StartBpm;
                TrackNumberFormatted = track.TrackNumberFormatted;
                RankDescription = track.RankDescription;
                Track = track;
                Key = track.Key;
            }
        }

        #region Properties

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Library Library { get; set; }

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SampleLibrary SampleLibrary { get; set; }

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MixLibrary MixLibrary { get; set; }

        /// <summary>
        /// Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BE.BassPlayer BassPlayer { get; set; }

        /// <summary>
        /// Gets the playlist control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PlaylistControl PlaylistControl { get; set; }

        /// <summary>
        /// Gets or sets the tool strip label.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ToolStripLabel ToolStripLabel { get; set; }

        public bool ShowMixableTracks
        {
            set
            {
                splLibraryMixable.Panel2Collapsed = !value;
                ShowCurrentTrackDetails();
            }
        }

        #endregion

        #region Constructors

        public TrackLibraryControl()
        {
            InitializeComponent();

            MinBpm = 0;
            MaxBpm = 1000;
            grdTracks.VirtualMode = true;

            txtMinBPM.TextChanged += new EventHandler(txtMinBPM_TextChanged);
            txtMaxBPM.TextChanged += new EventHandler(txtMaxBPM_TextChanged);

            grdArtist.RowEnter += new DataGridViewCellEventHandler(grdArtist_RowEnter);
            grdGenre.RowEnter += new DataGridViewCellEventHandler(grdGenre_RowEnter);
            grdTracks.CellDoubleClick += new DataGridViewCellEventHandler(grdTracks_CellDoubleClick);
            grdTracks.SelectionChanged += new EventHandler(grdTracks_SelectionChanged);
            grdTracks.CellFormatting += new DataGridViewCellFormattingEventHandler(grdTracks_CellFormatting);

            lstAlbum.SelectedIndexChanged += new EventHandler(lstAlbum_SelectedIndexChanged);
            lstAlbum.MouseClick += new MouseEventHandler(lstAlbum_MouseClick);
            mnuOpenFileLocation.Click += new EventHandler(mnuOpenFileLocation_Click);
            mnuRenameGenre.Click += new EventHandler(mnuRenameGenre_Click);
            mnuUpdateGenre.Click += new EventHandler(mnuUpdateGenre_Click);
            mnuRenameAlbum.Click += new EventHandler(mnuRenameAlbum_Click);
            mnuUpdateAlbum.Click += new EventHandler(mnuUpdateAlbum_Click);
            mnuRenameArtist.Click += new EventHandler(mnuRenameArtist_Click);
            mnuUpdateArtist.Click += new EventHandler(mnuUpdateArtist_Click);
            mnuUpdateAlbumArtist.Click += new EventHandler(mnuUpdateAlbumArtist_Click);
            mnuUpdateTrackDetails.Click += new EventHandler(mnuUpdateTrackDetails_Click);
            mnuUpdateShufflerDetails.Click += new EventHandler(mnuUpdateShufflerDetails_Click);
            mnuRemoveShufflerDetails.Click += new EventHandler(mnuRemoveShufflerDetails_Click);
            mnuQueue.Click += new EventHandler(mnuQueue_Click);
            mnuPlay.Click += new EventHandler(mnuPlay_Click);
            mnuTrack.Opening += new CancelEventHandler(mnuTrack_Opening);

            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

            lstAlbum.Scroll += new EventHandler(lstAlbum_Scroll);
            txtSearch.KeyPress += new KeyPressEventHandler(txtSearch_KeyPress);
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            cmbPlaylist.SelectedIndexChanged += new EventHandler(cmbPlaylist_SelectedIndexChanged);
            cmbExcludedPlaylist.SelectedIndexChanged += new EventHandler(cmbExcludedPlaylist_SelectedIndexChanged);
            cmbShufflerFilter.SelectedIndexChanged += new EventHandler(cmbShufflerFilter_SelectedIndexChanged);
            cmbQueued.SelectedIndexChanged += new EventHandler(cmbQueued_SelectedIndexChanged);
            cmbTrackRankFilter.SelectedIndexChanged += new EventHandler(cmbTrackRankFilter_SelectedIndexChanged);

            grdTracks.SortOrderChanged += new EventHandler(grdTracks_SortOrderChanged);
            grdTracks.CellValueNeeded += grdTracks_CellValueNeeded;
            grdTracks.SortColumnIndex = -1;

            lstAlbum.BackColor = grdArtist.StateNormal.Background.Color1;
            lstAlbum.ForeColor = grdArtist.ForeColor;

            PlaylistFilter = "";
            ExcludedPlaylistFilter = "";
            SearchFilter = "";
            ShufflerFilter = Library.ShufflerFilter.None;
            TrackRankFilter = Library.TrackRankFilter.None;

            grdTracks.DefaultCellStyle.Font = _font;

            SetShufflerFilter();
            SetTrackRankFilter();
        }

        public void Initalize()
        {
            trackDetails.Library = Library;

            mixableTracks.MixLibrary = MixLibrary;
            mixableTracks.PlaylistControl = PlaylistControl;
            mixableTracks.Initialize();

            trackDetails.DisplayTrackDetails(null);

            BindData();
        }

        #endregion

        #region Private Methods

        public List<Track> GetAvailableTracks()
        {
            return Library.GetTracks("", "", "", "", "", PlaylistFilter, ShufflerFilter, MinBpm, MaxBpm, TrackRankFilter, ExcludedPlaylistFilter);
        }

        public List<Track> GetDisplayedTracks()
        {
            var tracks = Library.GetTracks(GetSelectedGenres(),
                GetSelectedArtists(),
                GetSelectedAlbums(),
                SearchFilter,
                PlaylistFilter,
                ShufflerFilter,
                MinBpm,
                MaxBpm,
                TrackRankFilter,
                ExcludedPlaylistFilter);

            if (PlaylistControl != null && cmbQueued.Text != "")
            {
                var queuedTracks = PlaylistControl.GetTracks();
                if (cmbQueued.Text == "Yes")
                {
                    tracks = tracks.Where(t => queuedTracks.Contains(t)).ToList();
                }
                else if (cmbQueued.Text == "No")
                {
                    tracks = tracks.Except(queuedTracks).ToList();
                }
            }
            return tracks;
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            BindData(true, true, true, true);
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="bindGenres">If set to true, binds the genres.</param>
        /// <param name="bindArtists">If set to true, binds the artists.</param>
        /// <param name="bindAlbums">If set to true, binds the albums.</param>
        /// <param name="bindTracks">If set to true, binds the tracks.</param>
        private void BindData(bool bindGenres, bool bindArtists, bool bindAlbums, bool bindTracks)
        {
            if (_binding) return;

            var selectedGenres = GetSelectedGenres();
            var selectedArtists = GetSelectedArtists();
            var selectedAlbums = GetSelectedAlbums();
            var selectedTracks = GetSelectedTracks();

            if (bindTracks) BindPlaylists();
            if (bindTracks) BindExcludedPlaylists();

            if (bindGenres) BindGenres(selectedGenres);
            if (bindArtists) BindArtists(selectedGenres, selectedArtists);
            if (bindAlbums) BindAlbums(selectedGenres, selectedArtists, selectedAlbums);
            if (bindTracks) BindTracks(selectedGenres, selectedArtists, selectedAlbums, selectedTracks);
        }

        /// <summary>
        /// Binds the playlists.
        /// </summary>
        private void BindPlaylists()
        {
            _binding = true;

            var selectedPlaylist = "";
            if (cmbPlaylist.SelectedItem != null) selectedPlaylist = cmbPlaylist.SelectedItem.ToString();

            cmbPlaylist.Items.Clear();
            cmbPlaylist.Items.Add("");
            foreach (var playlist in Library.GetAllPlaylists())
            {
                cmbPlaylist.Items.Add(playlist.Name);
            }

            var index = cmbPlaylist.FindString(selectedPlaylist);
            if (index != -1) cmbPlaylist.SelectedIndex = index;

            _binding = false;
        }

        /// <summary>
        /// Binds the excluded playlists.
        /// </summary>
        private void BindExcludedPlaylists()
        {
            _binding = true;

            var selectedExcludedPlaylist = "";
            if (cmbExcludedPlaylist.SelectedItem != null) selectedExcludedPlaylist = cmbExcludedPlaylist.SelectedItem.ToString();

            cmbExcludedPlaylist.Items.Clear();
            cmbExcludedPlaylist.Items.Add("");
            foreach (var excludedExcludedPlaylist in Library.GetAllPlaylists())
            {
                cmbExcludedPlaylist.Items.Add(excludedExcludedPlaylist.Name);
            }

            var index = cmbExcludedPlaylist.FindString(selectedExcludedPlaylist);
            if (index != -1) cmbExcludedPlaylist.SelectedIndex = index;

            _binding = false;
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="bindGenres">If set to true, binds the genres.</param>
        /// <param name="bindArtists">If set to true, binds the artists.</param>
        /// <param name="bindAlbums">If set to true, binds the albums.</param>
        /// <param name="bindTracks">If set to true, binds the tracks.</param>
        private delegate void BindDataHandler(bool bindGenres, bool bindArtists, bool bindAlbums, bool bindTracks);

        /// <summary>
        /// Binds the genres.
        /// </summary>
        /// <param name="selectedGenres">The selected genres.</param>
        private void BindGenres(List<string> selectedGenres)
        {
            _binding = true;

            var genres = Library.GetGenres(SearchFilter, PlaylistFilter, ShufflerFilter, MinBpm, MaxBpm, TrackRankFilter, ExcludedPlaylistFilter);
            genres.Insert(0, new Genre("(All)"));

            grdGenre.DataSource = genres;
            SetSelectedGenres(selectedGenres);

            _binding = false;
        }

        /// <summary>
        /// Binds the artists.
        /// </summary>
        /// <param name="selectedGenres">The selected genres.</param>
        /// <param name="selectedArtists">The selected artists.</param>
        private void BindArtists(List<string> selectedGenres, List<string> selectedArtists)
        {
            _binding = true;

            var artists = Library.GetAlbumArtists(selectedGenres, SearchFilter, PlaylistFilter, ShufflerFilter, MinBpm, MaxBpm, TrackRankFilter, ExcludedPlaylistFilter);
            artists.Insert(0, new Artist("(All)"));

            grdArtist.DataSource = artists;
            SetSelectedArtists(selectedArtists);

            _binding = false;
        }

        /// <summary>
        /// Binds the albums.
        /// </summary>
        /// <param name="selectedGenres">The selected genres.</param>
        /// <param name="selectedArtists">The selected artists.</param>
        /// <param name="selectedAlbums">The selected albums.</param>
        private void BindAlbums(List<string> selectedGenres, List<string> selectedArtists, List<string> selectedAlbums)
        {
            _binding = true;

            var albums = Library.GetAlbums(selectedGenres, selectedArtists, SearchFilter, PlaylistFilter, ShufflerFilter, MinBpm, MaxBpm, TrackRankFilter, ExcludedPlaylistFilter);

            var items = new List<ListViewItem>();
            foreach (var album in albums)
            {
                if (album.Name == "(All)" || album.Name == "(None)" || album.Name == "") continue;
                var item = new ListViewItem(album.Name);
                item.Tag = album;
                item.SubItems.Add(album.AlbumArtist);
                items.Add(item);

                Application.DoEvents();
            }

            lstAlbum.BeginUpdate();
            lstAlbum.Items.Clear();
            lstAlbum.Items.AddRange(items.ToArray());
            SetSelectedAlbums(selectedAlbums);
            lstAlbum.EndUpdate();

            LoadVisibleAlbumCovers();

            _binding = false;
        }

        /// <summary>
        /// Binds the tracks.
        /// </summary>
        /// <param name="selectedGenres">The selected genres.</param>
        /// <param name="selectedArtists">The selected artists.</param>
        /// <param name="selectedAlbums">The selected albums.</param>
        /// <param name="selectedTracks">The selected tracks.</param>
        private void BindTracks(List<string> selectedGenres,
            List<string> selectedArtists,
            List<string> selectedAlbums,
            List<Track> selectedTracks)
        {
            _binding = true;

            if (DisplayedTracksChanging != null) DisplayedTracksChanging(this, EventArgs.Empty);

            var descriptions = new List<string>();

            grdTracks.SaveSelectedRows();

            MixLibrary.AvailableTracks = GetAvailableTracks();

            var trackModels = GetDisplayedTracks()
                .Take(2000)
                .Select(t => new TrackModel(t))
                .ToList();

            if (ShufflerFilter == Library.ShufflerFilter.ShuflerTracks)
            {
                foreach (var trackModel in trackModels)
                {
                    trackModel.InCount = MixLibrary.GetKeyMixedInCount(trackModel.Track);
                    trackModel.OutCount = MixLibrary.GetKeyMixedOutCount(trackModel.Track);
                }
            }

            if (grdTracks.SortedColumn != null)
            {
                var sortField = grdTracks.SortedColumn.DataPropertyName;
                if (sortField == "Description") trackModels = trackModels.OrderBy(t => t.Description).ToList();
                if (sortField == "Album") trackModels = trackModels.OrderBy(t => t.Album).ToList();
                if (sortField == "LengthFormatted") trackModels = trackModels.OrderBy(t => t.Length).ToList();
                if (sortField == "Genre") trackModels = trackModels.OrderBy(t => t.Genre).ToList();
                if (sortField == "StartBPM") trackModels = trackModels.OrderBy(t => t.StartBpm).ToList();
                if (sortField == "EndBPM") trackModels = trackModels.OrderBy(t => t.EndBpm).ToList();
                if (sortField == "InCount") trackModels = trackModels.OrderByDescending(t => t.InCount).ThenByDescending(t => t.OutCount).ToList();
                if (sortField == "OutCount") trackModels = trackModels.OrderByDescending(t => t.OutCount).ThenByDescending(t => t.InCount).ToList();
                //if (sortField == "UnrankedCount") trackModels = trackModels.OrderByDescending(t => t.UnrankedCount).ThenByDescending(t => t.OutCount).ToList();
                if (sortField == "RankDescription") trackModels = trackModels.OrderByDescending(t => t.Track.Rank).ToList();
                if (sortField == "Key") trackModels = trackModels.OrderByDescending(t => t.Key).ToList();

                if (grdTracks.SortOrder == SortOrder.Descending) trackModels.Reverse();
            }
            TrackModels = trackModels;

            if (trackModels.Count != grdTracks.RowCount)
            {
                grdTracks.RowCount = 0;
                grdTracks.RowCount = trackModels.Count;
            }

            grdTracks.RestoreSelectedRows();
            grdTracks.InvalidateDisplayedRows();

            SetToolStripLabel();

            _binding = false;

            RaiseSelectedTracksChanged();
        }

        private List<TrackModel> TrackModels { get; set; }

        private void grdTracks_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex == -1) return;

            var trackModel = GetTrackModelByIndex(e.RowIndex);

            if (trackModel == null) e.Value = "";
            else if (e.ColumnIndex == 0) e.Value = trackModel.Description;
            else if (e.ColumnIndex == 1) e.Value = trackModel.Album;
            else if (e.ColumnIndex == 2) e.Value = trackModel.Genre;
            else if (e.ColumnIndex == 3) e.Value = trackModel.LengthFormatted;
            else if (e.ColumnIndex == 4) e.Value = trackModel.StartBpm;
            else if (e.ColumnIndex == 5) e.Value = trackModel.Bpm;
            else if (e.ColumnIndex == 6) e.Value = trackModel.TrackNumberFormatted;
            else if (e.ColumnIndex == 7) e.Value = trackModel.InCount;
            else if (e.ColumnIndex == 8) e.Value = trackModel.OutCount;
            else if (e.ColumnIndex == 9) e.Value = trackModel.UnrankedCount;
            else if (e.ColumnIndex == 10) e.Value = trackModel.RankDescription;
            else if (e.ColumnIndex == 11) e.Value = KeyHelper.GetDisplayKey(trackModel.Key);
        }

        /// <summary>
        /// Handles the CellFormatting event of the grdTracks control.
        /// </summary>
        private void grdTracks_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var trackModel = GetTrackModelByIndex(e.RowIndex);
            if (trackModel == null) return;
            if (e.CellStyle == null) return;

            if (ShufflerFilter == Library.ShufflerFilter.ShuflerTracks && (trackModel.InCount == 0 || trackModel.OutCount == 0))
            {
                if (!e.CellStyle.Font.Italic) e.CellStyle.Font = FontHelper.ItalicizeFont(_font);
            }
            else
            {
                if (e.CellStyle.Font.Italic) e.CellStyle.Font = _font;
            }
        }

        private Font _font = new Font("Segoe UI", 9, GraphicsUnit.Point);

        /// <summary>
        /// Sets the selected genres.
        /// </summary>
        /// <param name="selectedGenres">The selected genres.</param>
        private void SetSelectedGenres(List<string> selectedGenres)
        {
            // select selected artist
            DataGridViewRow firstRow = null;
            foreach (DataGridViewRow row in grdGenre.Rows)
            {
                if (selectedGenres.Contains(row.Cells[0].Value.ToString()))
                {
                    row.Selected = true;
                    if (firstRow == null) firstRow = row;
                }
                else
                {
                    row.Selected = false;
                }
            }
            if (firstRow != null) grdGenre.FirstDisplayedScrollingRowIndex = firstRow.Index;
            else if (grdGenre.Rows.Count > 0) grdGenre.Rows[0].Selected = true;
        }

        /// <summary>
        /// Sets the selected artists.
        /// </summary>
        /// <param name="selectedArtists">The selected artists.</param>
        private void SetSelectedArtists(List<string> selectedArtists)
        {
            // select selected artist
            DataGridViewRow firstRow = null;
            foreach (DataGridViewRow row in grdArtist.Rows)
            {
                if (selectedArtists.Contains(row.Cells[0].Value.ToString()))
                {
                    row.Selected = true;
                    if (firstRow == null) firstRow = row;
                }
                else
                {
                    row.Selected = false;
                }
            }
            if (firstRow != null) grdArtist.FirstDisplayedScrollingRowIndex = firstRow.Index;
            else if (grdArtist.Rows.Count > 0) grdArtist.Rows[0].Selected = true;
        }

        /// <summary>
        /// Sets the selected albums.
        /// </summary>
        /// <param name="selectedAlbums">The selected albums.</param>
        private void SetSelectedAlbums(List<string> selectedAlbums)
        {
            // select selected album
            ListViewItem firstItem = null;
            foreach (ListViewItem item in lstAlbum.Items)
            {
                if (selectedAlbums.Contains(item.Text))
                {
                    item.Selected = true;
                    if (firstItem == null) firstItem = item;
                }
            }
            if (firstItem != null) firstItem.EnsureVisible();
        }

        ///// <summary>
        ///// Sets the selected tracks.
        ///// </summary>
        ///// <param name="selectedTracks">The selected tracks.</param>
        //private void SetSelectedTracks(List<Track> selectedTracks)
        //{
        //    DataGridViewRow firstRow = null;
        //    foreach (DataGridViewRow row in grdTracks.Rows)
        //    {
        //        var trackModel = row.DataBoundItem as TrackModel;
        //        if (trackModel == null) continue;
        //        if (selectedTracks.Select(t => t.Filename).Distinct().ToList().Contains(trackModel.Filename))
        //        {
        //            row.Selected = true;
        //            if (firstRow == null) firstRow = row;
        //        }
        //        else
        //        {
        //            row.Selected = false;
        //        }
        //    }
        //    if (firstRow != null) grdTracks.FirstDisplayedScrollingRowIndex = firstRow.Index;
        //    else if (grdTracks.Rows.Count > 0) grdTracks.Rows[0].Selected = true;
        //}

        /// <summary>
        /// Gets the selected genre.
        /// </summary>
        /// <returns>The selected genre</returns>
        private string GetSelectedGenre()
        {
            if (grdGenre.SelectedRows.Count == 0) return "";
            return grdGenre.SelectedRows[0].Cells[0].Value.ToString();
        }

        /// <summary>
        /// Gets the selected genres.
        /// </summary>
        /// <returns>The selected genres.</returns>
        private List<string> GetSelectedGenres()
        {
            var genres = new List<string>();

            // select selected artist
            foreach (DataGridViewRow row in grdGenre.SelectedRows)
            {
                var genre = row.Cells[0].Value.ToString();
                genres.Add(genre);
                if (genre == "(All)") break;
            }

            return genres;
        }

        /// <summary>
        /// Gets the selected artist.
        /// </summary>
        /// <returns>The selected artist.</returns>
        private string GetSelectedArtist()
        {
            if (grdArtist.SelectedRows.Count == 0) return "";
            return grdArtist.SelectedRows[0].Cells[0].Value.ToString();
        }

        /// <summary>
        /// Gets the selected artists.
        /// </summary>
        /// <returns></returns>
        private List<string> GetSelectedArtists()
        {
            var artists = new List<string>();

            // select selected artist
            foreach (DataGridViewRow row in grdArtist.SelectedRows)
            {
                var artist = row.Cells[0].Value.ToString();
                artists.Add(artist);
                if (artist == "(All)") break;
            }

            return artists;
        }

        /// <summary>
        /// Gets the selected album.
        /// </summary>
        /// <returns>The selected album</returns>
        private string GetSelectedAlbum()
        {
            // select selected album
            foreach (ListViewItem item in lstAlbum.Items)
            {
                if (item.Selected == true) return item.Text;
            }
            return "";
        }

        /// <summary>
        /// Gets the selected albums.
        /// </summary>
        /// <returns>The selected albums</returns>
        private List<string> GetSelectedAlbums()
        {
            var albums = new List<string>();

            // select selected album
            foreach (ListViewItem item in lstAlbum.Items)
            {
                if (item.Selected == true) albums.Add(item.Text);
            }
            return albums;
        }

        /// <summary>
        /// Gets the selected track.
        /// </summary>
        /// <returns>The selected track</returns>
        public Track GetSelectedTrack()
        {
            var tracks = GetSelectedTracks();
            if (tracks.Count == 0) return null;
            return tracks[0];
        }

        /// <summary>
        /// Gets the selected tracks.
        /// </summary>
        /// <returns>The selected tracks</returns>
        private List<Track> GetSelectedTracks()
        {
            var tracks = new List<Track>();

            for (var i = 0; i < grdTracks.SelectedRows.Count; i++)
            {
                var track = GetTrackByIndex(grdTracks.SelectedRows[i].Index);
                if (track != null) tracks.Add(track);
            }

            //for (var i = 0; i < grdTracks.Rows.Count; i++)
            //{
            //    if (!grdTracks.Rows[i].Selected) continue;
            //    var track = GetTrackByIndex(i);
            //    if (track != null) tracks.Add(track);
            //}
            return tracks;
        }

        /// <summary>
        /// Gets the index of the track model by.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private TrackModel GetTrackModelByIndex(int index)
        {
            if (TrackModels == null) return null;
            if (index < 0 || index >= TrackModels.Count) return null;
            return TrackModels[index];
        }

        /// <summary>
        /// Gets a track by its index
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The track at the index</returns>
        private Track GetTrackByIndex(int index)
        {
            var trackModel = GetTrackModelByIndex(index);
            if (trackModel == null) return null;
            var track = Library.GetTrackByFilename(trackModel.Filename);
            if (track == null)
                track = Library.GetTracksByDescription(trackModel.Description).FirstOrDefault();

            return track;
        }

        /// <summary>
        /// Gets the selected playlist.
        /// </summary>
        /// <returns>The selected playlist, or null if there isn't one</returns>
        private Playlist GetSelectedPlaylist()
        {
            var selectedPlaylist = "";
            if (cmbPlaylist.SelectedItem != null) selectedPlaylist = cmbPlaylist.SelectedItem.ToString();
            if (selectedPlaylist == "") return null;

            return Library.GetAllPlaylists()
                .Where(p => p.Name == selectedPlaylist)
                .FirstOrDefault();
        }

        /// <summary>
        /// Loads the images for the albums currently visible in the album list.
        /// </summary>
        private void LoadVisibleAlbumCovers()
        {
            var firstTile = lstAlbum.GetFirstDisplayedTile();
            if (firstTile == null) return;

            var tileSize = lstAlbum.LargeImageList.ImageSize.Width + 50;
            var imageCount = ((lstAlbum.Width / tileSize) + 1) * ((lstAlbum.Height / tileSize) + 1) * 3;
            if (imageCount < 20) imageCount = 20;

            var startIndex = firstTile.Index;
            var endIndex = startIndex + imageCount;
            if (endIndex > lstAlbum.Items.Count) endIndex = lstAlbum.Items.Count;

            var itemCount = lstAlbum.Items.Count;
            for (var i = startIndex; i < endIndex; i++)
            {
                Application.DoEvents();

                if (itemCount != lstAlbum.Items.Count) break;

                var item = lstAlbum.Items[i];
                if (item.ImageKey != "") continue;

                var album = item.Tag as Album;
                if (album == null) continue;

                if (!imlAlbumArt.Images.ContainsKey(album.Name))
                {
                    var image = Library.GetAlbumCover(album);
                    if (image == null) continue;

                    using (var graphics = Graphics.FromImage(image))
                    {
                        graphics.DrawRectangle(Pens.Gray, 0, 0, image.Width - 1, image.Height - 1);
                    }

                    imlAlbumArt.Images.Add(album.Name, image);
                }
                item.ImageKey = album.Name;
            }
        }

        /// <summary>
        /// Loads the images for the albums currently visible in the album list.
        /// </summary>
        private delegate void LoadVisibleAlbumCoversHandler();

        /// <summary>
        /// Opens the file location of the selected track
        /// </summary>
        private void OpenFileLocation()
        {
            var track = GetSelectedTrack();
            if (track == null) return;
            Process.Start(Path.GetDirectoryName(track.Filename));
        }

        /// <summary>
        /// Renames the selected genre.
        /// </summary>
        private void RenameGenre()
        {
            var form = new FrmUpdateGenre();
            form.Library = Library;
            form.Genre = GetSelectedGenre();
            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                BindData();
            }
        }

        /// <summary>
        /// Updates the selected genre.
        /// </summary>
        private void UpdateGenre()
        {
            var tracks = GetSelectedTracks();
            if (tracks.Count == 0) return;

            var form = new FrmUpdateGenre();
            form.Library = Library;
            form.Tracks = tracks;
            var result = form.ShowDialog();

            if (result == DialogResult.OK)
            {
                BindData();
            }
        }

        /// <summary>
        /// Renames the selected album.
        /// </summary>
        private void RenameAlbum()
        {
            var form = new FrmUpdateAlbum();
            form.Library = Library;
            form.Album = GetSelectedAlbum();
            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                BindData();
            }
        }

        /// <summary>
        /// Updates the album of the selected tracks
        /// </summary>
        private void UpdateAlbum()
        {
            var tracks = GetSelectedTracks();
            if (tracks.Count == 0) return;

            var form = new FrmUpdateAlbum();
            form.Library = Library;
            form.Tracks = tracks;
            var result = form.ShowDialog();

            if (result == DialogResult.OK)
            {
                BindData();
            }
        }

        /// <summary>
        /// Updates the album artist of the selected tracks
        /// </summary>
        private void UpdateAlbumArtist()
        {
            var form = new FrmUpdateArtist();
            form.Library = Library;
            form.Album = GetSelectedAlbum();
            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                BindData();
            }
        }

        /// <summary>
        /// Updates the artist of the selected tracks
        /// </summary>
        private void UpdateArtist()
        {
            var tracks = GetSelectedTracks();
            if (tracks.Count == 0) return;

            var form = new FrmUpdateArtist();
            form.Library = Library;
            form.Tracks = tracks;
            var result = form.ShowDialog();

            if (result == DialogResult.OK)
            {
                BindData();
            }
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
                Library.ReloadTrack(GetSelectedTrack().Filename);
                BassPlayer.ReloadTrack(GetSelectedTrack().Filename);
                BindData();
            }
        }

        /// <summary>
        /// Edits the samples for the selected tracl
        /// </summary>
        private void EditSamples()
        {
            if (GetSelectedTrack() == null) return;

            var form = new FrmEditTrackSamples();
            form.BassPlayer = BassPlayer;
            form.Filename = GetSelectedTrack().Filename;
            form.SampleLibrary = SampleLibrary;
            form.Library = Library;

            var result = form.ShowDialog();
            //if (result == DialogResult.OK)
            //{
            //    this.Library.ReloadTrack(this.GetSelectedTrack().Filename);
            //    this.BassPlayer.ReloadTrack(this.GetSelectedTrack().Filename);
            //    BindData();
            //}
        }

        /// <summary>
        /// Shows the Rename Artist screen.
        /// </summary>
        private void RenameArtist()
        {
            var form = new FrmUpdateArtist();
            form.Library = Library;
            form.Artist = GetSelectedArtist();
            var result = form.ShowDialog();
            if (result == DialogResult.OK) BindData(false, true, true, true);
        }

        /// <summary>
        /// Sets the search filter and performs a search
        /// </summary>
        private void Search()
        {
            if (SearchFilter == txtSearch.Text.Trim()) return;
            if (txtSearch.Text.Trim().Length > 2 || txtSearch.Text.Trim().Length == 0)
            {
                SearchFilter = txtSearch.Text.Trim();
                var bindData = new BindDataHandler(BindData);
                txtSearch.BeginInvoke(bindData, true, true, true, true);
            }
        }

        /// <summary>
        /// Inports the tracks.
        /// </summary>
        /// <param name="path">The path.</param>
        private void InportTracks(string path)
        {
            if (Directory.Exists(path)) Library.ImportTracks(path);
            else if (File.Exists(path))
            {
                if (!path.ToLower().EndsWith(".mp3")) return;
                Library.ImportTrack(path);
            }

            var bindData = new BindDataHandler(BindData);
            BeginInvoke(bindData, true, true, true, true);
        }

        private void SetToolStripLabel()
        {
            if (ToolStripLabel == null) return;

            var text = string.Format("{0} library tracks.  {1} available tracks.  {2} displayed tracks.",
                Library.GetTracks().Count,
                GetAvailableTracks().Count,
                GetDisplayedTracks().Count);

            ToolStripLabel.Text = text;
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadUiSettings()
        {
            splLeftRight.SplitterDistance = (Width / 5) * 2;
            splLeftMiddle.SplitterDistance = (Width / 5) * 1;

            try
            {
                var settings = new Settings();
                var filename = Path.Combine(Path.GetTempPath(), "Halloumi.Shuffler.Library.xml");
                if (File.Exists(filename))
                {
                    settings = SerializationHelper<Settings>.FromXmlFile(filename);

                    txtSearch.Text = settings.TxtSearchText;
                    SearchFilter = settings.TxtSearchText;

                    txtMinBPM.Text = settings.TxtMinBpmText;
                    MinBpm = ConversionHelper.ToInt(settings.TxtMinBpmText, 0);

                    txtMaxBPM.Text = settings.TxtMaxBpmText;
                    MaxBpm = ConversionHelper.ToInt(settings.TxtMaxBpmText, 1000);

                    cmbQueued.SelectedIndex = settings.CmbQueuedSelectedIndex;
                    cmbShufflerFilter.SelectedIndex = settings.CmbShufflerFilterSelectedIndex;
                    cmbTrackRankFilter.SelectedIndex = settings.CmbTrackRankFilterSelectedIndex;

                    var playlistIndex = cmbPlaylist.FindString(settings.Playlist);
                    if (playlistIndex != -1)
                    {
                        cmbPlaylist.SelectedIndex = playlistIndex;
                        PlaylistFilter = settings.Playlist;
                    }

                    var excludedPlaylistIndex = cmbExcludedPlaylist.FindString(settings.ExcludedPlaylist);
                    if (excludedPlaylistIndex != -1)
                    {
                        cmbExcludedPlaylist.SelectedIndex = excludedPlaylistIndex;
                        ExcludedPlaylistFilter = settings.ExcludedPlaylist;
                    }

                    if (settings.SortColumnName != "")
                    {
                        for (var i = 0; i < grdTracks.Columns.Count; i++)
                        {
                            var column = grdTracks.Columns[i];
                            if (column.DataPropertyName == settings.SortColumnName)
                            {
                                grdTracks.SetSortColumn(column.Index, settings.SortOrder);
                                break;
                            }
                        }
                    }

                    var bindData = new BindDataHandler(BindData);
                    BeginInvoke(bindData, true, true, true, true);
                }
            }
            catch
            { }
        }

        public class Settings
        {
            public string TxtSearchText { get; set; }

            public string TxtMinBpmText { get; set; }

            public string TxtMaxBpmText { get; set; }

            public int CmbTrackRankFilterSelectedIndex { get; set; }

            public int CmbShufflerFilterSelectedIndex { get; set; }

            public int CmbQueuedSelectedIndex { get; set; }

            public string Playlist { get; set; }

            public string ExcludedPlaylist { get; set; }

            public string SortColumnName { get; set; }

            public SortOrder SortOrder { get; set; }

            public Settings()
            {
                TxtSearchText = "";
                TxtMinBpmText = "";
                TxtMaxBpmText = "";
                CmbTrackRankFilterSelectedIndex = 0;
                CmbShufflerFilterSelectedIndex = 0;
                CmbQueuedSelectedIndex = 0;
                Playlist = "";
                ExcludedPlaylist = "";
                SortColumnName = "";
                SortOrder = SortOrder.None;
            }
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveSettings()
        {
            var settings = new Settings();
            settings.TxtSearchText = txtSearch.Text;
            settings.TxtMinBpmText = txtMinBPM.Text;
            settings.TxtMaxBpmText = txtMaxBPM.Text;
            settings.CmbTrackRankFilterSelectedIndex = cmbTrackRankFilter.SelectedIndex;
            settings.CmbShufflerFilterSelectedIndex = cmbShufflerFilter.SelectedIndex;
            settings.CmbQueuedSelectedIndex = cmbQueued.SelectedIndex;

            settings.SortColumnName = grdTracks.SortedColumn != null ? grdTracks.SortedColumn.DataPropertyName : "";
            settings.SortOrder = grdTracks.SortOrder;

            settings.Playlist = "";
            if (cmbPlaylist.SelectedIndex > 0)
            {
                settings.Playlist = cmbPlaylist.SelectedItem.ToString();
            }

            settings.ExcludedPlaylist = "";
            if (cmbExcludedPlaylist.SelectedIndex > 0)
            {
                settings.ExcludedPlaylist = cmbExcludedPlaylist.SelectedItem.ToString();
            }

            var filename = Path.Combine(Path.GetTempPath(), "Halloumi.Shuffler.Library.xml");
            SerializationHelper<Settings>.ToXmlFile(settings, filename);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the RowEnter event of the grdGenre control.
        /// </summary>
        private void grdGenre_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            var bindData = new BindDataHandler(BindData);
            grdGenre.BeginInvoke(bindData, false, true, true, true);
        }

        /// <summary>
        /// Handles the RowEnter event of the grdArtist control.
        /// </summary>
        private void grdArtist_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            var bindData = new BindDataHandler(BindData);
            grdArtist.BeginInvoke(bindData, false, false, true, true);
        }

        /// <summary>
        /// Handles the MouseClick event of the lstAlbum control.
        /// </summary>
        private void lstAlbum_MouseClick(object sender, MouseEventArgs e)
        {
            var bindData = new BindDataHandler(BindData);
            lstAlbum.BeginInvoke(bindData, false, false, false, true);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstAlbum control.
        /// </summary>
        private void lstAlbum_SelectedIndexChanged(object sender, EventArgs e)
        {
            var bindData = new BindDataHandler(BindData);
            lstAlbum.BeginInvoke(bindData, false, false, false, true);
        }

        /// <summary>
        /// Handles the Click event of the mnuOpenFileLocation control.
        /// </summary>
        private void mnuOpenFileLocation_Click(object sender, EventArgs e)
        {
            OpenFileLocation();
        }

        /// <summary>
        /// Handles the DoWork event of the backgroundWorker control.
        /// </summary>
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Library.ImportTracks();
            Library.CleanLibrary();
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the backgroundWorker control.
        /// </summary>
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var bindData = new BindDataHandler(BindData);
            lstAlbum.BeginInvoke(bindData, true, true, true, true);
        }

        /// <summary>
        /// Handles the Click event of the mnuRenameGenre control.
        /// </summary>
        private void mnuRenameGenre_Click(object sender, EventArgs e)
        {
            RenameGenre();
        }

        /// <summary>
        /// Handles the Click event of the mnuUpdateGenre control.
        /// </summary>
        private void mnuUpdateGenre_Click(object sender, EventArgs e)
        {
            UpdateGenre();
        }

        /// <summary>
        /// Handles the Click event of the mnuRenameAlbum control.
        /// </summary>
        private void mnuRenameAlbum_Click(object sender, EventArgs e)
        {
            RenameAlbum();
        }

        /// <summary>
        /// Handles the Click event of the mnuUpdateAlbum control.
        /// </summary>
        private void mnuUpdateAlbum_Click(object sender, EventArgs e)
        {
            UpdateAlbum();
        }

        /// <summary>
        /// Handles the Click event of the mnuUpdateAlbumArtist control.
        /// </summary>
        private void mnuUpdateAlbumArtist_Click(object sender, EventArgs e)
        {
            UpdateAlbumArtist();
        }

        /// <summary>
        /// Handles the Click event of the mnuUpdateArtist control.
        /// </summary>
        private void mnuUpdateArtist_Click(object sender, EventArgs e)
        {
            UpdateArtist();
        }

        /// <summary>
        /// Handles the Click event of the mnuRenameArtist control.
        /// </summary>
        private void mnuRenameArtist_Click(object sender, EventArgs e)
        {
            RenameArtist();
        }

        /// <summary>
        /// Handles the Click event of the mnuUpdateTrackDetails control.
        /// </summary>
        public void mnuUpdateTrackDetails_Click(object sender, EventArgs e)
        {
            UpdateTrackDetails();
        }

        /// <summary>
        /// Handles the Click event of the mnuUpdateShufflerDetails control.
        /// </summary>
        private void mnuUpdateShufflerDetails_Click(object sender, EventArgs e)
        {
            UpdateShufflerDetails();
        }

        /// <summary>
        /// Handles the Click event of the mnuRemoveShufflerDetails control.
        /// </summary>
        private void mnuRemoveShufflerDetails_Click(object sender, EventArgs e)
        {
            var track = GetSelectedTrack();
            if (track == null) return;

            var message = string.Format("Are you sure you wish to remove the shuffler details for '{0}'?", track.Description);
            if (MessageBoxHelper.Confirm(message))
            {
                Library.RemoveShufflerDetails(track);
                BindData();
            }
        }

        /// <summary>
        /// Handles the Opening event of the mnuTrack control.
        /// </summary>
        private void mnuTrack_Opening(object sender, CancelEventArgs e)
        {
            mnuUpdateAlbum.Visible = (GetSelectedTracks().Count > 1);
            mnuUpdateArtist.Visible = (GetSelectedTracks().Count > 1);
            mnuUpdateGenre.Visible = (GetSelectedTracks().Count > 1);
            mnuUpdateTrackDetails.Visible = (GetSelectedTracks().Count == 1);
            mnuRemoveShufflerDetails.Visible = (GetSelectedTracks().Count == 1);
            mnuUpdateShufflerDetails.Visible = (GetSelectedTracks().Count == 1);
            mnuEditSamples.Visible = (GetSelectedTracks().Count == 1);
            //mnuAddToSampler.Visible = (this.GetSelectedTracks().Count == 1);
            mnuAddToSampler.Visible = false;

            BeginInvoke(new MethodInvoker(delegate()
            {
                BindRankMenu();
                BindRemoveTrackFromPlaylistMenu();
                BindAddTrackToPlaylistMenu();
            }));
        }

        /// <summary>
        /// Binds the rank menu.
        /// </summary>
        private void BindRankMenu()
        {
            var currentMixRank = -1;
            if (GetSelectedTracks().Count == 1)
            {
                currentMixRank = GetSelectedTrack().Rank;
            }
            for (var i = 0; i < 6; i++)
            {
                mnuRank.DropDownItems[i].Text = MixLibrary.GetRankDescription(5 - i);
                ((ToolStripMenuItem)mnuRank.DropDownItems[i]).Checked = ((5 - i) == currentMixRank);
            }
        }

        /// <summary>
        /// Binds the add track to playlist menu.
        /// </summary>
        private void BindAddTrackToPlaylistMenu()
        {
            var selectedTracks = GetSelectedTracks();

            Track selectedTrack = null;
            if (selectedTracks.Count == 1) selectedTrack = selectedTracks[0];

            var playlists = Library.GetAllPlaylists();

            // generate 'add to playlist' sub menu
            mnuAddTrackToPlaylist.DropDownItems.Clear();
            foreach (var playlist in playlists)
            {
                if (selectedTrack != null)
                {
                    if (!playlist.Tracks.Contains(selectedTrack))
                    {
                        mnuAddTrackToPlaylist.DropDownItems.Add(playlist.Name, null, mnuAddTrackToPlaylist_Click);
                    }
                }
                else
                {
                    mnuAddTrackToPlaylist.DropDownItems.Add(playlist.Name, null, mnuAddTrackToPlaylist_Click);
                }
                Application.DoEvents();
            }
            mnuAddTrackToPlaylist.DropDownItems.Add("(New Playlist)", null, mnuAddNewPlaylist_Click);
            mnuAddTrackToPlaylist.Visible = (mnuAddTrackToPlaylist.DropDownItems.Count > 0);
        }

        private void BindRemoveTrackFromPlaylistMenu()
        {
            // generate 'remove from playlist' sub menu
            mnuRemoveTrackFromPlaylist.DropDownItems.Clear();
            var selectedPlaylists = Library.GetPlaylistsForTracks(GetSelectedTracks());
            foreach (var playlist in selectedPlaylists)
            {
                mnuRemoveTrackFromPlaylist.DropDownItems.Add(playlist.Name, null, mnuRemoveTrackFromPlaylist_Click);
                Application.DoEvents();
            }
            mnuRemoveTrackFromPlaylist.Visible = (mnuRemoveTrackFromPlaylist.DropDownItems.Count > 0);
        }

        /// <summary>
        /// Handles the Click event of the lstAlbum control.
        /// </summary>
        private void lstAlbum_Click(object sender, EventArgs e)
        { }

        /// <summary>
        /// Handles the Scroll event of the lstAlbum control.
        /// </summary>
        private void lstAlbum_Scroll(object sender, EventArgs e)
        {
            var loadVisibleAlbumCovers = new LoadVisibleAlbumCoversHandler(LoadVisibleAlbumCovers);
            lstAlbum.BeginInvoke(loadVisibleAlbumCovers);
        }

        /// <summary>
        /// Handles the Click event of the mnuAddTrackToPlaylist control.
        /// </summary>
        private void mnuAddTrackToPlaylist_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            var playlist = Library.GetPlaylistByName(menu.Text);
            Library.AddTracksToPlaylist(playlist, GetSelectedTracks());

            if (GetSelectedPlaylist() != null && GetSelectedPlaylist().Name == playlist.Name)
            {
                var bindData = new BindDataHandler(BindData);
                BeginInvoke(bindData, true, true, true, true);
            }
        }

        /// <summary>
        /// Handles the Click event of the mnuAddNewPlaylist control.
        /// </summary>
        private void mnuAddNewPlaylist_Click(object sender, EventArgs e)
        {
            var tracks = GetSelectedTracks();
            var form = new FrmAddPlaylist();
            form.Library = Library;
            form.Tracks = tracks;
            var result = form.ShowDialog();
            if (result == DialogResult.OK) BindData();
        }

        /// <summary>
        /// Handles the Click event of the mnuRemoveTrackFromPlaylist control.
        /// </summary>
        private void mnuRemoveTrackFromPlaylist_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            var playlist = Library.GetPlaylistByName(menu.Text);
            Library.RemoveTracksFromPlaylist(playlist, GetSelectedTracks());

            if (GetSelectedPlaylist() != null && GetSelectedPlaylist().Name == playlist.Name)
            {
                var bindData = new BindDataHandler(BindData);
                BeginInvoke(bindData, true, true, true, true);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbPlaylist control.
        /// </summary>
        private void cmbPlaylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPlaylist.SelectedItem.ToString() == PlaylistFilter) return;
            PlaylistFilter = cmbPlaylist.SelectedItem.ToString();
            SearchFilter = "";
            var bindData = new BindDataHandler(BindData);
            BeginInvoke(bindData, true, true, true, true);
            txtSearch.Text = "";
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbExcludedPlaylist control.
        /// </summary>
        private void cmbExcludedPlaylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbExcludedPlaylist.SelectedItem.ToString() == ExcludedPlaylistFilter) return;
            ExcludedPlaylistFilter = cmbExcludedPlaylist.SelectedItem.ToString();
            SearchFilter = "";
            var bindData = new BindDataHandler(BindData);
            BeginInvoke(bindData, true, true, true, true);
            txtSearch.Text = "";
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbShufflerFilter control.
        /// </summary>
        private void cmbShufflerFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetShufflerFilter();
        }

        /// <summary>
        /// Sets the shuffler filter.
        /// </summary>
        private void SetShufflerFilter()
        {
            var shufflerFilter = Library.ShufflerFilter.None;
            var comboIndex = cmbShufflerFilter.SelectedIndex;
            if (comboIndex == 1) shufflerFilter = Library.ShufflerFilter.ShuflerTracks;
            else if (comboIndex == 2) shufflerFilter = Library.ShufflerFilter.NonShufflerTracks;

            colTrackAlbum.Visible = (shufflerFilter != Library.ShufflerFilter.ShuflerTracks);
            //colTrackAlbum.Visible = true;

            colTrackNumber.Visible = (shufflerFilter != Library.ShufflerFilter.ShuflerTracks);
            colInCount.Visible = (shufflerFilter == Library.ShufflerFilter.ShuflerTracks);
            colOutCount.Visible = (shufflerFilter == Library.ShufflerFilter.ShuflerTracks);
            colTrackKey.Visible = (shufflerFilter == Library.ShufflerFilter.ShuflerTracks);

            //colUnrankedCount.Visible = (shufflerFilter == Library.ShufflerFilter.ShuflerTracks);

            //colInCount.Visible = false;
            //colOutCount.Visible = false;
            colUnrankedCount.Visible = false;

            if (shufflerFilter == ShufflerFilter) return;
            ShufflerFilter = shufflerFilter;
            var bindData = new BindDataHandler(BindData);
            BeginInvoke(bindData, true, true, true, true);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbTrackRankFilter control.
        /// </summary>
        private void cmbTrackRankFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTrackRankFilter();
        }

        /// <summary>
        /// Sets the track rank filter.
        /// </summary>
        private void SetTrackRankFilter()
        {
            var trackRankFilter = Library.TrackRankFilter.None;
            var comboIndex = cmbTrackRankFilter.SelectedIndex;

            if (comboIndex == 1) trackRankFilter = Library.TrackRankFilter.GoodPlus;
            else if (comboIndex == 2) trackRankFilter = Library.TrackRankFilter.BearablePlus;
            else if (comboIndex == 3) trackRankFilter = Library.TrackRankFilter.Unranked;
            else if (comboIndex == 4) trackRankFilter = Library.TrackRankFilter.Forbidden;

            if (trackRankFilter == TrackRankFilter) return;
            TrackRankFilter = trackRankFilter;
            var bindData = new BindDataHandler(BindData);
            BeginInvoke(bindData, true, true, true, true);
        }

        private void SetBpmFilter()
        {
            var min = 0;
            if (txtMinBPM.Text != "") min = Convert.ToInt32(txtMinBPM.Text);

            var max = 1000;
            if (txtMaxBPM.Text != "") max = Convert.ToInt32(txtMaxBPM.Text);

            if (min != MinBpm || max != MaxBpm)
            {
                MinBpm = min;
                MaxBpm = max;
                var bindData = new BindDataHandler(BindData);
                BeginInvoke(bindData, true, true, true, true);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbQueued control.
        /// </summary>
        private void cmbQueued_SelectedIndexChanged(object sender, EventArgs e)
        {
            var bindData = new BindDataHandler(BindData);
            BeginInvoke(bindData, true, true, true, true);
        }

        /// <summary>
        /// Handles the TextChanged event of the txtSearch control.
        /// </summary>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        /// <summary>
        /// Handles the KeyPress event of the txtSearch control.
        /// </summary>
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            Search();
        }

        /// <summary>
        /// Handles the Click event of the mnuPlay control.
        /// </summary>
        private void mnuPlay_Click(object sender, EventArgs e)
        {
            PlaylistControl.QueueTrack(GetSelectedTrack());
        }

        /// <summary>
        /// Handles the CellDoubleClick event of the grdTracks control.
        /// </summary>
        private void grdTracks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            var track = GetTrackByIndex(e.RowIndex);
            if (track == null) return;

            PlaylistControl.QueueTrack(track);
        }

        /// <summary>
        /// Handles the Click event of the mnuQueue control.
        /// </summary>
        private void mnuQueue_Click(object sender, EventArgs e)
        {
            PlaylistControl.QueueTracks(GetSelectedTracks());
        }

        /// <summary>
        /// Handles the SortOrderChanged event of the grdTracks control.
        /// </summary>
        private void grdTracks_SortOrderChanged(object sender, EventArgs e)
        {
            var bindData = new BindDataHandler(BindData);
            BeginInvoke(bindData, false, false, false, true);
        }

        /// <summary>
        /// Handles the SelectionChanged event of the grdTracks control.
        /// </summary>
        private void grdTracks_SelectionChanged(object sender, EventArgs e)
        {
            ShowCurrentTrackDetails();

            RaiseSelectedTracksChanged();
        }

        private void ShowCurrentTrackDetails()
        {
            var track = GetSelectedTrack();
            trackDetails.DisplayTrackDetails(track);

            if (!splLibraryMixable.Panel2Collapsed)
                mixableTracks.DisplayMixableTracks(track);
        }

        private void RaiseSelectedTracksChanged()
        {
            if (SelectedTracksChanged != null) SelectedTracksChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the TextChanged event of the txtMaxBPM control.
        /// </summary>
        private void txtMaxBPM_TextChanged(object sender, EventArgs e)
        {
            SetBpmFilter();
        }

        /// <summary>
        /// Handles the TextChanged event of the txtMinBPM control.
        /// </summary>
        private void txtMinBPM_TextChanged(object sender, EventArgs e)
        {
            SetBpmFilter();
        }

        /// <summary>
        /// Handles the Click event of the mnuAddToSampler control.
        /// </summary>
        private void mnuAddToSampler_Click(object sender, EventArgs e)
        {
            if (SamplerControl == null) return;
            SamplerControl.LoadAdditionalTrack(GetSelectedTrack());
        }

        #endregion

        /// <summary>
        /// Handles the Click event of the mnuRank control.
        /// </summary>
        private void mnuRank_Click(object sender, EventArgs e)
        {
            var mixRankDescription = (sender as ToolStripDropDownItem).Text;
            var mixRank = MixLibrary.GetRankFromDescription(mixRankDescription);

            foreach (var track in GetSelectedTracks())
            {
                track.Rank = mixRank;
                Library.SaveRank(track);
            }
            BindData();
        }

        /// <summary>
        /// Determines whether the library is currently updating.
        /// </summary>
        /// <returns>True if the library is updating; otherwise, false.</returns>
        public bool IsLibraryUpdating()
        {
            return backgroundWorker.IsBusy;
        }

        /// <summary>
        /// Cancels the library import.
        /// </summary>
        public void CancelLibraryImport()
        {
            Library.CancelImport();
        }

        /// <summary>
        /// Imports the library.
        /// </summary>
        public void ImportLibrary()
        {
            if (backgroundWorker.IsBusy) return;

            backgroundWorker.RunWorkerAsync();
        }

        private void mnuCalculateKey_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            foreach (var track in GetSelectedTracks())
            {
                if (track.Key != "") continue;
                try
                {
                    KeyHelper.CalculateKey(track.Filename);
                    Library.ImportTrack(track.Filename);
                }
                catch
                { }
                Application.DoEvents();
            }

            Cursor = Cursors.Default;
            Application.DoEvents();

            BindData(false, false, false, true);
        }

        private void mnuReloadMetadata_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            foreach (var track in GetSelectedTracks())
            {
                try
                {
                    Library.ReloadTrackMetaData(track.Filename);
                }
                catch
                { }
                Application.DoEvents();
            }

            Cursor = Cursors.Default;
            Application.DoEvents();

            BindData(false, false, false, true);
        }

        private void mnuEditSamples_Click(object sender, EventArgs e)
        {
            EditSamples();
        }
    }
}