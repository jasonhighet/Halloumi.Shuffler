# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build commands

**Do NOT use `dotnet build` on this project.** The .NET 9 SDK (and later) raises `MSB3823: Non-string resources require GenerateResourceUsePreserializedResources` on the WinForms `.resx` files, and setting that property breaks the .NET 4.8 WinForms build. Use Visual Studio's MSBuild directly instead:

```bash
# Find MSBuild via vswhere and build the full solution (x86 Release)
"%PROGRAMFILES(X86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe
# Then: MSBuild.exe Halloumi.Shuffler.sln /p:Platform=x86 /p:Configuration=Release /m
```

The `deploy.bat` script at the repo root handles this automatically — use it for all builds.

```bash
# Restore NuGet packages (dotnet restore is fine for this)
dotnet restore Halloumi.Shuffler.sln
```

There are no automated tests. The `Halloumi.Shuffler.TestHarness` project is a manual dev/debug harness (WinForms app), not a test framework.

**Target constraints:** .NET Framework 4.8, x86, Windows only. Do not change target framework or platform — Bass.Net requires .NET 4.8 and x86.

## Project structure

All projects use SDK-style `.csproj` with `net48` target.

| Project folder | Assembly | Role |
|---|---|---|
| `Halloumi.BassEngine/` | `Halloumi.Shuffler.AudioEngine` | Audio engine — Bass.Net wrapper |
| `Halloumi.Shuffler.Engine/` | `Halloumi.Shuffler.AudioLibrary` | Data/logic layer — no audio |
| `Halloumi.Shuffler/` | `Halloumi.Shuffler` | WinForms UI + `ShufflerApplication` orchestrator |
| `Halloumi.Shuffler.TestHarness/` | `Halloumi.Shuffler.TestHarness` | Manual debug harness |
| `Components/` | — | Pre-built DLLs (Bass.Net, Krypton, IdSharp, etc.) |
| `packages/` | — | NuGet packages (old-style folder) |

## Architecture

The UI goes through `ShufflerApplication` for all domain logic. `ShufflerApplication` is the single entry point for track queries, playlist generation, settings persistence, and event bridging:

```
Halloumi.Shuffler (UI)
    ├─► ShufflerApplication.cs (orchestrator/facade, lives in this project)
    │       ├─► BassPlayer       (Halloumi.BassEngine)
    │       ├─► Library          (Halloumi.Shuffler.Engine)
    │       ├─► MixLibrary
    │       ├─► LoopLibrary
    │       ├─► TrackSampleLibrary
    │       └─► MidiManager
    └─► Forms / Controls (talk to ShufflerApplication only, never directly to engine/library)
```

`ShufflerApplication` (`Halloumi.Shuffler/ShufflerApplication.cs`) owns all service instances and wires events between them.

### What belongs in ShufflerApplication

Add logic to `ShufflerApplication` if it:
- Queries or filters tracks (`GetTracks`, `GetMixableTracks`)
- Generates or manages playlists (`GeneratePlaylist`, `GetPlaylistGenerationRequest`)
- Persists or retrieves settings (`LoadPlaylistGenerationSettings`, `GetExportPlaylistFolder`, etc.)
- Bridges engine events to the UI (`OnAutoGenerateRequired`, `OnTrackChanged`)
- Performs any domain operation that a non-Windows UI would also need

`ShufflerApplication` may reference Windows Forms libraries (for types like `Rectangle` or `Point`), but it must **never** open a window/dialog, or return non-serializable data. It should return "httpy" (plain data objects/DTOs). It will always run on Windows.

### Intentional exceptions — direct engine access is accepted

These files legitimately bypass `ShufflerApplication` and should not be "fixed":

| File | Why it's accepted |
|---|---|
| `frmSettings.cs` | IS the settings editor; reads/writes `Settings.Default` directly |
| `frmPluginSettings.cs` | VST/WinAmp plugin configuration requires direct `BassPlayer` plugin slot manipulation; too intertwined to safely abstract |
| `frmMain.cs` | UI layout state only (`FormStateSettings`, `MinimizeToTray`, `VisualsShown`) — no domain significance |
| `Program.cs` | Start-up validation runs before `ShufflerApplication` is instantiated |
| `CommonFunctions.cs` | Error-logging utility; needs `ShufflerFolder` at start-up before the app is wired |
| Audio-reactive controls (`PlayerDetails`, `TrackMixerControl`, `SamplerControl`, `frmEditTrackSamples`, `frmShufflerDetails`) | Direct `BassPlayer` calls for volume/FX assignment in response to hardware events (MIDI, slider) are an accepted compromise — settings reads have been removed, but the BassPlayer assignments remain |
| `frmMonitorSettings.cs` | Active DJing feature; `GetMonitorVolume`/`SetMonitorVolume` are low-level audio operations not surfaced through `ShufflerApplication` |
| `SampleLibraryControl.cs` | Active DJing feature; holds `BassPlayer` for sample previewing and `LinkLoopSampleToTrack` — too low-level for the facade |
| `SamplePlayer.cs` | Active DJing feature; `PlaySample`/`PauseSample` are direct `BassPlayer` calls from mouse events — low-level audio control that stays outside the facade |

### BassPlayer — a facade over components

`BassPlayer` (`Halloumi.BassEngine/BassPlayer/BassPlayer.cs`) is the engine's public API and holds no logic: every member passes straight through to an internal component in the same folder. Put new logic in the component it belongs to, not in BassPlayer.

| Component | Responsibility |
|---|---|
| `MixingDesk` | Builds and owns every mixer channel, output routing, VST/WinAmp plug-in slots |
| `TrackLoader` | Loading tracks, tags, saved attributes and audio; freeing unused audio (`TrackCache`, `FadeDefaults`) |
| `Playback` | Track slots (`TrackDeck`), play/pause/queue, seeking, fade-in/fade-out transitions, "fade now", the slot lock |
| `ManualMixer` | Manual mix mode, the fader, recording and saving extended mixes |
| `TrackFxSend` | The track FX send |
| `Sampler` | Samples on the sampler mixer |
| `AutomationPlayer` | Playing back, saving, clearing and removing track FX and sample triggers |
| `RawLooper` | Looping a section of a track on its own mixer |
| `AudioDiagnostics` | Read-only glitch report (output buffer level, BASS CPU, what is playing); Help > Audio Diagnostics, Ctrl+Shift+D |

Supporting types: `TrackSyncs`/`TrackSyncType` (BASS position callbacks on a track), `SyncDispatcher` (each component registers handlers for its sync types with `syncs.On(...)`), `BackgroundQueue` (ordered worker for what sync handlers hand off), `TriggerRecorder` (automation being recorded), `VstPluginSlot`/`PluginChannel`.

**Threading rule:** sync handlers run on BASS's mixer thread while BASS holds its mixer lock, so the mixer thread must never wait for one of our locks (deadlock). Go through `Playback.RunSyncHandler`, keep `_slotLock` holds short, and never decode audio or raise events under it. Details in docs/audio-engine.md → "Threads and locking". In `Halloumi.BassEngine/Mixing/`, the BASS-free `TransitionPlanner`, `TrackTimeline` and `FadeDefaults`. See [docs/audio-engine.md](docs/audio-engine.md).

### Mixer channel topology

```
Tracks (current, previous, next) ──► TrackMixer ──► splitter ──► SpeakerOutput / MonitorOutput
                                         └──► TrackSendMixer (muted until a send) ──► TrackSendFxMixer ──► SpeakerOutput
Samples  ──► SamplerMixer ──► splitter ──► SpeakerOutput / MonitorOutput
RawLoop  ──► RawLoopMixer ──► splitter ──► SpeakerOutput / MonitorOutput
```

Both tracks in a crossfade are on `TrackMixer`, so anything that must affect one track only goes on that track's own stream, not on a mixer.

### Critical: two Track classes

There are two unrelated `Track` classes — do not confuse them:

| Class | Namespace | Purpose |
|---|---|---|
| `Track` | `Halloumi.Shuffler.Engine.Models` | Serialisable metadata: Artist, Title, Bpm, Rank, Genre, Filename |
| `Track` | `Halloumi.BassEngine.Models` | Runtime playback state: FadeInStart/End, FadeOutStart/End, LoopCounts, SyncHandles |

The engine `Track` extends `AudioStream` and holds BASS channel handles. The library `Track` is a plain data object serialised to XML.

## Key patterns

- **Events not polling** — UI controls subscribe to `BassPlayer` events (`OnTrackChange`, `OnEndFadeIn`, `OnTrackQueued`, `OnVolumeChanged`, etc.)
- **BASS sync callbacks** — automation triggers use `BASS_ChannelSetSync` for sample-accurate position callbacks, not timers
- **Safe saves** — write files with `SafeFile.ToXmlFile` / `SafeFile.WriteAllText` (`Halloumi.BassEngine/Helpers/SafeFile.cs`), not `SerializationHelper.ToXmlFile`. They write a temporary file and swap it in, so a crash mid-save leaves the old file intact instead of an empty one.
- **One value-picker dialog** — renaming an artist, album or genre, choosing a title and choosing source audio all use `FrmChooseValue`. The caller sets the title, list and starting value, and passes an `Apply` action.

## Data files (runtime, not in repo)

```
%APPDATA%\Halloumi Shuffler\                  # per-user, folder named after the exe
├── Halloumi.Shuffler.Library.db              # SQLite track cache (Library.xml there is the pre-SQLite copy, migrated once)
├── Halloumi.Shuffler.Settings.xml            # application settings
├── Halloumi.Shuffler.LibraryView.xml         # library screen filter and sort
├── Halloumi.Shuffler.WorkingPlaylist.xml     # current playlist
├── Halloumi.Shuffler.LoopLibrary.xml
└── SampleLibrary\                            # exported sample .wav files

%APPDATA%\Halloumi\Halloumi.Shuffler.PlaylistGenerationSettings.xml

<ShufflerFolder>\                             # shared, set in Settings
├── Halloumi.Shuffler.ExtendedAttributes.xml  # every track's fade points, BPMs, skip section
├── Halloumi.Shuffler.AutomationAttributes.xml
├── Halloumi.Shuffler.MixLibrary.xml          # track-pair mix ratings (0–5)
├── Halloumi.Shuffler.Collections.xml
└── Halloumi.Shuffler.SampleLibrary.xml
```

Attribute files saved before the `ExtendedAttributes` spelling fix use `<ExtenedAttributes>` tags. `ExtendedAttributesHelper.LoadFromDatabase` still reads them, and the next save writes the new spelling.

## Documentation

Detailed docs are in `docs/`:
- [docs/architecture.md](docs/architecture.md) — dependency graph, mixer topology, design patterns
- [docs/audio-engine.md](docs/audio-engine.md) — full BassPlayer API reference
- [docs/audio-library.md](docs/audio-library.md) — Library, MixLibrary, models
- [docs/ui.md](docs/ui.md) — forms and controls inventory
- [docs/workflows.md](docs/workflows.md) — key user workflows end-to-end
- [docs/settings.md](docs/settings.md) — settings reference
- [docs/mixer-waveform.md](docs/mixer-waveform.md) — the Mixer view's waveform strip: moving the coming mix's fades in whole sections (manual mix mode), kept as part of the extended mix
- [docs/fade-suggestions.md](docs/fade-suggestions.md) — how fade positions are suggested, and the harness (`tools/fade-eval/`) that scores them against hand-set fades
