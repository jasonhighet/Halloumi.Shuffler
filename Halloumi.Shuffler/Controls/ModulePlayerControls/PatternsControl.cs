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

            BindPatterns();
            BindChannels();
            BindSequence();

            _binding = false;
        }

        private void BindSequence()
        {
            var sequence = GetSequence();
            var samples = GetModuleSamples();

            listBuilder.SetSourceList(samples);
            listBuilder.SetDestinationList(sequence);
        }

        private List<string> GetSequence()
        {
            var currentChannelIndex = GetCurrentChannelIndex();
            var currentPatternIndex = GetCurrentPatternIndex();

            List<string> sequence;
            if (currentChannelIndex == -1 || currentPatternIndex == -1)
            {
                sequence = new List<string>();
            }
            else
            {
                var key = cmbPattern.GetTextThreadSafe();
                var pattern = ModulePlayer.Module.Patterns.FirstOrDefault(x => x.Key == key);
                if (pattern == null) return null;
                sequence = pattern.Sequence[currentChannelIndex];
            }

            return sequence;
        }

        private List<string> GetModuleSamples()
        {
            var samples = new List<string>();
            foreach (var audioFile in ModulePlayer.Module.AudioFiles)
            {
                samples.AddRange(audioFile.Samples.Select(sample => audioFile.Key + "." + sample.Key));
            }
            samples = samples.OrderBy(x => x).ToList();
            return samples;
        }

        private int GetCurrentPatternIndex()
        {
            return ModulePlayer.Module.Patterns.FindIndex(x => x.Key == cmbPattern.GetTextThreadSafe());
        }

        private int GetCurrentChannelIndex()
        {
            return ModulePlayer.Module.Channels.FindIndex(x => x.Key == cmbChannel.GetTextThreadSafe());
        }

        private void BindChannels()
        {
            var currentChannel = cmbChannel.GetTextThreadSafe();
            cmbChannel.Items.Clear();
            foreach (var channel in ModulePlayer.Module.Channels)
            {
                cmbChannel.Items.Add(channel.Key);
            }
            cmbChannel.SelectedIndex = cmbChannel.FindString(currentChannel);
        }

        private void BindPatterns()
        {
            var currentPattern = cmbPattern.GetTextThreadSafe();
            cmbPattern.Items.Clear();
            foreach (var pattern in ModulePlayer.Module.Patterns)
            {
                cmbPattern.Items.Add(pattern.Key);
            }
            cmbPattern.SelectedIndex = cmbPattern.FindString(currentPattern);
        }

        private void cmbPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            BindSequence();
        }

        private void cmbChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binding) return;
            BindSequence();
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

        private void listBuilder_OnDestinationListChanged(object sender, EventArgs e)
        {
            var channelIndex = GetCurrentChannelIndex();

            var pattern = ModulePlayer
                .Module
                .Patterns
                .FirstOrDefault(x => x.Key == cmbPattern.GetTextThreadSafe());

            if(pattern == null)
                return;
            
            pattern.Sequence[channelIndex] = listBuilder.GetDestinationList();

            ModulePlayer.LoadModule(ModulePlayer.Module);
        }
    }
}
