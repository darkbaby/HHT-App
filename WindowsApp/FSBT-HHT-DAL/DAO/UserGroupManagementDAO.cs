using FSBT_HHT_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_DAL.DAO
{
    public class UserGroupManagementDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        private Entities dbContext = new Entities();

        private List<User> allUser;
        private List<UserGroup> allUserGroup;
        private List<ConfigUserGroup> allConfigUserGroup;
        private List<ConfigUserGroupScreen> allConfigUserGroupScreen;
        private List<ConfigUserGroupPermission> allConfigUserGroupPermission;
        private List<ConfigUserGroupReport> allConfigUserGroupReport;
        private List<MasterReport> allMasterReport;

        public UserGroupManagementDAO()
        {
            allUser = dbContext.Users.ToList();
            allUserGroup = dbContext.UserGroups.ToList();
            allConfigUserGroup = dbContext.ConfigUserGroups.ToList();
            allConfigUserGroupScreen = dbContext.ConfigUserGroupScreens.ToList();
            allConfigUserGroupPermission = dbContext.ConfigUserGroupPermissions.ToList();
            allConfigUserGroupReport = dbContext.ConfigUserGroupReports.ToList();
            allMasterReport = dbContext.MasterReports.ToList();
        }

        public void RefreshDAO()
        {
            dbContext = new Entities();
            allUser = dbContext.Users.ToList();
            allUserGroup = dbContext.UserGroups.ToList();
            allConfigUserGroup = dbContext.ConfigUserGroups.ToList();
            allConfigUserGroupScreen = dbContext.ConfigUserGroupScreens.ToList();
            allConfigUserGroupPermission = dbContext.ConfigUserGroupPermissions.ToList();
            allConfigUserGroupReport = dbContext.ConfigUserGroupReports.ToList();
            allMasterReport = dbContext.MasterReports.ToList();
        }

        public List<UserGroup> GetAllUserGroup()
        {
            try
            {
                DataTable dtTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    //SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GetUpdateDate", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dtTable);

                    conn.Close();
                }


                DataTable usedDT = new DataTable();
                usedDT.Columns.Add("groupID", typeof(string));
                usedDT.Columns.Add("tableName", typeof(string));
                usedDT.Columns.Add("timeValue", typeof(DateTime));
                DataTable tempDT = new DataTable();
                tempDT.Columns.Add("groupID", typeof(string));
                tempDT.Columns.Add("tableName", typeof(string));
                tempDT.Columns.Add("timeValue", typeof(DateTime));
                foreach (DataRow row in dtTable.Rows)
                {
                    tempDT.Clear();
                    var row1 = tempDT.NewRow();
                    var row2 = tempDT.NewRow();
                    var row3 = tempDT.NewRow();
                    var row4 = tempDT.NewRow();
                    string groupID = row[0].ToString();
                    row1["groupID"] = groupID;
                    row2["groupID"] = groupID;
                    row3["groupID"] = groupID;
                    row4["groupID"] = groupID;

                    row1["tableName"] = row.Table.Columns[1].ColumnName.ToString();
                    row1["timeValue"] = (DateTime)row[1];
                    row2["tableName"] = row.Table.Columns[2].ColumnName.ToString();
                    row2["timeValue"] = (DateTime)row[2];
                    row3["tableName"] = row.Table.Columns[3].ColumnName.ToString();
                    row3["timeValue"] = (DateTime)row[3];
                    row4["tableName"] = row.Table.Columns[4].ColumnName.ToString();
                    row4["timeValue"] = (DateTime)row[4];
                    tempDT.Rows.Add(row1);
                    tempDT.Rows.Add(row2);
                    tempDT.Rows.Add(row3);
                    tempDT.Rows.Add(row4);
                    DataView tempDV = tempDT.DefaultView;
                    tempDV.Sort = "timeValue desc";
                    tempDT = tempDV.ToTable();
                    usedDT.ImportRow(tempDT.Rows[0]);
                }

                List<UserGroup> newList = new List<UserGroup>();
                DateTime tempDate = new DateTime(1900, 1, 1);
                foreach (UserGroup item in allUserGroup)
                {
                    UserGroup temp = new UserGroup();
                    temp.GroupID = item.GroupID;
                    temp.GroupName = item.GroupName;
                    temp.CreateBy = item.CreateBy;
                    temp.CreateDate = item.CreateDate;
                    DataRow dataRow = usedDT.Select("groupID = " + item.GroupID).First();
                    if (dataRow[1].Equals("lastUpdateUserGroup"))
                    {
                        DateTime dateTime = (DateTime)dataRow[2];
                        //temp.UpdateDate = DateTime.Compare(dateTime, tempDate) == 0 ? (DateTime?)null : dateTime;
                        temp.UpdateDate = dateTime;
                        temp.UpdateBy = dbContext.UserGroups.Where(x => DateTime.Compare((DateTime)x.UpdateDate, dateTime) == 0).First().UpdateBy;

                    }
                    else if (dataRow[1].Equals("lastUpdateConfigUserGroup"))
                    {
                        DateTime dateTime = (DateTime)dataRow[2];
                        //temp.UpdateDate = DateTime.Compare(dateTime, tempDate) == 0 ? (DateTime?)null : dateTime;
                        temp.UpdateDate = dateTime;
                        temp.UpdateBy = dbContext.ConfigUserGroups.Where(x => DateTime.Compare((DateTime)x.UpdateDate, dateTime) == 0).First().UpdateBy;

                    }
                    else if (dataRow[1].Equals("lastUpdateConfigUserGroupScreen"))
                    {
                        DateTime dateTime = (DateTime)dataRow[2];
                        //temp.UpdateDate = DateTime.Compare(dateTime, tempDate) == 0 ? (DateTime?)null : dateTime;
                        temp.UpdateDate = dateTime;
                        temp.UpdateBy = dbContext.ConfigUserGroupScreens.Where(x => DateTime.Compare((DateTime)x.UpdateDate, dateTime) == 0).First().UpdateBy;

                    }
                    else if (dataRow[1].Equals("lastUpdateConfigUserGroupPermission"))
                    {
                        DateTime dateTime = (DateTime)dataRow[2];
                        //temp.UpdateDate = DateTime.Compare(dateTime, tempDate) == 0 ? (DateTime?)null : dateTime;
                        temp.UpdateDate = dateTime;
                        temp.UpdateBy = dbContext.ConfigUserGroupPermissions.Where(x => DateTime.Compare((DateTime)x.UpdateDate, dateTime) == 0).First().UpdateBy;

                    }

                    newList.Add(temp);
                }

                return newList;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new List<UserGroup>();
            }
        }

        public List<ConfigUserGroupScreen> GetAllScreen()
        {
            return allConfigUserGroupScreen;
        }

        public List<UGM_UserModel> GetAllUserFilterbyGroupID(string GroupID)
        {
            try
            {
                List<UGM_UserModel> listTemp = (from user in dbContext.Users
                                                from userGroupMap in dbContext.ConfigUserGroups.Where(x => x.Username == user.Username).DefaultIfEmpty()
                                                orderby user.Username ascending
                                                select new UGM_UserModel
                                                {
                                                    Member = userGroupMap.GroupID == GroupID ? true : false,
                                                    UserName = user.Username,
                                                    FirstName = user.FirstName,
                                                    LastName = user.LastName,
                                                    AddedBy = userGroupMap.CreateBy,
                                                    AddedDate = userGroupMap.CreateDate
                                                    //AddedBy = userGroupMap.GroupID == GroupID ? userGroupMap.UpdateBy : null,
                                                    //AddedDate = userGroupMap.GroupID == GroupID ? userGroupMap.UpdateDate : null
                                                }).ToList<UGM_UserModel>();
                return listTemp;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new List<UGM_UserModel>();
            }

        }

        public List<UGM_ScreenModel> GetAllScreenFilterByGroupID(string GroupID)
        {
            try
            {
                List<UGM_ScreenModel> listTemp = (from screenMap in dbContext.ConfigUserGroupScreens
                                                  where screenMap.GroupID == GroupID
                                                  orderby screenMap.ScreenID
                                                  select new UGM_ScreenModel
                                                  {
                                                      Visible = screenMap.Visible,
                                                      ScreenID = screenMap.ScreenID
                                                  }).ToList<UGM_ScreenModel>();
                return listTemp;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new List<UGM_ScreenModel>();
            }

        }

        public List<UGM_PermissionModel> GetAllPermissionFilterByGroupIDANDScreenID(string groupID, string screenID)
        {
            try
            {
                List<UGM_PermissionModel> listTemp = (from screenPer in dbContext.ConfigUserGroupPermissions
                                                      where screenPer.GroupID.Equals(groupID) && screenPer.ScreenID.Equals(screenID)
                                                      orderby screenPer.ComponentType
                                                      select new UGM_PermissionModel
                                                      {
                                                          ComponentAlias = screenPer.ComponentAlias,
                                                          ComponentType = screenPer.ComponentType,
                                                          AllowAdd = screenPer.AllowAdd,
                                                          AllowEdit = screenPer.AllowEdit,
                                                          AllowDelete = screenPer.AllowDelete,
                                                          Enable = screenPer.Enable,
                                                          Visible = screenPer.Visible
                                                      }).ToList<UGM_PermissionModel>();
                return listTemp;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new List<UGM_PermissionModel>();
            }

        }

        public void UpdateUserGroupName(List<UGM_GroupModel> editUGM_GroupModel, string currentUser)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                foreach (UGM_GroupModel item in editUGM_GroupModel)
                {
                    UserGroup temp = allUserGroup.Where(x => x.GroupID.Equals(item.GroupID)).First();
                    temp.GroupName = item.GroupName;
                    temp.UpdateDate = dateTime;
                    temp.UpdateBy = currentUser;
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        public void UpdateMemberInGroup(List<string> userNameListDelete, List<string> userNameListAdd, string groupID, string currentUser)
        {
            try
            {
                List<ConfigUserGroup> newConfigUserGroup = new List<ConfigUserGroup>();

                List<string> DeleteUser = new List<string>();
                DeleteUser.AddRange(userNameListDelete);
                DeleteUser.AddRange(userNameListAdd);
                List<ConfigUserGroup> ConfigUserGroup = dbContext.ConfigUserGroups.Where(x => DeleteUser.Contains(x.Username)).ToList();
                DateTime dateTime = DateTime.Now;
                foreach (ConfigUserGroup item in ConfigUserGroup)
                {
                    if (userNameListAdd.Contains(item.Username))
                    {
                        ConfigUserGroup temp = new ConfigUserGroup();
                        temp.GroupID = groupID;
                        temp.Username = item.Username;
                        temp.CreateDate = item.CreateDate;
                        temp.CreateBy = item.CreateBy;
                        temp.UpdateDate = dateTime;
                        temp.UpdateBy = currentUser;
                        newConfigUserGroup.Add(temp);
                        userNameListAdd.Remove(item.Username);
                    }
                }

                foreach (string item in userNameListAdd)
                {
                    ConfigUserGroup temp = new ConfigUserGroup();
                    temp.GroupID = groupID;
                    temp.Username = item;
                    temp.CreateDate = dateTime;
                    temp.CreateBy = currentUser;
                    temp.UpdateDate = dateTime;
                    temp.UpdateBy = currentUser;
                    newConfigUserGroup.Add(temp);
                }

                dbContext.ConfigUserGroups.RemoveRange(ConfigUserGroup);
                dbContext.ConfigUserGroups.AddRange(newConfigUserGroup);

                //allConfigUserGroup.AddRange(newConfigUserGroup);
                
                foreach (UserGroup item in allUserGroup)
                {
                    if (item.GroupID.Equals(groupID))
                    {
                        item.UpdateDate = dateTime;
                        item.UpdateBy = currentUser;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        public void UpdateScreenForGroupID(List<UGM_ScreenModel> editUGM_ScreenModel, string groupID, string currentUser)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                List<string> screenIDList = new List<string>();
                foreach (UGM_ScreenModel item in editUGM_ScreenModel)
                {
                    screenIDList.Add(item.ScreenID);
                }

                List<ConfigUserGroupScreen> filterConfigUserGroupScreenList = allConfigUserGroupScreen.Where(x => x.GroupID == groupID).ToList();
                foreach (ConfigUserGroupScreen item in filterConfigUserGroupScreenList)
                {
                    if (screenIDList.Contains(item.ScreenID))
                    {
                        item.Visible = editUGM_ScreenModel.Where(x => x.ScreenID == item.ScreenID).First().Visible;
                        item.UpdateDate = dateTime;
                        item.UpdateBy = currentUser;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        public void UpdatePermissionOfScreenForGroupID(List<UGM_PermissionModel> editUGM_PermissionModel, string screenID, string groupID, string currentUser)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                List<string> componentAliasList = new List<string>();
                foreach (UGM_PermissionModel item in editUGM_PermissionModel)
                {
                    componentAliasList.Add(item.ComponentAlias);
                }

                List<ConfigUserGroupPermission> filterConfigUserGroupPermission = allConfigUserGroupPermission.Where(x => x.GroupID == groupID && x.ScreenID == screenID).ToList();
                foreach (ConfigUserGroupPermission item in filterConfigUserGroupPermission)
                {
                    if (componentAliasList.Contains(item.ComponentAlias))
                    {
                        UGM_PermissionModel temp = editUGM_PermissionModel.Where(x => x.ComponentAlias == item.ComponentAlias).First();
                        item.AllowAdd = temp.AllowAdd;
                        item.AllowEdit = temp.AllowEdit;
                        item.AllowDelete = temp.AllowDelete;
                        item.Enable = temp.Enable;
                        item.Visible = temp.Visible;
                        item.UpdateDate = dateTime;
                        item.UpdateBy = currentUser;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }
        public void UpdateGroupReport(List<UGM_ScreenModel> editUGM_ScreenModel, string groupID, string currentUser)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                List<ConfigUserGroupReport> newConfigUserGroupReport = new List<ConfigUserGroupReport>();
                List<ConfigUserGroupReport> ConfigUserGroupReportDelete =  new List<ConfigUserGroupReport>();
                var editUGM_ReportModel = editUGM_ScreenModel.Where(x => x.ScreenID == "ReportPrintForm").ToList();
                if(editUGM_ReportModel.Count > 0)
                {
                    var showScreen = editUGM_ReportModel.FirstOrDefault();
                    if(showScreen.Visible == false)
                    {
                        ConfigUserGroupReportDelete = dbContext.ConfigUserGroupReports.Where(x => x.GroupID == groupID).ToList();
                        if (ConfigUserGroupReportDelete.Count > 0)
                        {
                            dbContext.ConfigUserGroupReports.RemoveRange(ConfigUserGroupReportDelete);
                        }
                    }
                    else
                    {
                        List<MasterReport> masterReportList = allMasterReport.ToList();
                        List<ConfigUserGroupReport> filterConfigUserGroupReportList = allConfigUserGroupReport.Where(x => x.GroupID == groupID).ToList();
                        if (filterConfigUserGroupReportList.Count() == 0)
                        {
                            foreach (MasterReport item in masterReportList)
                            {
                                ConfigUserGroupReport temp = new ConfigUserGroupReport();
                                temp.GroupID = groupID;
                                temp.ReportCode = item.ReportCode;
                                temp.Visible = true;
                                temp.CreateDate = dateTime;
                                temp.CreateBy = currentUser;
                                temp.UpdateDate = dateTime;
                                temp.UpdateBy = currentUser;
                                newConfigUserGroupReport.Add(temp);
                            }
                            dbContext.ConfigUserGroupReports.AddRange(newConfigUserGroupReport);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }
        public bool SaveChange()
        {
            try
            {
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        }
    }
}
