using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halloumi.BassEngine
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
            this.Right = 0;
            this.Left = 0;
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
