using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Forms.TrackPlayerExtensions
{
    public partial class FrmFadeNow : TrackPlayerExtensionForm
    {
        public FrmFadeNow()
        {
            InitializeComponent();
            cmbFadeType.SelectedIndex = 0;
        }

        private void btnFadeNow_Click(object sender, EventArgs e)
        {
            var fadeType = cmbFadeType.ParseEnum<BE.ForceFadeType>();
            BassPlayer.ForceFadeNow(fadeType);
            Close();
        }
    }
}