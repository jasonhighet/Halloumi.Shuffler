using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Halloumi.BassEngine.Players;
using Halloumi.BassEngine.Channels;
using Halloumi.BassEngine.Helpers;

namespace Halloumi.Shuffler.TestHarness
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ChannelHelper.InitialiseBassEngine(Handle);

            var speakers = new SpeakerOutputChannel();
            var rawLoopPlayer = new RawLoopPlayer();

            speakers.AddInputChannel(rawLoopPlayer.Output);

            rawLoopPlayer.LoadAudio(@"E:\Music\Library\A Reggae Tribute To The Beatles\14 - Various - Roslyn Sweat & The Paragons  Blackbird.mp3");
            rawLoopPlayer.SetPositions(0, 3);
            rawLoopPlayer.Play();
        }
    }
}
