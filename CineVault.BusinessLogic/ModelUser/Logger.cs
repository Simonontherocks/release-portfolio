using CineVault.BusinessLogic.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.BusinessLogic.ModelUser
{
    public class Logger
    {
        #region Properties

        public int Id { get; private set; }
        public DateTime TimeOfDay { get; set; }
        public int? UserId { get; set; } // Nullable voor system logs
        public string Description { get; set; }
        public LogType Type { get; set; }

        #endregion

        #region constructor

        public Logger()
        {
            Id = IdGenerator.GenerateId();
        }

        #endregion

    }

}
