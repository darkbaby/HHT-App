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
using FSBT_HHT_Model;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web;
using System.IO;
using FSBT.HHT.App.Resources;

namespace FSBT.HHT.App.UI
{
    public partial class BarcodePrintForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        ReportManagementBll barcodeBLL = new ReportManagementBll();
        List<LocationBarcode> displayData = new List<LocationBarcode>();
        SystemSettingBll settingBLL = new SystemSettingBll();

        public BarcodePrintForm()
        {
            InitializeComponent();
        }
        private void BarcodePrintForm_Load(object sender, EventArgs e)
        {
            SystemSettingModel displayData = new SystemSettingModel();
            displayData = settingBLL.GetSettingData();
            string[] sectionType = displayData.SectionType.Split('|');
            foreach (string s in sectionType)
            {
                if (s.Equals("1")) { chFront.Checked = true; }
                if (s.Equals("2")) { chBack.Checked = true; }
                if (s.Equals("3")) { chWarehouse.Checked = true; }
                if (s.Equals("4")) { chFreshFood.Checked = true; }
            }
        }
        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.resultGridView.Rows.Clear();
                LocationManagementModel searchSection = GetSearchCondition();
                int x;
                bool resultValidate = true;
                if (!(string.IsNullOrWhiteSpace(searchSection.LocationFrom.Trim())))
                {
                    if (!(Int32.TryParse(searchSection.LocationFrom.Trim(), out x)))
                    {
                        resultValidate = false;
                        MessageBox.Show(MessageConstants.LocationFrommustbenumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                if (!(string.IsNullOrWhiteSpace(searchSection.LocationTo.Trim())))
                {
                    if (!(Int32.TryParse(searchSection.LocationTo.Trim(), out x)))
                    {
                        resultValidate = false;
                        MessageBox.Show(MessageConstants.LocationTomustbenumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                if (!(string.IsNullOrWhiteSpace(searchSection.SectionCode.Trim())))
                {
                    if (!(Int32.TryParse(searchSection.SectionCode.Trim(), out x)))
                    {
                        resultValidate = false;
                        MessageBox.Show(MessageConstants.Sectioncodemustbenumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                if (!(string.IsNullOrWhiteSpace(searchSection.DepartmentCode.Trim())))
                {
                    if (!(Int32.TryParse(searchSection.DepartmentCode.Trim(), out x)))
                    {
                        resultValidate = false;
                        MessageBox.Show(MessageConstants.Departmentisnumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                if (resultValidate)
                {
                    displayData = barcodeBLL.GetSearchSection(searchSection);
                    if (displayData == null)
                    {
                        MessageBox.Show(MessageConstants.Nosectiondatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        foreach (LocationBarcode result in displayData)
                        {
                            this.resultGridView.Rows.Add(result.LocationCode, result.SectionCode, result.SectionName);
                        }

                        if (this.resultGridView.Rows.Count <= 0)
                        {
                            MessageBox.Show(MessageConstants.Nosectiondatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }
        private void printButton_Click(object sender, EventArgs e)
        {
            try
            {
                Loading_Screen.ShowSplashScreen();
                List<LocationBarcode> barcodeData = barcodeBLL.GenDataToBarCode(displayData);
                BarCodeReportForm barcodeReport = new BarCodeReportForm();
                bool isCreateReportSuccess = barcodeReport.CreateReport(barcodeData);
                Loading_Screen.CloseForm();
                if (isCreateReportSuccess)
                {
                    barcodeReport.StartPosition = FormStartPosition.CenterParent;
                    DialogResult dialogResult = barcodeReport.ShowDialog();
                }
                else
                {
                    MessageBox.Show(MessageConstants.cannotgeneratereport, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }
        private LocationManagementModel GetSearchCondition()
        {
            //sectioncode
            if (textBoxSecCode.Text.Trim().Length == 1)
            {
                textBoxSecCode.Text = "0000" + textBoxSecCode.Text.Trim();
            }
            else if (textBoxSecCode.Text.Trim().Length == 2)
            {
                textBoxSecCode.Text = "000" + textBoxSecCode.Text.Trim();
            }
            else if (textBoxSecCode.Text.Trim().Length == 3)
            {
                textBoxSecCode.Text = "00" + textBoxSecCode.Text.Trim();
            }
            else if (textBoxSecCode.Text.Trim().Length == 4)
            {
                textBoxSecCode.Text = "0" + textBoxSecCode.Text.Trim();
            }
            ////locationfrom
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



            LocationManagementModel searchSection = new LocationManagementModel();
            searchSection.SectionCode = textBoxSecCode.Text;
            searchSection.SectionName = textBoxSecName.Text;
            searchSection.LocationFrom = textBoxLoFrom.Text;
            searchSection.LocationTo = textBoxLoTo.Text;
            searchSection.DepartmentCode = textBoxDeptCode.Text;





            string sectionTypeList = "";
            foreach (CheckBox sectionType in groupSectionType.Controls)
            {
                if (sectionType.Checked == true)
                {
                    sectionTypeList = sectionTypeList + (sectionTypeList == "" ? "" : "|") + (string)sectionType.Text;
                }
            }

            searchSection.SectionType = sectionTypeList;

            return searchSection;
        }
    }
}
