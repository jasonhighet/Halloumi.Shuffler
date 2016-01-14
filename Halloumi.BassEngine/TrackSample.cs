using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halloumi.BassEngine
{
    public class TrackSample
    {
        public double Start { get; set; }

        public double Length { get; set; }

        public bool IsLooped { get; set; }

        public string Key { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Gets the BPM of the sample
        /// </summary>
        public decimal CalculateBpm(Track track)
        {
            if (Key == "PreFadeIn")
            {
                return track.StartBpm;
            }
            else if (Length != 0 && IsLooped)
            {
                return BassHelper.GetBpmFromLoopLength(Length);
            }

            return track.Bpm;
        }

        public TrackSample Clone()
        {
            return new TrackSample()
            {
                Start = Start,
                Length = Length,
                IsLooped = IsLooped,
                Key = Key,
                Description = Description
            };
        }
    }
}