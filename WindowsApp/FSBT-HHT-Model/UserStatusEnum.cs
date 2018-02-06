using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_Model
{
    public enum UserStatus
    {
        UNREGISTER_DB = 0,
        INCORRECT_PASSWORD = 1,
        INACTIVE_STATUS = 2,
        LOCKED_USER = 3,
        SUCCESS = 4,
        ERROR = -99
    }
}
