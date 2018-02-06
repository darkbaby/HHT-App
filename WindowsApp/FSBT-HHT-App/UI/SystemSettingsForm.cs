using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSBT_HHT_Model;
using FSBT_HHT_BLL;
using FSBT.HHT.App.Resources;
using System.Globalization;

namespace FSBT.HHT.App.UI
{
    public partial class SettingsForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        SystemSettingBll settingBLL = new SystemSettingBll();
        SystemSettingModel displayData = new SystemSettingModel();
        private LocationManagementBll bllLocal = new LocationManagementBll();

        public SettingsForm()
        {
            InitializeComponent();
            displayData = settingBLL.GetSettingData();          
            DisplayData(displayData);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                    GetSettingDataFromSettingPanel();
                    string result = settingBLL.UpdateSetting(displayData);
                    if (result == "success")
                    {
                        MessageBox.Show(MessageConstants.Dataupdated, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.cannotsaveinformationtodatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    //do nothing
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DisplayData(SystemSettingModel dataForDisplay)
        {
            maxLoginFailTextBox.Text = dataForDisplay.MaxLoginFail.ToString();
            comIDTextBox.Text = dataForDisplay.ComID;
            comNameTextBox.Text = dataForDisplay.ComName;
            countDateDateTimePicker.Value = dataForDisplay.CountDate;
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            branch.Text = dataForDisplay.Branch;
            string[] sectionType = dataForDisplay.SectionType.Split('|');
            foreach (var sec in sectionType)
            {
                if (sec.Equals("1")) { chFront.Checked = true; }
                if (sec.Equals("2")) { chBack.Checked = true; }
                if (sec.Equals("3")) { chWareHouse.Checked = true; }
                if (sec.Equals("4")) { chFreshFood.Checked = true; }
            }
        }

        private void GetSettingDataFromSettingPanel()
        {
            string sectionType = "";
            displayData.MaxLoginFail = Int32.Parse(maxLoginFailTextBox.Text);
            displayData.ComID = comIDTextBox.Text;
            displayData.ComName = comNameTextBox.Text;
            displayData.CountDate = countDateDateTimePicker.Value;
            displayData.Branch = branch.Text;

            if (chFront.Checked)
            {
                if (sectionType == "")
                {
                    sectionType = (bllLocal.GetSectionTypeIDBySectionTypeName(chFront.Text)).ToString();
                }
                else
                {
                    sectionType += "|"+ (bllLocal.GetSectionTypeIDBySectionTypeName(chFront.Text)).ToString();
                }
            }
            if (chBack.Checked)
            {
                if (sectionType == "")
                {
                    sectionType = (bllLocal.GetSectionTypeIDBySectionTypeName(chBack.Text)).ToString();
                }
                else
                {
                    sectionType += "|" + (bllLocal.GetSectionTypeIDBySectionTypeName(chBack.Text)).ToString();
                }
            }
            if (chWareHouse.Checked)
            {
                if (sectionType == "")
                {
                    sectionType = (bllLocal.GetSectionTypeIDBySectionTypeName(chWareHouse.Text)).ToString();
                }
                else
                {
                    sectionType += "|" + (bllLocal.GetSectionTypeIDBySectionTypeName(chWareHouse.Text)).ToString();
                }
            }
            if (chFreshFood.Checked)
            {
                if (sectionType == "")
                {
                    sectionType = (bllLocal.GetSectionTypeIDBySectionTypeName(chFreshFood.Text)).ToString();
                }
                else
                {
                    sectionType += "|" + (bllLocal.GetSectionTypeIDBySectionTypeName(chFreshFood.Text)).ToString();
                }
            }

            displayData.SectionType = sectionType;
        }

        private bool ValidateData()
        {
            if (ValidateMaxLoginFail() && ValidateComID() && ValidateComName() && ValidateCountDate()
                && ValidateBranch() && ValidateSectionType())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidateMaxLoginFail()
        {
            string data = maxLoginFailTextBox.Text;
            int numberData;
            if (data.Length == 0 || data.Equals(null))
            {
                MessageBox.Show(MessageConstants.MaxLoginFailcannotbeblank, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!Int32.TryParse(maxLoginFailTextBox.Text, out numberData))
            {
                MessageBox.Show(MessageConstants.MaxLoginFailmustbenumberonly, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                if (numberData < 1)
                {
                    MessageBox.Show(MessageConstants.MaxLoginFailcannotbelessthan1, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private bool ValidateComID()
        {
            string data = comIDTextBox.Text;
            if (data.Length == 0 || data.Equals(null))
            {
                MessageBox.Show(MessageConstants.ComputerIDcannotbeblank, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (data.Length > 20)
            {
                MessageBox.Show(MessageConstants.ComputerIDistoolong, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidateComName()
        {
            string data = comNameTextBox.Text;
            if (data.Length == 0 || data.Equals(null))
            {
                MessageBox.Show(MessageConstants.ComputerNamecannotbeblank, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (data.Length > 50)
            {
                MessageBox.Show(MessageConstants.ComputerNameistoolong, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidateCountDate()
        {
            string data = countDateDateTimePicker.Value.ToString();
            int dataYear = Int32.Parse(countDateDateTimePicker.Value.ToString("yyyy"));
            if (data.Length == 0 || data.Equals(null))
            {
                MessageBox.Show(MessageConstants.CountDatecannotbeblank, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (dataYear > 2099)
            {
                MessageBox.Show(MessageConstants.CountDatecannotmorethan2099, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidateBranch()
        {
            if (string.IsNullOrWhiteSpace(branch.Text))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidateSectionType()
        {
            if (chFront.Checked || chBack.Checked || chWareHouse.Checked || chFreshFood.Checked)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
