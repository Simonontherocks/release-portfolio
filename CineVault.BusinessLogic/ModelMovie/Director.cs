using CineVault.BusinessLogic.ModelAbstractClass;
using CineVault.BusinessLogic.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.BusinessLogic.ModelMovie
{
    public class Director : Person
    {
        #region Properties

        //public int Id { get; private set; }
        //public string Name { get; set; }
        public IMDBEntry IMDBEntry { get; set; }

        #endregion

    }

}
