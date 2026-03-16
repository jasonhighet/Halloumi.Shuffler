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

### BassPlayer — split partial class

`BassPlayer` is the audio engine core, split across five files in `Halloumi.BassEngine/BassPlayer/`:

| File | Responsibility |
|---|---|
| `BassPlayer.cs` | Core playback, track queue (Current/Previous/Next/Preloaded), crossfade, volume, events |
| `BassPlayer.Plugins.cs` | VST and WinAmp DSP plugin loading across seven plugin slots |
| `BassPlayer.Automation.cs` | Position-triggered FX sends and sample playback via `BASS_ChannelSetSync` |
| `BassPlayer.Sampler.cs` | Sample/loop playback on a dedicated mixer channel |
| `BassPlayer.RawLoop.cs` | Looping of arbitrary track sections |

### Mixer channel topology

```
CurrentTrack  ──► CurrentTrackMixer  ──┐
PreviousTrack ──► PreviousTrackMixer ──┼──► MainMixer ──► OutputSplitter ──► SpeakerOutput
RawLoop       ──► RawLoopMixer       ──┤                               └──► MonitorOutput
Samples       ──► SamplerMixer       ──┘
                      └──► TrackSendMixer (FX send, routed back to MainMixer)
```

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
- **Orphaned legacy files** — `Halloumi.BassEngine/` root contains several `.cs` files (`Shuffler.cs`, `Library.cs`, `TrackSelector.cs`, etc.) that are excluded from the project. Do not add them back.
- **Slider.Designer.cs** — excluded from `Halloumi.Shuffler` project intentionally; do not add it back

## Data files (runtime, not in repo)

```
%APPDATA%\Halloumi\
├── Halloumi.Shuffler.Library.xml       # Cached track metadata
├── Halloumi.Shuffler.MixLibrary.xml    # Track-pair mix ratings (0–5)
└── Halloumi.Shuffler.Settings.xml      # Application settings

<ShufflerFolder>\<Artist - Title>.xml   # Per-track: fade points, loop counts,
                                        # automation triggers, manual mix curves
```

## Documentation

Detailed docs are in `docs/`:
- [docs/architecture.md](docs/architecture.md) — dependency graph, mixer topology, design patterns
- [docs/audio-engine.md](docs/audio-engine.md) — full BassPlayer API reference
- [docs/audio-library.md](docs/audio-library.md) — Library, MixLibrary, models
- [docs/ui.md](docs/ui.md) — forms and controls inventory
- [docs/workflows.md](docs/workflows.md) — key user workflows end-to-end
- [docs/settings.md](docs/settings.md) — settings reference
