using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_Model;

namespace FSBT_HHT_BLL
{
    public class PermissionScreenBll
    {
        private PermissionDAO permissDAO = new PermissionDAO();

        public List<PermissionModel> GetPermissionScreenByUser(string username)
        {
            List<PermissionModel> lstScreen = new List<PermissionModel>();
            lstScreen = permissDAO.LoadPermissionScreen(username);
            return lstScreen;
        }
    }
}
