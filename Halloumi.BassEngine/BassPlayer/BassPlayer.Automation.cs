using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Un4seen.Bass.AddOn.Mix;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public partial class BassPlayer
    {
        public event EventHandler OnManualMixVolumeChanged;

        /// <summary>
        ///     Gets the last track FX attributes.
        /// </summary>
        public TrackFXTrigger LastTrackFxTrigger { get; private set; }

        /// <summary>
        ///     Gets the last track FX track.
        /// </summary>
        public Track LastTrackFxTriggerTrack { get; private set; }

        /// <summary>
        ///     Gets the last track FX attributes.
        /// </summary>
        public SampleTrigger LastSampleTrigger { get; private set; }

        /// <summary>
        ///     Gets the last track FX track.
        /// </summary>
        public Track LastSampleTriggerTrack { get; private set; }

        public string LastSampleTriggerPrevTrackDescription { get; private set; }

        public string LastSampleTriggerNextTrackDescription { get; private set; }

        private ExtendedMixAttributes LastExtendedMixAttributes { get; set; }

        private Track LastExtendedMixTrack { get; set; }

        public ExtendedFadeType PreviousManaulExtendedFadeType { get; set; }

        public ExtendedFadeType CurrentManualExtendedFadeType { get; set; }

        public ForceFadeType CurrentForceFadeType { get; set; }

        public bool IsForceFadeNowMode { get; set; }

        private void InitialiseManualMixer()
        {
            IsManualMixMode = false;
            PreviousManaulExtendedFadeType = ExtendedFadeType.Default;
            CurrentManualExtendedFadeType = ExtendedFadeType.Default;
        }

        /// <summary>
        ///     Saves the last track FX.
        /// </summary>
        public void SaveLastTrackFxTrigger()
        {
            if (LastTrackFxTriggerTrack == null || LastTrackFxTrigger == null) return;

            var attributes = AutomationAttributesHelper.GetAutomationAttributes(LastTrackFxTriggerTrack.Description);
            attributes.TrackFXTriggers.Add(LastTrackFxTrigger);

            AutomationAttributesHelper.SaveAutomationAttributes(LastTrackFxTriggerTrack.Description, attributes);

            if (IsTrackInUse(LastTrackFxTriggerTrack)) ResetTrackSyncPositions();

            LastTrackFxTriggerTrack = null;
            LastTrackFxTrigger = null;
        }

        /// <summary>
        ///     Clears the track FX triggers for the specified track.
        /// </summary>
        public void ClearTrackFxTriggers(Track track)
        {
            if (track == null) return;
            var attributes = AutomationAttributesHelper.GetAutomationAttributes(track.Description);

            if (attributes.TrackFXTriggers.Count == 0) return;

            if (IsTrackInUse(track)) ClearTrackSyncPositions(track);
            attributes.TrackFXTriggers.Clear();
            
            AutomationAttributesHelper.SaveAutomationAttributes(track.Description, attributes);

            if (IsTrackInUse(track)) ResetTrackSyncPositions();
        }

        /// <summary>
        ///     Removes the previous track FX automation.
        /// </summary>
        public void RemovePreviousTrackFxTrigger()
        {
            if (CurrentTrack == null) return;
            var attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);
            if (attributes.TrackFXTriggers.Count == 0) return;

            var trigger = GetPrevTrackFxTrigger(attributes);
            if (trigger == null) return;

            ClearTrackSyncPositions(CurrentTrack);
            attributes.TrackFXTriggers.Remove(trigger);

            AutomationAttributesHelper.SaveAutomationAttributes(CurrentTrack.Description, attributes);

            ResetTrackSyncPositions();
        }

        /// <summary>
        ///     Saves the last track FX.
        /// </summary>
        public void SaveLastSampleTrigger()
        {
            if (LastSampleTriggerTrack == null || LastSampleTrigger == null) return;

            var attributes = AutomationAttributesHelper.GetAutomationAttributes(LastSampleTriggerTrack.Description);
            var sample = GetSampleBySampleId(LastSampleTrigger.SampleId);

            if (sample != null)
            {
                if (sample.LinkedTrackDescription != LastSampleTriggerPrevTrackDescription
                    && sample.LinkedTrackDescription != LastSampleTriggerNextTrackDescription)
                {
                    attributes.SampleTriggers.Add(LastSampleTrigger);
                }
                else if (sample.LinkedTrackDescription == LastSampleTriggerNextTrackDescription)
                {
                    var mixDetails = attributes.GetExtendedMixAttributes(LastSampleTriggerNextTrackDescription);
                    if (mixDetails == null)
                    {
                        mixDetails = new ExtendedMixAttributes
                        {
                            TrackDescription = LastSampleTriggerNextTrackDescription
                        };
                        attributes.ExtendedMixes.Add(mixDetails);
                    }
                    mixDetails.SampleTriggers.Add(LastSampleTrigger);
                }

                AutomationAttributesHelper.SaveAutomationAttributes(LastSampleTriggerTrack.Description, attributes);
                
                if (IsTrackInUse(LastSampleTriggerTrack)) ResetTrackSyncPositions();
            }

            LastSampleTriggerTrack = null;
            LastSampleTrigger = null;
            LastSampleTriggerPrevTrackDescription = "";
            LastSampleTriggerNextTrackDescription = "";
        }


        /// <summary>
        ///     Clears the track FX triggers for the specified track.
        /// </summary>
        public void ClearSampleTriggers()
        {
            if (CurrentTrack == null) return;
            var attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);

            ClearTrackSyncPositions(CurrentTrack);

            attributes.SampleTriggers.Clear();

            var mixDetails = attributes.GetExtendedMixAttributes(NextTrack.Description);
            mixDetails?.SampleTriggers.Clear();

            AutomationAttributesHelper.SaveAutomationAttributes(CurrentTrack.Description, attributes);

            ResetTrackSyncPositions();
        }

        /// <summary>
        ///     Removes the previous track FX automation.
        /// </summary>
        public void RemovePreviousSampleTrigger()
        {
            if (CurrentTrack == null) return;
            var attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);

            var trigger = GetPrevSampleTrigger(attributes);
            if (trigger == null) return;

            ClearTrackSyncPositions(CurrentTrack);

            var nextTrackSampleTriggers = GetNextTrackSampleTriggers(attributes);

            if (attributes.SampleTriggers.Contains(trigger))
            {
                attributes.SampleTriggers.Remove(trigger);
            }
            else if (nextTrackSampleTriggers.Contains(trigger))
            {
                nextTrackSampleTriggers.Remove(trigger);
            }

            AutomationAttributesHelper.SaveAutomationAttributes(CurrentTrack.Description, attributes);

            ResetTrackSyncPositions();
        }

        /// <summary>
        ///     Stops the automated track FX.
        /// </summary>
        private void StopTrackFxTrigger()
        {
            if (!TrackFxAutomationEnabled) return;

            StopTrackFxSend();
        }

        /// <summary>
        ///     Starts the automated track FX.
        /// </summary>
        private void StartTrackFxTrigger()
        {
            if (!TrackFxAutomationEnabled) return;
            if (IsTrackFxSending()) return;

            var trigger = GetCurrentTrackFxTrigger();
            if (trigger == null) return;

            if (TrackSendFxDelayNotes != trigger.DelayNotes) TrackSendFxDelayNotes = trigger.DelayNotes;

            StartTrackFxSend();
        }

        /// <summary>
        ///     Gets the current automated track FX.
        /// </summary>
        /// <returns>The current automated track FX.</returns>
        private TrackFXTrigger GetCurrentTrackFxTrigger(AutomationAttributes attributes = null)
        {
            if (CurrentTrack == null) return null;

            if (attributes == null)
                attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);

            var position = AudioStreamHelper.GetPosition(CurrentTrack);
            return attributes
                .TrackFXTriggers
                .OrderBy(ta => Math.Abs(ta.StartSample - position))
                .FirstOrDefault();
        }

        /// <summary>
        ///     Gets the current automated track FX.
        /// </summary>
        /// <returns>The current automated track FX.</returns>
        private TrackFXTrigger GetPrevTrackFxTrigger(AutomationAttributes attributes = null)
        {
            if (CurrentTrack == null) return null;

            if (attributes == null)
                attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);

            var position = AudioStreamHelper.GetPosition(CurrentTrack);
            return attributes
                .TrackFXTriggers
                .Where(ta => ta.StartSample <= position)
                .OrderBy(ta => Math.Abs(ta.StartSample - position))
                .FirstOrDefault();
        }

        /// <summary>
        ///     Stops the automated track FX.
        /// </summary>
        private void StopSampleTrigger()
        {
            if (!SampleAutomationEnabled) return;

            var trigger = GetPrevSampleTrigger();
            if (trigger == null) return;

            var sample = GetSampleBySampleId(trigger.SampleId);
            if (sample == null) return;

            PauseSample(sample);
        }

        /// <summary>
        ///     Starts the automated track FX.
        /// </summary>
        private void StartSampleTrigger()
        {
            if (!SampleAutomationEnabled) return;

            var trigger = GetCurrentSampleTrigger();
            if (trigger == null) return;

            var sample = GetSampleBySampleId(trigger.SampleId);
            if (sample == null) return;

            if (SamplerDelayNotes != trigger.DelayNotes) SamplerDelayNotes = trigger.DelayNotes;

            PlaySample(sample);
        }

        /// <summary>
        ///     Gets the current automated track FX.
        /// </summary>
        /// <returns>The current automated track FX.</returns>
        private SampleTrigger GetCurrentSampleTrigger(AutomationAttributes attributes = null)
        {
            if (CurrentTrack == null) return null;
            if (attributes == null)
                attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);

            var position = AudioStreamHelper.GetPosition(CurrentTrack);

            return GetCurrentSampleTriggers(attributes)
                .OrderBy(t => Math.Abs(t.StartSample - position))
                .FirstOrDefault();
        }

        /// <summary>
        ///     Gets the current automated track FX.
        /// </summary>
        /// <returns>The current automated track FX.</returns>
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
        ///     Gets the sample triggers for the current track, including ones specific to the next track
        /// </summary>
        /// <returns>A list of sample triggers, or an empty list if there are none</returns>
        private IEnumerable<SampleTrigger> GetCurrentSampleTriggers(AutomationAttributes attributes = null)
        {
            if (CurrentTrack == null) return new List<SampleTrigger>();

            if(attributes == null)
                attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);

            if (attributes == null) return new List<SampleTrigger>();

            return attributes
                .SampleTriggers
                .Union(GetNextTrackSampleTriggers())
                .OrderBy(t => t.StartSample)
                .ToList();
        }

        /// <summary>
        ///     Gets the sample triggers from the next track for the current track.
        /// </summary>
        /// <returns>A list of the sample triggers</returns>
        private List<SampleTrigger> GetNextTrackSampleTriggers(AutomationAttributes attributes = null)
        {
            if (NextTrack == null) return new List<SampleTrigger>();

            if (attributes == null)
                attributes = AutomationAttributesHelper.GetAutomationAttributes(CurrentTrack.Description);

            if (attributes == null) return new List<SampleTrigger>();

            var mixDetails = attributes.GetExtendedMixAttributes(NextTrack.Description);
            return mixDetails == null ? new List<SampleTrigger>() : mixDetails.SampleTriggers;
        }

        /// <summary>
        ///     Sets the automation sync positions.
        /// </summary>
        /// <param name="track">The track.</param>
        private void SetAutomationSyncPositions(Track track)
        {
            if (track != CurrentTrack) return;

            foreach (var trigger in AutomationAttributesHelper.GetAutomationAttributes(track.Description).TrackFXTriggers)
            {
                trigger.StartSample = track.SecondsToSamples(trigger.Start);
                trigger.EndSample = track.SecondsToSamples(trigger.Start + trigger.Length);
                trigger.StartSyncId = SetTrackSync(track, trigger.StartSample, SyncType.StartTrackFxTrigger);
                trigger.EndSyncId = SetTrackSync(track, trigger.EndSample, SyncType.EndTrackFxTrigger);
            }

            foreach (var trigger in GetCurrentSampleTriggers())
            {
                trigger.StartSample = track.SecondsToSamples(trigger.Start);
                trigger.EndSample = track.SecondsToSamples(trigger.Start + trigger.Length);
                trigger.StartSyncId = SetTrackSync(track, trigger.StartSample, SyncType.StartSampleTrigger);
                trigger.EndSyncId = SetTrackSync(track, trigger.EndSample, SyncType.EndSampleTrigger);
            }
        }

        /// <summary>
        ///     Clears the automation sync positions.
        /// </summary>
        /// <param name="track">The track.</param>
        private void ClearAutomationSyncPositions(AudioStream track)
        {
            foreach (var trigger in AutomationAttributesHelper.GetAutomationAttributes(track.Description).TrackFXTriggers)
            {
                if (trigger.StartSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, trigger.StartSyncId);
                    trigger.StartSyncId = int.MinValue;
                }
                if (trigger.EndSyncId == int.MinValue) continue;

                BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, trigger.EndSyncId);
                trigger.EndSyncId = int.MinValue;
            }

            foreach (var trigger in GetCurrentSampleTriggers())
            {
                if (trigger.StartSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, trigger.StartSyncId);
                    trigger.StartSyncId = int.MinValue;
                }

                if (trigger.EndSyncId == int.MinValue) continue;

                BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, trigger.EndSyncId);
                trigger.EndSyncId = int.MinValue;
            }
        }

        private void StartRecordingSampleTrigger(Sample sample)
        {
            if (CurrentTrack == null) return;
            if (sample == null) return;

            LastSampleTriggerTrack = CurrentTrack;
            LastSampleTriggerNextTrackDescription = NextTrack == null ? "" : NextTrack.Description;
            LastSampleTriggerPrevTrackDescription = PreviousTrack == null ? "" : PreviousTrack.Description;

            var position = AudioStreamHelper.GetPosition(LastSampleTriggerTrack);

            LastSampleTrigger = new SampleTrigger
            {
                Start = LastSampleTriggerTrack.SamplesToSeconds(position),
                DelayNotes = SamplerDelayNotes,
                SampleId = sample.SampleId
            };
        }

        private void StopRecordingSampleTrigger()
        {
            if (CurrentTrack == null || LastSampleTrigger == null || LastSampleTriggerTrack == null) return;

            var position = AudioStreamHelper.GetPosition(LastSampleTriggerTrack);
            var positionSeconds = LastSampleTriggerTrack.SamplesToSeconds(position);
            var length = positionSeconds - LastSampleTrigger.Start;

            if (length <= 0 || position >= LastSampleTriggerTrack.FadeOutStart)
            {
                length = LastSampleTriggerTrack.SamplesToSeconds(LastSampleTriggerTrack.FadeOutStart) -
                         LastSampleTrigger.Start;
            }

            LastSampleTrigger.Length = length;
        }

        public void StopRecordingManualExtendedMix()
        {
            StopRecordingManualExtendedMix(false);
        }

        public void StopRecordingManualExtendedMix(bool powerDownAfterFade)
        {
            if (CurrentTrack == null) return;
            if (PreviousTrack == null) return;
            if (!IsManualMixMode) return;
            if (PreviousManaulExtendedFadeType == ExtendedFadeType.PowerDown) return;

            CreateLastExtendedMixAttributes();

            var attributes = LastExtendedMixAttributes;
            attributes.FadeEnd = AudioStreamHelper.GetPosition(PreviousTrack);
            attributes.FadeLength = GetAdjustedPositionSeconds(CurrentTrack);
            attributes.FadeEndLoop = PreviousTrack.CurrentEndLoop;
            attributes.FadeEndVolume = Convert.ToSingle(AudioStreamHelper.GetVolume(PreviousTrack)/100);
            attributes.PowerDownAfterFade = powerDownAfterFade;

            if (IsManualMixMode)
                RaiseOnEndFadeIn();
        }

        public void StopRecordingAutoExtendedMix()
        {
            if (CurrentTrack == null) return;
            if (PreviousTrack == null) return;
            if (!IsManualMixMode) return;
            if (PreviousManaulExtendedFadeType == ExtendedFadeType.Default) return;

            CreateLastExtendedMixAttributes();

            var attributes = LastExtendedMixAttributes;
            attributes.FadeEnd = PreviousTrack.FadeOutEnd;
            attributes.FadeLength = PreviousTrack.FadeOutLengthSeconds;
            attributes.FadeEndLoop = PreviousTrack.EndLoopCount;
            attributes.FadeEndVolume = PreviousTrack.FadeInEndVolume;
            attributes.PowerDownAfterFade = false;
        }

        private void CreateLastExtendedMixAttributes()
        {
            LastExtendedMixTrack = PreviousTrack;
            LastExtendedMixAttributes = new ExtendedMixAttributes
            {
                TrackDescription = CurrentTrack.Description,
                ExtendedFadeType = PreviousManaulExtendedFadeType
            };
        }

        public void SaveExtendedMix()
        {
            if (LastExtendedMixTrack == null || LastExtendedMixAttributes == null) return;

            var attributes = AutomationAttributesHelper.GetAutomationAttributes(LastExtendedMixTrack.Description);
            if (attributes == null) return;

            var existingMix = attributes.GetExtendedMixAttributes(LastExtendedMixAttributes.TrackDescription);
            if (existingMix != null)
            {
                LastExtendedMixAttributes.SampleTriggers.AddRange(existingMix.SampleTriggers);
                attributes.RemoveExtendedMixAttributes(LastExtendedMixAttributes.TrackDescription);
            }

            attributes.ExtendedMixes.Add(LastExtendedMixAttributes);
            AutomationAttributesHelper.SaveAutomationAttributes(LastExtendedMixTrack.Description, attributes);

            LastExtendedMixTrack = null;
            LastExtendedMixAttributes = null;
        }

        public void ClearExtendedMix()
        {
            if (CurrentTrack == null || PreviousTrack == null) return;

            var attributes = AutomationAttributesHelper.GetAutomationAttributes(PreviousTrack.Description);
            if (attributes == null) return;

            attributes.RemoveExtendedMixAttributes(CurrentTrack.Description);
            
            AutomationAttributesHelper.SaveAutomationAttributes(PreviousTrack.Description, attributes);

            LastExtendedMixTrack = null;
            LastExtendedMixAttributes = null;
        }

        public ExtendedFadeType GetExtendedFadeType(Track fromTrack, Track toTrack)
        {
            if (fromTrack == null || toTrack == null) return ExtendedFadeType.Default;

            var hasExtendedMix = HasExtendedMixAttributes(fromTrack, toTrack);
            if (!hasExtendedMix) return ExtendedFadeType.Default;

            var mixAttributes = GetExtendedMixAttributes(fromTrack, toTrack);
            return mixAttributes.ExtendedFadeType;
        }

        public double GetExtendedFadeOutLength(Track fromTrack, Track toTrack)
        {
            if (fromTrack == null) return 0;

            var attributes = GetExtendedMixAttributes(fromTrack, toTrack);

            if (attributes != null) return attributes.FadeLength;
            var fadeOutLength = fromTrack.FullEndLoopLengthSeconds;

            if (fadeOutLength == 0)
                fadeOutLength = BpmHelper.GetDefaultLoopLength(fromTrack.EndBpm);

            if (toTrack != null)
                fadeOutLength = BpmHelper.GetLengthAdjustedToMatchAnotherTrack(fromTrack, toTrack, fadeOutLength);

            return fadeOutLength;
        }

        private ExtendedFadeType GetCurrentExtendedFadeType()
        {
            if (!HasExtendedMixAttributes()) return ExtendedFadeType.Default;

            var mixAttributes = GetExtendedMixAttributes();
            return mixAttributes.ExtendedFadeType;
        }

        public void SetManualMixVolume(decimal value)
        {
            value = 100M - value;

            var track = PreviousTrack;
            if (track == null) return;

            var range = (decimal)DefaultFadeOutStartVolume;
            var volume = (range * (value / 100));
            volume = (decimal)DefaultFadeOutEndVolume + volume;

            AudioStreamHelper.SetVolume(track, volume);

            OnManualMixVolumeChanged?.Invoke(CurrentTrack, EventArgs.Empty);
        }

        public decimal GetManualMixVolume()
        {
            if (PreviousTrack == null) return 100M;

            var volume = AudioStreamHelper.GetVolume(PreviousTrack) - (decimal)DefaultFadeOutEndVolume;
            volume = volume / (decimal)DefaultFadeOutStartVolume;
            volume = volume * 100;
            return 100 - volume;
        }

        /// <summary>
        /// Makes the power off noise on a track
        /// </summary>
        public void PowerOffPreviousTrack()
        {
            if (PreviousTrack == null) return;
            if (PlayState != PlayState.Playing) return;
            if (!IsTrackInUse(PreviousTrack)) return;

            AudioStreamHelper.PowerDown(PreviousTrack);
            StopRecordingManualExtendedMix(true);
        }

        /// <summary>
        /// Pauses the track.
        /// </summary>
        public void PausePreviousTrack()
        {
            if (PreviousTrack == null) return;
            if (!IsTrackInUse(PreviousTrack)) return;
            if (!AudioStreamHelper.IsPlaying(PreviousTrack)) return;

            StopRecordingManualExtendedMix();
            AudioStreamHelper.SmoothPause(PreviousTrack);
        }

        /// <summary>
        /// Makes the power off noise on a track
        /// </summary>
        public void PowerOffCurrentTrack()
        {
            StopSamples();
            if (CurrentTrack == null) return;
            if (PlayState != PlayState.Playing) return;
            if (!IsTrackInUse(CurrentTrack)) return;
            AudioStreamHelper.PowerDown(CurrentTrack);
        }

        public void ForceFadeNow(ForceFadeType fadeType)
        {
            if (PlayState != PlayState.Playing)
                return;

            if (NextTrack == null || CurrentTrack == null)
                return;

            if (fadeType == ForceFadeType.SkipToEnd)
            {
                AudioStreamHelper.SetPosition(CurrentTrack,
                    CurrentTrack.FadeOutStart - CurrentTrack.SecondsToSamples(0.01M));
            }
            else
            {
                CurrentForceFadeType = fadeType;
                IsForceFadeNowMode = true;

                var length = 0D;
                switch (fadeType)
                {
                    case ForceFadeType.Cut:
                        length = GetCutFadeLength(CurrentTrack);
                        break;
                    case ForceFadeType.PowerDown:
                        length = GetPowerDownFadeLength(CurrentTrack);
                        CurrentTrack.PowerDownOnEnd = true;
                        break;
                    case ForceFadeType.QuickFade:
                        length = GetCutFadeLength(CurrentTrack);
                        break;
                    case ForceFadeType.SkipToEnd:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(fadeType), fadeType, null);
                }

                if (CurrentTrack.FadeOutLengthSeconds < length)
                    length = CurrentTrack.FadeOutLengthSeconds;

                CurrentTrack.EndLoopCount = 0;

                BassMix.BASS_Mixer_ChannelRemoveSync(CurrentTrack.Channel, CurrentTrack.FadeOutStartSyncId);
                CurrentTrack.FadeOutStartSyncId = int.MinValue;
                BassMix.BASS_Mixer_ChannelRemoveSync(CurrentTrack.Channel, CurrentTrack.FadeOutEndSyncId);
                CurrentTrack.FadeOutEndSyncId = int.MinValue;

                var position = AudioStreamHelper.GetPosition(CurrentTrack);
                CurrentTrack.FadeOutStart = position + CurrentTrack.SecondsToSamples(0.05M);
                SetTrackSync(CurrentTrack, CurrentTrack.FadeOutStart, SyncType.StartFadeOut);

                CurrentTrack.FadeOutEnd = CurrentTrack.FadeOutStart + CurrentTrack.SecondsToSamples(length);
                SetTrackSync(CurrentTrack, CurrentTrack.FadeOutEnd, SyncType.EndFadeOut);
            }
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