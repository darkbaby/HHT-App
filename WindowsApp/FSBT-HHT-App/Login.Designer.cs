using System.Reflection;
namespace FSBT.HHT.App.UI
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.labelProgramName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdRealtime = new System.Windows.Forms.RadioButton();
            this.rdNoneRealtime = new System.Windows.Forms.RadioButton();
            this.btnLogin = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textUsername = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogin)).BeginInit();
            this.SuspendLayout();
            // 
            // labelProgramName
            // 
            this.labelProgramName.AutoSize = true;
            this.labelProgramName.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProgramName.ForeColor = System.Drawing.Color.DarkRed;
            this.labelProgramName.Location = new System.Drawing.Point(23, 9);
            this.labelProgramName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProgramName.Name = "labelProgramName";
            this.labelProgramName.Size = new System.Drawing.Size(697, 58);
            this.labelProgramName.TabIndex = 2;
            this.labelProgramName.Text = "Stocktaking System v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdRealtime);
            this.groupBox1.Controls.Add(this.rdNoneRealtime);
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textPassword);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textUsername);
            this.groupBox1.Location = new System.Drawing.Point(95, 113);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(625, 361);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // rdRealtime
            // 
            this.rdRealtime.AutoSize = true;
            this.rdRealtime.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.rdRealtime.Location = new System.Drawing.Point(409, 174);
            this.rdRealtime.Margin = new System.Windows.Forms.Padding(4);
            this.rdRealtime.Name = "rdRealtime";
            this.rdRealtime.Size = new System.Drawing.Size(151, 40);
            this.rdRealtime.TabIndex = 11;
            this.rdRealtime.TabStop = true;
            this.rdRealtime.Text = "Realtime";
            this.rdRealtime.UseVisualStyleBackColor = true;
            this.rdRealtime.Visible = false;
            // 
            // rdNoneRealtime
            // 
            this.rdNoneRealtime.AutoSize = true;
            this.rdNoneRealtime.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.rdNoneRealtime.Location = new System.Drawing.Point(27, 174);
            this.rdNoneRealtime.Margin = new System.Windows.Forms.Padding(4);
            this.rdNoneRealtime.Name = "rdNoneRealtime";
            this.rdNoneRealtime.Size = new System.Drawing.Size(217, 40);
            this.rdNoneRealtime.TabIndex = 10;
            this.rdNoneRealtime.TabStop = true;
            this.rdNoneRealtime.Text = "Non-Realtime";
            this.rdNoneRealtime.UseVisualStyleBackColor = true;
            this.rdNoneRealtime.Visible = false;
            // 
            // btnLogin
            // 
            this.btnLogin.Image = ((System.Drawing.Image)(resources.GetObject("btnLogin.Image")));
            this.btnLogin.Location = new System.Drawing.Point(212, 222);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(219, 64);
            this.btnLogin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnLogin.TabIndex = 7;
            this.btnLogin.TabStop = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.label4.Location = new System.Drawing.Point(20, 134);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 36);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password";
            // 
            // textPassword
            // 
            this.textPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.textPassword.Location = new System.Drawing.Point(212, 130);
            this.textPassword.Margin = new System.Windows.Forms.Padding(4);
            this.textPassword.MaxLength = 20;
            this.textPassword.Name = "textPassword";
            this.textPassword.PasswordChar = '•';
            this.textPassword.Size = new System.Drawing.Size(365, 41);
            this.textPassword.TabIndex = 5;
            this.textPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textPassword_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.label3.Location = new System.Drawing.Point(20, 73);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 36);
            this.label3.TabIndex = 4;
            this.label3.Text = "Username";
            // 
            // textUsername
            // 
            this.textUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.textUsername.Location = new System.Drawing.Point(212, 69);
            this.textUsername.Margin = new System.Windows.Forms.Padding(4);
            this.textUsername.MaxLength = 20;
            this.textUsername.Name = "textUsername";
            this.textUsername.Size = new System.Drawing.Size(365, 41);
            this.textUsername.TabIndex = 0;
            this.textUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textUsername_KeyDown);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(832, 544);
            this.Controls.Add(this.labelProgramName);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Login_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox btnLogin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textUsername;
        private System.Windows.Forms.Label labelProgramName;
        private System.Windows.Forms.RadioButton rdRealtime;
        private System.Windows.Forms.RadioButton rdNoneRealtime;
    }
}
