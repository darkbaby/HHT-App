namespace FSBT.HHT.App.UI
{
    partial class GenTextFileSummaryForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupSummary = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Computer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Record = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QtyFront = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QtyBack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QtyStockPcs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QtyStockPck = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QtyWTPcs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QtyWTG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupSummary
            // 
            this.groupSummary.Controls.Add(this.dataGridView1);
            this.groupSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupSummary.Location = new System.Drawing.Point(12, 12);
            this.groupSummary.Name = "groupSummary";
            this.groupSummary.Size = new System.Drawing.Size(1066, 371);
            this.groupSummary.TabIndex = 1;
            this.groupSummary.TabStop = false;
            this.groupSummary.Text = "Summary Merge File";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Computer,
            this.Record,
            this.QtyFront,
            this.QtyBack,
            this.QtyStockPcs,
            this.QtyStockPck,
            this.QtyWTPcs,
            this.QtyWTG});
            this.dataGridView1.Location = new System.Drawing.Point(28, 35);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1005, 302);
            this.dataGridView1.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "LocationCode";
            this.dataGridViewTextBoxColumn1.HeaderText = "Location";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Quantity";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTextBoxColumn2.HeaderText = "Quantity";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "NewQuantity";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn3.HeaderText = "New Quantity";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "UnitCode";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "QtyStockPck";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "QtyStockPcs";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "QtyWTPcs";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "QtyWTG";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // Computer
            // 
            this.Computer.HeaderText = "Computer";
            this.Computer.Name = "Computer";
            this.Computer.ReadOnly = true;
            // 
            // Record
            // 
            this.Record.HeaderText = "Record";
            this.Record.Name = "Record";
            this.Record.ReadOnly = true;
            // 
            // QtyFront
            // 
            this.QtyFront.HeaderText = "QtyFront";
            this.QtyFront.Name = "QtyFront";
            this.QtyFront.ReadOnly = true;
            // 
            // QtyBack
            // 
            this.QtyBack.HeaderText = "QtyBack";
            this.QtyBack.Name = "QtyBack";
            this.QtyBack.ReadOnly = true;
            // 
            // QtyStockPcs
            // 
            this.QtyStockPcs.HeaderText = "QtyWarehousePck";
            this.QtyStockPcs.Name = "QtyStockPcs";
            this.QtyStockPcs.ReadOnly = true;
            this.QtyStockPcs.Width = 140;
            // 
            // QtyStockPck
            // 
            this.QtyStockPck.HeaderText = "QtyWarehousePcs";
            this.QtyStockPck.Name = "QtyStockPck";
            this.QtyStockPck.ReadOnly = true;
            this.QtyStockPck.Width = 140;
            // 
            // QtyWTPcs
            // 
            this.QtyWTPcs.HeaderText = "QtyFreshFoodPcs";
            this.QtyWTPcs.Name = "QtyWTPcs";
            this.QtyWTPcs.ReadOnly = true;
            this.QtyWTPcs.Width = 140;
            // 
            // QtyWTG
            // 
            this.QtyWTG.HeaderText = "QtyFreshFoodG";
            this.QtyWTG.Name = "QtyWTG";
            this.QtyWTG.ReadOnly = true;
            this.QtyWTG.Width = 140;
            // 
            // GenTextFileSummaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 407);
            this.Controls.Add(this.groupSummary);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "GenTextFileSummaryForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GenTextFileSummaryForm";
            this.Load += new System.EventHandler(this.GenTextFileSummaryForm_Load);
            this.groupSummary.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupSummary;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn QtyWTG;
        private System.Windows.Forms.DataGridViewTextBoxColumn QtyWTPcs;
        private System.Windows.Forms.DataGridViewTextBoxColumn QtyStockPck;
        private System.Windows.Forms.DataGridViewTextBoxColumn QtyStockPcs;
        private System.Windows.Forms.DataGridViewTextBoxColumn QtyBack;
        private System.Windows.Forms.DataGridViewTextBoxColumn QtyFront;
        private System.Windows.Forms.DataGridViewTextBoxColumn Record;
        private System.Windows.Forms.DataGridViewTextBoxColumn Computer;
    }
}