using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Controls;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;
using Halloumi.Shuffler.Forms;

namespace Halloumi.Shuffler.Controls
{
    public partial class PlaylistControl : BaseUserControl
    {
        private readonly Font _font = new Font("Segoe UI", 9, GraphicsUnit.Point);

        private bool _binding;

        private bool _doNotBind;

        private bool _loaded;
       
        /// public EventHandler TrackClicked;
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
        }

        /// <summary>
        ///     Gets or sets the tool strip label.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ToolStripLabel ToolStripLabel { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowMixableTracks
        {
            set
            {
                splTopBottom.Panel2Collapsed = !value;
                ShowCurrentTrackDetails();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowTrackDetails
        {
            set
            {
                trackDetails.Visible = value;
                ShowCurrentTrackDetails();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ShufflerApplication ShufflerApplication { get; set; }


        private void mnuRemoveShufflerDetails_Click(object sender, EventArgs e)
        {
            var track = GetSelectedTrack();
            if (track == null) return;

            var message = $"Are you sure you wish to remove the shuffler details for '{track.Description}'?";
            if (!MessageBoxHelper.Confirm(message)) return;
            ShufflerApplication.Library.RemoveShufflerDetails(track);
            BindData();
        }

        private void SetToolStripLabel()
        {
            if (ToolStripLabel == null) return;

            var text = 
                $"{ShufflerApplication.Playlist.Tracks.Count} tracks in playlist ({TimeFormatHelper.GetFormattedHours(ShufflerApplication.Playlist.Tracks.Sum(t => t.Length))})";

            ToolStripLabel.Text = text;
        }

        //private void SetMixAndKeyRanks()
        //{
        //    Parallel.For(0, Application.Playlist.Tracks.Count, UpdateMixRank);
        //}

        private string GetMixRankDescription(int rowIndex)
        {
            if (rowIndex == 0)
                return "";

            var track1 = GetTrackByIndex(rowIndex - 1);
            var track2 = GetTrackByIndex(rowIndex);

            if (track1 == null || track2 == null)
                return "";

            return ShufflerApplication.MixLibrary.GetExtendedMixDescription(track1, track2);
        }

        private string GetKeyMixRankDescription(int rowIndex)
        {
            if (rowIndex == 0)
                return "";

            var track1 = GetTrackByIndex(rowIndex - 1);
            var track2 = GetTrackByIndex(rowIndex);

            if (track1 == null || track2 == null)
                return "";

            return KeyHelper.GetKeyMixRankDescription(track1.Key, track2.Key);
        }

        private void mnuTrackRank_Click(object sender, EventArgs e)
        {
            var toolStripDropDownItem = sender as ToolStripDropDownItem;
            if (toolStripDropDownItem == null)
                return;

            var trackRankDescription = toolStripDropDownItem.Text;
            var trackRank = ShufflerApplication.MixLibrary.GetRankFromDescription(trackRankDescription);

            var tracks = GetSelectedTracks();
            ShufflerApplication.Library.SetRank(tracks, (int) trackRank);

            grdPlaylist.InvalidateDisplayedRows();
        }

        private void mnuMixRank_Click(object sender, EventArgs e)
        {
            var toolStripDropDownItem = sender as ToolStripDropDownItem;
            if (toolStripDropDownItem == null) return;

            var mixRankDescription = toolStripDropDownItem.Text;
            var mixRank = ShufflerApplication.MixLibrary.GetRankFromDescription(mixRankDescription);

            foreach (DataGridViewRow row in grdPlaylist.SelectedRows)
            {
                if (row.Index == 0) continue;
                var track2 = GetTrackByIndex(row.Index);
                var track1 = GetTrackByIndex(row.Index - 1);
                ShufflerApplication.MixLibrary.SetMixLevel(track1, track2, (int) mixRank);
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
            var track = GetTrackByIndex(e.RowIndex);

            if (track == null)
            {
                e.Value = "";
            }
            else if (e.ColumnIndex == 0)
            {
                e.Value = track.Description;
            }
            else if (e.ColumnIndex == 1)
            {
                e.Value = track.LengthFormatted;
            }
            else if (e.ColumnIndex == 2)
            {
                e.Value = track.Bpm;
            }
            else if (e.ColumnIndex == 3)
            {
                e.Value = GetMixRankDescription(e.RowIndex);
            }
            else if (e.ColumnIndex == 4)
            {
                //e.Value = track.TrackRankDescription;
            }
            else if (e.ColumnIndex == 5)
            {
                e.Value = track.Key;
            }
            else if (e.ColumnIndex == 6)
            {
                e.Value = GetKeyMixRankDescription(e.RowIndex);
            }
        }

        public void QueueFiles(List<string> files)
        {
            var tracks = files.Select(file => ShufflerApplication.Library.GetTrackByFilename(file)).Where(track => track != null).ToList();

            QueueTracks(tracks);
        }

        /// <summary>
        ///     Handles the SelectionChanged event of the grdPlaylist control.
        /// </summary>
        private void grdPlaylist_SelectionChanged(object sender, EventArgs e)
        {
            if (_binding) return;

            Debug.WriteLine("start show current");
            ShowCurrentTrackDetails();
            Debug.WriteLine("end show current");
        }

        private void ShowCurrentTrackDetails()
        {
            var track = GetSelectedTrack();
            if (trackDetails.Visible)
                trackDetails.DisplayTrackDetails(track);

            if (!splTopBottom.Panel2Collapsed)
                mixableTracks.DisplayMixableTracks(track);
        }

        /// <summary>
        ///     Handles the CellContentDoubleClick event of the grdPlaylist control.
        /// </summary>
        public void ReplayMix()
        {
            ShufflerApplication.Playlist.PlayPrevious();
            ShufflerApplication.BassPlayer.SkipToFadeOut();
        }

        public int GetNumberOfTracksRemaining()
        {
            return ShufflerApplication.Playlist.GetNumberOfTracksRemaining();
        }

        public void Initalize(TrackLibraryControl trackLibraryControl, ShufflerApplication application)
        {
            ShufflerApplication = application;

            trackDetails.Library = application.Library;
            trackDetails.DisplayTrackDetails(null);

            mixableTracks.PlaylistControl = this;
            mixableTracks.Initialize(application.MixLibrary, trackLibraryControl);
        }


        /// <summary>
        ///     Queues tracks.
        /// </summary>
        /// <param name="queueTracks">The queue tracks.</param>
        public void QueueTracks(List<Track> queueTracks)
        {
            ShufflerApplication.Playlist.Add(queueTracks);
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

                ShufflerApplication.Playlist.Open(playlistName);

            }
            catch (Exception e)
            {
                HandleException(e);
            }
            Cursor = Cursors.Default;
            Application.DoEvents();
        }

        /// <summary>
        ///     Gets the next track.
        /// </summary>
        /// <returns>The next track.</returns>
        public Track GetCurrentTrack()
        {
            return ShufflerApplication.Playlist.GetCurrentTrack();
        }



        /// <summary>
        ///     Gets the next track.
        /// </summary>
        /// <returns>The next track.</returns>
        public Track GetNextTrack()
        {
            return ShufflerApplication.Playlist.GetNextTrack();
        }

        /// <summary>
        ///     Gets the previous track.
        /// </summary>
        /// <returns>The previous track.</returns>
        public Track GetPreviousTrack()
        {
            return ShufflerApplication.Playlist.GetPreviousTrack();
        }

        /// <summary>
        ///     Gets the selected track from the grid.
        /// </summary>
        /// <returns>The selected track</returns>
        public Track GetSelectedTrack()
        {
            var tracks = GetSelectedTracks();
            return tracks.Count == 0 ? null : ShufflerApplication.Playlist.Tracks[0];
        }

        /// <summary>
        ///     Gets the tracks in the play-list
        /// </summary>
        /// <returns>The tracks in the play-list</returns>
        public List<Track> GetTracks()
        {
            return ShufflerApplication.Playlist.Tracks;
        }

        /// <summary>
        ///     Plays the next track.
        /// </summary>
        public void PlayNextTrack()
        {
            ShufflerApplication.Playlist.PlayNext();
        }

        /// <summary>
        ///     Plays the previous track.
        /// </summary>
        public void PlayPreviousTrack()
        {
            ShufflerApplication.Playlist.PlayPrevious();
        }

        /// <summary>
        ///     Binds the data for the user control to the controls
        /// </summary>
        private void BindData()
        {
            if (_doNotBind) return;
            _binding = true;

            grdPlaylist.SaveSelectedRows();

            var trackCount = ShufflerApplication.Playlist.Tracks.Count;

            if (grdPlaylist.RowCount != trackCount)
                grdPlaylist.RowCount = trackCount;

            grdPlaylist.RestoreSelectedRows();

            grdPlaylist.InvalidateDisplayedRows();

            SetToolStripLabel();
            lblCount.Text = $@"{trackCount} tracks";

            _binding = false;

            ShowCurrentTrackDetails();
        }

        /// <summary>
        ///     Gets the selected tracks from the grid.
        /// </summary>
        /// <returns>The selected tracks</returns>
        private List<Track> GetSelectedTracks()
        {
            var tracks = new List<Track>();
            for (var i = 0; i < grdPlaylist.Rows.Count; i++)
            {
                var row = grdPlaylist.Rows[i];
                if (row.Selected && row.Index < ShufflerApplication.Playlist.Tracks.Count && row.Index >= 0)
                    tracks.Add(ShufflerApplication.Playlist.Tracks[row.Index]);
            }
            return tracks;
        }


        /// <summary>
        ///     Saves the play-list.
        /// </summary>
        /// <param name="playlistFile">The play-list file.</param>
        public void SavePlaylist(string playlistFile)
        {
            try
            {
                CollectionHelper.ExportPlaylist(playlistFile, GetTracks());
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
                return GetCurrentTrackIndexFromBassPlayer();

            var currentBassTrackDescription = BassPlayer.CurrentTrack == null
                ? ""
                : BassPlayer.CurrentTrack.Description;

            if (currentBassTrackDescription == currentTrack.Description)
                return TrackModels.IndexOf(currentTrack);

            var currentIndex = TrackModels.IndexOf(currentTrack);
            var match = TrackModels
                .FirstOrDefault(t => t.Description == currentBassTrackDescription &&
                                     TrackModels.IndexOf(t) >= currentIndex);

            if (match == null)
                return GetCurrentTrackIndexFromBassPlayer();

            currentTrack.IsCurrent = false;
            match.IsCurrent = true;

            return TrackModels.IndexOf(match);
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
            _binding = true;
            grdPlaylist.ClearSelectedRows();
            _binding = false;
            SetNextBassPlayerTrack();
            BindData();

            PlaylistChanged?.Invoke(this, EventArgs.Empty);
            SaveWorkingPlaylist();
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
            Library.LoadTrack(GetSelectedTrack().Filename);
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
            SaveWorkingPlaylist();
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
            //var currentTrack = GetLibraryTrack(SetCurrentTrack(e.RowIndex));

            //var forcePlay = new ForcePlayTrackHandler(ForcePlayTrack);
            //BeginInvoke(forcePlay, currentTrack);
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
            var singleTrack = GetSelectedTracks().Count == 1;
            mnuPlay.Visible = singleTrack;
            mnuSep1.Visible = singleTrack;
            mnuOpenFileLocation.Visible = singleTrack;
            mnuSep3.Visible = singleTrack;
            mnuUpdateTrackDetails.Visible = singleTrack;
            mnuUpdateShufflerDetails.Visible = singleTrack;
            mnuRemoveShufflerDetails.Visible = singleTrack;
            mnuSep4.Visible = singleTrack;
            mnuMixRank.Visible = grdPlaylist.SelectedRows.Count > 0 && grdPlaylist.SelectedRows[0].Index > 0;

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
                currentTrackRank = GetSelectedTrack().Rank;
            for (var i = 0; i < 6; i++)
            {
                mnuTrackRank.DropDownItems[i].Text = MixLibrary.GetRankDescription(5 - i);
                ((ToolStripMenuItem) mnuTrackRank.DropDownItems[i]).Checked = 5 - i == currentTrackRank;
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
                ((ToolStripMenuItem) mnuMixRank.DropDownItems[i]).Checked = 5 - i == currentMixRank;
            }
        }

        private void BindAddTrackToPlaylistMenu()
        {
            var selectedTracks = GetSelectedLibraryTracks();
            var playlists = CollectionHelper.GetCollectionsTracksArentIn(selectedTracks);

            // generate 'add to playlist' sub menu
            mnuAddTrackToCollection.DropDownItems.Clear();
            foreach (var playlist in playlists)
                mnuAddTrackToCollection.DropDownItems.Add(playlist, null, mnuAddTrackToPlaylist_Click);
            mnuAddTrackToCollection.DropDownItems.Add("(New Playlist)", null, mnuAddNewPlaylist_Click);
            mnuAddTrackToCollection.Visible = mnuAddTrackToCollection.DropDownItems.Count > 0;
        }

        private void BindRemoveTrackFromPlaylistMenu()
        {
            // generate 'remove from playlist' sub menu
            mnuRemoveTrackFromCollection.DropDownItems.Clear();
            var selectedPlaylists = CollectionHelper.GetCollectionsForTracks(GetSelectedLibraryTracks());
            foreach (var playlist in selectedPlaylists)
                mnuRemoveTrackFromCollection.DropDownItems.Add(playlist, null, mnuRemoveTrackFromPlaylist_Click);
            mnuRemoveTrackFromCollection.Visible = mnuRemoveTrackFromCollection.DropDownItems.Count > 0;
        }

        /// <summary>
        ///     Handles the Click event of the mnuAddTrackToPlaylist control.
        /// </summary>
        private void mnuAddTrackToPlaylist_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            if (menu == null) return;

            var playlist = menu.Text;
            CollectionHelper.AddTracksToCollection(playlist, GetSelectedLibraryTracks());
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

            var playlist = menu.Text;
            CollectionHelper.RemoveTracksFromCollection(playlist, GetSelectedLibraryTracks());
        }

        /// <summary>
        ///     Handles the Click event of the mnuPlay control.
        /// </summary>
        private void mnuPlay_Click(object sender, EventArgs e)
        {
            var track = GetSelectedTrack();
            BassPlayer.ForcePlay(track.Filename);
        }
    }
}