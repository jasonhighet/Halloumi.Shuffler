using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Halloumi.BassEngine
{
    public class TrackSelector
    {
        private static Random _random = new Random((int)DateTime.Now.Ticks);

        private enum Direction
        {
            Up,
            Down,
            Any
        }

        public Library Library { get; set; }
        public MixLibrary MixLibrary { get; set; }

        public Track GetNextTrack(Track currentTrack, List<Track> history)
        {
            Track nextTrack = null;
            List<Track> cycleHistory = null;

            Direction direction = GetPreferredDirection(currentTrack, history);

            nextTrack = GetRandomMixTrack(currentTrack, direction, history);
            if (nextTrack != null) Debug.Print(nextTrack.BPM.ToString("000.00") + "\t MixTrack \t" + nextTrack.Description);
            if (nextTrack != null) return nextTrack;

            if (nextTrack == null) cycleHistory = GetCycleHistory(history);
            if (nextTrack == null) nextTrack = GetRandomMixTrack(currentTrack, direction, cycleHistory);
            if (nextTrack != null) Debug.Print(nextTrack.BPM.ToString("000.00") + "\t MixTrack NonRecentHistory \t" + nextTrack.Description);
            if (nextTrack != null) return nextTrack;

            if (nextTrack == null) nextTrack = GetRandomTrack(currentTrack, direction, history);
            if (nextTrack != null) Debug.Print(nextTrack.BPM.ToString("000.00") + "\t RandomTrack \t" + nextTrack.Description);
            if (nextTrack != null) return nextTrack;

            if (nextTrack == null) nextTrack = GetRandomTrack(currentTrack, direction, cycleHistory);
            if (nextTrack != null) Debug.Print(nextTrack.BPM.ToString("000.00") + "\t RandomTrack NonRecentHistory \t" + nextTrack.Description);
            if (nextTrack != null) return nextTrack;

            if (nextTrack == null) nextTrack = GetRandomMixTrack(currentTrack, direction, null);
            if (nextTrack != null) Debug.Print(nextTrack.BPM.ToString("000.00") + "\t MixTrack History \t" + nextTrack.Description);
            if (nextTrack != null) return nextTrack;

            if (nextTrack == null) nextTrack = GetRandomTrack(currentTrack, direction, null);
            if (nextTrack != null) Debug.Print(nextTrack.BPM.ToString("000.00") + "\t RandomTrack History \t" + nextTrack.Description);
            if (nextTrack != null) return nextTrack;

            if (nextTrack == null) nextTrack = GetClosestTrack(currentTrack, direction);
            if (nextTrack != null) Debug.Print(nextTrack.BPM.ToString("000.00") + "\t ClosestTrack \t" + nextTrack.Description);
            if (nextTrack != null) return nextTrack;

            return nextTrack;
        }

        private Track GetRandomMixTrack(Track currentTrack, Direction direction, List<Track> history)
        {
            if (history == null) history = new List<Track>();

            Track nextTrack = null;
            if (nextTrack == null) nextTrack = GetRandomMixTrack(currentTrack, 5, direction, history);
            if (nextTrack == null) nextTrack = GetRandomMixTrack(currentTrack, 4, direction, history);
            if (nextTrack == null) nextTrack = GetRandomMixTrack(currentTrack, 3, direction, history);
            if (nextTrack == null) nextTrack = GetRandomMixTrack(currentTrack, 5, Direction.Any, history);
            if (nextTrack == null) nextTrack = GetRandomMixTrack(currentTrack, 4, Direction.Any, history);
            if (nextTrack == null) nextTrack = GetRandomMixTrack(currentTrack, 3, Direction.Any, history);
            if (nextTrack == null) nextTrack = GetRandomMixTrack(currentTrack, 2, direction, history);
            if (nextTrack == null) nextTrack = GetRandomMixTrack(currentTrack, 2, Direction.Any, history);
            return nextTrack;
        }

        private Track GetRandomTrack(Track currentTrack, Direction direction, List<Track> history)
        {
            if (history == null) history = new List<Track>();

            Track nextTrack = null;
            if (nextTrack == null) nextTrack = GetRandomTrack(currentTrack, 0M, 2.5M, true, direction, history);
            if (nextTrack == null) nextTrack = GetRandomTrack(currentTrack, 0M, 2.5M, true, Direction.Any, history);
            if (nextTrack == null) nextTrack = GetRandomTrack(currentTrack, 0M, 2.5M, false, direction, history);
            if (nextTrack == null) nextTrack = GetRandomTrack(currentTrack, 0M, 2.5M, false, Direction.Any, history);
            if (nextTrack == null) nextTrack = GetRandomTrack(currentTrack, 2.5M, 5M, true, direction, history);
            if (nextTrack == null) nextTrack = GetRandomTrack(currentTrack, 2.5M, 5M, true, Direction.Any, history);
            if (nextTrack == null) nextTrack = GetRandomTrack(currentTrack, 2.5M, 5M, false, direction, history);
            if (nextTrack == null) nextTrack = GetRandomTrack(currentTrack, 2.5M, 5M, false, Direction.Any, history);

            return nextTrack;
        }

        private Track GetClosestTrack(Track currentTrack, Direction direction)
        {
            if (currentTrack == null) return null;

            var track = this.Library.GetTracks().OrderBy(t => BassHelper.AbsoluteBPMPercentChange(currentTrack.EndBPM, t.StartBPM)).FirstOrDefault();
            if (track != null) return track;

            throw new NotImplementedException();
        }


        private Track GetRandomMixTrack(Track currentTrack, int rank, Direction direction, List<Track> history)
        {
            List<Track> tracks = GetMixTracks(currentTrack, rank);
            if (tracks.Count == 0) return null;

            // filter history/forbidden/wrong direction
            tracks = tracks.Except(history).ToList();
            if (tracks.Count == 0) return null;

            tracks = tracks.Except(GetMixTracks(currentTrack, 0)).ToList();
            if (tracks.Count == 0) return null;

            tracks = FilterTracksByDirection(currentTrack, tracks, direction);
            if (tracks.Count == 0) return null;


            return tracks[_random.Next(0, tracks.Count)];
        }

        private List<Track> FilterTracksByDirection(Track currentTrack, List<Track> tracks, Direction direction)
        {
            var filteredTracks = new List<Track>(tracks);
            if (direction == Direction.Down) return filteredTracks.Where(t => t.BPM < currentTrack.BPM).ToList();
            if (direction == Direction.Up) return filteredTracks.Where(t => t.BPM > currentTrack.BPM).ToList();
            else return filteredTracks;
        }

        private List<Track> GetMixTracks(Track currentTrack, int rank)
        {
            return this.MixLibrary.GetMixableTracks(currentTrack, rank);
        }

        private List<Track> GetTracksInBPMRange(Track currentTrack, decimal minBPM, decimal maxBPM)
        {
            return this.MixLibrary.GetMixableTracks(currentTrack, 1)
                .Where(t => t.BPM >= minBPM && t.BPM < maxBPM)
                .Where(t => t.Description != currentTrack.Description)
                .ToList();
        }

        private List<Track> GetTracksInBPMDiffRange(Track currentTrack, decimal minBPMDiff, decimal maxBPMDiff)
        {
            if (currentTrack == null) return new List<Track>();

            var lowerBPM1 = currentTrack.BPM * (1M - (maxBPMDiff / 100M));
            var lowerBPM2 = currentTrack.BPM * (1M - (minBPMDiff / 100M));

            var upperBPM1 = currentTrack.BPM * (1M + (minBPMDiff / 100M));
            var upperBPM2 = currentTrack.BPM * (1M + (maxBPMDiff / 100M));

            return GetTracksInBPMRange(currentTrack, lowerBPM1, lowerBPM2)
                .Union(GetTracksInBPMRange(currentTrack, upperBPM1, upperBPM2))
                .ToList();
        }


        private Track GetRandomTrack(Track currentTrack, decimal minBPMDiff, decimal maxBPMDiff, bool useSmartAvoid, Direction direction, List<Track> history)
        {
            List<Track> tracks = GetTracksInBPMDiffRange(currentTrack, minBPMDiff, maxBPMDiff);
            tracks = tracks.Except(history).ToList();

            if (useSmartAvoid && currentTrack != null)
            {
                if (currentTrack.Title.ToLower().Contains("instrumental"))
                {
                    tracks = tracks.Where(t => !t.Title.ToLower().Contains("instrumental")).ToList();
                }
                tracks = tracks.Where(t => t.Artist.ToLower() != currentTrack.Artist.ToLower()).ToList();
                tracks = tracks.Where(t => !t.Title.ToLower().StartsWith(currentTrack.Title.ToLower())).ToList();
                tracks = tracks.Where(t => !currentTrack.Title.ToLower().StartsWith(t.Title.ToLower())).ToList();
            }
            if (tracks.Count == 0) return null;

            tracks = FilterTracksByDirection(currentTrack, tracks, direction);
            if (tracks.Count == 0) return null;
            return tracks[_random.Next(0, tracks.Count)];
        }


        private Direction GetPreferredDirection(Track track, List<Track> history)
        {
            if (track == null) return Direction.Up;

            var direction = Direction.Any;

            var fullHistory = new List<Track>(history);
            fullHistory.Add(track);

            var medianBPM = GetMedianBPM(fullHistory);
            if (track.BPM < medianBPM) direction = Direction.Up;
            if (track.BPM > medianBPM) direction = Direction.Down;
            return direction;
        }

        private decimal GetMedianBPM(List<Track> history)
        {
            if (history == null) return 0M;
            history = history.Where(t => t != null).ToList();
            if (history.Count == 0) return 0M;

            var historyLength = history.Sum(t => t.LengthSeconds);
            var libraryLength = this.Library.GetTracks().Sum(t => t.LengthSeconds);

            var available = this.Library.GetTracks().OrderBy(t => t.BPM).ToList();
            if (available.Count == 0) return 0;

            double cycleLength = 60 * 60 * 2;
            if (cycleLength > libraryLength) cycleLength = libraryLength;

            double halfCycle = cycleLength / 2;
            double completedCycles = historyLength / cycleLength;

            var firstTrackBPMPosition = 0;
            for (int i = 0; i < available.Count; i++)
            {
                if (history[0].BPM >= available[i].BPM)
                {
                    firstTrackBPMPosition = i;
                }
            }
            var firsTrackBPMAdjustment = (double)(firstTrackBPMPosition + 1) / (double)available.Count;

            completedCycles += (firsTrackBPMAdjustment / 2);

            double currentCyclePosition = completedCycles - (int)completedCycles;

            double halfCyclePosition = 0F;
            var medianIndex = 0;
            if (currentCyclePosition < 0.5F)
            {
                halfCyclePosition = currentCyclePosition * 2;
                medianIndex = (int)(halfCyclePosition * available.Count);
            }
            else
            {
                halfCyclePosition = (currentCyclePosition - 0.5D) * 2;
                medianIndex = (available.Count - (int)(halfCyclePosition * available.Count)) - 1;
            }

            return available[medianIndex].BPM;
        }

        private List<Track> GetCycleHistory(List<Track> history)
        {
            // get history excluding tracks not in current cycle
            var cycleHistory = new List<Track>();
            for (int i = history.Count - 1; i >= 0; i--)
            {
                cycleHistory.Insert(0, history[i]);
                if (cycleHistory.Sum(t => t.LengthSeconds) > 60 * 60 * 4) break;
            }
            return cycleHistory;
        }

    }
}
