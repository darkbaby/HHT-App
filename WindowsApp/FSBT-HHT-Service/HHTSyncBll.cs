using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Runtime.InteropServices;
using PortableDeviceApiLib;
using System.Net;
using OpenNETCF.Desktop.Communication;
using FSBT_HHT_Model;
using FSBT_HHT_DAL.DAO;
using FtpLib;
using System.Data;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;

namespace FSBT_HHT_BLL
{
    public class HHTSyncBll
    {
        private LogErrorBll logBll = new LogErrorBll(); 
        private HHTSyncDAO dao = new HHTSyncDAO();
        private RAPI rapi = new RAPI();
        private string DBName = LocalParameter.DBName;
        private string validateDBName = LocalParameter.validateDBName;
        private string userFTP = "anonymous";
        private string passwordFTP = "s";

        public string GetComputerName()
        {
            string hostName = Dns.GetHostName();
            return hostName;
        }

        public bool D_IsDevicePlugIn()
        {
            return rapi.DevicePresent;
        }

        public void D_ConnectDevice()
        {
            if (!rapi.Connected)
            {
                rapi.Connect();
            }
        }

        public void D_DisconnectDevice()
        {
            if (rapi.Connected)
            {
                rapi.Disconnect();
            }
        }

        public string D_GetDeviceName()
        {
            try
            {
                string deviceName = CERegistry.LocalMachine.OpenSubKey("Ident").GetValue("Name").ToString();
                return deviceName;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message , DateTime.Now);
                Console.WriteLine("Error From D_GetDeviceName");
                return "";
            }
        }

        public bool IsStocktakingIDExist(string stocktakingID)
        {
            return dao.CheckStocktakingIDExist(stocktakingID);
            
        }

        public bool D_PCTransferFileToHHT(string HHTDBPath)
        {
            try
            {
                //string fileToPushToDevice = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
                string fileToPushToDevice = Path.Combine(LocalParameter.programPath, DBName);
                string remotePath = HHTDBPath + DBName;
                rapi.CopyFileToDevice(fileToPushToDevice, remotePath, true);
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool D_HHTTransferFileToPC(bool isCheckPermissionMode, string HHTDBPath)
        {
            string localPath = "";
            string fileToPullFromDevice = "";
            try
            {
                if (isCheckPermissionMode)
                {
                    fileToPullFromDevice = HHTDBPath + validateDBName;
                    localPath = Path.Combine(LocalParameter.programPath, validateDBName);
                }
                else
                {
                    fileToPullFromDevice = HHTDBPath + DBName;
                    localPath = Path.Combine(LocalParameter.programPath, DBName);
                }

                rapi.CopyFileFromDevice(localPath, fileToPullFromDevice, true);
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                Console.WriteLine("Error from D_HHTTransferFileToPC");
                return false;
            }
        }

        public bool D_HHTTransferFileUploadToPC(string localPath, string HHTTempPath)
        {
            try
            {
                string DicToPullFromDevice = HHTTempPath;
                if (rapi.DeviceFileExists(DicToPullFromDevice))
                {
                    if (rapi.DeviceFileExists(DicToPullFromDevice + "\\information.txt"))
                    {
                        //string txtPath = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\information.txt";
                        string txtPath = Path.Combine(LocalParameter.programPath,"information.txt");
                        
                        rapi.CopyFileFromDevice(txtPath, DicToPullFromDevice + "\\information.txt", true);
                        
                        string[] lines = System.IO.File.ReadAllLines(txtPath);
                        string fileName = "";
                        string fileNameZip = "";
                        foreach (string line in lines)
                        {
                            // Use a tab to indent each line of the file.
                            //Console.WriteLine("\t" + line);
                            fileName = line;
                            fileNameZip = line + ".zip";
                        }
                        if (fileName != string.Empty)
                        {
                            if (Directory.Exists(localPath))
                            {
                                //rapi.CopyFileFromDevice(localPath, fileName, true);
                                rapi.CopyFileFromDevice(localPath + "\\" + fileName, HHTTempPath + "\\" + fileName, true);
                                rapi.CopyFileFromDevice(localPath + "\\" + fileNameZip, HHTTempPath + "\\" + fileNameZip, true);
                                File.Delete(localPath + "\\" + fileName);
                                //string successFilePath = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\success.txt";

                                string successFilePath = LocalParameter.programPath + "\\success.txt";

                                FileStream fileStream = File.Create(successFilePath);
                                fileStream.Close();                            
                                rapi.CopyFileToDevice(successFilePath, HHTTempPath + "\\success.txt", true);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }

                        //FileList fileList = rapi.EnumFiles(DicToPullFromDevice);
                        //foreach(FileInformation file in fileList)
                        //{
                        //    string temp = file.FileName;
                        //    //rapi.CopyFileFromDevice(localPath, file, true);
                        //}                    
                        
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                Console.WriteLine("Error from D_HHTTransferFileToPC");
                return false;
            }
        }

        public List<DownloadLocationModel> GetLocation(string locationFrom, string LocationTo)
        {
            List<DownloadLocationModel> downloadLocationList = dao.GetLocationList(locationFrom, LocationTo);
            return downloadLocationList;
        }       

        public List<PCSKUModel> GetSKU(string scanMode)
        {
            List<PCSKUModel> skuList = dao.GetSKUList(scanMode);
            return skuList;
        }

        public List<PCSKUModel> GetSKU()
        {
            List<PCSKUModel> skuList = dao.GetSKUList();
            return skuList;
        }

        public List<PCSKUModel> GetSKUAll()
        {
            List<PCSKUModel> skuList = dao.GetSKUListAll();
            return skuList;
        }

        public List<UnitModel> GetUnit()
        {
            List<UnitModel> unitList = dao.GetUnit();
            return unitList;
        }

        public List<ScanModeModel> GetScanMode()
        {
            List<ScanModeModel> scanModeList = dao.GetScanMode();
            return scanModeList;
        }

        public List<MasterBarcodeModel> GetMasterBarcode()
        {
            List<MasterBarcodeModel> masterBarcodeList = dao.GetMasterBarcode();
            return masterBarcodeList;
        }

        public List<MasterSerialNumberModel> GetMasterSerialNumber()
        {
            List<MasterSerialNumberModel> masterSerialNumberList = dao.GetMasterSerialNumber();
            return masterSerialNumberList;
        }

        public List<MasterPackModel> GetMasterPack()
        {
            List<MasterPackModel> masterPackList = dao.GetMasterPack();
            return masterPackList;
        }

        public List<AuditStocktakingModel> GetAuditTempList()
        {
            //log.Info("in HHTSyncBLL GetAuditTempList function");
            return dao.GetRecordsFromTemp();
        }

        public InsertAutoResultModel SaveAutoImport(List<AuditStocktakingModel> auditRecord)
        {
            return dao.InsertAutoImport(auditRecord);
        }

        public bool SaveDownloadLog(List<DownloadLocationModel> downloadLocationList, List<PCSKUModel> skuList, string deviceName, string userName)
        {
            return dao.SaveDownloadLog(downloadLocationList, skuList, deviceName, userName);
        }

        public List<string> GetComputerList()
        {
            string dbfile = Path.Combine(LocalParameter.programPath, validateDBName);
            return dao.GetComputerList(dbfile);
        }

        public List<AuditStocktakingModel> GetAuditList()
        {
            string dbfile = Path.Combine(LocalParameter.programPath, DBName);
            List<AuditStocktakingModel> auditList = dao.GetAuditList(dbfile);
            return auditList;
        }

        public void DeleteDBFile()
        {
            try
            {
                //string computerNameDB = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + validateDBName;
                //string stocktakinDB = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;

                string computerNameDB = Path.Combine(LocalParameter.programPath, validateDBName);
                string stocktakinDB = Path.Combine(LocalParameter.programPath, DBName);

                if (File.Exists(computerNameDB))
                {
                    File.Delete(computerNameDB);
                }
                if (File.Exists(stocktakinDB))
                {
                    File.Delete(stocktakinDB);
                } 
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                Console.WriteLine("delete DB file error");
            }
            
        }

        public bool FTPTransferHHTToPC(string hostIP, bool checkpermissionMode, string HHTDBPath)
        {
            using (FtpConnection ftp = new FtpConnection(hostIP, userFTP, passwordFTP))
            {
                try
                {
                    ftp.Open(); /* Open the FTP connection */
                    ftp.Login(userFTP, passwordFTP); /* Login using previously provided credentials */

                    string remoteFile = "";
                    string localFile = "";

                    if (checkpermissionMode)
                    {
                        remoteFile = HHTDBPath + validateDBName;

                        //localFile = new System.IO.FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) + "\\" + validateDBName;
                        localFile = Path.Combine(LocalParameter.programPath, validateDBName);
                    }
                    else
                    {
                        remoteFile = HHTDBPath + DBName;
                        localFile = Path.Combine(LocalParameter.programPath, DBName);
                       // localFile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
                    }

                    if (ftp.FileExists(remoteFile)) /* check that a file exists */
                    {
                        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ftp.GetFile" + remoteFile , DateTime.Now);
                        ftp.GetFile(remoteFile, localFile, false); /* download /incoming/file.txt as file.txt to current executing directory, overwrite if it exists */
                        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ftp.GetFile" + remoteFile + "success", DateTime.Now);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(String.Format("FTP Error: {0} {1}", e.ErrorCode, e.Message));
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    return false;
                }
            }
        }

        public bool FTPTransferPCToHHT(string hostIP, string HHTDBPath)
        {
            using (FtpConnection ftp = new FtpConnection(hostIP, userFTP, passwordFTP))
            {
                try
                {
                    ftp.Open(); /* Open the FTP connection */
                    ftp.Login(userFTP, passwordFTP); /* Login using previously provided credentials */

                    string remoteFile = HHTDBPath + DBName;
                    //string localFile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
                    string localFile = Path.Combine(LocalParameter.programPath, DBName);
                    ftp.PutFile(localFile, remoteFile); 
                    return true;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(String.Format("FTP Error: {0} {1}", e.ErrorCode, e.Message));
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    return false;
                }
            }
        }

        public string GetDeviceNameFTP()
        {
            string dbfile = Path.Combine(LocalParameter.programPath, DBName);
            return dao.GetDeviceName(dbfile);
        }

        public DataTable LoadReport_StocktakingAuditCheckWithUnit_AutoPrint(string LocationCode, string StoreType, DateTime countDate, string allDepartmentCode, string allSectionCode, string allBrandCode)
        {
            return dao.GetReportAutoPrint_StocktakingAuditCheckWithUnit(LocationCode, StoreType, countDate, allDepartmentCode, allSectionCode, allBrandCode);
        }

        public DataTable LoadReport_StocktakingAuditCheckWithUnit_AutoPrint(string LocationCode,  DateTime countDate)
        {
            return dao.GetReportAutoPrint_StocktakingAuditCheckWithUnit(LocationCode,  countDate);
        }

        public bool UpdatePrintFlag(DataTable dt)
        {
            return dao.UpdatePrintFlagInHHTStocktaking(dt);
        }

        public bool UpdateImportFlag(List<AuditStocktakingModel> auditListTempOneLocation)
        {
            return dao.UpdateImportFlagInTmpHHTStocktaking(auditListTempOneLocation);
        }

        public bool ConnectSdfDeleteDataStockTaking(List<AuditStocktakingModel>[] auditListByLocationArr)
        {
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;

            string dbfile = Path.Combine(LocalParameter.programPath,DBName);
            return dao.ConnectSdfDeleteDataStockTaking(auditListByLocationArr, dbfile);
        }

        public bool ConnectSdfDeleteData(string type)
        {
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;

            string dbfile = Path.Combine(LocalParameter.programPath, DBName);
            return dao.ConnectSdfDeleteData(type, dbfile);
        }

        public bool ConnectSdfInsertLocationData(List<DownloadLocationModel> locationList, string username)
        {
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
            //string dbfile = new System.IO.FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) + "\\" + DBName;
            string dbfile = Path.Combine(LocalParameter.programPath, DBName);
            return dao.ConnectSdfInsertLocationData(locationList, dbfile, username);
        }

        public bool ConnectSdfInsertMasterBarcodeData(List<MasterBarcodeModel> masterBarcodeList, string username)
        {
            // Create a connection to the file datafile.sdf in the program folder
            //string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
            string dbfile = Path.Combine(LocalParameter.programPath, DBName);
            return dao.ConnectSdfInsertMasterBarcodeData(masterBarcodeList, dbfile, username);
        }
        public bool ConnectSdfInsertMasterSerialData(List<MasterSerialNumberModel> masterSerialNumberList, string username)
        {
            string dbfile = Path.Combine(LocalParameter.programPath, DBName);
            return dao.ConnectSdfInsertMasterSerialData(masterSerialNumberList, dbfile, username);

        }

        public bool ConnectSdfInsertMasterPackData(List<MasterPackModel> masterPackList, string username)
        {
            string dbfile = Path.Combine(LocalParameter.programPath, DBName);
            return dao.ConnectSdfInsertMasterPackData(masterPackList, dbfile, username);
        }

        public bool ConnectSdfInsertSKUData(List<PCSKUModel> skuList, string username)
        {
            string dbfile = Path.Combine(LocalParameter.programPath, DBName);
            return dao.ConnectSdfInsertSKUData(skuList, dbfile, username);
        }

        public bool ConnectSdfInsertUnitData(List<UnitModel> unitList, string username)
        {
            string dbfile = Path.Combine(LocalParameter.programPath, DBName);
            return dao.ConnectSdfInsertUnitData(unitList, dbfile, username);
        }
    }
}
