using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Mixing;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     Records automation as the DJ plays: a track FX send, a sample, or a hand-made extended mix.
    ///     Each recording is held until it is saved to the automation attributes or replaced by the
    ///     next one. Positions are passed in, so nothing here touches BASS.
    /// </summary>
    internal class TriggerRecorder
    {
        // ---------- track FX ----------

        /// <summary>The track FX trigger being recorded, or null.</summary>
        public TrackFXTrigger TrackFxTrigger { get; private set; }

        /// <summary>The track the track FX trigger was recorded on.</summary>
        public Track TrackFxTrack { get; private set; }

        public void StartTrackFx(Track track, long position, decimal delayNotes)
        {
            TrackFxTrack = track;
            TrackFxTrigger = new TrackFXTrigger
            {
                Start = track.SamplesToSeconds(position),
                DelayNotes = delayNotes
            };
        }

        /// <summary>
        ///     Sets the recorded trigger's length from where the track is now. If it ran past the
        ///     fade-out (or the position went backwards), it runs to the fade-out.
        /// </summary>
        public void EndTrackFx(long position)
        {
            if (TrackFxTrack == null || TrackFxTrigger == null) return;
            TrackFxTrigger.Length = GetLength(TrackFxTrack, TrackFxTrigger.Start, position);
        }

        /// <summary>
        ///     Saves the recorded track FX trigger to its track's automation attributes.
        /// </summary>
        /// <returns>The track it was saved to, or null if nothing was recorded.</returns>
        public Track SaveTrackFx()
        {
            var track = TrackFxTrack;
            var trigger = TrackFxTrigger;
            TrackFxTrack = null;
            TrackFxTrigger = null;
            if (track == null || trigger == null) return null;

            var attributes = AutomationAttributesHelper.GetAutomationAttributes(track.Description);
            attributes.TrackFXTriggers.Add(trigger);
            AutomationAttributesHelper.SaveAutomationAttributes(track.Description, attributes);
            return track;
        }

        // ---------- samples ----------

        /// <summary>The sample trigger being recorded, or null.</summary>
        public SampleTrigger SampleTrigger { get; private set; }

        /// <summary>The track the sample trigger was recorded on.</summary>
        public Track SampleTrack { get; private set; }

        /// <summary>The previous track when the sample trigger was recorded ("" if none).</summary>
        public string SamplePreviousTrackDescription { get; private set; }

        /// <summary>The next track when the sample trigger was recorded ("" if none).</summary>
        public string SampleNextTrackDescription { get; private set; }

        public void StartSample(Track track, Track previousTrack, Track nextTrack, long position,
            decimal delayNotes, string sampleId)
        {
            SampleTrack = track;
            SampleNextTrackDescription = nextTrack == null ? "" : nextTrack.Description;
            SamplePreviousTrackDescription = previousTrack == null ? "" : previousTrack.Description;
            SampleTrigger = new SampleTrigger
            {
                Start = track.SamplesToSeconds(position),
                DelayNotes = delayNotes,
                SampleId = sampleId
            };
        }

        /// <summary>
        ///     Sets the recorded sample trigger's length from where the track is now.
        /// </summary>
        public void EndSample(long position)
        {
            if (SampleTrack == null || SampleTrigger == null) return;
            SampleTrigger.Length = GetLength(SampleTrack, SampleTrigger.Start, position);
        }

        /// <summary>
        ///     Saves the recorded sample trigger. A sample from the next track is saved with the mix
        ///     into that track; a sample from the previous track is not saved; any other sample is
        ///     saved on the track it was played over.
        /// </summary>
        /// <param name="sample">The sample the trigger plays, or null if it is no longer loaded.</param>
        /// <returns>The track it was saved to, or null if nothing was saved.</returns>
        public Track SaveSample(Sample sample)
        {
            var track = SampleTrack;
            var trigger = SampleTrigger;
            var previousDescription = SamplePreviousTrackDescription;
            var nextDescription = SampleNextTrackDescription;

            SampleTrack = null;
            SampleTrigger = null;
            SamplePreviousTrackDescription = "";
            SampleNextTrackDescription = "";

            if (track == null || trigger == null || sample == null) return null;

            var attributes = AutomationAttributesHelper.GetAutomationAttributes(track.Description);

            if (sample.LinkedTrackDescription != previousDescription
                && sample.LinkedTrackDescription != nextDescription)
            {
                attributes.SampleTriggers.Add(trigger);
            }
            else if (sample.LinkedTrackDescription == nextDescription)
            {
                var mixDetails = attributes.GetExtendedMixAttributes(nextDescription);
                if (mixDetails == null)
                {
                    mixDetails = new ExtendedMixAttributes { TrackDescription = nextDescription };
                    attributes.ExtendedMixes.Add(mixDetails);
                }
                mixDetails.SampleTriggers.Add(trigger);
            }

            AutomationAttributesHelper.SaveAutomationAttributes(track.Description, attributes);
            return track;
        }

        // ---------- extended mix ----------

        /// <summary>The extended mix being recorded, or null.</summary>
        public ExtendedMixAttributes ExtendedMix { get; private set; }

        /// <summary>The fade-out track of the extended mix being recorded.</summary>
        public Track ExtendedMixTrack { get; private set; }

        /// <summary>
        ///     Records (or re-records) the mix from one track into the next.
        /// </summary>
        public void RecordExtendedMix(Track fadeOutTrack, Track fadeInTrack, ExtendedFadeType fadeType,
            long fadeEnd, double fadeLength, long fadeEndLoop, float fadeEndVolume, bool powerDownAfterFade)
        {
            ExtendedMixTrack = fadeOutTrack;
            ExtendedMix = new ExtendedMixAttributes
            {
                TrackDescription = fadeInTrack.Description,
                ExtendedFadeType = fadeType,
                FadeEnd = fadeEnd,
                FadeLength = fadeLength,
                FadeEndLoop = fadeEndLoop,
                FadeEndVolume = fadeEndVolume,
                PowerDownAfterFade = powerDownAfterFade,

                // the mix was played with the fades where they are now, moved or not, so it keeps those moves
                FadeOutShift = FadeShift.GetFadeOutShift(fadeOutTrack),
                FadeInShift = FadeShift.GetFadeInShift(fadeInTrack)
            };
        }

        /// <summary>
        ///     Saves the recorded extended mix, keeping any sample triggers already saved for that mix.
        /// </summary>
        public void SaveExtendedMix()
        {
            var track = ExtendedMixTrack;
            var mix = ExtendedMix;
            if (track == null || mix == null) return;

            var attributes = AutomationAttributesHelper.GetAutomationAttributes(track.Description);
            if (attributes == null) return;

            var existingMix = attributes.GetExtendedMixAttributes(mix.TrackDescription);
            if (existingMix != null)
            {
                mix.SampleTriggers.AddRange(existingMix.SampleTriggers);
                attributes.RemoveExtendedMixAttributes(mix.TrackDescription);
            }

            attributes.ExtendedMixes.Add(mix);
            AutomationAttributesHelper.SaveAutomationAttributes(track.Description, attributes);

            DiscardExtendedMix();
        }

        public void DiscardExtendedMix()
        {
            ExtendedMixTrack = null;
            ExtendedMix = null;
        }

        private static double GetLength(Track track, double startSeconds, long position)
        {
            var length = track.SamplesToSeconds(position) - startSeconds;
            if (length <= 0 || position >= track.FadeOutStart)
                length = track.SamplesToSeconds(track.FadeOutStart) - startSeconds;
            return length;
        }
    }
}
