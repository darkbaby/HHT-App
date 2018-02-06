namespace FSBT.HHT.App.UI
{
    partial class LocationManagementForm
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
            this.groupLocationType = new System.Windows.Forms.GroupBox();
            this.chLocationType4 = new System.Windows.Forms.CheckBox();
            this.chLocationType3 = new System.Windows.Forms.CheckBox();
            this.chLocationType2 = new System.Windows.Forms.CheckBox();
            this.chLocationType1 = new System.Windows.Forms.CheckBox();
            this.groupSearch = new System.Windows.Forms.GroupBox();
            this.lbDept = new System.Windows.Forms.Label();
            this.textBoxDept = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.textBoxLoTo = new System.Windows.Forms.TextBox();
            this.lbLocationTo = new System.Windows.Forms.Label();
            this.lbLocationFrom = new System.Windows.Forms.Label();
            this.lbSectionName = new System.Windows.Forms.Label();
            this.lbSectionCode = new System.Windows.Forms.Label();
            this.textBoxLoForm = new System.Windows.Forms.TextBox();
            this.textBoxSecName = new System.Windows.Forms.TextBox();
            this.textBoxSecCode = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupResult = new System.Windows.Forms.GroupBox();
            this.btnReportBarcode = new System.Windows.Forms.Button();
            this.btnLoadBrand = new System.Windows.Forms.Button();
            this.lbFilterSearch = new System.Windows.Forms.Label();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnReportLocation = new System.Windows.Forms.Button();
            this.btnImportSection = new System.Windows.Forms.Button();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeptCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SectionType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.OriginSectionCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SectionCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SectionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocationFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocationTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BrandCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Flag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginSectionType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupLocationType.SuspendLayout();
            this.groupSearch.SuspendLayout();
            this.groupResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 950F));
            this.tableLayoutPanel1.Controls.Add(this.groupLocationType, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupSearch, 1, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(18, 18);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1250, 211);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupLocationType
            // 
            this.groupLocationType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupLocationType.AutoSize = true;
            this.groupLocationType.Controls.Add(this.chLocationType4);
            this.groupLocationType.Controls.Add(this.chLocationType3);
            this.groupLocationType.Controls.Add(this.chLocationType2);
            this.groupLocationType.Controls.Add(this.chLocationType1);
            this.groupLocationType.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupLocationType.Location = new System.Drawing.Point(4, 5);
            this.groupLocationType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupLocationType.Name = "groupLocationType";
            this.groupLocationType.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupLocationType.Size = new System.Drawing.Size(292, 201);
            this.groupLocationType.TabIndex = 1;
            this.groupLocationType.TabStop = false;
            this.groupLocationType.Text = "Section Type";
            // 
            // chLocationType4
            // 
            this.chLocationType4.AutoSize = true;
            this.chLocationType4.Location = new System.Drawing.Point(26, 154);
            this.chLocationType4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chLocationType4.Name = "chLocationType4";
            this.chLocationType4.Size = new System.Drawing.Size(148, 30);
            this.chLocationType4.TabIndex = 6;
            this.chLocationType4.Tag = "freshfood";
            this.chLocationType4.Text = "Fresh Food";
            this.chLocationType4.UseVisualStyleBackColor = true;
            // 
            // chLocationType3
            // 
            this.chLocationType3.AutoSize = true;
            this.chLocationType3.Location = new System.Drawing.Point(26, 115);
            this.chLocationType3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chLocationType3.Name = "chLocationType3";
            this.chLocationType3.Size = new System.Drawing.Size(149, 30);
            this.chLocationType3.TabIndex = 5;
            this.chLocationType3.Tag = "warehouse";
            this.chLocationType3.Text = "Warehouse";
            this.chLocationType3.UseVisualStyleBackColor = true;
            // 
            // chLocationType2
            // 
            this.chLocationType2.AutoSize = true;
            this.chLocationType2.Location = new System.Drawing.Point(26, 72);
            this.chLocationType2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chLocationType2.Name = "chLocationType2";
            this.chLocationType2.Size = new System.Drawing.Size(87, 30);
            this.chLocationType2.TabIndex = 4;
            this.chLocationType2.Tag = "back";
            this.chLocationType2.Text = "Back";
            this.chLocationType2.UseVisualStyleBackColor = true;
            // 
            // chLocationType1
            // 
            this.chLocationType1.AutoSize = true;
            this.chLocationType1.Location = new System.Drawing.Point(26, 32);
            this.chLocationType1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chLocationType1.Name = "chLocationType1";
            this.chLocationType1.Size = new System.Drawing.Size(88, 30);
            this.chLocationType1.TabIndex = 3;
            this.chLocationType1.Tag = "front";
            this.chLocationType1.Text = "Front";
            this.chLocationType1.UseVisualStyleBackColor = true;
            // 
            // groupSearch
            // 
            this.groupSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSearch.AutoSize = true;
            this.groupSearch.Controls.Add(this.lbDept);
            this.groupSearch.Controls.Add(this.textBoxDept);
            this.groupSearch.Controls.Add(this.btnSearch);
            this.groupSearch.Controls.Add(this.textBoxLoTo);
            this.groupSearch.Controls.Add(this.lbLocationTo);
            this.groupSearch.Controls.Add(this.lbLocationFrom);
            this.groupSearch.Controls.Add(this.lbSectionName);
            this.groupSearch.Controls.Add(this.lbSectionCode);
            this.groupSearch.Controls.Add(this.textBoxLoForm);
            this.groupSearch.Controls.Add(this.textBoxSecName);
            this.groupSearch.Controls.Add(this.textBoxSecCode);
            this.groupSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupSearch.Location = new System.Drawing.Point(304, 5);
            this.groupSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupSearch.Name = "groupSearch";
            this.groupSearch.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupSearch.Size = new System.Drawing.Size(942, 201);
            this.groupSearch.TabIndex = 2;
            this.groupSearch.TabStop = false;
            this.groupSearch.Text = "Search";
            // 
            // lbDept
            // 
            this.lbDept.AutoSize = true;
            this.lbDept.Location = new System.Drawing.Point(28, 34);
            this.lbDept.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDept.Name = "lbDept";
            this.lbDept.Size = new System.Drawing.Size(202, 26);
            this.lbDept.TabIndex = 8;
            this.lbDept.Text = "Department Code : ";
            this.lbDept.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDept
            // 
            this.textBoxDept.Location = new System.Drawing.Point(238, 29);
            this.textBoxDept.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxDept.MaxLength = 3;
            this.textBoxDept.Name = "textBoxDept";
            this.textBoxDept.Size = new System.Drawing.Size(404, 32);
            this.textBoxDept.TabIndex = 7;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(723, 129);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(189, 62);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // textBoxLoTo
            // 
            this.textBoxLoTo.Location = new System.Drawing.Point(464, 157);
            this.textBoxLoTo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxLoTo.MaxLength = 5;
            this.textBoxLoTo.Name = "textBoxLoTo";
            this.textBoxLoTo.Size = new System.Drawing.Size(180, 32);
            this.textBoxLoTo.TabIndex = 3;
            // 
            // lbLocationTo
            // 
            this.lbLocationTo.AutoSize = true;
            this.lbLocationTo.Location = new System.Drawing.Point(428, 158);
            this.lbLocationTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLocationTo.Name = "lbLocationTo";
            this.lbLocationTo.Size = new System.Drawing.Size(30, 26);
            this.lbLocationTo.TabIndex = 6;
            this.lbLocationTo.Text = "to";
            this.lbLocationTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbLocationFrom
            // 
            this.lbLocationFrom.AutoSize = true;
            this.lbLocationFrom.Location = new System.Drawing.Point(28, 158);
            this.lbLocationFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLocationFrom.Name = "lbLocationFrom";
            this.lbLocationFrom.Size = new System.Drawing.Size(204, 26);
            this.lbLocationFrom.TabIndex = 5;
            this.lbLocationFrom.Text = "Location from        : ";
            this.lbLocationFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbSectionName
            // 
            this.lbSectionName.AutoSize = true;
            this.lbSectionName.Location = new System.Drawing.Point(28, 117);
            this.lbSectionName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSectionName.Name = "lbSectionName";
            this.lbSectionName.Size = new System.Drawing.Size(204, 26);
            this.lbSectionName.TabIndex = 4;
            this.lbSectionName.Text = "Section Name       : ";
            this.lbSectionName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbSectionCode
            // 
            this.lbSectionCode.AutoSize = true;
            this.lbSectionCode.Location = new System.Drawing.Point(28, 75);
            this.lbSectionCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSectionCode.Name = "lbSectionCode";
            this.lbSectionCode.Size = new System.Drawing.Size(203, 26);
            this.lbSectionCode.TabIndex = 3;
            this.lbSectionCode.Text = "Section Code        : ";
            this.lbSectionCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxLoForm
            // 
            this.textBoxLoForm.Location = new System.Drawing.Point(238, 157);
            this.textBoxLoForm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxLoForm.MaxLength = 5;
            this.textBoxLoForm.Name = "textBoxLoForm";
            this.textBoxLoForm.Size = new System.Drawing.Size(180, 32);
            this.textBoxLoForm.TabIndex = 2;
            // 
            // textBoxSecName
            // 
            this.textBoxSecName.Location = new System.Drawing.Point(238, 115);
            this.textBoxSecName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxSecName.MaxLength = 100;
            this.textBoxSecName.Name = "textBoxSecName";
            this.textBoxSecName.Size = new System.Drawing.Size(404, 32);
            this.textBoxSecName.TabIndex = 1;
            // 
            // textBoxSecCode
            // 
            this.textBoxSecCode.Location = new System.Drawing.Point(238, 72);
            this.textBoxSecCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxSecCode.MaxLength = 5;
            this.textBoxSecCode.Name = "textBoxSecCode";
            this.textBoxSecCode.Size = new System.Drawing.Size(404, 32);
            this.textBoxSecCode.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1292F));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(18, 229);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1292, 566);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // groupResult
            // 
            this.groupResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupResult.Controls.Add(this.btnReportBarcode);
            this.groupResult.Controls.Add(this.btnLoadBrand);
            this.groupResult.Controls.Add(this.lbFilterSearch);
            this.groupResult.Controls.Add(this.textBoxFilter);
            this.groupResult.Controls.Add(this.btnSave);
            this.groupResult.Controls.Add(this.btnReportLocation);
            this.groupResult.Controls.Add(this.btnImportSection);
            this.groupResult.Controls.Add(this.btnDeleteAll);
            this.groupResult.Controls.Add(this.dataGridViewResult);
            this.groupResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupResult.Location = new System.Drawing.Point(18, 238);
            this.groupResult.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupResult.Name = "groupResult";
            this.groupResult.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupResult.Size = new System.Drawing.Size(1250, 458);
            this.groupResult.TabIndex = 4;
            this.groupResult.TabStop = false;
            this.groupResult.Text = "Result";
            // 
            // btnReportBarcode
            // 
            this.btnReportBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReportBarcode.Location = new System.Drawing.Point(1434, 19);
            this.btnReportBarcode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnReportBarcode.Name = "btnReportBarcode";
            this.btnReportBarcode.Size = new System.Drawing.Size(112, 83);
            this.btnReportBarcode.TabIndex = 13;
            this.btnReportBarcode.Text = "Report Barcode";
            this.btnReportBarcode.UseVisualStyleBackColor = true;
            this.btnReportBarcode.Click += new System.EventHandler(this.btnReportBarcode_Click);
            // 
            // btnLoadBrand
            // 
            this.btnLoadBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadBrand.Location = new System.Drawing.Point(1184, 19);
            this.btnLoadBrand.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLoadBrand.Name = "btnLoadBrand";
            this.btnLoadBrand.Size = new System.Drawing.Size(122, 83);
            this.btnLoadBrand.TabIndex = 12;
            this.btnLoadBrand.Text = "Load Brand";
            this.btnLoadBrand.UseVisualStyleBackColor = true;
            this.btnLoadBrand.Click += new System.EventHandler(this.btnLoadBrand_Click);
            // 
            // lbFilterSearch
            // 
            this.lbFilterSearch.AutoSize = true;
            this.lbFilterSearch.Location = new System.Drawing.Point(21, 35);
            this.lbFilterSearch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFilterSearch.Name = "lbFilterSearch";
            this.lbFilterSearch.Size = new System.Drawing.Size(128, 26);
            this.lbFilterSearch.TabIndex = 9;
            this.lbFilterSearch.Text = "Filter Result";
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Location = new System.Drawing.Point(159, 31);
            this.textBoxFilter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(349, 32);
            this.textBoxFilter.TabIndex = 6;
            this.textBoxFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxFilter_KeyDown);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(1556, 19);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 83);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnReportLocation
            // 
            this.btnReportLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReportLocation.Location = new System.Drawing.Point(1314, 19);
            this.btnReportLocation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnReportLocation.Name = "btnReportLocation";
            this.btnReportLocation.Size = new System.Drawing.Size(112, 83);
            this.btnReportLocation.TabIndex = 9;
            this.btnReportLocation.Text = "Report Location";
            this.btnReportLocation.UseVisualStyleBackColor = true;
            this.btnReportLocation.Click += new System.EventHandler(this.btnReportLocation_Click);
            // 
            // btnImportSection
            // 
            this.btnImportSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportSection.Location = new System.Drawing.Point(1032, 19);
            this.btnImportSection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnImportSection.Name = "btnImportSection";
            this.btnImportSection.Size = new System.Drawing.Size(142, 83);
            this.btnImportSection.TabIndex = 8;
            this.btnImportSection.Text = "Import Section";
            this.btnImportSection.UseVisualStyleBackColor = true;
            this.btnImportSection.Click += new System.EventHandler(this.btnImportSection_Click);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAll.Location = new System.Drawing.Point(906, 19);
            this.btnDeleteAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(117, 83);
            this.btnDeleteAll.TabIndex = 7;
            this.btnDeleteAll.Text = "Delete All";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // dataGridViewResult
            // 
            this.dataGridViewResult.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dataGridViewResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DeptCode,
            this.SectionType,
            this.OriginSectionCode,
            this.SectionCode,
            this.SectionName,
            this.LocationFrom,
            this.LocationTo,
            this.BrandCode,
            this.Flag,
            this.OriginSectionType});
            this.dataGridViewResult.Location = new System.Drawing.Point(26, 106);
            this.dataGridViewResult.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.Size = new System.Drawing.Size(1197, 342);
            this.dataGridViewResult.TabIndex = 11;
            this.dataGridViewResult.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewResult_CellBeginEdit);
            this.dataGridViewResult.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewResult_CellEndEdit);
            this.dataGridViewResult.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewResult_UserAddedRow);
            this.dataGridViewResult.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridViewResult_UserDeletingRow);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 152.2843F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Department Code";
            this.dataGridViewTextBoxColumn1.MaxInputLength = 3;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "OriginalCode";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.FillWeight = 89.54314F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Section Code";
            this.dataGridViewTextBoxColumn3.MaxInputLength = 5;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.FillWeight = 89.54314F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Section Name";
            this.dataGridViewTextBoxColumn4.MaxInputLength = 100;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.FillWeight = 89.54314F;
            this.dataGridViewTextBoxColumn5.HeaderText = "Location From";
            this.dataGridViewTextBoxColumn5.MaxInputLength = 5;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.FillWeight = 89.54314F;
            this.dataGridViewTextBoxColumn6.HeaderText = "Location To";
            this.dataGridViewTextBoxColumn6.MaxInputLength = 5;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "BrandCode";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Flag";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Visible = false;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "OriginSectionType";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Visible = false;
            // 
            // DeptCode
            // 
            this.DeptCode.FillWeight = 152.2843F;
            this.DeptCode.HeaderText = "Department Code";
            this.DeptCode.MaxInputLength = 3;
            this.DeptCode.Name = "DeptCode";
            // 
            // SectionType
            // 
            this.SectionType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SectionType.FillWeight = 89.54314F;
            this.SectionType.HeaderText = "Section Type";
            this.SectionType.Name = "SectionType";
            this.SectionType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SectionType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // OriginSectionCode
            // 
            this.OriginSectionCode.HeaderText = "OriginalCode";
            this.OriginSectionCode.Name = "OriginSectionCode";
            this.OriginSectionCode.Visible = false;
            // 
            // SectionCode
            // 
            this.SectionCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SectionCode.FillWeight = 89.54314F;
            this.SectionCode.HeaderText = "Section Code";
            this.SectionCode.MaxInputLength = 5;
            this.SectionCode.Name = "SectionCode";
            // 
            // SectionName
            // 
            this.SectionName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SectionName.FillWeight = 89.54314F;
            this.SectionName.HeaderText = "Section Name";
            this.SectionName.MaxInputLength = 20;
            this.SectionName.Name = "SectionName";
            // 
            // LocationFrom
            // 
            this.LocationFrom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LocationFrom.FillWeight = 89.54314F;
            this.LocationFrom.HeaderText = "Location From";
            this.LocationFrom.MaxInputLength = 5;
            this.LocationFrom.Name = "LocationFrom";
            // 
            // LocationTo
            // 
            this.LocationTo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LocationTo.FillWeight = 89.54314F;
            this.LocationTo.HeaderText = "Location To";
            this.LocationTo.MaxInputLength = 5;
            this.LocationTo.Name = "LocationTo";
            // 
            // BrandCode
            // 
            this.BrandCode.HeaderText = "BrandCode";
            this.BrandCode.Name = "BrandCode";
            this.BrandCode.Visible = false;
            // 
            // Flag
            // 
            this.Flag.HeaderText = "Flag";
            this.Flag.Name = "Flag";
            this.Flag.Visible = false;
            // 
            // OriginSectionType
            // 
            this.OriginSectionType.HeaderText = "OriginSectionType";
            this.OriginSectionType.Name = "OriginSectionType";
            this.OriginSectionType.Visible = false;
            // 
            // LocationManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1779, 811);
            this.Controls.Add(this.groupResult);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "LocationManagementForm";
            this.ShowIcon = false;
            this.Text = "LocationManagement";
            this.Load += new System.EventHandler(this.LocationManagementForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupLocationType.ResumeLayout(false);
            this.groupLocationType.PerformLayout();
            this.groupSearch.ResumeLayout(false);
            this.groupSearch.PerformLayout();
            this.groupResult.ResumeLayout(false);
            this.groupResult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupLocationType;
        private System.Windows.Forms.GroupBox groupSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox textBoxLoTo;
        private System.Windows.Forms.Label lbLocationTo;
        private System.Windows.Forms.Label lbLocationFrom;
        private System.Windows.Forms.Label lbSectionName;
        private System.Windows.Forms.Label lbSectionCode;
        private System.Windows.Forms.TextBox textBoxLoForm;
        private System.Windows.Forms.TextBox textBoxSecName;
        private System.Windows.Forms.TextBox textBoxSecCode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupResult;
        private System.Windows.Forms.Label lbFilterSearch;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnReportLocation;
        private System.Windows.Forms.Button btnImportSection;
        private System.Windows.Forms.Button btnDeleteAll;
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.Button btnLoadBrand;
        private System.Windows.Forms.CheckBox chLocationType1;
        private System.Windows.Forms.CheckBox chLocationType2;
        private System.Windows.Forms.CheckBox chLocationType3;
        private System.Windows.Forms.CheckBox chLocationType4;
        private System.Windows.Forms.Label lbDept;
        private System.Windows.Forms.TextBox textBoxDept;
        private System.Windows.Forms.Button btnReportBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeptCode;
        private System.Windows.Forms.DataGridViewComboBoxColumn SectionType;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginSectionCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn BrandCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Flag;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginSectionType;
    }
}