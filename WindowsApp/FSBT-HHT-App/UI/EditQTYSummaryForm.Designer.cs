﻿namespace FSBT.HHT.App.UI
{
    partial class EditQTYSummaryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupSummary = new System.Windows.Forms.GroupBox();
            this.txtNewQuantity = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtNewQuantityKG = new System.Windows.Forms.TextBox();
            this.txtQuantityKG = new System.Windows.Forms.TextBox();
            this.label_TotalKG = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocationCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupSummary
            // 
            this.groupSummary.Controls.Add(this.txtNewQuantityKG);
            this.groupSummary.Controls.Add(this.txtQuantityKG);
            this.groupSummary.Controls.Add(this.label_TotalKG);
            this.groupSummary.Controls.Add(this.txtNewQuantity);
            this.groupSummary.Controls.Add(this.txtQuantity);
            this.groupSummary.Controls.Add(this.dataGridView1);
            this.groupSummary.Controls.Add(this.lblTotal);
            this.groupSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupSummary.Location = new System.Drawing.Point(12, 11);
            this.groupSummary.Name = "groupSummary";
            this.groupSummary.Size = new System.Drawing.Size(562, 411);
            this.groupSummary.TabIndex = 1;
            this.groupSummary.TabStop = false;
            this.groupSummary.Text = "Summary";
            // 
            // txtNewQuantity
            // 
            this.txtNewQuantity.Location = new System.Drawing.Point(303, 336);
            this.txtNewQuantity.Name = "txtNewQuantity";
            this.txtNewQuantity.ReadOnly = true;
            this.txtNewQuantity.Size = new System.Drawing.Size(120, 24);
            this.txtNewQuantity.TabIndex = 6;
            this.txtNewQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Location = new System.Drawing.Point(184, 336);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.ReadOnly = true;
            this.txtQuantity.Size = new System.Drawing.Size(120, 24);
            this.txtQuantity.TabIndex = 5;
            this.txtQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LocationCode,
            this.Quantity,
            this.NewQuantity,
            this.UnitName});
            this.dataGridView1.Location = new System.Drawing.Point(22, 35);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(523, 302);
            this.dataGridView1.TabIndex = 2;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(47, 340);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(115, 18);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "Total PCS(G,F) ";
            // 
            // txtNewQuantityKG
            // 
            this.txtNewQuantityKG.Location = new System.Drawing.Point(303, 359);
            this.txtNewQuantityKG.Name = "txtNewQuantityKG";
            this.txtNewQuantityKG.ReadOnly = true;
            this.txtNewQuantityKG.Size = new System.Drawing.Size(120, 24);
            this.txtNewQuantityKG.TabIndex = 9;
            this.txtNewQuantityKG.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtQuantityKG
            // 
            this.txtQuantityKG.Location = new System.Drawing.Point(184, 359);
            this.txtQuantityKG.Name = "txtQuantityKG";
            this.txtQuantityKG.ReadOnly = true;
            this.txtQuantityKG.Size = new System.Drawing.Size(120, 24);
            this.txtQuantityKG.TabIndex = 8;
            this.txtQuantityKG.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_TotalKG
            // 
            this.label_TotalKG.AutoSize = true;
            this.label_TotalKG.Location = new System.Drawing.Point(47, 363);
            this.label_TotalKG.Name = "label_TotalKG";
            this.label_TotalKG.Size = new System.Drawing.Size(96, 18);
            this.label_TotalKG.TabIndex = 7;
            this.label_TotalKG.Text = "Total KG(W) ";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "LocationCode";
            this.dataGridViewTextBoxColumn1.HeaderText = "Location";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 120;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Quantity";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewTextBoxColumn2.HeaderText = "Quantity";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 120;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "NewQuantity";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewTextBoxColumn3.HeaderText = "New Quantity";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 120;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "UnitName";
            this.dataGridViewTextBoxColumn4.HeaderText = "UnitCode";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn4.Width = 200;
            // 
            // LocationCode
            // 
            this.LocationCode.DataPropertyName = "LocationCode";
            this.LocationCode.HeaderText = "Sub Location Code";
            this.LocationCode.Name = "LocationCode";
            this.LocationCode.ReadOnly = true;
            this.LocationCode.Width = 120;
            // 
            // Quantity
            // 
            this.Quantity.DataPropertyName = "Quantity";
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            this.Quantity.Width = 120;
            // 
            // NewQuantity
            // 
            this.NewQuantity.DataPropertyName = "NewQuantity";
            this.NewQuantity.HeaderText = "New Quantity";
            this.NewQuantity.Name = "NewQuantity";
            this.NewQuantity.ReadOnly = true;
            this.NewQuantity.Width = 120;
            // 
            // UnitName
            // 
            this.UnitName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.UnitName.DataPropertyName = "UnitName";
            this.UnitName.HeaderText = "Unit Name";
            this.UnitName.Name = "UnitName";
            this.UnitName.ReadOnly = true;
            // 
            // EditQTYSummaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 452);
            this.Controls.Add(this.groupSummary);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "EditQTYSummaryForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EditQTYSummaryForm";
            this.Load += new System.EventHandler(this.EditQTYSummaryForm_Load);
            this.groupSummary.ResumeLayout(false);
            this.groupSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupSummary;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.TextBox txtNewQuantity;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.TextBox txtNewQuantityKG;
        private System.Windows.Forms.TextBox txtQuantityKG;
        private System.Windows.Forms.Label label_TotalKG;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitName;
    }
}