﻿namespace FSBT.HHT.App.UI
{
    partial class DeleteQtyReportForm
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
            this.deleteQtyCrystalReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // deleteQtyCrystalReportViewer
            // 
            this.deleteQtyCrystalReportViewer.ActiveViewIndex = -1;
            this.deleteQtyCrystalReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.deleteQtyCrystalReportViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.deleteQtyCrystalReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deleteQtyCrystalReportViewer.Location = new System.Drawing.Point(0, 0);
            this.deleteQtyCrystalReportViewer.Name = "deleteQtyCrystalReportViewer";
            this.deleteQtyCrystalReportViewer.Size = new System.Drawing.Size(821, 609);
            this.deleteQtyCrystalReportViewer.TabIndex = 0;
            this.deleteQtyCrystalReportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // DeleteQtyReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 609);
            this.Controls.Add(this.deleteQtyCrystalReportViewer);
            this.Name = "DeleteQtyReportForm";
            this.ShowIcon = false;
            this.Text = "DeleteQtyReportForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer deleteQtyCrystalReportViewer;
    }
}