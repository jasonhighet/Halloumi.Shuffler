using System;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Mixing;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     Manual mix mode: the DJ fades the outgoing track by hand (or picks an automatic fade type
    ///     for each transition), and the result is recorded as an extended mix that can be saved
    ///     and replayed automatically next time the same two tracks are mixed.
    ///     Playback asks it whether manual mode is on, tells it when a transition starts, and lets
    ///     it record mixes that used an automatic fade type.
    /// </summary>
    internal class ManualMixer
    {
        private readonly TrackDeck _deck;
        private readonly FadeDefaults _fadeDefaults;
        private readonly TriggerRecorder _recorder;
        private readonly Func<bool> _isPlaying;

        /// <param name="deck">The track slots.</param>
        /// <param name="fadeDefaults">The default fade-out volumes the fader works between.</param>
        /// <param name="recorder">Holds the mix being recorded until it is saved.</param>
        /// <param name="isPlaying">True while the player is playing.</param>
        public ManualMixer(TrackDeck deck, FadeDefaults fadeDefaults, TriggerRecorder recorder, Func<bool> isPlaying)
        {
            _deck = deck;
            _fadeDefaults = fadeDefaults;
            _recorder = recorder;
            _isPlaying = isPlaying;

            FadeTypeIn = ExtendedFadeType.Default;
            FadeTypeOut = ExtendedFadeType.Default;
        }

        public event EventHandler ModeChanged;

        public event EventHandler VolumeChanged;

        /// <summary>
        ///     Raised when a hand-made mix is recorded (the DJ has finished the fade).
        /// </summary>
        public event EventHandler MixRecorded;

        /// <summary>
        ///     True when the DJ fades the outgoing track by hand.
        /// </summary>
        public bool IsOn { get; private set; }

        /// <summary>
        ///     The fade type chosen for the transition out of the current track.
        /// </summary>
        public ExtendedFadeType FadeTypeOut { get; set; }

        /// <summary>
        ///     The fade type that was chosen for the transition into the current track (the one in progress).
        /// </summary>
        public ExtendedFadeType FadeTypeIn { get; private set; }

        private Track CurrentTrack => _deck.Current;

        private Track PreviousTrack => _deck.Previous;

        public void Toggle()
        {
            if (IsOn)
                Disable();
            else
                Enable();
        }

        public void Enable()
        {
            IsOn = true;
            FadeTypeOut = ExtendedFadeType.Default;
            FadeTypeIn = ExtendedFadeType.Default;
            SetVolume(0);
            ModeChanged?.Invoke(CurrentTrack, EventArgs.Empty);
        }

        public void Disable()
        {
            IsOn = false;
            FadeTypeOut = ExtendedFadeType.Default;
            FadeTypeIn = ExtendedFadeType.Default;
            PausePreviousTrack();
            SetVolume(0);
            ModeChanged?.Invoke(CurrentTrack, EventArgs.Empty);
        }

        /// <summary>
        ///     Called by Playback when a transition starts: the "out" fade type becomes the "in" one,
        ///     and the new "out" one is the recorded mix's type for the new current and next tracks.
        /// </summary>
        public void MoveToNextTransition(ExtendedFadeType nextFadeTypeOut)
        {
            FadeTypeIn = FadeTypeOut;
            FadeTypeOut = nextFadeTypeOut;
        }

        /// <summary>
        ///     Sets how far the outgoing track has been faded by hand (0 = not at all, 100 = fully).
        /// </summary>
        public void SetVolume(decimal value)
        {
            value = 100M - value;

            var track = PreviousTrack;
            if (track == null) return;

            var range = (decimal) _fadeDefaults.FadeOutStartVolume;
            var volume = range * (value / 100);
            volume = (decimal) _fadeDefaults.FadeOutEndVolume + volume;

            AudioStreamHelper.SetVolume(track, volume);

            VolumeChanged?.Invoke(CurrentTrack, EventArgs.Empty);
        }

        public decimal GetVolume()
        {
            if (PreviousTrack == null) return 100M;

            var volume = AudioStreamHelper.GetVolume(PreviousTrack) - (decimal) _fadeDefaults.FadeOutEndVolume;
            volume = volume / (decimal) _fadeDefaults.FadeOutStartVolume;
            volume = volume * 100;
            return 100 - volume;
        }

        /// <summary>
        ///     Plays the power-down effect on the outgoing track and records the mix, ending in a power-down.
        /// </summary>
        public void PowerOffPreviousTrack()
        {
            if (PreviousTrack == null) return;
            if (!_isPlaying()) return;
            if (!_deck.IsInUse(PreviousTrack)) return;

            AudioStreamHelper.PowerDown(PreviousTrack);
            RecordManualMix(true);
        }

        /// <summary>
        ///     Stops the outgoing track and records the mix as it stands.
        /// </summary>
        public void PausePreviousTrack()
        {
            if (PreviousTrack == null) return;
            if (!_deck.IsInUse(PreviousTrack)) return;
            if (!AudioStreamHelper.IsPlaying(PreviousTrack)) return;

            RecordManualMix();
            AudioStreamHelper.SmoothPause(PreviousTrack);
        }

        /// <summary>
        ///     In manual mix mode, records a mix that used one of the automatic fade types
        ///     (cut, quick fade, echo-out or power-down). Called by Playback once the fade has started.
        /// </summary>
        public void RecordAutoMix()
        {
            if (CurrentTrack == null) return;
            if (PreviousTrack == null) return;
            if (!IsOn) return;
            if (FadeTypeIn == ExtendedFadeType.Default) return;

            // the end volume is the fade-in end volume - kept as it has always been
            _recorder.RecordExtendedMix(PreviousTrack, CurrentTrack, FadeTypeIn,
                PreviousTrack.FadeOutEnd,
                PreviousTrack.FadeOutLengthSeconds,
                PreviousTrack.EndLoopCount,
                PreviousTrack.FadeInEndVolume,
                false);
        }

        /// <summary>
        ///     Saves the recorded mix.
        /// </summary>
        public void SaveMix()
        {
            _recorder.SaveExtendedMix();
        }

        /// <summary>
        ///     Removes the saved mix between the previous and current tracks.
        /// </summary>
        public void ClearMix()
        {
            if (CurrentTrack == null || PreviousTrack == null) return;

            var attributes = AutomationAttributesHelper.GetAutomationAttributes(PreviousTrack.Description);
            if (attributes == null) return;

            attributes.RemoveExtendedMixAttributes(CurrentTrack.Description);
            AutomationAttributesHelper.SaveAutomationAttributes(PreviousTrack.Description, attributes);

            _recorder.DiscardExtendedMix();
        }

        /// <summary>
        ///     Records the hand-made mix from the previous track into the current one as it stands now.
        /// </summary>
        private void RecordManualMix(bool powerDownAfterFade = false)
        {
            var current = CurrentTrack;
            var previous = PreviousTrack;
            if (current == null) return;
            if (previous == null) return;
            if (!IsOn) return;
            if (FadeTypeIn == ExtendedFadeType.PowerDown) return;

            var fadeLength = current.SamplesToSeconds(
                TrackTimeline.ToAdjustedPosition(current, AudioStreamHelper.GetPosition(current)));

            _recorder.RecordExtendedMix(previous, current, FadeTypeIn,
                AudioStreamHelper.GetPosition(previous),
                fadeLength,
                previous.CurrentEndLoop,
                Convert.ToSingle(AudioStreamHelper.GetVolume(previous) / 100),
                powerDownAfterFade);

            MixRecorded?.Invoke(CurrentTrack, EventArgs.Empty);
        }
    }
}
