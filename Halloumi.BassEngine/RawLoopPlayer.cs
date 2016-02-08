using System;
using System.IO;
using Halloumi.BassEngine.Channels;
using Halloumi.BassEngine.Helpers;
using Halloumi.BassEngine.Models;

namespace Halloumi.BassEngine
{
    public class RawLoopPlayer
    {
        private AudioStream _audioStream;
        private AudioSection _audioSection;

        public RawLoopPlayer()
        {
            Output = new MixerChannel();
        }

        public MixerChannel Output { get; }

        public void LoadAudio(string filename)
        {
            if (!File.Exists(filename)) throw new Exception("Cannot find file " + filename);

            if (_audioStream != null) UnloadAudio();

            _audioStream = new Sample
            {
                Filename = filename
            };

            AudioStreamHelper.LoadAudio(_audioStream);

            _audioStream.SyncProc = OnSync;

            AudioStreamHelper.AddToMixer(_audioStream, Output.InternalChannel);

            SetPositions(0, _audioStream.LengthSeconds);

        }

        public void UnloadAudio()
        {
            AudioStreamHelper.Pause(_audioStream);
            AudioStreamHelper.RemoveFromMixer(_audioStream, Output.InternalChannel);
            AudioStreamHelper.UnloadAudio(_audioStream);

            _audioStream.SyncProc = null;
            _audioStream = null;
        }

        public void SetPositions(double start, double length, double offset = 0)
        {
            if (_audioStream == null) return;

            _audioSection = new AudioSection(_audioStream, start, length, offset);

            AudioStreamHelper.SetPosition(_audioStream, _audioSection.OffsetSample);
        }

        public void Play()
        {
            AudioStreamHelper.Play(_audioStream);
        }

        public void Pause()
        {
            AudioStreamHelper.Pause(_audioStream);
        }

        private void OnSync(int handle, int channel, int data, IntPtr pointer)
        {
        }

        private class AudioSection
        {
            public AudioSection(AudioStream audioStream, double start, double length, double offset = 0)
            {
                AudioStream = audioStream;
                StartSample = AudioStream.SecondsToSamples(start);
                EndSample = AudioStream.SecondsToSamples(start + length);
                OffsetSample = AudioStream.SecondsToSamples(start + offset);

                var maxSample = AudioStream.Length - 500;

                if (EndSample >= maxSample) EndSample = maxSample;
                if (StartSample< 0 || StartSample> EndSample) StartSample = 0;
                if (EndSample <= 0 || EndSample <= start) EndSample = maxSample;

                if (OffsetSample<StartSample || OffsetSample> EndSample)
                    OffsetSample = StartSample;
            }

            private AudioStream AudioStream { get; set; }

            public long StartSample { get; set; }

            public long EndSample { get; set; }

            public long OffsetSample { get; set; }
        }
    }
}