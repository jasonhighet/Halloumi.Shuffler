namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     The kinds of position callback BassPlayer sets on a track. The value is passed to BASS as
    ///     the sync's user data and read back in BassPlayer.OnTrackSync.
    ///     To add one: add a value here, set it with track.Syncs.Set(...) where it belongs, and handle
    ///     it in OnTrackSync. If a track can have several at once, add it to TrackSyncs.AllowsMany.
    /// </summary>
    internal enum TrackSyncType
    {
        /// <summary>The track reached the end of the file.</summary>
        TrackEnd = 0,

        /// <summary>The fade-in starts.</summary>
        StartFadeIn = 1,

        /// <summary>The fade-in ends (loops back if the fade-in is looped).</summary>
        EndFadeIn = 2,

        /// <summary>The fade-out starts: the next track takes over.</summary>
        StartFadeOut = 3,

        /// <summary>The fade-out ends (loops back if the fade-out is looped).</summary>
        EndFadeOut = 4,

        /// <summary>Set on the current track: the next track's pre-fade-in starts.</summary>
        StartPreFadeIn = 5,

        /// <summary>The end of the raw-loop section.</summary>
        EndRawLoop = 6,

        /// <summary>A track FX trigger starts.</summary>
        StartTrackFxTrigger = 7,

        /// <summary>A track FX trigger ends.</summary>
        EndTrackFxTrigger = 8,

        /// <summary>A sample trigger starts.</summary>
        StartSampleTrigger = 9,

        /// <summary>A sample trigger ends.</summary>
        EndSampleTrigger = 10,

        /// <summary>Set on the previous track: the recorded extended mix ends.</summary>
        EndExtendedMix = 11,

        /// <summary>The skip section starts.</summary>
        StartSkipSection = 12
    }
}
