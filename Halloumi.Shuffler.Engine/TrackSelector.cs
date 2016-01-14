using System;
using System.Collections.Generic;
using System.Linq;
using Halloumi.Common.Helpers;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Engine
{
    public class TrackSelector
    {
        private static Random _random = new Random((int)DateTime.Now.Ticks);

        public enum Direction
        {
            Up,
            Down,
            Any,
            Cycle
        }

        public enum MixStrategy
        {
            None,
            BestMix,
            Variety,
            ExtraVariety,
            Unranked,
            SequentialUp,
            Working
        }

        public enum AllowBearableMixStrategy
        {
            Always,
            Never,
            AfterPowerDown,
            AfterEachGoodTrack,
            AfterTwoGoodTracks,
            WhenNecessary
        }

        public enum UseExtendedMixes
        {
            Always,
            Never,
            Any
        }

        public enum ContinueMix
        {
            Yes,
            No,
            IfPossible
        }

        public enum KeyMixStrategy
        {
            VeryGood,
            Good,
            GoodIfPossible,
            Bearable,
            Any
        }

        private List<Track> AvailableTracks { get; set; }

        private MixLibrary MixLibrary { get; set; }

        public void CancelGeneratePlayList()
        {
            _cancelGeneratePlayList = true;
        }

        private bool _cancelGeneratePlayList = false;

        public void StopGeneratePlayList()
        {
            _stopGeneratePlayList = true;
        }

        private bool _stopGeneratePlayList = false;

        private bool IsGenerationHalted()
        {
            return (_cancelGeneratePlayList || _stopGeneratePlayList);
        }

        /// <summary>
        /// Generates a playlist with the best mix rank.
        /// </summary>
        /// <param name="availableTracks">The available tracks.</param>
        /// <param name="mixLibrary">The mix library.</param>
        /// <param name="currentPlaylist">The current playlist.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="approximateLength">Approximate length the final playlist should be in minutes.</param>
        /// <param name="allowBearable">The allow bearable.</param>
        /// <param name="strategy">The strategy.</param>
        /// <param name="useExtendedMixes">The use extended mixes.</param>
        /// <param name="excludedMixes">The excluded mixes.</param>
        /// <param name="restrictArtistClumping">If set to true, artist clumping is restricted.</param>
        /// <param name="restrictGenreClumping">If set to true, genre clumping is restricted.</param>
        /// <param name="restrictTitleClumping">if set to true, restrict title clumping.</param>
        /// <param name="ignoreFirstMix">If set to true, ignores the quality of the first mix.</param>
        /// <param name="keyMixStrategy">The key mixing strategy.</param>
        /// <param name="maxTracksToAdd">The maximum number tracks to add to the existing playlist.</param>
        /// <returns>
        /// A list of tracks with the best mix rank
        /// </returns>
        public List<Track> GeneratePlayList(List<Track> availableTracks,
            MixLibrary mixLibrary,
            List<Track> currentPlaylist,
            Direction direction,
            int approximateLength,
            AllowBearableMixStrategy allowBearable,
            MixStrategy strategy,
            UseExtendedMixes useExtendedMixes,
            Dictionary<string, Dictionary<string, Track>> excludedMixes,
            bool restrictArtistClumping,
            bool restrictGenreClumping,
            bool restrictTitleClumping,
            ContinueMix continueMix,
            KeyMixStrategy keyMixStrategy,
            int maxTracksToAdd)
        {
            if (strategy == MixStrategy.Working && currentPlaylist.Count == 0) return currentPlaylist;

            Track workingTrack = null;
            if (strategy == MixStrategy.Working)
            {
                direction = Direction.Any;
                workingTrack = currentPlaylist.Last();
            }

            this.GeneratePlayListStatus = "";
            this.MixLibrary = mixLibrary;

            this.AvailableTracks = new List<Track>();
            foreach (var track in availableTracks)
            {
                if (!this.AvailableTracks.Exists(t => t.Description == track.Description))
                {
                    this.AvailableTracks.Add(track);
                }
            }
            if (strategy == MixStrategy.Working) this.AvailableTracks.RemoveAll(t => this.MixLibrary.GetMixOutCount(t) == 0);

            if (this.AvailableTracks.Count == 0) return currentPlaylist;

            var availableTrackDescriptions = GetDistinctTrackDescriptions(this.AvailableTracks);

            var trackCountLimit = int.MaxValue;
            if (approximateLength > 0 && approximateLength != int.MaxValue)
            {
                var currentLength = currentPlaylist.Sum(cp => cp.Length);
                var requiredLength = Convert.ToInt32((approximateLength * 60) - currentLength);

                if (requiredLength <= 0) return new List<Track>();
                var averageLength = this.AvailableTracks.Average(t => t.Length);

                trackCountLimit = (requiredLength / Convert.ToInt32(averageLength)) + currentPlaylist.Count;

                if (trackCountLimit == currentPlaylist.Count) return new List<Track>();
            }

            if (maxTracksToAdd != int.MaxValue && trackCountLimit > currentPlaylist.Count + maxTracksToAdd)
            {
                trackCountLimit = currentPlaylist.Count + maxTracksToAdd;
            }

            if (strategy != MixStrategy.Unranked && strategy != MixStrategy.Working)
            {
                if (trackCountLimit > this.AvailableTracks.Count)
                {
                    trackCountLimit = this.AvailableTracks.Count;
                }
            }

            var initialPlaylistCount = currentPlaylist.Count;

            var currentPaths = new List<TrackPath>();
            if (currentPlaylist.Count == 0)
            {
                foreach (var track in this.AvailableTracks)
                {
                    var path = new TrackPath(track);
                    CalculateAverageRankForPath(path);
                    currentPaths.Add(path);
                }
            }
            else if (continueMix == ContinueMix.No)
            {
                foreach (var track in this.AvailableTracks)
                {
                    var playlist = new List<Track>(currentPlaylist);
                    playlist.Add(track);
                    var path = new TrackPath(playlist);
                    CalculateAverageRankForPath(path);
                    currentPaths.Add(path);
                }
            }
            else
            {
                var path = new TrackPath(currentPlaylist);
                CalculateAverageRankForPath(path);
                currentPaths.Add(path);
            }

            _cancelGeneratePlayList = false;
            _stopGeneratePlayList = false;

            var nextPaths = new List<TrackPath>();
            while (!IsGenerationHalted())
            {
                ParallelHelper.ForEach(currentPaths, currentPath => GeneratePaths(direction,
                    allowBearable,
                    strategy,
                    useExtendedMixes,
                    excludedMixes,
                    restrictArtistClumping,
                    restrictGenreClumping,
                    restrictTitleClumping,
                    keyMixStrategy,
                    workingTrack,
                    availableTrackDescriptions,
                    nextPaths,
                    currentPath));

                if (IsGenerationHalted()) break;

                if (nextPaths.Count == 0) break;

                this.GeneratePlayListStatus = string.Format("Generated {0} possible paths for {1} of {2} tracks.",
                        nextPaths.Count,
                        nextPaths[0].Tracks.Count,
                        trackCountLimit);

                var max = 50 * Environment.ProcessorCount;

                if (nextPaths.Count > max)
                {
                    nextPaths = nextPaths
                        .OrderByDescending(t => t.AverageRank)
                        .Take(max)
                        .ToList();
                }

                currentPaths.Clear();
                currentPaths.AddRange(nextPaths);

                if (nextPaths[0].Tracks.Count >= trackCountLimit) break;
                nextPaths.Clear();
            }

            if (_cancelGeneratePlayList) return currentPlaylist;

            var resultPath = currentPaths
                .OrderByDescending(t => GetAverageTrackAndMixAndKeyRank(t.Tracks))
                .FirstOrDefault();

            if ((strategy == MixStrategy.BestMix || strategy == MixStrategy.Variety || strategy == MixStrategy.ExtraVariety)
                && resultPath != null
                && resultPath.Tracks.Count < trackCountLimit
                && resultPath.Tracks.Count > 0)
            {
                availableTrackDescriptions = GetDistinctTrackDescriptions(this.AvailableTracks);
                var excludeTrackDescriptions = GetDistinctTrackDescriptions(resultPath.Tracks);
                var currentTrack = resultPath.Tracks[resultPath.Tracks.Count - 1];
                var nextTrack = GetBestMixTracks(currentTrack, resultPath.Tracks, allowBearable, availableTrackDescriptions, excludeTrackDescriptions, restrictArtistClumping, restrictArtistClumping, restrictTitleClumping, keyMixStrategy)
                        .OrderBy(t => GetAverageTrackAndMixAndKeyRank(currentTrack, t))
                        .FirstOrDefault();

                if (nextTrack != null) resultPath.Tracks.Add(nextTrack);
            }

            var resultTracks = (resultPath != null)
                ? resultPath.Tracks
                : new List<Track>();

            if (continueMix == ContinueMix.IfPossible && resultTracks.Count == initialPlaylistCount)
            {
                return GeneratePlayList(availableTracks,
                    mixLibrary,
                    currentPlaylist,
                    direction,
                    approximateLength,
                    allowBearable,
                    strategy,
                    useExtendedMixes,
                    excludedMixes,
                    restrictArtistClumping,
                    restrictGenreClumping,
                    restrictTitleClumping,
                    ContinueMix.No,
                    keyMixStrategy,
                    maxTracksToAdd);
            }

            return resultTracks;
        }

        private void GeneratePaths(Direction direction,
            AllowBearableMixStrategy allowBearable,
            MixStrategy strategy,
            UseExtendedMixes useExtendedMixes,
            Dictionary<string, Dictionary<string, Track>> excludedMixes,
            bool restrictArtistClumping,
            bool restrictGenreClumping,
            bool restrictTitleClumping,
            KeyMixStrategy keyMixStrategy,
            Track workingTrack,
            HashSet<string> availableTrackDescriptions,
            List<TrackPath> nextPaths,
            TrackPath currentPath)
        {
            var currentTrack = currentPath.Tracks.Last();

            //DebugHelper.WriteLine("Start GeneratePaths " + currentTrack.Description);

            var excludeTrackDescriptions = GetDistinctTrackDescriptions(currentPath.Tracks);

            List<Track> mixTracks = null;
            if (strategy == MixStrategy.BestMix || strategy == MixStrategy.Variety || strategy == MixStrategy.ExtraVariety)
            {
                mixTracks = GetBestMixTracks(currentTrack, currentPath.Tracks, allowBearable, availableTrackDescriptions, excludeTrackDescriptions, restrictArtistClumping, restrictGenreClumping, restrictTitleClumping, keyMixStrategy);
            }
            else if (strategy == MixStrategy.Unranked)
            {
                mixTracks = GetUnrankedTracks(currentTrack, currentPath.Tracks, availableTrackDescriptions, excludeTrackDescriptions, keyMixStrategy, true);
            }
            else if (strategy == MixStrategy.Working)
            {
                mixTracks = GetWorkingTracks(currentTrack, currentPath.Tracks, workingTrack, availableTrackDescriptions, excludeTrackDescriptions, keyMixStrategy);
            }
            else
            {
                mixTracks = new List<Track>();
            }

            if (direction != Direction.Any)
            {
                var preferredDirection = direction;
                if (preferredDirection == Direction.Cycle) preferredDirection = GetPreferredDirection(currentTrack, currentPath.Tracks);

                var filteredTracks = FilterTracksByDirection(currentTrack, mixTracks, preferredDirection);
                if (filteredTracks.Count > 0) mixTracks = filteredTracks;
            }

            if ((strategy == MixStrategy.BestMix || strategy == MixStrategy.Variety || strategy == MixStrategy.ExtraVariety)
                && useExtendedMixes != UseExtendedMixes.Any)
            {
                mixTracks = FilterTracksByExtendedMix(currentTrack, mixTracks, useExtendedMixes);
            }

            if (excludedMixes != null)
            {
                mixTracks = FilterExcludedMixes(currentTrack, mixTracks, excludedMixes);
            }

            if (strategy == MixStrategy.BestMix || strategy == MixStrategy.Variety || strategy == MixStrategy.ExtraVariety)
            {
                mixTracks = mixTracks
                 .OrderByDescending(x => GetAverageTrackAndMixAndKeyRank(currentTrack, x))
                 .ThenBy(x => x.Length)
                 .ToList();

                if (strategy == MixStrategy.Variety)
                {
                    mixTracks = FilterTracksForVariety(currentTrack, mixTracks);
                }
                else if (strategy == MixStrategy.ExtraVariety)
                {
                    mixTracks = FilterTracksForExtraVariety(currentTrack, mixTracks);
                }
            }

            var max = 3 * Environment.ProcessorCount;
            mixTracks = mixTracks
             .Take(max)
             .ToList();

            foreach (var mixTrack in mixTracks)
            {
                var newPath = new TrackPath(mixTrack, currentPath);

                lock (nextPaths)
                {
                    nextPaths.Add(newPath);
                    CalculateAverageRankForPath(newPath);
                }

                if (IsGenerationHalted()) break;
            }

            //DebugHelper.WriteLine("End GeneratePaths " + currentTrack.Description);
        }

        private List<Track> FilterTracksForVariety(Track currentTrack, List<Track> mixTracks)
        {
            if (mixTracks.Count < 3) return mixTracks;

            var midIndex = mixTracks.Count / 2;

            mixTracks = mixTracks
                .OrderBy(np => _random.Next())
                .Take(midIndex)
                .OrderByDescending(x => GetAverageTrackAndMixAndKeyRank(currentTrack, x))
                .ToList();

            return mixTracks;
        }

        private List<Track> FilterTracksForExtraVariety(Track currentTrack, List<Track> mixTracks)
        {
            if (mixTracks.Count < 3) return mixTracks;

            var midIndex = mixTracks.Count / 2;

            if (_random.Next() % 2 == 0)
            {
                mixTracks = mixTracks
                    .OrderBy(np => _random.Next())
                    .Take(midIndex)
                    .OrderByDescending(x => GetAverageTrackAndMixAndKeyRank(currentTrack, x))
                    .ToList();
            }
            else
            {
                mixTracks.RemoveAll(x => mixTracks.IndexOf(x) < midIndex && mixTracks.IndexOf(x) % 2 == 0);
            }

            return mixTracks;
        }

        private class TrackPath
        {
            public List<Track> Tracks { get; private set; }

            public double AverageRank { get; set; }

            public TrackPath(Track newTrack)
                : this(newTrack, (List<Track>)null)
            { }

            public TrackPath(TrackPath existingTracks)
                : this(null, existingTracks)
            { }

            public TrackPath(Track newTrack, TrackPath existingTracks)
                : this(newTrack, existingTracks.Tracks)
            {
                this.AverageRank = existingTracks.AverageRank;
            }

            public TrackPath(List<Track> existingTracks)
                : this(null, existingTracks)
            { }

            public TrackPath(Track newTrack, List<Track> existingTracks)
            {
                this.Tracks = new List<Track>();

                if (existingTracks != null && existingTracks != null && existingTracks.Count > 0)
                {
                    this.Tracks.AddRange(existingTracks);
                }

                if (newTrack != null)
                    this.Tracks.Add(newTrack);

                this.AverageRank = 0;
            }
        }

        private void CalculateAverageRankForPath(TrackPath path)
        {
            if (path.AverageRank == 0 || path.Tracks.Count < 3)
            {
                path.AverageRank = GetAverageTrackAndMixAndKeyRank(path.Tracks);
            }
            else
            {
                var track1 = path.Tracks[path.Tracks.Count - 2];
                var track2 = path.Tracks[path.Tracks.Count - 1];
                var trackRank = GetAverageTrackAndMixAndKeyRank(track1, track2);

                path.AverageRank = (((path.Tracks.Count - 1) * path.AverageRank) + trackRank) / path.Tracks.Count;
            }
        }

        /// <summary>
        /// Gets a hash set of distinct track descriptions from a list of tracks
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <returns>A hash set of distinct track descriptions</returns>
        private HashSet<string> GetDistinctTrackDescriptions(List<Track> tracks)
        {
            return new HashSet<string>(tracks.Select(t => t.Description).Distinct());
        }

        /// <summary>
        /// Gets the best mix tracks for playlist generation.
        /// </summary>
        /// <param name="currentTrack">The current track.</param>
        /// <param name="currentPath">The current path.</param>
        /// <param name="allowBearable">The allow bearable.</param>
        /// <param name="availableTrackDescriptions">The available track descriptions.</param>
        /// <param name="excludeTrackDescriptions">The exclude track descriptions.</param>
        /// <param name="restrictArtistClumping">If set to true, artist clumping is restricted.</param>
        /// <param name="restrictGenreClumping">if set to true> restrict genre clumping.</param>
        /// <param name="restrictTitleClumping">if set to true restrict title clumping.</param>
        /// <returns>
        /// A list of filtered mix tracks
        /// </returns>
        private List<Track> GetBestMixTracks(Track currentTrack,
            List<Track> currentPath,
            AllowBearableMixStrategy allowBearable,
            HashSet<string> availableTrackDescriptions,
            HashSet<string> excludeTrackDescriptions,
            bool restrictArtistClumping,
            bool restrictGenreClumping,
            bool restrictTitleClumping,
            KeyMixStrategy keyMixStrategy)
        {
            var mixTracks = this.MixLibrary
                .GetGoodTracks(currentTrack)
                .Where(t => !excludeTrackDescriptions.Contains(t.Description))
                .Where(t => availableTrackDescriptions.Contains(t.Description))
                .ToList();

            FilterMixableTracks(currentTrack,
                currentPath,
                restrictArtistClumping,
                restrictGenreClumping,
                restrictTitleClumping,
                keyMixStrategy,
                mixTracks);

            var bearableAllowed = IsBearableTrackMixAllowed(currentTrack, currentPath, allowBearable, mixTracks);

            if (!bearableAllowed) mixTracks.RemoveAll(t => t.Rank <= 2);

            if (bearableAllowed)
            {
                var bearableTracks = this.MixLibrary
                    .GetBearableTracks(currentTrack)
                    .Where(t => !excludeTrackDescriptions.Contains(t.Description))
                    .Where(t => availableTrackDescriptions.Contains(t.Description))
                    .ToList();

                mixTracks.AddRange(bearableTracks);
            }

            FilterMixableTracks(currentTrack,
                currentPath,
                restrictArtistClumping,
                restrictGenreClumping,
                restrictTitleClumping,
                keyMixStrategy,
                mixTracks);

            return mixTracks;
        }

        /// <summary>
        /// Filters the mixable tracks.
        /// </summary>
        /// <param name="currentTrack">The current track.</param>
        /// <param name="currentPath">The current path.</param>
        /// <param name="restrictArtistClumping">If set to true, restrict artist clumping.</param>
        /// <param name="restrictGenreClumping">If set to true, restrict genre clumping.</param>
        /// <param name="restrictTitleClumping">If set to true, restrict title clumping.</param>
        /// <param name="keyMixStrategy">The key mixing strategy.</param>
        /// <param name="mixTracks">The mix tracks.</param>
        private void FilterMixableTracks(Track currentTrack,
            List<Track> currentPath,
            bool restrictArtistClumping,
            bool restrictGenreClumping,
            bool restrictTitleClumping,
            KeyMixStrategy keyMixStrategy,
            List<Track> mixTracks)
        {
            var cycleTracks = this.GetCycleHistory(currentPath);

            if (restrictTitleClumping)
                FilterMixableTracksByTitle(mixTracks, cycleTracks);

            if (restrictArtistClumping)
                FilterMixableTracksByArtist(mixTracks, cycleTracks);

            if (restrictGenreClumping)
                FilterMixableTracksByGenre(mixTracks, cycleTracks);

            FilterByKeyMixStrategy(currentTrack, mixTracks, keyMixStrategy);
        }

        private void FilterByKeyMixStrategy(Track currentTrack, List<Track> mixTracks, KeyMixStrategy keyMixStrategy)
        {
            if (keyMixStrategy == KeyMixStrategy.Any) return;

            var minimumKeyMixRank = 0;
            if (keyMixStrategy == KeyMixStrategy.VeryGood) minimumKeyMixRank = 4;
            if (keyMixStrategy == KeyMixStrategy.Bearable) minimumKeyMixRank = 2;
            if (keyMixStrategy == KeyMixStrategy.Good || keyMixStrategy == KeyMixStrategy.GoodIfPossible) minimumKeyMixRank = 3;
            if (minimumKeyMixRank == 0) return;

            if (keyMixStrategy == KeyMixStrategy.GoodIfPossible)
            {
                var notGoodCount = mixTracks.Count(t => GetAdjustedKeyMixRank(currentTrack, t, keyMixStrategy) < minimumKeyMixRank);
                if (notGoodCount == mixTracks.Count)
                    minimumKeyMixRank = 2;
            }

            mixTracks.RemoveAll(t => GetAdjustedKeyMixRank(currentTrack, t, keyMixStrategy) < minimumKeyMixRank);
        }

        public int GetAdjustedKeyMixRank(Track track1, Track track2, KeyMixStrategy keyMixStrategy)
        {
            var keyRank = BE.KeyHelper.GetKeyMixRank(track1.Key, track2.Key);

            if (keyMixStrategy != KeyMixStrategy.Good && keyMixStrategy != KeyMixStrategy.VeryGood)
            {
                var mixLevel = this.MixLibrary.GetExtendedMixLevel(track1, track2);
                if (mixLevel >= 5) keyRank += 3;
                else if (mixLevel >= 4) keyRank += 2;
                else if (mixLevel >= 3) keyRank += 1;
                if (keyRank > 5) keyRank = 5;
            }

            return keyRank;
        }

        /// <summary>
        /// Determines whether a bearable track/mix is allowed.
        /// </summary>
        /// <param name="currentTrack">The current track.</param>
        /// <param name="currentPath">The current path.</param>
        /// <param name="allowBearable">The allow bearable.</param>
        /// <param name="mixTracks">The mix tracks.</param>
        /// <returns>True if a bearable track/mix is allowed.</returns>
        private bool IsBearableTrackMixAllowed(Track currentTrack, List<Track> currentPath, AllowBearableMixStrategy allowBearable, List<Track> mixTracks)
        {
            var bearableAllowed = false;
            if (allowBearable == AllowBearableMixStrategy.Always)
                bearableAllowed = true;
            else if (allowBearable == AllowBearableMixStrategy.Never)
                bearableAllowed = false;
            else
            {
                var lastMixBearable = false;
                var penultimateMixBearable = false;
                if (currentPath.Count > 1)
                {
                    var lastTrack = currentPath[currentPath.Count - 2];
                    var lastMixRank = this.MixLibrary.GetMixLevel(lastTrack, currentTrack);
                    if (lastMixRank <= 2 || lastTrack.Rank == 2) lastMixBearable = true;

                    if (currentPath.Count > 2)
                    {
                        var penultimateTrack = currentPath[currentPath.Count - 3];
                        var penultimateMixRank = this.MixLibrary.GetMixLevel(penultimateTrack, lastTrack);
                        if (penultimateMixRank <= 2 || penultimateTrack.Rank == 2) penultimateMixBearable = true;
                    }
                }

                if (allowBearable == AllowBearableMixStrategy.AfterTwoGoodTracks)
                {
                    if (!lastMixBearable && !penultimateMixBearable)
                        bearableAllowed = true;
                }
                else if (allowBearable == AllowBearableMixStrategy.AfterEachGoodTrack)
                {
                    if (!lastMixBearable)
                        bearableAllowed = true;
                }
            }

            if (currentTrack.PowerDown
                && (allowBearable == AllowBearableMixStrategy.AfterPowerDown
                || allowBearable == AllowBearableMixStrategy.AfterEachGoodTrack
                || allowBearable == AllowBearableMixStrategy.AfterTwoGoodTracks
                || allowBearable == AllowBearableMixStrategy.WhenNecessary)
                && mixTracks.Count == 0)
            {
                bearableAllowed = true;
            }

            if (mixTracks.Count == 0
                && allowBearable == AllowBearableMixStrategy.WhenNecessary)
            {
                bearableAllowed = true;
            }
            return bearableAllowed;
        }

        /// <summary>
        /// Filters a mixable tracks by genre.
        /// </summary>
        /// <param name="mixTracks">The mix tracks.</param>
        /// <param name="cycleTracks">The cycle tracks.</param>
        private void FilterMixableTracksByGenre(List<Track> mixTracks, List<Track> cycleTracks)
        {
            if (cycleTracks.Count == 0) return;

            var genreTracksPerCycle = 16;

            var removeTracks = new List<Track>();
            foreach (var track in mixTracks)
            {
                var genre = GetGenreForClumpingPurposes(track.Genre);
                if (cycleTracks.Count(mt => GetGenreForClumpingPurposes(mt.Genre) == genre) > genreTracksPerCycle)
                    removeTracks.Add(track);

                if (cycleTracks.Count > 1
                    && GetGenreForClumpingPurposes(cycleTracks[cycleTracks.Count - 1].Genre) == genre
                    && GetGenreForClumpingPurposes(cycleTracks[cycleTracks.Count - 2].Genre) == genre)
                {
                    removeTracks.Add(track);
                }
            }
            if (removeTracks.Count > 0)
            {
                if (mixTracks.Count - removeTracks.Count <= 1)
                { }
                else
                {
                    mixTracks.RemoveAll(t => removeTracks.Exists(rt => rt.Description == t.Description));
                }
            }
        }

        /// <summary>
        /// Filters mixable tracks by artist.
        /// </summary>
        /// <param name="mixTracks">The mix tracks.</param>
        /// <param name="cycleTracks">The cycle tracks.</param>
        private void FilterMixableTracksByArtist(List<Track> mixTracks, List<Track> cycleTracks)
        {
            if (cycleTracks.Count == 0) return;

            var artistTracksPerCycle = 8;

            var removeTracks = new List<Track>();
            foreach (var track in mixTracks)
            {
                var artist = GetArtistForClumpingPurposes(track.Artist);
                if (cycleTracks.Count(mt => GetArtistForClumpingPurposes(mt.Artist) == artist) > artistTracksPerCycle)
                    removeTracks.Add(track);

                if (cycleTracks.Count > 1
                    && GetArtistForClumpingPurposes(cycleTracks[cycleTracks.Count - 1].Artist) == artist
                    && GetArtistForClumpingPurposes(cycleTracks[cycleTracks.Count - 2].Artist) == artist)
                {
                    removeTracks.Add(track);
                }
            }

            mixTracks.RemoveAll(t => removeTracks.Exists(rt => rt.Description == t.Description));
        }

        /// <summary>
        /// Gets the artist name for clumping purposes.
        /// </summary>
        /// <param name="artist">The artist.</param>
        /// <returns>The artist name</returns>
        private string GetArtistForClumpingPurposes(string artist)
        {
            return artist
                .Replace("The J.B.'s", "James Brown")
                .ToLower();
        }

        /// <summary>
        /// Gets genre name for clumping purposes.
        /// </summary>
        /// <param name="artist">The artist.</param>
        /// <returns>The genre name</returns>
        private string GetGenreForClumpingPurposes(string artist)
        {
            return artist
                .Replace("Dub", "Reggae")
                .Replace("Soul", "Funk")
                .ToLower();
        }

        /// <summary>
        /// Filters a mixable tracks by title.
        /// </summary>
        /// <param name="mixTracks">The mix tracks.</param>
        /// <param name="cycleTracks">The cycle tracks.</param>
        private void FilterMixableTracksByTitle(List<Track> mixTracks, List<Track> cycleTracks)
        {
            if (cycleTracks.Count == 0 || cycleTracks.Count == 1) return;

            var cycleTitles = cycleTracks
                .Select(t => t.Title)
                .Distinct()
                .ToList();

            var lastTitle = cycleTracks[cycleTracks.Count - 1].Title;
            cycleTitles.Remove(lastTitle);

            cycleTracks.RemoveAt(cycleTracks.Count - 1);

            var removeTracks = new List<Track>();
            foreach (var track in mixTracks)
            {
                var title = GetTitleForClumpingPurposes(track.Title);
                if (cycleTitles.Exists(ct => title.Equals(GetTitleForClumpingPurposes(ct), StringComparison.InvariantCultureIgnoreCase)))
                    removeTracks.Add(track);
            }

            mixTracks.RemoveAll(t => removeTracks.Exists(rt => rt.Description == t.Description));
        }

        /// <summary>
        /// Gets the title with anything after the ( removed
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>The title</returns>
        private string GetTitleForClumpingPurposes(string title)
        {
            if (!title.Contains("(")) return title;
            if (title.StartsWith("(")) return title;

            return title.Substring(0, title.IndexOf("(") - 1).Trim();
        }

        private List<Track> GetUnrankedTracks(Track currentTrack,
            List<Track> currentPath,
            HashSet<string> availableTrackDescriptions,
            HashSet<string> excludeTrackDescriptions,
            KeyMixStrategy keyMixStrategy,
            bool toTracksOnly)
        {
            var mixTracks = this.MixLibrary.GetUnrankedToTracks(currentTrack).ToList();

            if (!toTracksOnly)
                mixTracks.AddRange(this.MixLibrary.GetUnrankedFromTracks(currentTrack));

            mixTracks = mixTracks
                .Where(t => !excludeTrackDescriptions.Contains(t.Description))
                .Where(t => availableTrackDescriptions.Contains(t.Description))
                .Distinct()
                .ToList();

            FilterByKeyMixStrategy(currentTrack, mixTracks, keyMixStrategy);

            if (mixTracks.Count == 0)
            {
                // find all tracks in the list that have already been mixed into from the current track
                var excludeTracks = new List<Track>();
                for (var i = 1; i < currentPath.Count; i++)
                {
                    if (currentPath[i - 1].Title == currentTrack.Title)
                        excludeTracks.Add(currentPath[i]);
                }

                // get all unranked tracks apart from those ones
                mixTracks = this.MixLibrary
                    .GetUnrankedToTracks(currentTrack)
                    .Union(this.MixLibrary.GetUnrankedFromTracks(currentTrack))
                    .Except(excludeTracks)
                    .Where(t => availableTrackDescriptions.Contains(t.Description))
                    .Distinct()
                    .ToList();
            }

            FilterByKeyMixStrategy(currentTrack, mixTracks, keyMixStrategy);

            mixTracks = mixTracks.OrderByDescending(t => BE.KeyHelper.GetKeyMixRank(currentTrack.Key, t.Key))
                .ThenBy(t => t.Rank)
                .ThenBy(t => BE.BassHelper.GetAdjustedBpmPercentChange(currentTrack.EndBpm, t.StartBpm))
                .ToList();

            return mixTracks;
        }

        private List<Track> GetWorkingTracks(Track currentTrack,
            List<Track> currentPath,
            Track workingTrack,
            HashSet<string> availableTrackDescriptions,
            HashSet<string> excludeTrackDescriptions,
            KeyMixStrategy keyMixStrategy)
        {
            var mixTracks = new List<Track>();
            if (currentTrack.Title == workingTrack.Title)
            {
                mixTracks = GetUnrankedTracks(workingTrack,
                    currentPath,
                    availableTrackDescriptions,
                    excludeTrackDescriptions,
                    keyMixStrategy,
                    false)
                    .Take(1)
                    .ToList();
            }
            else
            {
                mixTracks.Add(workingTrack);
            }

            return mixTracks;
        }

        public string GeneratePlayListStatus
        {
            get;
            internal set;
        }

        private double GetAverageTrackAndMixAndKeyRank(List<Track> tracks)
        {
            if (tracks == null || tracks.Count < 2) return 0;

            var ranks = new List<double>();
            for (var i = 1; i < tracks.Count; i++)
            {
                ranks.Add(GetAverageTrackAndMixAndKeyRank(tracks[i - 1], tracks[i]));
            }
            return ranks.Average();
        }

        private double GetAverageTrackAndMixAndKeyRank(Track track1, Track track2)
        {
            const double mixRankWeight = 1.3D;
            const double trackRankWeight = 0.9D;
            const double keyRankWeight = 0.8D;
            // must equal 3
            const double totalWeight = mixRankWeight + trackRankWeight + keyRankWeight;

            var mixRank = this.MixLibrary.GetExtendedMixLevel(track1, track2);
            var keyMixRank = GetAdjustedKeyMixRank(track1, track2, KeyMixStrategy.Any);
            if (keyMixRank == 5) keyMixRank = 4;
            if (keyMixRank == 0) keyMixRank = 1;
            return ((mixRank * mixRankWeight) + ((double)track2.Rank * trackRankWeight) + ((double)keyMixRank * keyMixRank)) / totalWeight;
        }

        private List<Track> RemoveTracksFromList(IEnumerable<Track> tracks, IEnumerable<Track> excludeTracks)
        {
            if (excludeTracks == null) return new List<Track>();
            var excludeDescriptions = new HashSet<string>(excludeTracks.Select(t => t.Description).Distinct());
            return tracks.Where(t => !excludeDescriptions.Contains(t.Description)).ToList();
        }

        /// <summary>
        /// Filters the tracks by direction.
        /// </summary>
        /// <param name="currentTrack">The current track.</param>
        /// <param name="tracks">The tracks.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>The filtered tracks</returns>
        private List<Track> FilterTracksByDirection(Track currentTrack, List<Track> tracks, Direction direction)
        {
            var filteredTracks = new List<Track>(tracks);
            if (direction == Direction.Down) return filteredTracks.Where(t => t.StartBpm < currentTrack.EndBpm).ToList();
            if (direction == Direction.Up) return filteredTracks.Where(t => t.StartBpm >= currentTrack.EndBpm).ToList();
            else return filteredTracks;
        }

        /// <summary>
        /// Filters the tracks by extended mix.
        /// </summary>
        /// <param name="currentTrack">The current track.</param>
        /// <param name="mixTracks">The mix tracks.</param>
        /// <param name="useExtendedMixes">The use extended mixes.</param>
        /// <returns>The filtered tracks</returns>
        private List<Track> FilterTracksByExtendedMix(Track currentTrack, List<Track> mixTracks, UseExtendedMixes useExtendedMixes)
        {
            var extendedMixTracks = mixTracks.Where(mt => this.MixLibrary.HasExtendedMix(currentTrack, mt));

            if (useExtendedMixes == UseExtendedMixes.Always)
                return extendedMixTracks.ToList();
            else if (useExtendedMixes == UseExtendedMixes.Never)
                return mixTracks.Except(extendedMixTracks).ToList();
            else
                return mixTracks;
        }

        /// <summary>
        /// Filters out excluded mixes.
        /// </summary>
        /// <param name="currentTrack">The current track.</param>
        /// <param name="mixTracks">The mix tracks.</param>
        /// <param name="excludedMixes">The excluded mixes.</param>
        /// <returns>A list of mix tracks with the excluded mixes removed</returns>
        private List<Track> FilterExcludedMixes(Track currentTrack, List<Track> mixTracks, Dictionary<string, Dictionary<string, Track>> excludedMixes)
        {
            if (excludedMixes.ContainsKey(currentTrack.Description))
            {
                var trackExcludedMixes = excludedMixes[currentTrack.Description];
                mixTracks = mixTracks.Where(mt => !trackExcludedMixes.ContainsKey(mt.Description)).ToList();
            }
            return mixTracks;
        }

        private Direction GetPreferredDirection(Track track, List<Track> history)
        {
            if (track == null) return Direction.Up;

            var direction = Direction.Any;

            var fullHistory = new List<Track>(history);
            fullHistory.Add(track);

            var medianBpm = GetMedianBpm(fullHistory);
            if (track.Bpm < medianBpm) direction = Direction.Up;
            if (track.Bpm > medianBpm) direction = Direction.Down;
            return direction;
        }

        private decimal GetMedianBpm(List<Track> history)
        {
            if (history == null) return 0M;
            history = history.Where(t => t != null).ToList();
            if (history.Count == 0) return 0M;

            var historyLength = history.Sum(t => t.Length);
            var libraryLength = this.AvailableTracks.Sum(t => t.Length);

            var available = this.AvailableTracks.OrderBy(t => t.Bpm).ToList();
            if (available.Count == 0) return 0;

            decimal cycleLength = GetCycleLength();
            if (cycleLength > libraryLength) cycleLength = libraryLength;

            var halfCycle = cycleLength / 2;
            var completedCycles = historyLength / cycleLength;

            var firstTrackBpmPosition = 0;
            for (var i = 0; i < available.Count; i++)
            {
                if (history[0].Bpm >= available[i].Bpm)
                {
                    firstTrackBpmPosition = i;
                }
            }
            var firsTrackBpmAdjustment = (decimal)(firstTrackBpmPosition + 1) / (decimal)available.Count;

            completedCycles += (firsTrackBpmAdjustment / 2);

            var currentCyclePosition = completedCycles - (int)completedCycles;

            var halfCyclePosition = 0M;
            var medianIndex = 0;
            if (currentCyclePosition < 0.5M)
            {
                halfCyclePosition = currentCyclePosition * 2;
                medianIndex = (int)(halfCyclePosition * available.Count);
            }
            else
            {
                halfCyclePosition = (currentCyclePosition - 0.5M) * 2;
                medianIndex = (available.Count - (int)(halfCyclePosition * available.Count)) - 1;
            }

            if (medianIndex < 0)
            {
                return available.Average(t => t.Bpm);
            }

            return available[medianIndex].Bpm;
        }

        /// <summary>
        /// Gets the length of the cycle in seconds.
        /// </summary>
        /// <returns>The length of the cycle in seconds</returns>
        private int GetCycleLength()
        {
            // four hours
            return 60 * 60 * 4;
        }

        private List<Track> GetCycleHistory(List<Track> history)
        {
            var cycleLength = GetCycleLength();
            return GetCycleHistory(history, cycleLength);
        }

        private List<Track> GetCycleHistory(List<Track> history, int cycleLength)
        {
            // get history excluding tracks not in current cycle
            var cycleHistory = new List<Track>();
            for (var i = history.Count - 1; i >= 0; i--)
            {
                cycleHistory.Insert(0, history[i]);
                if (cycleHistory.Sum(t => t.Length) > cycleLength) break;
            }
            return cycleHistory;
        }
    }
}