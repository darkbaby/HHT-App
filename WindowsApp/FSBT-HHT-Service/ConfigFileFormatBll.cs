using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using FSBT_HHT_DAL;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_Model;


namespace FSBT_HHT_BLL
{
    public class ConfigFileFormatBll
    {
        ConfigFileFormatDAO configFileFormatDAO = new ConfigFileFormatDAO();

        public ConfigFileFormat GetConfigFileFormat(int FileID)
        {
            return configFileFormatDAO.GetConfigFileFormat(FileID);
        }

        public List<ConfigFileFormatDetail> GetConfigFileFormatDetail(int FileID)
        {
            return configFileFormatDAO.GetConfigFileFormatDetail(FileID);
        }

    }
}
