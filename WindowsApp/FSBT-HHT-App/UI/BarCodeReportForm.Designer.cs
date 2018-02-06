namespace FSBT.HHT.App.UI
{
    partial class BarCodeReportForm
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
            this.barcodeCrystalReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // barcodeCrystalReportViewer
            // 
            this.barcodeCrystalReportViewer.ActiveViewIndex = -1;
            this.barcodeCrystalReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.barcodeCrystalReportViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.barcodeCrystalReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barcodeCrystalReportViewer.Location = new System.Drawing.Point(0, 0);
            this.barcodeCrystalReportViewer.Name = "barcodeCrystalReportViewer";
            this.barcodeCrystalReportViewer.Size = new System.Drawing.Size(821, 609);
            this.barcodeCrystalReportViewer.TabIndex = 0;
            this.barcodeCrystalReportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // BarCodeReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 609);
            this.Controls.Add(this.barcodeCrystalReportViewer);
            this.Name = "BarCodeReportForm";
            this.ShowIcon = false;
            this.Text = "BarCodeReportForm";
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer barcodeCrystalReportViewer;
    }
}