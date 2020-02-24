using System;
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
    public class ItemMasterController : Controller
    {
        // GET: MasterManagement/ItemMaster
        WMSv2Entities db = new WMSv2Entities();
        mUser user = (mUser)System.Web.HttpContext.Current.Session["user"];
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetItemList()
        {
            //List<mItem> list = new List<mItem>();
            //list = (from c in db.mItems where c.Is_Deleted == false select c).ToList();
            //return Json(new { list = list }, JsonRequestBehavior.AllowGet);
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<mItem> list = new List<mItem>();

            list = db.mItems.Where(u => u.Is_Deleted == false).ToList<mItem>();

            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                list = list.Where(x => x.ItemName.ToLower().Contains(searchValue.ToLower()) || 
                                  x.Description.ToLower().Contains(searchValue.ToLower()) || 
                                  x.CusSupCode.ToLower().Contains(searchValue.ToLower()) || 
                                  x.CargoClass.ToLower().Contains(searchValue.ToLower())||
                                  x.BaseMeasurement.ToLower().Contains(searchValue.ToLower())).ToList<mItem>();
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
            list = list.Skip(start).Take(length).ToList<mItem>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateItem(mItem item)
        {
            item.SiteCode = System.Web.HttpContext.Current.Session["user"].ToString();
            string gencode = string.Empty;
            string lastgencode = (from u in db.mItems.ToList()
                                  orderby u.ItemCode descending
                                  select u.ItemCode).FirstOrDefault();
            if (lastgencode == null)
            {
                gencode = "ITM0001";
            }
            else
            {
                gencode = lastgencode.Substring(3);
                int upId = Convert.ToInt32(gencode.TrimStart(new char[] { '0' })) + 1;
                gencode = string.Format("ITM{0}", upId.ToString().PadLeft(4, '0'));
            }

            item.ItemCode = gencode;
            item.Is_Deleted = false;
            item.Create_Date = DateTime.Now;
            item.Create_User = user.UserID;
            item.Update_Date = DateTime.Now;
            item.Update_User = user.UserID;
            
            mItem checker = (from c in db.mItems
                                         where c.ItemName == item.ItemName
                                         && c.Description == item.Description
                                         && c.CusSupCode == item.CusSupCode
                                         && c.CargoClass == item.CargoClass
                                         && c.BaseMeasurement == item.BaseMeasurement
                                         && c.SiteCode == item.SiteCode
                                         && c.Is_Deleted == false
                                         select c).FirstOrDefault();

            if (checker == null)
            {
                try
                {
                    db.mItems.Add(item);
                    db.SaveChanges();

                    mItemPrice itemprice = new mItemPrice();
                 
                    itemprice.ItemCode = gencode;
                    itemprice.Is_Deleted = false;
                    itemprice.Create_Date = DateTime.Now;
                    itemprice.Create_User = user.UserID;
                    itemprice.Update_Date = DateTime.Now;
                    itemprice.Update_User = user.UserID;
                    db.mItemPrices.Add(itemprice);
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

        public ActionResult EditItem(mItem itemdata)
        {
            try
            {
                mItem item = new mItem();
                item = (from u in db.mItems.ToList()
                          where u.ItemCode == itemdata.ItemCode
                          select u).FirstOrDefault();

                item.ItemName = itemdata.ItemName;
                item.Description = itemdata.Description;
                item.CusSupCode = itemdata.CusSupCode;
                item.CargoClass = itemdata.CargoClass;
                item.BaseMeasurement = itemdata.BaseMeasurement;
   
                item.Update_User = "up";
                item.Update_Date = DateTime.Now;

                mItem checker = (from c in db.mItems
                                 where c.ItemName == item.ItemName
                                 && c.Description == item.Description
                                 && c.CusSupCode == item.CusSupCode
                                 && c.CargoClass == item.CargoClass
                                 && c.BaseMeasurement == item.BaseMeasurement
                                 && c.SiteCode == item.SiteCode
                                 select c).FirstOrDefault();
                if (checker == null)
                {
                    db.Entry(item).State = EntityState.Modified;
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

        public ActionResult DeleteItem(string ItemCode)
        {
            mItem item = new mItem();
            item = (from u in db.mItems.ToList()
                      where u.ItemCode == ItemCode
                    select u).FirstOrDefault();
            item.Is_Deleted = true;
            item.Update_Date = DateTime.Now;
            item.Update_User = user.UserID;
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemconList(string ItemCode)
        {
            List<ItemConModel> list = new List<ItemConModel>();
            string itemName = (from d in db.mItems where d.Is_Deleted == false && d.ItemCode == ItemCode select d.ItemName).FirstOrDefault();
            list = (from c in 
                    db.mItemConversions
                    where c.Is_Deleted == false 
                    && c.ItemCode == ItemCode select new ItemConModel
                    {
                        ItemCode = c.ItemCode
                        ,FromUnitType = (from d in db.mCodes where d.Code == c.FromUnitType select d.CodeName).FirstOrDefault()
                        ,FromUnit = c.FromUnit
                        ,ToUnitType = (from d in db.mCodes where d.Code == c.ToUnitType select d.CodeName).FirstOrDefault()
                        ,ToUnit = c.ToUnit

                    }).ToList();
            return Json(new { list = list,itemName = itemName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateItemcon(mItemConversion itemcon)
        {
            itemcon.Is_Deleted = false;
            itemcon.Create_Date = DateTime.Now;
            itemcon.Create_User = user.UserID;
            itemcon.Update_Date = DateTime.Now;
            itemcon.Update_User = user.UserID;
            
                try
                {
                    db.mItemConversions.Add(itemcon);
                    db.SaveChanges();
                }
                catch (Exception err) { }

            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemsupList(string ItemCode)
        {
            List<mItemSupplier> list = new List<mItemSupplier>();
            string itemName = (from d in db.mItems where d.Is_Deleted == false && d.ItemCode == ItemCode select d.ItemName).FirstOrDefault();
            list = (from c in db.mItemSuppliers where c.Is_Deleted == false && c.ItemCode == ItemCode select c).ToList();
            return Json(new { list = list, itemName = itemName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateItemsup(mItemSupplier itemsup)
        {
            itemsup.Is_Deleted = false;
            itemsup.Create_Date = DateTime.Now;
            itemsup.Create_User = user.UserID;
            itemsup.Update_Date = DateTime.Now;
            itemsup.Update_User = user.UserID;

            try
            {
                db.mItemSuppliers.Add(itemsup);
                db.SaveChanges();
            }
            catch (Exception err) { }

            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CreateItemUnit(mItemUnit itemunit, int stockqty, mItemUnit InnerCase, mItemUnit PerUnit)
        {
            mItemUnit checker = (from c in db.mItemUnits where c.ItemCode == itemunit.ItemCode select c).FirstOrDefault();
            if(checker != null)
            {
                List<mItemUnit> del = (from d in db.mItemUnits where d.ItemCode == itemunit.ItemCode select d).ToList();
                foreach(mItemUnit t in del)
                {
                    db.mItemUnits.Remove(t);
                    db.SaveChanges();
                }
                List<mItemCategory> dels = (from d in db.mItemCategories where d.ItemCode == itemunit.ItemCode select d).ToList();
                foreach (mItemCategory t in dels)
                {
                    db.mItemCategories.Remove(t);
                    db.SaveChanges();
                }
            }


            itemunit.Is_Deleted = false;
            itemunit.Create_Date = DateTime.Now;
            itemunit.Create_User = user.UserID;
            itemunit.Update_Date = DateTime.Now;
            itemunit.Update_User = user.UserID;
            itemunit.MaintainingQty = stockqty;

            InnerCase.Is_Deleted = false;
            InnerCase.Create_Date = DateTime.Now;
            InnerCase.Create_User = user.UserID;
            InnerCase.Update_Date = DateTime.Now;
            InnerCase.Update_User = user.UserID;
            InnerCase.MaintainingQty = stockqty;

            PerUnit.Is_Deleted = false;
            PerUnit.Create_Date = DateTime.Now;
            PerUnit.Create_User = user.UserID;
            PerUnit.Update_Date = DateTime.Now;
            PerUnit.Update_User = user.UserID;
            PerUnit.MaintainingQty = stockqty;
            try
            {
                db.mItemUnits.Add(itemunit);
                db.SaveChanges();

                db.mItemUnits.Add(InnerCase);
                db.SaveChanges();

                db.mItemUnits.Add(PerUnit);
                db.SaveChanges();

            }
            catch (Exception err) { }

            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateItemCategory(mItemCategory itemcategory)
        {
            itemcategory.Is_Deleted = false;
            itemcategory.Create_Date = DateTime.Now;
            itemcategory.Create_User = user.UserID;
            itemcategory.Update_Date = DateTime.Now;
            itemcategory.Update_User = user.UserID;

            try
            {
                db.mItemCategories.Add(itemcategory);
                db.SaveChanges();
            }
            catch (Exception err) { }

            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUnits(string ItemCode)
        {
            List<mItemUnit> itemunitlist = (from c in db.mItemUnits where c.ItemCode == ItemCode select c).ToList();
            mItemCategory itemcategory = (from c in db.mItemCategories where c.ItemCode == ItemCode select c).FirstOrDefault();


            return Json(
                new
                    { result = "success"
                    , itemunitlist= itemunitlist
                    , itemcategory = itemcategory
                    },
                    JsonRequestBehavior.AllowGet
                );
        }
    }
}