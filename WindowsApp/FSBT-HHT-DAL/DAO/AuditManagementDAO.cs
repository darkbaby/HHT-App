using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_Model;
using System.Globalization;
using System.Reflection;

namespace FSBT_HHT_DAL.DAO
{
    public class AuditManagementDAO
    {
        SystemSettingDAO daoSetting = new SystemSettingDAO();
        private LogErrorDAO logBll = new LogErrorDAO();
        //public List<EditQtyModel.Response> GetAuditHHTToPC(string DepartmentCode, string SectionCode, string SectionName, string locationFrom, string locationTo, string barcode)
        //{
        //    List<EditQtyModel.Response> auditList = new List<EditQtyModel.Response>();
        //    List<EditQtyModel.Response> listSKUIn = new List<EditQtyModel.Response>();
        //    List<EditQtyModel.Response> listSKUEx = new List<EditQtyModel.Response>();
        //    List<EditQtyModel.Response> listSKU = new List<EditQtyModel.Response>();
        //    try
        //    {
        //        Entities dbContext = new Entities();
        //        var settingData = daoSetting.GetSettingData();
        //        var countDate = settingData.CountDate;
        //        if (string.IsNullOrWhiteSpace(locationFrom) && string.IsNullOrWhiteSpace(locationTo))
        //        {
        //            auditList = (from s in dbContext.HHTStocktakings
        //                         where s.Barcode.Contains(barcode)
        //                         && s.CountDate == countDate
        //                         && ((s.Flag != "D" && s.SKUMode == true)
        //                         || (s.Flag != "D" && s.SKUMode == false && s.SKUCode != null))
        //                         orderby s.StocktakingID ascending
        //                         select new EditQtyModel.Response
        //                         {
        //                             StocktakingID = s.StocktakingID,
        //                             LocationCode = s.LocationCode,
        //                             Barcode = s.Barcode,
        //                             Quantity = s.Quantity,
        //                             NewQuantity = s.NewQuantity,
        //                             Flag = s.Flag,
        //                             Description = s.Description,
        //                             UnitCode = s.UnitCode,
        //                             ScanMode = s.ScanMode,
        //                             SKUCode = s.SKUCode,
        //                             InBarCode = s.InBarCode,
        //                             ExBarCode = s.ExBarCode,
        //                             BrandCode = s.BrandCode,
        //                             DepartmentCode=s.DepartmentCode,
        //                             SKUMode = s.SKUMode,
        //                             ChkSKUCode = s.SKUCode
        //                         }).ToList();

        //            listSKUIn = (from s in dbContext.HHTStocktakings
        //                         from sku in dbContext.MasterSKUs.Where(o => s.Barcode == o.InBarcode).DefaultIfEmpty()
        //                         where s.Barcode.Contains(barcode)
        //                         && s.CountDate == countDate
        //                         && s.Flag != "D"
        //                         && s.SKUMode == false
        //                         && s.SKUCode == null
        //                         orderby s.StocktakingID ascending
        //                         select new EditQtyModel.Response
        //                         {
        //                             StocktakingID = s.StocktakingID,
        //                             LocationCode = s.LocationCode,
        //                             Barcode = s.Barcode,
        //                             Quantity = s.Quantity,
        //                             NewQuantity = s.NewQuantity,
        //                             Flag = s.Flag,
        //                             Description = sku.Description,
        //                             UnitCode = s.UnitCode,
        //                             ScanMode = s.ScanMode,
        //                             SKUCode = sku.SKUCode,
        //                             InBarCode = sku.InBarcode,
        //                             ExBarCode = sku.ExBarcode,
        //                             BrandCode = sku.BrandCode,
        //                             DepartmentCode = s.DepartmentCode,
        //                             SKUMode = s.SKUMode
        //                             //}).OrderBy(x => new { x.SKUCode,x.InBarCode,x.ExBarCode }).Where(x => x.SKUCode != null).GroupBy(g => new { g.StocktakingID }).Select(g => g.FirstOrDefault()).ToList();  
        //                         }).OrderBy(x => new { x.SKUCode, x.InBarCode, x.ExBarCode }).Distinct().ToList();

        //            listSKUEx = (from s in dbContext.HHTStocktakings
        //                         from sku in dbContext.MasterSKUs.Where(o => s.Barcode == o.ExBarcode).DefaultIfEmpty()
        //                         where s.Barcode.Contains(barcode)
        //                         && s.CountDate == countDate
        //                         && s.Flag != "D"
        //                         && s.SKUMode == false
        //                         && s.SKUCode == null
        //                         orderby s.StocktakingID ascending
        //                         select new EditQtyModel.Response
        //                         {
        //                             StocktakingID = s.StocktakingID,
        //                             LocationCode = s.LocationCode,
        //                             Barcode = s.Barcode,
        //                             Quantity = s.Quantity,
        //                             NewQuantity = s.NewQuantity,
        //                             Flag = s.Flag,
        //                             Description = sku.Description,
        //                             UnitCode = s.UnitCode,
        //                             ScanMode = s.ScanMode,
        //                             SKUCode = sku.SKUCode,
        //                             InBarCode = sku.InBarcode,
        //                             ExBarCode = sku.ExBarcode,
        //                             BrandCode = sku.BrandCode,
        //                             DepartmentCode = s.DepartmentCode,
        //                             SKUMode = s.SKUMode
        //                             //}).OrderBy(x => new { x.SKUCode,x.InBarCode,x.ExBarCode }).Where(x => x.SKUCode != null).GroupBy(g => new { g.StocktakingID }).Select(g => g.FirstOrDefault()).ToList();       
        //                         }).OrderBy(x => new { x.SKUCode, x.InBarCode, x.ExBarCode }).Distinct().ToList();
        //            if (listSKUIn.Count() > 0)
        //            {
        //                auditList.AddRange(listSKUIn);
        //            }
        //            if (listSKUEx.Count() > 0)
        //            {
        //                auditList.AddRange(listSKUEx);
        //            }
        //            auditList = auditList.OrderByDescending(x => x.SKUCode).GroupBy(g => new { g.StocktakingID }).Select(g => g.FirstOrDefault()).OrderBy(x => x.StocktakingID).ToList();
        //        }
        //        else if (string.IsNullOrWhiteSpace(locationTo))
        //        {
        //            auditList = (from s in dbContext.HHTStocktakings
        //                         where s.LocationCode == locationFrom
        //                              && s.Barcode.Contains(barcode)
        //                              && s.CountDate == countDate
        //                             //&& s.Flag != "D"
        //                             //&& s.SKUCode != null
        //                              && ((s.Flag != "D" && s.SKUMode == true)
        //                              || (s.Flag != "D" && s.SKUMode == false && s.SKUCode != null))
        //                         orderby s.StocktakingID ascending
        //                         select new EditQtyModel.Response
        //                         {
        //                             StocktakingID = s.StocktakingID,
        //                             LocationCode = s.LocationCode,
        //                             Barcode = s.Barcode,
        //                             Quantity = s.Quantity,
        //                             NewQuantity = s.NewQuantity,
        //                             UnitCode = s.UnitCode,
        //                             Flag = s.Flag,
        //                             Description = s.Description,
        //                             ScanMode = s.ScanMode,
        //                             SKUCode = s.SKUCode,
        //                             InBarCode = s.InBarCode,
        //                             ExBarCode = s.ExBarCode,
        //                             BrandCode = s.BrandCode,
        //                             DepartmentCode = s.DepartmentCode,
        //                             ChkSKUCode = s.SKUCode,
        //                             SKUMode = s.SKUMode
        //                         }).ToList();

        //            listSKUIn = (from s in dbContext.HHTStocktakings
        //                         from sku in dbContext.MasterSKUs.Where(o => s.Barcode == o.InBarcode).DefaultIfEmpty()
        //                         where s.LocationCode == locationFrom
        //                               && s.Barcode.Contains(barcode)
        //                               && s.CountDate == countDate
        //                               && s.Flag != "D"
        //                               && s.SKUCode == null
        //                         && s.SKUMode == false
        //                         orderby s.StocktakingID ascending
        //                         select new EditQtyModel.Response
        //                         {
        //                             StocktakingID = s.StocktakingID,
        //                             LocationCode = s.LocationCode,
        //                             Barcode = s.Barcode,
        //                             Quantity = s.Quantity,
        //                             NewQuantity = s.NewQuantity,
        //                             Flag = s.Flag,
        //                             Description = sku.Description,
        //                             UnitCode = s.UnitCode,
        //                             ScanMode = s.ScanMode,
        //                             SKUCode = sku.SKUCode,
        //                             InBarCode = sku.InBarcode,
        //                             ExBarCode = sku.ExBarcode,
        //                             BrandCode = sku.BrandCode,
        //                             DepartmentCode = s.DepartmentCode,
        //                             SKUMode = s.SKUMode
        //                             //}).OrderBy(x => new { x.SKUCode,x.InBarCode,x.ExBarCode }).Where(x => x.SKUCode != null).GroupBy(g => new { g.StocktakingID }).Select(g => g.FirstOrDefault()).ToList();  
        //                         }).OrderBy(x => new { x.SKUCode, x.InBarCode, x.ExBarCode }).Distinct().ToList();

        //            listSKUEx = (from s in dbContext.HHTStocktakings
        //                         from sku in dbContext.MasterSKUs.Where(o => s.Barcode == o.ExBarcode).DefaultIfEmpty()
        //                         where s.LocationCode == locationFrom
        //                               && s.Barcode.Contains(barcode)
        //                               && s.CountDate == countDate
        //                               && s.Flag != "D"
        //                               && s.SKUMode == false
        //                               && s.SKUCode == null
        //                         orderby s.StocktakingID ascending
        //                         select new EditQtyModel.Response
        //                         {
        //                             StocktakingID = s.StocktakingID,
        //                             LocationCode = s.LocationCode,
        //                             Barcode = s.Barcode,
        //                             Quantity = s.Quantity,
        //                             NewQuantity = s.NewQuantity,
        //                             Flag = s.Flag,
        //                             Description = sku.Description,
        //                             UnitCode = s.UnitCode,
        //                             ScanMode = s.ScanMode,
        //                             SKUCode = sku.SKUCode,
        //                             InBarCode = sku.InBarcode,
        //                             ExBarCode = sku.ExBarcode,
        //                             BrandCode = sku.BrandCode,
        //                             DepartmentCode = s.DepartmentCode,
        //                             SKUMode = s.SKUMode
        //                             //}).OrderBy(x => new { x.SKUCode,x.InBarCode,x.ExBarCode }).Where(x => x.SKUCode != null).GroupBy(g => new { g.StocktakingID }).Select(g => g.FirstOrDefault()).ToList();       
        //                         }).OrderBy(x => new { x.SKUCode, x.InBarCode, x.ExBarCode }).Distinct().ToList();
        //            if (listSKUIn.Count() > 0)
        //            {
        //                auditList.AddRange(listSKUIn);
        //            }
        //            if (listSKUEx.Count() > 0)
        //            {
        //                auditList.AddRange(listSKUEx);
        //            }
        //            auditList = auditList.OrderByDescending(x => x.SKUCode).GroupBy(g => new { g.StocktakingID }).Select(g => g.FirstOrDefault()).OrderBy(x => x.StocktakingID).ToList();
        //        }
        //        else if (string.IsNullOrWhiteSpace(locationFrom))
        //        {

        //            auditList = (from s in dbContext.HHTStocktakings
        //                         where s.LocationCode == locationTo
        //                              && s.Barcode.Contains(barcode)
        //                              && s.CountDate == countDate
        //                             //&& s.Flag != "D"
        //                             //&& s.SKUCode != null
        //                              && ((s.Flag != "D" && s.SKUMode == true)
        //                              || (s.Flag != "D" && s.SKUMode == false && s.SKUCode != null))
        //                         orderby s.StocktakingID ascending
        //                         select new EditQtyModel.Response
        //                         {
        //                             StocktakingID = s.StocktakingID,
        //                             LocationCode = s.LocationCode,
        //                             Barcode = s.Barcode,
        //                             Quantity = s.Quantity,
        //                             NewQuantity = s.NewQuantity,
        //                             UnitCode = s.UnitCode,
        //                             Flag = s.Flag,
        //                             Description = s.Description,
        //                             ScanMode = s.ScanMode,
        //                             SKUCode = s.SKUCode,
        //                             InBarCode = s.InBarCode,
        //                             ExBarCode = s.ExBarCode,
        //                             BrandCode = s.BrandCode,
        //                             DepartmentCode = s.DepartmentCode,
        //                             ChkSKUCode = s.SKUCode,
        //                             SKUMode = s.SKUMode
        //                         }).ToList();

        //            listSKUIn = (from s in dbContext.HHTStocktakings
        //                         from sku in dbContext.MasterSKUs.Where(o => s.Barcode == o.InBarcode).DefaultIfEmpty()
        //                         where s.LocationCode == locationTo
        //                             && s.Barcode.Contains(barcode)
        //                             && s.CountDate == countDate
        //                             && s.Flag != "D"
        //                             && s.SKUMode == false
        //                             && s.SKUCode == null
        //                         orderby s.StocktakingID ascending
        //                         select new EditQtyModel.Response
        //                         {
        //                             StocktakingID = s.StocktakingID,
        //                             LocationCode = s.LocationCode,
        //                             Barcode = s.Barcode,
        //                             Quantity = s.Quantity,
        //                             NewQuantity = s.NewQuantity,
        //                             Flag = s.Flag,
        //                             Description = sku.Description,
        //                             UnitCode = s.UnitCode,
        //                             ScanMode = s.ScanMode,
        //                             SKUCode = sku.SKUCode,
        //                             InBarCode = sku.InBarcode,
        //                             ExBarCode = sku.ExBarcode,
        //                             BrandCode = sku.BrandCode,
        //                             DepartmentCode = s.DepartmentCode,
        //                             SKUMode = s.SKUMode
        //                             //}).OrderBy(x => new { x.SKUCode,x.InBarCode,x.ExBarCode }).Where(x => x.SKUCode != null).GroupBy(g => new { g.StocktakingID }).Select(g => g.FirstOrDefault()).ToList();  
        //                         }).OrderBy(x => new { x.SKUCode, x.InBarCode, x.ExBarCode }).Distinct().ToList();

        //            listSKUEx = (from s in dbContext.HHTStocktakings
        //                         from sku in dbContext.MasterSKUs.Where(o => s.Barcode == o.ExBarcode).DefaultIfEmpty()
        //                         where s.LocationCode == locationTo
        //                              && s.Barcode.Contains(barcode)
        //                              && s.CountDate == countDate
        //                              && s.Flag != "D"
        //                              && s.SKUMode == false
        //                              && s.SKUCode == null
        //                         orderby s.StocktakingID ascending
        //                         select new EditQtyModel.Response
        //                         {
        //                             StocktakingID = s.StocktakingID,
        //                             LocationCode = s.LocationCode,
        //                             Barcode = s.Barcode,
        //                             Quantity = s.Quantity,
        //                             NewQuantity = s.NewQuantity,
        //                             Flag = s.Flag,
        //                             Description = sku.Description,
        //                             UnitCode = s.UnitCode,
        //                             ScanMode = s.ScanMode,
        //                             SKUCode = sku.SKUCode,
        //                             InBarCode = sku.InBarcode,
        //                             ExBarCode = sku.ExBarcode,
        //                             BrandCode = sku.BrandCode,
        //                             DepartmentCode = s.DepartmentCode,
        //                             SKUMode = s.SKUMode
        //                             //}).OrderBy(x => new { x.SKUCode,x.InBarCode,x.ExBarCode }).Where(x => x.SKUCode != null).GroupBy(g => new { g.StocktakingID }).Select(g => g.FirstOrDefault()).ToList();       
        //                         }).OrderBy(x => new { x.SKUCode, x.InBarCode, x.ExBarCode }).Distinct().ToList();
        //            if (listSKUIn.Count() > 0)
        //            {
        //                auditList.AddRange(listSKUIn);
        //            }
        //            if (listSKUEx.Count() > 0)
        //            {
        //                auditList.AddRange(listSKUEx);
        //            }
        //            auditList = auditList.OrderByDescending(x => x.SKUCode).GroupBy(g => new { g.StocktakingID }).Select(g => g.FirstOrDefault()).OrderBy(x => x.StocktakingID).ToList();
        //        }
        //        else
        //        {
        //            auditList = (from s in dbContext.HHTStocktakings
        //                         where s.LocationCode.CompareTo(locationFrom) >= 0
        //                              && s.LocationCode.CompareTo(locationTo) <= 0
        //                              && s.CountDate == countDate
        //                              && s.Barcode.Contains(barcode)
        //                             //&& s.Flag != "D"
        //                             //&& s.SKUCode != null
        //                              && ((s.Flag != "D" && s.SKUMode == true)
        //                              || (s.Flag != "D" && s.SKUMode == false && s.SKUCode != null))
        //                         orderby s.StocktakingID ascending
        //                         select new EditQtyModel.Response
        //                         {
        //                             StocktakingID = s.StocktakingID,
        //                             LocationCode = s.LocationCode,
        //                             Barcode = s.Barcode,
        //                             Quantity = s.Quantity,
        //                             NewQuantity = s.NewQuantity,
        //                             UnitCode = s.UnitCode,
        //                             Flag = s.Flag,
        //                             Description = s.Description,
        //                             ScanMode = s.ScanMode,
        //                             SKUCode = s.SKUCode,
        //                             InBarCode = s.InBarCode,
        //                             ExBarCode = s.ExBarCode,
        //                             BrandCode = s.BrandCode,
        //                             DepartmentCode = s.DepartmentCode,
        //                             ChkSKUCode = s.SKUCode,
        //                             SKUMode = s.SKUMode
        //                         }).ToList();

        //            listSKUIn = (from s in dbContext.HHTStocktakings
        //                         from sku in dbContext.MasterSKUs.Where(o => s.Barcode == o.InBarcode).DefaultIfEmpty()
        //                         where s.LocationCode.CompareTo(locationFrom) >= 0
        //                              && s.LocationCode.CompareTo(locationTo) <= 0
        //                              && s.CountDate == countDate
        //                              && s.Barcode.Contains(barcode) && s.Flag != "D"
        //                              && s.Flag != "D"
        //                              && s.SKUMode == false
        //                              && s.SKUCode == null
        //                         orderby s.StocktakingID ascending
        //                         select new EditQtyModel.Response
        //                         {
        //                             StocktakingID = s.StocktakingID,
        //                             LocationCode = s.LocationCode,
        //                             Barcode = s.Barcode,
        //                             Quantity = s.Quantity,
        //                             NewQuantity = s.NewQuantity,
        //                             Flag = s.Flag,
        //                             Description = sku.Description,
        //                             UnitCode = s.UnitCode,
        //                             ScanMode = s.ScanMode,
        //                             SKUCode = sku.SKUCode,
        //                             InBarCode = sku.InBarcode,
        //                             ExBarCode = sku.ExBarcode,
        //                             BrandCode = sku.BrandCode,
        //                             DepartmentCode = s.DepartmentCode,
        //                             SKUMode = s.SKUMode
        //                             //}).OrderBy(x => new { x.SKUCode,x.InBarCode,x.ExBarCode }).Where(x => x.SKUCode != null).GroupBy(g => new { g.StocktakingID }).Select(g => g.FirstOrDefault()).ToList();  
        //                         }).OrderBy(x => new { x.SKUCode, x.InBarCode, x.ExBarCode }).Distinct().ToList();

        //            listSKUEx = (from s in dbContext.HHTStocktakings
        //                         from sku in dbContext.MasterSKUs.Where(o => s.Barcode == o.ExBarcode).DefaultIfEmpty()
        //                         where s.LocationCode.CompareTo(locationFrom) >= 0
        //                              && s.LocationCode.CompareTo(locationTo) <= 0
        //                              && s.CountDate == countDate
        //                              && s.Barcode.Contains(barcode) && s.Flag != "D"
        //                              && s.SKUMode == false
        //                              && s.SKUCode == null
        //                         orderby s.StocktakingID ascending
        //                         select new EditQtyModel.Response
        //                         {
        //                             StocktakingID = s.StocktakingID,
        //                             LocationCode = s.LocationCode,
        //                             Barcode = s.Barcode,
        //                             Quantity = s.Quantity,
        //                             NewQuantity = s.NewQuantity,
        //                             Flag = s.Flag,
        //                             Description = sku.Description,
        //                             UnitCode = s.UnitCode,
        //                             ScanMode = s.ScanMode,
        //                             SKUCode = sku.SKUCode,
        //                             InBarCode = sku.InBarcode,
        //                             ExBarCode = sku.ExBarcode,
        //                             BrandCode = sku.BrandCode,
        //                             DepartmentCode = s.DepartmentCode,
        //                             SKUMode = s.SKUMode
        //                             //}).OrderBy(x => new { x.SKUCode,x.InBarCode,x.ExBarCode }).Where(x => x.SKUCode != null).GroupBy(g => new { g.StocktakingID }).Select(g => g.FirstOrDefault()).ToList();       
        //                         }).OrderBy(x => new { x.SKUCode, x.InBarCode, x.ExBarCode }).Distinct().ToList();
        //            if (listSKUIn.Count() > 0)
        //            {
        //                auditList.AddRange(listSKUIn);
        //            }
        //            if (listSKUEx.Count() > 0)
        //            {
        //                auditList.AddRange(listSKUEx);
        //            }
        //            auditList = auditList.OrderByDescending(x => x.SKUCode).GroupBy(g => new { g.StocktakingID }).Select(g => g.FirstOrDefault()).OrderBy(x => x.StocktakingID).ToList();

        //        }
        //        return auditList;
        //    }
        //    catch (Exception ex)
        //    {
        //        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
        //        return auditList = new List<EditQtyModel.Response>();
        //    }
        //}

        public List<EditQtyModel.Response> GetAuditHHTToPC(EditQtyModel.Request searchSection)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            List<EditQtyModel.Response> resultListEditQty = new List<EditQtyModel.Response>();
            try
            {
                var settingData = daoSetting.GetSettingData();
                var countDate = settingData.CountDate;
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SCR02_SP_GET_HHTStocktaking", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = searchSection.PlantCode == "All" ? "" : searchSection.PlantCode;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = searchSection.CountSheet == "All" ? "" : searchSection.CountSheet;
                    cmd.Parameters.Add("@StorageLocCode", SqlDbType.VarChar).Value = searchSection.StorageLocationCode == "All" ? "" : searchSection.StorageLocationCode;
                    cmd.Parameters.Add("@LocationFrom", SqlDbType.VarChar).Value = searchSection.LocationFrom;
                    cmd.Parameters.Add("@LocationTo", SqlDbType.VarChar).Value = searchSection.LocationTo;
                    cmd.Parameters.Add("@MCHLevel1", SqlDbType.VarChar).Value = searchSection.MCHLevel1 == "All" ? "" : searchSection.MCHLevel1;
                    cmd.Parameters.Add("@MCHLevel2", SqlDbType.VarChar).Value = searchSection.MCHLevel2 == "All" ? "" : searchSection.MCHLevel2;
                    cmd.Parameters.Add("@MCHLevel3", SqlDbType.VarChar).Value = searchSection.MCHLevel3 == "All" ? "" : searchSection.MCHLevel3;
                    cmd.Parameters.Add("@MCHLevel4", SqlDbType.VarChar).Value = searchSection.MCHLevel4 == "All" ? "" : searchSection.MCHLevel4;
                    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = searchSection.Barcode;
                    cmd.Parameters.Add("@SKUCode", SqlDbType.VarChar).Value = searchSection.SKUCode;

                    cmd.CommandTimeout = 1000;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }

                resultListEditQty = convertDtToListEditQty(resultTable);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                resultListEditQty = new List<EditQtyModel.Response>();
            }

            return resultListEditQty;
        }

        public DataTable GetAuditHHTToPCSummary(EditQtyModel.Request searchSection)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();

            try
            {
                var settingData = daoSetting.GetSettingData();
                var countDate = settingData.CountDate;
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SCR02_SP_GET_HHTStocktakingSummary", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = searchSection.PlantCode == "All" ? "" : searchSection.PlantCode;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = searchSection.CountSheet == "All" ? "" : searchSection.CountSheet;
                    cmd.Parameters.Add("@StorageLocCode", SqlDbType.VarChar).Value = searchSection.StorageLocationCode == "All" ? "" : searchSection.StorageLocationCode;
                    cmd.Parameters.Add("@LocationFrom", SqlDbType.VarChar).Value = searchSection.LocationFrom;
                    cmd.Parameters.Add("@LocationTo", SqlDbType.VarChar).Value = searchSection.LocationTo;
                    cmd.Parameters.Add("@MCHLevel1", SqlDbType.VarChar).Value = searchSection.MCHLevel1 == "All" ? "" : searchSection.MCHLevel1;
                    cmd.Parameters.Add("@MCHLevel2", SqlDbType.VarChar).Value = searchSection.MCHLevel2 == "All" ? "" : searchSection.MCHLevel2;
                    cmd.Parameters.Add("@MCHLevel3", SqlDbType.VarChar).Value = searchSection.MCHLevel3 == "All" ? "" : searchSection.MCHLevel3;
                    cmd.Parameters.Add("@MCHLevel4", SqlDbType.VarChar).Value = searchSection.MCHLevel4 == "All" ? "" : searchSection.MCHLevel4;
                    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = searchSection.Barcode;
                    cmd.Parameters.Add("@SKUCode", SqlDbType.VarChar).Value = searchSection.SKUCode;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                resultTable = new DataTable();
            }

            return resultTable;

        }

        //public List<EditQtyModel.ResponseSummary> GetAuditHHTToPCSummary(EditQtyModel.Request searchSection)
        //{
        //    Entities dbContext = new Entities();
        //    DataTable resultTable = new DataTable();
        //    List<EditQtyModel.ResponseSummary> resultListEditQty = new List<EditQtyModel.ResponseSummary>();
        //    try
        //    {
        //        var settingData = daoSetting.GetSettingData();
        //        var countDate = settingData.CountDate;
        //        using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
        //        {
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand("SCR02_SP_GET_HHTStocktakingSummary", conn);
        //            SqlDataAdapter dtAdapter = new SqlDataAdapter();

        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
        //            cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = searchSection.PlantCode == "All" ? "" : searchSection.PlantCode;
        //            cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = searchSection.CountSheet == "All" ? "" : searchSection.CountSheet;
        //            cmd.Parameters.Add("@StorageLocCode", SqlDbType.VarChar).Value = searchSection.StorageLocationCode == "All" ? "" : searchSection.StorageLocationCode;
        //            cmd.Parameters.Add("@LocationFrom", SqlDbType.VarChar).Value = searchSection.LocationFrom;
        //            cmd.Parameters.Add("@LocationTo", SqlDbType.VarChar).Value = searchSection.LocationTo;
        //            cmd.Parameters.Add("@MCHLevel1", SqlDbType.VarChar).Value = searchSection.MCHLevel1 == "All" ? "" : searchSection.MCHLevel1;
        //            cmd.Parameters.Add("@MCHLevel2", SqlDbType.VarChar).Value = searchSection.MCHLevel2 == "All" ? "" : searchSection.MCHLevel2;
        //            cmd.Parameters.Add("@MCHLevel3", SqlDbType.VarChar).Value = searchSection.MCHLevel3 == "All" ? "" : searchSection.MCHLevel3;
        //            cmd.Parameters.Add("@MCHLevel4", SqlDbType.VarChar).Value = searchSection.MCHLevel4 == "All" ? "" : searchSection.MCHLevel4;
        //            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = searchSection.Barcode;
        //            cmd.Parameters.Add("@SKUCode", SqlDbType.VarChar).Value = searchSection.SKUCode;

        //            cmd.CommandTimeout = 900;

        //            dtAdapter.SelectCommand = cmd;
        //            dtAdapter.Fill(resultTable);

        //            conn.Close();
        //        }

        //        resultListEditQty = convertDtToListEditQtySummary(resultTable);
        //    }
        //    catch (Exception ex)
        //    {
        //        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
        //        resultListEditQty = new List<EditQtyModel.ResponseSummary>();
        //    }

        //    return resultListEditQty;

        //}
        public List<EditQtyModel.ResponseSummaryMKCode> GetAuditHHTToPCSummaryMKCode(EditQtyModel.Request searchSection)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            List<EditQtyModel.ResponseSummaryMKCode> resultListEditQty = new List<EditQtyModel.ResponseSummaryMKCode>();
            try
            {
                var settingData = daoSetting.GetSettingData();
                var countDate = settingData.CountDate;
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SCR02_SP_GET_HHTStocktakingSummaryMKCode", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = searchSection.PlantCode == "All" ? "" : searchSection.PlantCode;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = searchSection.CountSheet == "All" ? "" : searchSection.CountSheet;
                    cmd.Parameters.Add("@StorageLocCode", SqlDbType.VarChar).Value = searchSection.StorageLocationCode == "All" ? "" : searchSection.StorageLocationCode ;
                    cmd.Parameters.Add("@LocationFrom", SqlDbType.VarChar).Value = searchSection.LocationFrom;
                    cmd.Parameters.Add("@LocationTo", SqlDbType.VarChar).Value = searchSection.LocationTo;
                    cmd.Parameters.Add("@MCHLevel1", SqlDbType.VarChar).Value = searchSection.MCHLevel1 == "All" ? "" : searchSection.MCHLevel1 ;
                    cmd.Parameters.Add("@MCHLevel2", SqlDbType.VarChar).Value = searchSection.MCHLevel2 == "All" ? "" : searchSection.MCHLevel2 ;
                    cmd.Parameters.Add("@MCHLevel3", SqlDbType.VarChar).Value = searchSection.MCHLevel3 == "All" ? "" : searchSection.MCHLevel3 ;
                    cmd.Parameters.Add("@MCHLevel4", SqlDbType.VarChar).Value = searchSection.MCHLevel4 == "All" ? "" : searchSection.MCHLevel4 ;
                    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = searchSection.Barcode;
                    cmd.Parameters.Add("@SKUCode", SqlDbType.VarChar).Value = searchSection.SKUCode;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }

                resultListEditQty = convertDtToListEditQtySummaryMKCode(resultTable);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                resultListEditQty = new List<EditQtyModel.ResponseSummaryMKCode>();
            }

            return resultListEditQty;

        }
        private List<EditQtyModel.Response> convertDtToListEditQty(DataTable table)
        {
            var editQtyList = new List<EditQtyModel.Response>(table.Rows.Count);
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    var values = row.ItemArray;
                    var remark = string.Empty;
                    if (string.IsNullOrEmpty(values[2].ToString()))
                    {
                        remark += "ไม่มี Location, ";
                    }
                    if (string.IsNullOrEmpty(values[15].ToString()))
                    {
                        remark += "ไม่มี Description, ";
                    }
                    if (remark != string.Empty)
                    {
                        remark = remark.Substring(0, remark.Length - 2);
                    }
                    var editQty = new EditQtyModel.Response()
                    {
                        StocktakingID = values[1].ToString(),
                        LocationCode = values[2].ToString(),
                        Barcode = values[3].ToString(),
                        Quantity = values[4].ToString() == string.Empty ? 0 : (decimal?)values[4],
                        NewQuantity = values[5].ToString() == string.Empty ? 0 : (decimal?)values[5],
                        Flag = values[6].ToString(),
                        UnitCode = (int)values[7],
                        ScanMode = (int)values[8],
                        SKUMode = (bool)values[9],
                        ChkSKUCode = values[10].ToString(),
                        SKUCode = values[12].ToString(),
                        InBarCode = values[13].ToString(),
                        ExBarCode = values[14].ToString(),
                        Description = values[15].ToString(),
                        DepartmentCode = values[16].ToString(),
                        ProductType = values[17].ToString(),
                        MKCode = values[18].ToString(),
                        SerialNumber = values[20].ToString(),
                        ConversionCounter = values[21].ToString(),
                        Plant = values[22].ToString(),
                        StorageLocation = values[23].ToString(),
                        ComputerName = values[24].ToString(),
                        Remark = remark
                    };
                    editQtyList.Add(editQty);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                editQtyList = new List<EditQtyModel.Response>();
            }
            return editQtyList;
        }

        private List<EditQtyModel.ResponseSummary> convertDtToListEditQtySummary(DataTable table)
        {
            var editQtyList = new List<EditQtyModel.ResponseSummary>(table.Rows.Count);
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    var values = row.ItemArray;
                    var editQty = new EditQtyModel.ResponseSummary()
                    {
                        LocationCode = values[0].ToString(),
                        Quantity = values[1] == null ? 0 : (decimal)values[1],
                        NewQuantity = values[2] ==null ? 0 : (decimal)values[2],
                        UnitName = values[3].ToString(),
                    };
                    editQtyList.Add(editQty);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                editQtyList = new List<EditQtyModel.ResponseSummary>();
            }
            return editQtyList;
        }

        private List<EditQtyModel.ResponseSummaryMKCode> convertDtToListEditQtySummaryMKCode(DataTable table)
        {
            var editQtyList = new List<EditQtyModel.ResponseSummaryMKCode>(table.Rows.Count);
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    var values = row.ItemArray;
                    var editQty = new EditQtyModel.ResponseSummaryMKCode()
                    {
                        MKCode = values[0].ToString(),
                        Quantity = values[1] == null ? 0 : (decimal)values[1],
                        NewQuantity = values[2] == null ? 0 : (decimal)values[2],
                        UnitName = values[3].ToString(),
                    };
                    editQtyList.Add(editQty);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                editQtyList = new List<EditQtyModel.ResponseSummaryMKCode>();
            }
            return editQtyList;
        }

        private List<EditQtyModel.ResponseSerialNumberReport> convertDtToListSerialNumber(DataTable table)
        {
            var SerialNumberList = new List<EditQtyModel.ResponseSerialNumberReport>(table.Rows.Count);
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    var values = row.ItemArray;
                    var SerialNumberData = new EditQtyModel.ResponseSerialNumberReport()
                    {
                        SKUCode = values[0].ToString(),
                        Barcode = values[1].ToString(),
                        Description = values[2].ToString(),
                        StorageLocation = values[3].ToString(),
                        Location = values[4].ToString(),
                        Serialnumber = values[5].ToString(),
                        Status = values[6].ToString(),
                        CountSheet = values[7].ToString(),
                        Plant = values[8].ToString(),
                        PlantDesc = values[9].ToString(),
                        CountAllRecord = values[10] == null ? 0 : (Int32)values[10],
                        CountUnidentified = values[11] == null ? 0 : (Int32)values[11],
                        CountNew = values[12] == null ? 0 : (Int32)values[12],
                        MCHLevel1 = values[13].ToString(),
                    };
                    SerialNumberList.Add(SerialNumberData);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                SerialNumberList = new List<EditQtyModel.ResponseSerialNumberReport>();
            }
            return SerialNumberList;
        }

        private List<EditQtyModel.ResponseDeleteReport> convertDtToListDeleteQty(DataTable table)
        {
            var deleteQtyList = new List<EditQtyModel.ResponseDeleteReport>(table.Rows.Count);
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    var values = row.ItemArray;
                    var deleteQty = new EditQtyModel.ResponseDeleteReport()
                    {
                         StocktakingID = values[0].ToString()
                        ,LocationCode = values[1].ToString()
                        ,Barcode = values[2].ToString()
                        ,Quantity = values[3] == null ? 0 : (decimal)values[3]
                        ,NewQuantity = values[4] == null ? 0 : (decimal)values[4]
                        ,UnitName = values[5].ToString()
                        ,Flag = values[6].ToString()
                        ,Description = values[7].ToString()
                        ,Plant = values[8].ToString()
                        ,PlantDesc = values[9].ToString()
                        ,Countsheet = values[10].ToString()
                    };
                    deleteQtyList.Add(deleteQty);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                deleteQtyList = new List<EditQtyModel.ResponseDeleteReport>();
            }
            return deleteQtyList;
        }
        private string validateInList(string compare, List<string> list)
        {
            string errorList = "";

            try
            {
                if (!String.IsNullOrEmpty(compare) && (list.Count() > 0))
                {
                    string[] compareArr = compare.Split(',');
                    List<string> error = compareArr.Except(list).ToList<string>();

                    if (error.Count > 0)
                    {
                        errorList = String.Join(",", error);
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                errorList = "";
            }

            return errorList;
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
       
        /*public List<EditQtyModel.ResponseDeleteReport> GetAuditHHTToPCDelete(EditQtyModel.Request searchSection)
        {
            var settingData = daoSetting.GetSettingData();
            var countDate = settingData.CountDate;

            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            List<EditQtyModel.ResponseDeleteReport> auditList = new List<EditQtyModel.ResponseDeleteReport>();
            List<string> locationList = GetLocationList();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SCR02_SP_RPT_DeleteQtyReport", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = searchSection.PlantCode == "All" ? "" : searchSection.PlantCode;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = searchSection.CountSheet == "All" ? "" : searchSection.CountSheet;
                    cmd.Parameters.Add("@StorageLocCode", SqlDbType.VarChar).Value = searchSection.StorageLocationCode == "All" ? "" : searchSection.StorageLocationCode;
                    cmd.Parameters.Add("@LocationFrom", SqlDbType.VarChar).Value = searchSection.LocationFrom;
                    cmd.Parameters.Add("@LocationTo", SqlDbType.VarChar).Value = searchSection.LocationTo;
                    cmd.Parameters.Add("@MCHLevel1", SqlDbType.VarChar).Value = searchSection.MCHLevel1 == "All" ? "" : searchSection.MCHLevel1;
                    cmd.Parameters.Add("@MCHLevel2", SqlDbType.VarChar).Value = searchSection.MCHLevel2 == "All" ? "" : searchSection.MCHLevel2;
                    cmd.Parameters.Add("@MCHLevel3", SqlDbType.VarChar).Value = searchSection.MCHLevel3 == "All" ? "" : searchSection.MCHLevel3;
                    cmd.Parameters.Add("@MCHLevel4", SqlDbType.VarChar).Value = searchSection.MCHLevel4 == "All" ? "" : searchSection.MCHLevel4;
                    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = searchSection.Barcode;
                    cmd.Parameters.Add("@SKUCode", SqlDbType.VarChar).Value = searchSection.SKUCode;
                    
                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
                auditList = convertDtToListDeleteQty(resultTable);

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                auditList = new List<EditQtyModel.ResponseDeleteReport>();
            }
            return auditList;
        }*/

        public DataTable GetAuditHHTToPCDelete(EditQtyModel.Request searchSection)
        {
            var settingData = daoSetting.GetSettingData();
            var countDate = settingData.CountDate;

            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            List<EditQtyModel.ResponseDeleteReport> auditList = new List<EditQtyModel.ResponseDeleteReport>();
            List<string> locationList = GetLocationList();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SCR02_SP_RPT_DeleteQtyReport", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = searchSection.PlantCode == "All" ? "" : searchSection.PlantCode;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = searchSection.CountSheet == "All" ? "" : searchSection.CountSheet;
                    cmd.Parameters.Add("@StorageLocCode", SqlDbType.VarChar).Value = searchSection.StorageLocationCode == "All" ? "" : searchSection.StorageLocationCode;
                    cmd.Parameters.Add("@LocationFrom", SqlDbType.VarChar).Value = searchSection.LocationFrom;
                    cmd.Parameters.Add("@LocationTo", SqlDbType.VarChar).Value = searchSection.LocationTo;
                    cmd.Parameters.Add("@MCHLevel1", SqlDbType.VarChar).Value = searchSection.MCHLevel1 == "All" ? "" : searchSection.MCHLevel1;
                    cmd.Parameters.Add("@MCHLevel2", SqlDbType.VarChar).Value = searchSection.MCHLevel2 == "All" ? "" : searchSection.MCHLevel2;
                    cmd.Parameters.Add("@MCHLevel3", SqlDbType.VarChar).Value = searchSection.MCHLevel3 == "All" ? "" : searchSection.MCHLevel3;
                    cmd.Parameters.Add("@MCHLevel4", SqlDbType.VarChar).Value = searchSection.MCHLevel4 == "All" ? "" : searchSection.MCHLevel4;
                    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = searchSection.Barcode;
                    cmd.Parameters.Add("@SKUCode", SqlDbType.VarChar).Value = searchSection.SKUCode;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }


            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                resultTable = null;
            }
            return resultTable;
        }

        public List<EditQtyModel.ResponseSerialNumberReport> GetSerialNumberData(EditQtyModel.Request searchSection)
        {
            var settingData = daoSetting.GetSettingData();
            var countDate = settingData.CountDate;

            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            List<EditQtyModel.ResponseSerialNumberReport> serialList = new List<EditQtyModel.ResponseSerialNumberReport>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SCR02_SP_RPT_SerialNumberReport", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = searchSection.PlantCode == "All" ? "" : searchSection.PlantCode;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = searchSection.CountSheet == "All" ? "" : searchSection.CountSheet;
                    cmd.Parameters.Add("@StorageLocCode", SqlDbType.VarChar).Value = searchSection.StorageLocationCode == "All" ? "" : searchSection.StorageLocationCode;
                    cmd.Parameters.Add("@LocationFrom", SqlDbType.VarChar).Value = searchSection.LocationFrom;
                    cmd.Parameters.Add("@LocationTo", SqlDbType.VarChar).Value = searchSection.LocationTo;
                    cmd.Parameters.Add("@MCHLevel1", SqlDbType.VarChar).Value = searchSection.MCHLevel1 == "All" ? "" : searchSection.MCHLevel1;
                    cmd.Parameters.Add("@MCHLevel2", SqlDbType.VarChar).Value = searchSection.MCHLevel2 == "All" ? "" : searchSection.MCHLevel2;
                    cmd.Parameters.Add("@MCHLevel3", SqlDbType.VarChar).Value = searchSection.MCHLevel3 == "All" ? "" : searchSection.MCHLevel3;
                    cmd.Parameters.Add("@MCHLevel4", SqlDbType.VarChar).Value = searchSection.MCHLevel4 == "All" ? "" : searchSection.MCHLevel4;
                    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = searchSection.Barcode;
                    cmd.Parameters.Add("@SKUCode", SqlDbType.VarChar).Value = searchSection.SKUCode;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
                serialList = convertDtToListSerialNumber(resultTable);

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                serialList = new List<EditQtyModel.ResponseSerialNumberReport>();
            }
            return serialList;
        }
        public List<MasterStorageLocation> GetMasterScanMode()
        {
            Entities dbContext = new Entities();
            List<MasterStorageLocation> scanModeList = new List<MasterStorageLocation>();
            try
            {
                scanModeList = (from s in dbContext.MasterStorageLocations
                                orderby s.StorageLocationCode ascending
                                select new MasterStorageLocation
                                {
                                    StorageLocationCode = s.StorageLocationCode,
                                    StorageLocationName = s.StorageLocationName
                                }).ToList();

                return scanModeList;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return scanModeList = new List<MasterStorageLocation>();
            }
        }

        public List<EditQtyModel.MasterUnit> GetMasterUnit()
        {
            Entities dbContext = new Entities();
            List<EditQtyModel.MasterUnit> unitList = new List<EditQtyModel.MasterUnit>();
            try
            {
                unitList = (from s in dbContext.MasterUnits
                            where s.UnitCode != 4
                            orderby s.UnitCode ascending
                            select new EditQtyModel.MasterUnit
                            {
                                UnitCode = s.UnitCode,
                                UnitName = s.UnitName
                            }).ToList();

                return unitList;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return unitList = new List<EditQtyModel.MasterUnit>();
            }
        }

        public string CheckIsExistLocation(string LocationCode, string StorageLocationCode)
        {
            Entities dbContext = new Entities();
            string returnResult = string.Empty;
            List<Section> result = new List<Section>();
            try
            {
                if (LocationCode != "" && StorageLocationCode == "")
                {
                    result = (from l in dbContext.Locations
                              join s in dbContext.Sections on l.SectionCode equals s.SectionCode
                              where l.LocationCode == LocationCode
                              || s.StorageLocationCode == StorageLocationCode
                              select s
                            ).ToList();
                }
                else
                {
                    result = (from l in dbContext.Locations
                              join s in dbContext.Sections on l.SectionCode equals s.SectionCode
                              where l.LocationCode == LocationCode
                              && s.StorageLocationCode == StorageLocationCode
                              select s
                            ).ToList();
                }
                if (result.Count == 0)
                {
                    returnResult = "NOLOCATION";
                }
                else
                {
                    returnResult = result.FirstOrDefault().StorageLocationCode;
                }
                return returnResult;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return returnResult = "ERROR";
            }
        }

        //public EditQtyModel.MasterSKU GetDescriptionInMasterSKU(string barcode, string location, string stocktakingID, DateTime countDate, int unitCode,string flag,string serialnumber)
        //{
        //    var descriptionList = new EditQtyModel.MasterSKU();
        //    try
        //    {
        //        Entities dbContext = new Entities();
        //        var result = string.Empty;
        //        //scanMode = scanMode == 2 ? 1 : scanMode;

        //        //var b = barcode;
        //        //var l = location;
        //        //var s = stocktakingID;
        //        //var c = countDate;
        //        //var m = scanMode;
        //        //var u = unitCode;

        //        descriptionList = (from s in dbContext.MastSAP_SKU
        //                           join b in dbContext.MastSAP_Barcode  on new { p1=s.Material, p2 =s.PIDoc } equals new { p1 = b.Material, p2 = b.PIDoc }
        //                           join c in dbContext.Sections on new { p1 = s.Plant, p2 = s.PIDoc, p3 = s.StorageLocation } equals new { p1 = c.PlantCode, p2=c.CountSheet, p3= c.StorageLocationCode }
        //                           join l in dbContext.Locations    on    new { SectionCode = c.SectionCode, StorageLocationCode = c.StorageLocationCode }
        //                                                            equals new { SectionCode = l.SectionCode, StorageLocationCode = l.StorageLocationCode }
        //                           //where l.LocationCode.Equals(location)
        //                           where b.BarCode == barcode && l.LocationCode.Equals(location) 
        //                           select new EditQtyModel.MasterSKU
        //                           {
        //                                Plant = s.Plant,
        //                                StorageLocation = s.StorageLocation,
                                        
        //                                SKUCode = s.Material,
        //                                InBarCode = b.BarCode,
        //                                ExBarCode = b.BarCode,
        //                                Description = s.MaterialDesc,
        //                                DepartmentCode = "",
        //                                MKCode = "",
        //                                ProductType = "",
        //                                SerialNumber = s.SerialNumber
        //                           }).ToList().FirstOrDefault();

        //        if ((descriptionList != null) &&  (stocktakingID == ""))//new record
        //        {
        //            if (dbContext.HHTStocktakings.Any(x => x.Barcode == barcode && x.LocationCode == location && x.Flag != "D" && x.CountDate == countDate && x.UnitCode == unitCode && x.SerialNumber == serialnumber )) // add by noon
        //            {
        //                descriptionList = new EditQtyModel.MasterSKU();
        //                descriptionList.Result = "DuplicateBarcode";
        //            }
        //        }
        //        else if (flag == "N" && !string.IsNullOrWhiteSpace(stocktakingID)) //old record
        //        {
        //            if (dbContext.HHTStocktakings.Any(x => x.Barcode == barcode && x.LocationCode == location && x.Flag != "D" && x.CountDate == countDate && x.UnitCode == unitCode && x.StocktakingID != stocktakingID && x.SerialNumber == serialnumber)) // add by noon
        //            {
        //                descriptionList = new EditQtyModel.MasterSKU();
        //                descriptionList.Result = "DuplicateBarcode";
        //            }
        //        }

        //        /*
        //        if (scanMode == 4 && (barcode.Length == 5 || barcode.Length == 18))
        //        {
        //            if (barcode.Length == 5)
        //            {
        //                descriptionList = (from sku in dbContext.MasterSKUs
        //                                   join sec in dbContext.Sections on sku.Department equals sec.DepartmentCode
        //                                   join loc in dbContext.Locations on sec.SectionCode equals loc.SectionCode
        //                                   where sku.MKCode == barcode
        //                                   && sku.ScanMode == scanMode
        //                                   && loc.LocationCode == location
        //                                   && sec.ScanMode == scanMode
        //                                   select new EditQtyModel.MasterSKU
        //                                   {
        //                                       SKUCode = sku.SKUCode,
        //                                       InBarCode = sku.InBarcode,
        //                                       ExBarCode = sku.ExBarcode,
        //                                       Description = sku.Description,
        //                                       DepartmentCode = sku.Department,
        //                                       MKCode = sku.MKCode,
        //                                       ProductType = sku.ProductType
        //                                   }).ToList().FirstOrDefault();
        //            }
        //            else if (barcode.Length == 18)
        //            {
        //                barcode = barcode.Substring(1, 5);
        //                descriptionList = (from sku in dbContext.MasterSKUs
        //                                   join sec in dbContext.Sections on sku.Department equals sec.DepartmentCode
        //                                   join loc in dbContext.Locations on sec.SectionCode equals loc.SectionCode
        //                                   where sku.MKCode == barcode
        //                                   && sku.ScanMode == scanMode
        //                                   && loc.LocationCode == location
        //                                   && sec.ScanMode == scanMode
        //                                   select new EditQtyModel.MasterSKU
        //                                   {
        //                                       SKUCode = sku.SKUCode,
        //                                       InBarCode = sku.InBarcode,
        //                                       ExBarCode = sku.ExBarcode,
        //                                       Description = sku.Description,
        //                                       DepartmentCode = sku.Department,
        //                                       MKCode = sku.MKCode,
        //                                       ProductType = sku.ProductType
        //                                   }).ToList().FirstOrDefault();
        //            }


        //            if (descriptionList == null)
        //            {

        //            }
        //            else
        //            {
        //                if (stocktakingID == "")
        //                {
        //                    if (dbContext.HHTStocktakings.Any(x => x.Barcode == barcode && x.LocationCode == location && x.Flag != "D" && x.CountDate == countDate && (x.ScanMode == 1 || x.ScanMode == 2 || (x.ScanMode == 3 && x.UnitCode == unitCode))))
        //                    {
        //                        descriptionList = new EditQtyModel.MasterSKU();
        //                        descriptionList.Result = "DuplicateBarcode";
        //                    }
        //                }
        //            }
        //        }
        //        else if (scanMode == 3 && unitCode == 2)
        //        {
        //            descriptionList = (from p in dbContext.MasterPacks
        //                               join sku in dbContext.MasterSKUs on p.SKUCode equals sku.SKUCode
        //                               join sec in dbContext.Sections on sku.Department equals sec.DepartmentCode
        //                               join loc in dbContext.Locations on sec.SectionCode equals loc.SectionCode
        //                               where
        //                               (sku.InBarcode == barcode
        //                               ||
        //                               sku.ExBarcode == barcode
        //                               ||
        //                               p.Barcode == barcode)
        //                               &&
        //                               sku.ScanMode == scanMode
        //                                   && loc.LocationCode == location
        //                                   && sec.ScanMode == scanMode
        //                               select new EditQtyModel.MasterSKU
        //                               {
        //                                   SKUCode = sku.SKUCode,
        //                                   InBarCode = sku.InBarcode,
        //                                   ExBarCode = sku.ExBarcode,
        //                                   Description = sku.Description,
        //                                   DepartmentCode = sku.Department,
        //                                   MKCode = sku.MKCode,
        //                                   ProductType = sku.ProductType,
        //                                   UnitCode = 2
        //                               }).ToList().FirstOrDefault();

        //            if (descriptionList == null)
        //            {

        //            }
        //            else
        //            {
        //                if (stocktakingID == "")
        //                {
        //                    if (dbContext.HHTStocktakings.Any(x => x.Barcode == barcode && x.LocationCode == location && x.Flag != "D" && x.CountDate == countDate && (x.ScanMode == 1 || x.ScanMode == 2 || (x.ScanMode == 3 && x.UnitCode == unitCode))))
        //                    {
        //                        descriptionList = new EditQtyModel.MasterSKU();
        //                        descriptionList.Result = "DuplicateBarcode";
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {



        //            descriptionList = (from s in dbContext.MasterSKUs
        //                               join sec in dbContext.Sections on s.Department equals sec.DepartmentCode
        //                               join loc in dbContext.Locations on sec.SectionCode equals loc.SectionCode
        //                               where (s.InBarcode == barcode || s.ExBarcode == barcode)
        //                               && s.ScanMode == scanMode
        //                                   && loc.LocationCode == location
        //                                   && (sec.ScanMode == 2 ? 1 : sec.ScanMode) == scanMode
        //                               select new EditQtyModel.MasterSKU
        //                               {
        //                                   SKUCode = s.SKUCode,
        //                                   InBarCode = s.InBarcode,
        //                                   ExBarCode = s.ExBarcode,
        //                                   Description = s.Description,
        //                                   DepartmentCode = s.Department,
        //                                   MKCode = s.MKCode,
        //                                   ProductType = s.ProductType
        //                               }).ToList().FirstOrDefault();

        //            if (descriptionList == null)
        //            {
        //                descriptionList = (from b in dbContext.MasterBarcodes
        //                                   join sku in dbContext.MasterSKUs on b.SKUCode equals sku.SKUCode
        //                                   join sec in dbContext.Sections on sku.Department equals sec.DepartmentCode
        //                                   join loc in dbContext.Locations on sec.SectionCode equals loc.SectionCode
        //                                   where b.ExBarcode == barcode
        //                                   && sku.ScanMode == scanMode
        //                                   && loc.LocationCode == location
        //                                   && (sec.ScanMode == 2 ? 1 : sec.ScanMode) == scanMode
        //                                   select new EditQtyModel.MasterSKU
        //                                   {
        //                                       SKUCode = sku.SKUCode,
        //                                       InBarCode = sku.InBarcode,
        //                                       ExBarCode = sku.ExBarcode,
        //                                       Description = sku.Description,
        //                                       DepartmentCode = sku.Department,
        //                                       MKCode = sku.MKCode,
        //                                       ProductType = sku.ProductType
        //                                   }).ToList().FirstOrDefault();

        //                if (descriptionList == null)
        //                {

        //                    descriptionList = (from p in dbContext.MasterPacks
        //                                       join sku in dbContext.MasterSKUs on p.SKUCode equals sku.SKUCode
        //                                       join sec in dbContext.Sections on sku.Department equals sec.DepartmentCode
        //                                       join loc in dbContext.Locations on sec.SectionCode equals loc.SectionCode
        //                                       where p.Barcode == barcode
        //                                       && sku.ScanMode == scanMode
        //                                       && loc.LocationCode == location
        //                                        && (sec.ScanMode == 2 ? 1 : sec.ScanMode) == scanMode
        //                                       select new EditQtyModel.MasterSKU
        //                                       {
        //                                           SKUCode = sku.SKUCode,
        //                                           InBarCode = sku.InBarcode,
        //                                           ExBarCode = sku.ExBarcode,
        //                                           Description = sku.Description,
        //                                           DepartmentCode = sku.Department,
        //                                           MKCode = sku.MKCode,
        //                                           ProductType = sku.ProductType,
        //                                           UnitCode = 2
        //                                       }).ToList().FirstOrDefault();


        //                    if (descriptionList == null)
        //                    {
        //                        string firstChar = barcode.Substring(0, 1);
        //                        if (barcode.Length >= 13 && firstChar == "2")
        //                        {
        //                            string subSKUCode = barcode.Substring(4, 8);
        //                            descriptionList = (from sku in dbContext.MasterSKUs
        //                                               join sec in dbContext.Sections on sku.Department equals sec.DepartmentCode
        //                                               join loc in dbContext.Locations on sec.SectionCode equals loc.SectionCode
        //                                               where sku.SKUCode == subSKUCode
        //                                               && sku.ScanMode == scanMode
        //                                               && loc.LocationCode == location
        //                                                && (sec.ScanMode == 2 ? 1 : sec.ScanMode) == scanMode
        //                                               select new EditQtyModel.MasterSKU
        //                                               {
        //                                                   SKUCode = sku.SKUCode,
        //                                                   InBarCode = sku.InBarcode,
        //                                                   ExBarCode = sku.ExBarcode,
        //                                                   Description = sku.Description,
        //                                                   DepartmentCode = sku.Department,
        //                                                   MKCode = sku.MKCode,
        //                                                   ProductType = sku.ProductType
        //                                               }).ToList().FirstOrDefault();

        //                            if (descriptionList == null)
        //                            {


        //                            }
        //                            else
        //                            {
        //                                if (stocktakingID == "")
        //                                {
        //                                    if (dbContext.HHTStocktakings.Any(x => x.Barcode == barcode && x.LocationCode == location && x.Flag != "D" && x.CountDate == countDate && (x.ScanMode == 1 || x.ScanMode == 2 || (x.ScanMode == 3 && x.UnitCode == unitCode))))
        //                                    {
        //                                        descriptionList = new EditQtyModel.MasterSKU();
        //                                        descriptionList.Result = "DuplicateBarcode";
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (stocktakingID == "")
        //                            {
        //                                if (dbContext.HHTStocktakings.Any(x => x.Barcode == barcode && x.LocationCode == location && x.Flag != "D" && x.CountDate == countDate && x.ScanMode != 4))
        //                                {
        //                                    descriptionList = new EditQtyModel.MasterSKU();
        //                                    descriptionList.Result = "DuplicateBarcode";
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (stocktakingID == "")
        //                        {
        //                            if (dbContext.HHTStocktakings.Any(x => x.Barcode == barcode && x.LocationCode == location && x.Flag != "D" && x.CountDate == countDate && (x.ScanMode == 1 || x.ScanMode == 2 || (x.ScanMode == 3 && x.UnitCode == unitCode))))
        //                            {
        //                                descriptionList = new EditQtyModel.MasterSKU();
        //                                descriptionList.Result = "DuplicateBarcode";
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (stocktakingID == "")
        //                    {
        //                        if (dbContext.HHTStocktakings.Any(x => x.Barcode == barcode && x.LocationCode == location && x.Flag != "D" && x.CountDate == countDate && (x.ScanMode == 1 || x.ScanMode == 2 || (x.ScanMode == 3 && x.UnitCode == unitCode))))
        //                        {
        //                            descriptionList = new EditQtyModel.MasterSKU();
        //                            descriptionList.Result = "DuplicateBarcode";
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (stocktakingID == "")
        //                {
        //                    if (dbContext.HHTStocktakings.Any(x => x.Barcode == barcode && x.LocationCode == location && x.Flag != "D" && x.CountDate == countDate && (x.ScanMode == 1 || x.ScanMode == 2 || (x.ScanMode == 3 && x.UnitCode == unitCode))))
        //                    {
        //                        descriptionList = new EditQtyModel.MasterSKU();
        //                        descriptionList.Result = "DuplicateBarcode";
        //                    }
        //                }
        //            }
        //        }
        //        */
        //    }
        //    catch (Exception ex)
        //    {
        //        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
        //        descriptionList = new EditQtyModel.MasterSKU();
        //    }
        //    return descriptionList;
        //}

        public EditQtyModel.MasterSKU GetDescriptionInMasterSKU(string barcode, string location, string stocktakingID, DateTime countDate, int unitCode, string flag, string serialnumber)
        {
            var descriptionList = new EditQtyModel.MasterSKU();
            try
            {
                Entities dbContext = new Entities();
                var result = string.Empty;
                //scanMode = scanMode == 2 ? 1 : scanMode;

                //var b = barcode;
                //var l = location;
                //var s = stocktakingID;
                //var c = countDate;
                //var m = scanMode;
                //var u = unitCode;


                if(string.IsNullOrEmpty(serialnumber))
                {
                                   descriptionList = (from s in dbContext.tmp_MasterMapping
                                   where s.InBarCode == barcode && s.Location.Equals(location)                                                                   
                                   select new EditQtyModel.MasterSKU
                                   {
                                       Plant = s.Plant,
                                       StorageLocation = s.StorageLocation,
                                       SKUCode = s.SKUCode,
                                       InBarCode = s.InBarCode,
                                       ExBarCode = s.ExBarCode,
                                       Description = s.Description,
                                       DepartmentCode = s.DepartmentCode,
                                       MKCode = s.MKCode,
                                       ProductType =s.ProductType,
                                       SerialNumber = s.SerialNumber
                                   }).ToList().FirstOrDefault();
                }
                else 
                {
                    descriptionList = ( from s in dbContext.tmp_MasterMapping
                                        where s.InBarCode == barcode && s.Location.Equals(location) && s.SerialNumber.Equals(serialnumber)
                                        select new EditQtyModel.MasterSKU
                                       {
                                           Plant = s.Plant,
                                           StorageLocation = s.StorageLocation,
                                           SKUCode = s.SKUCode,
                                           InBarCode = s.InBarCode,
                                           ExBarCode = s.ExBarCode,
                                           Description = s.Description,
                                           DepartmentCode = s.DepartmentCode,
                                           MKCode = s.MKCode,
                                           ProductType = s.ProductType,
                                           SerialNumber = s.SerialNumber
                                       }).ToList().FirstOrDefault();
                }



                if ((descriptionList != null) && (stocktakingID == ""))//new record
                {
                    if (dbContext.HHTStocktakings.Any(x => x.Barcode == barcode && x.LocationCode == location && x.Flag != "D" && x.CountDate == countDate && x.UnitCode == unitCode && x.SerialNumber == serialnumber)) // add by noon
                    {
                        descriptionList = new EditQtyModel.MasterSKU();
                        descriptionList.Result = "DuplicateBarcode";
                    }
                }
                else if (flag == "N" && !string.IsNullOrWhiteSpace(stocktakingID)) //old record
                {
                    if (dbContext.HHTStocktakings.Any(x => x.Barcode == barcode && x.LocationCode == location && x.Flag != "D" && x.CountDate == countDate && x.UnitCode == unitCode && x.StocktakingID != stocktakingID && x.SerialNumber == serialnumber)) // add by noon
                    {
                        descriptionList = new EditQtyModel.MasterSKU();
                        descriptionList.Result = "DuplicateBarcode";
                    }
                }               

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                descriptionList = new EditQtyModel.MasterSKU();
            }
            return descriptionList;
        }



        public bool IsExistLocationBarcodeInCountDate(string barcode, string location, DateTime countDate)
        {
            Entities dbContext = new Entities();
            bool result = false;
            try
            {
                if (dbContext.HHTStocktakings.Any(x => x.Barcode == barcode && x.LocationCode == location && x.Flag != "D" && x.CountDate == countDate))
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = false;
            }
            return result;
        }

        public bool IsExistSerialNumberInCountDate(string serialNumber, DateTime countDate, string stocktakingID)
        {
            Entities dbContext = new Entities();
            bool result = false;
            try
            {
                if (dbContext.HHTStocktakings.Any(x => x.SerialNumber == serialNumber && x.Flag != "D" && x.CountDate == countDate && x.StocktakingID != stocktakingID))
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = false;
            }
            return result;
        }

        public bool SaveAuditHHTToPC(List<EditQtyModel.Response> insertList, List<EditQtyModel.Response> updateList, List<EditQtyModel.Response> updateSKUModeList, List<EditQtyModel.Response> deleteList, string username)
        {
            Entities dbContext = new Entities();
            using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    SqlCommand commandInsert = PrepareInserteAuditHHTToPCCommand(conn, transaction);
                    SqlCommand commandUpdateInital = PrepareUpdateInitalAuditHHTToPCCommand(conn, transaction);
                    SqlCommand commandUpdateNew = PrepareUpdateNewAuditHHTToPCCommand(conn, transaction);
                    //SqlCommand commandUpdateSKUMode = PrepareUpdateSKUModeAuditHHTToPCCommand(conn, transaction);
                    SqlCommand commandDelete = PrepareDeleteAuditHHTToPCCommand(conn, transaction);
                    DateTime createDate = DateTime.Now;
                    var settingData = daoSetting.GetSettingData();
                    var comID = string.Empty;
                    if (settingData.ComID.Length < 2)
                    {
                        if (settingData.ComID.Length == 1)
                        {
                            comID = "00" + settingData.ComID;
                        }
                        else
                        {
                            comID = "0A0";
                        }
                    }
                    else
                    {
                        comID = "0" + settingData.ComID;
                    }
                    var countDate = settingData.CountDate;

                    //CultureInfo defaulCulture = new CultureInfo("en-US");

                    CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                    newCulture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
                    newCulture.DateTimeFormat.LongDatePattern = "yyyy/MM/dd HH:mm:ss";
                    newCulture.DateTimeFormat.DateSeparator = "/";

                    string dateFormat = createDate.ToString("yyMMdd", newCulture);
                    var maxCode = 0;
                    var maxValue = dbContext.HHTStocktakings.Where(x => x.StocktakingID.Contains(dateFormat + comID)).Max(x => x.StocktakingID);
                    if (maxValue == null)
                    {
                        maxCode = 1;
                    }
                    else
                    {
                        maxCode = Convert.ToInt32(maxValue.Substring(9, 7)) + 1;
                    }
                    try
                    {
                        StringBuilder sql = new StringBuilder();

                        if (insertList.Count > 0)
                        {
                            foreach (var section in insertList)
                            {
                                if (maxCode < 10)
                                {
                                    maxValue = dateFormat + comID + "000000" + maxCode;
                                }
                                else if (maxCode < 100)
                                {
                                    maxValue = dateFormat + comID + "00000" + maxCode;
                                }
                                else if (maxCode < 1000)
                                {
                                    maxValue = dateFormat + comID + "0000" + maxCode;
                                }
                                else if (maxCode < 10000)
                                {
                                    maxValue = dateFormat + comID + "000" + maxCode;
                                }
                                else if (maxCode < 100000)
                                {
                                    maxValue = dateFormat + comID + "00" + maxCode;
                                }
                                else if (maxCode < 1000000)
                                {
                                    maxValue = dateFormat + comID + "0" + maxCode;
                                }
                                else
                                {
                                    maxValue = dateFormat + comID + maxCode;
                                }

                                commandInsert.Parameters["@StocktakingID"].Value = maxValue;

                                commandInsert.Parameters["@Plant"].Value = section.Plant;
                                commandInsert.Parameters["@StorageLocation"].Value = section.StorageLocation;
                                commandInsert.Parameters["@ScanMode"].Value = section.ScanMode;
                                commandInsert.Parameters["@LocationCode"].Value = section.LocationCode;
                                commandInsert.Parameters["@Barcode"].Value = section.Barcode;
                                commandInsert.Parameters["@Quantity"].Value = section.Quantity == 0 ? DBNull.Value : (object)section.Quantity;
                                commandInsert.Parameters["@NewQuantity"].Value = section.NewQuantity == 0 ? DBNull.Value : (object)section.NewQuantity;
                                commandInsert.Parameters["@UnitCode"].Value = section.UnitCode;
                                commandInsert.Parameters["@Flag"].Value = section.Flag;
                                commandInsert.Parameters["@Description"].Value = section.Description;
                                commandInsert.Parameters["@SKUCode"].Value = section.SKUCode;
                                commandInsert.Parameters["@InBarCode"].Value = section.InBarCode;
                                commandInsert.Parameters["@ExBarCode"].Value = section.ExBarCode;
                                commandInsert.Parameters["@SKUMode"].Value = section.SKUMode;
                                commandInsert.Parameters["@HHTName"].Value = section.HHTName;
                                commandInsert.Parameters["@CountDate"].Value = countDate;
                                commandInsert.Parameters["@DepartmentCode"].Value = section.DepartmentCode;
                                commandInsert.Parameters["@MKCode"].Value = section.MKCode;
                                commandInsert.Parameters["@ProductType"].Value = section.ProductType;
                                commandInsert.Parameters["@FlagPrint"].Value = 0;
                                commandInsert.Parameters["@CreateBy"].Value = username;
                                commandInsert.Parameters["@CreateDate"].Value = createDate;
                                commandInsert.Parameters["@UpdateBy"].Value = username;
                                commandInsert.Parameters["@UpdateDate"].Value = createDate;
                                commandInsert.Parameters["@SerialNumber"].Value = section.SerialNumber;
                                commandInsert.Parameters["@ConversionCounter"].Value = section.ConversionCounter;
                                commandInsert.Parameters["@ComputerName"].Value = section.ComputerName;
                               
                                commandInsert.ExecuteNonQuery();

                                maxCode += 1;
                            }
                        }
                        if (updateList.Count > 0)
                        {
                            foreach (var section in updateList)
                            {
                                if (section.Flag == "I" || section.Flag == "E" || section.Flag == "U" || section.Flag == "F")
                                {
                                    commandUpdateInital.Parameters["@StocktakingID"].Value = section.StocktakingID;
                                    commandUpdateInital.Parameters["@NewQuantity"].Value = section.NewQuantity == 0 ? DBNull.Value : (object)section.NewQuantity;
                                    commandUpdateInital.Parameters["@UnitCode"].Value = section.UnitCode;
                                    commandUpdateInital.Parameters["@Flag"].Value = section.Flag;
                                    commandUpdateInital.Parameters["@Description"].Value = section.Description;
                                    commandUpdateInital.Parameters["@SKUCode"].Value = section.SKUCode;//
                                    commandUpdateInital.Parameters["@ExBarCode"].Value = section.ExBarCode;//
                                    commandUpdateInital.Parameters["@UpdateBy"].Value = username;
                                    commandUpdateInital.Parameters["@UpdateDate"].Value = createDate;
                                    commandUpdateInital.Parameters["@SerialNumber"].Value = section.SerialNumber;
                                    commandUpdateInital.Parameters["@ConversionCounter"].Value = section.ConversionCounter;
                                    commandUpdateInital.Parameters["@ComputerName"].Value = section.ComputerName;

                                    commandUpdateInital.ExecuteNonQuery();
                                }
                                else if (section.Flag == "N")//N 
                                {
                                    commandUpdateNew.Parameters["@StocktakingID"].Value = section.StocktakingID;
                                    //commandUpdateNew.Parameters["@ScanMode"].Value = section.ScanMode;
                                    commandUpdateNew.Parameters["@LocationCode"].Value = section.LocationCode;
                                    commandUpdateNew.Parameters["@Barcode"].Value = section.Barcode;
                                    commandUpdateNew.Parameters["@Quantity"].Value = section.Quantity == 0 ? DBNull.Value : (object)section.Quantity;
                                    commandUpdateNew.Parameters["@UnitCode"].Value = section.UnitCode;
                                    commandUpdateNew.Parameters["@Description"].Value = section.Description;
                                    commandUpdateNew.Parameters["@SKUCode"].Value = section.SKUCode;
                                    commandUpdateNew.Parameters["@ExBarCode"].Value = section.ExBarCode;
                                    commandUpdateNew.Parameters["@UpdateBy"].Value = username;
                                    commandUpdateNew.Parameters["@UpdateDate"].Value = createDate;
                                    commandUpdateNew.Parameters["@SerialNumber"].Value = section.SerialNumber;
                                    commandUpdateNew.Parameters["@ConversionCounter"].Value = section.ConversionCounter;
                                    commandUpdateNew.ExecuteNonQuery();
                                }
                            }
                        }
                        if (updateSKUModeList.Count > 0)
                        {
                            //foreach (var section in updateSKUModeList)
                            //{
                            //    commandUpdateSKUMode.Parameters["@StocktakingID"].Value = section.StocktakingID;
                            //    commandUpdateSKUMode.Parameters["@Description"].Value = section.Description;
                            //    commandUpdateSKUMode.Parameters["@SKUCode"].Value = section.SKUCode;
                            //    //commandUpdateSKUMode.Parameters["@InBarCode"].Value = section.InBarCode;
                            //    commandUpdateSKUMode.Parameters["@ExBarCode"].Value = section.ExBarCode;
                            //    //commandUpdateSKUMode.Parameters["@DepartmentCode"].Value = section.DepartmentCode;
                            //    //commandUpdateSKUMode.Parameters["@MKCode"].Value = section.MKCode;
                            //    commandUpdateSKUMode.Parameters["@UpdateBy"].Value = username;
                            //    commandUpdateSKUMode.Parameters["@UpdateDate"].Value = createDate;

                            //    commandUpdateSKUMode.ExecuteNonQuery();
                            //}
                        }
                        if (deleteList.Count > 0)
                        {
                            foreach (var section in deleteList)
                            {
                                commandDelete.Parameters["@StocktakingID"].Value = section.StocktakingID;
                                commandDelete.Parameters["@Flag"].Value = section.Flag;
                                commandDelete.Parameters["@UpdateBy"].Value = username;
                                commandDelete.Parameters["@UpdateDate"].Value = createDate;
                                commandDelete.ExecuteNonQuery();
                            }
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

        public SqlCommand PrepareInserteAuditHHTToPCCommand(SqlConnection conn, SqlTransaction transaction)
        {
            // create SQL command object
            SqlCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;

            // create SQL command text
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO HHTStocktaking (Plant,StorageLocation,StocktakingID,ScanMode,LocationCode,Barcode,Quantity,NewQuantity,UnitCode,Flag,Description,SKUCode,ExBarCode,InBarCode,SKUMode,HHTName,CountDate,DepartmentCode,MKCode,ProductType,FlagPrint,CreateDate,CreateBy,UpdateDate,UpdateBy,SerialNumber,ConversionCounter,ComputerName) ");
            sb.Append("VALUES (@Plant,@StorageLocation,@StocktakingID,@ScanMode, @LocationCode, @Barcode, @Quantity,@NewQuantity,@UnitCode,@Flag , @Description,@SKUCode,@ExBarCode,@InBarCode,@SKUMode, @HHTName,@CountDate,@DepartmentCode,@MKCode,@ProductType,@FlagPrint,@CreateDate,@CreateBy,@UpdateDate,@UpdateBy,@SerialNumber,@ConversionCounter,@ComputerName);");
            command.CommandText = sb.ToString();

            // define parameter type
            command.Parameters.Add("@Plant", SqlDbType.VarChar, 16);
            command.Parameters.Add("@StorageLocation", SqlDbType.VarChar, 16);
            command.Parameters.Add("@StocktakingID", SqlDbType.VarChar, 16);
            command.Parameters.Add("@ScanMode", SqlDbType.Int);
            command.Parameters.Add("@LocationCode", SqlDbType.VarChar, 20);
            command.Parameters.Add("@Barcode", SqlDbType.VarChar, 25);
            command.Parameters.Add("@Quantity", SqlDbType.Decimal);
            command.Parameters.Add("@NewQuantity", SqlDbType.Decimal);
            SqlParameter param = command.Parameters["@Quantity"];
            param.Precision = 18;
            param.Scale = 3;
            SqlParameter param1 = command.Parameters["@NewQuantity"];
            param1.Precision = 18;
            param1.Scale = 3;
            command.Parameters.Add("@UnitCode", SqlDbType.Int);
            command.Parameters.Add("@Flag", SqlDbType.VarChar, 1);
            command.Parameters.Add("@Description", SqlDbType.VarChar, 50);
            command.Parameters.Add("@SKUCode", SqlDbType.VarChar, 25);
            command.Parameters.Add("@ExBarCode", SqlDbType.VarChar, 25);
            command.Parameters.Add("@InBarCode", SqlDbType.VarChar, 25);
            command.Parameters.Add("@SKUMode", SqlDbType.Bit, 1);
            command.Parameters.Add("@HHTName", SqlDbType.VarChar, 20);
            command.Parameters.Add("@CountDate", SqlDbType.Date);
            command.Parameters.Add("@DepartmentCode", SqlDbType.VarChar, 3);
            command.Parameters.Add("@MKCode", SqlDbType.VarChar, 5);
            command.Parameters.Add("@ProductType", SqlDbType.VarChar, 1);
            command.Parameters.Add("@FlagPrint", SqlDbType.Bit, 1);
            command.Parameters.Add("@CreateBy", SqlDbType.VarChar, 20);
            command.Parameters.Add("@CreateDate", SqlDbType.DateTime);
            command.Parameters.Add("@UpdateBy", SqlDbType.VarChar, 20);
            command.Parameters.Add("@UpdateDate", SqlDbType.DateTime);
            command.Parameters.Add("@SerialNumber", SqlDbType.VarChar, 25);
            command.Parameters.Add("@ConversionCounter", SqlDbType.VarChar, 5);
            command.Parameters.Add("@ComputerName", SqlDbType.VarChar, 100);

            command.Prepare();

            return command;
        }

        public SqlCommand PrepareUpdateInitalAuditHHTToPCCommand(SqlConnection conn, SqlTransaction transaction)
        {
            // create SQL command object
            SqlCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;

            // create SQL command text
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE HHTStocktaking ");
            sb.Append(" SET  NewQuantity = @NewQuantity,UnitCode=@UnitCode, Flag = @Flag,Description = @Description, SKUCode=@SKUCode ,ExBarCode=@ExBarCode,UpdateBy = @UpdateBy, UpdateDate = @UpdateDate,SerialNumber = @SerialNumber,ConversionCounter = @ConversionCounter ,ComputerName = @ComputerName ");
            sb.Append(" WHERE StocktakingID = @StocktakingID ");

            command.CommandText = sb.ToString();

            // define parameter type
            command.Parameters.Add("@StocktakingID", SqlDbType.VarChar, 16);
            command.Parameters.Add("@NewQuantity", SqlDbType.Decimal);
            SqlParameter param1 = command.Parameters["@NewQuantity"];
            param1.Precision = 18;
            param1.Scale = 3;
            command.Parameters.Add("@UnitCode", SqlDbType.Int);
            command.Parameters.Add("@Flag", SqlDbType.VarChar, 1);
            command.Parameters.Add("@Description", SqlDbType.VarChar, 50);
            command.Parameters.Add("@SKUCode", SqlDbType.VarChar, 25);
            command.Parameters.Add("@ExBarCode", SqlDbType.VarChar, 25);
            command.Parameters.Add("@UpdateBy", SqlDbType.VarChar, 20);
            command.Parameters.Add("@UpdateDate", SqlDbType.DateTime);
            command.Parameters.Add("@SerialNumber", SqlDbType.VarChar, 25);
            command.Parameters.Add("@ConversionCounter", SqlDbType.VarChar, 5);
            command.Parameters.Add("@ComputerName", SqlDbType.VarChar, 50);

            command.Prepare();

            return command;
        }

        public SqlCommand PrepareUpdateNewAuditHHTToPCCommand(SqlConnection conn, SqlTransaction transaction)
        {
            // create SQL command object
            SqlCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;

            // create SQL command text
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE HHTStocktaking ");
            sb.Append("SET LocationCode = @LocationCode, Barcode = @Barcode, Quantity = @Quantity ,UnitCode=@UnitCode, Description = @Description, SKUCode=@SKUCode ,ExBarCode=@ExBarCode,UpdateBy = @UpdateBy, UpdateDate = @UpdateDate,SerialNumber = @SerialNumber,ConversionCounter = @ConversionCounter ");
            sb.Append("WHERE StocktakingID = @StocktakingID");

            command.CommandText = sb.ToString();

            // define parameter type
            command.Parameters.Add("@StocktakingID", SqlDbType.VarChar, 16);
            command.Parameters.Add("@LocationCode", SqlDbType.VarChar, 20);
            command.Parameters.Add("@Barcode", SqlDbType.VarChar, 25);
            command.Parameters.Add("@Quantity", SqlDbType.Decimal);
            SqlParameter param = command.Parameters["@Quantity"];
            param.Precision = 18;
            param.Scale = 3;
            command.Parameters.Add("@UnitCode", SqlDbType.Int);
            command.Parameters.Add("@Description", SqlDbType.VarChar, 50);
            command.Parameters.Add("@SKUCode", SqlDbType.VarChar, 25);
            command.Parameters.Add("@ExBarCode", SqlDbType.VarChar, 25);
            command.Parameters.Add("@UpdateBy", SqlDbType.VarChar, 20);
            command.Parameters.Add("@UpdateDate", SqlDbType.DateTime);
            command.Parameters.Add("@SerialNumber", SqlDbType.VarChar, 25);
            command.Parameters.Add("@ConversionCounter", SqlDbType.VarChar, 5);

            command.Prepare();

            return command;
        }

        public SqlCommand PrepareUpdateSKUModeAuditHHTToPCCommand(SqlConnection conn, SqlTransaction transaction)
        {
            // create SQL command object
            SqlCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;

            // create SQL command text
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE HHTStocktaking ");
            sb.Append("SET  Description = @Description, SKUCode=@SKUCode ,ExBarCode=@ExBarCode,InBarCode=@InBarCode,DepartmentCode=@DepartmentCode,MKCode=@MKCode,ProductType=@ProductType,UpdateBy = @UpdateBy, UpdateDate = @UpdateDate ");
            sb.Append("WHERE StocktakingID = @StocktakingID");

            command.CommandText = sb.ToString();

            // define parameter type
            command.Parameters.Add("@StocktakingID", SqlDbType.VarChar, 16);

            command.Parameters.Add("@Description", SqlDbType.VarChar, 50);
            command.Parameters.Add("@SKUCode", SqlDbType.VarChar, 25);
            command.Parameters.Add("@ExBarCode", SqlDbType.VarChar, 25);
            command.Parameters.Add("@InBarCode", SqlDbType.VarChar, 25);
            command.Parameters.Add("@DepartmentCode", SqlDbType.VarChar, 3);
            command.Parameters.Add("@MKCode", SqlDbType.VarChar, 5);
            command.Parameters.Add("@ProductType", SqlDbType.VarChar, 1);
            command.Parameters.Add("@UpdateBy", SqlDbType.VarChar, 20);
            command.Parameters.Add("@UpdateDate", SqlDbType.DateTime);


            command.Prepare();

            return command;
        }

        public SqlCommand PrepareDeleteAuditHHTToPCCommand(SqlConnection conn, SqlTransaction transaction)
        {
            // create SQL command object
            SqlCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;

            // create SQL command text
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE HHTStocktaking ");
            sb.Append("SET Flag = @Flag,UpdateBy = @UpdateBy, UpdateDate = @UpdateDate ");
            sb.Append("WHERE StocktakingID = @StocktakingID");

            command.CommandText = sb.ToString();

            // define parameter type
            command.Parameters.Add("@StocktakingID", SqlDbType.VarChar, 16);
            command.Parameters.Add("@Flag", SqlDbType.VarChar, 1);
            command.Parameters.Add("@UpdateBy", SqlDbType.VarChar, 20);
            command.Parameters.Add("@UpdateDate", SqlDbType.DateTime);

            command.Prepare();

            return command;
        }


        //public List<EditQtyModel.ResponseSerialNumberReport> GetSerialNumberData(EditQtyModel.Request searchSection)
        //{
        //    var settingData = daoSetting.GetSettingData();
        //    var countDate = settingData.CountDate;

        //    Entities dbContext = new Entities();
        //    DataTable resultTable = new DataTable();
        //    List<EditQtyModel.ResponseSerialNumberReport> serialList = new List<EditQtyModel.ResponseSerialNumberReport>();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
        //        {
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand("SCR02_SP_RPT_SerialNumberReport", conn);
        //            SqlDataAdapter dtAdapter = new SqlDataAdapter();

        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@CountDate", SqlDbType.Date).Value = countDate;
        //            cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = searchSection.PlantCode == "All" ? "" : searchSection.PlantCode;
        //            cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = searchSection.CountSheet == "All" ? "" : searchSection.CountSheet;
        //            cmd.Parameters.Add("@StorageLocCode", SqlDbType.VarChar).Value = searchSection.StorageLocationCode == "All" ? "" : searchSection.StorageLocationCode;
        //            cmd.Parameters.Add("@LocationFrom", SqlDbType.VarChar).Value = searchSection.LocationFrom;
        //            cmd.Parameters.Add("@LocationTo", SqlDbType.VarChar).Value = searchSection.LocationTo;
        //            cmd.Parameters.Add("@MCHLevel1", SqlDbType.VarChar).Value = searchSection.MCHLevel1 == "All" ? "" : searchSection.MCHLevel1;
        //            cmd.Parameters.Add("@MCHLevel2", SqlDbType.VarChar).Value = searchSection.MCHLevel2 == "All" ? "" : searchSection.MCHLevel2;
        //            cmd.Parameters.Add("@MCHLevel3", SqlDbType.VarChar).Value = searchSection.MCHLevel3 == "All" ? "" : searchSection.MCHLevel3;
        //            cmd.Parameters.Add("@MCHLevel4", SqlDbType.VarChar).Value = searchSection.MCHLevel4 == "All" ? "" : searchSection.MCHLevel4;
        //            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = searchSection.Barcode;
        //            cmd.Parameters.Add("@SKUCode", SqlDbType.VarChar).Value = searchSection.SKUCode;

        //            cmd.CommandTimeout = 900;

        //            dtAdapter.SelectCommand = cmd;
        //            dtAdapter.Fill(resultTable);

        //            conn.Close();
        //        }
        //        serialList = convertDtToListSerialNumber(resultTable);

        //    }
        //    catch (Exception ex)
        //    {
        //        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
        //        serialList = new List<EditQtyModel.ResponseSerialNumberReport>();
        //    }
        //    return serialList;
        //}

        //private List<EditQtyModel.ResponseSerialNumberReport> convertDtToListSerialNumber(DataTable table)
        //{
        //    var SerialNumberList = new List<EditQtyModel.ResponseSerialNumberReport>(table.Rows.Count);
        //    try
        //    {
        //        foreach (DataRow row in table.Rows)
        //        {
        //            var values = row.ItemArray;
        //            var SerialNumberData = new EditQtyModel.ResponseSerialNumberReport()
        //            {
        //                SKUCode = values[0].ToString(),
        //                Barcode = values[1].ToString(),
        //                Description = values[2].ToString(),
        //                StorageLocation = values[3].ToString(),
        //                Location = values[4].ToString(),
        //                Serialnumber = values[5].ToString(),
        //                Status = values[6].ToString(),
        //                CountSheet = values[7].ToString(),
        //                Plant = values[8].ToString(),
        //                PlantDesc = values[9].ToString(),
        //                CountAllRecord = values[10] == null ? 0 : (Int32)values[10],
        //                CountUnidentified = values[11] == null ? 0 : (Int32)values[11],
        //                CountNew = values[12] == null ? 0 : (Int32)values[12],
        //            };
        //            SerialNumberList.Add(SerialNumberData);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
        //        SerialNumberList = new List<EditQtyModel.ResponseSerialNumberReport>();
        //    }
        //    return SerialNumberList;
        //}


        public int UpdateMasterForAddData()
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            int returnValue = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SCR02_SP_Update_MasterForNewRecord", con);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dt);
                    con.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                        returnValue = dr["result"] != null ? Convert.ToInt32(dr["result"]) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                returnValue = 0;
            }
            return returnValue;
        }


        public void GetMasterForMappingData()
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            //int returnValue = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SCR02_SP_GET_MASTERMAPPING", con);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(dt);
                    con.Close();

                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    returnValue = dr["result"] != null ? Convert.ToInt32(dr["result"]) : 0;
                    //}
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            //return returnValue;
        }

    }
}
