using FSBT_HHT_BatchCommon;
using FSBT_HHT_BatchCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_BatchCommon
{
    public class InitialSetting
    {
        public static SettingBean SetSettingBean()
        {
            Console.WriteLine("Enter to SetSettingBean");
            DateTime localDate = DateTime.Now;
            SettingBean settingBean = new SettingBean();
            String configFilePath = AppDomain.CurrentDomain.BaseDirectory + @"setting.ini";
            InitFile init = new InitFile(configFilePath);
            settingBean.sourcePath = init.InitReadValue("setting", "SourcePath");
            settingBean.dataSourceRequest = init.InitReadValue("database connection", "DataSourceRequest");
            settingBean.databaseRequest = init.InitReadValue("database connection", "DbNameRequest");
            settingBean.userIdRequest = init.InitReadValue("database connection", "UserIDRequest");
            settingBean.passwordRequest = init.InitReadValue("database connection", "PasswordRequest");
            settingBean.logPath = init.InitReadValue("Logfile", "Logpath");
            settingBean.logFileName = init.InitReadValue("Logfile", "LogName") + localDate.ToString("yyyyMM") + ".log";
            settingBean.timeout = init.InitReadValue("setting", "TimeOut");
            settingBean.sleeptime = Convert.ToInt32(init.InitReadValue("setting", "SleepTime")) * 1000;
            settingBean.isWindownAuthen = init.InitReadValue("database connection", "IsWindownAuthen");

            LogFile logfile = new LogFile(settingBean.logPath, settingBean.logFileName);
            return settingBean;
        }

        public static Boolean CheckSettingFile(SettingBean settingBean)
        {
            Boolean chk = true;
            if (String.IsNullOrWhiteSpace(settingBean.sourcePath)) chk = false;
            if (String.IsNullOrWhiteSpace(settingBean.dataSourceRequest)) chk = false;
            if (String.IsNullOrWhiteSpace(settingBean.databaseRequest)) chk = false;
            if (String.IsNullOrWhiteSpace(settingBean.userIdRequest)) chk = false;
            if (String.IsNullOrWhiteSpace(settingBean.passwordRequest)) chk = false;
            if (String.IsNullOrWhiteSpace(settingBean.logPath)) chk = false;
            if (String.IsNullOrWhiteSpace(settingBean.logFileName)) chk = false;
            if (String.IsNullOrWhiteSpace(settingBean.timeout)) chk = false;

            return chk;
        }

    }
}
