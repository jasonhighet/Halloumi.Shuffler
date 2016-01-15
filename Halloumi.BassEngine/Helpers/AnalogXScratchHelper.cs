using System.Linq;
using System.Diagnostics;
using System.IO;
using Halloumi.Common.Helpers;

namespace Halloumi.BassEngine
{
    public static class AnalogXScratchHelper
    {
        private static string _applicationFolder = @"C:\Program Files\AnalogX\Scratch\";
        private static string _scratchFile = "vdarkm8.wav";
        private static string _loopFile = "break1wa.wav";
        private static string _scratchExe = "scratch.exe";
        private static bool _silenceSaved = false;

        /// <summary>
        /// Sets the application folder.
        /// </summary>
        /// <param name="applicationFolder">The application folder.</param>
        public static void SetApplicationFolder(string applicationFolder)
        {
            _applicationFolder = applicationFolder;
        }

        /// <summary>
        /// Saves the specied sample as a mono wave in the Scratch folder and launches Scratch
        /// </summary>
        /// <param name="sample">The sample.</param>
        public static void Launch(Sample sample)
        {
            if (_applicationFolder == "") return;
            if (sample == null) return;
            if (!_silenceSaved) SaveSilenceLoop();
            if (!File.Exists(sample.Filename))
            {
                BassHelper.SaveAsWave(sample.AudioData, sample.Filename);
            }

            var scratchFilePath = Path.Combine(_applicationFolder, _scratchFile);
            BassHelper.SaveAsMonoWave(sample.Filename, scratchFilePath, sample.Gain);
            NormalizeWave(scratchFilePath);

            var scratchExePath = Path.Combine(_applicationFolder, _scratchExe);
            Process.Start(scratchExePath);
        }

        /// <summary>
        /// Saves a shortened version the specied sample as a mono wave in the Scratch folder and launches Scratch
        /// </summary>
        /// <param name="sample">The sample.</param>
        public static void LaunchShort(Sample sample)
        {
            if (_applicationFolder == "") return;
            if (!_silenceSaved) SaveSilenceLoop();

            var bpm = BassHelper.GetBpmFromLoopLength(sample.LengthSeconds);
            var loopLength = BassHelper.GetDefaultLoopLength(bpm);
            var shortLength = loopLength / 8;
            if (shortLength > sample.LengthSeconds) shortLength = sample.LengthSeconds;

            var scratchFilePath = Path.Combine(_applicationFolder, _scratchFile);
            BassHelper.SaveAsMonoWave(sample.Filename, scratchFilePath, shortLength, sample.Gain);
            NormalizeWave(scratchFilePath);

            var scratchExePath = Path.Combine(_applicationFolder, _scratchExe);
            Process.Start(scratchExePath);
        }

        /// <summary>
        /// Saves few seconds of silence as the Scratch loop wave.
        /// </summary>
        private static void SaveSilenceLoop()
        {
            var audioData = Properties.Resources.silence_mp3.ToArray();
            var loopFilePath = Path.Combine(_applicationFolder, _loopFile);

            BassHelper.SaveAsMonoWave(audioData, loopFilePath);
            _silenceSaved = true;
        }

        private static void NormalizeWave(string waveFile)
        {
            if (!File.Exists(waveFile)) return;

            var normalizeExe = "normalize.exe";
            normalizeExe = Path.Combine(ApplicationHelper.GetExecutableFolder(), normalizeExe);
            if(!File.Exists(normalizeExe)) return;

            var arguments = string.Format("-q \"{0}\"", waveFile);

            Process.Start(normalizeExe, arguments);
        }
    }
}
