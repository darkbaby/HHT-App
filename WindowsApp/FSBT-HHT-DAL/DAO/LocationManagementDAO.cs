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
            
    public class LocationManagementDAO
    {
        private LogErrorDAO logBll = new LogErrorDAO();
        public List<LocationManagementModel> GetReportSection(  string plantCode
                                                        , string countSheet
                                                        , string storageLocation
                                                        , string sectionCode
                                                        , string sectionName
                                                        , string locationFrom
                                                        , string locationTo
                                                        , string MCHLevel1
                                                        , string MCHLevel2
                                                        , string MCHLevel3
                                                        , string MCHLevel4)
        {
            Entities dbContext = new Entities();

            List<LocationManagementModel> sectionList = new List<LocationManagementModel>();

            string LocationCode = "";
            DataTable resultTable = new DataTable();
            List<string> locationList = GetLocationList();

            if (!String.IsNullOrEmpty(locationFrom) || !String.IsNullOrEmpty(locationTo))
            {
                string errorExist = validateInList(locationFrom + "," + locationTo, locationList);

                if (errorExist.Length == 0)
                {
                    int locFrom = Convert.ToInt32(locationFrom);
                    int locTo = Convert.ToInt32(locationTo);

                    if ((locTo > locFrom) || (locTo == locFrom))
                    {
                        LocationCode = locationFrom + "-" + locationTo;
                    }
                    else
                    {
                        LocationCode = "";
                    }
                }
                else
                {
                    LocationCode = "";
                }
            }

            string allLocationCode = "";
            string[] locationCode = LocationCode.Split('-');
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
                allLocationCode = LocationCode;
            }

            if (plantCode == "All")
            {
                plantCode = "";
            }
            if (countSheet == "All")
            {
                countSheet = "";
            }
            if (storageLocation == "All")
            {
                storageLocation = "";
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RPT07_SP_GET_SectionLocationByBrandGroup", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = plantCode;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = countSheet;
                    cmd.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = storageLocation;
                    cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar).Value = allLocationCode;
                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
                sectionList = convertDtToListSection(resultTable);           
                
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                sectionList = new List<LocationManagementModel>();

            }
            return sectionList;

        }

        public List<LocationManagementModel> GetSection(string plantCode
                                                , string countSheet
                                                , string storageLocation
                                                , string sectionCode
                                                , string sectionName
                                                , string locationFrom
                                                , string locationTo
                                                , string MCHLevel1
                                                , string MCHLevel2
                                                , string MCHLevel3
                                                , string MCHLevel4)
        {
            Entities dbContext = new Entities();

            List<LocationManagementModel> sectionList = new List<LocationManagementModel>();



            try
            {
                sectionList = (from s in dbContext.Sections
                               orderby s.LocationFrom ascending
                               select new LocationManagementModel
                               {
                                   PlantCode = s.PlantCode,
                                   CountSheet = s.CountSheet,
                                   StorageLocationCode = s.StorageLocationCode,
                                   SectionCode = s.SectionCode,
                                   SectionName = s.SectionName,
                                   LocationFrom = s.LocationFrom,
                                   LocationTo = s.LocationTo,
                                   BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                   OriginalPlantCode = s.PlantCode,
                                   OriginalCountSheet = s.CountSheet,
                                   OriginalStorageLocCode = s.StorageLocationCode,
                                   OriginalSectionCode = s.SectionCode,
                                   OriginalSectionName = s.SectionName
                               }).ToList();

                #region sectionCode
                if (!(string.IsNullOrWhiteSpace(sectionCode)))
                {
                    sectionList = (from sl in sectionList
                                   join s in dbContext.Sections
                                   on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                   equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                   //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                   //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                   // join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                   where s.SectionCode.Contains(sectionCode)
                                   orderby s.LocationFrom ascending
                                   select new LocationManagementModel
                                   {
                                       //OriginalSectionCode = s.SectionCode,
                                       //SectionCode = s.SectionCode,
                                       //StorageLocationCode = s.StorageLocationCode,
                                       ////StorageLocationName = m.StorageLocationName,
                                       //SectionName = s.SectionName,
                                       //LocationFrom = s.LocationFrom,
                                       //LocationTo = s.LocationTo,
                                       //BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                       //MCHLevel1 = s.MCHLevel1,
                                       //MCHLevel2 = s.MCHLevel2,
                                       //MCHLevel3 = s.MCHLevel3,
                                       //MCHLevel4 = s.MCHLevel4,
                                       //CountSheet = s.CountSheet,
                                       //PlantCode = s.PlantCode

                                       PlantCode = s.PlantCode,
                                       CountSheet = s.CountSheet,
                                       StorageLocationCode = s.StorageLocationCode,
                                       SectionCode = s.SectionCode,
                                       SectionName = s.SectionName,
                                       LocationFrom = s.LocationFrom,
                                       LocationTo = s.LocationTo,
                                       BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                       OriginalPlantCode = s.PlantCode,
                                       OriginalCountSheet = s.CountSheet,
                                       OriginalStorageLocCode = s.StorageLocationCode,
                                       OriginalSectionCode = s.SectionCode,
                                       OriginalSectionName = s.SectionName

                                   }).ToList();
                }
                #endregion

                #region sectionName
                if (!(string.IsNullOrWhiteSpace(sectionName)))
                {
                    sectionList = (from sl in sectionList
                                   join s in dbContext.Sections
                                   on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                   equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                   //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                   //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                   //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                   where s.SectionName.ToUpper().Contains(sectionName.ToUpper())
                                   orderby s.LocationFrom ascending
                                   select new LocationManagementModel
                                   {
                                       //OriginalSectionCode = s.SectionCode,
                                       //SectionCode = s.SectionCode,
                                       //StorageLocationCode = s.StorageLocationCode,
                                       ////StorageLocationName = m.StorageLocationName,
                                       //SectionName = s.SectionName,
                                       //LocationFrom = s.LocationFrom,
                                       //LocationTo = s.LocationTo,
                                       //BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                       //MCHLevel1 = s.MCHLevel1,
                                       //MCHLevel2 = s.MCHLevel2,
                                       //MCHLevel3 = s.MCHLevel3,
                                       //MCHLevel4 = s.MCHLevel4,
                                       //CountSheet = s.CountSheet,
                                       //PlantCode = s.PlantCode

                                       PlantCode = s.PlantCode,
                                       CountSheet = s.CountSheet,
                                       StorageLocationCode = s.StorageLocationCode,
                                       SectionCode = s.SectionCode,
                                       SectionName = s.SectionName,
                                       LocationFrom = s.LocationFrom,
                                       LocationTo = s.LocationTo,
                                       BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                       OriginalPlantCode = s.PlantCode,
                                       OriginalCountSheet = s.CountSheet,
                                       OriginalStorageLocCode = s.StorageLocationCode,
                                       OriginalSectionCode = s.SectionCode,
                                       OriginalSectionName = s.SectionName
                                   }).ToList();
                }
                #endregion

                #region locationFrom
                if (!(string.IsNullOrWhiteSpace(locationFrom)))
                {
                    if (string.IsNullOrWhiteSpace(locationTo))
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       join l in dbContext.Locations
                                       on new { SectionCode = s.SectionCode, SectionType = s.StorageLocationCode }
                                       equals new { SectionCode = l.SectionCode, SectionType = l.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where l.LocationCode.Equals(locationFrom)
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           //OriginalSectionCode = s.SectionCode,
                                           //SectionCode = s.SectionCode,
                                           //StorageLocationCode = s.StorageLocationCode,
                                           ////StorageLocationName = m.StorageLocationName,
                                           //SectionName = s.SectionName,
                                           //LocationFrom = s.LocationFrom,
                                           //LocationTo = s.LocationTo,
                                           //BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                           //MCHLevel1 = s.MCHLevel1,
                                           //MCHLevel2 = s.MCHLevel2,
                                           //MCHLevel3 = s.MCHLevel3,
                                           //MCHLevel4 = s.MCHLevel4,
                                           //CountSheet = s.CountSheet,
                                           //PlantCode = s.PlantCode

                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                    else
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where int.Parse(s.LocationTo) >= int.Parse(locationFrom)
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           //OriginalSectionCode = s.SectionCode,
                                           //SectionCode = s.SectionCode,
                                           //StorageLocationCode = s.StorageLocationCode,
                                           ////StorageLocationName = m.StorageLocationName,
                                           //SectionName = s.SectionName,
                                           //LocationFrom = s.LocationFrom,
                                           //LocationTo = s.LocationTo,
                                           //BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                           //MCHLevel1 = s.MCHLevel1,
                                           //MCHLevel2 = s.MCHLevel2,
                                           //MCHLevel3 = s.MCHLevel3,
                                           //MCHLevel4 = s.MCHLevel4,
                                           //CountSheet = s.CountSheet,
                                           //PlantCode = s.PlantCode

                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                #region locationTo
                if (!(string.IsNullOrWhiteSpace(locationTo)))
                {
                    if (string.IsNullOrWhiteSpace(locationFrom))
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       join l in dbContext.Locations
                                       on new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       equals new { SectionCode = l.SectionCode, StorageLocationCode = l.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where l.LocationCode.Equals(locationTo)
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                    else
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       where int.Parse(s.LocationFrom) <= int.Parse(locationTo)
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                #region MCHLevel1
                if (!string.IsNullOrEmpty(MCHLevel1))
                {
                    if (MCHLevel1.ToUpper() != "ALL")
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where s.MCHLevel1.ToUpper().Contains(MCHLevel1.ToUpper())
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                #region MCHLevel2
                if (!string.IsNullOrEmpty(MCHLevel2))
                {
                    if (MCHLevel2.ToUpper() != "ALL")
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       ////on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where s.MCHLevel2.ToUpper().Contains(MCHLevel2.ToUpper())
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                #region MCHLevel3
                if (!string.IsNullOrEmpty(MCHLevel3))
                {
                    if (MCHLevel3.ToUpper() != "ALL")
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where s.MCHLevel3.ToUpper().Contains(MCHLevel3.ToUpper())
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                #region MCHLevel4
                if (!string.IsNullOrEmpty(MCHLevel4))
                {
                    if (MCHLevel4.ToUpper() != "ALL")
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where s.MCHLevel4.ToUpper().Contains(MCHLevel4.ToUpper())
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                #region storageLocation
                if (!string.IsNullOrEmpty(storageLocation))
                {
                    if (storageLocation.ToUpper() != "ALL")
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where s.StorageLocationCode.ToUpper().Contains(storageLocation.ToUpper())
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           //OriginalSectionCode = s.SectionCode,
                                           //SectionCode = s.SectionCode,
                                           //StorageLocationCode = s.StorageLocationCode,
                                           ////StorageLocationName = m.StorageLocationName,
                                           //SectionName = s.SectionName,
                                           //LocationFrom = s.LocationFrom,
                                           //LocationTo = s.LocationTo,
                                           //BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                           //MCHLevel1 = s.MCHLevel1,
                                           //MCHLevel2 = s.MCHLevel2,
                                           //MCHLevel3 = s.MCHLevel3,
                                           //MCHLevel4 = s.MCHLevel4,
                                           //CountSheet = s.CountSheet,
                                           //PlantCode = s.PlantCode

                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                #region countSheet
                if (!string.IsNullOrEmpty(countSheet))
                {
                    if (countSheet.ToUpper() != "ALL")
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where s.CountSheet.ToUpper().Contains(countSheet.ToUpper())
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           //OriginalSectionCode = s.SectionCode,
                                           //SectionCode = s.SectionCode,
                                           //StorageLocationCode = s.StorageLocationCode,
                                           ////StorageLocationName = m.StorageLocationName,
                                           //SectionName = s.SectionName,
                                           //LocationFrom = s.LocationFrom,
                                           //LocationTo = s.LocationTo,
                                           //BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                           //MCHLevel1 = s.MCHLevel1,
                                           //MCHLevel2 = s.MCHLevel2,
                                           //MCHLevel3 = s.MCHLevel3,
                                           //MCHLevel4 = s.MCHLevel4,
                                           //CountSheet = s.CountSheet,
                                           //PlantCode = s.PlantCode

                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                #region Plant
                if (!string.IsNullOrEmpty(plantCode))
                {
                    if (plantCode.ToUpper() != "ALL")
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where s.PlantCode.ToUpper().Equals(plantCode.ToUpper())
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           //OriginalSectionCode = s.SectionCode,
                                           //SectionCode = s.SectionCode,
                                           //StorageLocationCode = s.StorageLocationCode,
                                           ////StorageLocationName = m.StorageLocationName,
                                           //SectionName = s.SectionName,
                                           //LocationFrom = s.LocationFrom,
                                           //LocationTo = s.LocationTo,
                                           //BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                           //MCHLevel1 = s.MCHLevel1,
                                           //MCHLevel2 = s.MCHLevel2,
                                           //MCHLevel3 = s.MCHLevel3,
                                           //MCHLevel4 = s.MCHLevel4,
                                           //CountSheet = s.CountSheet,
                                           //PlantCode = s.PlantCode

                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                return sectionList;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return sectionList;
            }
        }

        public List<LocationManagementModel> GetSection(ParameterModel searchParameter)
        {
            string plant = searchParameter.Plant;
            string countsheet = searchParameter.Countsheet;
            string storageLocation = searchParameter.StorageLocationCode;
            string locationFrom = searchParameter.LocationFromCode;
            string locationTo = searchParameter.LocationToCode;
            
            Entities dbContext = new Entities();

            List<LocationManagementModel> sectionList = new List<LocationManagementModel>();

            DataTable resultTable = new DataTable();

            if (plant == "All")
            {
                plant = "";
            }

            if (countsheet == "All")
            {
                countsheet = "";
            }
            if (storageLocation == "All")
            {
                storageLocation = "";
            }
            else
            {
                storageLocation = storageLocation.Substring(0, 4);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SCR03_SP_GET_SECTIONLOCATION", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.VarChar).Value = plant;
                    cmd.Parameters.Add("@CountSheet", SqlDbType.VarChar).Value = countsheet;
                    cmd.Parameters.Add("@StorageLocCode", SqlDbType.VarChar).Value = storageLocation;
                    cmd.Parameters.Add("@LocationFrom", SqlDbType.VarChar).Value = locationFrom;
                    cmd.Parameters.Add("@LocationTo", SqlDbType.VarChar).Value = locationTo;
                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }

                if (resultTable.Rows.Count > 0)
                {
                   foreach (DataRow row in resultTable.Rows)
                   {
                       var values = row.ItemArray;
                       var section = new LocationManagementModel()
                       {
                            PlantCode = values[0].ToString(),
                            CountSheet = values[1].ToString(),
                            StorageLocationCode = values[2].ToString(),
                            SectionCode = values[3].ToString(),
                            SectionName = values[4].ToString(),
                            LocationFrom = values[9].ToString(),
                            LocationTo = values[10].ToString(),
                            BrandCode = values[11] == null ? "" : values[7].ToString(),

                            OriginalPlantCode = values[0].ToString(),
                            OriginalCountSheet = values[1].ToString(),
                            OriginalStorageLocCode = values[2].ToString(),
                            OriginalSectionCode = values[3].ToString(),
                            OriginalSectionName = values[4].ToString(),

                            PlantDesc = values[12].ToString()
                       };
                       sectionList.Add(section);
                   }                   
                }
                else
                {
                    sectionList = null;
                }

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                sectionList = new List<LocationManagementModel>();

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

        private List<LocationManagementModel> convertDtToListSection(DataTable table)
        {
            var sectionList = new List<LocationManagementModel>(table.Rows.Count);
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    var values = row.ItemArray;
                    var section = new LocationManagementModel()
                    {
                        PlantCode = values[0].ToString(),
                        Plant = values[0].ToString(),
                        PlantDesc = values[1].ToString(),
                        CountSheet = values[2].ToString(),
                        MCHLevel1 = "",
                        MCHLevel2 = "",
                        MCHLevel3 = "",
                        MCHLevel4 = "",
                        StorageLocationCode = values[3].ToString(),
                        SectionCode = values[4].ToString(),
                        BrandCode = values[5].ToString(),
                        SectionName = values[6].ToString(),
                        LocationFrom = values[7].ToString(),
                        LocationTo = values[8].ToString()
                    };
                    sectionList.Add(section);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                sectionList = new List<LocationManagementModel>();
            }
            return sectionList;
        }


        //public List<LocationManagementModel> GetSectionOptionalKey(string sectionCode, string sectionName, string locationFrom, string locationTo)
        public List<LocationManagementModel> GetSectionOptionalKey(string sectionCode, string sectionName, string locationFrom
                                                , string locationTo, string countSheet
                                                , string MCHLevel1, string MCHLevel2, string MCHLevel3
                                                , string MCHLevel4, string storageLocationCode, string plantCode)

        {
            Entities dbContext = new Entities();

            List<LocationManagementModel> sectionList = new List<LocationManagementModel>();

            try
            {
                sectionList = (from s in dbContext.Sections
                               //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                               orderby s.LocationFrom ascending
                               select new LocationManagementModel
                               {
                                   //OriginalSectionCode = s.SectionCode,
                                   //SectionCode = s.SectionCode,
                                   //StorageLocationCode = s.StorageLocationCode,
                                   //SectionName = s.SectionName,
                                   //LocationFrom = s.LocationFrom,
                                   //LocationTo = s.LocationTo,
                                   //BrandCode = s.BrandCode == null ? "" : s.BrandCode

                                   PlantCode = s.PlantCode,
                                   CountSheet = s.CountSheet,
                                   StorageLocationCode = s.StorageLocationCode,
                                   SectionCode = s.SectionCode,
                                   SectionName = s.SectionName,
                                   LocationFrom = s.LocationFrom,
                                   LocationTo = s.LocationTo,
                                   BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                   OriginalPlantCode = s.PlantCode,
                                   OriginalCountSheet = s.CountSheet,
                                   OriginalStorageLocCode = s.StorageLocationCode,
                                   OriginalSectionCode = s.SectionCode,
                                   OriginalSectionName = s.SectionName
                               }).ToList();
                
                if (!(string.IsNullOrWhiteSpace(sectionCode)))
                {
                    sectionList = (from sl in sectionList
                                   join s in dbContext.Sections
                                   on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                   equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                   //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                   //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                   //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                   where s.SectionCode.Contains(sectionCode)
                                   orderby s.LocationFrom ascending
                                   select new LocationManagementModel
                                   {
                                       PlantCode = s.PlantCode,
                                       CountSheet = s.CountSheet,
                                       StorageLocationCode = s.StorageLocationCode,
                                       SectionCode = s.SectionCode,
                                       SectionName = s.SectionName,
                                       LocationFrom = s.LocationFrom,
                                       LocationTo = s.LocationTo,
                                       BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                       OriginalPlantCode = s.PlantCode,
                                       OriginalCountSheet = s.CountSheet,
                                       OriginalStorageLocCode = s.StorageLocationCode,
                                       OriginalSectionCode = s.SectionCode,
                                       OriginalSectionName = s.SectionName
                                   }).ToList();
                }

                if (!(string.IsNullOrWhiteSpace(sectionName)))
                {
                    sectionList = (from sl in sectionList
                                   join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                   //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                   //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                   //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                   where s.SectionName.ToUpper().Contains(sectionName.ToUpper())
                                   orderby s.LocationFrom ascending
                                   select new LocationManagementModel
                                   {
                                       PlantCode = s.PlantCode,
                                       CountSheet = s.CountSheet,
                                       StorageLocationCode = s.StorageLocationCode,
                                       SectionCode = s.SectionCode,
                                       SectionName = s.SectionName,
                                       LocationFrom = s.LocationFrom,
                                       LocationTo = s.LocationTo,
                                       BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                       OriginalPlantCode = s.PlantCode,
                                       OriginalCountSheet = s.CountSheet,
                                       OriginalStorageLocCode = s.StorageLocationCode,
                                       OriginalSectionCode = s.SectionCode,
                                       OriginalSectionName = s.SectionName
                                   }).ToList();
                }


                if (!(string.IsNullOrWhiteSpace(locationFrom)))
                {
                    if (string.IsNullOrWhiteSpace(locationTo))
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       join l in dbContext.Locations
                                       on new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       equals new { SectionCode = l.SectionCode, StorageLocationCode = l.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where l.LocationCode.Equals(locationFrom)
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                    else
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where int.Parse(s.LocationFrom) <= int.Parse(locationTo)
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }

                if (!(string.IsNullOrWhiteSpace(locationTo)))
                {
                    if (string.IsNullOrWhiteSpace(locationFrom))
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       join l in dbContext.Locations
                                       on new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       equals new { SectionCode = l.SectionCode, StorageLocationCode = l.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where l.LocationCode.Equals(locationTo)
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                    else
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where int.Parse(s.LocationTo) >= int.Parse(locationFrom)
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }

                //------------------------
                #region MCHLevel1
                if (!string.IsNullOrEmpty(MCHLevel1))
                {
                    if (MCHLevel1.ToUpper() != "ALL")
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where s.MCHLevel1.ToUpper().Contains(MCHLevel1.ToUpper())
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                #region MCHLevel2
                if (!string.IsNullOrEmpty(MCHLevel2))
                {
                    if (MCHLevel2.ToUpper() != "ALL")
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where s.MCHLevel2.ToUpper().Contains(MCHLevel2.ToUpper())
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                #region MCHLevel3
                if (!string.IsNullOrEmpty(MCHLevel3))
                {
                    if (MCHLevel3.ToUpper() != "ALL")
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where s.MCHLevel3.ToUpper().Contains(MCHLevel3.ToUpper())
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                #region MCHLevel4
                if (!string.IsNullOrEmpty(MCHLevel4))
                {
                    if (MCHLevel4.ToUpper() != "ALL")
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where s.MCHLevel4.ToUpper().Contains(MCHLevel4.ToUpper())
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                #region storageLocation
                if (!string.IsNullOrEmpty(storageLocationCode))
                {
                    if (storageLocationCode.ToUpper() != "ALL")
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where s.StorageLocationCode.ToUpper().Contains(storageLocationCode.ToUpper())
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                #region countSheet
                if (!string.IsNullOrEmpty(countSheet))
                {
                    if (countSheet.ToUpper() != "ALL")
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where s.CountSheet.ToUpper().Contains(countSheet.ToUpper())
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion

                #region Plant
                if (!string.IsNullOrEmpty(plantCode))
                {
                    if (plantCode.ToUpper() != "ALL")
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { PlantCode = sl.PlantCode, CountSheet = sl.CountSheet, StorageLocationCode = sl.StorageLocationCode, SectionCode = sl.SectionCode, SectionName = sl.SectionName }
                                       equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                                       //on new { SectionCode = sl.OriginalSectionCode, StorageLocationCode = sl.StorageLocationCode }
                                       //equals new { SectionCode = s.SectionCode, StorageLocationCode = s.StorageLocationCode }
                                       //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                       where s.PlantCode.ToUpper().Equals(plantCode.ToUpper())
                                       orderby s.LocationFrom ascending
                                       select new LocationManagementModel
                                       {
                                           //OriginalSectionCode = s.SectionCode,
                                           //SectionCode = s.SectionCode,
                                           //StorageLocationCode = s.StorageLocationCode,
                                           ////StorageLocationName = m.StorageLocationName,
                                           //SectionName = s.SectionName,
                                           //LocationFrom = s.LocationFrom,
                                           //LocationTo = s.LocationTo,
                                           //BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                           //MCHLevel1 = s.MCHLevel1,
                                           //MCHLevel2 = s.MCHLevel2,
                                           //MCHLevel3 = s.MCHLevel3,
                                           //MCHLevel4 = s.MCHLevel4,
                                           //CountSheet = s.CountSheet,
                                           //PlantCode = s.PlantCode

                                           PlantCode = s.PlantCode,
                                           CountSheet = s.CountSheet,
                                           StorageLocationCode = s.StorageLocationCode,
                                           SectionCode = s.SectionCode,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                                           OriginalPlantCode = s.PlantCode,
                                           OriginalCountSheet = s.CountSheet,
                                           OriginalStorageLocCode = s.StorageLocationCode,
                                           OriginalSectionCode = s.SectionCode,
                                           OriginalSectionName = s.SectionName
                                       }).ToList();
                    }
                }
                #endregion
                


                return sectionList;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return sectionList;
            }
        }

        public List<LocationManagementModel> GetAllSection()
        {
            Entities dbContext = new Entities();
            List<LocationManagementModel> list = new List<LocationManagementModel>();
            try
            {
                list = (from s in dbContext.Sections
                        orderby s.LocationFrom ascending
                        //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                        select new LocationManagementModel
                        {    
                            PlantCode = s.PlantCode,
                            CountSheet = s.CountSheet,
                            StorageLocationCode = s.StorageLocationCode,
                            SectionCode = s.SectionCode,
                            SectionName = s.SectionName,
                            LocationFrom = s.LocationFrom,
                            LocationTo = s.LocationTo,
                            BrandCode = s.BrandCode == null ? "" : s.BrandCode,

                            OriginalPlantCode = s.PlantCode,
                            OriginalCountSheet = s.CountSheet,
                            OriginalStorageLocCode = s.StorageLocationCode,
                            OriginalSectionCode = s.SectionCode,
                            OriginalSectionName = s.SectionName
                        }).ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                list = new List<LocationManagementModel>();
            }
            return list;
        }

        public List<LocationManagementModel> GetSection(List<LocationManagementModel> listSection)
        {
            Entities dbContext = new Entities();
            List<LocationManagementModel> list = new List<LocationManagementModel>();
            try
            {
                list = (from l in listSection
                        //join m in dbContext.MasterStorageLocations on l.StorageLocationCode equals m.StorageLocationCode
                        join s in dbContext.Sections
                        on new {  PlantCode = l.PlantCode, CountSheet = l.CountSheet, StorageLocationCode = l.StorageLocationCode, SectionCode = l.SectionCode, SectionName = l.SectionName }
                        equals new { PlantCode = s.PlantCode, CountSheet = s.CountSheet, StorageLocationCode = s.StorageLocationCode, SectionCode = s.SectionCode, SectionName = s.SectionName }
                        orderby s.LocationFrom ascending
                        select new LocationManagementModel
                        {
                            //OriginalSectionCode = s.SectionCode,
                            //SectionCode = s.SectionCode,
                            //SectionName = s.SectionName,
                            //StorageLocationCode = s.StorageLocationCode,
                            ////StorageLocationName = m.StorageLocationName,
                            //LocationFrom = s.LocationFrom,
                            //LocationTo = s.LocationTo,
                            //CountSheet = s.CountSheet,
                            //BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                            //PlantCode = s.PlantCode,
                            //OriginPlantCode = s.PlantCode,
                            //OriginCountSheet = s.CountSheet

                            PlantCode = s.PlantCode,
                            CountSheet = s.CountSheet,
                            StorageLocationCode = s.StorageLocationCode,
                            SectionCode = s.SectionCode,
                            SectionName = s.SectionName,
                            LocationFrom = s.LocationFrom,
                            LocationTo = s.LocationTo,
                            BrandCode = s.BrandCode == null ? "" : s.BrandCode,
     
                            OriginalPlantCode = s.PlantCode,
                            OriginalCountSheet = s.CountSheet,
                            OriginalStorageLocCode = s.StorageLocationCode,
                            OriginalSectionCode = s.SectionCode,
                            OriginalSectionName = s.SectionName

                        }).ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                list = new List<LocationManagementModel>();
            }
            return list;
        }

        public List<LocationManagementModel> GetBrandFromMASTERBRAND()
        {
            List<LocationManagementModel> list = new List<LocationManagementModel>();
            Entities dbContext = new Entities();
            try
            {
                //list = (from m in dbContext.MasterBrands
                //        where m.BrandCode != null
                //        select new LocationManagementModel
                //        {
                //            OriginalSectionCode = m.BrandCode,
                //            SectionCode = m.BrandCode,
                //            SectionName = m.BrandName,
                //            BrandCode = m.BrandCode
                //        }).Distinct().ToList();

                list = (from m in dbContext.MastSAP_SKU
                        select new LocationManagementModel
                        {
                            OriginalSectionCode = m.Brand,
                            SectionCode = m.Brand,
                            SectionName = m.BrandDesc,
                            BrandCode = m.Brand,
                            PlantCode = m.Plant,
                            CountSheet = m.PIDoc,
                            StorageLocationCode = m.StorageLocation
                        }).Distinct().ToList();

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                list = new List<LocationManagementModel>();
            }
            return list.Where(x => x.BrandCode != "").ToList();
        }

        public List<LocationModel> GetAllLocation()
        {
            List<LocationModel> list = new List<LocationModel>();
            Entities dbContext = new Entities();
            try
            {
                var sectionList = dbContext.Sections.ToList();                
                foreach (var section in sectionList)
                {
                    int localFrom = int.Parse(section.LocationFrom);
                    int localTo = int.Parse(section.LocationTo);
                    
                    while (localFrom <= localTo)
                    {
                        int length = 5;
                        LocationModel newLocation = new LocationModel
                        {
                            LocationCode = localFrom.ToString("D" + length),
                            PlantCode = section.PlantCode,
                            Countsheet = section.CountSheet,
                            StorageLocationCode = section.StorageLocationCode,
                            SectionCode = section.SectionCode
                        };

                        list.Add(newLocation);
                        localFrom++;
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                list = new List<LocationModel>();
            }
            return list;
        }

        public List<LocationModel> GetSectionLocationBarcode(List<LocationManagementModel> sectionList)
        {
            List<LocationModel> list = new List<LocationModel>();
            Entities dbContext = new Entities();
            try
            {
                foreach (var section in sectionList)
                {
                    int localFrom = int.Parse(section.LocationFrom);
                    int localTo = int.Parse(section.LocationTo);

                    while (localFrom <= localTo)
                    {
                        int length = 5;
                        LocationModel newLocation = new LocationModel
                        {
                            LocationCode = localFrom.ToString("D" + length),
                            PlantCode = section.PlantCode,
                            Countsheet = section.CountSheet,
                            StorageLocationCode = section.StorageLocationCode,
                            SectionCode = section.SectionCode,
                            SectionName = section.SectionName
                        };

                        list.Add(newLocation);
                        localFrom++;
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                list = null;
            }
            return list;
        }

        public string GetLastestSectionCode()
        {
            Entities dbContext = new Entities();
            string lastestSecCode = string.Empty;
            try
            {
                lastestSecCode = (from s in dbContext.Sections
                                  orderby s.SectionCode descending
                                  select s.SectionCode).First();

                int lastestCastInt = int.Parse(lastestSecCode) + 1;
                int length = 5;
                lastestSecCode = lastestCastInt.ToString("D" + length);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                lastestSecCode = string.Empty;
            }
            return lastestSecCode;
        }

        public string GetStorageCodeByStorageName(string StorageLocationName)
        {
            string StorageLocationCode = "";
            try
            {
                Entities dbContext = new Entities();

                StorageLocationCode = (from m in dbContext.MasterStorageLocations
                                       where m.StorageLocationName.ToUpper().Equals(StorageLocationName.ToUpper())
                                       select m.StorageLocationCode).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            return StorageLocationCode;
        }

        public List<MasterStorageLocation> GetMasterStorageLocation()
        {
            List<MasterStorageLocation> listType = new List<MasterStorageLocation>();
            try
            {
                Entities dbContext = new Entities();

                listType = (from m in dbContext.MasterStorageLocations
                            select m).OrderBy(x => x.StorageLocationCode).ToList();

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                listType = new List<MasterStorageLocation>();
            }
            return listType;
        }

        //public List<LocationManagementModel> GetAllBrand()
        //{
        //    List<LocationManagementModel> brandList = dbContext.Brands.Select(b => new LocationManagementModel { BrandCode = b.BrandCode, BrandName = b.BrandName }).ToList();
        //    return brandList;
        //}

        public bool CheckSectionCodeIsExistGroupByStorageLocCode(string sectionCode, string StorageLocationCode)
        {
            Entities dbContext = new Entities();
            try
            {
                if (dbContext.Sections.Any(x => x.SectionCode == sectionCode && x.StorageLocationCode == StorageLocationCode))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool CheckSectionCodeIsExistGroupByStorageLocCodeCountsheetPlant(string plant, string countsheet, string storageLocationCode, string sectionCode)
        {
            Entities dbContext = new Entities();
            try
            {
                if (dbContext.Sections.Any(x => x.SectionCode == sectionCode && x.StorageLocationCode == storageLocationCode && x.CountSheet == (countsheet ?? "") && x.PlantCode == plant))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool CheckDuplicateKey(string countsheet, string plant, string StorageLocationCode, string sectionCode,string sectionName)
        {
            Entities dbContext = new Entities();
            try
            {
                if (dbContext.Sections.Any(x => x.SectionName == sectionName && x.SectionCode == sectionCode && x.StorageLocationCode == StorageLocationCode && x.CountSheet == countsheet && x.PlantCode == plant))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool CheckSectionCodeIsExistGroupByStorageLocationName(string sectionCode, string StorageLocationName)
        {
            Entities dbContext = new Entities();
            string StorageLocationCode = GetStorageCodeByStorageName(StorageLocationName);
            try
            {
                if (dbContext.Sections.Any(x => x.SectionCode == sectionCode && x.StorageLocationCode == StorageLocationCode))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool CheckLocationIsExistByStoreLocName(string locationCode, string sectionCode, string storageLocationName)
        {
            Entities dbContext = new Entities();
            string storageLocationCode = GetStorageCodeByStorageName(storageLocationName);
            int count = 0;
            try
            {
                count = (from l in dbContext.Locations
                         where l.StorageLocationCode != storageLocationCode && l.SectionCode != sectionCode && l.LocationCode == locationCode
                         select l).Count();

                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool CheckLocationIsExistByStoreLocCode(string locationCode, string sectionCode, string storageLocationCode)
        {
            Entities dbContext = new Entities();
            int count = 0;
            try
            {
                count = (from l in dbContext.Locations
                         where l.StorageLocationCode != storageLocationCode && l.SectionCode != sectionCode && l.LocationCode == locationCode
                         select l).Count();

                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool CheckStorageLocationExistInMaster(string storageLocationCode, string plant , string countsheet)
        {
            Entities dbContext = new Entities();
            int count = 0;
            int countMaster = 0;
            try
            {
                countMaster = (from l in dbContext.MastSAP_SKU
                               select l).Count();

                if (countMaster == 0)
                {
                    return true;
                }

                count = (from l in dbContext.MastSAP_SKU
                         where l.StorageLocation.Equals(storageLocationCode) && 
                         l.PIDoc.Equals(countsheet) && 
                         l.Plant.Equals(plant)
                         select l).Count();

                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool CheckPlantExistInMaster(string plantCode)
        {
            Entities dbContext = new Entities();
            int count = 0;
            try
            {
                count = (from l in dbContext.MasterPlants
                         where l.PlantCode.Equals(plantCode)
                         select l).Count();

                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool CheckCountsheetExistInMaster(string countsheet)
        {
            Entities dbContext = new Entities();
            int count = 0;
            try
            {
                count = (from l in dbContext.MastSAP_SKU
                         where l.PIDoc.Equals(countsheet)
                         select l).Count();

                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool CheckBrandExistInMaster(string brandCode)
        {
            Entities dbContext = new Entities();
            int count = 0;
            try
            {
                count = (from l in dbContext.MasterBrands
                         where l.BrandCode.Equals(brandCode)
                         select l).Count();

                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool CheckDataExistInMaster(ref int countSku, ref int countBarcode, ref int countRegularPrice)
        {
            Entities dbContext = new Entities();
            //int countSku = 0;
            //int countBarcode = 0;
            //int countRegularPrice = 0;
            try
            {
                countSku = (from l in dbContext.MastSAP_SKU
                         select l).Count();

                countBarcode = (from l in dbContext.MastSAP_Barcode
                             select l).Count();

                countRegularPrice = (from l in dbContext.MastSAP_Barcode
                                select l).Count();

                if (countSku > 0 || countBarcode > 0 || countRegularPrice > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool SaveSection(List<LocationManagementModel> insertList, List<LocationManagementModel> updateList, List<LocationManagementModel> deleteList, string username)
        {
            try
            {
                if (deleteList.Count > 0)
                {
                    if (!(DeleteSection(deleteList)))
                    {
                        return false;
                    }
                }
                if (insertList.Count > 0)
                {
                    List<LocationManagementModel> deleteInsertList = new List<LocationManagementModel>();
                    foreach (var list in insertList)
                    {
                        LocationManagementModel deleteSection = new LocationManagementModel
                        {
                            SectionName = list.SectionName,
                            SectionCode = list.SectionCode,
                            StorageLocationCode = list.StorageLocationCode,
                            PlantCode = list.PlantCode,
                            CountSheet = list.CountSheet,
                            LocationFrom=list.LocationFrom,
                            LocationTo = list.LocationTo
                        };
                        deleteInsertList.Add(deleteSection);
                    }

                    if (DeleteSection(deleteInsertList))
                    {
                        if (!(InsertSection(insertList, username)))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                if (updateList.Count > 0)
                {
                    if (!(UpdateSection(updateList, username)))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool DeleteSection(List<LocationManagementModel> sectionCodeList)
        {
            Entities dbContext = new Entities();

            using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {                   
                    foreach (var list in sectionCodeList)
                    {
                        int localFrom = int.Parse(list.LocationFrom);
                        int localTo = int.Parse(list.LocationTo);
                        List<Location> loc = new List<Location>();

                        while (localFrom <= localTo)
                        {
                            int length = 5;
                            var lfrom = localFrom.ToString("D" + length);

                            try
                            {
                                dbContext.Locations.Remove(dbContext.Locations.Single(a => a.LocationCode.Equals(lfrom)));
                            }
                            catch { }                           
                            localFrom++;
                        }

                        try
                        {
                            dbContext.Sections.Remove(dbContext.Sections.Single(s => s.SectionCode.Equals(list.SectionCode)
                                                        && s.StorageLocationCode.Equals(list.StorageLocationCode)
                                                        && s.PlantCode.Equals(list.PlantCode)
                                                        && s.CountSheet.Equals(list.CountSheet)
                                                        && s.SectionName.Equals(list.SectionName)));
                        }
                        catch { }
                        dbContext.SaveChanges();
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public bool DeleteAllSection()
        {
            Entities dbContext = new Entities();

            using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                List<LocationManagementModel> sectionCodeList = new List<LocationManagementModel>();

                sectionCodeList = (from s in dbContext.Sections
                                   //join m in dbContext.MasterStorageLocations on s.StorageLocationCode equals m.StorageLocationCode
                                   select new LocationManagementModel
                                   {
                                       SectionName = s.SectionName,
                                       SectionCode = s.SectionCode,
                                       StorageLocationCode = s.StorageLocationCode,
                                       PlantCode = s.PlantCode,
                                       CountSheet = s.CountSheet
                                   }).ToList();

                try
                {
                    return DeleteSection(sectionCodeList);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    return false;
                }
            }
        }

        public bool UpdateSection(List<LocationManagementModel> updateList, string username)
        {
            Entities dbContext = new Entities();
            bool result = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    bool res = false;
                    string chkSection = string.Empty;
                    string brandCode = string.Empty;
                    if (updateList.Count > 0)
                    {

                        foreach (var section in updateList)
                        {
                            var brand = dbContext.MasterBrands.Where(x => x.BrandCode == section.SectionCode).FirstOrDefault();

                            if (brand != null)
                            {
                                chkSection = brand.BrandCode.Replace("'", "''");
                            }
                            else
                            {
                                chkSection = string.Empty;
                            }

                            string cmd = "EXEC [dbo].[SCR03_SP_UPDATE_SECTIONLOCATION] @SectionCode = '" + section.SectionCode.Replace("'", "''") + "'"
                                           + " ,@SectionName = '" + section.SectionName.Replace("'", "''") + "',@LocationFrom = '" + section.LocationFrom.Replace("'", "''") + "'"
                                           + " ,@LocationTo = '" + section.LocationTo.Replace("'", "''") + "',@StorageLocCode = '" + section.StorageLocationCode.Replace("'", "''") + "'"
                                           + " ,@PlantCode = '" + section.PlantCode.Replace("'", "''") + "',@CountSheet = '" + section.CountSheet.Replace("'", "''") + "'"
                                           + " ,@OriginalPlantCode = '" + section.OriginalPlantCode.Replace("'", "''") + "',@OriginalCountSheet = '" + section.OriginalCountSheet.Replace("'", "''") + "'"
                                           + " ,@OriginalSectionCode = '" + section.OriginalSectionCode.Replace("'", "''") + "',@OriginalStorageLocCode = '" + section.OriginalStorageLocCode.Replace("'", "''") + "'"
                                           + " ,@OriginalSectionName = '" + section.OriginalSectionName.Replace("'", "''") + "'"
                                           + " ,@BrandCode = '" + chkSection + "',@UpdateBy = '" + username.Replace("'", "''") + "'";

                            res = ExeScript(cmd, conn);
                            logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, cmd, DateTime.Now);

                            if (!res)
                            {
                                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Exception : Can't execute script", DateTime.Now);
                                result = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = false;
            }

            return result;
        }

        public bool InsertSection(List<LocationManagementModel> insertList, string username)
        {
            Entities dbContext = new Entities();
            bool result = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    bool res = false;
                    string chkSection = string.Empty;
                    string brandCode = string.Empty;
                    if (insertList.Count > 0)
                    {
                        foreach (var section in insertList)
                        {
                            var brand = dbContext.MasterBrands.Where(x => x.BrandCode == section.SectionCode).FirstOrDefault();

                            if (brand != null)
                            {
                                chkSection = brand.BrandCode.Replace("'", "''");
                            }
                            else
                            {
                                chkSection = string.Empty;
                            }

                            string cmd = "EXEC [dbo].[SCR03_SP_ADD_SECTIONLOCATION] @SectionCode = '" + section.SectionCode.Replace("'", "''") + "'"
                                        + " ,@SectionName = '" + section.SectionName.Replace("'", "''") + "',@LocationFrom = '" + section.LocationFrom.Replace("'", "''") + "'"
                                        + " ,@LocationTo = '" + section.LocationTo.Replace("'", "''") + "',@BrandCode = '" + chkSection + "'"
                                        + " ,@StorageLocCode = '" + section.StorageLocationCode.Replace("'", "''") + "',@PlantCode = '" + section.PlantCode.Replace("'", "''") + "'"
                                        + " ,@CountSheet = '" + section.CountSheet.Replace("'", "''") + "',@UpdateBy = '" + username.Replace("'", "''") + "'";

                            res = ExeScript(cmd, conn);
                            //logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, cmd, DateTime.Now);
                            if (!res)
                            {
                                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Exception : Can't execute script", DateTime.Now);
                                result = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = false;
            }

            return result;
        }

        public SqlCommand PrepareDeleteSectionCommand(SqlConnection conn, SqlTransaction transaction)
        {
            // create SQL command object
            SqlCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;

            // create SQL command text
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE From Location ");
            sb.Append("Where SectionCode = @SectionCode; ");
            sb.Append("DELETE From Section ");
            sb.Append("Where SectionCode = @SectionCode; ");

            command.CommandText = sb.ToString();

            // define parameter type
            command.Parameters.Add("@SectionCode", SqlDbType.VarChar, 5);

            command.Prepare();

            return command;
        }

        public SqlCommand PrepareInsertSectionCommand(SqlConnection conn, SqlTransaction transaction)
        {
            // create SQL command object
            SqlCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;

            // create SQL command text
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO Section ");
            sb.Append("VALUES (@SectionCode, @SectionType, @SectionName, @LocationFrom, @LocationTo,@BrandCode,@UpdateDate,@UpdateBy , @CreateDate, @CreateBy);");
            command.CommandText = sb.ToString();

            // define parameter type
            command.Parameters.Add("@SectionCode", SqlDbType.VarChar, 5);
            command.Parameters.Add("@SectionType", SqlDbType.VarChar, 5);
            command.Parameters.Add("@SectionName", SqlDbType.VarChar, 100);
            command.Parameters.Add("@LocationFrom", SqlDbType.VarChar, 5);
            command.Parameters.Add("@LocationTo", SqlDbType.VarChar, 5);
            command.Parameters.Add("@BrandCode", SqlDbType.VarChar, 10);
            command.Parameters.Add("@UpdateDate", SqlDbType.VarChar, 15);
            command.Parameters.Add("@UpdateBy", SqlDbType.VarChar, 20);
            command.Parameters.Add("@CreateDate", SqlDbType.DateTime, 8);
            command.Parameters.Add("@CreateBy", SqlDbType.VarChar, 20);

            command.Prepare();

            return command;
        }

        public SqlCommand PrepareUpdateSectionCommand(SqlConnection conn, SqlTransaction transaction)
        {
            // create SQL command object
            SqlCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;

            // create SQL command text
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE Section ");
            //sb.Append("SET SectionType = @SectionType, SectionName = @SectionName, LocationFrom = @LocationFrom, LocationTo = @LocationTo, BrandCode = @BrandCOde, UpdateDate = @UpdateDate, UpdateBy = @UpdateBy ");
            sb.Append("SET SectionType = @SectionType, SectionName = @SectionName, LocationFrom = @LocationFrom, LocationTo = @LocationTo, UpdateDate = @UpdateDate, UpdateBy = @UpdateBy ");
            sb.Append("WHERE SectionCode = @SectionCode");

            command.CommandText = sb.ToString();

            // define parameter type
            command.Parameters.Add("@SectionCode", SqlDbType.VarChar, 5);
            command.Parameters.Add("@SectionType", SqlDbType.VarChar, 5);
            command.Parameters.Add("@SectionName", SqlDbType.VarChar, 100);
            command.Parameters.Add("@LocationFrom", SqlDbType.VarChar, 5);
            command.Parameters.Add("@LocationTo", SqlDbType.VarChar, 5);
            //command.Parameters.Add("@BrandCode", SqlDbType.VarChar, 20);
            command.Parameters.Add("@UpdateDate", SqlDbType.DateTime);
            command.Parameters.Add("@UpdateBy", SqlDbType.VarChar, 20);

            command.Prepare();

            return command;
        }

        public bool ExeScript(string script, SqlConnection conn)
        {
            SqlCommand comm = new SqlCommand(script, conn);
            bool isExe;
            conn.Open();
            try
            {
                comm.ExecuteNonQuery();
                isExe = true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                isExe = false;
            }
            finally
            {
                conn.Close();
            }
            return isExe;
        }

        public string GetPlantFromCountSheet(string countsheet)
        {
            string plant = "";
            List<string> listPlant = new List<string>();
            Entities dbContext = new Entities();

            try
            {
                listPlant = (from m in dbContext.MastSAP_SKU
                                  where m.PIDoc.ToUpper().Equals(countsheet.ToUpper())
                                  select m.Plant).Distinct().ToList();

                foreach (var p in listPlant)
                {
                    plant = p;
                }

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                plant = "";
            }

            return plant;
        }

        public List<string> GetCountsheetFromPlant(string countsheet)
        {
            List<string> listCountSheet = new List<string>();
            Entities dbContext = new Entities();

            try
            {
                listCountSheet = (from m in dbContext.MastSAP_SKU
                                  where m.PIDoc.ToUpper().Equals(countsheet.ToUpper())
                                  select m.Plant).Distinct().ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                listCountSheet = null;
            }

            return listCountSheet;
        }
    }
}
