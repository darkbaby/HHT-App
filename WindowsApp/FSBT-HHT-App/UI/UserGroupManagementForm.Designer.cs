namespace FSBT.HHT.App.UI
{
    partial class UserGroupManagementForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgdUserGroup = new System.Windows.Forms.DataGridView();
            this.GroupID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpdateBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.userTabPage = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.dgdUser = new System.Windows.Forms.DataGridView();
            this.memberUserCheckBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.firstname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.screenTabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.dgdScreen = new System.Windows.Forms.DataGridView();
            this.screenVisibleCheckBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.screenName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.screenID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgdComponent = new System.Windows.Forms.DataGridView();
            this.objectAlias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.objectType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.add = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.edit = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.delete = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.visible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.objectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgdUserGroup)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.userTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgdUser)).BeginInit();
            this.screenTabPage.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgdScreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgdComponent)).BeginInit();
            this.SuspendLayout();
            // 
            // dgdUserGroup
            // 
            this.dgdUserGroup.AllowUserToAddRows = false;
            this.dgdUserGroup.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgdUserGroup.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgdUserGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgdUserGroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GroupID,
            this.GroupName,
            this.LastUpdate,
            this.UpdateBy});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgdUserGroup.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgdUserGroup.Location = new System.Drawing.Point(3, 33);
            this.dgdUserGroup.Name = "dgdUserGroup";
            this.dgdUserGroup.ReadOnly = true;
            this.dgdUserGroup.Size = new System.Drawing.Size(700, 99);
            this.dgdUserGroup.TabIndex = 1;
            this.dgdUserGroup.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgdUserGroup_CellBeginEdit);
            this.dgdUserGroup.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgdUserGroup_CellEndEdit);
            this.dgdUserGroup.SelectionChanged += new System.EventHandler(this.dgdUserGroup_SelectionChanged);
            // 
            // GroupID
            // 
            this.GroupID.HeaderText = "Group ID";
            this.GroupID.Name = "GroupID";
            this.GroupID.ReadOnly = true;
            this.GroupID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.GroupID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // GroupName
            // 
            this.GroupName.FillWeight = 300F;
            this.GroupName.HeaderText = "Group Name";
            this.GroupName.MaxInputLength = 50;
            this.GroupName.MinimumWidth = 284;
            this.GroupName.Name = "GroupName";
            this.GroupName.ReadOnly = true;
            this.GroupName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.GroupName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.GroupName.Width = 284;
            // 
            // LastUpdate
            // 
            this.LastUpdate.HeaderText = "Last Update";
            this.LastUpdate.Name = "LastUpdate";
            this.LastUpdate.ReadOnly = true;
            this.LastUpdate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LastUpdate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LastUpdate.Width = 170;
            // 
            // UpdateBy
            // 
            this.UpdateBy.HeaderText = "Update By";
            this.UpdateBy.Name = "UpdateBy";
            this.UpdateBy.ReadOnly = true;
            this.UpdateBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgdUserGroup, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(733, 142);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "User Group";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(12, 160);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 370F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(984, 370);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(978, 364);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.userTabPage);
            this.tabControl1.Controls.Add(this.screenTabPage);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(4, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(970, 358);
            this.tabControl1.TabIndex = 0;
            // 
            // userTabPage
            // 
            this.userTabPage.Controls.Add(this.label3);
            this.userTabPage.Controls.Add(this.dgdUser);
            this.userTabPage.Controls.Add(this.textBox2);
            this.userTabPage.Location = new System.Drawing.Point(4, 27);
            this.userTabPage.Name = "userTabPage";
            this.userTabPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.userTabPage.Size = new System.Drawing.Size(962, 327);
            this.userTabPage.TabIndex = 0;
            this.userTabPage.Text = "User";
            this.userTabPage.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "Search User";
            // 
            // dgdUser
            // 
            this.dgdUser.AllowUserToAddRows = false;
            this.dgdUser.AllowUserToDeleteRows = false;
            this.dgdUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgdUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgdUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.memberUserCheckBox,
            this.username,
            this.firstname,
            this.lastname,
            this.addBy,
            this.addDate});
            this.dgdUser.Location = new System.Drawing.Point(3, 32);
            this.dgdUser.Name = "dgdUser";
            this.dgdUser.ReadOnly = true;
            this.dgdUser.Size = new System.Drawing.Size(954, 290);
            this.dgdUser.TabIndex = 0;
            // 
            // memberUserCheckBox
            // 
            this.memberUserCheckBox.FalseValue = "false";
            this.memberUserCheckBox.HeaderText = "Member";
            this.memberUserCheckBox.Name = "memberUserCheckBox";
            this.memberUserCheckBox.ReadOnly = true;
            this.memberUserCheckBox.TrueValue = "true";
            // 
            // username
            // 
            this.username.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.username.HeaderText = "UserName";
            this.username.Name = "username";
            this.username.ReadOnly = true;
            // 
            // firstname
            // 
            this.firstname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.firstname.HeaderText = "First Name";
            this.firstname.Name = "firstname";
            this.firstname.ReadOnly = true;
            // 
            // lastname
            // 
            this.lastname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lastname.HeaderText = "Last Name";
            this.lastname.Name = "lastname";
            this.lastname.ReadOnly = true;
            // 
            // addBy
            // 
            this.addBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.addBy.HeaderText = "Added By";
            this.addBy.Name = "addBy";
            this.addBy.ReadOnly = true;
            // 
            // addDate
            // 
            this.addDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.addDate.HeaderText = "Added Date";
            this.addDate.Name = "addDate";
            this.addDate.ReadOnly = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(109, 5);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(284, 24);
            this.textBox2.TabIndex = 3;
            this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            // 
            // screenTabPage
            // 
            this.screenTabPage.Controls.Add(this.tableLayoutPanel3);
            this.screenTabPage.Controls.Add(this.label2);
            this.screenTabPage.Controls.Add(this.textBox1);
            this.screenTabPage.Location = new System.Drawing.Point(4, 27);
            this.screenTabPage.Name = "screenTabPage";
            this.screenTabPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.screenTabPage.Size = new System.Drawing.Size(962, 327);
            this.screenTabPage.TabIndex = 1;
            this.screenTabPage.Text = "Screen";
            this.screenTabPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel3.Controls.Add(this.dgdScreen, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.dgdComponent, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 30);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(950, 292);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // dgdScreen
            // 
            this.dgdScreen.AllowUserToAddRows = false;
            this.dgdScreen.AllowUserToDeleteRows = false;
            this.dgdScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgdScreen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgdScreen.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.screenVisibleCheckBox,
            this.screenName,
            this.screenID});
            this.dgdScreen.Location = new System.Drawing.Point(3, 3);
            this.dgdScreen.Name = "dgdScreen";
            this.dgdScreen.ReadOnly = true;
            this.dgdScreen.Size = new System.Drawing.Size(374, 286);
            this.dgdScreen.TabIndex = 0;
            this.dgdScreen.SelectionChanged += new System.EventHandler(this.dgdScreen_SelectionChanged);
            // 
            // screenVisibleCheckBox
            // 
            this.screenVisibleCheckBox.FalseValue = "false";
            this.screenVisibleCheckBox.HeaderText = "Visible";
            this.screenVisibleCheckBox.Name = "screenVisibleCheckBox";
            this.screenVisibleCheckBox.ReadOnly = true;
            this.screenVisibleCheckBox.TrueValue = "true";
            this.screenVisibleCheckBox.Width = 60;
            // 
            // screenName
            // 
            this.screenName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.screenName.FillWeight = 98.08917F;
            this.screenName.HeaderText = "Screen Name";
            this.screenName.Name = "screenName";
            this.screenName.ReadOnly = true;
            this.screenName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // screenID
            // 
            this.screenID.HeaderText = "Screen ID";
            this.screenID.Name = "screenID";
            this.screenID.ReadOnly = true;
            this.screenID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.screenID.Visible = false;
            // 
            // dgdComponent
            // 
            this.dgdComponent.AllowUserToAddRows = false;
            this.dgdComponent.AllowUserToDeleteRows = false;
            this.dgdComponent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgdComponent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgdComponent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.objectAlias,
            this.objectType,
            this.add,
            this.edit,
            this.delete,
            this.enable,
            this.visible,
            this.objectName});
            this.dgdComponent.Location = new System.Drawing.Point(383, 3);
            this.dgdComponent.Name = "dgdComponent";
            this.dgdComponent.ReadOnly = true;
            this.dgdComponent.Size = new System.Drawing.Size(564, 286);
            this.dgdComponent.TabIndex = 1;
            // 
            // objectAlias
            // 
            this.objectAlias.HeaderText = "Object";
            this.objectAlias.Name = "objectAlias";
            this.objectAlias.ReadOnly = true;
            this.objectAlias.Width = 150;
            // 
            // objectType
            // 
            this.objectType.HeaderText = "Object Type";
            this.objectType.Name = "objectType";
            this.objectType.ReadOnly = true;
            this.objectType.Width = 130;
            // 
            // add
            // 
            this.add.FalseValue = "false";
            this.add.HeaderText = "Add";
            this.add.Name = "add";
            this.add.ReadOnly = true;
            this.add.TrueValue = "true";
            this.add.Visible = false;
            this.add.Width = 50;
            // 
            // edit
            // 
            this.edit.FalseValue = "false";
            this.edit.HeaderText = "Edit";
            this.edit.Name = "edit";
            this.edit.ReadOnly = true;
            this.edit.TrueValue = "true";
            this.edit.Visible = false;
            this.edit.Width = 50;
            // 
            // delete
            // 
            this.delete.FalseValue = "false";
            this.delete.HeaderText = "Delete";
            this.delete.Name = "delete";
            this.delete.ReadOnly = true;
            this.delete.TrueValue = "true";
            this.delete.Visible = false;
            this.delete.Width = 50;
            // 
            // enable
            // 
            this.enable.FalseValue = "false";
            this.enable.HeaderText = "Enable";
            this.enable.Name = "enable";
            this.enable.ReadOnly = true;
            this.enable.TrueValue = "true";
            this.enable.Width = 70;
            // 
            // visible
            // 
            this.visible.FalseValue = "false";
            this.visible.HeaderText = "Visible";
            this.visible.Name = "visible";
            this.visible.ReadOnly = true;
            this.visible.TrueValue = "true";
            this.visible.Width = 70;
            // 
            // objectName
            // 
            this.objectName.HeaderText = "Object Name";
            this.objectName.Name = "objectName";
            this.objectName.ReadOnly = true;
            this.objectName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.objectName.Visible = false;
            this.objectName.Width = 90;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "Search Screen";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(123, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(284, 24);
            this.textBox1.TabIndex = 7;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnSave.Location = new System.Drawing.Point(802, 45);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(91, 49);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.Location = new System.Drawing.Point(899, 45);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(91, 49);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // UserGroupManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1008, 542);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UserGroupManagementForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UserGroupManagementForm";
            this.Activated += new System.EventHandler(this.UserGroupManagementForm_Activated);
            this.Load += new System.EventHandler(this.UserGroupManagementForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgdUserGroup)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.userTabPage.ResumeLayout(false);
            this.userTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgdUser)).EndInit();
            this.screenTabPage.ResumeLayout(false);
            this.screenTabPage.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgdScreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgdComponent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgdUserGroup;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage userTabPage;
        private System.Windows.Forms.TabPage screenTabPage;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.DataGridView dgdUser;
        private System.Windows.Forms.DataGridView dgdScreen;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridView dgdComponent;
        private System.Windows.Forms.DataGridViewCheckBoxColumn memberUserCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn username;
        private System.Windows.Forms.DataGridViewTextBoxColumn firstname;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastname;
        private System.Windows.Forms.DataGridViewTextBoxColumn addBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn addDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn screenID;
        private System.Windows.Forms.DataGridViewTextBoxColumn screenName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn screenVisibleCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn objectName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn visible;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enable;
        private System.Windows.Forms.DataGridViewCheckBoxColumn delete;
        private System.Windows.Forms.DataGridViewCheckBoxColumn edit;
        private System.Windows.Forms.DataGridViewCheckBoxColumn add;
        private System.Windows.Forms.DataGridViewTextBoxColumn objectType;
        private System.Windows.Forms.DataGridViewTextBoxColumn objectAlias;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpdateBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupID;
    }
}