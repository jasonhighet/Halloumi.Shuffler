using System;
using System.IO;
using IdSharp.Tagging.ID3v2;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    public static class BpmCalculator
    {
        /// <summary>
        ///     Detects the BPM of an MP3 file and writes it to the ID3 tag. Returns the detected BPM, or 0 on failure.
        ///     Uses <see cref="BeatAnalysis" /> (the BASS_FX detector this replaced was unreliable). The tempo
        ///     is returned as detected, between 70 and 180: normalise it before using it as a Shuffler BPM.
        /// </summary>
        public static float CalculateBpm(string trackFileName)
        {
            if (!File.Exists(trackFileName)) return 0;

            var bpm = DetectBpm(trackFileName);
            if (bpm <= 0) return 0;

            var tags = new ID3v2Tag(trackFileName) { BPM = bpm.ToString("0.00") };
            tags.Save(trackFileName);

            File.SetLastWriteTime(trackFileName, DateTime.Now);

            return bpm;
        }

        /// <summary>
        ///     Detects the tempo of a whole track without writing anything. Returns 0 on failure.
        /// </summary>
        public static float DetectBpm(string trackFileName)
        {
            var analysis = BeatAnalysis.Analyse(trackFileName);
            if (analysis == null) return 0;

            // skip a short intro if the track is long enough, as intros often have no beat
            var start = analysis.End - analysis.Start > 60.0 ? analysis.Start + 10.0 : analysis.Start;
            return (float) analysis.DetectBpm(start, analysis.End);
        }
    }
}
