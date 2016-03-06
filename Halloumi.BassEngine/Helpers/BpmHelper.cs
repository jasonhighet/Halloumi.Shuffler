using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public static class BpmHelper
    {
        /// <summary>
        ///     Determines whether BPM value is in a percentage range of another BPM value
        /// </summary>
        /// <param name="bpm1">The initial BPM.</param>
        /// <param name="bpm2">The BPM being matched.</param>
        /// <param name="percentVariance">The percent variance.</param>
        /// <returns>True if BPM2 is in range of BMP1</returns>
        public static bool IsBpmInRange(decimal bpm1, decimal bpm2, decimal percentVariance)
        {
            percentVariance = Math.Abs(percentVariance);
            return (GetAbsoluteBpmPercentChange(bpm1, bpm2) <= percentVariance);
        }

        /// <summary>
        ///     Gets the average of two BPMs. If one of the BPMs is close to double the other it is halved for averaging purposes.
        /// </summary>
        /// <param name="bpm1">The first BPM.</param>
        /// <param name="bpm2">The second BPM2.</param>
        /// <returns>The average BPM</returns>
        public static decimal GetAdjustedBpmAverage(decimal bpm1, decimal bpm2)
        {
            var bpms = new List<decimal>
            {
                bpm1,
                bpm2
            }
                .OrderBy(bpm => bpm)
                .ToList();

            var diff = GetAdjustedBpmPercentChange(bpms[0], bpms[1]);
            var multiplier = 1M + (diff/100);
            bpms[1] = bpms[0]*multiplier;
            return bpms.Average();
        }

        /// <summary>
        ///     Gets the BPM change between two values as percent (-100 to 100).
        /// </summary>
        /// <param name="bpm1">The first BPM.</param>
        /// <param name="bpm2">The second BPM2.</param>
        /// <returns>The BPM change as a percent (-100 - 100)</returns>
        public static decimal GetAdjustedBpmPercentChange(decimal bpm1, decimal bpm2)
        {
            if (bpm1 == 0M || bpm2 == 0M) return 100M;
            if (bpm1 == bpm2) return 0M;

            var percentChanges = new List<decimal>
            {
                GetBpmPercentChange(bpm1, bpm2),
                GetBpmPercentChange(bpm1, bpm2/2),
                GetBpmPercentChange(bpm1, bpm2*2)
            };

            var minPercentChange = percentChanges
                .OrderBy(Math.Abs)
                .ToList()[0];

            return minPercentChange;
        }

        /// <summary>
        ///     Gets the BPM change between two values as percent (-100 to 100).
        /// </summary>
        /// <param name="bpm1">The first BPM.</param>
        /// <param name="bpm2">The second BPM2.</param>
        /// <returns>The BPM change as a percent (-100 - 100)</returns>
        public static decimal GetBpmPercentChange(decimal bpm1, decimal bpm2)
        {
            if (bpm1 == 0M || bpm2 == 0M) return 100M;
            var bpmDiff = bpm2 - bpm1;
            var percentChange = (bpmDiff/bpm2)*100;

            return percentChange;
        }

        /// <summary>
        ///     Gets the absolute BPM change between two values as percent (0 - 100).
        /// </summary>
        /// <param name="bpm1">The first BPM.</param>
        /// <param name="bpm2">The second BPM2.</param>
        /// <returns>The BPM change as a percent (0 - 100)</returns>
        public static decimal GetAbsoluteBpmPercentChange(decimal bpm1, decimal bpm2)
        {
            return Math.Abs(GetAdjustedBpmPercentChange(bpm1, bpm2));
        }

        public static List<double> GetLoopLengths(decimal bpm)
        {
            if (bpm == 0) return new List<double>();

            var loopLengths = new List<double>();

            // scale BPM to be between 70 and 140
            bpm = NormaliseBpm(bpm);

            var bps = ((double) bpm)/60;
            var spb = 1/bps;

            loopLengths.Add(spb*4);
            loopLengths.Add(spb*8);
            loopLengths.Add(spb*16);
            loopLengths.Add(spb*32);
            loopLengths.Add(spb*64);

            return loopLengths;
        }

        /// <summary>
        ///     Gets the default length of the loop.
        /// </summary>
        /// <param name="bpm">The BPM.</param>
        /// <returns></returns>
        public static double GetDefaultLoopLength(decimal bpm)
        {
            return bpm == 0 ? 10 : GetLoopLengths(bpm)[2];
        }

        /// <summary>
        ///     Gets the default delay time. (1/4 note delay)
        /// </summary>
        /// <param name="bpm">The BPM.</param>
        /// <returns>The default delay time from the BPM (1/4 note delay)</returns>
        public static double GetDefaultDelayLength(decimal bpm)
        {
            bpm = NormaliseBpm(bpm);
            return (1D/((double) bpm/60D))*1000D;
        }

        /// <summary>
        ///     Gets the loop length for the specified BPM that is closest to the preferred length
        /// </summary>
        /// <param name="bpm">The BPM.</param>
        /// <param name="preferredLength">Preferred loop length.</param>
        /// <returns>A BPM loop length</returns>
        public static double GetBestFitLoopLength(decimal bpm, double preferredLength)
        {
            if (bpm == 0M) return preferredLength;

            var loopLengths = GetLoopLengths(bpm);
            var selectedLoopLengthIndex = 2;

            for (var i = 0; i < loopLengths.Count; i++)
            {
                var difference = Math.Abs(preferredLength - loopLengths[i]);
                var selectedIndexDifference = Math.Abs(preferredLength - loopLengths[selectedLoopLengthIndex]);
                if (difference < selectedIndexDifference)
                {
                    selectedLoopLengthIndex = i;
                }
            }
            return loopLengths[selectedLoopLengthIndex];
        }

        /// <summary>
        ///     Gets the BPM of loop.
        /// </summary>
        /// <param name="loopLength">Length of the loop in seconds.</param>
        /// <returns>The BPM of the loop</returns>
        public static decimal GetBpmFromLoopLength(double loopLength)
        {
            if (loopLength == 0) return 0;
            var spb = loopLength/16;
            var bps = 1/spb;
            var bpm = bps*60;

            return NormaliseBpm((decimal) bpm);
        }

        /// <summary>
        ///     Normalizes a BPM value by scaling it to be between 70 and 140
        /// </summary>
        /// <param name="bpm">The BPM.</param>
        /// <returns>The scaled BPM</returns>
        public static decimal NormaliseBpm(decimal bpm)
        {
            if (bpm == 0) bpm = 100;
            bpm = Math.Abs(bpm);

            const decimal upper = 136.5M;
            const decimal lower = upper/2;

            while (bpm < lower || bpm > upper)
            {
                if (bpm > upper) bpm = bpm/2;
                if (bpm < lower) bpm = bpm*2;
            }

            return bpm;
        }

        public static double GetFullEndLoopLengthAdjustedToMatchAnotherTrack(Track track1, Track track2)
        {
            if (track1 == null && track2 == null) return 10d;
            if (track2 == null) return track1.FullEndLoopLengthSeconds;
            return track1 == null
                ? track2.FullEndLoopLengthSeconds
                : GetLengthAdjustedToMatchAnotherTrack(track1, track2, track1.FullEndLoopLengthSeconds);
        }

        public static double GetLengthAdjustedToMatchAnotherTrack(Track track1, Track track2, double length)
        {
            if (track1 == null || track2 == null) return length;
            var ratio = GetTrackTempoChangeAsRatio(track2, track1);
            return length * ratio;
        }

        /// <summary>
        ///     Gets the track tempo change as a ratio (i.e. 1.02, .97 etc)
        /// </summary>
        /// <param name="track1">The track being fading out</param>
        /// <param name="track2">The track being faded into.</param>
        /// <returns>The ratio the first track needs to be multiplied by to in order to match the second track</returns>
        public static float GetTrackTempoChangeAsRatio(Track track1, Track track2)
        {
            if (track1 == null || track2 == null) return 1f;

            var percentChange = (float)(BpmHelper.GetAdjustedBpmPercentChange(track1.EndBpm, track2.StartBpm));

            return (1 + percentChange / 100f);
        }


    }
}