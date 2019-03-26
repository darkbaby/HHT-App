using CrystalDecisions.Shared;
using FSBT.HHT.App.Resources;
using FSBT_HHT_BLL;
using FSBT_HHT_DAL;
using FSBT_HHT_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSBT.HHT.App.UI
{
    public partial class EditQuantityForm : Form
    {
        private LogErrorBll logBll = new LogErrorBll();
        public string UserName { get; set; }
        AuditManagementBll auditBLL = new AuditManagementBll();
        private SystemSettingBll settingBLL = new SystemSettingBll();
        private LocationManagementBll bll = new LocationManagementBll();
        private ReportManagementBll bllReportManagement = new ReportManagementBll();
        private List<EditQtyModel.Response> gridViewDataBeforeEdit = new List<EditQtyModel.Response>();
        private List<EditQtyModel.Response> deleteList = new List<EditQtyModel.Response>();
        private List<EditQtyModel.Response> updateList = new List<EditQtyModel.Response>();
        private List<EditQtyModel.Response> updateSKUCodeList = new List<EditQtyModel.Response>();
        private List<EditQtyModel.Response> insertList = new List<EditQtyModel.Response>();
        private DateTime countDate = new DateTime();
        private string branchName = string.Empty;
        private string ComName = string.Empty;
        List<EditQtyModel.Response> listResult = new List<EditQtyModel.Response>();
        private List<EditQtyModel.MasterUnit> listUnit = new List<EditQtyModel.MasterUnit>();
        private int unitBefore = new int();
        string counterBefore = "";
        string serialBefore = "";
        int unitPackValue = 0;
        int scanMode = 1;
        string compName = "";
        //private string Plant = "";

        public EditQuantityForm(string username)
        {
            InitializeComponent();
            UserName = username;
        }

        private void EditQTYForm_Load(object sender, EventArgs e)
        {
            try
            {
                var settingData = settingBLL.GetSettingData();
                auditBLL.GetMasterForMappingData();
                AddDropDownPlant();
                //AddDropDownMCHLevel();
                AddDropDownCountSheet();
                AddDropDownStorageLocation();

                AddDropDownMCHLevel1();
                AddDropDownMCHLevel2();
                AddDropDownMCHLevel3();
                AddDropDownMCHLevel4();

                countDate = settingData.CountDate;
                ComName = settingData.ComID + "|" + settingData.ComName;
                //branchName = settingData.Branch;
                //Plant = settingBLL.GetSettingStringByKey("Plant");

                //dataGridView1.Rows[0].Cells["No"].ReadOnly = true;
                //dataGridView1.Rows[0].Cells["Plant"].ReadOnly = true;
                //dataGridView1.Rows[0].Cells["StorageLocation"].ReadOnly = true;
                //dataGridView1.Rows[0].Cells["SKUCode"].ReadOnly = true;
                //dataGridView1.Rows[0].Cells["NewQuantity"].ReadOnly = true;
                //dataGridView1.Rows[0].Cells["Flag"].ReadOnly = true;
                //dataGridView1.Rows[0].Cells["Description"].ReadOnly = true;

                var listScanMode = auditBLL.GetMasterScanMode();
                listUnit = auditBLL.GetMasterUnit();

                compName = settingBLL.GetComputerName();
                foreach (var l in listUnit)
                {
                    if (l.UnitName == "PCK")
                    {
                        unitPackValue = l.UnitCode;
                        break;
                    }
                }

                #region AddColumnUnit Index6
                DataGridViewComboBoxColumn ColumnUnit = new DataGridViewComboBoxColumn();
                ColumnUnit.DataPropertyName = "UnitCode";
                ColumnUnit.Name = "UnitCode";
                ColumnUnit.HeaderText = "Unit";
                ColumnUnit.Width = 120;
                ColumnUnit.DataSource = listUnit;
                ColumnUnit.ValueMember = "UnitCode";
                ColumnUnit.DisplayMember = "UnitName";
                dataGridView1.Columns.Insert(7, ColumnUnit);
                (dataGridView1.Rows[0].Cells["UnitCode"] as DataGridViewComboBoxCell).Value = 1;
                #endregion

                dataGridView1.Rows[0].Cells["ComputerName"].Value = compName;

                //dataGridView1.Rows[0].Cells["Plant"].Style.BackColor = Color.LightGray;
                //dataGridView1.Rows[0].Cells["StorageLocation"].Style.BackColor = Color.LightGray;
                //dataGridView1.Rows[0].Cells["ComputerName"].Style.BackColor = Color.LightGray;

                //dataGridView1.Rows[0].Cells["SKUCode"].Style.BackColor = Color.LightGray;
                //dataGridView1.Rows[0].Cells["NewQuantity"].Style.BackColor = Color.LightGray;
                //dataGridView1.Rows[0].Cells["Description"].Style.BackColor = Color.LightGray;
                //dataGridView1.Rows[0].Cells["Remark"].Style.BackColor = Color.LightGray;
                SetReadOnlyAndStyle(dataGridView1.Rows[0]);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateSearch())
                {
                    listResult.Clear();
                    insertList.Clear();
                    updateList.Clear();
                    updateSKUCodeList.Clear();
                    deleteList.Clear();
                    this.dataGridView1.Rows.Clear();
                    EditQtyModel.Request searchCondition = GetSearchCondition();
                    Loading_Screen.ShowSplashScreen();
                    listResult = auditBLL.GetAuditHHTToPC(searchCondition);
                    if (listResult.Count == 0)
                    {
                        dataGridView1.Rows[0].Cells["No"].Value = 1;
                        (dataGridView1.Rows[0].Cells["UnitCode"] as DataGridViewComboBoxCell).Value = 1;
                        dataGridView1.Rows[0].Cells["ComputerName"].Value = compName;

                        dataGridView1.Rows[0].Cells["Plant"].Style.BackColor = Color.LightGray;
                        dataGridView1.Rows[0].Cells["StorageLocation"].Style.BackColor = Color.LightGray;
                        dataGridView1.Rows[0].Cells["SKUCode"].Style.BackColor = Color.LightGray;
                        dataGridView1.Rows[0].Cells["NewQuantity"].Style.BackColor = Color.LightGray;
                        dataGridView1.Rows[0].Cells["Description"].Style.BackColor = Color.LightGray;
                        dataGridView1.Rows[0].Cells["ComputerName"].Style.BackColor = Color.LightGray;

                        dataGridView1.Rows[0].Cells["Plant"].ReadOnly = true;
                        dataGridView1.Rows[0].Cells["StorageLocation"].ReadOnly = true;
                        dataGridView1.Rows[0].Cells["SKUCode"].ReadOnly = true;
                        dataGridView1.Rows[0].Cells["NewQuantity"].ReadOnly = true;
                        dataGridView1.Rows[0].Cells["Flag"].ReadOnly = true;
                        dataGridView1.Rows[0].Cells["ComputerName"].ReadOnly = true;
                        dataGridView1.Rows[0].Cells["Description"].ReadOnly = true;

                        dataGridView1.Rows[0].Cells["LocationCode"].ReadOnly = false;
                        dataGridView1.Rows[0].Cells["BarCode"].ReadOnly = false;
                        dataGridView1.Rows[0].Cells["SerialNumber"].ReadOnly = false;
                        dataGridView1.Rows[0].Cells["Quantity"].ReadOnly = false;

                        dataGridView1.Rows[0].Cells["LocationCode"].Style.BackColor = Color.White;
                        dataGridView1.Rows[0].Cells["BarCode"].Style.BackColor = Color.White;
                        dataGridView1.Rows[0].Cells["SerialNumber"].Style.BackColor = Color.White;
                        dataGridView1.Rows[0].Cells["Quantity"].Style.BackColor = Color.White;
                        Loading_Screen.CloseForm();
                        MessageBox.Show(MessageConstants.NoAuditdatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);




                    }
                    else
                    {
                        int i = 0;
                        int runningNo = 1;
                        foreach (EditQtyModel.Response result in listResult)
                        {
                            result.UnitCode = result.UnitCode == 4 ? 5 : result.UnitCode;
                            this.dataGridView1.Rows.Add(result.No
                                                        , result.Plant
                                                        , result.StorageLocation
                                                        , result.LocationCode
                                                        , result.Barcode
                                                        , result.SKUCode
                                                        , result.Description
                                                        , result.UnitCode
                                                        , result.Quantity
                                                        , result.NewQuantity
                                                        , result.ConversionCounter
                                                        , result.SerialNumber
                                                        , result.Flag
                                                        , result.ComputerName
                                                        , result.Remark
                                                        , result.StocktakingID
                                                        , result.InBarCode
                                                        , result.ExBarCode
                                                        , result.DepartmentCode
                                                        , result.MKCode
                                                        , result.ProductType
                                                        , result.SKUMode
                                                        , result.ChkSKUCode
                                                        , result.ScanMode
                                                        , result.SerialNumber
                                                        , result.UnitCode
                                                        , result.ConversionCounter);

                            dataGridView1.Rows[i].Cells["No"].Value = runningNo;
                            dataGridView1.Rows[i].Cells["No"].ReadOnly = true;
                            //(dataGridView1.Rows[i].Cells["ScanMode_"] as DataGridViewComboBoxCell).Value = result.ScanMode;

                            //(dataGridView1.Rows[i].Cells["Plant"] as DataGridViewComboBoxCell).Value = result.Plant;
                            //(dataGridView1.Rows[i].Cells["StorageLocation"] as DataGridViewComboBoxCell).Value = result.StorageLocation;

                            (dataGridView1.Rows[i].Cells["UnitCode"] as DataGridViewComboBoxCell).Value = result.UnitCode;

                            //var quantity = dataGridView1.Rows[i].Cells["Quantity"].Value == null ? 0 : (decimal)dataGridView1.Rows[i].Cells["Quantity"].Value;
                            //var newQuantity = dataGridView1.Rows[i].Cells["NewQuantity"].Value == null ? 0 : (decimal)dataGridView1.Rows[i].Cells["NewQuantity"].Value;

                            var quantity = result.Quantity;
                            var newQuantity = result.NewQuantity;

                            bool isIntQty = quantity == (int)quantity;
                            bool isIntNewQty = newQuantity == (int)newQuantity;



                            if (quantity.ToString() == "0.00000000")
                            {
                                dataGridView1.Rows[i].Cells["Quantity"].Value = null;
                            }
                            else
                            {
                                if (/*(result.ScanMode == 5 || result.ScanMode == 6 ) && */ result.UnitCode == 5)
                                {
                                    dataGridView1.Rows[i].Cells["Quantity"].Value = String.Format("{0:#,0.000}", (decimal)quantity);
                                }
                                else
                                {
                                    if (isIntQty)
                                    {
                                        dataGridView1.Rows[i].Cells["Quantity"].Value = String.Format("{0:#,0}", (decimal)quantity);
                                    }
                                    else
                                    {
                                        dataGridView1.Rows[i].Cells["Quantity"].Value = String.Format("{0:#,0.000}", (decimal)quantity);
                                    }
                                }
                            }

                            if (newQuantity.ToString() == "0.00000000")
                            {
                                dataGridView1.Rows[i].Cells["NewQuantity"].Value = null;



                            }
                            else
                            {
                                if (/*(result.ScanMode == 5 || result.ScanMode == 6) && */result.UnitCode == 5)
                                {
                                    dataGridView1.Rows[i].Cells["NewQuantity"].Value = String.Format("{0:#,0.000}", (decimal)newQuantity);
                                }
                                else
                                {
                                    if (isIntNewQty)
                                    {
                                        dataGridView1.Rows[i].Cells["NewQuantity"].Value = String.Format("{0:#,0}", (decimal)newQuantity);
                                    }
                                    else
                                    {
                                        dataGridView1.Rows[i].Cells["NewQuantity"].Value = String.Format("{0:#,0.000}", (decimal)newQuantity);
                                    }
                                }
                            }
                            SetReadOnlyAndStyle(dataGridView1.Rows[i]);
                            i++;
                            runningNo++;
                        }

                        var lastDataNo = listResult.Count();
                        var rowCount = dataGridView1.Rows.Count - 1;


                        //SetReadOnlyAndStyle(dataGridView1.Rows[rowCount]);

                        dataGridView1.Rows[rowCount].Cells["No"].Value = lastDataNo + 1;
                        (dataGridView1.Rows[rowCount].Cells["UnitCode"] as DataGridViewComboBoxCell).Value = 1;
                        dataGridView1.Rows[rowCount].Cells["ComputerName"].Value = compName;

                        dataGridView1.Rows[rowCount].Cells["Plant"].Style.BackColor = Color.LightGray;
                        dataGridView1.Rows[rowCount].Cells["StorageLocation"].Style.BackColor = Color.LightGray;
                        dataGridView1.Rows[rowCount].Cells["SKUCode"].Style.BackColor = Color.LightGray;
                        dataGridView1.Rows[rowCount].Cells["NewQuantity"].Style.BackColor = Color.LightGray;
                        dataGridView1.Rows[rowCount].Cells["Description"].Style.BackColor = Color.LightGray;
                        dataGridView1.Rows[rowCount].Cells["ComputerName"].Style.BackColor = Color.LightGray;

                        dataGridView1.Rows[rowCount].Cells["Plant"].ReadOnly = true;
                        dataGridView1.Rows[rowCount].Cells["StorageLocation"].ReadOnly = true;
                        dataGridView1.Rows[rowCount].Cells["SKUCode"].ReadOnly = true;
                        dataGridView1.Rows[rowCount].Cells["NewQuantity"].ReadOnly = true;
                        dataGridView1.Rows[rowCount].Cells["Flag"].ReadOnly = true;
                        dataGridView1.Rows[rowCount].Cells["ComputerName"].ReadOnly = true;
                        dataGridView1.Rows[rowCount].Cells["Description"].ReadOnly = true;


                        dataGridView1.Rows[rowCount].Cells["LocationCode"].ReadOnly = false;
                        dataGridView1.Rows[rowCount].Cells["BarCode"].ReadOnly = false;
                        dataGridView1.Rows[rowCount].Cells["SerialNumber"].ReadOnly = false;
                        dataGridView1.Rows[rowCount].Cells["Quantity"].ReadOnly = false;

                        dataGridView1.Rows[rowCount].Cells["LocationCode"].Style.BackColor = Color.White;
                        dataGridView1.Rows[rowCount].Cells["BarCode"].Style.BackColor = Color.White;
                        dataGridView1.Rows[rowCount].Cells["SerialNumber"].Style.BackColor = Color.White;
                        dataGridView1.Rows[rowCount].Cells["Quantity"].Style.BackColor = Color.White;

                        Loading_Screen.CloseForm();


                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private bool ValidateSearch()
        {
            try
            {
                string locationForm = textBoxLoForm.Text.Trim();
                string locationTo = textBoxLoTo.Text.Trim();

                int fromNum = 0;
                int toNum = 0;

                if (locationForm != string.Empty)
                {
                    bool res2 = int.TryParse(locationForm, out fromNum);
                    if (res2 == false)
                    {
                        textBoxLoForm.Text = "";
                        MessageBox.Show(MessageConstants.LocationFromisnumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    if (locationForm.Length == 1)
                    {
                        textBoxLoForm.Text = "0000" + locationForm;
                    }
                    else if (locationForm.Length == 2)
                    {
                        textBoxLoForm.Text = "000" + locationForm;
                    }
                    else if (locationForm.Length == 3)
                    {
                        textBoxLoForm.Text = "00" + locationForm;
                    }
                    else if (locationForm.Length == 4)
                    {
                        textBoxLoForm.Text = "0" + locationForm;
                    }
                }

                if (locationTo != string.Empty)
                {
                    bool res3 = int.TryParse(locationTo, out toNum);
                    if (res3 == false)
                    {
                        textBoxLoTo.Text = "";
                        MessageBox.Show(MessageConstants.LocationToisnumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    if (locationTo.Length == 1)
                    {
                        textBoxLoTo.Text = "0000" + locationTo;
                    }
                    else if (locationTo.Length == 2)
                    {
                        textBoxLoTo.Text = "000" + locationTo;
                    }
                    else if (locationTo.Length == 3)
                    {
                        textBoxLoTo.Text = "00" + locationTo;
                    }
                    else if (locationTo.Length == 4)
                    {
                        textBoxLoTo.Text = "0" + locationTo;
                    }
                }

                if (toNum < fromNum && locationTo != string.Empty && locationForm != string.Empty)
                {
                    MessageBox.Show(MessageConstants.LocationFormmustequealorlessthanLocationTo, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return true;
            }
        }



        private bool IsConversionCounterNull()
        {

            bool result = false;

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {


                EditQtyModel.Response DataDto = new EditQtyModel.Response();
                string Flag = (string)dataGridView1.Rows[i].Cells["Flag"].Value;
                DataDto.Flag = (string)dataGridView1.Rows[i].Cells["Flag"].Value;
                DataDto.Plant = (string)dataGridView1.Rows[i].Cells["Plant"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["Plant"].Value).Trim();
                DataDto.StorageLocation = (string)dataGridView1.Rows[i].Cells["StorageLocation"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["StorageLocation"].Value).Trim();
                DataDto.LocationCode = (string)dataGridView1.Rows[i].Cells["LocationCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["LocationCode"].Value).Trim();
                DataDto.Barcode = (string)dataGridView1.Rows[i].Cells["Barcode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["BarCode"].Value).Trim();
                DataDto.SKUCode = dataGridView1.Rows[i].Cells["SKUCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["SKUCode"].Value).Trim();
                DataDto.SerialNumber = dataGridView1.Rows[i].Cells["SerialNumber"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["SerialNumber"].Value).Trim();
                DataDto.UnitCode = dataGridView1.Rows[i].Cells["UnitCode"].Value == null ? 0 : ((int)dataGridView1.Rows[i].Cells["UnitCode"].Value);
                DataDto.Quantity = dataGridView1.Rows[i].Cells["Quantity"].Value == null ? 0 : Convert.ToDecimal(dataGridView1.Rows[i].Cells["Quantity"].Value);
                DataDto.NewQuantity = dataGridView1.Rows[i].Cells["NewQuantity"].Value == null ? 0 : Convert.ToDecimal(dataGridView1.Rows[i].Cells["NewQuantity"].Value);
                DataDto.ConversionCounter = dataGridView1.Rows[i].Cells["ConversionCounter"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ConversionCounter"].Value).Trim();
                DataDto.ComputerName = dataGridView1.Rows[i].Cells["ComputerName"].Value == null ? compName : ((string)dataGridView1.Rows[i].Cells["ComputerName"].Value).Trim();
                DataDto.DepartmentCode = dataGridView1.Rows[i].Cells["DepartmentCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["DepartmentCode"].Value).Trim();
                DataDto.Description = dataGridView1.Rows[i].Cells["Description"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["Description"].Value).Trim();
                DataDto.Remark = (string)dataGridView1.Rows[i].Cells["Remark"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["Remark"].Value).Trim();
                DataDto.StocktakingID = (string)dataGridView1.Rows[i].Cells["StocktakingID"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["StocktakingID"].Value).Trim();
                DataDto.InBarCode = dataGridView1.Rows[i].Cells["InBarCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["InBarCode"].Value).Trim();
                DataDto.ExBarCode = dataGridView1.Rows[i].Cells["ExBarCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ExBarCode"].Value).Trim();
                DataDto.DepartmentCode = dataGridView1.Rows[i].Cells["DepartmentCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["DepartmentCode"].Value).Trim();
                DataDto.MKCode = dataGridView1.Rows[i].Cells["MKCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["MKCode"].Value).Trim();
                DataDto.ProductType = dataGridView1.Rows[i].Cells["ProductType"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ProductType"].Value).Trim();
                DataDto.SKUMode = dataGridView1.Rows[i].Cells["SKUMode"].Value == null ? false : Convert.ToBoolean(dataGridView1.Rows[i].Cells["SKUMode"].Value);
                DataDto.ChkSKUCode = dataGridView1.Rows[i].Cells["ChkSKUCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ChkSKUCode"].Value).Trim();
                DataDto.ScanMode = dataGridView1.Rows[i].Cells["ScanMode_"].Value == null ? 0 : ((int)dataGridView1.Rows[i].Cells["ScanMode_"].Value);
                DataDto.oldSerialNumber = dataGridView1.Rows[i].Cells["oldSerialNumber"].Value == null ? "" : dataGridView1.Rows[i].Cells["oldSerialNumber"].Value.ToString();


                if (string.IsNullOrEmpty(DataDto.ConversionCounter) && (DataDto.UnitCode == unitPackValue))
                {
                    result = true;

                    dataGridView1.Rows[i].Cells["No"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].Cells["Plant"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].Cells["StorageLocation"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].Cells["LocationCode"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].Cells["Barcode"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].Cells["SKUCode"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].Cells["Quantity"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].Cells["NewQuantity"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].Cells["ConversionCounter"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].Cells["ComputerName"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].Cells["UnitCode"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].Cells["FLAG"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].Cells["Description"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].Cells["Remark"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].Cells["SerialNumber"].Style.BackColor = Color.Red;
                }
            }
            return result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsConversionCounterNull())
                {
                    MessageBox.Show(MessageConstants.ConversionCounterCannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    GetGridViewAuditList();

                    bool includeNull = listResult.Any(x => x.LocationCode == string.Empty || x.Barcode == string.Empty || x.Quantity == 0);// || x.Description == string.Empty);//|| x.BrandCode == string.Empty);

                    bool flagNIncludeNull = listResult.Any(x => x.SKUCode == string.Empty && x.Flag == "N");

                    if (includeNull)
                    {

                        MessageBox.Show(MessageConstants.LocationBarcodeQuantityDescriptionCannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (flagNIncludeNull)
                    {
                        MessageBox.Show(MessageConstants.NewRecordSKUCodeCannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (deleteList.Count > 0 || updateList.Count > 0 || updateSKUCodeList.Count > 0 || insertList.Count > 0)
                        {
                            Save(insertList, updateList, updateSKUCodeList, deleteList);
                            btnSearch.PerformClick();
                            //SCR02_SP_Update_MasterForNewRecord
                        }
                        else
                        {
                            MessageBox.Show(MessageConstants.Nochangeddata, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void btnViewDeleteReport_Click(object sender, EventArgs e)
        {
            try
            {
                EditQtyModel.Request searchCondition = GetSearchCondition();
                DataTable listDeleteData = auditBLL.GetAuditHHTToPCDelete(searchCondition);
                DeleteQtyReportForm deleteQtyReport = new DeleteQtyReportForm();

                if (listDeleteData.Rows.Count == 0)
                {
                    MessageBox.Show(MessageConstants.Nodatadeleterecordreport, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Loading_Screen.ShowSplashScreen();
                    bool isCreateReportSuccess = deleteQtyReport.CreateReport(listDeleteData, countDate, branchName);
                    Loading_Screen.CloseForm();
                    if (isCreateReportSuccess)
                    {
                        deleteQtyReport.StartPosition = FormStartPosition.CenterParent;
                        DialogResult dialogResult = deleteQtyReport.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.cannotgeneratereport, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void txtLocationForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtLocationTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void GetGridViewAuditList()
        {
            try
            {
                listResult.Clear();
                insertList.Clear();
                updateList.Clear();
                updateSKUCodeList.Clear();

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    EditQtyModel.Response DataDto = new EditQtyModel.Response();
                    string Flag = (string)dataGridView1.Rows[i].Cells["Flag"].Value;
                    // DataDto.Flag = (string)dataGridView1.Rows[i].Cells["Flag"].Value;
                    DataDto.Plant = (string)dataGridView1.Rows[i].Cells["Plant"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["Plant"].Value).Trim();
                    DataDto.StorageLocation = (string)dataGridView1.Rows[i].Cells["StorageLocation"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["StorageLocation"].Value).Trim();
                    DataDto.LocationCode = (string)dataGridView1.Rows[i].Cells["LocationCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["LocationCode"].Value).Trim();
                    DataDto.Barcode = (string)dataGridView1.Rows[i].Cells["Barcode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["BarCode"].Value).Trim();
                    DataDto.SKUCode = dataGridView1.Rows[i].Cells["SKUCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["SKUCode"].Value).Trim();
                    DataDto.SerialNumber = dataGridView1.Rows[i].Cells["SerialNumber"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["SerialNumber"].Value).Trim();
                    DataDto.UnitCode = dataGridView1.Rows[i].Cells["UnitCode"].Value == null ? 0 : ((int)dataGridView1.Rows[i].Cells["UnitCode"].Value);
                    DataDto.Quantity = dataGridView1.Rows[i].Cells["Quantity"].Value == null ? 0 : Convert.ToDecimal(dataGridView1.Rows[i].Cells["Quantity"].Value);
                    DataDto.NewQuantity = dataGridView1.Rows[i].Cells["NewQuantity"].Value == null ? 0 : Convert.ToDecimal(dataGridView1.Rows[i].Cells["NewQuantity"].Value);

                    DataDto.ConversionCounter = dataGridView1.Rows[i].Cells["ConversionCounter"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ConversionCounter"].Value).Trim();
                    DataDto.ComputerName = dataGridView1.Rows[i].Cells["ComputerName"].Value == null ? compName : ((string)dataGridView1.Rows[i].Cells["ComputerName"].Value).Trim();
                    DataDto.DepartmentCode = dataGridView1.Rows[i].Cells["DepartmentCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["DepartmentCode"].Value).Trim();
                    DataDto.Description = dataGridView1.Rows[i].Cells["Description"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["Description"].Value).Trim();
                    DataDto.Remark = (string)dataGridView1.Rows[i].Cells["Remark"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["Remark"].Value).Trim();
                    DataDto.StocktakingID = (string)dataGridView1.Rows[i].Cells["StocktakingID"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["StocktakingID"].Value).Trim();
                    DataDto.InBarCode = dataGridView1.Rows[i].Cells["InBarCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["InBarCode"].Value).Trim();
                    DataDto.ExBarCode = dataGridView1.Rows[i].Cells["ExBarCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ExBarCode"].Value).Trim();
                    DataDto.DepartmentCode = dataGridView1.Rows[i].Cells["DepartmentCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["DepartmentCode"].Value).Trim();
                    DataDto.MKCode = dataGridView1.Rows[i].Cells["MKCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["MKCode"].Value).Trim();
                    DataDto.ProductType = dataGridView1.Rows[i].Cells["ProductType"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ProductType"].Value).Trim();
                    DataDto.SKUMode = dataGridView1.Rows[i].Cells["SKUMode"].Value == null ? false : Convert.ToBoolean(dataGridView1.Rows[i].Cells["SKUMode"].Value);
                    DataDto.ChkSKUCode = dataGridView1.Rows[i].Cells["ChkSKUCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ChkSKUCode"].Value).Trim();
                    DataDto.ScanMode = dataGridView1.Rows[i].Cells["ScanMode_"].Value == null ? 0 : ((int)dataGridView1.Rows[i].Cells["ScanMode_"].Value);

                    DataDto.oldSerialNumber = dataGridView1.Rows[i].Cells["oldSerialNumber"].Value == null ? "" : dataGridView1.Rows[i].Cells["oldSerialNumber"].Value.ToString();
                    DataDto.oldUnit = dataGridView1.Rows[i].Cells["oldUnit"].Value == null ? 0 : (int)dataGridView1.Rows[i].Cells["oldUnit"].Value;
                    DataDto.oldConversionCounter = dataGridView1.Rows[i].Cells["oldConversionCounter"].Value == null ? "" : dataGridView1.Rows[i].Cells["oldConversionCounter"].Value.ToString();

                    if (DataDto.Quantity != 0)
                    {
                        DataDto.Quantity = DataDto.UnitCode == 5 ? DataDto.Quantity /* * 1000*/ : DataDto.Quantity;
                    }
                    if (DataDto.NewQuantity != 0)
                    {
                        DataDto.NewQuantity = DataDto.UnitCode == 5 ? DataDto.NewQuantity /* * 1000 */: DataDto.NewQuantity;
                    }


                    //DataDto.UnitCode = DataDto.UnitCode == 5 ? 4 : DataDto.UnitCode;

                    //}
                    //if (DataDto.ScanMode == 4 && DataDto.UnitCode != 4)
                    //{
                    //    var qty = DataDto.Quantity == null ? 0 : Convert.ToDecimal(DataDto.Quantity);
                    //    var newqty = DataDto.NewQuantity == null ? 0 : Convert.ToDecimal(DataDto.NewQuantity);
                    //    if (qty != 0)
                    //    {
                    //        DataDto.Quantity = Math.Ceiling(qty);
                    //    }
                    //    if (newqty != 0)
                    //    {
                    //        DataDto.NewQuantity = Math.Ceiling(newqty);
                    //    }
                    //}

                    bool update = false;
                    if (DataDto.UnitCode != DataDto.oldUnit || DataDto.ConversionCounter != DataDto.oldConversionCounter)
                        update = true;

                    if (DataDto.StocktakingID == string.Empty && !string.IsNullOrEmpty(DataDto.SKUCode))//New
                    {
                        DataDto.Flag = "N";
                        DataDto.HHTName = ComName;
                        insertList.Add(DataDto);
                    }
                    else if (DataDto.SKUMode == true)
                    {
                        if ((Flag == "I" && DataDto.NewQuantity != 0))
                        {
                            DataDto.Flag = "E";
                            updateList.Add(DataDto);
                        }

                        // --- ADD column Old Serial Number and Check Old <> Current -> Update !!!!!
                        else if (Flag == "I" && DataDto.SerialNumber != DataDto.oldSerialNumber)
                        {
                            DataDto.Flag = "E";
                            updateList.Add(DataDto);
                        }
                        else if (Flag == "I" && update)
                        {
                            DataDto.Flag = "E";
                            updateList.Add(DataDto);
                        }
                        else if (Flag == "E" && (DataDto.NewQuantity != 0 || DataDto.SerialNumber != null || update))
                        {
                            DataDto.Flag = "E";
                            updateList.Add(DataDto);
                        }
                        else if (Flag == "F" && (DataDto.NewQuantity != 0 || DataDto.Description != string.Empty || update))
                        {
                            DataDto.Flag = "U";
                            updateList.Add(DataDto);
                        }
                        else if (Flag == "U" && (DataDto.NewQuantity != 0 || DataDto.Description != string.Empty || update))
                        {
                            DataDto.Flag = "U";
                            updateList.Add(DataDto);
                        }
                        else if (Flag == "N" && DataDto.Quantity != 0 && !string.IsNullOrEmpty(DataDto.SKUCode))
                        {
                            DataDto.Flag = "N";
                            updateList.Add(DataDto);
                        }
                        else if (Flag == "N" && update && !string.IsNullOrEmpty(DataDto.SKUCode))
                        {
                            DataDto.Flag = "N";
                            updateList.Add(DataDto);
                        }
                    }
                    else if (DataDto.SKUMode == false) // flag = F or U
                    {
                        if (DataDto.SKUCode == string.Empty)
                        {
                            if (Flag == "N")
                            {
                                DataDto.Flag = "N";
                            }
                            else if (DataDto.NewQuantity == 0 && DataDto.Description == string.Empty)
                            {
                                DataDto.Flag = "F";
                            }
                            else
                            {
                                DataDto.Flag = "U";
                                updateList.Add(DataDto);
                            }
                        }
                        else
                        {
                            //if (DataDto.NewQuantity == 0 && DataDto.Description != string.Empty)
                            //{
                            //    DataDto.Flag = "I";
                            //}
                            //else
                            //{
                            //    DataDto.Flag = "E";
                            //}
                            DataDto.Flag = "E";
                            updateList.Add(DataDto);
                        }
                    }

                    listResult.Add(DataDto);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        protected void AddDropDownPlant()
        {
            try
            {
                //Get DropDown
                List<string> listPlant = new List<string>();
                listPlant = settingBLL.GetAllPlant();

                comboBoxPlant.Items.Clear();

                if (listPlant.Count > 0)
                {
                    comboBoxPlant.Items.Add("All");

                    foreach (var l in listPlant)
                    {
                        comboBoxPlant.Items.Add(l);
                    }

                    comboBoxPlant.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void SetReadOnlyAndStyle(DataGridViewRow dataGridView1)
        {

            try
            {

                if ((string)dataGridView1.Cells["Flag"].Value == "I" || (string)dataGridView1.Cells["Flag"].Value == "E")
                {
                    dataGridView1.Cells["Plant"].ReadOnly = true;
                    dataGridView1.Cells["StorageLocation"].ReadOnly = true;
                    dataGridView1.Cells["LocationCode"].ReadOnly = true;
                    dataGridView1.Cells["BarCode"].ReadOnly = true;
                    dataGridView1.Cells["SKUCode"].ReadOnly = true;
                    dataGridView1.Cells["Quantity"].ReadOnly = true;


                    dataGridView1.Cells["ComputerName"].ReadOnly = true;
                    dataGridView1.Cells["Description"].ReadOnly = true;


                    dataGridView1.Cells["Description"].ReadOnly = true;
                    dataGridView1.Cells["Plant"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["StorageLocation"].Style.BackColor = Color.LightGray;

                    dataGridView1.Cells["LocationCode"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["BarCode"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["SKUCode"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["Quantity"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["ComputerName"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["Description"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["No"].ReadOnly = true;
                    dataGridView1.Cells["Flag"].ReadOnly = true;
                    dataGridView1.Cells["No"].Style.BackColor = Color.White;
                    dataGridView1.Cells["Flag"].Style.BackColor = Color.White;
                    dataGridView1.Cells["UnitCode"].Style.BackColor = Color.White;
                    dataGridView1.Cells["SerialNumber"].ReadOnly = false;
                    //dataGridView1.Cells["NewQuantity"].ReadOnly = false;
                    dataGridView1.Cells["NewQuantity"].Style.BackColor = Color.White;
                    dataGridView1.Cells["SerialNumber"].Style.BackColor = Color.White;

                    if ((int)dataGridView1.Cells["UnitCode"].Value == unitPackValue)//PACK
                    {
                        dataGridView1.Cells["ConversionCounter"].Style.BackColor = Color.White;
                        dataGridView1.Cells["ConversionCounter"].ReadOnly = false;
                    }
                    else
                    {
                        dataGridView1.Cells["ConversionCounter"].Style.BackColor = Color.LightGray;
                        dataGridView1.Cells["ConversionCounter"].ReadOnly = true;
                    }

                    if (dataGridView1.Cells["SerialNumber"].Value == null || (string)dataGridView1.Cells["SerialNumber"].Value == string.Empty)
                    {
                        dataGridView1.Cells["NewQuantity"].ReadOnly = false;
                        dataGridView1.Cells["NewQuantity"].Style.BackColor = Color.White;
                    }
                    else
                    {
                        dataGridView1.Cells["NewQuantity"].ReadOnly = true;
                        dataGridView1.Cells["NewQuantity"].Style.BackColor = Color.LightGray;
                    }

                }
                else if ((string)dataGridView1.Cells["Flag"].Value == "F" || (string)dataGridView1.Cells["Flag"].Value == "U")
                {
                    dataGridView1.Cells["No"].ReadOnly = true;
                    dataGridView1.Cells["Flag"].ReadOnly = true;
                    dataGridView1.Cells["Plant"].ReadOnly = true;
                    dataGridView1.Cells["StorageLocation"].ReadOnly = true;
                    dataGridView1.Cells["LocationCode"].ReadOnly = true;
                    dataGridView1.Cells["BarCode"].ReadOnly = true;
                    dataGridView1.Cells["SKUCode"].ReadOnly = true;
                    dataGridView1.Cells["Quantity"].ReadOnly = true;
                    dataGridView1.Cells["ComputerName"].ReadOnly = true;

                    dataGridView1.Cells["Plant"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["StorageLocation"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["LocationCode"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["BarCode"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["SKUCode"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["Quantity"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["ComputerName"].Style.BackColor = Color.LightGray;

                    dataGridView1.Cells["No"].Style.BackColor = Color.White;
                    dataGridView1.Cells["Flag"].Style.BackColor = Color.White;
                    dataGridView1.Cells["UnitCode"].Style.BackColor = Color.White;

                    dataGridView1.Cells["SerialNumber"].ReadOnly = false;
                    dataGridView1.Cells["Description"].ReadOnly = false;
                    dataGridView1.Cells["UnitCode"].ReadOnly = false;

                    dataGridView1.Cells["SerialNumber"].Style.BackColor = Color.White;
                    dataGridView1.Cells["Description"].Style.BackColor = Color.White;
                    dataGridView1.Cells["UnitCode"].Style.BackColor = Color.White;

                    if ((int)dataGridView1.Cells["UnitCode"].Value == unitPackValue)//Pack
                    {
                        dataGridView1.Cells["ConversionCounter"].Style.BackColor = Color.White;
                        dataGridView1.Cells["ConversionCounter"].ReadOnly = false;
                    }
                    else
                    {
                        dataGridView1.Cells["ConversionCounter"].Style.BackColor = Color.LightGray;
                        dataGridView1.Cells["ConversionCounter"].ReadOnly = true;
                    }

                    if (dataGridView1.Cells["SerialNumber"].Value == null || (string)dataGridView1.Cells["SerialNumber"].Value == string.Empty)
                    {
                        dataGridView1.Cells["NewQuantity"].ReadOnly = false;
                        dataGridView1.Cells["NewQuantity"].Style.BackColor = Color.White;
                    }
                    else
                    {
                        dataGridView1.Cells["NewQuantity"].ReadOnly = true;
                        dataGridView1.Cells["NewQuantity"].Style.BackColor = Color.LightGray;
                    }
                }
                else//Flag=N
                {
                    //Read Only 
                    dataGridView1.Cells["Plant"].ReadOnly = true;
                    dataGridView1.Cells["StorageLocation"].ReadOnly = true;
                    dataGridView1.Cells["SKUCode"].ReadOnly = true;
                    dataGridView1.Cells["NewQuantity"].ReadOnly = true;
                    dataGridView1.Cells["Flag"].ReadOnly = true;
                    dataGridView1.Cells["ComputerName"].ReadOnly = true;
                    dataGridView1.Cells["Description"].ReadOnly = true;

                    dataGridView1.Cells["Plant"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["StorageLocation"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["SKUCode"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["NewQuantity"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["Description"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["ComputerName"].Style.BackColor = Color.LightGray;

                    //Can Edit
                    dataGridView1.Cells["LocationCode"].ReadOnly = false;
                    dataGridView1.Cells["BarCode"].ReadOnly = false;
                    dataGridView1.Cells["UnitCode"].ReadOnly = false;
                    dataGridView1.Cells["SerialNumber"].ReadOnly = false;

                    dataGridView1.Cells["LocationCode"].Style.BackColor = Color.White;
                    dataGridView1.Cells["BarCode"].Style.BackColor = Color.White;
                    dataGridView1.Cells["SerialNumber"].Style.BackColor = Color.White;

                    dataGridView1.Cells["No"].Style.BackColor = Color.White;
                    dataGridView1.Cells["Flag"].Style.BackColor = Color.White;
                    dataGridView1.Cells["UnitCode"].Style.BackColor = Color.White;

                    if ((int)dataGridView1.Cells["UnitCode"].Value == unitPackValue)//Pack
                    {
                        dataGridView1.Cells["ConversionCounter"].Style.BackColor = Color.White;
                        dataGridView1.Cells["ConversionCounter"].ReadOnly = false;
                    }
                    else
                    {
                        dataGridView1.Cells["ConversionCounter"].Style.BackColor = Color.LightGray;
                        dataGridView1.Cells["ConversionCounter"].ReadOnly = true;
                    }

                    if (dataGridView1.Cells["SerialNumber"].Value == null || (string)dataGridView1.Cells["SerialNumber"].Value == string.Empty)
                    {
                        dataGridView1.Cells["Quantity"].ReadOnly = false;
                        dataGridView1.Cells["Quantity"].Style.BackColor = Color.White;
                    }
                    else
                    {
                        dataGridView1.Cells["Quantity"].ReadOnly = true;
                        dataGridView1.Cells["Quantity"].Style.BackColor = Color.LightGray;
                    }
                }
                dataGridView1.Cells["Remark"].Style.BackColor = Color.LightGray;
            }

            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }

        }

        private void Save(List<EditQtyModel.Response> insertList, List<EditQtyModel.Response> updateList, List<EditQtyModel.Response> updateSKUModeList, List<EditQtyModel.Response> deleteList)
        {
            try
            {
                bool saveComplete = auditBLL.SaveAuditHHTToPC(insertList, updateList, updateSKUCodeList, deleteList, UserName);
                if (saveComplete)
                {
                    UpdateMasterForNewRecord();
                    //SCR02_SP_Update_MasterForNewRecord
                    MessageBox.Show(MessageConstants.Savecomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(MessageConstants.CannotsaveEditQtydatatodatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void UpdateMasterForNewRecord()
        {
            auditBLL.UpdateMasterForAddData();
        }


        private EditQtyModel.Request GetSearchCondition()
        {
            ////locationfrom
            if (textBoxLoForm.Text.Trim().Length == 1)
            {
                textBoxLoForm.Text = "0000" + textBoxLoForm.Text.Trim();
            }
            else if (textBoxLoForm.Text.Trim().Length == 2)
            {
                textBoxLoForm.Text = "000" + textBoxLoForm.Text.Trim();
            }
            else if (textBoxLoForm.Text.Trim().Length == 3)
            {
                textBoxLoForm.Text = "00" + textBoxLoForm.Text.Trim();
            }
            else if (textBoxLoForm.Text.Trim().Length == 4)
            {
                textBoxLoForm.Text = "0" + textBoxLoForm.Text.Trim();
            }
            ////locationto
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

            EditQtyModel.Request searchCondition = new EditQtyModel.Request();

            searchCondition.PlantCode = comboBoxPlant.SelectedItem.ToString();
            searchCondition.CountSheet = comboBoxCountSheet.SelectedItem.ToString();
            searchCondition.MCHLevel1 = comboBoxMCH1.SelectedItem.ToString();
            searchCondition.MCHLevel2 = comboBoxMCH2.SelectedItem.ToString();
            searchCondition.MCHLevel3 = comboBoxMCH3.SelectedItem.ToString();
            searchCondition.MCHLevel4 = comboBoxMCH4.SelectedItem.ToString();
            searchCondition.StorageLocationCode = comboBoxStorageLocation.SelectedItem.ToString();
            searchCondition.LocationFrom = textBoxLoForm.Text;
            searchCondition.LocationTo = textBoxLoTo.Text;
            searchCondition.Barcode = txtBarcode.Text;
            searchCondition.SKUCode = txtSKUCode.Text;
            return searchCondition;
        }

        #region Gridview
        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.Row.Index].Cells["No"].Value = e.Row.Index + 1;

                dataGridView1.Rows[e.Row.Index].Cells["Flag"].Value = "N";
                dataGridView1.Rows[e.Row.Index].Cells["SKUMode"].Value = true;
                dataGridView1.Rows[e.Row.Index].Cells["ComputerName"].Value = compName;
                (dataGridView1.Rows[e.Row.Index].Cells["UnitCode"] as DataGridViewComboBoxCell).Value = 1;

                //Read Only 
                dataGridView1.Rows[e.Row.Index].Cells["Plant"].ReadOnly = true;
                dataGridView1.Rows[e.Row.Index].Cells["StorageLocation"].ReadOnly = true;
                dataGridView1.Rows[e.Row.Index].Cells["SKUCode"].ReadOnly = true;
                dataGridView1.Rows[e.Row.Index].Cells["NewQuantity"].ReadOnly = true;
                dataGridView1.Rows[e.Row.Index].Cells["Flag"].ReadOnly = true;
                dataGridView1.Rows[e.Row.Index].Cells["ComputerName"].ReadOnly = true;
                dataGridView1.Rows[e.Row.Index].Cells["Description"].ReadOnly = true;




                dataGridView1.Rows[e.Row.Index].Cells["Plant"].Style.BackColor = Color.LightGray;
                dataGridView1.Rows[e.Row.Index].Cells["StorageLocation"].Style.BackColor = Color.LightGray;
                dataGridView1.Rows[e.Row.Index].Cells["SKUCode"].Style.BackColor = Color.LightGray;
                dataGridView1.Rows[e.Row.Index].Cells["NewQuantity"].Style.BackColor = Color.LightGray;

                dataGridView1.Rows[e.Row.Index].Cells["Description"].Style.BackColor = Color.LightGray;
                dataGridView1.Rows[e.Row.Index].Cells["ComputerName"].Style.BackColor = Color.LightGray;

                //Can Edit
                dataGridView1.Rows[e.Row.Index].Cells["LocationCode"].ReadOnly = false;
                dataGridView1.Rows[e.Row.Index].Cells["BarCode"].ReadOnly = false;
                dataGridView1.Rows[e.Row.Index].Cells["SerialNumber"].ReadOnly = false;
                dataGridView1.Rows[e.Row.Index].Cells["Quantity"].ReadOnly = false;

                dataGridView1.Rows[e.Row.Index].Cells["LocationCode"].Style.BackColor = Color.White;
                dataGridView1.Rows[e.Row.Index].Cells["BarCode"].Style.BackColor = Color.White;
                dataGridView1.Rows[e.Row.Index].Cells["SerialNumber"].Style.BackColor = Color.White;
                dataGridView1.Rows[e.Row.Index].Cells["Quantity"].Style.BackColor = Color.White;

                dataGridView1.Rows[e.Row.Index].Cells["No"].Style.BackColor = Color.White;
                dataGridView1.Rows[e.Row.Index].Cells["Flag"].Style.BackColor = Color.White;
                dataGridView1.Rows[e.Row.Index].Cells["UnitCode"].Style.BackColor = Color.White;

                if ((int)dataGridView1.Rows[e.Row.Index].Cells["UnitCode"].Value == unitPackValue)//Pack
                {
                    dataGridView1.Rows[e.Row.Index].Cells["ConversionCounter"].Style.BackColor = Color.White;
                    dataGridView1.Rows[e.Row.Index].Cells["ConversionCounter"].ReadOnly = false;
                }
                else
                {
                    dataGridView1.Rows[e.Row.Index].Cells["ConversionCounter"].Style.BackColor = Color.LightGray;
                    dataGridView1.Rows[e.Row.Index].Cells["ConversionCounter"].ReadOnly = true;
                }
                dataGridView1.Rows[e.Row.Index].Cells["Remark"].Style.BackColor = Color.LightGray;

                //SetReadOnlyAndStyle(dataGridView1.Rows[e.Row.Index]);

                //dataGridView1.Rows[e.Row.Index].Cells["No"].ReadOnly = true;
                //dataGridView1.Rows[e.Row.Index].Cells["Plant"].ReadOnly = true;
                //dataGridView1.Rows[e.Row.Index].Cells["StorageLocation"].ReadOnly = true;
                //dataGridView1.Rows[e.Row.Index].Cells["SKUCode"].ReadOnly = true;
                //dataGridView1.Rows[e.Row.Index].Cells["NewQuantity"].ReadOnly = true;
                //dataGridView1.Rows[e.Row.Index].Cells["ConversionCounter"].ReadOnly = true;
                //dataGridView1.Rows[e.Row.Index].Cells["Flag"].ReadOnly = true;
                //dataGridView1.Rows[e.Row.Index].Cells["Description"].ReadOnly = true;

                //dataGridView1.Rows[e.Row.Index].Cells["Plant"].Style.BackColor = Color.LightGray;
                //dataGridView1.Rows[e.Row.Index].Cells["StorageLocation"].Style.BackColor = Color.LightGray;
                //dataGridView1.Rows[e.Row.Index].Cells["SKUCode"].Style.BackColor = Color.LightGray;
                //dataGridView1.Rows[e.Row.Index].Cells["NewQuantity"].Style.BackColor = Color.LightGray;
                //dataGridView1.Rows[e.Row.Index].Cells["ConversionCounter"].Style.BackColor = Color.LightGray;
                //dataGridView1.Rows[e.Row.Index].Cells["Description"].Style.BackColor = Color.LightGray;
                //dataGridView1.Rows[e.Row.Index].Cells["Remark"].Style.BackColor = Color.LightGray;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("Quantity") || dataGridView1.CurrentCell.OwningColumn.Name.Equals("NewQuantity"))
            {
                int cInt = Convert.ToInt32(e.KeyChar);
                if ((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57 || cInt == 8)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                EditQtyModel.Response deleteDto = new EditQtyModel.Response();
                string stocktakingID = (string)dataGridView1.Rows[e.Row.Index].Cells["StocktakingID"].Value;
                if (stocktakingID != null)
                {
                    deleteDto.StocktakingID = stocktakingID;
                    deleteDto.Flag = "D";
                    deleteList.Add(deleteDto);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            int num = 1;
            for (int count = 0; count < dataGridView1.Rows.Count; count++)
            {
                dataGridView1.Rows[count].Cells["No"].Value = num;
                num++;
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string valueAfterEdit = dataGridView1.CurrentCell.Value == null ? "" : (dataGridView1.CurrentCell.Value.ToString()).Trim();

                if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("Barcode")) // Barcode
                {
                    #region Barcode
                    if (valueAfterEdit == string.Empty)
                    {
                        MessageBox.Show("Barcode" + MessageConstants.Cannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        dataGridView1.CurrentRow.Cells["Plant"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["StorageLocation"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["SKUMode"].Value = false;
                        dataGridView1.CurrentRow.Cells["SerialNumber"].Value = string.Empty;

                        for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                        {
                            SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                        }
                    }
                    else
                    {
                        var scanMode = dataGridView1.CurrentRow.Cells["ScanMode_"].Value == null ? "0" : dataGridView1.CurrentRow.Cells["ScanMode_"].Value.ToString();
                        var barcode = dataGridView1.CurrentRow.Cells["Barcode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Barcode"].Value.ToString();
                        var location = dataGridView1.CurrentRow.Cells["LocationCode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["LocationCode"].Value.ToString();
                        var unitCode = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? 0 : (int)dataGridView1.CurrentRow.Cells["UnitCode"].Value;
                        var flag = dataGridView1.CurrentRow.Cells["Flag"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Flag"].Value.ToString();
                        var serialNumber = dataGridView1.CurrentRow.Cells["SerialNumber"].Value == null ? "" : dataGridView1.CurrentRow.Cells["SerialNumber"].Value.ToString();
                        if (location != "" && barcode != "")
                        {
                            var stocktakingID = dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null ? "" : dataGridView1.CurrentRow.Cells["StocktakingID"].Value.ToString();
                            var resultBarcodeInMasterSKU = auditBLL.GetDescriptionInMasterSKU(barcode, location, stocktakingID, countDate, unitCode, flag, serialNumber);
                            if (resultBarcodeInMasterSKU == null)
                            {
                                MessageBox.Show(MessageConstants.BarcodenotexistinMaster, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                dataGridView1.CurrentRow.Cells["Plant"].Value = string.Empty;
                                dataGridView1.CurrentRow.Cells["StorageLocation"].Value = string.Empty;
                                dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                dataGridView1.CurrentRow.Cells["SKUMode"].Value = false;
                                dataGridView1.CurrentRow.Cells["SerialNumber"].Value = string.Empty;
                                dataGridView1.CurrentRow.Cells["Barcode"].Value = string.Empty;

                                for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                                {
                                    SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                                }
                            }
                            else
                            {
                                if (resultBarcodeInMasterSKU.Result == "DuplicateBarcode")
                                {
                                    MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    dataGridView1.CurrentRow.Cells["Barcode"].Value = string.Empty;

                                    dataGridView1.CurrentRow.Cells["No"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["Plant"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["StorageLocation"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["LocationCode"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["Barcode"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["SKUCode"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["Quantity"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["NewQuantity"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["ConversionCounter"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["ComputerName"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["UnitCode"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["FLAG"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["Description"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["Remark"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["SerialNumber"].Style.BackColor = Color.Red;

                                    //dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                    //dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                    //dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                    //dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                    //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = string.Empty;
                                    //dataGridView1.CurrentRow.Cells["MKCode"].Value = string.Empty;
                                    //dataGridView1.CurrentRow.Cells["ProductType"].Value = string.Empty;

                                    for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                                    {
                                        var chkUnit = dataGridView1.Rows[count].Cells["UnitCode"].Value == null ? 0 : (int)dataGridView1.Rows[count].Cells["UnitCode"].Value;
                                        var chkLocation = dataGridView1.Rows[count].Cells["LocationCode"].Value == null ? "" : dataGridView1.Rows[count].Cells["LocationCode"].Value.ToString();
                                        var chkBarcode = dataGridView1.Rows[count].Cells["Barcode"].Value == null ? "" : dataGridView1.Rows[count].Cells["Barcode"].Value.ToString();


                                        if (location == chkLocation && barcode == chkBarcode /*&& scanMode=="3"*/ && unitCode == chkUnit)
                                        {
                                            dataGridView1.Rows[count].Cells["No"].Style.BackColor = Color.Red;

                                            dataGridView1.Rows[count].Cells["Plant"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["StorageLocation"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["LocationCode"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["Barcode"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["SKUCode"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["Quantity"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["NewQuantity"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["ConversionCounter"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["ComputerName"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["UnitCode"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["FLAG"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["Description"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["Remark"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["SerialNumber"].Style.BackColor = Color.Red;
                                        }
                                        //else if (location == chkLocation && barcode == chkBarcode && scanMode != "3")
                                        //{
                                        //    dataGridView1.Rows[count].Cells["No"].Style.BackColor = Color.LightGreen;
                                        //    dataGridView1.Rows[count].Cells["ScanMode_"].Style.BackColor = Color.LightGreen;
                                        //    dataGridView1.Rows[count].Cells["LocationCode"].Style.BackColor = Color.LightGreen;
                                        //    dataGridView1.Rows[count].Cells["Barcode"].Style.BackColor = Color.LightGreen;
                                        //    dataGridView1.Rows[count].Cells["SKUCode"].Style.BackColor = Color.LightGreen;
                                        //    dataGridView1.Rows[count].Cells["Quantity"].Style.BackColor = Color.LightGreen;
                                        //    dataGridView1.Rows[count].Cells["NewQuantity"].Style.BackColor = Color.LightGreen;
                                        //    dataGridView1.Rows[count].Cells["UnitCode"].Style.BackColor = Color.LightGreen;
                                        //    dataGridView1.Rows[count].Cells["FLAG"].Style.BackColor = Color.LightGreen;
                                        //    dataGridView1.Rows[count].Cells["Description"].Style.BackColor = Color.LightGreen;
                                        //    dataGridView1.Rows[count].Cells["Remark"].Style.BackColor = Color.LightGreen;
                                        //}
                                        //else
                                        //{
                                        //    SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                                        //}
                                    }

                                    //if (dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null)
                                    //{
                                    //    dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                                    //    int num = 1;
                                    //    for (int count = 0; count < dataGridView1.Rows.Count; count++)
                                    //    {
                                    //        dataGridView1.Rows[count].Cells["No"].Value = num;
                                    //        num++;
                                    //    }
                                    //}
                                    //dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);
                                }

                                else // not duplicate
                                {
                                    if (!string.IsNullOrEmpty(serialNumber) && resultBarcodeInMasterSKU.SerialNumber != serialNumber)
                                    {
                                        MessageBox.Show(MessageConstants.SeialNumbernotexistinMaster, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        dataGridView1.CurrentRow.Cells["SerialNumber"].Value = string.Empty;
                                    }
                                    else
                                    {
                                        dataGridView1.CurrentRow.Cells["Plant"].Value = resultBarcodeInMasterSKU.Plant;
                                        dataGridView1.CurrentRow.Cells["StorageLocation"].Value = resultBarcodeInMasterSKU.StorageLocation;
                                        dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                                        dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                                        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                                        dataGridView1.CurrentRow.Cells["SKUMode"].Value = true;


                                        var chkSN = dataGridView1.CurrentRow.Cells["SerialNumber"].Value;
                                        if ((chkSN == null) && (resultBarcodeInMasterSKU.SerialNumber != ""))
                                        {

                                            dataGridView1.CurrentRow.Cells["Remark"].Value = "* This Product is Require Serial Number";
                                        }

                                        //var scanModeCurrentRow = dataGridView1.CurrentRow.Cells["ScanMode_"].Value.ToString();
                                        int countDup = 0;
                                        int countDupWarehouseUnit = 0;
                                        int countDupHightlight = 0;
                                        var unitCodeOld = string.Empty;
                                        int num1 = 0;
                                        for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                                        {

                                            countDupHightlight = 0;
                                            var chkUnit = dataGridView1.Rows[count].Cells["UnitCode"].Value == null ? 0 : (int)dataGridView1.Rows[count].Cells["UnitCode"].Value;
                                            var stock = dataGridView1.Rows[count].Cells["StocktakingID"].Value == null ? "" : dataGridView1.Rows[count].Cells["StocktakingID"].Value;
                                            var chkLocation = dataGridView1.Rows[count].Cells["LocationCode"].Value == null ? "" : dataGridView1.Rows[count].Cells["LocationCode"].Value.ToString();
                                            var chkBarcode = dataGridView1.Rows[count].Cells["Barcode"].Value == null ? "" : dataGridView1.Rows[count].Cells["Barcode"].Value.ToString();

                                            if (location == chkLocation && barcode == chkBarcode && unitCode == chkUnit)
                                            {

                                                countDup++;
                                            }

                                            //if (location == chkLocation && barcode == chkBarcode /*&& scanModeCurrentRow == "3" */&& unitCode == chkUnit)

                                            //{
                                            //    if(countDupWarehouseUnit == 0)
                                            //    {
                                            //        num1 = count;
                                            //    }
                                            //    countDupWarehouseUnit++;
                                            //}

                                            //if (scanModeCurrentRow == "3")
                                            //{
                                            //    if (location == chkLocation && barcode == chkBarcode && unitCode == chkUnit.ToString() && scanModeCurrentRow == "3" && countDupWarehouseUnit > 1 )
                                            //    {
                                            //        MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            //        dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["MKCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["ProductType"].Value = string.Empty;


                                            //        dataGridView1.Rows[num1].Cells["No"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["ScanMode_"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["LocationCode"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["Barcode"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["SKUCode"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["Quantity"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["NewQuantity"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["UnitCode"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["FLAG"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["Description"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["Remark"].Style.BackColor = Color.LightGreen;

                                            //        countDupHightlight++;

                                            //        if (dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null)
                                            //        {
                                            //            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                                            //            int num = 1;
                                            //            for (int count1 = 0; count1 < dataGridView1.Rows.Count; count1++)
                                            //            {
                                            //                dataGridView1.Rows[count1].Cells["No"].Value = num;
                                            //                num++;
                                            //            }
                                            //        }
                                            //    }
                                            //    else
                                            //    {
                                            //        dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                                            //        dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                                            //        dataGridView1.CurrentRow.Cells["InBarCode"].Value = resultBarcodeInMasterSKU.InBarCode;
                                            //        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                                            //        //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = resultBarcodeInMasterSKU.DepartmentCode;
                                            //        dataGridView1.CurrentRow.Cells["MKCode"].Value = resultBarcodeInMasterSKU.MKCode;
                                            //        dataGridView1.CurrentRow.Cells["ProductType"].Value = resultBarcodeInMasterSKU.ProductType;
                                            //    }
                                            //}
                                            //else if (countDup > 1 && scanModeCurrentRow != "3")
                                            //{
                                            //    if (scanModeCurrentRow == "4")
                                            //    {
                                            //        dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                                            //        dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                                            //        dataGridView1.CurrentRow.Cells["InBarCode"].Value = resultBarcodeInMasterSKU.InBarCode;
                                            //        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                                            //        //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = resultBarcodeInMasterSKU.DepartmentCode;
                                            //        dataGridView1.CurrentRow.Cells["MKCode"].Value = resultBarcodeInMasterSKU.MKCode;
                                            //        dataGridView1.CurrentRow.Cells["ProductType"].Value = resultBarcodeInMasterSKU.ProductType;
                                            //    }
                                            //    else
                                            //    {
                                            //        MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            //        dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                            //        //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["MKCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["ProductType"].Value = string.Empty;
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                                            //    dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                                            //    dataGridView1.CurrentRow.Cells["InBarCode"].Value = resultBarcodeInMasterSKU.InBarCode;
                                            //    dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                                            //    //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = resultBarcodeInMasterSKU.DepartmentCode;
                                            //    dataGridView1.CurrentRow.Cells["MKCode"].Value = resultBarcodeInMasterSKU.MKCode;
                                            //    dataGridView1.CurrentRow.Cells["ProductType"].Value = resultBarcodeInMasterSKU.ProductType;
                                            //    if (resultBarcodeInMasterSKU.UnitCode != 0)
                                            //    {
                                            //        dataGridView1.CurrentRow.Cells["UnitCode"].Value = resultBarcodeInMasterSKU.UnitCode;
                                            //    }

                                            //}
                                            //if (countDupHightlight == 0)
                                            //{
                                            //    SetReadOnlyAndStyle(dataGridView1.Rows[count]);

                                            //}
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                            {
                                SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                            }
                        }
                    }
                    #endregion Barcode
                }

                else if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("LocationCode")) //Location Code
                {
                    #region Location Code

                    var location = dataGridView1.CurrentRow.Cells["LocationCode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["LocationCode"].Value.ToString();
                    var barcode = dataGridView1.CurrentRow.Cells["Barcode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Barcode"].Value.ToString();
                    var unitCode = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? 0 : (int)dataGridView1.CurrentRow.Cells["UnitCode"].Value;

                    if (location == string.Empty)
                    {
                        MessageBox.Show("Location" + MessageConstants.Cannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        dataGridView1.CurrentRow.Cells["Plant"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["StorageLocation"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["SKUMode"].Value = false;
                        dataGridView1.CurrentRow.Cells["SerialNumber"].Value = string.Empty;

                        for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                        {
                            SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                        }
                    }
                    else
                    {
                        if (location.Length < 5)
                        {
                            if (location.Length == 1)
                            {
                                location = "0000" + location;
                                dataGridView1.CurrentRow.Cells["LocationCode"].Value = location;
                            }
                            else if (location.Length == 2)
                            {
                                location = "000" + location;
                                dataGridView1.CurrentRow.Cells["LocationCode"].Value = location;
                            }
                            else if (location.Length == 3)
                            {
                                location = "00" + location;
                                dataGridView1.CurrentRow.Cells["LocationCode"].Value = location;
                            }
                            else
                            {
                                location = "0" + location;
                                dataGridView1.CurrentRow.Cells["LocationCode"].Value = location;
                            }
                        }

                        //var ExistLocation = auditBLL.CheckIsExistLocation(location, "");
                        //if (ExistLocation == "ERROR" || ExistLocation == "NOLOCATION")

                        //{
                        //    MessageBox.Show(MessageConstants.LocationnotexistinMaster, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)

                        //    {
                        //        SetReadOnlyAndStyle(dataGridView1.Rows[count]);

                        //    }
                        //    dataGridView1.CurrentRow.Cells["LocationCode"].Value = string.Empty;

                        //}

                        if (barcode != string.Empty && location != string.Empty)
                        {
                            var stocktakingID = dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null ? "" : dataGridView1.CurrentRow.Cells["StocktakingID"].Value.ToString();
                            var flag = dataGridView1.CurrentRow.Cells["Flag"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Flag"].Value.ToString();
                            var serialNumber = dataGridView1.CurrentRow.Cells["SerialNumber"].Value == null ? "" : dataGridView1.CurrentRow.Cells["SerialNumber"].Value.ToString();

                            var resultBarcodeInMasterSKU = auditBLL.GetDescriptionInMasterSKU(barcode, location, stocktakingID, countDate, unitCode, flag, serialNumber);
                            if (resultBarcodeInMasterSKU == null)
                            {
                                MessageBox.Show(MessageConstants.LocationnotexistinMaster, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                dataGridView1.CurrentRow.Cells["Plant"].Value = string.Empty;
                                dataGridView1.CurrentRow.Cells["StorageLocation"].Value = string.Empty;
                                dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                dataGridView1.CurrentRow.Cells["SKUMode"].Value = false;
                                dataGridView1.CurrentRow.Cells["LocationCode"].Value = string.Empty;
                                dataGridView1.CurrentRow.Cells["SerialNumber"].Value = string.Empty;

                                for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                                {
                                    SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                                }
                            }
                            else
                            {
                                if (resultBarcodeInMasterSKU.Result == "DuplicateBarcode")
                                {
                                    MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    dataGridView1.CurrentRow.Cells["LocationCode"].Value = string.Empty;
                                    //dataGridView1.CurrentRow.Cells["Barcode"].Value = string.Empty;
                                    //dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                    //dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                    //dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                    //dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;

                                    ////dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = string.Empty;
                                    //dataGridView1.CurrentRow.Cells["MKCode"].Value = string.Empty;
                                    //dataGridView1.CurrentRow.Cells["ProductType"].Value = string.Empty;

                                    dataGridView1.CurrentRow.Cells["No"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["Plant"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["StorageLocation"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["LocationCode"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["Barcode"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["SKUCode"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["Quantity"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["NewQuantity"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["ConversionCounter"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["ComputerName"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["UnitCode"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["FLAG"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["Description"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["Remark"].Style.BackColor = Color.Red;
                                    dataGridView1.CurrentRow.Cells["SerialNumber"].Style.BackColor = Color.Red;

                                    for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                                    {
                                        var chkUnit = dataGridView1.Rows[count].Cells["UnitCode"].Value == null ? 0 : (int)dataGridView1.Rows[count].Cells["UnitCode"].Value;
                                        var chkLocation = dataGridView1.Rows[count].Cells["LocationCode"].Value == null ? "" : dataGridView1.Rows[count].Cells["LocationCode"].Value.ToString();
                                        var chkBarcode = dataGridView1.Rows[count].Cells["Barcode"].Value == null ? "" : dataGridView1.Rows[count].Cells["Barcode"].Value.ToString();

                                        if (location == chkLocation && barcode == chkBarcode /*&& scanMode == "3"*/ && unitCode == chkUnit)
                                        {
                                            dataGridView1.Rows[count].Cells["No"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["Plant"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["StorageLocation"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["LocationCode"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["Barcode"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["SKUCode"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["Quantity"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["NewQuantity"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["ConversionCounter"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["ComputerName"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["UnitCode"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["FLAG"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["Description"].Style.BackColor = Color.Red;
                                            dataGridView1.Rows[count].Cells["Remark"].Style.BackColor = Color.Red;

                                            dataGridView1.Rows[count].Cells["SerialNumber"].Style.BackColor = Color.Red;
                                        }
                                        //else
                                        //{
                                        //    SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                                        //}
                                    }

                                    //if (dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null)
                                    //{
                                    //    dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                                    //    int num = 1;
                                    //    for (int count = 0; count < dataGridView1.Rows.Count; count++)
                                    //    {
                                    //        dataGridView1.Rows[count].Cells["No"].Value = num;
                                    //        num++;
                                    //    }
                                    //}
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(serialNumber) && resultBarcodeInMasterSKU.SerialNumber != serialNumber)
                                    {
                                        MessageBox.Show(MessageConstants.SeialNumbernotexistinMaster, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        dataGridView1.CurrentRow.Cells["SerialNumber"].Value = string.Empty;
                                    }
                                    else
                                    {
                                        dataGridView1.CurrentRow.Cells["Plant"].Value = resultBarcodeInMasterSKU.Plant;
                                        dataGridView1.CurrentRow.Cells["StorageLocation"].Value = resultBarcodeInMasterSKU.StorageLocation;
                                        dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                                        dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                                        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                                        dataGridView1.CurrentRow.Cells["SKUMode"].Value = true;


                                        //string scanModeCurrentRow = dataGridView1.CurrentRow.Cells["ScanMode_"].Value.ToString();
                                        int countDup = 0;
                                        int countDupWarehouseUnit = 0;
                                        int countDupHightlight = 0;
                                        var unitCodeOld = string.Empty;
                                        int num1 = 0;
                                        for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                                        {

                                            var chkUnit = dataGridView1.Rows[count].Cells["UnitCode"].Value == null ? 0 : (int)dataGridView1.Rows[count].Cells["UnitCode"].Value;
                                            var stock = dataGridView1.Rows[count].Cells["StocktakingID"].Value == null ? "" : dataGridView1.Rows[count].Cells["StocktakingID"].Value;
                                            var chkLocation = dataGridView1.Rows[count].Cells["LocationCode"].Value == null ? "" : dataGridView1.Rows[count].Cells["LocationCode"].Value.ToString();
                                            var chkBarcode = dataGridView1.Rows[count].Cells["Barcode"].Value == null ? "" : dataGridView1.Rows[count].Cells["Barcode"].Value.ToString();

                                            if (location == chkLocation && barcode == chkBarcode && unitCode == chkUnit)
                                            {

                                                countDup++;
                                            }
                                            //if (location == chkLocation && barcode == chkBarcode /*&& scanModeCurrentRow == "3"*/ && unitCode == chkUnit)

                                            //{
                                            //    if (countDupWarehouseUnit == 0)
                                            //    {
                                            //        num1 = count;
                                            //    }
                                            //    countDupWarehouseUnit++;
                                            //}

                                            //if (scanModeCurrentRow == "3")
                                            //{
                                            //    if (location == chkLocation && barcode == chkBarcode && unitCode == chkUnit.ToString() && scanModeCurrentRow == "3" && countDupWarehouseUnit > 1)
                                            //    {
                                            //        MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            //        dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["MKCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["ProductType"].Value = string.Empty;


                                            //        dataGridView1.Rows[num1].Cells["No"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["ScanMode_"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["LocationCode"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["Barcode"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["SKUCode"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["Quantity"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["NewQuantity"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["UnitCode"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["FLAG"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["Description"].Style.BackColor = Color.LightGreen;
                                            //        dataGridView1.Rows[num1].Cells["Remark"].Style.BackColor = Color.LightGreen;

                                            //        countDupHightlight++;

                                            //        if (dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null)
                                            //        {
                                            //            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                                            //            int num = 1;
                                            //            for (int count1 = 0; count1 < dataGridView1.Rows.Count; count1++)
                                            //            {
                                            //                dataGridView1.Rows[count1].Cells["No"].Value = num;
                                            //                num++;
                                            //            }
                                            //        }
                                            //    }
                                            //    else
                                            //    {
                                            //        dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                                            //        dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                                            //        dataGridView1.CurrentRow.Cells["InBarCode"].Value = resultBarcodeInMasterSKU.InBarCode;
                                            //        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                                            //        //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = resultBarcodeInMasterSKU.DepartmentCode;
                                            //        dataGridView1.CurrentRow.Cells["MKCode"].Value = resultBarcodeInMasterSKU.MKCode;
                                            //        dataGridView1.CurrentRow.Cells["ProductType"].Value = resultBarcodeInMasterSKU.ProductType;
                                            //        dataGridView1.CurrentRow.Cells["Plant"].Value = resultBarcodeInMasterSKU.Plant;
                                            //        dataGridView1.CurrentRow.Cells["StorageLocation"].Value = resultBarcodeInMasterSKU.StorageLocation;
                                            //    }
                                            //}
                                            //else if (countDup > 1 && scanModeCurrentRow != "3")
                                            ////if (countDup > 1)
                                            //{
                                            //    if (scanModeCurrentRow == "4" || (scanModeCurrentRow == "3"))
                                            //    {
                                            //        dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                                            //        dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                                            //        dataGridView1.CurrentRow.Cells["InBarCode"].Value = resultBarcodeInMasterSKU.InBarCode;
                                            //        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                                            //        //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = resultBarcodeInMasterSKU.DepartmentCode;
                                            //        dataGridView1.CurrentRow.Cells["MKCode"].Value = resultBarcodeInMasterSKU.MKCode;
                                            //        dataGridView1.CurrentRow.Cells["ProductType"].Value = resultBarcodeInMasterSKU.ProductType;
                                            //        dataGridView1.CurrentRow.Cells["Plant"].Value = resultBarcodeInMasterSKU.Plant;
                                            //        dataGridView1.CurrentRow.Cells["StorageLocation"].Value = resultBarcodeInMasterSKU.StorageLocation;
                                            //    }
                                            //    else
                                            //    {
                                            //        MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            //        dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                            //        //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["MKCode"].Value = string.Empty;
                                            //        dataGridView1.CurrentRow.Cells["ProductType"].Value = string.Empty;
                                            //    }
                                            //    //MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            //    //dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                            //    //dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                            //    //dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                            //    //dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                            //    //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = string.Empty;

                                            //}
                                            //else
                                            //{
                                            //    dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                                            //    dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                                            //    dataGridView1.CurrentRow.Cells["InBarCode"].Value = resultBarcodeInMasterSKU.InBarCode;
                                            //    dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                                            //    //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = resultBarcodeInMasterSKU.DepartmentCode;
                                            //    dataGridView1.CurrentRow.Cells["MKCode"].Value = resultBarcodeInMasterSKU.MKCode;
                                            //    dataGridView1.CurrentRow.Cells["ProductType"].Value = resultBarcodeInMasterSKU.ProductType;
                                            //    dataGridView1.CurrentRow.Cells["Plant"].Value = resultBarcodeInMasterSKU.Plant;
                                            //    dataGridView1.CurrentRow.Cells["StorageLocation"].Value = resultBarcodeInMasterSKU.StorageLocation;
                                            //    if (resultBarcodeInMasterSKU.UnitCode != 0)
                                            //    {
                                            //        dataGridView1.CurrentRow.Cells["UnitCode"].Value = resultBarcodeInMasterSKU.UnitCode;
                                            //    }
                                            //}

                                            if (countDupHightlight == 0)
                                            {

                                                SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                            {
                                SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                            }
                        }
                    }

                    /*
                    var scanMode = dataGridView1.CurrentRow.Cells["ScanMode_"].Value == null ? "" : dataGridView1.CurrentRow.Cells["ScanMode_"].Value.ToString();
                    var barcode = dataGridView1.CurrentRow.Cells["Barcode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Barcode"].Value.ToString();
                    var location = dataGridView1.CurrentRow.Cells["LocationCode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["LocationCode"].Value.ToString();
                    var unitCode = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["UnitCode"].Value.ToString();
                    if (location == string.Empty)
                    {
                        MessageBox.Show("Location" + MessageConstants.Cannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = string.Empty;

                        for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                        {
                            SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                        }
                    }
                    else
                    {
                        if (location.Length < 5)
                        {
                            if (location.Length == 1)
                            {
                                location = "0000" + location;
                                dataGridView1.CurrentRow.Cells["LocationCode"].Value = location;
                            }
                            else if (location.Length == 2)
                            {
                                location = "000" + location;
                                dataGridView1.CurrentRow.Cells["LocationCode"].Value = location;
                            }
                            else if (location.Length == 3)
                            {
                                location = "00" + location;
                                dataGridView1.CurrentRow.Cells["LocationCode"].Value = location;
                            }
                            else
                            {
                                location = "0" + location;
                                dataGridView1.CurrentRow.Cells["LocationCode"].Value = location;
                            }
                        }


                        var DepartmentCodeByLocation = auditBLL.CheckIsExistLocation(location, "");
                        if (DepartmentCodeByLocation == "ERROR" || DepartmentCodeByLocation == "NOLOCATION")
                        {
                            MessageBox.Show(MessageConstants.LocationnotexistinMaster, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                            {
                                SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                            }
                            dataGridView1.CurrentRow.Cells["LocationCode"].Value = string.Empty;
                        }
                        else
                        {
                            dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = DepartmentCodeByLocation;
                            var description = dataGridView1.CurrentRow.Cells["Description"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Description"].Value.ToString();
                            if (barcode != string.Empty && location != string.Empty)
                            {
                                var stocktakingID = dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null ? "" : dataGridView1.CurrentRow.Cells["StocktakingID"].Value.ToString();
                                var resultBarcodeInMasterSKU = auditBLL.GetDescriptionInMasterSKU(barcode, location, stocktakingID, countDate, Convert.ToInt16(scanMode), Convert.ToInt16(unitCode));
                                if (resultBarcodeInMasterSKU == null)
                                {
                                    MessageBox.Show(MessageConstants.BarcodenotexistinMaster, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    //dataGridView1.CurrentRow.Cells["Barcode"].Value = string.Empty;
                                    dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;

                                    for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                                    {
                                        SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                                    }
                                }
                                else
                                {
                                    if (resultBarcodeInMasterSKU.Result == "DuplicateBarcode")
                                    {
                                        MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        //dataGridView1.CurrentRow.Cells["Barcode"].Value = string.Empty;
                                        dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                        dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                        dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                        //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = string.Empty;
                                        dataGridView1.CurrentRow.Cells["MKCode"].Value = string.Empty;
                                        dataGridView1.CurrentRow.Cells["ProductType"].Value = string.Empty;

                                        for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                                        {
                                            string chkUnit = dataGridView1.Rows[count].Cells["UnitCode"].Value == null ? "" : dataGridView1.Rows[count].Cells["UnitCode"].Value.ToString();
                                            var chkLocation = dataGridView1.Rows[count].Cells["LocationCode"].Value == null ? "" : dataGridView1.Rows[count].Cells["LocationCode"].Value.ToString();
                                            var chkBarcode = dataGridView1.Rows[count].Cells["Barcode"].Value == null ? "" : dataGridView1.Rows[count].Cells["Barcode"].Value.ToString();

                                            if (location == chkLocation && barcode == chkBarcode && scanMode == "3" && unitCode == chkUnit)
                                            {
                                                dataGridView1.Rows[count].Cells["No"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["ScanMode_"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["LocationCode"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["Barcode"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["SKUCode"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["Quantity"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["NewQuantity"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["UnitCode"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["FLAG"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["Description"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["Remark"].Style.BackColor = Color.LightGreen;
                                            }
                                            else if (location == chkLocation && barcode == chkBarcode && scanMode != "3")  
                                            {
                                                dataGridView1.Rows[count].Cells["No"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["ScanMode_"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["LocationCode"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["Barcode"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["SKUCode"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["Quantity"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["NewQuantity"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["UnitCode"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["FLAG"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["Description"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[count].Cells["Remark"].Style.BackColor = Color.LightGreen;
                                            }
                                            else
                                            {
                                                SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                                            }
                                        }
                                        if (dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null)
                                        {
                                            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                                            int num = 1;
                                            for (int count = 0; count < dataGridView1.Rows.Count; count++)
                                            {
                                                dataGridView1.Rows[count].Cells["No"].Value = num;
                                                num++;
                                            }
                                        }
                                        //dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                                    }
                                    else
                                    {

                                        string scanModeCurrentRow = dataGridView1.CurrentRow.Cells["ScanMode_"].Value.ToString();
                                        int countDup = 0;
                                        int countDupWarehouseUnit = 0;
                                        int countDupHightlight = 0;
                                        var unitCodeOld = string.Empty;
                                        int num1 = 0;
                                        for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                                        {
                                            string chkUnit = dataGridView1.Rows[count].Cells["UnitCode"].Value == null ? "" : dataGridView1.Rows[count].Cells["UnitCode"].Value.ToString();
                                            var stock = dataGridView1.Rows[count].Cells["StocktakingID"].Value == null ? "" : dataGridView1.Rows[count].Cells["StocktakingID"].Value;
                                            var chkLocation = dataGridView1.Rows[count].Cells["LocationCode"].Value == null ? "" : dataGridView1.Rows[count].Cells["LocationCode"].Value.ToString();
                                            var chkBarcode = dataGridView1.Rows[count].Cells["Barcode"].Value == null ? "" : dataGridView1.Rows[count].Cells["Barcode"].Value.ToString();

                                            if (location == chkLocation && barcode == chkBarcode)
                                            {
                                                countDup++;
                                            }
                                            if (location == chkLocation && barcode == chkBarcode && scanModeCurrentRow == "3" && unitCode == chkUnit)
                                            {
                                                if (countDupWarehouseUnit == 0)
                                                {
                                                    num1 = count;
                                                }
                                                countDupWarehouseUnit++;
                                            }
                                            if (scanModeCurrentRow == "3")
                                            {
                                                if (location == chkLocation && barcode == chkBarcode && unitCode == chkUnit.ToString() && scanModeCurrentRow == "3" && countDupWarehouseUnit > 1)
                                                {
                                                    MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                    dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                                    dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                                    dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                                    dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                                    dataGridView1.CurrentRow.Cells["MKCode"].Value = string.Empty;
                                                    dataGridView1.CurrentRow.Cells["ProductType"].Value = string.Empty;

                                                    dataGridView1.Rows[num1].Cells["No"].Style.BackColor = Color.LightGreen;
                                                    dataGridView1.Rows[num1].Cells["ScanMode_"].Style.BackColor = Color.LightGreen;
                                                    dataGridView1.Rows[num1].Cells["LocationCode"].Style.BackColor = Color.LightGreen;
                                                    dataGridView1.Rows[num1].Cells["Barcode"].Style.BackColor = Color.LightGreen;
                                                    dataGridView1.Rows[num1].Cells["SKUCode"].Style.BackColor = Color.LightGreen;
                                                    dataGridView1.Rows[num1].Cells["Quantity"].Style.BackColor = Color.LightGreen;
                                                    dataGridView1.Rows[num1].Cells["NewQuantity"].Style.BackColor = Color.LightGreen;
                                                    dataGridView1.Rows[num1].Cells["UnitCode"].Style.BackColor = Color.LightGreen;
                                                    dataGridView1.Rows[num1].Cells["FLAG"].Style.BackColor = Color.LightGreen;
                                                    dataGridView1.Rows[num1].Cells["Description"].Style.BackColor = Color.LightGreen;
                                                    dataGridView1.Rows[num1].Cells["Remark"].Style.BackColor = Color.LightGreen;

                                                    countDupHightlight++;

                                                    if (dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null)
                                                    {
                                                        dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                                                        int num = 1;
                                                        for (int count1 = 0; count1 < dataGridView1.Rows.Count; count1++)
                                                        {
                                                            dataGridView1.Rows[count1].Cells["No"].Value = num;
                                                            num++;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                                                    dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                                                    dataGridView1.CurrentRow.Cells["InBarCode"].Value = resultBarcodeInMasterSKU.InBarCode;
                                                    dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                                                    //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = resultBarcodeInMasterSKU.DepartmentCode;
                                                    dataGridView1.CurrentRow.Cells["MKCode"].Value = resultBarcodeInMasterSKU.MKCode;
                                                    dataGridView1.CurrentRow.Cells["ProductType"].Value = resultBarcodeInMasterSKU.ProductType;
                                                }
                                            }
                                            else if (countDup > 1 && scanModeCurrentRow != "3")
                                            //if (countDup > 1)
                                            {
                                                if (scanModeCurrentRow == "4" || (scanModeCurrentRow == "3"))
                                                {
                                                    dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                                                    dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                                                    dataGridView1.CurrentRow.Cells["InBarCode"].Value = resultBarcodeInMasterSKU.InBarCode;
                                                    dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                                                    //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = resultBarcodeInMasterSKU.DepartmentCode;
                                                    dataGridView1.CurrentRow.Cells["MKCode"].Value = resultBarcodeInMasterSKU.MKCode;
                                                    dataGridView1.CurrentRow.Cells["ProductType"].Value = resultBarcodeInMasterSKU.ProductType;
                                                }
                                                else
                                                {
                                                    MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                    dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                                    dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                                    dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                                    dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                                    //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = string.Empty;
                                                    dataGridView1.CurrentRow.Cells["MKCode"].Value = string.Empty;
                                                    dataGridView1.CurrentRow.Cells["ProductType"].Value = string.Empty;
                                                }
                                                //MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                //dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                                //dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                                //dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                                //dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                                //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = string.Empty;

                                            }
                                            else
                                            {
                                                dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                                                dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                                                dataGridView1.CurrentRow.Cells["InBarCode"].Value = resultBarcodeInMasterSKU.InBarCode;
                                                dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                                                //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = resultBarcodeInMasterSKU.DepartmentCode;
                                                dataGridView1.CurrentRow.Cells["MKCode"].Value = resultBarcodeInMasterSKU.MKCode;
                                                dataGridView1.CurrentRow.Cells["ProductType"].Value = resultBarcodeInMasterSKU.ProductType;
                                                if (resultBarcodeInMasterSKU.UnitCode != 0)
                                                {
                                                    dataGridView1.CurrentRow.Cells["UnitCode"].Value = resultBarcodeInMasterSKU.UnitCode;
                                                }
                                            }

                                            if (countDupHightlight == 0)
                                            {
                                                SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                                {
                                    SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                                }
                            }
                        }
                    }
                    */
                    #endregion Location Code
                }

                else if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("SerialNumber")) //Serial Number
                {
                    #region Serial Number

                    var barcode = dataGridView1.CurrentRow.Cells["Barcode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Barcode"].Value.ToString();
                    var location = dataGridView1.CurrentRow.Cells["LocationCode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["LocationCode"].Value.ToString();
                    var serialNumber = dataGridView1.CurrentRow.Cells["SerialNumber"].Value == null ? "" : dataGridView1.CurrentRow.Cells["SerialNumber"].Value.ToString();
                    var stocktakingID = dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null ? "" : dataGridView1.CurrentRow.Cells["StocktakingID"].Value.ToString();
                    var unitCode = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? 0 : (int)dataGridView1.CurrentRow.Cells["UnitCode"].Value;
                    var flag = dataGridView1.CurrentRow.Cells["Flag"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Flag"].Value.ToString();

                    var resultBarcodeInMasterSKU = auditBLL.GetDescriptionInMasterSKU(barcode, location, stocktakingID, countDate, unitCode, flag, serialNumber);

                    if (barcode != string.Empty && location != string.Empty && serialNumber != string.Empty)
                    {
                        //if (!string.IsNullOrEmpty(serialNumber))
                        //{

                        if (resultBarcodeInMasterSKU == null)
                        {
                            MessageBox.Show(MessageConstants.SerialnotexistinMaster, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            dataGridView1.CurrentRow.Cells["Plant"].Value = string.Empty;
                            dataGridView1.CurrentRow.Cells["StorageLocation"].Value = string.Empty;
                            dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                            dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                            dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                            dataGridView1.CurrentRow.Cells["SKUMode"].Value = false;
                            dataGridView1.CurrentRow.Cells["SerialNumber"].Value = serialBefore;

                            for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                            {
                                SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                            }
                        }
                        else if (auditBLL.IsExistSerialNumberInCountDate(serialNumber, countDate, stocktakingID))
                        {
                            MessageBox.Show(MessageConstants.DuplicateSerialNumber, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridView1.CurrentRow.Cells["SerialNumber"].Value = serialBefore;

                            dataGridView1.CurrentRow.Cells["No"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Plant"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["StorageLocation"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["LocationCode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Barcode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["SKUCode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Quantity"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["NewQuantity"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["ConversionCounter"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["ComputerName"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["UnitCode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["FLAG"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Description"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Remark"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["SerialNumber"].Style.BackColor = Color.Red;
                        }
                        else if (resultBarcodeInMasterSKU.SerialNumber != serialNumber)
                        {
                            MessageBox.Show(MessageConstants.SeialNumbernotexistinMaster, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridView1.CurrentRow.Cells["SerialNumber"].Value = serialBefore;

                            dataGridView1.CurrentRow.Cells["No"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Plant"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["StorageLocation"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["LocationCode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Barcode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["SKUCode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Quantity"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["NewQuantity"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["ConversionCounter"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["ComputerName"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["UnitCode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["FLAG"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Description"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Remark"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["SerialNumber"].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            dataGridView1.CurrentRow.Cells["Plant"].Value = resultBarcodeInMasterSKU.Plant;
                            dataGridView1.CurrentRow.Cells["StorageLocation"].Value = resultBarcodeInMasterSKU.StorageLocation;
                            dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                            dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                            dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                            dataGridView1.CurrentRow.Cells["SKUMode"].Value = true;

                            if (flag == "I" || flag == "E" || flag == "F" || flag == "U")
                            {
                                dataGridView1.CurrentRow.Cells["NewQuantity"].Value = 1;
                                dataGridView1.CurrentRow.Cells["NewQuantity"].ReadOnly = true;
                                dataGridView1.CurrentRow.Cells["NewQuantity"].Style.BackColor = Color.LightGray;
                            }
                            else
                            {
                                dataGridView1.CurrentRow.Cells["Quantity"].Value = 1;
                                dataGridView1.CurrentRow.Cells["Quantity"].ReadOnly = true;
                                dataGridView1.CurrentRow.Cells["Quantity"].Style.BackColor = Color.LightGray;
                            }
                        }

                        //}
                    }
                    else if (serialNumber != string.Empty)
                    {
                        if (auditBLL.IsExistSerialNumberInCountDate(serialNumber, countDate, stocktakingID))
                        {
                            MessageBox.Show(MessageConstants.DuplicateSerialNumber, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridView1.CurrentRow.Cells["SerialNumber"].Value = serialBefore;

                            dataGridView1.CurrentRow.Cells["No"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Plant"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["StorageLocation"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["LocationCode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Barcode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["SKUCode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Quantity"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["NewQuantity"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["ConversionCounter"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["ComputerName"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["UnitCode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["FLAG"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Description"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Remark"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["SerialNumber"].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            if (flag == "I" || flag == "E" || flag == "F" || flag == "U")
                            {
                                dataGridView1.CurrentRow.Cells["NewQuantity"].Value = 1;
                                dataGridView1.CurrentRow.Cells["NewQuantity"].ReadOnly = true;
                                dataGridView1.CurrentRow.Cells["NewQuantity"].Style.BackColor = Color.LightGray;
                            }
                            else
                            {
                                dataGridView1.CurrentRow.Cells["Quantity"].Value = 1;
                                dataGridView1.CurrentRow.Cells["Quantity"].ReadOnly = true;
                                dataGridView1.CurrentRow.Cells["Quantity"].Style.BackColor = Color.LightGray;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(serialNumber))
                    {
                        if (flag == "I" || flag == "E" || flag == "F" || flag == "U")
                        {
                            dataGridView1.CurrentRow.Cells["NewQuantity"].ReadOnly = false;
                            dataGridView1.CurrentRow.Cells["NewQuantity"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            dataGridView1.CurrentRow.Cells["Quantity"].ReadOnly = false;
                            dataGridView1.CurrentRow.Cells["Quantity"].Style.BackColor = Color.White;
                        }
                    }

                    #endregion Serial Number
                }

                else if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("UnitCode"))
                {
                    #region unitcode

                    var scanMode = dataGridView1.CurrentRow.Cells["ScanMode_"].Value == null ? "" : dataGridView1.CurrentRow.Cells["ScanMode_"].Value.ToString();
                    var barcode = dataGridView1.CurrentRow.Cells["Barcode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Barcode"].Value.ToString();
                    var location = dataGridView1.CurrentRow.Cells["LocationCode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["LocationCode"].Value.ToString();
                    var unitCode = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? 0 : (int)dataGridView1.CurrentRow.Cells["UnitCode"].Value;
                    var conversionCounter = dataGridView1.CurrentRow.Cells["ConversionCounter"].Value == null ? "" : (string)dataGridView1.CurrentRow.Cells["ConversionCounter"].Value;
                    //if (location == string.Empty)
                    //{
                    //    MessageBox.Show("Location" + MessageConstants.Cannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;

                    //    for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                    //    {
                    //        SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                    //    }
                    //}
                    //else
                    //{
                    //var checkIsExistLocation = auditBLL.CheckIsExistLocation(location, Convert.ToInt16(scanMode));
                    //if (checkIsExistLocation)
                    //{
                    //    var description = dataGridView1.CurrentRow.Cells["Description"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Description"].Value.ToString();
                    if (barcode != string.Empty && location != string.Empty)
                    {
                        var stocktakingID = dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null ? "" : dataGridView1.CurrentRow.Cells["StocktakingID"].Value.ToString();
                        var flag = dataGridView1.CurrentRow.Cells["Flag"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Flag"].Value.ToString();
                        var serialNumber = dataGridView1.CurrentRow.Cells["SerialNumber"].Value == null ? "" : dataGridView1.CurrentRow.Cells["SerialNumber"].Value.ToString();
                        var oldConversion = dataGridView1.CurrentRow.Cells["OldConversionCounter"].Value == null ? "" : dataGridView1.CurrentRow.Cells["OldConversionCounter"].Value.ToString();

                        var resultBarcodeInMasterSKU = auditBLL.GetDescriptionInMasterSKU(barcode, location, stocktakingID, countDate, unitCode, flag, serialNumber);

                        //if (resultBarcodeInMasterSKU == null)
                        //{
                        //    MessageBox.Show(MessageConstants.BarcodenotexistinMaster, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    //dataGridView1.CurrentRow.Cells["Barcode"].Value = string.Empty;
                        //    dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;

                        //    for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)

                        //    {
                        //        SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                        //    }
                        //}
                        //else
                        //{
                        if (resultBarcodeInMasterSKU.Result == "DuplicateBarcode")
                        {
                            MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridView1.CurrentRow.Cells["UnitCode"].Value = unitBefore;


                            dataGridView1.CurrentRow.Cells["ConversionCounter"].Value = oldConversion;

                            dataGridView1.CurrentRow.Cells["No"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Plant"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["StorageLocation"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["LocationCode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Barcode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["SKUCode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Quantity"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["NewQuantity"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["ConversionCounter"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["ComputerName"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["UnitCode"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["FLAG"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Description"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["Remark"].Style.BackColor = Color.Red;
                            dataGridView1.CurrentRow.Cells["SerialNumber"].Style.BackColor = Color.Red;

                            ;
                            ////dataGridView1.CurrentRow.Cells["Barcode"].Value = string.Empty;
                            //dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                            //dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                            //dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                            //dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                            ////dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = string.Empty;
                            //dataGridView1.CurrentRow.Cells["MKCode"].Value = string.Empty;
                            //dataGridView1.CurrentRow.Cells["ProductType"].Value = string.Empty;

                            for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                            {
                                var chkUnit = dataGridView1.Rows[count].Cells["UnitCode"].Value == null ? 0 : (int)dataGridView1.Rows[count].Cells["UnitCode"].Value;
                                var chkLocation = dataGridView1.Rows[count].Cells["LocationCode"].Value == null ? "" : dataGridView1.Rows[count].Cells["LocationCode"].Value.ToString();
                                var chkBarcode = dataGridView1.Rows[count].Cells["Barcode"].Value == null ? "" : dataGridView1.Rows[count].Cells["Barcode"].Value.ToString();

                                if (location == chkLocation && barcode == chkBarcode /* && scanMode == "3"*/ && unitCode == chkUnit)
                                {
                                    dataGridView1.Rows[count].Cells["No"].Style.BackColor = Color.Red;

                                    dataGridView1.Rows[count].Cells["Plant"].Style.BackColor = Color.Red;
                                    dataGridView1.Rows[count].Cells["StorageLocation"].Style.BackColor = Color.Red;
                                    dataGridView1.Rows[count].Cells["LocationCode"].Style.BackColor = Color.Red;
                                    dataGridView1.Rows[count].Cells["Barcode"].Style.BackColor = Color.Red;
                                    dataGridView1.Rows[count].Cells["SKUCode"].Style.BackColor = Color.Red;
                                    dataGridView1.Rows[count].Cells["Quantity"].Style.BackColor = Color.Red;
                                    dataGridView1.Rows[count].Cells["NewQuantity"].Style.BackColor = Color.Red;
                                    dataGridView1.Rows[count].Cells["ConversionCounter"].Style.BackColor = Color.Red;
                                    dataGridView1.Rows[count].Cells["ComputerName"].Style.BackColor = Color.Red;
                                    dataGridView1.Rows[count].Cells["UnitCode"].Style.BackColor = Color.Red;
                                    dataGridView1.Rows[count].Cells["FLAG"].Style.BackColor = Color.Red;
                                    dataGridView1.Rows[count].Cells["Description"].Style.BackColor = Color.Red;
                                    dataGridView1.Rows[count].Cells["Remark"].Style.BackColor = Color.Red;
                                    dataGridView1.Rows[count].Cells["SerialNumber"].Style.BackColor = Color.Red;
                                }
                                //else if (location == chkLocation && barcode == chkBarcode && scanMode != "3")
                                //{
                                //    dataGridView1.Rows[count].Cells["No"].Style.BackColor = Color.LightGreen;
                                //    dataGridView1.Rows[count].Cells["ScanMode_"].Style.BackColor = Color.LightGreen;
                                //    dataGridView1.Rows[count].Cells["LocationCode"].Style.BackColor = Color.LightGreen;
                                //    dataGridView1.Rows[count].Cells["Barcode"].Style.BackColor = Color.LightGreen;
                                //    dataGridView1.Rows[count].Cells["SKUCode"].Style.BackColor = Color.LightGreen;
                                //    dataGridView1.Rows[count].Cells["Quantity"].Style.BackColor = Color.LightGreen;
                                //    dataGridView1.Rows[count].Cells["NewQuantity"].Style.BackColor = Color.LightGreen;
                                //    dataGridView1.Rows[count].Cells["UnitCode"].Style.BackColor = Color.LightGreen;
                                //    dataGridView1.Rows[count].Cells["FLAG"].Style.BackColor = Color.LightGreen;
                                //    dataGridView1.Rows[count].Cells["Description"].Style.BackColor = Color.LightGreen;
                                //    dataGridView1.Rows[count].Cells["Remark"].Style.BackColor = Color.LightGreen;
                                //}
                                //else
                                //{
                                //    SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                                //}
                            }

                            //if (dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null)
                            //{
                            //    dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                            //    int num = 1;
                            //    for (int count = 0; count < dataGridView1.Rows.Count; count++)
                            //    {
                            //        dataGridView1.Rows[count].Cells["No"].Value = num;
                            //        num++;
                            //    }
                            //}
                            //dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        }
                        else
                        {
                            //string scanModeCurrentRow = dataGridView1.CurrentRow.Cells["ScanMode_"].Value.ToString();
                            int countDupWarehouseUnit = 0;
                            int countDupHightlight = 0;
                            var unitCodeOld = string.Empty;
                            int num1 = 0;
                            int countDup = 0;
                            for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                            {
                                var chkUnit = dataGridView1.Rows[count].Cells["UnitCode"].Value == null ? 0 : (int)dataGridView1.Rows[count].Cells["UnitCode"].Value;
                                var stock = dataGridView1.Rows[count].Cells["StocktakingID"].Value == null ? "" : dataGridView1.Rows[count].Cells["StocktakingID"].Value;
                                var chkLocation = dataGridView1.Rows[count].Cells["LocationCode"].Value == null ? "" : dataGridView1.Rows[count].Cells["LocationCode"].Value.ToString();
                                var chkBarcode = dataGridView1.Rows[count].Cells["Barcode"].Value == null ? "" : dataGridView1.Rows[count].Cells["Barcode"].Value.ToString();

                                if (location == chkLocation && barcode == chkBarcode && unitCode == chkUnit)
                                {
                                    countDup++;
                                }

                                //if (location == chkLocation && barcode == chkBarcode && scanModeCurrentRow == "3" && unitCode == chkUnit)

                                //{
                                //    if (countDupWarehouseUnit == 0)



                                //    {
                                //        num1 = count;
                                //    }
                                //    countDupWarehouseUnit++;

                                //}
                                //if (scanModeCurrentRow == "3")

                                //{
                                //    if (location == chkLocation && barcode == chkBarcode && unitCode == chkUnit.ToString() && scanModeCurrentRow == "3" && countDupWarehouseUnit > 1)

                                //    {
                                //        MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //        dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                //        dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                //        dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                //        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                //        dataGridView1.CurrentRow.Cells["MKCode"].Value = string.Empty;
                                //        dataGridView1.CurrentRow.Cells["ProductType"].Value = string.Empty;

                                //        dataGridView1.Rows[num1].Cells["No"].Style.BackColor = Color.LightGreen;
                                //        dataGridView1.Rows[num1].Cells["ScanMode_"].Style.BackColor = Color.LightGreen;
                                //        dataGridView1.Rows[num1].Cells["LocationCode"].Style.BackColor = Color.LightGreen;
                                //        dataGridView1.Rows[num1].Cells["Barcode"].Style.BackColor = Color.LightGreen;
                                //        dataGridView1.Rows[num1].Cells["SKUCode"].Style.BackColor = Color.LightGreen;
                                //        dataGridView1.Rows[num1].Cells["Quantity"].Style.BackColor = Color.LightGreen;
                                //        dataGridView1.Rows[num1].Cells["NewQuantity"].Style.BackColor = Color.LightGreen;
                                //        dataGridView1.Rows[num1].Cells["UnitCode"].Style.BackColor = Color.LightGreen;
                                //        dataGridView1.Rows[num1].Cells["FLAG"].Style.BackColor = Color.LightGreen;
                                //        dataGridView1.Rows[num1].Cells["Description"].Style.BackColor = Color.LightGreen;
                                //        dataGridView1.Rows[num1].Cells["Remark"].Style.BackColor = Color.LightGreen;


                                //        countDupHightlight++;

                                //        if (dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null)

                                //        {
                                //            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);

                                //            int num = 1;
                                //            for (int count1 = 0; count1 < dataGridView1.Rows.Count; count1++)

                                //            {
                                //                dataGridView1.Rows[count1].Cells["No"].Value = num;






                                //                num++;
                                //            }
                                //        }
                                //    }
                                //    else
                                //    {
                                //        dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                                //        dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                                //        dataGridView1.CurrentRow.Cells["InBarCode"].Value = resultBarcodeInMasterSKU.InBarCode;
                                //        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                                //        //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = resultBarcodeInMasterSKU.DepartmentCode;
                                //        dataGridView1.CurrentRow.Cells["MKCode"].Value = resultBarcodeInMasterSKU.MKCode;
                                //        dataGridView1.CurrentRow.Cells["ProductType"].Value = resultBarcodeInMasterSKU.ProductType;


                                //    }
                                //}
                                //else if (countDup > 1 && scanModeCurrentRow != "3")

                                //{
                                //    if (scanModeCurrentRow == "4" || (scanModeCurrentRow == "3"))

                                //    {
                                //        dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                                //        dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                                //        dataGridView1.CurrentRow.Cells["InBarCode"].Value = resultBarcodeInMasterSKU.InBarCode;
                                //        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                                //        //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = resultBarcodeInMasterSKU.DepartmentCode;
                                //        dataGridView1.CurrentRow.Cells["MKCode"].Value = resultBarcodeInMasterSKU.MKCode;
                                //        dataGridView1.CurrentRow.Cells["ProductType"].Value = resultBarcodeInMasterSKU.ProductType;



                                //    }
                                //    else
                                //    {
                                //        MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //        dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                //        dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                //        dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                //        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                //        //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = string.Empty;
                                //        dataGridView1.CurrentRow.Cells["MKCode"].Value = string.Empty;
                                //        dataGridView1.CurrentRow.Cells["ProductType"].Value = string.Empty;

                                //    }
                                //    //MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //    //dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                //    //dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                //    //dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                //    //dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                //    //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = string.Empty;




                                //}
                                //else
                                //{
                                //    dataGridView1.CurrentRow.Cells["Description"].Value = resultBarcodeInMasterSKU.Description;
                                //    dataGridView1.CurrentRow.Cells["SKUCode"].Value = resultBarcodeInMasterSKU.SKUCode;
                                //    dataGridView1.CurrentRow.Cells["InBarCode"].Value = resultBarcodeInMasterSKU.InBarCode;
                                //    dataGridView1.CurrentRow.Cells["ExbarCode"].Value = resultBarcodeInMasterSKU.ExBarCode;
                                //    //dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = resultBarcodeInMasterSKU.DepartmentCode;
                                //    dataGridView1.CurrentRow.Cells["MKCode"].Value = resultBarcodeInMasterSKU.MKCode;
                                //    dataGridView1.CurrentRow.Cells["ProductType"].Value = resultBarcodeInMasterSKU.ProductType;
                                //    if (resultBarcodeInMasterSKU.UnitCode != 0)

                                //    {
                                //        dataGridView1.CurrentRow.Cells["UnitCode"].Value = resultBarcodeInMasterSKU.UnitCode;


                                //    }
                                //}
                                if (countDupHightlight == 0)
                                {
                                    SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                                }
                            }
                        }

                        // }
                    }
                    else
                    {
                        for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                        {
                            SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                        }
                    }
                    //}
                    //else {
                    //    MessageBox.Show(MessageConstants.LocationnotexistinMaster, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                    //    {
                    //        SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                    //    }
                    //    dataGridView1.CurrentRow.Cells["LocationCode"].Value = string.Empty;
                    //}
                    //}
                    #endregion unitcode
                }
                //else if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("ScanMode"))
                //{
                //    string location = dataGridView1.CurrentRow.Cells["LocationCode"].Value == null ? string.Empty : dataGridView1.CurrentRow.Cells["LocationCode"].Value.ToString();
                //    string barcode = dataGridView1.CurrentRow.Cells["Barcode"].Value == null ? string.Empty : dataGridView1.CurrentRow.Cells["Barcode"].Value.ToString();
                //    decimal quantity = dataGridView1.CurrentRow.Cells["Quantity"].Value == null || dataGridView1.CurrentRow.Cells["Quantity"].Value == string.Empty ? 0 : Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Quantity"].Value);
                //    int UnitCode = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? 1 : Convert.ToInt32(dataGridView1.CurrentRow.Cells["UnitCode"].Value);
                //    string description = dataGridView1.CurrentRow.Cells["Description"].Value == null ? string.Empty : dataGridView1.CurrentRow.Cells["Description"].Value.ToString();
                //    if (location == string.Empty && barcode == string.Empty && quantity == 0 && UnitCode == 1 && description == string.Empty)
                //    {

                //    }
                //    else
                //    {
                //        DialogResult dialogresult = MessageBox.Show(MessageConstants.DoyouwanttochangeScanmode, MessageConstants.TitleClearDataConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //        if (dialogresult == DialogResult.Yes)
                //        {
                //            dataGridView1.CurrentRow.Cells["LocationCode"].Value = string.Empty;
                //            dataGridView1.CurrentRow.Cells["Barcode"].Value = string.Empty;
                //            dataGridView1.CurrentRow.Cells["Quantity"].Value = string.Empty;
                //            dataGridView1.CurrentRow.Cells["UnitCode"].Value = 1;
                //            dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                //            dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                //            dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                //            dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                //            dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = string.Empty;
                //        }
                //        else
                //        {
                //            dataGridView1.CurrentRow.Cells["ScanMode_"].Value = scanMode;
                //        }
                //    }
                //}


                //else
                //{
                //    var newQuantity = dataGridView1.CurrentRow.Cells["NewQuantity"].Value == null ? 0 : Convert.ToDecimal(dataGridView1.CurrentRow.Cells["newQuantity"].Value);

                //    var flag = dataGridView1.CurrentRow.Cells["Flag"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Flag"].Value.ToString();
                //    var description = dataGridView1.CurrentRow.Cells["Description"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Description"].Value.ToString();
                //    if (valueAfterEdit == string.Empty)

                //    {
                //        if (flag == "F" || flag == "U")

                //        {


                //        }
                //        else if ((flag == "I" || flag == "E") && newQuantity == 0)

                //        {




                //        }
                //        else
                //        {
                //            MessageBox.Show(dataGridView1.CurrentCell.OwningColumn.Name + MessageConstants.Cannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //        }
                //    }
                //        )

                else if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("Quantity"))
                {
                    var resultQuantity = string.Empty;
                    var quantity = dataGridView1.CurrentRow.Cells["Quantity"].Value == null ? 0 : Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Quantity"].Value);
                    var scanMode = dataGridView1.CurrentRow.Cells["ScanMode_"].Value == null ? "1" : dataGridView1.CurrentRow.Cells["ScanMode_"].Value.ToString();
                    var unitCode = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? 1 : (int)dataGridView1.CurrentRow.Cells["UnitCode"].Value;
                    if (unitCode == 5)
                    {
                        dataGridView1.CurrentRow.Cells["Quantity"].Value = String.Format("{0:#,0.000}", (decimal)quantity);
                    }

                    else
                    {












                        dataGridView1.CurrentRow.Cells["Quantity"].Value = String.Format("{0:#,0}", Math.Ceiling(quantity));
                        //if (quantity.ToString().Contains('.'))

                        //{
                        //    var dotPosition = quantity.ToString().IndexOf('.');
                        //    var afterDot = string.Empty;
                        //    if (dotPosition != -1)

                        //    {
                        //        afterDot = quantity.ToString().Substring(dotPosition);

                        //    }
                        //    if (afterDot == ".000")

                        //    {
                        //        dataGridView1.CurrentRow.Cells["Quantity"].Value = String.Format("{0:#,0.###}", (decimal)quantity);



                        //    }
                        //    else
                        //    {
                        //        dataGridView1.CurrentRow.Cells["Quantity"].Value = String.Format("{0:#,0.000}", (decimal)quantity);




                        //    }
                        //}
                        //else
                        //{
                        //    dataGridView1.CurrentRow.Cells["Quantity"].Value = String.Format("{0:#,0.###}", (decimal)quantity);


                        //}
                    }

                }
                else if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("NewQuantity"))
                {
                    var newQuantity = dataGridView1.CurrentRow.Cells["NewQuantity"].Value == null ? 0 : Convert.ToDecimal(dataGridView1.CurrentRow.Cells["NewQuantity"].Value);
                    var scanMode = dataGridView1.CurrentRow.Cells["ScanMode_"].Value == null ? "1" : dataGridView1.CurrentRow.Cells["ScanMode_"].Value.ToString();
                    var unitCode = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? 1 : (int)dataGridView1.CurrentRow.Cells["UnitCode"].Value;
                    if (unitCode == 5)
                    {
                        dataGridView1.CurrentRow.Cells["NewQuantity"].Value = String.Format("{0:#,0.000}", (decimal)newQuantity);
                    }

                    else
                    {









                        dataGridView1.CurrentRow.Cells["NewQuantity"].Value = String.Format("{0:#,0}", Math.Ceiling(newQuantity));
                        //if (newQuantity.ToString().Contains('.'))

                        //{
                        //    var dotPosition = newQuantity.ToString().IndexOf('.');
                        //    var afterDot = string.Empty;
                        //    if (dotPosition != -1)

                        //    {
                        //        afterDot = newQuantity.ToString().Substring(dotPosition);

                        //    }
                        //    if (afterDot == ".000")

                        //    {
                        //        dataGridView1.CurrentRow.Cells["NewQuantity"].Value = String.Format("{0:#,0.###}", (decimal)newQuantity);



                        //    }
                        //    else
                        //    {
                        //        dataGridView1.CurrentRow.Cells["NewQuantity"].Value = String.Format("{0:#,0.000}", (decimal)newQuantity);




                        //    }
                        //}
                        //else
                        //{
                        //    dataGridView1.CurrentRow.Cells["NewQuantity"].Value = String.Format("{0:#,0.###}", (decimal)newQuantity);


                        //}
                    }
                }

            }


            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var currentCell = dataGridView1.CurrentCell.Value == null ? "" : (dataGridView1.CurrentCell.Value.ToString()).Trim();
            if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("ScanMode"))
            {
                scanMode = dataGridView1.CurrentRow.Cells["ScanMode_"].Value == null ? 1 : Convert.ToInt16(dataGridView1.CurrentRow.Cells["ScanMode_"].Value);
            }

            if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("UnitCode"))
            {
                unitBefore = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? 0 : (int)(dataGridView1.CurrentRow.Cells["UnitCode"].Value);
            }

            if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("ConversionCounter"))
            {
                counterBefore = dataGridView1.CurrentRow.Cells["ConversionCounter"].Value == null ? "" : (string)(dataGridView1.CurrentRow.Cells["ConversionCounter"].Value);
            }

            if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("SerialNumber"))
            {
                serialBefore = dataGridView1.CurrentRow.Cells["SerialNumber"].Value == null ? "" : (string)(dataGridView1.CurrentRow.Cells["SerialNumber"].Value);
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {

                e.Control.KeyPress -= new KeyPressEventHandler(Column23_KeyPress); // ใส่ตัวอักษร
                e.Control.KeyPress -= new KeyPressEventHandler(Column56_KeyPress); // ใส่ตัวเลข
                e.Control.KeyPress -= new KeyPressEventHandler(Column8_KeyPress); // ใส่อะไรก็ได้
                e.Control.KeyUp -= new KeyEventHandler(Column56_KeyUp);

                //if (dataGridView1.CurrentCell.ColumnIndex == 0)

                //{
                //    ComboBox cb = e.Control as ComboBox;
                //    if (cb != null)

                //    {
                //        cb.SelectedIndexChanged -= new EventHandler(cb_SelectedIndexChanged);

                //        cb.SelectedIndexChanged += new EventHandler(cb_SelectedIndexChanged);


                //    }
                //}
                //else 
                if (dataGridView1.CurrentCell.ColumnIndex == 3 || dataGridView1.CurrentCell.ColumnIndex == 4 || dataGridView1.CurrentCell.ColumnIndex == 11 /* || dataGridView1.CurrentCell.ColumnIndex == 8 || dataGridView1.CurrentCell.ColumnIndex == 9*/ )
                {
                    TextBox tb1 = e.Control as TextBox;
                    if (tb1 != null)
                    {
                        tb1.KeyPress += new KeyPressEventHandler(Column23_KeyPress);
                    }
                }
                else if (dataGridView1.CurrentCell.ColumnIndex == 8 || dataGridView1.CurrentCell.ColumnIndex == 9)
                {//ใส่ตัวเลขที่เป็น format $$ มีลูกน้ำ
                    TextBox tb2 = e.Control as TextBox;
                    if (tb2 != null)
                    {
                        tb2.KeyPress += new KeyPressEventHandler(Column56_KeyPress);
                        tb2.KeyUp += new KeyEventHandler(Column56_KeyUp);
                    }
                }
                else
                {
                    TextBox tb3 = e.Control as TextBox;
                    if (tb3 != null)
                    {
                        tb3.KeyPress += new KeyPressEventHandler(Column8_KeyPress);
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void Column23_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void Column56_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                if (e.KeyChar == 46)
                {
                    if (((System.Windows.Forms.TextBox)(sender)).Text.Contains('.'))
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void Column56_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                var afterDot = string.Empty;
                decimal valFormatCurrency = 0;
                if (((System.Windows.Forms.TextBox)(sender)).Text != string.Empty)
                {
                    var subLastLength = ((System.Windows.Forms.TextBox)(sender)).Text.Substring(((System.Windows.Forms.TextBox)(sender)).Text.Length - 1);

                    var dotPosition = ((System.Windows.Forms.TextBox)(sender)).Text.IndexOf('.');


                    var maxDot = 0;
                    if (dotPosition != -1)
                    {
                        afterDot = ((System.Windows.Forms.TextBox)(sender)).Text.Substring(dotPosition);
                        maxDot = ((System.Windows.Forms.TextBox)(sender)).Text.Substring(dotPosition).Length;
                    }
                    if (maxDot > 4)
                    {
                        ((System.Windows.Forms.TextBox)(sender)).Text = ((System.Windows.Forms.TextBox)(sender)).Text.Substring(0, ((System.Windows.Forms.TextBox)(sender)).Text.Length - 1);
                        valFormatCurrency = Convert.ToDecimal(((System.Windows.Forms.TextBox)(sender)).Text);
                        if (afterDot.Length == 5 && afterDot.Substring(3, 1) == "0")
                        {
                            ((System.Windows.Forms.TextBox)(sender)).Text = String.Format("{0:#,0.##0}", valFormatCurrency);
                        }
                        else
                        {
                            ((System.Windows.Forms.TextBox)(sender)).Text = String.Format("{0:#,0.###}", valFormatCurrency);
                        }
                    }
                    else
                    {
                        var containDot = (((System.Windows.Forms.TextBox)(sender)).Text.Contains('.'));
                        if ((((System.Windows.Forms.TextBox)(sender)).Text.Length == 8 && containDot == false && subLastLength != "."))
                        {
                            ((System.Windows.Forms.TextBox)(sender)).Text = ((System.Windows.Forms.TextBox)(sender)).Text.Substring(0, ((System.Windows.Forms.TextBox)(sender)).Text.Length - 1);
                            valFormatCurrency = Convert.ToDecimal(((System.Windows.Forms.TextBox)(sender)).Text);
                            ((System.Windows.Forms.TextBox)(sender)).Text = String.Format("{0:#,0.###}", valFormatCurrency);
                        }
                        else
                        {
                            valFormatCurrency = Convert.ToDecimal(((System.Windows.Forms.TextBox)(sender)).Text);
                            if (subLastLength == "." || afterDot == ".0")
                            {

                            }
                            else
                            {
                                if (afterDot.Length == 4 && afterDot.Substring(3) == "0")
                                {
                                    ((System.Windows.Forms.TextBox)(sender)).Text = String.Format("{0:#,0.##0}", valFormatCurrency);
                                }
                                else if (afterDot.Length == 3 && afterDot.Substring(2) == "0")
                                {
                                    ((System.Windows.Forms.TextBox)(sender)).Text = String.Format("{0:#,0.#0#}", valFormatCurrency);
                                }
                                else if (afterDot.Length == 2 && afterDot.Substring(1) == "0")
                                {
                                    ((System.Windows.Forms.TextBox)(sender)).Text = String.Format("{0:#,0.0##}", valFormatCurrency);
                                }
                                else
                                {
                                    ((System.Windows.Forms.TextBox)(sender)).Text = String.Format("{0:#,0.###}", valFormatCurrency);
                                }

                            }
                        }
                    }
                }
                else
                {
                    ((System.Windows.Forms.TextBox)(sender)).Text = string.Empty;
                }
                var lengthVal = ((System.Windows.Forms.TextBox)(sender)).Text.Length;
                TextBox tb2 = ((System.Windows.Forms.TextBox)(sender)) as TextBox;
                tb2.SelectionStart = lengthVal;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void Column8_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("ScanMode"))
            {
                string scanMode = dataGridView1.CurrentRow.Cells["ScanMode_"].Value == null ? "1" : dataGridView1.CurrentRow.Cells["ScanMode_"].Value.ToString();
                string location = dataGridView1.CurrentRow.Cells["LocationCode"].Value == null ? string.Empty : dataGridView1.CurrentRow.Cells["LocationCode"].Value.ToString();
                string barcode = dataGridView1.CurrentRow.Cells["Barcode"].Value == null ? string.Empty : dataGridView1.CurrentRow.Cells["Barcode"].Value.ToString();
                decimal quantity = dataGridView1.CurrentRow.Cells["Quantity"].Value == null || dataGridView1.CurrentRow.Cells["Quantity"].Value == string.Empty ? 0 : Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Quantity"].Value);
                int UnitCode = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? 1 : Convert.ToInt32(dataGridView1.CurrentRow.Cells["UnitCode"].Value);
                string description = dataGridView1.CurrentRow.Cells["Description"].Value == null ? string.Empty : dataGridView1.CurrentRow.Cells["Description"].Value.ToString();
                if (location == string.Empty && barcode == string.Empty && quantity == 0 && UnitCode == 1 && description == string.Empty)
                {

                }
                else
                {
                    DialogResult dialogresult = MessageBox.Show(MessageConstants.DoyouwanttochangeScanmode, MessageConstants.TitleClearDataConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogresult == DialogResult.Yes)
                    {
                        dataGridView1.CurrentRow.Cells["LocationCode"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["Barcode"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["Quantity"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["UnitCode"].Value = 1;
                        dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["DepartmentCode"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["MKCode"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["ProductType"].Value = string.Empty;
                    }
                    else
                    {
                        dataGridView1.CurrentRow.Cells["ScanMode_"].Value = scanMode;
                    }
                }
                ////if(scanMode == "1")
                ////{
                ////    dataGridView1.CurrentRow.Cells["ScanMode_"].ReadOnly = true;
                ////}
                ////else {
                ////    dataGridView1.CurrentRow.Cells["ScanMode_"].ReadOnly = false;
                ////}

            }
        }
        #endregion

        private void btnSummary_Click(object sender, EventArgs e)
        {
            EditQtyModel.Request searchCondition = GetSearchCondition();


            Loading_Screen.ShowSplashScreen();
            var listSummary = auditBLL.GetAuditHHTToPCSummary(searchCondition);
            Loading_Screen.CloseForm();
            if (listSummary.Rows.Count == 0)
            {
                MessageBox.Show(MessageConstants.NoAuditdatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                EditQTYSummaryForm fSummary = new EditQTYSummaryForm(); // Instantiate a Form3 object.
                fSummary.SummaryEditQty(listSummary);
                fSummary.ShowDialog();
            }
        }

        private void btnViewStocktakingReport_Click(object sender, EventArgs e)
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
        }

        private DataTable GetReport_DeleteRecordReportByLocation(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                ParameterField paramField1 = new ParameterField();
                ParameterField paramField2 = new ParameterField();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramFields.Add(paramField1);

                paramField2.Name = "pBranchName";
                paramDiscreteValue2.Value = reportParam.BranchName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramFields.Add(paramField2);

                paramField3.Name = "pReportName";
                paramDiscreteValue3.Value = ReportName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField3);

                return bllReportManagement.LoadReport_DeleteRecordReportByLocation(reportParam.CountDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_StocktakingAuditAdjustWithUnit(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                DateTime countDate = reportParam.CountDate;
                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();
                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                paramField.Name = "pCountDate";
                paramDiscreteValue.Value = reportParam.CountDate;
                paramField.CurrentValues.Add(paramDiscreteValue);
                paramField1.Name = "pBranchName";
                paramDiscreteValue1.Value = reportParam.BranchName;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pReportName";
                paramDiscreteValue2.Value = ReportName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramFields.Add(paramField2);
                paramFields.Add(paramField);
                paramFields.Add(paramField1);

                return bllReportManagement.LoadReport_StocktakingAuditAdjustWithUnit(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private void btnViewControlSheetReport_Click(object sender, EventArgs e)
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
        }


        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {

        }


        private void btnSummaryMKCode_Click(object sender, EventArgs e)
        {
            EditQtyModel.Request searchCondition = GetSearchCondition();
            Loading_Screen.ShowSplashScreen();
            var listSummary = auditBLL.GetAuditHHTToPCSummaryMKCode(searchCondition);
            Loading_Screen.CloseForm();
            if (listSummary.Count == 0)
            {
                MessageBox.Show(MessageConstants.NoAuditdatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                EditQTYSummaryMKCodeForm fSummary = new EditQTYSummaryMKCodeForm(); // Instantiate a Form3 object.
                fSummary.SummaryMKCodeEditQty(listSummary, listUnit);
                fSummary.Show();
            }
        }


        protected void AddDropDownCountSheet()
        {
            try
            {
                //Get DropDown
                List<string> listCountSheet = new List<string>();
                string plant = comboBoxPlant.Text;
                listCountSheet = settingBLL.GetDropDownCountSheetSKU(plant);

                comboBoxCountSheet.Items.Clear();
                comboBoxCountSheet.Items.Add("All");
                if (listCountSheet.Count > 0)
                {
                    foreach (var l in listCountSheet)
                    {
                        comboBoxCountSheet.Items.Add(l);
                    }
                }
                comboBoxCountSheet.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        protected void AddDropDownStorageLocation()
        {
            try
            {
                List<MasterStorageLocation> listType = new List<MasterStorageLocation>();

                listType = bll.GetListMasterStorageLocation();
                comboBoxStorageLocation.Items.Clear();
                comboBoxStorageLocation.Items.Add("All");
                foreach (MasterStorageLocation m in listType)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = m.StorageLocationCode + " - " + m.StorageLocationName;
                    item.Value = m.StorageLocationCode;

                    comboBoxStorageLocation.Items.Add(item);
                }
                comboBoxStorageLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void comboBoxPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddDropDownCountSheet();
            AddDropDownStorageLocation();
        }

        protected void AddDropDownMCHLevel1()
        {
            try
            {
                lbMCH1.Text = settingBLL.GetSettingStringByKey("MCHLevel1");


                //Get DropDown
                List<string> listMCH = new List<string>();
                String countSheet = comboBoxCountSheet.Text;
                comboBoxMCH1.Items.Clear();
                comboBoxMCH1.Items.Add("All");
                if (!string.IsNullOrEmpty(countSheet))
                {
                    listMCH = settingBLL.GetDropDownMCH1(countSheet);
                    if (listMCH.Count > 0)
                    {
                        foreach (var m in listMCH)
                        {
                            comboBoxMCH1.Items.Add(m);



                        }
                    }
                }

                comboBoxMCH1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        protected void AddDropDownMCHLevel2()
        {
            try
            {
                lbMCH2.Text = settingBLL.GetSettingStringByKey("MCHLevel2");

                //Get DropDown
                List<string> listMCH = new List<string>();
                String countSheet = comboBoxCountSheet.Text;
                String hLevel1 = comboBoxMCH1.Text;
                comboBoxMCH2.Items.Clear();
                comboBoxMCH2.Items.Add("All");
                if (!string.IsNullOrEmpty(countSheet) || !string.IsNullOrEmpty(hLevel1))
                {
                    if (string.IsNullOrEmpty(countSheet))
                    {
                        countSheet = "All";
                    }
                    if (string.IsNullOrEmpty(hLevel1))
                    {
                        hLevel1 = "All";
                    }

                    listMCH = settingBLL.GetDropDownMCH2(countSheet, hLevel1);
                    if (listMCH.Count > 0)
                    {
                        foreach (var m in listMCH)
                        {
                            comboBoxMCH2.Items.Add(m);
                        }
                    }
                }
                comboBoxMCH2.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        protected void AddDropDownMCHLevel3()
        {
            try
            {
                //Get Setting Label
                lbMCH3.Text = settingBLL.GetSettingStringByKey("MCHLevel3");

                //Get DropDown
                List<string> listMCH = new List<string>();
                String countSheet = comboBoxCountSheet.Text;
                String hLevel1 = comboBoxMCH1.Text;
                String hLevel2 = comboBoxMCH2.Text;
                comboBoxMCH3.Items.Clear();
                comboBoxMCH3.Items.Add("All");
                if (!string.IsNullOrEmpty(countSheet) || !string.IsNullOrEmpty(hLevel1) || !string.IsNullOrEmpty(hLevel2))
                {
                    if (string.IsNullOrEmpty(countSheet))
                    {
                        countSheet = "All";
                    }
                    if (string.IsNullOrEmpty(hLevel1))
                    {
                        hLevel1 = "All";
                    }
                    if (string.IsNullOrEmpty(hLevel2))
                    {
                        hLevel2 = "All";
                    }
                    listMCH = settingBLL.GetDropDownMCH3(countSheet, hLevel1, hLevel2);
                    if (listMCH.Count > 0)
                    {
                        foreach (var m in listMCH)
                        {
                            comboBoxMCH3.Items.Add(m);
                        }

                    }
                }


                comboBoxMCH3.SelectedIndex = 0;
                //comboBoxMCH3.Items.Add("");
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        protected void AddDropDownMCHLevel4()
        {
            try
            {
                //Get Setting Label
                lbMCH4.Text = settingBLL.GetSettingStringByKey("MCHLevel4");

                //Get DropDown
                List<string> listMCH = new List<string>();
                String countSheet = comboBoxCountSheet.Text;
                String hLevel1 = comboBoxMCH1.Text;
                String hLevel2 = comboBoxMCH2.Text;
                String hLevel3 = comboBoxMCH3.Text;
                comboBoxMCH4.Items.Clear();
                comboBoxMCH4.Items.Add("All");
                if (!string.IsNullOrEmpty(countSheet) || !string.IsNullOrEmpty(hLevel1)
                    || !string.IsNullOrEmpty(hLevel2) || !string.IsNullOrEmpty(hLevel3))
                {

                    if (string.IsNullOrEmpty(countSheet))
                    {
                        countSheet = "All";
                    }
                    if (string.IsNullOrEmpty(hLevel1))
                    {
                        hLevel1 = "All";
                    }
                    if (string.IsNullOrEmpty(hLevel2))
                    {
                        hLevel2 = "All";
                    }
                    if (string.IsNullOrEmpty(hLevel3))
                    {
                        hLevel3 = "All";
                    }

                    listMCH = settingBLL.GetDropDownMCH4(countSheet, hLevel1, hLevel2, hLevel3);
                    if (listMCH.Count > 0)
                    {
                        foreach (var m in listMCH)
                        {
                            comboBoxMCH4.Items.Add(m);
                        }

                    }
                }

                comboBoxMCH4.SelectedIndex = 0;
                //comboBoxMCH4.Items.Add("");

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void btnReportSerial_Click(object sender, EventArgs e)
        {
            try
            {
                EditQtyModel.Request searchCondition = GetSearchCondition();
                List<EditQtyModel.ResponseSerialNumberReport> listSerialNumberData = auditBLL.GetSerialNumberData(searchCondition);
                SerialNumberReportForm serialNumberReport = new SerialNumberReportForm();

                if (listSerialNumberData.Count() == 0)
                {
                    MessageBox.Show(MessageConstants.Nodatadeleterecordreport, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Loading_Screen.ShowSplashScreen();
                    bool isCreateReportSuccess = serialNumberReport.CreateReport(listSerialNumberData, countDate, branchName);
                    Loading_Screen.CloseForm();
                    if (isCreateReportSuccess)
                    {
                        serialNumberReport.StartPosition = FormStartPosition.CenterParent;
                        DialogResult dialogResult = serialNumberReport.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.cannotgeneratereport, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);

                if (dataGridView1.CurrentCell.ColumnIndex == 7)
                {
                    var unit = dataGridView1.CurrentRow.Cells[7].Value == null ? 0 : (int)dataGridView1.CurrentRow.Cells[7].Value;

                    if (unit == unitPackValue)
                    {
                        dataGridView1.CurrentRow.Cells["ConversionCounter"].Style.BackColor = Color.White;
                        dataGridView1.CurrentRow.Cells["ConversionCounter"].ReadOnly = false;
                    }
                    else
                    {
                        dataGridView1.CurrentRow.Cells["ConversionCounter"].Value = null;
                        dataGridView1.CurrentRow.Cells["ConversionCounter"].Style.BackColor = Color.LightGray;
                        dataGridView1.CurrentRow.Cells["ConversionCounter"].ReadOnly = true;
                    }


                    //var scanMode = dataGridView1.CurrentRow.Cells["ScanMode_"].Value == null ? "" : dataGridView1.CurrentRow.Cells["ScanMode_"].Value.ToString();
                    //var barcode = dataGridView1.CurrentRow.Cells["Barcode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Barcode"].Value.ToString();
                    //var location = dataGridView1.CurrentRow.Cells["LocationCode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["LocationCode"].Value.ToString();
                    //var unitCode = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? 0 : (int)dataGridView1.CurrentRow.Cells["UnitCode"].Value;
                    //var serialNumber = dataGridView1.CurrentRow.Cells["SerialNumber"].Value == null ? "" : dataGridView1.CurrentRow.Cells["SerialNumber"].Value.ToString();

                    //if (barcode != string.Empty && location != string.Empty)
                    //{
                    //    var stocktakingID = dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null ? "" : dataGridView1.CurrentRow.Cells["StocktakingID"].Value.ToString();
                    //    var flag = dataGridView1.CurrentRow.Cells["Flag"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Flag"].Value.ToString();
                    //    var resultBarcodeInMasterSKU = auditBLL.GetDescriptionInMasterSKU(barcode, location, stocktakingID, countDate,  unitCode, flag,serialNumber);
                    //    //if (resultBarcodeInMasterSKU == null)
                    //    //{
                    //    //    MessageBox.Show(MessageConstants.BarcodenotexistinMaster, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    //    dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                    //    //    dataGridView1.CurrentRow.Cells["Plant"].Value = string.Empty;
                    //    //    dataGridView1.CurrentRow.Cells["StorageLocation"].Value = string.Empty;

                    //    //    for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                    //    //    {
                    //    //        SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                    //    //    }
                    //    //}
                    //    //else
                    //    //{
                    //        if (resultBarcodeInMasterSKU.Result == "DuplicateBarcode")
                    //        {
                    //            MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //            dataGridView1.CurrentRow.Cells["UnitCode"].Value = unitBefore;

                    //            for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                    //            {
                    //                var chkUnit = dataGridView1.Rows[count].Cells["UnitCode"].Value == null ? 0 : (int)dataGridView1.Rows[count].Cells["UnitCode"].Value;
                    //                var chkLocation = dataGridView1.Rows[count].Cells["LocationCode"].Value == null ? "" : dataGridView1.Rows[count].Cells["LocationCode"].Value.ToString();
                    //                var chkBarcode = dataGridView1.Rows[count].Cells["Barcode"].Value == null ? "" : dataGridView1.Rows[count].Cells["Barcode"].Value.ToString();

                    //                if (location == chkLocation && barcode == chkBarcode /* && scanMode == "3"*/ && unitCode == chkUnit)
                    //                {
                    //                    dataGridView1.Rows[count].Cells["No"].Style.BackColor = Color.Red;
                    //                    dataGridView1.Rows[count].Cells["Plant"].Style.BackColor = Color.Red;
                    //                    dataGridView1.Rows[count].Cells["StorageLocation"].Style.BackColor = Color.Red;
                    //                    dataGridView1.Rows[count].Cells["LocationCode"].Style.BackColor = Color.Red;
                    //                    dataGridView1.Rows[count].Cells["Barcode"].Style.BackColor = Color.Red;
                    //                    dataGridView1.Rows[count].Cells["SKUCode"].Style.BackColor = Color.Red;
                    //                    dataGridView1.Rows[count].Cells["Quantity"].Style.BackColor = Color.Red;
                    //                    dataGridView1.Rows[count].Cells["NewQuantity"].Style.BackColor = Color.Red;
                    //                    dataGridView1.Rows[count].Cells["ConversionCounter"].Style.BackColor = Color.Red;
                    //                    dataGridView1.Rows[count].Cells["ComputerName"].Style.BackColor = Color.Red;
                    //                    dataGridView1.Rows[count].Cells["UnitCode"].Style.BackColor = Color.Red;
                    //                    dataGridView1.Rows[count].Cells["FLAG"].Style.BackColor = Color.Red;
                    //                    dataGridView1.Rows[count].Cells["Description"].Style.BackColor = Color.Red;
                    //                    dataGridView1.Rows[count].Cells["Remark"].Style.BackColor = Color.Red;
                    //                    dataGridView1.Rows[count].Cells["SerialNumber"].Style.BackColor = Color.Red;
                    //                }
                    //                else
                    //                {
                    //                    SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                    //                }
                    //            }
                    //            //if (dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null)
                    //            //{
                    //            //    dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                    //            //    int num = 1;
                    //            //    for (int count = 0; count < dataGridView1.Rows.Count; count++)
                    //            //    {
                    //            //        dataGridView1.Rows[count].Cells["No"].Value = num;
                    //            //        num++;
                    //            //    }
                    //            //}
                    //        }
                    //        else
                    //        {
                    //           // string scanModeCurrentRow = dataGridView1.CurrentRow.Cells["ScanMode_"].Value.ToString();
                    //            int countDupWarehouseUnit = 0;
                    //            int countDupHightlight = 0;
                    //            var unitCodeOld = string.Empty;
                    //            int num1 = 0;
                    //            int countDup = 0;
                    //            for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                    //            {
                    //                var chkUnit = dataGridView1.Rows[count].Cells["UnitCode"].Value == null ? 0 : (int)dataGridView1.Rows[count].Cells["UnitCode"].Value;
                    //                var stock = dataGridView1.Rows[count].Cells["StocktakingID"].Value == null ? "" : dataGridView1.Rows[count].Cells["StocktakingID"].Value;
                    //                var chkLocation = dataGridView1.Rows[count].Cells["LocationCode"].Value == null ? "" : dataGridView1.Rows[count].Cells["LocationCode"].Value.ToString();
                    //                var chkBarcode = dataGridView1.Rows[count].Cells["Barcode"].Value == null ? "" : dataGridView1.Rows[count].Cells["Barcode"].Value.ToString();

                    //                if (location == chkLocation && barcode == chkBarcode && unitCode == chkUnit)
                    //                {
                    //                    countDup++;
                    //                }

                    //                if (countDupHightlight == 0)
                    //                {
                    //                    SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                    //                }
                    //            }
                    //        }
                    //    //}
                    //}
                }
            }
        }


        private bool IsDuplicateInGrid()
        {
            bool result = false;

            int numRow = dataGridView1.Rows.Count;
            List<EditQtyModel.Response> listGrid = new List<EditQtyModel.Response>();

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                EditQtyModel.Response DataDto = new EditQtyModel.Response();
                DataDto.No = dataGridView1.Rows[i].Cells["No"].Value == null ? 0 : ((int)dataGridView1.Rows[i].Cells["No"].Value);
                DataDto.Plant = (string)dataGridView1.Rows[i].Cells["Plant"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["Plant"].Value).Trim();
                DataDto.StorageLocation = (string)dataGridView1.Rows[i].Cells["StorageLocation"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["StorageLocation"].Value).Trim();
                DataDto.LocationCode = (string)dataGridView1.Rows[i].Cells["LocationCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["LocationCode"].Value).Trim();
                DataDto.Barcode = dataGridView1.Rows[i].Cells["Barcode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["BarCode"].Value).Trim();
                DataDto.SKUCode = dataGridView1.Rows[i].Cells["SKUCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["SKUCode"].Value).Trim();
                DataDto.SerialNumber = dataGridView1.Rows[i].Cells["SerialNumber"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["SerialNumber"].Value).Trim();
                DataDto.UnitCode = dataGridView1.Rows[i].Cells["UnitCode"].Value == null ? 0 : ((int)dataGridView1.Rows[i].Cells["UnitCode"].Value);
                DataDto.Quantity = dataGridView1.Rows[i].Cells["Quantity"].Value == null ? 0 : Convert.ToDecimal(dataGridView1.Rows[i].Cells["Quantity"].Value);
                DataDto.NewQuantity = dataGridView1.Rows[i].Cells["NewQuantity"].Value == null ? 0 : Convert.ToDecimal(dataGridView1.Rows[i].Cells["NewQuantity"].Value);
                DataDto.ConversionCounter = dataGridView1.Rows[i].Cells["ConversionCounter"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ConversionCounter"].Value).Trim();
                DataDto.ComputerName = dataGridView1.Rows[i].Cells["ComputerName"].Value == null ? compName : ((string)dataGridView1.Rows[i].Cells["ComputerName"].Value).Trim();
                DataDto.DepartmentCode = dataGridView1.Rows[i].Cells["DepartmentCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["DepartmentCode"].Value).Trim();
                DataDto.Description = dataGridView1.Rows[i].Cells["Description"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["Description"].Value).Trim();
                DataDto.Remark = (string)dataGridView1.Rows[i].Cells["Remark"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["Remark"].Value).Trim();
                DataDto.StocktakingID = (string)dataGridView1.Rows[i].Cells["StocktakingID"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["StocktakingID"].Value).Trim();
                DataDto.InBarCode = dataGridView1.Rows[i].Cells["InBarCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["InBarCode"].Value).Trim();
                DataDto.ExBarCode = dataGridView1.Rows[i].Cells["ExBarCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ExBarCode"].Value).Trim();
                DataDto.DepartmentCode = dataGridView1.Rows[i].Cells["DepartmentCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["DepartmentCode"].Value).Trim();
                DataDto.MKCode = dataGridView1.Rows[i].Cells["MKCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["MKCode"].Value).Trim();
                DataDto.ProductType = dataGridView1.Rows[i].Cells["ProductType"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ProductType"].Value).Trim();
                DataDto.SKUMode = dataGridView1.Rows[i].Cells["SKUMode"].Value == null ? false : Convert.ToBoolean(dataGridView1.Rows[i].Cells["SKUMode"].Value);
                DataDto.ChkSKUCode = dataGridView1.Rows[i].Cells["ChkSKUCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ChkSKUCode"].Value).Trim();
                DataDto.ScanMode = dataGridView1.Rows[i].Cells["ScanMode_"].Value == null ? 0 : ((int)dataGridView1.Rows[i].Cells["ScanMode_"].Value);
                DataDto.oldSerialNumber = dataGridView1.Rows[i].Cells["oldSerialNumber"].Value == null ? "" : dataGridView1.Rows[i].Cells["oldSerialNumber"].Value.ToString();
                DataDto.Flag = dataGridView1.Rows[i].Cells["Flag"].Value == null ? "" : dataGridView1.Rows[i].Cells["Flag"].Value.ToString();
                listGrid.Add(DataDto);
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var barcode = row.Cells["Barcode"].Value == null ? "" : (row.Cells["BarCode"].Value.ToString()).Trim();
                var location = row.Cells["LocationCode"].Value == null ? "" : (row.Cells["LocationCode"].Value.ToString()).Trim();
                var unit = row.Cells["UnitCode"].Value == null ? 0 : (int)row.Cells["LocationCode"].Value;
                var no = row.Cells["No"].Value == null ? 0 : (int)row.Cells["No"].Value;

                if (listGrid.Any(x => x.LocationCode == location && x.Barcode == barcode && x.UnitCode == unit && x.No != no))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void EditQuantityForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool result = false;
            GetGridViewAuditList();

            if (deleteList.Count > 0 || updateList.Count > 0 || updateSKUCodeList.Count > 0 || insertList.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show(MessageConstants.Doyouwanttosaveallsectionbelow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            if (result)
            {
                if (IsConversionCounterNull())
                {
                    MessageBox.Show(MessageConstants.ConversionCounterCannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
                else
                {
                    bool includeNull = listResult.Any(x => x.LocationCode == string.Empty || x.Barcode == string.Empty || x.Quantity == 0);

                    if (includeNull)
                    {
                        MessageBox.Show(MessageConstants.LocationBarcodeQuantityDescriptionCannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        Save(insertList, updateList, updateSKUCodeList, deleteList);
                    }
                }
            }
        }


        //protected override void OnFormClosing(FormClosingEventArgs e)
        //{
        //    bool result = false;
        //    GetGridViewAuditList();

        //    if (deleteList.Count > 0 || updateList.Count > 0 || updateSKUCodeList.Count > 0 || insertList.Count > 0)
        //    {
        //        DialogResult dialogResult = MessageBox.Show(MessageConstants.Doyouwanttosaveallsectionbelow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //        if (dialogResult == DialogResult.Yes)
        //        {
        //            result = true;
        //        }
        //        else
        //        {
        //            result = false;
        //        }
        //    }

        //    if (result)
        //    {
        //        if (IsConversionCounterNull())
        //        {
        //            MessageBox.Show(MessageConstants.ConversionCounterCannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            e.Cancel = true;
        //            return;
        //        }
        //        else
        //        {
        //            bool includeNull = listResult.Any(x => x.LocationCode == string.Empty || x.Barcode == string.Empty || x.Quantity == 0 || x.Description == string.Empty);//|| x.BrandCode == string.Empty);

        //            if (includeNull)
        //            {
        //                MessageBox.Show(MessageConstants.LocationBarcodeQuantityDescriptionCannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                e.Cancel = true;
        //                return;
        //            }
        //            else
        //            {
        //                Save(insertList, updateList, updateSKUCodeList, deleteList);
        //            }
        //        }
        //    }
        //}
    }
}
