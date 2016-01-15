using System.Collections.Generic;

namespace Halloumi.Shuffler.Engine
{
    public class Sample
    {
        public Sample()
        {
            Start = 0D;
            Length = 0D;
            Offset = 0D;
            LoopMode = LoopMode.FullLoop;
            IsAtonal = false;
            IsPrimaryLoop = false;
            TrackLength = 0M;
            Bpm = 0M;
            Tags = new List<string>();
            Gain = 0F;
        }

        public double Start { get; set; }

        public double Length { get; set; }

        public double Offset { get; set; }

        public LoopMode LoopMode { get; set; }

        public bool IsAtonal { get; set; }

        public bool IsPrimaryLoop { get; set; }

        public string Key { get; set; }

        public string TrackArtist { get; set; }

        public string TrackTitle { get; set; }

        public decimal TrackLength { get; set; }

        public string Description { get; set; }

        public decimal Bpm { get; set; }

        public List<string> Tags { get; set; }

        public float Gain { get; set; }

        public Sample Clone()
        {
            return new Sample()
            {
                Start = Start,
                Length = Length,
                Offset = Offset,
                LoopMode = LoopMode,
                IsAtonal = IsAtonal,
                IsPrimaryLoop = IsPrimaryLoop,
                Key = Key,
                TrackArtist = TrackArtist,
                TrackTitle = TrackTitle,
                Bpm = Bpm,
                Description = Description,
                TrackLength = TrackLength,
                Tags = Tags,
                Gain = 0F
            };
        }
    }

    public enum LoopMode
    {
        FullLoop,
        PartialLoopAnchorStart,
        PartialLoopAnchorEnd
    }
}