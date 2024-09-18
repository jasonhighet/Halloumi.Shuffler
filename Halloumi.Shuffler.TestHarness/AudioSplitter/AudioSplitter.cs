using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Enc;
using static Halloumi.Shuffler.TestHarness.AudioSplitter.CSVReader;

namespace Halloumi.Shuffler.TestHarness.AudioSplitter
{
    public static class AudioSplitter
    {
        private const float SilenceThreshold = 0.00000000001f;
        private const int BufferlengthInBytes = 512 * 4;
        private const int SampleRate = 44100;
        private const double EndScanRangeSeconds = 20.0;
        private const double PreEndBufferSeconds = 10.0;

        /// <summary>
        /// Splits a WAV file into multiple files based on information from a CSV file.
        /// </summary>
        /// <param name="wavFile">Path to the input WAV file.</param>
        /// <param name="csvFile">Path to the CSV file containing song information.</param>
        public static void SplitWavByCsv(string wavFile, string csvFile)
        {
            List<SongInfo> songs = CSVReader.ReadCSV(csvFile);

            if (!InitializeBass())
            {
                Console.WriteLine($"Failed to initialize BASS: {Bass.BASS_ErrorGetCode()}");
                return;
            }

            int stream = CreateStream(wavFile);
            if (stream == 0)
            {
                Console.WriteLine($"Error loading WAV file: {Bass.BASS_ErrorGetCode()}");
                Bass.BASS_Free();
                return;
            }

            try
            {
                ProcessSongs(stream, songs);
            }
            finally
            {
                Bass.BASS_StreamFree(stream);
                Bass.BASS_Free();
            }
        }

        private static bool InitializeBass()
        {
            return Bass.BASS_Init(-1, SampleRate, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
        }

        private static int CreateStream(string wavFile)
        {
            return Bass.BASS_StreamCreateFile(wavFile, 0, 0, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);
        }

        private static void ProcessSongs(int stream, List<SongInfo> songs)
        {
            long currentPosition = 0;

            foreach (var song in songs)
            {
                Console.WriteLine($"Processing: {song.Artist} - {song.Title}");

                Bass.BASS_ChannelSetPosition(stream, currentPosition);
                long start = FindSongStart(stream);
                if (start == currentPosition && currentPosition != 0)
                {
                    Console.WriteLine($"Warning: No sound detected at position {currentPosition}. Stopping processing.");
                    break;
                }

                long end = FindSongEndFromStart(stream, start, song.Length);
                Bass.BASS_ChannelSetPosition(stream, start);

                string outputFileName = $"{MakeValidFileName(song.Artist)} - {MakeValidFileName(song.Title)}.wav";

                if (EncodeSongToWav(stream, outputFileName, end))
                {
                    currentPosition = end;
                }
            }
        }

        private static bool EncodeSongToWav(int stream, string outputFileName, long end)
        {
            int encHandle = BassEnc.BASS_Encode_Start(stream, outputFileName, BASSEncode.BASS_ENCODE_PCM, null, IntPtr.Zero);
            if (encHandle == 0)
            {
                Console.WriteLine($"Error starting WAV encoding: {Bass.BASS_ErrorGetCode()}");
                return false;
            }

            try
            {
                byte[] buffer = new byte[1024];
                while (Bass.BASS_ChannelGetPosition(stream) < end)
                {
                    int bytesRead = Bass.BASS_ChannelGetData(stream, buffer, buffer.Length);
                    if (bytesRead <= 0) break;
                }
                return true;
            }
            finally
            {
                BassEnc.BASS_Encode_Stop(encHandle);
            }
        }

        private static string MakeValidFileName(string name)
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            return string.Join("_", name.Split(invalidChars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
        }

        private static long FindSongEndFromStart(int stream, long songStart, TimeSpan songLength)
        {
            Bass.BASS_ChannelSetPosition(stream, songStart);

            long approxEnd = songStart + Bass.BASS_ChannelSeconds2Bytes(stream, songLength.TotalSeconds - PreEndBufferSeconds);
            long endScanRange = Bass.BASS_ChannelSeconds2Bytes(stream, EndScanRangeSeconds);
            long actualEnd = songStart;

            float[] buffer = new float[BufferlengthInBytes / 4];

            while (Bass.BASS_ChannelGetPosition(stream) < approxEnd + endScanRange)
            {
                int bytesRead = Bass.BASS_ChannelGetData(stream, buffer, BufferlengthInBytes | (int)BASSData.BASS_DATA_FLOAT);
                if (bytesRead <= 0) break;

                if (!HasSound(buffer) && Bass.BASS_ChannelGetPosition(stream) > approxEnd)
                {
                    break;
                }

                if (HasSound(buffer))
                {
                    actualEnd = Bass.BASS_ChannelGetPosition(stream);
                }
            }

            return actualEnd;
        }

        private static long FindSongStart(int stream)
        {
            float[] buffer = new float[BufferlengthInBytes / 4];
            long position = Bass.BASS_ChannelGetPosition(stream);

            while (Bass.BASS_ChannelGetData(stream, buffer, BufferlengthInBytes | (int)BASSData.BASS_DATA_FLOAT) > 0)
            {
                if (HasSound(buffer))
                {
                    return position;
                }

                position = Bass.BASS_ChannelGetPosition(stream);
            }

            return position;
        }

        private static bool HasSound(float[] buffer) => buffer.Any(sample => Math.Abs(sample) > SilenceThreshold);
    }
}