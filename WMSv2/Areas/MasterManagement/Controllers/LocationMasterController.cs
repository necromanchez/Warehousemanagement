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
    public class LocationMasterController : Controller
    {
        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];
        // GET: MasterManagement/Location
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetLocationList()
        {
            //List<mLocation> list = new List<mLocation>();
            //list = (from c in db.mLocations where c.Is_Deleted == false select c).ToList();
            //return Json(new { list = list }, JsonRequestBehavior.AllowGet);
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<mLocation> list = new List<mLocation>();
            list = db.mLocations.Where(u => u.Is_Deleted == false).ToList<mLocation>();
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                list = list.Where(x => x.LocationName.ToLower().Contains(searchValue.ToLower()) ||
                                  x.LocationDesc.ToLower().Contains(searchValue.ToLower()) ||
                                  x.CapacityUnity.ToLower().Contains(searchValue.ToLower()) ||
                                  x.Capacity.ToString().Contains(searchValue.ToString())).ToList<mLocation>();
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
            int totalrows = list.Count;
            int totalrowsafterfiltering = list.Count;
            list = list.Skip(start).Take(length).ToList<mLocation>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateLocation(mLocation location)
        {
            location.SiteCode = "thissite";
            location.CapacityUnity = "wew";
            string gencode = string.Empty;
            string lastgencode = (from u in db.mLocations.ToList()
                                  orderby u.LocationCode descending
                                  select u.LocationCode).FirstOrDefault();
            if (lastgencode == null)
            {
                gencode = "LOC0001";
            }
            else
            {
                gencode = lastgencode.Substring(3);
                int upId = Convert.ToInt32(gencode.TrimStart(new char[] { '0' })) + 1;
                gencode = string.Format("LOC{0}", upId.ToString().PadLeft(4, '0'));
            }
            
            location.LocationCode = gencode;
            location.Is_Deleted = false;
            location.Create_Date = DateTime.Now;
            location.Create_User = user.UserID;
            location.Update_Date = DateTime.Now;
            location.Update_User = user.UserID;


            mLocation checker = (from c in db.mLocations
                             where c.LocationName == location.LocationName
                             && c.LocationDesc == location.LocationDesc
                             && c.SiteCode == location.SiteCode
                             && c.Is_Deleted == false
                                 select c).FirstOrDefault();

            if (checker == null)
            {
                try
                {
                    db.mLocations.Add(location);
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

        public ActionResult EditLocation(mLocation location)
        {
            location.SiteCode = "thissite";
            location.CapacityUnity = "wew";
            try
            {
                mLocation Location = new mLocation();
                Location = (from u in db.mLocations.ToList()
                         where u.LocationCode == location.LocationCode
                         select u).FirstOrDefault();

                Location.LocationName = location.LocationName;
                Location.LocationDesc = location.LocationDesc;
                Location.Update_User = "up";
                Location.Update_Date = DateTime.Now;

                mLocation checker = (from c in db.mLocations
                                     where c.LocationName == location.LocationName
                                     && c.LocationDesc == location.LocationDesc
                                     && c.Is_Deleted == false
                                     select c).FirstOrDefault();
                if (checker == null)
                {
                    db.Entry(Location).State = EntityState.Modified;
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

        public ActionResult DeleteLocation(string LocationCode)
        {
            mLocation location = new mLocation();
            location = (from u in db.mLocations.ToList()
                    where u.LocationCode == LocationCode
                    select u).FirstOrDefault();
            location.Is_Deleted = true;
            location.Update_Date = DateTime.Now;
            location.Update_User = user.UserID;
            db.Entry(location).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }
    }
}