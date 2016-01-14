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
            if (this.Key == "PreFadeIn")
            {
                return track.StartBpm;
            }
            else if (this.Length != 0 && this.IsLooped)
            {
                return BassHelper.GetBpmFromLoopLength(this.Length);
            }

            return track.Bpm;
        }

        public TrackSample Clone()
        {
            return new TrackSample()
            {
                Start = this.Start,
                Length = this.Length,
                IsLooped = this.IsLooped,
                Key = this.Key,
                Description = this.Description
            };
        }
    }
}