using Halloumi.Shuffler.AudioEngine.Helpers;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Channels
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