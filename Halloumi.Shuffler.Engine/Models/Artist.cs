namespace Halloumi.Shuffler.Engine.Models
{
    public class Artist
    {
        /// <summary>
        /// Initializes a new instance of the Artist class.
        /// </summary>
        /// <param name="name">The name of the artist.</param>
        public Artist(string name)
        {
            Name = name;
        }
        
        /// <summary>
        /// Gets or sets the name of the artist
        /// </summary>
        public string Name { get; set; }
    }
}
