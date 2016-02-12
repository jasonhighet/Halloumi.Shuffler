using System;
using System.ComponentModel;
using Halloumi.Common.Windows.Forms;
using AE = Halloumi.Shuffler.AudioEngine;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmMonitorSettings : BaseForm
    {
        public FrmMonitorSettings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            sldVolume.Scrolled += new MediaSlider.MediaSlider.ScrollDelegate(sldVolume_Slid);
            sldVolume.Minimum = 0;
            sldVolume.Maximum = 100;

            SetVolume(Convert.ToInt32(BassPlayer.GetMonitorVolume()));
        }

        private void SetVolume(int volume)
        {
            if (volume < 0 || volume > 100) return;

            BassPlayer.SetMonitorVolume(Convert.ToDecimal(volume));
            lblVolume.Text = volume.ToString();

            if (sldVolume.Value != volume) sldVolume.Value = volume;
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            try
            {
                //var settings = Halloumi.Shuffler.Forms.Settings.Default;
                //SetVolume(settings.MonitorVolume);
            }
            catch
            { }
        }

        /// <summary>
        /// Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AE.BassPlayer BassPlayer { get; set; }

        /// <summary>
        /// Handles the Slid event of the sldVolume control.
        /// </summary>
        private void sldVolume_Slid(object sender, EventArgs e)
        {
            SetVolume(sldVolume.ScrollValue);
        }

        /// <summary>
        /// Handles the Load event of the frmMonitorSettings control.
        /// </summary>
        private void frmMonitorSettings_Load(object sender, EventArgs e)
        {
            Initialize();
            LoadSettings();
        }

        /// <summary>
        /// Handles the Click event of the btnClose control.
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
