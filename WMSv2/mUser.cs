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
    
    public partial class mUser
    {
        public string UserID { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public bool Approved { get; set; }
        public bool Locked { get; set; }
        public int LoginAttempts { get; set; }
        public bool SuperUser { get; set; }
        public bool Reversal { get; set; }
        public bool IsManager { get; set; }
        public string SiteCode { get; set; }
        public System.DateTime LastDateLoggedIn { get; set; }
        public bool Is_Deleted { get; set; }
        public string Create_User { get; set; }
        public System.DateTime Create_Date { get; set; }
        public string Update_User { get; set; }
        public System.DateTime Update_Date { get; set; }
        public bool Customer_Flag { get; set; }
    }
}