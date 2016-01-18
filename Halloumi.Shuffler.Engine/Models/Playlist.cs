using System.Collections.Generic;

namespace Halloumi.Shuffler.Engine.Models
{
    public class Playlist
    {
        public string Name { get; set; }
        public string Filename { get; set; }
        public List<Track> Tracks { get; set; }

        public Playlist()
        {
            Tracks = new List<Track>();
        }
    }
}
