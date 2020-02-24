using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSv2.Areas.Inbound.Models
{
    public class InboundPlanModel
    {
        public string ID { get; set; }
        public string InboundPlanCode { get; set; }
        public string SiteCode { get; set; }
        public string CodeOwner { get; set; }
        public DateTime InboundPlanDate { get; set; }
        public string CodeSupCode { get; set; }
        public string CodeSupplier { get; set; }
        public string SlipClass { get; set; }
        public DateTime SlipDate { get; set; }
        public string SlipNo { get; set; }
        public string CodeClassInboundCode { get; set; }
        public string CodeClassInbound { get; set; }
        public string InboundStatus { get; set; }
        public string PrintStatus { get; set; }
        public string Remarks{get;set;}
}
}