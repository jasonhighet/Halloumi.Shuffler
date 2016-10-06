﻿using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.Controls;
using Halloumi.Shuffler.Controls.ModulePlayerControls;
using AE = Halloumi.Shuffler.AudioEngine;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmSampleLibrary : BaseForm
    {
        public FrmSampleLibrary()
        {
            InitializeComponent();
        }

        internal void Initialize(AE.BassPlayer bassPlayer, SampleLibrary sampleLibrary, ISampleRecipient sampleRecipient = null)
        {
            sampleLibraryControl.BassPlayer = bassPlayer;
            sampleLibraryControl.SampleLibrary = sampleLibrary;
            sampleLibraryControl.SampleRecipient = sampleRecipient;
            sampleLibraryControl.Initialize();
        }

        private void frmSampleLibrary_FormClosing(object sender, FormClosingEventArgs e)
        {
            sampleLibraryControl.Close();
        }
    }
}