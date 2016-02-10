namespace Halloumi.BassEngine.Players
{
    internal class AudioSync
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