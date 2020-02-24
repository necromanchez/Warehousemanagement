using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSv2.Models
{
    public class ItemPriceModel
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Currency { get; set; }
        public string UnitPrice { get; set; }
    }
}