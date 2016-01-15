using System.Collections.Generic;
using System.Xml.Serialization;

namespace Halloumi.BassEngine
{
    public class ExtendedMixAttributes
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
        public long FadeEnd { get; set; }

        /// <summary>
        /// Gets or sets the fade end sample.
        /// </summary>
        public long FadeEndLoop { get; set; }

        /// <summary>
        /// Gets or sets the final volume level at the end of the fade section as a percentage (0 - 1)
        /// </summary>
        public float FadeEndVolume { get; set; }

        /// <summary>
        /// If true, a 'power-down' noise will be played at the end of the fade
        /// </summary>
        [XmlElement(ElementName = "PowerDownOnEnd")]
        public bool PowerDownAfterFade { get; set; }

        public ExtendedFadeType ExtendedFadeType { get; set; }

        /// <summary>
        /// Gets or sets the sample triggers.
        /// </summary>
        public List<SampleTrigger> SampleTriggers { get; set; }

        /// <summary>
        /// Initializes a new instance of the ExtendedMixAttributes class.
        /// </summary>
        public ExtendedMixAttributes()
        {
            SampleTriggers = new List<SampleTrigger>();
            FadeLength = 0;
            FadeEnd = 0;
            FadeEndLoop = 0;
            PowerDownAfterFade = false;
            ExtendedFadeType = ExtendedFadeType.Default;
        }
    }
}