using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.SectionDetector
{

    public static class BPMGuestimator
    {
        private const int BUFFER_LENGTH = 1024; // Buffer size
        private const double SECONDS_TO_SAMPLE = 20.0; // Duration of each sample (in seconds)
        private const float BEAT_ENERGY_THRESHOLD = 0.1f; // Beat detection energy threshold

        public static double EstimateBPM(string filePath)
        {
            int stream = Bass.BASS_StreamCreateFile(filePath, 0L, 0L, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
            if (stream == 0)
            {
                throw new Exception("Error loading file.");
            }

            // Get total duration of the track
            double totalDuration = Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream, BASSMode.BASS_POS_BYTE));

            // Detect beats and calculate BPM in the start, middle, and end sections
            var bpmStart = DetectAndCalculateBPM(stream, 10, SECONDS_TO_SAMPLE);
            var bpmMiddle = DetectAndCalculateBPM(stream, totalDuration / 2 - SECONDS_TO_SAMPLE / 2, SECONDS_TO_SAMPLE);
            var bpmEnd = DetectAndCalculateBPM(stream, totalDuration - SECONDS_TO_SAMPLE, SECONDS_TO_SAMPLE);

            // Calculate average BPM from all sections
            double averageBPM = (bpmStart + bpmMiddle + bpmEnd) / 3.0;

            // Free resources
            Bass.BASS_StreamFree(stream);

            return averageBPM;
        }

        public static double EstimateBPM(string filePath, double startSec)
        {
            int stream = Bass.BASS_StreamCreateFile(filePath, 0L, 0L, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
            if (stream == 0)
            {
                throw new Exception("Error loading file.");
            }

            // Get total duration of the track
            double totalDuration = Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream, BASSMode.BASS_POS_BYTE));

            if (startSec + SECONDS_TO_SAMPLE > totalDuration)
            {
                startSec = totalDuration - SECONDS_TO_SAMPLE;   
            }

            if (startSec < 0)
                startSec = 0;

            var bpm = DetectAndCalculateBPM(stream, startSec, SECONDS_TO_SAMPLE);

              // Free resources
            Bass.BASS_StreamFree(stream);

            return bpm;
        }

        // Function to detect beats in a section and calculate BPM based on the intervals between them
        private static double DetectAndCalculateBPM(int stream, double startSec, double durationSec)
        {
            float[] buffer = new float[BUFFER_LENGTH];
            List<double> beatTimes = new List<double>();

            // Seek to the start position
            Bass.BASS_ChannelSetPosition(stream, Bass.BASS_ChannelSeconds2Bytes(stream, startSec));

            double lastBeatTime = 0;
            double endSec = startSec + durationSec;

            while (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING && Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream)) < endSec)
            {
                int bytesRead = Bass.BASS_ChannelGetData(stream, buffer, BUFFER_LENGTH * 4 | (int)BASSData.BASS_DATA_FLOAT);
                if (bytesRead < 0) break;

                float energy = CalculateEnergy(buffer);
                double currentTime = Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream));

                if (energy > BEAT_ENERGY_THRESHOLD && (currentTime - lastBeatTime) > 0.3) // Minimum time between beats
                {
                    beatTimes.Add(currentTime);
                    lastBeatTime = currentTime;
                }
            }

            // Calculate BPM based on the average time between detected beats
            if (beatTimes.Count < 2)
            {
                return 0; // Not enough beats detected
            }

            List<double> intervals = new List<double>();
            for (int i = 1; i < beatTimes.Count; i++)
            {
                intervals.Add(beatTimes[i] - beatTimes[i - 1]);
            }

            // Calculate average interval and convert it to BPM
            double averageInterval = intervals.Average();
            double bpm = 60.0 / averageInterval;

            return bpm;
        }

        // Simple function to calculate energy in a buffer (sum of squares)
        private static float CalculateEnergy(float[] buffer)
        {
            float energy = 0;
            foreach (var sample in buffer)
            {
                energy += sample * sample;
            }
            return energy / buffer.Length;
        }
    }
}