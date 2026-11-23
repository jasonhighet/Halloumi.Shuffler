using System;
using System.Collections.Generic;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     Every track BassPlayer has loaded, looked up by filename, plus a short list of recently
    ///     loaded tracks whose audio is kept in memory even when they are not in a slot.
    /// </summary>
    internal class TrackCache
    {
        private const int RecentTrackCount = 5;

        private readonly object _lock = new object();
        private readonly List<Track> _tracks = new List<Track>();
        private List<Track> _recentTracks = new List<Track>();
        private int _nextTrackId;

        /// <summary>
        ///     Finds a loaded track by filename (case-insensitive).
        /// </summary>
        public Track Find(string filename)
        {
            lock (_lock)
            {
                return _tracks.FirstOrDefault(t =>
                    string.Equals(t.Filename, filename, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        public bool Contains(string filename)
        {
            return Find(filename) != null;
        }

        /// <summary>
        ///     Makes a new track with the next track id. It is not added to the cache.
        /// </summary>
        public Track CreateTrack(string filename)
        {
            lock (_lock)
            {
                return new Track
                {
                    Id = _nextTrackId++,
                    Filename = filename
                };
            }
        }

        public void Add(Track track)
        {
            lock (_lock)
            {
                _tracks.Add(track);
            }
        }

        public void Remove(Track track)
        {
            lock (_lock)
            {
                _tracks.Remove(track);
            }
        }

        /// <summary>
        ///     Takes every track out of the cache.
        /// </summary>
        /// <returns>The tracks that were in it.</returns>
        public List<Track> RemoveAll()
        {
            lock (_lock)
            {
                var tracks = _tracks.ToList();
                _tracks.Clear();
                return tracks;
            }
        }

        /// <summary>
        ///     Marks a track as recently loaded. The last few are kept in memory.
        /// </summary>
        public void MarkRecent(Track track)
        {
            if (track == null) return;

            lock (_lock)
            {
                _recentTracks.RemoveAll(rt => rt.Description == track.Description);
                _recentTracks.Insert(0, track);
                if (_recentTracks.Count > RecentTrackCount)
                    _recentTracks = _recentTracks.Take(RecentTrackCount).ToList();
            }
        }

        /// <summary>
        ///     Gets the tracks whose audio can be unloaded: loaded, not in use, and not recently loaded.
        /// </summary>
        public List<Track> GetUnloadable(Func<Track, bool> isInUse)
        {
            lock (_lock)
            {
                return _tracks
                    .Where(t => t.IsAudioLoaded() && !isInUse(t))
                    .Where(t => !_recentTracks.Exists(rt => rt.Description == t.Description))
                    .ToList();
            }
        }
    }
}
