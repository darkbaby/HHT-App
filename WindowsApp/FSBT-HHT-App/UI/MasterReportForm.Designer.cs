namespace FSBT.HHT.App.UI
{
    partial class MasterReportForm
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
            this.masterReportCrystalReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // masterReportCrystalReportViewer
            // 
            this.masterReportCrystalReportViewer.ActiveViewIndex = -1;
            this.masterReportCrystalReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.masterReportCrystalReportViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.masterReportCrystalReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.masterReportCrystalReportViewer.Location = new System.Drawing.Point(0, 0);
            this.masterReportCrystalReportViewer.Name = "masterReportCrystalReportViewer";
            this.masterReportCrystalReportViewer.Size = new System.Drawing.Size(845, 685);
            this.masterReportCrystalReportViewer.TabIndex = 0;
            // 
            // MasterReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 685);
            this.Controls.Add(this.masterReportCrystalReportViewer);
            this.Name = "MasterReportForm";
            this.ShowIcon = false;
            this.Text = "MasterReportForm";
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer masterReportCrystalReportViewer;
    }
}