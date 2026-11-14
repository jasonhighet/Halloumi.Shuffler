# Fade suggestions

How the Shuffler details screen suggests fade-in and fade-out positions, how good the suggestions are, and how to measure a change to them.

Suggestions only ever move the sections on the Shuffler details screen. Nothing is saved until OK is pressed there, and every section is expected to be auditioned and adjusted by ear before it is saved.

---

## What is suggested

The screen offers five numbered positions for the fade-in and five for the fade-out (see `TrackWave` in [ui.md](ui.md) for the keys). Number 1 is the best guess. "Auto Fades" applies number 1 of each.

### Fade-in (`BeatAnalysis.GetFadeInCandidates`)

1. For every point in the first 70% of the track, measure how much louder and bassier the 2 seconds after it are than the 2 seconds before it.
2. Favour points near where fade-ins usually start for the genre (the `FadeProfile`, below).
3. Take the best points that are at least 1.5 seconds apart.
4. Move each to the sharpest jump in loudness in the 2.5 seconds after it: the first hit of the new part. (The 2-second measure on its own lands early.)
5. Each suggestion is 16 beats long at the tempo detected there.

### Fade-out (`BeatAnalysis.GetFadeOutCandidates`)

The fade-out suggestions depend on the fade-in as it currently is on screen, and are worked out again when it changes.

1. Step forward from the fade-in start one fade-in length at a time, re-fitting the beat at each step so that tempo drift does not build up.
2. Stop at the last strong beat of the track.
3. Offer the steps that make the track play for closest to the genre's usual played length.

### Genre profile (`ShufflerApplication.GetFadeProfile`)

Worked out each time the screen opens, from the tracks that already have saved sections: those of the same genre if there are at least 15, otherwise all of them.

| Value | Meaning |
|---|---|
| `TypicalFadeInStart` | Median start of the fade-in |
| `FadeInSpread` | How much that start varies |
| `TypicalPlayedLength` | Median of fade-in start to fade-out end, less any skip section |

So the suggestions follow the library as more tracks are set up. Genre matters a lot: Drum & Bass fade-ins sit about 70 seconds in and play for nearly 4 minutes; Funk fade-ins sit about 3 seconds in and play for about 2 minutes.

---

## What the hand-set sections show

From the 1,497 tracks with saved sections (November 2026):

- **Fade-in.** In about two-thirds of tracks the hand-set start is in the top tenth of the track's rises in loudness and bass. Only a quarter start within the first second of the file.
- **Fade-out.** There is no such signature: the audio at a hand-set fade-out start does not stand out from the rest of the track. What is consistent is the played length (half of all tracks play for between 117 and 167 seconds).
- **Lengths.** Fade-in and fade-out are the same length in 44% of tracks, the fade-in is half the fade-out in 21%, and double in 15%.
- **Tempo.** In 41% of tracks the tempo implied by the fade-in differs from that of the fade-out by more than 1%, so one beat grid cannot be carried across a whole track.

---

## Measuring it: the harness

`tools/fade-eval/` scores the suggestions against the saved, hand-set sections. Both scripts must run in **32-bit** Windows PowerShell, after building the solution (Release, x86). Neither writes to the music files or the Shuffler data.

```
C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Bypass -File tools\fade-eval\Build-AnalysisCache.ps1
C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Bypass -File tools\fade-eval\Test-FadeSuggestions.ps1
```

| Script | What it does | Time |
|---|---|---|
| `Build-AnalysisCache.ps1` | Analyses the audio of every track that has saved sections and caches it in `%LOCALAPPDATA%\Halloumi\FadeEval` (about 700 MB). Skips tracks already cached. Run several copies with `-Shard 0..N-1 -Of N` to use more cores | About 40 minutes with four copies |
| `Test-FadeSuggestions.ps1` | Asks `BeatAnalysis` for each cached track's suggestions (the same calls the screen makes) and prints how close they came, by genre | Several minutes for a quarter of the tracks (`-Every 4`); a full run has not been timed |

Re-run `Build-AnalysisCache.ps1 -Force` only if what `BeatAnalysis` measures from the audio changes (the cache holds its onset and loudness curves). A change to how suggestions are chosen only needs the build and `Test-FadeSuggestions.ps1`.

`Test-FadeSuggestions.ps1 -Detail results.csv` saves a row per track, including how far the nearest suggestion was from the hand-set position, for looking at the misses.

### Baseline (November 2026, every fourth track, 373 tracks)

Share of tracks where a suggestion starts within the stated distance of the hand-set start:

| | Best guess within 0.15 s | One of five within 0.15 s | One of five within 1 s | One of five within 2.5 s |
|---|---|---|---|---|
| Fade-in | 30% | 44% | 69% | 82% |
| Fade-out (given the hand-set fade-in) | 5% | 21% | 29% | 37% |

By genre, one of five fade-in suggestions within 0.15 s: Downbeat 76%, Reggae 56%, Drum & Bass 52%, Jazz 50%, Latin 48%, Hip-Hop 45%, Christmas 44%, Soul 42%, Funk 41%, Disco 17%, Dub 12%. The smaller genres here have under 20 tracks in the sample, so treat their figures as rough.

---

## Things tried that did not help

Measured on the fade-in, one of five within 0.15 s:

- Using a 4- or 8-second window instead of 2 seconds: slightly worse.
- Adding a rise in drum activity (onset strength) to the score: no change.
- Always offering the first sound in the track as a suggestion: no change overall, and the best guess got worse.
- Moving each suggestion to the strongest bass hit nearby instead of the sharpest jump in loudness: worse.

## Ideas not yet tried

- **Fade-out.** It is the weak half. Candidates: score each phrase by how well it loops (the end running into the start), prefer phrases just before a drop in energy, and offer phrases at half and double the fade-in length.
- **Downbeats.** Nothing detects which beat is beat one. When a fade-in suggestion misses narrowly, it is far more often early (by up to two seconds) than late.
- **Disco and Dub** score far below the other genres and are worth looking at on their own.
- **Fade length.** Suggestions are always 16 beats; the hand-set ones are often 8 or 32.
