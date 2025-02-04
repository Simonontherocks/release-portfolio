using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineVault.ModelLayer.ModelLayerService;

namespace CineVault.ModelLayer.ModelAbstractClass
{
    public abstract class Person
    {
        /*
         * Hier wordt de abstracte klasse gemaakt.
         * Dit zal dienen als een sjabloon voor de andere klassen.
         * Het voordeel is dat het in de toekomst herbruikbaar is.
         */

        #region Properties

        [Key]
        public int Id { get; private set; }
        public string Name { get; set; }

        #endregion


    }

}
