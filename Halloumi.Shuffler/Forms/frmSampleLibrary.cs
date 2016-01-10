using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.Engine;
using BE = Halloumi.BassEngine;
using Halloumi.Shuffler.Controls;

namespace Halloumi.Shuffler.Forms
{
    public partial class frmSampleLibrary : BaseForm
    {
        public frmSampleLibrary()
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