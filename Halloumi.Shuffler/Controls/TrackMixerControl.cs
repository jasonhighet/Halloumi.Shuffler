using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Halloumi.BassEngine;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Forms;
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
            get { return this.PlaylistControl.MixLibrary; }
        }

        /// <summary>
        /// Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BE.BassPlayer BassPlayer { get; set; }

        /// <summary>
        /// Gets or sets the playlist control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PlaylistControl PlaylistControl { get; set; }

        private BE.Track PreviousTrack { get; set; }

        private BE.Track CurrentTrack { get; set; }

        private BE.Track NextTrack { get; set; }

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
            this.BassPlayer.ManualFadeOut = false;

            this.CurrentTrack = this.BassPlayer.CurrentTrack;
            this.PreviousTrack = this.BassPlayer.PreviousTrack;
            this.NextTrack = this.BassPlayer.NextTrack;

            this.BassPlayer.OnTrackQueued += new EventHandler(BassPlayer_OnTrackChange);
            this.BassPlayer.OnTrackChange += new EventHandler(BassPlayer_OnTrackChange);

            sldFader.Minimum = 0;
            sldFader.Maximum = 100;

            sldTrackFXVolume.Scrolled += new MediaSlider.MediaSlider.ScrollDelegate(sldTrackFXVolume_Scrolled);
            sldTrackFXVolume.Minimum = 0;
            sldTrackFXVolume.Maximum = 100;
            var volume = Convert.ToInt32(this.BassPlayer.GetTrackSendFXVolume());
            lblVolume.Text = volume.ToString();
            sldTrackFXVolume.Value = volume;

            rdbDelay2.Checked = true;
            cmbOutput.SelectedIndex = 0;

            chkEnableTrackFXAutomation.Checked = this.BassPlayer.TrackFXAutomationEnabled;

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
        private void SetTrackFXVolume(decimal volume)
        {
            if (volume < 0 || volume > 100) return;

            this.BassPlayer.SetTrackSendFXMixerVolume(volume);
            lblVolume.Text = volume.ToString();

            if (sldTrackFXVolume.Value != volume) sldTrackFXVolume.Value = Convert.ToInt32(volume);
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            _binding = true;

            lblPreviousTitle.Text = (this.PreviousTrack == null) ? "" : this.PreviousTrack.Description;
            lblPreviousFadeDetails.Text = FadeOutDescription(this.PreviousTrack, this.CurrentTrack)
                + " "
                + GetMixRankDescription(this.PreviousTrack, this.CurrentTrack);

            lblCurrentTitle.Text = (this.CurrentTrack == null) ? "" : this.CurrentTrack.Description;
            lblCurrentFadeDetails.Text = (FadeInDescription(this.PreviousTrack, this.CurrentTrack)
                + "    "
                + FadeOutDescription(this.CurrentTrack, this.NextTrack)).Trim();

            lblNextTitle.Text = (this.NextTrack == null) ? "" : this.NextTrack.Description;

            lblNextFadeDetails.Text = FadeInDescription(this.CurrentTrack, this.NextTrack)
                + " "
                + GetMixRankDescription(this.CurrentTrack, this.NextTrack);

            chkManualFading.Checked = this.BassPlayer.ManualFadeOut;
            sldFader.Enabled = chkManualFading.Checked;
            btnPreviousPowerOff.Enabled = chkManualFading.Checked;
            btnPreviousPause.Enabled = chkManualFading.Checked;
            btnPreviousPause.Visible = true;

            cmbFadeOutType.Enabled = chkManualFading.Checked;

            if (chkManualFading.Checked)
                cmbFadeOutType.SelectedIndex = (int)this.BassPlayer.CurrentManualExtendedFadeType;
            else
            {
                cmbFadeOutType.SelectedIndex = (int)this.BassPlayer.GetExtendedFadeType(this.CurrentTrack, this.NextTrack);
            }

            _binding = false;
        }

        private string FadeInDescription(BE.Track previousTrack, BE.Track track)
        {
            if (track == null) return "";

            var standardStartLength = track.FullStartLoopLengthSeconds;
            var looped = track.StartLoopCount > 1;
            var powerDown = (previousTrack == null) ? false : previousTrack.PowerDownOnEnd;

            var description = "Fade In: ";

            description += GetFormattedSeconds(standardStartLength);

            if (powerDown)
            {
                var powerDownFadeIn = BassHelper.GetDefaultLoopLength(track.StartBPM) / 4D;
                description += " (" + GetFormattedSeconds(powerDownFadeIn) + ")";
            }
            else
            {
                var hasExtendedMix = this.BassPlayer.HasExtendedMixAttributes(previousTrack, track);
                if (hasExtendedMix)
                {
                    var extendedEndLength = this.BassPlayer.GetExtendedFadeOutLength(previousTrack, track);
                    description += " (" + GetFormattedSeconds(extendedEndLength) + "*)";
                }
            }

            if (looped) description += " looped";

            description += "  " + track.StartBPM.ToString("00.00") + "BPM"; ;

            return description;
        }

        private string FadeOutDescription(BE.Track track, BE.Track nextTrack)
        {
            if (track == null) return "";

            var standardEndLength = BassHelper.GetFullEndLoopLengthAdjustedToMatchAnotherTrack(track, nextTrack);
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

            var hasExtendedMix = this.BassPlayer.HasExtendedMixAttributes(track, nextTrack);
            if (hasExtendedMix)
            {
                var extendedFadeType = this.BassPlayer.GetExtendedFadeType(track, nextTrack);
                if (extendedFadeType == ExtendedFadeType.Default)
                {
                    var extendedEndLength = this.BassPlayer.GetExtendedFadeOutLength(track, nextTrack);
                    description += " (" + GetFormattedSeconds(extendedEndLength) + "*)";
                }
                else
                {
                    description += " (" + extendedFadeType.ToString() + "*)";
                }
            }

            if (looped) description += " looped";

            description += "  " + track.EndBPM.ToString("00.00") + "BPM";

            return description;
        }

        private bool _binding = false;

        private string GetMixRankDescription(BE.Track currentTrack, BE.Track nextTrack)
        {
            if (currentTrack == null || nextTrack == null) return "";

            var currentLibraryTrack = this.Library.GetTrackByFilename(currentTrack.Filename);
            var nextLibraryTrack = this.Library.GetTrackByFilename(nextTrack.Filename);

            if (currentLibraryTrack == null || nextLibraryTrack == null) return "";

            var rank = this.MixLibrary.GetMixLevel(currentLibraryTrack, nextLibraryTrack);
            var hasExtendedMix = this.MixLibrary.HasExtendedMix(currentLibraryTrack, nextLibraryTrack);

            var mixRankDescription = this.MixLibrary.GetRankDescription(rank);
            if (hasExtendedMix) mixRankDescription += "*";

            return mixRankDescription;
        }

        /// <summary>
        /// Binds the delay notes.
        /// </summary>
        private void BindDelayNotes()
        {
            _binding = true;

            if (this.BassPlayer.TrackSendFXDelayNotes == 0.5M) rdbDelay1.Checked = true;
            else if (this.BassPlayer.TrackSendFXDelayNotes == 0.25M) rdbDelay2.Checked = true;
            else if (this.BassPlayer.TrackSendFXDelayNotes == 0.125M) rdbDelay3.Checked = true;
            else if (this.BassPlayer.TrackSendFXDelayNotes == 0.0625M) rdbDelay4.Checked = true;
            else if (this.BassPlayer.TrackSendFXDelayNotes == 0M) rdbDelayNone.Checked = true;
            else rdbDelay2.Checked = true;

            _binding = false;
        }

        /// <summary>
        /// Sets the tracks.
        /// </summary>
        private void SetTracks()
        {
            this.CurrentTrack = this.BassPlayer.CurrentTrack;
            this.PreviousTrack = this.BassPlayer.PreviousTrack;
            if (this.PreviousTrack == null) this.PreviousTrack = GetPreviousTrack();

            sldFader.Value = 0;
            this.NextTrack = this.BassPlayer.NextTrack;
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
                var settings = Halloumi.Shuffler.Forms.Settings.Default;

                this.BassPlayer.TrackSendFXDelayNotes = settings.TrackFXDelayNotes;
                BindDelayNotes();

                SetTrackFXVolume(settings.TrackFXVolume);

                this.BassPlayer.TrackOutput = settings.TrackOutput;
                if (settings.TrackOutput == BE.Channels.SoundOutput.Speakers) cmbOutput.SelectedIndex = 0;
                if (settings.TrackOutput == BE.Channels.SoundOutput.Monitor) cmbOutput.SelectedIndex = 1;
                if (settings.TrackOutput == BE.Channels.SoundOutput.Both) cmbOutput.SelectedIndex = 2;

                this.BassPlayer.TrackFXAutomationEnabled = settings.EnableTrackFXAutomation;
            }
            catch
            { }
        }

        /// <summary>
        /// Sets the volume.
        /// </summary>
        public void SetFaderVolumes()
        {
            if (this.BassPlayer.PreviousManaulExtendedFadeType != ExtendedFadeType.Default) return;

            var value = (float)sldFader.ScrollValue;
            value = 100F - value;

            var track = this.BassPlayer.PreviousTrack;
            if (track == null) return;

            var range = this.BassPlayer.DefaultFadeOutStartVolume - 0;
            var volume = (decimal)(this.BassPlayer.DefaultFadeOutEndVolume + (range * (value / 100)));

            DebugHelper.WriteLine(volume);

            BE.BassHelper.SetTrackVolume(track, volume);
        }

        /// <summary>
        /// Makes the power off noise on a track
        /// </summary>
        /// <param name="track">The track.</param>
        private void PowerOff(BE.Track track)
        {
            if (track == null) return;
            if (this.BassPlayer.PlayState != BE.PlayState.Playing) return;
            if (!this.BassPlayer.IsTrackInUse(track)) return;

            if (track == this.BassPlayer.CurrentTrack)
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
                BE.BassHelper.TrackPowerDown(track);
                this.BassPlayer.StopRecordingManualExtendedMix(true);
            }
        }

        /// <summary>
        /// Pauses the track.
        /// </summary>
        /// <param name="track">The track.</param>
        private void PauseTrack(BE.Track track)
        {
            if (track == null) return;
            if (!this.BassPlayer.IsTrackInUse(track)) return;

            if (BE.BassHelper.IsTrackPlaying(track))
            {
                if (track == this.BassPlayer.CurrentTrack)
                {
                    this.BassPlayer.Pause();
                }
                else if (track == this.BassPlayer.PreviousTrack)
                {
                    this.BassPlayer.StopRecordingManualExtendedMix();
                    BE.BassHelper.TrackSmoothPause(track);
                }
                else
                {
                    BE.BassHelper.TrackSmoothPause(track);
                }
            }
            else
            {
                if (track == this.BassPlayer.CurrentTrack)
                {
                    this.BassPlayer.Play();
                }
            }
        }

        /// <summary>
        /// Gets the previous track.
        /// </summary>
        /// <returns>The previous track</returns>
        private BE.Track GetPreviousTrack()
        {
            var prevTrack = this.PlaylistControl.GetPreviousTrack();
            if (prevTrack == null) return null;

            var track = this.BassPlayer.LoadTrack(prevTrack.Filename);
            this.BassPlayer.LoadTrackAudioData(track);
            this.BassPlayer.LoadExtendedAttributes(track);
            return track;
        }

        /// <summary>
        /// Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
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
            if (_bassPlayer_OnTrackChange) return;
            _bassPlayer_OnTrackChange = true;

            SetTracks();
            SetFaderVolumes();
            BindData();

            _bassPlayer_OnTrackChange = false;

            cmbFadeOutType.SelectedIndex = 0;
        }

        private bool _bassPlayer_OnTrackChange = false;

        /// <summary>
        /// Handles the Tick event of the Timer control.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
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
            if (_timer_Tick) return;
            _timer_Tick = true;

            var trackFXDelayNotes = this.BassPlayer.TrackSendFXDelayNotes;
            if (_lastTrackFXDelayNotes != trackFXDelayNotes)
            {
                BindDelayNotes();
            }
            _lastTrackFXDelayNotes = trackFXDelayNotes;

            _timer_Tick = false;
        }

        private bool _timer_Tick = false;

        private decimal _lastTrackFXDelayNotes = 0M;

        /// <summary>
        /// Handles the CheckedChanged event of the chkManualFading control.
        /// </summary>
        private void chkManualFading_CheckedChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            this.BassPlayer.ManualFadeOut = chkManualFading.Checked;
            this.BassPlayer.CurrentManualExtendedFadeType = ExtendedFadeType.Default;
            this.BassPlayer.PreviousManaulExtendedFadeType = ExtendedFadeType.Default;
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
            PowerOff(this.PreviousTrack);
        }

        /// <summary>
        /// Handles the Click event of the btnCurrentPowerOff control.
        /// </summary>
        private void btnCurrentPowerOff_Click(object sender, EventArgs e)
        {
            PowerOff(this.CurrentTrack);
        }

        /// <summary>
        /// Handles the Click event of the btnPreviousStop control.
        /// </summary>
        private void btnPreviousPause_Click(object sender, EventArgs e)
        {
            PauseTrack(this.PreviousTrack);
        }

        /// <summary>
        /// Handles the Click event of the btnCurrentStop control.
        /// </summary>
        private void btnCurrentPause_Click(object sender, EventArgs e)
        {
            PauseTrack(this.CurrentTrack);
        }

        /// <summary>
        /// Handles the MouseDown event of the btnTrackFX control.
        /// </summary>
        private void btnTrackFX_MouseDown(object sender, MouseEventArgs e)
        {
            this.BassPlayer.StartTrackFXSend();
            if (this.BassPlayer.CurrentTrack == null) return;
        }

        /// <summary>
        /// Handles the MouseUp event of the btnTrackFX control.
        /// </summary>
        private void btnTrackFX_MouseUp(object sender, MouseEventArgs e)
        {
            this.BassPlayer.StopTrackFXSend();
            if (this.BassPlayer.CurrentTrack == null) return;
        }

        /// <summary>
        /// Handles the Click event of the mnuSkipToEnd control.
        /// </summary>
        private void mnuSkipToEnd_Click(object sender, EventArgs e)
        {
            this.BassPlayer.SkipToFadeOut();
        }

        /// <summary>
        /// Handles the Click event of the mnuPowerDown control.
        /// </summary>
        private void mnuPowerDown_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.CurrentTrack != null) this.BassPlayer.CurrentTrack.PowerDownOnEnd = true;
            this.BassPlayer.SkipToFadeOut();
        }

        /// <summary>
        /// Handles the Click event of the mnuFadeNow control.
        /// </summary>
        private void mnuFadeNow_Click(object sender, EventArgs e)
        {
            this.BassPlayer.SkipToFadeOut();
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
            this.BassPlayer.TrackSendFXDelayNotes = delayNotes;
        }

        /// <summary>
        /// Handles the Click event of the btnTrackEffect control.
        /// </summary>
        private void btnTrackEffect_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.TrackVSTPlugin != null)
            {
                this.BassPlayer.ShowVSTPluginConfig(BassPlayer.TrackVSTPlugin);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnMainEffect control.
        /// </summary>
        private void btnMainEffect_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.MainVSTPlugin != null)
            {
                this.BassPlayer.ShowVSTPluginConfig(BassPlayer.MainVSTPlugin);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbOutput control.
        /// </summary>
        private void cmbOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            var outputType = cmbOutput.ParseEnum<BE.Channels.SoundOutput>();
            this.BassPlayer.TrackOutput = outputType;
        }

        /// <summary>
        /// Handles the Click event of the btnEffect2 control.
        /// </summary>
        private void btnEffect2_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.TrackSendFXVSTPlugin2 != null)
            {
                this.BassPlayer.ShowVSTPluginConfig(BassPlayer.TrackSendFXVSTPlugin2);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnEffect1 control.
        /// </summary>
        private void btnEffect1_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.TrackSendFXVSTPlugin != null)
            {
                this.BassPlayer.ShowVSTPluginConfig(BassPlayer.TrackSendFXVSTPlugin);
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkEnableTrackFXAutomation control.
        /// </summary>
        private void chkEnableTrackFXAutomation_CheckedChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            this.BassPlayer.TrackFXAutomationEnabled = chkEnableTrackFXAutomation.Checked;
            BindData();
        }

        /// <summary>
        /// Handles the Click event of the btnSaveLastTrackFX control.
        /// </summary>
        private void btnSaveLastTrackFX_Click(object sender, EventArgs e)
        {
            this.BassPlayer.SaveLastTrackFXTrigger();
        }

        /// <summary>
        /// Handles the Scrolled event of the sldTrackFXVolume control.
        /// </summary>
        private void sldTrackFXVolume_Scrolled(object sender, EventArgs e)
        {
            var volume = Convert.ToDecimal(sldTrackFXVolume.ScrollValue);
            SetTrackFXVolume(Convert.ToDecimal(sldTrackFXVolume.ScrollValue));
        }

        /// <summary>
        /// Handles the Click event of the btnClearSends control.
        /// </summary>
        private void btnClearSends_Click(object sender, EventArgs e)
        {
            var track = this.BassPlayer.CurrentTrack;
            if (track == null) return;

            if (!MessageBoxHelper.Confirm("Are you sure you wish to clear all Track FX triggers for " + track.Description + "?")) return;

            this.BassPlayer.ClearTrackFXTriggers(track);
        }

        /// <summary>
        /// Handles the Click event of the btnRemoveLastSend control.
        /// </summary>
        private void btnRemoveLastSend_Click(object sender, EventArgs e)
        {
            var track = this.BassPlayer.CurrentTrack;
            if (track == null) return;
            this.BassPlayer.RemovePreviousTrackFXTrigger();
        }

        private void btnSaveMix_Click(object sender, EventArgs e)
        {
            if (!chkManualFading.Checked) return;
            this.BassPlayer.SaveExtendedMix();
        }

        private void btnClearMix_Click(object sender, EventArgs e)
        {
            if (this.BassPlayer.CurrentTrack == null || this.BassPlayer.PreviousTrack == null) return;
            var message = String.Format("Are you sure you wish to clear the extend mix details for {0} into {1}?",
                this.BassPlayer.PreviousTrack.Description,
                this.BassPlayer.CurrentTrack.Description);

            if (!MessageBoxHelper.Confirm(message)) return;

            this.BassPlayer.ClearExtendedMix();
        }

        private void cmbFadeOutType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;

            var fadeType = cmbFadeOutType.ParseEnum<ExtendedFadeType>();
            this.BassPlayer.CurrentManualExtendedFadeType = fadeType;
        }
    }
}