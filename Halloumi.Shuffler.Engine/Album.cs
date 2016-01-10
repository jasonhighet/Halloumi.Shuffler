using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halloumi.Shuffler.Engine
{
    public class Album
    {
        /// <summary>
        /// Initializes a new instance of the Album class.
        /// </summary>
        /// <param name="name">The name of the album.</param>
        public Album(string name)
        {
            this.Name = name;
        }
        
        /// <summary>
        /// Gets or sets the name of the album.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the album.
        /// </summary>
        public string AlbumArtist { get; set; }
    }
}
