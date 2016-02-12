using Halloumi.Shuffler.AudioEngine.Helpers;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Channels
{
    public class MonitorOutputChannel : Channel
    {
        public MonitorOutputChannel()
            : base(null)
        {
            if (ChannelHelper.GetWaveOutDevices().Count >= 2)
            {
                const int monitorDeviceId = 2;
                ChannelHelper.InitialiseMonitorDevice(monitorDeviceId);

                // create monitor mixer channel
                InternalChannel = ChannelHelper.IntialiseOutputChannel();

                // set to use monitor sound card
                Bass.BASS_ChannelSetDevice(InternalChannel, monitorDeviceId);

                Bass.BASS_ChannelPlay(InternalChannel, false);
            }
            else
            {
                // create monitor channel on main speaker output
                InternalChannel = ChannelHelper.IntialiseOutputChannel();
                Bass.BASS_ChannelPlay(InternalChannel, false);
            }
        }
    }
}