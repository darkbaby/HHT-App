using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Globalization;
using System.Reflection;

namespace FSBT_HHT_BLL
{
    public class FilesUtilBll
    {
        private LogErrorBll logBll = new LogErrorBll(); 
        public  double ConvertUnit(long fileSizeByte, string unit)
        {
            double output = fileSizeByte;
            string[] sizes = { "B", "KB", "MB", "GB" };
            int results = Array.IndexOf(sizes, unit.ToUpper());
            output = fileSizeByte / (Math.Pow(1024, results));
            return output;
        }
        /// <summary>
        /// CreateZipFile.
        /// </summary>
        /// <param name="fileList">list of path file to zip.</param>
        /// <param name="destinationFile">path zip file to create.</param>
        /// <param name="overwrite"></param>
        /// <returns>Create zip file successful.</returns>
        public  bool CreateZipFile(List<string> fileList, string destinationFile, bool overwrite)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(destinationFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));

                }
                if (File.Exists(destinationFile) && overwrite)
                {
                    File.Delete(destinationFile);
                }
                var zip = ZipFile.Open(destinationFile, ZipArchiveMode.Create);

                foreach (string path in fileList)
                {
                    zip.CreateEntryFromFile(path, Path.GetFileName(path), CompressionLevel.Fastest);
                }
                zip.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }

        }
        public  List<string> ExtractZip(string sourceFile, string destinationFolder, bool overwrite)
        {
            List<string> fileList = new List<string>();
            try
            {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);

                }
                using (ZipArchive archive = ZipFile.OpenRead(sourceFile))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        entry.ExtractToFile(Path.Combine(destinationFolder, entry.FullName), overwrite);
                        fileList.Add(Path.Combine(destinationFolder, entry.FullName));
                    }
                }
                return fileList;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return null;
            }
        }
        public  long GetFileSize(string path)
        {
            FileInfo info = new FileInfo(path);
            long fileSize = info.Length;
            return fileSize;
        }

        public  bool IsFileOverLimit(string path, long limitSize)
        {
            bool output = false;
            FileInfo info = new FileInfo(path);
            long fileSize = info.Length;
            if (fileSize > limitSize) output = true;
            return output;
        }

        public  bool CopyFileFromFile(string sourceFile, string targetFile)
        {
            bool msg = false;
            // To move a file or folder to a new location:
            try
            {
                System.IO.File.Copy(sourceFile, targetFile, true);
                msg = true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                Console.WriteLine(ex.Message);
                msg = false;
            }
            return msg;
        }

        public  bool DeleteFile(string pathFIle)
        {
            bool msg = false;
            if (System.IO.File.Exists(pathFIle))
            {
                try
                {
                    System.IO.File.Delete(pathFIle);
                    msg = true;
                }
                catch (Exception ex)
                {
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    Console.WriteLine(ex.Message);
                    msg = false;
                }
            }
            else
            {
                msg = false;
            }
            return msg;
        }
        /// <summary>
        /// Create Directory by path string.
        /// </summary>
        /// <param name="path">path to create directory.</param>
        /// <returns>create directory successful.</returns>
        public  bool CreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        /// <summary>
        /// Delete directory, all sub directory and file in directory/sub directory.
        /// </summary>
        /// <param name="path">path directory to delete.</param>
        /// <returns>Delete directory successful.</returns>
        public  bool DeleteDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        /// <summary>
        /// Export data to Text File/CSV File from DataTable.
        /// </summary>
        /// <param name="Fullpath">Directory path + name of file + extension file Example C:\File\csv01.csv.</param>
        /// <param name="data">DataTable which to export.</param>
        /// <param name="delimiter">delimiter Example "," , "|".</param>
        /// <param name="isHeader">First row is Header column.</param>
        /// <param name="isDoubleQuote"> add DoubleQuote each column data.</param>
        /// <returns>Export Text File successful.</returns>
        public  bool ExportTextFileFromDataTable(string Fullpath, DataTable data, string delimiter, bool isHeader, bool isDoubleQuote)
        {
            FileStream FileOut = null;
            string path = Path.GetDirectoryName(Fullpath) + "//";
            string fileName = Path.GetFileName(Fullpath);
            try
            {
                CreateDirectory(path);

                FileOut = new FileStream(path + fileName, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(FileOut);

                DataTable dt = new DataTable();
                dt = data;

                StringBuilder sb = new StringBuilder();
                if (isHeader)
                {
                    IEnumerable<string> columnNames = null;
                    if (isDoubleQuote)
                    {
                        columnNames = dt.Columns.Cast<DataColumn>().Select(column => "\"" + column.ColumnName + "\"");
                    }
                    else
                    {
                        columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                    }
                    sb.AppendLine(string.Join(delimiter, columnNames));
                }


                foreach (DataRow row in dt.Rows)
                {
                    IEnumerable<string> fields = null;          
                    if (isDoubleQuote)
                    {
                        fields = row.ItemArray.Select(field => "\"" + field.ToString().Replace("\"", "").Replace(",", " ").Replace("'","") + "\"");
                    }
                    else
                    {
                        fields = row.ItemArray.Select(field => field.ToString().Replace("\"", "").Replace(",", "|").Replace("'",""));
                    }
                    sb.AppendLine(string.Join(delimiter, fields));
                }
                sw.Write(sb.ToString());
                sw.Close();
                FileOut.Close();


                return true;
            }
            catch (Exception ex)
            {
                if (FileOut != null)
                {
                    FileOut.Close();
                    File.Delete(path + fileName);
                }
                return false;
            }

        }

        /// <summary>
        /// Get data from Text File to DataTable.
        /// </summary>
        /// <param name="Fullpath">Directory of Text file to get data Example C:\File\csv01.csv.</param>
        /// <param name="delimiter">Delimiter in text file Example "," , "|".</param>
        /// <param name="isHeader">First row in text is header.</param>
        /// <param name="isDoubleQuote">Each column has DoubleQuote.</param>
        /// <returns>DataTable of data from Text File.</returns>
        public  DataTable GetDataTableFromTextFile(string Fullpath, string delimiter, bool isHeader, bool isDoubleQuote)
        {
            StreamReader steamReader = new StreamReader(Fullpath);
            try
            {
                DataTable datable = new DataTable();

                DateTime sysDate = DateTime.Now;

                int rowCount = 0;
                String[] columnName = null;
                String[] dataValue = null;

                while (!steamReader.EndOfStream)
                {
                    String rowData = steamReader.ReadLine().Trim();
                    if (rowData.Length > 0)
                    {
                        dataValue = rowData.Split(delimiter.ToCharArray());
                        if (rowCount == 0)
                        {
                            if (isHeader)
                            {
                                columnName = dataValue;
                            }
                            else
                            {
                                columnName = new String[dataValue.Length];
                                rowCount = 1;
                                foreach (String csvColumn in columnName)
                                {
                                    DataColumn dataColumn = new DataColumn("", typeof(String));
                                    dataColumn.DefaultValue = String.Empty;
                                    datable.Columns.Add(dataColumn);
                                }
                            }
                        }
                        if (rowCount == 0)
                        {
                            rowCount = 1;
                            foreach (String csvColumn in columnName)
                            {
                                DataColumn dataColumn = null;
                                if (isDoubleQuote)
                                {
                                    dataColumn = new DataColumn(csvColumn.Replace("\"", ""), typeof(String));
                                }
                                else
                                {
                                    dataColumn = new DataColumn(csvColumn, typeof(String));
                                }
                                dataColumn.DefaultValue = String.Empty;
                                datable.Columns.Add(dataColumn);
                            }
                        }
                        else
                        {
                            DataRow dataRow = datable.NewRow();
                            for (int i = 0; i < columnName.Length; i++)
                            {

                                if (!string.IsNullOrEmpty(dataValue[i].ToString()))
                                {
                                    if (isDoubleQuote)
                                    {
                                        dataRow[i] = dataValue[i].ToString().Replace("\"", "");
                                    }
                                    else
                                    {
                                        dataRow[i] = dataValue[i].ToString();
                                    }
                                }
                                else
                                {
                                    dataRow[i] = null;
                                }
                            }
                            datable.Rows.Add(dataRow);
                        }
                    }

                }
                steamReader.Close();
                steamReader.Dispose();

                return datable;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);

                steamReader.Close();
                steamReader.Dispose();
                return null;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                steamReader.Close();
                steamReader.Dispose();
                return null;
            }
        }

        /// <summary>
        /// Get data from Text File to DataTable with Schema DataTable.
        /// </summary>
        /// <param name="dtSource">Schema DataTable.</param>
        /// <param name="Fullpath">Directory of Text file to get data Example C:\File\csv01.csv.</param>
        /// <param name="delimiter">Delimiter in text file Example "," , "|".</param>
        /// <param name="isHeader">First row in text is header.</param>
        /// <param name="isDoubleQuote">Each column has DoubleQuote.</param>
        /// <returns>DataTable of data from Text File.</returns>
        public  DataTable GetDataTableFromTextFile(DataTable dtSource, string Fullpath, string delimiter, bool isHeader, bool isDoubleQuote)
        {
            StreamReader steamReader = new StreamReader(Fullpath);
            try
            {
                DataTable datable = new DataTable();
                datable = dtSource;
                int rowCount = 0;
                String[] dataValue = null;

                while (!steamReader.EndOfStream)
                {
                    String rowData = steamReader.ReadLine().Trim();
                    if (rowData.Length > 0)
                    {
                        dataValue = rowData.Split(delimiter.ToCharArray());
                        if (rowCount == 0)
                        {
                            rowCount = 1;
                        }
                        else
                        {
                            DataRow dataRow = datable.NewRow();
                            for (int i = 0; i < datable.Columns.Count; i++)
                            {

                                if (!string.IsNullOrEmpty(dataValue[i].ToString()))
                                {
                                    if (isDoubleQuote)
                                    {
                                        if (datable.Columns[i].DataType.Name == typeof(DateTime).Name)
                                        {
                                            DateTime date;
                                            if (DateTime.TryParse(dataValue[i].ToString().Replace("\"", ""), CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                            {
                                                dataRow[i] = date;
                                            }
                                            else
                                            {
                                                dataRow[i] = DBNull.Value;
                                            }
                                        }
                                        else if (datable.Columns[i].DataType.Name == typeof(int).Name)
                                        {
                                            int intValue;
                                            if (int.TryParse(dataValue[i].ToString().Replace("\"", ""), out intValue))
                                            {
                                                dataRow[i] = intValue;
                                            }
                                            else
                                            {
                                                dataRow[i] = DBNull.Value;
                                            }
                                        }
                                        else if (datable.Columns[i].DataType.Name == typeof(Boolean).Name)
                                        {
                                            bool boolValue = false;
                                            if (Boolean.TryParse(dataValue[i].ToString().Replace("\"", ""), out boolValue))
                                            {
                                                dataRow[i] = boolValue;
                                            }
                                            else
                                            {
                                                dataRow[i] = boolValue;
                                            }
                                        }
                                        else if (datable.Columns[i].DataType.Name == (typeof(float).Name) ||
                                                 datable.Columns[i].DataType.Name == (typeof(double).Name) ||
                                                datable.Columns[i].DataType.Name == (typeof(decimal).Name))
                                        {
                                            double doubleValue = 0.00;
                                            if (double.TryParse(dataValue[i].ToString().Replace("\"", ""), out doubleValue))
                                            {
                                                dataRow[i] = doubleValue;
                                            }
                                            else
                                            {
                                                dataRow[i] = DBNull.Value;
                                            }
                                        }
                                        else
                                        {
                                            dataRow[i] = dataValue[i].ToString().Replace("\"", "");
                                        }
                                    }
                                    else
                                    {
                                        if (datable.Columns[i].DataType.Name == typeof(DateTime).Name)
                                        {
                                            DateTime date;
                                            if (DateTime.TryParseExact(dataValue[i].ToString(), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                            {
                                                dataRow[i] = date;
                                            }
                                            else
                                            {
                                                dataRow[i] = DBNull.Value;
                                            }
                                        }
                                        else if (datable.Columns[i].DataType.Name == typeof(int).Name)
                                        {
                                            int intValue;
                                            if (int.TryParse(dataValue[i].ToString(), out intValue))
                                            {
                                                dataRow[i] = intValue;
                                            }
                                            else
                                            {
                                                dataRow[i] = DBNull.Value;
                                            }
                                        }
                                        else if (datable.Columns[i].DataType.Name == typeof(Boolean).Name)
                                        {
                                            bool boolValue = false;
                                            if (Boolean.TryParse(dataValue[i].ToString(), out boolValue))
                                            {
                                                dataRow[i] = boolValue;
                                            }
                                            else
                                            {
                                                dataRow[i] = boolValue;
                                            }
                                        }
                                        else if (datable.Columns[i].DataType.Name == (typeof(float).Name) ||
                                                 datable.Columns[i].DataType.Name == (typeof(double).Name) ||
                                                datable.Columns[i].DataType.Name == (typeof(decimal).Name))
                                        {
                                            double doubleValue = 0.00;
                                            if (double.TryParse(dataValue[i].ToString(), out doubleValue))
                                            {
                                                dataRow[i] = doubleValue;
                                            }
                                            else
                                            {
                                                dataRow[i] = DBNull.Value;
                                            }
                                        }
                                        else
                                        {
                                            dataRow[i] = dataValue[i].ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    dataRow[i] = null;
                                }
                            }
                            datable.Rows.Add(dataRow);
                        }
                    }
                }
                steamReader.Close();
                steamReader.Dispose();

                return datable;
            }
            catch (FileNotFoundException ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                Console.WriteLine(ex.Message);

                steamReader.Close();
                steamReader.Dispose();
                return null;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                steamReader.Close();
                steamReader.Dispose();
                return null;
            }
        }
    }
}
