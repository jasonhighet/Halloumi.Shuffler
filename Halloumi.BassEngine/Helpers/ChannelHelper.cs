using System;
using System.Linq;
using System.Threading;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    public static class ChannelHelper
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
        public static void StopAudioEngine(IntPtr windowHandle)
        {
            if (_engineStarted) return;

            BassNet.Registration("jason.highet@gmail.com", "2X1931822152222");

            var devices = Bass.BASS_GetDeviceInfos().ToList();
            var defaultDevice = devices.First(x => x.IsDefault);
            var defaultDeviceId = devices.IndexOf(defaultDevice);


            if (!Bass.BASS_Init(defaultDeviceId, DefaultSampleRate, BASSInit.BASS_DEVICE_DEFAULT, windowHandle))
                throw new Exception("Cannot create Bass Engine.");

            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, 200);
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 20);

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
            var deviceInfo = Bass.BASS_GetDeviceInfos()[deviceId];
            if (deviceInfo.IsInitialized) return;

            if (!Bass.BASS_Init(deviceId, DefaultSampleRate, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                throw new Exception("Cannot initialize device.");
            }
        }

        /// <summary>
        ///     Initializes the mixer channel.
        /// </summary>
        /// <returns>The channel Id of the mixer channel</returns>
        public static int IntialiseOutputChannel()
        {
            var mixerChannel = BassMix.BASS_Mixer_StreamCreate(DefaultSampleRate, 2, BASSFlag.BASS_SAMPLE_FLOAT);
            if (mixerChannel == 0) throw new Exception("Cannot create Bass Mixer.");
            return mixerChannel;
        }

        /// <summary>
        ///     Initializes a decoder mixer channel.
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
        ///     Initializes a mono decoder mixer channel.
        /// </summary>
        /// <returns>The channel Id of the mixer channel</returns>
        public static int IntialiseMonoDecoderMixerChannel()
        {
            var mixerChannel = BassMix.BASS_Mixer_StreamCreate(DefaultSampleRate, 1,
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE);
            if (mixerChannel == 0) throw new Exception("Cannot create Bass Mixer.");
            return mixerChannel;
        }
    }
}