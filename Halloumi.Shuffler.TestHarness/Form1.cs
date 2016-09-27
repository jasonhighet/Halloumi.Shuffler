using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.ModulePlayer;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;
using Halloumi.Shuffler.Forms;

namespace Halloumi.Shuffler.TestHarness
{
    public partial class Form1 : Form
    {
        private ModulePlayer _player;
        private BassPlayer _bassPlayer;
        private Library _library;
        private SampleLibrary _sampleLibrary;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _bassPlayer = new BassPlayer(Handle);
            ExtenedAttributesHelper.ExtendedAttributeFolder = @"D:\Music\ShufflerAudioDatabase";
            _library = new Library(_bassPlayer)
            {
                LibraryFolder = @"E:\Music\Library",
            };
            

            _library.LoadFromDatabase();
            _sampleLibrary = new SampleLibrary(_bassPlayer, _library);

            _player = new ModulePlayer();
            _bassPlayer.SpeakerOutput.AddInputChannel(_player.Output);

            _player.LoadModule("song.json");

            //_player.PlayModuleLooped();

            //_player.PlayPattern("StartMainLoop");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var samples = _player
                .Module
                .AudioFiles[0]
                .Samples
                .Select(x => new Sample()
                {
                    Start = x.Start,
                    Description = x.Key,
                    Length = x.Length,
                    Offset = x.Offset ?? 0
                }).ToList();


            var form = new FrmEditTrackSamples
            {
                BassPlayer = _bassPlayer,
                Filename = _player.Module.AudioFiles[0].Path,
                SampleLibrary = _sampleLibrary,
                Library = _library,
                Samples = samples
            };

            

            form.ShowDialog();
        }
    }
}