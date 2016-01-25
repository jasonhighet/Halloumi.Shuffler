﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using Halloumi.BassEngine;
using Halloumi.BassEngine.Helpers;
using Halloumi.BassEngine.Models;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.Engine;

using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Controls
{
    /// <summary>
    ///
    /// </summary>
    public partial class TrackMixerControl : UserControl
    {
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
        public MixLibrary MixLibrary
        {
            get { return PlaylistControl.MixLibrary; }
        }

        /// <summary>
        /// Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BassPlayer BassPlayer { get; set; }

        /// <summary>
        /// Gets or sets the playlist control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PlaylistControl PlaylistControl { get; set; }

        private Track PreviousTrack { get; set; }

        private Track CurrentTrack { get; set; }

        private Track NextTrack { get; set; }

        private Un4seen.Bass.BASSTimer _timer = new Un4seen.Bass.BASSTimer();

        /// <summary>
        /// Initializes a new instance of the TrackMixerControl class.
        /// </summary>
        public TrackMixerControl()
        {
            InitializeComponent();
            sldFader.ValueChanged += new MediaSlider.MediaSlider.ValueChangedDelegate(sldFader_ValueChanged);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            BassPlayer.ManualFadeOut = false;

            CurrentTrack = BassPlayer.CurrentTrack;
            PreviousTrack = BassPlayer.PreviousTrack;
            NextTrack = BassPlayer.NextTrack;

            BassPlayer.OnTrackQueued += new EventHandler(BassPlayer_OnTrackChange);
            BassPlayer.OnTrackChange += new EventHandler(BassPlayer_OnTrackChange);

            sldFader.Minimum = 0;
            sldFader.Maximum = 100;

            sldTrackFXVolume.Scrolled += new MediaSlider.MediaSlider.ScrollDelegate(sldTrackFXVolume_Scrolled);
            sldTrackFXVolume.Minimum = 0;
            sldTrackFXVolume.Maximum = 100;
            var volume = Convert.ToInt32(BassPlayer.GetTrackSendFxVolume());
            lblVolume.Text = volume.ToString();
            sldTrackFXVolume.Value = volume;

            rdbDelay2.Checked = true;
            cmbOutput.SelectedIndex = 0;

            chkEnableTrackFXAutomation.Checked = BassPlayer.TrackFxAutomationEnabled;

            cmbFadeOutType.SelectedIndex = 0;

            BindData();

            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Interval = 200;
            _timer.Start();
        }

        /// <summary>
        /// Sets the track FX volume.
        /// </summary>
        /// <param name="volume">The volume.</param>
        private void SetTrackFxVolume(decimal volume)
        {
            if (volume < 0 || volume > 100) return;

            BassPlayer.SetTrackSendFxMixerVolume(volume);
            lblVolume.Text = volume.ToString();

            if (sldTrackFXVolume.Value != volume) sldTrackFXVolume.Value = Convert.ToInt32(volume);
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            _binding = true;

            lblPreviousTitle.Text = (PreviousTrack == null) ? "" : PreviousTrack.Description;
            lblPreviousFadeDetails.Text = FadeOutDescription(PreviousTrack, CurrentTrack)
                + " "
                + GetMixRankDescription(PreviousTrack, CurrentTrack);

            lblCurrentTitle.Text = (CurrentTrack == null) ? "" : CurrentTrack.Description;
            lblCurrentFadeDetails.Text = (FadeInDescription(PreviousTrack, CurrentTrack)
                + "    "
                + FadeOutDescription(CurrentTrack, NextTrack)).Trim();

            lblNextTitle.Text = (NextTrack == null) ? "" : NextTrack.Description;

            lblNextFadeDetails.Text = FadeInDescription(CurrentTrack, NextTrack)
                + " "
                + GetMixRankDescription(CurrentTrack, NextTrack);

            chkManualFading.Checked = BassPlayer.ManualFadeOut;
            sldFader.Enabled = chkManualFading.Checked;
            btnPreviousPowerOff.Enabled = chkManualFading.Checked;
            btnPreviousPause.Enabled = chkManualFading.Checked;
            btnPreviousPause.Visible = true;

            cmbFadeOutType.Enabled = chkManualFading.Checked;

            if (chkManualFading.Checked)
                cmbFadeOutType.SelectedIndex = (int)BassPlayer.CurrentManualExtendedFadeType;
            else
            {
                cmbFadeOutType.SelectedIndex = (int)BassPlayer.GetExtendedFadeType(CurrentTrack, NextTrack);
            }

            _binding = false;
        }

        private string FadeInDescription(Track previousTrack, Track track)
        {
            if (track == null) return "";

            var standardStartLength = track.FullStartLoopLengthSeconds;
            var looped = track.StartLoopCount > 1;
            var powerDown = (previousTrack == null) ? false : previousTrack.PowerDownOnEnd;

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

            description += "  " + track.StartBpm.ToString("00.00") + "BPM"; ;

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
            {
                description = "PowerDown";
            }
            else
            {
                description += GetFormattedSeconds(standardEndLength);
            }

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
                    description += " (" + extendedFadeType.ToString() + "*)";
                }
            }

            if (looped) description += " looped";

            description += "  " + track.EndBpm.ToString("00.00") + "BPM";

            return description;
        }

        private bool _binding = false;

        private string GetMixRankDescription(Track currentTrack, Track nextTrack)
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
        /// Binds the delay notes.
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
        /// Sets the tracks.
        /// </summary>
        private void SetTracks()
        {
            CurrentTrack = BassPlayer.CurrentTrack;
            PreviousTrack = BassPlayer.PreviousTrack;
            if (PreviousTrack == null) PreviousTrack = GetPreviousTrack();

            sldFader.Value = 0;
            NextTrack = BassPlayer.NextTrack;
            SetFaderVolumes();
        }

        /// <summary>
        /// Gets the formatted seconds, or "Power Off" if powerdown set to true
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <param name="powerDown">The power down flag.</param>
        /// <returns>The formatted seconds.</returns>
        private string GetFormattedSeconds(double seconds, bool powerDown)
        {
            if (powerDown)
                return "Power Off";
            else
                return Convert.ToInt32(Math.Round(seconds, 0)).ToString() + "s";
        }

        /// <summary>
        /// Gets the formatted seconds.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns>the formatted seconds.</returns>
        private string GetFormattedSeconds(double seconds)
        {
            return GetFormattedSeconds(seconds, false);
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            try
            {
                var settings = Forms.Settings.Default;

                BassPlayer.TrackSendFxDelayNotes = settings.TrackFxDelayNotes;
                BindDelayNotes();

                SetTrackFxVolume(settings.TrackFxVolume);

                BassPlayer.TrackOutput = settings.TrackOutput;
                if (settings.TrackOutput == BE.Channels.SoundOutput.Speakers) cmbOutput.SelectedIndex = 0;
                if (settings.TrackOutput == BE.Channels.SoundOutput.Monitor) cmbOutput.SelectedIndex = 1;
                if (settings.TrackOutput == BE.Channels.SoundOutput.Both) cmbOutput.SelectedIndex = 2;

                BassPlayer.TrackFxAutomationEnabled = settings.EnableTrackFxAutomation;
            }
            catch
            { }
        }

        /// <summary>
        /// Sets the volume.
        /// </summary>
        public void SetFaderVolumes()
        {
            if (BassPlayer.PreviousManaulExtendedFadeType != ExtendedFadeType.Default) return;

            var value = (float)sldFader.ScrollValue;
            value = 100F - value;

            var track = BassPlayer.PreviousTrack;
            if (track == null) return;

            var range = BassPlayer.DefaultFadeOutStartVolume - 0;
            var volume = (decimal)(BassPlayer.DefaultFadeOutEndVolume + (range * (value / 100)));

            DebugHelper.WriteLine(volume);

            BassHelper.SetTrackVolume(track, volume);
        }

        /// <summary>
        /// Makes the power off noise on a track
        /// </summary>
        /// <param name="track">The track.</param>
        private void PowerOff(Track track)
        {
            if (track == null) return;
            if (BassPlayer.PlayState != PlayState.Playing) return;
            if (!BassPlayer.IsTrackInUse(track)) return;

            if (track == BassPlayer.CurrentTrack)
            {
                //if (MessageBoxHelper.Confirm("Are you sure you want to power down the current track?"))
                //{
                //    BE.BassHelper.TrackPowerDown(track);
                //    for (int i = 0; i < 8; i++)
                //    {
                //        Application.DoEvents();
                //        System.Threading.Thread.Sleep(200);
                //    }
                //    this.BassPlayer.Pause();
                //}
            }
            else
            {
                BassHelper.TrackPowerDown(track);
                BassPlayer.StopRecordingManualExtendedMix(true);
            }
        }

        /// <summary>
        /// Pauses the track.
        /// </summary>
        /// <param name="track">The track.</param>
        private void PauseTrack(Track track)
        {
            if (track == null) return;
            if (!BassPlayer.IsTrackInUse(track)) return;

            if (BassHelper.IsTrackPlaying(track))
            {
                if (track == BassPlayer.CurrentTrack)
                {
                    BassPlayer.Pause();
                }
                else if (track == BassPlayer.PreviousTrack)
                {
                    BassPlayer.StopRecordingManualExtendedMix();
                    BassHelper.TrackSmoothPause(track);
                }
                else
                {
                    BassHelper.TrackSmoothPause(track);
                }
            }
            else
            {
                if (track == BassPlayer.CurrentTrack)
                {
                    BassPlayer.Play();
                }
            }
        }

        /// <summary>
        /// Gets the previous track.
        /// </summary>
        /// <returns>The previous track</returns>
        private Track GetPreviousTrack()
        {
            var prevTrack = PlaylistControl.GetPreviousTrack();
            if (prevTrack == null) return null;

            var track = BassPlayer.LoadTrack(prevTrack.Filename);
            BassPlayer.LoadTrackAudioData(track);
            BassPlayer.LoadExtendedAttributes(track);
            return track;
        }

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

            SetTracks();
            SetFaderVolumes();
            BindData();

            _bassPlayerOnTrackChange = false;

            cmbFadeOutType.SelectedIndex = 0;
        }

        private bool _bassPlayerOnTrackChange = false;

        /// <summary>
        /// Handles the Tick event of the Timer control.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    Timer_Tick();
                }));
            }
            else Timer_Tick();
        }

        /// <summary>
        /// Handles the Tick event of the Timer control.
        /// </summary>
        private void Timer_Tick()
        {
            if (_timerTick) return;
            _timerTick = true;

            var trackFxDelayNotes = BassPlayer.TrackSendFxDelayNotes;
            if (_lastTrackFxDelayNotes != trackFxDelayNotes)
            {
                BindDelayNotes();
            }
            _lastTrackFxDelayNotes = trackFxDelayNotes;

            _timerTick = false;
        }

        private bool _timerTick = false;

        private decimal _lastTrackFxDelayNotes = 0M;

        /// <summary>
        /// Handles the CheckedChanged event of the chkManualFading control.
        /// </summary>
        private void chkManualFading_CheckedChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            BassPlayer.ManualFadeOut = chkManualFading.Checked;
            BassPlayer.CurrentManualExtendedFadeType = ExtendedFadeType.Default;
            BassPlayer.PreviousManaulExtendedFadeType = ExtendedFadeType.Default;
            sldFader.Value = 100;
            BindData();
        }

        /// <summary>
        /// Handles the Slid event of the sldFader control.
        /// </summary>
        private void sldFader_ValueChanged(object sender, EventArgs e)
        {
            SetFaderVolumes();
        }

        /// <summary>
        /// Handles the Click event of the btnPreviousPowerOff control.
        /// </summary>
        private void btnPreviousPowerOff_Click(object sender, EventArgs e)
        {
            PowerOff(PreviousTrack);
        }

        /// <summary>
        /// Handles the Click event of the btnCurrentPowerOff control.
        /// </summary>
        private void btnCurrentPowerOff_Click(object sender, EventArgs e)
        {
            PowerOff(CurrentTrack);
        }

        /// <summary>
        /// Handles the Click event of the btnPreviousStop control.
        /// </summary>
        private void btnPreviousPause_Click(object sender, EventArgs e)
        {
            PauseTrack(PreviousTrack);
        }

        /// <summary>
        /// Handles the Click event of the btnCurrentStop control.
        /// </summary>
        private void btnCurrentPause_Click(object sender, EventArgs e)
        {
            PauseTrack(CurrentTrack);
        }

        /// <summary>
        /// Handles the MouseDown event of the btnTrackFX control.
        /// </summary>
        private void btnTrackFX_MouseDown(object sender, MouseEventArgs e)
        {
            BassPlayer.StartTrackFxSend();
            if (BassPlayer.CurrentTrack == null) return;
        }

        /// <summary>
        /// Handles the MouseUp event of the btnTrackFX control.
        /// </summary>
        private void btnTrackFX_MouseUp(object sender, MouseEventArgs e)
        {
            BassPlayer.StopTrackFxSend();
            if (BassPlayer.CurrentTrack == null) return;
        }

        /// <summary>
        /// Handles the Click event of the mnuSkipToEnd control.
        /// </summary>
        private void mnuSkipToEnd_Click(object sender, EventArgs e)
        {
            BassPlayer.SkipToFadeOut();
        }

        /// <summary>
        /// Handles the Click event of the mnuPowerDown control.
        /// </summary>
        private void mnuPowerDown_Click(object sender, EventArgs e)
        {
            if (BassPlayer.CurrentTrack != null) BassPlayer.CurrentTrack.PowerDownOnEnd = true;
            BassPlayer.SkipToFadeOut();
        }

        /// <summary>
        /// Handles the Click event of the mnuFadeNow control.
        /// </summary>
        private void mnuFadeNow_Click(object sender, EventArgs e)
        {
            BassPlayer.SkipToFadeOut();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the rdbDelay control.
        /// </summary>
        private void rdbDelay_CheckedChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            var radioButton = sender as ComponentFactory.Krypton.Toolkit.KryptonRadioButton;
            if (!radioButton.Checked) return;
            var delayNotes = Decimal.Parse(radioButton.Tag.ToString());
            BassPlayer.TrackSendFxDelayNotes = delayNotes;
        }

        /// <summary>
        /// Handles the Click event of the btnTrackEffect control.
        /// </summary>
        private void btnTrackEffect_Click(object sender, EventArgs e)
        {
            if (BassPlayer.TrackVstPlugin != null)
            {
                BassPlayer.ShowVstPluginConfig(BassPlayer.TrackVstPlugin);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnMainEffect control.
        /// </summary>
        private void btnMainEffect_Click(object sender, EventArgs e)
        {
            if (BassPlayer.MainVstPlugin != null)
            {
                BassPlayer.ShowVstPluginConfig(BassPlayer.MainVstPlugin);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbOutput control.
        /// </summary>
        private void cmbOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            var outputType = cmbOutput.ParseEnum<BE.Channels.SoundOutput>();
            BassPlayer.TrackOutput = outputType;
        }

        /// <summary>
        /// Handles the Click event of the btnEffect2 control.
        /// </summary>
        private void btnEffect2_Click(object sender, EventArgs e)
        {
            if (BassPlayer.TrackSendFxvstPlugin2 != null)
            {
                BassPlayer.ShowVstPluginConfig(BassPlayer.TrackSendFxvstPlugin2);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnEffect1 control.
        /// </summary>
        private void btnEffect1_Click(object sender, EventArgs e)
        {
            if (BassPlayer.TrackSendFxvstPlugin != null)
            {
                BassPlayer.ShowVstPluginConfig(BassPlayer.TrackSendFxvstPlugin);
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkEnableTrackFXAutomation control.
        /// </summary>
        private void chkEnableTrackFXAutomation_CheckedChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            BassPlayer.TrackFxAutomationEnabled = chkEnableTrackFXAutomation.Checked;
            BindData();
        }

        /// <summary>
        /// Handles the Click event of the btnSaveLastTrackFX control.
        /// </summary>
        private void btnSaveLastTrackFX_Click(object sender, EventArgs e)
        {
            BassPlayer.SaveLastTrackFxTrigger();
        }

        /// <summary>
        /// Handles the Scrolled event of the sldTrackFXVolume control.
        /// </summary>
        private void sldTrackFXVolume_Scrolled(object sender, EventArgs e)
        {
            var volume = Convert.ToDecimal(sldTrackFXVolume.ScrollValue);
            SetTrackFxVolume(Convert.ToDecimal(sldTrackFXVolume.ScrollValue));
        }

        /// <summary>
        /// Handles the Click event of the btnClearSends control.
        /// </summary>
        private void btnClearSends_Click(object sender, EventArgs e)
        {
            var track = BassPlayer.CurrentTrack;
            if (track == null) return;

            if (!MessageBoxHelper.Confirm("Are you sure you wish to clear all Track FX triggers for " + track.Description + "?")) return;

            BassPlayer.ClearTrackFxTriggers(track);
        }

        /// <summary>
        /// Handles the Click event of the btnRemoveLastSend control.
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
            var message = String.Format("Are you sure you wish to clear the extend mix details for {0} into {1}?",
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