using System;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.ModulePlayer;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;
using Halloumi.Shuffler.Forms;

namespace Halloumi.Shuffler.TestHarness
{
    public partial class Form1 : Form
    {
        private BassPlayer _bassPlayer;
        private Library _library;
        private ModulePlayer _player;
        private SampleLibrary _sampleLibrary;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DebugHelper.DebugMode = false;

            _bassPlayer = new BassPlayer(Handle);
            ExtenedAttributesHelper.ExtendedAttributeFolder = @"D:\Music\ShufflerAudioDatabase";
            _library = new Library(_bassPlayer)
            {
                LibraryFolder = @"E:\Music\Library"
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
            var audioFile = _player.Module.AudioFiles[1];

            var samples = audioFile
                .Samples
                .Select(x => new Sample
                {
                    Start = x.Start,
                    Description = x.Key,
                    Length = x.Length,
                    Offset = x.Offset ?? 0
                }).ToList();


            var form = new FrmEditTrackSamples
            {
                BassPlayer = _bassPlayer,
                Filename = audioFile.Path,
                SampleLibrary = _sampleLibrary,
                Library = _library,
                Samples = samples
            };

            if (form.ShowDialog() != DialogResult.OK) return;

            var newSamples = form.Samples.Select(x => new Module.Sample
            {
                Start = x.Start,
                Length = x.Length,
                Offset = x.Offset,
                Key = x.Description
            }).ToList();

            _player.UpdateSamples(audioFile, newSamples);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _player.SaveModule("song.json");
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            _player.PlayModuleLooped();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _player.Pause();
        }
    }
}