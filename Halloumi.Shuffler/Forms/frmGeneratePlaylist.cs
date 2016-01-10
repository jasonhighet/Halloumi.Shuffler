using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.Controls;
using Halloumi.Shuffler.Engine;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Forms
{
    public partial class frmGeneratePlaylist : BaseForm
    {
        private List<Track> _displayedTracks = null;

        private ScreenMode _screenMode;

        /// <summary>
        /// Initializes a new instance of the frmGeneratePlaylist class.
        /// </summary>
        public frmGeneratePlaylist()
        {
            InitializeComponent();

            this.Load += frmGeneratePlaylist_Load;

            this.TrackSelector = new TrackSelector();

            InitialiseControls();
        }

        private void frmGeneratePlaylist_Load(object sender, EventArgs e)
        {
            if (_screenMode == ScreenMode.AutoGeneratePlaylist)
            {
                this.Opacity = 0;
                StartGeneratingPlaylist();
            }
        }

        /// <summary>
        /// Initialises the controls.
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

            for (int i = 0; i < 10; i++)
                cmbTracksToGenerate.Items.Add((i + 1) * 5);
            cmbTracksToGenerate.SelectedIndex = 2;

            LoadSettings();
            EnableControls();
        }

        private void PopulateApproxLength()
        {
            cmbApproxLength.Items.Add("No limit");

            int i = 10;
            while (i <= (24 * 60))
            {
                cmbApproxLength.Items.Add(i.ToString() + " minutes");

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
        /// Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                var settings = GetSettings();

                cmbDirection.SelectedIndex = settings.cmbDirection_SelectedIndex;
                cmbAllowBearable.SelectedIndex = settings.cmbAllowBearable_SelectedIndex;
                cmbApproxLength.SelectedIndex = settings.cmbApproxLength_SelectedIndex;
                cmbMode.SelectedIndex = settings.cmbMode_SelectedIndex;
                txtExcludeTracks.Text = settings.txtExcludeTracks_Text;
                cmbExtendedMixes.SelectedIndex = settings.cmbExtendedMixes_SelectedIndex;
                chkExlcudeMixesOnly.Checked = settings.chkExlcudeMixesOnly_Checked;
                chkRestrictArtistClumping.Checked = settings.chkRestrictArtistClumping_Checked;
                chkRestrictGenreClumping.Checked = settings.chkRestrictGenreClumping_Checked;
                chkRestrictTitleClumping.Checked = settings.chkRestrictTitleClumping_Checked;
                chkDisplayedTracksOnly.Checked = settings.chkDisplayedTracksOnly_Checked;
                cmbTracksToGenerate.SelectedIndex = settings.cmbTracksToGenerate_SelectedIndex;
                cmbContinueMix.SelectedIndex = settings.cmbContinueMix_SelectedIndex;
                cmbKeyMixing.SelectedIndex = settings.cmbKeyMixing_SelectedIndex;
            }
            catch
            { }
        }

        private Settings GetSettings()
        {
            var settings = new Settings();
            var filename = Path.Combine(Path.GetTempPath(), "Halloumi.Shuffler.frmGeneratePlaylist.xml");
            if (File.Exists(filename))
            {
                settings = SerializationHelper<Settings>.FromXmlFile(filename);
            }
            return settings;
        }

        public class Settings
        {
            public int cmbDirection_SelectedIndex { get; set; }

            public int cmbAllowBearable_SelectedIndex { get; set; }

            public int cmbApproxLength_SelectedIndex { get; set; }

            public int cmbMode_SelectedIndex { get; set; }

            public string txtExcludeTracks_Text { get; set; }

            public int cmbExtendedMixes_SelectedIndex { get; set; }

            public bool chkExlcudeMixesOnly_Checked { get; set; }

            public bool chkRestrictArtistClumping_Checked { get; set; }

            public bool chkRestrictGenreClumping_Checked { get; set; }

            public bool chkRestrictTitleClumping_Checked { get; set; }

            public bool chkDisplayedTracksOnly_Checked { get; set; }

            public int cmbTracksToGenerate_SelectedIndex { get; set; }

            public int cmbContinueMix_SelectedIndex { get; set; }

            public int cmbKeyMixing_SelectedIndex { get; set; }

            public Settings()
            {
                this.cmbDirection_SelectedIndex = 0;
                this.cmbAllowBearable_SelectedIndex = 0;
                this.cmbApproxLength_SelectedIndex = 0;
                this.cmbMode_SelectedIndex = 0;
                this.txtExcludeTracks_Text = "";
                this.cmbExtendedMixes_SelectedIndex = 0;
                this.chkExlcudeMixesOnly_Checked = false;
                this.chkRestrictArtistClumping_Checked = false;
                this.chkRestrictGenreClumping_Checked = false;
                this.chkDisplayedTracksOnly_Checked = false;
                this.chkRestrictTitleClumping_Checked = false;
                this.cmbTracksToGenerate_SelectedIndex = 0;
                this.cmbContinueMix_SelectedIndex = 0;
                this.cmbKeyMixing_SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            var settings = new Settings
            {
                cmbDirection_SelectedIndex = cmbDirection.SelectedIndex,
                cmbAllowBearable_SelectedIndex = cmbAllowBearable.SelectedIndex,
                cmbApproxLength_SelectedIndex = cmbApproxLength.SelectedIndex,
                cmbMode_SelectedIndex = cmbMode.SelectedIndex,
                txtExcludeTracks_Text = txtExcludeTracks.Text,
                cmbExtendedMixes_SelectedIndex = cmbExtendedMixes.SelectedIndex,
                chkExlcudeMixesOnly_Checked = chkExlcudeMixesOnly.Checked,
                chkRestrictArtistClumping_Checked = chkRestrictArtistClumping.Checked,
                chkRestrictGenreClumping_Checked = chkRestrictGenreClumping.Checked,
                chkRestrictTitleClumping_Checked = chkRestrictTitleClumping.Checked,
                chkDisplayedTracksOnly_Checked = chkDisplayedTracksOnly.Checked,
                cmbTracksToGenerate_SelectedIndex = cmbTracksToGenerate.SelectedIndex,
                cmbContinueMix_SelectedIndex = cmbContinueMix.SelectedIndex,
                cmbKeyMixing_SelectedIndex = cmbKeyMixing.SelectedIndex
            };

            var filename = Path.Combine(Path.GetTempPath(), "Halloumi.Shuffler.frmGeneratePlaylist.xml");
            SerializationHelper<Settings>.ToXmlFile(settings, filename);
        }

        /// <summary>
        /// Enables the controls.
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
            var enabled = (cmbMode.SelectedIndex == 0 || cmbMode.SelectedIndex == 1 || cmbMode.SelectedIndex == 2);

            chkRestrictArtistClumping.Enabled = enabled;
            chkRestrictGenreClumping.Enabled = enabled;
            chkRestrictTitleClumping.Enabled = enabled;
            cmbDirection.Enabled = enabled;
            cmbAllowBearable.Enabled = enabled;
            cmbExtendedMixes.Enabled = enabled;
            cmbContinueMix.Enabled = enabled;
        }

        /// <summary>
        /// Disables the controls.
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
        /// Queues the tracks.
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        private void QueueTracks(List<Track> tracks)
        {
            if (tracks.Count == 0) return;

            var currentTrackCount = this.PlaylistControl.GetTracks().Count;
            var additionalTrackCount = tracks.Count - currentTrackCount;

            if (additionalTrackCount <= 0) return;

            tracks.Reverse();
            tracks = tracks.Take(additionalTrackCount).ToList();
            tracks.Reverse();

            //this.PlaylistControl.ClearTracks();
            this.PlaylistControl.QueueTracks(tracks);
        }

        /// <summary>
        /// Gets or sets the library control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TrackLibraryControl LibraryControl { get; set; }

        /// <summary>
        /// Gets or sets the library control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PlaylistControl PlaylistControl { get; set; }

        /// <summary>
        /// Gets or sets the track selector.
        /// </summary>
        private TrackSelector TrackSelector { get; set; }

        /// <summary>
        /// Handles the Click event of the btnStop control.
        /// </summary>
        private void btnStop_Click(object sender, EventArgs e)
        {
            this.TrackSelector.StopGeneratePlayList();
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                _cancel = true;
                this.TrackSelector.CancelGeneratePlayList();
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private bool _cancel = false;

        /// <summary>
        /// Handles the DoWork event of the backgroundWorker control.
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
                availableTracks = this.LibraryControl
                    .GetAvailableTracks()
                    .Where(t => t.IsShufflerTrack)
                    .ToList();
            }

            var strategy = (TrackSelector.MixStrategy)Enum.Parse(typeof(TrackSelector.MixStrategy), cmbMode.GetTextThreadSafe().Replace(" ", ""));

            Dictionary<string, Dictionary<string, Track>> excludedMixes = null;

            if (txtExcludeTracks.Text != "" && File.Exists(txtExcludeTracks.Text))
            {
                var excludeTracks = BE.PlaylistHelper.GetFilesInPlaylist(txtExcludeTracks.Text)
                                      .Select(f => this.LibraryControl.Library.GetTrackByFilename(f))
                                      .Where(t => t != null)
                                      .ToList();

                if (chkExlcudeMixesOnly.Checked)
                {
                    if (excludeTracks.Count > 1)
                        excludedMixes = this.LibraryControl.MixLibrary.ConvertPlaylistToMixDictionary(excludeTracks);
                }
                else
                {
                    var excludeTrackTitles = excludeTracks
                        .Select(t => t.Title)
                        .ToList();
                    availableTracks.RemoveAll(t => excludeTrackTitles.Contains(t.Title));
                }
            }

            var direction = (TrackSelector.Direction)Enum.Parse(typeof(TrackSelector.Direction), cmbDirection.GetTextThreadSafe());
            var allowBearable = (TrackSelector.AllowBearableMixStrategy)Enum.Parse(typeof(TrackSelector.AllowBearableMixStrategy), cmbAllowBearable.GetTextThreadSafe().Replace(" ", ""));

            var approxLength = int.MaxValue;
            if (_screenMode == ScreenMode.GeneratePlaylist)
            {
                var comboText = cmbApproxLength.GetTextThreadSafe().Replace(" minutes", "");
                if (comboText != "No limit")
                    approxLength = Convert.ToInt32(comboText);
            }

            var continueMix = (TrackSelector.ContinueMix)Enum.Parse(typeof(TrackSelector.ContinueMix), cmbContinueMix.GetTextThreadSafe().Replace(" ", ""));

            var keyMixStrategy = (TrackSelector.KeyMixStrategy)Enum.Parse(typeof(TrackSelector.KeyMixStrategy), cmbKeyMixing.GetTextThreadSafe().Replace(" ", ""));

            var useExtendedMixes = (TrackSelector.UseExtendedMixes)Enum.Parse(typeof(TrackSelector.UseExtendedMixes), cmbExtendedMixes.GetTextThreadSafe().Replace(" ", ""));

            var tracksToAdd = int.MaxValue;
            if (_screenMode == ScreenMode.AutoGeneratePlaylist)
            {
                tracksToAdd = Convert.ToInt32(cmbTracksToGenerate.GetTextThreadSafe());
            }

            var mixPath = this.TrackSelector.GeneratePlayList(availableTracks,
                this.LibraryControl.MixLibrary,
                this.PlaylistControl.GetTracks(),
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
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(() => QueueTracks(mixPath)));
                }
                else QueueTracks(mixPath);
            }
        }

        /// <summary>
        /// Handles the Tick event of the timer control.
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            var status = this.TrackSelector.GeneratePlayListStatus;
            if (status != lblStatus.Text && status != "")
                lblStatus.Text = this.TrackSelector.GeneratePlayListStatus;
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the backgroundWorker control.
        /// </summary>
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer.Stop();
            EnableControls();
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the btnStart control.
        /// </summary>
        private void btnStart_Click(object sender, EventArgs e)
        {
            StartGeneratingPlaylist();
        }

        public void StartGeneratingPlaylist()
        {
            DisableControls();

            _displayedTracks = this.LibraryControl
                .GetDisplayedTracks()
                .Where(t => t.IsShufflerTrack)
                .ToList();

            backgroundWorker.RunWorkerAsync();
            timer.Start();
        }

        /// <summary>
        /// Handles the FormClosing event of the frmGeneratePlaylist control.
        /// </summary>
        private void frmGeneratePlaylist_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.Cancel)
                SaveSettings();
        }

        private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControlsByMode();
        }

        public enum ScreenMode
        {
            GeneratePlaylist,
            AutoGeneratePlaylist,
            AutoGenerateSettings,
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}