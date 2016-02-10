using Halloumi.BassEngine.Channels;

namespace Halloumi.BassEngine.Players
{
    public class RawLoopPlayer
    {
        private readonly AudioPlayer _audioPlayer;
        private AudioSection _audioSection;
        private const string StreamKey = "RawLoopTrack";

        public RawLoopPlayer()
        {
            _audioPlayer = new AudioPlayer();
        }

        public MixerChannel Output => _audioPlayer.Output;

        public void LoadAudio(string filename)
        {
            _audioPlayer.UnloadAll();
            _audioPlayer.Load(StreamKey, filename);
        }

        public void UnloadAudio()
        {
            _audioSection = null;
            _audioPlayer.UnloadAll();
        }

        public void SetPositions(double start, double length, double offset = double.MinValue)
        {
            _audioPlayer.Pause(StreamKey);
            if (_audioSection == null)
            {
                _audioSection = _audioPlayer.AddAudioSection(StreamKey, start, length, offset);
                _audioSection.LoopIndefinitely = true;
            }
            else
            {
                _audioPlayer.UpdateAudioSection(StreamKey, _audioSection , start, length, offset);
            }

            _audioPlayer.Queue(StreamKey, _audioSection);
            _audioPlayer.Play(StreamKey);
        }

        public void Play()
        {
            _audioPlayer.Play(StreamKey);
        }
        
        public void Pause()
        {
            _audioPlayer.Pause(StreamKey);
        }
    }
}