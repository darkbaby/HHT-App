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
    public partial class DownloadMasterSummarySubDepartmentForm : Form
    {
        private LogErrorBll logBll = new LogErrorBll();
        public DownloadMasterSummarySubDepartmentForm()
        {
            InitializeComponent();
        }

        public void SummaryDownloadMaster(DataTable listSummaryData, int flg)
        {
            try
            {
                dataGridView1.DataSource = listSummaryData;

                this.dataGridView1.Columns["Department Code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                this.dataGridView1.Columns["Department Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                this.dataGridView1.Columns["Book Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                //this.dataGridView1.Columns["StockOnHand"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                this.dataGridView1.Columns["Department Code"].Width = 100;
                this.dataGridView1.Columns["Department Name"].Width = 350;
                this.dataGridView1.Columns["Book Quantity"].Width = 200;

                this.dataGridView1.Columns["Book Quantity"].DefaultCellStyle.Format = "N3";

                var numberOfColumn = listSummaryData.Columns.Count;
                string[] columnname = new string[numberOfColumn];

                int i = 0;

                foreach (DataColumn column in listSummaryData.Columns)
                {
                    columnname[i] = column.ColumnName;
                    i++;
                }

                for (int j = 3; j < (numberOfColumn); j++)
                {
                    this.dataGridView1.Columns[columnname[j]].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    this.dataGridView1.Columns[columnname[j]].DefaultCellStyle.Format = "N3";
                }

                //decimal totalQuantity = listSummaryData.AsEnumerable()
                //                            .Sum(x => x.Field<decimal>("Book Quantity"));
                ////int totalStock = listSummaryData.AsEnumerable()
                ////                            .Sum(x => x.Field<int>("StockOnHand"));
                //txtQuantity.Text = String.Format("{0:N3}", (decimal)totalQuantity / 2);
                //    //txtStock.Text = String.Format("{0}", (int)totalStock);


                var totalPCS = "";
                var totalKG = "";

                for (int k = dataGridView1.Rows.Count - 2; k <= dataGridView1.Rows.Count - 1; k++)
                {
                    var total = dataGridView1.Rows[k].Cells[2].Value == null ? "" : dataGridView1.Rows[k].Cells[2].Value.ToString().Trim();

                    if (total.Length >= 5)
                    {
                        total = total.Substring(0, 5);

                        if (total.ToUpper() == "TOTAL" && !string.IsNullOrEmpty(totalPCS))
                        {
                            totalKG = dataGridView1.Rows[k].Cells[3].Value == null ? "" : dataGridView1.Rows[k].Cells[3].Value.ToString().Trim();
                        }
                        else if (total.ToUpper() == "TOTAL")
                        {
                            totalPCS = dataGridView1.Rows[k].Cells[3].Value == null ? "" : dataGridView1.Rows[k].Cells[3].Value.ToString().Trim();
                        }
                    }

                }

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
