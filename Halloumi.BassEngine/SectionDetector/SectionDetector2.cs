using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.SectionDetector
{
    public static class SectionDetector2
    {
        public struct SectionInfo
        {
            public double StartTime { get; }
            public double EndTime { get; }

            public SectionInfo(double startTime, double endTime)
            {
                StartTime = startTime;
                EndTime = endTime;
            }

            // Calculated property to get the duration of the beat
            public double Duration => EndTime - StartTime;
        }



        public static List<SectionInfo> SplitIntoSections(string mp3FilePath, List<Double> beatTimes, string csvFilePath = "")
        {
            var sections = new List<SectionInfo>();

            // Open the MP3 stream
            var stream = Bass.BASS_StreamCreateFile(mp3FilePath, 0L, 0L, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
            if (stream == 0)
            {
                throw new Exception($"Failed to load MP3 file: {Bass.BASS_ErrorGetCode()}");
            }

            // Prepare for CSV writing if a path is provided
            StreamWriter csvWriter = null;
            if (!string.IsNullOrEmpty(csvFilePath))
            {
                csvWriter = new StreamWriter(csvFilePath);
                // Write CSV headers
                csvWriter.WriteLine("StartTime,EndTime,AverageVolume,AverageBassVolume,AverageMidVolume,AverageHighVolume");
            }

            try
            {
                for (int i = 0; i < beatTimes.Count - 2; i++)
                {
                    // Get start and end times for the previous and current beats
                    double previousBeatStartTime = beatTimes[i];
                    double previousBeatEndTime = beatTimes[i + 1];

                    double currentBeatStartTime = beatTimes[i + 1];
                    double currentBeatEndTime = beatTimes[i + 2];

                    // Get average volumes for the previous beat
                    var previousBeatVolumeInfo = VolumeAnalyzer.GetAverageVolumes(stream, previousBeatStartTime, previousBeatEndTime);
                    var currentBeatVolumeInfo = VolumeAnalyzer.GetAverageVolumes(stream, currentBeatStartTime, currentBeatEndTime);

                    // Write to CSV if the file path is provided
                    if (csvWriter != null)
                    {
                        // Write previous beat info to CSV, formatted to 4 decimal places
                        csvWriter.WriteLine(
                            $"{previousBeatVolumeInfo.StartTime.ToString("F4", CultureInfo.InvariantCulture)}," +
                            $"{previousBeatVolumeInfo.EndTime.ToString("F4", CultureInfo.InvariantCulture)}," +
                            $"{previousBeatVolumeInfo.AverageVolume.ToString("F4", CultureInfo.InvariantCulture)}," +
                            $"{previousBeatVolumeInfo.AverageBassVolume.ToString("F4", CultureInfo.InvariantCulture)}," +
                            $"{previousBeatVolumeInfo.AverageMidVolume.ToString("F4", CultureInfo.InvariantCulture)}," +
                            $"{previousBeatVolumeInfo.AverageHighVolume.ToString("F4", CultureInfo.InvariantCulture)}"
                        );
                    }
                }
            }
            finally
            {
                // Ensure the BASS stream is freed
                Bass.BASS_StreamFree(stream);

                // Close the CSV writer if it was used
                csvWriter?.Close();
            }

            return sections;
        }
    }
}
