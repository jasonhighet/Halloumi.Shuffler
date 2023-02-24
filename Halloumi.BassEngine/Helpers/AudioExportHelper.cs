using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using Halloumi.Shuffler.AudioEngine.Models;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Enc;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.Misc;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public static class AudioExportHelper
    {
        /// <summary>
        ///     Saves the partial as wave.
        /// </summary>
        /// <param name="inFilename">The in filename.</param>
        /// <param name="outFilename">The out filename.</param>
        /// <param name="start">The start position in seconds.</param>
        /// <param name="length">The length in seconds.</param>
        /// <param name="offset">The offset position in seconds.</param>
        /// <param name="gain">The gain.</param>
        /// <param name="bpm">The BPM.</param>
        /// <param name="targetBpm">The target BPM.</param>
        public static void SavePartialAsWave(string inFilename,
            string outFilename,
            double start,
            double length,
            double offset = 0,
            float gain = 0,
            decimal bpm = 0,
            decimal targetBpm = 0)
        {
            // DebugHelper.WriteLine("Saving portion of track as wave with offset - " + inFilename);

            var audioStream = new Sample
            {
                Filename = inFilename,
                Description = inFilename,
                Gain = gain,
                Bpm = bpm
            };

            AudioStreamHelper.LoadAudio(audioStream);


            if (targetBpm != 0)
            {
                if (bpm == 0) bpm = BpmHelper.GetBpmFromLoopLength(length);
                var percentChange = BpmHelper.GetAdjustedBpmPercentChange(bpm, targetBpm) / 100;
                AudioStreamHelper.SetTempoToMatchBpm(audioStream.ChannelId, bpm, targetBpm);

                length = length * (double)(1 + percentChange);
            }

            const BASSEncode flags = BASSEncode.BASS_ENCODE_PCM;
            BassEnc.BASS_Encode_Start(audioStream.ChannelId, outFilename, flags, null, IntPtr.Zero);

            var startByte = Bass.BASS_ChannelSeconds2Bytes(audioStream.ChannelId, start);
            var endByte = Bass.BASS_ChannelSeconds2Bytes(audioStream.ChannelId, start + length);
            if (offset == 0 || offset == start)
            {
                TransferBytes(audioStream.ChannelId, startByte, endByte);
            }
            else
            {
                startByte = Bass.BASS_ChannelSeconds2Bytes(audioStream.ChannelId, offset);
                TransferBytes(audioStream.ChannelId, startByte, endByte);

                startByte = Bass.BASS_ChannelSeconds2Bytes(audioStream.ChannelId, start);
                endByte = Bass.BASS_ChannelSeconds2Bytes(audioStream.ChannelId, offset);
                TransferBytes(audioStream.ChannelId, startByte, endByte);
            }

            BassEnc.BASS_Encode_Stop(audioStream.ChannelId);

            Bass.BASS_StreamFree(audioStream.ChannelId);


            AudioStreamHelper.UnloadAudio(audioStream);
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

            Bass.BASS_ChannelSetPosition(channel, startByte, BASSMode.BASS_POS_BYTE);
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
            // DebugHelper.WriteLine("SaveAsMonoWave");

            var audioDataHandle = GCHandle.Alloc(audioData, GCHandleType.Pinned);
            var audioDataPointer = audioDataHandle.AddrOfPinnedObject();

            var channel = Bass.BASS_StreamCreateFile(audioDataPointer, 0, audioData.Length,
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN);
            if (channel == 0) throw new Exception("Cannot load audio data");

            // create a mono 44100Hz mixer
            var mixer = BassMix.BASS_Mixer_StreamCreate(44100, 1, BASSFlag.BASS_MIXER_END | BASSFlag.BASS_STREAM_DECODE);

            // plug in the source
            BassMix.BASS_Mixer_StreamAddChannel(mixer, channel,
                BASSFlag.BASS_MIXER_CHAN_DOWNMIX | BASSFlag.BASS_MIXER_NORAMPIN);

            AudioStreamHelper.SetReplayGain(mixer, gain);

            const BASSEncode flags = BASSEncode.BASS_ENCODE_PCM;
            BassEnc.BASS_Encode_Start(mixer, outFilename, flags, null, IntPtr.Zero);

            const int startByte = 0;

            if (length == 0) length = Bass.BASS_ChannelBytes2Seconds(channel, Bass.BASS_ChannelGetLength(channel));

            var totalTransferLength = Bass.BASS_ChannelSeconds2Bytes(mixer, length);

            Bass.BASS_ChannelSetPosition(channel, startByte, BASSMode.BASS_POS_BYTE);
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

            // DebugHelper.WriteLine("END SaveAsMonoWave");
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

        private static void TransferBytes(int channel, long startByte, long endByte)
        {
            var totalTransferLength = endByte - startByte;

            Bass.BASS_ChannelSetPosition(channel, startByte, BASSMode.BASS_POS_BYTE);
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
    }
}