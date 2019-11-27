namespace Halloumi.Shuffler.AudioLibrary.Samples
{

    public class SearchCriteria
    {
        public string SearchText { get; set; }

        public string Key { get; set; }

        public decimal MinBpm { get; set; }

        public decimal MaxBpm { get; set; }

        public bool AtonalOnly { get; set; }

        public bool IncludeAtonal { get; set; }

        public bool? Primary { get; set; }

        public LoopMode? LoopMode { get; set; }

        public string TrackTitle { get; set; }

        public string TrackArtist { get; set; }

        public string Description { get; set; }
    }
}
