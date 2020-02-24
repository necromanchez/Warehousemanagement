using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSv2.Models
{
    public class ItemUnitModel
    {
        public decimal pqc_q { get; set; }
        public decimal qpic_q { get; set; }

        public decimal QtyPerUnit { get; set; }

        public decimal QtyPerCase { get; set; }

        public decimal QtyPerInnerCase { get; set; }
        
    }
}