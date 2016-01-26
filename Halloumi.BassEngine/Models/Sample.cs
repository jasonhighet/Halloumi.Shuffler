using System.Diagnostics.CodeAnalysis;
using Halloumi.BassEngine.Helpers;

namespace Halloumi.BassEngine.Models
{
    /// <summary>
    ///     Represents a playable mp3 file
    /// </summary>
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class Sample : AudioStream
    {
        /// <summary>
        ///     The BPM at the start of the song
        /// </summary>
        private decimal _bpm = -1;

        /// <summary>
        ///     Initializes a new instance of the sample class.
        /// </summary>
        public Sample()
        {
            SampleEndSyncId = int.MinValue;
            IsLooped = false;

            LinkedTrackDescription = "";
            SampleKey = "None";
        }


        /// <summary>
        ///     Gets or sets the sample description
        /// </summary>
        public override string Description { get; set; }

        /// <summary>
        ///     Gets or sets the BPM.
        /// </summary>
        public override decimal Bpm
        {
            get { return _bpm != -1 ? _bpm : BpmHelper.GetBpmFromLoopLength(LengthSeconds); }
            set { _bpm = value; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this sample is looped.
        /// </summary>
        public bool IsLooped { get; set; }

        /// <summary>
        ///     Gets or sets the sample end sync handle
        /// </summary>
        internal int SampleEndSyncId { get; set; }

        public string LinkedTrackDescription { get; set; }

        public string SampleKey { get; set; }

        public string SampleId => LinkedTrackDescription + " - " + SampleKey;
    }
}