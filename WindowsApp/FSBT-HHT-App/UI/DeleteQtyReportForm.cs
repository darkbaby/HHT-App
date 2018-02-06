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

namespace FSBT.HHT.App.UI
{
    public partial class DeleteQtyReportForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        public DeleteQtyReportForm()
        {     
            InitializeComponent();       
        }
        SystemSettingBll bllSetting = new FSBT_HHT_BLL.SystemSettingBll();
        public bool CreateReport(List<EditQtyModel.ResponseDeleteReport> deleteData, DateTime countDate,string branchName)
        {
            try
            {
                //var settingData = bllSetting.GetSettingData();

                ReportDocument deleteQtyReport = new ReportDocument();
                string startFilePath = Application.StartupPath;
                //string parentPath = new DirectoryInfo(startFilePath).Parent.Parent.FullName;
                //string filePath = parentPath + "\\ReportTemplate\\R06_DeleteRecordReportByLocation.rpt";
                string filePath = Path.GetFullPath("./ReportTemplate/DeleteQtyReport.rpt");
                deleteQtyReport.Load(filePath);
                deleteQtyReport.SetDataSource(deleteData);
                deleteQtyReport.SetParameterValue("pCountDate", countDate);
                deleteQtyReport.SetParameterValue("pBranchName", branchName);
                this.deleteQtyCrystalReportViewer.ReportSource = deleteQtyReport;
                return true;

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        } 
    }
}
