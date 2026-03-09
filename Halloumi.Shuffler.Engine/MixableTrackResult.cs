namespace Halloumi.Shuffler.AudioLibrary
{
    /// <summary>
    ///     Represents a single result from a mixable-tracks query, containing all fields
    ///     needed to display or evaluate a candidate track for mixing.
    ///     This is a plain public model — it has no dependency on WinForms or the audio engine.
    /// </summary>
    public class MixableTrackResult
    {
        /// <summary>The underlying library track.</summary>
        public Models.Track Track { get; set; }

        /// <summary>Artist — Title description.</summary>
        public string Description { get; set; }

        /// <summary>Average BPM of the track.</summary>
        public decimal Bpm { get; set; }

        /// <summary>Absolute BPM percentage difference between parent end BPM and this track's start BPM.</summary>
        public decimal Diff { get; set; }

        /// <summary>Extended mix rank (0–5 scale, may be fractional).</summary>
        public double MixRank { get; set; }

        /// <summary>Human-readable mix rank description, e.g. "Good", "Poor", "Good*" for extended mixes.</summary>
        public string MixRankDescription { get; set; }

        /// <summary>Track rank (0 = Forbidden, 5 = Excellent).</summary>
        public int Rank { get; set; }

        /// <summary>Human-readable track rank description.</summary>
        public string RankDescription { get; set; }

        /// <summary>Musical key, formatted for display.</summary>
        public string Key { get; set; }

        /// <summary>Key distance from the parent track's key (lower = more compatible).</summary>
        public int KeyDiff { get; set; }

        /// <summary>Human-readable key compatibility description.</summary>
        public string KeyRankDescription { get; set; }
    }
}
