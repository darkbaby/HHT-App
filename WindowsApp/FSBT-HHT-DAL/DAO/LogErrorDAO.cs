using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_DAL.DAO
{
    public class LogErrorDAO
    {
        Entities dbContext = new Entities();
        public void LogError(string user, string errorClass, string errorMethod, string exception, DateTime errorDate)
        {
            LogError er = new LogError();
            er.Username = user;
            er.ErrorClass = errorClass;
            er.ErrorMethod = errorMethod;
            er.Exception = exception;
            er.ErrorDate = errorDate;
            er.New = true;
            dbContext.LogErrors.Add(er);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                LogError(user, "LogErrorDAO", "LogError", ex.Message, DateTime.Now);
            }
        }

        public void LogSystem(string logClass, string logMethod, string exception, DateTime logDate)
        {
            LogSystem er = new LogSystem();

            er.Class = logClass;
            er.Method = logMethod;
            er.Exception = exception;
            er.CreateDate = logDate;
            dbContext.LogSystems.Add(er);
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogSystem("LogErrorDAO", "LogSystem", ex.Message, DateTime.Now);
            }
        }

        public DataTable ExecStoredProcedure(string StoredProcedureName, List<string> param)
        {
            Entities dbContext = new Entities();

            string strParam = "";
            foreach (string str in param)
            {
                strParam += "'" + str + "',";
            }
            strParam = strParam.TrimEnd(new char[] { ',' });

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("exec " + StoredProcedureName + " " + strParam, conn);
                SqlDataAdapter dtAdapter = new SqlDataAdapter();

                cmd.CommandTimeout = 900;

                dtAdapter.SelectCommand = cmd;
                dtAdapter.Fill(dt);

                conn.Close();
            }

            return dt;
        }

        public void InsertErrorLog(string type)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SCR01_SP_InsertLogError", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@type", SqlDbType.VarChar).Value = type;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                LogSystem("LogErrorDAO", "InsertErrorLog", ex.Message, DateTime.Now);
            }
        }

        public void SetFlagErrorLog()
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SCR01_SP_SetLogErrorFlag", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;       
                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                LogSystem("LogErrorDAO", "SetErrorLogFlag", ex.Message, DateTime.Now);
            }
        }

        public DataTable GetErrorLog(string flag)
        {
            Entities dbContext = new Entities();
            DataTable resultTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SCR01_SP_GetLogError", conn);
                    SqlDataAdapter dtAdapter = new SqlDataAdapter();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@flag", SqlDbType.VarChar).Value = flag;

                    cmd.CommandTimeout = 900;

                    dtAdapter.SelectCommand = cmd;
                    dtAdapter.Fill(resultTable);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                LogSystem("LogErrorDAO", "GetErrorLog", ex.Message, DateTime.Now);
            }

            return resultTable;
        }

    }
}
