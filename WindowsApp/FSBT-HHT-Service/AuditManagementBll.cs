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

namespace FSBT_HHT_BLL
{
    public class AuditManagementBll
    {
        private AuditManagementDAO auditDAO = new AuditManagementDAO();

        public List<EditQtyModel.Response> GetAuditHHTToPC(EditQtyModel.Request searchSection)
        {
            List<EditQtyModel.Response> auditList = auditDAO.GetAuditHHTToPC(searchSection.DepartmentCode,searchSection.SectionCode, searchSection.SectionName, searchSection.LocationFrom, searchSection.LocationTo, searchSection.Barcode, searchSection.SectionType, searchSection.SKUCode);
            return auditList;
        }
        public List<EditQtyModel.ResponseSummary> GetAuditHHTToPCSummary(EditQtyModel.Request searchSection)
        {
            List<EditQtyModel.ResponseSummary> auditList = auditDAO.GetAuditHHTToPCSummary(searchSection.DepartmentCode, searchSection.SectionCode, searchSection.SectionName, searchSection.LocationFrom, searchSection.LocationTo, searchSection.Barcode, searchSection.SectionType, searchSection.SKUCode);
            return auditList;
        }
        public List<EditQtyModel.ResponseSummaryMKCode> GetAuditHHTToPCSummaryMKCode(EditQtyModel.Request searchSection)
        {
            List<EditQtyModel.ResponseSummaryMKCode> auditList = auditDAO.GetAuditHHTToPCSummaryMKCode(searchSection.DepartmentCode, searchSection.SectionCode, searchSection.SectionName, searchSection.LocationFrom, searchSection.LocationTo, searchSection.Barcode, searchSection.SectionType, searchSection.SKUCode);
            return auditList;
        }
        public List<EditQtyModel.ResponseDeleteReport> GetAuditHHTToPCDelete(EditQtyModel.Request searchSection)
        {
            List<EditQtyModel.ResponseDeleteReport> auditList = auditDAO.GetAuditHHTToPCDelete(searchSection.DepartmentCode, searchSection.SectionCode, searchSection.SectionName, searchSection.LocationFrom, searchSection.LocationTo, searchSection.Barcode);
            return auditList;
        }
        public List<EditQtyModel.MasterScanMode> GetMasterScanMode()
        {
            List<EditQtyModel.MasterScanMode> scanModeList = auditDAO.GetMasterScanMode();
            return scanModeList;
        }
        public List<EditQtyModel.MasterUnit> GetMasterUnit()
        {
            List<EditQtyModel.MasterUnit> unitList = auditDAO.GetMasterUnit();
            return unitList;
        }

        public string CheckIsExistLocation(string LocationCode,int scanMode)
        {
            string isExistlocation = auditDAO.CheckIsExistLocation(LocationCode ,scanMode);
            return isExistlocation;
        }
        public bool SaveAuditHHTToPC(List<EditQtyModel.Response> insertList, List<EditQtyModel.Response> updateList, List<EditQtyModel.Response> updateSKUModeList, List<EditQtyModel.Response> deleteList, string username)
        {
            return auditDAO.SaveAuditHHTToPC(insertList, updateList, updateSKUModeList, deleteList, username);
        }

        public EditQtyModel.MasterSKU GetDescriptionInMasterSKU(string barcode, string location, string stocktakingID, DateTime countDate,int scanMode,int unitCode)
        {
            return auditDAO.GetDescriptionInMasterSKU(barcode, location, stocktakingID, countDate, scanMode, unitCode);
        }
        public bool IsExistLocationBarcodeInCountDate(string barcode, string location, DateTime countDate)
        {
            return auditDAO.IsExistLocationBarcodeInCountDate(barcode, location, countDate);
        }

    }
}
