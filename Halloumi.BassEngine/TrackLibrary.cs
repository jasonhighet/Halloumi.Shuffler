using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Halloumi.BassEngine
{
    public class TrackLibrary
    {
        private List<Track> _trackList = new List<Track>();
        private Dictionary<string, Track> _trackDictionary = new Dictionary<string, Track>();
        private BassPlayer _bassPlayer = null;

        public TrackLibrary(BassPlayer bassPlayer)
        {
            _bassPlayer = bassPlayer;
        }

        public void Load(string filename)
        {
            if (!File.Exists(filename)) return;
            if (Path.GetExtension(filename).ToLower() != "m3u") return;

            this.Clear();
            this.Load(filename);
        }

        public void Save(string filename)
        {
            BassHelper.SaveAsPlaylist(filename, this.GetTracks());
        }


        public void AddTrack(string filename)
        {
            if (!File.Exists(filename)) return;

            if (Path.GetExtension(filename).ToLower() == "m3u")
            {
                foreach (var file in BassHelper.GetFilesInPlaylist(filename))
                {
                    AddTrack(file);
                }
            }
            else if (Path.GetExtension(filename).ToLower() == "mp3")
            {
                AddTrack(_bassPlayer.LoadTrack(filename));
            }
        }

        public void AddTrack(Track track)
        {
            if (!_trackDictionary.ContainsKey(track.Filename))
            {
                _trackDictionary.Add(track.Filename, track);
                _trackList.Add(track);
            }
        }

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

        public Track GetTrack(string filename)
        {
            if (_trackDictionary.ContainsKey(filename))
            {
                return _trackDictionary[filename];
            }
            return null;
        }

        public List<Track> GetTracks()
        {
            return _trackList;
        }

        /// <summary>
        /// Gets all tracks in a BPM range.
        /// </summary>
        /// <param name="bpm">The BPM.</param>
        /// <param name="percentVariance">The percent variance as a value from 0 - 100.
        /// (eg 5 returns all tracks with a Start BPM +/- 5% of the specified BPM .</param>
        /// <returns></returns>
        public List<Track> GetTracksInBPMRange(decimal bpm, decimal percentVariance)
        {
            percentVariance = Math.Abs(percentVariance);

            var minBPM = bpm * (1 - (percentVariance / 100));
            var maxBPM = bpm * (1 + (percentVariance / 100));

            return _trackList
                .Where(t => t.StartBPM >= minBPM && t.StartBPM <= maxBPM)
                .ToList();
        }

        /// <summary>
        /// Gets all tracks matching a BPM range.
        /// </summary>
        /// <param name="bpm">The BPM.</param>
        /// <param name="percentVariance">
        /// The percent variance as a value from 0 - 100.
        /// (eg 5 returns all tracks with a Start BPM +/- 5% of the specified BPM .
        /// </param>
        /// <param name="excludeTracks">Exclude these tracks.</param>
        /// <returns>A list if matching tracks</returns>
        public List<Track> GetTracksInBPMRange(decimal bpm, decimal percentVariance, List<Track> excludeTracks)
        {
            return GetTracksInBPMRange(bpm, percentVariance).Except(excludeTracks).ToList();
        }

        public List<string> GetTrackFilenames()
        {
            var trackFilenames = new List<string>();
            foreach (var track in _trackList)
            {
                trackFilenames.Add(track.Filename);
            }
            return trackFilenames;
        }

        public void Clear()
        {
            _trackList.Clear();
            _trackDictionary.Clear();
        }
    }
}
