using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Halloumi.Shuffler.AudioEngine.Models
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
        ///     How many whole fade-out sections the outgoing track's fade-out was moved when this mix was recorded
        ///     (negative is earlier). Moved on the Mixer view's waveform strip; see docs/mixer-waveform.md.
        /// </summary>
        public int FadeOutShift { get; set; }

        /// <summary>
        ///     How many whole fade-in sections the incoming track's fade-in was moved when this mix was recorded.
        /// </summary>
        public int FadeInShift { get; set; }

        /// <summary>
        ///     True if this holds a recorded (manual) mix. An entry can also hold just sample triggers, which leave
        ///     the transition itself as normal.
        /// </summary>
        [XmlIgnore]
        public bool IsRecordedMix => FadeLength != 0;

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

        public ExtendedMixAttributes Clone()
        {
            return new ExtendedMixAttributes()
            {
                SampleTriggers = SampleTriggers.Select(x => x.Clone()).ToList(),
                ExtendedFadeType = ExtendedFadeType,
                FadeEnd = FadeEnd,
                FadeEndLoop = FadeEndLoop,
                FadeEndVolume = FadeEndVolume,
                FadeLength = FadeLength,
                PowerDownAfterFade = PowerDownAfterFade,
                TrackDescription = TrackDescription,
                FadeOutShift = FadeOutShift,
                FadeInShift = FadeInShift
            };
        }
    }
}