using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_DAL;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_DAL.Helper;
using FSBT_HHT_Model;
using BarCode;
using System.Collections;

namespace FSBT_HHT_BLL
{
    public class ReportManagementBll
    {
        private ReportManagementDAO reportDAO = new ReportManagementDAO();
        private LocationManagementDAO locationDAO = new LocationManagementDAO();
        private DownloadMasterDAO masterDAO = new DownloadMasterDAO();

        public DataTable LoadUserReportDataTableByUser(string username)
        {
            DataTable dtReportList = new DataTable();
            List<MasterReport> reportList = new List<MasterReport>();
            reportList = reportDAO.LoadReportByUser(username);
            dtReportList = DbHelper.ToDataTable(reportList);
            return dtReportList;
        }

        public List<MasterReport> LoadUserReportListByUser(string username)
        {
            DataTable dtReportList = new DataTable();
            List<MasterReport> reportList = new List<MasterReport>();
            reportList = reportDAO.LoadReportByUser(username);
            return reportList;
        }

        public List<string> LoadReportConfigByReport(string reportCode)
        {
            List<string> lstreportConfig = new List<string>();
            lstreportConfig = reportDAO.LoadReportConfigComponentByReport(reportCode);
            return lstreportConfig;
        }

        public List<LocationBarcode> GetSearchSection(LocationManagementModel searchSection)
        {
            List<LocationManagementModel> searchSectionList = locationDAO.GetSectionOptionalKey(searchSection.SectionCode, searchSection.SectionName, searchSection.LocationFrom, searchSection.LocationTo, searchSection.SectionType, searchSection.DepartmentCode);
            if (searchSectionList.Count() == 0)
            {
                return null;
            }
            else
            {
                List<LocationBarcode> searchDataList = GenDataToDisplay(searchSectionList, searchSection);
                return searchDataList;
            }
        }

        public string GetReportFileFromReportCode(string reportCode)
        {
            return reportDAO.GetReportFileFromReportCode(reportCode);
        }

        public List<ConfigReport> LoadReportConfig()
        {
            List<ConfigReport> reportConfig = new List<ConfigReport>();
            reportConfig = reportDAO.LoadReportConfig();
            return reportConfig;
        }

        public List<string> GetDepartmentCodeList()
        {
            List<string> departmentList = new List<string>();

            departmentList = reportDAO.GetDepartmentCodeList();

            return departmentList;
        }

        public List<string> GetSectionCodeList()
        {
            List<string> sectionList = new List<string>();

            sectionList = reportDAO.GetSectionCodeList();

            return sectionList;
        }

        public List<string> GetLocationList()
        {
            List<string> locationList = new List<string>();

            locationList = reportDAO.GetLocationList();

            return locationList;
        }

        public List<ReportMasterBrand> GetBrandList()
        {
            List<ReportMasterBrand> brandList = new List<ReportMasterBrand>();

            brandList = reportDAO.GetBrandList();

            return brandList;
        }

        public List<string> GetBarcodeList()
        {
            List<string> barcodeList = new List<string>();

            barcodeList = reportDAO.GetBarcodeList();

            return barcodeList;
        }

        public string GetReportNameByReportCode(string reportCode)
        {
            return reportDAO.GetReportNameByReportCode(reportCode);
        }

        public string GetReportURLByReportCode(string reportCode)
        {
            return reportDAO.GetReportURLByReportCode(reportCode);
        }

        private List<LocationBarcode> GenDataToDisplay(List<LocationManagementModel> resultData, LocationManagementModel searchSection)
        {
            List<LocationBarcode> dataToDisplay = new List<LocationBarcode>();
            foreach (LocationManagementModel data in resultData)
            {

                if (string.IsNullOrWhiteSpace(searchSection.LocationFrom)
                    && string.IsNullOrWhiteSpace(searchSection.LocationTo))
                {
                    int sectionFrom = Int32.Parse(data.LocationFrom);
                    int sectionTo = Int32.Parse(data.LocationTo);
                    for (int i = sectionFrom; i <= sectionTo; i++)
                    {
                        LocationBarcode section = new LocationBarcode();
                        section.SectionCode = data.SectionCode;
                        section.SectionName = data.SectionName;
                        section.Location = i.ToString().PadLeft(5, '0');
                        section.LocationCode = "Section Code " + data.SectionCode + " , Location " + section.Location;

                        dataToDisplay.Add(section);
                    }
                }
                else if (string.IsNullOrWhiteSpace(searchSection.LocationFrom))
                {
                    int sectionFrom = Int32.Parse(data.LocationFrom);
                    int sectionTo = Int32.Parse(data.LocationTo);
                    int searchLocalTo = Int32.Parse(searchSection.LocationTo);
                    for (int i = sectionFrom; i <= sectionTo; i++)
                    {
                        if (i == searchLocalTo)
                        {
                            LocationBarcode section = new LocationBarcode();
                            section.SectionCode = data.SectionCode;
                            section.SectionName = data.SectionName;
                            section.Location = i.ToString().PadLeft(5, '0');
                            section.LocationCode = "Section Code " + data.SectionCode + " , Location " + section.Location;

                            dataToDisplay.Add(section);
                            break;
                        }
                    }
                }
                else if (string.IsNullOrWhiteSpace(searchSection.LocationTo))
                {
                    int sectionFrom = Int32.Parse(data.LocationFrom);
                    int sectionTo = Int32.Parse(data.LocationTo);
                    int searchLocalFrom = Int32.Parse(searchSection.LocationFrom);
                    for (int i = sectionFrom; i <= sectionTo; i++)
                    {
                        if (i == searchLocalFrom)
                        {
                            LocationBarcode section = new LocationBarcode();
                            section.SectionCode = data.SectionCode;
                            section.SectionName = data.SectionName;
                            section.Location = i.ToString().PadLeft(5, '0');
                            section.LocationCode = "Section Code " + data.SectionCode + " , Location " + section.Location;

                            dataToDisplay.Add(section);
                            break;
                        }
                    }
                }
                else
                {
                    int searchLocalFrom = Int32.Parse(searchSection.LocationFrom);
                    int searchLocalTo = Int32.Parse(searchSection.LocationTo);
                    int dataFrom = Int32.Parse(data.LocationFrom);
                    int dataTo = Int32.Parse(data.LocationTo);

                    if (dataFrom < searchLocalFrom) { dataFrom = searchLocalFrom; }

                    for (int i = dataFrom; i <= dataTo; i++)
                    {
                        if (i > searchLocalTo)
                        {
                            break;
                        }

                        LocationBarcode section = new LocationBarcode();
                        section.SectionCode = data.SectionCode;
                        section.SectionName = data.SectionName;
                        section.Location = i.ToString().PadLeft(5, '0');
                        section.LocationCode = "Section Code " + data.SectionCode + " , Location " + section.Location;

                        dataToDisplay.Add(section);
                    }
                }
            }
            return dataToDisplay;
        }

        public List<LocationBarcode> GenDataToBarCode(List<LocationBarcode> locationData)
        {
            List<LocationBarcode> locationBarcode = new List<LocationBarcode>();
            foreach (LocationBarcode data in locationData)
            {
                LocationBarcode barcode = new LocationBarcode();
                barcode.SectionCode = data.SectionCode;
                barcode.SectionName = data.SectionName;
                barcode.LocationCode = data.Location;
                barcode.Location = BarcodeConverter128.StringToBarcode(data.Location);
                locationBarcode.Add(barcode);
            }
            return locationBarcode;
        }

        public DataTable GetReport_SumStockOnHand(string allBrandCode, DateTime countDate, string allDepartmentCode)
        {
            DataTable resultTable = reportDAO.GetReport_SumStockOnHand(allBrandCode, countDate, allDepartmentCode);
            return resultTable;
        }

        public DataTable GetReport_SumStockOnHandWarehouse(string allBrandCode, DateTime countDate, string allDepartmentCode)
        {
            DataTable resultTable = reportDAO.GetReport_SumStockOnHandWarehouse(allBrandCode, countDate, allDepartmentCode);
            return resultTable;
        }

        public DataTable GetReport_SumStockOnHandFreshFood(string allBrandCode, DateTime countDate,string allDepartmentCode)
        {
            DataTable resultTable = reportDAO.GetReport_SumStockOnHandFreshFood(allBrandCode, countDate, allDepartmentCode);
            return resultTable;
        }

        public DataTable LoadReport_SectionLocationByBrandGroup(string SectionCode, string StoreType, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            return reportDAO.GetReport_SectionLocationByBrandGroup(SectionCode, StoreType, allDepartmentCode, allLocationCode, allBrandCode);
        }

        public DataTable LoadReport_StocktakingAuditCheckWithUnit(string LocationCode, string StoreType, DateTime countDate,string allDepartmentCode, string allSectionCode, string allBrandCode)
        {
            return reportDAO.GetReport_StocktakingAuditCheckWithUnit(LocationCode, StoreType, countDate, allDepartmentCode, allSectionCode, allBrandCode);
        }

        public DataTable LoadReport_StocktakingAuditCheck(string LocationCode, string StoreType, DateTime countDate, string allDepartmentCode, string allSectionCode, string allBrandCode)
        {
            return reportDAO.GetReport_StocktakingAuditCheck(LocationCode, StoreType, countDate, allDepartmentCode, allSectionCode, allBrandCode);
        }

        public DataTable LoadReport_DeleteRecordReportByLocation(DateTime countDate, string allDepartmentCode, string allSectionCode, string allLocationCode, string allBrandCode, string allStoreType)
        {
            return reportDAO.GetReport_DeleteRecordReportByLocation(countDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType);
        }

        public DataTable LoadReport_DeleteRecordReportBySection(DateTime countDate,string allDepartmentCode, string allSectionCode, string allLocationCode, string allBrandCode, string allStoreType)
        {
            return reportDAO.GetReport_DeleteRecordReportBySection(countDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType);
        }

        public DataTable LoadReport_StocktakingAuditAdjustWithUnit(string LocationCode, string StoreType, DateTime countDate, string allDepartmentCode, string allSectionCode, string allBrandCode, string Correction)
        {
            return reportDAO.GetReport_StocktakingAuditAdjustWithUnit(LocationCode, StoreType, countDate, allDepartmentCode, allSectionCode, allBrandCode, Correction);
        }

        public DataTable LoadReport_StocktakingAudiAdjust(string LocationCode, string StoreType, string Correction, DateTime countDate, string allDepartmentCode, string allSectionCode, string allBrandCode, string allSectionName)
        {
            return reportDAO.GetReport_StocktakingAudiAdjust(LocationCode, StoreType, Correction, countDate, allDepartmentCode, allSectionCode, allBrandCode,allSectionName);
        }

        public DataTable GetReport_ControlSheet(string allSectionCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode,string allSectionName)
        {
            DataTable resultTable = reportDAO.GetReport_ControlSheet(allSectionCode, allStoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode, allSectionName);
            return resultTable;
        }

        public DataTable GetReport_UncountedLocation(string allSectionCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            DataTable resultTable = reportDAO.GetReport_UncountedLocation(allSectionCode, allStoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode);
            return resultTable;
        }

        public DataTable LoadReport_UnidentifiedStockItem(string LocationCode, string StoreType, DateTime countDate, string allDepartmentCode, string allSectionCode, string allBrandCode)
        {
            return reportDAO.GetReport_UnidentifiedStockItem(LocationCode, StoreType, countDate, allDepartmentCode, allSectionCode, allBrandCode);
        }

        public DataTable LoadReport_InventoryControlBySection(DateTime countDate,string allDeprtmentCode, string allSectionCode,string allLocationCode,string allBrandCode, string allStoreType, string allDifftype, string allUnit,string storeMode, string unit)
        {
            return reportDAO.GetReport_InventoryControlBySection(countDate, allDeprtmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, storeMode,unit);
        }

        public DataTable LoadReport_InventoryControlByLocation(DateTime countDate, string allDepartmentCode, string allSectionCode, string allLocationCode, string allBrandCode, string allStoreType, string allDifftype, string allUnit, string storeMode, string unit)
        {
            return reportDAO.GetReport_InventoryControlByLocation(countDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, storeMode, unit);
        }

        public DataTable LoadReport_InventoryControlByBarcode(DateTime countDate, string allDepartmentCode, string allSectionCode, string allLocationCode, string allBrandCode, string allStoreType, string allDifftype, string allUnit, string Barcode, string storeMode,string unit)
        {
            return reportDAO.GetReport_InventoryControlByBarcode(countDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, Barcode, storeMode, unit);
        }

        public DataSet GetReport_ItemPhysicalCountBySection(string allSectionCode, string allStoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            return reportDAO.GetReport_ItemPhysicalCountBySection(allSectionCode, allStoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode);
        }

        public DataSet GetReport_ItemPhysicalCountByBarcode(string allBarcode, string allStoreType, DateTime countDate, string allDepartmentCode,string allSectionCode, string allLocationCode, string allBrandCode)
        {
            return reportDAO.GetReport_ItemPhysicalCountByBarcode(allBarcode, allStoreType, countDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode);
        }

        public DataTable loadReport_GroupSummaryReportByFrontBack(string SectionCode, string StoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            return reportDAO.GetReport_GroupSummaryReportByFrontBack(SectionCode, StoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode);
        }

        public DataTable loadReport_GroupSummaryReportByFreshFoodWarehouse(string SectionCode, string StoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            return reportDAO.GetReport_GroupSummaryReportByFreshFoodWarehouse(SectionCode, StoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode);
        }
        public DataTable LoadReport_CountedLocationsReport(string SectionCode, string StoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            return reportDAO.GetReport_CountedLocationsReport(SectionCode, StoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode);
        }

        public DataTable LoadReport_NoticeOfStocktakingSatisfactionByFrontBack(string SectionCode, string StoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            return reportDAO.GetReport_NoticeOfStocktakingSatisfactionByFrontBack(SectionCode, StoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode);
        }

        public Hashtable LoadReport_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse(string SectionCode, string StoreType, DateTime countDate, string allDepartmentCode, string allLocationCode, string allBrandCode)
        {
            return reportDAO.GetReport_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse(SectionCode, StoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode);
        }
    }
}
