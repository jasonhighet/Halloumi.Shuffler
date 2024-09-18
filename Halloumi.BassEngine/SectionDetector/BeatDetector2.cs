using Halloumi.Shuffler.AudioEngine.SectionDetector;
using System;
using System.Collections.Generic;
using System.Linq;
using Un4seen.Bass;

public static class BeatDetector2
{
    private const int BUFFER_LENGTH = 1024;
    private const float BEAT_ENERGY_THRESHOLD = 0.1f;
    private const double INITIAL_WINDOW = 5.0; // Look for the first beat within the first 5 seconds

    public static List<double> DetectBeats(string mp3FilePath, double approximateBpm)
    {
        List<double> beatTimes = new List<double>();

        int stream = Bass.BASS_StreamCreateFile(mp3FilePath, 0L, 0L, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
        if (stream == 0)
        {
            throw new Exception("Error loading file.");
        }

        try
        {

            double totalDuration = Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream, BASSMode.BASS_POS_BYTE));


            double windowStart = 0;
            while (true) 
            {
                var windowEnd = windowStart + 4;
                if (windowEnd > totalDuration) windowEnd = totalDuration;

                var nextBeat = BeatInspector.FindFirstBeat(stream, windowStart, windowEnd);

                if (nextBeat != -1)
                {
                    beatTimes.Add(nextBeat);
                    windowStart = nextBeat + 0.2;
                }
                else 
                {
                    windowStart = windowEnd;
                }

                if(windowStart >= totalDuration) break;
            }

        }
        finally
        {
            Bass.BASS_StreamFree(stream);
        }

        return beatTimes;
    }

    private static double FindFirstBeat(int stream, double windowDuration)
    {
        float[] buffer = new float[BUFFER_LENGTH];
        double highestEnergy = 0;
        double firstBeatTime = -1;

        Bass.BASS_ChannelSetPosition(stream, 0);

        double currentTime = 0;
        while (currentTime < windowDuration)
        {
            int bytesRead = Bass.BASS_ChannelGetData(stream, buffer, BUFFER_LENGTH * 4 | (int)BASSData.BASS_DATA_FLOAT);
            if (bytesRead < 0) break;

            float energy = CalculateEnergy(buffer);
            currentTime = Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream));

            if (energy > highestEnergy)
            {
                highestEnergy = energy;
                firstBeatTime = currentTime;
            }
        }

        // Reset stream position
        Bass.BASS_ChannelSetPosition(stream, Bass.BASS_ChannelSeconds2Bytes(stream, firstBeatTime));

        return firstBeatTime;
    }

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