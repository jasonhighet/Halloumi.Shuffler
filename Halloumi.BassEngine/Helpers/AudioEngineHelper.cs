using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    public static class AudioEngineHelper
    {
        private const int DefaultSampleRate = 44100;
        private static bool _engineStarted;

        /// <summary>
        ///     Gets or sets the volume of the bass player as decimal 0 - 100.
        /// </summary>
        public static decimal Volume
        {
            get
            {
                var value = (decimal)(Bass.BASS_GetVolume() * 100);
                Thread.Sleep(1);
                return value;
            }
            set
            {
                if (value < 0 || value > 100) return;
                Bass.BASS_SetVolume((float)(value / 100));
                Thread.Sleep(1);
            }
        }

        /// <summary>
        ///     Initializes the Bass audio engine.
        /// </summary>
        public static void StartAudioEngine(IntPtr windowHandle)
        {
            if (_engineStarted) return;

            BassNet.Registration("jason.highet@gmail.com", "2X1931822152222");

            var mainDevice = GetMainDevice();
            if (mainDevice == null)
                throw new Exception("No sound card");

            InitialiseDevice(mainDevice.Id, windowHandle);

            var monitorDevice = GetMonitorDevice();
            if (monitorDevice != null)
                InitialiseDevice(monitorDevice.Id, windowHandle);

            SetDevice(mainDevice);

            _engineStarted = true;
        }

        /// <summary>
        ///     Stops the Bass audio engine.
        /// </summary>
        public static void StopAudioEngine()
        {
            if (!_engineStarted) return;

            // DebugHelper.WriteLine("Stop Engine");
            Bass.BASS_Stop();
            Bass.BASS_Free();

            _engineStarted = false;
        }

        /// <summary>
        ///     Initializes the monitor device.
        /// </summary>
        /// <param name="deviceId">The monitor device Id.</param>
        public static void InitialiseDevice(int deviceId)
        {
            InitialiseDevice(deviceId, IntPtr.Zero);
        }

        /// <summary>
        ///     Initializes the monitor device.
        /// </summary>
        /// <param name="deviceId">The monitor device Id.</param>
        /// <param name="windowHandle"></param>
        public static void InitialiseDevice(int deviceId, IntPtr windowHandle)
        {
            var deviceInfo = Bass.BASS_GetDeviceInfos()[deviceId];
            if (deviceInfo.IsInitialized) return;

            if (!Bass.BASS_Init(deviceId, DefaultSampleRate, BASSInit.BASS_DEVICE_DEFAULT, windowHandle))
                throw new Exception("Cannot initialize device.");

            Bass.BASS_SetDevice(deviceId);

            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, 200);
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 20);
        }

        public static List<Device> GetDevices()
        {
            var bassDevices = Bass.BASS_GetDeviceInfos().ToList();
            return bassDevices.Select(x => new Device
                {
                    Name = x.name,
                    Id = bassDevices.IndexOf(x),
                    IsDefault = x.IsDefault,
                    IsInitialized = x.IsInitialized
                })
                .Where(x => x.Name != "No sound")
                .ToList();
        }

        public static Device GetMonitorDevice()
        {
            return GetDevices().FirstOrDefault(x => !x.IsDefault);
        }

        public static Device GetMainDevice()
        {
            return GetDevices().FirstOrDefault(x => x.IsDefault);
        }

        public static void SetDevice(Device device)
        {
            if(device == null) return;
            SetDevice(device.Id);
        }

        public static void SetDevice(int deviceId)
        {
            if (!Bass.BASS_SetDevice(deviceId))
            {
                throw new Exception("Can't set device");
            }
        }

        public static int GetCurrentDeviceId()
        {
            return Bass.BASS_GetDevice();
        }

        public class Device
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public bool IsDefault { get; set; }
            public bool IsInitialized { get; set; }
        }
    }
}