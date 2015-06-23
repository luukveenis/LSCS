using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LSCS.Web.Controllers
{
    [RoutePrefix("checklists")]
    public class ChecklistController : Controller
    {
        [Route]
        [HttpGet]
        [AllowAnonymous]
        // GET: Checklist
        public ActionResult Index()
        {
            return View();
        }
    }
}