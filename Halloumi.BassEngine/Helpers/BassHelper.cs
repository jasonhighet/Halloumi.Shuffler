using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using Halloumi.BassEngine.Models;
using Halloumi.BassEngine.Properties;
using Halloumi.Common.Helpers;
using IdSharp.Tagging.ID3v2;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Enc;
using Un4seen.Bass.AddOn.Fx;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.Misc;

namespace Halloumi.BassEngine.Helpers
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public static class BassHelper
    {
        private const int DefaultSampleRate = 44100;

        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        private static readonly object Lock = new object();

        static BassHelper()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        ///     Gets a seconds value as formatted hh:mm:ss.ttt text
        /// </summary>
        /// <returns></returns>
        public static string GetFormattedSeconds(double seconds)
        {
            if (double.IsNaN(seconds)) return "";

            var timeSpan = TimeSpan.FromSeconds(seconds);
            return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}.{timeSpan.Milliseconds:D3}";
        }

        /// <summary>
        ///     Gets a seconds value as formatted hh:mm:ss.ttt text
        /// </summary>
        /// <returns></returns>
        public static string GetFormattedSecondsNoHours(double seconds)
        {
            if (double.IsNaN(seconds)) return "";

            var timeSpan = TimeSpan.FromSeconds(seconds);
            return $"{(timeSpan.Hours*60) + timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}.{timeSpan.Milliseconds:D3}";
        }

        /// <summary>
        ///     Gets a seconds value as formatted hh:mm:ss.ttt text
        /// </summary>
        /// <returns></returns>
        public static string GetShortFormattedSeconds(decimal seconds)
        {
            return GetShortFormattedSeconds(Convert.ToDouble(seconds));
        }

        /// <summary>
        ///     Gets a seconds value as formatted hh:mm:ss.ttt text
        /// </summary>
        /// <returns></returns>
        public static string GetShortFormattedSeconds(double seconds)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);
            return timeSpan.Hours < 1 ? $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}" : $"{timeSpan.Hours}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }

        /// <summary>
        ///     Determines whether BPM value is in a percentage range of another BPM value
        /// </summary>
        /// <param name="bpm1">The initial BPM.</param>
        /// <param name="bpm2">The BPM being matched.</param>
        /// <param name="percentVariance">The percent variance.</param>
        /// <returns>True if BPM2 is in range of BMP1</returns>
        public static bool IsBpmInRange(decimal bpm1, decimal bpm2, decimal percentVariance)
        {
            percentVariance = Math.Abs(percentVariance);
            return (GetAbsoluteBpmPercentChange(bpm1, bpm2) <= percentVariance);
        }

        /// <summary>
        ///     Gets the average of two BPMs. If one of the BPMs is close to double the other it is halved for averaging purposes.
        /// </summary>
        /// <param name="bpm1">The first BPM.</param>
        /// <param name="bpm2">The second BPM2.</param>
        /// <returns>The average BPM</returns>
        public static decimal GetAdjustedBpmAverage(decimal bpm1, decimal bpm2)
        {
            var bpms = new List<decimal>
            {
                bpm1,
                bpm2
            }
                .OrderBy(bpm => bpm)
                .ToList();

            var diff = GetAdjustedBpmPercentChange(bpms[0], bpms[1]);
            var multiplier = 1M + (diff/100);
            bpms[1] = bpms[0]*multiplier;
            return bpms.Average();
        }

        /// <summary>
        ///     Gets the BPM change between two values as percent (-100 to 100).
        /// </summary>
        /// <param name="bpm1">The first BPM.</param>
        /// <param name="bpm2">The second BPM2.</param>
        /// <returns>The BPM change as a percent (-100 - 100)</returns>
        public static decimal GetAdjustedBpmPercentChange(decimal bpm1, decimal bpm2)
        {
            if (bpm1 == 0M || bpm2 == 0M) return 100M;

            var percentChanges = new List<decimal>
            {
                GetBpmPercentChange(bpm1, bpm2),
                GetBpmPercentChange(bpm1, bpm2/2),
                GetBpmPercentChange(bpm1, bpm2*2)
            };

            var minPercentChange = percentChanges
                .OrderBy(Math.Abs)
                .ToList()[0];

            return minPercentChange;
        }

        /// <summary>
        ///     Gets the BPM change between two values as percent (-100 to 100).
        /// </summary>
        /// <param name="bpm1">The first BPM.</param>
        /// <param name="bpm2">The second BPM2.</param>
        /// <returns>The BPM change as a percent (-100 - 100)</returns>
        public static decimal GetBpmPercentChange(decimal bpm1, decimal bpm2)
        {
            if (bpm1 == 0M || bpm2 == 0M) return 100M;
            var bpmDiff = bpm2 - bpm1;
            var percentChange = (bpmDiff/bpm2)*100;

            return percentChange;
        }

        /// <summary>
        ///     Gets the absolute BPM change between two values as percent (0 - 100).
        /// </summary>
        /// <param name="bpm1">The first BPM.</param>
        /// <param name="bpm2">The second BPM2.</param>
        /// <returns>The BPM change as a percent (0 - 100)</returns>
        public static decimal GetAbsoluteBpmPercentChange(decimal bpm1, decimal bpm2)
        {
            return Math.Abs(GetAdjustedBpmPercentChange(bpm1, bpm2));
        }

        public static List<double> GetLoopLengths(decimal bpm)
        {
            if (bpm == 0) return new List<double>();

            var loopLengths = new List<double>();

            // scale BPM to be between 70 and 140
            bpm = NormaliseBpm(bpm);

            var bps = ((double) bpm)/60;
            var spb = 1/bps;

            loopLengths.Add(spb*4);
            loopLengths.Add(spb*8);
            loopLengths.Add(spb*16);
            loopLengths.Add(spb*32);
            loopLengths.Add(spb*64);

            return loopLengths;
        }

        /// <summary>
        ///     Gets the default length of the loop.
        /// </summary>
        /// <param name="bpm">The BPM.</param>
        /// <returns></returns>
        public static double GetDefaultLoopLength(decimal bpm)
        {
            return bpm == 0 ? 10 : GetLoopLengths(bpm)[2];
        }

        /// <summary>
        ///     Gets the default delay time. (1/4 note delay)
        /// </summary>
        /// <param name="bpm">The BPM.</param>
        /// <returns>The default delay time from the BPM (1/4 note delay)</returns>
        public static double GetDefaultDelayLength(decimal bpm)
        {
            bpm = NormaliseBpm(bpm);
            return (1D/((double) bpm/60D))*1000D;
        }

        /// <summary>
        ///     Gets the loop length for the specified BPM that is closest to the preferred length
        /// </summary>
        /// <param name="bpm">The BPM.</param>
        /// <param name="preferredLength">Preferred loop length.</param>
        /// <returns>A BPM loop length</returns>
        public static double GetBestFitLoopLength(decimal bpm, double preferredLength)
        {
            if (bpm == 0M) return preferredLength;

            var loopLengths = GetLoopLengths(bpm);
            var selectedLoopLengthIndex = 2;

            for (var i = 0; i < loopLengths.Count; i++)
            {
                var difference = Math.Abs(preferredLength - loopLengths[i]);
                var selectedIndexDifference = Math.Abs(preferredLength - loopLengths[selectedLoopLengthIndex]);
                if (difference < selectedIndexDifference)
                {
                    selectedLoopLengthIndex = i;
                }
            }
            return loopLengths[selectedLoopLengthIndex];
        }

        /// <summary>
        ///     Gets the BPM of loop.
        /// </summary>
        /// <param name="loopLength">Length of the loop in seconds.</param>
        /// <returns>The BPM of the loop</returns>
        public static decimal GetBpmFromLoopLength(double loopLength)
        {
            if (loopLength == 0) return 0;
            var spb = loopLength/16;
            var bps = 1/spb;
            var bpm = bps*60;

            return NormaliseBpm((decimal) bpm);
        }

        /// <summary>
        ///     Normalizes a BPM value by scaling it to be between 70 and 140
        /// </summary>
        /// <param name="bpm">The BPM.</param>
        /// <returns>The scaled BPM</returns>
        public static decimal NormaliseBpm(decimal bpm)
        {
            if (bpm == 0) bpm = 100;
            bpm = Math.Abs(bpm);

            const decimal upper = 136.5M;
            const decimal lower = upper/2;

            while (bpm < lower || bpm > upper)
            {
                if (bpm > upper) bpm = bpm/2;
                if (bpm < lower) bpm = bpm*2;
            }

            return bpm;
        }

        /// <summary>
        ///     Converts a decibel value to a percent value.
        /// </summary>
        /// <param name="decibel">The decibel.</param>
        /// <returns>The percent value</returns>
        public static float DecibelToPercent(float decibel)
        {
            return (float) (Math.Pow(10, 0.05*decibel));
        }

        /// <summary>
        ///     Sets current volume for the track as a percentage (0 - 1)
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="volume">The volume as a percentage (0 - 1).</param>
        public static void SetTrackVolume(Track track, float volume)
        {
            SetChannelVolume(track.Channel, volume);
        }

        /// <summary>
        ///     Sets current volume for the track as a percentage (0 - 100)
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="volume">The volume as a percentage (0 - 100).</param>
        public static void SetTrackVolume(Track track, decimal volume)
        {
            SetChannelVolume(track.Channel, volume);
        }

        /// <summary>
        ///     Sets current volume for the sample as a percentage (0 - 1)
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="volume">The volume.</param>
        public static void SetSampleVolume(Sample sample, float volume)
        {
            SetChannelVolume(sample.Channel, volume);
        }

        /// <summary>
        ///     Sets current volume for the sample as a percentage (0 - 100)
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="volume">The volume.</param>
        public static void SetSampleVolume(Sample sample, decimal volume)
        {
            SetChannelVolume(sample.Channel, volume);
        }

        /// <summary>
        ///     Sets current volume for the channel as a percentage (0 - 1)
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="volume">The volume.</param>
        private static void SetChannelVolume(int channel, float volume)
        {
            if (channel == int.MinValue) return;
            if (volume > 1 || volume < 0) throw new Exception("Volume not it range");

            DebugHelper.WriteLine($"SetChannelVolume {channel} {volume}...");
            Bass.BASS_ChannelSetAttribute(channel, BASSAttribute.BASS_ATTRIB_VOL, volume);
            DebugHelper.WriteLine("done");
        }

        /// <summary>
        ///     Sets current volume for the channel as a percentage (0 - 100)
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="volume">The volume.</param>
        private static void SetChannelVolume(int channel, decimal volume)
        {
            if (volume > 100 || volume < 0) throw new Exception("Volume not it range");
            SetChannelVolume(channel, (float) (volume/100));
        }

        /// <summary>
        ///     Gets the track volume.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>The tracks volume</returns>
        public static decimal GetTrackVolume(Track track)
        {
            if (track == null || !track.IsAudioLoaded()) return 0;
            return GetChannelVolume(track.Channel);
        }

        private static decimal GetChannelVolume(int channel)
        {
            float volume = 0;
            DebugHelper.WriteLine($"GetChannelVolume {channel}...");
            Bass.BASS_ChannelGetAttribute(channel, BASSAttribute.BASS_ATTRIB_VOL, ref volume);
            DebugHelper.WriteLine("done");
            return Convert.ToDecimal(volume*100);
        }

        /// <summary>
        ///     Gets the sample volume.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <returns>The samples volume</returns>
        public static decimal GetSampleVolume(Sample sample)
        {
            if (sample == null || sample.Channel == int.MinValue) return 0;
            return GetChannelVolume(sample.Channel);
        }

        /// <summary>
        ///     Sets the duration and start/end volumes for a track volume slide.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="startVolume">The start volume.</param>
        /// <param name="endVolume">The end volume.</param>
        /// <param name="seconds">The seconds.</param>
        public static void SetTrackVolumeSlide(Track track, float startVolume, float endVolume, double seconds)
        {
            if (track == null || !track.IsAudioLoaded()) return;

            // set start volume
            SetTrackVolume(track, startVolume);

            var miliseconds = (int) (seconds*1000);

            // set the volume slide
            Bass.BASS_ChannelSlideAttribute(track.Channel, BASSAttribute.BASS_ATTRIB_VOL, endVolume, miliseconds);
        }

        /// <summary>
        ///     Sets the duration and start/end volumes for a track volume slide.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="startVolume">The start volume.</param>
        /// <param name="endVolume">The end volume.</param>
        /// <param name="sampleDuration">Sample length duration.</param>
        public static void SetTrackVolumeSlide(Track track, float startVolume, float endVolume, long sampleDuration)
        {
            if (track == null || !track.IsAudioLoaded()) return;

            var seconds = track.SamplesToSeconds(sampleDuration);
            SetTrackVolumeSlide(track, startVolume, endVolume, seconds);
        }

        /// <summary>
        ///     Sets the duration and start/end volumes for a sample1 volume slide.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="startVolume">The start volume.</param>
        /// <param name="endVolume">The end volume.</param>
        /// <param name="seconds">The seconds.</param>
        public static void SetSampleVolumeSlide(Sample sample, float startVolume, float endVolume, double seconds)
        {
            if (sample == null || sample.Channel == int.MinValue) return;

            // set start volume
            SetSampleVolume(sample, startVolume);

            var miliseconds = (int) (seconds*1000);

            // set the volume slide
            if (sample.Channel == int.MinValue) return;
            Bass.BASS_ChannelSlideAttribute(sample.Channel, BASSAttribute.BASS_ATTRIB_VOL, endVolume, miliseconds);
        }

        /// <summary>
        ///     Gets the track sample rate.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>The track sample rate</returns>
        public static int GetTrackSampleRate(Track track)
        {
            return GetSampleRate(track.Channel);
        }

        /// <summary>
        ///     Gets the track sample rate.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>
        ///     The track sample rate
        /// </returns>
        public static int GetSampleRate(int channel)
        {
            if (channel == int.MinValue) return 0;

            float trackSampleRate = DefaultSampleRate;
            Bass.BASS_ChannelGetAttribute(channel, BASSAttribute.BASS_ATTRIB_FREQ, ref trackSampleRate);
            return (int) trackSampleRate;
        }

        /// <summary>
        ///     Sets the track pitch, based on a percent pitch value (0 - 200, 100 being 'normal' pitch)
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="pitch">The pitch.</param>
        public static void SetTrackPitch(Track track, double pitch)
        {
            if (track == null || !track.IsAudioLoaded()) return;

            DebugHelper.WriteLine("SetTrackPitch");

            float sampleRate = track.DefaultSampleRate;
            Bass.BASS_ChannelSetAttribute(track.Channel, BASSAttribute.BASS_ATTRIB_TEMPO_FREQ, sampleRate);
        }

        /// <summary>
        ///     Sets the sample pitch, based on a percent pitch value (0 - 200, 00 being 'normal' pitch)
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="pitch">The pitch.</param>
        public static void SetSamplePitch(Track sample, double pitch)
        {
            if (sample == null || sample.Channel == int.MinValue) return;

            DebugHelper.WriteLine("SetSamplePitch");
            float sampleRate = sample.DefaultSampleRate;
            Bass.BASS_ChannelSetAttribute(sample.Channel, BASSAttribute.BASS_ATTRIB_TEMPO_FREQ, sampleRate);
        }

        /// <summary>
        ///     Sets the track pitch to match another track's BPM
        /// </summary>
        /// <param name="changeTrack">The track to change the pitch of.</param>
        /// <param name="matchTrack">The track to match the BPM of</param>
        public static void SetTrackPitchToMatchAnotherTrack(Track changeTrack, Track matchTrack)
        {
            if (changeTrack == null || matchTrack == null) return;
            if (!changeTrack.IsAudioLoaded()) return;
            if (!matchTrack.IsAudioLoaded()) return;

            var sampleRate = GetTrackTempoChangeAsSampleRate(changeTrack, matchTrack);
            Bass.BASS_ChannelSetAttribute(changeTrack.Channel, BASSAttribute.BASS_ATTRIB_TEMPO_FREQ, sampleRate);
        }

        /// <summary>
        ///     Sets the track tempo to match another track's tempo
        /// </summary>
        /// <param name="changeTrack">The track to change the temp of.</param>
        /// <param name="matchTrack">The track to match the BPM of</param>
        public static void SetTrackTempoToMatchAnotherTrack(Track changeTrack, Track matchTrack)
        {
            if (changeTrack == null || matchTrack == null) return;
            if (!changeTrack.IsAudioLoaded()) return;
            if (!matchTrack.IsAudioLoaded()) return;

            var percentChange = (float) (GetAdjustedBpmPercentChange(changeTrack.EndBpm, matchTrack.StartBpm));
            Bass.BASS_ChannelSetAttribute(changeTrack.Channel, BASSAttribute.BASS_ATTRIB_TEMPO, percentChange);
        }

        /// <summary>
        ///     Resets the track tempo.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void ResetTrackTempo(Track track)
        {
            if (track == null || !track.IsAudioLoaded()) return;

            Bass.BASS_ChannelSetAttribute(track.Channel, BASSAttribute.BASS_ATTRIB_TEMPO, 0F);
        }

        /// <summary>
        /// Sets a sample tempo to match another BPM
        /// </summary>
        /// <param name="sample">The sample to change the temp of.</param>
        /// <param name="matchBpm">The match BPM.</param>
        public static void SetSampleTempoToMatchBpm(Sample sample, decimal matchBpm)
        {
            if (sample == null || sample.Channel == int.MinValue) return;

            var percentChange = (float) (GetAdjustedBpmPercentChange(sample.Bpm, matchBpm));
            Bass.BASS_ChannelSetAttribute(sample.Channel, BASSAttribute.BASS_ATTRIB_TEMPO, percentChange);
        }

        /// <summary>
        ///     Resets the sample tempo.
        /// </summary>
        /// <param name="sample">The sample.</param>
        public static void ResetSampleTempo(Sample sample)
        {
            if (sample == null || sample.Channel == int.MinValue) return;

            Bass.BASS_ChannelSetAttribute(sample.Channel, BASSAttribute.BASS_ATTRIB_TEMPO, 0F);
        }

        /// <summary>
        ///     Sets the track position
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="samplePosition">The sample position.</param>
        public static void SetTrackPosition(Track track, long samplePosition)
        {
            if (track == null || !track.IsAudioLoaded()) return;
            if (samplePosition < 0 || samplePosition > track.Length) return;

            DebugHelper.WriteLine(
                $"SetTrackPosition {track.Description} {track.Channel} {samplePosition} {track.Length}...");
            Bass.BASS_ChannelSetPosition(track.Channel, samplePosition);
            DebugHelper.WriteLine("done");
        }

        /// <summary>
        ///     Gets the current track position.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>The current track position</returns>
        public static long GetTrackPosition(Track track)
        {
            if (track == null) return 0;
            return !track.IsAudioLoaded() ? 0 : Bass.BASS_ChannelGetPosition(track.Channel);
        }

        /// <summary>
        ///     Pause a track
        /// </summary>
        /// <param name="track">The track.</param>
        public static void TrackPause(Track track)
        {
            if (track == null || !track.IsAudioLoaded()) return;
            BassMix.BASS_Mixer_ChannelPause(track.Channel);
        }

        /// <summary>
        ///     Pauses a track smoothly
        /// </summary>
        /// <param name="track">The track.</param>
        public static void TrackSmoothPause(Track track)
        {
            var smoothPauseAction = new Action<Track>(TrackSmoothPauseAsync);
            smoothPauseAction.BeginInvoke(track, null, null);
        }

        /// <summary>
        ///     Does the track power down effect asynchronously
        /// </summary>
        /// <param name="track">The track.</param>
        private static void TrackSmoothPauseAsync(Track track)
        {
            if (track == null || !track.IsAudioLoaded()) return;

            var volume = ((float) GetTrackVolume(track))/100F;
            SetTrackVolumeSlide(track, volume, 0F, 0.15D);
            Thread.Sleep(150);

            if (!track.IsAudioLoaded()) return;
            BassMix.BASS_Mixer_ChannelPause(track.Channel);

            SetTrackVolume(track, volume);
        }

        /// <summary>
        ///     Plays a track
        /// </summary>
        /// <param name="track">The track.</param>
        public static void TrackPlay(Track track)
        {
            if (track == null || !track.IsAudioLoaded()) return;

            lock (Lock)
            {
                DebugHelper.WriteLine("Track Play (" + track.Description + ")");
                BassMix.BASS_Mixer_ChannelPlay(track.Channel);
                DebugHelper.WriteLine("Track Playing (" + track.Description + ")");
            }
        }

        /// <summary>
        ///     Determines whether a track is currently playing.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>
        ///     True if the track is playing; otherwise, false.
        /// </returns>
        public static bool IsTrackPlaying(Track track)
        {
            if (track == null) return false;
            var position1 = GetTrackPosition(track);
            Thread.Sleep(50);
            var position2 = GetTrackPosition(track);
            return (position1 != position2);
        }

        public static double GetFullEndLoopLengthAdjustedToMatchAnotherTrack(Track track1, Track track2)
        {
            if (track1 == null && track2 == null) return 10d;
            if (track2 == null) return track1.FullEndLoopLengthSeconds;
            return track1 == null 
                ? track2.FullEndLoopLengthSeconds 
                : GetLengthAdjustedToMatchAnotherTrack(track1, track2, track1.FullEndLoopLengthSeconds);
        }

        public static double GetLengthAdjustedToMatchAnotherTrack(Track track1, Track track2, double length)
        {
            if (track1 == null || track2 == null) return length;
            var ratio = GetTrackTempoChangeAsRatio(track2, track1);
            return length*ratio;
        }

        /// <summary>
        ///     Gets the track tempo change as a ratio (i.e. 1.02, .97 etc)
        /// </summary>
        /// <param name="track1">The track being fading out</param>
        /// <param name="track2">The track being faded into.</param>
        /// <returns>The ratio the first track needs to be multiplied by to in order to match the second track</returns>
        private static float GetTrackTempoChangeAsRatio(Track track1, Track track2)
        {
            if (track1 == null || track2 == null) return 1f;

            var percentChange = (float) (GetAdjustedBpmPercentChange(track1.EndBpm, track2.StartBpm));

            return (1 + percentChange/100f);
        }

        /// <summary>
        ///     Gets the track tempo change as a sample rate
        /// </summary>
        /// <param name="track1">The track being fading out</param>
        /// <param name="track2">The track being faded into.</param>
        /// <returns>The sample rate the first track needs to be changed to in order to match the second track</returns>
        private static float GetTrackTempoChangeAsSampleRate(Track track1, Track track2)
        {
            if (track1 == null || track2 == null) return DefaultSampleRate;

            return track1.DefaultSampleRate*GetTrackTempoChangeAsRatio(track1, track2);
        }

        /// <summary>
        ///     Initialises the Bass audio engine.
        /// </summary>
        public static void InitialiseBassEngine(IntPtr windowHandle)
        {
            if (!Bass.BASS_Init(-1, DefaultSampleRate, BASSInit.BASS_DEVICE_DEFAULT, windowHandle))
            {
                throw new Exception("Cannot create Bass Engine.");
            }
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, 200);
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 20);
        }

        /// <summary>
        ///     Initialises the monitor device.
        /// </summary>
        /// <param name="monitorDeviceId">The monitor device Id.</param>
        public static void InitialiseMonitorDevice(int monitorDeviceId)
        {
            if (GetWaveOutDevices().Count < 3) return;

            if (!Bass.BASS_Init(monitorDeviceId, DefaultSampleRate, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                //throw new Exception("Cannot initialize Monitor device.");
            }
        }

        /// <summary>
        ///     Initialises the mixer channel.
        /// </summary>
        /// <returns>The channel Id of the mixer channel</returns>
        public static int IntialiseOutputChannel()
        {
            var mixerChannel = BassMix.BASS_Mixer_StreamCreate(DefaultSampleRate, 2, BASSFlag.BASS_SAMPLE_FLOAT);
            if (mixerChannel == 0) throw new Exception("Cannot create Bass Mixer.");
            return mixerChannel;
        }

        /// <summary>
        ///     Initialises a decoder mixer channel.
        /// </summary>
        /// <returns>The channel Id of the mixer channel</returns>
        public static int IntialiseMixerChannel()
        {
            var mixerChannel = BassMix.BASS_Mixer_StreamCreate(DefaultSampleRate, 2,
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE);
            if (mixerChannel == 0) throw new Exception("Cannot create Bass Mixer.");
            return mixerChannel;
        }

        public static void AddChannelToDecoderMixer(int mixerChannel, int channel)
        {
            BassMix.BASS_Mixer_StreamAddChannel(mixerChannel, channel, BASSFlag.BASS_STREAM_DECODE);
        }

        public static int SplitDecoderMixer(int mixerChannel)
        {
            return BassMix.BASS_Split_StreamCreate(mixerChannel, BASSFlag.BASS_STREAM_DECODE, null);
        }

        /// <summary>
        ///     Initialises a mono decoder mixer channel.
        /// </summary>
        /// <returns>The channel Id of the mixer channel</returns>
        public static int IntialiseMonoDecoderMixerChannel()
        {
            var mixerChannel = BassMix.BASS_Mixer_StreamCreate(DefaultSampleRate, 1,
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE);
            if (mixerChannel == 0) throw new Exception("Cannot create Bass Mixer.");
            return mixerChannel;
        }

        /// <summary>
        ///     Picks a random track from a list of tracks
        /// </summary>
        /// <param name="tracks">The track list.</param>
        /// <returns>A randomly selected track</returns>
        public static Track GetRandomTrack(List<Track> tracks)
        {
            if (tracks == null) return null;
            return tracks.Count == 0 ? null : tracks[Random.Next(0, tracks.Count)];
        }

        /// <summary>
        ///     Plays a 'power down' effect on a track.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void TrackPowerDown(Track track)
        {
            var powerDownAction = new Action<Track>(TrackPowerDownAsync);
            powerDownAction.BeginInvoke(track, null, null);
        }

        /// <summary>
        ///     Does the track power down effect asynchronously
        /// </summary>
        /// <param name="track">The track.</param>
        private static void TrackPowerDownAsync(Track track)
        {
            if (track == null || !track.IsAudioLoaded()) return;

            var freq = track.DefaultSampleRate;
            var interval = (int) (GetDefaultLoopLength(track.EndBpm)*1000)/128;

            // set the volume slide
            Bass.BASS_ChannelSlideAttribute(track.Channel, BASSAttribute.BASS_ATTRIB_VOL, 0F, interval*8);

            var percentValue = 0.70;
            while (freq > 100)
            {
                percentValue = percentValue/1.2;
                interval = (int) (interval*0.9D);
                freq = (int) (track.DefaultSampleRate*percentValue);
                if (freq <= 100 || track.Channel == int.MinValue) continue;
                Bass.BASS_ChannelSlideAttribute(track.Channel, BASSAttribute.BASS_ATTRIB_FREQ, freq, interval);
                Thread.Sleep(interval);
            }
            TrackPause(track);
            if (!track.IsAudioLoaded()) return;
            Bass.BASS_ChannelSetAttribute(track.Channel, BASSAttribute.BASS_ATTRIB_FREQ, track.DefaultSampleRate);
            SetTrackVolume(track, 100M);
        }

        /// <summary>
        ///     Gets the length of the track.
        /// </summary>
        /// <param name="filename">The filename of the track.</param>
        /// <returns>The length of the track</returns>
        public static double GetTrackLength(string filename)
        {
            var channel = Bass.BASS_StreamCreateFile(filename, 0L, 0L,
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN);
            if (channel == 0) throw new Exception("Cannot load track " + filename);
            var length = Bass.BASS_ChannelBytes2Seconds(channel, Bass.BASS_ChannelGetLength(channel));
            Bass.BASS_StreamFree(channel);
            return length;
        }

        /// <summary>
        ///     Sets the sample replay gain.
        /// </summary>
        /// <param name="sample">The sample.</param>
        public static void SetSampleReplayGain(Sample sample)
        {
            SetReplayGain(sample.Channel, sample.Gain);
        }

        /// <summary>
        ///     Sets the track replay gain.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void SetTrackReplayGain(Track track)
        {
            SetReplayGain(track.Channel, track.Gain);
        }

        /// <summary>
        /// Sets the replay gain for a channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="gain">The gain.</param>
        public static void SetReplayGain(int channel, float gain)
        {
            if (gain == 0) return;
            if (channel == int.MinValue) return;

            DebugHelper.WriteLine("SetReplayGain " + gain);

            var fxChannel = Bass.BASS_ChannelSetFX(channel, BASSFXType.BASS_FX_BFX_VOLUME, int.MaxValue);
            var volume = DecibelToPercent(gain);
            var volumeParameters = new BASS_BFX_VOLUME(volume, BASSFXChan.BASS_BFX_CHANALL);
            Bass.BASS_FXSetParameters(fxChannel, volumeParameters);
        }

        /// <summary>
        ///     Loads the track image.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void LoadTrackImage(Track track)
        {
            if (track.Image != null) return;

            track.Image = Resources.DefaultMusicImage;
            var tags = ID3v2Helper.CreateID3v2(track.Filename);
            if (tags.PictureList.Count <= 0) return;

            try
            {
                var picture = tags.PictureList[0];
                using (var stream = new MemoryStream(picture.PictureData))
                {
                    track.Image = Image.FromStream(stream);
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Saves the track BPM tag.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void SaveTrackBpmTag(Track track)
        {
            var tags = ID3v2Helper.CreateID3v2(track.Filename);
            tags.BPM = track.TagBpm.ToString(CultureInfo.InvariantCulture);
            tags.Save(track.Filename);
        }

        /// <summary>
        ///     Sets the length of the track.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void SetTrackLength(Track track)
        {
            if (track == null || !track.IsAudioLoaded()) return;
            track.Length = Bass.BASS_ChannelGetLength(track.Channel);
        }

        /// <summary>
        ///     Saves a portion of a track as a wave file
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="outFilename">The output filename.</param>
        /// <param name="start">The start position in samples.</param>
        /// <param name="length">The length in samples.</param>
        public static void SavePartialAsWave(Track track, string outFilename, long start, long length)
        {
            SavePartialAsWave(track, outFilename, start, length, 0M);
        }

        /// <summary>
        ///     Saves a portion of an audio file as a wave file
        /// </summary>
        /// <param name="inFilename">The input filename.</param>
        /// <param name="outFilename">The output filename.</param>
        /// <param name="start">The start position in samples.</param>
        /// <param name="length">The length in samples.</param>
        public static void SavePartialAsWave(string inFilename, string outFilename, long start, long length)
        {
            var encoder = new EncoderWAV(0) {WAV_BitsPerSample = 16};
            BaseEncoder.EncodeFile(inFilename, outFilename, encoder, null, true, false, false, start, start + length);
        }

        /// <summary>
        ///     Saves a portion of a track as a wave file
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="outFilename">The output filename.</param>
        /// <param name="start">The start position in samples.</param>
        /// <param name="length">The length in samples.</param>
        /// <param name="bmpAdjustPercent">The BMP adjustment percent.</param>
        private static void SavePartialAsWave(Track track, string outFilename, long start, long length,
            decimal bmpAdjustPercent)
        {
            DebugHelper.WriteLine("Saving portion of track as wave - " + track.Description);

            var channel = Bass.BASS_StreamCreateFile(track.Filename, 0L, 0L,
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN);
            if (channel == 0) throw new Exception("Cannot load track " + track.Filename);

            if (bmpAdjustPercent != 0)
            {
                Bass.BASS_ChannelSetAttribute(channel, BASSAttribute.BASS_ATTRIB_TEMPO, (float) bmpAdjustPercent);
            }

            const BASSEncode flags = BASSEncode.BASS_ENCODE_PCM;
            BassEnc.BASS_Encode_Start(channel, outFilename, flags, null, IntPtr.Zero);

            var startByte = start;
            var endByte = start + length;

            TransferBytes(channel, startByte, endByte);
            BassEnc.BASS_Encode_Stop(channel);

            Bass.BASS_StreamFree(channel);
        }

        private static void TransferBytes(int channel, long startByte, long endByte)
        {
            var totalTransferLength = endByte - startByte;

            Bass.BASS_ChannelSetPosition(channel, startByte, BASSMode.BASS_POS_BYTES);
            while (totalTransferLength > 0)
            {
                var buffer = new byte[65536];

                var transferLength = totalTransferLength;
                if (transferLength > buffer.Length) transferLength = buffer.Length;

                // get the decoded sample data
                var transferred = Bass.BASS_ChannelGetData(channel, buffer, (int) transferLength);

                if (transferred < 1) break; // error or the end
                totalTransferLength -= transferred;
            }
        }

        public static void SavePartialAsWave(Track track, string outFilename, long start, long length, long offset,
            float gain)
        {
            DebugHelper.WriteLine("Saving portion of track as wave with offset - " + track.Description);

            var channel = Bass.BASS_StreamCreateFile(track.Filename, 0L, 0L,
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN);
            if (channel == 0) throw new Exception("Cannot load track " + track.Filename);

            if (gain > 0)
                SetReplayGain(channel, gain);

            const BASSEncode flags = BASSEncode.BASS_ENCODE_PCM;
            BassEnc.BASS_Encode_Start(channel, outFilename, flags, null, IntPtr.Zero);

            var startByte = start;
            var endByte = start + length;
            if (offset == 0 || offset == start)
            {
                TransferBytes(channel, startByte, endByte);
            }
            else
            {
                startByte = offset;
                TransferBytes(channel, startByte, endByte);

                startByte = start;
                endByte = offset;
                TransferBytes(channel, startByte, endByte);
            }

            BassEnc.BASS_Encode_Stop(channel);

            Bass.BASS_StreamFree(channel);
        }

        /// <summary>
        ///     Saves as wave.
        /// </summary>
        /// <param name="audioData">The audio data.</param>
        /// <param name="outFilename">The out filename.</param>
        public static void SaveAsWave(byte[] audioData, string outFilename)
        {
            var audioDataHandle = GCHandle.Alloc(audioData, GCHandleType.Pinned);
            var audioDataPointer = audioDataHandle.AddrOfPinnedObject();

            var channel = Bass.BASS_StreamCreateFile(audioDataPointer, 0, audioData.Length,
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN);
            if (channel == 0) throw new Exception("Cannot load audio data");

            const BASSEncode flags = BASSEncode.BASS_ENCODE_PCM;
            BassEnc.BASS_Encode_Start(channel, outFilename, flags, null, IntPtr.Zero);

            const int startByte = 0;
            var endByte = Bass.BASS_ChannelBytes2Seconds(channel, Bass.BASS_ChannelGetLength(channel));

            var totalTransferLength = endByte - startByte;

            Bass.BASS_ChannelSetPosition(channel, startByte, BASSMode.BASS_POS_BYTES);
            while (totalTransferLength > 0)
            {
                var buffer = new byte[65536];

                var transferLength = totalTransferLength;
                if (transferLength > buffer.Length) transferLength = buffer.Length;

                // get the decoded sample data
                var transferred = Bass.BASS_ChannelGetData(channel, buffer, (int) transferLength);

                if (transferred <= 1) break; // error or the end
                totalTransferLength -= transferred;
            }
            BassEnc.BASS_Encode_Stop(channel);

            Bass.BASS_StreamFree(channel);
            audioDataHandle.Free();
        }

        /// <summary>
        ///     Saves an audio file as a mono wave.
        /// </summary>
        /// <param name="inFilename">The input filename.</param>
        /// <param name="outFilename">The output filename.</param>
        public static void SaveAsMonoWave(string inFilename, string outFilename)
        {
            SaveAsMonoWave(inFilename, outFilename, 0, 0);
        }

        /// <summary>
        /// Saves an audio file as a mono wave.
        /// </summary>
        /// <param name="inFilename">The input filename.</param>
        /// <param name="outFilename">The output filename.</param>
        /// <param name="gain">The gain.</param>
        public static void SaveAsMonoWave(string inFilename, string outFilename, float gain)
        {
            SaveAsMonoWave(inFilename, outFilename, 0, gain);
        }

        /// <summary>
        /// Saves an audio file as a mono wave.
        /// </summary>
        /// <param name="inFilename">The input filename.</param>
        /// <param name="outFilename">The output filename.</param>
        /// <param name="length">The maximum length in seconds, or 0 for no limit.</param>
        /// <param name="gain">The gain.</param>
        public static void SaveAsMonoWave(string inFilename, string outFilename, double length, float gain)
        {
            var audioData = File.ReadAllBytes(inFilename);
            SaveAsMonoWave(audioData, outFilename, length, gain);
        }

        /// <summary>
        /// Saves audio data as a mono wave.
        /// </summary>
        /// <param name="audioData">The audio data.</param>
        /// <param name="outFilename">The output filename.</param>
        public static void SaveAsMonoWave(byte[] audioData, string outFilename)
        {
            SaveAsMonoWave(audioData, outFilename, 0, 0);
        }

        /// <summary>
        /// Saves audio data as a mono wave.
        /// </summary>
        /// <param name="audioData">The audio data.</param>
        /// <param name="outFilename">The output filename.</param>
        /// <param name="length">The maximum length in seconds, or 0 for no limit.</param>
        /// <param name="gain">The gain.</param>
        /// <exception cref="System.Exception">Cannot load audio data</exception>
        public static void SaveAsMonoWave(byte[] audioData, string outFilename, double length, float gain)
        {
            DebugHelper.WriteLine("SaveAsMonoWave");

            var audioDataHandle = GCHandle.Alloc(audioData, GCHandleType.Pinned);
            var audioDataPointer = audioDataHandle.AddrOfPinnedObject();

            var channel = Bass.BASS_StreamCreateFile(audioDataPointer, 0, audioData.Length,
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN);
            if (channel == 0) throw new Exception("Cannot load audio data");

            // create a mono 44100Hz mixer
            var mixer = BassMix.BASS_Mixer_StreamCreate(44100, 1, BASSFlag.BASS_MIXER_END | BASSFlag.BASS_STREAM_DECODE);

            // plug in the source
            BassMix.BASS_Mixer_StreamAddChannel(mixer, channel,
                BASSFlag.BASS_MIXER_DOWNMIX | BASSFlag.BASS_MIXER_NORAMPIN);

            SetReplayGain(mixer, gain);

            const BASSEncode flags = BASSEncode.BASS_ENCODE_PCM;
            BassEnc.BASS_Encode_Start(mixer, outFilename, flags, null, IntPtr.Zero);

            const int startByte = 0;

            if (length == 0) length = Bass.BASS_ChannelBytes2Seconds(channel, Bass.BASS_ChannelGetLength(channel));

            var totalTransferLength = Bass.BASS_ChannelSeconds2Bytes(mixer, length);

            Bass.BASS_ChannelSetPosition(channel, startByte, BASSMode.BASS_POS_BYTES);
            while (totalTransferLength > 0)
            {
                var buffer = new byte[65536];

                var transferLength = totalTransferLength;
                if (transferLength > buffer.Length) transferLength = buffer.Length;

                // get the decoded sample data
                var transferred = Bass.BASS_ChannelGetData(mixer, buffer, (int) transferLength);

                if (transferred <= 1) break; // error or the end
                totalTransferLength -= transferred;
            }
            BassEnc.BASS_Encode_Stop(mixer);

            BassMix.BASS_Mixer_ChannelRemove(channel);
            Bass.BASS_StreamFree(channel);
            Bass.BASS_StreamFree(mixer);

            audioDataHandle.Free();

            DebugHelper.WriteLine("END SaveAsMonoWave");
        }

        /// <summary>
        ///     Saves an audio file as a wave.
        /// </summary>
        /// <param name="inFilename">The input filename.</param>
        /// <param name="outFilename">The output filename.</param>
        public static void SaveAsWave(string inFilename, string outFilename)
        {
            var encoder = new EncoderWAV(0) {WAV_BitsPerSample = 16};
            BaseEncoder.EncodeFile(inFilename, outFilename, encoder, null, true, false);
        }

        /// <summary>
        ///     Saves a track as wave file.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="outFilename">The output filename.</param>
        public static void SaveAsWave(Track track, string outFilename)
        {
            SaveAsWave(track.Filename, outFilename);
        }

        /// <summary>
        ///     Guesses the artist and title of a track from its filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>A guess at the artist and filename</returns>
        public static TrackDetails GuessTrackDetailsFromFilename(string filename)
        {
            filename = (Path.GetFileNameWithoutExtension(filename) + "").Replace("_", " ").Trim();
            var elements = filename.Split('-').ToList();

            var trackDetails = new TrackDetails
            {
                AlbumArtist = "",
                Artist = "",
                Title = "",
                Description = ""
            };

            if (elements.Count > 3) for (var i = 3; i < elements.Count; i++) elements[2] += "-" + elements[i];

            switch (elements.Count)
            {
                case 1:
                    trackDetails.Title = elements[0].Trim();
                    break;
                case 2:
                    trackDetails.Artist = elements[0].Trim();
                    trackDetails.Title = elements[1].Trim();
                    break;
                case 3:
                    int trackNumber;
                    if (int.TryParse(elements[0], out trackNumber))
                    {
                        trackDetails.Artist = elements[1].Trim();
                        trackDetails.Title = elements[2].Trim();
                        trackDetails.TrackNumber = trackNumber.ToString();
                    }
                    else
                    {
                        trackDetails.Artist = elements[0].Trim();
                        trackDetails.Title = (elements[1] + "-" + elements[2]).Trim();
                    }
                    break;
            }

            trackDetails.AlbumArtist = trackDetails.Artist;
            if (trackDetails.Artist.ToLower().StartsWith("various") || trackDetails.Title.Contains("  "))
            {
                trackDetails.Title = trackDetails.Title.Replace("  ", "/");
                elements = trackDetails.Title.Split('/').ToList();
                if (elements.Count == 2)
                {
                    trackDetails.Artist = elements[0].Trim();
                    trackDetails.Title = elements[1].Trim();
                }
            }

            trackDetails.Description = GuessTrackDescription(filename, trackDetails.Artist, trackDetails.Title);

            return trackDetails;
        }

        /// <summary>
        ///     Guesses the track description.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="artist">The artist.</param>
        /// <param name="title">The title.</param>
        /// <returns>The track description</returns>
        public static string GuessTrackDescription(string filename, string artist, string title)
        {
            string description;

            if (artist != "" && title != "")
            {
                description = $"{artist} - {title}";
            }
            else
            {
                description = filename.Trim();

                var regex = new Regex("various artists", RegexOptions.IgnoreCase);
                description = regex.Replace(description, "");

                regex = new Regex("various artist", RegexOptions.IgnoreCase);
                description = regex.Replace(description, "");

                regex = new Regex("various", RegexOptions.IgnoreCase);
                description = regex.Replace(description, "");

                regex = new Regex("[0-9]+", RegexOptions.IgnoreCase);
                description = regex.Replace(description, "");

                description = description.Replace("_", " ");
                description = description.Replace(".", " ");

                description = StringHelper.TitleCase(description.Trim());
            }

            return description;
        }

        /// <summary>
        ///     Gets the formatted length
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>The formatted length</returns>
        public static string GetFormattedLength(double length)
        {
            var timespan = TimeSpan.FromSeconds(length);
            var lengthFormatted = $"{timespan.Minutes}:{timespan.Seconds:D2}";
            if (length > 60*60)
            {
                lengthFormatted = $"{timespan.Hours}:{timespan.Minutes:D2}:{timespan.Seconds:D2}";
            }
            return lengthFormatted;
        }

        /// <summary>
        ///     Gets the formatted length
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>The formatted length</returns>
        public static string GetFormattedLength(decimal length)
        {
            return GetFormattedLength((double) length);
        }

        /// <summary>
        ///     Gets the volume of a channel as a value between 0 and 100.
        /// </summary>
        /// <returns>A value between 0 and 100</returns>
        public static decimal GetVolume(int channel)
        {
            if (channel == int.MinValue) return 0;

            float volume = 0;
            Bass.BASS_ChannelGetAttribute(channel, BASSAttribute.BASS_ATTRIB_VOL, ref volume);
            return (decimal) (volume*100);
        }

        /// <summary>
        ///     Sets the volume of a channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="volume">The volume as a value between 0 and 100.</param>
        public static void SetVolume(int channel, decimal volume)
        {
            if (volume < 0 || volume > 100) return;
            if (channel == int.MinValue) return;

            Bass.BASS_ChannelSetAttribute(channel, BASSAttribute.BASS_ATTRIB_VOL, (float) (volume/100));
        }

        /// <summary>
        ///     Sets the volume of a channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="volume">The volume as a value between 0 and 100.</param>
        public static void SetVolume(int channel, double volume)
        {
            SetVolume(channel, (decimal) volume);
        }

        /// <summary>
        ///     Sets the volume of a channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="volume">The volume as a value between 0 and 100.</param>
        public static void SetVolume(int channel, int volume)
        {
            SetVolume(channel, (decimal) volume);
        }

        /// <summary>
        ///     Gets the wave out devices.
        /// </summary>
        /// <returns>A list of wave out devices</returns>
        public static List<string> GetWaveOutDevices()
        {
            var devices = new List<string>();
            var deviceCount = waveOutGetNumDevs();

            for (var i = -1; i < deviceCount; i++)
            {
                var waveOutCaps = new WaveOutCaps();
                waveOutGetDevCaps(i, ref waveOutCaps, Marshal.SizeOf(typeof (WaveOutCaps)));
                var deviceName = new string(waveOutCaps.szPname);

                if (deviceName.Contains('\0'))
                    deviceName = deviceName.Substring(0, deviceName.IndexOf('\0'));

                devices.Add(deviceName);
            }

            return devices;
        }

        /// <summary>
        ///     The waveOutGetNumDevs function retrieves the number of waveform-audio output devices present in the system.
        /// </summary>
        /// <returns>
        ///     Returns the number of devices. A return value of zero means that no devices are present or that an error occurred.
        /// </returns>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode,
            CallingConvention = CallingConvention.Winapi)]
        private static extern int waveOutGetNumDevs();

        /// <summary>
        ///     The waveOutGetDevCaps function retrieves the capabilities of a given waveform-audio output device.
        /// </summary>
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.Winapi)]
        private static extern int waveOutGetDevCaps(int uDeviceId, ref WaveOutCaps lpCaps, int uSize);

        /// <summary>
        ///     Adds a track to mixer
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="mixerChannel">The mixer channel.</param>
        public static void AddTrackToMixer(Track track, int mixerChannel)
        {
            if (track == null || !track.IsAudioLoaded()) throw new Exception("Track null or not audio not loaded");
            if (mixerChannel == int.MinValue) throw new Exception("Mixer channel not initialized");

            DebugHelper.WriteLine($"AddTrackToMixer {track.Description} {mixerChannel} {track.Channel}...");
            AddChannelToMixer(track.Channel, mixerChannel);
            DebugHelper.WriteLine("done");
        }

        /// <summary>
        ///     Adds a channel to a mixer channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="mixerChannel">The mixer channel.</param>
        public static void AddChannelToMixer(int channel, int mixerChannel)
        {
            if (channel == int.MinValue) throw new Exception("Channel not initialized");
            if (mixerChannel == int.MinValue) throw new Exception("Mixer channel not initialized");

            DebugHelper.WriteLine($"AddChannelToMixer {mixerChannel} {channel}");
            try
            {
                lock (Lock)
                {
                    BassMix.BASS_Mixer_StreamAddChannel(mixerChannel, channel,
                        BASSFlag.BASS_MIXER_PAUSE | BASSFlag.BASS_MIXER_DOWNMIX | BASSFlag.BASS_MIXER_NORAMPIN |
                        BASSFlag.BASS_MUSIC_AUTOFREE);
                }
            }
            catch (SEHException e)
            {
                DebugHelper.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var message = e.ExceptionObject.ToString();
            DebugHelper.WriteLine(message);
            throw new Exception(message);
        }

        public static void RemoveTrackFromMixer(Track track, int mixerChannel)
        {
            if (track == null || !track.IsAudioLoaded()) throw new Exception("Track null or not audio not loaded");
            if (mixerChannel == int.MinValue) throw new Exception("Mixer channel not initialized");

            lock (Lock)
            {
                DebugHelper.WriteLine($"RemoveTrackFromMixer {track.Description} {track.Channel}...");
                BassMix.BASS_Mixer_ChannelPause(track.Channel);
                Bass.BASS_ChannelLock(mixerChannel, true);

                foreach (var channel in track.Channels)
                {
                    BassMix.BASS_Mixer_ChannelRemove(channel);
                }

                Bass.BASS_ChannelLock(mixerChannel, false);
                DebugHelper.WriteLine("done");
            }
        }

        public static void UnloadTrackAudio(Track track)
        {
            if (track == null || !track.IsAudioLoaded()) throw new Exception("Track null or not audio not loaded");
            DebugHelper.WriteLine($"UnloadTrackAudio {track.Description}...");

            lock (Lock)
            {
                foreach (var channel in track.Channels)
                {
                    Bass.BASS_StreamFree(channel);
                }
                track.Channels.Clear();

                if (track.AudioData != null)
                {
                    track.AudioDataHandle.Free();
                    track.AudioData = null;
                }
            }

            DebugHelper.WriteLine("done");
        }

        public static void RemoveSampleFromMixer(Sample sample, int mixerChannel)
        {
            if (sample == null || sample.Channel == int.MinValue)
                throw new Exception("Sample null or not audio not loaded");
            if (mixerChannel == int.MinValue) throw new Exception("Mixer channel not initialized");

            lock (Lock)
            {
                DebugHelper.WriteLine($"RemoveSampleFromMixer {sample.Description} {sample.Channel}...");
                BassMix.BASS_Mixer_ChannelPause(sample.Channel);
                Bass.BASS_ChannelLock(mixerChannel, true);

                foreach (var channel in sample.Channels)
                {
                    BassMix.BASS_Mixer_ChannelRemove(channel);
                }

                Bass.BASS_ChannelLock(mixerChannel, false);
                DebugHelper.WriteLine("done");
            }
        }

        public static void UnloadSampleAudio(Sample sample)
        {
            if (sample == null || sample.Channel == int.MinValue)
                throw new Exception("Sample null or not audio not loaded");
            DebugHelper.WriteLine($"UnloadSampleAudio {sample.Description}...");

            lock (Lock)
            {
                foreach (var channel in sample.Channels)
                {
                    Bass.BASS_StreamFree(channel);
                }
                sample.Channels.Clear();
            }

            if (sample.AudioData != null)
            {
                sample.AudioDataHandle.Free();
                sample.AudioData = null;
            }

            DebugHelper.WriteLine("done");
        }

        public static void AddSampleToSampler(Sample sample, int mixerChannel)
        {
            if (sample == null || sample.Channel == int.MinValue)
                throw new Exception("Sample null or not audio not loaded");
            if (mixerChannel == int.MinValue) throw new Exception("Mixer channel not initialized");

            DebugHelper.WriteLine($"AddSampleToMixer {sample.Description} {mixerChannel} {sample.Channel}...");
            AddChannelToMixer(sample.Channel, mixerChannel);
            DebugHelper.WriteLine("done");

            SetSampleReplayGain(sample);
        }

        public static bool IsSameTrack(Track track1, Track track2)
        {
            if (track1 == null || track2 == null) return false;
            return (track1.Description == track2.Description);
        }

        /// <summary>
        ///     Track details
        /// </summary>
        public class TrackDetails
        {
            public TrackDetails()
            {
                Artist = "";
                Title = "";
                TrackNumber = "";
            }

            public string Title { get; set; }

            public string Artist { get; set; }

            public string Description { get; set; }

            public string AlbumArtist { get; set; }

            public string TrackNumber { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
        private struct WaveOutCaps
        {
            public readonly short wMid;
            public readonly short wPid;
            public readonly int vDriverVersion;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public readonly char[] szPname;

            public readonly uint dwFormats;
            public readonly short wChannels;
            public readonly short wReserved1;
            public readonly uint dwSupport;
        }
    }
}