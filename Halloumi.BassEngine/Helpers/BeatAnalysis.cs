using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    /// <summary>
    ///     A tempo and the position of one of its beats: enough to work out where every beat near it falls.
    /// </summary>
    public class BeatGrid
    {
        /// <summary>The tempo, as detected (not normalised to Shuffler's 68–136 range).</summary>
        public double Bpm { get; set; }

        /// <summary>The position of one beat, in seconds.</summary>
        public double Anchor { get; set; }

        /// <summary>How much the beats stand out from the audio around them (about 1 = no beat found).</summary>
        public double Confidence { get; set; }

        public double BeatLength
        {
            get { return 60.0 / Bpm; }
        }

        /// <summary>The beat closest to the position.</summary>
        public double Snap(double seconds)
        {
            return Anchor + Math.Round((seconds - Anchor) / BeatLength) * BeatLength;
        }

        /// <summary>
        ///     The beat or half-beat closest to the position. A detected grid is sometimes half a beat out
        ///     (it has locked on to the off-beats), so snapping to half-beats always includes the real beats.
        /// </summary>
        public double SnapToHalfBeat(double seconds)
        {
            var half = BeatLength / 2;
            return Anchor + Math.Round((seconds - Anchor) / half) * half;
        }
    }

    /// <summary>
    ///     A suggested position for a fade section, in seconds.
    /// </summary>
    public class FadeCandidate
    {
        public double Start { get; set; }
        public double Length { get; set; }

        /// <summary>How well it fits: only meaningful for comparing candidates of the same track.</summary>
        public double Score { get; set; }
    }

    /// <summary>
    ///     Where fade sections usually go, worked out from tracks that have already been set up (normally
    ///     those of the same genre). Steers the suggestions for a new track.
    /// </summary>
    public class FadeProfile
    {
        public FadeProfile()
        {
            // what the whole library looked like when this was written; used when there is nothing to go on
            TypicalFadeInStart = 5.6;
            FadeInSpread = 1.26;
            TypicalPlayedLength = 140;
        }

        /// <summary>The usual start of the fade-in, in seconds (the median).</summary>
        public double TypicalFadeInStart { get; set; }

        /// <summary>How much the start of the fade-in varies (standard deviation of ln(seconds + 2)).</summary>
        public double FadeInSpread { get; set; }

        /// <summary>The usual length played: start of the fade-in to end of the fade-out, less any skip section.</summary>
        public double TypicalPlayedLength { get; set; }

        /// <summary>How many tracks this was worked out from (0 = the built-in defaults).</summary>
        public int TrackCount { get; set; }

        /// <summary>
        ///     Works out a profile from fade sections that have already been set.
        /// </summary>
        /// <param name="fadeInStarts">The start of each track's fade-in, in seconds.</param>
        /// <param name="playedLengths">Each track's played length, in seconds.</param>
        public static FadeProfile Create(IList<double> fadeInStarts, IList<double> playedLengths)
        {
            var profile = new FadeProfile();
            if (fadeInStarts == null || fadeInStarts.Count == 0) return profile;

            var logs = new List<double>();
            foreach (var start in fadeInStarts) logs.Add(Math.Log(Math.Max(0, start) + 2));
            logs.Sort();

            var mean = 0.0;
            foreach (var value in logs) mean += value;
            mean /= logs.Count;
            var variance = 0.0;
            foreach (var value in logs) variance += (value - mean) * (value - mean);

            profile.TypicalFadeInStart = Math.Exp(logs[logs.Count / 2]) - 2;
            profile.FadeInSpread = Math.Max(0.6, Math.Sqrt(variance / logs.Count));
            profile.TrackCount = logs.Count;

            if (playedLengths != null && playedLengths.Count > 0)
            {
                var sorted = new List<double>(playedLengths);
                sorted.Sort();
                profile.TypicalPlayedLength = sorted[sorted.Count / 2];
            }

            return profile;
        }
    }

    /// <summary>
    ///     Beat analysis of an audio file (or a range of one): tempo detection, beat positions for snapping,
    ///     and suggested fade sections.
    ///     Adapted from TalentScout's BpmDetector: spectral-flux onset envelope → autocorrelation → comb-filter
    ///     tempo scoring with a mild prior around 110 BPM → fine tuning by fitting a beat grid. The grid fit also
    ///     gives the position of the beats, which is what snapping uses. Never writes to the file's tags.
    /// </summary>
    public class BeatAnalysis
    {
        public const double MinBpm = 70;
        public const double MaxBpm = 180;

        private const int FftSize = 1024;
        private const double FramesPerSecond = 100;

        /// <summary>
        ///     How far into an analysis frame a drum hit sits when the onset envelope peaks, as a fraction of
        ///     the frame (measured against synthetic click tracks).
        /// </summary>
        private const double OnsetLatencyFrames = 0.62;

        private const double GridWindowSeconds = 40;
        private const double GridBucketSeconds = 5;

        /// <summary>Below this a grid is treated as "no beat here" when walking through the track.</summary>
        private const double MinStepConfidence = 1.6;

        /// <summary>Below this the beat is too weak to snap to (free-time or loosely played music).</summary>
        public const double MinSnapConfidence = 2.5;

        private readonly Dictionary<int, BeatGrid> _gridCache = new Dictionary<int, BeatGrid>();

        private double[] _envelope; // full-band onset strength, one value per frame
        private double[] _lowEnvelope; // onset strength of the bass only
        private double[] _phaseEnvelope; // full-band + low-band, used to place the beats
        private double[] _level; // loudness (RMS) of each frame
        private double[] _lowLevel; // how much bass (under about 150 Hz) is in each frame
        private double _startTime; // position of frame 0 in the file
        private double _latency; // seconds from the start of a frame to the onset it reports
        private CancellationToken _cancellation = CancellationToken.None;

        private BeatAnalysis()
        {
        }

        /// <summary>The first analysed position, in seconds.</summary>
        public double Start
        {
            get { return _startTime; }
        }

        /// <summary>The last analysed position, in seconds.</summary>
        public double End
        {
            get { return _startTime + _envelope.Length / FramesPerSecond; }
        }

        /// <summary>
        ///     Analyses a whole file. Takes a few seconds for a full track, so call it from a background thread.
        /// </summary>
        /// <returns>The analysis, or null if the file could not be read or is too short.</returns>
        public static BeatAnalysis Analyse(string filename)
        {
            return Analyse(filename, 0, double.MaxValue);
        }

        /// <summary>
        ///     Analyses a whole file, giving up (and returning null) if cancellation is requested.
        /// </summary>
        public static BeatAnalysis Analyse(string filename, CancellationToken cancellation)
        {
            var analysis = new BeatAnalysis { _cancellation = cancellation };
            return analysis.Load(filename, 0, double.MaxValue) ? analysis : null;
        }

        /// <summary>
        ///     Analyses a range of a file.
        /// </summary>
        /// <returns>The analysis, or null if the file could not be read or the range is under about 5 seconds.</returns>
        public static BeatAnalysis Analyse(string filename, double startSeconds, double endSeconds)
        {
            var analysis = new BeatAnalysis();
            return analysis.Load(filename, startSeconds, endSeconds) ? analysis : null;
        }

        /// <summary>
        ///     Detects the tempo of a range of a file. Short ranges give unreliable results: use 20 seconds or more.
        /// </summary>
        /// <returns>The detected BPM (70–180), or 0 if detection failed.</returns>
        public static double DetectBpm(string filename, double startSeconds, double endSeconds)
        {
            var analysis = Analyse(filename, startSeconds, endSeconds);
            return analysis == null ? 0 : analysis.DetectBpm(analysis.Start, analysis.End);
        }

        /// <summary>
        ///     Detects the tempo of a range.
        /// </summary>
        /// <returns>The detected BPM (70–180), or 0 if detection failed.</returns>
        public double DetectBpm(double startSeconds, double endSeconds)
        {
            var grid = GetGrid(startSeconds, endSeconds, 0);
            return grid == null ? 0 : Math.Round(grid.Bpm, 2);
        }

        /// <summary>
        ///     Fits a beat grid to a range.
        /// </summary>
        /// <param name="startSeconds">The start of the range.</param>
        /// <param name="endSeconds">The end of the range.</param>
        /// <param name="bpmHint">
        ///     If given, the tempo is only searched for within 4% of this value (keeps the same beat when walking
        ///     through a track); otherwise it is detected from scratch.
        /// </param>
        /// <returns>The grid, or null if the range is too short or has no detectable tempo.</returns>
        public BeatGrid GetGrid(double startSeconds, double endSeconds, double bpmHint)
        {
            var first = Math.Max(0, (int) Math.Round((startSeconds - _startTime) * FramesPerSecond));
            var last = Math.Min(_envelope.Length, (int) Math.Round((endSeconds - _startTime) * FramesPerSecond));
            var count = last - first;
            if (count < 5 * FramesPerSecond) return null;

            var bpm = bpmHint;
            if (bpm <= 0)
            {
                var segment = new double[count];
                Array.Copy(_envelope, first, segment, 0, count);
                bpm = EstimateTempo(segment);
            }
            if (bpm <= 0) return null;

            var phaseSegment = new double[count];
            Array.Copy(_phaseEnvelope, first, phaseSegment, 0, count);

            var mean = 0.0;
            foreach (var value in phaseSegment) mean += value;
            mean /= count;
            if (mean <= 0) return null;

            // the tempo whose best-aligned beat grid collects the most onset energy
            var spread = bpmHint > 0 ? 0.04 : 0.015;
            var step = bpmHint > 0 ? 0.02 : 0.01;
            var bestBpm = bpm;
            var bestPhase = 0.0;
            var bestScore = double.MinValue;

            for (var candidate = bpm * (1 - spread); candidate <= bpm * (1 + spread); candidate += step)
            {
                var period = 60 * FramesPerSecond / candidate;
                for (var phase = 0.0; phase < period; phase += 1)
                {
                    var score = GridScore(phaseSegment, period, phase);
                    if (score <= bestScore) continue;
                    bestScore = score;
                    bestBpm = candidate;
                    bestPhase = phase;
                }
            }

            // place the beats more finely than one frame
            var bestPeriod = 60 * FramesPerSecond / bestBpm;
            var finePhase = bestPhase;
            for (var phase = bestPhase - 1; phase <= bestPhase + 1; phase += 0.05)
            {
                var score = GridScore(phaseSegment, bestPeriod, phase);
                if (score <= bestScore) continue;
                bestScore = score;
                finePhase = phase;
            }

            return new BeatGrid
            {
                Bpm = bestBpm,
                Anchor = _startTime + (first + finePhase) / FramesPerSecond + _latency,
                Confidence = bestScore / mean
            };
        }

        /// <summary>
        ///     Gets the beat grid of the audio around a position (results are cached).
        /// </summary>
        /// <returns>The grid, or null if there is no detectable tempo there.</returns>
        public BeatGrid GetGridAt(double seconds)
        {
            var bucket = (int) Math.Floor(seconds / GridBucketSeconds);

            lock (_gridCache)
            {
                BeatGrid cached;
                if (_gridCache.TryGetValue(bucket, out cached)) return cached;
            }

            var middle = (bucket + 0.5) * GridBucketSeconds;
            var start = middle - GridWindowSeconds / 2;
            var end = middle + GridWindowSeconds / 2;

            // keep the window full-sized at the ends of the track
            if (start < Start)
            {
                end += Start - start;
                start = Start;
            }
            if (end > End)
            {
                start = Math.Max(Start, start - (end - End));
                end = End;
            }

            var grid = GetGrid(start, end, 0);

            lock (_gridCache)
            {
                _gridCache[bucket] = grid;
            }
            return grid;
        }

        /// <summary>
        ///     Moves a position to the nearest beat or half-beat of the audio around it.
        /// </summary>
        /// <returns>The snapped position, or the position unchanged if there is no clear beat there.</returns>
        public double SnapToBeat(double seconds)
        {
            var grid = GetGridAt(seconds);
            if (grid == null || grid.Confidence < MinSnapConfidence) return seconds;

            var snapped = grid.SnapToHalfBeat(seconds);
            if (snapped < 0) snapped += grid.BeatLength / 2;
            return snapped;
        }

        /// <summary>
        ///     Moves the end of a section so that the section is a whole number of beats long (at least one),
        ///     at the tempo of the audio it covers.
        /// </summary>
        /// <returns>The new end position, or the end unchanged if no tempo was found.</returns>
        public double SnapEndToBeat(double startSeconds, double endSeconds)
        {
            var grid = GetGridAt((startSeconds + endSeconds) / 2);
            if (grid == null || grid.Confidence < MinSnapConfidence) return endSeconds;

            var beats = Math.Max(1, Math.Round((endSeconds - startSeconds) / grid.BeatLength));
            return startSeconds + beats * grid.BeatLength;
        }

        /// <summary>
        ///     Suggests where the fade-in could start, best first. They are starting points to audition, not
        ///     answers.
        ///     A candidate is a place where the track gets louder and gains bass (measured over 2 seconds either
        ///     side), favouring where fade-ins usually are for this kind of track, then moved to the sharpest
        ///     jump just after it (the first hit of the new part). Each is 16 beats long at the tempo there.
        ///     How well this does against hand-set fades is measured by tools/fade-eval (see
        ///     docs/fade-suggestions.md): check there before and after changing it.
        /// </summary>
        public List<FadeCandidate> GetFadeInCandidates(FadeProfile profile, int count)
        {
            var candidates = new List<FadeCandidate>();
            if (profile == null) profile = new FadeProfile();

            var frames = _level.Length;
            if (frames < 10 * FramesPerSecond) return candidates;

            var levelFloor = Percentile(_level, 0.9);
            var lowFloor = Percentile(_lowLevel, 0.9);

            // how much louder and bassier the 2 seconds after each frame are than the 2 seconds before
            const int longWindow = 200;
            var score = Rise(_level, longWindow, levelFloor * 0.02 + 1e-9);
            var lowRise = Rise(_lowLevel, longWindow, lowFloor * 0.02 + 1e-9);

            var centre = Math.Log(Math.Max(0, profile.TypicalFadeInStart) + 2);
            var spread = Math.Max(0.6, profile.FadeInSpread);
            var lastFrame = (int) (frames * 0.7); // a fade-in isn't in the last part of a track

            for (var i = 0; i < frames; i++)
            {
                if (i >= lastFrame)
                {
                    score[i] = double.NegativeInfinity;
                    continue;
                }

                var distance = (Math.Log(_startTime + i / FramesPerSecond + _latency + 2) - centre) / spread;
                score[i] += lowRise[i] - distance * distance;
            }

            // the same, over 0.3 seconds: sharp enough to find the hit itself
            const int shortWindow = 30;
            var sharp = Rise(_level, shortWindow, levelFloor * 0.05 + 1e-9);
            var sharpLow = Rise(_lowLevel, shortWindow, lowFloor * 0.05 + 1e-9);
            for (var i = 0; i < frames; i++) sharp[i] += sharpLow[i];

            var chosen = new List<int>();
            foreach (var peak in Peaks(score, 150, count + 2))
            {
                // the long measure peaks a little early: the hit is the sharpest jump in the 2.5 seconds after
                var from = Math.Max(0, peak - 30);
                var to = Math.Min(frames - 1, peak + 250);
                var best = from;
                for (var i = from; i < to; i++)
                {
                    if (sharp[i] > sharp[best]) best = i;
                }

                var duplicate = false;
                foreach (var other in chosen)
                {
                    if (Math.Abs(other - best) <= 40) duplicate = true;
                }
                if (duplicate) continue;

                chosen.Add(best);
                var start = Math.Max(0, _startTime + best / FramesPerSecond + _latency + CandidateOffset);
                candidates.Add(new FadeCandidate
                {
                    Start = start,
                    Length = SectionLengthAt(start),
                    Score = score[peak]
                });
                if (candidates.Count >= count) break;
            }

            return candidates;
        }

        /// <summary>
        ///     Suggests where the fade-out could start for a given fade-in, best first: a whole number of
        ///     fade-in lengths after the fade-in (following the beat, so that tempo drift doesn't build up),
        ///     the ones that make the track play for closest to the usual length first.
        /// </summary>
        public List<FadeCandidate> GetFadeOutCandidates(double fadeInStart, double fadeInLength, FadeProfile profile,
            int count)
        {
            var candidates = new List<FadeCandidate>();
            if (profile == null) profile = new FadeProfile();
            if (fadeInLength <= 0) return candidates;

            var grid = GetGrid(fadeInStart - 1, fadeInStart + GridWindowSeconds, 0);
            if (grid == null) return candidates;

            var beats = (int) Math.Max(1, Math.Round(fadeInLength / grid.BeatLength));

            int firstOnset, lastOnset;
            var lastTime = FindOnsetRange(out firstOnset, out lastOnset)
                ? _startTime + lastOnset / FramesPerSecond + _latency
                : End;

            var target = fadeInStart + profile.TypicalPlayedLength;
            var position = fadeInStart;
            var bpm = grid.Bpm;
            var phrases = 0;
            while (phrases < 2000)
            {
                var next = StepBeats(position, beats, ref bpm);
                var length = beats * 60.0 / bpm;
                if (next <= position || next + length > lastTime + 60.0 / bpm) break;

                position = next;
                phrases++;
                if (phrases < 2) continue;

                candidates.Add(new FadeCandidate
                {
                    Start = position,
                    Length = length,
                    Score = -Math.Abs(position + length - target)
                });
            }

            candidates.Sort((x, y) => y.Score.CompareTo(x.Score));
            if (candidates.Count > count) candidates.RemoveRange(count, candidates.Count - count);
            return candidates;
        }

        /// <summary>
        ///     How much later a hand-set fade point is than the jump in loudness it sits on (measured: 8 ms).
        /// </summary>
        private const double CandidateOffset = 0.008;

        /// <summary>
        ///     The length of a 16-beat section starting at a position, at the tempo of the audio there
        ///     (10 seconds if there is no tempo to be found).
        /// </summary>
        private double SectionLengthAt(double start)
        {
            var grid = GetGrid(start - 1, start + GridWindowSeconds, 0);
            if (grid == null) return BpmHelper.GetDefaultLoopLength(0);

            var beats = Math.Max(1, Math.Round(PhraseLength(grid.Bpm) / grid.BeatLength));
            return beats * grid.BeatLength;
        }

        /// <summary>
        ///     For each frame: log of the average of the following frames over the average of the preceding ones.
        /// </summary>
        private static double[] Rise(double[] values, int window, double floor)
        {
            var count = values.Length;
            var sums = new double[count + 1];
            for (var i = 0; i < count; i++) sums[i + 1] = sums[i] + values[i];

            var rise = new double[count];
            for (var i = 0; i < count; i++)
            {
                var to = Math.Min(count, i + window);
                var from = Math.Max(0, i - window);
                var after = (sums[to] - sums[i]) / Math.Max(1, to - i);
                var before = (sums[i] - sums[from]) / Math.Max(1, i - from);
                rise[i] = Math.Log(after + floor) - Math.Log(before + floor);
            }
            return rise;
        }

        private static double Percentile(double[] values, double fraction)
        {
            var sorted = (double[]) values.Clone();
            Array.Sort(sorted);
            return sorted[Math.Min(sorted.Length - 1, (int) (sorted.Length * fraction))];
        }

        /// <summary>The highest values that are at least a gap apart, best first.</summary>
        private static List<int> Peaks(double[] score, int gap, int count)
        {
            var order = new int[score.Length];
            for (var i = 0; i < order.Length; i++) order[i] = i;
            var keys = new double[score.Length];
            for (var i = 0; i < keys.Length; i++) keys[i] = -score[i];
            Array.Sort(keys, order);

            var chosen = new List<int>();
            foreach (var index in order)
            {
                if (chosen.Count >= count || double.IsNegativeInfinity(score[index])) break;

                var clear = true;
                foreach (var other in chosen)
                {
                    if (Math.Abs(other - index) < gap) clear = false;
                }
                if (clear) chosen.Add(index);
            }
            return chosen;
        }

        /// <summary>
        ///     The length in seconds of a 16-beat section at the tempo, once normalised the way Shuffler does.
        /// </summary>
        private static double PhraseLength(double bpm)
        {
            return BpmHelper.GetDefaultLoopLength((decimal) bpm);
        }

        /// <summary>
        ///     Moves forward a number of beats from a position that is on a beat, following the beat of the audio.
        /// </summary>
        private double StepBeats(double position, int beats, ref double bpm)
        {
            var length = beats * 60.0 / bpm;
            var grid = GetGrid(position - 1, position + Math.Max(20, length + 10), bpm);

            if (grid == null || grid.Confidence < MinStepConfidence)
                return position + length; // no clear beat here (a breakdown): keep counting at the last tempo

            bpm = grid.Bpm;
            return grid.SnapToHalfBeat(position + beats * grid.BeatLength);
        }

        /// <summary>
        ///     Finds the first and last frames of the part of the track that has a beat.
        /// </summary>
        private bool FindOnsetRange(out int first, out int last)
        {
            first = last = -1;

            var sorted = new List<double>();
            foreach (var value in _envelope)
            {
                if (value > 0) sorted.Add(value);
            }
            if (sorted.Count < 100) return false;
            sorted.Sort();

            var threshold = 0.3 * sorted[(int) (sorted.Count * 0.95)];
            const int window = (int) (4 * FramesPerSecond);

            // a hit only counts if more follow (or precede) it: skips a stray click before the music starts
            for (var i = 0; i < _envelope.Length; i++)
            {
                if (_envelope[i] <= threshold || CountOnsets(i, i + window, threshold) < 4) continue;
                first = i;
                break;
            }

            for (var i = _envelope.Length - 1; i >= 0; i--)
            {
                if (_envelope[i] <= threshold || CountOnsets(i - window, i, threshold) < 4) continue;
                last = i;
                break;
            }

            return first >= 0 && last > first;
        }

        private int CountOnsets(int from, int to, double threshold)
        {
            from = Math.Max(1, from);
            to = Math.Min(_envelope.Length - 2, to);

            var count = 0;
            for (var i = from; i <= to; i++)
            {
                if (_envelope[i] > threshold && _envelope[i] >= _envelope[i - 1] && _envelope[i] > _envelope[i + 1])
                    count++;
            }
            return count;
        }

        /// <summary>The average onset strength at the beats of a grid.</summary>
        private static double GridScore(double[] envelope, double period, double phase)
        {
            var sum = 0.0;
            var count = 0;
            for (var t = phase; t < envelope.Length - 1; t += period)
            {
                if (t < 0) continue;
                sum += Interpolate(envelope, t);
                count++;
            }
            return count == 0 ? double.MinValue : sum / count;
        }

        /// <summary>Coarse tempo: comb-filter score on the autocorrelation, weighted by a tempo prior.</summary>
        private static double EstimateTempo(double[] envelope)
        {
            var maxLag = (int) Math.Min(envelope.Length / 2, 4 * 60 * FramesPerSecond / MinBpm + 2);
            var autocorrelation = Autocorrelate(envelope, maxLag);

            var best = 0.0;
            var bestScore = double.MinValue;
            for (var bpm = MinBpm; bpm <= MaxBpm; bpm += 0.05)
            {
                var period = 60 * FramesPerSecond / bpm;
                var score = 0.0;
                for (var k = 1; k <= 4; k++)
                {
                    if (k * period >= maxLag - 1) break;
                    score += Interpolate(autocorrelation, k * period);
                }

                var octaves = Math.Log(bpm / 110.0, 2) / 0.9;
                score *= Math.Exp(-0.5 * octaves * octaves);

                if (score <= bestScore) continue;
                bestScore = score;
                best = bpm;
            }

            return bestScore > 0 ? best : 0;
        }

        /// <summary>
        ///     Decodes the range and builds the onset envelopes, a frame at a time so that a whole track is
        ///     never held in memory.
        /// </summary>
        private bool Load(string filename, double start, double end)
        {
            var stream = Bass.BASS_StreamCreateFile(filename, 0, 0,
                BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);
            if (stream == 0) return false;

            try
            {
                var info = Bass.BASS_ChannelGetInfo(stream);
                var sampleRate = info.freq;
                var channels = Math.Max(1, info.chans);
                if (sampleRate <= 0) return false;

                var length = Bass.BASS_ChannelBytes2Seconds(stream,
                    Bass.BASS_ChannelGetLength(stream, BASSMode.BASS_POS_BYTE));
                start = Math.Max(0, start);
                end = Math.Min(length, end);
                if (end - start < 5) return false;

                Bass.BASS_ChannelSetPosition(stream, Bass.BASS_ChannelSeconds2Bytes(stream, start),
                    BASSMode.BASS_POS_BYTE);

                var hop = sampleRate / FramesPerSecond;
                var totalSamples = (long) ((end - start) * sampleRate);
                var frameCount = (int) ((totalSamples - FftSize) / hop);
                if (frameCount < 5 * FramesPerSecond) return false;

                var window = new double[FftSize];
                for (var i = 0; i < FftSize; i++)
                    window[i] = 0.5 - 0.5 * Math.Cos(2 * Math.PI * i / (FftSize - 1));

                var maxBin = Math.Min(FftSize / 2, (int) (8000.0 * FftSize / sampleRate));
                var lowBin = Math.Min(maxBin, Math.Max(2, (int) (150.0 * FftSize / sampleRate)));
                var re = new double[FftSize];
                var im = new double[FftSize];
                var previous = new double[maxBin + 1];
                var flux = new double[frameCount];
                var lowFlux = new double[frameCount];
                var level = new double[frameCount];
                var lowLevel = new double[frameCount];

                // a sliding window of mono samples over the decoded stream
                const int chunkFrames = 8192;
                var decoded = new float[chunkFrames * channels];
                var mono = new float[FftSize + chunkFrames * 2];
                long monoStart = 0; // position in the range of mono[0]
                var monoCount = 0;
                var finished = false;

                for (var f = 0; f < frameCount; f++)
                {
                    if (_cancellation.IsCancellationRequested) return false;

                    var offset = (long) (f * hop);

                    // drop what is behind this frame, then read until the frame is covered
                    var drop = (int) Math.Min(monoCount, offset - monoStart);
                    if (drop > 0)
                    {
                        Array.Copy(mono, drop, mono, 0, monoCount - drop);
                        monoStart += drop;
                        monoCount -= drop;
                    }

                    while (!finished && monoStart + monoCount < offset + FftSize)
                    {
                        var bytes = Bass.BASS_ChannelGetData(stream, decoded, chunkFrames * channels * 4);
                        if (bytes <= 0)
                        {
                            finished = true;
                            break;
                        }

                        var frames = bytes / 4 / channels;
                        for (var i = 0; i < frames; i++)
                        {
                            var sum = 0f;
                            for (var c = 0; c < channels; c++) sum += decoded[i * channels + c];
                            mono[monoCount + i] = sum / channels;
                        }
                        monoCount += frames;
                    }

                    if (monoStart + monoCount < offset + FftSize)
                    {
                        // the file ended early
                        frameCount = f;
                        break;
                    }

                    var index = (int) (offset - monoStart);
                    var squares = 0.0;
                    for (var i = 0; i < FftSize; i++)
                    {
                        var sample = mono[index + i];
                        squares += sample * sample;
                        re[i] = sample * window[i];
                        im[i] = 0;
                    }
                    level[f] = Math.Sqrt(squares / FftSize);

                    Fft(re, im);

                    var bass = 0.0;
                    for (var k = 1; k <= lowBin; k++) bass += re[k] * re[k] + im[k] * im[k];
                    lowLevel[f] = Math.Sqrt(bass) / FftSize;

                    var total = 0.0;
                    var low = 0.0;
                    for (var k = 1; k <= maxBin; k++)
                    {
                        var magnitude = Math.Log(1 + 100 * Math.Sqrt(re[k] * re[k] + im[k] * im[k]));
                        var rise = magnitude - previous[k];
                        if (rise > 0 && f > 0)
                        {
                            total += rise;
                            if (k <= lowBin) low += rise;
                        }
                        previous[k] = magnitude;
                    }
                    flux[f] = total;
                    lowFlux[f] = low;
                }

                if (frameCount < 5 * FramesPerSecond) return false;
                if (frameCount < flux.Length)
                {
                    Array.Resize(ref flux, frameCount);
                    Array.Resize(ref lowFlux, frameCount);
                    Array.Resize(ref level, frameCount);
                    Array.Resize(ref lowLevel, frameCount);
                }

                _startTime = start;
                _latency = OnsetLatencyFrames * FftSize / sampleRate;
                _level = level;
                _lowLevel = lowLevel;
                SetEnvelopes(RemoveLocalMean(flux), RemoveLocalMean(lowFlux));

                return true;
            }
            finally
            {
                Bass.BASS_StreamFree(stream);
            }
        }

        /// <summary>
        ///     The beats are placed using the kick as well as everything else, so that a busy hi-hat pattern
        ///     doesn't pull the grid onto the off-beats.
        /// </summary>
        private void SetEnvelopes(double[] envelope, double[] lowEnvelope)
        {
            _envelope = envelope;
            _lowEnvelope = lowEnvelope;

            var mean = Mean(envelope);
            var lowMean = Mean(lowEnvelope);
            _phaseEnvelope = new double[envelope.Length];
            for (var i = 0; i < envelope.Length; i++)
            {
                _phaseEnvelope[i] = (mean > 0 ? envelope[i] / mean : 0)
                                    + (lowMean > 0 ? lowEnvelope[i] / lowMean : 0);
            }
        }

        /// <summary>
        ///     Saves the analysis to a file, so that it can be loaded again without decoding the audio
        ///     (used by the fade suggestion test harness, which analyses the same tracks over and over).
        /// </summary>
        public void Save(string filename)
        {
            using (var writer = new BinaryWriter(File.Create(filename)))
            {
                writer.Write(CacheMagic);
                writer.Write(_envelope.Length);
                writer.Write(_startTime);
                writer.Write(_latency);
                WriteArray(writer, _envelope);
                WriteArray(writer, _lowEnvelope);
                WriteArray(writer, _level);
                WriteArray(writer, _lowLevel);
            }
        }

        /// <summary>
        ///     Loads an analysis saved with <see cref="Save" />.
        /// </summary>
        /// <returns>The analysis, or null if the file isn't a saved analysis.</returns>
        public static BeatAnalysis Load(string filename)
        {
            if (!File.Exists(filename)) return null;

            using (var reader = new BinaryReader(File.OpenRead(filename)))
            {
                if (reader.ReadInt32() != CacheMagic) return null;

                var count = reader.ReadInt32();
                var analysis = new BeatAnalysis
                {
                    _startTime = reader.ReadDouble(),
                    _latency = reader.ReadDouble()
                };
                var envelope = ReadArray(reader, count);
                var lowEnvelope = ReadArray(reader, count);
                analysis._level = ReadArray(reader, count);
                analysis._lowLevel = ReadArray(reader, count);
                analysis.SetEnvelopes(envelope, lowEnvelope);
                return analysis;
            }
        }

        private const int CacheMagic = 0x31544142; // "BAT1"

        private static void WriteArray(BinaryWriter writer, double[] values)
        {
            foreach (var value in values) writer.Write((float) value);
        }

        private static double[] ReadArray(BinaryReader reader, int count)
        {
            var values = new double[count];
            for (var i = 0; i < count; i++) values[i] = reader.ReadSingle();
            return values;
        }

        private static double Mean(double[] values)
        {
            var sum = 0.0;
            foreach (var value in values) sum += value;
            return values.Length == 0 ? 0 : sum / values.Length;
        }

        /// <summary>Subtracts a moving average (±0.25s) and half-wave rectifies.</summary>
        private static double[] RemoveLocalMean(double[] flux)
        {
            const int radius = 25;
            var count = flux.Length;
            var envelope = new double[count];
            var running = 0.0;
            var lo = 0;
            var hi = -1;
            for (var i = 0; i < count; i++)
            {
                var newHi = Math.Min(count - 1, i + radius);
                while (hi < newHi) running += flux[++hi];
                var newLo = Math.Max(0, i - radius);
                while (lo < newLo) running -= flux[lo++];
                var mean = running / (hi - lo + 1);
                envelope[i] = Math.Max(0, flux[i] - mean);
            }
            return envelope;
        }

        private static double[] Autocorrelate(double[] x, int maxLag)
        {
            var result = new double[maxLag];
            for (var lag = 0; lag < maxLag; lag++)
            {
                var sum = 0.0;
                for (var i = 0; i + lag < x.Length; i++) sum += x[i] * x[i + lag];
                result[lag] = sum / (x.Length - lag);
            }
            return result;
        }

        private static double Interpolate(double[] values, double position)
        {
            var i = (int) position;
            if (position < 0 || i >= values.Length - 1) return 0;
            var fraction = position - i;
            return values[i] * (1 - fraction) + values[i + 1] * fraction;
        }

        /// <summary>In-place iterative radix-2 FFT.</summary>
        private static void Fft(double[] re, double[] im)
        {
            var n = re.Length;
            for (int i = 1, j = 0; i < n; i++)
            {
                var bit = n >> 1;
                for (; (j & bit) != 0; bit >>= 1) j ^= bit;
                j ^= bit;
                if (i >= j) continue;

                var t = re[i];
                re[i] = re[j];
                re[j] = t;
                t = im[i];
                im[i] = im[j];
                im[j] = t;
            }

            for (var length = 2; length <= n; length <<= 1)
            {
                var angle = -2 * Math.PI / length;
                var wRe = Math.Cos(angle);
                var wIm = Math.Sin(angle);
                var half = length / 2;
                for (var i = 0; i < n; i += length)
                {
                    double curRe = 1, curIm = 0;
                    for (var j = 0; j < half; j++)
                    {
                        var aRe = re[i + j];
                        var aIm = im[i + j];
                        var bRe = re[i + j + half] * curRe - im[i + j + half] * curIm;
                        var bIm = re[i + j + half] * curIm + im[i + j + half] * curRe;
                        re[i + j] = aRe + bRe;
                        im[i + j] = aIm + bIm;
                        re[i + j + half] = aRe - bRe;
                        im[i + j + half] = aIm - bIm;
                        var nextRe = curRe * wRe - curIm * wIm;
                        curIm = curRe * wIm + curIm * wRe;
                        curRe = nextRe;
                    }
                }
            }
        }
    }
}
