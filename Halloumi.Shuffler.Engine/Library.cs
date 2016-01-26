using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Halloumi.BassEngine;
using Halloumi.BassEngine.Helpers;
using Halloumi.BassEngine.Models;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.Engine.Models;
using IdSharp.Tagging.ID3v2;
using Track = Halloumi.Shuffler.Engine.Models.Track;

namespace Halloumi.Shuffler.Engine
{
    /// <summary>
    ///     Represents a cache-able library of mp3 tracks.
    /// </summary>
    public class Library
    {
        /// <summary>
        ///     Shuffler filter settings
        /// </summary>
        public enum ShufflerFilter
        {
            None,
            ShuflerTracks,
            NonShufflerTracks
        }

        /// <summary>
        ///     TrackRank filter settings
        /// </summary>
        public enum TrackRankFilter
        {
            None,
            Forbidden,
            Unranked,
            BearablePlus,
            GoodPlus
        }

        /// <summary>
        ///     The all filter
        /// </summary>
        private const string AllFilter = "(All)";

        /// <summary>
        ///     The "unknown" value.
        /// </summary>
        private const string NoValue = "(None)";

        private bool _cancelImport;

        /// <summary>
        ///     Initializes a new instance of the Library class.
        /// </summary>
        public Library(BassPlayer bassPlayer)
        {
            Tracks = new List<Track>();
            AlbumCovers = new Dictionary<string, Image>();
            BassPlayer = bassPlayer;

            LinkedSampleLibrary = new LinkedSampleLibrary(this);
            Playlists = new List<Playlist>();
        }

        /// <summary>
        ///     Initializes a new instance of the Library class.
        /// </summary>
        public Library()
            : this(null)
        {
        }

        /// <summary>
        ///     Gets or sets the bass player.
        /// </summary>
        private BassPlayer BassPlayer { get; }

        /// <summary>
        ///     Gets or sets the tracks in the library
        /// </summary>
        private List<Track> Tracks { get; }

        /// <summary>
        ///     Gets or sets the play-lists in the library
        /// </summary>
        private List<Playlist> Playlists { get; set; }

        /// <summary>
        ///     Gets or sets the folder where the mp3 files for the library are kept
        /// </summary>
        public string LibraryFolder { get; set; }

        /// <summary>
        ///     Gets or sets the folder where the m3u play-list files for the library are kept
        /// </summary>
        public string PlaylistFolder { get; set; }

        /// <summary>
        ///     Gets or sets the folder where the shuffler extended attribute files for the library are kept
        /// </summary>
        public string ShufflerFolder
        {
            get { return BassPlayer.ExtendedAttributeFolder; }
            set { BassPlayer.ExtendedAttributeFolder = value; }
        }

        /// <summary>
        ///     Gets the name of the file where the track data is cached.
        /// </summary>
        private static string LibraryCacheFilename
            => Path.Combine(ApplicationHelper.GetUserDataPath(), "Halloumi.Shuffler.Library.xml");

        /// <summary>
        ///     Gets or sets the a cached collection of album covers.
        /// </summary>
        private Dictionary<string, Image> AlbumCovers { get; }

        /// <summary>
        ///     Gets the linked sample library.
        /// </summary>
        public LinkedSampleLibrary LinkedSampleLibrary { get; private set; }

        public Track GetTrack(string artist, string title, decimal length)
        {
            artist = artist.ToLower();
            title = title.ToLower();

            var track = Tracks.FirstOrDefault(
                x => x.Artist.ToLower() == artist && x.Title.ToLower() == title && x.Length == length)
                        ?? Tracks.Where(x => x.Artist.ToLower() == artist && x.Title.ToLower() == title)
                            .OrderByDescending(x => Math.Abs(x.Length - length))
                            .FirstOrDefault();

            return track;
        }

        /// <summary>
        ///     Gets a list of genres.
        /// </summary>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="playlistFilter">The play-list filter.</param>
        /// <param name="shufflerFilter">The shuffler filter.</param>
        /// <param name="minBpm">The min BPM.</param>
        /// <param name="maxBpm">The max BPM.</param>
        /// <param name="trackRankFilter">The track rank filter.</param>
        /// <param name="excludePlaylistFilter">The exclude play-list filter.</param>
        /// <returns>
        ///     A list of genres.
        /// </returns>
        public List<Genre> GetGenres(string searchFilter,
            string playlistFilter,
            ShufflerFilter shufflerFilter,
            int minBpm,
            int maxBpm,
            TrackRankFilter trackRankFilter,
            string excludePlaylistFilter)
        {
            return
                GetTracks("", "", "", "", searchFilter, playlistFilter, shufflerFilter, minBpm, maxBpm, trackRankFilter,
                    excludePlaylistFilter)
                    .OrderBy(t => t.Genre)
                    .Where(t => t.Genre != "" && t.Genre != "(All)")
                    .Select(t => t.Genre)
                    .Distinct()
                    .Select(g => new Genre(g))
                    .ToList();
        }

        /// <summary>
        ///     Gets a list of genres.
        /// </summary>
        /// <returns>
        ///     A list of genres.
        /// </returns>
        public List<Genre> GetAllGenres()
        {
            return GetGenres("", "", ShufflerFilter.None, 0, 1000, TrackRankFilter.None, "");
        }

        /// <summary>
        ///     Gets all albums.
        /// </summary>
        /// <returns>A list of all albums</returns>
        public List<Album> GetAllAlbums()
        {
            return GetAlbums(null, null, "", "", ShufflerFilter.None, 0, 1000, TrackRankFilter.None, "");
        }

        /// <summary>
        ///     Gets a list of albums.
        /// </summary>
        /// <param name="genreFilters">The genre filters.</param>
        /// <param name="albumArtistFilters">The album artist filters.</param>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="playlistFilter">The play-list filter.</param>
        /// <param name="shufflerFilter">The shuffler filter.</param>
        /// <param name="minBpm">The minimum BPM.</param>
        /// <param name="maxBpm">The maximum BPM.</param>
        /// <param name="trackRankFilter">The track rank filter.</param>
        /// <param name="excludePlaylistFilter">The exclude play-list filter.</param>
        /// <returns>
        ///     A list of albums matching the criteria.
        /// </returns>
        public List<Album> GetAlbums(List<string> genreFilters,
            List<string> albumArtistFilters,
            string searchFilter,
            string playlistFilter,
            ShufflerFilter shufflerFilter,
            int minBpm,
            int maxBpm,
            TrackRankFilter trackRankFilter,
            string excludePlaylistFilter)
        {
            if (genreFilters == null)
            {
                genreFilters = new List<string> {""};
            }

            if (albumArtistFilters == null)
            {
                albumArtistFilters = new List<string> {""};
            }

            var albums = new List<string>();
            foreach (var genre in genreFilters)
            {
                foreach (var albumArtist in albumArtistFilters)
                {
                    albums.AddRange(
                        GetTracks(genre, "", albumArtist, "", searchFilter, playlistFilter, shufflerFilter, minBpm,
                            maxBpm, trackRankFilter, excludePlaylistFilter)
                            .Union(GetTracks(genre, albumArtist, "", "", searchFilter, playlistFilter, shufflerFilter,
                                minBpm, maxBpm, trackRankFilter, excludePlaylistFilter))
                            .OrderBy(t => t.Album)
                            .Where(t => t.Album != "")
                            .Select(t => t.Album)
                            .Distinct()
                            .ToList());
                }
            }

            return albums
                .Distinct()
                .OrderBy(a => a)
                .Select(a => new Album(a))
                .ToList();
        }

        /// <summary>
        ///     Gets all album artists.
        /// </summary>
        /// <returns> A list of all album artists.</returns>
        public List<Artist> GetAllAlbumArtists()
        {
            return GetAlbumArtists(null, "", "", ShufflerFilter.None, 0, 1000, TrackRankFilter.None, "");
        }

        /// <summary>
        ///     Gets a filtered list of album artists.
        /// </summary>
        /// <param name="genreFilters">The genre filters.</param>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="playlistFilter">The play-list filter.</param>
        /// <param name="shufflerFilter">The shuffler filter.</param>
        /// <param name="minBpm">The minimum BPM.</param>
        /// <param name="maxBpm">The maximum BPM.</param>
        /// <param name="trackRankFilter">The track rank filter.</param>
        /// <param name="excludePlaylistFilter">The exclude play-list filter.</param>
        /// <returns>
        ///     A list of album artists matching the criteria.
        /// </returns>
        public List<Artist> GetAlbumArtists(List<string> genreFilters,
            string searchFilter,
            string playlistFilter,
            ShufflerFilter shufflerFilter,
            int minBpm,
            int maxBpm,
            TrackRankFilter trackRankFilter,
            string excludePlaylistFilter)
        {
            if (genreFilters == null)
            {
                genreFilters = new List<string> {""};
            }

            var artists = new List<string>();
            foreach (var genreFilter in genreFilters)
            {
                artists.AddRange(
                    GetTracks(genreFilter, "", "", "", searchFilter, playlistFilter, shufflerFilter, minBpm, maxBpm,
                        trackRankFilter, excludePlaylistFilter)
                        .OrderBy(t => t.AlbumArtist)
                        .Where(t => t.AlbumArtist != "")
                        .Select(t => t.AlbumArtist)
                        .Distinct()
                        .ToList());
            }
            return artists
                .Distinct()
                .OrderBy(a => a)
                .Where(a => a != "(All)")
                .Select(a => new Artist(a))
                .ToList();
        }

        /// <summary>
        ///     Gets all tracks for an album.
        /// </summary>
        /// <param name="albumName">Name of the album.</param>
        /// <returns>A list of tracks</returns>
        public List<Track> GetAllTracksForAlbum(string albumName)
        {
            return GetTracks("", "", "", albumName, "", "", ShufflerFilter.None, 0, 1000, TrackRankFilter.None, "");
        }

        /// <summary>
        ///     Gets a filtered list of artists.
        /// </summary>
        /// <param name="genreFilter">The genre filter.</param>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="playlistFilter">The play-list filter.</param>
        /// <param name="shufflerFilter">The shuffler filter.</param>
        /// <param name="minBpm">The minimum BPM.</param>
        /// <param name="maxBpm">The maximum BPM.</param>
        /// <param name="trackRankFilter">The track rank filter.</param>
        /// <param name="excludePlaylistFilter">The exclude play-list filter.</param>
        /// <returns>
        ///     A list of artists matching the criteria.
        /// </returns>
        public List<Artist> GetArtists(string genreFilter,
            string searchFilter,
            string playlistFilter,
            ShufflerFilter shufflerFilter,
            int minBpm,
            int maxBpm,
            TrackRankFilter trackRankFilter,
            string excludePlaylistFilter)
        {
            var artists =
                GetTracks(genreFilter, "", "", "", searchFilter, playlistFilter, shufflerFilter, minBpm, maxBpm,
                    trackRankFilter, excludePlaylistFilter)
                    .OrderBy(t => t.AlbumArtist)
                    .Where(t => t.AlbumArtist != "")
                    .Select(t => t.AlbumArtist)
                    .Union(GetTracks(genreFilter, "", "", "", searchFilter, playlistFilter, shufflerFilter, minBpm,
                        maxBpm, trackRankFilter, excludePlaylistFilter)
                        .Where(t => t.Artist != "")
                        .OrderBy(t => t.Artist)
                        .Select(t => t.Artist))
                    .Distinct()
                    .Select(a => new Artist(a))
                    .ToList();

            return artists;
        }

        /// <summary>
        ///     Gets a list of all artists.
        /// </summary>
        /// <returns>
        ///     A list of all artists
        /// </returns>
        public List<Artist> GetAllArtists()
        {
            return GetArtists("", "", "", ShufflerFilter.None, 0, 1000, TrackRankFilter.None, "");
        }

        /// <summary>
        ///     Gets a list of tracks.
        /// </summary>
        /// <returns>
        ///     A list tracks
        /// </returns>
        public List<Track> GetTracks()
        {
            return GetTracks(null, null, null, "", "", ShufflerFilter.None, 0, 1000, TrackRankFilter.None, "");
        }

        /// <summary>
        ///     Gets a filtered list of tracks.
        /// </summary>
        /// <param name="genreFilters">The genre filters.</param>
        /// <param name="artistFilters">The artist filters.</param>
        /// <param name="albumFilters">The album filters.</param>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="playlistFilter">The play-list filter.</param>
        /// <param name="shufflerFilter">The shuffler filter.</param>
        /// <param name="minBpm">The minimum BPM.</param>
        /// <param name="maxBpm">The maximum BPM.</param>
        /// <param name="trackRankFilter">The track rank filter.</param>
        /// <param name="excludePlaylistFilter">The exclude play-list filter.</param>
        /// <returns>
        ///     A list tracks matching the criteria.
        /// </returns>
        public List<Track> GetTracks(List<string> genreFilters,
            List<string> artistFilters,
            List<string> albumFilters,
            string searchFilter,
            string playlistFilter,
            ShufflerFilter shufflerFilter,
            int minBpm,
            int maxBpm,
            TrackRankFilter trackRankFilter,
            string excludePlaylistFilter)
        {
            var tracks = new List<Track>();

            if (genreFilters == null) genreFilters = new List<string>();
            if (artistFilters == null) artistFilters = new List<string>();
            if (albumFilters == null) albumFilters = new List<string>();

            if (genreFilters.Contains("")) genreFilters.Clear();
            if (artistFilters.Contains("")) artistFilters.Clear();
            if (albumFilters.Contains("")) albumFilters.Clear();

            if (genreFilters.Count == 0) genreFilters.Add("");
            if (artistFilters.Count == 0) artistFilters.Add("");
            if (albumFilters.Count == 0) albumFilters.Add("");

            foreach (var genre in genreFilters)
            {
                foreach (var artist in artistFilters)
                {
                    foreach (var album in albumFilters)
                    {
                        tracks.AddRange(
                            GetTracks(genre, "", artist, album, searchFilter, playlistFilter, shufflerFilter, minBpm,
                                maxBpm, trackRankFilter, excludePlaylistFilter)
                                .Union(GetTracks(genre, artist, "", album, searchFilter, playlistFilter, shufflerFilter,
                                    minBpm, maxBpm, trackRankFilter, excludePlaylistFilter))
                                .Distinct()
                                .ToList());
                    }
                }
            }

            tracks = tracks
                .Distinct()
                .OrderBy(t => t.AlbumArtist)
                .ThenBy(t => t.Album)
                .ThenBy(t => t.TrackNumber)
                .ThenBy(t => t.Artist)
                .ThenBy(t => t.Title)
                .ToList();

            if (shufflerFilter != ShufflerFilter.ShuflerTracks) return tracks;

            var distinctTracks = new List<Track>();
            foreach (
                var track in tracks.Where(track => distinctTracks.Count(t => t.Description == track.Description) == 0))
            {
                distinctTracks.Add(track);
            }
            tracks.Clear();
            tracks.AddRange(distinctTracks);

            return tracks;
        }

        /// <summary>
        ///     Gets a filtered list of tracks.
        /// </summary>
        /// <param name="genreFilter">The genre filter.</param>
        /// <param name="artistFilter">The artist filter.</param>
        /// <param name="albumArtistFilter">The album artist filter.</param>
        /// <param name="albumFilter">The album filter.</param>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="playlistFilter">The play-list filter.</param>
        /// <param name="shufflerFilter">The shuffler filter.</param>
        /// <param name="minBpm">The minimum BPM.</param>
        /// <param name="maxBpm">The maximum BPM.</param>
        /// <param name="trackRankFilter">The track rank filter.</param>
        /// <param name="excludePlaylistFilter">The exclude play-list filter.</param>
        /// <returns>
        ///     A list tracks matching the criteria.
        /// </returns>
        public List<Track> GetTracks(string genreFilter,
            string artistFilter,
            string albumArtistFilter,
            string albumFilter,
            string searchFilter,
            string playlistFilter,
            ShufflerFilter shufflerFilter,
            int minBpm,
            int maxBpm,
            TrackRankFilter trackRankFilter,
            string excludePlaylistFilter)
        {
            genreFilter = genreFilter.Replace(AllFilter, "").ToLower().Trim();
            albumFilter = albumFilter.Replace(AllFilter, "").ToLower().Trim();
            artistFilter = artistFilter.Replace(AllFilter, "").ToLower().Trim();
            albumArtistFilter = albumArtistFilter.Replace(AllFilter, "").ToLower().Trim();

            if (maxBpm == 0) maxBpm = 200;

            List<Track> tracks;
            lock (Tracks)
            {
                tracks = Tracks
                    .Where(t => genreFilter == "" || t.Genre.ToLower() == genreFilter)
                    .Where(t => albumFilter == "" || t.Album.ToLower() == albumFilter)
                    .Where(t => artistFilter == "" || t.Artist.ToLower() == artistFilter)
                    .Where(t => albumArtistFilter == "" || t.AlbumArtist.ToLower() == albumArtistFilter)
                    .Where(
                        t =>
                            (t.StartBpm >= minBpm && t.StartBpm <= maxBpm) || (t.EndBpm >= minBpm && t.EndBpm <= maxBpm))
                    .Distinct()
                    .OrderBy(t => t.AlbumArtist)
                    .ThenBy(t => t.Album)
                    .ThenBy(t => t.TrackNumber)
                    .ThenBy(t => t.Artist)
                    .ThenBy(t => t.Title)
                    .ToList();
            }

            if (!string.IsNullOrEmpty(searchFilter))
            {
                tracks = tracks.Where(t => t.Genre.ToLower().Contains(searchFilter)
                                           || t.AlbumArtist.ToLower().Contains(searchFilter)
                                           || t.Artist.ToLower().Contains(searchFilter)
                                           || t.Album.ToLower().Contains(searchFilter)
                                           || t.Title.ToLower().Contains(searchFilter)).ToList();
            }

            if (!string.IsNullOrEmpty(playlistFilter))
            {
                var playlist = GetPlaylistByName(playlistFilter);
                if (playlist != null)
                {
                    var playlistTracks = new HashSet<string>(playlist.Tracks.Select(t => t.Description).Distinct());
                    tracks = tracks.Where(t => playlistTracks.Contains(t.Description)).ToList();
                }
            }

            if (!string.IsNullOrEmpty(excludePlaylistFilter))
            {
                var excludePlaylist = GetPlaylistByName(excludePlaylistFilter);
                if (excludePlaylist != null)
                {
                    var excludeTracks = new HashSet<string>(excludePlaylist.Tracks.Select(t => t.Description).Distinct());
                    tracks = tracks.Where(t => !excludeTracks.Contains(t.Description)).ToList();
                }
            }

            switch (trackRankFilter)
            {
                case TrackRankFilter.Unranked:
                    tracks = tracks.Where(t => t.Rank == 1).ToList();
                    break;
                case TrackRankFilter.Forbidden:
                    tracks = tracks.Where(t => t.Rank == 0).ToList();
                    break;
                case TrackRankFilter.BearablePlus:
                    tracks = tracks.Where(t => t.Rank >= 2).ToList();
                    break;
                case TrackRankFilter.GoodPlus:
                    tracks = tracks.Where(t => t.Rank >= 3).ToList();
                    break;
                case TrackRankFilter.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(trackRankFilter), trackRankFilter, null);
            }

            if (shufflerFilter != ShufflerFilter.None)
            {
                tracks = shufflerFilter == ShufflerFilter.ShuflerTracks
                    ? tracks.Where(t => t.IsShufflerTrack).ToList()
                    : tracks.Where(t => !t.IsShufflerTrack).ToList();
            }

            if (shufflerFilter != ShufflerFilter.ShuflerTracks)
                return tracks;

            var distinctTracks = new List<Track>();
            foreach (
                var track in tracks.Where(track => distinctTracks.Count(t => t.Description == track.Description) == 0))
            {
                distinctTracks.Add(track);
            }

            tracks.Clear();
            tracks.AddRange(distinctTracks);

            return tracks;
        }

        /// <summary>
        ///     Gets the tracks by description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>
        ///     A list of tracks matching the description
        /// </returns>
        public List<Track> GetTracksByDescription(string description)
        {
            description = description.ToLower().Trim();
            return Tracks.Where(t => t.Description.ToLower() == description).ToList();
        }

        /// <summary>
        ///     Gets a track from the library matching the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The associated track, or null if it doesn't exist</returns>
        public Track GetTrackByFilename(string filename)
        {
            filename = filename.ToLower().Trim();

            lock (Tracks)
            {
                var track = Tracks.FirstOrDefault(t => t.Filename.ToLower() == filename);
                if (track != null && !File.Exists(track.Filename))
                    track = null;

                return track;
            }
        }

        /// <summary>
        ///     Gets the album cover.
        /// </summary>
        /// <param name="album">The album.</param>
        /// <returns>The associated (small) album cover</returns>
        public Image GetAlbumCover(Album album)
        {
            return album == null ? null : GetAlbumCover(album.Name);
        }

        /// <summary>
        ///     Gets a album cover.
        /// </summary>
        /// <param name="albumName">Name of the album.</param>
        /// <returns>The album cover</returns>
        public Image GetAlbumCover(string albumName)
        {
            if (albumName == AllFilter) return null;

            if (AlbumCovers.ContainsKey(albumName)) return AlbumCovers[albumName];

            var tracks = GetAllTracksForAlbum(albumName);
            if (tracks.Count == 0) return null;

            LoadAlbumCover(tracks[0]);

            return AlbumCovers.ContainsKey(albumName) ? AlbumCovers[albumName] : null;
        }

        /// <summary>
        ///     Loads the library from the cache.
        /// </summary>
        public void LoadFromCache()
        {
            if (!File.Exists(LibraryCacheFilename)) return;

            var tracks = SerializationHelper<List<Track>>.FromXmlFile(LibraryCacheFilename);

            lock (Tracks)
            {
                Tracks.Clear();
                Tracks.AddRange(tracks.ToArray());
            }
        }

        public void ReloadTrackMetaData(string filename)
        {
            if (!File.Exists(filename)) return;
            if (!filename.ToLower().EndsWith(".mp3")) return;

            var track = GetTrackByFilename(filename);
            if (track == null)
                track = LoadNewTrack(filename);
            else
            {
                var dateModified = GetTrackLastModified(filename);
                LoadTrackDetails(track, dateModified);
            }

            track.OriginalDescription = track.Description;

            if (track.EndBpm == 0 || track.EndBpm == 100) track.EndBpm = track.Bpm;
            if (track.StartBpm == 0 || track.StartBpm == 100) track.StartBpm = track.Bpm;
        }

        /// <summary>
        ///     Imports a track.
        /// </summary>
        /// <param name="filename">The filename of the track.</param>
        public void ImportTrack(string filename)
        {
            if (!File.Exists(filename)) return;
            if (!filename.ToLower().EndsWith(".mp3")) return;

            var track = GetTrackByFilename(filename);
            if (track == null)
            {
                track = LoadNewTrack(filename);
            }
            else
            {
                var dateModified = GetTrackLastModified(filename);
                if (track.LastModified != dateModified || track.IsShufflerTrack)
                {
                    LoadTrackDetails(track, dateModified);
                }
            }
            track.OriginalDescription = track.Description;
            if (track.EndBpm == 0 || track.EndBpm == 100) track.EndBpm = track.Bpm;
            if (track.StartBpm == 0 || track.StartBpm == 100) track.StartBpm = track.Bpm;
        }

        private DateTime GetTrackLastModified(string filename)
        {
            var trackDetails = TrackHelper.GuessTrackDetailsFromFilename(filename);
            var shufflerFile = GetShufflerAttributeFile(trackDetails.Artist + " - " + trackDetails.Title);
            var shufflerDate = DateTime.MinValue;
            if (File.Exists(shufflerFile))
            {
                shufflerDate = File.GetLastWriteTime(shufflerFile);
                //shufflerDate = DateTime.Now;
            }

            var dateModified = File.GetLastWriteTime(filename);
            return shufflerDate > dateModified ? shufflerDate : dateModified;
        }

        /// <summary>
        ///     Reloads a track.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public Track ReloadTrack(string filename)
        {
            if (!File.Exists(filename)) return null;
            var track = GetTrackByFilename(filename);
            if (track == null) return null;

            GuessTrackDetailsFromFileName(track);
            var dateModified = File.GetLastWriteTime(filename);
            LoadTrackDetails(track, dateModified);

            return track;
        }

        /// <summary>
        ///     Imports the tracks.
        /// </summary>
        /// <param name="folder">The folder.</param>
        public void ImportTracks(string folder)
        {
            if (folder == "") folder = LibraryFolder;
            if (!folder.StartsWith(LibraryFolder)) return;

            _cancelImport = false;

            var files = FileSystemHelper.SearchFiles(folder, "*.mp3", true);

            // remove tracks that don't exist in file system
            var currentTracks = Tracks.Where(t => t.Filename.StartsWith(folder)).ToList();
            var missingTracks =
                currentTracks.TakeWhile(track => !_cancelImport)
                    .Where(track => !files.Contains(track.Filename))
                    .ToList();
            lock (Tracks)
            {
                foreach (var track in missingTracks.TakeWhile(track => !_cancelImport))
                {
                    Tracks.Remove(track);

                    if (_cancelImport) break;
                    RemoveTrackFromAllPlaylists(track);
                }
            }

            ParallelHelper.ForEach(files.TakeWhile(file => !_cancelImport), ImportTrack);

            //// update or add tracks from file system
            //foreach (var file in files.TakeWhile(file => !_cancelImport))
            //{
            //    ImportTrack(file);
            //}

            SaveCache();
        }

        /// <summary>
        ///     Cancels the import.
        /// </summary>
        public void CancelImport()
        {
            _cancelImport = true;
        }

        /// <summary>
        ///     Imports the details of all tracks from the library folder into the library
        /// </summary>
        public void ImportTracks()
        {
            ImportTracks(LibraryFolder);
        }

        /// <summary>
        ///     Renames a genre.
        /// </summary>
        /// <param name="oldGenre">The old genre name.</param>
        /// <param name="newGenre">The new genre name.</param>
        public void RenameGenre(string oldGenre, string newGenre)
        {
            if (newGenre.Trim() == "" || oldGenre.Trim() == "") return;
            if (newGenre.Trim() == NoValue || oldGenre.Trim() == NoValue) return;

            var tracks = GetTracks(oldGenre, "", "", "", "", "", ShufflerFilter.None, 0, 1000, TrackRankFilter.None, "");
            UpdateGenre(tracks, newGenre);
        }

        /// <summary>
        ///     Updates the genre for a list of tracks
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <param name="newGenre">The new genre name.</param>
        public void UpdateGenre(List<Track> tracks, string newGenre)
        {
            if (newGenre.Trim() == "" || newGenre.Trim() == NoValue) return;

            foreach (var track in tracks)
            {
                track.Genre = newGenre;
                SaveTrack(track);
            }

            SaveCache();
        }

        /// <summary>
        ///     Renames an album.
        /// </summary>
        /// <param name="oldAlbum">The old album name.</param>
        /// <param name="newAlbum">The new album name.</param>
        public void RenameAlbum(string oldAlbum, string newAlbum)
        {
            if (newAlbum.Trim() == "" || oldAlbum.Trim() == "") return;
            if (newAlbum.Trim() == NoValue || oldAlbum.Trim() == NoValue) return;

            var tracks = GetAllTracksForAlbum(oldAlbum);
            UpdateAlbum(tracks, newAlbum);
        }

        /// <summary>
        ///     Updates the album for a list of tracks
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <param name="newAlbum">The new album name.</param>
        public void UpdateAlbum(List<Track> tracks, string newAlbum)
        {
            if (newAlbum.Trim() == "" || newAlbum.Trim() == NoValue) return;

            foreach (var track in tracks)
            {
                track.Album = newAlbum;
                SaveTrack(track);
            }

            SaveCache();
        }

        /// <summary>
        ///     Renames an artist.
        /// </summary>
        /// <param name="oldArtist">The old artist name.</param>
        /// <param name="newArtist">The new artist name.</param>
        public void RenameArtist(string oldArtist, string newArtist)
        {
            if (newArtist.Trim() == "" || oldArtist.Trim() == "") return;
            if (newArtist.Trim() == NoValue || oldArtist.Trim() == NoValue) return;

            var tracks = GetTracks("", "", oldArtist, "", "", "", ShufflerFilter.None, 0, 1000, TrackRankFilter.None, "");
            foreach (var track in tracks)
            {
                track.AlbumArtist = newArtist;
                if (track.Artist == oldArtist) track.Artist = newArtist;
                SaveTrack(track);
            }

            tracks = GetTracks("", oldArtist, "", "", "", "", ShufflerFilter.None, 0, 1000, TrackRankFilter.None, "");
            foreach (var track in tracks)
            {
                track.Artist = newArtist;
                SaveTrack(track);
            }

            SaveCache();
        }

        /// <summary>
        ///     Updates the artist for a list of tracks
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <param name="newArtist">The new artist name.</param>
        public void UpdateArtist(List<Track> tracks, string newArtist)
        {
            if (newArtist.Trim() == "" || newArtist.Trim() == NoValue) return;

            foreach (var track in tracks)
            {
                if (track.Artist == track.AlbumArtist)
                {
                    track.Artist = newArtist;
                    track.AlbumArtist = newArtist;
                }
                else
                {
                    track.Artist = newArtist;
                }

                SaveTrack(track);
            }

            SaveCache();
        }

        /// <summary>
        ///     Updates the album artist for a list of tracks
        /// </summary>
        /// <param name="album">The album.</param>
        /// <param name="newAlbumArtist">The new album name.</param>
        public void UpdateAlbumArtist(string album, string newAlbumArtist)
        {
            if (newAlbumArtist.Trim() == "" || newAlbumArtist.Trim() == NoValue) return;
            var tracks = GetAllTracksForAlbum(album);
            foreach (var track in tracks)
            {
                track.AlbumArtist = newAlbumArtist;
                SaveTrack(track);
            }

            SaveCache();
        }

        /// <summary>
        ///     Updates the track number.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="trackNumber">The track number.</param>
        public void UpdateTrackNumber(Track track, int trackNumber)
        {
            track.TrackNumber = trackNumber;
            SaveTrack(track);
            SaveCache();
        }

        /// <summary>
        ///     Removes the shuffler details for a track
        /// </summary>
        /// <param name="track">The track.</param>
        public void RemoveShufflerDetails(Track track)
        {
            if (!track.IsShufflerTrack) return;
            File.Delete(track.ShufflerAttribuesFile);
            ReloadTrack(track.Filename);
        }

        /// <summary>
        ///     Updates the track title
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="title">The title.</param>
        /// <param name="updateAxillaryFiles">If set to true, will update axillary files.</param>
        public void UpdateTitle(Track track, string title, bool updateAxillaryFiles)
        {
            track.Title = title;
            SaveTrack(track, updateAxillaryFiles);
            SaveCache();
        }

        public void CopyAudioFromAnotherTrack(Track destinationTrack, Track sourceTrack)
        {
            File.Move(destinationTrack.Filename, destinationTrack.Filename + ".old");

            try
            {
                File.Copy(sourceTrack.Filename, destinationTrack.Filename);
            }
            catch (Exception)
            {
                try
                {
                    File.Move(destinationTrack.Filename + ".old", destinationTrack.Filename);
                }
                catch
                {
                    // ignored
                }
                throw;
            }

            var title = destinationTrack.Title;
            var album = destinationTrack.Album;
            var albumArtist = destinationTrack.AlbumArtist;
            var artist = destinationTrack.Artist;
            var genre = destinationTrack.Genre;
            var trackNumber = destinationTrack.TrackNumber;

            UpdateTrackDetails(destinationTrack, artist, title, album, albumArtist, genre, trackNumber, false);

            var albumCover = GetAlbumCover(destinationTrack.Album);
            if (albumCover != null) SetTrackAlbumCover(destinationTrack, albumCover);
            File.Delete(destinationTrack.Filename + ".old");

            ReloadTrack(destinationTrack.Filename);
        }

        /// <summary>
        ///     Updates the track title
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="artist">The artist.</param>
        /// <param name="title">The title.</param>
        /// <param name="album">The album.</param>
        /// <param name="albumArtist">The album artist.</param>
        /// <param name="genre">The genre.</param>
        /// <param name="trackNumber">The title.</param>
        /// <param name="updateAxillaryFiles">If set to true, will update axillary files.</param>
        /// <returns></returns>
        public bool UpdateTrackDetails(Track track, string artist, string title, string album, string albumArtist,
            string genre, int trackNumber, bool updateAxillaryFiles)
        {
            var oldTitle = track.Title;
            var oldAlbum = track.Album;
            var oldAlbumArtist = track.AlbumArtist;
            var oldArtist = track.Artist;
            var oldGenre = track.Genre;
            var oldTrackNumber = track.TrackNumber;

            track.Title = title;
            track.Album = album;
            track.AlbumArtist = albumArtist;
            track.Artist = artist;
            track.Genre = genre;
            track.TrackNumber = trackNumber;

            var result = SaveTrack(track, updateAxillaryFiles);

            if (result)
            {
                SaveCache();
            }
            else
            {
                track.Title = oldTitle;
                track.Album = oldAlbum;
                track.AlbumArtist = oldAlbumArtist;
                track.Artist = oldArtist;
                track.Genre = oldGenre;
                track.TrackNumber = oldTrackNumber;
            }
            return result;
        }

        /// <summary>
        ///     Saves the track details to a cache file
        /// </summary>
        public void SaveCache()
        {
            SerializationHelper<List<Track>>.ToXmlFile(Tracks, LibraryCacheFilename);
        }


        /// <summary>
        ///     Cleans the folder images.
        /// </summary>
        public void CleanLibrary()
        {
            var filenames = Tracks.Select(t => t.Filename).Distinct().ToList();

            var tracksToRemove = new List<Track>();
            foreach (var filename in filenames)
            {
                var matchingTracks = Tracks.Where(x => x.Filename == filename).ToList();

                if (!File.Exists(filename))
                {
                    tracksToRemove.AddRange(matchingTracks);
                    matchingTracks.Clear();
                }

                while (matchingTracks.Count > 1)
                {
                    var lastTrack = matchingTracks.Last();
                    tracksToRemove.Add(lastTrack);
                    matchingTracks.Remove(lastTrack);
                }
            }

            lock (Tracks)
            {
                foreach (var track in tracksToRemove)
                {
                    Tracks.Remove(track);
                }
            }

            CleanSpecialShufflerAlbumTracks();

            SaveCache();

            DeleteSurpusSystemFilesInLibrary();
        }

        private void CleanSpecialShufflerAlbumTracks()
        {
            var shufflerAlbumPath = Path.Combine(LibraryFolder, "#Shuffler");
            var shufflerAlbumTracks = Tracks.Where(t => t.Filename.StartsWith(shufflerAlbumPath)).ToList();
            var otherTracks = Tracks.Except(shufflerAlbumTracks).ToList();
            var duplicatedTracks =
                shufflerAlbumTracks.Where(
                    t => otherTracks.Count(ot => StringHelper.FuzzyCompare(t.Description, ot.Description)) != 0)
                    .ToList();

            foreach (var track in duplicatedTracks)
            {
                try
                {
                    Tracks.Remove(track);
                    File.Delete(track.Filename);
                }
                catch
                {
                    // ignored
                }
            }
        }

        private void DeleteSurpusSystemFilesInLibrary()
        {
            // delete annoying WMV files
            var wmvFiles = FileSystemHelper.SearchFiles(LibraryFolder, "AlbumArt_*.jpg", true);
            foreach (var wmvFile in wmvFiles)
            {
                try
                {
                    File.Delete(wmvFile);
                }
                catch
                {
                    // ignored
                }
            }

            // delete annoying thumbnail files
            var thumbnails = FileSystemHelper.SearchFiles(LibraryFolder, "thumbs.db", true);
            foreach (var thumbnail in thumbnails)
            {
                try
                {
                    File.Delete(thumbnail);
                }
                catch
                {
                    // ignored
                }
            }

            var folderFiles = FileSystemHelper.SearchFiles(LibraryFolder, "folder.jpg", true);
            foreach (var folderFile in folderFiles)
            {
                var attributes = File.GetAttributes(folderFile);
                if (attributes == FileAttributes.Hidden) File.SetAttributes(folderFile, FileAttributes.Normal);
            }
        }

        /// <summary>
        ///     Gets all play-lists.
        /// </summary>
        /// <returns>A list of all play-lists</returns>
        public List<Playlist> GetAllPlaylists()
        {
            return Playlists.OrderBy(p => p.Name).ToList();
        }

        /// <summary>
        ///     Gets a play-list by name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///     A play-list matching the specified name, or null if not found
        /// </returns>
        public Playlist GetPlaylistByName(string name)
        {
            name = name.ToLower();
            return Playlists.FirstOrDefault(p => p.Name.ToLower() == name);
        }

        /// <summary>
        ///     Loads all the play-lists in the play-list folder
        /// </summary>
        public void LoadPlaylists()
        {
            var playlistFiles = FileSystemHelper.SearchFiles(PlaylistFolder, "*.m3u", false);

            Playlists = new List<Playlist>();
            foreach (var playlist in playlistFiles.Select(LoadPlaylist))
            {
                Playlists.Add(playlist);
            }
        }

        /// <summary>
        ///     Adds tracks to play-list.
        /// </summary>
        /// <param name="playlist">The play-list.</param>
        /// <param name="tracks">The tracks.</param>
        public void AddTracksToPlaylist(Playlist playlist, List<Track> tracks)
        {
            if (tracks == null || playlist == null) return;

            foreach (var track in tracks)
            {
                if (!playlist.Tracks.Contains(track)) playlist.Tracks.Add(track);
            }
            SavePlaylist(playlist);
        }

        /// <summary>
        ///     Removes tracks from a play-list.
        /// </summary>
        /// <param name="playlist">The play-list.</param>
        /// <param name="tracks">The tracks to remove.</param>
        public void RemoveTracksFromPlaylist(Playlist playlist, List<Track> tracks)
        {
            if (tracks == null || playlist == null) return;

            foreach (var track in tracks)
            {
                if (playlist.Tracks.Contains(track)) playlist.Tracks.Remove(track);
            }
            SavePlaylist(playlist);
        }

        /// <summary>
        ///     Removes a track from all play-lists.
        /// </summary>
        /// <param name="track">The track.</param>
        public void RemoveTrackFromAllPlaylists(Track track)
        {
            foreach (var playlist in Playlists)
            {
                if (playlist.Tracks.Contains(track))
                {
                    playlist.Tracks.Remove(track);
                    SavePlaylist(playlist);
                }
            }
        }

        /// <summary>
        ///     Gets all the play-lists that contain a specific track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of play-lists that contain the track</returns>
        public List<Playlist> GetPlaylistsForTrack(Track track)
        {
            return GetAllPlaylists().Where(p => p.Tracks.Contains(track)).Distinct().ToList();
        }

        /// <summary>
        ///     Gets a distinct list of all play-lists that the specified tracks are in
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <returns>A distinct list of all play-lists that the specified tracks are in.</returns>
        public List<Playlist> GetPlaylistsForTracks(List<Track> tracks)
        {
            var playlists = new List<Playlist>();
            foreach (var track in tracks)
            {
                playlists.AddRange(GetPlaylistsForTrack(track));
            }
            return playlists.Distinct().OrderBy(p => p.Name).ToList();
        }

        /// <summary>
        ///     Creates the new play-list.
        /// </summary>
        /// <param name="playlistName">Name of the play-list.</param>
        /// <returns></returns>
        public Playlist CreateNewPlaylist(string playlistName)
        {
            playlistName = FileSystemHelper.StripInvalidFileNameChars(playlistName.Trim());

            var playlist = Playlists.FirstOrDefault(p => p.Name.ToLower() == playlistName.ToLower());
            if (playlist != null) return playlist;

            playlist = new Playlist {Name = playlistName};
            playlist.Filename = Path.Combine(PlaylistFolder, playlist.Name) + ".m3u";
            playlist.Tracks = new List<Track>();

            Playlists.Add(playlist);

            return playlist;
        }

        /// <summary>
        ///     Loads a play-list.
        /// </summary>
        /// <param name="playlistFile">The play-list file.</param>
        /// <returns>A play-list object</returns>
        public Playlist LoadPlaylist(string playlistFile)
        {
            var playlist = new Playlist
            {
                Filename = playlistFile,
                Name = Path.GetFileNameWithoutExtension(playlistFile)
            };
            playlist.Name = StringHelper.TitleCase(playlist.Name);

            var modified = false;

            foreach (var entry in PlaylistHelper.GetPlaylistEntries(playlistFile))
            {
                var entryFile = Path.GetFileName(entry.Path);
                var entryTitle = entry.Title.ToLower();
                var entryArtist = entry.Artist.ToLower();

                var track = Tracks.FirstOrDefault(t => t.Filename == entry.Path);

                if (track == null)
                {
                    if (entry.Path.StartsWith(LibraryFolder))
                    {
                        track = LoadNewTrack(entry.Path);
                    }

                    if (track == null)
                        track = Tracks.FirstOrDefault(t => Path.GetFileName(t.Filename) == entryFile);

                    if (track == null)
                        track =
                            Tracks.FirstOrDefault(
                                t => t.Artist.ToLower() == entryArtist && t.Title.ToLower() == entryTitle);

                    if (track == null)
                        track =
                            Tracks.FirstOrDefault(
                                t => t.AlbumArtist.ToLower() == entryArtist && t.Title.ToLower() == entryTitle);

                    modified = true;
                }

                if (track != null) playlist.Tracks.Add(track);
            }

            if (modified) SavePlaylist(playlist);

            return playlist;
        }

        /// <summary>
        ///     Saves the play-list.
        /// </summary>
        /// <param name="playlist">The play-list.</param>
        public void SavePlaylist(Playlist playlist)
        {
            if (playlist == null) return;
            if (playlist.Tracks.Count == 0)
            {
                try
                {
                    File.Delete(playlist.Filename);
                }
                catch
                {
                    // ignored
                }
                return;
            }

            var content = new StringBuilder();
            content.AppendLine("#EXTM3U");
            foreach (var track in playlist.Tracks)
            {
                content.AppendLine("#EXTINF:" + track.FullLength.ToString("0") + "," + track.Artist + " - " +
                                   track.Title);

                content.AppendLine(track.Filename);
            }

            File.WriteAllText(playlist.Filename, content.ToString(), Encoding.UTF8);
        }

        /// <summary>
        ///     Imports the shuffler details.
        /// </summary>
        /// <param name="importFolder">The import folder.</param>
        /// <param name="deleteAfterImport">If set to true, will delete Shuffler files after importing them</param>
        public void ImportShufflerDetails(string importFolder, bool deleteAfterImport)
        {
            if (!Directory.Exists(importFolder)) return;
            var importFiles = FileSystemHelper.SearchFiles(importFolder,
                "*.ExtendedAttributes.txt;*.AutomationAttributes.xml;", false);
            foreach (var importFile in importFiles)
            {
                var fileName = Path.GetFileName(importFile);
                if (fileName == null) continue;

                var existingFile = Path.Combine(ShufflerFolder, fileName);
                if (!File.Exists(existingFile))
                {
                    FileSystemHelper.Copy(importFile, existingFile);
                }
                else
                {
                    var existingFileDate = File.GetLastWriteTime(existingFile);
                    var importFileDate = File.GetLastWriteTime(importFile);

                    if (existingFileDate < importFileDate)
                    {
                        FileSystemHelper.Copy(importFile, existingFile);
                        File.SetLastWriteTime(existingFile, importFileDate);
                    }
                    else if (!deleteAfterImport && existingFileDate != importFileDate)
                    {
                        FileSystemHelper.Copy(existingFile, importFile);
                        File.SetLastWriteTime(importFile, existingFileDate);
                    }
                }
                if (deleteAfterImport) File.Delete(importFile);
            }

            if (!deleteAfterImport)
            {
                var existingFiles = FileSystemHelper.SearchFiles(ShufflerFolder,
                    "*.ExtendedAttributes.txt;*.AutomationAttributes.xml;", false);
                foreach (var existingFile in existingFiles)
                {
                    var fileName = Path.GetFileName(existingFile);
                    if (fileName == null) continue;

                    var importFile = Path.Combine(importFolder, fileName);

                    if (File.Exists(importFile)) continue;

                    var existingFileDate = File.GetLastWriteTime(existingFile);
                    FileSystemHelper.Copy(existingFile, importFile);
                    File.SetLastWriteTime(importFile, existingFileDate);
                }
            }

            AutomationAttributes.ClearCache();
        }

        public void SaveRank(Track track)
        {
            var bassTrack = BassPlayer.LoadTrack(track.Filename);
            bassTrack.Rank = track.Rank;
            BassPlayer.SaveExtendedAttributes(bassTrack);
        }


        /// <summary>
        ///     Calculates the length track and saves it in the tag data
        /// </summary>
        /// <param name="track">The track.</param>
        private void CalculateAndSaveTrackLength(Track track)
        {
            var length = decimal.Round(Convert.ToDecimal(AudioStreamHelper.GetLength(track.Filename)), 3);

            if (length == decimal.Round(track.FullLength, 3)) return;

            track.Length = length;
            track.FullLength = length;
            SaveTrack(track);
        }

        /// <summary>
        ///     Loads a new track from a file and adds it to the library
        /// </summary>
        /// <param name="filename">The filename.</param>
        private Track LoadNewTrack(string filename)
        {
            if (!File.Exists(filename)) return null;
            var track = new Track {Filename = filename};

            GuessTrackDetailsFromFileName(track);

            var dateModified = GetTrackLastModified(filename);
            LoadTrackDetails(track, dateModified);

            lock (Tracks)
            {
                Tracks.Add(track);
            }

            return track;
        }

        /// <summary>
        ///     Guesses the artist and title of a track from its filename.
        /// </summary>
        /// <param name="track">The track.</param>
        private static void GuessTrackDetailsFromFileName(Track track)
        {
            var trackDetails = TrackHelper.GuessTrackDetailsFromFilename(track.Filename);
            track.Title = trackDetails.Title;
            track.Artist = trackDetails.Artist;
            track.AlbumArtist = trackDetails.AlbumArtist;

            track.TrackNumber = trackDetails.TrackNumber != "" ? track.TrackNumber : 0;
        }

        /// <summary>
        ///     Loads the track details from the tags in the associate mp3
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public Track LoadNonLibraryTrack(string filename)
        {
            if (!File.Exists(filename)) return null;
            var track = new Track
            {
                Filename = filename,
                LastModified = File.GetLastAccessTime(filename)
            };
            LoadTrackDetails(track, track.LastModified);
            return track;
        }

        /// <summary>
        ///     Loads the track details from the tags in the associate mp3
        /// </summary>
        /// <param name="track">The track to load the details of.</param>
        /// <param name="dateLastModified">The date the file was last modified (passed in here to avoid loading it twice).</param>
        private void LoadTrackDetails(Track track, DateTime dateLastModified)
        {
            DebugHelper.WriteLine("Library - LoadTrackDetails - " + track.Description);

            track.LastModified = dateLastModified;

            var tagKey = "";
            if (ID3v2Tag.DoesTagExist(track.Filename))
            {
                var tags = new ID3v2Tag(track.Filename);

                if (!string.IsNullOrEmpty(tags.Artist)) track.Artist = tags.Artist.Trim();
                if (!string.IsNullOrEmpty(tags.Artist)) track.AlbumArtist = tags.Artist.Trim();
                if (!string.IsNullOrEmpty(tags.Title)) track.Title = tags.Title.Trim();
                if (!string.IsNullOrEmpty(tags.Album)) track.Album = tags.Album.Trim();
                if (!string.IsNullOrEmpty(tags.Genre)) track.Genre = tags.Genre.Trim();
                if (!string.IsNullOrEmpty(tags.InitialKey))
                {
                    tagKey = tags.InitialKey.Trim();
                    track.Key = tagKey;
                }

                LoadArtistAndAlbumArtist(track);

                if (tags.LengthMilliseconds.HasValue) track.Length = (decimal) tags.LengthMilliseconds/1000M;

                decimal bpm;
                if (decimal.TryParse(tags.BPM, out bpm)) track.Bpm = bpm;

                track.Bpm = BpmHelper.NormaliseBpm(track.Bpm);
                track.EndBpm = track.Bpm;
                track.StartBpm = track.Bpm;
                track.Bpm = BpmHelper.GetAdjustedBpmAverage(track.StartBpm, track.EndBpm);

                int trackNumber;
                var trackNumberTag = (tags.TrackNumber + "/").Split('/')[0].Trim();
                if (int.TryParse(trackNumberTag, out trackNumber)) track.TrackNumber = trackNumber;

                if (GenreCode.IsGenreCode(track.Genre)) track.Genre = GenreCode.GetGenre(track.Genre);
                if (track.Artist == "") track.Artist = NoValue;
                if (track.AlbumArtist == "") track.AlbumArtist = NoValue;
                if (track.Title == "") track.Title = NoValue;
                if (track.Album == "") track.Album = NoValue;
                if (track.Genre == "") track.Genre = NoValue;
            }

            track.OriginalDescription = track.Description;
            track.FullLength = track.Length;

            CalculateAndSaveTrackLength(track);
            var attributes = LoadShufflerDetails(track);

            UpdateKey(track, attributes, tagKey);

            if (attributes == null) return;

            track.Bpm = BpmHelper.GetAdjustedBpmAverage(track.StartBpm, track.EndBpm);
        }

        private void UpdateKey(Track track, Dictionary<string, string> attributes, string tagKey)
        {
            var attributeKey = (attributes == null) ? "" : attributes.ContainsKey("Key") ? attributes["Key"] : "";

            if (tagKey != "" && attributeKey == "" && track.IsShufflerTrack)
            {
                if (attributes != null && !attributes.ContainsKey("Key"))
                {
                    attributes.Add("Key", tagKey);
                }
                else
                {
                    if (attributes != null) attributes["Key"] = tagKey;
                }
                BassPlayer.SaveExtendedAttributes(attributes, GetShufflerAttributeFile(track.Description));
            }
            else if (tagKey == "" && attributeKey != "")
            {
                SaveTrack(track);
            }

            track.Key = string.IsNullOrEmpty(attributeKey) ? tagKey : attributeKey;
        }

        /// <summary>
        ///     Loads the artist and album artist for a track
        /// </summary>
        /// <param name="track">The track.</param>
        private static void LoadArtistAndAlbumArtist(Track track)
        {
            if (track.Title.Contains("/"))
            {
                var data = track.Title.Split('/').ToList();
                track.Artist = data[0].Trim();
                track.Title = data[1].Trim();
            }
        }


        public bool SaveNonLibraryTrack(Track track)
        {
            return SaveTrack(track, false);
        }

        /// <summary>
        ///     Saves a track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="updateAxillaryFiles">If set to true, update axillary files.</param>
        /// <returns></returns>
        public bool SaveTrack(Track track, bool updateAxillaryFiles = true)
        {
            var tags = new ID3v2Tag(track.Filename)
            {
                Genre = track.Genre.Replace(NoValue, ""),
                Album = track.Album.Replace(NoValue, ""),
                TrackNumber = track.TrackNumber.ToString(),
                LengthMilliseconds = Convert.ToInt32(track.FullLength*1000M),
                BPM = track.Bpm.ToString("0.00"),
                InitialKey = track.Key
            };

            if (track.Artist == track.AlbumArtist)
            {
                tags.Artist = track.Artist.Replace(NoValue, "");
                tags.Title = track.Title.Replace(NoValue, "");
            }
            else
            {
                tags.Artist = track.AlbumArtist.Replace(NoValue, "");
                tags.Title = track.Artist.Replace(NoValue, "") + " / " + track.Title.Replace(NoValue, "");
            }
            try
            {
                tags.Save(track.Filename);
            }
            catch
            {
                return false;
            }

            // attempt to rename the file based on the track details
            var filename = GetFileNameFromTrackDetails(track);
            if (filename != Path.GetFileName(track.Filename))
            {
                try
                {
                    var directoryName = Path.GetDirectoryName(track.Filename);
                    if (directoryName != null)
                    {
                        filename = Path.Combine(directoryName, filename);
                        File.Move(track.Filename, filename);
                        track.Filename = filename;
                    }
                }
                catch
                {
                    return false;
                }

                if (updateAxillaryFiles)
                {
                    try
                    {
                        if (track.IsShufflerTrack) RenameShufferFiles(track);

                        // if filename changed, save any associated play-list files
                        foreach (var playlist in Playlists.Where(playlist => playlist.Tracks.Contains(track)))
                        {
                            SavePlaylist(playlist);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            track.OriginalDescription = track.Description;

            return true;
        }

        /// <summary>
        ///     Updates the Shuffler files after a track has been changed.
        ///     Assumes the OriginalDescription is the old description,
        ///     and that the ShufflerAttribuesFile/ShufflerMixesFile properties
        ///     point to the old files
        /// </summary>
        /// <param name="track">The track.</param>
        private void RenameShufferFiles(Track track)
        {
            try
            {
                var newAttributesFile = GetShufflerAttributeFile(track.Description);
                var newMixesFile = GetShufflerMixesFile(track);

                File.Move(track.ShufflerAttribuesFile, newAttributesFile);
                File.Move(track.ShufflerMixesFile, newMixesFile);

                track.ShufflerAttribuesFile = newAttributesFile;
                track.ShufflerMixesFile = newMixesFile;

                var replacer = new TextReplacer(track.OriginalDescription + ",", track.Description + ",", false, false,
                    false, false);

                replacer.Replace(ShufflerFolder, "*.Mixes.txt", false);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Gets the file name from track details.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>The generated filename</returns>
        private string GetFileNameFromTrackDetails(Track track)
        {
            var filename = "";
            if (track.TrackNumber > 0) filename += track.TrackNumber.ToString("D2") + " - ";
            filename += track.AlbumArtist + " - ";
            if (track.Artist == track.AlbumArtist) filename += track.Title;
            else filename += track.Artist + " / " + track.Title;
            filename += ".mp3";
            filename = FileSystemHelper.StripInvalidFileNameChars(filename);
            return filename;
        }

        /// <summary>
        ///     Loads and caches an album cover.
        /// </summary>
        /// <param name="track">The track.</param>
        private void LoadAlbumCover(Track track)
        {
            try
            {
                var path = Path.GetDirectoryName(track.Filename);
                if (path == null) return;

                var albumArtImagePath = Path.Combine(path, "AlbumArtSmall.jpg");
                var folderImagePath = Path.Combine(path, "folder.jpg");

                var albumArtImageDate = DateTime.MinValue;
                if (File.Exists(albumArtImagePath)) albumArtImageDate = File.GetLastWriteTime(albumArtImagePath);

                var folderImageDate = DateTime.MinValue;
                if (File.Exists(folderImagePath)) folderImageDate = File.GetLastWriteTime(folderImagePath);

                if (!File.Exists(folderImagePath))
                {
                    if (ID3v2Tag.DoesTagExist(track.Filename))
                    {
                        var tags = new ID3v2Tag(track.Filename);
                        if (tags.PictureList.Count > 0)
                        {
                            using (Image folderImage = new Bitmap(tags.PictureList[0].Picture))
                            {
                                ImageHelper.SaveJpg(folderImagePath, folderImage);
                            }
                        }
                    }
                }

                if (!File.Exists(albumArtImagePath) || albumArtImageDate < folderImageDate)
                {
                    if (File.Exists(folderImagePath))
                    {
                        using (Image image = new Bitmap(folderImagePath))
                        {
                            using (var smallImage = ImageHelper.Resize(image, new Size(150, 150)))
                            {
                                ImageHelper.SaveJpg(albumArtImagePath, smallImage);
                                File.SetAttributes(albumArtImagePath, FileAttributes.Hidden);
                            }
                        }
                    }
                }

                if (File.Exists(albumArtImagePath))
                {
                    Image image = new Bitmap(albumArtImagePath);
                    AlbumCovers.Add(track.Album, image);
                }
            }
            catch (Exception e)
            {
                DebugHelper.WriteLine(e.ToString());
            }
        }

        /// <summary>
        ///     Sets the track album cover.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="image">The image.</param>
        public void SetTrackAlbumCover(Track track, Image image)
        {
            if (track == null) return;
            if (image == null) return;
            if (!ID3v2Tag.DoesTagExist(track.Filename)) return;

            var tags = new ID3v2Tag(track.Filename);
            if (tags.PictureList.Count > 0) tags.PictureList.Clear();

            var picture = tags.PictureList.AddNew();

            if (picture != null)
            {
                picture.PictureType = PictureType.CoverFront;
                picture.MimeType = "image/jpeg";

                using (var stream = new MemoryStream())
                {
                    ImageHelper.SaveJpg(stream, image);
                    picture.PictureData = stream.ToArray();
                }
            }
            tags.Save(track.Filename);
        }

        /// <summary>
        ///     Loads the shuffler details for a track
        /// </summary>
        /// <param name="track">The track.</param>
        private Dictionary<string, string> LoadShufflerDetails(Track track)
        {
            var shufflerAttribuesFile = GetShufflerAttributeFile(track.Description);
            track.IsShufflerTrack = File.Exists(shufflerAttribuesFile);

            if (!track.IsShufflerTrack) return null;

            track.ShufflerAttribuesFile = shufflerAttribuesFile;
            track.ShufflerMixesFile = GetShufflerMixesFile(track);

            var attributes = PlaylistHelper.GetShufflerAttributes(track.ShufflerAttribuesFile);

            if (attributes.ContainsKey("StartBPM"))
                track.StartBpm = BpmHelper.NormaliseBpm(ConversionHelper.ToDecimal(attributes["StartBPM"], track.Bpm));
            if (attributes.ContainsKey("EndBPM"))
                track.EndBpm = BpmHelper.NormaliseBpm(ConversionHelper.ToDecimal(attributes["EndBPM"], track.Bpm));

            if (attributes.ContainsKey("Rank")) track.Rank = ConversionHelper.ToInt(attributes["Rank"], 1);

            decimal start = 0;
            if (attributes.ContainsKey("FadeIn")) start = ConversionHelper.ToDecimal(attributes["FadeIn"], start);
            var end = track.Length;
            if (attributes.ContainsKey("FadeOut")) end = ConversionHelper.ToDecimal(attributes["FadeOut"], end);
            var length = end - start;

            var inLoopCount = 0;
            if (attributes.ContainsKey("StartLoopCount"))
                inLoopCount = ConversionHelper.ToInt(attributes["StartLoopCount"], inLoopCount);

            decimal inLoopLength = 0;
            if (attributes.ContainsKey("FadeInLengthInSeconds"))
                inLoopLength = ConversionHelper.ToDecimal(attributes["FadeInLengthInSeconds"]);
            if (inLoopLength > 0) track.StartBpm = BpmHelper.GetBpmFromLoopLength(Convert.ToDouble(inLoopLength));

            inLoopCount = inLoopCount - 1;
            if (inLoopCount > 0) length = length + (inLoopCount*inLoopLength);

            decimal skipLength = 0;
            if (attributes.ContainsKey("SkipLengthInSeconds"))
                skipLength = ConversionHelper.ToDecimal(attributes["SkipLengthInSeconds"]);
            if (skipLength > 0) length = length - skipLength;

            track.PowerDown = false;
            if (attributes.ContainsKey("PowerDown"))
                track.PowerDown = ConversionHelper.ToBoolean(attributes["PowerDown"]);

            if (attributes.ContainsKey("Key")) track.Key = attributes["Key"];

            decimal outLoopLength = 0;
            if (attributes.ContainsKey("FadeOutLengthInSeconds"))
                outLoopLength = ConversionHelper.ToDecimal(attributes["FadeOutLengthInSeconds"], 0);
            if (outLoopLength > 0) track.EndBpm = BpmHelper.GetBpmFromLoopLength(Convert.ToDouble(outLoopLength));

            track.Length = length;

            return attributes;
        }

        /// <summary>
        ///     Gets the shuffler attribute file for a track
        /// </summary>
        /// <param name="trackDescription">The track description.</param>
        /// <returns>
        ///     The shuffler attribute file
        /// </returns>
        private string GetShufflerAttributeFile(string trackDescription)
        {
            var filename = $"{trackDescription}.ExtendedAttributes.txt";
            filename = FileSystemHelper.StripInvalidFileNameChars(filename);
            filename = Path.Combine(ShufflerFolder, filename);
            return filename;
        }

        /// <summary>
        ///     Gets the shuffler mixes file for a track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>
        ///     The shuffler mixes file
        /// </returns>
        private string GetShufflerMixesFile(Track track)
        {
            var filename = $"{track.Description}.Mixes.txt";
            filename = FileSystemHelper.StripInvalidFileNameChars(filename);
            filename = Path.Combine(ShufflerFolder, filename);
            return filename;
        }
    }
}