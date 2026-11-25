using Halloumi.Shuffler.TestHarness.AudioSplitter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass;

namespace Halloumi.Shuffler.TestHarness
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false); 
            //Application.Run(new Form1());

            //// Initialize BASS
            //BassNet.Registration("jason.highet@gmail.com", "2X1931822152222");
            //if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            //{
            //    Console.WriteLine($"Failed to initialize BASS. Error code: {Bass.BASS_ErrorGetCode()}");
            //    return;
            //}








            var csvFilePath = @"C:\Users\jason\OneDrive\Music\Library\Techstep Drum & Bass\TechstepDrumBass.csv";
            var audioFilePath = @"C:\Users\jason\OneDrive\Desktop\techdb3.wav";
            AudioSplitter.AudioSplitter.SplitWavByCsv(audioFilePath, csvFilePath);



        }
    }
}
