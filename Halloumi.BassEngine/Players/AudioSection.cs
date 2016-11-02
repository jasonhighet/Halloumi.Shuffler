using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.Players
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class AudioSection
    {
        public AudioSection()
        {
            AudioSyncs = new List<AudioSync>();
            TargetBpm = 0;
            Bpm = 100;
        }

        public string Key { get; set; }

        public List<AudioSync> AudioSyncs { get; }

        public AudioSync Start => AudioSyncs.FirstOrDefault(x => x.SyncType == SyncType.Start);

        public AudioSync End => AudioSyncs.FirstOrDefault(x => x.SyncType == SyncType.End);

        public AudioSync Offset => AudioSyncs.FirstOrDefault(x => x.SyncType == SyncType.Offset);

        public bool HasOffset => Offset != null;

        public bool HasStartAndEnd => Start != null && End != null;

        /// <summary>
        ///     Gets or sets a value indicating whether this section should loop indefinitely.
        /// </summary>
        public bool LoopIndefinitely { get; set; }

        public bool AutoPlayNextSection { get; set; }

        /// <summary>
        ///     Gets the BPM.
        /// </summary>
        public decimal Bpm { get; set; }

        /// <summary>
        ///     Gets the target BPM.
        /// </summary>
        public decimal TargetBpm { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public double Length { get; set; }
    }
}