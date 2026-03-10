using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioLibrary.Samples;
using Halloumi.Shuffler.Controls;
using AE = Halloumi.Shuffler.AudioEngine;
using Halloumi.Shuffler;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmSampleLibrary : BaseForm
    {
        public FrmSampleLibrary()
        {
            InitializeComponent();
        }

        internal void Initialize(ShufflerApplication application, BassPlayer bassPlayer, ISampleLibrary trackSampleLibrary, ISampleRecipient sampleRecipient = null)
        {
            sampleLibraryControl.Application = application;
            sampleLibraryControl.BassPlayer = bassPlayer;
            sampleLibraryControl.SampleLibrary = trackSampleLibrary;
            sampleLibraryControl.SampleRecipient = sampleRecipient;
            sampleLibraryControl.Initialize();
        }

        private void frmSampleLibrary_FormClosing(object sender, FormClosingEventArgs e)
        {
            sampleLibraryControl.Close();
        }
    }
}