using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.AudioLibrary.Helpers
{
    public static class PlaylistHelper
    {
        static PlaylistHelper()
        {
            Playlists = new List<Playlist>();
        }

        /// <summary>
        ///     Gets or sets the folder where the m3u play-list files for the library are kept
        /// </summary>
        public static string PlaylistFolder { get; set; }

        /// <summary>
        ///     Gets or sets the play-lists in the library
        /// </summary>
        private static List<Playlist> Playlists { get; set; }

        /// <summary>
        ///     Gets all play-lists.
        /// </summary>
        /// <returns>A list of all play-lists</returns>
        public static List<Playlist> GetAllPlaylists()
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
        public static Playlist GetPlaylistByName(string name)
        {
            name = name.ToLower();
            return Playlists.FirstOrDefault(p => p.Name.ToLower() == name);
        }

        /// <summary>
        ///     Loads all the play-lists in the play-list folder
        /// </summary>
        public static void LoadPlaylists(Library libary)
        {
            var playlistFiles = FileSystemHelper.SearchFiles(PlaylistFolder, "*.m3u", false);

            Playlists = playlistFiles.Select(playlistFile => LoadPlaylist(playlistFile, libary)).ToList();
        }

        /// <summary>
        ///     Adds tracks to play-list.
        /// </summary>
        /// <param name="playlist">The play-list.</param>
        /// <param name="tracks">The tracks.</param>
        public static void AddTracksToPlaylist(Playlist playlist, List<Track> tracks)
        {
            if (tracks == null || playlist == null) return;

            foreach (var track in tracks.Where(track => !playlist.Tracks.Contains(track)))
            {
                playlist.Tracks.Add(track);
            }
            SavePlaylist(playlist);
        }

        /// <summary>
        ///     Removes tracks from a play-list.
        /// </summary>
        /// <param name="playlist">The play-list.</param>
        /// <param name="tracks">The tracks to remove.</param>
        public static void RemoveTracksFromPlaylist(Playlist playlist, List<Track> tracks)
        {
            if (tracks == null || playlist == null) return;

            foreach (var track in tracks.Where(track => playlist.Tracks.Contains(track)))
            {
                playlist.Tracks.Remove(track);
            }
            SavePlaylist(playlist);
        }

        /// <summary>
        ///     Removes a track from all play-lists.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void RemoveTrackFromAllPlaylists(Track track)
        {
            foreach (var playlist in Playlists.Where(playlist => playlist.Tracks.Contains(track)))
            {
                playlist.Tracks.Remove(track);
                SavePlaylist(playlist);
            }
        }

        /// <summary>
        ///     Gets all the play-lists that contain a specific track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of play-lists that contain the track</returns>
        public static List<Playlist> GetPlaylistsForTrack(Track track)
        {
            return GetAllPlaylists().Where(p => p.Tracks.Contains(track)).Distinct().ToList();
        }

        /// <summary>
        ///     Gets a distinct list of all play-lists that the specified tracks are in
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <returns>A distinct list of all play-lists that the specified tracks are in.</returns>
        public static List<Playlist> GetPlaylistsForTracks(List<Track> tracks)
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
        public static Playlist CreateNewPlaylist(string playlistName)
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
        /// Loads a play-list.
        /// </summary>
        /// <param name="playlistFile">The play-list file.</param>
        /// <param name="library">The library.</param>
        /// <returns>
        /// A play-list object
        /// </returns>
        public static Playlist LoadPlaylist(string playlistFile, Library library)
        {
            var playlist = new Playlist
            {
                Filename = playlistFile,
                Name = Path.GetFileNameWithoutExtension(playlistFile)
            };
            playlist.Name = StringHelper.TitleCase(playlist.Name);

            var tracks = from entry 
                         in AudioEngine.Helpers.PlaylistHelper.GetPlaylistEntries(playlistFile)
                         let entryTitle = entry.Title.ToLower()
                         let entryArtist = entry.Artist.ToLower()
                         select library.LoadTrack(entry.Path) ?? library.GetTrack(entryArtist, entryTitle, 0) 
                         into track
                         where track != null
                         select track;

            playlist.Tracks.AddRange(tracks);

            return playlist;
        }

        /// <summary>
        ///     Saves the play-list.
        /// </summary>
        /// <param name="playlist">The play-list.</param>
        public static void SavePlaylist(Playlist playlist)
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
    }
}