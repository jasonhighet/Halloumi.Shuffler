using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    public static class PlaylistHelper
    {
        /// <summary>
        ///     Gets a list of all files in an M3U playlist
        /// </summary>
        /// <param name="playlistFile">The playlist file.</param>
        /// <returns>A list of file names contained in the playlist</returns>
        public static List<PlaylistEntry> GetPlaylistEntries(string playlistFile)
        {
            var playlistEntries = new List<PlaylistEntry>();
            using (var reader = new StreamReader(playlistFile, Encoding.UTF7))
            {
                string currentLine;
                while ((currentLine = reader.ReadLine()) != null)
                {
                    if (currentLine.Length <= 0 || currentLine == "#EXTM3U") continue;

                    var playlistEntry = new PlaylistEntry();
                    if (currentLine.StartsWith("#"))
                    {
                        var elements = currentLine.Split(',').ToList();
                        if (elements.Count > 0)
                        {
                            elements = currentLine.Substring(currentLine.IndexOf(',') + 1).Split('-').ToList();
                            if (elements.Count == 2)
                            {
                                playlistEntry.Artist = elements[0].Trim();
                                playlistEntry.Title = elements[1].Trim();
                            }
                            else
                            {
                                playlistEntry.Title = string.Join("-", elements.ToArray());
                                playlistEntry.Artist = "";
                            }
                        }
                        currentLine = reader.ReadLine();
                    }

                    if(currentLine == null) continue;
                    
                    string path;
                    if (currentLine.StartsWith(@"\"))
                    {
                        path = Path.GetPathRoot(playlistFile) + currentLine;
                        playlistEntry.Path = path;
                        playlistEntry.Path = playlistEntry.Path.Replace(@"\\", @"\");
                    }
                    else if (currentLine.Contains(":"))
                    {
                        path = currentLine;
                    }
                    else
                    {
                        path = Path.Combine(Path.GetDirectoryName(playlistFile) + "", currentLine);
                    }

                    var trackDetails = TrackHelper.GuessTrackDetailsFromFilename(path.Trim());
                    if (playlistEntry.Title == "") playlistEntry.Title = trackDetails.Title;
                    if (playlistEntry.Artist == "") playlistEntry.Artist = trackDetails.Artist;
                    playlistEntry.Description = trackDetails.Description;

                    playlistEntry.Path = path.Trim();

                    if (playlistEntry.Path != "")
                    {
                        playlistEntries.Add(playlistEntry);
                    }
                }
                reader.Close();
            }
            return playlistEntries;
        }

        /// <summary>
        ///     Gets a list of all files in an M3U playlist
        /// </summary>
        /// <param name="playlistFile">The playlist file.</param>
        /// <returns>A list of file names contained in the playlist</returns>
        public static List<string> GetFilesInPlaylist(string playlistFile)
        {
            return (from playlistEntry 
                    in GetPlaylistEntries(playlistFile)
                    where playlistEntry.Path != ""
                    select playlistEntry.Path)
                    .ToList();
        }

        /// <summary>
        ///     Saves a list of tracks as a playlist.
        /// </summary>
        /// <param name="filename">The filename of the playlist.</param>
        /// <param name="tracks">The tracks to write to the playlist.</param>
        public static void SaveAsPlaylist(string filename, List<Track> tracks)
        {
            var content = new StringBuilder();
            content.AppendLine("#EXTM3U");
            foreach (var track in tracks)
            {
                content.AppendLine("#EXTINF:"
                                   + track.LengthSeconds.ToString("0")
                                   + ","
                                   + track.Artist
                                   + " - "
                                   + track.Title);

                content.AppendLine(track.Filename);
            }

            File.WriteAllText(filename, content.ToString(), Encoding.UTF8);
        }

        /// <summary>
        ///     Gets the shuffler attributes.
        /// </summary>
        /// <param name="extendedAttributeFile">The shuffler attributes file</param>
        /// <returns>
        ///     A collection of shuffler attributes
        /// </returns>
        public static Dictionary<string, string> GetShufflerAttributes(string extendedAttributeFile)
        {
            var attributes = new Dictionary<string, string>();
            if (!File.Exists(extendedAttributeFile)) return attributes;

            var elements = File.ReadAllText(extendedAttributeFile)
                .Split(';')
                .Select(element => element.Split('=').ToList())
                .Where(items => items.Count > 1 && !attributes.ContainsKey(items[0].Trim()));

            foreach (var element in elements)
            {
                attributes.Add(element[0].Trim(), element[1].Trim());
            }
            return attributes;
        }

        /// <summary>
        ///     Represents an entry in a m3u playlist
        /// </summary>
        public class PlaylistEntry
        {
            public PlaylistEntry()
            {
                Path = "";
                Artist = "";
                Title = "";
            }

            public string Path { get; set; }

            public string Artist { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }
        }
    }
}