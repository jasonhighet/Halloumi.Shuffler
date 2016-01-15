namespace Halloumi.Shuffler.Engine
{
    public class Genre
    {
        /// <summary>
        /// Initializes a new instance of the Genre class.
        /// </summary>
        /// <param name="name">The name of the genre.</param>
        public Genre(string name)
        {
            Name = name;
        }
        
        /// <summary>
        /// Gets or sets the name of the genre
        /// </summary>
        public string Name { get; set; }
    }
}
