# Key Workflows

This document traces the major user-facing workflows through the codebase, showing which classes and methods are involved at each step.

---

## 1. Application startup

```
Program.Main()
  └─► ShufflerApplication.ctor()
        ├─ new BassPlayer()                     initialise Bass.Net, create mixer channels
        ├─ new Library(BassPlayer)
        ├─ new LoopLibrary()
        ├─ new MixLibrary()
        ├─ new TrackSampleLibrary()
        ├─ LoadSettings()                       read %APPDATA%\Halloumi\Settings.xml
        │     applies: volume, plugin paths, output routing, feature flags
        ├─ LoadFromDatabase()
        │     ├─ Library.LoadFromDatabase()     read Library.xml (track cache)
        │     ├─ MixLibrary.LoadFromDatabase()  read MixLibrary.xml
        │     └─ TrackSampleLibrary.Load()      read sample associations
        └─ ResetMidi()                          enumerate MIDI devices, apply mapping

frmMain.ctor()
  └─ initialises child controls
       ShufflerController.Initialise()
         └─ subscribes to BassPlayer.OnEndFadeIn, OnSkipToEnd
```

If `UpdateLibraryOnStartup` is set, `Library.LoadFromDatabase()` also rescans the MP3 folder to pick up new/changed files.

---

## 2. Loading the track library

The library uses a two-level cache strategy:

```
Library.LoadFromDatabase()
  ├─ If XML cache exists:
  │     deserialise Track list from Halloumi.Shuffler.Library.xml
  ├─ Else:
  │     scan LibraryFolder for *.mp3
  │     foreach file: read ID3 tags via IdSharp → create Track
  └─ SaveToDatabase()   write XML cache for next launch
```

When the user explicitly refreshes (File → Rescan), the same path is taken but the cache is always rewritten.

---

## 3. Queuing and playing a track

```
User double-clicks a track in TrackLibraryControl
  └─► PlaylistControl.AddTrack(libraryTrack)
        └─► if playlist is empty or user requests immediate play:
              BassPlayer.QueueTrack(engineTrack)
                ├─ load stream via BASS_StreamCreateFile
                ├─ read extended attributes (fade points, loops, BPM, automation)
                │     ExtendedAttributesHelper.LoadAttributes(track)
                │     AutomationAttributesHelper.LoadTriggers(track)
                ├─ set up BASS sync callbacks at fade/loop/skip boundaries
                ├─ configure linked samples → RefreshSamples()
                └─ fire OnTrackQueued event

BassPlayer.Play()
  ├─ start CurrentTrack stream
  ├─ apply fade-in volume curve
  └─ fire OnTrackChange event → UI controls update
```

---

## 4. Automatic crossfade

The crossfade is driven entirely by BASS sync callbacks set during `QueueTrack`. No timer polling.

```
[BASS fires sync at FadeOutStart position]
  └─► BassPlayer internal handler
        ├─ begin ramping PreviousTrack volume → FadeOutEndVolume over FadeOutLength
        ├─ begin ramping CurrentTrack volume → FadeInEndVolume over FadeInLength
        └─ if LoopFadeInForever: restart fade-in instead of advancing

[BASS fires sync at FadeInEnd position]
  └─► BassPlayer internal handler
        ├─ fire OnEndFadeIn event
        │     └─► ShufflerController checks tracks remaining
        │           if < TracksRemainingThreshold → AutoGeneratePlaylist()
        └─ dispose PreviousTrack stream
```

---

## 5. Manual mix mode

Manual mix lets the DJ drive the crossfade by hand and optionally record a custom curve.

```
User enables manual mix mode
  └─► BassPlayer.IsManualMixMode = true
        └─ fire OnManualMixModeChanged

User moves crossfade slider
  └─► BassPlayer.SetManualMixVolume(value)
        └─ directly set PreviousTrack channel volume (bypasses automation)

User finishes mix
  └─► BassPlayer.StopRecordingManualExtendedMix(powerDownAfterFade)
        ├─ capture recorded volume curve as ExtendedMixAttributes
        └─► BassPlayer.SaveExtendedMix()
              └─ AutomationAttributesHelper.SaveExtendedMix(track, attributes)
                    └─ write to <ShufflerFolder>/<track>.xml
```

On next playback the stored curve is loaded and replayed automatically.

---

## 6. Force fade

Lets the DJ jump out of the current track immediately.

```
User presses Skip to End (menu, tray, or keyboard)
  └─► BassPlayer.ForceFadeNow(ForceFadeType)
        switch ForceFadeType:
          Cut        → silence CurrentTrack immediately, advance to NextTrack
          QuickFade  → compress fade-out to ~2 s
          PowerDown  → apply pitch-down effect then cut
          SkipToEnd  → seek CurrentTrack to FadeOutStart position
        └─ fire OnSkipToEnd event
              └─► ShufflerController: check tracks remaining
```

---

## 7. Sample and loop playback

```
SamplerControl shows loaded samples (from TrackSampleLibrary + LoopLibrary)

User clicks sample button
  └─► BassPlayer.PlaySample(sampleIndex)
        ├─ route through SamplerMixer
        ├─ apply SamplerVstPlugin chain
        └─ output to SamplerOutput device

User clicks stop
  └─► BassPlayer.PauseSample(sampleIndex)

Automation path (if SampleAutomationEnabled):
  [BASS fires sync at SampleTrigger.Position]
    └─► BassPlayer.StartSampleTrigger()
          └─ PlaySample(trigger.SampleId)
  [BASS fires sync at SampleTrigger.EndPosition]
    └─► BassPlayer.StopSampleTrigger()
          └─ PauseSample(trigger.SampleId)
```

---

## 8. FX send automation

```
TrackFxAutomationEnabled = true

[BASS fires sync at TrackFXTrigger.Position]
  └─► BassPlayer.StartTrackFxTrigger()
        ├─ wait DelayNotes (BPM-synced)
        └─► StartTrackFxSend*(delayNotes)
              ├─ open TrackSendMixer channel
              ├─ route CurrentTrack through TrackSendFxVstPlugin chain
              └─ blend back into MainMixer

[BASS fires sync at trigger end position]
  └─► BassPlayer.StopTrackFxSend()
        └─ close TrackSendMixer channel
```

Manual path: user presses an FX send button → `StartTrackFxSendQuarter()` etc. called directly.

---

## 9. Raw loop

```
User selects a track section for looping
  └─► BassPlayer.LoadRawLoopTrack(filename)
  └─► BassPlayer.SetRawLoopPositions(start, end, offset)
  └─► BassPlayer.PlayRawLoop()
        ├─ BASS stream created for the file
        ├─ seek to offset
        ├─ sync set at end → wraps to start on fire
        └─ output to RawLoopOutput device

User stops
  └─► BassPlayer.StopRawLoop()
  └─► BassPlayer.UnloadRawLoopTrack()
```

---

## 10. Auto-generating a playlist

```
ShufflerController.AutoGeneratePlaylist()
  └─► open FrmGeneratePlaylist dialog
        user selects: min mix rank, BPM range, genre, track count
        [Generate button]
          └─► MixLibrary queries compatible transitions:
                foreach candidate track:
                  GetMixLevel(currentTrack, candidate) >= minRank?
                  candidate BPM within range?
                  candidate genre matches?
                sort by mix level descending
                pick top N tracks
          └─► PlaylistControl.AddTracks(generatedList)
```

---

## 11. Rating a mix

```
User listens to CurrentTrack → NextTrack transition
User opens Mix Rating dialog (or uses keyboard shortcut)
  └─► MixLibrary.SetMixLevel(currentTrack, nextTrack, rating)
        └─ stored in memory; persisted by SaveToDatabase()

On shutdown or explicit save:
  MixLibrary.SaveToDatabase()
    └─ write Halloumi.Shuffler.MixLibrary.xml
```

---

## 12. Editing track audio sections

```
User right-clicks track → Edit Audio Sections
  └─► open frmUpdateTrackAudio
        waveform displayed via TrackWave control
        user drags markers for:
          FadeInStart / FadeInEnd
          FadeOutStart / FadeOutEnd
          SkipStart / SkipEnd
          PreFadeInStart
        user sets loop counts, BPM, power-down flag
        [Save]
          └─► ExtendedAttributesHelper.SaveAttributes(track, data)
                └─ write <ShufflerFolder>/<Artist - Title>.xml
```

If the track is currently queued, `BassPlayer.QueueTrack()` is called again to reload with the new attributes.

---

## 13. Plugin configuration

```
User opens Plugins menu → VST Plugin (Main) 1
  └─► frmPluginSettings opens (or focuses)
        user browses to a .dll file
        [Load]
          └─► ShufflerApplication.ShowPluginsForm() already has form open
          └─► BassPlayer.LoadMainVstPlugin(path, 1)
                ├─ unload previous plugin if any
                ├─ create VSTPlugin instance via BASS_VST_ChannelSetDSP
                └─ apply to MainMixerChannel DSP chain
        [Settings button]
          └─► VSTPlugin.ShowEditor()   open the VST plugin's own GUI
        [Clear]
          └─► BassPlayer.ClearMainVstPlugin(1)

On close: ShufflerApplication.SaveSettings() persists the plugin path
```

---

## 14. Application shutdown

```
frmMain.FormClosing
  └─► ShufflerApplication.Unload()
        ├─ Library.SaveToDatabase()
        ├─ MixLibrary.SaveToDatabase()
        ├─ SaveSettings()                 write Settings.xml
        ├─ BassPlayer.UnloadAllVstPlugins()
        ├─ BassPlayer.UnloadAllWaPlugins()
        └─ BassPlayer.Dispose()           free all BASS streams and handles
```
