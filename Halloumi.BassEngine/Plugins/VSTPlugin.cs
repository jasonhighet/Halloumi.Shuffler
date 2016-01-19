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
            public bool SyncToBpm;

            /// <summary>
            /// The delay length in milliseconds when the VST knob is set to 0.
            /// Used to convert the current BPM -> milliseconds -> the VST knob value (1-100)
            /// </summary>
            public int MinSyncMilliSeconds;

            /// <summary>
            /// The delay length in milliseconds when the VST knob is set to 100
            /// Used to convert the current BPM -> milliseconds -> the VST knob value (1-100)
            /// </summary>
            public int MaxSyncMilliSeconds;
        }
    }
}
