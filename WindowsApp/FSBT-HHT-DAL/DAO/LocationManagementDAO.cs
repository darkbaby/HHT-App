using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_Model;
using System.Globalization;

namespace FSBT_HHT_DAL.DAO
{
    public class LocationManagementDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        public List<LocationManagementModel> GetSection(string sectionCode, string sectionName, string locationFrom, string locationTo, string sectionType, string deptCode)
        {
            Entities dbContext = new Entities();

            List<LocationManagementModel> sectionList = new List<LocationManagementModel>();
            string[] secionTypeArr = sectionType.Split('|');
            List<string> listSectionType = new List<string>(secionTypeArr);
            listSectionType = (from l in listSectionType
                               where l != ""
                               select l.ToUpper()).ToList();

            try
            {
                if (!(string.IsNullOrWhiteSpace(sectionCode) && string.IsNullOrWhiteSpace(sectionName)
                    && string.IsNullOrWhiteSpace(locationFrom) && string.IsNullOrWhiteSpace(locationTo)
                    && listSectionType.Count <= 0 && string.IsNullOrWhiteSpace(deptCode)))
                {
                    sectionList = (from s in dbContext.Sections
                                   join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                   select new LocationManagementModel
                                   {
                                       DepartmentCode = s.DepartmentCode,
                                       OriginSectionCode = s.SectionCode,
                                       SectionCode = s.SectionCode,
                                       ScanMode = m.ScanModeID,
                                       SectionType = m.ScanModeName,
                                       SectionName = s.SectionName,
                                       LocationFrom = s.LocationFrom,
                                       LocationTo = s.LocationTo,
                                       BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                       OriginSectionType = m.ScanModeName
                                   }).ToList();

                    if (listSectionType.Count > 0)
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                       equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                       join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                       where listSectionType.Contains(m.ScanModeName.ToUpper())
                                       select new LocationManagementModel
                                       {
                                           DepartmentCode = s.DepartmentCode,
                                           OriginSectionCode = s.SectionCode,
                                           SectionCode = s.SectionCode,
                                           SectionType = m.ScanModeName,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                           ScanMode = s.ScanMode,
                                           OriginSectionType = m.ScanModeName
                                       }).ToList();
                    }

                    if (!(string.IsNullOrWhiteSpace(sectionCode)))
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                       equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                       join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                       where s.SectionCode.Contains(sectionCode)
                                       select new LocationManagementModel
                                       {
                                           DepartmentCode = s.DepartmentCode,
                                           OriginSectionCode = s.SectionCode,
                                           SectionCode = s.SectionCode,
                                           SectionType = m.ScanModeName,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                           ScanMode = s.ScanMode,
                                           OriginSectionType = m.ScanModeName
                                       }).ToList();
                    }

                    if (!(string.IsNullOrWhiteSpace(sectionName)))
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                       equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                       join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                       where s.SectionName.ToUpper().Contains(sectionName.ToUpper())
                                       select new LocationManagementModel
                                       {
                                           DepartmentCode = s.DepartmentCode,
                                           OriginSectionCode = s.SectionCode,
                                           SectionCode = s.SectionCode,
                                           SectionType = m.ScanModeName,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                           ScanMode = s.ScanMode,
                                           OriginSectionType = m.ScanModeName
                                       }).ToList();
                    }

                    if (!(string.IsNullOrWhiteSpace(locationFrom)))
                    {
                        if (string.IsNullOrWhiteSpace(locationTo))
                        {
                            sectionList = (from sl in sectionList
                                           join s in dbContext.Sections
                                           on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                           equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                           join l in dbContext.Locations
                                           on new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                           equals new { SectionCode = l.SectionCode, SectionType = l.ScanMode }
                                           join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                           where l.LocationCode.Equals(locationFrom)
                                           select new LocationManagementModel
                                           {
                                               DepartmentCode = s.DepartmentCode,
                                               OriginSectionCode = s.SectionCode,
                                               SectionCode = s.SectionCode,
                                               SectionType = m.ScanModeName,
                                               SectionName = s.SectionName,
                                               LocationFrom = s.LocationFrom,
                                               LocationTo = s.LocationTo,
                                               BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                               ScanMode = s.ScanMode,
                                               OriginSectionType = m.ScanModeName
                                           }).ToList();
                        }
                        else
                        {
                            sectionList = (from sl in sectionList
                                           join s in dbContext.Sections
                                           on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                           equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                           join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                           where int.Parse(s.LocationTo) >= int.Parse(locationFrom)
                                           select new LocationManagementModel
                                           {
                                               DepartmentCode = s.DepartmentCode,
                                               OriginSectionCode = s.SectionCode,
                                               SectionCode = s.SectionCode,
                                               SectionType = m.ScanModeName,
                                               SectionName = s.SectionName,
                                               LocationFrom = s.LocationFrom,
                                               LocationTo = s.LocationTo,
                                               BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                               ScanMode = s.ScanMode,
                                               OriginSectionType = m.ScanModeName
                                           }).ToList();
                        }
                    }

                    if (!(string.IsNullOrWhiteSpace(locationTo)))
                    {
                        if (string.IsNullOrWhiteSpace(locationFrom))
                        {
                            sectionList = (from sl in sectionList
                                           join s in dbContext.Sections
                                           on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                           equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                           join l in dbContext.Locations
                                           on new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                           equals new { SectionCode = l.SectionCode, SectionType = l.ScanMode }
                                           join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                           where l.LocationCode.Equals(locationTo)
                                           select new LocationManagementModel
                                           {
                                               DepartmentCode = s.DepartmentCode,
                                               OriginSectionCode = s.SectionCode,
                                               SectionCode = s.SectionCode,
                                               SectionType = m.ScanModeName,
                                               SectionName = s.SectionName,
                                               LocationFrom = s.LocationFrom,
                                               LocationTo = s.LocationTo,
                                               BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                               ScanMode = s.ScanMode,
                                               OriginSectionType = m.ScanModeName
                                           }).ToList();
                        }
                        else
                        {
                            sectionList = (from sl in sectionList
                                           join s in dbContext.Sections
                                           on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                           equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                           join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                           where int.Parse(s.LocationFrom) <= int.Parse(locationTo)
                                           //&& int.Parse(s.LocationTo) >= int.Parse(locationTo)
                                           select new LocationManagementModel
                                           {
                                               DepartmentCode = s.DepartmentCode,
                                               OriginSectionCode = s.SectionCode,
                                               SectionCode = s.SectionCode,
                                               SectionType = m.ScanModeName,
                                               SectionName = s.SectionName,
                                               LocationFrom = s.LocationFrom,
                                               LocationTo = s.LocationTo,
                                               BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                               ScanMode = s.ScanMode,
                                               OriginSectionType = m.ScanModeName
                                           }).ToList();
                        }
                    }

                    if (!(string.IsNullOrWhiteSpace(deptCode)))
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                       equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                       join mScan in dbContext.MasterScanModes on s.ScanMode equals mScan.ScanModeID
                                       where s.DepartmentCode.Equals(deptCode)
                                       select new LocationManagementModel
                                       {
                                           DepartmentCode = s.DepartmentCode,
                                           OriginSectionCode = s.SectionCode,
                                           SectionCode = s.SectionCode,
                                           SectionType = mScan.ScanModeName,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                           ScanMode = s.ScanMode,
                                           OriginSectionType = mScan.ScanModeName
                                       }).Distinct().ToList();
                    }
                }

                return sectionList;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return sectionList;
            }
        }

        public List<LocationManagementModel> GetSectionOptionalKey(string sectionCode, string sectionName, string locationFrom, string locationTo, string sectionType, string deptCode)
        {
            Entities dbContext = new Entities();

            List<LocationManagementModel> sectionList = new List<LocationManagementModel>();
            string[] secionTypeArr = sectionType.Split('|');
            List<string> listSectionType = new List<string>(secionTypeArr);
            listSectionType = (from l in listSectionType
                               where l != ""
                               select l.ToUpper()).ToList();

            try
            {
                sectionList = (from s in dbContext.Sections
                               join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                               select new LocationManagementModel
                               {
                                   DepartmentCode = s.DepartmentCode,
                                   OriginSectionCode = s.SectionCode,
                                   SectionCode = s.SectionCode,
                                   SectionType = m.ScanModeName,
                                   SectionName = s.SectionName,
                                   LocationFrom = s.LocationFrom,
                                   LocationTo = s.LocationTo,
                                   BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                   ScanMode = m.ScanModeID,
                                   OriginSectionType = m.ScanModeName
                               }).ToList();

                if (listSectionType.Count > 0)
                {
                    sectionList = (from sl in sectionList
                                   join s in dbContext.Sections
                                   on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                   equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                   join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                   where listSectionType.Contains(m.ScanModeName.ToUpper())
                                   select new LocationManagementModel
                                   {
                                       DepartmentCode = s.DepartmentCode,
                                       OriginSectionCode = s.SectionCode,
                                       SectionCode = s.SectionCode,
                                       SectionType = m.ScanModeName,
                                       SectionName = s.SectionName,
                                       LocationFrom = s.LocationFrom,
                                       LocationTo = s.LocationTo,
                                       BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                       ScanMode = s.ScanMode,
                                       OriginSectionType = m.ScanModeName
                                   }).ToList();
                }

                if (!(string.IsNullOrWhiteSpace(sectionCode)))
                {
                    sectionList = (from sl in sectionList
                                   join s in dbContext.Sections
                                   on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                   equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                   join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                   where s.SectionCode.Contains(sectionCode)
                                   select new LocationManagementModel
                                   {
                                       DepartmentCode = s.DepartmentCode,
                                       OriginSectionCode = s.SectionCode,
                                       SectionCode = s.SectionCode,
                                       SectionType = m.ScanModeName,
                                       SectionName = s.SectionName,
                                       LocationFrom = s.LocationFrom,
                                       LocationTo = s.LocationTo,
                                       BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                       ScanMode = s.ScanMode,
                                       OriginSectionType = m.ScanModeName
                                   }).ToList();
                }

                if (!(string.IsNullOrWhiteSpace(sectionName)))
                {
                    sectionList = (from sl in sectionList
                                   join s in dbContext.Sections
                                   on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                   equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                   join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                   where s.SectionName.ToUpper().Contains(sectionName.ToUpper())
                                   select new LocationManagementModel
                                   {
                                       DepartmentCode = s.DepartmentCode,
                                       OriginSectionCode = s.SectionCode,
                                       SectionCode = s.SectionCode,
                                       SectionType = m.ScanModeName,
                                       SectionName = s.SectionName,
                                       LocationFrom = s.LocationFrom,
                                       LocationTo = s.LocationTo,
                                       BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                       ScanMode = s.ScanMode,
                                       OriginSectionType = m.ScanModeName
                                   }).ToList();
                }


                if (!(string.IsNullOrWhiteSpace(locationFrom)))
                {
                    if (string.IsNullOrWhiteSpace(locationTo))
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                       equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                       join l in dbContext.Locations
                                       on new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                       equals new { SectionCode = l.SectionCode, SectionType = l.ScanMode }
                                       join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                       where l.LocationCode.Equals(locationFrom)
                                       select new LocationManagementModel
                                       {
                                           DepartmentCode = s.DepartmentCode,
                                           OriginSectionCode = s.SectionCode,
                                           SectionCode = s.SectionCode,
                                           SectionType = m.ScanModeName,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                           ScanMode = s.ScanMode,
                                           OriginSectionType = m.ScanModeName
                                       }).ToList();
                    }
                    else
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                       equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                       join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                       where int.Parse(s.LocationFrom) <= int.Parse(locationTo)
                                       select new LocationManagementModel
                                       {
                                           DepartmentCode = s.DepartmentCode,
                                           OriginSectionCode = s.SectionCode,
                                           SectionCode = s.SectionCode,
                                           SectionType = m.ScanModeName,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                           ScanMode = s.ScanMode,
                                           OriginSectionType = m.ScanModeName
                                       }).ToList();
                    }
                }

                if (!(string.IsNullOrWhiteSpace(locationTo)))
                {
                    if (string.IsNullOrWhiteSpace(locationFrom))
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                       equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                       join l in dbContext.Locations
                                       on new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                       equals new { SectionCode = l.SectionCode, SectionType = l.ScanMode }
                                       join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                       where l.LocationCode.Equals(locationTo)
                                       select new LocationManagementModel
                                       {
                                           DepartmentCode = s.DepartmentCode,
                                           OriginSectionCode = s.SectionCode,
                                           SectionCode = s.SectionCode,
                                           SectionType = m.ScanModeName,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                           ScanMode = s.ScanMode,
                                           OriginSectionType = m.ScanModeName
                                       }).ToList();
                    }
                    else
                    {
                        sectionList = (from sl in sectionList
                                       join s in dbContext.Sections
                                       on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                       equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                       join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                       where int.Parse(s.LocationTo) >= int.Parse(locationFrom)
                                       select new LocationManagementModel
                                       {
                                           DepartmentCode = s.DepartmentCode,
                                           OriginSectionCode = s.SectionCode,
                                           SectionCode = s.SectionCode,
                                           SectionType = m.ScanModeName,
                                           SectionName = s.SectionName,
                                           LocationFrom = s.LocationFrom,
                                           LocationTo = s.LocationTo,
                                           BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                           ScanMode = s.ScanMode,
                                           OriginSectionType = m.ScanModeName
                                       }).ToList();
                    }
                }


                if (!(string.IsNullOrWhiteSpace(deptCode)))
                {
                    sectionList = (from sl in sectionList
                                   join s in dbContext.Sections
                                   on new { SectionCode = sl.OriginSectionCode, SectionType = sl.ScanMode }
                                   equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                                   join mScan in dbContext.MasterScanModes on s.ScanMode equals mScan.ScanModeID
                                   where s.DepartmentCode.Equals(deptCode)
                                   select new LocationManagementModel
                                   {
                                       DepartmentCode = s.DepartmentCode,
                                       OriginSectionCode = s.SectionCode,
                                       SectionCode = s.SectionCode,
                                       SectionType = mScan.ScanModeName,
                                       SectionName = s.SectionName,
                                       LocationFrom = s.LocationFrom,
                                       LocationTo = s.LocationTo,
                                       BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                                       ScanMode = s.ScanMode,
                                       OriginSectionType = mScan.ScanModeName
                                   }).Distinct().ToList();
                }

                return sectionList;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                        join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                        select new LocationManagementModel
                        {
                            DepartmentCode = s.DepartmentCode,
                            OriginSectionCode = s.SectionCode,
                            SectionCode = s.SectionCode,
                            SectionType = m.ScanModeName,
                            SectionName = s.SectionName,
                            LocationFrom = s.LocationFrom,
                            LocationTo = s.LocationTo,
                            BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                            FlagAction = "",
                            OriginSectionType = m.ScanModeName
                        }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                        join m in dbContext.MasterScanModes on l.SectionType equals m.ScanModeName
                        join s in dbContext.Sections
                        on new { SectionCode = l.SectionCode, SectionType = m.ScanModeID }
                        equals new { SectionCode = s.SectionCode, SectionType = s.ScanMode }
                        select new LocationManagementModel
                        {
                            DepartmentCode = s.DepartmentCode,
                            OriginSectionCode = s.SectionCode,
                            SectionCode = s.SectionCode,
                            SectionType = m.ScanModeName,
                            SectionName = s.SectionName,
                            LocationFrom = s.LocationFrom,
                            LocationTo = s.LocationTo,
                            BrandCode = s.BrandCode == null ? "" : s.BrandCode,
                            FlagAction = "",
                            OriginSectionType = m.ScanModeName
                        }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                list = (from m in dbContext.MasterBrands
                        where m.BrandCode != null
                        select new LocationManagementModel
                        {
                            DepartmentCode = m.DepartmentCode,
                            OriginSectionCode = m.BrandCode,
                            SectionCode = m.BrandCode,
                            SectionName = m.BrandName,
                            BrandCode = m.BrandCode
                        }).Distinct().ToList();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                list = new List<LocationManagementModel>();
            }
            return list.Where(x => x.BrandCode != "").ToList();
        }

        public List<FSBT_HHT_Model.Location> GetAllLocation()
        {
            List<FSBT_HHT_Model.Location> list = new List<FSBT_HHT_Model.Location>();
            Entities dbContext = new Entities();
            try
            {
                list = (from l in dbContext.Locations
                        join m in dbContext.MasterScanModes on l.ScanMode equals m.ScanModeID
                        select new FSBT_HHT_Model.Location
                        {
                            LocationCode = l.LocationCode,
                            SectionCode = l.SectionCode,
                            SectionType = m.ScanModeName
                        }).Distinct().ToList();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                list = new List<FSBT_HHT_Model.Location>();
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                lastestSecCode = string.Empty;
            }
            return lastestSecCode;
        }

        public int GetSectionTypeIDBySectionTypeName(string SectionType)
        {
            int sectionID = 0;
            try
            {
                Entities dbContext = new Entities();

                sectionID = (from m in dbContext.MasterScanModes
                             where m.ScanModeName.ToUpper().Equals(SectionType.ToUpper())
                             select m.ScanModeID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            return sectionID;
        }

        public List<MasterScanMode> GetSectionMasterType()
        {
            List<MasterScanMode> listType = new List<MasterScanMode>();
            try
            {
                Entities dbContext = new Entities();

                listType = (from m in dbContext.MasterScanModes
                            select m).OrderBy(x => x.ScanModeID).ToList();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                listType = new List<MasterScanMode>();
            }
            return listType;
        }

        //public List<LocationManagementModel> GetAllBrand()
        //{
        //    List<LocationManagementModel> brandList = dbContext.Brands.Select(b => new LocationManagementModel { BrandCode = b.BrandCode, BrandName = b.BrandName }).ToList();
        //    return brandList;
        //}

        public bool CheckSectionCodeIsExistGroupBySectionType(string sectionCode, string sectionType)
        {
            Entities dbContext = new Entities();
            int scanModeID = GetSectionTypeIDBySectionTypeName(sectionType);
            try
            {
                if (dbContext.Sections.Any(x => x.SectionCode == sectionCode && x.ScanMode == scanModeID))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        }

        public bool CheckLocationIsExist(string locationCode, string sectionCode, string sectionType)
        {
            Entities dbContext = new Entities();
            int sectionTypeID = GetSectionTypeIDBySectionTypeName(sectionType);
            int count = 0;
            try
            {
                count = (from l in dbContext.Locations
                         where l.ScanMode != sectionTypeID && l.SectionCode != sectionCode && l.LocationCode == locationCode
                         select l).Count();

                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        }

        public bool SaveSection(List<LocationManagementModel> insertList, List<LocationManagementModel> updateList, List<FSBT_HHT_Model.Location> deleteList, string username)
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
                    List<FSBT_HHT_Model.Location> deleteInsertList = new List<FSBT_HHT_Model.Location>();
                    foreach (var list in insertList)
                    {
                        FSBT_HHT_Model.Location deleteSection = new FSBT_HHT_Model.Location
                        {
                            SectionCode = list.SectionCode,
                            SectionType = list.SectionType
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        }

        public bool DeleteSection(List<FSBT_HHT_Model.Location> sectionCodeList)
        {
            Entities dbContext = new Entities();

            using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {

                    StringBuilder sql = new StringBuilder();
                    foreach (var list in sectionCodeList)
                    {
                        List<Location> loc = new List<Location>();
                        int sectionType = GetSectionTypeIDBySectionTypeName(list.SectionType);
                        loc = (from l in dbContext.Locations
                               where l.SectionCode.Equals(list.SectionCode)
                               && l.ScanMode.Equals(sectionType)
                               select l).ToList();

                        if (loc.Count() > 0)
                        {
                            foreach (Location l in loc)
                            {
                                dbContext.Locations.Remove(l);
                                dbContext.SaveChanges();
                            }
                        }

                        List<Section> sec = (from s in dbContext.Sections
                                             where s.SectionCode.Equals(list.SectionCode)
                                             && s.ScanMode.Equals(sectionType)
                                             select s).ToList();

                        if (sec.Count() > 0)
                        {
                            foreach (Section s in sec)
                            {
                                dbContext.Sections.Remove(s);
                                dbContext.SaveChanges();
                            }
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
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
                List<FSBT_HHT_Model.Location> sectionCodeList = new List<FSBT_HHT_Model.Location>();

                sectionCodeList = (from s in dbContext.Sections
                                   join m in dbContext.MasterScanModes on s.ScanMode equals m.ScanModeID
                                   select new FSBT_HHT_Model.Location
                                   {
                                       SectionCode = s.SectionCode,
                                       SectionType = m.ScanModeName
                                   }).ToList();

                try
                {
                    return DeleteSection(sectionCodeList);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    log.Error(String.Format("Exception : {0}", ex.StackTrace));
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

                            //if (string.IsNullOrWhiteSpace(section.BrandCode))
                            //{
                            //    chkSection = section.SectionCode.Replace("'", "''");
                            //}
                            //else
                            //{
                            //chkSection = brandCode.Replace("'", "''");
                            //}
                            string cmd = "EXEC [dbo].[SP_UPDATE_SECTIONLOCATION] @DepartmentCode = '" + section.DepartmentCode.Replace("'", "''") + "'"
                                           + " ,@SectionCode = '" + section.SectionCode.Replace("'", "''") + "',@SectionTypeName = '" + section.SectionType.Replace("'", "''") + "'"
                                           + " ,@SectionName = '" + section.SectionName.Replace("'", "''") + "',@LocationFrom = '" + section.LocationFrom.Replace("'", "''") + "'"
                                           + " ,@LocationTo = '" + section.LocationTo.Replace("'", "''") + "',@BrandCode = '" + chkSection + "'"
                                           + " ,@UpdateBy = '" + username.Replace("'", "''") + "'";

                            res = ExeScript(cmd, conn);

                            if (!res)
                            {
                                log.Error(String.Format("Exception : Can't execute script"));
                                result = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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



                            string cmd = "EXEC [dbo].[SP_ADD_SECTIONLOCATION] @DepartmentCode = '" + section.DepartmentCode.Replace("'", "''") + "'"
                                           + " ,@SectionCode = '" + section.SectionCode.Replace("'", "''") + "',@SectionTypeName = '" + section.SectionType.Replace("'", "''") + "'"
                                           + " ,@SectionName = '" + section.SectionName.Replace("'", "''") + "',@LocationFrom = '" + section.LocationFrom.Replace("'", "''") + "'"
                                           + " ,@LocationTo = '" + section.LocationTo.Replace("'", "''") + "',@BrandCode = '" + chkSection + "'"
                                           + " ,@UpdateBy = '" + username.Replace("'", "''") + "'";

                            res = ExeScript(cmd, conn);

                            if (!res)
                            {
                                log.Error(String.Format("Exception : Can't execute script"));
                                result = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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

        static bool ExeScript(string script, SqlConnection conn)
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                isExe = false;
            }
            finally
            {
                conn.Close();
            }
            return isExe;
        }
    }
}
