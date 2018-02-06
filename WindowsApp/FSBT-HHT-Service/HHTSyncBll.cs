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

namespace FSBT_HHT_BLL
{
    public class HHTSyncBll
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        private HHTSyncDAO dao = new HHTSyncDAO();
        private RAPI rapi = new RAPI();
        private string DBName = "STOCKTAKING_HHT.sdf";
        private string validateDBName = "COMPUTER_NAME.sdf";
        private string HHTDBPath = @"\Flash\TheMall-Stocktaking\Database\";
        //private string HHTDBPath = @"\Program Files\denso-hht\Database\";
        private string HHTTempPath = @"\Flash\TheMall-Stocktaking\temp";
        //private string HHTDBPath = @"\Program Files\denso-hht\Database\";
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                Console.WriteLine("Error From D_GetDeviceName");
                return "";
            }
        }

        public bool IsStocktakingIDExist(string stocktakingID)
        {
            return dao.CheckStocktakingIDExist(stocktakingID);
            
        }

        public bool D_PCTransferFileToHHT()
        {
            try
            {
                string fileToPushToDevice = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
                //string fileToPushToDevice = "D:\\TestDB1.sdf";
                string remotePath = HHTDBPath + DBName;
                rapi.CopyFileToDevice(fileToPushToDevice, remotePath, true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                Console.Error.WriteLine("Error from D_PCTransferFileToHHT");
                return false;
            }
        }

        public bool D_HHTTransferFileToPC(bool isCheckPermissionMode)
        {
            try
            {
                string fileToPullFromDevice = "";
                string localPath = "";
                if (isCheckPermissionMode)
                {
                    fileToPullFromDevice = HHTDBPath + validateDBName;
                    localPath = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + validateDBName;
                    //localPath = "D:\\COMPUTER_NAME.sdf";
                }
                else
                {
                    fileToPullFromDevice = HHTDBPath + DBName;
                    localPath = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
                    //localPath ="D:\\STOCKTAKING_HHT.sdf";
                }

                rapi.CopyFileFromDevice(localPath, fileToPullFromDevice, true);
                //rapi.CreateDeviceDirectory(@"\Program Files\Test");
                return true;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                Console.WriteLine("Error from D_HHTTransferFileToPC");
                return false;
            }
        }

        public bool D_HHTTransferFileUploadToPC(string localPath)
        {
            try
            {

                string DicToPullFromDevice = HHTTempPath;
                //string localPath = ConfigurationManager.AppSettings["pathFTPFolder"];
                if (rapi.DeviceFileExists(DicToPullFromDevice))
                {
                    if (rapi.DeviceFileExists(DicToPullFromDevice + "\\information.txt"))
                    {
                        string txtPath = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\information.txt";
                        
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
                                string successFilePath = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\success.txt";
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
            return dao.GetComputerList();
        }

        public List<AuditStocktakingModel> GetAuditList()
        {
            List<AuditStocktakingModel> auditList = dao.GetAuditList();
            return auditList;
        }

        public void DeleteDBFile()
        {
            try
            {
                string computerNameDB = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + validateDBName;
                string stocktakinDB = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                Console.WriteLine("delete DB file error");
            }
            
        }

        public bool FTPTransferHHTToPC(string hostIP, bool checkpermissionMode)
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
                        localFile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + validateDBName;
                    }
                    else
                    {
                        remoteFile = HHTDBPath + DBName;
                        localFile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
                    }

                
                    if (ftp.FileExists(remoteFile)) /* check that a file exists */
                    {
                        ftp.GetFile(remoteFile, localFile, false); /* download /incoming/file.txt as file.txt to current executing directory, overwrite if it exists */
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
                    log.Error(String.Format("Exception : {0}", ex.StackTrace));
                    return false;
                }
            }
        }

        public bool FTPTransferPCToHHT(string hostIP)
        {
            using (FtpConnection ftp = new FtpConnection(hostIP, userFTP, passwordFTP))
            {
                try
                {
                    ftp.Open(); /* Open the FTP connection */
                    ftp.Login(userFTP, passwordFTP); /* Login using previously provided credentials */

                    string remoteFile = HHTDBPath + DBName;
                    string localFile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + DBName;
               
                    ftp.PutFile(localFile, remoteFile); 
                    return true;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(String.Format("FTP Error: {0} {1}", e.ErrorCode, e.Message));
                    log.Error(String.Format("Exception : {0}", ex.StackTrace));
                    return false;
                }
            }
        }

        public string GetDeviceNameFTP()
        {
            return dao.GetDeviceName();
        }

        public DataTable LoadReport_StocktakingAuditCheckWithUnit_AutoPrint(string LocationCode, string StoreType, DateTime countDate, string allDepartmentCode, string allSectionCode, string allBrandCode)
        {
            return dao.GetReportAutoPrint_StocktakingAuditCheckWithUnit(LocationCode, StoreType, countDate, allDepartmentCode, allSectionCode, allBrandCode);
        }

        public bool UpdatePrintFlag(DataTable dt)
        {
            return dao.UpdatePrintFlagInHHTStocktaking(dt);
        }

        public bool UpdateImportFlag(List<AuditStocktakingModel> auditListTempOneLocation)
        {
            return dao.UpdateImportFlagInTmpHHTStocktaking(auditListTempOneLocation);
        }

    }



    //public class FTP
    //{
    //    private string host = null;
    //    private string user = null;
    //    private string pass = null;
    //    private FtpWebRequest ftpRequest = null;
    //    private FtpWebResponse ftpResponse = null;
    //    private Stream ftpStream = null;
    //    private int bufferSize = 4000;

    //    /* Construct Object */
    //    public FTP(string hostIP, string userName, string password) { host = hostIP; user = userName; pass = password; }

    //    /* Download File */
    //    public bool TransferHHTToPC(bool checkPermission)
    //    {
    //        string remoteFile = @"\Program Files\denso_hht\Database\COMPUTER_NAME.sdf";
    //        string localFile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\COMPUTER_NAME.sdf";
    //        ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);

    //        ftpRequest.Credentials = new NetworkCredential(user, pass);

    //        FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();

    //        Stream responseStream = response.GetResponseStream();
    //        StreamReader reader = new StreamReader(responseStream);
    //        FileStream file = File.Create(localFile);
    //        byte[] buffer = new byte[32 * 1024];
    //        int read;
    //        while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
    //        {
    //            file.Write(buffer, 0, read);
    //        }
    //        file.Close();
    //        reader.Close();
    //        response.Close();
    //        return true;

    //        //try
    //        //{
    //        //    /* Create an FTP Request */
    //        //    //System.Net.ServicePointManager.Expect100Continue = false;
    //        //    string remoteFile = "";
    //        //    string localFile = "";
    //        //    if (checkPermission)
    //        //    {
    //        //        remoteFile = @"\Program Files\denso_hht\Database\COMPUTER_NAME.sdf";
    //        //        localFile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\COMPUTER_NAME.sdf";
    //        //        //remoteFile = @"\Program Files\denso_hht\Database\test_transfer.txt";
    //        //        //localFile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\test_transfer1.txt";
    //        //    }
    //        //    else
    //        //    {
    //        //        remoteFile = @"\Program Files\denso_hht\Database\STOCKTAKING_HHT.sdf";
    //        //        localFile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\STOCKTAKING_HHT.sdf";
    //        //    }                
                
    //        //    ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
    //        //    /* Log in to the FTP Server with the User Name and Password Provided */
    //        //    ftpRequest.Credentials = new NetworkCredential(user, pass);
    //        //    /* When in doubt, use these options */
    //        //    ftpRequest.UseBinary = true;
    //        //    ftpRequest.UsePassive = true;
    //        //    ftpRequest.KeepAlive = true;
    //        //    /* Specify the Type of FTP Request */
    //        //    ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
    //        //    /* Establish Return Communication with the FTP Server */
    //        //    ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
    //        //    /* Get the FTP Server's Response Stream */
    //        //    ftpStream = ftpResponse.GetResponseStream();
    //        //    /* Open a File Stream to Write the Downloaded File */
    //        //    FileStream localFileStream = new FileStream(localFile, FileMode.Create);
    //        //    /* Buffer for the Downloaded Data */
    //        //    byte[] byteBuffer = new byte[bufferSize];
    //        //    int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
    //        //    /* Download the File by Writing the Buffered Data Until the Transfer is Complete */
    //        //    try
    //        //    {
    //        //        while (bytesRead > 0)
    //        //        {
    //        //            localFileStream.Write(byteBuffer, 0, bytesRead);
    //        //            bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
    //        //        }
                    
    //        //    }
    //        //    catch (Exception ex) 
    //        //    { 
    //        //        Console.WriteLine(ex.ToString());
    //        //        return false;
    //        //    }

    //        //    /* Resource Cleanup */
    //        //    localFileStream.Close();
    //        //    ftpStream.Close();
    //        //    ftpResponse.Close();
    //        //    ftpRequest = null;
    //        //    return true;
                
    //        //}
    //        //catch (Exception ex) 
    //        //{ 
    //        //    Console.WriteLine(ex.ToString());
    //        //    return false;
    //        //}
            
    //    }

    //    /* Upload File */
    //    public bool TransferPCToHHT()
    //    {
    //        try
    //        {
    //            /* Create an FTP Request */
    //            string remoteFile = @"\Program Files\denso_hht\Database\STOCKTAKING_HHT.sdf";
    //            string localFile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\STOCKTAKING_HHT.sdf";
    //            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
    //            /* Log in to the FTP Server with the User Name and Password Provided */
    //            ftpRequest.Credentials = new NetworkCredential(user, pass);
    //            /* When in doubt, use these options */
    //            ftpRequest.UseBinary = true;
    //            ftpRequest.UsePassive = true;
    //            ftpRequest.KeepAlive = true;
    //            /* Specify the Type of FTP Request */
    //            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
    //            /* Establish Return Communication with the FTP Server */
    //            ftpStream = ftpRequest.GetRequestStream();
    //            /* Open a File Stream to Read the File for Upload */
    //            FileStream localFileStream = new FileStream(localFile, FileMode.Create);
    //            /* Buffer for the Downloaded Data */
    //            byte[] byteBuffer = new byte[bufferSize];
    //            int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
    //            /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
    //            try
    //            {
    //                while (bytesSent != 0)
    //                {
    //                    ftpStream.Write(byteBuffer, 0, bytesSent);
    //                    bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
    //                }

    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine(ex.ToString());
    //                return false;
    //            }
    //            /* Resource Cleanup */
    //            localFileStream.Close();
    //            ftpStream.Close();
    //            ftpRequest = null;
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.ToString());
    //            return false;
    //        }

    //    }
    //}

}
