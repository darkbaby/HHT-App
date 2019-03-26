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
using System.ComponentModel;
using System.Reflection;

namespace AcknowledgeUploader
{
    public class UploadAcknowledgeFile : RemoteAction
    {
        private LogErrorBll logBll = new LogErrorBll();
        private string dataType = "";

        public UploadAcknowledgeFile() { }

        public UploadAcknowledgeFile(string Type, string username)
        {
            SystemSettingBll setting = new SystemSettingBll();
            dataType = Type;
            userName = username;

            fileName = setting.GetSettingStringByKey("SFTPAcknowledge" + Type + "FileName");
            fileFormat = setting.GetSettingStringByKey("SFTPAcknowledge" + Type + "FileName");
            fileExtension = ".ack";
            fileFormatID = setting.GetSettingIntByKey("SFTPAcknowledge" + Type + "FileFormatID");

            filePathRemote = setting.GetSettingStringByKey("SFTPAcknowledge" + Type + "RemotePath");
            filePathBackup = setting.GetSettingStringByKey("SFTPAcknowledge" + Type + "BackupPath");
            filePathLocal = setting.GetSettingStringByKey("SFTPAcknowledge" + Type + "LocalPath");

            fileHost = setting.GetSettingStringByKey("SFTPAcknowledge" + Type + "Host");
            fileUserName = setting.GetSettingStringByKey("SFTPAcknowledge" + Type + "User");
            filePassword = setting.GetSettingStringByKey("SFTPAcknowledge" + Type + "Password");

            fileNames = new List<string>();
            fileNamesFull = new List<Tuple<string, int, int>>();
            fileNamesError = new List<Tuple<string, string>>();

            if (string.IsNullOrEmpty(fileHost) || string.IsNullOrEmpty(fileUserName) || string.IsNullOrEmpty(filePassword))
            {
                errorMeassge = "No setting server in database";
            }
        }

        public object SaveAcknowledge(List<Tuple<string, int, int>> FileNamesFull)
        {
            TempFileAcknowledgeBll bll = new TempFileAcknowledgeBll();
            TempFileImportBll imp = new TempFileImportBll();
            object oResult = new object();

            try
            {
                if (dataType == "SKU")
                {
                    List<TempFileAcknowledgeSKUDetail> details = new List<TempFileAcknowledgeSKUDetail>();
                    foreach (var o in FileNamesFull)
                    {
                        int success = Convert.ToInt32(o.Item2.ToString() ?? "0") - Convert.ToInt32(o.Item3.ToString() ?? "0");
                        TempFileAcknowledgeSKUDetail detail = new TempFileAcknowledgeSKUDetail
                        {
                            FileName = Path.GetFileName(o.Item1),
                            Total = o.Item2.ToString() ?? "0",
                            Error = o.Item3.ToString() ?? "0",
                            Success = success.ToString(),
                            Plant = o.Item1.Split(new char[] { '_' })[3],
                            SuccessTime = DateTime.Now.ToString("yyyyMMddHHmmss")
                        };
                        details.Add(detail);
                    }
                    oResult = details;             
                }
                else
                {
                    List<TempFileAcknowledgeBarDetail> details = new List<TempFileAcknowledgeBarDetail>();
                    foreach (var o in FileNamesFull)
                    {
                        int success = Convert.ToInt32(o.Item2.ToString() ?? "0") - Convert.ToInt32(o.Item3.ToString() ?? "0");
                        TempFileAcknowledgeBarDetail detail = new TempFileAcknowledgeBarDetail
                        {
                            FileName = Path.GetFileName(o.Item1),
                            Total = o.Item2.ToString() ?? "0",
                            Error = o.Item3.ToString() ?? "0",
                            Success = success.ToString(),
                            Plant = o.Item1.Split(new char[] { '_' })[3],
                            SuccessTime = DateTime.Now.ToString("yyyyMMddHHmmss")
                        };
                        details.Add(detail);
                    }
                    oResult = details;               
                }
            }
            catch(Exception ex)
            {
                oResult = null;
                logBll.LogError(userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }

            if (oResult != null)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Successful", DateTime.Now);
            }
            return oResult;
        }

        public string GenerateFile(object oResult)
        {
            string str = "";
            DataTable dt = new DataTable();
            try
            {
                if (dataType == "SKU")
                {
                    List<TempFileAcknowledgeSKUDetail> o = (List<TempFileAcknowledgeSKUDetail>)oResult;
                    dt = ToDataTable(o);
                }
                else
                {
                    List<TempFileAcknowledgeBarDetail> o = (List<TempFileAcknowledgeBarDetail>)oResult;
                    dt = ToDataTable(o);
                }

                ConfigFileFormatBll cfg = new ConfigFileFormatBll();
                ConfigFileFormat format = cfg.GetConfigFileFormat(fileFormatID);

                string contentStr = "";
                if (dt.Rows.Count > 0)
                {
                    if (!Directory.Exists(filePathLocal))
                    {
                        Directory.CreateDirectory(filePathLocal);
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        string downloadFileName = dr["FileName"] != null ? dr["FileName"].ToString() : "";
                        downloadFileName = Path.GetFileNameWithoutExtension(Path.GetFileName(downloadFileName));

                        string[] temp = downloadFileName.Split(new char[] { '_' });
                        string savefileName = fileName;
                        if (temp.Length == 5)
                        {
                            savefileName = savefileName.Replace("{DateSKUFile}", temp[0]);
                            savefileName = savefileName.Replace("{DateBarFile}", temp[0]);
                            savefileName = savefileName.Replace("{Plant}", temp[3]);
                            savefileName = savefileName.Replace("{CountSheet}", temp[4]);

                            savefileName = savefileName.Replace("{Date}", DateTime.Now.ToString("yyyyMMddHHmmssFF"));
                            savefileName = savefileName.Replace("{date14}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                            savefileName = savefileName.Replace("{date17}", DateTime.Now.ToString("yyyyMMddHHmmssFFF"));
                            Encoding utf8WithoutBom = new UTF8Encoding(false);

                            StreamWriter sw = new StreamWriter(filePathLocal + "/" + savefileName, false, utf8WithoutBom);

                            contentStr = "";
                            for (int j = 2; j <= dt.Columns.Count - 3; j++)
                            {
                                contentStr += dr[j].ToString() + "|";
                            }
                            contentStr = contentStr.TrimEnd(new char[] { '|' });
                            sw.WriteLine(contentStr);
                            sw.Flush();
                            sw.Close();
                            fileNames.Add(savefileName);
                            str = "OK:" + filePathLocal + "/" + savefileName;
                        }
                        else
                        {
                            str = "Fail:File name is wrong format";
                            logBll.LogError(userName,  MethodBase.GetCurrentMethod().Name,downloadFileName, str, DateTime.Now);
                        }
                    }
                }
                else
                {
                    str = "Fail:Not found data to generate file";
                    logBll.LogError(userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
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

        public DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public string  UploadToSFTP()
        {
            return UploadFileToSFTPServer();
        }
    }
}
