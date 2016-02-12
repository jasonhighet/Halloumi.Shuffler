using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Common.Helpers;
using System.Xml.Serialization;

namespace Halloumi.Shuffler.AudioEngine.Models
{
    public class AutomationAttributes
    {
        public List<TrackFXTrigger> TrackFXTriggers { get; set; }

        public List<SampleTrigger> SampleTriggers { get; set; }
        public List<ExtendedMixAttributes> ExtendedMixes { get; set; }
        public List<TrackSample> TrackSamples { get; set; }

        public AutomationAttributes()
        {
            TrackFXTriggers = new List<TrackFXTrigger>();
            SampleTriggers = new List<SampleTrigger>();
            ExtendedMixes = new List<ExtendedMixAttributes>();
            TrackSamples = new List<TrackSample>();
        }

        public ExtendedMixAttributes GetExtendedMixAttributes(Track track)
        {
            if (track == null) return null;
            return GetExtendedMixAttributes(track.Description);
        }

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
            if (_cachedAttributes.ContainsKey(trackDescription))
            {
                attributes = _cachedAttributes[trackDescription];
            }
            else
            {
                var filepath = GetAttributesFilePath(trackDescription, folder);
                if (File.Exists(filepath))
                {
                    attributes = SerializationHelper<AutomationAttributes>.FromXmlFile(filepath);
                }
                else
                {
                    attributes = new AutomationAttributes();
                }
                try
                {
                    _cachedAttributes.Add(trackDescription, attributes);
                }
                catch
                {
                    if (_cachedAttributes.ContainsKey(trackDescription)) attributes = _cachedAttributes[trackDescription];
                }
            }
            return attributes;
        }

        public static AutomationAttributes ReloadAutomationAttributes(Track track, string folder)
        {
            if (track == null || folder == "") return null;

            if (_cachedAttributes.ContainsKey(track.Description))
            {
                _cachedAttributes.Remove(track.Description);
            }

            return GetAutomationAttributes(track, folder);
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

        private static Dictionary<string, AutomationAttributes> _cachedAttributes = new Dictionary<string, AutomationAttributes>();

        private static string GetAttributesFilePath(string trackDescription, string folder)
        {
            return Path.Combine(folder, FileSystemHelper.StripInvalidFileNameChars(trackDescription) + ".AutomationAttributes.xml");
        }

        public static void ClearCache()
        {
            _cachedAttributes.Clear();
        }

        public TrackSample GetTrackSampleByKey(string key)
        {
            return TrackSamples.Where(ts => ts.Key == key).FirstOrDefault();
        }
    }
}