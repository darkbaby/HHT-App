using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_Model;

namespace FSBT_HHT_DAL.DAO
{
    public class SystemSettingDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        public SystemSettingDAO()
        {

        }

        public SystemSettingModel GetSettingData()
        {
            SystemSettingModel SettingData = new SystemSettingModel();
            Entities dbContext = new Entities();
            try
            {
                SettingData.MaxLoginFail = (from setting in dbContext.SystemSettings
                                            where setting.SettingKey.Equals("LoginFailLimit")
                                            select setting.ValueInt.Value).FirstOrDefault();
                SettingData.ComID = (from setting in dbContext.SystemSettings
                                     where setting.SettingKey.Equals("ComID")
                                     select setting.ValueString).FirstOrDefault();
                SettingData.ComName = (from setting in dbContext.SystemSettings
                                       where setting.SettingKey.Equals("ComName")
                                     select setting.ValueString).FirstOrDefault();
                SettingData.CountDate = (from setting in dbContext.SystemSettings
                                         where setting.SettingKey.Equals("CountDate")
                                         select setting.ValueDate.Value).FirstOrDefault();
                SettingData.SectionType = (from setting in dbContext.SystemSettings
                                         where setting.SettingKey.Equals("SectionType")
                                         select setting.ValueString).FirstOrDefault();
                SettingData.Branch = (from setting in dbContext.SystemSettings
                                           where setting.SettingKey.Equals("Branch")
                                           select setting.ValueString).FirstOrDefault();
                SettingData.PathSKUFile = (from setting in dbContext.SystemSettings
                                           where setting.SettingKey.Equals("PathSKUFile")
                                           select setting.ValueString).FirstOrDefault();
                SettingData.PathBarcodeFile = (from setting in dbContext.SystemSettings
                                           where setting.SettingKey.Equals("PathBarcodeFile")
                                           select setting.ValueString).FirstOrDefault();
                SettingData.PathPackBarcodeFile = (from setting in dbContext.SystemSettings
                                           where setting.SettingKey.Equals("PathPackBarcodeFile")
                                           select setting.ValueString).FirstOrDefault();
                SettingData.PathBrandFile = (from setting in dbContext.SystemSettings
                                           where setting.SettingKey.Equals("PathBrandFile")
                                           select setting.ValueString).FirstOrDefault();
                SettingData.RealTimeMode = (bool)(from setting in dbContext.SystemSettings
                                            where setting.SettingKey.Equals("RealtimeMode")
                                            select setting.ValueBoolean).FirstOrDefault();
                return SettingData;
            }
            catch(Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
                return SettingData;
            }
        }

        public string UpdateSettingData(SystemSettingModel newSettingData)
        {
            SystemSetting systemSetting = new SystemSetting();
            using (Entities DBcontext = new Entities())
            {
                using (var DBtransaction = DBcontext.Database.BeginTransaction())
                {
                    try
                    {
                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals("LoginFailLimit")).First();
                        systemSetting.ValueInt = newSettingData.MaxLoginFail;

                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals("ComID")).First();
                        systemSetting.ValueString = newSettingData.ComID;

                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals("ComName")).First();
                        systemSetting.ValueString = newSettingData.ComName;

                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals("CountDate")).First();
                        systemSetting.ValueDate = newSettingData.CountDate;

                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals("SectionType")).First();
                        systemSetting.ValueString = newSettingData.SectionType;

                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals("Branch")).First();
                        systemSetting.ValueString = newSettingData.Branch;

                        DBcontext.SaveChanges();

                        DBtransaction.Commit();

                        return "success";
                    }
                    catch(Exception ex)
                    {
                        DBtransaction.Rollback();
                        log.Error(String.Format("Exception : {0}", ex.StackTrace));
                        return "error";
                    }
                }
            }
        }

        public string GetComputerID()
        {
            string ComputerID = "";
            Entities dbContext = new Entities();

            ComputerID = (from s in dbContext.SystemSettings
                          where s.SettingKey.ToUpper().Equals("COMID")
                          select s.ValueString).FirstOrDefault();

            return ComputerID;
        }

        public bool saveModeRealtime(bool isRealtime)
        {
            bool result = false;
            SystemSetting systemSetting = new SystemSetting();
            using (Entities DBcontext = new Entities())
            {
                using (var DBtransaction = DBcontext.Database.BeginTransaction())
                {
                    try
                    {
                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals("RealtimeMode")).First();
                        systemSetting.ValueBoolean = isRealtime;

                        DBcontext.SaveChanges();

                        DBtransaction.Commit();

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        DBtransaction.Rollback();
                        log.Error(String.Format("Exception : {0}", ex.StackTrace));
                        result = false;
                    }
                }
            }

            return result;
        }

        public bool GetRealTimeMode()
        {
            bool realTime = false;
            Entities dbContext = new Entities();

            realTime = (from s in dbContext.SystemSettings
                          where s.SettingKey.ToUpper().Equals("RealtimeMode")
                          select s.ValueBoolean).FirstOrDefault().Value;

            return realTime;
        }
    }
}
