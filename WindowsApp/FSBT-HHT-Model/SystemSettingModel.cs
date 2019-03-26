using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_Model
{
    public class SystemSettingModel
    {
        public int MaxLoginFail { get; set; }
        public string ComID { get; set; }
        public string ComName { get; set; }        
        public string SectionType { get; set; }
        public string Branch { get; set; }
        public DateTime CountDate { get; set; }
        public string PathSKUFile { get; set; }
        public string PathBarcodeFile { get; set; }
        public string PathPackBarcodeFile { get; set; }
        public string PathBrandFile { get; set; }
        public string StoreID { get; set; }
        public string Plant { get; set; }
        public bool IsEditMchSettings { get; set; }
        public string MCHLevel1 { get; set; }
        public string MCHLevel2 { get; set; }
        public string MCHLevel3 { get; set; }
        public string MCHLevel4 { get; set; }
        public List<MasterPlantModel> listPlant { get; set; }
        public string UpdateBy { get; set; }

        public string ScanMode { get; set; }
        //public string SFTPSKUFileName { get; set; }
        //public string SFTPSKUHost { get; set; }
        //public string SFTPSKULocalPath { get; set; }
        //public string SFTPSKURemotePath { get; set; }

        //public string SFTPBarcodeFileName { get; set; }
        //public string SFTPBarcodeHost { get; set; }
        //public string SFTPBarcodeRemotePath { get; set; }
        //public string SFTPBarcodeLocalPath { get; set; }

        //public string SFTPRegularPriceFileName { get; set; }
        //public string SFTPRegularPriceHost { get; set; }
        //public string SFTPRegularPriceRemotePath { get; set; }
        //public string SFTPRegularPriceLocalPath { get; set; }

        //public string SFTPAcknowledgeFileName { get; set; }
        //public string SFTPAcknowledgeHost { get; set; }
        //public string SFTPAcknowledgeRemotePath { get; set; }
        //public string SFTPAcknowledgeLocalPath { get; set; }

        //public string SFTPExportFileName { get; set; }
        //public string SFTPExportHost { get; set; }
        //public string SFTPExportRemotePath { get; set; }
        //public string SFTPExportLocalPath { get; set; }


        //public string PrivateKeySAP { get; set; }
        //public string PrivateKeyPOS { get; set; }
        public bool RealTimeMode { get; set; }
    }
}
