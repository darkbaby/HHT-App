using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_Model;
using System.Security.Cryptography;
using System.Reflection;

namespace FSBT_HHT_BLL
{
    public class UserManagementBll
    {
        private LogErrorBll logBll = new LogErrorBll(); 
        private UserManagementDAO userDAO = new UserManagementDAO();

        public UserManagementBll()
        {

        }

        public List<UserModel> GetUserData()
        {
            List<UserModel> allUserData = userDAO.GetAllUser();
            return allUserData;
        }

        public string GetNewUserNumber()
        {
            try
            {
                int lastUserNumber = userDAO.GetLastUserNumber();
                if (lastUserNumber < 0)
                {
                    return "error";
                }
                else
                {
                    int newUserNumber = lastUserNumber + 1;
                    string _newUserNumber = newUserNumber.ToString();
                    _newUserNumber = _newUserNumber.PadLeft(2, '0');
                    return _newUserNumber;
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return "";
            }
        }

        public string AddUser(UserModel userData)
        {
            try
            {
                bool checkUser = userDAO.CheckUser(userData.Username);
                if (checkUser == true)
                {
                    return "have user";
                }
                else
                {
                    userData.Password = EncryptPasswordMD5(userData.Password, 1);
                    return userDAO.AddNewUser(userData);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return "";
            }
        }

        public string UpdateUserData(UserModel userData, bool isPasswordChange)
        {
            try
            {
                if (isPasswordChange)
                {
                    userData.Password = EncryptPasswordMD5(userData.Password, 1);
                }
                else
                {
                    //do nothing
                }
                return userDAO.UpdateData(userData);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return "";
            }
        }

        public List<UserModel> SearchUser(string word)
        {
            try
            {
                return userDAO.SearchData(word);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new List<UserModel>();
            }
        }

        public string DeleteUser(string username)
        {
            try
            {
                string isDeleteUserGroupSuccess = userDAO.DeleteUserGroup(username);
                if (isDeleteUserGroupSuccess.Equals("success"))
                {
                    return userDAO.DeleteUser(username);
                }
                else
                {
                    return "error";
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return "";
            }
        }

        public string EncryptPasswordMD5(string password, int layer)
        {
            try
            {
                string resultPassword = password;
                for (int i = 0; i < layer; i++)
                {
                    resultPassword = GetPassWordMD5(resultPassword);
                }
                return resultPassword;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return "";
            }
        }

        private string GetPassWordMD5(string password)
        {
            try
            {
                MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
                byte[] bytePassword;
                StringBuilder passwordEncrypt = new StringBuilder();
                bytePassword = Encoding.UTF8.GetBytes(password);
                bytePassword = md5Provider.ComputeHash(bytePassword);
                foreach (byte b in bytePassword)
                {
                    passwordEncrypt.Append(b.ToString("x2").ToLower());
                }
                return passwordEncrypt.ToString();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return "";
            }
        }
    }
}
