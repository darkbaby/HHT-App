using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_Model;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System.IO;
using System.Windows.Forms;
using FSBT_HHT_DAL;

namespace FSBT_HHT_BLL
{
    public class DownloadMasterDataBll
    {
        DownloadMasterDAO downloadMasterDAO = new DownloadMasterDAO();
        static GenTextFileDAO genTextFileDAO = new GenTextFileDAO();
        List<FileModel> fileAS400 = genTextFileDAO.GetListFileNameDownloadAS400();
        private string fileSKUName = "PCS0008 - Download File for Stocktaking (SP)";
        private string fileSKUFFName = "PCS0013-Download File for Stocktaking(ตลาดสด)";
        private string fileBarcodeName = "UPCR01P (POSFLIB) - Table file more than one Barcode (SP)";
        private string filePackName = "TABBOXSP (POSFLIB) - Table file of barcode (box) -S/P";
        private string fileBrandName = "PCS0006-Brand/Group Description for The Mall";
        public string errorMessage = "";
        public DownloadMasterDataBll()
        {

        }

        public Hashtable AddDataToDatabase(string tableType, List<string> file, int flg, DataTable resultData, string catagoryFlg)
        {
            Hashtable hashResult = new Hashtable();
            string deleteResult = "";
            string addResult = "";
            DataTable masterData = ConvertToDataTable(tableType, file, flg, catagoryFlg);
            if (masterData == null)
            {
                hashResult.Add("result", "wrongformat");
                return hashResult;
            }
            //bool validateNull = ValidateNull(masterData, tableType, flg);
            //if (!validateNull)
            //{
            //    return "null";
            //}
            bool validateData = ValidateData(masterData, tableType, flg);
            if (!validateData)
            {
                hashResult.Add("result", "wrongdata");
                return hashResult;
            }
            deleteResult = downloadMasterDAO.DeleteMasterWithFlg(tableType, flg);
            if (deleteResult != "error")
            {
                switch (tableType)
                {
                    case "SKU":
                        switch (flg)
                        {
                            case 1:
                                addResult = downloadMasterDAO.AddDataToMasterSKU_Front(masterData);
                                break;
                            //case 2:
                            //    addResult = downloadMasterDAO.AddDataToMasterSKU_Back(masterData);
                            //    break;
                            case 3:
                                addResult = downloadMasterDAO.AddDataToMasterSKU_Stock(masterData);
                                break;
                            case 4:
                                addResult = downloadMasterDAO.AddDataToMasterSKU_FreshFood(masterData);
                                break;
                        }
                        break;
                    case "Barcode":
                        addResult = downloadMasterDAO.AddDataToMasterBarcode(masterData);
                        break;
                    case "PackBarcode":
                        addResult = downloadMasterDAO.AddDateToMasterPack(masterData);
                        break;
                    case "Brand":
                        addResult = downloadMasterDAO.AddDataToMasterBrand(masterData);
                        break;
                }
            }
            if (addResult == "success")
            {
                hashResult.Add("result", "success");
                hashResult.Add("resultTable", masterData);
                return hashResult;
            }
            else
            {
                hashResult.Add("result", "error");
                return hashResult;
            }
        }

        private DataTable ConvertToDataTable(string tableType, List<string> file, int flg, string catagoryFlg)
        {
            DataTable tbl = new DataTable();
            switch (tableType)
            {
                case "SKU":
                    switch (flg)
                    {
                        case 1:
                        case 2:
                        case 3:
                            tbl = ConvertSKUFront(file, flg, catagoryFlg);
                            break;
                        case 4:
                            tbl = ConvertSKUFreshFood(file, catagoryFlg);
                            break;
                    }
                    break;
                case "Barcode":
                    tbl = ConvertBarcode(file, flg);
                    break;
                case "PackBarcode":
                    {
                        tbl = ConvertPack(file);
                        break;
                    }
                case "Brand":
                    {
                        tbl = ConvertBrand(file);
                        break;
                    }
            }
            return tbl;
        }

        private DataTable ConvertSKUFront(List<string> file, int flg, string catagoryFlg)
        {
            try
            {
                DataTable tbl = new DataTable();
                int fileSKUID = Int32.Parse(fileAS400.Where(x => x.fileName.Equals(fileSKUName)).Select(x => x.fileID).FirstOrDefault().ToString());
                List<FileModelDetail> AS400Detail = genTextFileDAO.GetFileConfigDetail(fileSKUID);
                int numberOfColumns = AS400Detail.Count();

                for (int col = 0; col < numberOfColumns; col++)
                {
                    tbl.Columns.Add(new DataColumn(AS400Detail[col].Description));
                }
                tbl.Columns.Add(new DataColumn("CreateDate"));
                tbl.Columns.Add(new DataColumn("CreateBy"));
                string departmentInFile = string.Empty;
                foreach (string line in file)
                {
                    departmentInFile = string.Empty;
                    DataRow dr = tbl.NewRow();
                    int i;
                    for (i = 0; i < AS400Detail.Count; i++)
                    {
                        if (i == 8 && flg == 1 && catagoryFlg == "Super")
                        {
                            dr[i] = "97";
                        }
                        else if (i == 8 && flg == 3 && catagoryFlg == "Super")
                        {
                            departmentInFile = line.Substring(AS400Detail[i].StartPos - 1, AS400Detail[i].Length + AS400Detail[i].DecPos).Trim();
                            dr[i] = "98";
                        }
                        else
                        {
                            dr[i] = line.Substring(AS400Detail[i].StartPos - 1, AS400Detail[i].Length + AS400Detail[i].DecPos).Trim();
                        }

                    }

                    if (catagoryFlg == "Department" && flg == 1)
                    {
                        //depart+brand
                        if (dr[2] != string.Empty)
                        {
                            dr[2] = ("00" + dr[8]).Substring(("00" + dr[8]).Length - 2) + ("000" + dr[2]).Substring(("000" + dr[2]).Length - 3);
                        }
                    }
                    else if (catagoryFlg == "Super" && flg == 1)
                    {
                        //97+brand
                        if (dr[2] != string.Empty)
                        {
                            dr[2] = "97" + ("000" + dr[2]).Substring(("000" + dr[2]).Length - 3);
                        }
                    }
                    else if (catagoryFlg == "Super" && flg == 3)
                    {
                        //98+Depart
                        if (departmentInFile == string.Empty)
                        {
                            dr[2] = string.Empty;
                        }
                        else
                        {
                            if (departmentInFile.Length == 1)
                            {
                                dr[2] = departmentInFile = "9800" + departmentInFile;
                            }
                            else if (departmentInFile.Length == 2)
                            {
                                dr[2] = departmentInFile = "980" + departmentInFile;
                            }
                            else
                            {
                                dr[2] = departmentInFile = "98" + departmentInFile;
                            }
                        }
                    }
                    //}
                    ////dr[11] = flg;
                    dr[i] = DateTime.Now;
                    dr[i + 1] = "Admin";

                    tbl.Rows.Add(dr);
                }

                return tbl;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DataTable ConvertSKUFreshFood(List<string> file, string catagoryFlg)
        {
            try
            {
                DataTable tbl = new DataTable();
                int fileSKUID = Int32.Parse(fileAS400.Where(x => x.fileName.Equals(fileSKUFFName)).Select(x => x.fileID).FirstOrDefault().ToString());
                List<FileModelDetail> AS400Detail = genTextFileDAO.GetFileConfigDetail(fileSKUID);
                int numberOfColumns = AS400Detail.Count();

                for (int col = 0; col < numberOfColumns; col++)
                {
                    tbl.Columns.Add(new DataColumn(AS400Detail[col].Description));
                }
                tbl.Columns.Add(new DataColumn("CreateDate"));
                tbl.Columns.Add(new DataColumn("CreateBy"));
                string departmentInFile = string.Empty;
                foreach (string line in file)
                {
                    departmentInFile = string.Empty;
                    DataRow dr = tbl.NewRow();
                    int i;
                    for (i = 0; i < AS400Detail.Count; i++)
                    {
                        if (i == 10 && catagoryFlg == "Super")
                        {
                            departmentInFile = line.Substring(AS400Detail[i].StartPos - 1, AS400Detail[i].Length + AS400Detail[i].DecPos).Trim();
                            dr[i] = "99";
                        }
                        else
                        {
                            dr[i] = line.Substring(AS400Detail[i].StartPos - 1, AS400Detail[i].Length + AS400Detail[i].DecPos).Trim();
                        }
                    }
                    //if (dr[2] != string.Empty)
                    //{
                    //    dr[2] = ("00" + dr[10]).Substring(("00" + dr[10]).Length - 2) + ("000" + dr[2]).Substring(("000" + dr[2]).Length - 3);
                    //}
                    //if (dr[2] != string.Empty)
                    //{
                    //99+Depart
                    if (departmentInFile == string.Empty)
                    {
                        dr[2] = string.Empty;
                    }
                    else
                    {
                        //dr[2] = "99" + departmentInFile;
                        if (departmentInFile.Length == 1)
                        {
                            dr[2] = departmentInFile = "9900" + departmentInFile;
                        }
                        else if (departmentInFile.Length == 2)
                        {
                            dr[2] = departmentInFile = "990" + departmentInFile;
                        }
                        else
                        {
                            dr[2] = departmentInFile = "99" + departmentInFile;
                        }
                    }
                    //}


                    //dr[11] = flg;
                    dr[i] = DateTime.Now;
                    dr[i + 1] = "Admin";

                    tbl.Rows.Add(dr);
                }

                return tbl;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DataTable ConvertBarcode(List<string> file, int flg)
        {
            try
            {
                DataTable tbl = new DataTable();
                int fileBarcodeID = Int32.Parse(fileAS400.Where(x => x.fileName.Equals(fileBarcodeName)).Select(x => x.fileID).FirstOrDefault().ToString());
                List<FileModelDetail> AS400Detail = genTextFileDAO.GetFileConfigDetail(fileBarcodeID);
                int numberOfColumns = AS400Detail.Count();
                for (int col = 0; col < numberOfColumns; col++)
                {
                    tbl.Columns.Add(new DataColumn(AS400Detail[col].Description));
                }
                tbl.Columns.Add(new DataColumn("ScanMode"));
                tbl.Columns.Add(new DataColumn("CreateDate"));
                tbl.Columns.Add(new DataColumn("CreateBy"));

                foreach (string line in file)
                {
                    DataRow dr = tbl.NewRow();
                    int i;
                    for (i = 0; i < AS400Detail.Count; i++)
                    {
                        dr[i] = line.Substring(AS400Detail[i].StartPos - 1, AS400Detail[i].Length + AS400Detail[i].DecPos).Trim();
                    }

                    dr[i] = flg;
                    dr[i + 1] = DateTime.Now;
                    dr[i + 2] = "Admin";

                    tbl.Rows.Add(dr);
                }

                return tbl;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DataTable ConvertPack(List<string> file)
        {
            try
            {
                DataTable tbl = new DataTable();
                int filePackID = Int32.Parse(fileAS400.Where(x => x.fileName.Equals(filePackName)).Select(x => x.fileID).FirstOrDefault().ToString());
                List<FileModelDetail> AS400Detail = genTextFileDAO.GetFileConfigDetail(9);
                int numberOfColumns = AS400Detail.Count();

                for (int col = 0; col < numberOfColumns; col++)
                {
                    tbl.Columns.Add(new DataColumn(AS400Detail[col].Description));
                }
                tbl.Columns.Add(new DataColumn("CreateDate"));
                tbl.Columns.Add(new DataColumn("CreateBy"));

                foreach (string line in file)
                {
                    DataRow dr = tbl.NewRow();
                    int i;
                    for (i = 0; i < AS400Detail.Count; i++)
                    {
                        dr[i] = line.Substring(AS400Detail[i].StartPos - 1, AS400Detail[i].Length + AS400Detail[i].DecPos).Trim();
                    }

                    ////dr[11] = flg;
                    dr[i] = DateTime.Now;
                    dr[i + 1] = "Admin";

                    tbl.Rows.Add(dr);
                }

                return tbl;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DataTable ConvertBrand(List<string> file)
        {
            try
            {
                DataTable tbl = new DataTable();
                int fileBrandID = Int32.Parse(fileAS400.Where(x => x.fileName.Equals(fileBrandName)).Select(x => x.fileID).FirstOrDefault().ToString());
                List<FileModelDetail> AS400Detail = genTextFileDAO.GetFileConfigDetail(fileBrandID);
                int numberOfColumns = AS400Detail.Count();

                for (int col = 0; col < numberOfColumns; col++)
                {
                    tbl.Columns.Add(new DataColumn(AS400Detail[col].Description));
                }
                tbl.Columns.Add(new DataColumn("CreateDate"));
                tbl.Columns.Add(new DataColumn("CreateBy"));

                foreach (string line in file)
                {
                    DataRow dr = tbl.NewRow();
                    int i;
                    for (i = 0; i < AS400Detail.Count; i++)
                    {
                        dr[i] = line.Substring(AS400Detail[i].StartPos - 1, AS400Detail[i].Length + AS400Detail[i].DecPos).Trim();
                    }
                    dr[0] = ("00" + dr[2]).Substring(("00" + dr[2]).Length - 2) + ("000" + dr[0]).Substring(("000" + dr[0]).Length - 3);
                    ////dr[11] = flg;
                    dr[i] = DateTime.Now;
                    dr[i + 1] = "Admin";

                    tbl.Rows.Add(dr);
                }

                return tbl;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetSummarySubDepartment(int flg)
        {
            return downloadMasterDAO.GetMasterSummarySubDepartment(flg);
        }

        public DataTable GetSummaryByBrand(int flg)
        {
            return downloadMasterDAO.GetMasterSummaryByBrand(flg);
        }
        public DataTable GetMasterDownload(int flg, string dataType)
        {
            return downloadMasterDAO.GetMasterDownload(flg, dataType);
        }
        public DataTable GetSummaryMaterialGroup(int flg)
        {
            return downloadMasterDAO.GetMasterSummaryMaterialGroup(flg);
        }
        public DataTable GetSummaryStorageLocation(int flg)
        {
            return downloadMasterDAO.GetMasterSummaryStorageLocation(flg);
        }
        public bool ClearDataTable()
        {
            string result = downloadMasterDAO.DeleteAllMaster();
            if (result == "success")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ClearMasterSAPDataTable()
        {
            string result = downloadMasterDAO.DeleteMasterSAP();
            if (result == "success")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidateNull(DataTable masterData, string tableType, int flg)
        {
            switch (tableType)
            {
                case "SKU":
                    switch (flg)
                    {
                        case 1:
                        case 2:
                            {
                                foreach (DataColumn column in masterData.Columns)
                                {
                                    if (column.ColumnName == "Column3" ||
                                        column.ColumnName == "Column4" ||
                                        column.ColumnName == "Column6" ||
                                        column.ColumnName == "Column7" ||
                                        column.ColumnName == "Column8" ||
                                        column.ColumnName == "Column9" ||
                                        column.ColumnName == "Column10")
                                    {
                                        continue;
                                    }
                                    foreach (DataRow row in masterData.Rows)
                                    {
                                        if (row[column].ToString() == "")
                                        {
                                            return false;
                                        }
                                    }
                                }
                                return true;
                            }
                        case 3:
                            {
                                foreach (DataColumn column in masterData.Columns)
                                {
                                    if (column.ColumnName == "Column4" ||
                                        column.ColumnName == "Column5" ||
                                        column.ColumnName == "Column6" ||
                                        column.ColumnName == "Column7" ||
                                        column.ColumnName == "Column8" ||
                                        column.ColumnName == "Column10" ||
                                        column.ColumnName == "Column11")
                                    {
                                        continue;
                                    }
                                    foreach (DataRow row in masterData.Rows)
                                    {
                                        if (row[column].ToString() == "")
                                        {
                                            return false;
                                        }
                                    }
                                }
                                return true;
                            }
                        case 4:
                            {
                                foreach (DataColumn column in masterData.Columns)
                                {
                                    if (column.ColumnName == "Column4" ||
                                        column.ColumnName == "Column5" ||
                                        column.ColumnName == "Column6" ||
                                        column.ColumnName == "Column7" ||
                                        column.ColumnName == "Column8" ||
                                        column.ColumnName == "Column10" ||
                                        column.ColumnName == "Column11" ||
                                        column.ColumnName == "Column12")
                                    {
                                        continue;
                                    }
                                    foreach (DataRow row in masterData.Rows)
                                    {
                                        if (row[column].ToString() == "")
                                        {
                                            return false;
                                        }
                                    }
                                }
                                return true;
                            }
                    }
                    return false;
                case "Barcode":
                    foreach (DataColumn column in masterData.Columns)
                    {
                        foreach (DataRow row in masterData.Rows)
                        {
                            if (row[column].ToString() == "")
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                case "PackBarcode":
                    return false;
                case "Brand":
                    return false;
            }
            return false;
        }

        private bool ValidateData(DataTable masterData, string tableType, int flg)
        {
            switch (tableType)
            {
                case "SKU":
                    switch (flg)
                    {
                        case 1:
                        case 2:
                        case 3:
                            {
                                int fileSKU1ID = Int32.Parse(fileAS400.Where(x => x.fileName.Equals(fileSKUName)).Select(x => x.fileID).FirstOrDefault().ToString());
                                List<FileModelDetail> AS400Detail1 = genTextFileDAO.GetFileConfigDetail(fileSKU1ID);
                                foreach (DataRow row in masterData.Rows)
                                {
                                    for (int i = 0; i < AS400Detail1.Count; i++)
                                    {
                                        if (AS400Detail1[i].Type == "S")
                                        {
                                            int test;
                                            bool tryInt = Int32.TryParse(row[i].ToString(), out test);
                                            if (!tryInt)
                                            {
                                                return false;
                                            }
                                        }
                                        else if (AS400Detail1[i].Type == "P")
                                        {
                                            decimal test;
                                            bool tryDecimal = System.Decimal.TryParse(row[i].ToString(), out test);
                                            if (!tryDecimal)
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                                return true;
                            }
                        case 4:
                            {
                                int fileSKU2ID = Int32.Parse(fileAS400.Where(x => x.fileName.Equals(fileSKUFFName)).Select(x => x.fileID).FirstOrDefault().ToString());
                                List<FileModelDetail> AS400Detail2 = genTextFileDAO.GetFileConfigDetail(fileSKU2ID);
                                foreach (DataRow row in masterData.Rows)
                                {
                                    for (int i = 0; i < AS400Detail2.Count; i++)
                                    {
                                        if (AS400Detail2[i].Type == "S")
                                        {
                                            int test;
                                            bool tryInt = Int32.TryParse(row[i].ToString(), out test);
                                            if (!tryInt)
                                            {
                                                return false;
                                            }
                                        }
                                        else if (AS400Detail2[i].Type == "P")
                                        {
                                            decimal test;
                                            bool tryDecimal = System.Decimal.TryParse(row[i].ToString(), out test);
                                            if (!tryDecimal)
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                                return true;
                            }
                    }
                    return false;
                case "Barcode":
                    //int fileBarcodeID = Int32.Parse(fileAS400.AsEnumerable().Where(x => x.fileName.Equals(fileBarcodeName)).FirstOrDefault().ToString());
                    //List<FileModelDetail> AS400Detail3 = genTextFileDAO.GetFileConfigDetail(fileBarcodeID);
                    return true;
                case "PackBarcode":
                    int filePackID = Int32.Parse(fileAS400.Where(x => x.fileName.Equals(filePackName)).Select(x => x.fileID).FirstOrDefault().ToString());
                    List<FileModelDetail> AS400Detail4 = genTextFileDAO.GetFileConfigDetail(filePackID);
                    foreach (DataRow row in masterData.Rows)
                    {
                        for (int i = 0; i < AS400Detail4.Count; i++)
                        {
                            if (AS400Detail4[i].Type == "S")
                            {
                                int test;
                                bool tryInt = Int32.TryParse(row[i].ToString(), out test);
                                if (!tryInt)
                                {
                                    return false;
                                }
                            }
                            else if (AS400Detail4[i].Type == "P")
                            {
                                decimal test;
                                bool tryDecimal = System.Decimal.TryParse(row[i].ToString(), out test);
                                if (!tryDecimal)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                case "Brand":
                    return true;
            }
            return false;
        }

        private bool ValidateDepartment(DataTable masterData, string tableType, int flg, string department)
        {
            int column;
            switch (tableType)
            {
                case "SKU":
                    switch (flg)
                    {
                        case 1:
                        case 2:
                        case 3:
                            {
                                column = 9;
                                foreach (DataRow row in masterData.Rows)
                                {
                                    if (row[column].ToString() != department)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                        case 4:
                            {
                                column = 11;
                                foreach (DataRow row in masterData.Rows)
                                {
                                    if (row[column].ToString() != department)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                    }
                    return false;
                case "Barcode":
                    {
                        column = 6;
                        foreach (DataRow row in masterData.Rows)
                        {
                            if (row[column].ToString() != department)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                case "PackBarcode":
                    {
                        column = 2;
                        foreach (DataRow row in masterData.Rows)
                        {
                            if (row[column].ToString() != department)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                case "Brand":
                    {
                        column = 3;
                        foreach (DataRow row in masterData.Rows)
                        {
                            if (row[column].ToString() != department)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
            }
            return false;
        }

        public DataTable ExportPDF123(int flg)
        {
            DataTable searchResult = downloadMasterDAO.SelectDataForExport(flg);
            //searchResult.Rows.RemoveAt(0);
            return searchResult;
        }

        public DataTable ExportPDF4(int flg)
        {
            DataTable searchResult = downloadMasterDAO.SelectDataForExportFreshFood();
            //searchResult.Rows.RemoveAt(0);
            return searchResult;
        }

        public void ExportExcel(string tableType, int flg)
        {
            DataTable searchResult = downloadMasterDAO.SelectDataForExport(flg);
            //string startFilePath = Application.StartupPath;
            //string parentPath = new DirectoryInfo(startFilePath).Parent.Parent.FullName;
            string parentPath = @"D:\Project FSBT-HHT";
            string filePath = @parentPath + tableType + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmm") + ".xls";
            CreateEmptyExcelFile(filePath);
            CreateEmptyWorkSheet("Sheet1", filePath);
            writeSheetIndexExcelNoHeader(searchResult, 0, filePath);
        }

        private void CreateEmptyExcelFile(string filepath)
        {
            HSSFWorkbook wkbook = new HSSFWorkbook();
            wkbook.CreateName();
            using (FileStream filesave = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                wkbook.Write(filesave);
            }

        }

        public void CreateEmptyWorkSheet(string sheetName, string filepath)
        {
            HSSFWorkbook wkbook;

            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.ReadWrite))
            {
                wkbook = new HSSFWorkbook(file);
            }

            wkbook.CreateSheet(sheetName);

            using (FileStream filesave = new FileStream(filepath, FileMode.Create))
            {
                wkbook.Write(filesave);
            }
        }

        public void writeSheetIndexExcelNoHeader(DataTable dataTable, int Ind, string filepath)
        {
            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.ReadWrite))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            int latestRow = 0;
            int rowCount = dataTable.Rows.Count;
            ISheet sheet;

            for (int excelRow = 0; excelRow < rowCount; excelRow++)
            {
                HSSFSheet sh;
                sh = (HSSFSheet)hssfworkbook.GetSheetAt(Ind);
                if (sh.GetRow(excelRow) == null)
                    sh.CreateRow(excelRow);
                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    string cellValue = dataTable.Rows[excelRow][col].ToString();
                    if (cellValue != null)
                    {
                        if (sh.GetRow(excelRow).GetCell(col) == null)
                        {
                            sh.GetRow(excelRow).CreateCell(col);
                        }
                        sh.GetRow(excelRow).GetCell(col).SetCellValue(cellValue.ToString());
                    }
                }
            }
            GC.Collect();
            using (FileStream filesave = new FileStream(filepath, FileMode.Open, FileAccess.ReadWrite))
            {
                hssfworkbook.Write(filesave);
            }

        }

        //public int CheckNewCountsheet(string Type, DataTable dtTemp)
        //{

        //    int newCountsheet = 0;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        List<string> param = new List<string>();
        //        TempDataTableDAO tb = new TempDataTableDAO();
        //        param.Add(Type);

        //        dt = tb.ExecStoredProcedure("SCR01_SP_CheckCountSheet", param);

        //        string countsheet = "";
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            countsheet = dr["CountSheet"].ToString();
        //        }

        //        if (countsheet == "")
        //            newCountsheet = 2;
        //        else
        //        {
        //            string tempCountsheet = dtTemp.Rows[0]["PIDoc"].ToString();
        //            if (tempCountsheet != countsheet) newCountsheet = 1;
        //            else newCountsheet = 0;
        //        }

        //        //newCountsheet   : 1= New countsheet , 0 = Old countsheet , 


        //        // TempDataTableDAO 
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage = ex.Message;
        //    }

        //    return newCountsheet;
        //}

        public List<string> GetCountsheet(string Type)
        {
            List<string> countsheets = new List<string>();
            DataTable dt = new DataTable();
            try
            {
                List<string> param = new List<string>();
                TempDataTableDAO tb = new TempDataTableDAO();
                param.Add(Type);

                dt = tb.ExecStoredProcedure("SCR01_SP_GetCountSheetFromMaster", param);

                foreach (DataRow dr in dt.Rows)
                {
                    countsheets.Add(dr["CountSheet"].ToString());
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return countsheets;
        }

        public List<string> GetFileNameFromMaster(string Type)
        {
            List<string> filename = new List<string>();
            DataTable dt = new DataTable();
            try
            {
                List<string> param = new List<string>();
                TempDataTableDAO tb = new TempDataTableDAO();
                param.Add(Type);

                dt = tb.ExecStoredProcedure("SCR01_SP_GetFileNameFromMaster", param);

                foreach (DataRow dr in dt.Rows)
                {
                    filename.Add(dr["FileName"].ToString());
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return filename;
        }

        public string CheckFileExistsRegularPrice(string filename)
        {
            string result = "";
            DataTable dt = new DataTable();
            try
            {
                List<string> param = new List<string>();
                TempDataTableDAO tb = new TempDataTableDAO();
                param.Add(filename);

                dt = tb.ExecStoredProcedure("SCR01_SP_CheckIsExistsFileRegularPrice", param);

                foreach (DataRow dr in dt.Rows)
                {
                    result = dr[0].ToString();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return result;
        }

        public int ConvertMasterFromTempToReal(string Type)
        {
            DataTable dt = new DataTable();
            int result = 0;
            try
            {
                List<string> param = new List<string>();
                TempDataTableDAO tb = new TempDataTableDAO();
                param.Add(Type);
                dt = tb.ExecStoredProcedure("SCR01_SP_DownloadMaster", param);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result = Convert.ToInt32(dr["complete"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return result;
        }

        public DataTable GetSerialAfterDownload()
        {
            return downloadMasterDAO.GetSerialAfterDownload();
        }

        public DataTable DownloadMCHLevelFromMasterSAP(ref string errorMessage)
        {
            DataTable dt = new DataTable();
            try
            {
                TempDataTableDAO tb = new TempDataTableDAO();
                List<string> param = new List<string>();
                dt = tb.ExecStoredProcedure("SCR01_SP_DownloadMCHLevel", param);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return dt;
        }

        public List<LogError> GetLogAll()
        {
            return downloadMasterDAO.GetLogAll();
        }

    }
}
