using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.BusinessLogic.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IMDBEntry IMDBEntry { get; set; }
    }

}
