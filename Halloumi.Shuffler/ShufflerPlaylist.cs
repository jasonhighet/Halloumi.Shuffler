using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Halloumi.Shuffler.AudioLibrary.Models;
using System.IO;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioLibrary.Helpers;

namespace Halloumi.Shuffler
{
    public class ShufflerPlaylist
    {
        private BassPlayer BassPlayer { get; }
        private Library Library { get; }
        private MixLibrary MixLibrary{ get; }
        public string CurrentPlaylistFile { get; internal set; }
        public List<Track> Tracks { get; internal set; }

        public ShufflerPlaylist(BassPlayer bassPlayer, Library library, MixLibrary mixLibrary)
        {
            BassPlayer = bassPlayer;
            Library = library;
            MixLibrary = mixLibrary;

            LoadWorkingPlaylist();
        }

        public void Open(string filename)
        {
            if (!File.Exists(filename))
                return;

            var tracks = CollectionHelper.GetTracksInPlaylistFile(filename);
            CurrentPlaylistFile = filename;

            Tracks = new List<Track>();
            QueueTracks(tracks);
        }

        public void Clear()
        {
            CurrentPlaylistFile = "";
            Tracks = new List<Track>();
            SetCurrentTrackIndex();
            RaisePlaylistChanged();
        }

        public void Add(List<Track> tracks)
        {
            QueueTracks(tracks);
        }

        public void Add(Track track)
        {
            QueueTracks(new List<Track> { track });
        }

        public void Save()
        {
            if (CurrentPlaylistFile == "")
                return;

            Save(CurrentPlaylistFile);
        }

        public void Save(string filename)
        {
            CollectionHelper.ExportPlaylist(filename, Tracks);
            CurrentPlaylistFile = filename;
        }

        public void RemoveTrack(int index)
        {
            RemoveTracks(index, 1);
        }

        public void RemoveTracks(int index, int count)
        {
            if (index < 0 || index >= Tracks.Count)
                return;

            Tracks.RemoveRange(index, count);
            SetCurrentTrackIndex();
            RaisePlaylistChanged();
        }

        private void RaisePlaylistChanged()
        { }

        private void SetCurrentTrackIndex()
        { }

        private string WorkingPlaylistFilename
        {
            get { return Path.Combine(ApplicationHelper.GetUserDataPath(), "Halloumi.Shuffler.WorkingPlaylist.xml"); }
        }

        private void LoadWorkingPlaylist()
        {
            if (!File.Exists(WorkingPlaylistFilename))
                return;

            Tracks = new List<Track>();
            CurrentPlaylistFile = "";

            var tracks = SerializationHelper<List<string>>
                .FromXmlFile(WorkingPlaylistFilename)
                .Select(x => Library.GetTrackByDescription(x))
                .Where(x => x != null)
                .ToList();
            
            QueueTracks(tracks);
        }

        private void SaveWorkingPlaylist()
        {
            var playlistFiles = Tracks.Select(x => x.Description).ToList();
            SerializationHelper<List<string>>.ToXmlFile(playlistFiles, WorkingPlaylistFilename);
        }

        private void QueueTracks(List<Track> tracks)
        {
            Tracks.AddRange(tracks);
            SetCurrentTrackIndex();
            RaisePlaylistChanged();
        }


        //Play()
        //Pause()
        //Next()
        //Previous()
        //ReplayMix()
        //SkipToEnd()
        //int CurrentTrackIndex()
        //Queue(int index)
        //Play(int index)
        //Event PlaylistChanged()
        //Event CurrentTrackChanged()

    }
}
