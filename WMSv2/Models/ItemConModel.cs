using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSv2.Models
{
    public class ItemConModel
    {
        public string ItemCode { get; set; }
        public string FromUnitType { get; set; }
        public decimal FromUnit { get; set; }
        public string ToUnitType { get; set; }
        public decimal ToUnit { get; set; }
    }
}