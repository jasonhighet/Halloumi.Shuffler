using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.Forms;
using AE = Halloumi.Shuffler.AudioEngine;


namespace Halloumi.Shuffler.Controls
{
    public partial class SamplerControl : UserControl
    {
        private bool _bindingVolumeSlider;
        private bool _bassPlayerOnTrackQueued;
        private Track _currentTrack;
        private Track _nextTrack;

        public SamplerControl()
        {
            InitializeComponent();

            SamplePlayers = new List<SamplePlayer>();

            var settings = Settings.Default;
            AnalogXScratchHelper.SetApplicationFolder(settings.AnalogXScratchFolder);
        }

        private List<SamplePlayer> SamplePlayers { get; }

        /// <summary>
        ///     Gets or sets the playlist control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PlaylistControl PlaylistControl { get; set; }

        /// <summary>
        ///     Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AE.BassPlayer BassPlayer { get; set; }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            sldVolume.Scrolled += sldVolume_Slid;
            sldVolume.Minimum = 0;
            sldVolume.Maximum = 100;

            SetVolume(Convert.ToInt32(BassPlayer.GetSamplerMixerVolume()));

            rdbDelay2.Checked = true;
            chkEnableAutomation.Checked = BassPlayer.SampleAutomationEnabled;

            LoadSamples();

            BassPlayer.OnTrackQueued += BassPlayer_OnTrackQueued;
            BassPlayer.OnSamplerMixerVolumeChanged += BassPlayer_OnSamplerMixerVolumeChanged;

            flpLeft.SuspendLayout();
            SamplePlayers.Clear();

            for (var i = 0; i < 20; i++)
            {
                var player = new SamplePlayer
                {
                    BackColor = Color.White,
                    Size = new Size(samplePlayer.Width, samplePlayer.Height),
                    Dock = DockStyle.Top,
                    BassPlayer = BassPlayer,
                    Library = PlaylistControl.Library
                };
                if (i%2 != 0) player.BackColor = Color.WhiteSmoke;
                SamplePlayers.Add(player);
                flpLeft.Controls.Add(player);
                player.Visible = false;
            }

            flpLeft.Controls.Remove(samplePlayer);

            flpLeft.ResumeLayout();
        }

        private void BassPlayer_OnSamplerMixerVolumeChanged(object sender, EventArgs e)
        {
            if (_bindingVolumeSlider) return;
            var volume = (int)BassPlayer.GetSamplerMixerVolume();

            lblVolume.Text = volume.ToString();
            if (sldVolume.Value != volume) sldVolume.Value = volume;
        }

        private void SetVolume(int volume)
        {
            if (volume < 0 || volume > 100) return;

            BassPlayer.SetSamplerMixerVolume(Convert.ToDecimal(volume));

            volume = (int) BassPlayer.GetSamplerMixerVolume();
            lblVolume.Text = volume.ToString();
            if (sldVolume.Value != volume) sldVolume.Value = volume;
        }

        public void Unload()
        {
            UnloadSamples();
        }

        /// <summary>
        ///     Loads the samples.
        /// </summary>
        private void LoadSamples()
        {
            BassPlayer.UnloadSamples(_currentTrack);
            BassPlayer.UnloadSamples(_nextTrack);

            _currentTrack = BassPlayer.CurrentTrack;
            _nextTrack = BassPlayer.NextTrack;

            BassPlayer.LoadSamples(_nextTrack);
            BassPlayer.LoadSamples(_currentTrack);

            PlaylistControl.Library.LinkedSampleLibrary.LoadLinkedSamples(BassPlayer, _currentTrack);

            LoadSamplePlayers();
        }

        /// <summary>
        ///     Unloads the samples.
        /// </summary>
        private void UnloadSamples()
        {
            BassPlayer.UnloadSamples(_nextTrack);
            BassPlayer.UnloadSamples(_currentTrack);
            BassPlayer.UnloadSamples();
        }

        /// <summary>
        ///     Loads the sample players.
        /// </summary>
        private void LoadSamplePlayers()
        {
            flpLeft.SuspendLayout();

            var samples = BassPlayer.Samples.ToList();
            samples.Reverse();

            for (var i = 0; i < SamplePlayers.Count; i++)
            {
                var player = SamplePlayers[i];
                if (i < samples.Count)
                {
                    player.SetSample(samples[i]);
                    player.Visible = true;
                }
                else
                {
                    player.Pause();
                    player.Visible = false;
                }
            }

            flpLeft.ResumeLayout();
        }

        /// <summary>
        ///     Refreshes the samples.
        /// </summary>
        public void RefreshSamples()
        {
            UnloadSamples();
            LoadSamples();
        }

        /// <summary>
        ///     Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            try
            {
                var settings = Settings.Default;

                if (settings.SamplerDelayNotes == 0.5M) rdbDelay1.Checked = true;
                else if (settings.SamplerDelayNotes == 0.25M) rdbDelay2.Checked = true;
                else if (settings.SamplerDelayNotes == 0.125M) rdbDelay3.Checked = true;
                else if (settings.SamplerDelayNotes == 0.0625M) rdbDelay4.Checked = true;
                else if (settings.SamplerDelayNotes == 0M) rdbDelayNone.Checked = true;
                BassPlayer.SamplerDelayNotes = settings.SamplerDelayNotes;

                SetVolume(settings.SamplerVolume);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Handles the Slid event of the sldVolume control.
        /// </summary>
        private void sldVolume_Slid(object sender, EventArgs e)
        {
            _bindingVolumeSlider = true;
            SetVolume(sldVolume.ScrollValue);
            _bindingVolumeSlider = false;
        }

        /// <summary>
        ///     Handles the OnTrackQueued event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackQueued(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(BassPlayer_OnTrackQueued));
            }
            else BassPlayer_OnTrackQueued();
        }

        /// <summary>
        ///     Handles the OnTrackQueued event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackQueued()
        {
            if (_bassPlayerOnTrackQueued) return;
            _bassPlayerOnTrackQueued = true;

            if (_currentTrack != BassPlayer.CurrentTrack) RefreshSamples();

            _bassPlayerOnTrackQueued = false;
        }

        /// <summary>
        ///     Handles the CheckedChanged event of the rdbDelay control.
        /// </summary>
        private void rdbDelay_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as KryptonRadioButton;
            if (radioButton == null) return;
            if (!radioButton.Checked) return;
            var delayNotes = decimal.Parse(radioButton.Tag.ToString());
            BassPlayer.SamplerDelayNotes = delayNotes;
        }

        /// <summary>
        ///     Handles the Click event of the btnSaveLastSampleTrigger control.
        /// </summary>
        private void btnSaveLastSampleTrigger_Click(object sender, EventArgs e)
        {
            BassPlayer.SaveLastSampleTrigger();
        }

        /// <summary>
        ///     Handles the Click event of the btnRemoveLastSampleTrigger control.
        /// </summary>
        private void btnRemoveLastSampleTrigger_Click(object sender, EventArgs e)
        {
            var track = BassPlayer.CurrentTrack;
            if (track == null) return;
            BassPlayer.RemovePreviousSampleTrigger();
        }

        /// <summary>
        ///     Handles the Click event of the btnClearSampleTriggers control.
        /// </summary>
        private void btnClearSampleTriggers_Click(object sender, EventArgs e)
        {
            var track = BassPlayer.CurrentTrack;
            if (track == null) return;

            var message = "Are you sure you wish to clear all sample triggers for " + track.Description + "?";
            if (!MessageBoxHelper.Confirm(message)) return;

            BassPlayer.ClearSampleTriggers();
        }

        private void chkEnableAutomation_CheckedChanged(object sender, EventArgs e)
        {
            BassPlayer.SampleAutomationEnabled = chkEnableAutomation.Checked;
        }
    }
}