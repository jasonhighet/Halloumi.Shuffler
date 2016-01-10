using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;

namespace Halloumi.BassEngine.Channels
{
    public class ChannelEngine
    {
        public MonitorOutputChannel MonitorOutput { get; private set; }

        public SpeakerOutputChannel SpeakerOutput { get; private set; }

        public IntPtr WindowHandle { get; private set; }

        public string WAPluginsFolder { get; set; }

        public string VSTPluginsFolder { get; set; }

        // private bool _locked = false;

        private static bool _engineStarted = false;

        public ChannelEngine(IntPtr windowHandle)
        {
            WindowHandle = windowHandle;

            StartAudioEngine();

            this.SpeakerOutput = new SpeakerOutputChannel();
            this.MonitorOutput = new MonitorOutputChannel();
        }

        private void StartAudioEngine()
        {
            if (_engineStarted) return;

            BassHelper.InitialiseBassEngine(this.WindowHandle);

            _engineStarted = true;
        }

        public void StopAudioEngine()
        {
            Bass.BASS_Stop();
            Bass.BASS_Free();
        }

        /// <summary>
        /// Gets or sets the volume of the bass player as decimal 0 - 99.99.
        /// </summary>
        public decimal Volume
        {
            get
            {
                return (decimal)(Bass.BASS_GetVolume() * 100);
            }
            set
            {
                if (value >= 0 && value < 100)
                {
                    Bass.BASS_SetVolume((float)(value / 100));
                }
            }
        }
    }
}