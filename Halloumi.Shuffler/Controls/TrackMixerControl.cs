using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.Forms;
using Un4seen.Bass;

namespace Halloumi.Shuffler.Controls
{
    /// <summary>
    /// </summary>
    public partial class TrackMixerControl : UserControl
    {
        private readonly BASSTimer _timer = new BASSTimer();
        private bool _bassPlayerOnTrackChange;
        private bool _binding;
        private bool _bindingManualMode;
        private bool _bindingTrackFxVolumeSlider;
        private bool _bindingVolumeSlider;
        private decimal _lastTrackFxDelayNotes;
        private bool _timerTick;

        /// <summary>
        ///     Initializes a new instance of the TrackMixerControl class.
        /// </summary>
        public TrackMixerControl()
        {
            InitializeComponent();
            sldFader.ValueChanged += sldFader_ValueChanged;
        }

        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Library Library { get; set; }

        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MixLibrary MixLibrary
        {
            get { return PlaylistControl.MixLibrary; }
        }

        /// <summary>
        ///     Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BassPlayer BassPlayer { get; set; }

        /// <summary>
        ///     Gets or sets the playlist control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PlaylistControl PlaylistControl { get; set; }

        private Track PreviousTrack { get; set; }

        private Track CurrentTrack { get; set; }

        private Track NextTrack { get; set; }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            CurrentTrack = BassPlayer.CurrentTrack;
            PreviousTrack = BassPlayer.PreviousTrack;
            NextTrack = BassPlayer.NextTrack;

            BassPlayer.OnTrackQueued += BassPlayer_OnTrackChange;
            BassPlayer.OnTrackChange += BassPlayer_OnTrackChange;

            BassPlayer.OnManualMixVolumeChanged += BassPlayer_OnManualMixVolumeChanged;
            BassPlayer.OnManualMixModeChanged += BassPlayer_OnManualMixModeChanged;

            sldFader.Minimum = 0;
            sldFader.Maximum = 100;

            rdbDelay2.Checked = true;

            chkEnableTrackFXAutomation.Checked = BassPlayer.TrackFxAutomationEnabled;

            cmbFadeOutType.SelectedIndex = 0;

            BassPlayer.DisableManualMixMode();

            BindData();

            _timer.Tick += Timer_Tick;
            _timer.Interval = 200;
            _timer.Start();
        }

        /// <summary>
        ///     Binds the data.
        /// </summary>
        private void BindData()
        {
            _binding = true;

            lblPreviousTitle.Text = PreviousTrack == null ? "" : PreviousTrack.Description;
            lblPreviousFadeDetails.Text = FadeOutDescription(PreviousTrack, CurrentTrack)
                                          + @" "
                                          + GetMixRankDescription(PreviousTrack, CurrentTrack);

            lblCurrentTitle.Text = CurrentTrack == null ? "" : CurrentTrack.Description;
            lblCurrentFadeDetails.Text = (FadeInDescription(PreviousTrack, CurrentTrack)
                                          + @"    "
                                          + FadeOutDescription(CurrentTrack, NextTrack)).Trim();

            lblNextTitle.Text = NextTrack == null ? "" : NextTrack.Description;

            lblNextFadeDetails.Text = FadeInDescription(CurrentTrack, NextTrack)
                                      + @" "
                                      + GetMixRankDescription(CurrentTrack, NextTrack);

            chkManualFading.Checked = BassPlayer.IsManualMixMode;
            sldFader.Enabled = chkManualFading.Checked;
            btnPreviousPowerOff.Enabled = chkManualFading.Checked;
            btnPreviousPause.Enabled = chkManualFading.Checked;
            btnPreviousPause.Visible = true;

            cmbFadeOutType.Enabled = chkManualFading.Checked;

            if (chkManualFading.Checked)
                cmbFadeOutType.SelectedIndex = (int) BassPlayer.CurrentManualExtendedFadeType;
            else
                cmbFadeOutType.SelectedIndex = (int) BassPlayer.GetExtendedFadeType(CurrentTrack, NextTrack);

            _binding = false;
        }

        private string FadeInDescription(Track previousTrack, Track track)
        {
            if (track == null) return "";

            var standardStartLength = track.FullStartLoopLengthSeconds;
            var looped = track.StartLoopCount > 1;
            var powerDown = previousTrack?.PowerDownOnEnd ?? false;

            var description = "Fade In: ";

            description += GetFormattedSeconds(standardStartLength);

            if (powerDown)
            {
                var powerDownFadeIn = BpmHelper.GetDefaultLoopLength(track.StartBpm) / 4D;
                description += " (" + GetFormattedSeconds(powerDownFadeIn) + ")";
            }
            else
            {
                var hasExtendedMix = BassPlayer.HasExtendedMixAttributes(previousTrack, track);
                if (hasExtendedMix)
                {
                    var extendedEndLength = BassPlayer.GetExtendedFadeOutLength(previousTrack, track);
                    description += " (" + GetFormattedSeconds(extendedEndLength) + "*)";
                }
            }

            if (looped) description += " looped";

            description += "  " + track.StartBpm.ToString("00.00") + "BPM";

            return description;
        }

        private string FadeOutDescription(Track track, Track nextTrack)
        {
            if (track == null) return "";

            var standardEndLength = BpmHelper.GetFullEndLoopLengthAdjustedToMatchAnotherTrack(track, nextTrack);
            var looped = track.EndLoopCount > 1;
            var powerDown = track.PowerDownOnEnd;
            var description = "Fade Out: ";

            if (powerDown)
                description = "PowerDown";
            else
                description += GetFormattedSeconds(standardEndLength);

            var hasExtendedMix = BassPlayer.HasExtendedMixAttributes(track, nextTrack);
            if (hasExtendedMix)
            {
                var extendedFadeType = BassPlayer.GetExtendedFadeType(track, nextTrack);
                if (extendedFadeType == ExtendedFadeType.Default)
                {
                    var extendedEndLength = BassPlayer.GetExtendedFadeOutLength(track, nextTrack);
                    description += " (" + GetFormattedSeconds(extendedEndLength) + "*)";
                }
                else
                {
                    description += " (" + extendedFadeType + "*)";
                }
            }

            if (looped) description += " looped";

            description += "  " + track.EndBpm.ToString("00.00") + "BPM";

            return description;
        }

        private string GetMixRankDescription(AudioStream currentTrack, AudioStream nextTrack)
        {
            if (currentTrack == null || nextTrack == null) return "";

            var currentLibraryTrack = Library.GetTrackByFilename(currentTrack.Filename);
            var nextLibraryTrack = Library.GetTrackByFilename(nextTrack.Filename);

            if (currentLibraryTrack == null || nextLibraryTrack == null) return "";

            var rank = MixLibrary.GetMixLevel(currentLibraryTrack, nextLibraryTrack);
            var hasExtendedMix = MixLibrary.HasExtendedMix(currentLibraryTrack, nextLibraryTrack);

            var mixRankDescription = MixLibrary.GetRankDescription(rank);
            if (hasExtendedMix) mixRankDescription += "*";

            return mixRankDescription;
        }

        /// <summary>
        ///     Binds the delay notes.
        /// </summary>
        private void BindDelayNotes()
        {
            _binding = true;

            if (BassPlayer.TrackSendFxDelayNotes == 0.5M) rdbDelay1.Checked = true;
            else if (BassPlayer.TrackSendFxDelayNotes == 0.25M) rdbDelay2.Checked = true;
            else if (BassPlayer.TrackSendFxDelayNotes == 0.125M) rdbDelay3.Checked = true;
            else if (BassPlayer.TrackSendFxDelayNotes == 0.0625M) rdbDelay4.Checked = true;
            else if (BassPlayer.TrackSendFxDelayNotes == 0M) rdbDelayNone.Checked = true;
            else rdbDelay2.Checked = true;

            _binding = false;
        }

        /// <summary>
        ///     Sets the tracks.
        /// </summary>
        private void SetTracks()
        {
            CurrentTrack = BassPlayer.CurrentTrack;
            PreviousTrack = BassPlayer.PreviousTrack ?? GetPreviousTrack();

            sldFader.Value = 0;
            BassPlayer.SetManualMixVolume(sldFader.Value);

            NextTrack = BassPlayer.NextTrack;
        }

        /// <summary>
        ///     Gets the formatted seconds, or "Power Off" if power-down set to true
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <param name="powerDown">The power down flag.</param>
        /// <returns>The formatted seconds.</returns>
        private static string GetFormattedSeconds(double seconds, bool powerDown = false)
        {
            if (powerDown)
                return "Power Off";
            return Convert.ToInt32(Math.Round(seconds, 0)) + "s";
        }

        /// <summary>
        ///     Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            try
            {
                var settings = Settings.Default;

                BassPlayer.TrackSendFxDelayNotes = settings.TrackFxDelayNotes;
                BindDelayNotes();

                BassPlayer.TrackFxAutomationEnabled = settings.EnableTrackFxAutomation;
            }
            catch
            {
                // ignored
            }
        }


        /// <summary>
        ///     Gets the previous track.
        /// </summary>
        /// <returns>The previous track</returns>
        private Track GetPreviousTrack()
        {
            var prevTrack = PlaylistControl.GetPreviousTrack();
            if (prevTrack == null) return null;

            var track = BassPlayer.LoadTrack(prevTrack.Filename);
            BassPlayer.LoadTrackAudioData(track);
            ExtenedAttributesHelper.LoadExtendedAttributes(track);
            return track;
        }

        /// <summary>
        ///     Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(BassPlayer_OnTrackChange));
            else BassPlayer_OnTrackChange();
        }

        /// <summary>
        ///     Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange()
        {
            if (_bassPlayerOnTrackChange) return;
            _bassPlayerOnTrackChange = true;

            SetTracks();

            BindData();

            _bassPlayerOnTrackChange = false;

            cmbFadeOutType.SelectedIndex = 0;
        }

        /// <summary>
        ///     Handles the Tick event of the Timer control.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(Timer_Tick));
            else Timer_Tick();
        }

        /// <summary>
        ///     Handles the Tick event of the Timer control.
        /// </summary>
        private void Timer_Tick()
        {
            if (_timerTick) return;
            _timerTick = true;

            var trackFxDelayNotes = BassPlayer.TrackSendFxDelayNotes;
            if (_lastTrackFxDelayNotes != trackFxDelayNotes)
                BindDelayNotes();
            _lastTrackFxDelayNotes = trackFxDelayNotes;

            _timerTick = false;
        }

        /// <summary>
        ///     Handles the CheckedChanged event of the chkManualFading control.
        /// </summary>
        private void chkManualFading_CheckedChanged(object sender, EventArgs e)
        {
            if (_binding) return;

            _bindingManualMode = true;
            if (chkManualFading.Checked)
                BassPlayer.EnableManualMixMode();
            else
                BassPlayer.DisableManualMixMode();
            _bindingManualMode = false;

            BindData();
        }

        /// <summary>
        ///     Handles the Slid event of the sldFader control.
        /// </summary>
        private void sldFader_ValueChanged(object sender, EventArgs e)
        {
            _bindingVolumeSlider = true;
            BassPlayer.SetManualMixVolume(sldFader.Value);
            _bindingVolumeSlider = false;
        }


        private void BassPlayer_OnManualMixVolumeChanged(object sender, EventArgs e)
        {
            if (_bindingVolumeSlider) return;
            var volume = (int) BassPlayer.GetManualMixVolume();
            sldFader.Value = volume;
        }

        private void BassPlayer_OnManualMixModeChanged(object sender, EventArgs e)
        {
            if (_bindingManualMode) return;
            chkManualFading.Checked = BassPlayer.IsManualMixMode;
        }

        /// <summary>
        ///     Handles the Click event of the btnPreviousPowerOff control.
        /// </summary>
        private void btnPreviousPowerOff_Click(object sender, EventArgs e)
        {
            BassPlayer.PowerOffPreviousTrack();
        }

        /// <summary>
        ///     Handles the Click event of the btnPreviousStop control.
        /// </summary>
        private void btnPreviousPause_Click(object sender, EventArgs e)
        {
            BassPlayer.PausePreviousTrack();
        }


        /// <summary>
        ///     Handles the MouseDown event of the btnTrackFX control.
        /// </summary>
        private void btnTrackFX_MouseDown(object sender, MouseEventArgs e)
        {
            BassPlayer.StartTrackFxSend();
        }

        /// <summary>
        ///     Handles the MouseUp event of the btnTrackFX control.
        /// </summary>
        private void btnTrackFX_MouseUp(object sender, MouseEventArgs e)
        {
            BassPlayer.StopTrackFxSend();
        }

        /// <summary>
        ///     Handles the CheckedChanged event of the rdbDelay control.
        /// </summary>
        private void rdbDelay_CheckedChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            var radioButton = sender as KryptonRadioButton;
            if (radioButton == null) return;
            if (!radioButton.Checked) return;
            var delayNotes = decimal.Parse(radioButton.Tag.ToString());
            BassPlayer.TrackSendFxDelayNotes = delayNotes;
        }

        /// <summary>
        ///     Handles the CheckedChanged event of the chkEnableTrackFXAutomation control.
        /// </summary>
        private void chkEnableTrackFXAutomation_CheckedChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            BassPlayer.TrackFxAutomationEnabled = chkEnableTrackFXAutomation.Checked;
            BindData();
        }

        /// <summary>
        ///     Handles the Click event of the btnSaveLastTrackFX control.
        /// </summary>
        private void btnSaveLastTrackFX_Click(object sender, EventArgs e)
        {
            BassPlayer.SaveLastTrackFxTrigger();
        }

        /// <summary>
        ///     Handles the Click event of the btnClearSends control.
        /// </summary>
        private void btnClearSends_Click(object sender, EventArgs e)
        {
            var track = BassPlayer.CurrentTrack;
            if (track == null) return;

            var message = "Are you sure you wish to clear all Track FX triggers for " + track.Description + "?";
            if (!MessageBoxHelper.Confirm(message))
                return;

            BassPlayer.ClearTrackFxTriggers(track);
        }

        /// <summary>
        ///     Handles the Click event of the btnRemoveLastSend control.
        /// </summary>
        private void btnRemoveLastSend_Click(object sender, EventArgs e)
        {
            var track = BassPlayer.CurrentTrack;
            if (track == null) return;
            BassPlayer.RemovePreviousTrackFxTrigger();
        }

        private void btnSaveMix_Click(object sender, EventArgs e)
        {
            if (!chkManualFading.Checked) return;
            BassPlayer.SaveExtendedMix();
        }

        private void btnClearMix_Click(object sender, EventArgs e)
        {
            if (BassPlayer.CurrentTrack == null || BassPlayer.PreviousTrack == null) return;

            // ReSharper disable once UseStringInterpolation
            var message = string.Format("Are you sure you wish to clear the extend mix details for {0} into {1}?",
                BassPlayer.PreviousTrack.Description,
                BassPlayer.CurrentTrack.Description);

            if (!MessageBoxHelper.Confirm(message)) return;

            BassPlayer.ClearExtendedMix();
        }

        private void cmbFadeOutType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;

            var fadeType = cmbFadeOutType.ParseEnum<ExtendedFadeType>();
            BassPlayer.CurrentManualExtendedFadeType = fadeType;
        }
    }
}