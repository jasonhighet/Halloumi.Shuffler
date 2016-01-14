using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Halloumi.BassEngine
{
    public static class PlaylistHelper
    {
        /// <summary>
        /// Gets a list of all files in an M3U playlist
        /// </summary>
        /// <param name="playlistFile">The playlist file.</param>
        /// <returns>A list of file names contained in the playlist</returns>
        public static List<PlaylistEntry> GetPlaylistEntries(string playlistFile)
        {
            var playlistEntries = new List<PlaylistEntry>();
            using (var reader = new StreamReader(playlistFile, Encoding.UTF7))
            {
                var currentLine = string.Empty;
                while ((currentLine = reader.ReadLine()) != null)
                {
                    if (currentLine.Length > 0 && currentLine != "#EXTM3U")
                    {
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

                        var path = "";
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
                            path = Path.Combine(Path.GetDirectoryName(playlistFile), currentLine);
                        }

                        var trackDetails = BassHelper.GuessTrackDetailsFromFilename(path.Trim());
                        if (playlistEntry.Title == "") playlistEntry.Title = trackDetails.Title;
                        if (playlistEntry.Artist == "") playlistEntry.Artist = trackDetails.Artist;
                        playlistEntry.Description = trackDetails.Description;

                        playlistEntry.Path = path.Trim();

                        if (playlistEntry.Path != "")
                        {
                            playlistEntries.Add(playlistEntry);
                        }
                    }
                }
                reader.Close();
            }
            return playlistEntries;
        }

        /// <summary>
        /// Represents an entry in a m3u playlist
        /// </summary>
        public class PlaylistEntry
        {
            public string Path { get; set; }

            public string Artist { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }

            public PlaylistEntry()
            {
                this.Path = "";
                this.Artist = "";
                this.Title = "";
            }
        }

        /// <summary>
        /// Gets a list of all files in an M3U playlist
        /// </summary>
        /// <param name="playlistFile">The playlist file.</param>
        /// <returns>A list of file names contained in the playlist</returns>
        public static List<string> GetFilesInPlaylist(string playlistFile)
        {
            var files = new List<string>();
            foreach (var playlistEntry in GetPlaylistEntries(playlistFile))
            {
                if (playlistEntry.Path != "") files.Add(playlistEntry.Path);
            }
            return files;
        }

        /// <summary>
        /// Saves a list of tracks as a playlist.
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
        /// Gets the shuffler attributes.
        /// </summary>
        /// <param name="extendedAttributeFile">The shuffler attributes file</param>
        /// <returns>
        /// A collection of shuffler attributes
        /// </returns>
        public static Dictionary<string, string> GetShufflerAttributes(string extendedAttributeFile)
        {
            var attributes = new Dictionary<string, string>();
            if (File.Exists(extendedAttributeFile))
            {
                foreach (var element in File.ReadAllText(extendedAttributeFile).Split(';').ToList())
                {
                    var items = element.Split('=').ToList();
                    if (items.Count > 1 && !attributes.ContainsKey(items[0].Trim()))
                    {
                        attributes.Add(items[0].Trim(), items[1].Trim());
                    }
                }
            }
            return attributes;
        }
    }
}