using FSBT_HHT_DAL;
using FSBT_HHT_DAL.DAO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace FSBT_HHT_BLL
{
    public class TempFileImportBll
    {
        TempFileImportDAO tempFileImportDAO = new TempFileImportDAO();
        public string errorMeassge { get; set; }

        public bool DeleteTempData(string tableName)
        {
            bool returnValue = tempFileImportDAO.DeleteTempData(tableName);
            errorMeassge = tempFileImportDAO.errorMeassge;
            //return tempFileImportDAO.DeleteTempData(tableName);
            return returnValue;
        }

        public bool InsertDataTableToDatabase(DataTable dt, string tableName)
        {
            bool returnValue = tempFileImportDAO.InsertDataTableToDatabase(dt, tableName);
            errorMeassge = tempFileImportDAO.errorMeassge;
         //   return tempFileImportDAO.InsertDataTableToDatabase(dt, tableName);
            return returnValue;
        }



        public string[] GetColumnName(string tableName)
        {
            return tempFileImportDAO.GetColumnName(tableName);
        }

        public List<MasterPlant> GetPlant()
        {
            return tempFileImportDAO.GetPlant();
        }

        public List<string> GetMasterPlant()
        {
            return tempFileImportDAO.GetMasterPlant();
        }

        public List<string> Plants()
        {
            return tempFileImportDAO.Plants();
        }

        public bool IsHaveSettingPlants()
        {
            return tempFileImportDAO.IsHaveSettingPlants();
        }

        public List<TempFileSKUDetail> GetSKUFileDetail()
        {
            return tempFileImportDAO.GetSKUFileDetail();
        }

        public List<TempFileSKUDetail> GetSKUFileDetail(string filename)
        {
            return tempFileImportDAO.GetSKUFileDetail(filename);
        }

        public List<TempFileSKUError> GetSKUFileError()
        {
            return tempFileImportDAO.GetSKUFileError();
        }

        public List<TempFileSKULineError> GetSKUFileLineError()
        {
            return tempFileImportDAO.GetSKUFileLineError();
        }

        public List<string> GetMasterCountsheet(string type)
        {
            return tempFileImportDAO.GetMasterCountsheet(type); 
        }

        public List<TempFileBarcodeDetail> GetBarcodeFileDetail()
        {
            return tempFileImportDAO.GetBarcodeFileDetail();
        }

        public List<TempFileBarcodeDetail> GetBarcodeFileDetail(string filename)
        {
            return tempFileImportDAO.GetBarcodeFileDetail(filename);
        }

        public List<TempFileBarcodeError> GetBarcodeFileError()
        {
            return tempFileImportDAO.GetBarcodeFileError();
        }

        public List<TempFileBarcodeLineError> GetBarcodeFileLineError()
        {
            return tempFileImportDAO.GetBarcodeFileLineError();
        }

        public List<TempFileRegularPriceDetail> GetRegularPriceFileDetail()
        {
            return tempFileImportDAO.GetRegularPriceFileDetail();
        }

        public List<TempFileRegularPriceDetail> GetRegularPriceFileDetail(string filename)
        {
            return tempFileImportDAO.GetRegularPriceFileDetail(filename);
        }
        public List<TempFileRegularPriceError> GetRegularPriceFileError()
        {
            return tempFileImportDAO.GetRegularPriceError();
        }

        public List<TempFileRegularPriceLineError> GetRegularPriceFileLineError()
        {
            return tempFileImportDAO.GetRegularPriceLineError();
        }

        public int GetMaxIDTempTableDetail(string tableName)
        {
            int id = 0;

            List<string> param = new List<string>();
            param.Add(tableName);
            DataTable dt = tempFileImportDAO.ExecStoredProcedure("SCR01_SP_GETMaxIDTableDetail", param);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    id = dr["id"] != null ? Convert.ToInt32(dr["id"]) : 0;
                }
            }
            return id;
        }
    }
}
