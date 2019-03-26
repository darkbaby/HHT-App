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
            this.groupResult = new System.Windows.Forms.GroupBox();
            this.btnSaveLocation = new System.Windows.Forms.Button();
            this.btnImportLocation = new System.Windows.Forms.Button();
            this.ClearGrid = new System.Windows.Forms.Button();
            this.btnSaveTemplate = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.PlantCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountSheet = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.StorageLoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SectionCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SectionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocationFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocationTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Flag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BrandCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalPlant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalCountSheet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalStorageLoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalSectionCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalSectionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnLoadBrand = new System.Windows.Forms.Button();
            this.lbFilterSearch = new System.Windows.Forms.Label();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnImportSection = new System.Windows.Forms.Button();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.groupSearch = new System.Windows.Forms.GroupBox();
            this.comboBoxPlant = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lbPlant = new System.Windows.Forms.Label();
            this.comboBoxCountSheet = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbCountSheet = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxStorageLocal = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.textBoxLoTo = new System.Windows.Forms.TextBox();
            this.lbLocationTo = new System.Windows.Forms.Label();
            this.lbLocationFrom = new System.Windows.Forms.Label();
            this.lbSectionName = new System.Windows.Forms.Label();
            this.textBoxLoForm = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnReportLocation = new System.Windows.Forms.Button();
            this.btnReportBarcode = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupResult.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            this.groupSearch.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupResult
            // 
            this.groupResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupResult.Controls.Add(this.btnSaveLocation);
            this.groupResult.Controls.Add(this.btnImportLocation);
            this.groupResult.Controls.Add(this.ClearGrid);
            this.groupResult.Controls.Add(this.btnSaveTemplate);
            this.groupResult.Controls.Add(this.panel2);
            this.groupResult.Controls.Add(this.BtnLoadBrand);
            this.groupResult.Controls.Add(this.lbFilterSearch);
            this.groupResult.Controls.Add(this.textBoxFilter);
            this.groupResult.Controls.Add(this.btnSave);
            this.groupResult.Controls.Add(this.btnImportSection);
            this.groupResult.Controls.Add(this.btnDeleteAll);
            this.groupResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupResult.Location = new System.Drawing.Point(5, 3);
            this.groupResult.Name = "groupResult";
            this.groupResult.Size = new System.Drawing.Size(1078, 442);
            this.groupResult.TabIndex = 4;
            this.groupResult.TabStop = false;
            this.groupResult.Text = "Result";
            // 
            // btnSaveLocation
            // 
            this.btnSaveLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveLocation.Location = new System.Drawing.Point(669, 20);
            this.btnSaveLocation.Name = "btnSaveLocation";
            this.btnSaveLocation.Size = new System.Drawing.Size(94, 59);
            this.btnSaveLocation.TabIndex = 33;
            this.btnSaveLocation.Text = "Export Sub Location";
            this.btnSaveLocation.UseVisualStyleBackColor = true;
            this.btnSaveLocation.Click += new System.EventHandler(this.btnExportLocation_Click);
            // 
            // btnImportLocation
            // 
            this.btnImportLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportLocation.Location = new System.Drawing.Point(569, 20);
            this.btnImportLocation.Name = "btnImportLocation";
            this.btnImportLocation.Size = new System.Drawing.Size(94, 59);
            this.btnImportLocation.TabIndex = 32;
            this.btnImportLocation.Text = "Import Sub Location";
            this.btnImportLocation.UseVisualStyleBackColor = true;
            this.btnImportLocation.Click += new System.EventHandler(this.btnImportLocation_Click);
            // 
            // ClearGrid
            // 
            this.ClearGrid.Location = new System.Drawing.Point(18, 390);
            this.ClearGrid.Name = "ClearGrid";
            this.ClearGrid.Size = new System.Drawing.Size(126, 41);
            this.ClearGrid.TabIndex = 31;
            this.ClearGrid.Text = "Clear Gridview";
            this.ClearGrid.UseVisualStyleBackColor = true;
            this.ClearGrid.Click += new System.EventHandler(this.ClearData_Click);
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveTemplate.Location = new System.Drawing.Point(869, 20);
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(94, 59);
            this.btnSaveTemplate.TabIndex = 16;
            this.btnSaveTemplate.Text = "Export Template";
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Click += new System.EventHandler(this.btnExportTemplate_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridViewResult);
            this.panel2.Location = new System.Drawing.Point(5, 84);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1054, 302);
            this.panel2.TabIndex = 15;
            // 
            // dataGridViewResult
            // 
            this.dataGridViewResult.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dataGridViewResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PlantCode,
            this.CountSheet,
            this.StorageLoc,
            this.SectionCode,
            this.SectionName,
            this.LocationFrom,
            this.LocationTo,
            this.Flag,
            this.BrandCode,
            this.OriginalPlant,
            this.OriginalCountSheet,
            this.OriginalStorageLoc,
            this.OriginalSectionCode,
            this.OriginalSectionName});
            this.dataGridViewResult.Location = new System.Drawing.Point(14, 3);
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.RowHeadersWidth = 20;
            this.dataGridViewResult.Size = new System.Drawing.Size(1039, 296);
            this.dataGridViewResult.TabIndex = 11;
            this.dataGridViewResult.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewResult_CellBeginEdit);
            this.dataGridViewResult.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewResult_CellEndEdit);
            this.dataGridViewResult.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewResult_CellValidating);
            this.dataGridViewResult.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewResult_CellValueChanged);
            this.dataGridViewResult.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridViewResult_CurrentCellDirtyStateChanged);
            this.dataGridViewResult.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewResult_DataError);
            this.dataGridViewResult.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewResult_EditingControlShowing);
            this.dataGridViewResult.NewRowNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewResult_NewRowNeeded);
            this.dataGridViewResult.SelectionChanged += new System.EventHandler(this.dataGridViewResult_SelectionChanged);
            this.dataGridViewResult.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewResult_UserAddedRow);
            this.dataGridViewResult.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridViewResult_UserDeletingRow);
            // 
            // PlantCode
            // 
            this.PlantCode.DataPropertyName = "Plant";
            this.PlantCode.HeaderText = "Plant";
            this.PlantCode.Name = "PlantCode";
            this.PlantCode.ReadOnly = true;
            this.PlantCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // CountSheet
            // 
            this.CountSheet.HeaderText = "Count Sheet";
            this.CountSheet.MaxDropDownItems = 20;
            this.CountSheet.Name = "CountSheet";
            this.CountSheet.Width = 115;
            // 
            // StorageLoc
            // 
            this.StorageLoc.HeaderText = "Storage Location";
            this.StorageLoc.MaxInputLength = 4;
            this.StorageLoc.Name = "StorageLoc";
            this.StorageLoc.Width = 150;
            // 
            // SectionCode
            // 
            this.SectionCode.DataPropertyName = "SectionCode";
            this.SectionCode.FillWeight = 89.54314F;
            this.SectionCode.HeaderText = "Section Code";
            this.SectionCode.MaxInputLength = 5;
            this.SectionCode.Name = "SectionCode";
            this.SectionCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SectionCode.Width = 140;
            // 
            // SectionName
            // 
            this.SectionName.DataPropertyName = "SectionName";
            this.SectionName.FillWeight = 89.54314F;
            this.SectionName.HeaderText = "Section Name";
            this.SectionName.MaxInputLength = 100;
            this.SectionName.Name = "SectionName";
            this.SectionName.Width = 200;
            // 
            // LocationFrom
            // 
            this.LocationFrom.DataPropertyName = "LocationFrom";
            this.LocationFrom.FillWeight = 89.54314F;
            this.LocationFrom.HeaderText = "Sub Location From";
            this.LocationFrom.MaxInputLength = 5;
            this.LocationFrom.Name = "LocationFrom";
            this.LocationFrom.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LocationFrom.Width = 170;
            // 
            // LocationTo
            // 
            this.LocationTo.DataPropertyName = "LocationTo";
            this.LocationTo.FillWeight = 89.54314F;
            this.LocationTo.HeaderText = "Sub Location To";
            this.LocationTo.MaxInputLength = 5;
            this.LocationTo.Name = "LocationTo";
            this.LocationTo.Width = 150;
            // 
            // Flag
            // 
            this.Flag.DataPropertyName = "Flag";
            this.Flag.HeaderText = "Flag";
            this.Flag.Name = "Flag";
            this.Flag.Visible = false;
            this.Flag.Width = 50;
            // 
            // BrandCode
            // 
            this.BrandCode.DataPropertyName = "BrandCode";
            this.BrandCode.HeaderText = "BrandCode";
            this.BrandCode.Name = "BrandCode";
            this.BrandCode.Visible = false;
            // 
            // OriginalPlant
            // 
            this.OriginalPlant.HeaderText = "Origin Plant";
            this.OriginalPlant.Name = "OriginalPlant";
            this.OriginalPlant.Visible = false;
            // 
            // OriginalCountSheet
            // 
            this.OriginalCountSheet.HeaderText = "Original CountSheet";
            this.OriginalCountSheet.Name = "OriginalCountSheet";
            this.OriginalCountSheet.Visible = false;
            // 
            // OriginalStorageLoc
            // 
            this.OriginalStorageLoc.DataPropertyName = "OriginalStorageLoc";
            this.OriginalStorageLoc.HeaderText = "Original Storage Loc";
            this.OriginalStorageLoc.MaxInputLength = 5;
            this.OriginalStorageLoc.Name = "OriginalStorageLoc";
            this.OriginalStorageLoc.Visible = false;
            // 
            // OriginalSectionCode
            // 
            this.OriginalSectionCode.DataPropertyName = "OriginSectionCode";
            this.OriginalSectionCode.HeaderText = "OriginalSectionCode";
            this.OriginalSectionCode.MaxInputLength = 5;
            this.OriginalSectionCode.Name = "OriginalSectionCode";
            this.OriginalSectionCode.Visible = false;
            this.OriginalSectionCode.Width = 50;
            // 
            // OriginalSectionName
            // 
            this.OriginalSectionName.HeaderText = "OriginSectionName";
            this.OriginalSectionName.Name = "OriginalSectionName";
            this.OriginalSectionName.Visible = false;
            // 
            // BtnLoadBrand
            // 
            this.BtnLoadBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLoadBrand.Location = new System.Drawing.Point(469, 20);
            this.BtnLoadBrand.Name = "BtnLoadBrand";
            this.BtnLoadBrand.Size = new System.Drawing.Size(94, 59);
            this.BtnLoadBrand.TabIndex = 14;
            this.BtnLoadBrand.Text = "Load Brand";
            this.BtnLoadBrand.UseVisualStyleBackColor = true;
            this.BtnLoadBrand.Click += new System.EventHandler(this.BtnLoadBrand_Click);
            // 
            // lbFilterSearch
            // 
            this.lbFilterSearch.AutoSize = true;
            this.lbFilterSearch.Location = new System.Drawing.Point(19, 43);
            this.lbFilterSearch.Name = "lbFilterSearch";
            this.lbFilterSearch.Size = new System.Drawing.Size(86, 18);
            this.lbFilterSearch.TabIndex = 9;
            this.lbFilterSearch.Text = "Filter Result";
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Location = new System.Drawing.Point(111, 40);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(234, 24);
            this.textBoxFilter.TabIndex = 6;
            this.textBoxFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxFilter_KeyDown);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(966, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 59);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnImportSection
            // 
            this.btnImportSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportSection.Location = new System.Drawing.Point(769, 20);
            this.btnImportSection.Name = "btnImportSection";
            this.btnImportSection.Size = new System.Drawing.Size(94, 59);
            this.btnImportSection.TabIndex = 8;
            this.btnImportSection.Text = "Import Template";
            this.btnImportSection.UseVisualStyleBackColor = true;
            this.btnImportSection.Click += new System.EventHandler(this.btnImportSection_Click);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAll.Location = new System.Drawing.Point(369, 20);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(94, 59);
            this.btnDeleteAll.TabIndex = 7;
            this.btnDeleteAll.Text = "Delete All";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // groupSearch
            // 
            this.groupSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSearch.Controls.Add(this.comboBoxPlant);
            this.groupSearch.Controls.Add(this.label9);
            this.groupSearch.Controls.Add(this.lbPlant);
            this.groupSearch.Controls.Add(this.comboBoxCountSheet);
            this.groupSearch.Controls.Add(this.label7);
            this.groupSearch.Controls.Add(this.lbCountSheet);
            this.groupSearch.Controls.Add(this.label5);
            this.groupSearch.Controls.Add(this.label4);
            this.groupSearch.Controls.Add(this.label1);
            this.groupSearch.Controls.Add(this.comboBoxStorageLocal);
            this.groupSearch.Controls.Add(this.btnSearch);
            this.groupSearch.Controls.Add(this.textBoxLoTo);
            this.groupSearch.Controls.Add(this.lbLocationTo);
            this.groupSearch.Controls.Add(this.lbLocationFrom);
            this.groupSearch.Controls.Add(this.lbSectionName);
            this.groupSearch.Controls.Add(this.textBoxLoForm);
            this.groupSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupSearch.Location = new System.Drawing.Point(3, 3);
            this.groupSearch.Name = "groupSearch";
            this.groupSearch.Size = new System.Drawing.Size(832, 88);
            this.groupSearch.TabIndex = 2;
            this.groupSearch.TabStop = false;
            this.groupSearch.Text = "Search";
            // 
            // comboBoxPlant
            // 
            this.comboBoxPlant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlant.FormattingEnabled = true;
            this.comboBoxPlant.Location = new System.Drawing.Point(143, 22);
            this.comboBoxPlant.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.comboBoxPlant.Name = "comboBoxPlant";
            this.comboBoxPlant.Size = new System.Drawing.Size(180, 26);
            this.comboBoxPlant.TabIndex = 30;
            this.comboBoxPlant.SelectionChangeCommitted += new System.EventHandler(this.comboBoxPlant_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(129, 24);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 18);
            this.label9.TabIndex = 29;
            this.label9.Text = ":";
            // 
            // lbPlant
            // 
            this.lbPlant.AutoSize = true;
            this.lbPlant.Location = new System.Drawing.Point(7, 28);
            this.lbPlant.Name = "lbPlant";
            this.lbPlant.Size = new System.Drawing.Size(41, 18);
            this.lbPlant.TabIndex = 28;
            this.lbPlant.Text = "Plant";
            this.lbPlant.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxCountSheet
            // 
            this.comboBoxCountSheet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCountSheet.FormattingEnabled = true;
            this.comboBoxCountSheet.Location = new System.Drawing.Point(504, 23);
            this.comboBoxCountSheet.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.comboBoxCountSheet.Name = "comboBoxCountSheet";
            this.comboBoxCountSheet.Size = new System.Drawing.Size(180, 26);
            this.comboBoxCountSheet.TabIndex = 27;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(491, 26);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 18);
            this.label7.TabIndex = 26;
            this.label7.Text = ":";
            // 
            // lbCountSheet
            // 
            this.lbCountSheet.AutoSize = true;
            this.lbCountSheet.Location = new System.Drawing.Point(353, 26);
            this.lbCountSheet.Name = "lbCountSheet";
            this.lbCountSheet.Size = new System.Drawing.Size(86, 18);
            this.lbCountSheet.TabIndex = 25;
            this.lbCountSheet.Text = "CountSheet";
            this.lbCountSheet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(603, 60);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 18);
            this.label5.TabIndex = 18;
            this.label5.Text = ":";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(489, 60);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 18);
            this.label4.TabIndex = 17;
            this.label4.Text = ":";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(128, 59);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 18);
            this.label1.TabIndex = 14;
            this.label1.Text = ":";
            // 
            // comboBoxStorageLocal
            // 
            this.comboBoxStorageLocal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStorageLocal.FormattingEnabled = true;
            this.comboBoxStorageLocal.Location = new System.Drawing.Point(143, 55);
            this.comboBoxStorageLocal.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.comboBoxStorageLocal.Name = "comboBoxStorageLocal";
            this.comboBoxStorageLocal.Size = new System.Drawing.Size(180, 26);
            this.comboBoxStorageLocal.TabIndex = 9;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(720, 22);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(94, 59);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // textBoxLoTo
            // 
            this.textBoxLoTo.Location = new System.Drawing.Point(619, 57);
            this.textBoxLoTo.MaxLength = 5;
            this.textBoxLoTo.Name = "textBoxLoTo";
            this.textBoxLoTo.Size = new System.Drawing.Size(65, 24);
            this.textBoxLoTo.TabIndex = 3;
            // 
            // lbLocationTo
            // 
            this.lbLocationTo.AutoSize = true;
            this.lbLocationTo.Location = new System.Drawing.Point(580, 60);
            this.lbLocationTo.Name = "lbLocationTo";
            this.lbLocationTo.Size = new System.Drawing.Size(26, 18);
            this.lbLocationTo.TabIndex = 6;
            this.lbLocationTo.Text = "To";
            this.lbLocationTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbLocationFrom
            // 
            this.lbLocationFrom.AutoSize = true;
            this.lbLocationFrom.Location = new System.Drawing.Point(353, 60);
            this.lbLocationFrom.Name = "lbLocationFrom";
            this.lbLocationFrom.Size = new System.Drawing.Size(139, 18);
            this.lbLocationFrom.TabIndex = 5;
            this.lbLocationFrom.Text = "Sub Location From ";
            this.lbLocationFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSectionName
            // 
            this.lbSectionName.AutoSize = true;
            this.lbSectionName.Location = new System.Drawing.Point(7, 60);
            this.lbSectionName.Name = "lbSectionName";
            this.lbSectionName.Size = new System.Drawing.Size(121, 18);
            this.lbSectionName.TabIndex = 4;
            this.lbSectionName.Text = "Storage Location";
            this.lbSectionName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxLoForm
            // 
            this.textBoxLoForm.Location = new System.Drawing.Point(504, 57);
            this.textBoxLoForm.MaxLength = 5;
            this.textBoxLoForm.Name = "textBoxLoForm";
            this.textBoxLoForm.Size = new System.Drawing.Size(65, 24);
            this.textBoxLoForm.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupSearch);
            this.panel1.Location = new System.Drawing.Point(10, 6);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(846, 94);
            this.panel1.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupResult);
            this.panel3.Location = new System.Drawing.Point(8, 102);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1094, 448);
            this.panel3.TabIndex = 16;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox1);
            this.panel4.Location = new System.Drawing.Point(858, 6);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(242, 97);
            this.panel4.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnReportLocation);
            this.groupBox1.Controls.Add(this.btnReportBarcode);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(228, 91);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Report";
            // 
            // btnReportLocation
            // 
            this.btnReportLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReportLocation.Location = new System.Drawing.Point(21, 23);
            this.btnReportLocation.Name = "btnReportLocation";
            this.btnReportLocation.Size = new System.Drawing.Size(94, 59);
            this.btnReportLocation.TabIndex = 14;
            this.btnReportLocation.Text = "Report Location";
            this.btnReportLocation.UseVisualStyleBackColor = true;
            this.btnReportLocation.Click += new System.EventHandler(this.btnReportLocation_Click);
            // 
            // btnReportBarcode
            // 
            this.btnReportBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReportBarcode.Location = new System.Drawing.Point(118, 23);
            this.btnReportBarcode.Name = "btnReportBarcode";
            this.btnReportBarcode.Size = new System.Drawing.Size(94, 59);
            this.btnReportBarcode.TabIndex = 15;
            this.btnReportBarcode.Text = "Report Barcode";
            this.btnReportBarcode.UseVisualStyleBackColor = true;
            this.btnReportBarcode.Click += new System.EventHandler(this.btnReportBarcode_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Plant";
            this.dataGridViewTextBoxColumn1.FillWeight = 152.2843F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Department Code";
            this.dataGridViewTextBoxColumn1.MaxInputLength = 3;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "OriginalStorageLoc";
            this.dataGridViewTextBoxColumn2.FillWeight = 89.54314F;
            this.dataGridViewTextBoxColumn2.HeaderText = "OriginalCode";
            this.dataGridViewTextBoxColumn2.MaxInputLength = 5;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "OriginSectionCode";
            this.dataGridViewTextBoxColumn3.FillWeight = 89.54314F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Section Code";
            this.dataGridViewTextBoxColumn3.MaxInputLength = 5;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "SectionCode";
            this.dataGridViewTextBoxColumn4.FillWeight = 89.54314F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Section Name";
            this.dataGridViewTextBoxColumn4.MaxInputLength = 100;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "SectionName";
            this.dataGridViewTextBoxColumn5.FillWeight = 89.54314F;
            this.dataGridViewTextBoxColumn5.HeaderText = "Location From";
            this.dataGridViewTextBoxColumn5.MaxInputLength = 5;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "LocationFrom";
            this.dataGridViewTextBoxColumn6.FillWeight = 89.54314F;
            this.dataGridViewTextBoxColumn6.HeaderText = "Location To";
            this.dataGridViewTextBoxColumn6.MaxInputLength = 5;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn7.DataPropertyName = "LocationTo";
            this.dataGridViewTextBoxColumn7.FillWeight = 89.54314F;
            this.dataGridViewTextBoxColumn7.HeaderText = "BrandCode";
            this.dataGridViewTextBoxColumn7.MaxInputLength = 5;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "Flag";
            this.dataGridViewTextBoxColumn8.FillWeight = 89.54314F;
            this.dataGridViewTextBoxColumn8.HeaderText = "Flag";
            this.dataGridViewTextBoxColumn8.MaxInputLength = 5;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Visible = false;
            this.dataGridViewTextBoxColumn8.Width = 50;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "BrandCode";
            this.dataGridViewTextBoxColumn9.HeaderText = "OriginSectionType";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Visible = false;
            this.dataGridViewTextBoxColumn9.Width = 50;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "BrandCode";
            this.dataGridViewTextBoxColumn10.HeaderText = "OriginSectionType";
            this.dataGridViewTextBoxColumn10.MaxInputLength = 5;
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Visible = false;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "OriginSectionCode";
            this.dataGridViewTextBoxColumn11.HeaderText = "OriginalSectionCode";
            this.dataGridViewTextBoxColumn11.MaxInputLength = 5;
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Visible = false;
            this.dataGridViewTextBoxColumn11.Width = 50;
            // 
            // LocationManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 632);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "LocationManagementForm";
            this.ShowIcon = false;
            this.Text = "LocationManagement";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LocationManagementForm_FormClosing);
            this.Load += new System.EventHandler(this.LocationManagementForm_Load);
            this.groupResult.ResumeLayout(false);
            this.groupResult.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            this.groupSearch.ResumeLayout(false);
            this.groupSearch.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupResult;
        private System.Windows.Forms.Label lbFilterSearch;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnImportSection;
        private System.Windows.Forms.Button btnDeleteAll;
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.Button BtnLoadBrand;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.GroupBox groupSearch;
        private System.Windows.Forms.ComboBox comboBoxCountSheet;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbCountSheet;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxStorageLocal;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox textBoxLoTo;
        private System.Windows.Forms.Label lbLocationTo;
        private System.Windows.Forms.Label lbLocationFrom;
        private System.Windows.Forms.Label lbSectionName;
        private System.Windows.Forms.TextBox textBoxLoForm;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox comboBoxPlant;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbPlant;
        private System.Windows.Forms.Button btnSaveTemplate;
        private System.Windows.Forms.Button ClearGrid;
        private System.Windows.Forms.Button btnSaveLocation;
        private System.Windows.Forms.Button btnImportLocation;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnReportLocation;
        private System.Windows.Forms.Button btnReportBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlantCode;
        private System.Windows.Forms.DataGridViewComboBoxColumn CountSheet;
        private System.Windows.Forms.DataGridViewTextBoxColumn StorageLoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Flag;
        private System.Windows.Forms.DataGridViewTextBoxColumn BrandCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalPlant;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalCountSheet;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalStorageLoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalSectionCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalSectionName;
    }
}