using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_DAL;
using FSBT_HHT_Model;
using System.Data;
using BarCode;

namespace FSBT_HHT_BLL
{
    public class LocationManagementBll
    {
        private LocationManagementDAO dao = new LocationManagementDAO();

        public List<LocationManagementModel> SearchSection(   string plantCode 
                                                            , string countSheet
                                                            , string storageLocationName
                                                            , string sectionCode
                                                            , string sectionName
                                                            , string locationFrom
                                                            , string locationTo
                                                            , string MCHLevel1
                                                            , string MCHLevel2
                                                            , string MCHLevel3
                                                            , string MCHLevel4 )
        {
            List<LocationManagementModel> sectionList = dao.GetSection(   plantCode
                                                                        , countSheet
                                                                        , storageLocationName
                                                                        , sectionCode
                                                                        , sectionName
                                                                        , locationFrom
                                                                        , locationTo
                                                                        , MCHLevel1
                                                                        , MCHLevel2
                                                                        , MCHLevel3
                                                                        , MCHLevel4 );
            return sectionList;
        }

        public List<LocationManagementModel> SearchSection(ParameterModel searchParameter)
        {
            List<LocationManagementModel> sectionList = dao.GetSection(searchParameter);
            return sectionList;
        }

        public List<LocationManagementModel> SearchReportSection(string plantCode
                                                    , string countSheet
                                                    , string storageLocationName
                                                    , string sectionCode
                                                    , string sectionName
                                                    , string locationFrom
                                                    , string locationTo
                                                    , string MCHLevel1
                                                    , string MCHLevel2
                                                    , string MCHLevel3
                                                    , string MCHLevel4)
        {
            List<LocationManagementModel> sectionList = dao.GetReportSection(plantCode
                                                                        , countSheet
                                                                        , storageLocationName
                                                                        , sectionCode
                                                                        , sectionName
                                                                        , locationFrom
                                                                        , locationTo
                                                                        , MCHLevel1
                                                                        , MCHLevel2
                                                                        , MCHLevel3
                                                                        , MCHLevel4);
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

        public List<LocationModel> GetAllLocation()
        {
            return dao.GetAllLocation();
        }

        public bool IsSectionCodeExistByStoreLocCodeCountsheetPlant(string plant, string countsheet, string storageLocationCode, string sectionCode)
        {
            return dao.CheckSectionCodeIsExistGroupByStorageLocCodeCountsheetPlant(plant, countsheet, storageLocationCode, sectionCode);
        }

        public bool IsSectionCodeExistByStoreLocCode(string sectionCode, string storageLocationCode)
        {
            return dao.CheckSectionCodeIsExistGroupByStorageLocCode(sectionCode, storageLocationCode);
        }

        public bool IsSectionCodeExistByStoreLocName(string sectionCode, string storageLocationName)
        {
            return dao.CheckSectionCodeIsExistGroupByStorageLocationName(sectionCode, storageLocationName);
        }

        public bool IsLocationCodeExistByStoreLocName(string locationCode, string sectionCode, string storageLocationName)
        {
            return dao.CheckLocationIsExistByStoreLocName(locationCode, sectionCode, storageLocationName);
        }

        public bool IsLocationCodeExistByStoreLocCode(string locationCode, string sectionCode, string storageLocationCode)
        {
            return dao.CheckLocationIsExistByStoreLocCode(locationCode, sectionCode, storageLocationCode);
        }

        public bool IsStorageLocationExistInMaster(string storageLocationCode ,string plant , string countsheet)
        {
            return dao.CheckStorageLocationExistInMaster(storageLocationCode,plant,countsheet);
        }


        public bool IsPlantExistInMaster(string plantCode)
        {
            return dao.CheckPlantExistInMaster(plantCode);
        }

        public bool IsCountsheetExistInMaster(string countsheet)
        {
            return dao.CheckCountsheetExistInMaster(countsheet);
        }

        public bool IsBrandExistInMaster(string brandCode)
        {
            return dao.CheckBrandExistInMaster(brandCode);
        }

        public bool IsHaveDataExistInMaster(ref int countSku, ref int countBarcode, ref int countRegularPrice)
        {
            return dao.CheckDataExistInMaster(ref countSku, ref countBarcode, ref countRegularPrice);
        }

        public List<LocationModel> GetSectionLocationBarcode(List<LocationManagementModel> sectionList)
        {
            return dao.GetSectionLocationBarcode(sectionList);
        }


        public bool ClearSection(List<LocationManagementModel> sectionCodeList)
        {
            return dao.DeleteSection(sectionCodeList);
        }

        public bool ClearAllSection()
        {
            return dao.DeleteAllSection();
        }

        public bool SaveSection(List<LocationManagementModel> insertList, List<LocationManagementModel> updateList, List<LocationManagementModel> deleteList, string username)
        {
            return dao.SaveSection(insertList, updateList, deleteList, username);
        }

        public string GetLastestSection()
        {
            return dao.GetLastestSectionCode();
        }

        public List<MasterStorageLocation> GetListMasterStorageLocation()
        {
            return dao.GetMasterStorageLocation();
        }

        public string GetStorageCodeByStorageName(string storageLocationName)
        {
            return dao.GetStorageCodeByStorageName(storageLocationName);
        }

        public List<LocationManagementModel> LoadBrand()
        {
            return dao.GetBrandFromMASTERBRAND();
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

        public List<LocationBarcode> GenDataToBarCode(List<LocationModel> locationData)
        {
            List<LocationBarcode> locationBarcode = new List<LocationBarcode>();
            foreach (LocationModel data in locationData)
            {
                LocationBarcode barcode = new LocationBarcode();
                barcode.SectionCode = data.SectionCode;
                barcode.SectionName = data.SectionName;
                barcode.LocationCode = data.LocationCode;
                barcode.Location = BarcodeConverter128.StringToBarcode(data.LocationCode);
                locationBarcode.Add(barcode);
            }
            return locationBarcode;
        }

        public string GetPlantByCountsheet(string countsheet)
        {
            return dao.GetPlantFromCountSheet(countsheet);
        }

        //public List<LocationManagementModel> SearchBrand()
        //{
        //    return dao.GetAllBrand();
        //}
    }
}
