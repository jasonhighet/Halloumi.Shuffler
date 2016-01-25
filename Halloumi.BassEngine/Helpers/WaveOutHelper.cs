using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;

namespace Halloumi.BassEngine.Helpers
{
    public static class WaveOutHelper
    {
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