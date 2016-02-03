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
            AudioStreamHelper.AddToMixer(_audioStream, Output.InternalChannel);
        }

        public void UnloadAudio()
        {
            AudioStreamHelper.Pause(_audioStream);
            AudioStreamHelper.RemoveFromMixer(_audioStream, Output.InternalChannel);
            AudioStreamHelper.UnloadAudio(_audioStream);
            _audioStream = null;
        }

        public void SetLoopPositions(decimal start, decimal length, decimal offset)
        {
        }

        public void Play()
        {
            AudioStreamHelper.Play(_audioStream);
        }

        public void Pause()
        {
            AudioStreamHelper.Pause(_audioStream);
        }
    }
}