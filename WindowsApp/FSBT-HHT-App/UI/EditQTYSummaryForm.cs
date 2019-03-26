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
    public partial class EditQTYSummaryForm : Form
    {
        private LogErrorBll logBll = new LogErrorBll(); 
        public EditQTYSummaryForm()
        {
            InitializeComponent();
        }
        private void EditQTYSummaryForm_Load(object sender, EventArgs e)
        {

        }

        public void SummaryEditQty(DataTable listSummaryData)
        {
            try
            {

                dataGridView1.DataSource = listSummaryData;

                //var numberOfColumn = listSummaryData.Columns.Count;
                //string[] columnname = new string[numberOfColumn];

                //int i = 0;

                //foreach (DataColumn column in listSummaryData.Columns)
                //{
                //    columnname[i] = column.ColumnName;
                //    i++;
                //}

                this.dataGridView1.Columns["LocationCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                this.dataGridView1.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView1.Columns["NewQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView1.Columns["UnitName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                //this.dataGridView1.Columns["LocationCode"].Width = 100;
                //this.dataGridView1.Columns["Quantity"].Width = 100;           
                //this.dataGridView1.Columns["NewQuantity"].Width = 100;
                //this.dataGridView1.Columns["UnitName"].Width = 100;

                //for (int j = 3; j < (numberOfColumn); j++)
                //{
                //    this.dataGridView1.Columns[columnname[j]].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                //}

                var totalPCS = "";
                var totalKG = "";
                var totalNewPCS = "";
                var totalNewKG = "";


                for (int k = dataGridView1.Rows.Count - 2; k <= dataGridView1.Rows.Count - 1; k++)
                {
                    var total = dataGridView1.Rows[k].Cells[0].Value == null ? "" : dataGridView1.Rows[k].Cells[0].Value.ToString().Trim();

                    if (total.Length >= 5)
                    {
                        total = total.Substring(0, 5);

                        if (total.ToUpper() == "TOTAL" && !string.IsNullOrEmpty(totalPCS))
                        {
                            totalKG = dataGridView1.Rows[k].Cells[1].Value == null ? "" : dataGridView1.Rows[k].Cells[1].Value.ToString().Trim();
                            totalNewKG = dataGridView1.Rows[k].Cells[2].Value == null ? "" : dataGridView1.Rows[k].Cells[2].Value.ToString().Trim();
                        }
                        else if (total.ToUpper() == "TOTAL")
                        {
                            totalPCS = dataGridView1.Rows[k].Cells[1].Value == null ? "" : dataGridView1.Rows[k].Cells[1].Value.ToString().Trim();
                            totalNewPCS = dataGridView1.Rows[k].Cells[2].Value == null ? "" : dataGridView1.Rows[k].Cells[2].Value.ToString().Trim();
                        }
                    }
                }

                txtQuantity.Text = totalPCS;
                txtNewQuantity.Text = totalNewPCS;

                if (!string.IsNullOrEmpty(totalKG))
                {
                    txtQuantityKG.Visible = true;
                    txtNewQuantityKG.Visible = true;
                    txtQuantityKG.Text = totalKG;
                    txtNewQuantityKG.Text = totalNewKG;
                    label_TotalKG.Visible = true;
                }
                else
                {
                    txtQuantityKG.Visible = false;
                    txtNewQuantityKG.Visible = false;
                    label_TotalKG.Visible = false;
                }

                //dataGridView1.AutoResizeColumns();
                //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;               
                            
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }
    }
}
