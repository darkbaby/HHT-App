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
    public partial class SerialNumberReportForm : Form
    {
private LogErrorBll logBll = new LogErrorBll(); 
        public SerialNumberReportForm()
        {     
            InitializeComponent();       
        }
        SystemSettingBll bllSetting = new FSBT_HHT_BLL.SystemSettingBll();
        public bool CreateReport(List<EditQtyModel.ResponseSerialNumberReport> serialNumberData, DateTime countDate,string branchName)
        {
            try
            {
                ReportDocument SerialNumberReport = new ReportDocument();
                string startFilePath = Application.StartupPath;
                string filePath = Path.GetFullPath("./ReportTemplate/RPT_SerialNumberReport.rpt");
                SerialNumberReport.Load(filePath);
                SerialNumberReport.SetDataSource(serialNumberData);
                SerialNumberReport.SetParameterValue("pCountDate", countDate);
                SerialNumberReport.SetParameterValue("pReportName", "Serial Number Control Report");
                this.SerialNumberCrystalReportViewer.ReportSource = SerialNumberReport;
             
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
