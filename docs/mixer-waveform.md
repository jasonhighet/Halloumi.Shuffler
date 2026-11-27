# Mixer waveform overview

A waveform strip on the **Mixer** view. It shows the current track, with the end of the previous track and the next track lined up on the transitions either side of it. It is built in three steps. Each step is useful on its own.

| Step | What it adds | Status |
|---|---|---|
| 1 | Display only: overview rows with the fade sections shaded | Built |
| 2 | Move the fade-out / fade-in by dragging, for this play only | Built |
| 3 | Keep a move as part of the extended mix between the two tracks | Built |

## What it shows

```
┌ Now: Artist – Title ───────────────────────────────────── mix in 1:23 ┐
│ │▓ fade in ▓│▁▃▅▇█▇▅▃▁▂▅▇██▇▅▃▁▂▃▅▇█▇│▓▓ fade out ▓▓│░░ tail ░░          │
├ Previous: Artist – Title ─────────── Next: Artist – Title ────────────┤
│ │▓ fade out ▓│░ tail ░                │▓▓ fade in ▓▓▓│▂▅▇██▇▅▃▁▃▅▇█▇▅▃ │
└───────────────────────────────────────────────────────────────────────┘
```

- **Two rows on one time axis.**
  - The top row is always the current track.
  - The bottom row holds the **end of the previous track** on the left and the **next track** on the right.
  - The previous track's fade-out starts directly under the current track's fade-in, and the next track's fade-in starts directly under the current track's fade-out.
  - The previous track is only shown while it is still playing out. If its tail would run into the next track, it is cut off where the next track starts.
- **The track as heard, not the file.** Each track is built from the parts of the file that are played, in the order they are played:
  - pre-fade-in (when the track uses one), which sits to the left of its fade-in;
  - the fade-in section, repeated once per start loop;
  - the body, from the end of the fade-in to the start of the fade-out, **with the skip section cut out**;
  - the fade-out section, repeated once per end loop;
  - the **tail**, from the end of the fade-out to the end of the file, drawn dim grey. It isn't normally heard, but manual mixing (and a recorded extended mix) can keep a track playing past its fade-out.
  The skipped audio never appears. Loop repeats are separated by a thin dashed line.
- **Shading.** The fade-in and fade-out sections are shaded in different colours. The pre-fade-in gets a lighter shade.
- **Playhead.** The part of each playing track already played is dimmed, and a line marks the position. The next track gets a playhead once it starts (its pre-fade-in).
- **Labels.** Each track's label starts where the track starts. The top row also shows the time until the mix starts.
- **Height.** The strip sits in the top half of a splitter. Dragging the splitter sets its height, which is saved as `MixOverviewHeight` in the settings. Until it is first dragged, the strip takes a quarter of the view.
- **Hiding.** **View > Mixer Waveform** shows or hides the strip (saved as `MixOverviewHidden`). While hidden it stops polling the engine.

### Which tracks are shown

The engine changes tracks when the fade-out *starts* (`Playback.StartFadeOut`): the outgoing track becomes `PreviousTrack` and the incoming one becomes `CurrentTrack`. So the top row moves on to the new track as soon as a mix starts, and the outgoing track appears bottom-left.

The previous track is shown while its position is past its fade-out start and it is still playing in the mixer. That includes playing on past the end of the fade-out in manual mixing. If the whole player is paused part-way through its fade-out section, it is still shown. When the engine pauses it at the end of its fade-out (or of a recorded mix), it disappears.

If the current track has run out with nothing queued, the last track is shown on its own in the top row.

### Known limits of step 1

- **Track time, not wall-clock time.** Positions are in each track's own time. During a mix the outgoing track's tempo is changed to match the incoming one, so the two playheads can drift apart slightly.
- **Extended mixes** (recorded manual mixes) can make a fade-out run longer than its section. The strip shows the section as set; the playhead carries on into the tail.
- **Manual mix mode** can loop the fade-out forever. The strip shows the fade-out repeated `EndLoopCount` times, and the playhead stays in the last repeat.

## How it is built

```
MixOverviewControl (UI, Mixer view)
    │  polls ~15×/s while visible; fetches peaks in the background
    ▼
ShufflerApplication.GetMixOverview()        ShufflerApplication.GetWaveformPeaks(filename)
    ▼                                            ▼   (small cache of recent tracks)
BassPlayer.GetMixOverview()                  WaveformPeaks.Compute(filename)
    ▼                                            (Helpers — decodes the file on the caller's thread)
Playback.GetMixOverview()  →  MixOverviewBuilder (Mixing)
```

| Piece | Where | Notes |
|---|---|---|
| `MixOverview`, `MixOverviewTrack`, `MixOverviewSegment` | `Halloumi.BassEngine/Models/MixOverview.cs` | Plain data, all in seconds, including `MixOverviewFadeMove` (where each movable fade is, what is saved, and how far it can go). Safe to hand to any UI. |
| `MixOverviewBuilder` | `Halloumi.BassEngine/Mixing/MixOverviewBuilder.cs` | Turns a track's fade points, loops and skip section into the heard segments, and works out the heard position. Reads tracks only. |
| `Playback.GetMixOverview` | `Halloumi.BassEngine/BassPlayer/Playback.cs` | Picks the tracks (above). It reads only and takes no lock, like `GetTrackPosition`, because the UI calls it often. |
| `WaveformPeaks` | `Halloumi.BassEngine/Helpers/WaveformPeaks.cs` | Decodes the whole file once into one peak per 50 ms. A five-minute track takes about a second, so it must run off the UI thread and never on the mixer thread. |
| `MixOverviewControl` | `Halloumi.Shuffler/Controls/MixOverviewControl.cs` | In the top panel of `MixerControl`'s `spltTopBottom` splitter. It draws each row's waveforms to a cached bitmap. Each tick it only redraws the dimmed played part and the playheads over that bitmap. |

Heard time for a track is measured from the start of its fade-in (t = 0), the same origin as `TrackTimeline.ToAdjustedPosition`. The pre-fade-in has negative times. Each track has an `Offset` on the shared axis:
- 0 for the current track;
- the current track's fade-out start for the next track;
- minus its own fade-out start for the previous track, so its fade-out lines up with the current track's fade-in.

## Step 2: moving the fades live

Drag a shaded section left or right. It moves **in whole sections**: one fade-out length at a time for the fade-out, one fade-in length for the fade-in. A section is usually 4, 8 or 16 bars, so a move always lands on a phrase boundary and the section keeps its length, which keeps the beat-match intact.

### Manual mix mode only

Fades can only be moved in **manual mix mode**. That is where you mix by hand, and an extended mix (a saved manual mix) is not played back, so the fades can be moved whether or not the pair has an extended mix. In automatic mode the strip shows the fades but nothing can be dragged, and the right-click menu doesn't open.

Where the coming mix's fades start (`Playback.ApplySavedFadeMoves`) is the same in both modes:
- if the pair has an extended mix, where its fade moves have them;
- otherwise, the tracks' own fades.

This is applied when a next track is queued, and again whenever manual mix mode is switched on or off (`Playback.OnManualMixModeChanged`). So leaving manual mix mode drops any move that wasn't saved with a mix. A fade that has already started is left where it is.

### What can be moved

- **Only the coming mix (current → next):**
  - the current track's fade-out, on the top row;
  - the next track's fade-in, on the bottom right.
  The cursor changes to ↔ over a section that can move.
- **The next track's intro is shown** in dim grey, left of its pre-fade-in or fade-in, so the fade-in can be dragged earlier into it.
- **While dragging,** a dashed ghost outline shows where the section will land, drawn over the audio it would play, with the number of sections moved. Nothing changes until the mouse is released. Right-click during a drag cancels it.
- **After release** the rows re-flow. A moved fade-out changes where the next track lines up. A moved fade-in changes which part of the next track sits under the current track's fade-out.

### Limits (`Mixing/FadeShift.cs`)

| Section | It can't move… |
|---|---|
| Fade-out | before the end of the current track's fade-in, past the end of the file, or across or into the skip section. It also can't move to within 1 second of the playhead, plus the next track's pre-fade-in length, so its sync can't be missed. Once the fade-out has started, it is locked. |
| Fade-in | before the start of the file (pre-fade-in included), into the next track's fade-out, or across or into the skip section. Once the next track starts playing (its pre-fade-in or fade-in), it is locked. |
| Both | outside manual mix mode. |

The overview carries the allowed range (`MixOverviewFadeMove.Min` / `Max`), and the drag snaps within it. `Playback.MoveFadeOut` / `MoveFadeIn` check again under `_slotLock` before moving, because the playhead may have moved on.

### How a move is held

A move shifts the engine track's in-memory fade points (`Track.ShiftFadeOut` / `ShiftFadeIn`). A fade-in move shifts the pre-fade-in start with it. The track remembers the move in samples and in sections (`LiveFadeOutShift` / `LiveFadeOutSections`, and the same for the fade-in). The section count is kept separately because a fade type can reshape (shorten) the fade-out after a move. Then the syncs are rebuilt (`SetTrackSyncPositions`).

- **Reloading keeps the move.** `ExtendedAttributesHelper.LoadExtendedAttributes` takes the move off, loads the saved fade points, and puts the move back. So `ResetTrackSyncPositions` and `ReloadTrack` don't undo it.
- **Unloading drops the move.** `TrackLoader.UnloadTrackAudioData` moves the fades back. A fresh audio load forgets any move before setting the fade points from scratch. A cached track is never left with a move from an earlier play.
- **"Fade now" forgets a fade-out move,** because it puts the fade-out at the playhead.
- **Changing the next track resets the mix's moves.** When a new next track is queued, `Playback.ApplySavedFadeMoves` puts the current track's fade-out and the new track's fade-in where they belong for the new pair (see *Manual mix mode only* above). The old next track leaves the slots, so its move is dropped with its audio.
- **After the transition the moves stay** on both tracks for the rest of that play, because they are playing at those positions.

The track editor (`TrackWave`) loads its own uncached copy of a track, so it always shows and saves the track's own fades, never a live move.

## Step 3: moves are part of the extended mix

An extended mix is a saved manual mix, and moving the fade sections is part of making that mix. So the moves are saved only when the manual mix is saved (**Save mix** on the mixer panel), and they belong to that extended mix:

- **Recording.** `TriggerRecorder.RecordExtendedMix` stores the fade moves in force while the mix was played: the outgoing track's fade-out and the incoming track's fade-in, in sections. They are saved with the rest of the mix by `SaveExtendedMix`. Re-recording replaces them, so a mix recorded with the fades back at normal saves no moves.
- **Playback.** When the pair comes up again, its fades start where the extended mix has them, in either mode (see *Manual mix mode only*). The automatic replay of the mix lines up with what was recorded.
- **Clearing.** Clearing the extended mix (`ManualMixer.ClearMix`) removes its moves with it.

There is no separate "save fade moves". The right-click menu only undoes live moves:

| Menu item | Does |
|---|---|
| Back to the extended mix's fades | Moves both fades back to where the pair's extended mix has them. |
| Reset to the tracks' own fades | Moves both fades back to 0, for this play. |

The top-right status shows `fades moved: kept if you save the mix` in manual mix mode when the fades differ from the extended mix (or from the tracks' own fades). Otherwise it shows `extended mix` or `extended mix, fades moved` when the pair has one.

### Where it is stored

The moves are fields of the pair's `ExtendedMixAttributes`, which lives in the outgoing track's automation attributes (`Halloumi.Shuffler.AutomationAttributes.xml`) and is keyed by the incoming track's description. The fields are `FadeOutShift` and `FadeInShift`, counted in sections. A count still means "one section earlier than normal" after a track's fades are re-edited, where a sample position would go stale. Moves are only read from an entry that is a recorded mix (`IsRecordedMix`), so an entry holding only sample triggers never moves a fade.

### Entries that are not recorded mixes

An `ExtendedMixAttributes` entry can hold a recorded mix, sample triggers, or both. `ExtendedMixAttributes.IsRecordedMix` (`FadeLength != 0`) is what makes it a recorded mix. `TransitionPlanner`, `Playback.HasExtendedMixAttributes` and the Mixer view's "*" marker check that.

Before this change, `TransitionPlanner` treated **any** entry as a recorded mix: `GetExtendedFadeOutLength` returned `mix.FadeLength`, so an entry holding only sample triggers gave a crossfade length of 0. That is fixed.
