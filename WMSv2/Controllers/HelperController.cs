using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WMSv2.Controllers
{
    public class HelperController : Controller
    {
        WMSv2Entities db = new WMSv2Entities();
        // GET: Helper
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dropdown_CodeBack()
        {
            ArrayList list = new ArrayList();
            List<mCodeBack> codelist = (from u in db.mCodeBacks.ToList()
                                        where u.Code != "OWN" && u.Code != "SUP" && u.Code != "BUY"
                                        && u.Code != "SHT" && u.Code != "BLT" && u.Code != "SHP" && u.Code != "CON"
                                        orderby u.CodeName ascending
                                        select u).ToList();
            foreach (mCodeBack s in codelist)
            {
                list.Add(new { value = s.Code, text = s.CodeName });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_ClassCusSup()
        {
            ArrayList list = new ArrayList();
            List<mCodeBack> codelist = (from u in db.mCodeBacks.ToList()
                                            //.Where(u => u.Code.Contains("OWN"))
                                        where u.Code == "OWN" || u.Code == "SUP" || u.Code == "BUY"
                                        || u.Code == "SHT" || u.Code == "BLT" || u.Code == "SHP" || u.Code == "CON"
                                        orderby u.CodeName ascending
                                    select u).ToList();
            foreach (mCodeBack s in codelist)
            {
                list.Add(new { value = s.Code, text = s.CodeName });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_ClassOwner()
        {
            ArrayList list = new ArrayList();
            List<mCustomerSupplier> codelist = (from u in db.mCustomerSuppliers.ToList()
                                        .Where(u => u.Class.Contains("OWN") && u.Is_Deleted != true)
                                    orderby u.Name ascending
                                    select u).ToList();
            foreach (mCustomerSupplier s in codelist)
            {
                list.Add(new { value = s.CusSupCode, text = s.Name });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_ClassSupplier()
        {
            ArrayList list = new ArrayList();
            List<mCustomerSupplier> codelist = (from u in db.mCustomerSuppliers.ToList()
                                        .Where(u => u.Class.Contains("SUP") && u.Is_Deleted != true)
                                                orderby u.Name ascending
                                                select u).ToList();
            foreach (mCustomerSupplier s in codelist)
            {
                list.Add(new { value = s.CusSupCode, text = s.Name });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_CodeClassInbound()
        {
            ArrayList list = new ArrayList();
            List<mCode> codelist = (from u in db.mCodes.ToList()
                                        .Where(u => u.Code.Contains("INC") && u.Is_Deleted != true)
                                                orderby u.CodeName ascending
                                                select u).ToList();
            foreach (mCode s in codelist)
            {
                list.Add(new { value = s.Code, text = s.CodeName });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_ClassStock()
        {
            ArrayList list = new ArrayList();
            List<mCode> codelist = (from u in db.mCodes.ToList()
                                        .Where(u => u.Code.Contains("STC") && u.Is_Deleted != true)
                                    orderby u.CodeName ascending
                                    select u).ToList();
            foreach (mCode s in codelist)
            {
                list.Add(new { value = s.Code, text = s.CodeName });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_Items()
        {
            ArrayList list = new ArrayList();
            List<mItem> itemlist = (from u in db.mItems.ToList()
                                    where u.Is_Deleted != true
                                    orderby u.ItemCode ascending
                                    select u).ToList();
            foreach (mItem s in itemlist)
            {
                list.Add(new { value = s.ItemCode, text = s.ItemName });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_Location()
        {
            ArrayList list = new ArrayList();
            List<mLocation> itemlist = (from u in db.mLocations.ToList()
                                    where u.Is_Deleted != true
                                    orderby u.LocationCode ascending
                                    select u).ToList();
            foreach (mLocation s in itemlist)
            {
                list.Add(new { value = s.LocationCode, text = s.LocationName });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_SubLocation(string LocationCode)
        {
            ArrayList list = new ArrayList();
            List<mLocationSub> itemlist = (from u in db.mLocationSubs.ToList()
                                        where u.LocationCode == LocationCode && u.Is_Deleted != true
                                        orderby u.LocationSubCode ascending
                                        select u).ToList();
            foreach (mLocationSub s in itemlist)
            {
                list.Add(new { value = s.LocationSubCode, text = s.LocationSubName });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_Units()
        {
            ArrayList list = new ArrayList();
            List<mCode> unitlist = (from u in db.mCodes.ToList()
                                    where u.Is_Deleted != true && u.Code.Contains("CAP")
                                    orderby u.CodeName ascending
                                    select u).ToList();
            foreach (mCode s in unitlist)
            {
                list.Add(new { value = s.Code, text = s.CodeName });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_CargoClass()
        {
            ArrayList list = new ArrayList();
            List<mCode> unitlist = (from u in db.mCodes.ToList()
                                    where u.Is_Deleted != true && u.Code.Contains("CAC")
                                    orderby u.CodeName ascending
                                    select u).ToList();
            foreach (mCode s in unitlist)
            {
                list.Add(new { value = s.Code, text = s.CodeName });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_Site()
        {
            ArrayList list = new ArrayList();
            List<mSite> siteList = (from u in db.mSites.ToList()
                                    where u.Is_Deleted == false
                                    orderby u.SiteName ascending
                                    select u).ToList();
            foreach (mSite s in siteList)
            {
                list.Add(new { value = s.SiteCode, text = s.SiteName });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_OutboundClass()
        {
            ArrayList list = new ArrayList();
            List<mCode> List = (from u in db.mCodes.ToList()
                                    where u.Is_Deleted != true && u.Code.Contains("OBC")
                                    orderby u.CodeName ascending
                                    select u).ToList();
            foreach (mCode s in List)
            {
                list.Add(new { value = s.Code, text = s.CodeName });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_OutboundClassTransport()
        {
            ArrayList list = new ArrayList();
            List<mCode> List = (from u in db.mCodes.ToList()
                                where u.Is_Deleted != true && u.Code.Contains("OBT")
                                orderby u.CodeName ascending
                                select u).ToList();
            foreach (mCode s in List)
            {
                list.Add(new { value = s.Code, text = s.CodeName });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_ShipToCode()
        {
            ArrayList list = new ArrayList();
            List<mCustomerSupplier> List = (from u in db.mCustomerSuppliers.ToList()
                                where u.Is_Deleted != true && u.Class.Contains("SHT")
                                orderby u.Name ascending
                                select u).ToList();
            foreach (mCustomerSupplier s in List)
            {
                list.Add(new { value = s.CusSupCode, text = s.Name });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_Shipper()
        {
            ArrayList list = new ArrayList();
            List<mCustomerSupplier> List = (from u in db.mCustomerSuppliers.ToList()
                                            where u.Is_Deleted != true && u.Class.Contains("SHP")
                                            orderby u.Name ascending
                                            select u).ToList();
            foreach (mCustomerSupplier s in List)
            {
                list.Add(new { value = s.CusSupCode, text = s.Name });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_Buyer()
        {
            ArrayList list = new ArrayList();
            List<mCustomerSupplier> List = (from u in db.mCustomerSuppliers.ToList()
                                            where u.Is_Deleted != true && u.Class.Contains("BUY")
                                            orderby u.Name ascending
                                            select u).ToList();
            foreach (mCustomerSupplier s in List)
            {
                list.Add(new { value = s.CusSupCode, text = s.Name });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dropdown_ConsigneeCode()
        {
            ArrayList list = new ArrayList();
            List<mCustomerSupplier> List = (from u in db.mCustomerSuppliers.ToList()
                                            where u.Is_Deleted != true && u.Class.Contains("CON")
                                            orderby u.Name ascending
                                            select u).ToList();
            foreach (mCustomerSupplier s in List)
            {
                list.Add(new { value = s.CusSupCode, text = s.Name });
            }
            return Json(new { data = list, ok = true }, JsonRequestBehavior.AllowGet);
        }
    }
}