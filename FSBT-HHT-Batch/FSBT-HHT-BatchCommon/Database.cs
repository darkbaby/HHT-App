using FSBT_HHT_BatchCommon.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.Common;

namespace FSBT_HHT_BatchCommon
{
    public class Database
    {
        public static String GetConnectionStringRequest(SettingBean setting)
        {
            StringBuilder connectionStr = new StringBuilder();
            connectionStr.Append("Data Source=" + setting.dataSourceRequest + ";");
            connectionStr.Append("Initial catalog=" + setting.databaseRequest + ";");

            if (setting.isWindownAuthen.Equals("0"))
            {
                connectionStr.Append("User ID=" + setting.userIdRequest + ";");
                connectionStr.Append("Password=" + setting.passwordRequest + ";");
            }
            else
            {
                connectionStr.Append("Integrated Security=True;");
                connectionStr.Append("MultipleActiveResultSets=True;");
            }

            return connectionStr.ToString();
        }

        public bool ImportRecord(List<AuditStocktakingModel> recordsList, string hhtID, string deviceName, string mode, string fileName, SettingBean setting)
        {
            SqlConnection conn = new SqlConnection(GetConnectionStringRequest(setting));
            SqlDataAdapter da = new SqlDataAdapter();

            DataSet dsCountDate = new DataSet();
            CultureInfo defaulCulture = new CultureInfo("en-US");
            DateTime countDate = new DateTime();
            DateTime currentDate = DateTime.Now;

            using (SqlConnection con = new SqlConnection(GetConnectionStringRequest(setting)))
            {
                SqlCommand ObjCommand = con.CreateCommand();
                ObjCommand.CommandType = CommandType.Text;
                SqlTransaction trans;
                con.Open();
                trans = con.BeginTransaction();
                ObjCommand.Transaction = trans;

                try
                {
                    string cmdGetCountDate = "SELECT TOP 1 ValueDate FROM SystemSettings WHERE SettingKey = 'CountDate'";
                    da = new SqlDataAdapter(cmdGetCountDate, conn);
                    da.Fill(dsCountDate);
                    //DateTime countDate = (DateTime)dtCountDate.Rows[0][0];
                    countDate = (DateTime)dsCountDate.Tables[0].Rows[0][0];
                }
                catch (Exception ex)
                {
                    LogFile.write(Status.ERROR.ToString(), ex.Message);
                }

                try
                {
                    List<string> duplicateList = new List<string>();
                    foreach (AuditStocktakingModel record in recordsList)
                    {
                        bool stocktakingIDExist = this.IsStocktakingIDExist(record.StockTakingID, setting);
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

                        int ScanMode = 0;
                        //DataTable dtScanMode = new DataTable();
                        //string cmdGetLocation = "SELECT TOP 1 ScanMode FROM Location WHERE LocationCode = '" + auditData.LocationCode + "'";
                        //try
                        //{
                        //    da = new SqlDataAdapter(cmdGetLocation, conn);
                        //    da.Fill(dtScanMode);
                        //}
                        //catch (Exception ex)
                        //{
                        //    LogFile.write(Status.ERROR.ToString(), ex.Message);
                        //}

                        //if (dtScanMode.Rows.Count > 0)
                        //{
                        //    ScanMode = (int)dtScanMode.Rows[0][0];
                        //}
                        //else
                        //{
                        //    ScanMode = auditData.ScanMode;
                        //}

                        ScanMode = auditData.ScanMode;

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

                        string[] fileNames = fileName.Split('\\');
                        fileName = fileNames[fileNames.Length - 1];


                        string cmdAddHHT = "EXEC [dbo].[SP_BATCH_ADDHHT] @StocktakingID = '" + auditData.StockTakingID + "'"
                                       + " ,@ScanMode = '" + ScanMode + "',@LocationCode = '" + auditData.LocationCode + "'"
                                       + " ,@Barcode = '" + auditData.Barcode + "',@Quantity = '" + auditData.Quantity + "'"
                                       + " ,@NewQuantity = NULL ,@UnitCode = '" + auditData.UnitCode + "'"
                                       + " ,@Flag = '" + auditData.Flag + "',@Description = '" + description + "'"
                                       + " ,@SKUCode = '" + SKUCode + "',@ExBarCode = '" + ExBarcode + "'"
                                       + " ,@InBarCode = '" + InBarcode + "',@SKUMode = '" + auditData.SKUMode + "'"
                                       + " ,@HHTName = '" + deviceName + "',@CountDate = '" + countDate + "'"
                                       + " ,@DepartmentCode = '" + auditData.DepartmentCode + "',@CreateDate = '" + auditData.CreateDate + "'"
                                       + " ,@CreateBy = '" + auditData.CreateBy + "',@UpdateDate = '" + auditData.CreateDate + "'"
                                       + " ,@UpdateBy = '" + auditData.CreateBy + "',@FileName ='" + fileName + "',@FlagImport = 0,@HHTID = '" + hhtID + "'"
                                       + " ,@ImportDate = '" + currentDate + "'";

                        try
                        {

                            ObjCommand.CommandText = cmdAddHHT.ToString();

                            ObjCommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            LogFile.write(Status.ERROR.ToString(), ex.Message);
                        }
                    }

                    ObjCommand.Transaction.Commit();
                    con.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    LogFile.write(Status.ERROR.ToString(), "Error but we are rollbacking");
                    ObjCommand.Transaction.Rollback();
                    con.Close();
                    return false;
                }
            }
        }

        public bool IsStocktakingIDExist(string stocktakingID, SettingBean setting)
        {
            SqlConnection conn = new SqlConnection(GetConnectionStringRequest(setting));
            SqlDataAdapter da = new SqlDataAdapter();

            DataTable dtStocktakingID = new DataTable();
            string cmd = "SELECT * FROM tmpHHTStocktaking WHERE StocktakingID = '" + stocktakingID + "'";
            try
            {
                da = new SqlDataAdapter(cmd, conn);
                da.Fill(dtStocktakingID);
            }
            catch (Exception ex)
            {
                LogFile.write(Status.ERROR.ToString(), ex.Message);
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

        public string GetProductTypeFromMasterSKUBarcode(string barcode, SettingBean setting)
        {
            SqlConnection conn = new SqlConnection(GetConnectionStringRequest(setting));
            SqlDataAdapter da = new SqlDataAdapter();
            string ProductType = "";

            DataSet dsProdType = new DataSet();

            using (SqlConnection con = new SqlConnection(GetConnectionStringRequest(setting)))
            {
                try
                {
                    string cmdGetProdType = "SELECT TOP 1 ProductType FROM MasterSKU WHERE MKCode = '" + barcode + "'";
                    da = new SqlDataAdapter(cmdGetProdType, conn);
                    da.Fill(dsProdType);
                    //DateTime countDate = (DateTime)dtCountDate.Rows[0][0];
                    ProductType = (string)dsProdType.Tables[0].Rows[0][0];
                }
                catch (Exception ex)
                {
                    LogFile.write(Status.ERROR.ToString(), ex.Message);
                }
            }

            return ProductType;
        }

        public string GetProductTypeFromMasterSKUInExBarcode(string InBarcode, string ExBarcode, SettingBean setting)
        {
            SqlConnection conn = new SqlConnection(GetConnectionStringRequest(setting));
            SqlDataAdapter da = new SqlDataAdapter();
            string ProductType = "";

            DataSet dsProdType = new DataSet();

            using (SqlConnection con = new SqlConnection(GetConnectionStringRequest(setting)))
            {
                try
                {
                    string cmdGetProdType = "SELECT TOP 1 ProductType FROM MasterSKU WHERE InBarcode = '" + InBarcode + "' OR ExBarcode = '" + ExBarcode + "'";
                    da = new SqlDataAdapter(cmdGetProdType, conn);
                    da.Fill(dsProdType);
                    //DateTime countDate = (DateTime)dtCountDate.Rows[0][0];
                    ProductType = (string)dsProdType.Tables[0].Rows[0][0];
                }
                catch (Exception ex)
                {
                    LogFile.write(Status.ERROR.ToString(), ex.Message);
                }
            }

            return ProductType;
        }
    }
}
