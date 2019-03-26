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
using System.Reflection;
using System.IO;

namespace FSBT_HHT_DAL.DAO
{
    public class HHTSyncDAO
    {
        private LogErrorDAO logBll = new LogErrorDAO(); 
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
                                    where l.LocationCode.Equals(locationFrom)
                                    select new DownloadLocationModel
                                    {
                                        LocationCode = l.LocationCode,
                                        SectionCode = l.SectionCode,
                                        StorageLocationCode = l.StorageLocationCode
                                    }).ToList();
                }
                else if (string.IsNullOrWhiteSpace(locationFrom))
                {
                    locationList = (from l in dbContext.Locations
                                    where l.LocationCode.Equals(locationTo) 
                                    select new DownloadLocationModel
                                    {
                                        LocationCode = l.LocationCode,
                                        SectionCode = l.SectionCode,
                                        StorageLocationCode = l.StorageLocationCode,
                                    }).ToList();
                }
                else
                {
                    locationList = (from l in dbContext.Locations
                                    where l.LocationCode.CompareTo(locationFrom) >= 0 && l.LocationCode.CompareTo(locationTo) <= 0
                                    select new DownloadLocationModel
                                    {
                                        LocationCode = l.LocationCode,
                                        SectionCode = l.SectionCode,
                                        StorageLocationCode = l.StorageLocationCode,
                                    }).ToList();
                }

                return locationList;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                           join e in dbContext.MasterSerialNumbers // on m.SKUCode equals e.SKUCode
                           on new { X1 = m.SKUCode, X2 = m.ExBarcode } equals new { X1 = e.SKUCode, X2 = e.Barcode }
                           where m.StorageLocation.Equals(scanmode)
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
                           }).Distinct().ToList();
                return skuList;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return skuList;
            }
        }

        public List<PCSKUModel> GetSKUList()
        {
            Entities dbContext = new Entities();
            List<PCSKUModel> skuList = new List<PCSKUModel>();
            try
            {
                skuList = (from m in dbContext.MasterSKUs
                           join e in dbContext.MasterSerialNumbers //on m.SKUCode equals e.SKUCode
                           on new { X1 = m.SKUCode, X2 = m.ExBarcode } equals new { X1 = e.SKUCode, X2 = e.Barcode }
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
                           }).Distinct().ToList();

                return skuList;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return skuList;
            }
        }

        public List<PCSKUModel> GetSKUListAll()
        {
            Entities dbContext = new Entities();
            List<PCSKUModel> skuList = new List<PCSKUModel>();
            try
            {
                skuList = (from m in dbContext.MasterSKUs
                           join e in dbContext.MasterSerialNumbers //on m.SKUCode equals e.SKUCode
                           on new { X1 = m.SKUCode, X2 = m.ExBarcode } equals new { X1 = e.SKUCode, X2 = e.Barcode }
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
                           }).Distinct().ToList();
                return skuList;
            }

            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return unitList;
            }
        }

        public List<ScanModeModel> GetScanMode()
        {
            Entities dbContext = new Entities();
            List<ScanModeModel> scanmodeList = new List<ScanModeModel>();
            try
            {
                //scanmodeList = (from m in dbContext.MasterScanModes
                //                select new ScanModeModel
                //                {
                //                    ScanModeID = m.ScanModeID,
                //                    ScanModeName = m.ScanModeName
                //                }).ToList();
                return scanmodeList;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                                         SKUCode = m.SKUCode
                                     }).ToList();
                return masterBarcodeList;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return masterBarcodeList;
            }
        }

        public List<MasterSerialNumberModel> GetMasterSerialNumber()
        {
            Entities dbContext = new Entities();
            List<MasterSerialNumberModel> masterSerialNumberList = new List<MasterSerialNumberModel>();
            try
            {
                masterSerialNumberList = (from m in dbContext.MasterSerialNumbers
                                         select new MasterSerialNumberModel
                                         {
                                            SKUCode = m.SKUCode,
                                            Barcode = m.Barcode,
                                            SerialNumber = m.SerialNumber,
                                            StorageLocation = m.StorageLocation,
                                            StorageLocationDesc = m.StorageLocationDesc
                                         }).ToList();
                return masterSerialNumberList;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return masterSerialNumberList;
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                    SqlCommand cmd = new SqlCommand("SCR04_SP_GET_TmpHHTStocktaking", conn);
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
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "in HHTSyncDAO GetRecordsFromTemp function", DateTime.Now);
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Countdate from settings system : " + countDate, DateTime.Now);
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "data get from tmpHHTStocktaking by storeprocedure : count is " + resultList.Count, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
            //    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                    FlagLocation = row.Field<string>(22),
                    SerialNumber = row.Field<string>(23),
                    ConversionCounter = row.Field<string>(24),
                    StorageLocation = row.Field<string>(25),
                    StorageLocationDesc = row.Field<string>(26),
                    Plant = row.Field<string>(27),
                    PIDoc = row.Field<string>(28),
                    MCHLevel1 = row.Field<string>(29),
                    MCHLevel2 = row.Field<string>(30),
                    MCHLevel3= row.Field<string>(31),
                    MaterialGroup= row.Field<string>(32),
                    ComputerName = row.Field<string>(33)
                }).ToList();
                return auditList;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                        recordToInsert.SerialNumber = record.SerialNumber;
                        recordToInsert.ConversionCounter = record.ConversionCounter;
                        recordToInsert.StorageLocation = record.StorageLocation;
                        recordToInsert.StorageLocationDesc = record.StorageLocationDesc;
                        recordToInsert.Plant = record.Plant;
                        recordToInsert.PIDoc =record.PIDoc;
                        recordToInsert.MCHLevel1 =record.MCHLevel1;
                        recordToInsert.MCHLevel2 =record.MCHLevel2;
                        recordToInsert.MCHLevel3 =record.MCHLevel3;
                        recordToInsert.MaterialGroup = record.MaterialGroup;
                        recordToInsert.ComputerName = record.ComputerName;

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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                insertResult.result = false;
                insertResult.hhtID = string.Empty;
                insertResult.hhtName = string.Empty;
                insertResult.stocktaker = string.Empty;
                return insertResult;
            }
        }

        public bool InsertHHTStocking()
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SCR04_SP_INSERT_HHTStocktaking", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                    //CultureInfo defaulCulture = new CultureInfo("en-US");

                    CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                    newCulture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
                    newCulture.DateTimeFormat.LongDatePattern = "yyyy/MM/dd HH:mm:ss";
                    newCulture.DateTimeFormat.DateSeparator = "/";

                    DateTime createDate = DateTime.Now;
                    string currentDate = DateTime.Now.ToString("yyyyMMdd", newCulture);
                    try
                    {
                        #region insertDownloadLocation
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
                    int index = 4;
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
                                if (maxCodeArr[index - 1] == 'Z')
                                {
                                    maxCodeArr[index - 1] = 'A';
                                    if (maxCodeArr[index - 2] == 'Z')
                                    {
                                        maxCodeArr[index - 2] = 'A';
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

                        //DownloadLocation newRecord = new DownloadLocation
                        //{
                        //    DownloadLID     = maxValue,
                        //    HHTName         = deviceName,
                        //    LocationCode    = location.LocationCode,
                        //    StorageCode     = location.SectionCode,
                        //    StorageName     = location.SectionName,
                        //    BrandCode       = location.BrandCode,
                        //    CreateDate      = createDate,
                        //    CreateBy        = userName
                        //};

                        //dbContext.DownloadLocations.Add(newRecord);
                        //dbContext.SaveChanges();

                        commandInsertLocation.Parameters["@DownLoadLID"].Value = maxValue;
                        commandInsertLocation.Parameters["@HHTName"].Value = deviceName;
                        commandInsertLocation.Parameters["@LocationCode"].Value = location.LocationCode;
                        //commmandInsertLocation.Parameters["@ScanMode"].Value = "";
                        //commandInsertLocation.Parameters["@SectionName"].Value = "testsection";
                        //commandInsertLocation.Parameters["@BrandCode"].Value = DBNull.Value;
                        commandInsertLocation.Parameters["@SectionCode"].Value = location.SectionCode;
                        commandInsertLocation.Parameters["@SectionName"].Value = location.SectionName;
                        if (string.IsNullOrEmpty(location.BrandCode))
                        {
                            commandInsertLocation.Parameters["@BrandCode"].Value = DBNull.Value;
                        }
                        else
                        {
                            commandInsertLocation.Parameters["@BrandCode"].Value = location.BrandCode;
                        }

                        commandInsertLocation.Parameters["@CreateDate"].Value = DateTime.Now;
                        commandInsertLocation.Parameters["@CreateBy"].Value = userName;

                        commandInsertLocation.ExecuteNonQuery();
                        //maxCode += 1;
                    }
                #endregion
                        }
        
                        #region insertSku
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
                        if ( string.IsNullOrEmpty(sku.Price.ToString()))
                        {
                            commandInsertSKU.Parameters["@Price"].Value = DBNull.Value;
                        }
                        else
                        {
                            commandInsertSKU.Parameters["@Price"].Value = sku.Price;
                        }
                        if (string.IsNullOrEmpty(sku.QTYOnHand.ToString()))
                        {
                            commandInsertSKU.Parameters["@QTYOnHand"].Value = DBNull.Value;
                        }
                        else
                        {
                            commandInsertSKU.Parameters["@QTYOnHand"].Value = sku.QTYOnHand;
                        }
                        if (string.IsNullOrEmpty(sku.StockOnHand.ToString()))
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
                #endregion
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
            sb.Append("VALUES (@DownLoadLID, @HHTName, @LocationCode, @SectionCode ,@SectionName , @BrandCode, @CreateDate, @CreateBy);");
            command.CommandText = sb.ToString();
        
            // define parameter type
            command.Parameters.Add("@DownLoadLID", SqlDbType.VarChar, 13);
            command.Parameters.Add("@HHTName", SqlDbType.VarChar, 20);
            command.Parameters.Add("@LocationCode", SqlDbType.VarChar, 5);
            command.Parameters.Add("@SectionCode", SqlDbType.VarChar, 5);
            //command.Parameters.Add("@ScanMode", SqlDbType.Int);
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

        public List<string> GetComputerList(string dbfile)
        {
            List<string> computerList = new List<string>();
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + validateDBName;

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
                    computerList.Add(myReader["Computer_Name"].ToString().ToUpper());
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                Console.WriteLine("Error GetComputerList from table Computer");
                return computerList;
            }
        
        }

        public List<AuditStocktakingModel> GetAuditList(string dbfile)
        {
            List<AuditStocktakingModel> auditList = new List<AuditStocktakingModel>();
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;

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
                    auditData.BrandCode = myReader["BrandCode"].ToString();
                    auditData.BrandCode = "";
                    auditData.SKUMode = Convert.ToBoolean(myReader["SKUMode"].ToString());
                    auditData.CreateDate = Convert.ToDateTime(myReader["CreateDate"].ToString());
                    auditData.CreateBy = myReader["CreateBy"].ToString();
                    auditData.DepartmentCode = myReader["DepartmentCode"].ToString();
                    //auditData.MKCode = myReader["MKCode"].ToString();
                    auditData.SerialNumber = myReader["SerialNumber"].ToString();
                    auditData.ConversionCounter = myReader["ConversionCounter"].ToString();
                    auditData.DepartmentCode = myReader["DepartmentCode"].ToString();
        
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return auditList;
            }
        }

        public string GetDeviceName(string dbfile)
        {
            string deviceName = "";
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                    SqlCommand cmd = new SqlCommand("RPT02-05_SP_GET_SumStockOnHandWarehouse", conn);
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        public DataTable GetReportAutoPrint_StocktakingAuditCheckWithUnit(string allLocationCode, DateTime countDate)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT08_SP_GET_StocktakingAuditCheckWithUnit", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = "";
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        //public DataTable GetReportAutoPrint_StocktakingAuditCheckWithUnit(string allLocationCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allSectionCode, string allBrandCode)
        //{
        //    Entities dbContext = new Entities();
        //    DataTable resultTable = new DataTable();
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
        //        {
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand("RPT01-04_SP_GET_SumStockOnHand", conn);
        //            SqlDataAdapter dtAdapter = new SqlDataAdapter();

        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
        //            cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
        //            cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
        //            cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
        //            cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
        //            cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
        //            cmd.Parameters.Add("@FlagPrint", SqlDbType.VarChar).Value = "0";
        //            cmd.Parameters.Add("@ShowStockTakingID", SqlDbType.VarChar).Value = "1";
        //            cmd.CommandTimeout = 900;

        //            dtAdapter.SelectCommand = cmd;
        //            dtAdapter.Fill(resultTable);

        //            conn.Close();
        //        }
        //        return resultTable;
        //    }
        //    catch (Exception ex)
        //    {
        //        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
        //        return new DataTable();
        //    }
        //}

        public bool ConnectSdfDeleteDataStockTaking(List<AuditStocktakingModel>[] auditListByLocationArr, string dbfile)
        {
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;

            using(SqlCeConnection connection = new SqlCeConnection("datasource=" + dbfile + ";password=" + DBPassword))
            {
                connection.Open();
                SqlCeTransaction tx = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                SqlCeCommand cmd = PrepareDeleteStocktakingCommand(connection, tx);
                try
                {
                    foreach (List<AuditStocktakingModel> auditListByLocation in auditListByLocationArr)
                    {
                        if (auditListByLocation.Count > 0)
                        {
                            foreach (var auditData in auditListByLocation)
                            {
                                cmd.Parameters["@StocktakingID"].Value = auditData.StockTakingID;

                                cmd.ExecuteNonQuery();
                            }
                        }

                    }
                    tx.Commit();
                    connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    connection.Close();
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    return false;
                }
            }
        }

        public SqlCeCommand PrepareDeleteStocktakingCommand(SqlCeConnection conn, SqlCeTransaction transaction)
        {
            // create SQL command object
            SqlCeCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;

            // create SQL command text
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM tb_t_Stocktaking ");
            sb.Append("WHERE StocktakingID = @StocktakingID;");
            command.CommandText = sb.ToString();

            // define parameter type
            command.Parameters.Add("@StocktakingID", SqlDbType.NVarChar, 15);

            command.Prepare();

            return command;
        }

        public bool ConnectSdfDeleteData(string type, string dbfile)
        {
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
            //string dbfile = "D:\\TestDB1.sdf";

           using( SqlCeConnection connection = new SqlCeConnection("datasource=" + dbfile + ";password=" + DBPassword))
           {
                connection.Open();
                SqlCeCommand cmd = connection.CreateCommand();
                try
                {
                    if (type == "Location")
                    {
                        cmd.CommandText = "DELETE FROM tb_m_Location";
                    }
                    if (type == "SKU")
                    {
                        cmd.CommandText = "DELETE tb_m_SKU";
                    }
                    if (type == "Unit")
                    {
                        cmd.CommandText = "DELETE tb_m_Unit";
                    }
                    if (type == "MasterBarcode")
                    {
                        cmd.CommandText = "DELETE tb_m_Barcode";
                    }
                    if (type == "MasterPack")
                    {
                        cmd.CommandText = "DELETE tb_m_Pack";
                    }
                    if (type == "MasterSerialNumber")
                    {
                        cmd.CommandText = "DELETE tb_m_SerialNumber";
                    }

                    cmd.ExecuteReader();

                    cmd.Dispose();
                    connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    cmd.Dispose();
                    connection.Close();
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    return false;
                }
           }
        }

        public bool ConnectSdfInsertLocationData(List<DownloadLocationModel> locationList, string dbfile, string username)
        {
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
           // string dbfile = new System.IO.FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) + "\\" + DBName;
            //SqlCeConnection connection = new SqlCeConnection("datasource=" + dbfile  + ";LCID=1030;password=1234");
            //string dbfile = "D:\\STOCKTAKING_HHT.sdf";

            using (SqlCeConnection connection = new SqlCeConnection("datasource=" + dbfile + ";password=" + DBPassword))
            {
                connection.Open();
                SqlCeTransaction tx = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                //SqlCeCommand cmd = PrepareInsertLoCommand(connection, tx);

                try
                {
                    // create SQL command object
                    SqlCeCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.Transaction = tx;

                    DateTime createDate = DateTime.Now;
                    foreach (DownloadLocationModel location in locationList)
                    {
                        // create SQL command text
                        StringBuilder sb = new StringBuilder();
                        sb.Append("INSERT INTO tb_m_Location ");
                        sb.Append("VALUES ('" + location.LocationCode + "', '" + location.SectionCode + "', '" + "" + "'");
                        sb.Append(",'" + "" + "', GETDATE(), '" + username + "', '" + location.StorageLocationCode + "');");

                        command.CommandText = sb.ToString();

                        command.ExecuteNonQuery();
                    }

                    tx.Commit();
                    connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    connection.Close();
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    return false;
                }
            }
        }

        public SqlCeCommand PrepareInsertLoCommand(SqlCeConnection conn, SqlCeTransaction transaction)
        {
            // create SQL command object
            SqlCeCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;

            // create SQL command text
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO tb_m_Location ");
            sb.Append("VALUES (@LocationCode, @SectionCode, @SectionName, @BrandCode, @CreateDate, @CreateBy, @StorageLocationCode);");
            command.CommandText = sb.ToString();

            // define parameter type
            command.Parameters.Add("@LocationCode", SqlDbType.NVarChar, 5);
            command.Parameters.Add("@SectionCode", SqlDbType.NVarChar, 5);
            command.Parameters.Add("@SectionName", SqlDbType.NVarChar, 100);
            command.Parameters.Add("@BrandCode", SqlDbType.NVarChar, 5);
            command.Parameters.Add("@CreateDate", SqlDbType.DateTime);
            command.Parameters.Add("@CreateBy", SqlDbType.NVarChar, 20);
            command.Parameters.Add("@StorageLocationCode", SqlDbType.NVarChar, 100);

            command.Prepare();

            return command;
        }

        public bool ConnectSdfInsertMasterBarcodeData(List<MasterBarcodeModel> masterBarcodeList, string dbfile, string username)
        {
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
            //string dbfile = new System.IO.FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) + "\\" + DBName;

            using (SqlCeConnection connection = new SqlCeConnection("datasource=" + dbfile + ";password=" + DBPassword))
            {
                connection.Open();
                SqlCeTransaction tx = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                SqlCeCommand cmd = PrepareInsertMasterBarcodeCommand(connection, tx);
                try
                {
                    DateTime createDate = DateTime.Now;
                    foreach (MasterBarcodeModel data in masterBarcodeList)
                    {
                        cmd.Parameters["@Status"].Value = data.Status;
                        cmd.Parameters["@ExBarcode"].Value = data.ExBarcode;
                        if (data.Barcode == null)
                        {
                            cmd.Parameters["@Barcode"].Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters["@Barcode"].Value = data.Barcode;
                        }
                        if (data.NoExBarcode == null)
                        {
                            cmd.Parameters["@NoExBarcode"].Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters["@NoExBarcode"].Value = data.NoExBarcode;
                        }
                        if (data.EAN_UPC == null)
                        {
                            cmd.Parameters["@EAN_UPC"].Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters["@EAN_UPC"].Value = data.EAN_UPC;
                        }
                        if (data.GroupCode == null)
                        {
                            cmd.Parameters["@GroupCode"].Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters["@GroupCode"].Value = data.GroupCode;
                        }
                        if (data.ProductCode == null)
                        {
                            cmd.Parameters["@ProductCode"].Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters["@ProductCode"].Value = data.ProductCode;
                        }

                        cmd.Parameters["@SKUCode"].Value = data.SKUCode;
                        cmd.Parameters["@ScanMode"].Value = data.ScanMode;
                        cmd.Parameters["@CreateDate"].Value = createDate;
                        cmd.Parameters["@CreateBy"].Value = username;

                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                    connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    connection.Close();
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    return false;
                }
            }         
        }

        private SqlCeCommand PrepareInsertMasterBarcodeCommand(SqlCeConnection conn, SqlCeTransaction transaction)
        {
            // create SQL command object
            SqlCeCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;

            // create SQL command text
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO tb_m_Barcode ");
            sb.Append("VALUES (@Status, @ExBarcode, @Barcode, @NoExBarcode, @EAN_UPC, @GroupCode, @ProductCode, @SKUCode, @ScanMode, @CreateDate, @CreateBy);");
            command.CommandText = sb.ToString();

            // define parameter type
            command.Parameters.Add("@Status", SqlDbType.NVarChar, 1);
            command.Parameters.Add("@ExBarcode", SqlDbType.NVarChar, 13);
            command.Parameters.Add("@Barcode", SqlDbType.NVarChar, 13);
            command.Parameters.Add("@NoExBarcode", SqlDbType.NVarChar, 2);
            command.Parameters.Add("@EAN_UPC", SqlDbType.NVarChar, 1);
            command.Parameters.Add("@GroupCode", SqlDbType.NVarChar, 2);
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar, 6);
            command.Parameters.Add("@SKUCode", SqlDbType.NVarChar, 25);
            command.Parameters.Add("@ScanMode", SqlDbType.Int);
            command.Parameters.Add("@CreateDate", SqlDbType.DateTime);
            command.Parameters.Add("@CreateBy", SqlDbType.NVarChar, 20);

            command.Prepare();

            return command;
        }

        public bool ConnectSdfInsertMasterSerialData(List<MasterSerialNumberModel> masterSerialNumberList, string dbfile, string username)
        {
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
            //string dbfile = new System.IO.FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) + "\\" + DBName;

            using (SqlCeConnection connection = new SqlCeConnection("datasource=" + dbfile + ";password=" + DBPassword))
            {
                connection.Open();
                SqlCeTransaction tx = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                SqlCeCommand cmd = PrepareInsertMasterBarcodeCommand(connection, tx);
                try
                {
                    SqlCeCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.Transaction = tx;

                    DateTime createDate = DateTime.Now;
                    foreach (MasterSerialNumberModel data in masterSerialNumberList)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("INSERT INTO Tb_m_SerialNumber ");
                        sb.Append("VALUES ('" + data.SKUCode + "', '" + data.Barcode + "'");
                        sb.Append(",'" + data.SerialNumber + "', '" + data.StorageLocation + "');");
                        command.CommandText = sb.ToString();
                        command.ExecuteNonQuery();
                    }

                    tx.Commit();
                    connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    connection.Close();
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    return false;
                }
            }
        }

        public bool ConnectSdfInsertMasterPackData(List<MasterPackModel> masterPackList, string dbfile, string username)
        {
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
           // string dbfile = new System.IO.FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) + "\\" + DBName;

            using (SqlCeConnection connection = new SqlCeConnection("datasource=" + dbfile + ";password=" + DBPassword))
            {
                connection.Open();
                SqlCeTransaction tx = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                SqlCeCommand cmd = PrepareInsertMasterPackCommand(connection, tx);
                try
                {
                    DateTime createDate = DateTime.Now;
                    foreach (MasterPackModel data in masterPackList)
                    {
                        if (data.Status == null)
                        {
                            cmd.Parameters["@Status"].Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters["@Status"].Value = data.Status;
                        }
                        cmd.Parameters["@GroupCode"].Value = data.GroupCode;
                        cmd.Parameters["@ProductCode"].Value = data.ProductCode;
                        cmd.Parameters["@Barcode"].Value = data.Barcode;

                        if (data.ProductName == null)
                        {
                            cmd.Parameters["@ProductName"].Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters["@ProductName"].Value = data.ProductName;
                        }
                        if (data.UnitQTY == null)
                        {
                            cmd.Parameters["@UnitQTY"].Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters["@UnitQTY"].Value = data.UnitQTY;
                        }
                        if (data.SKUCode == null)
                        {
                            cmd.Parameters["@SKUCode"].Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters["@SKUCode"].Value = data.SKUCode;
                        }

                        cmd.Parameters["@CreateDate"].Value = createDate;
                        cmd.Parameters["@CreateBy"].Value = username;

                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                    connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    connection.Close();
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    return false;
                }
            }
        }

        private SqlCeCommand PrepareInsertMasterPackCommand(SqlCeConnection conn, SqlCeTransaction transaction)
        {
            // create SQL command object
            SqlCeCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;

            // create SQL command text
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO tb_m_Pack ");
            sb.Append("VALUES (@Status, @GroupCode, @ProductCode, @Barcode, @ProductName, @UnitQTY, @SKUCode, @CreateDate, @CreateBy);");
            command.CommandText = sb.ToString();

            // define parameter type
            command.Parameters.Add("@Status", SqlDbType.NVarChar, 1);
            command.Parameters.Add("@GroupCode", SqlDbType.NVarChar, 2);
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar, 6);
            command.Parameters.Add("@Barcode", SqlDbType.NVarChar, 20);
            command.Parameters.Add("@ProductName", SqlDbType.NVarChar, 45);
            command.Parameters.Add("@UnitQTY", SqlDbType.Int);
            command.Parameters.Add("@SKUCode", SqlDbType.NVarChar, 25); ;
            command.Parameters.Add("@CreateDate", SqlDbType.DateTime);
            command.Parameters.Add("@CreateBy", SqlDbType.NVarChar, 20);

            command.Prepare();

            return command;
        }

        public bool ConnectSdfInsertSKUData(List<PCSKUModel> skuList, string dbfile, string username)
        {
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
            //string dbfile = new System.IO.FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) + "\\" + DBName;
            SqlCeConnection connection = new SqlCeConnection("datasource=" + dbfile + ";password=" + DBPassword);
            connection.Open();
            SqlCeTransaction tx = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            SqlCeCommand cmd = PrepareInsertSKUCommand(connection, tx);
            try
            {
                DateTime createDate = DateTime.Now;
                foreach (PCSKUModel sku in skuList)
                {
                    cmd.Parameters["@Department"].Value = sku.Department;
                    cmd.Parameters["@SKUCode"].Value = sku.SKUCode;
                    if (sku.BrandCode == null)
                    {
                        cmd.Parameters["@BrandCode"].Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters["@BrandCode"].Value = sku.BrandCode;
                    }
                    cmd.Parameters["@BrandName"].Value = DBNull.Value;
                    cmd.Parameters["@ExBarcode"].Value = sku.ExBarcode;
                    cmd.Parameters["@InBarcode"].Value = sku.InBarcode;
                    cmd.Parameters["@Description"].Value = sku.Description;
                    if (sku.Price == null)
                    {
                        cmd.Parameters["@Price"].Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters["@Price"].Value = sku.Price;
                    }
                    if (sku.QTYOnHand == null)
                    {
                        cmd.Parameters["@QTYOnHand"].Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters["@QTYOnHand"].Value = sku.QTYOnHand;
                    }
                    if (sku.StockOnHand == null)
                    {
                        cmd.Parameters["@StockOnHand"].Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters["@StockOnHand"].Value = sku.StockOnHand;
                    }

                    cmd.Parameters["@CreateDate"].Value = createDate;
                    cmd.Parameters["@CreateBy"].Value = username;
                    cmd.Parameters["@Department"].Value = sku.Department;
                    if (sku.MKCode == null)
                    {
                        cmd.Parameters["@MKCode"].Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters["@MKCode"].Value = sku.MKCode;
                    }

                    cmd.ExecuteNonQuery();
                }
                tx.Commit();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                connection.Close();
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        private SqlCeCommand PrepareInsertSKUCommand(SqlCeConnection conn, SqlCeTransaction transaction)
        {
            // create SQL command object
            SqlCeCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;

            // create SQL command text
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO tb_m_SKU ");
            sb.Append("VALUES (@Department, @SKUCode, @BrandCode, @BrandName, @ExBarcode, @InBarcode, @Description, @Price, @QTYOnHand, @StockOnHand, @CreateDate, @CreateBy, @MKCode);");
            command.CommandText = sb.ToString();

            // define parameter type
            command.Parameters.Add("@Department", SqlDbType.NVarChar, 3);
            command.Parameters.Add("@SKUCode", SqlDbType.NVarChar, 25);
            command.Parameters.Add("@BrandCode", SqlDbType.NVarChar, 5);
            command.Parameters.Add("@BrandName", SqlDbType.NVarChar, 50);
            command.Parameters.Add("@ExBarcode", SqlDbType.NVarChar, 25);
            command.Parameters.Add("@InBarcode", SqlDbType.NVarChar, 25);
            command.Parameters.Add("@Description", SqlDbType.NVarChar, 50);
            command.Parameters.Add("@Price", SqlDbType.Money);
            command.Parameters.Add("@QTYOnHand", SqlDbType.Int);
            command.Parameters.Add("@StockOnHand", SqlDbType.Int);
            command.Parameters.Add("@CreateDate", SqlDbType.DateTime);
            command.Parameters.Add("@CreateBy", SqlDbType.NVarChar, 20);
            command.Parameters.Add("@MKCode", SqlDbType.NVarChar, 5);

            command.Prepare();

            return command;
        }

        private SqlCeCommand PrepareInsertUnitCommand(SqlCeConnection conn, SqlCeTransaction transaction)
        {
            // create SQL command object
            SqlCeCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;

            // create SQL command text
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO tb_m_Unit ");
            sb.Append("VALUES (@UnitCode, @UnitName, @CodeType, @CreateDate, @CreateBy, @UpdateDate, @UpdateBy);");
            command.CommandText = sb.ToString();

            // define parameter type
            command.Parameters.Add("@UnitCode", SqlDbType.Int);
            command.Parameters.Add("@UnitName", SqlDbType.NVarChar, 5);
            command.Parameters.Add("@CodeType", SqlDbType.NVarChar, 1);
            command.Parameters.Add("@CreateDate", SqlDbType.DateTime);
            command.Parameters.Add("@CreateBy", SqlDbType.NVarChar, 20);
            command.Parameters.Add("@UpdateDate", SqlDbType.DateTime);
            command.Parameters.Add("@UpdateBy", SqlDbType.NVarChar, 20);

            command.Prepare();

            return command;
        }

        public bool ConnectSdfInsertUnitData(List<UnitModel> unitList, string dbfile, string username)
        {
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
            
            SqlCeConnection connection = new SqlCeConnection("datasource=" + dbfile + ";password=" + DBPassword);
            connection.Open();
            SqlCeTransaction tx = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            SqlCeCommand cmd = PrepareInsertUnitCommand(connection, tx);
            try
            {
                DateTime createDate = DateTime.Now;
                foreach (UnitModel unit in unitList)
                {
                    cmd.Parameters["@UnitCode"].Value = unit.UnitCode;
                    cmd.Parameters["@UnitName"].Value = unit.UnitName;
                    cmd.Parameters["@CodeType"].Value = unit.CodeType;
                    cmd.Parameters["@CreateDate"].Value = createDate;
                    cmd.Parameters["@CreateBy"].Value = username;
                    cmd.Parameters["@UpdateDate"].Value = createDate;
                    cmd.Parameters["@UpdateBy"].Value = username;

                    cmd.ExecuteNonQuery();
                }

                tx.Commit();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                connection.Close();
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }
    }
}
