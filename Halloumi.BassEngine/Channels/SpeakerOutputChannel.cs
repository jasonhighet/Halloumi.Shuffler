using Halloumi.Shuffler.AudioEngine.Helpers;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Channels
{
    public class SpeakerOutputChannel : Channel
    {
        public SpeakerOutputChannel()
            : base(null)
        {
            ChannelId = ChannelHelper.IntialiseOutputChannel();
            Bass.BASS_ChannelPlay(ChannelId, false);
        }
    }
}