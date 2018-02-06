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
    public partial class DownloadMasterSummaryByBrandForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        public DownloadMasterSummaryByBrandForm()
        {
            InitializeComponent();
        }

        public void SummaryDownloadMaster(DataTable listSummaryData, int flg)
        {
            try
            {
                dataGridView1.DataSource = listSummaryData;

                this.dataGridView1.Columns["BrandCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                this.dataGridView1.Columns["BrandName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                this.dataGridView1.Columns["QuantityOnHand"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView1.Columns["StockOnHand"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                if (flg == 4)
                {
                    this.dataGridView1.Columns["QuantityOnHandWt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    this.dataGridView1.Columns["StockOnHandWt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    textQuantityW.Visible = true;
                    textStockW.Visible = true;
                    int totalQuantity = listSummaryData.AsEnumerable()
                                            .Sum(x => x.Field<int>("QuantityOnHand"));
                    int totalStock = listSummaryData.AsEnumerable()
                                            .Sum(x => x.Field<int>("StockOnHand"));
                    int totalQuantityW = listSummaryData.AsEnumerable()
                                            .Sum(x => x.Field<int>("QuantityOnHandWt"));
                    int totalStockW = listSummaryData.AsEnumerable()
                                            .Sum(x => x.Field<int>("StockOnHandWt"));
                    txtQuantity.Text = String.Format("{0}", (int)totalQuantity);
                    txtStock.Text = String.Format("{0}", (int)totalStock);
                    textQuantityW.Text = String.Format("{0}", (int)totalQuantityW);
                    textStockW.Text = String.Format("{0}", (int)totalStockW);
                }
                else
                {
                    textQuantityW.Visible = false;
                    textStockW.Visible = false;
                    int totalQuantity = listSummaryData.AsEnumerable()
                                            .Sum(x => x.Field<int>("QuantityOnHand"));
                    int totalStock = listSummaryData.AsEnumerable()
                                            .Sum(x => x.Field<int>("StockOnHand"));
                    txtQuantity.Text = String.Format("{0}", (int)totalQuantity);
                    txtStock.Text = String.Format("{0}", (int)totalStock);
                }
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                //double totalQuantity = dataGridView1.Rows.Cast<DataGridViewRow>()
                //.Sum(t => Convert.ToDouble(t.Cells[1].Value));
                //double totalNewQuantity = dataGridView1.Rows.Cast<DataGridViewRow>()
                //    .Sum(t => Convert.ToDouble(t.Cells[2].Value));
                //txtQuantity.Text = String.Format("{0:#,0.00}", (decimal)totalQuantity);
                //txtNewQuantity.Text = String.Format("{0:#,0.00}", (decimal)totalNewQuantity);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }
    }
}
