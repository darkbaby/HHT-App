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
    
    public partial class Location
    {
        public string LocationCode { get; set; }
        public string SectionCode { get; set; }
        public int ScanMode { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual Section Section { get; set; }
    }
}
