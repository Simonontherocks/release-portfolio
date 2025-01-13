using CineVault.ModelLayer.ModelAbstractClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.ModelLayer.ModelMovie
{
    public class Director : Person
    {
        #region Properties

        // Using the Id- and Name properties from the abstract class "Person"

        public IMDBEntry IMDBEntry { get; set; }

        #endregion

    }

}
