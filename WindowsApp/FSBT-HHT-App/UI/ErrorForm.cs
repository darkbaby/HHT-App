using FSBT_HHT_BLL;
using FSBT_HHT_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSBT.HHT.App.UI
{
    public partial class ErrorForm : Form
    {
        private string m_username;
        private LogErrorBll logBll = new LogErrorBll();

        public ErrorForm(string username)
        {
            InitializeComponent();
            m_username = username;
        }
        public void SummaryDownloadMaster(DataTable listSummaryData, int flg, bool autosize)
        {
            try
            {          
                //Stopwatch timer = Stopwatch.StartNew();
                //dataGridView1.Columns.Clear();
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; //or even better .DisableResizing. Most time consumption enum is DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
                // set it to false if not needed
                dataGridView1.RowHeadersVisible = false;

                dataGridView1.DataSource = listSummaryData;

                if (autosize)
                {
                    dataGridView1.AutoResizeColumns();
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
                else
                {
                    dataGridView1.AutoResizeColumns();
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                }


                //timer.Stop();
                //TimeSpan timespan = timer.Elapsed;
                
                //string time = String.Format("{0:00}:{1:00}:{2:00}", timespan.Minutes, timespan.Seconds, timespan.Milliseconds / 10);
                //MessageBox.Show(time);

            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                logBll.LogError(m_username, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void linkLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
    }
}
