using System.Web.Mvc;

namespace LSCS.Web.Controllers
{
    [RoutePrefix("login")]
    public class LoginController : Controller
    {
        [Route]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string redirectUrl)
        {
            return View();
        }

        [Route]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login()
        {
            return null;
        }
    }
}