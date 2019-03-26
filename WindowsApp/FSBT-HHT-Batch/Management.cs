using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Reflection;
using FSBT_HHT_DAL.DAO;

namespace FSBT_HHT_Batch
{
    public class Management
    {
        private LogErrorDAO logBll = new LogErrorDAO(); 
        public Hashtable ReadFile(string filePath, List<string> filePathError, string fileNameModifyFirst)
        {
            Hashtable hastResult = new Hashtable();
            ImportModel importData = new ImportModel();
            Database db = new Database();
            List<AuditStocktakingModel> recordData = new List<AuditStocktakingModel>();
            string line = "";
            string[] columns;
            int countRow = 0;

            using (StreamReader sr = new StreamReader(filePath))
            {
                try
                {
                    for (int i = 1; sr.Peek() >= 0; i++)
                    {
                        line = sr.ReadLine();
                        if (i == 1)
                        {
                            importData.HHTID = line;
                        }
                        else if (i == 2)
                        {
                            importData.DeviceName = line;
                        }
                        else if (i == 3)
                        {
                            importData.Mode = line;
                        }
                        else
                        {
                            columns = line.Split(',');

                            string StockTakingID = columns[0];
                            int ScanMode = Convert.ToInt32(columns[1]);
                            string LocationCode = columns[2];
                            string Barcode = columns[3];
                            decimal Quantity = Convert.ToDecimal(columns[4]);
                            int UnitCode = Convert.ToInt32(columns[5]);
                            string Flag = columns[6];
                            string Description = columns[7];
                            string SKUCode = columns[8];
                            string ExBarcode = columns[9];
                            string InBarcode = columns[10];
                            bool SKUMode = Convert.ToBoolean(columns[12]);
                            string CreateDate = columns[13];
                            string CreateBy = columns[14];
                            //string DepartmentCode = columns[14];                                               
                            string SerialNumber = columns[15];
                            string ConversionCounting = columns[16];


                            // 4 is FreshFood
                            //if (ScanMode.Equals(4))
                            //{
                            //    string ProdType = "";
                            //    if (Barcode.Length == 5)
                            //    {
                            //        ProdType = db.GetProductTypeFromMasterSKUBarcode(Barcode, setting);
                            //    }
                            //    else if (Barcode.Length == 18)
                            //    {
                            //        string castBarcode = Barcode.Substring(1);
                            //        castBarcode = castBarcode.Substring(0, 5);
                            //        ProdType = db.GetProductTypeFromMasterSKUBarcode(castBarcode, setting);
                            //    }
                            //    else
                            //    {
                            //        ProdType = db.GetProductTypeFromMasterSKUInExBarcode(InBarcode, ExBarcode, setting);
                            //    }

                            //    if (string.IsNullOrWhiteSpace(ProdType))
                            //    {
                            //        Flag = "F";
                            //    }
                            //    else if (ProdType.Equals("G") || ProdType.Equals("F"))
                            //    {
                            //        UnitCode = 1; // 1 is Piece
                            //    }
                            //    else if(ProdType.Equals("W"))
                            //    {
                            //        Quantity = Quantity * 1000;
                            //        UnitCode = 4; // 4 is gram
                            //    }
                            //}

                            AuditStocktakingModel record = new AuditStocktakingModel();
                            record.StockTakingID = StockTakingID;
                            record.ScanMode = ScanMode;
                            record.LocationCode = LocationCode;
                            record.Barcode = Barcode;
                            record.Quantity = Quantity;
                            record.UnitCode = UnitCode;
                            record.Flag = Flag;
                            record.Description = Description;
                            record.SKUCode = SKUCode;
                            record.ExBarcode = ExBarcode;
                            record.InBarcode = InBarcode;
                            record.SKUMode = SKUMode;
                            record.CreateDate = CreateDate;
                            record.CreateBy = CreateBy;
                            //record.DepartmentCode = DepartmentCode;
                            record.SerialNumber = SerialNumber;
                            record.ConversionCounter = ConversionCounting;

                            recordData.Add(record);
                        }
                        countRow = i;
                    }

                    importData.RecordData = recordData;
                }
                catch (Exception ex)
                {
                    string error = String.Format("Exception :  {0} {1}", fileNameModifyFirst, ex.Message);
                    logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, error, DateTime.Now);
                    filePathError.Add(fileNameModifyFirst);
                    importData.RecordData = new List<AuditStocktakingModel>();
                }

                hastResult.Add("importData", importData);
                hastResult.Add("filePathError", filePathError);
                hastResult.Add("countRow", countRow);

                return hastResult;
            }
        }

        public DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();

            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
