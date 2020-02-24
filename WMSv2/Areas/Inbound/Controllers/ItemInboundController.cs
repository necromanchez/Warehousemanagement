using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMSv2.Models;
using static WMSv2.Controllers.SessionExpireController;

namespace WMSv2.Areas.Inbound.Controllers
{
    public class ItemInboundController : Controller
    {
        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];
        // GET: Inbound/ItemInbound
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetInboundPlanList(string InboundPlanCode)
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string columnsort = Request["order[0][column]"];
            string sortColumnName = Request["columns[" + columnsort + "][data]"];
            string sortDirection = Request["order[0][dir]"];
            List<GET_InboundItemsinbound_Result> list = new List<GET_InboundItemsinbound_Result>();
            list = (db.GET_InboundItemsinbound(InboundPlanCode).ToList());
            list = (from c in list where c.Result == "PARTIAL" select c).ToList();
            int totalrows = list.Count;
            int totalrowsafterfiltering = list.Count;
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                list = list.Where(x => x.ItemCode.ToLower().Contains(searchValue.ToLower()) ||
                                  x.ItemName.ToLower().Contains(searchValue.ToLower()) ||
                                  x.CodeName.ToLower().Contains(searchValue.ToLower()) ||
                                  x.PoNo.ToLower().Contains(searchValue.ToLower()) ||
                                  x.LotNo.ToLower().Contains(searchValue.ToLower()) ||
                                  x.QtyPerCase.ToString().Contains(searchValue.ToString()) ||
                                  x.QtyPerInnerCase.ToString().Contains(searchValue.ToString()) || 
                                  x.SUMQty.ToString().Contains(searchValue.ToString()) ||
                                  x.ActualReceived.ToString().Contains(searchValue.ToString()) ||
                                  x.QtyPerUnit.ToString().Contains(searchValue.ToString())).ToList<GET_InboundItemsinbound_Result>();
            }
            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortDirection == "asc")
                {
                    list = list.OrderBy(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();
                }
                else
                {
                    list = list.OrderByDescending(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();
                }
            }
            //paging
            list = list.Skip(start).Take(length).ToList<GET_InboundItemsinbound_Result>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInboundPlanList_inbounded(string InboundPlanCode)
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string columnsort = Request["order[0][column]"];
            string columntosort = Request["columns[" + columnsort + "][data]"];
            string sortDirection = Request["order[0][dir]"];
            List<GET_InboundItemsinboundSet_Result> list = new List<GET_InboundItemsinboundSet_Result>();
            list = (db.GET_InboundItemsinboundSet(InboundPlanCode).ToList());
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
            list = list.Skip(start).Take(length).ToList<GET_InboundItemsinboundSet_Result>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult CreateInboundItem(INinboundItem InboundItem)
        {
            
            InboundItem.Is_Deleted = false;
            InboundItem.Create_Date = DateTime.Now;
            InboundItem.Create_User = user.UserID;
            InboundItem.Update_Date = DateTime.Now;
            InboundItem.Update_User = user.UserID;
            try
            {
                db.INinboundItems.Add(InboundItem);
                db.SaveChanges();

            }
            catch (Exception err) { }



            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetInbound(INinboundSet InboundItem, STstockInventoryDetail stockdetails, ItemUnitModel itemunit)
        {
            InboundItem.Is_Deleted = false;
            InboundItem.Create_Date = DateTime.Now;
            InboundItem.Create_User = user.UserID;
            InboundItem.Update_Date = DateTime.Now;
            InboundItem.Update_User = user.UserID;
            string DateID = DateTime.Now.ToString("yyyyMMdd");
            string gencode = string.Empty;
            string lastgencode = (from u in db.INinboundSets.ToList()
                                  orderby u.InboundNoResult descending
                                  select u.InboundNoResult).FirstOrDefault();
            if (lastgencode == null)
            {
                gencode = DateID+"INR0000001";
            }
            else
            {
                gencode = lastgencode.Substring(11);
                int upId = Convert.ToInt32(gencode.TrimStart(new char[] { '0' })) + 1;
                gencode = DateID+string.Format("INR{0}", upId.ToString().PadLeft(7, '0'));
            }
            InboundItem.InboundNoResult = gencode;
            try
            {
                db.INinboundSets.Add(InboundItem);
                db.SaveChanges();
                
                string gencodes = string.Empty;
                string lastgencodes = (from u in db.STstockInventoryDetails.ToList()
                                      orderby u.StockDetailCode descending
                                      select u.StockDetailCode).FirstOrDefault();
                if (lastgencodes == null)
                {
                    gencodes = DateID+"STD0000001";
                }
                else
                {
                    gencodes = lastgencodes.Substring(11);
                    int upId = Convert.ToInt32(gencodes.TrimStart(new char[] { '0' })) + 1;
                    gencodes = DateID+string.Format("STD{0}", upId.ToString().PadLeft(7, '0'));
                }

                INinboundPlan inboundplan = (from c in db.INinboundPlans where c.InboundPlanCode == InboundItem.InboundPlanCode select c).FirstOrDefault();
                mItem itemcurrent_main = (from c in db.mItems where c.ItemCode == InboundItem.ItemCode select c).FirstOrDefault();
                INinboundItem inboundplanitem = (from c in db.INinboundItems where c.InboundPlanCode == InboundItem.InboundPlanCode && c.ItemCode == itemcurrent_main.ItemCode select c).FirstOrDefault();

                stockdetails.StockCode = (from c in db.STstockInventories where c.InboundPlanCode == InboundItem.InboundPlanCode select c.StockCode).FirstOrDefault();
                stockdetails.InboundPlanNo = inboundplan.InboundPlanCode;
                stockdetails.InboundNoResult = gencode;//Inbound result Gen code
                stockdetails.StockDetailCode = gencodes;//Stock Details Code
                stockdetails.ItemCode = inboundplanitem.ItemCode;
                stockdetails.ExpiredDate = inboundplanitem.ExpirationDate;
                stockdetails.InboundDate = inboundplan.InboundPlanDate;
                stockdetails.ActualStockQty = InboundItem.ActualReceived;
                stockdetails.AllocatedQty = 0;
                stockdetails.PickedQty = 0;
                stockdetails.LocationCode = InboundItem.Location;
                stockdetails.LocationSubCode = InboundItem.SubLocation;
                stockdetails.LotNo = inboundplanitem.LotNo;
                stockdetails.PONo = inboundplanitem.PoNo;
                stockdetails.SupplierCode = itemcurrent_main.CusSupCode;
                stockdetails.SupplierCode = itemcurrent_main.CusSupCode;
                stockdetails.OriginalItemCode = itemcurrent_main.ItemCode;
                stockdetails.CaseQty = itemunit.pqc_q;
                stockdetails.QtyPerCase = itemunit.QtyPerCase;
                stockdetails.InnerCaseQty = itemunit.qpic_q;
                stockdetails.InnerQtyPerCase = itemunit.QtyPerInnerCase;
                stockdetails.UnitQty = itemunit.QtyPerUnit;
                //stockdetails.NW = (itemunit.QtyPerCase + itemunit.QtyPerInnerCase + itemunit.QtyPerUnit) * itemcurrent_unit.Where(c => c.Type == "") sa sunod na to
                stockdetails.LastInboundDate = DateTime.Now;
                //stockdetails.LastOutboundDate = DateTime.Now;
                stockdetails.SlipClass = inboundplan.SlipClass;
                stockdetails.SlipDate = inboundplan.SlipDate;
                stockdetails.SlipNo = inboundplan.SlipNo;
                stockdetails.SlipRemarks = inboundplan.Remarks;
                stockdetails.Is_Deleted = false;
                stockdetails.Create_Date = DateTime.Now;
                stockdetails.Create_User = user.UserID;
                stockdetails.Update_Date = DateTime.Now;
                stockdetails.Update_User = user.UserID;
                db.STstockInventoryDetails.Add(stockdetails);
                db.SaveChanges();

            }
            catch (Exception err) { }
            return Json(new { result = "success", InboundItem = InboundItem }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InboundItemdelete(int ID)
        {

            INinboundItem Inbounditem = (from u in db.INinboundItems.ToList()
                           where u.ID == ID
                           select u).FirstOrDefault();
            Inbounditem.Is_Deleted = true;
            Inbounditem.Update_Date = DateTime.Now;
            Inbounditem.Update_User = user.UserID;
            db.Entry(Inbounditem).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDetails(string ItemCode, string Type)
        {
            mItemUnit unit = new mItemUnit();
            List<mItemUnit> list = new List<mItemUnit>();
            if (Type != "")
            {
                unit = (from c in db.mItemUnits where c.ItemCode == ItemCode && c.Type == Type select c).FirstOrDefault();
            }
            else
            {
                 list = (from c in db.mItemUnits where c.ItemCode == ItemCode select c).ToList();
            }
            return Json(new { result = "success", unit=unit, list=list }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetInboundItem(int ID) {
            
           
            INinboundItem item = (from c in db.INinboundItems where c.ID == ID select c).FirstOrDefault();
            DateTime PlanDate = (from c in db.INinboundPlans where c.InboundPlanCode == item.InboundPlanCode select c.InboundPlanDate).FirstOrDefault();
            INinboundSet set = (from c in db.INinboundSets where c.ItemCode == item.ItemCode && c.InboundPlanCode == item.InboundPlanCode select c).FirstOrDefault();
            return Json(new {item=item, set=set }, JsonRequestBehavior.AllowGet);
        }
    }
}