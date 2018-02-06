namespace FSBT.HHT.App.UI
{
    partial class DownloadMasterSummaryForm
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
            this.groupSummary = new System.Windows.Forms.GroupBox();
            this.textStockW = new System.Windows.Forms.TextBox();
            this.textQuantityW = new System.Windows.Forms.TextBox();
            this.txtStock = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.groupSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupSummary
            // 
            this.groupSummary.Controls.Add(this.textStockW);
            this.groupSummary.Controls.Add(this.textQuantityW);
            this.groupSummary.Controls.Add(this.txtStock);
            this.groupSummary.Controls.Add(this.txtQuantity);
            this.groupSummary.Controls.Add(this.dataGridView1);
            this.groupSummary.Controls.Add(this.lblTotal);
            this.groupSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupSummary.Location = new System.Drawing.Point(18, 18);
            this.groupSummary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupSummary.Name = "groupSummary";
            this.groupSummary.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupSummary.Size = new System.Drawing.Size(994, 571);
            this.groupSummary.TabIndex = 2;
            this.groupSummary.TabStop = false;
            this.groupSummary.Text = "Summary";
            // 
            // textStockW
            // 
            this.textStockW.Location = new System.Drawing.Point(696, 518);
            this.textStockW.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textStockW.Name = "textStockW";
            this.textStockW.ReadOnly = true;
            this.textStockW.Size = new System.Drawing.Size(148, 32);
            this.textStockW.TabIndex = 8;
            this.textStockW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textStockW.Visible = false;
            // 
            // textQuantityW
            // 
            this.textQuantityW.Location = new System.Drawing.Point(548, 518);
            this.textQuantityW.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textQuantityW.Name = "textQuantityW";
            this.textQuantityW.ReadOnly = true;
            this.textQuantityW.Size = new System.Drawing.Size(148, 32);
            this.textQuantityW.TabIndex = 7;
            this.textQuantityW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textQuantityW.Visible = false;
            // 
            // txtStock
            // 
            this.txtStock.Location = new System.Drawing.Point(399, 518);
            this.txtStock.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtStock.Name = "txtStock";
            this.txtStock.ReadOnly = true;
            this.txtStock.Size = new System.Drawing.Size(148, 32);
            this.txtStock.TabIndex = 6;
            this.txtStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Location = new System.Drawing.Point(254, 518);
            this.txtQuantity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.ReadOnly = true;
            this.txtQuantity.Size = new System.Drawing.Size(148, 32);
            this.txtQuantity.TabIndex = 5;
            this.txtQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(42, 54);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(819, 465);
            this.dataGridView1.TabIndex = 2;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(186, 523);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(71, 26);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "Total :";
            // 
            // DownloadMasterSummaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 605);
            this.Controls.Add(this.groupSummary);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DownloadMasterSummaryForm";
            this.ShowIcon = false;
            this.Text = "DownloadMasterSummaryForm";
            this.groupSummary.ResumeLayout(false);
            this.groupSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.GroupBox groupSummary;
        private System.Windows.Forms.TextBox textStockW;
        private System.Windows.Forms.TextBox textQuantityW;
    }
}