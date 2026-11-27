using System.Collections.Generic;

namespace Halloumi.Shuffler.AudioEngine.Models
{
    /// <summary>
    ///     What the Mixer view's waveform strip shows, on one time axis in seconds: the current track, the end of the
    ///     previous track while it is still fading out (lined up under the current track's fade-in), and the next
    ///     track (lined up under the current track's fade-out). Each is shown as heard: loops repeated, skip section
    ///     cut out. See docs/mixer-waveform.md.
    /// </summary>
    public class MixOverview
    {
        /// <summary>
        ///     The previous track from its fade-out on, while it is still playing out. Null otherwise.
        /// </summary>
        public MixOverviewTrack Previous { get; set; }

        /// <summary>
        ///     The current track (or the previous one if it ran out with nothing queued). Null when nothing is loaded.
        /// </summary>
        public MixOverviewTrack Current { get; set; }

        /// <summary>
        ///     The next track. Null when none is queued.
        /// </summary>
        public MixOverviewTrack Next { get; set; }

        /// <summary>
        ///     Moving the current track's fade-out for the coming mix. Null when no next track is queued.
        /// </summary>
        public MixOverviewFadeMove FadeOutMove { get; set; }

        /// <summary>
        ///     Moving the next track's fade-in for the coming mix. Null when no next track is queued.
        /// </summary>
        public MixOverviewFadeMove FadeInMove { get; set; }

        /// <summary>
        ///     True if the coming mix has an extended mix (a saved manual mix). Its fade moves are where the fades start.
        /// </summary>
        public bool IsRecordedMix { get; set; }

        /// <summary>
        ///     Fades can only be moved in manual mix mode, where an extended mix is not played back.
        /// </summary>
        public bool IsManualMixMode { get; set; }

        /// <summary>
        ///     True if a fade has been moved from where the extended mix has it (or from the track's own fade).
        /// </summary>
        public bool HasUnsavedMoves =>
            FadeOutMove != null && FadeOutMove.Shift != FadeOutMove.SavedShift
            || FadeInMove != null && FadeInMove.Shift != FadeInMove.SavedShift;

        /// <summary>
        ///     True if the extended mix has fade moves.
        /// </summary>
        public bool HasSavedMoves =>
            FadeOutMove != null && FadeOutMove.SavedShift != 0 || FadeInMove != null && FadeInMove.SavedShift != 0;
    }

    /// <summary>
    ///     A fade section that can be moved in whole sections: where it is, where it is saved, and how far it can go.
    /// </summary>
    public class MixOverviewFadeMove
    {
        /// <summary>
        ///     Sections moved from the track's own fade, now (negative is earlier).
        /// </summary>
        public int Shift { get; set; }

        /// <summary>
        ///     Sections saved with the extended mix, or 0.
        /// </summary>
        public int SavedShift { get; set; }

        /// <summary>
        ///     The earliest the section can be moved to, in sections. Equal to <see cref="Max" /> when it can't move.
        /// </summary>
        public int Min { get; set; }

        public int Max { get; set; }

        /// <summary>
        ///     The section's length in seconds: one step of a move.
        /// </summary>
        public double SectionLength { get; set; }

        /// <summary>
        ///     Where the section starts in the file now, in seconds.
        /// </summary>
        public double FileStart { get; set; }

        public bool CanMove => Min < Max;
    }

    public class MixOverviewTrack
    {
        public string Filename { get; set; }

        public string Description { get; set; }

        /// <summary>
        ///     Where this track's heard time 0 (the start of its fade-in) sits on the shared axis: 0 for the current
        ///     track, its fade-out start for the next track, and minus its own fade-out start for the previous track.
        /// </summary>
        public double Offset { get; set; }

        /// <summary>
        ///     The parts of the file that are played, in order. Each segment's heard start is the sum of the lengths
        ///     before it, starting from <see cref="HeardStart" />.
        /// </summary>
        public List<MixOverviewSegment> Segments { get; set; } = new List<MixOverviewSegment>();

        /// <summary>
        ///     The heard time of the first segment: negative when it starts with a pre-fade-in, the fade-out start
        ///     when only the end of the track is shown, otherwise 0.
        /// </summary>
        public double HeardStart { get; set; }

        /// <summary>
        ///     The heard time at which the fade-out section starts.
        /// </summary>
        public double FadeOutHeardStart { get; set; }

        /// <summary>
        ///     The heard time at which the track is first heard: its pre-fade-in, or its fade-in. Earlier segments
        ///     (an intro) are shown but not played.
        /// </summary>
        public double AudibleStart { get; set; }

        /// <summary>
        ///     The heard length up to the end of the last segment, from heard time 0.
        /// </summary>
        public double HeardEnd { get; set; }

        /// <summary>
        ///     True when the track is playing (or paused part-way) and <see cref="Position" /> is meaningful.
        /// </summary>
        public bool HasPosition { get; set; }

        /// <summary>
        ///     The heard position, on the same scale as the segments.
        /// </summary>
        public double Position { get; set; }

        /// <summary>
        ///     The heard time at which a file position (in seconds) is shown: in the last segment that holds it, so a
        ///     looped section maps to its last repeat. Null if no segment holds it.
        /// </summary>
        public double? HeardTimeOf(double fileSeconds)
        {
            double? result = null;
            var heard = HeardStart;
            foreach (var segment in Segments)
            {
                if (fileSeconds >= segment.FileStart && fileSeconds <= segment.FileEnd)
                    result = heard + fileSeconds - segment.FileStart;
                heard += segment.Length;
            }

            return result;
        }
    }

    public enum MixOverviewSegmentKind
    {
        /// <summary>
        ///     Before the pre-fade-in or fade-in: not heard. Shown for the next track, so its fade-in can be moved into it.
        /// </summary>
        Intro,

        PreFadeIn,
        FadeIn,
        Body,
        FadeOut,

        /// <summary>
        ///     After the fade-out: normally not heard, but manual and recorded mixes can play on into it.
        /// </summary>
        Tail
    }

    public class MixOverviewSegment
    {
        public MixOverviewSegmentKind Kind { get; set; }

        /// <summary>
        ///     Where the segment starts in the audio file, in seconds.
        /// </summary>
        public double FileStart { get; set; }

        /// <summary>
        ///     Where the segment ends in the audio file, in seconds.
        /// </summary>
        public double FileEnd { get; set; }

        /// <summary>
        ///     0 for the first play of a section, 1 for its first repeat, and so on.
        /// </summary>
        public int Repeat { get; set; }

        public double Length => FileEnd - FileStart;
    }
}
