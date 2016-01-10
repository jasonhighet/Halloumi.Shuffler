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
            this.InternalChannel = BassHelper.IntialiseOutputChannel();
            Bass.BASS_ChannelPlay(this.InternalChannel, false);
        }
    }
}