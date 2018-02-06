using FSBT_HHT_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_DAL.DAO
{
    public class GenTextFileDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        public List<GenTextFileModel> searchHHT(String hhtName, String sectionCode, String sectionName, String locationFrom, String locationTo, String deptCode, int sectionType)
        {
            Entities dbContext = new Entities();
            List<GenTextFileModel> genTextFile = new List<GenTextFileModel>();
            LocationManagementDAO localDAO = new LocationManagementDAO();

            try
            {
                var tmpHHT = dbContext.HHTStocktakings.Where(x => x.ScanMode.Equals(sectionType))
                                                        .GroupBy(n => new { n.HHTName, n.LocationCode })
                                                        .Select(z => new
                                                        {
                                                            LocationCode = z.Key.LocationCode,
                                                            Count = z.Count(),
                                                            HHTName = z.Key.HHTName
                                                        });

                genTextFile = (from hht in tmpHHT
                               join l in dbContext.Locations on hht.LocationCode equals l.LocationCode
                               join s in dbContext.Sections
                               on new { SectionCode = l.SectionCode, ScanMode = l.ScanMode }
                               equals new { SectionCode = s.SectionCode, ScanMode = s.ScanMode }
                               select new GenTextFileModel
                               {
                                   HHTName = hht.HHTName,
                                   SectionCode = s.SectionCode,
                                   SectionName = s.SectionName,
                                   LocationCode = l.LocationCode,
                                   RecordAmount = hht.Count,
                                   ScanMode = s.ScanMode,
                                   DepartmentCode = s.DepartmentCode
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
                                       ScanMode = sl.ScanMode,
                                       DepartmentCode = sl.DepartmentCode
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
                                       ScanMode = sl.ScanMode,
                                       DepartmentCode = sl.DepartmentCode
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
                                       ScanMode = sl.ScanMode,
                                       DepartmentCode = sl.DepartmentCode
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
                                           ScanMode = sl.ScanMode,
                                           DepartmentCode = sl.DepartmentCode
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
                                           ScanMode = sl.ScanMode,
                                           DepartmentCode = sl.DepartmentCode
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
                                           ScanMode = sl.ScanMode,
                                           DepartmentCode = sl.DepartmentCode
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
                                           ScanMode = sl.ScanMode,
                                           DepartmentCode = sl.DepartmentCode
                                       }).ToList();
                    }
                }

                if (!(string.IsNullOrWhiteSpace(deptCode)))
                {
                    genTextFile = (from sl in genTextFile
                                   where sl.DepartmentCode.Equals(deptCode)
                                   select new GenTextFileModel
                                   {
                                       HHTName = sl.HHTName,
                                       SectionCode = sl.SectionCode,
                                       SectionName = sl.SectionName,
                                       LocationCode = sl.LocationCode,
                                       RecordAmount = sl.RecordAmount,
                                       ScanMode = sl.ScanMode,
                                       DepartmentCode = sl.DepartmentCode
                                   }).ToList();
                }

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                                join m in dbContext.MasterScanModes on h.ScanMode equals m.ScanModeID
                                where h.CountDate.Equals(countDateSetting) && m.ScanModeName.ToLower().Equals("fresh food")
                                select new
                                {
                                    StocktakingID = h.StocktakingID,
                                    ScanMode = h.ScanMode,
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
                dtFreshFood.Columns.Add("ScanMode");
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
                    row["ScanMode"] = element.ScanMode;
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                                join m in dbContext.MasterScanModes on h.ScanMode equals m.ScanModeID
                                where h.CountDate.Equals(countDateSetting) && m.ScanModeName.ToLower().Equals("front")
                                select new
                                {
                                    StocktakingID = h.StocktakingID,
                                    ScanMode = h.ScanMode,
                                    LocationCode = h.LocationCode,
                                    Barcode = h.Barcode,
                                    Quantity = h.Quantity,
                                    NewQuantity = h.NewQuantity
                                });

                dtFront.Columns.Add("StocktakingID");
                dtFront.Columns.Add("ScanMode");
                dtFront.Columns.Add("LocationCode");
                dtFront.Columns.Add("Barcode");
                dtFront.Columns.Add("Quantity");
                dtFront.Columns.Add("NewQuantity");

                foreach (var element in queryHHT)
                {
                    var row = dtFront.NewRow();
                    row["StocktakingID"] = element.StocktakingID;
                    row["ScanMode"] = element.ScanMode;
                    row["LocationCode"] = element.LocationCode;
                    row["Barcode"] = element.Barcode;
                    row["Quantity"] = element.Quantity;
                    row["NewQuantity"] = element.NewQuantity;
                    dtFront.Rows.Add(row);
                }

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                                join m in dbContext.MasterScanModes on h.ScanMode equals m.ScanModeID
                                where h.CountDate.Equals(countDateSetting) && m.ScanModeName.ToLower().Equals("back")
                                select new
                                {
                                    StocktakingID = h.StocktakingID,
                                    ScanMode = h.ScanMode,
                                    LocationCode = h.LocationCode,
                                    Barcode = h.Barcode,
                                    Quantity = h.Quantity
                                });

                dtBack.Columns.Add("StocktakingID");
                dtBack.Columns.Add("ScanMode");
                dtBack.Columns.Add("LocationCode");
                dtBack.Columns.Add("Barcode");
                dtBack.Columns.Add("Quantity");

                foreach (var element in queryHHT)
                {
                    var row = dtBack.NewRow();
                    row["StocktakingID"] = element.StocktakingID;
                    row["ScanMode"] = element.ScanMode;
                    row["LocationCode"] = element.LocationCode;
                    row["Barcode"] = element.Barcode;
                    row["Quantity"] = element.Quantity;
                    dtBack.Rows.Add(row);
                }

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                                join m in dbContext.MasterScanModes on h.ScanMode equals m.ScanModeID
                                where h.CountDate.Equals(countDateSetting) && m.ScanModeName.ToLower().Equals("warehouse")
                                select new
                                {
                                    StocktakingID = h.StocktakingID,
                                    ScanMode = h.ScanMode,
                                    Barcode = h.Barcode,
                                    Quantity = h.Quantity
                                });

                dtStock.Columns.Add("StocktakingID");
                dtStock.Columns.Add("ScanMode");
                dtStock.Columns.Add("Barcode");
                dtStock.Columns.Add("Quantity");

                foreach (var element in queryHHT)
                {
                    var row = dtStock.NewRow();
                    row["StocktakingID"] = element.StocktakingID;
                    row["ScanMode"] = element.ScanMode;
                    row["Barcode"] = element.Barcode;
                    row["Quantity"] = element.Quantity;
                    dtStock.Rows.Add(row);
                }

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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

        public DataTable getSearchUploadFile(String DeptCode, String FileCode)
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    //SqlDataAdapter da = new SqlDataAdapter();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_SEARECHUPLOADFILE", con);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@DeptCode", SqlDbType.VarChar).Value = DeptCode;
                    cmd.Parameters.Add("@FileCode", SqlDbType.VarChar).Value = FileCode;
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                    string cmd = "EXEC [dbo].[SP_GETUPLOADFILEPCS0009] @DeptCode ='" + DeptCode + "'";

                    da = new SqlDataAdapter(cmd, con);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                    SqlCommand cmd = new SqlCommand("SP_GETUPLOADFILEPCS0011", con);
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                    SqlCommand cmd = new SqlCommand("SP_GETUPLOADFILEPCS0007", con);
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                    SqlCommand cmd = new SqlCommand("SP_GETUPLOADFILEPCS0012", con);
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                    SqlCommand cmd = new SqlCommand("SP_GETUPLOADFILEPCS0004", con);
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                    SqlCommand cmd = new SqlCommand("SP_GETUPLOADFILEPCS0010", con);
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                    SqlCommand cmd = new SqlCommand("SP_SUMEXPORTFILE", con);
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }

            return dt;
        }
    }
}
