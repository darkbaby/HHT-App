using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_Model
{
    public class ViewBackupHistoryModel
    {
        public int BackupID { get; set; }
        public string BackupName { get; set; }
        public DateTime BackupDate { get; set; }
        public TimeSpan BackupTime { get; set; }
        public string BackupBy { get; set; }

    }

    public class BackupHistoryModel : ViewBackupHistoryModel
    {
        public bool Deleted { get; set; }
        public DateTime DeleteDate { get; set; }
        public TimeSpan DeleteTime { get; set; }
        public string DeleteBy { get; set; }
    }

}
