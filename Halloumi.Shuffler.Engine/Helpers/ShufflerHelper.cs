using System;
using System.Collections.Generic;
using System.Globalization;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.AudioLibrary.Helpers
{
    public static class ShufflerHelper
    {
        public static string ShufflerFolder
        {
            get { return ExtendedAttributesHelper.ShufflerFolder; }
        }

        /// <summary>
        ///     Updates the Shuffler files after a track has been changed.
        ///     Assumes the OriginalDescription is the old description,
        ///     and that the ShufflerAttribuesFile/ShufflerMixesFile properties
        ///     point to the old files
        /// </summary>
        /// <param name="track">The track.</param>
        public static void RenameShufferFiles(Track track)
        {
            try
            {
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Loads the shuffler details for a track
        /// </summary>
        /// <param name="track">The track.</param>
        public static Dictionary<string, string> LoadShufflerDetails(Track track)
        {
            track.Key = KeyHelper.ParseKey(track.Key);

            track.IsShufflerTrack = ExtendedAttributesHelper.HasExtendedAttributes(track.Description);

            track.DateShuffled = null;
            track.DateShuffledEarliest = null;
            track.DateShuffledIsEstimate = false;

            if (!track.IsShufflerTrack) return null;

            var attributes = ExtendedAttributesHelper.GetExtendedAttributes(track.Description);

            if (attributes.ContainsKey(ExtendedAttributesHelper.ShuffledKey))
                LoadDateShuffled(track, attributes[ExtendedAttributesHelper.ShuffledKey]);

            if (attributes.ContainsKey("Rank")) track.Rank = ConversionHelper.ToInt(attributes["Rank"], 1);

            decimal start = 0;
            if (attributes.ContainsKey("FadeIn")) start = ConversionHelper.ToDecimal(attributes["FadeIn"], start);
            var end = track.Length;
            if (attributes.ContainsKey("FadeOut")) end = ConversionHelper.ToDecimal(attributes["FadeOut"], end);
            var length = end - start;

            var inLoopCount = 0;
            if (attributes.ContainsKey("StartLoopCount"))
                inLoopCount = ConversionHelper.ToInt(attributes["StartLoopCount"], inLoopCount);

            decimal inLoopLength = 0;
            if (attributes.ContainsKey("FadeInLengthInSeconds"))
                inLoopLength = ConversionHelper.ToDecimal(attributes["FadeInLengthInSeconds"]);
            if (inLoopLength > 0) track.StartBpm = BpmHelper.GetBpmFromLoopLength(Convert.ToDouble(inLoopLength));

            inLoopCount = inLoopCount - 1;
            if (inLoopCount > 0) length = length + inLoopCount*inLoopLength;

            decimal skipLength = 0;
            if (attributes.ContainsKey("SkipLengthInSeconds"))
                skipLength = ConversionHelper.ToDecimal(attributes["SkipLengthInSeconds"]);
            if (skipLength > 0) length = length - skipLength;

            track.PowerDown = false;
            if (attributes.ContainsKey("PowerDown"))
                track.PowerDown = ConversionHelper.ToBoolean(attributes["PowerDown"]);

            if (attributes.ContainsKey("Key")) track.Key = KeyHelper.ParseKey(attributes["Key"]);

            decimal outLoopLength = 0;
            if (attributes.ContainsKey("FadeOutLengthInSeconds"))
                outLoopLength = ConversionHelper.ToDecimal(attributes["FadeOutLengthInSeconds"], 0);
            if (outLoopLength > 0) track.EndBpm = BpmHelper.GetBpmFromLoopLength(Convert.ToDouble(outLoopLength));

            track.Length = length;

            if (attributes.ContainsKey("StartBPM"))
                track.StartBpm = BpmHelper.NormaliseBpm(ConversionHelper.ToDecimal(attributes["StartBPM"], track.Bpm));
            if (attributes.ContainsKey("EndBPM"))
                track.EndBpm = BpmHelper.NormaliseBpm(ConversionHelper.ToDecimal(attributes["EndBPM"], track.Bpm));

            track.Bpm = BpmHelper.GetAdjustedBpmAverage(track.StartBpm, track.EndBpm);

            return attributes;
        }

        /// <summary>
        ///     Parses the Shuffled attribute: "2018-05-02", "2024-03-01..2024-09-27" (a feasible range,
        ///     backfilled from history) or "..2016-04-10" (shuffled on or before that date).
        /// </summary>
        private static void LoadDateShuffled(Track track, string value)
        {
            var parts = value.Split(new[] {".."}, StringSplitOptions.None);
            var latest = ParseDate(parts[parts.Length - 1]);
            if (latest == null) return;

            track.DateShuffled = latest;
            if (parts.Length < 2) return;

            track.DateShuffledIsEstimate = true;
            track.DateShuffledEarliest = ParseDate(parts[0]);
        }

        private static DateTime? ParseDate(string value)
        {
            return DateTime.TryParseExact(value.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var date)
                ? date
                : (DateTime?) null;
        }
    }
}