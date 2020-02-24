using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static WMSv2.Controllers.SessionExpireController;
using WMSv2.Controllers;

namespace WMSv2.Areas.MasterManagement.Controllers
{
    [SessionExpireController]
    public class UserMasterController : Controller
    {
        WMSv2Entities db = new WMSv2Entities();
        // GET: MasterManagement/UserMaster
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUserList()
        {
            //List<mUser> list = new List<mUser>();
            //list = (from c in db.mUsers where c.Is_Deleted == false select c).ToList();
            //return Json(new { list = list }, JsonRequestBehavior.AllowGet);
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string input = Request["input[value]"];

            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
        
            List<mUser> list = new List<mUser>();
            //string UserIdFilter = Request["column[0][search][value]"];

            list = db.mUsers.Where(u => u.Is_Deleted == false).ToList<mUser>();
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                list = list.
                    Where(x => x.FirstName.ToLower().Contains(searchValue.ToLower()) || x.MiddleName.ToLower().Contains(searchValue.ToLower()) || x.LastName.ToLower().Contains(searchValue.ToLower()) || x.UserID.ToLower().Contains(searchValue.ToLower())).ToList<mUser>();
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
            list = list.Skip(start).Take(length).ToList<mUser>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

        }

        public string EncodePasswordMd5(string pass) //Encrypt using MD5    
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)    
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string    
            return BitConverter.ToString(encodedBytes);
        }

        public ActionResult CreateUser(mUser user)
        {
            user.LastDateLoggedIn = DateTime.Now;
            user.Customer_Flag = true;
            user.Is_Deleted = false;
            user.Create_Date = DateTime.Now;
            user.Create_User = user.UserID;
            user.Update_Date = DateTime.Now;
            user.Update_User = user.UserID;
            user.Password = EncodePasswordMd5(user.Password);

            mSite checker = (from c in db.mSites
                             where c.SiteName == user.UserID && c.Is_Deleted == false 
                             select c).FirstOrDefault();

            if (checker == null)
            {
                try
                {
                    db.mUsers.Add(user);
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

        public ActionResult EditUser(mUser user)
        {
            try
            {
                mUser users = new mUser();
                users = (from u in db.mUsers.ToList()
                         where u.UserID == user.UserID
                         select u).FirstOrDefault();
                users.FirstName = user.FirstName;
                users.MiddleName = user.MiddleName;
                users.LastName = user.LastName;

                users.Approved = user.Approved;
                users.Locked = user.Locked;
                users.SuperUser = user.SuperUser;
                users.Reversal = user.Reversal;
                users.IsManager = user.IsManager;

                user.Update_User = "up";
                user.Update_Date = DateTime.Now;

                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();


            }
            catch (Exception err) { }

            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteUser(string UserID)
        {
            mUser user = new mUser();
            user = (from u in db.mUsers.ToList()
                    where u.UserID == UserID
                    select u).FirstOrDefault();
            user.Is_Deleted = true;
            user.Update_Date = DateTime.Now;
            user.Update_User = user.UserID;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }
    }
}