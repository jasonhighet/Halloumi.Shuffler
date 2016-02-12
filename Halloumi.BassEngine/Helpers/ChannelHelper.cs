using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace Halloumi.BassEngine.Helpers
{
    public static class ChannelHelper
    {
        private const int DefaultSampleRate = 44100;

        /// <summary>
        ///     Initialises the Bass audio engine.
        /// </summary>
        public static void InitialiseBassEngine(IntPtr windowHandle)
        {
            BassNet.Registration("jason.highet@gmail.com", "2X1931822152222");

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
            if (ChannelHelper.GetWaveOutDevices().Count < 3) return;

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