﻿using System.ComponentModel;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.AudioLibrary;
using AE = Halloumi.Shuffler.AudioEngine;

namespace Halloumi.Shuffler.Controls
{
    public partial class SamplePlayer : UserControl
    {
        /// <summary>
        ///     Initializes a new instance of the SamplePlayer class.
        /// </summary>
        public SamplePlayer()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BassPlayer BassPlayer { get; set; }

        /// <summary>
        ///     Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Library Library { get; set; }

        /// <summary>
        ///     Gets or sets the this.Sample.
        /// </summary>
        private Sample Sample { get; set; }

        /// <summary>
        ///     Sets the sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        public void SetSample(Sample sample)
        {
            Sample = sample;
            BindData();
        }

        /// <summary>
        ///     Binds the data to the controls
        /// </summary>
        private void BindData()
        {
            var artist = Sample.LinkedTrackDescription.Split('-')[0].Trim();
            var title = Sample.LinkedTrackDescription.Split('-')[1].Trim();
            var description = $"{Sample.Description} - {title} - {artist}";

            description = description.Trim().Replace("&", "&&");
            DebugHelper.WriteLine("Setting sample:" + description);

            lblSampleDescription.Text = description;
            btnScratch.Visible = AnalogXScratchHelper.IsScratchEnabled();
        }

        public void Pause()
        {
            if (Sample == null) return;
            BassPlayer.PauseSample(Sample);
        }

        /// <summary>
        ///     Handles the MouseDown event of the btnPlay control.
        /// </summary>
        private void btnPlay_MouseDown(object sender, MouseEventArgs e)
        {
            if (Sample == null) return;
            BassPlayer.PlaySample(Sample);
        }

        /// <summary>
        ///     Handles the MouseUp event of the btnPlay control.
        /// </summary>
        private void btnPlay_MouseUp(object sender, MouseEventArgs e)
        {
            if (Sample == null) return;
            BassPlayer.PauseSample(Sample);
        }

        /// <summary>
        ///     Handles the MouseDown event of the btnScratch control.
        /// </summary>
        private void btnScratch_MouseDown(object sender, MouseEventArgs e)
        {
            AnalogXScratchHelper.LaunchShort(Sample);
        }
    }
}