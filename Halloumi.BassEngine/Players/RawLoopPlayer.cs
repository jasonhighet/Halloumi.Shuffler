using Halloumi.Shuffler.AudioEngine.Channels;

namespace Halloumi.Shuffler.AudioEngine.Players
{
    public class RawLoopPlayer
    {
        private readonly AudioPlayer _audioPlayer;
        private const string StreamKey = "RawLoop";
        private const string SectionKey = "RawLoop";

        public RawLoopPlayer(IBmpProvider bpmProvider = null)
        {
            _audioPlayer = new AudioPlayer(bpmProvider);
        }

        public MixerChannel Output
        {
            get { return _audioPlayer.Output; }
        }

        public void LoadAudio(string filename)
        {
            _audioPlayer.UnloadAll();
            _audioPlayer.Load(StreamKey, filename);
            var section = _audioPlayer.AddSection(StreamKey, SectionKey);
            section.LoopIndefinitely = true;
        }

        public void UnloadAudio()
        {
            _audioPlayer.UnloadAll();
        }

        public void SetPositions(double start = 0, double length = 0, double offset = 0)
        {
            _audioPlayer.Pause(StreamKey);
            _audioPlayer.SetSectionPositions(StreamKey, SectionKey, start, length, offset);
            _audioPlayer.QueueSection(StreamKey, SectionKey);
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