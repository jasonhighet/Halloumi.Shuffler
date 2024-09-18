using System;
using System.Collections.Generic;
using System.Linq;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Tags;

namespace Halloumi.Shuffler.AudioEngine.SectionDetector
{
    public class BeatDetector
    {
        private const int BUFFER_LENGTH = 1024;
        private const double ENERGY_SAMPLE_TIME = 10.0; // seconds

        public struct BeatInfo
        {
            public double StartTime { get; }

            public double EndTime { get; }

            public double Interval { get; }
            public double BPM { get; }
            public double BeatCount { get; }

            public BeatInfo(double startTime, double endTime, double interval, double bpm, double beatCount)
            {
                StartTime = startTime;
                EndTime = endTime;
                Interval = interval;
                BPM = bpm;
                BeatCount = beatCount;
            }
        }

        public static List<BeatInfo> DetectBeats(string mp3FilePath)
        {
            var approximateBPM = BPMGuestimator.EstimateBPM(mp3FilePath);
            Console.WriteLine($"Approximate BPM:{approximateBPM}");

      
            int stream = CreateStream(mp3FilePath);
            if (stream == 0)
            {
                throw new Exception($"Failed to load MP3 file: {Bass.BASS_ErrorGetCode()}");
            }

            try
            {
                // double approximateBPM = GetMetadataBpm(mp3FilePath);
                var rawBeats = ProcessBeats(stream, approximateBPM);

                return rawBeats;
            }
            finally
            {
                Bass.BASS_StreamFree(stream);
            }
        }
      
        private static int CreateStream(string mp3FilePath)
        {
            return Bass.BASS_StreamCreateFile(mp3FilePath, 0, 0, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
        }

        private static List<BeatInfo> ProcessBeats(int stream, double approximateBPM)
        {
            float beatEnergyThreshold = CalculateEnergyThreshold(stream);
            var beatTimes = DetectBeatTimes(stream, beatEnergyThreshold, approximateBPM);
            var beatIntervals = CalculateBeatIntervals(beatTimes);
            var beatsPerInterval = CalculateApproximateBeatsInInterval(beatIntervals, approximateBPM);

            var bpmValues = CalculateAdaptiveBPM(beatIntervals, beatsPerInterval, approximateBPM);

            return CreateBeatInfoList(beatTimes, bpmValues, beatIntervals, beatsPerInterval);
        }

        private static float CalculateEnergyThreshold(int stream)
        {
            var energies = SampleEnergies(stream, ENERGY_SAMPLE_TIME);
            return CalculateThresholdFromEnergies(energies);
        }

        private static List<float> SampleEnergies(int stream, double sampleTime)
        {
            float[] buffer = new float[BUFFER_LENGTH];
            List<float> energies = new List<float>();
            double currentTime = 0.0;

            while (currentTime < sampleTime && Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                int bytesRead = Bass.BASS_ChannelGetData(stream, buffer, BUFFER_LENGTH * 4 | (int)BASSData.BASS_DATA_FLOAT);
                if (bytesRead < 0) break;

                energies.Add(CalculateEnergy(buffer));
                currentTime = Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream));
            }

            Bass.BASS_ChannelSetPosition(stream, 0);
            return energies;
        }

        private static float CalculateThresholdFromEnergies(List<float> energies)
        {
            if (energies.Count == 0) return 0;

            float meanEnergy = energies.Average();
            float stdDev = (float)Math.Sqrt(energies.Average(v => Math.Pow(v - meanEnergy, 2)));
            return meanEnergy + stdDev;
        }

        private static List<double> DetectBeatTimes(int stream, float beatEnergyThreshold, double approximateBPM)
        {
            float[] buffer = new float[BUFFER_LENGTH];
            List<double> beatTimes = new List<double>();
            double lastBeatTime = 0;
            double minBeatInterval = (60.0 / approximateBPM) * 0.8;

            var firstBeat = DetectFirstBeat(stream, beatEnergyThreshold);
            if (firstBeat > 0)
            {
                beatTimes.Add(firstBeat);
                lastBeatTime = firstBeat;
            }

            while (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                int bytesRead = Bass.BASS_ChannelGetData(stream, buffer, BUFFER_LENGTH * 4 | (int)BASSData.BASS_DATA_FLOAT);
                if (bytesRead < 0) break;

                float energy = CalculateEnergy(buffer);
                double currentTime = Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream));

                if (energy > beatEnergyThreshold && (currentTime - lastBeatTime) > minBeatInterval)
                {
                    beatTimes.Add(currentTime);
                    lastBeatTime = currentTime;
                }
            }

            return beatTimes;
        }


        private static double DetectFirstBeat(int stream, float energyThreshold)
        {
            float[] buffer = new float[BUFFER_LENGTH];
            double firstBeatTime = -1;
            bool foundSignificantSample = false;
            int bytesRead;

            // Search for the first sample with energy over the threshold
            while (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                bytesRead = Bass.BASS_ChannelGetData(stream, buffer, BUFFER_LENGTH * 4 | (int)BASSData.BASS_DATA_FLOAT);
                if (bytesRead < 0) break;

                // Check if any sample in the buffer exceeds the energy threshold
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (Math.Abs(buffer[i]) > energyThreshold)
                    {
                        foundSignificantSample = true;
                        // Calculate the time of the first significant sample
                        long positionInBytes = Bass.BASS_ChannelGetPosition(stream);
                        double positionInSeconds = Bass.BASS_ChannelBytes2Seconds(stream, positionInBytes);

                        // Return the time of the first significant sample
                        firstBeatTime = positionInSeconds;
                        break;
                    }
                }

                if (foundSignificantSample) break;
            }

            // If no significant sample was found, return -1
            return firstBeatTime;
        }

        private static float CalculateEnergy(float[] buffer)
        {
            return buffer.Sum(sample => sample * sample) / buffer.Length;
        }

        private static List<double> CalculateBeatIntervals(List<double> beatTimes)
        {
            return beatTimes.Skip(1).Zip(beatTimes, (current, previous) => current - previous).ToList();
        }

        private static List<double> CalculateApproximateBeatsInInterval(List<double> beatIntervals, double approximateBPM)
        {
            List<double> beatsPerInterval = new List<double>();

            double secondsPerBeat = 60.0 / approximateBPM;

            foreach (double interval in beatIntervals)
            {
                // Calculate how many approximate beats fit into the interval
                double approximateBeatsInInterval = interval / secondsPerBeat;

                // Round to the nearest 0.25
                approximateBeatsInInterval = Math.Round(approximateBeatsInInterval * 8) / 8;

                beatsPerInterval.Add(approximateBeatsInInterval);
            }

            return beatsPerInterval;
        }
        private static List<double> CalculateAdaptiveBPM(List<double> beatIntervals, List<double> beatsPerInterval, double approximateBPM)
        {
            List<double> bpmValues = new List<double>();

            for (int i = 0; i < beatIntervals.Count; i++) 
            { 
                var beatInterval = beatIntervals[i];    
                var aproximateBeatsInInterval = beatsPerInterval[i];

                // Calculate BPM based on the adjusted number of beats in the interval
                double bpm = aproximateBeatsInInterval > 0
                    ? (60.0 * aproximateBeatsInInterval) / beatInterval
                    : approximateBPM; // Fallback to approximateBPM if something goes wrong

                bpmValues.Add(bpm);
            }

            return bpmValues;
        }

        private static List<BeatInfo> CreateBeatInfoList(List<double> beatTimes, List<double> bpmValues, List<double> beatIntervals, List<double> beatsPerInterval)
        {
            if (bpmValues.Count < 2) return new List<BeatInfo>();

            var beats = new List<BeatInfo>();

            for (int i = 0; i < beatTimes.Count - 1; i++)
            {
                double startTime = beatTimes[i];
                double endTime = beatTimes[i + 1];
                double bpm = bpmValues[i];
                double beatLength = beatIntervals[i];
                double approximateBeatCount = beatsPerInterval[i];
                beats.Add(new BeatInfo(startTime, endTime, beatLength, bpm, approximateBeatCount));
            }

            return beats;
        }
    }
}