using System.Collections.Generic;

namespace Halloumi.BassEngine.Plugins
{
    /// <summary>
    /// Represents a VST plug-in.
    /// </summary>
    public class VstPlugin
    {
        public int Id;
        public string Name;
        public string Location;
        public VstPluginConfigForm Form;
        public int EditorWidth;
        public int EditorHeight;
        public List<VstPluginParameter> Parameters;

        public class VstPluginParameter
        {
            public int Id;
            public string Name;
        }
    }
}
