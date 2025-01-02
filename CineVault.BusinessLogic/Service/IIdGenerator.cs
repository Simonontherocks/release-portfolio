using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.BusinessLogic.Service
{
    /// <summary>
    /// This interface was created so that it could be adjusted later to the code.
    /// For example, an int would no longer be able to create IDs, because the maximum data type for an int is 2147483647.
    /// This will take up less memory in the program.
    /// </summary>
    
    public interface IIdGenerator
    {
        public int Id { get; set; }

        int GenerateId();
    }

}
