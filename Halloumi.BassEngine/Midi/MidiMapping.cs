using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halloumi.Shuffler.AudioEngine.Midi
{
    public class MidiMapping
    {
        public string DeviceName { get; set; }
        public Dictionary<string, string> Commands { get; set; }
        public Dictionary<string, Dictionary<int, string>> VstCommands { get; set; }
        public Dictionary<string, int> Controls { get; set; }
    }
}
