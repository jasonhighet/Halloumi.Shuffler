using System;
using System.Linq;
using System.Runtime.InteropServices;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Channels
{
    public class SpeakerOutputChannel : Channel
    {
        public SpeakerOutputChannel()
            : base(null)
        {
            var currentDeviceId = Bass.BASS_GetDevice();

            var devices = Bass.BASS_GetDeviceInfos().ToList();
            var speakerDevice = devices.First(x => x.IsDefault);
            var speakerDeviceId = devices.IndexOf(speakerDevice);

            ChannelHelper.InitialiseDevice(speakerDeviceId);

            if (!Bass.BASS_SetDevice(speakerDeviceId))
            {
                throw new Exception("Can't set device");
            }
            
            // create monitor mixer channel
            ChannelId = ChannelHelper.IntialiseOutputChannel();

            if (!Bass.BASS_ChannelSetDevice(ChannelId, speakerDeviceId))
            {
                var error = Bass.BASS_ErrorGetCode();
                if (error != BASSError.BASS_ERROR_ALREADY)
                    throw new Exception("Can't set device " + error);
            }


            Bass.BASS_ChannelPlay(ChannelId, false);

            Bass.BASS_SetDevice(currentDeviceId);

        }
    }
}