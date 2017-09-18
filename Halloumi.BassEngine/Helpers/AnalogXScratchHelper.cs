using System.Diagnostics;
using System.IO;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.AudioEngine.Properties;
using Halloumi.Common.Helpers;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    public static class AnalogXScratchHelper
    {
        private static string _applicationFolder = @"C:\Program Files\AnalogX\Scratch\";
        private const string ScratchFile = "vdarkm8.wav";
        private const string LoopFile = "break1wa.wav";
        private const string ScratchExe = "scratch.exe";

        /// <summary>
        ///     Sets the application folder.
        /// </summary>
        /// <param name="applicationFolder">The application folder.</param>
        public static void SetApplicationFolder(string applicationFolder)
        {
            _applicationFolder = applicationFolder;
        }

        /// <summary>
        ///     Saves the specified sample as a mono wave in the Scratch folder and launches Scratch
        /// </summary>
        /// <param name="sample">The sample.</param>
        public static void Launch(Sample sample)
        {
            if (_applicationFolder == "") return;
            if (sample == null) return;

            SilenceHelper.GenerateSilenceAudioFile(GetSilenceFilename());
            SaveSample(sample);

            var gain = GetGain(sample.Filename);

            var scratchFilePath = Path.Combine(_applicationFolder, ScratchFile);
            AudioExportHelper.SaveAsMonoWave(sample.Filename, scratchFilePath, gain);
            

            var scratchExePath = Path.Combine(_applicationFolder, ScratchExe);
            Process.Start(scratchExePath);
        }

        private static void SaveSample(AudioStream sample)
        {
            if (!File.Exists(sample.Filename))
            {
                AudioExportHelper.SaveAsWave(sample.AudioData.Data, sample.Filename);
            }
        }

        private static string GetSilenceFilename()
        {
            return Path.Combine(_applicationFolder, LoopFile);
        }

        /// <summary>
        ///     Saves a shortened version the specified sample as a mono wave in the Scratch folder and launches Scratch
        /// </summary>
        /// <param name="sample">The sample.</param>
        public static void LaunchShort(Sample sample)
        {
            if (_applicationFolder == "") return;

            SilenceHelper.GenerateSilenceAudioFile(GetSilenceFilename());
            SaveSample(sample);

            var bpm = BpmHelper.GetBpmFromLoopLength(sample.LengthSeconds);
            var loopLength = BpmHelper.GetDefaultLoopLength(bpm);
            var shortLength = loopLength/8;
            if (shortLength > sample.LengthSeconds) shortLength = sample.LengthSeconds;

            var gain = GetGain(sample.Filename);
            var scratchFilePath = Path.Combine(_applicationFolder, ScratchFile);
            AudioExportHelper.SaveAsMonoWave(sample.Filename, scratchFilePath, shortLength, gain);

            var scratchExePath = Path.Combine(_applicationFolder, ScratchExe);
            Process.Start(scratchExePath);
        }

        public static bool IsScratchEnabled()
        {
            var scratchExePath = Path.Combine(_applicationFolder, ScratchExe);
            return File.Exists(scratchExePath);
        }

        /// <summary>
        /// Determines a gain factor (normalization), so that the maximum peak level of the stream will be at 0 dB.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="length">The length.</param>
        /// <returns>The gain factor</returns>
        private static float GetGain(string filename, double length = -1)
        {
            var  peak = 0F;
            return Utils.GetNormalizationGain(filename, 1, -1, length, ref peak);
        }
    }
}