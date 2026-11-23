using System;
using System.Collections.Generic;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.AudioEngine.Plugins;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     The audio engine's public face. It builds the components below and passes calls through
    ///     to them; it holds no logic of its own.
    ///     <list type="bullet">
    ///         <item><see cref="MixerGraph" /> - every mixer channel, output routing, plug-in slots</item>
    ///         <item><see cref="TrackLoader" /> - loading tracks and their audio</item>
    ///         <item><see cref="Playback" /> - track slots, play/pause, seeking, transitions</item>
    ///         <item><see cref="ManualMixer" /> - manual mix mode and extended-mix recording</item>
    ///         <item><see cref="TrackFxSend" /> - the track FX send</item>
    ///         <item><see cref="Sampler" /> - samples</item>
    ///         <item><see cref="AutomationPlayer" /> - playing and editing saved automation</item>
    ///         <item><see cref="RawLooper" /> - looping a section of a track</item>
    ///     </list>
    ///     BASS syncs reach the components through <see cref="SyncDispatcher" />; each component
    ///     registers the sync types it handles.
    /// </summary>
    public class BassPlayer : IDisposable, IBpmProvider
    {
        private readonly MixerGraph _graph;
        private readonly TrackDeck _deck = new TrackDeck();
        private readonly TriggerRecorder _recorder = new TriggerRecorder();
        private readonly TrackLoader _loader;
        private readonly TrackFxSend _fxSend;
        private readonly Sampler _sampler;
        private readonly RawLooper _rawLooper;
        private readonly AutomationPlayer _automation;
        private readonly ManualMixer _manualMixer;
        private readonly BackgroundQueue _background = new BackgroundQueue();
        private readonly Playback _playback;

        public BassPlayer()
            : this(IntPtr.Zero)
        {
        }

        public BassPlayer(IntPtr windowHandle)
        {
            AudioEngineHelper.StartAudioEngine(windowHandle);

            _graph = new MixerGraph(this);
            var syncs = new SyncDispatcher(_deck.FindByChannel);

            _loader = new TrackLoader(_graph);
            _fxSend = new TrackFxSend(_graph, _deck, _recorder);
            _sampler = new Sampler(this, _graph, _deck, _recorder);
            _rawLooper = new RawLooper(_graph, _loader, syncs);
            _automation = new AutomationPlayer(syncs, _deck, _fxSend, _sampler, _recorder,
                () => _playback.ResetTrackSyncPositions());
            _manualMixer = new ManualMixer(_deck, _loader.FadeDefaults, _recorder,
                () => _playback.PlayState == PlayState.Playing);
            _playback = new Playback(_graph, _deck, _loader, syncs, _automation, _sampler, _fxSend, _manualMixer,
                _background);

            _sampler.SamplesChanged += (sender, e) => OnTrackSamplesChanged?.Invoke(this, e);

            ExtendedAttributesHelper.ShufflerFolder = "";
        }

        public void Dispose()
        {
            _playback.Stop();

            // let queued work (events, freeing audio) finish before the audio is freed
            _background.WaitUntilIdle(2000);

            _graph.UnloadAllWaPlugins();
            _graph.UnloadAllVstPlugins();

            _loader.UnloadAll();

            AudioEngineHelper.StopAudioEngine();
        }

        // ---------- events ----------

        /// <summary>
        ///     Raised when a new track becomes current. The sender is the current track. When a
        ///     transition causes it, it arrives on a background thread just after the transition starts.
        /// </summary>
        public event EventHandler OnTrackChange
        {
            add { _playback.TrackChanged += value; }
            remove { _playback.TrackChanged -= value; }
        }

        /// <summary>Raised when a track is queued. The sender is the current track.</summary>
        public event EventHandler OnTrackQueued
        {
            add { _playback.TrackQueued += value; }
            remove { _playback.TrackQueued -= value; }
        }

        /// <summary>
        ///     Raised when the current track's fade-in ends (on a background thread), or when a manual
        ///     mix is recorded.
        /// </summary>
        public event EventHandler OnEndFadeIn
        {
            add
            {
                _playback.FadeInEnded += value;
                _manualMixer.MixRecorded += value;
            }
            remove
            {
                _playback.FadeInEnded -= value;
                _manualMixer.MixRecorded -= value;
            }
        }

        /// <summary>Raised when playback skips to the end or to the fade-out.</summary>
        public event EventHandler OnSkipToEnd
        {
            add { _playback.SkippedToEnd += value; }
            remove { _playback.SkippedToEnd -= value; }
        }

        public event EventHandler OnManualMixModeChanged
        {
            add { _manualMixer.ModeChanged += value; }
            remove { _manualMixer.ModeChanged -= value; }
        }

        public event EventHandler OnManualMixVolumeChanged
        {
            add { _manualMixer.VolumeChanged += value; }
            remove { _manualMixer.VolumeChanged -= value; }
        }

        /// <summary>Raised when the mixer volume is set. The sender is the current track.</summary>
        public event EventHandler OnVolumeChanged;

        /// <summary>Raised (on a worker thread) when the loaded samples change. The sender is this player.</summary>
        public event EventHandler OnTrackSamplesChanged;

        // ---------- tracks ----------

        public Track CurrentTrack => _deck.Current;

        public Track NextTrack => _deck.Next;

        public Track PreviousTrack => _deck.Previous;

        public Track LoadTrack(string filename) => _loader.LoadTrack(filename);

        public Track LoadTrackAndAudio(string filename) => _loader.LoadTrackAndAudio(filename);

        public bool IsTrackLoaded(string filename) => _loader.IsTrackLoaded(filename);

        public Track LoadTrackAudioData(Track track) => _loader.LoadTrackAudioData(track);

        public void UnloadTrackAudioData(Track track) => _loader.UnloadTrackAudioData(track);

        public void ReloadTrack(string filename) => _playback.ReloadTrack(filename);

        public Track QueueTrack(string filename) => _playback.QueueTrack(filename);

        public void QueueTrack(Track track) => _playback.QueueTrack(track);

        public void PreloadTrack(string filename) => _playback.PreloadTrack(filename);

        public void ClearTracks() => _playback.ClearTracks();

        // ---------- playback ----------

        public PlayState PlayState => _playback.PlayState;

        public bool IsPlaying() => _playback.IsPlaying();

        public void Play() => _playback.Play();

        public void Pause() => _playback.Pause();

        public void Stop() => _playback.Stop();

        public void ForcePlay(string filename) => _playback.ForcePlay(filename);

        public void ForcePlay(Track track) => _playback.ForcePlay(track.Filename);

        public TrackPosition GetTrackPosition() => _playback.GetTrackPosition();

        public void SetAdjustedTrackPosition(long position) => _playback.SetAdjustedTrackPosition(position);

        public void JumpBack() => _playback.JumpBack();

        public void SkipToEnd() => _playback.SkipToEnd();

        public void SkipToFadeOut(int beatsBeforeFadeOut = 8) => _playback.SkipToFadeOut(beatsBeforeFadeOut);

        public decimal GetCurrentBpm() => _playback == null ? BpmHelper.NormaliseBpm(100M) : _playback.GetCurrentBpm();

        public bool LoopFadeInForever
        {
            get { return _playback.LoopFadeInForever; }
            set { _playback.LoopFadeInForever = value; }
        }

        // ---------- fades ----------

        /// <summary>Default crossfade length in seconds, for tracks with no saved fade points.</summary>
        public double DefaultFadeLength
        {
            get { return _loader.FadeDefaults.FadeLength; }
            set { _loader.FadeDefaults.FadeLength = value; }
        }

        public bool LimitSongLength
        {
            get { return _loader.FadeDefaults.LimitSongLength; }
            set { _loader.FadeDefaults.LimitSongLength = value; }
        }

        /// <summary>Maximum song length in seconds when LimitSongLength is set.</summary>
        public double MaxSongLength
        {
            get { return _loader.FadeDefaults.MaxSongLength; }
            set { _loader.FadeDefaults.MaxSongLength = value; }
        }

        public void SetConservativeFadeOutSettings() => _playback.SetConservativeFadeOutSettings();

        public void ForceFadeNow(ForceFadeType fadeType) => _playback.ForceFadeNow(fadeType);

        public void PowerOffCurrentTrack() => _playback.PowerOffCurrentTrack();

        public bool HasExtendedMixAttributes(Track fadeOutTrack, Track fadeInTrack) =>
            _playback.HasExtendedMixAttributes(fadeOutTrack, fadeInTrack);

        public ExtendedFadeType GetExtendedFadeType(Track fromTrack, Track toTrack) =>
            _playback.GetExtendedFadeType(fromTrack, toTrack);

        public double GetExtendedFadeOutLength(Track fromTrack, Track toTrack) =>
            _playback.GetExtendedFadeOutLength(fromTrack, toTrack);

        // ---------- manual mixing ----------

        public bool IsManualMixMode => _manualMixer.IsOn;

        /// <summary>In manual mix mode, the fade type for the transition out of the current track.</summary>
        public ExtendedFadeType CurrentManualExtendedFadeType
        {
            get { return _manualMixer.FadeTypeOut; }
            set { _manualMixer.FadeTypeOut = value; }
        }

        public void ToggleManualMixMode() => _manualMixer.Toggle();

        public void EnableManualMixMode() => _manualMixer.Enable();

        public void DisableManualMixMode() => _manualMixer.Disable();

        public void SetManualMixVolume(decimal value) => _manualMixer.SetVolume(value);

        public decimal GetManualMixVolume() => _manualMixer.GetVolume();

        public void PowerOffPreviousTrack() => _manualMixer.PowerOffPreviousTrack();

        public void PausePreviousTrack() => _manualMixer.PausePreviousTrack();

        public void SaveExtendedMix() => _manualMixer.SaveMix();

        public void ClearExtendedMix() => _manualMixer.ClearMix();

        // ---------- volume and outputs ----------

        /// <summary>Gets or sets the BASS output volume (0 - 100).</summary>
        public decimal Volume
        {
            get { return AudioEngineHelper.Volume; }
            set { AudioEngineHelper.Volume = value; }
        }

        public SpeakerOutputChannel SpeakerOutput => _graph.SpeakerOutput;

        public MonitorOutputChannel MonitorOutput => _graph.MonitorOutput;

        public SoundOutput TrackOutput
        {
            get { return _graph.TrackOutput; }
            set { _graph.TrackOutput = value; }
        }

        public SoundOutput SamplerOutput
        {
            get { return _graph.SamplerOutput; }
            set { _graph.SamplerOutput = value; }
        }

        public SoundOutput RawLoopOutput
        {
            get { return _graph.RawLoopOutput; }
            set { _graph.RawLoopOutput = value; }
        }

        /// <summary>Gets the output levels (0 - 32768).</summary>
        public VolumeLevels GetVolumeLevels() => _graph.GetVolumeLevels();

        public decimal GetMixerVolume() => _graph.SpeakerOutput.GetVolume();

        public void SetMixerVolume(decimal volume)
        {
            _graph.SpeakerOutput.SetVolume(volume);
            OnVolumeChanged?.Invoke(CurrentTrack, EventArgs.Empty);
        }

        public decimal GetMonitorVolume() => _graph.MonitorOutput.GetVolume();

        public void SetMonitorVolume(decimal volume) => _graph.MonitorOutput.SetVolume(volume);

        /// <summary>Gets or sets whether the speaker and monitor outputs play in mono.</summary>
        public bool Mono
        {
            get { return _graph.Mono; }
            set { _graph.Mono = value; }
        }

        // ---------- plug-ins ----------

        public VstPlugin GetVstPlugin(PluginChannel channel, int index) => _graph.GetVstPlugin(channel, index);

        public VstPlugin LoadVstPlugin(PluginChannel channel, string location, int index) =>
            _graph.LoadVstPlugin(channel, location, index);

        public void ClearVstPlugin(PluginChannel channel, int index) => _graph.ClearVstPlugin(channel, index);

        // Named slot members, kept for the plug-in settings screen.

        public VstPlugin MainVstPlugin => GetVstPlugin(PluginChannel.Main, 0);
        public VstPlugin MainVstPlugin2 => GetVstPlugin(PluginChannel.Main, 1);
        public VstPlugin SamplerVstPlugin => GetVstPlugin(PluginChannel.Sampler, 0);
        public VstPlugin SamplerVstPlugin2 => GetVstPlugin(PluginChannel.Sampler, 1);
        public VstPlugin TrackVstPlugin => GetVstPlugin(PluginChannel.Tracks, 0);
        public VstPlugin TrackSendFxVstPlugin => GetVstPlugin(PluginChannel.TrackSendFx, 0);
        public VstPlugin TrackSendFxVstPlugin2 => GetVstPlugin(PluginChannel.TrackSendFx, 1);

        public VstPlugin LoadMainVstPlugin(string location, int index) => LoadVstPlugin(PluginChannel.Main, location, index);
        public VstPlugin LoadSamplerVstPlugin(string location, int index) => LoadVstPlugin(PluginChannel.Sampler, location, index);
        public VstPlugin LoadTracksVstPlugin(string location, int index) => LoadVstPlugin(PluginChannel.Tracks, location, index);
        public VstPlugin LoadTrackSendFxvstPlugin(string location, int index) => LoadVstPlugin(PluginChannel.TrackSendFx, location, index);

        public void ClearMainVstPlugin(int index) => ClearVstPlugin(PluginChannel.Main, index);
        public void ClearSamplerVstPlugin(int index) => ClearVstPlugin(PluginChannel.Sampler, index);
        public void ClearTracksVstPlugin(int index) => ClearVstPlugin(PluginChannel.Tracks, index);
        public void ClearTrackSendFxvstPlugin(int index) => ClearVstPlugin(PluginChannel.TrackSendFx, index);

        public WaPlugin WaPlugin => _graph.WaPlugin;

        /// <summary>
        ///     Loads a WinAmp DSP plug-in onto the main output (pausing playback while it loads).
        /// </summary>
        public WaPlugin LoadWaPlugin(string location)
        {
            var playing = PlayState == PlayState.Playing;
            if (playing) Pause();

            var plugin = _graph.LoadWaPlugin(location);

            if (playing) Play();

            return plugin;
        }

        public void UnloadAllWaPlugins() => _graph.UnloadAllWaPlugins();

        // ---------- track FX send ----------

        public decimal TrackSendFxDelayNotes
        {
            get { return _fxSend.DelayNotes; }
            set { _fxSend.DelayNotes = value; }
        }

        public void StartTrackFxSend() => _fxSend.Start();

        public void StartTrackFxSendHalf() => _fxSend.Start(0.5M);

        public void StartTrackFxSendQuarter() => _fxSend.Start(0.25M);

        public void StartTrackFxSendEighth() => _fxSend.Start(0.125M);

        public void StartTrackFxSendDottedEighth() => _fxSend.Start(0.1875M);

        public void StartTrackFxSendDottedQuarter() => _fxSend.Start(0.375M);

        public void StartTrackFxSendSixteenth() => _fxSend.Start(0.0625M);

        public void StopTrackFxSend() => _fxSend.Stop();

        // ---------- samples ----------

        public string LoopFolder
        {
            get { return _sampler.LoopFolder; }
            set { _sampler.LoopFolder = value; }
        }

        public decimal SamplerDelayNotes
        {
            get { return _sampler.DelayNotes; }
            set { _sampler.DelayNotes = value; }
        }

        public List<Sample> GetSamples() => _sampler.GetSamples();

        public void PlaySample(Sample sample) => _sampler.Play(sample);

        public void PlaySample(int sampleIndex) => _sampler.Play(_sampler.GetSample(sampleIndex));

        public void PlaySample(int trackIndex, int sampleIndex) => _sampler.Play(_sampler.GetSample(trackIndex, sampleIndex));

        public void PauseSample(Sample sample) => _sampler.Pause(sample);

        public void PauseSample(int sampleIndex) => _sampler.Pause(_sampler.GetSample(sampleIndex));

        public void PauseSample(int trackIndex, int sampleIndex) => _sampler.Pause(_sampler.GetSample(trackIndex, sampleIndex));

        public void StopSamples() => _sampler.StopAll();

        public void LinkLoopSampleToTrack(string loopKey, Track track) => _sampler.LinkLoopSampleToTrack(loopKey, track);

        // ---------- automation ----------

        /// <summary>If false, saved track FX triggers are not played.</summary>
        public bool TrackFxAutomationEnabled
        {
            get { return _automation.TrackFxEnabled; }
            set { _automation.TrackFxEnabled = value; }
        }

        /// <summary>If false, saved sample triggers are not played.</summary>
        public bool SampleAutomationEnabled
        {
            get { return _automation.SamplesEnabled; }
            set { _automation.SamplesEnabled = value; }
        }

        public void SaveLastTrackFxTrigger() => _automation.SaveTrackFxTrigger();

        public void ClearTrackFxTriggers(Track track) => _automation.ClearTrackFxTriggers(track);

        public void RemovePreviousTrackFxTrigger() => _automation.RemovePreviousTrackFxTrigger();

        public void SaveLastSampleTrigger() => _automation.SaveSampleTrigger();

        public void ClearSampleTriggers() => _automation.ClearSampleTriggers();

        public void RemovePreviousSampleTrigger() => _automation.RemovePreviousSampleTrigger();

        // ---------- raw loop ----------

        public Track RawLoopTrack => _rawLooper.Track;

        public Track LoadRawLoopTrack(string filename) => _rawLooper.Load(filename);

        public void UnloadRawLoopTrack() => _rawLooper.Unload();

        public void SetRawLoopPositions(long start, long end, long offset) => _rawLooper.SetPositions(start, end, offset);

        public void UpdateRawLoopPositions(long start, long end) => _rawLooper.UpdatePositions(start, end);

        public void PlayRawLoop() => _rawLooper.Play();

        public void StopRawLoop() => _rawLooper.Stop();
    }
}
