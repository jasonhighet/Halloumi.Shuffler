using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halloumi.BassEngine
{
    public class TrackHistory
    {
        private List<Track> _trackList = new List<Track>();

        public int MaximumSize
        {
            get; 
            set;
        }

        public TrackHistory()
        {
            this.MaximumSize = 255;
        }
    
        public void AddTrack(Track track)
        {
            _trackList.Insert(0, track);

            while (_trackList.Count >= this.MaximumSize)
            {
                _trackList.RemoveAt(this.MaximumSize - 1);
            }
        }

        public void Clear()
        {
            _trackList.Clear();
        }

        public void RemoveTrack(Track track)
        {
            _trackList.Remove(track);
        }
    }
}
