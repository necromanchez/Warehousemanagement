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
    public class ItemPriceMasterController : Controller
    {
        // GET: MasterManagement/ItemPriceMaster
        WMSv2Entities db = new WMSv2Entities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetItemPriceList()
        {
            //List<ItemPriceModel> list = new List<ItemPriceModel>();
            //list = (from c in db.mItemPrices join d in db.mItems on c.ItemCode equals d.ItemCode where c.Is_Deleted == false select new ItemPriceModel
            //{
            //    ItemCode = c.ItemCode
            //    ,ItemName = d.ItemName
            //    ,Currency = c.Currency
            //    ,UnitPrice = c.UnitPrice.ToString()

            //}).ToList();
            //return Json(new { list = list }, JsonRequestBehavior.AllowGet);

            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<ItemPriceModel> list = new List<ItemPriceModel>();

            list = (from c in db.mItemPrices
                    join d in db.mItems on c.ItemCode equals d.ItemCode
                    where c.Is_Deleted == false
                    select new ItemPriceModel
                    {
                        ItemCode = c.ItemCode,
                        ItemName = d.ItemName,
                        Currency = c.Currency,
                        UnitPrice = c.UnitPrice.ToString()

                    }).ToList();

            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                list = list.Where(x => x.ItemName.ToLower().Contains(searchValue.ToLower()) ||
                                  x.Currency.ToLower().Contains(searchValue.ToLower()) ||
                                  x.UnitPrice.ToLower().Contains(searchValue.ToLower())
                                  ).ToList<ItemPriceModel>();
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
            list = list.Skip(start).Take(length).ToList<ItemPriceModel>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditItemPrice(mItemPrice itemdata)
        {
            try
            {
                mItemPrice item = new mItemPrice();
                item = (from u in db.mItemPrices.ToList()
                        where u.ItemCode == itemdata.ItemCode
                        select u).FirstOrDefault();

                item.Currency = itemdata.Currency;
                item.UnitPrice = itemdata.UnitPrice;
                item.Update_User = "up";
                item.Update_Date = DateTime.Now;
                
               
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception err) { }

            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }
    }
}