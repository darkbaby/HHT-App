using FSBT_HHT_BLL;
using FSBT_HHT_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSBT.HHT.App.UI
{
    public partial class GenTextFileSummaryForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        public GenTextFileSummaryForm()
        {
            InitializeComponent();
        }
        private void GenTextFileSummaryForm_Load(object sender, EventArgs e)
        {

        }

        public void SummaryFirstRecordMeargeFile(List<MeargeFileFirstRecord> listSummaryData)
        {
            try
            {
                foreach (MeargeFileFirstRecord result in listSummaryData)
                {
                    
                    dataGridView1.Rows.Add(result.Computer, result.Record, result.QtyFront, result.QtyBack, result.QtyStockPcs, result.QtyStockPck, result.QtyWTPcs, result.QtyWTG);
                    
                }
                int totalRecord = listSummaryData.Sum(item => Convert.ToInt32(item.Record));
                int totalQtyFront = listSummaryData.Sum(item => item.QtyFront == string.Empty ? 0 : Convert.ToInt32(item.QtyFront));
                int totalQtyBack = listSummaryData.Sum(item => item.QtyBack == string.Empty ? 0 : Convert.ToInt32(item.QtyBack));
                int totalQtyStockPcs = listSummaryData.Sum(item => item.QtyStockPcs == string.Empty ? 0 : Convert.ToInt32(item.QtyStockPcs));
                int totalQtyStockPck = listSummaryData.Sum(item => item.QtyStockPck == string.Empty ? 0 : Convert.ToInt32(item.QtyStockPck));
                int totalQtyWTPcs = listSummaryData.Sum(item => item.QtyWTPcs == string.Empty ? 0 : Convert.ToInt32(item.QtyWTPcs));
                decimal totalQtyWTG = listSummaryData.Sum(item => item.QtyWTG == string.Empty ? 0 : Convert.ToDecimal(item.QtyWTG));

                dataGridView1.Rows.Add("Total", totalRecord, totalQtyFront, totalQtyBack, totalQtyStockPcs, totalQtyStockPck, totalQtyWTPcs, totalQtyWTG);
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }
    }
}
