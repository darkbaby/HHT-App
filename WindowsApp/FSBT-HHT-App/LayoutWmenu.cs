using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Windows.Forms;
using FSBT.HHT.App.UI;
using FSBT_HHT_BLL;
using FSBT_HHT_Model;
using System.Net;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace FSBT.HHT.App
{
    public partial class LayoutWmenu : FSBT.HHT.App.Layout
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        public string UserName { get; set; }
        private SyncHhtForm syncForm;

        private PermissionScreenBll permissionScreen = new PermissionScreenBll();
        private PermissionComponentBll permissionComponent;
        private Dictionary<string, Form> formPool;
        private const string DownloadMasterScreen = "DownloadMasterForm";
        private const string LocationScreen = "LocationManagementForm";
        private const string PrintBarcodeScreen = "BarcodePrintForm";
        private const string QuantityManagementScreen = "EditQuantityForm";
        private const string SyncHandheldScreen = "SyncHhtForm";
        private const string TextFileScreen = "TextFileForm";
        private const string ReportScreen = "ReportPrintForm";
        private const string BackupRestoreScreen = "BackupRestoreForm";
        private const string UserManagementScreen = "UserManagementForm";
        private const string GroupPermissionScreen = "UserGroupManagementForm";
        private const string SystemSettingsScreen = "SettingsForm";
        Process proc = new Process();

        #region Constructor
        public LayoutWmenu()
        {
            InitializeComponent();
        }

        public LayoutWmenu(string username,Process p)
        {
            UserName = username;
            InitializeComponent();
            SetMenuScreen();
            setStatusStrip();
            formPool = new Dictionary<string, Form>();
            proc = p;
        }
        #endregion

        #region Layout method
        private void SetMenuScreen()
        {
            try
            {
                List<PermissionModel> lstMenu = new List<PermissionModel>();
                lstMenu = permissionScreen.GetPermissionScreenByUser(UserName);

                foreach (PermissionModel lst in lstMenu)
                {
                    int idx = MainMenuStrip.Items.IndexOfKey(lst.ScreenID);
                    if (idx >= 0)
                    {
                        MainMenuStrip.Items[idx].Visible = lst.Visible;
                        MainMenuStrip.Items[idx].Text = lst.MenuName;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        /// <summary>
        /// Set StatusStrip information including the follow data
        /// - Database info
        /// - Logged-in username
        /// </summary>
        /// <param>N/A</param>
        /// <return>N/A</return>
        private void setStatusStrip()
        {
            try
            {
                string computerInfo = getRemoteComputerInfo();
                if (String.IsNullOrWhiteSpace(computerInfo))
                {
                    computerInfo = "";
                }
                else
                {
                    // do nothing   
                }
                string databaseStatus = String.Format(toolStripStatusDatabase.Text, computerInfo);
                toolStripStatusDatabase.Text = databaseStatus;

                if (String.IsNullOrWhiteSpace(UserName))
                {
                    UserName = "";
                }
                string userStatus = String.Format(toolStripStatusUsername.Text, UserName);
                toolStripStatusUsername.Text = userStatus;

                statusStrip1.Refresh();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        /// <summary>
        /// Get database info from connection string
        /// </summary>
        /// <param>N/A</param>
        /// <return>String which's contain database connection info (IP/Computername)</return>
        private string getRemoteComputerInfo()
        {
            string databaseInfo = "";
            try
            {
                // get connection string "Entities", then extract info by ConnectionStringBuilder class
                string connectString = ConfigurationManager.ConnectionStrings["Entities"].ToString();
                EntityConnectionStringBuilder builder;
                SqlConnectionStringBuilder sqlBuilder;


                if (!string.IsNullOrWhiteSpace(connectString))
                {
                    builder = new EntityConnectionStringBuilder(connectString);
                    sqlBuilder = new SqlConnectionStringBuilder(builder.ProviderConnectionString);
                    databaseInfo = sqlBuilder.DataSource;
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                databaseInfo = "";
            }

            return databaseInfo;
        }
        #endregion

        //public void SetAutoUploadGrid()
        //{
        //    HHTSyncBll hhtBll = new HHTSyncBll();
        //    SyncHhtForm otherForm = new SyncHhtForm();
        //    List<AuditStocktakingModel> auditListFormTemp = hhtBll.GetAuditTempList();
        //    try
        //    {
        //        List<AuditStocktakingModel> auditListTempGroupLocation = auditListFormTemp.GroupBy(x => x.LocationCode).Select(g => g.First()).ToList();

        //        int row = 0;
        //        foreach (AuditStocktakingModel data in auditListTempGroupLocation)
        //        {
        //            int countLocation = (from a in auditListFormTemp
        //                                 where a.LocationCode == data.LocationCode
        //                                 select a).Count();
        //            decimal sumQTY = (from a in auditListFormTemp
        //                              where a.LocationCode == data.LocationCode
        //                              select a.Quantity).Sum();

        //            string[] splitFileName = data.FileName.Split('\\');
        //            string fileName = splitFileName[splitFileName.Count() - 1];

        //            otherForm.dataGridViewAutoUp.Rows.Add(data.HHTID, data.HHTName, data.CreateBy, fileName, data.LocationCode, Convert.ToString(countLocation), Convert.ToString((int)sumQTY), "Not Start", "Waiting");



        //            //dataGridViewAutoUp.Rows[row].Cells[7] = new TextAndIconCell();
        //            //dataGridViewAutoUp.Rows[row].Cells[7].Value = "Not Start";
        //            //dataGridViewAutoUp.Rows[row].Cells[8] = new TextAndIconCell();
        //            //dataGridViewAutoUp.Rows[row].Cells[8].Value = "Waiting";
        //            row++;


        //        }
        //        foreach (AuditStocktakingModel auditTemp in auditListFormTemp)
        //        {
        //            string[] splitFileName = auditTemp.FileName.Split('\\');
        //            string fileName = splitFileName[splitFileName.Count() - 1];

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        #region Form Events
        private void LayoutWmenu_Load(object sender, EventArgs e)
        {
            try
            {
                this.MainMenuStrip = new MenuStrip();
                permissionComponent = new PermissionComponentBll(UserName);
                syncForm = new SyncHhtForm(UserName);
                syncForm.CallThread();
                

                //syncForm.SetAutoUploadGrid();

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void LocationManagementForm_Click(object sender, EventArgs e)
        {
            try
            {
                LocationManagementForm form;
                if (!formPool.ContainsKey(LocationScreen))
                {
                    form = new LocationManagementForm(UserName);
                    form.MdiParent = this;
                    permissionComponent.SetPermissionComponentByScreen(form);
                    formPool.Add(LocationScreen, form);
                }
                else
                {
                    form = (LocationManagementForm)formPool[LocationScreen];
                }

                form.Show();
                form.FormClosing += new FormClosingEventHandler(validationForm_FormClosing);
                form.WindowState = FormWindowState.Minimized;
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void BarcodePrintForm_Click(object sender, EventArgs e)
        {
            try
            {
                BarcodePrintForm form;
                if (!formPool.ContainsKey(PrintBarcodeScreen))
                {
                    form = new BarcodePrintForm();
                    form.MdiParent = this;
                    permissionComponent.SetPermissionComponentByScreen(form);
                    formPool.Add(PrintBarcodeScreen, form);
                }
                else
                {
                    form = (BarcodePrintForm)formPool[PrintBarcodeScreen];
                }
                form.Show();
                form.FormClosing += new FormClosingEventHandler(validationForm_FormClosing);
                form.WindowState = FormWindowState.Minimized;
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void EditQuantityForm_Click(object sender, EventArgs e)
        {
            try
            {
                EditQuantityForm form;
                if (!formPool.ContainsKey(QuantityManagementScreen))
                {
                    form = new EditQuantityForm(UserName);
                    form.MdiParent = this;
                    permissionComponent.SetPermissionComponentByScreen(form);
                    formPool.Add(QuantityManagementScreen, form);
                }
                else
                {
                    form = (EditQuantityForm)formPool[QuantityManagementScreen];
                }
                form.Show();
                form.FormClosing += new FormClosingEventHandler(validationForm_FormClosing);
                form.WindowState = FormWindowState.Minimized;
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void SyncHhtForm_Click(object sender, EventArgs e)
        {
            try
            {
               // SyncHhtForm form;
                if (!formPool.ContainsKey(SyncHandheldScreen))
                {
                    //form = new SyncHhtForm(UserName);
                    syncForm.MdiParent = this;
                    permissionComponent.SetPermissionComponentByScreen(syncForm);
                    formPool.Add(SyncHandheldScreen, syncForm);
                }
                else
                {
                    syncForm = (SyncHhtForm)formPool[SyncHandheldScreen];
                }

                syncForm.Show();
                syncForm.FormClosing += new FormClosingEventHandler(validationForm_FormClosing);
                syncForm.WindowState = FormWindowState.Minimized;
                syncForm.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void TextFileForm_Click(object sender, EventArgs e)
        {
            try
            {
                TextFileForm form;
                if (!formPool.ContainsKey(TextFileScreen))
                {
                    form = new TextFileForm();
                    form.MdiParent = this;
                    permissionComponent.SetPermissionComponentByScreen(form);
                    formPool.Add(TextFileScreen, form);
                }
                else
                {
                    form = (TextFileForm)formPool[TextFileScreen];
                }
                form.Show();
                form.FormClosing += new FormClosingEventHandler(validationForm_FormClosing);
                form.WindowState = FormWindowState.Minimized;
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void ReportPrintForm_Click(object sender, EventArgs e)
        {
            try
            {
                ReportPrintForm form;
                if (!formPool.ContainsKey(ReportScreen))
                {
                    form = new ReportPrintForm(UserName,string.Empty);
                    form.MdiParent = this;
                    permissionComponent.SetPermissionComponentByScreen(form);
                    formPool.Add(ReportScreen, form);
                }
                else
                {
                    form = (ReportPrintForm)formPool[ReportScreen];
                }
                form.Show();
                form.FormClosing += new FormClosingEventHandler(validationForm_FormClosing);
                form.WindowState = FormWindowState.Minimized;
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void SettingsForm_Click(object sender, EventArgs e)
        {
            try
            {
                SettingsForm form = new SettingsForm();
                form.MaximizeBox = false;
                form.MinimizeBox = false;
                permissionComponent.SetPermissionComponentByScreen(form);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void UserManagementForm_Click(object sender, EventArgs e)
        {
            try
            {
                UserManagementForm form;
                if (!formPool.ContainsKey(UserManagementScreen))
                {
                    form = new UserManagementForm(UserName);
                    form.MdiParent = this;
                    permissionComponent.SetPermissionComponentByScreen(form);
                    formPool.Add(UserManagementScreen, form);
                }
                else
                {
                    form = (UserManagementForm)formPool[UserManagementScreen];
                }
                form.Show();
                form.FormClosing += new FormClosingEventHandler(validationForm_FormClosing);
                form.WindowState = FormWindowState.Minimized;
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void UserGroupManagementForm_Click(object sender, EventArgs e)
        {
            try
            {
                UserGroupManagementForm form;
                if (!formPool.ContainsKey(GroupPermissionScreen))
                {
                    form = new UserGroupManagementForm(UserName);
                    form.MdiParent = this;
                    permissionComponent.SetPermissionComponentByScreen(form);
                    formPool.Add(GroupPermissionScreen, form);
                }
                else
                {
                    form = (UserGroupManagementForm)formPool[GroupPermissionScreen];
                }
                form.Show();
                form.FormClosing += new FormClosingEventHandler(validationForm_FormClosing);
                form.WindowState = FormWindowState.Minimized;
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void DownloadMasterForm_Click(object sender, EventArgs e)
        {
            try
            {
                DownloadMasterForm form;
                if (!formPool.ContainsKey(DownloadMasterScreen))
                {
                    form = new DownloadMasterForm(UserName);
                    form.MdiParent = this;
                    permissionComponent.SetPermissionComponentByScreen(form);
                    formPool.Add(DownloadMasterScreen, form);
                }
                else
                {
                    form = (DownloadMasterForm)formPool[DownloadMasterScreen];
                }
                form.Show();
                form.FormClosing += new FormClosingEventHandler(validationForm_FormClosing);
                form.WindowState = FormWindowState.Minimized;
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void BackupRestoreForm_Click(object sender, EventArgs e)
        {
            try
            {
                BackupRestoreForm form;
                if (!formPool.ContainsKey(BackupRestoreScreen))
                {
                    form = new BackupRestoreForm(UserName);
                    form.MdiParent = this;
                    permissionComponent.SetPermissionComponentByScreen(form);
                    formPool.Add(BackupRestoreScreen, form);
                }
                else
                {
                    form = (BackupRestoreForm)formPool[BackupRestoreScreen];
                }
                form.Show();
                form.FormClosing += new FormClosingEventHandler(validationForm_FormClosing);
                form.WindowState = FormWindowState.Minimized;
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void ExitForm_Click(object sender, EventArgs e)
        {
            try
            {
                Process[] pname = Process.GetProcessesByName("FSBT-HHT-Batch");
                if (pname.Length != 0)
                {
                    foreach (var process in Process.GetProcessesByName("FSBT-HHT-Batch"))
                        process.Kill();
                }
                //Cho ADD//
                Process[] pname2 = Process.GetProcessesByName("FSBT-HHT-Services");
                if (pname2.Length != 0)
                {
                    foreach (var process in Process.GetProcessesByName("FSBT-HHT-Services"))
                        process.Kill();
                }
                //End Cho ADD//
                syncForm.StopThread();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }

            Application.Exit();
        }

        private void LayoutWmenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Process[] pname = Process.GetProcessesByName("FSBT-HHT-Batch");
                if (pname.Length != 0)
                {
                    foreach (var process in Process.GetProcessesByName("FSBT-HHT-Batch"))
                        process.Kill();
                }
                //Cho ADD//
                Process[] pname2 = Process.GetProcessesByName("FSBT-HHT-Services");
                if (pname2.Length != 0)
                {
                    foreach (var process in Process.GetProcessesByName("FSBT-HHT-Services"))
                        process.Kill();
                }
                //End Cho ADD//
                syncForm.StopThread();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }

            Application.Exit();
        }

        private void validationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //child form closing remove in formPool
                formPool.Remove(sender.GetType().Name.ToString());
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }
                
        #endregion

        private void MainMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem.Text == "Download AS/400")
            {
                DownloadMasterForm.BackColor = SystemColors.ControlDark;
                //DownloadMasterForm.BackColor = SystemColors.ControlLight;
                LocationManagementForm.BackColor = SystemColors.ControlLight;
                BarcodePrintForm.BackColor = SystemColors.ControlLight;
                SyncHhtForm.BackColor = SystemColors.ControlLight;
                EditQuantityForm.BackColor = SystemColors.ControlLight;
                TextFileForm.BackColor = SystemColors.ControlLight;
                ReportPrintForm.BackColor = SystemColors.ControlLight;
                BackupRestoreForm.BackColor = SystemColors.ControlLight;
                UserManagementForm.BackColor = SystemColors.ControlLight;
                UserGroupManagementForm.BackColor = SystemColors.ControlLight;
                SettingsForm.BackColor = SystemColors.ControlLight;
            }
            else if (e.ClickedItem.Text == "Location")
            {
                LocationManagementForm.BackColor = SystemColors.ControlDark;
                DownloadMasterForm.BackColor = SystemColors.ControlLight;
                //LocationManagementForm.BackColor = SystemColors.ControlLight;
                BarcodePrintForm.BackColor = SystemColors.ControlLight;
                SyncHhtForm.BackColor = SystemColors.ControlLight;
                EditQuantityForm.BackColor = SystemColors.ControlLight;
                TextFileForm.BackColor = SystemColors.ControlLight;
                ReportPrintForm.BackColor = SystemColors.ControlLight;
                BackupRestoreForm.BackColor = SystemColors.ControlLight;
                UserManagementForm.BackColor = SystemColors.ControlLight;
                UserGroupManagementForm.BackColor = SystemColors.ControlLight;
                SettingsForm.BackColor = SystemColors.ControlLight;
            }
            else if (e.ClickedItem.Text == "Barcode")
            {
                BarcodePrintForm.BackColor = SystemColors.ControlDark;
                DownloadMasterForm.BackColor = SystemColors.ControlLight;
                LocationManagementForm.BackColor = SystemColors.ControlLight;
                //BarcodePrintForm.BackColor = SystemColors.ControlLight;
                SyncHhtForm.BackColor = SystemColors.ControlLight;
                EditQuantityForm.BackColor = SystemColors.ControlLight;
                TextFileForm.BackColor = SystemColors.ControlLight;
                ReportPrintForm.BackColor = SystemColors.ControlLight;
                BackupRestoreForm.BackColor = SystemColors.ControlLight;
                UserManagementForm.BackColor = SystemColors.ControlLight;
                UserGroupManagementForm.BackColor = SystemColors.ControlLight;
                SettingsForm.BackColor = SystemColors.ControlLight;
            }
            else if (e.ClickedItem.Text == "HHT Sync")
            {
                SyncHhtForm.BackColor = SystemColors.ControlDark;
                DownloadMasterForm.BackColor = SystemColors.ControlLight;
                LocationManagementForm.BackColor = SystemColors.ControlLight;
                BarcodePrintForm.BackColor = SystemColors.ControlLight;
                //SyncHhtForm.BackColor = SystemColors.ControlLight;
                EditQuantityForm.BackColor = SystemColors.ControlLight;
                TextFileForm.BackColor = SystemColors.ControlLight;
                ReportPrintForm.BackColor = SystemColors.ControlLight;
                BackupRestoreForm.BackColor = SystemColors.ControlLight;
                UserManagementForm.BackColor = SystemColors.ControlLight;
                UserGroupManagementForm.BackColor = SystemColors.ControlLight;
                SettingsForm.BackColor = SystemColors.ControlLight;
            }
            else if (e.ClickedItem.Text == "Edit QTY")
            {
                EditQuantityForm.BackColor = SystemColors.ControlDark;
                DownloadMasterForm.BackColor = SystemColors.ControlLight;
                LocationManagementForm.BackColor = SystemColors.ControlLight;
                BarcodePrintForm.BackColor = SystemColors.ControlLight;
                SyncHhtForm.BackColor = SystemColors.ControlLight;
                //EditQuantityForm.BackColor = SystemColors.ControlLight;
                TextFileForm.BackColor = SystemColors.ControlLight;
                ReportPrintForm.BackColor = SystemColors.ControlLight;
                BackupRestoreForm.BackColor = SystemColors.ControlLight;
                UserManagementForm.BackColor = SystemColors.ControlLight;
                UserGroupManagementForm.BackColor = SystemColors.ControlLight;
                SettingsForm.BackColor = SystemColors.ControlLight;
            }
            else if (e.ClickedItem.Text == "Text File")
            {
                TextFileForm.BackColor = SystemColors.ControlDark;
                DownloadMasterForm.BackColor = SystemColors.ControlLight;
                LocationManagementForm.BackColor = SystemColors.ControlLight;
                BarcodePrintForm.BackColor = SystemColors.ControlLight;
                SyncHhtForm.BackColor = SystemColors.ControlLight;
                EditQuantityForm.BackColor = SystemColors.ControlLight;
                //TextFileForm.BackColor = SystemColors.ControlLight;
                ReportPrintForm.BackColor = SystemColors.ControlLight;
                BackupRestoreForm.BackColor = SystemColors.ControlLight;
                UserManagementForm.BackColor = SystemColors.ControlLight;
                UserGroupManagementForm.BackColor = SystemColors.ControlLight;
                SettingsForm.BackColor = SystemColors.ControlLight;
            }
            else if (e.ClickedItem.Text == "Report")
            {
                ReportPrintForm.BackColor = SystemColors.ControlDark;
                DownloadMasterForm.BackColor = SystemColors.ControlLight;
                LocationManagementForm.BackColor = SystemColors.ControlLight;
                BarcodePrintForm.BackColor = SystemColors.ControlLight;
                SyncHhtForm.BackColor = SystemColors.ControlLight;
                EditQuantityForm.BackColor = SystemColors.ControlLight;
                TextFileForm.BackColor = SystemColors.ControlLight;
                //ReportPrintForm.BackColor = SystemColors.ControlLight;
                BackupRestoreForm.BackColor = SystemColors.ControlLight;
                UserManagementForm.BackColor = SystemColors.ControlLight;
                UserGroupManagementForm.BackColor = SystemColors.ControlLight;
                SettingsForm.BackColor = SystemColors.ControlLight;
            }
            else if (e.ClickedItem.Text == "Backup")
            {
                BackupRestoreForm.BackColor = SystemColors.ControlDark;
                DownloadMasterForm.BackColor = SystemColors.ControlLight;
                LocationManagementForm.BackColor = SystemColors.ControlLight;
                BarcodePrintForm.BackColor = SystemColors.ControlLight;
                SyncHhtForm.BackColor = SystemColors.ControlLight;
                EditQuantityForm.BackColor = SystemColors.ControlLight;
                TextFileForm.BackColor = SystemColors.ControlLight;
                ReportPrintForm.BackColor = SystemColors.ControlLight;
                //BackupRestoreForm.BackColor = SystemColors.ControlLight;
                UserManagementForm.BackColor = SystemColors.ControlLight;
                UserGroupManagementForm.BackColor = SystemColors.ControlLight;
                SettingsForm.BackColor = SystemColors.ControlLight;
            }
            else if (e.ClickedItem.Text == "Manage User")
            {
                UserManagementForm.BackColor = SystemColors.ControlDark;
                DownloadMasterForm.BackColor = SystemColors.ControlLight;
                LocationManagementForm.BackColor = SystemColors.ControlLight;
                BarcodePrintForm.BackColor = SystemColors.ControlLight;
                SyncHhtForm.BackColor = SystemColors.ControlLight;
                EditQuantityForm.BackColor = SystemColors.ControlLight;
                TextFileForm.BackColor = SystemColors.ControlLight;
                ReportPrintForm.BackColor = SystemColors.ControlLight;
                BackupRestoreForm.BackColor = SystemColors.ControlLight;
                //UserManagementForm.BackColor = SystemColors.ControlLight;
                UserGroupManagementForm.BackColor = SystemColors.ControlLight;
                SettingsForm.BackColor = SystemColors.ControlLight;
            }
            else if (e.ClickedItem.Text == "Permissions")
            {
                UserGroupManagementForm.BackColor = SystemColors.ControlDark;
                DownloadMasterForm.BackColor = SystemColors.ControlLight;
                LocationManagementForm.BackColor = SystemColors.ControlLight;
                BarcodePrintForm.BackColor = SystemColors.ControlLight;
                SyncHhtForm.BackColor = SystemColors.ControlLight;
                EditQuantityForm.BackColor = SystemColors.ControlLight;
                TextFileForm.BackColor = SystemColors.ControlLight;
                ReportPrintForm.BackColor = SystemColors.ControlLight;
                BackupRestoreForm.BackColor = SystemColors.ControlLight;
                UserManagementForm.BackColor = SystemColors.ControlLight;
                //UserGroupManagementForm.BackColor = SystemColors.ControlLight;
                SettingsForm.BackColor = SystemColors.ControlLight;
            }
        }
    }
}
