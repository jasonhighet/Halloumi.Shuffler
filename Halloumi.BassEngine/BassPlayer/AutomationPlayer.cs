using System;
using System.Collections.Generic;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     Plays back the current track's saved automation (track FX sends and samples) at their
    ///     positions, and saves, clears and removes those triggers.
    /// </summary>
    internal class AutomationPlayer
    {
        private readonly TrackDeck _deck;
        private readonly TrackFxSend _fxSend;
        private readonly Sampler _sampler;
        private readonly TriggerRecorder _recorder;
        private readonly Action _resetTrackSyncPositions;

        /// <param name="syncs">Where to register the trigger sync handlers.</param>
        /// <param name="deck">The track slots.</param>
        /// <param name="fxSend">Plays track FX triggers.</param>
        /// <param name="sampler">Plays sample triggers.</param>
        /// <param name="recorder">Holds the triggers being recorded.</param>
        /// <param name="resetTrackSyncPositions">Re-sets the in-use tracks' syncs after the triggers change.</param>
        public AutomationPlayer(SyncDispatcher syncs, TrackDeck deck, TrackFxSend fxSend, Sampler sampler,
            TriggerRecorder recorder, Action resetTrackSyncPositions)
        {
            _deck = deck;
            _fxSend = fxSend;
            _sampler = sampler;
            _recorder = recorder;
            _resetTrackSyncPositions = resetTrackSyncPositions;

            syncs.On(TrackSyncType.StartTrackFxTrigger, track => StartTrackFxTrigger());
            syncs.On(TrackSyncType.EndTrackFxTrigger, track => StopTrackFxTrigger());
            syncs.On(TrackSyncType.StartSampleTrigger, track => StartSampleTrigger());
            syncs.On(TrackSyncType.EndSampleTrigger, track => StopSampleTrigger());
        }

        /// <summary>
        ///     If false, saved track FX triggers are not played.
        /// </summary>
        public bool TrackFxEnabled { get; set; }

        /// <summary>
        ///     If false, saved sample triggers are not played.
        /// </summary>
        public bool SamplesEnabled { get; set; }

        private Track CurrentTrack => _deck.Current;

        private Track NextTrack => _deck.Next;

        /// <summary>
        ///     Gets the syncs for the current track's track FX and sample triggers (Playback sets them
        ///     together with the track's other syncs).
        /// </summary>
        public List<KeyValuePair<TrackSyncType, long>> GetSyncPositions(Track track)
        {
            var syncs = new List<KeyValuePair<TrackSyncType, long>>();
            if (track != CurrentTrack) return syncs;

            foreach (var trigger in AutomationAttributesHelper.GetAutomationAttributes(track.Description).TrackFXTriggers)
            {
                trigger.StartSample = track.SecondsToSamples(trigger.Start);
                trigger.EndSample = track.SecondsToSamples(trigger.Start + trigger.Length);
                syncs.Add(new KeyValuePair<TrackSyncType, long>(TrackSyncType.StartTrackFxTrigger, trigger.StartSample));
                syncs.Add(new KeyValuePair<TrackSyncType, long>(TrackSyncType.EndTrackFxTrigger, trigger.EndSample));
            }

            foreach (var trigger in GetCurrentSampleTriggers())
            {
                trigger.StartSample = track.SecondsToSamples(trigger.Start);
                trigger.EndSample = track.SecondsToSamples(trigger.Start + trigger.Length);
                syncs.Add(new KeyValuePair<TrackSyncType, long>(TrackSyncType.StartSampleTrigger, trigger.StartSample));
                syncs.Add(new KeyValuePair<TrackSyncType, long>(TrackSyncType.EndSampleTrigger, trigger.EndSample));
            }

            return syncs;
        }

        // ---------- track FX triggers ----------

        /// <summary>
        ///     Saves the recorded track FX trigger.
        /// </summary>
        public void SaveTrackFxTrigger()
        {
            var track = _recorder.SaveTrackFx();
            if (track != null && _deck.IsInUse(track)) _resetTrackSyncPositions();
        }

        /// <summary>
        ///     Clears the track FX triggers for a track.
        /// </summary>
        public void ClearTrackFxTriggers(Track track)
        {
            if (track == null) return;
            var attributes = AutomationAttributesHelper.GetAutomationAttributes(track.Description);

            if (attributes.TrackFXTriggers.Count == 0) return;

            attributes.TrackFXTriggers.Clear();
            AutomationAttributesHelper.SaveAutomationAttributes(track.Description, attributes);

            if (_deck.IsInUse(track)) _resetTrackSyncPositions();
        }

        /// <summary>
        ///     Removes the current track's most recent track FX trigger before the play position.
        /// </summary>
        public void RemovePreviousTrackFxTrigger()
        {
            if (CurrentTrack == null) return;
            var attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);
            if (attributes.TrackFXTriggers.Count == 0) return;

            var trigger = GetPrevTrackFxTrigger(attributes);
            if (trigger == null) return;

            attributes.TrackFXTriggers.Remove(trigger);
            AutomationAttributesHelper.SaveAutomationAttributes(CurrentTrack.Description, attributes);

            _resetTrackSyncPositions();
        }

        private void StartTrackFxTrigger()
        {
            if (!TrackFxEnabled) return;
            if (_fxSend.IsSending) return;

            var trigger = GetCurrentTrackFxTrigger();
            if (trigger == null) return;

            if (_fxSend.DelayNotes != trigger.DelayNotes) _fxSend.DelayNotes = trigger.DelayNotes;

            _fxSend.Start();
        }

        private void StopTrackFxTrigger()
        {
            if (!TrackFxEnabled) return;

            _fxSend.Stop();
        }

        /// <summary>
        ///     Gets the current track's track FX trigger nearest the play position.
        /// </summary>
        private TrackFXTrigger GetCurrentTrackFxTrigger()
        {
            if (CurrentTrack == null) return null;

            var attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);
            var position = AudioStreamHelper.GetPosition(CurrentTrack);
            return attributes
                .TrackFXTriggers
                .OrderBy(ta => Math.Abs(ta.StartSample - position))
                .FirstOrDefault();
        }

        /// <summary>
        ///     Gets the current track's nearest track FX trigger at or before the play position.
        /// </summary>
        private TrackFXTrigger GetPrevTrackFxTrigger(AutomationAttributes attributes)
        {
            if (CurrentTrack == null) return null;

            var position = AudioStreamHelper.GetPosition(CurrentTrack);
            return attributes
                .TrackFXTriggers
                .Where(ta => ta.StartSample <= position)
                .OrderBy(ta => Math.Abs(ta.StartSample - position))
                .FirstOrDefault();
        }

        // ---------- sample triggers ----------

        /// <summary>
        ///     Saves the recorded sample trigger.
        /// </summary>
        public void SaveSampleTrigger()
        {
            var trigger = _recorder.SampleTrigger;
            var sample = trigger == null ? null : _sampler.GetSampleById(trigger.SampleId);

            var track = _recorder.SaveSample(sample);
            if (track != null && _deck.IsInUse(track)) _resetTrackSyncPositions();
        }

        /// <summary>
        ///     Clears the current track's sample triggers, including those for the mix into the next track.
        /// </summary>
        public void ClearSampleTriggers()
        {
            if (CurrentTrack == null) return;
            var attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);

            attributes.SampleTriggers.Clear();

            var mixDetails = NextTrack != null ? attributes.GetExtendedMixAttributes(NextTrack.Description) : null;
            mixDetails?.SampleTriggers.Clear();

            AutomationAttributesHelper.SaveAutomationAttributes(CurrentTrack.Description, attributes);

            _resetTrackSyncPositions();
        }

        /// <summary>
        ///     Removes the current track's most recent sample trigger before the play position.
        /// </summary>
        public void RemovePreviousSampleTrigger()
        {
            if (CurrentTrack == null) return;
            var attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);

            var trigger = GetPrevSampleTrigger(attributes);
            if (trigger == null) return;

            var nextTrackSampleTriggers = GetNextTrackSampleTriggers(attributes);

            if (attributes.SampleTriggers.Contains(trigger))
                attributes.SampleTriggers.Remove(trigger);
            else if (nextTrackSampleTriggers.Contains(trigger))
                nextTrackSampleTriggers.Remove(trigger);

            AutomationAttributesHelper.SaveAutomationAttributes(CurrentTrack.Description, attributes);

            _resetTrackSyncPositions();
        }

        private void StartSampleTrigger()
        {
            if (!SamplesEnabled) return;

            var trigger = GetCurrentSampleTrigger();
            if (trigger == null) return;

            var sample = _sampler.GetSampleById(trigger.SampleId);
            if (sample == null) return;

            if (_sampler.DelayNotes != trigger.DelayNotes) _sampler.DelayNotes = trigger.DelayNotes;

            _sampler.Play(sample);
        }

        private void StopSampleTrigger()
        {
            if (!SamplesEnabled) return;

            var trigger = GetPrevSampleTrigger();
            if (trigger == null) return;

            var sample = _sampler.GetSampleById(trigger.SampleId);
            if (sample == null) return;

            _sampler.Pause(sample);
        }

        /// <summary>
        ///     Gets the current track's sample trigger nearest the play position.
        /// </summary>
        private SampleTrigger GetCurrentSampleTrigger()
        {
            if (CurrentTrack == null) return null;

            var attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);
            var position = AudioStreamHelper.GetPosition(CurrentTrack);

            return GetCurrentSampleTriggers(attributes)
                .OrderBy(t => Math.Abs(t.StartSample - position))
                .FirstOrDefault();
        }

        /// <summary>
        ///     Gets the current track's nearest sample trigger at or before the play position.
        /// </summary>
        private SampleTrigger GetPrevSampleTrigger(AutomationAttributes attributes = null)
        {
            if (CurrentTrack == null) return null;
            if (attributes == null)
                attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);

            var position = AudioStreamHelper.GetPosition(CurrentTrack);

            return GetCurrentSampleTriggers(attributes)
                .Where(t => t.StartSample <= position)
                .OrderBy(t => Math.Abs(t.StartSample - position))
                .FirstOrDefault();
        }

        /// <summary>
        ///     Gets the sample triggers for the current track, including ones saved with the mix into the next track.
        /// </summary>
        private IEnumerable<SampleTrigger> GetCurrentSampleTriggers(AutomationAttributes attributes = null)
        {
            if (CurrentTrack == null) return new List<SampleTrigger>();

            if (attributes == null)
                attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);

            if (attributes == null) return new List<SampleTrigger>();

            return attributes
                .SampleTriggers
                .Union(GetNextTrackSampleTriggers())
                .OrderBy(t => t.StartSample)
                .ToList();
        }

        /// <summary>
        ///     Gets the sample triggers saved with the mix from the current track into the next track.
        /// </summary>
        private List<SampleTrigger> GetNextTrackSampleTriggers(AutomationAttributes attributes = null)
        {
            if (NextTrack == null) return new List<SampleTrigger>();

            if (attributes == null)
                attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);

            if (attributes == null) return new List<SampleTrigger>();

            var mixDetails = attributes.GetExtendedMixAttributes(NextTrack.Description);
            return mixDetails == null ? new List<SampleTrigger>() : mixDetails.SampleTriggers;
        }
    }
}
