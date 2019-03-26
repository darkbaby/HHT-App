namespace FSBT.HHT.App.UI
{
    partial class DownloadMasterSummaryStorageLocationForm
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
            this.txtQuantityKG = new System.Windows.Forms.TextBox();
            this.label_TotalKG = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.groupSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupSummary
            // 
            this.groupSummary.Controls.Add(this.txtQuantityKG);
            this.groupSummary.Controls.Add(this.label_TotalKG);
            this.groupSummary.Controls.Add(this.txtQuantity);
            this.groupSummary.Controls.Add(this.dataGridView1);
            this.groupSummary.Controls.Add(this.lblTotal);
            this.groupSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupSummary.Location = new System.Drawing.Point(12, 11);
            this.groupSummary.Name = "groupSummary";
            this.groupSummary.Size = new System.Drawing.Size(639, 384);
            this.groupSummary.TabIndex = 2;
            this.groupSummary.TabStop = false;
            this.groupSummary.Text = "Summary";
            // 
            // txtQuantityKG
            // 
            this.txtQuantityKG.Location = new System.Drawing.Point(430, 339);
            this.txtQuantityKG.Name = "txtQuantityKG";
            this.txtQuantityKG.ReadOnly = true;
            this.txtQuantityKG.Size = new System.Drawing.Size(190, 24);
            this.txtQuantityKG.TabIndex = 19;
            this.txtQuantityKG.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtQuantityKG.Visible = false;
            // 
            // label_TotalKG
            // 
            this.label_TotalKG.AutoSize = true;
            this.label_TotalKG.Location = new System.Drawing.Point(332, 342);
            this.label_TotalKG.Name = "label_TotalKG";
            this.label_TotalKG.Size = new System.Drawing.Size(92, 18);
            this.label_TotalKG.TabIndex = 18;
            this.label_TotalKG.Text = "Total KG(W)";
            this.label_TotalKG.Visible = false;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Location = new System.Drawing.Point(132, 339);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.ReadOnly = true;
            this.txtQuantity.Size = new System.Drawing.Size(190, 24);
            this.txtQuantity.TabIndex = 5;
            this.txtQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 28);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(623, 302);
            this.dataGridView1.TabIndex = 2;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(16, 342);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(111, 18);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "Total PCS(G,F)";
            // 
            // DownloadMasterSummaryStorageLocationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 403);
            this.Controls.Add(this.groupSummary);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DownloadMasterSummaryStorageLocationForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DownloadMasterSummaryStorageLocationForm";
            this.groupSummary.ResumeLayout(false);
            this.groupSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.GroupBox groupSummary;
        private System.Windows.Forms.TextBox txtQuantityKG;
        private System.Windows.Forms.Label label_TotalKG;

    }
}