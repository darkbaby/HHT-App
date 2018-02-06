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
    public partial class LocationReportForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
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
                string reportFile = bllReportManagement.GetReportURLByReportCode("R05");
                string reportName = bllReportManagement.GetReportNameByReportCode("R05");
                string filePath = Path.GetFullPath("./ReportTemplate/" + reportFile);                

                ParameterFields paramFields = new ParameterFields();
                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();
                ParameterField paramField1 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue1 = new ParameterDiscreteValue();
                ParameterField paramField2 = new ParameterField();
                ParameterDiscreteValue paramDiscreteValue2 = new ParameterDiscreteValue();
                paramField.Name = "@Subject";
                paramDiscreteValue.Value = localType;
                paramField.CurrentValues.Add(paramDiscreteValue);
                paramField1.Name = "pBranchName";
                paramDiscreteValue1.Value = brachName;
                paramField1.CurrentValues.Add(paramDiscreteValue1);
                paramField2.Name = "pReportName";
                paramDiscreteValue2.Value = reportName;
                paramField2.CurrentValues.Add(paramDiscreteValue2);
                paramFields.Add(paramField);
                paramFields.Add(paramField1);
                paramFields.Add(paramField2);

                locationReport.Load(filePath);
                locationReport.SetDataSource(listSectionLocation);

                this.LocationCrystalReportViewer.ParameterFieldInfo = paramFields;
                this.LocationCrystalReportViewer.ReportSource = locationReport;

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
