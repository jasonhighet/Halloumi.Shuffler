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

        private bool _bassPlayerOnTrackChange;


        private bool _binding;

        private bool _doNotBind;

        private bool _loaded;
        //public EventHandler PlaylistChanged;

        private ShufflerApplication _application;

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
            //Tracks = new List<Track>();
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

        ///// <summary>
        /////     Gets or sets the library.
        ///// </summary>
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public MixLibrary MixLibrary { get; set; }

        ///// <summary>
        /////     Gets or sets the library.
        ///// </summary>
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public Library Library { get; set; }

        ///// <summary>
        /////     Gets or sets the bass player.
        ///// </summary>
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public BassPlayer BassPlayer { get; set; }

        ///// <summary>
        /////     Gets the tracks.
        ///// </summary>
        //private List<Track> Tracks { get; set; }

        private void mnuRemoveShufflerDetails_Click(object sender, EventArgs e)
        {
            var track = GetSelectedTrack();
            if (track == null) return;

            var message = $"Are you sure you wish to remove the shuffler details for '{track.Description}'?";
            if (!MessageBoxHelper.Confirm(message)) return;
            _application.Library.RemoveShufflerDetails(track);
            BindData();
        }

        private void SetToolStripLabel()
        {
            if (ToolStripLabel == null) return;

            var trackCount = _application.Playlist.Tracks.Count;
            var playlistLength = TimeFormatHelper.GetFormattedHours(_application.Playlist.Tracks.Sum(t => t.Length));
            var text = $"{trackCount} tracks in playlist ({playlistLength})";

            ToolStripLabel.Text = text;
        }

        //private void SetMixAndKeyRanks()
        //{
        //    //for (var i = 0; i < Tracks.Count; i++)
        //    //    UpdateMixRank(i);
        //    Parallel.For(0, Tracks.Count, UpdateMixRank);
        //}

        //private void UpdateMixRank(int rowIndex)
        //{
        //    if (rowIndex == 0)
        //    {
        //        Tracks[0].MixRankDescription = "";
        //        return;
        //    }

        //    var track1 = _application.Playlist.GetTrack(rowIndex - 1);
        //    var track2 = _application.Playlist.GetTrack(rowIndex);

        //    if (track1 == null || track2 == null)
        //        return;

        //    Tracks[rowIndex].MixRankDescription = _application.MixLibrary.GetExtendedMixDescription(track1, track2);
        //    Tracks[rowIndex].KeyRankDescription = KeyHelper.GetKeyMixRankDescription(track1.Key, track2.Key);
        //}

        private void mnuTrackRank_Click(object sender, EventArgs e)
        {
            var toolStripDropDownItem = sender as ToolStripDropDownItem;
            if (toolStripDropDownItem == null)
                return;

            var trackRankDescription = toolStripDropDownItem.Text;
            var trackRank = _application.MixLibrary.GetRankFromDescription(trackRankDescription);

            var tracks = GetSelectedTracks();
            _application.Library.SetRank(tracks, (int) trackRank);

            //foreach (var track in tracks)
            //{
            //    var track = Tracks.FirstOrDefault(t => t.Description == track.Description);
            //    if (track == null) continue;
            //    track.TrackRankDescription = trackRankDescription;
            //}

            //BindData();
            grdPlaylist.InvalidateDisplayedRows();
        }

        private void mnuMixRank_Click(object sender, EventArgs e)
        {
            var toolStripDropDownItem = sender as ToolStripDropDownItem;
            if (toolStripDropDownItem == null) return;

            var mixRankDescription = toolStripDropDownItem.Text;
            var mixRank = _application.MixLibrary.GetRankFromDescription(mixRankDescription);

            foreach (DataGridViewRow row in grdPlaylist.SelectedRows)
            {
                if (row.Index == 0) continue;
                var track2 = _application.Playlist.GetTrack(row.Index);
                var track1 = _application.Playlist.GetTrack(row.Index - 1);
                _application.MixLibrary.SetMixLevel(track1, track2, (int) mixRank);

                //var track = _application.Playlist.GetTrack(row.Index);
                //track.MixRankDescription = mixRankDescription;
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

            if (e.RowIndex == _application.Playlist.CurrentTrackIndex)
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
            var track = _application.Playlist.GetTrack(e.RowIndex);

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
                //UpdateMixRank(e.RowIndex);
                //e.Value = track.MixRankDescription;
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
                //e.Value = track.KeyRankDescription;
            }
        }

        public void QueueFiles(List<string> files)
        {
            var tracks = files.Select(file => _application.Library.GetTrackByFilename(file)).Where(track => track != null).ToList();

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
            //var previousTrack = GetPreviousTrack();
            //if (previousTrack == null) return;

            //var index = _application.Playlist.CurrentTrackIndex - 1;
            //SetCurrentTrack(index);

            //_doNotBind = true;
            //_application.BassPlayer.ForcePlay(previousTrack.Filename);
            //_application.BassPlayer.SkipToFadeOut();
            //_application.BassPlayer.Play();

            //_doNotBind = false;

            ////BindData();
            //grdPlaylist.InvalidateDisplayedRows();
        }

        public int GetNumberOfTracksRemaining()
        {
            return _application.Playlist.GetNumberOfTracksRemaining();
        }

        public void Initalize(TrackLibraryControl trackLibraryControl, ShufflerApplication application)
        {
            _application = application;

            trackDetails.Library = application.Library;
            trackDetails.DisplayTrackDetails(null);

            mixableTracks.PlaylistControl = this;
            mixableTracks.Initialize(application.MixLibrary, trackLibraryControl);

            //if (BassPlayer == null) return;
            //_application.BassPlayer.OnTrackChange += BassPlayer_OnTrackChange;
            //_application.BassPlayer.OnSkipToEnd += BassPlayer_OnFadeEnded;
            //_application.BassPlayer.OnEndFadeIn += BassPlayer_OnFadeEnded;
        }

        ///// <summary>
        /////     Handles the OnEndFadeIn event of the BassPlayer control.
        ///// </summary>
        //private void BassPlayer_OnFadeEnded(object sender, EventArgs e)
        //{
        //    if (InvokeRequired)
        //        BeginInvoke(new MethodInvoker(PreloadTrack));
        //    else PreloadTrack();
        //}

        ///// <summary>
        /////     Handles the OnEndFadeIn event of the BassPlayer control.
        ///// </summary>
        //private void PreloadTrack()
        //{
        //    var preloadTrack = GetTrackAfterNext();
        //    if (preloadTrack != null)
        //        _application.BassPlayer.PreloadTrack(preloadTrack.Filename);
        //}

        /// <summary>
        ///     Queues tracks.
        /// </summary>
        /// <param name="queueTracks">The queue tracks.</param>
        public void QueueTracks(List<Track> queueTracks)
        {
            _application.Playlist.Add(queueTracks);
            //Tracks.AddRange(Track.ToList(queueTracks, MixLibrary));


            //if (_application.BassPlayer.CurrentTrack == null && Tracks.Count > 0)
            //{
            //    var track = _application.Playlist.GetTrack(0);
            //    _application.BassPlayer.QueueTrack(track.Filename);
            //}
            //SetNextBassPlayerTrack();
            //PreloadTrack();

            //if (InvokeRequired)
            //    BeginInvoke(new MethodInvoker(BindData));
            //else BindData();

            //PlaylistChanged?.Invoke(this, EventArgs.Empty);
            //Task.Run(() => SaveWorkingPlaylist());
        }

        //private Track GetLibraryTrack(Track track)
        //{
        //    var track = _application.Library.GetTrackByFilename(track.Filename);
        //    if (track == null || !File.Exists(track.Filename))
        //        track = Library
        //            .GetTracksByDescription(track.Description)
        //            .FirstOrDefault(t => File.Exists(t.Filename));

        //    return track;
        //}


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

                _application.Playlist.Open(playlistName);

                //CurrentPlaylistFile = playlistName;
                //QueueTracks(CollectionHelper.GetTracksInPlaylistFile(playlistName));
            }
            catch (Exception e)
            {
                HandleException(e);
            }
            Cursor = Cursors.Default;
            Application.DoEvents();
        }

        /// <summary>
        ///     Gets the selected track from the grid.
        /// </summary>
        /// <returns>The selected track</returns>
        public Track GetSelectedTrack()
        {
            var tracks = GetSelectedTracks();
            return tracks.Count == 0 ? null : tracks[0];
        }

        /// <summary>
        ///     Gets the tracks in the play-list
        /// </summary>
        /// <returns>The tracks in the play-list</returns>
        public List<Track> GetTracks()
        {
            return _application.Playlist.Tracks;
        }

        /// <summary>
        ///     Plays the next track.
        /// </summary>
        public void PlayNextTrack()
        {
            _application.Playlist.PlayNext();
        }

        /// <summary>
        ///     Plays the previous track.
        /// </summary>
        public void PlayPreviousTrack()
        {
            _application.Playlist.PlayPrevious();
        }

        /// <summary>
        ///     Binds the data for the user control to the controls
        /// </summary>
        private void BindData()
        {
            if (_doNotBind) return;
            _binding = true;

           // SetMixAndKeyRanks();

            grdPlaylist.SaveSelectedRows();

            var trackCount = _application.Playlist.Tracks.Count;

            if (grdPlaylist.RowCount != trackCount)
                grdPlaylist.RowCount = trackCount;

            grdPlaylist.RestoreSelectedRows();

            grdPlaylist.InvalidateDisplayedRows();

            SetToolStripLabel();
            lblCount.Text = $@"{trackCount} tracks";

            _binding = false;

            ShowCurrentTrackDetails();
        }

        ///// <summary>
        /////     Queues the next track.
        ///// </summary>
        //private void SetNextBassPlayerTrack()
        //{
        //    var nextTrack = GetNextTrack();

        //    if (nextTrack == null) return;
        //    if (_application.BassPlayer.NextTrack == null
        //        ||
        //        _application.BassPlayer.NextTrack != null && _application.BassPlayer.NextTrack.Description != nextTrack.Description)
        //        _application.BassPlayer.QueueTrack(nextTrack.Filename);
        //}

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
                if (row.Selected && row.Index < _application.Playlist.Tracks.Count && row.Index >= 0)
                    tracks.Add(_application.Playlist.Tracks[row.Index]);
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
                _application.Playlist.Save(playlistFile);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        ///// <summary>
        /////     Gets the index of the current track.
        ///// </summary>
        ///// <returns>The index of the current track.</returns>
        //public int _application.Playlist.CurrentTrackIndex
        //{
        //    var currentTrack = Tracks.FirstOrDefault(t => t.IsCurrent);
        //    if (currentTrack == null)
        //        return GetCurrentTrackIndexFromBassPlayer();

        //    var currentBassTrackDescription = _application.BassPlayer.CurrentTrack == null
        //        ? ""
        //        : _application.BassPlayer.CurrentTrack.Description;

        //    if (currentBassTrackDescription == currentTrack.Description)
        //        return Tracks.IndexOf(currentTrack);

        //    var currentIndex = Tracks.IndexOf(currentTrack);
        //    var match = Tracks
        //        .FirstOrDefault(t => t.Description == currentBassTrackDescription &&
        //                             Tracks.IndexOf(t) >= currentIndex);

        //    if (match == null)
        //        return GetCurrentTrackIndexFromBassPlayer();

        //    currentTrack.IsCurrent = false;
        //    match.IsCurrent = true;

        //    return Tracks.IndexOf(match);
        //}

        //private int GetCurrentTrackIndexFromBassPlayer()
        //{
        //    if (_application.BassPlayer.CurrentTrack == null)
        //    {
        //        if (Tracks.Count <= 0) return -1;
        //        Tracks[0].IsCurrent = true;
        //        return 0;
        //    }
        //    var currentTrack = Tracks.FirstOrDefault(t => t.Description == _application.BassPlayer.CurrentTrack.Description);
        //    if (currentTrack == null) return -1;
        //    currentTrack.IsCurrent = true;
        //    return Tracks.IndexOf(currentTrack);
        //}

        /// <summary>
        ///     Removes the specified tracks.
        /// </summary>
        /// <param name="tracksToRemove">The tracks to remove.</param>
        private void RemoveTracks(IReadOnlyCollection<Track> tracksToRemove)
        {
            //if (tracksToRemove.Count == 0) return;
            //var tracks = Tracks.Except(tracksToRemove).ToList();
            //Tracks = tracks;
            //_binding = true;
            //grdPlaylist.ClearSelectedRows();
            //_binding = false;
            //SetNextBassPlayerTrack();
            //BindData();

            //PlaylistChanged?.Invoke(this, EventArgs.Empty);
            //SaveWorkingPlaylist();
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
                BassPlayer = _application.BassPlayer,
                Filename = GetSelectedTrack().Filename
            };

            var result = form.ShowDialog();
            if (result != DialogResult.OK) return;
            _application.Library.LoadTrack(GetSelectedTrack().Filename);
            _application.BassPlayer.ReloadTrack(GetSelectedTrack().Filename);
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
                Library = _application.Library,
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
            if (_application.Playlist.Tracks.Count == 0) return;
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

        ///// <summary>
        /////     Handles the OnTrackChange event of the BassPlayer control.
        ///// </summary>
        //private void BassPlayer_OnTrackChange(object sender, EventArgs e)
        //{
        //    if (InvokeRequired)
        //        BeginInvoke(new MethodInvoker(BassPlayer_OnTrackChange));
        //    else BassPlayer_OnTrackChange();
        //}

        ///// <summary>
        /////     Handles the OnTrackChange event of the BassPlayer control.
        ///// </summary>
        //private void BassPlayer_OnTrackChange()
        //{
        //    if (_bassPlayerOnTrackChange) return;
        //    _bassPlayerOnTrackChange = true;

        //    grdPlaylist.Invalidate();

        //    var currentTrack = GetCurrentTrack();
        //    var currentTrackDescription = currentTrack == null ? "" : currentTrack.Description;

        //    var nextTrack = GetNextTrack();
        //    var nextTrackDescription = nextTrack == null ? "" : nextTrack.Description;

        //    var currentBassTrackDescription = _application.BassPlayer.CurrentTrack == null
        //        ? ""
        //        : _application.BassPlayer.CurrentTrack.Description;
        //    var nextBassTrackDescription = _application.BassPlayer.NextTrack == null ? "" : _application.BassPlayer.NextTrack.Description;

        //    if (currentBassTrackDescription != currentTrackDescription ||
        //        nextBassTrackDescription != nextTrackDescription)
        //    {
        //        for (var i = 0; i < 10; i++)
        //        {
        //            Application.DoEvents();
        //            Thread.Sleep(20);
        //        }

        //        SetNextBassPlayerTrack();
        //        //BindData();
        //    }

        //    _bassPlayerOnTrackChange = false;
        //}

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
            //CurrentPlaylistFile = "";
            //var tracks = new List<Track>();
            //Tracks = tracks;
            //BindData();
            //PlaylistChanged?.Invoke(this, EventArgs.Empty);
            //SaveWorkingPlaylist();

            _application.Playlist.Clear();
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
            _application.Playlist.Play(e.RowIndex);
        }

        private void ForcePlayTrack(Track track)
        {
            _doNotBind = true;

            _application.BassPlayer.ForcePlay(track.Filename);

            _doNotBind = false;

            grdPlaylist.InvalidateDisplayedRows();
        }

        //private Track SetCurrentTrack(int trackIndex)
        //{
        //    Tracks.ForEach(t => t.IsCurrent = false);
        //    Tracks[trackIndex].IsCurrent = true;
        //    return Tracks[trackIndex];
        //}

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
                mnuTrackRank.DropDownItems[i].Text = _application.MixLibrary.GetRankDescription(5 - i);
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
                var track1 = _application.Playlist.GetTrack(grdPlaylist.SelectedRows[0].Index - 1);
                var mixRank = _application.MixLibrary.GetMixLevel(track1, track2);
                currentMixRank = mixRank;
            }
            for (var i = 0; i < 6; i++)
            {
                mnuMixRank.DropDownItems[i].Text = _application.MixLibrary.GetRankDescription(5 - i);
                ((ToolStripMenuItem) mnuMixRank.DropDownItems[i]).Checked = 5 - i == currentMixRank;
            }
        }

        private void BindAddTrackToPlaylistMenu()
        {
            var selectedTracks = GetSelectedTracks();
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
            var selectedPlaylists = CollectionHelper.GetCollectionsForTracks(GetSelectedTracks());
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
            CollectionHelper.AddTracksToCollection(playlist, GetSelectedTracks());
        }

        /// <summary>
        ///     Handles the Click event of the mnuAddNewPlaylist control.
        /// </summary>
        private void mnuAddNewPlaylist_Click(object sender, EventArgs e)
        {
            var tracks = GetSelectedTracks();
            var form = new FrmAddPlaylist
            {
                Library = _application.Library,
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
            CollectionHelper.RemoveTracksFromCollection(playlist, GetSelectedTracks());
        }

        /// <summary>
        ///     Handles the Click event of the mnuPlay control.
        /// </summary>
        private void mnuPlay_Click(object sender, EventArgs e)
        {
            var track = GetSelectedTrack();
            _application.BassPlayer.ForcePlay(track.Filename);
        }

        //private void SaveWorkingPlaylist()
        //{
        //    var playlistFiles = Tracks.Select(x => x.Description).ToList();
        //    SerializationHelper<List<string>>.ToXmlFile(playlistFiles, WorkingPlaylistFilename);
        //}

        //public void LoadWorkingPlaylist()
        //{
        //    if (!File.Exists(WorkingPlaylistFilename))
        //        return;

        //    var playlistFiles = SerializationHelper<List<string>>
        //        .FromXmlFile(WorkingPlaylistFilename)
        //        .Select(x => _application.Library.GetTrackByDescription(x))
        //        .Where(x => x != null)
        //        .Select(x => x.Filename)
        //        .ToList();

        //    Tracks = new List<Track>();
        //    QueueFiles(playlistFiles);
        //}

        //private class Track
        //{
        //    private Track(Track track, MixLibrary mixLibrary)
        //    {
        //        Description = track.Description;
        //        Filename = track.Filename;
        //        LengthFormatted = track.LengthFormatted;
        //        IsCurrent = false;
        //        Bpm = track.Bpm;
        //        Length = track.Length;
        //        TrackRankDescription = mixLibrary.GetRankDescription(track.Rank);
        //        Key = KeyHelper.GetDisplayKey(track.Key);
        //    }

        //    public string Description { get; }

        //    public string Filename { get; }

        //    public string LengthFormatted { get; }

        //    public decimal Bpm { get; }

        //    public decimal Length { get; }

        //    public bool IsCurrent { get; set; }

        //    public string MixRankDescription { get; set; }

        //    public string TrackRankDescription { get; set; }

        //    public string Key { get; }

        //    public string KeyRankDescription { get; set; }

        //    public static IEnumerable<Track> ToList(IEnumerable<Track> tracks, MixLibrary mixLibrary)
        //    {
        //        return tracks.Select(t => new Track(t, mixLibrary)).ToList();
        //    }
        //}

        //private delegate void ForcePlayTrackHandler(Track track);
    }
}