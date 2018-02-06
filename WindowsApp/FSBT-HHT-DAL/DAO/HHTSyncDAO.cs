using FSBT_HHT_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;

namespace FSBT_HHT_DAL.DAO
{
    public class HHTSyncDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        private string DBName = "STOCKTAKING_HHT.sdf";
        private string validateDBName = "COMPUTER_NAME.sdf";
        private string DBPassword = "1234";
        private string ValidateDBPassword = "1234";
        private SystemSettingDAO daoSetting = new SystemSettingDAO();

        public List<DownloadLocationModel> GetLocationList(string locationFrom, string locationTo)
        {
            Entities dbContext = new Entities();
            List<DownloadLocationModel> locationList = new List<DownloadLocationModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(locationTo))
                {
                    locationList = (from l in dbContext.Locations
                                    join s in dbContext.Sections on l.SectionCode equals s.SectionCode
                                    where l.LocationCode.Equals(locationFrom)
                                    select new DownloadLocationModel
                                    {
                                        LocationCode = l.LocationCode,
                                        SectionCode = l.SectionCode,
                                        ScanMode = s.ScanMode,
                                        SectionName = s.SectionName,
                                        BrandCode = s.BrandCode
                                    }).ToList();
                }
                else if (string.IsNullOrWhiteSpace(locationFrom))
                {
                    locationList = (from l in dbContext.Locations
                                    join s in dbContext.Sections on l.SectionCode equals s.SectionCode
                                    where l.LocationCode.Equals(locationTo)
                                    select new DownloadLocationModel
                                    {
                                        LocationCode = l.LocationCode,
                                        SectionCode = l.SectionCode,
                                        ScanMode = s.ScanMode,
                                        SectionName = s.SectionName,
                                        BrandCode = s.BrandCode
                                    }).ToList();
                }
                else
                {
                    locationList = (from l in dbContext.Locations
                                    join s in dbContext.Sections on l.SectionCode equals s.SectionCode
                                    where l.LocationCode.CompareTo(locationFrom) >= 0 && l.LocationCode.CompareTo(locationTo) <= 0
                                    select new DownloadLocationModel
                                    {
                                        LocationCode = l.LocationCode,
                                        SectionCode = l.SectionCode,
                                        ScanMode = s.ScanMode,
                                        SectionName = s.SectionName,
                                        BrandCode = s.BrandCode
                                    }).ToList();
                }

                return locationList;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return locationList;
            }
        }

        public List<PCSKUModel> GetSKUList(string scanMode)
        {
            Entities dbContext = new Entities();
            List<PCSKUModel> skuList = new List<PCSKUModel>();
            int scanmode = Int32.Parse(scanMode);
            try
            {
                skuList = (from m in dbContext.MasterSKUs
                           where m.ScanMode.Equals(scanmode)
                           select new PCSKUModel
                           {
                               Department = m.Department,
                               SKUCode = m.SKUCode,
                               BrandCode = m.BrandCode,
                               Description = m.Description,
                               QTYOnHand = (int)m.QTYOnHand,
                               StockOnHand = (int)m.StockOnHand,
                               Price = (decimal)m.Price,
                               ExBarcode = m.ExBarcode,
                               InBarcode = m.InBarcode,
                               MKCode = m.MKCode
                           }).ToList();
                return skuList;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return skuList;
            }
        }

        public List<UnitModel> GetUnit()
        {
            Entities dbContext = new Entities();
            List<UnitModel> unitList = new List<UnitModel>();
            try
            {
                unitList = (from m in dbContext.MasterUnits
                            select new UnitModel
                            {
                                UnitCode = m.UnitCode,
                                UnitName = m.UnitName,
                                CodeType = m.CodeType
                            }).ToList();
                return unitList;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return unitList;
            }
        }

        public List<ScanModeModel> GetScanMode()
        {
            Entities dbContext = new Entities();
            List<ScanModeModel> scanmodeList = new List<ScanModeModel>();
            try
            {
                scanmodeList = (from m in dbContext.MasterScanModes
                                select new ScanModeModel
                                {
                                    ScanModeID = m.ScanModeID,
                                    ScanModeName = m.ScanModeName
                                }).ToList();
                return scanmodeList;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return scanmodeList;
            }
        }

        public List<MasterBarcodeModel> GetMasterBarcode()
        {
            Entities dbContext = new Entities();
            List<MasterBarcodeModel> masterBarcodeList = new List<MasterBarcodeModel>();
            try
            {
                masterBarcodeList = (from m in dbContext.MasterBarcodes
                                     select new MasterBarcodeModel
                                     {
                                         Status = string.Empty,
                                         ExBarcode = m.ExBarcode,
                                         Barcode = string.Empty,
                                         NoExBarcode = string.Empty,
                                         EAN_UPC = string.Empty,
                                         GroupCode = string.Empty,
                                         ProductCode = string.Empty,
                                         SKUCode = m.SKUCode,
                                         ScanMode = m.ScanMode
                                     }).ToList();
                return masterBarcodeList;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return masterBarcodeList;
            }
        }

        public List<MasterPackModel> GetMasterPack()
        {
            Entities dbContext = new Entities();
            List<MasterPackModel> masterPackList = new List<MasterPackModel>();
            try
            {
                masterPackList = (from m in dbContext.MasterPacks
                                  select new MasterPackModel
                                  {
                                      Status = m.STATUS,
                                      GroupCode = m.GroupCode,
                                      ProductCode = m.ProductCode,
                                      Barcode = m.Barcode,
                                      ProductName = m.ProductName,
                                      UnitQTY = (int)m.UnitQTY,
                                      SKUCode = m.SKUCode,
                                  }).ToList();
                return masterPackList;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return masterPackList;
            }
        }

        public List<AuditStocktakingModel> GetRecordsFromTemp()
        {

            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            List<AuditStocktakingModel> resultList = new List<AuditStocktakingModel>();
            try
            {
                var settingData = daoSetting.GetSettingData();
                var countDate = settingData.CountDate;
                bool isHasData = dbContext.tmpHHTStocktakings.Any(s => s.FlagImport == false && s.CountDate == countDate);

                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_TmpHHTStocktaking", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;

                    cmd.CommandTimeout = 1000;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }

                resultList = convertDtToList(resultTable);
                if (isHasData && (resultList.Count == 0))
                {
                    log.Info("in HHTSyncDAO GetRecordsFromTemp function");
                    log.Info("Countdate from settings system : " + countDate);
                    log.Info("data get from tmpHHTStocktaking by storeprocedure : count is " + resultList.Count);
                }

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                resultList = new List<AuditStocktakingModel>();
            }

            return resultList;
            //List<AuditStocktakingModel> auditTempNotImportList = new List<AuditStocktakingModel>();
            //try
            //{
            //    auditTempNotImportList = (from t in dbContext.tmpHHTStocktakings
            //                              where t.FlagImport == false
            //                              orderby t.ImportDate ascending
            //                              select new AuditStocktakingModel
            //                              {
            //                                  StockTakingID = t.StocktakingID,
            //                                  ScanMode = t.ScanMode,
            //                                  LocationCode = t.LocationCode,
            //                                  Barcode = t.Barcode,
            //                                  Quantity = (decimal)t.Quantity,
            //                                  UnitCode = t.UnitCode,
            //                                  Flag = t.Flag,
            //                                  Description = t.Description,
            //                                  SKUCode = t.SKUCode,
            //                                  ExBarcode = t.ExBarCode,
            //                                  InBarcode = t.InBarCode,
            //                                  SKUMode = t.SKUMode,
            //                                  HHTName = t.HHTName,
            //                                  CountDate = (t.CountDate).ToString(),
            //                                  CreateDate = (t.CreateDate).ToString(),
            //                                  CreateBy = t.CreateBy,
            //                                  DepartmentCode = t.DepartmentCode,
            //                                  FileName = t.FileName,
            //                                  FlagImport = t.FlagImport,
            //                                  HHTID = t.HHTID,
            //                                  ImportDate = (t.ImportDate).ToString()
            //                              }).Take(10000).ToList();

            //    return auditTempNotImportList;
            //}
            //catch (Exception ex)
            //{
            //    log.Error(String.Format("Exception : {0}", ex.StackTrace));
            //    return auditTempNotImportList;
            //}

        }

        public bool UpdateImportFlagInTmpHHTStocktaking(List<AuditStocktakingModel> auditListTempOneLocation)
        {
            Entities dbContext = new Entities();
            try
            {
                foreach (AuditStocktakingModel record in auditListTempOneLocation)
                {
                    tmpHHTStocktaking recordToUpdate = (from tmp in dbContext.tmpHHTStocktakings
                                                        where tmp.StocktakingID == record.StockTakingID
                                                        select tmp).FirstOrDefault();

                    recordToUpdate.FlagImport = true;
                }
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        }

        public bool UpdatePrintFlagInHHTStocktaking(DataTable table)
        {
            Entities dbContext = new Entities();
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    string stocktakingID = row.Field<string>("StocktakingID");
                    HHTStocktaking recordToUpdate = (from tmp in dbContext.HHTStocktakings
                                                     where tmp.StocktakingID == stocktakingID
                                                     select tmp).FirstOrDefault();

                    recordToUpdate.FlagPrint = true;
                }
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        }

        public List<AuditStocktakingModel> convertDtToList(DataTable dt)
        {
            try
            {
                List<AuditStocktakingModel> auditList = dt.AsEnumerable()
                .Select(row => new AuditStocktakingModel
                {
                    // assuming column 0's type is Nullable<long>
                    StockTakingID = row.Field<string>(0),
                    ScanMode = row.Field<int>(1),
                    LocationCode = row.Field<string>(2),
                    Barcode = row.Field<string>(3),
                    Quantity = row.Field<decimal>(4),
                    UnitCode = row.Field<int>(5),
                    Flag = row.Field<string>(6),
                    Description = row.Field<string>(7),
                    SKUCode = row.Field<string>(8),
                    ExBarcode = row.Field<string>(9),
                    InBarcode = row.Field<string>(10),
                    SKUMode = row.Field<bool>(11),
                    HHTName = row.Field<string>(12),
                    CountDate = row.Field<DateTime>(13),
                    CreateDate = row.Field<DateTime>(14),
                    CreateBy = row.Field<string>(15),
                    DepartmentCode = row.Field<string>(16),
                    FileName = row.Field<string>(17),
                    HHTID = row.Field<string>(18),
                    ImportDate = row.Field<DateTime>(19),
                    MKCode = row.Field<string>(20),
                    ProductType = row.Field<string>(21),
                    FlagLoation = row.Field<string>(22)
                    //HHTID = row.Field<long?>(0).GetValueOrDefault(),
                }).ToList();
                return auditList;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new List<AuditStocktakingModel>();
            }

        }
        public InsertAutoResultModel InsertAutoImport(List<AuditStocktakingModel> auditRecord)
        {


            Entities dbContext = new Entities();
            InsertAutoResultModel insertResult = new InsertAutoResultModel();

            try
            {
                string location = auditRecord.Select(a => a.LocationCode).FirstOrDefault();
                string hhtName = auditRecord.Select(a => a.HHTName).FirstOrDefault();
                string hhtID = auditRecord.Select(a => a.HHTID).FirstOrDefault();
                //string hhtName_ID = hhtID + "|" + hhtName;
                //if (hhtName_ID.Length > 20)
                //{
                //    hhtName_ID = hhtName_ID.Substring(0, 20);
                //}
                string stocktaker = auditRecord.Select(a => a.CreateBy).FirstOrDefault();
                //bool hasDuplicateaudit = dbContext.tmpHHTStocktakings.Any(s => s.FlagImport == true && s.LocationCode == location && (s.HHTID != hhtID || s.HHTName != hhtName));
                //List<tmpHHTStocktaking> Duplicateaudit = (from t in dbContext.tmpHHTStocktakings
                //                                            where t.FlagImport == true && t.LocationCode == location && (t.HHTID == hhtID || t.HHTName == hhtName)
                //                                            select t).ToList();

                HHTStocktaking duplicateAudit = (from t in dbContext.HHTStocktakings
                                                 where t.LocationCode == location &&
                                                 t.Flag != "D" &&
                                                 (t.HHTName.Substring(0, 3) != hhtID || t.CreateBy != stocktaker)
                                                 select t).FirstOrDefault();

                if (duplicateAudit == null)
                {
                    foreach (AuditStocktakingModel record in auditRecord)
                    {
                        bool hasRecordDup = dbContext.HHTStocktakings.Any(a => a.StocktakingID == record.StockTakingID);
                        if (hasRecordDup)
                        {
                            HHTStocktaking removeRec = dbContext.HHTStocktakings.Where(x => x.StocktakingID == record.StockTakingID).FirstOrDefault();
                            dbContext.HHTStocktakings.Remove(removeRec);
                            dbContext.SaveChanges();
                        }

                        string HHTName_ID = record.HHTID + "|" + record.HHTName;
                        if (HHTName_ID.Length > 20)
                        {
                            HHTName_ID = HHTName_ID.Substring(0, 20);
                        }

                        HHTStocktaking recordToInsert = new HHTStocktaking();
                        recordToInsert.StocktakingID = record.StockTakingID;
                        recordToInsert.ScanMode = record.ScanMode;
                        recordToInsert.LocationCode = record.LocationCode;
                        recordToInsert.Barcode = record.Barcode;
                        recordToInsert.Quantity = record.Quantity;
                        recordToInsert.NewQuantity = null;
                        recordToInsert.UnitCode = record.UnitCode;
                        recordToInsert.Flag = record.Flag;
                        recordToInsert.Description = record.Description;
                        recordToInsert.SKUCode = record.SKUCode;
                        recordToInsert.ExBarCode = record.ExBarcode;
                        recordToInsert.InBarCode = record.InBarcode;
                        recordToInsert.SKUMode = record.SKUMode;
                        recordToInsert.HHTName = HHTName_ID;
                        recordToInsert.CountDate = record.CountDate;
                        recordToInsert.DepartmentCode = record.DepartmentCode;
                        recordToInsert.FlagPrint = false;
                        recordToInsert.CreateDate = record.CreateDate;
                        recordToInsert.CreateBy = record.CreateBy;
                        recordToInsert.UpdateDate = record.CreateDate;
                        recordToInsert.UpdateBy = record.CreateBy;
                        recordToInsert.MKCode = record.MKCode;
                        recordToInsert.ProductType = record.ProductType;

                        dbContext.HHTStocktakings.Add(recordToInsert);


                        tmpHHTStocktaking recordToUpdate = (from tmp in dbContext.tmpHHTStocktakings
                                                            where tmp.StocktakingID == record.StockTakingID
                                                            select tmp).FirstOrDefault();

                        recordToUpdate.FlagImport = true;
                        dbContext.SaveChanges();
                    }
                    
                    insertResult.result = true;
                    //return true;
                }
                else
                {
                    insertResult.result = false;
                    insertResult.hhtID = (duplicateAudit.HHTName).Split('|')[0];
                    if ((duplicateAudit.HHTName).Split('|').Length == 2)
                    {
                        insertResult.hhtName = (duplicateAudit.HHTName).Split('|')[1];
                    }
                    else
                    {
                        insertResult.hhtName = (duplicateAudit.HHTName).Split('|')[0];
                    }

                    //insertResult.hhtName = duplicateAudit.HHTName;
                    insertResult.stocktaker = duplicateAudit.CreateBy;
                    //return false;
                }
                return insertResult;


        }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                insertResult.result = false;
                insertResult.hhtID = string.Empty;
                insertResult.hhtName = string.Empty;
                insertResult.stocktaker = string.Empty;
                return insertResult;
            }

}

public bool CheckStocktakingIDExist(string stocktakingID)
{
    Entities dbContext = new Entities();
    try
    {
        return dbContext.HHTStocktakings.Any(x => x.StocktakingID == stocktakingID);
    }
    catch (Exception ex)
    {
        log.Error(String.Format("Exception : {0}", ex.StackTrace));
        return false;
    }
}

public bool SaveDownloadLog(List<DownloadLocationModel> downloadLocationList, List<PCSKUModel> skuList, string deviceName, string userName)
{
    Entities dbContext = new Entities();
    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
    {
        conn.Open();
        using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted))
        {
            SqlCommand commandInsertLocation = PrepareInsertLocationLogCommand(conn, transaction);
            SqlCommand commandInsertSKU = PrepareInsertSKULogCommand(conn, transaction);
            CultureInfo defaulCulture = new CultureInfo("en-US");
            DateTime createDate = DateTime.Now;
            string currentDate = DateTime.Now.ToString("yyyyMMdd", defaulCulture);
            //List<Location> downloadLocationList1 = (from l in dbContext.Locations
            //                                        where l.LocationCode.CompareTo("10001") > 0 && l.LocationCode.CompareTo("30038") < 0
            //                                        select l).ToList();
            try
            {
                if (downloadLocationList.Count > 0)
                {
                    string maxCode = "";
                    string maxValue = dbContext.DownloadLocations.Where(x => x.DownloadLID.Contains(currentDate)).Max(x => x.DownloadLID);
                    if (maxValue == null)
                    {
                        maxCode = string.Empty;
                    }
                    else
                    {
                        maxCode = maxValue.Substring(8, 5);
                    }
                    //var maxCode = 0;
                    //var maxValue = dbContext.DownloadLocations.Where(x => x.DownloadLID.Contains(currentDate)).Max(x => x.DownloadLID);
                    //if (maxValue == null)
                    //{
                    //    maxCode = 1;
                    //}
                    //else
                    //{
                    //    maxCode = Convert.ToInt32(maxValue.Substring(8, 5)) + 1;
                    //}

                    int index = 4;
                    //int indexDes = 4;
                    foreach (var location in downloadLocationList)
                    {
                        if (maxCode == string.Empty)
                        {
                            maxCode = "AAAAA";
                            maxValue = currentDate + maxCode;
                        }
                        else
                        {
                            char[] maxCodeArr = maxCode.ToCharArray();
                            maxCodeArr[index] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index]));
                            if (maxCodeArr[index] == 'Z')
                            {
                                maxCodeArr[index] = 'A';
                                //maxCodeArr[index - 1] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index]) + 1);
                                if (maxCodeArr[index - 1] == 'Z')
                                {
                                    maxCodeArr[index - 1] = 'A';
                                    //maxCodeArr[index - 2] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index]) + 1);
                                    if (maxCodeArr[index - 2] == 'Z')
                                    {
                                        maxCodeArr[index - 2] = 'A';
                                        //maxCodeArr[index - 3] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index]) + 1);
                                        if (maxCodeArr[index - 3] == 'Z')
                                        {
                                            maxCodeArr[index - 3] = 'A';
                                            if (maxCodeArr[index - 4] == 'Z')
                                            {
                                                return false;
                                            }
                                            else
                                            {
                                                maxCodeArr[index - 4] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index - 4]) + 1);
                                            }

                                        }
                                        else
                                        {
                                            maxCodeArr[index - 3] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index - 3]) + 1);
                                        }

                                    }
                                    else
                                    {
                                        maxCodeArr[index - 2] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index - 2]) + 1);
                                    }
                                }
                                else
                                {
                                    maxCodeArr[index - 1] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index - 1]) + 1);
                                }
                            }
                            else
                            {
                                maxCodeArr[index] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index]) + 1);
                            }

                            maxCode = new string(maxCodeArr);
                            maxValue = currentDate + maxCode;

                        }

                        //    if (maxCode < 10)
                        //    {
                        //        maxValue = currentDate + "0000" + maxCode;
                        //    }
                        //    else if (maxCode < 100)
                        //    {
                        //        maxValue = currentDate + "000" + maxCode;
                        //    }
                        //    else if (maxCode < 1000)
                        //    {
                        //        maxValue = currentDate + "00" + maxCode;
                        //    }
                        //    else if (maxCode < 10000)
                        //    {
                        //        maxValue = currentDate + "0" + maxCode;
                        //    }
                        //    else
                        //    {
                        //        maxValue = currentDate + maxCode;
                        //    }
                        commandInsertLocation.Parameters["@DownLoadLID"].Value = maxValue;
                        commandInsertLocation.Parameters["@HHTName"].Value = deviceName;
                        commandInsertLocation.Parameters["@LocationCode"].Value = location.LocationCode;
                        commandInsertLocation.Parameters["@SectionCode"].Value = location.SectionCode;
                        commandInsertLocation.Parameters["@ScanMode"].Value = location.ScanMode;
                        //commandInsertLocation.Parameters["@SectionName"].Value = "testsection";
                        //commandInsertLocation.Parameters["@BrandCode"].Value = DBNull.Value;
                        commandInsertLocation.Parameters["@SectionName"].Value = location.SectionName;
                        if (location.BrandCode == null)
                        {
                            commandInsertLocation.Parameters["@BrandCode"].Value = DBNull.Value;
                        }
                        else
                        {
                            commandInsertLocation.Parameters["@BrandCode"].Value = location.BrandCode;
                        }

                        commandInsertLocation.Parameters["@CreateDate"].Value = createDate;
                        commandInsertLocation.Parameters["@CreateBy"].Value = userName;

                        commandInsertLocation.ExecuteNonQuery();

                        //maxCode += 1;
                    }

                }

                if (skuList.Count > 0)
                {
                    string maxCode = "";
                    string maxValue = dbContext.DownloadSKUs.Where(x => x.DownloadSID.Contains(currentDate)).Max(x => x.DownloadSID);
                    if (maxValue == null)
                    {
                        maxCode = string.Empty;
                    }
                    else
                    {
                        maxCode = maxValue.Substring(8, 5);
                    }

                    int index = 4;

                    foreach (var sku in skuList)
                    {
                        //if (maxCode < 10)
                        //{
                        //    maxValue = currentDate + "0000" + maxCode;
                        //}
                        //else if (maxCode < 100)
                        //{
                        //    maxValue = currentDate + "000" + maxCode;
                        //}
                        //else if (maxCode < 1000)
                        //{
                        //    maxValue = currentDate + "00" + maxCode;
                        //}
                        //else if (maxCode < 10000)
                        //{
                        //    maxValue = currentDate + "0" + maxCode;
                        //}
                        //else
                        //{
                        //    maxValue = currentDate + maxCode;
                        //}
                        if (maxCode == string.Empty)
                        {
                            maxCode = "AAAAA";
                            maxValue = currentDate + maxCode;
                        }
                        else
                        {
                            char[] maxCodeArr = maxCode.ToCharArray();
                            maxCodeArr[index] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index]));
                            if (maxCodeArr[index] == 'Z')
                            {
                                maxCodeArr[index] = 'A';
                                //maxCodeArr[index - 1] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index]) + 1);
                                if (maxCodeArr[index - 1] == 'Z')
                                {
                                    maxCodeArr[index - 1] = 'A';
                                    //maxCodeArr[index - 2] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index]) + 1);
                                    if (maxCodeArr[index - 2] == 'Z')
                                    {
                                        maxCodeArr[index - 2] = 'A';
                                        //maxCodeArr[index - 3] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index]) + 1);
                                        if (maxCodeArr[index - 3] == 'Z')
                                        {
                                            maxCodeArr[index - 3] = 'A';
                                            if (maxCodeArr[index - 4] == 'Z')
                                            {
                                                return false;
                                            }
                                            else
                                            {
                                                maxCodeArr[index - 4] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index - 4]) + 1);
                                            }

                                        }
                                        else
                                        {
                                            maxCodeArr[index - 3] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index - 3]) + 1);
                                        }

                                    }
                                    else
                                    {
                                        maxCodeArr[index - 2] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index - 2]) + 1);
                                    }
                                }
                                else
                                {
                                    maxCodeArr[index - 1] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index - 1]) + 1);
                                }
                                //indexDes = indexDes - 1;
                            }
                            else
                            {
                                maxCodeArr[index] = Convert.ToChar(Convert.ToInt32(maxCodeArr[index]) + 1);
                            }

                            maxCode = new string(maxCodeArr);
                            maxValue = currentDate + maxCode;

                        }
                        commandInsertSKU.Parameters["@DownLoadSID"].Value = maxValue;
                        commandInsertSKU.Parameters["@HHTName"].Value = deviceName;
                        commandInsertSKU.Parameters["@Department"].Value = sku.Department;
                        commandInsertSKU.Parameters["@SKUCode"].Value = sku.SKUCode;
                        if (sku.BrandCode == null)
                        {
                            commandInsertSKU.Parameters["@BrandCode"].Value = DBNull.Value;
                        }
                        else
                        {
                            commandInsertSKU.Parameters["@BrandCode"].Value = sku.BrandCode;
                        }

                        commandInsertSKU.Parameters["@BrandName"].Value = DBNull.Value;
                        commandInsertSKU.Parameters["@ExBarcode"].Value = sku.ExBarcode;
                        commandInsertSKU.Parameters["@InBarcode"].Value = sku.InBarcode;
                        commandInsertSKU.Parameters["@Description"].Value = sku.Description;
                        if (sku.Price == null)
                        {
                            commandInsertSKU.Parameters["@Price"].Value = DBNull.Value;
                        }
                        else
                        {
                            commandInsertSKU.Parameters["@Price"].Value = sku.Price;
                        }
                        if (sku.QTYOnHand == null)
                        {
                            commandInsertSKU.Parameters["@QTYOnHand"].Value = DBNull.Value;
                        }
                        else
                        {
                            commandInsertSKU.Parameters["@QTYOnHand"].Value = sku.QTYOnHand;
                        }
                        if (sku.StockOnHand == null)
                        {
                            commandInsertSKU.Parameters["@StockOnHand"].Value = DBNull.Value;
                        }
                        else
                        {
                            commandInsertSKU.Parameters["@StockOnHand"].Value = sku.StockOnHand;
                        }
                        commandInsertSKU.Parameters["@CreateDate"].Value = createDate;
                        commandInsertSKU.Parameters["@CreateBy"].Value = userName;
                        commandInsertSKU.ExecuteNonQuery();

                        maxCode += 1;
                    }
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;

            }

        }

    }
}

public SqlCommand PrepareInsertLocationLogCommand(SqlConnection conn, SqlTransaction transaction)
{
    // create SQL command object
    SqlCommand command = conn.CreateCommand();
    command.CommandType = CommandType.Text;
    command.Transaction = transaction;

    // create SQL command text
    StringBuilder sb = new StringBuilder();
    sb.Append("INSERT INTO DownloadLocation ");
    sb.Append("VALUES (@DownLoadLID, @HHTName, @LocationCode, @SectionCode, @ScanMode ,@SectionName , @BrandCode, @CreateDate, @CreateBy);");
    command.CommandText = sb.ToString();

    // define parameter type
    command.Parameters.Add("@DownLoadLID", SqlDbType.VarChar, 13);
    command.Parameters.Add("@HHTName", SqlDbType.VarChar, 20);
    command.Parameters.Add("@LocationCode", SqlDbType.VarChar, 5);
    command.Parameters.Add("@SectionCode", SqlDbType.VarChar, 5);
    command.Parameters.Add("@ScanMode", SqlDbType.Int);
    command.Parameters.Add("@SectionName", SqlDbType.VarChar, 100);
    command.Parameters.Add("@BrandCode", SqlDbType.VarChar, 5);
    command.Parameters.Add("@CreateDate", SqlDbType.DateTime);
    command.Parameters.Add("@CreateBy", SqlDbType.VarChar, 20);


    command.Prepare();

    return command;
}

public SqlCommand PrepareInsertSKULogCommand(SqlConnection conn, SqlTransaction transaction)
{
    // create SQL command object
    SqlCommand command = conn.CreateCommand();
    command.CommandType = CommandType.Text;
    command.Transaction = transaction;

    // create SQL command text
    StringBuilder sb = new StringBuilder();
    sb.Append("INSERT INTO DownloadSKU ");
    sb.Append("VALUES (@DownLoadSID, @HHTName, @Department, @SKUCode, @BrandCode, @BrandName, @ExBarcode ,@InBarcode , @Description, @Price, @QTYOnHand, @StockOnHand, @CreateDate, @CreateBy);");
    command.CommandText = sb.ToString();

    // define parameter type
    command.Parameters.Add("@DownLoadSID", SqlDbType.VarChar, 13);
    command.Parameters.Add("@HHTName", SqlDbType.VarChar, 50);
    command.Parameters.Add("@Department", SqlDbType.VarChar, 2);
    command.Parameters.Add("@SKUCode", SqlDbType.VarChar, 25);
    command.Parameters.Add("@BrandCode", SqlDbType.VarChar, 5);
    command.Parameters.Add("@BrandName", SqlDbType.VarChar, 50);
    command.Parameters.Add("@ExBarcode", SqlDbType.VarChar, 25);
    command.Parameters.Add("@InBarcode", SqlDbType.VarChar, 25);
    command.Parameters.Add("@Description", SqlDbType.VarChar, 50);
    command.Parameters.Add("@Price", SqlDbType.Money);
    command.Parameters.Add("@QTYOnHand", SqlDbType.Int);
    command.Parameters.Add("@StockOnHand", SqlDbType.Int);
    command.Parameters.Add("@CreateDate", SqlDbType.DateTime);
    command.Parameters.Add("@CreateBy", SqlDbType.VarChar, 20);


    command.Prepare();

    return command;
}

public List<string> GetComputerList()
{
    List<string> computerList = new List<string>();
    // Create a connection to the file datafile.sdf in the program folder
    string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + validateDBName;
    SqlCeConnection connection = new SqlCeConnection("datasource=" + dbfile + ";password=" + ValidateDBPassword);
    connection.Open();
    SqlCeCommand cmd = connection.CreateCommand();
    try
    {
        cmd.CommandText = "SELECT * From Computer";
        SqlCeDataReader myReader = null;
        myReader = cmd.ExecuteReader();

        while (myReader.Read())
        {
            computerList.Add(myReader["Computer_Name"].ToString());
        }

        myReader.Close();
        cmd.Dispose();
        connection.Close();
        return computerList;

    }
    catch (Exception ex)
    {
        cmd.Dispose();
        connection.Close();
        log.Error(String.Format("Exception : {0}", ex.StackTrace));
        Console.WriteLine("Error GetComputerList from table Computer");
        return computerList;
    }

}

public List<AuditStocktakingModel> GetAuditList()
{
    List<AuditStocktakingModel> auditList = new List<AuditStocktakingModel>();
    // Create a connection to the file datafile.sdf in the program folder
    string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
    SqlCeConnection connection = new SqlCeConnection("datasource=" + dbfile + ";password=" + DBPassword);
    connection.Open();

    SqlCeCommand cmd = connection.CreateCommand();
    try
    {
        cmd.CommandText = "SELECT * From tb_t_Stocktaking";
        SqlCeDataReader myReader = null;
        myReader = cmd.ExecuteReader();

        while (myReader.Read())
        {
            //var test = myReader["Description"];
            AuditStocktakingModel auditData = new AuditStocktakingModel();
            auditData.StockTakingID = myReader["StocktakingID"].ToString();
            auditData.ScanMode = Convert.ToInt32(myReader["ScanMode"].ToString());
            auditData.LocationCode = myReader["LocationCode"].ToString();
            auditData.Barcode = myReader["Barcode"].ToString();
            auditData.Quantity = Convert.ToDecimal(myReader["Quantity"].ToString());
            auditData.UnitCode = Convert.ToInt32(myReader["UnitCode"].ToString());
            auditData.Flag = myReader["Flag"].ToString();
            auditData.Description = myReader["Description"].ToString();
            auditData.SKUCode = myReader["SKUCode"].ToString();
            auditData.ExBarcode = myReader["ExBarcode"].ToString();
            auditData.InBarcode = myReader["InBarcode"].ToString();
            //auditData.BrandCode = myReader["BrandCode"].ToString();
            auditData.BrandCode = "";
            auditData.SKUMode = Convert.ToBoolean(myReader["SKUMode"].ToString());
            auditData.CreateDate = Convert.ToDateTime(myReader["CreateDate"].ToString());
            auditData.CreateBy = myReader["CreateBy"].ToString();
            auditData.DepartmentCode = myReader["DepartmentCode"].ToString();
            auditData.MKCode = myReader["MKCode"].ToString();
            //auditData.DepartmentCode = myReader["DepartmentCode"].ToString();

            auditList.Add(auditData);
        }

        myReader.Close();
        cmd.Dispose();
        connection.Close();
        return auditList;

    }
    catch (Exception ex)
    {
        cmd.Dispose();
        connection.Close();
        Console.WriteLine("Error GetComputerList from table Computer");
        log.Error(String.Format("Exception : {0}", ex.StackTrace));
        return auditList;
    }
}

public string GetDeviceName()
{
    string deviceName = "";
    // Create a connection to the file datafile.sdf in the program folder
    string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
    SqlCeConnection connection = new SqlCeConnection("datasource=" + dbfile + ";password=" + DBPassword);
    connection.Open();
    SqlCeCommand cmd = connection.CreateCommand();
    try
    {
        cmd.CommandText = "SELECT Value FROM tb_s_Setting WHERE KeyMap = 'HHTName'";
        SqlCeDataReader myReader = null;
        myReader = cmd.ExecuteReader();

        while (myReader.Read())
        {
            deviceName = myReader["Value"].ToString();
        }

        myReader.Close();
        return deviceName;

    }
    catch (Exception ex)
    {
        Console.WriteLine("Error GetDeviceName from hht");
        log.Error(String.Format("Exception : {0}", ex.StackTrace));
        return deviceName;
    }
    finally
    {
        cmd.Dispose();
        connection.Close();
    }
}
public DataTable GetReportAutoPrint_StocktakingAuditCheckWithUnit(string allLocationCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allSectionCode, string allBrandCode)
{
    Entities dbContext = new Entities();
    DataTable resultTable = new DataTable();
    try
    {
        using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SP_GET_StocktakingAuditCheckWithUnit", conn);
            SqlDataAdapter dtAdapter = new SqlDataAdapter();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
            cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
            cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
            cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
            cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
            cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
            cmd.Parameters.Add("@FlagPrint", SqlDbType.VarChar).Value = "0";
            cmd.Parameters.Add("@ShowStockTakingID", SqlDbType.VarChar).Value = "1";
            cmd.CommandTimeout = 900;

            dtAdapter.SelectCommand = cmd;
            dtAdapter.Fill(resultTable);

            conn.Close();
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
