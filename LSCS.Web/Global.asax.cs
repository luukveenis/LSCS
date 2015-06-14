using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LSCS.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapMvcAttributeRoutes();
        }
    }
}
