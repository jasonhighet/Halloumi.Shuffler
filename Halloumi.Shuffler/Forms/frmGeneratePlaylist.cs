using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;
using Halloumi.Shuffler.Controls;
using AE = Halloumi.Shuffler.AudioEngine;

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

        private ScreenMode _screenMode;

        /// <summary>
        ///     Initializes a new instance of the frmGeneratePlaylist class.
        /// </summary>
        public FrmGeneratePlaylist()
        {
            InitializeComponent();

            Load += frmGeneratePlaylist_Load;

            TrackSelector = new TrackSelector();

            InitialiseControls();
        }

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

        /// <summary>
        ///     Gets or sets the track selector.
        /// </summary>
        private TrackSelector TrackSelector { get; }

        private void frmGeneratePlaylist_Load(object sender, EventArgs e)
        {
            if (_screenMode != ScreenMode.AutoGeneratePlaylist) return;
            Opacity = 0;
            StartGeneratingPlaylist();
        }

        /// <summary>
        ///     Initialises the controls.
        /// </summary>
        private void InitialiseControls()
        {
            cmbDirection.Items.Add("Cycle");
            cmbDirection.Items.Add("Any");
            cmbDirection.Items.Add("Up");
            cmbDirection.Items.Add("Down");
            cmbDirection.SelectedIndex = 0;

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

            LoadSettings();
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
            try
            {
                var settings = GetSettings();

                cmbDirection.SelectedIndex = settings.CmbDirectionSelectedIndex;
                cmbAllowBearable.SelectedIndex = settings.CmbAllowBearableSelectedIndex;
                cmbApproxLength.SelectedIndex = settings.CmbApproxLengthSelectedIndex;
                cmbMode.SelectedIndex = settings.CmbModeSelectedIndex;
                txtExcludeTracks.Text = settings.TxtExcludeTracksText;
                cmbExtendedMixes.SelectedIndex = settings.CmbExtendedMixesSelectedIndex;
                chkExlcudeMixesOnly.Checked = settings.ChkExlcudeMixesOnlyChecked;
                chkRestrictArtistClumping.Checked = settings.ChkRestrictArtistClumpingChecked;
                chkRestrictGenreClumping.Checked = settings.ChkRestrictGenreClumpingChecked;
                chkRestrictTitleClumping.Checked = settings.ChkRestrictTitleClumpingChecked;
                chkDisplayedTracksOnly.Checked = settings.ChkDisplayedTracksOnlyChecked;
                cmbTracksToGenerate.SelectedIndex = settings.CmbTracksToGenerateSelectedIndex;
                cmbContinueMix.SelectedIndex = settings.CmbContinueMixSelectedIndex;
                cmbKeyMixing.SelectedIndex = settings.CmbKeyMixingSelectedIndex;
            }
            catch
            {
                // ignored
            }
        }

        private static Settings GetSettings()
        {
            var settings = new Settings();
            var filename = Path.Combine(Path.GetTempPath(), "Halloumi.Shuffler.frmGeneratePlaylist.xml");
            if (File.Exists(filename))
            {
                settings = SerializationHelper<Settings>.FromXmlFile(filename);
            }
            return settings;
        }

        /// <summary>
        ///     Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            var settings = new Settings
            {
                CmbDirectionSelectedIndex = cmbDirection.SelectedIndex,
                CmbAllowBearableSelectedIndex = cmbAllowBearable.SelectedIndex,
                CmbApproxLengthSelectedIndex = cmbApproxLength.SelectedIndex,
                CmbModeSelectedIndex = cmbMode.SelectedIndex,
                TxtExcludeTracksText = txtExcludeTracks.Text,
                CmbExtendedMixesSelectedIndex = cmbExtendedMixes.SelectedIndex,
                ChkExlcudeMixesOnlyChecked = chkExlcudeMixesOnly.Checked,
                ChkRestrictArtistClumpingChecked = chkRestrictArtistClumping.Checked,
                ChkRestrictGenreClumpingChecked = chkRestrictGenreClumping.Checked,
                ChkRestrictTitleClumpingChecked = chkRestrictTitleClumping.Checked,
                ChkDisplayedTracksOnlyChecked = chkDisplayedTracksOnly.Checked,
                CmbTracksToGenerateSelectedIndex = cmbTracksToGenerate.SelectedIndex,
                CmbContinueMixSelectedIndex = cmbContinueMix.SelectedIndex,
                CmbKeyMixingSelectedIndex = cmbKeyMixing.SelectedIndex
            };

            var filename = Path.Combine(Path.GetTempPath(), "Halloumi.Shuffler.frmGeneratePlaylist.xml");
            SerializationHelper<Settings>.ToXmlFile(settings, filename);
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
            cmbApproxLength.Enabled = true;
            cmbDirection.Enabled = true;
            cmbAllowBearable.Enabled = true;
            cmbMode.Enabled = true;
            txtExcludeTracks.Enabled = true;
            btnExcludeTracks.Enabled = true;
            chkExlcudeMixesOnly.Enabled = true;
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
            cmbApproxLength.Enabled = false;
            cmbDirection.Enabled = false;
            cmbAllowBearable.Enabled = false;
            cmbMode.Enabled = false;
            txtExcludeTracks.Enabled = false;
            btnExcludeTracks.Enabled = false;
            chkExlcudeMixesOnly.Enabled = false;
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
        private void QueueTracks(List<Track> tracks)
        {
            if (tracks.Count == 0) return;

            var currentTrackCount = PlaylistControl.GetTracks().Count;
            var additionalTrackCount = tracks.Count - currentTrackCount;

            if (additionalTrackCount <= 0) return;

            tracks.Reverse();
            tracks = tracks.Take(additionalTrackCount).ToList();
            tracks.Reverse();

            //this.PlaylistControl.ClearTracks();
            PlaylistControl.QueueTracks(tracks);
        }

        /// <summary>
        ///     Handles the Click event of the btnStop control.
        /// </summary>
        private void btnStop_Click(object sender, EventArgs e)
        {
            TrackSelector.StopGeneratePlayList();
        }

        /// <summary>
        ///     Handles the Click event of the btnCancel control.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                _cancel = true;
                TrackSelector.CancelGeneratePlayList();
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
            List<Track> availableTracks;

            if (chkDisplayedTracksOnly.Checked)
            {
                availableTracks = _displayedTracks;
            }
            else
            {
                availableTracks = LibraryControl
                    .GetAvailableTracks()
                    .Where(t => t.IsShufflerTrack)
                    .ToList();
            }

            var strategy =
                (TrackSelector.MixStrategy)
                    Enum.Parse(typeof(TrackSelector.MixStrategy), cmbMode.GetTextThreadSafe().Replace(" ", ""));

            Dictionary<string, Dictionary<string, Track>> excludedMixes = null;

            if (txtExcludeTracks.Text != "" && File.Exists(txtExcludeTracks.Text))
            {
                var excludeTracks = PlaylistHelper.GetFilesInPlaylist(txtExcludeTracks.Text)
                    .Select(f => LibraryControl.Library.GetTrackByFilename(f))
                    .Where(t => t != null)
                    .ToList();

                if (chkExlcudeMixesOnly.Checked)
                {
                    if (excludeTracks.Count > 1)
                        excludedMixes = LibraryControl.MixLibrary.ConvertPlaylistToMixDictionary(excludeTracks);
                }
                else
                {
                    var excludeTrackTitles = excludeTracks
                        .Select(t => t.Title)
                        .ToList();
                    availableTracks.RemoveAll(t => excludeTrackTitles.Contains(t.Title));
                }
            }

            var direction =
                (TrackSelector.Direction) Enum.Parse(typeof(TrackSelector.Direction), cmbDirection.GetTextThreadSafe());
            var allowBearable =
                (TrackSelector.AllowBearableMixStrategy)
                    Enum.Parse(typeof(TrackSelector.AllowBearableMixStrategy),
                        cmbAllowBearable.GetTextThreadSafe().Replace(" ", ""));

            var approxLength = int.MaxValue;
            if (_screenMode == ScreenMode.GeneratePlaylist)
            {
                var comboText = cmbApproxLength.GetTextThreadSafe().Replace(" minutes", "");
                if (comboText != "No limit")
                    approxLength = Convert.ToInt32(comboText);
            }

            var continueMix =
                (TrackSelector.ContinueMix)
                    Enum.Parse(typeof(TrackSelector.ContinueMix), cmbContinueMix.GetTextThreadSafe().Replace(" ", ""));

            var keyMixStrategy =
                (TrackSelector.KeyMixStrategy)
                    Enum.Parse(typeof(TrackSelector.KeyMixStrategy), cmbKeyMixing.GetTextThreadSafe().Replace(" ", ""));

            var useExtendedMixes =
                (TrackSelector.UseExtendedMixes)
                    Enum.Parse(typeof(TrackSelector.UseExtendedMixes),
                        cmbExtendedMixes.GetTextThreadSafe().Replace(" ", ""));

            var tracksToAdd = int.MaxValue;
            if (_screenMode == ScreenMode.AutoGeneratePlaylist)
            {
                tracksToAdd = Convert.ToInt32(cmbTracksToGenerate.GetTextThreadSafe());
            }

            var mixPath = TrackSelector.GeneratePlayList(availableTracks,
                LibraryControl.MixLibrary,
                PlaylistControl.GetTracks(),
                direction,
                approxLength,
                allowBearable,
                strategy,
                useExtendedMixes,
                excludedMixes,
                chkRestrictArtistClumping.Checked,
                chkRestrictGenreClumping.Checked,
                chkRestrictTitleClumping.Checked,
                continueMix,
                keyMixStrategy,
                tracksToAdd);

            if (!_cancel)
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new MethodInvoker(() => QueueTracks(mixPath)));
                }
                else QueueTracks(mixPath);
            }
        }

        /// <summary>
        ///     Handles the Tick event of the timer control.
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            var status = TrackSelector.GeneratePlayListStatus;
            if (status != lblStatus.Text && status != "")
                lblStatus.Text = TrackSelector.GeneratePlayListStatus;
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
                .GetDisplayedTracks()
                .Where(t => t.IsShufflerTrack)
                .ToList();

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

        public class Settings
        {
            public Settings()
            {
                CmbDirectionSelectedIndex = 0;
                CmbAllowBearableSelectedIndex = 0;
                CmbApproxLengthSelectedIndex = 0;
                CmbModeSelectedIndex = 0;
                TxtExcludeTracksText = "";
                CmbExtendedMixesSelectedIndex = 0;
                ChkExlcudeMixesOnlyChecked = false;
                ChkRestrictArtistClumpingChecked = false;
                ChkRestrictGenreClumpingChecked = false;
                ChkDisplayedTracksOnlyChecked = false;
                ChkRestrictTitleClumpingChecked = false;
                CmbTracksToGenerateSelectedIndex = 0;
                CmbContinueMixSelectedIndex = 0;
                CmbKeyMixingSelectedIndex = 0;
            }

            public int CmbDirectionSelectedIndex { get; set; }

            public int CmbAllowBearableSelectedIndex { get; set; }

            public int CmbApproxLengthSelectedIndex { get; set; }

            public int CmbModeSelectedIndex { get; set; }

            public string TxtExcludeTracksText { get; set; }

            public int CmbExtendedMixesSelectedIndex { get; set; }

            public bool ChkExlcudeMixesOnlyChecked { get; set; }

            public bool ChkRestrictArtistClumpingChecked { get; set; }

            public bool ChkRestrictGenreClumpingChecked { get; set; }

            public bool ChkRestrictTitleClumpingChecked { get; set; }

            public bool ChkDisplayedTracksOnlyChecked { get; set; }

            public int CmbTracksToGenerateSelectedIndex { get; set; }

            public int CmbContinueMixSelectedIndex { get; set; }

            public int CmbKeyMixingSelectedIndex { get; set; }
        }
    }
}