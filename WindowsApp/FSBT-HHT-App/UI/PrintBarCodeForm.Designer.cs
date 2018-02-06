namespace FSBT.HHT.App.UI
{
    partial class BarcodePrintForm
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
            this.groupSectionType = new System.Windows.Forms.GroupBox();
            this.chFreshFood = new System.Windows.Forms.CheckBox();
            this.chWarehouse = new System.Windows.Forms.CheckBox();
            this.chBack = new System.Windows.Forms.CheckBox();
            this.chFront = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.printButton = new System.Windows.Forms.Button();
            this.resultGridView = new System.Windows.Forms.DataGridView();
            this.LocationFront = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SectionCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SectionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lbLocalFrom = new System.Windows.Forms.Label();
            this.textBoxLoFrom = new System.Windows.Forms.TextBox();
            this.lbLocalTo = new System.Windows.Forms.Label();
            this.textBoxLoTo = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.lbSecName = new System.Windows.Forms.Label();
            this.textBoxSecName = new System.Windows.Forms.TextBox();
            this.textBoxSecCode = new System.Windows.Forms.TextBox();
            this.lbSecCode = new System.Windows.Forms.Label();
            this.textBoxDeptCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupSectionType.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.groupSectionType, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(18, 18);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1476, 1240);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // groupSectionType
            // 
            this.groupSectionType.Controls.Add(this.chFreshFood);
            this.groupSectionType.Controls.Add(this.chWarehouse);
            this.groupSectionType.Controls.Add(this.chBack);
            this.groupSectionType.Controls.Add(this.chFront);
            this.groupSectionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupSectionType.Location = new System.Drawing.Point(4, 5);
            this.groupSectionType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupSectionType.Name = "groupSectionType";
            this.groupSectionType.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupSectionType.Size = new System.Drawing.Size(222, 240);
            this.groupSectionType.TabIndex = 2;
            this.groupSectionType.TabStop = false;
            this.groupSectionType.Text = "Section Type";
            // 
            // chFreshFood
            // 
            this.chFreshFood.AutoSize = true;
            this.chFreshFood.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chFreshFood.Location = new System.Drawing.Point(8, 154);
            this.chFreshFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chFreshFood.Name = "chFreshFood";
            this.chFreshFood.Size = new System.Drawing.Size(164, 33);
            this.chFreshFood.TabIndex = 3;
            this.chFreshFood.Text = "Fresh Food";
            this.chFreshFood.UseVisualStyleBackColor = true;
            // 
            // chWarehouse
            // 
            this.chWarehouse.AutoSize = true;
            this.chWarehouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chWarehouse.Location = new System.Drawing.Point(8, 114);
            this.chWarehouse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chWarehouse.Name = "chWarehouse";
            this.chWarehouse.Size = new System.Drawing.Size(162, 33);
            this.chWarehouse.TabIndex = 2;
            this.chWarehouse.Text = "Warehouse";
            this.chWarehouse.UseVisualStyleBackColor = true;
            // 
            // chBack
            // 
            this.chBack.AutoSize = true;
            this.chBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chBack.Location = new System.Drawing.Point(9, 74);
            this.chBack.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chBack.Name = "chBack";
            this.chBack.Size = new System.Drawing.Size(92, 33);
            this.chBack.TabIndex = 1;
            this.chBack.Text = "Back";
            this.chBack.UseVisualStyleBackColor = true;
            // 
            // chFront
            // 
            this.chFront.AutoSize = true;
            this.chFront.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chFront.Location = new System.Drawing.Point(9, 34);
            this.chFront.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chFront.Name = "chFront";
            this.chFront.Size = new System.Drawing.Size(95, 33);
            this.chFront.TabIndex = 0;
            this.chFront.Text = "Front";
            this.chFront.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox3, 2);
            this.groupBox3.Controls.Add(this.printButton);
            this.groupBox3.Controls.Add(this.resultGridView);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(4, 255);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(1468, 980);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Result";
            // 
            // printButton
            // 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.printButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printButton.Location = new System.Drawing.Point(1136, 48);
            this.printButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(112, 42);
            this.printButton.TabIndex = 5;
            this.printButton.Text = "Print";
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // resultGridView
            // 
            this.resultGridView.AllowUserToAddRows = false;
            this.resultGridView.AllowUserToDeleteRows = false;
            this.resultGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LocationFront,
            this.SectionCode,
            this.SectionName});
            this.resultGridView.Location = new System.Drawing.Point(15, 48);
            this.resultGridView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.resultGridView.Name = "resultGridView";
            this.resultGridView.ReadOnly = true;
            this.resultGridView.Size = new System.Drawing.Size(1113, 367);
            this.resultGridView.TabIndex = 6;
            // 
            // LocationFront
            // 
            this.LocationFront.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LocationFront.HeaderText = "Location";
            this.LocationFront.Name = "LocationFront";
            this.LocationFront.ReadOnly = true;
            // 
            // SectionCode
            // 
            this.SectionCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SectionCode.HeaderText = "Section Code";
            this.SectionCode.Name = "SectionCode";
            this.SectionCode.ReadOnly = true;
            // 
            // SectionName
            // 
            this.SectionName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SectionName.HeaderText = "Section Name";
            this.SectionName.Name = "SectionName";
            this.SectionName.ReadOnly = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(234, 5);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(1238, 240);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 225F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 267F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 279F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 316F));
            this.tableLayoutPanel2.Controls.Add(this.lbLocalFrom, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.textBoxLoFrom, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.lbLocalTo, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.textBoxLoTo, 3, 3);
            this.tableLayoutPanel2.Controls.Add(this.searchButton, 4, 3);
            this.tableLayoutPanel2.Controls.Add(this.lbSecName, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.textBoxSecName, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.textBoxSecCode, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lbSecCode, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBoxDeptCode, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(18, 31);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1113, 172);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // lbLocalFrom
            // 
            this.lbLocalFrom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbLocalFrom.AutoSize = true;
            this.lbLocalFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLocalFrom.Location = new System.Drawing.Point(4, 140);
            this.lbLocalFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLocalFrom.Name = "lbLocalFrom";
            this.lbLocalFrom.Size = new System.Drawing.Size(176, 29);
            this.lbLocalFrom.TabIndex = 5;
            this.lbLocalFrom.Text = "Location from : ";
            this.lbLocalFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxLoFrom
            // 
            this.textBoxLoFrom.Location = new System.Drawing.Point(229, 140);
            this.textBoxLoFrom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxLoFrom.MaxLength = 5;
            this.textBoxLoFrom.Name = "textBoxLoFrom";
            this.textBoxLoFrom.Size = new System.Drawing.Size(253, 33);
            this.textBoxLoFrom.TabIndex = 2;
            // 
            // lbLocalTo
            // 
            this.lbLocalTo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbLocalTo.AutoSize = true;
            this.lbLocalTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLocalTo.Location = new System.Drawing.Point(496, 140);
            this.lbLocalTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLocalTo.Name = "lbLocalTo";
            this.lbLocalTo.Size = new System.Drawing.Size(33, 29);
            this.lbLocalTo.TabIndex = 6;
            this.lbLocalTo.Text = "to";
            this.lbLocalTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxLoTo
            // 
            this.textBoxLoTo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxLoTo.Location = new System.Drawing.Point(541, 140);
            this.textBoxLoTo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxLoTo.MaxLength = 5;
            this.textBoxLoTo.Name = "textBoxLoTo";
            this.textBoxLoTo.Size = new System.Drawing.Size(253, 33);
            this.textBoxLoTo.TabIndex = 3;
            // 
            // searchButton
            // 
            this.searchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchButton.Location = new System.Drawing.Point(820, 140);
            this.searchButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(130, 30);
            this.searchButton.TabIndex = 4;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // lbSecName
            // 
            this.lbSecName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbSecName.AutoSize = true;
            this.lbSecName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSecName.Location = new System.Drawing.Point(4, 98);
            this.lbSecName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSecName.Name = "lbSecName";
            this.lbSecName.Size = new System.Drawing.Size(183, 29);
            this.lbSecName.TabIndex = 4;
            this.lbSecName.Text = "Section Name : ";
            this.lbSecName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSecName
            // 
            this.textBoxSecName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxSecName.Location = new System.Drawing.Point(229, 96);
            this.textBoxSecName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxSecName.MaxLength = 100;
            this.textBoxSecName.Name = "textBoxSecName";
            this.textBoxSecName.Size = new System.Drawing.Size(253, 33);
            this.textBoxSecName.TabIndex = 1;
            // 
            // textBoxSecCode
            // 
            this.textBoxSecCode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxSecCode.Location = new System.Drawing.Point(229, 51);
            this.textBoxSecCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxSecCode.MaxLength = 5;
            this.textBoxSecCode.Name = "textBoxSecCode";
            this.textBoxSecCode.Size = new System.Drawing.Size(253, 33);
            this.textBoxSecCode.TabIndex = 0;
            // 
            // lbSecCode
            // 
            this.lbSecCode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbSecCode.AutoSize = true;
            this.lbSecCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSecCode.Location = new System.Drawing.Point(4, 53);
            this.lbSecCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSecCode.Name = "lbSecCode";
            this.lbSecCode.Size = new System.Drawing.Size(177, 29);
            this.lbSecCode.TabIndex = 3;
            this.lbSecCode.Text = "Section Code : ";
            this.lbSecCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDeptCode
            // 
            this.textBoxDeptCode.Location = new System.Drawing.Point(229, 5);
            this.textBoxDeptCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxDeptCode.MaxLength = 3;
            this.textBoxDeptCode.Name = "textBoxDeptCode";
            this.textBoxDeptCode.Size = new System.Drawing.Size(253, 33);
            this.textBoxDeptCode.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 45);
            this.label1.TabIndex = 4;
            this.label1.Text = "Department Code : ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BarcodePrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1512, 834);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "BarcodePrintForm";
            this.ShowIcon = false;
            this.Text = "Print BarCode";
            this.Load += new System.EventHandler(this.BarcodePrintForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupSectionType.ResumeLayout(false);
            this.groupSectionType.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox textBoxLoFrom;
        private System.Windows.Forms.Label lbLocalTo;
        private System.Windows.Forms.Label lbLocalFrom;
        private System.Windows.Forms.Label lbSecName;
        private System.Windows.Forms.Label lbSecCode;
        private System.Windows.Forms.TextBox textBoxLoTo;
        private System.Windows.Forms.TextBox textBoxSecName;
        private System.Windows.Forms.TextBox textBoxSecCode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.DataGridView resultGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationFront;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionName;
        private System.Windows.Forms.GroupBox groupSectionType;
        private System.Windows.Forms.TextBox textBoxDeptCode;
        private System.Windows.Forms.CheckBox chFreshFood;
        private System.Windows.Forms.CheckBox chWarehouse;
        private System.Windows.Forms.CheckBox chBack;
        private System.Windows.Forms.CheckBox chFront;
        private System.Windows.Forms.Label label1;
    }
}