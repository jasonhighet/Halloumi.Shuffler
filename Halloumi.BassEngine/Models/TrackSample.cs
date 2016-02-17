using System.Diagnostics.CodeAnalysis;
using Halloumi.Shuffler.AudioEngine.Helpers;

namespace Halloumi.Shuffler.AudioEngine.Models
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class TrackSample
    {
        public double Start { get; set; }

        public double Length { get; set; }

        public bool IsLooped { get; set; }

        public string Key { get; set; }

        public string Description { get; set; }

        /// <summary>
        ///     Gets the BPM of the sample
        /// </summary>
        public decimal CalculateBpm(Track track)
        {
            if (Key == "PreFadeIn")
            {
                return track.StartBpm;
            }
            if (Length != 0 && IsLooped)
            {
                return BpmHelper.GetBpmFromLoopLength(Length);
            }

            return track.Bpm;
        }

        public TrackSample Clone()
        {
            return new TrackSample
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