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
            this.branch = new System.Windows.Forms.TextBox();
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
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.settingTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.chFront = new System.Windows.Forms.CheckBox();
            this.chBack = new System.Windows.Forms.CheckBox();
            this.chWareHouse = new System.Windows.Forms.CheckBox();
            this.chFreshFood = new System.Windows.Forms.CheckBox();
            this.settingTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // branch
            // 
            this.branch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.settingTableLayoutPanel.SetColumnSpan(this.branch, 2);
            this.branch.Location = new System.Drawing.Point(196, 145);
            this.branch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.branch.Name = "branch";
            this.branch.Size = new System.Drawing.Size(298, 32);
            this.branch.TabIndex = 11;
            // 
            // countDateDateTimePicker
            // 
            this.settingTableLayoutPanel.SetColumnSpan(this.countDateDateTimePicker, 2);
            this.countDateDateTimePicker.Location = new System.Drawing.Point(196, 275);
            this.countDateDateTimePicker.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.countDateDateTimePicker.Name = "countDateDateTimePicker";
            this.countDateDateTimePicker.Size = new System.Drawing.Size(298, 32);
            this.countDateDateTimePicker.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 281);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 26);
            this.label4.TabIndex = 3;
            this.label4.Text = "Count Date";
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(364, 365);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(112, 32);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(196, 365);
            this.saveButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(112, 32);
            this.saveButton.TabIndex = 8;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // comNameTextBox
            // 
            this.comNameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.settingTableLayoutPanel.SetColumnSpan(this.comNameTextBox, 2);
            this.comNameTextBox.Location = new System.Drawing.Point(196, 99);
            this.comNameTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comNameTextBox.Name = "comNameTextBox";
            this.comNameTextBox.Size = new System.Drawing.Size(298, 32);
            this.comNameTextBox.TabIndex = 6;
            // 
            // comIDTextBox
            // 
            this.comIDTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.settingTableLayoutPanel.SetColumnSpan(this.comIDTextBox, 2);
            this.comIDTextBox.Location = new System.Drawing.Point(196, 53);
            this.comIDTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comIDTextBox.MaxLength = 2;
            this.comIDTextBox.Name = "comIDTextBox";
            this.comIDTextBox.Size = new System.Drawing.Size(298, 32);
            this.comIDTextBox.TabIndex = 5;
            // 
            // maxLoginFailTextBox
            // 
            this.maxLoginFailTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.settingTableLayoutPanel.SetColumnSpan(this.maxLoginFailTextBox, 2);
            this.maxLoginFailTextBox.Location = new System.Drawing.Point(196, 7);
            this.maxLoginFailTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.maxLoginFailTextBox.Name = "maxLoginFailTextBox";
            this.maxLoginFailTextBox.Size = new System.Drawing.Size(298, 32);
            this.maxLoginFailTextBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 102);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 26);
            this.label3.TabIndex = 2;
            this.label3.Text = "Computer Name";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 56);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Computer ID";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Max Login Fail";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 148);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(146, 26);
            this.label5.TabIndex = 12;
            this.label5.Text = "Branch Name";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 191);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 26);
            this.label6.TabIndex = 13;
            this.label6.Text = "Section Type";
            // 
            // settingTableLayoutPanel
            // 
            this.settingTableLayoutPanel.ColumnCount = 3;
            this.settingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 192F));
            this.settingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 168F));
            this.settingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.settingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.settingTableLayoutPanel.Controls.Add(this.label6, 0, 4);
            this.settingTableLayoutPanel.Controls.Add(this.label5, 0, 3);
            this.settingTableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.settingTableLayoutPanel.Controls.Add(this.label2, 0, 1);
            this.settingTableLayoutPanel.Controls.Add(this.label3, 0, 2);
            this.settingTableLayoutPanel.Controls.Add(this.maxLoginFailTextBox, 1, 0);
            this.settingTableLayoutPanel.Controls.Add(this.comIDTextBox, 1, 1);
            this.settingTableLayoutPanel.Controls.Add(this.comNameTextBox, 1, 2);
            this.settingTableLayoutPanel.Controls.Add(this.branch, 1, 3);
            this.settingTableLayoutPanel.Controls.Add(this.cancelButton, 2, 8);
            this.settingTableLayoutPanel.Controls.Add(this.saveButton, 1, 8);
            this.settingTableLayoutPanel.Controls.Add(this.countDateDateTimePicker, 1, 6);
            this.settingTableLayoutPanel.Controls.Add(this.label4, 0, 6);
            this.settingTableLayoutPanel.Controls.Add(this.chFront, 1, 4);
            this.settingTableLayoutPanel.Controls.Add(this.chBack, 2, 4);
            this.settingTableLayoutPanel.Controls.Add(this.chWareHouse, 1, 5);
            this.settingTableLayoutPanel.Controls.Add(this.chFreshFood, 2, 5);
            this.settingTableLayoutPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingTableLayoutPanel.Location = new System.Drawing.Point(18, 18);
            this.settingTableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.settingTableLayoutPanel.Name = "settingTableLayoutPanel";
            this.settingTableLayoutPanel.RowCount = 9;
            this.settingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.settingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.settingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.settingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.settingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.settingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.settingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.settingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.settingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.settingTableLayoutPanel.Size = new System.Drawing.Size(545, 402);
            this.settingTableLayoutPanel.TabIndex = 0;
            // 
            // chFront
            // 
            this.chFront.AutoSize = true;
            this.chFront.Location = new System.Drawing.Point(196, 189);
            this.chFront.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chFront.Name = "chFront";
            this.chFront.Size = new System.Drawing.Size(88, 30);
            this.chFront.TabIndex = 14;
            this.chFront.Text = "Front";
            this.chFront.UseVisualStyleBackColor = true;
            // 
            // chBack
            // 
            this.chBack.AutoSize = true;
            this.chBack.Location = new System.Drawing.Point(364, 189);
            this.chBack.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chBack.Name = "chBack";
            this.chBack.Size = new System.Drawing.Size(87, 30);
            this.chBack.TabIndex = 15;
            this.chBack.Text = "Back";
            this.chBack.UseVisualStyleBackColor = true;
            // 
            // chWareHouse
            // 
            this.chWareHouse.AutoSize = true;
            this.chWareHouse.Location = new System.Drawing.Point(196, 230);
            this.chWareHouse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chWareHouse.Name = "chWareHouse";
            this.chWareHouse.Size = new System.Drawing.Size(149, 30);
            this.chWareHouse.TabIndex = 16;
            this.chWareHouse.Text = "Warehouse";
            this.chWareHouse.UseVisualStyleBackColor = true;
            // 
            // chFreshFood
            // 
            this.chFreshFood.AutoSize = true;
            this.chFreshFood.Location = new System.Drawing.Point(364, 230);
            this.chFreshFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chFreshFood.Name = "chFreshFood";
            this.chFreshFood.Size = new System.Drawing.Size(148, 30);
            this.chFreshFood.TabIndex = 17;
            this.chFreshFood.Text = "Fresh Food";
            this.chFreshFood.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 438);
            this.Controls.Add(this.settingTableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System Settings";
            this.settingTableLayoutPanel.ResumeLayout(false);
            this.settingTableLayoutPanel.PerformLayout();
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel settingTableLayoutPanel;
        private System.Windows.Forms.TextBox branch;
        private System.Windows.Forms.CheckBox chFront;
        private System.Windows.Forms.CheckBox chBack;
        private System.Windows.Forms.CheckBox chWareHouse;
        private System.Windows.Forms.CheckBox chFreshFood;
    }
}