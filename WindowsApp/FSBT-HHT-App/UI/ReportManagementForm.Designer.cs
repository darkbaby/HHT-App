namespace FSBT.HHT.App.UI
{
    partial class ReportManagementForm
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
            this.ReportManagementReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // ReportManagementReportViewer
            // 
            this.ReportManagementReportViewer.ActiveViewIndex = -1;
            this.ReportManagementReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ReportManagementReportViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.ReportManagementReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReportManagementReportViewer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReportManagementReportViewer.Location = new System.Drawing.Point(0, 0);
            this.ReportManagementReportViewer.Name = "ReportManagementReportViewer";
            this.ReportManagementReportViewer.Size = new System.Drawing.Size(843, 487);
            this.ReportManagementReportViewer.TabIndex = 0;
            // 
            // ReportManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 487);
            this.Controls.Add(this.ReportManagementReportViewer);
            this.Name = "ReportManagementForm";
            this.ShowIcon = false;
            this.Text = "ReportManagementForm";
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer ReportManagementReportViewer;
    }
}