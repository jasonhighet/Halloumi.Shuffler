using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Halloumi.Shuffler.Engine;
using BE = Halloumi.BassEngine;

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
        public void SetSample(BE.Sample sample)
        {
            this.Sample = sample;
            BindData();
        }

        /// <summary>
        /// Binds the data to the controls
        /// </summary>
        private void BindData()
        {
            lblSampleDescription.Text = (this.Sample.LinkedTrackDescription + " - " + this.Sample.Description).Trim().Replace("&", "&&");
            if (this.CurrentTrack == null
                || this.Sample == null
                || this.Sample.LinkedTrackDescription == this.CurrentTrack.Description)
            {
                btnLink.Text = "&Link";
                btnLink.Enabled = false;
            }
            else if (this.LinkedSampleLibrary.IsSampleLinkedToTrack(this.CurrentTrack, this.Sample))
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
        public BE.BassPlayer BassPlayer { get; set; }

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
            get { return this.Library.LinkedSampleLibrary; }
        }

        /// <summary>
        /// Gets the current track.
        /// </summary>
        private BE.Track CurrentTrack
        {
            get { return this.BassPlayer.CurrentTrack; }
        }

        /// <summary>
        /// Gets or sets the this.Sample.
        /// </summary>
        private BE.Sample Sample { get; set; }

        /// <summary>
        /// Handles the MouseDown event of the btnPlay control.
        /// </summary>
        private void btnPlay_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (btnPlay.Text == "&Stop")
                {
                    this.BassPlayer.MuteSample(this.Sample);
                    btnPlay.Text = "&Play";
                }
                else
                {
                    this.BassPlayer.PlaySample(this.Sample);
                }
            }
            else if (this.Sample.IsLooped)
            {
                this.BassPlayer.PlaySample(this.Sample);
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
                this.BassPlayer.MuteSample(this.Sample);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the btnScratch control.
        /// </summary>
        private void btnScratch_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                BE.AnalogXScratchHelper.LaunchShort(this.Sample);
            else
                BE.AnalogXScratchHelper.Launch(this.Sample);
        }

        /// <summary>
        /// Handles the Click event of the btnLink control.
        /// </summary>
        private void btnLink_Click(object sender, EventArgs e)
        {
            if (btnLink.Text == "&Link")
            {
                this.LinkedSampleLibrary.LinkSampleToTrack(this.CurrentTrack, this.Sample);
            }
            else if (btnLink.Text == "&Unlink")
            {
                this.LinkedSampleLibrary.UnlinkSampleFromTrack(this.CurrentTrack, this.Sample);
            }
            BindData();
        }
    }
}