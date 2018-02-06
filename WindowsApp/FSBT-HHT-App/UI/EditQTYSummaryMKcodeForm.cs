﻿using FSBT_HHT_BLL;
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
    public partial class EditQTYSummaryMKCodeForm : Form
    {
        DataTable dt = new DataTable();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        public EditQTYSummaryMKCodeForm()
        {
            InitializeComponent();
        }
        private void EditQTYSummaryMKCodeForm_Load(object sender, EventArgs e)
        {
            
        }

        public void SummaryMKCodeEditQty(List<EditQtyModel.ResponseSummaryMKCode> listSummaryData, List<EditQtyModel.MasterUnit> listUnit)
        {
            try
            {
                DataColumn dc = new DataColumn("MKCode", typeof(String));
                dt.Columns.Add(dc);

                dc = new DataColumn("Quantity", typeof(String));
                dt.Columns.Add(dc);

                dc = new DataColumn("NewQuantity", typeof(String));
                dt.Columns.Add(dc);

                dc = new DataColumn("UnitName", typeof(String));
                dt.Columns.Add(dc);
                foreach (EditQtyModel.ResponseSummaryMKCode result in listSummaryData)
                {
                    string mkCode = result.MKCode;
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


                    //dataGridView1.Rows.Add(mkCode, Qty,NewQty,unitName);
                    

                    DataRow dataRow = dt.NewRow();
                    dataRow[0] = mkCode;
                    dataRow[1] = Qty;
                    dataRow[2] = NewQty;
                    dataRow[3] = unitName;
                    dt.Rows.Add(dataRow);
                }

                dataGridView1.DataSource = dt;

                decimal totalQuantity = listSummaryData.Sum(item => item.Quantity);
                decimal totalNewQuantity = listSummaryData.Sum(item => item.NewQuantity);

                bindSumGranTotal(totalQuantity, totalNewQuantity);
                //bool isIntQtyTotal = totalQuantity == (int)totalQuantity;
                //bool isIntNewQtyTotal = totalNewQuantity == (int)totalNewQuantity;

                //if (isIntQtyTotal)
                //{
                //    txtQuantity.Text = String.Format("{0:#,0}", (decimal)totalQuantity);
                //}
                //else
                //{
                //    txtQuantity.Text = String.Format("{0:#,0.000}", (decimal)totalQuantity);
                //}
                //if (isIntNewQtyTotal)
                //{

                //    txtNewQuantity.Text = String.Format("{0:#,0}", (decimal)totalNewQuantity);
                //}
                //else
                //{
                //    txtNewQuantity.Text = String.Format("{0:#,0.000}", (decimal)totalNewQuantity);
                //}
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void textBoxFilter_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataTable searchData = SearchData();
                    dataGridView1.DataSource = searchData;
                }
                else
                {
                    //do nothing
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }
        public DataTable SearchData()
        {
            DataTable searchTable = new DataTable();
            try
            {
                //DataTable dt = (DataTable)dataGridView1.DataSource;


               
                string searchWord = textBoxFilter.Text;
                if (searchWord == "")
                {
                    return dt;
                }
                else
                {
                    var searchQuery = dt
                                        .Rows
                                        .Cast<DataRow>()
                                        .Where(r => r.ItemArray.Any(
                                            c => c.ToString().ToLower().Contains(searchWord.ToLower())
                                        ));
                    if (searchQuery.Any())
                    {
                        searchTable = searchQuery.CopyToDataTable();
                    }

                    decimal totalQuantity = searchTable.AsEnumerable().Sum(dr => Convert.ToDecimal(dr.Field<string>("Quantity")));

                    decimal totalNewQuantity = searchTable.AsEnumerable().Sum(dr => Convert.ToDecimal(dr.Field<string>("NewQuantity")));
                    bindSumGranTotal(totalQuantity, totalNewQuantity);

                    return searchTable;
                }
            }
            catch (Exception ex)
            {
                return searchTable = new DataTable();
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }
        private void bindSumGranTotal(decimal totalQuantity,decimal totalNewQuantity)
        {

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
    }
}
