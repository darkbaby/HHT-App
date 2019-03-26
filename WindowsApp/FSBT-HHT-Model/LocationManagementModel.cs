using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_Model
{
    public class LocationManagementModel
    {
        //ห้ามสลับที่ มีใหม่ต่อท้าย !!!
        //public string StorageLocationName { get; set; }

        public string CountSheet { get; set; }
        public string PlantCode { get; set; }
        public string StorageLocationCode { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string FlagAction { get; set; }
        public string BrandCode { get; set; }
        public string OriginalPlantCode { get; set; }
        public string OriginalCountSheet { get; set; }
        public string OriginalStorageLocCode { get; set; }
        public string OriginalSectionCode { get; set; }
        public string OriginalSectionName { get; set; }
        public string MCHLevel1 { get; set; }
        public string MCHLevel2 { get; set; }
        public string MCHLevel3 { get; set; }
        public string MCHLevel4 { get; set; }

        public string Plant { get; set; }

        public string PlantDesc { get; set; }

    }

    public class LocationModel
    {       
        public string BrandCode { get; set; }
        public string PlantCode { get; set; }
        public string Countsheet { get; set; }
        public string StorageLocationCode { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string LocationCode { get; set; }
    }

    public class ParameterModel
    {
        public string Plant { get; set; }
        public string Countsheet { get; set; }
        public string StorageLocationCode { get; set; }
        public string SectionCode { get; set; }
        public string LocationFromCode { get; set; }
        public string LocationToCode { get; set; }
    }


    public class DuplicateKeyLocationModel
    {
        public string PlantCode { get; set; }
        public string CountSheet { get; set; }
        public string StorageLocationCode { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
    }


}
