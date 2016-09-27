using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halloumi.Shuffler.AudioEngine.Players
{
    public class Event
    {
        public EventType StreamEventType { get; set; }

        public string TargetStreamKey { get; set; }

        public string TargetSectionKey { get; set; }

        internal int SyncId { get; set; }

        internal AudioPlayer Player { get; set; }
        public string StreamKey { get; internal set; }
    }
}
