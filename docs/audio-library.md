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
LoadFromDatabase()   Load cached XML. Falls back to scanning if cache is missing.
SaveToDatabase()     Write current track list to XML cache.
```

Cache location: `%APPDATA%\Halloumi\Halloumi.Shuffler.Library.xml`

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

### Configuration

```
AvailableTracks   The set of tracks to consider when generating playlists
ShufflerFolder    Where mix data is persisted
```

### API

```
LoadFromDatabase()                      Load ratings from XML
SaveToDatabase()                        Persist ratings to XML
SetMixLevel(fromTrack, toTrack, level) Store a rating for a transition
GetMixLevel(fromTrack, toTrack)        Retrieve a stored rating
HasExtendedMix(fromTrack, toTrack)     Check if a custom fade curve exists
```

Cache location: `%APPDATA%\Halloumi\Halloumi.Shuffler.MixLibrary.xml`

Loading uses `Parallel.ForEach` to speed up deserialisation on large libraries.

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
IsShufflerTrack   True if a .xml extended attributes file exists for this track
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

Helpers that read/write these files:
- `ExtendedAttributesHelper` — fade, loop, skip, BPM data
- `AutomationAttributesHelper` — FX trigger and sample trigger data

---

## Playlist models

`Halloumi.Shuffler.Engine/Models/Playlist.cs` / `PlaylistEntry.cs`

Simple data models for saving and loading playlists. A `Playlist` is a named ordered list of `PlaylistEntry` records, each holding a reference to a `Track`. Playlists are serialised to `.m3u`-style or XML files in the configured playlist folder.
