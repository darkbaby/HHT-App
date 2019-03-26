using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSBT_HHT_DAL.DAO;
using FSBT_HHT_Model;
using System.Reflection;

namespace FSBT_HHT_BLL
{
    public class PermissionComponentBll
    {
        private LogErrorBll logBll = new LogErrorBll(); 
        public Form FrmSetup { get; set; }
        public string UserName { get; set; }
        private PermissionDAO permissDAO = new PermissionDAO();
        private List<PermissionComponentModel> lstComponentUser = new List<PermissionComponentModel>();
        public PermissionComponentBll(string userName)
        {
            UserName = userName;
            LoadPermissionComponentByUser(UserName);
        }

        private void LoadPermissionComponentByUser(string userName)
        {
            lstComponentUser = permissDAO.LoadPermissionComponentByUser(userName);
        }

        private List<PermissionComponentModel> GetComponentByScreen(string ScreenID)
        {
            List<PermissionComponentModel> lstComp = new List<PermissionComponentModel>();
            lstComp = (from lst in lstComponentUser
                       where lst.ScreenID == ScreenID
                       select lst).ToList<PermissionComponentModel>();
            return lstComp;
        }

        public bool SetPermissionComponentByScreen(Form frm)
        {
            try
            {
                FrmSetup = frm;
                List<PermissionComponentModel> lstComp = GetComponentByScreen(FrmSetup.Name);
                foreach (PermissionComponentModel comp in lstComp)
                {
                    foreach (Control contrl in FrmSetup.Controls.Find(comp.ComponentName, true))
                    {
                        if (contrl.Name == comp.ComponentName)
                        {
                            contrl.Enabled = comp.Enable;
                            contrl.Visible = comp.Visible;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return false;
            }
        }
    }
}
