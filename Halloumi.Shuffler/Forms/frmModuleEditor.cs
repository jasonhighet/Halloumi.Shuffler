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
    public partial class frmModuleEditor : BaseForm
    {
        public frmModuleEditor()
        {
            InitializeComponent();
        }

        private SampleLibrary _sampleLibrary { get; set; }

        private BassPlayer _bassPlayer { get; set; }


        public void Initialize(BassPlayer bassPlayer, SampleLibrary sampleLibrary)
        {
            _bassPlayer = bassPlayer;
            _sampleLibrary = sampleLibrary;

            samplesControl.BassPlayer = _bassPlayer;
            samplesControl.SampleLibrary = _sampleLibrary;
            samplesControl.ModulePlayer = new ModulePlayer();
            samplesControl.ModulePlayer.LoadModule("song.json");

            samplesControl.Initialize();
        }
    }
}
