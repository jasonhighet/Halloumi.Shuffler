using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    public static class ExtenedAttributesHelper
    {
        private static readonly Dictionary<string, Dictionary<string, string>> CachedAttributes;

        static ExtenedAttributesHelper()
        {
            CachedAttributes = new Dictionary<string, Dictionary<string, string>>();
        }

        /// <summary>
        ///     Gets or sets the track extended attribute folder.
        /// </summary>
        public static string ExtendedAttributeFolder { get; set; }

        /// <summary>
        ///     Loads any attributes stored in a the track comment tag.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void LoadExtendedAttributes(Track track)
        {
            if (track == null) return;
            if (track.Artist == "" || track.Title == "") return;

            // DebugHelper.WriteLine("Loading Extended Attributes " + track.Description);

            var attributes = GetExtendedAttributes(track);
            if (attributes.ContainsKey("FadeIn"))
            {
                track.FadeInStart = track.SecondsToSamples(ConversionHelper.ToDouble(attributes["FadeIn"]));
            }
            if (attributes.ContainsKey("FadeOut"))
            {
                track.FadeOutStart = track.SecondsToSamples(ConversionHelper.ToDouble(attributes["FadeOut"]));
            }
            if (attributes.ContainsKey("BPMAdjust"))
            {
                track.BpmAdjustmentRatio = ConversionHelper.ToDecimal(attributes["BPMAdjust"]);
            }
            if (attributes.ContainsKey("FadeInLengthInSeconds"))
            {
                track.FadeInEnd = track.FadeInStart +
                                  track.SecondsToSamples(ConversionHelper.ToDouble(attributes["FadeInLengthInSeconds"]));
            }
            if (attributes.ContainsKey("FadeOutLengthInSeconds"))
            {
                track.FadeOutEnd = track.FadeOutStart +
                                   track.SecondsToSamples(ConversionHelper.ToDouble(attributes["FadeOutLengthInSeconds"]));
            }
            if (attributes.ContainsKey("PreFadeInStartVolume"))
            {
                track.PreFadeInStartVolume = ConversionHelper.ToFloat(attributes["PreFadeInStartVolume"])/100;
                track.UsePreFadeIn = true;
            }
            if (attributes.ContainsKey("PreFadeInPosition"))
            {
                track.PreFadeInStart = track.SecondsToSamples(ConversionHelper.ToDouble(attributes["PreFadeInPosition"]));
                track.UsePreFadeIn = true;
            }
            if (attributes.ContainsKey("PreFadeInStart"))
            {
                track.PreFadeInStart = track.SecondsToSamples(ConversionHelper.ToDouble(attributes["PreFadeInStart"]));
                track.UsePreFadeIn = true;
            }
            if (attributes.ContainsKey("StartBPM"))
            {
                track.StartBpm = BpmHelper.NormaliseBpm(ConversionHelper.ToDecimal(attributes["StartBPM"]));
            }
            if (attributes.ContainsKey("EndBPM"))
            {
                track.EndBpm = BpmHelper.NormaliseBpm(ConversionHelper.ToDecimal(attributes["EndBPM"]));
            }
            if (attributes.ContainsKey("Duration"))
            {
                if (track.Length == 0) track.Length = (long) (ConversionHelper.ToDouble(attributes["Duration"])*1000);
            }
            if (attributes.ContainsKey("PowerDown"))
            {
                track.PowerDownOnEnd = ConversionHelper.ToBoolean(attributes["PowerDown"]);
                track.PowerDownOnEndOriginal = track.PowerDownOnEnd;
            }
            if (attributes.ContainsKey("StartLoopCount"))
            {
                track.StartLoopCount = ConversionHelper.ToInt(attributes["StartLoopCount"]);
            }
            if (attributes.ContainsKey("EndLoopCount"))
            {
                track.EndLoopCount = ConversionHelper.ToInt(attributes["EndLoopCount"]);
            }
            if (attributes.ContainsKey("SkipStart"))
            {
                track.SkipStart = track.SecondsToSamples(ConversionHelper.ToDouble(attributes["SkipStart"]));
            }
            if (attributes.ContainsKey("SkipLengthInSeconds"))
            {
                track.SkipEnd = track.SkipStart +
                                track.SecondsToSamples(ConversionHelper.ToDouble(attributes["SkipLengthInSeconds"]));
            }
            if (attributes.ContainsKey("Rank"))
            {
                track.Rank = ConversionHelper.ToInt(attributes["Rank"], 1);
            }
            if (attributes.ContainsKey("Key"))
            {
                track.Key = attributes["Key"];
            }
        }

        /// <summary>
        ///     Gets the path of extended attribute file for the specified track
        /// </summary>
        /// <param name="trackDescription">The track description.</param>
        /// <returns>
        ///     A filename, including the full path
        /// </returns>
        public static string GetExtendedAttributeFile(string trackDescription)
        {
            var filename = trackDescription
                           + ".ExtendedAttributes.txt";
            filename = FileSystemHelper.StripInvalidFileNameChars(filename);
            return Path.Combine(ExtendedAttributeFolder, filename);
        }

        /// <summary>
        ///     Gets the shuffler attributes.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>
        ///     A collection of shuffler attributes
        /// </returns>
        private static Dictionary<string, string> GetExtendedAttributes(AudioStream track)
        {
            return GetExtendedAttributes(track.Description);
        }

        /// <summary>
        ///     Gets the shuffler attributes.
        /// </summary>
        /// <param name="trackDescription">The track description.</param>
        /// <returns>
        ///     A collection of shuffler attributes
        /// </returns>
        public static Dictionary<string, string> GetExtendedAttributes(string trackDescription)
        {
            var extendedAttributeFile = GetExtendedAttributeFile(trackDescription);

            if (CachedAttributes.ContainsKey(trackDescription))
                return CachedAttributes[trackDescription];

            var attributes = new Dictionary<string, string>();
            if (!File.Exists(extendedAttributeFile)) return attributes;

            var attributeLines = File.ReadAllText(extendedAttributeFile)
                .Split(';')
                .ToList()
                .Select(element => element.Split('=').ToList())
                .Where(items => items.Count > 1 && !attributes.ContainsKey(items[0].Trim()));

            foreach (var attributeLine in attributeLines)
            {
                attributes.Add(attributeLine[0].Trim(), attributeLine[1].Trim());
            }

            if (!CachedAttributes.ContainsKey(trackDescription))
                CachedAttributes.Add(trackDescription, attributes);

            return attributes;
        }

        /// <summary>
        ///     Saves the extended attributes.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="extendedAttributeFile">The extended attribute file.</param>
        public static void SaveExtendedAttributes(Dictionary<string, string> attributes, string extendedAttributeFile)
        {
            if (attributes.Count == 0)
            {
                if (File.Exists(extendedAttributeFile))
                    File.Delete((extendedAttributeFile));
            }
            else
            {
                var extendedAttributeData = attributes.Aggregate("",
                    (current, keyvalue) => current + $"{keyvalue.Key}={keyvalue.Value};");

                File.WriteAllText(extendedAttributeFile, extendedAttributeData, Encoding.UTF8);
            }
        }

        /// <summary>
        ///     Determines whether the specified track has an extended attribute file.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>
        ///     True if the specified track has an extended attribute file; otherwise, false.
        /// </returns>
        public static bool HasExtendedAttributeFile(Track track)
        {
            return File.Exists(GetExtendedAttributeFile(track.Description));
        }

        /// <summary>
        ///     Saves the track details to the track comment tag
        /// </summary>
        /// <param name="track">The track.</param>
        public static void SaveExtendedAttributes(Track track)
        {
            var attributes = new Dictionary<string, string>();

            if (track.StartBpm != 0)
                attributes.Add("StartBPM", $"{track.StartBpm}");

            if (track.EndBpm != 0)
                attributes.Add("EndBPM", $"{track.EndBpm}");

            attributes.Add("Duration", $"{track.LengthSeconds}");

            if (track.FadeInStart != 0)
                attributes.Add("FadeIn", $"{track.SamplesToSeconds(track.FadeInStart):0.000}");

            if (track.FadeOutStart != 0)
                attributes.Add("FadeOut", $"{track.SamplesToSeconds(track.FadeOutStart):0.000}");

            if (track.BpmAdjustmentRatio != 1)
                attributes.Add("BPMAdjust", $"{track.BpmAdjustmentRatio}");

            if (track.FadeInLength != 0)
                attributes.Add("FadeInLengthInSeconds", $"{track.FadeInLengthSeconds:0.000}");

            if (track.StartLoopCount > 0)
                attributes.Add("StartLoopCount", $"{track.StartLoopCount}");

            if (track.FadeOutLength != 0)
                attributes.Add("FadeOutLengthInSeconds", $"{track.SamplesToSeconds(track.FadeOutLength):0.000}");

            if (track.EndLoopCount > 0)
                attributes.Add("EndLoopCount", $"{track.EndLoopCount}");

            if (track.UsePreFadeIn)
            {
                attributes.Add("PreFadeInStartVolume", $"{track.PreFadeInStartVolume*100:00}");
                attributes.Add("PreFadeInPosition", $"{track.SamplesToSeconds(track.PreFadeInStart):0.000}");
            }

            if (track.PowerDownOnEnd) attributes.Add("PowerDown", $"{track.PowerDownOnEnd}");

            if (track.HasSkipSection)
            {
                attributes.Add("SkipStart", $"{track.SamplesToSeconds(track.SkipStart):0.000}");
                attributes.Add("SkipLengthInSeconds", $"{track.SamplesToSeconds(track.SkipLength):0.000}");
            }

            if (track.Rank != 1)
                attributes.Add("Rank", $"{track.Rank}");

            if (track.Key != "")
                attributes.Add("Key", track.Key);

            var extendedAttributeFile = GetExtendedAttributeFile(track.Description);
            SaveExtendedAttributes(attributes, extendedAttributeFile);
        }
    }
}