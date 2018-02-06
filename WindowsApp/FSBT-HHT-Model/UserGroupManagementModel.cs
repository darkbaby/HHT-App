using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_Model
{
    public class UGM_GroupModel
    {
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string UpdateBy { get; set; }

        public UGM_GroupModel()
        {

        }

        public UGM_GroupModel(UGM_GroupModel temp)
        {
            this.GroupID = temp.GroupID;
            this.GroupName = temp.GroupName;
            this.LastUpdate = temp.LastUpdate;
            this.UpdateBy = temp.UpdateBy;
        }

        public UGM_GroupModel(string groupID, string groupName, DateTime lastUpdate, string updateBy)
        {
            this.GroupID = groupID;
            this.GroupName = groupName;
            this.LastUpdate = lastUpdate;
            this.UpdateBy = updateBy;
        }
    }

    public class UGM_UserModel
    {
        public bool Member { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }

        public UGM_UserModel()
        {

        }

        public UGM_UserModel(UGM_UserModel temp)
        {
            this.Member = temp.Member;
            this.UserName = temp.UserName;
            this.FirstName = temp.FirstName;
            this.LastName = temp.LastName;
            this.AddedBy = temp.AddedBy;
            this.AddedDate = temp.AddedDate;
        }

        public UGM_UserModel(bool member, string userName, string firstName, string lastName,
            string addedBy, DateTime addedDate)
        {
            this.Member = member;
            this.UserName = userName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.AddedBy = addedBy;
            this.AddedDate = addedDate;

        }
    }

    public class UGM_ScreenModel
    {
        public bool Visible { get; set; }
        public string ScreenID { get; set; }

        public UGM_ScreenModel()
        {

        }

        public UGM_ScreenModel(UGM_ScreenModel temp)
        {
            this.Visible = temp.Visible;
            this.ScreenID = temp.ScreenID;
        }

        public UGM_ScreenModel(bool visible, string screenID)
        {
            this.Visible = visible;
            this.ScreenID = screenID;
        }
    }

    public class UGM_PermissionModel
    {
        public string ComponentAlias { get; set;}
        public string ComponentType { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool Enable { get; set; }
        public bool Visible { get; set; }

        public UGM_PermissionModel()
        {

        }

        public UGM_PermissionModel(UGM_PermissionModel temp)
        {
            this.ComponentAlias = temp.ComponentAlias;
            this.ComponentType = temp.ComponentType;
            this.AllowAdd = temp.AllowAdd;
            this.AllowEdit = temp.AllowEdit;
            this.AllowDelete = temp.AllowDelete;
            this.Enable = temp.Enable;
            this.Visible = temp.Visible;
        }

        public UGM_PermissionModel(string componentAlias,string componentType, bool allowAdd,bool allowEdit,
            bool allowDelete, bool enable, bool visible)
        {
            this.ComponentAlias = componentAlias;
            this.ComponentType = componentType;
            this.AllowAdd = allowAdd;
            this.AllowEdit = allowEdit;
            this.AllowDelete = allowDelete;
            this.Enable = enable;
            this.Visible = visible;
        }

    }
}
