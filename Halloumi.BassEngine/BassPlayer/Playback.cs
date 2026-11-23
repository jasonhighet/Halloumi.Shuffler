using System;
using System.Collections.Generic;
using System.Threading;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Mixing;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     Plays the tracks in the slots and moves from one to the next: queueing, play/pause,
    ///     seeking, the fade-in / fade-out transitions (driven by syncs on the tracks) and "fade now".
    ///     Manual mixing is in <see cref="ManualMixer" />.
    ///     <para>
    ///         Threads and locking. The public methods run on the UI (or MIDI) thread. The sync
    ///         handlers run on the BASS mixer thread, at the exact sample the sync is set for, so the
    ///         transitions stay on the beat. BASS holds its own lock on the mixer while a sync handler
    ///         runs, so a thread calling into BASS can be waiting for the handler to finish.
    ///         <list type="bullet">
    ///             <item>
    ///                 <c>_slotLock</c> covers changes to the slots and to the tracks' positions, volumes and
    ///                 syncs, so a transition never sees a half-made change. Hold it only for quick work:
    ///                 never while decoding audio, and never while raising an event.
    ///             </item>
    ///             <item>
    ///                 The mixer thread never waits for <c>_slotLock</c>: the thread holding it may be
    ///                 inside a BASS call that is waiting for the mixer thread (a deadlock). Sync handlers
    ///                 go through <see cref="RunSyncHandler" />, which takes the lock if it is free and
    ///                 otherwise runs the work on <c>_background</c> a few milliseconds later.
    ///             </item>
    ///             <item>
    ///                 Audio is decoded before the lock is taken (in QueueTrack and PreloadTrack).
    ///             </item>
    ///             <item>
    ///                 Sync handlers hand anything that can wait (raising events, freeing audio) to
    ///                 <c>_background</c>, which runs it in order on a worker thread.
    ///             </item>
    ///         </list>
    ///         Lock order: <c>_slotLock</c>, then the deck's, a track's syncs', or a track's own lock.
    ///         Never take <c>_slotLock</c> while holding one of those. A track's syncs are only changed
    ///         with <c>_slotLock</c> held (except the raw-loop track's, which the mixer thread never changes).
    ///     </para>
    /// </summary>
    internal class Playback
    {
        private readonly object _slotLock = new object();

        private readonly MixerGraph _graph;
        private readonly TrackDeck _deck;
        private readonly TrackLoader _loader;
        private readonly SyncDispatcher _syncs;
        private readonly AutomationPlayer _automation;
        private readonly Sampler _sampler;
        private readonly TrackFxSend _fxSend;
        private readonly ManualMixer _manualMixer;
        private readonly BackgroundQueue _background;

        public Playback(MixerGraph graph, TrackDeck deck, TrackLoader loader, SyncDispatcher syncs,
            AutomationPlayer automation, Sampler sampler, TrackFxSend fxSend, ManualMixer manualMixer,
            BackgroundQueue background)
        {
            _graph = graph;
            _deck = deck;
            _loader = loader;
            _syncs = syncs;
            _automation = automation;
            _sampler = sampler;
            _fxSend = fxSend;
            _manualMixer = manualMixer;
            _background = background;

            PlayState = PlayState.Stopped;

            syncs.On(TrackSyncType.StartPreFadeIn, track => RunSyncHandler(StartPreFadeIn));
            syncs.On(TrackSyncType.StartFadeOut, track => RunSyncHandler(() =>
            {
                // the outgoing track's fade-out and the incoming track's fade-in always start together
                if (TrackHelper.IsSameTrack(track, CurrentTrack))
                    StartFadeOut();
            }));
            syncs.On(TrackSyncType.EndFadeOut, track => RunSyncHandler(EndFadeOut));
            syncs.On(TrackSyncType.EndFadeIn, track => RunSyncHandler(EndFadeIn));
            syncs.On(TrackSyncType.EndExtendedMix, track => RunSyncHandler(EndExtendedMix));
            syncs.On(TrackSyncType.StartSkipSection, track => RunSyncHandler(StartSkipSection));
        }

        /// <summary>
        ///     Runs a sync handler's work with _slotLock held, without ever making the mixer thread
        ///     wait for the lock. If another thread holds it, that thread may be inside a BASS call
        ///     waiting for this handler to return, so the work is run on the background queue instead,
        ///     as soon as the lock is free (a few milliseconds late).
        /// </summary>
        private void RunSyncHandler(Action work)
        {
            if (Monitor.TryEnter(_slotLock))
            {
                try
                {
                    work();
                }
                finally
                {
                    Monitor.Exit(_slotLock);
                }
                return;
            }

            _background.Post(() =>
            {
                lock (_slotLock)
                {
                    work();
                }
            });
        }

        /// <summary>
        ///     Raised when a new track becomes current. The sender is the current track. When raised
        ///     by a transition it arrives on a worker thread, shortly after the transition starts.
        /// </summary>
        public event EventHandler TrackChanged;

        public event EventHandler TrackQueued;

        /// <summary>
        ///     Raised (on a worker thread) when the current track's fade-in ends.
        /// </summary>
        public event EventHandler FadeInEnded;

        public event EventHandler SkippedToEnd;

        public PlayState PlayState { get; private set; }

        /// <summary>
        ///     If true, the fade-in section loops until the user moves on.
        /// </summary>
        public bool LoopFadeInForever { get; set; }

        /// <summary>
        ///     True from a "fade now" until the fade-out it started ends.
        /// </summary>
        private bool IsForceFadeNowMode { get; set; }

        private Track CurrentTrack => _deck.Current;

        private Track NextTrack => _deck.Next;

        private Track PreviousTrack => _deck.Previous;

        // ---------- queueing ----------

        public Track QueueTrack(string filename)
        {
            var track = _loader.LoadTrack(filename);
            QueueTrack(track);
            return track;
        }

        /// <summary>
        ///     Queues a track as the next track, or as the current track if there is none.
        /// </summary>
        public void QueueTrack(Track track)
        {
            if (track == null) return;

            // decode the audio first, without holding the lock
            _loader.LoadTrackAudioData(track);

            bool becameCurrent;
            lock (_slotLock)
            {
                IsForceFadeNowMode = false;

                AddTrackToMixer(track);

                Track replacedNext;
                becameCurrent = _deck.Queue(track, out replacedNext);

                if (!becameCurrent && !_deck.IsInUse(replacedNext))
                    RemoveTrackFromMixers(replacedNext);

                SetTrackSyncPositions();
            }

            if (becameCurrent)
                RaiseTrackChanged();

            TrackQueued?.Invoke(CurrentTrack, EventArgs.Empty);
            _sampler.Refresh();
        }

        /// <summary>
        ///     Loads a track's audio ahead of time, so queueing it later is quick.
        /// </summary>
        public void PreloadTrack(string filename)
        {
            var track = _loader.LoadTrack(filename);

            if (track == null) return;
            if (TrackHelper.IsSameTrack(track, _deck.Preloaded)) return;
            if (track.IsAudioLoaded()) return;

            // in a slot first, so the audio is not freed as unused while it loads
            _deck.SetPreloaded(track);
            _loader.LoadTrackAudioData(track);
        }

        /// <summary>
        ///     Clears the next/current/previous tracks.
        /// </summary>
        public void ClearTracks()
        {
            lock (_slotLock)
            {
                foreach (var track in _deck.ClearPlaying())
                    RemoveTrackFromMixers(track);
            }
        }

        /// <summary>
        ///     Reloads a track's tags and saved attributes, and re-sets its syncs if it is in use.
        /// </summary>
        public void ReloadTrack(string filename)
        {
            var track = _loader.Find(filename);
            if (track == null) return;

            _loader.LoadTagData(track);

            lock (_slotLock)
            {
                ExtendedAttributesHelper.LoadExtendedAttributes(track);

                if (_deck.IsInUse(track))
                    SetTrackSyncPositions(track);
            }
        }

        /// <summary>
        ///     Adds a track to the current-track mixer (paused), at the position it will start from.
        ///     Call with _slotLock held, after decoding the audio.
        /// </summary>
        private void AddTrackToMixer(Track track)
        {
            track.ResetPowerDownOnEnd();

            // freed as unused between decoding and taking the lock (rare): load it again
            if (!track.IsAudioLoaded())
                _loader.LoadTrackAudioData(track);

            if (track == PreviousTrack || track == CurrentTrack) return;

            AudioStreamHelper.AddToMixer(track, _graph.CurrentTrackMixer);

            track.SyncProc = _syncs.Callback;
            track.CurrentStartLoop = 0;
            track.CurrentEndLoop = 0;

            if (PreviousTrack == null)
            {
                AudioStreamHelper.SetPosition(track, 0);
                AudioStreamHelper.SetVolume(track, 1F);
            }
            else if (track.UsePreFadeIn)
            {
                AudioStreamHelper.SetPosition(track, track.PreFadeInStart);
                AudioStreamHelper.SetVolume(track, track.PreFadeInStartVolume);
            }
            else
            {
                AudioStreamHelper.SetPosition(track, track.FadeInStart);
                AudioStreamHelper.SetVolume(track, track.FadeInStartVolume);
            }
        }

        private void RemoveTrackFromMixers(Track track)
        {
            if (track == null) return;
            if (!track.IsAudioLoaded()) return;

            AudioStreamHelper.Pause(track);
            _loader.UnloadTrackAudioData(track);
        }

        // ---------- play / pause ----------

        /// <summary>
        ///     Plays the current track (or resumes playing if paused)
        /// </summary>
        public void Play()
        {
            var track = CurrentTrack;
            if (track != null)
                _loader.LoadTrackAudioData(track);

            lock (_slotLock)
            {
                track = CurrentTrack;
                if (track != null && track.IsAudioLoaded())
                {
                    AudioStreamHelper.SetPitch(track, 100F);
                    AudioStreamHelper.SetVolume(track, 1F);
                    AudioStreamHelper.Play(track);

                    PlayState = PlayState.Playing;
                }

                _graph.SetDelayByBpm();
            }
        }

        /// <summary>
        ///     Stops everything and plays a track from the start.
        /// </summary>
        public void ForcePlay(string filename)
        {
            Stop();
            _sampler.StopAll();
            ClearTracks();
            QueueTrack(filename);
            RaiseTrackChanged();
            Play();
            _sampler.Refresh();
        }

        public void Stop()
        {
            Pause();
            lock (_slotLock)
            {
                if (CurrentTrack != null)
                    AudioStreamHelper.SetPosition(CurrentTrack, 0);
                PlayState = PlayState.Stopped;
            }
        }

        /// <summary>
        ///     Pauses every track. Holds the lock for the short fade-to-silence (about 10 ms a track),
        ///     which only delays a transition that happens at the same moment.
        /// </summary>
        public void Pause()
        {
            _sampler.StopAll();

            lock (_slotLock)
            {
                if (CurrentTrack != null)
                    AudioStreamHelper.SmoothPauseSync(CurrentTrack);

                if (NextTrack != null)
                    AudioStreamHelper.SmoothPauseSync(NextTrack);

                if (PreviousTrack != null)
                    AudioStreamHelper.SmoothPauseSync(PreviousTrack);

                PlayState = PlayState.Paused;
            }

            _fxSend.Stop();
        }

        public bool IsPlaying()
        {
            return AudioStreamHelper.IsPlaying(CurrentTrack)
                   || AudioStreamHelper.IsPlaying(NextTrack)
                   || AudioStreamHelper.IsPlaying(PreviousTrack);
        }

        // ---------- position ----------

        /// <summary>
        ///     Gets the position of the active track (the current one, or the previous one if the
        ///     current track has run out).
        /// </summary>
        public TrackPosition GetTrackPosition()
        {
            // reads only, and the UI calls it often, so it takes no lock
            return GetPositionNoLock();
        }

        private TrackPosition GetPositionNoLock()
        {
            var position = new TrackPosition();
            var track = _deck.Active;
            if (track == null) return position;

            position.Track = track;

            position.ChannelPosition = AudioStreamHelper.GetPosition(track);
            position.Positition = TrackTimeline.ToAdjustedPosition(track, position.ChannelPosition);

            if (NextTrack != null)
                position.Length = track.ActiveLength;
            else
                position.Length = track.FadeOutEnd - track.FadeInStart + track.AdditionalEndLoopLength;
            return position;
        }

        private static double GetAdjustedPositionSeconds(Track track)
        {
            if (track == null) return 0;
            var position = TrackTimeline.ToAdjustedPosition(track, AudioStreamHelper.GetPosition(track));
            return track.SamplesToSeconds(position);
        }

        /// <summary>
        ///     Gets the BPM at the play position: the start BPM during the fade-in, then moving
        ///     towards the end BPM. Called by the BPM-synced plug-ins, sometimes from a sync handler,
        ///     so it takes no lock.
        /// </summary>
        public decimal GetCurrentBpm()
        {
            var bpm = 100M;
            var track = CurrentTrack;
            if (track != null)
            {
                var trackPosition = GetPositionNoLock();
                var position = trackPosition.Positition;
                if (position < 0) position = 0;

                if (position < track.FullStartLoopLength)
                {
                    bpm = track.StartBpm;
                }
                else
                {
                    var range = track.EndBpm - track.StartBpm;
                    var percentComplete = (decimal) (position / (double) trackPosition.Length);
                    bpm = track.StartBpm + range * percentComplete;
                }
            }

            return BpmHelper.NormaliseBpm(bpm);
        }

        /// <summary>
        ///     Seeks the active track to an adjusted position (see TrackTimeline). Positions outside
        ///     the fade-in start .. fade-out start range are ignored.
        /// </summary>
        public void SetAdjustedTrackPosition(long position)
        {
            _fxSend.Stop();

            lock (_slotLock)
            {
                var track = _deck.Active;
                if (track == null) return;

                track.CurrentEndLoop = 0;

                long channelPosition;
                int startLoop;
                if (!TrackTimeline.TryToChannelPosition(track, position, out channelPosition, out startLoop))
                    return;

                track.CurrentStartLoop = startLoop;
                AudioStreamHelper.SetPosition(track, channelPosition);

                if (PreviousTrack != null) AudioStreamHelper.Pause(PreviousTrack);
            }
        }

        private void SetTrackPosition(double seconds)
        {
            var track = _deck.Active;
            if (track == null) return;

            SetAdjustedTrackPosition(track.SecondsToSamples(seconds));
        }

        /// <summary>
        ///     Jumps back half a loop.
        /// </summary>
        public void JumpBack()
        {
            if (!IsPlaying())
                return;

            var loopLength = BpmHelper.GetDefaultLoopLength(GetCurrentBpm()) / 2;
            var position = GetAdjustedPositionSeconds(CurrentTrack) - loopLength;
            if (position < 0) return;
            SetTrackPosition(position);
        }

        /// <summary>
        ///     Skips to the end of the current track.
        /// </summary>
        public void SkipToEnd()
        {
            var track = CurrentTrack;
            if (track == null) return;

            ResetTempos();
            SetTrackPosition(track.ActiveLengthSeconds - 0.01D);

            SkippedToEnd?.Invoke(CurrentTrack, EventArgs.Empty);
        }

        /// <summary>
        ///     Skips to shortly before the current track's fade-out, so the mix into the next track can be heard.
        ///     Lands early enough for the next track's pre-fade-in to still play.
        /// </summary>
        public void SkipToFadeOut(int beatsBeforeFadeOut)
        {
            var track = CurrentTrack;
            if (track == null) return;

            ResetTempos();

            // the default loop is 16 beats
            var beatLength = BpmHelper.GetDefaultLoopLength(track.EndBpm) / 16D;
            var runIn = beatLength * beatsBeforeFadeOut;

            // the pre-fade-in starts when playback passes its position, so jumping past it would skip it
            var next = NextTrack;
            if (next != null && next.UsePreFadeIn)
                runIn = Math.Max(runIn, next.PreFadeInLengthSeconds + beatLength);

            SetTrackPosition(track.ActiveLengthSeconds - runIn);

            SkippedToEnd?.Invoke(CurrentTrack, EventArgs.Empty);
        }

        /// <summary>
        ///     Before a skip: back to normal tempo, and the previous track stopped.
        /// </summary>
        private void ResetTempos()
        {
            lock (_slotLock)
            {
                AudioStreamHelper.ResetTempo(CurrentTrack);
                if (PreviousTrack != null)
                {
                    AudioStreamHelper.ResetTempo(PreviousTrack);
                    AudioStreamHelper.Pause(PreviousTrack);
                }
                if (NextTrack != null)
                    AudioStreamHelper.ResetTempo(NextTrack);
            }
        }

        // ---------- sync positions ----------

        /// <summary>
        ///     Reloads the saved attributes of the previous, current and next tracks and resets their sync positions.
        /// </summary>
        public void ResetTrackSyncPositions()
        {
            lock (_slotLock)
            {
                ExtendedAttributesHelper.LoadExtendedAttributes(PreviousTrack);
                ExtendedAttributesHelper.LoadExtendedAttributes(CurrentTrack);
                ExtendedAttributesHelper.LoadExtendedAttributes(NextTrack);

                SetTrackSyncPositions(PreviousTrack);
                SetTrackSyncPositions(CurrentTrack);
                SetTrackSyncPositions(NextTrack);
            }
        }

        /// <summary>
        ///     Sets the current and next tracks' syncs. Call with _slotLock held.
        /// </summary>
        private void SetTrackSyncPositions()
        {
            SetTrackSyncPositions(CurrentTrack);
            SetTrackSyncPositions(NextTrack);
        }

        /// <summary>
        ///     Replaces a track's syncs with ones for its current fade points. For the current track
        ///     this also sets its automation triggers, its skip section, and the end of the recorded
        ///     mix on the previous track. Call with _slotLock held.
        /// </summary>
        private void SetTrackSyncPositions(Track track)
        {
            if (track == null) return;

            var syncs = new List<KeyValuePair<TrackSyncType, long>>
            {
                Sync(TrackSyncType.TrackEnd, 0),
                Sync(TrackSyncType.StartFadeIn, track.FadeInStart),
                Sync(TrackSyncType.EndFadeIn, track.FadeInEnd),
                Sync(TrackSyncType.StartFadeOut, track.FadeOutStart),
                Sync(TrackSyncType.EndFadeOut, track.FadeOutEnd)
            };

            var isCurrent = track == CurrentTrack;
            if (isCurrent)
            {
                // the next track's pre-fade-in starts this long before the current track's fade-out
                if (NextTrack != null && NextTrack.UsePreFadeIn)
                {
                    var preFadeInLength = track.SecondsToSamples(NextTrack.PreFadeInLengthSeconds);
                    syncs.Add(Sync(TrackSyncType.StartPreFadeIn, track.FadeOutStart - preFadeInLength));
                }

                syncs.AddRange(_automation.GetSyncPositions(track));

                if (track.HasSkipSection)
                    syncs.Add(Sync(TrackSyncType.StartSkipSection, track.SkipStart));
            }

            track.Syncs.ReplaceAll(syncs);

            if (track == NextTrack && track.UsePreFadeIn)
            {
                var preFadeInLength = CurrentTrack.SecondsToSamples(track.PreFadeInLengthSeconds);
                CurrentTrack.Syncs.Set(TrackSyncType.StartPreFadeIn, CurrentTrack.FadeOutStart - preFadeInLength);
            }

            if (isCurrent && HasExtendedMixAttributes())
                PreviousTrack.Syncs.Set(TrackSyncType.EndExtendedMix, GetExtendedMixAttributes().FadeEnd);
        }

        private static KeyValuePair<TrackSyncType, long> Sync(TrackSyncType syncType, long position)
        {
            return new KeyValuePair<TrackSyncType, long>(syncType, position);
        }

        // ---------- transitions (sync handlers) ----------
        // Each runs with _slotLock held, through RunSyncHandler: normally on the BASS mixer thread at
        // the sync's exact sample, occasionally a few milliseconds later on the background queue.

        /// <summary>
        ///     The current track's fade-out has started: every track moves along a slot, the
        ///     outgoing track fades as planned, and the incoming track starts fading in.
        /// </summary>
        private void StartFadeOut()
        {
            var oldPreviousTrack = _deck.Advance();
            var newCurrentTrack = CurrentTrack;

            _manualMixer.MoveToNextTransition(GetCurrentExtendedFadeType());

            if (newCurrentTrack == null)
                PlayState = PlayState.Stopped;

            if (PreviousTrack != null)
            {
                PreviousTrack.CurrentEndLoop = 0;

                // change tempo if required
                AudioStreamHelper.SetTrackTempoToMatchAnotherTrack(PreviousTrack, CurrentTrack);

                var plan = TransitionPlanner.PlanFadeOut(PreviousTrack, CurrentTrack, GetExtendedMixAttributes(),
                    _manualMixer.IsOn, _manualMixer.FadeTypeIn, IsForceFadeNowMode);
                ApplyFadeOutPlan(PreviousTrack, plan);
            }

            if (newCurrentTrack != null)
                StartFadeIn(newCurrentTrack);

            // the track that was previous before this transition is now out of every slot
            if (oldPreviousTrack != null)
                _background.Post(() =>
                {
                    lock (_slotLock)
                    {
                        if (!_deck.IsInUse(oldPreviousTrack))
                            RemoveTrackFromMixers(oldPreviousTrack);
                    }
                });

            if (newCurrentTrack != null)
                _background.Post(() => TrackChanged?.Invoke(newCurrentTrack, EventArgs.Empty));

            _background.Post(_sampler.Refresh);
        }

        /// <summary>
        ///     Starts the new current track fading in. Called from StartFadeOut.
        /// </summary>
        private void StartFadeIn(Track track)
        {
            SetTrackSyncPositions();

            // normally loaded when it was queued
            if (!track.IsAudioLoaded()) _loader.LoadTrackAudioData(track);

            track.CurrentStartLoop = 0;

            AudioStreamHelper.SetPosition(track, track.FadeInStart);

            var startVolume = track.FadeInStartVolume;
            var endVolume = track.FadeInEndVolume;

            if (PreviousTrack != null && PreviousTrack.PowerDownOnEnd)
            {
                startVolume = track.FadeInEndVolume;
                endVolume = 1F;
            }

            AudioStreamHelper.SetVolumeSlide(track, startVolume, endVolume, GetFadeLength());
            AudioStreamHelper.SetPitch(track, 100);
            AudioStreamHelper.Play(track);

            _graph.SetDelayByBpm();
        }

        /// <summary>
        ///     The current track's fade-in has ended: loop it again if needed, otherwise go to full volume.
        /// </summary>
        private void EndFadeIn()
        {
            var track = CurrentTrack;
            if (track == null) return;

            var maxLoops = track.StartLoopCount;
            var looped = track.IsLoopedAtStart && track.CurrentStartLoop < maxLoops - 1 || LoopFadeInForever;

            if (looped)
            {
                track.CurrentStartLoop++;
                if (LoopFadeInForever && track.CurrentStartLoop == maxLoops) track.CurrentStartLoop = 0;

                // add one so the StartFadeIn sync does not fire again
                AudioStreamHelper.SetPosition(track, track.FadeInStart + 1);
                return;
            }

            AudioStreamHelper.SetVolume(track, 1F);

            if (!_manualMixer.IsOn || _manualMixer.FadeTypeIn == ExtendedFadeType.PowerDown)
                _background.Post(() => FadeInEnded?.Invoke(track, EventArgs.Empty));

            _background.Post(() =>
            {
                lock (_slotLock)
                {
                    _loader.UnloadUnusedAudioData(_deck.IsInUse);
                }
            });
        }

        /// <summary>
        ///     The previous track's fade-out has ended: loop it again if needed, otherwise stop it
        ///     (unless the DJ or a recorded mix is still fading it).
        /// </summary>
        private void EndFadeOut()
        {
            IsForceFadeNowMode = false;

            var track = PreviousTrack;
            if (track == null) return;

            var manual = _manualMixer.IsOn;
            var loopForever = track.IsLoopedAtEnd && (manual || HasExtendedMixAttributes());

            if (track.IsLoopedAtEnd && track.CurrentEndLoop < track.EndLoopCount - 1 || loopForever)
            {
                track.CurrentEndLoop++;

                // add one so the StartFadeOut sync does not fire again
                AudioStreamHelper.SetPosition(track, track.FadeOutStart + 1);
            }
            else if (!manual && !HasExtendedMixAttributes())
            {
                AudioStreamHelper.Pause(track);
            }
        }

        /// <summary>
        ///     A recorded extended mix has reached its end on the previous track.
        /// </summary>
        private void EndExtendedMix()
        {
            if (_manualMixer.IsOn) return;
            var track = PreviousTrack;
            if (track == null) return;
            if (!HasExtendedMixAttributes()) return;

            var mixAttributes = GetExtendedMixAttributes();
            if (mixAttributes.FadeEndLoop != 0 && track.CurrentEndLoop != mixAttributes.FadeEndLoop) return;

            if (mixAttributes.PowerDownAfterFade)
                AudioStreamHelper.PowerDown(track);
            else
                AudioStreamHelper.Pause(track);
        }

        /// <summary>
        ///     Starts the next track's pre-fade-in.
        /// </summary>
        private void StartPreFadeIn()
        {
            var track = NextTrack;
            if (track == null || !track.UsePreFadeIn) return;

            // normally loaded when it was queued
            if (!track.IsAudioLoaded()) _loader.LoadTrackAudioData(track);

            AudioStreamHelper.SetPosition(track, track.PreFadeInStart);
            AudioStreamHelper.SetVolumeSlide(track, track.PreFadeInStartVolume * track.FadeInStartVolume,
                track.FadeInStartVolume, track.PreFadeInLength);
            AudioStreamHelper.SetPitch(track, 100);
            AudioStreamHelper.Play(track);
        }

        /// <summary>
        ///     Jumps over the skip section.
        /// </summary>
        private void StartSkipSection()
        {
            var track = CurrentTrack;
            if (track == null || !track.HasSkipSection) return;

            AudioStreamHelper.SetPosition(track, track.SkipEnd);
        }
        /// <summary>
        ///     Carries out a fade-out plan on the outgoing track. Call with _slotLock held.
        /// </summary>
        private void ApplyFadeOutPlan(Track track, FadeOutPlan plan)
        {
            if (plan.ReshapeAs.HasValue)
                ShapeFadeOut(track, plan.ReshapeAs.Value);

            switch (plan.Action)
            {
                case FadeOutAction.None:
                    break;
                case FadeOutAction.VolumeSlide:
                    AudioStreamHelper.SetVolumeSlide(track, plan.StartVolume, plan.EndVolume, plan.LengthSeconds);
                    break;
                case FadeOutAction.PowerDown:
                    AudioStreamHelper.PowerDown(track);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(plan), plan.Action, null);
            }

            if (plan.EndsAutoMixRecording)
                _manualMixer.RecordAutoMix();
        }

        /// <summary>
        ///     Gets the crossfade length between the previous and current tracks.
        /// </summary>
        private double GetFadeLength()
        {
            return TransitionPlanner.GetFadeLength(PreviousTrack, CurrentTrack, GetExtendedMixAttributes());
        }

        // ---------- fade settings ----------

        /// <summary>
        ///     Sets a safe fade-out for the current track into the next one, unless it already powers down.
        /// </summary>
        public void SetConservativeFadeOutSettings()
        {
            lock (_slotLock)
            {
                var track = CurrentTrack;
                var next = NextTrack;
                if (track == null || next == null) return;
                if (track.PowerDownOnEnd) return;

                ShapeFadeOut(track, TransitionPlanner.GetConservativeFadeType(track, next));
            }
        }

        /// <summary>
        ///     Call with _slotLock held.
        /// </summary>
        private void ShapeFadeOut(Track track, ExtendedFadeType fadeType)
        {
            TransitionPlanner.ShapeFadeOut(track, fadeType);
            SetTrackSyncPositions(track);
        }

        /// <summary>
        ///     Starts the transition to the next track now, or skips to just before the fade-out.
        /// </summary>
        public void ForceFadeNow(ForceFadeType fadeType)
        {
            if (PlayState != PlayState.Playing)
                return;

            lock (_slotLock)
            {
                var track = CurrentTrack;
                if (NextTrack == null || track == null)
                    return;

                if (fadeType == ForceFadeType.SkipToEnd)
                {
                    AudioStreamHelper.SetPosition(track, track.FadeOutStart - track.SecondsToSamples(0.01M));
                    return;
                }

                IsForceFadeNowMode = true;

                var length = TransitionPlanner.GetForceFadeLength(track, fadeType);
                if (fadeType == ForceFadeType.PowerDown)
                    track.PowerDownOnEnd = true;

                track.EndLoopCount = 0;

                // Move the fade-out to the play position and start the transition now. (Setting a sync
                // just ahead of the play position is not reliable: BASS decodes ahead of what is heard,
                // and can already be past it.)
                var position = AudioStreamHelper.GetPosition(track);
                track.FadeOutStart = position;
                track.FadeOutEnd = position + track.SecondsToSamples(length);
                track.Syncs.Set(TrackSyncType.StartFadeOut, track.FadeOutStart);
                track.Syncs.Set(TrackSyncType.EndFadeOut, track.FadeOutEnd);

                StartFadeOut();
            }
        }

        public void PowerOffCurrentTrack()
        {
            _sampler.StopAll();

            var track = CurrentTrack;
            if (track == null) return;
            if (PlayState != PlayState.Playing) return;
            AudioStreamHelper.PowerDown(track);
        }

        // ---------- extended mixes ----------

        public bool HasExtendedMixAttributes(Track fadeOutTrack, Track fadeInTrack)
        {
            return GetExtendedMixAttributes(fadeOutTrack, fadeInTrack) != null;
        }

        public ExtendedFadeType GetExtendedFadeType(Track fromTrack, Track toTrack)
        {
            var mixAttributes = GetExtendedMixAttributes(fromTrack, toTrack);
            return mixAttributes == null ? ExtendedFadeType.Default : mixAttributes.ExtendedFadeType;
        }

        public double GetExtendedFadeOutLength(Track fromTrack, Track toTrack)
        {
            return TransitionPlanner.GetExtendedFadeOutLength(fromTrack, toTrack,
                GetExtendedMixAttributes(fromTrack, toTrack));
        }

        /// <summary>
        ///     True if the previous and current tracks have a recorded extended mix with a length.
        /// </summary>
        private bool HasExtendedMixAttributes()
        {
            var attributes = GetExtendedMixAttributes();
            return attributes != null && attributes.FadeLength != 0;
        }

        private ExtendedMixAttributes GetExtendedMixAttributes()
        {
            return GetExtendedMixAttributes(PreviousTrack, CurrentTrack);
        }

        private static ExtendedMixAttributes GetExtendedMixAttributes(Track fadeOutTrack, Track fadeInTrack)
        {
            if (fadeOutTrack == null || fadeInTrack == null) return null;
            var attributes = AutomationAttributesHelper.GetAutomationAttributes(fadeOutTrack.Description);
            return attributes?.GetExtendedMixAttributes(fadeInTrack.Description);
        }

        private ExtendedFadeType GetCurrentExtendedFadeType()
        {
            return HasExtendedMixAttributes() ? GetExtendedMixAttributes().ExtendedFadeType : ExtendedFadeType.Default;
        }

        private void RaiseTrackChanged()
        {
            _graph.SetDelayByBpm();
            TrackChanged?.Invoke(CurrentTrack, EventArgs.Empty);
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
