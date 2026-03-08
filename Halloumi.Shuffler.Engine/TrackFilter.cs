using System.Collections.Generic;

namespace Halloumi.Shuffler.AudioLibrary
{
    /// <summary>
    ///     Captures all parameters used to filter a track list query.
    ///     This is a plain data object — it contains no logic.
    /// </summary>
    public class TrackFilter
    {
        public List<string> Genres { get; set; } = new List<string>();

        public List<string> Artists { get; set; } = new List<string>();

        public List<string> Albums { get; set; } = new List<string>();

        public string SearchText { get; set; } = "";

        public string Collection { get; set; } = "";

        public string ExcludeCollection { get; set; } = "";

        public Library.ShufflerFilter ShufflerFilter { get; set; } = Library.ShufflerFilter.None;

        public int MinBpm { get; set; } = 0;

        public int MaxBpm { get; set; } = 1000;

        public Library.TrackRankFilter TrackRankFilter { get; set; } = Library.TrackRankFilter.None;

        /// <summary>
        ///     Whether to restrict results to tracks that are (or are not) in the current playlist.
        /// </summary>
        public QueuedFilter Queued { get; set; } = QueuedFilter.Any;

        public enum QueuedFilter
        {
            /// <summary>No restriction — return all matching tracks.</summary>
            Any,

            /// <summary>Only return tracks that are present in the current playlist.</summary>
            Queued,

            /// <summary>Only return tracks that are NOT present in the current playlist.</summary>
            NotQueued
        }

        /// <summary>Returns a TrackFilter with all fields set to their defaults (no filtering).</summary>
        public static TrackFilter Default() => new TrackFilter();
    }
}
