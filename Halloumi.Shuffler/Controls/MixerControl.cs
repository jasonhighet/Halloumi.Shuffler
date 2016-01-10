using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.Controls;
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
            get { return this.samplerControl; }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!this.DesignMode
                && this.SamplerControl != null
                && this.Visible)
            {
                this.SamplerControl.RefreshSamples();
            }
        }

        public MixerControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            samplerControl.BassPlayer = this.BassPlayer;
            samplerControl.PlaylistControl = this.PlaylistControl;
            trackMixerControl.BassPlayer = this.BassPlayer;
            trackMixerControl.Library = this.Library;
            trackMixerControl.PlaylistControl = this.PlaylistControl;

            samplerControl.Initialize();
            trackMixerControl.Initialize();

            this.SamplerControl.RefreshSamples();
        }

        public void LoadSettings()
        {
            samplerControl.LoadSettings();
            trackMixerControl.LoadSettings();
            spllLeftRight.SplitterDistance = (this.Width / 4) * 2;
        }

        public void Unload()
        {
            samplerControl.Unload();
        }
    }
}