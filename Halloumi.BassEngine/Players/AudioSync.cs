namespace Halloumi.Shuffler.AudioEngine.Players
{
    public class AudioSync
    {
        public AudioSync()
        {
            Id = int.MinValue;
        }

        public long Position { get; set; }

        public int Id { get; set; }

        public SyncType SyncType { get; set; }
    }
}