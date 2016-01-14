using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.Controls;
using Halloumi.Shuffler.Engine;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Forms.TrackPlayerExtensions
{
    public class TrackPlayerExtensionForm : BaseForm
    {
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
        /// Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MixLibrary MixLibrary
        {
            get { return PlaylistControl.MixLibrary; }
        }

        /// <summary>
        /// Gets or sets the playlist control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PlaylistControl PlaylistControl { get; set; }
    }
}