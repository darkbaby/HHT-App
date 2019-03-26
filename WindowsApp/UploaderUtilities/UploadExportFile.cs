using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FSBT_HHT_BLL;
using FSBT_HHT_DAL;
using SFTP;
using AbstractLib;
using System.Reflection;

namespace ExportFileUploader
{
    public class UploadExportFile  : RemoteAction
    {
        private LogErrorBll logBll = new LogErrorBll();

        public UploadExportFile(string username)
        {
            SystemSettingBll setting = new SystemSettingBll();

            userName = username;

            filePathLocal = setting.GetSettingStringByKey("SFTPExportLocalPath");
            fileName = setting.GetSettingStringByKey("SFTPExportFileName");
            fileFormat = setting.GetSettingStringByKey("SFTPExportFileName");
            filePathRemote = setting.GetSettingStringByKey("SFTPExportRemotePath");
            filePathBackup = setting.GetSettingStringByKey("SFTPExportBackupPath");

            fileHost = setting.GetSettingStringByKey("SFTPExportHost");
            fileUserName = setting.GetSettingStringByKey("SFTPExportUser");
            filePassword = setting.GetSettingStringByKey("SFTPExportPassword");

            moveToBackup = setting.GetSettingIntByKey("SFTPExportMoveBackup");
            fileFormatID = setting.GetSettingIntByKey("SFTPExportFileFormatID");

            string branchCode = setting.GetSettingStringByKey("BranchCode");

            fileNames = new List<string>();
            fileNamesFull = new List<Tuple<string, int, int>>();
            fileNamesError = new List<Tuple<string, string>>();

            if (string.IsNullOrEmpty(fileHost) || string.IsNullOrEmpty(fileUserName) || string.IsNullOrEmpty(filePassword))
            {
                errorMeassge = "No setting server in database";
            }
        }
        
        public string GenerateFile()
        {
            string str = "";
            try 
            { 
                TempFileExportDataBll bll = new TempFileExportDataBll();
                List<TempFileExportDetail> details = bll.GetTempFileExportDetails();

                ConfigFileFormatBll cfg = new ConfigFileFormatBll();
                ConfigFileFormat format = cfg.GetConfigFileFormat(fileFormatID);

                TempDataTableBll tempTableBll = new TempDataTableBll();

                var countSheets = (from d in details select d.PIDoc).Distinct();

                if (countSheets.Count() > 0 )
                {
                    if (!Directory.Exists(filePathLocal))
                    {
                        Directory.CreateDirectory(filePathLocal);
                    } 

                    foreach (var cs in countSheets)
                    {
                        DataTable dtCountsheet = tempTableBll.GetExportDataTableByCountsheet(format.TempTableName, cs);
                        string Plant = dtCountsheet.Rows[0]["Plant"] != null ? dtCountsheet.Rows[0]["Plant"].ToString() : "";
                        string filename = fileName;
                        filename = filename.Replace("{Date}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                        filename = filename.Replace("{Plant}", Plant);
                        filename = filename.Replace("{CountSheet}", cs);
                        Encoding utf8WithoutBom = new UTF8Encoding(false);

                        using (StreamWriter sw = new StreamWriter(filePathLocal + "/" + filename, false, utf8WithoutBom))
                        {
                            int i = 0;
                            string contentStr = "";
                            foreach (DataRow dr in dtCountsheet.Rows)
                            {
                                contentStr = "";
                                for (int j = 2; j <= dtCountsheet.Columns.Count - 3; j++)
                                {
                                    contentStr += dr[j].ToString() + "|";
                                }
                                contentStr = contentStr.TrimEnd(new char[] { '|' });
                                sw.WriteLine(contentStr);
                                sw.Flush();
                                i++;
                            }
                            sw.WriteLine("EOF|" + i);
                            sw.Flush();

                            sw.Close();
                            fileNames.Add(filename);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                str = "Fail:"+ex.Message;
                logBll.LogError(userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }

            if (string.IsNullOrEmpty(str))
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Successful", DateTime.Now);
            }
            else
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
            }

            return str;
        }

        public string GenerateFile(string filename)
        {
            string str = "";
            try
            {
                TempFileExportDataBll bll = new TempFileExportDataBll();
                List<TempFileExportDetail> details = bll.GetTempFileExportDetails();

                ConfigFileFormatBll cfg = new ConfigFileFormatBll();
                ConfigFileFormat format = cfg.GetConfigFileFormat(fileFormatID);

                TempDataTableBll tempTableBll = new TempDataTableBll();

                var countSheets = (from d in details select d.PIDoc).Distinct();

                if (countSheets.Count() > 0)
                {
                    foreach (var cs in countSheets)
                    {
                        DataTable dtCountsheet = tempTableBll.GetExportDataTableByCountsheet(format.TempTableName, cs);
                        string Plant = dtCountsheet.Rows[0]["Plant"] != null ? dtCountsheet.Rows[0]["Plant"].ToString() : "";
                        Encoding utf8WithoutBom = new UTF8Encoding(false);

                        using (StreamWriter sw = new StreamWriter(filename, false, utf8WithoutBom))
                        {
                            int i = 0;
                            string contentStr = "";
                            foreach (DataRow dr in dtCountsheet.Rows)
                            {
                                contentStr = "";
                                for (int j = 2; j <= dtCountsheet.Columns.Count - 3; j++)
                                {
                                    contentStr += dr[j].ToString() + "|";
                                }
                                contentStr = contentStr.TrimEnd(new char[] { '|' });
                                sw.WriteLine(contentStr);
                                sw.Flush();
                                i++;
                            }
                            sw.WriteLine("EOF|" + i);
                            sw.Flush();

                            sw.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                str = "Fail:" + ex.Message;
                logBll.LogError(userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }

            if (string.IsNullOrEmpty(str))
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Successful", DateTime.Now);
            }
            else
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
            }

            return str;
        }
    }
}
