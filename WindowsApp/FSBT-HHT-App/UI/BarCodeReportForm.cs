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

namespace FSBT.HHT.App.UI
{
    public partial class BarCodeReportForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);

        public BarCodeReportForm()
        {     
            InitializeComponent();       
        }

        public bool CreateReport(List<LocationBarcode> barcodeData)
        {
            try
            {
                ReportDocument barcodeReport = new ReportDocument();
                string startFilePath = Application.StartupPath;
                string parentPath = new DirectoryInfo(startFilePath).Parent.Parent.FullName;
                //string filePath = parentPath + "\\ReportTemplate\\BarcodeReport.rpt";
                //string filePath = Application.StartupPath + "\\ReportTemplate\\BarcodeReport.rpt";
                string filePath = Path.GetFullPath("./ReportTemplate/BarcodeReport.rpt");
                barcodeReport.Load(filePath);
                barcodeReport.SetDataSource(barcodeData);
                this.barcodeCrystalReportViewer.ReportSource = barcodeReport;
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
