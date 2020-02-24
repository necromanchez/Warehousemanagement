using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMSv2.Controllers;

namespace WMSv2.Areas.MasterManagement.Controllers
{
    [SessionExpireController]
    public class LocationSubController : Controller
    {
        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];
        // GET: MasterManagement/LocationSub
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetLocationList(string LocationCode)
        {
            string LocationName = string.Empty;
            List<mLocationSub> list = new List<mLocationSub>();
            list = (from c in db.mLocationSubs where c.Is_Deleted == false && c.LocationCode == LocationCode select c).ToList();
            LocationName = (from d in db.mLocations where d.LocationCode == LocationCode select d.LocationName).FirstOrDefault();
         
            return Json(new { list = list, LocationName= LocationName }, JsonRequestBehavior.AllowGet);
        }

    

        public ActionResult CreateSubLocation(mLocationSub sublocation)
        {
            sublocation.SiteCode = "thissite";
            string gencode = string.Empty;
            string lastgencode = (from u in db.mLocationSubs.ToList()
                                  orderby u.LocationSubCode descending
                                  select u.LocationSubCode).FirstOrDefault();
            if (lastgencode == null)
            {
                gencode = "SLC0001";
            }
            else
            {
                gencode = lastgencode.Substring(3);
                int upId = Convert.ToInt32(gencode.TrimStart(new char[] { '0' })) + 1;
                gencode = string.Format("SLC{0}", upId.ToString().PadLeft(4, '0'));
            }

            sublocation.LocationSubCode = gencode;
            sublocation.Is_Deleted = false;
            sublocation.Create_Date = DateTime.Now;
            sublocation.Create_User = user.UserID;
            sublocation.Update_Date = DateTime.Now;
            sublocation.Update_User = user.UserID;


            mLocationSub checker = (from c in db.mLocationSubs
                                    where c.LocationSubName == sublocation.LocationSubName
                                    && c.LocationCode == sublocation.LocationCode
                                 select c).FirstOrDefault();

            if (checker == null)
            {
                try
                {
                    db.mLocationSubs.Add(sublocation);
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

        public ActionResult EditSubLocation(mLocationSub sublocation)
        {
            sublocation.SiteCode = "thissite";
            try
            {
                mLocationSub Location = new mLocationSub();
                Location = (from u in db.mLocationSubs.ToList()
                            where u.LocationCode == sublocation.LocationCode
                            && u.LocationSubCode == sublocation.LocationSubCode
                            select u).FirstOrDefault();

                Location.LocationSubName = sublocation.LocationSubName;
                Location.Capacity = sublocation.Capacity;
                Location.Update_User = "up";
                Location.Update_Date = DateTime.Now;

                mLocationSub checker = (from c in db.mLocationSubs
                                     where c.LocationSubName == sublocation.LocationSubName
                                     && c.Capacity == sublocation.Capacity
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

        public ActionResult DeleteSubLocation(string SubLocationCode)
        {
            mLocationSub location = new mLocationSub();
            location = (from u in db.mLocationSubs.ToList()
                        where u.LocationSubCode == SubLocationCode
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