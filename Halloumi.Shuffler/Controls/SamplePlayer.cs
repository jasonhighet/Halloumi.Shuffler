using System;
using System.ComponentModel;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.Engine;
using AE = Halloumi.Shuffler.AudioEngine;

namespace Halloumi.Shuffler.Controls
{
    public partial class SamplePlayer : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the SamplePlayer class.
        /// </summary>
        public SamplePlayer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        public void SetSample(Sample sample)
        {
            Sample = sample;
            BindData();
        }

        /// <summary>
        /// Binds the data to the controls
        /// </summary>
        private void BindData()
        {
            lblSampleDescription.Text = (Sample.LinkedTrackDescription + " - " + Sample.Description).Trim().Replace("&", "&&");
            if (CurrentTrack == null
                || Sample == null
                || Sample.LinkedTrackDescription == CurrentTrack.Description)
            {
                btnLink.Text = "&Link";
                btnLink.Enabled = false;
            }
            else if (LinkedSampleLibrary.IsSampleLinkedToTrack(CurrentTrack, Sample))
            {
                btnLink.Text = "&Unlink";
                btnLink.Enabled = true;
            }
            else
            {
                btnLink.Text = "&Link";
                btnLink.Enabled = true;
            }
        }

        /// <summary>
        /// Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AE.BassPlayer BassPlayer { get; set; }

        /// <summary>
        /// Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Library Library { get; set; }

        /// <summary>
        /// Gets the linked sample library.
        /// </summary>
        private LinkedSampleLibrary LinkedSampleLibrary
        {
            get { return Library.LinkedSampleLibrary; }
        }

        /// <summary>
        /// Gets the current track.
        /// </summary>
        private Track CurrentTrack
        {
            get { return BassPlayer.CurrentTrack; }
        }

        /// <summary>
        /// Gets or sets the this.Sample.
        /// </summary>
        private Sample Sample { get; set; }

        /// <summary>
        /// Handles the MouseDown event of the btnPlay control.
        /// </summary>
        private void btnPlay_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (btnPlay.Text == "&Stop")
                {
                    BassPlayer.MuteSample(Sample);
                    btnPlay.Text = "&Play";
                }
                else
                {
                    BassPlayer.PlaySample(Sample);
                }
            }
            else if (Sample.IsLooped)
            {
                BassPlayer.PlaySample(Sample);
                btnPlay.Text = "&Stop";
            }
        }

        /// <summary>
        /// Handles the MouseUp event of the btnPlay control.
        /// </summary>
        private void btnPlay_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                BassPlayer.MuteSample(Sample);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the btnScratch control.
        /// </summary>
        private void btnScratch_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                AnalogXScratchHelper.LaunchShort(Sample);
            else
                AnalogXScratchHelper.Launch(Sample);
        }

        /// <summary>
        /// Handles the Click event of the btnLink control.
        /// </summary>
        private void btnLink_Click(object sender, EventArgs e)
        {
            if (btnLink.Text == "&Link")
            {
                LinkedSampleLibrary.LinkSampleToTrack(CurrentTrack, Sample);
            }
            else if (btnLink.Text == "&Unlink")
            {
                LinkedSampleLibrary.UnlinkSampleFromTrack(CurrentTrack, Sample);
            }
            BindData();
        }
    }
}