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
        public string UserName { get; set; }
        private SystemSettingBll settingBLL = new SystemSettingBll();
        private LocationManagementBll bll = new LocationManagementBll();
        private LogErrorBll logBll = new LogErrorBll();
        ReportManagementBll barcodeBLL = new ReportManagementBll();

        private List<LocationManagementModel> originalList = new List<LocationManagementModel>();
        private List<LocationManagementModel> searchList = new List<LocationManagementModel>();
        private List<LocationManagementModel> gridViewDataList = new List<LocationManagementModel>();
        private List<LocationModel> gridViewLocationList = new List<LocationModel>();

        //private List<LocationModel> deleteList = new List<LocationModel>();
        private List<LocationManagementModel> deleteList = new List<LocationManagementModel>();
        private List<LocationManagementModel> updateList = new List<LocationManagementModel>();
        private List<LocationManagementModel> insertList = new List<LocationManagementModel>();

        private List<LocationModel> allLocationList = new List<LocationModel>();
        private string valueBeforeEdit;
        private List<LocationManagementModel> gridViewDataBeforeEdit = new List<LocationManagementModel>();
        List<LocationBarcode> displayData = new List<LocationBarcode>();

        private const int plantCol = 0;
        private const int countSheetCol = 1;
        private const int storageLocationCol = 2;
        private const int sectionCodeCol = 3;
        private const int sectionNameCol = 4;
        private const int locationFromCol = 5;
        private const int locationToCol = 6;
        private const int flagCol = 7;
        private const int brandCodeCol = 8;

        private const int originalPlantCol = 9;
        private const int originalCountSheetCol = 10;
        private const int originalStorageCol = 11;
        private const int originalSectionCol = 12;
        private const int originalSectionNameCol = 13;

        //private String[] headerImportSection = { "Plant", "StorageLocation", "SectionCode", "SectionName", "LocationFrom", "LocationTo" };
        private String[] headerImportSection = { "StorageLocation", "SectionCode", "SectionName", "LocationFrom", "LocationTo" };
        private String[] headerImportBrand = { "StorageLocation", "BrandCode", "BrandName", "LocationFrom", "LocationTo" };
        private String[] headerImportLocation = { "Plant", "CountSheet", "StorageLocation", "SectionCode", "SectionName", "LocationFrom", "LocationTo" };
        private String[] headerImportLocationBrand = { "Plant", "CountSheet", "StorageLocation", "BrandCode", "BrandName", "LocationFrom", "LocationTo" };
        private string StorageLocationType;
        private bool IsLoadBrand = false;
      
        #region Initial

        public LocationManagementForm()
        {
            InitializeComponent();
        }

        public LocationManagementForm(string username)
        {
            try
            {
                InitializeComponent();
                AddDropDownPlant();
                AddDropDownStorageLocation();
                AddDropDownCountSheet();
                //AddDropDownPlantGrid();
                AddDropDownCountSheetGrid();
                UserName = username;

                originalList.Clear();
                originalList = bll.GetAllSection();
                allLocationList.Clear();
                allLocationList = bll.GetAllLocation();

            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void LocationManagementForm_Load(object sender, EventArgs e)
        {
            try
            {
                var settingData = settingBLL.GetSettingData();
                //settingBLL.UpdateSetting("StorageLocationType", "string", "SECTION");
                StorageLocationType = settingBLL.GetSettingStringByKey("StorageLocationType");
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        #endregion

        #region Event
        //generate deleteList when user delete gridview's row
        private void dataGridViewResult_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                string originalPlantCode = (string)dataGridViewResult.Rows[e.Row.Index].Cells[originalPlantCol].Value;
                string originalCountsheet = (string)dataGridViewResult.Rows[e.Row.Index].Cells[originalCountSheetCol].Value;
                string originalStorageLocationCode = (string)dataGridViewResult.Rows[e.Row.Index].Cells[originalStorageCol].Value;
                string originalSectionCode = (string)dataGridViewResult.Rows[e.Row.Index].Cells[originalSectionCol].Value;
                string originalSectionName = (string)dataGridViewResult.Rows[e.Row.Index].Cells[originalSectionNameCol].Value;

                string locationfrom = (string)dataGridViewResult.Rows[e.Row.Index].Cells[locationFromCol].Value;
                string locationto = (string)dataGridViewResult.Rows[e.Row.Index].Cells[locationToCol].Value;

                if (!string.IsNullOrEmpty(originalPlantCode) && 
                    !string.IsNullOrEmpty(originalStorageLocationCode) && 
                    !string.IsNullOrEmpty(originalSectionCode) && 
                    !string.IsNullOrEmpty(originalSectionName) && 
                    !string.IsNullOrEmpty(locationfrom) && 
                    !string.IsNullOrEmpty(locationfrom) ||
                    !string.IsNullOrEmpty(originalCountsheet))
                {
                    //LocationModel deleteSection = new LocationModel
                    LocationManagementModel deleteSection = new LocationManagementModel
                    {
                        PlantCode = originalPlantCode,
                        CountSheet = originalCountsheet,
                        StorageLocationCode = originalStorageLocationCode,
                        SectionCode = originalSectionCode,
                        SectionName = originalSectionName,
                        LocationFrom = locationfrom,
                        LocationTo = locationto
                    };
                    deleteList.Add(deleteSection);
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                            if (ValidateEmptyGridview() && ValidateNumbericGridview() && ValidateIsExistsMaster() && ValidateDupSubLocation())
                            {
                                if (SaveCondition())
                                {                                   
                                    Loading_Screen.CloseForm();
                                    DoSearchData();
                                }
                            }
                            Loading_Screen.CloseForm();
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            Loading_Screen.CloseForm();
                            DoSearchData();
                            deleteList.Clear();
                            updateList.Clear();
                            insertList.Clear();
                        }
                    }
                    else
                    {
                        Loading_Screen.CloseForm();
                        DoSearchData();
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                            if (ValidateEmptyGridview() && ValidateNumbericGridview() && ValidateIsExistsMaster() && ValidateDupSubLocation())
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
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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

                        List<LocationManagementModel> deletedsectionCodeList = new List<LocationManagementModel>();

                        for (int count = 0; count < dataGridViewResult.Rows.Count - 1; count++)
                        {
                            string plant = (string)dataGridViewResult.Rows[count].Cells[plantCol].Value;
                            string countsheet = (string)dataGridViewResult.Rows[count].Cells[countSheetCol].Value;
                            string storageLocationCode = (string)dataGridViewResult.Rows[count].Cells[storageLocationCol].Value;
                            string sectionCode = (string)dataGridViewResult.Rows[count].Cells[sectionCodeCol].Value;
                            string sectionName = (string)dataGridViewResult.Rows[count].Cells[sectionNameCol].Value;
                            string locationFrom = (string)dataGridViewResult.Rows[count].Cells[locationFromCol].Value;
                            string locationTo = (string)dataGridViewResult.Rows[count].Cells[locationToCol].Value;

                            if (bll.IsSectionCodeExistByStoreLocCodeCountsheetPlant(plant, countsheet, storageLocationCode, sectionCode))
                            {
                                LocationManagementModel deleteSection = new LocationManagementModel
                                {
                                    PlantCode = plant,
                                    CountSheet = countsheet,
                                    StorageLocationCode = storageLocationCode,
                                    SectionCode = sectionCode,
                                    SectionName = sectionName,
                                    LocationFrom = locationFrom,
                                    LocationTo = locationTo
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
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void UpdateStorageLocationType(string type)
        {
            if(type == "Brand")
            {
                settingBLL.UpdateSetting("StorageLocationType", "string", "BRAND");
            }
            else
            {
                settingBLL.UpdateSetting("StorageLocationType", "string", "SECTION");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(IsHaveDataToSave() )
                {
                    if (ValidateDataForSave())
                    {
                        if (SaveCondition())
                        {
                            if (dataGridViewResult.Columns[sectionCodeCol].HeaderText == "Brand Code")
                            {
                                settingBLL.UpdateSetting("StorageLocationType", "string", "BRAND");
                            }
                            else
                            {
                                settingBLL.UpdateSetting("StorageLocationType", "string", "SECTION");
                            }
                            
                            List<LocationManagementModel> loadSection = new List<LocationManagementModel>();
                            Loading_Screen.ShowSplashScreen();

                            loadSection = bll.GetSection(gridViewDataList);
                            BindDataToGridView(loadSection);
                            deleteList.Clear();
                            updateList.Clear();
                            insertList.Clear();

                            originalList.Clear();
                            originalList = bll.GetAllSection();
                            allLocationList.Clear();
                            allLocationList = bll.GetAllLocation();

                            Loading_Screen.CloseForm();
                            MessageBox.Show(MessageConstants.Savecomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(MessageConstants.Nochangeddata, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                Loading_Screen.CloseForm();
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }
        private void btnExportTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                GenSaveList(); // generate insertList updateList deleteList

                // 1. If no data 
                if (gridViewDataList.Count == 0)
                {
                    MessageBox.Show(MessageConstants.Nodata, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                // 2. Check empty data and incorrect format -> enter this condition when not found any empty data and correct format
                else if (ValidateEmptyGridviewExceptPlantAndCountsheet() && ValidateNumbericGridview())
                {
                    if (ValidateDupSubLocation())
                    {
                        string resultDialog = saveTemplateDialog();
                        if (resultDialog == "OK")
                        {
                            DialogResult dialogResult = MessageBox.Show("SAVE FILE");
                        }
                        else if(resultDialog == "FILENAMEERROR")
                        {
                            MessageBox.Show("File Name should start with 'SECTION-' or 'BRAND-'  ", MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else 
                        {
                            if (resultDialog != "CANCEL")
                                MessageBox.Show("Export File Error", MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.ExportLocationFromToIncorrect, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private bool AskToSave()
        {
            bool result = false;

            if(IsHaveDataToSave())
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
            return result;
        }

        private bool IsClearGrid()
        {
            bool isClear = false;
            if (dataGridViewResult.RowCount > 1)
            {
                DialogResult dialogResult = MessageBox.Show(MessageConstants.Doyouwanttoclearallsectionbelow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    isClear = true;
                }
            }
            return isClear;
        }

        private bool IsHaveDataToSave()
        {
            bool result = false;

            GenSaveList(); // generate insertList updateList deleteList
            if (deleteList.Count > 0 || updateList.Count > 0 || insertList.Count > 0)
            {
                result = true;
            }
            return result;
        }


        private bool ValidateDataForSave()
        {
            bool result = false;
            try
            {
                int countSku = 0;
                int countBarcode = 0;
                int countRegularPrice = 0;

                if (IsHaveDataExistInMaster(ref countSku, ref countBarcode, ref countRegularPrice))
                {
                    if (countSku > 0 && countBarcode > 0 && countRegularPrice > 0)
                    {
                        if (ValidateEmptyGridview())
                        {
                            if(ValidateNumbericGridview())
                            {
                                if (ValidateDuplicateInGrid())
                                {
                                    if(ValidateIsExistsMaster())
                                    {
                                        if (ValidateDupSubLocation())
                                        {
                                            //if (ValidateDupSubLocationWithMaster())
                                            //{    
                                                result = true;
                                           // }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string message = "Not found master data of  ";
                        string reason = "";

                        if (countSku == 0)
                        {
                            reason = reason + ",SKU";
                        }

                        if (countBarcode == 0)
                        {
                            reason = reason + ",Barcode";
                        }

                        if (countRegularPrice == 0)
                        {
                            reason = reason + ",Regular Price";
                        }

                        message = message + reason.Substring(1, reason.Count() - 1);

                        MessageBox.Show(message, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }                                  
                else
                {
                    if (ValidateEmptyGridviewExceptPlantAndCountsheet())
                    {
                        if(ValidateNumbericGridview())
                        {
                            if (ValidateDuplicateInGrid())
                            {
                                if (ValidateDupSubLocation())
                                {
                                    //if (ValidateDupSubLocationWithMaster())
                                    //{
                                        result = true;
                                    //}
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = false;
            }
            return result;
        }

        private void btnImportSection_Click(object sender, EventArgs e)
        {
            try
            {
                if(AskToSave())
                {
                    if (ValidateDataForSave())
                    {
                        if (SaveCondition())
                        {
                            if (IsClearGrid())
                                dataGridViewResult.Rows.Clear(); 
                            
                            deleteList.Clear();
                            updateList.Clear();
                            insertList.Clear();
                            List<LocationManagementModel> listDuplicateKeyInDB = new List<LocationManagementModel>();
                            string fileType = "";
                            string resultImport = ImportSection(ref listDuplicateKeyInDB, ref fileType, "Template");
                            if (resultImport == "OK")
                            {
                                if (fileType.Length > 0)
                                {
                                    if (fileType.ToLower().Equals("brand"))
                                    {
                                        dataGridViewResult.Columns[sectionCodeCol].HeaderText = "Brand Code";
                                        dataGridViewResult.Columns[sectionNameCol].HeaderText = "Brand Name";
                                        //IsLoadBrand = true;
                                        //UpdateStorageLocationType();
                                    }
                                    else
                                    {
                                        dataGridViewResult.Columns[sectionCodeCol].HeaderText = "Section Code";
                                        dataGridViewResult.Columns[sectionNameCol].HeaderText = "Section Name";
                                        //IsLoadBrand = false;
                                        //UpdateStorageLocationType();
                                    }
                                }

                                if (listDuplicateKeyInDB.Count > 0)
                                {
                                    string msg = "Section below is Duplicate Key " + System.Environment.NewLine;
                                    foreach (var list in listDuplicateKeyInDB)
                                    {
                                        msg += "Plant : " + list.PlantCode + "CountSheet :" + list.CountSheet + "Section Code : " + list.SectionCode + " StorageLocation Code : " + list.StorageLocationCode + System.Environment.NewLine;
                                    }

                                    MessageBox.Show(msg, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    MessageBox.Show(MessageConstants.Importcomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else if (resultImport == "ERROR")
                            {
                                MessageBox.Show(MessageConstants.Cannotloadtemplate, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                else
                {
                    if (IsClearGrid())
                        dataGridViewResult.Rows.Clear(); 

                    deleteList.Clear();
                    updateList.Clear();
                    insertList.Clear();
                    List<LocationManagementModel> listDuplicateKeyInDB = new List<LocationManagementModel>();
                    string fileType = "";
                    string resultImport = ImportSection(ref listDuplicateKeyInDB, ref fileType, "Template");
                    if (resultImport == "OK")
                    {
                        if (fileType.Length > 0)
                        {
                            if (fileType.ToLower().Equals("brand"))
                            {
                                dataGridViewResult.Columns[sectionCodeCol].HeaderText = "Brand Code";
                                dataGridViewResult.Columns[sectionNameCol].HeaderText = "Brand Name";
                                // IsLoadBrand = true;
                                //UpdateStorageLocationType();
                            }
                            else
                            {
                                dataGridViewResult.Columns[sectionCodeCol].HeaderText = "Section Code";
                                dataGridViewResult.Columns[sectionNameCol].HeaderText = "Section Name";
                                //IsLoadBrand = false;
                                //UpdateStorageLocationType();
                            }
                        }
                        
                        if (listDuplicateKeyInDB.Count > 0)
                        {
                            string msg = "Section below is Duplicate Key " + System.Environment.NewLine;
                            foreach (var list in listDuplicateKeyInDB)
                            {
                                msg += "Plant : " + list.PlantCode + "CountSheet :" + list.CountSheet + "Section Code : " + list.SectionCode + " StorageLocation Code : " + list.StorageLocationCode + System.Environment.NewLine;
                            }

                            MessageBox.Show(msg, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show(MessageConstants.Importcomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
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
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.InnerException == null ? ex.Message : ex.InnerException.ToString(), DateTime.Now);
                MessageBox.Show("ERROR");
            }
        }

        private void btnReportLocation_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateSearch())
                {
                    if (AskToSave())
                    {
                        if (ValidateDataForSave())
                        {
                            if (SaveCondition())
                            {
                                DoSearchData();
                                GenerateReportLocation();
                            }
                        }
                    }
                    else
                    {
                        DoSearchData();
                        GenerateReportLocation();
                        deleteList.Clear();
                        updateList.Clear();
                        insertList.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            
            //try
            //{
            //    // generate insertList updateList and deleteList
            //    GenSaveList();

            //    // Check that user have to save before searching again.
            //    if (deleteList.Count > 0 || insertList.Count > 0 || updateList.Count > 0)
            //    {
            //        DialogResult dialogResult = MessageBox.Show(MessageConstants.Youhavenotsavecurrentdatasavenow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //        if (dialogResult == DialogResult.Yes)
            //        {
            //            if (ValidateEmptyGridview(gridViewDataList) && ValidateNumbericGridview(gridViewDataList) && ValidateIsExistsMaster(gridViewDataList))
            //            {
            //                if (SaveCondition())
            //                {
            //                    DoSearchData();
            //                    GenerateReportLocation();
            //                }
            //            }
            //        }
            //        else if (dialogResult == DialogResult.No)
            //        {
            //            DoSearchData();
            //            GenerateReportLocation();
            //            deleteList.Clear();
            //            updateList.Clear();
            //            insertList.Clear();
            //        }
            //    }
            //    else
            //    {
            //        DoSearchData();
            //        GenerateReportLocation();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            //    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            //}
        }
        private void btnReportBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                if (AskToSave())
                {
                    if (ValidateDataForSave())
                    {
                        if (SaveCondition())
                        {
                            //Loading_Screen.ShowSplashScreen();
                            DoSearchData();
                            GenerateReportBarcode();
                            //Loading_Screen.CloseForm();
                            //MessageBox.Show(MessageConstants.Savecomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    DoSearchData();
                    GenerateReportBarcode();
                    deleteList.Clear();
                    updateList.Clear();
                    insertList.Clear();
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            
        }
        private void BtnLoadBrand_Click(object sender, EventArgs e)
        {
            try
            {
                IsLoadBrand = true;

                if (IsLoadBrand)
                {
                    // Check that user have to save before searching again.
                    if (AskToSave())
                    {
                        if (ValidateDataForSave())
                        {
                            if (SaveCondition())
                            {
                                deleteList.Clear();
                                updateList.Clear();
                                insertList.Clear();
                                loadBrand(IsClearGrid());
                            }
                        }
                    }
                    else
                    {                      
                        deleteList.Clear();
                        updateList.Clear();
                        insertList.Clear();
                        loadBrand(IsClearGrid());
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        #endregion

        #region Function
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
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        protected void AddDropDownCountSheetGrid()
        {
            try
            {
                //Get DropDown
                List<string> listCountSheet = new List<string>();

                listCountSheet = settingBLL.GetDropDownAllCountSheetSKU();
                CountSheet.Items.Clear();

                if (listCountSheet.Count > 0)
                {
                    foreach (var l in listCountSheet)
                    {
                        CountSheet.Items.Add(l);
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        //protected void AddDropDownPlantGrid()
        //{
        //    try
        //    {
        //        //Get DropDown
        //        List<string> listPlant = new List<string>();

        //        listPlant = settingBLL.GetAllPlant();
        //        PlantCode.Items.Clear();

        //        if (listPlant.Count > 0)
        //        {
        //            foreach (var l in listPlant)
        //            {
        //                PlantCode.Items.Add(l);
        //            }
        //        }
        //        PlantCode.Items.Add("");
        //    }
        //    catch (Exception ex)
        //    {
        //        logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
        //        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
        //    }
        //}
       
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
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
        // searching and showing section data to gridview
        private void DoSearchData()
        {
            try
            {
                ParameterModel searchParameter = new ParameterModel();
                string plant = comboBoxPlant.Text;
                string countsheet = comboBoxCountSheet.Text;
                string storagelocation = comboBoxStorageLocal.Text;


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

                string locationForm = textBoxLoForm.Text.Trim();
                string locationTo = textBoxLoTo.Text.Trim();

                if (storagelocation.ToUpper() != "ALL")
                {
                    storagelocation = storagelocation.Substring(0,4);
                }
 
                searchParameter = new ParameterModel
                {
                    Plant = plant,
                    Countsheet = countsheet,
                    StorageLocationCode = storagelocation,
                    LocationFromCode = locationForm,
                    LocationToCode = locationTo
                };

                textBoxFilter.Clear();

                //AddDropDownPlantGrid();
                AddDropDownCountSheetGrid();
                if (searchList != null)
                {
                    searchList.Clear();
                }
                //searchList = bll.SearchSection(plant,countsheet,storagelocation,"","",locationForm,locationTo,"","","","");
                searchList = bll.SearchSection(searchParameter);

                if (searchList != null )
                {
                    originalList.Clear();
                    originalList = bll.GetAllSection();
                    BindDataToGridView(searchList);
                }
                else 
                {
                    dataGridViewResult.Rows.Clear();
                    Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.NoAuditdatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void DoSearchReportData()
        {
            try
            {
                string plant = comboBoxPlant.Text;
                string countsheet = comboBoxCountSheet.Text;
                string storagelocation = comboBoxStorageLocal.Text;

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

                string locationForm = textBoxLoForm.Text.Trim();
                string locationTo = textBoxLoTo.Text.Trim();

                if (storagelocation.ToUpper() != "ALL")
                {
                    storagelocation = storagelocation.Substring(0, 4);
                }

                textBoxFilter.Clear();

                //AddDropDownPlantGrid();
                AddDropDownCountSheetGrid();

                if (searchList != null)
                {
                    searchList.Clear();
                }
                searchList = bll.SearchReportSection(plant, countsheet, storagelocation, "", "", locationForm, locationTo, "", "", "", "");
                originalList.Clear();
                originalList = bll.GetAllSection();

                BindDataToGridView(searchList);
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                        row.Cells[plantCol].Value = (string)section.PlantCode;
                        row.Cells[countSheetCol].Value = (string)section.CountSheet;
                        row.Cells[storageLocationCol].Value = (string)section.StorageLocationCode;
                        row.Cells[sectionCodeCol].Value = (string)section.SectionCode;
                        row.Cells[sectionNameCol].Value = (string)section.SectionName;
                        row.Cells[brandCodeCol].Value = (string)section.BrandCode;
                        row.Cells[locationFromCol].Value = (string)section.LocationFrom;
                        row.Cells[locationToCol].Value = (string)section.LocationTo;

                        row.Cells[originalPlantCol].Value = (string)section.OriginalPlantCode;
                        row.Cells[originalCountSheetCol].Value = (string)section.OriginalCountSheet;
                        row.Cells[originalStorageCol].Value = (string)section.OriginalStorageLocCode;
                        row.Cells[originalSectionCol].Value = (string)section.OriginalSectionCode;
                        row.Cells[originalSectionNameCol].Value = (string)section.OriginalSectionName;

                        dataGridViewResult.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }
        private bool SaveCondition()
        {
            bool ResultSave = false;
            bool resultSectionExistCode = false;

            //Check ExistSectionCode
            foreach (var list in insertList)
            {
                if (bll.IsSectionCodeExistByStoreLocCodeCountsheetPlant(list.PlantCode,list.CountSheet,list.StorageLocationCode,list.SectionCode))
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
                    if (bll.IsSectionCodeExistByStoreLocCodeCountsheetPlant(list.PlantCode, list.CountSheet, list.StorageLocationCode, list.SectionCode))
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
        private bool Save(List<LocationManagementModel> insertList, List<LocationManagementModel> updateList, List<LocationManagementModel> deleteList)
        {
            Loading_Screen.ShowSplashScreen();
            try
            {
                bool saveComplete = bll.SaveSection(insertList, updateList, deleteList, UserName);
                if (saveComplete)
                {
                    originalList = bll.GetAllSection();
                    Loading_Screen.CloseForm();
                    insertList.Clear();
                    updateList.Clear();
                    deleteList.Clear();
                    return true;
                }
                else
                {
                    Loading_Screen.CloseForm();
                    return false;
                }
            }
            catch (Exception ex)
            {
                ////Loading_Screen.CloseForm();
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }
        private void updateLocationList(string sectionCode, string locationCodeFrom, string locationCodeTo, string storageLocationCode, string plant ,string countsheet)
        {
            var itemToRemove = allLocationList;
            //var itemToRemove = allLocationList.ExceptWhere(r => r.SectionCode.Equals(sectionCode));
            allLocationList = itemToRemove.Where(r => r.SectionCode != sectionCode).ToList();

            int localFrom = int.Parse(locationCodeFrom);
            int localTo = int.Parse(locationCodeTo);

            if (string.IsNullOrEmpty(locationCodeTo))
            {
                LocationModel newLocation = new LocationModel
                {
                    LocationCode = locationCodeFrom,
                    SectionCode = sectionCode,
                    StorageLocationCode = storageLocationCode,
                    PlantCode = plant,
                    Countsheet = countsheet
                };

                allLocationList.Add(newLocation);
            }
            else if (string.IsNullOrEmpty(locationCodeFrom))
            {
                LocationModel newLocation = new LocationModel
                {
                    LocationCode = locationCodeTo,
                    SectionCode = sectionCode,
                    StorageLocationCode = storageLocationCode,
                    PlantCode = plant,
                    Countsheet = countsheet
                };

                allLocationList.Add(newLocation);
            }
            else
            {
                int length = 5;
                while (localFrom <= localTo)
                {
                    LocationModel newLocation = new LocationModel
                    {
                        LocationCode = localFrom.ToString("D" + length),
                        SectionCode = sectionCode,
                        StorageLocationCode = storageLocationCode,
                        PlantCode = plant,
                        Countsheet = countsheet
                    };

                    allLocationList.Add(newLocation);
                    localFrom++;
                }
            }
        }

        private List<LocationModel> distributionLocationExceptCurrentRow(DataGridView dtView, int currentRow)
        {
            List<LocationModel> listLocation = new List<LocationModel>();
            List<string> Location = new List<string>();
            try
            {
                int length = 5;
                for (int count = 0; count < dtView.Rows.Count - 1; count++)
                {
                    if (count != currentRow)
                    {
                        string plant = dtView.Rows[count].Cells[plantCol].Value == null ? "" : dtView.Rows[count].Cells[plantCol].Value.ToString();
                        string countsheet = dtView.Rows[count].Cells[countSheetCol].Value == null ? "" : dtView.Rows[count].Cells[countSheetCol].Value.ToString();
                        string StorageLocationCode = dtView.Rows[count].Cells[storageLocationCol].Value == null ? "" : dtView.Rows[count].Cells[storageLocationCol].Value.ToString();
                        string sectionCode = dtView.Rows[count].Cells[sectionCodeCol].Value == null ? "" : dtView.Rows[count].Cells[sectionCodeCol].Value.ToString();
                        string localFrom = dtView.Rows[count].Cells[locationFromCol].Value == null ? "" : dtView.Rows[count].Cells[locationFromCol].Value.ToString();
                        string localTo = dtView.Rows[count].Cells[locationToCol].Value == null ? "" : dtView.Rows[count].Cells[locationToCol].Value.ToString();


                        if (localFrom != "" && localTo != "")
                        {
                            int from = int.Parse(localFrom);
                            int to = int.Parse(localTo);

                            while (from <= to)
                            {
                                LocationModel loc = new LocationModel();
                                loc.LocationCode = from.ToString("D" + length);
                                loc.PlantCode = plant;
                                loc.Countsheet = countsheet;
                                loc.StorageLocationCode = StorageLocationCode;
                                loc.SectionCode = sectionCode;
                                listLocation.Add(loc);
                                Location.Add(from.ToString("D" + length));
                                from++;
                            }
                        }
                        else if (localFrom != "")
                        {
                            LocationModel loc = new LocationModel();
                            loc.LocationCode = localFrom;
                            loc.PlantCode = plant;
                            loc.Countsheet = countsheet;
                            loc.StorageLocationCode = StorageLocationCode;
                            loc.SectionCode = sectionCode;
                            listLocation.Add(loc);
                            Location.Add(localFrom);
                        }
                        else if (localTo != "")
                        {
                            LocationModel loc = new LocationModel();
                            loc.LocationCode = localTo;
                            loc.PlantCode = plant;
                            loc.Countsheet = countsheet;
                            loc.StorageLocationCode = StorageLocationCode;
                            loc.SectionCode = sectionCode;
                            listLocation.Add(loc);
                            Location.Add(localTo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                listLocation = new List<LocationModel>();
            }
            Location.Add("");
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

                    if (string.IsNullOrEmpty(sectionTypeList))
                    {
                        sectionTypeList = sectionTypeListAll;
                    }

                    var listsettingData = settingBLL.GetSettingData();

                    //bool isCreateReportSuccess = localReport.CreateReport(gridViewDataList, sectionTypeList, listsettingData.Branch);

                    bool isCreateReportSuccess = localReport.CreateReport(searchList, sectionTypeList, listsettingData.Branch);

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
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }
        private void GenerateReportBarcode()
        {
            try
            {
                //Loading_Screen.ShowSplashScreen();
                List<LocationModel> locationData = bll.GetSectionLocationBarcode(searchList);

                List<LocationBarcode> barcodeData = bll.GenDataToBarCode(locationData);

                BarCodeReportForm barcodeReport = new BarCodeReportForm();
                bool isCreateReportSuccess = barcodeReport.CreateReport(barcodeData);
                //Loading_Screen.CloseForm();
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
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        //private bool ExportTxtFile(string fileName)
        //{
        //    string fullTitlePath = AppDomain.CurrentDomain.BaseDirectory;
        //    string fullPath = Path.Combine(fullTitlePath, fileName);

        //    try
        //    {
        //        if (System.IO.File.Exists(fullPath))
        //        {
        //            System.IO.File.Delete(fullPath);
        //        }

        //        using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
        //        using (StreamWriter sw = new StreamWriter(fs))
        //        {
        //            //Type fieldsType = typeof(LocationManagementModel);

        //            //PropertyInfo[] props = fieldsType.GetProperties(BindingFlags.Public
        //            //    | BindingFlags.Instance);

        //            //sw.WriteLine(props[countSheetCol].Name + "|" + props[storageLocationCol].Name + "|" + props[sectionCodeCol].Name + "|" + props[sectionNameCol].Name + "|" + props[locationFromCol].Name + "|" + props[locationToCol].Name);
        //            //sw.WriteLine("Plant" + "|" + "StorageLocation" + "|" + "SectionCode" + "|" + "SectionName" + "|" + "LocationFrom" + "|" + "LocationTo");
        //            sw.WriteLine("StorageLocation" + "|" + "SectionCode" + "|" + "SectionName" + "|" + "LocationFrom" + "|" + "LocationTo");

        //            foreach (var list in gridViewDataList)
        //            {
        //                sw.WriteLine(list.StorageLocationCode + "|" + list.SectionCode + "|" + list.SectionName + "|" + list.LocationFrom + "|" + list.LocationTo);
        //            }
        //        }

        //        GC.Collect();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
        //        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
        //        return false;
        //    }
        //}

        private bool ExportLocationFile(string fileName)
        {
            string fullTitlePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(fullTitlePath, fileName);

            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                string header = "";

                if (dataGridViewResult.Columns[sectionCodeCol].HeaderText == "Brand Code")
                {
                    header = ("Plant" + "|" + "CountSheet" + "|" + "StorageLocation" + "|" + "BrandCode" + "|" + "BrandName" + "|" + "LocationFrom" + "|" + "LocationTo");
                }
                else
                {
                    header = ("Plant" + "|" + "CountSheet" + "|" + "StorageLocation" + "|" + "SectionCode" + "|" + "SectionName" + "|" + "LocationFrom" + "|" + "LocationTo");
                }

                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(header);
                    foreach (var list in gridViewDataList)
                    {
                        sw.WriteLine(list.PlantCode + "|" + list.CountSheet + "|" + list.StorageLocationCode + "|" + list.SectionCode + "|" + list.SectionName + "|" + list.LocationFrom + "|" + list.LocationTo);
                    }
                }
                GC.Collect();
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        private bool ExportTemplateFile(string fileName)
        {
            string fullTitlePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(fullTitlePath, fileName);

            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                string header = "";

                if (dataGridViewResult.Columns[sectionCodeCol].HeaderText == "Brand Code")
                {
                    header = ("StorageLocation" + "|" + "BrandCode" + "|" + "BrandName" + "|" + "LocationFrom" + "|" + "LocationTo");
                }
                else 
                {
                    header = ("StorageLocation" + "|" + "SectionCode" + "|" + "SectionName" + "|" + "LocationFrom" + "|" + "LocationTo");
                }

                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(header);

                    foreach (var list in gridViewDataList)
                    {
                        sw.WriteLine(list.StorageLocationCode + "|" + list.SectionCode + "|" + list.SectionName + "|" + list.LocationFrom + "|" + list.LocationTo);
                    }
                }
                GC.Collect();
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        private string saveDialog()
        {
            string resultDialog = "";
            try
            {
                //string fileName = "tmpTxtFileExport.txt";
                var dialog = new SaveFileDialog();
                dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                var result = dialog.ShowDialog(); //shows save file dialog
                if (result == DialogResult.OK)
                {
                    //if (dialog.FileName.ToUpper().Contains("SECTION-") || dialog.FileName.ToUpper().Contains("BRAND-"))
                    //{
                        if (ExportLocationFile(dialog.FileName))
                        {
                            resultDialog = "OK";
                            logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Save Location Success", DateTime.Now);
                        }
                        else
                        {
                            resultDialog = "EXPORTERROR";
                            logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "EXPORTERROR", DateTime.Now);
                        }
                    //}
                    //else
                    //{
                    //    resultDialog = "FILENAMEERROR";
                    //}
                }
                else
                {
                    resultDialog = "CANCEL";
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "CANCEL", DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                resultDialog = "ERROR";
            }
            return resultDialog;
        }

        private string saveTemplateDialog()
        {
            string resultDialog = "";
            try
            {
                //string fileName = "tmpTxtFileExport.txt";
                var dialog = new SaveFileDialog();
                dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                var result = dialog.ShowDialog(); //shows save file dialog
                if (result == DialogResult.OK)
                {
                    if (dialog.FileName.ToUpper().Contains("SECTION-") || dialog.FileName.ToUpper().Contains("BRAND-"))
                    {
                        if (ExportTemplateFile(dialog.FileName))
                        {
                            resultDialog = "OK";
                            logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Save Template Success", DateTime.Now);

                            //var wClient = new WebClient();
                            //wClient.DownloadFile(fileName, dialog.FileName);
                            //resultDialog = "OK";
                            //if (System.IO.File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)))
                            //{
                            //    System.IO.File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
                            //}
                        }
                        else
                        {
                            resultDialog = "ERROR";
                        }
                    }
                    //if (ExportTemplateFile(dialog.FileName))
                    //{
                    //    resultDialog = "OK";
                    //    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Save Template Success", DateTime.Now);
                    //    //var wClient = new WebClient();
                    //    //wClient.DownloadFile(fileName, dialog.FileName);
                    //    //resultDialog = "OK";
                    //    //if (System.IO.File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)))
                    //    //{
                    //    //    System.IO.File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
                    //    //}
                    //}
                    else
                    {
                        resultDialog = "FILENAMEERROR";
                    }
                }
                else
                {
                    resultDialog = "CANCEL";
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                resultDialog = "ERROR";
            }
            return resultDialog;
        }

        private string saveTemplate()
        {
            string resultDialog = "";
            try
            {
                //string fileName = "tmpTxtFileExport.txt";
                var dialog = new SaveFileDialog();
                dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                var result = dialog.ShowDialog(); //shows save file dialog
                if (result == DialogResult.OK)
                {
                    if (ExportLocationFile(dialog.FileName))
                    {
                        //var wClient = new WebClient();
                        //wClient.DownloadFile(fileName, dialog.FileName);
                        resultDialog = "OK";
                        //if (System.IO.File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)))
                        //{
                        //    System.IO.File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
                        //}
                        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Save Success" , DateTime.Now);
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
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                resultDialog = "ERROR";
            }
            return resultDialog;
        }

        private bool loadBrand(bool clear)
        {        
            try
            {
                //AddDropDownPlantGrid();
                AddDropDownCountSheetGrid();
                List<LocationManagementModel> listBrand = bll.LoadBrand();

                if (clear)                 
                    dataGridViewResult.Rows.Clear();

                if (listBrand.Count > 0)
                {
                    List<DuplicateKeyLocationModel> listDuplicateKeyInDB = new List<DuplicateKeyLocationModel>();
                    {
                        int rowcount = dataGridViewResult.Rows.Count;

                        for (int i = 0; i < rowcount-1; i++ )
                        {
                            DuplicateKeyLocationModel dup = new DuplicateKeyLocationModel
                            {
                                PlantCode = dataGridViewResult.Rows[i].Cells[plantCol].Value.ToString(),
                                CountSheet = dataGridViewResult.Rows[i].Cells[countSheetCol].Value.ToString(),
                                StorageLocationCode = dataGridViewResult.Rows[i].Cells[storageLocationCol].Value.ToString(),
                                SectionCode = dataGridViewResult.Rows[i].Cells[sectionCodeCol].Value.ToString(),
                                SectionName = dataGridViewResult.Rows[i].Cells[sectionNameCol].Value.ToString()
                            };
                            listDuplicateKeyInDB.Add(dup);
                        }
                    }
                    
                    dataGridViewResult.Columns[sectionCodeCol].HeaderText = "Brand Code";
                    dataGridViewResult.Columns[sectionNameCol].HeaderText = "Brand Name";
                    //IsLoadBrand = true;
                    //UpdateStorageLocationType();

                    foreach (LocationManagementModel list in listBrand)
                    {
                        if (!listDuplicateKeyInDB.Exists(x => x.PlantCode == list.PlantCode && x.CountSheet == list.CountSheet && x.StorageLocationCode == list.StorageLocationCode && x.SectionCode == list.SectionCode))
                        {
                            DataGridViewRow row = (DataGridViewRow)dataGridViewResult.Rows[0].Clone();
                            row.Cells[plantCol].Value = (string)list.PlantCode;
                            row.Cells[countSheetCol].Value = (string)list.CountSheet;
                            row.Cells[storageLocationCol].Value = (string)list.StorageLocationCode;
                            row.Cells[sectionCodeCol].Value = (string)list.SectionCode;
                            row.Cells[sectionNameCol].Value = (string)list.SectionName;
                            row.Cells[locationFromCol].Value = (string)list.LocationFrom;
                            row.Cells[locationToCol].Value = (string)list.LocationTo;                           
                            row.Cells[brandCodeCol].Value = (string)list.BrandCode;
                            row.Cells[flagCol].Value = "I";

                            dataGridViewResult.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(MessageConstants.NoDatafound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        private string ImportSection(ref List<LocationManagementModel> listDuplicateKeyInDB, ref string storageType, string type)
        {
            string result = "";
            try
            {
                var FD = new OpenFileDialog();
                if (type == "Location")
                {
                    FD.Filter = "txt files (*.txt)|*.txt;";
                }
                else
                {
                    FD.Filter = "txt files (*.txt)|Brand-*.txt;Section-*.txt;";
                }

                FD.FilterIndex = 2;
                FD.RestoreDirectory = true;
                FD.Multiselect = false;

                if (FD.ShowDialog() == DialogResult.OK)
                {
                    Loading_Screen.ShowSplashScreen();
                    string fileToOpen = FD.FileName;
                    //fileName = FD.SafeFileName;
                    System.IO.StreamReader reader = new System.IO.StreamReader(fileToOpen);
                    DataTable dtSuccess = new DataTable();
                    DataTable dtError = new DataTable();

                    try
                    {                   
                        using (StreamReader sr = reader)
                        {
                            string line;
                            int i = 0;
                            string[] words;
                            #region verify data
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
                                            if (words[x].Trim().ToUpper().Equals(headerImportSection[x].Trim().ToUpper()))
                                            {
                                                dtSuccess.Columns.Add(words[x]);
                                            }
                                            else if (words[x].Trim().ToUpper().Equals(headerImportBrand[x].Trim().ToUpper()))
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

                                        if (words[1].Trim().ToUpper() == "SECTIONCODE")
                                        {
                                            storageType = "SECTION";
                                        }
                                        else 
                                        {
                                            storageType = "BRAND";
                                        }

                                        dtSuccess.Columns.Add("Line");
                                    }
                                    else if (words.Length == headerImportLocation.Length)
                                    {
                                        for (int x = 0; x < words.Length; x++)
                                        {
                                            if (words[x].Trim().ToUpper().Equals(headerImportLocation[x].Trim().ToUpper()))
                                            {
                                                dtSuccess.Columns.Add(words[x]);
                                            }
                                            else if (words[x].Trim().ToUpper().Equals(headerImportLocationBrand[x].Trim().ToUpper()))
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
                                        if (words[3].Trim().ToUpper() == "SECTIONCODE")
                                        {
                                            storageType = "SECTION";
                                        }
                                        else
                                        {
                                            storageType = "BRAND";
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
                                            int idxStorageLocation = 0; //0 = StorageLocation
                                            int idxSectionCode = 1; //1 = SectionCode
                                            int idxLocationFrom = 3; //3 = LocationFrom
                                            int idxLocationTo = 4; //4 = LocationTo

                                            if (type == "Location")
                                            {
                                                idxStorageLocation = 2; //0 = StorageLocation
                                                idxSectionCode = 3; //1 = SectionCode
                                                idxLocationFrom = 5; //3 = LocationFrom
                                                idxLocationTo = 6; //4 = LocationTo
                                            }

                                            if (x == idxStorageLocation || x == idxSectionCode || x == idxLocationFrom || x == idxLocationTo)
                                            {
                                                int num;
                                                bool res = int.TryParse(words[x], out num);
                                                if (res == false)
                                                {
                                                    //if (x == idxStorageLocation)
                                                    //{
                                                    //    DataRow dtRowError = dtError.NewRow();
                                                    //    dtRowError[0] = (i + 1);
                                                    //    dtRowError[1] = "Storage Location is number only.";
                                                    //    dtError.Rows.Add(dtRowError);
                                                    //}
                                                    //else if (x == idxSectionCode)
                                                    //{
                                                    //    DataRow dtRowError = dtError.NewRow();
                                                    //    dtRowError[0] = (i + 1);
                                                    //    dtRowError[1] = "Section Code is number only.";
                                                    //    dtError.Rows.Add(dtRowError);
                                                    //}
                                                    //else 
                                                    if (x == idxLocationFrom)
                                                    {
                                                        DataRow dtRowError = dtError.NewRow();
                                                        dtRowError[0] = (i + 1);
                                                        dtRowError[1] = "Location From is number only.";
                                                        dtError.Rows.Add(dtRowError);
                                                    }
                                                    else if (x == idxLocationTo)
                                                    {
                                                        DataRow dtRowError = dtError.NewRow();
                                                        dtRowError[0] = (i + 1);
                                                        dtRowError[1] = "Location To is number only.";
                                                        dtError.Rows.Add(dtRowError);
                                                    }
                                                    else
                                                    {
                                                        dtRowSuccess[x] = words[x];
                                                    }
                                                }
                                                else if (words[x].Length != 4)
                                                {
                                                    if (x == idxStorageLocation)
                                                    {
                                                        DataRow dtRowError = dtError.NewRow();
                                                        dtRowError[0] = (i + 1);
                                                        dtRowError[1] = "Storage Location must be 4 digits.";
                                                        dtError.Rows.Add(dtRowError);
                                                    }
                                                    else
                                                    {
                                                        dtRowSuccess[x] = words[x];
                                                    }
                                                }
                                                else if (words[x].Length != 5)
                                                {
                                                    //if (x == idxSectionCode)
                                                    //{
                                                    //    DataRow dtRowError = dtError.NewRow();
                                                    //    dtRowError[0] = (i + 1);
                                                    //    dtRowError[1] = "Section Code must be 5 digits.";
                                                    //    dtError.Rows.Add(dtRowError);
                                                    //}
                                                    //else 
                                                    if (x == idxLocationFrom)
                                                    {
                                                        DataRow dtRowError = dtError.NewRow();
                                                        dtRowError[0] = (i + 1);
                                                        dtRowError[1] = "Location From must be 5 digits.";
                                                        dtError.Rows.Add(dtRowError);
                                                    }
                                                    else if (x == idxLocationTo)
                                                    {
                                                        DataRow dtRowError = dtError.NewRow();
                                                        dtRowError[0] = (i + 1);
                                                        dtRowError[1] = "Location To must be 5 digits.";
                                                        dtError.Rows.Add(dtRowError);
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
                                            }
                                            else
                                            {
                                                dtRowSuccess[x] = words[x];
                                            }
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
                            #endregion

                            //check duplicae key in files
                            Hashtable hasResult = new Hashtable();
                            //hasResult = checkDuplicateKeyInFiles(dtSuccess, dtError, storageType);
                            if (type != "Template")
                            {
                                hasResult = checkDuplicateKeyInFiles(dtSuccess, dtError, storageType);
                            }
                            else
                            {
                                hasResult.Add("tableError", dtError);
                                if (dtError.Rows.Count > 0)
                                {
                                    hasResult.Add("result", false);
                                }
                                else
                                {
                                    hasResult.Add("result", true);
                                }
                                
                            }
                            bool resultCheckFile = (bool)hasResult["result"];
                            dtError = (DataTable)hasResult["tableError"];
                            listDuplicateKeyInDB = new List<LocationManagementModel>();

                            if (dtError.Rows.Count <= 0)
                            {
                                #region
                                ////DataTable To list
                                //insertList = (from d in dtSuccess.AsEnumerable()
                                //              select new LocationManagementModel
                                //              {
                                //                  PlantCode = d.Field<string>("Plant"),
                                //                  OriginalSectionCode = d.Field<string>("SectionCode"),
                                //                  SectionCode = d.Field<string>("SectionCode"),
                                //                  StorageLocationCode = d.Field<string>("StorageLocation"),
                                //                  SectionName = d.Field<string>("SectionName"),
                                //                  LocationFrom = d.Field<string>("LocationFrom"),
                                //                  LocationTo = d.Field<string>("LocationTo")
                                //              }).ToList();

                                //List<LocationManagementModel> listSection = bll.GetAllSection();

                                //if (listSection.Count > 0)
                                //{
                                //    //Delete All Section Code not check duplicate key
                                //    DialogResult dialogResult = MessageBox.Show(MessageConstants.Oldsectiondatawillbedeletedafterthisaction, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                //    if (dialogResult == DialogResult.Yes)
                                //    {
                                //        bll.ClearAllSection();
                                //        if (Save(insertList, updateList, deleteList))
                                //        {
                                //            result = "OK";
                                //        }
                                //        else
                                //        {
                                //            result = "ERROR";
                                //        }
                                //    }
                                //    else
                                //    {
                                //        result = "CANCEL";
                                //    }
                                //}
                                //else
                                //{
                                //    bll.ClearAllSection();
                                //    if (Save(insertList, updateList, deleteList))
                                //    {
                                //        result = "OK";
                                //    }
                                //    else
                                //    {
                                //        result = "ERROR";
                                //    }
                                //}
                                #endregion

                                //allLocationList = bll.GetAllLocation();
                                List<LocationManagementModel> listSuccess = new List<LocationManagementModel>();
                                if (type == "Template" && storageType == "SECTION")
                                {
                                    IsLoadBrand = false;
                                    listSuccess = (from d in dtSuccess.AsEnumerable()
                                                   select new LocationManagementModel
                                                   {
                                                       SectionCode = d.Field<string>("SectionCode"),
                                                       StorageLocationCode = d.Field<string>("StorageLocation"),
                                                       SectionName = d.Field<string>("SectionName"),
                                                       LocationFrom = d.Field<string>("LocationFrom"),
                                                       LocationTo = d.Field<string>("LocationTo")
                                                   }).ToList();
                                }
                                else if (type == "Template" && storageType == "BRAND")
                                {
                                    IsLoadBrand = true;
                                    listSuccess = (from d in dtSuccess.AsEnumerable()
                                                   select new LocationManagementModel
                                                   {
                                                       SectionCode = d.Field<string>("BrandCode"),
                                                       StorageLocationCode = d.Field<string>("StorageLocation"),
                                                       SectionName = d.Field<string>("BrandName"),
                                                       LocationFrom = d.Field<string>("LocationFrom"),
                                                       LocationTo = d.Field<string>("LocationTo"),
                                                       BrandCode = d.Field<string>("BrandCode"),
                                                   }).ToList();
                                }
                                else if (type == "Location" && storageType == "SECTION")
                                {
                                    IsLoadBrand = false;
                                    listSuccess = (from d in dtSuccess.AsEnumerable()
                                                   select new LocationManagementModel
                                                   {
                                                       PlantCode = d.Field<string>("Plant"),
                                                       CountSheet = d.Field<string>("CountSheet"),
                                                       SectionCode = d.Field<string>("SectionCode"),
                                                       StorageLocationCode = d.Field<string>("StorageLocation"),
                                                       SectionName = d.Field<string>("SectionName"),
                                                       LocationFrom = d.Field<string>("LocationFrom"),
                                                       LocationTo = d.Field<string>("LocationTo")
                                                   }).ToList();
                                }
                                else if (type == "Location" && storageType == "BRAND")
                                {
                                    IsLoadBrand = true;
                                    listSuccess = (from d in dtSuccess.AsEnumerable()
                                                   select new LocationManagementModel
                                                   {
                                                       PlantCode = d.Field<string>("Plant"),
                                                       CountSheet = d.Field<string>("CountSheet"),
                                                       StorageLocationCode = d.Field<string>("StorageLocation"),
                                                       SectionCode = d.Field<string>("BrandCode"),
                                                       SectionName = d.Field<string>("BrandName"),
                                                       LocationFrom = d.Field<string>("LocationFrom"),
                                                       LocationTo = d.Field<string>("LocationTo"),
                                                       BrandCode = d.Field<string>("BrandCode"),
                                                   }).ToList();
                                }

                                bool rightPlant = false;
                                bool rightCountsheet = false;

                                if (type == "Template")
                                {
                                    rightPlant = true;
                                    rightCountsheet = true;
                                }
                                else
                                {
                                    rightPlant = IsHavePlant(listSuccess);
                                    rightCountsheet = IsHaveCountsheet(listSuccess);
                                }

                                if (rightPlant && rightCountsheet)
                                {
                                    List<LocationManagementModel> gridSection = new List<LocationManagementModel>();

                                    for (int count = 0; count < dataGridViewResult.Rows.Count - 1; count++)
                                    {
                                        DataGridViewRow dr = dataGridViewResult.Rows[count];
                                        LocationManagementModel item = new LocationManagementModel();

                                        item.PlantCode           = dr.Cells[plantCol].Value == null ? "" : dr.Cells[plantCol].Value.ToString();
                                        item.CountSheet          = dr.Cells[countSheetCol].Value == null ? "" : dr.Cells[countSheetCol].Value.ToString();
                                        item.SectionCode         = dr.Cells[sectionCodeCol].Value == null ? "" : dr.Cells[sectionCodeCol].Value.ToString();
                                        item.SectionName         = dr.Cells[sectionNameCol].Value == null ? "" : dr.Cells[sectionNameCol].Value.ToString();
                                        item.StorageLocationCode = dr.Cells[storageLocationCol].Value == null ? "" : dr.Cells[storageLocationCol].Value.ToString();
                                        item.LocationFrom        = dr.Cells[locationFromCol].Value == null ? "" : dr.Cells[locationFromCol].Value.ToString();
                                        item.LocationTo          = dr.Cells[locationToCol].Value == null ? "" : dr.Cells[locationToCol].Value.ToString();
                                        item.FlagAction          = dr.Cells[flagCol].Value == null ? "" : dr.Cells[flagCol].Value.ToString();
                                        item.BrandCode           = dr.Cells[brandCodeCol].Value == null ? "" : dr.Cells[brandCodeCol].Value.ToString();

                                        item.OriginalPlantCode       = dr.Cells[plantCol].Value == null ? "" : dr.Cells[plantCol].Value.ToString();
                                        item.OriginalCountSheet      = dr.Cells[countSheetCol].Value == null ? "" : dr.Cells[countSheetCol].Value.ToString() ;
                                        item.OriginalStorageLocCode  = dr.Cells[storageLocationCol].Value == null ? "" : dr.Cells[storageLocationCol].Value.ToString();
                                        item.OriginalSectionCode     = dr.Cells[sectionCodeCol].Value == null ? "" : dr.Cells[sectionCodeCol].Value.ToString();
                                        item.OriginalSectionName     = dr.Cells[sectionNameCol].Value == null ? "" : dr.Cells[sectionNameCol].Value.ToString();

                                        gridSection.Add(item);
                                    }

                                    foreach (LocationManagementModel section in listSuccess)
                                    {
                                        bool isExist = false;
                                        var countExistLocation = (from l in gridSection
                                                                  where l.SectionCode == section.SectionCode
                                                                  && l.StorageLocationCode == section.StorageLocationCode
                                                                  && l.PlantCode == section.PlantCode
                                                                  && l.CountSheet == section.CountSheet
                                                                  select l.SectionCode).ToList().Count();

                                        if (countExistLocation > 0)
                                        {
                                            isExist = true;
                                        }

                                        if (!isExist) //data ซ้ำ
                                        {
                                            DataGridViewRow row = (DataGridViewRow)dataGridViewResult.Rows[0].Clone();

                                            row.Cells[plantCol].Value = (string)section.PlantCode;
                                            row.Cells[countSheetCol].Value = (string)section.CountSheet;
                                            row.Cells[sectionCodeCol].Value = (string)section.SectionCode;
                                            row.Cells[sectionNameCol].Value = (string)section.SectionName;
                                            row.Cells[storageLocationCol].Value = (string)section.StorageLocationCode;
                                            row.Cells[locationFromCol].Value = (string)section.LocationFrom;
                                            row.Cells[locationToCol].Value = (string)section.LocationTo;
                                            row.Cells[flagCol].Value = (string)section.FlagAction;
                                            row.Cells[brandCodeCol].Value = (string)section.BrandCode;

                                            row.Cells[originalPlantCol].Value = (string)section.OriginalPlantCode;
                                            row.Cells[originalCountSheetCol].Value = (string)section.OriginalCountSheet;
                                            row.Cells[originalStorageCol].Value = (string)section.OriginalStorageLocCode;
                                            row.Cells[originalSectionCol].Value = (string)section.OriginalSectionCode;
                                            row.Cells[originalSectionNameCol].Value = (string)section.OriginalSectionName;

                                            dataGridViewResult.Rows.Add(row);
                                        }
                                        else
                                        {
                                            LocationManagementModel item = new LocationManagementModel();

                                            item.PlantCode           = section.PlantCode;
                                            item.CountSheet          = section.CountSheet;
                                            item.SectionCode         = section.SectionCode;
                                            item.SectionName         = section.SectionName;
                                            item.StorageLocationCode = section.StorageLocationCode;

                                            listDuplicateKeyInDB.Add(item);
                                        }
                                    }

                                    result = "OK";
                                }
                                else if (!rightPlant)
                                {
                                    result = "Plant in text file is mismatch with Master Data";
                                }
                                else
                                {
                                    result = "Countsheet in text file is mismatch with Master Data";
                                }
                            }
                            else
                            {
                                result = "";

                                foreach (DataRow dr in dtError.Rows)
                                {
                                    result += "Line " + dr[0].ToString() + " : " + dr[1].ToString() + " \n";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                        result = "File ERROR";
                    }
                }
                else
                {
                    result = "CANCEL";
                }
                Loading_Screen.CloseForm();

            }
            catch(Exception ex )
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                result = "File ERROR";
            }
            return result;
        }

        private Hashtable checkDuplicateKeyInFiles(DataTable dtSuccess, DataTable dtError, string storageType)
        {
            Hashtable hastResult = new Hashtable();
            bool result = true;
            List<LocationModel> listLocation = new List<LocationModel>();

            foreach (DataRow dr in dtSuccess.Rows)
            {
                LocationModel loc = new LocationModel();
                int start = int.Parse(dr["LocationFrom"].ToString());
                int end = int.Parse(dr["LocationTo"].ToString());
                string sectionCode = "";
                if (storageType == "SECTION")
                {
                    sectionCode = dr["SectionCode"].ToString();
                }
                else if (storageType == "BRAND")
                {
                    sectionCode = dr["BrandCode"].ToString();
                }
                string storageLocation = dr["StorageLocation"].ToString();

                int length = 5;

                for (int i = start; i <= end; i++)
                {
                    loc.LocationCode = i.ToString("D" + length);
                    loc.SectionCode = sectionCode;
                    loc.StorageLocationCode = storageLocation;
                    listLocation.Add(loc);
                }
            }

            foreach (DataRow dr in dtSuccess.Rows)
            {
                bool isduplicate = false;
                int countExists = 0;
                string sectionCode = "";
                string storageLocation = dr["StorageLocation"].ToString();
                string plant = dr["Plant"].ToString();
                string countsheet = dr["CountSheet"].ToString();
                
                if (storageType == "SECTION")
                {
                    sectionCode = dr["SectionCode"].ToString();
                    countExists = (from d in dtSuccess.AsEnumerable()
                                   where d.Field<string>("SectionCode").Equals(sectionCode)
                                   && d.Field<string>("StorageLocation").Equals(storageLocation)
                                   && d.Field<string>("Plant").Equals(plant)
                                   && d.Field<string>("CountSheet").Equals(countsheet)
                                   select d).ToList().Count();
                }
                else if (storageType == "BRAND")
                {
                    sectionCode = dr["BrandCode"].ToString();
                    countExists = (from d in dtSuccess.AsEnumerable()
                                   where d.Field<string>("BrandCode").Equals(sectionCode)
                                   && d.Field<string>("StorageLocation").Equals(storageLocation)
                                   && d.Field<string>("Plant").Equals(plant)
                                   && d.Field<string>("CountSheet").Equals(countsheet)
                                   select d).ToList().Count();
                }

                if (countExists == 1)
                {
                    int start = int.Parse(dr["LocationFrom"].ToString());
                    int end = int.Parse(dr["LocationTo"].ToString());
                    int length = 5;

                    for (int i = start; i <= end; i++)
                    {
                        string locationCurrent = i.ToString("D" + length);
                        int countLocation = (from l in listLocation
                                             where l.Equals(locationCurrent)
                                             //&& l.StorageLocationCode.Equals(storageLocation)
                                             //&& l.SectionCode.Equals(sectionCode)
                                             //&& l.PlantCode.Equals(PlantCode)
                                             //&& l.Countsheet.Equals(CountSheet)
                                             select l).ToList().Count();

                        if (countLocation > 0)
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
                    if (storageType == "SECTION")
                    {
                        dtRowError[1] = "Location " + dr["LocationFrom"] + " - " + dr["LocationTo"] + " Section : " + dr["SectionCode"] + " of " + dr["StorageLocation"] + " is Duplicate.";
                    }
                    else if (storageType == "BRAND")
                    {
                        dtRowError[1] = "Location " + dr["LocationFrom"] + " - " + dr["LocationTo"] + " Brand : " + dr["BrandCode"] + " of " + dr["StorageLocation"] + " is Duplicate.";
                    }
                    if (dtRowError[0].ToString() != string.Empty) { dtError.Rows.Add(dtRowError); }
                    result = false;
                }
            }

            hastResult.Add("tableError", dtError);
            hastResult.Add("result", result);

            return hastResult;
        }
       
        private LocationManagementModel GetSearchCondition()
        {
            LocationManagementModel searchSection = new LocationManagementModel();
            searchSection.LocationFrom = textBoxLoForm.Text;
            searchSection.LocationTo = textBoxLoTo.Text;
            searchSection.PlantCode = comboBoxPlant.SelectedText;
            searchSection.CountSheet = comboBoxCountSheet.SelectedText;
            searchSection.StorageLocationCode = comboBoxStorageLocal.Text;

            //string sectionTypeList = "";
            //foreach (CheckBox sectionType in groupLocationType.Controls)
            //{
            //    if (sectionType.Checked == true)
            //    {
            //        sectionTypeList = sectionTypeList + (sectionTypeList == "" ? "" : "|") + (string)sectionType.Text;
            //    }
            //}

            //searchSection.SectionType = sectionTypeList;

            return searchSection;
        }

        private void SetHeader(DataGridView DGV)
        {
            //bool isBrand = true;
            //foreach (DataGridViewRow row in DGV.Rows)
            //{
            //    if (string.IsNullOrEmpty(row.Cells[brandCodeCol].Value.ToString()))
            //    {
            //        isBrand = false;
            //        break;
            //    }
            //}
            //StorageLocationType = settingBLL.GetSettingStringByKey("StorageLocationType");
            if (StorageLocationType == "BRAND")
            {
                dataGridViewResult.Columns[sectionCodeCol].HeaderText = "Brand Code";
                dataGridViewResult.Columns[sectionNameCol].HeaderText = "Brand Name";
            }
            else
            {
                dataGridViewResult.Columns[sectionCodeCol].HeaderText = "Section Code";
                dataGridViewResult.Columns[sectionNameCol].HeaderText = "Section Name";
            }
        }

        #endregion

        #region Grid

        // get current data in gridview
        private List<LocationManagementModel> GetCurrentDataGridView()
        {
            List<LocationManagementModel> gridViewDataList = new List<LocationManagementModel>();
            gridViewDataList.Clear();
            try
            {
                for (int count = 0; count < dataGridViewResult.Rows.Count - 1; count++)
                {
                    LocationManagementModel gridViewData = new LocationManagementModel();

                    gridViewData.PlantCode = (string)dataGridViewResult.Rows[count].Cells[plantCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[plantCol].Value).Trim();
                    gridViewData.CountSheet = (string)dataGridViewResult.Rows[count].Cells[countSheetCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[countSheetCol].Value).Trim();
                    gridViewData.SectionCode = (string)dataGridViewResult.Rows[count].Cells[sectionCodeCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[sectionCodeCol].Value).Trim();
                    gridViewData.StorageLocationCode = (string)dataGridViewResult.Rows[count].Cells[storageLocationCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[storageLocationCol].Value).Trim();
                    gridViewData.SectionName = (string)dataGridViewResult.Rows[count].Cells[sectionNameCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[sectionNameCol].Value).Trim();
                    gridViewData.LocationFrom = (string)dataGridViewResult.Rows[count].Cells[locationFromCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[locationFromCol].Value).Trim();
                    gridViewData.LocationTo = (string)dataGridViewResult.Rows[count].Cells[locationToCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[locationToCol].Value).Trim();
                    gridViewData.FlagAction = (string)dataGridViewResult.Rows[count].Cells[flagCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[flagCol].Value).Trim();
                    gridViewData.BrandCode = (string)dataGridViewResult.Rows[count].Cells[brandCodeCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[brandCodeCol].Value).Trim();

                    gridViewData.OriginalPlantCode = (string)dataGridViewResult.Rows[count].Cells[originalPlantCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[originalPlantCol].Value).Trim();
                    gridViewData.OriginalCountSheet = (string)dataGridViewResult.Rows[count].Cells[originalCountSheetCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[originalCountSheetCol].Value).Trim();
                    gridViewData.OriginalStorageLocCode = (string)dataGridViewResult.Rows[count].Cells[originalStorageCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[originalStorageCol].Value).Trim();
                    gridViewData.OriginalSectionCode = (string)dataGridViewResult.Rows[count].Cells[originalSectionCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[originalSectionCol].Value).Trim();
                    gridViewData.OriginalSectionName = (string)dataGridViewResult.Rows[count].Cells[originalSectionNameCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[originalSectionNameCol].Value).Trim();

                    gridViewDataList.Add(gridViewData);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                gridViewDataList = null;
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
                    
                    string plantCode = (string)dataGridViewResult.Rows[count].Cells[plantCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[plantCol].Value).Trim();
                    string countsheet = (string)dataGridViewResult.Rows[count].Cells[countSheetCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[countSheetCol].Value).Trim();
                    string storageLocationCode = (string)dataGridViewResult.Rows[count].Cells[storageLocationCol].Value;
                    string sectionCode = (string)dataGridViewResult.Rows[count].Cells[sectionCodeCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[sectionCodeCol].Value).Trim();
                    string sectionName = (string)dataGridViewResult.Rows[count].Cells[sectionNameCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[sectionNameCol].Value).Trim();

                    string originalPlantCode = (string)dataGridViewResult.Rows[count].Cells[originalPlantCol].Value;
                    string originalCountsheet = (string)dataGridViewResult.Rows[count].Cells[originalCountSheetCol].Value;
                    string originalStorageCode = (string)dataGridViewResult.Rows[count].Cells[originalStorageCol].Value;
                    string originalSectionCode = (string)dataGridViewResult.Rows[count].Cells[originalSectionCol].Value;
                    string originalSectionName = (string)dataGridViewResult.Rows[count].Cells[originalSectionNameCol].Value;


                    //case insert new row set originalsectioncode equal sectioncode
                    /*if (originalSectionCode == null)
                    {
                        gridViewData.OriginalSectionCode = sectionCode;
                        dataGridViewResult.Rows[count].Cells[originalSectionCol].Value = sectionCode;
                    }
                    else
                    {
                        // case edit section code
                        if (originalSectionCode != sectionCode)
                        {
                            //LocationModel deleteSection = new LocationModel
                            //{
                            //    PlantCode = originalPlantCode,
                            //    Countsheet = originalCountsheet,
                            //    StorageLocationCode = originalStorageCode,
                            //    SectionCode = originalSectionCode,
                            //    SectionName = originalSectionName
                            //};

                            //deleteList.Add(deleteSection);

                            gridViewData.OriginalSectionCode = sectionCode;
                            dataGridViewResult.Rows[count].Cells[originalSectionCol].Value = sectionCode;
                        }
                        else
                        {
                            gridViewData.OriginalSectionCode = originalSectionCode;
                        }
                    }

                    if (originalStorageCode == null)
                    {
                        gridViewData.OriginalStorageLocCode = storageLocationCode;
                        dataGridViewResult.Rows[count].Cells[originalStorageCol].Value = storageLocationCode;
                    }
                    else
                    {
                        // case edit section code
                        if (originalStorageCode != storageLocationCode)
                        {
                            //LocationModel deleteSection = new LocationModel
                            //{
                            //    PlantCode = originalPlantCode,
                            //    Countsheet = originalCountsheet,
                            //    StorageLocationCode = originalStorageCode,
                            //    SectionCode = originalSectionCode,
                            //    SectionName = originalSectionName
                            //};

                            //deleteList.Add(deleteSection);

                            gridViewData.OriginalStorageLocCode = storageLocationCode;
                            dataGridViewResult.Rows[count].Cells[originalStorageCol].Value = storageLocationCode;
                        }
                        else
                        {
                            //gridViewData.OriginSectionType = originalSectionType;
                        }
                    }

                    if (originalPlantCode == null)
                    {
                        gridViewData.OriginalPlantCode = plantCode;
                        dataGridViewResult.Rows[count].Cells[originalPlantCol].Value = plantCode;
                    }
                    else
                    {
                        if (originalPlantCode != plantCode)
                        {
                            //LocationModel deleteSection = new LocationModel
                            //{
                            //    PlantCode = originalPlantCode,
                            //    Countsheet = originalCountsheet,
                            //    StorageLocationCode = originalStorageCode,
                            //    SectionCode = originalSectionCode,
                            //    SectionName = originalSectionName
                            //};

                            //deleteList.Add(deleteSection);

                            gridViewData.OriginalPlantCode = plantCode;
                            dataGridViewResult.Rows[count].Cells[originalPlantCol].Value = plantCode;
                        }
                        else
                        {
                            //gridViewData.OriginSectionType = originalSectionType;
                        }
                    }

                    if (originalCountsheet == null)
                    {
                        gridViewData.OriginalCountSheet = countsheet;
                        dataGridViewResult.Rows[count].Cells[originalCountSheetCol].Value = countsheet;
                    }
                    else
                    {
                        if (originalCountsheet != countsheet)
                        {
                            //LocationModel deleteSection = new LocationModel
                            //{
                            //    PlantCode = originalPlantCode,
                            //    Countsheet = originalCountsheet,
                            //    StorageLocationCode = originalStorageCode,
                            //    SectionCode = originalSectionCode,
                            //    SectionName = originalSectionName
                            //};

                            //deleteList.Add(deleteSection);

                            gridViewData.OriginalCountSheet = countsheet;
                            dataGridViewResult.Rows[count].Cells[originalCountSheetCol].Value = countsheet;
                        }
                        else
                        {
                            //gridViewData.OriginSectionType = originalSectionType;
                        }
                    }

                    if (originalSectionName == null)
                    {
                        gridViewData.OriginalCountSheet = sectionName;
                        dataGridViewResult.Rows[count].Cells[originalSectionNameCol].Value = sectionName;
                    }
                    else
                    {
                        if (originalSectionName != sectionName)
                        {
                            //LocationModel deleteSection = new LocationModel
                            //{
                            //    PlantCode = originalPlantCode,
                            //    Countsheet = originalCountsheet,
                            //    StorageLocationCode = originalStorageCode,
                            //    SectionCode = originalSectionCode,
                            //    SectionName = originalSectionName
                            //};

                            //deleteList.Add(deleteSection);

                            gridViewData.OriginalSectionName = sectionName;
                            dataGridViewResult.Rows[count].Cells[originalSectionNameCol].Value = sectionName;
                        }
                        else
                        {
                            //gridViewData.OriginSectionType = originalSectionType;
                        }
                    }*/

                    gridViewData.PlantCode = (string)dataGridViewResult.Rows[count].Cells[plantCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[plantCol].Value).Trim();
                    gridViewData.CountSheet = (string)dataGridViewResult.Rows[count].Cells[countSheetCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[countSheetCol].Value).Trim();
                    gridViewData.StorageLocationCode = (string)dataGridViewResult.Rows[count].Cells[storageLocationCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[storageLocationCol].Value).Trim();
                    gridViewData.SectionCode = sectionCode;
                    gridViewData.SectionName = (string)dataGridViewResult.Rows[count].Cells[sectionNameCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[sectionNameCol].Value).Trim();
                    gridViewData.LocationFrom = (string)dataGridViewResult.Rows[count].Cells[locationFromCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[locationFromCol].Value).Trim();
                    gridViewData.LocationTo = (string)dataGridViewResult.Rows[count].Cells[locationToCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[locationToCol].Value).Trim();
                    gridViewData.FlagAction = (string)dataGridViewResult.Rows[count].Cells[flagCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[flagCol].Value).Trim();
                    gridViewData.BrandCode = (string)dataGridViewResult.Rows[count].Cells[brandCodeCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[brandCodeCol].Value).Trim();

                    gridViewData.OriginalPlantCode = (string)dataGridViewResult.Rows[count].Cells[originalPlantCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[originalPlantCol].Value).Trim();
                    gridViewData.OriginalCountSheet = (string)dataGridViewResult.Rows[count].Cells[originalCountSheetCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[originalCountSheetCol].Value).Trim();
                    gridViewData.OriginalStorageLocCode = (string)dataGridViewResult.Rows[count].Cells[originalStorageCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[originalStorageCol].Value).Trim();
                    gridViewData.OriginalSectionCode = (string)dataGridViewResult.Rows[count].Cells[originalSectionCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[originalSectionCol].Value).Trim();
                    gridViewData.OriginalSectionName = (string)dataGridViewResult.Rows[count].Cells[originalSectionNameCol].Value == null ? "" : ((string)dataGridViewResult.Rows[count].Cells[originalSectionNameCol].Value).Trim();
                    gridViewDataList.Add(gridViewData);

                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private List<LocationModel> GetGridViewLocationList()
        {
            List<LocationModel> gridViewLocationList = new List<LocationModel>();
            gridViewLocationList.Clear();
            try
            {
                var gridviewList = GetCurrentDataGridView();
                foreach (var gridview in gridviewList)
                {
                    int localFrom = int.Parse(gridview.LocationFrom);
                    int localTo = int.Parse(gridview.LocationTo);

                    while (localFrom <= localTo)
                    {
                        int length = 5;
                        LocationModel newLocation = new LocationModel
                        {
                            LocationCode = localFrom.ToString("D" + length),
                            PlantCode = gridview.PlantCode,
                            Countsheet = gridview.CountSheet,
                            StorageLocationCode = gridview.StorageLocationCode,
                            SectionCode = gridview.SectionCode
                        };

                        gridViewLocationList.Add(newLocation);
                        localFrom++;
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                gridViewDataList = null;
            }
            return gridViewLocationList;
        }

        // Generate insertList and updateList
        private void GenSaveList()
        {
            try
            {
                gridViewDataList.Clear();
                gridViewDataList = GetCurrentDataGridView();

                updateList = (from o in originalList
                              join g in gridViewDataList
                              on new { OriginalSectionCode = o.OriginalSectionCode, OriginalStorageLocationCode = o.OriginalStorageLocCode, OriginPlant = o.OriginalPlantCode, OriginalCountSheet = o.OriginalCountSheet, OriginalSectionName = o.OriginalSectionName }
                              equals new { OriginalSectionCode = g.OriginalSectionCode, OriginalStorageLocationCode = g.OriginalStorageLocCode, OriginPlant = g.OriginalPlantCode, OriginalCountSheet = g.OriginalCountSheet, OriginalSectionName = g.OriginalSectionName }
                              where o.SectionCode != g.SectionCode || o.SectionName != g.SectionName || o.CountSheet != g.CountSheet || o.PlantCode != g.PlantCode || o.StorageLocationCode != g.StorageLocationCode
                              || o.LocationFrom != g.LocationFrom || o.LocationTo != g.LocationTo 
                              select new LocationManagementModel
                              {
                                  PlantCode = g.PlantCode,
                                  CountSheet = g.CountSheet,
                                  StorageLocationCode = g.StorageLocationCode,
                                  SectionCode = g.SectionCode,
                                  SectionName = g.SectionName,
                                  LocationFrom = g.LocationFrom,
                                  LocationTo = g.LocationTo,
                        
                                  OriginalPlantCode = g.OriginalPlantCode,
                                  OriginalCountSheet = g.OriginalCountSheet,
                                  OriginalStorageLocCode = g.OriginalStorageLocCode,
                                  OriginalSectionCode = g.OriginalSectionCode,
                                  OriginalSectionName = g.OriginalSectionName
                              }).ToList();

                insertList = (from g in gridViewDataList
                              where string.IsNullOrEmpty(g.OriginalSectionCode) && string.IsNullOrEmpty(g.OriginalSectionName) && string.IsNullOrEmpty(g.OriginalCountSheet) && string.IsNullOrEmpty(g.OriginalPlantCode) && string.IsNullOrEmpty(g.OriginalStorageLocCode)
                              select new LocationManagementModel
                              {
                                  PlantCode = g.PlantCode,
                                  CountSheet = g.CountSheet,
                                  StorageLocationCode = g.StorageLocationCode,
                                  SectionCode = g.SectionCode,
                                  SectionName = g.SectionName,
                                  LocationFrom = g.LocationFrom,
                                  LocationTo = g.LocationTo,
                              }).ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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
                        //if (!storageLoc.Items.Contains(section.StorageLocationCode))
                        //{
                        //    storageLoc.Items.Add(section.StorageLocationCode);
                        //}

                        if (!CountSheet.Items.Contains(section.CountSheet))
                        {
                            CountSheet.Items.Add(section.CountSheet);
                        }

                        DataGridViewRow row = (DataGridViewRow)dataGridViewResult.Rows[0].Clone();
                        row.Cells[plantCol].Value = (string)section.PlantCode;
                        row.Cells[countSheetCol].Value = (string)section.CountSheet;
                        row.Cells[storageLocationCol].Value = (string)section.StorageLocationCode;
                        row.Cells[sectionCodeCol].Value = (string)section.SectionCode;
                        row.Cells[sectionNameCol].Value = (string)section.SectionName;
                        //row.Cells[originalSectionCol].Value = (string)section.OriginalSectionCode ?? "";

                        row.Cells[locationFromCol].Value = (string)section.LocationFrom;
                        row.Cells[locationToCol].Value = (string)section.LocationTo;
                        row.Cells[flagCol].Value = (string)section.FlagAction ?? "";
                        row.Cells[brandCodeCol].Value = (string)section.BrandCode;

                        row.Cells[originalPlantCol].Value = (string)section.PlantCode;
                        row.Cells[originalCountSheetCol].Value = (string)section.CountSheet;
                        row.Cells[originalStorageCol].Value = (string)section.StorageLocationCode;
                        row.Cells[originalSectionCol].Value = (string)section.SectionCode;
                        row.Cells[originalSectionNameCol].Value = (string)section.SectionName;

                        dataGridViewResult.Rows.Add(row);
                    }
                    //dataGridViewResult.Rows[listSearch.Count].Cells[countSheetCol].Value = CountSheet.Items[0];
                    //SetHeader(dataGridViewResult);
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }
        private bool ValidateEmptyGridview()
        {
            bool result = true;
            List<LocationManagementModel> gridViewDataList = GetCurrentDataGridView();
            int countError = 0;
            for( int i = 0; i < dataGridViewResult.Rows.Count-1; i++ )
            {
                string plant                = dataGridViewResult.Rows[i].Cells[plantCol].Value  == null ? "" : (string)dataGridViewResult.Rows[i].Cells[plantCol].Value;
                string countsheet           = dataGridViewResult.Rows[i].Cells[countSheetCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[countSheetCol].Value;
                string storageLocationCode  = dataGridViewResult.Rows[i].Cells[storageLocationCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[storageLocationCol].Value;
                string sectionCode          = dataGridViewResult.Rows[i].Cells[sectionCodeCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[sectionCodeCol].Value;
                string sectionName          = dataGridViewResult.Rows[i].Cells[sectionNameCol].Value == null  ? "" : (string)dataGridViewResult.Rows[i].Cells[sectionNameCol].Value ;
                string locationFrom         = dataGridViewResult.Rows[i].Cells[locationFromCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[locationFromCol].Value;
                string locationTo           = dataGridViewResult.Rows[i].Cells[locationToCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[locationToCol].Value;

                if (plant == string.Empty)
                {
                    countError++;
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    if (countError == 1)
                    {
                        MessageBox.Show(MessageConstants.Plantcannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (countsheet == string.Empty)
                {
                    countError++;
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;

                    if(countError == 1 )
                    {
                        MessageBox.Show(MessageConstants.CountSheetcannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (storageLocationCode == string.Empty)
                {
                    countError++;
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    if (countError == 1)
                    {
                        MessageBox.Show(MessageConstants.StorageLocationcannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (sectionCode == string.Empty)
                {
                    countError++;
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    if (countError == 1)
                    {
                        MessageBox.Show(MessageConstants.Sectioncodecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (sectionName == string.Empty)
                {
                    countError++;
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    if (countError == 1)
                    {
                        MessageBox.Show(MessageConstants.Sectionnamecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
                else if (locationFrom == string.Empty)
                {
                    countError++;
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    if (countError == 1)
                    {
                        MessageBox.Show(MessageConstants.LocationFromcannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (locationTo == string.Empty)
                {
                    countError++;
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    if (countError == 1)
                    {
                        MessageBox.Show(MessageConstants.LocationTocannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }

            return result;
        }

        private bool ValidateEmptyGridviewExceptCountsheet()
        {
            bool result = true;
            List<LocationManagementModel> gridViewDataList = GetCurrentDataGridView();

            for (int i = 0; i < dataGridViewResult.Rows.Count-1; i++)
            {
                //string plant = (string)dataGridViewResult.Rows[i].Cells[plantCol].Value;
                //string storageLocationCode = (string)dataGridViewResult.Rows[i].Cells[storageLocationCol].Value;
                //string sectionCode = (string)dataGridViewResult.Rows[i].Cells[sectionCodeCol].Value;
                //string sectionName = (string)dataGridViewResult.Rows[i].Cells[sectionNameCol].Value;
                //string locationFrom = (string)dataGridViewResult.Rows[i].Cells[locationFromCol].Value;
                //string locationTo = (string)dataGridViewResult.Rows[i].Cells[locationToCol].Value;

                string plant = dataGridViewResult.Rows[i].Cells[plantCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[plantCol].Value;
                string storageLocationCode = dataGridViewResult.Rows[i].Cells[storageLocationCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[storageLocationCol].Value;
                string sectionCode = dataGridViewResult.Rows[i].Cells[sectionCodeCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[sectionCodeCol].Value;
                string sectionName = dataGridViewResult.Rows[i].Cells[sectionNameCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[sectionNameCol].Value;
                string locationFrom = dataGridViewResult.Rows[i].Cells[locationFromCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[locationFromCol].Value;
                string locationTo = dataGridViewResult.Rows[i].Cells[locationToCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[locationToCol].Value;

                if (plant == string.Empty)
                {
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    MessageBox.Show(MessageConstants.Plantcannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (storageLocationCode == string.Empty)
                {
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    MessageBox.Show(MessageConstants.StorageLocationcannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (sectionCode == string.Empty)
                {
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    MessageBox.Show(MessageConstants.Sectioncodecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (sectionName == string.Empty)
                {
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    MessageBox.Show(MessageConstants.Sectionnamecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (locationFrom == string.Empty)
                {
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    MessageBox.Show(MessageConstants.LocationFromcannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (locationTo == string.Empty)
                {
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    MessageBox.Show(MessageConstants.LocationTocannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }

            return result;
        }

        private bool ValidateEmptyGridviewExceptPlantAndCountsheet()
        {
            bool result = true;
            int countError = 0;
            for (int i = 0; i < dataGridViewResult.Rows.Count-1; i++)
            {
                //string storageLocationCode = (string)dataGridViewResult.Rows[i].Cells[storageLocationCol].Value;
                //string sectionCode = (string)dataGridViewResult.Rows[i].Cells[sectionCodeCol].Value;
                //string sectionName = (string)dataGridViewResult.Rows[i].Cells[sectionNameCol].Value;
                //string locationFrom = (string)dataGridViewResult.Rows[i].Cells[locationFromCol].Value;
                //string locationTo = (string)dataGridViewResult.Rows[i].Cells[locationToCol].Value;

                string storageLocationCode = dataGridViewResult.Rows[i].Cells[storageLocationCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[storageLocationCol].Value;
                string sectionCode = dataGridViewResult.Rows[i].Cells[sectionCodeCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[sectionCodeCol].Value;
                string sectionName = dataGridViewResult.Rows[i].Cells[sectionNameCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[sectionNameCol].Value;
                string locationFrom = dataGridViewResult.Rows[i].Cells[locationFromCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[locationFromCol].Value;
                string locationTo = dataGridViewResult.Rows[i].Cells[locationToCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[locationToCol].Value;


                if (storageLocationCode == string.Empty)
                {
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    countError++;
                    if (countError == 1)
                    {
                        MessageBox.Show(MessageConstants.StorageLocationcannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (sectionCode == string.Empty)
                {                
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    countError++;
                    if (countError == 1)
                    {
                        MessageBox.Show(MessageConstants.Sectioncodecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (sectionName == string.Empty)
                {
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    countError++;
                    if (countError == 1)
                    {
                        MessageBox.Show(MessageConstants.Sectionnamecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (locationFrom == string.Empty)
                {
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    countError++;
                    if (countError == 1)
                    {
                        MessageBox.Show(MessageConstants.LocationFromcannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (locationTo == string.Empty)
                {
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    countError++;
                    if (countError == 1)
                    {
                        MessageBox.Show(MessageConstants.LocationTocannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }

            return result;
        }

        private bool ValidateDupSubLocation()
        {
            List<LocationManagementModel> gridViewDataList = GetCurrentDataGridView();
            int countError = 0;
            dataGridViewResult.DefaultCellStyle.BackColor = Color.White;

            bool result = true;
            bool isExist = false;
            int numFROM_CHK;
            int numTO_CHK;
            int numFROM;
            int numTO;

            for (int i = 0; i < gridViewDataList.Count; i++ )
            {
                int.TryParse(gridViewDataList[i].LocationFrom, out numFROM_CHK);
                int.TryParse(gridViewDataList[i].LocationTo, out numTO_CHK);

                for (int j = 0; j < gridViewDataList.Count; j++)
                {
                    int.TryParse(gridViewDataList[j].LocationFrom, out numFROM);
                    int.TryParse(gridViewDataList[j].LocationTo, out numTO);

                    if ((i != j) && ((numFROM_CHK >= numFROM) && (numFROM_CHK <= numTO) && numFROM_CHK != 0)) 
                    {
                        countError++;
                        result = false;
                        dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        if (countError == 1)
                            MessageBox.Show(MessageConstants.LocationFromcannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    else if ((i != j) && ((numTO_CHK >= numFROM) && (numTO_CHK <= numTO) && numTO != 0))
                    {
                        countError++;
                        result = false;
                        dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        if (countError == 1)
                            MessageBox.Show(MessageConstants.LocationTocannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }                
                }
            }

            return result;
        }

        private bool ValidateDupSubLocationWithMaster()
        {
            List<LocationManagementModel> gridViewDataList = GetCurrentDataGridView();
            int countError = 0;
            dataGridViewResult.DefaultCellStyle.BackColor = Color.White;

            bool result = true;

            for (int i = 0; i < gridViewDataList.Count; i++)
            {
                var countExistLocation = (from l in allLocationList
                                          where l.LocationCode.Equals(gridViewDataList[i].LocationFrom) || l.LocationCode.Equals(gridViewDataList[i].LocationTo)
                                          select l.LocationCode).ToList().Count();

                if (countExistLocation > 0)
                {
                    countError++;
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    if (countError == 1)
                        MessageBox.Show(MessageConstants.LocationTocannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            return result;
        }


        private bool ValidateNumbericGridview()
        {
            bool result = true;
            int countError = 0;
            for (int i = 0; i < dataGridViewResult.Rows.Count-1; i++)
            {
                string locationFrom = (string)dataGridViewResult.Rows[i].Cells[locationFromCol].Value;
                string locationTo = (string)dataGridViewResult.Rows[i].Cells[locationToCol].Value;

                int num;

                if (!int.TryParse(locationFrom, out num) || !int.TryParse(locationTo, out num))
                {
                    countError++;
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    if (countError == 1)
                        MessageBox.Show(MessageConstants.LocationFromisnumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            return result;
        }

        private bool ValidateIsExistsMaster()
        {
            bool result = true;

            int countError = 0;
            for (int i = 0; i < dataGridViewResult.Rows.Count - 1; i++)
            {
                string plant = dataGridViewResult.Rows[i].Cells[plantCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[plantCol].Value;
                string countsheet = dataGridViewResult.Rows[i].Cells[countSheetCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[countSheetCol].Value;
                string storageLocationCode = dataGridViewResult.Rows[i].Cells[storageLocationCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[storageLocationCol].Value;
                string sectionCode = dataGridViewResult.Rows[i].Cells[sectionCodeCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[sectionCodeCol].Value;
                string sectionName = dataGridViewResult.Rows[i].Cells[sectionNameCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[sectionNameCol].Value;
                string locationFrom = dataGridViewResult.Rows[i].Cells[locationFromCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[locationFromCol].Value;
                string locationTo = dataGridViewResult.Rows[i].Cells[locationToCol].Value == null ? "" : (string)dataGridViewResult.Rows[i].Cells[locationToCol].Value;

                if (!bll.IsPlantExistInMaster(plant))
                {
                    countError++;
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;

                    if (countError == 1)
                        MessageBox.Show(MessageConstants.Plantisnotexists, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (!bll.IsCountsheetExistInMaster(countsheet))
                {
                    countError++;
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;

                    if (countError == 1)
                        MessageBox.Show(MessageConstants.Countsheetisnotexists, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (!bll.IsStorageLocationExistInMaster(storageLocationCode,plant,countsheet))
                {
                    countError++;
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;

                    if (countError == 1)
                        MessageBox.Show(MessageConstants.StorageLocationisnotexists, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (dataGridViewResult.Columns[sectionCodeCol].HeaderText == "Brand Code")
                {
                    if (!bll.IsBrandExistInMaster(sectionCode))
                    {
                        countError++;
                        result = false;
                        dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;

                        if (countError == 1)
                            MessageBox.Show(MessageConstants.Brandisnotexists, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            return result;
        }

        private bool IsHaveDataExistInMaster(ref int countSku, ref int countBarcode, ref int countRegularPrice)
        {
            return bll.IsHaveDataExistInMaster(ref countSku, ref countBarcode, ref countRegularPrice);
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
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void dataGridViewResult_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string valueAfterEdit = dataGridViewResult.CurrentCell.Value == null ? "" : (dataGridViewResult.CurrentCell.Value.ToString()).Trim();
                string linePlant = dataGridViewResult.Rows[e.RowIndex].Cells[plantCol].Value == null ? "" : dataGridViewResult.Rows[e.RowIndex].Cells[plantCol].Value.ToString().Trim();
                string lineCountsheet = dataGridViewResult.Rows[e.RowIndex].Cells[plantCol].Value == null ? "" : dataGridViewResult.Rows[e.RowIndex].Cells[countSheetCol].Value.ToString().Trim();

                if (valueAfterEdit == string.Empty && !string.IsNullOrEmpty(valueBeforeEdit))
                {
                    if (dataGridViewResult.CurrentCell.ColumnIndex == sectionCodeCol)
                    {
                        MessageBox.Show(MessageConstants.Sectioncodecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                    }
                    else if (dataGridViewResult.CurrentCell.ColumnIndex == storageLocationCol)
                    {
                        MessageBox.Show(MessageConstants.StorageLocationcannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                    }
                    else if (dataGridViewResult.CurrentCell.ColumnIndex == sectionNameCol)
                    {
                        MessageBox.Show(MessageConstants.Sectionnamecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                    }
                    else if (dataGridViewResult.CurrentCell.ColumnIndex == locationFromCol)
                    {
                        MessageBox.Show(MessageConstants.LocationFromcannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                    }
                    else if (dataGridViewResult.CurrentCell.ColumnIndex == locationToCol)
                    {
                        MessageBox.Show(MessageConstants.LocationTocannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                else
                {
                    if (dataGridViewResult.CurrentCell.OwningColumn.Name.Equals("CountSheet"))
                    {
                        if (valueAfterEdit != valueBeforeEdit)
                        {
                            string valuePlant = dataGridViewResult.CurrentRow.Cells[plantCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[plantCol].Value;
                            string valueSectionCode = dataGridViewResult.CurrentRow.Cells[sectionCodeCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[sectionCodeCol].Value;
                            string valueStorageLocationCode = dataGridViewResult.CurrentRow.Cells[storageLocationCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[storageLocationCol].Value;

                            if (gridViewDataBeforeEdit.Any(x => x.SectionCode == valueSectionCode && x.StorageLocationCode == valueStorageLocationCode && x.CountSheet == valueAfterEdit && x.PlantCode == valuePlant && !string.IsNullOrEmpty(valuePlant)))
                            {
                                MessageBox.Show(MessageConstants.Countsheetcannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                                dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                            }
                            else
                            {
                                dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                            }
                        }
                    }
                    else if (dataGridViewResult.CurrentCell.OwningColumn.Name.Equals("SectionCode") )
                    {
                        if (valueAfterEdit != valueBeforeEdit)
                        {
                            string valuePlant = dataGridViewResult.CurrentRow.Cells[plantCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[plantCol].Value;
                            string valueCountsheet = dataGridViewResult.CurrentRow.Cells[countSheetCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[countSheetCol].Value;
                            string valueStorageLocationCode = dataGridViewResult.CurrentRow.Cells[storageLocationCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[storageLocationCol].Value;

                            if (gridViewDataBeforeEdit.Any(x => x.SectionCode == valueAfterEdit && x.StorageLocationCode == valueStorageLocationCode && x.CountSheet == valueCountsheet && x.PlantCode == valuePlant))
                            {
                                MessageBox.Show(MessageConstants.SectionCodecannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                                dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                            }
                            else
                            {
                                dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                            }                    
                        }

                        //default plant countsheet by copy upper row
                        if (e.RowIndex > 0)
                        {
                            string previousCountsheet = (string)dataGridViewResult.Rows[e.RowIndex - 1].Cells[countSheetCol].Value ?? "";
                            if (!string.IsNullOrEmpty(previousCountsheet) && string.IsNullOrEmpty(linePlant) && string.IsNullOrEmpty(lineCountsheet))
                            {
                                var plant = bll.GetPlantByCountsheet(previousCountsheet);
                                dataGridViewResult.Rows[e.RowIndex].Cells[plantCol].Value = plant;
                                dataGridViewResult.Rows[e.RowIndex].Cells[countSheetCol].Value = previousCountsheet;
                            }
                        }
                    }
                    else if (dataGridViewResult.CurrentCell.OwningColumn.Name.Equals("SectionName"))
                    {
                        //default plant countsheet by copy upper row
                        if (e.RowIndex > 0)
                        {
                            string previousCountsheet = (string)dataGridViewResult.Rows[e.RowIndex - 1].Cells[countSheetCol].Value ?? "";
                            if (!string.IsNullOrEmpty(previousCountsheet) && string.IsNullOrEmpty(linePlant) && string.IsNullOrEmpty(lineCountsheet))
                            {
                                var plant = bll.GetPlantByCountsheet(previousCountsheet);
                                dataGridViewResult.Rows[e.RowIndex].Cells[plantCol].Value = plant;
                                dataGridViewResult.Rows[e.RowIndex].Cells[countSheetCol].Value = previousCountsheet;
                            }
                        }
                    }
                    else if (dataGridViewResult.CurrentCell.OwningColumn.Name.Equals("StorageLoc"))
                    {
                        int num;
                        bool res = int.TryParse((string.IsNullOrEmpty(valueAfterEdit) ? "0" : valueAfterEdit), out num);

                        if (valueAfterEdit.Length < 4)
                        {
                            if (valueAfterEdit.Length == 1)
                            {
                                dataGridViewResult.CurrentCell.Value = "000" + valueAfterEdit;
                                valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                            }
                            else if (valueAfterEdit.Length == 2)
                            {
                                dataGridViewResult.CurrentCell.Value = "00" + valueAfterEdit;
                                valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                            }
                            else if (valueAfterEdit.Length == 3)
                            {
                                dataGridViewResult.CurrentCell.Value = "0" + valueAfterEdit;
                                valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                            }
                            dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                        }
                        else if (valueAfterEdit.Length > 4)
                        {
                            MessageBox.Show(MessageConstants.StorageLocationmustbe4digits, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridViewResult.CurrentCell.Value = "";
                            dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                        }

                        if (res == false)
                        {
                            MessageBox.Show(MessageConstants.LocationFromisnumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                            dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                        }

                        if (valueAfterEdit != valueBeforeEdit)
                        {                           
                            string valuePlant = dataGridViewResult.CurrentRow.Cells[plantCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[plantCol].Value;
                            string valueCountsheet = dataGridViewResult.CurrentRow.Cells[countSheetCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[countSheetCol].Value;
                            string valueSectionCode = dataGridViewResult.CurrentRow.Cells[sectionCodeCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[sectionCodeCol].Value;

                            if (gridViewDataBeforeEdit.Any(x => x.SectionCode == valueSectionCode && x.StorageLocationCode == valueAfterEdit && x.CountSheet == valueCountsheet && x.PlantCode == valuePlant))
                            {
                                MessageBox.Show(MessageConstants.StorageLocationcannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                                dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                            }
                            else if (!bll.IsStorageLocationExistInMaster(valueAfterEdit, valuePlant, valueCountsheet))
                            {
                                MessageBox.Show(MessageConstants.StorageLocationisnotexists, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                                dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                            }
                            else
                            {
                                dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                            }
                        }

                        //default plant countsheet by copy upper row
                        if (e.RowIndex > 0)
                        {
                            string previousCountsheet = (string)dataGridViewResult.Rows[e.RowIndex - 1].Cells[countSheetCol].Value ?? "";
                            if (!string.IsNullOrEmpty(previousCountsheet) && string.IsNullOrEmpty(linePlant) && string.IsNullOrEmpty(lineCountsheet))
                            {
                                var plant = bll.GetPlantByCountsheet(previousCountsheet);
                                dataGridViewResult.Rows[e.RowIndex].Cells[plantCol].Value = plant;
                                dataGridViewResult.Rows[e.RowIndex].Cells[countSheetCol].Value = previousCountsheet;
                            }
                        }
                    }

                    else if (dataGridViewResult.CurrentCell.OwningColumn.Name.Equals("LocationFrom"))
                    {                       
                        #region LocationFrom

                        int num;
                        bool res = int.TryParse((string.IsNullOrEmpty(valueAfterEdit) ? "0" : valueAfterEdit), out num);

                        int valueLocationFrom = 0;
                        int valueLocationTo = 0;
                        try
                        {
                            string locationFrom = (string)dataGridViewResult.Rows[e.RowIndex].Cells[locationFromCol].Value;
                            string locationTo = (string)dataGridViewResult.Rows[e.RowIndex].Cells[locationToCol].Value;

                            bool from = int.TryParse(string.IsNullOrEmpty(locationFrom) ? "0" : locationFrom , out valueLocationFrom);
                            bool to = int.TryParse(string.IsNullOrEmpty(locationTo) ? "0" : locationTo, out valueLocationTo);
                        }
                        catch(Exception ex)
                        {
                            valueLocationFrom = 0;
                            valueLocationTo = 0;
                        }

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
                            else if (valueAfterEdit.Length == 4)
                            {
                                dataGridViewResult.CurrentCell.Value = "0" + valueAfterEdit;
                                valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                            }
                            dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                        }

                        if (res == false)
                        {
                            MessageBox.Show(MessageConstants.LocationFromisnumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                            dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                        }
                        else
                        {
                          
                            string locationCodeFrom = valueAfterEdit;
                            string locationcCodeTo = dataGridViewResult.CurrentRow.Cells[locationToCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[locationToCol].Value;

                            string plant = dataGridViewResult.CurrentRow.Cells[plantCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[plantCol].Value;
                            string countsheet = dataGridViewResult.CurrentRow.Cells[countSheetCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[countSheetCol].Value;
                            string storageLocationCode = dataGridViewResult.CurrentRow.Cells[storageLocationCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[storageLocationCol].Value;
                            string sectionCode = dataGridViewResult.CurrentRow.Cells[sectionCodeCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[sectionCodeCol].Value;

                            int localFrom = int.Parse(locationCodeFrom);
                            int countExistLocation = 0;
                            bool isExist = false;

                            if (bll.IsSectionCodeExistByStoreLocCodeCountsheetPlant(plant, countsheet, storageLocationCode, sectionCode))
                            {
                                var allLocation = bll.GetAllLocation();
                                var gridList = allLocation.FindAll(x=> x.PlantCode.Equals(plant) && x.Countsheet.Equals(countsheet ?? "") && x.StorageLocationCode.Equals(storageLocationCode) && x.SectionCode.Equals(sectionCode) );

                                foreach(var list in gridList)
                                {
                                    allLocation.Remove(list);
                                }

                                int intlocationfrom = int.Parse(string.IsNullOrEmpty((string)dataGridViewResult.CurrentRow.Cells[locationFromCol].Value) ? "0" : (string)dataGridViewResult.CurrentRow.Cells[locationFromCol].Value);
                                int intlocationto = int.Parse(string.IsNullOrEmpty((string)dataGridViewResult.CurrentRow.Cells[locationToCol].Value) ? "0" : (string)dataGridViewResult.CurrentRow.Cells[locationToCol].Value);
                                int length = 5;

                                if (intlocationto != 0)
                                {
                                    for (int i = intlocationfrom; i <= intlocationto; i++ )
                                    {
                                        string findLocation = i.ToString("D" + length);
                                        countExistLocation = (from l in allLocation
                                                              where l.LocationCode.Equals(findLocation)
                                                              select l.LocationCode).ToList().Count();

                                        if (countExistLocation > 0)
                                        {
                                            isExist = true;
                                        }
                                    }
                                }
                                else
                                {
                                    countExistLocation = (from l in allLocation
                                                          where l.LocationCode.Equals(locationCodeFrom)
                                                          select l.LocationCode).ToList().Count();

                                    if (countExistLocation > 0)
                                    {
                                        isExist = true;
                                    }
                                }
                            }
                            else
                            {
                                countExistLocation = (from l in allLocationList
                                                      where l.LocationCode.Equals(locationCodeFrom)
                                                      select l.LocationCode).ToList().Count();

                                if (countExistLocation > 0)
                                {
                                    isExist = true;
                                }
                            }

                            List<LocationModel> listLocationinGrid = distributionLocationExceptCurrentRow(dataGridViewResult, dataGridViewResult.CurrentRow.Index);
                            bool isGridExist = isLocationDupInGrid(listLocationinGrid, locationCodeFrom, locationcCodeTo);

                            if (isExist || countExistLocation > 0 || isGridExist)
                            {
                                MessageBox.Show(MessageConstants.LocationFromcannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                                dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
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
                                    dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                                }
                                else
                                {
                                    dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                                }
                            }
                            else
                            {
                                dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                            }

                            //default plant countsheet by copy upper row
                            if (e.RowIndex > 0)
                            {
                                string previousCountsheet = (string)dataGridViewResult.Rows[e.RowIndex - 1].Cells[countSheetCol].Value ?? "";
                                if (!string.IsNullOrEmpty(previousCountsheet) && string.IsNullOrEmpty(linePlant) && string.IsNullOrEmpty(lineCountsheet))
                                {
                                    dataGridViewResult.Rows[e.RowIndex].Cells[plantCol].Value = bll.GetPlantByCountsheet(previousCountsheet);
                                    dataGridViewResult.Rows[e.RowIndex].Cells[countSheetCol].Value = previousCountsheet;
                                }
                            }
                        }
                        #endregion LocationFrom
                    }
                    else if (dataGridViewResult.CurrentCell.OwningColumn.Name.Equals("LocationTo"))
                    {
                        # region LocationTo

                        int num;
                        bool res = int.TryParse((string.IsNullOrEmpty(valueAfterEdit) ? "0" : valueAfterEdit), out num);

                        int valueLocationFrom = 0;
                        int valueLocationTo = 0;
                        try
                        {
                            string locationFrom = (string)dataGridViewResult.Rows[e.RowIndex].Cells[locationFromCol].Value;
                            string locationTo = (string)dataGridViewResult.Rows[e.RowIndex].Cells[locationToCol].Value;

                            bool from = int.TryParse(locationFrom, out valueLocationFrom);
                            bool to = int.TryParse(locationTo, out valueLocationTo);

                        }
                        catch
                        {
                            valueLocationFrom = 0;
                            valueLocationTo = 0;
                        }

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
                            else if (valueAfterEdit.Length == 4)
                            {
                                dataGridViewResult.CurrentCell.Value = "0" + valueAfterEdit;
                                valueAfterEdit = dataGridViewResult.CurrentCell.Value.ToString();
                            }
                            dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                            
                        }

                        if (res == false)
                        {
                            MessageBox.Show(MessageConstants.LocationToisnumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridViewResult.CurrentCell.Value = valueBeforeEdit;

                            dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                        }
                        else
                        {
                            string locationCodeTo = valueAfterEdit;

                            string locationcodeFrom = dataGridViewResult.CurrentRow.Cells[locationFromCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[locationFromCol].Value;

                            string plant = dataGridViewResult.CurrentRow.Cells[plantCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[plantCol].Value;
                            string countsheet = dataGridViewResult.CurrentRow.Cells[countSheetCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[countSheetCol].Value;
                            string storageLocationCode = dataGridViewResult.CurrentRow.Cells[storageLocationCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[storageLocationCol].Value;
                            string sectionCode = dataGridViewResult.CurrentRow.Cells[sectionCodeCol].Value == null ? "" : (string)dataGridViewResult.CurrentRow.Cells[sectionCodeCol].Value;

                            int localTo = int.Parse(locationCodeTo);
                            int countExistLocation = 0;
                            bool isExist = false;

                            if (bll.IsSectionCodeExistByStoreLocCodeCountsheetPlant(plant, countsheet, storageLocationCode, sectionCode))
                            {
                                var allLocation = bll.GetAllLocation();
                                var gridList = allLocation.FindAll(x => x.PlantCode.Equals(plant) && x.Countsheet.Equals(countsheet ?? "") && x.StorageLocationCode.Equals(storageLocationCode) && x.SectionCode.Equals(sectionCode));
                                foreach (var list in gridList)
                                {
                                    allLocation.Remove(list);
                                }

                                int intlocationfrom = int.Parse(string.IsNullOrEmpty((string)dataGridViewResult.CurrentRow.Cells[locationFromCol].Value) ? "0" : (string)dataGridViewResult.CurrentRow.Cells[locationFromCol].Value);
                                int intlocationto = int.Parse(string.IsNullOrEmpty((string)dataGridViewResult.CurrentRow.Cells[locationToCol].Value) ? "0" : (string)dataGridViewResult.CurrentRow.Cells[locationToCol].Value);
                                int length = 5;
                                if (intlocationfrom != 0)
                                {
                                    for (int i = intlocationfrom; i <= intlocationto; i++)
                                    {
                                        string findLocation = i.ToString("D" + length);
                                        countExistLocation = (from l in allLocation
                                                              where l.LocationCode.Equals(findLocation)                                                             
                                                              select l.LocationCode).ToList().Count();

                                        if (countExistLocation > 0)
                                        {
                                            isExist = true;
                                        }
                                    }
                                }
                                else
                                {
                                    countExistLocation = (from l in allLocation
                                                          where l.LocationCode.Equals(locationCodeTo)
                                                          select l.LocationCode).ToList().Count();

                                    if (countExistLocation > 0)
                                    {
                                        isExist = true;
                                    }
                                }
                            }
                            else
                            {
                                countExistLocation = (from l in allLocationList
                                                      where l.LocationCode.Equals(locationCodeTo)
                                                      select l.LocationCode).ToList().Count();

                                if (countExistLocation > 0)
                                {
                                    isExist = true;
                                }
                            }

                            List<LocationModel> listLocationinGrid = distributionLocationExceptCurrentRow(dataGridViewResult, dataGridViewResult.CurrentRow.Index);
                            bool isGridExist = isLocationDupInGrid(listLocationinGrid, locationcodeFrom, locationCodeTo);

                            if (isExist || countExistLocation > 0 || isGridExist)
                            {
                                MessageBox.Show(MessageConstants.LocationTocannotbeduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                                dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                            }
                            else if (!(dataGridViewResult.CurrentRow.Cells[locationFromCol].Value == null
                                     || dataGridViewResult.CurrentRow.Cells[locationFromCol].Value == ""))
                            {
                                int localFrom = int.Parse(dataGridViewResult.CurrentRow.Cells[locationFromCol].Value.ToString());
                                string locationCodeFrom = dataGridViewResult.CurrentRow.Cells[locationFromCol].Value.ToString();

                                if (localTo < localFrom)
                                {
                                    MessageBox.Show(MessageConstants.LocationTomustequalormorethanLocationForm, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
                                    dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                                }
                                else
                                {
                                    dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                                }
                            }
                            else
                            {
                                dataGridViewResult.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                            }

                            //default plant countsheet by copy upper row
                            if (e.RowIndex > 0)
                            {
                                string previousCountsheet = (string)dataGridViewResult.Rows[e.RowIndex - 1].Cells[countSheetCol].Value ?? "";
                                if (!string.IsNullOrEmpty(previousCountsheet) && string.IsNullOrEmpty(linePlant) && string.IsNullOrEmpty(lineCountsheet))
                                {
                                    dataGridViewResult.Rows[e.RowIndex].Cells[plantCol].Value = bll.GetPlantByCountsheet(previousCountsheet);
                                    dataGridViewResult.Rows[e.RowIndex].Cells[countSheetCol].Value = previousCountsheet;
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private bool isLocationDupInGrid(List<LocationModel> gridLocation, string locationFrom, string locationTo)
        {
            bool result = false;

            int intlocationfrom = int.Parse(string.IsNullOrEmpty(locationFrom) ? "0" : locationFrom);
            int intlocationto = int.Parse(string.IsNullOrEmpty(locationTo) ? "0" : locationTo);
            int length = 5;

            if (intlocationfrom != 0 && intlocationto!= 0)
            {
                for (int i = intlocationfrom; i <= intlocationto; i++)
                {
                    string findLocation = i.ToString("D" + length);
                    int countExistLocation = (from l in gridLocation
                                          where l.LocationCode.Equals(findLocation)
                                          select l.LocationCode).ToList().Count();

                    if (countExistLocation > 0)
                    {
                        result = true;
                    }
                }
            }
            else
            {
                int countExistLocation = (from l in gridLocation
                                          where l.LocationCode.Equals(locationFrom) || l.LocationCode.Equals(locationTo)
                                          select l.LocationCode).ToList().Count();

                if (countExistLocation > 0)
                {
                    result = true;
                }
            }           

            return result;
        }

        private void dataGridViewResult_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                dataGridViewResult.Rows[e.Row.Index - 1].Cells[flagCol].Value = "I";

                var countSheet = dataGridViewResult.CurrentRow.Cells[countSheetCol].Value == null ? "" : dataGridViewResult.CurrentRow.Cells[countSheetCol].Value.ToString();
                var plant = bll.GetPlantByCountsheet(countSheet);

                if(string.IsNullOrEmpty(countSheet))
                {
                    dataGridViewResult.Rows[e.Row.Index].Cells[plantCol].Value = plant;
                    dataGridViewResult.Rows[e.Row.Index].Cells[countSheetCol].Value = countSheet;
                }           
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
            //dataGridViewResult.Rows[e.Row.Index].Cells[brandCol].Value = BrandType.Items[0];//brandtypedeleted
        }

        private void dataGridViewResult_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //string valueAfterEdit = dataGridViewResult.CurrentCell.Value == null ? "" : (dataGridViewResult.CurrentCell.Value.ToString()).Trim();

            //if (valueAfterEdit == string.Empty && !string.IsNullOrEmpty(valueBeforeEdit))
            //{
            //    if (dataGridViewResult.CurrentCell.ColumnIndex == sectionCodeCol)
            //    {
            //        MessageBox.Show(MessageConstants.Sectioncodecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //    else if (dataGridViewResult.CurrentCell.ColumnIndex == sectionNameCol)
            //    {
            //        MessageBox.Show(MessageConstants.Sectionnamecannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //    else if (dataGridViewResult.CurrentCell.ColumnIndex == locationFromCol)
            //    {
            //        MessageBox.Show(MessageConstants.LocationFromcannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //    else if (dataGridViewResult.CurrentCell.ColumnIndex == locationToCol)
            //    {
            //        MessageBox.Show(MessageConstants.LocationTocannotbenull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //    dataGridViewResult.CurrentCell.Value = valueBeforeEdit;
            //}
        }

        private void dataGridViewResult_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {           
            //if (this.dataGridViewResult.CurrentCellAddress.X == CountSheet.DisplayIndex)
            //{
            //    ComboBox cb = e.Control as ComboBox;
            //    DataGridViewComboBoxEditingControl cbo = e.Control as DataGridViewComboBoxEditingControl;
            //    if (cb != null)
            //    {
            //        cb.DropDownStyle = ComboBoxStyle.DropDownList;
            //    }
            //}
        }

        // This event handler manually raises the CellValueChanged event 
        // by calling the CommitEdit method. 
        void dataGridViewResult_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            List<string> rightPlant = new List<string>();
            List<string> rightCountsheet = new List<string>();

            if (this.dataGridViewResult.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                dataGridViewResult.CommitEdit(DataGridViewDataErrorContexts.Commit);

                //if (dataGridViewResult.CurrentCell.ColumnIndex == plantCol)
                //{
                //    rightCountsheet = null;
                //    var plant = dataGridViewResult.CurrentRow.Cells[plantCol].Value == null ? "" : dataGridViewResult.CurrentRow.Cells[plantCol].Value.ToString();

                //    var countSheet = "";
                //    try
                //    {
                //        countSheet = dataGridViewResult.CurrentRow.Cells[countSheetCol].Value.ToString() ?? ""; 
                //    }
                //    catch
                //    {
                //        countSheet = "";
                //    }
                    
                //    if (!ValidatePlantAndCountsheet(plant, countSheet, ref rightPlant, ref rightCountsheet) && !string.IsNullOrEmpty(countSheet))
                //    {
                //        string allPlant = ",";
                //        foreach (var cs in rightPlant)
                //        {
                //            allPlant = allPlant + cs;
                //        }

                //        if (rightPlant.Count >= 1 && !string.IsNullOrEmpty(plant))
                //        {
                //             mes = MessageConstants.PlantIncorrect + " Plant should be" + allPlant.Substring(1, allPlant.Length - 1);
                //             MessageBox.Show(mes, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        }
                //        else if (!string.IsNullOrEmpty(plant))
                //        {
                //            mes = "Not found plant match this countsheet";
                //            MessageBox.Show(mes, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        }
                //    }
                //}
                //else 

                var currentrow = dataGridViewResult.CurrentCell.RowIndex;
                if (dataGridViewResult.CurrentCell.ColumnIndex == countSheetCol)
                {   
                    var countSheet = dataGridViewResult.CurrentRow.Cells[countSheetCol].Value == null ? "" : dataGridViewResult.CurrentRow.Cells[countSheetCol].Value.ToString();
                    var plant = bll.GetPlantByCountsheet(countSheet);

                    dataGridViewResult.CurrentRow.Cells[plantCol].Value = plant;

                    //dataGridViewResult.Rows[currentrow + 1].Cells[plantCol].Value = plant;
                    //dataGridViewResult.Rows[currentrow + 1].Cells[countSheetCol].Value = countSheet;


                    //if (!ValidatePlantAndCountsheet(plant, countSheet, ref rightPlant,ref rightCountsheet) && !string.IsNullOrEmpty(plant))
                    //{
                    //    string allCountsheet = ",";
                    //    foreach (var cs in rightCountsheet)
                    //    {
                    //        allCountsheet = allCountsheet + cs;
                    //    }

                    //    if (rightPlant.Count >= 1 && !string.IsNullOrEmpty(countSheet))
                    //    {
                    //        mes = MessageConstants.CountsheetIncorrect + " Countsheet should be" + allCountsheet.Substring(1, allCountsheet.Length - 1);
                    //        MessageBox.Show(mes, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    }
                    //    else if (!string.IsNullOrEmpty(countSheet))
                    //    {
                    //        mes = "Not found countsheet match this plant";
                    //        MessageBox.Show(mes, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    }
                        
                    //}
                }
            }
        }

        private bool ValidatePlantAndCountsheet(string plant, string countsheet, ref List<string> rightPlant, ref List<string> righrCountsheet)
        {          
            var listCountSheet = settingBLL.GetDropDownCountSheetSKU(plant);

            righrCountsheet = settingBLL.GetDropDownCountSheetSKU(plant);
            rightPlant  = settingBLL.GetPlant(countsheet);

            if (string.IsNullOrEmpty(countsheet))
            {
                 return false;
            }
            else if (string.IsNullOrEmpty(plant))
            {
                return false;
            }
            else if(settingBLL.GetDropDownCountSheetSKU(plant).Count() > 0)
            {
                if (settingBLL.GetDropDownCountSheetSKU(plant).Find(cs => cs == countsheet) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool ValidateDuplicateInGrid()
        {
            bool result = true;
            int countError = 0;
            List<LocationManagementModel> gridViewDataList = GetCurrentDataGridView();

            for (int i = 0; i < gridViewDataList.Count; i++)
            {
                dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.White;
            }

            for (int i = 0; i < gridViewDataList.Count; i++ )
            {
                string plantCode = gridViewDataList[i].PlantCode;
                string countsheet = gridViewDataList[i].CountSheet;
                string storageLocation = gridViewDataList[i].StorageLocationCode;
                string section = gridViewDataList[i].SectionCode;

                for (int j = 0; j < gridViewDataList.Count; j++)
                {
                    string comparePlantCode = gridViewDataList[j].PlantCode;
                    string compareCountsheet = gridViewDataList[j].CountSheet;
                    string compareStorageLocation = gridViewDataList[j].StorageLocationCode;
                    string compareSection = gridViewDataList[j].SectionCode;

                    if ((i != j) && (plantCode.Equals(comparePlantCode) && countsheet.Equals(compareCountsheet) && storageLocation.Equals(compareStorageLocation) && section.Equals(compareSection))) 
                    {
                        countError++;
                        result = false;
                        dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        if (countError == 1)
                            MessageBox.Show(MessageConstants.Duplicatesectioncode, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            return result;
        }

        private bool IsRightPlantAndCountsheet(List<LocationManagementModel> gridViewDataList)
        {
            bool result = true;
            List<string> rightPlant = new List<string>();
            List<string> rightCountsheet = new List<string>();
            for (int i = 0; i < dataGridViewResult.Rows.Count-1; i++)
            {
                string plant = (string)dataGridViewResult.Rows[i].Cells[plantCol].Value;
                string countsheet = (string)dataGridViewResult.Rows[i].Cells[countSheetCol].Value;

                if (!ValidatePlantAndCountsheet(plant, countsheet, ref rightPlant, ref rightCountsheet))
                {
                    result = false;
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
                else
                {
                    dataGridViewResult.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
            return result;
        }

        private bool IsHavePlant(List<LocationManagementModel> dataList)
        {
            bool result = true;

            List<string> listPlant = new List<string>();
            listPlant = settingBLL.GetAllPlant();

            List<string> listPlantGrid = new List<string>();
            listPlantGrid = dataList.Select(x => x.PlantCode).Distinct().ToList();

            var secondNotFirst = listPlantGrid.Except(listPlant).ToList();

            if (secondNotFirst.Count != 0)
            {
                result = false;
            }

            return result;
        }


        private bool IsHaveCountsheet(List<LocationManagementModel> dataList)
        {
            bool result = true;

            List<string> listCountsheet = new List<string>();
            listCountsheet = settingBLL.GetDropDownAllCountSheetSKU();

            List<string> listCountsheetGrid = new List<string>();
            listCountsheetGrid = dataList.Select(x => x.CountSheet).Distinct().ToList();

            var secondNotFirst = listCountsheetGrid.Except(listCountsheet).ToList();

            if (secondNotFirst.Count != 0)
            {
                result = false;
            }

            return result;
        }

        private void dataGridViewResult_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridViewResult.CurrentCell.ColumnIndex == plantCol)
            //{
            //    var plant = dataGridViewResult.CurrentRow.Cells[plantCol].Value.ToString();
            //    AddDropDownCountSheetGrid(plant);
            //}
            
            
            //var plant = dataGridViewResult.CurrentRow.Cells[plantCol].Value.ToString();
            //AddDropDownCountSheetGrid(plant);

            //DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dataGridViewResult.Rows[e.RowIndex].Cells[plantCol];
            //if (cb.Value != null)
            //{
            //    // do stuff
            //    dataGridViewResult.Invalidate();
            //}
        }


        #endregion

        private void ClearData_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(MessageConstants.ClearGrid, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                dataGridViewResult.Rows.Clear();
            }
        }

        private void comboBoxPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddDropDownCountSheet();
            AddDropDownStorageLocation();
        }

        private void btnExportLocation_Click(object sender, EventArgs e)
        {
            try
            {
                GenSaveList(); // generate insertList updateList deleteList

                // 1. If no data 
                if (gridViewDataList.Count == 0)
                {
                    MessageBox.Show(MessageConstants.Nodata, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                // 2. Check empty data and incorrect format -> enter this condition when not found any empty data and correct format
                else if (ValidateEmptyGridview() && ValidateNumbericGridview())
                {
                    if (ValidateDupSubLocation())
                    {
                        string resultDialog = saveDialog();
                        if (resultDialog == "OK")
                        {
                            DialogResult dialogResult = MessageBox.Show("SAVE FILE");
                        }
                        else
                        {
                            if (resultDialog != "CANCEL")
                                MessageBox.Show("Export File Error", MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.ExportLocationFromToIncorrect, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export File Error", MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void btnImportLocation_Click(object sender, EventArgs e)
        {
            try
            {             
                if (AskToSave())
                {
                    if (ValidateDataForSave())
                    {
                        if (SaveCondition())
                        {
                            if (IsClearGrid())
                                dataGridViewResult.Rows.Clear(); 

                            deleteList.Clear();
                            updateList.Clear();
                            insertList.Clear();
                            List<LocationManagementModel> listDuplicateKeyInDB = new List<LocationManagementModel>();
                            string fileType = "";
                            string resultImport = ImportSection(ref listDuplicateKeyInDB, ref fileType, "Location");
                            if (resultImport == "OK")
                            {
                                if (fileType.Length > 0)
                                {
                                    if (fileType.ToLower().Equals("brand"))
                                    {
                                        dataGridViewResult.Columns[sectionCodeCol].HeaderText = "Brand Code";
                                        dataGridViewResult.Columns[sectionNameCol].HeaderText = "Brand Name";
                                       // IsLoadBrand = true;
                                        //UpdateStorageLocationType();
                                    }
                                    else
                                    {
                                        dataGridViewResult.Columns[sectionCodeCol].HeaderText = "Section Code";
                                        dataGridViewResult.Columns[sectionNameCol].HeaderText = "Section Name";
                                        //IsLoadBrand = false;
                                        //UpdateStorageLocationType();
                                    }
                                }
                                
                                if (listDuplicateKeyInDB.Count > 0)
                                {
                                    string msg = "Section below is Duplicate Key " + System.Environment.NewLine;
                                    foreach (var list in listDuplicateKeyInDB)
                                    {
                                        msg += "Plant : " +list.PlantCode + "CountSheet :"+ list.CountSheet + "Section Code : " + list.SectionCode + " StorageLocation Code : " + list.StorageLocationCode + System.Environment.NewLine;
                                    }

                                    MessageBox.Show(msg, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    MessageBox.Show(MessageConstants.Importcomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else if (resultImport == "ERROR")
                            {
                                MessageBox.Show(MessageConstants.Cannotloadtemplate, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                else
                {
                    if (IsClearGrid())
                        dataGridViewResult.Rows.Clear(); 
                    
                    deleteList.Clear();
                    updateList.Clear();
                    insertList.Clear();
                    List<LocationManagementModel> listDuplicateKeyInDB = new List<LocationManagementModel>();
                    string fileType = "";
                    string resultImport = ImportSection(ref listDuplicateKeyInDB, ref fileType, "Location");
                    if (resultImport == "OK")
                    {
                        if (fileType.Length > 0)
                        {
                            if (fileType.ToLower().Equals("brand"))
                            {
                                dataGridViewResult.Columns[sectionCodeCol].HeaderText = "Brand Code";
                                dataGridViewResult.Columns[sectionNameCol].HeaderText = "Brand Name";
                                // IsLoadBrand = true;
                                //UpdateStorageLocationType();
                            }
                            else
                            {
                                dataGridViewResult.Columns[sectionCodeCol].HeaderText = "Section Code";
                                dataGridViewResult.Columns[sectionNameCol].HeaderText = "Section Name";
                                //IsLoadBrand = false;
                                //UpdateStorageLocationType();
                            }
                        }

                        if (listDuplicateKeyInDB.Count > 0)
                        {
                            string msg = "Section below is Duplicate Key " + System.Environment.NewLine;
                            foreach (var list in listDuplicateKeyInDB)
                            {
                                msg += "Plant : " + list.PlantCode + "CountSheet :" + list.CountSheet + "Section Code : " + list.SectionCode + " StorageLocation Code : " + list.StorageLocationCode + System.Environment.NewLine;
                            }

                            MessageBox.Show(msg, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show(MessageConstants.Importcomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
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
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void dataGridViewResult_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.Exception.ToString() + ", column : "+ e.ColumnIndex.ToString(), DateTime.Now);
            e.ThrowException = false;
        }

        private void LocationManagementForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (AskToSave())
                {
                    if (ValidateDataForSave())
                    {
                        if (SaveCondition())
                        {
                            if (dataGridViewResult.Columns[sectionCodeCol].HeaderText == "Brand Code")
                            {
                                settingBLL.UpdateSetting("StorageLocationType", "string", "BRAND");
                            }
                            else
                            {
                                settingBLL.UpdateSetting("StorageLocationType", "string", "SECTION");
                            }

                            List<LocationManagementModel> loadSection = new List<LocationManagementModel>();
                            Loading_Screen.ShowSplashScreen();
                            loadSection = bll.GetSection(gridViewDataList);
                            BindDataToGridView(loadSection);
                            deleteList.Clear();
                            updateList.Clear();
                            insertList.Clear();

                            originalList.Clear();
                            originalList = bll.GetAllSection();
                            allLocationList.Clear();
                            allLocationList = bll.GetAllLocation();

                            Loading_Screen.CloseForm();
                            MessageBox.Show(MessageConstants.Savecomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            e.Cancel = true;
                            
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void dataGridViewResult_SelectionChanged(object sender, EventArgs e)
        {
            //var plant = dataGridViewResult.Rows[e.Row.Index - 1].Cells[plantCol].Value;
            //var countsheet = dataGridViewResult.Rows[e.Row.Index - 1].Cells[countSheetCol].Value;

            //dataGridViewResult.Rows[e.Row.Index].Cells[plantCol].Value = plant;
            //dataGridViewResult.Rows[e.Row.Index].Cells[countSheetCol].Value = countsheet;
        }

        private void dataGridViewResult_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
        {
            var plant = dataGridViewResult.Rows[e.Row.Index - 1].Cells[plantCol].Value;
            var countsheet = dataGridViewResult.Rows[e.Row.Index - 1].Cells[countSheetCol].Value;

            dataGridViewResult.Rows[e.Row.Index].Cells[plantCol].Value = plant;
            dataGridViewResult.Rows[e.Row.Index].Cells[countSheetCol].Value = countsheet;
        }
    }
}
