namespace Halloumi.Shuffler.AudioEngine.Channels
{
    /// <summary>
    /// Represents the current left and right volume levels
    /// </summary>
    public class VolumeLevels
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the VolumeLevels class.
        /// </summary>
        public VolumeLevels()
        {
            Right = 0;
            Left = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The left channel volume level
        /// </summary>
        public int Left
        {
            get;
            internal set;
        }

        /// <summary>
        /// The right channel volume level
        /// </summary>
        public int Right
        {
            get;
            internal set;
        }

        #endregion
    }
}
