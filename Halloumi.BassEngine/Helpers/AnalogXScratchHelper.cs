using System.Diagnostics;
using System.IO;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.AudioEngine.Properties;
using Halloumi.Common.Helpers;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    public static class AnalogXScratchHelper
    {
        private static string _applicationFolder = @"C:\Program Files\AnalogX\Scratch\";
        private const string ScratchFile = "vdarkm8.wav";
        private const string LoopFile = "break1wa.wav";
        private const string ScratchExe = "scratch.exe";
        private static bool _silenceSaved;

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
            if (!_silenceSaved) SaveSilenceLoop();
            if (!File.Exists(sample.Filename))
            {
                ExportHelper.SaveAsWave(sample.AudioData.Data, sample.Filename);
            }

            var scratchFilePath = Path.Combine(_applicationFolder, ScratchFile);
            ExportHelper.SaveAsMonoWave(sample.Filename, scratchFilePath, sample.Gain);
            NormalizeWave(scratchFilePath);

            var scratchExePath = Path.Combine(_applicationFolder, ScratchExe);
            Process.Start(scratchExePath);
        }

        /// <summary>
        ///     Saves a shortened version the specified sample as a mono wave in the Scratch folder and launches Scratch
        /// </summary>
        /// <param name="sample">The sample.</param>
        public static void LaunchShort(Sample sample)
        {
            if (_applicationFolder == "") return;
            if (!_silenceSaved) SaveSilenceLoop();

            var bpm = BpmHelper.GetBpmFromLoopLength(sample.LengthSeconds);
            var loopLength = BpmHelper.GetDefaultLoopLength(bpm);
            var shortLength = loopLength/8;
            if (shortLength > sample.LengthSeconds) shortLength = sample.LengthSeconds;

            var scratchFilePath = Path.Combine(_applicationFolder, ScratchFile);
            ExportHelper.SaveAsMonoWave(sample.Filename, scratchFilePath, shortLength, sample.Gain);
            NormalizeWave(scratchFilePath);

            var scratchExePath = Path.Combine(_applicationFolder, ScratchExe);
            Process.Start(scratchExePath);
        }

        /// <summary>
        ///     Saves few seconds of silence as the Scratch loop wave.
        /// </summary>
        private static void SaveSilenceLoop()
        {
            var audioData = Resources.silence_mp3.ToArray();
            var loopFilePath = Path.Combine(_applicationFolder, LoopFile);

            ExportHelper.SaveAsMonoWave(audioData, loopFilePath);
            _silenceSaved = true;
        }

        private static void NormalizeWave(string waveFile)
        {
            if (!File.Exists(waveFile)) return;

            var normalizeExe = "normalize.exe";
            normalizeExe = Path.Combine(ApplicationHelper.GetExecutableFolder(), normalizeExe);
            if (!File.Exists(normalizeExe)) return;

            var arguments = $"-q \"{waveFile}\"";

            Process.Start(normalizeExe, arguments);
        }
    }
}