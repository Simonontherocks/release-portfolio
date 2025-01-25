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
         * Here the abstract class is created. 
         * This will serve as a template for the other classes. 
         * The advantage is that it is reusable in the future.
         */

        #region Properties

        [Key]
        public int Id { get; private set; }            
        public string Name { get; set; }

        #endregion


    }

}
