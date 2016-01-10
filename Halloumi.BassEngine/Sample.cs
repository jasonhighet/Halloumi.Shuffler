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
    public class Sample
    {
        #region Private Variables

        /// <summary>
        /// The ratio used to convert samples to seconds for this sample
        /// </summary>
        private double _samplesToSecondsRatio = 0.001D;

        /// <summary>
        /// The BPM at the start of the song
        /// </summary>
        private decimal _bpm = -1;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the sample class.
        /// </summary>
        public Sample()
        {
            this.Channels = new List<int>();
            this.SampleEndSyncID = int.MinValue;
            this.Gain = 0;
            this.IsLooped = false;

            this.LinkedTrackDescription = "";
            this.SampleKey = "None";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ID for the sample.
        /// </summary>
        public int ID
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the bass channel ID for the track (set once the sample is loaded by the bass engine)
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
        /// Gets or sets the name of the mp3 file associated with the sample.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the sample description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the BPM.
        /// </summary>
        public decimal BPM
        {
            get
            {
                if (_bpm != -1) return _bpm;
                else return BassHelper.GetBPMFromLoopLength(this.LengthSeconds);
            }
            set
            {
                _bpm = value;
            }
        }

        /// <summary>
        /// Gets or sets the defaul volume level of the sample.
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Gets or sets the length of the sample in samples.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// Gets the length of the sample in seconds.
        /// </summary>
        public double LengthSeconds
        {
            get
            {
                return this.SamplesToSeconds(this.Length);
            }
        }

        /// <summary>
        /// Gets the length of the sample in seconds formatted as a string.
        /// </summary>
        public string LengthFormatted
        {
            get
            {
                return FormatSeconds(this.LengthSeconds);
            }
        }

        /// <summary>
        /// Gets or sets the gain (volume adjustment) for the sample
        /// </summary>
        public float Gain { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this sample is looped.
        /// </summary>
        public bool IsLooped { get; set; }

        /// <summary>
        /// Gets or sets the default sample rate.
        /// </summary>
        public int DefaultSampleRate
        {
            get;
            internal set;
        }

        /// <summary>
        /// Delegate for the method called when the sync events are fired
        /// </summary>
        internal SYNCPROC SampleSync = null;

        /// <summary>
        /// Gets or sets the sample end sync handle
        /// </summary>
        internal int SampleEndSyncID { get; set; }

        public string LinkedTrackDescription { get; set; }

        //public BassPlayer.TrackSampleType TrackSampleType { get; set; }

        public string SampleKey { get; set; }

        public string SampleID
        {
            get { return this.LinkedTrackDescription + " - " + this.SampleKey; }
        }

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
            return this.Description;
        }

        internal System.Runtime.InteropServices.GCHandle AudioDataHandle { get; set; }

        internal byte[] AudioData { get; set; }

        internal IntPtr AudioDataPointer { get { return this.AudioDataHandle.AddrOfPinnedObject(); } }

        #endregion
    }
}