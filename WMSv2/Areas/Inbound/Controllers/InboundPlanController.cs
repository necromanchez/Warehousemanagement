using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMSv2.Areas.Inbound.Models;
using WMSv2.Controllers;
using static WMSv2.Controllers.SessionExpireController;

namespace WMSv2.Areas.Inbound.Controllers
{
    public class InboundPlanController : Controller
    {
        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];
        // GET: Inbound/Inbound

        [SessionExpireController]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetInboundPlanList()
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
                    where c.Is_Deleted == false && c.InboundStatus == "Not Received"
                    select new InboundPlanModel
                    {
                        ID = c.ID.ToString()
                        ,InboundPlanCode =c.InboundPlanCode
                        ,SiteCode =c.SiteCode
                        ,CodeOwner = c.CodeOwner
                        ,InboundPlanDate = c.InboundPlanDate
                        ,CodeSupplier = (from a in db.mCustomerSuppliers where a.CusSupCode == c.CodeSupplier select a.Name).FirstOrDefault()
                        ,CodeSupCode = c.CodeSupplier
                        ,SlipClass =c.SlipClass
                        ,SlipDate =c.SlipDate
                        ,SlipNo =c.SlipNo
                        ,CodeClassInboundCode = c.CodeClassInbound
                        ,CodeClassInbound =(from a in db.mCodes where a.Code == c.CodeClassInbound select a.CodeName).FirstOrDefault()
                        ,InboundStatus =c.InboundStatus
                        ,PrintStatus =c.PrintStatus
                        ,Remarks =c.Remarks
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
            //paging
            list = list.Skip(start).Take(length).ToList<InboundPlanModel>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateInboundPlan(INinboundPlan InboundPlan)
        {
            string DateID = DateTime.Now.ToString("yyyyMMdd");
            string gencode = string.Empty;
            string lastgencode = (from u in db.INinboundPlans.ToList()
                                  orderby u.InboundPlanCode descending
                                  select u.InboundPlanCode).FirstOrDefault();
            if (lastgencode == null)
            {
                gencode = DateID+"INB0000001";
            }
            else
            {
                gencode = lastgencode.Substring(11);
                int upId = Convert.ToInt32(gencode.TrimStart(new char[] { '0' })) + 1;
                gencode = DateID+string.Format("INB{0}", upId.ToString().PadLeft(7, '0'));
            }

            InboundPlan.PrintStatus = "Not Printed";
            InboundPlan.InboundStatus = "Not Received";
            InboundPlan.InboundPlanCode = gencode;
            InboundPlan.Is_Deleted = false;
            InboundPlan.Create_Date = DateTime.Now;
            InboundPlan.Create_User = user.UserID;
            InboundPlan.Update_Date = DateTime.Now;
            InboundPlan.Update_User = user.UserID;
            try {
                db.INinboundPlans.Add(InboundPlan);
                db.SaveChanges();
            }
            catch(Exception err) { }
          
               

            return Json(new { result = "success", InboundPlan= InboundPlan }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteInboundPlan(string InboundPlanCode)
        {
            INinboundPlan inboundplan = new INinboundPlan();
            inboundplan = (from u in db.INinboundPlans.ToList()
                    where u.InboundPlanCode == InboundPlanCode
                             select u).FirstOrDefault();
            inboundplan.Is_Deleted = true;
            inboundplan.Update_Date = DateTime.Now;
            inboundplan.Update_User = user.UserID;
            db.Entry(inboundplan).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditInboundPlan(INinboundPlan InboundPlan)
        {
            try
            {
                INinboundPlan inbound = new INinboundPlan();
                inbound = (from u in db.INinboundPlans.ToList()
                         where u.ID == InboundPlan.ID
                         select u).FirstOrDefault();
                inbound.InboundPlanDate = InboundPlan.InboundPlanDate;
                inbound.CodeSupplier = InboundPlan.CodeSupplier;
                inbound.SlipNo = InboundPlan.SlipNo;
                inbound.SlipClass = InboundPlan.SlipClass;
                inbound.SlipDate = InboundPlan.SlipDate;
                inbound.CodeClassInbound = InboundPlan.CodeClassInbound;
                inbound.Remarks = InboundPlan.Remarks;
                inbound.CodeOwner = InboundPlan.CodeOwner;

                inbound.Update_User = "up";
                inbound.Update_Date = DateTime.Now;

                INinboundPlan checker = (from c in db.INinboundPlans
                                 where c.InboundPlanCode == c.InboundPlanCode
                                    && inbound.InboundPlanDate == c.InboundPlanDate
                                    && inbound.CodeSupplier == c.CodeSupplier
                                    && inbound.SlipNo == c.SlipNo
                                    && inbound.SlipClass == c.SlipClass
                                    && inbound.SlipDate == c.SlipDate
                                    && inbound.CodeClassInbound == c.CodeClassInbound
                                    && inbound.Remarks == c.Remarks
                                    && inbound.CodeOwner == c.CodeOwner
                                    select c).FirstOrDefault();
                if (checker == null)
                {
                    db.Entry(inbound).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    return Json(new { result = "failed" }, JsonRequestBehavior.AllowGet);

                }


            }
            catch (Exception err) { }

            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ToCompleteInbound(string InboundPlanCode,string Result)
        {
            INinboundPlan plan = (from c in db.INinboundPlans where c.InboundPlanCode == InboundPlanCode select c).FirstOrDefault();
            plan.PrintStatus = "Printed";
            plan.InboundStatus = Result;
            db.Entry(plan).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result="success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerifyInboundItems(string InboundPlanCode)
        {
            bool oktocomplete = true;
            List<GET_InboundItemsinbound_Result> list = new List<GET_InboundItemsinbound_Result>();
            list = (db.GET_InboundItemsinbound(InboundPlanCode).ToList());
            List<GET_InboundItemsinbound_Result> notInbounded = list.Where(c => c.ActualReceived < c.SUMQty).ToList();
            if (notInbounded.Count > 0)
            {
                oktocomplete = false;
            }
            return Json(new {ok = oktocomplete }, JsonRequestBehavior.AllowGet);
        }
    }
}