using CrystalDecisions.Shared;
using FSBT.HHT.App.Resources;
using FSBT_HHT_BLL;
using FSBT_HHT_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSBT.HHT.App.UI
{
    public partial class EditQuantityForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        public string UserName { get; set; }
        AuditManagementBll auditBLL = new AuditManagementBll();
        SystemSettingBll settingBLL = new SystemSettingBll();
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
        int scanMode = 1;

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
                countDate = settingData.CountDate;
                ComName = settingData.ComID + "|" + settingData.ComName;
                branchName = settingData.Branch;
                var sectionType = settingData.SectionType;
                if (sectionType != string.Empty)
                {
                    var listSectionType = sectionType.Split('|').ToList();
                    foreach (var item in listSectionType)
                    {
                        if (item == "1")
                        {
                            chLocationType1.Checked = true;
                        }
                        else if (item == "2")
                        {
                            chLocationType2.Checked = true;
                        }
                        else if (item == "3")
                        {
                            chLocationType3.Checked = true;
                        }
                        else if (item == "4")
                        {
                            chLocationType4.Checked = true;
                        }
                    }
                }

                dataGridView1.Rows[0].Cells["No"].ReadOnly = true;
                dataGridView1.Rows[0].Cells["SKUCode"].ReadOnly = true;
                dataGridView1.Rows[0].Cells["NewQuantity"].ReadOnly = true;
                dataGridView1.Rows[0].Cells["Flag"].ReadOnly = true;
                dataGridView1.Rows[0].Cells["Description"].ReadOnly = true;

                var listScanMode = auditBLL.GetMasterScanMode();
                listUnit = auditBLL.GetMasterUnit();

                #region AddColumnScanMode Index1
                DataGridViewComboBoxColumn ColumnScanMode = new DataGridViewComboBoxColumn();
                ColumnScanMode.DataPropertyName = "ScanMode";
                ColumnScanMode.Name = "ScanMode";
                ColumnScanMode.HeaderText = "Scan Mode";
                ColumnScanMode.Width = 120;
                ColumnScanMode.DataSource = listScanMode;
                ColumnScanMode.ValueMember = "ScanModeID";
                ColumnScanMode.DisplayMember = "ScanModeName";
                dataGridView1.Columns.Insert(1, ColumnScanMode);
                (dataGridView1.Rows[0].Cells["ScanMode"] as DataGridViewComboBoxCell).Value = 1;
                #endregion

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
                dataGridView1.Rows[0].Cells["SKUCode"].Style.BackColor = Color.LightGray;
                dataGridView1.Rows[0].Cells["NewQuantity"].Style.BackColor = Color.LightGray;
                dataGridView1.Rows[0].Cells["Description"].Style.BackColor = Color.LightGray;
                dataGridView1.Rows[0].Cells["Remark"].Style.BackColor = Color.LightGray;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
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
                    (dataGridView1.Rows[0].Cells["ScanMode"] as DataGridViewComboBoxCell).Value = 1;
                    (dataGridView1.Rows[0].Cells["UnitCode"] as DataGridViewComboBoxCell).Value = 1;
                    dataGridView1.Rows[0].Cells["SKUCode"].Style.BackColor = Color.LightGray;
                    dataGridView1.Rows[0].Cells["NewQuantity"].Style.BackColor = Color.LightGray;
                    dataGridView1.Rows[0].Cells["Description"].Style.BackColor = Color.LightGray;
                    dataGridView1.Rows[0].Cells["Remark"].Style.BackColor = Color.LightGray;
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
                        this.dataGridView1.Rows.Add(result.No, result.ScanMode, result.LocationCode, result.Barcode, result.SKUCode, result.Quantity, result.NewQuantity, result.UnitCode, result.Flag, result.Description, result.Remark, result.StocktakingID, result.InBarCode, result.ExBarCode, result.DepartmentCode, result.MKCode, result.ProductType, result.SKUMode, result.ChkSKUCode);
                        dataGridView1.Rows[i].Cells["No"].Value = runningNo;
                        dataGridView1.Rows[i].Cells["No"].ReadOnly = true;
                        (dataGridView1.Rows[i].Cells["ScanMode"] as DataGridViewComboBoxCell).Value = result.ScanMode;
                        (dataGridView1.Rows[i].Cells["UnitCode"] as DataGridViewComboBoxCell).Value = result.UnitCode;
                        var quantity = dataGridView1.Rows[i].Cells["Quantity"].Value == null ? 0 : (decimal)dataGridView1.Rows[i].Cells["Quantity"].Value;
                        var newQuantity = dataGridView1.Rows[i].Cells["NewQuantity"].Value == null ? 0 : (decimal)dataGridView1.Rows[i].Cells["NewQuantity"].Value;
                        bool isIntQty = quantity == (int)quantity;
                        bool isIntNewQty = newQuantity == (int)newQuantity;

                        ////if (result.ScanMode == 1)
                        ////{
                        ////    dataGridView1.Rows[i].Cells["ScanMode"].ReadOnly = true;
                        ////}
                        ////else {
                        ////    dataGridView1.Rows[i].Cells["ScanMode"].ReadOnly = false;
                        ////}
                        if (quantity.ToString() == "0.00000000")
                        {
                            dataGridView1.Rows[i].Cells["Quantity"].Value = null;
                        }
                        else
                        {
                            if (result.ScanMode == 4 && result.UnitCode == 5)
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
                            if (result.ScanMode == 4 && result.UnitCode == 5)
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
                    dataGridView1.Rows[rowCount].Cells["No"].Value = lastDataNo + 1;
                    dataGridView1.Rows[rowCount].Cells["No"].ReadOnly = true;
                    dataGridView1.Rows[rowCount].Cells["ScanMode"].ReadOnly = false;
                    dataGridView1.Rows[rowCount].Cells["LocationCode"].ReadOnly = false;
                    dataGridView1.Rows[rowCount].Cells["BarCode"].ReadOnly = false;
                    dataGridView1.Rows[rowCount].Cells["Quantity"].ReadOnly = false;
                    dataGridView1.Rows[rowCount].Cells["SKUCode"].Style.BackColor = Color.LightGray;
                    dataGridView1.Rows[rowCount].Cells["SKUCode"].ReadOnly = true;
                    dataGridView1.Rows[rowCount].Cells["NewQuantity"].Style.BackColor = Color.LightGray;
                    dataGridView1.Rows[rowCount].Cells["NewQuantity"].ReadOnly = true;
                    dataGridView1.Rows[rowCount].Cells["UnitCode"].ReadOnly = false;
                    dataGridView1.Rows[rowCount].Cells["Description"].Style.BackColor = Color.LightGray;
                    dataGridView1.Rows[rowCount].Cells["Remark"].Style.BackColor = Color.LightGray;
                    dataGridView1.Rows[rowCount].Cells["Flag"].ReadOnly = true;
                    dataGridView1.Rows[rowCount].Cells["Description"].ReadOnly = true;
                    (dataGridView1.Rows[rowCount].Cells["ScanMode"] as DataGridViewComboBoxCell).Value = 1;
                    (dataGridView1.Rows[rowCount].Cells["UnitCode"] as DataGridViewComboBoxCell).Value = 1;
                    Loading_Screen.CloseForm();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                GetGridViewAuditList();
                bool includeNull = listResult.Any(x => x.LocationCode == string.Empty || x.Barcode == string.Empty || x.Quantity == 0 || x.Description == string.Empty);//|| x.BrandCode == string.Empty);

                if (includeNull)
                {
                    if (listResult.Any(x => x.Description == string.Empty))
                    {
                        includeNull = listResult.Where(x => (x.Flag != "F" && x.Flag != "U") && x.Description == string.Empty).Any();
                        var resultNullSKUMode = listResult.Where(x => (x.Flag == "I" && x.SKUMode == false) && x.Description == string.Empty).ToList();
                        if (resultNullSKUMode.Count() > 0)
                        {
                            includeNull = false;
                        }
                        if (includeNull)
                        {
                            includeNull = true;
                        }
                        else
                        {
                            includeNull = false;
                        }
                    }
                    else
                    {
                        includeNull = true;
                    }
                }

                if (includeNull)
                {
                    MessageBox.Show(MessageConstants.LocationBarcodeQuantityDescriptionCannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (deleteList.Count > 0 || updateList.Count > 0 || updateSKUCodeList.Count > 0 || insertList.Count > 0)
                    {
                        Save(insertList, updateList, updateSKUCodeList, deleteList);
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.Nochangeddata, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void btnViewDeleteReport_Click(object sender, EventArgs e)
        {
            try
            {
                EditQtyModel.Request searchCondition = GetSearchCondition();
                List<EditQtyModel.ResponseDeleteReport> listDeleteData = auditBLL.GetAuditHHTToPCDelete(searchCondition);
                DeleteQtyReportForm deleteQtyReport = new DeleteQtyReportForm();

                if (listDeleteData.Count() == 0)
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                    DataDto.StocktakingID = (string)dataGridView1.Rows[i].Cells["StocktakingID"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["StocktakingID"].Value).Trim();
                    DataDto.ScanMode = dataGridView1.Rows[i].Cells["ScanMode"].Value == null ? 0 : ((int)dataGridView1.Rows[i].Cells["ScanMode"].Value);
                    DataDto.LocationCode = (string)dataGridView1.Rows[i].Cells["LocationCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["LocationCode"].Value).Trim();
                    DataDto.Barcode = (string)dataGridView1.Rows[i].Cells["Barcode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["BarCode"].Value).Trim();
                    DataDto.Quantity = dataGridView1.Rows[i].Cells["Quantity"].Value == null ? 0 : Convert.ToDecimal(dataGridView1.Rows[i].Cells["Quantity"].Value);
                    DataDto.NewQuantity = dataGridView1.Rows[i].Cells["NewQuantity"].Value == null ? 0 : Convert.ToDecimal(dataGridView1.Rows[i].Cells["NewQuantity"].Value);
                    DataDto.UnitCode = dataGridView1.Rows[i].Cells["UnitCode"].Value == null ? 0 : ((int)dataGridView1.Rows[i].Cells["UnitCode"].Value);
                    DataDto.Flag = (string)dataGridView1.Rows[i].Cells["Flag"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["Flag"].Value).Trim();
                    DataDto.Description = dataGridView1.Rows[i].Cells["Description"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["Description"].Value).Trim();
                    DataDto.SKUCode = dataGridView1.Rows[i].Cells["SKUCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["SKUCode"].Value).Trim();
                    DataDto.InBarCode = dataGridView1.Rows[i].Cells["InBarCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["InBarCode"].Value).Trim();
                    DataDto.ExBarCode = dataGridView1.Rows[i].Cells["ExBarCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ExBarCode"].Value).Trim();
                    DataDto.DepartmentCode = dataGridView1.Rows[i].Cells["DepartmentCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["DepartmentCode"].Value).Trim();
                    DataDto.MKCode = dataGridView1.Rows[i].Cells["MKCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["MKCode"].Value).Trim();
                    DataDto.ProductType = dataGridView1.Rows[i].Cells["ProductType"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ProductType"].Value).Trim();
                    DataDto.SKUMode = dataGridView1.Rows[i].Cells["SKUMode"].Value == null ? false : Convert.ToBoolean(dataGridView1.Rows[i].Cells["SKUMode"].Value);
                    DataDto.ChkSKUCode = dataGridView1.Rows[i].Cells["ChkSKUCode"].Value == null ? "" : ((string)dataGridView1.Rows[i].Cells["ChkSKUCode"].Value).Trim();


                    if (DataDto.Quantity != 0)
                    {
                        DataDto.Quantity = DataDto.UnitCode == 5 ? DataDto.Quantity * 1000 : DataDto.Quantity;
                    }
                    if (DataDto.NewQuantity != 0)
                    {
                        DataDto.NewQuantity = DataDto.UnitCode == 5 ? DataDto.NewQuantity * 1000 : DataDto.NewQuantity;
                    }
                    DataDto.UnitCode = DataDto.UnitCode == 5 ? 4 : DataDto.UnitCode;


                    //if (DataDto.ScanMode == 1 || DataDto.ScanMode == 2 || DataDto.ScanMode == 3)
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
                    if (DataDto.ScanMode == 4 && DataDto.UnitCode != 4)
                    {
                        var qty = DataDto.Quantity == null ? 0 : Convert.ToDecimal(DataDto.Quantity);
                        var newqty = DataDto.NewQuantity == null ? 0 : Convert.ToDecimal(DataDto.NewQuantity);
                        if (qty != 0)
                        {
                            DataDto.Quantity = Math.Ceiling(qty);
                        }
                        if (newqty != 0)
                        {
                            DataDto.NewQuantity = Math.Ceiling(newqty);
                        }
                    }

                    if (DataDto.StocktakingID == string.Empty)//New
                    {
                        DataDto.Flag = "N";
                        DataDto.SKUMode = true;
                        DataDto.HHTName = ComName;
                        insertList.Add(DataDto);
                    }
                    else if (DataDto.SKUMode == true)
                    {
                        if (Flag == "I")
                        {
                            if (DataDto.SKUMode == true && DataDto.NewQuantity != 0)
                            {
                                DataDto.Flag = "E";
                                updateList.Add(DataDto);
                            }
                        }
                        else if (Flag == "F")
                        {
                            if (DataDto.NewQuantity != 0 || DataDto.Description != string.Empty)
                            {
                                DataDto.Flag = "U";
                                updateList.Add(DataDto);
                            }
                        }
                        else if (Flag == "N" || Flag == "E" || Flag == "U")
                        {
                            if (Flag == "E" && DataDto.NewQuantity == 0)
                            {
                                DataDto.Flag = "I";
                            }
                            else if (Flag == "U" && DataDto.NewQuantity == 0 && DataDto.Description == string.Empty)
                            {
                                DataDto.Flag = "F";
                            }
                            updateList.Add(DataDto);
                        }
                    }
                    else if (DataDto.SKUMode == false)
                    {
                        if (DataDto.SKUCode == string.Empty)
                        {
                            if (DataDto.NewQuantity == 0 && DataDto.Description == string.Empty)
                            {
                                DataDto.Flag = "F";
                            }
                            else
                            {
                                DataDto.Flag = "U";
                            }
                            updateList.Add(DataDto);
                        }
                        else
                        {
                            if (DataDto.NewQuantity == 0 && DataDto.Description != string.Empty)
                            {
                                DataDto.Flag = "I";
                            }
                            else
                            {
                                DataDto.Flag = "E";
                            }
                            updateList.Add(DataDto);
                        }
                    }

                    listResult.Add(DataDto);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void SetReadOnlyAndStyle(DataGridViewRow dataGridView1)
        {
            if ((string)dataGridView1.Cells["Flag"].Value == "I" || (string)dataGridView1.Cells["Flag"].Value == "E")
            {
                dataGridView1.Cells["ScanMode"].Style.BackColor = Color.LightGray;
                dataGridView1.Cells["LocationCode"].Style.BackColor = Color.LightGray;
                dataGridView1.Cells["BarCode"].Style.BackColor = Color.LightGray;
                dataGridView1.Cells["Quantity"].Style.BackColor = Color.LightGray;
                dataGridView1.Cells["SKUCode"].Style.BackColor = Color.LightGray;
                dataGridView1.Cells["NewQuantity"].Style.BackColor = Color.White;
                dataGridView1.Cells["Flag"].Style.BackColor = Color.White;
                dataGridView1.Cells["No"].Style.BackColor = Color.White;

                dataGridView1.Cells["ScanMode"].ReadOnly = true;
                dataGridView1.Cells["LocationCode"].ReadOnly = true;
                dataGridView1.Cells["BarCode"].ReadOnly = true;
                dataGridView1.Cells["Quantity"].ReadOnly = true;
                
                dataGridView1.Cells["NewQuantity"].ReadOnly = false;
                if (dataGridView1.Cells["ScanMode"].Value.ToString() == "4")//Fresh Food
                {

                    dataGridView1.Cells["UnitCode"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["UnitCode"].ReadOnly = true;
                }
                else
                {
                    dataGridView1.Cells["UnitCode"].Style.BackColor = Color.White;
                    dataGridView1.Cells["UnitCode"].ReadOnly = false;
                }
                dataGridView1.Cells["Flag"].ReadOnly = true;
                dataGridView1.Cells["SKUCode"].ReadOnly = true;
                if ((bool)dataGridView1.Cells["SKUMode"].Value == false)
                {
                    dataGridView1.Cells["Description"].ReadOnly = false;
                    dataGridView1.Cells["Description"].Style.BackColor = Color.White;
                }
                else
                {
                    dataGridView1.Cells["Description"].ReadOnly = true;
                    dataGridView1.Cells["Description"].Style.BackColor = Color.LightGray;
                }
            }
            else if ((string)dataGridView1.Cells["Flag"].Value == "F" || (string)dataGridView1.Cells["Flag"].Value == "U")
            {
                dataGridView1.Cells["ScanMode"].Style.BackColor = Color.LightGray;
                dataGridView1.Cells["LocationCode"].Style.BackColor = Color.LightGray;
                dataGridView1.Cells["BarCode"].Style.BackColor = Color.LightGray;
                dataGridView1.Cells["Quantity"].Style.BackColor = Color.LightGray;
                dataGridView1.Cells["SKUCode"].Style.BackColor = Color.LightGray;
                dataGridView1.Cells["NewQuantity"].Style.BackColor = Color.White;
                dataGridView1.Cells["Flag"].Style.BackColor = Color.White;
                dataGridView1.Cells["No"].Style.BackColor = Color.White;

                dataGridView1.Cells["ScanMode"].ReadOnly = true;
                dataGridView1.Cells["LocationCode"].ReadOnly = true;
                dataGridView1.Cells["BarCode"].ReadOnly = true;
                dataGridView1.Cells["Quantity"].ReadOnly = true;
                dataGridView1.Cells["SKUCode"].ReadOnly = true;

                dataGridView1.Cells["NewQuantity"].ReadOnly = false;
                if (dataGridView1.Cells["ScanMode"].Value.ToString() == "4")//Fresh Food
                {
                    dataGridView1.Cells["UnitCode"].Style.BackColor = Color.LightGray;
                    dataGridView1.Cells["UnitCode"].ReadOnly = true;
                }
                else
                {
                    dataGridView1.Cells["UnitCode"].Style.BackColor = Color.White;
                    dataGridView1.Cells["UnitCode"].ReadOnly = false;
                }
                dataGridView1.Cells["Flag"].ReadOnly = true;
                dataGridView1.Cells["Description"].ReadOnly = false;
                dataGridView1.Cells["Description"].Style.BackColor = Color.White;

            }
            else//Flag=N
            {
                dataGridView1.Cells["ScanMode"].ReadOnly = false;
                dataGridView1.Cells["LocationCode"].ReadOnly = false;
                dataGridView1.Cells["BarCode"].ReadOnly = false;
                dataGridView1.Cells["Quantity"].ReadOnly = false;
                dataGridView1.Cells["SKUCode"].Style.BackColor = Color.LightGray;
                dataGridView1.Cells["SKUCode"].ReadOnly = true;
                dataGridView1.Cells["NewQuantity"].Style.BackColor = Color.LightGray;
                dataGridView1.Cells["NewQuantity"].ReadOnly = true;
                dataGridView1.Cells["UnitCode"].ReadOnly = false;
                dataGridView1.Cells["Description"].Style.BackColor = Color.LightGray;
                dataGridView1.Cells["Flag"].ReadOnly = true;
                dataGridView1.Cells["Description"].ReadOnly = true;

                dataGridView1.Cells["ScanMode"].Style.BackColor = Color.White;
                dataGridView1.Cells["LocationCode"].Style.BackColor = Color.White;
                dataGridView1.Cells["BarCode"].Style.BackColor = Color.White;
                dataGridView1.Cells["Quantity"].Style.BackColor = Color.White;
                dataGridView1.Cells["Flag"].Style.BackColor = Color.White;
                dataGridView1.Cells["No"].Style.BackColor = Color.White;
                dataGridView1.Cells["UnitCode"].Style.BackColor = Color.White;
            }
            dataGridView1.Cells["Remark"].Style.BackColor = Color.LightGray;
        }

        private void Save(List<EditQtyModel.Response> insertList, List<EditQtyModel.Response> updateList, List<EditQtyModel.Response> updateSKUModeList, List<EditQtyModel.Response> deleteList)
        {
            try
            {
                bool saveComplete = auditBLL.SaveAuditHHTToPC(insertList, updateList, updateSKUCodeList, deleteList, UserName);
                if (saveComplete)
                {
                    MessageBox.Show(MessageConstants.Savecomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(MessageConstants.CannotsaveEditQtydatatodatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                btnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private EditQtyModel.Request GetSearchCondition()
        {
            string sectionType = string.Empty;
            if (chLocationType1.Checked)
            {
                sectionType += "1,";
            }
            if (chLocationType2.Checked)
            {
                sectionType += "2,";
            }
            if (chLocationType3.Checked)
            {
                sectionType += "3,";
            }
            if (chLocationType4.Checked)
            {
                sectionType += "4,";
            }
            if (sectionType != string.Empty)
            {
                sectionType = sectionType.Substring(0, sectionType.Length - 1);
            }

            //sectioncode
            if (txtSectionCode.Text.Trim().Length == 1)
            {
                txtSectionCode.Text = "0000" + txtSectionCode.Text.Trim();
            }
            else if (txtSectionCode.Text.Trim().Length == 2)
            {
                txtSectionCode.Text = "000" + txtSectionCode.Text.Trim();
            }
            else if (txtSectionCode.Text.Trim().Length == 3)
            {
                txtSectionCode.Text = "00" + txtSectionCode.Text.Trim();
            }
            else if (txtSectionCode.Text.Trim().Length == 4)
            {
                txtSectionCode.Text = "0" + txtSectionCode.Text.Trim();
            }
            ////locationfrom
            if (txtLocationForm.Text.Trim().Length == 1)
            {
                txtLocationForm.Text = "0000" + txtLocationForm.Text.Trim();
            }
            else if (txtLocationForm.Text.Trim().Length == 2)
            {
                txtLocationForm.Text = "000" + txtLocationForm.Text.Trim();
            }
            else if (txtLocationForm.Text.Trim().Length == 3)
            {
                txtLocationForm.Text = "00" + txtLocationForm.Text.Trim();
            }
            else if (txtLocationForm.Text.Trim().Length == 4)
            {
                txtLocationForm.Text = "0" + txtLocationForm.Text.Trim();
            }
            ////locationto
            if (txtLocationTo.Text.Trim().Length == 1)
            {
                txtLocationTo.Text = "0000" + txtLocationTo.Text.Trim();
            }
            else if (txtLocationTo.Text.Trim().Length == 2)
            {
                txtLocationTo.Text = "000" + txtLocationTo.Text.Trim();
            }
            else if (txtLocationTo.Text.Trim().Length == 3)
            {
                txtLocationTo.Text = "00" + txtLocationTo.Text.Trim();
            }
            else if (txtLocationTo.Text.Trim().Length == 4)
            {
                txtLocationTo.Text = "0" + txtLocationTo.Text.Trim();
            }

            EditQtyModel.Request searchCondition = new EditQtyModel.Request();
            searchCondition.DepartmentCode = txtDepartmentCode.Text;
            searchCondition.SectionCode = txtSectionCode.Text;
            searchCondition.SectionName = txtSectionName.Text;
            searchCondition.SKUCode = txtSKUCode.Text;
            searchCondition.SectionType = sectionType;
            searchCondition.LocationFrom = txtLocationForm.Text;
            searchCondition.LocationTo = txtLocationTo.Text;
            searchCondition.Barcode = txtBarcode.Text;

            return searchCondition;
        }

        #region Gridview
        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.Row.Index].Cells["No"].Value = e.Row.Index + 1;
                dataGridView1.Rows[e.Row.Index].Cells["No"].ReadOnly = true;
                dataGridView1.Rows[e.Row.Index].Cells["SKUCode"].ReadOnly = true;
                dataGridView1.Rows[e.Row.Index].Cells["NewQuantity"].ReadOnly = true;
                dataGridView1.Rows[e.Row.Index].Cells["Flag"].ReadOnly = true;
                dataGridView1.Rows[e.Row.Index].Cells["Description"].ReadOnly = true;
                dataGridView1.Rows[e.Row.Index].Cells["Flag"].Value = "N";
                (dataGridView1.Rows[e.Row.Index].Cells["ScanMode"] as DataGridViewComboBoxCell).Value = 1;
                (dataGridView1.Rows[e.Row.Index].Cells["UnitCode"] as DataGridViewComboBoxCell).Value = 1;

                dataGridView1.Rows[e.Row.Index].Cells["SKUCode"].Style.BackColor = Color.LightGray;
                dataGridView1.Rows[e.Row.Index].Cells["NewQuantity"].Style.BackColor = Color.LightGray;
                dataGridView1.Rows[e.Row.Index].Cells["Description"].Style.BackColor = Color.LightGray;
                dataGridView1.Rows[e.Row.Index].Cells["Remark"].Style.BackColor = Color.LightGray;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("Barcode"))
                {
                    if (valueAfterEdit == string.Empty)
                    {
                        MessageBox.Show("Barcode" + MessageConstants.Cannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                        dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;

                        for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                        {
                            SetReadOnlyAndStyle(dataGridView1.Rows[count]);
                        }
                    }
                    else
                    {
                        var scanMode = dataGridView1.CurrentRow.Cells["ScanMode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["ScanMode"].Value.ToString();
                        var barcode = dataGridView1.CurrentRow.Cells["Barcode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Barcode"].Value.ToString();
                        var location = dataGridView1.CurrentRow.Cells["LocationCode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["LocationCode"].Value.ToString();
                        var unitCode = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["UnitCode"].Value.ToString();
                        if (location != "" && barcode != "")
                        {
                            var stocktakingID = dataGridView1.CurrentRow.Cells["StocktakingID"].Value == null ? "" : dataGridView1.CurrentRow.Cells["StocktakingID"].Value.ToString();
                            var resultBarcodeInMasterSKU = auditBLL.GetDescriptionInMasterSKU(barcode, location, stocktakingID, countDate, Convert.ToInt16(scanMode), Convert.ToInt16(unitCode));
                            if (resultBarcodeInMasterSKU == null)
                            {
                                MessageBox.Show(MessageConstants.BarcodenotexistinMaster, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                                        if (location == chkLocation && barcode == chkBarcode && scanMode=="3" && unitCode == chkUnit)
                                        {
                                            dataGridView1.Rows[count].Cells["No"].Style.BackColor = Color.LightGreen;
                                            dataGridView1.Rows[count].Cells["ScanMode"].Style.BackColor = Color.LightGreen;
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
                                            dataGridView1.Rows[count].Cells["ScanMode"].Style.BackColor = Color.LightGreen;
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
                                    string scanModeCurrentRow = dataGridView1.CurrentRow.Cells["ScanMode"].Value.ToString();
                                    int countDup = 0;
                                    int countDupWarehouseUnit = 0;
                                    int countDupHightlight = 0;
                                    var unitCodeOld = string.Empty;
                                    int num1 = 0;
                                    for (int count = 0; count < dataGridView1.Rows.Count - 1; count++)
                                    {
                                        countDupHightlight = 0;
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
                                            if(countDupWarehouseUnit == 0)
                                            {
                                                num1 = count;
                                            }
                                            countDupWarehouseUnit++;
                                        }
                                        if (scanModeCurrentRow == "3")
                                        {
                                            if (location == chkLocation && barcode == chkBarcode && unitCode == chkUnit.ToString() && scanModeCurrentRow == "3" && countDupWarehouseUnit > 1 )
                                            {
                                                MessageBox.Show(MessageConstants.LocationandBarcodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                dataGridView1.CurrentRow.Cells["Description"].Value = string.Empty;
                                                dataGridView1.CurrentRow.Cells["SKUCode"].Value = string.Empty;
                                                dataGridView1.CurrentRow.Cells["InBarCode"].Value = string.Empty;
                                                dataGridView1.CurrentRow.Cells["ExbarCode"].Value = string.Empty;
                                                dataGridView1.CurrentRow.Cells["MKCode"].Value = string.Empty;
                                                dataGridView1.CurrentRow.Cells["ProductType"].Value = string.Empty;

                                                dataGridView1.Rows[num1].Cells["No"].Style.BackColor = Color.LightGreen;
                                                dataGridView1.Rows[num1].Cells["ScanMode"].Style.BackColor = Color.LightGreen;
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
                                        {
                                            if (scanModeCurrentRow == "4")
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
                else if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("LocationCode"))
                {
                    var scanMode = dataGridView1.CurrentRow.Cells["ScanMode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["ScanMode"].Value.ToString();
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


                        var DepartmentCodeByLocation = auditBLL.CheckIsExistLocation(location, Convert.ToInt16(scanMode));
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
                                                dataGridView1.Rows[count].Cells["ScanMode"].Style.BackColor = Color.LightGreen;
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
                                                dataGridView1.Rows[count].Cells["ScanMode"].Style.BackColor = Color.LightGreen;
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

                                        string scanModeCurrentRow = dataGridView1.CurrentRow.Cells["ScanMode"].Value.ToString();
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
                                                    dataGridView1.Rows[num1].Cells["ScanMode"].Style.BackColor = Color.LightGreen;
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
                }
                else if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("UnitCode"))
                {
                    var scanMode = dataGridView1.CurrentRow.Cells["ScanMode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["ScanMode"].Value.ToString();
                    var barcode = dataGridView1.CurrentRow.Cells["Barcode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Barcode"].Value.ToString();
                    var location = dataGridView1.CurrentRow.Cells["LocationCode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["LocationCode"].Value.ToString();
                    var unitCode = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? "" : dataGridView1.CurrentRow.Cells["UnitCode"].Value.ToString();
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
                                        dataGridView1.Rows[count].Cells["ScanMode"].Style.BackColor = Color.LightGreen;
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
                                        dataGridView1.Rows[count].Cells["ScanMode"].Style.BackColor = Color.LightGreen;
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
                                string scanModeCurrentRow = dataGridView1.CurrentRow.Cells["ScanMode"].Value.ToString();
                                int countDupWarehouseUnit = 0;
                                int countDupHightlight = 0;
                                var unitCodeOld = string.Empty;
                                int num1 = 0;
                                int countDup = 0;
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
                                            dataGridView1.Rows[num1].Cells["ScanMode"].Style.BackColor = Color.LightGreen;
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
                //            dataGridView1.CurrentRow.Cells["ScanMode"].Value = scanMode;
                //        }
                //    }
                //}
                else
                {
                    var newQuantity = dataGridView1.CurrentRow.Cells["NewQuantity"].Value == null ? 0 : Convert.ToDecimal(dataGridView1.CurrentRow.Cells["newQuantity"].Value);
                    var flag = dataGridView1.CurrentRow.Cells["Flag"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Flag"].Value.ToString();
                    var description = dataGridView1.CurrentRow.Cells["Description"].Value == null ? "" : dataGridView1.CurrentRow.Cells["Description"].Value.ToString();
                    if (valueAfterEdit == string.Empty)
                    {
                        if (flag == "F" || flag == "U")
                        {

                        }
                        else if ((flag == "I" || flag == "E") && newQuantity == 0)
                        {

                        }
                        else
                        {
                            MessageBox.Show(dataGridView1.CurrentCell.OwningColumn.Name + MessageConstants.Cannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("Quantity"))
                    {
                        var resultQuantity = string.Empty;
                        var quantity = dataGridView1.CurrentRow.Cells["Quantity"].Value == null ? 0 : Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Quantity"].Value);
                        var scanMode = dataGridView1.CurrentRow.Cells["ScanMode"].Value == null ? "1" : dataGridView1.CurrentRow.Cells["ScanMode"].Value.ToString();
                        //var unitCode = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? "1" : dataGridView1.CurrentRow.Cells["UnitCode"].Value.ToString();
                        if (scanMode == "4")
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
                        //var newQuantity = dataGridView1.CurrentRow.Cells["NewQuantity"].Value == null ? 0 : Convert.ToDecimal(dataGridView1.CurrentRow.Cells["NewQuantity"].Value);
                        var scanMode = dataGridView1.CurrentRow.Cells["ScanMode"].Value == null ? "1" : dataGridView1.CurrentRow.Cells["ScanMode"].Value.ToString();
                        //var unitCode = dataGridView1.CurrentRow.Cells["UnitCode"].Value == null ? "1" : dataGridView1.CurrentRow.Cells["UnitCode"].Value.ToString();
                        if (scanMode == "4")
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

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var currentCell = dataGridView1.CurrentCell.Value == null ? "" : (dataGridView1.CurrentCell.Value.ToString()).Trim();
            if (dataGridView1.CurrentCell.OwningColumn.Name.Equals("ScanMode"))
            {
                scanMode = dataGridView1.CurrentRow.Cells["ScanMode"].Value == null ? 1 : Convert.ToInt16(dataGridView1.CurrentRow.Cells["ScanMode"].Value);
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {

                e.Control.KeyPress -= new KeyPressEventHandler(Column23_KeyPress);
                e.Control.KeyPress -= new KeyPressEventHandler(Column56_KeyPress);
                e.Control.KeyPress -= new KeyPressEventHandler(Column8_KeyPress);
                e.Control.KeyUp -= new KeyEventHandler(Column56_KeyUp);

                if (dataGridView1.CurrentCell.ColumnIndex == 1)
                {
                    ComboBox cb = e.Control as ComboBox;
                    if (cb != null)
                    {
                        cb.SelectedIndexChanged -= new EventHandler(cb_SelectedIndexChanged);

                        cb.SelectedIndexChanged += new EventHandler(cb_SelectedIndexChanged);
                    }
                }
                else if (dataGridView1.CurrentCell.ColumnIndex == 2 || dataGridView1.CurrentCell.ColumnIndex == 3)
                {
                    TextBox tb1 = e.Control as TextBox;
                    if (tb1 != null)
                    {
                        tb1.KeyPress += new KeyPressEventHandler(Column23_KeyPress);
                    }
                }
                else if (dataGridView1.CurrentCell.ColumnIndex == 5 || dataGridView1.CurrentCell.ColumnIndex == 6)
                {
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
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
                string scanMode = dataGridView1.CurrentRow.Cells["ScanMode"].Value == null ? "1" : dataGridView1.CurrentRow.Cells["ScanMode"].Value.ToString();
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
                        dataGridView1.CurrentRow.Cells["ScanMode"].Value = scanMode;
                    }
                }
                ////if(scanMode == "1")
                ////{
                ////    dataGridView1.CurrentRow.Cells["ScanMode"].ReadOnly = true;
                ////}
                ////else {
                ////    dataGridView1.CurrentRow.Cells["ScanMode"].ReadOnly = false;
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
            if (listSummary.Count == 0)
            {
                MessageBox.Show(MessageConstants.NoAuditdatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                EditQTYSummaryForm fSummary = new EditQTYSummaryForm(); // Instantiate a Form3 object.
                fSummary.SummaryEditQty(listSummary, listUnit);
                fSummary.Show();
            }
        }

        private void btnViewStocktakingReport_Click(object sender, EventArgs e)
        {
            try
            {
                ReportPrintForm fReport = new ReportPrintForm(UserName, "R28");
                fReport.StartPosition = FormStartPosition.CenterScreen;
                fReport.Show();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            //ReportManagementForm reportForm = new ReportManagementForm();
            //try
            //{
            //    string allDepartmentCode = txtDepartmentCode.Text;
            //    string allSectionCode = txtSectionCode.Text;
            //    string allSectionName = txtSectionName.Text;
            //    string allBrandCode = string.Empty;

            //    string allStoreType = string.Empty;
            //    if (chLocationType1.Checked)
            //    {
            //        allStoreType += "1,";
            //    }
            //    if (chLocationType2.Checked)
            //    {
            //        allStoreType += "2,";
            //    }
            //    if (chLocationType3.Checked)
            //    {
            //        allStoreType += "3,";
            //    }
            //    if (chLocationType4.Checked)
            //    {
            //        allStoreType += "4,";
            //    }
            //    if (allStoreType != string.Empty)
            //    {
            //        allStoreType = allStoreType.Substring(0, allStoreType.Length - 1);
            //    }
            //    else
            //    {
            //        allStoreType = "1,2,3,4";
            //    }
            //    string allCorrectDelete = "All";
            //    string allLocationCode = string.Empty;
            //    string lFrom = txtLocationForm.Text;
            //    string lTo = txtLocationTo.Text;
            //    if (lFrom == string.Empty && lTo == string.Empty)
            //    {
            //        allLocationCode = string.Empty;
            //    }
            //    else if (lFrom == string.Empty || lTo != string.Empty)
            //    {
            //        if (lFrom == string.Empty)
            //        {
            //            allLocationCode = lTo;
            //        }
            //        else
            //        {
            //            allLocationCode = lFrom;
            //        }
            //    }
            //    else if (txtLocationForm.Text != string.Empty && txtLocationTo.Text != string.Empty)
            //    {
            //        int locationCodeFrom = int.Parse(lFrom);
            //        int locationCodeTo = int.Parse(lTo);
            //        int length = 5;
            //        for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
            //        {
            //            if (i == 0)
            //            {
            //                allLocationCode = locationCodeFrom.ToString("D" + length);
            //            }
            //            else
            //            {
            //                allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
            //            }

            //            locationCodeFrom++;
            //        }
            //    }

            //    ParameterFields paramFields = new ParameterFields();
            //    ParameterField paramField1 = new ParameterField();
            //    ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
            //    ParameterField paramField2 = new ParameterField();
            //    ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
            //    paramField1.Name = "pCountDate";
            //    paramDiscreteValue1.Value = countDate;
            //    paramField1.CurrentValues.Add(paramDiscreteValue1);
            //    paramField2.Name = "pBranchName";
            //    paramDiscreteValue2.Value = branchName;
            //    paramField2.CurrentValues.Add(paramDiscreteValue2);
            //    paramFields.Add(paramField1);
            //    paramFields.Add(paramField2);
            //    Loading_Screen.ShowSplashScreen();
            //    DataTable dt = bllReportManagement.LoadReport_StocktakingAudiAdjust(allLocationCode, allStoreType, allCorrectDelete, countDate, allDepartmentCode, allSectionCode, allBrandCode, allSectionName);
            //    if (dt.Rows.Count > 0)
            //    {

            //        bool isCreateReportSuccess = reportForm.CreateReport(dt, "R10", paramFields);
            //        Loading_Screen.CloseForm();
            //        if (isCreateReportSuccess)
            //        {
            //            reportForm.StartPosition = FormStartPosition.CenterParent;
            //            DialogResult dialogResult = reportForm.ShowDialog();
            //            Loading_Screen.CloseForm();
            //        }
            //        else
            //        {
            //            Loading_Screen.CloseForm();
            //            MessageBox.Show(MessageConstants.cannotgeneratereport, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }

            //    }
            //    else
            //    {
            //        Loading_Screen.CloseForm();
            //        MessageBox.Show(MessageConstants.Nodataforgeneratereport, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(String.Format("Exception : {0}", ex.StackTrace));
            //}
        }

        private void btnViewControlSheetReport_Click(object sender, EventArgs e)
        {
            try
            {
                ReportPrintForm fReport = new ReportPrintForm(UserName, "R11");
                fReport.StartPosition = FormStartPosition.CenterScreen;
                fReport.Show();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }

            //ReportManagementForm reportForm = new ReportManagementForm();

            //try
            //{
            //    string allDepartmentCode = txtDepartmentCode.Text;
            //    string allSectionCode = txtSectionCode.Text;
            //    string allSectionName = txtSectionName.Text;
            //    string allBrandCode = string.Empty;
            //    string allStoreType = string.Empty;
            //    if (chLocationType1.Checked)
            //    {
            //        allStoreType += "1,";
            //    }
            //    if (chLocationType2.Checked)
            //    {
            //        allStoreType += "2,";
            //    }
            //    if (chLocationType3.Checked)
            //    {
            //        allStoreType += "3,";
            //    }
            //    if (chLocationType4.Checked)
            //    {
            //        allStoreType += "4,";
            //    }
            //    if (allStoreType != string.Empty)
            //    {
            //        allStoreType = allStoreType.Substring(0, allStoreType.Length - 1);
            //    }
            //    else
            //    {
            //        allStoreType = "1,2,3,4";
            //    }
            //    string allLocationCode = string.Empty;
            //    string lFrom = txtLocationForm.Text;
            //    string lTo = txtLocationTo.Text;
            //    if (lFrom == string.Empty && lTo == string.Empty)
            //    {
            //        allLocationCode = string.Empty;
            //    }
            //    else if (lFrom == string.Empty || lTo != string.Empty)
            //    {
            //        if (lFrom == string.Empty)
            //        {
            //            allLocationCode = lTo;
            //        }
            //        else
            //        {
            //            allLocationCode = lFrom;
            //        }
            //    }
            //    else if (txtLocationForm.Text != string.Empty && txtLocationTo.Text != string.Empty)
            //    {
            //        int locationCodeFrom = int.Parse(lFrom);
            //        int locationCodeTo = int.Parse(lTo);
            //        int length = 5;
            //        for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
            //        {
            //            if (i == 0)
            //            {
            //                allLocationCode = locationCodeFrom.ToString("D" + length);
            //            }
            //            else
            //            {
            //                allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
            //            }

            //            locationCodeFrom++;
            //        }
            //    }
            //    ParameterFields paramFields = new ParameterFields();
            //    ParameterField paramField1 = new ParameterField();
            //    ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
            //    ParameterField paramField2 = new ParameterField();
            //    ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
            //    paramField1.Name = "pCountDate";
            //    paramDiscreteValue1.Value = countDate;
            //    paramField1.CurrentValues.Add(paramDiscreteValue1);
            //    paramField2.Name = "pBranchName";
            //    paramDiscreteValue2.Value = branchName;
            //    paramField2.CurrentValues.Add(paramDiscreteValue2);
            //    paramFields.Add(paramField1);
            //    paramFields.Add(paramField2);
            //    Loading_Screen.ShowSplashScreen();
            //    DataTable dt = bllReportManagement.GetReport_ControlSheet(allSectionCode, allStoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode, allSectionName);

            //    if (dt.Rows.Count > 0)
            //    {

            //        bool isCreateReportSuccess = reportForm.CreateReport(dt, "R11", paramFields);
            //        Loading_Screen.CloseForm();
            //        if (isCreateReportSuccess)
            //        {
            //            reportForm.StartPosition = FormStartPosition.CenterParent;
            //            DialogResult dialogResult = reportForm.ShowDialog();
            //            Loading_Screen.CloseForm();
            //        }
            //        else
            //        {
            //            Loading_Screen.CloseForm();
            //            MessageBox.Show(MessageConstants.cannotgeneratereport, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }

            //    }
            //    else
            //    {
            //        Loading_Screen.CloseForm();
            //        MessageBox.Show(MessageConstants.Nodataforgeneratereport, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(String.Format("Exception : {0}", ex.StackTrace));
            //}
        }

        private void chLocationType4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chLocationType3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chLocationType2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chLocationType1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
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
    }
}
