using CineVault.ModelLayer;
using CineVault.ModelLayer.ModelLayerService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.ModelLayer.ModelMovie
{
    public class Movie
    {
        #region Field

        private double? _score;

        #endregion

        #region Properties

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Dit is toegevoegd om een unieke id te geven
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Seen { get; set; }
        public double? Score
        {
            get
            {
                return _score;
            }
            set
            {
                if (value < 0 || value > 10)
                {
                    throw new ArgumentException("Value must be at least 0 and can be at most 10.");
                }
                else if (value % 0.5 != 0)
                {
                    throw new ArgumentException("Value can only be 0.5 after the decimal point.");
                }
                else
                {
                    _score = value;
                }
            }
        }

        // Deze property mag NULL zijn.
        public string? Year { get; set; }

        #endregion

        #region navigationProperty

        public ICollection<MovieDirector> MovieDirectors { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; }

        #endregion

    }

}
