using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_DAL;
using FSBT_HHT_Model;
using System.Reflection;

namespace FSBT_HHT_DAL.DAO
{
    public class AuthenticationDAO
    {
        private LogErrorDAO logBll = new LogErrorDAO();
        public UserStatus getLoginUser(string username, string password)
        {
            Entities dbContext = new Entities();
            UserStatus userStatus = UserStatus.ERROR;
            try
            {
                List<User> user = new List<User>();
                user = (from u in dbContext.Users
                        where u.Username.Equals(username)
                        select u).ToList();

                if (user.Count > 0)
                {
                    if (user.FirstOrDefault().Enable)
                    {
                        if (!user.FirstOrDefault().Lock)
                        {
                            if (user.FirstOrDefault().Password.Equals(password))
                            {
                                userStatus = UserStatus.SUCCESS;
                                if (user.FirstOrDefault().WrongPasswordCount != 0)
                                {
                                    try
                                    {
                                        user.FirstOrDefault().WrongPasswordCount = 0;
                                        dbContext.SaveChanges();
                                    }
                                    catch
                                    {
                                        userStatus = UserStatus.ERROR;
                                    }
                                }
                            }
                            else
                            {
                                userStatus = UserStatus.INCORRECT_PASSWORD;
                                try
                                {
                                    int loginFailLimit = (int)(from s in dbContext.SystemSettings
                                                               where s.SettingKey.Equals("LoginFailLimit")
                                                               select s.ValueInt).FirstOrDefault();

                                    user.FirstOrDefault().WrongPasswordCount += 1;
                                    if (user.FirstOrDefault().WrongPasswordCount == loginFailLimit)
                                    {
                                        user.FirstOrDefault().Lock = true;
                                        user.FirstOrDefault().WrongPasswordCount = 0;
                                    }
                                    dbContext.SaveChanges();
                                }
                                catch
                                {
                                    userStatus = UserStatus.ERROR;
                                }
                            }
                        }
                        else
                        {
                            userStatus = UserStatus.LOCKED_USER;
                        }
                    }
                    else
                    {
                        userStatus = UserStatus.INACTIVE_STATUS;
                    }
                }
                else
                {
                    userStatus = UserStatus.UNREGISTER_DB;
                }
                return userStatus;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return userStatus;
            }
        }

        public int MaxLenPassword()
        {
            int maxLenPassword = 0;
            try
            {
                Entities dbContext = new Entities();
                maxLenPassword = (int)(from s in dbContext.SystemSettings
                                       where s.SettingKey.Equals("PasswordMaxLength")
                                       select s.ValueInt).FirstOrDefault();
                return maxLenPassword;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return maxLenPassword;
            }
        }
        public int MinLenPassword()
        {
            int minLenPassword = 0;
            try
            {
                Entities dbContext = new Entities();
                minLenPassword = (int)(from s in dbContext.SystemSettings
                                       where s.SettingKey.Equals("PasswordMinLength")
                                       select s.ValueInt).FirstOrDefault();

                return minLenPassword;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return minLenPassword;
            }
        }
        public int MaxLenUsername()
        {
            int maxLenUsername = 0;
            try
            {
            Entities dbContext = new Entities();
            maxLenUsername = (int)(from s in dbContext.SystemSettings
                                       where s.SettingKey.Equals("UsernameMaxLength")
                                       select s.ValueInt).FirstOrDefault();

            return maxLenUsername;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return maxLenUsername;
            }
        }
        public int MinLenUsername()
        {
            int minLenUsername = 0;
            try
            {
                Entities dbContext = new Entities();
                minLenUsername = (int)(from s in dbContext.SystemSettings
                                           where s.SettingKey.Equals("UsernameMinLength")
                                           select s.ValueInt).FirstOrDefault();

                return minLenUsername;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return minLenUsername;
            }
        }

        //public bool UpdateWrongPasswordCount(int num, string username){
        //    try
        //    {
        //        User user = dbContext.Users.Where(x => x.Username == username).FirstOrDefault();
        //        if (num == 0)
        //        {
        //            user.WrongPasswordCount = 0;
        //            dbContext.SaveChanges();
        //            return true;
        //        }
        //        else
        //        {
        //            user.WrongPasswordCount += 1;
        //            dbContext.SaveChanges();
        //            return true;
        //        }

        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //}

    }
}
