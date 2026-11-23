using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.Mixing
{
    /// <summary>
    ///     The fade settings a track gets when it has no hand-set fade points, and the code that
    ///     applies them. No BASS calls.
    /// </summary>
    public class FadeDefaults
    {
        public FadeDefaults()
        {
            FadeLength = 10;
            FadeInStartVolume = 50;
            FadeInEndVolume = 100;
            FadeOutStartVolume = 80;
            FadeOutEndVolume = 0;
            LimitSongLength = false;
            MaxSongLength = 5 * 60;
        }

        /// <summary>Default crossfade length in seconds.</summary>
        public double FadeLength { get; set; }

        /// <summary>Volume (0 - 100) at the start of the fade-in.</summary>
        public double FadeInStartVolume { get; set; }

        /// <summary>Volume (0 - 100) at the end of the fade-in.</summary>
        public double FadeInEndVolume { get; set; }

        /// <summary>Volume (0 - 100) at the start of the fade-out.</summary>
        public double FadeOutStartVolume { get; set; }

        /// <summary>Volume (0 - 100) at the end of the fade-out.</summary>
        public double FadeOutEndVolume { get; set; }

        /// <summary>If true, tracks with no fade-out set start fading at MaxSongLength.</summary>
        public bool LimitSongLength { get; set; }

        /// <summary>Maximum song length in seconds when LimitSongLength is set.</summary>
        public double MaxSongLength { get; set; }

        /// <summary>
        ///     Sets the fade volumes and clears the fade points, ready for the track's saved attributes
        ///     to be loaded over them. The fade-out start is cleared too: "fade now" moves it, and
        ///     without this a track with no saved fade-out kept that position the next time it played.
        /// </summary>
        public void ResetFades(Track track)
        {
            track.FadeInStart = 0;
            track.FadeInStartVolume = (float) (FadeInStartVolume / 100);
            track.FadeInEndVolume = (float) (FadeInEndVolume / 100);

            track.FadeOutStart = 0;
            track.FadeOutEnd = 0;
            track.FadeOutStartVolume = (float) (FadeOutStartVolume / 100);
            track.FadeOutEndVolume = (float) (FadeOutEndVolume / 100);
        }

        /// <summary>
        ///     Fills in any fade points the saved attributes did not set: the fade-out starts near the
        ///     end of the track (or at MaxSongLength), and each fade lasts the loop length nearest
        ///     FadeLength at the track's tempo. Needs the track's length and BPM.
        /// </summary>
        public void FillMissingFadePoints(Track track)
        {
            if (track.FadeOutStart == 0)
                if (LimitSongLength && track.LengthSeconds > MaxSongLength)
                    track.FadeOutStart = track.SecondsToSamples(MaxSongLength - FadeLength - 1);
                else
                    track.FadeOutStart = track.SecondsToSamples(track.LengthSeconds - FadeLength - 1);

            if (track.FadeInEnd == track.FadeInStart || track.FadeInEnd == 0)
                track.FadeInEnd = track.FadeInStart +
                                  track.SecondsToSamples(BpmHelper.GetBestFitLoopLength(track.StartBpm, FadeLength));

            // compares with FadeInStart, not FadeOutStart - kept as it has always been
            if (track.FadeOutEnd == track.FadeInStart || track.FadeOutEnd == 0)
                track.FadeOutEnd = track.FadeOutStart +
                                   track.SecondsToSamples(BpmHelper.GetBestFitLoopLength(track.EndBpm, FadeLength));

            if (!track.UsePreFadeIn)
            {
                track.PreFadeInStart = track.FadeInStart;
                track.PreFadeInStartVolume = 0;
            }
        }
    }
}
