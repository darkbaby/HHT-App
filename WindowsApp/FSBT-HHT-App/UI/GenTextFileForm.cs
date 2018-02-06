using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

namespace FSBT.HHT.App.UI
{
    public partial class TextFileForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
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
        //private String[] headerFront = { "StocktakingID", "ScanMode", "LocationCode", "Barcode", "Quantity", "NewQuantity" };
        //private String[] headerBack = { "StocktakingID", "ScanMode", "LocationCode", "Barcode", "Quantity" };
        //private String[] headerWareHouse = { "StocktakingID", "ScanMode", "Barcode", "Quantity" };
        //private String[] headerFreshFood = { "StocktakingID", "ScanMode", "LocationCode", "Quantity" };
        private String OldValueStoreType = "";

        string strToGenText = "";

        public TextFileForm()
        {
            InitializeComponent();
            AddHeaderData();
            InitialSelectedRadio();
            bindFileName();
        }

        private void InitialSelectedRadio()
        {
            rdSup.Checked = true;
            rdFront.Checked = true;
            rdONAS400.Checked = true;
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
                if (validSearch())
                {
                    MessageBox.Show(messageWarning, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Loading_Screen.ShowSplashScreen();
                    dataGridView1.Refresh();
                    DataTable DT = (DataTable)dataGridView1.DataSource;
                    if (DT != null)
                        DT.Clear();
                    DataTable dtHHT = new DataTable();
                    String FileCode = GetFileCodeByConditionExport();
                    String DeptCode = "";
                    if (!(string.IsNullOrWhiteSpace(txtBoxDepartment.Text)))
                    {
                        int deptNum = int.Parse(txtBoxDepartment.Text);
                        DeptCode = deptNum.ToString();
                    }
                    String ComputerID = bllSystem.GetComputerID();
                    dtHHT = bll.getSearchUploadFile(DeptCode, FileCode);
                    Loading_Screen.CloseForm();
                    if (string.IsNullOrWhiteSpace(FileCode))
                    {
                        MessageBox.Show(MessageConstants.ConditionExportNotMatch, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dtHHT.Rows.Count == 0)
                    {
                        MessageBox.Show(MessageConstants.Nodata, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        doSearch(FileCode, dtHHT);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }

        }

        private bool doSearch(string FileCode, DataTable dtHHT)
        {

            if (string.IsNullOrWhiteSpace(FileCode))
            {
                return false;
            }
            else if (dtHHT.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                dataGridView1.DataSource = dtHHT;
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                return true;
            }

            //List<GenTextFileModel> gtfModList = bll.searchHHT(hhtNameInput, sectionCode, sectionNameInput, locationFrom, locationTo, deptCode, sectionType);
            //if (gtfModList != null && gtfModList.Count() > 0)
            //{
            //    foreach (GenTextFileModel gtfm in gtfModList)
            //    {
            //        dataGridView1.Rows.Add(gtfm.HHTName, gtfm.DepartmentCode, gtfm.SectionCode, gtfm.SectionName, gtfm.LocationCode, gtfm.RecordAmount);
            //    }
            //    dataGridView1.Refresh();
            //}
        }

        private bool validSearch()
        {
            bool errorFlag = false;
            messageWarning = "";

            int num;
            bool res = int.TryParse(txtBoxDepartment.Text, out num);
            if (!(string.IsNullOrWhiteSpace(txtBoxDepartment.Text)))
            {
                if (res == false)
                {
                    errorFlag = true;
                    messageWarning = MessageConstants.Departmentcodemustbenumberonly;
                }
            }
            else if (string.IsNullOrWhiteSpace(txtFileName.Text))
            {
                errorFlag = true;
                messageWarning = MessageConstants.Filenamecannotnull;
            }

            return errorFlag;
        }

        private void btnExportTxt_Click(object sender, System.EventArgs e)
        {
            try
            {
                int deptNum;
                bool res = int.TryParse(txtBoxDepartment.Text, out deptNum);
                if (string.IsNullOrWhiteSpace(txtFileName.Text))
                {
                    Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.Filenamecannotnull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //if (res == false)
                    //{
                    //    Loading_Screen.CloseForm();
                    //    MessageBox.Show(MessageConstants.Departmentcodemustbenumberonly, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //}
                    //else
                    //{
                    Loading_Screen.ShowSplashScreen();
                    DataTable dtHHT = new DataTable();
                    DataTable dtHHTHeaderSum = new DataTable();
                    String FileCode = GetFileCodeByConditionExport();
                    String DeptCode = deptNum.ToString() == "0" ? "" : deptNum.ToString();
                    String ComputerID = bllSystem.GetComputerID();
                    if (FileCode.Equals("PCS0009"))
                    {
                        dtHHT = bll.getUploadFilePCS0009(DeptCode);
                    }
                    else if (FileCode.Equals("PCS0011"))
                    {
                        dtHHT = bll.getUploadFilePCS0011(DeptCode);
                    }
                    else if (FileCode.Equals("PCS0007"))
                    {
                        dtHHT = bll.getUploadFilePCS0007(DeptCode);
                    }
                    else if (FileCode.Equals("PCS0012"))
                    {
                        dtHHT = bll.getUploadFilePCS0012(DeptCode);
                    }
                    else if (FileCode.Equals("PCS0004"))
                    {
                        dtHHT = bll.getUploadFilePCS0004(DeptCode);
                    }
                    else if (FileCode.Equals("PCS0010"))
                    {
                        dtHHT = bll.getUploadFilePCS0010(DeptCode);
                    }

                    if (string.IsNullOrWhiteSpace(FileCode))
                    {
                        Loading_Screen.CloseForm();
                        MessageBox.Show(MessageConstants.ConditionExportNotMatch, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dtHHT.Rows.Count > 0)
                    {
                        DataTable dtGrid = new DataTable();
                        dtGrid = bll.getSearchUploadFile(DeptCode, FileCode);
                        doSearch(FileCode, dtGrid);
                        string fileName = "tmpTxtFileExport.txt";
                        var dialog = new SaveFileDialog();
                        dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                        dialog.FileName = FileCode + "_" + ComputerID;
                        var result = dialog.ShowDialog(); //shows save file dialog
                        Loading_Screen.CloseForm();
                        if (result == DialogResult.OK)
                        {
                            Loading_Screen.ShowSplashScreen();
                            dtHHTHeaderSum = bll.getSumExportFile(DeptCode, FileCode);
                            string resultExport = ExportTxtFile(fileName, dtHHT, FileCode, dtHHTHeaderSum);
                            Loading_Screen.CloseForm();
                            if (resultExport.Equals("SUCCESS"))
                            {
                                var wClient = new WebClient();
                                wClient.DownloadFile(fileName, dialog.FileName);
                                if (System.IO.File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)))
                                {
                                    System.IO.File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
                                }

                                MessageBox.Show(MessageConstants.Exportcomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (resultExport.Equals("NODATA"))
                            {
                                MessageBox.Show(MessageConstants.Nodata, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(MessageConstants.Cannotexporttextfile, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        Loading_Screen.CloseForm();
                        MessageBox.Show(MessageConstants.Nodata, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private string ExportTxtFile(string fileName, DataTable dtHHT, string fileCode)
        {
            string fullTitlePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(fullTitlePath, fileName);
            List<FileModelDetail> fileFotmatDetail = bll.GetFileConfigDetailByFileCode(fileCode);

            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                //if (dtHHT.Rows.Count > 0)
                if(strToGenText != "")
                {

                    using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        string headerText = "";
                        string contentText = "";

                        //string[] columnNames = (from dc in dtHHT.Columns.Cast<DataColumn>()
                        //                        select dc.ColumnName).ToArray();

                        //for (int i = 0; i < columnNames.Length; i++)
                        //{
                        //    if (i == 0)
                        //    {
                        //        headerText = columnNames[i];
                        //    }
                        //    else
                        //    {
                        //        headerText += "|" + columnNames[i];
                        //    }
                        //}

                        //sw.WriteLine(headerText);
                        foreach (DataRow dr in dtHHT.Rows)
                        {
                            contentText = "";
                            for (int z = 0; z < dtHHT.Columns.Count; z++)
                            {
                                int lengthCol = (from f in fileFotmatDetail
                                                 where f.Index.Equals(z + 1)
                                                 select f.Length).FirstOrDefault();

                                string defaultFormat = (from f in fileFotmatDetail
                                                        where f.Index.Equals(z + 1)
                                                        select f.Default).FirstOrDefault();

                                string typeFormat = (from f in fileFotmatDetail
                                                     where f.Index.Equals(z + 1)
                                                     select f.Type).FirstOrDefault();

                                int decPosFormat = (from f in fileFotmatDetail
                                                    where f.Index.Equals(z + 1)
                                                    select f.DecPos).FirstOrDefault();

                                string value = "";

                                //check value == length
                                if (dr[z].ToString().Length > lengthCol)
                                {
                                    value = dr[z].ToString().Substring(0, lengthCol);
                                }
                                else
                                {
                                    //check value is null
                                    if (dr[z].ToString().Length == 0)
                                    {
                                        if (defaultFormat.ToUpper().Equals("ZERO"))
                                        {
                                            value = "0";
                                        }
                                        else if (defaultFormat.ToUpper().Equals("BLANKS"))
                                        {
                                            value = " ";
                                        }
                                    }
                                    else
                                    {
                                        value = dr[z].ToString();
                                    }
                                }

                                //check type value + dec pos
                                string gabEmpty = "";
                                if (!(typeFormat.ToUpper().Equals("A")))
                                {
                                    double m;
                                    bool isDouble = double.TryParse(value, out m);

                                    if (decPosFormat > 0 && isDouble)
                                    {
                                        lengthCol = lengthCol + decPosFormat;
                                        for (int i = 1; i <= decPosFormat; i++)
                                        {
                                            gabEmpty += "0";
                                        }
                                        double d = Convert.ToDouble(value);
                                        string f = d.ToString("####." + gabEmpty);
                                        value = f;
                                    }
                                    else if (isDouble)
                                    {
                                        lengthCol = lengthCol + 1;
                                        ////gabEmpty += " ";
                                        value = gabEmpty + Math.Round(Convert.ToDouble(value)).ToString() + " ";
                                    }
                                    //else if (isNumeric)
                                    //{
                                    //    lengthCol = lengthCol + 1;
                                    //    gabEmpty += " ";
                                    //    value = gabEmpty + Convert.ToInt32(value).ToString();
                                    //}
                                    else
                                    {
                                        lengthCol = lengthCol + 1;
                                        value += " ";
                                        ////gabEmpty += " ";
                                        ////value = gabEmpty + value;
                                        value = gabEmpty + value;
                                    }
                                }

                                //add empty length
                                int difLength = lengthCol - value.ToString().Length;
                                for (int i = 1; i <= difLength; i++)
                                {
                                    value += " ";
                                    ////contentText += " ";
                                }
                                contentText = contentText + value;
                            }
                            sw.WriteLine(contentText);

                        }

                        if (strToGenText.Substring(strToGenText.Length - 2, 2) == "\r\n")
                        {
                            strToGenText = strToGenText.Substring(0, strToGenText.Length - 2);//remove last \r\n
                        }
                        sw.Write(strToGenText); 
                      
                    }

                    GC.Collect();
                    return "SUCCESS";
                }
                else
                {
                    return "NODATA";
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return "ERROR";
            }
        }

        private string ExportTxtFile(string fileName, DataTable dtHHT, string fileCode, DataTable dtHHTHeaderSum)
        {
            string fullTitlePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(fullTitlePath, fileName);
            List<FileModelDetail> fileFotmatDetail = bll.GetFileConfigDetailByFileCode(fileCode);

            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                if (dtHHT.Rows.Count > 0)
                {
                    string header = string.Empty;
                    int rowHeader = 1;
                    foreach (DataRow row in dtHHTHeaderSum.Rows)
                    {
                        var headerSum = row.ItemArray;
                        header = headerSum[0].ToString();
                    }
                    using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        string headerText = "";
                        string contentText = "";

                        //string[] columnNames = (from dc in dtHHT.Columns.Cast<DataColumn>()
                        //                        select dc.ColumnName).ToArray();

                        //for (int i = 0; i < columnNames.Length; i++)
                        //{
                        //    if (i == 0)
                        //    {
                        //        headerText = columnNames[i];
                        //    }
                        //    else
                        //    {
                        //        headerText += "|" + columnNames[i];
                        //    }
                        //}

                        //sw.WriteLine(headerText);
                        int countRow = 1;
                        foreach (DataRow dr in dtHHT.Rows)
                        {
                            contentText = "";
                            for (int z = 0; z < dtHHT.Columns.Count - 1; z++)
                            {
                                int lengthCol = (from f in fileFotmatDetail
                                                 where f.Index.Equals(z + 1)
                                                 select f.Length).FirstOrDefault();

                                string defaultFormat = (from f in fileFotmatDetail
                                                        where f.Index.Equals(z + 1)
                                                        select f.Default).FirstOrDefault();

                                string typeFormat = (from f in fileFotmatDetail
                                                     where f.Index.Equals(z + 1)
                                                     select f.Type).FirstOrDefault();

                                int decPosFormat = (from f in fileFotmatDetail
                                                    where f.Index.Equals(z + 1)
                                                    select f.DecPos).FirstOrDefault();

                                string value = "";

                                //check value == length
                                if (dr[z].ToString().Length > lengthCol)
                                {
                                    value = dr[z].ToString().Substring(0, lengthCol);
                                }
                                else
                                {
                                    //check value is null
                                    if (dr[z].ToString().Length == 0)
                                    {
                                        if (defaultFormat.ToUpper().Equals("ZERO"))
                                        {
                                            value = "0";
                                        }
                                        else if (defaultFormat.ToUpper().Equals("BLANKS"))
                                        {
                                            value = " ";
                                        }
                                    }
                                    else
                                    {
                                        value = dr[z].ToString();
                                    }
                                }

                                //check type value + dec pos
                                string gabEmpty = "";
                                if (!(typeFormat.ToUpper().Equals("A")))
                                {
                                    double m;
                                    bool isDouble = double.TryParse(value, out m);

                                    if (decPosFormat > 0 && isDouble)
                                    {
                                        lengthCol = lengthCol + decPosFormat;
                                        for (int i = 1; i <= decPosFormat; i++)
                                        {
                                            gabEmpty += "0";
                                        }
                                        double d = Convert.ToDouble(value);
                                        string f = d.ToString("####." + gabEmpty);
                                        value = f;
                                    }
                                    else if (isDouble)
                                    {
                                        lengthCol = lengthCol + 1;
                                        ////gabEmpty += " ";
                                        value = gabEmpty + Math.Round(Convert.ToDouble(value)).ToString()+ " ";
                                    }
                                    //else if (isNumeric)
                                    //{
                                    //    lengthCol = lengthCol + 1;
                                    //    gabEmpty += " ";
                                    //    value = gabEmpty + Convert.ToInt32(value).ToString();
                                    //}
                                    else
                                    {
                                        lengthCol = lengthCol + 1;
                                        value += " ";
                                        ////gabEmpty += " ";
                                        ////value = gabEmpty + value;
                                        value = gabEmpty + value;
                                    }
                                }

                                //add empty length
                                int difLength = lengthCol - value.ToString().Length;
                                for (int i = 1; i <= difLength; i++)
                                {
                                    ////contentText += " ";
                                    value += " ";
                                }
                                ////contentText = contentText + value;
                                contentText = contentText + value;
                            }
                            if (rowHeader == 1)
                            {
                                sw.WriteLine(header);
                            }
                            sw.WriteLine(contentText);
                            rowHeader++;
                        }
                    }

                    GC.Collect();
                    return "SUCCESS";
                }
                else
                {
                    return "NODATA";
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return "ERROR";
            }
        }

        private void btnAddTextFile_Click(object sender, System.EventArgs e)
        {
            //AddTextFileWithHeaderClick();
            AddTextFileNoHeaderClick();
        }

        private void AddTextFileNoHeaderClick()
        {
            try
            {
                DataTable dtAddNewTextFile = new DataTable();
                DataTable dtHeader = new DataTable();
                List<string> fileNameList = new List<string>();
                List<MeargeFileFirstRecord> firstRecordList = new List<FSBT_HHT_Model.MeargeFileFirstRecord>();
                Hashtable hasResult = new Hashtable();
                hasResult = AddTextFileVlidateNoHeader();
                dtAddNewTextFile = (DataTable)hasResult["table"];
                dtHeader = (DataTable)hasResult["headerTable"];
                fileNameList = (List<string>)hasResult["filename"];
                firstRecordList = (List<MeargeFileFirstRecord>)hasResult["firstRecord"];
                //if (dtAddNewTextFile.Rows.Count > 0)
                if (strToGenText != "")
                {
                    Hashtable hasResult1 = new Hashtable();
                    hasResult1 = MergeTextFile(dtAddTextFile, dtAddNewTextFile, dtHeader, false);
                    string result = (string)hasResult1["result"];
                    dtAddTextFile = (DataTable)hasResult1["tableMerge"];
                    if (result != "Error")
                    //if (result != "DuplicateFile" && result != "Error")
                    {
                        List<FSBT_HHT_Model.FileMod> fileList = new List<FSBT_HHT_Model.FileMod>();
                        List<FSBT_HHT_Model.MeargeFileFirstRecord> firstList = new List<FSBT_HHT_Model.MeargeFileFirstRecord>();
                        int rowCount = 0;
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            FSBT_HHT_Model.FileMod fileModel = new FSBT_HHT_Model.FileMod();
                            fileModel.fileCount = (int)row.Cells[0].Value;
                            fileModel.fileName = row.Cells[1].Value.ToString();
                            fileList.Add(fileModel);
                            rowCount = (int)row.Cells[0].Value;
                        }

                        foreach (var fileName in fileNameList)
                        {
                            rowCount += 1;
                            FSBT_HHT_Model.FileMod fm = new FSBT_HHT_Model.FileMod();
                            fm.fileCount = rowCount;
                            fm.fileName = fileName;
                            fileList.Add(fm);
                        }

                        #region SumMergeFileFirstRecord
                        MeargeFileFirstRecordList.AddRange(firstRecordList);

                        List<MeargeFileFirstRecord> resultFirstRecordComNameDuplicate = MeargeFileFirstRecordList
                        .GroupBy(l => l.Computer)
                        .Select(cl => new MeargeFileFirstRecord
                        {
                            Computer = cl.First().Computer,
                            Record = cl.Sum(c => c.Record == string.Empty ? 0 : Convert.ToInt32(c.Record)).ToString(),
                            QtyFront = cl.Sum(c => c.QtyFront == string.Empty ? 0 : Convert.ToInt32(c.QtyFront)).ToString(),
                            QtyBack = cl.Sum(c => c.QtyBack == string.Empty ? 0 : Convert.ToInt32(c.QtyBack)).ToString(),
                            QtyStockPcs = cl.Sum(c => c.QtyStockPcs == string.Empty ? 0 : Convert.ToInt32(c.QtyStockPcs)).ToString(),
                            QtyStockPck = cl.Sum(c => c.QtyStockPck == string.Empty ? 0 : Convert.ToInt32(c.QtyStockPck)).ToString(),
                            QtyWTPcs = cl.Sum(c => c.QtyWTPcs == string.Empty ? 0 : Convert.ToInt32(c.QtyWTPcs)).ToString(),
                            QtyWTG = cl.Sum(c => c.QtyWTG == string.Empty ? 0 : Convert.ToDecimal(c.QtyWTG)).ToString()
                        }).ToList();
                        MeargeFileFirstRecordList = resultFirstRecordComNameDuplicate;
                        #endregion

                        dataGridView2.DataSource = fileList;
                        dataGridView2.Columns[0].Width = 25;
                        dataGridView2.Columns[1].Width = 490;
                        Loading_Screen.CloseForm();
                        MessageBox.Show(result, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        Loading_Screen.CloseForm();
                        MessageBox.Show(result, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    Loading_Screen.CloseForm();
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void AddTextFileWithHeaderClick()
        {
            //try
            //{
            //    DataTable dtAddNewTextFile = new DataTable();
            //    if (string.IsNullOrEmpty(ddStoreType.Text))
            //    {
            //        Loading_Screen.CloseForm();
            //        MessageBox.Show(MessageConstants.Storetypecannotnull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //    else
            //    {
            //        DataTable dtHeader = new DataTable();
            //        if (ddStoreType.Text.Trim().ToLower().Equals("front"))
            //        {
            //            dtHeader = dtHeaderFront;
            //        }
            //        else if (ddStoreType.Text.Trim().ToLower().Equals("back"))
            //        {
            //            dtHeader = dtHeaderBack;
            //        }
            //        else if (ddStoreType.Text.Trim().ToLower().Equals("warehouse"))
            //        {
            //            dtHeader = dtHeaderWarehouse;
            //        }
            //        else if (ddStoreType.Text.Trim().ToLower().Equals("fresh food"))
            //        {
            //            dtHeader = dtHeaderFreshFood;
            //        }

            //        Hashtable hasResult = new Hashtable();
            //        hasResult = AddTextFileVlidateWithHeader(dtHeader);
            //        dtAddNewTextFile = (DataTable)hasResult["table"];
            //        if (dtAddNewTextFile.Rows.Count > 0)
            //        {
            //            Hashtable hasResult1 = new Hashtable();
            //            hasResult1 = MergeTextFile(dtAddTextFile, dtAddNewTextFile, dtHeader, false);
            //            string result = (string)hasResult1["result"];
            //            dtAddTextFile = (DataTable)hasResult1["tableMerge"];

            //            if (result != "DuplicateFile" && result != "Error")
            //            {
            //                List<FileModel> fileList = new List<FileModel>();

            //                foreach (DataGridViewRow row in dataGridView2.Rows)
            //                {
            //                    FileModel fileModel = new FileModel();
            //                    fileModel.fileCount = (int)row.Cells[0].Value;
            //                    fileModel.fileName = row.Cells[1].Value.ToString();
            //                    fileList.Add(fileModel);
            //                }

            //                FileModel fm = new FileModel();
            //                fm.fileCount = dataGridView2.RowCount + 1;
            //                fm.fileName = (string)hasResult["filename"];
            //                fileList.Add(fm);

            //                dataGridView2.DataSource = fileList;
            //                dataGridView2.Columns[0].Width = 25;
            //                dataGridView2.Columns[1].Width = 100;
            //                Loading_Screen.CloseForm();
            //                MessageBox.Show(result, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            }
            //            else
            //            {
            //                Loading_Screen.CloseForm();
            //                MessageBox.Show(result, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            }
            //        }
            //        else
            //        {
            //            Loading_Screen.CloseForm();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(String.Format("Exception : {0}", ex.StackTrace));
            //}
        }

        private Hashtable AddTextFileVlidateWithHeader(DataTable headerText)
        {
            Hashtable hastResult = new Hashtable();
            DataTable dtSuccess = new DataTable();
            DataTable dtError = new DataTable();
            string fileNameImport = "";

            // Add some elements to the hash table. There are no 
            // duplicate keys, but some of the values are duplicates.

            var FD = new System.Windows.Forms.OpenFileDialog();
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Loading_Screen.ShowSplashScreen();
                string fileToOpen = FD.FileName;
                System.IO.StreamReader reader = new System.IO.StreamReader(fileToOpen);
                fileNameImport = FD.SafeFileName;

                try
                {
                    using (StreamReader sr = reader)
                    {
                        string line;
                        int i = 0;
                        string[] words;

                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!string.IsNullOrEmpty(line))
                            {
                                words = line.Split('|');

                                //Data in Txt to DataTable
                                if (i == 0)
                                {
                                    dtError.Columns.Add("Line");
                                    dtError.Columns.Add("Reason");
                                    DataRow dtRowError = dtError.NewRow();

                                    if (words.Length == headerText.Rows.Count)
                                    {
                                        String[] headerArr = new string[headerText.Rows.Count];
                                        for (int x = 0; x < words.Length; x++)
                                        {
                                            headerArr[x] = words[x];
                                            dtSuccess.Columns.Add(words[x]);
                                        }

                                        dtSuccess.Columns.Add("Line");

                                        bool resultEqualHeader = true;
                                        int index = 0;

                                        foreach (DataRow dr in headerText.Rows)
                                        {
                                            if (dr["ColumnName"].ToString().Trim().ToUpper() != headerArr[index].ToString().Trim().ToUpper())
                                            {
                                                resultEqualHeader = false;
                                            }
                                            index++;
                                        }

                                        if (!(resultEqualHeader))
                                        {
                                            Loading_Screen.CloseForm();
                                            dtRowError[0] = (i + 1).ToString();
                                            dtRowError[1] = "Invalid Column Format File";
                                            dtError.Rows.Add(dtRowError);
                                            break;
                                            dtSuccess = new DataTable();
                                        }
                                    }
                                    else
                                    {
                                        Loading_Screen.CloseForm();
                                        dtRowError[0] = (i + 1).ToString();
                                        dtRowError[1] = "Invalid Column Format File";
                                        dtError.Rows.Add(dtRowError);
                                        break;
                                        dtSuccess = new DataTable();
                                    }
                                }
                                else
                                {
                                    if (words.Length > 0)
                                    {
                                        DataRow dtRowSuccess = dtSuccess.NewRow();
                                        DataRow dtRowError = dtError.NewRow();
                                        string validateResult = "";

                                        for (int x = 0; x < words.Length; x++)
                                        {
                                            int lengthColumn = int.Parse(headerText.Rows[x]["Length"].ToString());
                                            string dataTypeColumn = headerText.Rows[x]["DataType"].ToString();
                                            string nullableColumn = headerText.Rows[x]["Nullable"].ToString();

                                            double num;

                                            if (nullableColumn.ToUpper().Equals("NO"))
                                            {
                                                if (string.IsNullOrEmpty(words[x]))
                                                {
                                                    validateResult = (string)headerText.Rows[x]["ColumnName"] + " cannot be null.";
                                                }

                                                if (dataTypeColumn.ToUpper().Equals("INT") || dataTypeColumn.ToUpper().Equals("DOUBLE"))
                                                {
                                                    bool res = double.TryParse(words[x], out num);
                                                    if (!(res))
                                                    {
                                                        validateResult = (string)headerText.Rows[x]["ColumnName"] + " must be number only.";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (!(string.IsNullOrEmpty(words[x])))
                                                {
                                                    if (dataTypeColumn.ToUpper().Equals("INT") || dataTypeColumn.ToUpper().Equals("DOUBLE"))
                                                    {
                                                        bool res = double.TryParse(words[x], out num);
                                                        if (!(res))
                                                        {
                                                            validateResult = (string)headerText.Rows[x]["ColumnName"] + " must be number only.";
                                                        }
                                                    }
                                                }
                                            }

                                            if (words[x].Length > lengthColumn)
                                            {
                                                validateResult = (string)headerText.Rows[x]["ColumnName"] + " must equeal or less than " + lengthColumn + " digits.";
                                            }

                                            dtRowSuccess[x] = words[x];
                                        }

                                        if (validateResult != string.Empty)
                                        {
                                            dtRowError[0] = (i + 1).ToString();
                                            dtRowError[1] = validateResult;
                                        }
                                        dtRowSuccess[words.Length] = (i + 1).ToString();

                                        if (dtRowSuccess[0].ToString() != string.Empty) { dtSuccess.Rows.Add(dtRowSuccess); }
                                        if (dtRowError[0].ToString() != string.Empty) { dtError.Rows.Add(dtRowError); }
                                    }
                                }
                            }

                            i++;
                        }
                    }

                    if (dtError.Rows.Count > 0)
                    {
                        Loading_Screen.CloseForm();
                        string result = "";

                        foreach (DataRow dr in dtError.Rows)
                        {
                            result += "Line " + dr[0].ToString() + " : " + dr[1].ToString() + " \n";
                        }
                        MessageBox.Show(result, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dtSuccess = new DataTable();
                    }
                    else if (dtSuccess.Rows.Count > 0)
                    {
                        //do nothing
                    }
                    else if (dtSuccess.Rows.Count <= 0 && dtError.Rows.Count <= 0)
                    {
                        Loading_Screen.CloseForm();
                        MessageBox.Show(MessageConstants.Nodata, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dtSuccess = new DataTable();
                    }
                    else
                    {
                        dtSuccess = new DataTable();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(String.Format("Exception : {0}", ex.StackTrace));
                    Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.Cannotaddtextfile, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dtSuccess = new DataTable();
                }
            }
            else
            {
                dtSuccess = new DataTable();
            }

            hastResult.Add("table", dtSuccess);
            hastResult.Add("filename", fileNameImport);

            return hastResult;
        }

        private Hashtable AddTextFileVlidateNoHeader()
        {
            Hashtable hastResult = new Hashtable();
            DataTable dtSuccess = new DataTable();
            DataTable dtSuccessRawData = new DataTable();
            DataTable dtError = new DataTable();
            DataTable dtHeaderTable = new DataTable();
            List<string> fileNameImportList = new List<string>();
            List<MeargeFileFirstRecord> fiirstRecordList = new List<MeargeFileFirstRecord>();
            string fileNameImport = "";
            string fileCodeImport = "";
            bool differentFileType = false;
            string messageDuplicate = "";

            OpenFileDialog FD = new OpenFileDialog();
            FD.Filter = "TXT files|*.txt";
            FD.Multiselect = true;
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Loading_Screen.ShowSplashScreen();
                string[] fileToOpen = FD.FileNames;
                string[] fileNames = FD.SafeFileNames;
                List<string> listFileCode = new List<string>();

                List<string> listFileCodeError = new List<string>();
                string gridViewFileCode = "";

                if (dataGridView2.Rows.Count == 0) //กดครั้งแรก หรือหลังจาก clear
                {
                    //Check FileCode in MultiSelect First
                    foreach (string fn in fileNames)
                    {
                        string[] words = fn.Split('_');

                        if (listFileCode.Count > 0)
                        {
                            //int countName = (from l in listFileCode
                            //                 where l.Equals(words[0])
                            //                 select l).Count();

                            //if (countName > 0)//มี words[0] ใน listFileCode
                            if (listFileCode.Contains(words[0]))
                            {
                                if (bll.isExistsFileCode(words[0]))  //มี words[0] ใน db ใฟ้ add list
                                {
                                    listFileCode.Add(words[0]);
                                }
                                else
                                {
                                    listFileCodeError.Add(fn);
                                }
                            }
                            else
                            {
                                differentFileType = true;
                            }
                        }
                        else
                        {
                            if (bll.isExistsFileCode(words[0]))
                            {
                                listFileCode.Add(words[0]);
                            }
                            else
                            {
                                listFileCodeError.Add(fn);
                                differentFileType = true;
                            }
                        }
                    }
                }
                else
                {
                    foreach (string fn in fileNames)
                    {
                        string[] words = fn.Split('_');

                        if (bll.isExistsFileCode(words[0]))
                        {
                            listFileCode.Add(words[0]);
                        }
                        else
                        {
                            listFileCodeError.Add(fn);
                            differentFileType = true;
                        }
                    }

                    string tmpFileName = (dataGridView2.Rows[0].Cells[1]).Value.ToString();
                    string[] arrFileName = tmpFileName.Split('_');
                    gridViewFileCode = arrFileName[0];
                }

                // Add Coumn DataTable Error
                dtError.Columns.Add("Line");
                dtError.Columns.Add("Reason");
                dtError.Columns.Add("FileName");
                dtHeaderTable.Columns.Add("ColumnName");

                if (!differentFileType)
                {
                    if (gridViewFileCode == "")
                    {
                        gridViewFileCode = listFileCode[0];
                    }

                    List<FileModelDetail> fileDetail = bll.GetFileConfigDetailByFileCode(gridViewFileCode);

                    if (dataGridView2.Rows.Count == 0)
                    {
                        dtSuccess = new DataTable();
                        dtSuccessRawData = new DataTable();
                        dtAddTextFile = new DataTable();

                        foreach (var file in fileDetail)
                        {
                            DataRow dtRowHeaderTable = dtHeaderTable.NewRow();
                            dtRowHeaderTable[0] = file.Field;
                            dtHeaderTable.Rows.Add(dtRowHeaderTable);

                            dtSuccess.Columns.Add(file.Field);
                            dtSuccessRawData.Columns.Add(file.Field);
                            dtAddTextFile.Columns.Add(file.Field);
                        }

                            dtSuccess.Columns.Add("Line");
                            dtSuccess.Columns.Add("FileName");
                            dtSuccessRawData.Columns.Add("Line");
                            dtSuccessRawData.Columns.Add("FileName");
                        
                    }
                    else
                    {
                        foreach (var file in fileDetail)
                        {
                            DataRow dtRowHeaderTable = dtHeaderTable.NewRow();
                            dtRowHeaderTable[0] = file.Field;
                            dtHeaderTable.Rows.Add(dtRowHeaderTable);

                            dtSuccess.Columns.Add(file.Field);
                            dtSuccessRawData.Columns.Add(file.Field);
                        }

                        dtSuccess.Columns.Add("Line");
                        dtSuccess.Columns.Add("FileName");
                        dtSuccessRawData.Columns.Add("Line");
                        dtSuccessRawData.Columns.Add("FileName");
                    }

                    //int countColumn = fileDetail.Count;
                    
                    //for (int a = 1; a <= 2; a++)

                    foreach (string fileOpen in fileToOpen)
                    {
                        System.IO.StreamReader reader = new System.IO.StreamReader(fileOpen);
                        string[] tmpSplitFilePath = fileOpen.Split('\\');
                        fileNameImport = tmpSplitFilePath[tmpSplitFilePath.Length - 1];
                        string[] tmpLastSplit = fileNameImport.Split('_');
                        fileCodeImport = tmpLastSplit[0];

                        //Check FileCode in GridView
                        bool resultIsExistsFileGridView = true;
                        if (gridViewFileCode != "")
                        {
                            if (fileCodeImport != gridViewFileCode)
                            {
                                resultIsExistsFileGridView = false;
                            }
                        }

                        if (resultIsExistsFileGridView)
                        {
                            int sumLength = 0;
                            sumLength += (from f in fileDetail
                                          where f.Type.ToUpper().Equals("A")
                                          select f.Length).Sum();

                            var addLength = (from f in fileDetail
                                             where f.Type.ToUpper() != "A" && f.DecPos == 0
                                             select f.Length + 1).AsEnumerable();

                            sumLength += (from f in addLength
                                          select f).Sum();


                            sumLength += (from f in fileDetail
                                          where f.Type.ToUpper() != "A" && f.DecPos > 0
                                          select f.Length + f.DecPos).Sum();

                            string r = reader.ReadToEnd();

                            
                            List<string> rSplit = r.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();

                            try
                            {
                                string[] firstRecord = rSplit.First().Split('|');

                                var tmp = new MeargeFileFirstRecord()
                                {
                                    Computer = firstRecord[0].ToString(),
                                    Record = firstRecord[1].ToString(),
                                    QtyFront = firstRecord[2].ToString(),
                                    QtyBack = firstRecord[3].ToString(),
                                    QtyStockPcs = firstRecord[4].ToString(),
                                    QtyStockPck = firstRecord[5].ToString(),
                                    QtyWTPcs = firstRecord[6].ToString(),
                                    QtyWTG = firstRecord[7].ToString()
                                };
                                fiirstRecordList.Add(tmp);
                            }
                            catch
                            {
                                DataRow dtRowError = dtError.NewRow();
                                dtRowError[0] = (0).ToString();
                                dtRowError[1] = fileNameImport + " is Invalid Format.";
                                dtRowError[2] = fileNameImport;
                                dtError.Rows.Add(dtRowError);
                                continue;
                            }

                            rSplit.RemoveAt(0);

                            strToGenText = strToGenText + string.Join("\r\n", rSplit.Where(c => c.Length == sumLength)) + "\r\n";

                            var q = rSplit.Where(c => c.Length != sumLength && c != "");

                            foreach(var x in q)
                            {
                                DataRow dtRowError = dtError.NewRow();
                                dtRowError[0] = (rSplit.IndexOf(x) + 1).ToString();
                                dtRowError[1] = fileNameImport + " is Invalid Format.";
                                dtRowError[2] = fileNameImport;
                                dtError.Rows.Add(dtRowError);
                            }

                            if (!fileNameImportList.Contains(fileNameImport))
                            {
                                fileNameImportList.Add(fileNameImport);
                            }

                            using (StreamReader sr = reader)
                            {
                                string line;
                                string[] words;

                                int i = 0;
                                while ((line = sr.ReadLine()) != null)
                                {
                                    
                                    if (!string.IsNullOrEmpty(line))
                                    {
                                        if (line.Length == sumLength)
                                        {
                                            int x = 0;
                                            DataRow dtRowSuccessRawData = dtSuccessRawData.NewRow();
                                            int fileStart = 1;
                                            int fileEnd = 1;
                                            int fileEndOld = fileEnd;
                                            int valueFileEnd = fileEnd;
                                            foreach (var file in fileDetail)
                                            {
                                                if (x == 0)
                                                {
                                                    valueFileEnd = file.EndPos;
                                                }
                                                else
                                                {
                                                    valueFileEnd = fileEndOld + file.Length;
                                                }

                                                if (file.Type.Equals("A"))
                                                {
                                                    fileEnd = valueFileEnd;
                                                }
                                                else if (file.Type != "A" && file.DecPos > 0)
                                                {
                                                    fileEnd = (valueFileEnd + file.DecPos);
                                                }
                                                else if (file.Type != "A" && file.DecPos == 0)
                                                {
                                                    fileEnd = (valueFileEnd + 1);
                                                }

                                                fileEndOld = fileEnd;
                                                //if (fileStart == 1)
                                                //{
                                                //    dtRowSuccessRawData[x] = line.Substring(fileStart, (fileEnd - fileStart));
                                                //}
                                                //else
                                                //{
                                                    dtRowSuccessRawData[x] = line.Substring(fileStart - 1, (fileEnd - (fileStart-1)));
                                                //}
                                               
                                                x++;
                                                fileStart = fileEnd + 1;
                                            }

                                            dtRowSuccessRawData[dtSuccessRawData.Columns.Count - 2] = (i + 1).ToString();
                                            dtRowSuccessRawData[dtSuccessRawData.Columns.Count - 1] = fileNameImport;
                                            dtSuccessRawData.Rows.Add(dtRowSuccessRawData);

                                            int countFile = (from l in fileNameImportList
                                                             where l.Equals(fileNameImport)
                                                             select l).Count();

                                            if (countFile == 0)
                                            {
                                                fileNameImportList.Add(fileNameImport);
                                            }
                                        }
                                        else
                                        {
                                            if (i == 0)
                                            {
                                                //string[] firstRecord = line.Split('|');

                                                //var tmp = new MeargeFileFirstRecord()
                                                //{
                                                //    Computer = firstRecord[0].ToString(),
                                                //    Record = firstRecord[1].ToString(),
                                                //    QtyFront = firstRecord[2].ToString(),
                                                //    QtyBack = firstRecord[3].ToString(),
                                                //    QtyStockPcs = firstRecord[4].ToString(),
                                                //    QtyStockPck = firstRecord[5].ToString(),
                                                //    QtyWTPcs = firstRecord[6].ToString(),
                                                //    QtyWTG = firstRecord[7].ToString()
                                                //};
                                                //fiirstRecordList.Add(tmp);
                                            }
                                            else
                                            {
                                                DataRow dtRowError = dtError.NewRow();
                                                dtRowError[0] = (i + 1).ToString();
                                                dtRowError[1] = fileNameImport + " is Invalid Format.";
                                                dtRowError[2] = fileNameImport;
                                                dtError.Rows.Add(dtRowError);
                                            }

                                        }
                                    }
                                    i++;
                                }
                            }

                            
                        }
                        else
                        {
                            DataRow dtRowError = dtError.NewRow();
                            dtRowError[0] = "0";
                            dtRowError[1] = fileNameImport + " is Invalid File Type.";
                            dtRowError[2] = fileNameImport;
                            dtError.Rows.Add(dtRowError);
                        }

                        int count = (from l in fileNameImportList
                                     where l.Equals(fileNameImport)
                                     select l).Count();

                        if (count == 0)
                        {
                            dtSuccessRawData.Clear();
                        }
                        else
                        {
                            Hashtable hasResult1 = new Hashtable();
                            hasResult1 = MergeTextFile(dtSuccess, dtSuccessRawData, dtHeaderTable, true);
                            string result = (string)hasResult1["result"];
                            dtSuccess = (DataTable)hasResult1["tableMerge"];

                            dtSuccessRawData.Clear();

                            if (result != "DuplicateFile" && result != "Error" && result != "Success")
                            {
                                messageDuplicate += result;
                            }
                        }
                    }
                }
                else
                {
                    Loading_Screen.CloseForm();
                    MessageBox.Show(MessageConstants.DifferentFileType, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dtSuccess = new DataTable();
                }
            }

            if (differentFileType)
            {
                //do nothing
            }
            else if (dtError.Rows.Count > 0)
            {
                Loading_Screen.CloseForm();
                string result = "";

                foreach (DataRow dr in dtError.Rows)
                {
                    result += "[File : " + dr[2].ToString() + "][Line : " + dr[0].ToString() + "] " + dr[1].ToString() + " \n";
                }
                MessageBox.Show(result, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //else if (dtSuccess.Rows.Count > 0)
            else if (strToGenText != "")
            {
                //do nothing
            }
            //else if (dtSuccess.Rows.Count <= 0 && dtError.Rows.Count <= 0)
            else if (strToGenText == "" && dtError.Rows.Count <= 0)
            {
                Loading_Screen.CloseForm();
                MessageBox.Show(MessageConstants.Nodata, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtSuccess = new DataTable();
            }
            else
            {
                dtSuccess = new DataTable();
            }

            if (!(string.IsNullOrWhiteSpace(messageDuplicate)))
            {
                Loading_Screen.CloseForm();
                MessageBox.Show(messageDuplicate, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            hastResult.Add("table", dtSuccess);
            hastResult.Add("filename", fileNameImportList);
            hastResult.Add("firstRecord", fiirstRecordList);
            hastResult.Add("headerTable", dtHeaderTable);

            return hastResult;
        }

        private Hashtable MergeTextFile(DataTable dtAddTextFileOld, DataTable dtAddNewTextFile, DataTable headerText, bool isExitsLineFile)
        {
            Hashtable hastResult = new Hashtable();
            string resultReturn = "Success";
            try
            {
                DataTable TmpTextFile = new DataTable();
                TmpTextFile = dtAddTextFileOld.Clone();
                List<string> listFile = new List<string>();
                List<string> listFileDuplicate = new List<string>();
                foreach (DataRow dr in dtAddNewTextFile.Rows)
                {
                    int countFile = (from l in listFile
                                     where l.Equals(dr["FileName"].ToString())
                                     select l).Count();

                    if (countFile == 0) { listFile.Add(dr["FileName"].ToString()); }

                    DataTable TmpNewTextFile = new DataTable();
                    TmpNewTextFile = dtAddTextFileOld.Copy();
                    string[] resultWhereDuplicate = new string[headerText.Rows.Count];
                    int i = 0;
                    //foreach (DataRow rows in headerText.Rows)
                    //{
                    //    DataTable tmp = new DataTable();
                    //    tmp = TmpNewTextFile.Clone();
                    //    var query = (from d in TmpNewTextFile.AsEnumerable()
                    //                 where d[rows["ColumnName"].ToString()].Equals(dr[rows["ColumnName"].ToString()])
                    //                 select d);

                    //    foreach (var q in query)
                    //    {
                    //        tmp.ImportRow(q);
                    //    }

                    //    TmpNewTextFile = tmp;
                    //}

                    //if (TmpNewTextFile.Rows.Count <= 0)
                    //{
                    DataRow drTmpTextFile = TmpTextFile.NewRow();
                    for (int a = 0; a < headerText.Rows.Count; a++)
                    {
                        drTmpTextFile[a] = dr[a];
                    }

                    if (isExitsLineFile)
                    {
                        drTmpTextFile[headerText.Rows.Count] = dr["Line"].ToString();
                        drTmpTextFile[headerText.Rows.Count + 1] = dr["FileName"].ToString();
                    }

                    //TmpTextFile.Rows.Add(drTmpTextFile);

                    //}
                    //else
                    //{
                    //    int count = (from l in listFileDuplicate
                    //                 where l.Equals(dr["FileName"].ToString())
                    //                 select l).Count();

                    //    if (count == 0) { listFileDuplicate.Add(dr["FileName"].ToString()); }

                    //    // duplicate each other row
                    //    if (resultReturn.Equals("Success"))
                    //    {
                    //        resultReturn = "Line " + dr["Line"].ToString() + " of " + dr["FileName"].ToString() + " has duplicate data in file which conflict with previous uploaded data. \n";
                    //    }
                    //    else
                    //    {
                    //        resultReturn += "Line " + dr["Line"].ToString() + " of " + dr["FileName"].ToString() + " has duplicate data in file which conflict with previous uploaded data. \n";
                    //    }
                    //}
                }

                //if (TmpTextFile.Rows.Count > 0)
                //{
                foreach (DataRow dr in TmpTextFile.Rows)
                {
                    dtAddTextFileOld.ImportRow(dr);
                }
                //}
                //else
                //{
                ////if (TmpTextFile.Rows.Count == 0 && listFile.Count() > 1)
                ////{
                ////    if (listFileDuplicate.Count() > 1)
                ////    {
                ////        //do nothing
                ////    }
                ////    else
                ////    {
                ////        // duplicate all file
                ////        resultReturn = "DuplicateFile";
                ////    }

                ////}
                ////else
                ////{
                //// duplicate all file

                //resultReturn = "DuplicateFile";
                ////}
                //}
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                resultReturn = "Error";
            }

            hastResult.Add("tableMerge", dtAddTextFileOld);
            hastResult.Add("result", resultReturn);

            return hastResult;

        }

        private void btnMerge_Click(object sender, System.EventArgs e)
        {
            try
            {
                string destinationPath = vPath.Text.ToString();
                if (dataGridView2.Rows.Count > 0)
                {
                    bool browseFileExport = true;
                    if (!(string.IsNullOrWhiteSpace(destinationPath)))
                    {
                        try
                        {
                            //Check Path
                            if (destinationPath.ToLower().IndexOf(".txt") != -1)
                            {
                                if (!(File.Exists(destinationPath)))
                                {
                                    using (System.IO.StreamWriter file =
                                    new System.IO.StreamWriter(@destinationPath, true))
                                    {
                                        file.WriteLine("Fourth line");
                                    }
                                    File.Delete(destinationPath);
                                }
                                else
                                {
                                    File.Delete(destinationPath);
                                }

                                browseFileExport = false;
                            }
                            else
                            {
                                MessageBox.Show(MessageConstants.PathNotExists, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(MessageConstants.PathNotExists, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                    }

                    if (browseFileExport)
                    {
                        string fileName = "tmpTxtFileExport.txt";
                        var dialog = new SaveFileDialog();
                        dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                        string tmpFileName = (dataGridView2.Rows[0].Cells[1]).Value.ToString();
                        string[] arrFileName = tmpFileName.Split('_');
                        string gridViewFileCode = arrFileName[0];

                        var result = dialog.ShowDialog(); //shows save file dialog
                        if (result == DialogResult.OK)
                        {
                            string resultExport = ExportTxtFile(fileName, dtAddTextFile, gridViewFileCode);

                            if (resultExport.Equals("SUCCESS"))
                            {
                                var wClient = new WebClient();
                                wClient.DownloadFile(fileName, dialog.FileName);
                                if (System.IO.File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)))
                                {
                                    System.IO.File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
                                }

                                vPath.Text = dialog.FileName;
                                MessageBox.Show(MessageConstants.Mergecomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);

                                GenTextFileSummaryForm fSummary = new GenTextFileSummaryForm(); // Instantiate a Form3 object.
                                fSummary.SummaryFirstRecordMeargeFile(MeargeFileFirstRecordList);
                                fSummary.Show();
                            }
                            else if (resultExport.Equals("NODATA"))
                            {
                                MessageBox.Show(MessageConstants.Nodata, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(MessageConstants.Cannotmergetextfile, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        string fileName = "tmpTxtFileExport.txt";
                        string tmpFileName = (dataGridView2.Rows[0].Cells[1]).Value.ToString();
                        string[] arrFileName = tmpFileName.Split('_');
                        string gridViewFileCode = arrFileName[0];

                        string resultExport = ExportTxtFile(fileName, dtAddTextFile, gridViewFileCode);

                        if (resultExport.Equals("SUCCESS"))
                        {
                            var wClient = new WebClient();
                            wClient.DownloadFile(fileName, destinationPath);
                            if (System.IO.File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)))
                            {
                                System.IO.File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
                            }

                            vPath.Text = destinationPath;
                            MessageBox.Show(MessageConstants.Mergecomplete, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            GenTextFileSummaryForm fSummary = new GenTextFileSummaryForm(); // Instantiate a Form3 object.
                            fSummary.SummaryFirstRecordMeargeFile(MeargeFileFirstRecordList);
                            fSummary.Show();
                        }
                        else if (resultExport.Equals("NODATA"))
                        {
                            MessageBox.Show(MessageConstants.Nodata, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(MessageConstants.Cannotmergetextfile, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(MessageConstants.NoTextFile, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            try
            {
                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Defining type of data column gives proper data table 
                    var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name, type);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                dataTable = new DataTable(typeof(T).Name);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        private void ddStoreType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //try
            //{
            //    dataGridView2.Refresh();
            //    String selectedText = ((System.Windows.Forms.ComboBox)(sender)).SelectedItem.ToString();
            //    if (dtAddTextFile.Rows.Count <= 0)
            //    {
            //        dataGridView2.DataSource = null;
            //        dtAddTextFile = new DataTable();
            //        AddHeaderDataTableByStoreType(selectedText);
            //        OldValueStoreType = selectedText;
            //    }
            //    else if (OldValueStoreType.Trim() != selectedText.Trim())
            //    {
            //        DialogResult dialogResult = MessageBox.Show(MessageConstants.Doyouwanttoclearallfilebelow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //        if (dialogResult == DialogResult.Yes)
            //        {
            //            dataGridView2.DataSource = null;
            //            dtAddTextFile = new DataTable();
            //            AddHeaderDataTableByStoreType(selectedText);
            //            OldValueStoreType = selectedText;
            //        }
            //        else
            //        {
            //            ddStoreType.Text = OldValueStoreType;
            //        }
            //    }
            //    dataGridView2.Refresh();
            //}
            //catch (Exception ex)
            //{
            //    log.Error(String.Format("Exception : {0}", ex.StackTrace));
            //}
        }

        private void AddHeaderDataTableByStoreType(string selectedText)
        {
            try
            {
                if (selectedText.Trim().ToLower().Equals("front"))
                {
                    foreach (DataRow dr in dtHeaderFront.Rows)
                    {
                        dtAddTextFile.Columns.Add(dr["ColumnName"].ToString());
                    }
                }
                else if (selectedText.Trim().ToLower().Equals("back"))
                {
                    foreach (DataRow dr in dtHeaderBack.Rows)
                    {
                        dtAddTextFile.Columns.Add(dr["ColumnName"].ToString());
                    }
                }
                else if (selectedText.Trim().ToLower().Equals("warehouse"))
                {
                    foreach (DataRow dr in dtHeaderWarehouse.Rows)
                    {
                        dtAddTextFile.Columns.Add(dr["ColumnName"].ToString());
                    }
                }
                else if (selectedText.Trim().ToLower().Equals("fresh food"))
                {
                    foreach (DataRow dr in dtHeaderFreshFood.Rows)
                    {
                        dtAddTextFile.Columns.Add(dr["ColumnName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void btnClear_Click(object sender, System.EventArgs e)
        {
            try
            {
                dataGridView2.Refresh();
                if (dataGridView2.Rows.Count > 0)
                {
                    DialogResult dialogResult = MessageBox.Show(MessageConstants.Doyouwanttoclearallfilebelow, MessageConstants.TitleInfomation, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        dataGridView2.DataSource = null;
                        dtAddTextFile = new DataTable();
                        MeargeFileFirstRecordList.Clear();
                        //AddHeaderDataTableByStoreType(ddStoreType.SelectedItem.ToString());
                        //OldValueStoreType = ddStoreType.SelectedItem.ToString();
                        strToGenText = "";
                    }
                }
                else
                {
                    MessageBox.Show(MessageConstants.NoTextFile, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void rdSup_Click(object sender, EventArgs e)
        {
            bindFileName();
        }

        private void rdDept_Click(object sender, EventArgs e)
        {
            bindFileName();
        }

        private void rdFront_Click(object sender, EventArgs e)
        {
            bindFileName();
        }

        private void rdWarehouse_Click(object sender, EventArgs e)
        {
            bindFileName();
        }

        private void rdFreshFood_Click(object sender, EventArgs e)
        {
            bindFileName();
        }

        private void rdONPC_Click(object sender, EventArgs e)
        {
            bindFileName();
        }

        private void rdONAS400_Click(object sender, EventArgs e)
        {
            bindFileName();
        }

        private void bindFileName()
        {
            string FileCode = GetFileCodeByConditionExport();

            if (!(string.IsNullOrWhiteSpace(FileCode)))
            {
                string fileName = "";
                fileName = bll.GetFileNameByFileCode(FileCode);
                txtFileName.Text = fileName;
            }
            else
            {
                MessageBox.Show(MessageConstants.ConditionExportNotMatch, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GetFileCodeByConditionExport()
        {
            string catagory = "";
            string sectionType = "";
            string compareOn = "";

            if (rdSup.Checked)
            {
                catagory = rdSup.Text;
            }
            else if (rdDept.Checked)
            {
                catagory = rdDept.Text;
            }

            if (rdFront.Checked)
            {
                sectionType = rdFront.Text;
            }
            else if (rdWarehouse.Checked)
            {
                sectionType = rdWarehouse.Text;
            }
            else if (rdFreshFood.Checked)
            {
                sectionType = rdFreshFood.Text;
            }

            if (rdONAS400.Checked)
            {
                compareOn = rdONAS400.Text;
            }
            else if (rdONPC.Checked)
            {
                compareOn = rdONPC.Text;
            }

            string FileCode = "";
            if (catagory.Equals(rdSup.Text)
                && sectionType.Equals(rdFront.Text)
                && compareOn.Equals(rdONAS400.Text))
            {
                FileCode = "PCS0009";
            }
            else if (catagory.Equals(rdSup.Text)
                && sectionType.Equals(rdFront.Text)
                && compareOn.Equals(rdONPC.Text))
            {
                FileCode = "PCS0007";
            }
            else if (catagory.Equals(rdSup.Text)
                 && sectionType.Equals(rdWarehouse.Text)
                 && compareOn.Equals(rdONPC.Text))
            {
                FileCode = "PCS0011";
            }
            else if (catagory.Equals(rdSup.Text)
                && sectionType.Equals(rdFreshFood.Text)
                && compareOn.Equals(rdONAS400.Text))
            {
                FileCode = "PCS0012";
            }
            else if (catagory.Equals(rdDept.Text)
               && sectionType.Equals(rdFront.Text)
               && compareOn.Equals(rdONAS400.Text))
            {
                FileCode = "PCS0010";
            }
            else if (catagory.Equals(rdDept.Text)
               && sectionType.Equals(rdFront.Text)
               && compareOn.Equals(rdONPC.Text))
            {
                FileCode = "PCS0004";
            }

            return FileCode;

        }

        private void rdSup_CheckedChanged(object sender, EventArgs e)
        {
            rdFront.Enabled = true;
            rdWarehouse.Enabled = true;
            rdFreshFood.Enabled = true;
            //rdFront.Checked = false;
            //rdWarehouse.Checked = true;
            //rdFreshFood.Checked = false;

            //rdONAS400.Checked = false;
            //rdONPC.Checked = true;
        }

        private void rdDept_CheckedChanged(object sender, EventArgs e)
        {
            rdFront.Enabled = true;
            rdWarehouse.Enabled = false;
            rdFreshFood.Enabled = false;
            //rdFront.Checked = true;
            //rdWarehouse.Checked = false;
            //rdFreshFood.Checked = false;
        }
    }
}
