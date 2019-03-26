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
        private DbHelper dbHelper = new DbHelper();

        public DataTable LoadUserReportDataTableByUser(string username)
        {
            DataTable dtReportList = new DataTable();
            List<MasterReport> reportList = new List<MasterReport>();
            reportList = reportDAO.LoadReportByUser(username);
            dtReportList = dbHelper.ToDataTable(reportList);
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
            List<LocationManagementModel> searchSectionList = locationDAO.GetSectionOptionalKey(searchSection.SectionCode, 
                                                                                                searchSection.SectionName, 
                                                                                                searchSection.LocationFrom, 
                                                                                                searchSection.LocationTo,
                                                                                                searchSection.CountSheet,
                                                                                                searchSection.MCHLevel1,
                                                                                                searchSection.MCHLevel2,
                                                                                                searchSection.MCHLevel3,
                                                                                                searchSection.MCHLevel4,
                                                                                                searchSection.StorageLocationCode,
                                                                                                searchSection.PlantCode
                                                                                                );

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

        public DataTable GetReport_SumStockOnHand(DateTime countDate, ReportParameter reportParam)
        {
            DataTable resultTable = reportDAO.GetReport_SumStockOnHand(countDate, reportParam);
            return resultTable;
        }
        public DataTable GetReport_SumStockOnHandFreshFood(DateTime countDate, ReportParameter reportParam)
        {
            DataTable resultTable = reportDAO.GetReport_SumStockOnHandFreshFood(countDate, reportParam);
            return resultTable;
        }
        public DataTable GetReport_SumStockOnHandWarehouse(DateTime countDate, ReportParameter reportParam)
        {
            DataTable resultTable = reportDAO.GetReport_SumStockOnHandWarehouse(countDate, reportParam);
            return resultTable;
        }

        public DataTable LoadReport_SectionLocationByBrandGroup(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_SectionLocationByBrandGroup(countDate, reportParam);
        }

        public DataTable LoadReport_StocktakingAuditCheckWithUnit(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_StocktakingAuditCheckWithUnit(countDate, reportParam);
        }

        public DataTable LoadReport_StocktakingAuditCheck(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_StocktakingAuditCheck(countDate, reportParam);
        }

        public DataTable LoadReport_DeleteRecordReportByLocation(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_DeleteRecordReportByLocation(countDate, reportParam);
        }

        public DataTable LoadReport_DeleteRecordReportBySection(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_DeleteRecordReportBySection(countDate, reportParam);
        }

        public DataTable LoadReport_StocktakingAuditAdjustWithUnit(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_StocktakingAuditAdjustWithUnit(countDate, reportParam);
        }

        public DataTable LoadReport_StocktakingAudiAdjust(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_StocktakingAudiAdjust(countDate, reportParam);
        }

        public DataTable GetReport_ControlSheet(DateTime countDate, ReportParameter reportParam)
        {
            DataTable resultTable = reportDAO.GetReport_ControlSheet(countDate, reportParam);
            return resultTable;
        }

        public DataTable GetReport_UncountedLocation(DateTime countDate, ReportParameter reportParam)
        {
            DataTable resultTable = reportDAO.GetReport_UncountedLocation(countDate, reportParam);
            return resultTable;
        }

        public DataTable LoadReport_UnidentifiedStockItem(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_UnidentifiedStockItem(countDate, reportParam);
        }

        public DataTable LoadReport_InventoryControlBySection(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_InventoryControlBySection(countDate, reportParam);
        }

        public DataTable LoadReport_InventoryControlByLocation(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_InventoryControlByLocation(countDate, reportParam);
        }

        public DataTable LoadReport_InventoryControlByBarcode(DateTime countDate, ReportParameter reportParam, string allDifftype, string allUnit)
        {
            return reportDAO.GetReport_InventoryControlByBarcode(countDate, reportParam, allDifftype, allUnit);
        }

        public DataTable LoadReport_InventoryControlByBarcodeFreshFood(DateTime countDate, ReportParameter reportParam, string allDifftype)
        {
            return reportDAO.GetReport_InventoryControlByBarcodeFreshFood(countDate, reportParam, allDifftype);
        }
        

        public DataSet GetReport_ItemPhysicalCountBySection(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_ItemPhysicalCountBySection(countDate, reportParam);
        }

        public DataTable GetReport_ItemPhysicalCountByBarcode(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_ItemPhysicalCountByBarcode(countDate, reportParam);
        }

        public DataTable loadReport_GroupSummaryReportByFrontBack(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_GroupSummaryReportByFrontBack(countDate, reportParam);
        }

        public DataTable loadReport_GroupSummaryReportByFreshFoodWarehouse(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_GroupSummaryReportByFreshFoodWarehouse(countDate, reportParam);
        }
        public DataTable LoadReport_CountedLocationsReport(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_CountedLocationsReport(countDate, reportParam);
        }

        public DataTable LoadReport_NoticeOfStocktakingSatisfactionByFrontBack(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_NoticeOfStocktakingSatisfactionByFrontBack(countDate, reportParam);
        }

        public DataTable LoadReport_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse(DateTime countDate, ReportParameter reportParam)
        {
            return reportDAO.GetReport_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse(countDate, reportParam);
        }
    }
}
