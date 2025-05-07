using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CineVault.ModelLayer.ModelMovie
{
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
        public string Title { get; set; }
        public bool Seen { get; set; }

        [JsonPropertyName("vote_average")]
        public double? Score { get; set; }

        // Deze property mag NULL zijn.
        [JsonPropertyName("release_date")]
        public string? Year { get; set; }

        #endregion

        #region navigationProperty

        public ICollection<MovieDirector> MovieDirectors { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; }

        #endregion

    }

}
