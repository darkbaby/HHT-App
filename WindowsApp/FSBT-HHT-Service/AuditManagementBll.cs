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
            List<EditQtyModel.Response> auditList = auditDAO.GetAuditHHTToPC(searchSection);

            return auditList;
        }

        public DataTable GetAuditHHTToPCSummary(EditQtyModel.Request searchSection)
        {
            return auditDAO.GetAuditHHTToPCSummary(searchSection);

        }
        public List<EditQtyModel.ResponseSummaryMKCode> GetAuditHHTToPCSummaryMKCode(EditQtyModel.Request searchSection)
        {
            List<EditQtyModel.ResponseSummaryMKCode> auditList = auditDAO.GetAuditHHTToPCSummaryMKCode(searchSection);
            return auditList;
        }
        //public List<EditQtyModel.ResponseDeleteReport> GetAuditHHTToPCDelete(EditQtyModel.Request searchSection)
        //{
        //    List<EditQtyModel.ResponseDeleteReport> auditList = auditDAO.GetAuditHHTToPCDelete(searchSection);

        //    return auditList;
        //}

        public DataTable GetAuditHHTToPCDelete(EditQtyModel.Request searchSection)
        {
            DataTable auditList = auditDAO.GetAuditHHTToPCDelete(searchSection);

            return auditList;
        }

        public List<EditQtyModel.ResponseSerialNumberReport> GetSerialNumberData(EditQtyModel.Request searchSection)
        {
            List<EditQtyModel.ResponseSerialNumberReport> auditList = auditDAO.GetSerialNumberData(searchSection);

            return auditList;
        }
        public List<MasterStorageLocation> GetMasterScanMode()
        {
            List<MasterStorageLocation> scanModeList = auditDAO.GetMasterScanMode();
            return scanModeList;
        }

        public List<EditQtyModel.MasterUnit> GetMasterUnit()
        {
            List<EditQtyModel.MasterUnit> unitList = auditDAO.GetMasterUnit();
            return unitList;
        }

        public string CheckIsExistLocation(string LocationCode,string StorageLocationCode)
        {
            string isExistlocation = auditDAO.CheckIsExistLocation(LocationCode, StorageLocationCode);
            return isExistlocation;
        }
        public bool SaveAuditHHTToPC(List<EditQtyModel.Response> insertList, List<EditQtyModel.Response> updateList, List<EditQtyModel.Response> updateSKUModeList, List<EditQtyModel.Response> deleteList, string username)
        {
            return auditDAO.SaveAuditHHTToPC(insertList, updateList, updateSKUModeList, deleteList, username);
        }

        public EditQtyModel.MasterSKU GetDescriptionInMasterSKU(string barcode, string location, string stocktakingID, DateTime countDate,  int unitCode,string flag, string serialnumber)
        {
            return auditDAO.GetDescriptionInMasterSKU(barcode, location, stocktakingID, countDate,  unitCode, flag, serialnumber);
        }
        
        public bool IsExistLocationBarcodeInCountDate(string barcode, string location, DateTime countDate)
        {
            return auditDAO.IsExistLocationBarcodeInCountDate(barcode, location, countDate);
        }

        public bool IsExistSerialNumberInCountDate(string serialNumber, DateTime countDate, string stocktakingID)
        {
            return auditDAO.IsExistSerialNumberInCountDate(serialNumber, countDate, stocktakingID);
        }

        //public List<EditQtyModel.ResponseSerialNumberReport> GetSerialNumberData(EditQtyModel.Request searchSection)
        //{
        //    List<EditQtyModel.ResponseSerialNumberReport> auditList = auditDAO.GetSerialNumberData(searchSection);

        //    return auditList;
        //}

        public int UpdateMasterForAddData()
        {
            return auditDAO.UpdateMasterForAddData();
        }

        public void GetMasterForMappingData()
        {
            auditDAO.GetMasterForMappingData();
        }

    }
}
