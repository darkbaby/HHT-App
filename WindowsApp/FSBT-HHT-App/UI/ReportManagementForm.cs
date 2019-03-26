using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using FSBT_HHT_BLL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSBT.HHT.App.UI
{
    public partial class ReportManagementForm : Form
    {
private LogErrorBll logBll = new LogErrorBll(); 
        private ReportManagementBll bll = new ReportManagementBll();

        public ReportManagementForm()
        {
            InitializeComponent();
        }

        public bool CreateReport(DataTable dtData, string reportCode)
        {
            try
            {
                ReportDocument docReport = new ReportDocument();
                string startFilePath = Application.StartupPath;
                string parentPath = new DirectoryInfo(startFilePath).Parent.Parent.FullName;
                string reportFile = bll.GetReportFileFromReportCode(reportCode);
                //string filePath = parentPath + "\\ReportTemplate\\ " + reportFile;
                string filePath = Path.GetFullPath("./ReportTemplate/" + reportFile);
                docReport.Load(filePath);
                docReport.SetDataSource(dtData);

                this.ReportManagementReportViewer.ReportSource = docReport;

                return true;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool CreateReport(DataTable dtData, string reportCode, ParameterFields paramFields)
        {
            try
            {
                ReportDocument docReport = new ReportDocument();
                string startFilePath = Application.StartupPath;
                string parentPath = new DirectoryInfo(startFilePath).Parent.Parent.FullName;
                string reportFile = bll.GetReportFileFromReportCode(reportCode).Trim();
                //string filePath = parentPath + "\\ReportTemplate\\" + reportFile;
                /*
                if (reportCode == "R15") 
                {
                    var unitName = paramFields[6].CurrentValues[0].Description;
                    if (unitName == "2")
                    {
                        reportFile = "RPT_InventoryControlDifferenceBySectionWarehousePack.rpt";
                    }
                }
                else if (reportCode == "R17")
                {
                    var unitName = paramFields[6].CurrentValues[0].Description;
                    if (unitName == "2")
                    {
                        reportFile = "RPT_InventoryControlDifferenceByLocationWarehousePack.rpt";
                    }
                }
                */
                if (reportCode == "R13")
                {
                    var scanMode = paramFields[1].CurrentValues[0].Description;
                    if (scanMode == "Front")
                    {
                        reportFile = "RPT_InventoryControlDifferenceByBarcodeFront.rpt";
                    }
                    else if (scanMode == "Back")
                    {
                        reportFile = "RPT_InventoryControlDifferenceByBarcodeFront.rpt";
                    }
                }
                /*
                else if (reportCode == "R19") //14 Inventory Control Difference Report By Barcode (Warehouse)
                {
                    var unitName = paramFields[6].CurrentValues[0].Description;
                    if (unitName == "2")
                    {
                        reportFile = "RPT_InventoryControlDifferenceByBarcodeWarehousePack.rpt";
                    }
                }
                */

                string filePath = Path.GetFullPath("./ReportTemplate/" + reportFile);
                docReport.Load(filePath);
                docReport.SetDataSource(dtData);

                this.ReportManagementReportViewer.ParameterFieldInfo = paramFields;
                this.ReportManagementReportViewer.ReportSource = docReport;

                return true;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool CreateReport(DataSet dtData, string reportCode, ParameterFields paramFields)
        {
            try
            {
                ReportDocument docReport = new ReportDocument();
                string startFilePath = Application.StartupPath;
                string parentPath = new DirectoryInfo(startFilePath).Parent.Parent.FullName;
                string reportFile = bll.GetReportFileFromReportCode(reportCode).Trim();
                //string filePath = parentPath + "\\ReportTemplate\\" + reportFile                

                string filePath = Path.GetFullPath("./ReportTemplate/" + reportFile);
                docReport.Load(filePath);
                docReport.SetDataSource(dtData);

                this.ReportManagementReportViewer.ParameterFieldInfo = paramFields;
                this.ReportManagementReportViewer.ReportSource = docReport;

                return true;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool CreateReport(DataTable dtData, string reportCode, ParameterFields paramFields, string unitCode)
        {
            try
            {
                ReportDocument docReport = new ReportDocument();
                string startFilePath = Application.StartupPath;
                string parentPath = new DirectoryInfo(startFilePath).Parent.Parent.FullName;
                string reportFile = bll.GetReportFileFromReportCode(reportCode).Trim();
                //string filePath = parentPath + "\\ReportTemplate\\" + reportFile;
                if (unitCode == "2")
                {
                    /*
                    if (reportCode == "R15")
                    {
                        reportFile = "RPT_InventoryControlDifferenceBySectionWarehousePack.rpt";
                    }
                    else if (reportCode == "R17")
                    {
                        reportFile = "RPT_InventoryControlDifferenceByLocationWarehousePack.rpt";
                    }
                    else 
                    */
                    if (reportCode == "R14")
                    {
                        reportFile = "RPT_InventoryControlDifferenceByBarcodeWarehousePack.rpt";
                    }
                }

                string filePath = Path.GetFullPath("./ReportTemplate/" + reportFile);
                docReport.Load(filePath);
                docReport.SetDataSource(dtData);

                this.ReportManagementReportViewer.ParameterFieldInfo = paramFields;
                this.ReportManagementReportViewer.ReportSource = docReport;

                return true;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        public bool PrintReport(DataTable dtData, string reportCode, ParameterFields paramFields)
        {
            try
            {
                ReportDocument docReport = new ReportDocument();
                string startFilePath = Application.StartupPath;
                string parentPath = new DirectoryInfo(startFilePath).Parent.Parent.FullName;
                string reportFile = bll.GetReportFileFromReportCode(reportCode).Trim();
                string filePath = Path.GetFullPath("./ReportTemplate/" + reportFile);
                docReport.Load(filePath);
                docReport.SetDataSource(dtData);
                foreach (ParameterField param in paramFields)
                {
                    docReport.SetParameterValue(param.Name, param.CurrentValues);
                }

                PrintDocument printers = new PrintDocument();
                PrinterSettings printer = new PrinterSettings();
                printer = printers.PrinterSettings;
                string printerName = printer.PrinterName;

                docReport.PrintOptions.PrinterName = printerName;
                docReport.PrintToPrinter(1, false, 0, 0);
                docReport.Close();
                docReport.Dispose();
                docReport = null;

                return true;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

    }
}
