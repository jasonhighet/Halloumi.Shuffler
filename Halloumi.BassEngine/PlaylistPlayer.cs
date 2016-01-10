using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halloumi.BassEngine
{
    public class PlaylistPlayer
    {
        private List<string> PlaylistFiles { get; private set; }
        public BassPlayer BassPlayer { get; private set; }
        public Library Library { get; set; }
        public Track CurrentTrack { get; internal set; }

        public PlaylistPlayer()
        {
            this.BassPlayer = new BassPlayer();
            this.Library = new Library();
            this.BassPlayer.OnTrackChange += new EventHandler(BassPlayer_OnTrackChange);
        }

        public void LoadPlayList(string playlist)
        {
            this.PlaylistFiles = PlaylistHelper.GetFilesInPlaylist(playlist);

            this.Library.Clear();
            this.Library.LoadFromPlaylist(playlist);

            var existingFiles = this.Library.GetTracks().Select(t => t.Filename).ToList();
            var missingFiles = this.PlaylistFiles.Except(existingFiles).ToList();

            this.PlaylistFiles = this.PlaylistFiles.Except(missingFiles).ToList();

            this.QueueNextTrack();

        }


        private void BassPlayer_OnTrackChange(object sender, EventArgs e)
        {
            this.CurrentTrack = this.BassPlayer.CurrentTrack;
            this.QueueNextTrack();
        }

        private void QueueNextTrack()
        {
            throw new NotImplementedException();
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
        }

    }
}
