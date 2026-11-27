using System;
using System.Linq;
using System.Runtime.InteropServices;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Channels
{
    public class SpeakerOutputChannel : Channel
    {
        private const int SampleRate = 44100;

        private readonly MonoDownmix _monoDownmix;

        public SpeakerOutputChannel()
            : base(null)
        {
            var mainDevice = AudioEngineHelper.GetMainDevice();
            ChannelId = ChannelHelper.InitializeOutputChannel(mainDevice.Id);

            // Runs after every other DSP on the channel (except the tap), so VST and WinAmp plug-ins still
            // see stereo and only the final output is folded down.
            _monoDownmix = new MonoDownmix(ChannelId, OutputTap.Priority + 1);

            // last of all, so meters and visuals see exactly what is played
            Tap = new OutputTap(ChannelId, SampleRate, GetBufferedFrames);
        }

        /// <summary>
        ///     The copy of the output that level meters and visuals read, instead of reading this channel.
        /// </summary>
        public OutputTap Tap { get; }

        /// <summary>
        ///     The output is mixed this many frames ahead of what is heard: what BASS has buffered for the device.
        /// </summary>
        private int GetBufferedFrames()
        {
            var bytes = Bass.BASS_ChannelGetData(ChannelId, IntPtr.Zero, (int)BASSData.BASS_DATA_AVAILABLE);
            return bytes <= 0 ? 0 : bytes / (sizeof(float) * 2);
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
