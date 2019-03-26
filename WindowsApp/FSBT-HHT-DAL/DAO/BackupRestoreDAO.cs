using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_Model;
using FSBT_HHT_DAL.Helper;
using System.Reflection;

namespace FSBT_HHT_DAL.DAO
{
    public class BackupRestoreDAO
    {
        private LogErrorDAO logBll = new LogErrorDAO(); 
        private Entities dbContext = new Entities();
        private DbHelper dbHelper = new DbHelper();

        public List<ViewBackupHistoryModel> LoadBackupHistory(string userName)
        {
            List<ViewBackupHistoryModel> lstHist = new List<ViewBackupHistoryModel>();
            try
            {
                lstHist = (from lst in dbContext.BackupHistories
                           where lst.Deleted == false && lst.CreateBy == userName
                           select new ViewBackupHistoryModel
                           {
                               BackupID = lst.BackupID,
                               BackupName = lst.BackupName,
                               BackupDate = DbFunctions.TruncateTime(lst.CreateDate).Value,
                               BackupTime = DbFunctions.CreateTime(lst.CreateDate.Hour,
                                                                  lst.CreateDate.Minute,
                                                                  lst.CreateDate.Second).Value,
                               BackupBy = lst.CreateBy
                           }).ToList<ViewBackupHistoryModel>();
                return lstHist;
            }
            catch (Exception ex)
            {

                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return lstHist;

            }
        }

        public List<ConfigBackupTable> LoadBackupTable()
        {
            List<ConfigBackupTable> lstBckTable = new List<ConfigBackupTable>();
            try
            {
                lstBckTable = (from bckTable in dbContext.ConfigBackupTables
                               orderby bckTable.TableSeq ascending
                               select bckTable
                               ).ToList<ConfigBackupTable>();
                return lstBckTable;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return lstBckTable;
            }
        }

        public string GetBackupFolderPath()
        {
            string path = "";
            try
            {
                path = (from p in dbContext.SystemSettings
                        where p.SettingKey == "BackupFolder"
                        select p.ValueString).SingleOrDefault();
                return path;
            }
            catch (Exception ex)
            {

                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return path;
            }
        }

        public DataTable GetSchemaTableByTableName(string tableName)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = dbHelper.GetDataToDataTableByTableName(dbContext.Database.Connection.ConnectionString, tableName);
                return dt;
            }
            catch (Exception ex)
            {

                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return null;
            }
        }

        #region Delete
        public bool UpdateDeletedFlagBackupHistory(int backupID, string userName)
        {
            try
            {
                BackupHistory backupData = dbContext.BackupHistories.Find(backupID);
                backupData.Deleted = true;
                backupData.DeleteDate = DateTime.Now;
                backupData.DeleteBy = userName;
                try
                {
                    dbContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    return false;
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        #endregion
        #region Create
        public int AddBackupHistory(BackupHistoryModel backupModel)
        {
            int backupID = 0;
            try
            {
                BackupHistory backupData = new BackupHistory();
                backupData.BackupName = backupModel.BackupName;
                backupData.CreateBy = backupModel.BackupBy;
                backupData.CreateDate = backupModel.BackupDate;
                dbContext.BackupHistories.Add(backupData);
                try
                {
                    dbContext.SaveChanges();
                    backupID = backupData.BackupID;
                }
                catch (Exception ex)
                {
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                }
                return backupID;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return backupID;
            }
        }

        public bool CreateBackupTable(string sourceTableName, string destinationTableName)
        {
            try
            {
                DropTableByTableName(destinationTableName);
                string sql = "";
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("SELECT * INTO [dbo].[{0}] FROM [dbo].[{1}]", destinationTableName, sourceTableName);
                sql = sb.ToString();
                dbContext.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool AddBackupHistoryDetail(BackupHistoryDetail bckDetail)
        {
            try
            {
                dbContext.BackupHistoryDetails.Add(bckDetail);
                try
                {
                    dbContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    return false;
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }
        #endregion

        #region Restore
        public List<BackupHistoryDetail> LoadBackupHistoryDetailByBackupID(int backupID)
        {
            List<BackupHistoryDetail> lstBckHistDetail = new List<BackupHistoryDetail>();
            try
            {
                lstBckHistDetail = (from bckDetail in dbContext.BackupHistoryDetails
                                    where bckDetail.BackupID == backupID
                                    select bckDetail).ToList<BackupHistoryDetail>();
                return lstBckHistDetail;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return lstBckHistDetail;
            }
        }

        public bool DropTableByTableName(string tableName)
        {
            try
            {
                string sql = "";
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("IF OBJECT_ID('[dbo].[{0}]', 'U') IS NOT NULL DROP TABLE [dbo].[{0}]", tableName);
                sql = sb.ToString();
                dbContext.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool DeleteDataByTableName(string tableName)
        {
            try
            {
                string sql = "";
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("DELETE FROM [dbo].[{0}]", tableName);
                sql = sb.ToString();
                dbContext.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool RollbackDataFormBackupTable(string tableName, string tableBackupName)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string sql = "";
                sb.AppendFormat("DELETE FROM [dbo].[{0}]; ", tableName);
                //sql += sb.ToString();

                //sb.Clear();
                sb.AppendFormat("INSERT INTO [dbo].[{0}] SELECT * FROM [dbo].[{1}];", tableName, tableBackupName);
                sql += sb.ToString();
                dbContext.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool RestoreDataByTableName(string tableName, DataTable dtImport)
        {
            string connString = "";
            try
            {
                connString = dbContext.Database.Connection.ConnectionString;
                dbHelper.InsertDataTableToDatabase(connString, dtImport, tableName);
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }
        #endregion
    }
}
