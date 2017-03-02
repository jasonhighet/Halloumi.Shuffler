using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;
using IdSharp.Tagging.ID3v2;
using System.Threading.Tasks;

namespace Halloumi.Shuffler.AudioLibrary.Helpers
{
    public static class TrackHelper
    {
        private const string NoValue = "(None)";

        /// <summary>
        ///     Gets or sets the bass player.
        /// </summary>
        public static BassPlayer BassPlayer { get; set; }

        public static bool SaveTrack(Track track)
        {
            var tags = new ID3v2Tag(track.Filename)
            {
                Genre = track.Genre.Replace(NoValue, ""),
                Album = track.Album.Replace(NoValue, ""),
                TrackNumber = track.TrackNumber.ToString(),
                LengthMilliseconds = Convert.ToInt32(track.FullLength*1000M),
                BPM = track.Bpm.ToString("0.00"),
                InitialKey = track.Key
            };

            if (track.Artist == track.AlbumArtist)
            {
                tags.Artist = track.Artist.Replace(NoValue, "");
                tags.Title = track.Title.Replace(NoValue, "");
            }
            else
            {
                tags.Artist = track.AlbumArtist.Replace(NoValue, "");
                tags.Title = track.Artist.Replace(NoValue, "") + " / " + track.Title.Replace(NoValue, "");
            }
            try
            {
                tags.Save(track.Filename);
            }
            catch
            {
                return false;
            }

            track.OriginalDescription = track.Description;

            return true;
        }

        /// <summary>
        ///     Gets the file name from track details.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>The generated filename</returns>
        private static string GetFileNameFromTrackDetails(Track track)
        {
            var filename = "";
            if (track.TrackNumber > 0) filename += track.TrackNumber.ToString("D2") + " - ";
            filename += track.AlbumArtist + " - ";
            if (track.Artist == track.AlbumArtist) filename += track.Title;
            else filename += track.Artist + " / " + track.Title;
            filename += ".mp3";
            filename = FileSystemHelper.StripInvalidFileNameChars(filename);
            return filename;
        }

        public static bool RenameTrack(Track track)
        {
            var filename = GetFileNameFromTrackDetails(track);
            if (filename == Path.GetFileName(track.Filename)) return false;

            try
            {
                var directoryName = Path.GetDirectoryName(track.Filename);
                if (directoryName != null)
                {
                    filename = Path.Combine(directoryName, filename);
                    File.Move(track.Filename, filename);
                    track.Filename = filename;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static DateTime GetTrackLastModified(string filename)
        {
            return File.GetLastWriteTime(filename);
        }


        /// <summary>
        ///     Calculates the length track and saves it in the tag data
        /// </summary>
        /// <param name="track">The track.</param>
        public static void UpdateLength(Track track)
        {
            var length = decimal.Round(Convert.ToDecimal(AudioStreamHelper.GetLength(track.Filename)), 3);
            if (length == decimal.Round(track.FullLength, 3)) return;

            track.Length = length;
            track.FullLength = length;
            SaveTrack(track);
        }

        /// <summary>
        ///     Guesses the artist and title of a track from its filename.
        /// </summary>
        /// <param name="track">The track.</param>
        private static void GuessTrackDetailsFromFileName(Track track)
        {
            var trackDetails = AudioEngine.Helpers.TrackHelper.GuessTrackDetailsFromFilename(track.Filename);
            track.Title = trackDetails.Title;
            track.Artist = trackDetails.Artist;
            track.AlbumArtist = trackDetails.AlbumArtist;

            track.TrackNumber = trackDetails.TrackNumber != "" ? track.TrackNumber : 0;
        }

        public static Track LoadTrack(string filename, bool updateLength = true)
        {
            if (!File.Exists(filename)) return null;
            if (!filename.ToLower().EndsWith(".mp3")) return null;

            var track = new Track {Filename = filename};
            LoadTrack(track, updateLength);
            return track;
        }

        /// <summary>
        /// Loads the track details from the tags in the associate mp3
        /// </summary>
        /// <param name="track">The track to load the details of.</param>
        /// <param name="updateLength">if set to <c>true</c> [update length].</param>
        public static void LoadTrack(Track track, bool updateLength = true)
        {
            if (!File.Exists(track.Filename)) return;
            if (!track.Filename.ToLower().EndsWith(".mp3")) return;

            DebugHelper.WriteLine("Library - LoadTrack - " + track.Description);

            GuessTrackDetailsFromFileName(track);

            var dateLastModified = GetTrackLastModified(track.Filename);
            track.LastModified = dateLastModified;

            if (ID3v2Tag.DoesTagExist(track.Filename))
            {
                var tags = new ID3v2Tag(track.Filename);

                if (!string.IsNullOrEmpty(tags.Artist)) track.Artist = tags.Artist.Trim();
                if (!string.IsNullOrEmpty(tags.Artist)) track.AlbumArtist = tags.Artist.Trim();
                if (!string.IsNullOrEmpty(tags.Title)) track.Title = tags.Title.Trim();
                if (!string.IsNullOrEmpty(tags.Album)) track.Album = tags.Album.Trim();
                if (!string.IsNullOrEmpty(tags.Genre)) track.Genre = tags.Genre.Trim();
                if (!string.IsNullOrEmpty(tags.InitialKey))
                {
                    var tagKey = tags.InitialKey.Trim();
                    track.Key = tagKey;
                }

                LoadArtistAndAlbumArtist(track);

                if (tags.LengthMilliseconds.HasValue) track.Length = (decimal) tags.LengthMilliseconds/1000M;

                decimal bpm;
                if (decimal.TryParse(tags.BPM, out bpm)) track.Bpm = bpm;

                track.Bpm = BpmHelper.NormaliseBpm(track.Bpm);
                track.EndBpm = track.Bpm;
                track.StartBpm = track.Bpm;
                track.Bpm = BpmHelper.GetAdjustedBpmAverage(track.StartBpm, track.EndBpm);

                int trackNumber;
                var trackNumberTag = (tags.TrackNumber + "/").Split('/')[0].Trim();
                if (int.TryParse(trackNumberTag, out trackNumber)) track.TrackNumber = trackNumber;

                if (GenreCode.IsGenreCode(track.Genre)) track.Genre = GenreCode.GetGenre(track.Genre);
                if (track.Artist == "") track.Artist = NoValue;
                if (track.AlbumArtist == "") track.AlbumArtist = NoValue;
                if (track.Title == "") track.Title = NoValue;
                if (track.Album == "") track.Album = NoValue;
                if (track.Genre == "") track.Genre = NoValue;
            }

            track.OriginalDescription = track.Description;
            track.FullLength = track.Length;

            if(updateLength)
                UpdateLength(track);

            UpdateKey(track);

            track.Bpm = BpmHelper.GetAdjustedBpmAverage(track.StartBpm, track.EndBpm);

            track.OriginalDescription = track.Description;

            if (track.EndBpm == 0 || track.EndBpm == 100) track.EndBpm = track.Bpm;
            if (track.StartBpm == 0 || track.StartBpm == 100) track.StartBpm = track.Bpm;
        }


        private static void UpdateKey(Track track)
        {
            var attributes = ShufflerHelper.LoadShufflerDetails(track);
            var attributeKey = (attributes == null) ? "" : attributes.ContainsKey("Key") ? attributes["Key"] : "";

            if (track.Key != "" && attributeKey == "" && track.IsShufflerTrack)
            {
                ShufflerHelper.SetAttribute("Key", track.Key, attributes);
                ShufflerHelper.SaveShufflerAttributes(track, attributes);
            }
            else if (track.Key == "" && attributeKey != "")
            {
                SaveTrack(track);
            }

            track.Key = string.IsNullOrEmpty(attributeKey) ? track.Key : attributeKey;
        }




        /// <summary>
        ///     Loads the artist and album artist for a track
        /// </summary>
        /// <param name="track">The track.</param>
        private static void LoadArtistAndAlbumArtist(Track track)
        {
            if (!track.Title.Contains("/")) return;
            var data = track.Title.Split('/').ToList();
            track.Artist = data[0].Trim();
            track.Title = data[1].Trim();
        }

        public static void SetRank(List<Track> tracks, int rank)
        {
            foreach (var track in tracks)
            {
                track.Rank = rank;
                var bassTrack = BassPlayer.LoadTrack(track.Filename);
                bassTrack.Rank = track.Rank;
                ExtenedAttributesHelper.SetExtendedAttribute(track.Description, "Rank", track.Rank.ToString());
            }
            Task.Run(() => ExtenedAttributesHelper.SaveToDatabase());
        }
    }
}