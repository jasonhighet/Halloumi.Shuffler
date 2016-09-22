using System;
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
            var targetBpm = 97M;

            ChannelHelper.InitialiseAudioEngine(Handle);

            var speakers = new SpeakerOutputChannel();
            _player = new AudioPlayer();

            speakers.AddInputChannel(_player.Output);

            var loopLength = BpmHelper.GetDefaultLoopLength(targetBpm) * 4;
            _player.Load("Silence", SilenceHelper.GetSilenceAudioFile());
            _player.AddSection("Silence", "Silence", 0, loopLength, bpm: targetBpm, loopIndefinitely: true);

            _player.Load("VivaTirado",
                @"E:\Music\Library\100% Dynamite!\300% Dynamite!\14 - Various - Augustus Pablo  Viva Tirado.mp3");
            _player.AddSection("VivaTirado", "Loop1", 1.69, 2.545 * 2, targetBpm: targetBpm, loopIndefinitely: true);
            _player.AddSection("VivaTirado", "Loop2", 11.862, 10.053 /2, targetBpm: targetBpm, loopIndefinitely: true);
            _player.AddSection("VivaTirado", "Loop3", 31.865, 9.952 /2 , targetBpm: targetBpm, loopIndefinitely: true);


            _player.Load("JawBreaks05",
                @"E:\Music\Library\Samples\DrumLoops.Breaks\Jaw Breaks\05 - DrumLoops.Breaks - Jaw Breaks 05.mp3");
            _player.AddSection("JawBreaks05", "MainBreak", 6.35, 1.19, targetBpm: targetBpm, loopIndefinitely:true);

            double loop1Start = 0.0D;
            _player.AddPlayEvent("Silence", loop1Start, "JawBreaks05", "MainBreak");
            _player.AddPlayEvent("Silence", loop1Start, "VivaTirado", "Loop1");

            double loop2Start = BpmHelper.GetDefaultLoopLength(targetBpm);
            _player.AddPlayEvent("Silence", loop2Start, "VivaTirado", "Loop2");
            _player.AddPlayEvent("Silence", loop2Start, "JawBreaks05", "MainBreak");

            double loop3Start = BpmHelper.GetDefaultLoopLength(targetBpm) * 2;
            _player.AddPlayEvent("Silence", loop3Start, "VivaTirado", "Loop3");
            _player.AddPlayEvent("Silence", loop3Start, "JawBreaks05", "MainBreak");

            double loop4Start = BpmHelper.GetDefaultLoopLength(targetBpm) * 3;
            _player.AddPlayEvent("Silence", loop4Start, "VivaTirado", "Loop2");
            _player.AddPlayEvent("Silence", loop4Start, "JawBreaks05", "MainBreak");


            _player.QueueSection("Silence", "Silence");
            _player.Play("Silence");
        }

        private double GetLength(decimal bpm, int noteDivisor, int noteCount)
        {
            return BpmHelper.GetDefaultLoopLength(bpm)/(double) noteDivisor*(double) noteCount;
        }


    }

}