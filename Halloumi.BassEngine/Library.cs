using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Halloumi.BassEngine
{
    /// <summary>
    /// Represents a library of audio tracks
    /// </summary>
    public class Library
    {
        #region Fields
        
        private List<Track> _trackList = new List<Track>();
        private Dictionary<string, Track> _trackDictionary = new Dictionary<string, Track>();
        private BassPlayer _bassPlayer = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Library class.
        /// </summary>
        /// <param name="bassPlayer">The bass engine associated with the library.</param>
        public Library(BassPlayer bassPlayer)
        {
            _bassPlayer = bassPlayer;
        }

        #endregion

        #region Properties

        public BassPlayer BassPlayer
        {
            get { return _bassPlayer; }

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the library from an M3U folder.  Clears all existing tracks.
        /// </summary>
        /// <param name="filename">The folder filename.</param>
        public void LoadFromFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath)) return;
            this.Clear();
            this.AddTracks(folderPath);
        }

        /// <summary>
        /// Loads the library from an M3U playlist.  Clears all existing tracks.
        /// </summary>
        /// <param name="filename">The playlist filename.</param>
        public void LoadFromPlaylist(string filename)
        {
            if (!File.Exists(filename) && !Directory.Exists(filename)) return;
            if (Path.GetExtension(filename).ToLower() != ".m3u") return;

            this.Clear();
            this.AddTracks(filename);
        }

        /// <summary>
        /// Saves the library as an M3U playlist
        /// </summary>
        /// <param name="filename">The playlist filename.</param>
        public void SaveAsPlaylist(string filename)
        {
            PlaylistHelper.SaveAsPlaylist(filename, this.GetTracks());
        }

        /// <summary>
        /// Loads and adds a track to the library
        /// </summary>
        /// <param name="filename">The filename of the track to add.
        /// If the file is a playlist, all tracks in the playlist are added.
        /// </param>
        public void AddTracks(string path)
        {
            if (!File.Exists(path) && !Directory.Exists(path)) return;

            if (Path.GetExtension(path).ToLower() == ".m3u")
            {
                foreach (var entry in PlaylistHelper.GetPlaylistEntries(path))
                {
                    if (Path.GetExtension(entry.Path).ToLower() == ".mp3"
                        && File.Exists(entry.Path))
                    {
                        AddTrack(_bassPlayer.LoadTrack(entry.Path, entry.Artist, entry.Title));
                    }
                }
            }
            else if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path).ToList())
                {
                    if(Path.GetExtension(file).ToLower() == ".mp3") AddTracks(file);
                }  
            }
            else if (Path.GetExtension(path).ToLower() == ".mp3")
            {
                AddTrack(_bassPlayer.LoadTrack(path));
            }
        }

        /// <summary>
        /// Loads and adds a track to the library
        /// </summary>
        /// <param name="files">The files to add</param>
        public void AddTracks(List<string> files)
        {
            foreach (var file in files)
            {
                AddTracks(file);
            }
        }

        /// <summary>
        /// Adds a track to the library
        /// </summary>
        /// <param name="track">The track to add.</param>
        public void AddTrack(Track track)
        {
            if (track == null) return;
            if (!_trackDictionary.ContainsKey(track.Filename))
            {
                _trackDictionary.Add(track.Filename, track);
                _trackList.Add(track);
            }
        }

        /// <summary>
        /// Removes a track track from the library.
        /// </summary>
        /// <param name="filename">The filename of the track to remove.</param>
        public void RemoveTrack(string filename)
        {
            Track track = null;
            if (_trackDictionary.ContainsKey(filename))
            {
                track = _trackDictionary[filename];
                _trackDictionary.Remove(filename);
            }
            if (track != null && _trackList.Contains(track))
            {
                _trackList.Remove(track);
            }
        }

        /// <summary>
        /// Removes a track track from the library.
        /// </summary>
        /// <param name="track">The track.</param>
        public void RemoveTrack(Track track)
        {
            if (_trackDictionary.ContainsKey(track.Filename))
            {
                _trackDictionary.Remove(track.Filename);
            }
            if (_trackList.Contains(track))
            {
                _trackList.Remove(track);
            }
        }

        /// <summary>
        /// Gets a track from the library by filename
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The matching track</returns>
        public Track GetTrackByFilename(string filename)
        {
            if (_trackDictionary.ContainsKey(filename))
            {
                return _trackDictionary[filename];
            }
            return null;
        }

        /// <summary>
        /// Gets a track from the library by filename
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The matching track</returns>
        public Track GetTrackByDescription(string description)
        {
            return _trackList.Where(t => t.Description == description).FirstOrDefault();
        }

        /// <summary>
        /// Gets a list of all tracks in the library.
        /// </summary>
        /// <returns>A list of all tracks</returns>
        public List<Track> GetTracks()
        {
            return _trackList; //.Where(t => t.TagDataLoaded).ToList();
        }


        /// <summary>
        /// Clears the library
        /// </summary>
        public void Clear()
        {
            _trackList.Clear();
            _trackDictionary.Clear();
        }

        #endregion
    }
}
