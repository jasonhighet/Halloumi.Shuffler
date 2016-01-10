using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Halloumi.BassEngine;

namespace Halloumi.Shuffler.Engine
{
    public class Sample
    {
        public Sample()
        {
            this.Start = 0D;
            this.Length = 0D;
            this.Offset = 0D;
            this.LoopMode = LoopMode.FullLoop;
            this.IsAtonal = false;
            this.IsPrimaryLoop = false;
            this.TrackLength = 0M;
            this.BPM = 0M;
            this.Tags = new List<string>();
            this.Gain = 0F;
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

        public decimal BPM { get; set; }

        public List<string> Tags { get; set; }

        public float Gain { get; set; }

        public Sample Clone()
        {
            return new Sample()
            {
                Start = this.Start,
                Length = this.Length,
                Offset = this.Offset,
                LoopMode = this.LoopMode,
                IsAtonal = this.IsAtonal,
                IsPrimaryLoop = this.IsPrimaryLoop,
                Key = this.Key,
                TrackArtist = this.TrackArtist,
                TrackTitle = this.TrackTitle,
                BPM = this.BPM,
                Description = this.Description,
                TrackLength = this.TrackLength,
                Tags = this.Tags,
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