using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WMSv2.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        WMSv2Entities db = new WMSv2Entities();
        public ActionResult Index()
        {
            return View();
        }

        public class LoginViewModel
        {
            [Required]
            [Display(Name = "User name")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = checkAccount(model.UserName, EncodePasswordMd5(model.Password));
                if (user != null)
                {
                   
                    System.Web.HttpContext.Current.Session["user"] = user;
                    FormsAuthentication.SetAuthCookie(user.UserID, false);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                             user.UserID,
                             DateTime.Now,
                             DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                             false,
                             user.ToString());

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);

                    string userId = System.Web.HttpContext.Current.User.Identity.Name;
                    
                    return Json(new { re = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.Failed = "Invalid Username or Password";
                    return View();
                }
            }
            else
            {
                // If we got this far, something failed, redisplay form
                return View(model);
            }
         
        }

        public ActionResult SetSite(string Sitecode)
        {
            System.Web.HttpContext.Current.Session["site"] = Sitecode;
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        public mUser checkAccount(string username, string password)
        {
            mUser user = new mUser();
            user = (from c in db.mUsers where c.UserID == username && c.Password == password select c).FirstOrDefault();
            return user;
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

        public ActionResult Logout()
        {
            System.Web.HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            System.Web.HttpContext.Current.Response.AddHeader("Expires", "0");
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }



    }
}