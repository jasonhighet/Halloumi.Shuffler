using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halloumi.BassEngine
{
    public class SampleTrigger
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

        public string SampleID { get; set; }

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
        internal int StartSyncID { get; set; }

        /// <summary>
        /// Gets or sets the end track fx sync handle
        /// </summary>
        internal int EndSyncID { get; set; }

        /// <summary>
        /// Initializes a new instance of the AutomatedSample class.
        /// </summary>
        public SampleTrigger()
        {
            StartSyncID = int.MinValue;
            EndSyncID = int.MinValue;
            Start = 0;
            Length = 0;
            DelayNotes = 0.25M;
            StartSample = 0;
            EndSample = 0;
        }
    }
}
