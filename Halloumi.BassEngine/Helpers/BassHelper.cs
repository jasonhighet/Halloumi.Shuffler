using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using Halloumi.BassEngine.Models;
using Halloumi.Common.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;
using Un4seen.Bass.AddOn.Mix;

namespace Halloumi.BassEngine.Helpers
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public static class BassHelper
    {
        private const int DefaultSampleRate = 44100;


        private static readonly object Lock = new object();

        static BassHelper()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        ///     Converts a decibel value to a percent value.
        /// </summary>
        /// <param name="decibel">The decibel.</param>
        /// <returns>The percent value</returns>
        private static float DecibelToPercent(float decibel)
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
        ///     Gets the track tempo change as a sample rate
        /// </summary>
        /// <param name="track1">The track being fading out</param>
        /// <param name="track2">The track being faded into.</param>
        /// <returns>The sample rate the first track needs to be changed to in order to match the second track</returns>
        private static float GetTrackTempoChangeAsSampleRate(Track track1, Track track2)
        {
            if (track1 == null || track2 == null) return DefaultSampleRate;

            return track1.DefaultSampleRate*BpmHelper.GetTrackTempoChangeAsRatio(track1, track2);
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

            var percentChange = (float) (BpmHelper.GetAdjustedBpmPercentChange(changeTrack.EndBpm, matchTrack.StartBpm));
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
        ///     Sets a sample tempo to match another BPM
        /// </summary>
        /// <param name="sample">The sample to change the temp of.</param>
        /// <param name="matchBpm">The match BPM.</param>
        public static void SetSampleTempoToMatchBpm(Sample sample, decimal matchBpm)
        {
            if (sample == null || sample.Channel == int.MinValue) return;

            var percentChange = (float) (BpmHelper.GetAdjustedBpmPercentChange(sample.Bpm, matchBpm));
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
            if (WaveOutHelper.GetWaveOutDevices().Count < 3) return;

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
            var interval = (int) (BpmHelper.GetDefaultLoopLength(track.EndBpm)*1000)/128;

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
        ///     Sets the replay gain for a channel.
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
        ///     Sets the length of the track.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void SetTrackLength(Track track)
        {
            if (track == null || !track.IsAudioLoaded()) return;
            track.Length = Bass.BASS_ChannelGetLength(track.Channel);
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
    }
}