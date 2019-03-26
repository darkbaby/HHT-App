using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.Common;
using FSBT_HHT_DAL;
using FSBT_HHT_DAL.DAO;
using System.Reflection;
using System.Threading;

namespace FSBT_HHT_Batch
{
    public class Database
    {
        private LogErrorDAO logBll = new LogErrorDAO();

        public bool ImportRecord(List<AuditStocktakingModel> recordsList, string hhtID, string deviceName, string mode, string fileName)
        {
            Entities dbContext = new Entities();

            SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();

            DataSet dsCountDate = new DataSet();

            DateTime countDate = new DateTime();
            DateTime currentDate = DateTime.Now;

            using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString)) 
            {
                SqlCommand ObjCommand = con.CreateCommand();
                ObjCommand.CommandType = CommandType.Text;
                SqlTransaction trans;
                con.Open();
                trans = con.BeginTransaction();
                ObjCommand.Transaction = trans;

                try
                {
                    string cmdGetCountDate = "SELECT TOP 1 convert(datetime,ValueDate,105) FROM SystemSettings WHERE SettingKey = 'CountDate'";
                    da = new SqlDataAdapter(cmdGetCountDate, conn);
                    da.Fill(dsCountDate);
                    //DateTime countDate = (DateTime)dtCountDate.Rows[0][0];
                    countDate = (DateTime)dsCountDate.Tables[0].Rows[0][0];
                }
                catch (Exception ex)
                {
                   // LogFile.write(Status.ERROR.ToString(), ex.Message);
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                }

                try
                {
                    List<string> duplicateList = new List<string>();
                    foreach (AuditStocktakingModel record in recordsList)
                    {
                        bool stocktakingIDExist = this.IsStocktakingIDExist(record.StockTakingID);
                        if (stocktakingIDExist)
                        {
                            duplicateList.Add(record.StockTakingID);
                        }
                    }

                    // upload all
                    if (mode == "1")
                    {
                        //do nothing
                    }
                    else
                    //upload only different
                    if (mode == "2")
                    {
                        if (duplicateList.Count > 0)
                        {
                            recordsList = recordsList.Where(p => !duplicateList.Any(d => d == p.StockTakingID)).ToList();
                        }
                    }

                    foreach (var auditData in recordsList)
                    {

                        string description = "";

                        if (auditData.Description == "")
                        {
                            description = null;
                        }
                        else
                        {
                            description = auditData.Description;
                        }

                        string SKUCode = "";
                        if (auditData.SKUCode == "")
                        {
                            SKUCode = null;
                        }
                        else
                        {
                            SKUCode = auditData.SKUCode;
                        }

                        string ExBarcode = "";
                        if (auditData.ExBarcode == "")
                        {
                            ExBarcode = null;
                        }
                        else
                        {
                            ExBarcode = auditData.ExBarcode;
                        }

                        string InBarcode = "";
                        if (auditData.InBarcode == "")
                        {
                            InBarcode = null;
                        }
                        else
                        {
                            InBarcode = auditData.InBarcode;
                        }

                        string serialNumber = "";
                        if (auditData.SerialNumber == "")
                        {
                            serialNumber = null;
                        }
                        else
                        {
                            serialNumber = auditData.SerialNumber;
                        }

                        string conversionCounter = "";
                        if (auditData.ConversionCounter == "")
                        {
                            conversionCounter = null;
                        }
                        else
                        {
                            conversionCounter = auditData.ConversionCounter;
                        }

                        string[] fileNames = fileName.Split('\\');
                        fileName = fileNames[fileNames.Length - 1];

                        string cmdAddHHT = "EXEC [dbo].[BATCH_SP_BATCH_ADDHHT] @StocktakingID = '" + auditData.StockTakingID + "'"
                                       + " ,@ScanMode = '" + auditData.ScanMode + "',@LocationCode = '" + auditData.LocationCode + "'"
                                       + " ,@Barcode = '" + auditData.Barcode + "',@Quantity = '" + auditData.Quantity + "'"
                                       + " ,@NewQuantity = NULL ,@UnitCode = '" + auditData.UnitCode + "'"
                                       + " ,@Flag = '" + auditData.Flag + "',@Description = '" + description + "'"
                                       + " ,@SKUCode = '" + SKUCode + "',@ExBarCode = '" + ExBarcode + "'"
                                       + " ,@InBarCode = '" + InBarcode + "',@SKUMode = '" + auditData.SKUMode + "'"
                                       + " ,@HHTName = '" + deviceName + "',@CountDate = '" + countDate + "'"
                                       + " ,@DepartmentCode = '" + auditData.DepartmentCode + "',@CreateDate = '" + auditData.CreateDate + "'"
                                       + " ,@CreateBy = '" + auditData.CreateBy + "',@UpdateDate = '" + auditData.CreateDate + "'"
                                       + " ,@UpdateBy = '" + auditData.CreateBy + "',@FileName ='" + fileName + "',@FlagImport = 0,@HHTID = '" + hhtID + "'"
                                       + " ,@ImportDate = '" + currentDate + "'"
                                       + " ,@SerialNumber = '" + serialNumber + "'"
                                       + " ,@ConversionCounter = '" + conversionCounter + "'";
                        try
                        {
                            ObjCommand.CommandText = cmdAddHHT.ToString();
                            logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, cmdAddHHT.ToString(), DateTime.Now);
                            ObjCommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                        }
                    }

                    ObjCommand.Transaction.Commit();
                    con.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    string error = String.Format("Exception : Error but we are rollbacking : {0}", ex.InnerException.ToString());
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, error, DateTime.Now);
                    //LogFile.write(Status.ERROR.ToString(), "Error but we are rollbacking");
                    ObjCommand.Transaction.Rollback();
                    con.Close();
                    return false;
                }
            }
        }

        public bool IsStocktakingIDExist(string stocktakingID)
        {
            Entities dbContext = new Entities();
            DataTable dtStocktakingID = new DataTable();

            using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                string cmd = "SELECT * FROM tmpHHTStocktaking WHERE StocktakingID = '" + stocktakingID + "'";
                try
                {
                    da = new SqlDataAdapter(cmd, conn);
                    da.Fill(dtStocktakingID);
                }
                catch (Exception ex)
                {
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                }
            }
            
            int countRow = dtStocktakingID.Rows.Count;
            if (countRow > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool ExeScript(string script, SqlConnection conn)
        {
            SqlCommand comm = new SqlCommand(script, conn);
            bool isExe;
            conn.Open();
            try
            {
                comm.ExecuteNonQuery();
                isExe = true;
            }
            catch
            {
                isExe = false;
            }
            finally
            {
                conn.Close();
            }
            return isExe;
        }

        public string GetProductTypeFromMasterSKUBarcode(string barcode)
        {
            Entities dbContext = new Entities();
            string ProductType = "";

            DataSet dsProdType = new DataSet();
            using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                try
                {
                    string cmdGetProdType = "SELECT TOP 1 ProductType FROM MasterSKU WHERE MKCode = '" + barcode + "'";
                    da = new SqlDataAdapter(cmdGetProdType, con);
                    da.Fill(dsProdType);
                    //DateTime countDate = (DateTime)dtCountDate.Rows[0][0];
                    ProductType = (string)dsProdType.Tables[0].Rows[0][0];
                }
                catch (Exception ex)
                {
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    //LogFile.write(Status.ERROR.ToString(), ex.Message);
                }
            }
            return ProductType;
        }

        public string GetProductTypeFromMasterSKUInExBarcode(string InBarcode, string ExBarcode)
        {
            Entities dbContext = new Entities();
            string ProductType = "";

            DataSet dsProdType = new DataSet();

            using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                try
                {
                    string cmdGetProdType = "SELECT TOP 1 ProductType FROM MasterSKU WHERE InBarcode = '" + InBarcode + "' OR ExBarcode = '" + ExBarcode + "'";
                    da = new SqlDataAdapter(cmdGetProdType, con);
                    da.Fill(dsProdType);
                    //DateTime countDate = (DateTime)dtCountDate.Rows[0][0];
                    ProductType = (string)dsProdType.Tables[0].Rows[0][0];
                }
                catch (Exception ex)
                {
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    //LogFile.write(Status.ERROR.ToString(), ex.Message);
                }
            }
            return ProductType;
        }
    }
}
