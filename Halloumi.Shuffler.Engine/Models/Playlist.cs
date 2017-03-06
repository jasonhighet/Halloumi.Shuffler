using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Halloumi.Shuffler.AudioLibrary.Models
{
    public class Playlist
    {
        public string Name { get; set; }
        
        public List<PlaylistEntry> Entries { get; set; }

        public Playlist()
        {
            Entries = new List<PlaylistEntry>();
        }
    }

    public class PlaylistEntry
    {
        public string Artist { get; set; }

        public string Title { get; set; }
        
        public decimal Length { get; set; }
    }
}
