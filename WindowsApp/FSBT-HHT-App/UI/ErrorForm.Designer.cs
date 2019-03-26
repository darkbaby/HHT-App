namespace FSBT.HHT.App.UI
{
    partial class ErrorForm
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
            this.linkLog = new System.Windows.Forms.LinkLabel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.groupSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupSummary
            // 
            this.groupSummary.Controls.Add(this.linkLog);
            this.groupSummary.Controls.Add(this.dataGridView1);
            this.groupSummary.Controls.Add(this.lblTotal);
            this.groupSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupSummary.Location = new System.Drawing.Point(28, 39);
            this.groupSummary.Margin = new System.Windows.Forms.Padding(4);
            this.groupSummary.Name = "groupSummary";
            this.groupSummary.Padding = new System.Windows.Forms.Padding(4);
            this.groupSummary.Size = new System.Drawing.Size(1054, 509);
            this.groupSummary.TabIndex = 4;
            this.groupSummary.TabStop = false;
            this.groupSummary.Text = "Summary";
            // 
            // linkLog
            // 
            this.linkLog.AutoSize = true;
            this.linkLog.Location = new System.Drawing.Point(110, 462);
            this.linkLog.Name = "linkLog";
            this.linkLog.Size = new System.Drawing.Size(218, 24);
            this.linkLog.TabIndex = 11;
            this.linkLog.TabStop = true;
            this.linkLog.Text = "D:\\HHT\\Log\\20180531.txt";
            this.linkLog.Visible = false;
            this.linkLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLog_LinkClicked);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(25, 29);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1008, 419);
            this.dataGridView1.TabIndex = 9;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(26, 462);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(88, 24);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "Log File :";
            this.lblTotal.Visible = false;
            // 
            // ErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1116, 597);
            this.Controls.Add(this.groupSummary);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Summary";
            this.groupSummary.ResumeLayout(false);
            this.groupSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupSummary;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.LinkLabel linkLog;
    }
}