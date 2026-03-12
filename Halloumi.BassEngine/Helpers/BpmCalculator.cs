using System;
using System.IO;
using IdSharp.Tagging.ID3v2;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    public static class BpmCalculator
    {
        private const int MinBpm = 45;
        private const int MaxBpm = 230;

        /// <summary>
        ///     Detects the BPM of an MP3 file using the BASS_FX BPM algorithm and writes the result to the ID3 tag.
        ///     Uses bass_fx.dll which is already included in the project.
        /// </summary>
        /// <summary>
        ///     Detects BPM and writes it to the ID3 tag. Returns the detected BPM, or 0 on failure.
        /// </summary>
        public static float CalculateBpm(string trackFileName)
        {
            if (!File.Exists(trackFileName)) return 0;

            var stream = Bass.BASS_StreamCreateFile(trackFileName, 0, 0,
                BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
            if (stream == 0) return 0;

            float bpm;
            try
            {
                var totalSeconds = Bass.BASS_ChannelBytes2Seconds(stream,
                    Bass.BASS_ChannelGetLength(stream, BASSMode.BASS_POS_BYTE));

                // Analyse the full track; skip only a short intro if the track is long enough
                var startSec = totalSeconds > 60.0 ? 10.0 : 0.0;
                var endSec = totalSeconds;

                // Min/max BPM packed as a single int: high 16 bits = max, low 16 bits = min
                var minMaxBpm = (MaxBpm << 16) | MinBpm;

                // BASS_FX_BPM_MULT2: auto-doubles the result if it lands below MinBpm*2,
                // which corrects half-tempo octave errors common in D&B / fast electronic music
                bpm = BassFx.BASS_FX_BPM_DecodeGet(stream, startSec, endSec,
                    minMaxBpm, BASSFXBpm.BASS_FX_BPM_MULT2, null, IntPtr.Zero);
            }
            finally
            {
                Bass.BASS_StreamFree(stream);
            }

            if (bpm <= 0) return 0;

            var tags = new ID3v2Tag(trackFileName) { BPM = bpm.ToString("0.00") };
            tags.Save(trackFileName);

            File.SetLastWriteTime(trackFileName, DateTime.Now);

            return bpm;
        }
    }
}
