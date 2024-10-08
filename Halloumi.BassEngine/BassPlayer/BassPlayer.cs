﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.AudioEngine.SectionDetector;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     Track mixing engine utilizing the Bass.Net audio engine
    /// </summary>
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public partial class BassPlayer : IDisposable, IBmpProvider
    {
        /// <summary>
        ///     A collection of all loaded tracks
        /// </summary>
        private static readonly List<Track> CachedTracks = new List<Track>();

        /// <summary>
        ///     The next available track Id
        /// </summary>
        private static int _nextTrackId;

        private static List<Track> _recentTracks = new List<Track>();

        private static readonly object MixerLock = new object();
        public MonitorOutputChannel MonitorOutput { get; }

        private bool _locked;

        private MixerChannel _trackMixer;

        private MixerChannel _previousTrackMixer;

        private MixerChannel _currentTrackMixer;

        private OutputSplitter _trackOutputSplitter;

        private MixerChannel _trackSendFxMixer;

        private MixerChannel _trackSendMixer;


        /// <summary>
        ///     Initializes a new instance of the BassPlayer class.
        /// </summary>
        public BassPlayer()
            : this(IntPtr.Zero)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the BassPlayer class.
        /// </summary>
        public BassPlayer(IntPtr windowHandle)
        {
            LimitSongLength = false;
            MaxSongLength = 5 * 60; // 5 minutes (if LimitSongLength set to true)
            DefaultFadeLength = 10; // 10 seconds
            DefaultFadeInStartVolume = 50; // 50 %
            DefaultFadeInEndVolume = 100; // 100%
            DefaultFadeOutStartVolume = 80; // 100%
            DefaultFadeOutEndVolume = 0; // 0%
            LoopFadeInForever = false;

            TrackFxAutomationEnabled = false;

            // start audio engine
            AudioEngineHelper.StartAudioEngine(windowHandle);

            SpeakerOutput = new SpeakerOutputChannel();
            MonitorOutput = new MonitorOutputChannel();

            PlayState = PlayState.Stopped;

            InitialiseTrackMixer();
            InitialiseSampler();
            InitialiseRawLoopMixer();
            InitialiseManualMixer();

            ExtenedAttributesHelper.ShufflerFolder = "";
        }

        public bool LoopFadeInForever { get; set; }

        /// <summary>
        ///     Gets the current track.
        /// </summary>
        public Track CurrentTrack { get; private set; }

        public SpeakerOutputChannel SpeakerOutput { get; }

        /// <summary>
        ///     Gets the preloaded track.
        /// </summary>
        public Track PreloadedTrack { get; private set; }

        /// <summary>
        ///     Gets the track queued to play next.
        /// </summary>
        public Track NextTrack { get; private set; }

        /// <summary>
        ///     Gets the last track that was previously played.
        /// </summary>
        public Track PreviousTrack { get; private set; }

        /// <summary>
        ///     Gets the active track - usually the current, unless the current
        ///     has finished playing and there was no track queued to play next.
        /// </summary>
        public Track ActiveTrack
        {
            get { return CurrentTrack ?? PreviousTrack; }
        }

        /// <summary>
        ///     Gets the state of the play - playing, paused, or stopped.
        /// </summary>
        public PlayState PlayState { get; private set; }

        /// <summary>
        ///     Gets or sets the volume of the bass player as decimal 0 - 100.
        /// </summary>
        public decimal Volume
        {
            get { return AudioEngineHelper.Volume; }
            set { AudioEngineHelper.Volume = value; }
        }

        /// <summary>
        ///     Gets or sets the maximum length of songs (in seconds)
        /// </summary>
        public double MaxSongLength { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether song lengths should be limited
        /// </summary>
        public bool LimitSongLength { get; set; }

        /// <summary>
        ///     Gets or sets the default cross fade length (in seconds)
        /// </summary>
        public double DefaultFadeLength { get; set; }

        /// <summary>
        ///     Gets or sets the default fade-in start volume.
        /// </summary>
        public double DefaultFadeInStartVolume { get; set; }

        /// <summary>
        ///     Gets or sets the default fade-in end volume.
        /// </summary>
        public double DefaultFadeInEndVolume { get; set; }

        /// <summary>
        ///     Gets or sets the default fade-out start volume.
        /// </summary>
        public double DefaultFadeOutStartVolume { get; set; }

        /// <summary>
        ///     Gets or sets the default fade-out end volume.
        /// </summary>
        public double DefaultFadeOutEndVolume { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the volume fade out is manual or automatic
        /// </summary>
        public bool IsManualMixMode { get; private set; }


        /// <summary>
        ///     Gets or sets a value indicating whether automatic track FX is enabled
        /// </summary>
        public bool TrackFxAutomationEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether automatic track FX is enabled
        /// </summary>
        public bool SampleAutomationEnabled { get; set; }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            // DebugHelper.WriteLine("Destroying Bass Engine");

            Stop();

            UnloadAllWaPlugins();
            UnloadAllVstPlugins();

            // unload all tracks
            foreach (var track in CachedTracks)
                UnloadTrackAudioData(track);
            CachedTracks.Clear();

            AudioEngineHelper.StopAudioEngine();
        }

        public void ToggleManualMixMode()
        {
            if (IsManualMixMode)
                DisableManualMixMode();
            else
                EnableManualMixMode();
        }

        public void EnableManualMixMode()
        {
            IsManualMixMode = true;
            CurrentManualExtendedFadeType = ExtendedFadeType.Default;
            PreviousManaulExtendedFadeType = ExtendedFadeType.Default;
            SetManualMixVolume(0);
            OnManualMixModeChanged?.Invoke(CurrentTrack, EventArgs.Empty);
        }

        public void DisableManualMixMode()
        {
            IsManualMixMode = false;
            CurrentManualExtendedFadeType = ExtendedFadeType.Default;
            PreviousManaulExtendedFadeType = ExtendedFadeType.Default;
            PausePreviousTrack();
            SetManualMixVolume(0);
            OnManualMixModeChanged?.Invoke(CurrentTrack, EventArgs.Empty);
        }


        /// <summary>
        ///     Event raised when the currently playing track changes
        /// </summary>
        public event EventHandler OnTrackChange;

        /// <summary>
        ///     Event raised when a track is queued
        /// </summary>
        public event EventHandler OnTrackQueued;

        /// <summary>
        ///     Event raised when the fade in on the current track ends
        /// </summary>
        public event EventHandler OnEndFadeIn;

        /// <summary>
        ///     Event raised when the fade in on the current track ends
        /// </summary>
        public event EventHandler OnSkipToEnd;

        public event EventHandler TrackTagsLoaded;

        public event EventHandler OnVolumeChanged;

        public event EventHandler OnManualMixModeChanged;

        /// <summary>
        ///     Initializes the track mixer.
        /// </summary>
        private void InitialiseTrackMixer()
        {
            // DebugHelper.WriteLine("InitialiseTrackMixer");

            _currentTrackMixer = new MixerChannel(this);
            _previousTrackMixer = new MixerChannel(this);

            // create mixer channels
            _trackMixer = new MixerChannel(this, MixerChannelOutputType.MultipleOutputs);
            _trackOutputSplitter = new OutputSplitter(_trackMixer, SpeakerOutput, MonitorOutput);

            _trackMixer.AddInputChannel(_currentTrackMixer);
            _trackMixer.AddInputChannel(_previousTrackMixer);

            // create clone of mixer channel and mute it
            _trackSendMixer = new MixerChannel(this);
            _trackSendMixer.AddInputChannel(_trackMixer);
            _trackSendMixer.SetVolume(0);

            // add the track FX to the track FX mixer,
            _trackSendFxMixer = new MixerChannel(this);
            _trackSendFxMixer.AddInputChannel(_trackSendMixer);
            _trackSendFxMixer.CutBass();
            _trackSendFxMixer.SetVolume(80);

            // then that to the main mixer
            SpeakerOutput.AddInputChannel(_trackSendFxMixer);

            // DebugHelper.WriteLine("END InitialiseTrackMixer");
        }

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
        ///     Sets the previous track.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void SetPreviousTrack(string filename)
        {
            var track = LoadTrack(filename);
            PreviousTrack = track;
        }

        /// <summary>
        ///     Enqueues a track for playing.
        /// </summary>
        /// <param name="filename">The filename of the track to play.</param>
        /// <returns>A track object for the specified file</returns>
        public Track QueueTrack(string filename)
        {
            var track = LoadTrack(filename);
            QueueTrack(track);
            return track;
        }

        public void PreloadTrack(string filename)
        {
            var track = LoadTrack(filename);

            if (track == null) return;
            if (TrackHelper.IsSameTrack(track, PreloadedTrack)) return;
            if (track.IsAudioLoaded()) return;

            // load audio data if not loaded
            WaitForLock();
            Lock();

            PreloadedTrack = track;
            LoadTrackAudioData(track);
            Unlock();
        }

        /// <summary>
        ///     Enqueues a track for playing as the next track, or current track if there is no current one.
        /// </summary>
        /// <param name="track">The track to enqueue.</param>
        public void QueueTrack(Track track)
        {
            if (track == null) return;

            // DebugHelper.WriteLine("Queuing track " + track.Description);

            IsForceFadeNowMode = false;

            AddTrackToMixer(track);

            if (CurrentTrack == null)
            {
                CurrentTrack = track;
                SetTrackSyncPositions();
                RaiseOnTrackChange();
            }
            else
            {
                var oldNextTrack = NextTrack;
                NextTrack = track;
                if (!IsTrackInUse(oldNextTrack))
                    RemoveTrackFromMixers(oldNextTrack);

                SetTrackSyncPositions();
            }

            RaiseOnTrackQueued();
            RefreshSamples();
        }

        /// <summary>
        ///     Clears the next track.
        /// </summary>
        public void ClearNextTrack()
        {
            if (NextTrack == null) return;
            var oldNextTrack = NextTrack;
            NextTrack = null;
            if (!IsTrackInUse(oldNextTrack)) RemoveTrackFromMixers(oldNextTrack);

            SetTrackSyncPositions(CurrentTrack);
        }

        public bool IsTrackDataEntered(Track track)
        {
            return track.FadeInStart != 0;
        }

        /// <summary>
        ///     Loads the details and meta data about a track .
        /// </summary>
        /// <param name="filename">The filename of the track to load.</param>
        /// <returns>A track object</returns>
        public Track LoadTrack(string filename)
        {
            return LoadTrack(filename, "", "");
        }

        public Track LoadTrackAndAudio(string filename)
        {
            var track = LoadTrack(filename);

            if (track.IsAudioLoaded()) return track;

            LoadTrackAudioData(track);
            ExtenedAttributesHelper.LoadExtendedAttributes(track);

            return track;
        }

        /// <summary>
        ///     Loads the details and meta data about a track .
        /// </summary>
        /// <param name="filename">The filename of the track to load.</param>
        /// <param name="artist">The artist of the track.</param>
        /// <param name="title">The title of the track.</param>
        /// <returns>
        ///     A track object
        /// </returns>
        public Track LoadTrack(string filename, string artist, string title)
        {
            if (IsTrackLoaded(filename))
                return
                    CachedTracks.FirstOrDefault(
                        t => string.Equals(t.Filename, filename, StringComparison.CurrentCultureIgnoreCase));

            // DebugHelper.WriteLine("Loading track " + filename);

            if (!File.Exists(filename)) throw new Exception("Cannot find file " + filename);

            var track = new Track
            {
                Id = _nextTrackId++,
                Filename = filename
            };

            SetArtistAndTitle(track, title, artist);

            ExtenedAttributesHelper.LoadExtendedAttributes(track);

            CachedTracks.Add(track);

            LoadTagData(track);

            return track;
        }

        public bool IsTrackLoaded(string filename)
        {
            return CachedTracks.Exists(t => string.Equals(t.Filename, filename,
                StringComparison.CurrentCultureIgnoreCase));
        }


        /// <summary>
        ///     Determines whether this instance is playing.
        /// </summary>
        public bool IsPlaying()
        {
            return AudioStreamHelper.IsPlaying(CurrentTrack) 
                || AudioStreamHelper.IsPlaying(NextTrack) 
                || AudioStreamHelper.IsPlaying(PreviousTrack);
        }

        /// <summary>
        ///     Sets the artist and title for a track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="title">The title.</param>
        /// <param name="artist">The artist.</param>
        private static void SetArtistAndTitle(Track track, string title, string artist)
        {
            if (title == "" || artist == "")
            {
                GuessArtistAndTitleFromFilename(track);
            }
            else
            {
                track.Artist = artist;
                track.Title = title;
                if (!title.Contains("/")) return;

                track.Artist = title.Split('/')[0].Trim();
                track.Title = title.Split('/')[1].Trim();
            }
        }

        /// <summary>
        ///     Guesses the artist and title of a track from its filename.
        /// </summary>
        /// <param name="track">The track.</param>
        private static void GuessArtistAndTitleFromFilename(Track track)
        {
            var trackDetails = TrackHelper.GuessTrackDetailsFromFilename(track.Filename);
            track.Title = trackDetails.Title;
            track.Artist = trackDetails.Artist;
        }

        /// <summary>
        ///     Loads data for a track from its mp3 tags
        /// </summary>
        /// <param name="track">The track.</param>
        public void LoadTagData(Track track)
        {
            if (track == null) return;

            if ((Path.GetExtension(track.Filename) + "").ToLower() == ".wav") return;

            lock (track)
            {
                if (track.TagDataLoaded) return;

                // DebugHelper.WriteLine("Loading track tags - " + track.Description);

                var tags = TagHelper.LoadTags(track.Filename);

                track.Title = tags.Title;
                track.Artist = tags.Artist;

                if (track.Title == "" || track.Artist == "")
                    GuessArtistAndTitleFromFilename(track);

                if (tags.Gain.HasValue)
                    track.Gain = tags.Gain.Value;

                if (!string.IsNullOrEmpty(tags.Key))
                    track.Key = tags.Key;

                if (tags.Bpm.HasValue)
                    track.TagBpm = tags.Bpm.Value;

                if (tags.Length.HasValue)
                    track.Length = (long) tags.Length.Value * 1000;

                track.TagDataLoaded = true;

                TrackTagsLoaded?.Invoke(track, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Unloads a track from the cached tracks
        /// </summary>
        /// <param name="track">The track to unload.</param>
        public void UnloadTrack(Track track)
        {
            if (track == null) return;
            // DebugHelper.WriteLine("Unloading track " + track.Description);

            AudioStreamHelper.Pause(track);
            if (IsTrackInUse(track))
                RemoveTrackFromMixers(track);

            UnloadTrackAudioData(track);
            CachedTracks.Remove(track);
        }

        /// <summary>
        ///     Loads the track audio data.
        /// </summary>
        /// <param name="track">The track to load.</param>
        /// <returns>
        ///     The loaded track
        /// </returns>
        public Track LoadTrackAudioData(Track track)
        {
            // abort if audio data already loaded
            if (track.IsAudioLoaded()) return track;

            // ensure mp3 tag data is loaded
            if (!track.TagDataLoaded) LoadTagData(track);

            // DebugHelper.WriteLine("Loading track Audio Data " + track.Description);
            lock (track)
            {
                AudioStreamHelper.LoadAudio(track);

                track.FadeInStart = 0;
                track.FadeInStartVolume = (float) (DefaultFadeInStartVolume / 100);
                track.FadeInEndVolume = (float) (DefaultFadeInEndVolume / 100);

                track.FadeOutEnd = 0;
                track.FadeOutStartVolume = (float) (DefaultFadeOutStartVolume / 100);
                track.FadeOutEndVolume = (float) (DefaultFadeOutEndVolume / 100);

                ExtenedAttributesHelper.LoadExtendedAttributes(track);

                if (track.StartBpm == 0 || track.EndBpm == 0) 
                {
                    var bpm = BPMGuestimator.EstimateBPM(track.Filename);
                    track.StartBpm = Convert.ToDecimal(bpm);
                    track.EndBpm = Convert.ToDecimal(bpm);
                }

                if (track.FadeOutStart == 0)
                    if (LimitSongLength && track.LengthSeconds > MaxSongLength)
                        track.FadeOutStart = track.SecondsToSamples(MaxSongLength - DefaultFadeLength - 1);
                    else
                        track.FadeOutStart = track.SecondsToSamples(track.LengthSeconds - DefaultFadeLength - 1);

                if (track.FadeInEnd == track.FadeInStart || track.FadeInEnd == 0)
                    track.FadeInEnd = track.FadeInStart +
                                      track.SecondsToSamples(BpmHelper.GetBestFitLoopLength(track.StartBpm,
                                          DefaultFadeLength));

                if (track.FadeOutEnd == track.FadeInStart || track.FadeOutEnd == 0)
                    track.FadeOutEnd = track.FadeOutStart +
                                       track.SecondsToSamples(BpmHelper.GetBestFitLoopLength(track.EndBpm,
                                           DefaultFadeLength));

                if (!track.UsePreFadeIn)
                {
                    track.PreFadeInStart = track.FadeInStart;
                    track.PreFadeInStartVolume = 0;
                }
            }

            AddToRecentTracks(track);

            // DebugHelper.WriteLine("Finished loading track Audio Data " + track.Description);

            return track;
        }

        /// <summary>
        ///     Unloads the track audio data.
        /// </summary>
        /// <param name="track">The track.</param>
        public void UnloadTrackAudioData(Track track)
        {
            if (!track.IsAudioLoaded()) return;

            // DebugHelper.WriteLine("Unloading track Audio Data " + track.Description);

            
            AudioStreamHelper.RemoveFromMixer(track, _currentTrackMixer);
            AudioStreamHelper.RemoveFromMixer(track, _previousTrackMixer);

            AudioStreamHelper.UnloadAudio(track);
            track.SyncProc = null;
        }

        /// <summary>
        ///     Unloads all unused audio data.
        /// </summary>
        private void UnloadUnusedAudioData()
        {
            try
            {
                List<Track> unusedTracks;
                lock (_recentTracks)
                {
                    unusedTracks = CachedTracks
                        .Where(t => t.IsAudioLoaded() && !IsTrackInUse(t))
                        .Where(t => !_recentTracks.Exists(rt => rt.Description == t.Description))
                        .ToList();
                }

                foreach (var track in unusedTracks)
                    UnloadTrackAudioData(track);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void AddToRecentTracks(Track track)
        {
            const int length = 5;
            if (track == null) return;

            lock (_recentTracks)
            {
                if (_recentTracks.Exists(rt => rt.Description == track.Description))
                    _recentTracks.RemoveAll(rt => rt.Description == track.Description);

                _recentTracks.Insert(0, track);
                if (_recentTracks.Count > length)
                    _recentTracks = _recentTracks.Take(length).ToList();
            }
        }

        /// <summary>
        ///     Plays the current track (or resumes playing if paused)
        /// </summary>
        public void Play()
        {
            if (CurrentTrack != null)
            {
                // DebugHelper.WriteLine("Play/resume playing current Track");

                if (!CurrentTrack.IsAudioLoaded()) LoadTrackAudioData(CurrentTrack);

                AudioStreamHelper.SetPitch(CurrentTrack, 100F);
                AudioStreamHelper.SetVolume(CurrentTrack, 1F);
                AudioStreamHelper.Play(CurrentTrack);

                PlayState = PlayState.Playing;
            }

            SetDelayByBpm();
        }

        /// <summary>
        ///     Stops the current track and starts the next one
        /// </summary>
        public void ForcePlayNext()
        {
            // DebugHelper.WriteLine("ForceNextPrevious");
            if (NextTrack == null) return;
            var trackName = NextTrack.Filename;
            ForcePlay(trackName);
        }

        /// <summary>
        ///     Stops the current track and starts the next one
        /// </summary>
        public void ForceCueNext()
        {
            // DebugHelper.WriteLine("ForcePlayNext");
            if (NextTrack == null) return;
            var trackName = NextTrack.Filename;
            ForcePlay(trackName);
        }

        /// <summary>
        ///     Stops the current track and replays the previous one
        /// </summary>
        public void ForcePlayPrevious()
        {
            // DebugHelper.WriteLine("ForcePlayPrevious");
            if (PreviousTrack == null) return;
            var trackName = PreviousTrack.Filename;
            ForcePlay(trackName);
        }

        /// <summary>
        ///     Forces the play.
        /// </summary>
        /// <param name="filename">The filename of the track to play.</param>
        public void ForcePlay(string filename)
        {
            Stop();
            StopSamples();
            ClearTracks();
            QueueTrack(filename);
            RaiseOnTrackChange();
            Play();
            RefreshSamples();
        }

        /// <summary>
        ///     Clears the next/current/previous tracks.
        /// </summary>
        public void ClearTracks()
        {
            RemoveTrackFromMixers(PreviousTrack);
            PreviousTrack = null;
            RemoveTrackFromMixers(NextTrack);
            NextTrack = null;
            RemoveTrackFromMixers(CurrentTrack);
            CurrentTrack = null;
        }

        /// <summary>
        ///     Forces a track to be played
        /// </summary>
        /// <param name="track">The track.</param>
        public void ForcePlay(Track track)
        {
            ForcePlay(track.Filename);
        }

        /// <summary>
        ///     Stops the current playback
        /// </summary>
        public void Stop()
        {
            // DebugHelper.WriteLine("Stop");

            Pause();
            if (CurrentTrack != null)
                AudioStreamHelper.SetPosition(CurrentTrack, 0);
            PlayState = PlayState.Stopped;
        }

        /// <summary>
        ///     Pauses the current playback
        /// </summary>
        public void Pause()
        {
            // DebugHelper.WriteLine("Pause");

            StopSamples();

            if (CurrentTrack != null)
                AudioStreamHelper.SmoothPause(CurrentTrack);

            if (NextTrack != null)
                AudioStreamHelper.SmoothPause(NextTrack);

            if (PreviousTrack != null)
                AudioStreamHelper.SmoothPause(PreviousTrack);

            StopTrackFxSend();
            PlayState = PlayState.Paused;
            Thread.Sleep(150);
        }

        /// <summary>
        ///     Gets the current volume levels.
        /// </summary>
        /// <returns>A VolumeLevels object containing the left and right volume levels (0 - 32768)</returns>
        public VolumeLevels GetVolumeLevels()
        {
            WaitForLock();

            lock (MixerLock)
            {
                return SpeakerOutput.GetVolumeLevels();
            }
        }

        /// <summary>
        ///     Gets the mixer volume.
        /// </summary>
        /// <returns>A value between 0 and 100</returns>
        public decimal GetMixerVolume()
        {
            return SpeakerOutput.GetVolume();
        }

        /// <summary>
        ///     Sets the mixer volume.
        /// </summary>
        /// <param name="volume">The volume as a value between 0 and 100.</param>
        public void SetMixerVolume(decimal volume)
        {
            SpeakerOutput.SetVolume(volume);
            OnVolumeChanged?.Invoke(CurrentTrack, EventArgs.Empty);
        }

        /// <summary>
        ///     Gets the monitor volume.
        /// </summary>
        /// <returns>A value between 0 and 100</returns>
        public decimal GetMonitorVolume()
        {
            return MonitorOutput.GetVolume();
        }

        /// <summary>
        ///     Sets the monitor volume.
        /// </summary>
        /// <param name="volume">The volume as a value between 0 and 100.</param>
        public void SetMonitorVolume(decimal volume)
        {
            MonitorOutput.SetVolume(volume);
        }

        /// <summary>
        ///     Gets the position of the active track, including elapsed/remaining times
        /// </summary>
        /// <returns>A track position object</returns>
        public TrackPosition GetTrackPosition()
        {
            WaitForLock();
            lock (MixerLock)
            {
                return GetPositionNoLock();
            }
        }

        private TrackPosition GetPositionNoLock()
        {
            var position = new TrackPosition();
            var track = ActiveTrack;
            if (track == null) return position;

            position.Track = track;

            position.ChannelPosition = AudioStreamHelper.GetPosition(track);
            position.Positition = GetAdjustedPosition(position.ChannelPosition, track);

            if (NextTrack != null)
                position.Length = track.ActiveLength;
            else
                position.Length = track.FadeOutEnd - track.FadeInStart + track.AdditionalEndLoopLength;
            return position;
        }

        /// <summary>
        ///     Gets the adjusted position.
        /// </summary>
        /// <param name="channelPosition">The channel position.</param>
        /// <param name="track">The track.</param>
        /// <returns>The adjusted position</returns>
        private static long GetAdjustedPosition(long channelPosition, Track track)
        {
            var position = channelPosition - track.FadeInStart;

            if (track.IsLoopedAtStart)
                if (track.CurrentStartLoop < track.StartLoopCount)
                    position += track.CurrentStartLoop * track.FadeInLength;
                else
                    position += track.AdditionalStartLoopLength;

            if (track.HasSkipSection && channelPosition > track.SkipStart)
                position -= track.SkipLength;

            return position;
        }

        private static long GetAdjustedPosition(Track track)
        {
            if (track == null) return 0;
            var channelPosition = AudioStreamHelper.GetPosition(track);
            return GetAdjustedPosition(channelPosition, track);
        }

        private static double GetAdjustedPositionSeconds(Track track)
        {
            return track?.SamplesToSeconds(GetAdjustedPosition(track)) ?? 0;
        }

        /// <summary>
        ///     Sets the current track position.
        /// </summary>
        /// <param name="position">The position.</param>
        public void SetAdjustedTrackPosition(long position)
        {
            // DebugHelper.WriteLine("Set track position");
            var track = ActiveTrack;
            if (track == null) return;

            StopTrackFxSend();

            track.CurrentEndLoop = 0;

            var actualPosition = position;

            var newCurrentStartLoop = track.CurrentStartLoop;
            if (track.IsLoopedAtStart)
                if (actualPosition >= track.FullStartLoopLength)
                {
                    newCurrentStartLoop = track.StartLoopCount;
                    actualPosition = actualPosition - track.AdditionalStartLoopLength;
                }
                else
                {
                    newCurrentStartLoop = (int) (actualPosition / track.FadeInLength);
                    actualPosition = actualPosition - newCurrentStartLoop * track.FadeInLength;
                    if (newCurrentStartLoop > 0 && actualPosition == 0)
                        actualPosition = 1; // avoid start-fade event when looping
                }

            actualPosition = actualPosition + track.FadeInStart;

            if (track.HasSkipSection && actualPosition > track.SkipStart)
                actualPosition = actualPosition + track.SkipLength;

            if (actualPosition < track.FadeInStart || actualPosition > track.FadeOutStart)
                return;

            track.CurrentStartLoop = newCurrentStartLoop;
            AudioStreamHelper.SetPosition(track, actualPosition);

            if (PreviousTrack != null) AudioStreamHelper.Pause(PreviousTrack);
        }

        /// <summary>
        ///     Sets the current track position.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        public void SetTrackPosition(double seconds)
        {
            // DebugHelper.WriteLine("Set track position");

            var track = ActiveTrack;
            if (track == null) return;

            var posistion = track.SecondsToSamples(seconds);

            SetAdjustedTrackPosition(posistion);
        }

        /// <summary>
        ///     Sets the default fade out settings.
        /// </summary>
        public void SetConservativeFadeOutSettings()
        {
            if (CurrentTrack == null || NextTrack == null) return;
            if (CurrentTrack.PowerDownOnEnd) return;

            var fadeType = ExtendedFadeType.QuickFade;

            if (KeyHelper.GetKeyMixRank(CurrentTrack.Key, NextTrack.Key) < 3)
                fadeType = ExtendedFadeType.PowerDown;
            else if (!BpmHelper.IsBpmInRange(CurrentTrack.EndBpm, NextTrack.StartBpm, 10))
                fadeType = ExtendedFadeType.EchoOut;

            SetAutoFadeOutSettings(fadeType);
        }

        private void SetAutoFadeOutSettings(ExtendedFadeType fadeType, Track track = null)
        {
            if (track == null)
                track = CurrentTrack;
            if (track == null)
                return;

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

            SetTrackSyncPositions(track);
        }


        /// <summary>
        ///     Adds to the mixer channel, and sets the sync points.
        /// </summary>
        /// <param name="track">The track to sync.</param>
        private void AddTrackToMixer(Track track)
        {
            if (track == null) return;

            // DebugHelper.WriteLine("Add track to mixer " + track.Description);

            WaitForLock();
            Lock();

            track.ResetPowerDownOnEnd();


            // load audio data if not loaded
            if (!track.IsAudioLoaded())
                LoadTrackAudioData(track);

            if (track != PreviousTrack && track != CurrentTrack)
                lock (MixerLock)
                {
                    // add the new track to the mixer (in paused mode)
                    AudioStreamHelper.AddToMixer(track, _currentTrackMixer);

                    // set track sync event
                    track.SyncProc = OnTrackSync;
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

            Unlock();
        }

        /// <summary>
        ///     Resets the track sync positions.
        /// </summary>
        /// <param name="track">The track.</param>
        public void ResetTrackSyncPositions(Track track)
        {
            SetTrackSyncPositions(track);
        }

        /// <summary>
        ///     Resets the track sync positions.
        /// </summary>
        public void ResetTrackSyncPositions()
        {
            ExtenedAttributesHelper.LoadExtendedAttributes(PreviousTrack);
            ExtenedAttributesHelper.LoadExtendedAttributes(CurrentTrack);
            ExtenedAttributesHelper.LoadExtendedAttributes(NextTrack);

            SetTrackSyncPositions(PreviousTrack);
            SetTrackSyncPositions(CurrentTrack);
            SetTrackSyncPositions(NextTrack);
        }

        /// <summary>
        ///     Reloads the track.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void ReloadTrack(string filename)
        {
            var track =
                CachedTracks.FirstOrDefault(
                    t => string.Equals(t.Filename, filename, StringComparison.CurrentCultureIgnoreCase));
            if (track == null) return;

            LoadTagData(track);
            ExtenedAttributesHelper.LoadExtendedAttributes(track);

            if (IsTrackInUse(track))
                SetTrackSyncPositions(track);
        }

        /// <summary>
        ///     Sets the track sync positions.
        /// </summary>
        /// <param name="track">The track.</param>
        private void SetTrackSyncPositions(Track track)
        {
            if (track == null) return;

            ClearTrackSyncPositions(track);

            // DebugHelper.WriteLine("Set track sync positions " + track.Description);

            // set end track sync
            SetTrackSync(track, 0, SyncType.TrackEnd);

            // set fade-in sync events
            SetTrackSync(track, track.FadeInStart, SyncType.StartFadeIn);
            SetTrackSync(track, track.FadeInEnd, SyncType.EndFadeIn);

            // set fade-out sync events
            SetTrackSync(track, track.FadeOutStart, SyncType.StartFadeOut);
            SetTrackSync(track, track.FadeOutEnd, SyncType.EndFadeOut);

            // set pre-fade-in start (of next track) sync event
            if (NextTrack != null && NextTrack.UsePreFadeIn && track == CurrentTrack)
            {
                var preFadeInLength = track.SecondsToSamples(NextTrack.PreFadeInLengthSeconds);
                SetTrackSync(track, track.FadeOutStart - preFadeInLength, SyncType.StartPreFadeIn);
            }

            // set pre-fade-in start (of previous track) sync event
            if (track == NextTrack && track.UsePreFadeIn)
            {
                var preFadeInLength = CurrentTrack.SecondsToSamples(track.PreFadeInLengthSeconds);
                SetTrackSync(CurrentTrack, CurrentTrack.FadeOutStart - preFadeInLength, SyncType.StartPreFadeIn);
            }

            if (track != CurrentTrack) return;

            SetAutomationSyncPositions(track);
            if (HasExtendedMixAttributes())
                SetTrackSync(PreviousTrack, GetExtendedMixAttributes().FadeEnd, SyncType.EndExtendedMix);

            if (track.HasSkipSection)
                SetTrackSync(track, track.SkipStart, SyncType.StartSkipSection);
        }

        /// <summary>
        ///     Sets the track sync positions.
        /// </summary>
        private void SetTrackSyncPositions()
        {
            // set track sync points
            SetTrackSyncPositions(CurrentTrack);
            SetTrackSyncPositions(NextTrack);
        }

        /// <summary>
        ///     Sets a track sync.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="position">The position.</param>
        /// <param name="syncType">Type of the sync.</param>
        /// <returns>The sync Id</returns>
        private static int SetTrackSync(Track track, long position, SyncType syncType)
        {
            if (!track.IsAudioLoaded()) throw new Exception("Track audio not loaded");

            var flags = BASSSync.BASS_SYNC_POS | BASSSync.BASS_SYNC_MIXTIME;

            if (syncType == SyncType.TrackEnd)
            {
                flags = BASSSync.BASS_SYNC_END;
                position = 0;
            }

            lock (track)
            {
                var syncId = BassMix.BASS_Mixer_ChannelSetSync(track.ChannelId,
                    flags,
                    position,
                    track.SyncProc,
                    new IntPtr((int) syncType));

                switch (syncType)
                {
                    case SyncType.StartPreFadeIn:
                        track.PreFadeInStartSyncId = syncId;
                        break;
                    case SyncType.StartFadeIn:
                        track.FadeInStartSyncId = syncId;
                        break;
                    case SyncType.EndFadeIn:
                        track.FadeInEndSyncId = syncId;
                        break;
                    case SyncType.StartFadeOut:
                        track.FadeOutStartSyncId = syncId;
                        break;
                    case SyncType.EndFadeOut:
                        track.FadeOutEndSyncId = syncId;
                        break;
                    case SyncType.TrackEnd:
                        track.TrackEndSyncId = syncId;
                        break;
                    case SyncType.EndRawLoop:
                        track.RawLoopEndSyncId = syncId;
                        break;
                    case SyncType.EndExtendedMix:
                        track.ExtendedMixEndSyncId = syncId;
                        break;
                    case SyncType.StartTrackFxTrigger:
                        break;
                    case SyncType.EndTrackFxTrigger:
                        break;
                    case SyncType.StartSampleTrigger:
                        break;
                    case SyncType.EndSampleTrigger:
                        break;
                    case SyncType.StartSkipSection:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(syncType), syncType, null);
                }

                return syncId;
            }
        }

        /// <summary>
        ///     Clears the track sync positions.
        /// </summary>
        /// <param name="track">The track.</param>
        private void ClearTrackSyncPositions(Track track)
        {
            // DebugHelper.WriteLine("Clear track sync positions " + track.Description);

            if (track.IsAudioLoaded())
            {
                if (track.FadeInStartSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.ChannelId, track.FadeInStartSyncId);
                    track.FadeInStartSyncId = int.MinValue;
                }
                if (track.FadeInEndSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.ChannelId, track.FadeInEndSyncId);
                    track.FadeInEndSyncId = int.MinValue;
                }
                if (track.FadeOutStartSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.ChannelId, track.FadeOutStartSyncId);
                    track.FadeOutStartSyncId = int.MinValue;
                }
                if (track.FadeOutEndSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.ChannelId, track.FadeOutEndSyncId);
                    track.FadeInStartSyncId = int.MinValue;
                }
                if (track.PreFadeInStartSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.ChannelId, track.PreFadeInStartSyncId);
                    track.PreFadeInStartSyncId = int.MinValue;
                }
                if (track.TrackEndSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.ChannelId, track.TrackEndSyncId);
                    track.TrackEndSyncId = int.MinValue;
                }
                if (track.RawLoopEndSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.ChannelId, track.RawLoopEndSyncId);
                    track.TrackEndSyncId = int.MinValue;
                }
                if (track.ExtendedMixEndSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.ChannelId, track.ExtendedMixEndSyncId);
                    track.ExtendedMixEndSyncId = int.MinValue;
                }
                if (track.SkipSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.ChannelId, track.SkipSyncId);
                    track.SkipSyncId = int.MinValue;
                }
            }

            ClearAutomationSyncPositions(track);
        }

        /// <summary>
        ///     Unloads the track stream data.
        /// </summary>
        /// <param name="track">The track to unload.</param>
        private void RemoveTrackFromMixers(Track track)
        {
            if (track == null) return;
            if (!track.IsAudioLoaded()) return;

            AudioStreamHelper.Pause(track);

            // DebugHelper.WriteLine("Remove track from mixer " + track.Description);

            ClearTrackSyncPositions(track);

            UnloadTrackAudioData(track);

            track.SyncProc = null;
        }

        /// <summary>
        ///     Determines whether a track is currently in use as the next/current/previous track
        /// </summary>
        /// <param name="track">The track to check.</param>
        /// <returns>True if the track is in use; otherwise, false.</returns>
        public bool IsTrackInUse(Track track)
        {
            // no track, so not in use
            return track != null && GetInUseTracks().Any(t => TrackHelper.IsSameTrack(track, t));
        }

        private IEnumerable<Track> GetInUseTracks()
        {
            var tracks = new List<Track>();

            if (CurrentTrack != null) tracks.Add(CurrentTrack);
            if (NextTrack != null) tracks.Add(NextTrack);
            if (PreviousTrack != null) tracks.Add(PreviousTrack);
            if (PreloadedTrack != null) tracks.Add(PreloadedTrack);

            return tracks;
        }

        /// <summary>
        ///     Called when the StartFadeIn sync is fired
        /// </summary>
        private void StartFadeIn()
        {
            if (CurrentTrack == null) return;

            // DebugHelper.WriteLine("Start fade-in:" + CurrentTrack.Description);

            Lock();

            SetTrackSyncPositions();

            if (!CurrentTrack.IsAudioLoaded()) LoadTrackAudioData(CurrentTrack);

            // set loop count
            CurrentTrack.CurrentStartLoop = 0;

            // set position
            AudioStreamHelper.SetPosition(CurrentTrack, CurrentTrack.FadeInStart);

            var startVolume = CurrentTrack.FadeInStartVolume;
            var endVolume = CurrentTrack.FadeInEndVolume;

            if (PreviousTrack != null && PreviousTrack.PowerDownOnEnd)
            {
                startVolume = CurrentTrack.FadeInEndVolume;
                endVolume = 1F;
            }

            // set the volume slide
            AudioStreamHelper.SetVolumeSlide(CurrentTrack, startVolume, endVolume,
                GetFadeLength(PreviousTrack, CurrentTrack));

            // set to standard pitch
            AudioStreamHelper.SetPitch(CurrentTrack, 100);

            // start playing the track
            AudioStreamHelper.Play(CurrentTrack);

            SetDelayByBpm();

            Unlock();

            // DebugHelper.WriteLine("End Start fade-in:" + CurrentTrack.Description);
        }

        /// <summary>
        ///     Called when the EndFadeIn sync is fired
        /// </summary>
        private void EndFadeIn()
        {
            // DebugHelper.WriteLine("End fade-in:" + CurrentTrack.Description);

            Lock();

            var isLooped = CurrentTrack.IsLoopedAtStart;
            var maxLoops = CurrentTrack.StartLoopCount;
            var currentLoop = CurrentTrack.CurrentStartLoop;
            var loopForever = LoopFadeInForever;

            // set to loop start if necessary
            if (isLooped && currentLoop < maxLoops - 1 || loopForever)
            {
                //var message = $"Looping fade-in section - starting loop {currentLoop + 1} of {maxLoops}";
                // DebugHelper.WriteLine(message);

                CurrentTrack.CurrentStartLoop++;
                if (loopForever && CurrentTrack.CurrentStartLoop == maxLoops) CurrentTrack.CurrentStartLoop = 0;

                // set position (add one to void StartFadeIn firing)
                AudioStreamHelper.SetPosition(CurrentTrack, CurrentTrack.FadeInStart + 1);
                Unlock();
            }
            else
            {
                // set volume to full
                AudioStreamHelper.SetVolume(CurrentTrack, 1F);
                Unlock();

                if (!IsManualMixMode || IsManualMixMode && PreviousManaulExtendedFadeType == ExtendedFadeType.PowerDown)
                    RaiseOnEndFadeIn();

                UnloadUnusedAudioData();
            }

            // DebugHelper.WriteLine("END End fade-in");
        }

        private void Unlock()
        {
            _locked = false;
        }

        private void Lock()
        {
            WaitForLock();
            _locked = true;
        }

        private void WaitForLock()
        {
            while (IsLocked())
                Thread.Sleep(50);
        }

        public bool IsLocked()
        {
            return _locked;
        }

        /// <summary>
        ///     Called when the StartFadeOut sync is fired
        /// </summary>
        private void StartFadeOut()
        {
            // DebugHelper.WriteLine("Start fade-out");

            Lock();

            var oldPreviousTrack = PreviousTrack;

            PreviousTrack = CurrentTrack;
            CurrentTrack = NextTrack;
            NextTrack = null;

            PreviousManaulExtendedFadeType = CurrentManualExtendedFadeType;
            CurrentManualExtendedFadeType = GetCurrentExtendedFadeType();

            if (!IsTrackInUse(oldPreviousTrack))
                RemoveTrackFromMixers(oldPreviousTrack);

            if (CurrentTrack != null)
                RaiseOnTrackChange();
            else
                PlayState = PlayState.Stopped;

            if (PreviousTrack != null)
            {
                // set loop count
                PreviousTrack.CurrentEndLoop = 0;

                // change tempo if required
                AudioStreamHelper.SetTrackTempoToMatchAnotherTrack(PreviousTrack, CurrentTrack);

                if (IsManualMixMode)
                {
                    SetAutoFadeOutSettings(PreviousManaulExtendedFadeType, PreviousTrack);
                    switch (PreviousManaulExtendedFadeType)
                    {
                        case ExtendedFadeType.Cut:
                            AudioStreamHelper.SetVolumeSlide(PreviousTrack, PreviousTrack.FadeOutStartVolume, 0,
                                GetCutFadeLength(CurrentTrack));
                            StopRecordingAutoExtendedMix();
                            break;
                        case ExtendedFadeType.QuickFade:
                            AudioStreamHelper.SetVolumeSlide(PreviousTrack, PreviousTrack.FadeOutStartVolume, 0,
                                GetQuickFadeLength(CurrentTrack));
                            StopRecordingAutoExtendedMix();
                            break;
                        case ExtendedFadeType.EchoOut:
                            AudioStreamHelper.SetVolumeSlide(PreviousTrack, PreviousTrack.FadeOutStartVolume, 0,
                                BpmHelper.GetDefaultLoopLength(CurrentTrack.StartBpm) / 32 * 8);
                            StopRecordingAutoExtendedMix();
                            break;
                        case ExtendedFadeType.PowerDown:
                            AudioStreamHelper.PowerDown(PreviousTrack);
                            StopRecordingAutoExtendedMix();
                            break;
                        case ExtendedFadeType.Default:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    if (HasExtendedMixAttributes())
                    {
                        var mixAttributes = GetExtendedMixAttributes();
                        switch (mixAttributes.ExtendedFadeType)
                        {
                            case ExtendedFadeType.Default:
                                if (PreviousTrack.PowerDownOnEnd)
                                    AudioStreamHelper.PowerDown(PreviousTrack);
                                else
                                    AudioStreamHelper.SetVolumeSlide(PreviousTrack, PreviousTrack.FadeOutStartVolume,
                                        mixAttributes.FadeEndVolume, mixAttributes.FadeLength);
                                break;
                            case ExtendedFadeType.PowerDown:
                                AudioStreamHelper.PowerDown(PreviousTrack);
                                break;
                            case ExtendedFadeType.Cut:
                                AudioStreamHelper.SetVolumeSlide(PreviousTrack, PreviousTrack.FadeOutStartVolume, 0,
                                    GetCutFadeLength(CurrentTrack));
                                StopRecordingAutoExtendedMix();
                                break;
                            case ExtendedFadeType.QuickFade:
                                AudioStreamHelper.SetVolumeSlide(PreviousTrack, PreviousTrack.FadeOutStartVolume, 0,
                                    GetQuickFadeLength(CurrentTrack));
                                StopRecordingAutoExtendedMix();
                                break;
                            case ExtendedFadeType.EchoOut:
                                SetAutoFadeOutSettings(mixAttributes.ExtendedFadeType, PreviousTrack);
                                AudioStreamHelper.SetVolumeSlide(PreviousTrack, PreviousTrack.FadeOutStartVolume, 0,
                                    BpmHelper.GetDefaultLoopLength(CurrentTrack.StartBpm) / 32 * 8);
                                StopRecordingAutoExtendedMix();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else if (PreviousTrack.PowerDownOnEnd)
                    {
                        AudioStreamHelper.PowerDown(PreviousTrack);
                    }
                    else if (IsForceFadeNowMode)
                    {
                        AudioStreamHelper.SetVolumeSlide(PreviousTrack, PreviousTrack.FadeOutStartVolume,
                            PreviousTrack.FadeOutEndVolume, PreviousTrack.FadeOutLengthSeconds);
                    }
                    else
                    {
                        // set the volume slide
                        AudioStreamHelper.SetVolumeSlide(PreviousTrack, PreviousTrack.FadeOutStartVolume,
                            PreviousTrack.FadeOutEndVolume, GetFadeLength());
                    }
                }
            }

            Unlock();

            if (CurrentTrack != null)
                StartFadeIn();

            RefreshSamples();
        }

        /// <summary>
        ///     Determines whether the mix between the previous/current tracks has extended mix attributes
        /// </summary>
        public bool HasExtendedMixAttributes(string fadeOutTrackDescription, string fadeInTrackDescription)
        {
            return GetExtendedMixAttributes(fadeOutTrackDescription, fadeInTrackDescription) != null;
        }

        /// <summary>
        ///     Determines whether the mix between the previous/current tracks has extended mix attributes
        /// </summary>
        public bool HasExtendedMixAttributes(Track fadeOutTrack, Track fadeInTrack)
        {
            return GetExtendedMixAttributes(fadeOutTrack, fadeInTrack) != null;
        }

        /// <summary>
        ///     Determines whether the mix between the previous/current tracks has extended mix attributes
        /// </summary>
        private bool HasExtendedMixAttributes()
        {
            return GetExtendedMixAttributes() != null && GetExtendedMixAttributes().FadeLength != 0;
        }

        /// <summary>
        ///     Gets the extended mix attributes.
        /// </summary>
        /// <returns>The extended mix attributes.</returns>
        private ExtendedMixAttributes GetExtendedMixAttributes()
        {
            return GetExtendedMixAttributes(PreviousTrack, CurrentTrack);
        }

        /// <summary>
        ///     Gets the extended mix attributes.
        /// </summary>
        /// <param name="fadeOutTrack">The fade out track.</param>
        /// <param name="fadeInTrack">The fade in track.</param>
        /// <returns>The extended mix attributes.</returns>
        public ExtendedMixAttributes GetExtendedMixAttributes(Track fadeOutTrack, Track fadeInTrack)
        {
            if (fadeOutTrack == null || fadeInTrack == null) return null;
            return GetExtendedMixAttributes(fadeOutTrack.Description, fadeInTrack.Description);
        }

        /// <summary>
        ///     Gets the extended mix attributes.
        /// </summary>
        /// <param name="fadeOutTrackDescription">The fade out track description.</param>
        /// <param name="fadeInTrackDescription">The fade in track description.</param>
        /// <returns>The extended mix attributes.</returns>
        private ExtendedMixAttributes GetExtendedMixAttributes(string fadeOutTrackDescription,
            string fadeInTrackDescription)
        {
            var attributes = AutomationAttributesHelper.GetAutomationAttributes(fadeOutTrackDescription);
            return attributes?.GetExtendedMixAttributes(fadeInTrackDescription);
        }

        private static double GetCutFadeLength(Track track)
        {
            if (track == null) return 0D;
            return BpmHelper.GetDefaultLoopLength(track.StartBpm) / 16D;
        }

        private static double GetQuickFadeLength(Track track)
        {
            if (track == null) return 0D;
            return BpmHelper.GetDefaultLoopLength(track.EndBpm) / 2D;
        }

        private static double GetPowerDownFadeLength(Track track)
        {
            if (track == null) return 0D;
            return BpmHelper.GetDefaultLoopLength(track.EndBpm) / 4D;
        }

        private double GetFadeLength()
        {
            return GetFadeLength(PreviousTrack, CurrentTrack);
        }

        private double GetFadeLength(Track fromTrack, Track toTrack)
        {
            if (fromTrack == null && toTrack == null) return 0F;

            if (toTrack == null) return fromTrack.SamplesToSeconds(fromTrack.FullEndLoopLength);
            if (fromTrack == null) return BpmHelper.GetDefaultLoopLength(toTrack.StartBpm) / 4D;

            var fadeInLength = toTrack.FullStartLoopLengthSeconds;
            if (fadeInLength == 0)
                fadeInLength = BpmHelper.GetDefaultLoopLength(toTrack.StartBpm);

            var fadeOutLength = GetExtendedFadeOutLength(fromTrack, toTrack);

            if (!HasExtendedMixAttributes(fromTrack, toTrack))
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
        ///     Completes the fade-out.
        /// </summary>
        private void EndFadeOut()
        {
            // DebugHelper.WriteLine("End fade-out");

            IsForceFadeNowMode = false;

            if (PreviousTrack == null) return;

            var maxLoops = GetActiveEndLoopCount();
            var isLooped = PreviousTrack.IsLoopedAtEnd;
            var currentLoop = PreviousTrack.CurrentEndLoop;
            var loopForever = isLooped && (IsManualMixMode || HasExtendedMixAttributes());

            // set to loop start if necessary
            if (isLooped && currentLoop < maxLoops - 1 || loopForever)
            {
                //var message = $"Looping fade-out section - starting loop {currentLoop + 1} of {maxLoops}";
                // DebugHelper.WriteLine(message);

                PreviousTrack.CurrentEndLoop++;

                // set position (add one to void StartFadeIn firing)
                AudioStreamHelper.SetPosition(PreviousTrack, PreviousTrack.FadeOutStart + 1);
            }
            else if (!IsManualMixMode && !HasExtendedMixAttributes())
            {
                // stop track
                AudioStreamHelper.Pause(PreviousTrack);
            }
        }

        private void EndExtendedMix()
        {
            if (IsManualMixMode) return;
            if (PreviousTrack == null) return;
            if (!HasExtendedMixAttributes()) return;

            //var message = "Current End Loop: " + PreviousTrack.CurrentEndLoop + "  Fade End Loop: " + GetExtendedMixAttributes().FadeEndLoop;
            // DebugHelper.WriteLine(message);

            var mixAttributes = GetExtendedMixAttributes();
            if (mixAttributes.FadeEndLoop != 0 && PreviousTrack.CurrentEndLoop != mixAttributes.FadeEndLoop) return;

            if (GetExtendedMixAttributes().PowerDownAfterFade)
                AudioStreamHelper.PowerDown(PreviousTrack);
            else
                AudioStreamHelper.Pause(PreviousTrack);
        }

        private int GetActiveEndLoopCount()
        {
            return PreviousTrack.EndLoopCount;
        }

        /// <summary>
        ///     Starts the pre-fade.
        /// </summary>
        private void StartPreFadeIn()
        {
            // DebugHelper.WriteLine("Start pre-fade-in");

            if (NextTrack == null || !NextTrack.UsePreFadeIn) return;

            if (!NextTrack.IsAudioLoaded()) LoadTrackAudioData(NextTrack);

            // set position
            AudioStreamHelper.SetPosition(NextTrack, NextTrack.PreFadeInStart);

            // set the volume slide
            AudioStreamHelper.SetVolumeSlide(NextTrack, NextTrack.PreFadeInStartVolume * NextTrack.FadeInStartVolume,
                NextTrack.FadeInStartVolume, NextTrack.PreFadeInLength);

            // set to standard pitch
            AudioStreamHelper.SetPitch(NextTrack, 100);

            // start playing the track
            AudioStreamHelper.Play(NextTrack);

            // DebugHelper.WriteLine("End Start pre-fade-in");
        }

        /// <summary>
        ///     Raises the on-track-change event.
        /// </summary>
        private void RaiseOnTrackChange()
        {
            // DebugHelper.WriteLine("Track change event");
            //Task.Run(() =>
            //{
            OnTrackChange?.Invoke(CurrentTrack, EventArgs.Empty);
            SetDelayByBpm();
            //});
        }

        /// <summary>
        ///     Raises the OnEndFadeIn
        /// </summary>
        private void RaiseOnEndFadeIn()
        {
            // DebugHelper.WriteLine("End fade in event");
            //Task.Run(() => {
                OnEndFadeIn?.Invoke(CurrentTrack, EventArgs.Empty);
            //});
        }

        /// <summary>
        ///     Raises the OnSkipToEnd
        /// </summary>
        private void RaiseOnSkipToEnd()
        {
            // DebugHelper.WriteLine("Skip to end event");
            //Task.Run(() => {
                OnSkipToEnd?.Invoke(CurrentTrack, EventArgs.Empty);
            //});
        }

        /// <summary>
        ///     Raises the on-track-queued
        /// </summary>
        private void RaiseOnTrackQueued()
        {
            // DebugHelper.WriteLine("Track queued event");
            //Task.Run(() =>
            //{
            OnTrackQueued?.Invoke(CurrentTrack, EventArgs.Empty);
            //});
        }

        /// <summary>
        ///     Skips to the start of the current track
        /// </summary>
        public void SkipToStart()
        {
            if (CurrentTrack == null) return;
            SetAdjustedTrackPosition(0);
            if (PreviousTrack != null)
            {
                AudioStreamHelper.Pause(PreviousTrack);
                AudioStreamHelper.ResetTempo(PreviousTrack);
            }
            if (NextTrack != null)
            {
                AudioStreamHelper.Pause(NextTrack);
                AudioStreamHelper.ResetTempo(NextTrack);
            }
            AudioStreamHelper.ResetTempo(CurrentTrack);
        }

        /// <summary>
        ///     Skips to the end of the current track
        /// </summary>
        public void SkipToEnd()
        {
            if (CurrentTrack == null) return;

            AudioStreamHelper.ResetTempo(CurrentTrack);
            if (PreviousTrack != null)
            {
                AudioStreamHelper.ResetTempo(PreviousTrack);
                AudioStreamHelper.Pause(PreviousTrack);
            }
            if (NextTrack != null)
                AudioStreamHelper.ResetTempo(NextTrack);

            SetTrackPosition(CurrentTrack.ActiveLengthSeconds - 0.01D);

            RaiseOnSkipToEnd();
        }

        /// <summary>
        ///     Skips to the end of the current track
        /// </summary>
        public void SkipToFadeOut()
        {
            if (CurrentTrack == null) return;

            AudioStreamHelper.ResetTempo(CurrentTrack);
            if (PreviousTrack != null)
            {
                AudioStreamHelper.ResetTempo(PreviousTrack);
                AudioStreamHelper.Pause(PreviousTrack);
            }
            if (NextTrack != null)
                AudioStreamHelper.ResetTempo(NextTrack);

            SetTrackPosition(CurrentTrack.ActiveLengthSeconds -
                             BpmHelper.GetDefaultLoopLength(CurrentTrack.EndBpm) / 2);

            RaiseOnSkipToEnd();
        }

        /// <summary>
        ///     Skips the skip section.
        /// </summary>
        private void StartSkipSection()
        {
            // DebugHelper.WriteLine("Start skip section");

            if (CurrentTrack == null || !CurrentTrack.HasSkipSection) return;

            // set position
            AudioStreamHelper.SetPosition(CurrentTrack, CurrentTrack.SkipEnd);
        }


        /// <summary>
        ///     Called when the track sync event is fired
        /// </summary>
        private void OnTrackSync(int handle, int channel, int data, IntPtr pointer)
        {
            var syncType = (SyncType) pointer.ToInt32();
            var track = GetInUseTracks().FirstOrDefault(x => x.ChannelId == channel);
            //var description = track != null ? "Track: " + track.Description : "";

            // DebugHelper.WriteLine("Event Fired: " + syncType + " " + description);

            switch (syncType)
            {
                case SyncType.StartPreFadeIn:
                    StartPreFadeIn();
                    break;
                case SyncType.StartFadeOut:
                    // Start-Fade-Out also calls start fade in.
                    // Old track fade out & new track fade-in
                    // always start at same time.
                    if (TrackHelper.IsSameTrack(track, CurrentTrack))
                        StartFadeOut();
                    break;
                case SyncType.EndFadeOut:
                    EndFadeOut();
                    break;
                case SyncType.EndFadeIn:
                    EndFadeIn();
                    break;
                case SyncType.EndRawLoop:
                    PlayRawLoop();
                    break;
                case SyncType.StartTrackFxTrigger:
                    StartTrackFxTrigger();
                    break;
                case SyncType.EndTrackFxTrigger:
                    StopTrackFxTrigger();
                    break;
                case SyncType.StartSampleTrigger:
                    StartSampleTrigger();
                    break;
                case SyncType.EndSampleTrigger:
                    StopSampleTrigger();
                    break;
                case SyncType.EndExtendedMix:
                    EndExtendedMix();
                    break;
                case SyncType.StartSkipSection:
                    StartSkipSection();
                    break;
                case SyncType.TrackEnd:
                    break;
                case SyncType.StartFadeIn:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Enumeration style representing different types of sync events.
        /// </summary>
        private enum SyncType
        {
            /// <summary>
            ///     track end sync event type
            /// </summary>
            TrackEnd = 0,

            /// <summary>
            ///     Start fade-in sync event type
            /// </summary>
            StartFadeIn = 1,

            /// <summary>
            ///     End fade-in sync event type
            /// </summary>
            EndFadeIn = 2,

            /// <summary>
            ///     Start fade-out sync event type
            /// </summary>
            StartFadeOut = 3,

            /// <summary>
            ///     End fade-out sync event type
            /// </summary>
            EndFadeOut = 4,

            /// <summary>
            ///     Start pre-fade-in sync event type
            /// </summary>
            StartPreFadeIn = 5,

            /// <summary>
            ///     End the loop section in raw-loop mode
            /// </summary>
            EndRawLoop = 6,

            /// <summary>
            ///     Start track effect sync event type
            /// </summary>
            StartTrackFxTrigger = 7,

            /// <summary>
            ///     End track effect sync event type
            /// </summary>
            EndTrackFxTrigger = 8,

            /// <summary>
            ///     Start sample sync event type
            /// </summary>
            StartSampleTrigger = 9,

            /// <summary>
            ///     End sample sync event type
            /// </summary>
            EndSampleTrigger = 10,

            /// <summary>
            ///     End of the extended mix
            /// </summary>
            EndExtendedMix = 11,

            /// <summary>
            ///     Start of the skip section
            /// </summary>
            StartSkipSection = 12
        }
    }
}