using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_Model;

namespace FSBT_HHT_DAL.DAO
{
    public class PermissionDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        private Entities dbContext = new Entities();

        public List<PermissionModel> LoadPermissionScreen(string username)
        {
            List<PermissionModel> lstMenu = new List<PermissionModel>();
            try
            {
                lstMenu = (from user in dbContext.Users
                           join userGroup in dbContext.ConfigUserGroups on user.Username equals userGroup.Username
                           join userGroupScreen in dbContext.ConfigUserGroupScreens on userGroup.GroupID equals userGroupScreen.GroupID
                           join screen in dbContext.MasterScreens on userGroupScreen.ScreenID equals screen.ScreenID
                           where user.Username == username
                           select new PermissionModel
                          {
                              ScreenID = screen.ScreenID,
                              ScreenName = screen.ScreenName,
                              Visible = userGroupScreen.Visible,
                              MenuName = screen.MenuName
                          }).ToList<PermissionModel>();
                return lstMenu;
            }
            catch (Exception ex)
            {

                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return lstMenu;
            }
        }

        public List<PermissionComponentModel> LoadPermissionComponentByUserScreen(string username, string ScreenID)
        {
            List<PermissionComponentModel> lstcomp = new List<PermissionComponentModel>();
            try
            {
                lstcomp = (from user in dbContext.Users
                           join userGroup in dbContext.ConfigUserGroups on user.Username equals userGroup.Username
                           join ScreenPermiss in dbContext.ConfigUserGroupPermissions on userGroup.GroupID equals ScreenPermiss.GroupID
                           join screen in dbContext.MasterScreens on ScreenPermiss.ScreenID equals screen.ScreenID
                           where user.Username == username && ScreenPermiss.ScreenID == ScreenID
                           select new PermissionComponentModel
                           {
                               ScreenID = screen.ScreenID,
                               ComponentName = ScreenPermiss.ComponentName,
                               ComponentType = ScreenPermiss.ComponentType,
                               ComponentAlias = ScreenPermiss.ComponentAlias,
                               AllowAdd = ScreenPermiss.AllowAdd,
                               AllowEdit = ScreenPermiss.AllowEdit,
                               AllowDelete = ScreenPermiss.AllowDelete,
                               Enable = ScreenPermiss.Enable,
                               Visible = ScreenPermiss.Visible,

                           }).ToList<PermissionComponentModel>();
                return lstcomp;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return lstcomp;
            }
        }

        public List<PermissionComponentModel> LoadPermissionComponentByUser(string username)
        {
            List<PermissionComponentModel> lstcomp = new List<PermissionComponentModel>();
            try
            {
                lstcomp = (from user in dbContext.Users
                           join userGroup in dbContext.ConfigUserGroups on user.Username equals userGroup.Username
                           join ScreenPermiss in dbContext.ConfigUserGroupPermissions on userGroup.GroupID equals ScreenPermiss.GroupID
                           join screen in dbContext.MasterScreens on ScreenPermiss.ScreenID equals screen.ScreenID
                           where user.Username == username
                           select new PermissionComponentModel
                           {
                               ScreenID = screen.ScreenID,
                               ComponentName = ScreenPermiss.ComponentName,
                               ComponentType = ScreenPermiss.ComponentType,
                               ComponentAlias = ScreenPermiss.ComponentAlias,
                               AllowAdd = ScreenPermiss.AllowAdd,
                               AllowEdit = ScreenPermiss.AllowEdit,
                               AllowDelete = ScreenPermiss.AllowDelete,
                               Enable = ScreenPermiss.Enable,
                               Visible = ScreenPermiss.Visible,

                           }).ToList<PermissionComponentModel>();
                return lstcomp;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return lstcomp;
            }
        }
    
    }
}
