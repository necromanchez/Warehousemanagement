using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WMSv2.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    [SessionExpireController]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
     
    }
}