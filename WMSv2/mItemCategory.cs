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
    
    public partial class mItemCategory
    {
        public long ID { get; set; }
        public string ItemCode { get; set; }
        public string Category { get; set; }
        public string Maker { get; set; }
        public Nullable<decimal> MOQ { get; set; }
        public Nullable<decimal> SPQ { get; set; }
        public string LTpocoverage { get; set; }
        public string SPpocoverage { get; set; }
        public string Prodfamily { get; set; }
        public string PlanDT { get; set; }
        public Nullable<bool> serialno { get; set; }
        public Nullable<bool> imei1 { get; set; }
        public Nullable<bool> imei2 { get; set; }
        public Nullable<bool> stpoint { get; set; }
        public Nullable<bool> ltpoint { get; set; }
        public string remarks { get; set; }
        public bool Is_Deleted { get; set; }
        public string Create_User { get; set; }
        public System.DateTime Create_Date { get; set; }
        public string Update_User { get; set; }
        public System.DateTime Update_Date { get; set; }
    }
}
