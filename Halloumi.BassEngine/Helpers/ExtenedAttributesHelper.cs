using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    public static class ExtenedAttributesHelper
    {
        private static readonly Dictionary<string, ExtenedAttributes> AllAttributes = new Dictionary<string, ExtenedAttributes>();

        //public static void LoadAllExtendedAttributes(List<string> trackDescriptions)
        //{
        //    foreach (var trackDescription in trackDescriptions)
        //    {
        //        var filepath = GetExtendedAttributeFile(trackDescription);
        //        if (!File.Exists(filepath)) continue;

        //        var attributeDictionary = GetExtendedAttributes(trackDescription);

        //        var extendedAttributed = new ExtenedAttributes() {Track = trackDescription};
        //        extendedAttributed.SetAttributeDictionary(attributeDictionary);

        //        AllAttributes.Add(trackDescription, extendedAttributed);
        //    }
        //}

        public static void LoadFromDatabase()
        {
            AllAttributes.Clear();
            var filepath = ExtendedAttributesFile;
            if (!File.Exists(filepath))
                return;

            var allAttributes = SerializationHelper<List<ExtenedAttributes>>.FromXmlFile(filepath);
            foreach (var attributes in allAttributes)
            {
                AllAttributes.Add(attributes.Track, attributes);
            }
        }

        public static void SaveToDatabase()
        {
            var attributes = AllAttributes.Select(x => x.Value).ToList().OrderBy(x => x.Track).ToList();
            var filepath = ExtendedAttributesFile;
            SerializationHelper<List<ExtenedAttributes>>.ToXmlFile(attributes, filepath);
        }

        private static string ExtendedAttributesFile => Path.Combine(ShufflerFolder, "Haloumi.Shuffler.ExtendedAtrributes.xml");

        /// <summary>
        ///     Gets or sets the track extended attribute folder.
        /// </summary>
        public static string ShufflerFolder { get; set; }

        /// <summary>
        ///     Loads any attributes stored in a the track comment tag.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void LoadExtendedAttributes(Track track)
        {
            if (track == null) return;
            if (track.Artist == "" || track.Title == "") return;

            // DebugHelper.WriteLine("Loading Extended Attributes " + track.Description);

            var attributes = GetExtendedAttributes(track.Description);
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
        ///     Gets the shuffler attributes.
        /// </summary>
        /// <param name="trackDescription">The track description.</param>
        /// <returns>
        ///     A collection of shuffler attributes
        /// </returns>
       public static Dictionary<string, string> GetExtendedAttributes(string trackDescription)
        {
            if (trackDescription == "") return null;
            return AllAttributes.ContainsKey(trackDescription)
                ? AllAttributes[trackDescription].GetAttributeDictionary()
                : new Dictionary<string, string>();
        }

        public static void ClearExtendedAttributes(string trackDescription)
        {
            SaveExtendedAttributes(trackDescription, new Dictionary<string, string>());
        }

        /// <summary>
        /// Saves the extended attributes.
        /// </summary>
        /// <param name="trackDescription">The track description.</param>
        /// <param name="attributes">The attributes.</param>
        public static void SaveExtendedAttributes(string trackDescription, Dictionary<string, string> attributes)
        {
            SetExtendedAttributes(trackDescription, attributes);

            Task.Run(() => SaveToDatabase());
        }

        private static void SetExtendedAttributes(string trackDescription, Dictionary<string, string> attributes)
        {
            if (AllAttributes.ContainsKey(trackDescription))
            {
                if (attributes.Count == 0)
                {
                    AllAttributes.Remove(trackDescription);
                }
                else
                {
                    var extendedAttributes = new ExtenedAttributes() { Track = trackDescription };
                    extendedAttributes.SetAttributeDictionary(attributes);
                    AllAttributes[trackDescription] = extendedAttributes;
                }
            }
            else if (attributes.Count > 0)
            {
                var extendedAttributes = new ExtenedAttributes() { Track = trackDescription };
                extendedAttributes.SetAttributeDictionary(attributes);
            }
        }

        /// <summary>
        /// Determines whether the specified track has an extended attribute file.
        /// </summary>
        /// <param name="trackDescription">The track description.</param>
        /// <returns>
        /// True if the specified track has an extended attribute file; otherwise, false.
        /// </returns>
        public static bool HasExtendedAttributes(string trackDescription)
        {
            return AllAttributes.ContainsKey(trackDescription);
        }

        /// <summary>
        ///     Sets a track attribute, but does not save it
        /// </summary>
        /// <param name="track">The track.</param>
        public static void SetExtendedAttribute(string trackDescription, string key, string value)
        {
            var attributes = GetExtendedAttributes(trackDescription);
            if (attributes.ContainsKey(key))
                attributes.Remove(key);

            if (!string.IsNullOrEmpty(value))
                attributes.Add(key, value);

            SetExtendedAttributes(trackDescription, attributes);
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

            SaveExtendedAttributes(track.Description, attributes);
        }

        public class ExtenedAttributes
        {
            public string Track { get; set; }

            public string Attributes { get; set; }

            public Dictionary<string, string> GetAttributeDictionary()
            {
                var attributeDictionary = new Dictionary<string, string>();

                var attributeLines = Attributes
                    .Split(';')
                    .ToList()
                    .Select(element => element.Split('=').ToList())
                    .Where(items => items.Count > 1 && !attributeDictionary.ContainsKey(items[0].Trim()));

                foreach (var attributeLine in attributeLines)
                {
                    attributeDictionary.Add(attributeLine[0].Trim(), attributeLine[1].Trim());
                }

                return attributeDictionary;
            }

            public void SetAttributeDictionary(Dictionary<string, string> attributeDictionary)
            {
                Attributes = "";
                var keyValues = attributeDictionary
                    .ToList()
                    .OrderBy(x => x.Key)
                    .Where(x => !string.IsNullOrEmpty(x.Value));

                Attributes = keyValues.Aggregate("",
                    (current, keyvalue) => current + $"{keyvalue.Key}={keyvalue.Value};");
            }
        }
    }
}