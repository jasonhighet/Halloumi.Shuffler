using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Halloumi.Shuffler.AudioEngine.Players
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class AudioSection
    {
        public AudioSection()
        {
            AudioSyncs = new List<AudioSync>();
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
    }
}