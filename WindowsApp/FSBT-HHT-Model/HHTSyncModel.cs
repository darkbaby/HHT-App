using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;

namespace FSBT_HHT_Model
{
    public class LocationHHTModel
    {
        public string LocationCode1 { get; set; }
        public string LocationCode2 { get; set; }
        public string LocationCode3 { get; set; }
    }

    //public class StatusDownloadGridviewModel
    //{
    //    public string Number { get; set; }
    //    public string Type { get; set; }
    //    public string InitRecord { get; set; }
    //    public string DownloadRecord { get; set; }
    //}

    public class DownloadLocationModel
    {
        public string LocationCode { get; set; }
        public string SectionCode { get; set; }
        public int ScanMode { get; set; }
        public string SectionName { get; set; }
        public string BrandCode { get; set; }
    }

    public class PCSKUModel
    {
        public string Department { get; set; }
        public string SKUCode { get; set; }
        public string BrandCode { get; set; }
        //public string BrandName { get; set; }
        public string Description { get; set; }
        public int QTYOnHand { get; set; }
        public int StockOnHand { get; set; }
        public decimal Price { get; set; }
        public string ExBarcode { get; set; }
        public string InBarcode { get; set; }
        public string MKCode { get; set; }
    }

    public class UnitModel
    {
        public int UnitCode { get; set; }
        public string UnitName { get; set; }
        public string CodeType { get; set; }
    }

    public class ScanModeModel
    {
        public int ScanModeID { get; set; }
        public string ScanModeName { get; set; }
    }

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
        public string HHTName { get; set; }
        public DateTime CountDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string DepartmentCode { get; set; }
        public string FileName { get; set; }
        public bool FlagImport { get; set; }
        public string HHTID { get; set; }
        public DateTime ImportDate { get; set; }
        public string MKCode { get; set; }
        public string ProductType { get; set; }
        public string FlagLoation { get; set; }

    }

    public class InsertAutoResultModel
    {
        public bool result { get; set; }
        public string hhtID { get; set; } 
        public string hhtName { get; set; }
        public string stocktaker { get; set; }
    }

    public class MasterBarcodeModel
    {
        public string Status { get; set; }
        public string ExBarcode { get; set; }
        public string Barcode { get; set; }
        public string NoExBarcode { get; set; }
        public string EAN_UPC { get; set; }
        public string GroupCode { get; set; }
        public string ProductCode { get; set; }
        public string SKUCode { get; set; }
        public int ScanMode { get; set; }
    }

    public class MasterPackModel
    {
        public string Status { get; set; }
        public string GroupCode { get; set; }
        public string ProductCode { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public int UnitQTY { get; set; }
        public string SKUCode { get; set; }
    }

    public class USBDeviceInfo
    {
        public USBDeviceInfo(string deviceID, UInt32 pnpDeviceID, string description, string name, string manufacturer)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
            this.Name = name;
            this.Manufacturer = manufacturer;
        }
        public string DeviceID { get; private set; }
        public UInt32 PnpDeviceID { get; private set; }
        public string Description { get; private set; }
        public string Name { get; private set; }
        public string Manufacturer { get; private set; }
    }

    
    

    //public class DriveDetection
    //{
    //    // Used For Monitoring Of USB Connection 
    //    private ManagementEventWatcher Connected;
    //    private ManagementEventWatcher Removed;
    //    public void USBControl()
    //    {
    //        // USB Connection Event Watching
    //        Connected = new ManagementEventWatcher();
    //        Connected = OnConnected();
    //        // USB Removal Event Watching
    //        Removed = new ManagementEventWatcher();
    //        Removed = OnRemoved;
    //    }

    //    private void OnConnected()
    //    {
    //        //enable all the windows
    //        for (int i = 0; i < Application.Current.Windows.Count; i++)
    //        {
    //            Application.Current.Windows[i].IsEnabled = true;
    //        }
    //    }

    //    private void OnRemoved()
    //    {
    //        //disable all the windows
    //        for (int i = 0; i < Application.Current.Windows.Count; i++)
    //        {
    //            Application.Current.Windows[i].IsEnabled = false;
    //        }
    //    }
    //}

    
}
