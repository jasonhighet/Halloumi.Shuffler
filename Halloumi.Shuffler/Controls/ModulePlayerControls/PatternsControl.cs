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
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine.ModulePlayer;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioEngine;

namespace Halloumi.Shuffler.Controls.ModulePlayerControls
{
    public partial class PatternsControl : BaseUserControl
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

        public PatternsControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            BindData();
        }

        private bool _binding = false;
        public void BindData()
        {
            _binding = true;

            var currentPattern = cmbPattern.GetTextThreadSafe();
            cmbPattern.Items.Clear();
            foreach (var pattern in ModulePlayer.Module.Patterns)
            {
                cmbPattern.Items.Add(pattern.Key);
            }
            cmbPattern.SelectedIndex = cmbPattern.FindString(currentPattern);

            var currentChannel = cmbChannel.GetTextThreadSafe();
            cmbChannel.Items.Clear();
            foreach (var channel in ModulePlayer.Module.Channels)
            {
                cmbChannel.Items.Add(channel.Key);
            }
            cmbChannel.SelectedIndex = cmbChannel.FindString(currentChannel);

            _binding = false; 
        }

        private void cmbPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
        }

        private void cmbChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
        }

        private void btnAddPattern_Click(object sender, EventArgs e)
        {
            var patternKey = UserInputHelper.GetUserInput("Enter Pattern Key", "", ParentForm);
            if (patternKey == "") return;
            ModulePlayer.AddPattern(patternKey);
            BindData();
        }

        private void btnAddChannel_Click(object sender, EventArgs e)
        {
            var channelKey = UserInputHelper.GetUserInput("Enter Channel Key", "", ParentForm);
            if (channelKey == "") return;
            ModulePlayer.AddChannel(channelKey);
            BindData();
        }

        private void btnDeletePattern_Click(object sender, EventArgs e)
        {
            var patternKey = cmbPattern.GetTextThreadSafe();
            if (patternKey == "") return;
            ModulePlayer.DeletePattern(patternKey);
            BindData();
        }

        private void btnDeleteChannel_Click(object sender, EventArgs e)
        {
            var channelKey = cmbChannel.GetTextThreadSafe();
            if (channelKey == "") return;
            ModulePlayer.DeleteChannel(channelKey);
            BindData();
        }
    }
}
