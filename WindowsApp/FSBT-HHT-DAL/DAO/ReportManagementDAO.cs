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
using System.Reflection;

namespace FSBT_HHT_DAL.DAO
{
    public class ReportManagementDAO
    {
        private LogErrorDAO logBll = new LogErrorDAO(); 
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return lstCfgComponent;
            }
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                             orderby b.BrandCode
                             select new ReportMasterBrand
                             {
                                 BrandCode = b.BrandCode,
                                 BrandName = b.BrandCode + " : " + b.BrandName
                             }).ToList();
                
               /* brandSKUList = (from sku in dbContext.MasterSKUs
                                //where sku.StorageLocation.Equals(3) && (sku.BrandCode != string.Empty)
                                select new ReportMasterBrand
                                {
                                    BrandCode = sku.BrandCode
                                    //BrandName = sku.BrandCode + " : "
                                }).Distinct().ToList(); */


                //unionBrandList = brandList.Union(brandSKUList).Distinct().ToList();

                //var result = (from bb in unionBrandList
                //           join mb in dbContext.MasterBrands on bb.BrandCode equals mb.BrandCode into jj
                //           from b in jj.DefaultIfEmpty(new MasterBrand())
                //           select new 
                //           {
                //               BrandCode = bb.BrandCode,
                //               BrandName = (bb.BrandCode + " : " + b.BrandName)
                //           }).OrderBy(x=> x.BrandCode).ToList();
                //unionBrandList.Clear();

                //foreach (var i in result)
                //{
                //    var brand = new ReportMasterBrand()
                //    {
                //        BrandCode = i.BrandCode,
                //        BrandName = i.BrandName
                //    };
                //    unionBrandList.Add(brand);
                //}
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                brandList = new List<ReportMasterBrand>();
            }

            return brandList;
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return result;
            }
        }

        public string GetLocation(ReportParameter reportParam)
        { 
            string allLocationCode = "";
            string[] locationCode = reportParam.LocationCode.Split('-');
            int length = 5;

            if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
            {
                allLocationCode = locationCode[0];
            }
            else if (locationCode.Length > 1)
            {
                int locationCodeFrom = int.Parse(locationCode[0]);
                int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                {
                    if (i == 0)
                    {
                        allLocationCode = locationCodeFrom.ToString("D" + length);
                    }
                    else
                    {
                        allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                    }

                    locationCodeFrom++;
                }
            }
            else
            {
                allLocationCode = reportParam.LocationCode;
            }
            return allLocationCode;
        }
        public DataTable GetReport_SumStockOnHand(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT01-04_SP_GET_SumStockOnHand", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return resultTable;
        }
        public DataTable GetReport_SumStockOnHandFreshFood(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT03-06_SP_GET_SumStockOnHandFreshFood", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }

            return resultTable;
        }
        public DataTable GetReport_SumStockOnHandWarehouse(DateTime countDate, ReportParameter reportParam)
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


                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return resultTable;
        }

        public DataTable GetReport_SectionLocationByBrandGroup(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            string allLocationCode = "";
            allLocationCode = GetLocation(reportParam);
            
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT07_SP_GET_SectionLocationByBrandGroup", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;

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

        public DataTable GetReport_StocktakingAuditCheckWithUnit(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            string allLocationCode = "";
            allLocationCode = GetLocation(reportParam);

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT08_SP_GET_StocktakingAuditCheckWithUnit", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;
                    cmd.Parameters.Add("@FlagPrint", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@ShowStockTakingID", SqlDbType.VarChar).Value = "0";
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;

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

        public DataTable GetReport_StocktakingAuditCheck(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT06_SP_GET_StocktakingAuditCheck", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    //cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    //cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    //cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    //cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    //cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;

                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = reportParam.LocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;

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

        public DataTable GetReport_DeleteRecordReportByLocation(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            string allLocationCode = "";
            allLocationCode = GetLocation(reportParam);

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT11_SP_GET_DeleteRecordReportByLocation", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return resultTable;
        }

        public DataTable GetReport_DeleteRecordReportBySection(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT08_SP_GET_DeleteRecordReportBySection", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    //cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    //cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    //cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    //cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    //cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;

                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = reportParam.LocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;


                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return resultTable;
        }

        public DataTable GetReport_StocktakingAuditAdjustWithUnit(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            string allLocationCode = "";
            allLocationCode = GetLocation(reportParam);

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT09_SP_GET_StocktakingAuditAdjustWithUnit", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;
                    cmd.Parameters.Add("@Correction", SqlDbType.VarChar).Value = reportParam.CorrectDelete;

                    cmd.CommandTimeout = 100;

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

        public DataTable GetReport_StocktakingAudiAdjust(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT10_SP_GET_StocktakingAudiAdjust", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    //cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    //cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    //cmd.Parameters.Add("@SectionName", SqlDbType.VarChar).Value = allSectionName;
                    //cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    //cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    //cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                    //cmd.Parameters.Add("@Correction", SqlDbType.VarChar).Value = allCorrection;

                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    //cmd.Parameters.Add("@SectionName", SqlDbType.VarChar).Value = reportParam.;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = reportParam.LocationCode;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;
                    cmd.Parameters.Add("@Correction", SqlDbType.VarChar).Value = reportParam.CorrectDelete;

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

        public DataTable GetReport_ControlSheet(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            string allLocationCode = "";
            allLocationCode = GetLocation(reportParam);

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT12_SP_GET_ControlSheet", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    //cmd.Parameters.Add("@SectionName", SqlDbType.VarChar).Value = reportParam.;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return resultTable;
        }

        public DataTable GetReport_UncountedLocation(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            string allLocationCode = "";
            allLocationCode = GetLocation(reportParam);

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT19_SP_GET_UncountedLocation", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return resultTable;
        }

        public DataTable GetReport_UnidentifiedStockItem(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            string allLocationCode = "";
            allLocationCode = GetLocation(reportParam);

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT10_SP_GET_UnidentifiedStockItem", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;

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

        public DataTable GetReport_InventoryControlBySection(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                /*
                if (allStoreType == "4")
                {
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("RPT14_SP_GET_InventoryControlBySectionFreshFood", conn);
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
                        SqlCommand cmd = new SqlCommand("RPT28_SP_GET_InventoryControlBySectionWarehouse", conn);
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
                        SqlCommand cmd = new SqlCommand("RPT29_SP_GET_InventoryControlBySectionBack", conn);
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
                */
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("RPT30_SP_GET_InventoryControlBySectionFront", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        //cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDeprtmentCode;
                        //cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        //cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        //cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        //cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        //cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        //cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        //cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = StoreMode;
                        //cmd.Parameters.Add("@UnitReport", SqlDbType.VarChar).Value = unit;

                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                        cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                        cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                        cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                        cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                        cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                        //cmd.Parameters.Add("@SectionName", SqlDbType.VarChar).Value = reportParam.;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = reportParam.LocationCode;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = reportParam.DiffType;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = reportParam.Unit;
                        //cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = StoreMode;
                        cmd.Parameters.Add("@UnitReport", SqlDbType.VarChar).Value = reportParam.Unit;



                    cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }
                //}
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return resultTable;
        }

        public DataTable GetReport_InventoryControlByLocation(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                /*
                if (allStoreType == "4")
                {
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("RPT15_SP_GET_InventoryControlByLocationFreshFood", conn);
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
                        SqlCommand cmd = new SqlCommand("RPT31_SP_GET_InventoryControlByLocationWarehouse", conn);
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
                        SqlCommand cmd = new SqlCommand("RPT32_SP_GET_InventoryControlByLocationBack", conn);
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
                */
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("RPT33_SP_GET_InventoryControlByLocationFront", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        //cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                        //cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        //cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        //cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        //cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        //cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        //cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        //cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = storeMode;

                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                        cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                        cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                        cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                        cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                        cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = reportParam.LocationCode;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = reportParam.DiffType;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = reportParam.Unit;
                        //cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = StoreMode;


                    cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }
                //}
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return resultTable;
        }

        public DataTable GetReport_InventoryControlByBarcode(DateTime countDate, ReportParameter reportParam,string allDifftype, string allUnit)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                
                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("RPT13_SP_GET_InventoryControlByBarcodeFront", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();

                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        //cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                        //cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = allBarcode;
                        //cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                        //cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                        //cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                        //cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;
                        //cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        //cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        //cmd.Parameters.Add("@StoreMode", SqlDbType.VarChar).Value = storeMode;
                        //cmd.Parameters.Add("@UnitReport", SqlDbType.VarChar).Value = unit;

                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                        cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                        cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                        cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                        cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                        cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = reportParam.LocationCode;
                        cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = allUnit;
                        cmd.Parameters.Add("@UnitReport", SqlDbType.VarChar).Value = reportParam.Unit;
                        cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = reportParam.Barcode;

                    cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return resultTable;
        }

        public DataTable GetReport_InventoryControlByBarcodeFreshFood(DateTime countDate, ReportParameter reportParam, string allDifftype)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {

                    using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("RPT15_SP_GET_InventoryControlByBarcodeFreshFood", conn);
                        SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                        cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                        cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                        cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                        cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                        cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                        cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                        cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = reportParam.LocationCode;
                        cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                        cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;
                        cmd.Parameters.Add("@DiffType", SqlDbType.VarChar).Value = allDifftype;
                        cmd.Parameters.Add("@UnitReport", SqlDbType.VarChar).Value = reportParam.Unit;
                        cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = reportParam.Barcode;

                    cmd.CommandTimeout = 900;

                        dtAdapter.SelectCommand = cmd;
                        dtAdapter.Fill(resultTable);

                        conn.Close();
                    }

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return resultTable;
        }

        public DataSet GetReport_ItemPhysicalCountBySection(DateTime countDate, ReportParameter reportParam)
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
                    SqlCommand cmd = new SqlCommand("RPT17_SP_GET_ItemPhysicalCountBySection", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    //cmd.Parameters.Add("@DepartmentCode", SqlDbType.VarChar).Value = allDepartmentCode;
                    //cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = allSectionCode;
                    //cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    //cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = allBrandCode;
                    //cmd.Parameters.Add("@StoreType", SqlDbType.VarChar).Value = allStoreType;

                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = reportParam.LocationCode;


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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return dsDataSet;
        }

        public DataTable GetReport_ItemPhysicalCountByBarcode(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            //DataSet dsDataSet = new DataSet();
            DataTable resultTable1 = new DataTable();
            //DataTable resultTable2 = new DataTable();
            string allLocationCode = "";
            allLocationCode = GetLocation(reportParam);

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT20_SP_GET_ItemPhysicalCountByBarcode", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;
                    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = reportParam.Barcode;



                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable1);

                    //dsDataSet.Tables[0].TableName = "MasterReport";
                    //dsDataSet.Tables[1].TableName = "MasterSummary";

                conn.Close();
            }

                return resultTable1;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        public DataTable GetReport_GroupSummaryReportByFrontBack(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            string allLocationCode = "";
            allLocationCode = GetLocation(reportParam);

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT16_SP_GET_GroupSummaryReport", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;

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

        public DataTable GetReport_GroupSummaryReportByFreshFoodWarehouse(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            string allLocationCode = "";
            allLocationCode = GetLocation(reportParam);

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT17_SP_GET_GroupSummaryReportByFreshFood", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;

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

        public DataTable GetReport_CountedLocationsReport(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            string allLocationCode = "";
            allLocationCode = GetLocation(reportParam);

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT18_SP_GET_CountedLocationsReport", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;

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

        public DataTable GetReport_NoticeOfStocktakingSatisfactionByFrontBack(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            string allLocationCode = "";
            allLocationCode = GetLocation(reportParam);

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT21_SP_GET_NoticeOfStocktakingSatisfaction", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;

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

        public DataTable GetReport_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse(DateTime countDate, ReportParameter reportParam)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            string allLocationCode = "";
            allLocationCode = GetLocation(reportParam);

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT22_SP_GET_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = reportParam.Plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = reportParam.CountSheet;
                    cmd.Parameters.Add("@MCHL1", SqlDbType.VarChar).Value = reportParam.MCH1;
                    cmd.Parameters.Add("@MCHL2", SqlDbType.VarChar).Value = reportParam.MCH2;
                    cmd.Parameters.Add("@MCHL3", SqlDbType.VarChar).Value = reportParam.MCH3;
                    cmd.Parameters.Add("@MCHL4", SqlDbType.VarChar).Value = reportParam.MCH4;
                    cmd.Parameters.Add("@SectionCode", SqlDbType.VarChar).Value = reportParam.SectionCode;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = reportParam.StoregeLocation;
                    cmd.Parameters.Add("@BrandCode", SqlDbType.VarChar).Value = reportParam.BrandCode;

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

        public DataTable GetReport_ControlSheet(List<string> sectionCode)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                String allSectionCode = string.Join(",", sectionCode);
                dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[SCR01_SP_ADD_MASTERSKU_Front]");
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return resultTable;
        }
    }
}
