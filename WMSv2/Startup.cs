using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WMSv2.Startup))]
namespace WMSv2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
