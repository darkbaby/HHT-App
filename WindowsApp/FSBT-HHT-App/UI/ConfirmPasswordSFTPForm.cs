using FSBT.HHT.App.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSBT.HHT.App.UI
{
    public partial class ConfirmPasswordSFTPForm : Form
    {
        public string password { set; get; }

        public ConfirmPasswordSFTPForm()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textboxPassword.Text))
            {
                MessageBox.Show(MessageConstants.PleaseenterPassword, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                password = textboxPassword.Text;
            }
        }

        private void ConfirmPasswordSFTPForm_Load(object sender, EventArgs e)
        {
            textboxPassword.Select();
        }
    }
}
