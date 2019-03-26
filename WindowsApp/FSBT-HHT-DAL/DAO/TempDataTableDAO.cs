using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_DAL.DAO
{
    public class TempDataTableDAO
    {
        public DataTable GetTempDataTable(string TableName)
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from " + TableName, conn);
                SqlDataAdapter dtAdapter = new SqlDataAdapter();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 900;

                dtAdapter.Fill(dt);

                conn.Close();
            }
            return dt;
        }

        public DataTable GetExportDataTableByCountsheet(string TableName, string countsheet)
        {
            Entities dbContext = new Entities();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from " + TableName + " where PIDoc = '" + countsheet + "'", conn);
                SqlDataAdapter dtAdapter = new SqlDataAdapter();
                cmd.CommandTimeout = 900;
                dtAdapter.SelectCommand = cmd;
                dtAdapter.Fill(dt);
                conn.Close();
            }

            return dt;
        }

        public DataTable ExecStoredProcedure(string StoredProcedureName, List<string>  param )
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
    }
}
