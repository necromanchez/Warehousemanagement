using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WMSv2.Areas.StockManagement.Controllers
{
    public class StockInventoryController : Controller
    {
        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];
        // GET: StockManagement/StockInventory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetStockInventoryList()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<GET_StockInbound_Result> list = new List<GET_StockInbound_Result>();
            list = (db.GET_StockInbound().ToList());
            int totalrows = list.Count;
            int totalrowsafterfiltering = list.Count;
            
            //paging
            list = list.Skip(start).Take(length).ToList<GET_StockInbound_Result>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateStockInventory(INinboundPlan inboundplan)
        {
            STstockInventory StockInventory = new STstockInventory();
            StockInventory.OwnerCode = (from c in db.INinboundPlans where c.InboundPlanCode == inboundplan.InboundPlanCode select c.CodeOwner).FirstOrDefault();
            string DateID = DateTime.Now.ToString("yyyyMMdd");
            string gencode = string.Empty;
            string lastgencode = (from u in db.STstockInventories.ToList()
                                  orderby u.StockCode descending
                                  select u.StockCode).FirstOrDefault();
            if (lastgencode == null)
            {
                gencode = DateID+"STK0000001";
            }
            else
            {
                gencode = lastgencode.Substring(11);
                int upId = Convert.ToInt32(gencode.TrimStart(new char[] { '0' })) + 1;
                gencode = DateID+string.Format("STK{0}", upId.ToString().PadLeft(7, '0'));
            }
            StockInventory.InboundPlanCode = inboundplan.InboundPlanCode;
            StockInventory.SiteCode = inboundplan.SiteCode;
            StockInventory.StockCode = gencode;
            StockInventory.Is_Deleted = false;
            StockInventory.Create_Date = DateTime.Now;
            StockInventory.Create_User = user.UserID;
            StockInventory.Update_Date = DateTime.Now;
            StockInventory.Update_User = user.UserID;
            try
            {
                db.STstockInventories.Add(StockInventory);
                db.SaveChanges();
            }
            catch (Exception err) { }



            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }
    }
}