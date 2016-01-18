using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Halloumi.BassEngine.Helpers;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Controls;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.Engine;
using Halloumi.Shuffler.Engine.Models;
using Halloumi.Shuffler.Forms;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Controls
{
    public partial class PlaylistControl : BaseUserControl
    {
        public EventHandler TrackClicked;
        public EventHandler PlaylistChanged;

        private class TrackModel
        {
            public string Description { get; set; }

            public string Filename { get; set; }

            public string LengthFormatted { get; set; }

            public decimal Bpm { get; set; }

            public decimal Length { get; set; }

            public bool IsCurrent { get; set; }

            public string MixRankDescription { get; set; }

            public int MixRank { get; set; }

            public string TrackRankDescription { get; set; }

            public string Key { get; set; }

            public string KeyRankDescription { get; set; }

            public int TrackRank { get; set; }

            public TrackModel(Track track, MixLibrary mixLibrary)
            {
                Description = track.Description;
                Filename = track.Filename;
                LengthFormatted = track.LengthFormatted;
                IsCurrent = false;
                Bpm = track.Bpm;
                Length = track.Length;
                TrackRank = track.Rank;
                TrackRankDescription = mixLibrary.GetRankDescription(track.Rank);
                Key = KeyHelper.GetDisplayKey(track.Key);
            }

            public static List<TrackModel> ToList(List<Track> tracks, MixLibrary mixLibrary)
            {
                return tracks.Select(t => new TrackModel(t, mixLibrary)).ToList();
            }
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PlaylistControl class.
        /// </summary>
        public PlaylistControl()
        {
            InitializeComponent();

            grdPlaylist.CellDoubleClick += new DataGridViewCellEventHandler(grdPlaylist_CellContentDoubleClick);
            grdPlaylist.CellFormatting += new DataGridViewCellFormattingEventHandler(grdPlaylist_CellFormatting);
            grdPlaylist.CellValueNeeded += new DataGridViewCellValueEventHandler(grdPlaylist_CellValueNeeded);
            grdPlaylist.SelectionChanged += new EventHandler(grdPlaylist_SelectionChanged);
            //grdPlaylist.CellClick += new DataGridViewCellEventHandler(grdPlaylist_CellClick);

            mnuOpenFileLocation.Click += new EventHandler(mnuOpenFileLocation_Click);
            mnuUpdateTrackDetails.Click += new EventHandler(mnuUpdateTrackDetails_Click);
            mnuUpdateShufflerDetails.Click += new EventHandler(mnuUpdateShufflerDetails_Click);
            mnuPlay.Click += new EventHandler(mnuPlay_Click);
            mnuTrack.Opening += new CancelEventHandler(mnuTrack_Opening);

            btnClear.Click += new EventHandler(btnClear_Click);
            btnOpen.Click += new EventHandler(btnOpen_Click);
            btnRemove.Click += new EventHandler(btnRemove_Click);
            btnSave.Click += new EventHandler(btnSave_Click);

            Load += new EventHandler(PlaylistControl_Load);
            TrackModels = new List<TrackModel>();
        }

        #endregion

        #region Public Methods

        public bool ShowMixableTracks
        {
            set
            {
                splTopBottom.Panel2Collapsed = !value;
                ShowCurrentTrackDetails();
            }
        }

        public void Initalize()
        {
            trackDetails.Library = Library;
            trackDetails.DisplayTrackDetails(null);

            mixableTracks.MixLibrary = MixLibrary;
            mixableTracks.PlaylistControl = this;
            mixableTracks.Initialize();

            if (BassPlayer != null)
            {
                BassPlayer.OnTrackChange += new EventHandler(BassPlayer_OnTrackChange);
                BassPlayer.OnSkipToEnd += new EventHandler(BassPlayer_OnFadeEnded);
                BassPlayer.OnEndFadeIn += new EventHandler(BassPlayer_OnFadeEnded);
            }
        }

        /// <summary>
        /// Handles the OnEndFadeIn event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnFadeEnded(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    PreloadTrack();
                }));
            }
            else PreloadTrack();
        }

        /// <summary>
        /// Handles the OnEndFadeIn event of the BassPlayer control.
        /// </summary>
        private void PreloadTrack()
        {
            var preloadTrack = GetTrackAfterNext();
            if (preloadTrack != null)
            {
                BassPlayer.PreloadTrack(preloadTrack.Filename);
            }
        }

        /// <summary>
        /// Queues tracks.
        /// </summary>
        /// <param name="queueTracks">The queue tracks.</param>
        public void QueueTracks(List<Track> queueTracks)
        {
            TrackModels.AddRange(TrackModel.ToList(queueTracks, MixLibrary));

            //Application.DoEvents();
            if (BassPlayer.CurrentTrack == null && TrackModels.Count > 0)
            {
                var track = GetTrackByIndex(0);
                BassPlayer.QueueTrack(track.Filename);
            }
            SetNextBassPlayerTrack();
            PreloadTrack();

            if (PlaylistChanged != null) PlaylistChanged(this, EventArgs.Empty);
            BindData();
        }

        private Track GetLibraryTrack(TrackModel trackModel)
        {
            var track = Library.GetTrackByFilename(trackModel.Filename);
            if (track == null || !File.Exists(track.Filename))
            {
                track = Library.GetTracksByDescription(trackModel.Description)
                    .Where(t => File.Exists(t.Filename))
                    .FirstOrDefault();
            }

            return track;
        }

        private Track GetLibraryTrack(string filename)
        {
            var track = Library.GetTrackByFilename(filename);
            if (track != null && !File.Exists(track.Filename))
            {
                track = Library.GetTracksByDescription(track.Description)
                    .Where(t => File.Exists(t.Filename))
                    .FirstOrDefault();
            }

            return track;
        }

        /// <summary>
        /// Queues a tracks
        /// </summary>
        /// <param name="queueTrack">The queue track.</param>
        public void QueueTrack(Track queueTrack)
        {
            var queueTracks = new List<Track>();
            queueTracks.Add(queueTrack);
            QueueTracks(queueTracks);
        }

        /// <summary>
        /// Opens the playlist.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        public void OpenPlaylist(string playlistName)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                CurrentPlaylistFile = playlistName;
                var playlist = Library.LoadPlaylist(playlistName);
                QueuePlaylist(playlist);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
            Cursor = Cursors.Default;
            Application.DoEvents();
        }

        /// <summary>
        /// Queues a playlist.
        /// </summary>
        /// <param name="playlist">The playlist.</param>
        public void QueuePlaylist(Playlist playlist)
        {
            try
            {
                TrackModels.Clear();
                QueueTracks(playlist.Tracks);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        /// <summary>
        /// Gets the next track.
        /// </summary>
        /// <returns>The next track.</returns>
        public Track GetCurrentTrack()
        {
            return GetTrackByIndex(GetCurrentTrackIndex());
        }

        /// <summary>
        /// Gets a track by its index
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The track at the index</returns>
        private Track GetTrackByIndex(int index)
        {
            if (index >= 0 && index < TrackModels.Count)
                return GetLibraryTrack(TrackModels[index]);
            else
                return null;
        }

        /// <summary>
        /// Gets the next track.
        /// </summary>
        /// <returns>The next track.</returns>
        public Track GetNextTrack()
        {
            return GetTrackByIndex(GetCurrentTrackIndex() + 1);
        }

        /// <summary>
        /// Gets the next track.
        /// </summary>
        /// <returns>The next track.</returns>
        private Track GetTrackAfterNext()
        {
            return GetTrackByIndex(GetCurrentTrackIndex() + 2);
        }

        /// <summary>
        /// Gets the previous track.
        /// </summary>
        /// <returns>The previous track.</returns>
        public Track GetPreviousTrack()
        {
            return GetTrackByIndex(GetCurrentTrackIndex() - 1);
        }

        /// <summary>
        /// Gets the selected track from the grid.
        /// </summary>
        /// <returns>The selected track</returns>
        public Track GetSelectedTrack()
        {
            var tracks = GetSelectedTracks();
            if (tracks.Count == 0) return null;
            return GetLibraryTrack(tracks[0]);
        }

        /// <summary>
        /// Gets the last track.
        /// </summary>
        /// <returns>The last track</returns>
        public Track GetLastTrack()
        {
            if (TrackModels.Count == 0) return null;
            return GetLibraryTrack(TrackModels.Last());
        }

        /// <summary>
        /// Gets the last track.
        /// </summary>
        /// <returns>The last track</returns>
        public Track GetSecondToLastTrack()
        {
            if (TrackModels.Count < 2) return null;
            return GetLibraryTrack(TrackModels[TrackModels.Count - 2]);
        }

        /// <summary>
        /// Removes the last track.
        /// </summary>
        public void RemoveLastTrack()
        {
            if (TrackModels.Count == 0) return;
            var lastTrack = TrackModels.Last();
            if (lastTrack != null) RemoveTrack(lastTrack);
        }

        /// <summary>
        /// Gets the tracks in the playlist
        /// </summary>
        /// <returns>The tracks in the playlist</returns>
        public List<Track> GetTracks()
        {
            return TrackModels.Select(t => GetLibraryTrack(t)).ToList();
        }

        /// <summary>
        /// Plays the next track.
        /// </summary>
        public void PlayNextTrack()
        {
            var track = GetNextTrack();
            if (track != null)
            {
                var currentIndex = GetCurrentTrackIndex();
                if (currentIndex != -1)
                    TrackModels[currentIndex].IsCurrent = false;
                TrackModels[currentIndex + 1].IsCurrent = true;
                BassPlayer.ForcePlay(track.Filename);
            }
        }

        /// <summary>
        /// Plays the previous track.
        /// </summary>
        public void PlayPreviousTrack()
        {
            var track = GetPreviousTrack();
            if (track != null)
            {
                var currentIndex = GetCurrentTrackIndex();
                TrackModels[currentIndex].IsCurrent = false;
                TrackModels[currentIndex - 1].IsCurrent = true;
                BassPlayer.ForcePlay(track.Filename);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Binds the data for the user control to the controls
        /// </summary>
        private void BindData()
        {
            if (_doNotBind) return;

            SetMixAndKeyRanks();

            grdPlaylist.SaveSelectedRows();

            var trackCount = TrackModels.Count;

            if (grdPlaylist.RowCount != trackCount)
                grdPlaylist.RowCount = trackCount;

            grdPlaylist.RestoreSelectedRows();

            grdPlaylist.InvalidateDisplayedRows();

            SetToolStripLabel();
            lblCount.Text = string.Format("{0} tracks", trackCount);
        }

        /// <summary>
        /// Queues the next track.
        /// </summary>
        private void SetNextBassPlayerTrack()
        {
            //Application.DoEvents();
            var nextTrack = GetNextTrack();

            if (nextTrack != null)
            {
                if (BassPlayer.NextTrack == null
                    ||
                    (BassPlayer.NextTrack != null && BassPlayer.NextTrack.Description != nextTrack.Description))
                {
                    BassPlayer.QueueTrack(nextTrack.Filename);
                }
            }
        }

        /// <summary>
        /// Sets the previous track in the bass player
        /// </summary>
        private void SetPreviousBassPlayerTrack()
        {
            //Application.DoEvents();
            var previousTrack = GetPreviousTrack();
            if (previousTrack != null)
                BassPlayer.SetPreviousTrack(previousTrack.Filename);
        }

        /// <summary>
        /// Gets the selected tracks from the grid.
        /// </summary>
        /// <returns>The selected tracks</returns>
        private List<TrackModel> GetSelectedTracks()
        {
            var tracks = new List<TrackModel>();
            for (var i = 0; i < grdPlaylist.Rows.Count; i++)
            {
                var row = grdPlaylist.Rows[i];
                if (row.Selected && row.Index < TrackModels.Count && row.Index >= 0)
                    tracks.Add(TrackModels[row.Index]);
            }
            return tracks;
        }

        /// <summary>
        /// Gets the selected library tracks.
        /// </summary>
        /// <returns>The selected library tracks</returns>
        private List<Track> GetSelectedLibraryTracks()
        {
            return GetSelectedTracks().Select(t => GetLibraryTrack(t)).ToList();
        }

        /// <summary>
        /// Saves the playlist.
        /// </summary>
        /// <param name="playlist">The playlist.</param>
        public void SavePlaylist(string playlistFile)
        {
            try
            {
                var playlist = new Playlist();
                playlist.Filename = playlistFile;
                playlist.Tracks = GetTracks();
                Library.SavePlaylist(playlist);
                CurrentPlaylistFile = playlistFile;
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        /// <summary>
        /// Gets the index of the current track.
        /// </summary>
        /// <returns>The index of the current track.</returns>
        public int GetCurrentTrackIndex()
        {
            var currentTrack = TrackModels.Where(t => t.IsCurrent).FirstOrDefault();
            if (currentTrack == null)
            {
                return GetCurrentTrackIndexFromBassPlayer();
            }

            var currentBassTrack = BassPlayer.CurrentTrack == null ? "" : BassPlayer.CurrentTrack.Filename;

            if (currentBassTrack != currentTrack.Filename)
            {
                var currentIndex = TrackModels.IndexOf(currentTrack);
                var match = TrackModels.Where(t => t.Filename == currentBassTrack && TrackModels.IndexOf(t) > currentIndex).FirstOrDefault();
                if (match == null) return GetCurrentTrackIndexFromBassPlayer();
                else
                {
                    currentTrack.IsCurrent = false;
                    match.IsCurrent = true;
                    return TrackModels.IndexOf(match);
                }
            }
            else
            {
                return TrackModels.IndexOf(currentTrack);
            }
        }

        private int GetCurrentTrackIndexFromBassPlayer()
        {
            if (BassPlayer.CurrentTrack == null)
            {
                if (TrackModels.Count > 0)
                {
                    TrackModels[0].IsCurrent = true;
                    return 0;
                }
                {
                    return -1;
                }
            }
            else
            {
                var currentTrack = TrackModels.Where(t => t.Filename == BassPlayer.CurrentTrack.Filename).FirstOrDefault();
                if (currentTrack != null)
                {
                    currentTrack.IsCurrent = true;
                    return TrackModels.IndexOf(currentTrack);
                }
                else
                {
                    return -1;
                }
            }
        }

        private int _currentTrackIndex = 0;

        /// <summary>
        /// Removes a track.
        /// </summary>
        /// <param name="trackToRemove">The track to remove.</param>
        private void RemoveTrack(TrackModel trackToRemove)
        {
            if (trackToRemove == null) return;
            var tracksToRemove = new TrackModel[] { trackToRemove }.ToList();
            RemoveTracks(tracksToRemove);
        }

        /// <summary>
        /// Removes the specified tracks.
        /// </summary>
        /// <param name="tracksToRemove">The tracks to remove.</param>
        private void RemoveTracks(List<TrackModel> tracksToRemove)
        {
            if (tracksToRemove.Count == 0) return;
            var tracks = TrackModels.Except(tracksToRemove).ToList();
            TrackModels = tracks;
            grdPlaylist.ClearSelectedRows();
            SetNextBassPlayerTrack();
            BindData();

            if (PlaylistChanged != null) PlaylistChanged(this, EventArgs.Empty);
        }

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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MixLibrary MixLibrary { get; set; }

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Library Library { get; set; }

        /// <summary>
        /// Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BE.BassPlayer BassPlayer { get; set; }

        /// <summary>
        /// Gets the tracks.
        /// </summary>
        private List<TrackModel> TrackModels { get; set; }

        /// <summary>
        /// Gets the current playlist file.
        /// </summary>
        public string CurrentPlaylistFile { get; internal set; }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (TrackModels.Count == 0) return;
            var playlist = FileDialogHelper.SaveAs("Playlist (*.m3u)|*.m3u", "");
            if (playlist != "") SavePlaylist(playlist);
        }

        /// <summary>
        /// Handles the Load event of the PlaylistControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PlaylistControl_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            if (_loaded) return;

            _loaded = true;
        }

        private bool _loaded = false;

        /// <summary>
        /// Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    BassPlayer_OnTrackChange();
                }));
            }
            else BassPlayer_OnTrackChange();
        }

        /// <summary>
        /// Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange()
        {
            if (_bassPlayerOnTrackChange) return;
            _bassPlayerOnTrackChange = true;

            grdPlaylist.Invalidate();

            var currentTrack = GetCurrentTrack();
            var currentTrackDescription = currentTrack == null ? "" : currentTrack.Description;

            var nextTrack = GetNextTrack();
            var nextTrackDescription = nextTrack == null ? "" : nextTrack.Description;

            var currentBassTrackDescription = BassPlayer.CurrentTrack == null ? "" : BassPlayer.CurrentTrack.Description;
            var nextBassTrackDescription = BassPlayer.NextTrack == null ? "" : BassPlayer.NextTrack.Description;

            if (currentBassTrackDescription != currentTrackDescription || nextBassTrackDescription != nextTrackDescription)
            {
                for (var i = 0; i < 10; i++)
                {
                    Application.DoEvents();
                    Thread.Sleep(20);
                }

                SetNextBassPlayerTrack();
                //BindData();
            }

            _bassPlayerOnTrackChange = false;
        }

        private bool _bassPlayerOnTrackChange = false;

        /// <summary>
        /// Handles the Click event of the btnClear control.
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearTracks();
        }

        /// <summary>
        /// Clears the tracks.
        /// </summary>
        public void ClearTracks()
        {
            CurrentPlaylistFile = "";
            var tracks = new List<TrackModel>();
            TrackModels = tracks;
            BindData();
            if (PlaylistChanged != null) PlaylistChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the Click event of the btnRemove control.
        /// </summary>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            var tracksToRemove = GetSelectedTracks();
            RemoveTracks(tracksToRemove);
        }

        /// <summary>
        /// Handles the CellContentDoubleClick event of the grdPlaylist control.
        /// </summary>
        private void grdPlaylist_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var currentTrack = GetLibraryTrack(SetCurrentTrack(e.RowIndex));

            var forcePlay = new ForcePlayTrackHandler(ForcePlayTrack);
            BeginInvoke(forcePlay, currentTrack);
        }

        private delegate void ForcePlayTrackHandler(Track track);

        private void ForcePlayTrack(Track track)
        {
            _doNotBind = true;

            BassPlayer.ForcePlay(track.Filename);

            _doNotBind = false;

            grdPlaylist.InvalidateDisplayedRows();
        }

        private bool _doNotBind = false;

        private TrackModel SetCurrentTrack(int trackIndex)
        {
            _currentTrackIndex = trackIndex;
            TrackModels.ForEach(t => t.IsCurrent = false);
            TrackModels[trackIndex].IsCurrent = true;
            return TrackModels[trackIndex];
        }

        ///// <summary>
        ///// Handles the RowDragDrop event of the grdPlaylist control.
        ///// </summary>
        //public void grdPlaylist_RowDragDrop(object sender, DataGridView.RowDragDropEventArgs e)
        //{
        //    MoveSelectedTracks(e.DestinationRowIndex);
        //}

        /// <summary>
        /// Handles the Click event of the btnOpen control.
        /// </summary>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            var playlist = FileDialogHelper.OpenSingle("Playlist files (*.m3u)|*.m3u");
            if (playlist != "") OpenPlaylist(playlist);
        }

        ///// <summary>
        ///// Handles the CellClick event of the grdPlaylist control.
        ///// </summary>
        //private void grdPlaylist_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (this.TrackClicked != null) TrackClicked(this, EventArgs.Empty);
        //}

        /// <summary>
        /// Handles the Click event of the mnuOpenFileLocation control.
        /// </summary>
        private void mnuOpenFileLocation_Click(object sender, EventArgs e)
        {
            OpenFileLocation();
        }

        /// <summary>
        /// Handles the Click event of the mnuUpdateShufflerDetails control.
        /// </summary>
        private void mnuUpdateShufflerDetails_Click(object sender, EventArgs e)
        {
            UpdateShufflerDetails();
        }

        /// <summary>
        /// Handles the Click event of the mnuUpdateTrackDetails control.
        /// </summary>
        public void mnuUpdateTrackDetails_Click(object sender, EventArgs e)
        {
            UpdateTrackDetails();
        }

        /// <summary>
        /// Handles the Opening event of the mnuTrack control.
        /// </summary>
        private void mnuTrack_Opening(object sender, CancelEventArgs e)
        {
            var singleTrack = (GetSelectedTracks().Count == 1);
            mnuPlay.Visible = singleTrack;
            mnuSep1.Visible = singleTrack;
            mnuOpenFileLocation.Visible = singleTrack;
            mnuSep3.Visible = singleTrack;
            mnuUpdateTrackDetails.Visible = singleTrack;
            mnuUpdateShufflerDetails.Visible = singleTrack;
            mnuRemoveShufflerDetails.Visible = singleTrack;
            mnuSep4.Visible = singleTrack;
            mnuMixRank.Visible = (grdPlaylist.SelectedRows.Count > 0 && grdPlaylist.SelectedRows[0].Index > 0);

            BeginInvoke(new MethodInvoker(delegate()
            {
                BindTrackRankMenu();
                if (mnuMixRank.Visible) BindMixRankMenu();
                BindRemoveTrackFromPlaylistMenu();
                BindAddTrackToPlaylistMenu();
            }));
        }

        /// <summary>
        /// Binds the rank menu.
        /// </summary>
        private void BindTrackRankMenu()
        {
            var currentTrackRank = -1;
            if (GetSelectedTracks().Count == 1)
            {
                currentTrackRank = GetSelectedTrack().Rank;
            }
            for (var i = 0; i < 6; i++)
            {
                mnuTrackRank.DropDownItems[i].Text = MixLibrary.GetRankDescription(5 - i);
                ((ToolStripMenuItem)mnuTrackRank.DropDownItems[i]).Checked = ((5 - i) == currentTrackRank);
            }
        }

        /// <summary>
        /// Binds the rank menu.
        /// </summary>
        private void BindMixRankMenu()
        {
            var currentMixRank = -1;
            if (GetSelectedTracks().Count == 1 && grdPlaylist.SelectedRows[0].Index > 0)
            {
                var track2 = GetSelectedTrack();
                var track1 = GetTrackByIndex(grdPlaylist.SelectedRows[0].Index - 1);
                var mixRank = MixLibrary.GetMixLevel(track1, track2);
                currentMixRank = mixRank;
            }
            for (var i = 0; i < 6; i++)
            {
                mnuMixRank.DropDownItems[i].Text = MixLibrary.GetRankDescription(5 - i);
                ((ToolStripMenuItem)mnuMixRank.DropDownItems[i]).Checked = ((5 - i) == currentMixRank);
            }
        }

        private void BindAddTrackToPlaylistMenu()
        {
            var selectedTracks = GetSelectedLibraryTracks();

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
                //Application.DoEvents();
            }
            mnuAddTrackToPlaylist.DropDownItems.Add("(New Playlist)", null, mnuAddNewPlaylist_Click);
            mnuAddTrackToPlaylist.Visible = (mnuAddTrackToPlaylist.DropDownItems.Count > 0);
        }

        private void BindRemoveTrackFromPlaylistMenu()
        {
            // generate 'remove from playlist' sub menu
            mnuRemoveTrackFromPlaylist.DropDownItems.Clear();
            var selectedPlaylists = Library.GetPlaylistsForTracks(GetSelectedLibraryTracks());
            foreach (var playlist in selectedPlaylists)
            {
                mnuRemoveTrackFromPlaylist.DropDownItems.Add(playlist.Name, null, mnuRemoveTrackFromPlaylist_Click);
                //Application.DoEvents();
            }
            mnuRemoveTrackFromPlaylist.Visible = (mnuRemoveTrackFromPlaylist.DropDownItems.Count > 0);
        }

        //    // generate 'add to playlist' sub menu
        //    mnuAddTrackToPlaylist.DropDownItems.Clear();
        //    foreach (var playlist in this.Library.GetAllPlaylists())
        //    {
        //        if (this.GetSelectedTracks().Count == 1)
        //        {
        //            if (!playlist.Tracks.Contains(this.GetSelectedLibraryTracks()[0]))
        //            {
        //                mnuAddTrackToPlaylist.DropDownItems.Add(playlist.Name, null, mnuAddTrackToPlaylist_Click);
        //            }
        //        }
        //        else
        //        {
        //            mnuAddTrackToPlaylist.DropDownItems.Add(playlist.Name, null, mnuAddTrackToPlaylist_Click);
        //        }
        //    }
        //    mnuAddTrackToPlaylist.DropDownItems.Add("(New Playlist)", null, mnuAddNewPlaylist_Click);
        //    mnuAddTrackToPlaylist.Visible = (mnuAddTrackToPlaylist.DropDownItems.Count > 0);

        //    // generate 'remove from playlist' sub menu
        //    mnuRemoveTrackFromPlaylist.DropDownItems.Clear();
        //    foreach (var playlist in this.Library.GetPlaylistsForTracks(this.GetSelectedLibraryTracks()))
        //    {
        //        mnuRemoveTrackFromPlaylist.DropDownItems.Add(playlist.Name, null, mnuRemoveTrackFromPlaylist_Click);
        //    }
        //    mnuRemoveTrackFromPlaylist.Visible = (mnuRemoveTrackFromPlaylist.DropDownItems.Count > 0);
        //}

        /// <summary>
        /// Handles the Click event of the mnuAddTrackToPlaylist control.
        /// </summary>
        private void mnuAddTrackToPlaylist_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            var playlist = Library.GetPlaylistByName(menu.Text);
            Library.AddTracksToPlaylist(playlist, GetSelectedLibraryTracks());
        }

        /// <summary>
        /// Handles the Click event of the mnuAddNewPlaylist control.
        /// </summary>
        private void mnuAddNewPlaylist_Click(object sender, EventArgs e)
        {
            var tracks = GetSelectedLibraryTracks();
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
            Library.RemoveTracksFromPlaylist(playlist, GetSelectedLibraryTracks());
        }

        /// <summary>
        /// Handles the Click event of the mnuPlay control.
        /// </summary>
        private void mnuPlay_Click(object sender, EventArgs e)
        {
            var track = GetSelectedTrack();
            BassPlayer.ForcePlay(track.Filename);
        }

        #endregion

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
        /// Gets or sets the tool strip label.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ToolStripLabel ToolStripLabel { get; set; }

        private void SetToolStripLabel()
        {
            if (ToolStripLabel == null) return;

            var text = string.Format("{0} playlist tracks. Length: {1}",
                TrackModels.Count,
                BassHelper.GetFormattedLength(TrackModels.Sum(t => t.Length)));

            ToolStripLabel.Text = text;
        }

        private void SetMixAndKeyRanks()
        {
            for (var i = 0; i < TrackModels.Count; i++)
            {
                UpdateMixRank(i);
            }
        }

        private void UpdateMixRank(int rowIndex)
        {
            if (rowIndex == 0)
            {
                TrackModels[0].MixRank = 0;
                TrackModels[0].MixRankDescription = "";
                return;
            }

            var track1 = GetTrackByIndex(rowIndex - 1);
            var track2 = GetTrackByIndex(rowIndex);
            var mixRank = MixLibrary.GetMixLevel(track1, track2);
            var hasExtendedMix = MixLibrary.HasExtendedMix(track1, track2);

            TrackModels[rowIndex].MixRank = mixRank;
            TrackModels[rowIndex].MixRankDescription = MixLibrary.GetRankDescription(mixRank);
            if (hasExtendedMix) TrackModels[rowIndex].MixRankDescription += "*";

            TrackModels[rowIndex].KeyRankDescription = KeyHelper.GetKeyMixRankDescription(track1.Key, track2.Key);
        }

        private void mnuTrackRank_Click(object sender, EventArgs e)
        {
            var trackRankDescription = (sender as ToolStripDropDownItem).Text;
            var trackRank = MixLibrary.GetRankFromDescription(trackRankDescription);

            foreach (var track in GetSelectedLibraryTracks())
            {
                track.Rank = trackRank;
                Library.SaveRank(track);
                var trackModel = TrackModels.Where(t => t.Description == track.Description).FirstOrDefault();
                trackModel.TrackRank = trackRank;
                trackModel.TrackRankDescription = trackRankDescription;
            }
            //BindData();
            grdPlaylist.InvalidateDisplayedRows();
        }

        private void mnuMixRank_Click(object sender, EventArgs e)
        {
            var mixRankDescription = (sender as ToolStripDropDownItem).Text;
            var mixRank = MixLibrary.GetRankFromDescription(mixRankDescription);

            foreach (DataGridViewRow row in grdPlaylist.SelectedRows)
            {
                if (row.Index == 0) continue;
                var track2 = GetTrackByIndex(row.Index);
                var track1 = GetTrackByIndex(row.Index - 1);
                MixLibrary.SetMixLevel(track1, track2, mixRank);

                var trackModel = GetTrackModelByIndex(row.Index);
                trackModel.MixRank = mixRank;
                trackModel.MixRankDescription = mixRankDescription;
            }

            grdPlaylist.InvalidateDisplayedRows();
        }

        /// <summary>
        /// Handles the CellFormatting event of the grdPlaylist control.
        /// </summary>
        private void grdPlaylist_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (e.CellStyle == null) return;

            if (e.RowIndex == GetCurrentTrackIndex())
            {
                if (!e.CellStyle.Font.Bold) e.CellStyle.Font = FontHelper.EmboldenFont(_font);
            }
            else
            {
                if (e.CellStyle.Font.Bold) e.CellStyle.Font = _font;
            }
        }

        private Font _font = new Font("Segoe UI", 9, GraphicsUnit.Point);

        private void grdPlaylist_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex == -1) return;
            var row = grdPlaylist.Rows[e.RowIndex];
            var trackModel = GetTrackModelByIndex(e.RowIndex);

            if (trackModel == null) e.Value = "";
            else if (e.ColumnIndex == 0) e.Value = trackModel.Description;
            else if (e.ColumnIndex == 1) e.Value = trackModel.LengthFormatted;
            else if (e.ColumnIndex == 2) e.Value = trackModel.Bpm;
            else if (e.ColumnIndex == 3)
            {
                UpdateMixRank(e.RowIndex);
                e.Value = trackModel.MixRankDescription;
            }
            else if (e.ColumnIndex == 4) e.Value = trackModel.TrackRankDescription;
            else if (e.ColumnIndex == 5) e.Value = trackModel.Key;
            else if (e.ColumnIndex == 6) e.Value = trackModel.KeyRankDescription;
        }

        private TrackModel GetTrackModelByIndex(int index)
        {
            if (TrackModels == null) return null;
            if (index < 0 || index >= TrackModels.Count) return null;
            return TrackModels[index];
        }

        public void QueueFiles(List<string> files)
        {
            var tracks = new List<Track>();
            foreach (var file in files)
            {
                var track = Library.GetTrackByFilename(file);
                if (track != null) tracks.Add(track);
            }

            QueueTracks(tracks);
        }

        /// <summary>
        /// Handles the SelectionChanged event of the grdPlaylist control.
        /// </summary>
        private void grdPlaylist_SelectionChanged(object sender, EventArgs e)
        {
            ShowCurrentTrackDetails();
        }

        private void ShowCurrentTrackDetails()
        {
            var track = GetSelectedTrack();
            trackDetails.DisplayTrackDetails(track);
            if (!splTopBottom.Panel2Collapsed)
                mixableTracks.DisplayMixableTracks(track);
        }

        /// <summary>
        /// Handles the CellContentDoubleClick event of the grdPlaylist control.
        /// </summary>
        public void ReplayMix()
        {
            var previousTrack = GetPreviousTrack();
            if (previousTrack == null) return;

            var index = GetCurrentTrackIndex() - 1;
            SetCurrentTrack(index);

            _doNotBind = true;
            BassPlayer.ForcePlay(previousTrack.Filename);
            BassPlayer.SkipToEnd();
            BassPlayer.Play();

            _doNotBind = false;

            //BindData();
            grdPlaylist.InvalidateDisplayedRows();
        }

        public int GetNumberOfTracksRemaining()
        {
            return TrackModels.Count - (GetCurrentTrackIndex() + 1);
        }
    }
}