using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SFTP;
using FSBT_HHT_BLL;
using FSBT_HHT_Model;
using System.Reflection;
namespace AbstractLib
{
    public abstract class RemoteAction
    {
        private string _fileName;
        private string _fileType;
        private string _fileFormat;
        private string _fileFormatOld;
        private int _fileFormatID;
        private string _fileBackupSubfix = "_Used";
        private string _filePathLocal;
        private string _filePathRemote;
        private string _filePathRemoteOld;
        private string _fileHost;
        private string _fileUserName;
        private string _filePassword;
        private string _filePrivateKey;
        private string _filePathBackup;
        private string _filePathBackupOld;
        private int _moveToBackup;
        private string _fileExtension;
        private int _expectedNumRows;
        private string _tableName;
        private bool _notfound;
        private bool _oldversion;
        private bool _cannotdownload;
        private bool _cannotconnect;
        private string _endOfFile;
        private string _errorMeassge;
        private string _userName;
        private List<string> _fileNames;
        private List<Tuple<string, int, int>> _fileNamesFull;
        private List<Tuple<string, string>> _fileNamesError;
        private List<string> _countSheets;
        private List<DownloadFileModel> _downloadedFiles;

        public string fileName { get { return _fileName; } set { _fileName = value; } }
        public string fileType { get { return _fileType; } set { _fileType = value; } }
        public string fileFormat { get { return _fileFormat; } set { _fileFormat = value; } }
        public string fileFormatOld { get { return _fileFormatOld; } set { _fileFormatOld = value; } }
        public int fileFormatID { get { return _fileFormatID; } set { _fileFormatID = value; } }
        public string fileBackupSubfix { get { return _fileBackupSubfix; } set { _fileBackupSubfix = value; } }
        public string filePathLocal { get { return _filePathLocal; } set { _filePathLocal = value; } }
        public string filePathRemote { get { return _filePathRemote; } set { _filePathRemote = value; } }
        public string filePathRemoteOld { get { return _filePathRemoteOld; } set { _filePathRemoteOld = value; } }
        public string fileHost { get { return _fileHost; } set { _fileHost = value; } }
        public string fileUserName { get { return _fileUserName; } set { _fileUserName = value; } }
        public string filePassword { get { return _filePassword; } set { _filePassword = value; } }
        public string filePrivateKey { get { return _filePrivateKey; } set { _filePrivateKey = value; } }
        public string filePathBackup { get { return _filePathBackup; } set { _filePathBackup = value; } }
        public string filePathBackupOld { get { return _filePathBackupOld; } set { _filePathBackupOld = value; } }
        public int moveToBackup { get { return _moveToBackup; } set { _moveToBackup = value; } }
        public string fileExtension { get { return _fileExtension; } set { _fileExtension = value; } }
        public int expectedNumRows { get { return _expectedNumRows; } set { _expectedNumRows = value; } }
        public string tableName { get { return _tableName; } set { _tableName = value; } }
        public bool notfound { get { return _notfound; } set { _notfound = value; } }
        public bool oldversion { get { return _oldversion; } set { _oldversion = value; } }
        public bool cannotdownload { get { return _cannotdownload; } set { _cannotdownload = value; } }
        public bool cannotconnect { get { return _cannotconnect; } set { _cannotconnect = value; } }
        public string endOfFile { get { return _endOfFile; } set { _endOfFile = value; } }
        public List<string> fileNames { get { return _fileNames; } set { _fileNames = value; } }
        public string userName { get { return _userName; } set { _userName = value; } }

        //item 0 = filename item 1 = total item 2 = error
        public List<Tuple<string, int, int>> fileNamesFull { get { return _fileNamesFull; } set { _fileNamesFull = value; } }
        public List<Tuple<string, string>> fileNamesError { get { return _fileNamesError; } set { _fileNamesError = value; } }
        public List<string> countSheets { get { return _countSheets; } set { _countSheets = value; } }
        public string errorMeassge { get { return _errorMeassge; } set { _errorMeassge = value; } }
        public List<DownloadFileModel> downloadedFiles { get { return _downloadedFiles; } set { _downloadedFiles = value; } }

        private LogErrorBll logBll = new LogErrorBll();

        public string UploadFileToSFTPServer()
        {
            string str = "";
            if (string.IsNullOrEmpty(_fileHost) || string.IsNullOrEmpty(_fileUserName) || string.IsNullOrEmpty(_filePassword))
            {
                str = "No setting SFTP server in database";
                logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
            }
            else
            {
                SFTPHelper sftp = new SFTPHelper(_fileHost, _fileUserName, _filePassword);
                try
                {
                    if (sftp.Connect())
                    {
                        foreach (var file in fileNames)
                        {
                            var localFile = Path.Combine(_filePathLocal, file);
                            var remoteFile = _filePathRemote + "/" + file;

                            if (sftp.Upload(localFile, remoteFile))
                            {
                                str = "OK";
                                File.Delete(localFile);
                            }
                            else
                            {
                                str = "Cannot upload file";
                                logBll.LogError(_userName, MethodBase.GetCurrentMethod().Name, file, str, DateTime.Now);
                            }
                        }
                        sftp.Disconnect();
                    }
                    else
                    {
                        str = "Cannot connect to SFTP Server";
                        logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
                    }
                }
                catch (Exception ex)
                {
                    str = ex.Message;
                    logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
                }
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

        public bool IsConnected()
        {
            bool result = false;
            SFTPHelper sftp = new SFTPHelper(_fileHost, _fileUserName, _filePassword);
            if (sftp.Connect())
            {
                result = true;
            }
            return result;
        }

        public string DownloadFile()
        {
            string str = "";
            _fileNamesError = new List<Tuple<string, string>>();

            if (string.IsNullOrEmpty(_fileHost) || string.IsNullOrEmpty(_fileUserName) || string.IsNullOrEmpty(_filePassword))
            {
                str = "No setting SFTP server in database";
                logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
            }
            else
            {
                try
                {
                    SFTPHelper sftp = new SFTPHelper(_fileHost, _fileUserName, _filePassword);
                    if (sftp.Connect())
                    {
                        foreach (string file in _fileNames)
                        {
                            string tempFileName = Path.GetFileNameWithoutExtension(file);
                            string remoteFile = _filePathRemote + "/" + file;
                            string localFile = Path.Combine(_filePathLocal, file);

                            if (sftp.Download(remoteFile, localFile))
                            {
                                string countsheet = GetCountSheetFromFileName(file);
                                string plant = GetPlantFromFileName(file);
                                _downloadedFiles.Add(new DownloadFileModel
                                {
                                    filenamewithpath = remoteFile,
                                    filename = file,
                                    fileformat = _fileFormat,
                                    countsheet = countsheet,
                                    plant = plant
                                });
                                str = "";
                            }
                            else
                            {
                                _fileNamesError.Add(new Tuple<string, string>(file, "Cannot download file to local path"));
                                logBll.LogError(_userName, MethodBase.GetCurrentMethod().Name, file, "Cannot download file to local path", DateTime.Now);
                            }
                        }

                        if (_fileNames.Count == _fileNamesError.Count)
                        {
                            str = "Cannot download file";
                            cannotdownload = true;
                        }
                        sftp.Disconnect();
                    }
                    else
                    {
                        str = "Cannot connect to SFTP Server";
                        logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
                        cannotconnect = true;
                    }
                }
                catch (Exception ex)
                {
                    str = ex.Message;
                    logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
                }
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

        public string MoveFileToArchives(List<string> filenames)
        {
            string str = "";
            if (string.IsNullOrEmpty(_fileHost) || string.IsNullOrEmpty(_fileUserName) || string.IsNullOrEmpty(_filePassword))
            {
                str = "No setting SFTP server in database";
            }
            else
            {
                try
                {
                    SFTPHelper sftp = new SFTPHelper(_fileHost, _fileUserName, _filePassword);
                    if (sftp.Connect())
                    {
                        foreach (var file in filenames)
                        {
                            string tempFileName = Path.GetFileNameWithoutExtension(file);
                            string remotefile = _filePathRemote + "/" + file;
                            string backupfile = _filePathBackup + "/" + DateTime.Now.ToString("yyyyMMddHHmmssFF") + "_" + tempFileName + _fileBackupSubfix + "." + _fileExtension;

                            if (sftp.MoveFile(remotefile, backupfile))
                            {
                                fileName = tempFileName + "." + _fileExtension;
                                str = "";
                            }
                            else
                            {
                                str = "Cannot move file";
                                logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
                                cannotdownload = true;
                            }
                        }
                        sftp.Disconnect();
                    }
                    else
                    {
                        str = "Cannot connect to SFTP Server";
                        logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
                        cannotconnect = true;
                    }
                }
                catch (Exception ex)
                {
                    str = ex.Message;
                    logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                }
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

        public string GetListOfSFTPFiles()
        {
            string str = "";
            if (string.IsNullOrEmpty(_fileHost) || string.IsNullOrEmpty(_fileUserName) || string.IsNullOrEmpty(_filePassword))
            {
                str = "No setting SFTP server in database";
            }
            else
            {
                try
                {
                    SFTPHelper sftp = new SFTPHelper(_fileHost, _fileUserName, _filePassword);
                    if (sftp.Connect())
                    {
                        if (sftp.IsExistPath(_filePathRemote))
                        {
                            ArrayList arr = sftp.GetFileList(_filePathRemote, _fileExtension, ref str);

                            if (!string.IsNullOrEmpty(str))
                            {
                                errorMeassge = str;
                                notfound = true;
                                logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
                            }

                            if (arr.Count > 0)
                            {
                                foreach (string file in arr)
                                {
                                    string validate = ValidatorBll.ValidateFileName(file, _fileFormat);
                                    if (validate == "OK")
                                    {
                                        _fileNames.Add(file);
                                    }
                                }
                                if (_fileNames.Count <= 0)
                                {
                                    str = "File not found";
                                    notfound = true;
                                    logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
                                }
                            }
                            else
                            {
                                str = "File not found";
                                notfound = true;
                                logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
                            }
                        }
                        else
                        {
                            str = "Not Found Path " + _filePathRemote + " on SFTP Server";
                            notfound = true;
                            logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
                        }
                        sftp.Disconnect();
                    }
                    else
                    {
                        str = "Cannot connect to SFTP Server";
                        cannotconnect = true;
                        logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
                    }
                }
                catch (Exception ex)
                {
                    str = ex.Message;
                    logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
                }
            }
            errorMeassge = str;

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

        public string GetListOfRegularFiles(List<string> plants)
        {
            string str = "";
            List<string> regFile = new List<string>();
            try
            {
                foreach (var plant in plants)
                {
                    string store = plant.Substring(0, 2);
                    string remotePath = _filePathRemote + store;
                    if (Directory.Exists(remotePath))
                    {
                        string[] arrFile = Directory.GetFiles(remotePath, "*." + fileExtension, SearchOption.TopDirectoryOnly);
                        var arr = new ArrayList(arrFile);
                        if (arr.Count > 0)
                        {
                            foreach (string file in arrFile)
                            {
                                string validate = ValidatorBll.ValidateFileName(file, _fileFormat);
                                if (validate == "OK")
                                {
                                    regFile.Add(file);
                                }
                            }

                            if (regFile.Count <= 0)
                            {
                                str = "File not found";
                                notfound = true;
                                logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
                            }
                            else
                            {
                                DateTime maxDate = new DateTime();
                                maxDate = DateTime.ParseExact("00010101000000", "yyyyMMddHHmmss", null);
                                string maxFile = "";
                                foreach (var reg in regFile)
                                {
                                    string tempFilename = Path.GetFileNameWithoutExtension(reg);
                                    string[] temp = tempFilename.Split(new Char[] { '_' });

                                    if (temp[0].Contains(plant))
                                    {
                                        var regDate = DateTime.ParseExact(temp[1], "yyyyMMddHHmmss", null);
                                        if (regDate > maxDate)
                                        {
                                            maxDate = regDate;
                                            maxFile = reg;
                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(maxFile))
                                {
                                    _fileNames.Add(maxFile);
                                }                            
                            }
                        }
                        else
                        {
                            str = "File not found";
                            notfound = true;
                            logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
                        }                       
                    }
                    else
                    {
                        str = "Path not found";
                        notfound = true;
                        logBll.LogError(_userName, this.GetType().Name, str, remotePath, DateTime.Now);
                    }                   
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
                cannotconnect = true;
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
            }

            if (_fileNames.Count > 0)
            {
                str = "";
            }

            errorMeassge = str;

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

        public string CopyFileFromShareDrive() //sharedrive
        {
            string str = "";
            try
            {
                if (!Directory.Exists(_filePathLocal))
                    Directory.CreateDirectory(_filePathLocal);

                foreach (string file in _fileNames)
                {
                    string filename = Path.GetFileName(file);
                    string localPath =  Path.Combine(_filePathLocal ,filename);

                    if (File.Exists(localPath))
                        File.Delete(localPath);

                    File.Copy(file, localPath, true);
                    string countsheet = "";
                    string plant = GetPlantFromFileName(filename);

                    _downloadedFiles.Add(new DownloadFileModel
                    {
                        filenamewithpath = file,
                        filename = filename,
                        fileformat = _fileFormat,
                        countsheet = countsheet,
                        plant = plant
                    });
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
                logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
            }
            errorMeassge = str;

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

        private string GetCountSheetFromFileName(string filename)
        {
            string countsheetfromfilename = "";
            string[] temp1 = Path.GetFileNameWithoutExtension(_fileFormat.ToLower()).Split(new Char[] { '_' });
            string[] temp2 = Path.GetFileNameWithoutExtension(filename).Split(new Char[] { '_' });
            int i = 0;
            try
            {
                foreach (string tmp in temp1)
                {
                    string data = temp2[i];
                    if (tmp.ToLower() == "{countsheet}")
                    {
                        countsheetfromfilename = data;
                        break;
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                countsheetfromfilename = "";
                logBll.LogError(_userName, MethodBase.GetCurrentMethod().Name, filename, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, filename + " : " + ex.Message, DateTime.Now);
            }

            if (string.IsNullOrEmpty(countsheetfromfilename))
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, filename + " : Successful", DateTime.Now);
            }

            return countsheetfromfilename;
        }

        private string GetPlantFromFileName(string filename)
        {
            string plantfromfilename = "";
            string[] temp1 = Path.GetFileNameWithoutExtension(_fileFormat.ToLower()).Split(new Char[] { '_' });
            string[] temp2 = Path.GetFileNameWithoutExtension(filename).Split(new Char[] { '_' });
            int i = 0;
            try
            {
                foreach (string tmp in temp1)
                {
                    string data = temp2[i];
                    if (tmp.ToLower() == "{plant}")
                    {
                        plantfromfilename = data;
                        break;
                    }
                    i++;
                }
            }
            catch(Exception ex)
            {
                plantfromfilename = "";
                logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, filename + " : " + ex.Message, DateTime.Now);
            }

            if (string.IsNullOrEmpty(plantfromfilename))
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, filename + " : Successful", DateTime.Now);
            }

            return plantfromfilename;
        }

        public string ExtractFileFromTextBox(string selectedFile) //sharedrive
        {
            string str = "";
            string[] listPath = selectedFile.Split(',');
            try
            {
                foreach (string file in listPath)
                {
                    string filename = Path.GetFileName(file);
                    filePathLocal = Path.GetDirectoryName(file);

                    string countsheet = GetCountSheetFromFileName(filename);
                    string plant = GetPlantFromFileName(filename);

                    _downloadedFiles.Add(new DownloadFileModel
                    {
                        filenamewithpath = file,
                        filename = filename,
                        fileformat = _fileFormat,
                        countsheet = countsheet,
                        plant = plant
                    });
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
                logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
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

        public string MoveFileToBackup(string filepath)
        {
            string str = "";
            string tempFileName = Path.GetFileNameWithoutExtension(filepath);
            string tempFilePath = Path.GetDirectoryName(filepath);
            try
            {
                File.Move(filepath, tempFilePath +"/" +_filePathBackup + "/" + tempFileName + _fileBackupSubfix + "." + _fileExtension);
            }
            catch(Exception ex)
            {
                str = ex.Message;
                logBll.LogError(_userName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, str, DateTime.Now);
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
