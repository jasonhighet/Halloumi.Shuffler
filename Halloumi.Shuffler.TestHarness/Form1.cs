using System;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.Misc;

namespace Halloumi.Shuffler.TestHarness
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DebugHelper.DebugMode = true;

            AudioEngineHelper.StartAudioEngine(Handle);

            var monitorDevice = AudioEngineHelper.GetMonitorDevice() ?? AudioEngineHelper.GetMainDevice();
            Bass.BASS_SetDevice(AudioEngineHelper.GetMainDevice().Id);


            //var channelId = Bass.BASS_StreamCreateFile(@"D:\Jason\Music\Library\Beastwars\Beastwars\01 - Beastwars - Damn the Sky.mp3", 0, 0, BASSFlag.BASS_SAMPLE_FLOAT);

            //var file = AudioStreamHelper.LoadAudio()


            //var mixerChannel = new MixerChannel();
            

            //var output = new SpeakerOutputChannel();
            //output.AddInputChannel(mixerChannel);









            //var info = Bass.BASS_GetInfo();

            //var streamCopy = new DSP_StreamCopy
            //{
            //    OutputLatency = info.latency,
            //    ChannelHandle = channelId,
            //    SourceMixerStream = mixerId,
            //    DSPPriority = -1000,
            //    StreamCopyDevice = monitorDevice.Id
            //};
            //streamCopy.StreamCopyFlags = streamCopy.ChannelInfo.flags;



            //Bass.BASS_ChannelPlay(channelId, false);
            //streamCopy.Start();
        }
    }
}