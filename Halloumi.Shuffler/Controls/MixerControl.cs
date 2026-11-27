using System;
using System.ComponentModel;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioLibrary;
using AE = Halloumi.Shuffler.AudioEngine;
using Halloumi.Shuffler;

namespace Halloumi.Shuffler.Controls
{
    public partial class MixerControl : UserControl
    {
        public MixerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Library Library { get; set; }

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

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ShufflerApplication Application { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SamplerControl SamplerControl { get; private set; }

        public void Initialize()
        {
            SamplerControl.Application = Application;
            SamplerControl.BassPlayer = BassPlayer;
            SamplerControl.PlaylistControl = PlaylistControl;
            trackMixerControl.Application = Application;
            trackMixerControl.BassPlayer = BassPlayer;
            trackMixerControl.Library = Library;
            trackMixerControl.PlaylistControl = PlaylistControl;

            mixOverviewControl.Application = Application;

            SamplerControl.Initialize();
            trackMixerControl.Initialize();
            mixOverviewControl.Initialize();
        }

        public void LoadSettings()
        {
            SamplerControl.LoadSettings();
            trackMixerControl.LoadSettings();
            spllLeftRight.SplitterDistance = Width / 4 * 2;

            // the waveform strip's height is dragged to suit the screen, and remembered
            _loadingSettings = true;
            try
            {
                var height = Application.GetMixOverviewHeight();
                if (height <= 0) height = Height / 4;
                var max = spltTopBottom.Height - spltTopBottom.Panel2MinSize - spltTopBottom.SplitterWidth;
                spltTopBottom.SplitterDistance = Math.Max(spltTopBottom.Panel1MinSize, Math.Min(height, max));
            }
            finally
            {
                _loadingSettings = false;
            }

            spltTopBottom.SplitterMoved -= spltTopBottom_SplitterMoved;
            spltTopBottom.SplitterMoved += spltTopBottom_SplitterMoved;
        }

        private bool _loadingSettings;

        /// <summary>
        ///     Shows or hides the waveform strip across the top. Hidden, it stops polling the engine.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool WaveformShown
        {
            get { return !spltTopBottom.Panel1Collapsed; }
            set { spltTopBottom.Panel1Collapsed = !value; }
        }

        private void spltTopBottom_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (_loadingSettings) return;
            Application.SetMixOverviewHeight(spltTopBottom.SplitterDistance);
        }

        public void Unload()
        {
        }
    }
}