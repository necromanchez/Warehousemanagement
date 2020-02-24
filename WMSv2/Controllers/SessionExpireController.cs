using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WMSv2.Controllers
{
    public class SessionExpireController : ActionFilterAttribute
    {
        // GET: SessionExpire
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            mUser user = (mUser)HttpContext.Current.Session["user"];
            if (user == null)
            {
                filterContext.Result = new RedirectResult("/");
                return;
            }
            base.OnActionExecuting(filterContext);
        }

        public static class TypeHelper
        {
            public static object GetPropertyValue(object obj, string name)
            {
                return obj == null ? null : obj.GetType()
                .GetProperty(name)
                .GetValue(obj, null);
            }
        }
     
    }
}