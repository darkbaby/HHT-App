using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using FSBT_HHT_DAL;
using FSBT_HHT_DAL.DAO;
using System.Globalization;

namespace FSBT_HHT_BLL
{
    public class ValidatorBll
    {
        public string ValidateResult { get; set; }
        public DataTable DataResult { get; set; }
        public DataTable ErrorResult { get; set; }
        public string ErrorMsg { get; set; }
        public TempFileImportBll tempFileImportBll = new TempFileImportBll();
        public ValidatorBll()
        {

        }

        public static string ValidateFileName(string FileName, string FileFormat)
        {
            string str = "";

            //validate extension
            SystemSettingBll setting = new SystemSettingBll();
            string branchCode = setting.GetSettingStringByKey("BranchCode");
            string StoreID = setting.GetSettingStringByKey("StoreID");
            //string Plant = setting.GetSettingStringByKey("Plant");
            List<string> listPlant = setting.GetAllPlant();

            string ext1 = Path.GetExtension(FileFormat);
            string ext2 = Path.GetExtension(FileName);

            if (ext1.ToLower() != ext2.ToLower())
            {
                str = "Fail:wrong extension";
            }
            else
            {
                FileName = Path.GetFileNameWithoutExtension(FileName).ToLower();
                FileFormat = Path.GetFileNameWithoutExtension(FileFormat).ToLower();

                //Validate name after split 
                string[] temp1 = FileFormat.Split(new Char[] { '_' });
                string[] temp2 = FileName.Split(new Char[] { '_' });

                if (temp2.Length != temp1.Length)
                {
                    str = "Fail:invalid number of file name part";
                }
                else
                {
                    //validate  each part of file name
                    int i = 0;
                    foreach (string tmp in temp1)
                    {
                        if (str == "")
                        {
                            string data = temp2[i];
                            if (tmp == "{date}")
                            {
                                if (!Regex.IsMatch(data, "[\\d]+"))
                                {
                                    str = "Fail:datetime format should be in format  YYYYMMDDHHNNSSFF";
                                }
                                if (str == "")
                                {
                                    if (data.Length != 16)
                                        str = "Fail:datetime format should be in format  YYYYMMDDHHNNSSFF";
                                }
                                if (str == "")
                                {
                                    try
                                    {
                                        DateTime.ParseExact(data, "yyyyMMddHHmmssFF", null);
                                    }
                                    catch
                                    {
                                        str = "Fail:datetime format should be in format  YYYYMMDDHHNNSSFF";
                                    }
                                }
                                //Parse  to check is a real date
                            }
                            else if (tmp == "{date14}")
                            {
                                if (!Regex.IsMatch(data, "[\\d]+"))
                                {
                                    str = "Fail:datetime format should be in format  YYYYMMDDHHNNSS";
                                }
                                if (str == "")
                                {
                                    if (data.Length != 14)
                                        str = "Fail:datetime format should be in format  YYYYMMDDHHNNSS";
                                    else
                                    {
                                        DateTime.ParseExact(data, "yyyyMMddHHmmss", null);
                                    }
                                }
                                //if (str == "")
                                //{
                                //    try
                                //    {
                                //        if (DateTime.ParseExact(DateTime.Now.ToString("yyyyMMdd"), "yyyyMMdd", null) == DateTime.ParseExact(data.Substring(0,8), "yyyyMMdd", null))
                                //        {
                                //            DateTime.ParseExact(data, "yyyyMMddHHmmss", null);
                                //        }
                                //        else
                                //        {
                                //            str = "Fail:datetime should be today";
                                //        }
                                //    }
                                //    catch
                                //    {
                                //        str = "Fail:datetime format should be in format  YYYYMMDDHHNNSS";
                                //    }
                                //}
                                //Parse  to check is a real date
                            }
                            else if (tmp == "{plant}")
                            {
                                //if (!Regex.IsMatch("_"+data+"_", "_[\\dA-Za-z]+_"))
                                //{
                                //    str = "Fail:plant format not correct";
                                //}

                                int count = (from l in listPlant
                                             where l.ToLower().Equals(data.ToLower())
                                             select l).ToList().Count;

                                if (count == 0)
                                    str = "Fail:Incorrect Plant";
                            }
                            else if (tmp == "{storeid}")
                            {
                                //if (!Regex.IsMatch("_" + data + "_", "_[\\dA-Za-z]+_"))
                                //{
                                //    str = "Fail:StoreID format not correct";
                                //}
                                if (data != StoreID)
                                    str = "Fail:Incorrect Store ID";
                            }
                            else if (tmp == "{branch}")
                            {
                                //if (!Regex.IsMatch("_"+data+"_", "_[\\d]+_"))
                                //{
                                //    str = "Fail:branch format not correct";
                                //}
                                if (data.ToLower() != branchCode.ToLower())
                                    str = "Fail:Incorrect Branch";
                            }
                            else if (tmp == "{countsheet}")
                            {
                                if (data.Length != 10)
                                {
                                    str = "Fail:countsheet should be number 10 digits";
                                }
                                else if (!Regex.IsMatch("_" + data + "_", "_[\\d]+_"))
                                {
                                    str = "Fail:countsheet should be number 10 digits";
                                }
                            }
                            else if (!tmp.Contains("{"))
                            {
                                if (data.ToLower() != tmp.ToLower())
                                    str = "Fail:part " + (i + 1) + " should be '" + tmp + "'";
                            }
                        }
                        i++;
                    }
                }
            }
            if (str == "") str = "OK";

            return str;
        }

        private bool IsHavePlantInSetting(string fileformat, string filename)
        {
            string plant = "";
            fileformat = Path.GetFileNameWithoutExtension(fileformat);
            filename = Path.GetFileNameWithoutExtension(filename);

            string[] temp1 = fileformat.Split(new Char[] { '_' });
            string[] temp2 = filename.Split(new Char[] { '_' });

            int i = 0;
            foreach (string tmp in temp1)
            {
                string data = temp2[i];
                if (tmp == "{Plant}")
                {
                    plant = data;
                }
                i++;
            }

            List<MasterPlant> plants = new List<MasterPlant>();
            plants = tempFileImportBll.GetPlant();

            var result = plants.Single(s => s.PlantCode == plant);
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsThisCountsheetExist(string type ,string countsheet)
        {
            List<string> countsheets = new List<string>();
            countsheets = tempFileImportBll.GetMasterCountsheet(type);

            var result = countsheets.Find(s => s == countsheet);
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsThisPlantExist(string plant)
        {
            List<string> plants = new List<string>();
            plants = tempFileImportBll.GetMasterPlant();

            var result = plants.Find(s => s == plant);
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ValidateDataField(int FileID, DataTable dt)
        {
            try
            {
                ConfigFileFormatBll cfg = new ConfigFileFormatBll();

                ConfigFileFormat format = cfg.GetConfigFileFormat(FileID);
                List<ConfigFileFormatDetail> details = cfg.GetConfigFileFormatDetail(FileID);

                var itemToRemove = details.SingleOrDefault(r => r.Field == "TempFileName");
                if (itemToRemove != null)
                    details.Remove(itemToRemove);

                if (format != null)
                {
                    string table = format.TempTableName;
                    TempDataTableBll tempDataTableBll = new TempDataTableBll();
                    dt.Columns.Add(new DataColumn("Error", typeof(string)));
                    DataTable dtResult = dt.Clone();
                    DataTable dtError = dt.Clone();
                    //Add error msg  to  Error Table

                    //Check Number of Columns  should be equal
                    if (dt.Columns.Count - 5 != details.Count)
                    {
                        throw new Exception("the number of  columns incorrect");
                    }

                    //Check Data Type , Length
                    foreach (DataRow dr in dt.Rows)
                    {
                        bool error = false;
                        string str = "";
                        for (int i = 0; i < details.Count; i++)
                        {
                            ConfigFileFormatDetail detail = details[i];

                            string dataInField = dr[i + 1] != null ? dr[i + 1].ToString() : "";
                            string fieldName = dt.Columns[i + 1].ColumnName;

                            //Check if  field name not equals in file format configuration (table=ConfigFileFormatDetail)
                            if (fieldName.ToLower() != detail.Field.ToLower())
                                throw new Exception("Fail:Wrong File Format Configuration");

                            if (dataInField == "" && detail.Required == "Y")
                            {
                                str += "Column : '" + detail.Field + " is required,";
                                error = true;
                            }
                            else if (dataInField != "")
                            {
                                switch (detail.Type)
                                {
                                    case "string":
                                        //string : check only length

                                        if (dataInField.Length > detail.Length)
                                        {
                                            str += "Column : '" + detail.Field + "' Value : '" + dataInField + "' is exceed the maximum length,";                                           
                                            error = true;
                                        }
                                        break;

                                    case "numstring":
                                        int numtest = 0;
                                        if (dataInField.Length > detail.Length)
                                        {
                                            str += "Column : '" + detail.Field + "' Value : '" + dataInField + "' is exceed the maximum length,";
                                            error = true;
                                        }

                                        if (!Int32.TryParse(dataInField, out numtest))
                                        {
                                            str += "Column : '" + detail.Field + "' Value : '" + dataInField + "' should contain only number,";
                                            error = true;
                                        }
                                        break;

                                    case "datestring":

                                        if (dataInField.Length > detail.Length)
                                        {
                                            str += "Column : '" + detail.Field + "' Value : '" + dataInField + "' is exceed the maximum length,";
                                            error = true;
                                        }

                                        try
                                        {
                                            if (dataInField != "00000000")
                                                DateTime.ParseExact(dataInField, "yyyyMMdd", null);
                                        }
                                        catch
                                        {
                                            str += "Column : '" + detail.Field + "' Value : '" + dataInField + "' should be datetime,";
                                            error = true;
                                        }
                                        break;

                                    case "int":
                                        int inttest;

                                        if (!dataInField.Contains("."))
                                        {
                                            if (!Int32.TryParse(dataInField, out inttest))
                                            {
                                                str += "Column : '" + detail.Field + "' Value : '" + dataInField + "' should be interger";
                                                error = true;
                                            }                                                                       
                                        }
                                        else
                                        {
                                            str += "Column : '" + detail.Field + "' Value : '" + dataInField + "' should be interger,";
                                            error = true;
                                        }
                                        break;

                                    case "decimal":
                                        decimal decimalTest;
                                        if (!Decimal.TryParse(dataInField,out decimalTest))
                                        {
                                            str += "Column : '" + detail.Field + "' Value : '" + dataInField + "' should be decimal,";
                                            error = true;
                                        }
                                        break;
                                }
                            }
                        }

                        if (error)
                        {
                            dr["Error"] = str;
                            dtError.ImportRow(dr);
                            error = false;
                        }
                        else
                        {
                            dtResult.ImportRow(dr);
                        }
                    }

                    dtResult.Columns.Remove("Error");
                    DataResult = dtResult;
                    ErrorResult = dtError;
                }
                else throw new Exception("Fail:Not found this file format");
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
        }

        public bool IsNumberOfRecordsInMasterEqualInFile(string type, string filename, string fileformat, string countsheet)
        {
            fileformat = Path.GetFileNameWithoutExtension(fileformat).ToLower();
            filename = Path.GetFileNameWithoutExtension(filename).ToLower();
            bool equal = false;

            TempDataTableDAO tempDAO = new TempDataTableDAO();
            DataTable dt2 = new DataTable();

            List<string> param = new List<string>();
            param.Add(type);
            param.Add(countsheet);
            param.Add(filename);
            dt2 = tempDAO.ExecStoredProcedure("SCR01_SP_CompareCountSheetBetweenMasterAndFile", param);

            if (dt2.Rows.Count > 0)
            {
                foreach (DataRow dr in dt2.Rows)
                {
                    if (dr["equal"].ToString() == "1")
                        equal = true;
                }
            }
            return equal;
        }

        public int ValidateCountSheet(string type, string filename,  string countsheet)
        {
            int result = 0;

            TempDataTableDAO tempDAO = new TempDataTableDAO();
            DataTable dt = new DataTable();
            try
            {
                List<string> param = new List<string>();
                param.Add(type);
                param.Add(filename);
                param.Add(countsheet);
                dt = tempDAO.ExecStoredProcedure("SCR01_SP_ValidateCountSheet", param);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result = Convert.ToInt32(dr["error"].ToString());
                    }
                }

            }
            catch { result = 1; }
            return result;
        }

        private bool IsHavePlantInSetting(string plant)
        {
            List<MasterPlant> plants = new List<MasterPlant>();
            plants = tempFileImportBll.GetPlant();

            var result = plants.Single(s => s.PlantCode == plant);
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int ValidatePlant(string type, string filename, string plant)
        {
            int countError = 0;
            List<string> param = new List<string>();
            try
            {
                param.Add(type);
                param.Add(plant);
                param.Add(filename);
                TempDataTableDAO tempDAO = new TempDataTableDAO();
                DataTable dt = tempDAO.ExecStoredProcedure("SCR01_SP_ValidatePlant", param);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        countError = dr["error"] != null ? Convert.ToInt32(dr["error"]) : 0;
                    }
                }
            }
            catch { countError = 1; }

            return countError;
        }

        public int IsPlantInFileSameWithPlantInFileName(string FileName, string FileFormat)
        {
            int numberOfError = 0;
            var fileformat = Path.GetFileNameWithoutExtension(FileFormat).ToLower();
            var filename = Path.GetFileNameWithoutExtension(FileName).ToLower();
            string plant = "";
            List<string> param = new List<string>();

            TempDataTableDAO tempDAO = new TempDataTableDAO();
            DataTable dt2 = new DataTable();

            if (!string.IsNullOrEmpty(FileFormat))
            {
                string[] temp1 = fileformat.Split(new Char[] { '_' });
                string[] temp2 = filename.Split(new Char[] { '_' });

                int i = 0;
                foreach (string tmp in temp1)
                {
                    string data = temp2[i];
                    if (tmp == "{plant}")
                    {
                        plant = data;
                    }
                    i++;
                }
                param.Add(plant);
                param.Add(FileName);
                dt2 = tempDAO.ExecStoredProcedure("SCR01_SP_CheckPlantWithFileName", param);
            }

            if (dt2.Rows.Count == 1)
            {
                foreach (DataRow dr in dt2.Rows)
                {
                    numberOfError = Convert.ToInt32(dr["error"].ToString());
                }
            }
            return numberOfError;
        }

        public bool IsDuplicateData(int fileFormatID, string filename , ref DataTable dtDuplicate)
        {
            bool error = false;
            try
            {
                ConfigFileFormatBll cfg = new ConfigFileFormatBll();

                ConfigFileFormat format = cfg.GetConfigFileFormat(fileFormatID);
                List<ConfigFileFormatDetail> details = cfg.GetConfigFileFormatDetail(fileFormatID);

                if (format != null)
                {
                    string table = format.TempTableName;
                    string table_error = format.TempTableErrorName;

                    TempDataTableDAO tempDAO = new TempDataTableDAO();
                    List<string> param = new List<string>();
                    param.Add(table);
                    param.Add(table_error);
                    param.Add(fileFormatID.ToString());
                    param.Add(filename);
                    DataTable dt = tempDAO.ExecStoredProcedure("SCR01_SP_ValidateDuplicate", param);

                    if (dt.Rows.Count > 0)
                    {
                        dtDuplicate = dt;
                        error = true;
                    }
                }
            }
            catch (Exception ex)
            {
                error = true;
                dtDuplicate = null;
                ErrorMsg = ex.Message;
            }
            return error;
        }

        public int GetNumberOfRecordsOfCountSheetInMaster(string type, string countsheet)
        {
            int numberOfRecord = 0;
            TempDataTableDAO tempDAO = new TempDataTableDAO();
            try
            {
                DataTable dt = new DataTable();
                List<string> param = new List<string>();
                param.Add(type);
                param.Add(countsheet);
                dt = tempDAO.ExecStoredProcedure("SCR01_SP_GetNumberOfRowOFThisCountSheetFromMaster", param);
            
                if (dt.Rows.Count == 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        numberOfRecord = Convert.ToInt32(dr["CountSheet"].ToString());
                    }
                }
                return numberOfRecord;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
            return numberOfRecord;
        }

        public int GetNumberOfRecordsOfPlantInMaster(string plant)
        {
            int numberOfRecord = 0;
            TempDataTableDAO tempDAO = new TempDataTableDAO();
            try
            {
                DataTable dt = new DataTable();
                List<string> param = new List<string>();
                param.Add(plant);
                dt = tempDAO.ExecStoredProcedure("SCR01_SP_GetNumberOfRowOFThisPlantFromMaster", param);
                if (dt.Rows.Count == 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        numberOfRecord = Convert.ToInt32(dr["plant"].ToString());
                    }
                }
                return numberOfRecord;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
            return numberOfRecord;
        }

        public int ValidateDuplicateData(int fileFormatID, string filename)
        {
            int numberOfRecord = 0;
            try
            {
                ConfigFileFormatBll cfg = new ConfigFileFormatBll();
                ConfigFileFormat format = cfg.GetConfigFileFormat(fileFormatID);
                List<ConfigFileFormatDetail> details = cfg.GetConfigFileFormatDetail(fileFormatID);

                if (format != null)
                {
                    string table = format.TempTableName;
                    string table_error = format.TempTableErrorName;

                    TempDataTableDAO tempDAO = new TempDataTableDAO();
                    List<string> param = new List<string>();
                    param.Add(table);
                    param.Add(table_error);
                    param.Add(fileFormatID.ToString());
                    param.Add(filename);
                    DataTable dt = tempDAO.ExecStoredProcedure("SCR01_SP_ValidateDuplicate", param);

                    if (dt.Rows.Count == 1)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            numberOfRecord = Convert.ToInt32(dr["number"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                numberOfRecord = 1;
            }
            return numberOfRecord;
        }

        public int DeleteMasterPerCountsheet(string type,string countsheet)
        {
            int result = 0;
            TempDataTableDAO tempDAO = new TempDataTableDAO();
            try
            {
                DataTable dt = new DataTable();
                List<string> param = new List<string>();
                param.Add(type);
                param.Add(countsheet);
                dt = tempDAO.ExecStoredProcedure("SCR01_SP_DeleteMasterDataPerCountsheet", param);
                if (dt.Rows.Count == 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result = Convert.ToInt32(dr["result"].ToString());
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
            return result;
        }

        public int DeleteMasterPerPlant(string plant)
        {
            int result = 0;
            TempDataTableDAO tempDAO = new TempDataTableDAO();
            try
            {
                DataTable dt = new DataTable();
                List<string> param = new List<string>();
                param.Add(plant);
                dt = tempDAO.ExecStoredProcedure("SCR01_SP_DeleteMasterDataPerPlant", param);
                if (dt.Rows.Count == 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result = Convert.ToInt32(dr["result"].ToString());
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
            return result;
        }
    }
}
