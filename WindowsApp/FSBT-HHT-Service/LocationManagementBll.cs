using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_DAL;
using FSBT_HHT_Model;
using System.Data;

namespace FSBT_HHT_BLL
{
    public class LocationManagementBll
    {
        private LocationManagementDAO dao = new LocationManagementDAO();

        public List<LocationManagementModel> SearchSection(string sectionCode, string sectionName, string locationForm, string locationTo
                                                            , string sectionType, string deptCode = "")
        {
            List<LocationManagementModel> sectionList = dao.GetSection(sectionCode, sectionName, locationForm, locationTo, sectionType, deptCode);
            return sectionList;
        }

        public List<LocationManagementModel> GetAllSection()
        {
            return dao.GetAllSection();
        }

        public List<LocationManagementModel> GetSection(List<LocationManagementModel> listSection)
        {
            return dao.GetSection(listSection);
        }

        public List<FSBT_HHT_Model.Location> GetAllLocation()
        {
            return dao.GetAllLocation();
        }

        public bool IsSectionCodeExist(string sectionCode, string sectionType)
        {
            return dao.CheckSectionCodeIsExistGroupBySectionType(sectionCode, sectionType);
        }

        public bool IsLocationCodeExist(string locationCode, string sectionCode, string sectionType)
        {
            return dao.CheckLocationIsExist(locationCode, sectionCode, sectionType);
        }

        public bool ClearSection(List<FSBT_HHT_Model.Location> sectionCodeList)
        {
            return dao.DeleteSection(sectionCodeList);
        }

        public bool ClearAllSection()
        {
            return dao.DeleteAllSection();
        }

        public bool SaveSection(List<LocationManagementModel> insertList, List<LocationManagementModel> updateList, List<FSBT_HHT_Model.Location> deleteList, string username)
        {
            return dao.SaveSection(insertList, updateList, deleteList, username);
        }

        public string GetLastestSection()
        {
            return dao.GetLastestSectionCode();
        }

        public List<MasterScanMode> GetListSectionType()
        {
            return dao.GetSectionMasterType();
        }

        public int GetSectionTypeIDBySectionTypeName(string SectionName)
        {
            return dao.GetSectionTypeIDBySectionTypeName(SectionName);
        }

        public List<LocationManagementModel> LoadBrand()
        {
            return dao.GetBrandFromMASTERBRAND();
        }

        //public List<LocationManagementModel> SearchBrand()
        //{
        //    return dao.GetAllBrand();
        //}
    }
}
