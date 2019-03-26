using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBT_HHT_Model;

using System.Reflection;

namespace FSBT_HHT_DAL.DAO
{
    public class SystemSettingDAO
    {

        public SystemSettingDAO()
        {

        }

        private LogErrorDAO logBll = new LogErrorDAO();

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

                SettingData.CountDate = (from setting in dbContext.SystemSettings
                                         where setting.SettingKey.Equals("CountDate")
                                         select setting.ValueDate.Value).FirstOrDefault();

                SettingData.ComName = (from setting in dbContext.SystemSettings
                                       where setting.SettingKey.Equals("ComName")
                                       select setting.ValueString).FirstOrDefault();

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

                SettingData.StoreID = (from setting in dbContext.SystemSettings
                                       where setting.SettingKey.Equals("StoreID")
                                       select setting.ValueString).FirstOrDefault();

                SettingData.Plant = (from setting in dbContext.SystemSettings
                                     where setting.SettingKey.Equals("Plant")
                                     select setting.ValueString).FirstOrDefault();

                SettingData.MCHLevel1 = (from setting in dbContext.SystemSettings
                                         where setting.SettingKey.Equals("MCHLevel1")
                                         select setting.ValueString).FirstOrDefault();

                SettingData.MCHLevel2 = (from setting in dbContext.SystemSettings
                                         where setting.SettingKey.Equals("MCHLevel2")
                                         select setting.ValueString).FirstOrDefault();

                SettingData.MCHLevel3 = (from setting in dbContext.SystemSettings
                                         where setting.SettingKey.Equals("MCHLevel3")
                                         select setting.ValueString).FirstOrDefault();

                SettingData.MCHLevel4 = (from setting in dbContext.SystemSettings
                                         where setting.SettingKey.Equals("MCHLevel4")
                                         select setting.ValueString).FirstOrDefault();

                SettingData.ScanMode = (from setting in dbContext.SystemSettings
                                         where setting.SettingKey.Equals("ScanMode")
                                         select setting.ValueString).FirstOrDefault();

                SettingData.listPlant = (from master in dbContext.MasterPlants
                                         select new MasterPlantModel
                                         {
                                             PlantCode = master.PlantCode,
                                             PlantDescription = master.PlantDescription
                                         }).ToList();

                if (SettingData.MCHLevel1.ToString() == "" && SettingData.MCHLevel2.ToString() == ""
                    && SettingData.MCHLevel3.ToString() == "" && SettingData.MCHLevel4.ToString() == "")
                {
                    SettingData.IsEditMchSettings = false;
                }
                else
                {
                    SettingData.IsEditMchSettings = true;
                }

                return SettingData;
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                return SettingData;
            }
        }

        public string GetSettingStringByKey(string key)
        {
            string str = "";
            SystemSettingModel SettingData = new SystemSettingModel();
            Entities dbContext = new Entities();
            try
            {
                str = (from setting in dbContext.SystemSettings
                       where setting.SettingKey.ToUpper().Equals(key.ToUpper())
                       select setting.ValueString).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }

            return str;
        }

        public int GetSettingIntByKey(string key)
        {
            int value = 0;
            SystemSettingModel SettingData = new SystemSettingModel();
            Entities dbContext = new Entities();
            try
            {
                value = (from setting in dbContext.SystemSettings
                         where setting.SettingKey.Equals(key)
                         select setting.ValueInt.Value).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
            }

            return value;
        }

        public string UpdateSettingData(SystemSettingModel newSettingData)
        {
            SystemSetting systemSetting = new SystemSetting();
            MasterPlant masterPlant = new MasterPlant();
            using (Entities DBcontext = new Entities())
            {
                using (var DBtransaction = DBcontext.Database.BeginTransaction())
                {
                    try
                    {
                        #region SystemSetting

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

                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals("StoreID")).First();
                        systemSetting.ValueString = newSettingData.StoreID;

                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals("Plant")).First();
                        systemSetting.ValueString = newSettingData.Plant;

                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals("MCHLevel1")).First();
                        systemSetting.ValueString = newSettingData.MCHLevel1;

                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals("MCHLevel2")).First();
                        systemSetting.ValueString = newSettingData.MCHLevel2;

                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals("MCHLevel3")).First();
                        systemSetting.ValueString = newSettingData.MCHLevel3;

                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals("MCHLevel4")).First();
                        systemSetting.ValueString = newSettingData.MCHLevel4;

                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals("ScanMode")).First();
                        systemSetting.ValueString = newSettingData.ScanMode;

                        DBcontext.SaveChanges();

                        #endregion

                        #region MasterPlant

                        foreach (MasterPlantModel plant in newSettingData.listPlant)
                        {
                            if (plant.Mode == null) { plant.Mode = ""; }

                            if (plant.Mode.ToUpper().Equals("U"))
                            {
                                masterPlant = DBcontext.MasterPlants.Where(x => x.PlantCode.Equals(plant.PlantCode)).First();
                                masterPlant.PlantDescription = plant.PlantDescription;
                            }
                            else if (plant.Mode.ToUpper().Equals("I"))
                            {
                                MasterPlant m = new MasterPlant();
                                m.PlantCode = plant.PlantCode;
                                m.PlantDescription = plant.PlantDescription;
                                m.UpdateBy = newSettingData.UpdateBy;
                                m.UpdateDate = DateTime.Now;
                                m.CreateBy = newSettingData.UpdateBy;
                                m.CreateDate = DateTime.Now;
                                DBcontext.MasterPlants.Add(m);
                            }
                            else if (plant.Mode.ToUpper().Equals("D"))
                            {
                                MasterPlant m = new MasterPlant();
                                m = (from p in DBcontext.MasterPlants
                                     where p.PlantCode.Equals(plant.PlantCode)
                                     select p).FirstOrDefault();
                                DBcontext.MasterPlants.Remove(m);
                            }

                            DBcontext.SaveChanges();
                        }

                        #endregion

                        DBtransaction.Commit();

                        return "success";
                    }
                    catch (Exception ex)
                    {
                        DBtransaction.Rollback();
                        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                        return "error";
                    }
                }
            }
        }

        public string UpdateSettingData(string settingKey, string datatype, string value)
        {
            SystemSetting systemSetting = new SystemSetting();
            using (Entities DBcontext = new Entities())
            {
                using (var DBtransaction = DBcontext.Database.BeginTransaction())
                {
                    try
                    {
                        #region SystemSetting

                        systemSetting = DBcontext.SystemSettings.Where(x => x.SettingKey.Equals(settingKey)).First();

                        if (datatype.ToLower() == "string")
                            systemSetting.ValueString = value;
                        else if (datatype.ToLower() == "int")
                            systemSetting.ValueInt = Convert.ToInt32(value);
                        else if (datatype.ToLower() == "bool")
                            systemSetting.ValueBoolean = Convert.ToBoolean(value);
                        else if (datatype.ToLower() == "date")
                            systemSetting.ValueDate = DateTime.ParseExact(value.Substring(0, 8), "yyyyMMdd", null);
                        else if (datatype.ToLower() == "datetime")
                            systemSetting.ValueDateTime = Convert.ToDateTime(value);
                        else if (datatype.ToLower() == "double")
                            systemSetting.ValueDouble = Convert.ToDouble(value);
                        
                        DBcontext.SaveChanges();

                        #endregion
                      
                        DBtransaction.Commit();

                        return "success";
                    }
                    catch (Exception ex)
                    {
                        DBtransaction.Rollback();
                        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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

        public string GetComputerName()
        {
            string ComputerName = "";
            Entities dbContext = new Entities();

            ComputerName = (from s in dbContext.SystemSettings
                          where s.SettingKey.ToUpper().Equals("ComName")
                          select s.ValueString).FirstOrDefault();

            return ComputerName;
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
                        logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
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

        public List<string> GetDropDownMCH1(String countSheet)
        {
            List<string> listMCH = new List<string>();
            Entities dbContext = new Entities();
            try
            {
                if (countSheet.Equals("All"))
                {
                    listMCH = (from sap in dbContext.MastSAP_SKU
                               select sap.MCHLevel1).Distinct().ToList();
                }
                else
                {
                    listMCH = (from sap in dbContext.MastSAP_SKU
                               where sap.PIDoc.ToUpper()==countSheet.ToUpper()
                               select sap.MCHLevel1).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                listMCH = null;
            }

            return listMCH;
        }

        public List<String> GetDropDownMCH2(String countSheet, String HeadLevel1)
        {
            List<string> listMCH = new List<string>();
            Entities dbContext = new Entities();
            try
            {
                if (countSheet.Equals("All"))
                {
                    if (HeadLevel1.Equals("All"))
                    {
                        listMCH = (from sap in dbContext.MastSAP_SKU
                                   select sap.MCHLevel2).Distinct().ToList();                  
                    }
                    else
                    {
                        listMCH = (from sap in dbContext.MastSAP_SKU
                                   where sap.MCHLevel1.ToUpper() == HeadLevel1.ToUpper()
                                   select sap.MCHLevel2).Distinct().ToList();
                    }
                }
                else
                {
                    if (HeadLevel1.Equals("All"))
                    {
                        listMCH = (from sap in dbContext.MastSAP_SKU
                                   where sap.PIDoc.ToUpper() == countSheet.ToUpper()
                                   select sap.MCHLevel2).Distinct().ToList();
                    }
                    else
                    {
                        listMCH = (from sap in dbContext.MastSAP_SKU
                                   where sap.PIDoc.ToUpper() == countSheet.ToUpper() && sap.MCHLevel1.ToUpper() == HeadLevel1.ToUpper()
                                   select sap.MCHLevel2).Distinct().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                listMCH = null;
            }
            listMCH.Sort();
            return listMCH;
        }

        public List<String> GetDropDownMCH3(String countSheet, String HeadLevel1, String HeadLevel2)
        {
            List<string> listMCH = new List<string>();
            Entities dbContext = new Entities();
            try
            {
                if (countSheet.Equals("All"))
                {
                    if (HeadLevel1.Equals("All"))
                    {
                        if (HeadLevel2.Equals("All"))
                        {
                            listMCH = (from sap in dbContext.MastSAP_SKU
                                       select sap.MCHLevel3).Distinct().ToList();
                        }
                        else
                        {
                            listMCH = (from sap in dbContext.MastSAP_SKU
                                       where sap.MCHLevel2.ToUpper() == HeadLevel2.ToUpper()
                                       select sap.MCHLevel3).Distinct().ToList();
                        }
                    }
                    else //level1 has value
                    {
                        if (HeadLevel2.Equals("All"))
                        {
                            listMCH = (from sap in dbContext.MastSAP_SKU
                                       where sap.MCHLevel1.ToUpper() == HeadLevel1.ToUpper()
                                       select sap.MCHLevel3).Distinct().ToList();
                        }
                        else //level2 has value
                        {
                            listMCH = (from sap in dbContext.MastSAP_SKU
                                       where sap.MCHLevel1.ToUpper() == HeadLevel1.ToUpper() && sap.MCHLevel2.ToUpper() == HeadLevel2.ToUpper()
                                       select sap.MCHLevel3).Distinct().ToList();
                        }
                    }
                }
                else //countsheet has value
                {
                    if (HeadLevel1.Equals("All"))
                    {
                        if (HeadLevel2.Equals("All"))
                        {
                            listMCH = (from sap in dbContext.MastSAP_SKU
                                       where sap.PIDoc.ToUpper() == countSheet.ToUpper()
                                       select sap.MCHLevel3).Distinct().ToList();
                        }
                        else //level2 has value
                        {
                            listMCH = (from sap in dbContext.MastSAP_SKU
                                       where sap.PIDoc.ToUpper() == countSheet.ToUpper() && sap.MCHLevel2.ToUpper() == HeadLevel2.ToUpper()
                                       select sap.MCHLevel3).Distinct().ToList();
                        }
                    }
                    else //level1 has value
                    {
                        if (HeadLevel2.Equals("All"))
                        {
                            listMCH = (from sap in dbContext.MastSAP_SKU
                                       where sap.PIDoc.ToUpper() == countSheet.ToUpper() && sap.MCHLevel1.ToUpper() == HeadLevel1.ToUpper()
                                       select sap.MCHLevel3).Distinct().ToList();
                        }
                        else //level2 has value
                        {
                            listMCH = (from sap in dbContext.MastSAP_SKU
                                       where sap.PIDoc.ToUpper() == countSheet.ToUpper() && sap.MCHLevel1.ToUpper() == HeadLevel1.ToUpper() && sap.MCHLevel2.ToUpper() == HeadLevel2.ToUpper()
                                       select sap.MCHLevel3).Distinct().ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                listMCH = null;
            }
            listMCH.Sort();
            return listMCH;
        }
        public List<String> GetDropDownMCH4(String countSheet, String HeadLevel1, String HeadLevel2, String HeadLevel3)
        {
            List<string> listMCH = new List<string>();
            Entities dbContext = new Entities();
            try
            {
                if (countSheet.Equals("All"))
                {
                    if (HeadLevel1.Equals("All"))
                    {
                        if (HeadLevel2.Equals("All"))
                        {
                            if (HeadLevel3.Equals("All"))
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                            else
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.MCHLevel3.ToUpper() == HeadLevel3.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                        }
                        else //Level2 has value
                        {
                            if (HeadLevel3.Equals("All"))
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.MCHLevel2.ToUpper() == HeadLevel2.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                            else //Level3 has value
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.MCHLevel2.ToUpper() == HeadLevel2.ToUpper() && sap.MCHLevel3.ToUpper() == HeadLevel3.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                        }
                    }
                    else //level1 has value
                    {
                        if (HeadLevel2.Equals("All"))
                        {
                            if (HeadLevel3.Equals("All"))
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.MCHLevel1.ToUpper() == HeadLevel1.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                            else //level3 has value
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.MCHLevel1.ToUpper() == HeadLevel1.ToUpper() && sap.MCHLevel3.ToUpper() == HeadLevel3.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                        }
                        else //level2 has value
                        {
                            if (HeadLevel3.Equals("All"))
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.MCHLevel1.ToUpper() == HeadLevel1.ToUpper() && sap.MCHLevel2.ToUpper() == HeadLevel2.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                            else
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.MCHLevel1.ToUpper() == HeadLevel1.ToUpper() && sap.MCHLevel2.ToUpper() == HeadLevel2.ToUpper()  && sap.MCHLevel3.ToUpper() == HeadLevel3.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                        }
                    }
                }
                else //countsheet has value
                {
                    if (HeadLevel1.Equals("All"))
                    {
                        if (HeadLevel2.Equals("All"))
                        {
                            if (HeadLevel3.Equals("All"))
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.PIDoc.ToUpper() == countSheet.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                            else
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.PIDoc.ToUpper() == countSheet.ToUpper() && sap.MCHLevel3.ToUpper() == HeadLevel3.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                        }
                        else //level2 has value
                        {
                            if (HeadLevel3.Equals("All"))
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.PIDoc.ToUpper() == countSheet.ToUpper() && sap.MCHLevel2.ToUpper() == HeadLevel2.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                            else
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.PIDoc.ToUpper() == countSheet.ToUpper() && sap.MCHLevel2.ToUpper() == HeadLevel2.ToUpper() && sap.MCHLevel3.ToUpper() == HeadLevel3.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                        }
                    }
                    else //level1 has value
                    {
                        if (HeadLevel2.Equals("All"))
                        {
                            if (HeadLevel3.Equals("All"))
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.PIDoc.ToUpper() == countSheet.ToUpper() && sap.MCHLevel1.ToUpper() == HeadLevel1.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                            else
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.PIDoc.ToUpper() == countSheet.ToUpper() && sap.MCHLevel1.ToUpper() == HeadLevel1.ToUpper() && sap.MCHLevel3.ToUpper() == HeadLevel3.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                        }
                        else //level2 has value
                        {
                            if (HeadLevel3.Equals("All"))
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.PIDoc.ToUpper() == countSheet.ToUpper() && sap.MCHLevel1.ToUpper() == HeadLevel1.ToUpper() && sap.MCHLevel2.ToUpper() == HeadLevel2.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();

                            }
                            else
                            {
                                listMCH = (from sap in dbContext.MastSAP_SKU
                                           where sap.PIDoc.ToUpper() == countSheet.ToUpper() && sap.MCHLevel1.ToUpper() == HeadLevel1.ToUpper() && sap.MCHLevel2.ToUpper() == HeadLevel2.ToUpper() && sap.MCHLevel3.ToUpper() == HeadLevel3.ToUpper()
                                           select sap.MaterialGroup).Distinct().ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                listMCH = null;
            }
            listMCH.Sort();
            return listMCH;
        }

        public List<string> GetDropDownCountSheetSKU(string plant)
        {
            List<string> listCountSheet = new List<string>();
            Entities dbContext = new Entities();

            try
            {
                if (plant.Equals("All"))
                {
                    listCountSheet = (from m in dbContext.MastSAP_SKU
                                      select m.PIDoc).Distinct().ToList();
                }
                else
                {
                    listCountSheet = (from m in dbContext.MastSAP_SKU
                                      where m.Plant.ToUpper().Equals(plant.ToUpper())
                                      select m.PIDoc).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                listCountSheet = null;
            }
            listCountSheet.Sort();
            return listCountSheet;
        }



        public List<string> GetDropDownAllCountSheetSKU()
        {
            List<string> listCountSheet = new List<string>();
            Entities dbContext = new Entities();

            try
            {
                    listCountSheet = (from m in dbContext.MastSAP_SKU
                                      select m.PIDoc).Distinct().ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                listCountSheet = null;
            }
            listCountSheet.Sort();
            return listCountSheet;
        }

        public List<string> GetAllPlant()
        {
            List<string> listPlant = new List<string>();
            Entities dbContext = new Entities();

            try
            {
                listPlant = (from m in dbContext.MasterPlants
                             select m.PlantCode).Distinct().ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                listPlant = null;
            }
            listPlant.Sort();
            return listPlant;
        }

        public List<string> GetPlant(string countsheet)
        {
            List<string> listPlant = new List<string>();
            Entities dbContext = new Entities();
            try
            {
                listPlant = (from m in dbContext.MastSAP_SKU
                             where m.PIDoc.ToUpper().Equals(countsheet.ToUpper())
                             select m.Plant).Distinct().ToList();
            }
            catch (Exception ex)
            {
                logBll.LogSystem(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message, DateTime.Now);
                listPlant = null;
            }
            listPlant.Sort();
            return listPlant;
        }
    }
}
