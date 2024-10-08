﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.Channels;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;
using Un4seen.Bass.AddOn.Mix;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public static class AudioStreamHelper
    {
        private const int DefaultSampleRate = 44100;

        //private static readonly object Lock = new object();




        /// <summary>
        ///     Converts a decibel value to a percent value.
        /// </summary>
        /// <param name="decibel">The decibel.</param>
        /// <returns>The percent value</returns>
        private static float DecibelToPercent(float decibel)
        {
            return (float)(Math.Pow(10, 0.05 * decibel));
        }

        /// <summary>
        ///     Sets current volume for an audio stream as a percentage (0 - 1)
        /// </summary>
        /// <param name="audioStream"></param>
        /// <param name="volume">The volume as a percentage (0 - 1).</param>
        public static void SetVolume(AudioStream audioStream, float volume)
        {
            SetVolume(audioStream.ChannelId, volume);
        }

        /// <summary>
        ///     Sets current volume for an audio stream as a percentage (0 - 100)
        /// </summary>
        /// <param name="audioStream"></param>
        /// <param name="volume">The volume as a percentage (0 - 100).</param>
        public static void SetVolume(AudioStream audioStream, decimal volume)
        {
            if (volume > 100 || volume < 0) throw new Exception("Volume not it range");
            SetVolume(audioStream.ChannelId, (float)(volume / 100));
        }


        /// <summary>
        ///     Sets current volume for the channel as a percentage (0 - 1)
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="volume">The volume.</param>
        private static void SetVolume(int channel, float volume)
        {
            if (channel == int.MinValue) return;
            if (volume > 1 || volume < 0) throw new Exception("Volume not it range");

            // DebugHelper.WriteLine($"SetChannelVolume {channel} {volume}...");
            //lock (Lock)
            {
                Bass.BASS_ChannelSetAttribute(channel, BASSAttribute.BASS_ATTRIB_VOL, volume);
                Thread.Sleep(1);
            }

            // DebugHelper.WriteLine("done");
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

            //lock (Lock)
            {
                Bass.BASS_ChannelSetAttribute(channel, BASSAttribute.BASS_ATTRIB_VOL, (float)(volume / 100));
                Thread.Sleep(1);
            }
        }


        /// <summary>
        ///     Gets the audio stream volume.
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        /// <returns>The tracks volume</returns>
        public static decimal GetVolume(AudioStream audioStream)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded()) return 0;
            return GetVolume(audioStream.ChannelId);
        }

        /// <summary>
        ///     Gets the volume of a channel as a value between 0 and 100.
        /// </summary>
        /// <returns>A value between 0 and 100</returns>
        public static decimal GetVolume(int channel)
        {
            if (channel == int.MinValue) return 0;

            float volume = 0;
            // DebugHelper.WriteLine($"GetChannelVolume {channel}...");
            //lock (Lock)
            {
                Bass.BASS_ChannelGetAttribute(channel, BASSAttribute.BASS_ATTRIB_VOL, ref volume);
                Thread.Sleep(1);
            }
            // DebugHelper.WriteLine("done");
            return Convert.ToDecimal(volume * 100);
        }


        /// <summary>
        ///     Sets the duration and start/end volumes for an audio stream volume slide.
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        /// <param name="startVolume">The start volume as a percentage (0 - 1).</param>
        /// <param name="endVolume">The end volume as a percentage (0 - 1).</param>
        /// <param name="seconds">The seconds.</param>
        public static void SetVolumeSlide(AudioStream audioStream, float startVolume, float endVolume, double seconds)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded()) return;

            // set start volume
            SetVolume(audioStream, startVolume);

            var miliseconds = (int)(seconds * 1000);

            // set the volume slide
            //lock (Lock)
            {
                Bass.BASS_ChannelSlideAttribute(audioStream.ChannelId, BASSAttribute.BASS_ATTRIB_VOL, endVolume,
                    miliseconds);
                Thread.Sleep(1);
            }
        }

        /// <summary>
        ///     Sets the duration and start/end volumes for an audio stream volume slide.
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        /// <param name="startVolume">The start volume.</param>
        /// <param name="endVolume">The end volume.</param>
        /// <param name="sampleDuration">Sample length duration.</param>
        public static void SetVolumeSlide(AudioStream audioStream, float startVolume, float endVolume,
            long sampleDuration)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded()) return;

            var seconds = audioStream.SamplesToSeconds(sampleDuration);
            SetVolumeSlide(audioStream, startVolume, endVolume, seconds);
        }

        /// <summary>
        ///     Gets the audio stream sample rate.
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        /// <returns>The audio stream sample rate</returns>
        public static int GetSampleRate(AudioStream audioStream)
        {
            return GetSampleRate(audioStream.ChannelId);
        }

        /// <summary>
        ///     Gets the audio stream sample rate.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>
        ///     The audio stream sample rate
        /// </returns>
        public static int GetSampleRate(int channel)
        {
            if (channel == int.MinValue) return 0;

            float trackSampleRate = DefaultSampleRate;
            //lock (Lock)
            {
                Bass.BASS_ChannelGetAttribute(channel, BASSAttribute.BASS_ATTRIB_FREQ, ref trackSampleRate);
                Thread.Sleep(1);
            }
            return (int)trackSampleRate;
        }

        /// <summary>
        ///     Sets the audio stream pitch, based on a percent pitch value (0 - 200, 100 being 'normal' pitch)
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        /// <param name="pitch">The pitch.</param>
        public static void SetPitch(AudioStream audioStream, double pitch)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded()) return;

            // DebugHelper.WriteLine("SetPitch");

            float sampleRate = audioStream.DefaultSampleRate;
            //lock (Lock)
            {
                Bass.BASS_ChannelSetAttribute(audioStream.ChannelId, BASSAttribute.BASS_ATTRIB_TEMPO_FREQ, sampleRate);
                Thread.Sleep(1);
            }
        }


        /// <summary>
        ///     Sets the audio stream pitch to match another audioStream's BPM
        /// </summary>
        /// <param name="changeTrack">The audio stream to change the pitch of.</param>
        /// <param name="matchTrack">The audio stream to match the BPM of</param>
        public static void SetTrackPitchToMatchAnotherTrack(Track changeTrack, Track matchTrack)
        {
            if (changeTrack == null || matchTrack == null) return;
            if (!changeTrack.IsAudioLoaded()) return;
            if (!matchTrack.IsAudioLoaded()) return;

            var sampleRate = GetTrackTempoChangeAsSampleRate(changeTrack, matchTrack);
            //lock (Lock)
            {
                Bass.BASS_ChannelSetAttribute(changeTrack.ChannelId, BASSAttribute.BASS_ATTRIB_TEMPO_FREQ, sampleRate);
                Thread.Sleep(1);
            }
        }


        /// <summary>
        ///     Gets the audio stream tempo change as an audio stream rate
        /// </summary>
        /// <param name="track1">The audio stream being fading out</param>
        /// <param name="track2">The audio stream being faded into.</param>
        /// <returns>The audio stream rate the first audioStream needs to be changed to in order to match the second audioStream</returns>
        private static float GetTrackTempoChangeAsSampleRate(Track track1, Track track2)
        {
            if (track1 == null || track2 == null) return DefaultSampleRate;

            return track1.DefaultSampleRate * BpmHelper.GetTrackTempoChangeAsRatio(track1, track2);
        }

        /// <summary>
        ///     Sets the audio stream tempo to match another audio stream's tempo
        /// </summary>
        /// <param name="changeTrack">The audio stream to change the temp of.</param>
        /// <param name="matchTrack">The audio stream to match the BPM of</param>
        public static void SetTrackTempoToMatchAnotherTrack(Track changeTrack, Track matchTrack)
        {
            if (changeTrack == null || matchTrack == null) return;
            if (!changeTrack.IsAudioLoaded()) return;
            if (!matchTrack.IsAudioLoaded()) return;

            var percentChange = (float)(BpmHelper.GetAdjustedBpmPercentChange(changeTrack.EndBpm, matchTrack.StartBpm));

            //lock (Lock)
            {
                Bass.BASS_ChannelSetAttribute(changeTrack.ChannelId, BASSAttribute.BASS_ATTRIB_TEMPO, percentChange);
                Thread.Sleep(1);
            }
        }

        /// <summary>
        ///     Resets the audio stream tempo.
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        public static void ResetTempo(AudioStream audioStream)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded()) return;

            //lock (Lock)
            {
                Bass.BASS_ChannelSetAttribute(audioStream.ChannelId, BASSAttribute.BASS_ATTRIB_TEMPO, 0F);
                Thread.Sleep(1);
            }
        }

        /// <summary>
        ///     Sets the audio stream tempo to match another BPM
        /// </summary>
        /// <param name="audioStream">The audio stream to change the temp of.</param>
        /// <param name="matchBpm">The match BPM.</param>
        public static void SetTempoToMatchBpm(AudioStream audioStream, decimal matchBpm)
        {
            if (audioStream == null || audioStream.ChannelId == int.MinValue) return;
            SetTempoToMatchBpm(audioStream, audioStream.Bpm, matchBpm);
        }

        /// <summary>
        /// Sets the audio stream tempo to match another BPM
        /// </summary>
        /// <param name="audioStream">The audio stream to change the temp of.</param>
        /// <param name="streamBpm">The stream BPM.</param>
        /// <param name="matchBpm">The match BPM.</param>
        public static void SetTempoToMatchBpm(AudioStream audioStream, decimal streamBpm, decimal matchBpm)
        {
            if (audioStream == null || audioStream.ChannelId == int.MinValue) return;

            SetTempoToMatchBpm(audioStream.ChannelId, streamBpm, matchBpm);
        }

        internal static void SetTempoToMatchBpm(int channel, decimal streamBpm, decimal matchBpm)
        {
            if (channel == int.MinValue) return;
            var percentChange = (float)(BpmHelper.GetAdjustedBpmPercentChange(streamBpm, matchBpm));

            //lock (Lock)
            {
                Bass.BASS_ChannelSetAttribute(channel, BASSAttribute.BASS_ATTRIB_TEMPO, percentChange);
                Thread.Sleep(1);
                Bass.BASS_ChannelSetAttribute(channel, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_SEQUENCE_MS,
                    matchBpm > streamBpm ? 20 : 82);
                Thread.Sleep(1);
            }
        }

        /// <summary>
        ///     Sets the audio stream pitch to match another BPM
        /// </summary>
        /// <param name="audioStream">The audio stream to change the temp of.</param>
        /// <param name="matchBpm">The match BPM.</param>
        public static void SetPitchToMatchBpm(AudioStream audioStream, decimal matchBpm)
        {
            if (audioStream == null || audioStream.ChannelId == int.MinValue) return;
            SetPitchToMatchBpm(audioStream, audioStream.Bpm, matchBpm);
        }

        /// <summary>
        /// Sets the audio stream pitch to match another BPM
        /// </summary>
        /// <param name="audioStream">The audio stream to change the temp of.</param>
        /// <param name="streamBpm">The stream BPM.</param>
        /// <param name="matchBpm">The match BPM.</param>
        public static void SetPitchToMatchBpm(AudioStream audioStream, decimal streamBpm, decimal matchBpm)
        {
            if (audioStream == null || audioStream.ChannelId == int.MinValue) return;

            var percentChange = (float)(BpmHelper.GetAdjustedBpmPercentChange(streamBpm, matchBpm));
            //lock (Lock)
            {
                Bass.BASS_ChannelSetAttribute(audioStream.ChannelId, BASSAttribute.BASS_ATTRIB_TEMPO_PITCH, percentChange);
                Thread.Sleep(1);
            }
        }


        /// <summary>
        ///     Resets the audio stream pitch.
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        public static void ResetPitch(AudioStream audioStream)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded()) return;

            //lock (Lock)
            {
                Bass.BASS_ChannelSetAttribute(audioStream.ChannelId, BASSAttribute.BASS_ATTRIB_TEMPO_PITCH, 0F);
                Thread.Sleep(1);
            }
        }

        /// <summary>
        ///     Sets the audio stream position
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        /// <param name="samplePosition">The audio stream position.</param>
        public static void SetPosition(AudioStream audioStream, long samplePosition)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded()) return;
            if (samplePosition < 0 || samplePosition > audioStream.Length) return;

          //  var secondPosition = TimeFormatHelper.GetFormattedSeconds(audioStream.SamplesToSeconds(samplePosition));
            // DebugHelper.WriteLine($"SetPosition {audioStream.Description} {secondPosition} {samplePosition}");

            //lock (Lock)
            {
                Bass.BASS_ChannelSetPosition(audioStream.ChannelId, samplePosition);
                Thread.Sleep(1);
            }

            // DebugHelper.WriteLine($"SetPosition END {audioStream.Description} {secondPosition} {samplePosition}");
        }

        /// <summary>
        ///     Gets the current audioStream position.
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        /// <returns>The current audio stream position</returns>
        public static long GetPosition(AudioStream audioStream)
        {
            if (audioStream == null) return 0;
            if (!audioStream.IsAudioLoaded()) return 0;
            //lock (Lock)
            {
                var position = Bass.BASS_ChannelGetPosition(audioStream.ChannelId);
                Thread.Sleep(1);
                return position;
            }
        }

        /// <summary>
        ///     Pause an audio stream
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        public static void Pause(AudioStream audioStream)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded()) return;
            //lock (Lock)
            {
                // DebugHelper.WriteLine("Pause Audio Stream (" + audioStream.Description + ")");
                BassMix.BASS_Mixer_ChannelPause(audioStream.ChannelId);
                Thread.Sleep(10);
            }
        }

        /// <summary>
        ///     Pauses an audio stream smoothly
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        public static void SmoothPause(AudioStream audioStream)
        {
            var smoothPauseAction = new Action<AudioStream>(SmoothPauseAsync);
            smoothPauseAction.BeginInvoke(audioStream, null, null);
        }

        /// <summary>
        ///     Does an audio stream power down effect asynchronously
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        private static void SmoothPauseAsync(AudioStream audioStream)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded()) return;

            var volume = ((float)GetVolume(audioStream)) / 100F;
            SetVolumeSlide(audioStream, volume, 0F, 0.15D);


            Pause(audioStream);
            SetVolume(audioStream, volume);
        }

        /// <summary>
        ///     Plays an audio stream
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        public static void Play(AudioStream audioStream)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded()) return;

            //lock (Lock)
            {
                // DebugHelper.WriteLine("Play Audio Stream (" + audioStream.Description + ")");
                BassMix.BASS_Mixer_ChannelPlay(audioStream.ChannelId);
                Thread.Sleep(1);
            }
        }

        /// <summary>
        ///     Determines whether an audio stream is currently playing.
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        /// <returns>
        ///     True if an audio stream is playing; otherwise, false.
        /// </returns>
        public static bool IsPlaying(AudioStream audioStream)
        {
            //if (audioStream == null) return false;
            //var position1 = GetPosition(audioStream);
            //Thread.Sleep(50);
            //var position2 = GetPosition(audioStream);
            //return (position1 != position2);


            if (audioStream == null)
                return false;
            if (audioStream.ChannelId == int.MinValue)
                return false;

            var playing = (Bass.BASS_ChannelIsActive(audioStream.ChannelId) != BASSActive.BASS_ACTIVE_STOPPED);
            Thread.Sleep(1);

            return playing;
        }

        /// <summary>
        ///     Plays a 'power down' effect on an audio stream.
        /// </summary>
        /// <param name="audioStream">The audio file.</param>
        public static void PowerDown(AudioStream audioStream)
        {
            var powerDownAction = new Action<AudioStream>(PowerDownAsync);
            powerDownAction.BeginInvoke(audioStream, null, null);
        }

        /// <summary>
        ///     Does an audio stream power down effect asynchronously
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        private static void PowerDownAsync(AudioStream audioStream)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded()) return;

            var freq = audioStream.DefaultSampleRate;
            var interval = (int)(BpmHelper.GetDefaultLoopLength(audioStream.Bpm) * 1000) / 128;

            // set the volume slide

            //lock (Lock)
            {
                Bass.BASS_ChannelSlideAttribute(audioStream.ChannelId, BASSAttribute.BASS_ATTRIB_VOL, 0F, interval * 8);
                Thread.Sleep(1);
            }

            var percentValue = 0.70;
            while (freq > 100)
            {
                percentValue = percentValue / 1.2;
                interval = (int)(interval * 0.9D);
                freq = (int)(audioStream.DefaultSampleRate * percentValue);
                if (freq <= 100 || audioStream.ChannelId == int.MinValue) continue;
                //lock (Lock)
                {
                    Bass.BASS_ChannelSlideAttribute(audioStream.ChannelId, BASSAttribute.BASS_ATTRIB_FREQ, freq, interval);
                    Thread.Sleep(1);
                }
                Thread.Sleep(interval);
            }
            Pause(audioStream);
            if (!audioStream.IsAudioLoaded()) return;
            //lock (Lock)
            {
                Bass.BASS_ChannelSetAttribute(audioStream.ChannelId, BASSAttribute.BASS_ATTRIB_FREQ,
                    audioStream.DefaultSampleRate);
                Thread.Sleep(1);
            }
            SetVolume(audioStream, 100M);
        }

        /// <summary>
        ///     Gets the length of an audio stream.
        /// </summary>
        /// <param name="filename">The filename of an audio stream.</param>
        /// <returns>The length of an audio stream</returns>
        public static double GetLength(string filename)
        {
            var channel = Bass.BASS_StreamCreateFile(filename, 0L, 0L,
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN);
            if (channel == 0) throw new Exception("Cannot load audio file " + filename);
            var length = Bass.BASS_ChannelBytes2Seconds(channel, Bass.BASS_ChannelGetLength(channel));
            Bass.BASS_StreamFree(channel);
            return length;
        }

        /// <summary>
        /// Sets the replay gain for a channel.
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        private static void SetReplayGain(AudioStream audioStream)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded())
                throw new Exception("Audio file null or not audio not loaded");

            if (!audioStream.IsGainChannelInitialized() && audioStream.Gain == 0) return;

            var volume = DecibelToPercent(audioStream.Gain);
            // DebugHelper.WriteLine("SetReplayGain for " + audioStream + " to " + volume);

            if (!audioStream.IsGainChannelInitialized())
            {
                audioStream.GainChannel = Bass.BASS_ChannelSetFX(audioStream.ChannelId, BASSFXType.BASS_FX_BFX_VOLUME, int.MaxValue);
            }

            var volumeParameters = new BASS_BFX_VOLUME(volume, BASSFXChan.BASS_BFX_CHANALL);
            Bass.BASS_FXSetParameters(audioStream.GainChannel, volumeParameters);
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

            // DebugHelper.WriteLine("SetReplayGain " + gain);

            var fxChannel = Bass.BASS_ChannelSetFX(channel, BASSFXType.BASS_FX_BFX_VOLUME, int.MaxValue);
            var volume = DecibelToPercent(gain);
            var volumeParameters = new BASS_BFX_VOLUME(volume, BASSFXChan.BASS_BFX_CHANALL);
            Bass.BASS_FXSetParameters(fxChannel, volumeParameters);
        }


        /// <summary>
        ///     Sets the length of an audio stream.
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        public static void SetLength(AudioStream audioStream)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded())
                throw new Exception("Audio file null or not audio not loaded");

            audioStream.Length = Bass.BASS_ChannelGetLength(audioStream.ChannelId);
        }

        /// <summary>
        ///     Adds an audio stream to a mixer
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        /// <param name="mixerChannel">The mixer channel.</param>
        public static void AddToMixer(AudioStream audioStream, MixerChannel mixerChannel)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded())
                throw new Exception("Audio file null or not audio not loaded");

            if (mixerChannel.ChannelId == int.MinValue) throw new Exception("Mixer channel not initialized");

            // DebugHelper.WriteLine($"AddToMixer {audioStream.Description} {mixerChannel} {audioStream.Channel}...");

            BassMix.BASS_Mixer_StreamAddChannel(mixerChannel.ChannelId, audioStream.ChannelId,
                BASSFlag.BASS_MIXER_CHAN_PAUSE | BASSFlag.BASS_MIXER_CHAN_DOWNMIX | BASSFlag.BASS_MIXER_NORAMPIN |
                BASSFlag.BASS_MUSIC_AUTOFREE);
            Thread.Sleep(1);

            audioStream.MixerChannelId = mixerChannel.ChannelId;

            // DebugHelper.WriteLine("done");
        }

        /// <summary>
        ///     Removes and audio stream from a mixer.
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        /// <param name="mixerChannel">The mixer channel.</param>
        public static void RemoveFromMixer(AudioStream audioStream, MixerChannel mixerChannel)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded())
                return;

            //throw new Exception("Audio file null or not audio not loaded");

            if (mixerChannel.ChannelId == int.MinValue)
                throw new Exception("Mixer channel not initialized");

            //lock (Lock)
            {
                // DebugHelper.WriteLine($"RemoveFromMixer {audioStream.Description} {audioStream.Channel}...");
                BassMix.BASS_Mixer_ChannelPause(audioStream.ChannelId);
                Bass.BASS_ChannelLock(mixerChannel.ChannelId, true);

                foreach (var channel in audioStream.ChannelIds)
                {
                    BassMix.BASS_Mixer_ChannelRemove(channel);
                }

                Bass.BASS_ChannelLock(mixerChannel.ChannelId, false);
                // DebugHelper.WriteLine("done");


                if (audioStream.MixerChannelId == mixerChannel.ChannelId)
                    audioStream.MixerChannelId = int.MinValue;
            }
        }

        public static void LoadAudio(AudioStream audioStream)
        {
            // abort if audio data already loaded
            if (audioStream.IsAudioLoaded()) return;

            AudioDataHelper.LoadAudioData(audioStream);

            var channel = Bass.BASS_StreamCreateFile(audioStream.AudioData.DataPointer,
                0,
                audioStream.AudioData.Data.Length,
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN);

            audioStream.AddChannel(channel);

            if (audioStream.ChannelId == 0)
                throw new Exception("Cannot load " + audioStream.Filename + ". Error code: " + Bass.BASS_ErrorGetCode());

            // DebugHelper.WriteLine("Creating reverse FX stream " + audioStream.Description + "...");
            audioStream.AddChannel(BassFx.BASS_FX_ReverseCreate(audioStream.ChannelId, 1, BASSFlag.BASS_STREAM_DECODE));

            if (audioStream.ChannelId == 0)
                throw new Exception("Cannot load " + audioStream.Filename + ". Error code: " + Bass.BASS_ErrorGetCode());

            Bass.BASS_ChannelSetAttribute(audioStream.ChannelId, BASSAttribute.BASS_ATTRIB_REVERSE_DIR,
                (float)BASSFXReverse.BASS_FX_RVS_FORWARD);


            // DebugHelper.WriteLine("Creating tempo FX stream " + audioStream.Description + "...");

            audioStream.AddChannel(BassFx.BASS_FX_TempoCreate(audioStream.ChannelId,
                BASSFlag.BASS_FX_FREESOURCE | BASSFlag.BASS_STREAM_DECODE));

            if (audioStream.ChannelId == 0)
                throw new Exception("Cannot load " + audioStream.Filename + ". Error code: " + Bass.BASS_ErrorGetCode());

            // DebugHelper.WriteLine("Calculating track length " + audioStream.Description + "...");

            audioStream.Length = Bass.BASS_ChannelGetLength(audioStream.ChannelId);
            audioStream.DefaultSampleRate = GetSampleRate(audioStream.ChannelId);

            SetReplayGain(audioStream);
            SetPosition(audioStream, 0);


        }

        /// <summary>
        ///     Unloads the audio of an audio stream
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        public static void UnloadAudio(AudioStream audioStream)
        {
            if (audioStream == null || !audioStream.IsAudioLoaded())
                //throw new Exception("Audio file null or not audio not loaded");
                return;

            // DebugHelper.WriteLine($"UnloadAudio {audioStream.Description}...");

            foreach (var channel in audioStream.ChannelIds)
            {
                Bass.BASS_StreamFree(channel);
            }
            audioStream.ChannelIds.Clear();

            AudioDataHelper.UnloadAudioData(audioStream);

            // DebugHelper.WriteLine("done");
        }
    }
}