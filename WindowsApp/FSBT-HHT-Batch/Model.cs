using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_Batch
{
    public class AuditStocktakingModel
    {
        public string StockTakingID { get; set; }
        public int ScanMode { get; set; }
        public string LocationCode { get; set; }
        public string Barcode { get; set; }
        public decimal Quantity { get; set; }
        public int UnitCode { get; set; }
        public string Flag { get; set; }
        public string Description { get; set; }
        public string SKUCode { get; set; }
        public string ExBarcode { get; set; }
        public string InBarcode { get; set; }
        public string BrandCode { get; set; }
        public bool SKUMode { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string DepartmentCode { get; set; }
        public string SerialNumber { get; set; } // เพิ่มใหม่
        public string ConversionCounter { get; set; }// เพิ่มใหม่
    }

    public class ImportModel
    {
        public string HHTID { get; set; }
        public string DeviceName { get; set; }
        public string Mode { get; set; }
        public List<AuditStocktakingModel> RecordData { get; set; }
    }
}
