using System;
using System.Collections.Generic;
using System.Linq;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.SectionDetector
{


    public static class BeatInspector
    {
        private const int BUFFER_LENGTH = 256; // Buffer size
        private const int HISTORY_LENGTH = 43; // Roughly 1 second of history at 44.1kHz
        private const float ONSET_THRESHOLD = 0.1f; // Onset detection threshold

        public static double FindFirstBeat(int streamId, double startTime, double endTime)
        {
            float[] buffer = new float[BUFFER_LENGTH];
            Queue<float> energyHistory = new Queue<float>(HISTORY_LENGTH);

            // Initialize energy history
            for (int i = 0; i < HISTORY_LENGTH; i++)
            {
                energyHistory.Enqueue(0);
            }

            // Set the channel position to the start time
            Bass.BASS_ChannelSetPosition(streamId, Bass.BASS_ChannelSeconds2Bytes(streamId, startTime));

            double currentTime = startTime;

            while (Bass.BASS_ChannelIsActive(streamId) == BASSActive.BASS_ACTIVE_PLAYING && currentTime < endTime)
            {
                int bytesRead = Bass.BASS_ChannelGetData(streamId, buffer, BUFFER_LENGTH * 4 | (int)BASSData.BASS_DATA_FLOAT);
                if (bytesRead < 0) break;

                float energy = CalculateEnergy(buffer);
                energyHistory.Dequeue();
                energyHistory.Enqueue(energy);

                currentTime = Bass.BASS_ChannelBytes2Seconds(streamId, Bass.BASS_ChannelGetPosition(streamId));

                if (IsOnset(energyHistory))
                {
                    return currentTime; // Return the time of the first detected onset
                }
            }

            return -1; // No onset found
        }

        // Function to calculate energy in a buffer (sum of squares)
        private static float CalculateEnergy(float[] buffer)
        {
            return buffer.Sum(sample => sample * sample) / buffer.Length;
        }

        // Function to detect onset using local energy and spectral difference
        private static bool IsOnset(Queue<float> energyHistory)
        {
            float[] history = energyHistory.ToArray();
            int midPoint = history.Length / 2;

            // Calculate local energy
            float localEnergy = history[midPoint];
            float averageEnergy = history.Take(midPoint).Average();

            // Calculate spectral difference (simplified as energy difference in this case)
            float spectralDifference = localEnergy - averageEnergy;

            // Onset is detected if the spectral difference exceeds the threshold
            return spectralDifference > ONSET_THRESHOLD;
        }
    }
}
