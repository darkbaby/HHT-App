using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_DAL.DAO;
using System.Security.Cryptography;
using FSBT_HHT_Model;

namespace FSBT_HHT_BLL
{
    public class AuthenticationBll
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        private AuthenticationDAO authenDAO = new AuthenticationDAO();

        public string CheckLogin(string username, string password)
        {
            string result = null;
            try
            {
                password = EncryptPasswordMD5(password, 1);
                UserStatus userStatus = authenDAO.getLoginUser(username, password);

                switch (userStatus)
                {
                    case UserStatus.UNREGISTER_DB:
                        result = "Username not found, please try again.";
                        break;
                    case UserStatus.INCORRECT_PASSWORD:

                        result = "Invalid Password, please try again.";
                        break;
                    case UserStatus.INACTIVE_STATUS:
                        result = "Username is disabled";
                        break;
                    case UserStatus.LOCKED_USER:
                        result = "Username is locked";
                        break;
                    case UserStatus.SUCCESS:
                        result = "Success";
                        break;
                    case UserStatus.ERROR:
                        result = "Can't connect to database. Please check database connection.";
                        break;
                    default:
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return result = null;
            }
        }

        public string EncryptPasswordMD5(string password, int layer)
        {
            string resultPassword = password;
            try
            {

                for (int i = 0; i < layer; i++)
                {
                    resultPassword = GetPassWordMD5(resultPassword);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                resultPassword = string.Empty;
            }
            return resultPassword;
        }

        private string GetPassWordMD5(string password)
        {
            StringBuilder passwordEncrypt = new StringBuilder();
            try
            {
                MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
                byte[] bytePassword;
                
                bytePassword = Encoding.UTF8.GetBytes(password);
                bytePassword = md5Provider.ComputeHash(bytePassword);
                foreach (byte b in bytePassword)
                {
                    passwordEncrypt.Append(b.ToString("x2").ToLower());
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                passwordEncrypt = new StringBuilder();
            }
            return passwordEncrypt.ToString();
        }

        public int GetMaxLenPassword()
        {
            return authenDAO.MaxLenPassword();
        }
        public int GetMinLenPassword()
        {
            return authenDAO.MinLenPassword();
        }
        public int GetMaxLenUsername()
        {
            return authenDAO.MaxLenUsername();
        }
        public int GetMinLenUsername()
        {
            return authenDAO.MinLenUsername();
        }
    }
}
