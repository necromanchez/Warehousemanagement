using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static WMSv2.Controllers.SessionExpireController;

namespace WMSv2.Areas.Outbound.Controllers
{
    public class OutboundItemController : Controller
    {
        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];
        // GET: Outbound/OutboundItem
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetOutboundItemList(string OutboundPlanCode)
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string columnsort = Request["order[0][column]"];
            string columntosort = Request["columns[" + columnsort + "][data]"];
            string sortDirection = Request["order[0][dir]"];
            List<GET_OutboundItems_Result> list = new List<GET_OutboundItems_Result>();
            list = (db.GET_OutboundItems(OutboundPlanCode).ToList());
           
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
            list = list.Skip(start).Take(length).ToList<GET_OutboundItems_Result>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateOutboundItem(OBoutboundItem OutboundItem)
        {
            string gencode = string.Empty;
            string lastgencode = (from u in db.OBoutboundItems.ToList()
                                  where u.OutBoundPlanCode == OutboundItem.OutBoundPlanCode
                                  orderby u.OutboundItemCode descending
                                  select u.OutboundItemCode).FirstOrDefault();
            if (lastgencode == null)
            {
                gencode = "OBI0001";
            }
            else
            {
                gencode = lastgencode.Substring(3);
                int upId = Convert.ToInt32(gencode.TrimStart(new char[] { '0' })) + 1;
                gencode = string.Format("OBI{0}", upId.ToString().PadLeft(4, '0'));
            }
            OutboundItem.OutboundItemCode = gencode;
            OutboundItem.Is_Deleted = false;
            OutboundItem.Create_Date = DateTime.Now;
            OutboundItem.Create_User = user.UserID;
            OutboundItem.Update_Date = DateTime.Now;
            OutboundItem.Update_User = user.UserID;
            try
            {
                db.OBoutboundItems.Add(OutboundItem);
                db.SaveChanges();

            }
            catch (Exception err) { }



            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOutboundItem(int ID)
        {
            OBoutboundItem item = (from c in db.OBoutboundItems where c.ID == ID select c).FirstOrDefault();
            return Json(new { item = item }, JsonRequestBehavior.AllowGet);
        }
    }
}