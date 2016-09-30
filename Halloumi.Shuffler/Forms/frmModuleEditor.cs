using System;
using System.Windows.Forms;
using Halloumi.Common.Windows.Controllers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioEngine;
using Halloumi.Shuffler.AudioEngine.ModulePlayer;
using Halloumi.Shuffler.AudioLibrary;

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

        private ModulePlayer ModulePlayer { get; set; }


        public void Initialize(BassPlayer bassPlayer, SampleLibrary sampleLibrary, Library library)
        {
            BassPlayer = bassPlayer;
            SampleLibrary = sampleLibrary;
            Library = library;
            ModulePlayer = new ModulePlayer();
            ModulePlayer.CreateModule();

            samplesControl.BassPlayer = BassPlayer;
            samplesControl.SampleLibrary = SampleLibrary;
            samplesControl.Library = Library;
            samplesControl.ModulePlayer = ModulePlayer;

            samplesControl.Initialize();
        }

        private void FrmModuleEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            ModulePlayer.Pause();
            samplesControl.Close();
        }

        private void FrmModuleEditor_Load(object sender, EventArgs e)
        {
        }

        private void fileMenuController_LoadDocument(object sender, FileMenuControllerEventArgs e)
        {
            ModulePlayer.Pause();
            samplesControl.ModulePlayer.LoadModule(e.FileName);
            BindData();
        }

        private void BindData()
        {
            samplesControl.BindData();
        }

        private void fileMenuController_NewDocument(object sender, FileMenuControllerEventArgs e)
        {
            ModulePlayer.Pause();
            samplesControl.ModulePlayer.CreateModule();
            BindData();
        }

        private void fileMenuController_SaveDocument(object sender, FileMenuControllerEventArgs e)
        {
            samplesControl.ModulePlayer.SaveModule(e.FileName);
        }
    }
}