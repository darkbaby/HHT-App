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
    public class TempFileAcknowledgeDAO
    {

        private LogErrorDAO logBll = new LogErrorDAO(); 

        public List<TempFileAcknowledgeSKUDetail> GetAcknowledgeSKUFileDetail()
        {
            List<TempFileAcknowledgeSKUDetail> fileDetails;
            try
            {
                Entities dbContext = new Entities();
                fileDetails = dbContext.TempFileAcknowledgeSKUDetails.ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileDetails = new List<TempFileAcknowledgeSKUDetail>();
            }

            return fileDetails;

        }

        public string InsertAcknowledgeSKUFileDetail(List<TempFileAcknowledgeSKUDetail> details)
        {
            string str = "";

            try
            {
                Entities dbContext = new Entities();
                foreach (TempFileAcknowledgeSKUDetail detail in details)
                {
                    dbContext.TempFileAcknowledgeSKUDetails.Add(detail);
                }
                dbContext.SaveChanges();


            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }


        public string ClearAcknowledgeSKUFileDetail()
        {
            string str = "";

            try
            {
              

                Entities dbContext = new Entities();
                List<TempFileAcknowledgeSKUDetail> fileDetails;
                fileDetails = dbContext.TempFileAcknowledgeSKUDetails.ToList();
                //foreach (TempFileAcknowledgeDetail detail in fileDetails)
                //{
                //    dbContext.TempFileAcknowledgeDetails.Add(detail);
                //}
                dbContext.TempFileAcknowledgeSKUDetails.RemoveRange(fileDetails);
                dbContext.SaveChanges();


            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }



        //BarCode

        public List<TempFileAcknowledgeBarDetail> GetAcknowledgeBarFileDetail()
        {
            List<TempFileAcknowledgeBarDetail> fileDetails;
            try
            {
                Entities dbContext = new Entities();
                fileDetails = dbContext.TempFileAcknowledgeBarDetails.ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                fileDetails = new List<TempFileAcknowledgeBarDetail>();
            }

            return fileDetails;

        }

        public string InsertAcknowledgeBarFileDetail(List<TempFileAcknowledgeBarDetail> details)
        {
            string str = "";

            try
            {
                Entities dbContext = new Entities();
                foreach (TempFileAcknowledgeBarDetail detail in details)
                {
                    dbContext.TempFileAcknowledgeBarDetails.Add(detail);
                }
                dbContext.SaveChanges();


            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }


        public string ClearAcknowledgeBarFileDetail()
        {
            string str = "";

            try
            {


                Entities dbContext = new Entities();
                List<TempFileAcknowledgeBarDetail> fileDetails;
                fileDetails = dbContext.TempFileAcknowledgeBarDetails.ToList();
                //foreach (TempFileAcknowledgeDetail detail in fileDetails)
                //{
                //    dbContext.TempFileAcknowledgeDetails.Add(detail);
                //}
                dbContext.TempFileAcknowledgeBarDetails.RemoveRange(fileDetails);
                dbContext.SaveChanges();


            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }
    }
}
