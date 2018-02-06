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
    public partial class MessageBoxAutoHHTSyncForm : Form
    {
        public MessageBoxAutoHHTSyncForm(string msg)
        {
            InitializeComponent();
            SetMessage(msg);
            
        }
        private void SetMessage(string msg)
        {
            label1.Text = msg.Replace("@", System.Environment.NewLine);
        }
    }
}
