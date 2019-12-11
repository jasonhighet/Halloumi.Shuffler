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
                DSPPriority = -1000,
                StreamCopyDevice = monitorDevice.Id
            };

            streamCopy.StreamCopyFlags = streamCopy.ChannelInfo.flags;
            streamCopy.Start();
        }
    }
}