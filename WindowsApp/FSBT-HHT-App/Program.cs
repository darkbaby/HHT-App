using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSBT.HHT.App.UI;
using System.Diagnostics;

namespace FSBT.HHT.App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Layout());            
            //Application.Run(new LayoutWmenu());
            //Application.Run(new DownloadMaster());
            //Application.Run(new Location());
            //Application.Run(new Barcode());
            //Application.Run(new QuantityByLocation());
            //Application.Run(new QuantityBySection1());
            //Application.Run(new TextFile());
            if (CheckRunProgramResult())
            {
                MessageBox.Show(Resources.MessageConstants.CannotOpenProgram, Resources.MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                CloseTaskBatch();
                Application.Run(new Login());

            }
            //Application.Run(new ReportPrintForm());
            //Application.Run(new UserManagementForm());
            //Application.Run(new PrintBarCodeForm());
            //Application.Run(new SystemSettingsForm());
            //Application.Run(new DownloadMasterForm());
        }
        public static Boolean CheckRunProgramResult()
        {
            Boolean chkRunProgramResult = false;

            //Process[] programName = Process.GetProcessesByName("FSBT-HHT-Batch");
            Process[] programName = Process.GetProcessesByName("FSBT-HHT-App");
            var statusbatchRunning = new List<string>();
            if (programName.Length > 1)
            {
                chkRunProgramResult = true;
            }
            else
            {
                chkRunProgramResult = false;
            }
            return chkRunProgramResult;
        }
        private static void CloseTaskBatch()
        {
            Process[] pname = Process.GetProcessesByName("FSBT-HHT-Batch");
            if (pname.Length != 0)
            {
                foreach (var process in Process.GetProcessesByName("FSBT-HHT-Batch"))
                    process.Kill();
            }
            //Cho ADD//
            Process[] pname2 = Process.GetProcessesByName("FSBT-HHT-Services");
            if (pname2.Length != 0)
            {
                foreach (var process in Process.GetProcessesByName("FSBT-HHT-Services"))
                    process.Kill();
            }
        }
    }
}
