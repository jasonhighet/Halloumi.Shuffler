# Audio Engine

The audio engine lives entirely in `Halloumi.BassEngine/`. It wraps the [Bass.Net](https://www.un4seen.com/) library (Un4seen BASS) for all audio streaming, mixing, and DSP work.

The entry point for almost everything is `BassPlayer`. It is a facade with no logic of its own: each public member passes through to an internal component (see [BassPlayer internals](#bassplayer-internals)), and the BASS-free decisions live in [Mixing](#mixing--transition-decisions-and-position-maths). The sections below describe the public API by feature area, naming the component behind each.

---

## Core playback

`Playback.cs` (tracks loaded by `TrackLoader.cs`)

### Track queue

BassPlayer keeps up to four track slots (in `TrackDeck`); three are public:

| Property | Meaning |
|---|---|
| `CurrentTrack` | The track currently fading in / playing |
| `PreviousTrack` | The track currently fading out |
| `NextTrack` | The track queued to play after Current finishes |

The fourth, the preloaded track (`PreloadTrack(filename)`), has its audio loaded ahead of time so queueing it is quick.

### Playback control

```
Play()                          Start or resume playback
Pause()                         Pause playback
Stop()                          Stop and reset
ForcePlay(filename | track)     Stop, clear the slots and play this track now
QueueTrack(filename | track)    Queue as next (or current if nothing is playing)
JumpBack()                      Rewind half a loop within the current track
SetAdjustedTrackPosition(pos)   Seek (position counted from the fade-in, see TrackTimeline)
SkipToEnd() / SkipToFadeOut()   Jump to the end, or to SkipToFadeOutBeats beats before the fade-out
                                (a pre-fade-in that starts before that point is skipped)
GetCurrentBpm()                 Dynamic BPM interpolated from position
```

### Crossfade configuration

These set the fades a track gets when it has no saved fade points (they are held in `FadeDefaults`):

| Property | Default | Meaning |
|---|---|---|
| `LimitSongLength` | false | Truncate tracks to `MaxSongLength` |
| `MaxSongLength` | 5 min | Maximum track duration when limiting |
| `LoopFadeInForever` | false | Loop the fade-in section indefinitely |

The default fade volumes (fade-in 50 % → 100 %, fade-out 80 % → 0 %) are fixed in `FadeDefaults`.

### Mix modes

`IsManualMixMode` — when true the crossfade volume is driven by a UI slider via `SetManualMixVolume()` rather than the automated fade curve.

### Events

| Event | Fires when |
|---|---|
| `OnTrackChange` | A new `CurrentTrack` starts |
| `OnTrackQueued` | A track is added to the queue |
| `OnEndFadeIn` | The fade-in section completes |
| `OnSkipToEnd` | A force-skip is triggered |
| `OnVolumeChanged` | Master volume changes |
| `OnManualMixModeChanged` | Mix mode toggled |

### Master volume

`Volume` — decimal 0–100. Affects the main output level. Separate from individual track fade curves.

### Mono output

`Mono` — bool. When true, the speaker and monitor outputs each get a `MonoDownmix` DSP that averages left and right into both channels. On `SpeakerOutputChannel` it runs at the lowest priority, after any VST or WinAmp plug-ins on the main output, so they still receive stereo. On `MonitorOutputChannel` it runs just before the `DSP_StreamCopy` (priority -1000) that sends the mix to the headphone device. Toggled from **Options → Play In Mono** and saved as the `Mono` setting.

---

## Effects plugins and the track FX send

`MixingDesk.cs` (plug-in slots), `TrackFxSend.cs` (the send)

### Plugin slots

There are seven VST plugin slots, each a `PluginChannel` and an index on that channel. `VstPluginSlot.All` lists them:

| Slot | `PluginChannel`, index | Named property | Channel |
|---|---|---|---|
| Main 1 | `Main`, 0 | `MainVstPlugin` | Main mixer |
| Main 2 | `Main`, 1 | `MainVstPlugin2` | Main mixer |
| Sampler 1 | `Sampler`, 0 | `SamplerVstPlugin` | Sampler |
| Sampler 2 | `Sampler`, 1 | `SamplerVstPlugin2` | Sampler |
| Track | `Tracks`, 0 | `TrackVstPlugin` | Track mixer |
| Track FX 1 | `TrackSendFx`, 0 | `TrackSendFxVstPlugin` | FX send |
| Track FX 2 | `TrackSendFx`, 1 | `TrackSendFxVstPlugin2` | FX send |

`WaPlugin` — a single WinAmp DSP plugin loaded globally.

### Loading / unloading

```
GetVstPlugin(channel, index)
LoadVstPlugin(channel, location, index)    # index is 0-based
ClearVstPlugin(channel, index)
LoadWaPlugin(location)
UnloadAllWaPlugins()
StopWaPluginsForExit()
```

A WinAmp plug-in is detached from the speaker channel before it is stopped, so the mixer thread never calls a stopped plug-in. `StopWaPluginsForExit` (used on exit and by `Dispose`) stops it without unloading its DLL: iZotope Ozone still runs code after stopping, and unloading the DLL under it crashed the process. On exit `frmMain` saves settings, the library, mix ratings and the loop library before stopping the plug-in. Mix ratings are saved in the background after every rating, so on exit they are only saved if something changed since (`MixLibrary.SaveIfChanged`). Each step's time goes to `%APPDATA%\Halloumi Shuffler\Halloumi.Shuffler.Shutdown.log` (start-up: `Halloumi.Shuffler.Startup.log`).

The named members (`LoadMainVstPlugin`, `ClearSamplerVstPlugin`, `MainVstPlugin2`, ...) are one-line wrappers over these, kept for `frmPluginSettings`. To add a slot, add it to `VstPluginSlot.All`; to add a channel, add it to `PluginChannel` and to `GetPluginHost`.

### Track FX send

The FX send channel lets you route the track audio through an effects chain and blend the result back in. Sends are triggered with a configurable note-fraction delay (synced to BPM):

| Method | Delay |
|---|---|
| `StartTrackFxSendHalf()` | 1/2 note |
| `StartTrackFxSendQuarter()` | 1/4 note |
| `StartTrackFxSendEighth()` | 1/8 note |
| `StartTrackFxSendDottedQuarter()` | Dotted 1/4 |
| `StartTrackFxSendDottedEighth()` | Dotted 1/8 |
| `StartTrackFxSendSixteenth()` | 1/16 note |
| `StopTrackFxSend()` | Deactivate |

---

## Position-triggered automation

`AutomationPlayer.cs` (triggers), `ManualMixer.cs` (manual and extended mixes), `Playback.cs` (force fades)

Automation lets you record and replay two types of trigger at specific positions within a track:

- **TrackFXTrigger** — activates the FX send channel at a given position, with a configured delay note
- **SampleTrigger** — plays a specific sample at a given position

Triggers are stored in per-track extended attribute XML files and loaded when a track is queued. Playback uses `BASS_ChannelSetSync` so the callback fires at the exact sample position — no timer polling.

### Track FX triggers

```
SaveLastTrackFxTrigger()             Persist the currently recorded trigger
ClearTrackFxTriggers(track)          Remove all FX triggers for a track
RemovePreviousTrackFxTrigger()       Delete a specific trigger
StartTrackFxTrigger()                (internal) Activate FX at trigger position
StopTrackFxSend()                    Deactivate the FX send
```

The trigger being recorded is held in `TriggerRecorder` until it is saved.

### Sample triggers

```
SaveLastSampleTrigger()              Persist the currently recorded trigger
ClearSampleTriggers()                Remove all sample triggers for current track
RemovePreviousSampleTrigger()        Delete a specific trigger
StartSampleTrigger()                 (internal) Play sample at trigger position
StopSampleTrigger()                  Stop triggered sample
```

### Manual / extended mix

When `IsManualMixMode` is active the user drives the crossfade by hand. The resulting volume curve can be saved as an "extended mix":

```
SetManualMixVolume(value)            Drive crossfade volume manually (0–100)
GetManualMixVolume()                 Query current manual mix volume
PausePreviousTrack()                 Stop the outgoing track; records the mix as it stands
PowerOffPreviousTrack()              Power down the outgoing track; records the mix, ending in a power-down
SaveExtendedMix()                    Persist the recorded mix to the automation attributes
ClearExtendedMix()                   Remove saved curve
```

### Force fades

```
ForceFadeNow(ForceFadeType)          Immediately trigger one of:
                                       Cut          — abrupt stop
                                       PowerDown    — power-down audio effect
                                       QuickFade    — fast crossfade
                                       SkipToEnd    — jump to fade-out point
PowerOffPreviousTrack()              Play power-down on previous track
PowerOffCurrentTrack()               Play power-down on current track
PausePreviousTrack()                 Smoothly pause previous track
```

### Extended fade types

`ExtendedMixAttributes` on a track can specify a non-default fade type:

| Type | Behaviour |
|---|---|
| `Default` | Standard linear fade curve |
| `PowerDown` | Applies power-down pitch effect during fade |
| `Cut` | Abrupt cut instead of fade |
| `QuickFade` | Accelerated fade curve |
| `EchoOut` | Loops a short slice of the fade-out eight times while fading |

How each type is carried out is decided in `TransitionPlanner` (see Mixing below).

---

## Sample and loop playback

`Sampler.cs`

Samples and loops play on a dedicated mixer channel (`SamplerMixer`) separate from the track channel so they can be routed and level-controlled independently.

### Output routing

`SamplerOutput` and `TrackOutput` control which physical output each channel routes to (e.g. speakers vs monitor).

### API

```
PlaySample(Sample)              Play a sample by instance
PlaySample(int sampleIndex)     Play sample by index in the loaded list
PlaySample(int trackIndex, int sampleIndex)   Play sample for a specific track
PauseSample(Sample)             Stop a sample
PauseSample(int sampleIndex)    Stop sample by index
StopSamples()                   Pause all samples
GetSamples()                    List currently loaded samples
LinkLoopSampleToTrack(loopKey, track)   Associate an external loop with a track
```

Sample sources:
- Samples linked to the current or next track via extended attributes
- External WAV loop files from the configured `LoopFolder`

---

## Audio diagnostics

`AudioDiagnostics.cs`, reached through `BassPlayer.CaptureDiagnostics(ms)` and `ShufflerApplication.CaptureAudioDiagnostics()`. In the app: **Help > Audio Diagnostics** (Ctrl+Shift+D). Press it while a glitch (such as clicking) is happening.

It watches the speaker output for 3 seconds, then lists:
- **Output buffer**: minimum and average buffered audio, how often it was below 30 ms, and any stalled readings. Also BASS CPU and the garbage collections during the window. A buffer that runs low means dropouts (the audio thread isn't keeping up). A buffer that stays full means the glitch is in the audio itself.
- **Streams**: every track slot and every other track with audio loaded. For each: whether it is actually playing in its mixer (`BASS_Mixer_ChannelIsActive`), its volume, tempo, position and end loop. Also any samples that are playing.
- **Mixers and plug-ins**: mixer volumes (including whether the track FX send is open) and the loaded VST and WinAmp plug-ins.
- **Process**: memory, GDI objects and handles.
- **Notes**: what looks wrong, such as a track playing that isn't in any slot, the previous track still on its end loop, or the FX send left open.

The report is shown in a message box and appended to `%APPDATA%\Halloumi Shuffler\Halloumi.Shuffler.AudioDiagnostics.log`. It runs on a worker thread and changes nothing.

---

## Section looping

`RawLooper.cs`

Raw loop mode lets you define a start/end boundary within a track and loop that section continuously. Useful for extending a breakdown or building tension before a mix.

### API

```
LoadRawLoopTrack(filename)             Load an audio file for raw looping
UnloadRawLoopTrack()                   Unload the current raw loop track
SetRawLoopPositions(start, end, offset)  Define boundaries and initial offset
UpdateRawLoopPositions(start, end)     Move the boundaries of a playing loop without restarting it
PlayRawLoop()                          Start looping from offset
StopRawLoop()                          Pause raw loop playback
```

`RawLoopOutput` — output routing for the raw loop channel.

### Behaviour

- First playback starts at `offset` within the loop
- Subsequent loops restart from `start`
- `end` is the loop boundary; playback wraps back to `start` when reached

---

## Track model (engine)

`Halloumi.BassEngine/Models/Track.cs` — runtime track representation. Extends `AudioStream`.

### Fade section properties

```
FadeInStart / FadeInEnd            Sample positions bounding the fade-in
FadeInLength / FadeInLengthSeconds Fade-in duration
FadeInStartVolume / FadeInEndVolume  Volume envelope (0.0–1.0)

FadeOutStart / FadeOutEnd          Sample positions bounding the fade-out
FadeOutLength / FadeOutLengthSeconds
FadeOutStartVolume / FadeOutEndVolume
```

### Loop properties

```
StartLoopCount          How many times to repeat the fade-in section
CurrentStartLoop        Current iteration counter
EndLoopCount            How many times to repeat the fade-out section
CurrentEndLoop          Current iteration counter
IsLoopedAtStart / IsLoopedAtEnd   Quick boolean checks
FullStartLoopLength / FullEndLoopLength  Total looped section length
```

### BPM properties

```
StartBpm / EndBpm    BPM at the track edges (set from extended attributes)
Bpm                  Average of StartBpm and EndBpm
TagBpm               BPM read from the MP3 ID3 tag
BpmAdjustmentRatio   Manual multiplier for BPM correction
```

### Other sections

```
SkipStart / SkipEnd / SkipLength    Optional skip section
PreFadeInStart / PreFadeInLength    Pre-fade section played before fade-in
ActiveLength / ActiveLengthSeconds  Main audible section length
PowerDownOnEnd                      Play power-down effect on fade-out
```

### Internal sync handles

Each track's BASS sync callbacks are held in its `Syncs` (a `TrackSyncs`, see below). Tracks and triggers no longer carry their own sync ids.

---

## BassPlayer internals

`Halloumi.BassEngine/BassPlayer/` — the internal classes `BassPlayer` delegates to. None of them is visible outside the engine.

### Components

`BassPlayer`'s constructor builds them in this order, each getting only what it needs:

| Component | Owns | Uses |
|---|---|---|
| `MixingDesk` | every `MixerChannel`, the output splitters, plug-in slots | — |
| `TrackLoader` | `TrackCache`, `FadeDefaults` | `MixingDesk` (to remove tracks from mixers) |
| `TrackFxSend` | — (drives the send channels) | `MixingDesk`, `TrackDeck`, `TriggerRecorder` |
| `Sampler` | `TrackSamplePlayer` | `MixingDesk`, `TrackDeck`, `TriggerRecorder` |
| `RawLooper` | the raw-loop track | `MixingDesk`, `TrackLoader`, `SyncDispatcher` |
| `AutomationPlayer` | — | `TrackDeck`, `TrackFxSend`, `Sampler`, `TriggerRecorder`, `SyncDispatcher`, and a callback to `Playback.ResetTrackSyncPositions` |
| `ManualMixer` | manual-mix mode and fade types, extended-mix recording and saving | `TrackDeck`, `FadeDefaults`, `TriggerRecorder`, and a callback for whether the player is playing |
| `Playback` | `_slotLock`, play state, force-fade state | all of the above, and the `BackgroundQueue` |
| `AudioDiagnostics` | — (read-only; takes no locks) | `MixingDesk`, `TrackDeck`, `TrackLoader`, `Sampler` |

`BackgroundQueue` runs work handed off by sync handlers, in order, on a worker thread (see [Threads and locking](#threads-and-locking)).

`TrackDeck` and `TriggerRecorder` are shared: the deck says which tracks are where, the recorder holds automation being recorded until it is saved.

### SyncDispatcher — routing syncs to components

BASS calls one callback (`SyncDispatcher.Callback`, set as each track's `SyncProc`) for every sync. The dispatcher looks up the handler registered for the sync type and calls it with the in-use track the sync fired on (null if none, e.g. the raw-loop track). Components register their handlers in their constructors:

| Component | Sync types |
|---|---|
| `Playback` | `StartPreFadeIn`, `StartFadeOut`, `EndFadeOut`, `EndFadeIn`, `EndExtendedMix`, `StartSkipSection` |
| `AutomationPlayer` | `StartTrackFxTrigger`, `EndTrackFxTrigger`, `StartSampleTrigger`, `EndSampleTrigger` |
| `RawLooper` | `EndRawLoop` |

`TrackEnd` and `StartFadeIn` are set but have no handler. Registering a second handler for a type throws.

### TrackDeck — the track slots

Owns `Previous`, `Current`, `Next` and `Preloaded`, and is the only place they change. Compound changes happen under a lock, so a sync callback never sees a half-made change.

```
Queue(track, out replacedNext)   Becomes current if there is none, otherwise next; true if it became current
Advance()                        Start of a transition: current → previous, next → current; returns the old previous
ClearPlaying()                   Empty the previous, next and current slots; returns what was in them
SetPreloaded(track)
InUse() / IsInUse(track) / FindByChannel(channelId)
```

### TrackCache — loaded tracks

Every track the player has loaded, by filename (case-insensitive), plus the last five loaded, whose audio is kept even when they are not in a slot. Owned by `TrackLoader`. `GetUnloadable(isInUse)` gives the tracks whose audio `UnloadUnusedAudioData` frees.

Default fade points are applied by `FadeDefaults` in `Mixing/` (`ResetFades` before a track's saved attributes load, `FillMissingFadePoints` after). `BassPlayer`'s `LimitSongLength` and `MaxSongLength` read and write it.

### TrackSyncs and TrackSyncType — position callbacks

Every BASS sync on a track goes through `track.Syncs`:

```
Set(type, position)   Set a sync. Replaces the existing one of that type, except trigger types, which a track can have many of
Remove(types...)      Remove every sync of the given types
Clear()               Remove all of them (only forgets them if the audio is already unloaded)
```

`UnloadTrackAudioData` clears a track's syncs. To add a sync: add a value to `TrackSyncType`, call `track.Syncs.Set` where it belongs (for the slot tracks, `Playback.SetTrackSyncPositions`), and register a handler with `syncs.On(type, ...)` in the component that handles it. If a track can have several at once, add it to `TrackSyncs.AllowsMany`.

### Adding a feature

Make a component class in this folder that owns its channel(s) and state. Give it only the components it needs in its constructor, register its sync handlers there, add it to `BassPlayer`'s constructor, and add one-line pass-throughs to `BassPlayer` for whatever the UI needs.

### TriggerRecorder — recording automation

Holds what is being recorded until it is saved: a track FX send (`StartTrackFxSend` / `StopTrackFxSend` → `SaveLastTrackFxTrigger`), a sample (`PlaySample` / `PauseSample` → `SaveLastSampleTrigger`) and a manual extended mix (`PausePreviousTrack` / `PowerOffPreviousTrack`, or an automatic fade type in manual mode → `SaveExtendedMix`). Saving writes the automation attributes. Positions are passed in, so it makes no BASS calls.

---

## Mixing — transition decisions and position maths

`Halloumi.BassEngine/Mixing/`

These classes hold logic that used to be inline in `BassPlayer`. They use only track metadata and make no BASS calls, so they can be run and checked without playing audio. With no channel loaded, a `Track` converts at 1 sample = 1 ms.

### TrackTimeline

Converts between a track's **channel position** (where BASS is in the file) and its **adjusted position** (where the listener is in the mix: counted from the fade-in start, including start loops already played, leaving out the skip section).

```
ToAdjustedPosition(track, channelPosition)        Channel → adjusted (uses the track's current start loop)
TryToChannelPosition(track, adjusted, out channel, out startLoop)
                                                  Adjusted → channel; false if outside fade-in start .. fade-out start
```

`BassPlayer.GetTrackPosition` and `SetAdjustedTrackPosition` use these.

### TransitionPlanner

Decides how one track hands over to the next. `BassPlayer` looks up the extended mix attributes for the pair and passes them in (null = no recorded mix).

```
PlanFadeOut(outgoing, incoming, mix, isManualMixMode, manualFadeType, isForceFadeNow)
                                    → FadeOutPlan: what the outgoing track's volume does
GetFadeLength(from, to, mix)        Crossfade length: shorter of incoming fade-in and outgoing fade-out
GetExtendedFadeOutLength(from, to, mix)
ShapeFadeOut(track, fadeType)       Reshape a track's fade-out section for a fade type (metadata only)
GetConservativeFadeType(cur, next)  PowerDown if keys clash, EchoOut if tempos far apart, else QuickFade
GetForceFadeLength(track, type)     Length for ForceFadeNow
GetCut/Quick/PowerDown/EchoOutFadeLength(track)
```

`PlanFadeOut` checks these in order: manual mix mode, a recorded extended mix (with a non-zero length), the track's `PowerDownOnEnd`, a "fade now" request, and otherwise a normal crossfade.

`FadeOutPlan` holds an `Action` (`None`, `VolumeSlide` or `PowerDown`), the slide's start volume, end volume and length, an optional `ReshapeAs` fade type, `EndsAutoMixRecording`, and `HighPass`. `BassPlayer.StartFadeOut` builds the plan and `ApplyFadeOutPlan` carries it out. Carrying it out means reshaping the track and resetting its syncs, cutting its bass if `HighPass` is set, starting the slide or power-down, and finishing extended-mix recording.

`HighPass` is set on every fade-out: normal crossfades, "fade now", manual mix mode and recorded extended mixes (and power-downs, where it makes no audible difference). It puts a 150 Hz high-pass filter (BASS_FX biquad) on the outgoing track's own stream, not on a mixer, because both tracks play through `TrackMixer`. The filter is removed when the track next starts fading in, and is freed with the stream when its audio is unloaded.

To add a fade type: add it to `ExtendedFadeType`, then handle it in `ShapeFadeOut` and in the two private planners (`PlanManualFadeOut`, `PlanExtendedMixFadeOut`).

---

## Helper classes

### BeatAnalysis
`Halloumi.BassEngine/Helpers/BeatAnalysis.cs`

Tempo detection, beat positions and fade suggestions from the audio itself. Adapted from TalentScout's `BpmDetector`: spectral-flux onset envelope → autocorrelation → comb-filter tempo scoring → fine tuning by fitting a beat grid. The grid fit gives the position of the beats as well as the tempo. It never writes to the file's tags.

```
BeatAnalysis.Analyse(filename [, cancellation])   Analyse a whole file (about a second per track; run it off the UI thread)
BeatAnalysis.Analyse(filename, start, end)        Analyse a range (5 seconds or more)
BeatAnalysis.DetectBpm(filename, start, end)      Tempo of a range of a file (use 20 seconds or more)

DetectBpm(start, end)             Tempo of a range, 70–180 BPM (0 = none found)
GetGrid(start, end, bpmHint)      Beat grid (tempo + position of a beat + confidence) fitted to a range
GetGridAt(seconds)                Beat grid of the 40 seconds around a position (cached)
SnapToBeat(seconds)               Nearest beat or half-beat
SnapEndToBeat(start, end)         Moves an end so the section is a whole number of beats long
GetFadeInCandidates(profile, count)                          Suggested fade-in positions, best first
GetFadeOutCandidates(fadeInStart, fadeInLength, profile, count)   Suggested fade-out positions for that fade-in
Save(filename) / BeatAnalysis.Load(filename)                 Cache an analysis (used by the test harness)
```

`FadeProfile` (same file) says where fade sections usually go for a kind of track; `FadeProfile.Create(fadeInStarts, playedLengths)` builds one from sections that have already been set. See [fade-suggestions.md](fade-suggestions.md).

Things to know:

- Tempos are returned as detected (70–180). Pass them through `BpmHelper.NormaliseBpm` before using them as Shuffler BPMs.
- A detected grid is sometimes half a beat out (locked on to the off-beats), so snapping uses half-beats: the real beat is always one of the snap positions.
- `BeatGrid.Confidence` is how much the beats stand out from the audio around them. Below `MinSnapConfidence` (2.5) the snap methods return the position unchanged.
- Measured against hand-set fade points: on tracks with a steady beat the snapped position is typically within 10 ms of the hand-set one and the tempo within 0.05 BPM. Loosely played live recordings are much less reliable.

### BpmHelper
`Halloumi.BassEngine/Helpers/BpmHelper.cs`

Utilities for BPM-aware calculations:

```
IsBpmInRange(bpm1, bpm2, variance)          Is bpm2 within variance% of bpm1?
GetAbsoluteBpmPercentChange(bpm1, bpm2)     Absolute % difference (0–100)
GetBpmPercentChange(bpm1, bpm2)             Signed % change
GetAdjustedBpmPercentChange(bpm1, bpm2)     Accounts for octave (half/double) variations
GetAdjustedBpmAverage(bpm1, bpm2)           Smart average handling octave jumps

GetLoopLengths(bpm)                         5 standard loop lengths (4/8/16/32/64 beats)
GetDefaultLoopLength(bpm)                   16-beat loop length in samples
GetBestFitLoopLength(bpm, preferred)        Closest standard loop to preferred length
GetBpmFromLoopLength(loopLength)            BPM from loop duration

NormaliseBpm(bpm)                           Scale to 70–136.5 by halving or doubling
GetDefaultDelayLength(bpm)                  1/4-note delay in milliseconds

GetLengthAdjustedToMatchAnotherTrack(t1, t2, len)   Adjust length for BPM change
GetTrackTempoChangeAsRatio(t1, t2)          Ratio to match second track's tempo
GetAdjustedAudioLength(len, srcBpm, tgtBpm) Adjust sample count for BPM change
```

### AudioStreamHelper
Wraps BASS stream creation, seeking, and metadata extraction.

### AudioDataHelper
FFT and waveform data extraction from streams.

---

## MIDI

`BassPlayerMidiMapper` maps incoming MIDI messages to `BassPlayer` methods. Mappings are defined in `MidiMapping.cs` and serialised as JSON. `MidiManager` handles device enumeration and raw MIDI input events.

---

## Known issues and gotchas

### Threads and locking

Three kinds of thread touch the slot tracks:

| Thread | Runs |
|---|---|
| UI (or MIDI) | the public methods |
| BASS mixer thread | sync handlers, at the exact sample each sync is set for |
| `BackgroundQueue` worker | work handed off by sync handlers, in order |

`Playback._slotLock` covers changes to the slots and to the slot tracks' channels (position, volume, play/pause, syncs). The rules, each learnt from a deadlock or a missed transition found by stress testing:

- **The mixer thread never waits for `_slotLock`.** BASS holds its own lock on the mixer while a sync handler runs, so a thread holding `_slotLock` can be stuck inside a BASS call waiting for that handler: if the handler then waited for `_slotLock`, neither would move. Sync handlers go through `Playback.RunSyncHandler`, which takes the lock if it is free and otherwise runs the work on the background queue a few milliseconds later.
- **Hold `_slotLock` only for quick work.** Never while decoding audio (`QueueTrack` and `PreloadTrack` decode first), never while raising an event.
- **Sync handlers hand off what can wait.** Events (`OnTrackChange`, `OnEndFadeIn`), freeing audio and reloading samples go to the background queue. So those events arrive on a worker thread, just after the transition starts.
- **Only touch syncs that change.** `TrackSyncs` keeps a sync that is re-set at the same position. Removing and re-adding a sync while the mixer is working through that part of the track can make BASS miss it.
- **"Fade now" starts the transition directly.** A sync set just ahead of the play position can already be behind what BASS has decoded.
- **The mixer thread never sleeps.** Sync handlers pause with `AudioStreamHelper.PauseWithoutWaiting`, not `Pause` (which sleeps 10 ms so callers that change the stream straight after don't get heard). The raw looper's `EndRawLoop` handler only moves the position. Today the 200 ms output buffer hides a short stall; with ASIO's few-millisecond buffer every stall is a click.
- **Nothing reads the output channel.** Level meters and the spectrum visual read `SpeakerOutput.Tap` (`OutputTap`): a DSP at the very end of the output chain (lowest priority, after the mono fold-down) copies each block into a ring buffer without locking or allocating. `BassPlayer.VisualsChannelId` is a decode channel served from that copy, delayed by what the output has buffered so it matches what is heard. `BASS_ChannelGetData`/`GetLevel` on the output itself would take audio out of the mix once the output is a decode channel pulled by ASIO.
- **No `BASS_ChannelLock` around removing a source from a mixer** (`AudioStreamHelper.RemoveFromMixer`). It deadlocked with BASS's update thread, which locks the mixers above first.

Lock order: `_slotLock`, then the deck's, a track's syncs', or a track's own lock; never `_slotLock` while holding one of those. A track's syncs are only changed with `_slotLock` held (the raw-loop track's excepted: the mixer thread never changes them).

What is still possible: if a transition's track was not loaded in advance (normally it is, when queued), the mixer thread loads it, which takes the track's own lock; if the UI were loading the same track at that moment, the two could wait on each other through BASS. This needs the next track to be queued and not loaded, which the queueing code prevents.

### Static shared state

The track cache is per player, but `AudioEngineHelper` (BASS start-up), `AudioDataHelper` (the loaded file buffers), `ExtendedAttributesHelper` and `AutomationAttributesHelper` are static. The application is designed for a single `BassPlayer` instance.

### A track that has just faded out cannot be replayed straight away

Once a track has finished fading out, its channel no longer reports a position (`-1`). Queueing that same track again while it is still the previous track (or before its audio is unloaded) gives silence: `AddTrackToMixer` skips tracks that are already current or previous. `ForcePlay` works, because it unloads and reloads the audio.

### Automation trigger positions are not kept between lookups

`AutomationAttributesHelper.GetAutomationAttributes` returns a clone each time. `SetAutomationSyncPositions` sets `StartSample` on its clone, but `GetCurrentTrackFxTrigger` / `GetCurrentSampleTrigger` fetch a fresh clone where `StartSample` is still 0. So "the trigger nearest the play position" does not really use the position. The syncs fire at the right times; it is which trigger's settings (delay notes, sample) are used that can be wrong when a track has several.

### Thread.Sleep(10) in AudioStreamHelper.Pause

`AudioStreamHelper.Pause()` sleeps 10 milliseconds after calling `BASS_Mixer_ChannelPause`. This is a deliberate small settle wait to give BASS time to drain its output buffer before callers assume the channel is silent. The `PowerDownAsync` loop also uses `Thread.Sleep(interval)` intentionally to pace its pitch-slide animation — do not remove those.

### Events can arrive on any thread

`OnTrackChange` and `OnEndFadeIn` raised by a transition arrive on a background worker thread; raised by a UI call (`QueueTrack`, `ForcePlay`) they arrive on the calling thread. UI handlers must marshal with `Invoke`/`BeginInvoke` (they all do).
