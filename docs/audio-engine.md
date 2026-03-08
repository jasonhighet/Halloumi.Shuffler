# Audio Engine

The audio engine lives entirely in `Halloumi.BassEngine/`. It wraps the [Bass.Net](https://www.un4seen.com/) library (Un4seen BASS) for all audio streaming, mixing, and DSP work.

The entry point for almost everything is `BassPlayer`, a partial class split across five files.

---

## BassPlayer.cs — Core playback

`Halloumi.BassEngine/BassPlayer/BassPlayer.cs`

### Track queue

BassPlayer maintains up to four track slots at once:

| Property | Meaning |
|---|---|
| `CurrentTrack` | The track currently fading in / playing |
| `PreviousTrack` | The track currently fading out |
| `NextTrack` | The track queued to play after Current finishes |
| `PreloadedTrack` | A track preloaded in advance to reduce seek latency |
| `ActiveTrack` | Whichever of Current/Previous is still audible |

### Playback control

```
Play()                        Start or resume playback
Pause()                       Pause playback
Stop()                        Stop and reset
JumpBack()                    Rewind within current track
QueueTrack(Track)             Set the next track
SetPositionByPercentage(pct)  Seek within current track
GetCurrentBpm()               Dynamic BPM interpolated from position
```

### Crossfade configuration

All fade values are set as properties on `BassPlayer`. They define defaults applied when loading a track that has no extended attributes set yet.

| Property | Default | Meaning |
|---|---|---|
| `DefaultFadeLength` | 10 s | Cross-fade duration |
| `DefaultFadeInStartVolume` | 50 % | Volume at start of fade-in |
| `DefaultFadeInEndVolume` | 100 % | Volume at end of fade-in |
| `DefaultFadeOutStartVolume` | 80 % | Volume at start of fade-out |
| `DefaultFadeOutEndVolume` | 0 % | Volume at end of fade-out |
| `LimitSongLength` | false | Truncate tracks to `MaxSongLength` |
| `MaxSongLength` | 5 min | Maximum track duration when limiting |
| `LoopFadeInForever` | false | Loop the fade-in section indefinitely |

### Mix modes

`IsManualMixMode` — when true the crossfade volume is driven by a UI slider via `SetManualMixVolume()` rather than the automated fade curve.

### Events

| Event | Fires when |
|---|---|
| `OnTrackChange` | A new `CurrentTrack` starts |
| `OnTrackQueued` | A track is added to the queue |
| `OnEndFadeIn` | The fade-in section completes |
| `OnSkipToEnd` | A force-skip is triggered |
| `TrackTagsLoaded` | MP3 tag metadata has been read for a track |
| `OnVolumeChanged` | Master volume changes |
| `OnManualMixModeChanged` | Mix mode toggled |

### Master volume

`Volume` — decimal 0–100. Affects the main output level. Separate from individual track fade curves.

---

## BassPlayer.Plugins.cs — Effects plugins

`Halloumi.BassEngine/BassPlayer/BassPlayer.Plugins.cs`

### Plugin slots

There are seven plugin slots organised by which mixer channel they sit on:

| Slot | Property | Channel |
|---|---|---|
| Main 1 | `MainVstPlugin` | Main mixer |
| Main 2 | `MainVstPlugin2` | Main mixer |
| Sampler 1 | `SamplerVstPlugin` | Sampler |
| Sampler 2 | `SamplerVstPlugin2` | Sampler |
| Track | `TrackVstPlugin` | Track mixer |
| Track FX 1 | `TrackSendFxVstPlugin` | FX send |
| Track FX 2 | `TrackSendFxVstPlugin2` | FX send |

`WaPlugin` — a single WinAmp DSP plugin loaded globally.

### Loading / unloading

```
LoadWaPlugin(location)
LoadMainVstPlugin(location, index)       # index 1 or 2
LoadSamplerVstPlugin(location, index)
LoadTracksVstPlugin(location, index)
LoadTrackSendFxvstPlugin(location, index)

ClearMainVstPlugin(index)
ClearSamplerVstPlugin(index)
ClearTracksVstPlugin(index)
ClearTrackSendFxvstPlugin(index)

UnloadAllVstPlugins()
UnloadAllWaPlugins()
```

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

## BassPlayer.Automation.cs — Position-triggered automation

`Halloumi.BassEngine/BassPlayer/BassPlayer.Automation.cs`

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

`LastTrackFxTrigger` / `LastTrackFxTriggerTrack` — the most recently captured trigger, used before persisting.

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
StopRecordingManualExtendedMix(powerDown)  Finish recording; optionally power-down
SaveExtendedMix()                    Persist the curve to extended attributes
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

---

## BassPlayer.Sampler.cs — Sample and loop playback

`Halloumi.BassEngine/BassPlayer/BassPlayer.Sampler.cs`

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

## BassPlayer.RawLoop.cs — Section looping

`Halloumi.BassEngine/BassPlayer/BassPlayer.RawLoop.cs`

Raw loop mode lets you define a start/end boundary within a track and loop that section continuously. Useful for extending a breakdown or building tension before a mix.

### API

```
LoadRawLoopTrack(filename)             Load an audio file for raw looping
UnloadRawLoopTrack()                   Unload the current raw loop track
SetRawLoopPositions(start, end, offset)  Define boundaries and initial offset
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
LoopFadeInIndefinitely  Loop forever flag
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

`BassPlayer.Automation.cs` attaches BASS sync callbacks (`StartSyncId`, `EndSyncId`) to each track instance for automation triggers. These are internal and managed automatically.

---

## Helper classes

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

### VolumeAnalyzer
Real-time volume analysis using FFT data for the level meters.

### BeatDetector / BeatDetector2
Analyses audio data to detect beat positions and calculate BPM. `BeatDetector2` is an improved version.

### SectionDetector / SectionDetector2
Identifies structural sections (intro, verse, chorus, outro) by analysing energy/frequency patterns.

---

## MIDI

`BassPlayerMidiMapper` maps incoming MIDI messages to `BassPlayer` methods. Mappings are defined in `MidiMapping.cs` and serialised as JSON. `MidiManager` handles device enumeration and raw MIDI input events.
