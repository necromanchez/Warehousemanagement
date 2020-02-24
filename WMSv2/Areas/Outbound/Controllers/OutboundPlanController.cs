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
    public class OutboundPlanController : Controller
    {
        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];
        // GET: Outbound/OutboundPlan
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetOutboundPlanList()
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
                    where c.Is_Deleted == false && c.OutboundStatus == "Unprocessed"
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


        public ActionResult CreateOutboundPlan(OBoutboundPlan OutboundPlan)
        {
            string DateID = DateTime.Now.ToString("yyyyMMdd");
            string gencode = string.Empty;
            string lastgencode = (from u in db.OBoutboundPlans.ToList()
                                  orderby u.OutboundPlanNo descending
                                  select u.OutboundPlanNo).FirstOrDefault();
            if (lastgencode == null)
            {
                gencode = DateID+"OBP0000001";
            }
            else
            {
                gencode = lastgencode.Substring(11);
                int upId = Convert.ToInt32(gencode.TrimStart(new char[] { '0' })) + 1;
                gencode = DateID + string.Format("OBP{0}", upId.ToString().PadLeft(7, '0'));
            }


            OutboundPlan.OutboundPlanNo = gencode;
            OutboundPlan.Is_Deleted = false;
            OutboundPlan.Create_Date = DateTime.Now;
            OutboundPlan.Create_User = user.UserID;
            OutboundPlan.Update_Date = DateTime.Now;
            OutboundPlan.Update_User = user.UserID;
            OutboundPlan.OutboundStatus = "Unprocessed";
            try
            {
                db.OBoutboundPlans.Add(OutboundPlan);
                db.SaveChanges();
            }
            catch (Exception err) { }



            return Json(new { result = "success", OutboundPlan = OutboundPlan }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditOutboundPlan(OBoutboundPlan outboundPlan)
        {
            try
            {
                OBoutboundPlan outbound = new OBoutboundPlan();
                outbound = (from u in db.OBoutboundPlans.ToList()
                            where u.ID == outboundPlan.ID
                            select u).FirstOrDefault();
                outbound.CodeOwner = outboundPlan.CodeOwner;
                outbound.OutboundPlanDate = outboundPlan.OutboundPlanDate;
                outbound.UrgentFlagCode = outboundPlan.UrgentFlagCode;
                outbound.SlipDate = outboundPlan.SlipDate;
                outbound.SlipClass = outboundPlan.SlipClass;
                outbound.SlipNo = outboundPlan.SlipNo;
                outbound.OutboundClassCode = outboundPlan.OutboundClassCode;
                outbound.TransportClassCode = outboundPlan.TransportClassCode;
                outbound.DEClass = outboundPlan.DEClass;
                outbound.ShipToCode = outboundPlan.ShipToCode;
                outbound.ShipperCode = outboundPlan.ShipperCode;
                outbound.BuyerCode = outboundPlan.BuyerCode;
                outbound.ConsigneeCode = outboundPlan.ConsigneeCode;
                outbound.DeliveryPlanDate = outboundPlan.DeliveryPlanDate;
                outbound.ShipoutDate = outboundPlan.ShipoutDate;
                outbound.SlipRemarks = outboundPlan.SlipRemarks;


                outbound.Update_User = "up";
                outbound.Update_Date = DateTime.Now;

                db.Entry(outbound).State = EntityState.Modified;
                db.SaveChanges();

            }
            catch (Exception err) { }

            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteOutoundPlan(string OutboundPlanNo)
        {
            OBoutboundPlan outboundplan = new OBoutboundPlan();
            outboundplan = (from u in db.OBoutboundPlans.ToList()
                            where u.OutboundPlanNo == OutboundPlanNo
                            select u).FirstOrDefault();
            outboundplan.Is_Deleted = true;
            outboundplan.Update_Date = DateTime.Now;
            outboundplan.Update_User = user.UserID;
            db.Entry(outboundplan).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Allocate_Outbound(string OutboundPlanNo)
        {
            string DateID = DateTime.Now.ToString("yyyyMMdd");
            string gencode = string.Empty;
            string lastgencode = (from u in db.OBpickings.ToList()
                                  orderby u.OutboundBatchNo descending
                                  select u.OutboundBatchNo).FirstOrDefault();
            if (lastgencode == null)
            {
                gencode = DateID + "BTC0000001";
            }
            else
            {
                gencode = lastgencode.Substring(11);
                int upId = Convert.ToInt32(gencode.TrimStart(new char[] { '0' })) + 1;
                gencode = DateID + string.Format("BTC{0}", upId.ToString().PadLeft(7, '0'));
            }
            OBoutboundPlan outboundplan = new OBoutboundPlan();
            outboundplan = (from u in db.OBoutboundPlans.ToList()
                            where u.OutboundPlanNo == OutboundPlanNo
                            select u).FirstOrDefault();
            //outbound Item
            List<OBoutboundItem> outbound_itemlist = (from ob in db.OBoutboundItems where ob.OutBoundPlanCode == outboundplan.OutboundPlanNo select ob).ToList();


            foreach (OBoutboundItem item in outbound_itemlist)
            {
                List<STstockInventoryDetail> stockdetail_list = (from std in db.STstockInventoryDetails
                                                                 where std.ItemCode == item.ItemCode
                                                                 orderby std.Update_Date ascending
                                                                 select std).ToList();
                decimal? Case = (item.QtyPerCase == null) ? 0 : item.QtyPerCase;
                decimal? InnerCase = (item.QtyPerInnerCase == null) ? 0 : item.QtyPerInnerCase;
                decimal? Unit = (item.QtyPerUnit == null) ? 0 : item.QtyPerUnit;
                decimal? outboundItemSUM = Case + InnerCase + Unit;
                OBpicking picking = new OBpicking();
                #region for Picking
                picking.OutboundBatchNo = gencode;
                picking.OutboundPlanNo = item.OutBoundPlanCode;
                picking.OutboundDetailNo = item.OutboundItemCode;
                picking.ItemQty = outboundItemSUM;
                picking.ActualQty = 0;
                picking.Is_Deleted = false;
                picking.Create_Date = DateTime.Now;
                picking.Create_User = user.UserID;
                picking.Update_Date = DateTime.Now;
                picking.Update_User = user.UserID;
                db.OBpickings.Add(picking);
                db.SaveChanges();
                #endregion

                #region ALLOCATION COMPUTE
                //foreach (STstockInventoryDetail inv in stockdetail_list)
                //{
                //    OBallocation allocation = new OBallocation();
                  
                //    STstockInventoryDetail stockdetails = new STstockInventoryDetail();
                //    stockdetails = (from c in db.STstockInventoryDetails
                //                    where c.ActualStockQty > c.AllocatedQty
                //                    select c).FirstOrDefault();
                //    if (outboundItemSUM != 0)
                //    {
                //        #region for Allocation
                //        allocation.OutboundPlanNo = item.OutBoundPlanCode;
                //        allocation.OutboundDetailNo = item.OutboundItemCode;
                //        allocation.StockCode = inv.StockCode;
                //        allocation.StockDetailCode = inv.StockDetailCode;
                //        allocation.QtyCase = inv.CaseQty;
                //        allocation.QtyInnerCase = inv.InnerCaseQty;
                //        allocation.QtyUnit = inv.UnitQty;

                //        if (inv.ActualStockQty - outboundItemSUM > 0)
                //        {
                //            allocation.ItemQty = outboundItemSUM;
                //            stockdetails.AllocatedQty = stockdetails.AllocatedQty + outboundItemSUM;
                //            outboundItemSUM = 0;
                //        }
                //        else
                //        {
                //            allocation.ItemQty = inv.ActualStockQty;
                //            stockdetails.AllocatedQty = inv.ActualStockQty;
                //            outboundItemSUM = outboundItemSUM - inv.ActualStockQty;
                //        }


                //        allocation.Is_Deleted = false;
                //        allocation.Create_Date = DateTime.Now;
                //        allocation.Create_User = user.UserID;
                //        allocation.Update_Date = DateTime.Now;
                //        allocation.Update_User = user.UserID;

                //        db.OBallocations.Add(allocation);
                //        db.SaveChanges();
                //        #endregion

                      
                //        stockdetails.Update_User = user.UserID;
                //        stockdetails.Update_Date = DateTime.Now;
                //        db.Entry(stockdetails).State = EntityState.Modified;
                //        db.SaveChanges();
                //    }
                //}
                #endregion

            }
            outboundplan.Update_Date = DateTime.Now;
            outboundplan.Update_User = user.UserID;
            outboundplan.OutboundStatus = "Allocated";
            db.Entry(outboundplan).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }
    }
}