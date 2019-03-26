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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.printButton = new System.Windows.Forms.Button();
            this.resultGridView = new System.Windows.Forms.DataGridView();
            this.LocationFront = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SectionCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SectionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
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
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(985, 1086);
            this.tableLayoutPanel1.TabIndex = 1;
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
            this.groupBox3.Location = new System.Drawing.Point(3, 133);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(979, 950);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Result";
            // 
            // printButton
            // 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.printButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printButton.Location = new System.Drawing.Point(695, 31);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(75, 27);
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
            this.resultGridView.Location = new System.Drawing.Point(10, 31);
            this.resultGridView.Name = "resultGridView";
            this.resultGridView.ReadOnly = true;
            this.resultGridView.Size = new System.Drawing.Size(679, 191);
            this.resultGridView.TabIndex = 6;
            // 
            // LocationFront
            // 
            this.LocationFront.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LocationFront.FillWeight = 284.7716F;
            this.LocationFront.HeaderText = "Sub Location";
            this.LocationFront.Name = "LocationFront";
            this.LocationFront.ReadOnly = true;
            this.LocationFront.Width = 300;
            // 
            // SectionCode
            // 
            this.SectionCode.FillWeight = 14.80229F;
            this.SectionCode.HeaderText = "Section Code";
            this.SectionCode.Name = "SectionCode";
            this.SectionCode.ReadOnly = true;
            this.SectionCode.Width = 200;
            // 
            // SectionName
            // 
            this.SectionName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SectionName.FillWeight = 0.4261313F;
            this.SectionName.HeaderText = "Section Name";
            this.SectionName.Name = "SectionName";
            this.SectionName.ReadOnly = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.comboBoxPlant);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.lbPlant);
            this.groupBox2.Controls.Add(this.comboBoxCountSheet);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.lbCountSheet);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.comboBoxStorageLocal);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.textBoxLoTo);
            this.groupBox2.Controls.Add(this.lbLocationTo);
            this.groupBox2.Controls.Add(this.lbLocationFrom);
            this.groupBox2.Controls.Add(this.lbSectionName);
            this.groupBox2.Controls.Add(this.textBoxLoForm);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(979, 124);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search";
            // 
            // comboBoxPlant
            // 
            this.comboBoxPlant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlant.FormattingEnabled = true;
            this.comboBoxPlant.Location = new System.Drawing.Point(157, 35);
            this.comboBoxPlant.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.comboBoxPlant.Name = "comboBoxPlant";
            this.comboBoxPlant.Size = new System.Drawing.Size(249, 26);
            this.comboBoxPlant.TabIndex = 58;
            this.comboBoxPlant.SelectionChangeCommitted += new System.EventHandler(this.comboBoxPlant_SelectedIndexChanged);

            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(143, 37);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 18);
            this.label9.TabIndex = 57;
            this.label9.Text = ":";
            // 
            // lbPlant
            // 
            this.lbPlant.AutoSize = true;
            this.lbPlant.Location = new System.Drawing.Point(13, 40);
            this.lbPlant.Name = "lbPlant";
            this.lbPlant.Size = new System.Drawing.Size(41, 18);
            this.lbPlant.TabIndex = 56;
            this.lbPlant.Text = "Plant";
            this.lbPlant.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxCountSheet
            // 
            this.comboBoxCountSheet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCountSheet.FormattingEnabled = true;
            this.comboBoxCountSheet.Location = new System.Drawing.Point(562, 35);
            this.comboBoxCountSheet.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.comboBoxCountSheet.Name = "comboBoxCountSheet";
            this.comboBoxCountSheet.Size = new System.Drawing.Size(249, 26);
            this.comboBoxCountSheet.TabIndex = 55;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(547, 37);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 18);
            this.label7.TabIndex = 54;
            this.label7.Text = ":";
            // 
            // lbCountSheet
            // 
            this.lbCountSheet.AutoSize = true;
            this.lbCountSheet.Location = new System.Drawing.Point(446, 38);
            this.lbCountSheet.Name = "lbCountSheet";
            this.lbCountSheet.Size = new System.Drawing.Size(86, 18);
            this.lbCountSheet.TabIndex = 53;
            this.lbCountSheet.Text = "CountSheet";
            this.lbCountSheet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(692, 73);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 18);
            this.label5.TabIndex = 52;
            this.label5.Text = ":";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(547, 75);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 18);
            this.label4.TabIndex = 51;
            this.label4.Text = ":";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(142, 73);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 18);
            this.label1.TabIndex = 50;
            this.label1.Text = ":";
            // 
            // comboBoxStorageLocal
            // 
            this.comboBoxStorageLocal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStorageLocal.FormattingEnabled = true;
            this.comboBoxStorageLocal.Location = new System.Drawing.Point(157, 71);
            this.comboBoxStorageLocal.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.comboBoxStorageLocal.Name = "comboBoxStorageLocal";
            this.comboBoxStorageLocal.Size = new System.Drawing.Size(249, 26);
            this.comboBoxStorageLocal.TabIndex = 49;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(847, 40);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(126, 40);
            this.btnSearch.TabIndex = 47;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // textBoxLoTo
            // 
            this.textBoxLoTo.Location = new System.Drawing.Point(715, 72);
            this.textBoxLoTo.MaxLength = 5;
            this.textBoxLoTo.Name = "textBoxLoTo";
            this.textBoxLoTo.Size = new System.Drawing.Size(96, 24);
            this.textBoxLoTo.TabIndex = 44;
            // 
            // lbLocationTo
            // 
            this.lbLocationTo.AutoSize = true;
            this.lbLocationTo.Location = new System.Drawing.Point(663, 75);
            this.lbLocationTo.Name = "lbLocationTo";
            this.lbLocationTo.Size = new System.Drawing.Size(26, 18);
            this.lbLocationTo.TabIndex = 48;
            this.lbLocationTo.Text = "To";
            this.lbLocationTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbLocationFrom
            // 
            this.lbLocationFrom.AutoSize = true;
            this.lbLocationFrom.Location = new System.Drawing.Point(411, 75);
            this.lbLocationFrom.Name = "lbLocationFrom";
            this.lbLocationFrom.Size = new System.Drawing.Size(139, 18);
            this.lbLocationFrom.TabIndex = 46;
            this.lbLocationFrom.Text = "Sub Location From ";
            this.lbLocationFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSectionName
            // 
            this.lbSectionName.AutoSize = true;
            this.lbSectionName.Location = new System.Drawing.Point(13, 75);
            this.lbSectionName.Name = "lbSectionName";
            this.lbSectionName.Size = new System.Drawing.Size(121, 18);
            this.lbSectionName.TabIndex = 45;
            this.lbSectionName.Text = "Storage Location";
            this.lbSectionName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxLoForm
            // 
            this.textBoxLoForm.Location = new System.Drawing.Point(562, 72);
            this.textBoxLoForm.MaxLength = 5;
            this.textBoxLoForm.Name = "textBoxLoForm";
            this.textBoxLoForm.Size = new System.Drawing.Size(91, 24);
            this.textBoxLoForm.TabIndex = 43;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.FillWeight = 10.30928F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Sub Location";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.FillWeight = 189.6907F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Storage Location";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "Section Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // BarcodePrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 542);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BarcodePrintForm";
            this.ShowIcon = false;
            this.Text = "Print BarCode";
            this.Load += new System.EventHandler(this.BarcodePrintForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.DataGridView resultGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.ComboBox comboBoxPlant;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbPlant;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationFront;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionName;
    }
}