using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.BusinessLogic.Service
{
    ///This class will serve to create the unique ID numbers for each person created. 
    ///This is because the setter is private in each class. <summary>
    /// This class will serve to create the unique ID numbers for each person created. 

    public static class IdGenerator
    {
        private static int _intCurrendId = 1;

        public static int GenerateId()
        {
            return _intCurrendId++;   
        }

    }

}
