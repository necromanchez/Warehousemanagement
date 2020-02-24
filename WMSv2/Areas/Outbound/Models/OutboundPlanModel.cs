using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSv2.Areas.Outbound.Models
{
    public class OutboundPlanModel
    {
        public long ID { get; set; }
        public string OutboundPlanNo { get; set; }
        public string CodeOwner { get; set; }
        public string CodeOwner2 { get; set; }
        public DateTime OutboundPlanDate { get; set; }
        public DateTime DeliveryPlanDate { get; set; }
        public DateTime ShipoutDate { get; set; }
        public string ShipToCode { get; set; }
        public string ShipToCode2 { get; set; }
        public string SlipNo { get; set; }
        public string SlipClass { get; set; }
        public DateTime SlipDate { get; set; }
        public string OutboundClassCode { get; set; }
        public string OutboundClassCode2 { get; set; }
        public string OutboundStatus { get; set; }
        public string UrgentFlagCode { get; set; }
        public string TransportClassCode { get; set; }
        public string DEClass { get; set; }
        public string ShipperCode { get; set; }
        public string BuyerCode { get; set; }
        public string ConsigneeCode { get; set; }
        public string SlipRemarks { get; set; }
        

        

    }
}