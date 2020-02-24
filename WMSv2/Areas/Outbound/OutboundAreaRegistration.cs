using System.Web.Mvc;

namespace WMSv2.Areas.Outbound
{
    public class OutboundAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Outbound";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Outbound_default",
                "Outbound/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}