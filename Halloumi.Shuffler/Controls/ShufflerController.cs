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
using Halloumi.Common.Windows.Controls;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.Engine;
using Halloumi.Shuffler.Forms;
using Un4seen.Bass;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Controls
{
    public partial class ShufflerController : Component
    {
        /// <summary>
        /// Gets or sets the track selector.
        /// </summary>
        private TrackSelector TrackSelector { get; set; }

        /// <summary>
        /// Initializes a new instance of the ShufflerController class.
        /// </summary>
        public ShufflerController()
        {
            InitializeComponent();
        }

        /// <summary>
        ///// Initializes a new instance of the ShufflerController class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ShufflerController(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            TracksRemainingThreshold = 2;
            AutoGenerateEnabled = false;
        }

        /// <summary>
        /// Initalizes this instance.
        /// </summary>
        public void Initalize()
        {
            TrackSelector = new TrackSelector();
            BassPlayer.OnSkipToEnd += new EventHandler(BassPlayer_OnFadeEnded);
            BassPlayer.OnEndFadeIn += new EventHandler(BassPlayer_OnFadeEnded);
        }

        /// <summary>
        /// Handles the OnEndFadeIn event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnFadeEnded(object sender, EventArgs e)
        {
            if (PlaylistControl.InvokeRequired)
            {
                PlaylistControl.BeginInvoke(new MethodInvoker(delegate()
                {
                    BassPlayer_OnFadeEnded();
                }));
            }
            else BassPlayer_OnFadeEnded();
        }

        /// <summary>
        /// Handles the OnEndFadeIn event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnFadeEnded()
        {
            if (!AutoGenerateEnabled) return;
            var tracksRemaining = PlaylistControl.GetNumberOfTracksRemaining();
            if (tracksRemaining < TracksRemainingThreshold)
            {
                AutoGeneratePlaylist();
            }
        }

        private void AutoGeneratePlaylist()
        {
            using (var generatePlaylist = new FrmGeneratePlaylist())
            {
                generatePlaylist.LibraryControl = LibraryControl;
                generatePlaylist.PlaylistControl = PlaylistControl;
                generatePlaylist.SetScreenMode(FrmGeneratePlaylist.ScreenMode.AutoGeneratePlaylist);
                generatePlaylist.ShowDialog();
            }
        }

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TrackLibraryControl LibraryControl { get; set; }

        /// <summary>
        /// Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BE.BassPlayer BassPlayer { get; set; }

        /// <summary>
        /// Gets the playlist control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PlaylistControl PlaylistControl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether playlist automatic generating is on.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoGenerateEnabled { get; set; }

        /// <summary>
        /// Gets or sets the number of tracks remaining in the playlist before it should auto-generate more.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TracksRemainingThreshold { get; set; }

        /// <summary>
        /// Automatically generates the playlist now.
        /// </summary>
        public void AutoGenerateNow()
        {
            AutoGeneratePlaylist();
        }
    }
}