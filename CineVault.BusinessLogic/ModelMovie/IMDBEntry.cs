using System.Data;

namespace CineVault.BusinessLogic.ModelMovie
{
    /// Represents an entry from IMDb with details such as name, URL, and type.
    /// This class is used to store metadata related to movies, actors, directors, or years
    /// as represented on the IMDb platform.
        
    public class IMDBEntry
    {
        public string Name { get; set; } // The name of the item (for example, the title of the movie or the name of the actor).
        public string Url { get; set; }
        public string CoverUrl { get; set; }
        public IMDBType Type { get; set; }
    }

}
