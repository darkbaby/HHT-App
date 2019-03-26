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
using System.Reflection;

namespace FSBT.HHT.App.UI
{
    public partial class ReportPrintForm : Form
    {
        private LogErrorBll logBll = new LogErrorBll(); 
        private PermissionComponentBll permissionComponent = null;
        private ReportManagementBll bllReportManagement = new ReportManagementBll();
        private LocationManagementBll bllLocal = new LocationManagementBll();
        private SystemSettingBll bllSystemSetting = new SystemSettingBll();
        private List<ConfigReport> reportConfig = new List<ConfigReport>();
        ReportManagementForm reportForm = new ReportManagementForm();
        public string UserName;
        public string ReportCode;
        private DateTime selectedCountDate = DateTime.Now;
        public string scanMode = "";
        public string storageLocationType = "";
        public string MCH1 = "";
        public string MCH2 = "";
        public string MCH3 = "";
        public string MCH4 = "";

        public ReportPrintForm(string username, string reportCode)
        {
            UserName = username;
            ReportCode = reportCode;
            InitializeComponent();
            storageLocationType = bllSystemSetting.GetSettingStringByKey("StorageLocationType");
            MCH1 = bllSystemSetting.GetSettingStringByKey("MCHLevel1");
            MCH2 = bllSystemSetting.GetSettingStringByKey("MCHLevel2");
            MCH3 = bllSystemSetting.GetSettingStringByKey("MCHLevel3");
            MCH4 = bllSystemSetting.GetSettingStringByKey("MCHLevel4");
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void SetupCriteriaComponent(string reportCode)
        {
            lblReportName.Text = lstReport.Text;
            List<string> reportComponent = GetReportComponentByReport(reportCode);
            List<ConfigReport> reportComponentList = GetReportComponentListByReport(reportCode);
            var listSettingData = bllSystemSetting.GetSettingData();
            string sectionType = listSettingData.SectionType;

            //if (storageLocationType == "BRAND")
            //{
            //    AddDropDownBrand(); //use in all report
            //    rbSectionAll.Enabled = false;
            //    rbSectionCode.Enabled = false;
            //    SectionCode.Enabled = false;
            //}
            //else
            //{
            //    AddDropDownBrand(); //use in all report
            //    rbBrand.Enabled = false;
            //    rbBrandAll.Enabled = false;
            //    comboBoxBrandCode.Enabled = false;
            //}

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

                                    if (storageLocationType == "BRAND")
                                    {
                                        if (component.Name.Equals("rbSectionAll") || component.Name.Equals("rbSectionCode"))
                                        {
                                            component.Enabled = false;
                                        }
                                    }
                                    else
                                    {
                                        if (component.Name.Equals("rbBrandAll") || component.Name.Equals("rbBrand"))
                                        {
                                            component.Enabled = false;
                                        }
                                    }

                                }
                                else if ((component is RadioButton) && String.IsNullOrEmpty(defaulValue))
                                {
                                    ((RadioButton)(component)).Checked = false;

                                    if (storageLocationType == "BRAND")
                                    {
                                        if (component.Name.Equals("rbSectionAll") || component.Name.Equals("rbSectionCode"))
                                        {
                                            component.Enabled = false;
                                        }
                                    }
                                    else
                                    {
                                        if (component.Name.Equals("rbBrandAll") || component.Name.Equals("rbBrand"))
                                        {
                                            component.Enabled = false;
                                        }
                                    }
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
                /*
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
                    */

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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

                //Set Plant Code
                if (rbPlantAll.Checked)
                {
                    reportParam.Plant = "";
                }

                if (rbPlant.Checked)
                {
                    comboBoxPlant.Text = removeUnusedComma(comboBoxPlant.Text);
                    if (!String.IsNullOrEmpty(comboBoxPlant.Text))
                    {
                        reportParam.Plant = comboBoxPlant.Text;
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.PleaseinsertPlantCode, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                }

                //Set Count Sheet
                if (rbCountSheetAll.Checked)
                {
                    reportParam.CountSheet = "";
                }
                if (rbCountSheet.Checked)
                {
                    comboBoxCountSheet.Text = removeUnusedComma(comboBoxCountSheet.Text);
                    if (!String.IsNullOrEmpty(comboBoxCountSheet.Text))
                    {
                        reportParam.CountSheet = comboBoxCountSheet.Text;

                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.PleaseinsertCountSheet, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                }

                //Set MCH Level 1
                if (rbMCH1All.Checked)
                {
                    reportParam.MCH1 = "";
                }
                if (rbMCH1.Checked)
                {
                    comboBoxLevel1.Text = removeUnusedComma(comboBoxLevel1.Text);
                    if (!String.IsNullOrEmpty(comboBoxLevel1.Text))
                    {
                        reportParam.MCH1 = comboBoxLevel1.Text;

                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.PleaseinsertMCHLevel1, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                }

                //Set MCH Level 2
                if (rbMCH2All.Checked)
                {
                    reportParam.MCH2 = "";
                }
                if (rbMCH2.Checked)
                {
                    comboBoxLevel2.Text = removeUnusedComma(comboBoxLevel2.Text);
                    if (!String.IsNullOrEmpty(comboBoxLevel2.Text))
                    {
                        reportParam.MCH2 = comboBoxLevel2.Text;
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.PleaseinsertMCHLevel2, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                }

                //Set MCH Level 3
                if (rbMCH3All.Checked)
                {
                    reportParam.MCH3 = "";
                }
                if (rbMCH3.Checked)
                {
                    comboBoxLevel3.Text = removeUnusedComma(comboBoxLevel3.Text);
                    if (!String.IsNullOrEmpty(comboBoxLevel3.Text))
                    {
                        reportParam.MCH3 = comboBoxLevel3.Text;
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.PleaseinsertMCHLevel3, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                }

                //Set MCH Level 4
                if (rbMCH4All.Checked)
                {
                    reportParam.MCH4 = "";
                }
                if (rbMCH4.Checked)
                {
                    comboBoxLevel4.Text = removeUnusedComma(comboBoxLevel4.Text);
                    if (!String.IsNullOrEmpty(comboBoxLevel4.Text))
                    {
                        reportParam.MCH4 = comboBoxLevel4.Text;
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.PleaseinsertMCHLevel4, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                }

                //Set Storage Location
                if (rbStorageLocAll.Checked)
                {
                    reportParam.StoregeLocation = "";
                }
                if (rbStorageLoc.Checked)
                {
                    comboBoxStorageLocation.Text = removeUnusedComma(comboBoxStorageLocation.Text);
                    if (!String.IsNullOrEmpty(comboBoxStorageLocation.Text))
                    {
                        reportParam.StoregeLocation = comboBoxStorageLocation.Text.Substring(0,4);
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.PleaseinsertStoregeLocation, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    SectionCode.Text = removeUnusedComma(SectionCode.Text);
                    if (!String.IsNullOrEmpty(SectionCode.Text))
                    {
                        List<string> sectionList = bllReportManagement.GetSectionCodeList();
                        string errorExist = validateInList(SectionCode.Text, sectionList);

                        if (errorExist.Length == 0)
                        {
                            reportParam.SectionCode = SectionCode.Text;
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
                    Location.Text = removeUnusedComma(Location.Text);
                    if (!String.IsNullOrEmpty(Location.Text))
                    {
                        string errorExist = validateInList(Location.Text, locationList);

                        if (errorExist.Length == 0)
                        {
                            reportParam.LocationCode = Location.Text;
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
                    if (comboBoxBrandCode.Items.Count == 0)
                    {
                        MessageBox.Show(MessageConstants.PleaseinsertBrandCode, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return reportParam = null;
                    }
                    else
                    {
                        //reportParam.BrandCode = comboBoxBrandCode.SelectedValue.ToString();

                        reportParam.BrandCode = ((ReportMasterBrand)comboBoxBrandCode.SelectedItem).BrandCode.ToString();
                        reportParam.BrandName = ((ReportMasterBrand)comboBoxBrandCode.SelectedItem).BrandName.ToString();
                    }

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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                scanMode = listSettingData.ScanMode;
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

                if (storageLocationType == "BRAND")
                {
                    AddDropDownBrand(); //use in all report
                    rbSectionAll.Enabled = false;
                    rbSectionCode.Enabled = false;
                    SectionCode.Enabled = false;
                }
                else
                {
                    AddDropDownBrand(); //use in all report
                    rbBrand.Enabled = false;
                    rbBrandAll.Enabled = false;
                    comboBoxBrandCode.Enabled = false;
                }

                AddDropDownStorageLocation();

                gbMCH1.Text = MCH1;
                rbMCH1.Text = MCH1;
                gbMCH2.Text = MCH2;
                rbMCH2.Text = MCH2;
                gbMCH3.Text = MCH3;
                rbMCH3.Text = MCH3;
                gbMCH4.Text = MCH4;
                rbMCH4.Text = MCH4;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void lstReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupCriteriaComponent(lstReport.SelectedValue.ToString());

            if (storageLocationType == "BRAND")
            {
                AddDropDownBrand(); //use in all report
                rbSectionAll.Enabled = false;
                rbSectionCode.Enabled = false;
                SectionCode.Enabled = false;
            }
            else
            {
                AddDropDownBrand(); //use in all report
                rbBrand.Enabled = false;
                rbBrandAll.Enabled = false;
                comboBoxBrandCode.Enabled = false;
            }
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
                            if (scanMode == "P")
                            {
                                dt = GetReport_SumStockOnHand(reportParam, paramFields, reportName);
                            }
                            else
                            {
                                reportCode = "R03";
                                reportName = bllReportManagement.GetReportNameByReportCode(reportCode);
                                dt = GetReport_SumStockOnHandFreshFood(reportParam, paramFields, reportName);
                            }
                            break;

                        case "R02":
                            dt = GetReport_SumStockOnHandWarehouse(reportParam, paramFields, reportName);
                            break;

                        case "R04":
                            if (scanMode == "P")
                            {
                                dt = GetReport_SumStockOnHandByBrandGroup(reportParam, paramFields, reportName);
                            }
                            else
                            {
                                reportCode = "R06";
                                reportName = bllReportManagement.GetReportNameByReportCode(reportCode);
                                dt = GetReport_SumStockOnHandByBrandGroupFreshFood(reportParam, paramFields, reportName);
                            }
                            break;
                        case "R05":
                            dt = GetReport_SumStockOnHandByBrandGroupWarehouse(reportParam, paramFields, reportName);
                            break;
                        case "R07":
                            dt = GetReport_SectionLocationByBrandGroup(reportParam, paramFields, reportName);
                            break;
                        case "R08":
                            dt = GetReport_StocktakingAuditCheckWithUnit(reportParam, paramFields, reportName);
                            break;
                        case "R09":
                            dt = GetReport_StocktakingAuditAdjustWithUnit(reportParam, paramFields, reportName);
                            break;
                        case "R10":
                            dt = GetReport_UnidentifiedStockItems(reportParam, paramFields, reportName);
                            break;
                        case "R11":
                            dt = GetReport_DeleteRecordReportByLocation(reportParam, paramFields, reportName);
                            break;
                        case "R12":
                            dt = GetReport_ControlSheet(reportParam, paramFields, reportName);
                            break;
                        case "R13": 
                            if (scanMode == "P")
                            {
                                dt = GetReport_InventoryControlDifferenceByBarcodeFrontBack(reportParam, paramFields, reportName);
                            }
                            else
                            {
                                reportCode = "R15";
                                reportName = bllReportManagement.GetReportNameByReportCode(reportCode);
                                dt = GetReport_InventoryControlDifferenceByBarcodeFreshFood(reportParam, paramFields, reportName);
                            }
                            break;


                        case "R16":
                            if (scanMode == "P")
                            {
                                dt = GetReport_GroupSummaryReportByFrontBack(reportParam, paramFields, reportName);
                            }
                            else
                            {
                                reportCode = "R17";
                                reportName = bllReportManagement.GetReportNameByReportCode(reportCode);
                                dt = GetReport_GroupSummaryReportByFreshFoodWarehouse(reportParam, paramFields, reportName);
                            }
                            break;

                        case "R18":
                            dt = GetReport_CountedLocationsReport(reportParam, paramFields, reportName);
                            break;
                        case "R19":
                            dt = GetReport_UncountedLocation(reportParam, paramFields, reportName);
                            break;
                        case "R20":
                            dt = GetReport_ItemPhysicalCountByBarcode(reportParam, paramFields, reportName);
                            //dt = ds.Tables[0];
                            break;

                        case "R21":
                            if (scanMode == "P")
                            {
                                dt = GetReport_NoticeOfStocktakingSatisfactionByFrontBack(reportParam, paramFields, reportName);
                            }
                            else
                            {
                                reportCode = "R22";
                                reportName = bllReportManagement.GetReportNameByReportCode(reportCode);
                                dt = GetReport_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse(reportParam, paramFields, reportName); 
                            }

                            break;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        //if (reportCode == "R20")
                        //{
                        //    isCreateReportSuccess = reportForm.CreateReport(ds, reportCode, paramFields);
                        //}
                        //else
                        //{
                            isCreateReportSuccess = reportForm.CreateReport(dt, reportCode, paramFields);
                        //}
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

                        /*
                        if (reportCode == "R10" || reportCode == "R13" || reportCode == "R14" || reportCode == "R15")
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
                        
                        else
                        {
                        */
                            Loading_Screen.CloseForm();
                            MessageBox.Show(MessageConstants.Nodataforgeneratereport, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }


        private void rbPlantAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPlantAll.Checked)
            {
                comboBoxPlant.Enabled = false;
                comboBoxPlant.Text = "";
                rbCountSheet.Enabled = true;
                rbCountSheetAll.Enabled = true;
                rbCountSheetAll.Checked = true;
                comboBoxCountSheet.Enabled = false;
                comboBoxCountSheet.Text = "";

                rbMCH1.Enabled = true;
                rbMCH1All.Enabled = true;
                rbMCH1All.Checked = true;
                comboBoxLevel1.Enabled = false;
                comboBoxLevel1.Text = "";

                rbMCH2.Enabled = true;
                rbMCH2All.Enabled = true;
                rbMCH2All.Checked = true;
                comboBoxLevel2.Enabled = false;
                comboBoxLevel2.Text = "";

                rbMCH3.Enabled = true;
                rbMCH3All.Enabled = true;
                rbMCH3All.Checked = true;
                comboBoxLevel3.Enabled = false;
                comboBoxLevel3.Text = "";

                rbMCH4.Enabled = true;
                rbMCH4All.Enabled = true;
                rbMCH4All.Checked = true;
                comboBoxLevel4.Enabled = false;
                comboBoxLevel4.Text = "";
            }
            
        }
        private void rbPlant_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPlant.Checked)
            {
                comboBoxPlant.Enabled = true;
                AddDropDownPlant();

                rbCountSheet.Enabled = true;
                rbCountSheetAll.Enabled = true;
                rbCountSheetAll.Checked = true;
                comboBoxCountSheet.Enabled = false;
                comboBoxCountSheet.Text = "";

                rbMCH1.Enabled = true;
                rbMCH1All.Enabled = true;
                rbMCH1All.Checked = true;
                comboBoxLevel1.Enabled = false;
                comboBoxLevel1.Text = "";

                rbMCH2.Enabled = true;
                rbMCH2All.Enabled = true;
                rbMCH2All.Checked = true;
                comboBoxLevel2.Enabled = false;
                comboBoxLevel2.Text = "";

                rbMCH3.Enabled = true;
                rbMCH3All.Enabled = true;
                rbMCH3All.Checked = true;
                comboBoxLevel3.Enabled = false;
                comboBoxLevel3.Text = "";

                rbMCH4.Enabled = true;
                rbMCH4All.Enabled = true;
                rbMCH4All.Checked = true;
                comboBoxLevel4.Enabled = false;
                comboBoxLevel4.Text = "";
            }
        }

        private void comboBoxPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddDropDownCountSheet();

            rbCountSheet.Enabled = true;
            rbCountSheetAll.Enabled = true;
            rbCountSheetAll.Checked = true;
            comboBoxCountSheet.Enabled = false;
            comboBoxCountSheet.Text = "";

            rbMCH1.Enabled = true;
            rbMCH1All.Enabled = true;
            rbMCH1All.Checked = true;
            comboBoxLevel1.Enabled = false;
            comboBoxLevel1.Text = "";

            rbMCH2.Enabled = true;
            rbMCH2All.Enabled = true;
            rbMCH2All.Checked = true;
            comboBoxLevel2.Enabled = false;
            comboBoxLevel2.Text = "";

            rbMCH3.Enabled = true;
            rbMCH3All.Enabled = true;
            rbMCH3All.Checked = true;
            comboBoxLevel3.Enabled = false;
            comboBoxLevel3.Text = "";

            rbMCH4.Enabled = true;
            rbMCH4All.Enabled = true;
            rbMCH4All.Checked = true;
            comboBoxLevel4.Enabled = false;
            comboBoxLevel4.Text = "";
        }

        private void rbCountSheetAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCountSheetAll.Checked)
            {
                comboBoxCountSheet.Enabled = false;
                comboBoxCountSheet.Text = "";
                rbMCH1.Enabled = true;
                rbMCH1All.Enabled = true;
                rbMCH1All.Checked = true;
                comboBoxLevel1.Enabled = false;
                comboBoxLevel1.Text = "";

                rbMCH2.Enabled = true;
                rbMCH2All.Enabled = true;
                rbMCH2All.Checked = true;
                comboBoxLevel2.Enabled = false;
                comboBoxLevel2.Text = "";

                rbMCH3.Enabled = true;
                rbMCH3All.Enabled = true;
                rbMCH3All.Checked = true;
                comboBoxLevel3.Enabled = false;
                comboBoxLevel3.Text = "";

                rbMCH4.Enabled = true;
                rbMCH4All.Enabled = true;
                rbMCH4All.Checked = true;
                comboBoxLevel4.Enabled = false;
                comboBoxLevel4.Text = "";
            }
        }

        private void rbCountSheet_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCountSheet.Checked)
            {
                comboBoxCountSheet.Enabled = true;
                AddDropDownCountSheet();

                rbMCH1.Enabled = true;
                rbMCH1All.Enabled = true;
                rbMCH1All.Checked = true;
                comboBoxLevel1.Enabled = false;
                comboBoxLevel1.Text = "";

                rbMCH2.Enabled = true;
                rbMCH2All.Enabled = true;
                rbMCH2All.Checked = true;
                comboBoxLevel2.Enabled = false;
                comboBoxLevel2.Text = "";

                rbMCH3.Enabled = true;
                rbMCH3All.Enabled = true;
                rbMCH3All.Checked = true;
                comboBoxLevel3.Enabled = false;
                comboBoxLevel3.Text = "";

                rbMCH4.Enabled = true;
                rbMCH4All.Enabled = true;
                rbMCH4All.Checked = true;
                comboBoxLevel4.Enabled = false;
                comboBoxLevel4.Text = "";
            }
       }

        private void comboBoxCountSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddDropDownMCHLevel1();

            rbMCH1.Enabled = true;
            rbMCH1All.Enabled = true;
            rbMCH1All.Checked = true;
            comboBoxLevel1.Enabled = false;
            comboBoxLevel1.Text = "";

            rbMCH2.Enabled = true;
            rbMCH2All.Enabled = true;
            rbMCH2All.Checked = true;
            comboBoxLevel2.Enabled = false;
            comboBoxLevel2.Text = "";

            rbMCH3.Enabled = true;
            rbMCH3All.Enabled = true;
            rbMCH3All.Checked = true;
            comboBoxLevel3.Enabled = false;
            comboBoxLevel3.Text = "";

            rbMCH4.Enabled = true;
            rbMCH4All.Enabled = true;
            rbMCH4All.Checked = true;
            comboBoxLevel4.Enabled = false;
            comboBoxLevel4.Text = "";
        }

        private void rbMCH1All_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMCH1All.Checked)
            {
                comboBoxLevel1.Enabled = false;
                comboBoxLevel1.Text = "";
                rbMCH2.Enabled = true;
                rbMCH2All.Enabled = true;
                rbMCH2All.Checked = true;
                comboBoxLevel2.Enabled = false;
                comboBoxLevel2.Text = "";

                rbMCH3.Enabled = true;
                rbMCH3All.Enabled = true;
                rbMCH3All.Checked = true;
                comboBoxLevel3.Enabled = false;
                comboBoxLevel3.Text = "";

                rbMCH4.Enabled = true;
                rbMCH4All.Enabled = true;
                rbMCH4All.Checked = true;
                comboBoxLevel4.Enabled = false;
                comboBoxLevel4.Text = "";
            }
        }
        private void rbMCH1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMCH1.Checked)
            {
                comboBoxLevel1.Enabled = true;
                AddDropDownMCHLevel1();

                rbMCH2.Enabled = true;
                rbMCH2All.Enabled = true;
                rbMCH2All.Checked = true;
                comboBoxLevel2.Enabled = false;
                comboBoxLevel2.Text = "";

                rbMCH3.Enabled = true;
                rbMCH3All.Enabled = true;
                rbMCH3All.Checked = true;
                comboBoxLevel3.Enabled = false;
                comboBoxLevel3.Text = "";

                rbMCH4.Enabled = true;
                rbMCH4All.Enabled = true;
                rbMCH4All.Checked = true;
                comboBoxLevel4.Enabled = false;
                comboBoxLevel4.Text = "";
            }
        }

        private void comboBoxLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddDropDownMCHLevel2();

            rbMCH2.Enabled = true;
            rbMCH2All.Enabled = true;
            rbMCH2All.Checked = true;
            comboBoxLevel2.Enabled = false;
            comboBoxLevel2.Text = "";

            rbMCH3.Enabled = true;
            rbMCH3All.Enabled = true;
            rbMCH3All.Checked = true;
            comboBoxLevel3.Enabled = false;
            comboBoxLevel3.Text = "";

            rbMCH4.Enabled = true;
            rbMCH4All.Enabled = true;
            rbMCH4All.Checked = true;
            comboBoxLevel4.Enabled = false;
            comboBoxLevel4.Text = "";
        }

        private void rbMCH2All_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMCH2All.Checked)
            {
                comboBoxLevel2.Enabled = false;
                comboBoxLevel2.Text = "";
                rbMCH3.Enabled = true;
                rbMCH3All.Enabled = true;
                rbMCH3All.Checked = true;
                comboBoxLevel3.Enabled = false;
                comboBoxLevel3.Text = "";

                rbMCH4.Enabled = true;
                rbMCH4All.Enabled = true;
                rbMCH4All.Checked = true;
                comboBoxLevel4.Enabled = false;
                comboBoxLevel4.Text = "";
            }
        }

        private void rbMCH2_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMCH2.Checked)
            {
                comboBoxLevel2.Enabled = true;
                AddDropDownMCHLevel2();

                rbMCH3.Enabled = true;
                rbMCH3All.Enabled = true;
                rbMCH3All.Checked = true;
                comboBoxLevel3.Enabled = false;
                comboBoxLevel3.Text = "";

                rbMCH4.Enabled = true;
                rbMCH4All.Enabled = true;
                rbMCH4All.Checked = true;
                comboBoxLevel4.Enabled = false;
                comboBoxLevel4.Text = "";
            }
        }

        private void comboBoxLevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddDropDownMCHLevel3();
            rbMCH3.Enabled = true;
            rbMCH3All.Enabled = true;
            rbMCH3All.Checked = true;
            comboBoxLevel3.Enabled = false;
            comboBoxLevel3.Text = "";

            rbMCH4.Enabled = true;
            rbMCH4All.Enabled = true;
            rbMCH4All.Checked = true;
            comboBoxLevel4.Enabled = false;
            comboBoxLevel4.Text = "";
        }
        private void rbMCH3All_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMCH3All.Checked)
            {
                comboBoxLevel3.Enabled = false;
                comboBoxLevel3.Text = "";
                rbMCH4.Enabled = true;
                rbMCH4All.Enabled = true;
                rbMCH4All.Checked = true;
                comboBoxLevel4.Enabled = false;
                comboBoxLevel4.Text = "";
            }
        }
        private void rbMCH3_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMCH3.Checked)
            {
                comboBoxLevel3.Enabled = true;
                AddDropDownMCHLevel3();

                rbMCH4.Enabled = true;
                rbMCH4All.Enabled = true;
                rbMCH4All.Checked = true;
                comboBoxLevel4.Enabled = false;
                comboBoxLevel4.Text = "";
            }

        }

        private void comboBoxLevel3_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddDropDownMCHLevel4();
            rbMCH4.Enabled = true;
            rbMCH4All.Enabled = true;
            rbMCH4All.Checked = true;
            comboBoxLevel4.Enabled = false;
            comboBoxLevel4.Text = "";
        }


        private void rbMCH4All_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMCH4All.Checked)
            {
                comboBoxLevel4.Enabled = false;
                comboBoxLevel4.Text = "";
            }
        }
        private void rbMCH4_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMCH4.Checked)
            {
                comboBoxLevel4.Enabled = true;
                AddDropDownMCHLevel4();
            }
        }


        private void rbStorageLocAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbStorageLocAll.Checked)
            {
                comboBoxStorageLocation.Enabled = false;
                comboBoxStorageLocation.Text = "";
            }

        }

        private void rbSectionAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSectionAll.Checked)
            {
                SectionCode.Enabled = false;
                SectionCode.Text = "";
            }
        }
        private void rbSectionCode_CheckedChanged(object sender, EventArgs e)
        {
            SectionCode.Enabled = true;
        }

        private void sectionCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != ',' && !char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void rbStorageLoc_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxStorageLocation.Enabled = true;
            AddDropDownStorageLocation();
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

                Location.Enabled = false;
                Location.Text = "";
            }
        }

        private void rbLocationFromTo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLocationFromTo.Checked)
            {
                locationFrom.Enabled = true;
                locationTo.Enabled = true;

                Location.Enabled = false;
                Location.Text = "";
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

                Location.Enabled = true;
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
                comboBoxBrandCode.Enabled = false;
                comboBoxBrandCode.Text = "";
            }
        }

        private void rbBrand_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxBrandCode.Enabled = true;
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                errorList = "";
            }

            return errorList;
        }

        private DataTable GetReport_SumStockOnHand(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
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


                return bllReportManagement.GetReport_SumStockOnHand(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_SumStockOnHandFreshFood(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                //string allDepartmentCode = reportParam.Plant;
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

                return bllReportManagement.GetReport_SumStockOnHandFreshFood(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_SumStockOnHandWarehouse(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
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


                return bllReportManagement.GetReport_SumStockOnHandWarehouse(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_SumStockOnHandByBrandGroup(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
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

                return bllReportManagement.GetReport_SumStockOnHand(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_SumStockOnHandByBrandGroupFreshFood(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                //string allDepartmentCode = reportParam.Plant;
                //string allBrandCode = reportParam.BrandCode;
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

                return bllReportManagement.GetReport_SumStockOnHandFreshFood(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }
        private DataTable GetReport_SumStockOnHandByBrandGroupWarehouse(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.Plant;
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

                return bllReportManagement.GetReport_SumStockOnHandWarehouse(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_SectionLocationByBrandGroup(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
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


                return bllReportManagement.LoadReport_SectionLocationByBrandGroup(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_StocktakingAuditCheckWithUnit(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
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
                paramFields.Add(paramField);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);

                return bllReportManagement.LoadReport_StocktakingAuditCheckWithUnit(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_StocktakingAuditCheck(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            //Nouse
            try
            {
                string allDepartmentCode = reportParam.Plant;
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

                return bllReportManagement.LoadReport_StocktakingAuditCheck(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
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

        private DataTable GetReport_DeleteRecordReportBySection(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.Plant;
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

                return bllReportManagement.LoadReport_DeleteRecordReportBySection(reportParam.CountDate, reportParam);
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

        private DataTable GetReport_StocktakingAuditAdjust(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.Plant;
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

                return bllReportManagement.LoadReport_StocktakingAudiAdjust(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_ControlSheet(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
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

                return bllReportManagement.GetReport_ControlSheet(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_UncountedLocation(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
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

                return bllReportManagement.GetReport_UncountedLocation(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_UnidentifiedStockItems(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
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

                return bllReportManagement.LoadReport_UnidentifiedStockItem(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                string allDepartmentCode = reportParam.Plant;
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
                DateTime countDate = reportParam.CountDate;
                //DataTable dataAll = bllReportManagement.LoadReport_InventoryControlBySection(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, "DATA", reportParam.Unit);
                DataTable dataAll = bllReportManagement.LoadReport_InventoryControlBySection(countDate, reportParam);

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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                string allDepartmentCode = reportParam.Plant;
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
                DateTime countDate = reportParam.CountDate;
                //DataTable dtDepartmentCode = bllReportManagement.LoadReport_InventoryControlByLocation(reportParam.CountDate, allDepartmentCode, allSectionCode, allLocationCode, allBrandCode, allStoreType, allDifftype, allUnit, string.Empty, reportParam.Unit);
                DataTable dataAll = bllReportManagement.LoadReport_InventoryControlByLocation(countDate, reportParam);
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_InventoryControlDifferenceByBarcodeFrontBack(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDifftype = reportParam.DiffType;
                string allUnit = reportParam.Unit;
                if (reportParam.Unit == "1" || reportParam.Unit == "2")
                {
                    allUnit = "1,2";
                }
                //string allBarcode = reportParam.Barcode;
                //string allSectionCode = reportParam.SectionCode;
                //string allBrandCode = reportParam.BrandCode;

                if (allDifftype.Contains("3"))
                {
                    allDifftype = "1,2,3";
                }


                string allBrandCode = reportParam.BrandCode;
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
                ParameterField paramField2 = new ParameterField();
                ParameterField paramField3 = new ParameterField();
                ParameterField paramField4 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                ParameterDiscreteValue paramDiscreteValue4 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = reportParam.CountDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramFields.Add(paramField1);

                paramField2.Name = "pHeaderReportName";
                var reportName = string.Empty;
                reportName = "All Barcode ( ";

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

                paramDiscreteValue2.Value = reportName + diffType + ")";
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramFields.Add(paramField2);

                paramField3.Name = "pReportName";
                paramDiscreteValue3.Value = ReportName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField3);

                paramField4.Name = "selectedBrand";
                paramDiscreteValue4.Value = allBrandCodeForDisplay;
                paramField4.CurrentValues.Add(paramDiscreteValue4);
                paramFields.Add(paramField4);



                DataTable dataAll = bllReportManagement.LoadReport_InventoryControlByBarcode(reportParam.CountDate, reportParam, allDifftype, allUnit);
                //DataTable dataAll = bllReportManagement.LoadReport_InventoryControlByBarcode(reportParam.CountDate, allDepartmentCode, allSectionCode, allBrandCode, allDifftype, allUnit, allBarcode, "DATA", reportParam.Unit);

                return dataAll;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_InventoryControlDifferenceByBarcodeFreshFood(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDifftype = reportParam.DiffType;

                if (allDifftype.Contains("3"))
                {
                    allDifftype = "1,2,3";
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

                paramField2.Name = "pHeaderReportName";
                var reportName = string.Empty;
                reportName = "All Barcode ( ";

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

                paramDiscreteValue2.Value = reportName + diffType + ")";
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramFields.Add(paramField2);

                paramField3.Name = "pReportName";
                paramDiscreteValue3.Value = ReportName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField3);


                DataTable dataAll = bllReportManagement.LoadReport_InventoryControlByBarcodeFreshFood(reportParam.CountDate, reportParam, allDifftype);

                return dataAll;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataSet GetReport_ItemPhysicalCountBySection(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allDepartmentCode = reportParam.Plant;
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

                return bllReportManagement.GetReport_ItemPhysicalCountBySection(reportParam.CountDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataSet();
            }
        }

        private DataTable GetReport_ItemPhysicalCountByBarcode(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allSectionCode = reportParam.SectionCode;
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
                paramDiscreteValue5.Value = "";
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

                return bllReportManagement.GetReport_ItemPhysicalCountByBarcode(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_GroupSummaryReportByFrontBack(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
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

                return bllReportManagement.loadReport_GroupSummaryReportByFrontBack(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_GroupSummaryReportByFreshFoodWarehouse(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
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

                return bllReportManagement.loadReport_GroupSummaryReportByFreshFoodWarehouse(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_CountedLocationsReport(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {

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

                return bllReportManagement.LoadReport_CountedLocationsReport(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_NoticeOfStocktakingSatisfactionByFrontBack(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allBrandCode = reportParam.BrandCode;
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

                return bllReportManagement.LoadReport_NoticeOfStocktakingSatisfactionByFrontBack(countDate, reportParam);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }

        private DataTable GetReport_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse(ReportParameter reportParam, ParameterFields paramFields, string ReportName)
        {
            try
            {
                string allBrandCode = reportParam.BrandCode;
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

                //result = bllReportManagement.LoadReport_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse(countDate, reportParam);
                return bllReportManagement.LoadReport_NoticeOfStocktakingSatisfactionByFreshFoodWarehouse(countDate, reportParam);

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return new DataTable();
            }
        }
        protected void AddDropDownPlant()
        {

                try
                {
                    //Get DropDown
                    List<string> listPlant = new List<string>();
                    listPlant = bllSystemSetting.GetAllPlant();

                    comboBoxPlant.Items.Clear();
                    comboBoxPlant.Items.Add("");
                    if (listPlant.Count > 0)
                    {
                        foreach (var l in listPlant)
                        {
                            comboBoxPlant.Items.Add(l);
                        }
                        comboBoxPlant.SelectedIndex = 0;
                        //AddDropDownCountSheet();
                    }

                }
                catch (Exception ex)
                {
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                }
            
        }
        protected void AddDropDownCountSheet()
        {
            try
            {
                //Get DropDown
                List<string> listCountSheet = new List<string>();
                String plant = comboBoxPlant.Text;
                if (string.IsNullOrEmpty(plant))
                {
                    plant = "All";
                }
                listCountSheet = bllSystemSetting.GetDropDownCountSheetSKU(plant);

                comboBoxCountSheet.Items.Clear();
                comboBoxCountSheet.Items.Add("");
                if (listCountSheet.Count > 0)
                {
                    foreach (var l in listCountSheet)
                    {
                        comboBoxCountSheet.Items.Add(l);
                    }
                    comboBoxCountSheet.SelectedIndex = 0;
                }

                //AddDropDownMCHLevel1();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }


        protected void AddDropDownMCHLevel1()
        {
            try
            {
                //Get DropDown

                List<string> listMCH = new List<string>();
                String countSheet = comboBoxCountSheet.Text;
                comboBoxLevel1.Items.Clear();
                comboBoxLevel1.Items.Add("");
                if (string.IsNullOrEmpty(countSheet))
                {
                    countSheet = "All";
                }
                    listMCH = bllSystemSetting.GetDropDownMCH1(countSheet);
                    if (listMCH.Count > 0)
                    {
                        foreach (var m in listMCH)
                        {
                            comboBoxLevel1.Items.Add(m);
                        }

                        comboBoxLevel1.SelectedIndex = 0;
                    }
                
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
                //Get DropDown

                List<string> listMCH = new List<string>();
                String countSheet = comboBoxCountSheet.Text;
                String hLevel1 = comboBoxLevel1.Text;
                comboBoxLevel2.Items.Clear();
                comboBoxLevel2.Items.Add("");
                if (string.IsNullOrEmpty(countSheet))
                {
                    countSheet = "All";
                }
                if (string.IsNullOrEmpty(hLevel1))
                {
                    hLevel1 = "All";
                }

                    listMCH = bllSystemSetting.GetDropDownMCH2(countSheet, hLevel1);
                    if (listMCH.Count > 0)
                    {
                        foreach (var m in listMCH)
                        {
                            comboBoxLevel2.Items.Add(m);
                        }
                        comboBoxLevel2.SelectedIndex = 0;
                    }               
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
                //string MCHLevel3 = bllSystemSetting.GetSettingStringByKey("MCHLevel3");
                //lbMCHLevel3.Text = MCHLevel3;

                //Get DropDown

                List<string> listMCH = new List<string>();
                String countSheet = comboBoxCountSheet.Text;
                String hLevel1 = comboBoxLevel1.Text;
                String hLevel2 = comboBoxLevel2.Text;
                comboBoxLevel3.Items.Clear();
                comboBoxLevel3.Items.Add("");
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
                if (!string.IsNullOrEmpty(countSheet) || !string.IsNullOrEmpty(hLevel1) || !string.IsNullOrEmpty(hLevel2))
                {
                    listMCH = bllSystemSetting.GetDropDownMCH3(countSheet, hLevel1, hLevel2);
                    if (listMCH.Count > 0)
                    {
                        foreach (var m in listMCH)
                        {
                            comboBoxLevel3.Items.Add(m);
                        }
                        comboBoxLevel3.SelectedIndex = 0;
                    }
                }
                
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


                //Get DropDown
                List<string> listMCH = new List<string>();
                String countSheet = comboBoxCountSheet.Text;
                String hLevel1 = comboBoxLevel1.Text;
                String hLevel2 = comboBoxLevel2.Text;
                String hLevel3 = comboBoxLevel3.Text;
                comboBoxLevel4.Items.Clear();
                comboBoxLevel4.Items.Add("");
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
                if (!string.IsNullOrEmpty(countSheet) || !string.IsNullOrEmpty(hLevel1)
                    || !string.IsNullOrEmpty(hLevel2) || !string.IsNullOrEmpty(hLevel3))
                {

                    listMCH = bllSystemSetting.GetDropDownMCH4(countSheet, hLevel1, hLevel2, hLevel3);
                    if (listMCH.Count > 0)
                    {
                        foreach (var m in listMCH)
                        {
                            comboBoxLevel4.Items.Add(m);
                        }
                        comboBoxLevel4.SelectedIndex = 0;
                    }
                }
                
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
                listType = bllLocal.GetListMasterStorageLocation();
                comboBoxStorageLocation.Items.Clear();
                comboBoxStorageLocation.Items.Add("");
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

        protected void AddDropDownBrand()
        {
            try
            {
                List<ReportMasterBrand> brandList = new List<ReportMasterBrand>();
                brandList = bllReportManagement.GetBrandList();
                comboBoxBrandCode.Items.Clear();
                comboBoxBrandCode.Items.Add("");

                foreach (ReportMasterBrand m in brandList)
                {
                    ComboboxItem item = new ComboboxItem();
                    comboBoxBrandCode.Items.Add(m);
                }


                //comboBoxBrandCode.DataSource = brandList;
                comboBoxBrandCode.ValueMember = "BrandCode";
                comboBoxBrandCode.DisplayMember = "BrandName";
                comboBoxBrandCode.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        //private void cdNoCorrection_CheckedChanged(object sender, EventArgs e)
        //{
        //    //if (cdNoCorrection.Checked)
        //    //{
        //    //    foreach (CheckBox chk in gbCorrectionDel.Controls)
        //    //    {
        //    //        chk.Checked = false;
        //    //    }
        //    //}
        //    //else 
        //    //{
        //    //    foreach (CheckBox chk in gbCorrectionDel.Controls)
        //    //    {
        //    //        chk.Checked = false;
        //    //    }
        //    //}
        //}

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            
            CheckBox chkBox = (CheckBox)sender;
            if (cdNoCorrection.Checked && chkBox.Text == "All")
            {
                cdAdd.Checked = false;
                cdCorrection.Checked = false;
                cdDelete.Checked = false;
            }
            else if (cdAdd.Checked && chkBox.Text == "Add")
            {
                cdNoCorrection.Checked = false;
            }
            else if (cdCorrection.Checked && chkBox.Text == "Correction")
            {
                cdNoCorrection.Checked = false;
            }
            else if (cdDelete.Checked && chkBox.Text == "Delete")
            {
                cdNoCorrection.Checked = false;
            }
        }

        protected void chk_DiffTypeCheckedChanged(object sender, EventArgs e)
        {

            CheckBox chkBox = (CheckBox)sender;

            if (dtNoDiff.Checked && chkBox.Text == "All")
            {
                dtShortage.Checked = false;
                dtOver.Checked = false;
            }
            else if (dtShortage.Checked && chkBox.Text == "Shortage")
            {
                dtNoDiff.Checked = false;
            }
            else if (dtOver.Checked && chkBox.Text == "Over")
            {
                dtNoDiff.Checked = false;
            }

        }

    }
}

