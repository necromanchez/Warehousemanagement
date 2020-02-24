using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMSv2.Areas.Inbound.Models;
using static WMSv2.Controllers.SessionExpireController;

namespace WMSv2.Areas.Inbound.Controllers
{
    public class InboundResultController : Controller
    {
        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];
        // GET: Inbound/InboundResult
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetInboundResultList()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string columnsort = Request["order[0][column]"];
            string sortColumnName = Request["columns[" + columnsort + "][data]"];
            string sortDirection = Request["order[0][dir]"];
            List<InboundPlanModel> list = new List<InboundPlanModel>();
            list = (from c in db.INinboundPlans
                    where c.Is_Deleted == false && c.InboundStatus != "Not Received"
                    select new InboundPlanModel
                    {
                        ID = c.ID.ToString()
                        ,
                        InboundPlanCode = c.InboundPlanCode
                        ,
                        SiteCode = c.SiteCode
                        ,
                        CodeOwner = c.CodeOwner
                        ,
                        InboundPlanDate = c.InboundPlanDate
                        ,
                        CodeSupplier = (from a in db.mCustomerSuppliers where a.CusSupCode == c.CodeSupplier select a.Name).FirstOrDefault()
                        ,
                        CodeSupCode = c.CodeSupplier
                        ,
                        SlipClass = c.SlipClass
                        ,
                        SlipDate = c.SlipDate
                        ,
                        SlipNo = c.SlipNo
                        ,
                        CodeClassInboundCode = c.CodeClassInbound
                        ,
                        CodeClassInbound = (from a in db.mCodes where a.Code == c.CodeClassInbound select a.CodeName).FirstOrDefault()
                        ,
                        InboundStatus = c.InboundStatus
                        ,
                        PrintStatus = c.PrintStatus
                        ,
                        Remarks = c.Remarks
                    }).ToList<InboundPlanModel>();
            int totalrows = list.Count;
            int totalrowsafterfiltering = list.Count;
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                list = list.Where(x => x.InboundPlanCode.ToLower().Contains(searchValue.ToLower()) ||
                                  x.InboundPlanDate.ToString().Contains(searchValue.ToString()) ||
                                  x.CodeSupplier.ToLower().Contains(searchValue.ToLower()) ||
                                  x.SlipNo.ToLower().Contains(searchValue.ToLower()) ||
                                  x.SlipDate.ToString().Contains(searchValue.ToString()) ||
                                  x.CodeClassInbound.ToLower().Contains(searchValue.ToLower()) ||
                                  x.InboundStatus.ToLower().Contains(searchValue.ToLower()) ||
                                  x.PrintStatus.ToLower().Contains(searchValue.ToLower())).ToList<InboundPlanModel>();
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
            list = list.Skip(start).Take(length).ToList<InboundPlanModel>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
    }
}