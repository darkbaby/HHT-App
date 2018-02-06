using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_DAL.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using FSBT_HHT_Model;

namespace FSBT_HHT_DAL.DAO
{
    public class ReportManagementDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        public List<MasterReport> LoadReportByUser(string username)
        {
            Entities dbContext = new Entities();
            List<MasterReport> reportList = new List<MasterReport>();
            try
            {
                reportList = (from user in dbContext.Users
                              join userGroup in dbContext.ConfigUserGroups on user.Username equals userGroup.Username
                              join userGroupReportMap in dbContext.ConfigUserGroupReports on userGroup.GroupID equals userGroupReportMap.GroupID
                              join report in dbContext.MasterReports on userGroupReportMap.ReportCode equals report.ReportCode
                              where user.Username == username
                              orderby report.ReportName
                              select report).ToList<MasterReport>();

                return reportList;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return reportList;
            }
        }

        public List<string> LoadReportConfigComponentByReport(string reportCode)
        {
            Entities dbContext = new Entities();
            List<string> lstCfgComponent = new List<string>();
            try
            {
                lstCfgComponent = (from report in dbContext.MasterReports
                                   join reportConfig in dbContext.ConfigReports on report.ReportCode equals reportConfig.ReportCode
                                   where reportConfig.ReportCode == reportCode
                                   select reportConfig.ConditionObject).ToList<string>();

                return lstCfgComponent;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return lstCfgComponent;
            }
        }

        public List<ConfigReport> LoadReportConfig()
        {
            Entities dbContext = new Entities();
            List<ConfigReport> lstCfgComponent = new List<ConfigReport>();
            try
            {
                lstCfgComponent = (from report in dbContext.MasterReports
                                   join reportConfig in dbContext.ConfigReports on report.ReportCode equals reportConfig.ReportCode
                                   select reportConfig).ToList<ConfigReport>();

                return lstCfgComponent;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return lstCfgComponent;
            }
        }

        public List<string> GetDepartmentCodeList()
        {
            Entities dbContext = new Entities();
            List<string> departmentList = new List<string>();

            try
            {
                departmentList = (from section in dbContext.Sections
                                  select section.DepartmentCode).ToList<string>();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                departmentList = new List<string>();
            }

            return departmentList;
        }

        public List<string> GetSectionCodeList()
        {
            Entities dbContext = new Entities();
            List<string> sectionList = new List<string>();

            try
            {
                sectionList = (from section in dbContext.Sections
                               select section.SectionCode).ToList<string>();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                sectionList = new List<string>();
            }

            return sectionList;
        }

        public List<string> GetLocationList()
        {
            Entities dbContext = new Entities();
            List<string> locationList = new List<string>();

            try
            {
                locationList = (from location in dbContext.Locations
                                select location.LocationCode).ToList<string>();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                locationList = new List<string>();
            }

            return locationList;
        }

        public List<ReportMasterBrand> GetBrandList()
        {
            Entities dbContext = new Entities();
            List<ReportMasterBrand> brandList = new List<ReportMasterBrand>();
            List<ReportMasterBrand> brandSKUList = new List<ReportMasterBrand>();
            List<ReportMasterBrand> unionBrandList = new List<ReportMasterBrand>();
            try
            {
                //brandList = (from sku in dbContext.MasterSKUs
                //             where sku.BrandCode != null
                //             select sku.BrandCode
                //            ).Distinct().ToList<string>();

                brandList = (from b in dbContext.MasterBrands

                             select new ReportMasterBrand
                             {
                                 BrandCode = b.BrandCode,
                                 //BrandName = b.BrandCode + " : " + b.BrandName
                             }).ToList();

                brandSKUList = (from sku in dbContext.MasterSKUs
                                where sku.ScanMode.Equals(3) && (sku.BrandCode != string.Empty)
                                select new ReportMasterBrand
                                {
                                    BrandCode = sku.BrandCode,
                                    //BrandName = sku.BrandCode + " : "
                                }).Distinct().ToList();


                unionBrandList = brandList.Union(brandSKUList).Distinct().ToList();

                var result = (from bb in unionBrandList
                           join mb in dbContext.MasterBrands on bb.BrandCode equals mb.BrandCode into jj
                           from b in jj.DefaultIfEmpty(new MasterBrand())
                           select new 
                           {
                               BrandCode = bb.BrandCode,
                               BrandName = (bb.BrandCode + " : " + b.BrandName)
                           }).OrderBy(x=> x.BrandCode).ToList();
                unionBrandList.Clear();

                foreach (var i in result)
                {
                    var brand = new ReportMasterBrand()
                    {
                        BrandCode = i.BrandCode,
                        BrandName = i.BrandName
                    };
                    unionBrandList.Add(brand);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                unionBrandList = new List<ReportMasterBrand>();
            }

            return unionBrandList;
        }

        public List<string> GetBarcodeList()
        {
            Entities dbContext = new Entities();
            List<string> barcode = new List<string>();

            try
            {
                barcode = (from sku in dbContext.MasterSKUs select sku.ExBarcode)
                            .Union(from sku in dbContext.MasterSKUs select sku.InBarcode)
                            .Distinct().ToList<string>();

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                barcode = new List<string>();
            }

            return barcode;
        }

        public string GetReportNameByReportCode(string reportCode)
        {
            Entities dbContext = new Entities();
            string reportName = "";

            try
            {
                reportName = (from rpt in dbContext.MasterReports
                              where rpt.ReportCode.ToUpper().Equals(reportCode)
                              select rpt.ReportName).FirstOrDefault();

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                reportName = "";
            }

            return reportName;
        }

        public string GetReportURLByReportCode(string reportCode)
        {
            Entities dbContext = new Entities();
            string reportName = "";

            try
            {
                reportName = (from rpt in dbContext.MasterReports
                              where rpt.ReportCode.ToUpper().Equals(reportCode)
                              select rpt.ReportFile).FirstOrDefault();

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                reportName = "";
            }

            return reportName;
        }

        public string GetReportFileFromReportCode(string reportCode)
        {
            Entities dbContext = new Entities();
            string result = "";
            try
            {
                result = (from m in dbContext.MasterReports
                          where m.ReportCode.ToUpper().Trim().Equals(reportCode.ToUpper().Trim())
                          select m.ReportFile).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return result;
            }
        }

        public DataTable GetReport_SumStockOnHand(string allBrandCode, DateTime countDate, string allDepartmentCode)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_SumStockOnHand", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            return resultTable;
        }

        public DataTable GetReport_SumStockOnHandWarehouse(string allBrandCode, DateTime countDate, string allDepartmentCode)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_SumStockOnHandWarehouse", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            return resultTable;
        }

        public DataTable GetReport_SumStockOnHandFreshFood(string allBrandCode, DateTime countDate, string allDepartmentCode)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_SumStockOnHandFreshFood", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.DateTime).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }

            return resultTable;
        }

        public DataTable GetReport_SectionLocationByBrandGroup(string allSectionCode, string allStoreType, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_SectionLocationByBrandGroup", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
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

        public DataTable GetReport_StocktakingAuditCheckWithUnit(string allLocationCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allSectionCode, string allBrandCode)
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

        public DataTable GetReport_StocktakingAuditCheck(string allLocationCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allSectionCode, string allBrandCode)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_StocktakingAuditCheck", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
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

        public DataTable GetReport_DeleteRecordReportByLocation(DateTime countDate, string allDepartmentCode, string allSectionCode, string allLocationCode, string allBrandCode, string allStoreType)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();

            try
            {


                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_DeleteRecordReportByLocation", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            return resultTable;
        }

        public DataTable GetReport_DeleteRecordReportBySection(DateTime countDate, string allDepartmentCode, string allSectionCode, string allLocationCode, string allBrandCode, string allStoreType)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_DeleteRecordReportBySection", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            return resultTable;
        }

        public DataTable GetReport_StocktakingAuditAdjustWithUnit(string allLocationCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allSectionCode, string allBrandCode, string allCorrection)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_StocktakingAuditAdjustWithUnit", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                    cmd.Parameters.Add("@Correction", SqlDbType.VarChar).Value = allCorrection;
                    cmd.CommandTimeout = 100;

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

        public DataTable GetReport_StocktakingAudiAdjust(string allLocationCode, string allStoreType, string allCorrection, DateTime countDate, string allDepartmentCode, string allSectionCode, string allBrandCode, string allSectionName)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_StocktakingAudiAdjust", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@SectionName", SqlDbType.VarChar).Value = allSectionName;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                    cmd.Parameters.Add("@Correction", SqlDbType.VarChar).Value = allCorrection;
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

        public DataTable GetReport_ControlSheet(string allSectionCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode, string allSectionName)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_ControlSheet", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@SectionName", SqlDbType.VarChar).Value = allSectionName;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            return resultTable;
        }

        public DataTable GetReport_UncountedLocation(string allSectionCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_UncountedLocation", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            return resultTable;
        }

        public DataTable GetReport_UnidentifiedStockItem(string allLocationCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allSectionCode, string allBrandCode)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_UnidentifiedStockItem", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
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

        public DataTable GetReport_InventoryControlBySection(DateTime countDate, string allDeprtmentCode, string allSectionCode, string allLocationCode, string allBrandCode, string allStoreType, string allDifftype, string allUnit, string StoreMode, string unit)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                if (allStoreType == "4")
                {
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SP_GET_InventoryControlBySectionFreshFood", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDeprtmentCode;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = StoreMode;

                        cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }
                }
                else if (allStoreType == "3")
                {
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SP_GET_InventoryControlBySectionWarehouse", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDeprtmentCode;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = StoreMode;
                        cmd.Parameters.Add("@UnitReport", SqlDbType.VarChar).Value = unit;

                        cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }
                }
                else if (allStoreType == "2")
                {
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SP_GET_InventoryControlBySectionBack", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDeprtmentCode;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = StoreMode;
                        cmd.Parameters.Add("@UnitReport", SqlDbType.VarChar).Value = unit;

                        cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }
                }
                else if (allStoreType == "1")
                {
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SP_GET_InventoryControlBySectionFront", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDeprtmentCode;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = StoreMode;
                        cmd.Parameters.Add("@UnitReport", SqlDbType.VarChar).Value = unit;

                        cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            return resultTable;
        }

        public DataTable GetReport_InventoryControlByLocation(DateTime countDate, string allDepartmentCode, string allSectionCode, string allLocationCode, string allBrandCode, string allStoreType, string allDifftype, string allUnit, string storeMode, string unit)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                if (allStoreType == "4")
                {
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SP_GET_InventoryControlByLocationFreshFood", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = storeMode;

                        cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }
                }
                if (allStoreType == "3")
                {
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SP_GET_InventoryControlByLocationWarehouse", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = storeMode;
                        cmd.Parameters.Add("@UnitReport", SqlDbType.VarChar).Value = unit;

                        cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }
                }
                if (allStoreType == "2")
                {
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SP_GET_InventoryControlByLocationBack", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = storeMode;

                        cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }
                }
                if (allStoreType == "1")
                {
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SP_GET_InventoryControlByLocationFront", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = storeMode;

                        cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            return resultTable;
        }

        public DataTable GetReport_InventoryControlByBarcode(DateTime countDate, string allDepartmentCode, string allSectionCode, string allLocationCode, string allBrandCode, string allStoreType, string allDifftype, string allUnit, string allBarcode, string storeMode, string unit)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                if (allStoreType == "4")
                {
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SP_GET_InventoryControlByBarcodeFreshFood", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                        cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = allBarcode;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = storeMode;

                        cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }
                }
                else if (allStoreType == "3")
                {
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SP_GET_InventoryControlByBarcodeWarehouse", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                        cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = allBarcode;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = storeMode;
                        cmd.Parameters.Add("@UnitReport", SqlDbType.VarChar).Value = unit;

                        cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }
                }
                else if (allStoreType == "2")
                {
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SP_GET_InventoryControlByBarcodeBack", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                        cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = allBarcode;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = storeMode;
                        cmd.Parameters.Add("@UnitReport", SqlDbType.VarChar).Value = unit;

                        cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }
                }
                else if (allStoreType == "1")
                {
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SP_GET_InventoryControlByBarcodeFront", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                        cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = allBarcode;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = storeMode;
                        cmd.Parameters.Add("@UnitReport", SqlDbType.VarChar).Value = unit;

                        cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            return resultTable;
        }

        public DataSet GetReport_ItemPhysicalCountBySection(string allSectionCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            DataSet dsDataSet = new DataSet();
            DataTable resultTable1 = new DataTable();
            DataTable resultTable2 = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_ItemPhysicalCountBySection", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dsDataSet);

                    dsDataSet.Tables[0].TableName = "MasterReport";
                    dsDataSet.Tables[1].TableName = "MasterSummary";

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            return dsDataSet;
        }

        public DataSet GetReport_ItemPhysicalCountByBarcode(string allBarcode, string allStoreType, DateTime countDate, string allDepartmentCode, string allSectionCode, string allLocationCode, string allBrandCode)
        {
            Entities dbContext = new Entities();
            DataSet dsDataSet = new DataSet();
            DataTable resultTable1 = new DataTable();
            DataTable resultTable2 = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_ItemPhysicalCountByBarcode", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = allBarcode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dsDataSet);

                    dsDataSet.Tables[0].TableName = "MasterReport";
                    dsDataSet.Tables[1].TableName = "MasterSummary";

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            return dsDataSet;
        }

        public DataTable GetReport_GroupSummaryReportByFrontBack(string allSectionCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_GroupSummaryReportByFrontBack", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
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

        public DataTable GetReport_GroupSummaryReportByFreshFoodWarehouse(string allSectionCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_GroupSummaryReportByFreshFoodWarehouse", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
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

        public DataTable GetReport_CountedLocationsReport(string allSectionCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_CountedLocationsReport", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
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

        public DataTable GetReport_NoticeOfStocktakingSatisfactionByFrontBack(string allSectionCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_NoticeOfStocktakingSatisfactionByFrontBack", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
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

        public Hashtable GetReport_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse(string allSectionCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            Entities dbContext = new Entities();
            DataSet dsDataSet = new DataSet();
            DataTable resultTable1 = new DataTable();
            DataTable resultTable2 = new DataTable();
            Hashtable hastResult = new Hashtable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GET_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dsDataSet);

                    resultTable1 = dsDataSet.Tables[0];
                    resultTable2 = dsDataSet.Tables[1];

                    conn.Close();
                }

                hastResult.Add("dtTable1", resultTable1);
                hastResult.Add("dtTable2", resultTable2);

                return hastResult;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new Hashtable();
            }
        }

        public DataTable GetReport_ControlSheet(List<string> sectionCode)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                String allSectionCode = string.Join(",", sectionCode);
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SP_ADD_MASTERSKU_Front]");
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            return resultTable;
        }
    }
}
