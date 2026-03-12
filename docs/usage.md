# How the Application is Used

This document describes the real-world usage patterns and goals behind the Halloumi Shuffler, to provide context for feature development and prioritisation.

---

## Phase 1 — Library management

Importing songs, verifying and correcting metadata: artist, title, album, genre, BPM, tags. Mostly routine maintenance to keep the library clean.

---

## Phase 2 — Preparing songs for shuffling (audio section editing)

The most technically demanding phase. Each song must have its fade-in and fade-out sections defined before it can be mixed automatically.

- Open each track in the audio section editor (`frmUpdateTrackAudio`)
- Define **FadeInStart / FadeInEnd** — the section that fades up at the start of the track when mixed in
- Define **FadeOutStart / FadeOutEnd** — the section that fades down at the end when mixing out
- Optionally define a **SkipStart / SkipEnd** section to trim a middle portion of the song
- Goal is to make mixed tracks run **2–3 minutes** (some genres 3–4+ minutes)

**Critical constraint:** fade points must land exactly on the beat. The BPM is derived from these points — if they are off-beat, the BPM will be wrong and the track will mix incorrectly with everything else.

### Current goal / improvement area

Reduce the time spent on this phase. Specifically:
- **Auto-detect BPM** from audio analysis and use it to snap fade points to the beat
- **Auto-suggest fade points** using audio analysis (e.g. silence detection, energy envelope, beat grid) to give a starting position that the user only needs to fine-tune rather than set from scratch

---

## Phase 3 — Ranking mixes (mix rating)

Once tracks have audio sections defined, every possible pairing needs a mix rank so the auto-generator can make good playlists.

### Workflow mode: "working mode" playlist generation

1. Select a single seed track
2. Generate a playlist in **working mode** — this produces a playlist that alternates between the seed track and every other track it can potentially mix with (within ~10% BPM, not yet rated, prioritising matching key)
3. Listen to each crossfade transition
4. Assign a rank: **Excellent / Very Good / Good / OK / Poor / Forbidden**

### Goal per track

Build up **10–20 songs** that can mix into (or out of) each track, rated Excellent, Very Good, or Good.

### Current goal / improvement area

Speed up this phase:
- **One-click ranking** — assign a rank and immediately jump to the next mix without extra clicks
- **Skip to end** — after rating, jump directly to the FadeOutStart of the current track to hear the outgoing mix immediately, skipping the body of the song

---

## Phase 4 — Passive DJing (the Shuffler)

The intended primary use case. For relaxed listening (e.g. a BBQ), the application:

- Builds a playlist automatically from tracks with good mix ratings
- Or the user picks a seed track and the app suggests what mixes well into it
- Plays continuously with automatic crossfades between tracks
- Recorded manual mixes (from Phase 5) are replayed automatically during passive mode, making it sound better than a plain auto-crossfade

---

## Phase 5 — Active DJing (performance / recording mode)

Used for live performance or personal recording sessions.

- **Manual crossfade control** — the DJ drives the crossfade slider by hand
- **Sample launch** — buttons trigger pre-loaded samples mid-mix
- **FX sends** — delay effects can be triggered manually
- **Record mode** — after performing a mix, the DJ presses a button to save the recorded crossfade curve, sample triggers, and FX timings to the track's XML file
- On subsequent passive playbacks, the saved performance is replayed automatically

There are two contexts for active mode:
- **Live performance** (audience present) — mixing in real time, no recording
- **Home session** (office/room) — mixing and recording for later passive playback

---

## Summary of current development priorities

| Area | Goal |
|---|---|
| Audio section editing | Auto-detect BPM; auto-suggest fade-in/fade-out positions from audio analysis |
| Mix rating workflow | One-click rank assignment; instant skip to end-of-track for next mix |
