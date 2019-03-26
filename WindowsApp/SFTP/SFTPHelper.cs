using FSBT_HHT_DAL.DAO;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SFTP
{
    public class SFTPHelper
    {

        private SftpClient sftp;
        private string password;
        private LogErrorDAO logBll = new LogErrorDAO();
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="host">SFTP address</param>
        /// <param name="user">username</param>
        /// <param name="pwd">password</param>
        public SFTPHelper(string host, string user, string pwd)
        {
            try
            {
                password = pwd;
                KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(user);
                keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);

                string[] arr = host.Split(':');
                string ip = arr[0];
                int port = 22;
                if (arr.Length > 1) port = Int32.Parse(arr[1]);
                ConnectionInfo con = new ConnectionInfo(ip, port, user, keybAuth);
                sftp = new SftpClient(con);
            }
            catch(Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }

        }

        /// <summary>
        /// get SFTP connection status
        /// </summary>
        public bool Connected { get { return sftp.IsConnected; } }

        /// <summary>
        /// connect to SFTP
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            try
            {
                if (!Connected)
                {
                    sftp.Connect();
                }
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        /// <summary>
        /// disconnect the connection to SFTP
        /// </summary>
        public void Disconnect()
        {
            if (Connected)
            {
                sftp.Disconnect();
            }
        }

        /// <summary>
        /// put local file to SFTP
        /// </summary>
        /// <param name="localPath">local path</param>
        /// <param name="remotePath">remote path</param>
        /// <returns></returns>
        public bool Upload(string localPath, string remotePath)
        {
            try
            {
                string path = Path.GetDirectoryName(remotePath);
                //การสร้าง directory  ทำได้ทีละชั้นเท่านั้น จึงต้อง  split  เพื่อสร้าง  folder ทีละ level
                path = path.Replace('\\', '/');
                string[] temp = path.Split(new char[] { '/' });
                string pathStr = "";
                if (!sftp.Exists(path))
                {
                    try
                    {
                        sftp.CreateDirectory(path);
                    }
                    catch
                    {
                        foreach (string tmp in temp)
                        {
                            sftp.CreateDirectory(pathStr + "/" + tmp);
                        }
                    }
                }
                UploadFile(sftp, remotePath, localPath);
                return true;
            }
            catch(Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        /// <summary>
        /// get remote file from SFTP
        /// </summary>
        /// <param name="remotePath">remote path</param>
        /// <param name="localPath">local path</param>
        /// <returns></returns>
        public bool Download(string remotePath, string localPath)
        {
            try
            {
                string path = Path.GetDirectoryName(localPath);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (File.Exists(localPath))
                    File.Delete(localPath);

                DownloadFile(sftp, remotePath, localPath);

                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        /// <summary>
        /// delete file on sftp 
        /// </summary>
        /// <param name="remoteFile">remote path</param>
        /// <returns></returns>
        public bool Delete(string remoteFile)
        {
            try
            {
                sftp.DeleteFile(remoteFile);
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool CreateDir(string dirName)
        {
            try
            {
                sftp.CreateDirectory(dirName);
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool MoveFile(string sourcePath, string destinationPath)
        {
            try
            {
                //   string path = destinationPath.Split(new char[] { '/', '\\' })[0];
                string path = Path.GetDirectoryName(destinationPath);
                path = path.Replace('\\', '/');

                //การสร้าง directory  ทำได้ทีละชั้นเท่านั้น จึงต้อง  split  เพื่อสร้าง  folder ทีละ level
                string[] temp = path.Split(new char[] { '/' });
                string pathStr = "";
                if (!sftp.Exists(path))
                {
                    try
                    {
                        sftp.CreateDirectory(path);
                    }
                    catch
                    {
                        foreach (string tmp in temp)
                        {
                            if (!sftp.Exists(pathStr + "/" + tmp))
                            {
                                sftp.CreateDirectory(pathStr + "/" + tmp);
                            }
                        }
                    }
                }
                sftp.RenameFile(sourcePath, destinationPath);
                Delete(sourcePath);

                return true;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool IsExistPath(string sourcePath) {

            if (sftp.Exists(sourcePath))
            {
                return true ;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// get file list from SFTP
        /// </summary>
        /// <param name="remotePath">remote path</param>
        /// <param name="fileType">file type</param>
        /// <returns></returns>
        public ArrayList GetFileList(string remotePath, string fileType, ref string errorMeassge)
        {
            try
            {
                //Check Exist Path
                if (IsDirectoryExists(remotePath))
                {
                    //List file from Directory
                    var files = sftp.ListDirectory(remotePath);
                    ArrayList objList = new ArrayList();

                    if (files.Count() <= 0)
                    {
                        errorMeassge = "File Not Found";
                    }

                    foreach (var file in files)
                    {
                        string sss = file.Name;// file.FullName;
                        if (sss.Length > (fileType.Length + 1) && fileType.ToLower() == sss.Substring(sss.Length - fileType.Length).ToLower())
                        { objList.Add(sss); }
                        else { continue; }
                    }
                    return objList;
                }
                else
                {
                    errorMeassge = "Not Found Path " + remotePath + " on SFTP Server";
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, errorMeassge, DateTime.Now);
                    return null;
                }
            }
            catch(Exception ex)
            {
                errorMeassge = ex.Message;
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return null;
            }
        }

        private bool IsDirectoryExists(string path)
        {
            bool isDirectoryExist = false;

            try
            {
                sftp.ChangeDirectory(path);
                isDirectoryExist = true;
            }
            catch (SftpPathNotFoundException ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
            return isDirectoryExist;
        }

        private void HandleKeyEvent(object sender, AuthenticationPromptEventArgs e)
        {
            foreach (AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = password;
                }
            }
        }

        private static void DownloadFile(SftpClient client, string remotePath, string localPath)
        {
            using (Stream fileStream = File.OpenWrite(localPath))
            {
                client.DownloadFile(remotePath, fileStream);
            }
        }

        private static void UploadFile(SftpClient client, string remotePath, string localPath)
        {
            using (Stream fileStream = File.OpenRead(localPath))
            {
                client.UploadFile(fileStream, remotePath);
            }
        }
    }
}
