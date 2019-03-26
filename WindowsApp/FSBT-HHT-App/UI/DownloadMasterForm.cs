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
using FSBT_HHT_DAL;
using FSBT_HHT_Model;
using SFTP;
using AcknowledgeUploader;
using System.Globalization;
using Download;
using System.Reflection;
using System.Diagnostics;
namespace FSBT.HHT.App.UI
{
    public partial class DownloadMasterForm : Form
    {
        public string UserName { get; set; }
        private string dataType { get; set; }
        private string server { get; set; }
        private bool IsValidatedCountsheet { get; set; }
        private int modeFlg = 1;
        private DownloadMasterDataBll downloadMasterBLL = new DownloadMasterDataBll();
        private SystemSettingBll settingBLL = new SystemSettingBll();
        private GenTextFileBll genTextFileBLL = new GenTextFileBll();
        private TempDataTableBll tempbll = new TempDataTableBll();
        private TempFileImportBll tempFileImportBll = new TempFileImportBll();
        private ValidatorBll validatorBll = new ValidatorBll();
        private List<string> dataFromTxt = new List<string>();
        private DataTable dataTableForShow = new DataTable();
        private string FilePathSAPSku = "";
        private string FilePathSAPBarcode = "";
        private string FilePathRegularPrice = "";
        private LogErrorBll logBll = new LogErrorBll();
        List<Tuple<string, string>> fileNamesError = new List<Tuple<string, string>>();
        List<Tuple<string, int, int>> fileNamesFull = new List<Tuple<string, int, int>>();

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
            var settingData = settingBLL.GetSettingData();
            InitialAllItem();
            btnClearData.Enabled = btnClearData.Enabled;
            btnReport.Enabled = btnReport.Enabled;
            masterDataGridView.ReadOnly = true;
            SettingNameOfButton(settingData);
        }

        private void SettingDataType()
        {
            if (rbSku.Checked)
            {
                dataType = rbSku.Text;
                server = "SFTP";
            }
            else if (rbBarcode.Checked)
            {
                dataType = rbBarcode.Text;
                server = "SFTP";
            }
            else
            {
                dataType = rbRegPrice.Text.Replace(" ", "");
                server = "Share";
            }
        }

        private void DisableAllBrowseButtonAndTextbox()
        {
            btnBrowseSKU.Enabled = false;
            btnBrowseBarcode.Enabled = false;
            btnBrowseRegPrice.Enabled = false;

            filePathSKU.Enabled = false;
            filePathBarcode.Enabled = false;
            filePathRegPrice.Enabled = false;
        }

        private void SettingBrowseButtonAndTextBox()
        {
            DisableAllBrowseButtonAndTextbox();
            if (rbServer.Checked)
            {
                DefaultServerPath();
            }
            else
            {
                ClearAllFilePaths();
                if (rbSku.Checked)
                {
                    btnBrowseSKU.Enabled = true;
                    filePathSKU.Enabled = true;
                }
                else if (rbBarcode.Checked)
                {
                    btnBrowseBarcode.Enabled = true;
                    filePathBarcode.Enabled = true;
                }
                else
                {
                    btnBrowseRegPrice.Enabled = true;
                    filePathRegPrice.Enabled = true;
                }
            }
        }

        private void SettingLoadButton()
        {
            if (!string.IsNullOrEmpty(filePathSKU.Text) || !string.IsNullOrEmpty(filePathBarcode.Text) || !string.IsNullOrEmpty(filePathRegPrice.Text))
            {
                btnLoad.Enabled = true;
            }
            else
            {
                btnLoad.Enabled = false;
            }
        }

        private void SettingNameOfButton(SystemSettingModel settingData)
        {
            btnSumSubDep.Text = "Summary "+ settingData.MCHLevel1;
            btnSumMaterial.Text = "Summary " + settingData.MCHLevel4;
        }

        private void ClearAllFilePaths()
        {
            filePathSKU.Text = "";
            filePathBarcode.Text = "";
            filePathRegPrice.Text = "";
        }

        private void InitialAllItem()
        {
            SettingDataType();
            SettingBrowseButtonAndTextBox();
            SettingLoadButton();
            InitAllFileNames();
        }

        private void DefaultServerPath()
        {
            filePathSKU.Text = "";
            filePathBarcode.Text = "";
            filePathRegPrice.Text = "";
            if (dataType == "SKU")
                filePathSKU.Text = FilePathSAPSku;
            if (dataType == "Barcode")
                filePathBarcode.Text = FilePathSAPBarcode;
            if (dataType == "RegularPrice")
                filePathRegPrice.Text = FilePathRegularPrice;
        }

        private void DisableTextBox()
        {
            filePathSKU.Enabled = false;
            filePathBarcode.Enabled = false;
            filePathRegPrice.Enabled = false;
            if (!rbServer.Checked)
            {
                if (dataType == "SKU")
                    filePathSKU.Enabled = true;
                if (dataType == "Barcode")
                    filePathBarcode.Enabled = true;
                if (dataType == "RegularPrice")
                    filePathRegPrice.Enabled = true;
            }
        }

        private void InitAllFileNames()
        {
            string errorMeassge = "";

            Loading_Screen.ShowSplashScreen();
            DownloadFile skuData = new DownloadFile("SFTP", "SKU",UserName);
            FilePathSAPSku = skuData.filePathRemote;
            errorMeassge = skuData.errorMeassge;
            Loading_Screen.CloseForm();

            if (!string.IsNullOrEmpty(errorMeassge))
                MessageBox.Show("SFTP ERROR : " + errorMeassge + " of SKU.");

            Loading_Screen.ShowSplashScreen();
            DownloadFile barcodeData = new DownloadFile("SFTP", "BarCode",UserName);
            FilePathSAPBarcode = barcodeData.filePathRemote;
            errorMeassge = barcodeData.errorMeassge;
            Loading_Screen.CloseForm();
            if (!string.IsNullOrEmpty(errorMeassge))
                MessageBox.Show("SFTP ERROR : " + errorMeassge + " of Barcode.");

            FilePathRegularPrice = new DownloadFile("Share", "RegularPrice",UserName).filePathRemote;
        }

        private void SettingAllItem()
        {
            SettingDataType();
            SettingBrowseButtonAndTextBox();
            SettingLoadButton();
        }

        private void OnCheckedChanged(object sender, EventArgs e)
        {
            SettingAllItem();
        }

        private void btnBrowseSKU_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> filePaths = OpenMultiFileDialog("SKU");
                if (filePaths.Count > 0)
                {
                    filePathSKU.Text = "";
                    foreach (var f in filePaths)
                    {
                        if (filePathSKU.Text == "")
                        {
                            filePathSKU.Text += f;
                        }
                        else
                        {
                            filePathSKU.Text += "," + f;
                        }
                    }
                }
                SettingLoadButton();
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void btnBrowseBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> filePaths = OpenMultiFileDialog("Barcode");
                if (filePaths.Count > 0)
                {
                    filePathBarcode.Text = "";
                    foreach (var f in filePaths)
                    {
                        if (filePathBarcode.Text == "")
                        {
                            filePathBarcode.Text += f;
                        }
                        else
                        {
                            filePathBarcode.Text += "," + f;
                        }
                    }
                }
                SettingLoadButton();
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void btnBrowseRegPrice_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> filePaths = OpenMultiFileDialog("Price");
                if (filePaths.Count > 0)
                {
                    filePathRegPrice.Text = "";
                    foreach (var f in filePaths)
                    {
                        if (filePathRegPrice.Text == "")
                        {
                            filePathRegPrice.Text += f;
                        }
                        else
                        {
                            filePathRegPrice.Text += "," + f;
                        }
                    }
                }
                SettingLoadButton();
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                    dSummary.ShowDialog();
                }
                else
                {
                    Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.NoDatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void summarySubDepartment_Click(object sender, EventArgs e)
        {
            try
            {
                Loading_Screen.ShowSplashScreen();
                DataTable searchResult = new DataTable();

                searchResult = downloadMasterBLL.GetSummarySubDepartment(modeFlg);

                if (searchResult.Rows.Count > 0)
                {
                    DownloadMasterSummarySubDepartmentForm dSummary = new DownloadMasterSummarySubDepartmentForm(); // Instantiate a Form3 object.
                    dSummary.SummaryDownloadMaster(searchResult, modeFlg);
                    Loading_Screen.CloseForm();
                    dSummary.ShowDialog();
                }
                else
                {
                    Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.NoDatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show(MessageConstants.Doyouwanttoclearalldata, MessageConstants.TitleConfirm, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    Loading_Screen.ShowSplashScreen();
                    bool result = downloadMasterBLL.ClearMasterSAPDataTable();
                    Loading_Screen.CloseForm();
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
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (modeFlg == 4)
                {
                    ReportPrintForm fReport = new ReportPrintForm(UserName, "R04");
                    fReport.StartPosition = FormStartPosition.CenterScreen;
                    fReport.Show();
                }
                else if (modeFlg == 3)
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
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return searchTable = new DataTable();
            }
        }

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

        private List<string> OpenMultiFileDialog(string type)
        {
            var plants = tempFileImportBll.Plants();           
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (type == "SKU")
                {
                    string filter = "txt files (*.txt)|";
                    foreach(var plant in plants )
                    {
                        filter = filter + String.Format("*_MAIN_{0}_*.txt;", plant);
                    }
                    openFileDialog.Filter = filter;
                    //openFileDialog.Filter = "txt files (*.txt)|*_MAIN_*.txt";
                }
                else if (type == "Barcode")
                {
                    string filter = "txt files (*.txt)|";
                    foreach (var plant in plants)
                    {
                        filter = filter + String.Format("*_EAN_{0}_*.txt;", plant);
                    }                  
                    openFileDialog.Filter = filter;
                    //openFileDialog.Filter = "txt files (*.txt)|*_EAN_*.txt";
                }
                else
                {
                    string filter = "txt files (*.txt)|";
                    foreach (var plant in plants)
                    {
                        filter = filter + String.Format("{0}_*.txt;", plant);
                    }
                    openFileDialog.Filter = filter;
                }
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    List<string> fileNames = new List<string>();
                    foreach (String file in openFileDialog.FileNames)
                    {
                        fileNames.Add(file);
                    }
                    return fileNames;
                }
            }
            return new List<string>();
        }

        private void DisplayData(DataTable masterData)
        {
            try
            {
                //Stopwatch timer = Stopwatch.StartNew();
                masterDataGridView.Columns.Clear();

                masterDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; //or even better .DisableResizing. Most time consumption enum is DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
                // set it to false if not needed
                masterDataGridView.RowHeadersVisible = false;

                masterDataGridView.DataSource = masterData;

                //masterDataGridView.AutoResizeColumns();
                //masterDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                //timer.Stop();
                //TimeSpan timespan = timer.Elapsed;

                //string time = String.Format("{0:00}:{1:00}:{2:00}", timespan.Minutes, timespan.Seconds, timespan.Milliseconds / 10);
                //MessageBox.Show(time);
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }

        private void GetSearchData()
        {
            try
            {
                //Loading_Screen.ShowSplashScreen();
                dataTableForShow.Clear();
                DataTable result = downloadMasterBLL.GetMasterDownload(modeFlg, dataType);

                if (result.Rows.Count > 0)
                {
                    dataTableForShow = result;
                    DisplayData(dataTableForShow);
                    //Loading_Screen.CloseForm();
                }
                else
                {
                    //Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.NoDatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }

        }

        private void GetMasterData()
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
            }
        }

        private void GetErrorData()
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
            }
        }

        private void summaryMaterialGroup_Click(object sender, EventArgs e)
        {
            try
            {
                Loading_Screen.ShowSplashScreen();
                DataTable searchResult = new DataTable();

                searchResult = downloadMasterBLL.GetSummaryMaterialGroup(modeFlg);

                if (searchResult.Rows.Count > 0)
                {
                    DownloadMasterSummaryMaterialGroupForm dSummary = new DownloadMasterSummaryMaterialGroupForm(); // Instantiate a Form3 object.
                    dSummary.SummaryDownloadMaster(searchResult, modeFlg);

                    Loading_Screen.CloseForm();
                    dSummary.ShowDialog();
                }
                else
                {
                    Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.NoDatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void summaryStorgeLocation_Click(object sender, EventArgs e)
        {
            try
            {
                //Loading_Screen.ShowSplashScreen();
                DataTable searchResult = new DataTable();

                searchResult = downloadMasterBLL.GetSummaryStorageLocation(modeFlg);

                if (searchResult.Rows.Count > 0)
                {
                    DownloadMasterSummaryStorageLocationForm dSummary = new DownloadMasterSummaryStorageLocationForm(); // Instantiate a Form3 object.
                    dSummary.SummaryDownloadMaster(searchResult, modeFlg);

                    //Loading_Screen.CloseForm();
                    dSummary.ShowDialog();
                }
                else
                {
                    //Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.NoDatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                //Loading_Screen.CloseForm();
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            Loading_Screen.ShowSplashScreen();
            bool isSendAck = false;
            string errorMsg1 = "";
            string errorMsg2 = "";
            fileNamesError = new List<Tuple<string, string>>();
            fileNamesFull = new List<Tuple<string, int, int>>();
            try
            {
                #region Download from Server
                if (rbServer.Checked)
                {
                    if (rbSku.Checked || rbBarcode.Checked)
                    {
                        isSendAck = true;
                    }
                    else
                    {
                        isSendAck = false;
                    }

                    DownloadFile downloadFile = new DownloadFile(server, dataType,UserName);
                    if (server == "SFTP")
                    {
                        errorMsg1 = downloadFile.GetListOfSFTPFiles();
                        errorMsg2 = downloadFile.DownloadFile();
                    }
                    else
                    {
                        var plant = tempFileImportBll.Plants();
                        errorMsg1 = downloadFile.GetListOfRegularFiles(plant);
                        errorMsg2 = downloadFile.CopyFileFromShareDrive();
                    }

                    if (string.IsNullOrEmpty(errorMsg1) && string.IsNullOrEmpty(errorMsg2) && downloadFile.downloadedFiles.Count > 0)
                    {
                        ProcessData(downloadFile, dataType);
                        if (isSendAck)
                        {
                            SendAcknowledge();
                        }
                    }
                    else
                    {
                        logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, errorMsg1 + "," + errorMsg2, DateTime.Now);
                    }
                    ShowError();
                }
                #endregion Server

                #region Manual
                else
                {
                    string listPath;
                    if (rbSku.Checked)
                    {
                        isSendAck = false;
                        listPath = filePathSKU.Text;
                    }
                    else if (rbBarcode.Checked)
                    {
                        isSendAck = false;
                        listPath = filePathBarcode.Text;
                    }
                    else
                    {
                        isSendAck = false;
                        listPath = filePathRegPrice.Text;
                    }
                    DownloadFile downloadFile = new DownloadFile(server, dataType, UserName);
                    errorMsg1 = downloadFile.ExtractFileFromTextBox(listPath);
                    if (string.IsNullOrEmpty(errorMsg1))
                    {
                        ProcessData(downloadFile, dataType);
                    }
                    ShowError();
                }
                #endregion manual
            }
            catch(Exception ex)
            {
                Loading_Screen.CloseForm();
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void ProcessData(DownloadFile downloadFile, string type)
        {
            try
            {
                foreach (var file in downloadFile.downloadedFiles)
                {
                    int plantError = -1;
                    int countsheetError = -1;
                    int dataError = -1;
                    int numberOfRows = 0;
                    logBll.SetFlagErrorLog();
                    downloadFile.DeleteTempData();

                    string filepath = Path.Combine(downloadFile.filePathLocal, file.filename);
                    List<string> textFromfile = downloadFile.ReadTextFile(filepath);
                    numberOfRows = downloadFile.numberOfRows;

                    if (textFromfile == null)
                    {
                        fileNamesFull = downloadFile.fileNamesFull;
                        fileNamesError = downloadFile.fileNamesError;
                    }
                    else
                    {
                        DialogResult confirmResult = DialogResult.No;
                        bool countsheetExist = false;
                        bool plantExist = false;

                        if (type == "RegularPrice")
                        {
                            plantExist = validatorBll.IsThisPlantExist(file.plant);
                        }
                        else
                        {
                            countsheetExist = validatorBll.IsThisCountsheetExist(type, file.countsheet);
                        }

                        if (countsheetExist)
                        {
                            int numberOfRecord = validatorBll.GetNumberOfRecordsOfCountSheetInMaster(type, file.countsheet);
                            if (numberOfRecord == textFromfile.Count)
                            {
                                string confirmMsg = file.countsheet + " is exist , Please confirm to update data";
                                Loading_Screen.CloseForm(); 
                                confirmResult = MessageBox.Show(confirmMsg,"Confirm", MessageBoxButtons.YesNo);
                                Loading_Screen.ShowSplashScreen();
                            }
                            else
                            {
                                fileNamesError.Add(new Tuple<string, string>(file.filename, "Total records is not equal with current data in database"));
                                logBll.LogError(UserName, MethodBase.GetCurrentMethod().Name, file.filename, "Total records is not equal with current data in database", DateTime.Now);
                            }
                        }

                        if (plantExist && type != "RegularPrice"  )
                        {
                            int numberOfRecord = validatorBll.GetNumberOfRecordsOfPlantInMaster(file.plant);
                            if (numberOfRecord == textFromfile.Count)
                            {
                                string confirmMsg = file.plant + " is exist , Please confirm to update data";
                                Loading_Screen.CloseForm(); 
                                confirmResult = MessageBox.Show(confirmMsg,"Confirm",MessageBoxButtons.YesNo);
                                Loading_Screen.ShowSplashScreen();
                            }
                            else 
                            {
                                fileNamesError.Add(new Tuple<string, string>(file.filename, "Total records is not equal with current data in database"));
                                logBll.LogError(UserName,  MethodBase.GetCurrentMethod().Name,file.filename, "Total records is not equal with current data in database", DateTime.Now);
                            }
                        }

                        if (confirmResult == DialogResult.Yes || (!countsheetExist && !plantExist) || (type == "RegularPrice" && plantExist))
                        {
                            if (countsheetExist && confirmResult == DialogResult.Yes)
                                validatorBll.DeleteMasterPerCountsheet(type, file.countsheet);

                            if (plantExist && confirmResult == DialogResult.Yes)
                                validatorBll.DeleteMasterPerPlant(file.plant);

                            downloadFile.InsertDataToTempTable(textFromfile, file.fileformat, file.filename);
                            if (string.IsNullOrEmpty(downloadFile.errorMeassge))
                            {
                                plantError = validatorBll.ValidatePlant(type, file.filename, file.plant);
                                countsheetError = validatorBll.ValidateCountSheet(type, file.filename, file.countsheet);
                                dataError = validatorBll.ValidateDuplicateData(downloadFile.fileFormatID, file.filename);
                            }

                            if (plantError == 0 && countsheetError == 0 && dataError == 0)
                            {
                                fileNamesFull.Add(new Tuple<string, int, int>(file.filename, numberOfRows, 0));
                                downloadMasterBLL.ConvertMasterFromTempToReal(type);
                                logBll.LogError(UserName, MethodBase.GetCurrentMethod().Name, file.filename, "Load data success", DateTime.Now);
                            }
                            else 
                            {
                                if (type == "RegularPrice")
                                {
                                    var fileError = tempFileImportBll.GetRegularPriceFileError();
                                    logBll.InsertErrorLog(type);
                                    fileNamesError.Add(new Tuple<string, string>(file.filename, "file error click ViewLog to see detail"));
                                    fileNamesFull.Add(new Tuple<string, int, int>(file.filename, numberOfRows, fileError.Count));
                                }
                                else if (type == "SKU")
                                {
                                    var fileError = tempFileImportBll.GetSKUFileError();
                                    logBll.InsertErrorLog(type);
                                    fileNamesError.Add(new Tuple<string, string>(file.filename, "file error click ViewLog to see detail"));
                                    fileNamesFull.Add(new Tuple<string, int, int>(file.filename, numberOfRows, fileError.Count));
                                }
                                else
                                {
                                    var fileError = tempFileImportBll.GetBarcodeFileError();
                                    logBll.InsertErrorLog(type);
                                    fileNamesError.Add(new Tuple<string, string>(file.filename, "file error click ViewLog to see detail"));
                                    fileNamesFull.Add(new Tuple<string, int, int>(file.filename, numberOfRows, fileError.Count));
                                }
                            }
                        }
                    }
                    if (type == "RegularPrice" && rbServer.Checked)
                        downloadFile.MoveFileToBackup(file.filenamewithpath);
                    Loading_Screen.CloseForm(); 
                }

                if ((type == "SKU" || type == "Barcode" ) && rbServer.Checked)
                {
                    List<string> files = fileNamesFull.Select(x => x.Item1).ToList();
                    downloadFile.MoveFileToArchives(files);
                }
            }
            catch(Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                Loading_Screen.CloseForm(); 
            }
        }
        //แสดง Pop up error ไว้หลังสุด
        public void ShowError()
        {
             if (fileNamesError != null && fileNamesError.Count > 0)
             {
                 DataTable dtResult = new DataTable();
                 dtResult = logBll.GetLogError("New"); 

                 ErrorForm dSummary = new ErrorForm(UserName);
                 dSummary.SummaryDownloadMaster(dtResult, modeFlg,false);
                 dSummary.ShowDialog();

                 Loading_Screen.CloseForm(); 
             }
             else
             {
                //โชว์ข้อมูลในตาราง
                GetMasterData();

                //เช็คที่ upload ไปมี serial ไหม ถ้ามีให้ show popup
                if (rbSku.Checked)
                {
                     DataTable countResult = downloadMasterBLL.GetSerialAfterDownload();
                     if (countResult.Rows.Count > 0)
                     {
                         Loading_Screen.CloseForm(); 
                         MessageBox.Show("This Stocktaking require Serial Number, please upload master data to HHTApp."
                                         , "Download Completed"
                                         , MessageBoxButtons.OK
                                         , MessageBoxIcon.Exclamation);
                     }
                     else
                     {
                         Loading_Screen.CloseForm(); 
                         MessageBox.Show("Download Completed"
                                         , "Download Completed"
                                         , MessageBoxButtons.OK
                                         , MessageBoxIcon.Information);
                     }
                }
                else
                {
                    Loading_Screen.CloseForm(); 
                    MessageBox.Show("Download Completed"
                                    , "Download Completed"
                                    , MessageBoxButtons.OK
                                    , MessageBoxIcon.Information);
                }
            }
        }

        public void SendAcknowledge()
        {
            UploadAcknowledgeFile ack = new UploadAcknowledgeFile();
            string result = "";

            ack = new UploadAcknowledgeFile(dataType,UserName);

            //จัดการ ACK File All
            object oResult = ack.SaveAcknowledge(fileNamesFull);
            result = ack.GenerateFile(oResult);

            if (result != null)
            {
                if (result.Substring(0, 2) == "OK")
                {
                    result = ack.UploadToSFTP();
                    if (result.Substring(0, 2) != "OK")
                    {
                        fileNamesError.Add(new Tuple<string, string>("", result));
                        logBll.LogError(UserName, MethodBase.GetCurrentMethod().Name, "SendAcknowledge" , result, DateTime.Now);
                    }
                }
                else
                {
                    fileNamesError.Add(new Tuple<string, string>("", result));
                    logBll.LogError(UserName, MethodBase.GetCurrentMethod().Name, "", result, DateTime.Now);
                }
            }
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

        private void btnViewLog_Click(object sender, EventArgs e)
        {
            try
            {
                Loading_Screen.ShowSplashScreen();
                DataTable searchResult = new DataTable();
                searchResult = logBll.GetLogError("All");

                if (searchResult.Rows.Count > 0)
                {
                    ErrorForm dSummary = new ErrorForm(UserName); // Instantiate a Form3 object.
                    dSummary.SummaryDownloadMaster(searchResult, modeFlg,false);
                    Loading_Screen.CloseForm();
                    dSummary.ShowDialog();
                }
                else
                {
                    Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.NoDatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Loading_Screen.CloseForm();
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }
    }
}
