using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMSv2.Areas.Outbound.Models;
using static WMSv2.Controllers.SessionExpireController;

namespace WMSv2.Areas.Outbound.Controllers
{
    public class OutboundSummaryController : Controller
    {
        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];
        // GET: Outbound/OutboundSummary
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetOutboundList()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string columnsort = Request["order[0][column]"];
            string columntosort = Request["columns[" + columnsort + "][data]"];
            string sortDirection = Request["order[0][dir]"];
            List<OutboundPlanModel> list = new List<OutboundPlanModel>();
            list = (from c in db.OBoutboundPlans
                    where c.Is_Deleted == false
                    select new OutboundPlanModel
                    {
                        ID = c.ID
                        ,
                        OutboundPlanNo = c.OutboundPlanNo
                        ,
                        CodeOwner = (from d in db.mCustomerSuppliers where d.CusSupCode == c.CodeOwner select d.Name).FirstOrDefault()
                        ,
                        CodeOwner2 = c.CodeOwner
                        ,
                        OutboundPlanDate = c.OutboundPlanDate
                        ,
                        DeliveryPlanDate = c.DeliveryPlanDate
                        ,
                        ShipoutDate = c.ShipoutDate
                        ,
                        ShipToCode2 = c.ShipToCode
                        ,
                        ShipToCode = (from d in db.mCustomerSuppliers where d.CusSupCode == c.ShipToCode select d.Name).FirstOrDefault()
                        ,
                        SlipNo = c.SlipNo
                        ,
                        SlipClass = c.SlipClass
                        ,
                        SlipDate = c.SlipDate
                        ,
                        OutboundClassCode = (from d in db.mCodes where d.Code == c.OutboundClassCode select d.CodeName).FirstOrDefault()
                        ,
                        OutboundClassCode2 = c.OutboundClassCode
                        ,
                        OutboundStatus = c.OutboundStatus
                        ,
                        UrgentFlagCode = c.UrgentFlagCode
                        ,
                        TransportClassCode = c.TransportClassCode
                        ,
                        DEClass = c.DEClass
                        ,
                        ShipperCode = c.ShipperCode
                        ,
                        BuyerCode = c.BuyerCode
                        ,
                        ConsigneeCode = c.ConsigneeCode
                        ,
                        SlipRemarks = c.SlipRemarks
                    }).ToList();
            int totalrows = list.Count;
            int totalrowsafterfiltering = list.Count;
            if (sortDirection == "asc")
            {
                list = list.OrderBy(x => TypeHelper.GetPropertyValue(x, columntosort)).ToList();
            }
            else
            {
                list = list.OrderByDescending(x => TypeHelper.GetPropertyValue(x, columntosort)).ToList();
            }
            //paging
            list = list.Skip(start).Take(length).ToList<OutboundPlanModel>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
    }
}