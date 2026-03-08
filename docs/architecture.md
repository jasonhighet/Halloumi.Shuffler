# Architecture

## Operating modes

The application serves two distinct use cases with different platform requirements:

| Mode | Description | Platform |
|---|---|---|
| **Active** | Library curation, sync/fade point setup, finding compatible tracks, performance DJing | Windows-only (Bass.Net, x86) |
| **Shuffler / Passive** | Automated playback of pre-mixed tracks — shuffled, sequenced, with playlist control | Audio host must be Windows; control UI does not have to be |

**Active mode** covers anything that requires human judgement on the audio: loading a new track and listening to it, working out where sync points and fade points should be, finding which tracks it mixes well into, and live performance mixing. All of this requires the full Windows app with direct access to the audio engine.

**Shuffler mode** covers automated playback where the mixing decisions have already been made. A remote UI can still do meaningful things here — adjusting the playlist, choosing the next track, skipping, changing volume — but it is not making real-time mixing decisions. The host process (Windows, running BassPlayer) handles all audio; remote clients send commands over an API.

The natural API boundary is therefore between work that requires audio judgement (always Active/Windows) and work that is controlling already-configured playback (can be remote).

## Solution structure

```
Halloumi.Shuffler.sln
├── Halloumi.BassEngine/          # Audio engine
├── Halloumi.Shuffler.Engine/     # Domain logic and data
├── Halloumi.Shuffler.Application/# Application orchestrator
├── Halloumi.Shuffler/            # WinForms UI
├── Halloumi.Shuffler.TestHarness/# Dev/debug harness
├── Components/                   # Pre-built DLLs (Bass.Net, Krypton, etc.)
└── packages/                     # NuGet packages
```

## Layered dependency graph

```
┌─────────────────────────────────────┐
│         Halloumi.Shuffler           │  WinForms UI
│  (Forms, Controls, Krypton theming) │
└───────────────┬─────────────────────┘
                │ references
┌───────────────▼─────────────────────┐
│    Halloumi.Shuffler.Application    │  Orchestrator / facade
│         (ShufflerApplication)       │
└────────┬──────────────┬─────────────┘
         │              │ references
┌────────▼───────┐  ┌───▼────────────────────┐
│ Halloumi.Bass  │  │ Halloumi.Shuffler.Engine│
│ Engine         │  │ (Library, MixLibrary,   │
│ (BassPlayer,   │  │  Models, Samples)       │
│  MIDI, DSP)    │  └─────────────────────────┘
└────────────────┘
```

The UI layer never talks directly to the audio engine or library — it goes through `ShufflerApplication`. The engine and library layers have no knowledge of each other.

## Key subsystems

### ShufflerApplication
Central orchestrator. Owns all major service instances and wires them together. Lives in `Halloumi.Shuffler.Application/ShufflerApplication.cs`.

**Owns:**
- `BassPlayer` — the audio mixing engine
- `Library` — track metadata database
- `MixLibrary` — track-pair compatibility ratings
- `LoopLibrary` — external WAV loop files
- `TrackSampleLibrary` — sample-to-track mappings
- `MidiManager` + `MidiMapper` — MIDI input handling

**Startup sequence:**
1. Create `BassPlayer`
2. Create `Library` (passing BassPlayer reference)
3. Create `LoopLibrary`, `MixLibrary`, `TrackSampleLibrary`
4. Load settings from disk
5. Load database (tracks, mixes, samples, automation data)
6. Configure MIDI mapping
7. Subscribe to `BassPlayer.OnTrackQueued`

### BassPlayer (audio engine)
The heart of the application. A large partial class split across five files:

| File | Responsibility |
|---|---|
| `BassPlayer.cs` | Core playback, track queue, crossfade, volume, events |
| `BassPlayer.Plugins.cs` | VST and WinAmp plugin loading/unloading |
| `BassPlayer.Automation.cs` | Position-triggered FX sends and sample playback |
| `BassPlayer.Sampler.cs` | Sample and loop playback channel |
| `BassPlayer.RawLoop.cs` | Looping of arbitrary track sections |

See [audio-engine.md](audio-engine.md) for full detail.

### Library (track database)
Manages the MP3 track collection. Reads ID3 tags, caches metadata to XML, provides filtered queries. No audio playback. See [audio-library.md](audio-library.md).

### MixLibrary
Stores a 0–5 integer rating for every track-pair transition that has been evaluated. Used by the playlist generator to avoid bad mixes and prefer excellent ones.

### LoopLibrary / TrackSampleLibrary
Two implementations of `ISampleLibrary`:
- `LoopLibrary` — loads `.wav` files from a configured folder
- `TrackSampleLibrary` — manages samples extracted from tracks and linked via extended attributes

## Mixer channel topology

```
CurrentTrack stream ──► CurrentTrackMixer ──┐
                                             │
PreviousTrack stream ─► PreviousTrackMixer ──┼──► MainMixer ──► OutputSplitter ──► SpeakerOutput
                                             │                                └──► MonitorOutput
RawLoop stream ───────► RawLoopMixer ────────┤
                                             │
Sample streams ───────► SamplerMixer ────────┘
                              │
                         TrackSendMixer (effects send, routed back into MainMixer)
```

Effects chain per channel:
- `WaPlugin` (WinAmp DSP) on the main path
- `MainVstPlugin`, `MainVstPlugin2` on the main mixer
- `SamplerVstPlugin`, `SamplerVstPlugin2` on the sampler mixer
- `TrackVstPlugin` on the track mixer
- `TrackSendFxVstPlugin`, `TrackSendFxVstPlugin2` on the FX send

## Two Track models

There are two different `Track` classes. They serve different layers and should not be confused:

| | `Halloumi.Shuffler.Engine.Models.Track` | `Halloumi.BassEngine.Models.Track` |
|---|---|---|
| Layer | Library / data | Audio engine |
| Purpose | Serialisable metadata | Runtime playback state |
| Key fields | Artist, Title, Bpm, Rank, Genre, Filename | FadeInStart/End, FadeOutStart/End, LoopCounts, SyncHandles |
| Persistence | Serialised to XML cache | In-memory only |

## Data persistence

```
%APPDATA%\Halloumi\
├── Halloumi.Shuffler.Library.xml      # All track metadata (cached from MP3 tags)
├── Halloumi.Shuffler.MixLibrary.xml   # Track-pair mix ratings
└── Halloumi.Shuffler.Settings.xml     # Application settings

<ShufflerFolder>\                       # Configured by user
└── <ArtistTitle>.xml                  # Per-track extended attributes:
                                       #   fade points, loop counts, skip sections,
                                       #   automation triggers, manual mix curves
```

## Design patterns in use

**Partial classes** — `BassPlayer` split across five files to keep feature areas manageable without introducing separate types.

**Observer / events** — UI controls subscribe to `BassPlayer` events (`OnTrackChange`, `OnEndFadeIn`, `OnTrackQueued`, `OnVolumeChanged`, etc.) rather than polling.

**Facade** — `ShufflerApplication` hides the complexity of initialising and coordinating five subsystems behind a single object.

**Template method / interface** — `ISampleLibrary` defines the contract for sample sources; `LoopLibrary` and `TrackSampleLibrary` implement it differently.

**BASS sync callbacks** — Automation triggers use `BASS_ChannelSetSync` to fire callbacks at exact sample positions within a stream, rather than polling a timer.

**Settings persistence** — Windows Forms `Settings.Default` with XML backing for all user preferences.
