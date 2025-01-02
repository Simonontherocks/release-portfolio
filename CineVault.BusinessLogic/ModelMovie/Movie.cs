using CineVault.BusinessLogic.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.BusinessLogic.ModelMovie
{
    public class Movie
    {
        #region Properties

        public int Id { get; protected set; }
        public string Title { get; set; }
        public string Barcode { get; set; }
        public string CoverUrl { get; set; } // URL to the cover image or thumbnail associated with the IMDb entry.
        public Director Director { get; set; }
        public List<Actor> Actors { get; set; } = new List<Actor>();
        public IMDBEntry IMDBEntry { get; set; }
        public bool Seen { get; set; }

        [Range(0, 10, ErrorMessage = "The score must be between 0 and 10. Decimal numbers are also not accepted.")]
        public int Score { get; set; }
        public string Year { get; set; }

        #endregion

        #region Constructor

        public Movie() //(IIdGenerator idGenerator)
        {
            //IdGenerator.GenerateId();
        }

        #endregion
    }

}
