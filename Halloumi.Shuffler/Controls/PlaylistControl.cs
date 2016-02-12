using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Controls;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.Engine;
using Halloumi.Shuffler.Engine.Models;
using Halloumi.Shuffler.Forms;
using AE = Halloumi.Shuffler.AudioEngine;

namespace Halloumi.Shuffler.Controls
{
    public partial class PlaylistControl : BaseUserControl
    {
        private readonly Font _font = new Font("Segoe UI", 9, GraphicsUnit.Point);

        private bool _bassPlayerOnTrackChange;

        private bool _doNotBind;

        private bool _loaded;
        public EventHandler PlaylistChanged;
        public EventHandler TrackClicked;


        /// <summary>
        ///     Initializes a new instance of the PlaylistControl class.
        /// </summary>
        public PlaylistControl()
        {
            InitializeComponent();

            grdPlaylist.CellDoubleClick += grdPlaylist_CellContentDoubleClick;
            grdPlaylist.CellFormatting += grdPlaylist_CellFormatting;
            grdPlaylist.CellValueNeeded += grdPlaylist_CellValueNeeded;
            grdPlaylist.SelectionChanged += grdPlaylist_SelectionChanged;

            mnuOpenFileLocation.Click += mnuOpenFileLocation_Click;
            mnuUpdateTrackDetails.Click += mnuUpdateTrackDetails_Click;
            mnuUpdateShufflerDetails.Click += mnuUpdateShufflerDetails_Click;
            mnuPlay.Click += mnuPlay_Click;
            mnuTrack.Opening += mnuTrack_Opening;

            btnClear.Click += btnClear_Click;
            btnOpen.Click += btnOpen_Click;
            btnRemove.Click += btnRemove_Click;
            btnSave.Click += btnSave_Click;

            Load += PlaylistControl_Load;
            TrackModels = new List<TrackModel>();
        }

        /// <summary>
        ///     Gets or sets the tool strip label.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ToolStripLabel ToolStripLabel { get; set; }

        public bool ShowMixableTracks
        {
            set
            {
                splTopBottom.Panel2Collapsed = !value;
                ShowCurrentTrackDetails();
            }
        }


        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MixLibrary MixLibrary { get; set; }

        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Library Library { get; set; }

        /// <summary>
        ///     Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AE.BassPlayer BassPlayer { get; set; }

        /// <summary>
        ///     Gets the tracks.
        /// </summary>
        private List<TrackModel> TrackModels { get; set; }

        /// <summary>
        ///     Gets the current play-list file.
        /// </summary>
        public string CurrentPlaylistFile { get; internal set; }

        private void mnuRemoveShufflerDetails_Click(object sender, EventArgs e)
        {
            var track = GetSelectedTrack();
            if (track == null) return;

            var message = $"Are you sure you wish to remove the shuffler details for '{track.Description}'?";
            if (!MessageBoxHelper.Confirm(message)) return;
            Library.RemoveShufflerDetails(track);
            BindData();
        }

        private void SetToolStripLabel()
        {
            if (ToolStripLabel == null) return;

            var text =
                $"{TrackModels.Count} play-list tracks. Length: {FormatHelper.GetFormattedHours(TrackModels.Sum(t => t.Length))}";

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
                TrackModels[0].MixRankDescription = "";
                return;
            }

            var track1 = GetTrackByIndex(rowIndex - 1);
            var track2 = GetTrackByIndex(rowIndex);

            var mixRank = MixLibrary.GetMixLevel(track1, track2);
            var hasExtendedMix = MixLibrary.HasExtendedMix(track1, track2);

            TrackModels[rowIndex].MixRankDescription = MixLibrary.GetRankDescription(mixRank);
            if (hasExtendedMix) TrackModels[rowIndex].MixRankDescription += "*";

            TrackModels[rowIndex].KeyRankDescription = KeyHelper.GetKeyMixRankDescription(track1.Key, track2.Key);
        }

        private void mnuTrackRank_Click(object sender, EventArgs e)
        {
            var toolStripDropDownItem = sender as ToolStripDropDownItem;
            if (toolStripDropDownItem == null)
                return;

            var trackRankDescription = toolStripDropDownItem.Text;
            var trackRank = MixLibrary.GetRankFromDescription(trackRankDescription);

            foreach (var track in GetSelectedLibraryTracks())
            {
                track.Rank = trackRank;
                Library.SaveRank(track);
                var trackModel = TrackModels.FirstOrDefault(t => t.Description == track.Description);
                if (trackModel == null) continue;
                trackModel.TrackRankDescription = trackRankDescription;
            }

            //BindData();
            grdPlaylist.InvalidateDisplayedRows();
        }

        private void mnuMixRank_Click(object sender, EventArgs e)
        {
            var toolStripDropDownItem = sender as ToolStripDropDownItem;
            if (toolStripDropDownItem == null) return;

            var mixRankDescription = toolStripDropDownItem.Text;
            var mixRank = MixLibrary.GetRankFromDescription(mixRankDescription);

            foreach (DataGridViewRow row in grdPlaylist.SelectedRows)
            {
                if (row.Index == 0) continue;
                var track2 = GetTrackByIndex(row.Index);
                var track1 = GetTrackByIndex(row.Index - 1);
                MixLibrary.SetMixLevel(track1, track2, mixRank);

                var trackModel = GetTrackModelByIndex(row.Index);
                trackModel.MixRankDescription = mixRankDescription;
            }


            grdPlaylist.InvalidateDisplayedRows();
        }

        /// <summary>
        ///     Handles the CellFormatting event of the grdPlaylist control.
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

        private void grdPlaylist_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex == -1) return;
            var trackModel = GetTrackModelByIndex(e.RowIndex);

            if (trackModel == null) e.Value = "";
            else if (e.ColumnIndex == 0)
            {
                e.Value = trackModel.Description;
            }
            else if (e.ColumnIndex == 1)
            {
                e.Value = trackModel.LengthFormatted;
            }
            else if (e.ColumnIndex == 2)
            {
                e.Value = trackModel.Bpm;
            }
            else if (e.ColumnIndex == 3)
            {
                UpdateMixRank(e.RowIndex);
                e.Value = trackModel.MixRankDescription;
            }
            else if (e.ColumnIndex == 4)
            {
                e.Value = trackModel.TrackRankDescription;
            }
            else if (e.ColumnIndex == 5)
            {
                e.Value = trackModel.Key;
            }
            else if (e.ColumnIndex == 6)
            {
                e.Value = trackModel.KeyRankDescription;
            }
        }

        private TrackModel GetTrackModelByIndex(int index)
        {
            if (TrackModels == null) return null;
            if (index < 0 || index >= TrackModels.Count) return null;
            return TrackModels[index];
        }

        public void QueueFiles(List<string> files)
        {
            var tracks = files.Select(file => Library.GetTrackByFilename(file)).Where(track => track != null).ToList();

            QueueTracks(tracks);
        }

        /// <summary>
        ///     Handles the SelectionChanged event of the grdPlaylist control.
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
        ///     Handles the CellContentDoubleClick event of the grdPlaylist control.
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

        public void Initalize()
        {
            trackDetails.Library = Library;
            trackDetails.DisplayTrackDetails(null);

            mixableTracks.MixLibrary = MixLibrary;
            mixableTracks.PlaylistControl = this;
            mixableTracks.Initialize();

            if (BassPlayer == null) return;
            BassPlayer.OnTrackChange += BassPlayer_OnTrackChange;
            BassPlayer.OnSkipToEnd += BassPlayer_OnFadeEnded;
            BassPlayer.OnEndFadeIn += BassPlayer_OnFadeEnded;
        }

        /// <summary>
        ///     Handles the OnEndFadeIn event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnFadeEnded(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(PreloadTrack));
            }
            else PreloadTrack();
        }

        /// <summary>
        ///     Handles the OnEndFadeIn event of the BassPlayer control.
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
        ///     Queues tracks.
        /// </summary>
        /// <param name="queueTracks">The queue tracks.</param>
        public void QueueTracks(List<Track> queueTracks)
        {
            TrackModels.AddRange(TrackModel.ToList(queueTracks, MixLibrary));


            if (BassPlayer.CurrentTrack == null && TrackModels.Count > 0)
            {
                var track = GetTrackByIndex(0);
                BassPlayer.QueueTrack(track.Filename);
            }
            SetNextBassPlayerTrack();
            PreloadTrack();

            PlaylistChanged?.Invoke(this, EventArgs.Empty);
            BindData();
        }

        private Track GetLibraryTrack(TrackModel trackModel)
        {
            var track = Library.GetTrackByFilename(trackModel.Filename);
            if (track == null || !File.Exists(track.Filename))
            {
                track = Library
                    .GetTracksByDescription(trackModel.Description)
                    .FirstOrDefault(t => File.Exists(t.Filename));
            }

            return track;
        }


        /// <summary>
        ///     Queues a tracks
        /// </summary>
        /// <param name="queueTrack">The queue track.</param>
        public void QueueTrack(Track queueTrack)
        {
            var queueTracks = new List<Track> {queueTrack};
            QueueTracks(queueTracks);
        }

        /// <summary>
        ///     Opens the play-list.
        /// </summary>
        /// <param name="playlistName">Name of the play-list.</param>
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
        ///     Queues a play-list.
        /// </summary>
        /// <param name="playlist">The play-list.</param>
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
        ///     Gets the next track.
        /// </summary>
        /// <returns>The next track.</returns>
        public Track GetCurrentTrack()
        {
            return GetTrackByIndex(GetCurrentTrackIndex());
        }

        /// <summary>
        ///     Gets a track by its index
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The track at the index</returns>
        private Track GetTrackByIndex(int index)
        {
            if (index >= 0 && index < TrackModels.Count)
                return GetLibraryTrack(TrackModels[index]);
            return null;
        }

        /// <summary>
        ///     Gets the next track.
        /// </summary>
        /// <returns>The next track.</returns>
        public Track GetNextTrack()
        {
            return GetTrackByIndex(GetCurrentTrackIndex() + 1);
        }

        /// <summary>
        ///     Gets the next track.
        /// </summary>
        /// <returns>The next track.</returns>
        private Track GetTrackAfterNext()
        {
            return GetTrackByIndex(GetCurrentTrackIndex() + 2);
        }

        /// <summary>
        ///     Gets the previous track.
        /// </summary>
        /// <returns>The previous track.</returns>
        public Track GetPreviousTrack()
        {
            return GetTrackByIndex(GetCurrentTrackIndex() - 1);
        }

        /// <summary>
        ///     Gets the selected track from the grid.
        /// </summary>
        /// <returns>The selected track</returns>
        public Track GetSelectedTrack()
        {
            var tracks = GetSelectedTracks();
            return tracks.Count == 0 ? null : GetLibraryTrack(tracks[0]);
        }

        /// <summary>
        ///     Gets the last track.
        /// </summary>
        /// <returns>The last track</returns>
        public Track GetLastTrack()
        {
            return TrackModels.Count == 0 ? null : GetLibraryTrack(TrackModels.Last());
        }

        /// <summary>
        ///     Gets the last track.
        /// </summary>
        /// <returns>The last track</returns>
        public Track GetSecondToLastTrack()
        {
            return TrackModels.Count < 2 ? null : GetLibraryTrack(TrackModels[TrackModels.Count - 2]);
        }

        /// <summary>
        ///     Removes the last track.
        /// </summary>
        public void RemoveLastTrack()
        {
            if (TrackModels.Count == 0) return;
            var lastTrack = TrackModels.Last();
            if (lastTrack != null) RemoveTrack(lastTrack);
        }

        /// <summary>
        ///     Gets the tracks in the play-list
        /// </summary>
        /// <returns>The tracks in the play-list</returns>
        public List<Track> GetTracks()
        {
            return TrackModels.Select(GetLibraryTrack).ToList();
        }

        /// <summary>
        ///     Plays the next track.
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
        ///     Plays the previous track.
        /// </summary>
        public void PlayPreviousTrack()
        {
            var track = GetPreviousTrack();
            if (track == null) return;
            var currentIndex = GetCurrentTrackIndex();
            TrackModels[currentIndex].IsCurrent = false;
            TrackModels[currentIndex - 1].IsCurrent = true;
            BassPlayer.ForcePlay(track.Filename);
        }


        /// <summary>
        ///     Binds the data for the user control to the controls
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
            lblCount.Text = $"{trackCount} tracks";
        }

        /// <summary>
        ///     Queues the next track.
        /// </summary>
        private void SetNextBassPlayerTrack()
        {
            var nextTrack = GetNextTrack();

            if (nextTrack == null) return;
            if (BassPlayer.NextTrack == null
                ||
                (BassPlayer.NextTrack != null && BassPlayer.NextTrack.Description != nextTrack.Description))
            {
                BassPlayer.QueueTrack(nextTrack.Filename);
            }
        }

        /// <summary>
        ///     Gets the selected tracks from the grid.
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
        ///     Gets the selected library tracks.
        /// </summary>
        /// <returns>The selected library tracks</returns>
        private List<Track> GetSelectedLibraryTracks()
        {
            return GetSelectedTracks().Select(GetLibraryTrack).ToList();
        }

        /// <summary>
        ///     Saves the play-list.
        /// </summary>
        /// <param name="playlistFile">The play-list file.</param>
        public void SavePlaylist(string playlistFile)
        {
            try
            {
                var playlist = new Playlist
                {
                    Filename = playlistFile,
                    Tracks = GetTracks()
                };
                Library.SavePlaylist(playlist);
                CurrentPlaylistFile = playlistFile;
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        /// <summary>
        ///     Gets the index of the current track.
        /// </summary>
        /// <returns>The index of the current track.</returns>
        public int GetCurrentTrackIndex()
        {
            var currentTrack = TrackModels.FirstOrDefault(t => t.IsCurrent);
            if (currentTrack == null)
            {
                return GetCurrentTrackIndexFromBassPlayer();
            }

            var currentBassTrack = BassPlayer.CurrentTrack == null ? "" : BassPlayer.CurrentTrack.Filename;

            if (currentBassTrack == currentTrack.Filename) return TrackModels.IndexOf(currentTrack);

            var currentIndex = TrackModels.IndexOf(currentTrack);
            var match = TrackModels
                    .FirstOrDefault(t => t.Filename == currentBassTrack && TrackModels.IndexOf(t) > currentIndex);
            if (match == null) return GetCurrentTrackIndexFromBassPlayer();
            currentTrack.IsCurrent = false;
            match.IsCurrent = true;
            return TrackModels.IndexOf(match);
        }

        private int GetCurrentTrackIndexFromBassPlayer()
        {
            if (BassPlayer.CurrentTrack == null)
            {
                if (TrackModels.Count <= 0) return -1;
                TrackModels[0].IsCurrent = true;
                return 0;
            }
            var currentTrack = TrackModels.FirstOrDefault(t => t.Filename == BassPlayer.CurrentTrack.Filename);
            if (currentTrack == null) return -1;
            currentTrack.IsCurrent = true;
            return TrackModels.IndexOf(currentTrack);
        }

        /// <summary>
        ///     Removes a track.
        /// </summary>
        /// <param name="trackToRemove">The track to remove.</param>
        private void RemoveTrack(TrackModel trackToRemove)
        {
            if (trackToRemove == null) return;
            var tracksToRemove = new[] {trackToRemove}.ToList();
            RemoveTracks(tracksToRemove);
        }

        /// <summary>
        ///     Removes the specified tracks.
        /// </summary>
        /// <param name="tracksToRemove">The tracks to remove.</param>
        private void RemoveTracks(IReadOnlyCollection<TrackModel> tracksToRemove)
        {
            if (tracksToRemove.Count == 0) return;
            var tracks = TrackModels.Except(tracksToRemove).ToList();
            TrackModels = tracks;
            grdPlaylist.ClearSelectedRows();
            SetNextBassPlayerTrack();
            BindData();

            PlaylistChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Opens the file location of the selected track
        /// </summary>
        private void OpenFileLocation()
        {
            var track = GetSelectedTrack();
            if (track == null) return;
            var directoryName = Path.GetDirectoryName(track.Filename) + "";
            Process.Start(directoryName);
        }

        /// <summary>
        ///     Updates the shuffler details.
        /// </summary>
        private void UpdateShufflerDetails()
        {
            if (GetSelectedTrack() == null) return;

            var form = new FrmShufflerDetails
            {
                BassPlayer = BassPlayer,
                Filename = GetSelectedTrack().Filename
            };

            var result = form.ShowDialog();
            if (result != DialogResult.OK) return;
            Library.ReloadTrack(GetSelectedTrack().Filename);
            BassPlayer.ReloadTrack(GetSelectedTrack().Filename);
            BindData();
        }

        /// <summary>
        ///     Updates the details of the selected track
        /// </summary>
        private void UpdateTrackDetails()
        {
            if (GetSelectedTrack() == null) return;
            var form = new FrmUpdateTrackDetails
            {
                Library = Library,
                Track = GetSelectedTrack()
            };
            var result = form.ShowDialog();
            if (result == DialogResult.OK) BindData();
        }


        /// <summary>
        ///     Handles the Click event of the btnSave control.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (TrackModels.Count == 0) return;
            var playlist = FileDialogHelper.SaveAs("Play-list (*.m3u)|*.m3u", "");
            if (playlist != "") SavePlaylist(playlist);
        }

        /// <summary>
        ///     Handles the Load event of the PlaylistControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void PlaylistControl_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            if (_loaded) return;

            _loaded = true;
        }

        /// <summary>
        ///     Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(BassPlayer_OnTrackChange));
            }
            else BassPlayer_OnTrackChange();
        }

        /// <summary>
        ///     Handles the OnTrackChange event of the BassPlayer control.
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

            if (currentBassTrackDescription != currentTrackDescription ||
                nextBassTrackDescription != nextTrackDescription)
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

        /// <summary>
        ///     Handles the Click event of the btnClear control.
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearTracks();
        }

        /// <summary>
        ///     Clears the tracks.
        /// </summary>
        public void ClearTracks()
        {
            CurrentPlaylistFile = "";
            var tracks = new List<TrackModel>();
            TrackModels = tracks;
            BindData();
            PlaylistChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Handles the Click event of the btnRemove control.
        /// </summary>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            var tracksToRemove = GetSelectedTracks();
            RemoveTracks(tracksToRemove);
        }

        /// <summary>
        ///     Handles the CellContentDoubleClick event of the grdPlaylist control.
        /// </summary>
        private void grdPlaylist_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var currentTrack = GetLibraryTrack(SetCurrentTrack(e.RowIndex));

            var forcePlay = new ForcePlayTrackHandler(ForcePlayTrack);
            BeginInvoke(forcePlay, currentTrack);
        }

        private void ForcePlayTrack(Track track)
        {
            _doNotBind = true;

            BassPlayer.ForcePlay(track.Filename);

            _doNotBind = false;

            grdPlaylist.InvalidateDisplayedRows();
        }

        private TrackModel SetCurrentTrack(int trackIndex)
        {
            TrackModels.ForEach(t => t.IsCurrent = false);
            TrackModels[trackIndex].IsCurrent = true;
            return TrackModels[trackIndex];
        }

        /// <summary>
        ///     Handles the Click event of the btnOpen control.
        /// </summary>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            var playlist = FileDialogHelper.OpenSingle("Playlist files (*.m3u)|*.m3u");
            if (playlist != "") OpenPlaylist(playlist);
        }


        /// <summary>
        ///     Handles the Click event of the mnuOpenFileLocation control.
        /// </summary>
        private void mnuOpenFileLocation_Click(object sender, EventArgs e)
        {
            OpenFileLocation();
        }

        /// <summary>
        ///     Handles the Click event of the mnuUpdateShufflerDetails control.
        /// </summary>
        private void mnuUpdateShufflerDetails_Click(object sender, EventArgs e)
        {
            UpdateShufflerDetails();
        }

        /// <summary>
        ///     Handles the Click event of the mnuUpdateTrackDetails control.
        /// </summary>
        public void mnuUpdateTrackDetails_Click(object sender, EventArgs e)
        {
            UpdateTrackDetails();
        }

        /// <summary>
        ///     Handles the Opening event of the mnuTrack control.
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

            BeginInvoke(new MethodInvoker(delegate
            {
                BindTrackRankMenu();
                if (mnuMixRank.Visible) BindMixRankMenu();
                BindRemoveTrackFromPlaylistMenu();
                BindAddTrackToPlaylistMenu();
            }));
        }

        /// <summary>
        ///     Binds the rank menu.
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
                ((ToolStripMenuItem) mnuTrackRank.DropDownItems[i]).Checked = ((5 - i) == currentTrackRank);
            }
        }

        /// <summary>
        ///     Binds the rank menu.
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
                ((ToolStripMenuItem) mnuMixRank.DropDownItems[i]).Checked = ((5 - i) == currentMixRank);
            }
        }

        private void BindAddTrackToPlaylistMenu()
        {
            var selectedTracks = GetSelectedLibraryTracks();

            Track selectedTrack = null;
            if (selectedTracks.Count == 1) selectedTrack = selectedTracks[0];

            var playlists = Library.GetAllPlaylists();

            // generate 'add to play-list' sub menu
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
            }
            mnuAddTrackToPlaylist.DropDownItems.Add("(New Playlist)", null, mnuAddNewPlaylist_Click);
            mnuAddTrackToPlaylist.Visible = (mnuAddTrackToPlaylist.DropDownItems.Count > 0);
        }

        private void BindRemoveTrackFromPlaylistMenu()
        {
            // generate 'remove from play-list' sub menu
            mnuRemoveTrackFromPlaylist.DropDownItems.Clear();
            var selectedPlaylists = Library.GetPlaylistsForTracks(GetSelectedLibraryTracks());
            foreach (var playlist in selectedPlaylists)
            {
                mnuRemoveTrackFromPlaylist.DropDownItems.Add(playlist.Name, null, mnuRemoveTrackFromPlaylist_Click);
            }
            mnuRemoveTrackFromPlaylist.Visible = (mnuRemoveTrackFromPlaylist.DropDownItems.Count > 0);
        }

        /// <summary>
        ///     Handles the Click event of the mnuAddTrackToPlaylist control.
        /// </summary>
        private void mnuAddTrackToPlaylist_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            if (menu == null) return;

            var playlist = Library.GetPlaylistByName(menu.Text);
            Library.AddTracksToPlaylist(playlist, GetSelectedLibraryTracks());
        }

        /// <summary>
        ///     Handles the Click event of the mnuAddNewPlaylist control.
        /// </summary>
        private void mnuAddNewPlaylist_Click(object sender, EventArgs e)
        {
            var tracks = GetSelectedLibraryTracks();
            var form = new FrmAddPlaylist
            {
                Library = Library,
                Tracks = tracks
            };
            var result = form.ShowDialog();
            if (result == DialogResult.OK) BindData();
        }

        /// <summary>
        ///     Handles the Click event of the mnuRemoveTrackFromPlaylist control.
        /// </summary>
        private void mnuRemoveTrackFromPlaylist_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            if (menu == null) return;

            var playlist = Library.GetPlaylistByName(menu.Text);
            Library.RemoveTracksFromPlaylist(playlist, GetSelectedLibraryTracks());
        }

        /// <summary>
        ///     Handles the Click event of the mnuPlay control.
        /// </summary>
        private void mnuPlay_Click(object sender, EventArgs e)
        {
            var track = GetSelectedTrack();
            BassPlayer.ForcePlay(track.Filename);
        }

        private class TrackModel
        {
            private TrackModel(Track track, MixLibrary mixLibrary)
            {
                Description = track.Description;
                Filename = track.Filename;
                LengthFormatted = track.LengthFormatted;
                IsCurrent = false;
                Bpm = track.Bpm;
                Length = track.Length;
                TrackRankDescription = mixLibrary.GetRankDescription(track.Rank);
                Key = KeyHelper.GetDisplayKey(track.Key);
            }

            public string Description { get; }

            public string Filename { get; }

            public string LengthFormatted { get; }

            public decimal Bpm { get; }

            public decimal Length { get; }

            public bool IsCurrent { get; set; }

            public string MixRankDescription { get; set; }

            public string TrackRankDescription { get; set; }

            public string Key { get; }

            public string KeyRankDescription { get; set; }

            public static IEnumerable<TrackModel> ToList(IEnumerable<Track> tracks, MixLibrary mixLibrary)
            {
                return tracks.Select(t => new TrackModel(t, mixLibrary)).ToList();
            }
        }

        private delegate void ForcePlayTrackHandler(Track track);
    }
}