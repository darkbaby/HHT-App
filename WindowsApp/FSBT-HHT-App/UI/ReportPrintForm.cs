using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSBT_HHT_BLL;
using FSBT_HHT_DAL;
using FSBT_HHT_Model;
using CrystalDecisions.Shared;
using System.Text.RegularExpressions;
using FSBT.HHT.App.Resources;
using System.Collections;

namespace FSBT.HHT.App.UI
{
    public partial class ReportPrintForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        private PermissionComponentBll permissionComponent = null;
        private ReportManagementBll bllReportManagement = new ReportManagementBll();
        private SystemSettingBll bllSystemSetting = new SystemSettingBll();
        private List<ConfigReport> reportConfig = new List<ConfigReport>();
        ReportManagementForm reportForm = new ReportManagementForm();
        public string UserName;
        public string ReportCode;
        private DateTime selectedCountDate = DateTime.Now;

        public ReportPrintForm()
        {
            InitializeComponent();
        }

        public ReportPrintForm(string username, string reportCode)
        {
            UserName = username;
            ReportCode = reportCode;
            InitializeComponent();
        }

        private List<string> GetReportComponentByReport(string reportCode)
        {
            List<string> lstReportConfig = new List<string>();
            try
            {
                lstReportConfig = (from lst in reportConfig
                                   where lst.ReportCode == reportCode
                                   select lst.ConditionObject).ToList();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                lstReportConfig = new List<string>();
            }
            return lstReportConfig;
        }

        private List<ConfigReport> GetReportComponentListByReport(string reportCode)
        {
            List<ConfigReport> lstReportConfig = new List<ConfigReport>();
            try
            {
                lstReportConfig = (from lst in reportConfig
                                   where lst.ReportCode == reportCode
                                   select lst).ToList<ConfigReport>();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                lstReportConfig = new List<ConfigReport>();
            }
            return lstReportConfig;
        }

        private void SetupReportList()
        {
            try
            {
                lstReport.Items.Clear();
                //DataTable dtreport = reportManagement.LoadUserReportDataTableByUser(UserName);
                List<MasterReport> reportList = bllReportManagement.LoadUserReportListByUser(UserName);
                lstReport.DataSource = reportList;
                lstReport.ValueMember = "ReportCode";
                lstReport.DisplayMember = "ReportName";
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void SetupCriteriaComponent(string reportCode)
        {
            lblReportName.Text = lstReport.Text;
            List<string> reportComponent = GetReportComponentByReport(reportCode);
            List<ConfigReport> reportComponentList = GetReportComponentListByReport(reportCode);
            var listSettingData = bllSystemSetting.GetSettingData();
            string sectionType = listSettingData.SectionType;

            try
            {
                foreach (Control contrl in gbReportFilter.Controls)
                {
                    if (contrl is GroupBox)
                    {
                        foreach (Control component in contrl.Controls)
                        {
                            if ((reportComponentList.Select(s => s.ConditionObject).ToList<string>().Contains(component.Name)))
                            {
                                component.Enabled = true;

                                //Set DefaultValue
                                string defaulValue = reportConfig.Where(s => s.ConditionObject == component.Name && s.ReportCode == reportCode)
                                                        .Select(t => t.DefaultValue).FirstOrDefault().ToString();

                                if ((component is RadioButton) && component.Text.Equals(defaulValue))
                                {
                                    ((RadioButton)(component)).Checked = true;
                                }
                                else if ((component is RadioButton) && String.IsNullOrEmpty(defaulValue))
                                {
                                    ((RadioButton)(component)).Checked = false;
                                }

                                if ((component is CheckBox) && component.Text.Equals(defaulValue))
                                {
                                    ((CheckBox)(component)).Checked = true;
                                }
                                else if ((component is CheckBox) && String.IsNullOrEmpty(defaulValue))
                                {
                                    ((CheckBox)(component)).Checked = false;
                                }

                                //Set AllowChange
                                bool allowChange = reportConfig.Where(s => s.ConditionObject == component.Name && s.ReportCode == reportCode)
                                                        .Select(t => (bool)t.AllowChange).FirstOrDefault();

                                if (!allowChange)
                                {
                                    component.Enabled = false;
                                }
                            }
                            else
                            {
                                component.Enabled = false;

                                if (component is TextBox)
                                {
                                    component.Text = "";
                                }
                                else if (component is CheckBox)
                                {
                                    ((CheckBox)(component)).Checked = false;
                                }
                                else if (component is RadioButton)
                                {
                                    ((RadioButton)(component)).Checked = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!((contrl is Label) || ((contrl is Button))))
                        {
                            if ((reportComponentList.Select(s => s.ConditionObject).ToList<string>().Contains(contrl.Name)))
                            {
                                contrl.Enabled = true;

                                //Set DefaultValue
                                string defaulValue = reportConfig.Where(s => s.ConditionObject == contrl.Name && s.ReportCode == reportCode)
                                                        .Select(t => t.DefaultValue).FirstOrDefault().ToString();

                                if (defaulValue != "")
                                {
                                    if (contrl is ComboBox)
                                    {
                                        ((ComboBox)contrl).SelectedIndex = ((ComboBox)contrl).Items.IndexOf(defaulValue);
                                    }
                                    else if (contrl is TextBox)
                                    {
                                        contrl.Text = defaulValue;
                                    }
                                    else if (contrl is DateTimePicker)
                                    {
                                        DateTime dtValue = DateTime.Now;

                                        if (DateTime.TryParse(defaulValue, out dtValue))
                                        {
                                            ((DateTimePicker)contrl).Value = dtValue;
                                        }
                                    }

                                }
                                else
                                {
                                    if (contrl is ComboBox)
                                    {
                                        ((ComboBox)contrl).SelectedIndex = -1;
                                    }
                                    else if (contrl is TextBox)
                                    {
                                        contrl.Text = "";
                                    }
                                    else if (contrl is DateTimePicker)
                                    {
                                        ((DateTimePicker)contrl).Value = selectedCountDate;
                                    }
                                }

                                //Set AllowChange
                                bool allowChange = reportConfig.Where(s => s.ConditionObject == contrl.Name && s.ReportCode == reportCode)
                                                        .Select(t => (bool)t.AllowChange).FirstOrDefault();

                                if (!allowChange)
                                {
                                    contrl.Enabled = false;
                                }
                            }
                            else
                            {
                                if (!(contrl is GroupBox))
                                {
                                    contrl.Enabled = false;
                                    if (contrl is TextBox)
                                    {
                                        contrl.Text = "";
                                    }
                                    else if (contrl is DateTimePicker)
                                    {
                                        ((DateTimePicker)contrl).Value = selectedCountDate;
                                    }
                                }
                            }
                        }
                    }
                }
                if (stFront.Checked == false && stBack.Checked == false && stWarehouse.Checked == false && stFreshFood.Checked == false &&
                    stFront.Enabled == true && stBack.Enabled == true && stWarehouse.Enabled == true && stFreshFood.Enabled == true)
                    if (sectionType != string.Empty)
                    {
                        var listSectionType = sectionType.Split('|').ToList();
                        foreach (var item in listSectionType)
                        {
                            if (item == "1") { stFront.Checked = true; }
                            else if (item == "2") { stBack.Checked = true; }
                            else if (item == "3") { stWarehouse.Checked = true; }
                            else if (item == "4") { stFreshFood.Checked = true; }
                        }
                    }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private ReportParameter GetParameterReport()
        {
            ReportParameter reportParam = new ReportParameter();
            string reportCode = lstReport.SelectedValue.ToString();
            List<ConfigReport> reportComponentList = GetReportComponentListByReport(reportCode);

            try
            {
                reportParam.CountDate = countDate.Value;
                var listsettingData = bllSystemSetting.GetSettingData();
                reportParam.BranchName = listsettingData.Branch;

                //Set Department Code
                if (rbDepartmentAll.Checked)
                {
                    reportParam.DepartmentCode = "";
                }

                if (rbDepartmentCode.Checked)
                {
                    departmentCode.Text = removeUnusedComma(departmentCode.Text);
                    if (!String.IsNullOrEmpty(departmentCode.Text))
                    {
                        reportParam.DepartmentCode = departmentCode.Text;
                        //List<string> departmentCodeList = bllReportManagement.GetDepartmentCodeList();
                        //string errorExist = validateInList(departmentCode.Text, departmentCodeList);

                        //if (errorExist.Length == 0)
                        //{
                        //    reportParam.DepartmentCode = departmentCode.Text;
                        //}
                        //else
                        //{
                        //    MessageBox.Show(MessageConstants.TheseDepartmentCodedonotexistOpen + errorExist + MessageConstants.Close, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return reportParam = null;
                        //}
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.PleaseinsertDepartmentCode, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                }

                //Set SectionCode
                if (rbSectionAll.Checked)
                {
                    reportParam.SectionCode = "";
                }

                if (rbSectionCode.Checked)
                {
                    sectionCode.Text = removeUnusedComma(sectionCode.Text);
                    if (!String.IsNullOrEmpty(sectionCode.Text))
                    {
                        List<string> sectionList = bllReportManagement.GetSectionCodeList();
                        string errorExist = validateInList(sectionCode.Text, sectionList);

                        if (errorExist.Length == 0)
                        {
                            reportParam.SectionCode = sectionCode.Text;
                        }
                        else
                        {
                            MessageBox.Show(MessageConstants.TheseSectionCodedonotexistOpen + errorExist + MessageConstants.Close, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return reportParam = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.PleaseinsertSectionCode, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                }

                //Set LocationCode
                if (rbLocationAll.Checked)
                {
                    reportParam.LocationCode = "";
                }

                List<string> locationList = bllReportManagement.GetLocationList();
                if (rbLocationFromTo.Checked)
                {
                    locationFrom.Text = locationFrom.Text.Trim();
                    locationTo.Text = locationTo.Text.Trim();

                    if (!String.IsNullOrEmpty(locationFrom.Text) && !String.IsNullOrEmpty(locationTo.Text))
                    {
                        string loFrom = locationFrom.Text;
                        string loTo = locationTo.Text;
                        if (loFrom.Length < 5)
                        {
                            if (loFrom.Length == 1)
                            {
                                locationFrom.Text = "0000" + loFrom;
                            }
                            else if (loFrom.Length == 2)
                            {
                                locationFrom.Text = "000" + loFrom;
                            }
                            else if (loFrom.Length == 3)
                            {
                                locationFrom.Text = "00" + loFrom;
                            }
                            else if (loFrom.Length == 4)
                            {
                                locationFrom.Text = "0" + loFrom;
                            }
                        }
                        if (loTo.Length < 5)
                        {
                            if (loTo.Length == 1)
                            {
                                locationTo.Text = "0000" + loTo;
                            }
                            else if (loTo.Length == 2)
                            {
                                locationTo.Text = "000" + loTo;
                            }
                            else if (loTo.Length == 3)
                            {
                                locationTo.Text = "00" + loTo;
                            }
                            else if (loTo.Length == 4)
                            {
                                locationTo.Text = "0" + loTo;
                            }
                        }

                        string errorExist = validateInList(locationFrom.Text + "," + locationTo.Text, locationList);

                        if (errorExist.Length == 0)
                        {
                            int locFrom = Convert.ToInt32(locationFrom.Text);
                            int locTo = Convert.ToInt32(locationTo.Text);

                            if ((locTo > locFrom) || (locTo == locFrom))
                            {
                                reportParam.LocationCode = locationFrom.Text + "-" + locationTo.Text;
                            }
                            else
                            {
                                MessageBox.Show(MessageConstants.TheLocationFromhastobelessthanLocationTo, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return reportParam = null;
                            }
                        }
                        else
                        {
                            MessageBox.Show(MessageConstants.TheseLocationCodedonotexistinsystem + errorExist + MessageConstants.Close, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return reportParam = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.PleaseinsertLocationFromorLocationTo, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                }

                if (rbLocation.Checked)
                {
                    location.Text = removeUnusedComma(location.Text);
                    if (!String.IsNullOrEmpty(location.Text))
                    {
                        string errorExist = validateInList(location.Text, locationList);

                        if (errorExist.Length == 0)
                        {
                            reportParam.LocationCode = location.Text;
                        }
                        else
                        {
                            MessageBox.Show(MessageConstants.TheseLocationCodedonotexistinsystem + errorExist + MessageConstants.Close, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return reportParam = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.PleaseinsertLocation, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                }

                //Set Brand

                if (rbBrandAll.Checked)
                {
                    reportParam.BrandCode = "";
                    reportParam.BrandName = "ALL BRAND";
                }

                if (rbBrand.Checked)
                {
                    if (brandCode.Items.Count == 0)
                    {
                        MessageBox.Show(MessageConstants.PleaseinsertBrandCode, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                    else
                    {
                        reportParam.BrandCode = brandCode.SelectedValue.ToString();
                        reportParam.BrandName = ((ReportMasterBrand)brandCode.SelectedItem).BrandName.ToString();
                    }

                    //brandCode1.Text = removeUnusedComma(brandCode1.Text);
                    //if (!String.IsNullOrEmpty(brandCode1.Text))
                    //{
                    //string errorExist = validateInList(brandCode1.Text, brandList);

                    //if (errorExist.Length == 0)
                    //{
                    //    reportParam.BrandCode = brandCode1.Text;
                    //}
                    //else
                    //{
                    //    MessageBox.Show(MessageConstants.TheseBrandCodedonotexistinsystem + errorExist + MessageConstants.Close, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return reportParam = null;
                    //}
                    //}
                    //    else
                    //    {
                    //        MessageBox.Show(MessageConstants.PleaseinsertBrandCode, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //        return reportParam = null;
                    //    }
                }

                //Set Barcode
                if (rbBarcodeAll.Checked)
                {
                    reportParam.Barcode = "";
                }

                if (rbBarcode.Checked)
                {
                    barcode.Text = removeUnusedComma(barcode.Text);
                    if (!String.IsNullOrEmpty(barcode.Text))
                    {
                        List<string> barcodeList = bllReportManagement.GetBarcodeList();
                        string errorList = validateInList(barcode.Text, barcodeList);

                        if (errorList.Length == 0)
                        {
                            reportParam.Barcode = barcode.Text;
                        }
                        else
                        {
                            MessageBox.Show(MessageConstants.TheseBarcodedonotexistinsystem + errorList + ")", MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return reportParam = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.PleaseinsertBarcode, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                }

                //Set Store Type
                List<int> storeType = new List<int>();
                foreach (Control component in gbStoreType.Controls)
                {
                    if (((CheckBox)(component)).Checked)
                    {
                        int tmpStoreType = 0;

                        if (((CheckBox)(component)).Text.Equals("Front Store"))
                        {
                            tmpStoreType = (int)StoreType.Front;
                        }
                        else if (((CheckBox)(component)).Text.Equals("Back Store"))
                        {
                            tmpStoreType = (int)StoreType.Back;
                        }
                        else if (((CheckBox)(component)).Text.Equals("Fresh Food"))
                        {
                            tmpStoreType = (int)StoreType.FreshFood;
                        }
                        else if (((CheckBox)(component)).Text.Equals("Warehouse"))
                        {
                            tmpStoreType = (int)StoreType.Warehouse;
                        }

                        if (tmpStoreType > 0)
                        {
                            storeType.Add(tmpStoreType);
                        }
                    }
                }

                if (storeType.Count > 0)
                {
                    reportParam.StoreType = String.Join(",", storeType);
                }
                else
                {
                    MessageBox.Show(MessageConstants.PleaseselectStoreType, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return reportParam = null;
                }

                //Set Diff Type
                List<int> diffType = new List<int>();
                bool enableDiffType = false;
                foreach (Control component in gbDiff.Controls)
                {
                    if (((CheckBox)(component)).Checked)
                    {
                        int tmpDiffType = 0;
                        if (((CheckBox)(component)).Text.Equals("Shortage"))
                        {
                            tmpDiffType = (int)DiffType.Shortage;
                        }
                        else if (((CheckBox)(component)).Text.Equals("Over"))
                        {
                            tmpDiffType = (int)DiffType.Over;
                        }
                        else if (((CheckBox)(component)).Text.Equals("All"))
                        {
                            tmpDiffType = (int)DiffType.All;
                        }

                        if (tmpDiffType > 0)
                        {
                            diffType.Add(tmpDiffType);
                        }

                    }

                    if (((CheckBox)(component)).Enabled)
                    {
                        if (!enableDiffType)
                        {
                            enableDiffType = true;
                        }
                    }
                }

                if (enableDiffType)
                {
                    if (diffType.Count() > 0)
                    {
                        reportParam.DiffType = String.Join(",", diffType);
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.PleaseselectDiffType, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                }

                //Set Correct/Delete
                List<string> correctDel = new List<string>();
                bool enableCorrectDel = false;
                foreach (Control component in gbCorrectionDel.Controls)
                {
                    if (((CheckBox)(component)).Checked)
                    {
                        correctDel.Add(component.Text);
                    }

                    if (((CheckBox)(component)).Enabled)
                    {
                        if (!enableCorrectDel)
                        {
                            enableCorrectDel = true;
                        }
                    }
                }

                if (enableCorrectDel)
                {
                    if (correctDel.Count() > 0)
                    {
                        reportParam.CorrectDelete = String.Join(",", correctDel);
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.PleaseselectCorrectDelete, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                }

                //Set Unit
                List<int> unitCode = new List<int>();
                foreach (Control component in gbUnit.Controls)
                {
                    if (((RadioButton)(component)).Checked)
                    {
                        int tmpUnitCode = 0;

                        if (((RadioButton)(component)).Text.Equals("Pack"))
                        {
                            tmpUnitCode = (int)Unit.Pack;
                        }
                        else if (((RadioButton)(component)).Text.Equals("Piece"))
                        {
                            tmpUnitCode = (int)Unit.Piece;
                        }
                        else if (((RadioButton)(component)).Text.Equals("Gram"))
                        {
                            tmpUnitCode = (int)Unit.Gram;
                        }

                        if (tmpUnitCode > 0)
                        {
                            unitCode.Add(tmpUnitCode);
                        }
                    }
                }

                if (unitCode.Count > 0)
                {
                    reportParam.Unit = String.Join(",", unitCode);
                }
                else
                {
                    reportParam.Unit = "";
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                reportParam = null;
            }

            return reportParam;
        }

        #region Form Event

        private void ReportPrintForm_Load(object sender, EventArgs e)
        {
            try
            {
                var listSettingData = bllSystemSetting.GetSettingData();
                selectedCountDate = listSettingData.CountDate;
                reportConfig = bllReportManagement.LoadReportConfig();
                SetupCriteriaComponent("");
                SetupReportList();
                if (lstReport.Items.Count > 0)
                {
                    if (ReportCode == string.Empty)
                    {
                        lstReport.SelectedIndex = 0;
                    }
                    else
                    {
                        lstReport.SelectedValue = ReportCode;
                    }
                }
                List<ReportMasterBrand> brandList = bllReportManagement.GetBrandList();
                brandCode.DataSource = brandList;
                brandCode.ValueMember = "BrandCode";
                brandCode.DisplayMember = "BrandName";

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void lstReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupCriteriaComponent(lstReport.SelectedValue.ToString());
        }

        private void countDate_ValueChanged(object sender, EventArgs e)
        {
            selectedCountDate = countDate.Value;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                //Get Parameter Value
                ReportParameter reportParam = GetParameterReport();

                //Preview Report
                if (reportParam != null)
                {
                    //Generate Report
                    Loading_Screen.ShowSplashScreen();
                    string reportCode = lstReport.SelectedValue.ToString();
                    string reportName = bllReportManagement.GetReportNameByReportCode(reportCode);
                    bool isCreateReportSuccess = false;
                    ParameterFields paramFields = new ParameterFields();
                    DataTable dt = new DataTable();
                    DataSet ds = new DataSet();
                    switch (reportCode)
                    {
                        case "R01":
                            dt = GetReport_SumStockOnHand(reportParam, paramFields, reportName);
                            break;
                        case "R02":
                            dt = GetReport_SumStockOnHandFreshFood(reportParam, paramFields, reportName);
                            break;
                        case "R03":
                            dt = GetReport_SumStockOnHandByBrandGroup(reportParam, paramFields, reportName);
                            break;
                        case "R04":
                            dt = GetReport_SumStockOnHandByBrandGroupFreshFood(reportParam, paramFields, reportName);
                            break;
                        case "R05":
                            dt = GetReport_SectionLocationByBrandGroup(reportParam, paramFields, reportName);
                            break;
                        case "R06":
                            dt = GetReport_StocktakingAuditCheckWithUnit(reportParam, paramFields, reportName);
                            break;
                        case "R07":
                            dt = GetReport_StocktakingAuditCheck(reportParam, paramFields, reportName);
                            break;
                        case "R08":
                            dt = GetReport_DeleteRecordReportByLocation(reportParam, paramFields, reportName);
                            break;
                        case "R09":
                            dt = GetReport_DeleteRecordReportBySection(reportParam, paramFields, reportName);
                            break;
                        case "R10":
                            dt = GetReport_StocktakingAuditAdjust(reportParam, paramFields, reportName);
                            break;
                        case "R11":
                            dt = GetReport_ControlSheet(reportParam, paramFields, reportName);
                            break;
                        case "R12":
                            dt = GetReport_UncountedLocation(reportParam, paramFields, reportName);
                            break;
                        case "R13":
                            dt = GetReport_UnidentifiedStockItems(reportParam, paramFields, reportName);
                            break;
                        case "R14":
                        case "R15":
                        case "R29":
                            if (reportCode == "R14" && reportParam.StoreType == "2,1")
                            {
                                break;
                            }
                            else
                            {
                                dt = GetReport_InventoryControlDifferenceBySection(reportParam, paramFields, reportName);
                                break;
                            }
                        case "R16":
                        case "R17":
                        case "R30":
                            if (reportCode == "R16" && reportParam.StoreType == "2,1")
                            {
                                break;
                            }
                            else
                            {
                                dt = GetReport_InventoryControlDifferenceByLocation(reportParam, paramFields, reportName);
                                break;
                            }
                        case "R18":
                            if (reportParam.StoreType == "2,1")
                            {
                                break;
                            }
                            else
                            {
                                dt = GetReport_InventoryControlDifferenceByBarcodeFrontBack(reportParam, paramFields, reportName);
                                break;
                            }
                        case "R19":
                        case "R20":
                            dt = GetReport_InventoryControlDifferenceByBarcodeWarehouseFreshFood(reportParam, paramFields, reportName);
                            break;
                        case "R21":
                            if (reportParam.StoreType == "1" || reportParam.StoreType == "2" || reportParam.StoreType == "2,1" || reportParam.StoreType == "3" || reportParam.StoreType == "4")
                            {
                                ds = GetReport_ItemPhysicalCountBySection(reportParam, paramFields, reportName);
                                dt = ds.Tables[0];
                                break;
                            }
                            else
                            {
                                break;
                            }
                        case "R22":
                            if (reportParam.StoreType == "1" || reportParam.StoreType == "2" || reportParam.StoreType == "2,1" || reportParam.StoreType == "3" || reportParam.StoreType == "4")
                            {
                                ds = GetReport_ItemPhysicalCountByBarcode(reportParam, paramFields, reportName);
                                dt = ds.Tables[0];
                                break;
                            }
                            else
                            {
                                break;
                            }
                        case "R23":
                            dt = GetReport_GroupSummaryReportByFrontBack(reportParam, paramFields, reportName);
                            break;
                        case "R24":
                            dt = GetReport_CountedLocationsReport(reportParam, paramFields, reportName);
                            break;
                        case "R25":
                            dt = GetReport_NoticeOfStocktakingSatisfactionByFrontBack(reportParam, paramFields, reportName);
                            break;
                        case "R26":
                            dt = GetReport_SumStockOnHandWarehouse(reportParam, paramFields, reportName);
                            break;
                        case "R27":
                            dt = GetReport_SumStockOnHandByBrandGroupWarehouse(reportParam, paramFields, reportName);
                            break;
                        case "R28":
                            dt = GetReport_StocktakingAuditAdjustWithUnit(reportParam, paramFields, reportName);
                            break;
                        case "R31":
                            dt = GetReport_GroupSummaryReportByFreshFoodWarehouse(reportParam, paramFields, reportName);
                            break;
                        case "R32":
                            dt = GetReport_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse(reportParam, paramFields, reportName);
                            break;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (reportCode == "R21" || reportCode == "R22")
                        {
                            isCreateReportSuccess = reportForm.CreateReport(ds, reportCode, paramFields);
                        }
                        else
                        {
                            isCreateReportSuccess = reportForm.CreateReport(dt, reportCode, paramFields);
                        }
                        Loading_Screen.CloseForm();
                        if (isCreateReportSuccess)
                        {
                            reportForm.StartPosition = FormStartPosition.CenterParent;
                            DialogResult dialogResult = reportForm.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show(MessageConstants.cannotgeneratereport, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (reportCode == "R13" || reportCode == "R18" || reportCode == "R19" || reportCode == "R20")
                        {
                            isCreateReportSuccess = reportForm.CreateReport(dt, reportCode, paramFields);
                            Loading_Screen.CloseForm();
                            if (isCreateReportSuccess)
                            {
                                reportForm.StartPosition = FormStartPosition.CenterParent;
                                DialogResult dialogResult = reportForm.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show(MessageConstants.cannotgeneratereport, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if ((reportCode == "R18" || reportCode == "R14" || reportCode == "R16") && reportParam.StoreType == "2,1")
                        {
                            Loading_Screen.CloseForm();
                            MessageBox.Show(MessageConstants.PleaseselectStoreTypeFrontOrBack, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if ((reportCode == "R21" || reportCode == "R22") && (reportParam.StoreType == "3,4,2,1" || reportParam.StoreType == "3,2,1" || reportParam.StoreType == "4,2,1"))
                        {
                            Loading_Screen.CloseForm();
                            MessageBox.Show(MessageConstants.PleaseselectStoreTypeFrontAndBack, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            Loading_Screen.CloseForm();
                            MessageBox.Show(MessageConstants.Nodataforgeneratereport, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        #region Department Code Group
        private void rbDepartmentAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDepartmentAll.Checked)
            {
                departmentCode.Enabled = false;
                departmentCode.Text = "";
            }
        }
        private void rbDepartmentCode_CheckedChanged(object sender, EventArgs e)
        {
            departmentCode.Enabled = true;
        }
        private void departmentCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != ',' && !char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        #region Section Code Group

        private void rbSectionAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSectionAll.Checked)
            {
                sectionCode.Enabled = false;
                sectionCode.Text = "";
            }
        }

        private void rbSectionCode_CheckedChanged(object sender, EventArgs e)
        {
            sectionCode.Enabled = true;
        }


        private void sectionCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != ',' && !char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        #endregion

        #region Location Group
        private void rbLocationAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLocationAll.Checked)
            {
                locationFrom.Enabled = false;
                locationFrom.Text = "";

                locationTo.Enabled = false;
                locationTo.Text = "";

                location.Enabled = false;
                location.Text = "";
            }
        }

        private void rbLocationFromTo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLocationFromTo.Checked)
            {
                locationFrom.Enabled = true;
                locationTo.Enabled = true;

                location.Enabled = false;
                location.Text = "";
            }
        }

        private void rbLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLocation.Checked)
            {
                locationFrom.Enabled = false;
                locationFrom.Text = "";

                locationTo.Enabled = false;
                locationTo.Text = "";

                location.Enabled = true;
            }
        }

        private void locationFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void locationTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void location_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != ',' && !char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion

        #region Brand Code Group

        private void rbBrandAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBrandAll.Checked)
            {
                brandCode.Enabled = false;
                //brandCode1.Text = "";
            }
        }

        private void rbBrand_CheckedChanged(object sender, EventArgs e)
        {
            brandCode.Enabled = true;
        }
        private void brandCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //reportParam.brandCode = brandCode.SelectedValue;
        }

        private void brandCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != ',' && !char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion

        #region Barcode Group

        private void rbBarcodeAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBarcodeAll.Checked)
            {
                barcode.Enabled = false;
                barcode.Text = "";
            }
        }

        private void rbBarcode_CheckedChanged(object sender, EventArgs e)
        {
            barcode.Enabled = true;
        }

        private void barcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != ',' && !char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion

        private string removeUnusedComma(string message)
        {
            string result = "";

            string trimFirstEnd = Regex.Replace(message.Trim(), @"^[\,]*|\,*$", "");
            result = Regex.Replace(trimFirstEnd, @"[\, ]*\, *", ",");

            return result;
        }

        private string validateInList(string compare, List<string> list)
        {
            string errorList = "";

            try
            {
                if (!String.IsNullOrEmpty(compare) && (list.Count() > 0))
                {
                    string[] compareArr = compare.Split(',');
                    List<string> error = compareArr.Except(list).ToList<string>();

                    if (error.Count > 0)
                    {
                        errorList = String.Join(",", error);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                errorList = "";
            }

            return errorList;
        }

        #endregion

        private DataTable GetReport_SumStockOnHand(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allBrandCode = reportParam.BrandCode;
                DateTime countDate = reportParam.CountDate;
                string allBrandCodeForDisplay = "";

                if (allBrandCode == "")
                {
                    allBrandCodeForDisplay = "All Brand";
                }
                else
                {
                    allBrandCodeForDisplay = allBrandCode;
                }

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                ParameterField paramField4 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue4 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "selectedBrand";
                paramDiscreteValue2.Value = allBrandCodeForDisplay;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pBranchName";
                paramDiscreteValue3.Value = reportParam.BranchName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramField4.Name = "pReportName";
                paramDiscreteValue4.Value = ReportName;
                paramField4.CurrentValues.Add(paramDiscreteValue4);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);
                paramFields.Add(paramField3);
                paramFields.Add(paramField4);



                return bllReportManagement.GetReport_SumStockOnHand(allBrandCode, countDate, allDepartmentCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_SumStockOnHandWarehouse(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allBrandCode = reportParam.BrandCode;
                DateTime countDate = reportParam.CountDate;
                string allBrandCodeForDisplay = "";

                if (allBrandCode == "")
                {
                    allBrandCodeForDisplay = "All Brand";
                }
                else
                {
                    allBrandCodeForDisplay = allBrandCode;
                }

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                ParameterField paramField4 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue4 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "selectedBrand";
                paramDiscreteValue2.Value = allBrandCodeForDisplay;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pBranchName";
                paramDiscreteValue3.Value = reportParam.BranchName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramField4.Name = "pReportName";
                paramDiscreteValue4.Value = ReportName;
                paramField4.CurrentValues.Add(paramDiscreteValue4);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);
                paramFields.Add(paramField3);
                paramFields.Add(paramField4);


                return bllReportManagement.GetReport_SumStockOnHandWarehouse(allBrandCode, countDate, allDepartmentCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_SumStockOnHandFreshFood(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allBrandCode = reportParam.BrandCode;
                DateTime countDate = reportParam.CountDate;
                string allBrandCodeForDisplay = "";

                if (allBrandCode == "")
                {
                    allBrandCodeForDisplay = "All Brand";
                }
                else
                {
                    allBrandCodeForDisplay = allBrandCode;
                }

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                ParameterField paramField4 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue4 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "selectedBrand";
                paramDiscreteValue2.Value = allBrandCodeForDisplay;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pBranchName";
                paramDiscreteValue3.Value = reportParam.BranchName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramField4.Name = "pReportName";
                paramDiscreteValue4.Value = ReportName;
                paramField4.CurrentValues.Add(paramDiscreteValue4);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);
                paramFields.Add(paramField3);
                paramFields.Add(paramField4);

                return bllReportManagement.GetReport_SumStockOnHandFreshFood(allBrandCode, countDate, allDepartmentCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_SumStockOnHandByBrandGroup(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allBrandCode = reportParam.BrandCode;
                DateTime countDate = reportParam.CountDate;

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pBranchName";
                paramDiscreteValue2.Value = reportParam.BranchName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pReportName";
                paramDiscreteValue3.Value = ReportName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);
                paramFields.Add(paramField3);

                return bllReportManagement.GetReport_SumStockOnHand(allBrandCode, countDate, allDepartmentCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_SumStockOnHandByBrandGroupWarehouse(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allBrandCode = reportParam.BrandCode;
                DateTime countDate = reportParam.CountDate;

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pBranchName";
                paramDiscreteValue2.Value = reportParam.BranchName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pReportName";
                paramDiscreteValue3.Value = ReportName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);
                paramFields.Add(paramField3);

                return bllReportManagement.GetReport_SumStockOnHandWarehouse(allBrandCode, countDate, allDepartmentCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_SumStockOnHandByBrandGroupFreshFood(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allBrandCode = reportParam.BrandCode;
                DateTime countDate = reportParam.CountDate;

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pBranchName";
                paramDiscreteValue2.Value = reportParam.BranchName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pReportName";
                paramDiscreteValue3.Value = ReportName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);
                paramFields.Add(paramField3);

                return bllReportManagement.GetReport_SumStockOnHandFreshFood(allBrandCode, countDate, allDepartmentCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_SectionLocationByBrandGroup(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allSectionCode = reportParam.SectionCode;
                string allStoreType = reportParam.StoreType;
                string allBrandCode = reportParam.BrandCode;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                string subject = "";
                string[] storeType = allStoreType.Split(',');
                Array.Sort(storeType, StringComparer.InvariantCulture);

                foreach (string storeCode in storeType)
                {
                    int code = Int32.Parse(storeCode);
                    StoreType enumStoreName = (StoreType)code;
                    subject = subject + " " + enumStoreName.ToString() + ",";
                }

                subject = subject.Trim();
                subject = subject.Substring(0, subject.Length - 1);

                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();
                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                paramField.Name = "@Subject";
                paramDiscreteValue.Value = subject;
                paramField.CurrentValues.Add(paramDiscreteValue);
                paramField1.Name = "pBranchName";
                paramDiscreteValue1.Value = reportParam.BranchName;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pReportName";
                paramDiscreteValue2.Value = ReportName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramFields.Add(paramField);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);

                return bllReportManagement.LoadReport_SectionLocationByBrandGroup(allSectionCode, allStoreType, allDepartmentCode, allLocationCode, allBrandCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_StocktakingAuditCheckWithUnit(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allSectionCode = reportParam.SectionCode;
                string allBrandCode = reportParam.BrandCode;
                string allStoreType = reportParam.StoreType;
                DateTime countDate = reportParam.CountDate;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();
                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                paramField.Name = "pCountDate";
                paramDiscreteValue.Value = reportParam.CountDate;
                paramField.CurrentValues.Add(paramDiscreteValue);
                paramField1.Name = "pBranchName";
                paramDiscreteValue1.Value = reportParam.BranchName;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pReportName";
                paramDiscreteValue2.Value = ReportName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                //paramField3.Name = "pStocktaker";
                //paramDiscreteValue3.Value = "Test";
                //paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);
                //paramFields.Add(paramField3);

                return bllReportManagement.LoadReport_StocktakingAuditCheckWithUnit(allLocationCode, allStoreType, countDate, allDepartmentCode, allSectionCode, allBrandCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_StocktakingAuditCheck(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allSectionCode = reportParam.SectionCode;
                string allBrandCode = reportParam.BrandCode;
                string allStoreType = reportParam.StoreType;
                DateTime countDate = reportParam.CountDate;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();
                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                //ParameterField paramField3 = new ParameterField();
                //ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                paramField.Name = "pCountDate";
                paramDiscreteValue.Value = reportParam.CountDate;
                paramField.CurrentValues.Add(paramDiscreteValue);
                paramField1.Name = "pBranchName";
                paramDiscreteValue1.Value = reportParam.BranchName;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pReportName";
                paramDiscreteValue2.Value = ReportName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                //paramField3.Name = "pStocktaker";
                //paramDiscreteValue3.Value = "Test";
                //paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);
                //paramFields.Add(paramField3);

                return bllReportManagement.LoadReport_StocktakingAuditCheck(allLocationCode, allStoreType, countDate, allDepartmentCode, allSectionCode, allBrandCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_DeleteRecordReportByLocation(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allSectionCode = reportParam.SectionCode;
                string allBrandCode = reportParam.BrandCode;
                string allStoreType = reportParam.StoreType;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

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

                return bllReportManagement.LoadReport_DeleteRecordReportByLocation(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_DeleteRecordReportBySection(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                string allStoreType = reportParam.StoreType;
                string allSectionCode = reportParam.SectionCode;
                string allBrandCode = reportParam.BrandCode;

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

                return bllReportManagement.LoadReport_DeleteRecordReportBySection(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_StocktakingAuditAdjustWithUnit(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allSectionCode = reportParam.SectionCode;
                string allBrandCode = reportParam.BrandCode;
                string allStoreType = reportParam.StoreType;
                DateTime countDate = reportParam.CountDate;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                string allCorrectDelete = reportParam.CorrectDelete;
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

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

                return bllReportManagement.LoadReport_StocktakingAuditAdjustWithUnit(allLocationCode, allStoreType, countDate, allDepartmentCode, allSectionCode, allBrandCode, allCorrectDelete);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_StocktakingAuditAdjust(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allSectionCode = reportParam.SectionCode;
                string allSectionName = string.Empty;
                string allBrandCode = reportParam.BrandCode;
                string allStoreType = reportParam.StoreType;
                string allCorrectDelete = reportParam.CorrectDelete;
                DateTime countDate = reportParam.CountDate;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

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

                return bllReportManagement.LoadReport_StocktakingAudiAdjust(allLocationCode, allStoreType, allCorrectDelete, countDate, allDepartmentCode, allSectionCode, allBrandCode, allSectionName);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_ControlSheet(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allSectionCode = reportParam.SectionCode;
                string allSectionName = string.Empty;
                string allStoreType = reportParam.StoreType;
                DateTime countDate = reportParam.CountDate;
                string allBrandCode = reportParam.BrandCode;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pBranchName";
                paramDiscreteValue2.Value = reportParam.BranchName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pReportName";
                paramDiscreteValue3.Value = ReportName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField3);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);

                return bllReportManagement.GetReport_ControlSheet(allSectionCode, allStoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode, allSectionName);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_UncountedLocation(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allSectionCode = reportParam.SectionCode;
                string allStoreType = reportParam.StoreType;
                DateTime countDate = reportParam.CountDate;
                string allBrandCode = reportParam.BrandCode;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pBranchName";
                paramDiscreteValue2.Value = reportParam.BranchName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pReportName";
                paramDiscreteValue3.Value = ReportName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField3);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);

                return bllReportManagement.GetReport_UncountedLocation(allSectionCode, allStoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_UnidentifiedStockItems(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allSectionCode = reportParam.SectionCode;
                string allBrandCode = reportParam.BrandCode;
                string allStoreType = reportParam.StoreType;
                DateTime countDate = reportParam.CountDate;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

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

                return bllReportManagement.LoadReport_UnidentifiedStockItem(allLocationCode, allStoreType, countDate, allDepartmentCode, allSectionCode, allBrandCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_InventoryControlDifferenceBySection(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allSectionCode = reportParam.SectionCode;
                string allStoreType = reportParam.StoreType;
                string allDifftype = reportParam.DiffType;
                string allUnit = reportParam.Unit;

                if (reportParam.Unit == "1" || reportParam.Unit == "2")
                {
                    allUnit = "1,2";
                }
                string allDepartmentCode = reportParam.DepartmentCode;
                string allLocationCode = "";
                int length = 5;
                string[] locationCode = reportParam.LocationCode.Split('-');

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                string allBrandCode = reportParam.BrandCode;

                if (allDifftype.Contains("3"))
                {
                    allDifftype = "1,2,3";
                }

                ParameterField paramField1 = new ParameterField();
                ParameterField paramField2 = new ParameterField();
                ParameterField paramField3 = new ParameterField();
                ParameterField paramField4 = new ParameterField();
                ParameterField paramField5 = new ParameterField();
                ParameterField paramField6 = new ParameterField();
                ParameterField paramField7 = new ParameterField();
                ParameterField paramField8 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue4 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue5 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue6 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue7 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue8 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramFields.Add(paramField1);

                paramField2.Name = "pCriteria";
                paramDiscreteValue2.Value = allSectionCode == string.Empty ? "All" : allSectionCode;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramFields.Add(paramField2);

                paramField3.Name = "pHeaderReportName";
                var reportName = string.Empty;
                reportName = "All Barcode ( ";

                var spitStore = reportParam.StoreType.Split(',').ToList().OrderBy(x => x.First());
                var storeType = string.Empty;
                if (spitStore.Count() == 4)
                {
                    storeType = "All";
                }
                else
                {
                    foreach (var item in spitStore)
                    {
                        if (item == "1") { storeType += StoreType.Front.ToString() + ", "; }
                        else if (item == "2") { storeType += StoreType.Back.ToString() + ", "; }
                        else if (item == "3") { storeType += StoreType.Warehouse.ToString() + ", "; }
                        else { storeType += StoreType.FreshFood.ToString() + ", "; }
                    }
                    storeType = storeType.Substring(0, storeType.Length - 2);
                }
                var diffType = string.Empty;
                if (reportParam.DiffType == "3,2,1")
                {
                    diffType = "ทั้งหมด";
                }
                else
                {
                    var spitDiffType = reportParam.DiffType.Split(',').ToList().OrderBy(x => x.First());
                    foreach (var item in spitDiffType)
                    {
                        if (item == "1") { diffType += DiffType.Shortage.ToString() + ", "; }
                        else if (item == "2") { diffType += DiffType.Over.ToString() + ", "; }
                        else { diffType += DiffType.All.ToString() + ", "; }
                    }
                    diffType = diffType.Substring(0, diffType.Length - 2);
                }
                string pDepartmentCode = string.Empty;
                //DataTable dtDepartmentCode = bllReportManagement.LoadReport_InventoryControlBySection(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, string.Empty, reportParam.Unit);
                DataTable dataAll = bllReportManagement.LoadReport_InventoryControlBySection(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, "DATA", reportParam.Unit);
                var listAllDepart = new List<string>();
                foreach (DataRow row in dataAll.Rows)
                {
                    listAllDepart.Add(row[3].ToString());
                }
                var listDapert = listAllDepart.Where(x => x != String.Empty).Distinct();
                foreach (var item in listDapert)
                {
                    pDepartmentCode += item.ToString() + ", ";
                }
                if (pDepartmentCode == string.Empty)
                {
                    pDepartmentCode = "None";
                }
                else
                {
                    pDepartmentCode = pDepartmentCode.Substring(0, pDepartmentCode.Length - 2);
                }

                paramDiscreteValue3.Value = reportName + storeType + " , " + diffType + ")";
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField3);

                paramField4.Name = "pBranchName";
                paramDiscreteValue4.Value = reportParam.BranchName;
                paramField4.CurrentValues.Add(paramDiscreteValue4);
                paramFields.Add(paramField4);

                paramField5.Name = "pDepartmentCode";
                paramDiscreteValue5.Value = "(Department : " + pDepartmentCode + ")";
                paramField5.CurrentValues.Add(paramDiscreteValue5);
                paramFields.Add(paramField5);

                paramField6.Name = "pReportName";
                paramDiscreteValue6.Value = ReportName;
                paramField6.CurrentValues.Add(paramDiscreteValue6);
                paramFields.Add(paramField6);

                paramField7.Name = "pUnitCode";
                paramDiscreteValue7.Value = reportParam.Unit;
                paramDiscreteValue7.Description = reportParam.Unit;
                paramField7.CurrentValues.Add(paramDiscreteValue7);
                paramFields.Add(paramField7);

                paramField8.Name = "pBrandName";
                paramDiscreteValue8.Value = reportParam.BrandName;
                paramField8.CurrentValues.Add(paramDiscreteValue8);
                paramFields.Add(paramField8);

                return dataAll;

                //return bllReportManagement.LoadReport_InventoryControlBySection(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, "DATA", reportParam.Unit);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_InventoryControlDifferenceByLocation(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allSectionCode = reportParam.SectionCode;
                string allStoreType = reportParam.StoreType;
                string allDifftype = reportParam.DiffType;
                string allUnit = reportParam.Unit;
                if (reportParam.Unit == "1" || reportParam.Unit == "2")
                {
                    allUnit = "1,2";
                }
                string allDepartmentCode = reportParam.DepartmentCode;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                string allBrandCode = reportParam.BrandCode;

                if (allDifftype.Contains("3"))
                {
                    allDifftype = "1,2,3";
                }

                ParameterField paramField1 = new ParameterField();
                ParameterField paramField2 = new ParameterField();
                ParameterField paramField3 = new ParameterField();
                ParameterField paramField4 = new ParameterField();
                ParameterField paramField5 = new ParameterField();
                ParameterField paramField6 = new ParameterField();
                ParameterField paramField7 = new ParameterField();
                ParameterField paramField8 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue4 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue5 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue6 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue7 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue8 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramFields.Add(paramField1);

                paramField2.Name = "pCriteria";
                paramDiscreteValue2.Value = allSectionCode == string.Empty ? "All" : allSectionCode;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramFields.Add(paramField2);

                paramField3.Name = "pHeaderReportName";
                var reportName = string.Empty;
                reportName = "All Barcode ( ";

                var spitStore = reportParam.StoreType.Split(',').ToList().OrderBy(x => x.First());
                var storeType = string.Empty;
                if (spitStore.Count() == 4)
                {
                    storeType = "All";
                }
                else
                {
                    foreach (var item in spitStore)
                    {
                        if (item == "1")
                        {
                            storeType += StoreType.Front.ToString() + ", ";
                        }
                        else if (item == "2")
                        {
                            storeType += StoreType.Back.ToString() + ", ";
                        }
                        else if (item == "3")
                        {
                            storeType += StoreType.Warehouse.ToString() + ", ";
                        }
                        else
                        {
                            storeType += StoreType.FreshFood.ToString() + ", ";
                        }
                    }
                    storeType = storeType.Substring(0, storeType.Length - 2);
                }


                var diffType = string.Empty;
                if (reportParam.DiffType == "3,2,1")
                {
                    diffType = "ทั้งหมด";
                }
                else
                {
                    var spitDiffType = reportParam.DiffType.Split(',').ToList().OrderBy(x => x.First());
                    foreach (var item in spitDiffType)
                    {
                        if (item == "1")
                        {
                            diffType += DiffType.Shortage.ToString() + ", ";
                        }
                        else if (item == "2")
                        {
                            diffType += DiffType.Over.ToString() + ", ";
                        }
                        else
                        {
                            diffType += DiffType.All.ToString() + ", ";
                        }
                    }
                    diffType = diffType.Substring(0, diffType.Length - 2);
                }
                string pDepartmentCode = string.Empty;
                //DataTable dtDepartmentCode = bllReportManagement.LoadReport_InventoryControlByLocation(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, string.Empty, reportParam.Unit);
                DataTable dataAll = bllReportManagement.LoadReport_InventoryControlByLocation(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, "DATA", reportParam.Unit);
                var listAllDepart = new List<string>();
                foreach (DataRow row in dataAll.Rows)
                {
                    listAllDepart.Add(row[2].ToString());
                }
                var listDapert = listAllDepart.Where(x => x != String.Empty).Distinct();
                foreach (var item in listDapert)
                {
                    pDepartmentCode += item.ToString() + ", ";
                }
                if (pDepartmentCode == string.Empty)
                {
                    pDepartmentCode = "None";
                }
                else
                {
                    pDepartmentCode = pDepartmentCode.Substring(0, pDepartmentCode.Length - 2);
                }

                paramDiscreteValue3.Value = reportName + storeType + " , " + diffType + ")";
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField3);

                paramField4.Name = "pBranchName";
                paramDiscreteValue4.Value = reportParam.BranchName;
                paramField4.CurrentValues.Add(paramDiscreteValue4);
                paramFields.Add(paramField4);

                paramField5.Name = "pDepartmentCode";
                paramDiscreteValue5.Value = "(Department : " + pDepartmentCode + ")";
                paramField5.CurrentValues.Add(paramDiscreteValue5);
                paramFields.Add(paramField5);

                paramField6.Name = "pReportName";
                paramDiscreteValue6.Value = ReportName;
                paramField6.CurrentValues.Add(paramDiscreteValue6);
                paramFields.Add(paramField6);

                paramField7.Name = "pUnitCode";
                paramDiscreteValue7.Value = reportParam.Unit;
                paramDiscreteValue7.Description = reportParam.Unit;
                paramField7.CurrentValues.Add(paramDiscreteValue7);
                paramFields.Add(paramField7);

                paramField8.Name = "pBrandName";
                paramDiscreteValue8.Value = reportParam.BrandName;
                paramField8.CurrentValues.Add(paramDiscreteValue8);
                paramFields.Add(paramField8);

                return dataAll;
                //return bllReportManagement.LoadReport_InventoryControlByLocation(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, "DATA", reportParam.Unit);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_InventoryControlDifferenceByBarcodeFrontBack(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allStoreType = reportParam.StoreType;
                string allDifftype = reportParam.DiffType;
                string allUnit = reportParam.Unit;
                if (reportParam.Unit == "1" || reportParam.Unit == "2")
                {
                    allUnit = "1,2";
                }
                string allBarcode = reportParam.Barcode;
                string allDepartmentCode = reportParam.DepartmentCode;
                string allSectionCode = reportParam.SectionCode;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                string allBrandCode = reportParam.BrandCode;

                if (allDifftype.Contains("3"))
                {
                    allDifftype = "1,2,3";
                }

                ParameterField paramField1 = new ParameterField();
                ParameterField paramField2 = new ParameterField();
                ParameterField paramField3 = new ParameterField();
                ParameterField paramField4 = new ParameterField();
                ParameterField paramField5 = new ParameterField();
                ParameterField paramField6 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue4 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue5 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue6 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramFields.Add(paramField1);

                paramField2.Name = "pHeaderReportName";
                var reportName = string.Empty;
                reportName = "All Barcode ( ";

                var spitStore = reportParam.StoreType.Split(',').ToList().OrderBy(x => x.First());
                var storeType = string.Empty;
                if (spitStore.Count() == 4)
                {
                    storeType = "All";
                }
                else
                {
                    foreach (var item in spitStore)
                    {
                        if (item == "1")
                        {
                            storeType += StoreType.Front.ToString() + ", ";
                        }
                        else if (item == "2")
                        {
                            storeType += StoreType.Back.ToString() + ", ";
                        }
                        else if (item == "3")
                        {
                            storeType += StoreType.Warehouse.ToString() + ", ";
                        }
                        else
                        {
                            storeType += StoreType.FreshFood.ToString() + ", ";
                        }
                    }
                    storeType = storeType.Substring(0, storeType.Length - 2);
                }
                var diffType = string.Empty;
                if (reportParam.DiffType == "3,2,1")
                {
                    diffType = "ทั้งหมด";
                }
                else
                {
                    var spitDiffType = reportParam.DiffType.Split(',').ToList().OrderBy(x => x.First());
                    foreach (var item in spitDiffType)
                    {
                        if (item == "1")
                        {
                            diffType += DiffType.Shortage.ToString() + ", ";
                        }
                        else if (item == "2")
                        {
                            diffType += DiffType.Over.ToString() + ", ";
                        }
                        else
                        {
                            diffType += DiffType.All.ToString() + ", ";
                        }
                    }
                    diffType = diffType.Substring(0, diffType.Length - 2);
                }
                string pDepartmentCode = string.Empty;
                //DataTable dtDepartmentCode = bllReportManagement.LoadReport_InventoryControlByBarcode(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, allBarcode, string.Empty, reportParam.Unit);
                //foreach (DataRow item in dtDepartmentCode.Rows)
                //{
                //    pDepartmentCode += item[0].ToString() + ", ";
                //}
                //if (pDepartmentCode == string.Empty)
                //{
                //    pDepartmentCode = "None";
                //}
                //else
                //{
                //    pDepartmentCode = pDepartmentCode.Substring(0, pDepartmentCode.Length - 2);
                //}

                paramDiscreteValue2.Description = storeType;
                paramDiscreteValue2.Value = reportName + storeType + " , " + diffType + ")";
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramFields.Add(paramField2);

                paramField3.Name = "pBranchName";
                paramDiscreteValue3.Value = reportParam.BranchName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField3);

                //paramField4.Name = "pDepartmentCode";
                //paramDiscreteValue4.Value = "(Department : " + pDepartmentCode + ")";
                //paramField4.CurrentValues.Add(paramDiscreteValue4);
                //paramFields.Add(paramField4);

                paramField5.Name = "pReportName";
                paramDiscreteValue5.Value = ReportName;
                paramField5.CurrentValues.Add(paramDiscreteValue5);
                paramFields.Add(paramField5);

                paramField6.Name = "pBrandName";
                paramDiscreteValue6.Value = reportParam.BrandName;
                paramField6.CurrentValues.Add(paramDiscreteValue6);
                paramFields.Add(paramField6);

                DataTable dataAll = bllReportManagement.LoadReport_InventoryControlByBarcode(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, allBarcode, "DATA", reportParam.Unit);

                var listAllDepart = new List<string>();
                foreach (DataRow row in dataAll.Rows)
                {
                    listAllDepart.Add(row[1].ToString());
                }
                var listDapert = listAllDepart.Where(x => x != String.Empty).Distinct();
                foreach (var item in listDapert)
                {
                    pDepartmentCode += item.ToString() + ", ";
                }
                if (pDepartmentCode == string.Empty)
                {
                    pDepartmentCode = "None";
                }
                else
                {
                    pDepartmentCode = pDepartmentCode.Substring(0, pDepartmentCode.Length - 2);
                }

                paramField4.Name = "pDepartmentCode";
                paramDiscreteValue4.Value = "(Department : " + pDepartmentCode + ")";
                paramField4.CurrentValues.Add(paramDiscreteValue4);
                paramFields.Add(paramField4);

                return dataAll;


                //return bllReportManagement.LoadReport_InventoryControlByBarcode(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, allBarcode, "DATA", reportParam.Unit);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_InventoryControlDifferenceByBarcodeWarehouseFreshFood(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allStoreType = reportParam.StoreType;
                string allDifftype = reportParam.DiffType;
                string allUnit = reportParam.Unit;
                if (reportParam.Unit == "1" || reportParam.Unit == "2")
                {
                    allUnit = "1,2";
                }
                string allBarcode = reportParam.Barcode;
                string allDepartmentCode = reportParam.DepartmentCode;
                string allSectionCode = reportParam.SectionCode;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                string allBrandCode = reportParam.BrandCode;

                if (allDifftype.Contains("3"))
                {
                    allDifftype = "1,2,3";
                }


                ParameterField paramField1 = new ParameterField();
                ParameterField paramField2 = new ParameterField();
                ParameterField paramField3 = new ParameterField();
                ParameterField paramField4 = new ParameterField();
                ParameterField paramField5 = new ParameterField();
                ParameterField paramField6 = new ParameterField();
                ParameterField paramField7 = new ParameterField();
                ParameterField paramField8 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue4 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue5 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue6 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue7 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue8 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramFields.Add(paramField1);

                paramField2.Name = "pHeaderReportName";
                var reportName = string.Empty;
                reportName = "All Barcode ( ";

                var spitStore = reportParam.StoreType.Split(',').ToList().OrderBy(x => x.First());
                var storeType = string.Empty;
                if (spitStore.Count() == 4)
                {
                    storeType = "All";
                }
                else
                {
                    foreach (var item in spitStore)
                    {
                        if (item == "1")
                        {
                            storeType += StoreType.Front.ToString() + ", ";
                        }
                        else if (item == "2")
                        {
                            storeType += StoreType.Back.ToString() + ", ";
                        }
                        else if (item == "3")
                        {
                            storeType += StoreType.Warehouse.ToString() + ", ";
                        }
                        else
                        {
                            storeType += StoreType.FreshFood.ToString() + ", ";
                        }
                    }
                    storeType = storeType.Substring(0, storeType.Length - 2);
                }
                var diffType = string.Empty;
                if (reportParam.DiffType == "3,2,1")
                {
                    diffType = "ทั้งหมด";
                }
                else
                {
                    var spitDiffType = reportParam.DiffType.Split(',').ToList().OrderBy(x => x.First());
                    foreach (var item in spitDiffType)
                    {
                        if (item == "1")
                        {
                            diffType += DiffType.Shortage.ToString() + ", ";
                        }
                        else if (item == "2")
                        {
                            diffType += DiffType.Over.ToString() + ", ";
                        }
                        else
                        {
                            diffType += DiffType.All.ToString() + ", ";
                        }
                    }
                    diffType = diffType.Substring(0, diffType.Length - 2);
                }
                string pDepartmentCode = string.Empty;
                //DataTable dtDepartmentCode = bllReportManagement.LoadReport_InventoryControlByBarcode(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, allBarcode, string.Empty, reportParam.Unit);
                DataTable dataAll = bllReportManagement.LoadReport_InventoryControlByBarcode(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, allBarcode, "DATA", reportParam.Unit);

                var listAllDepart = new List<string>();
                foreach (DataRow row in dataAll.Rows)
                {
                    listAllDepart.Add(row[1].ToString());
                }
                var listDapert = listAllDepart.Where(x => x != String.Empty).Distinct();
                foreach (var item in listDapert)
                {
                    pDepartmentCode += item.ToString() + ", ";
                }
                if (pDepartmentCode == string.Empty)
                {
                    pDepartmentCode = "None";
                }
                else
                {
                    pDepartmentCode = pDepartmentCode.Substring(0, pDepartmentCode.Length - 2);
                }

                paramDiscreteValue2.Value = reportName + storeType + " , " + diffType + ")";
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramFields.Add(paramField2);

                paramField3.Name = "pSectionType";
                paramDiscreteValue3.Value = allStoreType == "3" ? "Warehouse" : "Fresh Food";
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField3);

                paramField4.Name = "pBranchName";
                paramDiscreteValue4.Value = reportParam.BranchName;
                paramField4.CurrentValues.Add(paramDiscreteValue4);
                paramFields.Add(paramField4);

                paramField5.Name = "pDepartmentCode";
                paramDiscreteValue5.Value = "(Department : " + pDepartmentCode + ")";
                paramField5.CurrentValues.Add(paramDiscreteValue5);
                paramFields.Add(paramField5);

                paramField6.Name = "pReportName";
                paramDiscreteValue6.Value = ReportName;
                paramField6.CurrentValues.Add(paramDiscreteValue6);
                paramFields.Add(paramField6);

                paramField7.Name = "pUnitCode";
                paramDiscreteValue7.Value = reportParam.Unit;
                paramDiscreteValue7.Description = reportParam.Unit;
                paramField7.CurrentValues.Add(paramDiscreteValue7);
                paramFields.Add(paramField7);

                paramField8.Name = "pBrandName";
                paramDiscreteValue8.Value = reportParam.BrandName;
                paramField8.CurrentValues.Add(paramDiscreteValue8);
                paramFields.Add(paramField8);

                return dataAll;

                //return bllReportManagement.LoadReport_InventoryControlByBarcode(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, allBarcode, "DATA", reportParam.Unit);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataSet GetReport_ItemPhysicalCountBySection(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allBrandCode = reportParam.BrandCode;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                string allSectionCode = reportParam.SectionCode;
                string allStoreType = reportParam.StoreType;
                DateTime countDate = reportParam.CountDate;
                string allSectionForDisplay = "";

                if (allSectionCode == "")
                {
                    allSectionForDisplay = "All Section";
                }
                else
                {
                    allSectionForDisplay = allSectionCode;
                }

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                ParameterField paramField4 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue4 = new ParameterDiscreteValue();
                ParameterField paramField5 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue5 = new ParameterDiscreteValue();
                ParameterField paramField6 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue6 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "selectedBrand";
                paramDiscreteValue2.Value = allSectionForDisplay;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pBranchName";
                paramDiscreteValue3.Value = reportParam.BranchName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramField4.Name = "pReportName";
                paramDiscreteValue4.Value = ReportName;
                paramField4.CurrentValues.Add(paramDiscreteValue4);
                paramField5.Name = "pStoreType";
                paramDiscreteValue5.Value = allStoreType;
                paramField5.CurrentValues.Add(paramDiscreteValue5);
                paramField6.Name = "pBrandName";
                paramDiscreteValue6.Value = reportParam.BrandName;
                paramField6.CurrentValues.Add(paramDiscreteValue6);
                paramFields.Add(paramField6);
                paramFields.Add(paramField5);
                paramFields.Add(paramField4);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);
                paramFields.Add(paramField3);

                return bllReportManagement.GetReport_ItemPhysicalCountBySection(allSectionCode, allStoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataSet();
            }
        }

        private DataSet GetReport_ItemPhysicalCountByBarcode(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allSectionCode = reportParam.SectionCode;
                string allBrandCode = reportParam.BrandCode;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                string allBarcode = reportParam.Barcode;
                string allStoreType = reportParam.StoreType;
                DateTime countDate = reportParam.CountDate;
                string allBarcodeForDisplay = "";

                if (allSectionCode == "")
                {
                    allBarcodeForDisplay = "All Section";
                }
                else
                {
                    allBarcodeForDisplay = allSectionCode;
                }

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                ParameterField paramField4 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue4 = new ParameterDiscreteValue();
                ParameterField paramField5 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue5 = new ParameterDiscreteValue();
                ParameterField paramField6 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue6 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "selectedBrand";
                paramDiscreteValue2.Value = allBarcodeForDisplay;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pBranchName";
                paramDiscreteValue3.Value = reportParam.BranchName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramField4.Name = "pReportName";
                paramDiscreteValue4.Value = ReportName;
                paramField4.CurrentValues.Add(paramDiscreteValue4);
                paramField5.Name = "pStoreType";
                paramDiscreteValue5.Value = allStoreType;
                paramField5.CurrentValues.Add(paramDiscreteValue5);
                paramField6.Name = "pBrandName";
                paramDiscreteValue6.Value = reportParam.BrandName;
                paramField6.CurrentValues.Add(paramDiscreteValue6);
                paramFields.Add(paramField6);
                paramFields.Add(paramField5);
                paramFields.Add(paramField4);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);
                paramFields.Add(paramField3);



                return bllReportManagement.GetReport_ItemPhysicalCountByBarcode(allBarcode, allStoreType, countDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataSet();
            }
        }

        private DataTable GetReport_GroupSummaryReportByFrontBack(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allBrandCode = reportParam.BrandCode;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                string allSectionCode = reportParam.SectionCode;
                string allStoreType = reportParam.StoreType;
                DateTime countDate = reportParam.CountDate;

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pBranchName";
                paramDiscreteValue2.Value = reportParam.BranchName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pReportName";
                paramDiscreteValue3.Value = ReportName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField3);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);

                return bllReportManagement.loadReport_GroupSummaryReportByFrontBack(allSectionCode, allStoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_GroupSummaryReportByFreshFoodWarehouse(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allBrandCode = reportParam.BrandCode;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                string allSectionCode = reportParam.SectionCode;
                string allStoreType = reportParam.StoreType;
                DateTime countDate = reportParam.CountDate;
                string reportName = string.Empty;
                if (allStoreType == "3")
                {
                    reportName = "26 Group Summary Report(Warehouse)";
                }
                else if (allStoreType == "4")
                {
                    reportName = "26 Group Summary Report(Fresh Food)";
                }
                else
                {
                    reportName = ReportName;
                }

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pBranchName";
                paramDiscreteValue2.Value = reportParam.BranchName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pReportName";
                paramDiscreteValue3.Value = reportName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField3);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);

                return bllReportManagement.loadReport_GroupSummaryReportByFreshFoodWarehouse(allSectionCode, allStoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_CountedLocationsReport(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allBrandCode = reportParam.BrandCode;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                string allSectionCode = reportParam.SectionCode;
                string allStoreType = reportParam.StoreType;
                DateTime countDate = reportParam.CountDate;

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();

                paramField1.Name = "pBranchName";
                paramDiscreteValue1.Value = reportParam.BranchName;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramFields.Add(paramField1);

                paramField2.Name = "pReportName";
                paramDiscreteValue2.Value = ReportName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramFields.Add(paramField2);

                return bllReportManagement.LoadReport_CountedLocationsReport(allSectionCode, allStoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_NoticeOfStocktakingSatisfactionByFrontBack(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allBrandCode = reportParam.BrandCode;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                string allSectionCode = reportParam.SectionCode;
                string allStoreType = reportParam.StoreType;
                DateTime countDate = reportParam.CountDate;

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                ParameterField paramField4 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue4 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pBranchName";
                paramDiscreteValue2.Value = reportParam.BranchName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pReportName";
                paramDiscreteValue3.Value = ReportName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramField4.Name = "pBrandName";
                paramDiscreteValue4.Value = reportParam.BrandName;
                paramField4.CurrentValues.Add(paramDiscreteValue4);

                paramFields.Add(paramField1);
                paramFields.Add(paramField2);
                paramFields.Add(paramField3);
                paramFields.Add(paramField4);

                return bllReportManagement.LoadReport_NoticeOfStocktakingSatisfactionByFrontBack(allSectionCode, allStoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }

        private DataTable GetReport_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.DepartmentCode;
                string allBrandCode = reportParam.BrandCode;
                string allLocationCode = "";
                string[] locationCode = reportParam.LocationCode.Split('-');
                int length = 5;

                if (locationCode.Length == 1 && !string.IsNullOrWhiteSpace(locationCode[0]))
                {
                    //allLocationCode = int.Parse(locationCode[0]).ToString("D" + length);
                    allLocationCode = locationCode[0];
                }
                else if (locationCode.Length > 1)
                {
                    int locationCodeFrom = int.Parse(locationCode[0]);
                    int locationCodeTo = int.Parse(locationCode[locationCode.Length - 1]);
                    for (int i = 0; locationCodeFrom <= locationCodeTo; i++)
                    {
                        if (i == 0)
                        {
                            allLocationCode = locationCodeFrom.ToString("D" + length);
                        }
                        else
                        {
                            allLocationCode += "," + (locationCodeFrom).ToString("D" + length);
                        }

                        locationCodeFrom++;
                    }
                }
                else
                {
                    allLocationCode = reportParam.LocationCode;
                }

                string allSectionCode = reportParam.SectionCode;
                string allStoreType = reportParam.StoreType;
                DateTime countDate = reportParam.CountDate;
                string reportName = string.Empty;
                if (allStoreType == "3")
                {
                    reportName = "22 Notice of Stocktaking Satisfaction (Warehouse)";
                }
                else if (allStoreType == "4")
                {
                    reportName = "22 Notice of Stocktaking Satisfaction (Fresh Food)";
                }
                else
                {
                    reportName = ReportName;
                }

                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                ParameterField paramField6 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue6 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pBranchName";
                paramDiscreteValue2.Value = reportParam.BranchName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pReportName";
                paramDiscreteValue3.Value = reportName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramField6.Name = "pBrandName";
                paramDiscreteValue6.Value = reportParam.BrandName;
                paramField6.CurrentValues.Add(paramDiscreteValue6);

                paramFields.Add(paramField1);
                paramFields.Add(paramField2);
                paramFields.Add(paramField3);
                paramFields.Add(paramField6);

                DataTable result1 = new DataTable();
                DataTable result2 = new DataTable();
                Hashtable result = new Hashtable();
                result = bllReportManagement.LoadReport_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse(allSectionCode, allStoreType, countDate, allDepartmentCode, allLocationCode, allBrandCode);
                result1 = (DataTable)result["dtTable1"];
                result2 = (DataTable)result["dtTable2"];

                ParameterField paramField4 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue4 = new ParameterDiscreteValue();
                ParameterField paramField5 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue5 = new ParameterDiscreteValue();
                paramField4.Name = "pNumberCountLocation";
                paramDiscreteValue4.Value = result2.Rows[0][0];
                paramField4.CurrentValues.Add(paramDiscreteValue4);
                paramField5.Name = "pNumberUncountLocation";
                paramDiscreteValue5.Value = result2.Rows[0][1];
                paramField5.CurrentValues.Add(paramDiscreteValue5);
                paramFields.Add(paramField4);
                paramFields.Add(paramField5);

                return result1;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return new DataTable();
            }
        }
    }
}
