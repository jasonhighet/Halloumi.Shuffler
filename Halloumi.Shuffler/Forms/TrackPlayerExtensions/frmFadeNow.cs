using System;
using AE = Halloumi.Shuffler.AudioEngine;

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
            var fadeType = cmbFadeType.ParseEnum<AE.ForceFadeType>();
            BassPlayer.ForceFadeNow(fadeType);
            Close();
        }
    }
}