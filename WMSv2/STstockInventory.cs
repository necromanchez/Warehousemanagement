//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WMSv2
{
    using System;
    using System.Collections.Generic;
    
    public partial class STstockInventory
    {
        public long ID { get; set; }
        public string StockCode { get; set; }
        public string SiteCode { get; set; }
        public string OwnerCode { get; set; }
        public string InboundPlanCode { get; set; }
        public bool Is_Deleted { get; set; }
        public string Create_User { get; set; }
        public System.DateTime Create_Date { get; set; }
        public string Update_User { get; set; }
        public System.DateTime Update_Date { get; set; }
    }
}