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
    
    public partial class TempFileRegularPriceDPError
    {
        public int ID { get; set; }
        public string Plant { get; set; }
        public string SKU { get; set; }
        public string UPC { get; set; }
        public string Price { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string TempFileName { get; set; }
        public string Error { get; set; }
        public int URNO { get; set; }
    }
}
