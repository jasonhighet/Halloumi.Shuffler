using System;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Players;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace Halloumi.Shuffler.TestHarness
{
    public partial class Form1 : Form
    {
        //private AudioPlayer _player;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var module =  JsonConvert.DeserializeObject<Module>(File.ReadAllText("song.json"));
            var targetBpm = module.Bpm;
            var loopLength = BpmHelper.GetDefaultLoopLength(targetBpm);

            ChannelHelper.InitialiseAudioEngine(Handle);
            var speakers = new SpeakerOutputChannel();


            var channelPlayers = new List<AudioPlayer>();
            for (var channelIndex  = 0; channelIndex < module.Channels.Count; channelIndex++)
            {
                var channelPlayer = new AudioPlayer();
                speakers.AddInputChannel(channelPlayer.Output);
                channelPlayers.Add(channelPlayer);

                foreach (var audioFile in module.AudioFiles)
                {
                    foreach (var sample in audioFile.Samples)
                    {
                        var fullSampleKey = audioFile.Key + "." + sample.Key;
                        channelPlayer.Load(fullSampleKey, audioFile.Path);
                        channelPlayer.AddSection(fullSampleKey,
                            fullSampleKey,
                            sample.Start,
                            sample.End,
                            sample.Offset ?? 0,
                            targetBpm: module.Bpm);
                    }
                }
            }
            foreach (var pattern in module.Patterns)
            {
                var columnIndex = 0;
                foreach (var column in pattern.Sequence)
                {
                    var player = channelPlayers.FirstOrDefault(x => channelPlayers.IndexOf(x) == columnIndex);
                    if(player == null)
                        continue;

                    var columnControlKey = pattern.Key + columnIndex;
                    player.Load(columnControlKey, SilenceHelper.GetSilenceAudioFile());
                    player.AddSection(columnControlKey, columnControlKey, 0, loopLength, bpm: targetBpm);

                    var rowIndex = 0;
                    foreach (var row in column)
                    {
                        var loopPosition = 0.0;
                        if (rowIndex > 0 )
                            loopPosition = rowIndex / (double)column.Count;

                        var position = GetLoopPosition(loopPosition, targetBpm);

                        player.AddPlayEvent(columnControlKey, position, row, row);

                        rowIndex++;
                    }

                    columnIndex++;
                }
            }
           

            var mainPlayer = new AudioPlayer();
            speakers.AddInputChannel(mainPlayer.Output);
            mainPlayer.Load("Song", SilenceHelper.GetSilenceAudioFile());
            var songLength = loopLength * module.Sequence.Count;
            mainPlayer.AddSection("Song", "Song", 0, songLength, bpm: targetBpm);

            var patternIndex = 0;
            foreach (var patternKey in module.Sequence)
            {
                var position = GetLoopPosition(patternIndex, targetBpm);
                var pattern = module.Patterns.FirstOrDefault(x => x.Key == patternKey);
                if (pattern == null)
                    continue;

                for (var columnIndex = 0; columnIndex < pattern.Sequence.Count; columnIndex++)
                {
                    var player = channelPlayers[columnIndex];
                    var columnControlKey = patternKey + columnIndex;
                    mainPlayer.AddEvent(player, "Song", position, columnControlKey, columnControlKey, EventType.Play);
                }
                patternIndex++;
            }



            mainPlayer.Play("Song");



            //_player = new AudioPlayer();
            //speakers.AddInputChannel(_player.Output);

            //var length = BpmHelper.GetDefaultLoopLength(targetBpm) * 6;
            //_player.Load("Silence", SilenceHelper.GetSilenceAudioFile());
            //_player.AddSection("Silence", "Silence", 0, length, bpm: targetBpm, loopIndefinitely: true);

            //_player.Load("JawBreaks05",
            //    @"E:\Music\Library\Samples\DrumLoops.Breaks\Jaw Breaks\05 - DrumLoops.Breaks - Jaw Breaks 05.mp3");
            //_player.AddSection("JawBreaks05", "MainBreak", 6.35, 1.19, targetBpm: targetBpm, loopIndefinitely: true);

            //var vivaTirado = @"E:\Music\Library\100% Dynamite!\300% Dynamite!\14 - Various - Augustus Pablo  Viva Tirado.mp3";
            //_player.Load("VivaTirado1", vivaTirado);
            //_player.Load("VivaTirado2", vivaTirado);
            //_player.Load("VivaTirado3", vivaTirado);

            //_player.Load("VivaTirado4", vivaTirado);
            //_player.Load("VivaTirado5", vivaTirado);
            //_player.Load("VivaTirado6", vivaTirado);

            //_player.Load("Chorus1", vivaTirado);
            //_player.Load("Chorus2", vivaTirado);
            //_player.Load("Chorus3", vivaTirado);


            //_player.AddSection("VivaTirado1", "Loop1", 1.730, 5.09, targetBpm: targetBpm, loopIndefinitely: true);
            //_player.AddSection("VivaTirado2", "Loop2", 2.718, 5.107, 6.820, targetBpm: targetBpm, loopIndefinitely: true);
            //_player.AddSection("VivaTirado3", "Loop3", 6.810, 5.041, targetBpm: targetBpm, loopIndefinitely: true);

            //_player.AddSection("VivaTirado4", "Loop4", 11.851, 5.021, targetBpm: targetBpm, loopIndefinitely: true);
            //_player.AddSection("VivaTirado5", "Loop5", 16.872, 5.005, targetBpm: targetBpm, loopIndefinitely: true);
            //_player.AddSection("VivaTirado6", "Loop6", 26.911, 5.006, targetBpm: targetBpm, loopIndefinitely: true);

            //_player.AddSection("Chorus1", "Chorus1",31.875, 4.975, targetBpm: targetBpm, loopIndefinitely: true);
            //_player.AddSection("Chorus2", "Chorus2", 36.83, 4.975, targetBpm: targetBpm, loopIndefinitely: true);
            //_player.AddSection("Chorus3", "Chorus3", 46.77, 4.9, targetBpm: targetBpm, loopIndefinitely: true);



            //double position = 0.0D;
            ////_player.AddPlayEvent("Silence", position, "Chorus3", "Chorus3");
            ////_player.AddPlayEvent("Silence", position, "JawBreaks05", "MainBreak");



            //_player.AddEvent("Silence", position, "Chorus3", "Chorus3", EventType.Pause);
            //_player.AddPlayEvent("Silence", position, "VivaTirado1", "Loop1");
            //_player.AddPlayEvent("Silence", position, "JawBreaks05", "MainBreak");

            //position = GetLoopPosition(0.5, targetBpm);
            //_player.AddEvent("Silence", position, "VivaTirado1", "Loop1", EventType.Pause);
            //_player.AddPlayEvent("Silence", position, "VivaTirado2", "Loop2");
            //_player.AddPlayEvent("Silence", position, "JawBreaks05", "MainBreak");

            //position = GetLoopPosition(1, targetBpm);
            //_player.AddEvent("Silence", position, "VivaTirado2", "Loop2", EventType.Pause);
            //_player.AddPlayEvent("Silence", position, "VivaTirado3", "Loop3");
            //_player.AddPlayEvent("Silence", position, "JawBreaks05", "MainBreak");

            //position = GetLoopPosition(1.5, targetBpm);
            //_player.AddEvent("Silence", position, "VivaTirado3", "Loop3", EventType.Pause);
            //_player.AddPlayEvent("Silence", position, "VivaTirado2", "Loop2");
            //_player.AddPlayEvent("Silence", position, "JawBreaks05", "MainBreak");


            //position = GetLoopPosition(2, targetBpm);
            //_player.AddEvent("Silence", position, "VivaTirado2", "Loop2", EventType.Pause);
            //_player.AddPlayEvent("Silence", position, "VivaTirado4", "Loop4");
            //_player.AddPlayEvent("Silence", position, "JawBreaks05", "MainBreak");

            //position = GetLoopPosition(2.5, targetBpm);
            //_player.AddEvent("Silence", position, "VivaTirado4", "Loop4", EventType.Pause);
            //_player.AddPlayEvent("Silence", position, "VivaTirado5", "Loop5");
            //_player.AddPlayEvent("Silence", position, "JawBreaks05", "MainBreak");

            //position = GetLoopPosition(3, targetBpm);
            //_player.AddEvent("Silence", position, "VivaTirado5", "Loop5", EventType.Pause);
            //_player.AddPlayEvent("Silence", position, "VivaTirado4", "Loop4");
            //_player.AddPlayEvent("Silence", position, "JawBreaks05", "MainBreak");

            //position = GetLoopPosition(3.5, targetBpm);
            //_player.AddEvent("Silence", position, "VivaTirado4", "Loop4", EventType.Pause);
            //_player.AddPlayEvent("Silence", position, "VivaTirado6", "Loop6");
            //_player.AddPlayEvent("Silence", position, "JawBreaks05", "MainBreak");




            //position = GetLoopPosition(4, targetBpm);
            //_player.AddEvent("Silence", position, "VivaTirado6", "Loop6", EventType.Pause);
            //_player.AddPlayEvent("Silence", position, "Chorus1", "Chorus1");
            //_player.AddPlayEvent("Silence", position, "JawBreaks05", "MainBreak");

            //position = GetLoopPosition(4.5, targetBpm);
            //_player.AddEvent("Silence", position, "Chorus1", "Chorus1", EventType.Pause);
            //_player.AddPlayEvent("Silence", position, "Chorus2", "Chorus2");
            //_player.AddPlayEvent("Silence", position, "JawBreaks05", "MainBreak");

            //position = GetLoopPosition(5, targetBpm);
            //_player.AddEvent("Silence", position, "Chorus2", "Chorus2", EventType.Pause);
            //_player.AddPlayEvent("Silence", position, "Chorus1", "Chorus1");
            //_player.AddPlayEvent("Silence", position, "JawBreaks05", "MainBreak");

            //position = GetLoopPosition(5.5, targetBpm);
            //_player.AddEvent("Silence", position, "Chorus1", "Chorus1", EventType.Pause);
            //_player.AddPlayEvent("Silence", position, "Chorus3", "Chorus3");
            //_player.AddPlayEvent("Silence", position, "JawBreaks05", "MainBreak");


            //_player.QueueSection("Silence", "Silence");
            //_player.Play("Silence");
        }

        private double GetLoopPosition(double loopCount, decimal bpm)
        {
            return BpmHelper.GetDefaultLoopLength(bpm) * loopCount;
        }


    }

}