using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.ModulePlayer;
using Halloumi.Shuffler.AudioEngine.Plugins;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;

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
            DebugHelper.DebugMode = true;

            const string libraryFolder = @"D:\Music\Library";

            _bassPlayer = new BassPlayer(Handle);
            ExtenedAttributesHelper.ExtendedAttributeFolder = @"D:\Music\ShufflerAudioDatabase";

            _library = new Library(_bassPlayer) { LibraryFolder = libraryFolder };


            _library.LoadFromDatabase();
            _sampleLibrary = new SampleLibrary(_bassPlayer, _library);

            _player = new ModulePlayer(libraryFolder);
            _bassPlayer.SpeakerOutput.AddInputChannel(_player.Output);

             //const string module = @"C:\Users\jason\Brooklyn2Birkenhead My Files\Music\Modules\Viva.json";
            //const string module = @"D:\Brooklyn2Birkenhead My Files\Music\Modules\Viva.json";
            const string module = @"D:\Brooklyn2Birkenhead My Files\Music\Modules\StereoFreeze.json";
            _player.LoadModule(module);

            //_player.PlayModuleLooped();
            //_player.PlayPattern("StartMainLoop");
            //_player.PlayPatternChannel("StartMainLoop", "MainLoops");
            _player.PlayPatternChannel("Loop0", "Drums");
            //_player.PlayPattern("DrumsOnly");

            //PluginHelper.VstPluginsFolder = @"D:\Music\VstPlugins";
        }

    }
}