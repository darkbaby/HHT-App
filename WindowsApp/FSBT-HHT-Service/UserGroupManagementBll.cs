using FSBT_HHT_DAL;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_BLL
{
    public class UserGroupManagementBll
    {
        private UserGroupManagementDAO dao = new UserGroupManagementDAO();

        private string currentUser;

        private DataTable dtAllUserGroup;
        private DataTable dtUserByGroup;
        private DataTable dtScreenByGroup;
        private DataTable dtPermissionGroupAndScreen;

        public UserGroupManagementBll(string currentUser)
        {
            this.currentUser = currentUser;

            dtAllUserGroup = new DataTable();
            dtAllUserGroup.Columns.Add("GroupID");
            dtAllUserGroup.Columns.Add("GroupName");
            dtAllUserGroup.Columns.Add("LastUpdate");
            dtAllUserGroup.Columns.Add("UpdateBy");

            dtUserByGroup = new DataTable();
            dtUserByGroup.Columns.Add("Member", typeof(Boolean));
            dtUserByGroup.Columns.Add("UserName");
            dtUserByGroup.Columns.Add("FirstName");
            dtUserByGroup.Columns.Add("LastName");
            dtUserByGroup.Columns.Add("AddedBy");
            dtUserByGroup.Columns.Add("AddedDate");

            dtScreenByGroup = new DataTable();
            dtScreenByGroup.Columns.Add("Visible", typeof(Boolean));
            dtScreenByGroup.Columns.Add("ScreenID");

            dtPermissionGroupAndScreen = new DataTable();
            dtPermissionGroupAndScreen.Columns.Add("ComponentAlias");
            dtPermissionGroupAndScreen.Columns.Add("ComponentType");
            dtPermissionGroupAndScreen.Columns.Add("AllowAdd", typeof(Boolean));
            dtPermissionGroupAndScreen.Columns.Add("AllowEdit", typeof(Boolean));
            dtPermissionGroupAndScreen.Columns.Add("AllowDelete", typeof(Boolean));
            dtPermissionGroupAndScreen.Columns.Add("Enable", typeof(Boolean));
            dtPermissionGroupAndScreen.Columns.Add("Visible", typeof(Boolean));
        }

        public void MediumToRefreshDAO()
        {
            dao.RefreshDAO();
        }

        public DataTable GetListAllUserGroup()
        {
            List<UserGroup> listUserGroup = dao.GetAllUserGroup();

            dtAllUserGroup.Rows.Clear();
            foreach (UserGroup item in listUserGroup)
            {
                DataRow row = dtAllUserGroup.NewRow();
                row["GroupID"] = item.GroupID;
                row["GroupName"] = item.GroupName;
                //if (item.UpdateDate.HasValue)
                //{
                    row["LastUpdate"] = item.UpdateDate;
                //}
                //else
                //{
                //    row["LastUpdate"] = null;
                //}
                row["UpdateBy"] = item.UpdateBy;
                dtAllUserGroup.Rows.Add(row);
            }

            return dtAllUserGroup;
        }

        public DataTable GetListUserByGroup(string groupID)
        {
            List<UGM_UserModel> listUser = dao.GetAllUserFilterbyGroupID(groupID);

            dtUserByGroup.Rows.Clear();
            foreach (UGM_UserModel item in listUser)
            {
                var row = dtUserByGroup.NewRow();
                row["Member"] = item.Member;
                row["UserName"] = item.UserName;
                row["FirstName"] = item.FirstName;
                row["LastName"] = item.LastName;
                row["AddedBy"] = item.AddedBy;
                row["AddedDate"] = item.AddedDate;
                dtUserByGroup.Rows.Add(row);
            }
            return dtUserByGroup;
        }

        public DataTable GetScreenByGroup(string groupID)
        {
            List<UGM_ScreenModel> listScreen = dao.GetAllScreenFilterByGroupID(groupID);

            dtScreenByGroup.Rows.Clear();
            foreach (UGM_ScreenModel item in listScreen)
            {
                var row = dtScreenByGroup.NewRow();
                row["Visible"] = item.Visible;
                row["ScreenID"] = item.ScreenID;
                dtScreenByGroup.Rows.Add(row);
            }
            return dtScreenByGroup;
        }

        public DataTable GetPermissionByGroupIDANDScreenID(string groupID,string screenID)
        {
            List<UGM_PermissionModel> listPermission = dao.GetAllPermissionFilterByGroupIDANDScreenID(groupID, screenID);

            dtPermissionGroupAndScreen.Rows.Clear();
            foreach (UGM_PermissionModel item in listPermission)
            {
                var row = dtPermissionGroupAndScreen.NewRow();
                row["ComponentAlias"] = item.ComponentAlias;
                row["ComponentType"] = item.ComponentType;
                row["AllowAdd"] = item.AllowAdd;
                row["AllowEdit"] = item.AllowEdit;
                row["AllowDelete"] = item.AllowDelete;
                row["Enable"] = item.Enable;
                row["Visible"] = item.Visible;
                dtPermissionGroupAndScreen.Rows.Add(row);
            }
            return dtPermissionGroupAndScreen;
        }

        public void UpdateChangedUserGroupName(List<UGM_GroupModel> ugm_GroupModel)
        {
            dao.UpdateUserGroupName(ugm_GroupModel, currentUser);
        }

        public void UpdateChangedMemberGroup(List<UGM_UserModel> userGroupManagementModelUser,string groupID)
        {
            List<string> userNameListDelete = new List<string>();
            List<string> userNameListAdd = new List<string>();
            foreach (UGM_UserModel item in userGroupManagementModelUser)
            {
                if (item.Member)
                {
                    userNameListAdd.Add(item.UserName);
                }
                else
                {
                    userNameListDelete.Add(item.UserName);
                }
            }
            dao.UpdateMemberInGroup(userNameListDelete, userNameListAdd, groupID,currentUser);
        }

        public void UpdateChangedVisibleScreen(List<UGM_ScreenModel> ugm_ScreenModel,string groupID)
        {
            dao.UpdateScreenForGroupID(ugm_ScreenModel, groupID, currentUser);
        }

        public void UpdateChangedPermission(List<UGM_PermissionModel> ugm_PermissionModel, string screenID, string groupID)
        {
            dao.UpdatePermissionOfScreenForGroupID(ugm_PermissionModel, screenID, groupID,currentUser);
        }

        public void UpdateChangedGroupReport(List<UGM_ScreenModel> ugm_ScreenModel, string groupID)
        {
            dao.UpdateGroupReport(ugm_ScreenModel, groupID, currentUser);
        }

        public bool SaveChange()
        {
            return dao.SaveChange();
        }
    }
}
