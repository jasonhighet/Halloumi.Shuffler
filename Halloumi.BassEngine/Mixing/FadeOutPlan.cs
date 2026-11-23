using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.Mixing
{
    /// <summary>
    ///     What happens to the outgoing track's volume when a transition starts.
    /// </summary>
    public enum FadeOutAction
    {
        /// <summary>Leave the volume alone (manual mix mode: the DJ fades it by hand).</summary>
        None,

        /// <summary>Slide the volume from StartVolume to EndVolume over LengthSeconds.</summary>
        VolumeSlide,

        /// <summary>Play the power-down effect.</summary>
        PowerDown
    }

    /// <summary>
    ///     How the outgoing track should fade when the next track takes over.
    ///     Made by <see cref="TransitionPlanner.PlanFadeOut" /> and carried out by BassPlayer.
    /// </summary>
    public class FadeOutPlan
    {
        public FadeOutAction Action { get; set; }

        /// <summary>Volume (0 - 1) at the start of the slide.</summary>
        public float StartVolume { get; set; }

        /// <summary>Volume (0 - 1) at the end of the slide.</summary>
        public float EndVolume { get; set; }

        /// <summary>Length of the slide in seconds.</summary>
        public double LengthSeconds { get; set; }

        /// <summary>
        ///     If set, the outgoing track's fade-out section is reshaped for this fade type
        ///     (see <see cref="TransitionPlanner.ShapeFadeOut" />) before the fade starts.
        /// </summary>
        public ExtendedFadeType? ReshapeAs { get; set; }

        /// <summary>
        ///     True if this fade finishes recording an automatic extended mix
        ///     (only has an effect in manual mix mode).
        /// </summary>
        public bool EndsAutoMixRecording { get; set; }

        public static FadeOutPlan None()
        {
            return new FadeOutPlan { Action = FadeOutAction.None };
        }

        public static FadeOutPlan PowerDown()
        {
            return new FadeOutPlan { Action = FadeOutAction.PowerDown };
        }

        public static FadeOutPlan Slide(float startVolume, float endVolume, double lengthSeconds)
        {
            return new FadeOutPlan
            {
                Action = FadeOutAction.VolumeSlide,
                StartVolume = startVolume,
                EndVolume = endVolume,
                LengthSeconds = lengthSeconds
            };
        }
    }
}
