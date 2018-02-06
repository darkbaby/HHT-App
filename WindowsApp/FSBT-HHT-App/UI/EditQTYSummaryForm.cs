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
    public partial class EditQTYSummaryForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        public EditQTYSummaryForm()
        {
            InitializeComponent();
        }
        private void EditQTYSummaryForm_Load(object sender, EventArgs e)
        {

        }

        public void SummaryEditQty(List<EditQtyModel.ResponseSummary> listSummaryData, List<EditQtyModel.MasterUnit> listUnit)
        {
            try
            {
                foreach (EditQtyModel.ResponseSummary result in listSummaryData)
                {
                    string locationCode = result.LocationCode;
                    string unitName = result.UnitName;
                    string Qty = string.Empty;
                    string NewQty = string.Empty;

                    var quantity = result.Quantity;
                    var newQuantity = result.NewQuantity;
                    bool isIntQty = quantity == (int)result.Quantity;
                    bool isIntNewQty = newQuantity == (int)result.NewQuantity;
                    if(unitName == "KG")
                    {
                        Qty = String.Format("{0:#,0.000}", (decimal)quantity);
                       
                        if (newQuantity.ToString() == "0.000")
                        {
                            NewQty = null;
                        }
                        else
                        {
                            NewQty = String.Format("{0:#,0.000}", (decimal)newQuantity);
                        }
                    }
                    else
                    {
                        Qty = String.Format("{0:#,0}", (decimal)quantity);
                        if (newQuantity.ToString() == "0.000")
                        {
                            NewQty = null;
                        }
                        else
                        {
                            NewQty = String.Format("{0:#,0}", (decimal)newQuantity);
                        }
                    }


                    //if (quantity.ToString() == "0.000")
                    //{
                    //    Qty = null;
                    //}
                    //else
                    //{
                    //    if (isIntQty)
                    //    {
                    //        Qty = String.Format("{0:#,0}", (decimal)quantity);
                    //    }
                    //    else
                    //    {
                    //        Qty = String.Format("{0:#,0.000}", (decimal)quantity);
                    //    }
                    //}

                    //if (newQuantity.ToString() == "0.000")
                    //{
                    //    NewQty = null;
                    //}
                    //else
                    //{
                    //    if (isIntNewQty)
                    //    {
                    //        NewQty = String.Format("{0:#,0}", (decimal)newQuantity);
                    //    }
                    //    else
                    //    {
                    //        NewQty = String.Format("{0:#,0.000}", (decimal)newQuantity);
                    //    }
                    //}
                    dataGridView1.Rows.Add(locationCode,Qty,NewQty,unitName);
                }

                //double totalQuantity = dataGridView1.Rows.Cast<DataGridViewRow>()
                //.Sum(t => Convert.ToDouble(t.Cells[1].Value));
                //double totalNewQuantity = dataGridView1.Rows.Cast<DataGridViewRow>()
                //    .Sum(t => Convert.ToDouble(t.Cells[2].Value));

                decimal totalQuantity = listSummaryData.Sum(item => item.Quantity);
                decimal totalNewQuantity = listSummaryData.Sum(item => item.NewQuantity);

                //txtQuantity.Text = String.Format("{0:#,0.00}", (decimal)totalQuantity);
                //txtNewQuantity.Text = String.Format("{0:#,0.00}", (decimal)totalNewQuantity);

                bool isIntQtyTotal = totalQuantity == (int)totalQuantity;
                bool isIntNewQtyTotal = totalNewQuantity == (int)totalNewQuantity;

                if (isIntQtyTotal)
                {
                    txtQuantity.Text = String.Format("{0:#,0}", (decimal)totalQuantity);
                }
                else
                {
                    txtQuantity.Text = String.Format("{0:#,0.000}", (decimal)totalQuantity);
                }
                if (isIntNewQtyTotal)
                {

                    txtNewQuantity.Text = String.Format("{0:#,0}", (decimal)totalNewQuantity);
                }
                else
                {
                    txtNewQuantity.Text = String.Format("{0:#,0.000}", (decimal)totalNewQuantity);
                }


            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }
    }
}
