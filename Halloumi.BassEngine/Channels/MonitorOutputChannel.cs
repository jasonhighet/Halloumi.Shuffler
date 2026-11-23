using System;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Vst;
using Un4seen.Bass.Misc;

namespace Halloumi.Shuffler.AudioEngine.Channels
{
    public class MonitorOutputChannel : Channel
    {
        /// <summary>
        ///     The priority of the DSP that copies this channel to the monitor device.
        /// </summary>
        private const int StreamCopyPriority = -1000;

        private readonly MonoDownmix _monoDownmix;

        public MonitorOutputChannel()
            : base(null)
        {
            var monitorDevice = AudioEngineHelper.GetMonitorDevice() ?? AudioEngineHelper.GetMainDevice();

            ChannelId = ChannelHelper.InitializeMixerChannel();
            var info = Bass.BASS_GetInfo();

            var streamCopy = new DSP_StreamCopy
            {
                OutputLatency = info.latency,
                ChannelHandle = ChannelId,
                DSPPriority = StreamCopyPriority,
                StreamCopyDevice = monitorDevice.Id
            };

            streamCopy.StreamCopyFlags = streamCopy.ChannelInfo.flags;
            streamCopy.Start();

            // Must run before the stream copy, or the monitor device receives the stereo signal.
            _monoDownmix = new MonoDownmix(ChannelId, StreamCopyPriority + 1);
        }

        /// <summary>
        ///     Gets or sets whether the monitor output is folded down to mono (left and right averaged).
        /// </summary>
        public bool Mono
        {
            get => _monoDownmix.Enabled;
            set => _monoDownmix.Enabled = value;
        }
    }
}