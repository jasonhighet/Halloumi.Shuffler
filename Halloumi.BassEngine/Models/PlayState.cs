namespace Halloumi.BassEngine.Models
{
    /// <summary>
    /// The current playback state of the bassplayer
    /// </summary>
    public enum PlayState
    {
        #region Members

        /// <summary>
        /// Playback is stopped
        /// </summary>
        Stopped,

        /// <summary>
        /// Currently playing
        /// </summary>
        Playing,

        /// <summary>
        /// Currently paused
        /// </summary>
        Paused

        #endregion
    }
}
