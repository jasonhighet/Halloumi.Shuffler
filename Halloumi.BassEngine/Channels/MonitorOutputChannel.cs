using System;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Vst;

namespace Halloumi.Shuffler.AudioEngine.Channels
{
    public class MonitorOutputChannel : Channel
    {
        public MonitorOutputChannel()
            : base(null)
        {
            var currentDeviceId = Bass.BASS_GetDevice();

            var devices = Bass.BASS_GetDeviceInfos().ToList();
            var monitorDevice = devices.FirstOrDefault(x => !x.IsDefault && x.name != "No sound")
                                ?? devices.FirstOrDefault(x => x.IsDefault && x.name != "No sound");

            var monitorDeviceId = devices.IndexOf(monitorDevice);
            
            ChannelHelper.InitialiseDevice(monitorDeviceId);

            if (!Bass.BASS_SetDevice(monitorDeviceId))
            {
                throw new Exception("Can't set device");
            }
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, 200);
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 20);


            // create monitor mixer channel
            ChannelId = ChannelHelper.IntialiseOutputChannel();

            var device = Bass.BASS_ChannelGetDevice(ChannelId);


            if (!Bass.BASS_ChannelSetDevice(ChannelId, monitorDeviceId))
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