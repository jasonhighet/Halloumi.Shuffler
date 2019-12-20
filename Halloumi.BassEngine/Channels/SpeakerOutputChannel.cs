using System;
using System.Linq;
using System.Runtime.InteropServices;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Channels
{
    public class SpeakerOutputChannel : Channel
    {
        public SpeakerOutputChannel()
            : base(null)
        {
            var mainDevice = AudioEngineHelper.GetMainDevice();
            ChannelId = ChannelHelper.InitializeOutputChannel(mainDevice.Id);
        }
    }
}