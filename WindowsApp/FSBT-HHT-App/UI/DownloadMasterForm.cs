using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSBT_HHT_BLL;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using FSBT.HHT.App.Resources;
using FSBT_HHT_BLL;
using FSBT_HHT_Model;

namespace FSBT.HHT.App.UI
{
    public partial class DownloadMasterForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        public string UserName { get; set; }
        private string dataType { get; set; }
        private string catagoryFlag = "Department";
        private int modeFlg = 1;
        private DownloadMasterDataBll downloadMasterBLL = new DownloadMasterDataBll();
        private SystemSettingBll settingBLL = new SystemSettingBll();
        private GenTextFileBll genTextFileBLL = new GenTextFileBll();
        private List<string> dataFromTxt = new List<string>();
        private DataTable dataTableForShow = new DataTable();
        private bool BrowseSKU;
        private bool BrowseBarcode;
        private bool BrowsePackBarcode;
        private bool BrowseBrand;
        private string fileSKUName = "PCS0008 - Download File for Stocktaking (SP)";
        private string fileSKUFFName = "PCS0013-Download File for Stocktaking(ตลาดสด)";
        private string fileBarcodeName = "UPCR01P (POSFLIB) - Table file more than one Barcode (SP)";
        private string filePackName = "TABBOXSP (POSFLIB) - Table file of barcode (box) -S/P";
        private string fileBrandName = "PCS0006-Brand/Group Description for The Mall";
        List<FileModel> fileAS400 = new List<FileModel>();

        public DownloadMasterForm()
        {
            InitializeComponent();
            rbStock.Enabled = false;
            rbFreshfood.Enabled = false;
        }

        public DownloadMasterForm(string username)
        {
            UserName = username;
            InitializeComponent();
            rbStock.Enabled = false;
            rbFreshfood.Enabled = false;
        }

        private void DownloadMasterForm_Load(object sender, EventArgs e)
        {
            fileAS400 = genTextFileBLL.GetListFileNameDownloadAS400();
            var settingData = settingBLL.GetSettingData();

            filePathSKU.Text = settingData.PathSKUFile;
            filePathBarcode.Text = settingData.PathBarcodeFile;
            filePathPackBarcode.Text = settingData.PathPackBarcodeFile;
            filePathBrand.Text = settingData.PathBrandFile;

            BrowseSKU = btnBrowseSKU.Enabled;
            BrowseBarcode = btnBrowseBarcode.Enabled;
            BrowsePackBarcode = btnBrowsePackBarcode.Enabled;
            BrowseBrand = btnBrowseBrand.Enabled;

            string sectionType = settingData.SectionType.Substring(0, 1);
            switch (sectionType)
            {
                case "1":
                case "2":
                    rbFront.Checked = true;
                    modeFlg = 1;
                    break;
                case "3":
                    rbStock.Checked = true;
                    modeFlg = 3;
                    break;
                case "4":
                    rbFreshfood.Checked = true;
                    modeFlg = 4;
                    break;
            }

            DisableAllBrowseButton();
            btnBrowseSKU.Enabled = BrowseSKU;
            btnClearData.Enabled = btnClearData.Enabled;
            btnLoad.Enabled = btnLoad.Enabled;
            btnReport.Enabled = btnReport.Enabled;
            dataType = "SKU";
            masterDataGridView.ReadOnly = true;
        }

        private void rbFront_CheckedChanged(object sender, EventArgs e)
        {
            modeFlg = 1;
        }

        //private void rbBack_CheckedChanged(object sender, EventArgs e)
        //{
        //    modeFlg = 2;
        //}

        private void rbStock_CheckedChanged(object sender, EventArgs e)
        {
            modeFlg = 3;
        }

        private void rbFreshfood_CheckedChanged(object sender, EventArgs e)
        {
            modeFlg = 4;
        }

        private void rbSku_CheckedChanged(object sender, EventArgs e)
        {
            //EnableAllDownloadMode();
            DisableAllBrowseButton();
            btnBrowseSKU.Enabled = BrowseSKU;
            dataFromTxt.Clear();
            dataType = "SKU";
        }

        private void rbBarcode_CheckedChanged(object sender, EventArgs e)
        {
            //DisableAllDownloadMode();
            DisableAllBrowseButton();
            btnBrowseBarcode.Enabled = BrowseBarcode;
            dataFromTxt.Clear();
            dataType = "Barcode";
        }

        private void rbPackBarcode_CheckedChanged(object sender, EventArgs e)
        {
            //DisableAllDownloadMode();
            DisableAllBrowseButton();
            btnBrowsePackBarcode.Enabled = BrowsePackBarcode;
            dataFromTxt.Clear();
            dataType = "PackBarcode";
        }

        private void rbBrand_CheckedChanged(object sender, EventArgs e)
        {
            //DisableAllDownloadMode();
            DisableAllBrowseButton();
            btnBrowseBrand.Enabled = BrowseBrand;
            dataFromTxt.Clear();
            dataType = "Brand";
        }

        private void btnBrowseSKU_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = OpenFileDialog();
                if (filePath != "")
                {
                    filePathSKU.Text = filePath;
                }
            }
            catch(Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void btnBrowseBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = OpenFileDialog();
                if (filePath != "")
                {
                    filePathBarcode.Text = filePath;
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void btnBrowsePackBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = OpenFileDialog();
                if (filePath != "")
                {
                    filePathPackBarcode.Text = filePath;
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void btnBrowseBrand_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = OpenFileDialog();
                if (filePath != "")
                {
                    filePathBrand.Text = filePath;
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string file = "";
            bool isFileExist = false;
            Loading_Screen.ShowSplashScreen();
            try
            {
                switch (dataType)
                {
                    case "SKU":
                        file = filePathSKU.Text;
                        isFileExist = ReadTextFile(file);
                        break;
                    case "Barcode":
                        file = filePathBarcode.Text;
                        isFileExist = ReadTextFile(file);
                        break;
                    case "PackBarcode":
                        file = filePathPackBarcode.Text;
                        isFileExist = ReadTextFile(file);
                        break;
                    case "Brand":
                        file = filePathBrand.Text;
                        isFileExist = ReadTextFile(file);
                        break;
                }

                if (!isFileExist)
                {
                    Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.Pleaseuploadfile, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //Validate
                    //bool validate = ValidateLengthData();
                    dataTableForShow.Clear();
                    Hashtable result = downloadMasterBLL.AddDataToDatabase(dataType, dataFromTxt, modeFlg, dataTableForShow,catagoryFlag);
                    
                    if (result["result"].ToString() == "success")
                    {
                        GetSearchData();
                        //DataTable displayDataTable = (DataTable)result["resultTable"];
                        //dataTableForShow = displayDataTable;

                        //int count = dataTableForShow.Columns.Count;
                        //dataTableForShow.Columns.RemoveAt(count - 1);
                        //dataTableForShow.Columns.RemoveAt(count - 2);
                        //if (dataType == "Barcode")
                        //{
                        //    dataTableForShow.Columns.RemoveAt(count - 3);
                        //}

                        //DisplayData(dataTableForShow);
                        //Loading_Screen.CloseForm();
                    }
                    else if (result["result"].ToString() == "wrongformat")
                    {
                        Loading_Screen.CloseForm();
                        MessageBox.Show(MessageConstants.WrongFormatData, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (result["result"].ToString() == "null")
                    {
                        Loading_Screen.CloseForm();
                        MessageBox.Show(MessageConstants.WrongFormatFileEmptyfield, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (result["result"].ToString() == "wrongdata")
                    {
                        Loading_Screen.CloseForm();
                        MessageBox.Show(MessageConstants.WrongFormatFileTextinnumberfield, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Loading_Screen.CloseForm();
                        MessageBox.Show(MessageConstants.CannotLoadDataToDatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void textBoxFilter_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataTable searchData = SearchData();
                    DisplayData(searchData);
                }
                else
                {
                    //do nothing
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void summaryBrand_Click(object sender, EventArgs e)
        {
            try
            {
                Loading_Screen.ShowSplashScreen();
                DataTable searchResult = new DataTable();

                searchResult = downloadMasterBLL.GetSummaryByBrand(modeFlg);

                if (searchResult.Rows.Count > 0)
                {
                    DownloadMasterSummaryByBrandForm dSummary = new DownloadMasterSummaryByBrandForm(); // Instantiate a Form3 object.
                    dSummary.SummaryDownloadMaster(searchResult, modeFlg);
                    Loading_Screen.CloseForm();
                    dSummary.Show();
                }
                else
                {
                    Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.NoDatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void summaryDepartment_Click(object sender, EventArgs e)
        {
            try
            {
                Loading_Screen.ShowSplashScreen();
                DataTable searchResult = new DataTable();

                searchResult = downloadMasterBLL.GetSummary(modeFlg);

                if (searchResult.Rows.Count > 0)
                {
                    DownloadMasterSummaryForm dSummary = new DownloadMasterSummaryForm(); // Instantiate a Form3 object.
                    dSummary.SummaryDownloadMaster(searchResult, modeFlg);
                    Loading_Screen.CloseForm();
                    dSummary.Show();
                }
                else
                {
                    Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.NoDatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show(MessageConstants.Doyouwanttoclearalldata, MessageConstants.TitleConfirm, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    bool result = downloadMasterBLL.ClearDataTable();
                    if (result)
                    {
                        masterDataGridView.DataSource = null;
                        MessageBox.Show(MessageConstants.ClearDataSuccessful, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.CannotClearData, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do nothing
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                if(modeFlg == 4)
                {
                    ReportPrintForm fReport = new ReportPrintForm(UserName, "R04");
                    fReport.StartPosition = FormStartPosition.CenterScreen;
                    fReport.Show();
                }
                else if(modeFlg == 3)
                {
                    ReportPrintForm fReport = new ReportPrintForm(UserName, "R27");
                    fReport.StartPosition = FormStartPosition.CenterScreen;
                    fReport.Show();
                }
                else
                {
                    ReportPrintForm fReport = new ReportPrintForm(UserName, "R03");
                    fReport.StartPosition = FormStartPosition.CenterScreen;
                    fReport.Show();
                }
                ////downloadMasterBLL.ExportExcel(dataType, modeFlg);
                //Loading_Screen.ShowSplashScreen();
                //DataTable searchResult = new DataTable();
                //string reportPath = "";

                //if (modeFlg == 4)
                //{
                //    searchResult = downloadMasterBLL.ExportPDF4(modeFlg);
                //    reportPath = "/ReportTemplate/MasterReport_4.rpt";
                //}
                //else if(modeFlg == 3)
                //{
                //    searchResult = downloadMasterBLL.ExportPDF123(modeFlg);
                //    reportPath = "/ReportTemplate/MasterReport_3.rpt";
                //}
                //else
                //{
                //    searchResult = downloadMasterBLL.ExportPDF123(modeFlg);
                //    reportPath = "/ReportTemplate/MasterReport_12.rpt";
                //}

                //if (searchResult.Rows.Count > 0)
                //{
                //    MasterReportForm masterReport = new MasterReportForm();
                //    bool isCreateReportSuccess = masterReport.CreateReport(searchResult, reportPath, dataType, modeFlg);
                //    Loading_Screen.CloseForm();
                //    if (isCreateReportSuccess)
                //    {
                //        masterReport.StartPosition = FormStartPosition.CenterParent;
                //        DialogResult dialogResult = masterReport.ShowDialog();
                //    }
                //    else
                //    {
                //        MessageBox.Show(MessageConstants.cannotgeneratereport, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    }
                //    //masterReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, fullexportPath);
                //}
                //else
                //{
                //    Loading_Screen.CloseForm();
                //    MessageBox.Show(MessageConstants.NoDatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        public DataTable SearchData()
        {
            DataTable searchTable = new DataTable();
            try
            {
                string searchWord = textBoxFilter.Text;
                if (searchWord == "")
                {
                    return dataTableForShow;
                }
                else
                {
                    var searchQuery = dataTableForShow
                                        .Rows
                                        .Cast<DataRow>()
                                        .Where(r => r.ItemArray.Any(
                                            c => c.ToString().ToLower().Contains(searchWord.ToLower())
                                        ));
                    if (searchQuery.Any())
                    {
                        searchTable = searchQuery.CopyToDataTable();
                    }
                    return searchTable;
                }
            }
            catch (Exception ex)
            {
                return searchTable = new DataTable();
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void DisableAllBrowseButton()
        {
            btnBrowseSKU.Enabled = false;
            btnBrowseBarcode.Enabled = false;
            btnBrowsePackBarcode.Enabled = false;
            btnBrowseBrand.Enabled = false;
        }

        //private void DisableAllDownloadMode()
        //{
        //    rbFront.Enabled = false;
        //    //rbBack.Enabled = false;
        //    rbStock.Enabled = false;
        //    rbFreshfood.Enabled = false;
        //}

        //private void EnableAllDownloadMode()
        //{
        //    rbFront.Enabled = true;
        //    //rbBack.Enabled = true;
        //    rbStock.Enabled = true;
        //    rbFreshfood.Enabled = true;
        //}

        private string OpenFileDialog()
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                return file;
            }
            return "";
        }

        private bool ReadTextFile(string file)
        {
            try
            {
                dataFromTxt.Clear();
                StreamReader steamReader = new StreamReader(file, Encoding.GetEncoding("TIS-620"));
                DataTable dataTable = new DataTable();

                DateTime sysDate = DateTime.Now;

                dataFromTxt = new List<string>();//File.ReadAllLines(path, Encoding.GetEncoding("TIS-620")).ToList();

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
                steamReader.Dispose();

                int lastData = dataFromTxt.Count - 1;
                if(dataFromTxt[lastData].Length == 1)
                {
                    dataFromTxt.RemoveAt(lastData);
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                MessageBox.Show(MessageConstants.CannotBrowse, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void DisplayData(DataTable masterData)
        {
            try
            {
                //masterDataGridView.Rows.Clear();
                //int count = masterData.Columns.Count;
                //DataTable tmp = new DataTable();
                //tmp.Merge(masterData);
                //tmp.Columns.RemoveAt(count - 1);
                //tmp.Columns.RemoveAt(count - 2);
                //if(dataType == "Barcode")
                //{
                //    tmp.Columns.RemoveAt(count - 3);
                //}
                masterDataGridView.Columns.Clear();
                masterDataGridView.DataSource = masterData;
                masterDataGridView.AutoResizeColumns();
                masterDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //switch (dataType)
                //{
                //    case "SKU":
                //        switch (modeFlg)
                //        {
                //            case 1:
                //            case 2:
                //                {
                //                    foreach (DataRow row in masterData.Rows)
                //                    {
                //                        this.masterDataGridView.Rows.Add(row[7].ToString()
                //                                                         , row[1].ToString()
                //                                                         , row[0].ToString()
                //                                                         , row[2].ToString()
                //                                                         , row[3].ToString()
                //                                                         , row[4].ToString()
                //                                                         , row[5].ToString()
                //                                                         , row[6].ToString());

                //                    }
                //                    break;
                //                }
                //            case 3:
                //            case 4:
                //                {
                //                    foreach (DataRow row in masterData.Rows)
                //                    {
                //                        this.masterDataGridView.Rows.Add(row[1].ToString()
                //                                                         , row[8].ToString()
                //                                                         , row[7].ToString()
                //                                                         , ""
                //                                                         , row[4].ToString()
                //                                                         , row[5].ToString()
                //                                                         , row[6].ToString()
                //                                                         , row[2].ToString());

                //                    }
                //                    break;
                //                }
                //        }
                //        break;
                //    case "Barcode":
                //        {
                //            foreach (DataRow row in masterData.Rows)
                //            {
                //                this.masterDataGridView.Rows.Add(row[1].ToString()
                //                                                  , row[2].ToString());
                //            }
                //            break;
                //        }
                //    case "PackBarcode":
                //        {
                //            break;
                //        }
                //}
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }

        private void GetSearchData()
        {
            Loading_Screen.ShowSplashScreen();
            dataTableForShow.Clear();
            DataTable result = downloadMasterBLL.GetMasterDownload(modeFlg, dataType);

            if (result.Rows.Count > 0)
            {
                dataTableForShow = result;
                DisplayData(dataTableForShow);
                Loading_Screen.CloseForm();
            }
            else
            {
                Loading_Screen.CloseForm();
                MessageBox.Show(MessageConstants.NoDatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void rbDepartment_CheckedChanged(object sender, EventArgs e)
        {
            catagoryFlag = "Department";
            rbStock.Enabled = false;
            rbFreshfood.Enabled = false;
        }

        private void rbSuper_CheckedChanged(object sender, EventArgs e)
        {
            catagoryFlag = "Super";
            rbStock.Enabled = true;
            rbFreshfood.Enabled = true;
        }

        //private bool ValidateLengthData()
        //{
        //    try
        //    {
        //        List<FileModelDetail> AS400Detail = new List<FileModelDetail>();
        //        int lastRecord = 0;
        //        int lengthFile = 0;
        //        int fileID = 0;
        //        switch (dataType)
        //        {
        //            case "SKU":
        //                switch (modeFlg)
        //                {
        //                    case 1:
        //                    case 2:
        //                    case 3:
        //                        fileID = Int32.Parse(fileAS400.AsEnumerable().Where(x => x.fileName.Equals(fileSKUName)).Select(x => x.fileID).FirstOrDefault().ToString());
        //                        AS400Detail = genTextFileBLL.GetFileConfigDetail(fileID);
        //                        lastRecord = AS400Detail.Count() - 1;
        //                        lengthFile = AS400Detail[lastRecord].EndPos;
        //                        foreach (string line in dataFromTxt)
        //                        {
        //                            if (line.Length < lengthFile)
        //                            {
        //                                return false;
        //                            }
        //                        }
        //                        break;
        //                    case 4:
        //                        fileID = Int32.Parse(fileAS400.Where(x => x.fileName.Equals(fileSKUFFName)).Select(x => x.fileID).FirstOrDefault().ToString());
        //                        AS400Detail = genTextFileBLL.GetFileConfigDetail(fileID);
        //                        lastRecord = AS400Detail.Count() - 1;
        //                        lengthFile = AS400Detail[lastRecord].EndPos;
        //                        foreach (string line in dataFromTxt)
        //                        {
        //                            if (line.Length < lengthFile)
        //                            {
        //                                return false;
        //                            }
        //                        }
        //                        break;
        //                }
        //                break;
        //            case "Barcode":
        //                fileID = Int32.Parse(fileAS400.Where(x => x.fileName.Equals(fileBarcodeName)).Select(x => x.fileID).FirstOrDefault().ToString());
        //                AS400Detail = genTextFileBLL.GetFileConfigDetail(fileID);
        //                lastRecord = AS400Detail.Count() - 1;
        //                lengthFile = AS400Detail[lastRecord].EndPos;
        //                foreach (string line in dataFromTxt)
        //                {
        //                    if (line.Length < lengthFile)
        //                    {
        //                        return false;
        //                    }
        //                }
        //                break;
        //            case "PackBarcode":
        //                fileID = Int32.Parse(fileAS400.Where(x => x.fileName.Equals(filePackName)).Select(x => x.fileID).FirstOrDefault().ToString());
        //                AS400Detail = genTextFileBLL.GetFileConfigDetail(fileID);
        //                lastRecord = AS400Detail.Count() - 1;
        //                lengthFile = AS400Detail[lastRecord].EndPos;
        //                foreach (string line in dataFromTxt)
        //                {
        //                    if (line.Length < lengthFile)
        //                    {
        //                        return false;
        //                    }
        //                }
        //                break;
        //            case "Brand":
        //                fileID = Int32.Parse(fileAS400.Where(x => x.fileName.Equals(fileBrandName)).Select(x => x.fileID).FirstOrDefault().ToString());
        //                AS400Detail = genTextFileBLL.GetFileConfigDetail(fileID);
        //                lastRecord = AS400Detail.Count() - 1;
        //                lengthFile = AS400Detail[lastRecord].EndPos;
        //                foreach (string line in dataFromTxt)
        //                {
        //                    if (line.Length < lengthFile)
        //                    {
        //                        return false;
        //                    }
        //                }
        //                break;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(String.Format("Exception : {0}", ex.StackTrace));
        //        return false;
        //    }
        //}
    }
}
