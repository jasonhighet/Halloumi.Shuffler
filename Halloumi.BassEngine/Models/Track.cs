using System.Diagnostics.CodeAnalysis;
using System.Drawing;
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
        private decimal _endBmp = 100;

        /// <summary>
        ///     The BPM at the start of the song
        /// </summary>
        private decimal _startBmp = 100;


        /// <summary>
        ///     Initializes a new instance of the track class.
        /// </summary>
        public Track()
        {
            FadeInStartSyncId = int.MinValue;
            FadeInEndSyncId = int.MinValue;
            FadeOutStartSyncId = int.MinValue;
            FadeOutEndSyncId = int.MinValue;
            PreFadeInStartSyncId = int.MinValue;
            TrackEndSyncId = int.MinValue;
            ExtendedMixEndSyncId = int.MinValue;
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
            RawLoopEndSyncId = int.MinValue;
            SkipStart = 0;
            SkipEnd = 0;
            SkipSyncId = int.MinValue;

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
                    return _startBmp;

                return TagBpm*BpmAdjustmentRatio;
            }
            internal set { _startBmp = value; }
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
                    return _endBmp;

                return TagBpm*BpmAdjustmentRatio;
            }
            internal set { _endBmp = value; }
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
        ///     Gets or sets the skip sync handle
        /// </summary>
        internal int SkipSyncId { get; set; }

        /// <summary>
        ///     Gets or sets the fade-in-start sync handle
        /// </summary>
        internal int FadeInStartSyncId { get; set; }

        /// <summary>
        ///     Gets or sets the pre-fade-in-start sync handle (where the pre-fade of the next track starts)
        /// </summary>
        internal int PreFadeInStartSyncId { get; set; }

        /// <summary>
        ///     Gets or sets the fade-in-end sync handle
        /// </summary>
        internal int FadeInEndSyncId { get; set; }

        /// <summary>
        ///     Gets or sets the fade-out-start sync handle
        /// </summary>
        internal int FadeOutStartSyncId { get; set; }

        /// <summary>
        ///     Gets or sets the fade-out-end sync handle
        /// </summary>
        internal int FadeOutEndSyncId { get; set; }

        /// <summary>
        ///     Gets or sets the extended mix end sync handle
        /// </summary>
        internal int ExtendedMixEndSyncId { get; set; }

        /// <summary>
        ///     Gets or sets the track end sync handle
        /// </summary>
        internal int TrackEndSyncId { get; set; }

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

        /// <summary>
        ///     Gets or sets the raw-loop-end sync handle
        /// </summary>
        internal int RawLoopEndSyncId { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this track is in raw-loop mode.
        /// </summary>
        public bool IsInRawLoopMode
        {
            get { return (RawLoopEnd != 0); }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the fade in section should loop fade in indefinitely.
        /// </summary>
        public bool LoopFadeInIndefinitely { get; set; }

        public int Rank { get; set; }

        internal void ResetPowerDownOnEnd()
        {
            PowerDownOnEnd = PowerDownOnEndOriginal;
        }
    }
}