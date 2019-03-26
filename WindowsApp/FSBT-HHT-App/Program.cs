using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSBT.HHT.App.UI;
using System.Diagnostics;
using FSBT_HHT_DAL;
using System.Data.SqlClient;
using System.IO;
using FSBT_HHT_BLL;
using System.Globalization;
using System.Threading;

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
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            newCulture.DateTimeFormat.LongDatePattern = "yyyy/MM/dd HH:mm:ss";
            newCulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = newCulture;
            
            if (CheckRunProgramResult())
            {
                MessageBox.Show(Resources.MessageConstants.CannotOpenProgram, Resources.MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (CheckConnection())
                {
                    CloseTaskBatch();
                    if (!Directory.Exists(LocalParameter.programPath))
                    {
                        Directory.CreateDirectory(LocalParameter.programPath);
                    }

                    Application.Run(new Login());
                }
                else
                {
                    MessageBox.Show(Resources.MessageConstants.Cannotconnecttodatabase, Resources.MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public static Boolean CheckRunProgramResult()
        {
            Boolean chkRunProgramResult = false;

            Process[] programName = Process.GetProcessesByName("The Mall Stocktaking");

            String thisprocessname = Process.GetCurrentProcess().ProcessName;
            var statusbatchRunning = new List<string>();
            if (Process.GetProcesses().Count(p => p.ProcessName == thisprocessname) > 1)
            {
                chkRunProgramResult = true;
            }
            else
            {
                chkRunProgramResult = false;
            }
            return chkRunProgramResult;

            //return false;
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

        public static bool CheckConnection()
        {
            Entities dbContext = new Entities();
            try
            {
                dbContext.Database.Connection.Open();
                dbContext.Database.Connection.Close();
            }
            catch (SqlException)
            {
                return false;
            }
            return true;
        }
    }
}
