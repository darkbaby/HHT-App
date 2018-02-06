using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSBT_HHT_BLL;
using FSBT_HHT_Model;
using FSBT_HHT_DAL.DAO;
using FSBT.HHT.App.Resources;

namespace FSBT.HHT.App.UI
{
    public partial class UserManagementForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        private UserManagementBll userBLL = new UserManagementBll();
        private UserManagementDAO dao = new UserManagementDAO();
        private int selectedRow = -1;
        private int selectedColumn = -1;
        private string statusToSave = "landing";
        private string Username;

        UserModel selectedUser = new UserModel();

        public UserManagementForm(string loginUsername)
        {
            try
            {
                Username = loginUsername;
                InitializeComponent();
                List<UserModel> allUserData = userBLL.GetUserData();
                DisplayUser(allUserData);
                AddGroupToDropdownlist();
                userDataGridView.ClearSelection();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        #region SelectRow
        private void userDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int currRow = e.RowIndex;
                if (currRow == selectedRow)
                {
                    return;
                }
                else
                {
                    selectedRow = currRow;
                    selectedColumn = e.ColumnIndex;
                }

                if (CheckIsDataEdit())
                {
                    DialogResult dialogResult = MessageBox.Show(MessageConstants.Youhavenotsavecurrentdata, MessageConstants.TitleSaveEdit, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ValidateToUpdate();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do nothing
                    }
                }
                else
                {
                    //do nothing
                }

                statusToSave = "edit";

                SetModelDataFromCurrentRow(currRow);

                usernameTextBox.Enabled = false;
                SetUserDetail();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private bool CheckIsDataEdit()
        {
            UserModel userDetailData = new UserModel();
            userDetailData = GetUserDetail(userDetailData);
            if (!statusToSave.Equals("landing"))
            {
                if (!selectedUser.Username.Equals(userDetailData.Username)
                || !selectedUser.Password.Equals(userDetailData.Password)
                || !selectedUser.FisrtName.Equals(userDetailData.FisrtName)
                || !selectedUser.LastName.Equals(userDetailData.LastName)
                || !selectedUser.GroupName.Equals(userDetailData.GroupName)
                || !selectedUser.Enable.Equals(userDetailData.Enable)
                || !selectedUser.Lock.Equals(userDetailData.Lock))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Add User
        private void addUserButton_Click(object sender, EventArgs e)
        {
            try
            {
                statusToSave = "add";
                selectedRow = -1;
                string userNumber = userBLL.GetNewUserNumber();

                if (userNumber == "error")
                {
                    MessageBox.Show(MessageConstants.Connectivityissuesoccurred, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    SetModelData("User" + userNumber, "User" + userNumber, "FirstName" + userNumber, "LastName" + userNumber, "GroupName" + userNumber, true, false);

                    usernameTextBox.Enabled = true;
                    groupComboBox.SelectedIndex = 0;
                    SetUserDetail();
                }

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }
        #endregion

        #region Edit Save User
        private void resetPasswordButton_Click(object sender, EventArgs e)
        {
            passwordTextBox.Text = usernameTextBox.Text;
        }

        private void saveUserButton_Click(object sender, EventArgs e)
        {
            ValidateToUpdate();
        }

        public void ValidateToUpdate()
        {
            try
            {
                bool isPasswordChange = IsPasswordChange();
                if (statusToSave == "add" || statusToSave == "landing")
                {
                    if (ValidateUsername() && ValidatePassword() && ValidateName() && ValidateEnableAndLock())
                    {
                        UpdateUserData(isPasswordChange);
                    }
                    else
                    {
                        //do nothing
                    }
                }
                else if (statusToSave == "edit")
                {
                    if (isPasswordChange)
                    {
                        if (ValidatePassword() && ValidateName())
                        {
                            UpdateUserData(isPasswordChange);
                        }
                        else
                        {
                            //do nothing
                        }
                    }
                    else
                    {
                        if (ValidateName())
                        {
                            UpdateUserData(isPasswordChange);
                        }
                        else
                        {
                            //do nothing
                        }
                    }
                }
                else
                {
                    MessageBox.Show(MessageConstants.cannotsaveuserinformationtodatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void UpdateUserData(bool isPasswordChange)
        {
            try
            {
                string isSuccess = "";
                UserModel selectedUserData = new UserModel();
                GetUserDetail(selectedUserData);

                if (statusToSave == "add" || statusToSave == "landing")
                {
                    selectedUserData.CreateBy = Username;
                    isSuccess = userBLL.AddUser(selectedUserData);
                    if (isSuccess.Equals("success"))
                    {
                        List<UserModel> allUserData = userBLL.GetUserData();
                        DisplayUser(allUserData);
                        selectedUser = selectedUserData;
                        SetUserDetail();
                        if (selectedRow > -1)
                        {
                            this.userDataGridView.CurrentCell = this.userDataGridView[selectedColumn, selectedRow];
                        }
                        MessageBox.Show(MessageConstants.Updatedatacompleted, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (isSuccess.Equals("have user"))
                    {
                        MessageBox.Show(MessageConstants.Alreadyhavethisuserinserver, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.cannotsaveuserinformationtodatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (statusToSave == "edit")
                {
                    selectedUserData.UpdateBy = Username;
                    isSuccess = userBLL.UpdateUserData(selectedUserData, isPasswordChange);
                    if (isSuccess.Equals("success"))
                    {
                        List<UserModel> allUserData = userBLL.GetUserData();
                        DisplayUser(allUserData);
                        selectedUser = selectedUserData;
                        SetUserDetail();
                        this.userDataGridView.CurrentCell = this.userDataGridView[selectedColumn, selectedRow];
                        MessageBox.Show(MessageConstants.Updatedatacompleted, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(MessageConstants.cannotsaveuserinformationtodatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(MessageConstants.cannotsaveuserinformationtodatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private bool IsPasswordChange()
        {
            if (passwordTextBox.Text.Equals(selectedUser.Password))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidateUsername()
        {
            if (usernameTextBox.Text.Length == 0)
            {
                MessageBox.Show(MessageConstants.Usernamecannotnull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (usernameTextBox.Text.Length < 4)
            {
                MessageBox.Show(MessageConstants.Usernametooshort, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (usernameTextBox.Text.Length > 20)
            {
                MessageBox.Show(MessageConstants.Usernametoolong, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (!(IsNotExistsUsername(usernameTextBox.Text)))
            {
                MessageBox.Show(MessageConstants.Usernamecannotduplicate, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidatePassword()
        {
            if (passwordTextBox.Text.Length == 0)
            {
                MessageBox.Show(MessageConstants.Passwordcannotnull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (passwordTextBox.Text.Length < 4)
            {
                MessageBox.Show(MessageConstants.Passwordtooshort, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (passwordTextBox.Text.Length > 20)
            {
                MessageBox.Show(MessageConstants.Passwordtoolong, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidateName()
        {
            if (firstNameTextBox.Text.Length == 0)
            {
                MessageBox.Show(MessageConstants.Firstnamecannotnull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (lastNameTextBox.Text.Length == 0)
            {
                MessageBox.Show(MessageConstants.Lastnamecannotnull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidateEnableAndLock()
        {
            bool result = true;

            if (enableFalseRadioButton.Checked == false && enableTrueRadioButton.Checked == false)
            {
                MessageBox.Show(MessageConstants.Enablecannotnull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                result = false;
            }

            if (lockFalseRadioButton.Checked == false && lockTrueRadioButton.Checked == false)
            {
                MessageBox.Show(MessageConstants.Lockcannotnull, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                result = false;
            }

            return result;

        }

        public bool IsNotExistsUsername(string username)
        {
            try
            {
                return dao.checkIsExistsUser(username);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return false;
            }
        }

        #endregion

        #region Delete User
        private void deleteUserButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsNotExistsUsername(selectedUser.Username))
                {
                    MessageBox.Show(MessageConstants.cannotdeleteusernamefromdatabase, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {                    
                    DialogResult dialogResult = MessageBox.Show(MessageConstants.Doyouwanttodeleteuser + selectedUser.Username, MessageConstants.TitleDeleteUser, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        statusToSave = "delete";
                        string isSuccess = userBLL.DeleteUser(selectedUser.Username);
                        if (isSuccess.Equals("success"))
                        {
                            List<UserModel> allUserData = userBLL.GetUserData();
                            DisplayUser(allUserData);

                            if (allUserData.Count > 0)
                            {
                                selectedRow = 0;
                                SetModelDataFromCurrentRow(0);
                                SetUserDetail();
                                usernameTextBox.Enabled = false;
                            }
                            else
                            {
                                ClearUserDetail();
                                SetModelData("", "", "", "", "", false, false);
                            }

                            MessageBox.Show(MessageConstants.Updatedatacompleted, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(MessageConstants.cannotdeleteusernamefromdatabase, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do nothing
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }
        #endregion

        #region Search
        private void searchUserButton_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        private void searchUserTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchData();
            }
            else
            {
                //do nothing
            }
        }

        private void SearchData()
        {
            try
            {
                string searchWord = searchUserTextBox.Text;
                List<UserModel> searchUser = userBLL.SearchUser(searchWord);
                if (searchUser.Count.Equals(0))
                {
                    MessageBox.Show(MessageConstants.Nouserfound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DisplayUser(searchUser);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        #endregion

        #region Get Set User Detail
        private UserModel GetUserDetail(UserModel data)
        {
            data.Username = usernameTextBox.Text;
            data.Password = passwordTextBox.Text;
            data.FisrtName = firstNameTextBox.Text;
            data.LastName = lastNameTextBox.Text;
            data.GroupName = groupComboBox.Text;
            if (enableTrueRadioButton.Checked == true)
            {
                data.Enable = true;
            }
            else
            {
                data.Enable = false;
            }

            if (lockTrueRadioButton.Checked == true)
            {
                data.Lock = true;
            }
            else
            {
                data.Lock = false;
            }
            return data;
        }

        private void SetUserDetail()
        {
            usernameTextBox.Text = selectedUser.Username;
            passwordTextBox.Text = selectedUser.Password;
            firstNameTextBox.Text = selectedUser.FisrtName;
            lastNameTextBox.Text = selectedUser.LastName;
            groupComboBox.Text = selectedUser.GroupName;
            if (selectedUser.Enable == true)
            {
                enableTrueRadioButton.PerformClick();
            }
            else
            {
                enableFalseRadioButton.PerformClick();
            }
            if (selectedUser.Lock == true)
            {
                lockTrueRadioButton.PerformClick();
            }
            else
            {
                lockFalseRadioButton.PerformClick();
            }
        }

        private void ClearUserDetail()
        {
            usernameTextBox.Text = "";
            passwordTextBox.Text = "";
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            enableTrueRadioButton.Checked = false;
            enableFalseRadioButton.Checked = false;
            lockTrueRadioButton.Checked = false;
            lockFalseRadioButton.Checked = false;
        }

        private void DisabledUserDetail()
        {
            usernameTextBox.Enabled = false;
            passwordTextBox.Enabled = false;
            firstNameTextBox.Enabled = false;
            lastNameTextBox.Enabled = false;
            enableTrueRadioButton.Checked = false;
            enableFalseRadioButton.Checked = false;
            lockTrueRadioButton.Checked = false;
            lockFalseRadioButton.Checked = false;
        }
        #endregion

        private void cancelUserButton_Click(object sender, EventArgs e)
        {
            try
            {
                //if (CheckIsDataEdit())
                //{
                //    DialogResult dialogResult = MessageBox.Show("Do you want to discard current data?", "", MessageBoxButtons.YesNo);
                //    if (dialogResult == DialogResult.Yes)
                //    {
                //        selectedRow = -1;
                //        ClearUserDetail();
                //        SetModelData("", "", "", "", "", false, false);
                //        usernameTextBox.Enabled = true;
                //    }
                //    else if (dialogResult == DialogResult.No)
                //    {

                //    }
                //}
                //else
                //{
                //    SetUserDetail();
                //}

                DialogResult dialogResult = MessageBox.Show("Do you want to discard current data?", "", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    List<UserModel> allUserData = userBLL.GetUserData();
                    DisplayUser(allUserData);
                    AddGroupToDropdownlist();
                    ClearUserDetail();
                    userDataGridView.ClearSelection();
                    usernameTextBox.Enabled = true;
                    statusToSave = "landing";
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void DisplayUser(List<UserModel> UserData)
        {
            try
            {
                userDataGridView.DataSource = UserData;
                userDataGridView.AutoResizeColumns();
                userDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                userDataGridView.Columns["Password"].Visible = false;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        public void SetModelDataFromCurrentRow(int currRow)
        {
            try
            {
                if (currRow >= 0)
                {
                    string Username = userDataGridView.Rows[currRow].Cells[0].Value.ToString();
                    string Password = userDataGridView.Rows[currRow].Cells[1].Value.ToString();
                    string FirstName = userDataGridView.Rows[currRow].Cells[2].Value.ToString();
                    string LastName = userDataGridView.Rows[currRow].Cells[3].Value.ToString();
                    string GroupName = userDataGridView.Rows[currRow].Cells[4].Value.ToString();
                    bool Enable = (bool)userDataGridView.Rows[currRow].Cells[5].Value;
                    bool Lock = (bool)userDataGridView.Rows[currRow].Cells[6].Value;

                    SetModelData(Username, Password, FirstName, LastName, GroupName, Enable, Lock);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        private void SetModelData(string username, string password, string firstName, string lastName, string groupName, bool enable, bool Lock)
        {
            selectedUser.Username = username;
            selectedUser.Password = password;
            selectedUser.FisrtName = firstName;
            selectedUser.LastName = lastName;
            selectedUser.GroupName = groupName;
            selectedUser.Enable = enable;
            selectedUser.Lock = Lock;
        }

        private void AddGroupToDropdownlist()
        {
            try
            {
                List<UGM_GroupModel> groupAll = new List<UGM_GroupModel>();
                groupAll = dao.GetAllGroup();

                foreach (var i in groupAll)
                {
                    groupComboBox.Items.Add(i.GroupName.ToString());
                }

                groupComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

    }
}
