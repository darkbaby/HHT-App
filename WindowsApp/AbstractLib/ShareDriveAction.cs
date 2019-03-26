using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SFTP;
using FSBT_HHT_BLL;
namespace AbstractLib
{
    public abstract class ShareDriveAction
    {

        private string _fileName;

        private string _fileType;

        private string _fileFormat;

        private int _fileFormatID;

        private string _fileBackupSubfix = "_Used";

        private string _filePathLocal;

        private string _filePathRemote;

        private string _fileHost;

        private string _fileUserName;

        private string _filePassword;

        private string _filePrivateKey;

        private string _filePathBackup;

        private int _moveToBackup;

        private string _fileExtension;

        private int _expectedNumRows;

        private string _tableName;

        private bool _notfound;

        private bool _oldversion;

        private bool _cannotdownload;

        private bool _cannotconnect;

        public string fileName { get { return _fileName; } set { _fileName = value; } }

        public string fileType { get { return _fileType; } set { _fileType = value; } }

        public string fileFormat { get { return _fileFormat; } set { _fileFormat = value; } }

        public int fileFormatID { get { return _fileFormatID; } set { _fileFormatID = value; } }

        private string fileBackupSubfix { get { return _fileBackupSubfix; } set { _fileBackupSubfix = value; } }

        public string filePathLocal { get { return _filePathLocal; } set { _filePathLocal = value; } }

        public string filePathRemote { get { return _filePathRemote; } set { _filePathRemote = value; } }

        public string fileHost { get { return _fileHost; } set { _fileHost = value; } }

        public string fileUserName { get { return _fileUserName; } set { _fileUserName = value; } }

        public string filePassword { get { return _filePassword; } set { _filePassword = value; } }

        public string filePrivateKey { get { return _filePrivateKey; } set { _filePrivateKey = value; } }

        public string filePathBackup { get { return _filePathBackup; } set { _filePathBackup = value; } }

        public int moveToBackup { get { return _moveToBackup; } set { _moveToBackup = value; } }

        public string fileExtension { get { return _fileExtension; } set { _fileExtension = value; } }

        public int expectedNumRows { get { return _expectedNumRows; } set { _expectedNumRows = value; } }

        public string tableName { get { return _tableName; } set { _tableName = value; } }

        public bool notfound { get { return _notfound; } set { _notfound = value; } }
        public bool oldversion { get { return _oldversion; } set { _oldversion = value; } }
        public bool cannotdownload { get { return _cannotdownload; } set { _cannotdownload = value; } }

        public bool cannotconnect { get { return _cannotconnect; } set { _cannotconnect = value; } }

        public string CopyFile()
        {
            string str = "";
            try
            {
                string[] arrFile = Directory.GetFiles(_filePathRemote, "*." + _fileExtension, SearchOption.TopDirectoryOnly);
                var arr = new ArrayList(arrFile);
                arr.Sort();
                arr.Reverse();
                if (arr.Count > 0)
                {
                    foreach (string file in arr)
                    {
                        string fileNameStr = Path.GetFileName(file);
                        string validate = ValidatorBll.ValidateFileName(fileNameStr, fileFormat);
                        if (validate == "OK")
                        {
                            string version = ValidatorBll.ValidateFileOldVersion(fileNameStr, fileFormat, _fileType);
                            if (version == "OK")
                            {
                                string tempFileName = Path.GetFileNameWithoutExtension(file);

                                fileName = tempFileName + "." + _fileExtension;
                                if (File.Exists(_filePathLocal + "/" + tempFileName + "." + _fileExtension))
                                    File.Delete(_filePathLocal + "/" + tempFileName + "." + _fileExtension);
                                if (!Directory.Exists(_filePathLocal))
                                    Directory.CreateDirectory(_filePathLocal);

                                File.Copy(_filePathRemote + "/" + tempFileName + "." + _fileExtension, _filePathLocal + "/" + tempFileName + "." + _fileExtension, true);
                                File.Move(_filePathRemote + "/" + fileName, _filePathBackup + "/" + tempFileName + _fileBackupSubfix + "." + _fileExtension);
                                str = "";
                                oldversion = false;
                                break;
                            }
                            else
                            {
                                str = "Fail:file version is older than current version";
                                oldversion = true;
                            }
                        }
                        else
                        {
                            notfound = true;
                        }
                    }
                }
                else
                {
                    notfound = true;
                    str = "There are no file on server";

                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        //public string CopyFile()
        //{
        //    string str = "";
        //    try
        //    {
        //        string[] arrFile = Directory.GetFiles(_filePathRemote, "*." + _fileExtension, SearchOption.TopDirectoryOnly);
        //        var arr = new ArrayList(arrFile);
        //        arr.Sort();
        //        arr.Reverse();
        //        if (arr.Count > 0)
        //        {
        //            foreach (string file in arr)
        //            {

        //                string fileNameStr = Path.GetFileName(file);
        //                string validate = ValidatorBll.ValidateFileName(fileNameStr, fileFormat);
        //                if (validate == "OK")
        //                {
        //                    string version = ValidatorBll.ValidateFileOldVersion(fileNameStr,fileFormat, _fileType);
        //                    if (version == "OK")
        //                    {
        //                        string tempFileName = Path.GetFileNameWithoutExtension(file);

        //                        fileName = tempFileName + "." + _fileExtension;
        //                        if (File.Exists(_filePathLocal + "/" + tempFileName + "." + _fileExtension))
        //                            File.Delete(_filePathLocal + "/" + tempFileName + "." + _fileExtension);
        //                        if (!Directory.Exists(_filePathLocal))
        //                            Directory.CreateDirectory(_filePathLocal);

        //                        File.Copy(_filePathRemote + "/" + tempFileName + "." + _fileExtension, _filePathLocal + "/" + tempFileName + "." + _fileExtension, true);
        //                        str = "";
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        str = "Fail:file version is older than current version";
        //                    }
        //                }
        //            }
        //        }
        //        else str = "There are no file on server";
        //    }
        //    catch (Exception ex)
        //    {
        //        str = ex.Message;
        //    }
        //    return str;
        //}

        //public string CopyFile()
        //{
        //    string str = "";
        //    //bool downloaded = false;
        //    try
        //    {
        //        string[] arrFile = Directory.GetFiles(_filePathRemote, "*." + _fileExtension, SearchOption.TopDirectoryOnly);
        //        var arr = new ArrayList(arrFile);
        //        arr.Sort();
        //        arr.Reverse();
        //        if (arr.Count > 0)
        //        {
        //            foreach (string file in arr)
        //            {
        //                string fileNameStr = Path.GetFileName(file);
        //                string validate = ValidatorBll.ValidateFileName(fileNameStr, fileFormat);
        //                if (validate == "OK")
        //                {
        //                    string version = ValidatorBll.ValidateFileOldVersion(fileNameStr, fileFormat, _fileType);
        //                    if (version == "OK")
        //                    {
        //                        string tempFileName = Path.GetFileNameWithoutExtension(file);

        //                        fileName = tempFileName + "." + _fileExtension;
        //                        if (File.Exists(_filePathLocal + "/" + tempFileName + "." + _fileExtension))
        //                            File.Delete(_filePathLocal + "/" + tempFileName + "." + _fileExtension);
        //                        if (!Directory.Exists(_filePathLocal))
        //                            Directory.CreateDirectory(_filePathLocal);

        //                        //if (!downloaded)
        //                        //{
        //                            File.Copy(_filePathRemote + "/" + tempFileName + "." + _fileExtension, _filePathLocal + "/" + tempFileName + "." + _fileExtension, true);
        //                            File.Move(_filePathRemote + "/" + fileName, _filePathBackup + "/" + tempFileName + _fileBackupSubfix + "." + _fileExtension);
        //                        //}
        //                        //else
        //                        //{
        //                        //    File.Move(_filePathRemote + "/" + fileName, _filePathBackup + "/" + tempFileName + _fileBackupSubfix + "." + _fileExtension);
        //                        //}
        //                        str = "";
        //                        oldversion = false;
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        //if (!downloaded)
        //                        //{
        //                        //    str = "Fail:file version is older than current version";
        //                        //    oldversion = true;
        //                        //}
        //                        str = "Fail:file version is older than current version";
        //                        oldversion = true;
        //                    }
        //                }
        //                else
        //                {
        //                    notfound = true;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            notfound = true;
        //            str = "There are no file on server";

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        str = ex.Message;
        //    }
        //    return str;
        //}

        //public string MoveFileBackup()
        //{
        //    string str = "";
        //    try
        //    {
        //        string[] arrFile = Directory.GetFiles(_filePathRemote, "*." + _fileExtension, SearchOption.TopDirectoryOnly);

        //        var arr = new ArrayList(arrFile);
        //        arr.Sort();
        //        arr.Reverse();
        //        string tempFileName = fileName.Substring(0, fileName.IndexOf('.'));

        //        if (!Directory.Exists(_filePathBackup))
        //        {
        //            DirectoryInfo di = Directory.CreateDirectory(_filePathBackup);
        //        }

        //        if (moveToBackup == 1)
        //        {
        //            foreach (string file in arr)
        //            {
        //                string validate = ValidatorBll.ValidateFileName(file, fileFormat);
        //                if (validate == "OK")
        //                {
        //                    if (fileName == Path.GetFileName(file))
        //                    {
        //                        if (File.Exists(_filePathBackup + "/" + tempFileName + _fileBackupSubfix + "." + _fileExtension))
        //                            File.Delete(_filePathBackup + "/" + tempFileName + _fileBackupSubfix + "." + _fileExtension);

        //                        File.Move(_filePathRemote + "/" + fileName, _filePathBackup + "/" + tempFileName + _fileBackupSubfix + "." + _fileExtension);
        //                    }
        //                    //else
        //                    //{
        //                    //    File.Move(_filePathRemote + "/" + file, _filePathBackup + "/" + file);
        //                    //}
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        str = ex.Message;
        //    }
        //    return str;
        //}

        public string MoveFileBackup()
        {
            string str = "";
            try
            {
                string[] arrFile = Directory.GetFiles(_filePathRemote, "*." + _fileExtension, SearchOption.TopDirectoryOnly);

                var arr = new ArrayList(arrFile);
                arr.Sort();
                arr.Reverse();
                string tempFileName = fileName.Substring(0, fileName.IndexOf('.'));

                if (!Directory.Exists(_filePathBackup))
                {
                    DirectoryInfo di = Directory.CreateDirectory(_filePathBackup);
                }

                if (moveToBackup == 1)
                {
                    foreach (string file in arr)
                    {
                        string fileNameStr = Path.GetFileName(file);
                        string validate = ValidatorBll.ValidateFileName(fileNameStr, fileFormat);
                        if (validate == "OK")
                        {
                            if (fileName.ToLower() == Path.GetFileName(file).ToLower())
                            {
                                if (File.Exists(_filePathBackup + "/" + tempFileName + _fileBackupSubfix + "." + _fileExtension))
                                    File.Delete(_filePathBackup + "/" + tempFileName + _fileBackupSubfix + "." + _fileExtension);

                                File.Move(_filePathRemote + "/" + fileName, _filePathBackup + "/" + tempFileName + _fileBackupSubfix + "." + _fileExtension);
                            }
                            //else
                            //{
                            //    File.Move(_filePathRemote + "/" + file, _filePathBackup + "/" + file);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string GetLastestFileShareDrive()
        {
            string returnFileName = "";
            string[] arrFile = Directory.GetFiles(_filePathRemote, "*." + _fileExtension, SearchOption.TopDirectoryOnly);
            var arr = new ArrayList(arrFile);
            arr.Sort();
            arr.Reverse();

            foreach (string file in arr)
            {
                string validate = ValidatorBll.ValidateFileName(file, fileFormat);
                if (validate == "OK")
                {
                    return file;
                }
            }
            return returnFileName;
        }
    }
}
