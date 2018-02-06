using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_Model
{
    public class ReportParameter
    {
        public System.DateTime CountDate { get; set; }
        public string DepartmentCode { get; set; }
        public string SectionCode { get; set; }
        public string LocationCode { get; set; }
        public string BrandCode { get; set; }
        public string BrandName { get; set; }
        public string Barcode { get; set; }
        public string StoreType { get; set; }
        public string DiffType { get; set; }
        public string CorrectDelete { get; set; }
        public string Unit { get; set; }
        public string BranchName { get; set; }
    }

    public enum StoreType
    {
        Front = 1,
        Back = 2,
        Warehouse = 3,
        FreshFood = 4
    }

    public enum Unit
    {
        Piece = 1,
        Pack = 2,
        Case = 3,
        Gram = 4,
        Kilogram = 5,
        CC = 6
    }

    public enum DiffType
    {
        Shortage = 1,
        Over = 2,
        All = 3
    }

    public class ReportMasterBrand
    {
        public string BrandCode { get; set; }
        public string BrandName { get; set; }
    }
}
