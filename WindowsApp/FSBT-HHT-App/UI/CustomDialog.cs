using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSBT.HHT.App.Resources;

namespace FSBT.HHT.App.UI
{
    public partial class CustomDialog : Form
    {
        public CustomDialog()
        {
            InitializeComponent();
        }

        private void CustomDialog_Load(object sender, EventArgs e)
        {
            string message = MessageConstants.Recentlyuploadeddatahasduplicatewithpreviousuploadeddata;
            label2.Text = message.Replace("@", System.Environment.NewLine);
        }

    }
}
