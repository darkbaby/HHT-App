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
using System.Reflection;

namespace FSBT.HHT.App.UI
{
    public partial class SettingsForm : Form
    {
        private LogErrorBll logBll = new LogErrorBll(); 
        SystemSettingBll settingBLL = new SystemSettingBll();
        SystemSettingModel displayData = new SystemSettingModel();
        private LocationManagementBll bllLocal = new LocationManagementBll();
        public string UserName { get; set; }
        public SettingsForm()
        {
            InitializeComponent();
        }

        public SettingsForm(string username)
        {
            InitializeComponent();
            dataGridViewPlant.UserDeletingRow += dataGridViewPlant_UserDeletingRow;
            displayData = settingBLL.GetSettingData();
            UserName = username;
            displayData.UpdateBy = UserName;
            DisplayData(displayData);
        }

        #region Events

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                    Loading_Screen.ShowSplashScreen();
                    GetSettingDataFromSettingPanel();
                    string result = settingBLL.UpdateSetting(displayData);
                    Loading_Screen.CloseForm();
                    if (result.ToLower() == "success")
                    {
                        MessageBox.Show(MessageConstants.Dataupdated, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Loading_Screen.ShowSplashScreen();
                        displayData = settingBLL.GetSettingData();
                        displayData.UpdateBy = UserName;
                        DisplayData(displayData);
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.cannotsaveinformationtodatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Loading_Screen.ShowSplashScreen();
                        displayData = settingBLL.GetSettingData();
                        displayData.UpdateBy = UserName;
                        DisplayData(displayData);
                    }
                    Loading_Screen.CloseForm();
                }
                else
                {
                    //do nothing
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            //this.Close();
            Loading_Screen.ShowSplashScreen();
            displayData = settingBLL.GetSettingData();
            displayData.UpdateBy = UserName;
            DisplayData(displayData);
            Loading_Screen.CloseForm();
        }

        private void chEditMCH_CheckedChanged(object sender, EventArgs e)
        {
            if (chEditMCH.Checked)
            {
                mchLevel1TextBox.Enabled = true;
                mchLevel2TextBox.Enabled = true;
                mchLevel3TextBox.Enabled = true;
                mchLevel4TextBox.Enabled = true;
            }
            else
            {
                //mchLevel1TextBox.Text = "";
                //mchLevel2TextBox.Text = "";
                //mchLevel3TextBox.Text = "";
                //mchLevel4TextBox.Text = "";

                mchLevel1TextBox.Enabled = false;
                mchLevel2TextBox.Enabled = false;
                mchLevel3TextBox.Enabled = false;
                mchLevel4TextBox.Enabled = false;
            }
        }

        private void dataGridViewPlant_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            //DialogResult usersChoice =
            //MessageBox.Show("Are you sure you want to delete the selected signs?\r\n" + dataGridViewPlant.SelectedRows.Count + " signs will be deleted!", "Signs", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            //// cancel the delete event
            //if (usersChoice == DialogResult.Cancel)
            //    e.Cancel = true;
        }

        #endregion

        private void DisplayData(SystemSettingModel dataForDisplay)
        {
            maxLoginFailTextBox.Text = dataForDisplay.MaxLoginFail.ToString();
            comIDTextBox.Text = dataForDisplay.ComID;
            comNameTextBox.Text = dataForDisplay.ComName;
            countDateDateTimePicker.Value = dataForDisplay.CountDate;
            //System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            newCulture.DateTimeFormat.LongDatePattern = "yyyy/MM/dd HH:mm:ss";
            newCulture.DateTimeFormat.DateSeparator = "/";
            System.Threading.Thread.CurrentThread.CurrentCulture = newCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = newCulture;

            txtPlant.Text = dataForDisplay.Plant;

            if (displayData.IsEditMchSettings)
            {
                chEditMCH.Checked = false;
                mchLevel1TextBox.Text = displayData.MCHLevel1;
                mchLevel2TextBox.Text = displayData.MCHLevel2;
                mchLevel3TextBox.Text = displayData.MCHLevel3;
                mchLevel4TextBox.Text = displayData.MCHLevel4;
            }
            else {
                chEditMCH.Checked = true;
            }

            if (displayData.listPlant != null)
            {
                dataGridViewPlant.Rows.Clear();
                foreach (MasterPlantModel plant in displayData.listPlant)
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridViewPlant.Rows[0].Clone();
                    row.Cells[0].Value = (string)plant.PlantCode;
                    row.Cells[1].Value = (string)plant.PlantDescription;
                    row.Cells[2].Value = (string)plant.Mode;
                    dataGridViewPlant.Rows.Add(row);
                }
            }
            else
            {
                dataGridViewPlant.Rows.Clear();
            }

            if (displayData.ScanMode == "P")
            {
                rbProduct.Checked = true;
                rbFreshfood.Checked = false;
            }
            else
            {
                rbProduct.Checked = false;
                rbFreshfood.Checked = true;
            }

        }

        private void GetSettingDataFromSettingPanel()
        {
            displayData.MaxLoginFail = Int32.Parse(maxLoginFailTextBox.Text);
            displayData.ComID = comIDTextBox.Text;
            displayData.ComName = comNameTextBox.Text;
            displayData.CountDate = countDateDateTimePicker.Value;
            //displayData.Branch = branch.Text;
            //displayData.StoreID = txtStoreID.Text;
            displayData.Plant = txtPlant.Text;

            if (chEditMCH.Checked)
            {
                displayData.IsEditMchSettings = true;
                displayData.MCHLevel1 = mchLevel1TextBox.Text;
                displayData.MCHLevel2 = mchLevel2TextBox.Text;
                displayData.MCHLevel3 = mchLevel3TextBox.Text;
                displayData.MCHLevel4 = mchLevel4TextBox.Text;
            }
            else {
                displayData.IsEditMchSettings = false;
                //displayData.MCHLevel1 = "";
                //displayData.MCHLevel2 = "";
                //displayData.MCHLevel3 = "";
                //displayData.MCHLevel4 = "";
            }

            //เช็ค insert ก่อนว่าอันไหนไม่มี ใน model บ้าง
            List<MasterPlantModel> listPlantInsert = new List<MasterPlantModel>();
            List<MasterPlantModel> listPlantDelete = new List<MasterPlantModel>();
            List<MasterPlantModel> listPlantUpdate = new List<MasterPlantModel>();
            List<MasterPlantModel> listPlantGrid = new List<MasterPlantModel>();

            for (int count = 0; count < dataGridViewPlant.Rows.Count - 1; count++)
            {
                MasterPlantModel plant = new MasterPlantModel();
                plant.PlantCode = (string)dataGridViewPlant.Rows[count].Cells[0].Value == null ? "" : ((string)dataGridViewPlant.Rows[count].Cells[0].Value).Trim().ToUpper();
                plant.PlantDescription = (string)dataGridViewPlant.Rows[count].Cells[1].Value == null ? "" : ((string)dataGridViewPlant.Rows[count].Cells[1].Value).Trim();
                plant.Mode = (string)dataGridViewPlant.Rows[count].Cells[2].Value == null ? "" : ((string)dataGridViewPlant.Rows[count].Cells[2].Value).Trim();
                listPlantGrid.Add(plant);
            }

            listPlantDelete = (from s in displayData.listPlant
                               where !listPlantGrid.Any(es => es.PlantCode == s.PlantCode)
                               select new MasterPlantModel {
                                   PlantCode = s.PlantCode,
                                   PlantDescription = s.PlantDescription,
                                   Mode = "D"
                               }).ToList();

            listPlantInsert = (from s in listPlantGrid
                               where !displayData.listPlant.Any(es => es.PlantCode == s.PlantCode)
                               select new MasterPlantModel
                               {
                                   PlantCode = s.PlantCode,
                                   PlantDescription = s.PlantDescription,
                                   Mode = "I"
                               }).ToList();

            listPlantUpdate = (from s in listPlantGrid
                               join d in displayData.listPlant on s.PlantCode equals d.PlantCode
                               where s.PlantDescription != d.PlantDescription
                               select new MasterPlantModel
                               {
                                   PlantCode = s.PlantCode,
                                   PlantDescription = s.PlantDescription,
                                   Mode = "U"
                               }).ToList();

            displayData.listPlant.Clear();

            displayData.listPlant = (from i in listPlantInsert
                                     select i)
                                    .Union
                                    (from u in listPlantUpdate
                                    select u).
                                    Union
                                    (from d in listPlantDelete
                                    select d).ToList();

            if(rbProduct.Checked)
            {
                displayData.ScanMode = "P";
            }
            else
            {
                displayData.ScanMode = "F";
            }

        }

        private bool ValidateData()
        {
            if (ValidateMaxLoginFail() && ValidateComID() && ValidateComName() && ValidateCountDate()
                && ValidateBranch() && ValidateMCHLevel() && ValidatePlantDuplicate())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidatePlantDuplicate()
        {
            //Validate In Rows
            List<MasterPlantModel> listPlantGrid = new List<MasterPlantModel>();
            bool IsDup = false;
            for (int count = 0; count < dataGridViewPlant.Rows.Count - 1; count++)
            {
                string Plantcode = (string)dataGridViewPlant.Rows[count].Cells[0].Value == null ? "" : ((string)dataGridViewPlant.Rows[count].Cells[0].Value).Trim();
                string PlantDesc = (string)dataGridViewPlant.Rows[count].Cells[1].Value == null ? "" : ((string)dataGridViewPlant.Rows[count].Cells[1].Value).Trim();
                string PlantMode = (string)dataGridViewPlant.Rows[count].Cells[2].Value == null ? "" : ((string)dataGridViewPlant.Rows[count].Cells[2].Value).Trim();

                int countRows = (from s in listPlantGrid
                                where s.PlantCode.ToLower() == Plantcode.ToLower()
                                select s).Count();

                if (countRows == 0)
                {
                    MasterPlantModel plant = new MasterPlantModel();
                    plant.PlantCode = Plantcode;
                    plant.PlantDescription = PlantDesc;
                    plant.Mode = PlantMode;
                    listPlantGrid.Add(plant);
                }
                else
                {
                    IsDup = true;
                    break;
                }
            }

            if (IsDup)
            {
                MessageBox.Show(MessageConstants.Plantcannotduplicate, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
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
            //if (string.IsNullOrWhiteSpace(branch.Text))
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
            return true;
        }

        private bool ValidateMCHLevel()
        {

            if (chEditMCH.Checked)
            {
                if (mchLevel1TextBox.Text == "" || mchLevel2TextBox.Text == "" || mchLevel3TextBox.Text == "" || mchLevel4TextBox.Text == "")
                {
                    MessageBox.Show(MessageConstants.MCHLevelcannotbeblank, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
