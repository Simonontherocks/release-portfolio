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
        public int IMDBId {  get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
        public bool Seen { get; set; }

        [JsonPropertyName("vote_average")]
        public double? Score { get; set; }

        // Deze property mag NULL zijn.
        [JsonPropertyName("release_date")]
        public string? Year { get; set; }

        //public string? Year
        //{
        //    get
        //    {
        //        return _year;
        //    }
        //    set
        //    {
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            // Probeer de waarde als datum te parseren
        //            if (DateTime.TryParse(value, out DateTime parsedDate))
        //            {
        //                _year = parsedDate.Year.ToString(); // Sla alleen het jaar op
        //            }
        //            else
        //            {
        //                throw new ArgumentException("De opgegeven waarde is geen geldige datum.", nameof(value));
        //            }
        //        }
        //        else
        //        {
        //            _year = null; // Stel null in als de waarde leeg is
        //        }
        //    }
        //}

        #endregion

        #region navigationProperty

        public ICollection<MovieDirector> MovieDirectors { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; }

        #endregion

    }

}
