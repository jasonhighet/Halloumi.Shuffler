using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Halloumi.BassEngine.Helpers;
using Un4seen.Bass;

namespace Halloumi.BassEngine.Models
{
    /// <summary>
    ///     Represents a playable mp3 file
    /// </summary>
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class Track
    {
        private readonly List<SecondSampleConversion> _cachedConversions = new List<SecondSampleConversion>();

        /// <summary>
        ///     The ratio used to convert samples to seconds for this track
        /// </summary>
        private const double SamplesToSecondsRatio = 0.001D;

        /// <summary>
        ///     The BPM at the end of the song
        /// </summary>
        private decimal _endBmp = 100;

        /// <summary>
        ///     The BPM at the start of the song
        /// </summary>
        private decimal _startBmp = 100;

        /// <summary>
        ///     Delegate for the method called when the sync events are fired
        /// </summary>
        internal SYNCPROC TrackSync = null;

        /// <summary>
        ///     Initializes a new instance of the track class.
        /// </summary>
        public Track()
        {
            Channels = new List<int>();
            FadeInStartSyncId = int.MinValue;
            FadeInEndSyncId = int.MinValue;
            FadeOutStartSyncId = int.MinValue;
            FadeOutEndSyncId = int.MinValue;
            PreFadeInStartSyncId = int.MinValue;
            TrackEndSyncId = int.MinValue;
            ExtendedMixEndSyncId = int.MinValue;
            ChangeTempoOnFadeOut = true;
            Gain = 0;
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
            Image = null;
            RawLoopStart = 0;
            RawLoopEnd = 0;
            RawLoopEndSyncId = int.MinValue;
            LoopFadeInIndefinitely = false;
            SkipStart = 0;
            SkipEnd = 0;
            SkipSyncId = int.MinValue;

            Rank = 1;
        }

        /// <summary>
        ///     Gets or sets the Id for the track.
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        ///     Gets or sets the bass channel Id for the track (set once the track is loaded by the bass engine)
        /// </summary>
        public int Channel => Channels.Count == 0 ? int.MinValue : Channels[0];

        public List<int> Channels { get; }

        /// <summary>
        ///     Gets or sets the name of the mp3 file associated with the track.
        /// </summary>
        public string Filename { get; set; }

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
        public string Description => Artist + " - " + Title;

        /// <summary>
        ///     Gets or sets the track comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        ///     Gets the BPM at the start of the track
        /// </summary>
        public decimal StartBpm
        {
            get
            {
                if (FadeInLengthSeconds != 0)
                    return BassHelper.GetBpmFromLoopLength(FadeInLengthSeconds);

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
                //if (this.Channel == int.MinValue) return _endBMP;
                if (FadeOutLengthSeconds != 0)
                    return BassHelper.GetBpmFromLoopLength(FadeOutLengthSeconds);

                if (!TagDataLoaded)
                    return _endBmp;

                return TagBpm*BpmAdjustmentRatio;
            }
            internal set { _endBmp = value; }
        }

        public decimal Bpm => BassHelper.GetAdjustedBpmAverage(StartBpm, EndBpm);

        /// <summary>
        ///     Gets or sets the default volume level of the track.
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        ///     Gets or sets the length of the track in samples.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        ///     Gets the length of the track in seconds.
        /// </summary>
        public double LengthSeconds => SamplesToSeconds(Length);

        /// <summary>
        ///     Gets the length of the track in seconds formatted as a string.
        /// </summary>
        public string LengthFormatted => FormatSeconds(LengthSeconds);

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
        public long FadeInLength => FadeInEnd - FadeInStart;

        /// <summary>
        ///     Gets the length of the fade-in section in seconds
        /// </summary>
        public double FadeInLengthSeconds => SamplesToSeconds(FadeInLength);

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
        public long FadeOutLength => FadeOutEnd - FadeOutStart;

        /// <summary>
        ///     Gets the length of the fade-out section in seconds
        /// </summary>
        public double FadeOutLengthSeconds => SamplesToSeconds(FadeOutLength);

        /// <summary>
        ///     Gets or sets the initial volume level at the start of the fade-out section as a percentage (0 - 1)
        /// </summary>
        public float FadeOutStartVolume { get; set; }

        /// <summary>
        ///     Gets or sets the final volume level at the end of the fade-out section as a percentage (0 - 1)
        /// </summary>
        public float FadeOutEndVolume { get; set; }

        /// <summary>
        ///     Gets or sets the gain (volume adjustment) for the track
        /// </summary>
        public float Gain { get; set; }

        /// <summary>
        ///     If true, the tempo will be changed to that of the next track when fading out
        /// </summary>
        public bool ChangeTempoOnFadeOut { get; set; }

        /// <summary>
        ///     Gets the length of the active section in samples
        ///     (The active section is the start of the fade-in to the start of the fade-out.)
        /// </summary>
        public long ActiveLength => (FadeOutStart - FadeInStart) + AdditionalStartLoopLength - SkipLength;

        /// <summary>
        ///     Returns true if this track is looped at the start.
        /// </summary>
        public bool IsLoopedAtStart => (StartLoopCount >= 2);

        /// <summary>
        ///     Gets the combined start loop length excluding the first loop.
        /// </summary>
        internal long AdditionalStartLoopLength
        {
            get
            {
                if (!IsLoopedAtStart) return 0;
                return (StartLoopCount - 1)*FadeInLength;
            }
        }

        /// <summary>
        ///     Gets the combined start loop length including the first loop.
        /// </summary>
        public long FullStartLoopLength
        {
            get
            {
                if (!IsLoopedAtStart) return FadeInLength;
                return StartLoopCount*FadeInLength;
            }
        }

        /// <summary>
        ///     Gets the combined start loop length including the first loop in seconds.
        /// </summary>
        public double FullStartLoopLengthSeconds => SamplesToSeconds(FullStartLoopLength);

        /// <summary>
        ///     Returns true if this track is looped at the end.
        /// </summary>
        public bool IsLoopedAtEnd => (EndLoopCount >= 2);

        /// <summary>
        ///     Gets the combined end loop length excluding the first loop.
        /// </summary>
        internal long AdditionalEndLoopLength
        {
            get
            {
                if (!IsLoopedAtEnd) return 0;
                return (EndLoopCount - 1)*FadeOutLength;
            }
        }

        /// <summary>
        ///     Gets the combined end loop length including the first loop.
        /// </summary>
        public long FullEndLoopLength
        {
            get
            {
                if (!IsLoopedAtEnd) return FadeOutLength;
                return EndLoopCount*FadeOutLength;
            }
        }

        /// <summary>
        ///     Gets the combined start loop length including the first loop in seconds.
        /// </summary>
        public double FullEndLoopLengthSeconds => SamplesToSeconds(FullEndLoopLength);

        /// <summary>
        ///     Gets the length of the active section in seconds
        ///     (The active section is the start of the fade-in to the start of the fade-out.)
        /// </summary>
        public double ActiveLengthSeconds => SamplesToSeconds(ActiveLength);

        /// <summary>
        ///     Gets the length of the active section in seconds formatted as a string
        ///     (The active section is the start of the fade-in to the start of the fade-out.)
        /// </summary>
        public string ActiveLengthFormatted => FormatSeconds(ActiveLengthSeconds);

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
        public long SkipLength => SkipEnd - SkipStart;

        /// <summary>
        ///     Gets the length of the skip section in seconds
        /// </summary>
        public double SkipLengthSeconds => SamplesToSeconds(SkipLength);

        /// <summary>
        ///     Gets or sets a value indicating whether skip section should be used.
        /// </summary>
        public bool HasSkipSection => SkipStart != 0 && SkipLength > 0;

        /// <summary>
        ///     Gets or sets the skip sync handle
        /// </summary>
        internal int SkipSyncId { get; set; }

        public Image Image { get; set; }

        public int DefaultSampleRate { get; set; }

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
        public long PreFadeInLength => FadeInStart - PreFadeInStart;

        /// <summary>
        ///     Gets the length of the pre-fade-in section in seconds
        /// </summary>
        public double PreFadeInLengthSeconds => SamplesToSeconds(PreFadeInLength);

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
        public bool IsInRawLoopMode => (RawLoopEnd != 0);

        /// <summary>
        ///     Gets or sets a value indicating whether the fade in section should loop fade in indefinitely.
        /// </summary>
        public bool LoopFadeInIndefinitely { get; set; }

        public int Rank { get; set; }

        public string Key { get; set; }

        internal GCHandle AudioDataHandle { get; set; }

        internal byte[] AudioData { get; set; }

        internal IntPtr AudioDataPointer => AudioDataHandle.AddrOfPinnedObject();


        internal void ResetPowerDownOnEnd()
        {
            PowerDownOnEnd = PowerDownOnEndOriginal;
        }

        public bool IsAudioLoaded()
        {
            return Channel != int.MinValue;
        }

        public void AddChannel(int channel)
        {
            Channels.Insert(0, channel);
        }


        /// <summary>
        ///     Converts a sample count into a duration in seconds.
        /// </summary>
        /// <param name="samples">The sample count.</param>
        /// <returns>The number of seconds</returns>
        public double SamplesToSeconds(long samples)
        {
            if (samples < 0) return 0D;

            lock (_cachedConversions)
            {
                if (_cachedConversions.Exists(c => c.Samples == samples))
                {
                    var firstOrDefault = _cachedConversions.FirstOrDefault(c => c.Samples == samples);
                    if (firstOrDefault != null)
                        return firstOrDefault.Seconds;
                }
            }

            if (Channel == int.MinValue) return samples*SamplesToSecondsRatio;
            var value = Bass.BASS_ChannelBytes2Seconds(Channel, samples);
            if (value == -1)
            {
                value = GuessSecondsFromSamples(samples);
            }
            else if (_cachedConversions.Count < 512)
            {
                lock (_cachedConversions)
                {
                    _cachedConversions.Add(new SecondSampleConversion {Seconds = value, Samples = samples});
                }
            }

            return value;
        }

        /// <summary>
        ///     Converts a duration in seconds into a sample count.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns>
        ///     The number of seconds
        /// </returns>
        public long SecondsToSamples(decimal seconds)
        {
            return SecondsToSamples(Convert.ToDouble(seconds));
        }

        /// <summary>
        ///     Converts a duration in seconds into a sample count.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns>
        ///     The number of seconds
        /// </returns>
        public long SecondsToSamples(double seconds)
        {
            if (seconds < 0) return 0;

            lock (_cachedConversions)
            {
                if (_cachedConversions.Exists(c => c.Seconds == seconds))
                {
                    var secondSampleConversion = _cachedConversions.FirstOrDefault(c => c.Seconds == seconds);
                    if (secondSampleConversion != null)
                        return secondSampleConversion.Samples;
                }
            }

            if (Channel == int.MinValue) return (long) (seconds/SamplesToSecondsRatio);
            var value = Bass.BASS_ChannelSeconds2Bytes(Channel, seconds);
            if (value == -1)
            {
                value = GuessSamplesFromSeconds(seconds);
            }
            else if (_cachedConversions.Count < 512)
            {
                lock (_cachedConversions)
                {
                    _cachedConversions.Add(new SecondSampleConversion {Seconds = seconds, Samples = value});
                }
            }

            return value;
        }

        private long GuessSamplesFromSeconds(double seconds)
        {
            if (_cachedConversions.Count == 0) return 0;

            lock (_cachedConversions)
            {
                var conversion = _cachedConversions.Last();
                var samplesPerSecond = Convert.ToDouble(conversion.Samples)/conversion.Seconds;
                return Convert.ToInt64(seconds*samplesPerSecond);
            }
        }

        private double GuessSecondsFromSamples(long samples)
        {
            if (_cachedConversions.Count == 0) return 0;
            lock (_cachedConversions)
            {
                var conversion = _cachedConversions.Last();
                var secondsPerSample = conversion.Seconds/Convert.ToDouble(conversion.Samples);
                return Convert.ToDouble(samples*secondsPerSample);
            }
        }

        /// <summary>
        ///     Formats the seconds as a string in a HH:MM:SS format.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns>A formatted string</returns>
        public string FormatSeconds(double seconds)
        {
            return Utils.FixTimespan(seconds, "HHMMSS");
        }

        /// <summary>
        ///     Returns a string that represents this instance.
        /// </summary>
        /// <returns> A string that represents this instance.</returns>
        public override string ToString()
        {
            return Filename;
        }

        private class SecondSampleConversion
        {
            public double Seconds { get; set; }

            public long Samples { get; set; }
        }
    }
}