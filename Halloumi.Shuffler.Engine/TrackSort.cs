namespace Halloumi.Shuffler.AudioLibrary
{
    /// <summary>
    ///     Identifies which field a track list should be sorted by.
    /// </summary>
    public enum TrackSortField
    {
        /// <summary>Use the default library sort: AlbumArtist → Album → TrackNumber → Artist → Title.</summary>
        Default,
        Description,
        Album,
        Length,
        Genre,
        StartBpm,
        EndBpm,
        Bitrate,
        Key,
        /// <summary>Requires MixLibrary — handled in ShufflerApplication, not Library.</summary>
        MixInCount,
        /// <summary>Requires MixLibrary — handled in ShufflerApplication, not Library.</summary>
        MixOutCount,
        Rank
    }

    /// <summary>
    ///     Captures the column and direction for sorting a track list.
    ///     This is a plain data object — it contains no logic.
    /// </summary>
    public class TrackSort
    {
        public TrackSortField Field { get; set; } = TrackSortField.Default;

        public bool Descending { get; set; } = false;
    }
}
