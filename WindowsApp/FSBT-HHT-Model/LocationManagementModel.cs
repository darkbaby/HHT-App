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
        public string DepartmentCode { get; set; }
        public string SectionType { get; set; }
        public string OriginSectionCode { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string BrandCode { get; set; }
        public string FlagAction { get; set; }
        public int ScanMode { get; set; }
        public string OriginSectionType { get; set; }
    }

    public class Location
    {
        public string LocationCode { get; set; }
        public string SectionCode { get; set; }
        public int ScanMode { get; set; }
        public string SectionType { get; set; }
    }

    //public class ComboboxBrandItem
    //{
    //    public string Text { get; set; }
    //    public string BrandCode { get; set; }
    //    public string BrandName { get; set; }

    //    public override string ToString()
    //    {
    //        return Text;
    //    }

    //    public string GetBrandCode()
    //    {
    //        return BrandCode;
    //    }

    //    public string GetBrandName()
    //    {
    //        return BrandName;
    //    }
    //}


}
