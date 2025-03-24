using CineVault.ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.ModelLayer.ModelUser
{
    public class UserLog
    {
        #region Properties

        public int Id { get; private set; }
        public DateTime TimeOfDay { get; set; }
        public int AmountOfUsers { get; set; }

        #endregion

        #region constructor

        //public UserLog()
        //{
        //    Id = IdGeneratorService.GenerateId();
        //}

        #endregion

    }

}
