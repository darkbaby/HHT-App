using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FSBT_HHT_DAL.DAO
{
    public class TempFileImportDAO
    {
        private LogErrorDAO logBll = new LogErrorDAO();       
        public string errorMeassge { get; set; }

        public List<MasterPlant> GetPlant()
        {
            List<MasterPlant> plants;
            try
            {
                Entities dbContext = new Entities();
                plants = dbContext.MasterPlants.ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                plants = new List<MasterPlant>();
            }
            return plants;
        }

        public List<string> Plants()
        {
            List<string> plants;
            try
            {
                Entities dbContext = new Entities();
                plants = dbContext.MasterPlants.Select(c => c.PlantCode).ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                plants = null;
            }
            return plants;
        }

        public bool IsHaveSettingPlants()
        {
            List<string> plants;
            try
            {
                Entities dbContext = new Entities();
                plants = dbContext.MasterPlants.Select(c => c.PlantCode).ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                plants = null;
            }

            if (plants.Count == 0)
            {
                return false ;
            }
            else
            {
                return true;
            }

        }

        public List<string> GetMasterPlant()
        {
            List<string> plants;
            try
            {
                Entities dbContext = new Entities();
                plants = dbContext.MastSAP_RegularPrice.Select(c => c.Plant).Distinct().ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                plants = null;
            }
            return plants;
        }
        public List<string> GetMasterCountsheet(string type)
        {
            List<string> countsheets;
            try
            {
                Entities dbContext = new Entities();
                if (type == "SKU")
                    countsheets = dbContext.MastSAP_SKU.Select(c => c.PIDoc).Distinct().ToList();
                else
                    countsheets = dbContext.MastSAP_Barcode.Select(c => c.PIDoc).Distinct().ToList();

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                countsheets = new List<string>();
            }
            return countsheets;
        }

        public List<TempFileSKUDetail> GetSKUFileDetail()
        {
            List<TempFileSKUDetail> fileDetails;
            try
            {
                Entities dbContext = new Entities();
                fileDetails = dbContext.TempFileSKUDetails.ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileDetails = new List<TempFileSKUDetail>();
            }
            return fileDetails;
        }

        public List<TempFileSKUDetail> GetSKUFileDetail(string fileName)
        {
            List<TempFileSKUDetail> fileDetails;
            try
            {
                Entities dbContext = new Entities();
                fileDetails = dbContext.TempFileSKUDetails.Where(c => c.TempFileName == fileName).ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileDetails = new List<TempFileSKUDetail>();
            }
            return fileDetails;
        }

        public List<TempFileSKUError> GetSKUFileError()
        {
            List<TempFileSKUError> fileErrors;
            try
            {
                Entities dbContext = new Entities();
                fileErrors = dbContext.TempFileSKUErrors.ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileErrors = new List<TempFileSKUError>();
            }

            return fileErrors;
        }

        public List<TempFileSKULineError> GetSKUFileLineError()
        {
            List<TempFileSKULineError> fileErrors;
            try
            {
                Entities dbContext = new Entities();
                fileErrors = dbContext.TempFileSKULineErrors.ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileErrors = new List<TempFileSKULineError>();
            }

            return fileErrors;
        }

        public List<TempFileBarcodeDetail> GetBarcodeFileDetail()
        {
            List<TempFileBarcodeDetail> fileDetails;
            try
            {
                Entities dbContext = new Entities();
                fileDetails = dbContext.TempFileBarcodeDetails.ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileDetails = new List<TempFileBarcodeDetail>();
            }
            return fileDetails;
        }

        public List<TempFileBarcodeDetail> GetBarcodeFileDetail(string filename)
        {
            List<TempFileBarcodeDetail> fileDetails;
            try
            {
                Entities dbContext = new Entities();
                fileDetails = dbContext.TempFileBarcodeDetails.Where(c => c.TempFileName == filename).ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileDetails = new List<TempFileBarcodeDetail>();
            }
            return fileDetails;
        }

        public List<TempFileBarcodeError> GetBarcodeFileError()
        {
            List<TempFileBarcodeError> fileErrors;
            try
            {
                Entities dbContext = new Entities();
                fileErrors = dbContext.TempFileBarcodeErrors.ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileErrors = new List<TempFileBarcodeError>();
            }

            return fileErrors;
        }
        
        public List<TempFileBarcodeLineError> GetBarcodeFileLineError()
        {
            List<TempFileBarcodeLineError> fileErrors;
            try
            {
                Entities dbContext = new Entities();
                fileErrors = dbContext.TempFileBarcodeLineErrors.ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileErrors = new List<TempFileBarcodeLineError>();
            }
            return fileErrors;
        }

        public List<TempFileRegularPriceDetail> GetRegularPriceFileDetail()
        {
            List<TempFileRegularPriceDetail> fileDetails;
            try
            {
                Entities dbContext = new Entities();
                fileDetails = dbContext.TempFileRegularPriceDetails.ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileDetails = new List<TempFileRegularPriceDetail>();
            }

            return fileDetails;
        }

        public List<TempFileRegularPriceDetail> GetRegularPriceFileDetail(string filename)
        {
            List<TempFileRegularPriceDetail> fileDetails;
            try
            {
                Entities dbContext = new Entities();
                fileDetails = dbContext.TempFileRegularPriceDetails.Where(c=>c.TempFileName == filename).ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileDetails = new List<TempFileRegularPriceDetail>();
            }
            return fileDetails;
        }

        public List<TempFileRegularPriceError> GetRegularPriceError()
        {
            List<TempFileRegularPriceError> fileErrors;
            try
            {
                Entities dbContext = new Entities();
                fileErrors = dbContext.TempFileRegularPriceErrors.ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileErrors = new List<TempFileRegularPriceError>();
            }
            return fileErrors;
        }

        public List<TempFileRegularPriceLineError> GetRegularPriceLineError()
        {
            List<TempFileRegularPriceLineError> fileErrors;
            try
            {
                Entities dbContext = new Entities();
                fileErrors = dbContext.TempFileRegularPriceLineErrors.ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileErrors = new List<TempFileRegularPriceLineError>();
            }
            return fileErrors;
        }

        public bool DeleteTempData(string tableName)
        {
            bool result = false;
            try
            {
                using (var dbContext = new Entities())
                {
                    dbContext.Database.ExecuteSqlCommand("Delete from dbo." + tableName  );
                }
                result = true;
            }
            catch (Exception ex)
            {
                errorMeassge = ex.Message;
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return result;
        }

        public bool InsertDataTableToDatabase(DataTable dtImport, string targetTable)
        {
            bool result = false;

            try
            {
                var dbContext = new Entities();
                var connString = dbContext.Database.Connection.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                    {
                        try
                        {
                            bulkCopy.BulkCopyTimeout = 3600;
                            bulkCopy.DestinationTableName = "[dbo].[" + targetTable + "]";
                            bulkCopy.WriteToServer(dtImport);
                            bulkCopy.Close();
                            result = true;
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                            {
                                string pattern = @"\d+";
                                Match match = Regex.Match(ex.Message.ToString(), pattern);
                                var index = Convert.ToInt32(match.Value) - 1;

                                FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                                var sortedColumns = fi.GetValue(bulkCopy);
                                var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                                FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                                var metadata = itemdata.GetValue(items[index]);

                                var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                                var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                                string s =(String.Format("Column: {0} contains data with a length greater than: {1}", column, length));
                                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, s, DateTime.Now);
                            }
                        } 
                        catch(Exception ex)
                        {
                            logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return result;
        }

        public string[] GetColumnName(string tableName)
        {
            try
            {
                using (var dbContext = new Entities())
                {
                    return dbContext.Database.SqlQuery<string>("select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + tableName + "'").ToArray();
                }
            }
            catch(Exception ex)
            {
                errorMeassge = ex.Message;
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return null;
            }
        }

        public DataTable ExecStoredProcedure(string StoredProcedureName, List<string> param)
        {
            Entities dbContext = new Entities();

            string strParam = "";
            foreach (string str in param)
            {
                strParam += "'" + str + "',";
            }
            strParam = strParam.TrimEnd(new char[] { ',' });

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("exec " + StoredProcedureName + " " + strParam, conn);
                SqlDataAdapter dtAdapter = new SqlDataAdapter();

                cmd.CommandTimeout = 900;

                dtAdapter.SelectCommand = cmd;
                dtAdapter.Fill(dt);

                conn.Close();
            }

            return dt;
        }

        //public DataTable ExecStoredProcedure(string StoredProcedureName, List<string> param)
        //{
        //    Entities dbContext = new Entities();

        //    string strParam = "";

        //    foreach (string str in param)
        //    {
        //        strParam += "'" + str + "',";
        //    }

        //    strParam = strParam.TrimEnd(new char[] { ',' });

        //    SqlConnection conn = (System.Data.SqlClient.SqlConnection)dbContext.Database.Connection;
        //    SqlCommand cmd = new SqlCommand("exec " + StoredProcedureName + " " + strParam, conn);
        //    DataTable dt = new DataTable();

        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);

        //    return dt;
        //}
    }  
}