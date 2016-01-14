using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;

namespace Halloumi.BassEngine.Channels
{
    public class SpeakerOutputChannel : Channel
    {
        public SpeakerOutputChannel()
            : base(null)
        {
            InternalChannel = BassHelper.IntialiseOutputChannel();
            Bass.BASS_ChannelPlay(InternalChannel, false);
        }
    }
}