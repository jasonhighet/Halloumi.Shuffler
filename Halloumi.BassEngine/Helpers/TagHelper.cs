using System;
using System.Linq;
using System.IO;
using Un4seen.Bass.AddOn.Tags;
using System.Collections.Generic;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    public static class TagHelper
    {
        private static Dictionary<string, TagDetails> _tagDetails;

        static TagHelper()
        {
            _tagDetails = new Dictionary<string, TagDetails>();
        }

        public static TagDetails LoadTags(string filename)
        {
            var extension = Path.GetExtension(filename);
            if (extension == null ) return null;
            if (extension.ToLower() != ".mp3")
                return null;

            lock (_tagDetails)
            {
                if (_tagDetails.ContainsKey(filename))
                {
                    return _tagDetails[filename];
                }
            }
            
            var tags = BassTags.BASS_TAG_GetFromFile(filename);
            if (tags == null) throw new Exception("Cannot load tags for file " + filename);

            var tagDetails = new TagDetails
            {
                Title = tags.title,
                Artist = tags.artist,
                Album = tags.album,
                AlbumArtist = tags.albumartist,
                Genre = tags.genre,
                Gain = tags.replaygain_track_peak
            };


            var key = tags.NativeTag("InitialKey");
            if (key != "") tagDetails.Key = key;

            decimal bpm;
            if(decimal.TryParse(tags.bpm, out bpm))
                tagDetails.Bpm = BpmHelper.NormaliseBpm(bpm);

            var duration = TimeSpan.FromSeconds(tags.duration);
            if(duration.TotalMilliseconds != 0)
                tagDetails.Length = (decimal)duration.TotalMilliseconds / 1000;

            int trackNumber;
            var trackNumberTag = (tags.track + "/").Split('/')[0].Trim();
            if (int.TryParse(trackNumberTag, out trackNumber))
                tagDetails.TrackNumber = trackNumber;

            if (tagDetails.AlbumArtist == "")
                tagDetails.AlbumArtist = tagDetails.Artist;

            if (tagDetails.Title.Contains("/"))
            {
                var data = tagDetails.Title.Split('/').ToList();
                tagDetails.Artist = data[0].Trim();
                tagDetails.Title = data[1].Trim();
            }

            lock (_tagDetails)
            {
                if (!_tagDetails.ContainsKey(filename))
                {
                    _tagDetails.Add(filename, tagDetails);
                }
            }

            return tagDetails;
        }

        public class TagDetails
        {
            public string Title { get; set; }
            public string Artist { get; set; }
            public string Album { get; set; }
            public string Genre { get; set; }
            public string AlbumArtist { get; set; }
            public string Key { get; set; }
            public decimal? Bpm { get; set; }
            public decimal? Length { get; set; }
            public float? Gain { get; set; }
            public int? TrackNumber { get; set; }
        }
    }
}
