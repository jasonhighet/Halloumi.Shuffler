﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Halloumi.BassEngine.Models;

namespace Halloumi.BassEngine.Players
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    internal class AudioSection
    {
        public AudioSection()
        {
            AudioSyncs = new List<AudioSync>();
        }

        public List<AudioSync> AudioSyncs { get; }

        public AudioSync Start => AudioSyncs.FirstOrDefault(x => x.SyncType == SyncType.Start);

        public AudioSync End => AudioSyncs.FirstOrDefault(x => x.SyncType == SyncType.End);

        public AudioSync Offset => AudioSyncs.FirstOrDefault(x => x.SyncType == SyncType.Offset);

        public bool HasOffset => Offset != null;

        /// <summary>
        ///     Gets or sets a value indicating whether this section should loop indefinitely.
        /// </summary>
        public bool LoopIndefinitely { get; set; }
    }
}