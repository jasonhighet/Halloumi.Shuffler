using System.Collections.Generic;

namespace Halloumi.Shuffler.AudioEngine.Plugins
{
    /// <summary>
    ///     Represents a VST plug-in.
    /// </summary>
    public class VstPlugin
    {
        public int EditorHeight;
        public int EditorWidth;
        public VstPluginConfigForm Form;
        public int Id;
        public string Location;
        public string Name;
        public List<VstPluginParameter> Parameters;

        public class VstPluginParameter
        {
            /// <summary>
            ///     The identifier of the parameter
            /// </summary>
            public int Id;

            /// <summary>
            ///     The sync length in milliseconds when the VST knob is set to 100
            ///     Used to convert the current BPM -> milliseconds -> the VST knob value (1-100)
            /// </summary>
            public decimal MaxSyncMilliSeconds;

            /// <summary>
            ///     The sync length in milliseconds when the VST knob is set to 0.
            ///     Used to convert the current BPM -> milliseconds -> the VST knob value (1-100)
            /// </summary>
            public decimal MinSyncMilliSeconds;

            /// <summary>
            ///     The name of the parameter
            /// </summary>
            public string Name;

            /// <summary>
            ///     The note length (1, 1/2, 1/4, 1/8 etc) used to calculated the sync length from the BPM
            /// </summary>
            public decimal SyncNotes = 0.25M;

            /// <summary>
            ///     If true, sync note length can be changed using the UI. If false, is fixed
            /// </summary>
            public bool VariableSyncNotes = false;

            /// <summary>
            ///     If true, synchronize this parameter (in milliseconds) to the current BPM
            /// </summary>
            public bool SyncToBpm;

            /// <summary>
            ///     If true, the range between min and mix milliseconds should be calculated using a logarithmic scale
            /// </summary>
            public bool SyncUsingLogScale = false;


        }
    }
}