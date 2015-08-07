using System;
using System.Net.Http;
using System.Web.Mvc;
using LSCS.Models;
using Newtonsoft.Json;

namespace LSCS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("checklists")]
    public class ChecklistsController : Controller
    {
        [Route]
        [Route("~/", Name = "default")]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Route("new")]
        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        [Route("edit/{id}")]
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var client = new HttpClient();
            var response = client.GetAsync(new Uri("http://localhost:1059/api/checklists/" + id)).Result;
            var checklist = JsonConvert.DeserializeObject<ChecklistDto>(response.Content.ReadAsStringAsync().Result);

            return View(checklist);
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult Show(Guid id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}