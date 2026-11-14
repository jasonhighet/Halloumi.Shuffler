# UI — Forms and Controls

The UI layer lives in `Halloumi.Shuffler/`. It is built with Windows Forms and styled with ComponentFactory.Krypton 4.3.2. All UI code references `ShufflerApplication` for any business logic or audio operations — it does not talk to `BassPlayer` or `Library` directly.

---

## Main window — frmMain

`Halloumi.Shuffler/Forms/frmMain.cs`

The primary application window. It hosts all major child controls and provides the menu bar, system tray icon, and status bar.

### Child controls

| Control | Type | Purpose |
|---|---|---|
| `playlistControl` | `PlaylistControl` | Current playlist and track queue |
| `trackLibraryControl` | `TrackLibraryControl` | Browse and search the track library |
| `mixerControl` | `MixerControl` | Effects chain status and controls |
| `playerDetails` | `PlayerDetails` | Current track info and waveform |
| `shufflerController` | `ShufflerController` | Auto-playlist generation |

### Menu structure

**File**
- Open / Save playlists
- Play, Pause, Next, Previous
- Skip to End

**View**
- Toggle Library panel
- Toggle Mixer panel
- Toggle Playlist panel
- Toggle Visuals
- Toggle Track Details
- Toggle Player

**Plugins**
- WinAmp DSP configuration
- VST Plugin (Main) × 2
- VST Plugin (Sampler) × 2
- VST Plugin (Track)
- VST Plugin (Track FX) × 2

**Settings**
- Application preferences

**Shuffle After Shuffling**
- Toggle auto-reshuffle after metadata update

### System tray

When minimised to tray the context menu provides: Play, Pause, Next, Previous, Skip to End.

### Status bar

Three labels: `LibraryStatus`, `PlaylistStatus`, `PlayerStatus` — updated by child controls as state changes.

---

## Controls

All controls live in `Halloumi.Shuffler/Controls/`.

### ShufflerController

`Controls/ShufflerController.cs`

Manages automatic playlist generation. Watches `BassPlayer.OnEndFadeIn` and `OnSkipToEnd` events; when the remaining track count drops below `TracksRemainingThreshold` (default: 2) it triggers `FrmGeneratePlaylist`.

```
AutoGenerateEnabled          Toggle auto-generation on/off
TracksRemainingThreshold     Minimum tracks before auto-generating (default 2)
AutoGenerateNow()            Manually trigger generation
```

References `PlaylistControl` and `TrackLibraryControl` to pass context into the generation dialog.

### PlaylistControl

`Controls/PlaylistControl.cs`

Displays the current playlist and track queue. Lets the user reorder tracks, remove entries, and queue a specific track for immediate mixing. Fires `PlaylistChanged` when the list changes.

### TrackLibraryControl

`Controls/TrackLibraryControl.cs`

Browseable, filterable view of the track library. Supports filtering by genre, artist, album, BPM range, rank, and free-text search. Double-clicking or dragging a track sends it to the playlist.

### MixerControl / TrackMixerControl

`Controls/MixerControl.cs` / `Controls/TrackMixerControl.cs`

Shows the current state of the effects chain and mixer channels. `TrackMixerControl` handles the per-track mixing interface (volume, crossfade position).

### MixableTracks

`Controls/MixableTracks.cs`

Shows tracks that are compatible with the currently playing track based on `MixLibrary` ratings. Filtered by the `MixableRankFilterIndex` setting (minimum acceptable mix rank).

### PlayerDetails

`Controls/PlayerDetails.cs`

Displays current track metadata, a waveform view, and playback position. Updates in real time from `BassPlayer` events.

### TrackDetails

`Controls/TrackDetails.cs`

Shows extended metadata for the selected track: BPM, key, rank, fade point offsets, loop counts, etc. Read from extended attributes.

### SamplerControl / SampleLibraryControl

`Controls/SamplerControl.cs` / `Controls/SampleLibraryControl.cs`

`SamplerControl` exposes buttons to trigger loaded samples. `SampleLibraryControl` lets the user browse the loop and sample libraries and assign samples to tracks.

### SamplePlayer

`Controls/SamplePlayer.cs`

A single sample trigger UI element — shows sample name and play/stop state.

### TrackWave

`Controls/TrackWave.cs`

Waveform editor for a track. Used by `frmShufflerDetails` (Shuffler mode: fade-in, fade-out, skip and pre-fade-in) and `frmEditTrackSamples` (Sampler mode: the current sample and its offset).

**View and sections are separate.** The view is what is on screen (`ZoomStart` → `ZoomEnd`). A section is a fade-in, fade-out, skip section or sample. One section can be the *active* section: it is drawn brighter and named after "Loop:" under the waveform.

- With an active section, Play loops that section whatever the view shows, and the loop follows the section while it is edited (no restart).
- With no active section, Play loops the view from the cursor. This is the original behaviour, and the forms' "Update" buttons still copy the view into a section.

A section becomes active when you click its coloured band, drag one of its edges, use a Set/Move command on it, press I / O / K, or zoom exactly to it (the forms' Zoom buttons). Shift+click, dragging out a range, and the Zoom In / Zoom Out / Zoom Full / < / > buttons make the view the loop again.

**What is drawn**

| Element | Meaning |
|---|---|
| Ruler | Time, plus light-blue ticks for the beats detected in the audio (shown when zoomed in to 45 seconds or less and the beat is clear) |
| Coloured band and tint | A section: green fade-in, orange fade-out, grey skip, purple sample. The band shows the length in beats and the tempo that length implies |
| White lines inside a fade section or sample | Beat grid anchored on the section start at the section's own tempo, so the last line is the section end. Bar lines are brighter. If the drum hits sit on the lines, the section is on the beat |
| Faint lines outside the sections | Bar lines of the active section's grid, carried on across the track (every fourth bar brighter) |
| Red line | Cursor: the last place clicked. Set/Move commands act on it |
| White line with a flag | Playhead |

**Mouse**

| Action | Result |
|---|---|
| Click | Move the cursor |
| Drag a section edge | Move that edge only |
| Drag a section's band | Move the whole section, keeping its length |
| Click a section's band | Make it the active section |
| Drag on empty waveform | Zoom to the range dragged (same as click then Shift+click) |
| Wheel / Shift+wheel / middle-drag | Zoom around the pointer / scroll / pan |
| Click a numbered box | Move the fade-in / fade-out to that suggestion |
| Right-click | Set and Move commands at the cursor, Snap to Beat, Suggest Fade Sections |

**Keyboard** (handled by `HandleKey`, called from the forms' `ProcessCmdKey` when the focus is not on a text box, drop-down, list or tick box; clicking the waveform takes the focus)

| Key | Action |
|---|---|
| Space | Play / stop |
| I / O / K | Select and show the fade-in / fade-out / skip section (Shuffler mode) |
| S / E | Set the active section's start / end at the cursor (the playhead while playing) |
| [ / ] | Halve / double the active section's length |
| , / . | Move the active section back / forward by its own length |
| Z | Show the active section with room to drag its edges |
| Down / Up | Move the fade-in (or the fade-out, if it is the active section) to its next / previous suggestion |
| 1 - 5 | Move it to that suggestion |
| N | Snap to beat on / off |
| + / - | Zoom in / out |
| Ctrl+0 | Show the whole track |

**Snap to beat** ("Snap" button, N). The beats of the whole track are analysed in the background when it loads (`BeatAnalysis`, see [audio-engine.md](audio-engine.md)). With Snap on, the cursor, dragged starts and moved sections land on the nearest beat or half-beat of the audio, and dragged ends land a whole number of beats from the section start. Holding Ctrl reverses the setting for one action. Snapping does nothing until the analysis has finished, or where the beat is too weak to trust (free-time or loosely played music).

**Suggestions** (Shuffler mode). Once the beats have been analysed, five suggested positions for the fade-in (green) and five for the fade-out (orange) are shown as numbered boxes under the section bands; 1 is the best guess. Down / Up, the number keys, or a click on a box move the section there, select it and show it. A playing loop carries straight on at the new position, so the suggestions can be auditioned one after another without stopping. The text under the waveform says which suggestion the section is on. The fade-out suggestions follow from the fade-in, and are worked out again when it changes. How they are chosen, and how well they do, is in [fade-suggestions.md](fade-suggestions.md).

**Auto Fades** ("Auto Fades" button). Moves both fade sections to their first suggestion. A track with no saved attributes gets this automatically when the analysis finishes, unless the fades have been changed by then. Nothing is saved until OK is pressed on the form: suggestions are starting points to audition and adjust.

### VolumeLevel

`Controls/VolumeLevel.cs`

Animated level meter driven by FFT data from `VolumeAnalyzer`.

### Slider

`Controls/Slider.cs`

Custom horizontal slider used for the manual mix crossfade control and other volume/position inputs.

### ListBuilder

`Controls/ListBuilder.cs`

Helper control for building dynamic filtered lists, used by TrackLibraryControl.

---

## Dialogs

All dialogs live in `Halloumi.Shuffler/Forms/`.

### frmPluginSettings

`Forms/frmPluginSettings.cs`

Configure all plugin slots: select VST `.dll` files for each of the seven slots, configure the WinAmp DSP plugin, set delay notes for the FX send. Changes are applied live to `BassPlayer` and persisted via `ShufflerApplication.SaveSettings()`.

### frmSettings

`Forms/frmSettings.cs` / `Forms/Settings.cs`

Application preferences. All settings are documented in [settings.md](settings.md).

### frmGeneratePlaylist

`Forms/frmGeneratePlaylist.cs`

Wizard for auto-generating a playlist. Uses `MixLibrary` ratings to find compatible track sequences. Options include minimum mix rank, BPM range constraints, genre filtering, and number of tracks.

### frmSampleLibrary

`Forms/frmSampleLibrary.cs`

Browse and manage loop and sample libraries. Assign samples to tracks. Preview samples through the monitor output.

### frmUpdateTrackDetails

`Forms/frmUpdateTrackDetails.cs`

Edit track metadata: artist, title, album, genre, BPM, rank, key. Changes are written back to MP3 ID3 tags and the library cache.

### frmUpdateTrackAudio

`Forms/frmUpdateTrackAudio.cs`

Visual editor for audio sections: set fade-in/fade-out boundaries, loop counts, skip sections, and pre-fade sections. Shows a waveform with draggable markers.

### frmShufflerDetails

`Forms/frmShufflerDetails.cs`

Editor for a track's Shuffler details: fade-in / fade-out / skip / pre-fade-in sections (on a `TrackWave`), loop counts, track rank, track FX triggers and track samples. The "Calculate" buttons detect the tempo of the audio around the fade section (`TrackWave.DetectBpmAround`) and set the fade length to 16 beats at that tempo.

### frmEditTrackSamples

`Forms/frmEditTrackSamples.cs`

Link specific samples to a track so they load automatically into the sampler when that track is queued.

### frmMonitorSettings

`Forms/frmMonitorSettings.cs`

Configure the monitor output device and volume (the "headphone cue" output, separate from the main speaker output).

### frmVstChain

`Forms/frmVstChain.cs`

Visual editor for the VST plugin chain order and bypass states.

### frmImportShufflerTracks

`Forms/frmImportShufflerTracks.cs`

Import extended attribute XML files from another Shuffler folder (e.g. when migrating a library).

### frmExportPlaylist

`Forms/frmExportPlaylist.cs`

Export the current playlist to a file (M3U or similar).

---

## Theming

All forms and controls use ComponentFactory.Krypton 4.3.2 for visual theming. The palette is set globally at startup. Standard WinForms controls are replaced with Krypton equivalents (e.g. `KryptonButton`, `KryptonListBox`) wherever possible.

---

## Form state persistence

Window sizes and positions are saved to `Settings.Default.FormStateSettings` as a serialised dictionary keyed by form type name. `LeftRightSplit` and `TrackSplit` store splitter positions. These are restored on next launch.

---

## Design-time gotcha: BASSTimer in constructors

`BASSTimer` (from `Un4seen.Bass.Misc`) requires the native BASS DLL to be loaded. Visual Studio instantiates UserControls to render them in the designer — if `BASSTimer` is constructed unconditionally, the designer silently fails and shows a blank control.

**`DesignMode` is unreliable in constructors.** The `Control.DesignMode` property returns `false` inside a constructor because the control hasn't been sited yet. Use `LicenseManager.UsageMode` instead, which is set before the constructor runs:

```csharp
public MyControl()
{
    InitializeComponent();
    if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

    _timer = new BASSTimer(100);
    _timer.Tick += Timer_Tick;
}
```

The combined check handles both the designer (via `LicenseManager`) and nested host scenarios (via `DesignMode`). Use this same combined form in event handlers and methods too — `DesignMode` works there, but the combined check is consistent and safe everywhere.

Field initialisers cannot use either check, so Bass.Net fields must be declared without an initialiser and assigned in the constructor after the guard.

**Affected controls (already fixed):** `PlayerDetails`, `TrackMixerControl`, `TrackWave`, `Slider`, `PlaylistControl`.
