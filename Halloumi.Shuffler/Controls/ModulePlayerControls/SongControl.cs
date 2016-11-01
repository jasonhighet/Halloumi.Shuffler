using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Halloumi.Common.Windows.Controls;
using Halloumi.Shuffler.AudioEngine.ModulePlayer;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioEngine;

namespace Halloumi.Shuffler.Controls.ModulePlayerControls
{
    public partial class SongControl : BaseUserControl
    {
        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ModulePlayer ModulePlayer { get; set; }

        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SampleLibrary SampleLibrary { get; set; }

        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BassPlayer BassPlayer { get; set; }

        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Library Library { get; set; }

        public SongControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            cmbBPM.Items.Clear();
            for (var i = 68; i <= 136; i++)
            {
                cmbBPM.Items.Add(i);
            }

            BindData();
        }

        private bool _binding = false;

        public void BindData()
        {
            _binding = true;
            var bpm = Convert.ToInt32(Math.Round(ModulePlayer.Module.Bpm,0)).ToString();
            cmbBPM.SelectedIndex = cmbBPM.FindString(bpm);

            var patternKeys = ModulePlayer.Module.Patterns.Select(x=>x.Key).ToList();
            listBuilder.SetAvailableItems(patternKeys);

            listBuilder.SetSelectedItems(ModulePlayer.Module.Sequence);

            _binding = false; 
        }

        private void cmbBPM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            SetBpm();
        }

        private void SetBpm()
        {
            var bpm = decimal.Parse(cmbBPM.GetTextThreadSafe());
            ModulePlayer.SetBpm(bpm);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            ModulePlayer.PlayModule();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            ModulePlayer.Pause();
        }

        private void listBuilder_OnDestinationListChanged(object sender, EventArgs e)
        {
            var sequence = listBuilder.GetSelectedItems();
            ModulePlayer.Module.Sequence = sequence;
            ModulePlayer.ReloadModule();
        }

        private void btnLoop_Click(object sender, EventArgs e)
        {
            ModulePlayer.PlayModuleLooped();
        }
    }
}
