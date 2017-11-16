using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BB.Web.Startup))]
namespace BB.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
