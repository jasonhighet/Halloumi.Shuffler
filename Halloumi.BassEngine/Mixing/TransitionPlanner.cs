using System;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.Mixing
{
    /// <summary>
    ///     Decides how one track hands over to the next: how long the crossfade is, and what the
    ///     outgoing track's volume does. Works only on track metadata - no BASS calls - so every
    ///     decision can be checked without playing audio.
    ///     BassPlayer looks up the extended mix attributes and passes them in; a null value means
    ///     the pair has no recorded mix.
    /// </summary>
    public static class TransitionPlanner
    {
        // the default loop is 16 beats, so these are fractions of a 16-beat loop

        /// <summary>A cut: one beat of the incoming track.</summary>
        public static double GetCutFadeLength(Track track)
        {
            if (track == null) return 0D;
            return BpmHelper.GetDefaultLoopLength(track.StartBpm) / 16D;
        }

        /// <summary>A quick fade: eight beats at the track's end tempo.</summary>
        public static double GetQuickFadeLength(Track track)
        {
            if (track == null) return 0D;
            return BpmHelper.GetDefaultLoopLength(track.EndBpm) / 2D;
        }

        /// <summary>A power-down: four beats at the track's end tempo.</summary>
        public static double GetPowerDownFadeLength(Track track)
        {
            if (track == null) return 0D;
            return BpmHelper.GetDefaultLoopLength(track.EndBpm) / 4D;
        }

        /// <summary>
        ///     An echo-out: eight half-beat echoes. Timed to the incoming track's tempo, because the
        ///     outgoing track is matched to it; if there is no incoming track, timed to the outgoing track.
        /// </summary>
        public static double GetEchoOutFadeLength(Track outgoingTrack, Track incomingTrack)
        {
            if (incomingTrack != null)
                return BpmHelper.GetDefaultLoopLength(incomingTrack.StartBpm) / 32 * 8;
            if (outgoingTrack != null)
                return BpmHelper.GetDefaultLoopLength(outgoingTrack.EndBpm) / 32 * 8;
            return 0D;
        }

        /// <summary>
        ///     Gets how long the outgoing track fades out for: the recorded mix length if there is
        ///     one, otherwise its end loops (or one default loop), stretched to the incoming track's tempo.
        /// </summary>
        /// <param name="fromTrack">The outgoing track.</param>
        /// <param name="toTrack">The incoming track, or null.</param>
        /// <param name="mix">The extended mix attributes for this pair, or null.</param>
        public static double GetExtendedFadeOutLength(Track fromTrack, Track toTrack, ExtendedMixAttributes mix)
        {
            if (fromTrack == null) return 0;

            if (mix != null) return mix.FadeLength;
            var fadeOutLength = fromTrack.FullEndLoopLengthSeconds;

            if (fadeOutLength == 0)
                fadeOutLength = BpmHelper.GetDefaultLoopLength(fromTrack.EndBpm);

            if (toTrack != null)
                fadeOutLength = BpmHelper.GetLengthAdjustedToMatchAnotherTrack(fromTrack, toTrack, fadeOutLength);

            return fadeOutLength;
        }

        /// <summary>
        ///     Gets the crossfade length: the shorter of the incoming track's fade-in and the
        ///     outgoing track's fade-out.
        /// </summary>
        /// <param name="fromTrack">The outgoing track, or null.</param>
        /// <param name="toTrack">The incoming track, or null.</param>
        /// <param name="mix">The extended mix attributes for this pair, or null.</param>
        public static double GetFadeLength(Track fromTrack, Track toTrack, ExtendedMixAttributes mix)
        {
            if (fromTrack == null && toTrack == null) return 0F;

            if (toTrack == null) return fromTrack.SamplesToSeconds(fromTrack.FullEndLoopLength);
            if (fromTrack == null) return BpmHelper.GetDefaultLoopLength(toTrack.StartBpm) / 4D;

            var fadeInLength = toTrack.FullStartLoopLengthSeconds;
            if (fadeInLength == 0)
                fadeInLength = BpmHelper.GetDefaultLoopLength(toTrack.StartBpm);

            var fadeOutLength = GetExtendedFadeOutLength(fromTrack, toTrack, mix);

            if (mix == null)
            {
                if (fromTrack.PowerDownOnEnd)
                {
                    fadeOutLength = GetPowerDownFadeLength(fromTrack);
                    fadeOutLength = BpmHelper.GetLengthAdjustedToMatchAnotherTrack(fromTrack, toTrack, fadeOutLength);
                }

                if (toTrack.StartBpm > fromTrack.EndBpm * 1.1M)
                    fadeOutLength = fadeOutLength / 2;
            }

            var fadeLength = fadeOutLength;
            if (fadeInLength < fadeLength) fadeLength = fadeInLength;
            return fadeLength;
        }

        /// <summary>
        ///     Picks a safe fade type for two tracks that have no hand-set mix: a power-down if the
        ///     keys clash, an echo-out if the tempos are far apart, otherwise a quick fade.
        /// </summary>
        public static ExtendedFadeType GetConservativeFadeType(Track currentTrack, Track nextTrack)
        {
            if (KeyHelper.GetKeyMixRank(currentTrack.Key, nextTrack.Key) < 3)
                return ExtendedFadeType.PowerDown;

            if (!BpmHelper.IsBpmInRange(currentTrack.EndBpm, nextTrack.StartBpm, 10))
                return ExtendedFadeType.EchoOut;

            return ExtendedFadeType.QuickFade;
        }

        /// <summary>
        ///     Reshapes a track's fade-out section for a fade type. Changes only the track's
        ///     metadata; the caller must reset the track's sync positions afterwards.
        /// </summary>
        public static void ShapeFadeOut(Track track, ExtendedFadeType fadeType)
        {
            double newFadeOutLength;
            switch (fadeType)
            {
                case ExtendedFadeType.Default:
                    break;
                case ExtendedFadeType.PowerDown:
                    track.PowerDownOnEnd = true;
                    break;
                case ExtendedFadeType.EchoOut:
                    track.EndLoopCount = 8;
                    track.FadeOutEnd = track.FadeOutStart +
                                       track.SecondsToSamples(BpmHelper.GetDefaultLoopLength(track.EndBpm) / 32);
                    break;
                case ExtendedFadeType.Cut:
                    newFadeOutLength = GetCutFadeLength(track);
                    if (newFadeOutLength < track.FadeOutLengthSeconds)
                        track.FadeOutEnd = track.FadeOutStart + track.SecondsToSamples(newFadeOutLength);
                    track.EndLoopCount = 0;
                    break;
                case ExtendedFadeType.QuickFade:
                    newFadeOutLength = GetQuickFadeLength(track);
                    if (newFadeOutLength < track.FadeOutLengthSeconds)
                        track.FadeOutEnd = track.FadeOutStart + track.SecondsToSamples(newFadeOutLength);
                    track.EndLoopCount = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fadeType), fadeType, null);
            }
        }

        /// <summary>
        ///     Gets the fade-out length for a "fade now" request, capped at the track's own fade-out length.
        /// </summary>
        public static double GetForceFadeLength(Track track, ForceFadeType fadeType)
        {
            var length = 0D;
            switch (fadeType)
            {
                case ForceFadeType.Cut:
                    length = GetCutFadeLength(track);
                    break;
                case ForceFadeType.PowerDown:
                    length = GetPowerDownFadeLength(track);
                    break;
                case ForceFadeType.QuickFade:
                    length = GetQuickFadeLength(track);
                    break;
                case ForceFadeType.SkipToEnd:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fadeType), fadeType, null);
            }

            if (track.FadeOutLengthSeconds < length)
                length = track.FadeOutLengthSeconds;

            return length;
        }

        /// <summary>
        ///     Plans how the outgoing track fades when the incoming track starts. Checked in order:
        ///     manual mix mode, a recorded extended mix, the track's power-down flag, a "fade now"
        ///     request, and otherwise a normal crossfade.
        /// </summary>
        /// <param name="outgoingTrack">The track fading out (not null).</param>
        /// <param name="incomingTrack">The track fading in, or null.</param>
        /// <param name="mix">The extended mix attributes for this pair, or null.</param>
        /// <param name="isManualMixMode">True if the DJ is mixing by hand.</param>
        /// <param name="manualFadeType">The fade type chosen for this transition in manual mix mode.</param>
        /// <param name="isForceFadeNow">True if a "fade now" was requested.</param>
        public static FadeOutPlan PlanFadeOut(Track outgoingTrack, Track incomingTrack, ExtendedMixAttributes mix,
            bool isManualMixMode, ExtendedFadeType manualFadeType, bool isForceFadeNow)
        {
            var plan = PlanFadeOutVolume(outgoingTrack, incomingTrack, mix, isManualMixMode, manualFadeType,
                isForceFadeNow);

            // the outgoing track's bass is cut on every fade, so it does not clash with the incoming track's
            plan.HighPass = true;
            return plan;
        }

        private static FadeOutPlan PlanFadeOutVolume(Track outgoingTrack, Track incomingTrack,
            ExtendedMixAttributes mix, bool isManualMixMode, ExtendedFadeType manualFadeType, bool isForceFadeNow)
        {
            if (isManualMixMode)
            {
                var plan = PlanManualFadeOut(outgoingTrack, incomingTrack, manualFadeType);
                plan.ReshapeAs = manualFadeType;
                return plan;
            }

            if (mix != null && mix.FadeLength != 0)
                return PlanExtendedMixFadeOut(outgoingTrack, incomingTrack, mix);

            if (outgoingTrack.PowerDownOnEnd)
                return FadeOutPlan.PowerDown();

            if (isForceFadeNow)
                return FadeOutPlan.Slide(outgoingTrack.FadeOutStartVolume, outgoingTrack.FadeOutEndVolume,
                    outgoingTrack.FadeOutLengthSeconds);

            return FadeOutPlan.Slide(outgoingTrack.FadeOutStartVolume, outgoingTrack.FadeOutEndVolume,
                GetFadeLength(outgoingTrack, incomingTrack, mix));
        }

        private static FadeOutPlan PlanManualFadeOut(Track outgoingTrack, Track incomingTrack,
            ExtendedFadeType fadeType)
        {
            FadeOutPlan plan;
            switch (fadeType)
            {
                case ExtendedFadeType.Default:
                    return FadeOutPlan.None();
                case ExtendedFadeType.Cut:
                    plan = FadeOutPlan.Slide(outgoingTrack.FadeOutStartVolume, 0, GetCutFadeLength(incomingTrack));
                    break;
                case ExtendedFadeType.QuickFade:
                    plan = FadeOutPlan.Slide(outgoingTrack.FadeOutStartVolume, 0, GetQuickFadeLength(incomingTrack));
                    break;
                case ExtendedFadeType.EchoOut:
                    plan = FadeOutPlan.Slide(outgoingTrack.FadeOutStartVolume, 0,
                        GetEchoOutFadeLength(outgoingTrack, incomingTrack));
                    break;
                case ExtendedFadeType.PowerDown:
                    plan = FadeOutPlan.PowerDown();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fadeType), fadeType, null);
            }

            plan.EndsAutoMixRecording = true;
            return plan;
        }

        private static FadeOutPlan PlanExtendedMixFadeOut(Track outgoingTrack, Track incomingTrack,
            ExtendedMixAttributes mix)
        {
            FadeOutPlan plan;
            switch (mix.ExtendedFadeType)
            {
                case ExtendedFadeType.Default:
                    return outgoingTrack.PowerDownOnEnd
                        ? FadeOutPlan.PowerDown()
                        : FadeOutPlan.Slide(outgoingTrack.FadeOutStartVolume, mix.FadeEndVolume, mix.FadeLength);
                case ExtendedFadeType.PowerDown:
                    return FadeOutPlan.PowerDown();
                case ExtendedFadeType.Cut:
                    plan = FadeOutPlan.Slide(outgoingTrack.FadeOutStartVolume, 0, GetCutFadeLength(incomingTrack));
                    break;
                case ExtendedFadeType.QuickFade:
                    plan = FadeOutPlan.Slide(outgoingTrack.FadeOutStartVolume, 0, GetQuickFadeLength(incomingTrack));
                    break;
                case ExtendedFadeType.EchoOut:
                    plan = FadeOutPlan.Slide(outgoingTrack.FadeOutStartVolume, 0,
                        GetEchoOutFadeLength(outgoingTrack, incomingTrack));
                    plan.ReshapeAs = ExtendedFadeType.EchoOut;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mix), mix.ExtendedFadeType, null);
            }

            plan.EndsAutoMixRecording = true;
            return plan;
        }
    }
}
