using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.SectionDetector
{
    public static class VolumeAnalyzer
    {
        // Struct to hold volume information along with start and end times
        public struct VolumeInfo
        {
            public double StartTime { get; }
            public double EndTime { get; }
            public double AverageVolume { get; }
            public double AverageBassVolume { get; }
            public double AverageMidVolume { get; }
            public double AverageHighVolume { get; }

            // Constructor to initialize all fields, including time range and volume data
            public VolumeInfo(double startTime, double endTime, double averageVolume, double averageBassVolume, double averageMidVolume, double averageHighVolume)
            {
                StartTime = startTime;
                EndTime = endTime;
                AverageVolume = averageVolume;
                AverageBassVolume = averageBassVolume;
                AverageMidVolume = averageMidVolume;
                AverageHighVolume = averageHighVolume;
            }
        }

        private const int SAMPLE_RATE = 44100;
        private const int BUFFER_SIZE = 1024;
        private const double BASS_CUTOFF_HZ = 250;
        private const double MID_CUTOFF_HZ = 2000;

        // Function to compute average overall and banded (bass, mid, high) volume for a given section of an audio stream
        public static VolumeInfo GetAverageVolumes(int stream, double startTime, double endTime)
        {
            // Seek to the start position in the audio stream (in bytes)
            long startPosition = Bass.BASS_ChannelSeconds2Bytes(stream, startTime);
            long endPosition = Bass.BASS_ChannelSeconds2Bytes(stream, endTime);
            Bass.BASS_ChannelSetPosition(stream, startPosition);

            // Prepare to hold total volume sums and band-specific volume sums (bass, mid, high)
            double totalVolume = 0;
            double[] bandVolumes = new double[3]; // [bass, mid, high]
            int sampleCount = 0;

            float[] buffer = new float[BUFFER_SIZE]; // Temporary buffer for audio data
            Complex32[] fftBuffer = new Complex32[BUFFER_SIZE]; // Buffer for FFT processing

            // Calculate frequency per FFT bin
            double freqPerBin = (double)SAMPLE_RATE / BUFFER_SIZE;

            // Calculate bin indices for bass and mid frequency cutoffs
            int bassEndBin = (int)(BASS_CUTOFF_HZ / freqPerBin);
            int midEndBin = (int)(MID_CUTOFF_HZ / freqPerBin);

            // Process the stream in chunks until we reach the end position
            while (Bass.BASS_ChannelGetPosition(stream) < endPosition)
            {
                // Read audio data from the stream
                int bytesRead = Bass.BASS_ChannelGetData(stream, buffer, BUFFER_SIZE * sizeof(float) | (int)BASSData.BASS_DATA_FLOAT);
                if (bytesRead < 0) break; // Exit loop if no more data is available

                // Convert byte length to number of samples read
                int samplesRead = bytesRead / sizeof(float);

                // Merge left and right channels by converting each sample to absolute value
                for (int i = 0; i < samplesRead; i++)
                {
                    totalVolume += Math.Abs(buffer[i]); // Sum up absolute sample values (total volume)
                }

                // Prepare FFT buffer for frequency analysis (real values become complex)
                for (int i = 0; i < samplesRead && i < BUFFER_SIZE; i++)
                {
                    fftBuffer[i] = new Complex32(buffer[i], 0);
                }

                // Apply Fourier transform to convert time-domain samples to frequency domain
                Fourier.Forward(fftBuffer, FourierOptions.AsymmetricScaling);

                // Sum up magnitude (power) in each frequency band
                for (int i = 0; i < BUFFER_SIZE / 2; i++)
                {
                    double magnitude = fftBuffer[i].Magnitude;

                    if (i <= bassEndBin)
                    {
                        bandVolumes[0] += magnitude; // Bass band (up to 250Hz or custom)
                    }
                    else if (i > bassEndBin && i <= midEndBin)
                    {
                        bandVolumes[1] += magnitude; // Mid band (250Hz to 2kHz or custom)
                    }
                    else
                    {
                        bandVolumes[2] += magnitude; // High band (above 2kHz or custom)
                    }
                }

                sampleCount += samplesRead; // Keep track of the total number of samples processed
            }

            // Calculate the average volume by dividing the total sum by the number of samples
            // We scale the volume into the range 0-99 (i.e., map all values to this range)
            double scaledVolume = (totalVolume / sampleCount) * 99;
            double scaledBassVolume = (bandVolumes[0] / sampleCount) * 99;
            double scaledMidVolume = (bandVolumes[1] / sampleCount) * 99;
            double scaledHighVolume = (bandVolumes[2] / sampleCount) * 99;

            // Return a struct containing start/end times and all calculated volumes
            return new VolumeInfo(startTime, endTime, scaledVolume, scaledBassVolume, scaledMidVolume, scaledHighVolume);
        }
    }
}
