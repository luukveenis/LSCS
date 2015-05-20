using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LSCS.Web.Startup))]
namespace LSCS.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
