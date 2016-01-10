using System;
using System.Collections.Generic;
using System.Text;

namespace Halloumi.BassEngine.Tracker
{
    public class Song
    {
        public List<Pattern> Patterns { get; set; }
        public List<Channel> Channels { get; set; }
        public List<Instrument> Instruments { get; set; }
    }
}
