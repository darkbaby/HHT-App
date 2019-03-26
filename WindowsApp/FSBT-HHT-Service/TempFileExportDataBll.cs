using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using FSBT_HHT_DAL;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_Model;

namespace FSBT_HHT_BLL
{
   
    public class TempFileExportDataBll
    {
        TempFileExportDAO tempExportFileDAO = new TempFileExportDAO();

        public List<TempFileExportDetail> GetTempFileExportDetails()
        {
            return tempExportFileDAO.GetExportFileDetail();
        }
    }
}
