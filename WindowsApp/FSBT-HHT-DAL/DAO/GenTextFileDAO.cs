using FSBT_HHT_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FSBT_HHT_DAL.DAO
{
    public class GenTextFileDAO
    {
        private LogErrorDAO logBll = new LogErrorDAO();
        public List<string> getColumnsName(string tablename)
        {
            Entities dbContext = new Entities();
            List<string> listacolumnas = new List<string>();
            using (SqlConnection connection = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "select c.name from sys.columns c inner join sys.tables t on t.object_id = c.object_id and t.name = '" + tablename + "' and t.type = 'U'";
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listacolumnas.Add(reader.GetString(0));
                    }
                }
            }
            return listacolumnas.ToList();
        }


        public DataTable getAllCountsheet()
        {
            List<string> listCountSheet = new List<string>();
            Entities dbContext = new Entities();
            DataTable CountSheetTable = new DataTable();
            try
            {
                listCountSheet = (from m in dbContext.MastSAP_SKU
                                  select m.PIDoc).Distinct().ToList();

                CountSheetTable.Columns.Add("IsChecked", typeof(int));
                CountSheetTable.Columns.Add("countsheet", typeof(string));

                if (listCountSheet.Count > 0)
                {
                    foreach (var l in listCountSheet)
                    {
                        DataRow row = CountSheetTable.NewRow();
                        row["IsChecked"] = 1;
                        row["countsheet"] = l;
                        CountSheetTable.Rows.Add(row);
                    }
                }
            }

            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                CountSheetTable = null;
            }

            return CountSheetTable;
        }

        public List<string> getListAllCountsheet()
        {
            List<string> listCountSheet = new List<string>();
            Entities dbContext = new Entities();
            try
            {
                listCountSheet = (from m in dbContext.MastSAP_SKU
                                  select m.PIDoc).Distinct().ToList();
            }

            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                listCountSheet = null;
            }

            return listCountSheet;
        }


        public List<GenTextFileModel> searchHHT(String hhtName, String sectionCode, String sectionName, String locationFrom, String locationTo)
        {
            Entities dbContext = new Entities();
            List<GenTextFileModel> genTextFile = new List<GenTextFileModel>();
            LocationManagementDAO localDAO = new LocationManagementDAO();

            try
            {
                var tmpHHT = dbContext.HHTStocktakings.GroupBy(n => new { n.HHTName, n.LocationCode })
                                                        .Select(z => new
                                                        {
                                                            LocationCode = z.Key.LocationCode,
                                                            Count = z.Count(),
                                                            HHTName = z.Key.HHTName
                                                        });

                genTextFile = (from hht in tmpHHT
                               join l in dbContext.Locations on hht.LocationCode equals l.LocationCode
                               join s in dbContext.Sections
                               on new { SectionCode = l.SectionCode, StorageLocationCode = l.StorageLocationCode }
                               equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                               select new GenTextFileModel
                               {
                                   HHTName = hht.HHTName,
                                   SectionCode = s.SectionCode,
                                   SectionName = s.SectionName,
                                   LocationCode = l.LocationCode,
                                   RecordAmount = hht.Count,
                                   StorageLocationCode = s.StorageLocationCode
                               }).ToList();

                if (!(string.IsNullOrWhiteSpace(hhtName)))
                {
                    genTextFile = (from sl in genTextFile
                                   where sl.HHTName.Contains(hhtName)
                                   select new GenTextFileModel
                                   {
                                       HHTName = sl.HHTName,
                                       SectionCode = sl.SectionCode,
                                       SectionName = sl.SectionName,
                                       LocationCode = sl.LocationCode,
                                       RecordAmount = sl.RecordAmount,
                                       StorageLocationCode = sl.StorageLocationCode
                                   }).ToList();
                }

                if (!(string.IsNullOrWhiteSpace(sectionCode)))
                {
                    genTextFile = (from sl in genTextFile
                                   //join s in dbContext.Sections
                                   //on new { SectionCode = sl.SectionCode, ScanMode = sl.ScanMode }
                                   //equals new { SectionCode = s.SectionCode, ScanMode = s.ScanMode }
                                   //join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                   where sl.SectionCode.Contains(sectionCode)
                                   select new GenTextFileModel
                                   {
                                       HHTName = sl.HHTName,
                                       SectionCode = sl.SectionCode,
                                       SectionName = sl.SectionName,
                                       LocationCode = sl.LocationCode,
                                       RecordAmount = sl.RecordAmount,
                                       StorageLocationCode = sl.StorageLocationCode
                                   }).ToList();
                }

                if (!(string.IsNullOrWhiteSpace(sectionName)))
                {
                    genTextFile = (from sl in genTextFile
                                   where sl.SectionName.ToUpper().Contains(sectionName.ToUpper())
                                   select new GenTextFileModel
                                   {
                                       HHTName = sl.HHTName,
                                       SectionCode = sl.SectionCode,
                                       SectionName = sl.SectionName,
                                       LocationCode = sl.LocationCode,
                                       RecordAmount = sl.RecordAmount,
                                       StorageLocationCode = sl.StorageLocationCode
                                   }).ToList();
                }

                if (!(string.IsNullOrWhiteSpace(locationFrom)))
                {
                    if (string.IsNullOrWhiteSpace(locationTo))
                    {
                        genTextFile = (from sl in genTextFile
                                       where sl.LocationCode.Equals(locationFrom)
                                       select new GenTextFileModel
                                       {
                                           HHTName = sl.HHTName,
                                           SectionCode = sl.SectionCode,
                                           SectionName = sl.SectionName,
                                           LocationCode = sl.LocationCode,
                                           RecordAmount = sl.RecordAmount,
                                           StorageLocationCode = sl.StorageLocationCode
                                       }).ToList();
                    }
                    else
                    {
                        genTextFile = (from sl in genTextFile
                                       where int.Parse(sl.LocationCode) >= int.Parse(locationFrom)
                                       select new GenTextFileModel
                                       {
                                           HHTName = sl.HHTName,
                                           SectionCode = sl.SectionCode,
                                           SectionName = sl.SectionName,
                                           LocationCode = sl.LocationCode,
                                           RecordAmount = sl.RecordAmount,
                                           StorageLocationCode = sl.StorageLocationCode
                                       }).ToList();
                    }
                }

                if (!(string.IsNullOrWhiteSpace(locationTo)))
                {
                    if (string.IsNullOrWhiteSpace(locationFrom))
                    {
                        genTextFile = (from sl in genTextFile
                                       where sl.LocationCode.Equals(locationTo)
                                       select new GenTextFileModel
                                       {
                                           HHTName = sl.HHTName,
                                           SectionCode = sl.SectionCode,
                                           SectionName = sl.SectionName,
                                           LocationCode = sl.LocationCode,
                                           RecordAmount = sl.RecordAmount,
                                           StorageLocationCode = sl.StorageLocationCode
                                       }).ToList();
                    }
                    else
                    {
                        genTextFile = (from sl in genTextFile
                                       where int.Parse(sl.LocationCode) <= int.Parse(locationTo)
                                       select new GenTextFileModel
                                       {
                                           HHTName = sl.HHTName,
                                           SectionCode = sl.SectionCode,
                                           SectionName = sl.SectionName,
                                           LocationCode = sl.LocationCode,
                                           RecordAmount = sl.RecordAmount,
                                           StorageLocationCode = sl.StorageLocationCode
                                       }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                genTextFile = new List<GenTextFileModel>();
            }

            return genTextFile;
        }

        public DataTable getHHTStocktakingFreshFoodByCountDate()
        {
            DataTable dtFreshFood = new DataTable();
            Entities dbContext = new Entities();

            DateTime countDateSetting = (from s in dbContext.SystemSettings
                                         where s.SettingKey.ToUpper().Equals("COUNTDATE")
                                         select s.ValueDate.Value
                                        ).FirstOrDefault();

            try
            {
                var queryHHT = (from h in dbContext.HHTStocktakings
                                where h.CountDate.Equals(countDateSetting)
                                select new
                                {
                                    StocktakingID = h.StocktakingID,
                                    LocationCode = h.LocationCode,
                                    Barcode = h.Barcode,
                                    Quantity = h.Quantity,
                                    NewQuantity = h.NewQuantity,
                                    UnitCode = h.UnitCode,
                                    Flag = h.Flag,
                                    Description = h.Description,
                                    SKUCode = h.SKUCode,
                                    ExBarCode = h.ExBarCode,
                                    InBarCode = h.InBarCode,
                                    SKUMode = h.SKUMode,
                                    HHTName = h.HHTName,
                                    CountDate = h.CountDate
                                });

                dtFreshFood.Columns.Add("StocktakingID");
                dtFreshFood.Columns.Add("LocationCode");
                dtFreshFood.Columns.Add("Barcode");
                dtFreshFood.Columns.Add("Quantity");
                dtFreshFood.Columns.Add("NewQuantity");
                dtFreshFood.Columns.Add("UnitCode");
                dtFreshFood.Columns.Add("Flag");
                dtFreshFood.Columns.Add("Description");
                dtFreshFood.Columns.Add("SKUCode");
                dtFreshFood.Columns.Add("ExBarCode");
                dtFreshFood.Columns.Add("InBarCode");
                dtFreshFood.Columns.Add("BrandCode");
                dtFreshFood.Columns.Add("SKUMode");
                dtFreshFood.Columns.Add("HHTName");
                dtFreshFood.Columns.Add("CountDate");

                foreach (var element in queryHHT)
                {
                    var row = dtFreshFood.NewRow();
                    row["StocktakingID"] = element.StocktakingID;
                    row["LocationCode"] = element.LocationCode;
                    row["Barcode"] = element.Barcode;
                    row["Quantity"] = element.Quantity;
                    row["NewQuantity"] = element.NewQuantity;
                    row["UnitCode"] = element.UnitCode;
                    row["Flag"] = element.Flag;
                    row["Description"] = element.Description;
                    row["SKUCode"] = element.SKUCode;
                    row["ExBarCode"] = element.ExBarCode;
                    row["InBarCode"] = element.InBarCode;
                    row["SKUMode"] = element.SKUMode;
                    row["HHTName"] = element.HHTName;
                    row["CountDate"] = element.CountDate;
                    dtFreshFood.Rows.Add(row);
                }

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                dtFreshFood = new DataTable();
            }

            return dtFreshFood;
        }

        public DataTable getHHTStocktakingFrontByCountDate()
        {
            DataTable dtFront = new DataTable();
            Entities dbContext = new Entities();

            DateTime countDateSetting = (from s in dbContext.SystemSettings
                                         where s.SettingKey.ToUpper().Equals("COUNTDATE")
                                         select s.ValueDate.Value
                                        ).FirstOrDefault();

            try
            {
                var queryHHT = (from h in dbContext.HHTStocktakings
                                where h.CountDate.Equals(countDateSetting)
                                select new
                                {
                                    StocktakingID = h.StocktakingID,
                                    LocationCode = h.LocationCode,
                                    Barcode = h.Barcode,
                                    Quantity = h.Quantity,
                                    NewQuantity = h.NewQuantity
                                });

                dtFront.Columns.Add("StocktakingID");
                dtFront.Columns.Add("LocationCode");
                dtFront.Columns.Add("Barcode");
                dtFront.Columns.Add("Quantity");
                dtFront.Columns.Add("NewQuantity");

                foreach (var element in queryHHT)
                {
                    var row = dtFront.NewRow();
                    row["StocktakingID"] = element.StocktakingID;
                    row["LocationCode"] = element.LocationCode;
                    row["Barcode"] = element.Barcode;
                    row["Quantity"] = element.Quantity;
                    row["NewQuantity"] = element.NewQuantity;
                    dtFront.Rows.Add(row);
                }

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                dtFront = new DataTable();
            }

            return dtFront;
        }

        public DataTable getHHTStocktakingBackByCountDate()
        {
            DataTable dtBack = new DataTable();
            Entities dbContext = new Entities();

            DateTime countDateSetting = (from s in dbContext.SystemSettings
                                         where s.SettingKey.ToUpper().Equals("COUNTDATE")
                                         select s.ValueDate.Value
                                        ).FirstOrDefault();

            try
            {
                var queryHHT = (from h in dbContext.HHTStocktakings
                                where h.CountDate.Equals(countDateSetting)
                                select new
                                {
                                    StocktakingID = h.StocktakingID,
                                    LocationCode = h.LocationCode,
                                    Barcode = h.Barcode,
                                    Quantity = h.Quantity
                                });

                dtBack.Columns.Add("StocktakingID");
                dtBack.Columns.Add("LocationCode");
                dtBack.Columns.Add("Barcode");
                dtBack.Columns.Add("Quantity");

                foreach (var element in queryHHT)
                {
                    var row = dtBack.NewRow();
                    row["StocktakingID"] = element.StocktakingID;
                    row["LocationCode"] = element.LocationCode;
                    row["Barcode"] = element.Barcode;
                    row["Quantity"] = element.Quantity;
                    dtBack.Rows.Add(row);
                }

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                dtBack = new DataTable();
            }

            return dtBack;
        }

        public DataTable getHHTStocktakingWarehouseByCountDate()
        {
            DataTable dtStock = new DataTable();
            Entities dbContext = new Entities();

            DateTime countDateSetting = (from s in dbContext.SystemSettings
                                         where s.SettingKey.ToUpper().Equals("COUNTDATE")
                                         select s.ValueDate.Value
                                        ).FirstOrDefault();

            try
            {
                var queryHHT = (from h in dbContext.HHTStocktakings
                                where h.CountDate.Equals(countDateSetting)
                                select new
                                {
                                    StocktakingID = h.StocktakingID,
                                    Barcode = h.Barcode,
                                    Quantity = h.Quantity
                                });

                dtStock.Columns.Add("StocktakingID");
                dtStock.Columns.Add("Barcode");
                dtStock.Columns.Add("Quantity");

                foreach (var element in queryHHT)
                {
                    var row = dtStock.NewRow();
                    row["StocktakingID"] = element.StocktakingID;
                    row["Barcode"] = element.Barcode;
                    row["Quantity"] = element.Quantity;
                    dtStock.Rows.Add(row);
                }

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                dtStock = new DataTable();
            }

            return dtStock;
        }

        public List<FileModel> GetListFileNameUploadAS400()
        {
            List<FileModel> fileNameUpload = new List<FileModel>();
            Entities dbContext = new Entities();

            try
            {
                fileNameUpload = (from c in dbContext.ConfigFileAS400
                                  where c.Type.ToUpper().Equals("UPLOAD")
                                  select new FileModel
                                  {
                                      fileID = c.FileID,
                                      fileName = c.FileName
                                  }).ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileNameUpload = new List<FileModel>();
            }

            return fileNameUpload;
        }

        public List<FileModel> GetListFileNameDownloadAS400()
        {
            List<FileModel> fileNameDownload = new List<FileModel>();
            Entities dbContext = new Entities();

            try
            {
                fileNameDownload = (from c in dbContext.ConfigFileAS400
                                    where c.Type.ToUpper().Equals("DOWNLOAD")
                                    select new FileModel
                                    {
                                        fileID = c.FileID,
                                        fileName = c.FileName
                                    }).ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileNameDownload = new List<FileModel>();
            }

            return fileNameDownload;
        }

        public String GetFileNameByFileCode(string FileCode)
        {
            String FileName = "";
            Entities dbContext = new Entities();

            FileName = (from c in dbContext.ConfigFileAS400
                        where c.FileCode.Equals(FileCode)
                        select c.FileName).FirstOrDefault();

            return FileName;
        }

        public String GetFileCodeByFileID(int FileID)
        {
            String FileCode = "";
            Entities dbContext = new Entities();

            FileCode = (from c in dbContext.ConfigFileAS400
                        where c.FileID.Equals(FileID)
                        select c.FileCode).FirstOrDefault();

            return FileCode;
        }

        public List<FileModelDetail> GetFileConfigDetail(int fileID)
        {
            List<FileModelDetail> fileDetail = new List<FileModelDetail>();
            Entities dbContext = new Entities();

            fileDetail = (from d in dbContext.ConfigFileDetailAS400
                          where d.FileID.Equals(fileID)
                          select new FileModelDetail
                          {
                              FileDetailID = d.FileDetailID,
                              FileID = d.FileID,
                              Index = d.Index,
                              PrimaryKey = d.PrimaryKey,
                              Field = d.Field,
                              Description = d.Description,
                              Type = d.Type,
                              Length = d.Length,
                              DecPos = d.DecPos,
                              StartPos = d.StartPos,
                              EndPos = d.EndPos,
                              Default = d.Default
                          }).ToList();
            return fileDetail;
        }

        public List<FileModelDetail> GetFileConfigDetailByFileCode(string fileCode)
        {
            List<FileModelDetail> fileDetail = new List<FileModelDetail>();
            Entities dbContext = new Entities();

            fileDetail = (from d in dbContext.ConfigFileDetailAS400
                          join c in dbContext.ConfigFileAS400 on d.FileID equals c.FileID
                          where c.FileCode.Equals(fileCode)
                          select new FileModelDetail
                          {
                              FileDetailID = d.FileDetailID,
                              FileID = d.FileID,
                              Index = d.Index,
                              PrimaryKey = d.PrimaryKey,
                              Field = d.Field,
                              Description = d.Description,
                              Type = d.Type,
                              Length = d.Length,
                              DecPos = d.DecPos,
                              StartPos = d.StartPos,
                              EndPos = d.EndPos,
                              Default = d.Default
                          }).ToList();
            return fileDetail;
        }

        public bool IsExistsFileCode(string fileCode)
        {
            Entities dbContext = new Entities();
            bool result = true;
            int count = 0;

            count = (from c in dbContext.ConfigFileAS400
                     where c.FileCode.ToUpper().Equals(fileCode.ToUpper())
                     select c.FileID).Count();

            if (count > 0) { result = true; } else { result = false; }

            return result;

        }

        public DataTable getSearchUploadFile(Request searchCondition)
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    //SqlDataAdapter da = new SqlDataAdapter();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SCR04_SP_SEARECHUPLOADFILE", con);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = searchCondition.PlantCode;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = searchCondition.CountSheet;
                    cmd.Parameters.Add("@MCHLevel1", SqlDbType.VarChar).Value = searchCondition.MCHLevel1;
                    cmd.Parameters.Add("@MCHLevel2", SqlDbType.VarChar).Value = searchCondition.MCHLevel2;
                    cmd.Parameters.Add("@MCHLevel3", SqlDbType.VarChar).Value = searchCondition.MCHLevel3;
                    cmd.Parameters.Add("@MCHLevel4", SqlDbType.VarChar).Value = searchCondition.MCHLevel4;
                    cmd.Parameters.Add("@StorageLocationCode", SqlDbType.VarChar).Value = searchCondition.StorageLocationCode;
                    cmd.Parameters.Add("@LocationFrom", SqlDbType.VarChar).Value = searchCondition.LocationFrom;
                    cmd.Parameters.Add("@LocationTo", SqlDbType.VarChar).Value = searchCondition.LocationTo;

                    cmd.CommandTimeout = 900;
                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dt);

                    con.Close();

                    //cmd.CommandType = CommandType.StoredProcedure;
                    //string cmd = "EXEC [dbo].[SP_SEARECHUPLOADFILE] @DeptCode ='" + DeptCode + "',@FileCode = '" + FileCode + "'";
                    ////cmd.CommandTimeout = 900;
                    //da = new SqlDataAdapter(cmd, con);
                    //da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }

            return dt;
        }

        public DataTable getUploadFilePCS0009(String DeptCode)
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    string cmd = "EXEC [dbo].[SCR04_SP_GETUPLOADFILEPCS0009] @DeptCode ='" + DeptCode + "'";

                    da = new SqlDataAdapter(cmd, con);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }

            return dt;
        }

        public DataTable getUploadFilePCS0011(String DeptCode)
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    //SqlDataAdapter da = new SqlDataAdapter();
                    //string cmd = "EXEC [dbo].[SP_GETUPLOADFILEPCS0011]@DeptCode ='" + DeptCode + "'";

                    //da = new SqlDataAdapter(cmd, con);
                    //da.Fill(dt);

                    con.Open();
                    SqlCommand cmd = new SqlCommand("SCR04_SP_GETUPLOADFILEPCS0011", con);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@DeptCode", SqlDbType.VarChar).Value = DeptCode;
                    cmd.CommandTimeout = 900;
                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dt);

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }

            return dt;
        }

        public DataTable getUploadFilePCS0007(String DeptCode)
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    //SqlDataAdapter da = new SqlDataAdapter();
                    //string cmd = "EXEC [dbo].[SP_GETUPLOADFILEPCS0007]@DeptCode ='" + DeptCode + "'";

                    //da = new SqlDataAdapter(cmd, con);
                    //da.Fill(dt);

                    con.Open();
                    SqlCommand cmd = new SqlCommand("SCR04_SP_GETUPLOADFILEPCS0007", con);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@DeptCode", SqlDbType.VarChar).Value = DeptCode;
                    cmd.CommandTimeout = 900;
                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dt);

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }

            return dt;
        }

        public DataTable getUploadFilePCS0012(String DeptCode)
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    //SqlDataAdapter da = new SqlDataAdapter();
                    //string cmd = "EXEC [dbo].[SP_GETUPLOADFILEPCS0012] @DeptCode ='" + DeptCode + "'";

                    //da = new SqlDataAdapter(cmd, con);
                    //da.Fill(dt);

                    con.Open();
                    SqlCommand cmd = new SqlCommand("SCR04_SP_GETUPLOADFILEPCS0012", con);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@DeptCode", SqlDbType.VarChar).Value = DeptCode;
                    cmd.CommandTimeout = 900;
                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dt);

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }

            return dt;
        }

        public DataTable getUploadFilePCS0004(String DeptCode)
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    //SqlDataAdapter da = new SqlDataAdapter();
                    //string cmd = "EXEC [dbo].[SP_GETUPLOADFILEPCS0004]@DeptCode ='" + DeptCode + "'";

                    //da = new SqlDataAdapter(cmd, con);
                    //da.Fill(dt);

                    con.Open();
                    SqlCommand cmd = new SqlCommand("SCR04_SP_GETUPLOADFILEPCS0004", con);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@DeptCode", SqlDbType.VarChar).Value = DeptCode;
                    cmd.CommandTimeout = 900;
                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dt);

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }

            return dt;
        }

        public DataTable getUploadFilePCS0010(String DeptCode)
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    //SqlDataAdapter da = new SqlDataAdapter();
                    //string cmd = "EXEC [dbo].[SP_GETUPLOADFILEPCS0010]@DeptCode ='" + DeptCode + "'";

                    //da = new SqlDataAdapter(cmd, con);
                    //da.Fill(dt);

                    con.Open();
                    SqlCommand cmd = new SqlCommand("SCR04_SP_GETUPLOADFILEPCS0010", con);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@DeptCode", SqlDbType.VarChar).Value = DeptCode;
                    cmd.CommandTimeout = 900;
                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dt);

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }

            return dt;
        }

        public DataTable getSumExportFile(String DeptCode,String FileCode)
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    //SqlDataAdapter da = new SqlDataAdapter();
                    //string cmd = "EXEC [dbo].[SP_SUMEXPORTFILE] @DeptCode ='" + DeptCode + "',@FileCode = '" + FileCode + "'";

                    //da = new SqlDataAdapter(cmd, con);
                    //da.Fill(dt);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SCR04_SP_SUMEXPORTFILE", con);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@DeptCode", SqlDbType.VarChar).Value = DeptCode;
                    cmd.Parameters.Add("@FileCode", SqlDbType.VarChar).Value = FileCode;
                    cmd.CommandTimeout = 900;
                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dt);

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }

            return dt;
        }

        public DataTable getTextFileData(Request search, string type)
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable(type);
            try
            {
                using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SCR04_SP_GenDataTextFile", con);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = search.PlantCode;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = search.CountSheet;
                    cmd.Parameters.Add("@MCHLevel1", SqlDbType.VarChar).Value = search.MCHLevel1;
                    cmd.Parameters.Add("@MCHLevel2", SqlDbType.VarChar).Value = search.MCHLevel2;
                    cmd.Parameters.Add("@MCHLevel3", SqlDbType.VarChar).Value = search.MCHLevel3;
                    cmd.Parameters.Add("@MCHLevel4", SqlDbType.VarChar).Value = search.MCHLevel4;
                    cmd.Parameters.Add("@StorageLocationCode", SqlDbType.VarChar).Value = search.StorageLocationCode;
                    cmd.Parameters.Add("@LocationFrom", SqlDbType.VarChar).Value = search.LocationFrom;
                    cmd.Parameters.Add("@LocationTo", SqlDbType.VarChar).Value = search.LocationTo;
                    cmd.Parameters.Add("@DataType", SqlDbType.VarChar).Value = type.Replace("MastSAP_",""); ;
                    cmd.CommandTimeout = 900;
                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
            return dt;
        }

        public bool TruncateTempTextFileData()
        {
            bool result = false;
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SCR04_SP_TruncateDataTempTextFile", con);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dt);
                    con.Close();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return result;
        }

        public DataTable InsertDataTextFile()
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SCR04_SP_InsertDataTextFile", con);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                dt = null;
            }
            return dt;
        }

        public int ProcessExportData(DataTable countsheet)
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            int returnValue = 0;

            DeleteTempData("TempCountSheet");
            InsertDataTableToDatabase(countsheet, "TempCountSheet");

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SCR04_SP_ExportFile", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        returnValue = dr["result"] != null ? Convert.ToInt32(dr["result"]) : 0;
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                returnValue = 0;

            }
            return returnValue;

        }

        public bool DeleteTempData(string tableName)
        {
            bool result = false;
            try
            {
                using (var dbContext = new Entities())
                {
                    dbContext.Database.ExecuteSqlCommand("Delete from dbo." + tableName);
                }
                result = true;
            }
            catch (Exception ex)
            {
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
                                string s = (String.Format("Column: {0} contains data with a length greater than: {1}", column, length));
                                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, s, DateTime.Now);
                            }
                        }
                        catch (Exception ex)
                        {
                            logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
                result = true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return result;
        }


        //public int ProcessExportData(List<string> countsheet)
        //{

        //    DataTable dt = new DataTable();
        //    int returnValue = 0;

        //    try
        //    {
        //        List<string> param = new List<string>();
        //        TempDataTableDAO tb = new TempDataTableDAO();
        //        param = countsheet;

        //        dt = tb.ExecStoredProcedure("SCR04_SP_ExportFile ", param);

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            returnValue = dr["result"] != null ? Convert.ToInt32(dr["result"]) : 0;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        returnValue = 0;
        //    }

        //    return returnValue;
        //}
    }
}
