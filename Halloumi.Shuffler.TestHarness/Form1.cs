using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.Players;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;

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
            ChannelHelper.InitialiseAudioEngine(Handle);

            var speakers = new SpeakerOutputChannel();
            var player = new AudioPlayer();

            speakers.AddInputChannel(player.Output);

            var file = @"E:\Music\Library\A Reggae Tribute To The Beatles\14 - Various - Roslyn Sweat & The Paragons  Blackbird.mp3";
            Parallel.For(0, 10, i => 
            {
                player.Load("stream" + i.ToString(), file);

                for (int j = 0; j < 10; j++)
                {
                    player.AddSection("stream"+ i.ToString(), "section" + j.ToString());
                    player.SetSectionPositions("stream" + i.ToString(), "section" + j.ToString(), (j * 10) + i, 3);
                }

                player.QueueSection("stream" + i.ToString(), "section0");
                player.Play("stream" + i.ToString());
            });


        }
    }
}
