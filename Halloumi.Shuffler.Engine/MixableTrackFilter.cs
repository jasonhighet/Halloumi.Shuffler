using System.Collections.Generic;

namespace Halloumi.Shuffler.AudioLibrary
{
    /// <summary>
    ///     Captures all parameters used to filter and retrieve a list of mixable tracks.
    ///     This is a plain data object — it contains no logic.
    /// </summary>
    public class MixableTrackFilter
    {
        /// <summary>
        ///     Allowed mix-rank levels, e.g. {5, 4, 3} for "Good+".
        ///     Defaults to all ranks.
        /// </summary>
        public List<int> MixRankLevels { get; set; } = new List<int> { 0, 1, 2, 3, 4, 5 };

        /// <summary>
        ///     Minimum key-compatibility rank.
        ///     -1 = Any (no key filter)
        ///      0 = "Not Good" only (key rank &lt;= 1)
        ///      2, 3, 4 = ascending quality thresholds
        /// </summary>
        public int MinKeyRank { get; set; } = -1;

        /// <summary>
        ///     Whether to return tracks that can mix FROM the parent track,
        ///     or tracks that the parent can mix TO.
        /// </summary>
        public TrackDirection Direction { get; set; } = TrackDirection.From;

        /// <summary>
        ///     When true, tracks already present in the current playlist are excluded.
        /// </summary>
        public bool ExcludeQueued { get; set; } = false;

        public enum TrackDirection
        {
            /// <summary>Tracks that can mix FROM the parent (predecessor tracks).</summary>
            From,

            /// <summary>Tracks that the parent can mix TO (successor tracks).</summary>
            To
        }
    }
}
