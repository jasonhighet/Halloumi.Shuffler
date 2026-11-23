using System;
using System.Linq;
using System.Runtime.InteropServices;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Channels
{
    public class SpeakerOutputChannel : Channel
    {
        private readonly MonoDownmix _monoDownmix;

        public SpeakerOutputChannel()
            : base(null)
        {
            var mainDevice = AudioEngineHelper.GetMainDevice();
            ChannelId = ChannelHelper.InitializeOutputChannel(mainDevice.Id);

            // Runs after every other DSP on the channel, so VST and WinAmp plug-ins still see stereo
            // and only the final output is folded down.
            _monoDownmix = new MonoDownmix(ChannelId, int.MinValue);
        }

        /// <summary>
        ///     Gets or sets whether the output is folded down to mono (left and right averaged).
        /// </summary>
        public bool Mono
        {
            get => _monoDownmix.Enabled;
            set => _monoDownmix.Enabled = value;
        }
    }
}
