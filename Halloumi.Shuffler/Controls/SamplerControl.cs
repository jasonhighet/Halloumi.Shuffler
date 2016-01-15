using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.Engine;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Controls
{
    public partial class SamplerControl : UserControl
    {
        private BE.Track _currentTrack = null;
        private BE.Track _nextTrack = null;

        //private BE.Track _previousTrack = null;
        private BE.Track _additionalTrack = null;

        private List<SamplePlayer> SamplePlayers { get; set; }

        public SamplerControl()
        {
            InitializeComponent();

            SamplePlayers = new List<SamplePlayer>();

            var settings = Forms.Settings.Default;
            BE.AnalogXScratchHelper.SetApplicationFolder(settings.AnalogXScratchFolder);
        }

        public void LoadAdditionalTrack(Track track)
        {
            _additionalTrack = BassPlayer.LoadTrack(track.Filename);
            BassPlayer.LoadTagData(_additionalTrack);
            BassPlayer.LoadTrackAudioData(_additionalTrack);
            LoadSamples();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            sldVolume.Scrolled += new MediaSlider.MediaSlider.ScrollDelegate(sldVolume_Slid);
            sldVolume.Minimum = 0;
            sldVolume.Maximum = 100;

            SetVolume(Convert.ToInt32(BassPlayer.GetSamplerMixerVolume()));

            rdbDelay2.Checked = true;
            chkEnableAutomation.Checked = BassPlayer.SampleAutomationEnabled;
            cmbOutput.SelectedIndex = 0;

            LoadSamples();

            BassPlayer.OnTrackQueued += new EventHandler(BassPlayer_OnTrackQueued);
        }

        private void SetVolume(int volume)
        {
            if (volume < 0 || volume > 100) return;

            BassPlayer.SetSamplerMixerVolume(Convert.ToDecimal(volume));

            volume = (int)BassPlayer.GetSamplerMixerVolume();

            lblVolume.Text = volume.ToString();

            if (sldVolume.Value != volume) sldVolume.Value = volume;
        }

        public void Unload()
        {
            UnloadSamples();
        }

        /// <summary>
        /// Loads the samples.
        /// </summary>
        private void LoadSamples()
        {
            //this.BassPlayer.UnloadSamples(_previousTrack);
            BassPlayer.UnloadSamples(_currentTrack);
            BassPlayer.UnloadSamples(_nextTrack);
            BassPlayer.UnloadSamples(_additionalTrack);

            _currentTrack = BassPlayer.CurrentTrack;
            _nextTrack = BassPlayer.NextTrack;
            //_previousTrack = this.BassPlayer.PreviousTrack;
            //if (_previousTrack == null) _previousTrack = GetPreviousTrack();

            BassPlayer.LoadSamples(_nextTrack);
            BassPlayer.LoadSamples(_currentTrack);
            //this.BassPlayer.LoadTrackSamples(_previousTrack);

            if (!BassPlayer.IsTrackInUse(_additionalTrack))
                BassPlayer.LoadSamples(_additionalTrack);

            PlaylistControl.Library.LinkedSampleLibrary.LoadLinkedSamples(BassPlayer, _currentTrack);

            LoadSamplePlayers();
        }

        /// <summary>
        /// Unloads the samples.
        /// </summary>
        private void UnloadSamples()
        {
            BassPlayer.UnloadSamples(_nextTrack);
            BassPlayer.UnloadSamples(_currentTrack);
            //this.BassPlayer.UnloadSamples(_previousTrack);
            BassPlayer.UnloadSamples(_additionalTrack);
            BassPlayer.UnloadSamples();
        }

        /// <summary>
        /// Loads the sample players.
        /// </summary>
        private void LoadSamplePlayers()
        {
            flpLeft.SuspendLayout();

            SamplePlayers.ForEach(sp => sp.Visible = false);
            foreach (var samplePlayer in SamplePlayers)
            {
                flpLeft.Controls.Remove(samplePlayer);
                samplePlayer.Dispose();
            }
            SamplePlayers.Clear();

            var samples = BassPlayer.Samples.ToList();
            samples.Reverse();

            var i = 0;
            foreach (var sample in samples)
            {
                var samplePlayer = new SamplePlayer();

                samplePlayer.BackColor = Color.White;
                samplePlayer.Size = new Size(577, 50);
                samplePlayer.Dock = DockStyle.Top;

                samplePlayer.BassPlayer = BassPlayer;
                samplePlayer.Library = PlaylistControl.Library;

                if (i % 2 != 0) samplePlayer.BackColor = Color.WhiteSmoke;

                samplePlayer.SetSample(sample);
                SamplePlayers.Add(samplePlayer);
                flpLeft.Controls.Add(samplePlayer);

                i++;
            }

            flpLeft.ResumeLayout();
        }

        /// <summary>
        /// Gets the previous track.
        /// </summary>
        /// <returns>The previous track</returns>
        private BE.Track GetPreviousTrack()
        {
            var prevTrack = PlaylistControl.GetPreviousTrack();
            if (prevTrack == null) return null;

            var track = BassPlayer.LoadTrack(prevTrack.Filename);
            BassPlayer.LoadTrackAudioData(track);
            BassPlayer.LoadExtendedAttributes(track);
            return track;
        }

        /// <summary>
        /// Refreshes the samples.
        /// </summary>
        public void RefreshSamples()
        {
            UnloadSamples();
            LoadSamples();
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            try
            {
                var settings = Forms.Settings.Default;

                if (settings.SamplerDelayNotes == 0.5M) rdbDelay1.Checked = true;
                else if (settings.SamplerDelayNotes == 0.25M) rdbDelay2.Checked = true;
                else if (settings.SamplerDelayNotes == 0.125M) rdbDelay3.Checked = true;
                else if (settings.SamplerDelayNotes == 0.0625M) rdbDelay4.Checked = true;
                else if (settings.SamplerDelayNotes == 0M) rdbDelayNone.Checked = true;
                BassPlayer.SamplerDelayNotes = settings.SamplerDelayNotes;

                SetVolume(settings.SamplerVolume);

                BassPlayer.SamplerOutput = settings.SamplerOutput;
                if (settings.SamplerOutput == BE.Channels.SoundOutput.Speakers) cmbOutput.SelectedIndex = 0;
                if (settings.SamplerOutput == BE.Channels.SoundOutput.Monitor) cmbOutput.SelectedIndex = 1;
                if (settings.SamplerOutput == BE.Channels.SoundOutput.Both) cmbOutput.SelectedIndex = 2;
            }
            catch
            { }
        }

        /// <summary>
        /// Gets or sets the playlist control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PlaylistControl PlaylistControl { get; set; }

        /// <summary>
        /// Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BE.BassPlayer BassPlayer { get; set; }

        /// <summary>
        /// Handles the Click event of the btnRefresh control.
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshSamples();
        }

        /// <summary>
        /// Handles the Slid event of the sldVolume control.
        /// </summary>
        private void sldVolume_Slid(object sender, EventArgs e)
        {
            SetVolume(sldVolume.ScrollValue);
        }

        /// <summary>
        /// Handles the OnTrackQueued event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackQueued(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    BassPlayer_OnTrackQueued();
                }));
            }
            else BassPlayer_OnTrackQueued();
        }

        /// <summary>
        /// Handles the OnTrackQueued event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackQueued()
        {
            if (_bassPlayerOnTrackQueued) return;
            _bassPlayerOnTrackQueued = true;

            if (_currentTrack != BassPlayer.CurrentTrack) RefreshSamples();

            _bassPlayerOnTrackQueued = false;
        }

        private bool _bassPlayerOnTrackQueued = false;

        /// <summary>
        /// Handles the CheckedChanged event of the rdbDelay control.
        /// </summary>
        private void rdbDelay_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as ComponentFactory.Krypton.Toolkit.KryptonRadioButton;
            if (!radioButton.Checked) return;
            var delayNotes = Decimal.Parse(radioButton.Tag.ToString());
            BassPlayer.SamplerDelayNotes = delayNotes;
        }

        /// <summary>
        /// Handles the Click event of the btnEffect2 control.
        /// </summary>
        private void btnEffect2_Click(object sender, EventArgs e)
        {
            if (BassPlayer.SamplerVstPlugin2 != null)
            {
                BassPlayer.ShowVstPluginConfig(BassPlayer.SamplerVstPlugin2);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnEffect1 control.
        /// </summary>
        private void btnEffect1_Click(object sender, EventArgs e)
        {
            if (BassPlayer.SamplerVstPlugin != null)
            {
                BassPlayer.ShowVstPluginConfig(BassPlayer.SamplerVstPlugin);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbOutput control.
        /// </summary>
        private void cmbOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            var outputType = cmbOutput.ParseEnum<BE.Channels.SoundOutput>();
            BassPlayer.SamplerOutput = outputType;
        }

        /// <summary>
        /// Handles the Click event of the btnSaveLastSampleTrigger control.
        /// </summary>
        private void btnSaveLastSampleTrigger_Click(object sender, EventArgs e)
        {
            BassPlayer.SaveLastSampleTrigger();
        }

        /// <summary>
        /// Handles the Click event of the btnRemoveLastSampleTrigger control.
        /// </summary>
        private void btnRemoveLastSampleTrigger_Click(object sender, EventArgs e)
        {
            var track = BassPlayer.CurrentTrack;
            if (track == null) return;
            BassPlayer.RemovePreviousSampleTrigger();
        }

        /// <summary>
        /// Handles the Click event of the btnClearSampleTriggers control.
        /// </summary>
        private void btnClearSampleTriggers_Click(object sender, EventArgs e)
        {
            var track = BassPlayer.CurrentTrack;
            if (track == null) return;

            if (!MessageBoxHelper.Confirm("Are you sure you wish to clear all sample triggers for " + track.Description + "?")) return;

            BassPlayer.ClearSampleTriggers();
        }

        private void chkEnableAutomation_CheckedChanged(object sender, EventArgs e)
        {
            BassPlayer.SampleAutomationEnabled = chkEnableAutomation.Checked;
        }
    }
}