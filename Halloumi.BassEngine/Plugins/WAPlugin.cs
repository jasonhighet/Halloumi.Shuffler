using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;
using System.Drawing;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.AddOn.Tags;
using Un4seen.Bass.AddOn.WaDsp;
using Un4seen.Bass.Misc;
using System.Threading;
using System.Diagnostics;

namespace Halloumi.BassEngine
{
    /// <summary>
    /// Represents a Winamp DSP plugin.
    /// </summary>
    public class WAPlugin
    {
        public int ID;
        public string Name;
        public string Location;
        public int Module;
    }
}
