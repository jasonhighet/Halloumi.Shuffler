using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioEngine.Helpers;

namespace Halloumi.Shuffler.AudioEngine.Models
{
    /// <summary>
    ///     Represents a playable mp3 file
    /// </summary>
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class Track : AudioStream
    {
        /// <summary>
        ///     The BPM at the end of the song
        /// </summary>
        private decimal _endBpm = 100;

        /// <summary>
        ///     The BPM at the start of the song
        /// </summary>
        private decimal _startBpm = 100;

        /// <summary>
        ///     Initializes a new instance of the track class.
        /// </summary>
        public Track()
        {
            Syncs = new TrackSyncs(this);
            UsePreFadeIn = false;
            PreFadeInStart = 0;
            PreFadeInStartVolume = 0;
            BpmAdjustmentRatio = 1;
            StartLoopCount = 0;
            CurrentStartLoop = 0;
            EndLoopCount = 0;
            CurrentEndLoop = 0;
            TagDataLoaded = false;
            FadeInEnd = 0;
            FadeInStart = 0;
            FadeOutEnd = 0;
            FadeOutStart = 0;
            PowerDownOnEnd = false;
            PowerDownOnEndOriginal = false;
            RawLoopStart = 0;
            RawLoopEnd = 0;
            SkipStart = 0;
            SkipEnd = 0;

            Rank = 1;
        }

        /// <summary>
        ///     Gets or sets the track title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the track artist.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        ///     Gets a description of the track
        /// </summary>
        public override string Description
        {
            get { return Artist + " - " + Title; }
            set { }
        }

        /// <summary>
        ///     Gets the BPM at the start of the track
        /// </summary>
        public decimal StartBpm
        {
            get
            {
                if (FadeInLengthSeconds != 0)
                    return BpmHelper.GetBpmFromLoopLength(FadeInLengthSeconds);

                if (!TagDataLoaded)
                    return _startBpm;

                return TagBpm*BpmAdjustmentRatio;
            }
            internal set { _startBpm = value; }
        }

        /// <summary>
        ///     Gets the BPM at the end of the track
        /// </summary>
        public decimal EndBpm
        {
            get
            {
                if (FadeOutLengthSeconds != 0)
                    return BpmHelper.GetBpmFromLoopLength(FadeOutLengthSeconds);

                if (!TagDataLoaded)
                    return _endBpm;

                return TagBpm*BpmAdjustmentRatio;
            }
            internal set { _endBpm = value; }
        }

        public override decimal Bpm
        {
            get { return BpmHelper.GetAdjustedBpmAverage(StartBpm, EndBpm); }
            set { }
        }

        /// <summary>
        ///     Gets or sets the start of the fade-in section as a sample position
        /// </summary>
        public long FadeInStart { get; set; }

        /// <summary>
        ///     Gets or sets the end of the fade-in section as a sample position
        /// </summary>
        public long FadeInEnd { get; set; }

        /// <summary>
        ///     Gets the length of the fade-in section in samples.
        /// </summary>
        public long FadeInLength
        {
            get { return FadeInEnd - FadeInStart; }
        }

        /// <summary>
        ///     Gets the length of the fade-in section in seconds
        /// </summary>
        public double FadeInLengthSeconds
        {
            get { return SamplesToSeconds(FadeInLength); }
        }

        /// <summary>
        ///     Gets or sets the initial volume level at the start of the fade-in section as a percentage (0 - 1)
        /// </summary>
        public float FadeInStartVolume { get; set; }

        /// <summary>
        ///     Gets or sets the final volume level at the end of the fade-in section as a percentage (0 - 1)
        /// </summary>
        public float FadeInEndVolume { get; set; }

        /// <summary>
        ///     Gets or sets the start of the fade-out section as a sample position
        /// </summary>
        public long FadeOutStart { get; set; }

        /// <summary>
        ///     Gets or sets the end of the fade-out section as a sample position
        /// </summary>
        public long FadeOutEnd { get; set; }

        /// <summary>
        ///     Gets the length of the fade-out section in samples.
        /// </summary>
        public long FadeOutLength
        {
            get { return FadeOutEnd - FadeOutStart; }
        }

        /// <summary>
        ///     Gets the length of the fade-out section in seconds
        /// </summary>
        public double FadeOutLengthSeconds
        {
            get { return SamplesToSeconds(FadeOutLength); }
        }

        /// <summary>
        ///     Gets or sets the initial volume level at the start of the fade-out section as a percentage (0 - 1)
        /// </summary>
        public float FadeOutStartVolume { get; set; }

        /// <summary>
        ///     Gets or sets the final volume level at the end of the fade-out section as a percentage (0 - 1)
        /// </summary>
        public float FadeOutEndVolume { get; set; }

        /// <summary>
        ///     Gets the length of the active section in samples
        ///     (The active section is the start of the fade-in to the start of the fade-out.)
        /// </summary>
        public long ActiveLength
        {
            get { return (FadeOutStart - FadeInStart) + AdditionalStartLoopLength - SkipLength; }
        }

        /// <summary>
        ///     Returns true if this track is looped at the start.
        /// </summary>
        public bool IsLoopedAtStart
        {
            get { return (StartLoopCount >= 2); }
        }

        /// <summary>
        ///     Gets the combined start loop length excluding the first loop.
        /// </summary>
        internal long AdditionalStartLoopLength
        {
            get { return !IsLoopedAtStart ? 0 : (StartLoopCount - 1) * FadeInLength; }
        }

        /// <summary>
        ///     Gets the combined start loop length including the first loop.
        /// </summary>
        public long FullStartLoopLength
        {
            get { return !IsLoopedAtStart ? FadeInLength : StartLoopCount * FadeInLength; }
        }

        /// <summary>
        ///     Gets the combined start loop length including the first loop in seconds.
        /// </summary>
        public double FullStartLoopLengthSeconds
        {
            get { return SamplesToSeconds(FullStartLoopLength); }
        }

        /// <summary>
        ///     Returns true if this track is looped at the end.
        /// </summary>
        public bool IsLoopedAtEnd
        {
            get { return (EndLoopCount >= 2); }
        }

        /// <summary>
        ///     Gets the combined end loop length excluding the first loop.
        /// </summary>
        internal long AdditionalEndLoopLength
        {
            get { return !IsLoopedAtEnd ? 0 : (EndLoopCount - 1) * FadeOutLength; }
        }

        /// <summary>
        ///     Gets the combined end loop length including the first loop.
        /// </summary>
        public long FullEndLoopLength
        {
            get { return !IsLoopedAtEnd ? FadeOutLength : EndLoopCount * FadeOutLength; }
        }

        /// <summary>
        ///     Gets the combined start loop length including the first loop in seconds.
        /// </summary>
        public double FullEndLoopLengthSeconds
        {
            get { return SamplesToSeconds(FullEndLoopLength); }
        }

        /// <summary>
        ///     Gets the length of the active section in seconds
        ///     (The active section is the start of the fade-in to the start of the fade-out.)
        /// </summary>
        public double ActiveLengthSeconds
        {
            get { return SamplesToSeconds(ActiveLength); }
        }

        /// <summary>
        ///     Gets the length of the active section in seconds formatted as a string
        ///     (The active section is the start of the fade-in to the start of the fade-out.)
        /// </summary>
        public string ActiveLengthFormatted
        {
            get { return FormatSeconds(ActiveLengthSeconds); }
        }

        /// <summary>
        ///     Gets or sets the start of the skip section as a sample position
        /// </summary>
        public long SkipStart { get; set; }

        /// <summary>
        ///     Gets or sets the end of the skip section as a sample position
        /// </summary>
        public long SkipEnd { get; set; }

        /// <summary>
        ///     Gets the length of the skip section in samples.
        /// </summary>
        public long SkipLength
        {
            get { return SkipEnd - SkipStart; }
        }

        /// <summary>
        ///     Gets the length of the skip section in seconds
        /// </summary>
        public double SkipLengthSeconds
        {
            get { return SamplesToSeconds(SkipLength); }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether skip section should be used.
        /// </summary>
        public bool HasSkipSection
        {
            get { return SkipStart != 0 && SkipLength > 0; }
        }

        /// <summary>
        ///     The BASS position callbacks BassPlayer has set on this track.
        /// </summary>
        internal TrackSyncs Syncs { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether pre-fade-in should be used.
        /// </summary>
        public bool UsePreFadeIn { get; set; }

        /// <summary>
        ///     Gets or sets the pre-fade-in start.
        /// </summary>
        public long PreFadeInStart { get; set; }

        /// <summary>
        ///     Gets or sets the pre-fade-in start volume as a percentage (0 - 1)
        /// </summary>
        public float PreFadeInStartVolume { get; set; }

        /// <summary>
        ///     Gets or sets the BPM adjustment.
        /// </summary>
        public decimal BpmAdjustmentRatio { get; set; }

        /// <summary>
        ///     Gets the length of the pre-fade-in section in samples.
        /// </summary>
        public long PreFadeInLength
        {
            get { return FadeInStart - PreFadeInStart; }
        }

        /// <summary>
        ///     Gets the length of the pre-fade-in section in seconds
        /// </summary>
        public double PreFadeInLengthSeconds
        {
            get { return SamplesToSeconds(PreFadeInLength); }
        }

        /// <summary>
        ///     Gets or sets the BPM of the track (as specified in the MP3 tag)
        /// </summary>
        public decimal TagBpm { get; set; }

        /// <summary>
        ///     Gets or sets the number of times the start loop should be repeated.
        /// </summary>
        public int StartLoopCount { get; set; }

        /// <summary>
        ///     Gets or sets the number times of times the start loop has been played.
        /// </summary>
        internal int CurrentStartLoop { get; set; }

        /// <summary>
        ///     Gets or sets the number of times the end loop should be repeated.
        /// </summary>
        public int EndLoopCount { get; set; }

        /// <summary>
        ///     Gets or sets the number times of times the end loop has been played.
        /// </summary>
        internal int CurrentEndLoop { get; set; }

        /// <summary>
        ///     Set to true when the data in the track mp3 tags is loaded
        /// </summary>
        public bool TagDataLoaded { get; internal set; }

        /// <summary>
        ///     If true, a 'power-down' noise will be played on fade out
        /// </summary>
        public bool PowerDownOnEnd { get; set; }

        internal bool PowerDownOnEndOriginal { get; set; }

        /// <summary>
        ///     Gets or sets the start of the raw-loop section as a sample position
        /// </summary>
        public long RawLoopStart { get; set; }

        /// <summary>
        ///     Gets or sets the start offset of the raw-loop section as a sample position
        ///     The raw-loop will start playing here initially, but loop back to the start
        /// </summary>
        public long RawLoopOffset { get; set; }

        /// <summary>
        ///     Gets or sets the end of the raw-loop section as a sample position
        /// </summary>
        public long RawLoopEnd { get; set; }

        public int Rank { get; set; }

        internal void ResetPowerDownOnEnd()
        {
            PowerDownOnEnd = PowerDownOnEndOriginal;
        }

        // ---------- live fade moves (docs/mixer-waveform.md) ----------
        // A fade section moved for this play only. The fade points above hold the moved positions; these hold how
        // far they were moved, so a reload from disk can put the move back and an unload can take it away.

        /// <summary>
        ///     How far the fade-in section (and the pre-fade-in start with it) has been moved for this play, in samples.
        /// </summary>
        internal long LiveFadeInShift { get; private set; }

        /// <summary>
        ///     The same move in whole fade-in sections. Kept as well as the samples, because a section's length can
        ///     change after a move (a fade type reshapes the fade-out).
        /// </summary>
        internal int LiveFadeInSections { get; private set; }

        /// <summary>
        ///     How far the fade-out section has been moved for this play, in samples.
        /// </summary>
        internal long LiveFadeOutShift { get; private set; }

        /// <summary>
        ///     The same move in whole fade-out sections.
        /// </summary>
        internal int LiveFadeOutSections { get; private set; }

        internal void ShiftFadeIn(long samples, int sections)
        {
            FadeInStart += samples;
            FadeInEnd += samples;
            PreFadeInStart += samples;
            LiveFadeInShift += samples;
            LiveFadeInSections += sections;
        }

        internal void ShiftFadeOut(long samples, int sections)
        {
            FadeOutStart += samples;
            FadeOutEnd += samples;
            LiveFadeOutShift += samples;
            LiveFadeOutSections += sections;
        }

        /// <summary>
        ///     Moves the fade sections back to where they were before any live move.
        /// </summary>
        internal void RemoveLiveShifts()
        {
            ShiftFadeIn(-LiveFadeInShift, -LiveFadeInSections);
            ShiftFadeOut(-LiveFadeOutShift, -LiveFadeOutSections);
        }

        /// <summary>
        ///     Forgets the live moves without touching the fade points: for when the fade points are about to be
        ///     set from scratch.
        /// </summary>
        internal void ForgetLiveShifts()
        {
            LiveFadeInShift = 0;
            LiveFadeInSections = 0;
            ForgetLiveFadeOutShift();
        }

        /// <summary>
        ///     Forgets a live fade-out move without touching the fade points: for when the fade-out has been put
        ///     somewhere else ("fade now").
        /// </summary>
        internal void ForgetLiveFadeOutShift()
        {
            LiveFadeOutShift = 0;
            LiveFadeOutSections = 0;
        }
    }
}