# Audio Library

The library layer lives in `Halloumi.Shuffler.Engine/`. It is purely data and logic — no audio playback, no UI. All classes here are safe to use from any thread as long as callers handle their own locking.

---

## Library

`Halloumi.Shuffler.Engine/Library.cs`

The track database. Scans a folder of MP3 files, reads ID3 tags via IdSharp, and caches everything to XML so subsequent launches are fast.

### Configuration properties

```
LibraryFolder        Root folder of the MP3 collection
PlaylistFolder       Where playlists are saved/loaded
ShufflerFolder       Where per-track extended attribute XML files live
```

### Loading

```
LoadFromDatabase()   Load the SQLite cache. If there is none, migrate the old XML cache once.
SaveToDatabase()     Write the current track list to the SQLite cache.
```

Cache location: `%APPDATA%\Halloumi Shuffler\Halloumi.Shuffler.Library.db` (see `Database/LibraryDatabase.cs`). The old `Halloumi.Shuffler.Library.xml` in the same folder is only read when the database doesn't exist.

### Querying tracks

```
GetTracks(
    filter,              // ShufflerFilter: None | ShufflerTracks | NonShufflerTracks
    rankFilter,          // TrackRankFilter: None | Forbidden | Unranked | BearablePlus | GoodPlus
    genreFilter,         // Genre name or ""
    artistFilter,        // Artist name or ""
    albumFilter,         // Album name or ""
    searchText,          // Free-text search across artist/title
    collectionFilter,    // Playlist name or ""
    minBpm, maxBpm       // BPM range (0 = no limit)
)
```

Returns a filtered, sorted `List<Track>`.

Other query methods:

```
GetTrack(artist, title, length)       Find a single track by identity
GetAllTracksForAlbum(albumName)       All tracks on an album
GetAllTracksForArtist(artistName)     All tracks by an artist
GetAllGenres()                        Distinct genre list
GetAllAlbums()                        Distinct album list
GetAllArtists()                       Distinct artist list
TrackCount()                          Total tracks in library
```

### Filters

**ShufflerFilter** — restricts to tracks that have (or don't have) extended attribute files:
- `None` — no filter
- `ShufflerTracks` — only tracks with a `.xml` in the Shuffler folder
- `NonShufflerTracks` — only tracks without one

**TrackRankFilter** — restricts by quality rating:
- `None` — no filter
- `Forbidden` — rank 0 only
- `Unranked` — rank 1 only
- `BearablePlus` — rank ≥ 2
- `GoodPlus` — rank ≥ 3

---

## MixLibrary

`Halloumi.Shuffler.Engine/MixLibrary.cs`

Stores a compatibility rating for every track-pair transition that has been evaluated. The playlist generator uses these ratings to build mixes that flow well.

### Mix rank

| Value | Label |
|---|---|
| 0 | Forbidden |
| 1 | Unranked |
| 2 | Bearable |
| 3 | Good |
| 4 | Very Good |
| 5 | Excellent |

A pair with no stored rating is Unranked (1) if the outgoing track's end BPM and the incoming track's start BPM are within `UnrankedBpmVariance` percent of each other (half and double time count as the same BPM), and Forbidden (0) otherwise. The same range decides the unranked to/from track lists, so the mixable tracks panel, the library's unranked counts, the least-mixed roulette and the playlist generator's Unranked mode all follow it.

### Configuration

```
AvailableTracks       The set of tracks to consider when generating playlists
ShufflerFolder        Where mix data is persisted
UnrankedBpmVariance   BPM range (percent) for an unrated pair to count as Unranked; default 5,
                      set from Settings by ShufflerApplication (Options → Unranked BPM Range)
```

### API

```
LoadFromDatabase()                      Load ratings from XML
SaveToDatabase()                        Persist ratings to XML
SetMixLevel(fromTrack, toTrack, level) Store a rating for a transition
GetMixLevel(fromTrack, toTrack)        Retrieve a stored rating
HasExtendedMix(fromTrack, toTrack)     Check if a custom fade curve exists
```

Location: `<ShufflerFolder>\Halloumi.Shuffler.MixLibrary.xml`

Loading uses `Parallel.ForEach` to speed up deserialisation on large libraries.

---

## Playlist generation

`Halloumi.Shuffler.Engine/TrackSelector.cs`, called through `ShufflerApplication.GeneratePlaylist` with a `PlaylistGenerationRequest` (saved in `%APPDATA%\Halloumi\Halloumi.Shuffler.PlaylistGenerationSettings.xml`).

**Best Mix, Variety and Extra Variety** grow many candidate playlists a track at a time:

1. **Candidates.** From the last track, only tracks rated Good or better (3–5) as a mix, plus Bearable (2) according to *Allow Bearables*.
2. **Filters.** Tracks already used; title, artist and genre clumping; the *Key Mixing* minimum; BPM direction (only applied if something is left); extended mixes; excluded mixes.
3. **Score** for each mix (`GetAverageTrackAndMixAndKeyRank`), a weighted average on a 0–5 scale:

   | Part | Weight |
   |---|---|
   | Mix rating (+0.5 with an extended mix, else +0.25 if the track powers down) | 1.3 |
   | Incoming track's rank | 0.9 |
   | Key compatibility, raised by the mix rating (+1/+2/+3 for Good/Very Good/Excellent) and kept within 1–4 | 0.8 |
   | *Favour recently shuffled tracks* only: recency, 5 for a track first shuffled in the last three months, falling to 0 at three years (no date counts as old) | 0.4 |

   With recency on, a brand-new track gains up to about 0.6: enough for its Good mix to beat an old track's Very Good mix, but not an old track's Excellent mix.
4. **Pruning.** Each step keeps the best 3 × CPU-core candidates per playlist and the best 50 × CPU-core playlists. Variety drops a random half of the candidates first; Extra Variety sometimes drops every other one instead.
5. **Result.** The playlist with the highest average score. If it is one track short, the best-scoring next track is added.

Ranking playlists by their average so far front-loads quality. A playlist that uses its Excellent mixes early always leads one that saves them, so by halfway every surviving playlist has used them up. This shows most when the playlist covers most of a small pool, such as one genre at a fixed length.

**Save some of the best mixes for later** (`SpreadQualityEvenly`) changes steps 4 and 5:

- **Pruning** ranks playlists by an estimated final average (`GetSpreadScore`). That is the mixes so far plus one estimated mix for each slot still to fill. For each unused track, the estimate takes the best Good-or-better mix into it from another unused track or the playlist's open end. The best of these, one per remaining slot, are added up. Using up the best links lowers the estimate, so saving them is no longer penalised. Tracks with no Good link left add nothing. The links are scored once per generation (`BuildSpreadLinks`), and the per-step scoring runs in parallel.
- **Result** is chosen on the average of two numbers: the overall average and the weakest run of 5 consecutive mixes (`GetWeakestStretchRank`). An even playlist can beat one with a slightly higher average but a weak tail.

The estimate only helps when the good links are scarce. Generating 30 tracks from a large library behaves almost as before.

**Unranked** picks from unrated pairs within the unranked BPM range: best key match first, then the highest-ranked track, then the smallest BPM change.

---

## Track model (library)

`Halloumi.Shuffler.Engine/Models/Track.cs`

The serialisable track record. This is what `Library` stores and what the UI browses. It is distinct from the engine's `Track` class — see [architecture.md](architecture.md#two-track-models).

### Identity and metadata

```
Filename          Full path to the MP3 file
Artist            Track artist
Title             Track title
AlbumArtist       Album artist (may differ from track artist)
Album             Album name
Genre             Genre string
TrackNumber       Position within album
Length            Duration in decimal seconds
Bitrate           Audio bitrate
LastModified      File modification timestamp
```

### BPM

```
Bpm               Average BPM (StartBpm + EndBpm) / 2
StartBpm          BPM at the start of the track
EndBpm            BPM at the end of the track
TagBpm            BPM stored in the MP3 ID3 tag
CannotCalculateBpm  True if BPM detection failed
```

### Library state

```
Rank              Quality rating (0 = Forbidden … 5 = Excellent)
Key               Musical key if detected
IsShufflerTrack   True if the track has saved Shuffler details in Halloumi.Shuffler.ExtendedAttributes.xml
DateShuffled      Date the Shuffler details were first saved (from the Shuffled attribute)
PowerDown         True if this track uses the power-down fade effect
```

### Computed / formatted properties

```
Description           "Artist - Title"
OriginalDescription   Description as loaded (before any edits)
FullLength            Total audio duration (may differ from Length if truncated)
TrackNumberFormatted  "1", "2", etc., or "" if not set
LengthFormatted       "mm:ss" or "h:mm:ss"
FullLengthFormatted   Full duration formatted
RankDescription       "Excellent" / "Very Good" / "Good" / "Bearable" / "Forbidden" / "Unranked"
BitrateFormatted      Bitrate as a string
```

---

## Sample libraries

Both implement `ISampleLibrary`, which defines the contract for listing and loading samples.

### LoopLibrary

`Halloumi.Shuffler.Engine/Samples/LoopLibrary.cs`

Loads `.wav` files from a configured folder. Each file becomes a sample that can be triggered from the sampler.

### TrackSampleLibrary

`Halloumi.Shuffler.Engine/Samples/TrackSampleLibrary.cs`

Manages samples that are associated with specific tracks. Associations are stored in per-track extended attribute files. When a track is queued, its associated samples are loaded into the sampler automatically.

---

## Extended attributes

Extended attributes are per-track settings stored as XML files in the Shuffler folder. They hold everything that isn't in the MP3 tags and isn't global:

| Data | Description |
|---|---|
| Fade in/out points | Start, end, and volume envelope for each fade section |
| Loop counts | How many times to repeat the fade-in/out |
| Skip section | Region to skip during playback |
| Pre-fade section | Section played before the main fade-in |
| BPM override | Manual BPM correction |
| Power-down flag | Whether to play the power-down effect |
| Automation triggers | TrackFXTrigger and SampleTrigger instances |
| Manual mix curve | Recorded crossfade volume curve |
| Linked samples | Which samples play with this track |
| Date shuffled | `Shuffled` attribute: when Shuffler details were first saved (see below) |

Helpers that read/write these files:
- `ExtendedAttributesHelper` — fade, loop, skip, BPM data
- `AutomationAttributesHelper` — FX trigger and sample trigger data

### Date shuffled

`ExtendedAttributesHelper` stamps `Shuffled=<yyyy-MM-dd>` the first time a track gets any extended attributes, and carries it over on every later save (the save paths rebuild the attribute set from the track, so the helper re-adds it). Clearing a track's Shuffler details removes it.

Tracks shuffled before the date was recorded were backfilled from the `ShufflerAudioDatabase` git history by [tools/shuffled-backfill/backfill_shuffled.py](../tools/shuffled-backfill/backfill_shuffled.py), using the date of the first commit each track appears in (renamed tracks keep the original name's date). Tracks already shuffled when the history starts are dated 2016-04-10. `ShufflerHelper.LoadShufflerDetails` parses the value into `DateShuffled`.

---

## Playlist models

`Halloumi.Shuffler.Engine/Models/Playlist.cs` / `PlaylistEntry.cs`

Simple data models for saving and loading playlists. A `Playlist` is a named ordered list of `PlaylistEntry` records, each holding a reference to a `Track`. Playlists are serialised to `.m3u`-style or XML files in the configured playlist folder.
