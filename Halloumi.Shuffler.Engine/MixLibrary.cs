using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Track = Halloumi.Shuffler.AudioLibrary.Models.Track;

namespace Halloumi.Shuffler.AudioLibrary
{
    /// <summary>
    ///     A library of preferred/compulsory/forbidden track segues.
    /// </summary>
    public class MixLibrary
    {
        public enum MixRank
        {
            Forbidden = 0,
            Bearable = 2,
            Good = 3,
            VeryGood = 4,
            Excellent = 5,
            Unranked = 1
        }

        private readonly Dictionary<string, List<MixRanking>> _fromMixes = new Dictionary<string, List<MixRanking>>();

        private readonly List<string> _loadedTracks = new List<string>();

        private readonly Dictionary<string, List<MixRanking>> _toMixes = new Dictionary<string, List<MixRanking>>();


        /// <summary>
        ///     Initializes a new instance of the MixLibrary class.
        /// </summary>
        /// <param name="mixDetailsFolder">The mix details folder.</param>
        public MixLibrary(string mixDetailsFolder)
        {
            MixDetailsFolder = mixDetailsFolder;
            AvailableTracks = new List<Track>();
        }

        /// <summary>
        ///     The tracks available for mixing
        /// </summary>
        public List<Track> AvailableTracks { get; set; }


        /// <summary>
        ///     Gets or sets the folder where mix detail files are stored
        /// </summary>
        public string MixDetailsFolder { get; set; }


        /// <summary>
        ///     Loads all mix details.
        /// </summary>
        public void LoadAllMixDetails()
        {
            var availableTracks = AvailableTracks.Where(t => t.IsShufflerTrack).ToList();
            ParallelHelper.ForEach(availableTracks, track => { LoadMixRankings(track.Description); });
        }

        /// <summary>
        ///     Gets the description of a mix ranking
        /// </summary>
        /// <param name="ranking">The ranking.</param>
        /// <returns>The description of the mix ranking</returns>
        public string GetRankDescription(int ranking)
        {
            switch ((MixRank) ranking)
            {
                case MixRank.Forbidden:
                    return "Forbidden";
                case MixRank.Bearable:
                    return "Bearable";
                case MixRank.Good:
                    return "Good";
                case MixRank.VeryGood:
                    return "Very Good";
                case MixRank.Excellent:
                    return "Excellent";
                case MixRank.Unranked:
                    return "Unranked";
                default:
                    throw new Exception("Weird mix rank");
            }
        }

        /// <summary>
        ///     Gets the mix ranking from a description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The mix ranking</returns>
        public MixRank GetRankFromDescription(string description)
        {
            switch (description)
            {
                case "Forbidden":
                    return MixRank.Forbidden;
                case "Bearable":
                    return MixRank.Bearable;
                case "Good":
                    return MixRank.Good;
                case "Very Good":
                    return MixRank.VeryGood;
                case "Excellent":
                    return MixRank.Excellent;
                default:
                    return MixRank.Unranked;
            }
        }

        /// <summary>
        ///     Clears all loaded mix details
        /// </summary>
        public void Clear()
        {
            lock (_toMixes)
            {
                _loadedTracks.Clear();
                _toMixes.Clear();
                _fromMixes.Clear();
            }
        }

        /// <summary>
        ///     Gets all preferred tracks for the specified track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of tracks that specified track should prefer to mix with</returns>
        private IEnumerable<Track> GetToTracksInRange(Track track)
        {
            if (track == null) return new List<Track>();

            var tracksInRange = GetTracksInStartBpmRange(track.EndBpm, 5M, AvailableTracks);
            return tracksInRange
                .Where(t => t.Description != track.Description)
                .ToList();
        }

        /// <summary>
        ///     Gets all preferred tracks for the specified track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of tracks that specified track should prefer to mix from</returns>
        private IEnumerable<Track> GetFromTracksInRange(Track track)
        {
            if (track == null) return new List<Track>();

            var tracksInRange = GetTracksInEndBpmRange(track.StartBpm, 5M, AvailableTracks);
            return tracksInRange
                .Where(t => t.Description != track.Description)
                .ToList();
        }

        /// <summary>
        ///     Gets the tracks in BPM range.
        /// </summary>
        /// <param name="bpm">The BPM.</param>
        /// <param name="percentVariance">The percent variance.</param>
        /// <param name="tracks">The tracks.</param>
        /// <returns>A list of matching tracks</returns>
        public List<Track> GetTracksInEndBpmRange(decimal bpm, decimal percentVariance, List<Track> tracks)
        {
            return tracks
                .Where(t => BpmHelper.IsBpmInRange(bpm, t.EndBpm, percentVariance))
                .ToList();
        }

        /// <summary>
        ///     Gets the tracks in BPM range.
        /// </summary>
        /// <param name="bpm">The BPM.</param>
        /// <param name="percentVariance">The percent variance.</param>
        /// <param name="tracks">The tracks.</param>
        /// <returns>A list of matching tracks</returns>
        public List<Track> GetTracksInStartBpmRange(decimal bpm, decimal percentVariance, List<Track> tracks)
        {
            return tracks
                .Where(t => BpmHelper.IsBpmInRange(bpm, t.StartBpm, percentVariance))
                .ToList();
        }

        /// <summary>
        ///     Gets the preferred tracks to mix with the supplied track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>The preferred tracks to mix with the supplied track</returns>
        public List<Track> GetPreferredTracks(Track track)
        {
            var ranks = new[] {5, 4, 3, 2}.ToList();
            return GetMixableToTracks(track, ranks);
        }

        /// <summary>
        ///     Gets the preferred tracks to mix with the supplied track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>The preferred tracks to mix with the supplied track</returns>
        public List<Track> GetGoodTracks(Track track)
        {
            var ranks = new[] {5, 4, 3}.ToList();
            return GetMixableToTracks(track, ranks);
        }

        /// <summary>
        ///     Gets the preferred tracks to mix with the supplied track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>The preferred tracks to mix with the supplied track</returns>
        public List<Track> GetBearableTracks(Track track)
        {
            var ranks = new[] {2}.ToList();
            return GetMixableToTracks(track, ranks);
        }

        /// <summary>
        ///     Gets the forbidden tracks for a particular track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>The tracks forbidden to mix with the supplied track</returns>
        public List<Track> GetForbiddenTracks(Track track)
        {
            var ranks = new[] {0}.ToList();
            return GetMixableToTracks(track, ranks);
        }

        public List<Track> GetUnrankedToTracks(Track track)
        {
            // excluded all ranked tracks and current track
            var toMixes = GetToMixes(track.Description);
            var excludeTracks = GetToTracksFromMixes(toMixes).Select(t => t.Description).ToList();
            excludeTracks.Add(track.Description);

            // find tracks in range
            var tracksInRange = GetToTracksInRange(track);

            // return tracks in range, except ranked tracks and current track
            return tracksInRange
                .Where(t => !excludeTracks.Contains(t.Description))
                .ToList();
        }

        public List<Track> GetUnrankedFromTracks(Track track)
        {
            // excluded all ranked tracks and current track
            var fromMixes = GetFromMixes(track.Description);
            var excludeTracks = GetFromTracksFromMixes(fromMixes).Select(t => t.Description).ToList();
            excludeTracks.Add(track.Description);

            // find tracks in range
            var tracksInRange = GetFromTracksInRange(track);

            // return tracks in range, except ranked tracks and current track
            return tracksInRange
                .Where(t => !excludeTracks.Contains(t.Description))
                .ToList();
        }

        public List<Track> GetRankedTracks(Track track)
        {
            var toMixes = GetToMixes(track.Description);
            return GetDistinctToTracksFromMixes(toMixes);
        }

        /// <summary>
        ///     Gets all preferred tracks for the specified track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="mixLevels">The mix levels.</param>
        /// <returns>
        ///     A list of tracks from the available tracks the specified track should prefer to mix with
        /// </returns>
        public List<Track> GetMixableToTracks(Track track, List<int> mixLevels)
        {
            var tracks = new List<Track>();
            if (track == null) return tracks;
            if (mixLevels == null || mixLevels.Count == 0) return tracks;

            if (mixLevels.Contains(1)) tracks.AddRange(GetUnrankedToTracks(track));

            if (mixLevels.Count == 1 && mixLevels[0] == 1) return tracks;

            var mixes = GetToMixes(track.Description)
                .Where(t => mixLevels.Contains(t.MixLevel))
                .ToList();

            tracks.AddRange(GetDistinctToTracksFromMixes(mixes));

            return tracks;
        }

        /// <summary>
        ///     Gets all preferred tracks for the specified track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="mixLevels">The mix levels.</param>
        /// <returns>
        ///     A list of tracks from the available tracks the specified track should prefer to mix with
        /// </returns>
        public List<Track> GetMixableFromTracks(Track track, List<int> mixLevels)
        {
            var tracks = new List<Track>();
            if (track == null) return tracks;
            if (mixLevels == null || mixLevels.Count == 0) return tracks;

            if (mixLevels.Contains(1)) tracks.AddRange(GetUnrankedFromTracks(track));

            if (mixLevels.Count == 1 && mixLevels[0] == 1) return tracks;

            var mixes = GetFromMixes(track.Description)
                .Where(t => mixLevels.Contains(t.MixLevel))
                .ToList();

            tracks.AddRange(GetDistinctFromTracksFromMixes(mixes));

            return tracks;
        }

        private List<Track> GetToTracksFromMixes(IEnumerable<MixRanking> mixRankings)
        {
            var toTracks = mixRankings.Select(r => r.ToTrack).ToList();
            return AvailableTracks.Where(t => toTracks.Contains(t.Description)).ToList();
        }

        private List<Track> GetDistinctToTracksFromMixes(List<MixRanking> mixRankings)
        {
            var tracks = GetToTracksFromMixes(mixRankings);
            var trackNames = tracks.Select(t => t.Description).Distinct().ToList();

            return trackNames
                .Select(trackName => tracks.FirstOrDefault(t => t.Description == trackName))
                .Where(distinctTrack => distinctTrack != null)
                .ToList();
        }

        private List<Track> GetFromTracksFromMixes(IEnumerable<MixRanking> mixRankings)
        {
            var fromTracks = mixRankings.Select(r => r.FromTrack).ToList();
            return AvailableTracks.Where(t => fromTracks.Contains(t.Description)).ToList();
        }

        private IEnumerable<Track> GetDistinctFromTracksFromMixes(IEnumerable<MixRanking> mixRankings)
        {
            var tracks = GetFromTracksFromMixes(mixRankings);
            var trackNames = tracks.Select(t => t.Description).Distinct().ToList();

            return trackNames
                .Select(trackName => tracks.FirstOrDefault(t => t.Description == trackName))
                .Where(distinctTrack => distinctTrack != null)
                .ToList();
        }

        /// <summary>
        ///     Flags track 2 as a mix track for track 1
        /// </summary>
        /// <param name="fromTrack">From track.</param>
        /// <param name="toTrack">To track.</param>
        /// <param name="mixLevel">The mix level.</param>
        public void SetMixLevel(Track fromTrack, Track toTrack, int mixLevel)
        {
            if (fromTrack == null || toTrack == null) return;
            SetMixLevel(fromTrack.Description, toTrack.Description, mixLevel);
            SaveMixRankings(fromTrack.Description);
        }

        /// <summary>
        ///     Sets the mix level.
        /// </summary>
        /// <param name="fromTrackDescription">From track description.</param>
        /// <param name="toTrackDescription">To track description.</param>
        /// <param name="mixLevel">The mix level.</param>
        private void SetMixLevel(string fromTrackDescription, string toTrackDescription, int mixLevel)
        {
            lock (_toMixes)
            {
                var mixRank = GetMixRank(fromTrackDescription, toTrackDescription);
                if (mixLevel == 1)
                {
                    if (mixRank == null) return;

                    var toMixes = GetToMixes(fromTrackDescription);
                    toMixes.Remove(mixRank);

                    var fromMixes = GetFromMixes(toTrackDescription);
                    fromMixes.Remove(mixRank);
                }
                else
                {
                    if (mixRank == null)
                    {
                        mixRank = new MixRanking
                        {
                            FromTrack = fromTrackDescription,
                            ToTrack = toTrackDescription
                        };

                        var toMixes = GetToMixes(fromTrackDescription);
                        toMixes.Add(mixRank);

                        var fromMixes = GetFromMixes(toTrackDescription);
                        fromMixes.Add(mixRank);
                    }
                    mixRank.MixLevel = mixLevel;
                }
            }
        }

        /// <summary>
        ///     Gets a mix track.
        /// </summary>
        /// <param name="fromDescription">From description.</param>
        /// <param name="toDescription">To description.</param>
        /// <returns>The mix track</returns>
        private MixRanking GetMixRank(string fromDescription, string toDescription)
        {
            return GetToMixes(fromDescription)
                .FirstOrDefault(mt => mt.ToTrack == toDescription);
        }

        /// <summary>
        ///     Gets the mix level for mixing track 1 into track 1
        /// </summary>
        /// <param name="track1">The track 1.</param>
        /// <param name="track2">The track 2.</param>
        /// <returns>A mix level from 0 to 5</returns>
        public double GetExtendedMixLevel(Track track1, Track track2)
        {
            double mixLevel = GetMixLevel(track1, track2);
            if (HasExtendedMix(track1, track2)) mixLevel += 0.5;
            else if (track1.PowerDown) mixLevel += 0.25;
            return mixLevel;
        }

        /// <summary>
        ///     Gets the mix level for mixing track 1 into track 1
        /// </summary>
        /// <param name="track1">The track 1.</param>
        /// <param name="track2">The track 2.</param>
        /// <returns>A mix level from 0 to 5</returns>
        public string GetExtendedMixDescription(Track track1, Track track2)
        {
            var description = GetRankDescription(GetMixLevel(track1, track2));
            return HasExtendedMix(track1, track2)
                ? description + "*"
                : description;
        }


        /// <summary>
        ///     Gets the extended mix attributes.
        /// </summary>
        /// <param name="fadeOutTrackDescription">The fade out track description.</param>
        /// <param name="fadeInTrackDescription">The fade in track description.</param>
        /// <returns>The extended mix attributes.</returns>
        private ExtendedMixAttributes GetExtendedMixAttributes(string fadeOutTrackDescription,
            string fadeInTrackDescription)
        {
            var attributes = AutomationAttributes.GetAutomationAttributes(fadeOutTrackDescription, MixDetailsFolder);
            return attributes?.GetExtendedMixAttributes(fadeInTrackDescription);
        }

        /// <summary>
        ///     Determines whether track 1 has extended mix details with track 2
        /// </summary>
        /// <param name="track1">The track1.</param>
        /// <param name="track2">The track2.</param>
        /// <returns>
        ///     True if track 1 has extended mix details with track 2; otherwise, false.
        /// </returns>
        public bool HasExtendedMix(Track track1, Track track2)
        {
            if (track1 == null || track2 == null) return false;

            var attributes = GetExtendedMixAttributes(track1.Description, track2.Description);
            return attributes != null;
        }

        /// <summary>
        ///     Converts a list of tracks into a dictionary of mixes. Assumes each track is mixed into the next one.
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, Track>> ConvertPlaylistToMixDictionary(List<Track> tracks)
        {
            var mixes = new Dictionary<string, Dictionary<string, Track>>();

            for (var i = 0; i < tracks.Count - 1; i++)
            {
                var track1 = tracks[i];
                var track2 = tracks[i + 1];
                if (track1 == null || track2 == null) continue;

                if (!mixes.ContainsKey(track1.Description))
                    mixes.Add(track1.Description, new Dictionary<string, Track>());

                var track1Mixes = mixes[track1.Description];

                if (!track1Mixes.ContainsKey(track2.Description))
                    track1Mixes.Add(track2.Description, track2);
            }

            return mixes;
        }

        /// <summary>
        ///     Gets the mix level for mixing track 1 into track 1
        /// </summary>
        /// <param name="track1">The track 1.</param>
        /// <param name="track2">The track 2.</param>
        /// <returns>A mix level from 0 to 5</returns>
        public int GetMixLevel(Track track1, Track track2)
        {
            if (track1 == null || track2 == null) return 0;

            var mixRank = GetMixRank(track1.Description, track2.Description);
            if (mixRank != null) return mixRank.MixLevel;

            return BpmHelper.IsBpmInRange(track1.EndBpm, track2.StartBpm, 5M) ? 1 : 0;
        }

        public int GetKeyMixedOutCount(Track track)
        {
            var outTracks = GetMixableToTracks(track, new List<int> {5, 4, 3});
            return outTracks.Count(t => KeyHelper.GetKeyMixRank(track.Key, t.Key) >= 3);
        }

        public int GetKeyMixedInCount(Track track)
        {
            var inTracks = GetMixableFromTracks(track, new List<int> {5, 4, 3});
            return inTracks.Count(t => KeyHelper.GetKeyMixRank(t.Key, track.Key) >= 3);
        }

        public int GetMixOutCount(Track track)
        {
            return GetMixOutCount(track, 3);
        }

        public int GetMixOutCount(Track track, int minimumLevel)
        {
            if (!track.IsShufflerTrack) return 0;

            var mixes = GetToMixes(track.Description)
                .Where(mt => mt.MixLevel >= minimumLevel)
                .ToList();

            return GetDistinctToTracksFromMixes(mixes).Count();
        }

        public int GetMixInCount(Track track)
        {
            return GetMixInCount(track, 3);
        }

        public int GetMixInCount(Track track, int minimumLevel)
        {
            if (!track.IsShufflerTrack) return 0;

            var mixes = GetFromMixes(track.Description)
                .Where(mt => mt.MixLevel >= minimumLevel)
                .ToList();

            return GetDistinctFromTracksFromMixes(mixes).Count();
        }

        public int GetUnrankedToCount(Track track)
        {
            return GetUnrankedToTracks(track).Count();
        }

        public void ExportToFolder(string folder)
        {
            ExportToFolder(folder, 0);
        }

        public void ExportToFolder(string folder, int days)
        {
            if (!Directory.Exists(folder)) return;

            FileSystemHelper.DeleteFiles(folder, "*.Mixes.txt", false);

            var sourceFiles = FileSystemHelper.SearchFiles(MixDetailsFolder, "*.Mixes.txt", false);
            foreach (var source in sourceFiles)
            {
                var fileName = Path.GetFileName(source);
                if (fileName == null) continue;

                var destinationFile = Path.Combine(folder, fileName);
                var dateModified = File.GetLastWriteTime(source);
                var difference = DateTime.Now - dateModified;
                if (days <= 0 || difference.Days < days) FileSystemHelper.Copy(source, destinationFile);
            }
        }

        public void ImportFromFolder(string folder, bool deleteAfterImport)
        {
            var importFiles = FileSystemHelper.SearchFiles(folder, "*.Mixes.txt", false);
            foreach (var importFile in importFiles)
            {
                var trackDescription = GetFuzzyTrackDescriptionFromMixFile(importFile);
                var existingFile = GetMixRankingFileName(trackDescription);

                var existingFileDate = File.GetLastWriteTime(existingFile);
                var importFileDate = File.GetLastWriteTime(importFile);

                if (!File.Exists(existingFile))
                {
                    FileSystemHelper.Copy(importFile, existingFile);
                }
                else if (FileSystemHelper.AreFilesDifferent(existingFile, importFile))
                {
                    var changed = MergeMixFile(existingFile, importFile, existingFileDate, importFileDate);
                    if (changed && !deleteAfterImport)
                    {
                        FileSystemHelper.Copy(existingFile, importFile);
                        existingFileDate = File.GetLastWriteTime(existingFile);
                    }
                    File.SetLastWriteTime(importFile, existingFileDate);
                }

                if (deleteAfterImport)
                    File.Delete(importFile);

                FuzzyUncacheMixRanking(trackDescription);
            }

            if (deleteAfterImport) return;

            var existingFiles = FileSystemHelper.SearchFiles(MixDetailsFolder, "*.Mixes.txt", false);
            foreach (var existingFile in existingFiles)
            {
                var fileName = Path.GetFileName(existingFile);
                if (fileName == null) continue;


                var importFile = Path.Combine(folder, fileName);
                if (File.Exists(importFile)) continue;

                var existingFileDate = File.GetLastWriteTime(existingFile);
                FileSystemHelper.Copy(existingFile, importFile);
                File.SetLastWriteTime(importFile, existingFileDate);
            }
        }


        /// <summary>
        ///     Gets the 'mix-from tracks' for a track.
        /// </summary>
        /// <param name="toTrackDescription">To track description.</param>
        /// <returns>
        ///     The mix-from tracks
        /// </returns>
        private List<MixRanking> GetFromMixes(string toTrackDescription)
        {
            lock (_toMixes)
            {
                if (_fromMixes.ContainsKey(toTrackDescription))
                    return _fromMixes[toTrackDescription];

                _fromMixes.Add(toTrackDescription, new List<MixRanking>());

                return _fromMixes[toTrackDescription];
            }
        }

        /// <summary>
        ///     Gets the 'mix-to tracks' for a track.
        /// </summary>
        /// <param name="fromTrackDescription">From track description.</param>
        /// <returns>
        ///     The mix-to tracks
        /// </returns>
        private List<MixRanking> GetToMixes(string fromTrackDescription)
        {
            lock (_toMixes)
            {
                if (!_loadedTracks.Contains(fromTrackDescription))
                {
                    LoadMixRankings(fromTrackDescription);
                }

                if (!_toMixes.ContainsKey(fromTrackDescription))
                {
                    _toMixes.Add(fromTrackDescription, new List<MixRanking>());
                }
                return _toMixes[fromTrackDescription];
            }
        }

        private void SaveMixRankings(string trackDescription)
        {
            var filename = GetMixRankingFileName(trackDescription);
            var toTracks = GetToMixes(trackDescription);
            SaveMixRankings(filename, toTracks);
        }

        private static void SaveMixRankings(string filename, IEnumerable<MixRanking> toTracks)
        {
            var content = new StringBuilder();
            foreach (var toTrack in toTracks)
            {
                content.AppendLine(toTrack.ToString());
            }

            var blank = content.Length == 0 || content.ToString().Trim().Replace(Environment.NewLine, "") == "";
            if (blank && File.Exists(filename))
            {
                File.Delete(filename);
            }
            else if (!blank)
            {
                File.WriteAllText(filename, content.ToString(), Encoding.UTF8);
            }
        }

        /// <summary>
        ///     Loads the mix tracks.
        /// </summary>
        /// <param name="trackDescription">The track description.</param>
        private void LoadMixRankings(string trackDescription)
        {
            lock (_toMixes)
            {
                if (_loadedTracks.Contains(trackDescription)) return;
            }

            DebugHelper.WriteLine("LoadMixRankings - " + trackDescription);

            lock (_toMixes)
            {
                _loadedTracks.Add(trackDescription);
            }

            var filename = GetMixRankingFileName(trackDescription);

            if (!File.Exists(filename)) return;

            var content = File.ReadAllLines(filename).ToList();
            var mixRankings = content
                .Select(line => MixRanking.FromString(line, trackDescription))
                .Where(mixRank => mixRank != null);

            foreach (var mixRank in mixRankings)
            {
                SetMixLevel(mixRank.FromTrack, mixRank.ToTrack, mixRank.MixLevel);
            }
        }

        private void FuzzyUncacheMixRanking(string trackDescription)
        {
            trackDescription = StringHelper.GetAlphaNumericCharactersOnly(trackDescription).ToLower();

            List<string> trackDescriptions;
            lock (_toMixes)
            {
                trackDescriptions =
                    _loadedTracks
                        .Where(
                            loadedTrack =>
                                StringHelper.GetAlphaNumericCharactersOnly(loadedTrack).ToLower() == trackDescription)
                        .ToList();
            }

            foreach (var description in trackDescriptions)
            {
                UncacheMixRanking(description);
            }
        }

        private static bool MergeMixFile(string existingFile, string importFile, DateTime existingFileDate,
            DateTime importFileDate)
        {
            if (!File.Exists(existingFile) || !File.Exists(importFile)) return false;

            var fuzzyTrackDescription = GetFuzzyTrackDescriptionFromMixFile(existingFile);

            var existingRankings = File.ReadAllLines(existingFile)
                .Distinct()
                .Select(s => MixRanking.FromString(s, fuzzyTrackDescription))
                .Where(s => s != null)
                .ToList();

            var importRankings = File.ReadAllLines(importFile)
                .Distinct()
                .Select(s => MixRanking.FromString(s, fuzzyTrackDescription))
                .ToList();

            var changed = false;
            foreach (var importRanking in importRankings)
            {
                var existingRanking = existingRankings
                    .FirstOrDefault(r => r.ToTrack == importRanking.ToTrack);

                if (existingRanking == null)
                {
                    existingRankings.Add(importRanking);
                    changed = true;
                }
                else if (existingRanking.MixLevel != importRanking.MixLevel && existingFileDate < importFileDate)
                {
                    existingRanking.MixLevel = importRanking.MixLevel;
                    changed = true;
                }
            }

            if (changed)
                SaveMixRankings(existingFile, existingRankings);

            return changed;
        }

        private static string GetFuzzyTrackDescriptionFromMixFile(string filename)
        {
            var fileName = Path.GetFileName(filename);
            return fileName?.Replace(".Mixes.txt", "") ?? "";
        }

        /// <summary>
        ///     Gets the preferred tracks object for the specified track.
        ///     Loads from file if not in collection.
        /// </summary>
        /// <param name="trackDescription">The track description.</param>
        private void UncacheMixRanking(string trackDescription)
        {
            lock (_loadedTracks)
            {
                if (!_loadedTracks.Contains(trackDescription)) return;
                _loadedTracks.Remove(trackDescription);
            }
        }

        /// <summary>
        ///     Gets the name of the file used to store the details of a mix ranking object for a specific track
        /// </summary>
        /// <param name="trackDescription">The track description.</param>
        /// <returns>
        ///     A filename, including the full path
        /// </returns>
        private string GetMixRankingFileName(string trackDescription)
        {
            var filename = trackDescription
                           + ".Mixes.txt";

            filename = FileSystemHelper.StripInvalidFileNameChars(filename);

            return Path.Combine(MixDetailsFolder, filename);
        }


        private class MixRanking
        {
            public string FromTrack { get; set; }

            public string ToTrack { get; set; }

            public int MixLevel { get; set; }

            public override string ToString()
            {
                return ToTrack + ", " + MixLevel;
            }

            public static MixRanking FromString(string value, string fromTrack)
            {
                if (value == "") return null;
                var mixRank = new MixRanking {FromTrack = fromTrack};

                if (value.Contains(","))
                {
                    var commaIndex = value.LastIndexOf(",", StringComparison.Ordinal);
                    mixRank.ToTrack = value.Substring(0, commaIndex).Trim();

                    var ranking = value.Substring(commaIndex + 1).Trim();
                    int mixLevel;
                    int.TryParse(ranking, out mixLevel);
                    mixRank.MixLevel = mixLevel;
                }
                else
                {
                    mixRank.ToTrack = value;
                    mixRank.MixLevel = (int) MixRank.Unranked;
                }

                return mixRank;
            }
        }
    }
}