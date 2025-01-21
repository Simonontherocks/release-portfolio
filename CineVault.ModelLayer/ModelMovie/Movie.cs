using CineVault.ModelLayer;
using CineVault.ModelLayer.ModelLayerService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.ModelLayer.ModelMovie
{
    public class Movie
    {
        #region Properties

        [Key]
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Barcode { get; set; }
        public string CoverUrl { get; set; } // URL to the cover image or thumbnail associated with the IMDb entry.        
        public IMDBEntry IMDBEntry { get; set; }
        public bool Seen { get; set; }

        [Range(0, 10, ErrorMessage = "The score must be between 0 and 10. Decimal numbers are also not accepted.")]
        public int Score { get; set; }
        public string Year { get; set; }

        #endregion

        #region navigationProperty

        public ICollection<MovieDirector> MovieDirectors { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; }

        #endregion

        #region constructor

        public Movie()
        {
            Id = IdGeneratorService.GenerateId();
        }

        #endregion

    }

}
