using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using FSBT_HHT_DAL;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_Model;

namespace FSBT_HHT_BLL
{
    public class TempFileAcknowledgeBll
    {
        TempFileAcknowledgeDAO tempAcknowledgeFileDAO = new TempFileAcknowledgeDAO();

        public List<TempFileAcknowledgeSKUDetail> GetTempFileAcknowledgeSKUDetails()
        {
            return tempAcknowledgeFileDAO.GetAcknowledgeSKUFileDetail();
        }

        public string InsertAcknowledgeSKUFileDetail(List<TempFileAcknowledgeSKUDetail> details)
        {
            return tempAcknowledgeFileDAO.InsertAcknowledgeSKUFileDetail(details);
        }


        public  string ClearAcknowledgeSKUFileDetail()
        {
            return tempAcknowledgeFileDAO.ClearAcknowledgeSKUFileDetail();
        }



        public List<TempFileAcknowledgeBarDetail> GetTempFileAcknowledgeBarDetails()
        {
            return tempAcknowledgeFileDAO.GetAcknowledgeBarFileDetail();
        }

        public string InsertAcknowledgeBarFileDetail(List<TempFileAcknowledgeBarDetail> details)
        {
            return tempAcknowledgeFileDAO.InsertAcknowledgeBarFileDetail(details);
        }


        public string ClearAcknowledgeBarFileDetail()
        {
            return tempAcknowledgeFileDAO.ClearAcknowledgeBarFileDetail();
        }
    }
}
