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
            this.groupBoxDownloadType = new System.Windows.Forms.GroupBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnBrowseRegPrice = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.filePathRegPrice = new System.Windows.Forms.TextBox();
            this.btnBrowseBarcode = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.filePathBarcode = new System.Windows.Forms.TextBox();
            this.rbRegPrice = new System.Windows.Forms.RadioButton();
            this.rbBarcode = new System.Windows.Forms.RadioButton();
            this.rbSku = new System.Windows.Forms.RadioButton();
            this.btnBrowseSKU = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.filePathSKU = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbServer = new System.Windows.Forms.RadioButton();
            this.rbManual = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnViewLog = new System.Windows.Forms.Button();
            this.btnSumStorage = new System.Windows.Forms.Button();
            this.btnSumMaterial = new System.Windows.Forms.Button();
            this.btnSumBrand = new System.Windows.Forms.Button();
            this.btnSumSubDep = new System.Windows.Forms.Button();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnClearData = new System.Windows.Forms.Button();
            this.masterDataGridView = new System.Windows.Forms.DataGridView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBoxDownloadMode.SuspendLayout();
            this.groupBoxDownloadType.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterDataGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxDownloadMode
            // 
            this.groupBoxDownloadMode.Controls.Add(this.rbStock);
            this.groupBoxDownloadMode.Controls.Add(this.rbFreshfood);
            this.groupBoxDownloadMode.Controls.Add(this.rbFront);
            this.groupBoxDownloadMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxDownloadMode.Location = new System.Drawing.Point(384, 165);
            this.groupBoxDownloadMode.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxDownloadMode.Name = "groupBoxDownloadMode";
            this.groupBoxDownloadMode.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxDownloadMode.Size = new System.Drawing.Size(259, 105);
            this.groupBoxDownloadMode.TabIndex = 10;
            this.groupBoxDownloadMode.TabStop = false;
            this.groupBoxDownloadMode.Text = "Download mode";
            this.groupBoxDownloadMode.Visible = false;
            // 
            // rbStock
            // 
            this.rbStock.Location = new System.Drawing.Point(0, 0);
            this.rbStock.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbStock.Name = "rbStock";
            this.rbStock.Size = new System.Drawing.Size(104, 23);
            this.rbStock.TabIndex = 0;
            // 
            // rbFreshfood
            // 
            this.rbFreshfood.Location = new System.Drawing.Point(0, 0);
            this.rbFreshfood.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbFreshfood.Name = "rbFreshfood";
            this.rbFreshfood.Size = new System.Drawing.Size(104, 23);
            this.rbFreshfood.TabIndex = 1;
            // 
            // rbFront
            // 
            this.rbFront.Location = new System.Drawing.Point(0, 0);
            this.rbFront.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbFront.Name = "rbFront";
            this.rbFront.Size = new System.Drawing.Size(104, 23);
            this.rbFront.TabIndex = 2;
            // 
            // groupBoxDownloadType
            // 
            this.groupBoxDownloadType.Controls.Add(this.btnLoad);
            this.groupBoxDownloadType.Controls.Add(this.textBox1);
            this.groupBoxDownloadType.Controls.Add(this.btnSearch);
            this.groupBoxDownloadType.Controls.Add(this.groupBoxDownloadMode);
            this.groupBoxDownloadType.Controls.Add(this.btnBrowseRegPrice);
            this.groupBoxDownloadType.Controls.Add(this.label3);
            this.groupBoxDownloadType.Controls.Add(this.filePathRegPrice);
            this.groupBoxDownloadType.Controls.Add(this.btnBrowseBarcode);
            this.groupBoxDownloadType.Controls.Add(this.label2);
            this.groupBoxDownloadType.Controls.Add(this.filePathBarcode);
            this.groupBoxDownloadType.Controls.Add(this.rbRegPrice);
            this.groupBoxDownloadType.Controls.Add(this.rbBarcode);
            this.groupBoxDownloadType.Controls.Add(this.rbSku);
            this.groupBoxDownloadType.Controls.Add(this.btnBrowseSKU);
            this.groupBoxDownloadType.Controls.Add(this.label1);
            this.groupBoxDownloadType.Controls.Add(this.filePathSKU);
            this.groupBoxDownloadType.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxDownloadType.Location = new System.Drawing.Point(240, 12);
            this.groupBoxDownloadType.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxDownloadType.Name = "groupBoxDownloadType";
            this.groupBoxDownloadType.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxDownloadType.Size = new System.Drawing.Size(1291, 150);
            this.groupBoxDownloadType.TabIndex = 11;
            this.groupBoxDownloadType.TabStop = false;
            this.groupBoxDownloadType.Text = "Download Type";
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.Location = new System.Drawing.Point(992, 47);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(125, 86);
            this.btnLoad.TabIndex = 20;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(-160, 28);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(143, 28);
            this.textBox1.TabIndex = 16;
            this.textBox1.Text = "D:\\as400as400dw\\brandmas.txt";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(1140, 47);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(125, 87);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.Search_Click);
            // 
            // btnBrowseRegPrice
            // 
            this.btnBrowseRegPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseRegPrice.Location = new System.Drawing.Point(851, 111);
            this.btnBrowseRegPrice.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseRegPrice.Name = "btnBrowseRegPrice";
            this.btnBrowseRegPrice.Size = new System.Drawing.Size(100, 32);
            this.btnBrowseRegPrice.TabIndex = 13;
            this.btnBrowseRegPrice.Text = "Browse";
            this.btnBrowseRegPrice.UseVisualStyleBackColor = true;
            this.btnBrowseRegPrice.Click += new System.EventHandler(this.btnBrowseRegPrice_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(209, 114);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 24);
            this.label3.TabIndex = 12;
            this.label3.Text = "File Path : ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filePathRegPrice
            // 
            this.filePathRegPrice.Location = new System.Drawing.Point(321, 112);
            this.filePathRegPrice.Margin = new System.Windows.Forms.Padding(4);
            this.filePathRegPrice.Name = "filePathRegPrice";
            this.filePathRegPrice.Size = new System.Drawing.Size(516, 28);
            this.filePathRegPrice.TabIndex = 11;
            // 
            // btnBrowseBarcode
            // 
            this.btnBrowseBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseBarcode.Location = new System.Drawing.Point(851, 70);
            this.btnBrowseBarcode.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseBarcode.Name = "btnBrowseBarcode";
            this.btnBrowseBarcode.Size = new System.Drawing.Size(100, 32);
            this.btnBrowseBarcode.TabIndex = 10;
            this.btnBrowseBarcode.Text = "Browse";
            this.btnBrowseBarcode.UseVisualStyleBackColor = true;
            this.btnBrowseBarcode.Click += new System.EventHandler(this.btnBrowseBarcode_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(211, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 24);
            this.label2.TabIndex = 9;
            this.label2.Text = "File Path : ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filePathBarcode
            // 
            this.filePathBarcode.Location = new System.Drawing.Point(321, 71);
            this.filePathBarcode.Margin = new System.Windows.Forms.Padding(4);
            this.filePathBarcode.Name = "filePathBarcode";
            this.filePathBarcode.Size = new System.Drawing.Size(516, 28);
            this.filePathBarcode.TabIndex = 8;
            // 
            // rbRegPrice
            // 
            this.rbRegPrice.AutoSize = true;
            this.rbRegPrice.Location = new System.Drawing.Point(21, 112);
            this.rbRegPrice.Margin = new System.Windows.Forms.Padding(4);
            this.rbRegPrice.Name = "rbRegPrice";
            this.rbRegPrice.Size = new System.Drawing.Size(145, 28);
            this.rbRegPrice.TabIndex = 7;
            this.rbRegPrice.Text = "Regular Price";
            this.rbRegPrice.UseVisualStyleBackColor = true;
            this.rbRegPrice.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // rbBarcode
            // 
            this.rbBarcode.AutoSize = true;
            this.rbBarcode.Location = new System.Drawing.Point(21, 71);
            this.rbBarcode.Margin = new System.Windows.Forms.Padding(4);
            this.rbBarcode.Name = "rbBarcode";
            this.rbBarcode.Size = new System.Drawing.Size(102, 28);
            this.rbBarcode.TabIndex = 6;
            this.rbBarcode.Text = "Barcode";
            this.rbBarcode.UseVisualStyleBackColor = true;
            this.rbBarcode.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // rbSku
            // 
            this.rbSku.AutoSize = true;
            this.rbSku.Checked = true;
            this.rbSku.Location = new System.Drawing.Point(21, 34);
            this.rbSku.Margin = new System.Windows.Forms.Padding(4);
            this.rbSku.Name = "rbSku";
            this.rbSku.Size = new System.Drawing.Size(68, 28);
            this.rbSku.TabIndex = 3;
            this.rbSku.TabStop = true;
            this.rbSku.Text = "SKU";
            this.rbSku.UseVisualStyleBackColor = true;
            this.rbSku.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // btnBrowseSKU
            // 
            this.btnBrowseSKU.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseSKU.Location = new System.Drawing.Point(851, 32);
            this.btnBrowseSKU.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseSKU.Name = "btnBrowseSKU";
            this.btnBrowseSKU.Size = new System.Drawing.Size(100, 32);
            this.btnBrowseSKU.TabIndex = 4;
            this.btnBrowseSKU.Text = "Browse";
            this.btnBrowseSKU.UseVisualStyleBackColor = true;
            this.btnBrowseSKU.Click += new System.EventHandler(this.btnBrowseSKU_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(211, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "File Path : ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filePathSKU
            // 
            this.filePathSKU.Location = new System.Drawing.Point(321, 33);
            this.filePathSKU.Margin = new System.Windows.Forms.Padding(4);
            this.filePathSKU.Name = "filePathSKU";
            this.filePathSKU.Size = new System.Drawing.Size(516, 28);
            this.filePathSKU.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbServer);
            this.panel1.Controls.Add(this.rbManual);
            this.panel1.Location = new System.Drawing.Point(7, 36);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(201, 92);
            this.panel1.TabIndex = 21;
            // 
            // rbServer
            // 
            this.rbServer.AutoSize = true;
            this.rbServer.ForeColor = System.Drawing.Color.Black;
            this.rbServer.Location = new System.Drawing.Point(31, 11);
            this.rbServer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbServer.Name = "rbServer";
            this.rbServer.Size = new System.Drawing.Size(86, 28);
            this.rbServer.TabIndex = 20;
            this.rbServer.TabStop = true;
            this.rbServer.Text = "Server";
            this.rbServer.UseVisualStyleBackColor = true;
            this.rbServer.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // rbManual
            // 
            this.rbManual.AutoSize = true;
            this.rbManual.Checked = true;
            this.rbManual.ForeColor = System.Drawing.Color.Black;
            this.rbManual.Location = new System.Drawing.Point(31, 49);
            this.rbManual.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbManual.Name = "rbManual";
            this.rbManual.Size = new System.Drawing.Size(93, 28);
            this.rbManual.TabIndex = 21;
            this.rbManual.TabStop = true;
            this.rbManual.Text = "Manual";
            this.rbManual.UseVisualStyleBackColor = true;
            this.rbManual.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.AutoSize = true;
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.btnViewLog);
            this.groupBox3.Controls.Add(this.btnSumStorage);
            this.groupBox3.Controls.Add(this.btnSumMaterial);
            this.groupBox3.Controls.Add(this.btnSumBrand);
            this.groupBox3.Controls.Add(this.btnSumSubDep);
            this.groupBox3.Controls.Add(this.textBoxFilter);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.btnReport);
            this.groupBox3.Controls.Add(this.btnClearData);
            this.groupBox3.Controls.Add(this.masterDataGridView);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(16, 172);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.MinimumSize = new System.Drawing.Size(1156, 480);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(1514, 480);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Result";
            // 
            // btnViewLog
            // 
            this.btnViewLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewLog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnViewLog.Location = new System.Drawing.Point(21, 352);
            this.btnViewLog.Margin = new System.Windows.Forms.Padding(4);
            this.btnViewLog.Name = "btnViewLog";
            this.btnViewLog.Size = new System.Drawing.Size(147, 52);
            this.btnViewLog.TabIndex = 17;
            this.btnViewLog.Text = "View Log";
            this.btnViewLog.UseVisualStyleBackColor = true;
            this.btnViewLog.Click += new System.EventHandler(this.btnViewLog_Click);
            // 
            // btnSumStorage
            // 
            this.btnSumStorage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSumStorage.Location = new System.Drawing.Point(1071, 15);
            this.btnSumStorage.Margin = new System.Windows.Forms.Padding(4);
            this.btnSumStorage.Name = "btnSumStorage";
            this.btnSumStorage.Size = new System.Drawing.Size(177, 80);
            this.btnSumStorage.TabIndex = 16;
            this.btnSumStorage.Text = "Summary Storage Location";
            this.btnSumStorage.UseVisualStyleBackColor = true;
            this.btnSumStorage.Click += new System.EventHandler(this.summaryStorgeLocation_Click);
            // 
            // btnSumMaterial
            // 
            this.btnSumMaterial.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSumMaterial.Location = new System.Drawing.Point(889, 15);
            this.btnSumMaterial.Margin = new System.Windows.Forms.Padding(4);
            this.btnSumMaterial.Name = "btnSumMaterial";
            this.btnSumMaterial.Size = new System.Drawing.Size(177, 80);
            this.btnSumMaterial.TabIndex = 15;
            this.btnSumMaterial.Text = "Summary Material Group";
            this.btnSumMaterial.UseVisualStyleBackColor = true;
            this.btnSumMaterial.Click += new System.EventHandler(this.summaryMaterialGroup_Click);
            // 
            // btnSumBrand
            // 
            this.btnSumBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSumBrand.Location = new System.Drawing.Point(527, 15);
            this.btnSumBrand.Margin = new System.Windows.Forms.Padding(4);
            this.btnSumBrand.Name = "btnSumBrand";
            this.btnSumBrand.Size = new System.Drawing.Size(177, 80);
            this.btnSumBrand.TabIndex = 13;
            this.btnSumBrand.Text = "Summary Brand";
            this.btnSumBrand.UseVisualStyleBackColor = true;
            this.btnSumBrand.Click += new System.EventHandler(this.summaryBrand_Click);
            // 
            // btnSumSubDep
            // 
            this.btnSumSubDep.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSumSubDep.Location = new System.Drawing.Point(708, 15);
            this.btnSumSubDep.Margin = new System.Windows.Forms.Padding(4);
            this.btnSumSubDep.Name = "btnSumSubDep";
            this.btnSumSubDep.Size = new System.Drawing.Size(177, 80);
            this.btnSumSubDep.TabIndex = 12;
            this.btnSumSubDep.Text = "Summary Department";
            this.btnSumSubDep.UseVisualStyleBackColor = true;
            this.btnSumSubDep.Click += new System.EventHandler(this.summarySubDepartment_Click);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Location = new System.Drawing.Point(143, 49);
            this.textBoxFilter.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(283, 28);
            this.textBoxFilter.TabIndex = 11;
            this.textBoxFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxFilter_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(19, 53);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 24);
            this.label5.TabIndex = 10;
            this.label5.Text = "Filter Result";
            // 
            // btnReport
            // 
            this.btnReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReport.Location = new System.Drawing.Point(1381, 15);
            this.btnReport.Margin = new System.Windows.Forms.Padding(4);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(125, 80);
            this.btnReport.TabIndex = 6;
            this.btnReport.Text = "Report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnClearData
            // 
            this.btnClearData.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearData.Location = new System.Drawing.Point(1252, 15);
            this.btnClearData.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearData.Name = "btnClearData";
            this.btnClearData.Size = new System.Drawing.Size(125, 80);
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
            this.masterDataGridView.Location = new System.Drawing.Point(23, 103);
            this.masterDataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.masterDataGridView.Name = "masterDataGridView";
            this.masterDataGridView.Size = new System.Drawing.Size(1482, 241);
            this.masterDataGridView.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(216, 150);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Download mode";
            // 
            // DownloadMasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1691, 795);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBoxDownloadType);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DownloadMasterForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Download Master SAP";
            this.Load += new System.EventHandler(this.DownloadMasterForm_Load);
            this.groupBoxDownloadMode.ResumeLayout(false);
            this.groupBoxDownloadType.ResumeLayout(false);
            this.groupBoxDownloadType.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterDataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxDownloadMode;
        private System.Windows.Forms.RadioButton rbFreshfood;
        private System.Windows.Forms.RadioButton rbFront;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RadioButton rbStock;
        private System.Windows.Forms.TextBox filePathSKU;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseSKU;
        private System.Windows.Forms.RadioButton rbSku;
        private System.Windows.Forms.RadioButton rbBarcode;
        private System.Windows.Forms.RadioButton rbRegPrice;
        private System.Windows.Forms.TextBox filePathBarcode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowseBarcode;
        private System.Windows.Forms.TextBox filePathRegPrice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowseRegPrice;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBoxDownloadType;
        private System.Windows.Forms.DataGridView masterDataGridView;
        private System.Windows.Forms.Button btnClearData;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Button btnSumSubDep;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSumBrand;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSumStorage;
        private System.Windows.Forms.Button btnSumMaterial;
        private System.Windows.Forms.RadioButton rbServer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbManual;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnViewLog;
    }
}