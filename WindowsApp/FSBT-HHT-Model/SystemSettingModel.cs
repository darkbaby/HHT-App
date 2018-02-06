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
        public bool RealTimeMode { get; set; }
    }
}
