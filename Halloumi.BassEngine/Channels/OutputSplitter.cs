using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            this.SpeakerMixerChannel = new MixerChannel(inputChannel.BpmProvider, MixerChannelOutputType.SingleOutput);
            this.SpeakerMixerChannel.AddInputChannel(inputChannel);
            speakerChannel.AddInputChannel(this.SpeakerMixerChannel);

            this.MonitorMixerChannel = new MixerChannel(inputChannel.BpmProvider, MixerChannelOutputType.SingleOutput);
            this.MonitorMixerChannel.AddInputChannel(inputChannel);
            monitorChannel.AddInputChannel(this.MonitorMixerChannel);

            this.SoundOutput = SoundOutput.Speakers;
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
                    this.SpeakerMixerChannel.SetVolume(0);
                    this.MonitorMixerChannel.SetVolume(100);
                }
                else if (value == SoundOutput.Speakers)
                {
                    this.SpeakerMixerChannel.SetVolume(100);
                    this.MonitorMixerChannel.SetVolume(0);
                }
                else if (value == SoundOutput.Both)
                {
                    this.SpeakerMixerChannel.SetVolume(100);
                    this.MonitorMixerChannel.SetVolume(100);
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