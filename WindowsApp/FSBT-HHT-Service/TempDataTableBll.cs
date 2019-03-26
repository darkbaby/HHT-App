using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using FSBT_HHT_DAL;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_Model;

namespace FSBT_HHT_BLL
{
    public class TempDataTableBll
    {
        TempDataTableDAO tempDataTableDAO = new TempDataTableDAO();
        public DataTable GetTempDataTable(string TableName)
        {
            return tempDataTableDAO.GetTempDataTable(TableName);
        }

        public DataTable  CallMasterErrorReport(string  Type)
        {
            DataTable dt = new DataTable();
            List<string> param = new List<string>();
            param.Add(Type);

            dt = tempDataTableDAO.ExecStoredProcedure("SCR01_SP_LastestErrorReport", param);

            return dt;
        }

        public DataTable GetExportDataTableByCountsheet(string TableName, string countsheet)
        {
            return tempDataTableDAO.GetExportDataTableByCountsheet(TableName, countsheet);
        }
    }
}
