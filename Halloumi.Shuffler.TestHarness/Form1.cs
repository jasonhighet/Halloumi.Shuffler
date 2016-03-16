using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Players;

namespace Halloumi.Shuffler.TestHarness
{
    public partial class Form1 : Form
    {
        private AudioPlayer _player;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var targetBpm = 123;

            ChannelHelper.InitialiseAudioEngine(Handle);

            var speakers = new SpeakerOutputChannel();
            _player = new AudioPlayer();

            speakers.AddInputChannel(_player.Output);

            _player.OnCustomSync += Player_OnCustomSync;

            var loopLength = BpmHelper.GetDefaultLoopLength(targetBpm);

            _player.Load("Silence", SilenceHelper.GetSilenceAudioFile());
            _player.AddSection("Silence", "Silence", true);
            _player.SetSectionPositions("Silence", "Silence", 0, loopLength);
            _player.SetSectionBpm("Silence", "Silence", calculateBpmFromLength: true, targetBpm: targetBpm);
            _player.AddCustomSync("Silence", 0);
            _player.QueueSection("Silence", "Silence");

            _player.Load("DrumLoop1", @"H:\Music\Samples\DrumLoop1.wav");
             _player.AddSection("DrumLoop1", "DrumLoop1", true);
            _player.SetSectionPositions("DrumLoop1", "DrumLoop1");
            _player.SetSectionBpm("DrumLoop1", "DrumLoop1",calculateBpmFromLength:true, targetBpm:targetBpm);
            _player.QueueSection("DrumLoop1", "DrumLoop1");

            _player.Load("BassLoop", @"H:\Music\Samples\BassLoop.wav");
            _player.AddSection("BassLoop", "BassLoop", true);
            _player.SetSectionPositions("BassLoop", "BassLoop");
            _player.SetSectionBpm("BassLoop", "BassLoop", calculateBpmFromLength: true, targetBpm: targetBpm);
            _player.QueueSection("BassLoop", "BassLoop");

            _player.Play("Silence");

            //var file2 = @"H:\Music\Samples\BassLoop1.wav";
            //player.Load("BassLoop", file2);


            //var file = @"H:\Music\Pucho & His Latin Soul Brothers\Big Stick Dateline\04 - Pucho & His Latin Soul Brothers - Sunny.mp3";
            //Parallel.For(0, 10, i => 
            //{
            //    player.Load("stream" + i.ToString(), file);

            //    for (int j = 0; j < 10; j++)
            //    {
            //        player.AddSection("stream"+ i.ToString(), "section" + j.ToString());
            //        player.SetSectionPositions("stream" + i.ToString(), "section" + j.ToString(), (j * 10) + i, 3);
            //        player.AddCustomSync("stream" + i.ToString(), (j * 10) + i + 2);
            //    }

            //    player.QueueSection("stream" + i.ToString(), "section0");
            //    player.Play("stream" + i.ToString());
            //});
        }

        private void Player_OnCustomSync(object sender, CustomSyncEventArgs e)
        {
            if (e.StreamKey != "Silence") return;
            var streamKeys = _player.GetStreamKeys().Where(x => x != "Silence").ToList();
            _player.Play(streamKeys);
        }
    }
}