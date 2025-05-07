using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineVault.ModelLayer.ModelMovie
{
    public class MovieDirector
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Dit is toegevoegd om een unieke id te geven
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie movie { get; set; }

        public int DirectorId { get; set; }
        public Director Director { get; set; }

    }

}
