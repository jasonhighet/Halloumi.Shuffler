using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halloumi.BassEngine
{
    /// <summary>
    /// Represents the position of the active track1, including elapsed/remaining times
    /// </summary>
    public class TrackPosition
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TrackPosition class.
        /// </summary>
        public TrackPosition()
        {
            this.Positition = 0;
        }

        #endregion

        #region Properties

        public Track Track
        {
            get;
            set;
        }

        /// <summary>
        /// The current position in samples
        /// </summary>
        public long Positition
        {
            get;
            internal set;
        }

        /// <summary>
        /// The current position in samples
        /// </summary>
        public long ChannelPosition
        {
            get;
            internal set;
        }

        /// <summary>
        /// The current length in samples
        /// </summary>
        public long Length
        {
            get;
            internal set;
        }

        /// <summary>
        /// The amount of samples remaining
        /// </summary>
        public long Remaining
        {
            get
            {
                return this.Length - this.Positition;
            }
        }

        /// <summary>
        /// The length of the current track1 in a hh:mm:ss format
        /// </summary>
        public string LengthFormatted
        {
            get
            {
                if (this.Track == null) return "00:00:00";
                return this.Track.ActiveLengthFormatted;
            }
        }

        /// <summary>
        /// The elapsed time in a hh:mm:ss format
        /// </summary>
        public string ElapsedFormatted
        {
            get
            {
                if (this.Track == null) return "00:00:00";
                return this.Track.FormatSeconds(this.Track.SamplesToSeconds(this.Positition));
            }
        }

        /// <summary>
        /// The elapsed time in a hh:mm:ss format
        /// </summary>
        public string ChannelFormatted
        {
            get
            {
                if (this.Track == null) return "00:00:00";
                return this.Track.FormatSeconds(this.Track.SamplesToSeconds(this.ChannelPosition));
            }
        }


        /// <summary>
        /// The time remaining for the current track1 in a hh:mm:ss format
        /// </summary>
        public string RemainingFormatted
        {
            get
            {
                if (this.Track == null) return "00:00:00";
                return this.Track.FormatSeconds(this.Track.SamplesToSeconds(this.Remaining));
            }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Pos:{0} ({2}) \t CPos:{1} ({3})\t SFI:{4} ({5})\t SFO:{6} ({7})\t", 
                this.Positition, 
                this.ChannelPosition, 
                this.ElapsedFormatted, 
                this.ChannelFormatted,
                this.Track.FadeInStart,
                this.Track.SamplesToSeconds(this.Track.FadeInStart),
                this.Track.FadeInEnd,
                this.Track.SamplesToSeconds(this.Track.FadeInEnd));
        }
    }
}
