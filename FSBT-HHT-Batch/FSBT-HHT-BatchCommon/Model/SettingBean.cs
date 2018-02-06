using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_BatchCommon.Model
{
    public class SettingBean
    {
        public String sourcePath { get; set; }
        public String dataSourceRequest { get; set; }
        public String databaseRequest { get; set; }
        public String userIdRequest { get; set; }
        public String passwordRequest { get; set; }
        public String logPath { get; set; }
        public String logFileName { get; set; }
        public String timeout { get; set; }
        public Int32 sleeptime { get; set; }
        public String isWindownAuthen { get; set; }
    }

    public enum Status
    {
        ERROR = 0, INFO = 1
    }

}
