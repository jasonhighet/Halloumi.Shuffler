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

            songControl.BassPlayer = BassPlayer;
            songControl.SampleLibrary = SampleLibrary;
            songControl.Library = Library;
            songControl.ModulePlayer = ModulePlayer;

            patternsControl.BassPlayer = BassPlayer;
            patternsControl.SampleLibrary = SampleLibrary;
            patternsControl.Library = Library;
            patternsControl.ModulePlayer = ModulePlayer;

            samplesControl.Initialize();
            songControl.Initialize();
            patternsControl.Initialize();

            SetVisibleControls();
        }

        private void SetVisibleControls()
        {
            samplesControl.Visible = btnSamples.Checked;
            songControl.Visible = btnSong.Checked;
            patternsControl.Visible = btnPatterns.Checked;
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
            ModulePlayer.LoadModule(e.FileName);
            BindData();
        }

        private void BindData()
        {
            samplesControl.BindData();
            songControl.BindData();
            patternsControl.BindData();
        }

        private void fileMenuController_NewDocument(object sender, FileMenuControllerEventArgs e)
        {
            ModulePlayer.Pause();
            ModulePlayer.CreateModule();
            BindData();
        }

        private void fileMenuController_SaveDocument(object sender, FileMenuControllerEventArgs e)
        {
            samplesControl.ModulePlayer.SaveModule(e.FileName);
        }

        private void btnSamples_Click(object sender, EventArgs e)
        {
            SetVisibleControls();
        }

        private void btnSong_Click(object sender, EventArgs e)
        {
            SetVisibleControls();
            songControl.BindData();
        }

        private void btnPatterns_Click(object sender, EventArgs e)
        {
            SetVisibleControls();
            patternsControl.BindData();
        }
    }
}