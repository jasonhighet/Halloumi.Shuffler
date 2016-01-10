using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Halloumi.BassEngine;

namespace Halloumi.Shuffler.Engine
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
            this.Filename = "";
            this.AlbumArtist = "";
            this.Album = "";
            this.Artist = "";
            this.BPM = 0;
            this.LastModified = DateTime.MinValue;
            this.Genre = "";
            this.Length = 0;
            this.Title = "";
            this.TrackNumber = 0;
            this.CannotCalculateBPM = false;
            this.IsShufflerTrack = false;
            this.ShufflerAttribuesFile = "";
            this.ShufflerMixesFile = "";
            this.StartBPM = 0;
            this.EndBPM = 0;
            this.Rank = 1;
            this.PowerDown = false;
            this.Key = "";
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
            get { return this.TrackNumber == 0 ? "" : this.TrackNumber.ToString(); }
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
                return BassHelper.GetFormattedLength(this.Length);
            }
        }

        /// <summary>
        /// Gets the full length of the track (excludes fade in/out details) formatted as a string
        /// </summary>
        public string FullLengthFormatted
        {
            get
            {
                return BassHelper.GetFormattedLength(this.FullLength);
            }
        }

        /// <summary>
        /// Gets or sets the BPM.
        /// </summary>
        public decimal BPM { get; set; }

        /// <summary>
        /// Gets or sets the start BPM.
        /// </summary>
        public decimal StartBPM { get; set; }

        /// <summary>
        /// Gets or sets the end BPM.
        /// </summary>
        public decimal EndBPM { get; set; }

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
                if (this.Rank == 5) return "Excellent";
                if (this.Rank == 4) return "Very Good";
                if (this.Rank == 3) return "Good";
                if (this.Rank == 2) return "Bearable";
                if (this.Rank == 0) return "Forbidden";
                return "Unranked";
            }
        }

        /// <summary>
        /// If true, the BPM calculater cannot calculate the BPM for this track
        /// </summary>
        public bool CannotCalculateBPM { get; set; }

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
            get { return this.Artist + " - " + this.Title; }
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
            if (this.Description != "") return this.Description;
            return base.ToString();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the track powers down at the end
        /// </summary>
        public bool PowerDown { get; set; }

        #endregion
    }
}