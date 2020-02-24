using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMSv2.Areas.Outbound.Models;
using static WMSv2.Controllers.SessionExpireController;

namespace WMSv2.Areas.Outbound.Controllers
{
    public class PickedController : Controller
    {
        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];
        // GET: Outbound/Picked
        public ActionResult Index()
        {
            return View();
        }

     

        public ActionResult GetPickedList()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string columnsort = Request["order[0][column]"];
            string columntosort = "ID";//Request["columns[0][data]"];
            string sortDirection = Request["order[0][dir]"];
            List<OutboundPlanModel> list = new List<OutboundPlanModel>();
            list = (from c in db.OBoutboundPlans
                    where c.Is_Deleted == false && c.OutboundStatus == "Allocated"
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
        
        public ActionResult SetPicked(long PickID, int ActualpickQty, string OutboundPlanNo)
        {
            OBpicking picking = new OBpicking();
            picking = (from c in db.OBpickings where c.ID == PickID select c).FirstOrDefault();
            picking.ActualQty = ActualpickQty;
            db.Entry(picking).State = EntityState.Modified;
            db.SaveChanges();
            
            OBoutboundPlan outboundplan = new OBoutboundPlan();
            outboundplan = (from u in db.OBoutboundPlans.ToList()
                            where u.OutboundPlanNo == OutboundPlanNo
                            select u).FirstOrDefault();
            //outbound Item
            List<OBoutboundItem> outbound_itemlist = (from ob in db.OBoutboundItems where ob.OutBoundPlanCode == outboundplan.OutboundPlanNo select ob).ToList();


            foreach (OBoutboundItem item in outbound_itemlist)
            {
                List<STstockInventoryDetail> stockdetail_list = (from std in db.STstockInventoryDetails
                                                                 join al in db.OBallocations 
                                                                 on new { X1 = std.StockCode, X2 = std.StockDetailCode } 
                                                                 equals new { X1 = al.StockCode, X2 = al.StockDetailCode }
                                                                 where std.ItemCode == item.ItemCode
                                                                 && al.OutboundPlanNo == item.OutBoundPlanCode
                                                                 orderby std.Update_Date ascending
                                                                 select std).ToList();

                foreach (STstockInventoryDetail inv in stockdetail_list) //Reset Picked Qty
                {
                    inv.PickedQty = 0;
                    //db.Entry(inv).State = EntityState.Modified;
                    //db.SaveChanges();
                }
                decimal? outboundItemSUM = ActualpickQty;
                foreach (STstockInventoryDetail inv in stockdetail_list)
                {
                    if (outboundItemSUM != 0)
                    {
                        if (inv.ActualStockQty - outboundItemSUM > 0)
                        {
                            inv.PickedQty = outboundItemSUM;
                            outboundItemSUM = 0;
                        }
                        else
                        {
                            inv.PickedQty = inv.AllocatedQty;
                            outboundItemSUM = outboundItemSUM - inv.AllocatedQty;
                        }
                        db.Entry(inv).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

            }
            outboundplan.Update_Date = DateTime.Now;
            outboundplan.Update_User = user.UserID;
            db.Entry(outboundplan).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetOutboundPicked_ItemList(string OutboundPlanCode)
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string columnsort = Request["order[0][column]"];
            string columntosort = Request["columns[" + columnsort + "][data]"];
            string sortDirection = Request["order[0][dir]"];
            List<GET_Outbound_PickedItems_Result> list = new List<GET_Outbound_PickedItems_Result>();
            list = (db.GET_Outbound_PickedItems(OutboundPlanCode).ToList());

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
            list = list.Skip(start).Take(length).ToList<GET_Outbound_PickedItems_Result>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PickedOutboundPlan(List<string> OutboundPlanNoList)
        {
            foreach(string i in OutboundPlanNoList)
            {
                OBoutboundPlan outboud_plan = new OBoutboundPlan();
                outboud_plan = (from c in db.OBoutboundPlans where c.OutboundPlanNo == i select c).FirstOrDefault();
                outboud_plan.OutboundStatus = "Loading";
                db.Entry(outboud_plan).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }
    }
}