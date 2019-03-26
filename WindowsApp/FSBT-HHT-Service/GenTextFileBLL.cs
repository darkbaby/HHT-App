using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_Model;
using FSBT_HHT_DAL;
using System.Data;

namespace FSBT_HHT_BLL
{
    public class GenTextFileBll
    {
        private GenTextFileDAO genTextFileDAO = new GenTextFileDAO();
        public List<GenTextFileModel> searchHHT(String hhtname, String sectioncode, String sectionname, String locationfrom, String locationto)
        {
            Console.WriteLine("GENTEXTFILEBLL ACCESS");
            return genTextFileDAO.searchHHT(hhtname, sectioncode, sectionname, locationfrom, locationto);
        }

        public DataTable getHHTStocktakingFrontByCountDate()
        {
            return genTextFileDAO.getHHTStocktakingFrontByCountDate();
        }

        public DataTable getHHTStocktakingBackByCountDate()
        {
            return genTextFileDAO.getHHTStocktakingBackByCountDate();
        }

        public DataTable getHHTStocktakingWarehouseByCountDate()
        {
            return genTextFileDAO.getHHTStocktakingWarehouseByCountDate();
        }

        public DataTable getHHTStocktakingFreshFoodByCountDate()
        {
            return genTextFileDAO.getHHTStocktakingFreshFoodByCountDate();
        }

        public List<FileModel> GetListFileNameAS400()
        {
            return genTextFileDAO.GetListFileNameUploadAS400();
        }

        public List<FileModel> GetListFileNameDownloadAS400()
        {
            return genTextFileDAO.GetListFileNameDownloadAS400();
        }

        public String GetFileNameByFileCode(string fileCode)
        {
            return genTextFileDAO.GetFileNameByFileCode(fileCode);
        }

        public String GetFileCodeByFileID(int fileID)
        {
            return genTextFileDAO.GetFileCodeByFileID(fileID);
        }

        public List<FileModelDetail> GetFileConfigDetail(int fileID)
        {
            return genTextFileDAO.GetFileConfigDetail(fileID);
        }

        public List<FileModelDetail> GetFileConfigDetailByFileCode(string fileCode)
        {
            return genTextFileDAO.GetFileConfigDetailByFileCode(fileCode);
        }

        public DataTable getSearchUploadFile(Request searchCondition)
        {
            return genTextFileDAO.getSearchUploadFile(searchCondition);
        }

        public DataTable getUploadFilePCS0009(String DeptCode)
        {
            return genTextFileDAO.getUploadFilePCS0009(DeptCode);
        }

        public DataTable getUploadFilePCS0011(String DeptCode)
        {
            return genTextFileDAO.getUploadFilePCS0011(DeptCode);
        }

        public DataTable getUploadFilePCS0007(String DeptCode)
        {
            return genTextFileDAO.getUploadFilePCS0007(DeptCode);
        }

        public DataTable getUploadFilePCS0012(String DeptCode)
        {
            return genTextFileDAO.getUploadFilePCS0012(DeptCode);
        }

        public DataTable getUploadFilePCS0004(String DeptCode)
        {
            return genTextFileDAO.getUploadFilePCS0004(DeptCode);
        }

        public DataTable getUploadFilePCS0010(String DeptCode)
        {
            return genTextFileDAO.getUploadFilePCS0010(DeptCode);
        }

        public bool isExistsFileCode(string fileCode)
        {
            return genTextFileDAO.IsExistsFileCode(fileCode);
        }

        public DataTable getSumExportFile(String DeptCode, String FileCode)
        {
            return genTextFileDAO.getSumExportFile(DeptCode,FileCode);
        }

        public int ProcessExportData(DataTable countsheet)
        {
            return genTextFileDAO.ProcessExportData(countsheet);
        }


        public DataSet getTextFileData(Request search)
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            //DataTable SKU = new DataTable("MastSAP_SKU");
            //SKU = genTextFileDAO.getTextFileData(search, "MastSAP_SKU");

            //DataTable Barcode = new DataTable("MastSAP_Barcode");
            //Barcode = genTextFileDAO.getTextFileData(search, "MastSAP_Barcode");

            //DataTable RegularPrice = new DataTable("MastSAP_RegularPrice");
            //RegularPrice = genTextFileDAO.getTextFileData(search, "MastSAP_RegularPrice");

            DataTable HHTStocktaking = new DataTable("HHTStocktaking");
            HHTStocktaking = genTextFileDAO.getTextFileData(search, "HHTStocktaking");

            //DataTable Section = new DataTable("Section");
            //Section = genTextFileDAO.getTextFileData(search, "Section");

            //DataTable Location = new DataTable("Location");
            //Location = genTextFileDAO.getTextFileData(search, "Location");

            //if (SKU.Rows.Count > 0)
            //    ds.Tables.Add(SKU);

            //if (Barcode.Rows.Count > 0)
            //    ds.Tables.Add(Barcode);

            //if (RegularPrice.Rows.Count > 0)
            //    ds.Tables.Add(RegularPrice);

            if (HHTStocktaking.Rows.Count > 0)
                ds.Tables.Add(HHTStocktaking);

            //if (Section.Rows.Count > 0)
            //    ds.Tables.Add(Section);

            //if (Location.Rows.Count > 0)
            //    ds.Tables.Add(Location);

            return ds;
        }

        public List<string> getColumnsName(string tablename)
        {
            return genTextFileDAO.getColumnsName(tablename);
        }

        public bool TruncateTempTextFileData()
        {
            bool returnValue = genTextFileDAO.TruncateTempTextFileData() ;
            return returnValue;
        }

        public DataTable InsertDataTextFile()
        {
            return genTextFileDAO.InsertDataTextFile();
        }

        public DataTable GetExportCountsheet()
        {
            return genTextFileDAO.getAllCountsheet();
        }


        public List<string> GetExportListCountsheet()
        {
            return genTextFileDAO.getListAllCountsheet();
        }
    }
}
