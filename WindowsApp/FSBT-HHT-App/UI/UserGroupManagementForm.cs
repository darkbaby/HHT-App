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
using System.Text.RegularExpressions;
using FSBT_HHT_Model;
using System.Reflection;
using System.Diagnostics;
using FSBT.HHT.App.Resources;

namespace FSBT.HHT.App.UI
{
    //this class like a view (MVC)
    public partial class UserGroupManagementForm : Form
    {
private LogErrorBll logBll = new LogErrorBll(); 
        private enum SaveChangeState
        {
            All, ExcludeUserGroup, OnlyPermission
        }

        //BBL PART : use like a controller (MVC)
        private UserGroupManagementBll userGroupManagement;

        //especially for datagridview selection changed to prevent fired when no need
        private bool switchEvent = false;

        private bool isOutOfForm = false;

        private string beforeEditGroupName = "";
        private string selectedGroupID = null;
        private string selectedScreenID = null;

        private DataTable dataGroup;
        private BindingSource userGroup_Data = new BindingSource();
        private List<UGM_GroupModel> firstGroupNameList = new List<UGM_GroupModel>();
        private List<UGM_GroupModel> editGroupNameList = new List<UGM_GroupModel>();
        private List<UGM_GroupModel> changeGroupNameList = new List<UGM_GroupModel>();

        private DataTable dataUser;
        private BindingSource memberInGroup_Data = new BindingSource();
        private List<UGM_UserModel> firstUserList = new List<UGM_UserModel>();
        private List<UGM_UserModel> editUserList = new List<UGM_UserModel>();
        private List<UGM_UserModel> changeUserList = new List<UGM_UserModel>();

        private DataTable dataScreen;
        private BindingSource screenInGroup_Data = new BindingSource();
        private List<UGM_ScreenModel> firstScreenList = new List<UGM_ScreenModel>();
        private List<UGM_ScreenModel> editScreenList = new List<UGM_ScreenModel>();
        private List<UGM_ScreenModel> changeScreenList = new List<UGM_ScreenModel>();

        private DataTable dataPermission;
        private BindingSource permissionInScreen_Data = new BindingSource();
        private List<UGM_PermissionModel> firstPermissionList = new List<UGM_PermissionModel>();
        private List<UGM_PermissionModel> editPermissionList = new List<UGM_PermissionModel>();
        private List<UGM_PermissionModel> changePermissionList = new List<UGM_PermissionModel>();

        #region constant variable
        private const int d_ug_GroupID_i = 0;
        private const int d_ug_GroupName_i = 1;
        private const int d_ug_LastUpdate_i = 2;
        private const int d_ug_UpdateBy_i = 3;

        private const int d_u_Member_i = 0;
        private const int d_u_UserName_i = 1;
        private const int d_u_FirstName_i = 2;
        private const int d_u_LastName_i = 3;
        private const int d_u_AddedBy_i = 4;
        private const int d_u_AddedDate_i = 5;

        private const int d_s_Visible_i = 0;
        private const int d_s_ScreenID_i = 1;

        private const int d_p_ComponentAlias_i = 0;
        private const int d_p_ComponentType_i = 1;
        private const int d_p_AllowAdd_i = 2;
        private const int d_p_AllowEdit_i = 3;
        private const int d_p_AllowDelete_i = 4;
        private const int d_p_Enable_i = 5;
        private const int d_p_Visible_i = 6;
        #endregion

        public UserGroupManagementForm()
        {
            InitializeComponent();
        }

        public UserGroupManagementForm(string usrName)
        {
            InitializeComponent();

            userGroupManagement = new UserGroupManagementBll(usrName);

            //set initial option for user group datagridview
            dgdUserGroup.ReadOnly = false;
            dgdUserGroup.AutoGenerateColumns = false;
            dgdUserGroup.Columns[d_ug_GroupID_i].DataPropertyName = "GroupID";
            dgdUserGroup.Columns[d_ug_GroupID_i].Name = "GroupID";
            dgdUserGroup.Columns[d_ug_GroupID_i].ReadOnly = true;
            dgdUserGroup.Columns[d_ug_GroupName_i].DataPropertyName = "GroupName";
            dgdUserGroup.Columns[d_ug_LastUpdate_i].DataPropertyName = "LastUpdate";
            dgdUserGroup.Columns[d_ug_LastUpdate_i].ReadOnly = true;
            dgdUserGroup.Columns[d_ug_UpdateBy_i].DataPropertyName = "UpdateBy";
            dgdUserGroup.Columns[d_ug_UpdateBy_i].ReadOnly = true;
            dgdUserGroup.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgdUserGroup.MultiSelect = false;

            //set initial option for user datagridview
            dgdUser.ReadOnly = false;
            dgdUser.AutoGenerateColumns = false;
            dgdUser.Columns[d_u_Member_i].DataPropertyName = "Member";
            dgdUser.Columns[d_u_UserName_i].DataPropertyName = "UserName";
            dgdUser.Columns[d_u_UserName_i].ReadOnly = true;
            dgdUser.Columns[d_u_FirstName_i].DataPropertyName = "FirstName";
            dgdUser.Columns[d_u_FirstName_i].ReadOnly = true;
            dgdUser.Columns[d_u_LastName_i].DataPropertyName = "LastName";
            dgdUser.Columns[d_u_LastName_i].ReadOnly = true;
            dgdUser.Columns[d_u_AddedBy_i].DataPropertyName = "AddedBy";
            dgdUser.Columns[d_u_AddedBy_i].ReadOnly = true;
            dgdUser.Columns[d_u_AddedDate_i].DataPropertyName = "AddedDate";
            dgdUser.Columns[d_u_AddedDate_i].ReadOnly = true;
            dgdUser.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgdUser.MultiSelect = false;

            //set initial option for screen datagridview
            dgdScreen.ReadOnly = false;
            dgdScreen.AutoGenerateColumns = false;
            dgdScreen.Columns[d_s_Visible_i].DataPropertyName = "Visible";
            dgdScreen.Columns[d_s_ScreenID_i].DataPropertyName = "ScreenID";
            dgdScreen.Columns[d_s_ScreenID_i].Name = "ScreenID";
            dgdScreen.Columns[d_s_ScreenID_i].ReadOnly = true;
            dgdScreen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgdScreen.MultiSelect = false;

            //set initial option for permission datagridview
            dgdComponent.ReadOnly = false;
            dgdComponent.AutoGenerateColumns = false;
            dgdComponent.Columns[d_p_ComponentAlias_i].DataPropertyName = "ComponentAlias";
            dgdComponent.Columns[d_p_ComponentAlias_i].ReadOnly = true;
            dgdComponent.Columns[d_p_ComponentType_i].DataPropertyName = "ComponentType";
            dgdComponent.Columns[d_p_ComponentType_i].ReadOnly = true;
            dgdComponent.Columns[d_p_AllowAdd_i].DataPropertyName = "AllowAdd";
            dgdComponent.Columns[d_p_AllowEdit_i].DataPropertyName = "AllowEdit";
            dgdComponent.Columns[d_p_AllowDelete_i].DataPropertyName = "AllowDelete";
            dgdComponent.Columns[d_p_Enable_i].DataPropertyName = "Enable";
            dgdComponent.Columns[d_p_Visible_i].DataPropertyName = "Visible";
            dgdComponent.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgdComponent.MultiSelect = false;

            //first we need to show only user group data grid view
            switchEvent = false;
            SetupGroupView();
            switchEvent = true;
        }

        private void UserGroupManagementForm_Load(object sender, EventArgs e)
        {
            dgdUserGroup.ClearSelection();
        }

        //refresh specific datasource for user data grid view when update success
        private void RefreshDataGridViewSource(SaveChangeState saveChangeState)
        {
            if (saveChangeState == SaveChangeState.All)
            {
                RefreshUserGroup();
                RefreshUser();
                RefreshScreen();
                RefreshPermission();
            }
            else
            {
                return;
            }
        }

        private void RefreshUserGroup()
        {
            try
            {
                if (changeGroupNameList.Count != 0)
                {
                    firstGroupNameList = editGroupNameList;
                    editGroupNameList = new List<UGM_GroupModel>();

                    changeGroupNameList.Clear();
                    int tempRowIndex = dgdUserGroup.SelectedRows[0].Index;
                    dataGroup = userGroupManagement.GetListAllUserGroup();
                    userGroup_Data.DataSource = dataGroup;
                    dgdUserGroup.DataSource = userGroup_Data;
                    dgdUserGroup.CurrentCell = dgdUserGroup[0, tempRowIndex];
                    dgdUserGroup.Rows[tempRowIndex].Selected = true;
                }
                else
                {
                    editGroupNameList.Clear();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void RefreshUser()
        {
            try
            {
                if (changeUserList.Count != 0)
                {
                    firstUserList = editUserList;
                    editUserList = new List<UGM_UserModel>();

                    changeUserList.Clear();
                    int tempRowIndex = dgdUser.SelectedRows[0].Index;
                    dataUser = userGroupManagement.GetListUserByGroup(selectedGroupID);
                    memberInGroup_Data.DataSource = dataUser;
                    dgdUser.DataSource = memberInGroup_Data;
                    dgdUser.CurrentCell = dgdUser[0, tempRowIndex];
                    dgdUser.Rows[tempRowIndex].Selected = true;
                }
                else
                {
                    editUserList.Clear();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void RefreshScreen()
        {
            try
            {
                if (changeScreenList.Count != 0)
                {
                    firstScreenList = editScreenList;
                    editScreenList = new List<UGM_ScreenModel>();

                    changeScreenList.Clear();
                    int tempRowIndex = dgdScreen.SelectedRows[0].Index;
                    dataScreen = userGroupManagement.GetScreenByGroup(selectedGroupID);
                    screenInGroup_Data.DataSource = dataScreen;
                    dgdScreen.DataSource = screenInGroup_Data;
                    dgdScreen.CurrentCell = dgdScreen[0, tempRowIndex];
                    dgdScreen.Rows[tempRowIndex].Selected = true;
                }
                else
                {
                    editScreenList.Clear();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void RefreshPermission()
        {
            try
            {
                if (changePermissionList.Count != 0)
                {
                    firstPermissionList = editPermissionList;
                    editPermissionList = new List<UGM_PermissionModel>();

                    changePermissionList.Clear();
                    int tempRowIndex = dgdComponent.SelectedRows[0].Index;
                    dataPermission = userGroupManagement.GetPermissionByGroupIDANDScreenID(selectedGroupID, selectedScreenID);
                    permissionInScreen_Data.DataSource = dataPermission;
                    dgdComponent.DataSource = permissionInScreen_Data;
                    dgdComponent.CurrentCell = dgdComponent[0, tempRowIndex];
                    dgdComponent.Rows[tempRowIndex].Selected = true;
                }
                else
                {
                    editPermissionList.Clear();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        //fire event when select or change user group
        private void dgdUserGroup_SelectionChanged(object sender, EventArgs e)
        {
            if (!switchEvent)
            {
                return;
            }

            if (dgdUserGroup.Focused)
            {
                //if (dgdUserGroup.CurrentRow == null)
                //{
                //    return;
                //}

                //prevent alert box when state that we don't need to show
                if (selectedGroupID != null)
                {
                    //check whether have any data change from user
                    if (isDataChanged(SaveChangeState.ExcludeUserGroup))
                    {
                        //alert save changes box for yes/no answer
                        if (AlertSaveChanges(SaveChangeState.ExcludeUserGroup))
                        {

                            //START ADD NEW 01/12/2017
                            foreach (UGM_UserModel modifiedUser in changeUserList)
                            {
                                UGM_UserModel originalUser = firstUserList.Find(x => x.UserName == modifiedUser.UserName);
                                if (originalUser.Member)
                                {
                                    if (!modifiedUser.Member)
                                    {
                                        MessageBox.Show(MessageConstants.Usermusthaveagroup, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        selectedGroupID = dgdUserGroup.CurrentRow.Cells["GroupID"].Value.ToString();
                                        SetupUserView();
                                        SetupScreenView();
                                        SetupPermissionView();
                                        return;
                                    }
                                }
                            }
                            //END ADD NEW 01/12/2017

                            if (!SaveAction(SaveChangeState.ExcludeUserGroup))
                            {
                                return;
                            }
                        }
                    }
                }

                //normally process when user change selected group and no any data change
                selectedGroupID = dgdUserGroup.CurrentRow.Cells["GroupID"].Value.ToString();
                SetupUserView();
                SetupScreenView();
                SetupPermissionView();
            }
        }

        //fire event when select or change screen name
        private void dgdScreen_SelectionChanged(object sender, EventArgs e)
        {
            if (!switchEvent)
            {
                return;
            }

            if (dgdScreen.Focused)
            {
                if (isDataChanged(SaveChangeState.OnlyPermission))
                {
                    if (AlertSaveChanges(SaveChangeState.OnlyPermission))
                    {
                        if (!SaveAction(SaveChangeState.OnlyPermission))
                        {
                            return;
                        }
                    }
                }

                //normally process when select or change screen name
                selectedScreenID = dgdScreen.CurrentRow.Cells["ScreenID"].Value.ToString();
                SetupPermissionView();
            }
        }

        private void SetupGroupView()
        {
            try
            {
                firstGroupNameList.Clear();
                editGroupNameList.Clear();
                changeGroupNameList.Clear();

                dataGroup = userGroupManagement.GetListAllUserGroup();
                ConvertToList<UGM_GroupModel>(dataGroup, ref firstGroupNameList);
                if (firstGroupNameList.Count != 0)
                {
                    //switchEvent = false;
                    userGroup_Data.DataSource = dataGroup;
                    dgdUserGroup.DataSource = null;
                    dgdUserGroup.DataSource = userGroup_Data;
                    //switchEvent = true;
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void SetupUserView()
        {
            try
            {
                firstUserList.Clear();
                editUserList.Clear();
                changeUserList.Clear();

                dataUser = userGroupManagement.GetListUserByGroup(selectedGroupID);
                ConvertToList<UGM_UserModel>(dataUser, ref firstUserList);
                if (firstUserList.Count != 0)
                {
                    memberInGroup_Data.DataSource = dataUser;
                    dgdUser.DataSource = null;
                    dgdUser.DataSource = memberInGroup_Data;
                }
                FilterUser();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void SetupScreenView()
        {
            try
            {
                firstScreenList.Clear();
                editScreenList.Clear();
                changeScreenList.Clear();

                dataScreen = userGroupManagement.GetScreenByGroup(selectedGroupID);
                ConvertToList<UGM_ScreenModel>(dataScreen, ref firstScreenList);
                if (firstScreenList.Count != 0)
                {
                    switchEvent = false;
                    screenInGroup_Data.DataSource = dataScreen;
                    dgdScreen.DataSource = null;
                    dgdScreen.DataSource = screenInGroup_Data;
                    switchEvent = true;
                }
                FilterScreen();
                dgdScreen.Rows[0].Selected = true;
                selectedScreenID = dgdScreen.SelectedRows[0].Cells[screenName.Index].Value.ToString();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void SetupPermissionView()
        {
            try
            {
                firstPermissionList.Clear();
                editPermissionList.Clear();
                changePermissionList.Clear();

                dataPermission = userGroupManagement.GetPermissionByGroupIDANDScreenID(selectedGroupID, selectedScreenID);
                ConvertToList<UGM_PermissionModel>(dataPermission, ref firstPermissionList);

                //dont need *if-condition* check becasue maybe allow empty
                permissionInScreen_Data.DataSource = dataPermission;
                dgdComponent.DataSource = null;
                dgdComponent.DataSource = permissionInScreen_Data;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private bool isDataChanged(SaveChangeState saveChangeState)
        {
            try
            {
                //save button
                if (saveChangeState == SaveChangeState.All)
                {
                    //editGroupNameList = ConvertToList<UGM_GroupModel>(dataGroup);
                    ConvertToList<UGM_GroupModel>(dataGroup, ref editGroupNameList);
                    for (int i = 0; i < editGroupNameList.Count; i++)
                    {
                        if (!(firstGroupNameList[i].GroupName.Equals(editGroupNameList[i].GroupName)))
                        {
                            changeGroupNameList.Add(editGroupNameList[i]);
                        }
                    }

                    //editUserList = ConvertToList<UGM_UserModel>(dataUser);
                    ConvertToList<UGM_UserModel>(dataUser, ref editUserList);
                    for (int i = 0; i < editUserList.Count; i++)
                    {
                        if (firstUserList[i].Member != editUserList[i].Member)
                        {
                            changeUserList.Add(editUserList[i]);
                        }
                    }

                    //editScreenList = ConvertToList<UGM_ScreenModel>(dataScreen);
                    ConvertToList<UGM_ScreenModel>(dataScreen, ref editScreenList);
                    for (int i = 0; i < editScreenList.Count; i++)
                    {
                        if (firstScreenList[i].Visible != editScreenList[i].Visible)
                        {
                            changeScreenList.Add(editScreenList[i]);
                        }
                    }

                    //editPermissionList = ConvertToList<UGM_PermissionModel>(dataPermission);
                    ConvertToList<UGM_PermissionModel>(dataPermission, ref editPermissionList);
                    for (int i = 0; i < editPermissionList.Count; i++)
                    {
                        if (firstPermissionList[i].AllowAdd != editPermissionList[i].AllowAdd
                                || firstPermissionList[i].AllowEdit != editPermissionList[i].AllowEdit
                                || firstPermissionList[i].AllowDelete != editPermissionList[i].AllowDelete
                                || firstPermissionList[i].Enable != editPermissionList[i].Enable
                                || firstPermissionList[i].Visible != editPermissionList[i].Visible)
                        {
                            changePermissionList.Add(editPermissionList[i]);
                        }
                    }

                    if (changeGroupNameList.Count != 0 || changeUserList.Count != 0
                        || changeScreenList.Count != 0 || changePermissionList.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        editGroupNameList.Clear();
                        editUserList.Clear();
                        editScreenList.Clear();
                        editPermissionList.Clear();
                        return false;
                    }
                }
                //alert dialog 1
                else if (saveChangeState == SaveChangeState.ExcludeUserGroup)
                {
                    //editUserList = ConvertToList<UGM_UserModel>(dataUser);
                    ConvertToList<UGM_UserModel>(dataUser, ref editUserList);
                    for (int i = 0; i < editUserList.Count; i++)
                    {
                        if (firstUserList[i].Member != editUserList[i].Member)
                        {
                            changeUserList.Add(editUserList[i]);
                        }
                    }

                    //editScreenList = ConvertToList<UGM_ScreenModel>(dataScreen);
                    ConvertToList<UGM_ScreenModel>(dataScreen, ref editScreenList);
                    for (int i = 0; i < editScreenList.Count; i++)
                    {
                        if (firstScreenList[i].Visible != editScreenList[i].Visible)
                        {
                            changeScreenList.Add(editScreenList[i]);
                        }
                    }

                    //editPermissionList = ConvertToList<UGM_PermissionModel>(dataPermission);
                    ConvertToList<UGM_PermissionModel>(dataPermission, ref editPermissionList);
                    for (int i = 0; i < editPermissionList.Count; i++)
                    {
                        if (firstPermissionList[i].AllowAdd != editPermissionList[i].AllowAdd
                                || firstPermissionList[i].AllowEdit != editPermissionList[i].AllowEdit
                                || firstPermissionList[i].AllowDelete != editPermissionList[i].AllowDelete
                                || firstPermissionList[i].Enable != editPermissionList[i].Enable
                                || firstPermissionList[i].Visible != editPermissionList[i].Visible)
                        {
                            changePermissionList.Add(editPermissionList[i]);
                        }
                    }

                    if (changeUserList.Count != 0 || changeScreenList.Count != 0 || changePermissionList.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        editUserList.Clear();
                        editScreenList.Clear();
                        editPermissionList.Clear();
                        return false;
                    }
                }
                //alert dialog 2
                else if (saveChangeState == SaveChangeState.OnlyPermission)
                {
                    //editPermissionList = ConvertToList<UGM_PermissionModel>(dataPermission);
                    ConvertToList<UGM_PermissionModel>(dataPermission, ref editPermissionList);
                    for (int i = 0; i < firstPermissionList.Count; i++)
                    {
                        if (firstPermissionList[i].AllowAdd != editPermissionList[i].AllowAdd
                                || firstPermissionList[i].AllowEdit != editPermissionList[i].AllowEdit
                                || firstPermissionList[i].AllowDelete != editPermissionList[i].AllowDelete
                                || firstPermissionList[i].Enable != editPermissionList[i].Enable
                                || firstPermissionList[i].Visible != editPermissionList[i].Visible)
                        {
                            changePermissionList.Add(editPermissionList[i]);
                        }
                    }

                    if (changePermissionList.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        editPermissionList.Clear();
                        return false;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedGroupID != null)
                {
                    if (isDataChanged(0))
                    {

                        //START ADD NEW 01/12/2017
                        foreach (UGM_UserModel modifiedUser in changeUserList)
                        {
                            UGM_UserModel originalUser = firstUserList.Find(x => x.UserName == modifiedUser.UserName);
                            if (originalUser.Member)
                            {
                                if (!modifiedUser.Member)
                                {
                                    MessageBox.Show(MessageConstants.Usermusthaveagroup, MessageConstants.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    BackToOriginalState();
                                    return;
                                }
                            }
                        }
                        //END ADD NEW 01/12/2017

                        SaveAction(SaveChangeState.All);
                    }
                    else
                    {
                        editGroupNameList.Clear();
                        changeGroupNameList.Clear();
                        editUserList.Clear();
                        changeUserList.Clear();
                        editScreenList.Clear();
                        changeScreenList.Clear();
                        editPermissionList.Clear();
                        changePermissionList.Clear();
                        MessageBox.Show(MessageConstants.Nodatachangedsaveactionisnothappened, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private bool SaveAction(SaveChangeState saveChangeState)
        {
            try
            {
                if (saveChangeState == SaveChangeState.All)
                {
                    GoUpdateGroup();
                    GoUpdateUser();
                    GoUpdateScreen();
                    GoUpdatePermission();
                }
                else if (saveChangeState == SaveChangeState.ExcludeUserGroup)
                {
                    GoUpdateUser();
                    GoUpdateScreen();
                    GoUpdatePermission();
                }
                else if (saveChangeState == SaveChangeState.OnlyPermission)
                {
                    GoUpdatePermission();
                }

                if (userGroupManagement.SaveChange())
                {
                    RefreshDataGridViewSource(saveChangeState);
                    MessageBox.Show(MessageConstants.Updatedatacompleted, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show(MessageConstants.cannotupdatedataindatabase, MessageConstants.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    BackToOriginalState();
                    return false;
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        private bool AlertSaveChanges(SaveChangeState saveChangeState)
        {
            try
            {
                if (!btnSave.Visible || !btnSave.Enabled)
                {
                    return false;
                }

                DialogResult prompt;
                if (saveChangeState == SaveChangeState.ExcludeUserGroup)
                {
                    prompt = MessageBox.Show(MessageConstants.OneDoyouwanttosavechanges, MessageConstants.TitleAlert, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                else if (saveChangeState == SaveChangeState.OnlyPermission)
                {
                    prompt = MessageBox.Show(MessageConstants.TwoDoyouwanttosavechanges, MessageConstants.TitleAlert, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                else
                {
                    prompt = MessageBox.Show(MessageConstants.AnyDoyouwanttosavechanges, MessageConstants.TitleAlert, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }

                if (prompt == DialogResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }

        private void GoUpdateGroup()
        {
            try
            {
                if (changeGroupNameList.Count != 0)
                {
                    userGroupManagement.UpdateChangedUserGroupName(changeGroupNameList);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void GoUpdateUser()
        {
            try
            {
                if (changeUserList.Count != 0)
                {
                    userGroupManagement.UpdateChangedMemberGroup(changeUserList, selectedGroupID);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void GoUpdateScreen()
        {
            try
            {
                if (changeScreenList.Count != 0)
                {
                    userGroupManagement.UpdateChangedVisibleScreen(changeScreenList, selectedGroupID);

                    var reportFrom = changeScreenList.Where(x => x.ScreenID == "ReportPrintForm");
                    if (reportFrom.Count() > 0)
                    {
                        userGroupManagement.UpdateChangedGroupReport(changeScreenList, selectedGroupID);
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void GoUpdatePermission()
        {
            try
            {
                if (changePermissionList.Count != 0)
                {
                    userGroupManagement.UpdateChangedPermission(changePermissionList, selectedScreenID, selectedGroupID);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            BackToOriginalState();
        }

        private void BackToOriginalState()
        {
            try
            {
                if (selectedGroupID != null)
                {
                    int tempIndexRowUserGroup = dgdUserGroup.SelectedRows[0].Index;
                    int tempIndexRowScreen = dgdScreen.SelectedRows[0].Index;
                    switchEvent = false;
                    SetupGroupView();
                    dgdUserGroup.Rows[tempIndexRowUserGroup].Selected = true;
                    dgdUserGroup.CurrentCell = dgdUserGroup.Rows[tempIndexRowUserGroup].Cells[1];
                    switchEvent = true;                    
                    SetupUserView();
                    string tempSelectedScreenID = selectedScreenID;
                    SetupScreenView();
                    selectedScreenID = tempSelectedScreenID;
                    SetupPermissionView();
                    dgdScreen.Rows[tempIndexRowScreen].Selected = true;
                    dgdScreen.CurrentCell = dgdScreen.Rows[tempIndexRowScreen].Cells[1];
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        #region stable code
        private void ConvertToList<T>(DataTable data, ref List<T> tempList)
        {
            try
            {
                foreach (DataRow row in data.Rows)
                {
                    Type temp = typeof(T);
                    T obj = Activator.CreateInstance<T>();

                    foreach (DataColumn column in row.Table.Columns)
                    {
                        foreach (PropertyInfo pro in temp.GetProperties())
                        {
                            if (pro.Name == column.ColumnName)
                            {
                                if (pro.PropertyType == typeof(DateTime) || pro.PropertyType == typeof(DateTime?))
                                {
                                    if (Convert.IsDBNull(row[column.ColumnName]) || row[column.ColumnName] == null)
                                    {
                                        pro.SetValue(obj, (DateTime?)null, null);
                                    }
                                    else
                                    {
                                        pro.SetValue(obj, Convert.ToDateTime(row[column.ColumnName]), null);
                                    }
                                }
                                else
                                {
                                    pro.SetValue(obj, Convert.ChangeType(row[column.ColumnName], Type.GetType(pro.PropertyType.ToString())), null);
                                }
                            }
                            else
                                continue;
                        }
                    }
                    tempList.Add(obj);
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                FilterScreen();
            }
            else if (!(Regex.IsMatch(e.KeyChar.ToString(), @"[a-zA-Z0-9]") || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void FilterScreen()
        {
            if (textBox1.Text == "")
            {
                screenInGroup_Data.RemoveFilter();
            }
            else
            {
                screenInGroup_Data.Filter = string.Format("ScreenID LIKE '%{0}%'", textBox1.Text);
                dgdScreen.ClearSelection();
                dgdScreen.CurrentCell = null;
                if (dgdScreen.RowCount == 0)
                {
                    MessageBox.Show(MessageConstants.Noscreenfound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                FilterUser();
            }
            else if (!(Regex.IsMatch(e.KeyChar.ToString(), @"[a-zA-Z0-9]") || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void FilterUser()
        {
            if (textBox2.Text == "")
            {
                memberInGroup_Data.RemoveFilter();
            }
            else
            {
                memberInGroup_Data.Filter = string.Format("UserName LIKE '%{0}%'", textBox2.Text);
                dgdUser.ClearSelection();
                dgdUser.CurrentCell = null;
                if (dgdUser.RowCount == 0)
                {
                    MessageBox.Show(MessageConstants.Nouserfound, MessageConstants.TitleInfomation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void dgdUserGroup_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                beforeEditGroupName = dgdUserGroup[e.ColumnIndex, e.RowIndex].Value.ToString();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }

        private void dgdUserGroup_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgdUserGroup[e.ColumnIndex, e.RowIndex].Value == null)
                {
                    dgdUserGroup[e.ColumnIndex, e.RowIndex].Value = beforeEditGroupName;
                    return;
                }
                else
                {
                    string tempString = dgdUserGroup.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim();
                    tempString = Regex.Replace(tempString, @"\s{2,}", " ");
                    if (String.IsNullOrEmpty(tempString))
                    {
                        dgdUserGroup[e.ColumnIndex, e.RowIndex].Value = beforeEditGroupName;
                        return;
                    }
                    else if (Regex.IsMatch(tempString, @"[^a-zA-z0-9\s]"))
                    {
                        dgdUserGroup[e.ColumnIndex, e.RowIndex].Value = beforeEditGroupName;
                        return;
                    }
                    else if (tempString.Equals(beforeEditGroupName))
                    {
                        dgdUserGroup[e.ColumnIndex, e.RowIndex].Value = beforeEditGroupName;
                        return;
                    }
                    else
                    {
                        dgdUserGroup[e.ColumnIndex, e.RowIndex].Value = tempString;
                        beforeEditGroupName = "";
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }
        }
        #endregion

        private void UserGroupManagementForm_Activated(object sender, EventArgs e)
        {
            if (selectedGroupID == null)
            {
                return;
            }

            userGroupManagement.MediumToRefreshDAO();
            BackToOriginalState();
        }
    }
}
