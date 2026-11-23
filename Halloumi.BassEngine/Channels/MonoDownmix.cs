using System;
using System.Runtime.InteropServices;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Channels
{
    /// <summary>
    ///     A DSP that folds a stereo float channel down to mono by averaging left and right into both channels.
    /// </summary>
    internal class MonoDownmix
    {
        private readonly int _channelId;
        private readonly int _priority;

        // Held in a field so the garbage collector cannot free the delegate while BASS still calls it.
        private readonly DSPPROC _dspProc;
        private int _dspHandle;
        private float[] _buffer = new float[0];

        /// <param name="channelId">The stereo float channel to fold down.</param>
        /// <param name="priority">The DSP priority. Higher priorities run first.</param>
        public MonoDownmix(int channelId, int priority)
        {
            _channelId = channelId;
            _priority = priority;
            _dspProc = Process;
        }

        public bool Enabled
        {
            get => _dspHandle != 0;
            set
            {
                if (value == Enabled) return;

                if (value)
                {
                    _dspHandle = Bass.BASS_ChannelSetDSP(_channelId, _dspProc, IntPtr.Zero, _priority);
                }
                else
                {
                    Bass.BASS_ChannelRemoveDSP(_channelId, _dspHandle);
                    _dspHandle = 0;
                }
            }
        }

        private void Process(int handle, int channel, IntPtr buffer, int length, IntPtr user)
        {
            if (length == 0 || buffer == IntPtr.Zero) return;

            var count = length / sizeof(float);
            if (_buffer.Length < count) _buffer = new float[count];

            Marshal.Copy(buffer, _buffer, 0, count);

            for (var i = 0; i + 1 < count; i += 2)
            {
                var mono = (_buffer[i] + _buffer[i + 1]) * 0.5f;
                _buffer[i] = mono;
                _buffer[i + 1] = mono;
            }

            Marshal.Copy(_buffer, 0, buffer, count);
        }
    }
}
