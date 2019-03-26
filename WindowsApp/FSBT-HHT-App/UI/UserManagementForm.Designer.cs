namespace FSBT.HHT.App.UI
{
    partial class UserManagementForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.deleteUserButton = new System.Windows.Forms.Button();
            this.addUserButton = new System.Windows.Forms.Button();
            this.searchUserButton = new System.Windows.Forms.Button();
            this.searchUserTextBox = new System.Windows.Forms.TextBox();
            this.searchUserLabel = new System.Windows.Forms.Label();
            this.userDataGridView = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cancelUserButton = new System.Windows.Forms.Button();
            this.saveUserButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lockPanel = new System.Windows.Forms.Panel();
            this.lockFalseRadioButton = new System.Windows.Forms.RadioButton();
            this.lockTrueRadioButton = new System.Windows.Forms.RadioButton();
            this.firstNameLabel = new System.Windows.Forms.Label();
            this.enableLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.firstNameTextBox = new System.Windows.Forms.TextBox();
            this.enablePanel = new System.Windows.Forms.Panel();
            this.enableFalseRadioButton = new System.Windows.Forms.RadioButton();
            this.enableTrueRadioButton = new System.Windows.Forms.RadioButton();
            this.resetPasswordButton = new System.Windows.Forms.Button();
            this.lockLabel = new System.Windows.Forms.Label();
            this.lastNameLabel = new System.Windows.Forms.Label();
            this.lastNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupComboBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userDataGridView)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.lockPanel.SuspendLayout();
            this.enablePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1441, 625);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.deleteUserButton);
            this.panel1.Controls.Add(this.addUserButton);
            this.panel1.Controls.Add(this.searchUserButton);
            this.panel1.Controls.Add(this.searchUserTextBox);
            this.panel1.Controls.Add(this.searchUserLabel);
            this.panel1.Controls.Add(this.userDataGridView);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1435, 244);
            this.panel1.TabIndex = 0;
            // 
            // deleteUserButton
            // 
            this.deleteUserButton.Location = new System.Drawing.Point(900, 4);
            this.deleteUserButton.Name = "deleteUserButton";
            this.deleteUserButton.Size = new System.Drawing.Size(75, 23);
            this.deleteUserButton.TabIndex = 5;
            this.deleteUserButton.Text = "Delete";
            this.deleteUserButton.UseVisualStyleBackColor = true;
            this.deleteUserButton.Click += new System.EventHandler(this.deleteUserButton_Click);
            // 
            // addUserButton
            // 
            this.addUserButton.Location = new System.Drawing.Point(819, 4);
            this.addUserButton.Name = "addUserButton";
            this.addUserButton.Size = new System.Drawing.Size(75, 23);
            this.addUserButton.TabIndex = 4;
            this.addUserButton.Text = "Add";
            this.addUserButton.UseVisualStyleBackColor = true;
            this.addUserButton.Click += new System.EventHandler(this.addUserButton_Click);
            // 
            // searchUserButton
            // 
            this.searchUserButton.Location = new System.Drawing.Point(362, 5);
            this.searchUserButton.Name = "searchUserButton";
            this.searchUserButton.Size = new System.Drawing.Size(75, 23);
            this.searchUserButton.TabIndex = 1;
            this.searchUserButton.Text = "Search";
            this.searchUserButton.UseVisualStyleBackColor = true;
            this.searchUserButton.Click += new System.EventHandler(this.searchUserButton_Click);
            // 
            // searchUserTextBox
            // 
            this.searchUserTextBox.Location = new System.Drawing.Point(97, 7);
            this.searchUserTextBox.Name = "searchUserTextBox";
            this.searchUserTextBox.Size = new System.Drawing.Size(259, 24);
            this.searchUserTextBox.TabIndex = 0;
            this.searchUserTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchUserTextBox_KeyDown);
            // 
            // searchUserLabel
            // 
            this.searchUserLabel.AutoSize = true;
            this.searchUserLabel.Location = new System.Drawing.Point(3, 10);
            this.searchUserLabel.Name = "searchUserLabel";
            this.searchUserLabel.Size = new System.Drawing.Size(91, 18);
            this.searchUserLabel.TabIndex = 1;
            this.searchUserLabel.Text = "Search User";
            // 
            // userDataGridView
            // 
            this.userDataGridView.AllowUserToAddRows = false;
            this.userDataGridView.AllowUserToDeleteRows = false;
            this.userDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.userDataGridView.Location = new System.Drawing.Point(6, 55);
            this.userDataGridView.Name = "userDataGridView";
            this.userDataGridView.ReadOnly = true;
            this.userDataGridView.Size = new System.Drawing.Size(1315, 176);
            this.userDataGridView.TabIndex = 2;
            this.userDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.userDataGridView_CellClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Location = new System.Drawing.Point(3, 253);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(978, 305);
            this.panel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.cancelUserButton);
            this.groupBox1.Controls.Add(this.saveUserButton);
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Location = new System.Drawing.Point(6, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(969, 299);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Detail";
            // 
            // cancelUserButton
            // 
            this.cancelUserButton.Location = new System.Drawing.Point(87, 19);
            this.cancelUserButton.Name = "cancelUserButton";
            this.cancelUserButton.Size = new System.Drawing.Size(75, 23);
            this.cancelUserButton.TabIndex = 13;
            this.cancelUserButton.Text = "Clear";
            this.cancelUserButton.UseVisualStyleBackColor = true;
            this.cancelUserButton.Click += new System.EventHandler(this.cancelUserButton_Click);
            // 
            // saveUserButton
            // 
            this.saveUserButton.Location = new System.Drawing.Point(6, 19);
            this.saveUserButton.Name = "saveUserButton";
            this.saveUserButton.Size = new System.Drawing.Size(75, 23);
            this.saveUserButton.TabIndex = 12;
            this.saveUserButton.Text = "Save";
            this.saveUserButton.UseVisualStyleBackColor = true;
            this.saveUserButton.Click += new System.EventHandler(this.saveUserButton_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 197F));
            this.tableLayoutPanel2.Controls.Add(this.lockPanel, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.firstNameLabel, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.enableLabel, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.passwordLabel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.usernameTextBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.usernameLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.passwordTextBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.firstNameTextBox, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.enablePanel, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.resetPasswordButton, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.lockLabel, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.lastNameLabel, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.lastNameTextBox, 3, 2);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.groupComboBox, 1, 3);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 47);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(551, 215);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // lockPanel
            // 
            this.lockPanel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lockPanel.Controls.Add(this.lockFalseRadioButton);
            this.lockPanel.Controls.Add(this.lockTrueRadioButton);
            this.lockPanel.Location = new System.Drawing.Point(100, 181);
            this.lockPanel.Name = "lockPanel";
            this.lockPanel.Size = new System.Drawing.Size(144, 29);
            this.lockPanel.TabIndex = 11;
            // 
            // lockFalseRadioButton
            // 
            this.lockFalseRadioButton.AutoSize = true;
            this.lockFalseRadioButton.Location = new System.Drawing.Point(71, 6);
            this.lockFalseRadioButton.Name = "lockFalseRadioButton";
            this.lockFalseRadioButton.Size = new System.Drawing.Size(46, 22);
            this.lockFalseRadioButton.TabIndex = 1;
            this.lockFalseRadioButton.TabStop = true;
            this.lockFalseRadioButton.Text = "No";
            this.lockFalseRadioButton.UseVisualStyleBackColor = true;
            // 
            // lockTrueRadioButton
            // 
            this.lockTrueRadioButton.AutoSize = true;
            this.lockTrueRadioButton.Location = new System.Drawing.Point(3, 6);
            this.lockTrueRadioButton.Name = "lockTrueRadioButton";
            this.lockTrueRadioButton.Size = new System.Drawing.Size(51, 22);
            this.lockTrueRadioButton.TabIndex = 0;
            this.lockTrueRadioButton.TabStop = true;
            this.lockTrueRadioButton.Text = "Yes";
            this.lockTrueRadioButton.UseVisualStyleBackColor = true;
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.Location = new System.Drawing.Point(3, 78);
            this.firstNameLabel.Name = "firstNameLabel";
            this.firstNameLabel.Size = new System.Drawing.Size(81, 18);
            this.firstNameLabel.TabIndex = 1;
            this.firstNameLabel.Text = "First Name";
            // 
            // enableLabel
            // 
            this.enableLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.enableLabel.AutoSize = true;
            this.enableLabel.Location = new System.Drawing.Point(3, 149);
            this.enableLabel.Name = "enableLabel";
            this.enableLabel.Size = new System.Drawing.Size(53, 18);
            this.enableLabel.TabIndex = 2;
            this.enableLabel.Text = "Enable";
            // 
            // passwordLabel
            // 
            this.passwordLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(3, 43);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(75, 18);
            this.passwordLabel.TabIndex = 4;
            this.passwordLabel.Text = "Password";
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.usernameTextBox.Location = new System.Drawing.Point(100, 5);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(144, 24);
            this.usernameTextBox.TabIndex = 5;
            // 
            // usernameLabel
            // 
            this.usernameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(3, 8);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(77, 18);
            this.usernameLabel.TabIndex = 0;
            this.usernameLabel.Text = "Username";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.passwordTextBox.Location = new System.Drawing.Point(100, 40);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(144, 24);
            this.passwordTextBox.TabIndex = 6;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // firstNameTextBox
            // 
            this.firstNameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.firstNameTextBox.Location = new System.Drawing.Point(100, 75);
            this.firstNameTextBox.Name = "firstNameTextBox";
            this.firstNameTextBox.Size = new System.Drawing.Size(144, 24);
            this.firstNameTextBox.TabIndex = 8;
            // 
            // enablePanel
            // 
            this.enablePanel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.enablePanel.Controls.Add(this.enableFalseRadioButton);
            this.enablePanel.Controls.Add(this.enableTrueRadioButton);
            this.enablePanel.Location = new System.Drawing.Point(100, 144);
            this.enablePanel.Name = "enablePanel";
            this.enablePanel.Size = new System.Drawing.Size(144, 29);
            this.enablePanel.TabIndex = 10;
            // 
            // enableFalseRadioButton
            // 
            this.enableFalseRadioButton.AutoSize = true;
            this.enableFalseRadioButton.Location = new System.Drawing.Point(71, 6);
            this.enableFalseRadioButton.Name = "enableFalseRadioButton";
            this.enableFalseRadioButton.Size = new System.Drawing.Size(46, 22);
            this.enableFalseRadioButton.TabIndex = 1;
            this.enableFalseRadioButton.TabStop = true;
            this.enableFalseRadioButton.Text = "No";
            this.enableFalseRadioButton.UseVisualStyleBackColor = true;
            // 
            // enableTrueRadioButton
            // 
            this.enableTrueRadioButton.AutoSize = true;
            this.enableTrueRadioButton.Location = new System.Drawing.Point(3, 6);
            this.enableTrueRadioButton.Name = "enableTrueRadioButton";
            this.enableTrueRadioButton.Size = new System.Drawing.Size(51, 22);
            this.enableTrueRadioButton.TabIndex = 0;
            this.enableTrueRadioButton.TabStop = true;
            this.enableTrueRadioButton.Text = "Yes";
            this.enableTrueRadioButton.UseVisualStyleBackColor = true;
            // 
            // resetPasswordButton
            // 
            this.resetPasswordButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.resetPasswordButton.Location = new System.Drawing.Point(265, 41);
            this.resetPasswordButton.Name = "resetPasswordButton";
            this.resetPasswordButton.Size = new System.Drawing.Size(64, 23);
            this.resetPasswordButton.TabIndex = 7;
            this.resetPasswordButton.Text = "Reset";
            this.resetPasswordButton.UseVisualStyleBackColor = true;
            this.resetPasswordButton.Click += new System.EventHandler(this.resetPasswordButton_Click);
            // 
            // lockLabel
            // 
            this.lockLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lockLabel.AutoSize = true;
            this.lockLabel.Location = new System.Drawing.Point(3, 187);
            this.lockLabel.Name = "lockLabel";
            this.lockLabel.Size = new System.Drawing.Size(41, 18);
            this.lockLabel.TabIndex = 10;
            this.lockLabel.Text = "Lock";
            // 
            // lastNameLabel
            // 
            this.lastNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lastNameLabel.AutoSize = true;
            this.lastNameLabel.Location = new System.Drawing.Point(265, 78);
            this.lastNameLabel.Name = "lastNameLabel";
            this.lastNameLabel.Size = new System.Drawing.Size(80, 18);
            this.lastNameLabel.TabIndex = 11;
            this.lastNameLabel.Text = "Last Name";
            // 
            // lastNameTextBox
            // 
            this.lastNameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lastNameTextBox.Location = new System.Drawing.Point(358, 75);
            this.lastNameTextBox.Name = "lastNameTextBox";
            this.lastNameTextBox.Size = new System.Drawing.Size(144, 24);
            this.lastNameTextBox.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 18);
            this.label1.TabIndex = 12;
            this.label1.Text = "Group";
            // 
            // groupComboBox
            // 
            this.groupComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.groupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupComboBox.FormattingEnabled = true;
            this.groupComboBox.Location = new System.Drawing.Point(100, 112);
            this.groupComboBox.Name = "groupComboBox";
            this.groupComboBox.Size = new System.Drawing.Size(144, 26);
            this.groupComboBox.TabIndex = 13;
            // 
            // UserManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 594);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UserManagementForm";
            this.ShowIcon = false;
            this.Text = "UserManagement";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userDataGridView)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.lockPanel.ResumeLayout(false);
            this.lockPanel.PerformLayout();
            this.enablePanel.ResumeLayout(false);
            this.enablePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView userDataGridView;
        private System.Windows.Forms.Button searchUserButton;
        private System.Windows.Forms.TextBox searchUserTextBox;
        private System.Windows.Forms.Label searchUserLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label firstNameLabel;
        private System.Windows.Forms.Label enableLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox firstNameTextBox;
        private System.Windows.Forms.Button cancelUserButton;
        private System.Windows.Forms.Button saveUserButton;
        private System.Windows.Forms.Panel lockPanel;
        private System.Windows.Forms.RadioButton lockFalseRadioButton;
        private System.Windows.Forms.RadioButton lockTrueRadioButton;
        private System.Windows.Forms.Panel enablePanel;
        private System.Windows.Forms.RadioButton enableFalseRadioButton;
        private System.Windows.Forms.RadioButton enableTrueRadioButton;
        private System.Windows.Forms.Button resetPasswordButton;
        private System.Windows.Forms.Label lockLabel;
        private System.Windows.Forms.Label lastNameLabel;
        private System.Windows.Forms.TextBox lastNameTextBox;
        private System.Windows.Forms.Button deleteUserButton;
        private System.Windows.Forms.Button addUserButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox groupComboBox;
    }
}