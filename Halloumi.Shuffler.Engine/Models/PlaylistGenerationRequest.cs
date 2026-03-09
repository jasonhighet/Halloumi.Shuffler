namespace Halloumi.Shuffler.AudioLibrary.Models
{
    public class PlaylistGenerationRequest
    {
        public PlaylistGenerationRequest()
        {
            Strategy = TrackSelector.MixStrategy.BestMix;
            BpmDirection = TrackSelector.BpmDirection.Cycle;
            Direction = TrackSelector.GenerateDirection.Forwards;
            AllowBearable = TrackSelector.AllowBearableMixStrategy.AfterTwoGoodTracks;
            ContinueMix = TrackSelector.ContinueMix.Yes;
            KeyMixStrategy = TrackSelector.KeyMixStrategy.VeryGood;
            UseExtendedMixes = TrackSelector.UseExtendedMixes.Any;
            RestrictArtistClumping = false;
            RestrictGenreClumping = false;
            RestrictTitleClumping = false;
            ApproximateLengthMinutes = int.MaxValue;
            MaxTracksToAdd = 5;
            ExcludeFromPlaylistFile = "";
            ExcludeMixesOnly = false;
            DisplayedTracksOnly = false;
        }

        public static PlaylistGenerationRequest Default() => new PlaylistGenerationRequest();

        public TrackSelector.MixStrategy Strategy { get; set; }
        public TrackSelector.BpmDirection BpmDirection { get; set; }
        public TrackSelector.GenerateDirection Direction { get; set; }
        public TrackSelector.AllowBearableMixStrategy AllowBearable { get; set; }
        public TrackSelector.ContinueMix ContinueMix { get; set; }
        public TrackSelector.KeyMixStrategy KeyMixStrategy { get; set; }
        public TrackSelector.UseExtendedMixes UseExtendedMixes { get; set; }
        public bool RestrictArtistClumping { get; set; }
        public bool RestrictGenreClumping { get; set; }
        public bool RestrictTitleClumping { get; set; }
        public int ApproximateLengthMinutes { get; set; }
        public int MaxTracksToAdd { get; set; }
        public string ExcludeFromPlaylistFile { get; set; }
        public bool ExcludeMixesOnly { get; set; }
        public bool DisplayedTracksOnly { get; set; }
    }
}
