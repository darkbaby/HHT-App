using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_BatchCommon;
using FSBT_HHT_BatchCommon.Model;
using System.Threading;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Data;

namespace FSBT_HHT_Batch
{
    class Program
    {
        static void Main(string[] args)
        {
            SettingBean setting = InitialSetting.SetSettingBean();
            LogFile.write(Status.INFO.ToString(), "Start Process");
            Boolean chkSettings = InitialSetting.CheckSettingFile(setting);

            if (chkSettings)
            {
                Management manage = new Management();
                Database db = new Database();

                DataTable dtFile = new DataTable();
                dtFile.Columns.Add("PathFileName", typeof(string));
                dtFile.Columns.Add("DateModify", typeof(DateTime));
                dtFile.Columns.Add("FileName", typeof(string));
                dtFile.Columns.Add("FileType", typeof(string));
                List<string> filePathError = new List<string>();

                if (Directory.Exists(setting.sourcePath))
                {
                    string directoryPath = setting.sourcePath;
                    deleteFileBin(directoryPath);                    
                }

                while (true)
                {
                    Thread.Sleep(Convert.ToInt32(setting.sleeptime));
                    if (Directory.Exists(setting.sourcePath))
                    {
                        if (!(Directory.Exists(setting.sourcePath + "\\tmp")))
                        {
                            Directory.CreateDirectory(setting.sourcePath + "\\tmp");
                        }

                        string directoryPath = setting.sourcePath;

                        dtFile.Clear();
                        string[] zipFiles = Directory.GetFiles(directoryPath);
                        List<string> listZipFiles = new List<string>();

                        //listZipFiles = zipFiles.ToList();
                        listZipFiles = zipFiles.Where(all => !filePathError.Any(except => except == all)).ToList();
                        //listZipFiles = listZipFiles.Where(all => !all.ToUpper().Contains("TEMP")).ToList();

                        if (listZipFiles.Count() > 0)
                        {
                            foreach (string fileZipPath in listZipFiles)
                            {
                                DateTime lastModified = System.IO.File.GetLastWriteTime(fileZipPath);
                                string[] splitName = fileZipPath.Split('.');
                                string lastSplit = splitName[0];
                                string extension = Path.GetExtension(fileZipPath);

                                DataRow drFile = dtFile.NewRow();
                                drFile[0] = fileZipPath;
                                drFile[1] = lastModified;
                                drFile[2] = lastSplit;
                                drFile[3] = extension;
                                dtFile.Rows.Add(drFile);
                            }

                            List<string> tmpFileName = new List<string>();

                            tmpFileName = (from dt in dtFile.AsEnumerable()
                                           group dt by dt[2] into grp
                                           where grp.Count() == 1
                                           select grp.Key.ToString()
                                            ).ToList();

                            if (tmpFileName.Count > 0)
                            {
                                dtFile = dtFile.AsEnumerable().Where(all => tmpFileName.Any(except => except.Equals(all["FileName"]))).CopyToDataTable();
                                if (dtFile.AsEnumerable().Where(all => !all["FileType"].Equals("")).Count() > 0)
                                {
                                    dtFile = dtFile.AsEnumerable().Where(all => !all["FileType"].Equals("")).CopyToDataTable();
                                }
                                else
                                {
                                    dtFile.Clear();
                                }
                                //dtFile = manage.LINQResultToDataTable(dtTemp);
                            }
                            else
                            {
                                dtFile.Clear();
                            }

                            if (dtFile.Rows.Count > 0)
                            {
                                string FileNameModifyFirst = (from dt in dtFile.AsEnumerable()
                                                              select dt[0].ToString()).OrderBy(x => x[1]).FirstOrDefault();

                                try
                                {
                                    String extensionZip = Path.GetExtension(FileNameModifyFirst);

                                    if (extensionZip.Equals(".zip"))
                                    {
                                        if (File.Exists(FileNameModifyFirst))
                                        {
                                            string filePath = directoryPath + "\\tmp\\Record.txt";
                                            File.Delete(filePath);
                                            ZipFile.ExtractToDirectory(FileNameModifyFirst, directoryPath + "\\tmp");

                                            String extensionTxt = Path.GetExtension(filePath);

                                            if (extensionTxt.Equals(".txt"))
                                            {
                                                if (File.Exists(filePath))
                                                {
                                                    Hashtable hasResult = new Hashtable();
                                                    hasResult = manage.ReadFile(filePath, filePathError, FileNameModifyFirst, setting);
                                                    ImportModel importData = (ImportModel)hasResult["importData"];
                                                    filePathError = (List<string>)hasResult["filePathError"];
                                                    int countRow = (int)hasResult["countRow"];
                                                    string[] splitName1 = FileNameModifyFirst.Split('_');
                                                    string lastSplit1 = splitName1[splitName1.Length - 1];
                                                    string[] splitName2 = lastSplit1.Split('.');
                                                    string lastSplit2 = splitName2[0];

                                                    int countRealFile;
                                                    if (Int32.TryParse(lastSplit2, out countRealFile))
                                                    {
                                                        if (countRealFile == (countRow - 3))
                                                        {
                                                            if (importData.RecordData.Count > 0)
                                                            {
                                                                bool importSuccess = db.ImportRecord(importData.RecordData, importData.HHTID, importData.DeviceName, importData.Mode, FileNameModifyFirst, setting);
                                                                if (importSuccess)
                                                                {
                                                                    LogFile.write(Status.INFO.ToString(), "Success File " + FileNameModifyFirst);
                                                                    File.Delete(FileNameModifyFirst);
                                                                    File.Delete(filePath);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            File.Delete(filePath);
                                                            filePathError.Add(FileNameModifyFirst);
                                                            LogFile.write(Status.ERROR.ToString(), FileNameModifyFirst + " is incorrect data");
                                                            //File.Delete(FileNameModifyFirst);
                                                        }
                                                    }
                                                    else {
                                                        File.Delete(filePath);
                                                        filePathError.Add(FileNameModifyFirst);
                                                        LogFile.write(Status.ERROR.ToString(), filePath + " is string could not be parsed.");
                                                        //File.Delete(FileNameModifyFirst);
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                File.Delete(filePath);
                                                filePathError.Add(FileNameModifyFirst);
                                                LogFile.write(Status.ERROR.ToString(), filePath + " is not TXT. ");
                                                //File.Delete(FileNameModifyFirst);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        filePathError.Add(FileNameModifyFirst);
                                        LogFile.write(Status.ERROR.ToString(), FileNameModifyFirst + " is not zip. ");
                                        //File.Delete(FileNameModifyFirst);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    filePathError.Add(FileNameModifyFirst);
                                    LogFile.write(Status.ERROR.ToString(), FileNameModifyFirst + " : " + ex.Message);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                LogFile.write(Status.ERROR.ToString(), "Initial Setting is not Correctly.");
            }

            LogFile.write(Status.INFO.ToString(), "End Process");

        }

        private static void deleteFileBin(string directoryPath)
        {

            DataTable dtFile = new DataTable();
            dtFile.Columns.Add("PathFileName", typeof(string));
            dtFile.Columns.Add("DateModify", typeof(DateTime));
            dtFile.Columns.Add("FileName", typeof(string));
            dtFile.Columns.Add("FileType", typeof(string));

            string[] zipFiles = Directory.GetFiles(directoryPath);

            foreach (string fileZipPath in zipFiles.ToList())
            {
                DateTime lastModified = System.IO.File.GetLastWriteTime(fileZipPath);
                string[] splitName = fileZipPath.Split('.');
                string lastSplit = splitName[0];
                string extension = Path.GetExtension(fileZipPath);

                DataRow drFile = dtFile.NewRow();
                drFile[0] = fileZipPath;
                drFile[1] = lastModified;
                drFile[2] = lastSplit;
                drFile[3] = extension;
                dtFile.Rows.Add(drFile);
            }

            List<string> tmpFileName = new List<string>();

            tmpFileName = (from dt in dtFile.AsEnumerable()
                           group dt by dt[2] into grp
                           where grp.Count() > 1
                           select grp.Key.ToString()
                            ).Union(
                            from dt in dtFile.AsEnumerable()
                            where dt[3].Equals("")
                            select dt[2].ToString()
                            ).ToList();

            foreach (string tmp in tmpFileName)
            {
                List<string> pathFile = new List<string>();
                try
                {
                    pathFile = (from dt in dtFile.AsEnumerable()
                                where dt[2].Equals(tmp)
                                select dt[0].ToString()).ToList();

                    foreach (string p in pathFile)
                    {
                        File.Delete(p);
                    }
                }
                catch (Exception ex)
                {
                    LogFile.write(Status.ERROR.ToString(), ex.Message);
                }
            }
        }
    }
}
