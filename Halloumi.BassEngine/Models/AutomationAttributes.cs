using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Common.Helpers;

namespace Halloumi.Shuffler.AudioEngine.Models
{
    public class AutomationAttributes
    {
        public AutomationAttributes()
        {
            TrackFXTriggers = new List<TrackFXTrigger>();
            SampleTriggers = new List<SampleTrigger>();
            ExtendedMixes = new List<ExtendedMixAttributes>();
            TrackSamples = new List<TrackSample>();
        }

        public string Track { get; set; }

        // ReSharper disable once InconsistentNaming
        public List<TrackFXTrigger> TrackFXTriggers { get; set; }

        public List<SampleTrigger> SampleTriggers { get; set; }

        public List<ExtendedMixAttributes> ExtendedMixes { get; set; }

        public List<TrackSample> TrackSamples { get; set; }

        public AutomationAttributes Clone()
        {
            return new AutomationAttributes()
            {
                Track = Track,
                ExtendedMixes = ExtendedMixes.Select(x => x.Clone()).ToList(),
                SampleTriggers = SampleTriggers.Select(x => x.Clone()).ToList(),
                TrackFXTriggers = TrackFXTriggers.Select(x=>x.Clone()).ToList(),
                TrackSamples = TrackSamples.Select(x => x.Clone()).ToList(),
            };
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
    }
}