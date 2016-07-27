using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;
using IdSharp.Tagging.ID3v2;

namespace Halloumi.Shuffler.AudioLibrary.Helpers
{
    public static class AlbumCoverHelper
    {
        /// <summary>
        ///     Gets or sets the a cached collection of album covers.
        /// </summary>
        private static Dictionary<string, Image> AlbumCovers { get; }

        static AlbumCoverHelper()
        {
            AlbumCovers = new Dictionary<string, Image>();
        }

        public static Image GetAlbumCover(string albumName)
        {
            return AlbumCovers.ContainsKey(albumName) ? AlbumCovers[albumName] : null;
        }

        /// <summary>
        ///     Loads and caches an album cover.
        /// </summary>
        /// <param name="track">The track.</param>
        public static void LoadAlbumCover(Track track)
        {
            try
            {
                var path = Path.GetDirectoryName(track.Filename);
                if (path == null) return;

                var albumArtImagePath = Path.Combine(path, "AlbumArtSmall.jpg");
                var folderImagePath = Path.Combine(path, "folder.jpg");

                var albumArtImageDate = DateTime.MinValue;
                if (File.Exists(albumArtImagePath)) albumArtImageDate = File.GetLastWriteTime(albumArtImagePath);

                var folderImageDate = DateTime.MinValue;
                if (File.Exists(folderImagePath)) folderImageDate = File.GetLastWriteTime(folderImagePath);

                if (!File.Exists(folderImagePath))
                {
                    if (ID3v2Tag.DoesTagExist(track.Filename))
                    {
                        var tags = new ID3v2Tag(track.Filename);
                        if (tags.PictureList.Count > 0)
                        {
                            using (Image folderImage = new Bitmap(tags.PictureList[0].Picture))
                            {
                                ImageHelper.SaveJpg(folderImagePath, folderImage);
                            }
                        }
                    }
                }

                if (!File.Exists(albumArtImagePath) || albumArtImageDate < folderImageDate)
                {
                    if (File.Exists(folderImagePath))
                    {
                        using (Image image = new Bitmap(folderImagePath))
                        {
                            using (var smallImage = ImageHelper.Resize(image, new Size(150, 150)))
                            {
                                ImageHelper.SaveJpg(albumArtImagePath, smallImage);
                                File.SetAttributes(albumArtImagePath, FileAttributes.Hidden);
                            }
                        }
                    }
                }

                if (File.Exists(albumArtImagePath))
                {
                    Image image = new Bitmap(albumArtImagePath);
                    AlbumCovers.Add(track.Album, image);
                }
            }
            catch (Exception e)
            {
                DebugHelper.WriteLine(e.ToString());
            }
        }

        /// <summary>
        ///     Sets the track album cover.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="image">The image.</param>
        public static void SetTrackAlbumCover(Track track, Image image)
        {
            if (track == null) return;
            if (image == null) return;
            if (!ID3v2Tag.DoesTagExist(track.Filename)) return;

            var tags = new ID3v2Tag(track.Filename);
            if (tags.PictureList.Count > 0) tags.PictureList.Clear();

            var picture = tags.PictureList.AddNew();

            if (picture != null)
            {
                picture.PictureType = PictureType.CoverFront;
                picture.MimeType = "image/jpeg";

                using (var stream = new MemoryStream())
                {
                    ImageHelper.SaveJpg(stream, image);
                    picture.PictureData = stream.ToArray();
                }
            }
            tags.Save(track.Filename);
        }
    }
}
