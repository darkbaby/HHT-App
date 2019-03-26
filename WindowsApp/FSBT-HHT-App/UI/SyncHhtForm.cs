using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSBT_HHT_Model;
using System.Management;
using System.IO;
using FSBT_HHT_BLL;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Data.SqlServerCe;
using System.Configuration;
using System.Data.SqlClient;
using FSBT_HHT_DAL;
using System.Globalization;
using FSBT.HHT.App.Resources;
using CrystalDecisions.Shared;
using System.Reflection;

namespace FSBT.HHT.App.UI
{
    public partial class SyncHhtForm : Form
    {
        private LogErrorBll logBll = new LogErrorBll();
        private string sleepTime = ConfigurationManager.AppSettings["sleepTime"];
        private string HHTDBPath = ConfigurationManager.AppSettings["HHTDBPath"];
        private string HHTTempPath = ConfigurationManager.AppSettings["HHTTempPath"];
        private string UserName { get; set; }
        //public LayoutWmenu layoutForm { get; set; }               
        private WqlEventQuery queryInsert;
        private WqlEventQuery queryRemove;
        //private ManagementEventWatcher watcherInsert;
        //private ManagementEventWatcher watcherRemove;
        private HHTSyncBll hhtBll = new HHTSyncBll();
        private bool connectedWiFiDownload = false;
        private bool connectedWiFiUpload = false;
        private bool connectedCable = false;
        private bool originalbtnWifiDEnable;
        private bool originalbtnWifiUEnable;

        Thread importThread;
        Thread printThread;

        SystemSettingBll settings = new SystemSettingBll();
        SystemSettingModel settingModel;

        private Image imgProcess = Image.FromFile(Path.GetFullPath("./Image/processing.ico"));
        private Image imgFinish = Image.FromFile(Path.GetFullPath("./Image/success.ico"));
        private Image imgError = Image.FromFile(Path.GetFullPath("./Image/error.ico"));

        private ReportManagementBll bllReportManagement = new ReportManagementBll();
        ReportManagementForm reportForm = new ReportManagementForm();

        private string DBName = "STOCKTAKING_HHT.sdf";
        private string DBPassword = "1234";
        private int rowPrintCurrent = 0;
        private int Seq = 1;

        List<AuditStocktakingModel> auditList = new List<AuditStocktakingModel>();
        string ipAddress = "";
        string ipAddressUpload = "";

        public void AutoImportToHHTStocktaking(List<AuditStocktakingModel> auditListTempOneLocation, int rowStart)
        {
            InsertAutoResultModel insertResult = hhtBll.SaveAutoImport(auditListTempOneLocation);
            Thread.Sleep(200);
            if (insertResult.result == true)
            {
                if (this.dataGridViewAutoUp.InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        dataGridViewAutoUp.Rows[rowStart].Cells[7] = new TextAndIconCell();
                        dataGridViewAutoUp.Rows[rowStart].Cells[7].Value = "Finished";
                        ((TextAndIconCell)dataGridViewAutoUp.Rows[rowStart].Cells[7]).Image = imgFinish;
                    });
                }
                else
                {
                    dataGridViewAutoUp.Rows[rowStart].Cells[7] = new TextAndIconCell();
                    dataGridViewAutoUp.Rows[rowStart].Cells[7].Value = "Finished";
                    ((TextAndIconCell)dataGridViewAutoUp.Rows[rowStart].Cells[7]).Image = imgFinish;
                }
            }
            else
            {
                string location = auditListTempOneLocation.Select(a => a.LocationCode).FirstOrDefault();
                hhtBll.UpdateImportFlag(auditListTempOneLocation);
                if (this.dataGridViewAutoUp.InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        dataGridViewAutoUp.Rows[rowStart].Cells[7] = new TextAndIconCell();
                        dataGridViewAutoUp.Rows[rowStart].Cells[7].Value = "Error";
                        ((TextAndIconCell)dataGridViewAutoUp.Rows[rowStart].Cells[7]).Image = imgError;
                        string msg = "Location " + location + " has been uploaded by @Hand-held's ID : " + insertResult.hhtID + ". @Hand-held's name : " + insertResult.hhtName + ". @Stocktaker : " + insertResult.stocktaker + ".";
                        dataGridViewAutoUp.Rows[rowStart].Cells[10].Value = msg.Replace("@", string.Empty);
                        MessageBoxAutoHHTSyncForm messageBox = new MessageBoxAutoHHTSyncForm(msg);
                        messageBox.Show();
                        //MessageBox.Show("Location " + location + "has been uploaded by Hand-held's ID : " + insertResult.hhtID + ". Hand-held's name : " + insertResult.hhtName + ". Stocktaker : " + insertResult.stocktaker + ".");
                    });
                }
                else
                {
                    dataGridViewAutoUp.Rows[rowStart].Cells[7] = new TextAndIconCell();
                    dataGridViewAutoUp.Rows[rowStart].Cells[7].Value = "Error";
                    ((TextAndIconCell)dataGridViewAutoUp.Rows[rowStart].Cells[7]).Image = imgError;
                    string msg = "Location " + location + " has been uploaded by @Hand-held's ID : " + insertResult.hhtID + ". @Hand-held's name : " + insertResult.hhtName + ". @Stocktaker : " + insertResult.stocktaker + ".";
                    dataGridViewAutoUp.Rows[rowStart].Cells[10].Value = msg.Replace("@", string.Empty);
                    MessageBoxAutoHHTSyncForm messageBox = new MessageBoxAutoHHTSyncForm(msg);
                    messageBox.Show();
                    // MessageBox.Show("Location " + location + "has been uploaded by Hand-held's ID : " + insertResult.hhtID + ". Hand-held's name : " + insertResult.hhtName + ". Stocktaker : " + insertResult.stocktaker + ".");
                }
            }
        }
        public void SetAutoUploadGrid()
        {
            //int rowStart = 0;
            //int rowStop = 0;
            
            int rowStop = -1;
            int Time = Convert.ToInt32(sleepTime) * 1000;
            while (true)
            {
                System.Threading.Thread.Sleep(Time);
                List<AuditStocktakingModel> auditListFromTemp = hhtBll.GetAuditTempList();
                if (auditListFromTemp.Count > 0)
                {
                    try
                    {
                        int rowStart = 0;
                        List<AuditStocktakingModel> auditListTempGroupLocation = auditListFromTemp.GroupBy(x => new { x.HHTID, x.HHTName, x.CreateBy, x.FileName, x.LocationCode, x.ImportDate }).Select(g => g.First()).ToList();

                        foreach (AuditStocktakingModel data in auditListTempGroupLocation)
                        {
                            int countLocation = (from a in auditListFromTemp
                                                 where a.HHTID == data.HHTID && a.HHTName == data.HHTName && a.CreateBy == data.CreateBy && a.FileName == data.FileName && a.LocationCode == data.LocationCode && a.ImportDate == data.ImportDate
                                                 select a).Count();
                            decimal sumQTY = (from a in auditListFromTemp
                                              where a.ScanMode != 4 && a.HHTID == data.HHTID && a.HHTName == data.HHTName && a.CreateBy == data.CreateBy && a.FileName == data.FileName && a.LocationCode == data.LocationCode && a.ImportDate == data.ImportDate
                                              select a.Quantity).Sum();

                            int transactionQTY = (from a in auditListFromTemp
                                                  where a.ScanMode == 4 && a.HHTID == data.HHTID && a.HHTName == data.HHTName && a.CreateBy == data.CreateBy && a.FileName == data.FileName && a.LocationCode == data.LocationCode && a.ImportDate == data.ImportDate
                                                  select a.Quantity).Count();

                            int QTY = (int)sumQTY + transactionQTY;

                            if (this.dataGridViewAutoUp.InvokeRequired)
                            {
                                Invoke((MethodInvoker)delegate
                                {
                                    var row1=new string[] { data.HHTID, data.HHTName, data.CreateBy, data.FileName, data.LocationCode, Convert.ToString(countLocation), Convert.ToString(QTY), "Not Start", "Waiting" , (data.ImportDate).ToString("yyyy-MM-dd HH:mm:ss.fff") };
                                    dataGridViewAutoUp.Rows.Insert(0, row1);
                                    //this.dataGridViewAutoUp.Rows.Insert(0, data.HHTID, data.HHTName, data.CreateBy, data.FileName, data.LocationCode, Convert.ToString(countLocation), Convert.ToString(QTY), "Not Start", "Waiting", (data.ImportDate).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                    rowStart++;
                                });
                            }
                            else
                            {
                                var row1 = new string[] { data.HHTID, data.HHTName, data.CreateBy, data.FileName, data.LocationCode, Convert.ToString(countLocation), Convert.ToString(QTY), "Not Start", "Waiting", (data.ImportDate).ToString("yyyy-MM-dd HH:mm:ss.fff") };
                                dataGridViewAutoUp.Rows.Insert(0, row1);
                                //this.dataGridViewAutoUp.Rows.Add(0, data.HHTID, data.HHTName, data.CreateBy, data.FileName, data.LocationCode, Convert.ToString(countLocation), Convert.ToString(QTY), "Not Start", "Waiting", (data.ImportDate).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                rowStart++;
                            }
                            //rowStop++;
                            //rowStart++;
                        }

                        rowStart = rowStart - 1;
                        while (rowStart > rowStop)
                        {
                            string hhtID = "";
                            string hhtName = "";
                            string stocktaker = "";
                            string fileName = "";
                            string location = "";
                            string importDate = "";

                            if (this.dataGridViewAutoUp.InvokeRequired)
                            {
                                Invoke((MethodInvoker)delegate
                                {
                                    hhtID = ((string)dataGridViewAutoUp.Rows[rowStart].Cells[0].Value).Trim();
                                    hhtName = ((string)dataGridViewAutoUp.Rows[rowStart].Cells[1].Value).Trim();
                                    stocktaker = ((string)dataGridViewAutoUp.Rows[rowStart].Cells[2].Value).Trim();
                                    fileName = ((string)dataGridViewAutoUp.Rows[rowStart].Cells[3].Value).Trim();
                                    location = ((string)dataGridViewAutoUp.Rows[rowStart].Cells[4].Value).Trim();
                                    importDate = ((string)dataGridViewAutoUp.Rows[rowStart].Cells[9].Value).Trim();
                                    dataGridViewAutoUp.Rows[rowStart].Cells[7] = new TextAndIconCell();
                                    dataGridViewAutoUp.Rows[rowStart].Cells[7].Value = "Processing...";
                                    ((TextAndIconCell)dataGridViewAutoUp.Rows[rowStart].Cells[7]).Image = imgProcess;
                                });
                            }
                            else
                            {
                                hhtID = ((string)dataGridViewAutoUp.Rows[rowStart].Cells[0].Value).Trim();
                                hhtName = ((string)dataGridViewAutoUp.Rows[rowStart].Cells[1].Value).Trim();
                                stocktaker = ((string)dataGridViewAutoUp.Rows[rowStart].Cells[2].Value).Trim();
                                fileName = ((string)dataGridViewAutoUp.Rows[rowStart].Cells[3].Value).Trim();
                                location = ((string)dataGridViewAutoUp.Rows[rowStart].Cells[4].Value).Trim();
                                importDate = ((string)dataGridViewAutoUp.Rows[rowStart].Cells[9].Value).Trim();
                                dataGridViewAutoUp.Rows[rowStart].Cells[7] = new TextAndIconCell();
                                dataGridViewAutoUp.Rows[rowStart].Cells[7].Value = "Processing...";
                                ((TextAndIconCell)dataGridViewAutoUp.Rows[rowStart].Cells[7]).Image = imgProcess;
                            }

                            List<AuditStocktakingModel> auditListTempOneLocation = (from a in auditListFromTemp
                                                                                    where a.HHTID == hhtID && a.HHTName == hhtName && a.CreateBy == stocktaker && a.FileName == fileName && a.LocationCode == location && a.ImportDate == Convert.ToDateTime(importDate)
                                                                                    orderby a.ImportDate
                                                                                    select a).ToList();
                            if (auditListTempOneLocation[0].FlagLocation == "E")
                            {
                                if (this.dataGridViewAutoUp.InvokeRequired)
                                {
                                    Invoke((MethodInvoker)delegate
                                    {
                                        dataGridViewAutoUp.Rows[rowStart].Cells[7] = new TextAndIconCell();
                                        dataGridViewAutoUp.Rows[rowStart].Cells[7].Value = "Error";
                                        ((TextAndIconCell)dataGridViewAutoUp.Rows[rowStart].Cells[7]).Image = imgError;
                                        dataGridViewAutoUp.Rows[rowStart].Cells[10].Value = "Location doesn't exist.";
                                    });
                                }
                                else
                                {
                                    dataGridViewAutoUp.Rows[rowStart].Cells[7] = new TextAndIconCell();
                                    dataGridViewAutoUp.Rows[rowStart].Cells[7].Value = "Error";
                                    ((TextAndIconCell)dataGridViewAutoUp.Rows[rowStart].Cells[7]).Image = imgError;
                                    dataGridViewAutoUp.Rows[rowStart].Cells[10].Value = "Location doesn't exist.";
                                }
                                hhtBll.UpdateImportFlag(auditListTempOneLocation);
                            }
                            else
                            {
                                AutoImportToHHTStocktaking(auditListTempOneLocation, rowStart);
                            }
                            //rowStart++;
                            rowStart--;
                        }
                    }
                    catch (Exception ex)
                    {
                        //log.Error(String.Format("Exception : {0} {1}", ex.Message, ex.StackTrace));
                        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                    }
                    Thread.Sleep(5000);
                }
            }
        }

        public void AutoPrint()
        {
            int printingRow = 0;

            printingRow = dataGridViewAutoUp.RowCount - 1;
            //dataGridViewAutoUp.Rows.Count == 0 ? 0 : dataGridViewAutoUp.Rows.Count - 1;
            //rowStop =  dataGridViewAutoUp.Rows.Count == 0 ? 0 : dataGridViewAutoUp.Rows.Count - 1;
            int allRow = -1;
            try
            {
                while (true)
                {
                    if (this.dataGridViewAutoUp.InvokeRequired)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                        //if (rowPrintCurrent == 0)
                        //{
                        printingRow = dataGridViewAutoUp.RowCount - 1;
                        //}
                        //else
                        //{
                        //    printingRow = rowPrintCurrent;
                        //}
                        //allRow = dataGridViewAutoUp.RowCount;


                    });
                    }
                    else
                    {
                        //if (rowPrintCurrent == 0)
                        //{
                        printingRow = dataGridViewAutoUp.RowCount - 1;
                        //}
                        //else
                        //{
                        //    printingRow = rowPrintCurrent;
                        //}
                        //allRow = dataGridViewAutoUp.RowCount;
                    }

                    while (printingRow > allRow)
                    {
                        string importStatus = "";
                        string printStatus = "";
                        if (this.dataGridViewAutoUp.InvokeRequired)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                importStatus = (dataGridViewAutoUp.Rows[printingRow].Cells[7].Value).ToString();
                                printStatus = (dataGridViewAutoUp.Rows[printingRow].Cells[8].Value).ToString();
                            });
                        }
                        else
                        {
                            importStatus = (dataGridViewAutoUp.Rows[printingRow].Cells[7].Value).ToString();
                            printStatus = (dataGridViewAutoUp.Rows[printingRow].Cells[8].Value).ToString();
                        }

                        if (printStatus == "Done" || printStatus == "Error")
                        {
                            printingRow--;
                        }
                        else
                        {
                            if (importStatus == "Finished")
                            {
                                string locationCode = (string)dataGridViewAutoUp.Rows[printingRow].Cells[4].Value;
                                SystemSettingBll settingBLL = new SystemSettingBll();
                                DateTime countDate = new DateTime();
                                var settingData = settingBLL.GetSettingData();
                                countDate = settingData.CountDate;
                                string allStoreType = "1,2,3,4";
                                //string allLocationCode = string.Empty;
                                string allLocationCode = locationCode;
                                string allDepartmentCode = string.Empty;
                                string allSectionCode = string.Empty;
                                string allBrandCode = string.Empty;
                                string reportName = bllReportManagement.GetReportNameByReportCode("R08");
                                //DataTable dt = hhtBll.LoadReport_StocktakingAuditCheckWithUnit_AutoPrint(allLocationCode, allStoreType, countDate, allDepartmentCode, allSectionCode, allBrandCode);
                                DataTable dt = hhtBll.LoadReport_StocktakingAuditCheckWithUnit_AutoPrint(allLocationCode, countDate);
                                ParameterFields paramFields = new ParameterFields();
                                ParameterField paramField1 = new ParameterField();
                                ParameterField paramField2 = new ParameterField();
                                ParameterField paramField3 = new ParameterField();
                                ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();
                                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                                paramField1.Name = "pCountDate";
                                paramDiscreteValue.Value = countDate;
                                paramField1.CurrentValues.Add(paramDiscreteValue);

                                paramField2.Name = "pBranchName";
                                paramDiscreteValue1.Value = settingData.Branch;
                                paramField2.CurrentValues.Add(paramDiscreteValue1);

                                paramField3.Name = "pReportName";
                                paramDiscreteValue2.Value = reportName;
                                paramField3.CurrentValues.Add(paramDiscreteValue2);

                                paramFields.Add(paramField1);
                                paramFields.Add(paramField2);
                                paramFields.Add(paramField3);

                                if (dt.Rows.Count > 0)
                                {
                                    if (this.dataGridViewAutoUp.InvokeRequired)
                                    {
                                        Invoke((MethodInvoker)delegate
                                        {
                                            dataGridViewAutoUp.Rows[printingRow].Cells[8] = new TextAndIconCell();
                                            dataGridViewAutoUp.Rows[printingRow].Cells[8].Value = "Printing...";
                                            ((TextAndIconCell)dataGridViewAutoUp.Rows[printingRow].Cells[8]).Image = imgProcess;
                                        });
                                    }
                                    else
                                    {
                                        dataGridViewAutoUp.Rows[printingRow].Cells[8] = new TextAndIconCell();
                                        dataGridViewAutoUp.Rows[printingRow].Cells[8].Value = "Printing...";
                                        ((TextAndIconCell)dataGridViewAutoUp.Rows[printingRow].Cells[8]).Image = imgProcess;
                                    }
                                    DataTable dtToPrint = (from d in dt.AsEnumerable()
                                                           select d).CopyToDataTable();
                                    dtToPrint.Columns.Remove("StocktakingID");

                                    //dtToPrint = (from d in dtToPrint.AsEnumerable()
                                    //             select d).Distinct().CopyToDataTable();

                                    //dtToPrint = dtToPrint.AsEnumerable().GroupBy(x => x.Field<string>("Barcode")).Select(g => g.First()).CopyToDataTable();

                                    bool isPrintReportSuccess = reportForm.PrintReport(dtToPrint, "R08", paramFields);
                                    //log.Info("print success status : " + isPrintReportSuccess);
                                    hhtBll.UpdatePrintFlag(dt);
                                    if (isPrintReportSuccess)
                                    {

                                        if (this.dataGridViewAutoUp.InvokeRequired)
                                        {
                                            Invoke((MethodInvoker)delegate
                                            {
                                                dataGridViewAutoUp.Rows[printingRow].Cells[8] = new TextAndIconCell();
                                                dataGridViewAutoUp.Rows[printingRow].Cells[8].Value = "Done";
                                                ((TextAndIconCell)dataGridViewAutoUp.Rows[printingRow].Cells[8]).Image = imgFinish;
                                                printingRow--;
                                            });
                                        }
                                        else
                                        {
                                            dataGridViewAutoUp.Rows[printingRow].Cells[8] = new TextAndIconCell();
                                            dataGridViewAutoUp.Rows[printingRow].Cells[8].Value = "Done";
                                            ((TextAndIconCell)dataGridViewAutoUp.Rows[printingRow].Cells[8]).Image = imgFinish;
                                            printingRow--;
                                        }

                                    }
                                    else
                                    {
                                        if (this.dataGridViewAutoUp.InvokeRequired)
                                        {
                                            Invoke((MethodInvoker)delegate
                                            {
                                                dataGridViewAutoUp.Rows[printingRow].Cells[8] = new TextAndIconCell();
                                                dataGridViewAutoUp.Rows[printingRow].Cells[8].Value = "Error";
                                                ((TextAndIconCell)dataGridViewAutoUp.Rows[printingRow].Cells[8]).Image = imgError;
                                                printingRow--;

                                            });
                                        }
                                        else
                                        {
                                            dataGridViewAutoUp.Rows[printingRow].Cells[8] = new TextAndIconCell();
                                            dataGridViewAutoUp.Rows[printingRow].Cells[8].Value = "Error";
                                            ((TextAndIconCell)dataGridViewAutoUp.Rows[printingRow].Cells[8]).Image = imgError;
                                            printingRow--;
                                        }
                                    }
                                }
                                else
                                {
                                    //กรณีที่ปริ้นไปแล้วตั้งแต่อันแรก
                                    if (this.dataGridViewAutoUp.InvokeRequired)
                                    {
                                        Invoke((MethodInvoker)delegate
                                        {
                                            dataGridViewAutoUp.Rows[printingRow].Cells[8] = new TextAndIconCell();
                                            dataGridViewAutoUp.Rows[printingRow].Cells[8].Value = "Done";
                                            ((TextAndIconCell)dataGridViewAutoUp.Rows[printingRow].Cells[8]).Image = imgFinish;
                                            printingRow--;
                                        });
                                    }
                                    else
                                    {
                                        dataGridViewAutoUp.Rows[printingRow].Cells[8] = new TextAndIconCell();
                                        dataGridViewAutoUp.Rows[printingRow].Cells[8].Value = "Done";
                                        ((TextAndIconCell)dataGridViewAutoUp.Rows[printingRow].Cells[8]).Image = imgFinish;
                                        printingRow--;
                                    }
                                }
                            }
                            else if (importStatus == "Error")
                            {
                                if (this.dataGridViewAutoUp.InvokeRequired)
                                {
                                    Invoke((MethodInvoker)delegate
                                    {
                                        dataGridViewAutoUp.Rows[printingRow].Cells[8] = new TextAndIconCell();
                                        dataGridViewAutoUp.Rows[printingRow].Cells[8].Value = "Error";
                                        ((TextAndIconCell)dataGridViewAutoUp.Rows[printingRow].Cells[8]).Image = imgError;
                                        printingRow--;

                                    });
                                }
                                else
                                {
                                    dataGridViewAutoUp.Rows[printingRow].Cells[8] = new TextAndIconCell();
                                    dataGridViewAutoUp.Rows[printingRow].Cells[8].Value = "Error";
                                    ((TextAndIconCell)dataGridViewAutoUp.Rows[printingRow].Cells[8]).Image = imgError;
                                    printingRow--;
                                }
                            }
                            else
                            {
                                printingRow--;
                                //กรณีที่ยังเป็น processing ใน import status
                                continue;
                            }
                            //printingRow--;
                        }
                    }
                    Thread.Sleep(10000);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        #region Prepare
        //public SyncHhtForm()
        //{
        //    InitializeComponent();
        //    SetAutoUploadGrid();
        //}
        public SyncHhtForm(string username)
        {
            InitializeComponent();
            this.ControlBox = false;
            UserName = username;
            dataGridViewAutoUp.Columns[10].DefaultCellStyle.ForeColor = Color.Red;
        }

        public void CallThread()
        {
            settingModel = settings.GetSettingData();
            try
            {
                importThread = new Thread(new ThreadStart(SetAutoUploadGrid));
                importThread.Start();
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "importThread start", DateTime.Now);

                if (settingModel.RealTimeMode)
                {
                    dataGridViewAutoUp.Columns[8].Visible = false;
                }
                else
                {
                    //Thread.Sleep(5000);
                    printThread = new Thread(new ThreadStart(AutoPrint));
                    printThread.Start();
                    //log.Info("printThread start");
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "printThread start", DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }


        }

        public void StopThread()
        {
            try
            {
                if (settingModel.RealTimeMode)
                {
                    importThread.Abort();
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "importThread stop", DateTime.Now);
                }
                else
                {
                    importThread.Abort();
                    printThread.Abort();
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "importThread stop", DateTime.Now);
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "printThread stop", DateTime.Now);
                    //log.Info("importThread stop");
                    //log.Info("printThread stop");
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void SetScanModeComboBox()
        {
            List<ScanModeModel> scanList = hhtBll.GetScanMode();
            comboBoxSKU.DataSource = scanList;
            comboBoxSKU.DisplayMember = "ScanModeName";
            comboBoxSKU.ValueMember = "ScanModeID";
        }

        private void SyncHhtForm_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
            tabControl1.TabPages.Remove(tabPage2);

            //LayoutWmenu layoutForm = new LayoutWmenu();
            //layoutForm.syncForm = this;
            queryInsert = new WqlEventQuery();
            queryInsert.WithinInterval = new TimeSpan(0, 0, 1);
            queryInsert.EventClassName = "__InstanceCreationEvent";
            queryInsert.Condition = @"TargetInstance ISA 'Win32_USBControllerdevice'";

            queryRemove = new WqlEventQuery();
            queryRemove.WithinInterval = new TimeSpan(0, 0, 1);
            queryRemove.EventClassName = "__InstanceDeletionEvent";
            queryRemove.Condition = @"TargetInstance ISA 'Win32_USBControllerdevice'";

            ManagementScope scope = new ManagementScope("root\\CIMV2");
            scope.Options.EnablePrivileges = true;

            //watcherInsert = new ManagementEventWatcher(scope, queryInsert);
            //watcherInsert.EventArrived += new EventArrivedEventHandler(USBInsertionEvent);

            //watcherRemove = new ManagementEventWatcher(scope, queryRemove);
            //watcherRemove.EventArrived += new EventArrivedEventHandler(USBRemoveEvent);

            SetScanModeComboBox();

            originalbtnWifiDEnable = btnWifiDownload.Enabled;
            originalbtnWifiUEnable = btnWifiUpload.Enabled;
            connectedCable = CheckPermission();
            //watcherInsert.Start();
            //watcherRemove.Start();
        }

        private void radioLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (radioNoLocation.Checked)
            {
                textBoxLoFrom.Text = "";
                textBoxLoTo.Text = "";
                textBoxLoFrom.Enabled = false;
                textBoxLoTo.Enabled = false;
            }
            if (radioLocation.Checked)
            {
                textBoxLoFrom.Enabled = true;
                textBoxLoTo.Enabled = true;
            }
        }

        private void radioBtnSKU_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnSKU.Checked)
            {
                comboBoxSKU.Enabled = true;
            }
            if (radioBtnNoSKU.Checked)
            {
                comboBoxSKU.Enabled = false;
            }
        }

        private void USBRemoveEvent(object sender, EventArrivedEventArgs e)
        {
            try
            {
                connectedCable = false;
                if (this.comnameTxtBox1.InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        comnameTxtBox1.Text = "";
                        hhtnameTxtBox1.Text = "";
                        comnameTxtBox2.Text = "";
                        hhtnameTxtBox2.Text = "";
                        textBoxIP1.ReadOnly = false;
                        textBoxIP2.ReadOnly = false;
                        btnWifiDownload.Enabled = originalbtnWifiDEnable;
                        btnWifiUpload.Enabled = originalbtnWifiUEnable;

                        this.dataGridView1.Rows.Clear();
                        //radioLocation.Checked = true;
                        //radioBtnSKU.Checked = true;
                        textBoxLoFrom.Text = "";
                        textBoxLoTo.Text = "";
                        this.dataGridViewLo.Rows.Clear();
                        this.dataGridView3.Rows.Clear();
                        //comboBoxSKU.SelectedIndex = 0;

                    });
                }
                else
                {
                    comnameTxtBox1.Text = "";
                    hhtnameTxtBox1.Text = "";
                    comnameTxtBox2.Text = "";
                    hhtnameTxtBox2.Text = "";
                    textBoxIP1.ReadOnly = false;
                    textBoxIP2.ReadOnly = false;
                    btnWifiDownload.Enabled = originalbtnWifiDEnable;
                    btnWifiUpload.Enabled = originalbtnWifiUEnable;

                    this.dataGridView1.Rows.Clear();
                    //radioLocation.Checked = true;
                    //radioBtnSKU.Checked = true;
                    textBoxLoFrom.Text = "";
                    textBoxLoTo.Text = "";
                    this.dataGridViewLo.Rows.Clear();
                    this.dataGridView3.Rows.Clear();
                    //comboBoxSKU.SelectedIndex = 0;
                }
                //BeginInvoke((MethodInvoker)delegate
                //{
                //    comnameTxtBox1.Text = "";
                //    hhtnameTxtBox1.Text = "";
                //    comnameTxtBox2.Text = "";
                //    hhtnameTxtBox2.Text = "";
                //    textBoxIP1.ReadOnly = false;
                //    textBoxIP2.ReadOnly = false;
                //    btnWifiDownload.Enabled = originalbtnWifiDEnable;
                //    btnWifiUpload.Enabled = originalbtnWifiUEnable;

                //    this.dataGridView1.Rows.Clear();
                //    radioLocation.Checked = true;
                //    radioBtnSKU.Checked = true;
                //    textBoxLoFrom.Text = "";
                //    textBoxLoTo.Text = "";
                //    this.dataGridViewLo.Rows.Clear();
                //    this.dataGridView3.Rows.Clear();
                //    comboBoxSKU.SelectedIndex = 0;

                //});
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void USBInsertionEvent(object sender, EventArrivedEventArgs e)
        {
            try
            {
                if (this.comnameTxtBox1.InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        //Loading_Screen.ShowSplashScreen();
                        comnameTxtBox1.Text = "";
                        hhtnameTxtBox1.Text = "";
                        comnameTxtBox2.Text = "";
                        hhtnameTxtBox2.Text = "";
                        textBoxIP1.Text = "";
                        textBoxIP2.Text = "";
                        //radioLocation.Checked = true;
                        //radioBtnSKU.Checked = true;
                        textBoxLoFrom.Text = "";
                        textBoxLoTo.Text = "";
                        dataGridView1.Rows.Clear();
                        dataGridViewLo.Rows.Clear();
                        dataGridView3.Rows.Clear();
                        //comboBoxSKU.SelectedIndex = 0;
                        tabControl1.Enabled = false;

                    });
                }
                else
                {
                    //Loading_Screen.ShowSplashScreen();
                    comnameTxtBox1.Text = "";
                    hhtnameTxtBox1.Text = "";
                    comnameTxtBox2.Text = "";
                    hhtnameTxtBox2.Text = "";
                    textBoxIP1.Text = "";
                    textBoxIP2.Text = "";
                    //radioLocation.Checked = true;
                    //radioBtnSKU.Checked = true;
                    textBoxLoFrom.Text = "";
                    textBoxLoTo.Text = "";
                    dataGridView1.Rows.Clear();
                    dataGridViewLo.Rows.Clear();
                    dataGridView3.Rows.Clear();
                    //comboBoxSKU.SelectedIndex = 0;
                    tabControl1.Enabled = false;
                }

                Thread.Sleep(15000);
                connectedCable = CheckPermission();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private bool CheckPermission()
        {
            if (hhtBll.D_IsDevicePlugIn())
            {
                hhtBll.D_ConnectDevice();
                string deviceName = hhtBll.D_GetDeviceName();
                string myComputerName = hhtBll.GetComputerName().ToUpper();
                //get database
                bool checkValidateDeviceMode = true;
                bool transferComplete = hhtBll.D_HHTTransferFileToPC(checkValidateDeviceMode, HHTDBPath);
                if (transferComplete)
                {
                    ////check permission
                    List<string> coumputerList = hhtBll.GetComputerList();
                    bool validComputer = coumputerList.Any(s => myComputerName.Contains(s));

                    if (validComputer)
                    {
                        if (this.comnameTxtBox1.InvokeRequired)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                comnameTxtBox1.Text = myComputerName;
                                hhtnameTxtBox1.Text = deviceName;
                                comnameTxtBox2.Text = myComputerName;
                                hhtnameTxtBox2.Text = deviceName;
                                textBoxIP1.ReadOnly = true;
                                textBoxIP2.ReadOnly = true;
                                btnWifiDownload.Enabled = false;
                                btnWifiUpload.Enabled = false;
                            });
                        }
                        else
                        {
                            comnameTxtBox1.Text = myComputerName;
                            hhtnameTxtBox1.Text = deviceName;
                            comnameTxtBox2.Text = myComputerName;
                            hhtnameTxtBox2.Text = deviceName;
                            textBoxIP1.ReadOnly = true;
                            textBoxIP2.ReadOnly = true;
                            btnWifiDownload.Enabled = false;
                            btnWifiUpload.Enabled = false;
                        }

                        checkValidateDeviceMode = false;
                        bool transferDBSuccess = hhtBll.D_HHTTransferFileToPC(checkValidateDeviceMode, HHTDBPath);
                        if (transferDBSuccess)
                        {
                            string pathFolder = ConfigurationManager.AppSettings["SourcePath"];
                            bool transferfileSuccess = hhtBll.D_HHTTransferFileUploadToPC(pathFolder, HHTTempPath);
                            if (transferfileSuccess)
                            {
                                DateTime localDate = DateTime.Now;
                                var culture = new CultureInfo("en-US");
                                string log = localDate.ToString(culture) + " Transfering uploaded file from hand-held to PC success.";
                                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, log, DateTime.Now);
                            }
                            else
                            {
                                DateTime localDate = DateTime.Now;
                                var culture = new CultureInfo("en-US");
                                string log = localDate.ToString(culture) + " Do not Transfer uploaded file from hand-held to PC";
                                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, log, DateTime.Now);
                            }

                            if (this.tabControl1.InvokeRequired)
                            {
                                Invoke((MethodInvoker)delegate
                                {
                                    Loading_Screen.CloseForm();
                                    tabControl1.Enabled = true;

                                });
                            }
                            else
                            {
                                Loading_Screen.CloseForm();
                                tabControl1.Enabled = true;
                            }
                        }
                        else
                        {
                            if (this.tabControl1.InvokeRequired)
                            {
                                Invoke((MethodInvoker)delegate
                                {
                                    Loading_Screen.CloseForm();
                                    tabControl1.Enabled = true;

                                });
                            }
                            else
                            {
                                Loading_Screen.CloseForm();
                                tabControl1.Enabled = true;
                            }
                            Console.WriteLine("cannot transfer database STOCKTAKING_HHT.sdf from hht device");
                            MessageBox.Show(MessageConstants.CannotconnecttoHandhelddatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        hhtBll.D_DisconnectDevice();
                        return true;
                    }
                    else
                    {
                        hhtBll.D_DisconnectDevice();
                        if (this.tabControl1.InvokeRequired)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                Loading_Screen.CloseForm();
                                tabControl1.Enabled = true;

                            });
                        }
                        else
                        {
                            Loading_Screen.CloseForm();
                            tabControl1.Enabled = true;
                        }
                        MessageBox.Show(MessageConstants.Invaliddevice, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    if (this.tabControl1.InvokeRequired)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            Loading_Screen.CloseForm();
                            tabControl1.Enabled = true;

                        });
                    }
                    else
                    {
                        Loading_Screen.CloseForm();
                        tabControl1.Enabled = true;
                    }
                    Console.WriteLine("cannot transfer database COMPUTER_NAME.sdf from hht device");
                    MessageBox.Show(MessageConstants.CannotconnecttoHandhelddatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    hhtBll.D_DisconnectDevice();
                    return false;
                }
            }
            else
            {
                if (this.tabControl1.InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        Loading_Screen.CloseForm();
                        tabControl1.Enabled = true;
                    });
                }
                else
                {
                    Loading_Screen.CloseForm();
                    tabControl1.Enabled = true;
                }
                MessageBox.Show(MessageConstants.Nodeviceconnected, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comnameTxtBox1.Text = "";
                hhtnameTxtBox1.Text = "";
                if (this.textBoxIP1.InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        textBoxIP1.ReadOnly = false;
                        textBoxIP2.ReadOnly = false;
                        btnWifiDownload.Enabled = originalbtnWifiDEnable;
                        btnWifiUpload.Enabled = originalbtnWifiUEnable;

                    });
                }
                else
                {
                    textBoxIP1.ReadOnly = false;
                    textBoxIP2.ReadOnly = false;
                    btnWifiDownload.Enabled = originalbtnWifiDEnable;
                    btnWifiUpload.Enabled = originalbtnWifiUEnable;
                }

                hhtBll.D_DisconnectDevice();
                return false;
            }
        }

        private bool CheckCableConnection()
        {
            hhtBll = new HHTSyncBll();
            Loading_Screen.ShowSplashScreen();
            if (hhtBll.D_IsDevicePlugIn())
            {
                hhtBll.D_ConnectDevice();
                string deviceName = hhtBll.D_GetDeviceName();
                string myComputerName = hhtBll.GetComputerName().ToUpper();
                //get database
                bool checkValidateDeviceMode = true;
                bool transferComplete = hhtBll.D_HHTTransferFileToPC(checkValidateDeviceMode, HHTDBPath);
                if (transferComplete)
                {
                    ////check permission
                    List<string> coumputerList = hhtBll.GetComputerList();
                    bool validComputer = coumputerList.Any(s => myComputerName.Contains(s));

                    if (validComputer)
                    {
                        if (this.comnameTxtBox1.InvokeRequired)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                comnameTxtBox1.Text = myComputerName;
                                hhtnameTxtBox1.Text = deviceName;
                                comnameTxtBox2.Text = myComputerName;
                                hhtnameTxtBox2.Text = deviceName;
                                textBoxIP1.ReadOnly = true;
                                textBoxIP2.ReadOnly = true;
                                btnWifiDownload.Enabled = false;
                                btnWifiUpload.Enabled = false;
                            });
                        }
                        else
                        {
                            comnameTxtBox1.Text = myComputerName;
                            hhtnameTxtBox1.Text = deviceName;
                            comnameTxtBox2.Text = myComputerName;
                            hhtnameTxtBox2.Text = deviceName;
                            textBoxIP1.ReadOnly = true;
                            textBoxIP2.ReadOnly = true;
                            btnWifiDownload.Enabled = false;
                            btnWifiUpload.Enabled = false;
                        }

                        checkValidateDeviceMode = false;
                        bool transferDBSuccess = hhtBll.D_HHTTransferFileToPC(checkValidateDeviceMode, HHTDBPath);
                        if (transferDBSuccess)
                        {
                            string pathFolder = ConfigurationManager.AppSettings["SourcePath"];
                            bool transferfileSuccess = hhtBll.D_HHTTransferFileUploadToPC(pathFolder, HHTTempPath);
                            if (transferfileSuccess)
                            {
                                DateTime localDate = DateTime.Now;
                                var culture = new CultureInfo("en-US");
                                string log = localDate.ToString(culture) + " Transfering uploaded file from hand-held to PC success.";
                                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, log, DateTime.Now);
                            }
                            else
                            {
                                DateTime localDate = DateTime.Now;
                                var culture = new CultureInfo("en-US");
                                string log = localDate.ToString(culture) + " Do not Transfer uploaded file from hand-held to PC";
                                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, log, DateTime.Now);
                            }

                            if (this.tabControl1.InvokeRequired)
                            {
                                Invoke((MethodInvoker)delegate
                                {
                                    tabControl1.Enabled = true;
                                });
                            }
                            else
                            {
                                tabControl1.Enabled = true;
                            }
                        }
                        else
                        {
                            if (this.tabControl1.InvokeRequired)
                            {
                                Invoke((MethodInvoker)delegate
                                {
                                    tabControl1.Enabled = true;
                                });
                            }
                            else
                            {
                                tabControl1.Enabled = true;
                            }
                        }

                        hhtBll.D_DisconnectDevice();
                        Loading_Screen.CloseForm();
                        return true;
                    }
                    else
                    {
                        hhtBll.D_DisconnectDevice();
                        if (this.tabControl1.InvokeRequired)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                tabControl1.Enabled = true;
                            });
                        }
                        else
                        {
                            tabControl1.Enabled = true;
                        }
                        Loading_Screen.CloseForm();
                        return false;
                    }
                }
                else
                {
                    if (this.tabControl1.InvokeRequired)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            tabControl1.Enabled = true;
                        });
                    }
                    else
                    {
                        tabControl1.Enabled = true;
                    }

                    hhtBll.D_DisconnectDevice();
                    Loading_Screen.CloseForm();
                    return false;
                }
            }
            else
            {
                if (this.tabControl1.InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        tabControl1.Enabled = true;
                    });
                }
                else
                {
                    tabControl1.Enabled = true;
                }

                if (this.textBoxIP1.InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        textBoxIP1.ReadOnly = false;
                        textBoxIP2.ReadOnly = false;
                        btnWifiDownload.Enabled = originalbtnWifiDEnable;
                        btnWifiUpload.Enabled = originalbtnWifiUEnable;
                    });
                }
                else
                {
                    textBoxIP1.ReadOnly = false;
                    textBoxIP2.ReadOnly = false;
                    btnWifiDownload.Enabled = originalbtnWifiDEnable;
                    btnWifiUpload.Enabled = originalbtnWifiUEnable;
                }

                hhtBll.D_DisconnectDevice();
                Loading_Screen.CloseForm();
                return false;
            }
        }

        private bool GetAuditDataFromHHT()
        {
            hhtBll = new HHTSyncBll();
            Loading_Screen.ShowSplashScreen();
            if (hhtBll.D_IsDevicePlugIn())
            {
                hhtBll.D_ConnectDevice();                                   
                string pathFolder = ConfigurationManager.AppSettings["SourcePath"];
                bool transferfileSuccess = hhtBll.D_HHTTransferFileUploadToPC(pathFolder, HHTTempPath);
                if (transferfileSuccess)
                {
                    DateTime localDate = DateTime.Now;
                    var culture = new CultureInfo("en-US");
                    string log = localDate.ToString(culture) + " Transfering uploaded file from hand-held to PC success.";
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, log, DateTime.Now);
                }
                else
                {
                    DateTime localDate = DateTime.Now;
                    var culture = new CultureInfo("en-US");
                    string log = localDate.ToString(culture) + " Do not Transfer uploaded file from hand-held to PC";
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, log, DateTime.Now);
                }

                hhtBll.D_DisconnectDevice();
                Loading_Screen.CloseForm();
                return true;
            }         
            else
            {
                Loading_Screen.CloseForm();
                MessageBox.Show(MessageConstants.Nodeviceconnected, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comnameTxtBox1.Text = "";
                hhtnameTxtBox1.Text = "";
                hhtBll.D_DisconnectDevice();
                return false;
            }
        }

        //public bool CheckPermissionFTP(FTP ftpClient)
        private bool CheckPermissionFTP(string hostIP)
        {
            bool checkPermissionMode = true;
            //bool success = ftpClient.TransferHHTToPC(checkPermissionMode);
            string myComputerName = hhtBll.GetComputerName().ToUpper();
            bool success = hhtBll.FTPTransferHHTToPC(hostIP, checkPermissionMode, HHTDBPath);
            if (success)
            {
                //check permission
                List<string> coumputerList = hhtBll.GetComputerList();
                bool validComputer = coumputerList.Any(s => myComputerName.Contains(s));
                if (validComputer)
                {
                    return true;
                }
                else
                {
                    Loading_Screen.CloseForm();
                    tabControl1.Enabled = true;

                    MessageBox.Show(MessageConstants.Invaliddevice, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                Loading_Screen.CloseForm();
                tabControl1.Enabled = true;

                //MessageBox.Show("Error : transfer file ftp");
                MessageBox.Show(MessageConstants.CannotconnecttoHandhelddatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        private bool CheckWifiConnection(string hostIP)
        {
            bool checkPermissionMode = true;
            //bool success = ftpClient.TransferHHTToPC(checkPermissionMode);
            string myComputerName = hhtBll.GetComputerName().ToUpper();
            bool success = hhtBll.FTPTransferHHTToPC(hostIP, checkPermissionMode, HHTDBPath);
            if (success)
            {
                //check permission
                List<string> coumputerList = hhtBll.GetComputerList();
                bool validComputer = coumputerList.Any(s => myComputerName.Contains(s));
                if (validComputer)
                {
                    return true;
                }
                else
                {
                    tabControl1.Enabled = true;
                    return false;
                }
            }
            else
            {
                tabControl1.Enabled = true;              
                return false;
            }
        }


        private void SetLocationGridview(List<AuditStocktakingModel> auditList)
        {
            if (this.dataGridViewLo.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    dataGridViewLo.Columns[0].Visible = true;
                    dataGridViewLo.Columns[1].Visible = true;
                    dataGridViewLo.Columns[2].Visible = true;
                    dataGridViewLo.Columns[4].Visible = true;
                    dataGridViewLo.Columns[5].Visible = true;
                    dataGridViewLo.Rows.Clear();
                });
            }
            else
            {
                dataGridViewLo.Columns[0].Visible = true;
                dataGridViewLo.Columns[1].Visible = true;
                dataGridViewLo.Columns[2].Visible = true;
                dataGridViewLo.Columns[4].Visible = true;
                dataGridViewLo.Columns[5].Visible = true;
                dataGridViewLo.Rows.Clear();
            }

            if (auditList.Count > 0)
            {
                List<AuditStocktakingModel> auditListDisLocation = auditList.GroupBy(x => x.LocationCode).Select(g => g.First()).ToList();

                int count = 1;
                int num = 1;
                var newListModel = new List<LocationHHTModel>();
                var L1 = string.Empty;
                var L2 = string.Empty;
                var L3 = string.Empty;
                foreach (var i in auditListDisLocation)
                {
                    if (count == auditListDisLocation.Count())
                    {
                        if (num == 1)
                        {
                            L1 = i.LocationCode;
                            var newModel = new LocationHHTModel();
                            newModel.LocationCode1 = L1;
                            newModel.LocationCode2 = string.Empty;
                            newModel.LocationCode3 = string.Empty;
                            newListModel.Add(newModel);
                            L1 = string.Empty;
                            L2 = string.Empty;
                            L3 = string.Empty;
                            num = 0;
                        }
                        else if (num == 2)
                        {
                            L2 = i.LocationCode;
                            var newModel = new LocationHHTModel();
                            newModel.LocationCode1 = L1;
                            newModel.LocationCode2 = L2;
                            newModel.LocationCode3 = string.Empty;
                            newListModel.Add(newModel);
                            L1 = string.Empty;
                            L2 = string.Empty;
                            L3 = string.Empty;
                            num = 0;
                        }
                        else if (num == 3)
                        {
                            L3 = i.LocationCode;
                            var newModel = new LocationHHTModel();
                            newModel.LocationCode1 = L1;
                            newModel.LocationCode2 = L2;
                            newModel.LocationCode3 = L3;
                            newListModel.Add(newModel);
                            L1 = string.Empty;
                            L2 = string.Empty;
                            L3 = string.Empty;
                            num = 0;
                        }
                    }
                    else
                    {
                        if (num == 1)
                        {
                            L1 = i.LocationCode;
                        }
                        else if (num == 2)
                        {
                            L2 = i.LocationCode;
                        }
                        else if (num == 3)
                        {
                            L3 = i.LocationCode;

                            var newModel = new LocationHHTModel();
                            newModel.LocationCode1 = L1;
                            newModel.LocationCode2 = L2;
                            newModel.LocationCode3 = L3;
                            newListModel.Add(newModel);
                            L1 = string.Empty;
                            L2 = string.Empty;
                            L3 = string.Empty;
                            num = 0;
                        }
                    }
                    num++;
                    count++;
                }

                var check = newListModel;
                int rowIndex = 1;
                foreach (var i in newListModel)
                {
                    if (this.dataGridViewLo.InvokeRequired)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            dataGridViewLo.Rows.Add(false, i.LocationCode1, false, i.LocationCode2, false, i.LocationCode3);
                            if (rowIndex == newListModel.Count)
                            {
                                if (dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[3].Value == string.Empty)
                                {
                                    //dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[2].Value = false;
                                    dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[2] = new DataGridViewTextBoxCell();
                                    dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[2].Value = "";
                                }
                                if (dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[5].Value == string.Empty)
                                {
                                    //dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[4].Value = false;
                                    dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[4] = new DataGridViewTextBoxCell();
                                    dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[4].Value = "";
                                }
                            }
                            rowIndex++;
                        });
                    }
                    else
                    {
                        dataGridViewLo.Rows.Add(false, i.LocationCode1, false, i.LocationCode2, false, i.LocationCode3);
                        if (rowIndex == newListModel.Count)
                        {
                            if (dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[3].Value == string.Empty)
                            {
                                //dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[2].Value = false;
                                dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[2] = new DataGridViewTextBoxCell();
                                dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[2].Value = "";
                            }
                            if (dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[5].Value == string.Empty)
                            {
                                //dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[4].Value = false;
                                dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[4] = new DataGridViewTextBoxCell();
                                dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[4].Value = "";
                            }
                        }
                        rowIndex++;
                    }
                }
                rowIndex = 1;
            }
            else
            {
                if (this.dataGridViewLo.InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        dataGridViewLo.Rows.Add(false, "", false, "No data found.", false, "");
                        dataGridViewLo.Columns[0].Visible = false;
                        dataGridViewLo.Columns[1].Visible = false;
                        dataGridViewLo.Columns[2].Visible = false;
                        dataGridViewLo.Columns[4].Visible = false;
                        dataGridViewLo.Columns[5].Visible = false;
                        dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[3].Style = new DataGridViewCellStyle { ForeColor = Color.Red };

                    });
                }
                else
                {
                    dataGridViewLo.Rows.Add(false, "", false, "No data found.", false, "");
                    dataGridViewLo.Columns[0].Visible = false;
                    dataGridViewLo.Columns[1].Visible = false;
                    dataGridViewLo.Columns[2].Visible = false;
                    dataGridViewLo.Columns[4].Visible = false;
                    dataGridViewLo.Columns[5].Visible = false;
                    dataGridViewLo.Rows[dataGridViewLo.Rows.Count - 1].Cells[3].Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                }
            }
        }

        private void dataGridViewLo_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0 || e.ColumnIndex == 2 || e.ColumnIndex == 4)
                {
                    object value = dataGridViewLo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    if (value.ToString() == "True")
                    {
                        dataGridViewLo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                    }
                    if (value.ToString() == "False")
                    {
                        dataGridViewLo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                    }

                }
            }
        }

        private void SyncHhtForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //watcherInsert.Dispose();
            //watcherRemove.Dispose();
            hhtBll.D_DisconnectDevice();
            hhtBll.DeleteDBFile();
        }
        #endregion

        private void btnWifiDownload_Click(object sender, EventArgs e)
        {
            comnameTxtBox1.Text = "";
            hhtnameTxtBox1.Text = "";
            //radioLocation.Checked = true;
            //radioBtnSKU.Checked = true;
            textBoxLoFrom.Text = "";
            textBoxLoTo.Text = "";
            dataGridView1.Rows.Clear();

            ipAddress = textBoxIP1.Text.Trim();
            if (ipAddress == string.Empty)
            {
                MessageBox.Show(MessageConstants.PleaseenterIPaddress, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connectedWiFiDownload = false;
            }
            else
            {
                Loading_Screen.ShowSplashScreen();
                tabControl1.Enabled = false;

                connectedWiFiDownload = CheckPermissionFTP(ipAddress);
                if (connectedWiFiDownload)
                {
                    bool checkPermissionMode = false;
                    //bool transferSuccess = ftpClient.TransferHHTToPC(checkPermissionMode);
                    bool transferSuccess = hhtBll.FTPTransferHHTToPC(ipAddress, checkPermissionMode, HHTDBPath);
                    if (transferSuccess)
                    {
                        //auditList = hhtBll.GetAuditList();
                        //SetLocationGridview(auditList);
                        string deviceName = hhtBll.GetDeviceNameFTP();
                        string myComputerName = hhtBll.GetComputerName();
                        comnameTxtBox1.Text = myComputerName;
                        hhtnameTxtBox1.Text = deviceName;

                        Loading_Screen.CloseForm();
                        tabControl1.Enabled = true;

                    }
                    else
                    {

                        Loading_Screen.CloseForm();
                        tabControl1.Enabled = true;

                        MessageBox.Show(MessageConstants.CannotconnecttoHandhelddatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("ftp cannot transfer database STOCKTAKING_HHT.sdf from HHT.");
                    }
                }
                else
                {
                    comnameTxtBox1.Text = "";
                    hhtnameTxtBox1.Text = "";
                }
            }
        }

        private void btnWifiUpload_Click(object sender, EventArgs e)
        {
            comnameTxtBox2.Text = "";
            hhtnameTxtBox2.Text = "";
            dataGridViewLo.Rows.Clear();
            dataGridView3.Rows.Clear();

            ipAddressUpload = textBoxIP2.Text.Trim();
            //hhtBll.FTPTransferHHTToPC(ipAddressUpload, false);

            if (ipAddressUpload == string.Empty)
            {
                MessageBox.Show(MessageConstants.PleaseenterIPaddress, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connectedWiFiUpload = false;
            }
            else
            {
                //string hostIP = "ftp://" + ipAddress + "/";
                //ftpClient = new FTP(hostIP, "anonymous", "s");

                Loading_Screen.ShowSplashScreen();
                tabControl1.Enabled = false;

                connectedWiFiUpload = CheckPermissionFTP(ipAddressUpload);
                if (connectedWiFiUpload)
                {
                    bool checkPermissionMode = false;
                    //bool transferSuccess = ftpClient.TransferHHTToPC(checkPermissionMode);
                    bool transferSuccess = hhtBll.FTPTransferHHTToPC(ipAddressUpload, checkPermissionMode, HHTDBPath);
                    if (transferSuccess)
                    {
                        string deviceName = hhtBll.GetDeviceNameFTP();
                        string myComputerName = hhtBll.GetComputerName();
                        comnameTxtBox2.Text = myComputerName;
                        hhtnameTxtBox2.Text = deviceName;

                        auditList = hhtBll.GetAuditList();
                        SetLocationGridview(auditList);

                        Loading_Screen.CloseForm();
                        tabControl1.Enabled = true;

                        //MessageBox.Show("Success : transfer file ftp");
                    }
                    else
                    {

                        Loading_Screen.CloseForm();
                        tabControl1.Enabled = true;

                        MessageBox.Show(MessageConstants.CannotconnecttoHandhelddatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("ftp cannot transfer database STOCKTAKING_HHT.sdf from HHT.");
                    }
                }
                else
                {
                    comnameTxtBox2.Text = "";
                    hhtnameTxtBox2.Text = "";
                }
            }
        }

        #region Upload function
        private void btnUpload_Click(object sender, EventArgs e)
        {
            tabControl1.Enabled = false;
            Loading_Screen.ShowSplashScreen();
            this.dataGridView3.Rows.Clear();

            if (connectedCable || connectedWiFiUpload)
            {
                DoUpload(auditList);
            }
            else
            {
                tabControl1.Enabled = true;
                Loading_Screen.CloseForm();
                comnameTxtBox1.Text = "";
                hhtnameTxtBox1.Text = "";
                MessageBox.Show(MessageConstants.Nodeviceconnected, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DoUpload(List<AuditStocktakingModel> auditList)
        {
            List<string> selectedLoList = new List<string>();
            string deviceName = "";
            bool insertSuccess = false;
            bool uploadReplace = false;
            bool uploadDiff = false;
            bool uploadNormal = false;
            List<string> duplicateList = new List<string>();

            if (connectedCable)
            {
                hhtBll.D_ConnectDevice();
                deviceName = hhtBll.D_GetDeviceName();
                hhtBll.D_DisconnectDevice();
            }
            if (connectedWiFiUpload)
            {
                deviceName = hhtBll.GetDeviceNameFTP();
            }

            foreach (DataGridViewRow row in dataGridViewLo.Rows)
            {

                object checkBox1Checked = row.Cells[0].Value;
                if (checkBox1Checked.ToString() == "True")
                {
                    selectedLoList.Add((row.Cells[1].Value).ToString());
                }

                object checkBox2Checked = row.Cells[2].Value;
                if (checkBox2Checked.ToString() == "True")
                {
                    selectedLoList.Add((row.Cells[3].Value).ToString());
                }

                object checkBox3Checked = row.Cells[4].Value;
                if (checkBox3Checked.ToString() == "True")
                {
                    selectedLoList.Add((row.Cells[5].Value).ToString());
                }

            }

            if (selectedLoList.Count > 0)
            {
                // find duplicate stocktakingID
                foreach (string selectedLocation in selectedLoList)
                {
                    List<AuditStocktakingModel> auditSelectedList = auditList.Where(a => a.LocationCode.Equals(selectedLocation)).ToList();
                    foreach (AuditStocktakingModel audit in auditSelectedList)
                    {
                        bool stocktakingIDExist = hhtBll.IsStocktakingIDExist(audit.StockTakingID);
                        if (stocktakingIDExist)
                        {
                            duplicateList.Add(audit.StockTakingID);
                        }
                    }
                }


                List<AuditStocktakingModel>[] auditListByLocationArr = new List<AuditStocktakingModel>[selectedLoList.Count];
                List<AuditStocktakingModel>[] diffListArr = new List<AuditStocktakingModel>[selectedLoList.Count];

                // if has duplicate record
                if (duplicateList.Count > 0)
                {
                    Loading_Screen.CloseForm();
                    CustomDialog dialog = new CustomDialog();
                    DialogResult result = dialog.ShowDialog();
                    switch (result)
                    {
                        //upload and replace
                        case DialogResult.Yes:
                            {
                                Loading_Screen.ShowSplashScreen();
                                uploadReplace = true;
                                int count = 0;
                                foreach (string selectedLocation in selectedLoList)
                                {

                                    auditListByLocationArr[count] = auditList.Where(a => a.LocationCode.Equals(selectedLocation)).ToList();

                                    this.dataGridView3.Rows.Add(deviceName, selectedLocation, auditListByLocationArr[count].Count, 0);

                                    count++;
                                }

                                insertSuccess = InsertToStocktaking(auditListByLocationArr, duplicateList, deviceName);
                                break;
                            }
                        //upload only difference
                        case DialogResult.No:
                            {
                                Loading_Screen.ShowSplashScreen();
                                uploadDiff = true;

                                List<AuditStocktakingModel> temp = new List<AuditStocktakingModel>();
                                bool hasDiffData = false;
                                int run = 0;
                                foreach (string selectedLocation in selectedLoList)
                                {
                                    temp = auditList.Where(a => a.LocationCode.Equals(selectedLocation)).ToList();
                                    diffListArr[run] = temp.Where(p => !duplicateList.Any(d => d == p.StockTakingID)).ToList();
                                    if (diffListArr[run].Count > 0)
                                    {
                                        hasDiffData = hasDiffData || true;
                                        this.dataGridView3.Rows.Add(deviceName, selectedLocation, diffListArr[run].Count, 0);
                                    }
                                    run++;
                                }

                                if (hasDiffData)
                                {
                                    insertSuccess = InsertToStocktaking(diffListArr, new List<string>(), deviceName);
                                }
                                else
                                {
                                    tabControl1.Enabled = true;
                                    Loading_Screen.CloseForm();
                                    MessageBox.Show(MessageConstants.NoDiffTransactionData, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                break;
                            }
                        case DialogResult.Cancel:
                            {
                                tabControl1.Enabled = true;
                                return;
                            }
                    }
                }
                else
                {
                    uploadNormal = true;
                    int count = 0;
                    foreach (string selectedLocation in selectedLoList)
                    {

                        auditListByLocationArr[count] = auditList.Where(a => a.LocationCode.Equals(selectedLocation)).ToList();

                        this.dataGridView3.Rows.Add(deviceName, selectedLocation, auditListByLocationArr[count].Count, 0);

                        count++;
                    }
                    insertSuccess = InsertToStocktaking(auditListByLocationArr, new List<string>(), deviceName);
                }


                // delete data in hht database and transfer back to hht
                if (insertSuccess)
                {
                    Loading_Screen.CloseForm();
                    tabControl1.Enabled = true;
                    DialogResult result3 = MessageBox.Show(MessageConstants.DoyouwanttodeleteuploadeddataonHHT, MessageConstants.UploadComplete, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    switch (result3)
                    {
                        case DialogResult.Yes:
                            {

                                Loading_Screen.ShowSplashScreen();
                                tabControl1.Enabled = false;
                                if (uploadNormal)
                                {
                                    DeleteUpload(auditListByLocationArr, selectedLoList);
                                }
                                if (uploadReplace)
                                {
                                    DeleteUpload(auditListByLocationArr, selectedLoList);
                                }
                                if (uploadDiff)
                                {
                                    DeleteUpload(diffListArr, selectedLoList);
                                }

                                break;
                            }
                        case DialogResult.No:
                            {
                                break;
                            }
                    }
                }
                else
                {
                    tabControl1.Enabled = true;
                    Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.UploadNotComplete, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                tabControl1.Enabled = true;
                Loading_Screen.CloseForm();
                MessageBox.Show(MessageConstants.Pleasechooselocationbeforeupload, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void DeleteUpload(List<AuditStocktakingModel>[] auditListByLocationArr, List<string> selectedLoList)
        {
            bool deleteSuccess = ConnectSdfDeleteDataStockTaking(auditListByLocationArr);
            if (deleteSuccess)
            {
                if (connectedCable)
                {
                    hhtBll.D_ConnectDevice();
                    if (hhtBll.D_PCTransferFileToHHT(HHTDBPath))
                    {
                        foreach (List<AuditStocktakingModel> auditListByLocation in auditListByLocationArr)
                        {
                            if (auditListByLocation.Count > 0)
                            {
                                foreach (AuditStocktakingModel item in auditListByLocation)
                                {
                                    auditList.RemoveAll(x => x.StockTakingID == item.StockTakingID);
                                }
                            }
                        }
                        SetLocationGridview(auditList);

                        dataGridView3.Rows.Clear();

                        Loading_Screen.CloseForm();
                        tabControl1.Enabled = true;
                        MessageBox.Show(MessageConstants.Deleteuploadeddatasuccessful, MessageConstants.TitleDeleteuploadeddatasuccessful, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {

                        Loading_Screen.CloseForm();
                        tabControl1.Enabled = true;
                        MessageBox.Show(MessageConstants.Deleteuploadeddatafail, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    hhtBll.D_DisconnectDevice();
                }
                else if (connectedWiFiUpload)
                {
                    if (hhtBll.FTPTransferPCToHHT(ipAddressUpload, HHTDBPath))
                    {
                        foreach (List<AuditStocktakingModel> auditListByLocation in auditListByLocationArr)
                        {
                            if (auditListByLocation.Count > 0)
                            {
                                foreach (AuditStocktakingModel item in auditListByLocation)
                                {
                                    auditList.RemoveAll(x => x.StockTakingID == item.StockTakingID);
                                }
                            }
                        }
                        SetLocationGridview(auditList);

                        dataGridView3.Rows.Clear();

                        Loading_Screen.CloseForm();
                        tabControl1.Enabled = true;

                        MessageBox.Show(MessageConstants.Deleteuploadeddatasuccessful, MessageConstants.TitleDeleteuploadeddatasuccessful, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        Loading_Screen.CloseForm();
                        tabControl1.Enabled = true;
                        MessageBox.Show(MessageConstants.DeleteuploadeddatafailFailtransfertohht, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    Loading_Screen.CloseForm();
                    tabControl1.Enabled = true;
                    MessageBox.Show(MessageConstants.DeleteuploadeddatafailNodeviceconnected, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                Loading_Screen.CloseForm();
                tabControl1.Enabled = true;
                MessageBox.Show(MessageConstants.Deleteuploadeddatafail, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ConnectSdfDeleteDataStockTaking(List<AuditStocktakingModel>[] auditListByLocationArr)
        {
            return hhtBll.ConnectSdfDeleteDataStockTaking(auditListByLocationArr);
        }


        private bool InsertToStocktaking(List<AuditStocktakingModel>[] auditListByLocationArr, List<string> duplicateIDList, string deviceName)
        {                        
            Entities dbContext = new Entities();
            using (SqlConnection conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                conn.Open();
                string column = "CountDate";
                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    SqlCommand commandInsert = PrepareInsertStocktakingCommand(conn, transaction);
                    SqlCommand commandDelete = PrepareDeleteStocktakingCommand(conn, transaction);
                    //CultureInfo defaulCulture = new CultureInfo("en-US");

                    try
                    {
                        DateTime countDate = (DateTime)(from s in dbContext.SystemSettings
                                                        where s.SettingKey.Equals(column)
                                                        select s.ValueDate).FirstOrDefault();

                        int rowCount = 0;
                        int uploadedRec = 0;

                        if (duplicateIDList.Count > 0)
                        {
                            foreach (string stocktakingID in duplicateIDList)
                            {
                                commandDelete.Parameters["@StocktakingID"].Value = stocktakingID;
                                commandDelete.ExecuteNonQuery();
                            }
                        }

                        foreach (List<AuditStocktakingModel> auditListByLocation in auditListByLocationArr)
                        {
                            if (auditListByLocation.Count > 0)
                            {
                                foreach (var auditData in auditListByLocation)
                                {

                                    commandInsert.Parameters["@StocktakingID"].Value = auditData.StockTakingID;
                                    commandInsert.Parameters["@ScanMode"].Value = auditData.ScanMode;
                                    commandInsert.Parameters["@LocationCode"].Value = auditData.LocationCode;
                                    commandInsert.Parameters["@Barcode"].Value = auditData.Barcode;
                                    commandInsert.Parameters["@Quantity"].Value = auditData.Quantity;
                                    commandInsert.Parameters["@NewQuantity"].Value = DBNull.Value;
                                    commandInsert.Parameters["@UnitCode"].Value = auditData.UnitCode;
                                    commandInsert.Parameters["@Flag"].Value = auditData.Flag;
                                    if (auditData.Description == "")
                                    {
                                        commandInsert.Parameters["@Description"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@Description"].Value = auditData.Description;
                                    }
                                    if (auditData.SKUCode == "")
                                    {
                                        commandInsert.Parameters["@SKUCode"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@SKUCode"].Value = auditData.SKUCode;
                                    }
                                    if (auditData.ExBarcode == "")
                                    {
                                        commandInsert.Parameters["@ExBarCode"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@ExBarCode"].Value = auditData.ExBarcode;
                                    }
                                    if (auditData.InBarcode == "")
                                    {
                                        commandInsert.Parameters["@InBarCode"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@InBarCode"].Value = auditData.InBarcode;
                                    }
                                    //if (auditData.BrandCode == "")
                                    //{
                                    //    commandInsert.Parameters["@BrandCode"].Value = DBNull.Value;
                                    //}
                                    //else
                                    //{
                                    //    commandInsert.Parameters["@BrandCode"].Value = auditData.BrandCode;
                                    //}
                                    commandInsert.Parameters["@SKUMode"].Value = auditData.SKUMode;
                                    commandInsert.Parameters["@HHTName"].Value = deviceName;
                                    commandInsert.Parameters["@CountDate"].Value = countDate;
                                    commandInsert.Parameters["@DepartmentCode"].Value = auditData.DepartmentCode;
                                    if (auditData.MKCode == "")
                                    {
                                        commandInsert.Parameters["@MKCode"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@MKCode"].Value = auditData.MKCode;
                                    }
                                    commandInsert.Parameters["@FlagPrint"].Value = false;
                                    commandInsert.Parameters["@CreateDate"].Value = auditData.CreateDate;
                                    commandInsert.Parameters["@CreateBy"].Value = auditData.CreateBy;
                                    commandInsert.Parameters["@UpdateDate"].Value = auditData.CreateDate;
                                    commandInsert.Parameters["@UpdateBy"].Value = auditData.CreateBy;

                                    if (auditData.SerialNumber == "")
                                    {
                                        commandInsert.Parameters["@SerialNumber"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@SerialNumber"].Value = auditData.SerialNumber;
                                    }

                                    if (auditData.ConversionCounter == "")
                                    {
                                        commandInsert.Parameters["@ConversionCounter"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@ConversionCounter"].Value = auditData.ConversionCounter;
                                    }

                                    if (auditData.StorageLocation == "")
                                    {
                                        commandInsert.Parameters["@StorageLocation"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@StorageLocation"].Value = auditData.StorageLocation;
                                    }

                                    if (auditData.StorageLocationDesc == "")
                                    {
                                        commandInsert.Parameters["@StorageLocationDesc"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@StorageLocationDesc"].Value = auditData.StorageLocationDesc;
                                    }

                                    if (auditData.PIDoc == "")
                                    {
                                        commandInsert.Parameters["@PIDoc"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@PIDoc"].Value = auditData.PIDoc;
                                    }

                                    if (auditData.Plant == "")
                                    {
                                        commandInsert.Parameters["@Plant"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@Plant"].Value = auditData.Plant;
                                    }

                                    if (auditData.MCHLevel1 == "")
                                    {
                                        commandInsert.Parameters["@MCHLevel1"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@MCHLevel1"].Value = auditData.MCHLevel1;
                                    }

                                    if (auditData.MCHLevel2 == "")
                                    {
                                        commandInsert.Parameters["@MCHLevel2"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@MCHLevel2"].Value = auditData.MCHLevel2;
                                    }

                                    if (auditData.MCHLevel3 == "")
                                    {
                                        commandInsert.Parameters["@MCHLevel3"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@MCHLevel3"].Value = auditData.MCHLevel3;
                                    }

                                    if (auditData.MaterialGroup == "")
                                    {
                                        commandInsert.Parameters["@MaterialGroup"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        commandInsert.Parameters["@MaterialGroup"].Value = auditData.MaterialGroup;
                                    }

                                    commandInsert.ExecuteNonQuery();

                                    this.dataGridView3.Rows[rowCount].Cells[3].Value = ++uploadedRec;
                                    //this.dataGridView3.Refresh();
                                }
                                rowCount++;
                                uploadedRec = 0;
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                        transaction.Rollback();
                        return false;
                    }
                }

                }
            }
            private SqlCommand PrepareInsertStocktakingCommand(SqlConnection conn, SqlTransaction transaction)
            {
                // create SQL command object
                SqlCommand command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                command.Transaction = transaction;

                // create SQL command text
                StringBuilder sb = new StringBuilder();
                sb.Append("INSERT INTO HHTStocktaking ");
                sb.Append("VALUES (@StocktakingID, @ScanMode, @LocationCode, @Barcode, @Quantity ,@NewQuantity , @UnitCode, @Flag, @Description, @SKUCode, @ExBarCode, @InBarCode, @SKUMode, @HHTName, @CountDate, @DepartmentCode, @MKCode, @FlagPrint, @CreateDate, @CreateBy, @UpdateDate, @UpdateBy,@SerialNumber, @ConversionCounter, @StorageLocation, @StorageLocationDesc, @Plant, @PIDoc, @MCHLevel1, @MCHLevel2, @MCHLevel3, @MaterialGroup);");
                command.CommandText = sb.ToString();

                // define parameter type
                command.Parameters.Add("@StocktakingID", SqlDbType.VarChar, 16);
                command.Parameters.Add("@ScanMode", SqlDbType.Int);
                command.Parameters.Add("@LocationCode", SqlDbType.VarChar, 5);
                command.Parameters.Add("@Barcode", SqlDbType.VarChar, 20);
                command.Parameters.Add("@Quantity", SqlDbType.Decimal, 18);
                command.Parameters["@Quantity"].Precision = 18;
                command.Parameters["@Quantity"].Scale = 3;
                command.Parameters.Add("@NewQuantity", SqlDbType.Decimal, 18);
                command.Parameters["@NewQuantity"].Precision = 18;
                command.Parameters["@NewQuantity"].Scale = 3;
                command.Parameters.Add("@UnitCode", SqlDbType.Int);
                command.Parameters.Add("@Flag", SqlDbType.VarChar, 1);
                command.Parameters.Add("@Description", SqlDbType.VarChar, 50);
                command.Parameters.Add("@SKUCode", SqlDbType.VarChar, 25);
                command.Parameters.Add("@ExBarCode", SqlDbType.VarChar, 25);
                command.Parameters.Add("@InBarCode", SqlDbType.VarChar, 25);
                //command.Parameters.Add("@BrandCode", SqlDbType.VarChar, 5);
                command.Parameters.Add("@SKUMode", SqlDbType.Bit);
                command.Parameters.Add("@HHTName", SqlDbType.VarChar, 20);
                command.Parameters.Add("@CountDate", SqlDbType.Date);
                command.Parameters.Add("@DepartmentCode", SqlDbType.VarChar, 3);
                command.Parameters.Add("@MKCode", SqlDbType.VarChar, 5);
                command.Parameters.Add("@FlagPrint", SqlDbType.Bit);
                command.Parameters.Add("@CreateDate", SqlDbType.DateTime);
                command.Parameters.Add("@CreateBy", SqlDbType.VarChar, 20);
                command.Parameters.Add("@UpdateDate", SqlDbType.DateTime);
                command.Parameters.Add("@UpdateBy", SqlDbType.VarChar, 20);

                command.Parameters.Add("@SerialNumber", SqlDbType.VarChar, 100);
                command.Parameters.Add("@ConversionCounter", SqlDbType.VarChar,10);
                command.Parameters.Add("@StorageLocation", SqlDbType.VarChar, 50);
                command.Parameters.Add("@StorageLocationDesc", SqlDbType.VarChar,50);
                command.Parameters.Add("@Plant", SqlDbType.VarChar, 50);
                command.Parameters.Add("@PIDoc", SqlDbType.VarChar, 50);
                command.Parameters.Add("@MCHLevel1", SqlDbType.VarChar,50);
                command.Parameters.Add("@MCHLevel2", SqlDbType.VarChar, 50);
                command.Parameters.Add("@MCHLevel3", SqlDbType.VarChar, 50);
                command.Parameters.Add("@MaterialGroup", SqlDbType.VarChar, 50);
                command.Prepare();

                return command;
            }

            private SqlCommand PrepareDeleteStocktakingCommand(SqlConnection conn, SqlTransaction transaction)
            {
                // create SQL command object
                SqlCommand command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                command.Transaction = transaction;

                // create SQL command text
                StringBuilder sb = new StringBuilder();
                sb.Append("DELETE FROM HHTStocktaking ");
                sb.Append("WHERE StocktakingID = @StocktakingID");
                command.CommandText = sb.ToString();

                // define parameter type
                command.Parameters.Add("@StocktakingID", SqlDbType.VarChar, 16);
                command.Prepare();

                return command;
            }

        #endregion

        #region Download function
        private void btnDownload_Click(object sender, EventArgs e)
        {
            FillLocationFom();
            FillLocationTo();
            tabControl1.Enabled = false;
            this.dataGridView1.Rows.Clear();

            string locationFrom = textBoxLoFrom.Text.Trim();
            string locationTo = textBoxLoTo.Text.Trim();
            
            List<UnitModel> unitList = new List<UnitModel>();
            List<DownloadLocationModel> downloadLocationList = new List<DownloadLocationModel>();       
            List<MasterSerialNumberModel> masterSerialNumberList = new List<MasterSerialNumberModel>();

            connectedCable = CheckCableConnection();
            connectedWiFiDownload = CheckWifiConnection(ipAddress);

            if (connectedCable || connectedWiFiDownload)
            {               
                string deviceNameForFTP = "";
                if (connectedWiFiDownload)
                {
                    deviceNameForFTP = hhtBll.GetDeviceNameFTP();
                }

                if (radioLocation.Checked && radioBtnSKU.Checked) // load ทั้ง location และ SKU
                {
                    if (locationFrom == string.Empty && locationTo == string.Empty)
                    {
                        tabControl1.Enabled = true;
                        MessageBox.Show(MessageConstants.PleasEnterLocationFormOrLocationTo, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Loading_Screen.ShowSplashScreen();
                        unitList = hhtBll.GetUnit();

                        bool downloadUnitSuccess = DoDownLoadUnit();
                        if (downloadUnitSuccess)
                        {
                            downloadLocationList = hhtBll.GetLocation(locationFrom, locationTo);
                            bool downloadLocationSuccess = DoDownloadLocation(downloadLocationList);

                            if (downloadLocationSuccess)
                            {
                                masterSerialNumberList = hhtBll.GetMasterSerialNumber();
                                bool downloadMasterSerialSuccess = DoDownLoadMasterSerialNumber(masterSerialNumberList);

                                if (downloadMasterSerialSuccess)
                                {
                                    if (connectedCable)
                                    {
                                        hhtBll.D_ConnectDevice();
                                        if (hhtBll.D_PCTransferFileToHHT(HHTDBPath))
                                        {
                                            Loading_Screen.CloseForm();
                                            tabControl1.Enabled = true;
                                            int rowCount = dataGridView1.Rows.Count;
                                            this.dataGridView1.Rows[1].Cells[3].Value = masterSerialNumberList.Count;
                                            this.dataGridView1.Rows[0].Cells[3].Value = downloadLocationList.Count;
                                            MessageBox.Show(MessageConstants.DownloadComplete, MessageConstants.DownloadComplete, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            Loading_Screen.CloseForm();
                                            tabControl1.Enabled = true;
                                            MessageBox.Show(MessageConstants.DownloadNotCompleteFailtotransferfiletoHandheld, MessageConstants.DownloadNotComplete, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        hhtBll.D_DisconnectDevice();
                                    }
                                    else
                                    {
                                        if (hhtBll.FTPTransferPCToHHT(ipAddress, HHTDBPath))
                                        {
                                            Loading_Screen.CloseForm();
                                            tabControl1.Enabled = true;
                                            int rowCount = dataGridView1.Rows.Count;
                                            this.dataGridView1.Rows[1].Cells[3].Value = masterSerialNumberList.Count;
                                            this.dataGridView1.Rows[0].Cells[3].Value = downloadLocationList.Count;
                                            MessageBox.Show(MessageConstants.DownloadComplete, MessageConstants.DownloadComplete, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            Loading_Screen.CloseForm();
                                            tabControl1.Enabled = true;
                                            MessageBox.Show(MessageConstants.DownloadNotCompleteFailtotransferfiletoHandheld, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                else
                                {
                                    Loading_Screen.CloseForm();
                                    tabControl1.Enabled = true;
                                    MessageBox.Show(MessageConstants.DownloadNotCompleteFailtodownloadSKUMaster, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                Loading_Screen.CloseForm();
                                tabControl1.Enabled = true;
                                MessageBox.Show(MessageConstants.DownloadNotCompleteFailtodownloadLocation, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            Loading_Screen.CloseForm();
                            tabControl1.Enabled = true;
                            MessageBox.Show(MessageConstants.DownloadNotCompleteFailtodownloadUnit, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (radioLocation.Checked) // load แค่ location
                {
                    Loading_Screen.ShowSplashScreen();
                    if (locationFrom == string.Empty && locationTo == string.Empty)
                    {
                        Loading_Screen.CloseForm();
                        tabControl1.Enabled = true;
                        MessageBox.Show(MessageConstants.PleasEnterLocationFormOrLocationTo, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        unitList = hhtBll.GetUnit();
                        bool downloadUniSuccess = DoDownLoadUnit();
                        
                        if (downloadUniSuccess)
                        {
                            downloadLocationList = hhtBll.GetLocation(locationFrom, locationTo);
                            bool downloadLocationSuccess = DoDownloadLocation(downloadLocationList);
                            if (downloadLocationSuccess)
                            {
                                bool deleteSerial = ConnectSdfDeleteData("MasterSerialNumber");
                                if (deleteSerial)
                                {
                                    if (connectedCable)
                                    {
                                        hhtBll.D_ConnectDevice();
                                        if (hhtBll.D_PCTransferFileToHHT(HHTDBPath))
                                        {
                                            Loading_Screen.CloseForm();
                                            tabControl1.Enabled = true;
                                            int rowCount = dataGridView1.Rows.Count;
                                            this.dataGridView1.Rows[0].Cells[3].Value = downloadLocationList.Count;
                                            MessageBox.Show(MessageConstants.DownloadComplete, MessageConstants.DownloadComplete, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            Loading_Screen.CloseForm();
                                            tabControl1.Enabled = true;
                                            MessageBox.Show(MessageConstants.DownloadNotCompleteFailtotransferfiletoHandheld, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        hhtBll.D_DisconnectDevice();
                                    }
                                    else
                                    {
                                        if (hhtBll.FTPTransferPCToHHT(ipAddress, HHTDBPath))
                                        {
                                            Loading_Screen.CloseForm();
                                            tabControl1.Enabled = true;
                                            int rowCount = dataGridView1.Rows.Count;
                                            this.dataGridView1.Rows[0].Cells[3].Value = downloadLocationList.Count;
                                            MessageBox.Show(MessageConstants.DownloadComplete, MessageConstants.DownloadComplete, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            Loading_Screen.CloseForm();
                                            tabControl1.Enabled = true;
                                            MessageBox.Show(MessageConstants.DownloadNotCompleteFailtotransferfiletoHandheld, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                else
                                {
                                    Loading_Screen.CloseForm();
                                    tabControl1.Enabled = true;
                                    MessageBox.Show(MessageConstants.FaildeleteSKUMasterdataindatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                Loading_Screen.CloseForm();
                                tabControl1.Enabled = true;
                                MessageBox.Show(MessageConstants.DownloadNotCompleteFailtodownloadLocation, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            Loading_Screen.CloseForm();
                            tabControl1.Enabled = true;
                            MessageBox.Show(MessageConstants.DownloadNotCompleteFailtodownloadUnit, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                else if (radioBtnSKU.Checked)// load แค่ skU
                {
                    Loading_Screen.ShowSplashScreen();
                    unitList = hhtBll.GetUnit();
                    
                    bool downloadUniSuccess = DoDownLoadUnit();
                    if (downloadUniSuccess)
                    {
                        masterSerialNumberList = hhtBll.GetMasterSerialNumber();
                        bool downloadMasterSerialSuccess = DoDownLoadMasterSerialNumber(masterSerialNumberList);
                        if (downloadMasterSerialSuccess)
                        {
                            bool deleteSuccess = ConnectSdfDeleteData("Location");
                            if (deleteSuccess)
                            {
                                if (connectedCable)
                                {
                                    hhtBll.D_ConnectDevice();
                                    if (hhtBll.D_PCTransferFileToHHT(HHTDBPath))
                                    {
                                        Loading_Screen.CloseForm();
                                        tabControl1.Enabled = true;
                                        int rowCount = dataGridView1.Rows.Count;
                                        this.dataGridView1.Rows[0].Cells[3].Value = masterSerialNumberList.Count;
                                        MessageBox.Show(MessageConstants.DownloadComplete, MessageConstants.DownloadComplete, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        Loading_Screen.CloseForm();
                                        tabControl1.Enabled = true;
                                        int rowCount = dataGridView1.Rows.Count;
                                        this.dataGridView1.Rows[0].Cells[3].Value = masterSerialNumberList.Count;
                                        MessageBox.Show(MessageConstants.DownloadNotCompleteFailtotransferfiletoHandheld, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    hhtBll.D_DisconnectDevice();
                                }
                                else
                                {
                                    if (hhtBll.FTPTransferPCToHHT(ipAddress, HHTDBPath))
                                    {
                                        Loading_Screen.CloseForm();
                                        tabControl1.Enabled = true;
                                        int rowCount = dataGridView1.Rows.Count;
                                        this.dataGridView1.Rows[0].Cells[3].Value = masterSerialNumberList.Count;
                                        MessageBox.Show(MessageConstants.DownloadComplete, MessageConstants.DownloadComplete, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        Loading_Screen.CloseForm();
                                        tabControl1.Enabled = true;
                                        MessageBox.Show(MessageConstants.DownloadNotCompleteFailtotransferfiletoHandheld, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            else
                            {
                                Loading_Screen.CloseForm();
                                tabControl1.Enabled = true;
                                MessageBox.Show(MessageConstants.FaildeleteLocationdatainHandhelddatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            Loading_Screen.CloseForm();
                            tabControl1.Enabled = true;
                            MessageBox.Show(MessageConstants.DownloadNotCompleteFailtodownloadSKUMaster, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        Loading_Screen.CloseForm();
                        tabControl1.Enabled = true;
                        MessageBox.Show(MessageConstants.DownloadNotCompleteFailtodownloadUnit, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else// ไม่โหลดทั้ง location and sku
                {
                    Loading_Screen.ShowSplashScreen();
                    unitList = hhtBll.GetUnit();
                    bool downloadUniSuccess = DoDownLoadUnit();
                    if (downloadUniSuccess)
                    {
                        bool deleteLoSuccess = ConnectSdfDeleteData("Location");
                        if (deleteLoSuccess)
                        {
                            bool deleteSerial = ConnectSdfDeleteData("MasterSerialNumber");
                            if (deleteSerial)
                            {
                                if (connectedCable)
                                {
                                    hhtBll.D_ConnectDevice();
                                    if (hhtBll.D_PCTransferFileToHHT(HHTDBPath))
                                    {
                                        Loading_Screen.CloseForm();
                                        tabControl1.Enabled = true;
                                        MessageBox.Show(MessageConstants.DownloadCompleteDeleteAllTable, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        Loading_Screen.CloseForm();
                                        tabControl1.Enabled = true;
                                        MessageBox.Show(MessageConstants.DownloadNotComplete, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    hhtBll.D_DisconnectDevice();
                                }
                                else
                                {
                                    if (hhtBll.FTPTransferPCToHHT(ipAddress, HHTDBPath))
                                    {
                                        Loading_Screen.CloseForm();
                                        tabControl1.Enabled = true;
                                        MessageBox.Show(MessageConstants.DownloadCompleteDeleteAllTable, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        Loading_Screen.CloseForm();
                                        tabControl1.Enabled = true;
                                        MessageBox.Show(MessageConstants.DownloadNotComplete, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            else
                            {
                                Loading_Screen.CloseForm();
                                tabControl1.Enabled = true;
                                MessageBox.Show(MessageConstants.DownloadNotComplete, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            Loading_Screen.CloseForm();
                            tabControl1.Enabled = true;
                            MessageBox.Show(MessageConstants.DownloadNotComplete, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        Loading_Screen.CloseForm();
                        tabControl1.Enabled = true;
                        MessageBox.Show(MessageConstants.DownloadNotCompleteFailtodownloadUnit, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                tabControl1.Enabled = true;
                comnameTxtBox1.Text = "";
                hhtnameTxtBox1.Text = "";
                MessageBox.Show(MessageConstants.Nodeviceconnected, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool DoDownLoadUnit()
        {
            List<UnitModel> unitList = hhtBll.GetUnit();
            if (unitList.Count > 0)
            {
                ConnectSdfDeleteData("Unit");
                return ConnectSdfInsertUnitData(unitList);
            }
            logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "No data in MasterUnit", DateTime.Now);
            return false;
        }

        private bool DoDownLoadMasterBarcode()
        {
            List<MasterBarcodeModel> masterBarcodeList = hhtBll.GetMasterBarcode();
            if (masterBarcodeList.Count > 0)
            {
                ConnectSdfDeleteData("MasterBarcode");
                return ConnectSdfInsertMasterBarcodeData(masterBarcodeList);
            }
            logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "No data in MasterBarcode", DateTime.Now);
            return false;

        }

        private bool DoDownLoadMasterSerialNumber(List<MasterSerialNumberModel> masterSerialNumberList)
        {
            //List<MasterSerialNumberModel> masterSerialNumberList = hhtBll.GetMasterSerialNumber();
            this.dataGridView1.Rows.Add(this.dataGridView1.Rows.Count + 1, "MasterSKU", masterSerialNumberList.Count, 0);

            if (masterSerialNumberList.Count > 0)
            {
                ConnectSdfDeleteData("MasterSerialNumber");
                return ConnectSdfInsertMasterSerialData(masterSerialNumberList);
            }
            else
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "No data in MasterSerialNumber", DateTime.Now);
                return true;
            }
        }

        private bool DoDownLoadMasterPack()
        {
            List<MasterPackModel> masterPackList = hhtBll.GetMasterPack();
            if (masterPackList.Count > 0)
            {
                ConnectSdfDeleteData("MasterPack");
                return ConnectSdfInsertMasterPackData(masterPackList);
            }
            return false;
        }

        private bool DoDownloadLocation(List<DownloadLocationModel> downloadLocationList)
        {

            if (downloadLocationList.Count > 0)
            {

                this.dataGridView1.Rows.Add(this.dataGridView1.Rows.Count + 1, "Location", downloadLocationList.Count, 0);

                ConnectSdfDeleteData("Location");

                return ConnectSdfInsertLocationData(downloadLocationList);
            }
            else
            {
                MessageBox.Show(MessageConstants.NoLocationdatafound, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        private bool DoDownloadSKU(List<PCSKUModel> skuList)
        {
            if (skuList.Count > 0)
            {
                //BeginInvoke((MethodInvoker)delegate
                //{

                this.dataGridView1.Rows.Add(this.dataGridView1.Rows.Count + 1, "MasterSKU", skuList.Count, 0);

                //});

                ConnectSdfDeleteData("SKU");
                return ConnectSdfInsertSKUData(skuList);
            }
            else
            {
                MessageBox.Show(MessageConstants.NoSKUdataFound, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private bool ConnectSdfDeleteData(string type)
        {
            return hhtBll.ConnectSdfDeleteData(type);
        }

        private bool ConnectSdfInsertSKUData(List<PCSKUModel> skuList)
        {
            return hhtBll.ConnectSdfInsertSKUData(skuList, UserName);
        }

        private bool ConnectSdfInsertLocationData(List<DownloadLocationModel> locationList)
        {
            return hhtBll.ConnectSdfInsertLocationData(locationList,UserName);
        }

        private bool ConnectSdfInsertUnitData(List<UnitModel> unitList)
        {
            return hhtBll.ConnectSdfInsertUnitData(unitList, UserName);
        }

        private bool ConnectSdfInsertMasterBarcodeData(List<MasterBarcodeModel> masterBarcodeList)
        {
            return hhtBll.ConnectSdfInsertMasterBarcodeData(masterBarcodeList, UserName);
        }

        private bool ConnectSdfInsertMasterSerialData(List<MasterSerialNumberModel> masterSerialNumberList)
        {
            return hhtBll.ConnectSdfInsertMasterSerialData(masterSerialNumberList, UserName);
        }

        private bool ConnectSdfInsertMasterPackData(List<MasterPackModel> masterPackList)
        {
            return hhtBll.ConnectSdfInsertMasterPackData(masterPackList, UserName);
        }

        #endregion

        #region Report
        private void btnReport_AuditC_Click(object sender, EventArgs e)
        {
            try
            {
                ReportPrintForm fReport = new ReportPrintForm(UserName, "R08");
                fReport.StartPosition = FormStartPosition.CenterScreen;
                fReport.ShowDialog();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }

            //Loading_Screen.ShowSplashScreen();
            //tabControl1.Enabled = false;
            //SystemSettingBll settingBLL = new SystemSettingBll();
            //DateTime countDate = new DateTime();
            //var settingData = settingBLL.GetSettingData();
            //countDate = settingData.CountDate;
            //string allStoreType = "1,2,3,4";
            //string allLocationCode = string.Empty;
            //string allDepartmentCode = string.Empty;
            //string allSectionCode = string.Empty;
            //string allBrandCode = string.Empty;
            //DataTable dt = bllReportManagement.LoadReport_StocktakingAuditCheck(allLocationCode, allStoreType, countDate, allDepartmentCode, allSectionCode, allBrandCode);

            //ParameterFields paramFields = new ParameterFields();
            //ParameterField paramField = new ParameterField();
            //ParameterField paramField1 = new ParameterField();
            //ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();
            //ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
            //paramField.Name = "pCountDate";
            //paramDiscreteValue.Value = countDate;
            //paramField.CurrentValues.Add(paramDiscreteValue);
            //paramField1.Name = "pBranchName";
            //paramDiscreteValue1.Value = settingData.Branch;
            //paramField1.CurrentValues.Add(paramDiscreteValue1);
            //paramFields.Add(paramField);
            //paramFields.Add(paramField1);
            //if (dt.Rows.Count > 0)
            //{
            //    bool isCreateReportSuccess = reportForm.CreateReport(dt, "R07", paramFields);
            //    Loading_Screen.CloseForm();
            //    tabControl1.Enabled = true;
            //    if (isCreateReportSuccess)
            //    {
            //        reportForm.StartPosition = FormStartPosition.CenterParent;
            //        DialogResult dialogResult = reportForm.ShowDialog();
            //    }
            //    else
            //    {
            //        MessageBox.Show(MessageConstants.cannotgeneratereport, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            //else
            //{
            //    Loading_Screen.CloseForm();
            //    tabControl1.Enabled = true;
            //    MessageBox.Show(MessageConstants.Nodataforgeneratereport, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }


        private void btnReport_AuditA_Click(object sender, EventArgs e)
        {
            try
            {
                ReportPrintForm fReport = new ReportPrintForm(UserName, "R09");
                fReport.StartPosition = FormStartPosition.CenterScreen;
                fReport.ShowDialog();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            //Loading_Screen.ShowSplashScreen();
            //tabControl1.Enabled = false;
            //SystemSettingBll settingBLL = new SystemSettingBll();
            //DateTime countDate = new DateTime();
            //var settingData = settingBLL.GetSettingData();
            //countDate = settingData.CountDate;
            //string allStoreType = "1,2,3,4";
            //string allLocationCode = "";
            //string allCorrectDelete = "All";
            //string allDepartmentCode = string.Empty;
            //string allSectionCode = string.Empty;
            //string allSectionName = string.Empty;
            //string allBrandCode = string.Empty;
            //DataTable dt = bllReportManagement.LoadReport_StocktakingAudiAdjust(allLocationCode, allStoreType, allCorrectDelete, countDate, allDepartmentCode, allSectionCode, allBrandCode, allSectionName);

            //ParameterFields paramFields = new ParameterFields();
            //ParameterField paramField = new ParameterField();
            //ParameterField paramField1 = new ParameterField();
            //ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();
            //ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
            //paramField.Name = "pCountDate";
            //paramDiscreteValue.Value = countDate;
            //paramField.CurrentValues.Add(paramDiscreteValue);
            //paramField1.Name = "pBranchName";
            //paramDiscreteValue1.Value = settingData.Branch;
            //paramField1.CurrentValues.Add(paramDiscreteValue1);
            //paramFields.Add(paramField);
            //paramFields.Add(paramField1);

            //if (dt.Rows.Count > 0)
            //{
            //    bool isCreateReportSuccess = reportForm.CreateReport(dt, "R10", paramFields);
            //    Loading_Screen.CloseForm();
            //    tabControl1.Enabled = true;
            //    if (isCreateReportSuccess)
            //    {
            //        reportForm.StartPosition = FormStartPosition.CenterParent;
            //        DialogResult dialogResult = reportForm.ShowDialog();
            //    }
            //    else
            //    {
            //        MessageBox.Show(MessageConstants.cannotgeneratereport, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            //else
            //{
            //    Loading_Screen.CloseForm();
            //    tabControl1.Enabled = true;
            //    MessageBox.Show(MessageConstants.Nodataforgeneratereport, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }


        private void btnReport_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                ReportPrintForm fReport = new ReportPrintForm(UserName, "R11");
                fReport.StartPosition = FormStartPosition.CenterScreen;
                fReport.ShowDialog();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }

            //Loading_Screen.ShowSplashScreen();
            //tabControl1.Enabled = false;
            //SystemSettingBll settingBLL = new SystemSettingBll();
            //DateTime countDate = new DateTime();
            //var settingData = settingBLL.GetSettingData();
            //countDate = settingData.CountDate;
            //string branchName = settingData.Branch;
            //string allDepartmentCode = "";
            //string allSectionCode = "";
            //string allBrandCode = "";
            //string allStoreType = "1,2,3,4";
            //string allLocationCode = "";

            //DataTable dt = bllReportManagement.LoadReport_DeleteRecordReportByLocation(countDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType);

            //ParameterFields paramFields = new ParameterFields();
            //ParameterField paramField1 = new ParameterField();
            //ParameterField paramField2 = new ParameterField();
            //ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
            //ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
            //paramField1.Name = "pCountDate";
            //paramDiscreteValue1.Value = countDate;
            //paramField1.CurrentValues.Add(paramDiscreteValue1);
            //paramFields.Add(paramField1);

            //paramField2.Name = "pBranchName";
            //paramDiscreteValue2.Value = branchName;
            //paramField2.CurrentValues.Add(paramDiscreteValue2);
            //paramFields.Add(paramField2);

            //if (dt.Rows.Count > 0)
            //{
            //    bool isCreateReportSuccess = reportForm.CreateReport(dt, "R08", paramFields);
            //    Loading_Screen.CloseForm();
            //    tabControl1.Enabled = true;
            //    if (isCreateReportSuccess)
            //    {
            //        reportForm.StartPosition = FormStartPosition.CenterParent;
            //        DialogResult dialogResult = reportForm.ShowDialog();
            //    }
            //    else
            //    {
            //        MessageBox.Show(MessageConstants.cannotgeneratereport, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            //else
            //{
            //    Loading_Screen.CloseForm();
            //    tabControl1.Enabled = true;
            //    MessageBox.Show(MessageConstants.Nodataforgeneratereport, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void btnReport_Control_Click(object sender, EventArgs e)
        {
            try
            {
                ReportPrintForm fReport = new ReportPrintForm(UserName, "R12");
                fReport.StartPosition = FormStartPosition.CenterScreen;
                fReport.ShowDialog();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }

            //Loading_Screen.ShowSplashScreen();
            //tabControl1.Enabled = false;
            //SystemSettingBll settingBLL = new SystemSettingBll();
            //DateTime countDate = new DateTime();
            //var settingData = settingBLL.GetSettingData();
            //countDate = settingData.CountDate;
            ////string branchName = settingData.Branch;
            //string allSectionCode = "";
            //string allStoreType = "1,2,3,4";
            //string allDepartmentCode = string.Empty;
            //string allLocationCode = string.Empty;
            //string allBrandCode = string.Empty;
            //string allSectionName = string.Empty;

            //DataTable dt = bllReportManagement.GetReport_ControlSheet(allSectionCode, allStoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode, allSectionName);

            //ParameterFields paramFields = new ParameterFields();
            //ParameterField paramField = new ParameterField();
            //ParameterField paramField1 = new ParameterField();
            //ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();
            //ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
            //paramField.Name = "pCountDate";
            //paramDiscreteValue.Value = countDate;
            //paramField.CurrentValues.Add(paramDiscreteValue);
            //paramField1.Name = "pBranchName";
            //paramDiscreteValue1.Value = settingData.Branch;
            //paramField1.CurrentValues.Add(paramDiscreteValue1);
            //paramFields.Add(paramField);
            //paramFields.Add(paramField1);


            ////paramField2.Name = "pBranchName";
            ////paramDiscreteValue2.Value = branchName;
            ////paramField2.CurrentValues.Add(paramDiscreteValue2);
            ////paramFields.Add(paramField2);

            //if (dt.Rows.Count > 0)
            //{
            //    bool isCreateReportSuccess = reportForm.CreateReport(dt, "R11", paramFields);
            //    Loading_Screen.CloseForm();
            //    tabControl1.Enabled = true;
            //    if (isCreateReportSuccess)
            //    {
            //        reportForm.StartPosition = FormStartPosition.CenterParent;
            //        DialogResult dialogResult = reportForm.ShowDialog();
            //    }
            //    else
            //    {
            //        MessageBox.Show(MessageConstants.cannotgeneratereport, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            //else
            //{
            //    Loading_Screen.CloseForm();
            //    tabControl1.Enabled = true;
            //    MessageBox.Show(MessageConstants.Nodataforgeneratereport, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }
        #endregion

        private void btnCheckFile_Click(object sender, EventArgs e)
        {
            int count = 0;
            string pathFolder = ConfigurationManager.AppSettings["pathFTPFolder"];
            if (Directory.Exists(pathFolder))
            {
                string[] subdirectoryEntries = Directory.GetDirectories(pathFolder);
                if (subdirectoryEntries.Count() > 0)
                {
                    foreach (string subdirectory in subdirectoryEntries)
                    {
                        string[] zipFiles = Directory.GetFiles(subdirectory, "*.zip");
                        count = count + zipFiles.Count();
                    }
                }
                if (count > 0)
                {
                    MessageBox.Show("Files waiting to be uploaded : " + count);
                }
                else
                {
                    MessageBox.Show("There are no files waiting to be uploaded.");
                }
            }
            else
            {
                MessageBox.Show("Cannot find " + pathFolder);
            }
        }

        private void FillLocationFom()
        {
            if (textBoxLoFrom.Text.Trim().Length == 1)
            {
                textBoxLoFrom.Text = "0000" + textBoxLoFrom.Text.Trim();
            }
            else if (textBoxLoFrom.Text.Trim().Length == 2)
            {
                textBoxLoFrom.Text = "000" + textBoxLoFrom.Text.Trim();
            }
            else if (textBoxLoFrom.Text.Trim().Length == 3)
            {
                textBoxLoFrom.Text = "00" + textBoxLoFrom.Text.Trim();
            }
            else if (textBoxLoFrom.Text.Trim().Length == 4)
            {
                textBoxLoFrom.Text = "0" + textBoxLoFrom.Text.Trim();
            }
        }
        private void FillLocationTo()
        {
            if (textBoxLoTo.Text.Trim().Length == 1)
            {
                textBoxLoTo.Text = "0000" + textBoxLoTo.Text.Trim();
            }
            else if (textBoxLoTo.Text.Trim().Length == 2)
            {
                textBoxLoTo.Text = "000" + textBoxLoTo.Text.Trim();
            }
            else if (textBoxLoTo.Text.Trim().Length == 3)
            {
                textBoxLoTo.Text = "00" + textBoxLoTo.Text.Trim();
            }
            else if (textBoxLoTo.Text.Trim().Length == 4)
            {
                textBoxLoTo.Text = "0" + textBoxLoTo.Text.Trim();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            connectedCable = CheckCableConnection();
            if(!connectedCable)
            {
                comnameTxtBox1.Text = "";
                hhtnameTxtBox1.Text = "";
                MessageBox.Show(MessageConstants.Nodeviceconnected, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btnReceiveData_Click(object sender, EventArgs e)
        {
            connectedCable = GetAuditDataFromHHT();       
        }
    }
}
