using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSBT_HHT_BLL;
using FSBT_HHT_DAL;
using FSBT_HHT_Model;
using FSBT.HHT.App.Resources;
using System.IO;

namespace FSBT.HHT.App.UI
{
    public partial class BackupRestoreForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        public string UserName { get; set; }
        private string FormatPath { get; set; }
        private BackUpRestoreDataBll backuprestoreData = new BackUpRestoreDataBll();
        private List<ConfigBackupTable> lstBackupTable = new List<ConfigBackupTable>();
        private string suffixBackup = @"_bak";

        #region Constructor
        public BackupRestoreForm()
        {
            InitializeComponent();
        }
        
        public BackupRestoreForm(string userName)
        {
            UserName = userName;
            InitializeComponent();
        }
        #endregion

        #region User Define Function

        private void RefreshData()
        {
            List<ViewBackupHistoryModel> lstHist = new List<ViewBackupHistoryModel>();
            lstHist = backuprestoreData.LoadBackupHistory(UserName);
            dgvBackUpHistory.DataSource = lstHist;
        }

        private List<int> GetListSelected()
        {
            List<int> lstIdx = new List<int>();
            foreach (DataGridViewRow row in dgvBackUpHistory.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (chk.Value == chk.TrueValue)
                {
                    lstIdx.Add(row.Index);
                }
            }
            return lstIdx;
        }

        private bool ValidateBackUpName()
        {
            if (txtBackupName.Text == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private string GetBackupFolderPath(string backupID)
        {
            string path = "";
            try
            {
                StringBuilder strBuilder = new StringBuilder(AppDomain.CurrentDomain.BaseDirectory);
                strBuilder.AppendFormat(backuprestoreData.GetBackupFolderPath(), backupID);
                path = strBuilder.ToString();
                return path;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return path;
            }  
        }

        private void FocusOnLastRow()
        {
            if (dgvBackUpHistory.Rows.Count > 0)
            {
                try
                {
                    dgvBackUpHistory.Rows[dgvBackUpHistory.Rows.Count - 1].Selected = true;
                    dgvBackUpHistory.CurrentCell = dgvBackUpHistory.Rows[dgvBackUpHistory.Rows.Count - 1].Cells[1];
                }
                catch (IndexOutOfRangeException ex)
                {
                    log.Error(String.Format("Exception : {0}", ex.StackTrace));
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    log.Error(String.Format("Exception : {0}", ex.StackTrace));
                }
            }
        }
        #endregion

        #region Create backup Function
        private BackupHistoryModel GetInsertBackupHistory()
        {
            BackupHistoryModel backupData = new BackupHistoryModel();
            try
            {

                backupData.BackupName = txtBackupName.Text;
                backupData.BackupBy = UserName;
                backupData.BackupDate = DateTime.Now;
                return backupData;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return backupData;
            }
        }

        private bool ExportTableToTextFile(string folderPath,int backupID)
        {
            try
            {
                foreach (ConfigBackupTable bck in lstBackupTable.OrderBy(o => o.TableSeq).ToList<ConfigBackupTable>())
                {
                    int tableSeq = bck.TableSeq;
                    string tableName = bck.TableName;
                    string fileName = bck.TableName + @".csv";
                    string fullPath = folderPath + @"\" + fileName;
                    DataTable dtExportFile = backuprestoreData.GetSchemaTableByTableName(tableName);
                    if (FilesUtilBll.ExportTextFileFromDataTable(fullPath, dtExportFile, ",", true, true))
                    {
                        BackupHistoryDetail backupDetail = new BackupHistoryDetail();
                        backupDetail.BackupID = backupID;
                        backupDetail.BackupSeq = tableSeq;
                        backupDetail.BackupTable = tableName;
                        backupDetail.FileName = fileName;
                        backupDetail.CreateDate = DateTime.Now;
                        backupDetail.CreateBy = UserName;
                        //Insert History Detail
                        if (!backuprestoreData.AddBackupHistoryDetail(backupDetail))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        }
        #endregion

        #region Restore function
        private bool CreateBackupTable(ref List<string> bakTableName)
        {
            List<ConfigBackupTable> bckTable = lstBackupTable.OrderBy(o => o.TableSeq).ToList<ConfigBackupTable>();
            try
            {
                foreach (ConfigBackupTable bak in bckTable)
                {
                    string sourceTableName = bak.TableName;
                    string destinationTableName = sourceTableName + suffixBackup;
                    if (backuprestoreData.CreateBackupTable(sourceTableName, destinationTableName))
                    {
                        bakTableName.Add(destinationTableName);
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        }

        private bool DeleteTableData(ref List<string> deletedTable)
        {
            List<ConfigBackupTable> bckTable = lstBackupTable.OrderByDescending(o => o.TableSeq).ToList<ConfigBackupTable>();
            try
            {
                foreach (ConfigBackupTable bak in bckTable)
                {
                    if (backuprestoreData.DeleteDataByTableName(bak.TableName))
                    {
                        deletedTable.Add(bak.TableName);
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        }

        private bool RestoreDataByTableName(int backupID,string folderPath)
        {
            List<ConfigBackupTable> bckTable = lstBackupTable.OrderBy(o => o.TableSeq).ToList<ConfigBackupTable>();
            List<BackupHistoryDetail> bckDetail = backuprestoreData.LoadBackupHistoryDetailByBackupID(backupID);
            try
            {
                foreach (ConfigBackupTable bck in bckTable)
                {
                    int seqTable = bck.TableSeq;
                    List<BackupHistoryDetail> bDetail = (from b in bckDetail
                                                         where b.BackupSeq == bck.TableSeq
                                                         select b).ToList<BackupHistoryDetail>();
                    string fullPath = folderPath + @bDetail[0].FileName;
                    DataTable dtFormat = new DataTable();
                    DataTable dtImport = new DataTable();
                    dtFormat = backuprestoreData.GetSchemaTableByTableName(bDetail[0].BackupTable);
                    if (dtFormat != null)
                    {
                        dtImport = FilesUtilBll.GetDataTableFromTextFile(dtFormat, fullPath, ",", true, true);
                    }
                    else
                    {
                        dtImport = FilesUtilBll.GetDataTableFromTextFile(fullPath, ",", true, true);
                    }
                    if (dtImport != null)
                    {
                        if(!backuprestoreData.RestoreDataByTableName(bDetail[0].BackupTable, dtImport))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        }
        
        private bool RollBackDataFromBackupTable(List<string> lstTableName)
        {
            bool success = false;
            List<ConfigBackupTable> bakTable = lstBackupTable.OrderBy(o => o.TableSeq).ToList<ConfigBackupTable>();

            foreach (ConfigBackupTable bak in bakTable)
            {
                if (lstTableName.Contains(bak.TableName))
                {
                    backuprestoreData.RollbackDataFormBackupTable(bak.TableName, bak.TableName + suffixBackup);
                }
            }
            return success;
        }

        private bool DropBackupTable(List<string> lstTableName)
        {
            bool success = false;
            foreach (string tableName in lstTableName)
            {
                backuprestoreData.DropTableByTableName(tableName);
            }
            return success;
        }

        #endregion

        #region Form Event
        private void BackupRestoreForm_Load(object sender, EventArgs e)
        {
            //Initial Data
            lstBackupTable = backuprestoreData.LoadBackupTable();
            FormatPath = backuprestoreData.GetBackupFolderPath();
            //add checkbox dataGrid
            DataGridViewCheckBoxColumn CheckboxColumn = new DataGridViewCheckBoxColumn();
            CheckboxColumn.TrueValue = true;
            CheckboxColumn.FalseValue = false;
            CheckboxColumn.Width = 25;
            dgvBackUpHistory.Columns.Add(CheckboxColumn);
            //add column dataGrid by Model and Set FormatDate = "dd/MM/yyyy"
            List<ViewBackupHistoryModel> lstTemplate = new List<ViewBackupHistoryModel>();
            dgvBackUpHistory.DataSource = lstTemplate;
            dgvBackUpHistory.Columns[2].Width = 200;
            dgvBackUpHistory.Columns[3].DefaultCellStyle.Format = "dd/MM/yyyy";
            RefreshData();
        }

        private void dgvBackUpHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dgvBackUpHistory.Rows[e.RowIndex].Cells[0];
                if (chk.Value == chk.FalseValue || chk.Value == null)
                {
                    chk.Value = chk.TrueValue;
                }
                else
                {
                    chk.Value = chk.FalseValue;
                }
                
            }
        }

        private void btnRestoreData_Click(object sender, EventArgs e)
        {
            bool success = true;
            List<string> bakTableName = new List<string>();
            List<string> deletedTable = new List<string>();
            List<ConfigBackupTable> bckTable = new List<ConfigBackupTable>();
            bckTable = lstBackupTable.OrderBy(o => o.TableSeq).ToList<ConfigBackupTable>();
            List<int> selectedRow = GetListSelected();
            int selected = selectedRow.Count;
            if (selected > 1)
            {
                MessageBox.Show(MessageConstants.CannotrestoreOnly1recordselectedforrestore, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (selected == 0)
            {
                MessageBox.Show(MessageConstants.CannotrestorePleaseselectrecordforrestore, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    string folderPath = "";
                    int backupID = 0;
                    backupID = int.Parse(dgvBackUpHistory.Rows[selectedRow[0]].Cells[1].Value.ToString());
                    folderPath = this.GetBackupFolderPath(backupID.ToString());

                    success = CreateBackupTable(ref bakTableName);
                    if (success)
                    {
                        success = DeleteTableData(ref deletedTable);
                        if (success)
                        {
                            success = RestoreDataByTableName(backupID, folderPath);
                            if (success)
                            {
                                MessageBox.Show(MessageConstants.Restoresuccessful, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                RollBackDataFromBackupTable(deletedTable);
                                MessageBox.Show(MessageConstants.CannotrestoredataPleasetryagain, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            RollBackDataFromBackupTable(deletedTable);
                            MessageBox.Show(MessageConstants.CannotrestoredataPleasetryagain, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.CannotrestoredataPleasetryagain, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    //rollback
                    RollBackDataFromBackupTable(deletedTable);
                    log.Error(String.Format("Exception : {0}", ex.StackTrace));
                    MessageBox.Show(MessageConstants.CannotrestoredataPleasetryagain, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                DropBackupTable(bakTableName);
                RefreshData();
            }
        }

        private void btnCreateBackUp_Click(object sender, EventArgs e)
        {
            int backupID = 0;
            if (ValidateBackUpName())
            {
                BackupHistoryModel backupData = new BackupHistoryModel();
                backupData = GetInsertBackupHistory();
                backupID = backuprestoreData.AddBackupHistory(backupData);
                if (backupID > 0)
                {
                    string folderPath ="";
                    folderPath = this.GetBackupFolderPath(backupID.ToString());
                    //
                    if (folderPath == "")
                    {
                        MessageBox.Show(MessageConstants.Cannotcreatebackupfilepathisinvalid, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //Dele
                    }
                    else
                    {
                        if (Directory.Exists(folderPath))
                        {
                            FilesUtilBll.DeleteDirectory(folderPath);
                        }

                        if (!ExportTableToTextFile(folderPath, backupID))
                        {
                            FilesUtilBll.DeleteDirectory(folderPath);
                            backuprestoreData.UpdateDeletedFlagBackupHistory(backupID, UserName);
                            MessageBox.Show(MessageConstants.Cannotcreatebackupfileerrorexportcsvfile, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show(MessageConstants.Createbackupfilesuccessfull, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                txtBackupName.Text = "";
                RefreshData();
                FocusOnLastRow();
            }
            else
            {
                MessageBox.Show(MessageConstants.BackupNamemustnotbeNullorBlank, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            

        }

        private void btnDeleteBackUp_Click(object sender, EventArgs e)
        {
            List<int> selectedRow = GetListSelected();
            List<string> error = new List<string>();
            string errorMsg = "";
            int selected = selectedRow.Count;
            if (selected > 0)
            {

                try
                {
                    DialogResult dialogresult = MessageBox.Show(MessageConstants.Doyouwanttodeletebackupdata, MessageConstants.TitleClearDataConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogresult == DialogResult.Yes)
                    {
                        //delete backup
                        foreach (int idx in selectedRow)
                        {
                            int backupID = 0;
                            backupID = int.Parse(dgvBackUpHistory.Rows[idx].Cells[1].Value.ToString());
                            if (backupID > 0)
                            {
                                //delete Directory
                                string folderPath = "";
                                folderPath = this.GetBackupFolderPath(backupID.ToString());
                                if (folderPath == "")
                                {
                                    errorMsg += MessageConstants.CannotdeletebackupfilewithbackupID + backupID + "." + Environment.NewLine;
                                }
                                else
                                {
                                    if (FilesUtilBll.DeleteDirectory(folderPath))
                                    {
                                        //update flag
                                        backuprestoreData.UpdateDeletedFlagBackupHistory(backupID, UserName);
                                    }
                                    else
                                    {
                                        errorMsg += MessageConstants.CannotdeletebackupfilewithbackupID + backupID + "." + Environment.NewLine;
                                    }
                                }

                            }
                        }
                        if (errorMsg != "")
                        {
                            MessageBox.Show(errorMsg, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show(MessageConstants.Deletebackupdatasuccessfull, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        RefreshData();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(String.Format("Exception : {0}", ex.StackTrace));
                    MessageBox.Show(MessageConstants.Cannotdeletebackupdata, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(MessageConstants.CannotdeletebackupdataPleaseselectrecordfordelete, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {
            bool success = true;
            List<string> bakTableName = new List<string>();
            List<string> deletedTable = new List<string>();
            try
            {
                DialogResult dialogresult = MessageBox.Show(MessageConstants.Doyouwanttodeletecurrentdata, MessageConstants.TitleClearDataConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogresult == DialogResult.Yes)
                {
                    success = CreateBackupTable(ref bakTableName);
                    if (success)
                    {
                        success = DeleteTableData(ref deletedTable);
                        if (success)
                        {
                            MessageBox.Show(MessageConstants.Cleardatasuccessful, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            RollBackDataFromBackupTable(deletedTable);
                            MessageBox.Show(MessageConstants.Cannotcleardata, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.Cannotcleardata, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                //rollback
                RollBackDataFromBackupTable(deletedTable);
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                MessageBox.Show(MessageConstants.Cannotcleardata, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            DropBackupTable(bakTableName);
            RefreshData();
        }

        #endregion
    }
}
