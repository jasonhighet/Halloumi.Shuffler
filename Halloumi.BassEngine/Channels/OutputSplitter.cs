namespace Halloumi.BassEngine.Channels
{
    public class OutputSplitter
    {
        public MixerChannel SpeakerMixerChannel { get; private set; }

        public MixerChannel MonitorMixerChannel { get; private set; }

        public OutputSplitter(MixerChannel inputChannel, SpeakerOutputChannel speakerChannel, MonitorOutputChannel monitorChannel)
        {
            //if (inputChannel.OutputType != MixerChannelOutputType.MultipleOutputs)
            //    throw new Exception("Multiple outputs required");

            SpeakerMixerChannel = new MixerChannel(inputChannel.BpmProvider, MixerChannelOutputType.SingleOutput);
            SpeakerMixerChannel.AddInputChannel(inputChannel);
            speakerChannel.AddInputChannel(SpeakerMixerChannel);

            MonitorMixerChannel = new MixerChannel(inputChannel.BpmProvider, MixerChannelOutputType.SingleOutput);
            MonitorMixerChannel.AddInputChannel(inputChannel);
            monitorChannel.AddInputChannel(MonitorMixerChannel);

            SoundOutput = SoundOutput.Speakers;
        }

        /// <summary>
        /// Gets or sets the sampler output.
        /// </summary>
        public SoundOutput SoundOutput
        {
            get { return _soundOutput; }
            set
            {
                if (value == SoundOutput.Monitor)
                {
                    SpeakerMixerChannel.SetVolume(0);
                    MonitorMixerChannel.SetVolume(100);
                }
                else if (value == SoundOutput.Speakers)
                {
                    SpeakerMixerChannel.SetVolume(100);
                    MonitorMixerChannel.SetVolume(0);
                }
                else if (value == SoundOutput.Both)
                {
                    SpeakerMixerChannel.SetVolume(100);
                    MonitorMixerChannel.SetVolume(100);
                }
                _soundOutput = value;
            }
        }

        private SoundOutput _soundOutput = SoundOutput.Speakers;
    }

    public enum SoundOutput
    {
        Speakers,
        Monitor,
        Both
    }
}