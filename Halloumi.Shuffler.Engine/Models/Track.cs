using System;
using Halloumi.Shuffler.AudioEngine.Helpers;

namespace Halloumi.Shuffler.AudioLibrary.Models
{
    /// <summary>
    /// Represents a track in the library
    /// </summary>
    [Serializable]
    public class Track
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Track class.
        /// </summary>
        public Track()
        {
            Filename = "";
            AlbumArtist = "";
            Album = "";
            Artist = "";
            Bpm = 0;
            LastModified = DateTime.MinValue;
            Genre = "";
            Length = 0;
            Title = "";
            TrackNumber = 0;
            CannotCalculateBpm = false;
            IsShufflerTrack = false;
            ShufflerAttribuesFile = "";
            ShufflerMixesFile = "";
            StartBpm = 0;
            EndBpm = 0;
            Rank = 1;
            PowerDown = false;
            Key = "";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Gets or sets the album artist.
        /// </summary>
        public string AlbumArtist { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the track number.
        /// </summary>
        public int TrackNumber { get; set; }

        /// <summary>
        /// Gets the formatted track number.
        /// </summary>
        public string TrackNumberFormatted
        {
            get { return TrackNumber == 0 ? "" : TrackNumber.ToString(); }
        }

        /// <summary>
        /// Gets or sets the album.
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public decimal Length { get; set; }

        /// <summary>
        /// Gets the formatted length.
        /// </summary>
        public string LengthFormatted
        {
            get
            {
                return TimeFormatHelper.GetFormattedHours(Length);
            }
        }

        /// <summary>
        /// Gets the full length of the track (excludes fade in/out details) formatted as a string
        /// </summary>
        public string FullLengthFormatted
        {
            get
            {
                return TimeFormatHelper.GetFormattedHours(FullLength);
            }
        }

        /// <summary>
        /// Gets or sets the BPM.
        /// </summary>
        public decimal Bpm { get; set; }

        /// <summary>
        /// Gets or sets the start BPM.
        /// </summary>
        public decimal StartBpm { get; set; }

        /// <summary>
        /// Gets or sets the end BPM.
        /// </summary>
        public decimal EndBpm { get; set; }

        /// <summary>
        /// Gets or sets the date modified.
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Gets or sets the rank
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets the rank
        /// </summary>
        public string RankDescription
        {
            get
            {
                if (Rank == 5) return "Excellent";
                if (Rank == 4) return "Very Good";
                if (Rank == 3) return "Good";
                if (Rank == 2) return "Bearable";
                if (Rank == 0) return "Forbidden";
                return "Unranked";
            }
        }

        /// <summary>
        /// If true, the BPM calculater cannot calculate the BPM for this track
        /// </summary>
        public bool CannotCalculateBpm { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shuffler attributes exist for this track.
        /// </summary>
        public bool IsShufflerTrack { get; set; }

        /// <summary>
        /// Gets or sets the shuffler attribues file.
        /// </summary>
        public string ShufflerAttribuesFile { get; set; }

        /// <summary>
        /// Gets or sets the shuffler mixes file.
        /// </summary>
        public string ShufflerMixesFile { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description
        {
            get { return Artist + " - " + Title; }
        }

        /// <summary>
        /// Gets description at the time the track was loaded
        /// </summary>
        public string OriginalDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Gets full length of the track (excludes fade in/out details)
        /// </summary>
        public decimal FullLength
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a String that represents this instance.
        /// </summary>
        public override string ToString()
        {
            if (Description != "") return Description;
            return base.ToString();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the track powers down at the end
        /// </summary>
        public bool PowerDown { get; set; }

        #endregion
    }
}