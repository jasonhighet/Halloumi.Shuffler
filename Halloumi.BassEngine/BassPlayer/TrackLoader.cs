using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Mixing;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     Loads tracks (details, tags, saved attributes, audio) and unloads their audio.
    ///     A track's details stay cached for the life of the player; its audio is loaded when it is
    ///     needed and freed when it is no longer in a slot or among the last few loaded.
    /// </summary>
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    internal class TrackLoader
    {
        private readonly TrackCache _cache = new TrackCache();
        private readonly MixingDesk _desk;

        public TrackLoader(MixingDesk desk)
        {
            _desk = desk;
        }

        /// <summary>
        ///     Fade settings for tracks with no hand-set fade points.
        /// </summary>
        public FadeDefaults FadeDefaults { get; } = new FadeDefaults();

        /// <summary>
        ///     Loads the details and meta data about a track (not its audio). Returns the cached
        ///     track if it is already loaded.
        /// </summary>
        public Track LoadTrack(string filename)
        {
            var cachedTrack = _cache.Find(filename);
            if (cachedTrack != null) return cachedTrack;

            if (!File.Exists(filename)) throw new Exception("Cannot find file " + filename);

            var track = _cache.CreateTrack(filename);

            GuessArtistAndTitleFromFilename(track);

            ExtendedAttributesHelper.LoadExtendedAttributes(track);

            _cache.Add(track);

            LoadTagData(track);

            return track;
        }

        public Track LoadTrackAndAudio(string filename)
        {
            var track = LoadTrack(filename);

            if (track.IsAudioLoaded()) return track;

            LoadTrackAudioData(track);
            ExtendedAttributesHelper.LoadExtendedAttributes(track);

            return track;
        }

        /// <summary>
        ///     Makes a track that is not cached (the raw-loop track).
        /// </summary>
        public Track CreateUncachedTrack(string filename)
        {
            var track = _cache.CreateTrack(filename);
            GuessArtistAndTitleFromFilename(track);
            return track;
        }

        public Track Find(string filename)
        {
            return _cache.Find(filename);
        }

        public bool IsTrackLoaded(string filename)
        {
            return _cache.Contains(filename);
        }

        /// <summary>
        ///     Loads data for a track from its mp3 tags
        /// </summary>
        public void LoadTagData(Track track)
        {
            if (track == null) return;

            if ((Path.GetExtension(track.Filename) + "").ToLower() == ".wav") return;

            lock (track)
            {
                if (track.TagDataLoaded) return;

                var tags = TagHelper.LoadTags(track.Filename);

                track.Title = tags.Title;
                track.Artist = tags.Artist;

                if (track.Title == "" || track.Artist == "")
                    GuessArtistAndTitleFromFilename(track);

                if (tags.Gain.HasValue)
                    track.Gain = tags.Gain.Value;

                if (!string.IsNullOrEmpty(tags.Key))
                    track.Key = tags.Key;

                if (tags.Bpm.HasValue)
                    track.TagBpm = tags.Bpm.Value;

                if (tags.Length.HasValue)
                    track.Length = (long) tags.Length.Value * 1000;

                track.TagDataLoaded = true;
            }
        }

        /// <summary>
        ///     Loads the track audio data, then its saved attributes, then fills in any fade points
        ///     the attributes did not set.
        /// </summary>
        public Track LoadTrackAudioData(Track track)
        {
            if (track.IsAudioLoaded()) return track;

            if (!track.TagDataLoaded) LoadTagData(track);

            lock (track)
            {
                AudioStreamHelper.LoadAudio(track);

                // the fade points are set from scratch below, so any live move is gone
                track.ForgetLiveShifts();
                FadeDefaults.ResetFades(track);

                ExtendedAttributesHelper.LoadExtendedAttributes(track);

                FadeDefaults.FillMissingFadePoints(track);
            }

            _cache.MarkRecent(track);

            return track;
        }

        /// <summary>
        ///     Unloads the track's audio data (and its syncs).
        /// </summary>
        public void UnloadTrackAudioData(Track track)
        {
            if (!track.IsAudioLoaded()) return;

            // a fade moved live was for that play only
            track.RemoveLiveShifts();

            track.Syncs.Clear();

            AudioStreamHelper.RemoveFromMixer(track, _desk.TrackMixer);

            AudioStreamHelper.UnloadAudio(track);
            track.SyncProc = null;
        }

        /// <summary>
        ///     Unloads a track's audio and drops it from the cache.
        /// </summary>
        public void Unload(Track track)
        {
            AudioStreamHelper.Pause(track);
            UnloadTrackAudioData(track);
            _cache.Remove(track);
        }

        /// <summary>
        ///     Unloads the audio of tracks that are not in use and were not loaded recently.
        /// </summary>
        public void UnloadUnusedAudioData(Func<Track, bool> isInUse)
        {
            try
            {
                foreach (var track in _cache.GetUnloadable(isInUse))
                    UnloadTrackAudioData(track);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("UnloadUnusedAudioData failed: " + e.Message);
            }
        }

        /// <summary>
        ///     Gets every track whose audio is loaded, whether or not it is in a slot.
        /// </summary>
        public List<Track> GetLoadedTracks()
        {
            return _cache.GetAll().Where(t => t.IsAudioLoaded()).ToList();
        }

        /// <summary>
        ///     Unloads every track's audio and empties the cache.
        /// </summary>
        public void UnloadAll()
        {
            foreach (var track in _cache.RemoveAll())
                UnloadTrackAudioData(track);
        }

        private static void GuessArtistAndTitleFromFilename(Track track)
        {
            var trackDetails = TrackHelper.GuessTrackDetailsFromFilename(track.Filename);
            track.Title = trackDetails.Title;
            track.Artist = trackDetails.Artist;
        }
    }
}
