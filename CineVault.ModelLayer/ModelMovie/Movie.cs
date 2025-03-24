using CineVault.ModelLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CineVault.ModelLayer.ModelMovie
{
    public class Movie
    {
        #region Field

        // Empty

        #endregion

        #region Properties

        [Key]
        public int Id { get; set; }

        // ToDO
        [JsonPropertyName("id")]
        public int IMDBId {  get; set; }

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
