using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Halloumi.Common.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.AddOn.Vst;
using Un4seen.Bass.AddOn.WaDsp;

namespace Halloumi.BassEngine
{
    public partial class BassPlayer
    {
        /// <summary>
        /// Gets the last track FX attributes.
        /// </summary>
        public TrackFxTrigger LastTrackFxTrigger
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the last track FX track.
        /// </summary>
        public Track LastTrackFxTriggerTrack
        {
            get;
            private set;
        }

        private void InitialiseManualMixer()
        {
            this.ManualFadeOut = false;
            PreviousManaulExtendedFadeType = ExtendedFadeType.Default;
            CurrentManualExtendedFadeType = ExtendedFadeType.Default;
        }

        /// <summary>
        /// Saves the last track FX.
        /// </summary>
        public void SaveLastTrackFxTrigger()
        {
            if (this.LastTrackFxTriggerTrack == null || this.LastTrackFxTrigger == null) return;

            var attributes = GetAutomationAttributes(this.LastTrackFxTriggerTrack);
            attributes.TrackFxTriggers.Add(this.LastTrackFxTrigger);

            SaveAutomationAttributes(this.LastTrackFxTriggerTrack);

            if (IsTrackInUse(this.LastTrackFxTriggerTrack)) ResetTrackSyncPositions();

            this.LastTrackFxTriggerTrack = null;
            this.LastTrackFxTrigger = null;
        }

        /// <summary>
        /// Clears the track FX triggers for the specifed track.
        /// </summary>
        public void ClearTrackFxTriggers(Track track)
        {
            if (track == null) return;
            var attributes = GetAutomationAttributes(track);

            if (attributes.TrackFxTriggers.Count == 0) return;

            if (IsTrackInUse(track)) ClearTrackSyncPositions(track);
            attributes.TrackFxTriggers.Clear();
            SaveAutomationAttributes(track);
            if (IsTrackInUse(track)) ResetTrackSyncPositions();
        }

        /// <summary>
        /// Removes the previous track FX automation.
        /// </summary>
        public void RemovePreviousTrackFxTrigger()
        {
            if (this.CurrentTrack == null) return;
            var attributes = GetAutomationAttributes(this.CurrentTrack);
            if (attributes.TrackFxTriggers.Count == 0) return;

            var automation = GetPrevTrackFxTrigger();
            if (automation == null) return;

            ClearTrackSyncPositions(this.CurrentTrack);
            attributes.TrackFxTriggers.Remove(automation);
            SaveAutomationAttributes(this.CurrentTrack);

            ResetTrackSyncPositions();
        }

        /// <summary>
        /// Gets the last track FX attributes.
        /// </summary>
        public SampleTrigger LastSampleTrigger
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the last track FX track.
        /// </summary>
        public Track LastSampleTriggerTrack
        {
            get;
            private set;
        }

        public String LastSampleTriggerPrevTrackDescription
        {
            get;
            private set;
        }

        public String LastSampleTriggerNextTrackDescription
        {
            get;
            private set;
        }

        /// <summary>
        /// Saves the last track FX.
        /// </summary>
        public void SaveLastSampleTrigger()
        {
            if (this.LastSampleTriggerTrack == null || this.LastSampleTrigger == null) return;

            var attributes = GetAutomationAttributes(this.LastSampleTriggerTrack);
            var sample = this.GetSampleBySampleId(this.LastSampleTrigger.SampleId);

            if (sample != null)
            {
                if (sample.LinkedTrackDescription != this.LastSampleTriggerPrevTrackDescription
                    && sample.LinkedTrackDescription != this.LastSampleTriggerNextTrackDescription)
                {
                    attributes.SampleTriggers.Add(this.LastSampleTrigger);
                }
                else if (sample.LinkedTrackDescription == this.LastSampleTriggerNextTrackDescription)
                {
                    var mixDetails = attributes.GetExtendedMixAttributes(this.LastSampleTriggerNextTrackDescription);
                    if (mixDetails == null)
                    {
                        mixDetails = new ExtendedMixAttributes() { TrackDescription = this.LastSampleTriggerNextTrackDescription };
                        attributes.ExtendedMixes.Add(mixDetails);
                    }
                    mixDetails.SampleTriggers.Add(this.LastSampleTrigger);
                }

                SaveAutomationAttributes(this.LastSampleTriggerTrack);
                if (IsTrackInUse(this.LastSampleTriggerTrack)) ResetTrackSyncPositions();
            }

            this.LastSampleTriggerTrack = null;
            this.LastSampleTrigger = null;
            this.LastSampleTriggerPrevTrackDescription = "";
            this.LastSampleTriggerNextTrackDescription = "";
        }

        /// <summary>
        /// Clears the track FX triggers for the specifed track.
        /// </summary>
        public void ClearSampleTriggers(Track track)
        {
            if (track == null) return;
            var attributes = GetAutomationAttributes(track);

            if (attributes.SampleTriggers.Count == 0) return;

            if (IsTrackInUse(track)) ClearTrackSyncPositions(track);
            attributes.SampleTriggers.Clear();
            SaveAutomationAttributes(track);
            if (IsTrackInUse(track)) ResetTrackSyncPositions();
        }

        /// <summary>
        /// Clears the track FX triggers for the specifed track.
        /// </summary>
        public void ClearSampleTriggers()
        {
            if (this.CurrentTrack == null) return;
            var attributes = GetAutomationAttributes(this.CurrentTrack);

            ClearTrackSyncPositions(this.CurrentTrack);

            attributes.SampleTriggers.Clear();
            var nextTrackSampleTriggers = this.GetNextTrackSampleTriggers();
            nextTrackSampleTriggers.Clear();

            SaveAutomationAttributes(this.CurrentTrack);
            ResetTrackSyncPositions();
        }

        /// <summary>
        /// Removes the previous track FX automation.
        /// </summary>
        public void RemovePreviousSampleTrigger()
        {
            if (this.CurrentTrack == null) return;
            var attributes = GetAutomationAttributes(this.CurrentTrack);

            var trigger = GetPrevSampleTrigger();
            if (trigger == null) return;

            ClearTrackSyncPositions(this.CurrentTrack);

            var nextTrackSampleTriggers = this.GetNextTrackSampleTriggers();

            if (attributes.SampleTriggers.Contains(trigger))
            {
                attributes.SampleTriggers.Remove(trigger);
            }
            else if (nextTrackSampleTriggers.Contains(trigger))
            {
                nextTrackSampleTriggers.Remove(trigger);
            }

            SaveAutomationAttributes(this.CurrentTrack);

            ResetTrackSyncPositions();
        }

        /// <summary>
        /// Stops the automated track FX.
        /// </summary>
        private void StopTrackFxTrigger()
        {
            if (!this.TrackFxAutomationEnabled) return;

            this.StopTrackFxSend();
        }

        /// <summary>
        /// Starts the automated track FX.
        /// </summary>
        private void StartTrackFxTrigger()
        {
            if (!this.TrackFxAutomationEnabled) return;
            if (this.IsTrackFxSending()) return;

            var trigger = this.GetCurrentTrackFxTrigger();
            if (trigger == null) return;

            if (this.TrackSendFxDelayNotes != trigger.DelayNotes) this.TrackSendFxDelayNotes = trigger.DelayNotes;

            this.StartTrackFxSend();
        }

        /// <summary>
        /// Gets the current automated track FX.
        /// </summary>
        /// <returns>The current automated track FX.</returns>
        private TrackFxTrigger GetCurrentTrackFxTrigger()
        {
            if (this.CurrentTrack == null) return null;

            var position = BassHelper.GetTrackPosition(this.CurrentTrack);
            return this.GetAutomationAttributes(this.CurrentTrack)
                .TrackFxTriggers
                .OrderBy(ta => Math.Abs(ta.StartSample - position))
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the current automated track FX.
        /// </summary>
        /// <returns>The current automated track FX.</returns>
        private TrackFxTrigger GetPrevTrackFxTrigger()
        {
            if (this.CurrentTrack == null) return null;

            var position = BassHelper.GetTrackPosition(this.CurrentTrack);
            return this.GetAutomationAttributes(this.CurrentTrack)
                .TrackFxTriggers
                .Where(ta => ta.StartSample <= position)
                .OrderBy(ta => Math.Abs(ta.StartSample - position))
                .FirstOrDefault();
        }

        /// <summary>
        /// Stops the automated track FX.
        /// </summary>
        private void StopSampleTrigger()
        {
            if (!this.SampleAutomationEnabled) return;

            var trigger = this.GetPrevSampleTrigger();
            if (trigger == null) return;

            var sample = this.GetSampleBySampleId(trigger.SampleId);
            if (sample == null) return;

            this.MuteSample(sample);
        }

        /// <summary>
        /// Starts the automated track FX.
        /// </summary>
        private void StartSampleTrigger()
        {
            if (!this.SampleAutomationEnabled) return;

            var trigger = this.GetCurrentSampleTrigger();
            if (trigger == null) return;

            var sample = this.GetSampleBySampleId(trigger.SampleId);
            if (sample == null) return;

            if (this.SamplerDelayNotes != trigger.DelayNotes) this.SamplerDelayNotes = trigger.DelayNotes;

            this.PlaySample(sample);
        }

        /// <summary>
        /// Gets the current automated track FX.
        /// </summary>
        /// <returns>The current automated track FX.</returns>
        private SampleTrigger GetCurrentSampleTrigger()
        {
            if (this.CurrentTrack == null) return null;
            var position = BassHelper.GetTrackPosition(this.CurrentTrack);

            return GetCurrentSampleTriggers()
                .OrderBy(t => Math.Abs(t.StartSample - position))
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the current automated track FX.
        /// </summary>
        /// <returns>The current automated track FX.</returns>
        private SampleTrigger GetPrevSampleTrigger()
        {
            if (this.CurrentTrack == null) return null;

            var position = BassHelper.GetTrackPosition(this.CurrentTrack);

            return GetCurrentSampleTriggers()
                .Where(t => t.StartSample <= position)
                .OrderBy(t => Math.Abs(t.StartSample - position))
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the sample triggers for the current track, including ones specific to the next track
        /// </summary>
        /// <returns>A list of sample triggers, or an empty list if there are none</returns>
        private List<SampleTrigger> GetCurrentSampleTriggers()
        {
            if (this.CurrentTrack == null) return new List<SampleTrigger>();

            var attributes = this.GetAutomationAttributes(this.CurrentTrack);
            if (attributes == null) return new List<SampleTrigger>();

            return attributes
                .SampleTriggers
                .Union(GetNextTrackSampleTriggers())
                .OrderBy(t => t.StartSample)
                .ToList();
        }

        /// <summary>
        /// Gets the sample triggers from the next track for the current track.
        /// </summary>
        /// <returns>A list of the sample triggers</returns>
        private List<SampleTrigger> GetNextTrackSampleTriggers()
        {
            if (this.NextTrack == null) return new List<SampleTrigger>();

            var attributes = this.GetAutomationAttributes(this.CurrentTrack);
            if (attributes == null) return new List<SampleTrigger>();

            var mixDetails = attributes.GetExtendedMixAttributes(this.NextTrack.Description);
            if (mixDetails == null) return new List<SampleTrigger>();

            return mixDetails.SampleTriggers;
        }

        /// <summary>
        /// Gets the automation attributes for a track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>The automation attributes</returns>
        public AutomationAttributes GetAutomationAttributes(Track track)
        {
            if (track == null) return null;
            return GetAutomationAttributes(track.Description);
        }

        /// <summary>
        /// Gets the automation attributes for a track.
        /// </summary>
        /// <param name="trackDescription">The track description.</param>
        /// <returns>The automation attributes</returns>
        public AutomationAttributes GetAutomationAttributes(string trackDescription)
        {
            return AutomationAttributes.GetAutomationAttributes(trackDescription, this.ExtendedAttributeFolder);
        }

        /// <summary>
        /// Saves the automation attributes.
        /// </summary>
        /// <param name="track">The track.</param>
        public void SaveAutomationAttributes(Track track)
        {
            if (track == null) return;
            AutomationAttributes.SaveAutomationAttributes(track, this.ExtendedAttributeFolder);
        }

        /// <summary>
        /// Reloads the automation attributes.
        /// </summary>
        /// <param name="track">The track.</param>
        public void ReloadAutomationAttributes(Track track)
        {
            if (track == null) return;
            AutomationAttributes.SaveAutomationAttributes(track, this.ExtendedAttributeFolder);
        }

        /// <summary>
        /// Sets the automation sync positions.
        /// </summary>
        /// <param name="track">The track.</param>
        private void SetAutomationSyncPositions(Track track)
        {
            if (track != this.CurrentTrack) return;

            foreach (var trigger in this.GetAutomationAttributes(track).TrackFxTriggers)
            {
                trigger.StartSample = track.SecondsToSamples(trigger.Start);
                trigger.EndSample = track.SecondsToSamples(trigger.Start + trigger.Length);
                trigger.StartSyncId = SetTrackSync(track, trigger.StartSample, SyncType.StartTrackFxTrigger);
                trigger.EndSyncId = SetTrackSync(track, trigger.EndSample, SyncType.EndTrackFxTrigger);
            }

            foreach (var trigger in this.GetCurrentSampleTriggers())
            {
                trigger.StartSample = track.SecondsToSamples(trigger.Start);
                trigger.EndSample = track.SecondsToSamples(trigger.Start + trigger.Length);
                trigger.StartSyncId = SetTrackSync(track, trigger.StartSample, SyncType.StartSampleTrigger);
                trigger.EndSyncId = SetTrackSync(track, trigger.EndSample, SyncType.EndSampleTrigger);
            }
        }

        /// <summary>
        /// Clears the automation sync positions.
        /// </summary>
        /// <param name="track">The track.</param>
        private void ClearAutomationSyncPositions(Track track)
        {
            foreach (var trigger in this.GetAutomationAttributes(track).TrackFxTriggers)
            {
                if (trigger.StartSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, trigger.StartSyncId);
                    trigger.StartSyncId = int.MinValue;
                }
                if (trigger.EndSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, trigger.EndSyncId);
                    trigger.EndSyncId = int.MinValue;
                }
            }

            foreach (var trigger in this.GetCurrentSampleTriggers())
            {
                if (trigger.StartSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, trigger.StartSyncId);
                    trigger.StartSyncId = int.MinValue;
                }
                if (trigger.EndSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, trigger.EndSyncId);
                    trigger.EndSyncId = int.MinValue;
                }
            }
        }

        private void StartRecordingSampleTrigger(Sample sample)
        {
            if (this.CurrentTrack == null) return;
            if (sample == null) return;

            this.LastSampleTriggerTrack = this.CurrentTrack;
            this.LastSampleTriggerNextTrackDescription = this.NextTrack == null ? "" : this.NextTrack.Description;
            this.LastSampleTriggerPrevTrackDescription = this.PreviousTrack == null ? "" : this.PreviousTrack.Description;

            var position = BassHelper.GetTrackPosition(this.LastSampleTriggerTrack);

            this.LastSampleTrigger = new SampleTrigger();
            this.LastSampleTrigger.Start = this.LastSampleTriggerTrack.SamplesToSeconds(position);
            this.LastSampleTrigger.DelayNotes = this.SamplerDelayNotes;
            this.LastSampleTrigger.SampleId = sample.SampleId;
        }

        private void StopRecordingSampleTrigger()
        {
            if (this.CurrentTrack == null || this.LastSampleTrigger == null || this.LastSampleTriggerTrack == null) return;

            var position = BassHelper.GetTrackPosition(this.LastSampleTriggerTrack);
            var positionSeconds = this.LastSampleTriggerTrack.SamplesToSeconds(position);
            var length = positionSeconds - this.LastSampleTrigger.Start;

            if (length <= 0 || position >= this.LastSampleTriggerTrack.FadeOutStart)
            {
                length = this.LastSampleTriggerTrack.SamplesToSeconds(this.LastSampleTriggerTrack.FadeOutStart) - this.LastSampleTrigger.Start;
            }

            this.LastSampleTrigger.Length = length;
        }

        public void StopRecordingManualExtendedMix()
        {
            StopRecordingManualExtendedMix(false);
        }

        public void StopRecordingManualExtendedMix(bool powerDownAfterFade)
        {
            if (this.CurrentTrack == null) return;
            if (this.PreviousTrack == null) return;
            if (!this.ManualFadeOut) return;
            if (this.PreviousManaulExtendedFadeType == ExtendedFadeType.PowerDown) return;

            CreateLastExtendedMixAttributes();

            var attributes = this.LastExtendedMixAttributes;
            attributes.FadeEnd = BassHelper.GetTrackPosition(this.PreviousTrack);
            attributes.FadeLength = this.GetAdjustedPositionSeconds(this.CurrentTrack);
            attributes.FadeEndLoop = this.PreviousTrack.CurrentEndLoop;
            attributes.FadeEndVolume = Convert.ToSingle(BassHelper.GetTrackVolume(this.PreviousTrack) / 100);
            attributes.PowerDownAfterFade = powerDownAfterFade;

            if (this.ManualFadeOut)
                RaiseOnEndFadeIn();
        }

        public void StopRecordingAutoExtendedMix()
        {
            if (this.CurrentTrack == null) return;
            if (this.PreviousTrack == null) return;
            if (!this.ManualFadeOut) return;
            if (this.PreviousManaulExtendedFadeType != ExtendedFadeType.PowerDown
                && this.PreviousManaulExtendedFadeType != ExtendedFadeType.Cut) return;

            CreateLastExtendedMixAttributes();

            var attributes = this.LastExtendedMixAttributes;
            attributes.FadeEnd = this.PreviousTrack.FadeOutEnd;
            attributes.FadeLength = this.PreviousTrack.FadeOutLengthSeconds;
            attributes.FadeEndLoop = this.PreviousTrack.EndLoopCount;
            attributes.FadeEndVolume = this.PreviousTrack.FadeInEndVolume;
            attributes.PowerDownAfterFade = false;

            //if (this.ManualFadeOut)
            //    RaiseOnEndFadeIn();
        }

        private void CreateLastExtendedMixAttributes()
        {
            this.LastExtendedMixTrack = this.PreviousTrack;
            this.LastExtendedMixAttributes = new ExtendedMixAttributes()
            {
                TrackDescription = this.CurrentTrack.Description,
                ExtendedFadeType = this.PreviousManaulExtendedFadeType
            };
        }

        private ExtendedMixAttributes LastExtendedMixAttributes { get; set; }

        private Track LastExtendedMixTrack { get; set; }

        public void SaveExtendedMix()
        {
            if (this.LastExtendedMixTrack == null || this.LastExtendedMixAttributes == null) return;

            var attributes = GetAutomationAttributes(this.LastExtendedMixTrack);
            if (attributes == null) return;

            var existingMix = attributes.GetExtendedMixAttributes(this.LastExtendedMixAttributes.TrackDescription);
            if (existingMix != null)
            {
                this.LastExtendedMixAttributes.SampleTriggers.AddRange(existingMix.SampleTriggers);
                attributes.RemoveExtendedMixAttributes(this.LastExtendedMixAttributes.TrackDescription);
            }

            attributes.ExtendedMixes.Add(this.LastExtendedMixAttributes);
            SaveAutomationAttributes(this.LastExtendedMixTrack);

            this.LastExtendedMixTrack = null;
            this.LastExtendedMixAttributes = null;
        }

        public void ClearExtendedMix()
        {
            if (this.CurrentTrack == null || this.PreviousTrack == null) return;

            var attributes = GetAutomationAttributes(this.PreviousTrack);
            if (attributes == null) return;

            attributes.RemoveExtendedMixAttributes(this.CurrentTrack.Description);
            SaveAutomationAttributes(this.PreviousTrack);

            this.LastExtendedMixTrack = null;
            this.LastExtendedMixAttributes = null;
        }

        public ExtendedFadeType GetExtendedFadeType(Track fromTrack, Track toTrack)
        {
            if (fromTrack == null || toTrack == null) return ExtendedFadeType.Default;

            var hasExtendedMix = this.HasExtendedMixAttributes(fromTrack, toTrack);
            if (hasExtendedMix)
            {
                var mixAttributes = this.GetExtendedMixAttributes(fromTrack, toTrack);
                return mixAttributes.ExtendedFadeType;
            }

            return ExtendedFadeType.Default;
        }

        public double GetExtendedFadeOutLength(Track fromTrack, Track toTrack)
        {
            if (fromTrack == null) return 0;

            var attributes = GetExtendedMixAttributes(fromTrack, toTrack);
            if (attributes == null)
            {
                var fadeOutLength = fromTrack.FullEndLoopLengthSeconds;
                if (fadeOutLength == 0)
                    fadeOutLength = BassHelper.GetDefaultLoopLength(fromTrack.EndBpm);

                if (toTrack != null)
                    fadeOutLength = BassHelper.GetLengthAdjustedToMatchAnotherTrack(fromTrack, toTrack, fadeOutLength);

                return fadeOutLength;
            }

            return attributes.FadeLength;
        }

        public ExtendedFadeType PreviousManaulExtendedFadeType { get; set; }

        public ExtendedFadeType CurrentManualExtendedFadeType { get; set; }

        private ExtendedFadeType GetCurrentExtendedFadeType()
        {
            if (this.HasExtendedMixAttributes())
            {
                var mixAttributes = this.GetExtendedMixAttributes();
                return mixAttributes.ExtendedFadeType;
            }

            return ExtendedFadeType.Default;
        }

        public void ForceFadeNow(ForceFadeType fadeType)
        {
            if (this.PlayState != PlayState.Playing)
                return;

            if (this.NextTrack == null || this.CurrentTrack == null)
                return;

            if (fadeType == ForceFadeType.SkipToEnd)
            {
                BassHelper.SetTrackPosition(this.CurrentTrack, this.CurrentTrack.FadeOutStart - this.CurrentTrack.SecondsToSamples(0.05M));
            }
            else
            {
                this.CurrentForceFadeType = fadeType;
                this.IsForceFadeNowMode = true;

                var length = 0D;
                if (fadeType == ForceFadeType.Cut)
                {
                    length = GetCutFadeLength(this.CurrentTrack);
                }
                else if (fadeType == ForceFadeType.PowerDown)
                {
                    length = this.GetPowerDownFadeLength(this.CurrentTrack);
                    this.CurrentTrack.PowerDownOnEnd = true;
                }
                else if (fadeType == ForceFadeType.QuickFade)
                {
                    length = GetCutFadeLength(this.CurrentTrack);
                }

                if (this.CurrentTrack.FadeOutLengthSeconds < length)
                    length = this.CurrentTrack.FadeOutLengthSeconds;

                this.CurrentTrack.EndLoopCount = 0;

                BassMix.BASS_Mixer_ChannelRemoveSync(this.CurrentTrack.Channel, this.CurrentTrack.FadeOutStartSyncId);
                this.CurrentTrack.FadeOutStartSyncId = int.MinValue;
                BassMix.BASS_Mixer_ChannelRemoveSync(this.CurrentTrack.Channel, this.CurrentTrack.FadeOutEndSyncId);
                this.CurrentTrack.FadeOutEndSyncId = int.MinValue;

                var position = BassHelper.GetTrackPosition(this.CurrentTrack);
                this.CurrentTrack.FadeOutStart = position + this.CurrentTrack.SecondsToSamples(0.05M);
                SetTrackSync(this.CurrentTrack, this.CurrentTrack.FadeOutStart, SyncType.StartFadeOut);

                this.CurrentTrack.FadeOutEnd = this.CurrentTrack.FadeOutStart + this.CurrentTrack.SecondsToSamples(length);
                SetTrackSync(this.CurrentTrack, this.CurrentTrack.FadeOutEnd, SyncType.EndFadeOut);
            }
        }

        public ForceFadeType CurrentForceFadeType
        {
            get;
            set;
        }

        public bool IsForceFadeNowMode
        {
            get;
            set;
        }
    }

    public enum ForceFadeType
    {
        PowerDown,
        SkipToEnd,
        Cut,
        QuickFade
    }
}