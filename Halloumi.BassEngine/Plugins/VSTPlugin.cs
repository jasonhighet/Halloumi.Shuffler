using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halloumi.BassEngine
{
    /// <summary>
    /// Represents a VST plugin.
    /// </summary>
    public class VSTPlugin
    {
        public int ID;
        public string Name;
        public string Location;
        public VSTPluginConfigForm Form;
        public int EditorWidth;
        public int EditorHeight;
    }
}
