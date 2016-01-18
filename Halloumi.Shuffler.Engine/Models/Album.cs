namespace Halloumi.Shuffler.Engine.Models
{
    public class Album
    {
        /// <summary>
        /// Initializes a new instance of the Album class.
        /// </summary>
        /// <param name="name">The name of the album.</param>
        public Album(string name)
        {
            Name = name;
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
