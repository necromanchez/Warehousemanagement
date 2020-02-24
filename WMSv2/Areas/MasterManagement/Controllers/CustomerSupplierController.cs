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
    public class CustomerSupplierController : Controller
    {
        // GET: MasterManagement/CustomerSupplier
        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCusSupList()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<CustomerModel> list = new List<CustomerModel>();

            //list = db.mCustomerSuppliers.Where(u => u.Is_Deleted == false).ToList<mCustomerSupplier>();
            list = (from c in db.mCustomerSuppliers
                    where c.Is_Deleted == false
                    select new CustomerModel
                    {
                        SiteCode = c.SiteCode
                        ,CusSupCode = c.CusSupCode
                        ,Name = c.Name
                        ,Class = (from d in db.mCodeBacks where c.Class == d.Code select d.CodeName).FirstOrDefault()
                        ,Address = c.Address
                        ,Contact = c.Contact
                        ,EmailAddress = c.EmailAddress
,
                    }).ToList();

            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                list = list.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()) ||
                                  x.Class.ToLower().Contains(searchValue.ToLower()) ||
                                  x.EmailAddress.ToLower().Contains(searchValue.ToLower()) ||
                                  x.Contact.ToLower().Contains(searchValue.ToLower()) ||
                                  x.Address.ToLower().Contains(searchValue.ToLower())
                                  ).ToList<CustomerModel>();
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
            int totalrows = list.Count;
            int totalrowsafterfiltering = list.Count;
            list = list.Skip(start).Take(length).ToList<CustomerModel>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateCusSup(mCustomerSupplier cussup)
        {
            
            string gencode = string.Empty;
            string lastgencode = (from u in db.mCustomerSuppliers.ToList()
                                  orderby u.CusSupCode descending
                                  select u.CusSupCode).FirstOrDefault();
            if (lastgencode == null)
            {
                gencode = "CUS0001";
            }
            else
            {
                gencode = lastgencode.Substring(3);
                int upId = Convert.ToInt32(gencode.TrimStart(new char[] { '0' })) + 1;
                gencode = string.Format("CUS{0}", upId.ToString().PadLeft(4, '0'));
            }
          
            cussup.CusSupCode = gencode;
            cussup.Is_Deleted = false;
            cussup.Create_Date = DateTime.Now;
            cussup.Create_User = user.UserID;
            cussup.Update_Date = DateTime.Now;
            cussup.Update_User = user.UserID;


            mCustomerSupplier checker = (from c in db.mCustomerSuppliers
                                 where c.Class == cussup.Class
                                 && c.Name == cussup.Name
                                 && c.Address == cussup.Address
                                 && c.Contact == cussup.Contact
                                 && c.EmailAddress == cussup.EmailAddress
                                 && c.SiteCode == cussup.SiteCode
                                 && c.Is_Deleted == false
                                 select c).FirstOrDefault();

            if (checker == null)
            {
                try
                {
                    db.mCustomerSuppliers.Add(cussup);
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

        public ActionResult EditCusSup(mCustomerSupplier cussup)
        {
            try
            {
                mCustomerSupplier CusSup = new mCustomerSupplier();
                CusSup = (from u in db.mCustomerSuppliers.ToList()
                            where u.CusSupCode == cussup.CusSupCode
                            select u).FirstOrDefault();

                CusSup.Class = cussup.Class;
                CusSup.Name = cussup.Name;
                CusSup.Address = cussup.Address;
                CusSup.Contact = cussup.Contact;
                CusSup.EmailAddress = cussup.EmailAddress;
                CusSup.Update_User = "up";
                CusSup.Update_Date = DateTime.Now;

                mCustomerSupplier checker = (from c in db.mCustomerSuppliers
                                             where c.Class == cussup.Class
                                             && c.Name == cussup.Name
                                             && c.Address == cussup.Address
                                             && c.Contact == cussup.Contact
                                             && c.EmailAddress == cussup.EmailAddress
                                             && c.SiteCode == cussup.SiteCode
                                             && c.Is_Deleted == false
                                             select c).FirstOrDefault();
                if (checker == null)
                {
                    db.Entry(CusSup).State = EntityState.Modified;
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

        public ActionResult DeleteCusSup(string CusSupCode)
        {
            mCustomerSupplier cussup = new mCustomerSupplier();
            cussup = (from u in db.mCustomerSuppliers.ToList()
                        where u.CusSupCode == CusSupCode
                      select u).FirstOrDefault();
            cussup.Is_Deleted = true;
            cussup.Update_Date = DateTime.Now;
            cussup.Update_User = user.UserID;
            db.Entry(cussup).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

       
    }
}