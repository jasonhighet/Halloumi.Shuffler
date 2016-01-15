using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.Engine;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmSampleLibrary : BaseForm
    {
        public FrmSampleLibrary()
        {
            InitializeComponent();
        }

        internal void Initialize(BE.BassPlayer bassPlayer, SampleLibrary sampleLibrary)
        {
            sampleLibraryControl.BassPlayer = bassPlayer;
            sampleLibraryControl.SampleLibrary = sampleLibrary;
            sampleLibraryControl.Initialize();
        }

        private void frmSampleLibrary_FormClosing(object sender, FormClosingEventArgs e)
        {
            sampleLibraryControl.Close();
        }
    }
}