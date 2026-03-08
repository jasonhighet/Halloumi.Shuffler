# Settings Reference

Settings are managed by `Halloumi.Shuffler/Forms/Settings.cs` and persisted to `%APPDATA%\Halloumi\Halloumi.Shuffler.Settings.xml` via Windows Forms `Settings.Default`.

They are loaded by `ShufflerApplication.LoadSettings()` and saved by `ShufflerApplication.SaveSettings()` (called on shutdown and after any settings dialog change).

---

## Folders

| Setting | Description |
|---|---|
| `LibraryFolder` | Root folder scanned for MP3 files |
| `ShufflerFolder` | Folder where per-track extended attribute `.xml` files are stored |
| `PlaylistFolder` | Default folder for saving and loading playlists |
| `WaPluginsFolder` | Folder browsed when selecting a WinAmp DSP plugin |
| `VstPluginsFolder` | Folder browsed when selecting a VST plugin `.dll` |
| `LoopLibraryFolder` | Folder containing `.wav` loop files for the loop library |
| `ExportPlaylistFolder` | Default destination when exporting playlists |
| `KeyFinderFolder` | Path to the external key-detection tool |
| `AnalogXScratchFolder` | Path to the AnalogX Scratch effect |
| `ImportShufflerFilesFolder` | Source folder when importing extended attributes from another library |

---

## Plugin configuration

### WinAmp DSP

| Setting | Description |
|---|---|
| `WaPlugin` | File path of the active WinAmp DSP plugin (empty = none) |

### Main mixer VST (2 slots)

| Setting | Description |
|---|---|
| `MainMixerVstPlugin` | Path to VST plugin for slot 1 |
| `MainMixerVstPlugin2` | Path to VST plugin for slot 2 |
| `MainMixerVstPluginParameters` | Serialised parameter preset for slot 1 |
| `MainMixerVstPlugin2Parameters` | Serialised parameter preset for slot 2 |

### Sampler VST (2 slots)

| Setting | Description |
|---|---|
| `SamplerVstPlugin` | Path to VST plugin for slot 1 |
| `SamplerVstPlugin2` | Path to VST plugin for slot 2 |
| `SamplerVstPluginParameters` | Serialised parameter preset for slot 1 |
| `SamplerVstPlugin2Parameters` | Serialised parameter preset for slot 2 |

### Track mixer VST (1 slot)

| Setting | Description |
|---|---|
| `TrackVstPlugin` | Path to VST plugin |
| `TrackVstPluginParameters` | Serialised parameter preset |

### Track FX send VST (2 slots)

| Setting | Description |
|---|---|
| `TrackFxvstPlugin` | Path to VST plugin for slot 1 |
| `TrackFxvstPlugin2` | Path to VST plugin for slot 2 |
| `TrackFxvstPluginParameters` | Serialised parameter preset for slot 1 |
| `TrackFxvstPlugin2Parameters` | Serialised parameter preset for slot 2 |

---

## Audio / output

| Setting | Type | Description |
|---|---|---|
| `Volume` | decimal 0–100 | Master output volume |
| `LoopVolume` | decimal 0–100 | Sampler / loop channel volume |
| `MonitorVolume` | decimal 0–100 | Monitor (headphone cue) output volume |
| `SamplerOutput` | enum | Which physical output the sampler routes to |
| `TrackOutput` | enum | Which physical output the tracks route to |
| `RawLoopOutput` | enum | Which physical output the raw loop routes to |

---

## Mixer / effects

| Setting | Type | Description |
|---|---|---|
| `CutSamplerBass` | bool | Apply high-pass filter on the sampler channel (removes bass frequencies) |
| `BypassSamplerEffect1` | bool | Bypass sampler VST slot 1 |
| `BypassSamplerEffect2` | bool | Bypass sampler VST slot 2 |
| `BypassTrackFxEffect1` | bool | Bypass track FX VST slot 1 |
| `BypassTrackFxEffect2` | bool | Bypass track FX VST slot 2 |
| `SamplerDelayNotes` | decimal | Sync delay before sampler activates (e.g. 0.25 = 1/4 note) |
| `TrackFxDelayNotes` | decimal | Sync delay before track FX send activates |

---

## Behaviour

| Setting | Type | Description |
|---|---|---|
| `LimitSongLength` | bool | Truncate tracks to 5 minutes maximum |
| `UpdateLibraryOnStartup` | bool | Rescan the MP3 folder on every launch |
| `EnableTrackFxAutomation` | bool | Auto-trigger FX sends at recorded positions |
| `EnableSampleAutomation` | bool | Auto-trigger samples at recorded positions |
| `ShuffleAfterShuffling` | bool | Re-shuffle playlist after extended attributes are updated |
| `MinimizeToTray` | bool | Minimise to system tray instead of taskbar |
| `ShufflerMode` | enum | Playlist generation strategy |

---

## UI state

| Setting | Type | Description |
|---|---|---|
| `ShowMixableTracks` | bool | Show the compatible-tracks panel |
| `VisualsShown` | bool | Show visualisation panel |
| `AlbumArtShown` | bool | Show album art |
| `ShowTrackDetails` | bool | Show track detail panel |
| `ShowPlayer` | bool | Show the player controls panel |
| `LeftRightSplit` | int | Left panel width (splitter position) |
| `TrackSplit` | int | Track list height (splitter position) |
| `MixableRankFilterIndex` | int | Selected minimum mix rank filter (0–5) |
| `MixableViewIndex` | int | Selected view in the mixable tracks panel |
| `FormStateSettings` | serialised dict | Window sizes and positions, keyed by form type name |

---

## Recent files

| Setting | Type | Description |
|---|---|---|
| `RecentFiles` | `StringCollection` | Recently opened playlist files (shown in File menu) |

---

## Where to find them in code

**Reading** — `ShufflerApplication.LoadSettings()` reads from `Settings.Default` and applies values to `BassPlayer` and other subsystems.

**Writing** — `ShufflerApplication.SaveSettings()` copies current subsystem state back to `Settings.Default` and calls `Settings.Default.Save()`.

**Dialog** — `frmSettings.cs` is the user-facing settings UI. Changes are applied immediately and `SaveSettings()` is called on close.

**Plugin dialog** — `frmPluginSettings.cs` handles all plugin-related settings. Each slot has a path field, a Load button, a Clear button, and (for VST) a Settings button to open the plugin's own GUI.
