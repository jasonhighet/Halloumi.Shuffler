using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Halloumi.Common.Helpers;

namespace Halloumi.BassEngine
{
    /// <summary>
    /// A library of preferred/compulsory/forbidden track segues.
    /// </summary>
    public class MixLibrary
    {
        #region Fields

        /// <summary>
        /// A collection of all track mixes
        /// </summary>
        private Dictionary<string, MixTracks> _mixes = new Dictionary<string, MixTracks>();

        /// <summary>
        /// The library of tracks
        /// </summary>
        private Library _library = null;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the folder where mix detail files are stored
        /// </summary>
        public string MixDetailsFolder { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MixLibrary class.
        /// </summary>
        /// <param name="library">The track library.</param>
        /// <param name="mixDetailsFolder">The mix details folder.</param>
        public MixLibrary(Library library, string mixDetailsFolder)
        {
            _library = library;
            this.MixDetailsFolder = mixDetailsFolder;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears all loaded mix details
        /// </summary>
        public void Clear()
        {
            _mixes.Clear();
        }

        /// <summary>
        /// Gets all preferred tracks for the specified track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of tracks in the library the specified track should prefer to mix with</returns>
        public List<Track> GetMixableTracks(Track track)
        {
            var tracks = new List<Track>();
            tracks.AddRange(this.GetMixableTracks(track, 5));
            tracks.AddRange(this.GetMixableTracks(track, 4));
            tracks.AddRange(this.GetMixableTracks(track, 3));
            tracks.AddRange(this.GetMixableTracks(track, 2));
            tracks.AddRange(this.GetMixableTracks(track, 1));
            return tracks;        
        }


        /// <summary>
        /// Gets all preferred tracks for the specified track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of tracks in the library the specified track should prefer to mix with</returns>
        public List<Track> GetMixableTracks(Track track, int mixLevel)
        {
            var tracks = new List<Track>();
            if (track == null) return tracks;

            if (mixLevel == 1)
            {
                var excludeTracks = new List<Track>();
                excludeTracks.Add(track);
                excludeTracks.AddRange(GetMixableTracks(track, 0));
                excludeTracks.AddRange(GetMixableTracks(track, 2));
                excludeTracks.AddRange(GetMixableTracks(track, 3));
                excludeTracks.AddRange(GetMixableTracks(track, 4));
                excludeTracks.AddRange(GetMixableTracks(track, 5));

                var tracksInRange = BassHelper.GetTracksInBPMRange(track.EndBPM, 5M, _library.GetTracks());
                return tracksInRange
                    .Distinct()
                    .Except(excludeTracks)
                    .ToList();
            }
            else
            {
                foreach (var toTrack in GetMixTracks(track).GetToTracks(mixLevel))
                {
                    var mixableTrack = _library.GetTracks()
                        .Where(t => t.Description == toTrack.TrackDescription)
                        .FirstOrDefault();
                    if (mixableTrack != null) tracks.Add(mixableTrack);
                }
            }

            return tracks;
        }

        /// <summary>
        /// Flags track 2 as a mix track for track 1
        /// </summary>
        /// <param name="track1">Track1.</param>
        /// <param name="track2">Track2.</param>
        public void SetMixLevel(Track track1, Track track2, int mixLevel)
        {
            if (track1 == null || track2 == null) return;

            var mixTracks = GetMixTracks(track1);
            mixTracks.AddToTrack(track2.Description, mixLevel);
            mixTracks.Save(GetMixTracksFileName(track1));
        }


        /// <summary>
        /// Gets the mix level for mixing track 1 into track 1
        /// </summary>
        /// <param name="track1">The track 1.</param>
        /// <param name="track2">The track 2.</param>
        /// <returns>A mix level from 0 to 5</returns>
        public int GetMixLevel(Track track1, Track track2)
        {
            if (track1 == null || track2 == null) return 0;

            var mixTracks = GetMixTracks(track1);
            var mixTrack = mixTracks.GetToTracks()
                .Where(mt => mt.TrackDescription == track2.Description)
                .FirstOrDefault();

            if (mixTrack != null) return mixTrack.MixLevel;

            if (BassHelper.AbsoluteBPMPercentChange(track1.EndBPM, track2.StartBPM) > 5M)
            {
                return 0;
            }
            return 1;
        }


        /// <summary>
        /// Gets the preferred tracks to mix with the supplied track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>The preferred tracks to mix with the supplied track</returns>
        public List<Track> GetPreferredTracks(Track track)
        {
            var tracks = new List<Track>();

            tracks.AddRange(this.GetMixableTracks(track, 5));
            tracks.AddRange(this.GetMixableTracks(track, 4));
            tracks.AddRange(this.GetMixableTracks(track, 3));
            tracks.AddRange(this.GetMixableTracks(track, 2));
            return tracks;
        }

        /// <summary>
        /// Gets the forbidden tracks for a particular track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>The tracks forbidden to mix with the supplied track</returns>
        public List<Track> GetForbiddenTracks(Track track)
        {
            return this.GetMixableTracks(track, 0);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the preferred tracks object for the specifed track.
        /// Loads from file if not in collection.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>The associated preferred mix tracks object</returns>
        private MixTracks GetMixTracks(Track track)
        {
            if (!_mixes.ContainsKey(track.Description))
            {
                lock (_mixes)
                {
                    var mixTracks = new MixTracks(track.Description);
                    var filename = GetMixTracksFileName(track);
                    mixTracks.Load(filename);
                    try
                    { 
                        _mixes.Add(track.Description, mixTracks);
                    }
                    catch { }
                    
                }
            }
            return _mixes[track.Description];
        }


        /// <summary>
        /// Gets the name of the file used to store the details of a mixtracks object for a specific track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="description">The mixtracks description.</param>
        /// <returns>A filename, including the full path</returns>
        private string GetMixTracksFileName(Track track)
        {
            string filename = track.Description
                + ".Mixes.txt";
            filename = FileSystemHelper.StripInvalidFileNameChars(filename);

            return Path.Combine(this.MixDetailsFolder, filename);
        }

        #endregion

        #region Private Classes

        /// <summary>
        /// Represents a collection of tracks that mix into a specific track
        /// </summary>
        private class MixTracks
        {
            #region Fields

            /// <summary>
            /// The track description of the from track
            /// </summary>
            private string _fromTrackDescription = null;

            /// <summary>
            /// A collection of all to tracks for the from track
            /// </summary>
            private Dictionary<string, MixTrack> _toTracks = new Dictionary<string, MixTrack>();

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the MixTracks class.
            /// </summary>
            /// <param name="trackDescription">The from track description.</param>
            public MixTracks(string trackDescription)
            {
                _fromTrackDescription = trackDescription;
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Adds a mix into track.
            /// </summary>
            /// <param name="trackDescription">The track description.</param>
            public void AddToTrack(string trackDescription, int mixLevel)
            {
                trackDescription = trackDescription.Trim();
                if (trackDescription == "") return;
                if (trackDescription == _fromTrackDescription) return;

                var mixTrack = new MixTrack()
                {
                    MixLevel = mixLevel,
                    TrackDescription = trackDescription.Trim()
                };
                AddToTrack(mixTrack);
            }

            /// <summary>
            /// Adds a mix into track.
            /// </summary>
            /// <param name="trackDescription">The track description.</param>
            private void AddToTrack(MixTrack mixTrack)
            {
                if (mixTrack == null) return;
                var trackDescription = mixTrack.TrackDescription;
                if (trackDescription == _fromTrackDescription) return;

                if (mixTrack.MixLevel == 1)
                {
                    if (_toTracks.ContainsKey(trackDescription)) _toTracks.Remove(trackDescription);
                }
                else if (!_toTracks.ContainsKey(trackDescription))
                {
                    _toTracks.Add(trackDescription, mixTrack);
                }
                else
                {
                    _toTracks[trackDescription].MixLevel = mixTrack.MixLevel;
                }
            }



            /// <summary>
            /// Gets a list of all to track descriptions.
            /// </summary>
            /// <returns>A list of all to track descriptions.</returns>
            public List<MixTrack> GetToTracks()
            {
                return
                (
                    from toTrack
                    in _toTracks
                    orderby toTrack.Value.MixLevel, toTrack.Value.TrackDescription
                    select toTrack.Value
                ).ToList();
            }

            /// <summary>
            /// Gets a list of all to track descriptions.
            /// </summary>
            /// <returns>A list of all to track descriptions.</returns>
            public List<MixTrack> GetToTracks(int level)
            {
                return
                (
                    from toTrack
                    in _toTracks
                    where toTrack.Value.MixLevel == level
                    orderby toTrack.Value.TrackDescription
                    select toTrack.Value
                ).ToList();
            }

            /// <summary>
            /// Saves mixtracks object the specified filename.
            /// </summary>
            /// <param name="filename">The filename.</param>
            public void Save(string filename)
            {
                StringBuilder content = new StringBuilder();
                foreach (var toTrack in this.GetToTracks())
                {
                    content.AppendLine(toTrack.ToString());
                }

                var blank = (content.Length == 0  || content.ToString().Trim().Replace(Environment.NewLine, "") == "");

                if (blank && File.Exists(filename))
                {
                    File.Delete(filename);
                }
                else if(!blank)
                {
                    File.WriteAllText(filename, content.ToString(), Encoding.Unicode);
                }
            }

            /// <summary>
            /// Populates this mixtracks object from the specified filename.
            /// </summary>
            /// <param name="filename">The filename.</param>
            public void Load(string filename)
            {
                if (!File.Exists(filename)) return;

                var content = File.ReadAllLines(filename).ToList();
                _toTracks.Clear();
                foreach (var line in content)
                {
                    AddToTrack(MixTrack.FromString(line));
                }
            }

            #endregion
        }

        private class MixTrack
        {
            public string TrackDescription { get; set; }
            public int MixLevel { get; set; }
            public override string ToString()
            {
                return this.TrackDescription + ", " + MixLevel.ToString();
            }

            public static MixTrack FromString(string value)
            {
                if (value == "") return null;
                var mixTrack = new MixTrack();

                if (value.Contains(","))
                {
                    var valueArray = value.Trim().Split(',');

                    int mixLevel = 1;
                    if(int.TryParse(valueArray[valueArray.Length - 1], out mixLevel))
                    {
                        mixTrack.MixLevel = mixLevel;
                        var descriptionList = new List<string>();
                        for (int i = 0; i < valueArray.Length - 1; i++)
                        {
                            descriptionList.Add(valueArray[i]);
                        }
                        mixTrack.TrackDescription = string.Join(", ", descriptionList.ToArray());
                    }
                    else
                    {
                        mixTrack.TrackDescription = value;
                        mixTrack.MixLevel = 1;
                    }
                }
                else
                {
                    mixTrack.TrackDescription = value;
                    mixTrack.MixLevel = 1;
                }

                return mixTrack;
            }
        }

        #endregion
   }
}
