using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMSv2.Models;
using static WMSv2.Controllers.SessionExpireController;
using WMSv2.Controllers;

namespace WMSv2.Areas.MasterManagement.Controllers
{
    [SessionExpireController]
    public class CodeController : Controller
    {
        // GET: MasterManagement/Code
        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];
        public ActionResult Index()
        {
            return View();
        }

       

        public ActionResult GetCodeList()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<CodeModel> list = new List<CodeModel>();
            list = (from c in db.mCodes
                    join d in db.mCodeBacks on c.Code.Substring(0, 3) equals d.Code
                    where c.Is_Deleted == false
                    select new CodeModel
                    {
                        Code = c.Code
                        ,CodeGroup = d.CodeName
                        ,CodeName = c.CodeName
                        ,CodeDescription = c.CodeDescription
                    }).ToList();

            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                list = list.Where(x => x.CodeGroup.ToLower().Contains(searchValue.ToLower()) ||
                                  x.CodeName.ToLower().Contains(searchValue.ToLower()) ||
                                  x.CodeDescription.ToLower().Contains(searchValue.ToLower())
                                  ).ToList<CodeModel>();
            }
            if (sortColumnName != ""  &&  sortColumnName != null)
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
            //list = db.mCodes.ToList<mCode>();
            int totalrows = list.Count;
            int totalrowsafterfiltering = list.Count;
         

            //paging
            list = list.Skip(start).Take(length).ToList<CodeModel>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateCode(mCode code, string cc)
        {
            string gencode = string.Empty;
            string lastgencode = (from u in db.mCodes.ToList()
                                  .Where(u=>u.Code.Contains(cc))
                                  orderby u.Code descending
                                  select u.Code).FirstOrDefault();
            if (lastgencode == null)
            {
                switch (cc)
                {
                    case "STC":
                        gencode = "STC0001";
                        break;
                    case "INC":
                        gencode = "INC0001";
                        break;
                    case "INS":
                        gencode = "INS0001";
                        break;
                    case "CAC":
                        gencode = "CAC0001";
                        break;
                    case "CLS":
                        gencode = "CLS0001";
                        break;
                    case "OST":
                        gencode = "OST0001";
                        break;
                    case "STU":
                        gencode = "STU0001";
                        break;
                    case "OBC":
                        gencode = "OBC0001";
                        break;
                    case "OBT":
                        gencode = "OBT0001";
                        break;
                    case "OWN":
                        gencode = "OWN0001";
                        break;
                    case "CAP":
                        gencode = "CAP0001";
                        break;
                }
                
            }
            else
            {
                gencode = lastgencode.Substring(3);
                int upId = Convert.ToInt32(gencode.TrimStart(new char[] { '0' })) + 1;
                gencode = string.Format(cc+"{0}", upId.ToString().PadLeft(4, '0'));
            }


            code.Code = gencode;
            code.Is_Deleted = false;
            code.Create_Date = DateTime.Now;
            code.Create_User = user.UserID;
            code.Update_Date = DateTime.Now;
            code.Update_User = user.UserID;


            mCode checker = (from c in db.mCodes
                             where c.Code == code.Code
                             && c.CodeDescription == code.CodeDescription
                             && c.Is_Deleted == false
                             select c).FirstOrDefault();

            if (checker == null)
            {
                try
                {
                    db.mCodes.Add(code);
                    db.SaveChanges();
                }
                catch (Exception err) { }

            }
            else
            {
                return Json(new { result = "failed" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditCode(mCode code)
        {
            try
            {
                mCode codes = new mCode();
                codes = (from u in db.mCodes.ToList()
                         where u.Code == code.Code
                         select u).FirstOrDefault();
                codes.CodeName = code.CodeName;
                codes.CodeDescription = code.CodeDescription;

                codes.Update_User = "up";
                codes.Update_Date = DateTime.Now;

                mCode checker = (from c in db.mCodes
                                 where c.CodeDescription == code.CodeDescription
                                 && c.CodeName == code.CodeName
                                 && c.Is_Deleted == false
                                 select c).FirstOrDefault();
                if (checker == null)
                {
                    db.Entry(codes).State = EntityState.Modified;
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

        public ActionResult DeleteCode(string Code)
        {
            mCode code = new mCode();
            code = (from u in db.mCodes.ToList()
                    where u.Code == Code
                    select u).FirstOrDefault();
            code.Is_Deleted = true;
            code.Update_Date = DateTime.Now;
            code.Update_User = user.UserID;
            db.Entry(code).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }
    }
}