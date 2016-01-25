using Halloumi.BassEngine.Helpers;
using Un4seen.Bass;

namespace Halloumi.BassEngine.Channels
{
    public class MonitorOutputChannel : Channel
    {
        private int _monitorDeviceId = int.MinValue;

        public MonitorOutputChannel()
            : base(null)
        {
            if (WaveOutHelper.GetWaveOutDevices().Count >= 2)
            {
                _monitorDeviceId = 2;
                BassHelper.InitialiseMonitorDevice(_monitorDeviceId);

                // create monitor mixer channel
                InternalChannel = BassHelper.IntialiseOutputChannel();

                // set to use monitor sound card
                Bass.BASS_ChannelSetDevice(InternalChannel, _monitorDeviceId);

                Bass.BASS_ChannelPlay(InternalChannel, false);
            }
            else
            {
                // create monitor channel on main speaker output
                InternalChannel = BassHelper.IntialiseOutputChannel();
                Bass.BASS_ChannelPlay(InternalChannel, false);
            }
        }
    }
}