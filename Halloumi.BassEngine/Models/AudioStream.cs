using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Players;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Models
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public abstract class AudioStream
    {
        /// <summary>
        ///     The ratio used to convert samples to seconds for this track
        /// </summary>
        private const double SamplesToSecondsRatio = 0.001D;

        private readonly List<SecondSampleConversion> _cachedConversions = new List<SecondSampleConversion>();

        /// <summary>
        ///     Delegate for the method called when the sync events are fired
        /// </summary>
        internal SYNCPROC SyncProc = null;

        protected AudioStream()
        {
            AudioSyncs = new List<AudioSync>();
            Channels = new List<int>();
            Gain = 0;
            GainChannel = int.MinValue;
        }

        public AudioData AudioData { get; set; }

        /// <summary>
        ///     Gets or sets the Id for the track.
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        ///     Gets or sets the bass channel Id for the track (set once the track is loaded by the bass engine)
        /// </summary>
        public int Channel => Channels.Count == 0 ? int.MinValue : Channels[0];

        /// <summary>
        ///     Gets the BPM.
        /// </summary>
        public abstract decimal Bpm { get; set; }

        /// <summary>
        ///     Gets the description.
        /// </summary>
        public abstract string Description { get; set; }

        /// <summary>
        ///     Gets the channels.
        /// </summary>
        public List<int> Channels { get; }

        /// <summary>
        ///     Gets or sets the name of the mp3 file associated with the track.
        /// </summary>
        public string Filename { get; set; }

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
        ///     Gets or sets the gain (volume adjustment) for the track
        /// </summary>
        public float Gain { get; set; }

        /// <summary>
        ///     Gets or sets the default sample rate.
        /// </summary>
        /// <value>
        ///     The default sample rate.
        /// </value>
        public int DefaultSampleRate { get; set; }

        /// <summary>
        ///     Gets the audio syncs.
        /// </summary>
        public List<AudioSync> AudioSyncs { get; }

        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Gets or sets the gain channel for the track
        /// </summary>
        internal int GainChannel { get; set; }

        /// <summary>
        ///     Determines whether the audio is loaded.
        /// </summary>
        /// <returns></returns>
        public bool IsAudioLoaded()
        {
            return AudioData != null && Channel != int.MinValue;
        }

        /// <summary>
        ///     Adds a channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public void AddChannel(int channel)
        {
            Channels.Insert(0, channel);
        }

        /// <summary>
        ///     Determines whether the gain channel has been initialized.
        /// </summary>
        /// <returns>True if the gain channel has been initialized</returns>
        internal bool IsGainChannelInitialized()
        {
            return GainChannel == int.MinValue;
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