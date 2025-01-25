using Halloumi.Shuffler.AudioEngine.SectionDetector;
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



            //var mp3Path = @"C:\Users\jason\OneDrive\Music\Library\Disco Doesn't Suck\Lee MC Donald - I 'Ll Do Anything for You (Patchworks Remix).mp3";
            ////var mp3Path = @"C:\Users\jason\OneDrive\Music\Library\Disco Doesn't Suck\Mirage - Summer Grooves.mp3";
            //// var mp3Path = @"C:\Users\jason\OneDrive\Music\Library\Disco Doesn't Suck\Nina Simone - Ain't Got No (I Got Life) (Single Version).mp3";

            ////var mp3Path = @"C:\Users\jason\OneDrive\Music\Library\Disco Doesn't Suck\Al Green - Standing In The Rain.mp3";

            //var approximateBPM = BPMGuestimator.EstimateBPM(mp3Path);
            //var beatTimes = BeatDetector2.DetectBeats(mp3Path, approximateBPM);

            //SectionDetector2.SplitIntoSections(mp3Path, beatTimes, @"C:\Users\jason\OneDrive\Music\Library\Disco Doesn't Suck\Sections.csv");

            //// , @"C:\Users\jason\OneDrive\Music\Library\Disco Doesn't Suck\Al Green - Standing In The Rain.csv"

            //Console.WriteLine($"\nDetected {beats.Count} beats:");
            //foreach (var beat in beats)
            //{
            //    Console.WriteLine($"Start: {beat.StartTime:F3}s, End: {beat.EndTime:F3}s, Duration: {beat.Duration:F3}s");
            //}

            //// Detect sections
            //var sections = SectionDetector2.DetectSections(mp3Path, approximateBPM, beats);
            //Console.WriteLine($"\nDetected {sections.Count} sections:");
            //foreach (var section in sections)
            //{
            //    Console.WriteLine($"Section - Start: {section.StartTime:F3}s, End: {section.EndTime:F3}s, Duration: {section.Duration:F3}s");
            //}





            var csvFilePath = @"C:\Users\jason\OneDrive\Music\Library\Techstep Drum & Bass\TechstepDrumBass.csv";
            var audioFilePath = @"C:\Users\jason\OneDrive\Desktop\techdb3.wav";
            AudioSplitter.AudioSplitter.SplitWavByCsv(audioFilePath, csvFilePath);



        }
    }
}
