using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using FSBT_HHT_Model;
using System.IO;
using FSBT_HHT_BLL;
using System.Reflection;

namespace FSBT.HHT.App.UI
{
    public partial class DeleteQtyReportForm : Form
    {
private LogErrorBll logBll = new LogErrorBll(); 
        public DeleteQtyReportForm()
        {     
            InitializeComponent();       
        }
        SystemSettingBll bllSetting = new FSBT_HHT_BLL.SystemSettingBll();
        public bool CreateReport(DataTable deleteData, DateTime countDate,string branchName)
        {
            try
            {
                ReportManagementBll bllReportManagement = new ReportManagementBll();
                ReportDocument deleteQtyReport = new ReportDocument();
                string startFilePath = Application.StartupPath;
                //string filePath = Path.GetFullPath("./ReportTemplate/RPT_DeleteRecordReportByLocation.rpt");

                string reportFile = bllReportManagement.GetReportURLByReportCode("R11");
                string reportName = bllReportManagement.GetReportNameByReportCode("R11");
                string filePath = Path.GetFullPath("./ReportTemplate/" + reportFile); 
  
                deleteQtyReport.Load(filePath);
                deleteQtyReport.SetDataSource(deleteData);
                deleteQtyReport.SetParameterValue("pCountDate", countDate);
                deleteQtyReport.SetParameterValue("pBranchName", branchName);
                deleteQtyReport.SetParameterValue("pReportName", reportName);
                this.deleteQtyCrystalReportViewer.ReportSource = deleteQtyReport;
                return true;

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        } 
    }
}
