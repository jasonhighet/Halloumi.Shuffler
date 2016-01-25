using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Halloumi.BassEngine.Models;
using Halloumi.BassEngine.Properties;
using Halloumi.Common.Helpers;
using IdSharp.Tagging.ID3v2;

namespace Halloumi.BassEngine.Helpers
{
    public static class TrackHelper
    {
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);


        /// <summary>
        ///     Picks a random track from a list of tracks
        /// </summary>
        /// <param name="tracks">The track list.</param>
        /// <returns>A randomly selected track</returns>
        public static Track GetRandomTrack(List<Track> tracks)
        {
            if (tracks == null) return null;
            return tracks.Count == 0 ? null : tracks[Random.Next(0, tracks.Count)];
        }

        /// <summary>
        ///     Guesses the artist and title of a track from its filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>A guess at the artist and filename</returns>
        public static TrackDetails GuessTrackDetailsFromFilename(string filename)
        {
            filename = (Path.GetFileNameWithoutExtension(filename) + "").Replace("_", " ").Trim();
            var elements = filename.Split('-').ToList();

            var trackDetails = new TrackDetails
            {
                AlbumArtist = "",
                Artist = "",
                Title = "",
                Description = ""
            };

            if (elements.Count > 3) for (var i = 3; i < elements.Count; i++) elements[2] += "-" + elements[i];

            switch (elements.Count)
            {
                case 1:
                    trackDetails.Title = elements[0].Trim();
                    break;
                case 2:
                    trackDetails.Artist = elements[0].Trim();
                    trackDetails.Title = elements[1].Trim();
                    break;
                case 3:
                    int trackNumber;
                    if (int.TryParse(elements[0], out trackNumber))
                    {
                        trackDetails.Artist = elements[1].Trim();
                        trackDetails.Title = elements[2].Trim();
                        trackDetails.TrackNumber = trackNumber.ToString();
                    }
                    else
                    {
                        trackDetails.Artist = elements[0].Trim();
                        trackDetails.Title = (elements[1] + "-" + elements[2]).Trim();
                    }
                    break;
            }

            trackDetails.AlbumArtist = trackDetails.Artist;
            if (trackDetails.Artist.ToLower().StartsWith("various") || trackDetails.Title.Contains("  "))
            {
                trackDetails.Title = trackDetails.Title.Replace("  ", "/");
                elements = trackDetails.Title.Split('/').ToList();
                if (elements.Count == 2)
                {
                    trackDetails.Artist = elements[0].Trim();
                    trackDetails.Title = elements[1].Trim();
                }
            }

            trackDetails.Description = GuessTrackDescription(filename, trackDetails.Artist, trackDetails.Title);

            return trackDetails;
        }

        /// <summary>
        ///     Guesses the track description.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="artist">The artist.</param>
        /// <param name="title">The title.</param>
        /// <returns>The track description</returns>
        public static string GuessTrackDescription(string filename, string artist, string title)
        {
            string description;

            if (artist != "" && title != "")
            {
                description = $"{artist} - {title}";
            }
            else
            {
                description = filename.Trim();

                var regex = new Regex("various artists", RegexOptions.IgnoreCase);
                description = regex.Replace(description, "");

                regex = new Regex("various artist", RegexOptions.IgnoreCase);
                description = regex.Replace(description, "");

                regex = new Regex("various", RegexOptions.IgnoreCase);
                description = regex.Replace(description, "");

                regex = new Regex("[0-9]+", RegexOptions.IgnoreCase);
                description = regex.Replace(description, "");

                description = description.Replace("_", " ");
                description = description.Replace(".", " ");

                description = StringHelper.TitleCase(description.Trim());
            }

            return description;
        }


        /// <summary>
        ///     Loads the track image.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void LoadTrackImage(Track track)
        {
            if (track.Image != null) return;

            track.Image = Resources.DefaultMusicImage;
            var tags = ID3v2Helper.CreateID3v2(track.Filename);
            if (tags.PictureList.Count <= 0) return;

            try
            {
                var picture = tags.PictureList[0];
                using (var stream = new MemoryStream(picture.PictureData))
                {
                    track.Image = Image.FromStream(stream);
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Saves the track BPM tag.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void SaveTrackBpmTag(Track track)
        {
            var tags = ID3v2Helper.CreateID3v2(track.Filename);
            tags.BPM = track.TagBpm.ToString(CultureInfo.InvariantCulture);
            tags.Save(track.Filename);
        }

        public static bool IsSameTrack(Track track1, Track track2)
        {
            if (track1 == null || track2 == null) return false;
            return (track1.Description == track2.Description);
        }

        /// <summary>
        ///     Track details
        /// </summary>
        public class TrackDetails
        {
            public TrackDetails()
            {
                Artist = "";
                Title = "";
                TrackNumber = "";
            }

            public string Title { get; set; }

            public string Artist { get; set; }

            public string Description { get; set; }

            public string AlbumArtist { get; set; }

            public string TrackNumber { get; set; }
        }
    }
}