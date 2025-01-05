using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.ModelLayer.ModelLayerService
{
    ///This class will serve to create the unique ID numbers for each person created. 
    ///This is because the setter is private in each class. <summary>
    /// This class will serve to create the unique ID numbers for each person created. 

    public static class IdGeneratorService
    {
        private static int _intCurrentId = 1;

        public static int GenerateId()
        {
            return _intCurrentId++;   
        }

        public static void Reset() 
        { 
            _intCurrentId = 1; 
        }

    }

}
