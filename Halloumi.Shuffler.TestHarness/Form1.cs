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

            var file = @"H:\Music\Pucho & His Latin Soul Brothers\Big Stick Dateline\04 - Pucho & His Latin Soul Brothers - Sunny.mp3";
            Parallel.For(0, 10, i => 
            {
                player.Load("stream" + i.ToString(), file);

                for (int j = 0; j < 10; j++)
                {
                    player.AddSection("stream"+ i.ToString(), "section" + j.ToString());
                    player.SetSectionPositions("stream" + i.ToString(), "section" + j.ToString(), (j * 10) + i, 3);
                    player.AddCustomSync("stream" + i.ToString(), (j * 10) + i + 2);
                }

                player.QueueSection("stream" + i.ToString(), "section0");
                player.Play("stream" + i.ToString());
            });


            //player.UnloadAll();
        }
    }
}
