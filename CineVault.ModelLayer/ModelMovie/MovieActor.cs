using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.ModelLayer.ModelMovie
{
    public class MovieActor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Dit is toegevoegd om een unieke id te geven
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int ActorId { get; set; }
        public Actor Actor { get; set; }

    }

}
