namespace FSBT.HHT.App.UI
{
    partial class SerialNumberReportForm
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
            this.SerialNumberCrystalReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();

            this.SerialNumberCrystalReportViewer.ActiveViewIndex = -1;
            this.SerialNumberCrystalReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SerialNumberCrystalReportViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.SerialNumberCrystalReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SerialNumberCrystalReportViewer.Location = new System.Drawing.Point(0, 0);
            this.SerialNumberCrystalReportViewer.Name = "SerialNumberCrystalReportViewer";
            this.SerialNumberCrystalReportViewer.Size = new System.Drawing.Size(821, 609);
            this.SerialNumberCrystalReportViewer.TabIndex = 0;
            this.SerialNumberCrystalReportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;


            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 609);
            this.Controls.Add(this.SerialNumberCrystalReportViewer);
            this.Name = "SerialNumberReportForm";
            this.ShowIcon = false;
            this.Text = "SerialNumberReportForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer SerialNumberCrystalReportViewer;
    }
}