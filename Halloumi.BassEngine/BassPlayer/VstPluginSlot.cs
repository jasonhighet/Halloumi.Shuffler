using System.Collections.Generic;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     The mixer channels that can host VST plug-ins.
    /// </summary>
    public enum PluginChannel
    {
        /// <summary>The speaker output (everything).</summary>
        Main,

        /// <summary>The sampler mixer.</summary>
        Sampler,

        /// <summary>The track mixer (current and previous tracks).</summary>
        Tracks,

        /// <summary>The track FX send, routed back into the speaker output.</summary>
        TrackSendFx
    }

    /// <summary>
    ///     One VST plug-in slot: a channel and a position in that channel's plug-in chain.
    /// </summary>
    public struct VstPluginSlot
    {
        public VstPluginSlot(PluginChannel channel, int index)
        {
            Channel = channel;
            Index = index;
        }

        public PluginChannel Channel { get; }

        public int Index { get; }

        /// <summary>
        ///     The seven slots BassPlayer offers: two on the main output, two on the sampler, one on
        ///     the tracks and two on the track FX send.
        /// </summary>
        public static readonly IReadOnlyList<VstPluginSlot> All = new[]
        {
            new VstPluginSlot(PluginChannel.Main, 0),
            new VstPluginSlot(PluginChannel.Main, 1),
            new VstPluginSlot(PluginChannel.Sampler, 0),
            new VstPluginSlot(PluginChannel.Sampler, 1),
            new VstPluginSlot(PluginChannel.Tracks, 0),
            new VstPluginSlot(PluginChannel.TrackSendFx, 0),
            new VstPluginSlot(PluginChannel.TrackSendFx, 1)
        };

        public override string ToString()
        {
            return Channel + " " + (Index + 1);
        }
    }
}
