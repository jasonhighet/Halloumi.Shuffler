using System;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.Mixing
{
    /// <summary>
    ///     Builds the Mixer view's waveform overview: each track as heard (fade-in loops repeated, skip section cut
    ///     out, fade-out loops repeated, then the rest of the file), lined up on the transitions. Reads tracks only -
    ///     safe to call without the slot lock, like Playback.GetTrackPosition. See docs/mixer-waveform.md.
    /// </summary>
    /// <summary>
    ///     What is saved for the mix from the current track into the next one.
    /// </summary>
    internal class SavedFadeMoves
    {
        public bool IsRecordedMix { get; set; }

        /// <summary>
        ///     Fades are only moved in manual mix mode, where an extended mix is not played back.
        /// </summary>
        public bool IsManualMixMode { get; set; }

        public bool CanMoveFades => IsManualMixMode;

        public int FadeOutShift { get; set; }

        public int FadeInShift { get; set; }
    }

    internal static class MixOverviewBuilder
    {
        /// <summary>
        ///     Builds the overview. <paramref name="previous" /> is only passed while it is still fading out; any of
        ///     the tracks may be null.
        /// </summary>
        public static MixOverview Build(Track previous, Track current, Track next, SavedFadeMoves saved)
        {
            var overview = new MixOverview();
            if (current == null) return overview;

            overview.Current = BuildTrack(current, false, false, true);

            if (previous != null)
            {
                // its fade-out started when the current track's fade-in did
                overview.Previous = BuildTrack(previous, true, false, true);
                overview.Previous.Offset = -overview.Previous.FadeOutHeardStart;
            }

            if (next != null)
            {
                // the next track's position only means something once it has started (its pre-fade-in)
                // its intro is shown too, so its fade-in can be moved into it
                overview.Next = BuildTrack(next, false, true, AudioStreamHelper.IsPlayingInMixer(next));
                overview.Next.Offset = overview.Current.FadeOutHeardStart;
                AddFadeMoves(overview, current, next, saved ?? new SavedFadeMoves());
            }

            return overview;
        }

        private static void AddFadeMoves(MixOverview overview, Track current, Track next, SavedFadeMoves saved)
        {
            overview.IsRecordedMix = saved.IsRecordedMix;
            overview.IsManualMixMode = saved.IsManualMixMode;
            var position = AudioStreamHelper.GetPosition(current);

            var fadeOutShift = FadeShift.GetFadeOutShift(current);
            FadeShift.GetRange(n => saved.CanMoveFades && FadeShift.CanPlaceFadeOut(current, next, n, position),
                fadeOutShift, out var fadeOutMin, out var fadeOutMax);
            overview.FadeOutMove = new MixOverviewFadeMove
            {
                Shift = fadeOutShift,
                SavedShift = saved.FadeOutShift,
                Min = fadeOutMin,
                Max = fadeOutMax,
                SectionLength = current.FadeOutLengthSeconds,
                FileStart = Seconds(current, current.FadeOutStart)
            };

            var fadeInShift = FadeShift.GetFadeInShift(next);
            var canMoveFadeIn = saved.CanMoveFades && !FadeShift.HasFadeInStarted(next);
            FadeShift.GetRange(n => canMoveFadeIn && FadeShift.CanPlaceFadeIn(next, n),
                fadeInShift, out var fadeInMin, out var fadeInMax);
            overview.FadeInMove = new MixOverviewFadeMove
            {
                Shift = fadeInShift,
                SavedShift = saved.FadeInShift,
                Min = fadeInMin,
                Max = fadeInMax,
                SectionLength = next.FadeInLengthSeconds,
                FileStart = Seconds(next, next.FadeInStart)
            };
        }

        /// <summary>
        ///     True while a track (the previous one) is still playing out after its fade-out started: its position is
        ///     past the fade-out start and it is playing in the mixer. Manual and recorded mixes can keep it playing
        ///     past the end of the fade-out. When the whole player is paused, a track part-way through its fade-out
        ///     section still counts.
        /// </summary>
        public static bool IsFadingOut(Track track, bool isPlayerPlaying)
        {
            if (track == null || !track.IsAudioLoaded()) return false;

            var position = AudioStreamHelper.GetPosition(track);
            if (position < track.FadeOutStart) return false;
            if (AudioStreamHelper.IsPlayingInMixer(track)) return true;

            return !isPlayerPlaying && position < track.FadeOutEnd;
        }

        private static MixOverviewTrack BuildTrack(Track track, bool endOnly, bool includeIntro, bool includePosition)
        {
            var result = new MixOverviewTrack
            {
                Filename = track.Filename,
                Description = track.Description
            };

            // heard time 0 is the start of the fade-in, so the pre-fade-in and intro run at negative times
            var usePreFadeIn = track.UsePreFadeIn && track.PreFadeInStart >= 0 && track.PreFadeInStart < track.FadeInStart;
            var audibleStart = usePreFadeIn ? track.PreFadeInStart : track.FadeInStart;

            if (includeIntro)
                Add(result, Segment(track, MixOverviewSegmentKind.Intro, 0, audibleStart, 0));

            if (usePreFadeIn)
                Add(result, Segment(track, MixOverviewSegmentKind.PreFadeIn, track.PreFadeInStart, track.FadeInStart, 0));

            result.AudibleStart = -Seconds(track, track.FadeInStart - audibleStart);
            result.HeardStart = -result.Segments.Sum(x => x.Length);

            var heard = 0D;

            var startLoops = Math.Max(1, track.StartLoopCount);
            for (var i = 0; i < startLoops; i++)
                heard += Add(result, Segment(track, MixOverviewSegmentKind.FadeIn, track.FadeInStart, track.FadeInEnd, i));

            // the skip section is only cut out when it sits inside the body, which is the only place it is used
            if (track.HasSkipSection && track.SkipStart >= track.FadeInEnd && track.SkipEnd <= track.FadeOutStart)
            {
                heard += Add(result, Segment(track, MixOverviewSegmentKind.Body, track.FadeInEnd, track.SkipStart, 0));
                heard += Add(result, Segment(track, MixOverviewSegmentKind.Body, track.SkipEnd, track.FadeOutStart, 0));
            }
            else
            {
                heard += Add(result, Segment(track, MixOverviewSegmentKind.Body, track.FadeInEnd, track.FadeOutStart, 0));
            }

            result.FadeOutHeardStart = heard;

            var endLoops = Math.Max(1, track.EndLoopCount);
            for (var i = 0; i < endLoops; i++)
                heard += Add(result, Segment(track, MixOverviewSegmentKind.FadeOut, track.FadeOutStart, track.FadeOutEnd, i));

            // manual and recorded mixes can play on past the fade-out, so show the rest of the file
            heard += Add(result, Segment(track, MixOverviewSegmentKind.Tail, track.FadeOutEnd, track.Length, 0));
            result.HeardEnd = heard;

            if (endOnly)
            {
                result.Segments = result.Segments
                    .Where(x => x.Kind == MixOverviewSegmentKind.FadeOut || x.Kind == MixOverviewSegmentKind.Tail)
                    .ToList();
                result.HeardStart = result.FadeOutHeardStart;
                result.AudibleStart = result.FadeOutHeardStart;
            }

            if (includePosition && track.IsAudioLoaded())
            {
                result.HasPosition = true;
                result.Position = GetHeardPosition(track);
            }

            return result;
        }

        /// <summary>
        ///     The heard position: the adjusted position (start loops counted, skip section left out) plus the end
        ///     loops already played.
        /// </summary>
        private static double GetHeardPosition(Track track)
        {
            var channelPosition = AudioStreamHelper.GetPosition(track);
            var position = TrackTimeline.ToAdjustedPosition(track, channelPosition);

            if (track.IsLoopedAtEnd && channelPosition >= track.FadeOutStart)
            {
                // manual mixes can loop forever: stay in the last repeat shown
                var endLoop = Math.Min(track.CurrentEndLoop, track.EndLoopCount - 1);
                position += endLoop * track.FadeOutLength;
            }

            return Seconds(track, position);
        }

        private static double Add(MixOverviewTrack result, MixOverviewSegment segment)
        {
            if (segment.Length <= 0) return 0;
            result.Segments.Add(segment);
            return segment.Length;
        }

        private static MixOverviewSegment Segment(Track track, MixOverviewSegmentKind kind, long start, long end,
            int repeat)
        {
            return new MixOverviewSegment
            {
                Kind = kind,
                FileStart = Seconds(track, start),
                FileEnd = Seconds(track, end),
                Repeat = repeat
            };
        }

        /// <summary>
        ///     Converts samples to seconds, keeping the sign (SamplesToSeconds returns 0 for negative values).
        /// </summary>
        private static double Seconds(Track track, long samples)
        {
            return samples < 0 ? -track.SamplesToSeconds(-samples) : track.SamplesToSeconds(samples);
        }
    }
}
