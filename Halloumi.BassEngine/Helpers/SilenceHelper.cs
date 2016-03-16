using System.IO;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Properties;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    public static class SilenceHelper
    {
        /// <summary>
        ///     Gets the filename of an audio file containing silence.
        ///     Generates the file if it doesn't exist
        /// </summary>
        /// <returns></returns>
        public static string GetSilenceAudioFile()
        {
            var filename = Path.Combine(Path.GetTempPath(), "silence.wav");

            GenerateSilenceAudioFile(filename);

            return filename;
        }

        /// <summary>
        ///     Generates an audio file of silence.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public static void GenerateSilenceAudioFile(string filename)
        {
            if (File.Exists(filename))
                return;

            var audioData = Resources.silence_mp3.ToArray();
            AudioExportHelper.SaveAsMonoWave(audioData, filename);
        }
    }
}