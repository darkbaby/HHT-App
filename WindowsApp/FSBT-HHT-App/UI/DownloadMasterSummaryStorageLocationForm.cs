using FSBT_HHT_BLL;
using FSBT_HHT_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSBT.HHT.App.UI
{
    public partial class DownloadMasterSummaryStorageLocationForm : Form
    {
        private LogErrorBll logBll = new LogErrorBll();
        public DownloadMasterSummaryStorageLocationForm()
        {
            InitializeComponent();
        }

        public void SummaryDownloadMaster(DataTable listSummaryData, int flg)
        {
            try
            {
                dataGridView1.DataSource = listSummaryData;

                //this.dataGridView1.Columns["Storage Location Code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                //this.dataGridView1.Columns["Book Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                var colCount = this.dataGridView1.ColumnCount;
                var totalPCS = "";
                var totalKG = "";
                var lastRow = dataGridView1.Rows.Count - 1;

                if (colCount == 4)
                {
                    this.dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    totalKG = dataGridView1.Rows[lastRow].Cells[3].Value == null ? "" : dataGridView1.Rows[lastRow].Cells[3].Value.ToString().Trim();
                }

                this.dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                this.dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                totalPCS = dataGridView1.Rows[lastRow].Cells[2].Value == null ? "" : dataGridView1.Rows[lastRow].Cells[2].Value.ToString().Trim();                

                txtQuantity.Text = totalPCS;//String.Format("N", totalPCS); //

                if (!string.IsNullOrEmpty(totalKG))
                {
                    txtQuantityKG.Visible = true;
                    txtQuantityKG.Text = totalKG;//String.Format("{0:N3}", totalKG); //
                    label_TotalKG.Visible = true;
                }
                else
                {
                    txtQuantityKG.Visible = false;
                    label_TotalKG.Visible = false;
                }
            
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }
    }
}
