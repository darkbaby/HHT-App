using FSBT_HHT_BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FSBT.HHT.App.UI
{
    public partial class Login : FSBT.HHT.App.Layout
    {
        Authentication authen = new Authentication();
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = textUsername.Text;
            string password = textPassword.Text;

            if (username.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Username/Password ห้ามเป็นค่าว่าง");
            }
            else
            {
                if (username.Length < 4 || password.Length < 4)
                {
                    MessageBox.Show("Username/Password อย่างน้อย 4 ตัวอักษร");
                }
                else
                {
                    string checkLoginResult = authen.CheckLogin(username, password);

                    if ("Success".Equals(checkLoginResult))
                    {
                        //this.Close();
                        LayoutWmenu baseScreen = new LayoutWmenu();               
                        baseScreen.Show();
                        Hide();  
                    }
                    else
                    {
                        textPassword.Text = string.Empty;
                        MessageBox.Show(checkLoginResult);
                        
                    }                    
                    
                }
            }
            
        }

    }
}
