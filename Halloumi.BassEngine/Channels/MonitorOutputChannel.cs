using System;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Vst;

namespace Halloumi.Shuffler.AudioEngine.Channels
{
    public class MonitorOutputChannel : Channel
    {
        public MonitorOutputChannel()
            : base(null)
        {
            var monitorDevice = AudioEngineHelper.GetMonitorDevice() ?? AudioEngineHelper.GetMainDevice();
            ChannelId = ChannelHelper.InitializeOutputChannel(monitorDevice.Id);
        }
    }
}