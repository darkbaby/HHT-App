using AbstractLib;
using FSBT_HHT_BLL;
using FSBT_HHT_DAL;
using FSBT_HHT_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Download
{
    public class DownloadFile : RemoteAction
    {
        public DownloadFile() { }       
        private TempFileImportBll tempFileImportBll = new TempFileImportBll();
        private ConfigFileFormatBll cfg = new ConfigFileFormatBll();
        private ValidatorBll validatorBll = new ValidatorBll();
        private LogErrorBll logBll = new LogErrorBll();

        string Server = "";
        string Type = "";
        public int numberOfRows { get; set; }
        
        public DownloadFile(string strServer, string strType, string strUsername)
        {
            userName = strUsername;
            Server = strServer;
            Type = strType;

            SystemSettingBll setting = new SystemSettingBll();
            filePathLocal = setting.GetSettingStringByKey(strServer + Type + "LocalPath");
            fileFormat = setting.GetSettingStringByKey(strServer + Type + "FileName");
            filePathRemote = setting.GetSettingStringByKey(strServer + Type + "RemotePath");

            fileHost = setting.GetSettingStringByKey(strServer + Type + "Host");
            fileUserName = setting.GetSettingStringByKey(strServer + Type + "User");
            filePassword = setting.GetSettingStringByKey(strServer + Type + "Password");

            if (Type == "RegularPrice")
                fileBackupSubfix = "_Used";
            else
                fileBackupSubfix = "";

            moveToBackup = setting.GetSettingIntByKey(strServer + Type + "MoveBackup");

            fileExtension = setting.GetSettingStringByKey(strServer + Type + "FileExtension");
            filePathBackup = setting.GetSettingStringByKey(strServer + Type + "BackupPath");

            fileFormatID = setting.GetSettingIntByKey(strServer + Type + "FileFormatID");
            endOfFile = setting.GetSettingStringByKey(strServer + Type + "EOF"); 

            downloadedFiles = new List<DownloadFileModel>();

            fileNames = new List<string>();
            fileNamesFull = new List<Tuple<string, int, int>>();
            fileNamesError = new List<Tuple<string, string>>();

            if(string.IsNullOrEmpty(fileHost) || string.IsNullOrEmpty(fileUserName) || string.IsNullOrEmpty(filePassword))
            {
                errorMeassge = "No setting server in database";
            }
           
        }

        public void DeleteTempData()
        {         
            ConfigFileFormat format = cfg.GetConfigFileFormat(fileFormatID);
            try
            {
                tempFileImportBll.DeleteTempData(format.TempTableName);
                tempFileImportBll.DeleteTempData(format.TempTableLineErrorName);
                tempFileImportBll.DeleteTempData(format.TempTableErrorName);
            }
            catch (Exception ex)
            {
                errorMeassge = ex.Message;
                logBll.LogError(userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        public void DeleteTempError()
        {
            ConfigFileFormat format = cfg.GetConfigFileFormat(fileFormatID);
            try
            {
                tempFileImportBll.DeleteTempData(format.TempTableLineErrorName);
                tempFileImportBll.DeleteTempData(format.TempTableErrorName);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Successful", DateTime.Now);
            }
            catch (Exception ex)
            {
                errorMeassge = ex.Message;
                logBll.LogError(userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        public List<string> ReadTextFile(string filePath)
        {
            numberOfRows = 0;
            errorMeassge = "";
            List<string> dataFromTxt = new List<string>();
            string filename = Path.GetFileName(filePath);
            try
            {
                StreamReader steamReader = new StreamReader(filePath);
               
                //Read data in file
                while (!steamReader.EndOfStream)
                {
                    String rowData = steamReader.ReadLine();
                    if (rowData.Length > 0)
                    {
                        dataFromTxt.Add(rowData);
                    }
                }
                steamReader.Close();
                //steamReader.Dispose();

                int lastData = dataFromTxt.Count - 1;
                int tmpExpectedNumRows = 0;

                //check EOF 
                // ถ้าไม่มี EOF ไม่ทำต่อ
                // ถ้าจำนวนบรรทัดไม่เท่ากับ EOF ไม่ทำต่อ
                if (dataFromTxt[lastData].Contains(endOfFile)) // EOF of SKU, Barcode File
                {
                    numberOfRows = dataFromTxt.Count - 1;

                    string pattern = @"\d";
                    StringBuilder sb = new StringBuilder();
                    foreach (Match m in Regex.Matches(dataFromTxt[lastData], pattern))
                    {
                        sb.Append(m);
                    }
                    string resultString = sb.ToString();

                    try
                    {
                        tmpExpectedNumRows = Convert.ToInt32(resultString);
                    }
                    catch(Exception ex)
                    {
                        errorMeassge = "Mismatch value between EOF and total records";
                        fileNamesError.Add(new Tuple<string, string>(filename,errorMeassge) );
                        fileNamesFull.Add(new Tuple<string, int, int>(filename, 0, numberOfRows));
                        logBll.LogError(userName, this.GetType().Name, filename, errorMeassge, DateTime.Now);
                        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                        return null;
                    }

                    if (numberOfRows != tmpExpectedNumRows)
                    {
                        errorMeassge = "Mismatch value between EOF and total records";
                        fileNamesError.Add(new Tuple<string, string>(filename, errorMeassge));
                        fileNamesFull.Add(new Tuple<string, int, int>(filename, 0, numberOfRows));
                        logBll.LogError(userName, this.GetType().Name, filename, errorMeassge, DateTime.Now);
                        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, errorMeassge, DateTime.Now);
                        return null;
                    }
                    dataFromTxt.RemoveAt(lastData);
                }
                else
                {
                    numberOfRows = dataFromTxt.Count;
                    errorMeassge = "Not found EOF in source file";
                    fileNamesError.Add(new Tuple<string, string>(filename, errorMeassge));
                    fileNamesFull.Add(new Tuple<string, int, int>(filename, dataFromTxt.Count, dataFromTxt.Count));
                    logBll.LogError(userName, this.GetType().Name, filename, errorMeassge, DateTime.Now);
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, errorMeassge, DateTime.Now);
                    return null;
                }
            }
            catch (Exception ex)
            {
                errorMeassge = ex.Message;
                fileNamesError.Add(new Tuple<string, string>(filename, "Not found EOF in source file"));
                fileNamesFull.Add(new Tuple<string, int, int>(filename, 0, 0));
                logBll.LogError(userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Not found EOF in source file", DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return null;
            }
            logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Successful", DateTime.Now);
            return dataFromTxt;
        }

        public DataSet ConvertToDataSet(List<string> textFromFile, string filename)
        {
            errorMeassge = "";
            try
            {
                DataSet ds = new DataSet();

                DataTable tblDetail = new DataTable();
                DataTable tblError = new DataTable();
                DataTable tblLineError = new DataTable();

                ConfigFileFormat format = cfg.GetConfigFileFormat(fileFormatID);

                var detailColumnName = tempFileImportBll.GetColumnName(format.TempTableName);
                var errorColumnName = tempFileImportBll.GetColumnName(format.TempTableErrorName);
                var lineErrorColumnName = tempFileImportBll.GetColumnName(format.TempTableLineErrorName);

                for (int i = 0; i < detailColumnName.Length; i++)
                {
                    tblDetail.Columns.Add(new DataColumn(detailColumnName[i]));
                }

                for (int i = 0; i < errorColumnName.Length; i++)
                {
                    tblError.Columns.Add(new DataColumn(errorColumnName[i]));
                }

                for (int i = 0; i < lineErrorColumnName.Length; i++)
                {
                    tblLineError.Columns.Add(new DataColumn(lineErrorColumnName[i]));
                }

                int dID = 0;
                //int eID = 0;
                dID = tempFileImportBll.GetMaxIDTempTableDetail(format.TempTableName);
                char seperator = '|';

                if (Type.ToUpper() == "SKU" || Type.ToUpper() == "BARCODE")
                    seperator = '|';
                else seperator = ',';

                foreach (string line in textFromFile)
                {
                    string[] element = line.Split(seperator);

                    //เช็คจำนวน Column ว่าเท่ากันจริงไหม
                    if (element.Length == (detailColumnName.Length - 4))
                    {
                        dID++;
                        DataRow dr = tblDetail.NewRow();
                        dr[0] = dID;
                        for (int i = 1; i <= element.Length; i++)
                        {
                            dr[i] = element[i - 1].Trim();
                        }
                        dr[element.Length + 1] = Assembly.GetExecutingAssembly().GetName().Name;
                        dr[element.Length + 2] = DateTime.Now;
                        dr[element.Length + 3] = filename;
                        tblDetail.Rows.Add(dr);
                    }
                    else
                    {
                        dID++;
                        DataRow ndr = tblLineError.NewRow();
                        ndr[0] = dID;
                        ndr[1] = line;
                        ndr[2] = "Number of column in file more than database";
                        ndr[3] = filename;
                        ndr[4] = Assembly.GetExecutingAssembly().GetName().Name;
                        ndr[5] = DateTime.Now;
                        tblLineError.Rows.Add(ndr);
                    }
                }

                //เช็คจำนวน data type และ เช็ค length
                validatorBll.ValidateDataField(fileFormatID, tblDetail);
                DataTable dataResult = validatorBll.DataResult;
                DataTable errorResult = validatorBll.ErrorResult;
                DataTable tmpDataResult = validatorBll.DataResult;

                ds.Tables.Add(dataResult);
                ds.Tables.Add(errorResult);
                ds.Tables.Add(tblLineError);

                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Successful", DateTime.Now);
                return ds;
            }
            catch (Exception ex)
            {
                errorMeassge = ex.Message;
                DataSet ds = new DataSet();
                DataTable tblDetail = new DataTable();
                DataTable tblError = new DataTable();
                DataTable tblLineError = new DataTable();

                ds.Tables.Add(tblDetail);
                ds.Tables.Add(tblError);
                ds.Tables.Add(tblLineError);

                logBll.LogError(userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);

                return ds;
            }
        }

        public void InsertDataToTempTable(List<string> txtFromFile, string fileformat, string filename)
        {
            try
            {
                errorMeassge = "";
                ConfigFileFormat format = cfg.GetConfigFileFormat(fileFormatID);
                //Validate Data , DataType , Length
                DataSet masterData = ConvertToDataSet(txtFromFile, filename);
                
                DataTable detailData = masterData.Tables[0];
                DataTable errorData = masterData.Tables[1];
                DataTable lineErrorData = masterData.Tables[2];

                if (lineErrorData.Rows.Count == 0 && errorData.Rows.Count == 0)
                {
                    tempFileImportBll.InsertDataTableToDatabase(detailData, format.TempTableName);
                }
                else if (errorData.Rows.Count > 0)
                {
                    tempFileImportBll.InsertDataTableToDatabase(errorData, format.TempTableErrorName);

                    foreach (DataRow row in errorData.Rows)
                    {
                        string error = row["Error"].ToString();
                        fileNamesError.Add(new Tuple<string, string>(filename, error));
                    }
                    errorMeassge = "File Error";
                }
                else if (lineErrorData.Rows.Count > 0)
                {
                    tempFileImportBll.InsertDataTableToDatabase(lineErrorData, format.TempTableLineErrorName);
                    foreach (DataRow row in errorData.Rows)
                    {
                        string error = row["Error"].ToString();
                        fileNamesError.Add(new Tuple<string, string>(filename, error));
                    }
                    errorMeassge = "File Error";
                }
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Successful", DateTime.Now);
            }
            catch (Exception ex)
            {
                errorMeassge = ex.Message;
                logBll.LogError(userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        public void MoveFileToArchive(List<string> filenames)
        {
            errorMeassge = "";
            errorMeassge =  MoveFileToArchives(filenames);
        }
    }
}
