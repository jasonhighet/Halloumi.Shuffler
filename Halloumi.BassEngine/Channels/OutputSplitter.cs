using System;

namespace Halloumi.BassEngine.Channels
{
    public class OutputSplitter
    {
        private SoundOutput _soundOutput = SoundOutput.Speakers;

        public OutputSplitter(MixerChannel inputChannel, Channel speakerChannel,
            Channel monitorChannel)
        {
            SpeakerMixerChannel = new MixerChannel(MixerChannelOutputType.SingleOutput, inputChannel.BpmProvider);
            SpeakerMixerChannel.AddInputChannel(inputChannel);
            speakerChannel.AddInputChannel(SpeakerMixerChannel);

            MonitorMixerChannel = new MixerChannel(MixerChannelOutputType.SingleOutput, inputChannel.BpmProvider);
            MonitorMixerChannel.AddInputChannel(inputChannel);
            monitorChannel.AddInputChannel(MonitorMixerChannel);

            SoundOutput = SoundOutput.Speakers;
        }

        public MixerChannel SpeakerMixerChannel { get; }

        public MixerChannel MonitorMixerChannel { get; }

        /// <summary>
        ///     Gets or sets the sampler output.
        /// </summary>
        public SoundOutput SoundOutput
        {
            get { return _soundOutput; }
            set
            {
                switch (value)
                {
                    case SoundOutput.Monitor:
                        SpeakerMixerChannel.SetVolume(0);
                        MonitorMixerChannel.SetVolume(100);
                        break;
                    case SoundOutput.Speakers:
                        SpeakerMixerChannel.SetVolume(100);
                        MonitorMixerChannel.SetVolume(0);
                        break;
                    case SoundOutput.Both:
                        SpeakerMixerChannel.SetVolume(100);
                        MonitorMixerChannel.SetVolume(100);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
                _soundOutput = value;
            }
        }
    }

    public enum SoundOutput
    {
        Speakers,
        Monitor,
        Both
    }
}