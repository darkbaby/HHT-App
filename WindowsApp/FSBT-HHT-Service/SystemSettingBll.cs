using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_DAL;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_Model;


namespace FSBT_HHT_BLL
{
    public class SystemSettingBll  
    {
        private SystemSettingDAO settingDAO = new SystemSettingDAO();
        public SystemSettingBll()
        {

        }

        public SystemSettingModel GetSettingData()
        {
            return settingDAO.GetSettingData();
        }

        public string  GetSettingStringByKey(string  key)
        {
            return settingDAO.GetSettingStringByKey(key);
        }
        public int GetSettingIntByKey(string key)
        {
            return settingDAO.GetSettingIntByKey(key);
        }
        
        public string UpdateSetting(SystemSettingModel newDateSetting)
        {
            return settingDAO.UpdateSettingData(newDateSetting);
        }

        public string UpdateSetting(string settingKey, string datatype, string value)
        {
            return settingDAO.UpdateSettingData(settingKey, datatype, value);
        }

        public string GetComputerID()
        {
            return settingDAO.GetComputerID();
        }

        public string GetComputerName()
        {
            return settingDAO.GetComputerName();
        }

        public bool SaveModeRealtime(bool isRealtime)
        {
            return settingDAO.saveModeRealtime(isRealtime);
        }

        public bool GetRealTimeMode()
        {
            return settingDAO.GetRealTimeMode();
        }

        public List<string> GetDropDownMCH1(String countSheet)
        {
            return settingDAO.GetDropDownMCH1(countSheet);
        }

        public List<string> GetDropDownMCH2(String countSheet, String HeadLevel1)
        {
            return settingDAO.GetDropDownMCH2(countSheet, HeadLevel1);
        }

        public List<string> GetDropDownMCH3(String countSheet, String HeadLevel1, String HeadLevel2)
        {
            return settingDAO.GetDropDownMCH3(countSheet, HeadLevel1, HeadLevel2);
        }

        public List<string> GetDropDownMCH4(String countSheet, String HeadLevel1, String HeadLevel2, String HeadLevel3)
        {
            return settingDAO.GetDropDownMCH4(countSheet, HeadLevel1, HeadLevel2, HeadLevel3);
        }

        public List<string> GetDropDownCountSheetSKU(string plant)
        {
            return settingDAO.GetDropDownCountSheetSKU(plant);
        }

        public List<string> GetDropDownAllCountSheetSKU()
        {
            return settingDAO.GetDropDownAllCountSheetSKU();
        }

        public List<string> GetAllPlant()
        {
            return settingDAO.GetAllPlant();
        }

        public List<string> GetPlant(string countsheet)
        {
            return settingDAO.GetPlant(countsheet);
        }

    }
}
