using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_Model;

namespace FSBT_HHT_DAL.DAO
{
    public class DownloadMasterDAO
    {
        private LogErrorDAO logBll = new LogErrorDAO(); 
        
        public DownloadMasterDAO()
        {

        }

        public string AddDataToMasterSKU_Front(DataTable masterData)
        {

            string result = "";
            try
            {
                Entities dbContext = new Entities();
                var masterSKUParam = new SqlParameter();
                masterSKUParam.ParameterName = "@SKUValue";
                masterSKUParam.SqlDbType = SqlDbType.Structured;
                masterSKUParam.TypeName = "dbo.MasterSKU_FrontTable";
                masterSKUParam.SqlValue = masterData;
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SCR01_SP_ADD_MASTERSKU_Front] @SKUValue ", masterSKUParam);

                result = "success";
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = "error";
            }
            return result;
        }

        //public string AddDataToMasterSKU_Back(DataTable masterData)
        //{
        //    string result = "";
        //    try
        //    {
        //        Entities dbContext = new Entities();
        //        var masterSKUParam = new SqlParameter();
        //        masterSKUParam.ParameterName = "@SKUValue";
        //        masterSKUParam.SqlDbType = SqlDbType.Structured;
        //        masterSKUParam.TypeName = "dbo.MasterSKU_FrontTable";
        //        masterSKUParam.SqlValue = masterData;
        //        dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SP_ADD_MASTERSKU_Back] @SKUValue", masterSKUParam);
        //        result = "success";
        //    }
        //    catch (Exception ex)
        //    {
        //        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
        //        result = "error";
        //    }
        //    return result;
        //}

        public string AddDataToMasterSKU_Stock(DataTable masterData)
        {
            string result = "";
            try
            {
                Entities dbContext = new Entities();
                var masterSKUParam = new SqlParameter();
                masterSKUParam.ParameterName = "@SKUValue";
                masterSKUParam.SqlDbType = SqlDbType.Structured;
                masterSKUParam.TypeName = "dbo.MasterSKU_FrontTable";
                masterSKUParam.SqlValue = masterData;
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SCR01_SP_ADD_MASTERSKU_Stock] @SKUValue", masterSKUParam);
                result = "success";
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = "error";
            }
            return result;
        }

        public string AddDataToMasterSKU_FreshFood(DataTable masterData)
        {
            string result = "";
            try
            {
                Entities dbContext = new Entities();
                var masterSKUParam = new SqlParameter();
                masterSKUParam.ParameterName = "@SKUValue";
                masterSKUParam.SqlDbType = SqlDbType.Structured;
                masterSKUParam.TypeName = "dbo.MasterSKU_FreshFoodTable";
                masterSKUParam.SqlValue = masterData;
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SCR01_SP_ADD_MASTERSKU_FreshFood] @SKUValue", masterSKUParam);
                result = "success";
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = "error";
            }
            return result;
        }

        public string AddDataToMasterBarcode(DataTable masterData)
        {
            string result = "";
            try
            {
                Entities dbContext = new Entities();
                var masterBarcodeParam = new SqlParameter();
                masterBarcodeParam.ParameterName = "@BarcodeValue";
                masterBarcodeParam.SqlDbType = SqlDbType.Structured;
                masterBarcodeParam.TypeName = "dbo.MasterBarcodeTable";
                masterBarcodeParam.SqlValue = masterData;
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SCR01_SP_ADD_MASTERBARCODE] @BarcodeValue", masterBarcodeParam);
                result = "success";
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = "error";
            }
            return result;
        }

        public string AddDateToMasterPack(DataTable masterData)
        {
            string result = "";
            try
            {
                Entities dbContext = new Entities();
                var masterBarcodeParam = new SqlParameter();
                masterBarcodeParam.ParameterName = "@PackValue";
                masterBarcodeParam.SqlDbType = SqlDbType.Structured;
                masterBarcodeParam.TypeName = "dbo.MasterPackTable";
                masterBarcodeParam.SqlValue = masterData;
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SCR01_SP_ADD_MASTERPACK] @PackValue", masterBarcodeParam);
                result = "success";
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = "error";
            }
            return result;
        }

        public string AddDataToMasterBrand(DataTable masterData)
        {
            string result = "";
            try
            {
                Entities dbContext = new Entities();
                var masterBarcodeParam = new SqlParameter();
                masterBarcodeParam.ParameterName = "@BrandValue";
                masterBarcodeParam.SqlDbType = SqlDbType.Structured;
                masterBarcodeParam.TypeName = "dbo.MasterBrandTable";
                masterBarcodeParam.SqlValue = masterData;
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SCR01_SP_ADD_MASTERBRAND] @BrandValue", masterBarcodeParam);
                result = "success";
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = "error";
            }
            return result;
        }

        public string DeleteMasterWithFlg(string dataType, int flg)
        {
            string result = "";
            try
            {
                Entities dbContext = new Entities();
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SCR01_SP_DELETE_MASTER_WITH_FLG] " + dataType + "," + flg);
                result = "success";
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = "error";
            }
            return result;
        }

        public string DeleteAllMaster()
        {
            string result = "";
            try
            {
                Entities dbContext = new Entities();
                // Clear Master of SAP + Share Drive
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MastSAP_Barcode");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MastSAP_RegularPrice");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MastSAP_SKU");
                
                // Clear Master
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterSKU");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterBarcode");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterPack");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterBrand");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterStorageLocation");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM Location");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM Section");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterPlant");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterSKUReport");

                // Clear Log
                dbContext.Database.ExecuteSqlCommand("DELETE FROM LogError");

                // Clear Data from HandHeld
                dbContext.Database.ExecuteSqlCommand("DELETE FROM tmpHHTStocktaking");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM HHTStocktaking");

                SystemSettingDAO settingDAO = new SystemSettingDAO();
                settingDAO.UpdateSettingData("StorageLocationType", "string", "SECTION");
                settingDAO.UpdateSettingData("ScanMode", "string", "P");

                result = "success";
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = "error";
            }
            return result;
        }

        public string DeleteMasterSAP()
        {
            string result = "";
            try
            {
                Entities dbContext = new Entities();
                // Clear Master of SAP + Share Drive
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MastSAP_Barcode");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MastSAP_RegularPrice");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MastSAP_SKU");

                // Clear Master
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterSKU");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterBarcode");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterPack");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterBrand");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterStorageLocation");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterSKUReport");

                // Clear Log
                dbContext.Database.ExecuteSqlCommand("DELETE FROM LogError");
                
                SystemSettingDAO settingDAO = new SystemSettingDAO();
                settingDAO.UpdateSettingData("StorageLocationType", "string", "SECTION");
                settingDAO.UpdateSettingData("ScanMode", "string", "P");

                result = "success";
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = "error";
            }
            return result;
        }

        public DataTable GetMasterSummarySubDepartment(int flg)
        {
            DataTable resultTable = new DataTable();
            StringBuilder sqlQuery = new StringBuilder();
            Entities dbContext = new Entities();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    string cmd = "EXEC [dbo].[SCR01_SP_GET_MASTERSUMMARY_SUB_DEPARTMENT] '" + flg + "'";

                    da = new SqlDataAdapter(cmd, conn);
                    da.Fill(resultTable);
                }
                return resultTable;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        public DataTable GetMasterSummaryByBrand(int flg)
        {
            DataTable resultTable = new DataTable();
            StringBuilder sqlQuery = new StringBuilder();
            Entities dbContext = new Entities();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    string cmd = "EXEC [dbo].[SCR01_SP_GET_MASTERSUMMARY_BY_BRAND] '" + flg + "'";

                    da = new SqlDataAdapter(cmd, conn);
                    da.Fill(resultTable);
                }
                return resultTable;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        public DataTable SelectDataForExport(int flg)
        {
            DataTable resultTable = new DataTable();
            StringBuilder sqlQuery = new StringBuilder();
            Entities dbContext = new Entities();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    string cmd = "EXEC [dbo].[GET_AS400_REPORT] '" + flg + "'";

                    da = new SqlDataAdapter(cmd, conn);
                    da.Fill(resultTable);
                }
                return resultTable;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return null;
            }
        }

        public DataTable SelectDataForExportFreshFood()
        {
            DataTable resultTable = new DataTable();
            StringBuilder sqlQuery = new StringBuilder();
            Entities dbContext = new Entities();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    string cmd = "EXEC [dbo].[GET_FRESHFOOD_REPORT]";

                    da = new SqlDataAdapter(cmd, conn);
                    da.Fill(resultTable);
                }
                return resultTable;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return null;
            }
        }

        public DataTable SelectDataForExportExcel(string tableType, int flg)
        {
            DataTable resultTable = new DataTable();
            StringBuilder sqlQuery = new StringBuilder();
            Entities dbContext = new Entities();
            try
            {
                switch (tableType)
                {
                    case "SKU":
                        List<MasterSKU> querySKU = dbContext.MasterSKUs.Where(x => x.StorageLocation.Equals(flg)).ToList();
                        resultTable = ToDataTable<MasterSKU>(querySKU);
                        break;
                    case "Barcode":
                        List<MasterBarcode> queryBarcode = dbContext.MasterBarcodes.ToList();
                        resultTable = ToDataTable<MasterBarcode>(queryBarcode);
                        break;
                }

                int allColumn = resultTable.Columns.Count;
                resultTable.Columns.RemoveAt(allColumn - 1);
                resultTable.Columns.RemoveAt(allColumn - 2);
                resultTable.Columns.RemoveAt(allColumn - 3);

                return resultTable;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return null;
            }
        }

        private DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        private DataTable ToDataTableForExcel<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            try
            {
                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }

                AddRowNameForTable(dataTable, Props);

                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                dataTable = new DataTable(typeof(T).Name);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        private void AddRowNameForTable(DataTable dataTable, PropertyInfo[] Props)
        {
            try
            {
                DataRow dr = dataTable.NewRow();
                int index = 0;
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dr[index] = prop.Name;
                    index++;
                }
                dataTable.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        public DataTable GetMasterDownload(int flg, string dataType)
        {
            DataTable resultTable = new DataTable();
            StringBuilder sqlQuery = new StringBuilder();
            Entities dbContext = new Entities();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    string cmd = "EXEC [dbo].[SCR01_SP_GET_MASTERDOWNLOAD] '" + flg + "','" + dataType + "'";

                    da = new SqlDataAdapter(cmd, conn);
                    da.Fill(resultTable);
                }
                return resultTable;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        public DataTable GetMasterSummaryMaterialGroup(int flg)
        {
            DataTable resultTable = new DataTable();
            StringBuilder sqlQuery = new StringBuilder();
            Entities dbContext = new Entities();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    string cmd = "EXEC [dbo].[SCR01_SP_GET_MASTERSUMMARY_MATERIAL_GROUP] '" + flg + "'";

                    da = new SqlDataAdapter(cmd, conn);
                    da.Fill(resultTable);
                }
                return resultTable;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        public DataTable GetMasterSummaryStorageLocation(int flg)
        {
            DataTable resultTable = new DataTable();
            StringBuilder sqlQuery = new StringBuilder();
            Entities dbContext = new Entities();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    string cmd = "EXEC [dbo].[SCR01_SP_GET_MASTERSUMMARY_STORAGE_LOCATION] '" + flg + "'";

                    da = new SqlDataAdapter(cmd, conn);
                    da.Fill(resultTable);
                }
                return resultTable;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        public DataTable GetSerialAfterDownload()
        {
            DataTable resultTable = new DataTable();
            StringBuilder sqlQuery = new StringBuilder();
            Entities dbContext = new Entities();
            try
            {
                List<TempFileSKUDetail> querySKU = (from skuDetail in dbContext.TempFileSKUDetails
                                                    where skuDetail.SerialNumber != ""
                                                    select skuDetail).ToList();

                resultTable = ToDataTable<TempFileSKUDetail>(querySKU);

                return resultTable;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return null;
            }
        }

        public List<LogError> GetLogAll()
        {
            StringBuilder sqlQuery = new StringBuilder();
            Entities dbContext = new Entities();
            try
            {
                List<LogError> queryLog = (from log in dbContext.LogErrors
                                                    select log).ToList();

                //resultTable = ToDataTable<LogError>(queryLog);

                return queryLog;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return null;
            }
        }
    }
}