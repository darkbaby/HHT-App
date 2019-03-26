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
using FSBT_HHT_DAL;
using System.Reflection;

namespace FSBT.HHT.App.UI
{
    public partial class BarcodePrintForm : Form
    {
        private LogErrorBll logBll = new LogErrorBll(); 
        ReportManagementBll barcodeBLL = new ReportManagementBll();
        List<LocationBarcode> displayData = new List<LocationBarcode>();
        SystemSettingBll settingBLL = new SystemSettingBll();
        private string Plant = "";
        private LocationManagementBll bll = new LocationManagementBll();

        public BarcodePrintForm()
        {
            InitializeComponent();
            AddDropDownPlant();
            AddDropDownCountSheet();
            AddDropDownStorageLocation();

            //AddDropDownMCHLevel();
            //AddDropDownCountSheet();
        }
        private void BarcodePrintForm_Load(object sender, EventArgs e)
        {
            SystemSettingModel displayData = new SystemSettingModel();
            displayData = settingBLL.GetSettingData();
            Plant = settingBLL.GetSettingStringByKey("Plant");
            //lbPlant.Text = "Plant : " + Plant;
            string[] sectionType = displayData.SectionType.Split('|');
            foreach (string s in sectionType)
            {
                //if (s.Equals("1")) { chFront.Checked = true; }
                //if (s.Equals("2")) { chBack.Checked = true; }
                //if (s.Equals("3")) { chWarehouse.Checked = true; }
                //if (s.Equals("4")) { chFreshFood.Checked = true; }
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

                if (resultValidate)
                {
                    displayData = barcodeBLL.GetSearchSection(searchSection);
                    //displayData = barcodeBLL.GetSection(searchSection);
                    
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }
        private void printButton_Click(object sender, EventArgs e)
        {
            try
            {
                Loading_Screen.ShowSplashScreen();
                List<LocationBarcode> barcodeData = barcodeBLL.GenDataToBarCode(displayData);

                //List<LocationModel> locationData = barcodeBLL.GetSectionLocationBarcode(searchList);

                //List<LocationBarcode> barcodeData = barcodeBLL.GenDataToBarCode(locationData);


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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }
        private LocationManagementModel GetSearchCondition()
        {
            #region
            LocationManagementModel searchSection = new LocationManagementModel();
            try
            {
                string storageLoc = "";

                if (comboBoxStorageLocal.Text.ToUpper().Equals("ALL"))
                {
                    storageLoc = comboBoxStorageLocal.Text;
                }
                else
                {
                    storageLoc = (string)((FSBT_HHT_Model.ComboboxItem)comboBoxStorageLocal.SelectedItem).Value;
                }

                //locationfrom
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

                //locationto
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

            #endregion

                searchSection.PlantCode = comboBoxPlant.Text;
                searchSection.CountSheet = comboBoxCountSheet.Text;

                searchSection.StorageLocationCode = storageLoc;

                searchSection.LocationFrom = textBoxLoForm.Text;
                searchSection.LocationTo = textBoxLoTo.Text;

            }
            catch(Exception ex)
            {

            }
            return searchSection;
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

        protected void AddDropDownStorageLocation()
        {
            try
            {
                List<MasterStorageLocation> listType = new List<MasterStorageLocation>();

                listType = bll.GetListMasterStorageLocation();
                comboBoxStorageLocal.Items.Clear();
                comboBoxStorageLocal.Items.Add("All");
                foreach (MasterStorageLocation m in listType)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = m.StorageLocationCode + " - " + m.StorageLocationName;
                    item.Value = m.StorageLocationCode;

                    comboBoxStorageLocal.Items.Add(item);
                }
                comboBoxStorageLocal.SelectedIndex = 0;
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

        private void comboBoxPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddDropDownCountSheet();
            AddDropDownStorageLocation();
        }
    }
}
