using FSBT_HHT_DAL.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_BLL
{
    public class LogErrorBll
    {
        private LogErrorDAO logDAO = new LogErrorDAO();

        public void LogError(string user, string errorClass, string errorMethod, string exception, DateTime errorDate)
        {
            logDAO.LogError(user, errorClass, errorMethod, exception, errorDate);
        }

        public void LogSystem( string logClass, string logMethod, string exception, DateTime errorDate)
        {
            logDAO.LogSystem(logClass, logMethod, exception, errorDate);
        }

        public void InsertErrorLog(string type)
        {
            logDAO.InsertErrorLog(type);
        }

        public void SetFlagErrorLog()
        {
            logDAO.SetFlagErrorLog();
        }

        public DataTable GetLogError(string flag)
        {
            return logDAO.GetErrorLog(flag);
        }

    }
}
