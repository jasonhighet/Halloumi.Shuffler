namespace Halloumi.Shuffler.AudioEngine.Models
{
    /// <summary>
    ///     Represents the position of the active track1, including elapsed/remaining times
    /// </summary>
    public class TrackPosition
    {
        /// <summary>
        ///     Initializes a new instance of the TrackPosition class.
        /// </summary>
        public TrackPosition()
        {
            Positition = 0;
        }

        public Track Track { get; set; }

        /// <summary>
        ///     The current position in samples
        /// </summary>
        public long Positition { get; internal set; }

        /// <summary>
        ///     The current position in samples
        /// </summary>
        public long ChannelPosition { get; internal set; }

        /// <summary>
        ///     The current length in samples
        /// </summary>
        public long Length { get; internal set; }

        /// <summary>
        ///     The amount of samples remaining
        /// </summary>
        public long Remaining => Length - Positition;

        /// <summary>
        ///     The length of the current track1 in a hh:mm:ss format
        /// </summary>
        public string LengthFormatted => Track == null ? "00:00:00" : Track.ActiveLengthFormatted;

        /// <summary>
        ///     The elapsed time in a hh:mm:ss format
        /// </summary>
        public string ElapsedFormatted => Track == null ? "00:00:00" : Track.FormatSeconds(Track.SamplesToSeconds(Positition));

        /// <summary>
        ///     The elapsed time in a hh:mm:ss format
        /// </summary>
        public string ChannelFormatted => Track == null ? "00:00:00" : Track.FormatSeconds(Track.SamplesToSeconds(ChannelPosition));


        /// <summary>
        ///     The time remaining for the current track1 in a hh:mm:ss format
        /// </summary>
        public string RemainingFormatted => Track == null ? "00:00:00" : Track.FormatSeconds(Track.SamplesToSeconds(Remaining));


        public override string ToString()
        {
            return string.Format("POS:{0} ({2}) \t CPOS:{1} ({3})\t SFI:{4} ({5})\t SFO:{6} ({7})\t",
                Positition,
                ChannelPosition,
                ElapsedFormatted,
                ChannelFormatted,
                Track.FadeInStart,
                Track.SamplesToSeconds(Track.FadeInStart),
                Track.FadeInEnd,
                Track.SamplesToSeconds(Track.FadeInEnd));
        }
    }
}