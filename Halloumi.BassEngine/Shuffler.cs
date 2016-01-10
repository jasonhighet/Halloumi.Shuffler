using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Halloumi.BassEngine
{
    public class Shuffler
    {
        private List<Track> _history = new List<Track>();
        public BassPlayer BassPlayer { get; private set; }
        public Library Library { get; set; }
        public MixLibrary MixLibrary { get; set; }

        public int HistorySize { get; set; }

        public Track CurrentTrack { get; internal set; }
        public Track WorkingTrack { get; set; }

        private bool _trackChanged = false;

        public event EventHandler TrackChanged;

        public Track PreviousTrack 
        {
            get 
            {
                if (_history.Count == 0) return null;
                return _history[_history.Count - 1];
            }   
        }


        public Shuffler(BassPlayer bassPlayer, Library library, MixLibrary mixLibrary)
        {
            Timer timer = new Timer();
            timer.Interval = 50;
            timer.Tick += new EventHandler(timer_Tick);
            
            this.BassPlayer = bassPlayer;
            this.BassPlayer.OnTrackChange += new EventHandler(BassPlayer_OnTrackChange);
            this.Library = library;
            this.MixLibrary = mixLibrary;

            this.HistorySize = 255;

            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (_trackChanged) ChangeTrack();
        }

        private void BassPlayer_OnTrackChange(object sender, EventArgs e)
        {
            //var changeTrackAction = new Action(ChangeTrack);
            //changeTrackAction.BeginInvoke(null, null);
            _trackChanged = true;
        }

        private bool _trackChanging = false;
        private void ChangeTrack()
        {
            if (_trackChanging) return;
            _trackChanging = true;

            if (this.BassPlayer.CurrentTrack == null)
            {
                this.QueueRandom();
                this.CurrentTrack = this.BassPlayer.CurrentTrack;
            }
            else if (this.CurrentTrack != this.BassPlayer.CurrentTrack)
            {
                AddToHistory(this.CurrentTrack);
                this.CurrentTrack = this.BassPlayer.CurrentTrack;
            }
            
            if (this.BassPlayer.NextTrack == null)
            {
                this.QueueRandom();
            }

            _trackChanging = false;
            _trackChanged = false;

            if(this.TrackChanged != null) TrackChanged(this, EventArgs.Empty);
        }

        private void AddToHistory(Track track)
        {
            if (track == null) return;
            _history.Add(track);
            while (_history.Count >= this.HistorySize)
            {
                _history.RemoveAt(0);
            }
        }

        public void RemoveFromHistory(Track track)
        {
            if (track == null) return;
            _history.Remove(track);
        }

        public void Play()
        {
            if (this.CurrentTrack == null)
            {
                this.QueueRandom();
            }
            this.BassPlayer.Play();
        }

        public void Stop()
        {
            this.BassPlayer.Stop();
            _history.Clear();
            this.CurrentTrack = null;
        }

        public void Back()
        {
            if (_history.Count == 0) return;
            this.BassPlayer.Stop();
            this.CurrentTrack = _history[_history.Count - 1];
            _history.RemoveAt(_history.Count - 1);

            this.BassPlayer.QueueTrack(this.CurrentTrack);
            if (this.BassPlayer.NextTrack != null)
            {
                this.BassPlayer.ForcePlayNext();
            }
            else
            {
                this.BassPlayer.Play();
            }
            this.QueueRandom();
        }

        public void Queue(Track track)
        {
            if (this.CurrentTrack == null)
            {
                this.CurrentTrack = track;
            }
            this.BassPlayer.QueueTrack(track);
            if (this.BassPlayer.CurrentTrack == track)
            {
                this.CurrentTrack = track;
            }
            ChangeTrack();
        }

        public void QueueRandom()
        {
            this.Queue(this.GetRandomTrack());
        }

        public Track GetRandomTrack()
        {
            if (this.WorkingTrack != null)
            {
                return GetRandomWorkingTrack();
            }
            else
            {
                TrackSelector selector = new TrackSelector() { MixLibrary = this.MixLibrary, Library = this.Library };
                return selector.GetNextTrack(this.CurrentTrack, this.GetHistoryTracks());
            }
        }

        private Track GetRandomWorkingTrack()
        {
            var excludeTracks = new List<Track>();
            excludeTracks.Add(this.CurrentTrack);
            excludeTracks.AddRange(_history);

            var preferredTracks = this.MixLibrary.GetPreferredTracks(this.CurrentTrack);
            var forbiddenTracks = this.MixLibrary.GetForbiddenTracks(this.CurrentTrack);
            excludeTracks.AddRange(forbiddenTracks);
            excludeTracks.AddRange(preferredTracks);

            Track randomTrack = null;
            if (this.WorkingTrack != null && this.CurrentTrack != this.WorkingTrack && this.CurrentTrack != null)
            {
                randomTrack = this.WorkingTrack;
            }
            else if (this.WorkingTrack != null)
            {
                var currentTrack = this.CurrentTrack;
                if (currentTrack == null) currentTrack = this.WorkingTrack;

                randomTrack = BassHelper.GetTracksInBPMRange(currentTrack.EndBPM, 5M, this.Library.GetTracks())
                    .Union(BassHelper.GetTracksInEndBPMRange(currentTrack.StartBPM, 5M, this.Library.GetTracks()))
                    .Distinct()
                    .Except(excludeTracks)
                    .OrderBy(t => BassHelper.AbsoluteBPMPercentChange(currentTrack.EndBPM, t.StartBPM))
                    .FirstOrDefault();

            }
            if (randomTrack == null) randomTrack = this.WorkingTrack;

            return randomTrack;
        }

        public void Clear()
        {
            this.Stop();
        }

        public List<Track> GetHistoryTracks()
        {
            return _history;
        }
    }
}
