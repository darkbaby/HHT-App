namespace FSBT.HHT.App.UI
{
    partial class LocationReportForm
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
            this.LocationCrystalReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // LocationCrystalReportViewer
            // 
            this.LocationCrystalReportViewer.ActiveViewIndex = -1;
            this.LocationCrystalReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LocationCrystalReportViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.LocationCrystalReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LocationCrystalReportViewer.Location = new System.Drawing.Point(0, 0);
            this.LocationCrystalReportViewer.Name = "LocationCrystalReportViewer";
            this.LocationCrystalReportViewer.Size = new System.Drawing.Size(821, 609);
            this.LocationCrystalReportViewer.TabIndex = 0;
            this.LocationCrystalReportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // LocationReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 609);
            this.Controls.Add(this.LocationCrystalReportViewer);
            this.Name = "LocationReportForm";
            this.ShowIcon = false;
            this.Text = "LocationReportForm";
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer LocationCrystalReportViewer;
    }
}