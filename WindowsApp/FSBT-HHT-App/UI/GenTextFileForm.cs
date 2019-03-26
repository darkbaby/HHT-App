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
using System.IO;
using System.Reflection;
using System.Net;
using System.Collections;
using FSBT.HHT.App.Resources;
using ExportFileUploader;

namespace FSBT.HHT.App.UI
{
    public partial class TextFileForm : Form
    {
        private LogErrorBll logBll = new LogErrorBll(); 
        private GenTextFileBll bll = new GenTextFileBll();
        private LocationManagementBll bllLocal = new LocationManagementBll();
        private SystemSettingBll bllSystem = new SystemSettingBll();
        private GenTextFileModel fileData = new GenTextFileModel();
        private List<MeargeFileFirstRecord> MeargeFileFirstRecordList = new List<MeargeFileFirstRecord>();
        private String messageWarning = "";
        DataTable dtAddTextFile = new DataTable();
        DataTable dtHeaderFront = new DataTable();
        DataTable dtHeaderBack = new DataTable();
        DataTable dtHeaderWarehouse = new DataTable();
        DataTable dtHeaderFreshFood = new DataTable();
        private String[] headerGridViewNotFreshFood = { "Department", "Location", "Total Record", "QTY (Front)", "QTY (Stock)" };
        private String[] headerGridViewFreshFood = { "Department", "Location", "Total Record", "QTY/WtCount" };
        private String OldValueStoreType = "";
        private TempFileImportBll tempFileImportBll = new TempFileImportBll();
        private List<string> fileNamesError = new List<string>();
        public string UserName { get; set; }
        CheckBox HeaderCheckBox = null;
        string strToGenText = "";

        int TotalCheckBoxes = 0;
        int TotalCheckedCheckBoxes = 0;
        bool IsHeaderCheckBoxClicked = false;

        public TextFileForm(string username)
        {
            UserName = username;
            InitializeComponent();
            
            AddHeaderData();
            //InitialSelectedRadio();
            //bindFileName();
            AddDropDownPlant();
            AddDropDownCountSheet();
            AddDropDownMCHLevel1();
            AddDropDownMCHLevel2();
            AddDropDownMCHLevel3();
            AddDropDownMCHLevel4();
            AddDropDownStorageLocation();

            InitialDatagridView();
        }

        private void AddHeaderData()
        {
            //---------------- Header Front -------------------
            #region
            dtHeaderFront.Columns.Add("ColumnName");
            dtHeaderFront.Columns.Add("DataType");
            dtHeaderFront.Columns.Add("Length");
            dtHeaderFront.Columns.Add("Nullable");

            DataRow drHeaderFront1 = dtHeaderFront.NewRow();
            drHeaderFront1[0] = "StocktakingID"; drHeaderFront1[1] = "STRING"; drHeaderFront1[2] = "15"; drHeaderFront1[3] = "YES";
            dtHeaderFront.Rows.Add(drHeaderFront1);

            DataRow drHeaderFront2 = dtHeaderFront.NewRow();
            drHeaderFront2[0] = "ScanMode"; drHeaderFront2[1] = "INT"; drHeaderFront2[2] = "4"; drHeaderFront2[3] = "YES";
            dtHeaderFront.Rows.Add(drHeaderFront2);

            DataRow drHeaderFront3 = dtHeaderFront.NewRow();
            drHeaderFront3[0] = "LocationCode"; drHeaderFront3[1] = "INT"; drHeaderFront3[2] = "5"; drHeaderFront3[3] = "YES";
            dtHeaderFront.Rows.Add(drHeaderFront3);

            DataRow drHeaderFront4 = dtHeaderFront.NewRow();
            drHeaderFront4[0] = "Barcode"; drHeaderFront4[1] = "INT"; drHeaderFront4[2] = "20"; drHeaderFront4[3] = "YES";
            dtHeaderFront.Rows.Add(drHeaderFront4);

            DataRow drHeaderFront5 = dtHeaderFront.NewRow();
            drHeaderFront5[0] = "Quantity"; drHeaderFront5[1] = "DOUBLE"; drHeaderFront5[2] = "18"; drHeaderFront5[3] = "YES";
            dtHeaderFront.Rows.Add(drHeaderFront5);

            DataRow drHeaderFront6 = dtHeaderFront.NewRow();
            drHeaderFront6[0] = "NewQuantity"; drHeaderFront6[1] = "DOUBLE"; drHeaderFront6[2] = "18"; drHeaderFront6[3] = "YES";
            dtHeaderFront.Rows.Add(drHeaderFront6);
            #endregion

            //---------------- Header Back -------------------
            #region
            dtHeaderBack.Columns.Add("ColumnName");
            dtHeaderBack.Columns.Add("DataType");
            dtHeaderBack.Columns.Add("Length");
            dtHeaderBack.Columns.Add("Nullable");

            DataRow dtHeaderBack1 = dtHeaderBack.NewRow();
            dtHeaderBack1[0] = "StocktakingID"; dtHeaderBack1[1] = "STRING"; dtHeaderBack1[2] = "15"; dtHeaderBack1[3] = "YES";
            dtHeaderBack.Rows.Add(dtHeaderBack1);

            DataRow dtHeaderBack2 = dtHeaderBack.NewRow();
            dtHeaderBack2[0] = "ScanMode"; dtHeaderBack2[1] = "INT"; dtHeaderBack2[2] = "4"; dtHeaderBack2[3] = "YES";
            dtHeaderBack.Rows.Add(dtHeaderBack2);

            DataRow dtHeaderBack3 = dtHeaderBack.NewRow();
            dtHeaderBack3[0] = "LocationCode"; dtHeaderBack3[1] = "INT"; dtHeaderBack3[2] = "5"; dtHeaderBack3[3] = "YES";
            dtHeaderBack.Rows.Add(dtHeaderBack3);

            DataRow dtHeaderBack4 = dtHeaderBack.NewRow();
            dtHeaderBack4[0] = "Barcode"; dtHeaderBack4[1] = "INT"; dtHeaderBack4[2] = "20"; dtHeaderBack4[3] = "YES";
            dtHeaderBack.Rows.Add(dtHeaderBack4);

            DataRow dtHeaderBack5 = dtHeaderBack.NewRow();
            dtHeaderBack5[0] = "Quantity"; dtHeaderBack5[1] = "DOUBLE"; dtHeaderBack5[2] = "18"; dtHeaderBack5[3] = "YES";
            dtHeaderBack.Rows.Add(dtHeaderBack5);
            #endregion

            //---------------- Header WareHouse -------------------
            #region
            dtHeaderWarehouse.Columns.Add("ColumnName");
            dtHeaderWarehouse.Columns.Add("DataType");
            dtHeaderWarehouse.Columns.Add("Length");
            dtHeaderWarehouse.Columns.Add("Nullable");

            DataRow dtHeaderWarehouse1 = dtHeaderWarehouse.NewRow();
            dtHeaderWarehouse1[0] = "StocktakingID"; dtHeaderWarehouse1[1] = "STRING"; dtHeaderWarehouse1[2] = "15"; dtHeaderWarehouse1[3] = "YES";
            dtHeaderWarehouse.Rows.Add(dtHeaderWarehouse1);

            DataRow dtHeaderWarehouse2 = dtHeaderWarehouse.NewRow();
            dtHeaderWarehouse2[0] = "ScanMode"; dtHeaderWarehouse2[1] = "INT"; dtHeaderWarehouse2[2] = "4"; dtHeaderWarehouse2[3] = "YES";
            dtHeaderWarehouse.Rows.Add(dtHeaderWarehouse2);

            DataRow dtHeaderWarehouse4 = dtHeaderWarehouse.NewRow();
            dtHeaderWarehouse4[0] = "Barcode"; dtHeaderWarehouse4[1] = "INT"; dtHeaderWarehouse4[2] = "20"; dtHeaderWarehouse4[3] = "YES";
            dtHeaderWarehouse.Rows.Add(dtHeaderWarehouse4);

            DataRow dtHeaderWarehouse5 = dtHeaderWarehouse.NewRow();
            dtHeaderWarehouse5[0] = "Quantity"; dtHeaderWarehouse5[1] = "DOUBLE"; dtHeaderWarehouse5[2] = "18"; dtHeaderWarehouse5[3] = "YES";
            dtHeaderWarehouse.Rows.Add(dtHeaderWarehouse5);

            #endregion

            //---------------- Header FreshFood -------------------
            #region
            dtHeaderFreshFood.Columns.Add("ColumnName");
            dtHeaderFreshFood.Columns.Add("DataType");
            dtHeaderFreshFood.Columns.Add("Length");
            dtHeaderFreshFood.Columns.Add("Nullable");

            DataRow dtHeaderFreshFood1 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood1[0] = "StocktakingID"; dtHeaderFreshFood1[1] = "STRING"; dtHeaderFreshFood1[2] = "15"; dtHeaderFreshFood1[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood1);

            DataRow dtHeaderFreshFood2 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood2[0] = "ScanMode"; dtHeaderFreshFood2[1] = "INT"; dtHeaderFreshFood2[2] = "4"; dtHeaderFreshFood2[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood2);

            DataRow dtHeaderFreshFood3 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood3[0] = "LocationCode"; dtHeaderFreshFood3[1] = "INT"; dtHeaderFreshFood3[2] = "5"; dtHeaderFreshFood3[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood3);

            DataRow dtHeaderFreshFood4 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood4[0] = "Barcode"; dtHeaderFreshFood4[1] = "INT"; dtHeaderFreshFood4[2] = "20"; dtHeaderFreshFood4[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood4);

            DataRow dtHeaderFreshFood5 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood5[0] = "Quantity"; dtHeaderFreshFood5[1] = "DOUBLE"; dtHeaderFreshFood5[2] = "18"; dtHeaderFreshFood5[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood5);

            DataRow dtHeaderFreshFood6 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood6[0] = "NewQuantity"; dtHeaderFreshFood6[1] = "DOUBLE"; dtHeaderFreshFood6[2] = "18"; dtHeaderFreshFood6[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood6);

            DataRow dtHeaderFreshFood7 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood7[0] = "UnitCode"; dtHeaderFreshFood7[1] = "INT"; dtHeaderFreshFood7[2] = "20"; dtHeaderFreshFood7[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood7);

            DataRow dtHeaderFreshFood8 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood8[0] = "Flag"; dtHeaderFreshFood8[1] = "STRING"; dtHeaderFreshFood8[2] = "2"; dtHeaderFreshFood8[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood8);

            DataRow dtHeaderFreshFood9 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood9[0] = "Description"; dtHeaderFreshFood9[1] = "STRING"; dtHeaderFreshFood9[2] = "50"; dtHeaderFreshFood9[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood9);

            DataRow dtHeaderFreshFood10 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood10[0] = "SKUCode"; dtHeaderFreshFood10[1] = "STRING"; dtHeaderFreshFood10[2] = "25"; dtHeaderFreshFood10[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood10);

            DataRow dtHeaderFreshFood11 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood11[0] = "ExBarCode"; dtHeaderFreshFood11[1] = "STRING"; dtHeaderFreshFood11[2] = "15"; dtHeaderFreshFood11[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood11);

            DataRow dtHeaderFreshFood12 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood12[0] = "InBarCode"; dtHeaderFreshFood12[1] = "STRING"; dtHeaderFreshFood12[2] = "15"; dtHeaderFreshFood12[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood12);

            DataRow dtHeaderFreshFood13 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood13[0] = "BrandCode"; dtHeaderFreshFood13[1] = "STRING"; dtHeaderFreshFood13[2] = "5"; dtHeaderFreshFood13[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood13);

            DataRow dtHeaderFreshFood14 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood14[0] = "SKUMode"; dtHeaderFreshFood14[1] = "STRING"; dtHeaderFreshFood14[2] = "5"; dtHeaderFreshFood14[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood14);

            DataRow dtHeaderFreshFood15 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood15[0] = "HHTName"; dtHeaderFreshFood15[1] = "STRING"; dtHeaderFreshFood15[2] = "20"; dtHeaderFreshFood15[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood15);

            DataRow dtHeaderFreshFood16 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood16[0] = "CountDate"; dtHeaderFreshFood16[1] = "STRING"; dtHeaderFreshFood16[2] = "50"; dtHeaderFreshFood16[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood16);

            DataRow dtHeaderFreshFood17 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood17[0] = "CreateDate"; dtHeaderFreshFood17[1] = "STRING"; dtHeaderFreshFood17[2] = "20"; dtHeaderFreshFood17[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood17);

            DataRow dtHeaderFreshFood18 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood18[0] = "CreateBy"; dtHeaderFreshFood18[1] = "STRING"; dtHeaderFreshFood18[2] = "20"; dtHeaderFreshFood18[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood18);

            DataRow dtHeaderFreshFood19 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood19[0] = "UpdateDate"; dtHeaderFreshFood19[1] = "STRING"; dtHeaderFreshFood19[2] = "20"; dtHeaderFreshFood19[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood19);

            DataRow dtHeaderFreshFood20 = dtHeaderFreshFood.NewRow();
            dtHeaderFreshFood20[0] = "UpdateBy"; dtHeaderFreshFood20[1] = "STRING"; dtHeaderFreshFood20[2] = "20"; dtHeaderFreshFood20[3] = "YES";
            dtHeaderFreshFood.Rows.Add(dtHeaderFreshFood20);

            #endregion
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Loading_Screen.ShowSplashScreen();
                dataGridView1.Refresh();
                DataTable DT = (DataTable)dataGridView1.DataSource;
                if (DT != null)
                    DT.Clear();
                DataTable dtHHT = new DataTable();
                String FileCode = "";// GetFileCodeByConditionExport();
                String DeptCode = "";
                String ComputerID = bllSystem.GetComputerID();

                Request searchCondition = GetSearchCondition();

                dtHHT = bll.getSearchUploadFile(searchCondition);
                //if (string.IsNullOrWhiteSpace(FileCode))
                //{
                //    MessageBox.Show(MessageConstants.ConditionExportNotMatch, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //else 
                if (dtHHT.Rows.Count == 0)
                {
                    Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.Nodata, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    doSearch(dtHHT);
                    Loading_Screen.CloseForm();
                }
            }
            catch (Exception ex)
            {
                Loading_Screen.CloseForm();
                logBll.LogError(UserName,this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private Request GetSearchCondition()
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

            Request searchCondition = new Request();

            searchCondition.PlantCode = comboBoxPlant.Text;//.SelectedItem.ToString();
            searchCondition.CountSheet = comboBoxCountSheet.Text;//.SelectedItem.ToString();
            searchCondition.MCHLevel1 = comboBoxLevel1.Text;//.SelectedItem.ToString();
            searchCondition.MCHLevel2 = comboBoxLevel2.Text;//.SelectedItem.ToString();
            searchCondition.MCHLevel3 = comboBoxLevel3.Text;//.SelectedItem.ToString();
            searchCondition.MCHLevel4 = comboBoxLevel4.Text;//.SelectedItem.ToString();
            searchCondition.StorageLocationCode = comboBoxStorageLocation.Text;//.SelectedItem.ToString();
            searchCondition.LocationFrom = textBoxLoForm.Text;
            searchCondition.LocationTo = textBoxLoTo.Text;
            return searchCondition;
        }

        private bool doSearch(DataTable dtHHT)
        {

            //if (string.IsNullOrWhiteSpace(FileCode))
            //{
            //    return false;
            //}
            //else if (dtHHT.Rows.Count == 0)
            //{
            //    return false;
            //}
            //else
            //{
            dataGridView1.DataSource = dtHHT;
            //dataGridView1.AutoResizeColumns();
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            return true;
            //}
        }

        private void btnAddTextFile_Click(object sender, System.EventArgs e)
        {
            try
            {
                List<string> filePaths = OpenMultiFileDialog();
                dgv_ShowFile.DataSource = ConvertListToDataTable(filePaths);
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private DataTable ConvertListToDataTable(List<string> list)
        {
            // New table.
            DataTable table = new DataTable();

            table.Columns.Add("File Name");
            // Add rows.
            foreach (var item in list)
            {
                table.Rows.Add(item);
            }

            return table;
        }

        private List<string> OpenMultiFileDialog()
        {
            DialogResult result = openFileDialog.ShowDialog(); // Show the dialog.

            if (result == DialogResult.OK) // Test result.
            {
                List<string> files = openFileDialog.FileNames.OfType<string>().ToList();
                return files;
            }
            return new List<string>();
        }

        private void btnMerge_Click(object sender, System.EventArgs e)
        {
            try
            {
                fileNamesError.Clear();
                Loading_Screen.ShowSplashScreen();
                List<string> files = new List<string>();
                
                var numberOfRows = dgv_ShowFile.Rows.Count;
                if (numberOfRows > 0)
                {                    
                    for (int i = 0; i < dgv_ShowFile.Rows.Count - 1; i++)
                    {
                        var f = (string)dgv_ShowFile.Rows[i].Cells[0].Value;
                        files.Add(f);
                    }
                    IsTruncateTempTextFileData();
                    foreach (var file in files)
                    {
                        DataSet ds = ConvertListToDataSet(file);

                        if (ds == null)
                        {
                            fileNamesError.Add(file);
                        }
                        else
                        {
                            foreach (DataTable dt in ds.Tables)
                            {
                                bool resultadd = IsAddDataToDatabase(dt, dt.TableName);
                                if (!resultadd)
                                {
                                    fileNamesError.Add(file);
                                }
                            }
                        }

                    }
                    if (fileNamesError.Count > 0)
                    {
                        string fileNameError = ",";
                        foreach (var i in fileNamesError)
                        {
                            fileNameError = fileNameError + i;
                        }
                        fileNameError = fileNameError.Substring(1, (fileNameError.Length - 1));
                        Loading_Screen.CloseForm();
                        MessageBox.Show(MessageConstants.Cannotmergetextfile + "filename : " + fileNameError, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                    else
                    {
                        var data = IsInsertDataTextFile();

                        dgv_ShowFile.Refresh();
                        dgv_ShowFile.DataSource = null;
                        fileNamesError.Clear();

                        Loading_Screen.CloseForm();
                        MessageBox.Show(data.Rows[0]["result"].ToString(), MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }

                }
                else
                {
                    Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.NoTextFile, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
            }
            catch (Exception ex)
            {
                Loading_Screen.CloseForm();
                logBll.LogError(UserName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private DataSet ConvertListToDataSet(string filename)
        {
            DataSet ds = new DataSet();
            List<string> columnsName = new List<string>();
            DataTable dt = new DataTable();
            try
            {
                var txtFromFile = ReadTextFile(filename);
                foreach (string line in txtFromFile)
                {
                    string[] element = line.Split('|');
                    string tablename = "";
                    if (element[0] == "START")
                    {
                        tablename = "TempTextFile" + element[1];
                        columnsName = bll.getColumnsName(tablename);
                        dt = new DataTable(tablename);
                        foreach (var i in columnsName)
                        {
                            dt.Columns.Add(new DataColumn(i));
                        }
                    }
                    else if (element[0] == "END")
                    {
                        ds.Tables.Add(dt);
                    }
                    else
                    {
                        //if (element.Length == (columnsName.Count))
                        //{
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < element.Length; i++)
                            {
                                dr[i] = element[i].Trim();
                            }
                            dt.Rows.Add(dr);
                        //}
                        //else
                        //{
                        //    fileNamesError.Add(filename);
                        //}
                    }


                }
                return ds;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return null;
            }
        }

        private bool IsAddDataToDatabase(DataTable dt, string tableName)
        {
            try
            {
                tempFileImportBll.InsertDataTableToDatabase(dt, tableName);
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        private bool IsTruncateTempTextFileData()
        {
            try
            {
                bll.TruncateTempTextFileData();
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        private DataTable IsInsertDataTextFile()
        {
            try
            {
                return bll.InsertDataTextFile();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return null;
            }
        }

        //public static DataTable ToDataTable<T>(List<T> items)
        //{
        //    DataTable dataTable = new DataTable(typeof(T).Name);
        //    try
        //    {
        //        //Get all the properties
        //        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //        foreach (PropertyInfo prop in Props)
        //        {
        //            //Defining type of data column gives proper data table 
        //            var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
        //            //Setting column names as Property names
        //            dataTable.Columns.Add(prop.Name, type);
        //        }
        //        foreach (T item in items)
        //        {
        //            var values = new object[Props.Length];
        //            for (int i = 0; i < Props.Length; i++)
        //            {
        //                //inserting property values to datatable rows
        //                values[i] = Props[i].GetValue(item, null);
        //            }
        //            dataTable.Rows.Add(values);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
        //        dataTable = new DataTable(typeof(T).Name);
        //    }
        //    //put a breakpoint here and check datatable
        //    return dataTable;
        //}

        //private void ddStoreType_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    //try
        //    //{
        //    //    dataGridView2.Refresh();
        //    //    String selectedText = ((System.Windows.Forms.ComboBox)(sender)).SelectedItem.ToString();
        //    //    if (dtAddTextFile.Rows.Count <= 0)
        //    //    {
        //    //        dataGridView2.DataSource = null;
        //    //        dtAddTextFile = new DataTable();
        //    //        AddHeaderDataTableByStoreType(selectedText);
        //    //        OldValueStoreType = selectedText;
        //    //    }
        //    //    else if (OldValueStoreType.Trim() != selectedText.Trim())
        //    //    {
        //    //        DialogResult dialogResult = MessageBox.Show(MessageConstants.Doyouwanttoclearallfilebelow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //    //        if (dialogResult == DialogResult.Yes)
        //    //        {
        //    //            dataGridView2.DataSource = null;
        //    //            dtAddTextFile = new DataTable();
        //    //            AddHeaderDataTableByStoreType(selectedText);
        //    //            OldValueStoreType = selectedText;
        //    //        }
        //    //        else
        //    //        {
        //    //            ddStoreType.Text = OldValueStoreType;
        //    //        }
        //    //    }
        //    //    dataGridView2.Refresh();
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
        //    //}
        //}

        //private void AddHeaderDataTableByStoreType(string selectedText)
        //{
        //    try
        //    {
        //        if (selectedText.Trim().ToLower().Equals("front"))
        //        {
        //            foreach (DataRow dr in dtHeaderFront.Rows)
        //            {
        //                dtAddTextFile.Columns.Add(dr["ColumnName"].ToString());
        //            }
        //        }
        //        else if (selectedText.Trim().ToLower().Equals("back"))
        //        {
        //            foreach (DataRow dr in dtHeaderBack.Rows)
        //            {
        //                dtAddTextFile.Columns.Add(dr["ColumnName"].ToString());
        //            }
        //        }
        //        else if (selectedText.Trim().ToLower().Equals("warehouse"))
        //        {
        //            foreach (DataRow dr in dtHeaderWarehouse.Rows)
        //            {
        //                dtAddTextFile.Columns.Add(dr["ColumnName"].ToString());
        //            }
        //        }
        //        else if (selectedText.Trim().ToLower().Equals("fresh food"))
        //        {
        //            foreach (DataRow dr in dtHeaderFreshFood.Rows)
        //            {
        //                dtAddTextFile.Columns.Add(dr["ColumnName"].ToString());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
        //    }
        //}

        private void btnClear_Click(object sender, System.EventArgs e)
        {
            try
            {
                dgv_ShowFile.Refresh();
                dgv_ShowFile.DataSource = null;
                fileNamesError.Clear();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        //private void rdSup_Click(object sender, EventArgs e)
        //{
        //    bindFileName();
        //}

        //private void rdDept_Click(object sender, EventArgs e)
        //{
        //    bindFileName();
        //}

        //private void rdFront_Click(object sender, EventArgs e)
        //{
        //    bindFileName();
        //}

        //private void rdWarehouse_Click(object sender, EventArgs e)
        //{
        //    bindFileName();
        //}

        //private void rdFreshFood_Click(object sender, EventArgs e)
        //{
        //    bindFileName();
        //}

        //private void rdONPC_Click(object sender, EventArgs e)
        //{
        //    bindFileName();
        //}

        //private void rdONAS400_Click(object sender, EventArgs e)
        //{
        //    bindFileName();
        //}

        //private void bindFileName()
        //{
        //    string FileCode = GetFileCodeByConditionExport();

        //    if (!(string.IsNullOrWhiteSpace(FileCode)))
        //    {
        //        string fileName = "";
        //        fileName = bll.GetFileNameByFileCode(FileCode);
        //        //txtFileName.Text = fileName;
        //    }
        //    else
        //    {
        //        MessageBox.Show(MessageConstants.ConditionExportNotMatch, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}

        //private string GetFileCodeByConditionExport()
        //{
        //    string catagory = "";
        //    string sectionType = "";
        //    string compareOn = "";

        //    //if (rdSup.Checked)
        //    //{
        //    //    catagory = rdSup.Text;
        //    //}
        //    //else if (rdDept.Checked)
        //    //{
        //    //    catagory = rdDept.Text;
        //    //}

        //    //if (rdFront.Checked)
        //    //{
        //    //    sectionType = rdFront.Text;
        //    //}
        //    //else if (rdWarehouse.Checked)
        //    //{
        //    //    sectionType = rdWarehouse.Text;
        //    //}
        //    //else if (rdFreshFood.Checked)
        //    //{
        //    //    sectionType = rdFreshFood.Text;
        //    //}

        //    //if (rdONAS400.Checked)
        //    //{
        //    //    compareOn = rdONAS400.Text;
        //    //}
        //    //else if (rdONPC.Checked)
        //    //{
        //    //    compareOn = rdONPC.Text;
        //    //}

        //    //string FileCode = "";
        //    //if (catagory.Equals(rdSup.Text)
        //    //    && sectionType.Equals(rdFront.Text)
        //    //    && compareOn.Equals(rdONAS400.Text))
        //    //{
        //    //    FileCode = "PCS0009";
        //    //}
        //    //else if (catagory.Equals(rdSup.Text)
        //    //    && sectionType.Equals(rdFront.Text)
        //    //    && compareOn.Equals(rdONPC.Text))
        //    //{
        //    //    FileCode = "PCS0007";
        //    //}
        //    //else if (catagory.Equals(rdSup.Text)
        //    //     && sectionType.Equals(rdWarehouse.Text)
        //    //     && compareOn.Equals(rdONPC.Text))
        //    //{
        //    //    FileCode = "PCS0011";
        //    //}
        //    //else if (catagory.Equals(rdSup.Text)
        //    //    && sectionType.Equals(rdFreshFood.Text)
        //    //    && compareOn.Equals(rdONAS400.Text))
        //    //{
        //    //    FileCode = "PCS0012";
        //    //}
        //    //else if (catagory.Equals(rdDept.Text)
        //    //   && sectionType.Equals(rdFront.Text)
        //    //   && compareOn.Equals(rdONAS400.Text))
        //    //{
        //    //    FileCode = "PCS0010";
        //    //}
        //    //else if (catagory.Equals(rdDept.Text)
        //    //   && sectionType.Equals(rdFront.Text)
        //    //   && compareOn.Equals(rdONPC.Text))
        //    //{
        //    //    FileCode = "PCS0004";
        //    //}

        //    return "ss";// FileCode;

        //}

        //private void rdSup_CheckedChanged(object sender, EventArgs e)
        //{
        //    //rdFront.Enabled = true;
        //    //rdWarehouse.Enabled = true;
        //    //rdFreshFood.Enabled = true;
        //    //rdFront.Checked = false;
        //    //rdWarehouse.Checked = true;
        //    //rdFreshFood.Checked = false;

        //    //rdONAS400.Checked = false;
        //    //rdONPC.Checked = true;
        //}

        //private void rdDept_CheckedChanged(object sender, EventArgs e)
        //{
        //    //rdFront.Enabled = true;
        //    //rdWarehouse.Enabled = false;
        //    //rdFreshFood.Enabled = false;
        //    //rdFront.Checked = true;
        //    //rdWarehouse.Checked = false;
        //    //rdFreshFood.Checked = false;
        //}

        private void btnExportTXT_Click(object sender, System.EventArgs e)
        {
                                
            Loading_Screen.ShowSplashScreen();
            string fileName = "";
            Request searchCondition = GetSearchCondition();

            DataSet ds = new DataSet();
            btnSearch.PerformClick();
            ds = bll.getTextFileData(searchCondition);
            
            if (ds.Tables.Count == 0)
            {
                Loading_Screen.CloseForm();
                MessageBox.Show(MessageConstants.Nodata, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //dataGridView1.Refresh();
                ////DataTable DT = (DataTable)dataGridView1.DataSource;
                ////if (DT != null)
                ////    DT.Clear();
                ////else
                ////{
                //DataTable dtHHT = new DataTable();
                //dtHHT = bll.getSearchUploadFile(searchCondition);
                //doSearch(dtHHT);
                ////}

                Loading_Screen.CloseForm();
                saveFileDialog.FileName = "";
                DialogResult result = saveFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    fileName = saveFileDialog.FileName;
                    WriteTextFile(fileName, ds);
                }
            }
        }

        protected void AddDropDownMCHLevel1()
        {
            try
            {
                //Get Setting Label
                string MCHLevel1 = bllSystem.GetSettingStringByKey("MCHLevel1");
                lbMCHLevel1.Text = MCHLevel1;

                //Get DropDown
                List<string> listMCH = new List<string>();
                String countSheet = comboBoxCountSheet.Text;

                if (string.IsNullOrEmpty(countSheet))
                {
                    countSheet = "All";
                }

                listMCH = bllSystem.GetDropDownMCH1(countSheet);
                comboBoxLevel1.Items.Clear();
                comboBoxLevel1.Items.Add("All");
                if (listMCH.Count > 0)
                {
                    foreach (var m in listMCH)
                    {
                        comboBoxLevel1.Items.Add(m);
                    }
                }
                comboBoxLevel1.SelectedIndex = 0;
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
                //Get Setting Label
                string MCHLevel2 = bllSystem.GetSettingStringByKey("MCHLevel2");
                lbMCHLevel2.Text = MCHLevel2;

                //Get DropDown
                List<string> listMCH = new List<string>();
                String countSheet = comboBoxCountSheet.Text;
                String hLevel1 = comboBoxLevel1.Text;
                if (string.IsNullOrEmpty(countSheet))
                {
                    countSheet = "All";
                }
                if (string.IsNullOrEmpty(hLevel1))
                {
                    hLevel1 = "All";
                }
                listMCH = bllSystem.GetDropDownMCH2(countSheet, hLevel1);

                comboBoxLevel2.Items.Clear();
                comboBoxLevel2.Items.Add("All");
                if (listMCH.Count > 0)
                {
                    foreach (var m in listMCH)
                    {
                        comboBoxLevel2.Items.Add(m);
                    }                   
                }
                comboBoxLevel2.SelectedIndex = 0;
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
                string MCHLevel3 = bllSystem.GetSettingStringByKey("MCHLevel3");
                lbMCHLevel3.Text = MCHLevel3;

                //Get DropDown
                List<string> listMCH = new List<string>();
                String countSheet = comboBoxCountSheet.Text;
                String hLevel1 = comboBoxLevel1.Text;
                String hLevel2 = comboBoxLevel2.Text;
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
                listMCH = bllSystem.GetDropDownMCH3(countSheet, hLevel1, hLevel2);

                comboBoxLevel3.Items.Clear();
                comboBoxLevel3.Items.Add("All");
                if (listMCH.Count > 0)
                {
                    foreach (var m in listMCH)
                    {
                        comboBoxLevel3.Items.Add(m);
                    } 
                }
                comboBoxLevel3.SelectedIndex = 0;
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
                string MCHLevel4 = bllSystem.GetSettingStringByKey("MCHLevel4");
                lbMCHLevel4.Text = MCHLevel4;

                //Get DropDown
                List<string> listMCH = new List<string>();
                String countSheet = comboBoxCountSheet.Text;
                String hLevel1 = comboBoxLevel1.Text;
                String hLevel2 = comboBoxLevel2.Text;
                String hLevel3 = comboBoxLevel3.Text;
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

                listMCH = bllSystem.GetDropDownMCH4(countSheet, hLevel1, hLevel2, hLevel3);

                comboBoxLevel4.Items.Clear();
                comboBoxLevel4.Items.Add("All");
                if (listMCH.Count > 0)
                {
                    foreach (var m in listMCH)
                    {
                        comboBoxLevel4.Items.Add(m);
                    }
                }
                comboBoxLevel4.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private List<string> ReadTextFile(string file)
        {
            try
            {
                StreamReader steamReader = new StreamReader(file);
                DateTime sysDate = DateTime.Now;

                List<string> dataFromTxt = new List<string>();

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

                return dataFromTxt;
            }
            catch (Exception ex)
            {
                //log.Error("Exception : " + ex.Message);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return null;
            }
        }

        protected void AddDropDownCountSheet()
        {
            try
            {
                //Get DropDown
                List<string> listCountSheet = new List<string>();
                string plant = comboBoxPlant.Text;
                listCountSheet = bllSystem.GetDropDownCountSheetSKU(plant);
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

        protected void AddDropDownPlant()
        {
            try
            {
                //Get DropDown
                List<string> listPlant = new List<string>();
                listPlant = bllSystem.GetAllPlant();

                comboBoxPlant.Items.Clear();
                comboBoxPlant.Items.Add("All");
                if (listPlant.Count > 0)
                {
                    foreach (var l in listPlant)
                    {
                        comboBoxPlant.Items.Add(l);
                    }
                }
                comboBoxPlant.SelectedIndex = 0;
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

        private DataTable GetCountSheetList()
        {
            DataTable CountSheetTable = new DataTable();
            CountSheetTable.Columns.Add("Countsheet", typeof(string));


            foreach (DataGridViewRow row in dataGridView_Countsheet.Rows)
            {
                if (row.Cells["chkBxSelect"].Value != null)
                {
                    if ((int)row.Cells["chkBxSelect"].Value == 1)
                    {
                        DataRow nrow = CountSheetTable.NewRow();
                        nrow["Countsheet"] = row.Cells[1].Value.ToString();
                        CountSheetTable.Rows.Add(nrow);
                    }
                }
            }

            return CountSheetTable;
        }

        private void btnExportSFTP_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable cs = new DataTable();
                cs = GetCountSheetList();

                if(cs.Rows.Count == 0)
                {
                    MessageBox.Show(MessageConstants.PleaseSelectCountSheet, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    bool canExport = false;

                    string textFilePassword = bllSystem.GetSettingStringByKey("CheckExportPassword");
                    UploadExportFile upd = new UploadExportFile(UserName);
                    ConfirmPasswordSFTPForm pass = new ConfirmPasswordSFTPForm();
                    pass.ShowDialog();
                    if (pass.DialogResult == DialogResult.OK)
                    {
                        if (textFilePassword == pass.password)
                        {
                            if (rbServer.Checked)
                            {
                                Loading_Screen.ShowSplashScreen();
                                if (upd.IsConnected())
                                {
                                    canExport = true;
                                    Loading_Screen.CloseForm();
                                }
                                else
                                {
                                    Loading_Screen.CloseForm();
                                    MessageBox.Show(MessageConstants.CannotconnectSFTP, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                canExport = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show(MessageConstants.PasswordWrong, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }


                        if (canExport)
                        {

                            if (rbServer.Checked)
                            {
                                Loading_Screen.ShowSplashScreen();
                                GenTextFileBll genText = new GenTextFileBll();

                                genText.ProcessExportData(cs);
                                string strErr = upd.GenerateFile();

                                if (!strErr.Contains("Fail"))
                                {
                                    strErr = upd.UploadFileToSFTPServer();
                                    if (strErr.Contains("OK"))
                                    {
                                        Loading_Screen.CloseForm();
                                        MessageBox.Show(MessageConstants.Exportcomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        Loading_Screen.CloseForm();
                                        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, strErr, DateTime.Now);
                                        MessageBox.Show(strErr, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                else
                                {
                                    Loading_Screen.CloseForm();
                                }
                            }
                            else
                            {
                                GenTextFileBll genText = new GenTextFileBll();
                                Loading_Screen.ShowSplashScreen();
                                genText.ProcessExportData(cs);
                                string strErr = upd.GenerateFile();

                                if (string.IsNullOrEmpty(strErr))
                                {
                                    Loading_Screen.CloseForm();
                                    MessageBox.Show(MessageConstants.Exportcomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    Loading_Screen.CloseForm();
                                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, strErr, DateTime.Now);
                                    MessageBox.Show(strErr, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                    }
                }       
            }
            catch(Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void WriteTextFile(string filename, DataSet ds)
        {
            int rowCount = 0;
            int rowAll = 0;
            try
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(filename, FileMode.Create), Encoding.UTF8))
                {
                    rowAll = 0;
                    foreach(DataTable dt in ds.Tables)
                    {
                        rowAll = rowAll + dt.Rows.Count;
                        sw.WriteLine("START|" + dt.TableName);
                        foreach (DataRow row in dt.Rows)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                sb = sb.Append('|' + row[dc].ToString());
                            }
                            string s = sb.ToString();
                            int num = s.Length - 1;
                            sw.WriteLine(s.Substring(1, num));
                            rowCount = rowCount + 1;
                        }
                        sw.WriteLine("END|" + dt.TableName);
                    }
                }
                MessageBox.Show("Export file complete : " + rowCount.ToString() + " rows.");
            }
            catch (Exception ex)
            {
                Loading_Screen.CloseForm();
                MessageBox.Show("Export file error : " + ex.Message);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void comboBoxPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddDropDownCountSheet();
            AddDropDownStorageLocation();
            AddDropDownMCHLevel1();
            AddDropDownMCHLevel2();
            AddDropDownMCHLevel3();
            AddDropDownMCHLevel4();
        }

        private void comboBoxCountSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddDropDownMCHLevel1();
            AddDropDownMCHLevel2();
            AddDropDownMCHLevel3();
            AddDropDownMCHLevel4();
        }

        private void comboBoxLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddDropDownMCHLevel2();
            AddDropDownMCHLevel3();
            AddDropDownMCHLevel4();
        }

        private void comboBoxLevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddDropDownMCHLevel3();
            AddDropDownMCHLevel4();
        }

        private void comboBoxLevel3_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddDropDownMCHLevel4();
        }

        private void InitialDatagridView()
        {
            var cslist = bll.GetExportListCountsheet();
            
            if(cslist.Count > 0)
            {
                AddHeaderCheckBox();
                HeaderCheckBox.KeyUp += new KeyEventHandler(HeaderCheckBox_KeyUp);
                HeaderCheckBox.MouseClick += new MouseEventHandler(HeaderCheckBox_MouseClick);
                dataGridView_Countsheet.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_Monitor_CellValueChanged);
                dataGridView_Countsheet.CurrentCellDirtyStateChanged += new EventHandler(dataGridView_Monitor_CurrentCellDirtyStateChanged);
                dataGridView_Countsheet.CellPainting += new DataGridViewCellPaintingEventHandler(dataGridView_Monitor_CellPainting);
                BindGridView();
            }




            
            //HeaderCheckBox.Checked = true;

            //MouseEventArgs ee = new MouseEventArgs(MouseButtons.Left, 1, 5, 6, 0);

            //HeaderCheckBox_MouseClick(this.HeaderCheckBox, ee);

            //this.HeaderCheckBox.Checked = true;
           // HeaderCheckBoxClick(this.HeaderCheckBox);
        }

        private void AddHeaderCheckBox()
        {
            HeaderCheckBox = new CheckBox();

            HeaderCheckBox.Size = new Size(15, 15);
            HeaderCheckBox.Checked = true;
           
            //Add the CheckBox into the DataGridView
            this.dataGridView_Countsheet.Controls.Add(HeaderCheckBox);
        }

        private void ResetHeaderCheckBoxLocation(int ColumnIndex, int RowIndex)
        {
            //Get the column header cell bounds
            Rectangle oRectangle = this.dataGridView_Countsheet.GetCellDisplayRectangle(ColumnIndex, RowIndex, true);

            Point oPoint = new Point();

            oPoint.X = oRectangle.Location.X + (oRectangle.Width - HeaderCheckBox.Width) / 2 + 1;
            oPoint.Y = oRectangle.Location.Y + (oRectangle.Height - HeaderCheckBox.Height) / 2 + 1;

            //Change the location of the CheckBox to make it stay on the header
            HeaderCheckBox.Location = oPoint;
        }

        private void HeaderCheckBoxClick(CheckBox HCheckBox)
        {
            try
            {
                IsHeaderCheckBoxClicked = true;

                foreach (DataGridViewRow Row in dataGridView_Countsheet.Rows)
                    ((DataGridViewCheckBoxCell)Row.Cells["chkBxSelect"]).Value = HCheckBox.Checked;

                dataGridView_Countsheet.RefreshEdit();

                TotalCheckedCheckBoxes = HCheckBox.Checked ? TotalCheckBoxes : 0;

                IsHeaderCheckBoxClicked = false;

            }
            catch(Exception ex)
            {

            }

        }

        private void RowCheckBoxClick(DataGridViewCheckBoxCell RCheckBox)
        {
            if (RCheckBox != null)
            {
                //Modifiy Counter;            
                if (/*(bool)RCheckBox.Value */(int)RCheckBox.Value == 1 && TotalCheckedCheckBoxes < TotalCheckBoxes)
                    TotalCheckedCheckBoxes++;
                else if (TotalCheckedCheckBoxes > 0)
                    TotalCheckedCheckBoxes--;

                //Change state of the header CheckBox.
                if (TotalCheckedCheckBoxes < TotalCheckBoxes)
                    HeaderCheckBox.Checked = false;
                else if (TotalCheckedCheckBoxes == TotalCheckBoxes)
                    HeaderCheckBox.Checked = true;
            }
        }

        private void BindGridView()
        {
            dataGridView_Countsheet.DataSource = GetCountSheetDataSource();
            TotalCheckBoxes = dataGridView_Countsheet.RowCount;
            TotalCheckedCheckBoxes = 2;
        }

        private DataTable GetCountSheetDataSource()
        {
            DataTable dTable = new DataTable();
            var countsheet = bll.GetExportCountsheet();

            return countsheet;
        }

        private void dataGridView_Monitor_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!IsHeaderCheckBoxClicked)
                RowCheckBoxClick((DataGridViewCheckBoxCell)dataGridView_Countsheet[e.ColumnIndex, e.RowIndex]);
        }

        private void dataGridView_Monitor_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView_Countsheet.CurrentCell is DataGridViewCheckBoxCell)
                dataGridView_Countsheet.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void HeaderCheckBox_MouseClick(object sender, MouseEventArgs e)
        {
            HeaderCheckBoxClick((CheckBox)sender);
        }

        private void HeaderCheckBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                HeaderCheckBoxClick((CheckBox)sender);
        }

        private void dataGridView_Monitor_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 0)
                ResetHeaderCheckBoxLocation(e.ColumnIndex, e.RowIndex);
        }

    }
}
    
