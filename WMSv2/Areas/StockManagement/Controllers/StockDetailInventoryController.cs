using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WMSv2.Areas.StockManagement.Controllers
{
    public class StockDetailInventoryController : Controller
    {
        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];
        // GET: StockManagement/StockDetailInventory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetStockDetailedInventoryList()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<GET_StockInboundDetailed_Result> list = new List<GET_StockInboundDetailed_Result>();
            list = (db.GET_StockInboundDetailed().ToList());
            int totalrows = list.Count;
            int totalrowsafterfiltering = list.Count;

            //paging
            list = list.Skip(start).Take(length).ToList<GET_StockInboundDetailed_Result>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

        }
    }
}