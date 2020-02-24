using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static WMSv2.Controllers.SessionExpireController;
using WMSv2.Controllers;

namespace WMSv2.Areas.MasterManagement.Controllers
{
    [SessionExpireController]
    public class SiteMasterController : Controller
    {
        // GET: MasterManagement/SiteMaster

        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetSiteList()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string columnsort = Request["order[0][column]"];//Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortColumnName = Request["columns["+columnsort+"][data]"];
            string sortDirection = Request["order[0][dir]"];

            List<mSite> list = new List<mSite>();
          
                list = db.mSites.Where(u => u.Is_Deleted == false).ToList<mSite>();
                int totalrows = list.Count;
                if (!string.IsNullOrEmpty(searchValue))//filter
                {
                    list = list.
                        Where(x => x.SiteName.ToLower().Contains(searchValue.ToLower()) || x.Address.ToLower().Contains(searchValue.ToLower())).ToList<mSite>();
                }
                int totalrowsafterfiltering = list.Count;
                //sorting
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
                //paging
                list = list.Skip(start).Take(length).ToList<mSite>();
                return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateSite(mSite site)
        {
            string gencode = string.Empty;
            string lastgencode = (from u in db.mSites.ToList()
                                  orderby u.SiteCode descending
                                  select u.SiteCode).FirstOrDefault();
            if (lastgencode == null)
            {
                gencode = "STS0001";
            }
            else
            {
                gencode = lastgencode.Substring(3);
                int upId = Convert.ToInt32(gencode.TrimStart(new char[] { '0' })) + 1;
                gencode = string.Format("STS{0}", upId.ToString().PadLeft(4, '0'));
            }


            site.SiteCode = gencode;
            site.Is_Deleted = false;
            site.Create_Date = DateTime.Now;
            site.Create_User = user.UserID;
            site.Update_Date = DateTime.Now;
            site.Update_User = user.UserID;


            mSite checker = (from c in db.mSites
                                     where c.SiteName == site.SiteName
                                     && c.Address == site.Address
                                     && c.Is_Deleted == false
                                     select c).FirstOrDefault();

            if (checker == null)
            {
                try
                {
                    db.mSites.Add(site);
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

        public ActionResult EditSite(mSite site)
        {
            try
            {
                mSite sites = new mSite();
                sites = (from u in db.mSites.ToList()
                           where u.SiteCode == site.SiteCode
                           select u).FirstOrDefault();
                sites.SiteName = site.SiteName;
                sites.Address = site.Address;

                site.Update_User = "up";
                site.Update_Date = DateTime.Now;

                mSite checker = (from c in db.mSites
                                 where c.SiteName == site.SiteName
                                 && c.Address == site.Address
                                 && c.Is_Deleted == false
                                 && c.SiteCode != site.SiteCode
                                 select c).FirstOrDefault();
                if (checker == null)
                {
                    db.Entry(sites).State = EntityState.Modified;
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

        public ActionResult DeleteSite(string SiteCode)
        {
            mSite site = new mSite();
            site = (from u in db.mSites.ToList()
                           where u.SiteCode == SiteCode
                    select u).FirstOrDefault();
            site.Is_Deleted = true;
            site.Update_Date = DateTime.Now;
            site.Update_User = user.UserID;
            db.Entry(site).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }
    }
}