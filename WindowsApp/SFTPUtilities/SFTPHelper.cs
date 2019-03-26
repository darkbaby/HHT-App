using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Tamir.SharpSsh;
using Tamir.SharpSsh.jsch;
using System.IO;

namespace SFTPUtilities
{
    public class SFTPHelper
    {
        private Session m_session;
        private Channel m_channel;
        private ChannelSftp m_sftp;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="host">SFTP address</param>
        /// <param name="user">username</param>
        /// <param name="pwd">password</param>
        public SFTPHelper(string host, string user, string pwd,string  privateKeyPath)
        {    
            string[] arr = host.Split(':');
            string ip = arr[0];
            int port = 22;
            if (arr.Length > 1) port = Int32.Parse(arr[1]);

            JSch jsch = new JSch();

            jsch.addIdentity(privateKeyPath);
            m_session = jsch.getSession(user, ip, port);

            MyUserInfo ui = new MyUserInfo();
            ui.setPassword(pwd);
            m_session.setUserInfo(ui);
        }

        public SFTPHelper(string host, string user, string pwd)
        {

            string[] arr = host.Split(':');
            string ip = arr[0];
            int port = 22;
            if (arr.Length > 1) port = Int32.Parse(arr[1]);

            JSch jsch = new JSch();
            m_session = jsch.getSession(user, ip, port);

            MyUserInfo ui = new MyUserInfo();
            ui.setPassword(pwd);
            m_session.setUserInfo(ui);
        }

        /// <summary>
        /// get SFTP connection status
        /// </summary>
        public bool Connected { get { return m_session.isConnected(); } }

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
                    m_session.connect();
                    m_channel = m_session.openChannel("sftp");
                    m_channel.connect();
                    m_sftp = (ChannelSftp)m_channel;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
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
                m_channel.disconnect();
                m_session.disconnect();
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
                Tamir.SharpSsh.java.String src = new Tamir.SharpSsh.java.String(localPath);
                Tamir.SharpSsh.java.String dst = new Tamir.SharpSsh.java.String(remotePath);

                string  path = Path.GetDirectoryName(remotePath);
                //การสร้าง directory  ทำได้ทีละชั้นเท่านั้น จึงต้อง  split  เพื่อสร้าง  folder ทีละ level
                path = path.Replace('\\', '/');
                string[] temp = path.Split(new char[] { '/' });
                string pathStr = "";
                foreach (string tmp in temp)
                {
                    try
                    {
                        SftpATTRS attr = m_sftp.stat(pathStr + "/" + tmp);

                    }
                    catch { m_sftp.mkdir(pathStr + "/" + tmp); }
                    pathStr += "/" + tmp;
                    // m_sftp.cd(tmp);
                }
                m_sftp.put(src, dst);
                
                return true;
            }
            catch
            {
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
                Tamir.SharpSsh.java.String src = new Tamir.SharpSsh.java.String(remotePath);
                Tamir.SharpSsh.java.String dst = new Tamir.SharpSsh.java.String(localPath);
                string path = Path.GetDirectoryName(localPath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                
                m_sftp.get(src, dst);
               
                return true;
            }
            catch
            {
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
                m_sftp.rm(remoteFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CreateDir(string dirName)
        {
            try
            {
                m_sftp.mkdir(dirName);
               
                return true;
            }
            catch
            {
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
                foreach (string tmp in temp)
                {
                    try
                    {
                        SftpATTRS attr = m_sftp.stat(pathStr+"/" +tmp);

                    }
                    catch { m_sftp.mkdir(pathStr + "/" + tmp); }
                    pathStr += "/" + tmp;
                   // m_sftp.cd(tmp);
                }
                try
                {
                    m_sftp.ls(destinationPath);
                    Delete(destinationPath);
                }
                catch { }
                

                m_sftp.rename(sourcePath, destinationPath);

                return true;
            }
            catch(Exception ex)
            {
                string str = ex.Message;
                return false;
            }
        }


        /// <summary>
        /// get file list from SFTP
        /// </summary>
        /// <param name="remotePath">remote path</param>
        /// <param name="fileType">file type</param>
        /// <returns></returns>
        public ArrayList GetFileList(string remotePath, string fileType)
        {
            try
            {
                Tamir.SharpSsh.java.util.Vector vvv = m_sftp.ls(remotePath);
                ArrayList objList = new ArrayList();
                foreach (Tamir.SharpSsh.jsch.ChannelSftp.LsEntry qqq in vvv)
                {
                    string sss = qqq.getFilename();
                    if (sss.Length > (fileType.Length + 1) && fileType.ToLower() == sss.Substring(sss.Length - fileType.Length).ToLower())
                    { objList.Add(sss); }
                    else { continue; }
                }

                return objList;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// SFTP login user info
        /// </summary>
        public class MyUserInfo : UserInfo
        {
            String passwd;
            public String getPassword() { return passwd; }
            public void setPassword(String passwd) { this.passwd = passwd; }

            public String getPassphrase() { return null; }
            public bool promptPassphrase(String message) { return true; }

            public bool promptPassword(String message) { return true; }
            public bool promptYesNo(String message) { return true; }
            public void showMessage(String message) { }
        }
    }
}
