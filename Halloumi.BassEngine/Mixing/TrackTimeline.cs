using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.Mixing
{
    /// <summary>
    ///     Converts between a track's channel position (where BASS is in the file) and its adjusted
    ///     position (where the listener is in the mix). The two differ because the fade-in can loop
    ///     and the skip section is jumped over.
    ///     Pure arithmetic on the track's fade points - no BASS calls.
    /// </summary>
    public static class TrackTimeline
    {
        /// <summary>
        ///     Gets the adjusted position for a channel position. The adjusted position is counted
        ///     from the start of the fade-in, includes start loops already played and leaves out the
        ///     skip section.
        /// </summary>
        public static long ToAdjustedPosition(Track track, long channelPosition)
        {
            var position = channelPosition - track.FadeInStart;

            if (track.IsLoopedAtStart)
                if (track.CurrentStartLoop < track.StartLoopCount)
                    position += track.CurrentStartLoop * track.FadeInLength;
                else
                    position += track.AdditionalStartLoopLength;

            if (track.HasSkipSection && channelPosition > track.SkipStart)
                position -= track.SkipLength;

            return position;
        }

        /// <summary>
        ///     Converts an adjusted position back to a channel position, and works out which start
        ///     loop it falls in.
        /// </summary>
        /// <returns>
        ///     False if the position is outside the playable range (before the fade-in or past the
        ///     start of the fade-out); the out values are then not to be used.
        /// </returns>
        public static bool TryToChannelPosition(Track track, long adjustedPosition, out long channelPosition,
            out int startLoop)
        {
            var position = adjustedPosition;

            startLoop = track.CurrentStartLoop;
            if (track.IsLoopedAtStart)
                if (position >= track.FullStartLoopLength)
                {
                    startLoop = track.StartLoopCount;
                    position = position - track.AdditionalStartLoopLength;
                }
                else
                {
                    startLoop = (int) (position / track.FadeInLength);
                    position = position - startLoop * track.FadeInLength;
                    if (startLoop > 0 && position == 0)
                        position = 1; // avoid the start-fade event firing when looping
                }

            position = position + track.FadeInStart;

            if (track.HasSkipSection && position > track.SkipStart)
                position = position + track.SkipLength;

            channelPosition = position;
            return position >= track.FadeInStart && position <= track.FadeOutStart;
        }
    }
}
