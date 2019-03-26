using FSBT_HHT_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_DAL.DAO
{
    public class ConfigFileFormatDAO
    {
        private LogErrorDAO logBll = new LogErrorDAO(); 
        public ConfigFileFormat GetConfigFileFormat(int FileID)
        {
            ConfigFileFormat fileConfig;
            try
            {

                Entities dbContext = new Entities();
                fileConfig = dbContext.ConfigFileFormats.Where(f => f.FileID == FileID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileConfig = null;
            }

            return fileConfig;
        }
        
        
        public List<ConfigFileFormatDetail> GetConfigFileFormatDetail(int FileID)
        {
            List<ConfigFileFormatDetail> fileDetails;
            try
            {

                Entities dbContext = new Entities();
                fileDetails = dbContext.ConfigFileFormatDetails.Where(f => f.FileID == FileID).OrderBy(d=>d.ColumnNo).ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileDetails = new List<ConfigFileFormatDetail>();
            }

            return fileDetails;
        }
    }
}
