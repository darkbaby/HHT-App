using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_Model
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FisrtName { get; set; }
        public string LastName { get; set; }
        public string GroupName { get; set; }
        public bool Enable { get; set; }
        public bool Lock { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}
