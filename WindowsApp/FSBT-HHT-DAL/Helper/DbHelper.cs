using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_DAL;
using System.Reflection;
using FSBT_HHT_DAL.DAO;

namespace FSBT_HHT_DAL.Helper
{
    public class DbHelper
    {
        private LogErrorDAO logBll = new LogErrorDAO();  
        /// <summary>
        /// Convert List<T> to DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns>DataTable of List<T>.</returns>
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                Type colType = prop.PropertyType;

                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }

                dataTable.Columns.Add(prop.Name, colType);
            }

            foreach (T item in items)
            {
                DataRow dr = dataTable.NewRow();
                for (int i = 0; i < Props.Length; i++)
                {
                    dr[Props[i].Name] = Props[i].GetValue(item, null) == null ? DBNull.Value : Props[i].GetValue
                        (item, null);
                }
                dataTable.Rows.Add(dr);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        /// <summary>
        /// Get Schema DataTable by SqlQuery
        /// </summary>
        /// <param name="connString">Connection String to Connect Database.</param>
        /// <param name="sqlQuery">SqlQuery to create schema DataTable.</param>
        /// <returns>Schema DataTable.</returns>
        public DataTable GetSchemaDataTable(string connString,string sqlQuery)
        {
            try
            {
                DataTable dt = new DataTable();
                string column = "";
                //SqlConnection conn = new SqlConnection();
                //conn.ConnectionString = connString;
                //conn.Close();
                //SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                //conn.Open();
                //SqlDataReader reader = cmd.ExecuteReader();
                //for (int i = 0; i < reader.FieldCount; i++)
                //{
                //    if (column == "")
                //        column += reader.GetName(i);
                //    else
                //        column += "," + reader.GetName(i);
                //    dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                //}
                //conn.Close();
                //conn.Dispose();

                
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (column == "")
                            column += reader.GetName(i);
                        else
                            column += "," + reader.GetName(i);
                        dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                    }
                    conn.Close();
                    conn.Dispose();
                }
                return dt;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return null;
            }
        }

        /// <summary>
        /// Get data from database to DataTable by SqlQuery
        /// </summary>
        /// <param name="connString">Connection String to Connect Database.</param>
        /// <param name="sqlQuery">SqlQuery to get data</param>
        /// <returns>DataTable of data which get from database.</returns>
        public DataTable GetDataToDataTableBySqlCmd(string connString, string sqlQuery)
        {
            DataTable dt = new DataTable();
            try
            {
                
                dt = GetSchemaDataTable(connString, sqlQuery);
                //SqlConnection conn = new SqlConnection();
                //conn.ConnectionString = connString;
                //conn.Close();
                //SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                //conn.Open();
                //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //adapter.Fill(dt);
                //conn.Close();
                //conn.Dispose();
                //return dt;

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    conn.Close();
                    conn.Dispose();
                }
                return dt;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return dt;
            }
        }

        /// <summary>
        /// Get data from database to DataTable by Table
        /// </summary>
        /// <param name="connString">Connection String to Connect Database.</param>
        /// <param name="tableName">table name in database to get data.</param>
        /// <returns>DataTable of data which get from database.</returns>
        public DataTable GetDataToDataTableByTableName(string connString, string tableName)
        {
            DataTable dt = new DataTable();
            string sqlQuery = "";

            StringBuilder sb = new StringBuilder();
            //SqlConnection conn = new SqlConnection();
            try
            {
                sb.AppendFormat("SELECT * FROM [dbo].[{0}]", tableName);
                sqlQuery = sb.ToString();
                dt = GetSchemaDataTable(connString, sqlQuery);
                
                //conn.ConnectionString = connString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    conn.Close();
                    conn.Dispose();
                }

                return dt;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return dt;
            }
        }

        /// <summary>
        /// Insert all data from DataTable to table in database using SqlBulkCopy
        /// </summary>
        /// <param name="connString">Connection String to Connect Database.</param>
        /// <param name="dtImport">Data of DataTable to import.</param>
        /// <param name="targetTable">Which Table to import.</param>
        /// <returns>Import data successful.</returns>
        public bool InsertDataTableToDatabase(string connString, DataTable dtImport, string targetTable)
        {
            bool success = false;
            using(SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = "[dbo].[" + targetTable + "]";
                    bulkCopy.WriteToServer(dtImport);
                    bulkCopy.Close();
                }
                conn.Close();
                conn.Dispose();
            }
            //bulkCopy.

            return success;
        }
    }
    //public class DbHelper
    //{
    //    private static string connectionString;
    //    private static Entities FsbtEntities;

    //    #region Constructor / Getter
    //    public DbHelper()
    //    {
    //        FsbtEntities = new Entities();
    //    }

    //    public static Entities GetEntities()
    //    {
    //        return FsbtEntities;
    //    }
    //    #endregion

    //    #region Helper method
    //    public static DbRawSqlQuery ExecuteQuery(Type t, string sql, params object[] obj)
    //    {
    //        return FsbtEntities.Database.SqlQuery(t, sql, obj);
    //    }

    //    public static int ExecuteNonQuery(string sql, params object[] obj)
    //    {
    //        return FsbtEntities.Database.ExecuteSqlCommand(sql, obj);
    //    }
    //    #endregion

    //}
}
