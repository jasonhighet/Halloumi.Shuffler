using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler
{
    public class ShufflerPlaylist
    {
        public ShufflerPlaylist(BassPlayer bassPlayer, Library library)
        {
            BassPlayer = bassPlayer;
            Library = library;

            BassPlayer.OnTrackChange += BassPlayer_OnTrackChange;
            BassPlayer.OnSkipToEnd += BassPlayer_OnFadeEnded;
            BassPlayer.OnEndFadeIn += BassPlayer_OnFadeEnded;

            LoadWorkingPlaylist();
        }


        private BassPlayer BassPlayer { get; }
        private Library Library { get; }
        public string CurrentPlaylistFile { get; internal set; }
        public List<Track> Tracks { get; internal set; }
        public int CurrentTrackIndex { get; internal set; }

        private static string WorkingPlaylistFilename
        {
            get { return Path.Combine(ApplicationHelper.GetUserDataPath(), "Halloumi.Shuffler.WorkingPlaylist.xml"); }
        }


        public void Open(string filename)
        {
            if (!File.Exists(filename))
                return;

            var tracks = CollectionHelper.GetTracksInPlaylistFile(filename);
            CurrentPlaylistFile = filename;

            SetTracks(tracks, GetTrackIndexFromBassPlayer());
        }


        public void Clear()
        {
            CurrentPlaylistFile = "";
            SetTracks(new List<Track>(), -1);
        }

        public void Add(List<Track> tracksToAdd)
        {
            if (tracksToAdd == null || tracksToAdd.Count == 0)
                return;

            var tracks = new List<Track>();
            tracks.AddRange(Tracks);
            tracks.AddRange(tracksToAdd);

            var newIndex = GetNewCurrentIndex(tracksToAdd.Count);

            SetTracks(tracks, newIndex);
        }

        public void Add(Track track)
        {
            if (track == null)
                return;

            Add(new List<Track> {track});
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

            var tracks = new List<Track>();
            tracks.AddRange(Tracks);
            tracks.RemoveRange(index, count);

            var newIndex = GetNewCurrentIndex(removeFrom: index, removeCount: count);

            SetTracks(tracks, newIndex);
        }

        public void Play(int index)
        {
            var track = GetTrack(index);
            if (track == null) return;

            SetCurrentTrackIndex(index);
            BassPlayer.ForcePlay(track.Filename);
            SetNextBassPlayerTrack();
            RaiseCurrentTrackChanged();
        }

        public void PlayCurrent()
        {
            Play(CurrentTrackIndex);
        }

        public void PlayPrevious()
        {
            Play(CurrentTrackIndex - 1);
        }

        public void PlayNext()
        {
            Play(CurrentTrackIndex + 1);
        }

        public int GetNumberOfTracksRemaining()
        {
            return Tracks.Count - (CurrentTrackIndex + 1);
        }

        private int GetNewCurrentIndex(int addCount = 0, int removeFrom = 0, int removeCount = 0)
        {
            var isCurrent = new List<bool>();
            for (var i = 0; i < Tracks.Count; i++)
                isCurrent.Add(false);

            if (CurrentTrackIndex >= 0 && CurrentTrackIndex < isCurrent.Count)
                isCurrent[CurrentTrackIndex] = true;

            if (removeCount > 0)
                isCurrent.RemoveRange(removeFrom, removeCount);

            for (var i = 0; i < addCount; i++)
                isCurrent.Add(false);

            return isCurrent.FindIndex(x => x);
        }

        private void SetNextBassPlayerTrack()
        {
            var nextTrack = GetTrack(CurrentTrackIndex + 1);
            if (nextTrack == null)
                BassPlayer.ClearNextTrack();
            else if (BassPlayer.NextTrack == null)
                BassPlayer.QueueTrack(nextTrack.Filename);
            else if (BassPlayer.NextTrack.Description != nextTrack.Description)
                BassPlayer.QueueTrack(nextTrack.Filename);
        }

        private void SetTracks(List<Track> tracks, int newIndex)
        {
            Tracks = tracks;
            SaveWorkingPlaylist();
            SetCurrentTrackIndex(newIndex);
            SetNextBassPlayerTrack();
            PreloadTrack();
            RaisePlaylistChanged();
        }

        private int GetTrackIndexFromBassPlayer()
        {
            return BassPlayer.CurrentTrack == null
                ? -1
                : Tracks.FindIndex(t => t.Description == BassPlayer.CurrentTrack.Description);
        }


        private void SetCurrentTrackIndex(int index)
        {
            if (index < 0 || index >= Tracks.Count)
                return;

            CurrentTrackIndex = index;
        }

        private void LoadWorkingPlaylist()
        {
            if (!File.Exists(WorkingPlaylistFilename))
                return;

            CurrentPlaylistFile = "";
            var tracks = SerializationHelper<List<string>>
                .FromXmlFile(WorkingPlaylistFilename)
                .Select(x => Library.GetTrackByDescription(x))
                .Where(x => x != null)
                .ToList();

            SetTracks(tracks, GetTrackIndexFromBassPlayer());
        }

        private void SaveWorkingPlaylist()
        {
            var playlistFiles = Tracks.Select(x => x.Description).ToList();
            SerializationHelper<List<string>>.ToXmlFile(playlistFiles, WorkingPlaylistFilename);
        }

        public Track GetTrack(int index)
        {
            if (index < 0 || index >= Tracks.Count)
                return null;

            return Tracks[index];
        }

        private void BassPlayer_OnFadeEnded(object sender, EventArgs e)
        {
            PreloadTrack();
        }

        private void PreloadTrack()
        {
            var preloadTrack = GetTrack(CurrentTrackIndex + 2);
            if (preloadTrack == null)
                return;

            if (BassPlayer.PreloadedTrack == null)
                BassPlayer.PreloadTrack(preloadTrack.Filename);
            else if (BassPlayer.PreloadedTrack.Description != preloadTrack.Description)
                BassPlayer.PreloadTrack(preloadTrack.Filename);
        }

        /// <summary>
        ///     Gets the next track.
        /// </summary>
        /// <returns>The next track.</returns>
        public Track GetCurrentTrack()
        {
            return GetTrack(CurrentTrackIndex);
        }

        /// <summary>
        ///     Gets the next track.
        /// </summary>
        /// <returns>The next track.</returns>
        public Track GetNextTrack()
        {
            return GetTrack(CurrentTrackIndex + 1);
        }

        /// <summary>
        ///     Gets the next track.
        /// </summary>
        /// <returns>The next track.</returns>
        private Track GetTrackAfterNext()
        {
            return GetTrack(CurrentTrackIndex + 2);
        }

        /// <summary>
        ///     Gets the previous track.
        /// </summary>
        /// <returns>The previous track.</returns>
        public Track GetPreviousTrack()
        {
            return GetTrack(CurrentTrackIndex - 1);
        }

        private void RaiseCurrentTrackChanged()
        {
            CurrentTrackChanged?.Invoke(this, EventArgs.Empty);
        }

        private void RaisePlaylistChanged()
        {
            PlaylistChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BassPlayer_OnTrackChange(object sender, EventArgs e)
        {
            var currentTrack = GetTrack(CurrentTrackIndex)?.Description;
            var nextTrack = GetTrack(CurrentTrackIndex + 1)?.Description;
            var currentBassTrack = BassPlayer.CurrentTrack?.Description;

            if (currentBassTrack == currentTrack)
            {
                SetNextBassPlayerTrack();
            }
            else if (currentBassTrack == nextTrack)
            {
                SetCurrentTrackIndex(CurrentTrackIndex++);
                SetNextBassPlayerTrack();
                RaiseCurrentTrackChanged();
            }
            else
            {
                var newIndex = GetTrackIndexFromBassPlayer();
                if (newIndex == CurrentTrackIndex) return;

                SetCurrentTrackIndex(newIndex);
                SetNextBassPlayerTrack();
                RaiseCurrentTrackChanged();
            }
        }

        public event EventHandler PlaylistChanged;
        public event EventHandler CurrentTrackChanged;
    }
}