# Halloumi.Shuffler

A Windows desktop DJ mixing application built on .NET 4.8 and the Bass.Net audio engine.

## What it does

- Manages a library of audio tracks with metadata (BPM, key, rank, genre, etc.)
- Plays and cross-fades between tracks with configurable fade curves
- Stores compatibility ratings between track pairs to power intelligent playlist sequencing
- Plays back samples and loops synchronized to the current track BPM
- Chains VST and WinAmp DSP effects across multiple mixer channels
- Accepts MIDI controller input mapped to player functions
- Automates effect sends and sample triggers at specific positions within a track

## Projects

| Project | Description |
|---|---|
| `Halloumi.BassEngine` | Low-level audio engine — wraps Bass.Net for streaming, DSP, MIDI, beat/section detection |
| `Halloumi.Shuffler.Engine` | Pure data/logic layer — track library, mix rankings, playlist models. No audio. |
| `Halloumi.Shuffler.Application` | Thin facade that wires all subsystems together |
| `Halloumi.Shuffler` | WinForms UI — forms, controls, Krypton theming |
| `Halloumi.Shuffler.TestHarness` | Dev/debug harness |

## Tech stack

- **.NET Framework 4.8**, x86, Windows only
- **WinForms** with ComponentFactory.Krypton 4.3.2 for theming
- **Bass.Net 2.4.6.4** (Un4seen) for audio streaming and DSP
- **MathNet.Numerics 5.0.0** for FFT/signal processing
- **Sanford.Multimedia.Midi** for MIDI device handling
- **IdSharp** for MP3/ID3 tag reading and writing
- **Newtonsoft.Json** for MIDI mapping serialization

## Documentation

- [Architecture overview](docs/architecture.md)
- [Audio engine (BassPlayer)](docs/audio-engine.md)
- [Audio library and models](docs/audio-library.md)
- [UI — forms and controls](docs/ui.md)
- [Key workflows](docs/workflows.md)
- [Settings reference](docs/settings.md)

## Data files

Per-user data is in `%APPDATA%\Halloumi Shuffler\`:

| File | Contents |
|---|---|
| `Halloumi.Shuffler.Library.db` | Cached track metadata (SQLite) |
| `Halloumi.Shuffler.Settings.xml` | Application settings |
| `Halloumi.Shuffler.WorkingPlaylist.xml` | Current playlist |

Shuffler data is shared through the **Shuffler Folder** set in Settings:

| File | Contents |
|---|---|
| `Halloumi.Shuffler.ExtendedAttributes.xml` | Every track's fade points, BPMs and skip section |
| `Halloumi.Shuffler.AutomationAttributes.xml` | Automation triggers and manual mix curves |
| `Halloumi.Shuffler.MixLibrary.xml` | Track-pair mix ratings |

The full list is in [docs/architecture.md](docs/architecture.md#data-persistence).
