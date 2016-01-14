using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;

namespace Halloumi.BassEngine.Channels
{
    public class MonitorOutputChannel : Channel
    {
        private int _monitorDeviceId = int.MinValue;

        public MonitorOutputChannel()
            : base(null)
        {
            if (BassHelper.GetWaveOutDevices().Count >= 2)
            {
                _monitorDeviceId = 2;
                BassHelper.InitialiseMonitorDevice(_monitorDeviceId);

                // create monitor mixer channel
                this.InternalChannel = BassHelper.IntialiseOutputChannel();

                // set to use monitor sound card
                Bass.BASS_ChannelSetDevice(this.InternalChannel, _monitorDeviceId);

                Bass.BASS_ChannelPlay(this.InternalChannel, false);
            }
            else
            {
                // create monitor channel on main speaker output
                this.InternalChannel = BassHelper.IntialiseOutputChannel();
                Bass.BASS_ChannelPlay(this.InternalChannel, false);
            }
        }
    }
}