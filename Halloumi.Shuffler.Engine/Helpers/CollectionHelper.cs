using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.AudioLibrary.Helpers
{
    public static class CollectionHelper
    {
        private static Dictionary<string, Dictionary<string, Track>> _collectionTracks= new Dictionary<string, Dictionary<string, Track>>();
        private static List<Playlist> _collections = new List<Playlist>();


        public static void LoadFromDatabase()
        {
            var filepath = CollectionFile;
            if (!File.Exists(filepath))
                return;

            _collections = SerializationHelper<List<Playlist>>.FromXmlFile(filepath);
            
            _collectionTracks = new Dictionary<string, Dictionary<string, Track>>();

            foreach (var collection in _collections)
            {
                var tracks = collection
                    .Entries
                    .Select(entry => Library.GetTrack(entry.Artist, entry.Title, entry.Length))
                    .Where(track => track != null)
                    .OrderBy(x=>x.Description)
                    .ToList();

                var dictionary = new Dictionary<string, Track>();
                foreach (var track in tracks)
                {
                    if(!dictionary.ContainsKey(track.Description))
                        dictionary.Add(track.Description, track);
                }

                _collectionTracks.Add(collection.Name, dictionary);
            }
        }

        private static string CollectionFile
        {
            get { return Path.Combine(Library.ShufflerFolder, "Haloumi.Shuffler.Collections.xml"); }
        }

        public static void SaveToDatabase()
        {
            var collections = _collections.ToList().OrderBy(x => x.Name).ToList();
            var filepath = CollectionFile;
            SerializationHelper<List<Playlist>>.ToXmlFile(collections, filepath);
        }

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        public static Library Library { get; set; }

        /// <summary>
        ///     Gets all play-lists.
        /// </summary>
        /// <returns>A list of all play-lists</returns>
        public static List<string> GetAllCollections()
        {
            return _collections.OrderBy(p => p.Name).Select(x=>x.Name).ToList();
        }

        /// <summary>
        /// Creates the new play-list.
        /// </summary>
        /// <param name="collectionName">Name of the play-list.</param>
        public static void CreateNewCollection(string collectionName)
        {
            if (!_collectionTracks.ContainsKey(collectionName))
            {
                _collectionTracks.Add(collectionName, new Dictionary<string, Track>());
            }

            var collection = _collections.FirstOrDefault(x => string.Equals(x.Name, collectionName, StringComparison.CurrentCultureIgnoreCase));
            if (collection == null)
            {
                collection = new Playlist() { Name = collectionName };
                _collections.Add(collection);
            }

            SaveCollections();
        }

        /// <summary>
        /// Adds tracks to play-list.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="tracks">The tracks.</param>
        public static void AddTracksToCollection(string collectionName, List<Track> tracks)
        {
            if (tracks == null || tracks.Count == 0) return;
            if (!_collectionTracks.ContainsKey(collectionName))
            {
                _collectionTracks.Add(collectionName, new Dictionary<string, Track>());
            }
            
            var collection = _collections.FirstOrDefault(x => string.Equals(x.Name, collectionName, StringComparison.CurrentCultureIgnoreCase));
            if (collection == null)
            {
                collection = new Playlist() {Name = collectionName};
                _collections.Add(collection);
            }

            var collectionTracks = _collectionTracks[collectionName];
            foreach (var track in tracks)
            {
                if(!collectionTracks.ContainsKey(track.Description))
                    collectionTracks.Add(track.Description, track);

                if (!collection.Entries.Any(x => x.Artist == track.Artist && x.Title == track.Artist))
                    collection.Entries.Add(new PlaylistEntry() {Artist = track.Artist, Title = track.Title, Length = track.Length});
            }            

            SaveCollections();
        }

        /// <summary>
        /// Removes tracks from a play-list.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="tracks">The tracks to remove.</param>
        public static void RemoveTracksFromCollection(string collectionName, List<Track> tracks)
        {
            RemoveTracks(collectionName, tracks);
            SaveCollections();
        }

        private static void RemoveTracks(string collectionName, IReadOnlyCollection<Track> tracks)
        {
            if (tracks == null || tracks.Count == 0) return;
            if (!_collectionTracks.ContainsKey(collectionName))
                return;

            var collection =
                _collections.FirstOrDefault(x => string.Equals(x.Name, collectionName, StringComparison.CurrentCultureIgnoreCase));
            if (collection == null)
                return;

            var collectionTracks = _collectionTracks[collectionName];
            foreach (var track in tracks)
            {
                if (collectionTracks.ContainsKey(track.Description))
                    collectionTracks.Remove(track.Description);

                collection.Entries.RemoveAll(x => x.Artist == track.Artist && x.Title == track.Title);
            }
        }

        /// <summary>
        ///     Removes a track from all play-lists.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void RemoveTrackFromAllCollections(Track track)
        {
            var collections = GetCollectionsForTrack(track);
            var tracks = new List<Track> {track};

            foreach (var collection in collections)
            {
                RemoveTracks(collection, tracks);
            }

            SaveCollections();
        }

        public static List<Track> GetTracksInCollection(string collectionName)
        {
            return !_collectionTracks.ContainsKey(collectionName) 
                ? new List<Track>() 
                : _collectionTracks[collectionName].Select(x => x.Value).ToList();
        }


        /// <summary>
        /// Gets all the play-lists that contain a specific track.
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <returns>
        /// A list of play-lists that contain the track
        /// </returns>
        public static List<string> GetCollectionsTracksArentIn(List<Track> tracks)
        {
            return tracks.SelectMany(GetCollectionsTrackIsntIn).Distinct().OrderBy(x => x).ToList(); 
        }

        /// <summary>
        ///     Gets all the play-lists that contain a specific track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of play-lists that contain the track</returns>
        public static List<string> GetCollectionsTrackIsntIn(Track track)
        {
            if (track == null) return new List<string>();
            return
            (
                from collectionTracks
                in _collectionTracks
                where !collectionTracks.Value.ContainsKey(track.Description)
                select collectionTracks.Key

            )
            .OrderBy(x => x)
            .ToList();
        }

        /// <summary>
        ///     Gets all the play-lists that contain a specific track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of play-lists that contain the track</returns>
        public static List<string> GetCollectionsForTrack(Track track)
        {
            if (track == null)
                return new List<string>();

            return 
            (
                from collectionTracks 
                in _collectionTracks
                where collectionTracks.Value.ContainsKey(track.Description)
                select collectionTracks.Key

            )
            .OrderBy(x=>x)
            .ToList();
        }

        /// <summary>
        ///     Gets a distinct list of all play-lists that the specified tracks are in
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <returns>A distinct list of all play-lists that the specified tracks are in.</returns>
        public static List<string> GetCollectionsForTracks(List<Track> tracks)
        {
            return tracks.SelectMany(GetCollectionsForTrack).Distinct().OrderBy(x => x).ToList();
        }


        private static Track LoadLibraryTrack(AudioEngine.Helpers.PlaylistHelper.PlaylistEntry entry)
        {
            var entryTitle = entry.Title.ToLower();
            var entryArtist = entry.Artist.ToLower();

            if (entryArtist == "various" && entryTitle.Contains("/"))
            {
                entryArtist = entryTitle.Split('/')[0].Trim();
                entryTitle = entryTitle.Split('/')[1].Trim();
            }

            return (IsValidLibraryTrack(entry.Path))
                ? Library.LoadTrack(entry.Path, false)
                : Library.GetTrack(entryArtist, entryTitle);
        }

        private static bool IsValidLibraryTrack(string path)
        {
            return path.StartsWith(Library.LibraryFolder) && File.Exists(path);
        }


        public static void SaveCollections()
        {
            Task.Run(() => SaveToDatabase());
        }

        /// <summary>
        /// Loads a play-list.
        /// </summary>
        /// <param name="filename">The play-list file.</param>
        public static void ImportPlaylist(string filename)
        {
            if(!File.Exists(filename))
                return;

            var collectionName = StringHelper.TitleCase(Path.GetFileNameWithoutExtension(filename));
            var tracks = AudioEngine.Helpers.PlaylistHelper.GetPlaylistEntries(filename)
                    .Select(LoadLibraryTrack)
                    .Where(x => x != null)
                    .OrderBy(x=>x.Description)
                    .ToList();

            if (tracks.Count <= 0) return;

            AddTracksToCollection(collectionName, tracks);
            SaveCollections();
        }


        /// <summary>
        /// Saves the play-list.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="filename">The filename.</param>
        public static void ExportPlaylist(string collectionName, string filename)
        {
            ExportPlaylist(filename, GetTracksInCollection(collectionName));
        }


        /// <summary>
        ///     Saves a list of tracks as a collection.
        /// </summary>
        /// <param name="filename">The filename of the collection.</param>
        /// <param name="tracks">The tracks to write to the collection.</param>
        public static void ExportPlaylist(string filename, List<Track> tracks)
        {
            var content = new StringBuilder();
            content.AppendLine("#EXTM3U");
            foreach (var track in tracks)
            {
                content.AppendLine("#EXTINF:"
                                   + track.Length.ToString("0")
                                   + ","
                                   + track.Artist
                                   + " - "
                                   + track.Title);

                content.AppendLine(track.Filename);
            }

            File.WriteAllText(filename, content.ToString(), Encoding.UTF8);
        }

        public static List<Track> GetTracksInPlaylistFile(string filename)
        {
            return AudioEngine.Helpers.PlaylistHelper.GetPlaylistEntries(filename)
                    .Select(LoadLibraryTrack)
                    .Where(x => x != null)
                    .ToList();
        }

        public static void DeleteCollection(string collectionFilter)
        {
            if(_collectionTracks.ContainsKey(collectionFilter))
                _collectionTracks.Remove(collectionFilter);

            _collections.RemoveAll(x => x.Name == collectionFilter);

            SaveCollections();
        }
    }
}