using System;
using System.Collections.Generic;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    /// <summary>
    ///     A track's loudest sample in each short stretch of time, for drawing an overview waveform.
    /// </summary>
    public class WaveformPeaks
    {
        public const int PeaksPerSecond = 20;

        /// <summary>
        ///     One value per <see cref="SecondsPerPeak" />, scaled so the loudest peak in the track is 1.
        /// </summary>
        public float[] Peaks { get; set; }

        public double SecondsPerPeak { get; set; }

        /// <summary>
        ///     The loudest peak between two file positions in seconds, 0 to 1.
        /// </summary>
        public float GetPeak(double startSeconds, double endSeconds)
        {
            if (Peaks == null || Peaks.Length == 0 || SecondsPerPeak <= 0) return 0;

            var first = Math.Max(0, (int)(startSeconds / SecondsPerPeak));
            var last = Math.Min(Peaks.Length - 1, (int)(endSeconds / SecondsPerPeak));

            var peak = 0F;
            for (var i = first; i <= last; i++)
                if (Peaks[i] > peak) peak = Peaks[i];
            return peak;
        }

        /// <summary>
        ///     Decodes a whole audio file and returns its peaks, or null if it cannot be decoded. Takes about a
        ///     second for a five-minute track: call it from a worker thread, never the UI or mixer thread.
        /// </summary>
        public static WaveformPeaks Compute(string filename)
        {
            var channel = Bass.BASS_StreamCreateFile(filename, 0L, 0L,
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE);
            if (channel == 0) return null;

            try
            {
                var info = Bass.BASS_ChannelGetInfo(channel);
                if (info == null || info.freq <= 0 || info.chans <= 0) return null;

                var framesPerPeak = Math.Max(1, info.freq / PeaksPerSecond);
                var channels = info.chans;
                var buffer = new float[framesPerPeak * channels * 8];
                var peaks = new List<float>();

                var peak = 0F;
                var frames = 0;
                while (true)
                {
                    var bytes = Bass.BASS_ChannelGetData(channel, buffer, buffer.Length * sizeof(float));
                    if (bytes <= 0) break;

                    var count = bytes / sizeof(float);
                    for (var i = 0; i + channels <= count; i += channels)
                    {
                        for (var c = 0; c < channels; c++)
                        {
                            var value = Math.Abs(buffer[i + c]);
                            if (value > peak) peak = value;
                        }

                        if (++frames < framesPerPeak) continue;
                        peaks.Add(peak);
                        peak = 0;
                        frames = 0;
                    }
                }

                if (frames > 0) peaks.Add(peak);

                var max = 0F;
                foreach (var value in peaks)
                    if (value > max) max = value;
                if (max > 0)
                    for (var i = 0; i < peaks.Count; i++)
                        peaks[i] = Math.Min(1F, peaks[i] / max);

                return new WaveformPeaks
                {
                    Peaks = peaks.ToArray(),
                    SecondsPerPeak = framesPerPeak / (double)info.freq
                };
            }
            finally
            {
                Bass.BASS_StreamFree(channel);
            }
        }
    }
}
