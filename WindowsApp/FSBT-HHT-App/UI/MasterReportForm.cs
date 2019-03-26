using CrystalDecisions.CrystalReports.Engine;
using FSBT_HHT_BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSBT.HHT.App.UI
{
    public partial class MasterReportForm : Form
    {
        private LogErrorBll logBll = new LogErrorBll(); 

        public MasterReportForm()
        {
            InitializeComponent();
        }

        public bool CreateReport(DataTable masterData, string reportPath, string dataType, int modeFlg)
        {
            try
            {
                SystemSettingBll bllSetting = new FSBT_HHT_BLL.SystemSettingBll();
                var settingData = bllSetting.GetSettingData();

                //DataTable a = new DataTable();
                //a.Columns.Add(new DataColumn("Location"));
                //a.Columns.Add(new DataColumn("SectionCode"));
                //a.Columns.Add(new DataColumn("SectionName"));
                //a.Columns.Add(new DataColumn("Description"));

                //for (int i = 0; i < 100; i++)
                //{
                //    DataRow r = a.NewRow();
                //    r[0] = (i.ToString()).PadLeft(5,'0');
                //    r[1] = "01";
                //    r[2] = "Section";
                //    r[3] = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

                //    a.Rows.Add(r);
                //}

                //ReportDocument masterReport = new ReportDocument();
                //string startFilePath = Application.StartupPath;
                //string parentPath = new DirectoryInfo(startFilePath).Parent.Parent.FullName;
                //reportPath = "\\ReportTemplate\\R10_UncountedLocation.rpt";
                //string filePath = parentPath + reportPath;
                //masterReport.Load(filePath);
                //masterReport.SetDataSource(a);
                //masterReport.SetParameterValue("pCountDate", settingData.CountDate);
                //this.masterReportCrystalReportViewer.ReportSource = masterReport;

                //masterData.Columns.Add(new DataColumn("SectionCode"));
                //masterData.Columns.Add(new DataColumn("SectionName"));
                //foreach (DataRow dr in masterData.Rows)
                //{
                //    dr["Barcode"] = "1234567890123";
                //    dr["SectionCode"] = "0001";
                //    dr["SectionName"] = "Section";
                //}

                ReportDocument masterReport = new ReportDocument();
                string startFilePath = Application.StartupPath;
                //string parentPath = new DirectoryInfo(startFilePath).Parent.Parent.FullName;
                //reportPath = "\\ReportTemplate\\R14_ItemPhysicalCountBySection.rpt";
                string filePath = Path.GetFullPath("." + reportPath);
                //string exportPath = @"D:\Project FSBT-HHT\";
                //string fileName = dataType + modeFlg + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmm") + ".pdf";
                //string fullexportPath = exportPath + fileName;
                masterReport.Load(filePath);
                masterReport.SetDataSource(masterData);
                masterReport.SetParameterValue("pCountDate", settingData.CountDate);
                masterReport.SetParameterValue("pBranchName", settingData.Branch);
                //masterReport.SetParameterValue("selectedBrand", "AllBrand");
                //if (!Directory.Exists(exportPath))
                //{
                //    Directory.CreateDirectory(exportPath);
                //}
                //Cursor.Current = Cursors.WaitCursor;
                this.masterReportCrystalReportViewer.ReportSource = masterReport;
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
