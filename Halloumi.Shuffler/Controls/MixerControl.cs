using System;
using System.ComponentModel;
using System.Windows.Forms;
using Halloumi.Shuffler.Engine;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Controls
{
    public partial class MixerControl : UserControl
    {
        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Library Library { get; set; }

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

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SamplerControl SamplerControl
        {
            get { return samplerControl; }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!DesignMode
                && SamplerControl != null
                && Visible)
            {
                SamplerControl.RefreshSamples();
            }
        }

        public MixerControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            samplerControl.BassPlayer = BassPlayer;
            samplerControl.PlaylistControl = PlaylistControl;
            trackMixerControl.BassPlayer = BassPlayer;
            trackMixerControl.Library = Library;
            trackMixerControl.PlaylistControl = PlaylistControl;

            samplerControl.Initialize();
            trackMixerControl.Initialize();

            SamplerControl.RefreshSamples();
        }

        public void LoadSettings()
        {
            samplerControl.LoadSettings();
            trackMixerControl.LoadSettings();
            spllLeftRight.SplitterDistance = (Width / 4) * 2;
        }

        public void Unload()
        {
            samplerControl.Unload();
        }
    }
}