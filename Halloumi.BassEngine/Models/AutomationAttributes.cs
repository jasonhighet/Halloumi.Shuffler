using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Common.Helpers;

namespace Halloumi.Shuffler.AudioEngine.Models
{
    public class AutomationAttributes
    {
        private static readonly Dictionary<string, AutomationAttributes> CachedAttributes =
            new Dictionary<string, AutomationAttributes>();

        private static readonly Dictionary<string, AutomationAttributes> AllAttributes =
                    new Dictionary<string, AutomationAttributes>();

        public AutomationAttributes()
        {
            TrackFXTriggers = new List<TrackFXTrigger>();
            SampleTriggers = new List<SampleTrigger>();
            ExtendedMixes = new List<ExtendedMixAttributes>();
            TrackSamples = new List<TrackSample>();
        }

        // ReSharper disable once InconsistentNaming
        public List<TrackFXTrigger> TrackFXTriggers { get; set; }

        public List<SampleTrigger> SampleTriggers { get; set; }

        public List<ExtendedMixAttributes> ExtendedMixes { get; set; }

        public List<TrackSample> TrackSamples { get; set; }

        public string Track { get; set; }

        public ExtendedMixAttributes GetExtendedMixAttributes(string trackDescription)
        {
            if (trackDescription == "") return null;
            return ExtendedMixes
                .FirstOrDefault(em => em.TrackDescription == trackDescription);
        }

        public void RemoveExtendedMixAttributes(string trackDescription)
        {
            if (trackDescription == "") return;
            var extendedMix = GetExtendedMixAttributes(trackDescription);
            if (extendedMix != null) ExtendedMixes.Remove(extendedMix);
        }

        public static AutomationAttributes GetAutomationAttributes(Track track, string folder)
        {
            if (track == null || folder == "") return null;
            return GetAutomationAttributes(track.Description, folder);
        }

        public static AutomationAttributes GetAutomationAttributes(string trackDescription, string folder)
        {
            if (trackDescription == "" || folder == "") return null;

            AutomationAttributes attributes;
            if (CachedAttributes.ContainsKey(trackDescription))
            {
                attributes = CachedAttributes[trackDescription];
            }
            else
            {
                var filepath = GetAttributesFilePath(trackDescription, folder);
                attributes = File.Exists(filepath)
                    ? SerializationHelper<AutomationAttributes>.FromXmlFile(filepath)
                    : new AutomationAttributes();

                attributes.Track = trackDescription;

                try
                {
                    CachedAttributes.Add(trackDescription, attributes);
                }
                catch
                {
                    if (CachedAttributes.ContainsKey(trackDescription)) attributes = CachedAttributes[trackDescription];
                }
            }
            return attributes;
        }


        public static void SaveAutomationAttributes(Track track, string folder)
        {
            var filepath = GetAttributesFilePath(track.Description, folder);
            var attributes = GetAutomationAttributes(track, folder);

            if (attributes.TrackFXTriggers.Count > 0
                || attributes.SampleTriggers.Count > 0
                || attributes.ExtendedMixes.Count > 0
                || attributes.TrackSamples.Count > 0)
            {
                SerializationHelper<AutomationAttributes>.ToXmlFile(attributes, filepath);
            }
            else
            {
                if (File.Exists(filepath)) File.Delete(filepath);
            }
        }

        private static string GetAttributesFilePath(string trackDescription, string folder)
        {
            return Path.Combine(folder,
                FileSystemHelper.StripInvalidFileNameChars(trackDescription) + ".AutomationAttributes.xml");
        }

        public static void ClearCache()
        {
            CachedAttributes.Clear();
        }
    }
}