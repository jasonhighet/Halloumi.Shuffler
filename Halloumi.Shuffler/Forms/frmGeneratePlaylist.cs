using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;
using Halloumi.Shuffler.Controls;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmGeneratePlaylist : BaseForm
    {
        public enum ScreenMode
        {
            GeneratePlaylist,
            AutoGeneratePlaylist,
            AutoGenerateSettings
        }

        private bool _cancel;
        private List<Track> _displayedTracks;
        private List<Track> _allShufflerTracks;
        private List<Track> _currentPlaylist;
        private PlaylistGenerationRequest _pendingRequest;

        private ScreenMode _screenMode;

        /// <summary>
        ///     Initializes a new instance of the frmGeneratePlaylist class.
        /// </summary>
        public FrmGeneratePlaylist()
        {
            InitializeComponent();
            AddFavourRecentTracksRow();

            Load += frmGeneratePlaylist_Load;

            InitialiseControls();
        }

        private Halloumi.Common.Windows.Controls.CheckBox chkFavourRecentTracks;

        /// <summary>
        ///     Adds the "Recent Tracks" row under "Displayed Tracks", moving the rows below it down.
        /// </summary>
        private void AddFavourRecentTracksRow()
        {
            const int row = 13;
            const int rowHeight = 31;

            tblMain.SuspendLayout();

            foreach (Control control in tblMain.Controls)
            {
                var position = tblMain.GetCellPosition(control);
                if (position.Row >= row) tblMain.SetRow(control, position.Row + 1);
            }

            tblMain.RowCount++;
            // a share of the height like the other rows, so it grows with display scaling as they do
            tblMain.RowStyles.Insert(row, new RowStyle(SizeType.Percent, tblMain.RowStyles[row].Height));

            var label = new Halloumi.Common.Windows.Controls.Label
            {
                Font = label10.Font,
                ForeColor = label10.ForeColor,
                Margin = label10.Margin,
                Padding = label10.Padding,
                Size = label10.Size,
                Style = label10.Style,
                Text = "Recent Tracks:"
            };

            chkFavourRecentTracks = new Halloumi.Common.Windows.Controls.CheckBox
            {
                Margin = chkRestrictTitleClumping.Margin,
                Name = "chkFavourRecentTracks",
                Size = chkRestrictTitleClumping.Size
            };
            chkFavourRecentTracks.Values.Text = "Favour recently shuffled tracks";

            tblMain.Controls.Add(label, 0, row);
            tblMain.Controls.Add(chkFavourRecentTracks, 1, row);
            tblMain.SetColumnSpan(chkFavourRecentTracks, 3);

            tblMain.ResumeLayout();
            ClientSize = new System.Drawing.Size(ClientSize.Width, ClientSize.Height + rowHeight);
        }

        /// <summary>
        ///     Gets or sets the shuffler application.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ShufflerApplication Application { get; set; }

        /// <summary>
        ///     Gets or sets the library control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TrackLibraryControl LibraryControl { get; set; }

        /// <summary>
        ///     Gets or sets the library control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PlaylistControl PlaylistControl { get; set; }

        private void frmGeneratePlaylist_Load(object sender, EventArgs e)
        {
            LoadSettings();
            if (_screenMode == ScreenMode.AutoGeneratePlaylist)
            {
                Opacity = 0;
                StartGeneratingPlaylist();
            }
        }

        /// <summary>
        ///     Initialises the controls.
        /// </summary>
        private void InitialiseControls()
        {
            cmbDirection.Items.Add("Forwards");
            cmbDirection.Items.Add("Backwards");
            cmbDirection.SelectedIndex = 0;

            cmbBpmDirection.Items.Add("Cycle");
            cmbBpmDirection.Items.Add("Any");
            cmbBpmDirection.Items.Add("Up");
            cmbBpmDirection.Items.Add("Down");
            cmbBpmDirection.SelectedIndex = 0;

            cmbAllowBearable.Items.Add("After Two Good Tracks");
            cmbAllowBearable.Items.Add("After Each Good Track");
            cmbAllowBearable.Items.Add("After Power Down");
            cmbAllowBearable.Items.Add("When Necessary");
            cmbAllowBearable.Items.Add("Never");
            cmbAllowBearable.Items.Add("Always");
            cmbAllowBearable.SelectedIndex = 0;

            cmbMode.Items.Add("Best Mix");
            cmbMode.Items.Add("Variety");
            cmbMode.Items.Add("Extra Variety");
            cmbMode.Items.Add("Unranked");
            cmbMode.Items.Add("Working");
            cmbMode.SelectedIndex = 0;

            cmbKeyMixing.Items.Add("Very Good");
            cmbKeyMixing.Items.Add("Good");
            cmbKeyMixing.Items.Add("Good If Possible");
            cmbKeyMixing.Items.Add("Bearable");
            cmbKeyMixing.Items.Add("Any");
            cmbKeyMixing.SelectedIndex = 0;

            cmbContinueMix.Items.Add("Yes");
            cmbContinueMix.Items.Add("No");
            cmbContinueMix.Items.Add("If Possible");
            cmbContinueMix.SelectedIndex = 0;

            PopulateApproxLength();

            for (var i = 0; i < 10; i++)
                cmbTracksToGenerate.Items.Add((i + 1)*5);
            cmbTracksToGenerate.SelectedIndex = 2;

            EnableControls();
        }

        private void PopulateApproxLength()
        {
            cmbApproxLength.Items.Add("No limit");

            var i = 10;
            while (i <= 24*60)
            {
                cmbApproxLength.Items.Add(i + " minutes");

                if (i < 30)
                    i += 10;
                else if (i < 90)
                    i += 5;
                else
                    i += 30;
            }
            cmbApproxLength.SelectedIndex = 0;
        }

        public void SetScreenMode(ScreenMode screenMode)
        {
            _screenMode = screenMode;

            if (screenMode == ScreenMode.GeneratePlaylist)
            {
                tblMain.RowStyles[0].SizeType = SizeType.Absolute;
                tblMain.RowStyles[0].Height = 0;
                btnOK.Visible = false;
            }
            else if (screenMode == ScreenMode.AutoGenerateSettings)
            {
                tblMain.RowStyles[2].SizeType = SizeType.Absolute;
                tblMain.RowStyles[2].Height = 0;
                btnStart.Visible = false;
                btnStop.Visible = false;
            }
        }

        /// <summary>
        ///     Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            if (Application == null) return;
            try
            {
                var request = Application.LoadPlaylistGenerationSettings();
                SetComboFromEnum(cmbBpmDirection, request.BpmDirection);
                SetComboFromEnum(cmbDirection, request.Direction);
                SetComboFromEnum(cmbAllowBearable, request.AllowBearable);
                SetComboFromEnum(cmbMode, request.Strategy);
                SetComboFromEnum(cmbKeyMixing, request.KeyMixStrategy);
                SetComboFromEnum(cmbContinueMix, request.ContinueMix);
                SetComboFromEnum(cmbExtendedMixes, request.UseExtendedMixes);
                txtExcludeTracks.Text = request.ExcludeFromPlaylistFile;
                chkExcludeMixesOnly.Checked = request.ExcludeMixesOnly;
                chkRestrictArtistClumping.Checked = request.RestrictArtistClumping;
                chkRestrictGenreClumping.Checked = request.RestrictGenreClumping;
                chkRestrictTitleClumping.Checked = request.RestrictTitleClumping;
                chkDisplayedTracksOnly.Checked = request.DisplayedTracksOnly;
                chkFavourRecentTracks.Checked = request.FavourRecentTracks;
                SetApproxLengthCombo(request.ApproximateLengthMinutes);
                SetTracksToGenerateCombo(request.MaxTracksToAdd);
            }
            catch
            {
                // ignored
            }
        }

        private static void SetComboFromEnum<T>(Halloumi.Common.Windows.Controls.ComboBox combo, T value)
        {
            var text = Regex.Replace(value.ToString(), "([A-Z])", " $1").Trim();
            SetComboText(combo, text);
        }

        private static void SetComboText(Halloumi.Common.Windows.Controls.ComboBox combo, string text)
        {
            for (var i = 0; i < combo.Items.Count; i++)
            {
                if (combo.Items[i].ToString() == text)
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
        }

        private void SetApproxLengthCombo(int minutes)
        {
            if (minutes == int.MaxValue)
            {
                cmbApproxLength.SelectedIndex = 0;
                return;
            }
            SetComboText(cmbApproxLength, minutes + " minutes");
        }

        private void SetTracksToGenerateCombo(int maxTracks)
        {
            if (maxTracks == int.MaxValue)
            {
                cmbTracksToGenerate.SelectedIndex = 0;
                return;
            }
            SetComboText(cmbTracksToGenerate, maxTracks.ToString());
        }

        /// <summary>
        ///     Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            if (Application == null) return;
            Application.SavePlaylistGenerationSettings(BuildRequestFromUi());
        }

        /// <summary>
        ///     Builds a PlaylistGenerationRequest from current UI state (call on UI thread only).
        /// </summary>
        private PlaylistGenerationRequest BuildRequestFromUi()
        {
            return new PlaylistGenerationRequest
            {
                Strategy = ParseEnum<TrackSelector.MixStrategy>(cmbMode.Text),
                BpmDirection = ParseEnum<TrackSelector.BpmDirection>(cmbBpmDirection.Text),
                Direction = ParseEnum<TrackSelector.GenerateDirection>(cmbDirection.Text),
                AllowBearable = ParseEnum<TrackSelector.AllowBearableMixStrategy>(cmbAllowBearable.Text),
                ContinueMix = ParseEnum<TrackSelector.ContinueMix>(cmbContinueMix.Text),
                KeyMixStrategy = ParseEnum<TrackSelector.KeyMixStrategy>(cmbKeyMixing.Text),
                UseExtendedMixes = ParseEnum<TrackSelector.UseExtendedMixes>(cmbExtendedMixes.Text),
                RestrictArtistClumping = chkRestrictArtistClumping.Checked,
                RestrictGenreClumping = chkRestrictGenreClumping.Checked,
                RestrictTitleClumping = chkRestrictTitleClumping.Checked,
                ExcludeFromPlaylistFile = txtExcludeTracks.Text,
                ExcludeMixesOnly = chkExcludeMixesOnly.Checked,
                DisplayedTracksOnly = chkDisplayedTracksOnly.Checked,
                FavourRecentTracks = chkFavourRecentTracks.Checked,
                ApproximateLengthMinutes = ParseApproxLength(),
                MaxTracksToAdd = ParseTracksToAdd()
            };
        }

        private static T ParseEnum<T>(string text) where T : struct
        {
            return (T)Enum.Parse(typeof(T), text.Replace(" ", ""));
        }

        private int ParseApproxLength()
        {
            var text = cmbApproxLength.Text.Replace(" minutes", "");
            return text == "No limit" ? int.MaxValue : int.Parse(text);
        }

        private int ParseTracksToAdd()
        {
            int n;
            return int.TryParse(cmbTracksToGenerate.Text, out n) ? n : int.MaxValue;
        }

        /// <summary>
        ///     Enables the controls.
        /// </summary>
        private void EnableControls()
        {
            chkRestrictArtistClumping.Enabled = true;
            chkRestrictGenreClumping.Enabled = true;
            chkRestrictTitleClumping.Enabled = true;
            chkDisplayedTracksOnly.Enabled = true;
            chkFavourRecentTracks.Enabled = true;
            cmbDirection.Enabled = true;
            cmbApproxLength.Enabled = true;
            cmbBpmDirection.Enabled = true;
            cmbAllowBearable.Enabled = true;
            cmbMode.Enabled = true;
            txtExcludeTracks.Enabled = true;
            btnExcludeTracks.Enabled = true;
            chkExcludeMixesOnly.Enabled = true;
            cmbExtendedMixes.Enabled = true;
            cmbContinueMix.Enabled = true;
            cmbKeyMixing.Enabled = true;

            lblStatus.Visible = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;

            EnableControlsByMode();
        }

        private void EnableControlsByMode()
        {
            var enabled = cmbMode.SelectedIndex == 0 || cmbMode.SelectedIndex == 1 || cmbMode.SelectedIndex == 2;

            chkRestrictArtistClumping.Enabled = enabled;
            chkRestrictGenreClumping.Enabled = enabled;
            chkRestrictTitleClumping.Enabled = enabled;
            chkFavourRecentTracks.Enabled = enabled;
            cmbBpmDirection.Enabled = enabled;
            cmbDirection.Enabled = enabled;
            cmbAllowBearable.Enabled = enabled;
            cmbExtendedMixes.Enabled = enabled;
            cmbContinueMix.Enabled = enabled;
        }

        /// <summary>
        ///     Disables the controls.
        /// </summary>
        private void DisableControls()
        {
            chkRestrictArtistClumping.Enabled = false;
            chkRestrictGenreClumping.Enabled = false;
            chkRestrictTitleClumping.Enabled = false;
            chkDisplayedTracksOnly.Enabled = false;
            chkFavourRecentTracks.Enabled = false;
            cmbDirection.Enabled = false;
            cmbApproxLength.Enabled = false;
            cmbBpmDirection.Enabled = false;
            cmbAllowBearable.Enabled = false;
            cmbMode.Enabled = false;
            txtExcludeTracks.Enabled = false;
            btnExcludeTracks.Enabled = false;
            chkExcludeMixesOnly.Enabled = false;
            cmbExtendedMixes.Enabled = false;
            cmbContinueMix.Enabled = false;
            cmbKeyMixing.Enabled = false;
            lblStatus.Visible = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

        /// <summary>
        ///     Queues the tracks.
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        private void QueueTracks(List<Track> tracks, TrackSelector.GenerateDirection direction)
        {
            if (tracks.Count == 0) return;

            if (direction == TrackSelector.GenerateDirection.Forwards)
            {
                var currentTrackCount = PlaylistControl.GetTracks().Count;
                var additionalTrackCount = tracks.Count - currentTrackCount;

                if (additionalTrackCount <= 0) return;

                tracks.Reverse();
                tracks = tracks.Take(additionalTrackCount).ToList();
                tracks.Reverse();

                PlaylistControl.QueueTracks(tracks);
            }
            else
            {
                this.PlaylistControl.ClearTracks();
                PlaylistControl.QueueTracks(tracks);
            }
        }

        /// <summary>
        ///     Handles the Click event of the btnStop control.
        /// </summary>
        private void btnStop_Click(object sender, EventArgs e)
        {
            Application.StopPlaylistGeneration();
        }

        /// <summary>
        ///     Handles the Click event of the btnCancel control.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                _cancel = true;
                Application.CancelPlaylistGeneration();
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        /// <summary>
        ///     Handles the DoWork event of the backgroundWorker control.
        /// </summary>
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var request = _pendingRequest;

            // Apply mode-specific overrides
            if (_screenMode != ScreenMode.GeneratePlaylist)
                request.ApproximateLengthMinutes = int.MaxValue;
            if (_screenMode != ScreenMode.AutoGeneratePlaylist)
                request.MaxTracksToAdd = int.MaxValue;

            var availableTracks = request.DisplayedTracksOnly ? _displayedTracks : _allShufflerTracks;
            var tracks = Application.GeneratePlaylist(request, availableTracks, _currentPlaylist);

            if (!_cancel)
            {
                if (InvokeRequired)
                    BeginInvoke(new MethodInvoker(() => QueueTracks(tracks, request.Direction)));
                else
                    QueueTracks(tracks, request.Direction);
            }
        }

        /// <summary>
        ///     Handles the Tick event of the timer control.
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            var status = Application.PlaylistGenerationStatus;
            if (status != lblStatus.Text && status != "")
                lblStatus.Text = status;
        }

        /// <summary>
        ///     Handles the RunWorkerCompleted event of the backgroundWorker control.
        /// </summary>
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer.Stop();
            EnableControls();
            Close();
        }

        /// <summary>
        ///     Handles the Click event of the btnStart control.
        /// </summary>
        private void btnStart_Click(object sender, EventArgs e)
        {
            StartGeneratingPlaylist();
        }

        public void StartGeneratingPlaylist()
        {
            DisableControls();

            _displayedTracks = LibraryControl
                .DisplayedTracks
                .Where(t => t.IsShufflerTrack)
                .ToList();

            _allShufflerTracks = LibraryControl
                .AvailableTracks
                .Where(t => t.IsShufflerTrack)
                .ToList();

            _currentPlaylist = PlaylistControl.GetTracks();
            _pendingRequest = BuildRequestFromUi();

            backgroundWorker.RunWorkerAsync();
            timer.Start();
        }

        /// <summary>
        ///     Handles the FormClosing event of the frmGeneratePlaylist control.
        /// </summary>
        private void frmGeneratePlaylist_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.Cancel)
                SaveSettings();
        }

        private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControlsByMode();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
