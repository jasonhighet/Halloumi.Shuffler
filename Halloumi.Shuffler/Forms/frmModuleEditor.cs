using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioEngine;
using Halloumi.Shuffler.AudioEngine.ModulePlayer;
using Halloumi.Shuffler.AudioLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmModuleEditor : BaseForm
    {
        public FrmModuleEditor()
        {
            InitializeComponent();
        }

        private SampleLibrary SampleLibrary { get; set; }

        private BassPlayer BassPlayer { get; set; }

        private Library Library { get; set; }


        public void Initialize(BassPlayer bassPlayer, SampleLibrary sampleLibrary, Library library)
        {
            BassPlayer = bassPlayer;
            SampleLibrary = sampleLibrary;

            samplesControl.BassPlayer = BassPlayer;
            samplesControl.SampleLibrary = SampleLibrary;
            samplesControl.Library = Library;
            samplesControl.ModulePlayer = new ModulePlayer();
            samplesControl.ModulePlayer.LoadModule("song.json");

            samplesControl.Initialize();
        }
    }
}
