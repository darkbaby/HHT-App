using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_DAL;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_Model;
namespace FSBT_HHT_BLL
{
    public class BackUpRestoreDataBll
    {
        private BackupRestoreDAO backuprestoreDAO = new BackupRestoreDAO();

        public List<ViewBackupHistoryModel> LoadBackupHistory(string userName)
        {
            List<ViewBackupHistoryModel> lstHist = new List<ViewBackupHistoryModel>();
            lstHist = backuprestoreDAO.LoadBackupHistory(userName);
            return lstHist;
        }

        public string GetBackupFolderPath()
        {
            string path = "";
            path = backuprestoreDAO.GetBackupFolderPath();
            return path;
        }

        public List<ConfigBackupTable> LoadBackupTable()
        {
            List<ConfigBackupTable> lstBckTable = new List<ConfigBackupTable>();
            lstBckTable = backuprestoreDAO.LoadBackupTable();
            return lstBckTable;
        }

        public DataTable GetSchemaTableByTableName(string tableName)
        {
            DataTable dt = new DataTable();
            dt = backuprestoreDAO.GetSchemaTableByTableName(tableName);
            return dt;
        }

        #region Create
        public bool CreateBackupTable(string sourceTableName, string destinationTableName)
        {
            bool success = false;
            success = backuprestoreDAO.CreateBackupTable(sourceTableName, destinationTableName);
            return success;
        }
        
        public int AddBackupHistory(BackupHistoryModel backupModel)
        {
            int backupID = 0;
            backupID = backuprestoreDAO.AddBackupHistory(backupModel);
            return backupID;
        }

        public bool AddBackupHistoryDetail(BackupHistoryDetail bckDetail)
        {
            bool success = false;
            success = backuprestoreDAO.AddBackupHistoryDetail(bckDetail);
            return success;
        }
        #endregion

        #region Delete
        public bool UpdateDeletedFlagBackupHistory(int backupID,string userName)
        {
            bool success = false;
            success = backuprestoreDAO.UpdateDeletedFlagBackupHistory(backupID, userName);
            return success;
        }
        #endregion

        #region Restore
        public List<BackupHistoryDetail> LoadBackupHistoryDetailByBackupID(int backupID)
        {
            List<BackupHistoryDetail> lstBckHistDetail = new List<BackupHistoryDetail>();
            lstBckHistDetail = backuprestoreDAO.LoadBackupHistoryDetailByBackupID(backupID);
            return lstBckHistDetail;
        }

        public bool DeleteDataByTableName(string tableName)
        {
            bool success = false;
            success = backuprestoreDAO.DeleteDataByTableName(tableName);
            return success;
        }

        public bool RestoreDataByTableName(string tableName, DataTable dtImport)
        {
            bool success = false;
            success = backuprestoreDAO.RestoreDataByTableName(tableName, dtImport);
            return success;
        }

        public bool DropTableByTableName(string tableName)
        {
            bool success = false;
            success = backuprestoreDAO.DropTableByTableName(tableName);
            return success;
        }

        public bool RollbackDataFormBackupTable(string tableName,string tableBackupName)
        {
            bool success = false;
            success = backuprestoreDAO.RollbackDataFormBackupTable(tableName, tableBackupName);
            return success;
        }

        #endregion
    }
}
