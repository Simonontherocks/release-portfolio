using CineVault.BusinessLogic.ModelAbstractClass;
using CineVault.BusinessLogic.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CineVault.BusinessLogic.ModelUser
{
    public class User : Person
    {
        #region Properties

        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateOfCreation { get; set; }

        #endregion

        #region constructor

        #endregion

    }

}
