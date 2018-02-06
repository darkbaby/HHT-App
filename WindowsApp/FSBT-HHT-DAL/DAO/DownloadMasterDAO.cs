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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
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
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SP_ADD_MASTERSKU_Front] @SKUValue ", masterSKUParam);

                result = "success";
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
        //        log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SP_ADD_MASTERSKU_Stock] @SKUValue", masterSKUParam);
                result = "success";
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SP_ADD_MASTERSKU_FreshFood] @SKUValue", masterSKUParam);
                result = "success";
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SP_ADD_MASTERBARCODE] @BarcodeValue", masterBarcodeParam);
                result = "success";
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SP_ADD_MASTERPACK] @PackValue", masterBarcodeParam);
                result = "success";
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SP_ADD_MASTERBRAND] @BrandValue", masterBarcodeParam);
                result = "success";
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SP_DELETE_MASTER_WITH_FLG] " + dataType + "," + flg);
                result = "success";
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterSKU");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterBarcode");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterPack");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM MasterBrand");

                result = "success";
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                result = "error";
            }
            return result;
        }

        public DataTable GetMasterSummary(int flg)
        {
            DataTable resultTable = new DataTable();
            StringBuilder sqlQuery = new StringBuilder();
            Entities dbContext = new Entities();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    string cmd = "EXEC [dbo].[SP_GET_MASTERSUMMARY] '" + flg + "'";

                    da = new SqlDataAdapter(cmd, conn);
                    da.Fill(resultTable);
                }
                return resultTable;

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                    string cmd = "EXEC [dbo].[SP_GET_MASTERSUMMARY_BY_BRAND] '" + flg + "'";

                    da = new SqlDataAdapter(cmd, conn);
                    da.Fill(resultTable);
                }
                return resultTable;

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                        List<MasterSKU> querySKU = dbContext.MasterSKUs.Where(x => x.ScanMode.Equals(flg)).ToList();
                        resultTable = ToDataTable<MasterSKU>(querySKU);
                        break;
                    case "Barcode":
                        List<MasterBarcode> queryBarcode = dbContext.MasterBarcodes.Where(x => x.ScanMode.Equals(flg)).ToList();
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return null;
            }
        }

        private static DataTable ToDataTable<T>(List<T> items)
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

        private static DataTable ToDataTableForExcel<T>(List<T> items)
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                dataTable = new DataTable(typeof(T).Name);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        private static void AddRowNameForTable(DataTable dataTable, PropertyInfo[] Props)
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                    string cmd = "EXEC [dbo].[SP_GET_MASTERDOWNLOAD] '" + flg + "','" + dataType + "'";

                    da = new SqlDataAdapter(cmd, conn);
                    da.Fill(resultTable);
                }
                return resultTable;

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }
    }
}