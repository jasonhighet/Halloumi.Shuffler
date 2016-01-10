using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halloumi.BassEngine
{
    public class ExtendedMix
    {
        /// <summary>
        /// Gets or sets the track description of the track to mix in to.
        /// </summary>
        public string TrackDescription { get; set; }

        /// <summary>
        /// Gets the length of the fade-out section in seconds
        /// </summary>
        public double FadeLength { get; set; }
        
        /// <summary>
        /// Gets or sets the fade end sample.
        /// </summary>
        internal long FadeEnd { get; set; }

        /// <summary>
        /// Gets or sets the fade end sample.
        /// </summary>
        internal long FadeEndLoop { get; set; }

        /// <summary>
        /// Gets or sets the final volume level at the end of the fade section as a percentage (0 - 1)
        /// </summary>
        public float FadeEndVolume { get; set; }

        /// <summary>
        /// If true, a 'power-down' noise will be played at the end of the fade
        /// </summary>
        public bool PowerDownOnEnd { get; set; }
    }
}
