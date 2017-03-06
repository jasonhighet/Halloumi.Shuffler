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
    public static class PlaylistHelper
    {
        private static Dictionary<string, Dictionary<string, Track>> _playlistTracks= new Dictionary<string, Dictionary<string, Track>>();
        private static List<Playlist> _playlists = new List<Playlist>();


        public static void LoadFromDatabase()
        {
            var filepath = PlaylistFile;
            if (!File.Exists(filepath))
                return;

            _playlists = SerializationHelper<List<Playlist>>.FromXmlFile(filepath);
            
            _playlistTracks = new Dictionary<string, Dictionary<string, Track>>();

            foreach (var playlist in _playlists)
            {
                var tracks = playlist
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

                _playlistTracks.Add(playlist.Name, dictionary);
            }
        }

        private static string PlaylistFile => Path.Combine(Library.ShufflerFolder, "Haloumi.Shuffler.Playlists.xml");

        public static void SaveToDatabase()
        {
            var playlists = _playlists.ToList().OrderBy(x => x.Name).ToList();
            var filepath = PlaylistFile;
            SerializationHelper<List<Playlist>>.ToXmlFile(playlists, filepath);
        }

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        public static Library Library { get; set; }

        /// <summary>
        ///     Gets all play-lists.
        /// </summary>
        /// <returns>A list of all play-lists</returns>
        public static List<string> GetAllPlaylists()
        {
            return _playlists.OrderBy(p => p.Name).Select(x=>x.Name).ToList();
        }

        /// <summary>
        /// Creates the new play-list.
        /// </summary>
        /// <param name="playlistName">Name of the play-list.</param>
        public static void CreateNewPlaylist(string playlistName)
        {
            if (!_playlistTracks.ContainsKey(playlistName))
            {
                _playlistTracks.Add(playlistName, new Dictionary<string, Track>());
            }

            var playlist = _playlists.FirstOrDefault(x => string.Equals(x.Name, playlistName, StringComparison.CurrentCultureIgnoreCase));
            if (playlist == null)
            {
                playlist = new Playlist() { Name = playlistName };
                _playlists.Add(playlist);
            }

            SavePlaylists();
        }

        /// <summary>
        /// Adds tracks to play-list.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        /// <param name="tracks">The tracks.</param>
        public static void AddTracksToPlaylist(string playlistName, List<Track> tracks)
        {
            if (tracks == null || tracks.Count == 0) return;
            if (!_playlistTracks.ContainsKey(playlistName))
            {
                _playlistTracks.Add(playlistName, new Dictionary<string, Track>());
            }
            
            var playlist = _playlists.FirstOrDefault(x => string.Equals(x.Name, playlistName, StringComparison.CurrentCultureIgnoreCase));
            if (playlist == null)
            {
                playlist = new Playlist() {Name = playlistName};
                _playlists.Add(playlist);
            }

            var playlistTracks = _playlistTracks[playlistName];
            foreach (var track in tracks)
            {
                if(!playlistTracks.ContainsKey(track.Description))
                    playlistTracks.Add(track.Description, track);

                if (!playlist.Entries.Any(x => x.Artist == track.Artist && x.Title == track.Artist))
                    playlist.Entries.Add(new PlaylistEntry() {Artist = track.Artist, Title = track.Title, Length = track.Length});
            }            

            SavePlaylists();
        }

        /// <summary>
        /// Removes tracks from a play-list.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        /// <param name="tracks">The tracks to remove.</param>
        public static void RemoveTracksFromPlaylist(string playlistName, List<Track> tracks)
        {
            RemoveTracks(playlistName, tracks);
            SavePlaylists();
        }

        private static void RemoveTracks(string playlistName, IReadOnlyCollection<Track> tracks)
        {
            if (tracks == null || tracks.Count == 0) return;
            if (!_playlistTracks.ContainsKey(playlistName))
                return;

            var playlist =
                _playlists.FirstOrDefault(x => string.Equals(x.Name, playlistName, StringComparison.CurrentCultureIgnoreCase));
            if (playlist == null)
                return;

            var playlistTracks = _playlistTracks[playlistName];
            foreach (var track in tracks)
            {
                if (playlistTracks.ContainsKey(track.Description))
                    playlistTracks.Remove(track.Description);

                playlist.Entries.RemoveAll(x => x.Artist == track.Artist && x.Title == track.Artist);
            }
        }

        /// <summary>
        ///     Removes a track from all play-lists.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void RemoveTrackFromAllPlaylists(Track track)
        {
            var playlists = GetPlaylistsForTrack(track);
            var tracks = new List<Track> {track};

            foreach (var playlist in playlists)
            {
                RemoveTracks(playlist, tracks);
            }

            SavePlaylists();
        }

        public static List<Track> GetTracksInPlaylist(string playlistName)
        {
            return !_playlistTracks.ContainsKey(playlistName) 
                ? new List<Track>() 
                : _playlistTracks[playlistName].Select(x => x.Value).ToList();
        }

        public static List<Track> GetTracksInPlaylistFile(string filename)
        {
            return AudioEngine.Helpers.PlaylistHelper.GetPlaylistEntries(filename)
                    .Select(LoadLibraryTrack)
                    .Where(x => x != null)
                    .ToList();
        }

        /// <summary>
        ///     Gets all the play-lists that contain a specific track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of play-lists that contain the track</returns>
        public static List<string> GetPlaylistsNotForTracks(List<Track> tracks)
        {
            return tracks.SelectMany(GetPlaylistsNotForTrack).Distinct().OrderBy(x => x).ToList(); ;
        }

        /// <summary>
        ///     Gets all the play-lists that contain a specific track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of play-lists that contain the track</returns>
        public static List<string> GetPlaylistsNotForTrack(Track track)
        {
            return
            (
                from playlistTracks
                in _playlistTracks
                where !playlistTracks.Value.ContainsKey(track.Description)
                select playlistTracks.Key

            )
            .OrderBy(x => x)
            .ToList();
        }

        /// <summary>
        ///     Gets all the play-lists that contain a specific track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of play-lists that contain the track</returns>
        public static List<string> GetPlaylistsForTrack(Track track)
        {
            return 
            (
                from playlistTracks 
                in _playlistTracks
                where playlistTracks.Value.ContainsKey(track.Description)
                select playlistTracks.Key

            )
            .OrderBy(x=>x)
            .ToList();
        }

        /// <summary>
        ///     Gets a distinct list of all play-lists that the specified tracks are in
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <returns>A distinct list of all play-lists that the specified tracks are in.</returns>
        public static List<string> GetPlaylistsForTracks(List<Track> tracks)
        {
            return tracks.SelectMany(GetPlaylistsForTrack).Distinct().OrderBy(x => x).ToList();
        }


        private static Track LoadLibraryTrack(AudioEngine.Helpers.PlaylistHelper.PlaylistEntry entry)
        {
            var entryTitle = entry.Title.ToLower();
            var entryArtist = entry.Artist.ToLower();

            return (IsValidLibraryTrack(entry.Path))
                ? Library.LoadTrack(entry.Path, false)
                : Library.GetTrack(entryArtist, entryTitle, 0);
        }

        private static bool IsValidLibraryTrack(string path)
        {
            return path.StartsWith(Library.LibraryFolder) && File.Exists(path);
        }


        public static void SavePlaylists()
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

            var playlistName = StringHelper.TitleCase(Path.GetFileNameWithoutExtension(filename));
            var tracks = AudioEngine.Helpers.PlaylistHelper.GetPlaylistEntries(filename)
                    .Select(LoadLibraryTrack)
                    .Where(x => x != null)
                    .OrderBy(x=>x.Description)
                    .ToList();

            if (tracks.Count <= 0) return;

            AddTracksToPlaylist(playlistName, tracks);
            SavePlaylists();
        }


        /// <summary>
        /// Saves the play-list.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        /// <param name="filename">The filename.</param>
        public static void ExportPlaylist(string playlistName, string filename)
        {
            ExportPlaylist(filename, GetTracksInPlaylist(playlistName));
        }


        /// <summary>
        ///     Saves a list of tracks as a playlist.
        /// </summary>
        /// <param name="filename">The filename of the playlist.</param>
        /// <param name="tracks">The tracks to write to the playlist.</param>
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
    }
}