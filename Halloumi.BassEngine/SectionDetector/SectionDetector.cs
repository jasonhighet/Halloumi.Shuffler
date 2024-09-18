using System;
using System.Collections.Generic;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.SectionDetector
{
    public static class SectionDetector
    {
        private const float VOLUME_CHANGE_THRESHOLD = 1.5f; // Adjust as needed
        private const int MIN_SECTION_LENGTH = 8; // Minimum section length in seconds
        private const int WINDOW_SIZE = 4096; // FFT window size

        public struct SectionInfo
        {
            public double StartTime { get; }
            public double Length { get; }
            public double StartVolumeChangeTime { get; }
            public double EndVolumeChangeTime { get; }
            public double AverageBPM { get; }
            public double Bars { get; }

            public SectionInfo(double startTime, double length, double startVolumeChangeTime, double endVolumeChangeTime, double averageBPM, double bars)
            {
                StartTime = startTime;
                Length = length;
                StartVolumeChangeTime = startVolumeChangeTime;
                EndVolumeChangeTime = endVolumeChangeTime;
                AverageBPM = averageBPM;
                Bars = bars;
            }

            public override string ToString()
            {
                return $"Start: {StartTime:F4}, Length: {Length:F4}, StartVolumeChange: {StartVolumeChangeTime:F4}, EndVolumeChange: {EndVolumeChangeTime:F4}, BPM: {AverageBPM:F2}, Bars: {Bars:F2}";
            }
        }

        public static List<SectionInfo> SplitIntoSections(string mp3FilePath, List<BeatDetector.BeatInfo> beats)
        {
            var sections = new List<SectionInfo>();


            int stream = CreateStream(mp3FilePath);
            if (stream == 0)
            {
                throw new Exception($"Failed to load MP3 file: {Bass.BASS_ErrorGetCode()}");
            }

            try
            {
                var volumeChanges = DetectVolumeChanges(stream);
                sections = CreateSections(beats, volumeChanges);
            }
            finally
            {
                Bass.BASS_StreamFree(stream);
            }
            

            return sections;
        }

        private static int CreateStream(string mp3FilePath)
        {
            return Bass.BASS_StreamCreateFile(mp3FilePath, 0, 0, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
        }

        private static List<double> DetectVolumeChanges(int stream)
        {
            List<double> volumeChangeTimes = new List<double>();
            float[] buffer = new float[WINDOW_SIZE];
            float[] prevVolumes = new float[3] { 0, 0, 0 }; // Low, Mid, High

            while (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                int bytesRead = Bass.BASS_ChannelGetData(stream, buffer, (int)BASSData.BASS_DATA_FFT8192);
                if (bytesRead < 0) break;

                double currentTime = Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream));
                float[] currentVolumes = CalculateBandVolumes(buffer);

                if (IsSignificantVolumeChange(prevVolumes, currentVolumes))
                {
                    volumeChangeTimes.Add(currentTime);
                }

                prevVolumes = currentVolumes;
            }

            return volumeChangeTimes;
        }

        private static float[] CalculateBandVolumes(float[] fftData)
        {
            int lowBand = 0;
            int midBand = fftData.Length / 3;
            int highBand = 2 * fftData.Length / 3;

            return new float[]
            {
                CalculateBandEnergy(fftData, lowBand, midBand),
                CalculateBandEnergy(fftData, midBand, highBand),
                CalculateBandEnergy(fftData, highBand, fftData.Length)
            };
        }

        private static float CalculateBandEnergy(float[] fftData, int start, int end)
        {
            float sum = 0;
            for (int i = start; i < end; i++)
            {
                sum += fftData[i] * fftData[i];
            }
            return (float)Math.Sqrt(sum / (end - start));
        }

        private static bool IsSignificantVolumeChange(float[] prevVolumes, float[] currentVolumes)
        {
            for (int i = 0; i < 3; i++)
            {
                if (currentVolumes[i] > prevVolumes[i] * VOLUME_CHANGE_THRESHOLD ||
                    currentVolumes[i] < prevVolumes[i] / VOLUME_CHANGE_THRESHOLD)
                {
                    return true;
                }
            }
            return false;
        }

        private static List<SectionInfo> CreateSections(List<BeatDetector.BeatInfo> beats, List<double> volumeChanges)
        {
            List<SectionInfo> sections = new List<SectionInfo>();
            int beatIndex = 0; // Track the current beat
            double sectionStart = beats[0].StartTime; // Start from the first beat
            double lastBeatTime = beats[beats.Count - 1].StartTime; // Last beat time

            // Iterate through each volume change
            foreach (var volumeChangeTime in volumeChanges)
            {
                // Find the closest beat to the volume change
                while (beatIndex < beats.Count - 1 && beats[beatIndex + 1].StartTime < volumeChangeTime)
                {
                    beatIndex++;
                }

                // Now we have two candidates: the current beat and the next one
                double currentBeatTime = beats[beatIndex].StartTime;
                double nextBeatTime = beatIndex < beats.Count - 1 ? beats[beatIndex + 1].StartTime : double.MaxValue;

                // Find which beat is closer to the volume change
                double closestBeatTime = Math.Abs(volumeChangeTime - currentBeatTime) < Math.Abs(volumeChangeTime - nextBeatTime)
                    ? currentBeatTime
                    : nextBeatTime;

                // Only create a section if the duration is greater than the minimum section length
                if (closestBeatTime - sectionStart >= MIN_SECTION_LENGTH)
                {
                    // Calculate section length
                    double sectionEnd = closestBeatTime;
                    double sectionLength = sectionEnd - sectionStart;

                    // Calculate the average BPM and total beats in the section
                    double totalBPM = 0;
                    double totalBeatCount = 0;
                    int bpmCount = 0;

                    for (int i = 0; i < beats.Count; i++)
                    {
                        if (beats[i].StartTime >= sectionStart && beats[i].StartTime <= sectionEnd)
                        {
                            totalBPM += beats[i].BPM;
                            totalBeatCount += beats[i].BeatCount;
                            bpmCount++;
                        }
                    }

                    double averageBPM = bpmCount > 0 ? totalBPM / bpmCount : 0;
                    double bars = totalBeatCount / 4.0; // Assuming 4 beats per bar

                    // Add the section to the list
                    sections.Add(new SectionInfo(sectionStart, sectionLength, sectionStart, sectionEnd, averageBPM, bars));

                    // Update section start for the next section
                    sectionStart = sectionEnd;
                }
            }

            // Handle the last section if it's long enough
            if (lastBeatTime - sectionStart >= MIN_SECTION_LENGTH)
            {
                double sectionLength = lastBeatTime - sectionStart;

                // Calculate average BPM and total beats for the last section
                double totalBPM = 0;
                double totalBeatCount = 0;
                int bpmCount = 0;

                for (int i = 0; i < beats.Count; i++)
                {
                    if (beats[i].StartTime >= sectionStart && beats[i].StartTime <= lastBeatTime)
                    {
                        totalBPM += beats[i].BPM;
                        totalBeatCount += beats[i].BeatCount;
                        bpmCount++;
                    }
                }

                double averageBPM = bpmCount > 0 ? totalBPM / bpmCount : 0;
                double bars = totalBeatCount / 4.0;

                sections.Add(new SectionInfo(sectionStart, sectionLength, sectionStart, lastBeatTime, averageBPM, bars));
            }

            return sections;
        }

    }
}
