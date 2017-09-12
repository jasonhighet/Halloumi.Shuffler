using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.ModulePlayer;
using Halloumi.Shuffler.AudioEngine.Players;
using Halloumi.Shuffler.AudioEngine.Plugins;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.TestHarness
{
    public partial class Form1 : Form
    {
        private BassPlayer _bassPlayer;
        private Library _library;
        private ModulePlayer _modulePlayer;
        private SampleLibrary _sampleLibrary;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DebugHelper.DebugMode = true;

            //TestControlTrack();

            TestModulePlayer();
        }

        private void TestControlTrack()
        {
            var controlTrack =
                @"D:\Music\Library\A Reggae Tribute To The Beatles\14 - Various - Roslyn Sweat & The Paragons  Blackbird.mp3";
            var triggerTrack =
                @"D:\Music\Library\Black Sabbath\Black Sabbath Instrumentals\Black Sabbath - Hand Of Doom (Instrumental).mp3";


            const decimal bpm = 100M;
            var loopLength = BpmHelper.GetDefaultLoopLength(bpm);
            const int loopCount = 2;
            var songLength = loopLength*loopCount;

            _bassPlayer = new BassPlayer(Handle);
            var player = new AudioPlayer();
            _bassPlayer.SpeakerOutput.AddInputChannel(player.Output);


            player.Load("ControlTrack", controlTrack);
            player.AddSection("ControlTrack", "ControlTrack", 0, songLength, bpm: bpm);

            var section = player.GetAudioSection("ControlTrack", "ControlTrack");
            section.LoopIndefinitely = true;

            var stream = player.Load("Triggered", triggerTrack);
            player.AddSection("Triggered", "Triggered", 0, loopLength*2, bpm: bpm);
            stream.DisableSyncs = true;


            var position = 0D;
            for (var i = 0; i < loopCount; i++)
            {
                player.AddEvent("ControlTrack", position, "Triggered", "Triggered", EventType.Play);
                position += loopLength;
            }

            DebugHelper.WriteLine(loopLength);

            player.Play("ControlTrack");
        }

        private void TestModulePlayer()
        {
            const string libraryFolder = @"D:\Music\Library";

            _bassPlayer = new BassPlayer(Handle);
            ExtenedAttributesHelper.ShufflerFolder = @"D:\Music\ShufflerAudioDatabase";

            _library = new Library(_bassPlayer) {LibraryFolder = libraryFolder};

            _library.LoadFromDatabase();
            _sampleLibrary = new SampleLibrary(_bassPlayer, _library);

            _modulePlayer = new ModulePlayer(libraryFolder);
            _bassPlayer.SpeakerOutput.AddInputChannel(_modulePlayer.Output);

            //const string module = @"C:\Users\jason\Dropbox\Music\Modules\Viva.json";
            //const string module = @"D:\Dropbox\Music\Modules\Viva.json";
            const string module = @"D:\Dropbox\Music\Modules\StereoFreeze.json";
            _modulePlayer.LoadModule(module);

            //_modulePlayer.PlayModuleLooped();
            //_modulePlayer.PlayPattern("StartMainLoop");
            //_modulePlayer.PlayPatternChannel("StartMainLoop", "MainLoops");
            //_modulePlayer.PlayPatternChannel("Loop0", "Drums");
            //_modulePlayer.PlayPattern("DrumsOnly");
            _modulePlayer.PlayPattern("Loop0");

            //PluginHelper.VstPluginsFolder = @"D:\Music\VstPlugins";
        }
    }
}