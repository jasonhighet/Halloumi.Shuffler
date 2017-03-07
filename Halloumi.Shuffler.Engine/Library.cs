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

        public string PlaylistFolder { get; set; }

        /// <summary>
        ///     Gets or sets the folder where the shuffler extended attribute files for the library are kept
        /// </summary>
        public string ShufflerFolder => ExtenedAttributesHelper.ShufflerFolder;


        public Track GetTrack(string artist, string title, decimal length = 0)
        {
            var tracks = Tracks.Where(x => string.Equals(x.Artist, artist, StringComparison.CurrentCultureIgnoreCase)
                                           && string.Equals(x.Title, title, StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            return tracks.Count == 1
                ? tracks[0]
                : tracks.OrderByDescending(x => Math.Abs(x.Length - length)).FirstOrDefault();
        }

        /// <summary>
        ///     Gets all tracks for an album.
        /// </summary>
        /// <param name="albumName">Name of the album.</param>
        /// <returns>A list of tracks</returns>
        public List<Track> GetAllTracksForAlbum(string albumName)
        {
            var albumFilter = new List<string> {albumName};
            return GetTracks(albumFilter: albumFilter);
        }

        /// <summary>
        ///     Gets all tracks for an album.
        /// </summary>
        /// <param name="artistName">Name of the album.</param>
        /// <returns>A list of tracks</returns>
        public List<Track> GetAllTracksForArtist(string artistName)
        {
            var artistFilter = new List<string> {artistName};
            return GetTracks(artistFilter: artistFilter);
        }

        public List<Genre> GetGenresFromTracks(List<Track> tracks)
        {
            return tracks.Select(x => x.Genre).Distinct().OrderBy(x => x).Select(x => new Genre(x)).ToList();
        }

        public List<Genre> GetAllGenres()
        {
            return GetGenresFromTracks(Tracks);
        }

        public List<Album> GetAlbumsFromTracks(List<Track> tracks)
        {
            return tracks.Select(x => x.Album).Distinct().OrderBy(x => x).Select(x => new Album(x)).ToList();
        }

        public List<Album> GetAllAlbums()
        {
            return GetAlbumsFromTracks(Tracks);
        }

        public List<Artist> GetArtistsFromTracks(List<Track> tracks)
        {
            return tracks.Select(x => x.Artist).Distinct().OrderBy(x => x).Select(x => new Artist(x)).ToList();
        }

        public List<Artist> GetAllArtists()
        {
            return GetArtistsFromTracks(Tracks);
        }

        public List<Artist> GetAlbumArtistsFromTracks(List<Track> tracks)
        {
            return tracks.Select(x => x.AlbumArtist).Distinct().OrderBy(x => x).Select(x => new Artist(x)).ToList();
        }

        public List<Artist> GetAllAlbumArtists()
        {
            return GetAlbumArtistsFromTracks(Tracks);
        }


        public int TrackCount()
        {
            return Tracks.Count;
        }

        /// <summary>
        ///     Gets a filtered list of tracks.
        /// </summary>
        /// <param name="genreFilters">The genre filters.</param>
        /// <param name="artistFilter">The artist filters.</param>
        /// <param name="albumFilter">The album filters.</param>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="collectionFilter">The play-list filter.</param>
        /// <param name="shufflerFilter">The shuffler filter.</param>
        /// <param name="minBpm">The minimum BPM.</param>
        /// <param name="maxBpm">The maximum BPM.</param>
        /// <param name="trackRankFilter">The track rank filter.</param>
        /// <param name="excludeCollectionFilter">The exclude play-list filter.</param>
        /// <returns>
        ///     A list tracks matching the criteria.
        /// </returns>
        public List<Track> GetTracks(List<string> genreFilters = null,
            List<string> artistFilter = null,
            List<string> albumFilter = null,
            string searchFilter = "",
            string collectionFilter = "",
            ShufflerFilter shufflerFilter = ShufflerFilter.None,
            int minBpm = 0,
            int maxBpm = 200,
            TrackRankFilter trackRankFilter = TrackRankFilter.None,
            string excludeCollectionFilter = "")
        {
            var genres = GetHashSet(genreFilters);
            var albums = GetHashSet(albumFilter);
            var artists = GetHashSet(artistFilter);

            if (maxBpm == 0) maxBpm = 200;

            Console.WriteLine(@"GET TRACKS!!!");

            var tracks = Tracks;
            if (!string.IsNullOrEmpty(collectionFilter))
            {
                tracks = CollectionHelper.GetTracksInCollection(collectionFilter);
            }

            tracks = tracks
                .Where(t => genres.Count == 0 || genres.Contains(t.Genre))
                .Where(t => albums.Count == 0 || albums.Contains(t.Album))
                .Where(t => artists.Count == 0 || artists.Contains(t.Artist) || artists.Contains(t.AlbumArtist))
                .Where(t => (t.StartBpm >= minBpm && t.StartBpm <= maxBpm) || (t.EndBpm >= minBpm && t.EndBpm <= maxBpm))
                .Distinct()
                .OrderBy(t => t.AlbumArtist)
                .ThenBy(t => t.Album)
                .ThenBy(t => t.TrackNumber)
                .ThenBy(t => t.Artist)
                .ThenBy(t => t.Title)
                .ToList();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                tracks = tracks.Where(t => t.Genre.ToLower().Contains(searchFilter)
                                           || t.AlbumArtist.ToLower().Contains(searchFilter)
                                           || t.Artist.ToLower().Contains(searchFilter)
                                           || t.Album.ToLower().Contains(searchFilter)
                                           || t.Title.ToLower().Contains(searchFilter)).ToList();
            }

            if (!string.IsNullOrEmpty(excludeCollectionFilter))
            {
                var excludeTracks =
                    new HashSet<string>(
                        CollectionHelper.GetTracksInCollection(excludeCollectionFilter)
                            .Select(t => t.Description)
                            .Distinct());
                tracks = tracks.Where(t => !excludeTracks.Contains(t.Description)).ToList();
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

            return tracks;
        }

        private static HashSet<string> GetHashSet(List<string> list)
        {
            if (list == null) list = new List<string>();
            if (list.Any(x => x == AllFilter))
                list.Clear();
            return new HashSet<string>(list.Distinct());
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
        /// <param name="updateLength">if set to <c>true</c> [update length].</param>
        /// <returns></returns>
        public Track LoadTrack(string filename, bool updateLength = true)
        {
            var track = GetTrackByFilename(filename);

            if (track == null)
                track = LoadNewTrack(filename);
            else
                TrackHelper.LoadTrack(track, updateLength);

            return track;
        }

        public void SetRank(List<Track> tracks, int rank)
        {
            TrackHelper.SetRank(tracks, rank);
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
                }
            }

            ParallelHelper.ForEach(files.TakeWhile(file => !_cancelImport), file => { LoadTrack(file); });

            LoadAllExtendedAttributes();

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

            var tracks = GetTracks(new List<string> {oldGenre}, null, null, "", "", ShufflerFilter.None, 0, 1000);
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

            var tracks = GetAllTracksForArtist(oldArtist);
            foreach (var track in tracks)
            {
                if (track.AlbumArtist == oldArtist) track.AlbumArtist = newArtist;
                if (track.Artist == oldArtist) track.Artist = newArtist;
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
            ExtenedAttributesHelper.SaveExtendedAttributes(track.Description, new Dictionary<string, string>());
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

        public void LoadAllExtendedAttributes()
        {
            foreach (var track in Tracks)
            {
                ShufflerHelper.LoadShufflerDetails(track);
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

            //if (!updateAxillaryFiles) return true;
            //try
            //{
            //    if (track.IsShufflerTrack) ShufflerHelper.RenameShufferFiles(track);

            //    // if filename changed, save any associated play-list files
            //    var playlists = PlaylistHelper.GetAllPlaylists().Where(playlist => playlist.Tracks.Contains(track));
            //    foreach (var playlist in playlists)
            //    {
            //        PlaylistHelper.SavePlaylist(playlist);
            //    }
            //}
            //catch
            //{
            //    // ignored
            //}

            return true;
        }

        /// <summary>
        /// Gets a album cover.
        /// </summary>
        /// <param name="albumName">Name of the album.</param>
        /// <param name="tracks">The tracks.</param>
        /// <returns>
        ///     The album cover
        /// </returns>
        public Image GetAlbumCover(string albumName, List<Track> tracks = null)
        {
            if (albumName == AllFilter) return null;

            var cover = AlbumCoverHelper.GetAlbumCover(albumName);
            if (cover != null) return cover;

            Console.WriteLine("GET ALBUM TRACKS");
            tracks = tracks?.Where(x => x.Album == albumName).ToList() ?? GetAllTracksForAlbum(albumName);
            
            if (tracks.Count == 0) return null;

            AlbumCoverHelper.LoadAlbumCover(tracks[0]);
            return AlbumCoverHelper.GetAlbumCover(tracks[0].Album);
        }

        public void SetTrackAlbumCover(Track track, Image image)
        {
            AlbumCoverHelper.SetTrackAlbumCover(track, image);
        }

        private Track LoadNewTrack(string filename, bool updateLength = false)
        {
            var track = TrackHelper.LoadTrack(filename, updateLength);
            if (track == null) return null;

            lock (Tracks)
            {
                Tracks.Add(track);
            }
            return track;
        }
    }
}