using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using Un4seen.Bass;
using Un4seen.Bass.Misc;

namespace Halloumi.BassEngine
{
    /// <summary>
    /// Represents a playable mp3 file
    /// </summary>
    public class Track
    {
        #region Private Variables

        /// <summary>
        /// The ratio used to convert samples to seconds for this track
        /// </summary>
        private double _samplesToSecondsRatio = 0.001D;

        /// <summary>
        /// The BPM at the start of the song
        /// </summary>
        private decimal _startBmp = 100;

        /// <summary>
        /// The BPM at the end of the song
        /// </summary>
        private decimal _endBmp = 100;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the track class.
        /// </summary>
        public Track()
        {
            this.Channels = new List<int>();
            this.FadeInStartSyncId = int.MinValue;
            this.FadeInEndSyncId = int.MinValue;
            this.FadeOutStartSyncId = int.MinValue;
            this.FadeOutEndSyncId = int.MinValue;
            this.PreFadeInStartSyncId = int.MinValue;
            this.TrackEndSyncId = int.MinValue;
            this.ExtendedMixEndSyncId = int.MinValue;
            this.ChangeTempoOnFadeOut = true;
            this.Gain = 0;
            this.UsePreFadeIn = false;
            this.PreFadeInStart = 0;
            this.PreFadeInStartVolume = 0;
            this.BpmAdjustmentRatio = 1;
            this.StartLoopCount = 0;
            this.CurrentStartLoop = 0;
            this.EndLoopCount = 0;
            this.CurrentEndLoop = 0;
            this.TagDataLoaded = false;
            this.FadeInEnd = 0;
            this.FadeInStart = 0;
            this.FadeOutEnd = 0;
            this.FadeOutStart = 0;
            this.PowerDownOnEnd = false;
            this.PowerDownOnEndOriginal = false;
            this.Image = null;
            this.RawLoopStart = 0;
            this.RawLoopEnd = 0;
            this.RawLoopEndSyncId = int.MinValue;
            this.LoopFadeInIndefinitely = false;
            this.SkipStart = 0;
            this.SkipEnd = 0;
            this.SkipSyncId = int.MinValue;

            this.Rank = 1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Id for the track.
        /// </summary>
        public int Id
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the bass channel Id for the track (set once the track is loaded by the bass engine)
        /// </summary>
        public int Channel
        {
            get
            {
                if (Channels.Count == 0)
                    return int.MinValue;

                return Channels[0];
            }
        }

        public void AddChannel(int channel)
        {
            Channels.Insert(0, channel);
        }

        public List<int> Channels { get; private set; }

        /// <summary>
        /// Gets or sets the name of the mp3 file associated with the track.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the track title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the track artist.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Gets a description of the track
        /// </summary>
        public string Description
        {
            get
            {
                return this.Artist + " - " + this.Title;
            }
        }

        /// <summary>
        /// Gets or sets the track comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets the BPM at the start of the track
        /// </summary>
        public decimal StartBpm
        {
            get
            {
                if (this.FadeInLengthSeconds != 0)
                    return BassHelper.GetBpmFromLoopLength(this.FadeInLengthSeconds);

                if (!this.TagDataLoaded)
                    return _startBmp;

                return this.TagBpm * this.BpmAdjustmentRatio;
            }
            internal set
            {
                _startBmp = value;
            }
        }

        /// <summary>
        /// Gets the BPM at the end of the track
        /// </summary>
        public decimal EndBpm
        {
            get
            {
                //if (this.Channel == int.MinValue) return _endBMP;
                if (this.FadeOutLengthSeconds != 0)
                    return BassHelper.GetBpmFromLoopLength(this.FadeOutLengthSeconds);

                if (!this.TagDataLoaded)
                    return _endBmp;

                return this.TagBpm * this.BpmAdjustmentRatio;
            }
            internal set
            {
                _endBmp = value;
            }
        }

        public decimal Bpm
        {
            get
            {
                //return (this.StartBPM + this.EndBPM) / 2M;
                return BassHelper.GetAdjustedBpmAverage(this.StartBpm, this.EndBpm);
            }
        }

        /// <summary>
        /// Gets or sets the defaul volume level of the track.
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Gets or sets the length of the track in samples.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// Gets the length of the track in seconds.
        /// </summary>
        public double LengthSeconds
        {
            get
            {
                return this.SamplesToSeconds(this.Length);
            }
        }

        /// <summary>
        /// Gets the length of the track in seconds formatted as a string.
        /// </summary>
        public string LengthFormatted
        {
            get
            {
                return FormatSeconds(this.LengthSeconds);
            }
        }

        /// <summary>
        /// Gets or sets the start of the fade-in section as a sample position
        /// </summary>
        public long FadeInStart { get; set; }

        /// <summary>
        /// Gets or sets the end of the fade-in section as a sample position
        /// </summary>
        public long FadeInEnd { get; set; }

        /// <summary>
        /// Gets the length of the fade-in section in samples.
        /// </summary>
        public long FadeInLength
        {
            get
            {
                return this.FadeInEnd - this.FadeInStart;
            }
        }

        /// <summary>
        /// Gets the length of the fade-in section in seconds
        /// </summary>
        public double FadeInLengthSeconds
        {
            get
            {
                return this.SamplesToSeconds(this.FadeInLength);
            }
        }

        /// <summary>
        /// Gets or sets the initial volume level at the start of the fade-in section as a percentage (0 - 1)
        /// </summary>
        public float FadeInStartVolume { get; set; }

        /// <summary>
        /// Gets or sets the final volume level at the end of the fade-in section as a percentage (0 - 1)
        /// </summary>
        public float FadeInEndVolume { get; set; }

        /// <summary>
        /// Gets or sets the start of the fade-out section as a sample position
        /// </summary>
        public long FadeOutStart { get; set; }

        /// <summary>
        /// Gets or sets the end of the fade-out section as a sample position
        /// </summary>
        public long FadeOutEnd { get; set; }

        /// <summary>
        /// Gets the length of the fade-out section in samples.
        /// </summary>
        public long FadeOutLength
        {
            get
            {
                return this.FadeOutEnd - this.FadeOutStart;
            }
        }

        /// <summary>
        /// Gets the length of the fade-out section in seconds
        /// </summary>
        public double FadeOutLengthSeconds
        {
            get
            {
                return this.SamplesToSeconds(this.FadeOutLength);
            }
        }

        /// <summary>
        /// Gets or sets the initial volume level at the start of the fade-out section as a percentage (0 - 1)
        /// </summary>
        public float FadeOutStartVolume { get; set; }

        /// <summary>
        /// Gets or sets the final volume level at the end of the fade-out section as a percentage (0 - 1)
        /// </summary>
        public float FadeOutEndVolume { get; set; }

        /// <summary>
        /// Gets or sets the gain (volume adjustment) for the track
        /// </summary>
        public float Gain { get; set; }

        /// <summary>
        /// If true, the tempo will be changed to that of the next track when fading out
        /// </summary>
        public bool ChangeTempoOnFadeOut { get; set; }

        /// <summary>
        /// Gets the length of the active section in samples
        /// (The active secion is the start of the fade-in to the start of the fade-out.)
        /// </summary>
        public long ActiveLength
        {
            get
            {
                return (this.FadeOutStart - this.FadeInStart) + this.AdditionalStartLoopLength - this.SkipLength;
            }
        }

        /// <summary>
        /// Returns true if this track is looped at the start.
        /// </summary>
        public bool IsLoopedAtStart
        {
            get { return (this.StartLoopCount >= 2); }
        }

        /// <summary>
        /// Gets the combined start loop length excluding the first loop.
        /// </summary>
        internal long AdditionalStartLoopLength
        {
            get
            {
                if (!this.IsLoopedAtStart) return 0;
                return (this.StartLoopCount - 1) * this.FadeInLength;
            }
        }

        /// <summary>
        /// Gets the combined start loop length including the first loop.
        /// </summary>
        public long FullStartLoopLength
        {
            get
            {
                if (!this.IsLoopedAtStart) return this.FadeInLength;
                return this.StartLoopCount * this.FadeInLength;
            }
        }

        /// <summary>
        /// Gets the combined start loop length including the first loop in seconds.
        /// </summary>
        public double FullStartLoopLengthSeconds
        {
            get
            {
                return this.SamplesToSeconds(this.FullStartLoopLength);
            }
        }

        /// <summary>
        /// Returns true if this track is looped at the end.
        /// </summary>
        public bool IsLoopedAtEnd
        {
            get { return (this.EndLoopCount >= 2); }
        }

        /// <summary>
        /// Gets the combined end loop length excluding the first loop.
        /// </summary>
        internal long AdditionalEndLoopLength
        {
            get
            {
                if (!this.IsLoopedAtEnd) return 0;
                return (this.EndLoopCount - 1) * this.FadeOutLength;
            }
        }

        /// <summary>
        /// Gets the combined end loop length including the first loop.
        /// </summary>
        public long FullEndLoopLength
        {
            get
            {
                if (!this.IsLoopedAtEnd) return this.FadeOutLength;
                return this.EndLoopCount * this.FadeOutLength;
            }
        }

        /// <summary>
        /// Gets the combined start loop length including the first loop in seconds.
        /// </summary>
        public double FullEndLoopLengthSeconds
        {
            get
            {
                return this.SamplesToSeconds(this.FullEndLoopLength);
            }
        }

        /// <summary>
        /// Gets the length of the active section in seconds
        /// (The active secion is the start of the fade-in to the start of the fade-out.)
        /// </summary>
        public double ActiveLengthSeconds
        {
            get
            {
                return this.SamplesToSeconds(this.ActiveLength);
            }
        }

        /// <summary>
        /// Gets the length of the active section in seconds formatted as a string
        /// (The active secion is the start of the fade-in to the start of the fade-out.)
        /// </summary>
        public string ActiveLengthFormatted
        {
            get
            {
                return FormatSeconds(this.ActiveLengthSeconds);
            }
        }

        /// <summary>
        /// Gets or sets the start of the skip section as a sample position
        /// </summary>
        public long SkipStart { get; set; }

        /// <summary>
        /// Gets or sets the end of the skip section as a sample position
        /// </summary>
        public long SkipEnd { get; set; }

        /// <summary>
        /// Gets the length of the skip section in samples.
        /// </summary>
        public long SkipLength
        {
            get
            {
                return this.SkipEnd - this.SkipStart;
            }
        }

        /// <summary>
        /// Gets the length of the skip section in seconds
        /// </summary>
        public double SkipLengthSeconds
        {
            get
            {
                return this.SamplesToSeconds(this.SkipLength);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether skip section should be used.
        /// </summary>
        public bool HasSkipSection
        {
            get { return this.SkipStart != 0 && this.SkipLength > 0; }
        }

        /// <summary>
        /// Gets or sets the skip sync handle
        /// </summary>
        internal int SkipSyncId { get; set; }

        public Image Image
        {
            get;
            set;
        }

        public int DefaultSampleRate
        {
            get;
            set;
        }

        /// <summary>
        /// Delegate for the method called when the sync events are fired
        /// </summary>
        internal SYNCPROC TrackSync = null;

        /// <summary>
        /// Gets or sets the fade-in-start sync handle
        /// </summary>
        internal int FadeInStartSyncId { get; set; }

        /// <summary>
        /// Gets or sets the pre-fade-in-start sync handle (where the pre-fade of the next track starts)
        /// </summary>
        internal int PreFadeInStartSyncId { get; set; }

        /// <summary>
        /// Gets or sets the fade-in-end sync handle
        /// </summary>
        internal int FadeInEndSyncId { get; set; }

        /// <summary>
        /// Gets or sets the fade-out-start sync handle
        /// </summary>
        internal int FadeOutStartSyncId { get; set; }

        /// <summary>
        /// Gets or sets the fade-out-end sync handle
        /// </summary>
        internal int FadeOutEndSyncId { get; set; }

        /// <summary>
        /// Gets or sets the extended mix end sync handle
        /// </summary>
        internal int ExtendedMixEndSyncId { get; set; }

        /// <summary>
        /// Gets or sets the track end sync handle
        /// </summary>
        internal int TrackEndSyncId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pre-fade-in should be used.
        /// </summary>
        public bool UsePreFadeIn { get; set; }

        /// <summary>
        /// Gets or sets the pre-fade-in start.
        /// </summary>
        public long PreFadeInStart { get; set; }

        /// <summary>
        /// Gets or sets the pre-fade-in start volume as a percentage (0 - 1)
        /// </summary>
        public float PreFadeInStartVolume { get; set; }

        /// <summary>
        /// Gets or sets the BPM adjustment.
        /// </summary>
        public decimal BpmAdjustmentRatio { get; set; }

        /// <summary>
        /// Gets the length of the pre-fade-in section in samples.
        /// </summary>
        public long PreFadeInLength
        {
            get
            {
                return this.FadeInStart - this.PreFadeInStart;
            }
        }

        /// <summary>
        /// Gets the length of the pre-fade-in section in seconds
        /// </summary>
        public double PreFadeInLengthSeconds
        {
            get
            {
                return this.SamplesToSeconds(this.PreFadeInLength);
            }
        }

        /// <summary>
        /// Gets or sets the BPM of the track (as specified in the MP3 tag)
        /// </summary>
        public decimal TagBpm { get; set; }

        /// <summary>
        /// Gets or sets the number of times the start loop should be repeated.
        /// </summary>
        public int StartLoopCount { get; set; }

        /// <summary>
        /// Gets or sets the number times of times the start loop has been played.
        /// </summary>
        internal int CurrentStartLoop { get; set; }

        /// <summary>
        /// Gets or sets the number of times the end loop should be repeated.
        /// </summary>
        public int EndLoopCount { get; set; }

        /// <summary>
        /// Gets or sets the number times of times the end loop has been played.
        /// </summary>
        internal int CurrentEndLoop { get; set; }

        /// <summary>
        /// Set to true when the data in the track mp3 tags is loaded
        /// </summary>
        public bool TagDataLoaded { get; internal set; }

        /// <summary>
        /// If true, a 'power-down' noise will be played on fade out
        /// </summary>
        public bool PowerDownOnEnd { get; set; }

        internal bool PowerDownOnEndOriginal { get; set; }

        /// <summary>
        /// Gets or sets the start of the raw-loop section as a sample position
        /// </summary>
        public long RawLoopStart { get; set; }

        /// <summary>
        /// Gets or sets the start offset of the raw-loop section as a sample position
        /// The raw-loop will start playing here initially, but loop back to the start
        /// </summary>
        public long RawLoopOffset { get; set; }

        /// <summary>
        /// Gets or sets the end of the raw-loop section as a sample position
        /// </summary>
        public long RawLoopEnd { get; set; }

        /// <summary>
        /// Gets or sets the raw-loop-end sync handle
        /// </summary>
        internal int RawLoopEndSyncId { get; set; }

        /// <summary>
        /// Gets a value indicating whether this track is in raw-loop mode.
        /// </summary>
        public bool IsInRawLoopMode
        {
            get
            {
                return (this.RawLoopEnd != 0);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the fade in section should loop fade in indefinitely.
        /// </summary>
        public bool LoopFadeInIndefinitely { get; set; }

        public int Rank { get; set; }

        public string Key { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts a sample count into a duration in seconds.
        /// </summary>
        /// <param name="samples">The sample count.</param>
        /// <returns>The number of seconds</returns>
        public double SamplesToSeconds(long samples)
        {
            if (samples < 0) return 0D;

            lock (_cachedConversions)
            {
                if (_cachedConversions.Exists(c => c.Samples == samples))
                    return _cachedConversions.Where(c => c.Samples == samples).FirstOrDefault().Seconds;
            }

            if (this.Channel == int.MinValue) return (double)samples * _samplesToSecondsRatio;
            else
            {
                var value = Bass.BASS_ChannelBytes2Seconds(this.Channel, samples);
                if (value == -1)
                {
                    value = GuessSecondsFromSamples(samples);
                }
                else if (_cachedConversions.Count < 512)
                {
                    lock (_cachedConversions)
                    {
                        _cachedConversions.Add(new SecondSampleConversion() { Seconds = value, Samples = samples });
                    }
                }

                return value;
            }
        }

        /// <summary>
        /// Converts a duration in seconds into a sample count.
        /// </summary>
        /// <param name="samples">The sample count.</param>
        /// <returns>The number of seconds</returns>
        public long SecondsToSamples(decimal seconds)
        {
            return SecondsToSamples(Convert.ToDouble(seconds));
        }

        /// <summary>
        /// Converts a duration in seconds into a sample count.
        /// </summary>
        /// <param name="samples">The sample count.</param>
        /// <returns>The number of seconds</returns>
        public long SecondsToSamples(double seconds)
        {
            if (seconds < 0) return 0;

            lock (_cachedConversions)
            {
                if (_cachedConversions.Exists(c => c.Seconds == seconds))
                    return _cachedConversions.Where(c => c.Seconds == seconds).FirstOrDefault().Samples;
            }

            if (this.Channel == int.MinValue) return (long)((double)seconds / _samplesToSecondsRatio);
            else
            {
                var value = Bass.BASS_ChannelSeconds2Bytes(this.Channel, seconds);
                if (value == -1)
                {
                    value = GuessSamplesFromSeconds(seconds);
                }
                else if (_cachedConversions.Count < 512)
                {
                    lock (_cachedConversions)
                    {
                        _cachedConversions.Add(new SecondSampleConversion() { Seconds = seconds, Samples = value });
                    }
                }

                return value;
            }
        }

        private List<SecondSampleConversion> _cachedConversions = new List<SecondSampleConversion>();

        private class SecondSampleConversion
        {
            public double Seconds { get; set; }

            public long Samples { get; set; }
        }

        private long GuessSamplesFromSeconds(double seconds)
        {
            if (_cachedConversions.Count == 0) return 0;
            var conversion = _cachedConversions.Last();
            var samplesPerSecond = Convert.ToDouble(conversion.Samples) / conversion.Seconds;
            return Convert.ToInt64(seconds * samplesPerSecond);
        }

        private double GuessSecondsFromSamples(long samples)
        {
            if (_cachedConversions.Count == 0) return 0;
            var conversion = _cachedConversions.Last();
            var secondsPerSample = conversion.Seconds / Convert.ToDouble(conversion.Samples);
            return Convert.ToDouble(samples * secondsPerSample);
        }

        /// <summary>
        /// Formats the seconds as a string in a HH:MM:SS format.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns>A formatted string</returns>
        public string FormatSeconds(double seconds)
        {
            return Utils.FixTimespan(seconds, "HHMMSS");
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns> A string that represents this instance.</returns>
        public override string ToString()
        {
            return this.Filename;
        }

        internal System.Runtime.InteropServices.GCHandle AudioDataHandle { get; set; }

        internal byte[] AudioData { get; set; }

        internal IntPtr AudioDataPointer { get { return this.AudioDataHandle.AddrOfPinnedObject(); } }

        #endregion

        internal void ResetPowerDownOnEnd()
        {
            this.PowerDownOnEnd = PowerDownOnEndOriginal;
        }

        public bool IsAudioLoaded()
        {
            return this.Channel != int.MinValue;
        }
    }
}