using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioLibrary.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;
using PlaylistHelper = Halloumi.Shuffler.AudioLibrary.Helpers.PlaylistHelper;
using TrackHelper = Halloumi.Shuffler.AudioLibrary.Helpers.TrackHelper;

namespace Halloumi.Shuffler.AudioLibrary
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
            TrackHelper.BassPlayer = bassPlayer;
        }

        /// <summary>
        ///     Initializes a new instance of the Library class.
        /// </summary>
        public Library()
            : this(null)
        {
        }

        /// <summary>
        ///     Gets or sets the tracks in the library
        /// </summary>
        private List<Track> Tracks { get; }


        /// <summary>
        ///     Gets the name of the file where the track data is cached.
        /// </summary>
        private static string LibraryCacheFilename
            => Path.Combine(ApplicationHelper.GetUserDataPath(), "Halloumi.Shuffler.Library.xml");

        /// <summary>
        ///     Gets or sets the folder where the mp3 files for the library are kept
        /// </summary>
        public string LibraryFolder { get; set; }

        public string PlaylistFolder
        {
            get { return PlaylistHelper.PlaylistFolder; }
            set { PlaylistHelper.PlaylistFolder = value; }
        }

        /// <summary>
        ///     Gets or sets the folder where the shuffler extended attribute files for the library are kept
        /// </summary>
        public string ShufflerFolder => ExtenedAttributesHelper.ExtendedAttributeFolder;


        public Track GetTrack(string artist, string title, decimal length = 0)
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
        public List<Track> GetTracks(string genreFilter = "",
            string artistFilter = "",
            string albumArtistFilter = "",
            string albumFilter = "",
            string searchFilter = "",
            string playlistFilter = "",
            ShufflerFilter shufflerFilter = ShufflerFilter.None,
            int minBpm = 0,
            int maxBpm = 200,
            TrackRankFilter trackRankFilter = TrackRankFilter.None,
            string excludePlaylistFilter = "")
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

            //var distinctTracks = new List<Track>();
            //foreach (
            //    var track in tracks.Where(track => distinctTracks.Count(t => t.Description == track.Description) == 0))
            //{
            //    distinctTracks.Add(track);
            //}

            //tracks.Clear();
            //tracks.AddRange(distinctTracks);

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
        ///     Reloads a track.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public Track LoadTrack(string filename)
        {
            var track = GetTrackByFilename(filename);

            if (track == null)
                track = LoadNewTrack(filename);
            else
                TrackHelper.LoadTrack(track);

            return track;
        }

        public void SaveRank(Track track)
        {
            TrackHelper.UpdateRank(track);
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

            ParallelHelper.ForEach(files.TakeWhile(file => !_cancelImport), file => { LoadTrack(file); });

            SaveToDatabase();
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

            SaveToDatabase();
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

            SaveToDatabase();
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

            SaveToDatabase();
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

            SaveToDatabase();
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

            SaveToDatabase();
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
            SaveToDatabase();
        }

        /// <summary>
        ///     Removes the shuffler details for a track
        /// </summary>
        /// <param name="track">The track.</param>
        public void RemoveShufflerDetails(Track track)
        {
            if (!track.IsShufflerTrack) return;
            File.Delete(track.ShufflerAttribuesFile);
            LoadTrack(track.Filename);
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
            SaveToDatabase();
        }

        public List<Playlist> GetAllPlaylists()
        {
            return PlaylistHelper.GetAllPlaylists();
        }

        public Playlist GetPlaylistByName(string name)
        {
            return PlaylistHelper.GetPlaylistByName(name);
        }

        public void LoadPlaylists()
        {
            PlaylistHelper.LoadPlaylists(this);
        }

        public void AddTracksToPlaylist(Playlist playlist, List<Track> tracks)
        {
            PlaylistHelper.AddTracksToPlaylist(playlist, tracks);
        }

        public void RemoveTracksFromPlaylist(Playlist playlist, List<Track> tracks)
        {
            PlaylistHelper.RemoveTracksFromPlaylist(playlist, tracks);
        }

        public void RemoveTrackFromAllPlaylists(Track track)
        {
            PlaylistHelper.RemoveTrackFromAllPlaylists(track);
        }

        public List<Playlist> GetPlaylistsForTrack(Track track)
        {
            return PlaylistHelper.GetPlaylistsForTrack(track);
        }

        public List<Playlist> GetPlaylistsForTracks(List<Track> tracks)
        {
            return PlaylistHelper.GetPlaylistsForTracks(tracks);
        }

        public Playlist CreateNewPlaylist(string playlistName)
        {
            return PlaylistHelper.CreateNewPlaylist(playlistName);
        }

        public Playlist LoadPlaylist(string playlistFile)
        {
            return PlaylistHelper.LoadPlaylist(playlistFile, this);
        }

        public void SavePlaylist(Playlist playlist)
        {
            PlaylistHelper.SavePlaylist(playlist);
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

            var albumCover = AlbumCoverHelper.GetAlbumCover(destinationTrack.Album);
            if (albumCover != null) AlbumCoverHelper.SetTrackAlbumCover(destinationTrack, albumCover);
            File.Delete(destinationTrack.Filename + ".old");

            LoadTrack(destinationTrack.Filename);
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
                SaveToDatabase();
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
        public void SaveToDatabase()
        {
            SerializationHelper<List<Track>>.ToXmlFile(Tracks, LibraryCacheFilename);
        }

        /// <summary>
        ///     Loads the library from the cache.
        /// </summary>
        public void LoadFromDatabase()
        {
            if (!File.Exists(LibraryCacheFilename)) return;
            try
            {
                var tracks = SerializationHelper<List<Track>>.FromXmlFile(LibraryCacheFilename);
                lock (Tracks)
                {
                    Tracks.Clear();
                    Tracks.AddRange(tracks.ToArray());
                }
            }
            catch
            {
                // ignored
            }
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

            SaveToDatabase();
        }


        ///// <summary>
        /////     Imports the shuffler details.
        ///// </summary>
        ///// <param name="importFolder">The import folder.</param>
        ///// <param name="deleteAfterImport">If set to true, will delete Shuffler files after importing them</param>
        //public void ImportShufflerDetails(string importFolder, bool deleteAfterImport)
        //{
        //    ShufflerHelper.ImportShufflerDetails(importFolder, deleteAfterImport);
        //}

        public Track LoadNonLibraryTrack(string filename)
        {
            return TrackHelper.LoadTrack(filename);
        }

        public bool SaveNonLibraryTrack(Track track)
        {
            return TrackHelper.SaveTrack(track);
        }

        /// <summary>
        ///     Saves a track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="updateAxillaryFiles">If set to true, update axillary files.</param>
        /// <returns></returns>
        public bool SaveTrack(Track track, bool updateAxillaryFiles = true)
        {
            if (!TrackHelper.SaveTrack(track)) return false;
            if (!TrackHelper.RenameTrack(track)) return false;

            if (!updateAxillaryFiles) return true;
            try
            {
                if (track.IsShufflerTrack) ShufflerHelper.RenameShufferFiles(track);

                // if filename changed, save any associated play-list files
                var playlists = PlaylistHelper.GetAllPlaylists().Where(playlist => playlist.Tracks.Contains(track));
                foreach (var playlist in playlists)
                {
                    PlaylistHelper.SavePlaylist(playlist);
                }
            }
            catch
            {
                // ignored
            }

            return true;
        }

        /// <summary>
        ///     Gets a album cover.
        /// </summary>
        /// <param name="albumName">Name of the album.</param>
        /// <returns>The album cover</returns>
        public Image GetAlbumCover(string albumName)
        {
            if (albumName == AllFilter) return null;

            var cover = AlbumCoverHelper.GetAlbumCover(albumName);
            if (cover != null) return cover;

            var tracks = GetAllTracksForAlbum(albumName);
            if (tracks.Count == 0) return null;

            AlbumCoverHelper.LoadAlbumCover(tracks[0]);
            return AlbumCoverHelper.GetAlbumCover(tracks[0].Album);
        }

        public void SetTrackAlbumCover(Track track, Image image)
        {
            AlbumCoverHelper.SetTrackAlbumCover(track, image);
        }

        private Track LoadNewTrack(string filename)
        {
            var track = TrackHelper.LoadTrack(filename);
            if (track == null) return null;

            lock (Tracks)
            {
                Tracks.Add(track);
            }
            return track;
        }
    }
}