using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Mvc;
using LSCS.Models;
using Newtonsoft.Json;

namespace LSCS.Web.Controllers
{
    [RoutePrefix("checklists")]
    public class ChecklistsController : Controller
    {
        [Route]
        [Route("~/", Name = "default")]
        [HttpGet]
        [AllowAnonymous]
        // GET: Checklist
        public ActionResult Index()
        {
            return View();
        }

        [Route("new")]
        public ActionResult New()
        {
            return View();
        }

        [Route("edit/{id}")]
        public ActionResult Edit(Guid id)
        {
            var client = new HttpClient();
            var response = client.GetAsync(new Uri("http://localhost:1059/api/checklists/" + id)).Result;
            var checklist = JsonConvert.DeserializeObject<ChecklistDto>(response.Content.ReadAsStringAsync().Result);

            return View(checklist);
        }
    }
}