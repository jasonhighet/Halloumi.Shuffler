using System;
using System.Threading;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    public static class ChannelHelper
    {
        private const int DefaultSampleRate = 44100;

        /// <summary>
        ///     Initializes the mixer channel.
        /// </summary>
        /// <returns>The channel Id of the mixer channel</returns>
        public static int InitializeOutputChannel(int deviceId)
        {
            var currentDeviceId = AudioEngineHelper.GetCurrentDeviceId();

            AudioEngineHelper.SetDevice(deviceId);

            var mixerChannel = BassMix.BASS_Mixer_StreamCreate(DefaultSampleRate, 2, BASSFlag.BASS_SAMPLE_FLOAT);
            if (mixerChannel == 0) throw new Exception("Cannot create Bass Mixer.");

            AssignChannelToDevice(mixerChannel, deviceId);
            AudioEngineHelper.SetDevice(currentDeviceId);

            return mixerChannel;
        }

        /// <summary>
        ///     Initializes a decoder mixer channel.
        /// </summary>
        /// <returns>The channel Id of the mixer channel</returns>
        public static int InitializeMixerChannel()
        {
            var mixerChannel = BassMix.BASS_Mixer_StreamCreate(DefaultSampleRate, 2,
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE);
            if (mixerChannel == 0) throw new Exception("Cannot create Bass Mixer.");
            return mixerChannel;
        }

        public static void AddChannelToMixer(int mixerChannel, int channel)
        {
            BassMix.BASS_Mixer_StreamAddChannel(mixerChannel, channel, BASSFlag.BASS_STREAM_DECODE);
        }

        public static int SplitDecoderMixer(int mixerChannel)
        {
            return BassMix.BASS_Split_StreamCreate(mixerChannel, BASSFlag.BASS_STREAM_DECODE, null);
        }


        public static void AssignChannelToDevice(int channelId, int deviceId)
        {
            if (!Bass.BASS_ChannelSetDevice(channelId, deviceId))
            {
                var error = Bass.BASS_ErrorGetCode();
                if (error != BASSError.BASS_ERROR_ALREADY)
                    throw new Exception("Can't set device " + error);
            }

            Bass.BASS_ChannelPlay(channelId, false);

        }
    }
}