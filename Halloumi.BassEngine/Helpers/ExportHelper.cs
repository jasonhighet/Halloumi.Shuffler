using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using Halloumi.BassEngine.Models;
using Halloumi.Common.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Enc;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.Misc;

namespace Halloumi.BassEngine.Helpers
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public static class ExportHelper
    {
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
                BassHelper.SetReplayGain(channel, gain);

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
        /// <param name="gain">The gain.</param>
        public static void SaveAsMonoWave(string inFilename, string outFilename, float gain)
        {
            SaveAsMonoWave(inFilename, outFilename, 0, gain);
        }

        /// <summary>
        ///     Saves an audio file as a mono wave.
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
        ///     Saves audio data as a mono wave.
        /// </summary>
        /// <param name="audioData">The audio data.</param>
        /// <param name="outFilename">The output filename.</param>
        public static void SaveAsMonoWave(byte[] audioData, string outFilename)
        {
            SaveAsMonoWave(audioData, outFilename, 0, 0);
        }

        /// <summary>
        ///     Saves audio data as a mono wave.
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

            BassHelper.SetReplayGain(mixer, gain);

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
    }
}