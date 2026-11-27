using System;
using System.Runtime.InteropServices;
using System.Threading;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Channels
{
    /// <summary>
    ///     Keeps a copy of the last moments of a stereo float output, for level meters and visuals, so they never
    ///     read the output channel itself. Reading an output with BASS_ChannelGetData or BASS_ChannelGetLevel only
    ///     works while BASS plays it to a device; once the output is a decode channel pulled by something else
    ///     (ASIO), those calls would take audio out of the mix.
    ///     <para>
    ///         A DSP at the end of the output's chain copies each block into a ring buffer. It takes no lock and
    ///         allocates nothing, because it runs on the mixer thread. Readers get the audio that is being heard
    ///         now: the DSP sees each block when it is mixed, which is <c>delayFrames()</c> ahead of the speakers.
    ///         A reader racing the DSP can see a block half overwritten, which is harmless for a meter.
    ///     </para>
    /// </summary>
    public class OutputTap
    {
        /// <summary>
        ///     The DSP priority: the lowest possible, so it runs after every plug-in and the mono fold-down.
        /// </summary>
        public const int Priority = int.MinValue;

        private const int Channels = 2;

        /// <summary>
        ///     About 0.75 s at 44.1 kHz: room for a 200 ms output buffer, a mixing block and an FFT window.
        /// </summary>
        private const int CapacityFrames = 32768;

        /// <summary>
        ///     BASS_ChannelGetLevel measures the last 20 ms.
        /// </summary>
        private const double LevelSeconds = 0.02;

        private readonly float[] _ring = new float[CapacityFrames * Channels];
        private readonly Func<int> _delayFrames;
        private readonly int _sampleRate;

        // held in fields so the garbage collector cannot free the delegates while BASS still calls them
        private readonly DSPPROC _dspProc;
        private readonly STREAMPROC _visualsProc;

        private long _framesWritten;
        private long _visualsCursor;
        private float[] _levelBuffer = new float[0];
        private float[] _visualsBuffer = new float[0];

        /// <param name="channelId">The stereo float output channel to copy.</param>
        /// <param name="sampleRate">The output's sample rate.</param>
        /// <param name="delayFrames">
        ///     How many frames the output has mixed but not yet played (its buffer); readers look this far back.
        /// </param>
        public OutputTap(int channelId, int sampleRate, Func<int> delayFrames)
        {
            _sampleRate = sampleRate;
            _delayFrames = delayFrames;

            _dspProc = Copy;
            Bass.BASS_ChannelSetDSP(channelId, _dspProc, IntPtr.Zero, Priority);

            _visualsProc = ReadForVisuals;
            VisualsChannelId = Bass.BASS_StreamCreate(sampleRate, Channels,
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE, _visualsProc, IntPtr.Zero);
        }

        /// <summary>
        ///     A decode channel that plays back what is being heard, for the Visuals spectrum drawing (which only
        ///     takes a channel). Each read of it is served from the tap; reading it takes nothing from the output.
        /// </summary>
        public int VisualsChannelId { get; }

        /// <summary>
        ///     The peak levels of the last 20 ms heard, 0 - 32768 like BASS_ChannelGetLevel.
        /// </summary>
        public VolumeLevels GetLevels()
        {
            var frames = (int)(_sampleRate * LevelSeconds);
            var buffer = Grow(ref _levelBuffer, frames);
            Read(buffer, frames, HeardFrame() - frames);

            float left = 0, right = 0;
            for (var i = 0; i + 1 < frames * Channels; i += Channels)
            {
                left = Math.Max(left, Math.Abs(buffer[i]));
                right = Math.Max(right, Math.Abs(buffer[i + 1]));
            }

            return new VolumeLevels
            {
                Left = ToLevel(left),
                Right = ToLevel(right)
            };
        }

        private static int ToLevel(float peak)
        {
            return (int)Math.Min(32768, peak * 32768);
        }

        /// <summary>
        ///     The DSP: copies the block into the ring buffer.
        /// </summary>
        private void Copy(int handle, int channel, IntPtr buffer, int length, IntPtr user)
        {
            if (length <= 0 || buffer == IntPtr.Zero) return;

            var frames = length / (sizeof(float) * Channels);
            if (frames > CapacityFrames)
            {
                // keep only the end of an oversized block
                buffer += (frames - CapacityFrames) * sizeof(float) * Channels;
                frames = CapacityFrames;
            }

            var written = Interlocked.Read(ref _framesWritten);
            var start = (int)(written % CapacityFrames);
            var firstFrames = Math.Min(frames, CapacityFrames - start);

            Marshal.Copy(buffer, _ring, start * Channels, firstFrames * Channels);
            if (firstFrames < frames)
                Marshal.Copy(buffer + firstFrames * sizeof(float) * Channels, _ring, 0,
                    (frames - firstFrames) * Channels);

            // published after the data, so a reader never looks past what has been copied
            Interlocked.Exchange(ref _framesWritten, written + frames);
        }

        /// <summary>
        ///     The visuals channel's STREAMPROC, called on the reader's thread from BASS_ChannelGetData. Serves the
        ///     audio being heard now; a request split over several calls carries on from where the last call ended.
        /// </summary>
        private int ReadForVisuals(int handle, IntPtr buffer, int length, IntPtr user)
        {
            var frames = length / (sizeof(float) * Channels);
            if (frames <= 0) return 0;

            var heard = HeardFrame();
            if (_visualsCursor < heard - frames || _visualsCursor > heard + frames)
                _visualsCursor = heard - frames;

            var data = Grow(ref _visualsBuffer, frames);
            Read(data, frames, _visualsCursor);
            _visualsCursor += frames;

            Marshal.Copy(data, 0, buffer, frames * Channels);
            return frames * sizeof(float) * Channels;
        }

        /// <summary>
        ///     The absolute frame being heard now (frames written minus what is still in the output's buffer).
        /// </summary>
        private long HeardFrame()
        {
            var delay = 0;
            try
            {
                delay = Math.Max(0, _delayFrames());
            }
            catch
            {
                // no delay information: show what was mixed last
            }

            return Interlocked.Read(ref _framesWritten) - delay;
        }

        /// <summary>
        ///     Copies frames starting at an absolute frame into a buffer. Frames that aren't in the ring (not yet
        ///     written, or already overwritten) come back as silence.
        /// </summary>
        private void Read(float[] destination, int frames, long fromFrame)
        {
            var written = Interlocked.Read(ref _framesWritten);
            var oldest = written - CapacityFrames;

            for (var i = 0; i < frames; i++)
            {
                var frame = fromFrame + i;
                var index = i * Channels;
                if (frame < 0 || frame < oldest || frame >= written)
                {
                    destination[index] = 0;
                    destination[index + 1] = 0;
                    continue;
                }

                var ringIndex = (int)(frame % CapacityFrames) * Channels;
                destination[index] = _ring[ringIndex];
                destination[index + 1] = _ring[ringIndex + 1];
            }
        }

        /// <summary>
        ///     A reader's scratch buffer, grown as needed. Each reader has its own; the DSP uses none.
        /// </summary>
        private static float[] Grow(ref float[] buffer, int frames)
        {
            if (buffer.Length < frames * Channels) buffer = new float[frames * Channels];
            return buffer;
        }
    }
}
