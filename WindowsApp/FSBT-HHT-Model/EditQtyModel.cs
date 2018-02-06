using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_Model
{
    public class EditQtyModel
    {
        public class Request
        {
            public string DepartmentCode { get; set; }
            public string SectionCode { get; set; }
            public string SectionName { get; set; }
            public string SectionType { get; set; }
            public string LocationFrom { get; set; }
            public string LocationTo { get; set; }
            public string Barcode { get; set; }
            public string SKUCode { get; set; }
        }
        
        public class Response
        {
            public Int32 No { get; set; }
            public string StocktakingID { get; set; }
            public string LocationCode { get; set; }
            public string Barcode { get; set; }
            public decimal? Quantity { get; set; }
            public decimal? NewQuantity { get; set; }
            public int UnitCode { get; set; }
            public string Flag { get; set; }
            public string Description { get; set; }
            public string DepartmentCode { get; set; }
            public string HHTName { get; set; }
            public int ScanMode { get; set; }
            public string SKUCode { get; set; }
            public string InBarCode { get; set; }
            public string ExBarCode { get; set; }
            public bool SKUMode { get; set; }
            public string ChkSKUCode { get; set; }
            public string Remark { get; set; }
            public string MKCode { get; set; }
            public string ProductType { get; set; }
        }
        public class ResponseSummary
        {
            public string LocationCode { get; set; }
            public decimal Quantity { get; set; }
            public decimal NewQuantity { get; set; }
            public string UnitName { get; set; }
        }
        public class ResponseSummaryMKCode : ResponseSummary
        {
            public string MKCode { get; set; }
        }

        public class MasterSKU
        {
            public string SKUCode { get; set; }
            public string InBarCode { get; set; }
            public string ExBarCode { get; set; }
            public string Description { get; set; }
            public string DepartmentCode { get; set; }
            public string MKCode { get; set; }
            public string ProductType { get; set; }
            public int UnitCode { get; set; }
            public string Result { get; set; }
        }

        public class MasterScanMode
        {
            public int ScanModeID { get; set; }
            public string ScanModeName { get; set; }
        }

        public class MasterUnit
        {
            public int UnitCode { get; set; }
            public string UnitName { get; set; }
        }

        public class ResponseDeleteReport
        {
            public string StocktakingID { get; set; }
            public string ScanModeName { get; set; }
            public string LocationCode { get; set; }
            public string Barcode { get; set; }
            public decimal Quantity { get; set; }
            public decimal NewQuantity { get; set; }
            public string UnitName { get; set; }
            public string Flag { get; set; }
            public string Description { get; set; }
            public string Store { get; set; }
            public string Section { get; set; }
        }
    }
}
