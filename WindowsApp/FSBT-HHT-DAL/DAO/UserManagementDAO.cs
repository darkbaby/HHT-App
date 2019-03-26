using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_Model;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.Data.SqlClient;
using System.Reflection;

namespace FSBT_HHT_DAL.DAO
{
    public class UserManagementDAO
    {
        private LogErrorDAO logBll = new LogErrorDAO(); 
        public UserManagementDAO()
        {

        }

        public List<UserModel> GetAllUser()
        {
            List<UserModel> allUserData = new List<UserModel>();
            try
            {
                Entities dbContext = new Entities();

                var allUserDataFromDB = (from u in dbContext.Users
                                         join um in dbContext.ConfigUserGroups on u.Username equals um.Username
                                         join ug in dbContext.UserGroups on um.GroupID equals ug.GroupID
                                         select new
                                         {
                                             Username = u.Username,
                                             Password = u.Password,
                                             FirstName = u.FirstName,
                                             LastName = u.LastName,
                                             Enable = u.Enable,
                                             Lock = u.Lock,
                                             CreateDate = u.CreateDate,
                                             CreateBy = u.CreateBy,
                                             UpdateDate = u.UpdateDate,
                                             UpdateBy = u.UpdateBy,
                                             GroupName = ug.GroupName
                                         });



                foreach (var all in allUserDataFromDB)
                {
                    UserModel userData = new UserModel();
                    userData.Username = all.Username;
                    userData.Password = all.Password;
                    userData.FisrtName = all.FirstName;
                    userData.LastName = all.LastName;
                    userData.Enable = all.Enable;
                    userData.Lock = all.Lock;
                    userData.CreateDate = all.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");
                    userData.CreateBy = all.CreateBy;
                    userData.UpdateDate = all.UpdateDate != null ? all.UpdateDate.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
                    userData.UpdateBy = all.UpdateBy;
                    userData.GroupName = all.GroupName;

                    allUserData.Add(userData);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                allUserData = new List<UserModel>();
            }

            return allUserData;
        }

        public List<UGM_GroupModel> GetAllGroup()
        {
            Entities dbContext = new Entities();
            List<UGM_GroupModel> allGroup = new List<UGM_GroupModel>();
            try
            {
                allGroup = (from ug in dbContext.UserGroups
                            select new UGM_GroupModel
                              {
                                  GroupID = ug.GroupID,
                                  GroupName = ug.GroupName
                              }).ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                allGroup = new List<UGM_GroupModel>();
            }
            return allGroup;
        }

        public bool CheckUser(string username)
        {
            Entities dbContext = new Entities();

            try
            {
                User selectedUserDB = dbContext.Users.Find(username);
                if (selectedUserDB != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public string AddNewUser(UserModel newUserData)
        {
            Entities dbContext = new Entities();

            string result = "success";
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    bool res = false;
                    string cmd = "EXEC [dbo].[SCR10_SP_ADD_USERDETAIL]"
                                   + " @Username = '" + newUserData.Username + "',@EncryPassword = '" + newUserData.Password + "'"
                                   + " ,@GroupName = '" + newUserData.GroupName + "'"
                                   + " ,@FirstName = '" + newUserData.FisrtName + "',@LastName = '" + newUserData.LastName + "'"
                                   + " ,@Enable = '" + newUserData.Enable + "',@Lock = '" + newUserData.Lock + "'"
                                   + " ,@CreateBy = '" + newUserData.CreateBy + "'";

                    res = ExeScript(cmd, conn);

                    if (!res) { result = "error"; }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = "error";
            }

            return result;
        }

        public int GetLastUserNumber()
        {
            int newUserNumber = -1;
            Entities dbContext = new Entities();
            try
            {
                User lastUser = dbContext.Users.Where(x => x.Username.StartsWith("user")).ToList().Last();
                string Number = lastUser.Username.Substring(4);
                newUserNumber = Int32.Parse(Number);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                newUserNumber = 0;
            }
            return newUserNumber;
        }

        public string UpdateData(UserModel userNewData)
        {
            Entities dbContext = new Entities();

            string result = "success";
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    bool res = false;
                    string cmd = "EXEC [dbo].[SCR10_SP_UPDATE_USERDETAIL]"
                                   + " @Username = '" + userNewData.Username + "'"
                                   + " ,@NewGroupName = '" + userNewData.GroupName + "',@EncryPassword = '" + userNewData.Password + "'"
                                   + " ,@FirstName = '" + userNewData.FisrtName + "',@LastName = '" + userNewData.LastName + "'"
                                   + " ,@Enable = '" + userNewData.Enable + "',@Lock = '" + userNewData.Lock + "'"
                                   + " ,@UpdateBy = '" + userNewData.UpdateBy + "'";

                    res = ExeScript(cmd, conn);

                    if (!res) { result = "error"; }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = "error";
            }

            return result;
        }

        public bool checkIsExistsUser(string username)
        {
            bool result = false;
            try
            {
                Entities dbContext = new Entities();
                int count = 0;


                count = (from u in dbContext.Users
                         where u.Username.Equals(username)
                         select u
                            ).Count();

                if (count > 0) { result = false; }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = false;
            }
            return result;
        }

        public List<UserModel> SearchData(string word)
        {
            Entities dbContext = new Entities();

            List<UserModel> searchUserData = new List<UserModel>();
            try
            {
                var searchUserDataDB = (from u in dbContext.Users
                                        join um in dbContext.ConfigUserGroups on u.Username equals um.Username
                                        join ug in dbContext.UserGroups on um.GroupID equals ug.GroupID
                                        where u.Username.Contains(word)
                                        select new
                                        {
                                            Username = u.Username,
                                            Password = u.Password,
                                            FirstName = u.FirstName,
                                            LastName = u.LastName,
                                            Enable = u.Enable,
                                            Lock = u.Lock,
                                            CreateDate = u.CreateDate,
                                            CreateBy = u.CreateBy,
                                            UpdateDate = u.UpdateDate,
                                            UpdateBy = u.UpdateBy,
                                            GroupName = ug.GroupName
                                        });

                foreach (var search in searchUserDataDB)
                {
                    UserModel userData = new UserModel();
                    userData.Username = search.Username;
                    userData.Password = search.Password;
                    userData.FisrtName = search.FirstName;
                    userData.LastName = search.LastName;
                    userData.Enable = search.Enable;
                    userData.Lock = search.Lock;
                    userData.CreateDate = search.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");
                    userData.CreateBy = search.CreateBy;
                    userData.UpdateDate = search.UpdateDate != null ? search.UpdateDate.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
                    userData.UpdateBy = search.UpdateBy;
                    userData.GroupName = search.GroupName;

                    searchUserData.Add(userData);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                searchUserData = new List<UserModel>();
            }
            return searchUserData;
        }

        public string DeleteUserGroup(string username)
        {
            Entities dbContext = new Entities();

            List<ConfigUserGroup> userGroupToDelete = dbContext.ConfigUserGroups.Where(x => x.Username.Equals(username)).ToList();
            if (userGroupToDelete != null)
            {
                dbContext.ConfigUserGroups.RemoveRange(userGroupToDelete);
            }
            else
            {
                //do nothing
            }
            try
            {
                dbContext.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return "error";
        }

        public string DeleteUser(string username)
        {
            Entities dbContext = new Entities();

            User userToDelete = dbContext.Users.Find(username);
            if (userToDelete != null)
            {
                dbContext.Users.Remove(userToDelete);
            }
            else
            {
                return "no user";
            }
            try
            {
                dbContext.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return "error";
        }

        public bool ExeScript(string script, SqlConnection conn)
        {
            SqlCommand comm = new SqlCommand(script, conn);
            bool isExe;
            conn.Open();
            try
            {
                comm.ExecuteNonQuery();
                isExe = true;
            }
            catch(Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                isExe = false;
            }
            finally
            {
                conn.Close();
            }
            return isExe;
        }
    }
}
