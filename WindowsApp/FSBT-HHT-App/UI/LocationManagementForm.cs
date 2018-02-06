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
using System.Net;
using System.IO;
using System.Reflection;
using FSBT.HHT.App.Resources;
using System.Collections;

namespace FSBT.HHT.App.UI
{
    public partial class LocationManagementForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        public string UserName { get; set; }
        private SystemSettingBll settingBLL = new SystemSettingBll();
        private LocationManagementBll bll = new LocationManagementBll();
        ReportManagementBll barcodeBLL = new ReportManagementBll();
        private List<LocationManagementModel> originalList = new List<LocationManagementModel>();
        private List<FSBT_HHT_Model.Location> deleteList = new List<FSBT_HHT_Model.Location>();
        private List<LocationManagementModel> updateList = new List<LocationManagementModel>();
        private List<LocationManagementModel> insertList = new List<LocationManagementModel>();
        private List<LocationManagementModel> gridViewDataList = new List<LocationManagementModel>();
        private List<FSBT_HHT_Model.Location> allLocationList = new List<FSBT_HHT_Model.Location>();
        private string valueBeforeEdit;
        private List<LocationManagementModel> gridViewDataBeforeEdit = new List<LocationManagementModel>();
        List<LocationBarcode> displayData = new List<LocationBarcode>();
        private const int deptCodeCol = 0;
        private const int sectionTypeCol = 1;
        private const int originalSecCol = 2;
        private const int sectionCodeCol = 3;
        private const int sectionNameCol = 4;
        private const int locationFromCol = 5;
        private const int locationToCol = 6;
        private const int brandCol = 7;
        private const int flagCol = 8;
        private const int originalSecTypeCol = 9;
        private String[] headerImportSection = { "DepartmentCode", "SectionType", "SectionCode", "SectionName", "LocationFrom", "LocationTo" };

        public LocationManagementForm()
        {
            InitializeComponent();
            AddDropDownSectionType();
        }

        public LocationManagementForm(string username)
        {
            try
            {
                InitializeComponent();
                AddDropDownSectionType();
                AddSettingSectionType();
                UserName = username;
                originalList = bll.GetAllSection();
                allLocationList.Clear();
                allLocationList = bll.GetAllLocation();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        protected void AddDropDownSectionType()
        {
            try
            {
                List<MasterScanMode> listType = new List<MasterScanMode>();

                listType = bll.GetListSectionType();

                foreach (MasterScanMode m in listType)
                {
                    SectionType.Items.Add(m.ScanModeName);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void AddSettingSectionType()
        {
            SystemSettingModel displayData = new SystemSettingModel();
            displayData = settingBLL.GetSettingData();
            string[] sectionType = displayData.SectionType.Split('|');
            foreach (string s in sectionType)
            {
                if (s.Equals("1")) { chLocationType1.Checked = true; }
                if (s.Equals("2")) { chLocationType2.Checked = true; }
                if (s.Equals("3")) { chLocationType3.Checked = true; }
                if (s.Equals("4")) { chLocationType4.Checked = true; }
            }
        }

        private void LocationManagementForm_Load(object sender, EventArgs e)
        {
            try
            {
                //SectionType.DefaultCellStyle.NullValue = SectionType.Items[0];            
                dataGridViewResult.Rows[0].Cells[sectionTypeCol].Value = SectionType.Items[0];
                //List<LocationManagementModel> brandList = bll.SearchBrand();

                //foreach (LocationManagementModel brand in brandList)
                //{
                //    //ComboboxBrandItem item = new ComboboxBrandItem();
                //    //item.Text = brand.BrandCode + " - " + brand.BrandName;
                //    //item.BrandCode = brand.BrandCode;
                //    //item.BrandName = brand.BrandName;
                //    //BrandType.Items.Add(item);
                //    //BrandType.Items.Add(brand.BrandCode + "-" + brand.BrandName); //brandtypedeleted

                //}
                //BrandType.DefaultCellStyle.NullValue = BrandType.Items[0];
                //var brandtype = BrandType.Items[0];
                //string text = brandtype.ToString();
                //dataGridViewResult.Rows[0].Cells[brandCol].Value = BrandType.Items[0];//brandtypedeleted
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        // get current data in gridview
        private List<LocationManagementModel> GetCurrentDataGridView()
        {
            List<LocationManagementModel> gridViewDataList = new List<LocationManagementModel>();
            try
            {

                for (int count = 0; count < dataGridViewResult.Rows.Count - 1; count++)
                {
                    LocationManagementModel gridViewData = new LocationManagementModel();
                    gridViewData.DepartmentCode = (string)dataGridViewResult.Rows[count].Cells[deptCodeCol].Value;
                    gridViewData.OriginSectionCode = (string)dataGridViewResult.Rows[count].Cells[originalSecCol].Value;
                    gridViewData.SectionCode = (string)dataGridViewResult.Rows[count].Cells[sectionCodeCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[sectionCodeCol].Value).Trim();
                    gridViewData.SectionType = (string)dataGridViewResult.Rows[count].Cells[sectionTypeCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[sectionTypeCol].Value).Trim();
                    gridViewData.SectionName = (string)dataGridViewResult.Rows[count].Cells[sectionNameCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[sectionNameCol].Value).Trim();
                    gridViewData.LocationFrom = (string)dataGridViewResult.Rows[count].Cells[locationFromCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[locationFromCol].Value).Trim();
                    gridViewData.LocationTo = (string)dataGridViewResult.Rows[count].Cells[locationToCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[locationToCol].Value).Trim();
                    gridViewData.BrandCode = (string)dataGridViewResult.Rows[count].Cells[brandCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[brandCol].Value).Trim();
                    gridViewData.FlagAction = (string)dataGridViewResult.Rows[count].Cells[flagCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[flagCol].Value).Trim();
                    gridViewData.OriginSectionType = (string)dataGridViewResult.Rows[count].Cells[originalSecTypeCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[originalSecTypeCol].Value).Trim();
                    gridViewDataList.Add(gridViewData);

                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                gridViewDataList = new List<LocationManagementModel>();
            }

            return gridViewDataList;
        }

        //prepare data in gridview to push in gridViewDataList for generate insertList updateList and deleteList
        private void GetGridViewSectionList()
        {
            try
            {
                gridViewDataList.Clear();

                for (int count = 0; count < dataGridViewResult.Rows.Count - 1; count++)
                {
                    LocationManagementModel gridViewData = new LocationManagementModel();
                    string originalSectionCode = (string)dataGridViewResult.Rows[count].Cells[originalSecCol].Value;
                    string sectionCode = (string)dataGridViewResult.Rows[count].Cells[sectionCodeCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[sectionCodeCol].Value).Trim();
                    string sectionType = (string)dataGridViewResult.Rows[count].Cells[sectionTypeCol].Value;
                    string originalSectionType = (string)dataGridViewResult.Rows[count].Cells[originalSecTypeCol].Value;
                    //case insert new row set originalsectioncode equal sectioncode
                    if (originalSectionCode == null)
                    {
                        gridViewData.OriginSectionCode = sectionCode;
                        dataGridViewResult.Rows[count].Cells[originalSecCol].Value = sectionCode;
                    }
                    else
                    {
                        // case edit section code
                        if (originalSectionCode != sectionCode)
                        {
                            FSBT_HHT_Model.Location deleteSection = new FSBT_HHT_Model.Location
                            {
                                SectionCode = originalSectionCode,
                                SectionType = originalSectionType
                            };

                            deleteList.Add(deleteSection);

                            gridViewData.OriginSectionCode = sectionCode;
                            dataGridViewResult.Rows[count].Cells[originalSecCol].Value = sectionCode;
                        }
                        else
                        {
                            gridViewData.OriginSectionCode = originalSectionCode;
                        }
                    }

                    if (originalSectionType == null)
                    {
                        gridViewData.OriginSectionType = sectionType;
                        dataGridViewResult.Rows[count].Cells[originalSecTypeCol].Value = sectionType;
                    }
                    else
                    {
                        // case edit section code
                        if (originalSectionType != sectionType)
                        {
                            FSBT_HHT_Model.Location deleteSection = new FSBT_HHT_Model.Location
                            {
                                SectionCode = originalSectionCode,
                                SectionType = originalSectionType
                            };

                            deleteList.Add(deleteSection);

                            gridViewData.OriginSectionType = sectionType;
                            dataGridViewResult.Rows[count].Cells[originalSecTypeCol].Value = sectionType;
                        }
                        else
                        {
                            gridViewData.OriginSectionType = originalSectionType;
                        }
                    }
                    gridViewData.DepartmentCode = (string)dataGridViewResult.Rows[count].Cells[deptCodeCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[deptCodeCol].Value).Trim();
                    gridViewData.SectionCode = sectionCode;
                    gridViewData.SectionType = (string)dataGridViewResult.Rows[count].Cells[sectionTypeCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[sectionTypeCol].Value).Trim();
                    gridViewData.SectionName = (string)dataGridViewResult.Rows[count].Cells[sectionNameCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[sectionNameCol].Value).Trim();
                    gridViewData.LocationFrom = (string)dataGridViewResult.Rows[count].Cells[locationFromCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[locationFromCol].Value).Trim();
                    gridViewData.LocationTo = (string)dataGridViewResult.Rows[count].Cells[locationToCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[locationToCol].Value).Trim();
                    gridViewData.BrandCode = (string)dataGridViewResult.Rows[count].Cells[brandCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[brandCol].Value).Trim();
                    gridViewData.FlagAction = (string)dataGridViewResult.Rows[count].Cells[flagCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[flagCol].Value).Trim();
                    gridViewDataList.Add(gridViewData);

                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        // Generate insertList and updateList
        private void GenSaveList()
        {
            try
            {
                gridViewDataList.Clear();
                GetGridViewSectionList();

                updateList = (from o in originalList
                              join g in gridViewDataList
                              on new { OriginSectionCode = o.OriginSectionCode, OriginSectionType = o.OriginSectionType }
                              equals new { OriginSectionCode = g.OriginSectionCode, OriginSectionType = g.OriginSectionType }
                              where o.DepartmentCode != g.DepartmentCode || (o.SectionType != g.SectionType && o.SectionCode != g.SectionCode) || o.SectionName != g.SectionName
                              || o.LocationFrom != g.LocationFrom || o.LocationTo != g.LocationTo || o.BrandCode != g.BrandCode
                              select new LocationManagementModel
                              {
                                  DepartmentCode = g.DepartmentCode,
                                  OriginSectionCode = g.OriginSectionCode,
                                  SectionCode = g.SectionCode,
                                  SectionType = g.SectionType,
                                  SectionName = g.SectionName,
                                  LocationFrom = g.LocationFrom,
                                  LocationTo = g.LocationTo,
                                  BrandCode = g.BrandCode,
                                  OriginSectionType = g.OriginSectionType
                              }).ToList();

                List<LocationManagementModel> originalSectionCodeList = (from o in originalList
                                                                         select new LocationManagementModel
                                                                         {
                                                                             OriginSectionCode = o.OriginSectionCode,
                                                                             OriginSectionType = o.OriginSectionType
                                                                         }).ToList();

                insertList = (from g in gridViewDataList
                              where !originalSectionCodeList.Any(t2 => g.OriginSectionCode == t2.OriginSectionCode && g.OriginSectionType == t2.OriginSectionType)
                              select new LocationManagementModel
                              {
                                  DepartmentCode = g.DepartmentCode,
                                  OriginSectionCode = g.OriginSectionCode,
                                  SectionCode = g.SectionCode,
                                  SectionType = g.SectionType,
                                  SectionName = g.SectionName,
                                  LocationFrom = g.LocationFrom,
                                  LocationTo = g.LocationTo,
                                  BrandCode = g.BrandCode,
                                  OriginSectionType = g.OriginSectionType
                              }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        //generate deleteList when user delete gridview's row
        private void dataGridViewResult_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                string originalSectionCode = (string)dataGridViewResult.Rows[e.Row.Index].Cells[originalSecCol].Value;
                string sectionType = (string)dataGridViewResult.Rows[e.Row.Index].Cells[sectionTypeCol].Value;
                if (originalSectionCode != null)
                {
                    FSBT_HHT_Model.Location deleteSection = new FSBT_HHT_Model.Location
                    {
                        SectionCode = originalSectionCode,
                        SectionType = sectionType
                    };

                    deleteList.Add(deleteSection);
                }
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
                if (ValidateSearch())
                {
                    // generate insertList updateList and deleteList
                    GenSaveList();

                    // Check that user have to save before searching again.
                    if (deleteList.Count > 0 || insertList.Count > 0 || updateList.Count > 0)
                    {
                        DialogResult dialogResult = MessageBox.Show(MessageConstants.Youhavenotsavecurrentdatasavenow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Loading_Screen.ShowSplashScreen();
                            if (ValidateEmptyGridview(gridViewDataList))
                            {
                                if (SaveCondition())
                                {
                                    DoSearchData();
                                }
                            }
                            Loading_Screen.CloseForm();

                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            Loading_Screen.ShowSplashScreen();
                            DoSearchData();
                            deleteList.Clear();
                            updateList.Clear();
                            insertList.Clear();
                            Loading_Screen.CloseForm();
                        }
                    }
                    else
                    {
                        Loading_Screen.ShowSplashScreen();
                        DoSearchData();
                        Loading_Screen.CloseForm();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private bool ValidateSearch()
        {
            try
            {
                string sectionCode = textBoxSecCode.Text.Trim();
                string sectionName = textBoxSecName.Text.Trim();
                string locationForm = textBoxLoForm.Text.Trim();
                string locationTo = textBoxLoTo.Text.Trim();
                string deptCode = textBoxDept.Text.Trim();

                int num;
                if (sectionCode != string.Empty)
                {
                    bool res1 = int.TryParse(sectionCode, out num);
                    if (res1 == false)
                    {
                        textBoxSecCode.Text = "";
                        MessageBox.Show(MessageConstants.SectionCodeisnumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    if (sectionCode.Length == 1)
                    {
                        textBoxSecCode.Text = "0000" + sectionCode;
                    }
                    else if (sectionCode.Length == 2)
                    {
                        textBoxSecCode.Text = "000" + sectionCode;
                    }
                    else if (sectionCode.Length == 3)
                    {
                        textBoxSecCode.Text = "00" + sectionCode;
                    }
                    else if (sectionCode.Length == 4)
                    {
                        textBoxSecCode.Text = "0" + sectionCode;
                    }
                }

                if (locationForm != string.Empty)
                {
                    bool res2 = int.TryParse(locationForm, out num);
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
                    bool res3 = int.TryParse(locationTo, out num);
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

                if (deptCode != string.Empty)
                {
                    bool res4 = int.TryParse(deptCode, out num);
                    if (res4 == false)
                    {
                        textBoxDept.Text = "";
                        MessageBox.Show(MessageConstants.Departmentisnumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return true;
            }
        }

        // searching and showing section data to gridview
        private void DoSearchData()
        {
            ////Loading_Screen.ShowSplashScreen();

            try
            {
                //var checkedButton = groupBox1.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                //string sectionType = (string)checkedButton.Tag;
                string sectionTypeList = "";
                foreach (CheckBox sectionType in groupLocationType.Controls)
                {
                    if (sectionType.Checked == true)
                    {
                        sectionTypeList = sectionTypeList + (sectionTypeList == "" ? "" : "|") + (string)sectionType.Text;
                    }
                }

                string sectionCode = textBoxSecCode.Text.Trim();
                string sectionName = textBoxSecName.Text.Trim();
                string locationForm = textBoxLoForm.Text.Trim();
                string locationTo = textBoxLoTo.Text.Trim();
                string deptCode = textBoxDept.Text.Trim();
                textBoxFilter.Clear();
                List<LocationManagementModel> listSearch = new List<LocationManagementModel>();
                listSearch = bll.SearchSection(sectionCode, sectionName, locationForm, locationTo, sectionTypeList, deptCode);
                ////Loading_Screen.CloseForm();
                BindDataToGridView(listSearch);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));

                ////Loading_Screen.CloseForm();
            }
        }

        private void BindDataToGridView(List<LocationManagementModel> listSearch)
        {
            try
            {
                dataGridViewResult.Rows.Clear();
                allLocationList.Clear();
                allLocationList = bll.GetAllLocation();
                if (listSearch.Count == 0)
                {
                    ////Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.Nosectiondatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    foreach (LocationManagementModel section in listSearch)
                    {
                        DataGridViewRow row = (DataGridViewRow)dataGridViewResult.Rows[0].Clone();
                        row.Cells[deptCodeCol].Value = (string)section.DepartmentCode;
                        row.Cells[originalSecCol].Value = (string)section.OriginSectionCode;
                        row.Cells[sectionCodeCol].Value = (string)section.SectionCode;
                        row.Cells[sectionTypeCol].Value = (string)section.SectionType;
                        row.Cells[sectionNameCol].Value = (string)section.SectionName;
                        row.Cells[locationFromCol].Value = (string)section.LocationFrom;
                        row.Cells[locationToCol].Value = (string)section.LocationTo;
                        row.Cells[brandCol].Value = (string)section.BrandCode;
                        row.Cells[originalSecTypeCol].Value = (string)section.OriginSectionType;
                        dataGridViewResult.Rows.Add(row);
                    }
                    dataGridViewResult.Rows[listSearch.Count].Cells[sectionTypeCol].Value = SectionType.Items[0];
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void textBoxFilter_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    GenSaveList();
                    if (deleteList.Count > 0 || insertList.Count > 0 || updateList.Count > 0)
                    {
                        DialogResult dialogResult = MessageBox.Show(MessageConstants.Youhavenotsavecurrentdatasavenow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (ValidateEmptyGridview(gridViewDataList))
                            {
                                if (SaveCondition())
                                {
                                    ShowFilterData();
                                }
                            }
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            ShowFilterData();
                            deleteList.Clear();
                            updateList.Clear();
                            insertList.Clear();
                        }
                    }
                    else
                    {
                        ShowFilterData();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        // show filter result to gridview
        private void ShowFilterData()
        {
            try
            {
                string textFilter = textBoxFilter.Text;
                DoSearchData();
                GetGridViewSectionList();
                dataGridViewResult.Rows.Clear();
                foreach (LocationManagementModel section in gridViewDataList)
                {
                    string sectionName = section.SectionName;
                    if (sectionName.ToLowerInvariant().Contains(textFilter.ToLowerInvariant()))
                    {
                        DataGridViewRow row = (DataGridViewRow)dataGridViewResult.Rows[0].Clone();
                        row.Cells[deptCodeCol].Value = (string)section.DepartmentCode;
                        row.Cells[originalSecCol].Value = (string)section.OriginSectionCode;
                        row.Cells[sectionCodeCol].Value = (string)section.SectionCode;
                        row.Cells[sectionTypeCol].Value = (string)section.SectionType;
                        row.Cells[sectionNameCol].Value = (string)section.SectionName;
                        row.Cells[locationFromCol].Value = (string)section.LocationFrom;
                        row.Cells[locationToCol].Value = (string)section.LocationTo;
                        row.Cells[brandCol].Value = (string)section.BrandCode;
                        row.Cells[originalSecTypeCol].Value = (string)section.OriginSectionType;

                        dataGridViewResult.Rows.Add(row);
                    }
                }
                dataGridViewResult.Rows[dataGridViewResult.Rows.Count - 1].Cells[sectionTypeCol].Value = SectionType.Items[0];
                //dataGridViewResult.Rows[dataGridViewResult.Rows.Count - 1].Cells[brandCol].Value = BrandType.Items[0]; //brandtypedeleted
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewResult.Rows.Count <= 1)
                {
                    MessageBox.Show(MessageConstants.Nosectiondatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show(MessageConstants.Doyouwanttodeleteallsectionbelow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ////Loading_Screen.ShowSplashScreen();

                        List<FSBT_HHT_Model.Location> deletedsectionCodeList = new List<FSBT_HHT_Model.Location>();

                        for (int count = 0; count < dataGridViewResult.Rows.Count - 1; count++)
                        {
                            string originalSectionCode = (string)dataGridViewResult.Rows[count].Cells[originalSecCol].Value;
                            string originalSectionType = (string)dataGridViewResult.Rows[count].Cells[originalSecTypeCol].Value;
                            if (bll.IsSectionCodeExist(originalSectionCode, originalSectionType))
                            {
                                FSBT_HHT_Model.Location deleteSection = new FSBT_HHT_Model.Location
                                {
                                    SectionCode = originalSectionCode,
                                    SectionType = originalSectionType
                                };

                                deletedsectionCodeList.Add(deleteSection);
                            }
                        }

                        bool sectionDeleted = bll.ClearSection(deletedsectionCodeList); // delete section data in database
                        if (sectionDeleted)
                        {
                            dataGridViewResult.Rows.Clear();
                            originalList = bll.GetAllSection();
                            allLocationList.Clear();
                            allLocationList = bll.GetAllLocation();
                            ////Loading_Screen.CloseForm();
                            MessageBox.Show(MessageConstants.Allsectiondatahasbeendeleted, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            ////Loading_Screen.CloseForm();
                            MessageBox.Show(MessageConstants.Errorclearall, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private bool ValidateEmptyGridview(List<LocationManagementModel> gridViewDataList)
        {
            bool result = true;

            if (gridViewDataList.Where(x => x.DepartmentCode == string.Empty).ToList().Count > 0)
            {
                result = false;
                MessageBox.Show(MessageConstants.Departmentcodecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (gridViewDataList.Where(x => x.SectionCode == string.Empty).ToList().Count > 0)
            {
                result = false;
                MessageBox.Show(MessageConstants.Sectioncodecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (gridViewDataList.Where(x => x.SectionType == string.Empty).ToList().Count > 0)
            {
                result = false;
                MessageBox.Show(MessageConstants.Sectiontypecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (gridViewDataList.Where(x => x.SectionName == string.Empty).ToList().Count > 0)
            {
                result = false;
                MessageBox.Show(MessageConstants.Sectionnamecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (gridViewDataList.Where(x => x.LocationFrom == string.Empty).ToList().Count > 0)
            {
                result = false;
                MessageBox.Show(MessageConstants.LocationFromcannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (gridViewDataList.Where(x => x.LocationTo == string.Empty).ToList().Count > 0)
            {
                result = false;
                MessageBox.Show(MessageConstants.LocationTocannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                GenSaveList(); // generate insertList updateList deleteList

                if (ValidateEmptyGridview(gridViewDataList))
                {
                    if (deleteList.Count > 0 || updateList.Count > 0 || insertList.Count > 0)
                    {
                        DialogResult dialogResult = MessageBox.Show(MessageConstants.Doyouwanttosaveallsectionbelow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (SaveCondition())
                            {
                                string resultDialog = saveDialog();
                                if (resultDialog == "OK" || resultDialog == "CANCEL")
                                {
                                    List<LocationManagementModel> loadSection = new List<LocationManagementModel>();
                                    Loading_Screen.ShowSplashScreen();
                                    loadSection = bll.GetSection(gridViewDataList);
                                    BindDataToGridView(loadSection);
                                    deleteList.Clear();
                                    updateList.Clear();
                                    insertList.Clear();
                                    allLocationList.Clear();
                                    allLocationList = bll.GetAllLocation();
                                    Loading_Screen.CloseForm();
                                    MessageBox.Show(MessageConstants.Savecomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show(MessageConstants.SavesectiondatacompletebutCannotexporttextfile, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.Nodatachanged, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private bool SaveCondition()
        {
            ////Loading_Screen.ShowSplashScreen();

            bool ResultSave = false;

            bool resultSectionExistCode = false;

            //Check ExistSectionCode
            foreach (var list in insertList)
            {
                if (bll.IsSectionCodeExist(list.SectionCode, list.SectionType))
                {
                    resultSectionExistCode = true;
                }
            }

            foreach (var list in updateList)
            {
                int count = (from d in gridViewDataList
                             where d.SectionCode.Equals(list.SectionCode) && d.FlagAction.Equals("I")
                             select d).ToList().Count();

                if (count > 0)
                {
                    if (bll.IsSectionCodeExist(list.SectionCode, list.SectionType))
                    {
                        resultSectionExistCode = true;
                    }
                }
            }

            if (resultSectionExistCode)
            {
                ////Loading_Screen.CloseForm();
                DialogResult dialogDuplicateSection = MessageBox.Show(MessageConstants.DuplicatesectioncodeDoyouwanttoreplacesection, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogDuplicateSection == DialogResult.Yes)
                {
                    ResultSave = Save(insertList, updateList, deleteList);
                    if (!(ResultSave)) { MessageBox.Show(MessageConstants.Cannotsavesectiondatatodatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            else
            {
                ////Loading_Screen.CloseForm();
                ResultSave = Save(insertList, updateList, deleteList);
                if (!(ResultSave)) { MessageBox.Show(MessageConstants.Cannotsavesectiondatatodatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }

            return ResultSave;
        }

        private bool Save(List<LocationManagementModel> insertList, List<LocationManagementModel> updateList, List<FSBT_HHT_Model.Location> deleteList)
        {
            ////Loading_Screen.ShowSplashScreen();
            try
            {
                bool saveComplete = bll.SaveSection(insertList, updateList, deleteList, UserName);
                if (saveComplete)
                {
                    originalList = bll.GetAllSection();
                    ////Loading_Screen.CloseForm();
                    return true;
                }
                else
                {
                    ////Loading_Screen.CloseForm();
                    return false;
                }
            }
            catch (Exception ex)
            {
                ////Loading_Screen.CloseForm();
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        }

        private void dataGridViewResult_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                valueBeforeEdit = dataGridViewResult.CurrentCell.Value == null ? "" : (dataGridViewResult.CurrentCell.Value.ToString()).Trim();
                gridViewDataBeforeEdit = GetCurrentDataGridView();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void dataGridViewResult_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string valueAfterEdit = dataGridViewResult.CurrentCell.Value == null ? "" : (dataGridViewResult.CurrentCell.Value.ToString()).Trim();

                if (valueAfterEdit == string.Empty)
                {
                    if (dataGridViewResult.CurrentCell.ColumnIndex == deptCodeCol)
                    {
                        MessageBox.Show(MessageConstants.Departmentcodecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (dataGridViewResult.CurrentCell.ColumnIndex == sectionCodeCol)
                    {
                        MessageBox.Show(MessageConstants.Sectioncodecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (dataGridViewResult.CurrentCell.ColumnIndex == sectionTypeCol)
                    {
                        MessageBox.Show(MessageConstants.Sectiontypecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (dataGridViewResult.CurrentCell.ColumnIndex == sectionNameCol)
                    {
                        MessageBox.Show(MessageConstants.Sectionnamecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (dataGridViewResult.CurrentCell.ColumnIndex == locationFromCol)
                    {
                        MessageBox.Show(MessageConstants.LocationFromcannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (dataGridViewResult.CurrentCell.ColumnIndex == locationToCol)
                    {
                        MessageBox.Show(MessageConstants.LocationTocannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                }
                else
                {
                    if (dataGridViewResult.CurrentCell.OwningColumn.Name.Equals("SectionCode"))
                    {
                        if (valueAfterEdit != valueBeforeEdit)
                        {
                            int num;
                            bool res = int.TryParse(valueAfterEdit, out num);
                            string valueSectionType = (string)dataGridViewResult.Rows[e.RowIndex].Cells[sectionTypeCol].Value;

                            if (res == false)
                            {
                                MessageBox.Show(MessageConstants.SectionCodeisnumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                            }
                            else if (valueAfterEdit.Length < 5)
                            {
                                if (valueAfterEdit.Length == 1)
                                {
                                    dataGridViewResult.CurrentCell.Value = "0000" + valueAfterEdit;
                                    valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                                }
                                else if (valueAfterEdit.Length == 2)
                                {
                                    dataGridViewResult.CurrentCell.Value = "000" + valueAfterEdit;
                                    valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                                }
                                else if (valueAfterEdit.Length == 3)
                                {
                                    dataGridViewResult.CurrentCell.Value = "00" + valueAfterEdit;
                                    valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                                }
                                else
                                {
                                    dataGridViewResult.CurrentCell.Value = "0" + valueAfterEdit;
                                    valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                                }
                                //MessageBox.Show(MessageConstants.SectionCodemustbe5digits, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                            }
                            else
                            {
                                string originalSectionCode = dataGridViewResult.Rows[e.RowIndex].Cells[originalSecCol].Value == null ? "" : (dataGridViewResult.Rows[e.RowIndex].Cells[originalSecCol].Value).ToString().Trim();
                                if (valueAfterEdit == originalSectionCode)
                                {
                                    dataGridViewResult.CurrentCell.Value = dataGridViewResult.Rows[e.RowIndex].Cells[originalSecCol].Value;
                                }
                                else
                                {
                                    //if (bll.IsSectionCodeExist(valueAfterEdit) || gridViewDataBeforeEdit.Any(x => x.SectionCode == valueAfterEdit))
                                    if (gridViewDataBeforeEdit.Any(x => x.SectionCode == valueAfterEdit && x.SectionType == valueSectionType))
                                    {
                                        MessageBox.Show(MessageConstants.SectionCodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                                    }
                                }
                            }
                        }
                    }
                    else if (dataGridViewResult.CurrentCell.OwningColumn.Name.Equals("LocationFrom"))
                    {
                        int num;
                        bool res = int.TryParse(valueAfterEdit, out num);

                        if (valueAfterEdit.Length < 5)
                        {
                            if (valueAfterEdit.Length == 1)
                            {
                                dataGridViewResult.CurrentCell.Value = "0000" + valueAfterEdit;
                                valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                            }
                            else if (valueAfterEdit.Length == 2)
                            {
                                dataGridViewResult.CurrentCell.Value = "000" + valueAfterEdit;
                                valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                            }
                            else if (valueAfterEdit.Length == 3)
                            {
                                dataGridViewResult.CurrentCell.Value = "00" + valueAfterEdit;
                                valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                            }
                            else
                            {
                                dataGridViewResult.CurrentCell.Value = "0" + valueAfterEdit;
                                valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                            }
                        }
                        if (res == false)
                        {
                            MessageBox.Show(MessageConstants.LocationFromisnumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                        }
                        //else if (valueAfterEdit.Length < 5)
                        //{
                        //    //MessageBox.Show(MessageConstants.LocationFrommustbe5digits, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    //dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                        //}
                        else
                        {
                            string locationCodeFrom = valueAfterEdit;
                            string sectionCode = (string)dataGridViewResult.CurrentRow.Cells[sectionCodeCol].Value;
                            string sectionType = (string)dataGridViewResult.Rows[e.RowIndex].Cells[sectionTypeCol].Value;

                            int localFrom = int.Parse(locationCodeFrom);
                            int countExistLocation = 0;

                            if (bll.IsSectionCodeExist(sectionCode, sectionType))
                            {
                                countExistLocation = (from l in allLocationList
                                                      where l.LocationCode.Equals(locationCodeFrom)
                                                      && l.SectionCode != sectionCode
                                                      && l.SectionType != sectionType
                                                      select l.LocationCode).ToList().Count();
                            }
                            else
                            {
                                countExistLocation = (from l in allLocationList
                                                      where l.LocationCode.Equals(locationCodeFrom)
                                                      select l.LocationCode).ToList().Count();
                            }

                            if (countExistLocation > 0
                                || distributionLocationExceptCurrentRow(dataGridViewResult, dataGridViewResult.CurrentRow.Index).Any(x => x.Equals(locationCodeFrom)))
                            {
                                MessageBox.Show(MessageConstants.LocationFromcannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                            }
                            else if (!(dataGridViewResult.CurrentRow.Cells[locationToCol].Value == null
                                     || dataGridViewResult.CurrentRow.Cells[locationToCol].Value == ""))
                            {
                                int localTo = int.Parse(dataGridViewResult.CurrentRow.Cells[locationToCol].Value.ToString());
                                string locationCodeTo = dataGridViewResult.CurrentRow.Cells[locationToCol].Value.ToString();

                                if (localTo < localFrom)
                                {
                                    MessageBox.Show(MessageConstants.LocationFormmustequealorlessthanLocationTo, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                                }
                                else
                                {
                                    updateLocationList(sectionCode, locationCodeFrom, locationCodeTo, sectionType);
                                }
                            }
                        }
                    }
                    else if (dataGridViewResult.CurrentCell.OwningColumn.Name.Equals("LocationTo"))
                    {
                        int num;
                        bool res = int.TryParse(valueAfterEdit, out num);
                        if (valueAfterEdit.Length < 5)
                        {
                            if (valueAfterEdit.Length == 1)
                            {
                                dataGridViewResult.CurrentCell.Value = "0000" + valueAfterEdit;
                                valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                            }
                            else if (valueAfterEdit.Length == 2)
                            {
                                dataGridViewResult.CurrentCell.Value = "000" + valueAfterEdit;
                                valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                            }
                            else if (valueAfterEdit.Length == 3)
                            {
                                dataGridViewResult.CurrentCell.Value = "00" + valueAfterEdit;
                                valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                            }
                            else
                            {
                                dataGridViewResult.CurrentCell.Value = "0" + valueAfterEdit;
                                valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                            }
                        }
                        if (res == false)
                        {
                            MessageBox.Show(MessageConstants.LocationToisnumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                        }
                        //else if (valueAfterEdit.Length < 5)
                        //{
                        //    //MessageBox.Show(MessageConstants.LocationTomustbe5digits, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    //dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                        //}
                        else
                        {
                            string locationCodeTo = valueAfterEdit;
                            string sectionCode = (string)dataGridViewResult.CurrentRow.Cells[sectionCodeCol].Value;
                            string sectionType = (string)dataGridViewResult.Rows[e.RowIndex].Cells[sectionTypeCol].Value;

                            int localTo = int.Parse(locationCodeTo);
                            int countExistLocation = 0;

                            if (bll.IsSectionCodeExist(sectionCode, sectionType))
                            {
                                countExistLocation = (from l in allLocationList
                                                      where l.LocationCode.Equals(localTo)
                                                      && l.SectionCode != sectionCode
                                                      && l.SectionType != sectionType
                                                      select l.LocationCode).ToList().Count();
                            }
                            else
                            {
                                countExistLocation = (from l in allLocationList
                                                      where l.LocationCode.Equals(localTo)
                                                      select l.LocationCode).ToList().Count();
                            }

                            if (countExistLocation > 0
                                || distributionLocationExceptCurrentRow(dataGridViewResult, dataGridViewResult.CurrentRow.Index).Any(x => x.Equals(locationCodeTo)))
                            {
                                MessageBox.Show(MessageConstants.LocationTocannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                            }
                            else if (!(dataGridViewResult.CurrentRow.Cells[locationFromCol].Value == null
                                    || dataGridViewResult.CurrentRow.Cells[locationFromCol].Value == ""))
                            {
                                int localFrom = int.Parse(dataGridViewResult.CurrentRow.Cells[locationFromCol].Value.ToString());
                                string locationCodeFrom = dataGridViewResult.CurrentRow.Cells[locationFromCol].Value.ToString();

                                if (localFrom > localTo)
                                {
                                    MessageBox.Show(MessageConstants.LocationTomustequalormorethanLocationForm, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                                }
                                else
                                {
                                    updateLocationList(sectionCode, locationCodeFrom, locationCodeTo, sectionType);
                                }
                            }
                        }
                    }
                    else if (dataGridViewResult.CurrentCell.OwningColumn.Name.Equals("DeptCode"))
                    {
                        int num;
                        bool res = int.TryParse(valueAfterEdit, out num);
                        if (res == false)
                        {
                            MessageBox.Show(MessageConstants.Departmentisnumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                        }
                    }
                    else if (dataGridViewResult.CurrentCell.OwningColumn.Name.Equals("SectionType"))
                    {
                        if (valueAfterEdit != valueBeforeEdit)
                        {
                            string valueSectionCode = (string)dataGridViewResult.Rows[e.RowIndex].Cells[sectionCodeCol].Value;

                            //if (bll.IsSectionCodeExist(valueAfterEdit) || gridViewDataBeforeEdit.Any(x => x.SectionCode == valueAfterEdit))
                            if (gridViewDataBeforeEdit.Where(x => x.SectionCode == valueSectionCode && x.SectionType == valueAfterEdit).Count() > 0)
                            {
                                MessageBox.Show(MessageConstants.SectionTypecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void updateLocationList(string sectionCode, string locationCodeFrom, string locationCodeTo, string sectionType)
        {
            var itemToRemove = allLocationList;
            //var itemToRemove = allLocationList.ExceptWhere(r => r.SectionCode.Equals(sectionCode));
            allLocationList = itemToRemove.Where(r => r.SectionCode != sectionCode).ToList();

            int localFrom = int.Parse(locationCodeFrom);
            int localTo = int.Parse(locationCodeTo);

            if (string.IsNullOrEmpty(locationCodeTo))
            {
                FSBT_HHT_Model.Location newLocation = new FSBT_HHT_Model.Location
                {
                    LocationCode = locationCodeFrom,
                    SectionCode = sectionCode,
                    SectionType = sectionType
                };

                allLocationList.Add(newLocation);
            }
            else if (string.IsNullOrEmpty(locationCodeFrom))
            {
                FSBT_HHT_Model.Location newLocation = new FSBT_HHT_Model.Location
                {
                    LocationCode = locationCodeTo,
                    SectionCode = sectionCode,
                    SectionType = sectionType
                };

                allLocationList.Add(newLocation);
            }
            else
            {
                int length = 5;
                while (localFrom <= localTo)
                {
                    FSBT_HHT_Model.Location newLocation = new FSBT_HHT_Model.Location
                    {
                        LocationCode = localFrom.ToString("D" + length),
                        SectionCode = sectionCode,
                        SectionType = sectionType
                    };

                    allLocationList.Add(newLocation);
                    localFrom++;
                }
            }
        }

        private void dataGridViewResult_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                dataGridViewResult.Rows[e.Row.Index - 1].Cells[sectionTypeCol].Value = SectionType.Items[0];
                dataGridViewResult.Rows[e.Row.Index - 1].Cells[flagCol].Value = "I";
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
            //dataGridViewResult.Rows[e.Row.Index].Cells[brandCol].Value = BrandType.Items[0];//brandtypedeleted
        }

        private List<string> distributionLocationExceptCurrentRow(DataGridView dtView, int currentRow)
        {
            List<string> listLocation = new List<string>();
            try
            {
                int length = 5;
                for (int count = 0; count < dtView.Rows.Count - 1; count++)
                {
                    if (count != currentRow)
                    {
                        string localFrom = dtView.Rows[count].Cells[locationFromCol].Value == null ? "" : dtView.Rows[count].Cells[locationFromCol].Value.ToString();
                        string localTo = dtView.Rows[count].Cells[locationToCol].Value == null ? "" : dtView.Rows[count].Cells[locationToCol].Value.ToString();

                        if (localFrom != "" && localTo != "")
                        {
                            int from = int.Parse(localFrom);
                            int to = int.Parse(localTo);

                            while (from <= to)
                            {

                                listLocation.Add(from.ToString("D" + length));
                                from++;
                            }
                        }
                        else if (localFrom != "")
                        {
                            listLocation.Add(localFrom);
                        }
                        else if (localTo != "")
                        {
                            listLocation.Add(localTo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                listLocation = new List<string>();
            }

            return listLocation;
        }

        private void GenerateReportLocation()
        {
            try
            {
                ////Loading_Screen.ShowSplashScreen();
                gridViewDataList.Clear();
                GetGridViewSectionList();

                if (gridViewDataList.Count > 0)
                {
                    LocationReportForm localReport = new LocationReportForm();

                    string sectionTypeList = "";
                    string sectionTypeListAll = "";
                    foreach (CheckBox sectionType in groupLocationType.Controls)
                    {
                        if (sectionType.Checked == true)
                        {
                            sectionTypeList = sectionTypeList + (sectionTypeList == "" ? "" : ",") + (string)sectionType.Text;
                        }

                        sectionTypeListAll = sectionTypeListAll + (sectionTypeListAll == "" ? "" : ",") + (string)sectionType.Text;

                    }

                    if (string.IsNullOrEmpty(sectionTypeList))
                    {
                        sectionTypeList = sectionTypeListAll;
                    }

                    var listsettingData = settingBLL.GetSettingData();

                    bool isCreateReportSuccess = localReport.CreateReport(gridViewDataList, sectionTypeList, listsettingData.Branch);
                    ////Loading_Screen.CloseForm();
                    if (isCreateReportSuccess)
                    {
                        localReport.StartPosition = FormStartPosition.CenterParent;
                        DialogResult dialogResult = localReport.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.cannotgeneratereport, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    ////Loading_Screen.CloseForm();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void GenerateReportBarcode()
        {
            LocationManagementModel searchSection = GetSearchCondition();
            displayData = barcodeBLL.GetSearchSection(searchSection);
            try
            {
                ////Loading_Screen.ShowSplashScreen();
                List<LocationBarcode> barcodeData = barcodeBLL.GenDataToBarCode(displayData);
                BarCodeReportForm barcodeReport = new BarCodeReportForm();
                bool isCreateReportSuccess = barcodeReport.CreateReport(barcodeData);
                ////Loading_Screen.CloseForm();
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

        private bool ExportTxtFile(string fileName)
        {
            ////Loading_Screen.ShowSplashScreen();

            string fullTitlePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(fullTitlePath, fileName);

            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    Type fieldsType = typeof(LocationManagementModel);

                    PropertyInfo[] props = fieldsType.GetProperties(BindingFlags.Public
                        | BindingFlags.Instance);

                    sw.WriteLine(props[deptCodeCol].Name + "|" + props[sectionTypeCol].Name + "|" + props[sectionCodeCol].Name + "|" + props[sectionNameCol].Name + "|" + props[locationFromCol].Name + "|" + props[locationToCol].Name);

                    foreach (var list in gridViewDataList)
                    {
                        sw.WriteLine(list.DepartmentCode + "|" + list.SectionType + "|" + list.SectionCode + "|" + list.SectionName + "|" + list.LocationFrom + "|" + list.LocationTo);
                    }
                }

                GC.Collect();
                ////Loading_Screen.CloseForm();
                return true;
            }
            catch (Exception ex)
            {
                ////Loading_Screen.CloseForm();
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        }

        private string saveDialog()
        {
            string resultDialog = "";
            try
            {
                string fileName = "tmpTxtFileExport.txt";
                var dialog = new SaveFileDialog();
                dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";


                var result = dialog.ShowDialog(); //shows save file dialog
                if (result == DialogResult.OK)
                {
                    if (ExportTxtFile(fileName))
                    {
                        var wClient = new WebClient();
                        wClient.DownloadFile(fileName, dialog.FileName);
                        resultDialog = "OK";
                        if (System.IO.File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)))
                        {
                            System.IO.File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
                        }
                    }
                    else
                    {
                        resultDialog = "ERROR";
                    }
                }
                else
                {
                    resultDialog = "CANCEL";
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                resultDialog = "";
            }
            return resultDialog;
        }

        private void btnLoadBrand_Click(object sender, EventArgs e)
        {
            try
            {

                // generate insertList updateList and deleteList
                GenSaveList();

                // Check that user have to save before searching again.
                if (deleteList.Count > 0 || insertList.Count > 0 || updateList.Count > 0)
                {
                    DialogResult dialogResult = MessageBox.Show(MessageConstants.Youhavenotsavecurrentdatasavenow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (ValidateEmptyGridview(gridViewDataList))
                        {
                            if (SaveCondition())
                            {
                                loadBrand();
                            }
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        deleteList.Clear();
                        updateList.Clear();
                        insertList.Clear();
                        loadBrand();
                    }
                }
                else
                {
                    loadBrand();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }

            ////////Loading_Screen.CloseForm();
        }

        private bool loadBrand()
        {
            try
            {
                ////Loading_Screen.ShowSplashScreen();
                List<LocationManagementModel> listBrand = bll.LoadBrand();
                dataGridViewResult.Rows.Clear();

                foreach (LocationManagementModel list in listBrand)
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridViewResult.Rows[0].Clone();
                    row.Cells[deptCodeCol].Value = (string)list.DepartmentCode;
                    row.Cells[originalSecCol].Value = (string)list.OriginSectionCode;
                    row.Cells[sectionCodeCol].Value = (string)list.SectionCode;
                    row.Cells[sectionTypeCol].Value = "Front";
                    row.Cells[sectionNameCol].Value = (string)list.SectionName;
                    row.Cells[locationFromCol].Value = (string)list.LocationFrom;
                    row.Cells[locationToCol].Value = (string)list.LocationTo;
                    row.Cells[brandCol].Value = (string)list.BrandCode;
                    row.Cells[flagCol].Value = "I";
                    row.Cells[originalSecTypeCol].Value = null;
                    dataGridViewResult.Rows.Add(row);
                }
                ////Loading_Screen.CloseForm();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        }

        private void btnImportSection_Click(object sender, EventArgs e)
        {
            try
            {
                // generate insertList updateList and deleteList
                GenSaveList();

                // Check that user have to save before searching again.
                if (deleteList.Count > 0 || insertList.Count > 0 || updateList.Count > 0)
                {
                    DialogResult dialogResult = MessageBox.Show(MessageConstants.Youhavenotsavecurrentdatasavenow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (ValidateEmptyGridview(gridViewDataList))
                        {
                            if (SaveCondition())
                            {
                                deleteList.Clear();
                                updateList.Clear();
                                insertList.Clear();
                                string resultImport = ImportSection();
                                if (resultImport == "OK")
                                {
                                    List<LocationManagementModel> loadSection = new List<LocationManagementModel>();
                                    List<LocationManagementModel> listSection = new List<LocationManagementModel>();

                                    listSection = insertList.Union(updateList).ToList();
                                    loadSection = bll.GetSection(listSection);
                                    BindDataToGridView(loadSection);
                                    deleteList.Clear();
                                    updateList.Clear();
                                    insertList.Clear();
                                    MessageBox.Show(MessageConstants.Importcomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else if (resultImport == "ERROR")
                                {
                                    MessageBox.Show(MessageConstants.Cannotimportsectiondatatodatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else if (resultImport == "CANCEL")
                                {
                                    //do nothing
                                }
                                else
                                {
                                    MessageBox.Show(resultImport);
                                }
                            }
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        deleteList.Clear();
                        updateList.Clear();
                        insertList.Clear();
                        string resultImport = ImportSection();
                        if (resultImport == "OK")
                        {
                            List<LocationManagementModel> loadSection = new List<LocationManagementModel>();
                            List<LocationManagementModel> listSection = new List<LocationManagementModel>();

                            listSection = insertList.Union(updateList).ToList();
                            loadSection = bll.GetSection(listSection);
                            BindDataToGridView(loadSection);
                            deleteList.Clear();
                            updateList.Clear();
                            insertList.Clear();
                            MessageBox.Show(MessageConstants.Importcomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (resultImport == "ERROR")
                        {
                            MessageBox.Show(MessageConstants.Cannotimportsectiondatatodatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (resultImport == "CANCEL")
                        {
                            //do nothing
                        }
                        else
                        {
                            MessageBox.Show(resultImport);
                        }
                    }
                }
                else
                {
                    deleteList.Clear();
                    updateList.Clear();
                    insertList.Clear();
                    string resultImport = ImportSection();
                    if (resultImport == "OK")
                    {
                        List<LocationManagementModel> loadSection = new List<LocationManagementModel>();
                        List<LocationManagementModel> listSection = new List<LocationManagementModel>();

                        listSection = insertList.Union(updateList).ToList();
                        loadSection = bll.GetSection(listSection);
                        BindDataToGridView(loadSection);
                        deleteList.Clear();
                        updateList.Clear();
                        insertList.Clear();
                        MessageBox.Show(MessageConstants.Importcomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (resultImport == "ERROR")
                    {
                        MessageBox.Show(MessageConstants.Cannotimportsectiondatatodatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (resultImport == "CANCEL")
                    {
                        //do nothing
                    }
                    else
                    {
                        MessageBox.Show(resultImport);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private string ImportSection()
        {
            string result = "OK";

            var FD = new System.Windows.Forms.OpenFileDialog();
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileToOpen = FD.FileName;
                System.IO.StreamReader reader = new System.IO.StreamReader(fileToOpen);
                DataTable dtSuccess = new DataTable();
                DataTable dtError = new DataTable();

                try
                {
                    ////Loading_Screen.ShowSplashScreen();

                    using (StreamReader sr = reader)
                    {
                        string line;
                        int i = 0;
                        string[] words;

                        while ((line = sr.ReadLine()) != null)
                        {
                            words = line.Split('|');

                            //Data in Txt to DataTable
                            if (i == 0)
                            {

                                dtError.Columns.Add("Line");
                                dtError.Columns.Add("Reason");

                                if (words.Length == headerImportSection.Length)
                                {
                                    for (int x = 0; x < words.Length; x++)
                                    {
                                        if (words[x].ToUpper().Equals(headerImportSection[x].ToUpper()))
                                        {
                                            dtSuccess.Columns.Add(words[x]);
                                        }
                                        else
                                        {
                                            DataRow dtRowError = dtError.NewRow();
                                            dtRowError[0] = (i + 1);
                                            dtRowError[1] = "Header is Invalid format.";
                                            dtError.Rows.Add(dtRowError);
                                            break;
                                        }
                                    }

                                    dtSuccess.Columns.Add("Line");
                                }
                                else
                                {
                                    DataRow dtRowError = dtError.NewRow();
                                    dtRowError[0] = (i + 1);
                                    dtRowError[1] = "Header is Invalid format.";
                                    dtError.Rows.Add(dtRowError);
                                }
                            }
                            else
                            {
                                if (words.Length > 0)
                                {
                                    DataRow dtRowSuccess = dtSuccess.NewRow();

                                    for (int x = 0; x < words.Length; x++)
                                    {
                                        DataRow dtRowError = dtError.NewRow();
                                        if (x == (sectionCodeCol - 1) || x == (locationFromCol - 1) || x == (locationToCol - 1))
                                        {
                                            int num;
                                            bool res = int.TryParse(words[x], out num);
                                            if (res == false)
                                            {
                                                dtRowError[0] = (i + 1);
                                                if (x == (sectionCodeCol - 1))
                                                {
                                                    dtRowError[1] = "Section Code is number only.";
                                                }
                                                else if (x == (locationFromCol - 1))
                                                {
                                                    dtRowError[1] = "Location From is number only.";
                                                }
                                                else if (x == (locationToCol - 1))
                                                {
                                                    dtRowError[1] = "Location To is number only.";
                                                }
                                            }
                                            else if (words[x].Length != 5)
                                            {
                                                dtRowError[0] = (i + 1);
                                                if (x == (sectionCodeCol - 1))
                                                {
                                                    dtRowError[1] = "Section Code must be 5 digits.";
                                                }
                                                else if (x == (locationFromCol - 1))
                                                {
                                                    dtRowError[1] = "Location From must be 5 digits.";
                                                }
                                                else if (x == (locationToCol - 1))
                                                {
                                                    dtRowError[1] = "Location To must be 5 digits.";
                                                }
                                            }
                                            else
                                            {
                                                dtRowSuccess[x] = words[x];
                                            }
                                        }
                                        else
                                        {
                                            dtRowSuccess[x] = words[x];
                                        }
                                        if (dtRowError[0].ToString() != string.Empty) { dtError.Rows.Add(dtRowError); }
                                    }

                                    if (dtRowSuccess[0].ToString() != string.Empty)
                                    {
                                        dtRowSuccess["Line"] = (i + 1);
                                        dtSuccess.Rows.Add(dtRowSuccess);
                                    }
                                }
                            }

                            if (i == 0 && dtError.Rows.Count > 0)
                            {
                                break;
                            }

                            i++;
                        }

                        //check duplicae key in files
                        Hashtable hasResult = new Hashtable();
                        hasResult = checkDuplicateKeyInFiles(dtSuccess, dtError);
                        bool resultCheckFile = (bool)hasResult["result"];
                        dtError = (DataTable)hasResult["tableError"];

                        if (dtError.Rows.Count <= 0)
                        {
                            //DataTable To list
                            insertList = (from d in dtSuccess.AsEnumerable()
                                          select new LocationManagementModel
                                          {
                                              DepartmentCode = d.Field<string>("DepartmentCode"),
                                              OriginSectionCode = d.Field<string>("SectionCode"),
                                              SectionCode = d.Field<string>("SectionCode"),
                                              SectionType = d.Field<string>("SectionType"),
                                              SectionName = d.Field<string>("SectionName"),
                                              LocationFrom = d.Field<string>("LocationFrom"),
                                              LocationTo = d.Field<string>("LocationTo"),
                                              OriginSectionType = d.Field<string>("SectionType")
                                          }).ToList();

                            ////Loading_Screen.CloseForm();

                            List<LocationManagementModel> listSection = bll.GetAllSection();

                            if (listSection.Count > 0)
                            {
                                //Delete All Section Code not check duplicate key
                                DialogResult dialogResult = MessageBox.Show(MessageConstants.Oldsectiondatawillbedeletedafterthisaction, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    ////Loading_Screen.ShowSplashScreen();
                                    bll.ClearAllSection();
                                    if (Save(insertList, updateList, deleteList))
                                    {
                                        result = "OK";
                                    }
                                    else
                                    {
                                        result = "ERROR";
                                    }

                                    ////Loading_Screen.CloseForm();
                                }
                                else
                                {
                                    result = "CANCEL";
                                    ////Loading_Screen.CloseForm();
                                }
                            }
                            else
                            {
                                ////Loading_Screen.ShowSplashScreen();
                                bll.ClearAllSection();
                                if (Save(insertList, updateList, deleteList))
                                {
                                    result = "OK";
                                }
                                else
                                {
                                    result = "ERROR";
                                }

                                ////Loading_Screen.CloseForm();
                            }
                        }
                        else
                        {
                            result = "";

                            foreach (DataRow dr in dtError.Rows)
                            {
                                result += "Line " + dr[0].ToString() + " : " + dr[1].ToString() + " \n";
                            }

                            ////Loading_Screen.CloseForm();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(String.Format("Exception : {0}", ex.StackTrace));
                    result = "ERROR";
                }
            }
            else
            {
                result = "CANCEL";
            }
            return result;
        }

        private Hashtable checkDuplicateKeyInFiles(DataTable dtSuccess, DataTable dtError)
        {
            Hashtable hastResult = new Hashtable();
            bool result = true;
            List<string> listLocation = new List<string>();
            foreach (DataRow dr in dtSuccess.Rows)
            {
                int start = int.Parse(dr["LocationFrom"].ToString());
                int end = int.Parse(dr["LocationTo"].ToString());
                int length = 5;

                for (int i = start; i <= end; i++)
                {
                    listLocation.Add(i.ToString("D" + length));
                }
            }

            foreach (DataRow dr in dtSuccess.Rows)
            {
                bool isduplicate = false;
                int countExists = 0;

                countExists = (from d in dtSuccess.AsEnumerable()
                               where d.Field<string>("SectionCode").Equals(dr["SectionCode"])
                               && d.Field<string>("SectionType").Equals(dr["SectionType"])
                               select d).ToList().Count();

                if (countExists == 1)
                {
                    //countExists = (from d in dtSuccess.AsEnumerable()
                    //               where int.Parse(d.Field<string>("LocationFrom")) >= int.Parse(dr["LocationFrom"].ToString())
                    //               && int.Parse(d.Field<string>("LocationTo")) <= int.Parse(dr["LocationTo"].ToString())
                    //               select d).ToList().Count();

                    int start = int.Parse(dr["LocationFrom"].ToString());
                    int end = int.Parse(dr["LocationTo"].ToString());
                    int length = 5;

                    for (int i = start; i <= end; i++)
                    {
                        string locationCurrent = i.ToString("D" + length);
                        int countLocation = (from l in listLocation
                                             where l.Equals(locationCurrent)
                                             select l).ToList().Count();

                        if (countLocation > 1)
                        {
                            isduplicate = true;
                        }
                    }
                }
                else
                {
                    isduplicate = true;
                }

                if (isduplicate)
                {
                    DataRow dtRowError = dtError.NewRow();
                    dtRowError[0] = dr["Line"];
                    dtRowError[1] = "Location " + dr["LocationFrom"] + " - " + dr["LocationTo"] + " Section : " + dr["SectionCode"] + " of " + dr["SectionType"] + " is Duplicate.";
                    if (dtRowError[0].ToString() != string.Empty) { dtError.Rows.Add(dtRowError); }
                    result = false;
                }
            }

            hastResult.Add("tableError", dtError);
            hastResult.Add("result", result);

            return hastResult;
        }

        private void btnReportLocation_Click(object sender, EventArgs e)
        {
            try
            {
                // generate insertList updateList and deleteList
                GenSaveList();

                // Check that user have to save before searching again.
                if (deleteList.Count > 0 || insertList.Count > 0 || updateList.Count > 0)
                {
                    DialogResult dialogResult = MessageBox.Show(MessageConstants.Youhavenotsavecurrentdatasavenow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (ValidateEmptyGridview(gridViewDataList))
                        {
                            if (SaveCondition())
                            {
                                DoSearchData();
                                GenerateReportLocation();
                            }
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        DoSearchData();
                        GenerateReportLocation();
                        deleteList.Clear();
                        updateList.Clear();
                        insertList.Clear();
                    }
                }
                else
                {
                    DoSearchData();
                    GenerateReportLocation();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void btnReportBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                // generate insertList updateList and deleteList
                GenSaveList();

                // Check that user have to save before searching again.
                if (deleteList.Count > 0 || insertList.Count > 0 || updateList.Count > 0)
                {
                    DialogResult dialogResult = MessageBox.Show(MessageConstants.Youhavenotsavecurrentdatasavenow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (ValidateEmptyGridview(gridViewDataList))
                        {
                            if (SaveCondition())
                            {
                                DoSearchData();
                                GenerateReportBarcode();
                            }
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        DoSearchData();
                        GenerateReportBarcode();
                        deleteList.Clear();
                        updateList.Clear();
                        insertList.Clear();
                    }
                }
                else
                {
                    DoSearchData();
                    GenerateReportBarcode();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private LocationManagementModel GetSearchCondition()
        {
            LocationManagementModel searchSection = new LocationManagementModel();
            searchSection.SectionCode = textBoxSecCode.Text;
            searchSection.SectionName = textBoxSecName.Text;
            searchSection.LocationFrom = textBoxLoForm.Text;
            searchSection.LocationTo = textBoxLoTo.Text;
            searchSection.DepartmentCode = textBoxDept.Text;

            string sectionTypeList = "";
            foreach (CheckBox sectionType in groupLocationType.Controls)
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
