using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_Model
{
    public class PermissionModel
    {
        public string ScreenID { get; set; }
        public string ScreenName { get; set; }
        public string MenuName { get; set; }
        public bool Visible { get; set; }
    }

    public class PermissionComponentModel
    {
        public string ScreenID { get; set; }
        public string ComponentName { get; set; }
        public string ComponentType { get; set; }
        public string ComponentAlias { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool Enable { get; set; }
        public bool Visible { get; set; }
    }
}
