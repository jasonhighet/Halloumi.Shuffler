using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.Forms;
using AE = Halloumi.Shuffler.AudioEngine;


namespace Halloumi.Shuffler.Controls
{
    public partial class SamplerControl : UserControl
    {
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
        public BassPlayer BassPlayer { get; set; }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            rdbDelay2.Checked = true;
            chkEnableAutomation.Checked = BassPlayer.SampleAutomationEnabled;

            LoadSamplePlayers();

            BassPlayer.OnTrackSamplesChanged += BassPlayer_OnTrackSamplesChanged;

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
                if (i % 2 != 0) player.BackColor = Color.WhiteSmoke;
                SamplePlayers.Add(player);
                flpLeft.Controls.Add(player);
                player.Visible = false;
            }

            flpLeft.Controls.Remove(samplePlayer);

            flpLeft.ResumeLayout();
        }

        private void BassPlayer_OnTrackSamplesChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(LoadSamplePlayers));
            else LoadSamplePlayers();
        }

        /// <summary>
        ///     Loads the sample players.
        /// </summary>
        private void LoadSamplePlayers()
        {
            flpLeft.SuspendLayout();

            var samples = BassPlayer.GetSamples();
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
                else if (settings.SamplerDelayNotes == 0.375M) rdbDelay5.Checked = true;
                else if (settings.SamplerDelayNotes == 0.1875M) rdbDelay6.Checked = true;

                BassPlayer.SamplerDelayNotes = settings.SamplerDelayNotes;
            }
            catch
            {
                // ignored
            }
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