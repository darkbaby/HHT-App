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
   public class TempFileExportDAO
    {
       private LogErrorDAO logBll = new LogErrorDAO();       
       public List<TempFileExportDetail> GetExportFileDetail()
       {
           List<TempFileExportDetail> fileDetails;
            try
            {
                Entities dbContext = new Entities();
                fileDetails = dbContext.TempFileExportDetails.ToList();
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Successful", DateTime.Now);
            }
            catch(Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileDetails = new List<TempFileExportDetail>();
            }
            return fileDetails;
       }

       public string  InsertExportFileDetail(List<TempFileExportDetail>  details)
       {
           string str = "";
           
           try
           {
               Entities dbContext = new Entities();
               foreach(TempFileExportDetail detail in details)
               {
                    dbContext.TempFileExportDetails.Add(detail);
               }
               dbContext.SaveChanges();
               logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Successful", DateTime.Now);
           }
           catch(Exception ex)
           {
               str = ex.Message;
               logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
           }
           return str;
       }
    }
}
