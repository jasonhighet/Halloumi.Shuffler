﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halloumi.Shuffler.AudioEngine.Players
{
    public class AudioStreamEvent
    {
        public AudioStreamEventType StreamEventType { get; set; }

        public string StreamKey { get; set; }

        public string SectionKey { get; set; }

        internal int SyncId { get; set; }
    }
}
