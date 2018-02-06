using FSBT.HHT.App.Resources;
using FSBT_HHT_BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;

namespace FSBT.HHT.App.UI
{
    public partial class Login : FSBT.HHT.App.Layout
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        AuthenticationBll authen = new AuthenticationBll();
        SystemSettingBll settingBll = new SystemSettingBll();
        private int passwordMaxLen;
        private int passwordMinLen;
        private int usernameMaxLen;
        private int usernameMinLen;
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            DoLogin();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            passwordMaxLen = authen.GetMaxLenPassword();
            passwordMinLen = authen.GetMinLenPassword();
            usernameMaxLen = authen.GetMaxLenUsername();
            usernameMinLen = authen.GetMinLenUsername();

            textUsername.MaxLength = usernameMaxLen;
            textPassword.MaxLength = passwordMaxLen;

            rdNoneRealtime.Checked = true;
        }

        private void DoLogin()
        {
            try
            {
                string username = textUsername.Text;
                string password = textPassword.Text;




                if (username.Length == 0 || password.Length == 0)
                {
                    MessageBox.Show(MessageConstants.PleaseenterUsernamePassword, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    if (username.Length < usernameMinLen || password.Length < passwordMinLen)
                    {
                        MessageBox.Show(MessageConstants.UsernamePasswordmustatleast4letters, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string checkLoginResult = authen.CheckLogin(username, password);
                        if ("Success".Equals(checkLoginResult))
                        {
                            bool isRealtime = false;
                            if (rdNoneRealtime.Checked)
                            {
                                isRealtime = false;
                            }
                            else
                            {
                                isRealtime = true;
                            }

                            settingBll.SaveModeRealtime(isRealtime);

                            //this.Close();
                            //startInfo.FileName = @"D:\CFM_SERVERCOMPANY\192.168.10.6\FSBT_HHT\SourceCode\Batch\FSBT-HHT-Batch\FSBT-HHT-Batch\bin\Debug\FSBT-HHT-Batch.exe";
                            //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            //Process.Start(startInfo);

                            Process proc = new Process();

                            if (!(settingBll.GetRealTimeMode()))
                            {
                                try
                                {
                                    string pathBatch = ConfigurationManager.AppSettings["pathBatchMoveFileHHT"];
                                    ProcessStartInfo startInfo = new ProcessStartInfo();
                                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                    startInfo.FileName = @AppDomain.CurrentDomain.BaseDirectory + pathBatch;

                                    log.Info(String.Format("Call Batch : {0}", @AppDomain.CurrentDomain.BaseDirectory + pathBatch));

                                    proc = Process.Start(startInfo);
                                }
                                catch (Exception ex)
                                {
                                    proc = new Process();
                                    log.Error(String.Format("Exception : {0}", ex.StackTrace));
                                }
                            }
                            else
                            {
                                try
                                {
                                    string pathServices = ConfigurationManager.AppSettings["pathServices"];
                                    ProcessStartInfo processInfo = new ProcessStartInfo();
                                    processInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                    processInfo.FileName = @AppDomain.CurrentDomain.BaseDirectory + pathServices;
                                    processInfo.Verb = "runas";

                                    log.Info(String.Format("Call Services : {0}", @AppDomain.CurrentDomain.BaseDirectory + pathServices));

                                    proc = Process.Start(processInfo);
                                }
                                catch (Exception ex)
                                {
                                    proc = new Process();
                                    log.Error(String.Format("Exception : {0}", ex.StackTrace));
                                }
                            }

                            LayoutWmenu baseScreen = new LayoutWmenu(username, proc);
                            baseScreen.Show();
                            Hide();
                        }
                        else
                        {
                            textPassword.Text = string.Empty;
                            MessageBox.Show(checkLoginResult, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void textUsername_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DoLogin();
            }
        }

        private void textPassword_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DoLogin();
            }
        }


    }
}
