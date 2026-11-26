# Architecture

## Operating modes

The application serves two distinct use cases with different platform requirements:

| Mode | Description | Platform |
|---|---|---|
| **Active** | Library curation, sync/fade point setup, finding compatible tracks, performance DJing | Windows-only (Bass.Net, x86) |
| **Shuffler / Passive** | Automated playback of pre-mixed tracks — shuffled, sequenced, with playlist control | Audio host must be Windows; control UI does not have to be |

**Active mode** covers anything that requires human judgement on the audio: loading a new track and listening to it, working out where sync points and fade points should be, finding which tracks it mixes well into, and live performance mixing. All of this requires the full Windows app with direct access to the audio engine.

**Shuffler mode** covers automated playback where the mixing decisions have already been made. Eventually, `ShufflerApplication` will evolve into a service called via REST or HTTP by UIs running in separate processes. A remote UI can still do meaningful things here — adjusting the playlist, choosing the next track, skipping, changing volume — but it is not making real-time mixing decisions. The host process (always Windows, running BassPlayer) handles all audio; remote clients send commands over an API.

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

The goal is for UI controls to never talk directly to the audio engine or library — everything routes through `ShufflerApplication`. The engine and library layers have no knowledge of each other. See the boundary rules section below for what is currently achieved and what is accepted as a deliberate exception.

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
The heart of the application. `BassPlayer` is a facade: its public members pass through to internal components in `Halloumi.BassEngine/BassPlayer/`:

| Component | Responsibility |
|---|---|
| `MixingDesk` | Mixer channels, output routing, plug-in slots |
| `TrackLoader` | Loading tracks and their audio |
| `Playback` | Track slots, play/pause/queue, seeking, transitions |
| `ManualMixer` | Manual mix mode and extended-mix recording |
| `TrackFxSend` | The track FX send |
| `Sampler` | Sample playback channel |
| `AutomationPlayer` | Position-triggered FX sends and sample playback |
| `RawLooper` | Looping of arbitrary track sections |

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
Tracks (current, previous, next) ──► TrackMixer ──► splitter ──► SpeakerOutput / MonitorOutput
                                         └──► TrackSendMixer (muted until a send) ──► TrackSendFxMixer ──► SpeakerOutput
Sample streams ──► SamplerMixer ──► splitter ──► SpeakerOutput / MonitorOutput
RawLoop stream ──► RawLoopMixer ──► splitter ──► SpeakerOutput / MonitorOutput
```

Every playing track, both the outgoing and the incoming one during a crossfade, is on `TrackMixer`. Anything that must affect only one track (its volume slide, the fade-out high-pass) goes on that track's own stream, not on a mixer. Each splitter sends its group to the speakers, the monitor or both (`TrackOutput`, `SamplerOutput`, `RawLoopOutput` settings). `MixingDesk` builds all of this.

Plug-in slots:

- `WaPlugin` (WinAmp DSP) and the Main VST slots on `SpeakerOutput`
- The Tracks VST slot on `TrackMixer`
- The Sampler VST slots on `SamplerMixer` (which also has a fixed bass cut)
- The Track FX VST slots on `TrackSendFxMixer` (which also has a fixed bass cut)

## Two Track models

There are two different `Track` classes. They serve different layers and should not be confused:

| | `Halloumi.Shuffler.Engine.Models.Track` | `Halloumi.BassEngine.Models.Track` |
|---|---|---|
| Layer | Library / data | Audio engine |
| Purpose | Serialisable metadata | Runtime playback state |
| Key fields | Artist, Title, Bpm, Rank, Genre, Filename | FadeInStart/End, FadeOutStart/End, LoopCounts, SyncHandles |
| Persistence | SQLite track cache | In-memory only |

## Data persistence

Per-user data is in `%APPDATA%\Halloumi Shuffler\` (the folder is named after the exe, via `ApplicationHelper.GetUserDataPath()`). Shared Shuffler data is in the **Shuffler Folder** set in Settings, so it can be kept under version control or synced.

```
%APPDATA%\Halloumi Shuffler\
├── Halloumi.Shuffler.Library.db              # SQLite cache of track metadata read from MP3 tags
├── Halloumi.Shuffler.Library.xml             # pre-SQLite cache; migrated once if no .db exists
├── Halloumi.Shuffler.Settings.xml            # application settings
├── Halloumi.Shuffler.LibraryView.xml         # library screen filter, search and sort
├── Halloumi.Shuffler.WorkingPlaylist.xml     # current playlist
├── Halloumi.Shuffler.LoopLibrary.xml         # loop samples
└── SampleLibrary\                            # exported sample .wav files

%APPDATA%\Halloumi\
└── Halloumi.Shuffler.PlaylistGenerationSettings.xml

<ShufflerFolder>\
├── Halloumi.Shuffler.ExtendedAttributes.xml  # all tracks: fade points and lengths, start/end BPM,
│                                             #   skip section, loop counts, rank
├── Halloumi.Shuffler.AutomationAttributes.xml # all tracks: automation triggers, manual mix curves
├── Halloumi.Shuffler.MixLibrary.xml          # track-pair mix ratings (0–5)
├── Halloumi.Shuffler.Collections.xml         # track collections
└── Halloumi.Shuffler.SampleLibrary.xml       # sample definitions
```

Files in `ExtendedAttributes.xml` saved by older builds use `<ExtenedAttributes>` tags (a misspelling since fixed). They still load, and the next save writes `<ExtendedAttributes>`.

## Boundary rules

### What belongs in ShufflerApplication

Add logic to `ShufflerApplication` when it:

- Queries or filters tracks (`GetTracks`, `GetMixableTracks`)
- Generates or manages playlists (`GeneratePlaylist`, `GetPlaylistGenerationRequest`)
- Persists or retrieves settings (`LoadPlaylistGenerationSettings`, `GetExportPlaylistFolder`, etc.)
- Bridges engine events to UI (`OnAutoGenerateRequired`, `OnTrackChanged`, `OnFadeEnded`)
- Performs any domain operation a non-Windows UI would also need

`ShufflerApplication` may reference Windows Forms libraries (e.g., for types like `Rectangle` or `Point`), but it must **never** open a window or dialog, or return non-serializable data. All data returned should be "httpy" (plain data objects/DTOs) suitable for a future REST/HTTP interface.

It is assumed that `ShufflerApplication` will always be running on a Windows machine for the foreseeable future.

### What UI code must NOT do

- Hold a `Library`, `MixLibrary`, or `BassPlayer` field and call it directly for domain operations
- Read or write `Settings.Default` for domain-significant settings — use a `ShufflerApplication` method instead
- Subscribe directly to `BassPlayer` events — subscribe to the corresponding `ShufflerApplication` event instead

### Intentional exceptions — direct engine access is accepted here

| File | Reason |
| --- | --- |
| `frmSettings.cs` | IS the settings editor; reads and writes `Settings.Default` directly |
| `frmPluginSettings.cs` | VST/WinAmp plugin config requires direct `BassPlayer` plugin slot access; too intertwined to safely abstract |
| `frmMain.cs` | Persists pure UI layout state (`FormStateSettings`, `MinimizeToTray`, `VisualsShown`) with no domain significance |
| `Program.cs` | Start-up folder validation runs before `ShufflerApplication` is instantiated |
| `CommonFunctions.cs` | Error-logging utility that needs `ShufflerFolder` before the app is wired |
| `PlayerDetails`, `TrackMixerControl`, `SamplerControl`, `frmEditTrackSamples`, `frmShufflerDetails` | Direct `BassPlayer` calls for volume/FX assignment in response to hardware events (MIDI, sliders) are an accepted compromise — `Settings.Default` reads have been removed from these, but the `BassPlayer` assignments remain |
| `frmMonitorSettings.cs` | Active DJing feature (headphone cue output); `GetMonitorVolume`/`SetMonitorVolume` are low-level audio operations that will not be surfaced through `ShufflerApplication` |
| `SampleLibraryControl.cs` | Active DJing feature; holds `BassPlayer` for sample previewing and `LinkLoopSampleToTrack` — too low-level to route through the facade |
| `SamplePlayer.cs` | Active DJing feature; `PlaySample`/`PauseSample` are direct `BassPlayer` calls triggered by mouse events — low-level audio control that stays outside the facade |
| `TrackWave.cs` | Active DJing feature; waveform display and real-time fade/skip point editor — calls `LoadRawLoopTrack`, `SetRawLoopPositions`, `PlayRawLoop`, `GetPosition`, and mutates `BassTrack` properties directly during user interaction |

## Design patterns in use

**Facade over components** — `BassPlayer` holds no logic; each feature area is its own internal class that owns its channels and state. BASS sync callbacks reach them through `SyncDispatcher`, where each component registers the sync types it handles.

**Observer / events** — UI controls subscribe to `BassPlayer` events (`OnTrackChange`, `OnEndFadeIn`, `OnTrackQueued`, `OnVolumeChanged`, etc.) rather than polling.

**Facade** — `ShufflerApplication` hides the complexity of initialising and coordinating five subsystems behind a single object.

**Template method / interface** — `ISampleLibrary` defines the contract for sample sources; `LoopLibrary` and `TrackSampleLibrary` implement it differently.

**BASS sync callbacks** — Automation triggers use `BASS_ChannelSetSync` to fire callbacks at exact sample positions within a stream, rather than polling a timer.

**Settings persistence** — Windows Forms `Settings.Default` with XML backing for all user preferences.
