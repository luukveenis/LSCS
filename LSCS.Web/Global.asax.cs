using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using LSCS.Web.Authentication;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LSCS.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapMvcAttributeRoutes();
            RegisterBundles(BundleTable.Bundles);
            CreateUserRoles();
        }

        private static void CreateUserRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(UserDbContext.Create()));
            roleManager.Create(new IdentityRole("Administrator"));
            roleManager.Create(new IdentityRole("Surveyor"));
        }

        private static void RegisterBundles(BundleCollection bundles)
        {
            // Create static asset bundles here
        }
    }
}
