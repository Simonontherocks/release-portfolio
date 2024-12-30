using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.BusinessLogic.Models
{
    public class UserLog
    {
        public int Id { get; set; }
        public DateTime TimeOfDay { get; set; }
        public int AmountOfUsers { get; set; }
    }

}
