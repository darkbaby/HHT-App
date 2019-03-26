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
    public partial class LocationReportForm : Form
    {
        private LogErrorBll logBll = new LogErrorBll(); 
        public LocationReportForm()
        {
            InitializeComponent();
        }

        public bool CreateReport(List<LocationManagementModel> listSectionLocation, string localType, string brachName)
        {
            try
            {
                ReportDocument locationReport = new ReportDocument();
                ReportManagementBll bllReportManagement = new ReportManagementBll();
                string startFilePath = Application.StartupPath;
                //string parentPath = new DirectoryInfo(startFilePath).Parent.Parent.FullName;
                //string filePath = parentPath + "\\ReportTemplate\\RPT_SectionLocationByBrandGroup.rpt";
                string reportFile = bllReportManagement.GetReportURLByReportCode("R07");
                string reportName = bllReportManagement.GetReportNameByReportCode("R07");
                string filePath = Path.GetFullPath("./ReportTemplate/" + reportFile);                
 
                SystemSettingBll bllSystemSetting = new SystemSettingBll();
                DateTime countDate = DateTime.Now;
                countDate = bllSystemSetting.GetSettingData().CountDate;
                ParameterFields paramFields = new ParameterFields();
                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                ParameterField paramField3 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue3 = new ParameterDiscreteValue();
                paramField1.Name = "pCountDate";
                paramDiscreteValue1.Value = countDate;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pBranchName";
                paramDiscreteValue2.Value = brachName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramField3.Name = "pReportName";
                paramDiscreteValue3.Value = reportName;
                paramField3.CurrentValues.Add(paramDiscreteValue3);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);
                paramFields.Add(paramField3);

                locationReport.Load(filePath);
                locationReport.SetDataSource(listSectionLocation);

                this.LocationCrystalReportViewer.ParameterFieldInfo = paramFields;
                this.LocationCrystalReportViewer.ReportSource = locationReport;

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
