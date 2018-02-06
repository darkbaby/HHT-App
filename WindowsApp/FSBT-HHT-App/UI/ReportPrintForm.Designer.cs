namespace FSBT.HHT.App.UI
{
    partial class ReportPrintForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstReport = new System.Windows.Forms.ListBox();
            this.gbReportFilter = new System.Windows.Forms.GroupBox();
            this.gbDepartment = new System.Windows.Forms.GroupBox();
            this.departmentCode = new System.Windows.Forms.TextBox();
            this.rbDepartmentCode = new System.Windows.Forms.RadioButton();
            this.rbDepartmentAll = new System.Windows.Forms.RadioButton();
            this.btnPreview = new System.Windows.Forms.Button();
            this.gbUnit = new System.Windows.Forms.GroupBox();
            this.unitPiece = new System.Windows.Forms.RadioButton();
            this.unitPack = new System.Windows.Forms.RadioButton();
            this.gbCorrectionDel = new System.Windows.Forms.GroupBox();
            this.cdAdd = new System.Windows.Forms.CheckBox();
            this.cdDelete = new System.Windows.Forms.CheckBox();
            this.cdCorrection = new System.Windows.Forms.CheckBox();
            this.cdNoCorrection = new System.Windows.Forms.CheckBox();
            this.gbDiff = new System.Windows.Forms.GroupBox();
            this.dtNoDiff = new System.Windows.Forms.CheckBox();
            this.dtOver = new System.Windows.Forms.CheckBox();
            this.dtShortage = new System.Windows.Forms.CheckBox();
            this.gbStoreType = new System.Windows.Forms.GroupBox();
            this.stWarehouse = new System.Windows.Forms.CheckBox();
            this.stFreshFood = new System.Windows.Forms.CheckBox();
            this.stBack = new System.Windows.Forms.CheckBox();
            this.stFront = new System.Windows.Forms.CheckBox();
            this.gbBarcode = new System.Windows.Forms.GroupBox();
            this.barcode = new System.Windows.Forms.TextBox();
            this.rbBarcode = new System.Windows.Forms.RadioButton();
            this.rbBarcodeAll = new System.Windows.Forms.RadioButton();
            this.gbBrand = new System.Windows.Forms.GroupBox();
            this.brandCode = new System.Windows.Forms.ComboBox();
            this.rbBrand = new System.Windows.Forms.RadioButton();
            this.rbBrandAll = new System.Windows.Forms.RadioButton();
            this.gbLocation = new System.Windows.Forms.GroupBox();
            this.location = new System.Windows.Forms.TextBox();
            this.locationTo = new System.Windows.Forms.TextBox();
            this.lbllocationTo = new System.Windows.Forms.Label();
            this.locationFrom = new System.Windows.Forms.TextBox();
            this.rbLocation = new System.Windows.Forms.RadioButton();
            this.rbLocationFromTo = new System.Windows.Forms.RadioButton();
            this.rbLocationAll = new System.Windows.Forms.RadioButton();
            this.gbSection = new System.Windows.Forms.GroupBox();
            this.sectionCode = new System.Windows.Forms.TextBox();
            this.rbSectionCode = new System.Windows.Forms.RadioButton();
            this.rbSectionAll = new System.Windows.Forms.RadioButton();
            this.countDate = new System.Windows.Forms.DateTimePicker();
            this.lblcountDate = new System.Windows.Forms.Label();
            this.lblReportName = new System.Windows.Forms.Label();
            this.lblReport = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbReportFilter.SuspendLayout();
            this.gbDepartment.SuspendLayout();
            this.gbUnit.SuspendLayout();
            this.gbCorrectionDel.SuspendLayout();
            this.gbDiff.SuspendLayout();
            this.gbStoreType.SuspendLayout();
            this.gbBarcode.SuspendLayout();
            this.gbBrand.SuspendLayout();
            this.gbLocation.SuspendLayout();
            this.gbSection.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.62213F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.37787F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbReportFilter, 1, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(18, 18);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1450, 824);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lstReport);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(4, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(552, 814);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Report";
            // 
            // lstReport
            // 
            this.lstReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstReport.FormattingEnabled = true;
            this.lstReport.ItemHeight = 29;
            this.lstReport.Items.AddRange(new object[] {
            "test1",
            "test2",
            "test3",
            "test4"});
            this.lstReport.Location = new System.Drawing.Point(8, 36);
            this.lstReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstReport.Name = "lstReport";
            this.lstReport.Size = new System.Drawing.Size(532, 613);
            this.lstReport.TabIndex = 0;
            this.lstReport.SelectedIndexChanged += new System.EventHandler(this.lstReport_SelectedIndexChanged);
            // 
            // gbReportFilter
            // 
            this.gbReportFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbReportFilter.AutoSize = true;
            this.gbReportFilter.Controls.Add(this.gbDepartment);
            this.gbReportFilter.Controls.Add(this.btnPreview);
            this.gbReportFilter.Controls.Add(this.gbUnit);
            this.gbReportFilter.Controls.Add(this.gbCorrectionDel);
            this.gbReportFilter.Controls.Add(this.gbDiff);
            this.gbReportFilter.Controls.Add(this.gbStoreType);
            this.gbReportFilter.Controls.Add(this.gbBarcode);
            this.gbReportFilter.Controls.Add(this.gbBrand);
            this.gbReportFilter.Controls.Add(this.gbLocation);
            this.gbReportFilter.Controls.Add(this.gbSection);
            this.gbReportFilter.Controls.Add(this.countDate);
            this.gbReportFilter.Controls.Add(this.lblcountDate);
            this.gbReportFilter.Controls.Add(this.lblReportName);
            this.gbReportFilter.Controls.Add(this.lblReport);
            this.gbReportFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbReportFilter.Location = new System.Drawing.Point(564, 5);
            this.gbReportFilter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbReportFilter.Name = "gbReportFilter";
            this.gbReportFilter.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbReportFilter.Size = new System.Drawing.Size(882, 814);
            this.gbReportFilter.TabIndex = 1;
            this.gbReportFilter.TabStop = false;
            this.gbReportFilter.Text = "Report Filter";
            // 
            // gbDepartment
            // 
            this.gbDepartment.Controls.Add(this.departmentCode);
            this.gbDepartment.Controls.Add(this.rbDepartmentCode);
            this.gbDepartment.Controls.Add(this.rbDepartmentAll);
            this.gbDepartment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDepartment.Location = new System.Drawing.Point(18, 100);
            this.gbDepartment.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbDepartment.Name = "gbDepartment";
            this.gbDepartment.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbDepartment.Size = new System.Drawing.Size(850, 72);
            this.gbDepartment.TabIndex = 12;
            this.gbDepartment.TabStop = false;
            this.gbDepartment.Text = "Department Code";
            // 
            // departmentCode
            // 
            this.departmentCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.departmentCode.Location = new System.Drawing.Point(376, 26);
            this.departmentCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.departmentCode.Name = "departmentCode";
            this.departmentCode.Size = new System.Drawing.Size(439, 33);
            this.departmentCode.TabIndex = 2;
            this.departmentCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.departmentCode_KeyPress);
            // 
            // rbDepartmentCode
            // 
            this.rbDepartmentCode.AutoSize = true;
            this.rbDepartmentCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDepartmentCode.Location = new System.Drawing.Point(153, 31);
            this.rbDepartmentCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbDepartmentCode.Name = "rbDepartmentCode";
            this.rbDepartmentCode.Size = new System.Drawing.Size(191, 29);
            this.rbDepartmentCode.TabIndex = 1;
            this.rbDepartmentCode.TabStop = true;
            this.rbDepartmentCode.Text = "Department Code";
            this.rbDepartmentCode.UseVisualStyleBackColor = true;
            this.rbDepartmentCode.CheckedChanged += new System.EventHandler(this.rbDepartmentCode_CheckedChanged);
            // 
            // rbDepartmentAll
            // 
            this.rbDepartmentAll.AutoSize = true;
            this.rbDepartmentAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDepartmentAll.Location = new System.Drawing.Point(10, 31);
            this.rbDepartmentAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbDepartmentAll.Name = "rbDepartmentAll";
            this.rbDepartmentAll.Size = new System.Drawing.Size(59, 29);
            this.rbDepartmentAll.TabIndex = 0;
            this.rbDepartmentAll.TabStop = true;
            this.rbDepartmentAll.Text = "All";
            this.rbDepartmentAll.UseVisualStyleBackColor = true;
            this.rbDepartmentAll.CheckedChanged += new System.EventHandler(this.rbDepartmentAll_CheckedChanged);
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreview.Location = new System.Drawing.Point(756, 55);
            this.btnPreview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(112, 46);
            this.btnPreview.TabIndex = 11;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // gbUnit
            // 
            this.gbUnit.Controls.Add(this.unitPiece);
            this.gbUnit.Controls.Add(this.unitPack);
            this.gbUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbUnit.Location = new System.Drawing.Point(18, 740);
            this.gbUnit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbUnit.Name = "gbUnit";
            this.gbUnit.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbUnit.Size = new System.Drawing.Size(852, 66);
            this.gbUnit.TabIndex = 10;
            this.gbUnit.TabStop = false;
            this.gbUnit.Text = "Unit";
            // 
            // unitPiece
            // 
            this.unitPiece.AutoSize = true;
            this.unitPiece.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unitPiece.Location = new System.Drawing.Point(184, 23);
            this.unitPiece.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.unitPiece.Name = "unitPiece";
            this.unitPiece.Size = new System.Drawing.Size(86, 29);
            this.unitPiece.TabIndex = 4;
            this.unitPiece.TabStop = true;
            this.unitPiece.Text = "Piece";
            this.unitPiece.UseVisualStyleBackColor = true;
            // 
            // unitPack
            // 
            this.unitPack.AutoSize = true;
            this.unitPack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unitPack.Location = new System.Drawing.Point(10, 23);
            this.unitPack.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.unitPack.Name = "unitPack";
            this.unitPack.Size = new System.Drawing.Size(81, 29);
            this.unitPack.TabIndex = 3;
            this.unitPack.TabStop = true;
            this.unitPack.Text = "Pack";
            this.unitPack.UseVisualStyleBackColor = true;
            // 
            // gbCorrectionDel
            // 
            this.gbCorrectionDel.Controls.Add(this.cdAdd);
            this.gbCorrectionDel.Controls.Add(this.cdDelete);
            this.gbCorrectionDel.Controls.Add(this.cdCorrection);
            this.gbCorrectionDel.Controls.Add(this.cdNoCorrection);
            this.gbCorrectionDel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbCorrectionDel.Location = new System.Drawing.Point(18, 672);
            this.gbCorrectionDel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbCorrectionDel.Name = "gbCorrectionDel";
            this.gbCorrectionDel.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbCorrectionDel.Size = new System.Drawing.Size(852, 66);
            this.gbCorrectionDel.TabIndex = 9;
            this.gbCorrectionDel.TabStop = false;
            this.gbCorrectionDel.Text = "Correction/Delete";
            // 
            // cdAdd
            // 
            this.cdAdd.AutoSize = true;
            this.cdAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cdAdd.Location = new System.Drawing.Point(580, 23);
            this.cdAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cdAdd.Name = "cdAdd";
            this.cdAdd.Size = new System.Drawing.Size(74, 29);
            this.cdAdd.TabIndex = 3;
            this.cdAdd.Text = "Add";
            this.cdAdd.UseVisualStyleBackColor = true;
            // 
            // cdDelete
            // 
            this.cdDelete.AutoSize = true;
            this.cdDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cdDelete.Location = new System.Drawing.Point(374, 23);
            this.cdDelete.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cdDelete.Name = "cdDelete";
            this.cdDelete.Size = new System.Drawing.Size(94, 29);
            this.cdDelete.TabIndex = 2;
            this.cdDelete.Text = "Delete";
            this.cdDelete.UseVisualStyleBackColor = true;
            // 
            // cdCorrection
            // 
            this.cdCorrection.AutoSize = true;
            this.cdCorrection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cdCorrection.Location = new System.Drawing.Point(184, 25);
            this.cdCorrection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cdCorrection.Name = "cdCorrection";
            this.cdCorrection.Size = new System.Drawing.Size(128, 29);
            this.cdCorrection.TabIndex = 1;
            this.cdCorrection.Text = "Correction";
            this.cdCorrection.UseVisualStyleBackColor = true;
            // 
            // cdNoCorrection
            // 
            this.cdNoCorrection.AutoSize = true;
            this.cdNoCorrection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cdNoCorrection.Location = new System.Drawing.Point(10, 25);
            this.cdNoCorrection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cdNoCorrection.Name = "cdNoCorrection";
            this.cdNoCorrection.Size = new System.Drawing.Size(60, 29);
            this.cdNoCorrection.TabIndex = 0;
            this.cdNoCorrection.Text = "All";
            this.cdNoCorrection.UseVisualStyleBackColor = true;
            // 
            // gbDiff
            // 
            this.gbDiff.Controls.Add(this.dtNoDiff);
            this.gbDiff.Controls.Add(this.dtOver);
            this.gbDiff.Controls.Add(this.dtShortage);
            this.gbDiff.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDiff.Location = new System.Drawing.Point(18, 604);
            this.gbDiff.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbDiff.Name = "gbDiff";
            this.gbDiff.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbDiff.Size = new System.Drawing.Size(852, 66);
            this.gbDiff.TabIndex = 9;
            this.gbDiff.TabStop = false;
            this.gbDiff.Text = "Diff Type";
            // 
            // dtNoDiff
            // 
            this.dtNoDiff.AutoSize = true;
            this.dtNoDiff.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtNoDiff.Location = new System.Drawing.Point(10, 23);
            this.dtNoDiff.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtNoDiff.Name = "dtNoDiff";
            this.dtNoDiff.Size = new System.Drawing.Size(60, 29);
            this.dtNoDiff.TabIndex = 2;
            this.dtNoDiff.Text = "All";
            this.dtNoDiff.UseVisualStyleBackColor = true;
            // 
            // dtOver
            // 
            this.dtOver.AutoSize = true;
            this.dtOver.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtOver.Location = new System.Drawing.Point(374, 23);
            this.dtOver.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtOver.Name = "dtOver";
            this.dtOver.Size = new System.Drawing.Size(81, 29);
            this.dtOver.TabIndex = 1;
            this.dtOver.Text = "Over";
            this.dtOver.UseVisualStyleBackColor = true;
            // 
            // dtShortage
            // 
            this.dtShortage.AutoSize = true;
            this.dtShortage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtShortage.Location = new System.Drawing.Point(184, 23);
            this.dtShortage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtShortage.Name = "dtShortage";
            this.dtShortage.Size = new System.Drawing.Size(118, 29);
            this.dtShortage.TabIndex = 0;
            this.dtShortage.Text = "Shortage";
            this.dtShortage.UseVisualStyleBackColor = true;
            // 
            // gbStoreType
            // 
            this.gbStoreType.Controls.Add(this.stWarehouse);
            this.gbStoreType.Controls.Add(this.stFreshFood);
            this.gbStoreType.Controls.Add(this.stBack);
            this.gbStoreType.Controls.Add(this.stFront);
            this.gbStoreType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbStoreType.Location = new System.Drawing.Point(18, 537);
            this.gbStoreType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbStoreType.Name = "gbStoreType";
            this.gbStoreType.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbStoreType.Size = new System.Drawing.Size(852, 66);
            this.gbStoreType.TabIndex = 8;
            this.gbStoreType.TabStop = false;
            this.gbStoreType.Text = "Store Type";
            // 
            // stWarehouse
            // 
            this.stWarehouse.AutoSize = true;
            this.stWarehouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stWarehouse.Location = new System.Drawing.Point(374, 28);
            this.stWarehouse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stWarehouse.Name = "stWarehouse";
            this.stWarehouse.Size = new System.Drawing.Size(140, 29);
            this.stWarehouse.TabIndex = 3;
            this.stWarehouse.Text = "Warehouse";
            this.stWarehouse.UseVisualStyleBackColor = true;
            // 
            // stFreshFood
            // 
            this.stFreshFood.AutoSize = true;
            this.stFreshFood.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stFreshFood.Location = new System.Drawing.Point(580, 28);
            this.stFreshFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stFreshFood.Name = "stFreshFood";
            this.stFreshFood.Size = new System.Drawing.Size(138, 29);
            this.stFreshFood.TabIndex = 2;
            this.stFreshFood.Text = "Fresh Food";
            this.stFreshFood.UseVisualStyleBackColor = true;
            // 
            // stBack
            // 
            this.stBack.AutoSize = true;
            this.stBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stBack.Location = new System.Drawing.Point(184, 28);
            this.stBack.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stBack.Name = "stBack";
            this.stBack.Size = new System.Drawing.Size(134, 29);
            this.stBack.TabIndex = 1;
            this.stBack.Text = "Back Store";
            this.stBack.UseVisualStyleBackColor = true;
            // 
            // stFront
            // 
            this.stFront.AutoSize = true;
            this.stFront.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stFront.Location = new System.Drawing.Point(10, 28);
            this.stFront.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stFront.Name = "stFront";
            this.stFront.Size = new System.Drawing.Size(135, 29);
            this.stFront.TabIndex = 0;
            this.stFront.Text = "Front Store";
            this.stFront.UseVisualStyleBackColor = true;
            // 
            // gbBarcode
            // 
            this.gbBarcode.Controls.Add(this.barcode);
            this.gbBarcode.Controls.Add(this.rbBarcode);
            this.gbBarcode.Controls.Add(this.rbBarcodeAll);
            this.gbBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbBarcode.Location = new System.Drawing.Point(18, 470);
            this.gbBarcode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbBarcode.Name = "gbBarcode";
            this.gbBarcode.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbBarcode.Size = new System.Drawing.Size(852, 66);
            this.gbBarcode.TabIndex = 7;
            this.gbBarcode.TabStop = false;
            this.gbBarcode.Text = "Barcode";
            // 
            // barcode
            // 
            this.barcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.barcode.Location = new System.Drawing.Point(320, 20);
            this.barcode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barcode.Name = "barcode";
            this.barcode.Size = new System.Drawing.Size(496, 33);
            this.barcode.TabIndex = 2;
            this.barcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.barcode_KeyPress);
            // 
            // rbBarcode
            // 
            this.rbBarcode.AutoSize = true;
            this.rbBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBarcode.Location = new System.Drawing.Point(153, 26);
            this.rbBarcode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbBarcode.Name = "rbBarcode";
            this.rbBarcode.Size = new System.Drawing.Size(110, 29);
            this.rbBarcode.TabIndex = 1;
            this.rbBarcode.TabStop = true;
            this.rbBarcode.Text = "Barcode";
            this.rbBarcode.UseVisualStyleBackColor = true;
            this.rbBarcode.CheckedChanged += new System.EventHandler(this.rbBarcode_CheckedChanged);
            // 
            // rbBarcodeAll
            // 
            this.rbBarcodeAll.AutoSize = true;
            this.rbBarcodeAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBarcodeAll.Location = new System.Drawing.Point(10, 26);
            this.rbBarcodeAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbBarcodeAll.Name = "rbBarcodeAll";
            this.rbBarcodeAll.Size = new System.Drawing.Size(59, 29);
            this.rbBarcodeAll.TabIndex = 0;
            this.rbBarcodeAll.TabStop = true;
            this.rbBarcodeAll.Text = "All";
            this.rbBarcodeAll.UseVisualStyleBackColor = true;
            this.rbBarcodeAll.CheckedChanged += new System.EventHandler(this.rbBarcodeAll_CheckedChanged);
            // 
            // gbBrand
            // 
            this.gbBrand.Controls.Add(this.brandCode);
            this.gbBrand.Controls.Add(this.rbBrand);
            this.gbBrand.Controls.Add(this.rbBrandAll);
            this.gbBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbBrand.Location = new System.Drawing.Point(18, 399);
            this.gbBrand.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbBrand.Name = "gbBrand";
            this.gbBrand.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbBrand.Size = new System.Drawing.Size(852, 69);
            this.gbBrand.TabIndex = 6;
            this.gbBrand.TabStop = false;
            this.gbBrand.Text = "Brand Code";
            // 
            // brandCode
            // 
            this.brandCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brandCode.FormattingEnabled = true;
            this.brandCode.Location = new System.Drawing.Point(320, 20);
            this.brandCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.brandCode.Name = "brandCode";
            this.brandCode.Size = new System.Drawing.Size(496, 37);
            this.brandCode.TabIndex = 13;
            // 
            // rbBrand
            // 
            this.rbBrand.AutoSize = true;
            this.rbBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBrand.Location = new System.Drawing.Point(153, 29);
            this.rbBrand.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbBrand.Name = "rbBrand";
            this.rbBrand.Size = new System.Drawing.Size(142, 29);
            this.rbBrand.TabIndex = 1;
            this.rbBrand.TabStop = true;
            this.rbBrand.Text = "Brand Code";
            this.rbBrand.UseVisualStyleBackColor = true;
            this.rbBrand.CheckedChanged += new System.EventHandler(this.rbBrand_CheckedChanged);
            // 
            // rbBrandAll
            // 
            this.rbBrandAll.AutoSize = true;
            this.rbBrandAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBrandAll.Location = new System.Drawing.Point(10, 29);
            this.rbBrandAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbBrandAll.Name = "rbBrandAll";
            this.rbBrandAll.Size = new System.Drawing.Size(59, 29);
            this.rbBrandAll.TabIndex = 0;
            this.rbBrandAll.TabStop = true;
            this.rbBrandAll.Text = "All";
            this.rbBrandAll.UseVisualStyleBackColor = true;
            this.rbBrandAll.CheckedChanged += new System.EventHandler(this.rbBrandAll_CheckedChanged);
            // 
            // gbLocation
            // 
            this.gbLocation.Controls.Add(this.location);
            this.gbLocation.Controls.Add(this.locationTo);
            this.gbLocation.Controls.Add(this.lbllocationTo);
            this.gbLocation.Controls.Add(this.locationFrom);
            this.gbLocation.Controls.Add(this.rbLocation);
            this.gbLocation.Controls.Add(this.rbLocationFromTo);
            this.gbLocation.Controls.Add(this.rbLocationAll);
            this.gbLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbLocation.Location = new System.Drawing.Point(18, 245);
            this.gbLocation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbLocation.Name = "gbLocation";
            this.gbLocation.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbLocation.Size = new System.Drawing.Size(852, 152);
            this.gbLocation.TabIndex = 6;
            this.gbLocation.TabStop = false;
            this.gbLocation.Text = "Location";
            // 
            // location
            // 
            this.location.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.location.Location = new System.Drawing.Point(204, 102);
            this.location.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.location.Name = "location";
            this.location.Size = new System.Drawing.Size(612, 33);
            this.location.TabIndex = 7;
            this.location.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.location_KeyPress);
            // 
            // locationTo
            // 
            this.locationTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.locationTo.Location = new System.Drawing.Point(530, 60);
            this.locationTo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.locationTo.MaxLength = 5;
            this.locationTo.Name = "locationTo";
            this.locationTo.Size = new System.Drawing.Size(148, 33);
            this.locationTo.TabIndex = 6;
            this.locationTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.locationTo_KeyPress);
            // 
            // lbllocationTo
            // 
            this.lbllocationTo.AutoSize = true;
            this.lbllocationTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbllocationTo.Location = new System.Drawing.Point(372, 65);
            this.lbllocationTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbllocationTo.Name = "lbllocationTo";
            this.lbllocationTo.Size = new System.Drawing.Size(115, 25);
            this.lbllocationTo.TabIndex = 5;
            this.lbllocationTo.Text = "Location To";
            // 
            // locationFrom
            // 
            this.locationFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.locationFrom.Location = new System.Drawing.Point(204, 60);
            this.locationFrom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.locationFrom.MaxLength = 5;
            this.locationFrom.Name = "locationFrom";
            this.locationFrom.Size = new System.Drawing.Size(148, 33);
            this.locationFrom.TabIndex = 4;
            this.locationFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.locationFrom_KeyPress);
            // 
            // rbLocation
            // 
            this.rbLocation.AutoSize = true;
            this.rbLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLocation.Location = new System.Drawing.Point(10, 106);
            this.rbLocation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbLocation.Name = "rbLocation";
            this.rbLocation.Size = new System.Drawing.Size(111, 29);
            this.rbLocation.TabIndex = 3;
            this.rbLocation.TabStop = true;
            this.rbLocation.Text = "Location";
            this.rbLocation.UseVisualStyleBackColor = true;
            this.rbLocation.CheckedChanged += new System.EventHandler(this.rbLocation_CheckedChanged);
            // 
            // rbLocationFromTo
            // 
            this.rbLocationFromTo.AutoSize = true;
            this.rbLocationFromTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLocationFromTo.Location = new System.Drawing.Point(10, 65);
            this.rbLocationFromTo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbLocationFromTo.Name = "rbLocationFromTo";
            this.rbLocationFromTo.Size = new System.Drawing.Size(161, 29);
            this.rbLocationFromTo.TabIndex = 2;
            this.rbLocationFromTo.TabStop = true;
            this.rbLocationFromTo.Text = "Location From";
            this.rbLocationFromTo.UseVisualStyleBackColor = true;
            this.rbLocationFromTo.CheckedChanged += new System.EventHandler(this.rbLocationFromTo_CheckedChanged);
            // 
            // rbLocationAll
            // 
            this.rbLocationAll.AutoSize = true;
            this.rbLocationAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLocationAll.Location = new System.Drawing.Point(10, 29);
            this.rbLocationAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbLocationAll.Name = "rbLocationAll";
            this.rbLocationAll.Size = new System.Drawing.Size(59, 29);
            this.rbLocationAll.TabIndex = 1;
            this.rbLocationAll.TabStop = true;
            this.rbLocationAll.Text = "All";
            this.rbLocationAll.UseVisualStyleBackColor = true;
            this.rbLocationAll.CheckedChanged += new System.EventHandler(this.rbLocationAll_CheckedChanged);
            // 
            // gbSection
            // 
            this.gbSection.Controls.Add(this.sectionCode);
            this.gbSection.Controls.Add(this.rbSectionCode);
            this.gbSection.Controls.Add(this.rbSectionAll);
            this.gbSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbSection.Location = new System.Drawing.Point(18, 174);
            this.gbSection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbSection.Name = "gbSection";
            this.gbSection.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbSection.Size = new System.Drawing.Size(852, 69);
            this.gbSection.TabIndex = 5;
            this.gbSection.TabStop = false;
            this.gbSection.Text = "Section Code";
            // 
            // sectionCode
            // 
            this.sectionCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sectionCode.Location = new System.Drawing.Point(376, 23);
            this.sectionCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sectionCode.Name = "sectionCode";
            this.sectionCode.Size = new System.Drawing.Size(439, 33);
            this.sectionCode.TabIndex = 2;
            this.sectionCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.sectionCode_KeyPress);
            // 
            // rbSectionCode
            // 
            this.rbSectionCode.AutoSize = true;
            this.rbSectionCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSectionCode.Location = new System.Drawing.Point(153, 29);
            this.rbSectionCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbSectionCode.Name = "rbSectionCode";
            this.rbSectionCode.Size = new System.Drawing.Size(156, 29);
            this.rbSectionCode.TabIndex = 1;
            this.rbSectionCode.TabStop = true;
            this.rbSectionCode.Text = "Section Code";
            this.rbSectionCode.UseVisualStyleBackColor = true;
            this.rbSectionCode.CheckedChanged += new System.EventHandler(this.rbSectionCode_CheckedChanged);
            // 
            // rbSectionAll
            // 
            this.rbSectionAll.AutoSize = true;
            this.rbSectionAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSectionAll.Location = new System.Drawing.Point(10, 29);
            this.rbSectionAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbSectionAll.Name = "rbSectionAll";
            this.rbSectionAll.Size = new System.Drawing.Size(59, 29);
            this.rbSectionAll.TabIndex = 0;
            this.rbSectionAll.TabStop = true;
            this.rbSectionAll.Text = "All";
            this.rbSectionAll.UseVisualStyleBackColor = true;
            this.rbSectionAll.CheckedChanged += new System.EventHandler(this.rbSectionAll_CheckedChanged);
            // 
            // countDate
            // 
            this.countDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.countDate.Location = new System.Drawing.Point(171, 65);
            this.countDate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.countDate.Name = "countDate";
            this.countDate.Size = new System.Drawing.Size(402, 33);
            this.countDate.TabIndex = 4;
            this.countDate.ValueChanged += new System.EventHandler(this.countDate_ValueChanged);
            // 
            // lblcountDate
            // 
            this.lblcountDate.AutoSize = true;
            this.lblcountDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcountDate.Location = new System.Drawing.Point(24, 69);
            this.lblcountDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblcountDate.Name = "lblcountDate";
            this.lblcountDate.Size = new System.Drawing.Size(111, 25);
            this.lblcountDate.TabIndex = 3;
            this.lblcountDate.Text = "Count Date";
            // 
            // lblReportName
            // 
            this.lblReportName.AutoSize = true;
            this.lblReportName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportName.Location = new System.Drawing.Point(110, 29);
            this.lblReportName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReportName.Name = "lblReportName";
            this.lblReportName.Size = new System.Drawing.Size(103, 29);
            this.lblReportName.TabIndex = 2;
            this.lblReportName.Text = "asdfghjk";
            // 
            // lblReport
            // 
            this.lblReport.AutoSize = true;
            this.lblReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReport.Location = new System.Drawing.Point(14, 29);
            this.lblReport.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReport.Name = "lblReport";
            this.lblReport.Size = new System.Drawing.Size(85, 25);
            this.lblReport.TabIndex = 1;
            this.lblReport.Text = "Report : ";
            // 
            // ReportPrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1512, 894);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ReportPrintForm";
            this.ShowIcon = false;
            this.Text = "Print Report";
            this.Load += new System.EventHandler(this.ReportPrintForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.gbReportFilter.ResumeLayout(false);
            this.gbReportFilter.PerformLayout();
            this.gbDepartment.ResumeLayout(false);
            this.gbDepartment.PerformLayout();
            this.gbUnit.ResumeLayout(false);
            this.gbUnit.PerformLayout();
            this.gbCorrectionDel.ResumeLayout(false);
            this.gbCorrectionDel.PerformLayout();
            this.gbDiff.ResumeLayout(false);
            this.gbDiff.PerformLayout();
            this.gbStoreType.ResumeLayout(false);
            this.gbStoreType.PerformLayout();
            this.gbBarcode.ResumeLayout(false);
            this.gbBarcode.PerformLayout();
            this.gbBrand.ResumeLayout(false);
            this.gbBrand.PerformLayout();
            this.gbLocation.ResumeLayout(false);
            this.gbLocation.PerformLayout();
            this.gbSection.ResumeLayout(false);
            this.gbSection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstReport;
        private System.Windows.Forms.GroupBox gbReportFilter;
        private System.Windows.Forms.Label lblReportName;
        private System.Windows.Forms.Label lblReport;
        private System.Windows.Forms.GroupBox gbSection;
        private System.Windows.Forms.TextBox sectionCode;
        private System.Windows.Forms.RadioButton rbSectionCode;
        private System.Windows.Forms.RadioButton rbSectionAll;
        private System.Windows.Forms.DateTimePicker countDate;
        private System.Windows.Forms.Label lblcountDate;
        private System.Windows.Forms.GroupBox gbLocation;
        private System.Windows.Forms.RadioButton rbLocation;
        private System.Windows.Forms.RadioButton rbLocationFromTo;
        private System.Windows.Forms.RadioButton rbLocationAll;
        private System.Windows.Forms.TextBox location;
        private System.Windows.Forms.TextBox locationTo;
        private System.Windows.Forms.Label lbllocationTo;
        private System.Windows.Forms.TextBox locationFrom;
        private System.Windows.Forms.GroupBox gbBrand;
        private System.Windows.Forms.RadioButton rbBrand;
        private System.Windows.Forms.RadioButton rbBrandAll;
        private System.Windows.Forms.GroupBox gbBarcode;
        private System.Windows.Forms.TextBox barcode;
        private System.Windows.Forms.RadioButton rbBarcode;
        private System.Windows.Forms.RadioButton rbBarcodeAll;
        private System.Windows.Forms.GroupBox gbStoreType;
        private System.Windows.Forms.CheckBox stWarehouse;
        private System.Windows.Forms.CheckBox stFreshFood;
        private System.Windows.Forms.CheckBox stBack;
        private System.Windows.Forms.CheckBox stFront;
        private System.Windows.Forms.GroupBox gbCorrectionDel;
        private System.Windows.Forms.CheckBox cdAdd;
        private System.Windows.Forms.CheckBox cdDelete;
        private System.Windows.Forms.CheckBox cdCorrection;
        private System.Windows.Forms.CheckBox cdNoCorrection;
        private System.Windows.Forms.GroupBox gbDiff;
        private System.Windows.Forms.CheckBox dtNoDiff;
        private System.Windows.Forms.CheckBox dtOver;
        private System.Windows.Forms.CheckBox dtShortage;
        private System.Windows.Forms.GroupBox gbUnit;
        private System.Windows.Forms.RadioButton unitPiece;
        private System.Windows.Forms.RadioButton unitPack;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.GroupBox gbDepartment;
        private System.Windows.Forms.TextBox departmentCode;
        private System.Windows.Forms.RadioButton rbDepartmentCode;
        private System.Windows.Forms.RadioButton rbDepartmentAll;
        private System.Windows.Forms.ComboBox brandCode;
    }
}