using System;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.Mixing
{
    /// <summary>
    ///     Where a fade section may be moved to, in whole sections. The current track's fade-out and the next track's
    ///     fade-in can each be moved by their own length, so a move always lands on a phrase boundary and keeps the
    ///     section's length. See docs/mixer-waveform.md.
    /// </summary>
    internal static class FadeShift
    {
        /// <summary>
        ///     The furthest a section can be moved either way, in sections.
        /// </summary>
        public const int MaxSections = 64;

        /// <summary>
        ///     A fade-out can't be moved to start sooner than this after the playhead, so its sync is not missed.
        /// </summary>
        private const double MinLeadSeconds = 1;

        /// <summary>
        ///     How many fade-out sections the track's fade-out has been moved live.
        /// </summary>
        public static int GetFadeOutShift(Track track)
        {
            return track.LiveFadeOutSections;
        }

        /// <summary>
        ///     How many fade-in sections the track's fade-in has been moved live.
        /// </summary>
        public static int GetFadeInShift(Track track)
        {
            return track.LiveFadeInSections;
        }

        /// <summary>
        ///     True if the current track's fade-out could sit the given number of sections from its saved place:
        ///     after the fade-in, inside the file, on the same side of the skip section, and far enough ahead of the
        ///     playhead (with room for the next track's pre-fade-in) that it has not started.
        /// </summary>
        public static bool CanPlaceFadeOut(Track current, Track next, int sections, long position)
        {
            var length = current.FadeOutLength;
            if (length <= 0 || Math.Abs(sections) > MaxSections) return false;

            var savedStart = current.FadeOutStart - current.LiveFadeOutShift;
            var start = current.FadeOutStart + (sections - current.LiveFadeOutSections) * length;
            var end = start + length;

            if (start <= current.FadeInEnd) return false;
            if (current.Length > 0 && end > current.Length) return false;
            if (!IsSameSideOfSkip(current, savedStart, savedStart + length, start, end)) return false;

            var lead = current.SecondsToSamples(MinLeadSeconds);
            if (next != null && next.UsePreFadeIn) lead += current.SecondsToSamples(next.PreFadeInLengthSeconds);
            return start - lead > position;
        }

        /// <summary>
        ///     True if the next track's fade-in could sit the given number of sections from its saved place: inside
        ///     the file (pre-fade-in included), before its fade-out, and on the same side of the skip section. Check
        ///     <see cref="HasFadeInStarted" /> as well.
        /// </summary>
        public static bool CanPlaceFadeIn(Track next, int sections)
        {
            var length = next.FadeInLength;
            if (length <= 0 || Math.Abs(sections) > MaxSections) return false;

            var delta = (sections - next.LiveFadeInSections) * length;
            var savedStart = next.FadeInStart - next.LiveFadeInShift;
            var start = next.FadeInStart + delta;
            var end = start + length;

            if (start < 0) return false;
            if (next.UsePreFadeIn && next.PreFadeInStart + delta < 0) return false;
            if (end >= next.FadeOutStart) return false;
            return IsSameSideOfSkip(next, savedStart, savedStart + length, start, end);
        }

        /// <summary>
        ///     True once the next track's pre-fade-in or fade-in is playing: its fade-in can no longer move.
        /// </summary>
        public static bool HasFadeInStarted(Track next)
        {
            return AudioStreamHelper.IsPlayingInMixer(next);
        }

        /// <summary>
        ///     The range of section counts a fade can be moved to, walking out from where it is now. When it can't be
        ///     moved at all (it has started), both are the current count.
        /// </summary>
        public static void GetRange(Func<int, bool> canPlace, int now, out int min, out int max)
        {
            min = now;
            max = now;
            if (!canPlace(now)) return;

            while (min - 1 >= -MaxSections && canPlace(min - 1)) min--;
            while (max + 1 <= MaxSections && canPlace(max + 1)) max++;
        }

        private static bool IsSameSideOfSkip(Track track, long savedStart, long savedEnd, long start, long end)
        {
            if (!track.HasSkipSection) return true;
            if (end > track.SkipStart && start < track.SkipEnd) return false;
            return savedEnd <= track.SkipStart == end <= track.SkipStart;
        }

        /// <summary>
        ///     Moves the current track's fade-out to a number of sections from its saved place.
        /// </summary>
        public static void MoveFadeOut(Track track, int sections)
        {
            var steps = sections - track.LiveFadeOutSections;
            track.ShiftFadeOut(steps * track.FadeOutLength, steps);
        }

        /// <summary>
        ///     Moves the next track's fade-in to a number of sections from its saved place.
        /// </summary>
        public static void MoveFadeIn(Track track, int sections)
        {
            var steps = sections - track.LiveFadeInSections;
            track.ShiftFadeIn(steps * track.FadeInLength, steps);
        }
    }
}
