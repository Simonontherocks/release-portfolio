using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CineVault.ModelLayer.ModelMovie
{
    /// <summary>  
    /// - Dit model vertegenwoordigt een film (Movie) binnen de applicatie.  
    /// - Bevat eigenschappen zoals titel, jaar, score, en gezien-status.  
    /// - Een film is een instantie die zal worden opgeslagen in en opgehaald uit de database.   
    /// </summary>
    public class Movie
    {
        #region Field

        //private string? _year;

        #endregion

        #region Constructor

        //public Movie()
        //{
        //    //Year = _year;
        //}

        #endregion

        #region Properties

        [Key]
        public int Id { get; set; }

        // ToDO
        [JsonPropertyName("id")]
        public int TMDBId {  get; set; }

        [JsonPropertyName("title")]
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Display(Name = "Gezien")]
        public bool Seen { get; set; }

        [JsonPropertyName("vote_average")]
        [Display(Name = "Gemiddelde score op 10")]
        public double? Score { get; set; }

        // Deze property mag NULL zijn.
        [JsonPropertyName("release_date")]
        [Display(Name = "Jaar van uitgifte")]
        public string? Year { get; set; }

        #endregion

        #region navigationProperty

        // Zorgen ervoor dat een film gekoppeld kan worden aan regisseurs en acteurs via tussen-tabellen.
        public ICollection<MovieDirector> MovieDirectors { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; }

        #endregion

    }

}
