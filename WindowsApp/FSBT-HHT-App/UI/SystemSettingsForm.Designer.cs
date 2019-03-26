namespace FSBT.HHT.App.UI
{
    partial class SettingsForm
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
            this.countDateDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.comNameTextBox = new System.Windows.Forms.TextBox();
            this.comIDTextBox = new System.Windows.Forms.TextBox();
            this.maxLoginFailTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.settingTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPlant = new System.Windows.Forms.TextBox();
            this.gbSystemSettings = new System.Windows.Forms.GroupBox();
            this.dataGridViewPlant = new System.Windows.Forms.DataGridView();
            this.PlantCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlantDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbMCHSettings = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.chEditMCH = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.mchLevel1TextBox = new System.Windows.Forms.TextBox();
            this.mchLevel2TextBox = new System.Windows.Forms.TextBox();
            this.mchLevel3TextBox = new System.Windows.Forms.TextBox();
            this.mchLevel4TextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbScanMode = new System.Windows.Forms.GroupBox();
            this.rbFreshfood = new System.Windows.Forms.RadioButton();
            this.rbProduct = new System.Windows.Forms.RadioButton();
            this.settingTableLayoutPanel.SuspendLayout();
            this.gbSystemSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlant)).BeginInit();
            this.gbMCHSettings.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbScanMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // countDateDateTimePicker
            // 
            this.settingTableLayoutPanel.SetColumnSpan(this.countDateDateTimePicker, 2);
            this.countDateDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.countDateDateTimePicker.Location = new System.Drawing.Point(153, 93);
            this.countDateDateTimePicker.Name = "countDateDateTimePicker";
            this.countDateDateTimePicker.Size = new System.Drawing.Size(200, 24);
            this.countDateDateTimePicker.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "Count Date";
            // 
            // cancelButton
            // 
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.cancelButton.Location = new System.Drawing.Point(460, 466);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(97, 49);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.saveButton.Location = new System.Drawing.Point(324, 466);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(97, 49);
            this.saveButton.TabIndex = 8;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // comNameTextBox
            // 
            this.comNameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.settingTableLayoutPanel.SetColumnSpan(this.comNameTextBox, 2);
            this.comNameTextBox.Location = new System.Drawing.Point(153, 63);
            this.comNameTextBox.Name = "comNameTextBox";
            this.comNameTextBox.Size = new System.Drawing.Size(200, 24);
            this.comNameTextBox.TabIndex = 6;
            // 
            // comIDTextBox
            // 
            this.comIDTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.settingTableLayoutPanel.SetColumnSpan(this.comIDTextBox, 2);
            this.comIDTextBox.Location = new System.Drawing.Point(153, 33);
            this.comIDTextBox.MaxLength = 2;
            this.comIDTextBox.Name = "comIDTextBox";
            this.comIDTextBox.Size = new System.Drawing.Size(200, 24);
            this.comIDTextBox.TabIndex = 5;
            // 
            // maxLoginFailTextBox
            // 
            this.maxLoginFailTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.settingTableLayoutPanel.SetColumnSpan(this.maxLoginFailTextBox, 2);
            this.maxLoginFailTextBox.Location = new System.Drawing.Point(153, 3);
            this.maxLoginFailTextBox.Name = "maxLoginFailTextBox";
            this.maxLoginFailTextBox.Size = new System.Drawing.Size(200, 24);
            this.maxLoginFailTextBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Computer Name";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Computer ID";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Max Login Fail";
            // 
            // settingTableLayoutPanel
            // 
            this.settingTableLayoutPanel.ColumnCount = 3;
            this.settingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.settingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.settingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.settingTableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.settingTableLayoutPanel.Controls.Add(this.label2, 0, 1);
            this.settingTableLayoutPanel.Controls.Add(this.label3, 0, 2);
            this.settingTableLayoutPanel.Controls.Add(this.maxLoginFailTextBox, 1, 0);
            this.settingTableLayoutPanel.Controls.Add(this.comIDTextBox, 1, 1);
            this.settingTableLayoutPanel.Controls.Add(this.comNameTextBox, 1, 2);
            this.settingTableLayoutPanel.Controls.Add(this.label4, 0, 3);
            this.settingTableLayoutPanel.Controls.Add(this.countDateDateTimePicker, 1, 3);
            this.settingTableLayoutPanel.Controls.Add(this.label7, 0, 4);
            this.settingTableLayoutPanel.Controls.Add(this.txtPlant, 1, 4);
            this.settingTableLayoutPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingTableLayoutPanel.Location = new System.Drawing.Point(13, 29);
            this.settingTableLayoutPanel.Name = "settingTableLayoutPanel";
            this.settingTableLayoutPanel.RowCount = 5;
            this.settingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.settingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.settingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.settingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.settingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.settingTableLayoutPanel.Size = new System.Drawing.Size(363, 148);
            this.settingTableLayoutPanel.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 125);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 18);
            this.label7.TabIndex = 18;
            this.label7.Text = "Plant";
            // 
            // txtPlant
            // 
            this.settingTableLayoutPanel.SetColumnSpan(this.txtPlant, 2);
            this.txtPlant.Location = new System.Drawing.Point(152, 123);
            this.txtPlant.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtPlant.Name = "txtPlant";
            this.txtPlant.Size = new System.Drawing.Size(201, 24);
            this.txtPlant.TabIndex = 20;
            this.txtPlant.Visible = false;
            // 
            // gbSystemSettings
            // 
            this.gbSystemSettings.Controls.Add(this.dataGridViewPlant);
            this.gbSystemSettings.Controls.Add(this.settingTableLayoutPanel);
            this.gbSystemSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.gbSystemSettings.Location = new System.Drawing.Point(21, 19);
            this.gbSystemSettings.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gbSystemSettings.Name = "gbSystemSettings";
            this.gbSystemSettings.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gbSystemSettings.Size = new System.Drawing.Size(400, 422);
            this.gbSystemSettings.TabIndex = 1;
            this.gbSystemSettings.TabStop = false;
            this.gbSystemSettings.Text = "System Settings";
            // 
            // dataGridViewPlant
            // 
            this.dataGridViewPlant.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPlant.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PlantCode,
            this.PlantDesc,
            this.Mode});
            this.dataGridViewPlant.Location = new System.Drawing.Point(13, 183);
            this.dataGridViewPlant.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewPlant.Name = "dataGridViewPlant";
            this.dataGridViewPlant.RowTemplate.Height = 28;
            this.dataGridViewPlant.Size = new System.Drawing.Size(353, 219);
            this.dataGridViewPlant.TabIndex = 1;
            // 
            // PlantCode
            // 
            this.PlantCode.HeaderText = "Plant Code";
            this.PlantCode.Name = "PlantCode";
            this.PlantCode.Width = 110;
            // 
            // PlantDesc
            // 
            this.PlantDesc.HeaderText = "Plant Description";
            this.PlantDesc.Name = "PlantDesc";
            this.PlantDesc.Width = 200;
            // 
            // Mode
            // 
            this.Mode.HeaderText = "Mode";
            this.Mode.Name = "Mode";
            this.Mode.Visible = false;
            // 
            // gbMCHSettings
            // 
            this.gbMCHSettings.Controls.Add(this.tableLayoutPanel1);
            this.gbMCHSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.gbMCHSettings.Location = new System.Drawing.Point(472, 19);
            this.gbMCHSettings.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gbMCHSettings.Name = "gbMCHSettings";
            this.gbMCHSettings.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gbMCHSettings.Size = new System.Drawing.Size(400, 202);
            this.gbMCHSettings.TabIndex = 2;
            this.gbMCHSettings.TabStop = false;
            this.gbMCHSettings.Text = "MCH Settings";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 265F));
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chEditMCH, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.mchLevel1TextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.mchLevel2TextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.mchLevel3TextBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.mchLevel4TextBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 4);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(29, 26);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(355, 151);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 18);
            this.label8.TabIndex = 21;
            this.label8.Text = "MCH Level 1";
            // 
            // chEditMCH
            // 
            this.chEditMCH.AutoSize = true;
            this.chEditMCH.Location = new System.Drawing.Point(2, 3);
            this.chEditMCH.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chEditMCH.Name = "chEditMCH";
            this.chEditMCH.Size = new System.Drawing.Size(129, 22);
            this.chEditMCH.TabIndex = 0;
            this.chEditMCH.Text = "Edit MCH Level";
            this.chEditMCH.UseVisualStyleBackColor = true;
            this.chEditMCH.CheckedChanged += new System.EventHandler(this.chEditMCH_CheckedChanged);
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 18);
            this.label9.TabIndex = 22;
            this.label9.Text = "MCH Level 2";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 96);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 18);
            this.label10.TabIndex = 23;
            this.label10.Text = "MCH Level 3";
            // 
            // mchLevel1TextBox
            // 
            this.mchLevel1TextBox.Enabled = false;
            this.mchLevel1TextBox.Location = new System.Drawing.Point(135, 33);
            this.mchLevel1TextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.mchLevel1TextBox.Name = "mchLevel1TextBox";
            this.mchLevel1TextBox.Size = new System.Drawing.Size(200, 24);
            this.mchLevel1TextBox.TabIndex = 25;
            // 
            // mchLevel2TextBox
            // 
            this.mchLevel2TextBox.Enabled = false;
            this.mchLevel2TextBox.Location = new System.Drawing.Point(135, 63);
            this.mchLevel2TextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.mchLevel2TextBox.Name = "mchLevel2TextBox";
            this.mchLevel2TextBox.Size = new System.Drawing.Size(200, 24);
            this.mchLevel2TextBox.TabIndex = 26;
            // 
            // mchLevel3TextBox
            // 
            this.mchLevel3TextBox.Enabled = false;
            this.mchLevel3TextBox.Location = new System.Drawing.Point(135, 93);
            this.mchLevel3TextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.mchLevel3TextBox.Name = "mchLevel3TextBox";
            this.mchLevel3TextBox.Size = new System.Drawing.Size(200, 24);
            this.mchLevel3TextBox.TabIndex = 27;
            // 
            // mchLevel4TextBox
            // 
            this.mchLevel4TextBox.Enabled = false;
            this.mchLevel4TextBox.Location = new System.Drawing.Point(135, 123);
            this.mchLevel4TextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.mchLevel4TextBox.Name = "mchLevel4TextBox";
            this.mchLevel4TextBox.Size = new System.Drawing.Size(200, 24);
            this.mchLevel4TextBox.TabIndex = 28;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 126);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(93, 18);
            this.label11.TabIndex = 24;
            this.label11.Text = "MCH Level 4";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Plant Code";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 200;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Plant Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 300;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Mode";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // gbScanMode
            // 
            this.gbScanMode.Controls.Add(this.rbFreshfood);
            this.gbScanMode.Controls.Add(this.rbProduct);
            this.gbScanMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.gbScanMode.Location = new System.Drawing.Point(472, 239);
            this.gbScanMode.Name = "gbScanMode";
            this.gbScanMode.Size = new System.Drawing.Size(400, 81);
            this.gbScanMode.TabIndex = 10;
            this.gbScanMode.TabStop = false;
            this.gbScanMode.Text = "Scan Mode";
            // 
            // rbFreshfood
            // 
            this.rbFreshfood.AutoSize = true;
            this.rbFreshfood.Location = new System.Drawing.Point(205, 34);
            this.rbFreshfood.Name = "rbFreshfood";
            this.rbFreshfood.Size = new System.Drawing.Size(103, 22);
            this.rbFreshfood.TabIndex = 1;
            this.rbFreshfood.Text = "Fresh Food";
            this.rbFreshfood.UseVisualStyleBackColor = true;
            // 
            // rbProduct
            // 
            this.rbProduct.AutoSize = true;
            this.rbProduct.Checked = true;
            this.rbProduct.Location = new System.Drawing.Point(35, 34);
            this.rbProduct.Name = "rbProduct";
            this.rbProduct.Size = new System.Drawing.Size(78, 22);
            this.rbProduct.TabIndex = 0;
            this.rbProduct.TabStop = true;
            this.rbProduct.Text = "Product";
            this.rbProduct.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 527);
            this.Controls.Add(this.gbScanMode);
            this.Controls.Add(this.gbMCHSettings);
            this.Controls.Add(this.gbSystemSettings);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.Text = "System Settings";
            this.settingTableLayoutPanel.ResumeLayout(false);
            this.settingTableLayoutPanel.PerformLayout();
            this.gbSystemSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlant)).EndInit();
            this.gbMCHSettings.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.gbScanMode.ResumeLayout(false);
            this.gbScanMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DateTimePicker countDateDateTimePicker;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox comNameTextBox;
        private System.Windows.Forms.TextBox comIDTextBox;
        private System.Windows.Forms.TextBox maxLoginFailTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel settingTableLayoutPanel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPlant;
        private System.Windows.Forms.GroupBox gbSystemSettings;
        private System.Windows.Forms.GroupBox gbMCHSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox chEditMCH;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox mchLevel1TextBox;
        private System.Windows.Forms.TextBox mchLevel2TextBox;
        private System.Windows.Forms.TextBox mchLevel3TextBox;
        private System.Windows.Forms.TextBox mchLevel4TextBox;
        private System.Windows.Forms.DataGridView dataGridViewPlant;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlantCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlantDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.GroupBox gbScanMode;
        private System.Windows.Forms.RadioButton rbFreshfood;
        private System.Windows.Forms.RadioButton rbProduct;
    }
}