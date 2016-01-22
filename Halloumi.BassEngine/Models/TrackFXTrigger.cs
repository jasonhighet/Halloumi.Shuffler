using System.Xml.Serialization;

namespace Halloumi.BassEngine.Models
{
    public class TrackFXTrigger
    {
        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        public double Start { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Gets or sets the delay notes.
        /// </summary>
        public decimal DelayNotes { get; set; }

        /// <summary>
        /// Gets or sets the start sample.
        /// </summary>
        internal long StartSample { get; set; }

        /// <summary>
        /// Gets or sets the end sample.
        /// </summary>
        internal long EndSample { get; set; }
        
        /// <summary>
        /// Gets or sets the start track sync handle
        /// </summary>
        internal int StartSyncId { get; set; }
        
        /// <summary>
        /// Gets or sets the end track fx sync handle
        /// </summary>
        internal int EndSyncId { get; set; }

        /// <summary>
        /// Initializes a new instance of the AutomatedTrackFX class.
        /// </summary>
        public TrackFXTrigger()
        {
            StartSyncId = int.MinValue;
            EndSyncId = int.MinValue;
            Start = 0;
            Length = 0;
            DelayNotes = 0.25M;
            StartSample = 0;
            EndSample = 0;
        }
    }
}
