using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.BusinessLogic.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Barcode { get; set; }
        public string Director { get; set; }
        public List<string> Actors { get; set; } = new List<string>();
        public string IMDBUrl { get; set; }
        public bool Owned { get; set; }
        public bool Seen { get; set; }
        public int Score { get; set; } // 0 - 10
    }

}
