//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FSBT_HHT_DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserReport
    {
        public UserReport()
        {
            this.UserGroupReportMaps = new HashSet<UserGroupReportMap>();
            this.UserReportConfigs = new HashSet<UserReportConfig>();
        }
    
        public string ReportCode { get; set; }
        public string ReportName { get; set; }
        public string ReportFile { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    
        public virtual ICollection<UserGroupReportMap> UserGroupReportMaps { get; set; }
        public virtual ICollection<UserReportConfig> UserReportConfigs { get; set; }
    }
}
