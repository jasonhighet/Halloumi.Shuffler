using System.ComponentModel;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioLibrary;
using AE = Halloumi.Shuffler.AudioEngine;

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
        public AE.BassPlayer BassPlayer { get; set; }

        /// <summary>
        ///     Gets or sets the playlist control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PlaylistControl PlaylistControl { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SamplerControl SamplerControl { get; private set; }

        public void Initialize()
        {
            SamplerControl.BassPlayer = BassPlayer;
            SamplerControl.PlaylistControl = PlaylistControl;
            trackMixerControl.BassPlayer = BassPlayer;
            trackMixerControl.Library = Library;
            trackMixerControl.PlaylistControl = PlaylistControl;

            SamplerControl.Initialize();
            trackMixerControl.Initialize();
        }

        public void LoadSettings()
        {
            SamplerControl.LoadSettings();
            trackMixerControl.LoadSettings();
            spllLeftRight.SplitterDistance = Width / 4 * 2;
        }

        public void Unload()
        {
            //SamplerControl.UnloadAll();
        }
    }
}