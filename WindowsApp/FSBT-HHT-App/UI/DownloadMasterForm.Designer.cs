namespace FSBT.HHT.App.UI
{
    partial class DownloadMasterForm
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
            this.groupBoxDownloadMode = new System.Windows.Forms.GroupBox();
            this.rbStock = new System.Windows.Forms.RadioButton();
            this.rbFreshfood = new System.Windows.Forms.RadioButton();
            this.rbFront = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbSuper = new System.Windows.Forms.RadioButton();
            this.rbDepartment = new System.Windows.Forms.RadioButton();
            this.groupBoxDownloadType = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnBrowseBrand = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.filePathBrand = new System.Windows.Forms.TextBox();
            this.rbBrand = new System.Windows.Forms.RadioButton();
            this.btnBrowsePackBarcode = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.filePathPackBarcode = new System.Windows.Forms.TextBox();
            this.btnBrowseBarcode = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.filePathBarcode = new System.Windows.Forms.TextBox();
            this.rbPackBarcode = new System.Windows.Forms.RadioButton();
            this.rbBarcode = new System.Windows.Forms.RadioButton();
            this.rbSku = new System.Windows.Forms.RadioButton();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnBrowseSKU = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.filePathSKU = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Search = new System.Windows.Forms.Button();
            this.summaryBrand = new System.Windows.Forms.Button();
            this.summaryDepartment = new System.Windows.Forms.Button();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnClearData = new System.Windows.Forms.Button();
            this.masterDataGridView = new System.Windows.Forms.DataGridView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxDownloadMode.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxDownloadType.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxDownloadMode
            // 
            this.groupBoxDownloadMode.Controls.Add(this.rbStock);
            this.groupBoxDownloadMode.Controls.Add(this.rbFreshfood);
            this.groupBoxDownloadMode.Controls.Add(this.rbFront);
            this.groupBoxDownloadMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxDownloadMode.Location = new System.Drawing.Point(15, 89);
            this.groupBoxDownloadMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxDownloadMode.Name = "groupBoxDownloadMode";
            this.groupBoxDownloadMode.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxDownloadMode.Size = new System.Drawing.Size(291, 131);
            this.groupBoxDownloadMode.TabIndex = 10;
            this.groupBoxDownloadMode.TabStop = false;
            this.groupBoxDownloadMode.Text = "Download mode";
            // 
            // rbStock
            // 
            this.rbStock.AutoSize = true;
            this.rbStock.Location = new System.Drawing.Point(26, 58);
            this.rbStock.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbStock.Name = "rbStock";
            this.rbStock.Size = new System.Drawing.Size(148, 30);
            this.rbStock.TabIndex = 3;
            this.rbStock.TabStop = true;
            this.rbStock.Text = "Warehouse";
            this.rbStock.UseVisualStyleBackColor = true;
            this.rbStock.CheckedChanged += new System.EventHandler(this.rbStock_CheckedChanged);
            // 
            // rbFreshfood
            // 
            this.rbFreshfood.AutoSize = true;
            this.rbFreshfood.Location = new System.Drawing.Point(26, 88);
            this.rbFreshfood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbFreshfood.Name = "rbFreshfood";
            this.rbFreshfood.Size = new System.Drawing.Size(147, 30);
            this.rbFreshfood.TabIndex = 2;
            this.rbFreshfood.TabStop = true;
            this.rbFreshfood.Text = "Fresh Food";
            this.rbFreshfood.UseVisualStyleBackColor = true;
            this.rbFreshfood.CheckedChanged += new System.EventHandler(this.rbFreshfood_CheckedChanged);
            // 
            // rbFront
            // 
            this.rbFront.AutoSize = true;
            this.rbFront.Checked = true;
            this.rbFront.Location = new System.Drawing.Point(26, 29);
            this.rbFront.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbFront.Name = "rbFront";
            this.rbFront.Size = new System.Drawing.Size(87, 30);
            this.rbFront.TabIndex = 0;
            this.rbFront.TabStop = true;
            this.rbFront.Text = "Front";
            this.rbFront.UseVisualStyleBackColor = true;
            this.rbFront.CheckedChanged += new System.EventHandler(this.rbFront_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1500F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxDownloadType, 1, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 6);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1800, 230);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbSuper);
            this.groupBox1.Controls.Add(this.rbDepartment);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(4, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(291, 69);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Catagory mode";
            // 
            // rbSuper
            // 
            this.rbSuper.AutoSize = true;
            this.rbSuper.Location = new System.Drawing.Point(184, 29);
            this.rbSuper.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbSuper.Name = "rbSuper";
            this.rbSuper.Size = new System.Drawing.Size(95, 30);
            this.rbSuper.TabIndex = 3;
            this.rbSuper.TabStop = true;
            this.rbSuper.Text = "Super";
            this.rbSuper.UseVisualStyleBackColor = true;
            this.rbSuper.CheckedChanged += new System.EventHandler(this.rbSuper_CheckedChanged);
            // 
            // rbDepartment
            // 
            this.rbDepartment.AutoSize = true;
            this.rbDepartment.Checked = true;
            this.rbDepartment.Location = new System.Drawing.Point(26, 29);
            this.rbDepartment.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbDepartment.Name = "rbDepartment";
            this.rbDepartment.Size = new System.Drawing.Size(151, 30);
            this.rbDepartment.TabIndex = 0;
            this.rbDepartment.TabStop = true;
            this.rbDepartment.Text = "Department";
            this.rbDepartment.UseVisualStyleBackColor = true;
            this.rbDepartment.CheckedChanged += new System.EventHandler(this.rbDepartment_CheckedChanged);
            // 
            // groupBoxDownloadType
            // 
            this.groupBoxDownloadType.Controls.Add(this.textBox1);
            this.groupBoxDownloadType.Controls.Add(this.btnBrowseBrand);
            this.groupBoxDownloadType.Controls.Add(this.label4);
            this.groupBoxDownloadType.Controls.Add(this.filePathBrand);
            this.groupBoxDownloadType.Controls.Add(this.rbBrand);
            this.groupBoxDownloadType.Controls.Add(this.btnBrowsePackBarcode);
            this.groupBoxDownloadType.Controls.Add(this.label3);
            this.groupBoxDownloadType.Controls.Add(this.filePathPackBarcode);
            this.groupBoxDownloadType.Controls.Add(this.btnBrowseBarcode);
            this.groupBoxDownloadType.Controls.Add(this.label2);
            this.groupBoxDownloadType.Controls.Add(this.filePathBarcode);
            this.groupBoxDownloadType.Controls.Add(this.rbPackBarcode);
            this.groupBoxDownloadType.Controls.Add(this.rbBarcode);
            this.groupBoxDownloadType.Controls.Add(this.rbSku);
            this.groupBoxDownloadType.Controls.Add(this.btnLoad);
            this.groupBoxDownloadType.Controls.Add(this.btnBrowseSKU);
            this.groupBoxDownloadType.Controls.Add(this.label1);
            this.groupBoxDownloadType.Controls.Add(this.filePathSKU);
            this.groupBoxDownloadType.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxDownloadType.Location = new System.Drawing.Point(304, 5);
            this.groupBoxDownloadType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxDownloadType.Name = "groupBoxDownloadType";
            this.groupBoxDownloadType.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxDownloadType.Size = new System.Drawing.Size(1480, 209);
            this.groupBoxDownloadType.TabIndex = 11;
            this.groupBoxDownloadType.TabStop = false;
            this.groupBoxDownloadType.Text = "Download Type";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(-180, 35);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(160, 32);
            this.textBox1.TabIndex = 16;
            this.textBox1.Text = "D:\\as400as400dw\\brandmas.txt";
            // 
            // btnBrowseBrand
            // 
            this.btnBrowseBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseBrand.Location = new System.Drawing.Point(915, 152);
            this.btnBrowseBrand.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBrowseBrand.Name = "btnBrowseBrand";
            this.btnBrowseBrand.Size = new System.Drawing.Size(112, 35);
            this.btnBrowseBrand.TabIndex = 17;
            this.btnBrowseBrand.Text = "Browse";
            this.btnBrowseBrand.UseVisualStyleBackColor = true;
            this.btnBrowseBrand.Click += new System.EventHandler(this.btnBrowseBrand_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(195, 157);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 26);
            this.label4.TabIndex = 16;
            this.label4.Text = "File Path : ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filePathBrand
            // 
            this.filePathBrand.Location = new System.Drawing.Point(320, 154);
            this.filePathBrand.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.filePathBrand.Name = "filePathBrand";
            this.filePathBrand.Size = new System.Drawing.Size(580, 32);
            this.filePathBrand.TabIndex = 15;
            // 
            // rbBrand
            // 
            this.rbBrand.AutoSize = true;
            this.rbBrand.Location = new System.Drawing.Point(24, 155);
            this.rbBrand.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbBrand.Name = "rbBrand";
            this.rbBrand.Size = new System.Drawing.Size(95, 30);
            this.rbBrand.TabIndex = 14;
            this.rbBrand.Text = "Brand";
            this.rbBrand.UseVisualStyleBackColor = true;
            this.rbBrand.CheckedChanged += new System.EventHandler(this.rbBrand_CheckedChanged);
            // 
            // btnBrowsePackBarcode
            // 
            this.btnBrowsePackBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowsePackBarcode.Location = new System.Drawing.Point(915, 112);
            this.btnBrowsePackBarcode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBrowsePackBarcode.Name = "btnBrowsePackBarcode";
            this.btnBrowsePackBarcode.Size = new System.Drawing.Size(112, 35);
            this.btnBrowsePackBarcode.TabIndex = 13;
            this.btnBrowsePackBarcode.Text = "Browse";
            this.btnBrowsePackBarcode.UseVisualStyleBackColor = true;
            this.btnBrowsePackBarcode.Click += new System.EventHandler(this.btnBrowsePackBarcode_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(195, 115);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 26);
            this.label3.TabIndex = 12;
            this.label3.Text = "File Path : ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filePathPackBarcode
            // 
            this.filePathPackBarcode.Location = new System.Drawing.Point(320, 112);
            this.filePathPackBarcode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.filePathPackBarcode.Name = "filePathPackBarcode";
            this.filePathPackBarcode.Size = new System.Drawing.Size(580, 32);
            this.filePathPackBarcode.TabIndex = 11;
            // 
            // btnBrowseBarcode
            // 
            this.btnBrowseBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseBarcode.Location = new System.Drawing.Point(915, 71);
            this.btnBrowseBarcode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBrowseBarcode.Name = "btnBrowseBarcode";
            this.btnBrowseBarcode.Size = new System.Drawing.Size(112, 35);
            this.btnBrowseBarcode.TabIndex = 10;
            this.btnBrowseBarcode.Text = "Browse";
            this.btnBrowseBarcode.UseVisualStyleBackColor = true;
            this.btnBrowseBarcode.Click += new System.EventHandler(this.btnBrowseBarcode_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 74);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 26);
            this.label2.TabIndex = 9;
            this.label2.Text = "File Path : ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filePathBarcode
            // 
            this.filePathBarcode.Location = new System.Drawing.Point(320, 72);
            this.filePathBarcode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.filePathBarcode.Name = "filePathBarcode";
            this.filePathBarcode.Size = new System.Drawing.Size(580, 32);
            this.filePathBarcode.TabIndex = 8;
            // 
            // rbPackBarcode
            // 
            this.rbPackBarcode.AutoSize = true;
            this.rbPackBarcode.Location = new System.Drawing.Point(24, 114);
            this.rbPackBarcode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbPackBarcode.Name = "rbPackBarcode";
            this.rbPackBarcode.Size = new System.Drawing.Size(173, 30);
            this.rbPackBarcode.TabIndex = 7;
            this.rbPackBarcode.Text = "Pack Barcode";
            this.rbPackBarcode.UseVisualStyleBackColor = true;
            this.rbPackBarcode.CheckedChanged += new System.EventHandler(this.rbPackBarcode_CheckedChanged);
            // 
            // rbBarcode
            // 
            this.rbBarcode.AutoSize = true;
            this.rbBarcode.Location = new System.Drawing.Point(24, 72);
            this.rbBarcode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbBarcode.Name = "rbBarcode";
            this.rbBarcode.Size = new System.Drawing.Size(118, 30);
            this.rbBarcode.TabIndex = 6;
            this.rbBarcode.Text = "Barcode";
            this.rbBarcode.UseVisualStyleBackColor = true;
            this.rbBarcode.CheckedChanged += new System.EventHandler(this.rbBarcode_CheckedChanged);
            // 
            // rbSku
            // 
            this.rbSku.AutoSize = true;
            this.rbSku.Checked = true;
            this.rbSku.Location = new System.Drawing.Point(24, 35);
            this.rbSku.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbSku.Name = "rbSku";
            this.rbSku.Size = new System.Drawing.Size(83, 30);
            this.rbSku.TabIndex = 3;
            this.rbSku.TabStop = true;
            this.rbSku.Text = "SKU";
            this.rbSku.UseVisualStyleBackColor = true;
            this.rbSku.CheckedChanged += new System.EventHandler(this.rbSku_CheckedChanged);
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.Location = new System.Drawing.Point(1149, 35);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(112, 109);
            this.btnLoad.TabIndex = 5;
            this.btnLoad.Text = "LOAD";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnBrowseSKU
            // 
            this.btnBrowseSKU.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseSKU.Location = new System.Drawing.Point(915, 29);
            this.btnBrowseSKU.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBrowseSKU.Name = "btnBrowseSKU";
            this.btnBrowseSKU.Size = new System.Drawing.Size(112, 35);
            this.btnBrowseSKU.TabIndex = 4;
            this.btnBrowseSKU.Text = "Browse";
            this.btnBrowseSKU.UseVisualStyleBackColor = true;
            this.btnBrowseSKU.Click += new System.EventHandler(this.btnBrowseSKU_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(195, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 26);
            this.label1.TabIndex = 3;
            this.label1.Text = "File Path : ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filePathSKU
            // 
            this.filePathSKU.Location = new System.Drawing.Point(320, 31);
            this.filePathSKU.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.filePathSKU.Name = "filePathSKU";
            this.filePathSKU.Size = new System.Drawing.Size(580, 32);
            this.filePathSKU.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.AutoSize = true;
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.Search);
            this.groupBox3.Controls.Add(this.summaryBrand);
            this.groupBox3.Controls.Add(this.summaryDepartment);
            this.groupBox3.Controls.Add(this.textBoxFilter);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.btnReport);
            this.groupBox3.Controls.Add(this.btnClearData);
            this.groupBox3.Controls.Add(this.masterDataGridView);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(17, 230);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.MinimumSize = new System.Drawing.Size(1300, 600);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(1780, 600);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Result";
            // 
            // Search
            // 
            this.Search.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Search.Location = new System.Drawing.Point(920, 35);
            this.Search.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(128, 71);
            this.Search.TabIndex = 14;
            this.Search.Text = "Search";
            this.Search.UseVisualStyleBackColor = true;
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // summaryBrand
            // 
            this.summaryBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.summaryBrand.Location = new System.Drawing.Point(1056, 35);
            this.summaryBrand.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.summaryBrand.Name = "summaryBrand";
            this.summaryBrand.Size = new System.Drawing.Size(128, 71);
            this.summaryBrand.TabIndex = 13;
            this.summaryBrand.Text = "Summary Brand";
            this.summaryBrand.UseVisualStyleBackColor = true;
            this.summaryBrand.Click += new System.EventHandler(this.summaryBrand_Click);
            // 
            // summaryDepartment
            // 
            this.summaryDepartment.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.summaryDepartment.Location = new System.Drawing.Point(1191, 35);
            this.summaryDepartment.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.summaryDepartment.Name = "summaryDepartment";
            this.summaryDepartment.Size = new System.Drawing.Size(168, 71);
            this.summaryDepartment.TabIndex = 12;
            this.summaryDepartment.Text = "Summary Department";
            this.summaryDepartment.UseVisualStyleBackColor = true;
            this.summaryDepartment.Click += new System.EventHandler(this.summaryDepartment_Click);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Location = new System.Drawing.Point(154, 62);
            this.textBoxFilter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(349, 32);
            this.textBoxFilter.TabIndex = 11;
            this.textBoxFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxFilter_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(21, 66);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 26);
            this.label5.TabIndex = 10;
            this.label5.Text = "Filter Result";
            // 
            // btnReport
            // 
            this.btnReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReport.Location = new System.Drawing.Point(1503, 35);
            this.btnReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(114, 71);
            this.btnReport.TabIndex = 6;
            this.btnReport.Text = "Report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnClearData
            // 
            this.btnClearData.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearData.Location = new System.Drawing.Point(1368, 35);
            this.btnClearData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClearData.Name = "btnClearData";
            this.btnClearData.Size = new System.Drawing.Size(126, 71);
            this.btnClearData.TabIndex = 3;
            this.btnClearData.Text = "Clear Data";
            this.btnClearData.UseVisualStyleBackColor = true;
            this.btnClearData.Click += new System.EventHandler(this.btnClearData_Click);
            // 
            // masterDataGridView
            // 
            this.masterDataGridView.AllowUserToAddRows = false;
            this.masterDataGridView.AllowUserToDeleteRows = false;
            this.masterDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.masterDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.masterDataGridView.Location = new System.Drawing.Point(26, 129);
            this.masterDataGridView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.masterDataGridView.Name = "masterDataGridView";
            this.masterDataGridView.Size = new System.Drawing.Size(1728, 436);
            this.masterDataGridView.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // DownloadMasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1902, 994);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBoxDownloadMode);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DownloadMasterForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Download Master AS400";
            this.Load += new System.EventHandler(this.DownloadMasterForm_Load);
            this.groupBoxDownloadMode.ResumeLayout(false);
            this.groupBoxDownloadMode.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxDownloadType.ResumeLayout(false);
            this.groupBoxDownloadType.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxDownloadMode;
        private System.Windows.Forms.RadioButton rbFreshfood;
        private System.Windows.Forms.RadioButton rbFront;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RadioButton rbStock;
        private System.Windows.Forms.TextBox filePathSKU;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseSKU;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.RadioButton rbSku;
        private System.Windows.Forms.RadioButton rbBarcode;
        private System.Windows.Forms.RadioButton rbPackBarcode;
        private System.Windows.Forms.TextBox filePathBarcode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowseBarcode;
        private System.Windows.Forms.TextBox filePathPackBarcode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowsePackBarcode;
        private System.Windows.Forms.RadioButton rbBrand;
        private System.Windows.Forms.TextBox filePathBrand;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnBrowseBrand;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBoxDownloadType;
        private System.Windows.Forms.DataGridView masterDataGridView;
        private System.Windows.Forms.Button btnClearData;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Button summaryDepartment;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button summaryBrand;
        private System.Windows.Forms.Button Search;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbSuper;
        private System.Windows.Forms.RadioButton rbDepartment;
    }
}