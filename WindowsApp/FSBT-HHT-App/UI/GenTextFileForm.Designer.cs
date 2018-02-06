namespace FSBT.HHT.App.UI
{
    partial class TextFileForm
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SectionBox = new System.Windows.Forms.GroupBox();
            this.rdFront = new System.Windows.Forms.RadioButton();
            this.rdWarehouse = new System.Windows.Forms.RadioButton();
            this.rdFreshFood = new System.Windows.Forms.RadioButton();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.CompareBox = new System.Windows.Forms.GroupBox();
            this.rdONPC = new System.Windows.Forms.RadioButton();
            this.rdONAS400 = new System.Windows.Forms.RadioButton();
            this.CatagoryBox = new System.Windows.Forms.GroupBox();
            this.rdSup = new System.Windows.Forms.RadioButton();
            this.rdDept = new System.Windows.Forms.RadioButton();
            this.DepartmentBox = new System.Windows.Forms.GroupBox();
            this.txtBoxDepartment = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExportTxt = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnAddTextFile = new System.Windows.Forms.Button();
            this.btnMerge = new System.Windows.Forms.Button();
            this.vPath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SectionBox.SuspendLayout();
            this.CompareBox.SuspendLayout();
            this.CatagoryBox.SuspendLayout();
            this.DepartmentBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(20, 18);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(1820, 523);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Check Sample Data";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.24782F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.34325F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.40893F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.SectionBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtFileName, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.CompareBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.CatagoryBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.DepartmentBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSearch, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnExportTxt, 2, 5);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(22, 29);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1468, 485);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(4, 5);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.tableLayoutPanel1.SetRowSpan(this.dataGridView1, 6);
            this.dataGridView1.Size = new System.Drawing.Size(920, 475);
            this.dataGridView1.TabIndex = 0;
            // 
            // SectionBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.SectionBox, 2);
            this.SectionBox.Controls.Add(this.rdFront);
            this.SectionBox.Controls.Add(this.rdWarehouse);
            this.SectionBox.Controls.Add(this.rdFreshFood);
            this.SectionBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SectionBox.Location = new System.Drawing.Point(932, 163);
            this.SectionBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SectionBox.Name = "SectionBox";
            this.SectionBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SectionBox.Size = new System.Drawing.Size(531, 127);
            this.SectionBox.TabIndex = 30;
            this.SectionBox.TabStop = false;
            this.SectionBox.Text = "Section Type";
            // 
            // rdFront
            // 
            this.rdFront.AutoSize = true;
            this.rdFront.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdFront.Location = new System.Drawing.Point(9, 29);
            this.rdFront.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdFront.Name = "rdFront";
            this.rdFront.Size = new System.Drawing.Size(94, 33);
            this.rdFront.TabIndex = 22;
            this.rdFront.TabStop = true;
            this.rdFront.Text = "Front";
            this.rdFront.UseVisualStyleBackColor = true;
            this.rdFront.Click += new System.EventHandler(this.rdFront_Click);
            // 
            // rdWarehouse
            // 
            this.rdWarehouse.AutoSize = true;
            this.rdWarehouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdWarehouse.Location = new System.Drawing.Point(232, 29);
            this.rdWarehouse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdWarehouse.Name = "rdWarehouse";
            this.rdWarehouse.Size = new System.Drawing.Size(161, 33);
            this.rdWarehouse.TabIndex = 23;
            this.rdWarehouse.TabStop = true;
            this.rdWarehouse.Text = "Warehouse";
            this.rdWarehouse.UseVisualStyleBackColor = true;
            this.rdWarehouse.Click += new System.EventHandler(this.rdWarehouse_Click);
            // 
            // rdFreshFood
            // 
            this.rdFreshFood.AutoSize = true;
            this.rdFreshFood.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdFreshFood.Location = new System.Drawing.Point(9, 72);
            this.rdFreshFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdFreshFood.Name = "rdFreshFood";
            this.rdFreshFood.Size = new System.Drawing.Size(157, 33);
            this.rdFreshFood.TabIndex = 24;
            this.rdFreshFood.TabStop = true;
            this.rdFreshFood.Text = "FreshFood";
            this.rdFreshFood.UseVisualStyleBackColor = true;
            this.rdFreshFood.Click += new System.EventHandler(this.rdFreshFood_Click);
            // 
            // txtFileName
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtFileName, 2);
            this.txtFileName.Enabled = false;
            this.txtFileName.Location = new System.Drawing.Point(932, 385);
            this.txtFileName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(529, 33);
            this.txtFileName.TabIndex = 32;
            // 
            // CompareBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.CompareBox, 2);
            this.CompareBox.Controls.Add(this.rdONPC);
            this.CompareBox.Controls.Add(this.rdONAS400);
            this.CompareBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CompareBox.Location = new System.Drawing.Point(932, 300);
            this.CompareBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CompareBox.Name = "CompareBox";
            this.CompareBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CompareBox.Size = new System.Drawing.Size(531, 74);
            this.CompareBox.TabIndex = 31;
            this.CompareBox.TabStop = false;
            this.CompareBox.Text = "Compare";
            // 
            // rdONPC
            // 
            this.rdONPC.AutoSize = true;
            this.rdONPC.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdONPC.Location = new System.Drawing.Point(232, 29);
            this.rdONPC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdONPC.Name = "rdONPC";
            this.rdONPC.Size = new System.Drawing.Size(194, 33);
            this.rdONPC.TabIndex = 27;
            this.rdONPC.TabStop = true;
            this.rdONPC.Text = "เทียบยอดบน PC";
            this.rdONPC.UseVisualStyleBackColor = true;
            this.rdONPC.Click += new System.EventHandler(this.rdONPC_Click);
            // 
            // rdONAS400
            // 
            this.rdONAS400.AutoSize = true;
            this.rdONAS400.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdONAS400.Location = new System.Drawing.Point(9, 29);
            this.rdONAS400.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdONAS400.Name = "rdONAS400";
            this.rdONAS400.Size = new System.Drawing.Size(231, 33);
            this.rdONAS400.TabIndex = 28;
            this.rdONAS400.TabStop = true;
            this.rdONAS400.Text = "เทียบยอดบน AS400";
            this.rdONAS400.UseVisualStyleBackColor = true;
            this.rdONAS400.Click += new System.EventHandler(this.rdONAS400_Click);
            // 
            // CatagoryBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.CatagoryBox, 2);
            this.CatagoryBox.Controls.Add(this.rdSup);
            this.CatagoryBox.Controls.Add(this.rdDept);
            this.CatagoryBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CatagoryBox.Location = new System.Drawing.Point(932, 85);
            this.CatagoryBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CatagoryBox.Name = "CatagoryBox";
            this.CatagoryBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CatagoryBox.Size = new System.Drawing.Size(531, 68);
            this.CatagoryBox.TabIndex = 29;
            this.CatagoryBox.TabStop = false;
            this.CatagoryBox.Text = "Catagory";
            // 
            // rdSup
            // 
            this.rdSup.AutoSize = true;
            this.rdSup.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdSup.Location = new System.Drawing.Point(9, 29);
            this.rdSup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdSup.Name = "rdSup";
            this.rdSup.Size = new System.Drawing.Size(182, 33);
            this.rdSup.TabIndex = 19;
            this.rdSup.TabStop = true;
            this.rdSup.Text = "Super market";
            this.rdSup.UseVisualStyleBackColor = true;
            this.rdSup.CheckedChanged += new System.EventHandler(this.rdSup_CheckedChanged);
            this.rdSup.Click += new System.EventHandler(this.rdSup_Click);
            // 
            // rdDept
            // 
            this.rdDept.AutoSize = true;
            this.rdDept.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdDept.Location = new System.Drawing.Point(232, 26);
            this.rdDept.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdDept.Name = "rdDept";
            this.rdDept.Size = new System.Drawing.Size(227, 33);
            this.rdDept.TabIndex = 20;
            this.rdDept.TabStop = true;
            this.rdDept.Text = "Department Store";
            this.rdDept.UseVisualStyleBackColor = true;
            this.rdDept.CheckedChanged += new System.EventHandler(this.rdDept_CheckedChanged);
            this.rdDept.Click += new System.EventHandler(this.rdDept_Click);
            // 
            // DepartmentBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.DepartmentBox, 2);
            this.DepartmentBox.Controls.Add(this.txtBoxDepartment);
            this.DepartmentBox.Controls.Add(this.label1);
            this.DepartmentBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DepartmentBox.Location = new System.Drawing.Point(932, 5);
            this.DepartmentBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DepartmentBox.Name = "DepartmentBox";
            this.DepartmentBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DepartmentBox.Size = new System.Drawing.Size(531, 70);
            this.DepartmentBox.TabIndex = 33;
            this.DepartmentBox.TabStop = false;
            this.DepartmentBox.Text = "Department";
            // 
            // txtBoxDepartment
            // 
            this.txtBoxDepartment.Location = new System.Drawing.Point(202, 25);
            this.txtBoxDepartment.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBoxDepartment.MaxLength = 3;
            this.txtBoxDepartment.Name = "txtBoxDepartment";
            this.txtBoxDepartment.Size = new System.Drawing.Size(190, 33);
            this.txtBoxDepartment.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Department code";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(932, 433);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(152, 38);
            this.btnSearch.TabIndex = 34;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExportTxt
            // 
            this.btnExportTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportTxt.Location = new System.Drawing.Point(1215, 433);
            this.btnExportTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExportTxt.Name = "btnExportTxt";
            this.btnExportTxt.Size = new System.Drawing.Size(152, 38);
            this.btnExportTxt.TabIndex = 11;
            this.btnExportTxt.Text = "Export TXT";
            this.btnExportTxt.UseVisualStyleBackColor = true;
            this.btnExportTxt.Click += new System.EventHandler(this.btnExportTxt_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnClear);
            this.groupBox3.Controls.Add(this.btnAddTextFile);
            this.groupBox3.Controls.Add(this.btnMerge);
            this.groupBox3.Controls.Add(this.vPath);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.dataGridView2);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(18, 569);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(1820, 237);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Merge Text File";
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(952, 98);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(158, 45);
            this.btnClear.TabIndex = 16;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAddTextFile
            // 
            this.btnAddTextFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddTextFile.Location = new System.Drawing.Point(952, 34);
            this.btnAddTextFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddTextFile.Name = "btnAddTextFile";
            this.btnAddTextFile.Size = new System.Drawing.Size(158, 55);
            this.btnAddTextFile.TabIndex = 13;
            this.btnAddTextFile.Text = "Add Text File";
            this.btnAddTextFile.UseVisualStyleBackColor = true;
            this.btnAddTextFile.Click += new System.EventHandler(this.btnAddTextFile_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMerge.Location = new System.Drawing.Point(952, 182);
            this.btnMerge.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(158, 45);
            this.btnMerge.TabIndex = 12;
            this.btnMerge.Text = "Merge";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // vPath
            // 
            this.vPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vPath.Location = new System.Drawing.Point(164, 192);
            this.vPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.vPath.Name = "vPath";
            this.vPath.Size = new System.Drawing.Size(780, 33);
            this.vPath.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(39, 197);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(163, 29);
            this.label7.TabIndex = 2;
            this.label7.Text = "Path                :";
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.ColumnHeadersVisible = false;
            this.dataGridView2.Location = new System.Drawing.Point(164, 34);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.Size = new System.Drawing.Size(780, 135);
            this.dataGridView2.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(44, 34);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(156, 29);
            this.label6.TabIndex = 0;
            this.label6.Text = "File                :";
            // 
            // TextFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1875, 972);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TextFileForm";
            this.ShowIcon = false;
            this.Text = "GenTextFileForm";
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.SectionBox.ResumeLayout(false);
            this.SectionBox.PerformLayout();
            this.CompareBox.ResumeLayout(false);
            this.CompareBox.PerformLayout();
            this.CatagoryBox.ResumeLayout(false);
            this.CatagoryBox.PerformLayout();
            this.DepartmentBox.ResumeLayout(false);
            this.DepartmentBox.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnExportTxt;
        private System.Windows.Forms.TextBox vPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnAddTextFile;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton rdSup;
        private System.Windows.Forms.RadioButton rdDept;
        private System.Windows.Forms.RadioButton rdFront;
        private System.Windows.Forms.RadioButton rdWarehouse;
        private System.Windows.Forms.RadioButton rdFreshFood;
        private System.Windows.Forms.GroupBox CatagoryBox;
        private System.Windows.Forms.GroupBox SectionBox;
        private System.Windows.Forms.RadioButton rdONAS400;
        private System.Windows.Forms.RadioButton rdONPC;
        private System.Windows.Forms.GroupBox CompareBox;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.GroupBox DepartmentBox;
        private System.Windows.Forms.TextBox txtBoxDepartment;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label1;
    }
}