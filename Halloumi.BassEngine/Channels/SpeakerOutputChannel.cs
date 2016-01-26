using Halloumi.BassEngine.Helpers;
using Un4seen.Bass;

namespace Halloumi.BassEngine.Channels
{
    public class SpeakerOutputChannel : Channel
    {
        public SpeakerOutputChannel()
            : base(null)
        {
            InternalChannel = ChannelHelper.IntialiseOutputChannel();
            Bass.BASS_ChannelPlay(InternalChannel, false);
        }
    }
}