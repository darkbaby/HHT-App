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

        public string UpdateSetting(SystemSettingModel newDateSetting)
        {
            string result = settingDAO.UpdateSettingData(newDateSetting);
            return result;
        }
        
        public string GetComputerID()
        {
            return settingDAO.GetComputerID();
        }

        public bool SaveModeRealtime(bool isRealtime)
        {
            return settingDAO.saveModeRealtime(isRealtime);
        }

        public bool GetRealTimeMode()
        {
            return settingDAO.GetRealTimeMode();
        }
    }
}
